using GR.Collections;
using GR.Memory;
using RetroDevStudio.Formats;
using RetroDevStudio.Parser;
using RetroDevStudio.Types;
using RetroDevStudio.Types.ASM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tiny64;

namespace RetroDevStudio.Parser
{
  public partial class ASMFileParser : ParserBase
  {
    private ParseLineResult PODefine( List<TokenInfo> lineTokenInfos, LineInfo info, Map<byte,byte> textCodeMapping, ref int programStepPos, ref int trueCompileCurrentAddress )
    {
      if ( ScopeInsideMacroDefinition() )
      {
        return ParseLineResult.CALL_CONTINUE;
      }
      StripInternalBrackets( lineTokenInfos, 1 );
      int equPos = lineTokenInfos[1].StartPos;
      string operatorToken = lineTokenInfos[1].Content;

      string defineName = lineTokenInfos[0].Content;
      if ( !m_AssemblerSettings.CaseSensitive )
      {
        defineName = defineName.ToUpper();
      }
      int   defineLength = lineTokenInfos[lineTokenInfos.Count - 1].StartPos + lineTokenInfos[lineTokenInfos.Count - 1].Length - ( equPos + lineTokenInfos[1].Content.Length );
      string defineValue = TokensToExpression( lineTokenInfos, 2, lineTokenInfos.Count - 2 );
      // avoid detecting as label
      if ( m_AssemblerSettings.LabelsMustBeAtStartOfLine )
      {
        defineValue = " " + defineValue;
      }

      // removed line text code mapping from here
      // TODO - WHY??
      var origMapping = _ParseContext.CurrentTextMapping;
      _ParseContext.CurrentTextMapping = new Map<byte, byte>();

      _ParseContext.DuringExpressionEvaluation = true;
      List<Types.TokenInfo>  valueTokens = ParseTokenInfo( defineValue, 0, defineValue.Length );
      _ParseContext.DuringExpressionEvaluation = false;

      if ( defineName == "*" )
      {
        // set program step
        info.AddressSource = "*";

        List<Types.TokenInfo> tokens = ParseTokenInfo( defineValue, 0, defineValue.Length );
        if ( ( tokens.Count > 0 )
        &&   ( tokens[tokens.Count - 1].Type == TokenInfo.TokenType.LITERAL_STRING ) )
        {
          info.AddressSource = "*" + tokens[tokens.Count - 1].Content;
          tokens.RemoveAt( tokens.Count - 1 );
        }
        if ( !EvaluateTokens( _ParseContext.LineIndex, tokens, out SymbolInfo newStepPosSymbol ) )
        {
          _ParseContext.CurrentTextMapping = origMapping;
          AddError( _ParseContext.LineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Could not evaluate * position value", lineTokenInfos[0].StartPos, lineTokenInfos[0].Length );
          return ParseLineResult.ERROR_ABORT;
        }

        var origPos = CreateIntegerSymbol( programStepPos );
        if ( !HandleAssignmentOperator( _ParseContext.LineIndex, lineTokenInfos, origPos, operatorToken, newStepPosSymbol, out SymbolInfo resultingValue ) )
        {
          _ParseContext.CurrentTextMapping = origMapping;
          return ParseLineResult.ERROR_ABORT;
        }

        programStepPos = resultingValue.ToInt32();
        m_CompileCurrentAddress = programStepPos;
        trueCompileCurrentAddress = programStepPos;

        info.AddressStart = programStepPos;
      }
      else
      {
        if ( ScopeInsideMacroDefinition() )
        {
          _ParseContext.CurrentTextMapping = origMapping;
          return ParseLineResult.CALL_CONTINUE;
        }

        if ( !EvaluateTokens( _ParseContext.LineIndex, valueTokens, out SymbolInfo addressSymbol ) )
        {
          if ( !IsPlainAssignment( operatorToken ) )
          {
            _ParseContext.CurrentTextMapping = origMapping;
            AddError( _ParseContext.LineIndex, ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Assignment operators must be solvable in one pass" );
            return ParseLineResult.ERROR_ABORT;
          }
          AddUnparsedLabel( defineName, defineValue, _ParseContext.LineIndex );
        }
        else
        {
          _ParseContext.DoNotAddReferences = true;
          EvaluateTokens( _ParseContext.LineIndex, lineTokenInfos, 0, 1, out SymbolInfo originalValue );
          _ParseContext.DoNotAddReferences = false;

          if ( !HandleAssignmentOperator( _ParseContext.LineIndex, lineTokenInfos, originalValue, operatorToken, addressSymbol, out SymbolInfo resultingValue ) )
          {
            _ParseContext.CurrentTextMapping = origMapping;
            return ParseLineResult.ERROR_ABORT;
          }

          resultingValue.Zone = m_CurrentZoneName;
          if ( resultingValue.Type == SymbolInfo.Types.CONSTANT_REAL_NUMBER )
          {
            AddConstantF( defineName, resultingValue, _ParseContext.LineIndex, m_CurrentCommentSB.ToString(), m_CurrentZoneName, lineTokenInfos[0].StartPos, lineTokenInfos[0].Length );
          }
          else if ( resultingValue.Type == SymbolInfo.Types.CONSTANT_STRING )
          {
            AddConstantString( defineName, resultingValue, _ParseContext.LineIndex, m_CurrentCommentSB.ToString(), m_CurrentZoneName, lineTokenInfos[0].StartPos, lineTokenInfos[0].Length );
          }
          else
          {
            AddConstant( defineName, resultingValue, _ParseContext.LineIndex, m_CurrentCommentSB.ToString(), m_CurrentZoneName, lineTokenInfos[0].StartPos, lineTokenInfos[0].Length );
          }
        }
      }
      m_CurrentCommentSB = new StringBuilder();
      _ParseContext.CurrentTextMapping = origMapping;

      return ParseLineResult.CALL_CONTINUE;
    }



  }
}
