﻿namespace RetroDevStudio.Dialogs.Preferences
{
  partial class DlgPrefSounds
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
      this.checkPlaySoundSearchTextNotFound = new System.Windows.Forms.CheckBox();
      this.checkPlaySoundCompileSuccessful = new System.Windows.Forms.CheckBox();
      this.checkPlaySoundCompileFail = new System.Windows.Forms.CheckBox();
      this.label11 = new System.Windows.Forms.Label();
      this.btnTestSoundNotFound = new DecentForms.Button();
      this.btnTestSoundBuildSuccess = new DecentForms.Button();
      this.btnTestSoundBuildFailure = new DecentForms.Button();
      this.SuspendLayout();
      // 
      // checkPlaySoundSearchTextNotFound
      // 
      this.checkPlaySoundSearchTextNotFound.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkPlaySoundSearchTextNotFound.Location = new System.Drawing.Point(18, 67);
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
      this.checkPlaySoundCompileSuccessful.Location = new System.Drawing.Point(18, 44);
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
      this.checkPlaySoundCompileFail.Location = new System.Drawing.Point(18, 21);
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
      this.label11.Location = new System.Drawing.Point(3, 4);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(55, 13);
      this.label11.TabIndex = 17;
      this.label11.Text = "play when";
      // 
      // btnTestSoundNotFound
      // 
      this.btnTestSoundNotFound.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnTestSoundNotFound.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnTestSoundNotFound.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnTestSoundNotFound.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnTestSoundNotFound.Image = null;
      this.btnTestSoundNotFound.Location = new System.Drawing.Point(258, 64);
      this.btnTestSoundNotFound.Name = "btnTestSoundNotFound";
      this.btnTestSoundNotFound.Size = new System.Drawing.Size(75, 20);
      this.btnTestSoundNotFound.TabIndex = 18;
      this.btnTestSoundNotFound.Text = "Test";
      this.btnTestSoundNotFound.Click += new DecentForms.EventHandler(this.btnTestSoundNotFound_Click);
      // 
      // btnTestSoundBuildSuccess
      // 
      this.btnTestSoundBuildSuccess.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnTestSoundBuildSuccess.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnTestSoundBuildSuccess.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnTestSoundBuildSuccess.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnTestSoundBuildSuccess.Image = null;
      this.btnTestSoundBuildSuccess.Location = new System.Drawing.Point(258, 41);
      this.btnTestSoundBuildSuccess.Name = "btnTestSoundBuildSuccess";
      this.btnTestSoundBuildSuccess.Size = new System.Drawing.Size(75, 20);
      this.btnTestSoundBuildSuccess.TabIndex = 18;
      this.btnTestSoundBuildSuccess.Text = "Test";
      this.btnTestSoundBuildSuccess.Click += new DecentForms.EventHandler(this.btnTestSoundBuildSuccess_Click);
      // 
      // btnTestSoundBuildFailure
      // 
      this.btnTestSoundBuildFailure.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnTestSoundBuildFailure.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnTestSoundBuildFailure.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnTestSoundBuildFailure.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnTestSoundBuildFailure.Image = null;
      this.btnTestSoundBuildFailure.Location = new System.Drawing.Point(258, 18);
      this.btnTestSoundBuildFailure.Name = "btnTestSoundBuildFailure";
      this.btnTestSoundBuildFailure.Size = new System.Drawing.Size(75, 20);
      this.btnTestSoundBuildFailure.TabIndex = 18;
      this.btnTestSoundBuildFailure.Text = "Test";
      this.btnTestSoundBuildFailure.Click += new DecentForms.EventHandler(this.btnTestSoundBuildFailure_Click);
      // 
      // PrefSounds
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.btnTestSoundNotFound);
      this.Controls.Add(this.btnTestSoundBuildSuccess);
      this.Controls.Add(this.checkPlaySoundCompileFail);
      this.Controls.Add(this.btnTestSoundBuildFailure);
      this.Controls.Add(this.label11);
      this.Controls.Add(this.checkPlaySoundSearchTextNotFound);
      this.Controls.Add(this.checkPlaySoundCompileSuccessful);
      this.Name = "PrefSounds";
      this.Size = new System.Drawing.Size(370, 119);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

        #endregion
        private System.Windows.Forms.CheckBox checkPlaySoundSearchTextNotFound;
        private System.Windows.Forms.CheckBox checkPlaySoundCompileSuccessful;
        private System.Windows.Forms.CheckBox checkPlaySoundCompileFail;
        private System.Windows.Forms.Label label11;
    private DecentForms.Button btnTestSoundNotFound;
    private DecentForms.Button btnTestSoundBuildSuccess;
    private DecentForms.Button btnTestSoundBuildFailure;
  }
}
