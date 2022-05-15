namespace RetroDevStudio
{
  partial class EditC64Filename
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
      this.SuspendLayout();
      // 
      // FormTextBox
      // 
      this.Leave += new System.EventHandler( this.FormTextBox_Leave );
      this.MouseClick += new System.Windows.Forms.MouseEventHandler( this.FormTextBox_MouseClick );
      this.MouseDown += new System.Windows.Forms.MouseEventHandler( this.FormTextBox_MouseDown );
      this.KeyPress += new System.Windows.Forms.KeyPressEventHandler( this.FormTextBox_KeyPress );
      this.Enter += new System.EventHandler( this.FormTextBox_Enter );
      this.SizeChanged += new System.EventHandler( this.FormTextBox_SizeChanged );
      this.ResumeLayout( false );

    }

    #endregion
  }
}
