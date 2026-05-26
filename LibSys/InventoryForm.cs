using LibSys.Models;
using LibSys.Services;

namespace LibSys
{
    public partial class InventoryForm : Form
    {
        private readonly IDataQueryService _queryService;
        private readonly ILoanService _loanService;

        public InventoryForm()
        {
            InitializeComponent();
            _queryService = new DataQueryService();
            _loanService = new LoanService();

            chkAvailable.Checked = true;
            chkLoaned.Checked = true;
            chkLost.Checked = false;
            chkWrittenOff.Checked = false;

            cmbSort.Items.AddRange(
                "Стан: гірші", "Стан: кращі",
                "Дата: новіші", "Дата: старіші"
            );
            cmbSort.SelectedIndex = 0;

            chkAvailable.CheckedChanged += StatusCheckbox_CheckedChanged;
            chkLoaned.CheckedChanged += StatusCheckbox_CheckedChanged;
            chkLost.CheckedChanged += StatusCheckbox_CheckedChanged;
            chkWrittenOff.CheckedChanged += StatusCheckbox_CheckedChanged;

            panelTop.Resize += PanelTop_Resize;

            this.Load += (o, e) => RefreshData();
        }

        private async void StatusCheckbox_CheckedChanged(object? sender, EventArgs e) => await RefreshDataAsync();

        public void RefreshData()
        {
            _ = RefreshDataAsync();
        }

        private async Task RefreshDataAsync()
        {
            if (!this.IsHandleCreated) return;

            var sortOption = (InventorySortOption)cmbSort.SelectedIndex;

            var data = await _queryService.GetInventoryAsync(
                txtSearch.Text,
                sortOption,
                chkAvailable.Checked,
                chkLoaned.Checked,
                chkLost.Checked,
                chkWrittenOff.Checked
            );

            dgvInventory.DataSource = data;

            if (dgvInventory.Columns["ID_Книги"] != null)
                dgvInventory.Columns["ID_Книги"].Visible = false;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e) => RefreshData();
        private void cmbSort_SelectedIndexChanged(object sender, EventArgs e) => RefreshData();

        private async void btnWriteOff_Click(object sender, EventArgs e)
        {
            if (dgvInventory.SelectedRows.Count == 0) return;

            int copyId = (int)dgvInventory.SelectedRows[0].Cells["Інв_Номер"].Value;

            var copy = await _queryService.GetBookCopyByIdAsync(copyId);
            if (copy == null) return;

            if (copy.Status == BookCopyStatus.Loaned)
            {
                MessageBox.Show("Не можна списати книгу, яка зараз знаходиться на руках у читача!", "Помилка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (copy.Status == BookCopyStatus.WrittenOff)
            {
                MessageBox.Show("Цей примірник вже списано.", "Інфо", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show($"Ви впевнені, що хочете остаточно СПИСАТИ примірник №{copyId}?", "Списання",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                var result = await _loanService.WriteOffCopyAsync(copyId);
                if (result.IsSuccess)
                {
                    await RefreshDataAsync();
                    MessageBox.Show(result.Message, "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(result.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void PanelTop_Resize(object? sender, EventArgs e)
        {
            int w = panelTop.Width;
            int startX = 20;

            txtSearch.Location = new Point(startX, 24);
            cmbSort.Location = new Point(txtSearch.Right + 20, 24);

            chkAvailable.Location = new Point(cmbSort.Right + 30, 15);
            chkLoaned.Location = new Point(cmbSort.Right + 30, 45);

            chkLost.Location = new Point(chkAvailable.Right + 15, 15);
            chkWrittenOff.Location = new Point(chkLoaned.Right + 15, 45);

            btnWriteOff.Location = new Point(w - btnWriteOff.Width - 20, 20);
        }
    }
}