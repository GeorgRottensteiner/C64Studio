using GR.Memory;
using RetroDevStudio.Formats;
using RetroDevStudio.Parser;
using RetroDevStudio.Types;
using RetroDevStudio.Types.ASM;
using System;
using System.Collections.Generic;



namespace RetroDevStudio.Parser
{
  public partial class ASMFileParser : ParserBase
  {
    private ParseLineResult POACMEHex( LineInfo info, List<TokenInfo> lineTokenInfos, int lineIndex, out int lineSizeInBytes )
    {
      lineSizeInBytes = 0;
      info.LineData = new GR.Memory.ByteBuffer();
      for ( int i = 1; i < lineTokenInfos.Count; ++i )
      {
        Types.TokenInfo tokenHex = lineTokenInfos[i];

        string    hexData = DeQuote( tokenHex.Content );

        if ( ( hexData.Length % 2 ) != 0 )
        {
          AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1000_SYNTAX_ERROR, "Malformed hex data" );

          return ParseLineResult.RETURN_NULL;
        }
        if ( !info.LineData.AppendHex( hexData ) )
        {
          AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1000_SYNTAX_ERROR, "Malformed hex data" );

          return ParseLineResult.RETURN_NULL;
        }
      }
      info.NumBytes = (int)info.LineData.Length;
      lineSizeInBytes = (int)info.LineData.Length;

      return ParseLineResult.OK;
    }


  }
}
