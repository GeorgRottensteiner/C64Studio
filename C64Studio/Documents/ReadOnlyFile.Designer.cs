namespace C64Studio
{
  partial class ReadOnlyFile
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( ReadOnlyFile ) );
      this.editText = new FastColoredTextBoxNS.FastColoredTextBox();
      ( (System.ComponentModel.ISupportInitialize)( this.m_FileWatcher ) ).BeginInit();
      ( (System.ComponentModel.ISupportInitialize)( this.editText ) ).BeginInit();
      this.SuspendLayout();
      // 
      // editText
      // 
      this.editText.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
      this.editText.AutoScrollMinSize = new System.Drawing.Size( 27, 14 );
      this.editText.BackBrush = null;
      this.editText.CharHeight = 14;
      this.editText.CharWidth = 8;
      this.editText.Cursor = System.Windows.Forms.Cursors.IBeam;
      this.editText.DisabledColor = System.Drawing.Color.FromArgb( ( (int)( ( (byte)( 100 ) ) ) ), ( (int)( ( (byte)( 180 ) ) ) ), ( (int)( ( (byte)( 180 ) ) ) ), ( (int)( ( (byte)( 180 ) ) ) ) );
      this.editText.Dock = System.Windows.Forms.DockStyle.Fill;
      this.editText.IsReplaceMode = false;
      this.editText.Location = new System.Drawing.Point( 0, 0 );
      this.editText.Name = "editText";
      this.editText.Paddings = new System.Windows.Forms.Padding( 0 );
      this.editText.ReadOnly = true;
      this.editText.SelectionColor = System.Drawing.Color.FromArgb( ( (int)( ( (byte)( 60 ) ) ) ), ( (int)( ( (byte)( 0 ) ) ) ), ( (int)( ( (byte)( 0 ) ) ) ), ( (int)( ( (byte)( 255 ) ) ) ) );
      this.editText.ServiceColors = ( (FastColoredTextBoxNS.ServiceColors)( resources.GetObject( "editText.ServiceColors" ) ) );
      this.editText.Size = new System.Drawing.Size( 534, 390 );
      this.editText.TabIndex = 0;
      this.editText.TabLength = 2;
      this.editText.Zoom = 100;
      // 
      // ReadOnlyFile
      // 
      this.ClientSize = new System.Drawing.Size( 534, 390 );
      this.Controls.Add( this.editText );
      this.Name = "ReadOnlyFile";
      ( (System.ComponentModel.ISupportInitialize)( this.m_FileWatcher ) ).EndInit();
      ( (System.ComponentModel.ISupportInitialize)( this.editText ) ).EndInit();
      this.ResumeLayout( false );

    }

    #endregion

    public FastColoredTextBoxNS.FastColoredTextBox editText;
  }
}
