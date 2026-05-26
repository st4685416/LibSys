using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LibSys.Models;

namespace LibSys
{
    partial class PayBookDebtForm
    {
        private IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            dgvDebts = new DataGridView();
            panelBottom = new Panel();
            lblInfoMessage = new Label();
            btnPay = new Button();
            btnClose = new Button();
            
            ((ISupportInitialize)dgvDebts).BeginInit();
            panelBottom.SuspendLayout();
            SuspendLayout();
            
            // dgvDebts
            dgvDebts.AllowUserToAddRows = false;
            dgvDebts.AllowUserToDeleteRows = false;
            dgvDebts.BackgroundColor = UIColors.BackgroundAlt;
            dgvDebts.BorderStyle = BorderStyle.None;
            dgvDebts.Dock = DockStyle.Fill;
            dgvDebts.ReadOnly = true;
            dgvDebts.RowHeadersVisible = false;
            dgvDebts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDebts.ColumnHeadersHeight = 45;
            dgvDebts.DefaultCellStyle.Font = Fonts.Regular13;
            dgvDebts.ColumnHeadersDefaultCellStyle.Font = Fonts.Bold14;
            dgvDebts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            
            // panelBottom
            panelBottom.BackColor = UIColors.White;
            panelBottom.Controls.Add(lblInfoMessage);
            panelBottom.Controls.Add(btnPay);
            panelBottom.Controls.Add(btnClose);
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Location = new Point(0, 360);
            panelBottom.Size = new Size(800, 120);
            
            // lblInfoMessage
            lblInfoMessage.Location = new Point(20, 15);
            lblInfoMessage.Size = new Size(760, 40);
            lblInfoMessage.Font = Fonts.Italic14;
            lblInfoMessage.TextAlign = ContentAlignment.MiddleLeft;
            
            // btnClose
            btnClose.Location = new Point(440, 65);
            btnClose.Size = new Size(160, 40);
            btnClose.Text = "Скасувати";
            btnClose.BackColor = UIColors.ButtonSecondary;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.FlatAppearance.BorderSize = 0;
            
            // btnPay
            btnPay.Location = new Point(615, 65);
            btnPay.Size = new Size(160, 40);
            btnPay.Text = "Оплатити";
            btnPay.BackColor = UIColors.Success;
            btnPay.ForeColor = UIColors.White;
            btnPay.FlatStyle = FlatStyle.Flat;
            btnPay.FlatAppearance.BorderSize = 0;
            btnPay.Font = Fonts.Bold14;
            
            // PayBookDebtForm
            ClientSize = new Size(800, 480);
            Controls.Add(dgvDebts);
            Controls.Add(panelBottom);
            Font = Fonts.Regular14;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Погашення заборгованостей по книгах";
            
            ((ISupportInitialize)dgvDebts).EndInit();
            panelBottom.ResumeLayout(false);
            ResumeLayout(false);
        }

        private DataGridView dgvDebts;
        private Panel panelBottom;
        private Label lblInfoMessage;
        private Button btnPay;
        private Button btnClose;
    }
}