using RetroDevStudio.Types;
using RetroDevStudio;
using RetroDevStudio.Formats;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static RetroDevStudio.BaseDocument;

namespace RetroDevStudio.Controls
{
  public partial class ExportCharsetAsImage : ExportCharsetFormBase
  {
    public ExportCharsetAsImage() :
      base( null )
    { 
    }



    public ExportCharsetAsImage( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    public override bool HandleExport( ExportCharsetInfo Info, TextBox EditOutput, DocumentInfo DocInfo )
    {
      GR.Image.MemoryImage    targetImg = new GR.Image.MemoryImage( 128, 128, Info.Charset.Characters[0].Tile.Image.PixelFormat );
      PaletteManager.ApplyPalette( targetImg );

      List<int>     exportIndices = Info.ExportIndices;

      foreach ( int i in exportIndices )
      {
        Info.Charset.Characters[i].Tile.Image.DrawTo( targetImg, ( i % 16 ) * 8, ( i / 16 ) * 8 );
      }

      System.Drawing.Bitmap bmpTarget = targetImg.GetAsBitmap();

      Clipboard.SetImage( bmpTarget );
      bmpTarget.Dispose();

      return true;
    }



  }
}
