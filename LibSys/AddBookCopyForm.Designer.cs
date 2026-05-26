using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LibSys.Models;

namespace LibSys
{
    partial class AddBookCopyForm
    {
        private IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblQuantity = new Label();
            numQuantity = new NumericUpDown();
            lblInitialState = new Label();
            numInitialState = new NumericUpDown();
            lblArrivalDate = new Label();
            dtpArrivalDate = new DateTimePicker();
            btnSave = new Button();
            btnCancel = new Button();
            
            ((ISupportInitialize)numQuantity).BeginInit();
            ((ISupportInitialize)numInitialState).BeginInit();
            SuspendLayout();
            
            int startX = 30;
            int inputWidth = 340;
            
            // ================= КІЛЬКІСТЬ =================
            lblQuantity.AutoSize = true;
            lblQuantity.Location = new Point(startX, 30);
            lblQuantity.Text = "Кількість примірників *";
            lblQuantity.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            
            numQuantity.Location = new Point(startX, 65);
            numQuantity.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numQuantity.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            numQuantity.Value = new decimal(new int[] { 1, 0, 0, 0 });
            numQuantity.Size = new Size(inputWidth, 32);
            
            // ================= СТАН =================
            lblInitialState.AutoSize = true;
            lblInitialState.Location = new Point(startX, 115);
            lblInitialState.Text = "Стан (0-100) *";
            lblInitialState.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            
            numInitialState.Location = new Point(startX, 150);
            numInitialState.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            numInitialState.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            numInitialState.Value = new decimal(new int[] { 100, 0, 0, 0 });
            numInitialState.Size = new Size(inputWidth, 32);

            // ================= ДАТА =================
            lblArrivalDate.AutoSize = true;
            lblArrivalDate.Location = new Point(startX, 200);
            lblArrivalDate.Text = "Дата надходження *";
            lblArrivalDate.Font = new Font("Segoe UI", 14F, FontStyle.Bold);

            dtpArrivalDate.Location = new Point(startX, 235);
            dtpArrivalDate.Size = new Size(inputWidth, 32);
            
            // ================= КНОПКИ =================
            // btnCancel
            btnCancel.BackColor = UIColors.ButtonSecondary;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Location = new Point(30, 300);
            btnCancel.Size = new Size(160, 40);
            btnCancel.Text = "Скасувати";
            btnCancel.Click += btnCancel_Click;

            // btnSave
            btnSave.BackColor = UIColors.Success;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(210, 300);
            btnSave.Size = new Size(160, 40);
            btnSave.Text = "Додати";
            btnSave.Click += btnSave_Click;
            
            // ================= ФОРМА =================
            ClientSize = new Size(400, 380);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(dtpArrivalDate);
            Controls.Add(lblArrivalDate);
            Controls.Add(numInitialState);
            Controls.Add(lblInitialState);
            Controls.Add(numQuantity);
            Controls.Add(lblQuantity);
            
            Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.White;
            Text = "Додавання примірників";
            
            ((ISupportInitialize)numQuantity).EndInit();
            ((ISupportInitialize)numInitialState).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblQuantity;
        private NumericUpDown numQuantity;
        private Label lblInitialState;
        private NumericUpDown numInitialState;
        private Label lblArrivalDate;
        private DateTimePicker dtpArrivalDate;
        private Button btnSave;
        private Button btnCancel;
    }
}