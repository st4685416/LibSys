using LibSys.Models;
using LibSys.Services;

namespace LibSys
{
    public partial class ReaderProfileForm : Form
    {
        private int _currentReaderId;
        private readonly IDataQueryService _queryService;
        private readonly ILoanService _loanService;

        private string _sourceMenu = "Readers";

        public ReaderProfileForm()
        {
            InitializeComponent();
            _queryService = new DataQueryService();
            _loanService = new LoanService();

            btnBack.Click += btnBack_Click;
            btnEdit.Click += btnEdit_Click;
            btnIssueBook.Click += btnIssueBook_Click;
            btnReturn.Click += btnReturn_Click;
            btnReportLoss.Click += btnReportLoss_Click;
            btnPayDebt.Click += btnPayDebt_Click;

            panelTop.Resize += panelTop_Resize;
        }

        public void OpenProfile(int readerId, string sourceMenu = "Readers")
        {
            _currentReaderId = readerId;
            _sourceMenu = sourceMenu;
            LoadReaderData();
            RefreshLoansGrid();
        }

        private async void LoadReaderData()
        {
            await _loanService.UpdateOverdueLoansForReaderAsync(_currentReaderId);
            await _loanService.MarkZeroDebtLoansAsPaidAsync(_currentReaderId);

            var reader = await _queryService.GetReaderByIdAsync(_currentReaderId);
            if (reader == null) return;

            lblReaderName.Text = $"{UIIcons.User} {reader.FullName}";

            var unpaidLoans = await _queryService.GetLoansWithDetailsByReaderIdAsync(_currentReaderId);
            var unpaid = unpaidLoans.Where(l => !l.IsLossPaid).ToList();
            decimal debt = unpaid.Sum(l => l.CalculateCurrentDebt());

            if (debt > 0)
            {
                lblDebtStatus.Text = $"{UIIcons.Lost} Борг: {debt:0.00} грн";
                lblDebtStatus.ForeColor = UIColors.Error;
                btnPayDebt.Enabled = true;
            }
            else
            {
                lblDebtStatus.Text = $"{UIIcons.Available} Борги відсутні";
                lblDebtStatus.ForeColor = UIColors.Success;
                btnPayDebt.Enabled = false;
            }

            lblDebtStatus.Location = new Point(lblReaderName.Right + 30, lblReaderName.Top + 2);

            string birthDateStr = reader.BirthDate.FromUnixTimestamp().ToString("dd.MM.yyyy");

            lblPassport.Text = $"Паспорт: {reader.PassportNumber}";
            lblPhone.Text = $"Тел: {reader.Phone}";
            lblBirthDate.Text = $"Дн: {birthDateStr}";
            lblEmail.Text = $"Email: {reader.Email ?? "не вказано"}";

            panelTop_Resize(this, EventArgs.Empty);
        }

        private async void RefreshLoansGrid()
        {
            var loans = await _queryService.GetLoansWithDetailsByReaderIdAsync(_currentReaderId);

            dgvLoans.DataSource = loans.Select(l => new
            {
                ID_Видачі = l.Id,
                Книга = l.BookCopy.Book.Title,
                Інв_Номер = l.BookCopyId,
                Дата_Видачі = l.IssueDate.ToShortDateString(),
                Дедлайн = l.ExpectedReturnDate.ToShortDateString(),
                IsLostStatus = l.IsLost,
                IsReturnedStatus = l.ActualReturnDate != null,
                Статус = l.IsLost
                    ? $"{UIIcons.ActionLoss} ВТРАЧЕНО"
                    : l.ActualReturnDate != null
                        ? $"{UIIcons.Available} ПОВЕРНЕНО"
                        : l.ExpectedReturnDate < DateTime.Now
                            ? $"{UIIcons.Overdue} ПРОСТРОЧЕНО"
                            : $"{UIIcons.Loaned} НА РУКАХ"
            }).ToList();

            if (dgvLoans.Columns["ID_Видачі"] != null)
                dgvLoans.Columns["ID_Видачі"].Visible = false;
            if (dgvLoans.Columns["IsLostStatus"] != null)
                dgvLoans.Columns["IsLostStatus"].Visible = false;
            if (dgvLoans.Columns["IsReturnedStatus"] != null)
                dgvLoans.Columns["IsReturnedStatus"].Visible = false;
        }

        private void btnBack_Click(object? sender, EventArgs e)
        {
            if (_sourceMenu == "Journal")
            {
                var journalForm = MainForm.Instance.GetCachedForm<JournalForm>();
                journalForm.RefreshData();
                MainForm.Instance.ShowForm(journalForm, "Журнал операцій");
            }
            else
            {
                var readersForm = MainForm.Instance.GetCachedForm<ReadersForm>();
                readersForm.RefreshData();
                MainForm.Instance.ShowForm(readersForm, "Управління читачами");
            }
        }

        private void btnEdit_Click(object? sender, EventArgs e)
        {
            var editForm = MainForm.Instance.GetCachedForm<ReaderEditForm>();
            editForm.SetMode(_currentReaderId);
            MainForm.Instance.ShowForm(editForm, "Редагування профілю читача");
        }

        private async void btnIssueBook_Click(object? sender, EventArgs e)
        {
            if (btnPayDebt.Enabled)
            {
                MessageBox.Show(
                    "Неможливо видати нову книгу: у читача є неоплачена заборгованість! Спершу погасіть борг.",
                    "Заборона видачі", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var settings = await _queryService.GetSystemSettingsAsync();
            if (settings == null) return;
            int activeLoansCount = await _loanService.GetActiveLoansCountAsync(_currentReaderId);

            if (activeLoansCount >= settings.MaxBooksPerReader)
            {
                MessageBox.Show(
                    $"Неможливо видати нову книгу: читач уже досяг ліміту і має на руках {settings.MaxBooksPerReader} кн. (максимум).",
                    "Ліміт вичерпано", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var form = new IssueBookForm(_currentReaderId);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadReaderData();
                RefreshLoansGrid();
            }
        }

        private async void btnReturn_Click(object? sender, EventArgs e)
        {
            if (dgvLoans.SelectedRows.Count == 0)
            {
                MessageBox.Show("Оберіть активну видачу із таблиці!", "Увага", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            bool isLost = (bool)dgvLoans.SelectedRows[0].Cells["IsLostStatus"].Value;
            bool isReturned = (bool)dgvLoans.SelectedRows[0].Cells["IsReturnedStatus"].Value;

            if (isLost)
            {
                MessageBox.Show("Неможливо повернути книгу, яка вже має статус: ВТРАЧЕНО", "Інформація",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (isReturned)
            {
                MessageBox.Show("Ця книга вже має статус: ПОВЕРНЕНО", "Інформація",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int loanId = (int)dgvLoans.SelectedRows[0].Cells["ID_Видачі"].Value;
            var form = new ReturnBookForm(loanId);
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadReaderData();
                RefreshLoansGrid();
            }
        }

        private async void btnReportLoss_Click(object? sender, EventArgs e)
        {
            if (dgvLoans.SelectedRows.Count == 0)
            {
                MessageBox.Show("Оберіть книгу зі списку видач, яку читач втратив!", "Увага", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            bool isReturned = (bool)dgvLoans.SelectedRows[0].Cells["IsReturnedStatus"].Value;
            bool isLost = (bool)dgvLoans.SelectedRows[0].Cells["IsLostStatus"].Value;

            if (isReturned)
            {
                MessageBox.Show("Неможливо зафіксувати втрату книги, яка вже має статус: ПОВЕРНЕНО", "Інформація",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (isLost)
            {
                MessageBox.Show("Ця книга вже має статус: ВТРАЧЕНО", "Інформація",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int loanId = (int)dgvLoans.SelectedRows[0].Cells["ID_Видачі"].Value;
            string bookTitle = dgvLoans.SelectedRows[0].Cells["Книга"].Value.ToString() ?? "книгу";
            string confirmText =
                $"Ви впевнені, що хочете зафіксувати ВТРАТУ книги \"{bookTitle}\"?\n\nПримірник буде назавжди списано, а на баланс читача буде нараховано борг.";

            if (MessageBox.Show(confirmText, "Реєстрація втрати", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) ==
                DialogResult.Yes)
            {
                var result = await _loanService.ReportLossAsync(loanId);
                if (result.IsSuccess)
                {
                    MessageBox.Show(result.Message, "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadReaderData();
                    RefreshLoansGrid();
                }
                else
                {
                    MessageBox.Show(result.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnPayDebt_Click(object? sender, EventArgs e)
        {
            var payForm = new PayBookDebtForm(_currentReaderId);
            if (payForm.ShowDialog() == DialogResult.OK)
            {
                LoadReaderData();
                RefreshLoansGrid();
            }
        }

        private void panelTop_Resize(object? sender, EventArgs e)
        {
            int w = panelTop.Width;
            int btnW = 160;
            int btnH = 40;
            int gap = 8;
            int rightMargin = 20;
            int textGap = 5;

            int textY1 = 60;
            int textY2 = 95;

            lblPassport.Location = new Point(20, textY1);
            lblSep1.Visible = true;
            lblSep1.Location = new Point(lblPassport.Right + textGap, textY1);
            lblPhone.Location = new Point(lblSep1.Right + textGap, textY1);

            lblSep2.Visible = false;

            lblBirthDate.Location = new Point(20, textY2);
            lblSep3.Visible = true;
            lblSep3.Location = new Point(lblBirthDate.Right + textGap, textY2);
            lblEmail.Location = new Point(lblSep3.Right + textGap, textY2);

            if (w >= 1580)
            {
                panelTop.Height = 140;
                int startX = w - rightMargin - btnW;
                int btnY = 50;

                btnIssueBook.Location = new Point(startX, btnY);
                btnReturn.Location = new Point(startX - (btnW + gap), btnY);
                btnReportLoss.Location = new Point(startX - (btnW + gap) * 2, btnY);
                btnPayDebt.Location = new Point(startX - (btnW + gap) * 3, btnY);
                btnEdit.Location = new Point(startX - (btnW + gap) * 4, btnY);
            }
            else if (w >= 1240)
            {
                panelTop.Height = 160;
                int col3X = w - rightMargin - btnW;
                int col2X = col3X - (btnW + gap);
                int col1X = col2X - (btnW + gap);

                btnIssueBook.Location = new Point(col3X, 30);
                btnReturn.Location = new Point(col2X, 30);
                btnReportLoss.Location = new Point(col1X, 30);

                btnPayDebt.Location = new Point(col3X, 30 + btnH + gap);
                btnEdit.Location = new Point(col2X, 30 + btnH + gap);
            }
            else if (w >= 1070)
            {
                panelTop.Height = 190;
                int col2X = w - rightMargin - btnW;
                int col1X = col2X - (btnW + gap);

                int row1Y = 20;
                int row2Y = 20 + btnH + gap;
                int row3Y = 20 + (btnH + gap) * 2;

                btnIssueBook.Location = new Point(col2X, row1Y);
                btnReturn.Location = new Point(col1X, row1Y);
                btnReportLoss.Location = new Point(col2X, row2Y);
                btnPayDebt.Location = new Point(col1X, row2Y);
                btnEdit.Location = new Point(col2X, row3Y);
            }
            else
            {
                panelTop.Height = 265;
                int colX = w - rightMargin - btnW;
                int startY = 15;

                btnIssueBook.Location = new Point(colX, startY);
                btnReturn.Location = new Point(colX, startY + (btnH + gap));
                btnReportLoss.Location = new Point(colX, startY + (btnH + gap) * 2);
                btnPayDebt.Location = new Point(colX, startY + (btnH + gap) * 3);
                btnEdit.Location = new Point(colX, startY + (btnH + gap) * 4);
            }
        }
    }
}