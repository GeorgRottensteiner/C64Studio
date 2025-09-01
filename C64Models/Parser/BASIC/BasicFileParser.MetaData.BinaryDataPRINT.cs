using GR.Memory;
using RetroDevStudio.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Security.Policy;
using System.Text;

namespace RetroDevStudio.Parser.BASIC
{
  public partial class BasicFileParser : ParserBase
  {
    /// <summary>
    /// #BINDATA <filename>,<length>,<offset>,<start line>,<line step>
    /// #BINPRINTDATA "music.bin",,2,1000,1
    /// </summary>
    /// <param name="LineIndex"></param>
    /// <param name="MetaData"></param>
    /// <param name="MetaDataParams"></param>
    /// <returns></returns>
    private bool MetaDataBinaryDataAsPRINT( int LineIndex, string MetaData, string MetaDataParams )
    {
      var  parms = PureTokenizeLine( MetaDataParams );

      if ( !ParseLineInParameters( parms.Tokens, 0, parms.Tokens.Count, LineIndex, true, out var cleanedParms ) )
      {
        AddError( LineIndex, Types.ErrorCode.E3007_BASIC_MALFORMED_METADATA, "BinPrintData expects <Filename>,<Length>,<Offset>,<Start Line No>,<Line Step>" );
        return false;
      }

      // followed by string literal for file name
      if ( ( cleanedParms.Count < 5 )
      ||   ( cleanedParms[0].Count != 1 )
      ||   ( cleanedParms[1].Count > 1 )
      ||   ( cleanedParms[2].Count > 1 )
      ||   ( cleanedParms[3].Count != 1 )
      ||   ( cleanedParms[4].Count != 1 )
      ||   ( !cleanedParms[0][0].Content.StartsWith( "\"" ) )
      ||   ( !cleanedParms[0][0].Content.EndsWith( "\"" ) ) )
      {
        AddError( LineIndex, Types.ErrorCode.E3007_BASIC_MALFORMED_METADATA, "BinPrintData expects <Filename>,<Length>,<Offset>,<Start Line No>,<Line Step>" );
        return false;
      }

      var filename = cleanedParms[0][0];
      string    includeFile = filename.Content.Substring( 1, filename.Content.Length - 2 );
      string    subFilenameFull   = GR.Path.RenameFile( m_CompileConfig.InputFile, includeFile );
      if ( ( string.IsNullOrEmpty( includeFile ) )
      ||   ( !System.IO.File.Exists( subFilenameFull ) ) )
      {
        AddError( LineIndex, Types.ErrorCode.E2000_FILE_OPEN_ERROR, $"Could not open {subFilenameFull}" );
        return false;
      }

      int localIndex = 0;
      string dummyFilename = "";
      if ( !m_ASMFileInfo.FindTrueLineSource( LineIndex, out dummyFilename, out localIndex ) )
      {
        AddError( LineIndex, Types.ErrorCode.E1401_INTERNAL_ERROR, "Includes caused a problem" );
        return false;
      }

      ByteBuffer subFile;

      try
      {
        subFile = GR.IO.File.ReadAllBytes( subFilenameFull );
      }
      catch ( System.IO.IOException )
      {
        AddError( LineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + subFilenameFull );
        return false;
      }

      int length    = (int)subFile.Length;
      int offset    = 0;
      int startLine = GR.Convert.ToI32( cleanedParms[3][0].Content );
      int stepLine  = GR.Convert.ToI32( cleanedParms[4][0].Content );

      if ( cleanedParms[1].Count > 0 )
      {
        length = GR.Convert.ToI32( cleanedParms[1][0].Content );
      }
      if ( cleanedParms[2].Count > 0 )
      {
        offset = GR.Convert.ToI32( cleanedParms[2][0].Content );
      }

      if ( ( length <= 0 )
      ||   ( offset >= subFile.Length )
      ||   ( offset + length <= 0 ) )
      {
        return true;
      }

      if ( offset < 0 )
      {
        length += offset;
        offset = 0;
      }
      if ( offset + length > subFile.Length )
      {
        length = (int)subFile.Length - offset;
      }

      string basicDatas = ToBASICPrintData( subFile.SubBuffer( offset, length ), startLine, stepLine, 0, Settings.BASICDialect.SafeLineLength, false );

      string[]  newLines = basicDatas.Split( new string[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries );

      Types.ASM.SourceInfo sourceInfo = new Types.ASM.SourceInfo();
      sourceInfo.Filename         = subFilenameFull;
      sourceInfo.FullPath         = subFilenameFull;
      sourceInfo.GlobalStartLine  = LineIndex;
      sourceInfo.LineCount        = newLines.Length;
      sourceInfo.FilenameParent   = filename.Content;
      sourceInfo.Source           = Types.ASM.SourceInfo.SourceInfoSource.MEDIA_INCLUDE;

      //SourceInfoLog( "-include at global index " + LineIndex );
      //SourceInfoLog( "-has " + subFile.Length + " lines" );

      sourceInfo.GlobalStartLine = LineIndex + 1;
      InsertSourceInfo( sourceInfo );

      // keep !src line on top
      string[] result = new string[_Lines.Length + newLines.Length];

      System.Array.Copy( _Lines, 0, result, 0, LineIndex + 1 );
      System.Array.Copy( newLines, 0, result, LineIndex + 1, newLines.Length );

      // this keeps the !source line in the final code, makes working with source infos easier though
      System.Array.Copy( _Lines, LineIndex + 1, result, LineIndex + newLines.Length + 1, _Lines.Length - LineIndex - 1 );

      // replace !source with empty line (otherwise source infos would have one line more!)
      result[LineIndex] = "";

      _Lines = result;

      m_ASMFileInfo.LineInfo.Remove( LineIndex );

      //--LineIndex;*/

      return true;
    }



    private string ToBASICPrintData( ByteBuffer data, int startLine, int stepLine, int v1, int safeLineLength, bool v2 )
    {
      var   sb = new StringBuilder();

      if ( ( startLine < 0 )
      ||   ( startLine > 63999 ) )
      {
        startLine = 10;
      }
      if ( ( stepLine < 0 )
      ||   ( stepLine > 63999 ) )
      {
        stepLine = 10;
      }

      int     wrapCharCount = Settings.BASICDialect.SafeLineLength;
      bool    isReverse = false;
      int     startLength = sb.Length;
      int     writtenBytes = 0;

      var charsToAppend = new List<string>();
      for ( int i = 0; i < (int)data.Length; ++i )
      {
        byte newByte = data.ByteAt( i );

        if ( newByte >= 128 )
        {
          if ( !isReverse )
          {
            isReverse = true;
            charsToAppend.Add( "" + ConstantData.PetSCIIToChar[18].CharValue );
          }
        }
        else
        {
          if ( isReverse )
          {
            isReverse = false;
            charsToAppend.Add( "" + ConstantData.PetSCIIToChar[146].CharValue );
          }
        }
        newByte &= 0x7f;
        if ( newByte == 34 )
        {
          string replacement = "\"CHR$(34)CHR$(34)\"" + ConstantData.PetSCIIToChar[157].CharValue;
          // if " is the last char we don't need to place a second
          if ( i + 1 == data.Length )
          {
            replacement = "\"CHR$(34)";
          }
          charsToAppend.Add( replacement );
        }
        else
        {
          var key = ConstantData.AllPhysicalKeyInfos[MachineType.C64].FirstOrDefault( pk => pk.HasScreenCode && pk.ScreenCodeValue == newByte );
          if ( key != null )
          {
            charsToAppend.Add( "" + key.CharValue );
          }
        }
      }

      if ( charsToAppend.Any() )
      {
        bool    lineStarted = false;
        foreach ( var s in charsToAppend )
        {
          if ( !lineStarted )
          {
            lineStarted = true;
            startLength = sb.Length;
            sb.Append( startLine );
            startLine += stepLine;
            sb.Append( " PRINT\"" );
          }

          // don't make lines too long!
          if ( sb.Length - startLength + s.Length >= wrapCharCount - 1 )
          {
            // we need to break and start a new line
            sb.AppendLine( "\";" );

            startLength = sb.Length;
            sb.Append( startLine );
            startLine += stepLine;
            sb.Append( " PRINT\"" );
          }

          foreach ( var c in s )
          {
            sb.Append( c );
          }
          ++writtenBytes;
        }
        sb.AppendLine( "\"" );
      }

      return sb.ToString();
    }



  }
}
