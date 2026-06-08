using GR.Memory;
using RetroDevStudio.Formats;
using RetroDevStudio.Parser;
using RetroDevStudio.Parser.BASIC;
using RetroDevStudio.Types;
using RetroDevStudio.Types.ASM;
using System;
using System.Collections.Generic;



namespace RetroDevStudio.Parser
{
  public partial class ASMFileParser : ParserBase
  {
    private ParseLineResult PODataDWord( List<Types.TokenInfo> lineTokenInfos, int LineIndex, int StartIndex, int Count, Types.ASM.LineInfo info, String parseLine, bool AllowNeededExpression, bool LittleEndian, out int lineSizeInBytes )
    {
      GR.Memory.ByteBuffer data = new GR.Memory.ByteBuffer();

      if ( !ParseLineInParameters( lineTokenInfos, StartIndex, Count, LineIndex, false, out List<List<TokenInfo>> paramList ) )
      {
        lineSizeInBytes = 0;
        return ParseLineResult.ERROR_ABORT;
      }

      foreach ( var parms in paramList )
      {
        int     wordValue = -1;
        int     numBytesGiven = 0;

        if ( ( parms.Count == 1 )
        &&   ( parms[0].Content == "?" ) )
        {
          if ( paramList.Count == 1 )
          {
            info.NumBytes     = 4;
            lineSizeInBytes   = 4;
            return ParseLineResult.OK;
          }
          AddError( info.LineIndex, Types.ErrorCode.E1000_SYNTAX_ERROR, "Virtual value only allowed as single value. Expression:"
                       + TokensToExpression( parms ),
                       parms[0].StartPos,
                       parms[parms.Count - 1].EndPos - parms[0].StartPos + 1 );
        }

        if ( EvaluateTokens( LineIndex, parms, 0, parms.Count, out SymbolInfo wordValueSymbol, out numBytesGiven ) )
        {
          if ( wordValueSymbol.Type == SymbolInfo.Types.CONSTANT_STRING )
          {
            string    textLiteral = wordValueSymbol.ToString();

            textLiteral = BasicFileParser.ReplaceAllMacrosByPETSCIICode( textLiteral, info.LineCodeMapping, MachineType.C64, out bool hadError );
            if ( hadError )
            {
              AddError( LineIndex, Types.ErrorCode.E3005_BASIC_UNKNOWN_MACRO, "Failed to evaluate " + textLiteral );
              lineSizeInBytes = 0;
              return ParseLineResult.ERROR_ABORT;
            }

            // a text
            foreach ( char aChar in textLiteral )
            {
              // map to PETSCII!
              if ( LittleEndian )
              {
                data.AppendU32( (byte)aChar );
              }
              else
              {
                data.AppendU32NetworkOrder( (byte)aChar );
              }
            }
          }
          else
          {
            wordValue = wordValueSymbol.ToInt32();
            if ( !ValidDWordValue( wordValue ) )
            {
              AddError( info.LineIndex,
                        Types.ErrorCode.E1003_VALUE_OUT_OF_BOUNDS_WORD,
                        "Value out of bounds for word, needs to be >= −2147483648 and <= 4294967295. Expression:" + TokensToExpression( parms ),
                        parms[0].StartPos,
                        parms[parms.Count - 1].EndPos - parms[0].StartPos + 1 );
            }
            if ( LittleEndian )
            {
              data.AppendU32( (uint)wordValue );
            }
            else
            {
              data.AppendU32NetworkOrder( (uint)wordValue );
            }
          }
        }
        else if ( AllowNeededExpression )
        {
          info.NeededParsedExpression = lineTokenInfos.GetRange( StartIndex, Count );
        }
        else
        {
          AddError( info.LineIndex,
                      Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION,
                      "Failed to evaluate expression " + TokensToExpression( parms ),
                      parms[0].StartPos,
                      parms[parms.Count - 1].EndPos - parms[0].StartPos + 1 );
        }
      }

      // TODO - this is a ugly check if there was an error or not
      if ( ( ( AllowNeededExpression )
      &&     ( info.NeededParsedExpression == null ) )
      ||   ( !AllowNeededExpression ) )
      {
        info.LineData = data;
      }
      info.NumBytes   = 4 * paramList.Count;
      info.Line       = parseLine;
      lineSizeInBytes = info.NumBytes;
      return ParseLineResult.OK;
    }



  }
}
