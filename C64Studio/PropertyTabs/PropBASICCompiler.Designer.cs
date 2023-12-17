namespace RetroDevStudio
{
  partial class PropBASICCompiler
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropBASICCompiler));
      this.label1 = new System.Windows.Forms.Label();
      this.checkBASICWriteTempFileWithoutMetaData = new System.Windows.Forms.CheckBox();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point(305, 17);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(292, 87);
      this.label1.TabIndex = 11;
      this.label1.Text = resources.GetString("label1.Text");
      // 
      // checkBASICWriteTempFileWithoutMetaData
      // 
      this.checkBASICWriteTempFileWithoutMetaData.AutoSize = true;
      this.checkBASICWriteTempFileWithoutMetaData.Location = new System.Drawing.Point(12, 17);
      this.checkBASICWriteTempFileWithoutMetaData.Name = "checkBASICWriteTempFileWithoutMetaData";
      this.checkBASICWriteTempFileWithoutMetaData.Size = new System.Drawing.Size(180, 17);
      this.checkBASICWriteTempFileWithoutMetaData.TabIndex = 12;
      this.checkBASICWriteTempFileWithoutMetaData.Text = "Write temp file without meta data";
      this.checkBASICWriteTempFileWithoutMetaData.UseVisualStyleBackColor = true;
      this.checkBASICWriteTempFileWithoutMetaData.CheckedChanged += new System.EventHandler(this.checkBASICWriteTempFileWithoutMetaData_CheckedChanged);
      // 
      // PropBASICCompiler
      // 
      this.ClientSize = new System.Drawing.Size(599, 364);
      this.ControlBox = false;
      this.Controls.Add(this.checkBASICWriteTempFileWithoutMetaData);
      this.Controls.Add(this.label1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.Name = "PropBASICCompiler";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.CheckBox checkBASICWriteTempFileWithoutMetaData;
  }
}
