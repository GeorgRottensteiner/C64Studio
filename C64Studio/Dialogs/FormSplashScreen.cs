using GR.Image;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RetroDevStudio
{
  public partial class FormSplashScreen : Form
  {
    public FormSplashScreen()
    {
      InitializeComponent();

      labelInfo.Text = labelInfo.Text.Replace( "<v>", StudioCore.StudioVersion );

      DPIHandler.ResizeControlsForDPI( this );
    }



    protected override void OnPaint( PaintEventArgs e )
    {
      base.OnPaint( e );
      ControlPaint.DrawBorder( e.Graphics, ClientRectangle,
                                  Color.Black, 1, ButtonBorderStyle.Inset,
                                  Color.Black, 1, ButtonBorderStyle.Inset,
                                  Color.Black, 1, ButtonBorderStyle.Inset,
                                  Color.Black, 1, ButtonBorderStyle.Inset );
    }
  }
}
