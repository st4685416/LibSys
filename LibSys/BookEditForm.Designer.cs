using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LibSys.Models;

namespace LibSys
{
    partial class BookEditForm
    {
        private IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            pbCoverEdit = new PictureBox();
            btnUploadCover = new Button();

            lblTitle = new Label(); txtTitle = new TextBox();
            lblYear = new Label(); numYear = new NumericUpDown();
            lblPrice = new Label(); numPrice = new NumericUpDown();
            
            lblAuthors = new Label(); txtSearchAuthor = new TextBox(); clbAuthors = new CheckedListBox(); btnQuickAddAuthor = new Button();
            panelAddAuthor = new Panel(); lblNewAuthLast = new Label(); txtNewAuthLast = new TextBox();
            lblNewAuthFirst = new Label(); txtNewAuthFirst = new TextBox(); lblNewAuthMiddle = new Label(); txtNewAuthMiddle = new TextBox();
            lblAuthOr = new Label(); lblNewAuthPseudo = new Label(); txtNewAuthPseudo = new TextBox(); btnSaveAuth = new Button(); btnCancelAuth = new Button();

            lblPublisher = new Label(); txtSearchPublisher = new TextBox(); clbPublishers = new CheckedListBox(); btnQuickAddPublisher = new Button();
            panelAddPublisher = new Panel(); lblNewPubName = new Label(); txtNewPubName = new TextBox(); btnSavePub = new Button(); btnCancelPub = new Button();

            lblTags = new Label(); txtSearchTag = new TextBox(); clbTags = new CheckedListBox(); btnQuickAddTag = new Button();
            panelAddTag = new Panel(); lblNewTagName = new Label(); txtNewTagName = new TextBox(); btnSaveTag = new Button(); btnCancelTag = new Button();

            btnSave = new Button(); btnCancel = new Button();

            ((ISupportInitialize)pbCoverEdit).BeginInit();
            ((ISupportInitialize)numYear).BeginInit();
            ((ISupportInitialize)numPrice).BeginInit();
            panelAddPublisher.SuspendLayout();
            panelAddAuthor.SuspendLayout();
            panelAddTag.SuspendLayout();
            SuspendLayout();
            
            // ================= КООРДИНАТИ СТОВПЧИКІВ =================
            int col1 = 20;  
            int col2 = 420; 
            int col3 = 820; 

            // ================= ЛІВИЙ СТОВПЧИК =================
            lblTitle.AutoSize = true; lblTitle.Location = new Point(col1, 20); lblTitle.Text = "Назва книги *"; lblTitle.Font = Fonts.Bold14;
            txtTitle.Location = new Point(col1, 50); txtTitle.Size = new Size(380, 32);
            
            lblYear.AutoSize = true; lblYear.Location = new Point(col1, 95); lblYear.Text = "Рік видання *"; lblYear.Font = Fonts.Bold14;
            numYear.Location = new Point(col1, 125); numYear.Maximum = new decimal(new int[] { 2100, 0, 0, 0 }); numYear.Minimum = new decimal(new int[] { 1800, 0, 0, 0 }); numYear.Size = new Size(180, 32);
            
            lblPrice.AutoSize = true; lblPrice.Location = new Point(col1 + 200, 95); lblPrice.Text = "Вартість (грн) *"; lblPrice.Font = Fonts.Bold14;
            numPrice.Location = new Point(col1 + 200, 125); numPrice.Maximum = new decimal(new int[] { 100000, 0, 0, 0 }); numPrice.Size = new Size(180, 32);
            
            lblAuthors.AutoSize = true; lblAuthors.Location = new Point(col1, 170); lblAuthors.Text = "Автори *"; lblAuthors.Font = Fonts.Bold14;
            txtSearchAuthor.Location = new Point(col1, 200); txtSearchAuthor.Size = new Size(330, 32); txtSearchAuthor.PlaceholderText = $"{UIIcons.Search} Пошук автора..."; txtSearchAuthor.TextChanged += txtSearchAuthor_TextChanged;
            btnQuickAddAuthor.Location = new Point(col1 + 340, 200); btnQuickAddAuthor.Size = new Size(40, 32); btnQuickAddAuthor.Text = UIIcons.Add; btnQuickAddAuthor.BackColor = UIColors.Success; btnQuickAddAuthor.ForeColor = UIColors.White; btnQuickAddAuthor.FlatStyle = FlatStyle.Flat; btnQuickAddAuthor.FlatAppearance.BorderSize = 0; btnQuickAddAuthor.Click += btnQuickAddAuthor_Click;

            clbAuthors.FormattingEnabled = true; clbAuthors.CheckOnClick = true; clbAuthors.Location = new Point(col1, 240); clbAuthors.Size = new Size(380, 230);

            // ПАНЕЛЬ ДОДАВАННЯ АВТОРА
            panelAddAuthor.BackColor = UIColors.BackgroundAlt; panelAddAuthor.Location = new Point(col1, 240); panelAddAuthor.Size = new Size(380, 230); panelAddAuthor.Visible = false;
            lblNewAuthLast.AutoSize = true; lblNewAuthLast.Location = new Point(15, 5); lblNewAuthLast.Text = "Прізвище *"; lblNewAuthLast.Font = Fonts.Bold14;
            txtNewAuthLast.Location = new Point(15, 25); txtNewAuthLast.Size = new Size(160, 32);
            lblNewAuthFirst.AutoSize = true; lblNewAuthFirst.Location = new Point(190, 5); lblNewAuthFirst.Text = "Ім'я *"; lblNewAuthFirst.Font = Fonts.Bold14;
            txtNewAuthFirst.Location = new Point(190, 25); txtNewAuthFirst.Size = new Size(175, 32);
            lblNewAuthMiddle.AutoSize = true; lblNewAuthMiddle.Location = new Point(15, 60); lblNewAuthMiddle.Text = "По батькові"; lblNewAuthMiddle.Font = Fonts.Bold14;
            txtNewAuthMiddle.Location = new Point(15, 80); txtNewAuthMiddle.Size = new Size(350, 32);
            lblAuthOr.AutoSize = true; lblAuthOr.Location = new Point(140, 115); lblAuthOr.Text = "— І/АБО —"; lblAuthOr.Font = Fonts.Bold14; lblAuthOr.ForeColor = UIColors.TextMuted;
            lblNewAuthPseudo.AutoSize = true; lblNewAuthPseudo.Location = new Point(15, 135); lblNewAuthPseudo.Text = "Псевдонім *"; lblNewAuthPseudo.Font = Fonts.Bold14;
            txtNewAuthPseudo.Location = new Point(15, 155); txtNewAuthPseudo.Size = new Size(350, 32);
            btnCancelAuth.Location = new Point(15, 190); btnCancelAuth.Size = new Size(160, 35); btnCancelAuth.Text = "Скасувати"; btnCancelAuth.Click += btnCancelAuth_Click; btnCancelAuth.BackColor = UIColors.ButtonSecondary; btnCancelAuth.FlatStyle = FlatStyle.Flat; btnCancelAuth.FlatAppearance.BorderSize = 0;
            btnSaveAuth.Location = new Point(190, 190); btnSaveAuth.Size = new Size(175, 35); btnSaveAuth.Text = "Зберегти"; btnSaveAuth.Click += btnSaveAuth_Click; btnSaveAuth.BackColor = UIColors.Success; btnSaveAuth.ForeColor = UIColors.White; btnSaveAuth.FlatStyle = FlatStyle.Flat; btnSaveAuth.FlatAppearance.BorderSize = 0;
            panelAddAuthor.Controls.Add(lblNewAuthLast); panelAddAuthor.Controls.Add(txtNewAuthLast); panelAddAuthor.Controls.Add(lblNewAuthFirst); panelAddAuthor.Controls.Add(txtNewAuthFirst); panelAddAuthor.Controls.Add(lblNewAuthMiddle); panelAddAuthor.Controls.Add(txtNewAuthMiddle); panelAddAuthor.Controls.Add(lblAuthOr); panelAddAuthor.Controls.Add(lblNewAuthPseudo); panelAddAuthor.Controls.Add(txtNewAuthPseudo); panelAddAuthor.Controls.Add(btnSaveAuth); panelAddAuthor.Controls.Add(btnCancelAuth);

            // ================= ЦЕНТРАЛЬНИЙ СТОВПЧИК =================
            lblPublisher.AutoSize = true; lblPublisher.Location = new Point(col2, 20); lblPublisher.Text = "Видавництво *"; lblPublisher.Font = Fonts.Bold14;
            txtSearchPublisher.Location = new Point(col2, 50); txtSearchPublisher.Size = new Size(330, 32); txtSearchPublisher.PlaceholderText = $"{UIIcons.Search} Пошук видавництва..."; txtSearchPublisher.TextChanged += txtSearchPublisher_TextChanged;
            btnQuickAddPublisher.Location = new Point(col2 + 340, 50); btnQuickAddPublisher.Size = new Size(40, 32); btnQuickAddPublisher.Text = UIIcons.Add; btnQuickAddPublisher.BackColor = UIColors.Success; btnQuickAddPublisher.ForeColor = UIColors.White; btnQuickAddPublisher.FlatStyle = FlatStyle.Flat; btnQuickAddPublisher.FlatAppearance.BorderSize = 0; btnQuickAddPublisher.Click += btnQuickAddPublisher_Click;

            clbPublishers.FormattingEnabled = true; clbPublishers.CheckOnClick = true; clbPublishers.Location = new Point(col2, 90); clbPublishers.Size = new Size(380, 150);

            // ПАНЕЛЬ ДОДАВАННЯ ВИДАВНИЦТВА
            panelAddPublisher.BackColor = UIColors.BackgroundAlt; panelAddPublisher.Location = new Point(col2, 90); panelAddPublisher.Size = new Size(380, 150); panelAddPublisher.Visible = false;
            lblNewPubName.AutoSize = true; lblNewPubName.Location = new Point(20, 15); lblNewPubName.Text = "Нове видавництво *"; lblNewPubName.Font = Fonts.Bold14;
            txtNewPubName.Location = new Point(20, 45); txtNewPubName.Size = new Size(340, 32);
            btnCancelPub.Location = new Point(20, 95); btnCancelPub.Size = new Size(160, 40); btnCancelPub.Text = "Скасувати"; btnCancelPub.Click += btnCancelPub_Click; btnCancelPub.BackColor = UIColors.ButtonSecondary; btnCancelPub.FlatStyle = FlatStyle.Flat; btnCancelPub.FlatAppearance.BorderSize = 0;
            btnSavePub.Location = new Point(200, 95); btnSavePub.Size = new Size(160, 40); btnSavePub.Text = "Зберегти"; btnSavePub.Click += btnSavePub_Click; btnSavePub.BackColor = UIColors.Success; btnSavePub.ForeColor = UIColors.White; btnSavePub.FlatStyle = FlatStyle.Flat; btnSavePub.FlatAppearance.BorderSize = 0;
            panelAddPublisher.Controls.Add(lblNewPubName); panelAddPublisher.Controls.Add(txtNewPubName); panelAddPublisher.Controls.Add(btnSavePub); panelAddPublisher.Controls.Add(btnCancelPub);

            lblTags.AutoSize = true; lblTags.Location = new Point(col2, 251); lblTags.Text = "Жанри / Теги"; lblTags.Font = Fonts.Bold14;
            txtSearchTag.Location = new Point(col2, 281); txtSearchTag.Size = new Size(330, 32); txtSearchTag.PlaceholderText = $"{UIIcons.Search} Пошук тегів..."; txtSearchTag.TextChanged += txtSearchTag_TextChanged;
            btnQuickAddTag.Location = new Point(col2 + 340, 281); btnQuickAddTag.Size = new Size(40, 32); btnQuickAddTag.Text = UIIcons.Add; btnQuickAddTag.BackColor = UIColors.Success; btnQuickAddTag.ForeColor = UIColors.White; btnQuickAddTag.FlatStyle = FlatStyle.Flat; btnQuickAddTag.FlatAppearance.BorderSize = 0; btnQuickAddTag.Click += btnQuickAddTag_Click;

            clbTags.FormattingEnabled = true; clbTags.CheckOnClick = true; clbTags.Location = new Point(col2, 321); clbTags.Size = new Size(380, 150);

            // ПАНЕЛЬ ДОДАВАННЯ ТЕГУ
            panelAddTag.BackColor = UIColors.BackgroundAlt; panelAddTag.Location = new Point(col2, 321); panelAddTag.Size = new Size(380, 150); panelAddTag.Visible = false;
            lblNewTagName.AutoSize = true; lblNewTagName.Location = new Point(20, 15); lblNewTagName.Text = "Назва нового тегу/жанру *"; lblNewTagName.Font = Fonts.Bold14;
            txtNewTagName.Location = new Point(20, 45); txtNewTagName.Size = new Size(340, 32);
            btnCancelTag.Location = new Point(20, 95); btnCancelTag.Size = new Size(160, 40); btnCancelTag.Text = "Скасувати"; btnCancelTag.Click += btnCancelTag_Click; btnCancelTag.BackColor = UIColors.ButtonSecondary; btnCancelTag.FlatStyle = FlatStyle.Flat; btnCancelTag.FlatAppearance.BorderSize = 0;
            btnSaveTag.Location = new Point(200, 95); btnSaveTag.Size = new Size(160, 40); btnSaveTag.Text = "Зберегти"; btnSaveTag.Click += btnSaveTag_Click; btnSaveTag.BackColor = UIColors.Success; btnSaveTag.ForeColor = UIColors.White; btnSaveTag.FlatStyle = FlatStyle.Flat; btnSaveTag.FlatAppearance.BorderSize = 0;
            panelAddTag.Controls.Add(lblNewTagName); panelAddTag.Controls.Add(txtNewTagName); panelAddTag.Controls.Add(btnSaveTag); panelAddTag.Controls.Add(btnCancelTag);

            // ================= ПРАВИЙ СТОВПЧИК (Обкладинка) =================
            pbCoverEdit.Location = new Point(col3, 20);
            pbCoverEdit.Size = new Size(293, 440); 
            pbCoverEdit.BackColor = UIColors.BackgroundAlt;
            pbCoverEdit.SizeMode = PictureBoxSizeMode.Zoom;
            pbCoverEdit.BorderStyle = BorderStyle.FixedSingle;

            btnUploadCover.Location = new Point(col3, 490);
            btnUploadCover.Size = new Size(294, 40);
            btnUploadCover.Text = "Обрати фото";
            btnUploadCover.BackColor = UIColors.Info;
            btnUploadCover.ForeColor = UIColors.White;
            btnUploadCover.FlatStyle = FlatStyle.Flat;
            btnUploadCover.FlatAppearance.BorderSize = 0;
            btnUploadCover.Click += btnUploadCover_Click;

            // ================= КНОПКИ ЗБЕРЕЖЕННЯ =================
            btnCancel.BackColor = UIColors.ButtonSecondary; btnCancel.FlatAppearance.BorderSize = 0; btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Location = new Point(col1, 490); btnCancel.Size = new Size(170, 40); btnCancel.Text = "Скасувати"; btnCancel.Click += btnCancel_Click;
            
            btnSave.BackColor = UIColors.Success; btnSave.FlatAppearance.BorderSize = 0; btnSave.FlatStyle = FlatStyle.Flat; btnSave.ForeColor = UIColors.White;
            btnSave.Location = new Point(col1 + 190, 490); btnSave.Size = new Size(200, 40); btnSave.Text = "Зберегти книгу"; btnSave.Click += btnSave_Click;
            
            // ================= ФОРМА =================
            ClientSize = new Size(1150, 560);
            Controls.Add(panelAddTag); Controls.Add(panelAddAuthor); Controls.Add(panelAddPublisher);
            Controls.Add(btnCancel); Controls.Add(btnSave); 
            
            Controls.Add(pbCoverEdit); Controls.Add(btnUploadCover); 
            
            Controls.Add(btnQuickAddTag); Controls.Add(clbTags); Controls.Add(txtSearchTag); Controls.Add(lblTags);
            Controls.Add(btnQuickAddAuthor); Controls.Add(clbAuthors); Controls.Add(txtSearchAuthor); Controls.Add(lblAuthors);
            Controls.Add(btnQuickAddPublisher); Controls.Add(clbPublishers); Controls.Add(txtSearchPublisher); Controls.Add(lblPublisher);
            Controls.Add(numPrice); Controls.Add(lblPrice); Controls.Add(numYear); Controls.Add(lblYear);
            Controls.Add(txtTitle); Controls.Add(lblTitle);
            
            Font = Fonts.Regular14;
            FormBorderStyle = FormBorderStyle.None;
            Text = "Редактор книги";
            BackColor = UIColors.White;
            
            ((ISupportInitialize)pbCoverEdit).EndInit();
            ((ISupportInitialize)numYear).EndInit();
            ((ISupportInitialize)numPrice).EndInit();
            panelAddPublisher.ResumeLayout(false); panelAddPublisher.PerformLayout();
            panelAddAuthor.ResumeLayout(false); panelAddAuthor.PerformLayout();
            panelAddTag.ResumeLayout(false); panelAddTag.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        
        private PictureBox pbCoverEdit;
        private Button btnUploadCover;

        private Label lblTitle; private TextBox txtTitle;
        private Label lblYear; private NumericUpDown numYear;
        private Label lblPrice; private NumericUpDown numPrice;
        
        private Label lblPublisher; private TextBox txtSearchPublisher; private CheckedListBox clbPublishers; private Button btnQuickAddPublisher;
        private Panel panelAddPublisher; private Label lblNewPubName; private TextBox txtNewPubName; private Button btnSavePub; private Button btnCancelPub;

        private Label lblAuthors; private TextBox txtSearchAuthor; private CheckedListBox clbAuthors; private Button btnQuickAddAuthor;
        private Panel panelAddAuthor; private Label lblNewAuthLast; private TextBox txtNewAuthLast;
        private Label lblNewAuthFirst; private TextBox txtNewAuthFirst; private Label lblNewAuthMiddle; private TextBox txtNewAuthMiddle;
        private Label lblAuthOr; private Label lblNewAuthPseudo; private TextBox txtNewAuthPseudo; private Button btnSaveAuth; private Button btnCancelAuth;

        private Label lblTags; private TextBox txtSearchTag; private CheckedListBox clbTags; private Button btnQuickAddTag;
        private Panel panelAddTag; private Label lblNewTagName; private TextBox txtNewTagName; private Button btnSaveTag; private Button btnCancelTag;

        private Button btnSave; private Button btnCancel;
    }
}