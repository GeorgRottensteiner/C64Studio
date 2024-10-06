
namespace RetroDevStudio.Controls
{
  partial class ColorSettingsNES
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
      GR.Image.FastImage fastImage6 = new GR.Image.FastImage();
      GR.Image.FastImage fastImage7 = new GR.Image.FastImage();
      GR.Image.FastImage fastImage8 = new GR.Image.FastImage();
      GR.Image.FastImage fastImage9 = new GR.Image.FastImage();
      GR.Image.FastImage fastImage10 = new GR.Image.FastImage();
      this.picPalette1 = new GR.Forms.FastPictureBox();
      this.picPalette2 = new GR.Forms.FastPictureBox();
      this.picPalette3 = new GR.Forms.FastPictureBox();
      this.picPalette4 = new GR.Forms.FastPictureBox();
      this.picFullPalette = new GR.Forms.FastPictureBox();
      this.checkPaletteSet1 = new DecentForms.CheckBox();
      this.checkPaletteSet2 = new DecentForms.CheckBox();
      this.checkPaletteSet3 = new DecentForms.CheckBox();
      this.checkPaletteSet4 = new DecentForms.CheckBox();
      ((System.ComponentModel.ISupportInitialize)(this.picPalette1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picPalette2)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picPalette3)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picPalette4)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.picFullPalette)).BeginInit();
      this.SuspendLayout();
      // 
      // picPalette1
      // 
      this.picPalette1.AutoResize = false;
      this.picPalette1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.picPalette1.DisplayPage = fastImage6;
      this.picPalette1.Image = null;
      this.picPalette1.Location = new System.Drawing.Point(6, 2);
      this.picPalette1.Name = "picPalette1";
      this.picPalette1.Size = new System.Drawing.Size(85, 20);
      this.picPalette1.TabIndex = 0;
      this.picPalette1.TabStop = false;
      this.picPalette1.PostPaint += new GR.Forms.FastPictureBox.PostPaintCallback(this.picPalette1_PostPaint);
      this.picPalette1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picPalette_MouseDown);
      this.picPalette1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picPalette_MouseMove);
      // 
      // picPalette2
      // 
      this.picPalette2.AutoResize = false;
      this.picPalette2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.picPalette2.DisplayPage = fastImage7;
      this.picPalette2.Image = null;
      this.picPalette2.Location = new System.Drawing.Point(6, 28);
      this.picPalette2.Name = "picPalette2";
      this.picPalette2.Size = new System.Drawing.Size(85, 20);
      this.picPalette2.TabIndex = 0;
      this.picPalette2.TabStop = false;
      this.picPalette2.PostPaint += new GR.Forms.FastPictureBox.PostPaintCallback(this.picPalette2_PostPaint);
      this.picPalette2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picPalette_MouseDown);
      this.picPalette2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picPalette_MouseMove);
      // 
      // picPalette3
      // 
      this.picPalette3.AutoResize = false;
      this.picPalette3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.picPalette3.DisplayPage = fastImage8;
      this.picPalette3.Image = null;
      this.picPalette3.Location = new System.Drawing.Point(6, 54);
      this.picPalette3.Name = "picPalette3";
      this.picPalette3.Size = new System.Drawing.Size(85, 20);
      this.picPalette3.TabIndex = 0;
      this.picPalette3.TabStop = false;
      this.picPalette3.PostPaint += new GR.Forms.FastPictureBox.PostPaintCallback(this.picPalette3_PostPaint);
      this.picPalette3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picPalette_MouseDown);
      this.picPalette3.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picPalette_MouseMove);
      // 
      // picPalette4
      // 
      this.picPalette4.AutoResize = false;
      this.picPalette4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.picPalette4.DisplayPage = fastImage9;
      this.picPalette4.Image = null;
      this.picPalette4.Location = new System.Drawing.Point(6, 80);
      this.picPalette4.Name = "picPalette4";
      this.picPalette4.Size = new System.Drawing.Size(85, 20);
      this.picPalette4.TabIndex = 0;
      this.picPalette4.TabStop = false;
      this.picPalette4.PostPaint += new GR.Forms.FastPictureBox.PostPaintCallback(this.picPalette4_PostPaint);
      this.picPalette4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picPalette_MouseDown);
      this.picPalette4.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picPalette_MouseMove);
      // 
      // picFullPalette
      // 
      this.picFullPalette.AutoResize = false;
      this.picFullPalette.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.picFullPalette.DisplayPage = fastImage10;
      this.picFullPalette.Image = null;
      this.picFullPalette.Location = new System.Drawing.Point(6, 104);
      this.picFullPalette.Name = "picFullPalette";
      this.picFullPalette.Size = new System.Drawing.Size(167, 78);
      this.picFullPalette.TabIndex = 0;
      this.picFullPalette.TabStop = false;
      this.picFullPalette.PostPaint += new GR.Forms.FastPictureBox.PostPaintCallback(this.picFullPalette_PostPaint);
      this.picFullPalette.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picFullPalette_MouseDown);
      this.picFullPalette.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picFullPalette_MouseMove);
      // 
      // checkPaletteSet1
      // 
      this.checkPaletteSet1.Appearance = System.Windows.Forms.Appearance.Button;
      this.checkPaletteSet1.BorderStyle = DecentForms.BorderStyle.NONE;
      this.checkPaletteSet1.CheckAlign = DecentForms.ContentAlignment.MiddleLeft;
      this.checkPaletteSet1.Checked = true;
      this.checkPaletteSet1.Enabled = false;
      this.checkPaletteSet1.Image = null;
      this.checkPaletteSet1.Location = new System.Drawing.Point(150, 2);
      this.checkPaletteSet1.Name = "checkPaletteSet1";
      this.checkPaletteSet1.Size = new System.Drawing.Size(23, 20);
      this.checkPaletteSet1.TabIndex = 0;
      this.checkPaletteSet1.Text = "A";
      this.checkPaletteSet1.CheckedChanged += new DecentForms.EventHandler(this.checkPaletteSet1_CheckedChanged);
      // 
      // checkPaletteSet2
      // 
      this.checkPaletteSet2.Appearance = System.Windows.Forms.Appearance.Button;
      this.checkPaletteSet2.BorderStyle = DecentForms.BorderStyle.NONE;
      this.checkPaletteSet2.CheckAlign = DecentForms.ContentAlignment.MiddleLeft;
      this.checkPaletteSet2.Checked = false;
      this.checkPaletteSet2.Image = null;
      this.checkPaletteSet2.Location = new System.Drawing.Point(150, 28);
      this.checkPaletteSet2.Name = "checkPaletteSet2";
      this.checkPaletteSet2.Size = new System.Drawing.Size(23, 20);
      this.checkPaletteSet2.TabIndex = 1;
      this.checkPaletteSet2.Text = "B";
      this.checkPaletteSet2.CheckedChanged += new DecentForms.EventHandler(this.checkPaletteSet2_CheckedChanged);
      // 
      // checkPaletteSet3
      // 
      this.checkPaletteSet3.Appearance = System.Windows.Forms.Appearance.Button;
      this.checkPaletteSet3.BorderStyle = DecentForms.BorderStyle.NONE;
      this.checkPaletteSet3.CheckAlign = DecentForms.ContentAlignment.MiddleLeft;
      this.checkPaletteSet3.Checked = false;
      this.checkPaletteSet3.Image = null;
      this.checkPaletteSet3.Location = new System.Drawing.Point(150, 54);
      this.checkPaletteSet3.Name = "checkPaletteSet3";
      this.checkPaletteSet3.Size = new System.Drawing.Size(23, 20);
      this.checkPaletteSet3.TabIndex = 2;
      this.checkPaletteSet3.Text = "C";
      this.checkPaletteSet3.CheckedChanged += new DecentForms.EventHandler(this.checkPaletteSet3_CheckedChanged);
      // 
      // checkPaletteSet4
      // 
      this.checkPaletteSet4.Appearance = System.Windows.Forms.Appearance.Button;
      this.checkPaletteSet4.BorderStyle = DecentForms.BorderStyle.NONE;
      this.checkPaletteSet4.CheckAlign = DecentForms.ContentAlignment.MiddleLeft;
      this.checkPaletteSet4.Checked = false;
      this.checkPaletteSet4.Image = null;
      this.checkPaletteSet4.Location = new System.Drawing.Point(150, 80);
      this.checkPaletteSet4.Name = "checkPaletteSet4";
      this.checkPaletteSet4.Size = new System.Drawing.Size(23, 20);
      this.checkPaletteSet4.TabIndex = 3;
      this.checkPaletteSet4.Text = "D";
      this.checkPaletteSet4.CheckedChanged += new DecentForms.EventHandler(this.checkPaletteSet4_CheckedChanged);
      // 
      // ColorSettingsNES
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.checkPaletteSet4);
      this.Controls.Add(this.checkPaletteSet3);
      this.Controls.Add(this.checkPaletteSet2);
      this.Controls.Add(this.checkPaletteSet1);
      this.Controls.Add(this.picFullPalette);
      this.Controls.Add(this.picPalette4);
      this.Controls.Add(this.picPalette3);
      this.Controls.Add(this.picPalette2);
      this.Controls.Add(this.picPalette1);
      this.Name = "ColorSettingsNES";
      ((System.ComponentModel.ISupportInitialize)(this.picPalette1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picPalette2)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picPalette3)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picPalette4)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.picFullPalette)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion
    private GR.Forms.FastPictureBox picPalette1;
    private GR.Forms.FastPictureBox picPalette2;
    private GR.Forms.FastPictureBox picPalette3;
    private GR.Forms.FastPictureBox picPalette4;
    private GR.Forms.FastPictureBox picFullPalette;
    private DecentForms.CheckBox checkPaletteSet1;
    private DecentForms.CheckBox checkPaletteSet2;
    private DecentForms.CheckBox checkPaletteSet3;
    private DecentForms.CheckBox checkPaletteSet4;
  }
}
