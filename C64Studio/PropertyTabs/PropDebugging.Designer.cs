namespace RetroDevStudio
{
  partial class PropDebugging
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
      this.label1 = new System.Windows.Forms.Label();
      this.editDebugCommand = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.btnMacros = new System.Windows.Forms.Button();
      this.comboDebugFileType = new System.Windows.Forms.ComboBox();
      this.label3 = new System.Windows.Forms.Label();
      this.comboConfig = new System.Windows.Forms.ComboBox();
      this.label7 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point( 394, 17 );
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size( 193, 83 );
      this.label1.TabIndex = 11;
      this.label1.Text = "Leave empty to run the built file.\r\nFill manually to provide an alternative file " +
          "name to run for debugging.\r\n";
      // 
      // editDebugCommand
      // 
      this.editDebugCommand.Location = new System.Drawing.Point( 110, 39 );
      this.editDebugCommand.Name = "editDebugCommand";
      this.editDebugCommand.Size = new System.Drawing.Size( 278, 20 );
      this.editDebugCommand.TabIndex = 9;
      this.editDebugCommand.TextChanged += new System.EventHandler( this.editDebugCommand_TextChanged );
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point( 6, 42 );
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size( 61, 13 );
      this.label2.TabIndex = 8;
      this.label2.Text = "Debug File:";
      // 
      // btnMacros
      // 
      this.btnMacros.Location = new System.Drawing.Point( 313, 65 );
      this.btnMacros.Name = "btnMacros";
      this.btnMacros.Size = new System.Drawing.Size( 75, 23 );
      this.btnMacros.TabIndex = 15;
      this.btnMacros.Text = "Macros";
      this.btnMacros.UseVisualStyleBackColor = true;
      this.btnMacros.Click += new System.EventHandler( this.btnMacros_Click );
      // 
      // comboDebugFileType
      // 
      this.comboDebugFileType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboDebugFileType.FormattingEnabled = true;
      this.comboDebugFileType.Location = new System.Drawing.Point( 110, 94 );
      this.comboDebugFileType.Name = "comboDebugFileType";
      this.comboDebugFileType.Size = new System.Drawing.Size( 278, 21 );
      this.comboDebugFileType.TabIndex = 17;
      this.comboDebugFileType.SelectedIndexChanged += new System.EventHandler( this.comboDebugFileType_SelectedIndexChanged );
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point( 6, 97 );
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size( 84, 13 );
      this.label3.TabIndex = 16;
      this.label3.Text = "Debug File type:";
      // 
      // comboConfig
      // 
      this.comboConfig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboConfig.FormattingEnabled = true;
      this.comboConfig.Location = new System.Drawing.Point( 110, 12 );
      this.comboConfig.Name = "comboConfig";
      this.comboConfig.Size = new System.Drawing.Size( 278, 21 );
      this.comboConfig.TabIndex = 19;
      this.comboConfig.SelectedIndexChanged += new System.EventHandler( this.comboConfig_SelectedIndexChanged );
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point( 6, 15 );
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size( 72, 13 );
      this.label7.TabIndex = 18;
      this.label7.Text = "Configuration:";
      // 
      // PropDebugging
      // 
      this.ClientSize = new System.Drawing.Size( 599, 364 );
      this.ControlBox = false;
      this.Controls.Add( this.comboConfig );
      this.Controls.Add( this.label7 );
      this.Controls.Add( this.comboDebugFileType );
      this.Controls.Add( this.label3 );
      this.Controls.Add( this.btnMacros );
      this.Controls.Add( this.label1 );
      this.Controls.Add( this.editDebugCommand );
      this.Controls.Add( this.label2 );
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.Name = "PropDebugging";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.ResumeLayout( false );
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox editDebugCommand;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button btnMacros;
    private System.Windows.Forms.ComboBox comboDebugFileType;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.ComboBox comboConfig;
    private System.Windows.Forms.Label label7;
  }
}
