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
    private bool POIncludeMediaSpriteset( int lineIndex, bool binary, string subFilename, string method, List<List<TokenInfo>> paramTokens, string labelPrefix, out ByteBuffer dataToInclude, out string[] replacementLines )
    {
      replacementLines  = null;
      dataToInclude     = null;

      if ( !binary )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Assembly include for sprites is not supported" );
        return false;
      }

      if ( ( method != "SPRITE" )
      &&   ( method != "SPRITEDATA" ) )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Unknown method '" + method + "', supported values for this file name are SPRITE or SPRITEDATA" );
        return false;
      }

      if ( ( method == "SPRITE" )
      &&   ( paramTokens.Count > 4 ) )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected <Sprite>[,<Index>[,<Count>]]" );
        return false;
      }
      if ( ( method == "SPRITEDATA" )
      &&   ( paramTokens.Count != 6 ) )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected <Spritedata>,<Index>,<Count>,<Offset>,<NumBytes>" );
        return false;
      }

      if ( method == "SPRITE" )
      {
        int startIndex = 0;
        int numSprites = -1;

        if ( ( paramTokens.Count >= 3 )
        &&   ( EvaluateTokens( lineIndex, paramTokens[2], out SymbolInfo startIndexSymbol ) ) )
        {
          startIndex = (int)startIndexSymbol.ToInteger();
        }
        if ( ( paramTokens.Count >= 4 )
        &&   ( EvaluateTokens( lineIndex, paramTokens[3], out SymbolInfo numSpritesSymbol ) ) )
        {
          numSprites = (int)numSpritesSymbol.ToInteger();
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
        if ( numSprites == -1 )
        {
          numSprites = (int)dataToInclude.Length / 64;
        }
        if ( ( startIndex < 0 )
        ||   ( startIndex * 64 >= dataToInclude.Length ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid start index " + startIndex );
          return false;
        }
        if ( ( numSprites <= 0 )
        ||   ( ( startIndex + numSprites ) * 64 > dataToInclude.Length ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid sprite count " + numSprites );
          return false;
        }
        dataToInclude = dataToInclude.SubBuffer( startIndex * 64, numSprites * 64 );
      }
      else if ( method == "SPRITEDATA" )
      {
        int   startIndex = 0;
        int   numSprites = -1;
        int   offsetBytes = 0;
        int   numBytes = numSprites * 64;

        if ( EvaluateTokens( lineIndex, paramTokens[2], out SymbolInfo startIndexSymbol ) )
        {
          startIndex = (int)startIndexSymbol.ToInteger();
        }
        if ( EvaluateTokens( lineIndex, paramTokens[3], out SymbolInfo numSpritesSymbol ) )
        {
          numSprites = (int)numSpritesSymbol.ToInteger();
        }
        if ( EvaluateTokens( lineIndex, paramTokens[4], out SymbolInfo offsetBytesSymbol ) )
        {
          offsetBytes = (int)offsetBytesSymbol.ToInteger();
        }
        if ( EvaluateTokens( lineIndex, paramTokens[5], out SymbolInfo numBytesSymbol ) )
        {
          numBytes = (int)numBytesSymbol.ToInteger();
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
        if ( numSprites == -1 )
        {
          numSprites = (int)dataToInclude.Length / 64;
        }
        if ( ( startIndex < 0 )
        ||   ( startIndex * 64 >= dataToInclude.Length ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid start index " + startIndex );
          return false;
        }
        if ( ( numSprites <= 0 )
        ||   ( ( startIndex + numSprites ) * 64 > dataToInclude.Length ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid sprite count " + numSprites );
          return false;
        }
        dataToInclude = dataToInclude.SubBuffer( startIndex * 64, numSprites * 64 );
        if ( ( offsetBytes >= dataToInclude.Length )
        ||   ( offsetBytes < 0 ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid data offset " + offsetBytes );
          return false;
        }
        if ( ( offsetBytes + numBytes > dataToInclude.Length )
        ||   ( numBytes <= 0 ) )
        {
          AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid data size " + numBytes );
          return false;
        }
        dataToInclude = dataToInclude.SubBuffer( offsetBytes, numBytes );
      }
      return true;
    }


  }
}
