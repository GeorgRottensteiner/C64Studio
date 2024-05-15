namespace RetroDevStudio.Dialogs
{
  partial class FormDeltaValue
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
      this.editAddress = new System.Windows.Forms.TextBox();
      this.btnCancel = new DecentForms.Button();
      this.btnOK = new DecentForms.Button();
      this.chkHex = new System.Windows.Forms.CheckBox();
      this.SuspendLayout();
      // 
      // labelAddress
      // 
      this.labelAddress.AutoSize = true;
      this.labelAddress.Location = new System.Drawing.Point(5, 15);
      this.labelAddress.Name = "labelAddress";
      this.labelAddress.Size = new System.Drawing.Size(58, 13);
      this.labelAddress.TabIndex = 1;
      this.labelAddress.Text = "Delta (+/-):";
      // 
      // editAddress
      // 
      this.editAddress.Location = new System.Drawing.Point(76, 12);
      this.editAddress.Name = "editAddress";
      this.editAddress.Size = new System.Drawing.Size(158, 20);
      this.editAddress.TabIndex = 0;
      this.editAddress.TextChanged += new System.EventHandler(this.editAddress_TextChanged);
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnCancel.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Image = null;
      this.btnCancel.Location = new System.Drawing.Point(175, 72);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 5;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.Click += new DecentForms.EventHandler(this.btnCancel_Click);
      // 
      // btnOK
      // 
      this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnOK.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnOK.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOK.Image = null;
      this.btnOK.Location = new System.Drawing.Point(94, 72);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 4;
      this.btnOK.Text = "OK";
      this.btnOK.Click += new DecentForms.EventHandler(this.btnOK_Click);
      // 
      // chkHex
      // 
      this.chkHex.AutoSize = true;
      this.chkHex.Checked = true;
      this.chkHex.CheckState = System.Windows.Forms.CheckState.Checked;
      this.chkHex.Location = new System.Drawing.Point(78, 46);
      this.chkHex.Name = "chkHex";
      this.chkHex.Size = new System.Drawing.Size(130, 17);
      this.chkHex.TabIndex = 6;
      this.chkHex.Text = "Insert as Hexadecimal";
      this.chkHex.UseVisualStyleBackColor = true;
      this.chkHex.CheckedChanged += new System.EventHandler(this.chkHex_CheckedChanged);
      // 
      // FormDeltaValue
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(262, 107);
      this.Controls.Add(this.chkHex);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.editAddress);
      this.Controls.Add(this.labelAddress);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormDeltaValue";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Delta Value";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
    private System.Windows.Forms.Label labelAddress;
    private DecentForms.Button btnCancel;
    private DecentForms.Button btnOK;
    public System.Windows.Forms.TextBox editAddress;
    public System.Windows.Forms.CheckBox chkHex;
  }
}