
namespace RetroDevStudio.Controls
{
  partial class ExportMapCharsetAsBinaryFile
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
      this.editPrefixLoadAddress = new System.Windows.Forms.TextBox();
      this.checkPrefixLoadAddress = new System.Windows.Forms.CheckBox();
      this.SuspendLayout();
      // 
      // editPrefixLoadAddress
      // 
      this.editPrefixLoadAddress.Enabled = false;
      this.editPrefixLoadAddress.Location = new System.Drawing.Point(155, 1);
      this.editPrefixLoadAddress.MaxLength = 4;
      this.editPrefixLoadAddress.Name = "editPrefixLoadAddress";
      this.editPrefixLoadAddress.Size = new System.Drawing.Size(66, 20);
      this.editPrefixLoadAddress.TabIndex = 1;
      this.editPrefixLoadAddress.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.editPrefixLoadAddress_KeyPress);
      // 
      // checkPrefixLoadAddress
      // 
      this.checkPrefixLoadAddress.AutoSize = true;
      this.checkPrefixLoadAddress.Location = new System.Drawing.Point(3, 3);
      this.checkPrefixLoadAddress.Name = "checkPrefixLoadAddress";
      this.checkPrefixLoadAddress.Size = new System.Drawing.Size(146, 17);
      this.checkPrefixLoadAddress.TabIndex = 0;
      this.checkPrefixLoadAddress.Text = "Prefix Load Address (hex)";
      this.checkPrefixLoadAddress.UseVisualStyleBackColor = true;
      this.checkPrefixLoadAddress.CheckedChanged += new System.EventHandler(this.checkPrefixLoadAddress_CheckedChanged);
      // 
      // ExportMapCharsetAsBinaryFile
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.editPrefixLoadAddress);
      this.Controls.Add(this.checkPrefixLoadAddress);
      this.Name = "ExportMapCharsetAsBinaryFile";
      this.Size = new System.Drawing.Size(317, 317);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox editPrefixLoadAddress;
    private System.Windows.Forms.CheckBox checkPrefixLoadAddress;
  }
}
