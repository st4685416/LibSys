using LibSys.Services;

namespace LibSys
{
	public partial class AddBookCopyForm : Form
	{
		private int _selectedBookId;
		private readonly IBookEditService _bookEditService;

		public AddBookCopyForm()
		{
			InitializeComponent();
			_bookEditService = new BookEditService();
		}

		public void PrepareForm(int bookId)
		{
			_selectedBookId = bookId;

			DateTime today = DateTime.Today;
			dtpArrivalDate.MaxDate = today;
			dtpArrivalDate.Value = today;

			numQuantity.Value = 1;
			numInitialState.Value = 100;
		}

		private async void btnSave_Click(object sender, EventArgs e)
		{
			int qty = (int)numQuantity.Value;
			int state = (int)numInitialState.Value;
			DateTime arrival = dtpArrivalDate.Value;

			await _bookEditService.AddBookCopiesAsync(_selectedBookId, qty, state, arrival);

			MessageBox.Show($"Успішно додано {qty} примірник(ів).", "Успіх", MessageBoxButtons.OK,
				MessageBoxIcon.Information);
			GoBack();
		}

		private void btnCancel_Click(object sender, EventArgs e) => GoBack();

		private void GoBack()
		{
			var catalog = MainForm.Instance.GetCachedForm<CatalogForm>();
			catalog.RefreshData();
			MainForm.Instance.ShowForm(catalog, "Каталог книг");
		}
	}
}