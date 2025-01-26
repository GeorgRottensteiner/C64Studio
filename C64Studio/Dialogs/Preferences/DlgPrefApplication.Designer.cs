namespace RetroDevStudio.Dialogs.Preferences
{
  partial class DlgPrefApplication
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.comboAppMode = new System.Windows.Forms.ComboBox();
      this.label29 = new System.Windows.Forms.Label();
      this.btnBrowseDefaultOpenSolutionPath = new DecentForms.Button();
      this.editDefaultOpenSolutionPath = new System.Windows.Forms.TextBox();
      this.editMaxMRUEntries = new System.Windows.Forms.TextBox();
      this.label32 = new System.Windows.Forms.Label();
      this.label31 = new System.Windows.Forms.Label();
      this.checkForUpdate = new System.Windows.Forms.CheckBox();
      this.checkShowOutputDisplayAfterBuild = new System.Windows.Forms.CheckBox();
      this.checkShowCompilerMessagesAfterBuild = new System.Windows.Forms.CheckBox();
      this.checkAutoOpenLastSolution = new System.Windows.Forms.CheckBox();
      this.btnRegisterSolutionFileType = new DecentForms.Button();
      this.btnRegisterProjectFileType = new DecentForms.Button();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // comboAppMode
      // 
      this.comboAppMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboAppMode.FormattingEnabled = true;
      this.comboAppMode.Items.AddRange(new object[] {
            "Undecided",
            "Normal (settings in UserAppData)",
            "Portable Mode (settings local)"});
      this.comboAppMode.Location = new System.Drawing.Point(219, 7);
      this.comboAppMode.Name = "comboAppMode";
      this.comboAppMode.Size = new System.Drawing.Size(322, 21);
      this.comboAppMode.TabIndex = 0;
      this.comboAppMode.SelectedIndexChanged += new System.EventHandler(this.comboAppMode_SelectedIndexChanged);
      // 
      // label29
      // 
      this.label29.AutoSize = true;
      this.label29.Location = new System.Drawing.Point(3, 10);
      this.label29.Name = "label29";
      this.label29.Size = new System.Drawing.Size(92, 13);
      this.label29.TabIndex = 14;
      this.label29.Text = "Application Mode:";
      // 
      // btnBrowseDefaultOpenSolutionPath
      // 
      this.btnBrowseDefaultOpenSolutionPath.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnBrowseDefaultOpenSolutionPath.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnBrowseDefaultOpenSolutionPath.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnBrowseDefaultOpenSolutionPath.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnBrowseDefaultOpenSolutionPath.Image = null;
      this.btnBrowseDefaultOpenSolutionPath.Location = new System.Drawing.Point(518, 87);
      this.btnBrowseDefaultOpenSolutionPath.Name = "btnBrowseDefaultOpenSolutionPath";
      this.btnBrowseDefaultOpenSolutionPath.Size = new System.Drawing.Size(23, 20);
      this.btnBrowseDefaultOpenSolutionPath.TabIndex = 4;
      this.btnBrowseDefaultOpenSolutionPath.Text = "...";
      this.btnBrowseDefaultOpenSolutionPath.Click += new DecentForms.EventHandler(this.btnBrowseDefaultOpenSolutionPath_Click);
      // 
      // editDefaultOpenSolutionPath
      // 
      this.editDefaultOpenSolutionPath.Location = new System.Drawing.Point(219, 87);
      this.editDefaultOpenSolutionPath.Name = "editDefaultOpenSolutionPath";
      this.editDefaultOpenSolutionPath.Size = new System.Drawing.Size(293, 20);
      this.editDefaultOpenSolutionPath.TabIndex = 3;
      this.editDefaultOpenSolutionPath.TextChanged += new System.EventHandler(this.editDefaultOpenSolutionPath_TextChanged);
      // 
      // editMaxMRUEntries
      // 
      this.editMaxMRUEntries.Location = new System.Drawing.Point(219, 34);
      this.editMaxMRUEntries.MaxLength = 2;
      this.editMaxMRUEntries.Name = "editMaxMRUEntries";
      this.editMaxMRUEntries.Size = new System.Drawing.Size(322, 20);
      this.editMaxMRUEntries.TabIndex = 1;
      this.editMaxMRUEntries.TextChanged += new System.EventHandler(this.editMaxMRUEntries_TextChanged);
      // 
      // label32
      // 
      this.label32.AutoSize = true;
      this.label32.Location = new System.Drawing.Point(3, 37);
      this.label32.Name = "label32";
      this.label32.Size = new System.Drawing.Size(95, 13);
      this.label32.TabIndex = 19;
      this.label32.Text = "Max. MRU entries:";
      // 
      // label31
      // 
      this.label31.AutoSize = true;
      this.label31.Location = new System.Drawing.Point(3, 90);
      this.label31.Name = "label31";
      this.label31.Size = new System.Drawing.Size(139, 13);
      this.label31.TabIndex = 20;
      this.label31.Text = "Default Solution Open Path:";
      // 
      // checkForUpdate
      // 
      this.checkForUpdate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkForUpdate.Checked = true;
      this.checkForUpdate.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkForUpdate.Location = new System.Drawing.Point(3, 173);
      this.checkForUpdate.Name = "checkForUpdate";
      this.checkForUpdate.Size = new System.Drawing.Size(230, 24);
      this.checkForUpdate.TabIndex = 6;
      this.checkForUpdate.Text = "Check for update on startup";
      this.checkForUpdate.UseVisualStyleBackColor = true;
      this.checkForUpdate.CheckedChanged += new System.EventHandler(this.checkForUpdate_CheckedChanged);
      // 
      // checkShowOutputDisplayAfterBuild
      // 
      this.checkShowOutputDisplayAfterBuild.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkShowOutputDisplayAfterBuild.Checked = true;
      this.checkShowOutputDisplayAfterBuild.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkShowOutputDisplayAfterBuild.Location = new System.Drawing.Point(3, 143);
      this.checkShowOutputDisplayAfterBuild.Name = "checkShowOutputDisplayAfterBuild";
      this.checkShowOutputDisplayAfterBuild.Size = new System.Drawing.Size(230, 24);
      this.checkShowOutputDisplayAfterBuild.TabIndex = 5;
      this.checkShowOutputDisplayAfterBuild.Text = "Show \"Output Display\" after build";
      this.checkShowOutputDisplayAfterBuild.UseVisualStyleBackColor = true;
      this.checkShowOutputDisplayAfterBuild.CheckedChanged += new System.EventHandler(this.checkShowOutputDisplayAfterBuild_CheckedChanged);
      // 
      // checkShowCompilerMessagesAfterBuild
      // 
      this.checkShowCompilerMessagesAfterBuild.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkShowCompilerMessagesAfterBuild.Checked = true;
      this.checkShowCompilerMessagesAfterBuild.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkShowCompilerMessagesAfterBuild.Location = new System.Drawing.Point(3, 113);
      this.checkShowCompilerMessagesAfterBuild.Name = "checkShowCompilerMessagesAfterBuild";
      this.checkShowCompilerMessagesAfterBuild.Size = new System.Drawing.Size(230, 24);
      this.checkShowCompilerMessagesAfterBuild.TabIndex = 4;
      this.checkShowCompilerMessagesAfterBuild.Text = "Show \"Compiler Messages\" after build";
      this.checkShowCompilerMessagesAfterBuild.UseVisualStyleBackColor = true;
      this.checkShowCompilerMessagesAfterBuild.CheckedChanged += new System.EventHandler(this.checkShowCompilerMessagesAfterBuild_CheckedChanged);
      // 
      // checkAutoOpenLastSolution
      // 
      this.checkAutoOpenLastSolution.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkAutoOpenLastSolution.Location = new System.Drawing.Point(3, 60);
      this.checkAutoOpenLastSolution.Name = "checkAutoOpenLastSolution";
      this.checkAutoOpenLastSolution.Size = new System.Drawing.Size(230, 24);
      this.checkAutoOpenLastSolution.TabIndex = 2;
      this.checkAutoOpenLastSolution.Text = "Open last solution on startup";
      this.checkAutoOpenLastSolution.UseVisualStyleBackColor = true;
      this.checkAutoOpenLastSolution.CheckedChanged += new System.EventHandler(this.checkAutoOpenLastSolution_CheckedChanged);
      // 
      // btnRegisterSolutionFileType
      // 
      this.btnRegisterSolutionFileType.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnRegisterSolutionFileType.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnRegisterSolutionFileType.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnRegisterSolutionFileType.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnRegisterSolutionFileType.Image = null;
      this.btnRegisterSolutionFileType.Location = new System.Drawing.Point(13, 28);
      this.btnRegisterSolutionFileType.Name = "btnRegisterSolutionFileType";
      this.btnRegisterSolutionFileType.Size = new System.Drawing.Size(123, 20);
      this.btnRegisterSolutionFileType.TabIndex = 4;
      this.btnRegisterSolutionFileType.Text = "Solution Files (*.s64)";
      this.btnRegisterSolutionFileType.Click += new DecentForms.EventHandler(this.btnRegisterSolutionFileType_Click);
      // 
      // btnRegisterProjectFileType
      // 
      this.btnRegisterProjectFileType.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnRegisterProjectFileType.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnRegisterProjectFileType.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnRegisterProjectFileType.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnRegisterProjectFileType.Image = null;
      this.btnRegisterProjectFileType.Location = new System.Drawing.Point(142, 28);
      this.btnRegisterProjectFileType.Name = "btnRegisterProjectFileType";
      this.btnRegisterProjectFileType.Size = new System.Drawing.Size(123, 20);
      this.btnRegisterProjectFileType.TabIndex = 4;
      this.btnRegisterProjectFileType.Text = "Project Files (*.c64)";
      this.btnRegisterProjectFileType.Click += new DecentForms.EventHandler(this.btnRegisterProjectFileType_Click);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.btnRegisterSolutionFileType);
      this.groupBox1.Controls.Add(this.btnRegisterProjectFileType);
      this.groupBox1.Location = new System.Drawing.Point(6, 203);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(535, 72);
      this.groupBox1.TabIndex = 21;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Register File Types with Explorer";
      this.groupBox1.Visible = false;
      // 
      // DlgPrefApplication
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.btnBrowseDefaultOpenSolutionPath);
      this.Controls.Add(this.comboAppMode);
      this.Controls.Add(this.checkShowOutputDisplayAfterBuild);
      this.Controls.Add(this.label32);
      this.Controls.Add(this.editDefaultOpenSolutionPath);
      this.Controls.Add(this.checkAutoOpenLastSolution);
      this.Controls.Add(this.checkForUpdate);
      this.Controls.Add(this.label29);
      this.Controls.Add(this.checkShowCompilerMessagesAfterBuild);
      this.Controls.Add(this.label31);
      this.Controls.Add(this.editMaxMRUEntries);
      this.Name = "DlgPrefApplication";
      this.Size = new System.Drawing.Size(579, 295);
      this.groupBox1.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

        #endregion
        private System.Windows.Forms.ComboBox comboAppMode;
        private System.Windows.Forms.Label label29;
    private DecentForms.Button btnBrowseDefaultOpenSolutionPath;
    private System.Windows.Forms.TextBox editDefaultOpenSolutionPath;
    private System.Windows.Forms.TextBox editMaxMRUEntries;
    private System.Windows.Forms.Label label32;
    private System.Windows.Forms.Label label31;
    private System.Windows.Forms.CheckBox checkAutoOpenLastSolution;
    private System.Windows.Forms.CheckBox checkShowCompilerMessagesAfterBuild;
    private System.Windows.Forms.CheckBox checkShowOutputDisplayAfterBuild;
    private System.Windows.Forms.CheckBox checkForUpdate;
    private DecentForms.Button btnRegisterSolutionFileType;
    private DecentForms.Button btnRegisterProjectFileType;
    private System.Windows.Forms.GroupBox groupBox1;
  }
}
