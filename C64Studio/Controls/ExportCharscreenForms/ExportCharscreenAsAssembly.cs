using GR.Memory;
using RetroDevStudio;
using RetroDevStudio.Formats;
using RetroDevStudio.Types;
using SharpSid;
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
      bool            useHex = checkExportHex.Checked;
      bool            usePETStatement = checkExportASMAsPetSCII.Checked;    
      bool            usePETSCIIEncoding = checkPETSCIIEncoding.Checked;

      // can't have both
      if ( usePETStatement )
      {
        usePETSCIIEncoding = false;
      }

      var charData = new ByteBuffer( Info.ScreenCharData );

      if ( ( ( checkExportASMAsPetSCII.Checked )
      ||     ( checkPETSCIIEncoding.Checked ) )
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

        if ( checkExportASMAsPetSCII.Checked )
        {
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
                if ( !isReverse )
                {
                  if ( usePETStatement )
                  {
                    EnableQuotes( x, Info, sbPET, ref insideQuotes );
                    sbPET.Append( "{rvson}" );
                  }
                  isReverse = true;
                }
              }
              else if ( isReverse )
              {
                if ( usePETStatement )
                {
                  EnableQuotes( x, Info, sbPET, ref insideQuotes );
                  sbPET.Append( "{rvsoff}" );
                }
                isReverse = false;
              }
              if ( isReverse )
              {
                charToAdd -= 128;
              }
              if ( ( ConstantData.ScreenCodeToChar[charToAdd].HasNative )
              &&   ( ConstantData.ScreenCodeToChar[charToAdd].CharValue < 256 ) )
              {
                EnableQuotes( x, Info, sbPET, ref insideQuotes );
                sbPET.Append( ConstantData.ScreenCodeToChar[charToAdd].CharValue );
                insideQuotes = true;
              }
              else
              {
                DisableQuotes( x, Info, sbPET, ref insideQuotes );
                if ( x > Info.Area.Left )
                {
                  sbPET.Append( ", " );
                }
                newChar = ConstantData.ScreenCodeToChar[charToAdd].NativeValue;
                if ( useHex )
                {
                  sbPET.Append( '$' );
                  sbPET.Append( newChar.ToString( "X2" ) );
                }
                else
                {
                  sbPET.Append( newChar );
                }
              }
            }
            DisableQuotes( Info.Area.Right, Info, sbPET, ref insideQuotes );
            sbPET.Append( ", " );
            if ( useHex )
            {
              sbPET.Append( "$0D" );
            }
            else
            {
              sbPET.Append( "13" );
            }
            isReverse = false;
            sbPET.AppendLine();
          }
        }
        else
        {
          // encode as PETSCII values
          for ( int i = Info.Area.Top; i < Info.Area.Bottom; ++i )
          {
            for ( int x = Info.Area.Left; x < Info.Area.Right; ++x )
            {
              byte value = (byte)Info.Charscreen.CharacterAt( x, i );
              charData.SetU8At( x - Info.Area.Left + ( i - Info.Area.Top ) * Info.Area.Width, ConstantData.ScreenCodeToChar[value].NativeValue );
            }
          }
        }
      }

      ByteBuffer  interleavedBuffer = null;
      if ( Info.Data == ExportCharsetScreenInfo.ExportData.CHAR_AND_COLOR_INTERLEAVED )
      {
        interleavedBuffer = new ByteBuffer( charData.Length + Info.ScreenColorData.Length );
        for ( int i = 0; i < charData.Length; ++i )
        {
          interleavedBuffer.SetU8At( i * 2, charData.ByteAt( i ) );
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
        screenData = Util.ToASMData( charData, checkExportToDataWrap.Checked, GR.Convert.ToI32( editWrapByteCount.Text ), checkExportToDataIncludeRes.Checked ? editPrefix.Text : "", useHex );
      }
      string colorData  = Util.ToASMData( Info.ScreenColorData, checkExportToDataWrap.Checked, GR.Convert.ToI32( editWrapByteCount.Text ), checkExportToDataIncludeRes.Checked ? editPrefix.Text : "", useHex );

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
          sb.Append( ";charset data" + Environment.NewLine + Util.ToASMData( Info.CharsetData, checkExportToDataWrap.Checked, GR.Convert.ToI32( editWrapByteCount.Text ), checkExportToDataIncludeRes.Checked ? editPrefix.Text : "", useHex ) );
          break;
        case ExportCharsetScreenInfo.ExportData.CHAR_AND_COLOR_INTERLEAVED:
          sb.Append( ";interleaved data" + Environment.NewLine + Util.ToASMData( interleavedBuffer, checkExportToDataWrap.Checked, GR.Convert.ToI32( editWrapByteCount.Text ), checkExportToDataIncludeRes.Checked ? editPrefix.Text : "", useHex ) );
          break;
        default:
          return false;
      }
      EditOutput.Text = sb.ToString();
      return true;
    }



    private void EnableQuotes( int x, ExportCharsetScreenInfo info, StringBuilder sbPET, ref bool insideQuotes )
    {
      if ( !insideQuotes )
      {
        if ( x > info.Area.Left )
        {
          sbPET.Append( ", \"" );
        }
        else
        {
          sbPET.Append( "\"" );
        }
        insideQuotes = true;
      }
    }



    private void DisableQuotes( int x, ExportCharsetScreenInfo info, StringBuilder sbPET, ref bool insideQuotes )
    {
      if ( insideQuotes )
      {
        if ( x > info.Area.Left )
        {
          sbPET.Append( "\"" );
        }
        insideQuotes = false;
      }
    }



    private void checkExportToDataIncludeRes_CheckedChanged( object sender, EventArgs e )
    {
      editPrefix.Enabled = checkExportToDataIncludeRes.Checked;
    }



  }
}
