namespace RetroDevStudio.Dialogs.Preferences
{
  partial class DlgPrefSourceControl
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
      this.checkGenerateProjectRepository = new System.Windows.Forms.CheckBox();
      this.checkGenerateSolutionRepository = new System.Windows.Forms.CheckBox();
      this.editCommitterEmail = new System.Windows.Forms.TextBox();
      this.editCommitAuthor = new System.Windows.Forms.TextBox();
      this.label32 = new System.Windows.Forms.Label();
      this.label31 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // checkGenerateProjectRepository
      // 
      this.checkGenerateProjectRepository.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkGenerateProjectRepository.Location = new System.Drawing.Point(3, 91);
      this.checkGenerateProjectRepository.Name = "checkGenerateProjectRepository";
      this.checkGenerateProjectRepository.Size = new System.Drawing.Size(232, 17);
      this.checkGenerateProjectRepository.TabIndex = 22;
      this.checkGenerateProjectRepository.Text = "Generate Project Repository";
      this.checkGenerateProjectRepository.UseVisualStyleBackColor = true;
      this.checkGenerateProjectRepository.CheckedChanged += new System.EventHandler(this.checkGenerateProjectRepository_CheckedChanged);
      // 
      // checkGenerateSolutionRepository
      // 
      this.checkGenerateSolutionRepository.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkGenerateSolutionRepository.Location = new System.Drawing.Point(3, 65);
      this.checkGenerateSolutionRepository.Name = "checkGenerateSolutionRepository";
      this.checkGenerateSolutionRepository.Size = new System.Drawing.Size(232, 17);
      this.checkGenerateSolutionRepository.TabIndex = 22;
      this.checkGenerateSolutionRepository.Text = "Generate Solution Repository";
      this.checkGenerateSolutionRepository.UseVisualStyleBackColor = true;
      this.checkGenerateSolutionRepository.CheckedChanged += new System.EventHandler(this.checkGenerateSolutionRepository_CheckedChanged);
      // 
      // editCommitterEmail
      // 
      this.editCommitterEmail.Location = new System.Drawing.Point(221, 36);
      this.editCommitterEmail.Name = "editCommitterEmail";
      this.editCommitterEmail.Size = new System.Drawing.Size(322, 20);
      this.editCommitterEmail.TabIndex = 17;
      this.editCommitterEmail.TextChanged += new System.EventHandler(this.editCommitterEmail_TextChanged);
      // 
      // editCommitAuthor
      // 
      this.editCommitAuthor.Location = new System.Drawing.Point(221, 10);
      this.editCommitAuthor.Name = "editCommitAuthor";
      this.editCommitAuthor.Size = new System.Drawing.Size(322, 20);
      this.editCommitAuthor.TabIndex = 21;
      this.editCommitAuthor.TextChanged += new System.EventHandler(this.editCommitAuthor_TextChanged);
      // 
      // label32
      // 
      this.label32.AutoSize = true;
      this.label32.Location = new System.Drawing.Point(5, 13);
      this.label32.Name = "label32";
      this.label32.Size = new System.Drawing.Size(78, 13);
      this.label32.TabIndex = 19;
      this.label32.Text = "Commit Author:";
      // 
      // label31
      // 
      this.label31.AutoSize = true;
      this.label31.Location = new System.Drawing.Point(5, 39);
      this.label31.Name = "label31";
      this.label31.Size = new System.Drawing.Size(84, 13);
      this.label31.TabIndex = 20;
      this.label31.Text = "Committer Email:";
      // 
      // DlgPrefSourceControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.checkGenerateProjectRepository);
      this.Controls.Add(this.checkGenerateSolutionRepository);
      this.Controls.Add(this.editCommitterEmail);
      this.Controls.Add(this.label31);
      this.Controls.Add(this.editCommitAuthor);
      this.Controls.Add(this.label32);
      this.Name = "DlgPrefSourceControl";
      this.Size = new System.Drawing.Size(577, 174);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

        #endregion
    private System.Windows.Forms.TextBox editCommitterEmail;
    private System.Windows.Forms.TextBox editCommitAuthor;
    private System.Windows.Forms.Label label32;
    private System.Windows.Forms.Label label31;
    private System.Windows.Forms.CheckBox checkGenerateSolutionRepository;
    private System.Windows.Forms.CheckBox checkGenerateProjectRepository;
  }
}
