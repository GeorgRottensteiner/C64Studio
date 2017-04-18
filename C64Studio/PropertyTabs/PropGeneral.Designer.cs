namespace C64Studio
{
  partial class PropGeneral
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

    #region Vom Windows Form-Designer generierter Code

    /// <summary>
    /// Erforderliche Methode für die Designerunterstützung.
    /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
    /// </summary>
    private void InitializeComponent()
    {
      this.label2 = new System.Windows.Forms.Label();
      this.labelFilename = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.labelFilePath = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point( 6, 17 );
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size( 57, 13 );
      this.label2.TabIndex = 8;
      this.label2.Text = "File Name:";
      // 
      // labelFilename
      // 
      this.labelFilename.Location = new System.Drawing.Point( 100, 17 );
      this.labelFilename.Name = "labelFilename";
      this.labelFilename.Size = new System.Drawing.Size( 439, 13 );
      this.labelFilename.TabIndex = 8;
      this.labelFilename.Text = "Target File:";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point( 6, 41 );
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size( 32, 13 );
      this.label1.TabIndex = 8;
      this.label1.Text = "Path:";
      // 
      // labelFilePath
      // 
      this.labelFilePath.Location = new System.Drawing.Point( 100, 41 );
      this.labelFilePath.Name = "labelFilePath";
      this.labelFilePath.Size = new System.Drawing.Size( 439, 13 );
      this.labelFilePath.TabIndex = 8;
      this.labelFilePath.Text = "Target File:";
      // 
      // PropGeneral
      // 
      this.ClientSize = new System.Drawing.Size( 599, 364 );
      this.ControlBox = false;
      this.Controls.Add( this.labelFilePath );
      this.Controls.Add( this.label1 );
      this.Controls.Add( this.labelFilename );
      this.Controls.Add( this.label2 );
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.Name = "PropGeneral";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.ResumeLayout( false );
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label labelFilename;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label labelFilePath;
  }
}
