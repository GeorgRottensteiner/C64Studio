
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
      this.checkOptimizePalette = new DecentForms.CheckBox();
      this.label1 = new System.Windows.Forms.Label();
      this.checkUseCompression = new DecentForms.CheckBox();
      this.label2 = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // checkOptimizePalette
      // 
      this.checkOptimizePalette.Appearance = System.Windows.Forms.Appearance.Normal;
      this.checkOptimizePalette.BorderStyle = DecentForms.BorderStyle.NONE;
      this.checkOptimizePalette.CheckAlign = DecentForms.ContentAlignment.MiddleLeft;
      this.checkOptimizePalette.Checked = true;
      this.checkOptimizePalette.Image = null;
      this.checkOptimizePalette.Location = new System.Drawing.Point(3, 3);
      this.checkOptimizePalette.Name = "checkOptimizePalette";
      this.checkOptimizePalette.Size = new System.Drawing.Size(178, 17);
      this.checkOptimizePalette.TabIndex = 0;
      this.checkOptimizePalette.Text = "Optimize Palette if possible";
      // 
      // label1
      // 
      this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label1.Location = new System.Drawing.Point(38, 23);
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
      this.checkUseCompression.Location = new System.Drawing.Point(3, 84);
      this.checkUseCompression.Name = "checkUseCompression";
      this.checkUseCompression.Size = new System.Drawing.Size(178, 17);
      this.checkUseCompression.TabIndex = 0;
      this.checkUseCompression.Text = "Use Compression if optional";
      // 
      // label2
      // 
      this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.label2.Location = new System.Drawing.Point(35, 104);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(279, 39);
      this.label2.TabIndex = 1;
      this.label2.Text = "Currently applies only to IFF format, uses optional compression";
      // 
      // ExportGraphicScreenAsImage
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.checkUseCompression);
      this.Controls.Add(this.checkOptimizePalette);
      this.Name = "ExportGraphicScreenAsImage";
      this.Size = new System.Drawing.Size(317, 317);
      this.ResumeLayout(false);

    }

    #endregion

    private DecentForms.CheckBox checkOptimizePalette;
    private System.Windows.Forms.Label label1;
    private DecentForms.CheckBox checkUseCompression;
    private System.Windows.Forms.Label label2;
  }
}
