namespace RetroDevStudio.Dialogs.Preferences
{
  partial class PrefSourceControl
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
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.checkGenerateSolutionRepository = new System.Windows.Forms.CheckBox();
      this.editCommitterEmail = new System.Windows.Forms.TextBox();
      this.editCommitAuthor = new System.Windows.Forms.TextBox();
      this.label32 = new System.Windows.Forms.Label();
      this.label31 = new System.Windows.Forms.Label();
      this.checkGenerateProjectRepository = new System.Windows.Forms.CheckBox();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnExportSettings
      // 
      this.btnExportSettings.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnExportSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnExportSettings.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnExportSettings.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnExportSettings.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnExportSettings.Image = null;
      this.btnExportSettings.Location = new System.Drawing.Point(819, 100);
      this.btnExportSettings.Name = "btnExportSettings";
      this.btnExportSettings.Size = new System.Drawing.Size(75, 23);
      this.btnExportSettings.TabIndex = 12;
      this.btnExportSettings.Text = "Export here";
      this.btnExportSettings.Click += new DecentForms.EventHandler(this.btnExportSettings_Click);
      // 
      // btnImportSettings
      // 
      this.btnImportSettings.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnImportSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnImportSettings.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnImportSettings.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnImportSettings.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnImportSettings.Image = null;
      this.btnImportSettings.Location = new System.Drawing.Point(738, 100);
      this.btnImportSettings.Name = "btnImportSettings";
      this.btnImportSettings.Size = new System.Drawing.Size(75, 23);
      this.btnImportSettings.TabIndex = 13;
      this.btnImportSettings.Text = "Import here";
      this.btnImportSettings.Click += new DecentForms.EventHandler(this.btnImportSettings_Click);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.checkGenerateProjectRepository);
      this.groupBox1.Controls.Add(this.checkGenerateSolutionRepository);
      this.groupBox1.Controls.Add(this.editCommitterEmail);
      this.groupBox1.Controls.Add(this.editCommitAuthor);
      this.groupBox1.Controls.Add(this.label32);
      this.groupBox1.Controls.Add(this.label31);
      this.groupBox1.Controls.Add(this.btnExportSettings);
      this.groupBox1.Controls.Add(this.btnImportSettings);
      this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBox1.Location = new System.Drawing.Point(0, 0);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(900, 129);
      this.groupBox1.TabIndex = 16;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Source Control";
      // 
      // checkGenerateSolutionRepository
      // 
      this.checkGenerateSolutionRepository.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkGenerateSolutionRepository.Location = new System.Drawing.Point(7, 74);
      this.checkGenerateSolutionRepository.Name = "checkGenerateSolutionRepository";
      this.checkGenerateSolutionRepository.Size = new System.Drawing.Size(232, 17);
      this.checkGenerateSolutionRepository.TabIndex = 22;
      this.checkGenerateSolutionRepository.Text = "Generate Solution Repository";
      this.checkGenerateSolutionRepository.UseVisualStyleBackColor = true;
      this.checkGenerateSolutionRepository.CheckedChanged += new System.EventHandler(this.checkGenerateSolutionRepository_CheckedChanged);
      // 
      // editCommitterEmail
      // 
      this.editCommitterEmail.Location = new System.Drawing.Point(225, 45);
      this.editCommitterEmail.Name = "editCommitterEmail";
      this.editCommitterEmail.Size = new System.Drawing.Size(322, 20);
      this.editCommitterEmail.TabIndex = 17;
      this.editCommitterEmail.TextChanged += new System.EventHandler(this.editCommitterEmail_TextChanged);
      // 
      // editCommitAuthor
      // 
      this.editCommitAuthor.Location = new System.Drawing.Point(225, 19);
      this.editCommitAuthor.MaxLength = 2;
      this.editCommitAuthor.Name = "editCommitAuthor";
      this.editCommitAuthor.Size = new System.Drawing.Size(322, 20);
      this.editCommitAuthor.TabIndex = 21;
      this.editCommitAuthor.TextChanged += new System.EventHandler(this.editCommitAuthor_TextChanged);
      // 
      // label32
      // 
      this.label32.AutoSize = true;
      this.label32.Location = new System.Drawing.Point(9, 22);
      this.label32.Name = "label32";
      this.label32.Size = new System.Drawing.Size(78, 13);
      this.label32.TabIndex = 19;
      this.label32.Text = "Commit Author:";
      // 
      // label31
      // 
      this.label31.AutoSize = true;
      this.label31.Location = new System.Drawing.Point(9, 48);
      this.label31.Name = "label31";
      this.label31.Size = new System.Drawing.Size(84, 13);
      this.label31.TabIndex = 20;
      this.label31.Text = "Committer Email:";
      // 
      // checkGenerateProjectRepository
      // 
      this.checkGenerateProjectRepository.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkGenerateProjectRepository.Location = new System.Drawing.Point(7, 100);
      this.checkGenerateProjectRepository.Name = "checkGenerateProjectRepository";
      this.checkGenerateProjectRepository.Size = new System.Drawing.Size(232, 17);
      this.checkGenerateProjectRepository.TabIndex = 22;
      this.checkGenerateProjectRepository.Text = "Generate Project Repository";
      this.checkGenerateProjectRepository.UseVisualStyleBackColor = true;
      this.checkGenerateProjectRepository.CheckedChanged += new System.EventHandler(this.checkGenerateProjectRepository_CheckedChanged);
      // 
      // PrefSourceControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.groupBox1);
      this.Name = "PrefSourceControl";
      this.Size = new System.Drawing.Size(900, 129);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);

    }

        #endregion

        private DecentForms.Button btnExportSettings;
        private DecentForms.Button btnImportSettings;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.TextBox editCommitterEmail;
    private System.Windows.Forms.TextBox editCommitAuthor;
    private System.Windows.Forms.Label label32;
    private System.Windows.Forms.Label label31;
    private System.Windows.Forms.CheckBox checkGenerateSolutionRepository;
    private System.Windows.Forms.CheckBox checkGenerateProjectRepository;
  }
}
