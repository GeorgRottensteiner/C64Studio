namespace RetroDevStudio.Documents
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
      components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DebugWatch));
      listWatch = new DecentForms.ListControl();
      contextDebugItem = new System.Windows.Forms.ContextMenuStrip( components );
      displayAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      hexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      decimalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      binaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      petSCIIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      displayBoundsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      bytes1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      bytes2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      bytes4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      bytes8ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      bytes16ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      bytes32ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      watchReadFromMemoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      toggleEndiannessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      pinToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      moveToTopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      moveUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      moveDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      moveToBottomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
      removeEntryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      removeAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
      copyToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      copySelectedValuesToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      ( (System.ComponentModel.ISupportInitialize)m_FileWatcher ).BeginInit();
      contextDebugItem.SuspendLayout();
      SuspendLayout();
      // 
      // listWatch
      // 
      listWatch.BorderStyle = DecentForms.BorderStyle.SUNKEN;
      listWatch.CheckBoxes = false;
      listWatch.ContextMenuStrip = contextDebugItem;
      listWatch.DisplayAntiAliased = true;
      listWatch.Dock = System.Windows.Forms.DockStyle.Fill;
      listWatch.HasHeader = true;
      listWatch.HeaderHeight = 24;
      listWatch.ImageList = null;
      listWatch.ItemHeight = 15;
      listWatch.Location = new System.Drawing.Point( 0, 0 );
      listWatch.MultiSelected = false;
      listWatch.Name = "listWatch";
      listWatch.ScrollAlwaysVisible = false;
      listWatch.SelectionMode = DecentForms.SelectionMode.ONE;
      listWatch.Size = new System.Drawing.Size( 608, 195 );
      listWatch.SortColumn = 0;
      listWatch.SortOrder = DecentForms.SortOrder.NONE;
      listWatch.TabIndex = 0;
      listWatch.ColumnClicked +=  listWatch_ColumnClicked ;
      listWatch.DrawSubItem +=  listWatch_DrawSubItem ;
      listWatch.KeyDown +=  listWatch_KeyDown ;
      // 
      // contextDebugItem
      // 
      contextDebugItem.Items.AddRange( new System.Windows.Forms.ToolStripItem[] { displayAsToolStripMenuItem, displayBoundsToolStripMenuItem, watchReadFromMemoryToolStripMenuItem, toggleEndiannessToolStripMenuItem, toolStripSeparator1, pinToolStripMenuItem, moveToTopToolStripMenuItem, moveUpToolStripMenuItem, moveDownToolStripMenuItem, moveToBottomToolStripMenuItem, toolStripSeparator2, removeEntryToolStripMenuItem, removeAllToolStripMenuItem, toolStripSeparator3, copyToClipboardToolStripMenuItem, copySelectedValuesToClipboardToolStripMenuItem } );
      contextDebugItem.Name = "contextDebugItem";
      contextDebugItem.Size = new System.Drawing.Size( 264, 308 );
      contextDebugItem.Opening +=  contextDebugItem_Opening ;
      // 
      // displayAsToolStripMenuItem
      // 
      displayAsToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] { hexToolStripMenuItem, decimalToolStripMenuItem, binaryToolStripMenuItem, petSCIIToolStripMenuItem } );
      displayAsToolStripMenuItem.Name = "displayAsToolStripMenuItem";
      displayAsToolStripMenuItem.Size = new System.Drawing.Size( 263, 22 );
      displayAsToolStripMenuItem.Text = "Display as";
      // 
      // hexToolStripMenuItem
      // 
      hexToolStripMenuItem.Name = "hexToolStripMenuItem";
      hexToolStripMenuItem.Size = new System.Drawing.Size( 117, 22 );
      hexToolStripMenuItem.Text = "Hex";
      hexToolStripMenuItem.Click +=  hexToolStripMenuItem_Click ;
      // 
      // decimalToolStripMenuItem
      // 
      decimalToolStripMenuItem.Name = "decimalToolStripMenuItem";
      decimalToolStripMenuItem.Size = new System.Drawing.Size( 117, 22 );
      decimalToolStripMenuItem.Text = "Decimal";
      decimalToolStripMenuItem.Click +=  decimalToolStripMenuItem_Click ;
      // 
      // binaryToolStripMenuItem
      // 
      binaryToolStripMenuItem.Name = "binaryToolStripMenuItem";
      binaryToolStripMenuItem.Size = new System.Drawing.Size( 117, 22 );
      binaryToolStripMenuItem.Text = "Binary";
      binaryToolStripMenuItem.Click +=  binaryToolStripMenuItem_Click ;
      // 
      // petSCIIToolStripMenuItem
      // 
      petSCIIToolStripMenuItem.Name = "petSCIIToolStripMenuItem";
      petSCIIToolStripMenuItem.Size = new System.Drawing.Size( 117, 22 );
      petSCIIToolStripMenuItem.Text = "PetSCII";
      // 
      // displayBoundsToolStripMenuItem
      // 
      displayBoundsToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] { bytes1ToolStripMenuItem, bytes2ToolStripMenuItem, bytes4ToolStripMenuItem, bytes8ToolStripMenuItem, bytes16ToolStripMenuItem, bytes32ToolStripMenuItem } );
      displayBoundsToolStripMenuItem.Name = "displayBoundsToolStripMenuItem";
      displayBoundsToolStripMenuItem.Size = new System.Drawing.Size( 263, 22 );
      displayBoundsToolStripMenuItem.Text = "Display Bounds";
      displayBoundsToolStripMenuItem.Visible = false;
      // 
      // bytes1ToolStripMenuItem
      // 
      bytes1ToolStripMenuItem.Name = "bytes1ToolStripMenuItem";
      bytes1ToolStripMenuItem.Size = new System.Drawing.Size( 117, 22 );
      bytes1ToolStripMenuItem.Text = "1 byte";
      bytes1ToolStripMenuItem.Click +=  bytes1ToolStripMenuItem_Click ;
      // 
      // bytes2ToolStripMenuItem
      // 
      bytes2ToolStripMenuItem.Name = "bytes2ToolStripMenuItem";
      bytes2ToolStripMenuItem.Size = new System.Drawing.Size( 117, 22 );
      bytes2ToolStripMenuItem.Text = "2 bytes";
      bytes2ToolStripMenuItem.Click +=  bytes2ToolStripMenuItem_Click ;
      // 
      // bytes4ToolStripMenuItem
      // 
      bytes4ToolStripMenuItem.Name = "bytes4ToolStripMenuItem";
      bytes4ToolStripMenuItem.Size = new System.Drawing.Size( 117, 22 );
      bytes4ToolStripMenuItem.Text = "4 bytes";
      bytes4ToolStripMenuItem.Click +=  bytes4ToolStripMenuItem_Click ;
      // 
      // bytes8ToolStripMenuItem
      // 
      bytes8ToolStripMenuItem.Name = "bytes8ToolStripMenuItem";
      bytes8ToolStripMenuItem.Size = new System.Drawing.Size( 117, 22 );
      bytes8ToolStripMenuItem.Text = "8 bytes";
      bytes8ToolStripMenuItem.Click +=  bytes8ToolStripMenuItem_Click ;
      // 
      // bytes16ToolStripMenuItem
      // 
      bytes16ToolStripMenuItem.Name = "bytes16ToolStripMenuItem";
      bytes16ToolStripMenuItem.Size = new System.Drawing.Size( 117, 22 );
      bytes16ToolStripMenuItem.Text = "16 bytes";
      bytes16ToolStripMenuItem.Click +=  bytes16ToolStripMenuItem_Click ;
      // 
      // bytes32ToolStripMenuItem
      // 
      bytes32ToolStripMenuItem.Name = "bytes32ToolStripMenuItem";
      bytes32ToolStripMenuItem.Size = new System.Drawing.Size( 117, 22 );
      bytes32ToolStripMenuItem.Text = "32 bytes";
      bytes32ToolStripMenuItem.Click +=  bytes32ToolStripMenuItem_Click ;
      // 
      // watchReadFromMemoryToolStripMenuItem
      // 
      watchReadFromMemoryToolStripMenuItem.Name = "watchReadFromMemoryToolStripMenuItem";
      watchReadFromMemoryToolStripMenuItem.Size = new System.Drawing.Size( 263, 22 );
      watchReadFromMemoryToolStripMenuItem.Text = "Read from memory";
      watchReadFromMemoryToolStripMenuItem.Click +=  watchReadFromMemoryToolStripMenuItem_Click ;
      // 
      // toggleEndiannessToolStripMenuItem
      // 
      toggleEndiannessToolStripMenuItem.Name = "toggleEndiannessToolStripMenuItem";
      toggleEndiannessToolStripMenuItem.Size = new System.Drawing.Size( 263, 22 );
      toggleEndiannessToolStripMenuItem.Text = "Little Endian";
      toggleEndiannessToolStripMenuItem.Click +=  toggleEndiannessToolStripMenuItem_Click ;
      // 
      // toolStripSeparator1
      // 
      toolStripSeparator1.Name = "toolStripSeparator1";
      toolStripSeparator1.Size = new System.Drawing.Size( 260, 6 );
      // 
      // pinToolStripMenuItem
      // 
      pinToolStripMenuItem.Name = "pinToolStripMenuItem";
      pinToolStripMenuItem.Size = new System.Drawing.Size( 263, 22 );
      pinToolStripMenuItem.Text = "Pin";
      pinToolStripMenuItem.Click +=  pinToolStripMenuItem_Click ;
      // 
      // moveToTopToolStripMenuItem
      // 
      moveToTopToolStripMenuItem.Name = "moveToTopToolStripMenuItem";
      moveToTopToolStripMenuItem.Size = new System.Drawing.Size( 263, 22 );
      moveToTopToolStripMenuItem.Text = "Move to Top";
      moveToTopToolStripMenuItem.Click +=  moveToTopToolStripMenuItem_Click ;
      // 
      // moveUpToolStripMenuItem
      // 
      moveUpToolStripMenuItem.Name = "moveUpToolStripMenuItem";
      moveUpToolStripMenuItem.Size = new System.Drawing.Size( 263, 22 );
      moveUpToolStripMenuItem.Text = "Move Up";
      moveUpToolStripMenuItem.Click +=  moveUpToolStripMenuItem_Click ;
      // 
      // moveDownToolStripMenuItem
      // 
      moveDownToolStripMenuItem.Name = "moveDownToolStripMenuItem";
      moveDownToolStripMenuItem.Size = new System.Drawing.Size( 263, 22 );
      moveDownToolStripMenuItem.Text = "Move Down";
      moveDownToolStripMenuItem.Click +=  moveDownToolStripMenuItem_Click ;
      // 
      // moveToBottomToolStripMenuItem
      // 
      moveToBottomToolStripMenuItem.Name = "moveToBottomToolStripMenuItem";
      moveToBottomToolStripMenuItem.Size = new System.Drawing.Size( 263, 22 );
      moveToBottomToolStripMenuItem.Text = "Move to Bottom";
      moveToBottomToolStripMenuItem.Click +=  moveToBottomToolStripMenuItem_Click ;
      // 
      // toolStripSeparator2
      // 
      toolStripSeparator2.Name = "toolStripSeparator2";
      toolStripSeparator2.Size = new System.Drawing.Size( 260, 6 );
      // 
      // removeEntryToolStripMenuItem
      // 
      removeEntryToolStripMenuItem.Name = "removeEntryToolStripMenuItem";
      removeEntryToolStripMenuItem.Size = new System.Drawing.Size( 263, 22 );
      removeEntryToolStripMenuItem.Text = "&Remove entry";
      removeEntryToolStripMenuItem.Click +=  removeEntryToolStripMenuItem_Click ;
      // 
      // removeAllToolStripMenuItem
      // 
      removeAllToolStripMenuItem.Name = "removeAllToolStripMenuItem";
      removeAllToolStripMenuItem.Size = new System.Drawing.Size( 263, 22 );
      removeAllToolStripMenuItem.Text = "Remove all";
      removeAllToolStripMenuItem.Click +=  removeAllToolStripMenuItem_Click ;
      // 
      // toolStripSeparator3
      // 
      toolStripSeparator3.Name = "toolStripSeparator3";
      toolStripSeparator3.Size = new System.Drawing.Size( 260, 6 );
      // 
      // copyToClipboardToolStripMenuItem
      // 
      copyToClipboardToolStripMenuItem.Name = "copyToClipboardToolStripMenuItem";
      copyToClipboardToolStripMenuItem.Size = new System.Drawing.Size( 263, 22 );
      copyToClipboardToolStripMenuItem.Text = "Copy selected watches to Clipboard";
      copyToClipboardToolStripMenuItem.Click +=  copyToClipboardToolStripMenuItem_Click ;
      // 
      // copySelectedValuesToClipboardToolStripMenuItem
      // 
      copySelectedValuesToClipboardToolStripMenuItem.Name = "copySelectedValuesToClipboardToolStripMenuItem";
      copySelectedValuesToClipboardToolStripMenuItem.Size = new System.Drawing.Size( 263, 22 );
      copySelectedValuesToClipboardToolStripMenuItem.Text = "Copy selected values to Clipboard";
      copySelectedValuesToClipboardToolStripMenuItem.Click +=  copySelectedValuesToClipboardToolStripMenuItem_Click ;
      // 
      // DebugWatch
      // 
      ClientSize = new System.Drawing.Size( 608, 195 );
      Controls.Add( listWatch );
      Icon = (System.Drawing.Icon)resources.GetObject( "$this.Icon" );
      Name = "DebugWatch";
      Text = "Watch";
      ( (System.ComponentModel.ISupportInitialize)m_FileWatcher ).EndInit();
      contextDebugItem.ResumeLayout( false );
      ResumeLayout( false );

    }

    #endregion

    private DecentForms.ListControl listWatch;
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
    private System.Windows.Forms.ToolStripMenuItem moveUpToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem moveDownToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripMenuItem toggleEndiannessToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem removeAllToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    private System.Windows.Forms.ToolStripMenuItem copyToClipboardToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem copySelectedValuesToClipboardToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem pinToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem moveToTopToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem moveToBottomToolStripMenuItem;
  }
}
