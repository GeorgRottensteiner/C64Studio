namespace RetroDevStudio.Dialogs
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
      this.labelProjectFile = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.tabConfigurations = new System.Windows.Forms.TabPage();
      this.groupConfigSettings = new System.Windows.Forms.GroupBox();
      this.btnApplyChanges = new DecentForms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.editDebugStartAddress = new System.Windows.Forms.TextBox();
      this.editPreDefines = new System.Windows.Forms.TextBox();
      this.btnDeleteConfig = new DecentForms.Button();
      this.btnAddConfig = new DecentForms.Button();
      this.editConfigName = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.comboConfiguration = new System.Windows.Forms.ComboBox();
      this.label3 = new System.Windows.Forms.Label();
      this.btnClose = new DecentForms.Button();
      this.tabProjectProperties.SuspendLayout();
      this.tabGeneral.SuspendLayout();
      this.tabConfigurations.SuspendLayout();
      this.groupConfigSettings.SuspendLayout();
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
      this.tabProjectProperties.Size = new System.Drawing.Size(637, 376);
      this.tabProjectProperties.TabIndex = 0;
      // 
      // tabGeneral
      // 
      this.tabGeneral.Controls.Add(this.editProjectName);
      this.tabGeneral.Controls.Add(this.labelProjectFile);
      this.tabGeneral.Controls.Add(this.label6);
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
      // labelProjectFile
      // 
      this.labelProjectFile.Location = new System.Drawing.Point(117, 39);
      this.labelProjectFile.Name = "labelProjectFile";
      this.labelProjectFile.Size = new System.Drawing.Size(455, 13);
      this.labelProjectFile.TabIndex = 0;
      this.labelProjectFile.Text = "Name:";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(6, 39);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(62, 13);
      this.label6.TabIndex = 0;
      this.label6.Text = "Project File:";
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
      this.tabConfigurations.Controls.Add(this.groupConfigSettings);
      this.tabConfigurations.Controls.Add(this.btnDeleteConfig);
      this.tabConfigurations.Controls.Add(this.btnAddConfig);
      this.tabConfigurations.Controls.Add(this.editConfigName);
      this.tabConfigurations.Controls.Add(this.label5);
      this.tabConfigurations.Controls.Add(this.comboConfiguration);
      this.tabConfigurations.Controls.Add(this.label3);
      this.tabConfigurations.Location = new System.Drawing.Point(4, 22);
      this.tabConfigurations.Name = "tabConfigurations";
      this.tabConfigurations.Padding = new System.Windows.Forms.Padding(3);
      this.tabConfigurations.Size = new System.Drawing.Size(629, 350);
      this.tabConfigurations.TabIndex = 1;
      this.tabConfigurations.Text = "Configuration";
      this.tabConfigurations.UseVisualStyleBackColor = true;
      // 
      // groupConfigSettings
      // 
      this.groupConfigSettings.Controls.Add(this.btnApplyChanges);
      this.groupConfigSettings.Controls.Add(this.label1);
      this.groupConfigSettings.Controls.Add(this.label4);
      this.groupConfigSettings.Controls.Add(this.editDebugStartAddress);
      this.groupConfigSettings.Controls.Add(this.editPreDefines);
      this.groupConfigSettings.Location = new System.Drawing.Point(9, 82);
      this.groupConfigSettings.Name = "groupConfigSettings";
      this.groupConfigSettings.Size = new System.Drawing.Size(614, 262);
      this.groupConfigSettings.TabIndex = 8;
      this.groupConfigSettings.TabStop = false;
      this.groupConfigSettings.Text = "Configuration: Default";
      // 
      // btnApplyChanges
      // 
      this.btnApplyChanges.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnApplyChanges.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnApplyChanges.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnApplyChanges.Enabled = false;
      this.btnApplyChanges.Image = null;
      this.btnApplyChanges.Location = new System.Drawing.Point(507, 233);
      this.btnApplyChanges.Name = "btnApplyChanges";
      this.btnApplyChanges.Size = new System.Drawing.Size(101, 23);
      this.btnApplyChanges.TabIndex = 7;
      this.btnApplyChanges.Text = "Apply Changes";
      this.btnApplyChanges.Click += new DecentForms.EventHandler(this.btnApplyChanges_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(6, 25);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(108, 13);
      this.label1.TabIndex = 4;
      this.label1.Text = "Debug Start Address:";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(10, 52);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(60, 13);
      this.label4.TabIndex = 4;
      this.label4.Text = "Predefines:";
      // 
      // editDebugStartAddress
      // 
      this.editDebugStartAddress.Location = new System.Drawing.Point(124, 22);
      this.editDebugStartAddress.Name = "editDebugStartAddress";
      this.editDebugStartAddress.Size = new System.Drawing.Size(86, 20);
      this.editDebugStartAddress.TabIndex = 5;
      this.editDebugStartAddress.TextChanged += new System.EventHandler(this.editDebugStartAddress_TextChanged);
      // 
      // editPreDefines
      // 
      this.editPreDefines.AcceptsReturn = true;
      this.editPreDefines.Location = new System.Drawing.Point(124, 49);
      this.editPreDefines.Multiline = true;
      this.editPreDefines.Name = "editPreDefines";
      this.editPreDefines.Size = new System.Drawing.Size(377, 207);
      this.editPreDefines.TabIndex = 6;
      this.editPreDefines.TextChanged += new System.EventHandler(this.editPreDefines_TextChanged);
      // 
      // btnDeleteConfig
      // 
      this.btnDeleteConfig.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnDeleteConfig.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnDeleteConfig.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnDeleteConfig.Image = null;
      this.btnDeleteConfig.Location = new System.Drawing.Point(516, 7);
      this.btnDeleteConfig.Name = "btnDeleteConfig";
      this.btnDeleteConfig.Size = new System.Drawing.Size(101, 23);
      this.btnDeleteConfig.TabIndex = 7;
      this.btnDeleteConfig.Text = "Delete";
      this.btnDeleteConfig.Click += new DecentForms.EventHandler(this.btnDeleteConfig_Click);
      // 
      // btnAddConfig
      // 
      this.btnAddConfig.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnAddConfig.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnAddConfig.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnAddConfig.Image = null;
      this.btnAddConfig.Location = new System.Drawing.Point(516, 36);
      this.btnAddConfig.Name = "btnAddConfig";
      this.btnAddConfig.Size = new System.Drawing.Size(101, 23);
      this.btnAddConfig.TabIndex = 7;
      this.btnAddConfig.Text = "Add New";
      this.btnAddConfig.Click += new DecentForms.EventHandler(this.btnAddConfig_Click);
      // 
      // editConfigName
      // 
      this.editConfigName.Location = new System.Drawing.Point(133, 36);
      this.editConfigName.Name = "editConfigName";
      this.editConfigName.Size = new System.Drawing.Size(377, 20);
      this.editConfigName.TabIndex = 5;
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
      // comboConfiguration
      // 
      this.comboConfiguration.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboConfiguration.FormattingEnabled = true;
      this.comboConfiguration.Location = new System.Drawing.Point(133, 9);
      this.comboConfiguration.Name = "comboConfiguration";
      this.comboConfiguration.Size = new System.Drawing.Size(377, 21);
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
      this.btnClose.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnClose.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnClose.Image = null;
      this.btnClose.Location = new System.Drawing.Point(570, 394);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new System.Drawing.Size(75, 23);
      this.btnClose.TabIndex = 1;
      this.btnClose.Text = "Close";
      this.btnClose.Click += new DecentForms.EventHandler(this.btnClose_Click);
      // 
      // ProjectProperties
      // 
      this.AcceptButton = this.btnClose;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(661, 429);
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
      this.groupConfigSettings.ResumeLayout(false);
      this.groupConfigSettings.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TabControl tabProjectProperties;
    private System.Windows.Forms.TabPage tabGeneral;
    private System.Windows.Forms.TabPage tabConfigurations;
    private DecentForms.Button btnClose;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.TextBox editProjectName;
    private System.Windows.Forms.ComboBox comboConfiguration;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox editDebugStartAddress;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox editPreDefines;
    private DecentForms.Button btnDeleteConfig;
    private DecentForms.Button btnApplyChanges;
    private DecentForms.Button btnAddConfig;
    private System.Windows.Forms.TextBox editConfigName;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.GroupBox groupConfigSettings;
    private System.Windows.Forms.Label labelProjectFile;
    private System.Windows.Forms.Label label6;
  }
}