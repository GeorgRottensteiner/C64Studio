namespace RetroDevStudio.Dialogs
{
  partial class DlgImportBASICTextAdjustment
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
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
      this.labelIssueInfo = new System.Windows.Forms.Label();
      this.btnCancel = new DecentForms.Button();
      this.checkAdjustCasing = new DecentForms.CheckBox();
      this.checkSkipInvalidCharacters = new DecentForms.CheckBox();
      this.checkReplaceInvalidCharacters = new DecentForms.CheckBox();
      this.btnOK = new DecentForms.Button();
      this.SuspendLayout();
      // 
      // labelIssueInfo
      // 
      this.labelIssueInfo.Location = new System.Drawing.Point(12, 9);
      this.labelIssueInfo.Name = "labelIssueInfo";
      this.labelIssueInfo.Size = new System.Drawing.Size(411, 57);
      this.labelIssueInfo.TabIndex = 0;
      this.labelIssueInfo.Text = "The imported image is x,y, the current screen size is x2,y2.\\r\\nShould the image " +
    "be clipped or the screen size be adjusted?";
      this.labelIssueInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // btnCancel
      // 
      this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnCancel.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnCancel.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Image = null;
      this.btnCancel.Location = new System.Drawing.Point(296, 170);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(127, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Cancel Paste";
      this.btnCancel.Click += new DecentForms.EventHandler(this.btnCancel_Click);
      // 
      // checkAdjustCasing
      // 
      this.checkAdjustCasing.Appearance = System.Windows.Forms.Appearance.Normal;
      this.checkAdjustCasing.BorderStyle = DecentForms.BorderStyle.NONE;
      this.checkAdjustCasing.CheckAlign = DecentForms.ContentAlignment.MiddleLeft;
      this.checkAdjustCasing.Checked = false;
      this.checkAdjustCasing.Image = null;
      this.checkAdjustCasing.Location = new System.Drawing.Point(102, 69);
      this.checkAdjustCasing.Name = "checkAdjustCasing";
      this.checkAdjustCasing.Size = new System.Drawing.Size(230, 23);
      this.checkAdjustCasing.TabIndex = 2;
      this.checkAdjustCasing.Text = "Automatic case adjustment";
      // 
      // checkSkipInvalidCharacters
      // 
      this.checkSkipInvalidCharacters.Appearance = System.Windows.Forms.Appearance.Normal;
      this.checkSkipInvalidCharacters.BorderStyle = DecentForms.BorderStyle.NONE;
      this.checkSkipInvalidCharacters.CheckAlign = DecentForms.ContentAlignment.MiddleLeft;
      this.checkSkipInvalidCharacters.Checked = false;
      this.checkSkipInvalidCharacters.Image = null;
      this.checkSkipInvalidCharacters.Location = new System.Drawing.Point(102, 98);
      this.checkSkipInvalidCharacters.Name = "checkSkipInvalidCharacters";
      this.checkSkipInvalidCharacters.Size = new System.Drawing.Size(230, 23);
      this.checkSkipInvalidCharacters.TabIndex = 2;
      this.checkSkipInvalidCharacters.Text = "Skip invalid characters";
      // 
      // checkReplaceInvalidCharacters
      // 
      this.checkReplaceInvalidCharacters.Appearance = System.Windows.Forms.Appearance.Normal;
      this.checkReplaceInvalidCharacters.BorderStyle = DecentForms.BorderStyle.NONE;
      this.checkReplaceInvalidCharacters.CheckAlign = DecentForms.ContentAlignment.MiddleLeft;
      this.checkReplaceInvalidCharacters.Checked = false;
      this.checkReplaceInvalidCharacters.Image = null;
      this.checkReplaceInvalidCharacters.Location = new System.Drawing.Point(102, 127);
      this.checkReplaceInvalidCharacters.Name = "checkReplaceInvalidCharacters";
      this.checkReplaceInvalidCharacters.Size = new System.Drawing.Size(230, 23);
      this.checkReplaceInvalidCharacters.TabIndex = 2;
      this.checkReplaceInvalidCharacters.Text = "Replace invalid characters (with ?)";
      // 
      // btnOK
      // 
      this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnOK.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnOK.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnOK.Image = null;
      this.btnOK.Location = new System.Drawing.Point(163, 170);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(127, 23);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "OK";
      this.btnOK.Click += new DecentForms.EventHandler(this.btnOK_Click);
      // 
      // DlgImportBASICTextAdjustment
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(435, 205);
      this.Controls.Add(this.checkReplaceInvalidCharacters);
      this.Controls.Add(this.checkSkipInvalidCharacters);
      this.Controls.Add(this.checkAdjustCasing);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.labelIssueInfo);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.KeyPreview = true;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "DlgImportBASICTextAdjustment";
      this.Padding = new System.Windows.Forms.Padding(9);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Paste issues detected";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label labelIssueInfo;
    private DecentForms.Button btnCancel;
    private DecentForms.CheckBox checkAdjustCasing;
    private DecentForms.CheckBox checkSkipInvalidCharacters;
    private DecentForms.CheckBox checkReplaceInvalidCharacters;
    private DecentForms.Button btnOK;
  }
}
