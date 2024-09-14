
namespace RetroDevStudio.Controls
{
  partial class ImportCharscreenFromBinaryFile
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
      this.label1 = new System.Windows.Forms.Label();
      this.editImportSkipBytes = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(3, 6);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(59, 13);
      this.label1.TabIndex = 5;
      this.label1.Text = "Skip bytes:";
      // 
      // editImportSkipBytes
      // 
      this.editImportSkipBytes.Location = new System.Drawing.Point(68, 3);
      this.editImportSkipBytes.Name = "editImportSkipBytes";
      this.editImportSkipBytes.Size = new System.Drawing.Size(179, 20);
      this.editImportSkipBytes.TabIndex = 0;
      this.editImportSkipBytes.Text = "0";
      // 
      // label2
      // 
      this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label2.Location = new System.Drawing.Point(3, 26);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(291, 45);
      this.label2.TabIndex = 6;
      this.label2.Text = "If a file extension is \".prg\", 2 bytes are automatically skipped from the beginni" +
    "ng";
      // 
      // ImportCharscreenFromBinaryFile
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.editImportSkipBytes);
      this.Name = "ImportCharscreenFromBinaryFile";
      this.Size = new System.Drawing.Size(317, 317);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox editImportSkipBytes;
    private System.Windows.Forms.Label label2;
  }
}
