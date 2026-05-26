using LibSys.Models;
using LibSys.Services;

namespace LibSys
{
    public partial class CatalogForm : Form
    {
        private readonly IDataQueryService _queryService;

        private List<Author> _filterAuthors = new();
        private List<Publisher> _filterPublishers = new();
        private List<int> _filterYears = new();
        private List<Tag> _filterTags = new();

        private readonly HashSet<int> _checkedAuthorIds = new();
        private readonly HashSet<int> _checkedPublisherIds = new();
        private readonly HashSet<int> _checkedYears = new();
        private readonly HashSet<int> _checkedTagIds = new();

        public CatalogForm()
        {
            InitializeComponent();
            _queryService = new DataQueryService();

            cmbSort.Items.AddRange(
                "Назва: А-Я",
                "Назва: Я-А",
                "Видавництво: А-Я",
                "Видавництво: Я-А",
                "Рік: Новіші",
                "Рік: Старіші"
            );
            cmbSort.SelectedIndex = 0;
            cmbSort.SelectedIndexChanged += cmbSort_SelectedIndexChanged;

            clbAuthorsFilter.ItemCheck += ClbAuthorsFilter_ItemCheck;
            clbPublishersFilter.ItemCheck += ClbPublishersFilter_ItemCheck;
            clbYearsFilter.ItemCheck += ClbYearsFilter_ItemCheck;
            clbTagsFilter.ItemCheck += ClbTagsFilter_ItemCheck;

            txtSearch.TextChanged += txtSearch_TextChanged;
            txtSearchAuthorFilter.TextChanged += txtSearchAuthorFilter_TextChanged;
            txtSearchPublisherFilter.TextChanged += txtSearchPublisherFilter_TextChanged;
            txtSearchYearFilter.TextChanged += txtSearchYearFilter_TextChanged;
            txtSearchTagFilter.TextChanged += txtSearchTagFilter_TextChanged;

            btnClearAuthorFilter.Click += btnClearAuthorFilter_Click;
            btnClearPublisherFilter.Click += btnClearPublisherFilter_Click;
            btnClearYearFilter.Click += btnClearYearFilter_Click;
            btnClearTagFilter.Click += btnClearTagFilter_Click;

            btnAddBook.Click += btnAddBook_Click;
            btnEditBook.Click += btnEditBook_Click;

            dgvBooks.SelectionChanged += DgvBooks_SelectionChanged;

            this.Load += CatalogForm_Load;
        }

        private async void CatalogForm_Load(object? sender, EventArgs e) => await FullRefresh();

        public async Task FullRefresh()
        {
            await LoadFiltersFromDbAsync();
            UpdateFilterButtonColors();
            await RefreshDataAsync();
        }

        public async Task LoadFiltersFromDbAsync()
        {
            _filterAuthors = await _queryService.GetFilterAuthorsAsync();
            _filterPublishers = await _queryService.GetFilterPublishersAsync();
            _filterYears = await _queryService.GetFilterYearsAsync();
            _filterTags = await _queryService.GetFilterTagsAsync();

            ApplyAuthorsListFilter();
            ApplyPublishersListFilter();
            ApplyYearsListFilter();
            ApplyTagsListFilter();
        }

        public void RefreshData()
        {
            _ = RefreshDataAsync();
        }

        private async Task RefreshDataAsync()
        {
            if (!this.IsHandleCreated) return;

            var sortOption = (CatalogSortOption)cmbSort.SelectedIndex;

            var data = await _queryService.GetCatalogAsync(
                txtSearch.Text,
                sortOption,
                _checkedAuthorIds,
                _checkedPublisherIds,
                _checkedYears,
                _checkedTagIds,
                false
            );

            dgvBooks.DataSource = data;

            if (dgvBooks.Columns["ID"] != null) dgvBooks.Columns["ID"].Visible = false;

            if (dgvBooks.SelectedRows.Count > 0)
            {
                await DgvBooks_SelectionChangedAsync(null, EventArgs.Empty);
            }
        }

        private void UpdateFilterButtonColors()
        {
            if (_checkedAuthorIds.Count > 0)
            {
                btnClearAuthorFilter.ForeColor = UIColors.Error;
                btnClearAuthorFilter.FlatAppearance.BorderColor = UIColors.Error;
            }
            else
            {
                btnClearAuthorFilter.ForeColor = UIColors.TextMuted;
                btnClearAuthorFilter.FlatAppearance.BorderColor = Color.FromArgb(220, 222, 226);
            }

            if (_checkedPublisherIds.Count > 0)
            {
                btnClearPublisherFilter.ForeColor = UIColors.Error;
                btnClearPublisherFilter.FlatAppearance.BorderColor = UIColors.Error;
            }
            else
            {
                btnClearPublisherFilter.ForeColor = UIColors.TextMuted;
                btnClearPublisherFilter.FlatAppearance.BorderColor = Color.FromArgb(220, 222, 226);
            }

            if (_checkedYears.Count > 0)
            {
                btnClearYearFilter.ForeColor = UIColors.Error;
                btnClearYearFilter.FlatAppearance.BorderColor = UIColors.Error;
            }
            else
            {
                btnClearYearFilter.ForeColor = UIColors.TextMuted;
                btnClearYearFilter.FlatAppearance.BorderColor = Color.FromArgb(220, 222, 226);
            }

            if (_checkedTagIds.Count > 0)
            {
                btnClearTagFilter.ForeColor = UIColors.Error;
                btnClearTagFilter.FlatAppearance.BorderColor = UIColors.Error;
            }
            else
            {
                btnClearTagFilter.ForeColor = UIColors.TextMuted;
                btnClearTagFilter.FlatAppearance.BorderColor = Color.FromArgb(220, 222, 226);
            }
        }

        private void DgvBooks_SelectionChanged(object? sender, EventArgs e)
        {
            _ = DgvBooks_SelectionChangedAsync(sender, e);
        }

        private async Task DgvBooks_SelectionChangedAsync(object? sender, EventArgs e)
        {
            if (dgvBooks.SelectedRows.Count == 0)
            {
                pbCover.Image = null;
                lblNoCover.Visible = true;
                lblCopiesInfo.Text = "";
                return;
            }

            int bookId = (int)dgvBooks.SelectedRows[0].Cells["ID"].Value;

            var coverData = await _queryService.GetCoverImageAsync(bookId);
            if (coverData != null && coverData.Length > 0)
            {
                using var ms = new MemoryStream(coverData);
                pbCover.Image = Image.FromStream(ms);
                lblNoCover.Visible = false;
            }
            else
            {
                pbCover.Image = null;
                lblNoCover.Visible = true;
            }

            var (totalCopies, availableCopies) = await _queryService.GetCopiesCountAsync(bookId);

            if (totalCopies == 0)
            {
                lblCopiesInfo.Text = "⛔ Немає в інвентарі";
                lblCopiesInfo.ForeColor = UIColors.Error;
            }
            else if (availableCopies == 0)
            {
                lblCopiesInfo.Text = $"📚 Всього: {totalCopies}  |  🔴 Доступно: 0 (Всі на руках)";
                lblCopiesInfo.ForeColor = UIColors.Warning;
            }
            else
            {
                lblCopiesInfo.Text = $"📚 Всього: {totalCopies}  |  🟢 Доступно: {availableCopies}";
                lblCopiesInfo.ForeColor = UIColors.Success;
            }
        }

        private void ApplyAuthorsListFilter()
        {
            clbAuthorsFilter.Items.Clear();
            string search = txtSearchAuthorFilter.Text.Trim().ToLower();
            var filtered = _filterAuthors.AsEnumerable();

            if (!string.IsNullOrEmpty(search))
            {
                var terms = search.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                filtered = filtered.Where(a => terms.All(term =>
                    (a.LastName != null && a.LastName.ToLower().Contains(term)) ||
                    (a.FirstName != null && a.FirstName.ToLower().Contains(term)) ||
                    (a.MiddleName != null && a.MiddleName.ToLower().Contains(term)) ||
                    (a.Pseudonym != null && a.Pseudonym.ToLower().Contains(term))
                ));
            }

            foreach (var author in filtered)
            {
                string pib = string.Join(" ",
                    new[] { author.LastName, author.FirstName, author.MiddleName }.Where(s =>
                        !string.IsNullOrWhiteSpace(s)));
                string displayName = !string.IsNullOrWhiteSpace(author.Pseudonym)
                    ? (!string.IsNullOrWhiteSpace(pib) ? $"{pib} ({author.Pseudonym})" : author.Pseudonym)
                    : pib;
                var item = new ListItem { Id = author.Id, Name = displayName };
                clbAuthorsFilter.Items.Add(item, _checkedAuthorIds.Contains(author.Id));
            }
        }

        private void ApplyPublishersListFilter()
        {
            clbPublishersFilter.Items.Clear();
            string search = txtSearchPublisherFilter.Text.Trim().ToLower();
            var filtered = _filterPublishers.AsEnumerable();
            if (!string.IsNullOrEmpty(search))
            {
                var terms = search.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                filtered = filtered.Where(p => terms.All(term => p.Name.ToLower().Contains(term)));
            }

            foreach (var pub in filtered)
                clbPublishersFilter.Items.Add(new ListItem { Id = pub.Id, Name = pub.Name },
                    _checkedPublisherIds.Contains(pub.Id));
        }

        private void ApplyYearsListFilter()
        {
            clbYearsFilter.Items.Clear();
            string search = txtSearchYearFilter.Text.Trim();
            var filtered = _filterYears.AsEnumerable();
            if (!string.IsNullOrEmpty(search)) filtered = filtered.Where(y => y.ToString().Contains(search));
            foreach (var year in filtered)
                clbYearsFilter.Items.Add(new ListItem { Id = year, Name = year.ToString() },
                    _checkedYears.Contains(year));
        }

        private void ApplyTagsListFilter()
        {
            clbTagsFilter.Items.Clear();
            string search = txtSearchTagFilter.Text.Trim().ToLower();
            var filtered = _filterTags.AsEnumerable();
            if (!string.IsNullOrEmpty(search))
            {
                var terms = search.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                filtered = filtered.Where(t => terms.All(term => t.Name.ToLower().Contains(term)));
            }

            foreach (var tag in filtered)
                clbTagsFilter.Items.Add(new ListItem { Id = tag.Id, Name = tag.Name }, _checkedTagIds.Contains(tag.Id));
        }

        private async void btnClearAuthorFilter_Click(object? sender, EventArgs e)
        {
            _checkedAuthorIds.Clear();
            txtSearchAuthorFilter.TextChanged -= txtSearchAuthorFilter_TextChanged;
            txtSearchAuthorFilter.Clear();
            txtSearchAuthorFilter.TextChanged += txtSearchAuthorFilter_TextChanged;
            ApplyAuthorsListFilter();
            UpdateFilterButtonColors();
            await RefreshDataAsync();
        }

        private async void btnClearPublisherFilter_Click(object? sender, EventArgs e)
        {
            _checkedPublisherIds.Clear();
            txtSearchPublisherFilter.TextChanged -= txtSearchPublisherFilter_TextChanged;
            txtSearchPublisherFilter.Clear();
            txtSearchPublisherFilter.TextChanged += txtSearchPublisherFilter_TextChanged;
            ApplyPublishersListFilter();
            UpdateFilterButtonColors();
            await RefreshDataAsync();
        }

        private async void btnClearYearFilter_Click(object? sender, EventArgs e)
        {
            _checkedYears.Clear();
            txtSearchYearFilter.TextChanged -= txtSearchYearFilter_TextChanged;
            txtSearchYearFilter.Clear();
            txtSearchYearFilter.TextChanged += txtSearchYearFilter_TextChanged;
            ApplyYearsListFilter();
            UpdateFilterButtonColors();
            await RefreshDataAsync();
        }

        private async void btnClearTagFilter_Click(object? sender, EventArgs e)
        {
            _checkedTagIds.Clear();
            txtSearchTagFilter.TextChanged -= txtSearchTagFilter_TextChanged;
            txtSearchTagFilter.Clear();
            txtSearchTagFilter.TextChanged += txtSearchTagFilter_TextChanged;
            ApplyTagsListFilter();
            UpdateFilterButtonColors();
            await RefreshDataAsync();
        }

        private async void ClbAuthorsFilter_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (clbAuthorsFilter.Items[e.Index] is ListItem item)
            {
                if (e.NewValue == CheckState.Checked) _checkedAuthorIds.Add(item.Id);
                else _checkedAuthorIds.Remove(item.Id);
                UpdateFilterButtonColors();
                await RefreshDataAsync();
            }
        }

        private async void ClbPublishersFilter_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (clbPublishersFilter.Items[e.Index] is ListItem item)
            {
                if (e.NewValue == CheckState.Checked) _checkedPublisherIds.Add(item.Id);
                else _checkedPublisherIds.Remove(item.Id);
                UpdateFilterButtonColors();
                await RefreshDataAsync();
            }
        }

        private async void ClbYearsFilter_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (clbYearsFilter.Items[e.Index] is ListItem item)
            {
                if (e.NewValue == CheckState.Checked) _checkedYears.Add(item.Id);
                else _checkedYears.Remove(item.Id);
                UpdateFilterButtonColors();
                await RefreshDataAsync();
            }
        }

        private async void ClbTagsFilter_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (clbTagsFilter.Items[e.Index] is ListItem item)
            {
                if (e.NewValue == CheckState.Checked) _checkedTagIds.Add(item.Id);
                else _checkedTagIds.Remove(item.Id);
                UpdateFilterButtonColors();
                await RefreshDataAsync();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e) => RefreshData();
        private void cmbSort_SelectedIndexChanged(object sender, EventArgs e) => RefreshData();
        private void txtSearchAuthorFilter_TextChanged(object sender, EventArgs e) => ApplyAuthorsListFilter();
        private void txtSearchPublisherFilter_TextChanged(object sender, EventArgs e) => ApplyPublishersListFilter();
        private void txtSearchYearFilter_TextChanged(object sender, EventArgs e) => ApplyYearsListFilter();
        private void txtSearchTagFilter_TextChanged(object sender, EventArgs e) => ApplyTagsListFilter();

        private void btnAddBook_Click(object sender, EventArgs e)
        {
            var form = MainForm.Instance.GetCachedForm<BookEditForm>();
            form.SetMode(null);
            MainForm.Instance.ShowForm(form, "Додавання нової книги");
        }

        private void btnEditBook_Click(object sender, EventArgs e)
        {
            if (dgvBooks.SelectedRows.Count > 0)
            {
                int bookId = (int)dgvBooks.SelectedRows[0].Cells["ID"].Value;
                var form = MainForm.Instance.GetCachedForm<BookEditForm>();
                form.SetMode(bookId);
                MainForm.Instance.ShowForm(form, "Редагування книги");
            }
        }

        private void btnAddCopy_Click(object sender, EventArgs e)
        {
            if (dgvBooks.SelectedRows.Count > 0)
            {
                int bookId = (int)dgvBooks.SelectedRows[0].Cells["ID"].Value;
                var form = MainForm.Instance.GetCachedForm<AddBookCopyForm>();
                form.PrepareForm(bookId);
                MainForm.Instance.ShowForm(form, "Додавання примірників");
            }
            else
            {
                MessageBox.Show("Оберіть книгу з таблиці.", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}