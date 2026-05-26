using LibSys.Services;

namespace LibSys
{
	public partial class ReturnBookForm : Form
	{
		private readonly int _loanId;
		private readonly ILoanService _loanService;
		private readonly IDataQueryService _queryService;

		public ReturnBookForm(int loanId)
		{
			InitializeComponent();
			_loanId = loanId;
			_loanService = new LoanService();
			_queryService = new DataQueryService();
			Load += ReturnBookForm_Load;
		}

		private async void ReturnBookForm_Load(object? sender, EventArgs e)
		{
			var loan = await _queryService.GetLoanWithDetailsByIdAsync(_loanId);
			if (loan == null) return;

			lblBookInfo.Text = $"Книга: {loan.BookCopy.Book.Title}\nІнв. №: {loan.BookCopyId}";
			numState.Value = loan.BookCopy.StateScore;
		}

		private async void btnSave_Click(object sender, EventArgs e)
		{
			int newState = (int)numState.Value;
			var result = await _loanService.ReturnBookAsync(_loanId, newState);

			if (result.IsSuccess)
			{
				MessageBox.Show(result.Message, "Повернення", MessageBoxButtons.OK, MessageBoxIcon.Information);
				DialogResult = DialogResult.OK;
				Close();
			}
			else
			{
				MessageBox.Show(result.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}
	}
}