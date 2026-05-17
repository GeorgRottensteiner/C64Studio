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
    private bool POIncludeMediaCharset( int lineIndex, bool binary, string subFilename, string method, List<List<TokenInfo>> paramTokens, string labelPrefix, out ByteBuffer dataToInclude, out string[] replacementLines )
    {
      replacementLines  = null;
      dataToInclude     = null;
      if ( !binary )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Assembly include for charsets is not supported" );
        return false;
      }

      // char set file
      //  char,index,count
      if ( paramTokens.Count > 4 )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected <Char>[,<Index>[,<Count>]]" );
        return false;
      }
      if ( method != "CHAR" )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Unknown method '" + method + "', supported values for this file name are CHAR" );
        return false;
      }
      int startIndex = 0;
      int numChars = 256;

      if ( ( paramTokens.Count >= 3 )
      &&   ( EvaluateTokens( lineIndex, paramTokens[2], out SymbolInfo startIndexSymbol ) ) )
      {
        startIndex = (int)startIndexSymbol.ToInteger();
      }
      if ( ( paramTokens.Count >= 4 )
      &&   ( EvaluateTokens( lineIndex, paramTokens[3], out SymbolInfo numCharsSymbol ) ) )
      {
        numChars = (int)numCharsSymbol.ToInteger();
      }
      try
      {
        dataToInclude = GR.IO.File.ReadAllBytes( subFilename );
        if ( dataToInclude == null )
        {
          AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + subFilename );
          return false;
        }
      }
      catch ( System.IO.IOException )
      {
        AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read file " + subFilename );
        return false;
      }
      if ( ( startIndex < 0 )
      ||   ( startIndex * 8 >= dataToInclude.Length ) )
      {
        AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid start index " + startIndex );
        return false;
      }
      if ( ( numChars <= 0 )
      ||   ( ( startIndex + numChars ) * 8 > dataToInclude.Length ) )
      {
        AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid char count " + numChars );
        return false;
      }
      dataToInclude = dataToInclude.SubBuffer( startIndex * 8, numChars * 8 );
      return true;
    }


  }
}
