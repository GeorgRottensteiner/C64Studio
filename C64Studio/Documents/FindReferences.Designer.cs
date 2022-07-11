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
      this.listResults = new System.Windows.Forms.ListView();
      this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).BeginInit();
      this.SuspendLayout();
      // 
      // listResults
      // 
      this.listResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
      this.listResults.Dock = System.Windows.Forms.DockStyle.Fill;
      this.listResults.FullRowSelect = true;
      this.listResults.HideSelection = false;
      this.listResults.Location = new System.Drawing.Point(0, 0);
      this.listResults.Name = "listResults";
      this.listResults.Size = new System.Drawing.Size(678, 200);
      this.listResults.TabIndex = 0;
      this.listResults.UseCompatibleStateImageBehavior = false;
      this.listResults.View = System.Windows.Forms.View.Details;
      this.listResults.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listMessages_ColumnClick);
      this.listResults.ItemActivate += new System.EventHandler(this.listResults_ItemActivate);
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "Line";
      this.columnHeader2.Width = 50;
      // 
      // columnHeader3
      // 
      this.columnHeader3.Text = "File";
      this.columnHeader3.Width = 200;
      // 
      // columnHeader4
      // 
      this.columnHeader4.Text = "Text";
      this.columnHeader4.Width = 400;
      // 
      // FindReferences
      // 
      this.ClientSize = new System.Drawing.Size(678, 200);
      this.Controls.Add(this.listResults);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "FindReferences";
      this.Text = "Find References";
      ((System.ComponentModel.ISupportInitialize)(this.m_FileWatcher)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListView listResults;
    private System.Windows.Forms.ColumnHeader columnHeader2;
    private System.Windows.Forms.ColumnHeader columnHeader3;
    private System.Windows.Forms.ColumnHeader columnHeader4;



  }
}
