
namespace C64Studio.Controls
{
  partial class ExportCharscreenAsCharset
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
      this.comboCharsetFiles = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // comboCharsetFiles
      // 
      this.comboCharsetFiles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboCharsetFiles.FormattingEnabled = true;
      this.comboCharsetFiles.Location = new System.Drawing.Point(93, 3);
      this.comboCharsetFiles.Name = "comboCharsetFiles";
      this.comboCharsetFiles.Size = new System.Drawing.Size(224, 21);
      this.comboCharsetFiles.TabIndex = 29;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(3, 6);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(52, 13);
      this.label1.TabIndex = 28;
      this.label1.Text = "Export to:";
      // 
      // ExportAsCharset
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.comboCharsetFiles);
      this.Controls.Add(this.label1);
      this.Name = "ExportAsCharset";
      this.Size = new System.Drawing.Size(317, 317);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ComboBox comboCharsetFiles;
    private System.Windows.Forms.Label label1;
  }
}
