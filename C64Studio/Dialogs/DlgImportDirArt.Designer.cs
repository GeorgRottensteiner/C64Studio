namespace C64Studio
{
  partial class DlgImportDirArt
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
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnOK = new System.Windows.Forms.Button();
      this.labelASMInfo = new System.Windows.Forms.Label();
      this.editASMDirArt = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // btnCancel
      // 
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(519, 366);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      // 
      // btnOK
      // 
      this.btnOK.Location = new System.Drawing.Point(438, 366);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 1;
      this.btnOK.Text = "OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // labelASMInfo
      // 
      this.labelASMInfo.Location = new System.Drawing.Point(12, 366);
      this.labelASMInfo.Name = "labelASMInfo";
      this.labelASMInfo.Size = new System.Drawing.Size(399, 23);
      this.labelASMInfo.TabIndex = 3;
      this.labelASMInfo.Text = "label1";
      // 
      // editASMDirArt
      // 
      this.editASMDirArt.Location = new System.Drawing.Point(12, 96);
      this.editASMDirArt.Multiline = true;
      this.editASMDirArt.Name = "editASMDirArt";
      this.editASMDirArt.Size = new System.Drawing.Size(579, 264);
      this.editASMDirArt.TabIndex = 4;
      this.editASMDirArt.TextChanged += new System.EventHandler(this.editASMDirArt_TextChanged);
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point(9, 9);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(582, 74);
      this.label1.TabIndex = 5;
      this.label1.Text = "Insert Assembler statements (!byte ...) with screencodes to convert to file name " +
    "entries. File names are expected to be 16 characters in length. Screencode is co" +
    "nverted to PETSCII if possible.";
      // 
      // DlgImportDirArt
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(603, 396);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.editASMDirArt);
      this.Controls.Add(this.labelASMInfo);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.btnCancel);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "DlgImportDirArt";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = "Import Dir Art";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Label labelASMInfo;
    private System.Windows.Forms.TextBox editASMDirArt;
    private System.Windows.Forms.Label label1;
  }
}