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

      if ( !ParseLineInParameters( lineTokenInfos, StartIndex, Count, LineIndex, false, out List<List<TokenInfo>> paramList ) )
      {
        return;
      }

      foreach ( var parms in paramList )
      {
        int     numBytesGiven = 0;

        if ( ( parms.Count  == 1 )
        &&   ( parms[0].Content == "?" ) )
        {
          if ( paramList.Count == 1 )
          {
            info.NumBytes = 1;
            return;
          }
          AddError( info.LineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, "Virtual value only allowed as single value. Expression:"
                       + TokensToExpression( parms ),
                       parms[0].StartPos,
                       parms[parms.Count - 1].EndPos - parms[0].StartPos + 1 );
        }

        if ( EvaluateTokens( LineIndex, parms, 0, parms.Count, TextMapping, out SymbolInfo byteValueSymbol, out numBytesGiven ) )
        {
          if ( byteValueSymbol.Type == SymbolInfo.Types.CONSTANT_STRING )
          {
            string    textLiteral = byteValueSymbol.ToString();

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
                        + TokensToExpression( parms ),
                        parms[0].StartPos,
                        parms[parms.Count - 1].EndPos - parms[0].StartPos + 1 );
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
                    + TokensToExpression( parms ),
                    parms[0].StartPos,
                    parms[parms.Count - 1].EndPos - parms[0].StartPos + 1 );
        }
      }

      if ( ( ( AllowNeededExpression )
      &&     ( info.NeededParsedExpression == null ) )
      ||   ( !AllowNeededExpression ) )
      {
        info.LineData = data;
      }
      info.NumBytes = paramList.Count;
    }





  }
}
