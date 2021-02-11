using GR.Image;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;



namespace C64Studio
{
  public partial class FormAbout : Form
  {
    public FormAbout()
    {
      InitializeComponent();

      labelInfo.Text = labelInfo.Text.Replace( "<v>", StudioCore.StudioVersion );

      pictureBox1.Image = pictureBox1.Image.GetImageStretchedDPI();
      pictureBox2.Image = pictureBox2.Image.GetImageStretchedDPI();
    }

    private void btnOK_Click( object sender, EventArgs e )
    {
      Close();
    }

  }
}
