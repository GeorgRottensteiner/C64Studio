
namespace RetroDevStudio.Controls
{
  partial class ColorPickerCharsNES
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
      GR.Image.FastImage fastImage2 = new GR.Image.FastImage();
      GR.Image.FastImage fastImage3 = new GR.Image.FastImage();
      GR.Image.FastImage fastImage4 = new GR.Image.FastImage();
      this.picPalette4 = new GR.Forms.FastPictureBox();
      this.picPalette3 = new GR.Forms.FastPictureBox();
      this.picPalette2 = new GR.Forms.FastPictureBox();
      this.picPalette1 = new GR.Forms.FastPictureBox();
      ((System.ComponentModel.ISupportInitialize)(this.picPalette4)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picPalette3)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picPalette2)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picPalette1)).BeginInit();
      this.SuspendLayout();
      // 
      // picPalette4
      // 
      this.picPalette4.AutoResize = false;
      this.picPalette4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.picPalette4.DisplayPage = fastImage1;
      this.picPalette4.Image = null;
      this.picPalette4.Location = new System.Drawing.Point(94, 29);
      this.picPalette4.Name = "picPalette4";
      this.picPalette4.Size = new System.Drawing.Size(85, 20);
      this.picPalette4.TabIndex = 1;
      this.picPalette4.TabStop = false;
      this.picPalette4.PostPaint += new GR.Forms.FastPictureBox.PostPaintCallback(this.picPalette4_PostPaint);
      this.picPalette4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picPalette_MouseDown);
      this.picPalette4.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picPalette_MouseMove);
      // 
      // picPalette3
      // 
      this.picPalette3.AutoResize = false;
      this.picPalette3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.picPalette3.DisplayPage = fastImage2;
      this.picPalette3.Image = null;
      this.picPalette3.Location = new System.Drawing.Point(3, 29);
      this.picPalette3.Name = "picPalette3";
      this.picPalette3.Size = new System.Drawing.Size(85, 20);
      this.picPalette3.TabIndex = 2;
      this.picPalette3.TabStop = false;
      this.picPalette3.PostPaint += new GR.Forms.FastPictureBox.PostPaintCallback(this.picPalette3_PostPaint);
      this.picPalette3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picPalette_MouseDown);
      this.picPalette3.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picPalette_MouseMove);
      // 
      // picPalette2
      // 
      this.picPalette2.AutoResize = false;
      this.picPalette2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.picPalette2.DisplayPage = fastImage3;
      this.picPalette2.Image = null;
      this.picPalette2.Location = new System.Drawing.Point(94, 3);
      this.picPalette2.Name = "picPalette2";
      this.picPalette2.Size = new System.Drawing.Size(85, 20);
      this.picPalette2.TabIndex = 3;
      this.picPalette2.TabStop = false;
      this.picPalette2.PostPaint += new GR.Forms.FastPictureBox.PostPaintCallback(this.picPalette2_PostPaint);
      this.picPalette2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picPalette_MouseDown);
      this.picPalette2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picPalette_MouseMove);
      // 
      // picPalette1
      // 
      this.picPalette1.AutoResize = false;
      this.picPalette1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.picPalette1.DisplayPage = fastImage4;
      this.picPalette1.Image = null;
      this.picPalette1.Location = new System.Drawing.Point(3, 3);
      this.picPalette1.Name = "picPalette1";
      this.picPalette1.Size = new System.Drawing.Size(85, 20);
      this.picPalette1.TabIndex = 4;
      this.picPalette1.TabStop = false;
      this.picPalette1.PostPaint += new GR.Forms.FastPictureBox.PostPaintCallback(this.picPalette1_PostPaint);
      this.picPalette1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picPalette_MouseDown);
      this.picPalette1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picPalette_MouseMove);
      // 
      // ColorChooserNES
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.picPalette4);
      this.Controls.Add(this.picPalette3);
      this.Controls.Add(this.picPalette2);
      this.Controls.Add(this.picPalette1);
      this.Name = "ColorChooserNES";
      ((System.ComponentModel.ISupportInitialize)(this.picPalette4)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picPalette3)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picPalette2)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picPalette1)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private GR.Forms.FastPictureBox picPalette4;
    private GR.Forms.FastPictureBox picPalette3;
    private GR.Forms.FastPictureBox picPalette2;
    private GR.Forms.FastPictureBox picPalette1;
  }
}
