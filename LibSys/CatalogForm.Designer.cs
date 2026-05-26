using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LibSys.Models;

namespace LibSys
{
    public class SmartScrollPanel : Panel
    {
        protected override Point ScrollToControl(Control activeControl)
        {
            return this.AutoScrollPosition;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            int scrollAmount = 189;
            int currentY = Math.Abs(this.AutoScrollPosition.Y);
            
            int newY = e.Delta > 0 ? currentY - scrollAmount : currentY + scrollAmount;
            
            if (newY < 0) newY = 0;
            
            this.AutoScrollPosition = new Point(0, newY);
            
            if (e is HandledMouseEventArgs he)
            {
                he.Handled = true;
            }
        }
    }

    partial class CatalogForm
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
            cmbSort = new ComboBox();
            lblCopiesInfo = new Label();
            btnAddBook = new Button();
            btnEditBook = new Button();
            btnAddCopy = new Button(); 
            dgvBooks = new DataGridView();
            
            panelRightBase = new Panel();
            panelCover = new Panel();
            pbCover = new PictureBox();
            lblNoCover = new Label();
            
            panelScrollFilters = new SmartScrollPanel();
            
            txtSearchAuthorFilter = new TextBox();
            btnClearAuthorFilter = new Button();
            clbAuthorsFilter = new CheckedListBox();
            
            txtSearchPublisherFilter = new TextBox();
            btnClearPublisherFilter = new Button();
            clbPublishersFilter = new CheckedListBox();
            
            txtSearchYearFilter = new TextBox();
            btnClearYearFilter = new Button();
            clbYearsFilter = new CheckedListBox();

            txtSearchTagFilter = new TextBox();
            btnClearTagFilter = new Button();
            clbTagsFilter = new CheckedListBox();

            panelTop.SuspendLayout();
            ((ISupportInitialize)dgvBooks).BeginInit();
            panelRightBase.SuspendLayout();
            panelCover.SuspendLayout();
            ((ISupportInitialize)pbCover).BeginInit();
            panelScrollFilters.SuspendLayout();
            SuspendLayout();
            
            // ================= ВЕРХНЯ ПАНЕЛЬ КОНТРОЛЕРІВ =================
            panelTop.BackColor = UIColors.White;
            panelTop.Controls.Add(btnAddCopy);
            panelTop.Controls.Add(btnEditBook);
            panelTop.Controls.Add(btnAddBook);
            panelTop.Controls.Add(lblCopiesInfo);
            panelTop.Controls.Add(cmbSort);
            panelTop.Controls.Add(txtSearch);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Size = new Size(1160, 80);
            
            txtSearch.Location = new Point(20, 24);
            txtSearch.Size = new Size(300, 32);
            txtSearch.PlaceholderText = $"{UIIcons.Search} Пошук за назвою...";

            cmbSort.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSort.Location = new Point(340, 24);
            cmbSort.Size = new Size(250, 33);
            
            lblCopiesInfo.AutoSize = true;
            lblCopiesInfo.Location = new Point(605, 27);
            lblCopiesInfo.Font = Fonts.Bold14;
            lblCopiesInfo.Text = "";
            
            btnAddBook.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAddBook.BackColor = UIColors.Success;
            btnAddBook.FlatAppearance.BorderSize = 0;
            btnAddBook.FlatStyle = FlatStyle.Flat;
            btnAddBook.ForeColor = UIColors.White;
            btnAddBook.Location = new Point(690, 20);
            btnAddBook.Size = new Size(130, 40);
            btnAddBook.Text = $"{UIIcons.Add} Книга";
            
            btnEditBook.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnEditBook.BackColor = UIColors.Info;
            btnEditBook.FlatAppearance.BorderSize = 0;
            btnEditBook.FlatStyle = FlatStyle.Flat;
            btnEditBook.ForeColor = UIColors.White;
            btnEditBook.Location = new Point(830, 20);
            btnEditBook.Size = new Size(130, 40);
            btnEditBook.Text = $"{UIIcons.Edit} Редаг.";

            btnAddCopy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAddCopy.BackColor = Color.Teal; 
            btnAddCopy.FlatAppearance.BorderSize = 0;
            btnAddCopy.FlatStyle = FlatStyle.Flat;
            btnAddCopy.ForeColor = UIColors.White;
            btnAddCopy.Location = new Point(970, 20);
            btnAddCopy.Size = new Size(170, 40);
            btnAddCopy.Text = $"{UIIcons.Copies} Примірники";
            btnAddCopy.Click += btnAddCopy_Click;

            // ================= ПРАВИЙ БАЗОВИЙ БЛОК =================
            panelRightBase.BackColor = UIColors.BackgroundAlt;
            panelRightBase.Dock = DockStyle.Right;
            panelRightBase.Size = new Size(401, 640);

            // ================= СТАТИЧНИЙ БЛОК ОБКЛАДИНКИ =================
            panelCover.Dock = DockStyle.Top;
            panelCover.Size = new Size(401, 591); 
            panelCover.Controls.Add(lblNoCover);
            panelCover.Controls.Add(pbCover);

            pbCover.Location = new Point(10, 10); 
            pbCover.Size = new Size(381, 571);    
            pbCover.BackColor = Color.LightGray;
            pbCover.SizeMode = PictureBoxSizeMode.Zoom;
            
            lblNoCover.AutoSize = false;
            lblNoCover.Size = new Size(381, 40);
            lblNoCover.Location = new Point(10, 275); 
            lblNoCover.TextAlign = ContentAlignment.MiddleCenter;
            lblNoCover.ForeColor = UIColors.TextMuted;
            lblNoCover.Font = Fonts.Italic14;
            lblNoCover.Text = $"{UIIcons.NoCover} Зображення відсутнє";
            lblNoCover.BackColor = Color.LightGray;

            // ================= ДИНАМІЧНА ПАНЕЛЬ ФІЛЬТРІВ =================
            panelScrollFilters.Dock = DockStyle.Fill; 
            panelScrollFilters.AutoScroll = true; 
            panelScrollFilters.Padding = new Padding(10, 0, 10, 10);

            int startX = 10;
            int inputW = 320;
            int clearX = 335;
            int listWidth = 357; 
            
            int listHeight = 140; 
            int blockOffset = 189; 

            // --- Фільтр: Автори ---
            int yPos = 10;
            txtSearchAuthorFilter.Location = new Point(startX, yPos);
            txtSearchAuthorFilter.Size = new Size(inputW, 32);
            txtSearchAuthorFilter.PlaceholderText = $"{UIIcons.User} Автор...";
            
            btnClearAuthorFilter.Location = new Point(clearX, yPos);
            btnClearAuthorFilter.Size = new Size(32, 32);
            btnClearAuthorFilter.Text = "✖";
            btnClearAuthorFilter.FlatStyle = FlatStyle.Flat;
            btnClearAuthorFilter.FlatAppearance.BorderSize = 1;

            clbAuthorsFilter.FormattingEnabled = true;
            clbAuthorsFilter.Location = new Point(startX, yPos + 40);
            clbAuthorsFilter.Size = new Size(listWidth, listHeight); 
            clbAuthorsFilter.CheckOnClick = true;

            // --- Фільтр: Видавництва ---
            yPos += blockOffset;
            txtSearchPublisherFilter.Location = new Point(startX, yPos);
            txtSearchPublisherFilter.Size = new Size(inputW, 32);
            txtSearchPublisherFilter.PlaceholderText = $"{UIIcons.Inventory} Видавництво...";
            
            btnClearPublisherFilter.Location = new Point(clearX, yPos);
            btnClearPublisherFilter.Size = new Size(32, 32);
            btnClearPublisherFilter.Text = "✖";
            btnClearPublisherFilter.FlatStyle = FlatStyle.Flat;
            btnClearPublisherFilter.FlatAppearance.BorderSize = 1;

            clbPublishersFilter.FormattingEnabled = true;
            clbPublishersFilter.Location = new Point(startX, yPos + 40);
            clbPublishersFilter.Size = new Size(listWidth, listHeight); 
            clbPublishersFilter.CheckOnClick = true;

            // --- Фільтр: Роки ---
            yPos += blockOffset;
            txtSearchYearFilter.Location = new Point(startX, yPos);
            txtSearchYearFilter.Size = new Size(inputW, 32);
            txtSearchYearFilter.PlaceholderText = "📅 Рік...";

            btnClearYearFilter.Location = new Point(clearX, yPos);
            btnClearYearFilter.Size = new Size(32, 32);
            btnClearYearFilter.Text = "✖";
            btnClearYearFilter.FlatStyle = FlatStyle.Flat;
            btnClearYearFilter.FlatAppearance.BorderSize = 1;

            clbYearsFilter.FormattingEnabled = true;
            clbYearsFilter.Location = new Point(startX, yPos + 40);
            clbYearsFilter.Size = new Size(listWidth, listHeight); 
            clbYearsFilter.CheckOnClick = true;

            // --- Фільтр: Теги ---
            yPos += blockOffset;
            txtSearchTagFilter.Location = new Point(startX, yPos);
            txtSearchTagFilter.Size = new Size(inputW, 32);
            txtSearchTagFilter.PlaceholderText = $"{UIIcons.Catalog} Тег / Жанр...";

            btnClearTagFilter.Location = new Point(clearX, yPos);
            btnClearTagFilter.Size = new Size(32, 32);
            btnClearTagFilter.Text = "✖";
            btnClearTagFilter.FlatStyle = FlatStyle.Flat;
            btnClearTagFilter.FlatAppearance.BorderSize = 1;

            clbTagsFilter.FormattingEnabled = true;
            clbTagsFilter.Location = new Point(startX, yPos + 40);
            clbTagsFilter.Size = new Size(listWidth, listHeight); 
            clbTagsFilter.CheckOnClick = true;

            panelScrollFilters.Controls.Add(txtSearchAuthorFilter); panelScrollFilters.Controls.Add(btnClearAuthorFilter); panelScrollFilters.Controls.Add(clbAuthorsFilter);
            panelScrollFilters.Controls.Add(txtSearchPublisherFilter); panelScrollFilters.Controls.Add(btnClearPublisherFilter); panelScrollFilters.Controls.Add(clbPublishersFilter);
            panelScrollFilters.Controls.Add(txtSearchYearFilter); panelScrollFilters.Controls.Add(btnClearYearFilter); panelScrollFilters.Controls.Add(clbYearsFilter);
            panelScrollFilters.Controls.Add(txtSearchTagFilter); panelScrollFilters.Controls.Add(btnClearTagFilter); panelScrollFilters.Controls.Add(clbTagsFilter);

            panelRightBase.Controls.Add(panelScrollFilters); 
            panelRightBase.Controls.Add(panelCover); 

            // ================= ІНФОРМАЦІЙНА ТАБЛИЦЯ =================
            dgvBooks.AllowUserToAddRows = false;
            dgvBooks.AllowUserToDeleteRows = false;
            dgvBooks.BackgroundColor = UIColors.BackgroundAlt;
            dgvBooks.BorderStyle = BorderStyle.None;
            dgvBooks.Dock = DockStyle.Fill; 
            dgvBooks.Location = new Point(0, 80);
            dgvBooks.ReadOnly = true;
            dgvBooks.RowHeadersVisible = false;
            dgvBooks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBooks.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvBooks.ColumnHeadersHeight = 45;
            
            dgvBooks.DefaultCellStyle.Font = Fonts.Regular14;
            dgvBooks.ColumnHeadersDefaultCellStyle.Font = Fonts.Bold14;
            
            dgvBooks.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvBooks.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvBooks.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            
            ClientSize = new Size(1160, 720); 
            Controls.Add(dgvBooks);
            Controls.Add(panelRightBase); 
            Controls.Add(panelTop);
            Font = Fonts.Regular14;
            FormBorderStyle = FormBorderStyle.None;
            Text = "Каталог книг";
            
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            ((ISupportInitialize)dgvBooks).EndInit();
            panelRightBase.ResumeLayout(false);
            panelCover.ResumeLayout(false);
            ((ISupportInitialize)pbCover).EndInit();
            panelScrollFilters.ResumeLayout(false);
            panelScrollFilters.PerformLayout();
            ResumeLayout(false);
        }

        private Panel panelTop;
        private TextBox txtSearch;
        private ComboBox cmbSort;
        private Button btnAddBook;
        private Button btnEditBook;
        private Button btnAddCopy; 
        private DataGridView dgvBooks;
        
        private Panel panelRightBase;
        private Panel panelCover;
        private PictureBox pbCover;
        private Label lblNoCover;
        
        private SmartScrollPanel panelScrollFilters;
        
        private TextBox txtSearchAuthorFilter;
        private Button btnClearAuthorFilter;
        private CheckedListBox clbAuthorsFilter;
        
        private TextBox txtSearchPublisherFilter;
        private Button btnClearPublisherFilter;
        private CheckedListBox clbPublishersFilter;
        
        private TextBox txtSearchYearFilter;
        private Button btnClearYearFilter;
        private CheckedListBox clbYearsFilter;

        private TextBox txtSearchTagFilter;
        private Button btnClearTagFilter;
        private CheckedListBox clbTagsFilter;
        private Label lblCopiesInfo;
    }
}