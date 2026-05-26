using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LibSys.Models;

namespace LibSys
{
    partial class SettingsForm
    {
        private IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitleSettings = new Label();
            lblMaxBooks = new Label(); numMaxBooks = new NumericUpDown();
            lblShortLoan = new Label(); numShortLoan = new NumericUpDown();
            lblLongLoan = new Label(); numLongLoan = new NumericUpDown();
            lblBanDays = new Label(); numBanDays = new NumericUpDown();
            lblPenaltyBase = new Label(); numPenaltyBase = new NumericUpDown();
            lblPenaltyPercent = new Label(); numPenaltyPercent = new NumericUpDown();
            lblNewBookAge = new Label(); numNewBookAge = new NumericUpDown();
            lblRareCopies = new Label(); numRareCopies = new NumericUpDown();
            btnSaveSettings = new Button();
            btnCancelSettings = new Button(); 

            lblTitleArchive = new Label();
            cmbDictType = new ComboBox();
            chkShowArchived = new CheckBox();
            txtSearchDict = new TextBox(); 
            lbDictionary = new ListBox();
            btnToggleArchive = new Button();
            
            panelDivider = new Panel();

            ((ISupportInitialize)numMaxBooks).BeginInit(); ((ISupportInitialize)numShortLoan).BeginInit();
            ((ISupportInitialize)numLongLoan).BeginInit(); ((ISupportInitialize)numBanDays).BeginInit();
            ((ISupportInitialize)numPenaltyBase).BeginInit(); ((ISupportInitialize)numPenaltyPercent).BeginInit();
            ((ISupportInitialize)numNewBookAge).BeginInit(); ((ISupportInitialize)numRareCopies).BeginInit();
            SuspendLayout();
            
            // ================= ЛІВА ЧАСТИНА (НАЛАШТУВАННЯ) =================
            lblTitleSettings.AutoSize = true; 
            lblTitleSettings.Font = Fonts.Bold16; 
            lblTitleSettings.Location = new Point(20, 20); 
            lblTitleSettings.Text = $"{UIIcons.Settings} Глобальні параметри системи";

            int y = 80; int gap = 50;

            lblMaxBooks.AutoSize = true; lblMaxBooks.Location = new Point(20, y); lblMaxBooks.Text = "Ліміт книг \"на руках\":";
            numMaxBooks.Location = new Point(310, y - 2); numMaxBooks.Size = new Size(120, 32); numMaxBooks.Minimum = 1;

            y += gap;
            lblShortLoan.AutoSize = true; lblShortLoan.Location = new Point(20, y); lblShortLoan.Text = "Дедлайн для рідкісних (днів):";
            numShortLoan.Location = new Point(310, y - 2); numShortLoan.Size = new Size(120, 32); numShortLoan.Minimum = 1;

            y += gap;
            lblLongLoan.AutoSize = true; lblLongLoan.Location = new Point(20, y); lblLongLoan.Text = "Дедлайн стандартний (днів):";
            numLongLoan.Location = new Point(310, y - 2); numLongLoan.Size = new Size(120, 32); numLongLoan.Minimum = 1;

            y += gap;
            lblBanDays.AutoSize = true; lblBanDays.Location = new Point(20, y); lblBanDays.Text = "Тривалість бану (днів):";
            numBanDays.Location = new Point(310, y - 2); numBanDays.Size = new Size(120, 32); numBanDays.Minimum = 0;

            y += gap;
            lblPenaltyBase.AutoSize = true; lblPenaltyBase.Location = new Point(20, y); lblPenaltyBase.Text = "Базовий штраф (грн):";
            numPenaltyBase.Location = new Point(310, y - 2); numPenaltyBase.DecimalPlaces = 2; numPenaltyBase.Size = new Size(120, 32); numPenaltyBase.Minimum = 0; numPenaltyBase.Maximum = 10000;

            y += gap;
            lblPenaltyPercent.AutoSize = true; lblPenaltyPercent.Location = new Point(20, y); lblPenaltyPercent.Text = "Пеня від ціни книги (%):";
            numPenaltyPercent.Location = new Point(310, y - 2); numPenaltyPercent.DecimalPlaces = 2; numPenaltyPercent.Size = new Size(120, 32); numPenaltyPercent.Minimum = 0; numPenaltyPercent.Maximum = 100;

            y += gap;
            lblNewBookAge.AutoSize = true; lblNewBookAge.Location = new Point(20, y); lblNewBookAge.Text = "Вік \"нової\" книги (років):";
            numNewBookAge.Location = new Point(310, y - 2); numNewBookAge.Size = new Size(120, 32); numNewBookAge.Minimum = 0;

            y += gap;
            lblRareCopies.AutoSize = true; lblRareCopies.Location = new Point(20, y); lblRareCopies.Text = "Поріг \"рідкісної\" книги (шт):";
            numRareCopies.Location = new Point(310, y - 2); numRareCopies.Size = new Size(120, 32); numRareCopies.Minimum = 1;

            y += gap + 20;
            btnCancelSettings.BackColor = UIColors.ButtonSecondary; btnCancelSettings.FlatStyle = FlatStyle.Flat; btnCancelSettings.FlatAppearance.BorderSize = 0;
            btnCancelSettings.Location = new Point(20, y); btnCancelSettings.Size = new Size(180, 40); btnCancelSettings.Text = "Скасувати зміни"; btnCancelSettings.Click += btnCancelSettings_Click;

            btnSaveSettings.BackColor = Color.FromArgb(51, 51, 76); btnSaveSettings.ForeColor = UIColors.White; btnSaveSettings.FlatStyle = FlatStyle.Flat; btnSaveSettings.FlatAppearance.BorderSize = 0;
            btnSaveSettings.Location = new Point(250, y); btnSaveSettings.Size = new Size(180, 40); btnSaveSettings.Text = "Зберегти"; btnSaveSettings.Click += btnSaveSettings_Click;

            // Вертикальний розділювач
            panelDivider.BackColor = Color.LightGray; panelDivider.Location = new Point(470, 20); panelDivider.Size = new Size(1, 600);

            // ================= ПРАВА ЧАСТИНА =================
            lblTitleArchive.AutoSize = true; lblTitleArchive.Font = Fonts.Bold16; lblTitleArchive.Location = new Point(500, 20); lblTitleArchive.Text = $"{UIIcons.Settings} Довідники";

            cmbDictType.DropDownStyle = ComboBoxStyle.DropDownList; cmbDictType.Location = new Point(500, 78); cmbDictType.Size = new Size(200, 33);
            cmbDictType.SelectedIndexChanged += cmbDictType_SelectedIndexChanged;

            chkShowArchived.AutoSize = true; chkShowArchived.Location = new Point(720, 80); chkShowArchived.Text = "Архівовані"; chkShowArchived.CheckedChanged += chkShowArchived_CheckedChanged;

            // ПОШУК
            txtSearchDict.Location = new Point(500, 128);
            txtSearchDict.Size = new Size(400, 32);
            txtSearchDict.PlaceholderText = $"{UIIcons.Search} Швидкий пошук...";
            txtSearchDict.TextChanged += txtSearchDict_TextChanged;

            lbDictionary.Location = new Point(500, 178); 
            lbDictionary.Size = new Size(400, 300); 

            // Кнопка Архіву / Видалення
            btnToggleArchive.FlatStyle = FlatStyle.Flat; btnToggleArchive.FlatAppearance.BorderSize = 0; btnToggleArchive.ForeColor = UIColors.White;
            btnToggleArchive.Location = new Point(500, y); btnToggleArchive.Size = new Size(400, 40); btnToggleArchive.Click += btnToggleArchive_Click;

            // Форма
            ClientSize = new Size(950, 650);
            Controls.Add(panelDivider); Controls.Add(lblTitleSettings); Controls.Add(lblMaxBooks); Controls.Add(numMaxBooks); Controls.Add(lblShortLoan); Controls.Add(numShortLoan);
            Controls.Add(lblLongLoan); Controls.Add(numLongLoan); Controls.Add(lblBanDays); Controls.Add(numBanDays); Controls.Add(lblPenaltyBase); Controls.Add(numPenaltyBase);
            Controls.Add(lblPenaltyPercent); Controls.Add(numPenaltyPercent); Controls.Add(lblNewBookAge); Controls.Add(numNewBookAge); Controls.Add(lblRareCopies); Controls.Add(numRareCopies);
            Controls.Add(btnSaveSettings); Controls.Add(btnCancelSettings); Controls.Add(lblTitleArchive); Controls.Add(cmbDictType); Controls.Add(chkShowArchived); 
            Controls.Add(txtSearchDict); Controls.Add(lbDictionary); Controls.Add(btnToggleArchive); 
            
            Font = Fonts.Regular14;
            FormBorderStyle = FormBorderStyle.None; BackColor = UIColors.White; Text = "Налаштування";

            ((ISupportInitialize)numMaxBooks).EndInit(); ((ISupportInitialize)numShortLoan).EndInit();
            ((ISupportInitialize)numLongLoan).EndInit(); ((ISupportInitialize)numBanDays).EndInit();
            ((ISupportInitialize)numPenaltyBase).EndInit(); ((ISupportInitialize)numPenaltyPercent).EndInit();
            ((ISupportInitialize)numNewBookAge).EndInit(); ((ISupportInitialize)numRareCopies).EndInit();
            ResumeLayout(false); PerformLayout();
        }

        private Label lblTitleSettings;
        private Label lblMaxBooks; private NumericUpDown numMaxBooks;
        private Label lblShortLoan; private NumericUpDown numShortLoan;
        private Label lblLongLoan; private NumericUpDown numLongLoan;
        private Label lblBanDays; private NumericUpDown numBanDays;
        private Label lblPenaltyBase; private NumericUpDown numPenaltyBase;
        private Label lblPenaltyPercent; private NumericUpDown numPenaltyPercent;
        private Label lblNewBookAge; private NumericUpDown numNewBookAge;
        private Label lblRareCopies; private NumericUpDown numRareCopies;
        private Button btnSaveSettings; private Button btnCancelSettings;

        private Label lblTitleArchive; private ComboBox cmbDictType; private CheckBox chkShowArchived;
        private TextBox txtSearchDict;
        private ListBox lbDictionary; private Button btnToggleArchive;
        private Panel panelDivider;
    }
}