namespace RetroDevStudio
{
  partial class PropBuildEvents
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
      this.comboConfig = new System.Windows.Forms.ComboBox();
      this.comboBuildEvents = new System.Windows.Forms.ComboBox();
      this.label7 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // comboConfig
      // 
      this.comboConfig.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.comboConfig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboConfig.FormattingEnabled = true;
      this.comboConfig.Location = new System.Drawing.Point(84, 14);
      this.comboConfig.Name = "comboConfig";
      this.comboConfig.Size = new System.Drawing.Size(503, 21);
      this.comboConfig.TabIndex = 12;
      this.comboConfig.SelectedIndexChanged += new System.EventHandler(this.comboConfig_SelectedIndexChanged);
      // 
      // comboBuildEvents
      // 
      this.comboBuildEvents.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBuildEvents.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBuildEvents.FormattingEnabled = true;
      this.comboBuildEvents.Location = new System.Drawing.Point(84, 38);
      this.comboBuildEvents.Name = "comboBuildEvents";
      this.comboBuildEvents.Size = new System.Drawing.Size(503, 21);
      this.comboBuildEvents.TabIndex = 11;
      this.comboBuildEvents.SelectedIndexChanged += new System.EventHandler(this.comboBuildEvents_SelectedIndexChanged);
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(6, 17);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(72, 13);
      this.label7.TabIndex = 9;
      this.label7.Text = "Configuration:";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(6, 41);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(64, 13);
      this.label5.TabIndex = 10;
      this.label5.Text = "Build Event:";
      // 
      // PropBuildEvents
      // 
      this.ClientSize = new System.Drawing.Size(599, 364);
      this.ControlBox = false;
      this.Controls.Add(this.comboConfig);
      this.Controls.Add(this.comboBuildEvents);
      this.Controls.Add(this.label7);
      this.Controls.Add(this.label5);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.Name = "PropBuildEvents";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ComboBox comboConfig;
    private System.Windows.Forms.ComboBox comboBuildEvents;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Label label5;

  }
}
