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
      this.btnToggleLabelMode = new System.Windows.Forms.CheckBox();
      this.menuBASIC = new System.Windows.Forms.MenuStrip();
      this.bASICToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.renumberToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.btnToggleSymbolMode = new System.Windows.Forms.CheckBox();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.editSource)).BeginInit();
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
      this.editSource.DragDrop += new System.Windows.Forms.DragEventHandler(this.editSource_DragDrop);
      this.editSource.DragEnter += new System.Windows.Forms.DragEventHandler(this.editSource_DragEnter);
      // 
      // contextSource
      // 
      this.contextSource.Name = "contextSource";
      this.contextSource.Size = new System.Drawing.Size(61, 4);
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
      // SourceBasicEx
      // 
      this.AllowDrop = true;
      this.ClientSize = new System.Drawing.Size(698, 588);
      this.Controls.Add(this.menuBASIC);
      this.Controls.Add(this.editSource);
      this.Controls.Add(this.btnToggleSymbolMode);
      this.Controls.Add(this.btnToggleLabelMode);
      this.MainMenuStrip = this.menuBASIC;
      this.Name = "SourceBasicEx";
      this.DragDrop += new System.Windows.Forms.DragEventHandler(this.SourceBasicEx_DragDrop);
      this.DragEnter += new System.Windows.Forms.DragEventHandler(this.SourceBasicEx_DragEnter);
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.editSource)).EndInit();
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
    private System.Windows.Forms.CheckBox btnToggleSymbolMode;
    private System.Windows.Forms.ToolTip toolTip1;
  }
}
