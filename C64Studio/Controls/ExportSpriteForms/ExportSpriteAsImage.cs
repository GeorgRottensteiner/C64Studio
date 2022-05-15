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
  public partial class ExportSpriteAsImage : ExportSpriteFormBase
  {
    public ExportSpriteAsImage() :
      base( null )
    { 
    }



    public ExportSpriteAsImage( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    public override bool HandleExport( ExportSpriteInfo Info, TextBox EditOutput, DocumentInfo DocInfo )
    {
      GR.Image.MemoryImage    targetImg = new GR.Image.MemoryImage( Info.Project.Sprites[0].Tile.Width * 4, ( ( Info.Project.TotalNumberOfSprites + 3 ) / 4 ) * Info.Project.Sprites[0].Tile.Height, Info.Project.Sprites[0].Tile.Image.PixelFormat );
      PaletteManager.ApplyPalette( targetImg );

      List<int>     exportIndices = Info.ExportIndices;

      foreach ( int i in exportIndices )
      {
        Info.Project.Sprites[i].Tile.Image.DrawTo( targetImg, 
                                                   ( i % 4 ) * Info.Project.Sprites[i].Tile.Width, 
                                                   ( i / 4 ) * Info.Project.Sprites[i].Tile.Height );
      }

      var bmpTarget = targetImg.GetAsBitmap();

      Clipboard.SetImage( bmpTarget );
      bmpTarget.Dispose();

      return true;
    }



  }
}
