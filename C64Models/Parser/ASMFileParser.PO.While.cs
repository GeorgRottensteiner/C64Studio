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
    private ParseLineResult POWhile( List<Types.TokenInfo> lineTokenInfos, int lineIndex, Types.ASM.LineInfo info, ref string[] Lines, out int lineSizeInBytes )
    {
      lineSizeInBytes = 0;

      if ( ScopeInsideMacroDefinition() )
      {
        return ParseLineResult.CALL_CONTINUE;
      }

      // WHILE <Expression> {
      if ( lineTokenInfos[lineTokenInfos.Count - 1].Content != "{" )
      {
        AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1000_SYNTAX_ERROR, "Malformed WHILE, expected WHILE <Expression> {" );
        return ParseLineResult.ERROR_ABORT;
      }

      int expressionCheck = -1;

      if ( EvaluateTokens( lineIndex, lineTokenInfos, 1, lineTokenInfos.Count - 2, info.LineCodeMapping, out SymbolInfo whileExpressionSymbol ) )
      {
        expressionCheck = whileExpressionSymbol.ToInt32();

        // TODO - find matching loop end and copy lines now (to avoid auto-inserting macros only in the first iteration)
        Types.ScopeInfo scope = new RetroDevStudio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.WHILE );

        scope.StartIndex  = lineIndex;
        scope.While       = new WhileInfo() { LineIndex = lineIndex };
        scope.Active      = ( expressionCheck > 0 );
        _ParseContext.Scopes.Add( scope );

        scope.While.EndValueTokens            = lineTokenInfos.GetRange( 1, lineTokenInfos.Count - 2 );
        scope.While.EndValueTokensTextmapping = info.LineCodeMapping;

        //var tempContent = new string[loopLength];
        //System.Array.Copy( Lines, lineIndex + 1, tempContent, loopLength, loopLength );

        /*
        // adjust source infos to make lookup work correctly
        string outerFilename = "";
        int outerLineIndex = -1;
        m_ASMFileInfo.FindTrueLineSource( lineIndex + 1, out outerFilename, out outerLineIndex );

        //ASMFileInfo.LineInfo.Remove( lineIndex );

        Types.ASM.SourceInfo sourceInfo = new Types.ASM.SourceInfo();
        sourceInfo.Filename = outerFilename;
        sourceInfo.FullPath = outerFilename;
        sourceInfo.GlobalStartLine = lineIndex + 1 + ( 1 + i ) * loopLength;
        sourceInfo.LineCount = loopLength;
        sourceInfo.LocalStartLine = outerLineIndex;

        InsertSourceInfo( sourceInfo );

        Lines = newLines;

        //Debug.Log( "New total " + Lines.Length + " lines" );*/
        return ParseLineResult.CALL_CONTINUE;
      }
      else
      {
        AddError( lineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Could not evaluate " + TokensToExpression( lineTokenInfos, 1, lineTokenInfos.Count - 2 ) );
      }
      return ParseLineResult.OK;
    }






  }
}
