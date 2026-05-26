using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LibSys.Models;

namespace LibSys
{
    partial class ReadersForm
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
            txtSearch = new TextBox();
            chkDebtorsOnly = new CheckBox();
            btnAddReader = new Button();
            btnProfile = new Button();
            dgvReaders = new DataGridView();
            panelTop.SuspendLayout();
            ((ISupportInitialize)dgvReaders).BeginInit();
            SuspendLayout();
            
            // panelTop
            panelTop.BackColor = UIColors.White;
            panelTop.Controls.Add(txtSearch);
            panelTop.Controls.Add(chkDebtorsOnly);
            panelTop.Controls.Add(btnAddReader);
            panelTop.Controls.Add(btnProfile);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Size = new Size(950, 80);
            
            // txtSearch
            txtSearch.Location = new Point(20, 24);
            txtSearch.Size = new Size(300, 32);
            txtSearch.PlaceholderText = $"{UIIcons.Search} Пошук (ПІБ, телефон, паспорт)...";
            txtSearch.TextChanged += txtSearch_TextChanged;
            
            // chkDebtorsOnly
            chkDebtorsOnly.AutoSize = true;
            chkDebtorsOnly.Location = new Point(360, 26);
            chkDebtorsOnly.Text = $"{UIIcons.Lost} Тільки боржники";
            chkDebtorsOnly.CheckedChanged += chkDebtorsOnly_CheckedChanged;
            
            // btnAddReader
            btnAddReader.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAddReader.BackColor = UIColors.Success;
            btnAddReader.ForeColor = UIColors.White;
            btnAddReader.FlatStyle = FlatStyle.Flat;
            btnAddReader.FlatAppearance.BorderSize = 0;
            btnAddReader.Location = new Point(650, 20);
            btnAddReader.Size = new Size(130, 40);
            btnAddReader.Text = $"{UIIcons.Add} Додати";
            btnAddReader.Click += btnAddReader_Click;
            
            // btnProfile
            btnProfile.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnProfile.BackColor = UIColors.Info;
            btnProfile.ForeColor = UIColors.White;
            btnProfile.FlatStyle = FlatStyle.Flat;
            btnProfile.FlatAppearance.BorderSize = 0;
            btnProfile.Location = new Point(800, 20);
            btnProfile.Size = new Size(130, 40);
            btnProfile.Text = $"{UIIcons.User} Профіль";
            btnProfile.Click += btnProfile_Click;
            
            // dgvReaders
            dgvReaders.AllowUserToAddRows = false;
            dgvReaders.AllowUserToDeleteRows = false;
            dgvReaders.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvReaders.BackgroundColor = UIColors.BackgroundAlt;
            dgvReaders.BorderStyle = BorderStyle.None;
            dgvReaders.Dock = DockStyle.Fill;
            dgvReaders.Location = new Point(0, 80);
            dgvReaders.ReadOnly = true;
            dgvReaders.RowHeadersVisible = false;
            dgvReaders.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReaders.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvReaders.ColumnHeadersHeight = 45;
            
            dgvReaders.DefaultCellStyle.Font = Fonts.Regular14;
            dgvReaders.ColumnHeadersDefaultCellStyle.Font = Fonts.Bold14;
            dgvReaders.RowTemplate.Height = 40;
            
            // ReadersForm
            ClientSize = new Size(950, 650);
            Controls.Add(dgvReaders);
            Controls.Add(panelTop);
            Font = Fonts.Regular14;
            FormBorderStyle = FormBorderStyle.None;
            Text = "Читачі";
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            ((ISupportInitialize)dgvReaders).EndInit();
            ResumeLayout(false);
        }

        private Panel panelTop;
        private TextBox txtSearch;
        private CheckBox chkDebtorsOnly;
        private Button btnAddReader;
        private Button btnProfile;
        private DataGridView dgvReaders;
    }
}