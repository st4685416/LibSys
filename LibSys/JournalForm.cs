using LibSys.Services;
using LibSys.Models;

namespace LibSys
{
    public partial class JournalForm : Form
    {
        private readonly IDataQueryService _queryService;

        public JournalForm()
        {
            InitializeComponent();

            _queryService = new DataQueryService();

            DateTime today = DateTime.Today;
            dtpStart.MaxDate = today;
            dtpEnd.MaxDate = today;
            dtpStart.Value = today.AddDays(-30);
            dtpEnd.Value = today;

            dtpStart.ValueChanged += DatePicker_ValueChanged;
            dtpEnd.ValueChanged += DatePicker_ValueChanged;

            dgvJournal.CellClick += DgvJournal_CellClick;
            dgvJournal.CellMouseEnter += DgvJournal_CellMouseEnter;
            dgvJournal.CellMouseLeave += DgvJournal_CellMouseLeave;

            Activated += (s, e) => RefreshData();

            Load += (s, e) => RefreshData();
        }

        private void DatePicker_ValueChanged(object? sender, EventArgs e)
        {
            dtpStart.ValueChanged -= DatePicker_ValueChanged;
            dtpEnd.ValueChanged -= DatePicker_ValueChanged;

            if (dtpStart.Value > dtpEnd.Value)
            {
                if (sender == dtpStart)
                {
                    dtpEnd.Value = dtpStart.Value;
                }
                else
                {
                    dtpStart.Value = dtpEnd.Value;
                }
            }

            dtpStart.ValueChanged += DatePicker_ValueChanged;
            dtpEnd.ValueChanged += DatePicker_ValueChanged;

            RefreshData();
        }

        public void RefreshData()
        {
            _ = RefreshDataAsync();
        }

        private async Task RefreshDataAsync()
        {
            var data = await _queryService.GetJournalAsync(dtpStart.Value, dtpEnd.Value);
            dgvJournal.DataSource = data;

            if (dgvJournal.Columns["ReaderId"] != null)
                dgvJournal.Columns["ReaderId"].Visible = false;

            if (dgvJournal.Columns["DateValue"] != null)
                dgvJournal.Columns["DateValue"].Visible = false;

            if (dgvJournal.Columns["Читач"] != null)
            {
                dgvJournal.Columns["Читач"].DefaultCellStyle.ForeColor = UIColors.Info;
                dgvJournal.Columns["Читач"].DefaultCellStyle.Font = Fonts.Underline14;
            }
        }

        private void DgvJournal_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvJournal.Columns[e.ColumnIndex].Name == "Читач")
            {
                int readerId = (int)dgvJournal.Rows[e.RowIndex].Cells["ReaderId"].Value;
                var profileForm = MainForm.Instance.GetCachedForm<ReaderProfileForm>();

                profileForm.OpenProfile(readerId, "Journal");
                MainForm.Instance.ShowForm(profileForm, "Профіль читача");
            }
        }

        private void DgvJournal_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvJournal.Columns[e.ColumnIndex].Name == "Читач")
            {
                dgvJournal.Cursor = Cursors.Hand;
            }
        }

        private void DgvJournal_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            dgvJournal.Cursor = Cursors.Default;
        }
    }
}