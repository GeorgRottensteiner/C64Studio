namespace RetroDevStudio.Documents
{
  partial class SampleExplorer
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SampleExplorer));
      this.toolStripNavigation = new System.Windows.Forms.ToolStrip();
      this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
      this.editSampleFilter = new System.Windows.Forms.ToolStripTextBox();
      this.gridSamples = new DecentForms.GridList();
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).BeginInit();
      this.toolStripNavigation.SuspendLayout();
      this.SuspendLayout();
      // 
      // toolStripNavigation
      // 
      this.toolStripNavigation.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.editSampleFilter});
      this.toolStripNavigation.Location = new System.Drawing.Point(0, 0);
      this.toolStripNavigation.Name = "toolStripNavigation";
      this.toolStripNavigation.Size = new System.Drawing.Size(896, 25);
      this.toolStripNavigation.TabIndex = 1;
      this.toolStripNavigation.Text = "toolStrip1";
      // 
      // toolStripLabel1
      // 
      this.toolStripLabel1.Name = "toolStripLabel1";
      this.toolStripLabel1.Size = new System.Drawing.Size(36, 22);
      this.toolStripLabel1.Text = "Filter:";
      // 
      // editSampleFilter
      // 
      this.editSampleFilter.AutoSize = false;
      this.editSampleFilter.Font = new System.Drawing.Font("Segoe UI", 9F);
      this.editSampleFilter.Name = "editSampleFilter";
      this.editSampleFilter.Size = new System.Drawing.Size(300, 25);
      this.editSampleFilter.TextChanged += new System.EventHandler(this.editSampleFilter_TextChanged);
      // 
      // gridSamples
      // 
      this.gridSamples.BorderStyle = DecentForms.BorderStyle.SUNKEN;
      this.gridSamples.CustomMouseHandling = true;
      this.gridSamples.Dock = System.Windows.Forms.DockStyle.Fill;
      this.gridSamples.ItemHeight = 200;
      this.gridSamples.ItemWidth = 320;
      this.gridSamples.Location = new System.Drawing.Point(0, 25);
      this.gridSamples.Name = "gridSamples";
      this.gridSamples.ScrollAlwaysVisible = false;
      this.gridSamples.SelectedIndex = -1;
      this.gridSamples.SelectedItem = null;
      this.gridSamples.SelectionMode = DecentForms.SelectionMode.NONE;
      this.gridSamples.Size = new System.Drawing.Size(896, 679);
      this.gridSamples.TabIndex = 2;
      this.gridSamples.Text = "gridList1";
      this.gridSamples.DrawItem += new DecentForms.GridList.DrawGridListItemEventHandler(this.gridSamples_DrawItem);
      this.gridSamples.CustomEventHandler += new DecentForms.GridList.ForwardedEventHandler(this.gridSamples_CustomEventHandler);
      // 
      // SampleExplorer
      // 
      this.ClientSize = new System.Drawing.Size(896, 704);
      this.Controls.Add(this.gridSamples);
      this.Controls.Add(this.toolStripNavigation);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "SampleExplorer";
      this.Text = "Sample Explorer";
      this.Load += new System.EventHandler(this.SampleExplorer_Load);
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).EndInit();
      this.toolStripNavigation.ResumeLayout(false);
      this.toolStripNavigation.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ToolStrip toolStripNavigation;
    private System.Windows.Forms.ToolStripLabel toolStripLabel1;
    private System.Windows.Forms.ToolStripTextBox editSampleFilter;
    private DecentForms.GridList gridSamples;
  }
}
