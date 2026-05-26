using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LibSys.Models;

namespace LibSys
{
    partial class ReaderProfileForm
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
            btnBack = new Button();
            lblReaderName = new Label();
            lblDebtStatus = new Label();

            lblPassport = new Label();
            lblPhone = new Label();
            lblBirthDate = new Label();
            lblEmail = new Label();
            
            lblSep1 = new Label();
            lblSep2 = new Label();
            lblSep3 = new Label();
            
            btnEdit = new Button();
            btnIssueBook = new Button();
            btnReturn = new Button();
            btnReportLoss = new Button();
            btnPayDebt = new Button();
            
            dgvLoans = new DataGridView();
            panelTop.SuspendLayout();
            ((ISupportInitialize)dgvLoans).BeginInit();
            SuspendLayout();
            
            panelTop.BackColor = UIColors.White;
            panelTop.Controls.Add(btnBack);
            panelTop.Controls.Add(lblReaderName);
            panelTop.Controls.Add(lblDebtStatus);
            
            panelTop.Controls.Add(lblPassport);
            panelTop.Controls.Add(lblSep1);
            panelTop.Controls.Add(lblPhone);
            panelTop.Controls.Add(lblSep2);
            panelTop.Controls.Add(lblBirthDate);
            panelTop.Controls.Add(lblSep3);
            panelTop.Controls.Add(lblEmail);
            
            panelTop.Controls.Add(btnEdit);
            panelTop.Controls.Add(btnIssueBook);
            panelTop.Controls.Add(btnReturn);
            panelTop.Controls.Add(btnReportLoss);
            panelTop.Controls.Add(btnPayDebt);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Size = new Size(1100, 115); 
            
            // btnBack 
            btnBack.BackColor = UIColors.ButtonSecondary;
            btnBack.FlatStyle = FlatStyle.Flat; 
            btnBack.FlatAppearance.BorderSize = 0;
            btnBack.Location = new Point(20, 15); 
            btnBack.Size = new Size(130, 40);
            btnBack.Text = $"{UIIcons.Back} Назад";

            // lblReaderName
            lblReaderName.AutoSize = true;
            lblReaderName.Font = Fonts.Bold18;
            lblReaderName.Location = new Point(170, 17);
            lblReaderName.Text = "ПІБ Читача";

            // lblDebtStatus
            lblDebtStatus.AutoSize = true;
            lblDebtStatus.Font = Fonts.Bold14;
            lblDebtStatus.Location = new Point(400, 21); 
            lblDebtStatus.Text = "Статус боргу: ...";

            Color infoColor = UIColors.TextMuted;
            
            lblPassport.AutoSize = true; lblPassport.ForeColor = infoColor; lblPassport.Text = "Паспорт: ...";
            lblPhone.AutoSize = true; lblPhone.ForeColor = infoColor; lblPhone.Text = "Тел: ...";
            lblBirthDate.AutoSize = true; lblBirthDate.ForeColor = infoColor; lblBirthDate.Text = "Дн: ...";
            lblEmail.AutoSize = true; lblEmail.ForeColor = infoColor; lblEmail.Text = "Email: ...";

            lblSep1.AutoSize = true; lblSep1.ForeColor = Color.Silver; lblSep1.Text = "|";
            lblSep2.AutoSize = true; lblSep2.ForeColor = Color.Silver; lblSep2.Text = "|";
            lblSep3.AutoSize = true; lblSep3.ForeColor = Color.Silver; lblSep3.Text = "|";

            // Кнопки дій
            btnEdit.BackColor = Color.FromArgb(51, 51, 76);
            btnEdit.ForeColor = UIColors.White;
            btnEdit.FlatStyle = FlatStyle.Flat;
            btnEdit.FlatAppearance.BorderSize = 0;
            btnEdit.Size = new Size(160, 40);
            btnEdit.Text = $"{UIIcons.Edit} Редагувати";

            btnPayDebt.BackColor = UIColors.Warning;
            btnPayDebt.ForeColor = UIColors.White;
            btnPayDebt.FlatStyle = FlatStyle.Flat;
            btnPayDebt.FlatAppearance.BorderSize = 0;
            btnPayDebt.Size = new Size(160, 40);
            btnPayDebt.Text = $"{UIIcons.Money} Оплатити"; 

            btnReportLoss.BackColor = UIColors.Error;
            btnReportLoss.ForeColor = UIColors.White;
            btnReportLoss.FlatStyle = FlatStyle.Flat;
            btnReportLoss.FlatAppearance.BorderSize = 0;
            btnReportLoss.Size = new Size(160, 40);
            btnReportLoss.Text = $"{UIIcons.ActionLoss} Втрата"; 

            btnReturn.BackColor = UIColors.Info;
            btnReturn.ForeColor = UIColors.White;
            btnReturn.FlatStyle = FlatStyle.Flat;
            btnReturn.FlatAppearance.BorderSize = 0;
            btnReturn.Size = new Size(160, 40);
            btnReturn.Text = $"{UIIcons.Return} Повернути"; 

            btnIssueBook.BackColor = UIColors.Success;
            btnIssueBook.ForeColor = UIColors.White;
            btnIssueBook.FlatStyle = FlatStyle.Flat;
            btnIssueBook.FlatAppearance.BorderSize = 0;
            btnIssueBook.Size = new Size(160, 40);
            btnIssueBook.Text = $"{UIIcons.ActionIssue} Видати книгу"; 

            // dgvLoans
            dgvLoans.AllowUserToAddRows = false;
            dgvLoans.AllowUserToDeleteRows = false;
            dgvLoans.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLoans.BackgroundColor = UIColors.BackgroundAlt;
            dgvLoans.BorderStyle = BorderStyle.None;
            dgvLoans.Dock = DockStyle.Fill;
            dgvLoans.Location = new Point(0, 115);
            dgvLoans.ReadOnly = true;
            dgvLoans.RowHeadersVisible = false;
            dgvLoans.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            
            dgvLoans.DefaultCellStyle.Font = Fonts.Regular14;
            dgvLoans.ColumnHeadersDefaultCellStyle.Font = Fonts.Bold14;
            dgvLoans.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvLoans.ColumnHeadersHeight = 45; 
            dgvLoans.RowTemplate.Height = 40; 
            
            // ReaderProfileForm
            ClientSize = new Size(1100, 700); 
            Controls.Add(dgvLoans);
            Controls.Add(panelTop);
            Font = Fonts.Regular14;
            FormBorderStyle = FormBorderStyle.None;
            Text = "Картка Читача";
            
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            ((ISupportInitialize)dgvLoans).EndInit();
            ResumeLayout(false);
        }

        private Panel panelTop;
        private Label lblReaderName;
        private Label lblDebtStatus;
        
        private Label lblPassport;
        private Label lblPhone;
        private Label lblBirthDate;
        private Label lblEmail;
        
        private Label lblSep1;
        private Label lblSep2;
        private Label lblSep3;

        private Button btnBack;
        private Button btnEdit;
        private Button btnIssueBook;
        private Button btnReturn;
        private Button btnReportLoss;
        private Button btnPayDebt;
        private DataGridView dgvLoans;
    }
}