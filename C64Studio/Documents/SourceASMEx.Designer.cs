namespace C64Studio
{
  partial class SourceASMEx
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SourceASMEx));
      this.editSource = new FastColoredTextBoxNS.FastColoredTextBox();
      this.contextSource = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
      this.runToCursorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.addToWatchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.addDataBreakpointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.readAndWriteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.readOnlyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.writeOnlyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.addBreakpointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      this.gotoDeclarationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.showAddressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
      this.commentSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.uncommentSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.separatorCommenting = new System.Windows.Forms.ToolStripSeparator();
      this.openFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.comboZoneSelector = new System.Windows.Forms.ComboBox();
      this.comboLocalLabelSelector = new System.Windows.Forms.ComboBox();
      this.documentMap1 = new FastColoredTextBoxNS.DocumentMap();
      this.showMemoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.editSource)).BeginInit();
      this.contextSource.SuspendLayout();
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
      this.editSource.AutoScrollMinSize = new System.Drawing.Size(46, 14);
      this.editSource.BackBrush = null;
      this.editSource.ChangedLineColor = System.Drawing.Color.DarkOrange;
      this.editSource.CharHeight = 14;
      this.editSource.CharWidth = 7;
      this.editSource.ContextMenuStrip = this.contextSource;
      this.editSource.ConvertTabsToSpaces = false;
      this.editSource.Cursor = System.Windows.Forms.Cursors.IBeam;
      this.editSource.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
      this.editSource.Font = new System.Drawing.Font("Consolas", 9F);
      this.editSource.IsReplaceMode = false;
      this.editSource.Location = new System.Drawing.Point(0, 29);
      this.editSource.Name = "editSource";
      this.editSource.Paddings = new System.Windows.Forms.Padding(0);
      this.editSource.ReservedCountOfLineNumberChars = 4;
      this.editSource.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
      this.editSource.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("editSource.ServiceColors")));
      this.editSource.Size = new System.Drawing.Size(423, 441);
      this.editSource.TabIndex = 0;
      this.editSource.TabLength = 2;
      this.editSource.Zoom = 100;
      this.editSource.PaintLine += new System.EventHandler<FastColoredTextBoxNS.PaintLineEventArgs>(this.editSource_PaintLine);
      this.editSource.DragDrop += new System.Windows.Forms.DragEventHandler(this.editSource_DragDrop);
      this.editSource.DragEnter += new System.Windows.Forms.DragEventHandler(this.editSource_DragEnter);
      this.editSource.MouseClick += new System.Windows.Forms.MouseEventHandler(this.editSource_MouseClick);
      // 
      // contextSource
      // 
      this.contextSource.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.cutToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripSeparator4,
            this.runToCursorToolStripMenuItem,
            this.toolStripSeparator1,
            this.addToWatchToolStripMenuItem,
            this.addDataBreakpointToolStripMenuItem,
            this.addBreakpointToolStripMenuItem,
            this.toolStripSeparator2,
            this.gotoDeclarationToolStripMenuItem,
            this.showAddressToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.toolStripSeparator3,
            this.commentSelectionToolStripMenuItem,
            this.uncommentSelectionToolStripMenuItem,
            this.toolStripMenuItem1,
            this.showMemoryToolStripMenuItem,
            this.separatorCommenting,
            this.openFileToolStripMenuItem});
      this.contextSource.Name = "contextSource";
      this.contextSource.Size = new System.Drawing.Size(193, 320);
      // 
      // copyToolStripMenuItem
      // 
      this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
      this.copyToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
      this.copyToolStripMenuItem.Text = "&Copy";
      this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
      // 
      // cutToolStripMenuItem
      // 
      this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
      this.cutToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
      this.cutToolStripMenuItem.Text = "C&ut";
      this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
      // 
      // pasteToolStripMenuItem
      // 
      this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
      this.pasteToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
      this.pasteToolStripMenuItem.Text = "&Paste";
      this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
      // 
      // toolStripSeparator4
      // 
      this.toolStripSeparator4.Name = "toolStripSeparator4";
      this.toolStripSeparator4.Size = new System.Drawing.Size(189, 6);
      // 
      // runToCursorToolStripMenuItem
      // 
      this.runToCursorToolStripMenuItem.Name = "runToCursorToolStripMenuItem";
      this.runToCursorToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
      this.runToCursorToolStripMenuItem.Text = "Run to cursor";
      this.runToCursorToolStripMenuItem.Click += new System.EventHandler(this.runToCursorToolStripMenuItem_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(189, 6);
      // 
      // addToWatchToolStripMenuItem
      // 
      this.addToWatchToolStripMenuItem.Name = "addToWatchToolStripMenuItem";
      this.addToWatchToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
      this.addToWatchToolStripMenuItem.Text = "Add to Watch";
      this.addToWatchToolStripMenuItem.Click += new System.EventHandler(this.addToWatchToolStripMenuItem_Click);
      // 
      // addDataBreakpointToolStripMenuItem
      // 
      this.addDataBreakpointToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.readAndWriteToolStripMenuItem,
            this.readOnlyToolStripMenuItem,
            this.writeOnlyToolStripMenuItem});
      this.addDataBreakpointToolStripMenuItem.Name = "addDataBreakpointToolStripMenuItem";
      this.addDataBreakpointToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
      this.addDataBreakpointToolStripMenuItem.Text = "Add Data Breakpoint";
      // 
      // readAndWriteToolStripMenuItem
      // 
      this.readAndWriteToolStripMenuItem.Name = "readAndWriteToolStripMenuItem";
      this.readAndWriteToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
      this.readAndWriteToolStripMenuItem.Text = "Read and Write";
      this.readAndWriteToolStripMenuItem.Click += new System.EventHandler(this.readAndWriteToolStripMenuItem_Click);
      // 
      // readOnlyToolStripMenuItem
      // 
      this.readOnlyToolStripMenuItem.Name = "readOnlyToolStripMenuItem";
      this.readOnlyToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
      this.readOnlyToolStripMenuItem.Text = "Read only";
      this.readOnlyToolStripMenuItem.Click += new System.EventHandler(this.readOnlyToolStripMenuItem_Click);
      // 
      // writeOnlyToolStripMenuItem
      // 
      this.writeOnlyToolStripMenuItem.Name = "writeOnlyToolStripMenuItem";
      this.writeOnlyToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
      this.writeOnlyToolStripMenuItem.Text = "Write only";
      this.writeOnlyToolStripMenuItem.Click += new System.EventHandler(this.writeOnlyToolStripMenuItem_Click);
      // 
      // addBreakpointToolStripMenuItem
      // 
      this.addBreakpointToolStripMenuItem.Name = "addBreakpointToolStripMenuItem";
      this.addBreakpointToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
      this.addBreakpointToolStripMenuItem.Text = "Add Breakpoint";
      this.addBreakpointToolStripMenuItem.Click += new System.EventHandler(this.addBreakpointToolStripMenuItem_Click);
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size(189, 6);
      // 
      // gotoDeclarationToolStripMenuItem
      // 
      this.gotoDeclarationToolStripMenuItem.Name = "gotoDeclarationToolStripMenuItem";
      this.gotoDeclarationToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
      this.gotoDeclarationToolStripMenuItem.Text = "Goto declaration";
      this.gotoDeclarationToolStripMenuItem.Click += new System.EventHandler(this.gotoDeclarationToolStripMenuItem_Click);
      // 
      // showAddressToolStripMenuItem
      // 
      this.showAddressToolStripMenuItem.Name = "showAddressToolStripMenuItem";
      this.showAddressToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
      this.showAddressToolStripMenuItem.Text = "Show runtime value";
      this.showAddressToolStripMenuItem.Click += new System.EventHandler(this.showAddressToolStripMenuItem_Click);
      // 
      // helpToolStripMenuItem
      // 
      this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
      this.helpToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
      this.helpToolStripMenuItem.Text = "Help";
      this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
      // 
      // toolStripSeparator3
      // 
      this.toolStripSeparator3.Name = "toolStripSeparator3";
      this.toolStripSeparator3.Size = new System.Drawing.Size(189, 6);
      // 
      // commentSelectionToolStripMenuItem
      // 
      this.commentSelectionToolStripMenuItem.Name = "commentSelectionToolStripMenuItem";
      this.commentSelectionToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
      this.commentSelectionToolStripMenuItem.Text = "Comment Selection";
      this.commentSelectionToolStripMenuItem.Click += new System.EventHandler(this.commentSelectionToolStripMenuItem_Click);
      // 
      // uncommentSelectionToolStripMenuItem
      // 
      this.uncommentSelectionToolStripMenuItem.Name = "uncommentSelectionToolStripMenuItem";
      this.uncommentSelectionToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
      this.uncommentSelectionToolStripMenuItem.Text = "Uncomment Selection";
      this.uncommentSelectionToolStripMenuItem.Click += new System.EventHandler(this.uncommentSelectionToolStripMenuItem_Click);
      // 
      // separatorCommenting
      // 
      this.separatorCommenting.Name = "separatorCommenting";
      this.separatorCommenting.Size = new System.Drawing.Size(189, 6);
      // 
      // openFileToolStripMenuItem
      // 
      this.openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
      this.openFileToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
      this.openFileToolStripMenuItem.Text = "Open File";
      this.openFileToolStripMenuItem.Click += new System.EventHandler(this.openFileToolStripMenuItem_Click);
      // 
      // comboZoneSelector
      // 
      this.comboZoneSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboZoneSelector.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.comboZoneSelector.FormattingEnabled = true;
      this.comboZoneSelector.Location = new System.Drawing.Point(0, 2);
      this.comboZoneSelector.Name = "comboZoneSelector";
      this.comboZoneSelector.Size = new System.Drawing.Size(318, 21);
      this.comboZoneSelector.TabIndex = 1;
      this.comboZoneSelector.SelectionChangeCommitted += new System.EventHandler(this.comboZoneSelector_SelectedIndexChanged);
      // 
      // comboLocalLabelSelector
      // 
      this.comboLocalLabelSelector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.comboLocalLabelSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboLocalLabelSelector.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.comboLocalLabelSelector.FormattingEnabled = true;
      this.comboLocalLabelSelector.Location = new System.Drawing.Point(324, 2);
      this.comboLocalLabelSelector.Name = "comboLocalLabelSelector";
      this.comboLocalLabelSelector.Size = new System.Drawing.Size(252, 21);
      this.comboLocalLabelSelector.TabIndex = 1;
      this.comboLocalLabelSelector.SelectionChangeCommitted += new System.EventHandler(this.comboLocalLabelSelector_SelectedIndexChanged);
      // 
      // documentMap1
      // 
      this.documentMap1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.documentMap1.ForeColor = System.Drawing.Color.Maroon;
      this.documentMap1.Location = new System.Drawing.Point(429, 29);
      this.documentMap1.Name = "documentMap1";
      this.documentMap1.Size = new System.Drawing.Size(147, 441);
      this.documentMap1.TabIndex = 2;
      this.documentMap1.Target = this.editSource;
      this.documentMap1.Text = "documentMap1";
      // 
      // showMemoryToolStripMenuItem
      // 
      this.showMemoryToolStripMenuItem.Name = "showMemoryToolStripMenuItem";
      this.showMemoryToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
      this.showMemoryToolStripMenuItem.Text = "Show Memory";
      this.showMemoryToolStripMenuItem.Click += new System.EventHandler(this.addShowMemoryToolStripMenuItem_Click);
      // 
      // toolStripMenuItem1
      // 
      this.toolStripMenuItem1.Name = "toolStripMenuItem1";
      this.toolStripMenuItem1.Size = new System.Drawing.Size(189, 6);
      // 
      // SourceASMEx
      // 
      this.ClientSize = new System.Drawing.Size(575, 471);
      this.Controls.Add(this.documentMap1);
      this.Controls.Add(this.comboZoneSelector);
      this.Controls.Add(this.comboLocalLabelSelector);
      this.Controls.Add(this.editSource);
      this.Name = "SourceASMEx";
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.editSource)).EndInit();
      this.contextSource.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    public FastColoredTextBoxNS.FastColoredTextBox editSource;
    private System.Windows.Forms.ContextMenuStrip contextSource;
    private System.Windows.Forms.ToolStripMenuItem runToCursorToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem addToWatchToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripMenuItem gotoDeclarationToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem showAddressToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    private System.Windows.Forms.ToolStripMenuItem openFileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    private System.Windows.Forms.ComboBox comboZoneSelector;
    private System.Windows.Forms.ComboBox comboLocalLabelSelector;
    private System.Windows.Forms.ToolStripMenuItem commentSelectionToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem uncommentSelectionToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator separatorCommenting;
    private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
    private FastColoredTextBoxNS.DocumentMap documentMap1;
    private System.Windows.Forms.ToolStripMenuItem addDataBreakpointToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem addBreakpointToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem readAndWriteToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem readOnlyToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem writeOnlyToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem showMemoryToolStripMenuItem;
  }
}
