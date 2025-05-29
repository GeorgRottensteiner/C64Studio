
namespace RetroDevStudio.Controls
{
  partial class ImportSpriteFromBinaryFile
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
      this.checkImportExpectPadding = new System.Windows.Forms.CheckBox();
      this.label2 = new System.Windows.Forms.Label();
      this.checkAutoProcessFileTypes = new System.Windows.Forms.CheckBox();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(3, 6);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(59, 13);
      this.label1.TabIndex = 0;
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
      // checkImportExpectPadding
      // 
      this.checkImportExpectPadding.AutoSize = true;
      this.checkImportExpectPadding.Location = new System.Drawing.Point(6, 29);
      this.checkImportExpectPadding.Name = "checkImportExpectPadding";
      this.checkImportExpectPadding.Size = new System.Drawing.Size(138, 17);
      this.checkImportExpectPadding.TabIndex = 1;
      this.checkImportExpectPadding.Text = "Data has padding bytes";
      this.checkImportExpectPadding.UseVisualStyleBackColor = true;
      // 
      // label2
      // 
      this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label2.Location = new System.Drawing.Point(23, 72);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(281, 45);
      this.label2.TabIndex = 3;
      this.label2.Text = "If a file extension is \".prg\", 2 bytes are automatically skipped from the beginni" +
    "ng";
      // 
      // checkAutoProcessFileTypes
      // 
      this.checkAutoProcessFileTypes.AutoSize = true;
      this.checkAutoProcessFileTypes.Checked = true;
      this.checkAutoProcessFileTypes.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkAutoProcessFileTypes.Location = new System.Drawing.Point(6, 52);
      this.checkAutoProcessFileTypes.Name = "checkAutoProcessFileTypes";
      this.checkAutoProcessFileTypes.Size = new System.Drawing.Size(189, 17);
      this.checkAutoProcessFileTypes.TabIndex = 2;
      this.checkAutoProcessFileTypes.Text = "Auto handle file types by extension";
      this.checkAutoProcessFileTypes.UseVisualStyleBackColor = true;
      // 
      // ImportSpriteFromBinaryFile
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.checkAutoProcessFileTypes);
      this.Controls.Add(this.checkImportExpectPadding);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.editImportSkipBytes);
      this.Name = "ImportSpriteFromBinaryFile";
      this.Size = new System.Drawing.Size(317, 317);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox editImportSkipBytes;
    private System.Windows.Forms.CheckBox checkImportExpectPadding;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.CheckBox checkAutoProcessFileTypes;
  }
}
