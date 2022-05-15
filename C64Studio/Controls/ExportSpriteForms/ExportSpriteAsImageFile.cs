using RetroDevStudio.Types;
using GR.Memory;
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
  public partial class ExportSpriteAsImageFile : ExportSpriteFormBase
  {
    public ExportSpriteAsImageFile() :
      base( null )
    { 
    }



    public ExportSpriteAsImageFile( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    public override bool HandleExport( ExportSpriteInfo Info, TextBox EditOutput, DocumentInfo DocInfo )
    {
      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Export Sprites to Image";
      saveDlg.Filter = Core.MainForm.FilterString( RetroDevStudio.Types.Constants.FILEFILTER_IMAGE_FILES );
      if ( saveDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
      {
        return false;
      }

      GR.Image.MemoryImage    targetImg = new GR.Image.MemoryImage( Info.Project.Sprites[0].Tile.Width * 4, ( ( Info.Project.TotalNumberOfSprites + 3 ) / 4 ) * Info.Project.Sprites[0].Tile.Height, Info.Project.Sprites[0].Tile.Image.PixelFormat );
      PaletteManager.ApplyPalette( targetImg );

      List<int>     exportIndices = Info.ExportIndices;

      foreach ( int i in exportIndices )
      {
        Info.Project.Sprites[i].Tile.Image.DrawTo( targetImg,
                                                   ( i % 4 ) * Info.Project.Sprites[i].Tile.Width,
                                                   ( i / 4 ) * Info.Project.Sprites[i].Tile.Height );
      }

      System.Drawing.Bitmap bmpTarget = targetImg.GetAsBitmap();
      bmpTarget.Save( saveDlg.FileName, System.Drawing.Imaging.ImageFormat.Png );
      return true;
    }



  }
}
