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
      int groupIndex = 0;
      foreach ( var paramGroup in lineParams )
      {
        if ( ( groupIndex + 1 == lineParams.Count )
        &&   ( !paramGroup.Any() ) )
        {
          continue;
        }
        bool  isPotentiallyExpandable = false;
        if ( paramGroup.Any( t => ( t.Type == TokenInfo.TokenType.LITERAL_STRING ) ) )
        {
          isPotentiallyExpandable = true;
        }
        if ( paramGroup.Any( t => ( ( IsKnownLabel( t ) ) && ( IsTokenLabel( t.Type ) ) && ( IsLabelString( t ) ) ) ) )
        {
          isPotentiallyExpandable = true;
        }
        if ( !isPotentiallyExpandable )
        {
          // everything else is a expression resulting in a single byte
          ++numBytes;
        }
        else
        {
          int tokenIndex = 0;
          foreach ( var token in paramGroup )
          {
            if ( token.Type == TokenInfo.TokenType.LITERAL_STRING )
            {
              numBytes += ActualTextTokenLength( token );
            }
            else if ( IsTokenLabel( token.Type ) )
            {
              if ( ( IsKnownLabel( token ) )
              &&   ( IsLabelString( token ) ) )
              {
                // replace directly
                var evaluatedString = EvaluateAsText( _ParseContext.LineIndex, paramGroup, tokenIndex, 1, _ParseContext.CurrentTextMapping );
                numBytes += evaluatedString.Length;

                int origIndex = lineTokenInfos.IndexOf( token );
                lineTokenInfos[origIndex].Type = TokenInfo.TokenType.LITERAL_STRING;
                lineTokenInfos[origIndex].Content = '"' + evaluatedString + '"';
                lineTokenInfos[origIndex].Length = evaluatedString.Length + 2;
              }
              else
              {
                ++numBytes;
              }
            }
            else
            {
              // everything else is a expression resulting in a single byte
              ++numBytes;
            }
            ++tokenIndex;
          }
        }
        ++groupIndex;
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
        int tokenIndex = 0;
        foreach ( var token in paramGroup )
        {
          if ( token.Type == TokenInfo.TokenType.LITERAL_STRING )
          {
            numBytes += ActualTextTokenLength( token );
          }
          else if ( IsTokenLabel( token.Type ) )
          {
            if ( IsLabelString( token ) )
            {
              var evaluatedString = EvaluateAsText( _ParseContext.LineIndex, paramGroup, tokenIndex, 1, _ParseContext.CurrentTextMapping );
              numBytes += evaluatedString.Length;
            }
            else if ( !IsKnownLabel( token ) )
            {
              AddError( _ParseContext.LineIndex, ErrorCode.E1010_UNKNOWN_LABEL, $"Cannot evaluate {token.Content}, must be evaluable directly" );
              return ParseLineResult.ERROR_ABORT;
            }
          }
          else
          {
            // everything else is a single char
            ++numBytes;
          }
          ++tokenIndex;
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
