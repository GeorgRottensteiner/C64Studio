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
    private ParseLineResult POLoopStart( List<Types.TokenInfo> lineTokenInfos, int lineIndex, Types.ASM.LineInfo info, ref string[] Lines, List<Types.ScopeInfo> Scopes, out int lineSizeInBytes )
    {
      lineSizeInBytes = 0;

      if ( ScopeInsideMacroDefinition( Scopes ) )
      {
        return ParseLineResult.CALL_CONTINUE;
      }

      // DO <num>
      if ( lineTokenInfos.Count < 2 )
      {
        if ( m_AssemblerSettings.DoWithoutParameterIsUntil )
        {
          // add dummy scope so !ends properly match
          var repeatUntil = new RepeatUntilInfo();

          repeatUntil.LineIndex = lineIndex;

          Types.ScopeInfo   scope = new RetroDevStudio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.DO_UNTIL );

          scope.Active = true;
          scope.RepeatUntil = repeatUntil;
          scope.StartIndex = lineIndex;
          Scopes.Add( scope );
          OnScopeAdded( scope );
        }
        else
        {
          AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO, "Malformed DO loop, expected DO <Expression>" );
        }
      }
      else
      {
        int numLoops = -1;

        if ( EvaluateTokens( lineIndex, lineTokenInfos, 1, lineTokenInfos.Count - 1, info.LineCodeMapping, out SymbolInfo numLoopsSymbol ) )
        {
          numLoops = numLoopsSymbol.ToInt32();
          bool hadError = false;
          if ( numLoops <= 0 )
          {
            AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO, "Loop count must be positive" );
            hadError = true;
          }
          if ( !hadError )
          {
            // TODO - find matching loop end and copy lines now (to avoid auto-inserting macros only in the first iteration)
            int nextLineIndex = FindLoopEnd( Lines, lineIndex + 1, Scopes, info.LineCodeMapping );
            if ( nextLineIndex == -1 )
            {
              AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1008_MISSING_LOOP_END, "Missing loop end" );
              hadError = true;
            }
            else
            {
              int loopLength = nextLineIndex - lineIndex - 1;
              string[] tempContent = new string[loopLength * ( numLoops - 1 )];

              for ( int i = 0; i < numLoops - 1; ++i )
              {
                System.Array.Copy( Lines, lineIndex + 1, tempContent, i * loopLength, loopLength );
              }

              string[] replacementLines = RelabelLocalLabelsForLoop( tempContent, Scopes, lineIndex, info.LineCodeMapping );

              string[] newLines = new string[Lines.Length + replacementLines.Length];

              System.Array.Copy( Lines, 0, newLines, 0, lineIndex + 1 + loopLength );
              System.Array.Copy( replacementLines, 0, newLines, lineIndex + 1 + loopLength, replacementLines.Length );

              // replaces the REPEND
              newLines[lineIndex + 1 + loopLength + replacementLines.Length] = "";
              System.Array.Copy( Lines, nextLineIndex + 1, newLines, lineIndex + 1 + loopLength + replacementLines.Length + 1, Lines.Length - nextLineIndex - 1 );

              // adjust source infos to make lookup work correctly
              string outerFilename = "";
              int outerLineIndex = -1;
              ASMFileInfo.FindTrueLineSource( lineIndex + 1, out outerFilename, out outerLineIndex );

              //ASMFileInfo.LineInfo.Remove( lineIndex );

              for ( int i = 0; i < numLoops - 1; ++i )
              {
                Types.ASM.SourceInfo sourceInfo = new Types.ASM.SourceInfo();
                sourceInfo.Filename = outerFilename;
                sourceInfo.FullPath = outerFilename;
                sourceInfo.GlobalStartLine = lineIndex + 1 + ( 1 + i ) * loopLength;
                sourceInfo.LineCount = loopLength;
                sourceInfo.LocalStartLine = outerLineIndex;

                InsertSourceInfo( sourceInfo );
              }


              Lines = newLines;

              //Debug.Log( "New total " + Lines.Length + " lines" );
              return ParseLineResult.CALL_CONTINUE;
            }
          }
        }
        else
        {
          AddError( lineIndex, Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, "Could not evaluate " + TokensToExpression( lineTokenInfos, 1, lineTokenInfos.Count - 1 ) );
        }
      }
      return ParseLineResult.OK;
    }






  }
}
