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
  public partial class ExportCharscreenAsImage : ExportCharscreenFormBase
  {
    public ExportCharscreenAsImage() :
      base( null )
    { 
    }



    public ExportCharscreenAsImage( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    public override bool HandleExport( ExportCharsetScreenInfo Info, TextBox EditOutput, DocumentInfo DocInfo )
    {
      int     neededWidth   = Info.Charscreen.ScreenWidth * 8;
      int     neededHeight  = Info.Charscreen.ScreenHeight * 8;

      GR.Image.MemoryImage targetImg = new GR.Image.MemoryImage( neededWidth, neededHeight, System.Drawing.Imaging.PixelFormat.Format32bppRgb );

      Info.Image.DrawTo( targetImg, 0, 0 );

      System.Drawing.Bitmap bmpTarget = targetImg.GetAsBitmap();

      Clipboard.SetImage( bmpTarget );
      bmpTarget.Dispose();

      return true;
    }



  }
}
