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
    private void POFor( string zoneName, ref int intermediateLineOffset, List<Types.TokenInfo> lineTokenInfos )
    {
      if ( ScopeInsideMacroDefinition() )
      {
        // ignore for loop if we are inside a macro definition!

        // add dummy scope so !ends properly match
        Types.LoopInfo loop = new Types.LoopInfo();

        loop.LineIndex = _ParseContext.LineIndex;

        if ( lineTokenInfos.Last().Content == "{" )
        {
          loop.IsUsingBrackets = true;
        }

        Types.ScopeInfo   scope = new RetroDevStudio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.LOOP );

        scope.Active = true;
        scope.Loop = loop;
        scope.StartIndex = _ParseContext.LineIndex;
        _ParseContext.Scopes.Add( scope );
        return;
      }

      if ( lineTokenInfos.Count < 5 )
      {
        AddError( _ParseContext.LineIndex, 
                   RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO, 
                  "Malformed macro, expect either !FOR <Variable> = <Start Value Expression> TO <End Value Expression [STEP] <Step Value Expression> or !FOR <Variable>, <Start Value Expression>, <End Value Expression> {" );
      }
      else
      {
        // determine which format is used
        // ACME style?
        if ( lineTokenInfos[lineTokenInfos.Count - 1].Content == "{" )
        {
          if ( ( !ParseLineInParameters( lineTokenInfos, 1, lineTokenInfos.Count - 2, _ParseContext.LineIndex, false, out var lineParts ) )
          ||   ( lineParts.Count != 3 )
          ||   ( lineParts[0].Count != 1 )
          ||   ( !IsTokenLabel( lineParts[0][0].Type ) ) )    // must be a single label
          {
            AddError( _ParseContext.LineIndex,
                       RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO,
                      "Malformed macro, expect either !FOR <Variable> = <Start Value Expression> TO <End Value Expression [STEP] <Step Value Expression> or !FOR <Variable>, <Start Value Expression>, <End Value Expression> {" );
            return;
          }
          
          bool hadError = false;
          if ( !EvaluateTokens( _ParseContext.LineIndex, lineParts[1], out SymbolInfo startValueSymbol ) )
          {
            AddError( _ParseContext.LineIndex,
                      RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO,
                      "Malformed macro, expect !FOR <Variable>, <Start Value Expression>, <End Value Expression> {",
                      lineParts[1][0].StartPos,
                      lineParts[1][lineParts[1].Count - 1].EndPos + 1 - lineParts[1][0].StartPos );
            hadError = true;
          }
          else if ( !EvaluateTokens( _ParseContext.LineIndex, lineParts[2], out SymbolInfo endValueSymbol ) )
          {
            AddError( _ParseContext.LineIndex,
                      RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO,
                      "Malformed macro, expect !FOR <Variable>, <Start Value Expression>, <End Value Expression> {",
                      lineParts[2][0].StartPos,
                      lineParts[2][lineParts[2].Count - 1].EndPos + 1 - lineParts[2][0].StartPos );
            hadError = true;
          }
          else
          {
            int startValue  = startValueSymbol.ToInt32();
            int endValue    = endValueSymbol.ToInt32();

            if ( !hadError )
            {
              int stepValue = ( endValue >= startValue ) ? 1 : -1;
              Types.LoopInfo loop = new Types.LoopInfo()
              {
                Label                     = lineParts[0][0].Content,
                LineIndex                 = _ParseContext.LineIndex,
                StartValue                = startValue,
                EndValue                  = endValue,
                StepValue                 = stepValue,
                CurrentValue              = startValue,
                EndValueTokens            = lineParts[2],
                EndValueTokensTextmapping = _ParseContext.CurrentTextMapping,
                IsUsingBrackets           = true
              };
              var scope = new ScopeInfo( Types.ScopeInfo.ScopeType.LOOP )
              {
                Active      = true,
                Loop        = loop,
                StartIndex  = _ParseContext.LineIndex
              };
              _ParseContext.Scopes.Add( scope );

              AddTempLabel( loop.Label, _ParseContext.LineIndex + 1, -1, CreateIntegerSymbol( startValue ), "" ).IsForVariable = true;

              intermediateLineOffset = 0;
            }
          }
          return;
        }


        // C64Studio style
        if ( ( ( lineTokenInfos[1].Type != RetroDevStudio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
        &&   ( lineTokenInfos[1].Type != RetroDevStudio.Types.TokenInfo.TokenType.LABEL_LOCAL ) )
        ||   ( lineTokenInfos[2].Content != "=" ) )
        {
          AddError( _ParseContext.LineIndex,
                    RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO,
                    "Malformed macro, expect !FOR <Variable> = <Start Value Expression> TO <End Value Expression [STEP] <Step Value Expression>",
                    lineTokenInfos[1].StartPos,
                    lineTokenInfos[lineTokenInfos.Count - 1].EndPos + 1 - lineTokenInfos[1].StartPos );
        }
        else
        {
          int     indexTo = FindTokenContent( lineTokenInfos, "to" );
          int     indexStep = FindTokenContent( lineTokenInfos, "step" );
          int     stepValue = 1;
          bool    hadError = false;

          if ( ( indexTo == -1 )
          ||   ( indexTo < 3 ) )
          {
            AddError( _ParseContext.LineIndex,
                      RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO,
                      "Malformed macro, expect !FOR <Variable> = <Start Value Expression> TO <End Value Expression [STEP] <Step Value Expression>",
                      lineTokenInfos[1].StartPos,
                      lineTokenInfos[lineTokenInfos.Count - 1].EndPos + 1 - lineTokenInfos[1].StartPos );
            hadError = true;
          }
          if ( indexStep != -1 )
          {
            if ( !EvaluateTokens( _ParseContext.LineIndex, lineTokenInfos, indexStep + 1, lineTokenInfos.Count - indexStep - 1, out SymbolInfo stepValueSymbol ) )
            {
              AddError( _ParseContext.LineIndex,
                        RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO,
                        "Malformed macro, expect !FOR <Variable> = <Start Value Expression> TO <End Value Expression [STEP] <Step Value Expression>",
                        lineTokenInfos[indexStep + 1].StartPos,
                        lineTokenInfos[lineTokenInfos.Count - 1].EndPos + 1 - lineTokenInfos[indexStep + 1].StartPos );
              hadError = true;
            }
            else
            {
              stepValue = stepValueSymbol.ToInt32();
              if ( stepValue == 0 )
              {
                AddError( _ParseContext.LineIndex,
                          RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO,
                          "Value of step must not be zero",
                          lineTokenInfos[indexStep + 1].StartPos,
                          lineTokenInfos[lineTokenInfos.Count - 1].EndPos + 1 - lineTokenInfos[indexStep + 1].StartPos );
                hadError = true;
              }
            }
          }
          else
          {
            indexStep = lineTokenInfos.Count;
          }

          if ( !hadError )
          {
            var endValueTokens = lineTokenInfos.GetRange( indexTo + 1, indexStep - indexTo - 1 );
            int startValue = 0;
            int endValue = 0;
            if ( !EvaluateTokens( _ParseContext.LineIndex, lineTokenInfos, 3, indexTo - 3, out SymbolInfo startValueSymbol ) )
            {
              AddError( _ParseContext.LineIndex,
                        RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO,
                        "Could not evaluate start value",
                        lineTokenInfos[3].StartPos,
                        lineTokenInfos[indexTo - 3].EndPos + 1 - lineTokenInfos[3].StartPos );
              hadError = true;
            }
            else if ( !EvaluateTokens( _ParseContext.LineIndex, lineTokenInfos, indexTo + 1, indexStep - indexTo - 1, out SymbolInfo endValueSymbol ) )
            {
              AddError( _ParseContext.LineIndex,
                        RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO,
                        "Could not evaluate end value",
                        lineTokenInfos[indexTo + 1].StartPos,
                        lineTokenInfos[indexStep - indexTo - 1].EndPos + 1 - lineTokenInfos[indexTo + 1].StartPos );
              hadError = true;
            }
            else
            {
              startValue = startValueSymbol.ToInt32();
              endValue = endValueSymbol.ToInt32();
              if ( ( stepValue < 0 )
              &&   ( endValue >= startValue ) )
              {
                AddError( _ParseContext.LineIndex,
                          RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO,
                          "End value must be lower than start value with negative step",
                          lineTokenInfos[indexTo + 1].StartPos,
                          lineTokenInfos[indexStep - 1].EndPos + 1 - lineTokenInfos[indexTo + 1].StartPos );
                hadError = true;
              }
              else if ( ( stepValue > 0 )
              &&        ( endValue < startValue ) )
              {
                AddError( _ParseContext.LineIndex,
                          RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO,
                          "End value must be higher than start value with positive step",
                          lineTokenInfos[indexTo + 1].StartPos,
                          lineTokenInfos[indexStep - indexTo - 1].EndPos + 1 - lineTokenInfos[indexTo + 1].StartPos );
                hadError = true;
              }
            }
            if ( !hadError )
            {
              Types.LoopInfo loop = new Types.LoopInfo();

              loop.Label = lineTokenInfos[1].Content;
              loop.LineIndex = _ParseContext.LineIndex;
              loop.StartValue = startValue;
              loop.EndValue = endValue;
              loop.StepValue = stepValue;
              loop.CurrentValue = startValue;
              loop.EndValueTokens = endValueTokens;
              loop.EndValueTokensTextmapping = _ParseContext.CurrentTextMapping;

              //Debug.Log( $"Begin Loop for {loop.Label}" );

              Types.ScopeInfo   scope = new RetroDevStudio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.LOOP );
              // TODO - active depends on parent scopes
              scope.Active = true;
              scope.Loop = loop;
              scope.StartIndex = _ParseContext.LineIndex;
              _ParseContext.Scopes.Add( scope );

              AddTempLabel( loop.Label, _ParseContext.LineIndex + 1, -1, CreateIntegerSymbol( startValue ), "" ).IsForVariable = true;

              intermediateLineOffset = 0;
            }
          }
        }
      }
    }



  }
}
