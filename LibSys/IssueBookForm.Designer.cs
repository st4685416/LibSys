using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LibSys.Models;

namespace LibSys
{
    partial class IssueBookForm
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
            dgvCopies = new DataGridView();
            panelBottom = new Panel();
            btnCancel = new Button();
            btnIssue = new Button();
            
            panelTop.SuspendLayout();
            ((ISupportInitialize)dgvCopies).BeginInit();
            panelBottom.SuspendLayout();
            SuspendLayout();
            
            // panelTop
            panelTop.BackColor = UIColors.White;
            panelTop.Controls.Add(txtSearch);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Size = new Size(850, 70);
            
            // txtSearch
            txtSearch.Location = new Point(20, 20);
            txtSearch.Size = new Size(350, 32);
            txtSearch.PlaceholderText = $"{UIIcons.Search} Введіть інвентарний номер (штрихкод)...";
            txtSearch.TextChanged += txtSearch_TextChanged;
            
            // dgvCopies
            dgvCopies.AllowUserToAddRows = false;
            dgvCopies.AllowUserToDeleteRows = false;
            dgvCopies.BackgroundColor = UIColors.BackgroundAlt;
            dgvCopies.BorderStyle = BorderStyle.None;
            dgvCopies.Dock = DockStyle.Fill;
            dgvCopies.Location = new Point(0, 70);
            dgvCopies.ReadOnly = true;
            dgvCopies.RowHeadersVisible = false;
            dgvCopies.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCopies.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvCopies.ColumnHeadersHeight = 45;
            
            dgvCopies.DefaultCellStyle.Font = Fonts.Regular14;
            dgvCopies.ColumnHeadersDefaultCellStyle.Font = Fonts.Bold14;
            dgvCopies.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCopies.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvCopies.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            
            // panelBottom
            panelBottom.BackColor = UIColors.White;
            panelBottom.Controls.Add(btnCancel);
            panelBottom.Controls.Add(btnIssue);
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Location = new Point(0, 520);
            panelBottom.Size = new Size(850, 80);
            
            // btnCancel
            btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnCancel.BackColor = UIColors.ButtonSecondary;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Location = new Point(20, 20);
            btnCancel.Size = new Size(160, 40);
            btnCancel.Text = "Скасувати";
            btnCancel.Click += btnCancel_Click;
            
            // btnIssue
            btnIssue.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnIssue.BackColor = UIColors.Success;
            btnIssue.FlatAppearance.BorderSize = 0;
            btnIssue.FlatStyle = FlatStyle.Flat;
            btnIssue.ForeColor = UIColors.White;
            btnIssue.Location = new Point(670, 20);
            btnIssue.Size = new Size(160, 40);
            btnIssue.Text = $"{UIIcons.ActionIssue} Видати книгу";
            btnIssue.Click += btnIssue_Click;
            
            // IssueBookForm
            ClientSize = new Size(850, 600);
            Controls.Add(dgvCopies);
            Controls.Add(panelBottom);
            Controls.Add(panelTop);
            Font = Fonts.Regular14;
            FormBorderStyle = FormBorderStyle.FixedDialog; 
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent; 
            Text = "Оформлення видачі";
            
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            ((ISupportInitialize)dgvCopies).EndInit();
            panelBottom.ResumeLayout(false);
            ResumeLayout(false);
        }

        private Panel panelTop;
        private TextBox txtSearch;
        private DataGridView dgvCopies;
        private Panel panelBottom;
        private Button btnCancel;
        private Button btnIssue;
    }
}