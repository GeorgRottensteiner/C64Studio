
namespace RetroDevStudio.Controls
{
  partial class ExportGraphicScreenAsImage
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
      this.checkUseCompression = new DecentForms.CheckBox();
      this.label2 = new System.Windows.Forms.Label();
      this.comboExportImageNumberOfColors = new System.Windows.Forms.ComboBox();
      this.label3 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label1.Location = new System.Drawing.Point(35, 40);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(276, 34);
      this.label1.TabIndex = 1;
      this.label1.Text = "Currently applies only to IFF format, tries to use as few planes as possible";
      // 
      // checkUseCompression
      // 
      this.checkUseCompression.Appearance = System.Windows.Forms.Appearance.Normal;
      this.checkUseCompression.BorderStyle = DecentForms.BorderStyle.NONE;
      this.checkUseCompression.CheckAlign = DecentForms.ContentAlignment.MiddleLeft;
      this.checkUseCompression.Checked = true;
      this.checkUseCompression.Image = null;
      this.checkUseCompression.Location = new System.Drawing.Point(3, 86);
      this.checkUseCompression.Name = "checkUseCompression";
      this.checkUseCompression.Size = new System.Drawing.Size(180, 17);
      this.checkUseCompression.TabIndex = 1;
      this.checkUseCompression.Text = "Use Compression if optional";
      // 
      // label2
      // 
      this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label2.Location = new System.Drawing.Point(35, 106);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(279, 39);
      this.label2.TabIndex = 1;
      this.label2.Text = "Currently applies only to IFF format, uses optional compression";
      // 
      // comboExportImageNumberOfColors
      // 
      this.comboExportImageNumberOfColors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.comboExportImageNumberOfColors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboExportImageNumberOfColors.FormattingEnabled = true;
      this.comboExportImageNumberOfColors.Location = new System.Drawing.Point(38, 16);
      this.comboExportImageNumberOfColors.Name = "comboExportImageNumberOfColors";
      this.comboExportImageNumberOfColors.Size = new System.Drawing.Size(259, 21);
      this.comboExportImageNumberOfColors.TabIndex = 0;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(4, 0);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(135, 13);
      this.label3.TabIndex = 3;
      this.label3.Text = "Number of Colors to export:";
      // 
      // ExportGraphicScreenAsImage
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.label3);
      this.Controls.Add(this.comboExportImageNumberOfColors);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.checkUseCompression);
      this.Name = "ExportGraphicScreenAsImage";
      this.Size = new System.Drawing.Size(317, 317);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
    private System.Windows.Forms.Label label1;
    private DecentForms.CheckBox checkUseCompression;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox comboExportImageNumberOfColors;
    private System.Windows.Forms.Label label3;
  }
}
