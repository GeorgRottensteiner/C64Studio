using GR.Collections;
using GR.Generic;
using GR.Memory;
using RetroDevStudio.Formats;
using RetroDevStudio.Parser;
using RetroDevStudio.Types;
using RetroDevStudio.Types.ASM;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RetroDevStudio.Parser
{
  public partial class ASMFileParser : ParserBase
  {
    private ParseLineResult POCallMacro( List<Types.TokenInfo> lineTokenInfos,
                                         ref int lineIndex,
                                         Types.ASM.LineInfo info,
                                         string parseLine,
                                         string ParentFilename,
                                         string labelInFront,
                                         GR.Collections.Map<GR.Generic.Tupel<string, int>, Types.MacroFunctionInfo> macroFunctions,
                                         ref string[] Lines,
                                         GR.Collections.Map<byte, byte> TextCodeMapping,
                                         out int lineSizeInBytes )
    {
      // +macro Macroname [param1[,param2]]
      lineSizeInBytes = 0;

      if ( ( m_AssemblerSettings.MacroFunctionCallPrefix.Count > 0 )
      &&   ( m_AssemblerSettings.MacroFunctionCallPrefix[0].Length >= lineTokenInfos[0].Content.Length ) )
      {
        AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO, "Unnamed macro function" );
        return ParseLineResult.OK;
      }

      string functionName = lineTokenInfos[0].Content;
      if ( m_AssemblerSettings.MacroFunctionCallPrefix.Count > 0 )
      {
        foreach ( var prefix in m_AssemblerSettings.MacroFunctionCallPrefix )
        {
          if ( functionName.StartsWith( prefix ) )
          {
            functionName = functionName.Substring( prefix.Length );
          }
        }
      }

      int numParams = EstimateNumberOfParameters( lineTokenInfos, 1, lineTokenInfos.Count - 1 );
      var macroKey = new GR.Generic.Tupel<string,int>( functionName, numParams );

      if ( !DoesMacroExist( macroFunctions, macroKey, out Types.MacroFunctionInfo macro ) )
      {
        AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO, $"No matching macro with name {functionName} and {numParams + 1} arguments found", lineTokenInfos[0].StartPos, lineTokenInfos[0].Length );
      }
      else
      {
        Types.MacroFunctionInfo  functionInfo = macro;

        List<string>  param = new List<string>();
        List<bool>    paramIsRef = new List<bool>();
        int           startIndex = 1;
        bool          hadError = false;

        if ( !_ParseContext.DoNotAddReferences )
        {
          functionInfo.Symbol.References.Add( lineIndex, new SymbolReference() { GlobalLineIndex = lineIndex, TokenInfo = lineTokenInfos[0] } );
        }

        for ( int i = 1; i < lineTokenInfos.Count; ++i )
        {
          if ( lineTokenInfos[i].Content == "," )
          {
            // separator
            // we're using a custom internal brace to not mix up opcode detection with expression parsing
            param.Add( AssemblerSettings.INTERNAL_OPENING_BRACE + TokensToExpression( lineTokenInfos, startIndex, i - startIndex ) + AssemblerSettings.INTERNAL_CLOSING_BRACE );
            //parseLine.Substring( lineTokenInfos[startIndex].StartPos, lineTokenInfos[i].StartPos - lineTokenInfos[startIndex].StartPos ) + AssemblerSettings.INTERNAL_CLOSING_BRACE );

            // is reference properly matched?
            if ( !m_AssemblerSettings.MacrosHaveVariableNumberOfArguments )
            {
              if ( param.Count > functionInfo.ParametersAreReferences.Count )
              {
                AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO, "Referenced parameters are not matching macro definition" );
                hadError = true;
              }
              else if ( param[param.Count - 1].StartsWith( "~" ) != functionInfo.ParametersAreReferences[param.Count - 1] )
              {
                AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO, "Referenced parameters are not matching macro definition" );
                hadError = true;
              }
            }

            if ( ( !hadError )
            &&   ( !m_AssemblerSettings.MacrosHaveVariableNumberOfArguments )
            &&   ( functionInfo.ParametersAreReferences[param.Count - 1] ) )
            {
              param[param.Count - 1] = param[param.Count - 1].Substring( 1 );

              string paramName = param[param.Count - 1];

              if ( m_ASMFileInfo.UnparsedLabels.ContainsKey( paramName ) )
              {
                AddLabel( paramName, 0, lineIndex, info.Zone, -1, 0 );
              }
            }
            startIndex = i + 1;
          }
        }
        if ( ( startIndex == lineTokenInfos.Count )
        &&   ( startIndex > 1 ) )
        {
          param.Add( "" );
        }
        else if ( startIndex < lineTokenInfos.Count )
        {
          if ( lineTokenInfos.Count - startIndex == 1 )
          {
            param.Add( TokensToExpression( lineTokenInfos, startIndex, lineTokenInfos.Count - startIndex ) );
          }
          else
          {
            // braces so potential original operators are evaluated before the rest is
            param.Add( AssemblerSettings.INTERNAL_OPENING_BRACE + TokensToExpression( lineTokenInfos, startIndex, lineTokenInfos.Count - startIndex ) + AssemblerSettings.INTERNAL_CLOSING_BRACE );
          }
          // is reference properly matched?
          if ( !m_AssemblerSettings.MacrosHaveVariableNumberOfArguments )
          {
            if ( param.Count > functionInfo.ParametersAreReferences.Count )
            {
              AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO, "Referenced parameters are not matching macro definition" );
              hadError = true;
            }
            else if ( param[param.Count - 1].StartsWith( "~" ) != functionInfo.ParametersAreReferences[param.Count - 1] )
            {
              AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO, "Referenced parameters are not matching macro definition" );
              hadError = true;
            }
          }
          if ( ( !hadError )
          &&   ( !m_AssemblerSettings.MacrosHaveVariableNumberOfArguments )
          &&   ( functionInfo.ParametersAreReferences[param.Count - 1] ) )
          {
            param[param.Count - 1] = param[param.Count - 1].Substring( 1 );
            string paramName = param[param.Count - 1];

            if ( m_ASMFileInfo.UnparsedLabels.ContainsKey( paramName ) )
            {
              AddLabel( paramName, 0, lineIndex, info.Zone, -1, 0 );
            }
          }
        }
        if ( ( !m_AssemblerSettings.MacrosHaveVariableNumberOfArguments )
        &&   ( param.Count != functionInfo.ParameterNames.Count ) )
        {
          AddError( lineIndex, RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO, "Parameter count does not match for macro " + functionInfo.Name );
        }
        else if ( !hadError )
        {
          int lineIndexInMacro = -1;
          string[] replacementLines = RelabelLocalLabelsForMacro( Lines, lineIndex, functionName, functionInfo, functionInfo.ParameterNames, param, info.LineCodeMapping, out lineIndexInMacro );
          if ( replacementLines == null )
          {
            AddError( lineIndexInMacro, RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO, "Syntax error during macro replacement at position " + m_LastErrorInfo.Pos );
          }
          else
          {
            // insert macro code with clearing macro call line
            // readd label if there was one before the +macro
            {
              // if label in front insert macro one line below!
              string[] newLines = new string[Lines.Length + replacementLines.Length];

              System.Array.Copy( Lines, 0, newLines, 0, lineIndex + 1 );
              System.Array.Copy( replacementLines, 0, newLines, lineIndex + 1, replacementLines.Length );
              if ( Lines.Length - lineIndex - 1 >= 1 )
              {
                System.Array.Copy( Lines, lineIndex + 1, newLines, lineIndex + 1 + replacementLines.Length, Lines.Length - lineIndex - 1 );
              }

              newLines[lineIndex] = labelInFront;

              // adjust source infos to make lookup work correctly
              Types.ASM.SourceInfo sourceInfo = new Types.ASM.SourceInfo();
              sourceInfo.Filename         = functionInfo.ParentFileName;
              sourceInfo.FullPath         = functionInfo.ParentFileName;
              sourceInfo.GlobalStartLine  = lineIndex + 1;
              sourceInfo.LineCount        = replacementLines.Length;
              sourceInfo.Source           = SourceInfo.SourceInfoSource.MACRO;
              string dummy;
              m_ASMFileInfo.FindTrueLineSource( functionInfo.LineIndex + 1, out dummy, out sourceInfo.LocalStartLine );

              InsertSourceInfo( sourceInfo );

              Lines = newLines;

              return ParseLineResult.CALL_CONTINUE;
            }
          }
        }
      }
      return ParseLineResult.OK;
    }



    public int EstimateNumberOfParameters( List<TokenInfo> TokenInfos, int StartIndex, int Count )
    {
      int     numParams = 0;
      if ( TokenInfos.Count > 1 )
      {
        ++numParams;
      }
      for ( int i = StartIndex; i < StartIndex + Count; ++i )
      {
        if ( TokenInfos[i].Content == "," )
        {
          ++numParams;
        }
      }
      return numParams;
    }



    private bool DoesMacroExist( Map<Tupel<string, int>, MacroFunctionInfo> macroFunctions, GR.Generic.Tupel<string, int> Key, out MacroFunctionInfo Macro )
    {
      Macro = null;
      if ( m_AssemblerSettings.MacrosCanBeOverloaded )
      {
        //if ( macroFunctions.Keys.Any( k => ( k.first == Key.first ) && ( k.second == Key.second ) ) )
        if ( !macroFunctions.ContainsKey( Key ) )
        {
          return false;
        }
        Macro = macroFunctions[Key];
        return true;
      }
      if ( !macroFunctions.Keys.Any( m => m.first == Key.first ) )
      {
        return false;
      }
      Macro = macroFunctions.FirstOrDefault( m => m.Key.first == Key.first ).Value;
      return true;
    }


  }
}
