using LibSys.Models;
using LibSys.Services;

namespace LibSys
{
    public partial class ReaderEditForm : Form
    {
        private int? _readerId;
        private readonly IReaderEditService _readerEditService;

        public ReaderEditForm()
        {
            InitializeComponent();
            _readerEditService = new ReaderEditService();
        }

        public void SetMode(int? readerId)
        {
            _readerId = readerId;

            DateTime maxAllowedDate = DateTime.Today.AddYears(-14);
            dtpBirthDate.MaxDate = maxAllowedDate;

            if (_readerId.HasValue)
            {
                btnCancel.Text = "Скасувати зміни";
                LoadReaderData();
            }
            else
            {
                btnCancel.Text = "Скасувати реєстрацію";
                ClearFields(maxAllowedDate);
            }
        }

        private async void LoadReaderData()
        {
            var reader = await _readerEditService.GetReaderByIdAsync(_readerId.Value);
            if (reader == null) return;

            txtLastName.Text = reader.LastName;
            txtFirstName.Text = reader.FirstName;
            txtMiddleName.Text = reader.MiddleName;
            txtPassport.Text = reader.PassportNumber;
            txtPhone.Text = reader.Phone;
            txtEmail.Text = reader.Email;
            dtpBirthDate.Value = reader.BirthDate.FromUnixTimestamp();
        }

        private void ClearFields(DateTime defaultDate)
        {
            txtLastName.Clear();
            txtFirstName.Clear();
            txtMiddleName.Clear();
            txtPassport.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            dtpBirthDate.Value = defaultDate;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            // 1. Перевірка на заповненість обов'язкових полів
            if (string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtPassport.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Будь ласка, заповніть всі обов'язкові поля, позначені зірочкою (*).", "Увага",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var targetReader = _readerId.HasValue
                ? await _readerEditService.GetReaderByIdAsync(_readerId.Value)
                : new Reader();

            if (targetReader == null && _readerId.HasValue)
            {
                MessageBox.Show("Читача не знайдено!", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Заповнення моделі даними з форми
            targetReader.LastName = txtLastName.Text.Trim();
            targetReader.FirstName = txtFirstName.Text.Trim();
            targetReader.MiddleName = string.IsNullOrWhiteSpace(txtMiddleName.Text) ? null : txtMiddleName.Text.Trim();
            targetReader.PassportNumber = txtPassport.Text.Trim();
            targetReader.Phone = txtPhone.Text.Trim();
            targetReader.Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim();
            targetReader.BirthDate = dtpBirthDate.Value.ToUnixTimestamp();

            // Валідація через сервіс
            var validationResult = await _readerEditService.ValidateReaderDataAsync(targetReader);
            if (!validationResult.IsSuccess)
            {
                MessageBox.Show(validationResult.Message, "Помилка валідації", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Перевірка унікальності паспорта
            if (!await _readerEditService.IsPassportUniqueAsync(targetReader.PassportNumber, _readerId))
            {
                MessageBox.Show("Читач із таким номером паспорта / ID вже зареєстрований!", "Дублікат",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            await _readerEditService.SaveReaderAsync(targetReader);

            MessageBox.Show("Профіль читача збережено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
            GoBack();
        }

        private void btnCancel_Click(object sender, EventArgs e) => GoBack();

        private void GoBack()
        {
            if (_readerId.HasValue)
            {
                var profileForm = MainForm.Instance.GetCachedForm<ReaderProfileForm>();
                profileForm.OpenProfile(_readerId.Value);
                MainForm.Instance.ShowForm(profileForm, "Профіль читача");
            }
            else
            {
                var readersForm = MainForm.Instance.GetCachedForm<ReadersForm>();
                readersForm.RefreshData();
                MainForm.Instance.ShowForm(readersForm, "Управління читачами");
            }
        }
    }
}