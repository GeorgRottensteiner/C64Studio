namespace RetroDevStudio.Documents
{
  partial class TextFile
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
      components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextFile));
      editSource = new FastColoredTextBoxNS.FastColoredTextBox();
      contextSource = new System.Windows.Forms.ContextMenuStrip( components );
      copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
      toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      addBookmarkHereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      removeBookmarkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      removeAllBookmarksOfThisFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
      goToLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      showAddressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
      toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
      showMemoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      showMiniOverviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      separatorCommenting = new System.Windows.Forms.ToolStripSeparator();
      openFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
      readAndWriteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      readOnlyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      writeOnlyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
      miniMap = new FastColoredTextBoxNS.DocumentMap();
      contextMenuMiniMap = new System.Windows.Forms.ContextMenuStrip( components );
      hideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      toolTip1 = new System.Windows.Forms.ToolTip( components );
      ( (System.ComponentModel.ISupportInitialize)m_FileWatcher ).BeginInit();
      ( (System.ComponentModel.ISupportInitialize)editSource ).BeginInit();
      contextSource.SuspendLayout();
      contextMenuMiniMap.SuspendLayout();
      SuspendLayout();
      // 
      // editSource
      // 
      editSource.Anchor =    System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Bottom   |  System.Windows.Forms.AnchorStyles.Left   |  System.Windows.Forms.AnchorStyles.Right ;
      editSource.AutoCompleteBracketsList = new char[]
  {
    '(',
    ')',
    '{',
    '}',
    '[',
    ']',
    '"',
    '"',
    '\'',
    '\''
  };
      editSource.AutoScrollMinSize = new System.Drawing.Size( 46, 14 );
      editSource.BackBrush = null;
      editSource.ChangedLineColor = System.Drawing.Color.DarkOrange;
      editSource.CharHeight = 14;
      editSource.CharWidth = 7;
      editSource.ContextMenuStrip = contextSource;
      editSource.ConvertTabsToSpaces = false;
      editSource.Cursor = System.Windows.Forms.Cursors.IBeam;
      editSource.DisabledColor = System.Drawing.Color.FromArgb( 100, 180, 180, 180 );
      editSource.Font = new System.Drawing.Font( "Consolas", 9F );
      editSource.IsReplaceMode = false;
      editSource.Location = new System.Drawing.Point( 0, 0 );
      editSource.Name = "editSource";
      editSource.Paddings = new System.Windows.Forms.Padding( 0 );
      editSource.ReservedCountOfLineNumberChars = 4;
      editSource.SelectionColor = System.Drawing.Color.FromArgb( 60, 0, 0, 255 );
      editSource.ServiceColors = (FastColoredTextBoxNS.ServiceColors)resources.GetObject( "editSource.ServiceColors" );
      editSource.Size = new System.Drawing.Size( 423, 470 );
      editSource.TabIndex = 0;
      editSource.TabLength = 2;
      editSource.Zoom = 100;
      editSource.LineVisited +=  editSource_LineVisited ;
      editSource.DragDrop +=  editSource_DragDrop ;
      editSource.DragEnter +=  editSource_DragEnter ;
      // 
      // contextSource
      // 
      contextSource.ImageScalingSize = new System.Drawing.Size( 28, 28 );
      contextSource.Items.AddRange( new System.Windows.Forms.ToolStripItem[] { copyToolStripMenuItem, cutToolStripMenuItem, pasteToolStripMenuItem, toolStripSeparator4, toolStripSeparator1, toolStripSeparator2, addBookmarkHereToolStripMenuItem, removeBookmarkToolStripMenuItem, removeAllBookmarksOfThisFileToolStripMenuItem, toolStripSeparator5, goToLineToolStripMenuItem, showAddressToolStripMenuItem, helpToolStripMenuItem, toolStripSeparator3, toolStripMenuItem1, showMemoryToolStripMenuItem, showMiniOverviewToolStripMenuItem, separatorCommenting, openFileToolStripMenuItem } );
      contextSource.Name = "contextSource";
      contextSource.Size = new System.Drawing.Size( 250, 310 );
      // 
      // copyToolStripMenuItem
      // 
      copyToolStripMenuItem.Name = "copyToolStripMenuItem";
      copyToolStripMenuItem.Size = new System.Drawing.Size( 249, 22 );
      copyToolStripMenuItem.Text = "&Copy";
      copyToolStripMenuItem.Click +=  copyToolStripMenuItem_Click ;
      // 
      // cutToolStripMenuItem
      // 
      cutToolStripMenuItem.Name = "cutToolStripMenuItem";
      cutToolStripMenuItem.Size = new System.Drawing.Size( 249, 22 );
      cutToolStripMenuItem.Text = "C&ut";
      // 
      // pasteToolStripMenuItem
      // 
      pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
      pasteToolStripMenuItem.Size = new System.Drawing.Size( 249, 22 );
      pasteToolStripMenuItem.Text = "&Paste";
      pasteToolStripMenuItem.Click +=  pasteToolStripMenuItem_Click ;
      // 
      // toolStripSeparator4
      // 
      toolStripSeparator4.Name = "toolStripSeparator4";
      toolStripSeparator4.Size = new System.Drawing.Size( 246, 6 );
      // 
      // toolStripSeparator1
      // 
      toolStripSeparator1.Name = "toolStripSeparator1";
      toolStripSeparator1.Size = new System.Drawing.Size( 246, 6 );
      // 
      // toolStripSeparator2
      // 
      toolStripSeparator2.Name = "toolStripSeparator2";
      toolStripSeparator2.Size = new System.Drawing.Size( 246, 6 );
      // 
      // addBookmarkHereToolStripMenuItem
      // 
      addBookmarkHereToolStripMenuItem.Name = "addBookmarkHereToolStripMenuItem";
      addBookmarkHereToolStripMenuItem.Size = new System.Drawing.Size( 249, 22 );
      addBookmarkHereToolStripMenuItem.Text = "Add Bookmark here";
      addBookmarkHereToolStripMenuItem.Click +=  addBookmarkHereToolStripMenuItem_Click ;
      // 
      // removeBookmarkToolStripMenuItem
      // 
      removeBookmarkToolStripMenuItem.Name = "removeBookmarkToolStripMenuItem";
      removeBookmarkToolStripMenuItem.Size = new System.Drawing.Size( 249, 22 );
      removeBookmarkToolStripMenuItem.Text = "Remove Bookmark";
      removeBookmarkToolStripMenuItem.Click +=  removeBookmarkToolStripMenuItem_Click ;
      // 
      // removeAllBookmarksOfThisFileToolStripMenuItem
      // 
      removeAllBookmarksOfThisFileToolStripMenuItem.Name = "removeAllBookmarksOfThisFileToolStripMenuItem";
      removeAllBookmarksOfThisFileToolStripMenuItem.Size = new System.Drawing.Size( 249, 22 );
      removeAllBookmarksOfThisFileToolStripMenuItem.Text = "Remove all bookmarks of this file";
      removeAllBookmarksOfThisFileToolStripMenuItem.Click +=  removeAllBookmarksOfThisFileToolStripMenuItem_Click ;
      // 
      // toolStripSeparator5
      // 
      toolStripSeparator5.Name = "toolStripSeparator5";
      toolStripSeparator5.Size = new System.Drawing.Size( 246, 6 );
      // 
      // goToLineToolStripMenuItem
      // 
      goToLineToolStripMenuItem.Name = "goToLineToolStripMenuItem";
      goToLineToolStripMenuItem.Size = new System.Drawing.Size( 249, 22 );
      goToLineToolStripMenuItem.Text = "Jump to line...";
      goToLineToolStripMenuItem.Click +=  goToLineToolStripMenuItem_Click ;
      // 
      // showAddressToolStripMenuItem
      // 
      showAddressToolStripMenuItem.Name = "showAddressToolStripMenuItem";
      showAddressToolStripMenuItem.Size = new System.Drawing.Size( 249, 22 );
      showAddressToolStripMenuItem.Text = "Show runtime value";
      // 
      // helpToolStripMenuItem
      // 
      helpToolStripMenuItem.Name = "helpToolStripMenuItem";
      helpToolStripMenuItem.Size = new System.Drawing.Size( 249, 22 );
      helpToolStripMenuItem.Text = "Help";
      // 
      // toolStripSeparator3
      // 
      toolStripSeparator3.Name = "toolStripSeparator3";
      toolStripSeparator3.Size = new System.Drawing.Size( 246, 6 );
      // 
      // toolStripMenuItem1
      // 
      toolStripMenuItem1.Name = "toolStripMenuItem1";
      toolStripMenuItem1.Size = new System.Drawing.Size( 246, 6 );
      // 
      // showMemoryToolStripMenuItem
      // 
      showMemoryToolStripMenuItem.Name = "showMemoryToolStripMenuItem";
      showMemoryToolStripMenuItem.Size = new System.Drawing.Size( 249, 22 );
      showMemoryToolStripMenuItem.Text = "Show Memory";
      // 
      // showMiniOverviewToolStripMenuItem
      // 
      showMiniOverviewToolStripMenuItem.Name = "showMiniOverviewToolStripMenuItem";
      showMiniOverviewToolStripMenuItem.Size = new System.Drawing.Size( 249, 22 );
      showMiniOverviewToolStripMenuItem.Text = "Show Mini Overview";
      showMiniOverviewToolStripMenuItem.Click +=  showMiniOverviewToolStripMenuItem_Click ;
      // 
      // separatorCommenting
      // 
      separatorCommenting.Name = "separatorCommenting";
      separatorCommenting.Size = new System.Drawing.Size( 246, 6 );
      // 
      // openFileToolStripMenuItem
      // 
      openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
      openFileToolStripMenuItem.Size = new System.Drawing.Size( 249, 22 );
      // 
      // toolStripSeparator6
      // 
      toolStripSeparator6.Name = "toolStripSeparator6";
      toolStripSeparator6.Size = new System.Drawing.Size( 174, 6 );
      // 
      // readAndWriteToolStripMenuItem
      // 
      readAndWriteToolStripMenuItem.Name = "readAndWriteToolStripMenuItem";
      readAndWriteToolStripMenuItem.Size = new System.Drawing.Size( 154, 22 );
      readAndWriteToolStripMenuItem.Text = "Read and Write";
      // 
      // readOnlyToolStripMenuItem
      // 
      readOnlyToolStripMenuItem.Name = "readOnlyToolStripMenuItem";
      readOnlyToolStripMenuItem.Size = new System.Drawing.Size( 154, 22 );
      readOnlyToolStripMenuItem.Text = "Read only";
      // 
      // writeOnlyToolStripMenuItem
      // 
      writeOnlyToolStripMenuItem.Name = "writeOnlyToolStripMenuItem";
      writeOnlyToolStripMenuItem.Size = new System.Drawing.Size( 154, 22 );
      writeOnlyToolStripMenuItem.Text = "Write only";
      // 
      // toolStripSeparator8
      // 
      toolStripSeparator8.Name = "toolStripSeparator8";
      toolStripSeparator8.Size = new System.Drawing.Size( 163, 6 );
      // 
      // miniMap
      // 
      miniMap.Anchor =   System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Bottom   |  System.Windows.Forms.AnchorStyles.Right ;
      miniMap.ContextMenuStrip = contextMenuMiniMap;
      miniMap.ForeColor = System.Drawing.Color.Maroon;
      miniMap.Location = new System.Drawing.Point( 429, 0 );
      miniMap.Name = "miniMap";
      miniMap.Size = new System.Drawing.Size( 147, 470 );
      miniMap.TabIndex = 2;
      miniMap.Target = editSource;
      miniMap.Text = "documentMap1";
      // 
      // contextMenuMiniMap
      // 
      contextMenuMiniMap.ImageScalingSize = new System.Drawing.Size( 28, 28 );
      contextMenuMiniMap.Items.AddRange( new System.Windows.Forms.ToolStripItem[] { hideToolStripMenuItem } );
      contextMenuMiniMap.Name = "contextMenuMiniMap";
      contextMenuMiniMap.Size = new System.Drawing.Size( 100, 26 );
      // 
      // hideToolStripMenuItem
      // 
      hideToolStripMenuItem.Name = "hideToolStripMenuItem";
      hideToolStripMenuItem.Size = new System.Drawing.Size( 99, 22 );
      hideToolStripMenuItem.Text = "Hide";
      hideToolStripMenuItem.Click +=  hideToolStripMenuItem_Click ;
      // 
      // TextFile
      // 
      AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      ClientSize = new System.Drawing.Size( 575, 471 );
      Controls.Add( miniMap );
      Controls.Add( editSource );
      Name = "TextFile";
      ( (System.ComponentModel.ISupportInitialize)m_FileWatcher ).EndInit();
      ( (System.ComponentModel.ISupportInitialize)editSource ).EndInit();
      contextSource.ResumeLayout( false );
      contextMenuMiniMap.ResumeLayout( false );
      ResumeLayout( false );

    }

    #endregion

    public FastColoredTextBoxNS.FastColoredTextBox editSource;
    private System.Windows.Forms.ContextMenuStrip contextSource;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripMenuItem showAddressToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    private System.Windows.Forms.ToolStripMenuItem openFileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    private System.Windows.Forms.ToolStripSeparator separatorCommenting;
    private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
    private FastColoredTextBoxNS.DocumentMap miniMap;
    private System.Windows.Forms.ToolStripMenuItem readAndWriteToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem readOnlyToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem writeOnlyToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1; 
    private System.Windows.Forms.ToolStripMenuItem showMemoryToolStripMenuItem;
    private System.Windows.Forms.ContextMenuStrip contextMenuMiniMap;
    private System.Windows.Forms.ToolStripMenuItem hideToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem showMiniOverviewToolStripMenuItem;
    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.ToolStripMenuItem addBookmarkHereToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem removeBookmarkToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem removeAllBookmarksOfThisFileToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
    private System.Windows.Forms.ToolStripMenuItem goToLineToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
  }
}
