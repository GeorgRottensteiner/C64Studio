namespace RetroDevStudio.Documents
{
  partial class FindReferences
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FindReferences));
      this.listResults = new DecentForms.ListControl();
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).BeginInit();
      this.SuspendLayout();
      // 
      // listResults
      // 
      this.listResults.BorderStyle = DecentForms.BorderStyle.SUNKEN;
      this.listResults.Dock = System.Windows.Forms.DockStyle.Fill;
      this.listResults.FirstVisibleItemIndex = 0;
      this.listResults.HasHeader = true;
      this.listResults.HeaderHeight = 24;
      this.listResults.ItemHeight = 15;
      this.listResults.Location = new System.Drawing.Point(0, 0);
      this.listResults.Name = "listResults";
      this.listResults.ScrollAlwaysVisible = false;
      this.listResults.SelectedIndex = -1;
      this.listResults.SelectedItem = null;
      this.listResults.SelectionMode = DecentForms.SelectionMode.NONE;
      this.listResults.Size = new System.Drawing.Size(678, 200);
      this.listResults.SortColumn = -1;
      this.listResults.SortOrder = DecentForms.SortOrder.NONE;
      this.listResults.TabIndex = 0;
      this.listResults.ItemActivate += new DecentForms.EventHandler(this.listResults_ItemActivate);
      // 
      // FindReferences
      // 
      this.ClientSize = new System.Drawing.Size(678, 200);
      this.Controls.Add(this.listResults);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "FindReferences";
      this.Text = "Find References";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FindReferences_FormClosing);
      this.Load += new System.EventHandler(this.FindReferences_Load);
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private DecentForms.ListControl listResults;
  }
}
