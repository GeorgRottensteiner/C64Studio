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
    private ParseLineResult POJumpTable( List<Types.TokenInfo> lineTokenInfos, int LineIndex, int StartIndex, int Count, Types.ASM.LineInfo info, String parseLine, bool AllowNeededExpression, out int lineSizeInBytes )
    {
      GR.Memory.ByteBuffer data = new GR.Memory.ByteBuffer();

      if ( !ParseLineInParameters( lineTokenInfos, StartIndex, Count, LineIndex, false, out List<List<TokenInfo>> paramList ) )
      {
        lineSizeInBytes = 0;
        return ParseLineResult.ERROR_ABORT;
      }

      // get value of jump list name
      if ( !m_ASMFileInfo.Labels.ContainsKey( info.CheapLabelZone ) )
      {
        AddError( LineIndex,
                  Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION,
                  $"Failed to evaluate value for {info.CheapLabelZone}" );
        lineSizeInBytes = 0;
        return ParseLineResult.ERROR_ABORT;
      }

      var zoneToken = m_ASMFileInfo.Labels[info.CheapLabelZone];

      int paramIndex = 0;
      foreach ( var parms in paramList )
      {
        int     numBytesGiven = 0;

        if ( parms.Count > 1 )
        {
          AddError( LineIndex, 
                    Types.ErrorCode.E1009_INVALID_VALUE, 
                    "Jump Table entries allow only a named label",
                    parms[0].StartPos,
                    parms[parms.Count - 1].EndPos - parms[0].StartPos + 1 );
          lineSizeInBytes = 0;
          return ParseLineResult.ERROR_ABORT;
        }
        if ( !IsTokenLabel( parms[0].Type ) )
        {
          AddError( LineIndex,
                    Types.ErrorCode.E1009_INVALID_VALUE,
                    "Jump Table entries allow only a named label",
                    parms[0].StartPos,
                    parms[parms.Count - 1].EndPos - parms[0].StartPos + 1 );
          lineSizeInBytes = 0;
          return ParseLineResult.ERROR_ABORT;
        }

        // get actual value of list entry
        var newList = new List<TokenInfo>();
            newList.Add( new TokenInfo()
            {
              Content = info.CheapLabelZone,
              Type = TokenInfo.TokenType.LABEL_GLOBAL
            } 
          );

        string realLabel = info.CheapLabelZone + "." + parms[0].Content;
        if ( !AllowNeededExpression )
        {
          realLabel = parms[0].Content;
        }
        newList[0].Content = realLabel;
        if ( EvaluateTokens( LineIndex, parms, 0, parms.Count, out var wordValueSymbol, out numBytesGiven ) )
        {
          // add label with offset
          if ( AllowNeededExpression )
          {
            ushort  labelValue = (ushort)( info.AddressStart + paramIndex * 2 - zoneToken.AddressOrValue );
            AddLabel( realLabel,
                      labelValue,
                      LineIndex,
                      info.Zone,
                      parms[0].StartPos,
                      parms[parms.Count - 1].EndPos - parms[0].StartPos + 1 );
          }

          // add actual list entry value
          data.AppendU16( (ushort)wordValueSymbol.ToInt32() );
        }
        else if ( AllowNeededExpression )
        {
          AddUnparsedLabel( realLabel,
                            $"{info.AddressStart + paramIndex * 2}-{info.CheapLabelZone}",
                            LineIndex );
          info.NeededParsedExpression = parms;
        }
        else
        {
          AddError( info.LineIndex,
                      Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION,
                      "Failed to evaluate expression " + TokensToExpression( parms ),
                      parms[0].StartPos,
                      parms[parms.Count - 1].EndPos - parms[0].StartPos + 1 );
        }
        /*
        if ( !AllowNeededExpression )
        {
          realLabel = parms[0].Content;
        }
        var newList = new List<TokenInfo>( parms );
        newList[0].Content = realLabel;
        if ( EvaluateTokens( LineIndex, newList, 0, newList.Count, TextMapping, out SymbolInfo wordValueSymbol, out numBytesGiven ) )
        {
          if ( AllowNeededExpression )
          {
            AddUnparsedLabel( info.CheapLabelZone + "." + parms[0].Content,
                              $"{info.AddressStart + paramIndex * 2}-{info.CheapLabelZone}",
                              LineIndex );
          }
          wordValue = wordValueSymbol.ToInt32();
          if ( !ValidWordValue( wordValue ) )
          {
            AddError( info.LineIndex,
                      Types.ErrorCode.E1003_VALUE_OUT_OF_BOUNDS_WORD,
                      "Value out of bounds for word, needs to be >= -32768 and <= 65535. Expression:" + TokensToExpression( parms ),
                      parms[0].StartPos,
                      parms[parms.Count - 1].EndPos - parms[0].StartPos + 1 );
          }
          else
          {
            AddLabel( parms[0].Content,
                      (ushort)wordValue,
                      LineIndex,
                      info.Zone, 
                      parms[0].StartPos,
                      parms[parms.Count - 1].EndPos - parms[0].StartPos + 1 );
          }
          data.AppendU16( (ushort)wordValue );
        }
        else if ( AllowNeededExpression )
        {
          info.NeededParsedExpression = newList;
        }
        else
        {
          AddError( info.LineIndex,
                      Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION,
                      "Failed to evaluate expression " + TokensToExpression( parms ),
                      parms[0].StartPos,
                      parms[parms.Count - 1].EndPos - parms[0].StartPos + 1 );
        }*/

        ++paramIndex;
      }


      // TODO - this is a ugly check if there was an error or not
      if ( ( ( AllowNeededExpression )
      &&     ( info.NeededParsedExpression == null ) )
      ||   ( !AllowNeededExpression ) )
      {
        info.LineData = data;
      }
      info.NumBytes   = 2 * paramList.Count;
      info.Line       = parseLine;
      lineSizeInBytes = info.NumBytes;
      return ParseLineResult.OK;
    }



  }
}
