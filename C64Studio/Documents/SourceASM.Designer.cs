namespace C64Studio
{
  partial class SourceASM
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
      this.editSource = new ScintillaNET.Scintilla();
      this.contextSource = new System.Windows.Forms.ContextMenuStrip( this.components );
      this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
      this.runToCursorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.addToWatchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
      ( (System.ComponentModel.ISupportInitialize)( this.m_FileWatcher ) ).BeginInit();
      ( (System.ComponentModel.ISupportInitialize)( this.editSource ) ).BeginInit();
      this.contextSource.SuspendLayout();
      this.SuspendLayout();
      // 
      // editSource
      // 
      this.editSource.AllowDrop = true;
      this.editSource.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                  | System.Windows.Forms.AnchorStyles.Left )
                  | System.Windows.Forms.AnchorStyles.Right ) ) );
      this.editSource.ContextMenuStrip = this.contextSource;
      this.editSource.Font = new System.Drawing.Font( "Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
      this.editSource.Indentation.SmartIndentType = ScintillaNET.SmartIndent.Simple;
      this.editSource.Indentation.TabIndents = false;
      this.editSource.Indentation.TabWidth = 2;
      this.editSource.Indentation.UseTabs = false;
      this.editSource.Location = new System.Drawing.Point( 0, 29 );
      this.editSource.Name = "editSource";
      this.editSource.Size = new System.Drawing.Size( 576, 441 );
      this.editSource.Styles.BraceBad.FontName = "Verdana\0";
      this.editSource.Styles.BraceBad.Size = 9F;
      this.editSource.Styles.BraceLight.FontName = "Verdana\0";
      this.editSource.Styles.BraceLight.Size = 9F;
      this.editSource.Styles.ControlChar.FontName = "Verdana\0";
      this.editSource.Styles.ControlChar.Size = 9F;
      this.editSource.Styles.Default.BackColor = System.Drawing.SystemColors.Window;
      this.editSource.Styles.Default.FontName = "Verdana\0";
      this.editSource.Styles.Default.Size = 9F;
      this.editSource.Styles.IndentGuide.FontName = "Verdana\0";
      this.editSource.Styles.IndentGuide.Size = 9F;
      this.editSource.Styles.LastPredefined.FontName = "Verdana\0";
      this.editSource.Styles.LastPredefined.Size = 9F;
      this.editSource.Styles.LineNumber.FontName = "Verdana\0";
      this.editSource.Styles.LineNumber.Size = 9F;
      this.editSource.Styles.Max.FontName = "Verdana\0";
      this.editSource.Styles.Max.Size = 9F;
      this.editSource.TabIndex = 0;
      this.editSource.DragEnter += new System.Windows.Forms.DragEventHandler( this.editSource_DragEnter );
      this.editSource.DragDrop += new System.Windows.Forms.DragEventHandler( this.editSource_DragDrop );
      // 
      // contextSource
      // 
      this.contextSource.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.cutToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripSeparator4,
            this.runToCursorToolStripMenuItem,
            this.toolStripSeparator1,
            this.addToWatchToolStripMenuItem,
            this.toolStripSeparator2,
            this.gotoDeclarationToolStripMenuItem,
            this.showAddressToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.toolStripSeparator3,
            this.commentSelectionToolStripMenuItem,
            this.uncommentSelectionToolStripMenuItem,
            this.separatorCommenting,
            this.openFileToolStripMenuItem} );
      this.contextSource.Name = "contextSource";
      this.contextSource.Size = new System.Drawing.Size( 193, 276 );
      // 
      // copyToolStripMenuItem
      // 
      this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
      this.copyToolStripMenuItem.Size = new System.Drawing.Size( 192, 22 );
      this.copyToolStripMenuItem.Text = "&Copy";
      this.copyToolStripMenuItem.Click += new System.EventHandler( this.copyToolStripMenuItem_Click );
      // 
      // cutToolStripMenuItem
      // 
      this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
      this.cutToolStripMenuItem.Size = new System.Drawing.Size( 192, 22 );
      this.cutToolStripMenuItem.Text = "C&ut";
      this.cutToolStripMenuItem.Click += new System.EventHandler( this.cutToolStripMenuItem_Click );
      // 
      // pasteToolStripMenuItem
      // 
      this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
      this.pasteToolStripMenuItem.Size = new System.Drawing.Size( 192, 22 );
      this.pasteToolStripMenuItem.Text = "&Paste";
      this.pasteToolStripMenuItem.Click += new System.EventHandler( this.pasteToolStripMenuItem_Click );
      // 
      // toolStripSeparator4
      // 
      this.toolStripSeparator4.Name = "toolStripSeparator4";
      this.toolStripSeparator4.Size = new System.Drawing.Size( 189, 6 );
      // 
      // runToCursorToolStripMenuItem
      // 
      this.runToCursorToolStripMenuItem.Name = "runToCursorToolStripMenuItem";
      this.runToCursorToolStripMenuItem.Size = new System.Drawing.Size( 192, 22 );
      this.runToCursorToolStripMenuItem.Text = "Run to cursor";
      this.runToCursorToolStripMenuItem.Click += new System.EventHandler( this.runToCursorToolStripMenuItem_Click );
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size( 189, 6 );
      // 
      // addToWatchToolStripMenuItem
      // 
      this.addToWatchToolStripMenuItem.Name = "addToWatchToolStripMenuItem";
      this.addToWatchToolStripMenuItem.Size = new System.Drawing.Size( 192, 22 );
      this.addToWatchToolStripMenuItem.Text = "Add to Watch";
      this.addToWatchToolStripMenuItem.Click += new System.EventHandler( this.addToWatchToolStripMenuItem_Click );
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size( 189, 6 );
      // 
      // gotoDeclarationToolStripMenuItem
      // 
      this.gotoDeclarationToolStripMenuItem.Name = "gotoDeclarationToolStripMenuItem";
      this.gotoDeclarationToolStripMenuItem.Size = new System.Drawing.Size( 192, 22 );
      this.gotoDeclarationToolStripMenuItem.Text = "Goto declaration";
      this.gotoDeclarationToolStripMenuItem.Click += new System.EventHandler( this.gotoDeclarationToolStripMenuItem_Click );
      // 
      // showAddressToolStripMenuItem
      // 
      this.showAddressToolStripMenuItem.Name = "showAddressToolStripMenuItem";
      this.showAddressToolStripMenuItem.Size = new System.Drawing.Size( 192, 22 );
      this.showAddressToolStripMenuItem.Text = "Show runtime value";
      this.showAddressToolStripMenuItem.Click += new System.EventHandler( this.showAddressToolStripMenuItem_Click );
      // 
      // helpToolStripMenuItem
      // 
      this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
      this.helpToolStripMenuItem.Size = new System.Drawing.Size( 192, 22 );
      this.helpToolStripMenuItem.Text = "Help";
      this.helpToolStripMenuItem.Click += new System.EventHandler( this.helpToolStripMenuItem_Click );
      // 
      // toolStripSeparator3
      // 
      this.toolStripSeparator3.Name = "toolStripSeparator3";
      this.toolStripSeparator3.Size = new System.Drawing.Size( 189, 6 );
      // 
      // commentSelectionToolStripMenuItem
      // 
      this.commentSelectionToolStripMenuItem.Name = "commentSelectionToolStripMenuItem";
      this.commentSelectionToolStripMenuItem.Size = new System.Drawing.Size( 192, 22 );
      this.commentSelectionToolStripMenuItem.Text = "Comment Selection";
      this.commentSelectionToolStripMenuItem.Click += new System.EventHandler( this.commentSelectionToolStripMenuItem_Click );
      // 
      // uncommentSelectionToolStripMenuItem
      // 
      this.uncommentSelectionToolStripMenuItem.Name = "uncommentSelectionToolStripMenuItem";
      this.uncommentSelectionToolStripMenuItem.Size = new System.Drawing.Size( 192, 22 );
      this.uncommentSelectionToolStripMenuItem.Text = "Uncomment Selection";
      this.uncommentSelectionToolStripMenuItem.Click += new System.EventHandler( this.uncommentSelectionToolStripMenuItem_Click );
      // 
      // separatorCommenting
      // 
      this.separatorCommenting.Name = "separatorCommenting";
      this.separatorCommenting.Size = new System.Drawing.Size( 189, 6 );
      // 
      // openFileToolStripMenuItem
      // 
      this.openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
      this.openFileToolStripMenuItem.Size = new System.Drawing.Size( 192, 22 );
      this.openFileToolStripMenuItem.Text = "Open File";
      this.openFileToolStripMenuItem.Click += new System.EventHandler( this.openFileToolStripMenuItem_Click );
      // 
      // comboZoneSelector
      // 
      this.comboZoneSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboZoneSelector.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.comboZoneSelector.FormattingEnabled = true;
      this.comboZoneSelector.Location = new System.Drawing.Point( 0, 2 );
      this.comboZoneSelector.Name = "comboZoneSelector";
      this.comboZoneSelector.Size = new System.Drawing.Size( 318, 21 );
      this.comboZoneSelector.TabIndex = 1;
      this.comboZoneSelector.SelectionChangeCommitted += new System.EventHandler( this.comboZoneSelector_SelectedIndexChanged );
      // 
      // comboLocalLabelSelector
      // 
      this.comboLocalLabelSelector.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left )
                  | System.Windows.Forms.AnchorStyles.Right ) ) );
      this.comboLocalLabelSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboLocalLabelSelector.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.comboLocalLabelSelector.FormattingEnabled = true;
      this.comboLocalLabelSelector.Location = new System.Drawing.Point( 324, 2 );
      this.comboLocalLabelSelector.Name = "comboLocalLabelSelector";
      this.comboLocalLabelSelector.Size = new System.Drawing.Size( 252, 21 );
      this.comboLocalLabelSelector.TabIndex = 1;
      this.comboLocalLabelSelector.SelectionChangeCommitted += new System.EventHandler( this.comboLocalLabelSelector_SelectedIndexChanged );
      // 
      // SourceASM
      // 
      this.ClientSize = new System.Drawing.Size( 575, 471 );
      this.Controls.Add( this.comboZoneSelector );
      this.Controls.Add( this.comboLocalLabelSelector );
      this.Controls.Add( this.editSource );
      this.Name = "SourceASM";
      ( (System.ComponentModel.ISupportInitialize)( this.m_FileWatcher ) ).EndInit();
      ( (System.ComponentModel.ISupportInitialize)( this.editSource ) ).EndInit();
      this.contextSource.ResumeLayout( false );
      this.ResumeLayout( false );

    }

    #endregion

    public ScintillaNET.Scintilla editSource;
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
  }
}
