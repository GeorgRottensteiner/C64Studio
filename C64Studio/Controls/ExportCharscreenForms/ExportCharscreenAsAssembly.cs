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
      bool            stripInvisibleColors = false;//checkExportToBASICCollapseColors.Checked;

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
        int             curColor = -1;
        int             colorChangeCache = -1;
        bool            mega65UpperColorRange = false;
        bool            isFirstCharInLine = true;
        bool            addColors = ( Info.Data != ExportCharsetScreenInfo.ExportData.CHAR_ONLY );

        if ( checkExportASMAsPetSCII.Checked )
        {
          sbPET = new StringBuilder();
          for ( int i = Info.Area.Top; i < Info.Area.Bottom; ++i )
          {
            sbPET.Append( "!pet " );
            insideQuotes = false;
            isFirstCharInLine = true;
            for ( int x = Info.Area.Left; x < Info.Area.Right; ++x )
            {
              ushort newColor = (ushort)Info.Charscreen.ColorAt( x, i );
              byte newChar = (byte)Info.Charscreen.CharacterAt( x, i );

              byte charToAdd = newChar;

              if ( newChar >= 128 )
              {
                if ( !isReverse )
                {
                  if ( usePETStatement )
                  {
                    EnableQuotes( isFirstCharInLine, Info, sbPET, ref insideQuotes );
                    sbPET.Append( "{rvson}" );
                    isFirstCharInLine = false;
                  }
                  isReverse = true;
                }
              }
              else if ( isReverse )
              {
                if ( usePETStatement )
                {
                  EnableQuotes( isFirstCharInLine, Info, sbPET, ref insideQuotes );
                  sbPET.Append( "{rvsoff}" );
                  isFirstCharInLine = false;
                }
                isReverse = false;
              }
              if ( isReverse )
              {
                charToAdd -= 128;
              }

              if ( ( addColors )
              &&   ( newColor != curColor ) )
              {
                if ( stripInvisibleColors )
                {
                  colorChangeCache = newColor;
                }
                else
                {
                  int colorToUse = newColor;
                  if ( newColor >= 16 )
                  {
                    if ( !mega65UpperColorRange )
                    {
                      mega65UpperColorRange = true;
                      EnableQuotes( isFirstCharInLine, Info, sbPET, ref insideQuotes );
                      sbPET.Append( "{" + ConstantData.PetSCIIToChar[1].Replacements[0] + "}" );
                      isFirstCharInLine = false;
                    }
                    colorToUse &= 0x0f;
                  }
                  else if ( mega65UpperColorRange )
                  {
                    mega65UpperColorRange = false;
                    EnableQuotes( isFirstCharInLine, Info, sbPET, ref insideQuotes );
                    sbPET.Append( "{" + ConstantData.PetSCIIToChar[4].Replacements[0] + "}" );
                    isFirstCharInLine = false;
                  }
                  EnableQuotes( isFirstCharInLine, Info, sbPET, ref insideQuotes );
                  sbPET.Append( "{" + ConstantData.PetSCIIToChar[ConstantData.ColorToPetSCIIChar[(byte)colorToUse]].Replacements[0] +"}" );
                  isFirstCharInLine = false;
                }
                curColor = newColor;
              }


              if ( ( ConstantData.ScreenCodeToChar[charToAdd].HasNative )
              &&   ( ConstantData.ScreenCodeToChar[charToAdd].CharValue < 256 ) )
              {
                EnableQuotes( isFirstCharInLine, Info, sbPET, ref insideQuotes );
                sbPET.Append( ConstantData.ScreenCodeToChar[charToAdd].CharValue );
                insideQuotes = true;
                isFirstCharInLine = false;
              }
              else
              {
                DisableQuotes( isFirstCharInLine, Info, sbPET, ref insideQuotes );
                if ( !isFirstCharInLine )
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
                isFirstCharInLine = false;
              }
            }
            DisableQuotes( isFirstCharInLine, Info, sbPET, ref insideQuotes );
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

      var mode = Info.Data;
      if ( checkExportASMAsPetSCII.Checked )
      {
        if ( ( mode == ExportCharsetScreenInfo.ExportData.COLOR_THEN_CHAR )
        ||   ( mode == ExportCharsetScreenInfo.ExportData.CHAR_THEN_COLOR ) )
        {
          mode = ExportCharsetScreenInfo.ExportData.CHAR_ONLY;
        }
      }

      switch ( mode )
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



    private void EnableQuotes( bool isFirstCharInLine, ExportCharsetScreenInfo info, StringBuilder sbPET, ref bool insideQuotes )
    {
      if ( !insideQuotes )
      {
        if ( !isFirstCharInLine )
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



    private void DisableQuotes( bool isFirstCharInLine, ExportCharsetScreenInfo info, StringBuilder sbPET, ref bool insideQuotes )
    {
      if ( insideQuotes )
      {
        if ( !isFirstCharInLine )
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
