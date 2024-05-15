namespace RetroDevStudio.Dialogs.Preferences
{
  partial class PrefSounds
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
      this.btnExportSettings = new DecentForms.Button();
      this.btnImportSettings = new DecentForms.Button();
      this.checkPlaySoundSearchTextNotFound = new System.Windows.Forms.CheckBox();
      this.checkPlaySoundCompileSuccessful = new System.Windows.Forms.CheckBox();
      this.checkPlaySoundCompileFail = new System.Windows.Forms.CheckBox();
      this.label11 = new System.Windows.Forms.Label();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.btnTestSoundBuildFailure = new DecentForms.Button();
      this.btnTestSoundBuildSuccess = new DecentForms.Button();
      this.btnTestSoundNotFound = new DecentForms.Button();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnExportSettings
      // 
      this.btnExportSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnExportSettings.Location = new System.Drawing.Point(819, 93);
      this.btnExportSettings.Name = "btnExportSettings";
      this.btnExportSettings.Size = new System.Drawing.Size(75, 23);
      this.btnExportSettings.TabIndex = 12;
      this.btnExportSettings.Text = "Export here";
      this.btnExportSettings.Click += new DecentForms.EventHandler(this.btnExportSettings_Click);
      // 
      // btnImportSettings
      // 
      this.btnImportSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnImportSettings.Location = new System.Drawing.Point(738, 93);
      this.btnImportSettings.Name = "btnImportSettings";
      this.btnImportSettings.Size = new System.Drawing.Size(75, 23);
      this.btnImportSettings.TabIndex = 13;
      this.btnImportSettings.Text = "Import here";
      this.btnImportSettings.Click += new DecentForms.EventHandler(this.btnImportSettings_Click);
      // 
      // checkPlaySoundSearchTextNotFound
      // 
      this.checkPlaySoundSearchTextNotFound.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkPlaySoundSearchTextNotFound.Location = new System.Drawing.Point(32, 78);
      this.checkPlaySoundSearchTextNotFound.Name = "checkPlaySoundSearchTextNotFound";
      this.checkPlaySoundSearchTextNotFound.Size = new System.Drawing.Size(214, 17);
      this.checkPlaySoundSearchTextNotFound.TabIndex = 16;
      this.checkPlaySoundSearchTextNotFound.Text = "Search Text not found";
      this.checkPlaySoundSearchTextNotFound.UseVisualStyleBackColor = true;
      this.checkPlaySoundSearchTextNotFound.CheckedChanged += new System.EventHandler(this.checkPlaySoundSearchTextNotFound_CheckedChanged);
      // 
      // checkPlaySoundCompileSuccessful
      // 
      this.checkPlaySoundCompileSuccessful.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkPlaySoundCompileSuccessful.Location = new System.Drawing.Point(32, 55);
      this.checkPlaySoundCompileSuccessful.Name = "checkPlaySoundCompileSuccessful";
      this.checkPlaySoundCompileSuccessful.Size = new System.Drawing.Size(214, 17);
      this.checkPlaySoundCompileSuccessful.TabIndex = 15;
      this.checkPlaySoundCompileSuccessful.Text = "Build Successful";
      this.checkPlaySoundCompileSuccessful.UseVisualStyleBackColor = true;
      this.checkPlaySoundCompileSuccessful.CheckedChanged += new System.EventHandler(this.checkPlaySoundCompileSuccessful_CheckedChanged);
      // 
      // checkPlaySoundCompileFail
      // 
      this.checkPlaySoundCompileFail.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkPlaySoundCompileFail.Location = new System.Drawing.Point(32, 32);
      this.checkPlaySoundCompileFail.Name = "checkPlaySoundCompileFail";
      this.checkPlaySoundCompileFail.Size = new System.Drawing.Size(214, 17);
      this.checkPlaySoundCompileFail.TabIndex = 14;
      this.checkPlaySoundCompileFail.Text = "Build Failed";
      this.checkPlaySoundCompileFail.UseVisualStyleBackColor = true;
      this.checkPlaySoundCompileFail.CheckedChanged += new System.EventHandler(this.checkPlaySoundCompileFail_CheckedChanged);
      // 
      // label11
      // 
      this.label11.AutoSize = true;
      this.label11.Location = new System.Drawing.Point(17, 16);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(55, 13);
      this.label11.TabIndex = 17;
      this.label11.Text = "play when";
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.btnTestSoundNotFound);
      this.groupBox1.Controls.Add(this.btnTestSoundBuildSuccess);
      this.groupBox1.Controls.Add(this.btnTestSoundBuildFailure);
      this.groupBox1.Controls.Add(this.btnExportSettings);
      this.groupBox1.Controls.Add(this.checkPlaySoundSearchTextNotFound);
      this.groupBox1.Controls.Add(this.btnImportSettings);
      this.groupBox1.Controls.Add(this.checkPlaySoundCompileSuccessful);
      this.groupBox1.Controls.Add(this.label11);
      this.groupBox1.Controls.Add(this.checkPlaySoundCompileFail);
      this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBox1.Location = new System.Drawing.Point(0, 0);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(900, 122);
      this.groupBox1.TabIndex = 18;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Sounds";
      // 
      // btnTestSoundBuildFailure
      // 
      this.btnTestSoundBuildFailure.Location = new System.Drawing.Point(272, 28);
      this.btnTestSoundBuildFailure.Name = "btnTestSoundBuildFailure";
      this.btnTestSoundBuildFailure.Size = new System.Drawing.Size(75, 23);
      this.btnTestSoundBuildFailure.TabIndex = 18;
      this.btnTestSoundBuildFailure.Text = "Test";
      this.btnTestSoundBuildFailure.Click += new DecentForms.EventHandler(this.btnTestSoundBuildFailure_Click);
      // 
      // btnTestSoundBuildSuccess
      // 
      this.btnTestSoundBuildSuccess.Location = new System.Drawing.Point(272, 51);
      this.btnTestSoundBuildSuccess.Name = "btnTestSoundBuildSuccess";
      this.btnTestSoundBuildSuccess.Size = new System.Drawing.Size(75, 23);
      this.btnTestSoundBuildSuccess.TabIndex = 18;
      this.btnTestSoundBuildSuccess.Text = "Test";
      this.btnTestSoundBuildSuccess.Click += new DecentForms.EventHandler(this.btnTestSoundBuildSuccess_Click);
      // 
      // btnTestSoundNotFound
      // 
      this.btnTestSoundNotFound.Location = new System.Drawing.Point(272, 74);
      this.btnTestSoundNotFound.Name = "btnTestSoundNotFound";
      this.btnTestSoundNotFound.Size = new System.Drawing.Size(75, 23);
      this.btnTestSoundNotFound.TabIndex = 18;
      this.btnTestSoundNotFound.Text = "Test";
      this.btnTestSoundNotFound.Click += new DecentForms.EventHandler(this.btnTestSoundNotFound_Click);
      // 
      // PrefSounds
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBox1);
      this.Name = "PrefSounds";
      this.Size = new System.Drawing.Size(900, 122);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);

    }

        #endregion

        private DecentForms.Button btnExportSettings;
        private DecentForms.Button btnImportSettings;
        private System.Windows.Forms.CheckBox checkPlaySoundSearchTextNotFound;
        private System.Windows.Forms.CheckBox checkPlaySoundCompileSuccessful;
        private System.Windows.Forms.CheckBox checkPlaySoundCompileFail;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox groupBox1;
    private DecentForms.Button btnTestSoundNotFound;
    private DecentForms.Button btnTestSoundBuildSuccess;
    private DecentForms.Button btnTestSoundBuildFailure;
  }
}
