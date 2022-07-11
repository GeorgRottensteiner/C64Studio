namespace RetroDevStudio.Documents
{
  partial class DebugStack
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( DebugStack ) );
      this.listCallStack = new System.Windows.Forms.ListView();
      this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
      this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
      this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
      ( (System.ComponentModel.ISupportInitialize)( this.m_FileWatcher ) ).BeginInit();
      this.SuspendLayout();
      // 
      // listCallStack
      // 
      this.listCallStack.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader3,
            this.columnHeader2} );
      this.listCallStack.Dock = System.Windows.Forms.DockStyle.Fill;
      this.listCallStack.FullRowSelect = true;
      this.listCallStack.Location = new System.Drawing.Point( 0, 0 );
      this.listCallStack.Name = "listCallStack";
      this.listCallStack.Size = new System.Drawing.Size( 534, 390 );
      this.listCallStack.TabIndex = 0;
      this.listCallStack.UseCompatibleStateImageBehavior = false;
      this.listCallStack.View = System.Windows.Forms.View.Details;
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "Address";
      this.columnHeader1.Width = 74;
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "File";
      this.columnHeader2.Width = 317;
      // 
      // columnHeader3
      // 
      this.columnHeader3.Text = "Line";
      this.columnHeader3.Width = 63;
      // 
      // DebugStack
      // 
      this.ClientSize = new System.Drawing.Size( 534, 390 );
      this.Controls.Add( this.listCallStack );
      this.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
      this.Icon = ( (System.Drawing.Icon)( resources.GetObject( "$this.Icon" ) ) );
      this.Name = "DebugStack";
      this.Text = "Call Stack";
      ( (System.ComponentModel.ISupportInitialize)( this.m_FileWatcher ) ).EndInit();
      this.ResumeLayout( false );

    }

    #endregion

    private System.Windows.Forms.ListView listCallStack;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader3;
    private System.Windows.Forms.ColumnHeader columnHeader2;



  }
}
