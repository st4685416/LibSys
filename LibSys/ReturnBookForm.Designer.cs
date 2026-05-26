using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LibSys.Models;

namespace LibSys
{
    partial class ReturnBookForm
    {
        private IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitle = new Label();
            lblBookInfo = new Label();
            lblState = new Label();
            numState = new NumericUpDown();
            btnSave = new Button();
            btnCancel = new Button();
            ((ISupportInitialize)numState).BeginInit();
            SuspendLayout();
            
            // lblTitle
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTitle.Location = new Point(20, 20);
            lblTitle.Text = $"{UIIcons.ActionReturn} Повернення книги";
            
            // lblBookInfo
            lblBookInfo.AutoSize = true;
            lblBookInfo.Font = Fonts.Regular14;
            lblBookInfo.ForeColor = UIColors.TextMuted;
            lblBookInfo.Location = new Point(25, 60);
            lblBookInfo.Text = "Книга: Завантаження...\nІнв. №: ...";
            
            // lblState
            lblState.AutoSize = true;
            lblState.Font = Fonts.Bold14;
            lblState.Location = new Point(25, 140);
            lblState.Text = "Оцініть поточний стан (0-100):";
            
            // numState
            numState.Location = new Point(25, 175);
            numState.Minimum = new decimal(new int[] { 0, 0, 0, 0 });
            numState.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            numState.Value = new decimal(new int[] { 100, 0, 0, 0 });
            numState.Size = new Size(350, 32);
            
            // btnCancel
            btnCancel.BackColor = UIColors.ButtonSecondary;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Location = new Point(45, 240);
            btnCancel.Size = new Size(160, 40);
            btnCancel.Text = "Скасувати";
            btnCancel.Click += btnCancel_Click;

            // btnSave
            btnSave.BackColor = UIColors.Success;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.ForeColor = UIColors.White;
            btnSave.Location = new Point(215, 240);
            btnSave.Size = new Size(160, 40);
            btnSave.Text = "Підтвердити";
            btnSave.Click += btnSave_Click;
            
            // ReturnBookForm
            ClientSize = new Size(400, 320);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(numState);
            Controls.Add(lblState);
            Controls.Add(lblBookInfo);
            Controls.Add(lblTitle);
            
            Font = Fonts.Regular14;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Повернення";
            
            ((ISupportInitialize)numState).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblTitle;
        private Label lblBookInfo;
        private Label lblState;
        private NumericUpDown numState;
        private Button btnSave;
        private Button btnCancel;
    }
}