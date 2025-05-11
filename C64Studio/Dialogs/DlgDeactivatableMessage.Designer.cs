namespace RetroDevStudio.Dialogs
{
  partial class DlgDeactivatableMessage
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
      this.labelImageInfo = new System.Windows.Forms.Label();
      this.btnYes = new DecentForms.Button();
      this.btnNo = new DecentForms.Button();
      this.btnCancel = new DecentForms.Button();
      this.checkRememberDecision = new System.Windows.Forms.CheckBox();
      this.SuspendLayout();
      // 
      // labelImageInfo
      // 
      this.labelImageInfo.Location = new System.Drawing.Point(12, 9);
      this.labelImageInfo.Name = "labelImageInfo";
      this.labelImageInfo.Size = new System.Drawing.Size(411, 75);
      this.labelImageInfo.TabIndex = 0;
      this.labelImageInfo.Text = "The imported image is x,y, the current screen size is x2,y2.\\r\\nShould the image " +
    "be clipped or the screen size be adjusted?";
      this.labelImageInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // btnYes
      // 
      this.btnYes.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnYes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnYes.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnYes.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnYes.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnYes.Image = null;
      this.btnYes.Location = new System.Drawing.Point(14, 139);
      this.btnYes.Name = "btnYes";
      this.btnYes.Size = new System.Drawing.Size(127, 23);
      this.btnYes.TabIndex = 1;
      this.btnYes.Text = "Yes";
      this.btnYes.Click += new DecentForms.EventHandler(this.btn_Click);
      // 
      // btnNo
      // 
      this.btnNo.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnNo.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnNo.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnNo.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnNo.Image = null;
      this.btnNo.Location = new System.Drawing.Point(154, 139);
      this.btnNo.Name = "btnNo";
      this.btnNo.Size = new System.Drawing.Size(127, 23);
      this.btnNo.TabIndex = 1;
      this.btnNo.Text = "No";
      this.btnNo.Click += new DecentForms.EventHandler(this.btn_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnCancel.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnCancel.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Image = null;
      this.btnCancel.Location = new System.Drawing.Point(294, 139);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(127, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.Click += new DecentForms.EventHandler(this.btn_Click);
      // 
      // checkRememberDecision
      // 
      this.checkRememberDecision.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.checkRememberDecision.AutoSize = true;
      this.checkRememberDecision.Location = new System.Drawing.Point(16, 107);
      this.checkRememberDecision.Name = "checkRememberDecision";
      this.checkRememberDecision.Size = new System.Drawing.Size(135, 17);
      this.checkRememberDecision.TabIndex = 2;
      this.checkRememberDecision.Text = "Remember my decision";
      this.checkRememberDecision.UseVisualStyleBackColor = true;
      // 
      // DlgDeactivatableMessage
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(435, 176);
      this.Controls.Add(this.checkRememberDecision);
      this.Controls.Add(this.btnNo);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnYes);
      this.Controls.Add(this.labelImageInfo);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "DlgDeactivatableMessage";
      this.Padding = new System.Windows.Forms.Padding(9);
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Caption";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DlgDeactivatableMessage_FormClosing);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label labelImageInfo;
    private DecentForms.Button btnYes;
    private DecentForms.Button btnNo;
    private DecentForms.Button btnCancel;
    private System.Windows.Forms.CheckBox checkRememberDecision;
  }
}
