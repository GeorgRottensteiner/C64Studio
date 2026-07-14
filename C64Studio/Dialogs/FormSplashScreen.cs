using GR.Image;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace RetroDevStudio.Dialogs
{
  public partial class FormSplashScreen : Form
  {
    public FormSplashScreen()
    {
      InitializeComponent();

      labelInfo.Text = labelInfo.Text.Replace( "<v>", StudioCore.StudioVersion + "." + Version.BuildNumber );
#if ( NET48_OR_GREATER ) || ( NET10_0_OR_GREATER )
      labelInfo.Text = labelInfo.Text.Replace( "<rt>", System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription );
#else
      labelInfo.Text = labelInfo.Text.Replace( "<rt>", ".NET 3.5" );
#endif

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
