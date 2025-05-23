﻿using GR.Memory;
using RetroDevStudio;
using RetroDevStudio.Documents;
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



namespace RetroDevStudio.Controls
{
  public partial class ExportGraphicScreenAsAssembly : ExportGraphicScreenFormBase
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



    public ExportGraphicScreenAsAssembly() :
      base( null )
    { 
    }



    public ExportGraphicScreenAsAssembly( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();

      UtilForms.FillComboWithEnumDescription( comboExportType, typeof( ExportType ) );
      UtilForms.FillComboWithEnumDescription( comboExportContent, typeof( ExportContent ) );

      comboExportType.SelectedIndex     = 0;
      comboExportContent.SelectedIndex  = 0;
    }



    private void checkExportToDataWrap_CheckedChanged( object sender, EventArgs e )
    {
      editWrapByteCount.Enabled = checkExportToDataWrap.Checked;
    }



    public override bool HandleExport( ExportGraphicScreenInfo Info, TextBox EditOutput, DocumentInfo DocInfo )
    {
      var exportType = (ExportType)comboExportType.SelectedIndex;
      var exportContent = (ExportContent)comboExportContent.SelectedIndex;

      var             sb = new StringBuilder();

      var screenChar        = new GR.Memory.ByteBuffer();
      var screenColor       = new GR.Memory.ByteBuffer();
      var bitmapData        = new GR.Memory.ByteBuffer();
      var charsetScreenData = new List<uint>();
      var charsetData       = new GR.Memory.ByteBuffer();

      switch ( exportType )
      {
        case ExportType.HIRES_BITMAP:
          Info.Project.ImageToHiresBitmapData( Info.Project.ColorMapping, null, null, 
                                               0, 0, Info.BlockWidth, Info.BlockHeight, 
                                               out bitmapData, out screenChar, out screenColor );
          break;
        case ExportType.MULTICOLOR_BITMAP:
          Info.Project.ImageToMCBitmapData( Info.Project.ColorMapping, null, null,
                                            0, 0, Info.BlockWidth, Info.BlockHeight,
                                            out bitmapData, out screenChar, out screenColor );
          break;
        case ExportType.HIRES_CHARSET:
        case ExportType.MULTICOLOR_CHARSET:
          if ( !ApplyCharsetChecks( Info, exportType == ExportType.MULTICOLOR_CHARSET, true, out charsetScreenData, out charsetData ) )
          {
            return false;
          }
          break;
        case ExportType.MULTICOLOR_CHARSET_SCREEN_ASSEMBLY:
        case ExportType.HIRES_CHARSET_SCREEN_ASSEMBLY:
          if ( !ApplyCharsetChecks( Info, exportType == ExportType.MULTICOLOR_CHARSET_SCREEN_ASSEMBLY, true, out charsetScreenData, out charsetData ) )
          {
            return false;
          }
          break;
      }


      string bitmapAssembly = Util.ToASMData( bitmapData, checkExportToDataWrap.Checked, GR.Convert.ToI32( editWrapByteCount.Text ), checkExportToDataIncludeRes.Checked ? editPrefix.Text : "", checkExportHex.Checked );
      string screenAssembly = Util.ToASMData( screenChar, checkExportToDataWrap.Checked, GR.Convert.ToI32( editWrapByteCount.Text ), checkExportToDataIncludeRes.Checked ? editPrefix.Text : "", checkExportHex.Checked );
      string colorAssembly  = Util.ToASMData( screenColor, checkExportToDataWrap.Checked, GR.Convert.ToI32( editWrapByteCount.Text ), checkExportToDataIncludeRes.Checked ? editPrefix.Text : "", checkExportHex.Checked );

      switch ( exportType )
      {
        case ExportType.HIRES_CHARSET:
        case ExportType.MULTICOLOR_CHARSET:
          sb.AppendLine( ";charset data" );
          sb.AppendLine( Util.ToASMData( charsetData, checkExportToDataWrap.Checked, GR.Convert.ToI32( editWrapByteCount.Text ), checkExportToDataIncludeRes.Checked ? editPrefix.Text : "", checkExportHex.Checked ) );
          EditOutput.Text = sb.ToString();
          return true;
        case ExportType.HIRES_CHARSET_SCREEN_ASSEMBLY:
        case ExportType.MULTICOLOR_CHARSET_SCREEN_ASSEMBLY:
          sb.AppendLine( ";charset data" );
          sb.AppendLine( Util.ToASMData( charsetData, checkExportToDataWrap.Checked, GR.Convert.ToI32( editWrapByteCount.Text ), checkExportToDataIncludeRes.Checked ? editPrefix.Text : "", checkExportHex.Checked ) );

          {
            var dataChars = new ByteBuffer( (uint)charsetScreenData.Count );
            var dataColors = new ByteBuffer( (uint)charsetScreenData.Count );
            for ( int i = 0; i < charsetScreenData.Count; ++i )
            {
              dataChars.SetU8At( i, (byte)( charsetScreenData[i] & 0x00ff ) );
              dataColors.SetU8At( i, (byte)( ( charsetScreenData[i] >> 16 ) & 0x00ff ) );
            }
            sb.AppendLine( ";screen data" );
            sb.AppendLine( Util.ToASMData( dataChars, checkExportToDataWrap.Checked, GR.Convert.ToI32( editWrapByteCount.Text ), checkExportToDataIncludeRes.Checked ? editPrefix.Text : "", checkExportHex.Checked ) );
            sb.AppendLine( ";color data" );
            sb.AppendLine( Util.ToASMData( dataColors, checkExportToDataWrap.Checked, GR.Convert.ToI32( editWrapByteCount.Text ), checkExportToDataIncludeRes.Checked ? editPrefix.Text : "", checkExportHex.Checked ) );
          }

          EditOutput.Text = sb.ToString();
          return true;
      }

      switch ( exportContent )
      {
        case ExportContent.BITMAP:
          sb.AppendLine( ";bitmap data" );
          sb.AppendLine( bitmapAssembly );
          break;
        case ExportContent.BITMAP_COLOR:
          sb.AppendLine( ";bitmap data" );
          sb.AppendLine( bitmapAssembly );
          sb.AppendLine( ";color data" );
          sb.AppendLine( colorAssembly );
          break;
        case ExportContent.BITMAP_COLOR_SCREEN:
          sb.AppendLine( ";bitmap data" );
          sb.AppendLine( bitmapAssembly );
          sb.AppendLine( ";color data" );
          sb.AppendLine( colorAssembly );
          sb.AppendLine( ";screen data" );
          sb.AppendLine( screenAssembly );
          break;
        case ExportContent.BITMAP_SCREEN:
          sb.AppendLine( ";bitmap data" );
          sb.AppendLine( bitmapAssembly );
          sb.AppendLine( ";screen data" );
          sb.AppendLine( screenAssembly );
          break;
        case ExportContent.BITMAP_SCREEN_COLOR:
          sb.AppendLine( ";bitmap data" );
          sb.AppendLine( bitmapAssembly );
          sb.AppendLine( ";screen data" );
          sb.AppendLine( screenAssembly );
          sb.AppendLine( ";color data" );
          sb.AppendLine( colorAssembly );
          break;
        case ExportContent.COLOR:
          sb.AppendLine( ";color data" );
          sb.AppendLine( colorAssembly );
          break;
        case ExportContent.SCREEN:
          sb.AppendLine( ";screen data" );
          sb.AppendLine( screenAssembly );
          break;
        default:
          return false;
      }

      EditOutput.Text = sb.ToString();
      return true;
    }



    private void checkExportToDataIncludeRes_CheckedChanged( object sender, EventArgs e )
    {
      editPrefix.Enabled = checkExportToDataIncludeRes.Checked;
    }



    private void comboExportType_SelectedIndexChanged( object sender, EventArgs e )
    {
      var exportType = (ExportType)comboExportType.SelectedIndex;

      if ( ( exportType == ExportType.HIRES_CHARSET )
      ||   ( exportType == ExportType.HIRES_CHARSET_SCREEN_ASSEMBLY )
      ||   ( exportType == ExportType.MULTICOLOR_CHARSET )
      ||   ( exportType == ExportType.MULTICOLOR_CHARSET_SCREEN_ASSEMBLY ) )
      {
        comboExportContent.Enabled = false;
      }
      else
      {
        comboExportContent.Enabled = true;
      }
    }



  }
}
