namespace LibSys
{
    public partial class MainForm : Form
    {
        public static MainForm Instance { get; private set; } = null!;

        private readonly Dictionary<Type, Form> _formCache = new Dictionary<Type, Form>();

        public MainForm()
        {
            InitializeComponent();
            Instance = this;

            WindowState = FormWindowState.Maximized;
            MinimumSize = new Size(1250, 780);
        }

        public T GetCachedForm<T>() where T : Form, new()
        {
            Type type = typeof(T);
            if (!_formCache.ContainsKey(type))
            {
                T newForm = new T();
                newForm.TopLevel = false;
                newForm.FormBorderStyle = FormBorderStyle.None;
                newForm.Dock = DockStyle.Fill;
                panelContent.Controls.Add(newForm);
                _formCache[type] = newForm;
            }

            return (T)_formCache[type];
        }

        public void ShowForm(Form form, string title)
        {
            foreach (Control ctrl in panelContent.Controls)
            {
                if (ctrl is Form f && f != form) f.Hide();
            }

            lblTitle.Text = title;
            lblWelcome.Visible = false;

            form.Show();
            form.BringToFront();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            foreach (Control ctrl in panelContent.Controls)
                if (ctrl is Form f)
                    f.Hide();

            lblTitle.Text = "Головна панель";
            lblWelcome.Visible = true;
        }

        private void btnReaders_Click(object sender, EventArgs e)
        {
            var form = GetCachedForm<ReadersForm>();
            form.RefreshData();
            ShowForm(form, "Управління читачами");
        }

        private void btnCatalog_Click(object sender, EventArgs e)
        {
            var form = GetCachedForm<CatalogForm>();
            form.RefreshData();
            ShowForm(form, "Каталог книг");
        }

        private void btnInventory_Click(object sender, EventArgs e)
        {
            var form = GetCachedForm<InventoryForm>();
            form.RefreshData();
            ShowForm(form, "Інвентар (Примірники)");
        }

        private void btnJournal_Click(object sender, EventArgs e)
        {
            var form = GetCachedForm<JournalForm>();
            form.RefreshData();
            ShowForm(form, "Журнал операцій");
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            var form = GetCachedForm<SettingsForm>();
            form.LoadSettings();
            form.RefreshDictionary();
            ShowForm(form, "Налаштування та Довідники");
        }
    }
}