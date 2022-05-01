using C64Studio.Controls;
namespace C64Studio
{
  partial class CharsetEditor
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
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CharsetEditor));
      this.tabCharsetEditor = new System.Windows.Forms.TabControl();
      this.tabCharEditor = new System.Windows.Forms.TabPage();
      this.characterEditor = new C64Studio.Controls.CharacterEditor();
      this.tabExport = new System.Windows.Forms.TabPage();
      this.comboExportRange = new System.Windows.Forms.ComboBox();
      this.editCharactersCount = new System.Windows.Forms.TextBox();
      this.editCharactersFrom = new System.Windows.Forms.TextBox();
      this.labelCharactersTo = new System.Windows.Forms.Label();
      this.labelCharactersFrom = new System.Windows.Forms.Label();
      this.editDataExport = new System.Windows.Forms.TextBox();
      this.btnExport = new System.Windows.Forms.Button();
      this.panelExport = new System.Windows.Forms.Panel();
      this.comboExportMethod = new System.Windows.Forms.ComboBox();
      this.label4 = new System.Windows.Forms.Label();
      this.label11 = new System.Windows.Forms.Label();
      this.tabImport = new System.Windows.Forms.TabPage();
      this.panelImport = new System.Windows.Forms.Panel();
      this.btnImport = new System.Windows.Forms.Button();
      this.comboImportMethod = new System.Windows.Forms.ComboBox();
      this.label2 = new System.Windows.Forms.Label();
      this.contextMenuDefaultCharsets = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.c64UppercaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.c64LowercaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.viC20UppercaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.viC20LowercaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.openCharsetProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveCharsetProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.closeCharsetProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).BeginInit();
      this.tabCharsetEditor.SuspendLayout();
      this.tabCharEditor.SuspendLayout();
      this.tabExport.SuspendLayout();
      this.tabImport.SuspendLayout();
      this.contextMenuDefaultCharsets.SuspendLayout();
      this.menuStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // tabCharsetEditor
      // 
      this.tabCharsetEditor.Controls.Add(this.tabCharEditor);
      this.tabCharsetEditor.Controls.Add(this.tabExport);
      this.tabCharsetEditor.Controls.Add(this.tabImport);
      this.tabCharsetEditor.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabCharsetEditor.Location = new System.Drawing.Point(0, 24);
      this.tabCharsetEditor.Name = "tabCharsetEditor";
      this.tabCharsetEditor.SelectedIndex = 0;
      this.tabCharsetEditor.Size = new System.Drawing.Size(1064, 503);
      this.tabCharsetEditor.TabIndex = 0;
      // 
      // tabCharEditor
      // 
      this.tabCharEditor.Controls.Add(this.characterEditor);
      this.tabCharEditor.Location = new System.Drawing.Point(4, 22);
      this.tabCharEditor.Name = "tabCharEditor";
      this.tabCharEditor.Size = new System.Drawing.Size(1056, 477);
      this.tabCharEditor.TabIndex = 3;
      this.tabCharEditor.Text = "Editor";
      this.tabCharEditor.UseVisualStyleBackColor = true;
      // 
      // characterEditor
      // 
      this.characterEditor.AllowModeChange = true;
      this.characterEditor.Dock = System.Windows.Forms.DockStyle.Fill;
      this.characterEditor.Location = new System.Drawing.Point(0, 0);
      this.characterEditor.Name = "characterEditor";
      this.characterEditor.Size = new System.Drawing.Size(1056, 477);
      this.characterEditor.TabIndex = 0;
      this.characterEditor.Modified += new C64Studio.Controls.CharacterEditor.ModifiedHandler(this.characterEditor_Modified);
      // 
      // tabExport
      // 
      this.tabExport.Controls.Add(this.comboExportRange);
      this.tabExport.Controls.Add(this.editCharactersCount);
      this.tabExport.Controls.Add(this.editCharactersFrom);
      this.tabExport.Controls.Add(this.labelCharactersTo);
      this.tabExport.Controls.Add(this.labelCharactersFrom);
      this.tabExport.Controls.Add(this.editDataExport);
      this.tabExport.Controls.Add(this.btnExport);
      this.tabExport.Controls.Add(this.panelExport);
      this.tabExport.Controls.Add(this.comboExportMethod);
      this.tabExport.Controls.Add(this.label4);
      this.tabExport.Controls.Add(this.label11);
      this.tabExport.Location = new System.Drawing.Point(4, 22);
      this.tabExport.Name = "tabExport";
      this.tabExport.Padding = new System.Windows.Forms.Padding(3);
      this.tabExport.Size = new System.Drawing.Size(1056, 477);
      this.tabExport.TabIndex = 4;
      this.tabExport.Text = "Export";
      this.tabExport.UseVisualStyleBackColor = true;
      // 
      // comboExportRange
      // 
      this.comboExportRange.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboExportRange.FormattingEnabled = true;
      this.comboExportRange.Location = new System.Drawing.Point(90, 5);
      this.comboExportRange.Name = "comboExportRange";
      this.comboExportRange.Size = new System.Drawing.Size(88, 21);
      this.comboExportRange.TabIndex = 44;
      this.comboExportRange.SelectedIndexChanged += new System.EventHandler(this.comboExportRange_SelectedIndexChanged);
      // 
      // editCharactersCount
      // 
      this.editCharactersCount.Location = new System.Drawing.Point(328, 5);
      this.editCharactersCount.Name = "editCharactersCount";
      this.editCharactersCount.Size = new System.Drawing.Size(56, 20);
      this.editCharactersCount.TabIndex = 42;
      this.editCharactersCount.TextChanged += new System.EventHandler(this.editUsedCharacters_TextChanged);
      // 
      // editCharactersFrom
      // 
      this.editCharactersFrom.Location = new System.Drawing.Point(223, 5);
      this.editCharactersFrom.Name = "editCharactersFrom";
      this.editCharactersFrom.Size = new System.Drawing.Size(56, 20);
      this.editCharactersFrom.TabIndex = 43;
      this.editCharactersFrom.TextChanged += new System.EventHandler(this.editStartCharacters_TextChanged);
      // 
      // labelCharactersTo
      // 
      this.labelCharactersTo.AutoSize = true;
      this.labelCharactersTo.Location = new System.Drawing.Point(285, 8);
      this.labelCharactersTo.Name = "labelCharactersTo";
      this.labelCharactersTo.Size = new System.Drawing.Size(37, 13);
      this.labelCharactersTo.TabIndex = 40;
      this.labelCharactersTo.Text = "count:";
      // 
      // labelCharactersFrom
      // 
      this.labelCharactersFrom.AutoSize = true;
      this.labelCharactersFrom.Location = new System.Drawing.Point(187, 8);
      this.labelCharactersFrom.Name = "labelCharactersFrom";
      this.labelCharactersFrom.Size = new System.Drawing.Size(30, 13);
      this.labelCharactersFrom.TabIndex = 41;
      this.labelCharactersFrom.Text = "from:";
      // 
      // editDataExport
      // 
      this.editDataExport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.editDataExport.Location = new System.Drawing.Point(451, 3);
      this.editDataExport.Multiline = true;
      this.editDataExport.Name = "editDataExport";
      this.editDataExport.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.editDataExport.Size = new System.Drawing.Size(597, 469);
      this.editDataExport.TabIndex = 39;
      this.editDataExport.WordWrap = false;
      // 
      // btnExport
      // 
      this.btnExport.Location = new System.Drawing.Point(370, 36);
      this.btnExport.Name = "btnExport";
      this.btnExport.Size = new System.Drawing.Size(75, 21);
      this.btnExport.TabIndex = 33;
      this.btnExport.Text = "Export";
      this.btnExport.UseVisualStyleBackColor = true;
      this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
      // 
      // panelExport
      // 
      this.panelExport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
      this.panelExport.Location = new System.Drawing.Point(6, 63);
      this.panelExport.Name = "panelExport";
      this.panelExport.Size = new System.Drawing.Size(439, 409);
      this.panelExport.TabIndex = 32;
      // 
      // comboExportMethod
      // 
      this.comboExportMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboExportMethod.FormattingEnabled = true;
      this.comboExportMethod.Location = new System.Drawing.Point(90, 36);
      this.comboExportMethod.Name = "comboExportMethod";
      this.comboExportMethod.Size = new System.Drawing.Size(274, 21);
      this.comboExportMethod.TabIndex = 30;
      this.comboExportMethod.SelectedIndexChanged += new System.EventHandler(this.comboExportMethod_SelectedIndexChanged);
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(8, 8);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(37, 13);
      this.label4.TabIndex = 31;
      this.label4.Text = "Export";
      // 
      // label11
      // 
      this.label11.AutoSize = true;
      this.label11.Location = new System.Drawing.Point(8, 39);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(79, 13);
      this.label11.TabIndex = 31;
      this.label11.Text = "Export Method:";
      // 
      // tabImport
      // 
      this.tabImport.Controls.Add(this.panelImport);
      this.tabImport.Controls.Add(this.btnImport);
      this.tabImport.Controls.Add(this.comboImportMethod);
      this.tabImport.Controls.Add(this.label2);
      this.tabImport.Location = new System.Drawing.Point(4, 22);
      this.tabImport.Name = "tabImport";
      this.tabImport.Padding = new System.Windows.Forms.Padding(3);
      this.tabImport.Size = new System.Drawing.Size(1056, 477);
      this.tabImport.TabIndex = 5;
      this.tabImport.Text = "Import";
      this.tabImport.UseVisualStyleBackColor = true;
      // 
      // panelImport
      // 
      this.panelImport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.panelImport.Location = new System.Drawing.Point(3, 36);
      this.panelImport.Name = "panelImport";
      this.panelImport.Size = new System.Drawing.Size(1050, 438);
      this.panelImport.TabIndex = 37;
      // 
      // btnImport
      // 
      this.btnImport.Location = new System.Drawing.Point(337, 8);
      this.btnImport.Name = "btnImport";
      this.btnImport.Size = new System.Drawing.Size(75, 21);
      this.btnImport.TabIndex = 36;
      this.btnImport.Text = "Import";
      this.btnImport.UseVisualStyleBackColor = true;
      this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
      // 
      // comboImportMethod
      // 
      this.comboImportMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboImportMethod.FormattingEnabled = true;
      this.comboImportMethod.Location = new System.Drawing.Point(88, 9);
      this.comboImportMethod.Name = "comboImportMethod";
      this.comboImportMethod.Size = new System.Drawing.Size(243, 21);
      this.comboImportMethod.TabIndex = 34;
      this.comboImportMethod.SelectedIndexChanged += new System.EventHandler(this.comboImportMethod_SelectedIndexChanged);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(3, 12);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(78, 13);
      this.label2.TabIndex = 35;
      this.label2.Text = "Import Method:";
      // 
      // contextMenuDefaultCharsets
      // 
      this.contextMenuDefaultCharsets.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.c64UppercaseToolStripMenuItem,
            this.c64LowercaseToolStripMenuItem,
            this.viC20UppercaseToolStripMenuItem,
            this.viC20LowercaseToolStripMenuItem});
      this.contextMenuDefaultCharsets.Name = "contextMenuDefaultCharsets";
      this.contextMenuDefaultCharsets.Size = new System.Drawing.Size(163, 92);
      // 
      // c64UppercaseToolStripMenuItem
      // 
      this.c64UppercaseToolStripMenuItem.Name = "c64UppercaseToolStripMenuItem";
      this.c64UppercaseToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
      this.c64UppercaseToolStripMenuItem.Text = "C64 Uppercase";
      this.c64UppercaseToolStripMenuItem.Click += new System.EventHandler(this.c64UppercaseToolStripMenuItem_Click);
      // 
      // c64LowercaseToolStripMenuItem
      // 
      this.c64LowercaseToolStripMenuItem.Name = "c64LowercaseToolStripMenuItem";
      this.c64LowercaseToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
      this.c64LowercaseToolStripMenuItem.Text = "C64 Lowercase";
      this.c64LowercaseToolStripMenuItem.Click += new System.EventHandler(this.c64LowercaseToolStripMenuItem_Click);
      // 
      // viC20UppercaseToolStripMenuItem
      // 
      this.viC20UppercaseToolStripMenuItem.Name = "viC20UppercaseToolStripMenuItem";
      this.viC20UppercaseToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
      this.viC20UppercaseToolStripMenuItem.Text = "ViC20 Uppercase";
      this.viC20UppercaseToolStripMenuItem.Click += new System.EventHandler(this.viC20UppercaseToolStripMenuItem_Click);
      // 
      // viC20LowercaseToolStripMenuItem
      // 
      this.viC20LowercaseToolStripMenuItem.Name = "viC20LowercaseToolStripMenuItem";
      this.viC20LowercaseToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
      this.viC20LowercaseToolStripMenuItem.Text = "ViC20 Lowercase";
      this.viC20LowercaseToolStripMenuItem.Click += new System.EventHandler(this.viC20LowercaseToolStripMenuItem_Click);
      // 
      // menuStrip1
      // 
      this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(1064, 24);
      this.menuStrip1.TabIndex = 1;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // fileToolStripMenuItem
      // 
      this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openCharsetProjectToolStripMenuItem,
            this.saveCharsetProjectToolStripMenuItem,
            this.closeCharsetProjectToolStripMenuItem});
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new System.Drawing.Size(75, 20);
      this.fileToolStripMenuItem.Text = "&Characters";
      // 
      // openCharsetProjectToolStripMenuItem
      // 
      this.openCharsetProjectToolStripMenuItem.Name = "openCharsetProjectToolStripMenuItem";
      this.openCharsetProjectToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
      this.openCharsetProjectToolStripMenuItem.Text = "&Open Charset Project...";
      this.openCharsetProjectToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
      // 
      // saveCharsetProjectToolStripMenuItem
      // 
      this.saveCharsetProjectToolStripMenuItem.Enabled = false;
      this.saveCharsetProjectToolStripMenuItem.Name = "saveCharsetProjectToolStripMenuItem";
      this.saveCharsetProjectToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
      this.saveCharsetProjectToolStripMenuItem.Text = "&Save Project";
      this.saveCharsetProjectToolStripMenuItem.Click += new System.EventHandler(this.saveCharsetProjectToolStripMenuItem_Click);
      // 
      // closeCharsetProjectToolStripMenuItem
      // 
      this.closeCharsetProjectToolStripMenuItem.Enabled = false;
      this.closeCharsetProjectToolStripMenuItem.Name = "closeCharsetProjectToolStripMenuItem";
      this.closeCharsetProjectToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
      this.closeCharsetProjectToolStripMenuItem.Text = "&Close Charset Project";
      this.closeCharsetProjectToolStripMenuItem.Click += new System.EventHandler(this.closeCharsetProjectToolStripMenuItem_Click);
      // 
      // CharsetEditor
      // 
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.ClientSize = new System.Drawing.Size(1064, 527);
      this.Controls.Add(this.tabCharsetEditor);
      this.Controls.Add(this.menuStrip1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "CharsetEditor";
      this.Text = "Charset Editor";
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).EndInit();
      this.tabCharsetEditor.ResumeLayout(false);
      this.tabCharEditor.ResumeLayout(false);
      this.tabExport.ResumeLayout(false);
      this.tabExport.PerformLayout();
      this.tabImport.ResumeLayout(false);
      this.tabImport.PerformLayout();
      this.contextMenuDefaultCharsets.ResumeLayout(false);
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TabControl tabCharsetEditor;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem openCharsetProjectToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem closeCharsetProjectToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveCharsetProjectToolStripMenuItem;
    private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TabPage tabCharEditor;
        public CharacterEditor characterEditor;
    private System.Windows.Forms.ContextMenuStrip contextMenuDefaultCharsets;
    private System.Windows.Forms.ToolStripMenuItem c64UppercaseToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem c64LowercaseToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem viC20UppercaseToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem viC20LowercaseToolStripMenuItem;
    private System.Windows.Forms.TabPage tabExport;
    private System.Windows.Forms.Button btnExport;
    private System.Windows.Forms.Panel panelExport;
    private System.Windows.Forms.ComboBox comboExportMethod;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.TabPage tabImport;
    private System.Windows.Forms.TextBox editDataExport;
    private System.Windows.Forms.ComboBox comboExportRange;
    private System.Windows.Forms.TextBox editCharactersCount;
    private System.Windows.Forms.TextBox editCharactersFrom;
    private System.Windows.Forms.Label labelCharactersTo;
    private System.Windows.Forms.Label labelCharactersFrom;
    private System.Windows.Forms.Panel panelImport;
    private System.Windows.Forms.Button btnImport;
    private System.Windows.Forms.ComboBox comboImportMethod;
    private System.Windows.Forms.Label label2;
  }
}
