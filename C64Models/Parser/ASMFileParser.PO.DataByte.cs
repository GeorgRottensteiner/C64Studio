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
    private void PODataByte( int LineIndex, List<Types.TokenInfo> lineTokenInfos, int StartIndex, int Count, Types.ASM.LineInfo info, Types.MacroInfo.PseudoOpType Type, GR.Collections.Map<byte, byte> TextMapping, bool AllowNeededExpression )
    {
      GR.Memory.ByteBuffer data = new GR.Memory.ByteBuffer();

      int commaCount = 0;
      int firstTokenIndex = StartIndex;
      int insideOtherBrackets = 0;
      for ( int tokenIndex = StartIndex; tokenIndex < StartIndex + Count; ++tokenIndex )
      {
        string token = lineTokenInfos[tokenIndex].Content;

        if ( IsOpeningBraceChar( token ) )
        {
          ++insideOtherBrackets;
          continue;
        }
        if ( IsClosingBraceChar( token ) )
        {
          --insideOtherBrackets;
          
        }
        if ( insideOtherBrackets > 0 )
        {
          continue;
        }

        if ( ( tokenIndex == StartIndex )
        &&   ( token == "#" ) )
        {
          // direct value?
          if ( ( lineTokenInfos.Count > 2 )
          &&   ( lineTokenInfos[2].Content != "#" )
          &&   ( lineTokenInfos[2].Content != "." ) )
          {
            // not a binary value
            continue;
          }
        }

        if ( token == "," )
        {
          ++commaCount;

          if ( tokenIndex - firstTokenIndex >= 1 )
          {
            int     numBytesGiven = 0;

            if ( ( tokenIndex - firstTokenIndex == 1 )
            &&   ( lineTokenInfos[firstTokenIndex].Content == "?" ) )
            {
              AddError( info.LineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, "Virtual value only allowed as single value. Expression:"
                           + TokensToExpression( lineTokenInfos, firstTokenIndex, tokenIndex - firstTokenIndex ),
                           lineTokenInfos[firstTokenIndex].StartPos,
                           lineTokenInfos[tokenIndex - 1].EndPos - lineTokenInfos[firstTokenIndex].StartPos + 1 );
            }

            if ( EvaluateTokens( LineIndex, lineTokenInfos, firstTokenIndex, tokenIndex - firstTokenIndex, TextMapping, out SymbolInfo byteValueSymbol, out numBytesGiven ) )
            {
              if ( byteValueSymbol.Type == SymbolInfo.Types.CONSTANT_STRING )
              {
                if ( ( byteValueSymbol.String.StartsWith( "\"" ) )
                &&   ( byteValueSymbol.String.Length > 1 )
                &&   ( byteValueSymbol.String.EndsWith( "\"" ) ) )
                {
                  string    textLiteral = byteValueSymbol.String.Substring( 1, byteValueSymbol.String.Length - 2 );

                  textLiteral = BasicFileParser.ReplaceAllMacrosByPETSCIICode( textLiteral, TextMapping, out bool hadError );
                  if ( hadError )
                  {
                    AddError( LineIndex, Types.ErrorCode.E3005_BASIC_UNKNOWN_MACRO, "Failed to evaluate " + textLiteral );
                    return;
                  }

                  if ( textLiteral.Length > 1 )
                  {
                    AddError( LineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, "More than one character in literal is not allowed in this context" );
                    return;
                  }

                  // a text
                  foreach ( char aChar in textLiteral )
                  {
                    // map to PETSCII!
                    data.AppendU8( (byte)aChar );
                  }
                }
                else
                {
                  string    textLiteral = byteValueSymbol.String;

                  textLiteral = BasicFileParser.ReplaceAllMacrosByPETSCIICode( textLiteral, TextMapping, out bool hadError );
                  if ( hadError )
                  {
                    AddError( LineIndex, Types.ErrorCode.E3005_BASIC_UNKNOWN_MACRO, "Failed to evaluate " + textLiteral );
                    return;
                  }

                  if ( textLiteral.Length > 1 )
                  {
                    AddError( LineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, "More than one character in literal is not allowed in this context" );
                    return;
                  }

                  // a text
                  foreach ( char aChar in textLiteral )
                  {
                    // map to PETSCII!
                    data.AppendU8( (byte)aChar );
                  }
                }
              }
              else
              {
                long   byteValue = byteValueSymbol.ToInteger();
                switch ( Type )
                {
                  case RetroDevStudio.Types.MacroInfo.PseudoOpType.LOW_BYTE:
                    byteValue = byteValue & 0x00ff;
                    break;
                  case RetroDevStudio.Types.MacroInfo.PseudoOpType.HIGH_BYTE:
                    byteValue = ( byteValue >> 8 ) & 0xff;
                    break;
                }
                if ( !ValidByteValue( byteValue ) )
                {
                  AddError( info.LineIndex, Types.ErrorCode.E1002_VALUE_OUT_OF_BOUNDS_BYTE, "Value out of bounds for byte, needs to be >= -128 and <= 255. Expression:"
                            + TokensToExpression( lineTokenInfos, firstTokenIndex, tokenIndex - firstTokenIndex ),
                            lineTokenInfos[firstTokenIndex].StartPos,
                            lineTokenInfos[tokenIndex - 1].EndPos - lineTokenInfos[firstTokenIndex].StartPos + 1 );
                }
                data.AppendU8( (byte)byteValue );
              }
            }
            else if ( AllowNeededExpression )
            {
              info.NeededParsedExpression = lineTokenInfos.GetRange( StartIndex, Count );
            }
            else
            {
              AddError( info.LineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Cannot evaluate expression " 
                        + TokensToExpression( lineTokenInfos, firstTokenIndex, tokenIndex - firstTokenIndex ),
                        lineTokenInfos[firstTokenIndex].StartPos,
                        lineTokenInfos[tokenIndex - 1].EndPos - lineTokenInfos[firstTokenIndex].StartPos + 1 );
            }
          }
          firstTokenIndex = tokenIndex + 1;
        }
      }
      if ( ( firstTokenIndex > 0 )
      &&   ( firstTokenIndex == lineTokenInfos.Count )
      &&   ( commaCount > 0 ) )
      {
        // last parameter has no value!
        AddError( info.LineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, "Missing value after last separator."
                          + TokensToExpression( lineTokenInfos, lineTokenInfos.Count - 1, 1 ),
                          lineTokenInfos[lineTokenInfos.Count - 1].StartPos,
                          lineTokenInfos[lineTokenInfos.Count - 1].Length );
      }
         
      if ( firstTokenIndex + 1 <= lineTokenInfos.Count )
      {
        int numBytesGiven = 0;

        if ( ( lineTokenInfos.Count - firstTokenIndex == 1 )
        &&   ( lineTokenInfos[firstTokenIndex].Content == "?" ) )
        {
          info.NumBytes = 1;
          return;
        }

        if ( EvaluateTokens( LineIndex, lineTokenInfos, firstTokenIndex, lineTokenInfos.Count - firstTokenIndex, TextMapping, out SymbolInfo byteValueSymbol, out numBytesGiven ) )
        {
          if ( byteValueSymbol.Type == SymbolInfo.Types.CONSTANT_STRING )
          {
            if ( ( byteValueSymbol.String.StartsWith( "\"" ) )
            &&   ( byteValueSymbol.String.Length > 1 )
            &&   ( byteValueSymbol.String.EndsWith( "\"" ) ) )
            {
              string    textLiteral = byteValueSymbol.String.Substring( 1, byteValueSymbol.String.Length - 2 );

              textLiteral = BasicFileParser.ReplaceAllMacrosByPETSCIICode( textLiteral, TextMapping, out bool hadError );
              if ( hadError )
              {
                AddError( LineIndex, Types.ErrorCode.E3005_BASIC_UNKNOWN_MACRO, "Failed to evaluate " + textLiteral );
                return;
              }

              if ( textLiteral.Length > 1 )
              {
                AddError( LineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, "More than one character in literal is not allowed in this context" );
                return;
              }

              // a text
              foreach ( char aChar in textLiteral )
              {
                // map to PETSCII!
                data.AppendU8( (byte)aChar );
              }
            }
            else
            {
              string    textLiteral = byteValueSymbol.String;

              textLiteral = BasicFileParser.ReplaceAllMacrosByPETSCIICode( textLiteral, TextMapping, out bool hadError );
              if ( hadError )
              {
                AddError( LineIndex, Types.ErrorCode.E3005_BASIC_UNKNOWN_MACRO, "Failed to evaluate " + textLiteral );
                return;
              }

              if ( textLiteral.Length > 1 )
              {
                AddError( LineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, "More than one character in literal is not allowed in this context" );
                return;
              }

              // a text
              foreach ( char aChar in textLiteral )
              {
                // map to PETSCII!
                data.AppendU8( (byte)aChar );
              }
            }
          }
          else
          {
            long byteValue = byteValueSymbol.ToInteger();
            switch ( Type )
            {
              case RetroDevStudio.Types.MacroInfo.PseudoOpType.LOW_BYTE:
                byteValue = byteValue & 0x00ff;
                break;
              case RetroDevStudio.Types.MacroInfo.PseudoOpType.HIGH_BYTE:
                byteValue = ( byteValue >> 8 ) & 0xff;
                break;
            }
            if ( !ValidByteValue( byteValue ) )
            {
              AddError( info.LineIndex, Types.ErrorCode.E1002_VALUE_OUT_OF_BOUNDS_BYTE, "Value out of bounds for byte, needs to be >= -128 and <= 255. Expression:"
                        + TokensToExpression( lineTokenInfos, firstTokenIndex, lineTokenInfos.Count - firstTokenIndex ),
                        lineTokenInfos[firstTokenIndex].StartPos,
                        lineTokenInfos[lineTokenInfos.Count - 1].EndPos - lineTokenInfos[firstTokenIndex].StartPos + 1 );
            }
            data.AppendU8( (byte)byteValue );
          }
        }
        else if ( AllowNeededExpression )
        {
          info.NeededParsedExpression = lineTokenInfos.GetRange( StartIndex, Count );

          AdjustLabelCasing( info.NeededParsedExpression );
        }
        else
        {
          AddError( info.LineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Cannot evaluate expression "
                    + TokensToExpression( lineTokenInfos, firstTokenIndex, lineTokenInfos.Count - firstTokenIndex ),
                    lineTokenInfos[firstTokenIndex].StartPos,
                    lineTokenInfos[lineTokenInfos.Count - 1].EndPos - lineTokenInfos[firstTokenIndex].StartPos + 1 );
        }
      }
      if ( ( ( AllowNeededExpression )
      &&     ( info.NeededParsedExpression == null ) )
      ||   ( !AllowNeededExpression ) )
      {
        info.LineData = data;
      }
      info.NumBytes = commaCount + 1;
    }





  }
}
