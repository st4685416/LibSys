using LibSys.Services;

namespace LibSys
{
    public partial class IssueBookForm : Form
    {
        private readonly int _readerId;
        private readonly ILoanService _loanService;
        private readonly IDataQueryService _queryService;

        public IssueBookForm(int readerId)
        {
            InitializeComponent();
            _readerId = readerId;

            _loanService = new LoanService();
            _queryService = new DataQueryService();

            Load += IssueBookForm_Load;
        }

        private void IssueBookForm_Load(object? sender, EventArgs e)
        {
            SearchAvailableBooks();
        }

        private void txtSearch_TextChanged(object? sender, EventArgs e)
        {
            SearchAvailableBooks();
        }

        private async void SearchAvailableBooks()
        {
            dgvCopies.DataSource = await _queryService.GetAvailableCopiesForIssueAsync(txtSearch.Text);

            if (dgvCopies.Columns.Count > 0)
            {
                if (dgvCopies.Columns["Інв_Номер"] != null)
                    dgvCopies.Columns["Інв_Номер"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                if (dgvCopies.Columns["Видавництво"] != null)
                    dgvCopies.Columns["Видавництво"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private async void btnIssue_Click(object sender, EventArgs e)
        {
            if (dgvCopies.SelectedRows.Count == 0)
            {
                MessageBox.Show("Будь ласка, оберіть примірник зі списку!", "Увага", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            int copyId = (int)dgvCopies.SelectedRows[0].Cells["Інв_Номер"].Value;
            var result = await _loanService.IssueBookAsync(_readerId, copyId);

            if (result.IsSuccess)
            {
                MessageBox.Show(result.Message, "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show(result.Message, "Відмова у видачі", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}