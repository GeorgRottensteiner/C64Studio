﻿using GR.Memory;
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



namespace RetroDevStudio.Controls
{
  public partial class ExportCharscreenAsAssembly : ExportCharscreenFormBase
  {
    public ExportCharscreenAsAssembly() :
      base( null )
    { 
    }



    public ExportCharscreenAsAssembly( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    private void checkExportToDataWrap_CheckedChanged( object sender, EventArgs e )
    {
      editWrapByteCount.Enabled = checkExportToDataWrap.Checked;
    }



    public override bool HandleExport( ExportCharsetScreenInfo Info, TextBox EditOutput, DocumentInfo DocInfo )
    {
      var             sb = new StringBuilder();
      StringBuilder   sbPET = null;

      if ( ( checkExportASMAsPetSCII.Checked )
      &&   ( ( Info.Data == ExportCharsetScreenInfo.ExportData.CHAR_THEN_COLOR )
      ||     ( Info.Data == ExportCharsetScreenInfo.ExportData.COLOR_THEN_CHAR )
      ||     ( Info.Data == ExportCharsetScreenInfo.ExportData.CHAR_ONLY ) ) )
      {
        if ( Lookup.NumBytesOfSingleCharacter( Lookup.TextCharModeFromTextMode( Info.Charscreen.Mode ) ) > 1 )
        {
          EditOutput.Text = "PETSCII export is only allowed for 8bit character types";
          return true;
        }

        // pet export only exports chars, no color changes
        bool            isReverse = false;
        bool            insideQuotes = false;

        sbPET = new StringBuilder();


        for ( int i = Info.Area.Top; i < Info.Area.Bottom; ++i )
        {
          sbPET.Append( "!pet " );
          insideQuotes = false;
          for ( int x = Info.Area.Left; x < Info.Area.Right; ++x )
          {
            byte newChar = (byte)Info.Charscreen.CharacterAt( x, i );
              
            byte charToAdd = newChar;

            if ( newChar >= 128 )
            {
              isReverse = true;
            }
            else if ( isReverse )
            {
              isReverse = false;
            }
            if ( isReverse )
            {
              charToAdd -= 128;
            }
            if ( ( ConstantData.ScreenCodeToChar[newChar].HasPetSCII )
            &&   ( ConstantData.ScreenCodeToChar[charToAdd].CharValue < 256 ) )
            {
              if ( x > Info.Area.Left )
              {
                if ( !insideQuotes )
                {
                  sbPET.Append( ", \"" );
                }
              }
              else
              {
                sbPET.Append( "\"" );
              }
              sbPET.Append( ConstantData.ScreenCodeToChar[charToAdd].CharValue );
              insideQuotes = true;
            }
            else
            {
              if ( x > Info.Area.Left )
              {
                if ( insideQuotes )
                {
                  sbPET.Append( "\", " );
                }
                else
                {
                  sbPET.Append( ", " );
                }
              }
              sbPET.Append( '$' );
              sbPET.Append( newChar.ToString( "X2" ) );
              insideQuotes = false;
            }
          }
          if ( insideQuotes )
          {
            sbPET.AppendLine( "\"" );
          }
          else
          {
            sbPET.AppendLine();
          }
        }
      }

      ByteBuffer  interleavedBuffer = null;
      if ( Info.Data == ExportCharsetScreenInfo.ExportData.CHAR_AND_COLOR_INTERLEAVED )
      {
        interleavedBuffer = new ByteBuffer( Info.ScreenCharData.Length + Info.ScreenColorData.Length );
        for ( int i = 0; i < Info.ScreenCharData.Length; ++i )
        {
          interleavedBuffer.SetU8At( i * 2, Info.ScreenCharData.ByteAt( i ) );
          interleavedBuffer.SetU8At( i * 2 + 1, Info.ScreenColorData.ByteAt( i ) );
        }
      }

      string screenData = "";

      if ( sbPET != null )
      {
        screenData = sbPET.ToString();
      }
      else
      {
        screenData = Util.ToASMData( Info.ScreenCharData, checkExportToDataWrap.Checked, GR.Convert.ToI32( editWrapByteCount.Text ), checkExportToDataIncludeRes.Checked ? editPrefix.Text : "", checkExportHex.Checked );
      }
      string colorData  = Util.ToASMData( Info.ScreenColorData, checkExportToDataWrap.Checked, GR.Convert.ToI32( editWrapByteCount.Text ), checkExportToDataIncludeRes.Checked ? editPrefix.Text : "", checkExportHex.Checked );

      sb.Append( ";size " );
      sb.Append( Info.Area.Width );
      sb.Append( "," );
      sb.Append( Info.Area.Height );
      sb.AppendLine();

      switch ( Info.Data )
      {
        case ExportCharsetScreenInfo.ExportData.CHAR_THEN_COLOR:
          sb.Append( ";screen char data" + Environment.NewLine + screenData + Environment.NewLine + ";screen color data" + Environment.NewLine + colorData );
          break;
        case ExportCharsetScreenInfo.ExportData.CHAR_ONLY:
          sb.Append( ";screen char data" + Environment.NewLine + screenData + Environment.NewLine );
          break;
        case ExportCharsetScreenInfo.ExportData.COLOR_ONLY:
          sb.Append( ";screen color data" + Environment.NewLine + colorData );
          break;
        case ExportCharsetScreenInfo.ExportData.COLOR_THEN_CHAR:
          sb.Append( ";screen color data" + Environment.NewLine + colorData + Environment.NewLine + ";screen char data" + Environment.NewLine + screenData );
          break;
        case ExportCharsetScreenInfo.ExportData.CHARSET:
          sb.Append( ";charset data" + Environment.NewLine + Util.ToASMData( Info.CharsetData, checkExportToDataWrap.Checked, GR.Convert.ToI32( editWrapByteCount.Text ), checkExportToDataIncludeRes.Checked ? editPrefix.Text : "", checkExportHex.Checked ) );
          break;
        case ExportCharsetScreenInfo.ExportData.CHAR_AND_COLOR_INTERLEAVED:
          sb.Append( ";interleaved data" + Environment.NewLine + Util.ToASMData( interleavedBuffer, checkExportToDataWrap.Checked, GR.Convert.ToI32( editWrapByteCount.Text ), checkExportToDataIncludeRes.Checked ? editPrefix.Text : "", checkExportHex.Checked ) );
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



  }
}
