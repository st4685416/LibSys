using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LibSys.Models;

namespace LibSys;

partial class MainForm
{
    private IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        panelMenu = new Panel();
        btnSettings = new Button();
        btnJournal = new Button();
        btnInventory = new Button();
        btnCatalog = new Button();
        btnReaders = new Button();
        btnHome = new Button();
        panelLogo = new Panel();
        lblLogo = new Label();
        panelHeader = new Panel();
        lblTitle = new Label();
        panelContent = new Panel();
        lblWelcome = new Label();
        panelMenu.SuspendLayout();
        panelLogo.SuspendLayout();
        panelHeader.SuspendLayout();
        panelContent.SuspendLayout();
        SuspendLayout();
        
        // panelMenu
        panelMenu.BackColor = Color.FromArgb(39, 39, 58);
        panelMenu.Controls.Add(btnSettings);
        panelMenu.Controls.Add(btnJournal);
        panelMenu.Controls.Add(btnInventory);
        panelMenu.Controls.Add(btnCatalog);
        panelMenu.Controls.Add(btnReaders);
        panelMenu.Controls.Add(btnHome);
        panelMenu.Controls.Add(panelLogo);
        panelMenu.Dock = DockStyle.Left;
        panelMenu.Location = new Point(0, 0);
        panelMenu.Name = "panelMenu";
        panelMenu.Size = new Size(220, 561);
        panelMenu.TabIndex = 0;
        
        // btnSettings
        btnSettings.Dock = DockStyle.Bottom;
        btnSettings.FlatAppearance.BorderSize = 0;
        btnSettings.FlatStyle = FlatStyle.Flat;
        btnSettings.ForeColor = Color.Gainsboro;
        btnSettings.Font = Fonts.Regular14;
        btnSettings.Location = new Point(0, 501);
        btnSettings.Name = "btnSettings";
        btnSettings.Size = new Size(220, 60);
        btnSettings.Text = $"{UIIcons.Settings} Налаштування";
        btnSettings.UseVisualStyleBackColor = true;
        btnSettings.Click += btnSettings_Click;
        btnSettings.TextAlign = ContentAlignment.MiddleLeft;
        btnSettings.Padding = new Padding(15, 0, 0, 0); 
        
        // btnJournal
        btnJournal.Dock = DockStyle.Top;
        btnJournal.FlatAppearance.BorderSize = 0;
        btnJournal.FlatStyle = FlatStyle.Flat;
        btnJournal.ForeColor = Color.Gainsboro;
        btnJournal.Font = Fonts.Regular14;
        btnJournal.Location = new Point(0, 320);
        btnJournal.Name = "btnJournal";
        btnJournal.Size = new Size(220, 60);
        btnJournal.Text = $"{UIIcons.Journal} Журнал";
        btnJournal.UseVisualStyleBackColor = true;
        btnJournal.Click += btnJournal_Click;
        btnJournal.TextAlign = ContentAlignment.MiddleLeft;
        btnJournal.Padding = new Padding(15, 0, 0, 0); 

        // btnInventory
        btnInventory.Dock = DockStyle.Top;
        btnInventory.FlatAppearance.BorderSize = 0;
        btnInventory.FlatStyle = FlatStyle.Flat;
        btnInventory.ForeColor = Color.Gainsboro;
        btnInventory.Font = Fonts.Regular14;
        btnInventory.Location = new Point(0, 260);
        btnInventory.Name = "btnInventory";
        btnInventory.Size = new Size(220, 60);
        btnInventory.Text = $"{UIIcons.Inventory} Інвентар";
        btnInventory.UseVisualStyleBackColor = true;
        btnInventory.Click += btnInventory_Click;
        btnInventory.TextAlign = ContentAlignment.MiddleLeft;
        btnInventory.Padding = new Padding(15, 0, 0, 0); 
        
        // btnCatalog
        btnCatalog.Dock = DockStyle.Top;
        btnCatalog.FlatAppearance.BorderSize = 0;
        btnCatalog.FlatStyle = FlatStyle.Flat;
        btnCatalog.ForeColor = Color.Gainsboro;
        btnCatalog.Font = Fonts.Regular14;
        btnCatalog.Location = new Point(0, 200);
        btnCatalog.Name = "btnCatalog";
        btnCatalog.Size = new Size(220, 60);
        btnCatalog.Text = $"{UIIcons.Catalog} Каталог книг";
        btnCatalog.UseVisualStyleBackColor = true;
        btnCatalog.Click += btnCatalog_Click;
        btnCatalog.TextAlign = ContentAlignment.MiddleLeft;
        btnCatalog.Padding = new Padding(15, 0, 0, 0); 
        
        // btnReaders
        btnReaders.Dock = DockStyle.Top;
        btnReaders.FlatAppearance.BorderSize = 0;
        btnReaders.FlatStyle = FlatStyle.Flat;
        btnReaders.ForeColor = Color.Gainsboro;
        btnReaders.Font = Fonts.Regular14;
        btnReaders.Location = new Point(0, 140);
        btnReaders.Name = "btnReaders";
        btnReaders.Size = new Size(220, 60);
        btnReaders.Text = $"{UIIcons.User} Читачі";
        btnReaders.UseVisualStyleBackColor = true;
        btnReaders.Click += btnReaders_Click;
        btnReaders.TextAlign = ContentAlignment.MiddleLeft;
        btnReaders.Padding = new Padding(15, 0, 0, 0); 
        
        // btnHome
        btnHome.Dock = DockStyle.Top;
        btnHome.FlatAppearance.BorderSize = 0;
        btnHome.FlatStyle = FlatStyle.Flat;
        btnHome.ForeColor = Color.Gainsboro;
        btnHome.Font = Fonts.Regular14;
        btnHome.Location = new Point(0, 80);
        btnHome.Name = "btnHome";
        btnHome.Size = new Size(220, 60);
        btnHome.Text = $"{UIIcons.Home} Головна";
        btnHome.UseVisualStyleBackColor = true;
        btnHome.Click += btnHome_Click;
        btnHome.TextAlign = ContentAlignment.MiddleLeft;
        btnHome.Padding = new Padding(15, 0, 0, 0); 
        
        // panelLogo
        panelLogo.BackColor = Color.FromArgb(23, 23, 36);
        panelLogo.Controls.Add(lblLogo);
        panelLogo.Dock = DockStyle.Top;
        panelLogo.Location = new Point(0, 0);
        panelLogo.Name = "panelLogo";
        panelLogo.Size = new Size(220, 80);
        
        // lblLogo
        lblLogo.AutoSize = true;
        lblLogo.Font = Fonts.Bold16;
        lblLogo.ForeColor = UIColors.White;
        lblLogo.Location = new Point(55, 25);
        lblLogo.Name = "lblLogo";
        lblLogo.Text = "LibSys";
        
        // panelHeader
        panelHeader.BackColor = Color.FromArgb(51, 51, 76);
        panelHeader.Controls.Add(lblTitle);
        panelHeader.Dock = DockStyle.Top;
        panelHeader.Location = new Point(220, 0);
        panelHeader.Name = "panelHeader";
        panelHeader.Size = new Size(764, 80);
        
        // lblTitle
        lblTitle.AutoSize = true;
        lblTitle.Font = Fonts.Regular14;
        lblTitle.ForeColor = UIColors.White;
        lblTitle.Location = new Point(30, 25);
        lblTitle.Name = "lblTitle";
        lblTitle.Text = "Головна панель";
        
        // panelContent 
        panelContent.BackColor = UIColors.BackgroundAlt;
        panelContent.Controls.Add(lblWelcome);
        panelContent.Dock = DockStyle.Fill;
        panelContent.Location = new Point(220, 80);
        panelContent.Name = "panelContent";
        panelContent.Size = new Size(764, 481);
        panelContent.TabIndex = 1;
        
        // lblWelcome
        lblWelcome.Anchor = AnchorStyles.None;
        lblWelcome.AutoSize = true;
        lblWelcome.Font = Fonts.Regular14;
        lblWelcome.ForeColor = UIColors.TextMuted;
        lblWelcome.Location = new Point(200, 200);
        lblWelcome.Name = "lblWelcome";
        lblWelcome.Text = "Оберіть розділ у меню зліва для початку роботи";
        
        // MainForm
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(984, 561);
        Controls.Add(panelContent);
        Controls.Add(panelHeader);
        Controls.Add(panelMenu);
        MinimumSize = new Size(800, 500);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Система управління бібліотекою";
        panelMenu.ResumeLayout(false);
        panelLogo.ResumeLayout(false);
        panelLogo.PerformLayout();
        panelHeader.ResumeLayout(false);
        panelHeader.PerformLayout();
        panelContent.ResumeLayout(false);
        panelContent.PerformLayout();
        ResumeLayout(false);
    }

    private Panel panelMenu;
    private Panel panelLogo;
    private Button btnHome;
    private Button btnReaders;
    private Button btnCatalog;
    private Button btnInventory;
    private Button btnJournal;
    private Button btnSettings;
    private Panel panelHeader;
    private Label lblTitle;
    private Label lblLogo;
    private Panel panelContent;
    private Label lblWelcome;
}