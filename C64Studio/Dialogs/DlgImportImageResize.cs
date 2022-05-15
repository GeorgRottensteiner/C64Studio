using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace RetroDevStudio.Dialogs
{
  partial class DlgImportImageResize : Form
  {
    public enum ImportBehaviour
    {
      CLIP_IMAGE,
      ADJUST_SCREEN_SIZE,
      CANCEL
    }



    public ImportBehaviour ChosenResult
    {
      get;
      set;
    }




    public DlgImportImageResize( int ImageWidth, int ImageHeight, int ScreenWidth, int ScreenHeight, StudioCore Core )
    {
      ChosenResult = ImportBehaviour.CANCEL;
      InitializeComponent();
      labelImageInfo.Text = "The imported image has the size " + ImageWidth + "x" + ImageHeight + ".\r\n"
                          + "The current screen has the size " + ScreenWidth + "x" + ScreenHeight + ".\r\n"
                          + "\r\nClip the image (if it is bigger than the screen), adjust the screen size to the image size or cancel the import?";

      Core.Theming.ApplyTheme( this );
    }



    private void btnClip_Click( object sender, EventArgs e )
    {
      ChosenResult = ImportBehaviour.CLIP_IMAGE;
      DialogResult = DialogResult.OK;
      Close();
    }



    private void btnAdjustScreenSize_Click( object sender, EventArgs e )
    {
      ChosenResult = ImportBehaviour.ADJUST_SCREEN_SIZE;
      DialogResult = DialogResult.OK;
      Close();
    }



    private void btnCancel_Click( object sender, EventArgs e )
    {
      ChosenResult = ImportBehaviour.CANCEL;
      DialogResult = DialogResult.Cancel;
      Close();
    }


  }
}
