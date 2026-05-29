using LibSys.Models;
using LibSys.Services;

namespace LibSys
{
    public partial class BookEditForm : Form
    {
        private int? _bookId;
        private byte[]? _coverImageBytes;

        private List<Publisher> _allPublishers = new();
        private List<Author> _allAuthors = new();
        private List<Tag> _allTags = new();

        private readonly HashSet<int> _checkedAuthorIds = new();
        private readonly HashSet<int> _checkedTagIds = new();
        private int? _selectedPublisherId;

        private readonly IDataQueryService _queryService;
        private readonly IBookEditService _bookEditService;

        public BookEditForm()
        {
            InitializeComponent();
            _queryService = new DataQueryService();
            _bookEditService = new BookEditService();

            clbAuthors.ItemCheck += ClbAuthors_ItemCheck;
            clbTags.ItemCheck += ClbTags_ItemCheck;
            clbPublishers.ItemCheck += ClbPublishers_ItemCheck;
        }

        private void ClbPublishers_ItemCheck(object? sender, ItemCheckEventArgs e)
        {
            if (clbPublishers.Items[e.Index] is ListItem item)
            {
                if (e.NewValue == CheckState.Checked)
                {
                    _selectedPublisherId = item.Id;
                    for (int i = 0; i < clbPublishers.Items.Count; i++)
                    {
                        if (i != e.Index) clbPublishers.SetItemChecked(i, false);
                    }
                }
                else
                {
                    if (_selectedPublisherId == item.Id) _selectedPublisherId = null;
                }
            }
        }

        private void ClbAuthors_ItemCheck(object? sender, ItemCheckEventArgs e)
        {
            if (clbAuthors.Items[e.Index] is ListItem item)
            {
                if (e.NewValue == CheckState.Checked) _checkedAuthorIds.Add(item.Id);
                else _checkedAuthorIds.Remove(item.Id);
            }
        }

        private void ClbTags_ItemCheck(object? sender, ItemCheckEventArgs e)
        {
            if (clbTags.Items[e.Index] is ListItem item)
            {
                if (e.NewValue == CheckState.Checked) _checkedTagIds.Add(item.Id);
                else _checkedTagIds.Remove(item.Id);
            }
        }

        public async void SetMode(int? bookId)
        {
            _bookId = bookId;
            numYear.Maximum = DateTime.Now.Year;

            _checkedAuthorIds.Clear();
            _checkedTagIds.Clear();
            _selectedPublisherId = null;

            txtSearchAuthor.Clear();
            txtSearchPublisher.Clear();
            txtSearchTag.Clear();

            panelAddAuthor.Visible = false;
            panelAddPublisher.Visible = false;
            panelAddTag.Visible = false;

            await LoadDictionariesAsync();

            if (_bookId.HasValue)
            {
                btnSave.Text = "Зберегти зміни";
                await LoadBookDataAsync();
            }
            else
            {
                btnSave.Text = "Створити книгу";
                ClearFields();
            }
        }

        private async Task LoadDictionariesAsync()
        {
            _allPublishers = await _queryService.GetAllPublishersNonArchivedAsync();
            _allAuthors = await _queryService.GetAllAuthorsNonArchivedAsync();
            _allTags = await _queryService.GetAllTagsNonArchivedAsync();

            FilterPublishers();
            FilterAuthors();
            FilterTags();
        }

        private void FilterPublishers()
        {
            string search = txtSearchPublisher.Text.Trim();
            var filtered = _allPublishers.Where(p => p.Name.ContainsAllSearchTerms(search));

            clbPublishers.ItemCheck -= ClbPublishers_ItemCheck;
            clbPublishers.Items.Clear();
            foreach (var p in filtered)
            {
                bool isChecked = _selectedPublisherId.HasValue && _selectedPublisherId.Value == p.Id;
                clbPublishers.Items.Add(new ListItem { Id = p.Id, Name = p.Name }, isChecked);
            }

            clbPublishers.ItemCheck += ClbPublishers_ItemCheck;
        }

        private void FilterAuthors()
        {
            string search = txtSearchAuthor.Text.Trim();
            var filtered = _allAuthors.Where(a =>
                $"{a.LastName} {a.FirstName} {a.MiddleName} {a.Pseudonym}".ContainsAllSearchTerms(search));

            clbAuthors.ItemCheck -= ClbAuthors_ItemCheck;
            clbAuthors.Items.Clear();
            foreach (var a in filtered)
            {
                string display = string.IsNullOrWhiteSpace(a.Pseudonym)
                    ? $"{a.LastName} {a.FirstName}"
                    : (!string.IsNullOrWhiteSpace(a.LastName)
                        ? $"{a.LastName} {a.FirstName} ({a.Pseudonym})"
                        : a.Pseudonym);
                clbAuthors.Items.Add(new ListItem { Id = a.Id, Name = display }, _checkedAuthorIds.Contains(a.Id));
            }

            clbAuthors.ItemCheck += ClbAuthors_ItemCheck;
        }

        private void FilterTags()
        {
            string search = txtSearchTag.Text.Trim();
            var filtered = _allTags.Where(t => t.Name.ContainsAllSearchTerms(search));

            clbTags.ItemCheck -= ClbTags_ItemCheck;
            clbTags.Items.Clear();
            foreach (var t in filtered)
            {
                clbTags.Items.Add(new ListItem { Id = t.Id, Name = t.Name }, _checkedTagIds.Contains(t.Id));
            }

            clbTags.ItemCheck += ClbTags_ItemCheck;
        }

        private void txtSearchPublisher_TextChanged(object sender, EventArgs e) => FilterPublishers();
        private void txtSearchAuthor_TextChanged(object sender, EventArgs e) => FilterAuthors();
        private void txtSearchTag_TextChanged(object sender, EventArgs e) => FilterTags();

        private async Task LoadBookDataAsync()
        {
            var book = await _bookEditService.GetBookWithDetailsAsync(_bookId.Value);
            if (book == null) return;

            txtTitle.Text = book.Title;
            numYear.Value = book.PublicationYear;
            numPrice.Value = book.Price;
            _selectedPublisherId = book.PublisherId;

            _coverImageBytes = book.CoverImage;
            if (_coverImageBytes != null && _coverImageBytes.Length > 0)
            {
                using var ms = new MemoryStream(_coverImageBytes);
                pbCoverEdit.Image = new Bitmap(ms);
            }
            else
            {
                pbCoverEdit.Image = null;
            }

            foreach (var a in book.Authors) _checkedAuthorIds.Add(a.Id);
            foreach (var t in book.Tags) _checkedTagIds.Add(t.Id);

            FilterPublishers();
            FilterAuthors();
            FilterTags();
        }

        private void ClearFields()
        {
            txtTitle.Clear();
            numYear.Value = DateTime.Now.Year;
            numPrice.Value = 100;
            _coverImageBytes = null;
            pbCoverEdit.Image = null;

            _checkedAuthorIds.Clear();
            _checkedTagIds.Clear();
            _selectedPublisherId = null;

            FilterPublishers();
            FilterAuthors();
            FilterTags();
        }

        private void btnUploadCover_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog();
            ofd.Filter = "Зображення|*.jpg;*.jpeg;*.png;*.bmp";
            ofd.Title = "Оберіть обкладинку книги";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                _coverImageBytes = File.ReadAllBytes(ofd.FileName);
                using var ms = new MemoryStream(_coverImageBytes);
                pbCoverEdit.Image = new Bitmap(ms);
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text) || !_selectedPublisherId.HasValue ||
                _checkedAuthorIds.Count == 0)
            {
                MessageBox.Show("Назва, Видавництво та хоча б один Автор - обов'язкові!", "Увага", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var book = new Book
            {
                Id = _bookId ?? 0,
                Title = txtTitle.Text.Trim(),
                PublicationYear = (int)numYear.Value,
                Price = numPrice.Value
            };

            await _bookEditService.SaveBookAsync(book, _selectedPublisherId.Value, _checkedAuthorIds.ToList(), _checkedTagIds.ToList(), _coverImageBytes);

            MessageBox.Show("Книгу успішно збережено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
            GoBack();
        }

        private void btnCancel_Click(object sender, EventArgs e) => GoBack();

        private void GoBack()
        {
            var catalog = MainForm.Instance.GetCachedForm<CatalogForm>();
            catalog.RefreshData();
            MainForm.Instance.ShowForm(catalog, "Каталог книг");
        }

        private void btnQuickAddPublisher_Click(object sender, EventArgs e)
        {
            panelAddPublisher.Visible = true;
            panelAddPublisher.BringToFront();
            txtNewPubName.Clear();
            txtNewPubName.Focus();
        }

        private void btnCancelPub_Click(object sender, EventArgs e) => panelAddPublisher.Visible = false;

        private async void btnSavePub_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNewPubName.Text)) return;

            int newPubId = await _bookEditService.AddPublisherAsync(txtNewPubName.Text.Trim());
            var newPub = new Publisher { Id = newPubId, Name = txtNewPubName.Text.Trim() };
            _allPublishers.Add(newPub);
            _selectedPublisherId = newPubId;
            FilterPublishers();
            panelAddPublisher.Visible = false;
        }

        private void btnQuickAddAuthor_Click(object sender, EventArgs e)
        {
            panelAddAuthor.Visible = true;
            panelAddAuthor.BringToFront();
            txtNewAuthLast.Clear();
            txtNewAuthFirst.Clear();
            txtNewAuthMiddle.Clear();
            txtNewAuthPseudo.Clear();
            txtNewAuthLast.Focus();
        }

        private void btnCancelAuth_Click(object sender, EventArgs e) => panelAddAuthor.Visible = false;

        private async void btnSaveAuth_Click(object sender, EventArgs e)
        {
            bool hasRealName = !string.IsNullOrWhiteSpace(txtNewAuthLast.Text) &&
                               !string.IsNullOrWhiteSpace(txtNewAuthFirst.Text);
            bool hasPseudo = !string.IsNullOrWhiteSpace(txtNewAuthPseudo.Text);

            if (!hasRealName && !hasPseudo)
            {
                MessageBox.Show("Заповніть (Прізвище та Ім'я) І/АБО (Псевдонім)!", "Помилка", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            var author = new Author
            {
                LastName = string.IsNullOrWhiteSpace(txtNewAuthLast.Text) ? null : txtNewAuthLast.Text.Trim(),
                FirstName = string.IsNullOrWhiteSpace(txtNewAuthFirst.Text) ? null : txtNewAuthFirst.Text.Trim(),
                MiddleName = string.IsNullOrWhiteSpace(txtNewAuthMiddle.Text) ? null : txtNewAuthMiddle.Text.Trim(),
                Pseudonym = string.IsNullOrWhiteSpace(txtNewAuthPseudo.Text) ? null : txtNewAuthPseudo.Text.Trim()
            };

            int newAuthorId = await _bookEditService.AddAuthorAsync(author);
            author.Id = newAuthorId;
            _allAuthors.Add(author);
            _checkedAuthorIds.Add(author.Id);
            FilterAuthors();
            panelAddAuthor.Visible = false;
        }

        private void btnQuickAddTag_Click(object sender, EventArgs e)
        {
            panelAddTag.Visible = true;
            panelAddTag.BringToFront();
            txtNewTagName.Clear();
            txtNewTagName.Focus();
        }

        private void btnCancelTag_Click(object sender, EventArgs e) => panelAddTag.Visible = false;

        private async void btnSaveTag_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNewTagName.Text)) return;

            int newTagId = await _bookEditService.AddTagAsync(txtNewTagName.Text.Trim());
            var newTag = new Tag { Id = newTagId, Name = txtNewTagName.Text.Trim() };
            _allTags.Add(newTag);
            _checkedTagIds.Add(newTag.Id);
            FilterTags();
            panelAddTag.Visible = false;
        }
    }
}