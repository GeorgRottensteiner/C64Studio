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
  public partial class ExportBinaryAsPRINTStatement : ExportBinaryFormBase
  {
    public ExportBinaryAsPRINTStatement() :
      base( null )
    { 
    }



    public ExportBinaryAsPRINTStatement( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    private int GetExportCharCount()
    {
      if ( checkWrapAtMaxChars.Checked )
      {
        return GR.Convert.ToI32( editWrapCharCount.Text );
      }
      return 80;
    }



    public override bool HandleExport( ExportBinaryInfo info, DocumentInfo DocInfo )
    {
      var   sb = new StringBuilder();

      int startLine = GR.Convert.ToI32( editExportBASICLineNo.Text );
      if ( ( startLine < 0 )
      ||   ( startLine > 63999 ) )
      {
        startLine = 10;
      }
      int lineOffset = GR.Convert.ToI32( editExportBASICLineOffset.Text );
      if ( ( lineOffset < 0 )
      ||   ( lineOffset > 63999 ) )
      {
        lineOffset = 10;
      }

      int     wrapCharCount = GetExportCharCount();
      bool    useAddress = checkPlaceAddress.Checked;
      bool    isReverse = false;
      int     startLength = sb.Length;
      int     address = ParseValue( editStartAddress.Text );

      List<char>  charsToAppend = new List<char>();
      if ( useAddress )
      {
        sb.Append( startLine );
        sb.Append( "POKE648," );
        sb.Append( address / 256 );
        sb.Append( ":PRINT\"" );
        sb.Append( ConstantData.PetSCIIToChar[19].CharValue );
        sb.Append( "\"" );
        if ( ( address % 256 ) != 0 )
        {
          int   delta = address % 256;

          sb.Append( "SPC(" );
          sb.Append( delta );
          sb.AppendLine( ");" );
        }
        else
        {
          sb.AppendLine( "," );
        }
        startLine += lineOffset;
      }
      for ( int i = 0; i < (int)info.Data.Length; ++i )
      {
        byte newByte = info.Data.ByteAt( i );

        if ( newByte >= 128 )
        {
          if ( !isReverse )
          {
            isReverse = true;
            charsToAppend.Add( ConstantData.PetSCIIToChar[18].CharValue );
          }
        }
        else
        {
          if ( isReverse )
          {
            isReverse = false;
            charsToAppend.Add( ConstantData.PetSCIIToChar[146].CharValue );
          }
        }
        newByte &= 0x7f;
        if ( newByte == 34 )
        {
          string replacement = "\"CHR$(34)CHR$(34)\"" + ConstantData.PetSCIIToChar[157].CharValue;
          // if " is the last char we don't need to place a second
          if ( i + 1 == info.Data.Length )
          {
            replacement = "\"CHR$(34)";
          }
          for ( int t = 0; t < replacement.Length; ++t )
          {
            charsToAppend.Add( ConstantData.CharToC64Char[replacement[t]].CharValue );
          }
        }
        else
        {
          var key = ConstantData.AllPhysicalKeyInfos.FirstOrDefault( pk => pk.HasScreenCode && pk.ScreenCodeValue == newByte );
          if ( key != null )
          {
            charsToAppend.Add( key.CharValue );
          }
        }
      }

      if ( charsToAppend.Any() )
      {
        bool    lineStarted = false;
        foreach ( var c in charsToAppend )
        {
          if ( !lineStarted )
          {
            lineStarted = true;
            startLength = sb.Length;
            sb.Append( startLine );
            startLine += lineOffset;
            sb.Append( " PRINT\"" );
          }

          // don't make lines too long!
          if ( sb.Length - startLength + 3 >= wrapCharCount - 1 )
          {
            // we need to break and start a new line
            sb.Append( "\";\n" );
            startLength = sb.Length;
            sb.Append( startLine );
            startLine += lineOffset;
            sb.Append( " PRINT\"" );
            sb.Append( c );
          }
          else
          {
            sb.Append( c );
          }
        }
        sb.Append( "\"" );
        sb.Append( "\n" );
      }

      if ( useAddress )
      {
        sb.Append( startLine );
        sb.AppendLine( "POKE648,4" );
      }

      editTextOutput.Font = new System.Drawing.Font( Core.MainForm.m_FontC64.Families[0], 16, System.Drawing.GraphicsUnit.Pixel );
      editTextOutput.Text = sb.ToString().Replace( "\n", "\r\n" );
      return true;
    }



    private int ParseValue( string text )
    {
      if ( text.StartsWith( "$" ) )
      {
        return GR.Convert.ToI32( text.Substring( 1 ), 16 );
      }
      else if ( text.StartsWith( "0x" ) )
      {
        return GR.Convert.ToI32( text.Substring( 2 ), 16 );
      }
      return GR.Convert.ToI32( text );
    }



    private void checkPlaceAddress_CheckedChanged( object sender, EventArgs e )
    {
      editStartAddress.Enabled = checkPlaceAddress.Checked;
    }



  }
}
