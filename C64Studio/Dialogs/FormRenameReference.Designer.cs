using RetroDevStudio.Controls;



namespace RetroDevStudio.Dialogs
{
  partial class FormRenameReference
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
      this.btnCancel = new DecentForms.Button();
      this.btnOK = new DecentForms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.editReferenceName = new System.Windows.Forms.TextBox();
      this.labelRenameInfo = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // btnCancel
      // 
      this.btnCancel.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnCancel.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Image = null;
      this.btnCancel.Location = new System.Drawing.Point(418, 116);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.Click += new DecentForms.EventHandler(this.btnCancel_Click);
      // 
      // btnOK
      // 
      this.btnOK.BorderStyle = DecentForms.BorderStyle.FLAT;
      this.btnOK.ButtonBorder = DecentForms.Button.ButtonStyle.RAISED;
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOK.Enabled = false;
      this.btnOK.Image = null;
      this.btnOK.Location = new System.Drawing.Point(337, 116);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "OK";
      this.btnOK.Click += new DecentForms.EventHandler(this.btnOK_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 12);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(115, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Rename Reference to:";
      // 
      // editReferenceName
      // 
      this.editReferenceName.Location = new System.Drawing.Point(133, 9);
      this.editReferenceName.Name = "editReferenceName";
      this.editReferenceName.Size = new System.Drawing.Size(360, 20);
      this.editReferenceName.TabIndex = 3;
      this.editReferenceName.TextChanged += new System.EventHandler(this.editSolutionName_TextChanged);
      // 
      // labelRenameInfo
      // 
      this.labelRenameInfo.Location = new System.Drawing.Point(13, 44);
      this.labelRenameInfo.Name = "labelRenameInfo";
      this.labelRenameInfo.Size = new System.Drawing.Size(480, 69);
      this.labelRenameInfo.TabIndex = 4;
      this.labelRenameInfo.Text = "label2";
      // 
      // FormRenameReference
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(505, 153);
      this.Controls.Add(this.labelRenameInfo);
      this.Controls.Add(this.editReferenceName);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.btnCancel);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FormRenameReference";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Rename Reference";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
    private DecentForms.Button btnCancel;
    private DecentForms.Button btnOK;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox editReferenceName;
    private System.Windows.Forms.Label labelRenameInfo;
  }
}