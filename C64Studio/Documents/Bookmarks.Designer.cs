namespace RetroDevStudio.Documents
{
  partial class Bookmarks
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Bookmarks));
      this.listMessages = new System.Windows.Forms.ListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.contextMenuBookmarks = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.jumpToFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.deleteBookmarkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.deleteAllBookmarksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.imageListCompileResult = new System.Windows.Forms.ImageList(this.components);
      this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).BeginInit();
      this.contextMenuBookmarks.SuspendLayout();
      this.SuspendLayout();
      // 
      // listMessages
      // 
      this.listMessages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
      this.listMessages.ContextMenuStrip = this.contextMenuBookmarks;
      this.listMessages.Dock = System.Windows.Forms.DockStyle.Fill;
      this.listMessages.FullRowSelect = true;
      this.listMessages.HideSelection = false;
      this.listMessages.Location = new System.Drawing.Point(0, 0);
      this.listMessages.Name = "listMessages";
      this.listMessages.Size = new System.Drawing.Size(678, 200);
      this.listMessages.SmallImageList = this.imageListCompileResult;
      this.listMessages.TabIndex = 0;
      this.listMessages.UseCompatibleStateImageBehavior = false;
      this.listMessages.View = System.Windows.Forms.View.Details;
      this.listMessages.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listMessages_ColumnClick);
      this.listMessages.ItemActivate += new System.EventHandler(this.listMessages_ItemActivate);
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = ".";
      this.columnHeader1.Width = 20;
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "Line";
      this.columnHeader2.Width = 50;
      // 
      // columnHeader3
      // 
      this.columnHeader3.Text = "File";
      this.columnHeader3.Width = 400;
      // 
      // contextMenuBookmarks
      // 
      this.contextMenuBookmarks.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.jumpToFileToolStripMenuItem,
            this.toolStripSeparator1,
            this.deleteBookmarkToolStripMenuItem,
            this.deleteAllBookmarksToolStripMenuItem});
      this.contextMenuBookmarks.Name = "contextCompilerMessage";
      this.contextMenuBookmarks.Size = new System.Drawing.Size(185, 76);
      // 
      // jumpToFileToolStripMenuItem
      // 
      this.jumpToFileToolStripMenuItem.Name = "jumpToFileToolStripMenuItem";
      this.jumpToFileToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
      this.jumpToFileToolStripMenuItem.Text = "&Jump to file";
      this.jumpToFileToolStripMenuItem.Click += new System.EventHandler(this.jumpToFileToolStripMenuItem_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(181, 6);
      // 
      // deleteBookmarkToolStripMenuItem
      // 
      this.deleteBookmarkToolStripMenuItem.Name = "deleteBookmarkToolStripMenuItem";
      this.deleteBookmarkToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
      this.deleteBookmarkToolStripMenuItem.Text = "Delete bookmark";
      this.deleteBookmarkToolStripMenuItem.Click += new System.EventHandler(this.deleteBookmarkToolStripMenuItem_Click);
      // 
      // deleteAllBookmarksToolStripMenuItem
      // 
      this.deleteAllBookmarksToolStripMenuItem.Name = "deleteAllBookmarksToolStripMenuItem";
      this.deleteAllBookmarksToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
      this.deleteAllBookmarksToolStripMenuItem.Text = "Delete all bookmarks";
      this.deleteAllBookmarksToolStripMenuItem.Click += new System.EventHandler(this.deleteAllBookmarksToolStripMenuItem_Click);
      // 
      // imageListCompileResult
      // 
      this.imageListCompileResult.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListCompileResult.ImageStream")));
      this.imageListCompileResult.TransparentColor = System.Drawing.Color.Magenta;
      this.imageListCompileResult.Images.SetKeyName(0, "bookmark-icon.ico");
      // 
      // columnHeader4
      // 
      this.columnHeader4.Text = "Comment";
      this.columnHeader4.Width = 250;
      // 
      // Bookmarks
      // 
      this.ClientSize = new System.Drawing.Size(678, 200);
      this.Controls.Add(this.listMessages);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "Bookmarks";
      this.Text = "Bookmarks";
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).EndInit();
      this.contextMenuBookmarks.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListView listMessages;
    private System.Windows.Forms.ColumnHeader columnHeader2;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader3;
    private System.Windows.Forms.ImageList imageListCompileResult;
    private System.Windows.Forms.ContextMenuStrip contextMenuBookmarks;
    private System.Windows.Forms.ToolStripMenuItem jumpToFileToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem deleteBookmarkToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem deleteAllBookmarksToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeader4;
    }
}
