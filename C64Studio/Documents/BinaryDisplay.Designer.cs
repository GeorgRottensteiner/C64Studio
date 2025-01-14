namespace RetroDevStudio.Documents
{
  partial class BinaryDisplay
  {
    /// <summary>
    /// Erforderliche Designervariable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Verwendete Ressourcen bereinigen.
    /// </summary>
    /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
    protected override void Dispose( bool disposing )
    {
      if ( disposing && ( components != null ) )
      {
        components.Dispose();
      }
      base.Dispose( disposing );
    }

    #region Vom Komponenten-Designer generierter Code

    /// <summary>
    /// Erforderliche Methode für die Designerunterstützung.
    /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BinaryDisplay));
      this.tabMain = new System.Windows.Forms.TabControl();
      this.tabData = new System.Windows.Forms.TabPage();
      this.hexView = new Be.Windows.Forms.HexBox();
      this.tabModify = new System.Windows.Forms.TabPage();
      this.editDivideBy = new System.Windows.Forms.TextBox();
      this.editDeleteNthByte = new System.Windows.Forms.TextBox();
      this.btnSwizzle = new DecentForms.Button();
      this.btnDivide = new DecentForms.Button();
      this.btnPackNibble = new DecentForms.Button();
      this.btnDeleteNthByte = new DecentForms.Button();
      this.btnUpsize = new DecentForms.Button();
      this.btnInterleave = new DecentForms.Button();
      this.tabExport = new System.Windows.Forms.TabPage();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.panelExport = new System.Windows.Forms.Panel();
      this.label9 = new System.Windows.Forms.Label();
      this.comboExportMethod = new System.Windows.Forms.ComboBox();
      this.btnExport = new DecentForms.Button();
      this.tabImport = new System.Windows.Forms.TabPage();
      this.groupBox4 = new System.Windows.Forms.GroupBox();
      this.panelImport = new System.Windows.Forms.Panel();
      this.label1 = new System.Windows.Forms.Label();
      this.comboImportMethod = new System.Windows.Forms.ComboBox();
      this.btnImport = new DecentForms.Button();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.dataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.importFromFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exportToFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.modifyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.interleaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).BeginInit();
      this.tabMain.SuspendLayout();
      this.tabData.SuspendLayout();
      this.tabModify.SuspendLayout();
      this.tabExport.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.tabImport.SuspendLayout();
      this.groupBox4.SuspendLayout();
      this.menuStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // tabMain
      // 
      this.tabMain.Controls.Add(this.tabData);
      this.tabMain.Controls.Add(this.tabModify);
      this.tabMain.Controls.Add(this.tabExport);
      this.tabMain.Controls.Add(this.tabImport);
      this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabMain.Location = new System.Drawing.Point(0, 24);
      this.tabMain.Name = "tabMain";
      this.tabMain.SelectedIndex = 0;
      this.tabMain.Size = new System.Drawing.Size(733, 514);
      this.tabMain.TabIndex = 0;
      // 
      // tabData
      // 
      this.tabData.Controls.Add(this.hexView);
      this.tabData.Location = new System.Drawing.Point(4, 22);
      this.tabData.Name = "tabData";
      this.tabData.Padding = new System.Windows.Forms.Padding(3);
      this.tabData.Size = new System.Drawing.Size(725, 488);
      this.tabData.TabIndex = 0;
      this.tabData.Text = "Data";
      this.tabData.UseVisualStyleBackColor = true;
      // 
      // hexView
      // 
      this.hexView.ColumnInfoVisible = true;
      this.hexView.CustomHexViewer = null;
      this.hexView.DisplayedAddressOffset = ((long)(0));
      this.hexView.DisplayedByteOffset = 0;
      this.hexView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.hexView.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.hexView.InfoForeColor = System.Drawing.SystemColors.AppWorkspace;
      this.hexView.LineInfoVisible = true;
      this.hexView.Location = new System.Drawing.Point(3, 3);
      this.hexView.MarkedForeColor = System.Drawing.Color.Empty;
      this.hexView.Name = "hexView";
      this.hexView.NumDigitsMemorySize = 8;
      this.hexView.SelectedByteProvider = null;
      this.hexView.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
      this.hexView.Size = new System.Drawing.Size(719, 482);
      this.hexView.StringViewVisible = true;
      this.hexView.TabIndex = 0;
      this.hexView.TextFont = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.hexView.VScrollBarVisible = true;
      // 
      // tabModify
      // 
      this.tabModify.Controls.Add(this.editDivideBy);
      this.tabModify.Controls.Add(this.editDeleteNthByte);
      this.tabModify.Controls.Add(this.btnInterleave);
      this.tabModify.Controls.Add(this.btnSwizzle);
      this.tabModify.Controls.Add(this.btnUpsize);
      this.tabModify.Controls.Add(this.btnDivide);
      this.tabModify.Controls.Add(this.btnDeleteNthByte);
      this.tabModify.Controls.Add(this.btnPackNibble);
      this.tabModify.Location = new System.Drawing.Point(4, 22);
      this.tabModify.Name = "tabModify";
      this.tabModify.Padding = new System.Windows.Forms.Padding(3);
      this.tabModify.Size = new System.Drawing.Size(725, 488);
      this.tabModify.TabIndex = 1;
      this.tabModify.Text = "Modify";
      this.tabModify.UseVisualStyleBackColor = true;
      // 
      // editDivideBy
      // 
      this.editDivideBy.Location = new System.Drawing.Point(89, 152);
      this.editDivideBy.Name = "editDivideBy";
      this.editDivideBy.Size = new System.Drawing.Size(75, 20);
      this.editDivideBy.TabIndex = 9;
      this.editDivideBy.Text = "2";
      // 
      // editDeleteNthByte
      // 
      this.editDeleteNthByte.Location = new System.Drawing.Point(89, 66);
      this.editDeleteNthByte.Name = "editDeleteNthByte";
      this.editDeleteNthByte.Size = new System.Drawing.Size(75, 20);
      this.editDeleteNthByte.TabIndex = 5;
      this.editDeleteNthByte.Text = "2";
      // 
      // btnSwizzle
      // 
      this.btnSwizzle.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnSwizzle.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnSwizzle.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnSwizzle.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnSwizzle.Image = null;
      this.btnSwizzle.Location = new System.Drawing.Point(8, 121);
      this.btnSwizzle.Name = "btnSwizzle";
      this.btnSwizzle.Size = new System.Drawing.Size(75, 23);
      this.btnSwizzle.TabIndex = 7;
      this.btnSwizzle.Text = "Swizzle";
      this.btnSwizzle.Click += new DecentForms.EventHandler(this.btnSwizzle_Click);
      // 
      // btnDivide
      // 
      this.btnDivide.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnDivide.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnDivide.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnDivide.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnDivide.Image = null;
      this.btnDivide.Location = new System.Drawing.Point(8, 150);
      this.btnDivide.Name = "btnDivide";
      this.btnDivide.Size = new System.Drawing.Size(75, 23);
      this.btnDivide.TabIndex = 8;
      this.btnDivide.Text = "Divide by";
      this.btnDivide.Click += new DecentForms.EventHandler(this.btnDivide_Click);
      // 
      // btnPackNibble
      // 
      this.btnPackNibble.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnPackNibble.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnPackNibble.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnPackNibble.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnPackNibble.Image = null;
      this.btnPackNibble.Location = new System.Drawing.Point(8, 92);
      this.btnPackNibble.Name = "btnPackNibble";
      this.btnPackNibble.Size = new System.Drawing.Size(75, 23);
      this.btnPackNibble.TabIndex = 6;
      this.btnPackNibble.Text = "Pack Nibble";
      this.btnPackNibble.Click += new DecentForms.EventHandler(this.btnPackNibbles_Click);
      // 
      // btnDeleteNthByte
      // 
      this.btnDeleteNthByte.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnDeleteNthByte.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnDeleteNthByte.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnDeleteNthByte.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnDeleteNthByte.Image = null;
      this.btnDeleteNthByte.Location = new System.Drawing.Point(8, 64);
      this.btnDeleteNthByte.Name = "btnDeleteNthByte";
      this.btnDeleteNthByte.Size = new System.Drawing.Size(75, 23);
      this.btnDeleteNthByte.TabIndex = 4;
      this.btnDeleteNthByte.Text = "Delete nth";
      this.btnDeleteNthByte.Click += new DecentForms.EventHandler(this.btnDeleteNthByte_Click);
      // 
      // btnUpsize
      // 
      this.btnUpsize.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnUpsize.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnUpsize.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnUpsize.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnUpsize.Image = null;
      this.btnUpsize.Location = new System.Drawing.Point(8, 35);
      this.btnUpsize.Name = "btnUpsize";
      this.btnUpsize.Size = new System.Drawing.Size(75, 23);
      this.btnUpsize.TabIndex = 3;
      this.btnUpsize.Text = "Upsize";
      this.btnUpsize.Click += new DecentForms.EventHandler(this.btnUpsize_Click);
      // 
      // btnInterleave
      // 
      this.btnInterleave.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnInterleave.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnInterleave.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnInterleave.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnInterleave.Image = null;
      this.btnInterleave.Location = new System.Drawing.Point(8, 6);
      this.btnInterleave.Name = "btnInterleave";
      this.btnInterleave.Size = new System.Drawing.Size(75, 23);
      this.btnInterleave.TabIndex = 2;
      this.btnInterleave.Text = "Interleave...";
      this.btnInterleave.Click += new DecentForms.EventHandler(this.btnInterleave_Click);
      // 
      // tabExport
      // 
      this.tabExport.Controls.Add(this.groupBox3);
      this.tabExport.Location = new System.Drawing.Point(4, 22);
      this.tabExport.Name = "tabExport";
      this.tabExport.Padding = new System.Windows.Forms.Padding(3);
      this.tabExport.Size = new System.Drawing.Size(725, 488);
      this.tabExport.TabIndex = 2;
      this.tabExport.Text = "Export";
      this.tabExport.UseVisualStyleBackColor = true;
      // 
      // groupBox3
      // 
      this.groupBox3.Controls.Add(this.panelExport);
      this.groupBox3.Controls.Add(this.label9);
      this.groupBox3.Controls.Add(this.comboExportMethod);
      this.groupBox3.Controls.Add(this.btnExport);
      this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBox3.Location = new System.Drawing.Point(3, 3);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(719, 482);
      this.groupBox3.TabIndex = 1;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Text Data";
      // 
      // panelExport
      // 
      this.panelExport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.panelExport.Location = new System.Drawing.Point(6, 40);
      this.panelExport.Name = "panelExport";
      this.panelExport.Size = new System.Drawing.Size(707, 433);
      this.panelExport.TabIndex = 10;
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Location = new System.Drawing.Point(6, 16);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(54, 13);
      this.label9.TabIndex = 9;
      this.label9.Text = "Export as:";
      // 
      // comboExportMethod
      // 
      this.comboExportMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboExportMethod.FormattingEnabled = true;
      this.comboExportMethod.Location = new System.Drawing.Point(66, 13);
      this.comboExportMethod.Name = "comboExportMethod";
      this.comboExportMethod.Size = new System.Drawing.Size(171, 21);
      this.comboExportMethod.TabIndex = 8;
      this.comboExportMethod.SelectedIndexChanged += new System.EventHandler(this.comboExportMethod_SelectedIndexChanged);
      // 
      // btnExport
      // 
      this.btnExport.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnExport.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnExport.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnExport.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnExport.Image = null;
      this.btnExport.Location = new System.Drawing.Point(243, 13);
      this.btnExport.Name = "btnExport";
      this.btnExport.Size = new System.Drawing.Size(231, 23);
      this.btnExport.TabIndex = 1;
      this.btnExport.Text = "Export";
      this.btnExport.Click += new DecentForms.EventHandler(this.btnExport_Click_1);
      // 
      // tabImport
      // 
      this.tabImport.Controls.Add(this.groupBox4);
      this.tabImport.Location = new System.Drawing.Point(4, 22);
      this.tabImport.Name = "tabImport";
      this.tabImport.Padding = new System.Windows.Forms.Padding(3);
      this.tabImport.Size = new System.Drawing.Size(725, 488);
      this.tabImport.TabIndex = 3;
      this.tabImport.Text = "Import";
      this.tabImport.UseVisualStyleBackColor = true;
      // 
      // groupBox4
      // 
      this.groupBox4.Controls.Add(this.panelImport);
      this.groupBox4.Controls.Add(this.label1);
      this.groupBox4.Controls.Add(this.comboImportMethod);
      this.groupBox4.Controls.Add(this.btnImport);
      this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBox4.Location = new System.Drawing.Point(3, 3);
      this.groupBox4.Name = "groupBox4";
      this.groupBox4.Size = new System.Drawing.Size(719, 482);
      this.groupBox4.TabIndex = 2;
      this.groupBox4.TabStop = false;
      this.groupBox4.Text = "Text Data";
      // 
      // panelImport
      // 
      this.panelImport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.panelImport.Location = new System.Drawing.Point(5, 46);
      this.panelImport.Name = "panelImport";
      this.panelImport.Size = new System.Drawing.Size(708, 430);
      this.panelImport.TabIndex = 10;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(7, 22);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(53, 13);
      this.label1.TabIndex = 9;
      this.label1.Text = "Import as:";
      // 
      // comboImportMethod
      // 
      this.comboImportMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboImportMethod.FormattingEnabled = true;
      this.comboImportMethod.Location = new System.Drawing.Point(67, 19);
      this.comboImportMethod.Name = "comboImportMethod";
      this.comboImportMethod.Size = new System.Drawing.Size(171, 21);
      this.comboImportMethod.TabIndex = 8;
      this.comboImportMethod.SelectedIndexChanged += new System.EventHandler(this.comboImportMethod_SelectedIndexChanged);
      // 
      // btnImport
      // 
      this.btnImport.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnImport.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnImport.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnImport.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnImport.Image = null;
      this.btnImport.Location = new System.Drawing.Point(244, 17);
      this.btnImport.Name = "btnImport";
      this.btnImport.Size = new System.Drawing.Size(231, 23);
      this.btnImport.TabIndex = 1;
      this.btnImport.Text = "Import";
      this.btnImport.Click += new DecentForms.EventHandler(this.btnImport_Click);
      // 
      // menuStrip1
      // 
      this.menuStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dataToolStripMenuItem});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(733, 24);
      this.menuStrip1.TabIndex = 1;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // dataToolStripMenuItem
      // 
      this.dataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importFromFileToolStripMenuItem,
            this.exportToFileToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.toolStripSeparator1,
            this.modifyToolStripMenuItem});
      this.dataToolStripMenuItem.Name = "dataToolStripMenuItem";
      this.dataToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
      this.dataToolStripMenuItem.Text = "Binary";
      // 
      // importFromFileToolStripMenuItem
      // 
      this.importFromFileToolStripMenuItem.Name = "importFromFileToolStripMenuItem";
      this.importFromFileToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
      this.importFromFileToolStripMenuItem.Text = "&Import from file...";
      this.importFromFileToolStripMenuItem.Click += new System.EventHandler(this.importFromFileToolStripMenuItem_Click);
      // 
      // exportToFileToolStripMenuItem
      // 
      this.exportToFileToolStripMenuItem.Name = "exportToFileToolStripMenuItem";
      this.exportToFileToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
      this.exportToFileToolStripMenuItem.Text = "Export to file...";
      this.exportToFileToolStripMenuItem.Click += new System.EventHandler(this.exportToFileToolStripMenuItem_Click);
      // 
      // closeToolStripMenuItem
      // 
      this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
      this.closeToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
      this.closeToolStripMenuItem.Text = "&Close";
      this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(164, 6);
      // 
      // modifyToolStripMenuItem
      // 
      this.modifyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.interleaveToolStripMenuItem});
      this.modifyToolStripMenuItem.Name = "modifyToolStripMenuItem";
      this.modifyToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
      this.modifyToolStripMenuItem.Text = "Modify";
      // 
      // interleaveToolStripMenuItem
      // 
      this.interleaveToolStripMenuItem.Name = "interleaveToolStripMenuItem";
      this.interleaveToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
      this.interleaveToolStripMenuItem.Text = "Interleave...";
      this.interleaveToolStripMenuItem.Click += new System.EventHandler(this.interleaveToolStripMenuItem_Click);
      // 
      // BinaryDisplay
      // 
      this.ClientSize = new System.Drawing.Size(733, 538);
      this.Controls.Add(this.tabMain);
      this.Controls.Add(this.menuStrip1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MainMenuStrip = this.menuStrip1;
      this.Name = "BinaryDisplay";
      this.Text = "Binary Editor";
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).EndInit();
      this.tabMain.ResumeLayout(false);
      this.tabData.ResumeLayout(false);
      this.tabModify.ResumeLayout(false);
      this.tabModify.PerformLayout();
      this.tabExport.ResumeLayout(false);
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.tabImport.ResumeLayout(false);
      this.groupBox4.ResumeLayout(false);
      this.groupBox4.PerformLayout();
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TabControl tabMain;
    private System.Windows.Forms.TabPage tabData;
    private System.Windows.Forms.TabPage tabModify;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem dataToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exportToFileToolStripMenuItem;
    private Be.Windows.Forms.HexBox hexView;
    private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem importFromFileToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem modifyToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem interleaveToolStripMenuItem;
    private DecentForms.Button btnInterleave;
    private DecentForms.Button btnUpsize;
    private System.Windows.Forms.TextBox editDeleteNthByte;
    private DecentForms.Button btnDeleteNthByte;
    private DecentForms.Button btnPackNibble;
    private DecentForms.Button btnSwizzle;
    private System.Windows.Forms.TextBox editDivideBy;
    private DecentForms.Button btnDivide;
    private System.Windows.Forms.TabPage tabExport;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.ComboBox comboExportMethod;
    private DecentForms.Button btnExport;
    private System.Windows.Forms.Panel panelExport;
    private System.Windows.Forms.TabPage tabImport;
    private System.Windows.Forms.GroupBox groupBox4;
    private System.Windows.Forms.Panel panelImport;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox comboImportMethod;
    private DecentForms.Button btnImport;
  }
}
