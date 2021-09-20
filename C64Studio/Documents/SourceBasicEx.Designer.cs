namespace C64Studio
{
  partial class SourceBasicEx
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SourceBasicEx));
      this.editSource = new FastColoredTextBoxNS.FastColoredTextBox();
      this.contextSource = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.renumberToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
      this.btnToggleLabelMode = new System.Windows.Forms.CheckBox();
      this.menuBASIC = new System.Windows.Forms.MenuStrip();
      this.bASICToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.renumberToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.btnToggleSymbolMode = new C64Studio.Controls.CSCheckBox();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.btnToggleUpperLowerCase = new C64Studio.Controls.CSCheckBox();
      this.btnToggleStringEntryMode = new C64Studio.Controls.CSCheckBox();
      this.editBASICStartAddress = new System.Windows.Forms.TextBox();
      this.labelStartAddress = new System.Windows.Forms.Label();
      this.labelBASICVersion = new System.Windows.Forms.Label();
      this.comboBASICVersion = new System.Windows.Forms.ComboBox();
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.editSource)).BeginInit();
      this.contextSource.SuspendLayout();
      this.menuBASIC.SuspendLayout();
      this.SuspendLayout();
      // 
      // editSource
      // 
      this.editSource.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.editSource.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
      this.editSource.AutoScrollMinSize = new System.Drawing.Size(25, 14);
      this.editSource.BackBrush = null;
      this.editSource.CharHeight = 14;
      this.editSource.CharWidth = 7;
      this.editSource.ContextMenuStrip = this.contextSource;
      this.editSource.ConvertTabsToSpaces = false;
      this.editSource.Cursor = System.Windows.Forms.Cursors.IBeam;
      this.editSource.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
      this.editSource.Font = new System.Drawing.Font("Consolas", 9F);
      this.editSource.IsReplaceMode = false;
      this.editSource.Location = new System.Drawing.Point(0, 56);
      this.editSource.Name = "editSource";
      this.editSource.Paddings = new System.Windows.Forms.Padding(0);
      this.editSource.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
      this.editSource.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("editSource.ServiceColors")));
      this.editSource.Size = new System.Drawing.Size(698, 532);
      this.editSource.TabIndex = 0;
      this.editSource.Zoom = 100;
      this.editSource.LineVisited += new System.EventHandler<FastColoredTextBoxNS.LineVisitedArgs>(this.editSource_LineVisited);
      this.editSource.DragDrop += new System.Windows.Forms.DragEventHandler(this.editSource_DragDrop);
      this.editSource.DragEnter += new System.Windows.Forms.DragEventHandler(this.editSource_DragEnter);
      // 
      // contextSource
      // 
      this.contextSource.ImageScalingSize = new System.Drawing.Size(20, 20);
      this.contextSource.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.cutToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripSeparator1,
            this.renumberToolStripMenuItem1});
      this.contextSource.Name = "contextSource";
      this.contextSource.Size = new System.Drawing.Size(139, 98);
      // 
      // copyToolStripMenuItem
      // 
      this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
      this.copyToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
      this.copyToolStripMenuItem.Text = "&Copy";
      this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
      // 
      // cutToolStripMenuItem
      // 
      this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
      this.cutToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
      this.cutToolStripMenuItem.Text = "C&ut";
      this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
      // 
      // pasteToolStripMenuItem
      // 
      this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
      this.pasteToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
      this.pasteToolStripMenuItem.Text = "&Paste";
      this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(135, 6);
      // 
      // renumberToolStripMenuItem1
      // 
      this.renumberToolStripMenuItem1.Name = "renumberToolStripMenuItem1";
      this.renumberToolStripMenuItem1.Size = new System.Drawing.Size(138, 22);
      this.renumberToolStripMenuItem1.Text = "Renumber...";
      this.renumberToolStripMenuItem1.Click += new System.EventHandler(this.renumberToolStripMenuItem_Click);
      // 
      // btnToggleLabelMode
      // 
      this.btnToggleLabelMode.Appearance = System.Windows.Forms.Appearance.Button;
      this.btnToggleLabelMode.AutoSize = true;
      this.btnToggleLabelMode.Location = new System.Drawing.Point(0, 27);
      this.btnToggleLabelMode.Name = "btnToggleLabelMode";
      this.btnToggleLabelMode.Size = new System.Drawing.Size(73, 23);
      this.btnToggleLabelMode.TabIndex = 2;
      this.btnToggleLabelMode.Text = "Label Mode";
      this.btnToggleLabelMode.UseVisualStyleBackColor = true;
      this.btnToggleLabelMode.CheckedChanged += new System.EventHandler(this.btnToggleLabelMode_CheckedChanged);
      // 
      // menuBASIC
      // 
      this.menuBASIC.ImageScalingSize = new System.Drawing.Size(20, 20);
      this.menuBASIC.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bASICToolStripMenuItem});
      this.menuBASIC.Location = new System.Drawing.Point(0, 0);
      this.menuBASIC.Name = "menuBASIC";
      this.menuBASIC.Size = new System.Drawing.Size(698, 24);
      this.menuBASIC.TabIndex = 3;
      this.menuBASIC.Text = "menuStrip1";
      // 
      // bASICToolStripMenuItem
      // 
      this.bASICToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renumberToolStripMenuItem});
      this.bASICToolStripMenuItem.Name = "bASICToolStripMenuItem";
      this.bASICToolStripMenuItem.Size = new System.Drawing.Size(51, 20);
      this.bASICToolStripMenuItem.Text = "BASIC";
      // 
      // renumberToolStripMenuItem
      // 
      this.renumberToolStripMenuItem.Name = "renumberToolStripMenuItem";
      this.renumberToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
      this.renumberToolStripMenuItem.Text = "&Renumber...";
      this.renumberToolStripMenuItem.Click += new System.EventHandler(this.renumberToolStripMenuItem_Click);
      // 
      // btnToggleSymbolMode
      // 
      this.btnToggleSymbolMode.Appearance = System.Windows.Forms.Appearance.Button;
      this.btnToggleSymbolMode.AutoSize = true;
      this.btnToggleSymbolMode.Image = ((System.Drawing.Image)(resources.GetObject("btnToggleSymbolMode.Image")));
      this.btnToggleSymbolMode.Location = new System.Drawing.Point(79, 27);
      this.btnToggleSymbolMode.Name = "btnToggleSymbolMode";
      this.btnToggleSymbolMode.Size = new System.Drawing.Size(22, 22);
      this.btnToggleSymbolMode.TabIndex = 2;
      this.toolTip1.SetToolTip(this.btnToggleSymbolMode, "Toggle Symbol/Macro");
      this.btnToggleSymbolMode.UseVisualStyleBackColor = true;
      this.btnToggleSymbolMode.CheckedChanged += new System.EventHandler(this.btnToggleSymbolMode_CheckedChanged);
      // 
      // btnToggleUpperLowerCase
      // 
      this.btnToggleUpperLowerCase.Appearance = System.Windows.Forms.Appearance.Button;
      this.btnToggleUpperLowerCase.AutoSize = true;
      this.btnToggleUpperLowerCase.Image = ((System.Drawing.Image)(resources.GetObject("btnToggleUpperLowerCase.Image")));
      this.btnToggleUpperLowerCase.Location = new System.Drawing.Point(107, 27);
      this.btnToggleUpperLowerCase.Name = "btnToggleUpperLowerCase";
      this.btnToggleUpperLowerCase.Size = new System.Drawing.Size(22, 22);
      this.btnToggleUpperLowerCase.TabIndex = 2;
      this.toolTip1.SetToolTip(this.btnToggleUpperLowerCase, "Toggle Upper/Lower Case (Currently Upper Case)");
      this.btnToggleUpperLowerCase.UseVisualStyleBackColor = true;
      this.btnToggleUpperLowerCase.CheckedChanged += new System.EventHandler(this.btnToggleUpperLowerCase_CheckedChanged);
      // 
      // btnToggleStringEntryMode
      // 
      this.btnToggleStringEntryMode.Appearance = System.Windows.Forms.Appearance.Button;
      this.btnToggleStringEntryMode.AutoSize = true;
      this.btnToggleStringEntryMode.Image = ((System.Drawing.Image)(resources.GetObject("btnToggleStringEntryMode.Image")));
      this.btnToggleStringEntryMode.Location = new System.Drawing.Point(135, 27);
      this.btnToggleStringEntryMode.Name = "btnToggleStringEntryMode";
      this.btnToggleStringEntryMode.Size = new System.Drawing.Size(22, 22);
      this.btnToggleStringEntryMode.TabIndex = 2;
      this.toolTip1.SetToolTip(this.btnToggleStringEntryMode, "Toggle String Entry Mode (currently inactive)");
      this.btnToggleStringEntryMode.UseVisualStyleBackColor = true;
      this.btnToggleStringEntryMode.CheckedChanged += new System.EventHandler(this.btnToggleStringEntryMode_CheckedChanged);
      // 
      // editBASICStartAddress
      // 
      this.editBASICStartAddress.Location = new System.Drawing.Point(250, 29);
      this.editBASICStartAddress.MaxLength = 7;
      this.editBASICStartAddress.Name = "editBASICStartAddress";
      this.editBASICStartAddress.Size = new System.Drawing.Size(65, 20);
      this.editBASICStartAddress.TabIndex = 4;
      this.editBASICStartAddress.Text = "2049";
      this.editBASICStartAddress.TextChanged += new System.EventHandler(this.editBASICStartAddress_TextChanged);
      // 
      // labelStartAddress
      // 
      this.labelStartAddress.AutoSize = true;
      this.labelStartAddress.Location = new System.Drawing.Point(171, 32);
      this.labelStartAddress.Name = "labelStartAddress";
      this.labelStartAddress.Size = new System.Drawing.Size(73, 13);
      this.labelStartAddress.TabIndex = 5;
      this.labelStartAddress.Text = "Start Address:";
      // 
      // labelBASICVersion
      // 
      this.labelBASICVersion.AutoSize = true;
      this.labelBASICVersion.Location = new System.Drawing.Point(333, 32);
      this.labelBASICVersion.Name = "labelBASICVersion";
      this.labelBASICVersion.Size = new System.Drawing.Size(79, 13);
      this.labelBASICVersion.TabIndex = 5;
      this.labelBASICVersion.Text = "BASIC Version:";
      // 
      // comboBASICVersion
      // 
      this.comboBASICVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBASICVersion.FormattingEnabled = true;
      this.comboBASICVersion.Location = new System.Drawing.Point(418, 28);
      this.comboBASICVersion.Name = "comboBASICVersion";
      this.comboBASICVersion.Size = new System.Drawing.Size(150, 21);
      this.comboBASICVersion.TabIndex = 6;
      this.comboBASICVersion.SelectedIndexChanged += new System.EventHandler(this.comboBASICVersion_SelectedIndexChanged);
      // 
      // SourceBasicEx
      // 
      this.AllowDrop = true;
      this.ClientSize = new System.Drawing.Size(698, 588);
      this.Controls.Add(this.comboBASICVersion);
      this.Controls.Add(this.labelBASICVersion);
      this.Controls.Add(this.labelStartAddress);
      this.Controls.Add(this.editBASICStartAddress);
      this.Controls.Add(this.menuBASIC);
      this.Controls.Add(this.editSource);
      this.Controls.Add(this.btnToggleStringEntryMode);
      this.Controls.Add(this.btnToggleUpperLowerCase);
      this.Controls.Add(this.btnToggleSymbolMode);
      this.Controls.Add(this.btnToggleLabelMode);
      this.MainMenuStrip = this.menuBASIC;
      this.Name = "SourceBasicEx";
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.editSource)).EndInit();
      this.contextSource.ResumeLayout(false);
      this.menuBASIC.ResumeLayout(false);
      this.menuBASIC.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    public FastColoredTextBoxNS.FastColoredTextBox    editSource;
    private System.Windows.Forms.ContextMenuStrip contextSource;
    private System.Windows.Forms.CheckBox btnToggleLabelMode;
    private System.Windows.Forms.MenuStrip menuBASIC;
    private System.Windows.Forms.ToolStripMenuItem bASICToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem renumberToolStripMenuItem;
    private C64Studio.Controls.CSCheckBox btnToggleSymbolMode;
    private System.Windows.Forms.ToolTip toolTip1;
    private C64Studio.Controls.CSCheckBox btnToggleUpperLowerCase;
    private System.Windows.Forms.TextBox editBASICStartAddress;
    private System.Windows.Forms.Label labelStartAddress;
    private System.Windows.Forms.Label labelBASICVersion;
    private System.Windows.Forms.ComboBox comboBASICVersion;
    private C64Studio.Controls.CSCheckBox btnToggleStringEntryMode;
    private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem renumberToolStripMenuItem1;
  }
}
