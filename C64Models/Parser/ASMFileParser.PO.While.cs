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
    private ParseLineResult POWhile( List<Types.TokenInfo> lineTokenInfos, Types.ASM.LineInfo info, ref string[] Lines, out int lineSizeInBytes )
    {
      lineSizeInBytes = 0;

      if ( ScopeInsideMacroDefinition() )
      {
        return ParseLineResult.CALL_CONTINUE;
      }

      // WHILE <Expression> {
      if ( lineTokenInfos[lineTokenInfos.Count - 1].Content != "{" )
      {
        AddError( _ParseContext.LineIndex, RetroDevStudio.Types.ErrorCode.E1000_SYNTAX_ERROR, "Malformed WHILE, expected WHILE <Expression> {" );
        return ParseLineResult.ERROR_ABORT;
      }

      int expressionCheck = -1;

      if ( EvaluateTokens( _ParseContext.LineIndex, lineTokenInfos, 1, lineTokenInfos.Count - 2, info.LineCodeMapping, out SymbolInfo whileExpressionSymbol ) )
      {
        expressionCheck = whileExpressionSymbol.ToInt32();

        // TODO - find matching loop end and copy lines now (to avoid auto-inserting macros only in the first iteration)
        Types.ScopeInfo scope = new RetroDevStudio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.WHILE );

        scope.StartIndex  = _ParseContext.LineIndex;
        scope.While       = new WhileInfo() { LineIndex = _ParseContext.LineIndex };
        scope.Active      = ( expressionCheck > 0 );
        _ParseContext.Scopes.Add( scope );

        scope.While.EndValueTokens            = lineTokenInfos.GetRange( 1, lineTokenInfos.Count - 2 );
        scope.While.EndValueTokensTextmapping = info.LineCodeMapping;

        return ParseLineResult.CALL_CONTINUE;
      }
      else
      {
        AddError( _ParseContext.LineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Could not evaluate " + TokensToExpression( lineTokenInfos, 1, lineTokenInfos.Count - 2 ) );
      }
      return ParseLineResult.OK;
    }






  }
}
