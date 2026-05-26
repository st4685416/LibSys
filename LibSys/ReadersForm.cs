using LibSys.Services;

namespace LibSys
{
	public partial class ReadersForm : Form
	{
		private readonly IDataQueryService _queryService;
		private readonly ILoanService _loanService;

		public ReadersForm()
		{
			InitializeComponent();
			_queryService = new DataQueryService();
			_loanService = new LoanService();
			Load += ReadersForm_Load;
		}

		private async void ReadersForm_Load(object? sender, EventArgs e)
		{
			await _loanService.UpdateAllOverdueLoansAsync();
			await RefreshDataAsync();
		}
        
		public void RefreshData()
		{
			_ = RefreshDataAsync();
		}

		private async Task RefreshDataAsync()
		{
			if (!this.IsHandleCreated) return;

			dgvReaders.DataSource = await _queryService.GetReadersAsync(txtSearch.Text, chkDebtorsOnly.Checked);

			if (dgvReaders.Columns["ID"] != null) 
				dgvReaders.Columns["ID"].Visible = false;
		}

		private void txtSearch_TextChanged(object sender, EventArgs e) => RefreshData();
        
		private void chkDebtorsOnly_CheckedChanged(object sender, EventArgs e) => RefreshData();

		private void btnAddReader_Click(object sender, EventArgs e)
		{
			var form = MainForm.Instance.GetCachedForm<ReaderEditForm>();
			form.SetMode(null); 
			MainForm.Instance.ShowForm(form, "Реєстрація читача");
		}

		private void btnProfile_Click(object sender, EventArgs e)
		{
			if (dgvReaders.SelectedRows.Count == 0) return;

			int readerId = (int)dgvReaders.SelectedRows[0].Cells["ID"].Value;
			var profileForm = MainForm.Instance.GetCachedForm<ReaderProfileForm>();
			profileForm.OpenProfile(readerId);
			MainForm.Instance.ShowForm(profileForm, "Профіль читача");
		}
		
	}
}