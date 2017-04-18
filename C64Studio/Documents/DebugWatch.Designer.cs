namespace C64Studio
{
  partial class DebugWatch
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( DebugWatch ) );
      this.listWatch = new System.Windows.Forms.ListView();
      this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
      this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
      this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
      this.contextDebugItem = new System.Windows.Forms.ContextMenuStrip( this.components );
      this.displayAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.hexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.decimalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.binaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.petSCIIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.displayBoundsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.bytes1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.bytes2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.bytes8ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.bytes16ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.bytes32ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.watchReadFromMemoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.removeEntryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.bytes4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      ( (System.ComponentModel.ISupportInitialize)( this.m_FileWatcher ) ).BeginInit();
      this.contextDebugItem.SuspendLayout();
      this.SuspendLayout();
      // 
      // listWatch
      // 
      this.listWatch.Columns.AddRange( new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3} );
      this.listWatch.ContextMenuStrip = this.contextDebugItem;
      this.listWatch.Dock = System.Windows.Forms.DockStyle.Fill;
      this.listWatch.FullRowSelect = true;
      this.listWatch.Location = new System.Drawing.Point( 0, 0 );
      this.listWatch.Name = "listWatch";
      this.listWatch.Size = new System.Drawing.Size( 608, 195 );
      this.listWatch.TabIndex = 0;
      this.listWatch.UseCompatibleStateImageBehavior = false;
      this.listWatch.View = System.Windows.Forms.View.Details;
      this.listWatch.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler( this.listWatch_ColumnClick );
      this.listWatch.KeyDown += new System.Windows.Forms.KeyEventHandler( this.listWatch_KeyDown );
      // 
      // columnHeader1
      // 
      this.columnHeader1.Text = "Name";
      this.columnHeader1.Width = 140;
      // 
      // columnHeader2
      // 
      this.columnHeader2.Text = "Type";
      this.columnHeader2.Width = 110;
      // 
      // columnHeader3
      // 
      this.columnHeader3.Text = "Content";
      this.columnHeader3.Width = 300;
      // 
      // contextDebugItem
      // 
      this.contextDebugItem.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.displayAsToolStripMenuItem,
            this.displayBoundsToolStripMenuItem,
            this.watchReadFromMemoryToolStripMenuItem,
            this.toolStripSeparator1,
            this.removeEntryToolStripMenuItem} );
      this.contextDebugItem.Name = "contextDebugItem";
      this.contextDebugItem.Size = new System.Drawing.Size( 178, 120 );
      this.contextDebugItem.Opening += new System.ComponentModel.CancelEventHandler( this.contextDebugItem_Opening );
      // 
      // displayAsToolStripMenuItem
      // 
      this.displayAsToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.hexToolStripMenuItem,
            this.decimalToolStripMenuItem,
            this.binaryToolStripMenuItem,
            this.petSCIIToolStripMenuItem} );
      this.displayAsToolStripMenuItem.Name = "displayAsToolStripMenuItem";
      this.displayAsToolStripMenuItem.Size = new System.Drawing.Size( 177, 22 );
      this.displayAsToolStripMenuItem.Text = "Display as";
      // 
      // hexToolStripMenuItem
      // 
      this.hexToolStripMenuItem.Name = "hexToolStripMenuItem";
      this.hexToolStripMenuItem.Size = new System.Drawing.Size( 117, 22 );
      this.hexToolStripMenuItem.Text = "Hex";
      this.hexToolStripMenuItem.Click += new System.EventHandler( this.hexToolStripMenuItem_Click );
      // 
      // decimalToolStripMenuItem
      // 
      this.decimalToolStripMenuItem.Name = "decimalToolStripMenuItem";
      this.decimalToolStripMenuItem.Size = new System.Drawing.Size( 117, 22 );
      this.decimalToolStripMenuItem.Text = "Decimal";
      this.decimalToolStripMenuItem.Click += new System.EventHandler( this.decimalToolStripMenuItem_Click );
      // 
      // binaryToolStripMenuItem
      // 
      this.binaryToolStripMenuItem.Name = "binaryToolStripMenuItem";
      this.binaryToolStripMenuItem.Size = new System.Drawing.Size( 117, 22 );
      this.binaryToolStripMenuItem.Text = "Binary";
      this.binaryToolStripMenuItem.Click += new System.EventHandler( this.binaryToolStripMenuItem_Click );
      // 
      // petSCIIToolStripMenuItem
      // 
      this.petSCIIToolStripMenuItem.Name = "petSCIIToolStripMenuItem";
      this.petSCIIToolStripMenuItem.Size = new System.Drawing.Size( 117, 22 );
      this.petSCIIToolStripMenuItem.Text = "PetSCII";
      // 
      // displayBoundsToolStripMenuItem
      // 
      this.displayBoundsToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.bytes1ToolStripMenuItem,
            this.bytes2ToolStripMenuItem,
            this.bytes4ToolStripMenuItem,
            this.bytes8ToolStripMenuItem,
            this.bytes16ToolStripMenuItem,
            this.bytes32ToolStripMenuItem} );
      this.displayBoundsToolStripMenuItem.Name = "displayBoundsToolStripMenuItem";
      this.displayBoundsToolStripMenuItem.Size = new System.Drawing.Size( 177, 22 );
      this.displayBoundsToolStripMenuItem.Text = "Display Bounds";
      this.displayBoundsToolStripMenuItem.Visible = false;
      // 
      // bytes1ToolStripMenuItem
      // 
      this.bytes1ToolStripMenuItem.Name = "bytes1ToolStripMenuItem";
      this.bytes1ToolStripMenuItem.Size = new System.Drawing.Size( 152, 22 );
      this.bytes1ToolStripMenuItem.Text = "1 byte";
      this.bytes1ToolStripMenuItem.Click += new System.EventHandler( this.bytes1ToolStripMenuItem_Click );
      // 
      // bytes2ToolStripMenuItem
      // 
      this.bytes2ToolStripMenuItem.Name = "bytes2ToolStripMenuItem";
      this.bytes2ToolStripMenuItem.Size = new System.Drawing.Size( 152, 22 );
      this.bytes2ToolStripMenuItem.Text = "2 bytes";
      this.bytes2ToolStripMenuItem.Click += new System.EventHandler( this.bytes2ToolStripMenuItem_Click );
      // 
      // bytes8ToolStripMenuItem
      // 
      this.bytes8ToolStripMenuItem.Name = "bytes8ToolStripMenuItem";
      this.bytes8ToolStripMenuItem.Size = new System.Drawing.Size( 152, 22 );
      this.bytes8ToolStripMenuItem.Text = "8 bytes";
      this.bytes8ToolStripMenuItem.Click += new System.EventHandler( this.bytes8ToolStripMenuItem_Click );
      // 
      // bytes16ToolStripMenuItem
      // 
      this.bytes16ToolStripMenuItem.Name = "bytes16ToolStripMenuItem";
      this.bytes16ToolStripMenuItem.Size = new System.Drawing.Size( 152, 22 );
      this.bytes16ToolStripMenuItem.Text = "16 bytes";
      this.bytes16ToolStripMenuItem.Click += new System.EventHandler( this.bytes16ToolStripMenuItem_Click );
      // 
      // bytes32ToolStripMenuItem
      // 
      this.bytes32ToolStripMenuItem.Name = "bytes32ToolStripMenuItem";
      this.bytes32ToolStripMenuItem.Size = new System.Drawing.Size( 152, 22 );
      this.bytes32ToolStripMenuItem.Text = "32 bytes";
      this.bytes32ToolStripMenuItem.Click += new System.EventHandler( this.bytes32ToolStripMenuItem_Click );
      // 
      // watchReadFromMemoryToolStripMenuItem
      // 
      this.watchReadFromMemoryToolStripMenuItem.Name = "watchReadFromMemoryToolStripMenuItem";
      this.watchReadFromMemoryToolStripMenuItem.Size = new System.Drawing.Size( 177, 22 );
      this.watchReadFromMemoryToolStripMenuItem.Text = "Read from memory";
      this.watchReadFromMemoryToolStripMenuItem.Click += new System.EventHandler( this.watchReadFromMemoryToolStripMenuItem_Click );
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size( 174, 6 );
      // 
      // removeEntryToolStripMenuItem
      // 
      this.removeEntryToolStripMenuItem.Name = "removeEntryToolStripMenuItem";
      this.removeEntryToolStripMenuItem.Size = new System.Drawing.Size( 177, 22 );
      this.removeEntryToolStripMenuItem.Text = "&Remove entry";
      this.removeEntryToolStripMenuItem.Click += new System.EventHandler( this.removeEntryToolStripMenuItem_Click );
      // 
      // bytes4ToolStripMenuItem
      // 
      this.bytes4ToolStripMenuItem.Name = "bytes4ToolStripMenuItem";
      this.bytes4ToolStripMenuItem.Size = new System.Drawing.Size( 152, 22 );
      this.bytes4ToolStripMenuItem.Text = "4 bytes";
      this.bytes4ToolStripMenuItem.Click += new System.EventHandler( this.bytes4ToolStripMenuItem_Click );
      // 
      // DebugWatch
      // 
      this.ClientSize = new System.Drawing.Size( 608, 195 );
      this.Controls.Add( this.listWatch );
      this.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
      this.Icon = ( (System.Drawing.Icon)( resources.GetObject( "$this.Icon" ) ) );
      this.Name = "DebugWatch";
      this.Text = "Watch";
      ( (System.ComponentModel.ISupportInitialize)( this.m_FileWatcher ) ).EndInit();
      this.contextDebugItem.ResumeLayout( false );
      this.ResumeLayout( false );

    }

    #endregion

    private System.Windows.Forms.ListView listWatch;
    private System.Windows.Forms.ColumnHeader columnHeader1;
    private System.Windows.Forms.ColumnHeader columnHeader2;
    private System.Windows.Forms.ColumnHeader columnHeader3;
    private System.Windows.Forms.ContextMenuStrip contextDebugItem;
    private System.Windows.Forms.ToolStripMenuItem displayAsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem hexToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem decimalToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem petSCIIToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem binaryToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem watchReadFromMemoryToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem removeEntryToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem displayBoundsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem bytes1ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem bytes2ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem bytes16ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem bytes32ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem bytes8ToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem bytes4ToolStripMenuItem;



  }
}
