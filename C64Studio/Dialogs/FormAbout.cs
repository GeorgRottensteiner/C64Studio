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
  public partial class FormAbout : Form
  {
    public FormAbout( StudioCore Core )
    {
      InitializeComponent();

      labelInfo.Text = labelInfo.Text.Replace( "<v>", StudioCore.StudioVersion + "." + Version.BuildNumber );

#if ( NET48_OR_GREATER ) || ( NET10_0_OR_GREATER )
      labelInfo.Text = labelInfo.Text.Replace( "<rt>", System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription );
#else
      labelInfo.Text = labelInfo.Text.Replace( "<rt>", ".NET 3.5" );
#endif

      pictureBox1.Image = pictureBox1.Image.GetImageStretchedDPI();
      pictureBox2.Image = pictureBox2.Image.GetImageStretchedDPI();

      Core.Theming.ApplyTheme( this );
    }



    private void btnOK_Click( DecentForms.ControlBase Sender )
    {
      Close();
    }

  }
}
