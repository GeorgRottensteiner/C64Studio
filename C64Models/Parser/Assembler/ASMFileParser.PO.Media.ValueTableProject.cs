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
    private bool POIncludeMediaValueTableProject( int lineIndex, bool binary, string subFilename, string method, List<List<TokenInfo>> paramTokens, string labelPrefix, out ByteBuffer dataToInclude, out string[] replacementLines )
    {
      replacementLines  = null;
      dataToInclude     = null;

      if ( !binary )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Assembly include for value table projects is not supported" );
        return false;
      }

      // value table project file
      if ( paramTokens.Count > 4 )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Pseudo op not formatted as expected. Expected <Data>[,<Offset>[,<Bytes>]]" );
        return false;
      }
      if ( method != "DATA" )
      {
        AddError( lineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Unknown method '" + method + "', supported values for this file name are DATA" );
        return false;
      }
      int   numBytes = -1;
      int   startIndex = 0;
      if ( ( paramTokens.Count >= 3 )
      &&   ( EvaluateTokens( lineIndex, paramTokens[2], out SymbolInfo startIndexSymbol ) ) )
      {
        startIndex = (int)startIndexSymbol.ToInteger();
      }
      if ( ( paramTokens.Count >= 4 )
      &&   ( EvaluateTokens( lineIndex, paramTokens[3], out SymbolInfo numBytesSymbol ) ) )
      {
        numBytes = (int)numBytesSymbol.ToInteger();
      }
      Formats.ValueTableProject   valueTableProject = new Formats.ValueTableProject();

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
      if ( !valueTableProject.ReadFromBuffer( dataToInclude ) )
      {
        AddError( lineIndex, Types.ErrorCode.E2001_FILE_READ_ERROR, "Could not read value table project file " + subFilename );
        return false;
      }
      dataToInclude = valueTableProject.GenerateTableData();
      if ( ( startIndex < 0 )
      ||   ( startIndex >= dataToInclude.Length ) )
      {
        AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid start index " + startIndex );
        return false;
      }
      if ( numBytes== -1 )
      {
        numBytes = (int)dataToInclude.Length;
      }
      if ( ( numBytes <= 0 )
      || ( ( startIndex + numBytes ) > dataToInclude.Length ) )
      {
        AddError( lineIndex, Types.ErrorCode.E1009_INVALID_VALUE, "Invalid byte count " + numBytes );
        return false;
      }
      dataToInclude = dataToInclude.SubBuffer( 0, numBytes );
      return true;
    }


  }
}
