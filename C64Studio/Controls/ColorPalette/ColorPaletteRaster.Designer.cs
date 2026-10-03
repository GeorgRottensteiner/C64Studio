
namespace RetroDevStudio.Controls
{
  partial class ColorPaletteRaster
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
      panelCharColors = new GR.Forms.FastPictureBox();
      ( (System.ComponentModel.ISupportInitialize)panelCharColors ).BeginInit();
      SuspendLayout();
      // 
      // panelCharColors
      // 
      panelCharColors.Anchor =   System.Windows.Forms.AnchorStyles.Top  |  System.Windows.Forms.AnchorStyles.Left   |  System.Windows.Forms.AnchorStyles.Right ;
      panelCharColors.AutoResize = false;
      panelCharColors.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      panelCharColors.Location = new System.Drawing.Point( 0, 0 );
      panelCharColors.Name = "panelCharColors";
      panelCharColors.Size = new System.Drawing.Size( 26, 258 );
      panelCharColors.TabIndex = 1;
      panelCharColors.TabStop = false;
      panelCharColors.PostPaint +=  panelCharColors_PostPaint ;
      panelCharColors.MouseDown +=  panelCharColors_MouseDown ;
      panelCharColors.MouseMove +=  panelCharColors_MouseMove ;
      // 
      // ColorPaletteUpTo16
      // 
      AutoScaleDimensions = new System.Drawing.SizeF( 7F, 15F );
      AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      Controls.Add( panelCharColors );
      Name = "ColorPaletteUpTo16";
      ( (System.ComponentModel.ISupportInitialize)panelCharColors ).EndInit();
      ResumeLayout( false );

    }

    #endregion

    private GR.Forms.FastPictureBox panelCharColors;
  }
}
