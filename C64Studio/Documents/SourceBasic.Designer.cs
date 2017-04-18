namespace C64Studio
{
  partial class SourceBasic
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
      this.editSource = new ScintillaNET.Scintilla();
      this.contextSource = new System.Windows.Forms.ContextMenuStrip( this.components );
      this.btnToggleLabelMode = new System.Windows.Forms.CheckBox();
      this.menuBASIC = new System.Windows.Forms.MenuStrip();
      this.bASICToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.renumberToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      ( (System.ComponentModel.ISupportInitialize)( this.m_FileWatcher ) ).BeginInit();
      ( (System.ComponentModel.ISupportInitialize)( this.editSource ) ).BeginInit();
      this.menuBASIC.SuspendLayout();
      this.SuspendLayout();
      // 
      // editSource
      // 
      this.editSource.AllowDrop = true;
      this.editSource.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( ( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom )
                  | System.Windows.Forms.AnchorStyles.Left )
                  | System.Windows.Forms.AnchorStyles.Right ) ) );
      this.editSource.ContextMenuStrip = this.contextSource;
      this.editSource.Font = new System.Drawing.Font( "Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
      this.editSource.Indentation.SmartIndentType = ScintillaNET.SmartIndent.Simple;
      this.editSource.Indentation.TabIndents = false;
      this.editSource.Indentation.TabWidth = 2;
      this.editSource.Indentation.UseTabs = false;
      this.editSource.Location = new System.Drawing.Point( 0, 56 );
      this.editSource.Name = "editSource";
      this.editSource.Size = new System.Drawing.Size( 698, 532 );
      this.editSource.TabIndex = 0;
      //this.editSource.UseFont = true;
      // 
      // contextSource
      // 
      this.contextSource.Name = "contextSource";
      this.contextSource.Size = new System.Drawing.Size( 61, 4 );
      // 
      // btnToggleLabelMode
      // 
      this.btnToggleLabelMode.Appearance = System.Windows.Forms.Appearance.Button;
      this.btnToggleLabelMode.AutoSize = true;
      this.btnToggleLabelMode.Location = new System.Drawing.Point( 0, 27 );
      this.btnToggleLabelMode.Name = "btnToggleLabelMode";
      this.btnToggleLabelMode.Size = new System.Drawing.Size( 73, 23 );
      this.btnToggleLabelMode.TabIndex = 2;
      this.btnToggleLabelMode.Text = "Label Mode";
      this.btnToggleLabelMode.UseVisualStyleBackColor = true;
      this.btnToggleLabelMode.CheckedChanged += new System.EventHandler( this.btnToggleLabelMode_CheckedChanged );
      // 
      // menuBASIC
      // 
      this.menuBASIC.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.bASICToolStripMenuItem} );
      this.menuBASIC.Location = new System.Drawing.Point( 0, 0 );
      this.menuBASIC.Name = "menuBASIC";
      this.menuBASIC.Size = new System.Drawing.Size( 698, 24 );
      this.menuBASIC.TabIndex = 3;
      this.menuBASIC.Text = "menuStrip1";
      // 
      // bASICToolStripMenuItem
      // 
      this.bASICToolStripMenuItem.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.renumberToolStripMenuItem} );
      this.bASICToolStripMenuItem.Name = "bASICToolStripMenuItem";
      this.bASICToolStripMenuItem.Size = new System.Drawing.Size( 49, 20 );
      this.bASICToolStripMenuItem.Text = "BASIC";
      // 
      // renumberToolStripMenuItem
      // 
      this.renumberToolStripMenuItem.Name = "renumberToolStripMenuItem";
      this.renumberToolStripMenuItem.Size = new System.Drawing.Size( 152, 22 );
      this.renumberToolStripMenuItem.Text = "&Renumber...";
      this.renumberToolStripMenuItem.Click += new System.EventHandler( this.renumberToolStripMenuItem_Click );
      // 
      // SourceBasic
      // 
      this.ClientSize = new System.Drawing.Size( 698, 588 );
      this.Controls.Add( this.menuBASIC );
      this.Controls.Add( this.editSource );
      this.Controls.Add( this.btnToggleLabelMode );
      this.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
      this.MainMenuStrip = this.menuBASIC;
      this.Name = "SourceBasic";
      ( (System.ComponentModel.ISupportInitialize)( this.m_FileWatcher ) ).EndInit();
      ( (System.ComponentModel.ISupportInitialize)( this.editSource ) ).EndInit();
      this.menuBASIC.ResumeLayout( false );
      this.menuBASIC.PerformLayout();
      this.ResumeLayout( false );
      this.PerformLayout();

    }

    #endregion

    public ScintillaNET.Scintilla editSource;
    private System.Windows.Forms.ContextMenuStrip contextSource;
    private System.Windows.Forms.CheckBox btnToggleLabelMode;
    private System.Windows.Forms.MenuStrip menuBASIC;
    private System.Windows.Forms.ToolStripMenuItem bASICToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem renumberToolStripMenuItem;
  }
}
