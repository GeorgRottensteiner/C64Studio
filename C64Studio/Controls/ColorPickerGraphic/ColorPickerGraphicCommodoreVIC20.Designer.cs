
namespace RetroDevStudio.Controls
{
  partial class ColorPickerGraphicCommodoreVIC20
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
      GR.Image.FastImage fastImage1 = new GR.Image.FastImage();
      this.panelCharColors = new GR.Forms.FastPictureBox();
      ((System.ComponentModel.ISupportInitialize)(this.panelCharColors)).BeginInit();
      this.SuspendLayout();
      // 
      // panelCharColors
      // 
      this.panelCharColors.AutoResize = false;
      this.panelCharColors.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.panelCharColors.DisplayPage = fastImage1;
      this.panelCharColors.Image = null;
      this.panelCharColors.Location = new System.Drawing.Point(0, 3);
      this.panelCharColors.Name = "panelCharColors";
      this.panelCharColors.Size = new System.Drawing.Size(260, 20);
      this.panelCharColors.TabIndex = 1;
      this.panelCharColors.TabStop = false;
      this.panelCharColors.PostPaint += new GR.Forms.FastPictureBox.PostPaintCallback(this.panelCharColors_PostPaint);
      this.panelCharColors.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelCharColors_MouseDown);
      this.panelCharColors.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelCharColors_MouseMove);
      // 
      // ColorPickerGraphicCommodoreVIC20
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.panelCharColors);
      this.Name = "ColorPickerGraphicCommodoreVIC20";
      this.Size = new System.Drawing.Size(266, 31);
      ((System.ComponentModel.ISupportInitialize)(this.panelCharColors)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private GR.Forms.FastPictureBox panelCharColors;
  }
}
