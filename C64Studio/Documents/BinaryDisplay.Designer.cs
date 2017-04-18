namespace C64Studio
{
  partial class BinaryDisplay
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( BinaryDisplay ) );
      this.tabMain = new System.Windows.Forms.TabControl();
      this.tabData = new System.Windows.Forms.TabPage();
      this.hexView = new Be.Windows.Forms.HexBox();
      this.tabModify = new System.Windows.Forms.TabPage();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.btnExport = new System.Windows.Forms.Button();
      this.btnImport = new System.Windows.Forms.Button();
      this.btnInterleave = new System.Windows.Forms.Button();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.button2 = new System.Windows.Forms.Button();
      this.btnFromBASIC = new System.Windows.Forms.Button();
      this.btnFromASM = new System.Windows.Forms.Button();
      this.button1 = new System.Windows.Forms.Button();
      this.btnToBASIC = new System.Windows.Forms.Button();
      this.btnToText = new System.Windows.Forms.Button();
      this.textBinaryData = new System.Windows.Forms.TextBox();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.dataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.importFromFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exportToFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
      this.modifyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.interleaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      ( (System.ComponentModel.ISupportInitialize)( this.m_FileWatcher ) ).BeginInit();
      this.tabMain.SuspendLayout();
      this.tabData.SuspendLayout();
      this.tabModify.SuspendLayout();
      this.groupBox2.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.menuStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // tabMain
      // 
      this.tabMain.Controls.Add( this.tabData );
      this.tabMain.Controls.Add( this.tabModify );
      this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabMain.Location = new System.Drawing.Point( 0, 24 );
      this.tabMain.Name = "tabMain";
      this.tabMain.SelectedIndex = 0;
      this.tabMain.Size = new System.Drawing.Size( 669, 366 );
      this.tabMain.TabIndex = 0;
      // 
      // tabData
      // 
      this.tabData.Controls.Add( this.hexView );
      this.tabData.Location = new System.Drawing.Point( 4, 22 );
      this.tabData.Name = "tabData";
      this.tabData.Padding = new System.Windows.Forms.Padding( 3 );
      this.tabData.Size = new System.Drawing.Size( 661, 340 );
      this.tabData.TabIndex = 0;
      this.tabData.Text = "Data";
      this.tabData.UseVisualStyleBackColor = true;
      // 
      // hexView
      // 
      this.hexView.ColumnInfoVisible = true;
      this.hexView.Dock = System.Windows.Forms.DockStyle.Fill;
      this.hexView.Font = new System.Drawing.Font( "Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
      this.hexView.InfoForeColor = System.Drawing.SystemColors.AppWorkspace;
      this.hexView.LineInfoVisible = true;
      this.hexView.Location = new System.Drawing.Point( 3, 3 );
      this.hexView.Name = "hexView";
      this.hexView.SelectedByteProvider = null;
      this.hexView.ShadowSelectionColor = System.Drawing.Color.FromArgb( ( (int)( ( (byte)( 100 ) ) ) ), ( (int)( ( (byte)( 60 ) ) ) ), ( (int)( ( (byte)( 188 ) ) ) ), ( (int)( ( (byte)( 255 ) ) ) ) );
      this.hexView.Size = new System.Drawing.Size( 655, 334 );
      this.hexView.StringViewVisible = true;
      this.hexView.TabIndex = 0;
      this.hexView.VScrollBarVisible = true;
      // 
      // tabModify
      // 
      this.tabModify.Controls.Add( this.groupBox2 );
      this.tabModify.Controls.Add( this.groupBox1 );
      this.tabModify.Location = new System.Drawing.Point( 4, 22 );
      this.tabModify.Name = "tabModify";
      this.tabModify.Padding = new System.Windows.Forms.Padding( 3 );
      this.tabModify.Size = new System.Drawing.Size( 661, 340 );
      this.tabModify.TabIndex = 1;
      this.tabModify.Text = "Modify";
      this.tabModify.UseVisualStyleBackColor = true;
      // 
      // groupBox2
      // 
      this.groupBox2.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                  | System.Windows.Forms.AnchorStyles.Right ) ) );
      this.groupBox2.Controls.Add( this.btnExport );
      this.groupBox2.Controls.Add( this.btnImport );
      this.groupBox2.Controls.Add( this.btnInterleave );
      this.groupBox2.Location = new System.Drawing.Point( 428, 6 );
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size( 225, 250 );
      this.groupBox2.TabIndex = 1;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Actions";
      // 
      // btnExport
      // 
      this.btnExport.Location = new System.Drawing.Point( 87, 19 );
      this.btnExport.Name = "btnExport";
      this.btnExport.Size = new System.Drawing.Size( 75, 23 );
      this.btnExport.TabIndex = 1;
      this.btnExport.Text = "Export";
      this.btnExport.UseVisualStyleBackColor = true;
      this.btnExport.Click += new System.EventHandler( this.btnExport_Click );
      // 
      // btnImport
      // 
      this.btnImport.Location = new System.Drawing.Point( 6, 19 );
      this.btnImport.Name = "btnImport";
      this.btnImport.Size = new System.Drawing.Size( 75, 23 );
      this.btnImport.TabIndex = 1;
      this.btnImport.Text = "Import";
      this.btnImport.UseVisualStyleBackColor = true;
      this.btnImport.Click += new System.EventHandler( this.btnImport_Click );
      // 
      // btnInterleave
      // 
      this.btnInterleave.Location = new System.Drawing.Point( 6, 48 );
      this.btnInterleave.Name = "btnInterleave";
      this.btnInterleave.Size = new System.Drawing.Size( 75, 23 );
      this.btnInterleave.TabIndex = 0;
      this.btnInterleave.Text = "Interleave...";
      this.btnInterleave.UseVisualStyleBackColor = true;
      this.btnInterleave.Click += new System.EventHandler( this.btnInterleave_Click );
      // 
      // groupBox1
      // 
      this.groupBox1.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                  | System.Windows.Forms.AnchorStyles.Left )
                  | System.Windows.Forms.AnchorStyles.Right ) ) );
      this.groupBox1.Controls.Add( this.button2 );
      this.groupBox1.Controls.Add( this.btnFromBASIC );
      this.groupBox1.Controls.Add( this.btnFromASM );
      this.groupBox1.Controls.Add( this.button1 );
      this.groupBox1.Controls.Add( this.btnToBASIC );
      this.groupBox1.Controls.Add( this.btnToText );
      this.groupBox1.Controls.Add( this.textBinaryData );
      this.groupBox1.Location = new System.Drawing.Point( 6, 6 );
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size( 416, 250 );
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Text Data";
      // 
      // button2
      // 
      this.button2.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
      this.button2.Location = new System.Drawing.Point( 330, 190 );
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size( 75, 23 );
      this.button2.TabIndex = 1;
      this.button2.Text = "From Hex";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new System.EventHandler( this.btnFromHex_Click );
      // 
      // btnFromBASIC
      // 
      this.btnFromBASIC.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
      this.btnFromBASIC.Location = new System.Drawing.Point( 330, 161 );
      this.btnFromBASIC.Name = "btnFromBASIC";
      this.btnFromBASIC.Size = new System.Drawing.Size( 75, 23 );
      this.btnFromBASIC.TabIndex = 1;
      this.btnFromBASIC.Text = "From BASIC";
      this.btnFromBASIC.UseVisualStyleBackColor = true;
      this.btnFromBASIC.Click += new System.EventHandler( this.btnFromBASIC_Click );
      // 
      // btnFromASM
      // 
      this.btnFromASM.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
      this.btnFromASM.Location = new System.Drawing.Point( 330, 132 );
      this.btnFromASM.Name = "btnFromASM";
      this.btnFromASM.Size = new System.Drawing.Size( 75, 23 );
      this.btnFromASM.TabIndex = 1;
      this.btnFromASM.Text = "From ASM";
      this.btnFromASM.UseVisualStyleBackColor = true;
      this.btnFromASM.Click += new System.EventHandler( this.btnFromASM_Click );
      // 
      // button1
      // 
      this.button1.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
      this.button1.Location = new System.Drawing.Point( 330, 77 );
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size( 75, 23 );
      this.button1.TabIndex = 1;
      this.button1.Text = "To Hex";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler( this.btnToHex_Click );
      // 
      // btnToBASIC
      // 
      this.btnToBASIC.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
      this.btnToBASIC.Location = new System.Drawing.Point( 330, 48 );
      this.btnToBASIC.Name = "btnToBASIC";
      this.btnToBASIC.Size = new System.Drawing.Size( 75, 23 );
      this.btnToBASIC.TabIndex = 1;
      this.btnToBASIC.Text = "To BASIC";
      this.btnToBASIC.UseVisualStyleBackColor = true;
      this.btnToBASIC.Click += new System.EventHandler( this.btnToBASIC_Click );
      // 
      // btnToText
      // 
      this.btnToText.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
      this.btnToText.Location = new System.Drawing.Point( 330, 19 );
      this.btnToText.Name = "btnToText";
      this.btnToText.Size = new System.Drawing.Size( 75, 23 );
      this.btnToText.TabIndex = 1;
      this.btnToText.Text = "To ASM";
      this.btnToText.UseVisualStyleBackColor = true;
      this.btnToText.Click += new System.EventHandler( this.btnToText_Click );
      // 
      // textBinaryData
      // 
      this.textBinaryData.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                  | System.Windows.Forms.AnchorStyles.Left )
                  | System.Windows.Forms.AnchorStyles.Right ) ) );
      this.textBinaryData.Location = new System.Drawing.Point( 6, 19 );
      this.textBinaryData.Multiline = true;
      this.textBinaryData.Name = "textBinaryData";
      this.textBinaryData.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.textBinaryData.Size = new System.Drawing.Size( 318, 225 );
      this.textBinaryData.TabIndex = 0;
      this.textBinaryData.WordWrap = false;
      // 
      // menuStrip1
      // 
      this.menuStrip1.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.dataToolStripMenuItem} );
      this.menuStrip1.Location = new System.Drawing.Point( 0, 0 );
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size( 669, 24 );
      this.menuStrip1.TabIndex = 1;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // dataToolStripMenuItem
      // 
      this.dataToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.importFromFileToolStripMenuItem,
            this.exportToFileToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.toolStripSeparator1,
            this.modifyToolStripMenuItem} );
      this.dataToolStripMenuItem.Name = "dataToolStripMenuItem";
      this.dataToolStripMenuItem.Size = new System.Drawing.Size( 52, 20 );
      this.dataToolStripMenuItem.Text = "Binary";
      // 
      // importFromFileToolStripMenuItem
      // 
      this.importFromFileToolStripMenuItem.Name = "importFromFileToolStripMenuItem";
      this.importFromFileToolStripMenuItem.Size = new System.Drawing.Size( 167, 22 );
      this.importFromFileToolStripMenuItem.Text = "&Import from file...";
      this.importFromFileToolStripMenuItem.Click += new System.EventHandler( this.importFromFileToolStripMenuItem_Click );
      // 
      // exportToFileToolStripMenuItem
      // 
      this.exportToFileToolStripMenuItem.Name = "exportToFileToolStripMenuItem";
      this.exportToFileToolStripMenuItem.Size = new System.Drawing.Size( 167, 22 );
      this.exportToFileToolStripMenuItem.Text = "Export to file...";
      this.exportToFileToolStripMenuItem.Click += new System.EventHandler( this.exportToFileToolStripMenuItem_Click );
      // 
      // closeToolStripMenuItem
      // 
      this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
      this.closeToolStripMenuItem.Size = new System.Drawing.Size( 167, 22 );
      this.closeToolStripMenuItem.Text = "&Close";
      this.closeToolStripMenuItem.Click += new System.EventHandler( this.closeToolStripMenuItem_Click );
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size( 164, 6 );
      // 
      // modifyToolStripMenuItem
      // 
      this.modifyToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.interleaveToolStripMenuItem} );
      this.modifyToolStripMenuItem.Name = "modifyToolStripMenuItem";
      this.modifyToolStripMenuItem.Size = new System.Drawing.Size( 167, 22 );
      this.modifyToolStripMenuItem.Text = "Modify";
      // 
      // interleaveToolStripMenuItem
      // 
      this.interleaveToolStripMenuItem.Name = "interleaveToolStripMenuItem";
      this.interleaveToolStripMenuItem.Size = new System.Drawing.Size( 134, 22 );
      this.interleaveToolStripMenuItem.Text = "Interleave...";
      this.interleaveToolStripMenuItem.Click += new System.EventHandler( this.interleaveToolStripMenuItem_Click );
      // 
      // BinaryDisplay
      // 
      this.ClientSize = new System.Drawing.Size( 669, 390 );
      this.Controls.Add( this.tabMain );
      this.Controls.Add( this.menuStrip1 );
      this.Icon = ( (System.Drawing.Icon)( resources.GetObject( "$this.Icon" ) ) );
      this.MainMenuStrip = this.menuStrip1;
      this.Name = "BinaryDisplay";
      this.Text = "Binary Editor";
      ( (System.ComponentModel.ISupportInitialize)( this.m_FileWatcher ) ).EndInit();
      this.tabMain.ResumeLayout( false );
      this.tabData.ResumeLayout( false );
      this.tabModify.ResumeLayout( false );
      this.groupBox2.ResumeLayout( false );
      this.groupBox1.ResumeLayout( false );
      this.groupBox1.PerformLayout();
      this.menuStrip1.ResumeLayout( false );
      this.menuStrip1.PerformLayout();
      this.ResumeLayout( false );
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TabControl tabMain;
    private System.Windows.Forms.TabPage tabData;
    private System.Windows.Forms.TabPage tabModify;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem dataToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exportToFileToolStripMenuItem;
    private Be.Windows.Forms.HexBox hexView;
    private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem importFromFileToolStripMenuItem;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.TextBox textBinaryData;
    private System.Windows.Forms.Button btnFromBASIC;
    private System.Windows.Forms.Button btnFromASM;
    private System.Windows.Forms.Button btnToBASIC;
    private System.Windows.Forms.Button btnToText;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem modifyToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem interleaveToolStripMenuItem;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.Button btnInterleave;
    private System.Windows.Forms.Button btnExport;
    private System.Windows.Forms.Button btnImport;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;



  }
}
