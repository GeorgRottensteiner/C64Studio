using C64Studio.Types;
using RetroDevStudio;
using RetroDevStudio.Formats;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static C64Studio.BaseDocument;

namespace C64Studio.Controls
{
  public partial class ExportAsImageFile : ExportCharscreenFormBase
  {
    public ExportAsImageFile() :
      base( null )
    { 
    }



    public ExportAsImageFile( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    public override bool HandleExport( ExportCharsetScreenInfo Info, TextBox EditOutput, DocumentInfo DocInfo )
    {
      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Export Screen to Image";
      saveDlg.Filter = "PNG File|*.png";
      if ( saveDlg.ShowDialog() != DialogResult.OK )
      {
        return false;
      }

      int     neededWidth   = Info.Charscreen.ScreenWidth * 8;
      int     neededHeight  = Info.Charscreen.ScreenHeight * 8;

      GR.Image.MemoryImage targetImg = new GR.Image.MemoryImage( neededWidth, neededHeight, System.Drawing.Imaging.PixelFormat.Format32bppRgb );

      Info.Image.DrawTo( targetImg, 0, 0 );

      System.Drawing.Bitmap bmpTarget = targetImg.GetAsBitmap();
      bmpTarget.Save( saveDlg.FileName, System.Drawing.Imaging.ImageFormat.Png );
      return true;
    }



  }
}
