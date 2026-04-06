namespace RetroDevStudio.Documents
{
  partial class SearchResults
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchResults));
      this.listResults = new DecentForms.ListControl();
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).BeginInit();
      this.SuspendLayout();
      // 
      // listResults
      // 
      this.listResults.Dock = System.Windows.Forms.DockStyle.Fill;
      this.listResults.Location = new System.Drawing.Point(0, 0);
      this.listResults.Name = "listResults";
      this.listResults.Size = new System.Drawing.Size(678, 200);
      this.listResults.TabIndex = 0;
      this.listResults.ColumnClicked += listMessages_ColumnClick;
      this.listResults.ItemActivate += listResults_ItemActivate;
      
      // 
      // SearchResults
      // 
      this.ClientSize = new System.Drawing.Size(678, 200);
      this.Controls.Add(this.listResults);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "SearchResults";
      this.Text = "Search Results";
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private DecentForms.ListControl   listResults;
  }
}
