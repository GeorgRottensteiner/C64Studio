using GR.Collections;
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
    private ParseLineResult POSkip( List<TokenInfo> lineTokenInfos, int lineIndex, LineInfo info, ref int programStepPos, ref int trueCompileCurrentAddress )
    {
      if ( lineTokenInfos.Count <= 1 )
      {
        AddError( lineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Missing expression for !skip" );
        return ParseLineResult.RETURN_NULL;
      }

      // set program step
      info.AddressSource = "*";
      if ( !EvaluateTokens( lineIndex, lineTokenInfos, 1, lineTokenInfos.Count - 1, out SymbolInfo newStepPosSymbol ) )
      {
        AddError( lineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Could not evaluate * position value", lineTokenInfos[0].StartPos, lineTokenInfos[0].Length );
        return ParseLineResult.ERROR_ABORT;
      }

      programStepPos += newStepPosSymbol.ToInt32();
      programStepPos = programStepPos & 0xffff;

      m_CompileCurrentAddress   = programStepPos;
      trueCompileCurrentAddress = programStepPos;

      info.AddressStart = programStepPos;
      return ParseLineResult.OK;
    }



  }
}
