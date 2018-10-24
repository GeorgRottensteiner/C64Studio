namespace C64Studio
{
  partial class ProjectProperties
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose( bool disposing )
    {
      if ( disposing && ( components != null ) )
      {
        components.Dispose();
      }
      base.Dispose( disposing );
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.tabProjectProperties = new System.Windows.Forms.TabControl();
      this.tabGeneral = new System.Windows.Forms.TabPage();
      this.editProjectName = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.tabConfigurations = new System.Windows.Forms.TabPage();
      this.btnDeleteConfig = new System.Windows.Forms.Button();
      this.btnApplyChanges = new System.Windows.Forms.Button();
      this.btnAddConfig = new System.Windows.Forms.Button();
      this.editPreDefines = new System.Windows.Forms.TextBox();
      this.editConfigName = new System.Windows.Forms.TextBox();
      this.editDebugStartAddress = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.comboConfiguration = new System.Windows.Forms.ComboBox();
      this.label3 = new System.Windows.Forms.Label();
      this.btnClose = new System.Windows.Forms.Button();
      this.tabProjectProperties.SuspendLayout();
      this.tabGeneral.SuspendLayout();
      this.tabConfigurations.SuspendLayout();
      this.SuspendLayout();
      // 
      // tabProjectProperties
      // 
      this.tabProjectProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tabProjectProperties.Controls.Add(this.tabGeneral);
      this.tabProjectProperties.Controls.Add(this.tabConfigurations);
      this.tabProjectProperties.Location = new System.Drawing.Point(12, 12);
      this.tabProjectProperties.Name = "tabProjectProperties";
      this.tabProjectProperties.SelectedIndex = 0;
      this.tabProjectProperties.Size = new System.Drawing.Size(611, 376);
      this.tabProjectProperties.TabIndex = 0;
      // 
      // tabGeneral
      // 
      this.tabGeneral.Controls.Add(this.editProjectName);
      this.tabGeneral.Controls.Add(this.label2);
      this.tabGeneral.Location = new System.Drawing.Point(4, 22);
      this.tabGeneral.Name = "tabGeneral";
      this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
      this.tabGeneral.Size = new System.Drawing.Size(603, 350);
      this.tabGeneral.TabIndex = 0;
      this.tabGeneral.Text = "General";
      this.tabGeneral.UseVisualStyleBackColor = true;
      // 
      // editProjectName
      // 
      this.editProjectName.Location = new System.Drawing.Point(120, 9);
      this.editProjectName.Name = "editProjectName";
      this.editProjectName.Size = new System.Drawing.Size(180, 20);
      this.editProjectName.TabIndex = 1;
      this.editProjectName.TextChanged += new System.EventHandler(this.editProjectName_TextChanged);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(6, 12);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(38, 13);
      this.label2.TabIndex = 0;
      this.label2.Text = "Name:";
      // 
      // tabConfigurations
      // 
      this.tabConfigurations.Controls.Add(this.btnDeleteConfig);
      this.tabConfigurations.Controls.Add(this.btnApplyChanges);
      this.tabConfigurations.Controls.Add(this.btnAddConfig);
      this.tabConfigurations.Controls.Add(this.editPreDefines);
      this.tabConfigurations.Controls.Add(this.editConfigName);
      this.tabConfigurations.Controls.Add(this.editDebugStartAddress);
      this.tabConfigurations.Controls.Add(this.label5);
      this.tabConfigurations.Controls.Add(this.label4);
      this.tabConfigurations.Controls.Add(this.label1);
      this.tabConfigurations.Controls.Add(this.comboConfiguration);
      this.tabConfigurations.Controls.Add(this.label3);
      this.tabConfigurations.Location = new System.Drawing.Point(4, 22);
      this.tabConfigurations.Name = "tabConfigurations";
      this.tabConfigurations.Padding = new System.Windows.Forms.Padding(3);
      this.tabConfigurations.Size = new System.Drawing.Size(603, 350);
      this.tabConfigurations.TabIndex = 1;
      this.tabConfigurations.Text = "Configuration";
      this.tabConfigurations.UseVisualStyleBackColor = true;
      // 
      // btnDeleteConfig
      // 
      this.btnDeleteConfig.Location = new System.Drawing.Point(488, 7);
      this.btnDeleteConfig.Name = "btnDeleteConfig";
      this.btnDeleteConfig.Size = new System.Drawing.Size(75, 23);
      this.btnDeleteConfig.TabIndex = 7;
      this.btnDeleteConfig.Text = "Delete";
      this.btnDeleteConfig.UseVisualStyleBackColor = true;
      this.btnDeleteConfig.Click += new System.EventHandler(this.btnDeleteConfig_Click);
      // 
      // btnApplyChanges
      // 
      this.btnApplyChanges.Enabled = false;
      this.btnApplyChanges.Location = new System.Drawing.Point(488, 108);
      this.btnApplyChanges.Name = "btnApplyChanges";
      this.btnApplyChanges.Size = new System.Drawing.Size(72, 23);
      this.btnApplyChanges.TabIndex = 7;
      this.btnApplyChanges.Text = "Apply Changes";
      this.btnApplyChanges.UseVisualStyleBackColor = true;
      this.btnApplyChanges.Click += new System.EventHandler(this.btnApplyChanges_Click);
      // 
      // btnAddConfig
      // 
      this.btnAddConfig.Location = new System.Drawing.Point(488, 36);
      this.btnAddConfig.Name = "btnAddConfig";
      this.btnAddConfig.Size = new System.Drawing.Size(75, 23);
      this.btnAddConfig.TabIndex = 7;
      this.btnAddConfig.Text = "Add New";
      this.btnAddConfig.UseVisualStyleBackColor = true;
      this.btnAddConfig.Click += new System.EventHandler(this.btnAddConfig_Click);
      // 
      // editPreDefines
      // 
      this.editPreDefines.AcceptsReturn = true;
      this.editPreDefines.Location = new System.Drawing.Point(120, 138);
      this.editPreDefines.Multiline = true;
      this.editPreDefines.Name = "editPreDefines";
      this.editPreDefines.Size = new System.Drawing.Size(362, 133);
      this.editPreDefines.TabIndex = 6;
      this.editPreDefines.TextChanged += new System.EventHandler(this.editPreDefines_TextChanged);
      // 
      // editConfigName
      // 
      this.editConfigName.Location = new System.Drawing.Point(120, 36);
      this.editConfigName.Name = "editConfigName";
      this.editConfigName.Size = new System.Drawing.Size(362, 20);
      this.editConfigName.TabIndex = 5;
      // 
      // editDebugStartAddress
      // 
      this.editDebugStartAddress.Location = new System.Drawing.Point(120, 111);
      this.editDebugStartAddress.Name = "editDebugStartAddress";
      this.editDebugStartAddress.Size = new System.Drawing.Size(86, 20);
      this.editDebugStartAddress.TabIndex = 5;
      this.editDebugStartAddress.TextChanged += new System.EventHandler(this.editDebugStartAddress_TextChanged);
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(6, 39);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(38, 13);
      this.label5.TabIndex = 4;
      this.label5.Text = "Name:";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(6, 141);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(60, 13);
      this.label4.TabIndex = 4;
      this.label4.Text = "Predefines:";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(6, 114);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(108, 13);
      this.label1.TabIndex = 4;
      this.label1.Text = "Debug Start Address:";
      // 
      // comboConfiguration
      // 
      this.comboConfiguration.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboConfiguration.FormattingEnabled = true;
      this.comboConfiguration.Location = new System.Drawing.Point(120, 9);
      this.comboConfiguration.Name = "comboConfiguration";
      this.comboConfiguration.Size = new System.Drawing.Size(362, 21);
      this.comboConfiguration.TabIndex = 1;
      this.comboConfiguration.SelectedIndexChanged += new System.EventHandler(this.comboConfiguration_SelectedIndexChanged);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(6, 12);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(72, 13);
      this.label3.TabIndex = 0;
      this.label3.Text = "Configuration:";
      // 
      // btnClose
      // 
      this.btnClose.Location = new System.Drawing.Point(548, 394);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new System.Drawing.Size(75, 23);
      this.btnClose.TabIndex = 1;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
      // 
      // ProjectProperties
      // 
      this.AcceptButton = this.btnClose;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(635, 429);
      this.Controls.Add(this.btnClose);
      this.Controls.Add(this.tabProjectProperties);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ProjectProperties";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = "Project Properties";
      this.tabProjectProperties.ResumeLayout(false);
      this.tabGeneral.ResumeLayout(false);
      this.tabGeneral.PerformLayout();
      this.tabConfigurations.ResumeLayout(false);
      this.tabConfigurations.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TabControl tabProjectProperties;
    private System.Windows.Forms.TabPage tabGeneral;
    private System.Windows.Forms.TabPage tabConfigurations;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox editProjectName;
    private System.Windows.Forms.ComboBox comboConfiguration;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox editDebugStartAddress;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox editPreDefines;
    private System.Windows.Forms.Button btnDeleteConfig;
    private System.Windows.Forms.Button btnApplyChanges;
    private System.Windows.Forms.Button btnAddConfig;
    private System.Windows.Forms.TextBox editConfigName;
    private System.Windows.Forms.Label label5;
  }
}