namespace RetroDevStudio.Dialogs
{
  partial class FormProjectWizard
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
      this.label1 = new System.Windows.Forms.Label();
      this.labelACMEPath = new System.Windows.Forms.Label();
      this.editBasePath = new System.Windows.Forms.TextBox();
      this.btnBrowseBasePath = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnOK = new System.Windows.Forms.Button();
      this.label2 = new System.Windows.Forms.Label();
      this.editProjectName = new System.Windows.Forms.TextBox();
      this.labelProjectSummary = new System.Windows.Forms.Label();
      this.checkCreateRepository = new System.Windows.Forms.CheckBox();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point(13, 13);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(520, 38);
      this.label1.TabIndex = 0;
      this.label1.Text = "This wizard walks you through setting up a new project\r\n\r\n";
      // 
      // labelACMEPath
      // 
      this.labelACMEPath.AutoSize = true;
      this.labelACMEPath.Location = new System.Drawing.Point(13, 83);
      this.labelACMEPath.Name = "labelACMEPath";
      this.labelACMEPath.Size = new System.Drawing.Size(102, 13);
      this.labelACMEPath.TabIndex = 1;
      this.labelACMEPath.Text = "Project Base Folder:";
      // 
      // editBasePath
      // 
      this.editBasePath.Location = new System.Drawing.Point(121, 80);
      this.editBasePath.Name = "editBasePath";
      this.editBasePath.Size = new System.Drawing.Size(339, 20);
      this.editBasePath.TabIndex = 0;
      this.editBasePath.TextChanged += new System.EventHandler(this.editBasePath_TextChanged);
      // 
      // btnBrowseBasePath
      // 
      this.btnBrowseBasePath.Location = new System.Drawing.Point(466, 78);
      this.btnBrowseBasePath.Name = "btnBrowseBasePath";
      this.btnBrowseBasePath.Size = new System.Drawing.Size(67, 23);
      this.btnBrowseBasePath.TabIndex = 1;
      this.btnBrowseBasePath.Text = "...";
      this.btnBrowseBasePath.UseVisualStyleBackColor = true;
      this.btnBrowseBasePath.Click += new System.EventHandler(this.btnBrowseBasePath_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(458, 216);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // btnOK
      // 
      this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnOK.Location = new System.Drawing.Point(377, 216);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 4;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(13, 57);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(74, 13);
      this.label2.TabIndex = 1;
      this.label2.Text = "Project Name:";
      // 
      // editProjectName
      // 
      this.editProjectName.Location = new System.Drawing.Point(121, 54);
      this.editProjectName.Name = "editProjectName";
      this.editProjectName.Size = new System.Drawing.Size(339, 20);
      this.editProjectName.TabIndex = 0;
      this.editProjectName.Text = "New Project";
      this.editProjectName.TextChanged += new System.EventHandler(this.editProjectName_TextChanged);
      // 
      // labelProjectSummary
      // 
      this.labelProjectSummary.Location = new System.Drawing.Point(12, 153);
      this.labelProjectSummary.Name = "labelProjectSummary";
      this.labelProjectSummary.Size = new System.Drawing.Size(520, 60);
      this.labelProjectSummary.TabIndex = 0;
      this.labelProjectSummary.Text = "summary";
      // 
      // checkCreateRepository
      // 
      this.checkCreateRepository.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
      this.checkCreateRepository.Location = new System.Drawing.Point(12, 115);
      this.checkCreateRepository.Name = "checkCreateRepository";
      this.checkCreateRepository.Size = new System.Drawing.Size(123, 24);
      this.checkCreateRepository.TabIndex = 6;
      this.checkCreateRepository.Text = "Create Repository";
      this.checkCreateRepository.UseVisualStyleBackColor = true;
      // 
      // FormProjectWizard
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(545, 251);
      this.Controls.Add(this.checkCreateRepository);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnBrowseBasePath);
      this.Controls.Add(this.editProjectName);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.editBasePath);
      this.Controls.Add(this.labelACMEPath);
      this.Controls.Add(this.labelProjectSummary);
      this.Controls.Add(this.label1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormProjectWizard";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "New Project Wizard";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label labelACMEPath;
    private System.Windows.Forms.Button btnBrowseBasePath;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnOK;
    public System.Windows.Forms.TextBox editBasePath;
    private System.Windows.Forms.Label label2;
    public System.Windows.Forms.TextBox editProjectName;
    private System.Windows.Forms.Label labelProjectSummary;
    private System.Windows.Forms.CheckBox checkCreateRepository;
  }
}