namespace RetroDevStudio.Dialogs
{
  partial class FormAdvanceToLine
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.labelAddress = new System.Windows.Forms.Label();
      this.editLineNo = new System.Windows.Forms.TextBox();
      this.btnCancel = new DecentForms.Button();
      this.btnOK = new DecentForms.Button();
      this.SuspendLayout();
      // 
      // labelAddress
      // 
      this.labelAddress.AutoSize = true;
      this.labelAddress.Location = new System.Drawing.Point(5, 15);
      this.labelAddress.Name = "labelAddress";
      this.labelAddress.Size = new System.Drawing.Size(30, 13);
      this.labelAddress.TabIndex = 1;
      this.labelAddress.Text = "Line:";
      // 
      // editLineNo
      // 
      this.editLineNo.Location = new System.Drawing.Point(41, 12);
      this.editLineNo.Name = "editLineNo";
      this.editLineNo.Size = new System.Drawing.Size(193, 20);
      this.editLineNo.TabIndex = 0;
      this.editLineNo.TextChanged += new System.EventHandler(this.editAddress_TextChanged);
      // 
      // btnCancel
      // 
      this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnCancel.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.DisplayAntiAliased = true;
      this.btnCancel.Image = null;
      this.btnCancel.Location = new System.Drawing.Point(158, 45);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.Click += new DecentForms.EventHandler(this.btnCancel_Click);
      // 
      // btnOK
      // 
      this.btnOK.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
      this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnOK.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnOK.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOK.DisplayAntiAliased = true;
      this.btnOK.Image = null;
      this.btnOK.Location = new System.Drawing.Point(77, 45);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 4;
      this.btnOK.Text = "OK";
      this.btnOK.Click += new DecentForms.EventHandler(this.btnOK_Click);
      // 
      // FormAdvanceToLine
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(245, 80);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.editLineNo);
      this.Controls.Add(this.labelAddress);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormAdvanceToLine";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Advance to Raster Line";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
    private System.Windows.Forms.Label labelAddress;
    private DecentForms.Button btnCancel;
    private DecentForms.Button btnOK;
    private System.Windows.Forms.TextBox editLineNo;
  }
}