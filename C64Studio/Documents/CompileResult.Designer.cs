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
      this.listMessages = new DecentForms.ListControl();
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
      this.listMessages.BorderStyle = DecentForms.BorderStyle.SUNKEN;
      this.listMessages.ContextMenuStrip = this.contextCompilerMessage;
      this.listMessages.Dock = System.Windows.Forms.DockStyle.Fill;
      this.listMessages.FirstVisibleItemIndex = 0;
      this.listMessages.HasHeader = true;
      this.listMessages.HeaderHeight = 24;
      this.listMessages.ImageList = null;
      this.listMessages.ItemHeight = 15;
      this.listMessages.ListViewItemSorter = null;
      this.listMessages.Location = new System.Drawing.Point(0, 0);
      this.listMessages.Name = "listMessages";
      this.listMessages.ScrollAlwaysVisible = false;
      this.listMessages.SelectedIndex = -1;
      this.listMessages.SelectedItem = null;
      this.listMessages.SelectionMode = DecentForms.SelectionMode.NONE;
      this.listMessages.Size = new System.Drawing.Size(678, 200);
      this.listMessages.SortColumn = -1;
      this.listMessages.SortOrder = DecentForms.SortOrder.NONE;
      this.listMessages.TabIndex = 0;
      this.listMessages.ItemActivate += new DecentForms.EventHandler(this.listMessages_ItemActivate);
      this.listMessages.ColumnClicked += new DecentForms.EventHandler(this.listMessages_ColumnClicked);
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
    private DecentForms.ListControl listMessages;
    private System.Windows.Forms.ImageList imageListCompileResult;
    private System.Windows.Forms.ContextMenuStrip contextCompilerMessage;
    private System.Windows.Forms.ToolStripMenuItem jumpToFileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem ignoreWarningToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem manageWarningIgnoreListToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem copyListToClipboardToolStripMenuItem;



  }
}
