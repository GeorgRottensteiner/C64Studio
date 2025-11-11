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
    private bool POMacro( string LabelInFront, string Zone, GR.Collections.Map<GR.Generic.Tupel<string,int>, Types.MacroFunctionInfo> macroFunctions, 
                          string OuterFilename,
                          List<Types.TokenInfo> lineTokenInfos, 
                          out string MacroFunctionName )
    {
      // !macro Macroname [param1[,param2]]
      bool  hadError = false;
      bool  hasBracket = false;

      MacroFunctionName = "";
      if ( lineTokenInfos[lineTokenInfos.Count - 1].Content == "{" )
      {
        hasBracket = true;
        lineTokenInfos.RemoveAt( lineTokenInfos.Count - 1 );
      }

      if ( m_AssemblerSettings.MacroKeywordAfterName )
      {
        // PDS style (<macroname> MACRO)
        if ( lineTokenInfos.Count != 1 )
        {
          AddError( _ParseContext.LineIndex, RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO, "Malformed macro, expect <Macroname> MACRO" );
          return false;
        }
        if ( macroFunctions.Keys.Any( m => m.first == LabelInFront ) )
        {
          AddError( _ParseContext.LineIndex, RetroDevStudio.Types.ErrorCode.E1200_REDEFINITION_OF_LABEL, $"Macro name {LabelInFront} is already in use" );
          return false;
        }
        Types.MacroFunctionInfo macroFunction = new RetroDevStudio.Types.MacroFunctionInfo();

        macroFunction.Name            = LabelInFront;
        macroFunction.LineIndex       = _ParseContext.LineIndex;
        macroFunction.ParentFileName  = OuterFilename;
        macroFunction.UsesBracket     = hasBracket;

        macroFunction.Symbol                  = new SymbolInfo();
        macroFunction.Symbol.Name             = LabelInFront;
        macroFunction.Symbol.LocalLineIndex   = _ParseContext.LineIndex;
        macroFunction.Symbol.Type             = SymbolInfo.Types.MACRO;
        macroFunction.Symbol.DocumentFilename = OuterFilename;
        macroFunction.Symbol.Zone             = Zone;

        if ( !_ParseContext.DoNotAddReferences )
        {
          macroFunction.Symbol.AddReference( _ParseContext.LineIndex, lineTokenInfos[0] );
        }
        macroFunction.Symbol.NumArguments     = -1;


        macroFunctions.Add( new GR.Generic.Tupel<string, int>( LabelInFront, -1 ), macroFunction );

        MacroFunctionName = LabelInFront;

        // macro scope
        Types.ScopeInfo scope = new RetroDevStudio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.MACRO_FUNCTION );

        scope.StartIndex  = _ParseContext.LineIndex;
        scope.Macro       = macroFunction;
        scope.Active      = true;
        _ParseContext.Scopes.Add( scope );
        return true;
      }

      if ( ( m_AssemblerSettings.MacrosHaveVariableNumberOfArguments )
      &&   ( lineTokenInfos.Count < 2 ) )
      {
        AddError( _ParseContext.LineIndex, RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO, "Malformed macro, expect !MACRO <Macroname>" );
        hadError = true;
      }
      else if ( lineTokenInfos.Count < 2 )
      {
        AddError( _ParseContext.LineIndex, RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO, "Malformed macro, expect !MACRO <Macroname> [<Param1>[,<Param2>[...]]]" );
        hadError = true;
      }
      else if ( lineTokenInfos[1].Type != RetroDevStudio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
      {
        AddError( _ParseContext.LineIndex, RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO, "Macro name must be formatted like a global label" );
        hadError = true;
      }
      else
      {
        string macroName = lineTokenInfos[1].Content;
        
        if ( ( !m_AssemblerSettings.MacrosCanBeOverloaded )
        &&   ( macroFunctions.Keys.Any( m => m.first == macroName ) ) )
        {
          AddError( _ParseContext.LineIndex, RetroDevStudio.Types.ErrorCode.E1200_REDEFINITION_OF_LABEL, $"Macro name {macroName} is already in use" );
          hadError = true;
        }
        else
        {
          List<string>  param = new List<string>();
          List<bool>    paramIsRef = new List<bool>();

          bool shouldParameterBeAComma = false;

          for ( int i = 2; i < lineTokenInfos.Count; ++i )
          {
            if ( shouldParameterBeAComma )
            {
              if ( lineTokenInfos[i].Content != "," )
              {
                AddError( _ParseContext.LineIndex, RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO, "Parameter names must be separated by comma" );
                hadError = true;
              }
            }
            else if ( ( !shouldParameterBeAComma )
            &&        ( ( lineTokenInfos[i].Type != RetroDevStudio.Types.TokenInfo.TokenType.OPERATOR )
            ||          ( ( lineTokenInfos[i].Type == RetroDevStudio.Types.TokenInfo.TokenType.OPERATOR )
            &&            ( lineTokenInfos[i].Content != "~" ) ) )
            &&        ( lineTokenInfos[i].Type != RetroDevStudio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
            &&        ( lineTokenInfos[i].Type != RetroDevStudio.Types.TokenInfo.TokenType.LABEL_LOCAL ) )
            {
              AddError( _ParseContext.LineIndex, RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO, "Parameter name must be formatted like a global label" );
              hadError = true;
            }
            else if ( ( !shouldParameterBeAComma )
            &&        ( lineTokenInfos[i].Type == RetroDevStudio.Types.TokenInfo.TokenType.OPERATOR )
            &&        ( lineTokenInfos[i].Content == "~" ) )
            {
              if ( ( i + 1 >= lineTokenInfos.Count )
              ||   ( ( lineTokenInfos[i + 1].Type != RetroDevStudio.Types.TokenInfo.TokenType.LABEL_GLOBAL )
              &&     ( lineTokenInfos[i + 1].Type != RetroDevStudio.Types.TokenInfo.TokenType.LABEL_LOCAL ) ) )
              {
                AddError( _ParseContext.LineIndex, RetroDevStudio.Types.ErrorCode.E1302_MALFORMED_MACRO, "Syntax error, expected parameter name after ~" );
                hadError = true;
              }
              else
              {
                // ACME speciality, parameter is a reference (means it changes the outside value)
                param.Add( lineTokenInfos[i + 1].Content );
                paramIsRef.Add( true );
                ++i;
                shouldParameterBeAComma = true;
                continue;
              }
            }
            else
            {
              param.Add( lineTokenInfos[i].Content );
              paramIsRef.Add( false );
            }
            shouldParameterBeAComma = !shouldParameterBeAComma;
          }
          if ( !hadError )
          {
            Types.MacroFunctionInfo macroFunction = new RetroDevStudio.Types.MacroFunctionInfo();

            macroFunction.Name                    = macroName;
            macroFunction.LineIndex               = _ParseContext.LineIndex;
            macroFunction.ParentFileName          = OuterFilename;
            macroFunction.ParameterNames          = param;
            macroFunction.ParametersAreReferences = paramIsRef;
            macroFunction.UsesBracket             = hasBracket;

            macroFunction.Symbol                  = new SymbolInfo();
            macroFunction.Symbol.Name             = macroName;
            macroFunction.Symbol.LocalLineIndex   = _ParseContext.LineIndex;
            macroFunction.Symbol.Type             = SymbolInfo.Types.MACRO;
            macroFunction.Symbol.DocumentFilename = OuterFilename;
            macroFunction.Symbol.Zone             = Zone;
            macroFunction.Symbol.NumArguments     = param.Count;
            macroFunction.Symbol.LineIndex        = _ParseContext.LineIndex;

            if ( !_ParseContext.DoNotAddReferences )
            {
              macroFunction.Symbol.AddReference( _ParseContext.LineIndex, lineTokenInfos[1] );
            }

            macroFunctions.Add( new GR.Generic.Tupel<string, int>( macroName, param.Count ), macroFunction );

            MacroFunctionName = macroName;

            // macro scope
            Types.ScopeInfo scope = new RetroDevStudio.Types.ScopeInfo( Types.ScopeInfo.ScopeType.MACRO_FUNCTION );

            scope.StartIndex  = _ParseContext.LineIndex;
            scope.Macro       = macroFunction;
            scope.Active      = true;

            OnScopeAdded( scope );
            _ParseContext.Scopes.Add( scope );
          }
        }
      }
      return !hadError;
    }






  }
}
