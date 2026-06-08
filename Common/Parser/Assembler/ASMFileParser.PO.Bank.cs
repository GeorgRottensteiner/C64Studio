using GR.Memory;
using RetroDevStudio.Formats;
using RetroDevStudio.Parser;
using RetroDevStudio.Types;
using RetroDevStudio.Types.ASM;
using System;
using System.Collections.Generic;
using Tiny64;

namespace RetroDevStudio.Parser
{
  public partial class ASMFileParser : ParserBase
  {
    private void POBank( List<TokenInfo> lineTokenInfos, LineInfo info, int sizeInBytes, ref int lineSizeInBytes )
    {
      if ( ScopeInsideMacroDefinition() )
      {
        return;
      }
      // !BANK no,size

      int paramPos = 0;
      List<Types.TokenInfo> paramsNo = new List<Types.TokenInfo>();
      List<Types.TokenInfo> paramsSize = new List<Types.TokenInfo>();
      for ( int i = 1; i < lineTokenInfos.Count; ++i )
      {
        if ( lineTokenInfos[i].Content == "," )
        {
          ++paramPos;
          if ( paramPos >= 2 )
          {
            AddError( _ParseContext.LineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Macro not formatted as expected. Expected !bank <Number>,<Size>", lineTokenInfos[i].StartPos, lineTokenInfos[i].Length );
            break;
          }
        }
        else
        {
          switch ( paramPos )
          {
            case 0:
              paramsNo.Add( lineTokenInfos[i] );
              break;
            case 1:
              paramsSize.Add( lineTokenInfos[i] );
              break;
          }
        }
      }
      if ( ( paramPos == 0 )
      ||   ( paramPos > 1 ) )
      {
        AddError( _ParseContext.LineIndex, Types.ErrorCode.E1302_MALFORMED_MACRO, "Macro not formatted as expected. Expected !bank <Number>[,<Size>]" );
      }
      else
      {
        int number = -1;
        int size = -1;
        SymbolInfo sizeSymbol = null;
        if ( !EvaluateTokens( _ParseContext.LineIndex, paramsNo, out SymbolInfo numberSymbol ) )
        {
          string expressionCheck = TokensToExpression( paramsNo );

          AddError( _ParseContext.LineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Could not evaluate expression " + expressionCheck );
        }
        else if ( ( paramsSize.Count > 0 )
        && ( !EvaluateTokens( _ParseContext.LineIndex, paramsSize, out sizeSymbol ) ) )
        {
          string expressionCheck = TokensToExpression( paramsNo );

          AddError( _ParseContext.LineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Could not evaluate expression " + expressionCheck );
        }
        else
        {
          number = numberSymbol.ToInt32();
          size = sizeSymbol.ToInt32();
          if ( m_ASMFileInfo.Banks.Count > 0 )
          {
            // fill from previous bank
            Types.ASM.BankInfo lastBank = m_ASMFileInfo.Banks[m_ASMFileInfo.Banks.Count - 1];

            // size was not given, reuse from previous bank
            if ( paramsSize.Count == 0 )
            {
              size = lastBank.SizeInBytes;
            }

            if ( sizeInBytes <= lastBank.SizeInBytesStart + lastBank.SizeInBytes )
            {
              // we need to fill

              int delta = lastBank.SizeInBytesStart + lastBank.SizeInBytes - sizeInBytes;

              info.NumBytes = delta;
              info.LineData = new GR.Memory.ByteBuffer( (uint)delta );
              lineSizeInBytes = delta;
            }
            else
            {
              int overflow = sizeInBytes - lastBank.SizeInBytesStart;
              AddError( _ParseContext.LineIndex, Types.ErrorCode.E1101_BANK_TOO_BIG, "Bank " + lastBank.Number + " contains too many bytes, " + lastBank.SizeInBytes + " chosen, " + overflow + " encountered" );
            }
          }
          if ( size == 0 )
          {
            AddError( _ParseContext.LineIndex, Types.ErrorCode.E1104_BANK_SIZE_INVALID, "Bank size is invalid" );
          }

          Types.ASM.BankInfo bank = new RetroDevStudio.Types.ASM.BankInfo();
          bank.Number = number;
          bank.SizeInBytes = size;
          bank.StartLine = _ParseContext.LineIndex;
          bank.SizeInBytesStart = sizeInBytes + info.NumBytes;

          foreach ( Types.ASM.BankInfo oldBank in m_ASMFileInfo.Banks )
          {
            if ( oldBank.Number == number )
            {
              AddWarning( _ParseContext.LineIndex,
                          Types.ErrorCode.W0003_BANK_INDEX_ALREADY_USED,
                          "Bank with index " + number + " already exists",
                          lineTokenInfos[0].StartPos,
                          lineTokenInfos[lineTokenInfos.Count - 1].EndPos + 1 - lineTokenInfos[0].StartPos );
            }
          }

          m_ASMFileInfo.Banks.Add( bank );
        }
      }
    }






  }
}
