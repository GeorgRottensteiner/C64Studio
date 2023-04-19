namespace RetroDevStudio.Documents
{
  partial class CompileResult
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CompileResult));
      this.listMessages = new RetroDevStudio.Controls.CSListView();
      this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.contextCompilerMessage = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.jumpToFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.ignoreWarningToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.manageWarningIgnoreListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.copyListToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.imageListCompileResult = new System.Windows.Forms.ImageList(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).BeginInit();
      this.contextCompilerMessage.SuspendLayout();
      this.SuspendLayout();
      // 
      // listMessages
      // 
      this.listMessages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader5,
            this.columnHeader3,
            this.columnHeader4});
      this.listMessages.ContextMenuStrip = this.contextCompilerMessage;
      this.listMessages.Dock = System.Windows.Forms.DockStyle.Fill;
      this.listMessages.FullRowSelect = true;
      this.listMessages.HideSelection = false;
      this.listMessages.Location = new System.Drawing.Point(0, 0);
      this.listMessages.Name = "listMessages";
      this.listMessages.OwnerDraw = true;
      this.listMessages.SelectedTextBGColor = ((uint)(4278190335u));
      this.listMessages.SelectedTextColor = ((uint)(4294967295u));
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
      // columnHeader5
      // 
      this.columnHeader5.Text = "Code";
      this.columnHeader5.Width = 49;
      // 
      // columnHeader3
      // 
      this.columnHeader3.Text = "File";
      this.columnHeader3.Width = 200;
      // 
      // columnHeader4
      // 
      this.columnHeader4.Text = "Message";
      this.columnHeader4.Width = 400;
      // 
      // contextCompilerMessage
      // 
      this.contextCompilerMessage.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.jumpToFileToolStripMenuItem,
            this.ignoreWarningToolStripMenuItem,
            this.manageWarningIgnoreListToolStripMenuItem,
            this.copyListToClipboardToolStripMenuItem});
      this.contextCompilerMessage.Name = "contextCompilerMessage";
      this.contextCompilerMessage.Size = new System.Drawing.Size(233, 92);
      this.contextCompilerMessage.Opening += new System.ComponentModel.CancelEventHandler(this.contextCompilerMessage_Opening);
      // 
      // jumpToFileToolStripMenuItem
      // 
      this.jumpToFileToolStripMenuItem.Name = "jumpToFileToolStripMenuItem";
      this.jumpToFileToolStripMenuItem.Size = new System.Drawing.Size(232, 22);
      this.jumpToFileToolStripMenuItem.Text = "&Jump to file";
      this.jumpToFileToolStripMenuItem.Click += new System.EventHandler(this.jumpToFileToolStripMenuItem_Click);
      // 
      // ignoreWarningToolStripMenuItem
      // 
      this.ignoreWarningToolStripMenuItem.Name = "ignoreWarningToolStripMenuItem";
      this.ignoreWarningToolStripMenuItem.Size = new System.Drawing.Size(232, 22);
      this.ignoreWarningToolStripMenuItem.Text = "&Ignore Warning";
      this.ignoreWarningToolStripMenuItem.Click += new System.EventHandler(this.ignoreWarningToolStripMenuItem_Click);
      // 
      // manageWarningIgnoreListToolStripMenuItem
      // 
      this.manageWarningIgnoreListToolStripMenuItem.Name = "manageWarningIgnoreListToolStripMenuItem";
      this.manageWarningIgnoreListToolStripMenuItem.Size = new System.Drawing.Size(232, 22);
      this.manageWarningIgnoreListToolStripMenuItem.Text = "&Manage Warning Ignore List...";
      this.manageWarningIgnoreListToolStripMenuItem.Click += new System.EventHandler(this.manageWarningIgnoreListToolStripMenuItem_Click);
      // 
      // copyListToClipboardToolStripMenuItem
      // 
      this.copyListToClipboardToolStripMenuItem.Name = "copyListToClipboardToolStripMenuItem";
      this.copyListToClipboardToolStripMenuItem.Size = new System.Drawing.Size(232, 22);
      this.copyListToClipboardToolStripMenuItem.Text = "&Copy to Clipboard";
      this.copyListToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyListToClipboardToolStripMenuItem_Click);
      // 
      // imageListCompileResult
      // 
      this.imageListCompileResult.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListCompileResult.ImageStream")));
      this.imageListCompileResult.TransparentColor = System.Drawing.Color.Magenta;
      this.imageListCompileResult.Images.SetKeyName(0, "icon_error.bmp");
      this.imageListCompileResult.Images.SetKeyName(1, "icon_warning.bmp");
      this.imageListCompileResult.Images.SetKeyName(2, "icon_severe_warning.png");
      this.imageListCompileResult.Images.SetKeyName(3, "icon_info.bmp");
      // 
      // CompileResult
      // 
      this.ClientSize = new System.Drawing.Size(678, 200);
      this.Controls.Add(this.listMessages);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "CompileResult";
      this.Text = "Compiler Messages";
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).EndInit();
      this.contextCompilerMessage.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    //private System.Windows.Forms.ListView listMessages;
    private Controls.CSListView listMessages;
    private System.Windows.Forms.ColumnHeader columnHeader2;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader3;
    private System.Windows.Forms.ColumnHeader columnHeader4;
    private System.Windows.Forms.ImageList imageListCompileResult;
    private System.Windows.Forms.ColumnHeader columnHeader5;
    private System.Windows.Forms.ContextMenuStrip contextCompilerMessage;
    private System.Windows.Forms.ToolStripMenuItem jumpToFileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem ignoreWarningToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem manageWarningIgnoreListToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem copyListToClipboardToolStripMenuItem;



  }
}
