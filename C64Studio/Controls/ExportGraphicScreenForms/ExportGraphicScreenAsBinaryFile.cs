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
using GR.Memory;

namespace RetroDevStudio.Controls
{
  public partial class ExportGraphicScreenAsBinaryFile : ExportGraphicScreenFormBase
  {
    private enum ExportType
    {
      [Description( "Hires Bitmap" )]
      HIRES_BITMAP,
      [Description( "Multicolor Bitmap" )]
      MULTICOLOR_BITMAP,
      [Description( "HiRes Charset" )]
      HIRES_CHARSET,
      [Description( "HiRes Charset and screen data assembly" )]
      HIRES_CHARSET_SCREEN_ASSEMBLY,
      [Description( "Multicolor Charset" )]
      MULTICOLOR_CHARSET,
      [Description( "Multicolor Charset and screen data assembly" )]
      MULTICOLOR_CHARSET_SCREEN_ASSEMBLY
    };



    private enum ExportContent
    {
      [Description( "Bitmap, Screen, Color" )]
      BITMAP_SCREEN_COLOR = 0,
      [Description( "Bitmap, Color, Screen" )]
      BITMAP_COLOR_SCREEN,
      [Description( "Bitmap, Screen" )]
      BITMAP_SCREEN,
      [Description( "Bitmap, Color" )]
      BITMAP_COLOR,
      [Description( "Bitmap" )]
      BITMAP,
      [Description( "Screen" )]
      SCREEN,
      [Description( "Color" )]
      COLOR
    };



    public ExportGraphicScreenAsBinaryFile() :
      base( null )
    { 
    }



    public ExportGraphicScreenAsBinaryFile( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();

      UtilForms.FillComboWithEnumDescription( comboExportType, typeof( ExportType ) );
      UtilForms.FillComboWithEnumDescription( comboExportContent, typeof( ExportContent ) );

      comboExportType.SelectedIndex = 0;
      comboExportContent.SelectedIndex = 0;
    }



    public override bool HandleExport( ExportGraphicScreenInfo Info, TextBox EditOutput, DocumentInfo DocInfo )
    {
      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Save data as";
      saveDlg.Filter = "Binary Data|*.bin|All Files|*.*";
      if ( DocInfo.Project != null )
      {
        saveDlg.InitialDirectory = DocInfo.Project.Settings.BasePath;
      }
      if ( saveDlg.ShowDialog() != DialogResult.OK )
      {
        return false;
      }

      // prepare data
      var exportType = (ExportType)comboExportType.SelectedIndex;
      var exportContent = (ExportContent)comboExportContent.SelectedIndex;

      GR.Memory.ByteBuffer finalData = null;
      var screenData   = new GR.Memory.ByteBuffer();
      var colorData  = new GR.Memory.ByteBuffer();
      var bitmapData   = new GR.Memory.ByteBuffer();

      switch ( exportType )
      {
        case ExportType.HIRES_BITMAP:
          Info.Project.ImageToHiresBitmapData( Info.Project.ColorMapping, null, null,
                                               0, 0, Info.BlockWidth, Info.BlockHeight,
                                               out bitmapData, out screenData, out colorData );
          break;
        case ExportType.MULTICOLOR_BITMAP:
          Info.Project.ImageToMCBitmapData( Info.Project.ColorMapping, null, null,
                                            0, 0, Info.BlockWidth, Info.BlockHeight,
                                            out bitmapData, out screenData, out colorData );
          break;
          /*
        case 2:
          // hires charset
          break;
        case 3:
          // MC charset
          //ExportMCCharset( false );
          break;
        case 4:
          // MC charset and screen assembly
          //ExportMCCharset( true );
          break;*/
      }

      switch ( exportContent )
      {
        case ExportContent.BITMAP:
          finalData.Append( bitmapData );
          break;
        case ExportContent.BITMAP_COLOR:
          finalData.Append( bitmapData );
          finalData.Append( colorData );
          break;
        case ExportContent.BITMAP_COLOR_SCREEN:
          finalData.Append( bitmapData );
          finalData.Append( colorData );
          finalData.Append( screenData );
          break;
        case ExportContent.BITMAP_SCREEN:
          finalData.Append( bitmapData );
          finalData.Append( screenData );
          break;
        case ExportContent.BITMAP_SCREEN_COLOR:
          finalData.Append( bitmapData );
          finalData.Append( screenData );
          finalData.Append( colorData );
          break;
        case ExportContent.COLOR:
          finalData.Append( colorData );
          break;
        case ExportContent.SCREEN:
          finalData.Append( screenData );
          break;
        default:
          return false;
      }

      if ( finalData != null )
      {
        if ( checkPrefixLoadAddress.Checked )
        {
          ushort address = GR.Convert.ToU16( editPrefixLoadAddress.Text, 16 );

          var addressData = new ByteBuffer();
          addressData.AppendU16( address );
          finalData = addressData + finalData;
        }

        GR.IO.File.WriteAllBytes( saveDlg.FileName, finalData );
      }
      return true;
    }



    private void checkPrefixLoadAddress_CheckedChanged( object sender, EventArgs e )
    {
      editPrefixLoadAddress.Enabled = checkPrefixLoadAddress.Checked;
    }

    private void editPrefixLoadAddress_KeyPress( object sender, KeyPressEventArgs e )
    {
      if ( ( ( e.KeyChar >= '0' )
      &&     ( e.KeyChar <= '9' ) )
      ||   ( ( e.KeyChar >= 'A' )
      &&     ( e.KeyChar <= 'F' ) )
      ||   ( ( e.KeyChar >= 'a' )
      &&     ( e.KeyChar <= 'f' ) )
      ||   ( char.IsControl( e.KeyChar ) ) )
      {
      }
      else
      {
        e.Handled = true;
      }
    }



  }
}
