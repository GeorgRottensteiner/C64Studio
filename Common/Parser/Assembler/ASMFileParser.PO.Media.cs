using GR.Memory;
using RetroDevStudio.Formats;
using RetroDevStudio.Parser;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;

namespace RetroDevStudio.Parser
{
  public partial class ASMFileParser : ParserBase
  {
    private bool POIncludeMedia( List<Types.TokenInfo> lineTokenInfos, int lineIndex, bool Binary, Types.ASM.LineInfo info, string ParentFilename, out int lineSizeInBytes, out string[] ReplacementLines )
    {
      ReplacementLines = null;
      lineSizeInBytes = 0;

      ParseLineInParameters( lineTokenInfos, 1, lineTokenInfos.Count - 1, lineIndex, true, out List<List<Types.TokenInfo>> paramTokens );

      if ( ( paramTokens.Count > 8 )
      ||   ( paramTokens.Count < 2 ) )
      {
        if ( Binary )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected !media <Filename>[,<MediaParams>...]" );
        }
        else
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected !mediasrc <Filename>,SrcLabelPrefix[,<MediaParams>...]" );
        }
        return false;
      }

      // validate filename
      if ( ( paramTokens[0].Count != 1 )
      ||   ( paramTokens[0][0].Length < 2 ) )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected string literal as filename" );
        return false;
      }
      if ( !IsInQuotes( paramTokens[0][0].Content ) )
      {
        AddError( lineIndex, Types.ErrorCode.E1307_FILENAME_INCOMPLETE, "Expected proper file name between apostrophes" );
        return false;
      }
      string    subFilename = paramTokens[0][0].Content.Substring( 1, paramTokens[0][0].Length - 2 );
      int       includeMethodParamIndex = 1;
      string    labelPrefix = "";

      subFilename = BuildFullPath( GR.Path.GetDirectoryName( ParentFilename ), subFilename );

      // !mediasrc includes a label prefix parameter, but !media does not, so we need to adjust the index of the method parameter accordingly
      if ( !Binary )
      {
        // !media src <Filename>,<LabelPrefix>[,more params]
        if ( paramTokens.Count < 3 )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected !mediasrc <Filename>,SrcLabelPrefix[,<MediaParams>...]" );
          return false;
        }
        // label prefix 
        if ( ( paramTokens[1].Count > 1 )
        ||   ( ( paramTokens[1].Count == 1 )
        &&     ( paramTokens[1][0].Type != RetroDevStudio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
        &&     ( paramTokens[1][0].Type != RetroDevStudio.Types.TokenInfo.TokenType.LITERAL_STRING )
        &&     ( paramTokens[1][0].Type != RetroDevStudio.Types.TokenInfo.TokenType.LABEL_LOCAL ) ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected proper global or local label prefix, or empty for no prefix" );
          return false;
        }
        if ( paramTokens[1].Count == 1 )
        {
          labelPrefix = ASMFileParser.DeQuote( paramTokens[1][0].Content );
        }
        paramTokens.RemoveAt( 1 );
      }

      string    extension = GR.Path.GetExtension( subFilename ).ToUpper();
      if ( ( paramTokens[includeMethodParamIndex].Count != 1 )
      ||   ( ( paramTokens[includeMethodParamIndex][0].Type != RetroDevStudio.Types.TokenInfo.TokenType.LABEL_GLOBAL ) 
      &&     ( paramTokens[includeMethodParamIndex][0].Type != RetroDevStudio.Types.TokenInfo.TokenType.LITERAL_STRING ) ) )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected known import method" );
        return false;
      }
      string    method = DeQuote( paramTokens[includeMethodParamIndex][0].Content.ToUpper() );

      var dataToInclude = new GR.Memory.ByteBuffer();


      if ( extension == ".CHR" )
      {
        if ( !POIncludeMediaCharset( lineIndex, Binary, subFilename, method, paramTokens, labelPrefix, out dataToInclude, out ReplacementLines ) )
        {
          return false;
        }
      }
      else if ( extension == ".CHARSETPROJECT" )
      {
        if ( !POIncludeMediaCharsetProject( lineIndex, Binary, subFilename, method, paramTokens, labelPrefix, out dataToInclude, out ReplacementLines ) )
        {
          return false;
        }
      }
      else if ( extension == ".VALUETABLEPROJECT" )
      {
        if ( !POIncludeMediaValueTableProject( lineIndex, Binary, subFilename, method, paramTokens, labelPrefix, out dataToInclude, out ReplacementLines ) )
        {
          return false;
        }
      }
      else if ( extension == ".SPR" )
      {
        if ( !POIncludeMediaSpriteset( lineIndex, Binary, subFilename, method, paramTokens, labelPrefix, out dataToInclude, out ReplacementLines ) )
        {
          return false;
        }
      }
      else if ( extension == ".SPRITEPROJECT" )
      {
        if ( !POIncludeMediaSpriteProject( lineIndex, Binary, subFilename, method, paramTokens, labelPrefix, out dataToInclude, out ReplacementLines ) )
        {
          return false;
        }
      }
      else if ( extension == ".SPD" )
      {
        if ( !POIncludeMediaSpritePad( lineIndex, Binary, subFilename, method, paramTokens, labelPrefix, out dataToInclude, out ReplacementLines ) )
        {
          return false;
        }

      }
      else if ( extension == ".CHARSCREEN" )
      {
        if ( !POIncludeMediaCharscreen( lineIndex, Binary, subFilename, method, paramTokens, labelPrefix, out dataToInclude, out ReplacementLines ) )
        {
          return false;
        }
      }
      else if ( extension == ".GRAPHICSCREEN" )
      {
        if ( !POIncludeMediaGraphicScreen( lineIndex, Binary, subFilename, method, paramTokens, labelPrefix, out dataToInclude, out ReplacementLines ) )
        {
          return false;
        }
      }
      else if ( extension == ".MAPPROJECT" )
      {
        if ( !POIncludeMediaMapProject( lineIndex, Binary, subFilename, method, paramTokens, labelPrefix, out dataToInclude, out ReplacementLines ) )
        {
          return false;
        }
      }
      else
      {
        AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E2002_UNSUPPORTED_FILE_TYPE, "Unknown file type" );
        return false;
      }

      ExternallyIncludedFiles.Add( subFilename );

      if ( !Binary )
      {
        return true;
      }

      info.LineData = dataToInclude;
      info.NumBytes = (int)info.LineData.Length;
      lineSizeInBytes = (int)dataToInclude.Length;
      return true;
    }

    

    private string OptionalPrefix( string Prefix, string Separator, string Content )
    {
      if ( string.IsNullOrEmpty( Prefix ) )
      {
        return Content;
      }
      return Prefix + Separator + Content;
    }



  }
}
