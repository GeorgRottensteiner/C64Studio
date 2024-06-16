﻿using GR.Image;
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
