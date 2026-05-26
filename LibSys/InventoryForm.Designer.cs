using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using LibSys.Models;

namespace LibSys
{
    partial class InventoryForm
    {
        private IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }
        
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            panelTop = new System.Windows.Forms.Panel();
            txtSearch = new System.Windows.Forms.TextBox();
            cmbSort = new System.Windows.Forms.ComboBox();
            btnWriteOff = new System.Windows.Forms.Button();
            chkAvailable = new System.Windows.Forms.CheckBox();
            chkLoaned = new System.Windows.Forms.CheckBox();
            chkLost = new System.Windows.Forms.CheckBox();
            chkWrittenOff = new System.Windows.Forms.CheckBox();
            dgvInventory = new System.Windows.Forms.DataGridView();
            panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvInventory).BeginInit();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.BackColor = UIColors.White;
            panelTop.Controls.Add(txtSearch);
            panelTop.Controls.Add(cmbSort);
            panelTop.Controls.Add(btnWriteOff);
            panelTop.Controls.Add(chkAvailable);
            panelTop.Controls.Add(chkLoaned);
            panelTop.Controls.Add(chkLost);
            panelTop.Controls.Add(chkWrittenOff);
            panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            panelTop.Location = new System.Drawing.Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new System.Drawing.Size(950, 80);
            panelTop.TabIndex = 1;
            //
            // txtSearch
            //
            txtSearch.Location = new Point(20, 24);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = $"{UIIcons.Search} Пошук (Назва або №)...";
            txtSearch.Size = new Size(300, 32);
            txtSearch.TabIndex = 0;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // cmbSort
            // 
            cmbSort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbSort.Location = new System.Drawing.Point(300, 24);
            cmbSort.Name = "cmbSort";
            cmbSort.Size = new System.Drawing.Size(230, 33);
            cmbSort.TabIndex = 1;
            cmbSort.SelectedIndexChanged += cmbSort_SelectedIndexChanged;
            // 
            // btnWriteOff
            // 
            btnWriteOff.Anchor = ((System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right));
            btnWriteOff.BackColor = UIColors.Error;
            btnWriteOff.FlatAppearance.BorderSize = 0;
            btnWriteOff.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnWriteOff.ForeColor = UIColors.White;
            btnWriteOff.Location = new System.Drawing.Point(800, 20);
            btnWriteOff.Name = "btnWriteOff";
            btnWriteOff.Size = new System.Drawing.Size(130, 40);
            btnWriteOff.TabIndex = 3;
            btnWriteOff.Text = $"{UIIcons.WriteOff} Списати";
            btnWriteOff.UseVisualStyleBackColor = false;
            btnWriteOff.Click += btnWriteOff_Click;
            // 
            // chkAvailable
            // 
            chkAvailable.AutoSize = true;
            chkAvailable.Location = new System.Drawing.Point(20, 50);
            chkAvailable.Name = "chkAvailable";
            chkAvailable.Size = new System.Drawing.Size(134, 29);
            chkAvailable.TabIndex = 4;
            chkAvailable.Text = $"{UIIcons.Available} Доступні";
            // 
            // chkLoaned
            // 
            chkLoaned.AutoSize = true;
            chkLoaned.Location = new System.Drawing.Point(180, 50);
            chkLoaned.Name = "chkLoaned";
            chkLoaned.Size = new System.Drawing.Size(133, 29);
            chkLoaned.TabIndex = 5;
            chkLoaned.Text = $"{UIIcons.Loaned} На руках";
            // 
            // chkLost
            // 
            chkLost.AutoSize = true;
            chkLost.Location = new System.Drawing.Point(340, 50);
            chkLost.Name = "chkLost";
            chkLost.Size = new System.Drawing.Size(134, 29);
            chkLost.TabIndex = 6;
            chkLost.Text = $"{UIIcons.Lost} Втрачені";
            // 
            // chkWrittenOff
            // 
            chkWrittenOff.AutoSize = true;
            chkWrittenOff.Location = new System.Drawing.Point(500, 50);
            chkWrittenOff.Name = "chkWrittenOff";
            chkWrittenOff.Size = new System.Drawing.Size(126, 29);
            chkWrittenOff.TabIndex = 7;
            chkWrittenOff.Text = $"{UIIcons.WrittenOff} Списані";
            // 
            // dgvInventory
            // 
            dgvInventory.AllowUserToAddRows = false;
            dgvInventory.AllowUserToDeleteRows = false;
            dgvInventory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvInventory.BackgroundColor = UIColors.BackgroundAlt;
            dgvInventory.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = Fonts.Bold14;
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dgvInventory.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvInventory.ColumnHeadersHeight = 45;
            dgvInventory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = Fonts.Regular14;
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            dgvInventory.DefaultCellStyle = dataGridViewCellStyle2;
            dgvInventory.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvInventory.Location = new System.Drawing.Point(0, 80);
            dgvInventory.Name = "dgvInventory";
            dgvInventory.ReadOnly = true;
            dgvInventory.RowHeadersVisible = false;
            dgvInventory.RowTemplate.Height = 40;
            dgvInventory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvInventory.Size = new System.Drawing.Size(950, 570);
            dgvInventory.TabIndex = 0;
            // 
            // InventoryForm
            // 
            ClientSize = new System.Drawing.Size(950, 650);
            Controls.Add(dgvInventory);
            Controls.Add(panelTop);
            Font = Fonts.Regular14;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Text = "Інвентар";
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvInventory).EndInit();
            ResumeLayout(false);
        }

        private System.Windows.Forms.Panel panelTop;
        private TextBox txtSearch;
        private ComboBox cmbSort;
        private Button btnWriteOff;
        private CheckBox chkAvailable;
        private CheckBox chkLoaned;
        private CheckBox chkLost;
        private CheckBox chkWrittenOff;
        private DataGridView dgvInventory;
    }
}