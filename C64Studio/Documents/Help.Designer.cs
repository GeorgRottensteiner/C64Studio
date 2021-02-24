namespace C64Studio
{
  partial class Help
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Help));
      this.toolStripNavigation = new System.Windows.Forms.ToolStrip();
      this.toolStripBtnBack = new System.Windows.Forms.ToolStripButton();
      this.toolStripBtnForward = new System.Windows.Forms.ToolStripButton();
      this.toolStripBtnHome = new System.Windows.Forms.ToolStripButton();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.toolStripBtnZoomIn = new System.Windows.Forms.ToolStripButton();
      this.toolStripBtnZoomOut = new System.Windows.Forms.ToolStripButton();
      this.toolStripBtnZoomReset = new System.Windows.Forms.ToolStripButton();
      this.webBrowser = new C64Studio.ZoomBrowser();
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).BeginInit();
      this.toolStripNavigation.SuspendLayout();
      this.SuspendLayout();
      // 
      // toolStripNavigation
      // 
      this.toolStripNavigation.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripBtnBack,
            this.toolStripBtnForward,
            this.toolStripBtnHome,
            this.toolStripSeparator1,
            this.toolStripBtnZoomIn,
            this.toolStripBtnZoomOut,
            this.toolStripBtnZoomReset});
      this.toolStripNavigation.Location = new System.Drawing.Point(0, 0);
      this.toolStripNavigation.Name = "toolStripNavigation";
      this.toolStripNavigation.Size = new System.Drawing.Size(896, 25);
      this.toolStripNavigation.TabIndex = 1;
      this.toolStripNavigation.Text = "toolStrip1";
      // 
      // toolStripBtnBack
      // 
      this.toolStripBtnBack.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.toolStripBtnBack.Image = ((System.Drawing.Image)(resources.GetObject("toolStripBtnBack.Image")));
      this.toolStripBtnBack.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripBtnBack.Name = "toolStripBtnBack";
      this.toolStripBtnBack.Size = new System.Drawing.Size(36, 22);
      this.toolStripBtnBack.Text = "Back";
      this.toolStripBtnBack.Click += new System.EventHandler(this.toolStripBtnBack_Click);
      // 
      // toolStripBtnForward
      // 
      this.toolStripBtnForward.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.toolStripBtnForward.Image = ((System.Drawing.Image)(resources.GetObject("toolStripBtnForward.Image")));
      this.toolStripBtnForward.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripBtnForward.Name = "toolStripBtnForward";
      this.toolStripBtnForward.Size = new System.Drawing.Size(54, 22);
      this.toolStripBtnForward.Text = "Forward";
      this.toolStripBtnForward.Click += new System.EventHandler(this.toolStripBtnForward_Click);
      // 
      // toolStripBtnHome
      // 
      this.toolStripBtnHome.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
      this.toolStripBtnHome.Image = ((System.Drawing.Image)(resources.GetObject("toolStripBtnHome.Image")));
      this.toolStripBtnHome.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripBtnHome.Name = "toolStripBtnHome";
      this.toolStripBtnHome.Size = new System.Drawing.Size(44, 22);
      this.toolStripBtnHome.Text = "Home";
      this.toolStripBtnHome.Click += new System.EventHandler(this.toolStripBtnHome_Click);
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
      // 
      // toolStripBtnZoomIn
      // 
      this.toolStripBtnZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripBtnZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("toolStripBtnZoomIn.Image")));
      this.toolStripBtnZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripBtnZoomIn.Name = "toolStripBtnZoomIn";
      this.toolStripBtnZoomIn.Size = new System.Drawing.Size(23, 22);
      this.toolStripBtnZoomIn.Text = "Zoom In";
      this.toolStripBtnZoomIn.Click += new System.EventHandler(this.toolStripBtnZoomIn_Click);
      // 
      // toolStripBtnZoomOut
      // 
      this.toolStripBtnZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripBtnZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("toolStripBtnZoomOut.Image")));
      this.toolStripBtnZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripBtnZoomOut.Name = "toolStripBtnZoomOut";
      this.toolStripBtnZoomOut.Size = new System.Drawing.Size(23, 22);
      this.toolStripBtnZoomOut.Text = "Zoom Out";
      this.toolStripBtnZoomOut.Click += new System.EventHandler(this.toolStripBtnZoomOut_Click);
      // 
      // toolStripBtnZoomReset
      // 
      this.toolStripBtnZoomReset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
      this.toolStripBtnZoomReset.Image = ((System.Drawing.Image)(resources.GetObject("toolStripBtnZoomReset.Image")));
      this.toolStripBtnZoomReset.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.toolStripBtnZoomReset.Name = "toolStripBtnZoomReset";
      this.toolStripBtnZoomReset.Size = new System.Drawing.Size(23, 22);
      this.toolStripBtnZoomReset.Text = "Reset Zoom";
      this.toolStripBtnZoomReset.Click += new System.EventHandler(this.toolStripBtnZoomReset_Click);
      // 
      // webBrowser
      // 
      this.webBrowser.AllowWebBrowserDrop = false;
      this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
      this.webBrowser.Location = new System.Drawing.Point(0, 25);
      this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
      this.webBrowser.Name = "webBrowser";
      this.webBrowser.ScriptErrorsSuppressed = true;
      this.webBrowser.Size = new System.Drawing.Size(896, 679);
      this.webBrowser.TabIndex = 2;
      // 
      // Help
      // 
      this.ClientSize = new System.Drawing.Size(896, 704);
      this.Controls.Add(this.webBrowser);
      this.Controls.Add(this.toolStripNavigation);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "Help";
      this.Text = "Help";
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).EndInit();
      this.toolStripNavigation.ResumeLayout(false);
      this.toolStripNavigation.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ToolStrip toolStripNavigation;
    private ZoomBrowser webBrowser;
    private System.Windows.Forms.ToolStripButton toolStripBtnBack;
    private System.Windows.Forms.ToolStripButton toolStripBtnForward;
    private System.Windows.Forms.ToolStripButton toolStripBtnHome;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripButton toolStripBtnZoomIn;
    private System.Windows.Forms.ToolStripButton toolStripBtnZoomOut;
    private System.Windows.Forms.ToolStripButton toolStripBtnZoomReset;


  }
}
