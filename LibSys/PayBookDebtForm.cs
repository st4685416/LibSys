using LibSys.Services;
using LibSys.Models;

namespace LibSys
{
    public partial class PayBookDebtForm : Form
    {
        private readonly int _readerId;
        private readonly ILoanService _loanService;
        private readonly IDataQueryService _queryService;

        public PayBookDebtForm(int readerId)
        {
            InitializeComponent();
            _readerId = readerId;
            _loanService = new LoanService();
            _queryService = new DataQueryService();

            dgvDebts.SelectionChanged += DgvDebts_SelectionChanged;
            btnPay.Click += btnPay_Click;
            btnClose.Click += btnClose_Click;

            Load += (o, e) => RefreshGrid();
        }

        private async void RefreshGrid()
        {
            var loans = await _queryService.GetUnpaidLoansByReaderAsync(_readerId);

            var debtList = loans
                .Select(l =>
                {
                    decimal currentDebt = l.CalculateCurrentDebt();
                    string statusText = $"{UIIcons.Loaned} На руках (Пеня зростає)";
                    bool isPayable = false;

                    if (l.IsLost)
                    {
                        statusText = $"{UIIcons.ActionLoss} Втрачено (Пеня зростає до оплати)";
                        isPayable = true;
                    }
                    else if (l.ActualReturnDate != null)
                    {
                        statusText = $"{UIIcons.ActionReturn} Повернено (Фіксований борг)";
                        isPayable = true;
                    }

                    return new
                    {
                        ID_Видачі = l.Id,
                        Книга = l.BookCopy.Book.Title,
                        Статус = statusText,
                        Борг_Грн = currentDebt,  // ← decimal
                        Доступно_До_Оплати = isPayable
                    };
                })
                .Where(x => x.Борг_Грн > 0)
                .ToList();

            dgvDebts.DataSource = debtList;

            if (dgvDebts.Columns["ID_Видачі"] != null) dgvDebts.Columns["ID_Видачі"].Visible = false;
            if (dgvDebts.Columns["Доступно_До_Оплати"] != null) dgvDebts.Columns["Доступно_До_Оплати"].Visible = false;
            if (dgvDebts.Columns["Борг_Грн"] != null)
            {
                dgvDebts.Columns["Борг_Грн"].HeaderText = "Борг (грн)";
                dgvDebts.Columns["Борг_Грн"].DefaultCellStyle.Format = "N2";  // ← ДОДАНО форматування
                dgvDebts.Columns["Борг_Грн"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }

            UpdatePaymentStatus();
        }

        private void DgvDebts_SelectionChanged(object? sender, EventArgs e)
        {
            UpdatePaymentStatus();
        }

        private void UpdatePaymentStatus()
        {
            if (dgvDebts.SelectedRows.Count == 0)
            {
                btnPay.Enabled = false;
                lblInfoMessage.Text = "Оберіть книгу зі списку для проведення оплати.";
                lblInfoMessage.ForeColor = UIColors.TextMuted;
                return;
            }

            var row = dgvDebts.SelectedRows[0];
            bool isPayable = (bool)row.Cells["Доступно_До_Оплати"].Value;
            decimal debtAmount = (decimal)row.Cells["Борг_Грн"].Value;
            string status = row.Cells["Статус"].Value.ToString() ?? "";

            if (isPayable)
            {
                btnPay.Enabled = true;
                lblInfoMessage.Text = status.Contains("Втрачено")
                    ? $"Поточна сума до сплати: {debtAmount:0.00} грн (Вартість книги + пеня). Оплата зупинить нарахування."
                    : $"Фіксована сума до сплати: {debtAmount:0.00} грн. Натисніть «Оплатити» для підтвердження.";
                lblInfoMessage.ForeColor = UIColors.Success;
            }
            else
            {
                btnPay.Enabled = false;
                lblInfoMessage.Text =
                    $"{UIIcons.Overdue} Оплата штрафу можлива лише після її повернення або фіксації втрати.";
                lblInfoMessage.ForeColor = UIColors.Error;
            }
        }

        private async void btnPay_Click(object? sender, EventArgs e)
        {
            if (dgvDebts.SelectedRows.Count == 0) return;

            int loanId = (int)dgvDebts.SelectedRows[0].Cells["ID_Видачі"].Value;
            decimal amount = (decimal)dgvDebts.SelectedRows[0].Cells["Борг_Грн"].Value;

            var result = await _loanService.PayDebtAsync(loanId);

            if (result.IsSuccess)
            {
                MessageBox.Show($"Успішно оплачено {amount:0.00} грн.", "Успіх", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                RefreshGrid();

                if (dgvDebts.Rows.Count == 0)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            else
            {
                MessageBox.Show(result.Message, "Помилка транзакції", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}