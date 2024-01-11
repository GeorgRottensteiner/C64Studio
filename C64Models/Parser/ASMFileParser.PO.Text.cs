using GR.Collections;
using GR.Memory;
using RetroDevStudio.Formats;
using RetroDevStudio.Parser;
using RetroDevStudio.Types;
using RetroDevStudio.Types.ASM;
using System;
using System.Collections.Generic;
using System.Linq;
using Tiny64;

namespace RetroDevStudio.Parser
{
  public partial class ASMFileParser : ParserBase
  {
    private ParseLineResult POText( int LineIndex, List<Types.TokenInfo> lineTokenInfos, Types.ASM.LineInfo info, String parseLine, GR.Collections.Map<byte, byte> TextMapping, out int lineSizeInBytes )
    {
      lineSizeInBytes = 0;
      
      if ( !ParseLineInParameters( lineTokenInfos, 1, lineTokenInfos.Count - 1, LineIndex, true, out List<List<TokenInfo>> lineParams ) )
      {
        return ParseLineResult.ERROR_ABORT;
      }

      int numBytes = 0;

      foreach ( var paramGroup in lineParams )
      {
        foreach ( var token in paramGroup )
        {
          if ( token.Type == TokenInfo.TokenType.LITERAL_STRING )
          {
            numBytes += ActualTextTokenLength( token );
          }
          else
          {
            // everything else is a expression resulting in a single byte
            ++numBytes;
            break;
          }
        }
      }

      info.NumBytes               = numBytes;
      info.Line                   = parseLine;
      info.NeededParsedExpression = lineTokenInfos.GetRange( 1, lineTokenInfos.Count - 1 );
      info.LineCodeMapping        = TextMapping;
      lineSizeInBytes             = info.NumBytes;

      return ParseLineResult.OK;
    }



    private ParseLineResult POTextXor( int LineIndex, List<Types.TokenInfo> lineTokenInfos, Types.ASM.LineInfo info, String parseLine, GR.Collections.Map<byte, byte> TextMapping, out int lineSizeInBytes )
    {
      lineSizeInBytes = 0;

      if ( !ParseLineInParameters( lineTokenInfos, 1, lineTokenInfos.Count - 1, LineIndex, true, out List<List<TokenInfo>> lineParams ) )
      {
        return ParseLineResult.ERROR_ABORT;
      }

      // first is the XOR value
      int numTokensInFirstParam = lineParams[0].Count;
      lineParams.RemoveAt( 0 );

      int numBytes = 0;

      foreach ( var paramGroup in lineParams )
      {
        foreach ( var token in paramGroup )
        {
          if ( token.Type == TokenInfo.TokenType.LITERAL_STRING )
          {
            numBytes += ActualTextTokenLength( token );
          }
          else
          {
            // everything else is a single char
            ++numBytes;
          }
        }
      }

      info.NumBytes               = numBytes;
      info.Line                   = parseLine;
      info.LineCodeMapping        = TextMapping;
      info.NeededParsedExpression = lineTokenInfos.GetRange( numTokensInFirstParam + 1 + 1, lineTokenInfos.Count - numTokensInFirstParam - 1 - 1 );
      lineSizeInBytes             = info.NumBytes;

      return ParseLineResult.OK;
    }






  }
}
