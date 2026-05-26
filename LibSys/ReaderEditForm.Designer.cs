using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LibSys.Models;

namespace LibSys
{
    partial class ReaderEditForm
    {
        private IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblLastName = new Label(); txtLastName = new TextBox();
            lblFirstName = new Label(); txtFirstName = new TextBox();
            lblMiddleName = new Label(); txtMiddleName = new TextBox();
            lblPassport = new Label(); txtPassport = new TextBox();
            lblPhone = new Label(); txtPhone = new TextBox();
            lblEmail = new Label(); txtEmail = new TextBox();
            lblBirthDate = new Label(); dtpBirthDate = new DateTimePicker();
            btnSave = new Button();
            btnCancel = new Button();
            
            SuspendLayout();
            
            int startY = 30; int gap = 50; int labelX = 30; int inputX = 180; int inputW = 360;

            // Поля вводу персональних даних
            lblLastName.AutoSize = true; lblLastName.Location = new Point(labelX, startY); lblLastName.Text = "Прізвище *"; lblLastName.Font = Fonts.Bold14;
            txtLastName.Location = new Point(inputX, startY); txtLastName.Size = new Size(inputW, 32);

            lblFirstName.AutoSize = true; lblFirstName.Location = new Point(labelX, startY + gap); lblFirstName.Text = "Ім'я *"; lblFirstName.Font = Fonts.Bold14;
            txtFirstName.Location = new Point(inputX, startY + gap); txtFirstName.Size = new Size(inputW, 32);

            lblMiddleName.AutoSize = true; lblMiddleName.Location = new Point(labelX, startY + gap * 2); lblMiddleName.Text = "По батькові";
            txtMiddleName.Location = new Point(inputX, startY + gap * 2); txtMiddleName.Size = new Size(inputW, 32);

            lblPassport.AutoSize = true; lblPassport.Location = new Point(labelX, startY + gap * 3); lblPassport.Text = "Паспорт / ID *"; lblPassport.Font = Fonts.Bold14;
            txtPassport.Location = new Point(inputX, startY + gap * 3); txtPassport.Size = new Size(inputW, 32);

            lblPhone.AutoSize = true; lblPhone.Location = new Point(labelX, startY + gap * 4); lblPhone.Text = "Телефон *"; lblPhone.Font = Fonts.Bold14;
            txtPhone.Location = new Point(inputX, startY + gap * 4); txtPhone.Size = new Size(inputW, 32);

            lblEmail.AutoSize = true; lblEmail.Location = new Point(labelX, startY + gap * 5); lblEmail.Text = "Email";
            txtEmail.Location = new Point(inputX, startY + gap * 5); txtEmail.Size = new Size(inputW, 32);

            lblBirthDate.AutoSize = true; lblBirthDate.Location = new Point(labelX, startY + gap * 6); lblBirthDate.Text = "Народження *"; lblBirthDate.Font = Fonts.Bold14;
            dtpBirthDate.Location = new Point(inputX, startY + gap * 6); dtpBirthDate.Size = new Size(200, 32); dtpBirthDate.Format = DateTimePickerFormat.Short;

            // ================= КНОПКИ КЕРУВАННЯ (Зсунуто вниз на 268px) =================
            int buttonsY = 658; 

            btnCancel.BackColor = UIColors.ButtonSecondary;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Location = new Point(inputX, buttonsY);
            btnCancel.Size = new Size(160, 40);
            btnCancel.Text = "Скасувати";
            btnCancel.Click += btnCancel_Click;

            btnSave.BackColor = UIColors.Success;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.ForeColor = UIColors.White;
            btnSave.Location = new Point(inputX + 180, buttonsY);
            btnSave.Size = new Size(180, 40);
            btnSave.Text = "Зберегти";
            btnSave.Click += btnSave_Click;

            // Корекція розмірів контейнера форми під нове розташування кнопок
            ClientSize = new Size(580, 730);
            
            Controls.Add(lblLastName); Controls.Add(txtLastName);
            Controls.Add(lblFirstName); Controls.Add(txtFirstName); Controls.Add(lblMiddleName); Controls.Add(txtMiddleName);
            Controls.Add(lblPassport); Controls.Add(txtPassport); Controls.Add(lblPhone); Controls.Add(txtPhone);
            Controls.Add(lblEmail); Controls.Add(txtEmail); Controls.Add(lblBirthDate); Controls.Add(dtpBirthDate);
            Controls.Add(btnSave); Controls.Add(btnCancel);
            
            Font = Fonts.Regular14;
            FormBorderStyle = FormBorderStyle.None; 
            BackColor = UIColors.White;
            Text = "Картка читача";

            ResumeLayout(false); PerformLayout();
        }

        private Label lblLastName; private TextBox txtLastName;
        private Label lblFirstName; private TextBox txtFirstName;
        private Label lblMiddleName; private TextBox txtMiddleName;
        private Label lblPassport; private TextBox txtPassport;
        private Label lblPhone; private TextBox txtPhone;
        private Label lblEmail; private TextBox txtEmail;
        private Label lblBirthDate; private DateTimePicker dtpBirthDate;
        private Button btnSave; private Button btnCancel;
    }
}