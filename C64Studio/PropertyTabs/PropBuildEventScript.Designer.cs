namespace RetroDevStudio
{
  partial class PropBuildEventScript
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
      this.btnMacros = new DecentForms.Button();
      this.editBuildCommand = new System.Windows.Forms.TextBox();
      this.label6 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // btnMacros
      // 
      this.btnMacros.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnMacros.Location = new System.Drawing.Point(84, 255);
      this.btnMacros.Name = "btnMacros";
      this.btnMacros.Size = new System.Drawing.Size(75, 23);
      this.btnMacros.TabIndex = 14;
      this.btnMacros.Text = "Macros";
      this.btnMacros.Click += new DecentForms.EventHandler(this.btnMacros_Click);
      // 
      // editBuildCommand
      // 
      this.editBuildCommand.AcceptsReturn = true;
      this.editBuildCommand.AcceptsTab = true;
      this.editBuildCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.editBuildCommand.Location = new System.Drawing.Point(84, 12);
      this.editBuildCommand.Multiline = true;
      this.editBuildCommand.Name = "editBuildCommand";
      this.editBuildCommand.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.editBuildCommand.Size = new System.Drawing.Size(503, 237);
      this.editBuildCommand.TabIndex = 13;
      this.editBuildCommand.WordWrap = false;
      this.editBuildCommand.TextChanged += new System.EventHandler(this.editBuildCommand_TextChanged);
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(6, 15);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(62, 13);
      this.label6.TabIndex = 8;
      this.label6.Text = "Commands:";
      // 
      // PropBuildEvents
      // 
      this.ClientSize = new System.Drawing.Size(599, 364);
      this.ControlBox = false;
      this.Controls.Add(this.btnMacros);
      this.Controls.Add(this.editBuildCommand);
      this.Controls.Add(this.label6);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.Name = "PropBuildEvents";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private DecentForms.Button btnMacros;
    private System.Windows.Forms.TextBox editBuildCommand;
    private System.Windows.Forms.Label label6;

  }
}
