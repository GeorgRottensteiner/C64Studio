using C64Studio.Types;
using GR.Memory;
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
  public partial class ExportCharsetAsImageFile : ExportCharsetFormBase
  {
    public ExportCharsetAsImageFile() :
      base( null )
    { 
    }



    public ExportCharsetAsImageFile( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    public override bool HandleExport( ExportCharsetInfo Info, TextBox EditOutput, DocumentInfo DocInfo )
    {
      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Export Characters to Image";
      saveDlg.Filter = Core.MainForm.FilterString( C64Studio.Types.Constants.FILEFILTER_IMAGE_FILES );
      if ( saveDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
      {
        return false;
      }

      GR.Image.MemoryImage    targetImg = new GR.Image.MemoryImage( 128, 128, Info.Charset.Characters[0].Tile.Image.PixelFormat );
      PaletteManager.ApplyPalette( targetImg );

      List<int>     exportIndices = Info.ExportIndices;

      foreach ( int i in exportIndices )
      {
        Info.Charset.Characters[i].Tile.Image.DrawTo( targetImg, ( i % 16 ) * 8, ( i / 16 ) * 8 );
      }
      System.Drawing.Bitmap bmpTarget = targetImg.GetAsBitmap();
      bmpTarget.Save( saveDlg.FileName, System.Drawing.Imaging.ImageFormat.Png );
      return true;
    }



  }
}
