using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LibSys.Models;

namespace LibSys
{
    partial class JournalForm
    {
        private IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelTop = new Panel();
            lblStart = new Label();
            dtpStart = new DateTimePicker();
            lblEnd = new Label();
            dtpEnd = new DateTimePicker();
            dgvJournal = new DataGridView();
            
            panelTop.SuspendLayout();
            ((ISupportInitialize)dgvJournal).BeginInit();
            SuspendLayout();
            
            // panelTop
            panelTop.BackColor = UIColors.White;
            panelTop.Controls.Add(lblStart);
            panelTop.Controls.Add(dtpStart);
            panelTop.Controls.Add(lblEnd);
            panelTop.Controls.Add(dtpEnd);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Size = new Size(1160, 80);
            
            // lblStart
            lblStart.AutoSize = true;
            lblStart.Font = Fonts.Bold14;
            lblStart.Location = new Point(20, 27);
            lblStart.Text = "Період з:";
            
            // dtpStart
            dtpStart.Format = DateTimePickerFormat.Short;
            dtpStart.Location = new Point(130, 24);
            dtpStart.Size = new Size(160, 32);
            
            // lblEnd
            lblEnd.AutoSize = true;
            lblEnd.Font = Fonts.Bold14;
            lblEnd.Location = new Point(310, 27);
            lblEnd.Text = "по:";
            
            // dtpEnd
            dtpEnd.Format = DateTimePickerFormat.Short;
            dtpEnd.Location = new Point(360, 24);
            dtpEnd.Size = new Size(160, 32);
            
            // dgvJournal
            dgvJournal.AllowUserToAddRows = false;
            dgvJournal.AllowUserToDeleteRows = false;
            dgvJournal.BackgroundColor = UIColors.BackgroundAlt;
            dgvJournal.BorderStyle = BorderStyle.None;
            dgvJournal.Dock = DockStyle.Fill;
            dgvJournal.Location = new Point(0, 80);
            dgvJournal.ReadOnly = true;
            dgvJournal.RowHeadersVisible = false;
            dgvJournal.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvJournal.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvJournal.ColumnHeadersHeight = 45;
            
            dgvJournal.DefaultCellStyle.Font = Fonts.Regular14;
            dgvJournal.ColumnHeadersDefaultCellStyle.Font = Fonts.Bold14;
            dgvJournal.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvJournal.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvJournal.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            
            // JournalForm
            ClientSize = new Size(1160, 720);
            Controls.Add(dgvJournal);
            Controls.Add(panelTop);
            Font = Fonts.Regular14;
            FormBorderStyle = FormBorderStyle.None;
            Text = "Журнал";
            
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            ((ISupportInitialize)dgvJournal).EndInit();
            ResumeLayout(false);
        }

        private Panel panelTop;
        private Label lblStart;
        private DateTimePicker dtpStart;
        private Label lblEnd;
        private DateTimePicker dtpEnd;
        private DataGridView dgvJournal;
    }
}