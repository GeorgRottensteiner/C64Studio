using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio.Parser
{
  public class AssemblerSettings
  {
    public GR.Collections.Map<Types.TokenInfo.TokenType, string>    AllowedTokenStartChars = new GR.Collections.Map<Types.TokenInfo.TokenType, string>();
    public GR.Collections.Map<Types.TokenInfo.TokenType, string>    AllowedTokenChars = new GR.Collections.Map<Types.TokenInfo.TokenType, string>();
    public GR.Collections.Map<Types.TokenInfo.TokenType, string>    AllowedTokenEndChars = new GR.Collections.Map<Types.TokenInfo.TokenType, string>();
    public string                                                   AllowedSingleTokens;
    public GR.Collections.Map<string, Types.MacroInfo>              Macros = new GR.Collections.Map<string, Types.MacroInfo>();
    public string                                                   MacroPrefix = "";
    public string                                                   LabelPostfix = "";
    public string                                                   MacroFunctionCallPrefix = "";                                               
    public bool                                                     GlobalLabelsAutoZone = false;
    public bool                                                     MacroIsZone = false;
    public bool                                                     MacrosHaveVariableNumberOfArguments = false;
    public GR.Collections.Set<string>                               DefineSeparatorKeywords = new GR.Collections.Set<string>();
    public bool                                                     CaseSensitive = true;
    public bool                                                     IncludeExpectsStringLiteral = true;
    public GR.Collections.Set<char>                                 StatementSeparatorChars = new GR.Collections.Set<char>();



    public void AddMacro( string Macro, Types.MacroInfo.MacroType Type )
    {
      Macros[Macro] = new Types.MacroInfo();
      Macros[Macro].Keyword = Macro;
      Macros[Macro].Type = Type;
    }



    public void SetAssemblerType( Types.AssemblerType Type )
    {
      AllowedSingleTokens = "";
      AllowedTokenChars.Clear();
      AllowedTokenEndChars.Clear();
      AllowedTokenStartChars.Clear();
      DefineSeparatorKeywords.Clear();
      Macros.Clear();
      StatementSeparatorChars.Clear();
      MacroPrefix = "";
      LabelPostfix = "";
      MacroFunctionCallPrefix = "";
      MacroIsZone = false;
      MacrosHaveVariableNumberOfArguments = false;
      CaseSensitive = true;

      switch ( Type )
      {
        case Types.AssemblerType.C64_STUDIO:
          AllowedTokenStartChars[Types.TokenInfo.TokenType.LABEL_GLOBAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzÄÖÜäöü_";
          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_GLOBAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_äöüÄÖÜß.";
          AllowedTokenEndChars[Types.TokenInfo.TokenType.LABEL_GLOBAL] = "#:";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.LABEL_LOCAL] = ".";
          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_LOCAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_äöüÄÖÜß.";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.LITERAL_CHAR] = "'";
          AllowedTokenEndChars[Types.TokenInfo.TokenType.LITERAL_CHAR] = "'";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.LITERAL_STRING] = "\"";
          AllowedTokenEndChars[Types.TokenInfo.TokenType.LITERAL_STRING] = "\"";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.LITERAL_NUMBER] = "0123456789abcdefABCDEF$%";
          AllowedTokenChars[Types.TokenInfo.TokenType.LITERAL_NUMBER] = "0123456789abcdefABCDEFx";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.COMMENT] = ";";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.MACRO] = "!";
          AllowedTokenChars[Types.TokenInfo.TokenType.MACRO] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_INTERNAL] = "+-";

          AllowedSingleTokens = ",#(){}*╚╝";

          AddMacro( "!BYTE", Types.MacroInfo.MacroType.BYTE );
          AddMacro( "!BY", Types.MacroInfo.MacroType.BYTE );
          AddMacro( "!BASIC", Types.MacroInfo.MacroType.BASIC );
          AddMacro( "!8", Types.MacroInfo.MacroType.BYTE );
          AddMacro( "!08", Types.MacroInfo.MacroType.BYTE );
          AddMacro( "!WORD", Types.MacroInfo.MacroType.WORD );
          AddMacro( "!WO", Types.MacroInfo.MacroType.WORD );
          AddMacro( "!16", Types.MacroInfo.MacroType.WORD );
          AddMacro( "!TEXT", Types.MacroInfo.MacroType.TEXT );
          AddMacro( "!TX", Types.MacroInfo.MacroType.TEXT );
          AddMacro( "!SCR", Types.MacroInfo.MacroType.TEXT_SCREEN );
          AddMacro( "!PET", Types.MacroInfo.MacroType.TEXT_PET );
          AddMacro( "!RAW", Types.MacroInfo.MacroType.TEXT_RAW );
          AddMacro( "!PSEUDOPC", Types.MacroInfo.MacroType.PSEUDO_PC );
          AddMacro( "!REALPC", Types.MacroInfo.MacroType.REAL_PC );
          AddMacro( "!BANK", Types.MacroInfo.MacroType.BANK );
          AddMacro( "!CONVTAB", Types.MacroInfo.MacroType.CONVERSION_TAB );
          AddMacro( "!CT", Types.MacroInfo.MacroType.CONVERSION_TAB );
          AddMacro( "!BINARY", Types.MacroInfo.MacroType.INCLUDE_BINARY );
          AddMacro( "!BIN", Types.MacroInfo.MacroType.INCLUDE_BINARY );
          AddMacro( "!BI", Types.MacroInfo.MacroType.INCLUDE_BINARY );
          AddMacro( "!SOURCE", Types.MacroInfo.MacroType.INCLUDE_SOURCE );
          AddMacro( "!SRC", Types.MacroInfo.MacroType.INCLUDE_SOURCE );
          AddMacro( "!TO", Types.MacroInfo.MacroType.COMPILE_TARGET );
          AddMacro( "!ZONE", Types.MacroInfo.MacroType.ZONE );
          AddMacro( "!ZN", Types.MacroInfo.MacroType.ZONE );
          AddMacro( "!ERROR", Types.MacroInfo.MacroType.ERROR );
          AddMacro( "!SERIOUS", Types.MacroInfo.MacroType.ERROR );
          AddMacro( "!WARN", Types.MacroInfo.MacroType.WARN );
          AddMacro( "!MESSAGE", Types.MacroInfo.MacroType.MESSAGE );
          AddMacro( "!IFDEF", Types.MacroInfo.MacroType.IFDEF );
          AddMacro( "!IFNDEF", Types.MacroInfo.MacroType.IFNDEF );
          AddMacro( "!IF", Types.MacroInfo.MacroType.IF );
          AddMacro( "!FILL", Types.MacroInfo.MacroType.FILL );
          AddMacro( "!FI", Types.MacroInfo.MacroType.FILL );
          AddMacro( "!ALIGN", Types.MacroInfo.MacroType.ALIGN );
          AddMacro( "!ENDOFFILE", Types.MacroInfo.MacroType.END_OF_FILE );
          AddMacro( "!EOF", Types.MacroInfo.MacroType.END_OF_FILE );
          AddMacro( "!NOWARN", Types.MacroInfo.MacroType.NO_WARNING );
          AddMacro( "!FOR", Types.MacroInfo.MacroType.FOR );
          AddMacro( "!END", Types.MacroInfo.MacroType.END );
          AddMacro( "!MACRO", Types.MacroInfo.MacroType.MACRO );
          AddMacro( "!TRACE", Types.MacroInfo.MacroType.TRACE );
          AddMacro( "!MEDIA", Types.MacroInfo.MacroType.INCLUDE_MEDIA );
          AddMacro( "!MEDIASRC", Types.MacroInfo.MacroType.INCLUDE_MEDIA_SOURCE );
          AddMacro( "!SL", Types.MacroInfo.MacroType.LABEL_FILE );
          AddMacro( "!CPU", Types.MacroInfo.MacroType.IGNORE );
          AddMacro( "!SET", Types.MacroInfo.MacroType.SET );

          // helper pseudo ops from ACME to generate some address vs. value warnings
          AddMacro( "!ADDR", Types.MacroInfo.MacroType.IGNORE );
          AddMacro( "!ADDRESS", Types.MacroInfo.MacroType.IGNORE );

          MacroPrefix = "!";
          MacroFunctionCallPrefix = "+";
          GlobalLabelsAutoZone = false;
          DefineSeparatorKeywords.Add( "=" );
          IncludeExpectsStringLiteral = true;
          StatementSeparatorChars.Add( ':' );
          break;
        case Types.AssemblerType.DASM:
          AllowedTokenStartChars[Types.TokenInfo.TokenType.LABEL_GLOBAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzÄÖÜäöü";
          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_GLOBAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_äöüÄÖÜß.";
          AllowedTokenEndChars[Types.TokenInfo.TokenType.LABEL_GLOBAL] = "#:";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.LABEL_LOCAL] = ".";
          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_LOCAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_äöüÄÖÜß.";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.LITERAL_CHAR] = "'";
          AllowedTokenEndChars[Types.TokenInfo.TokenType.LITERAL_CHAR] = "'";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.LITERAL_STRING] = "\"";
          AllowedTokenEndChars[Types.TokenInfo.TokenType.LITERAL_STRING] = "\"";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.LITERAL_NUMBER] = "0123456789abcdefABCDEF$%";
          AllowedTokenChars[Types.TokenInfo.TokenType.LITERAL_NUMBER] = "0123456789abcdefABCDEFx";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.COMMENT] = ";";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.MACRO] = "!";
          AllowedTokenChars[Types.TokenInfo.TokenType.MACRO] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.CALL_MACRO] = ":";
          AllowedTokenChars[Types.TokenInfo.TokenType.CALL_MACRO] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_äöüÄÖÜß.";

          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_INTERNAL] = "+-";

          AllowedSingleTokens = ",#(){}*";

          AddMacro( "DC.B", Types.MacroInfo.MacroType.TEXT );
          AddMacro( "DC.W", Types.MacroInfo.MacroType.WORD );
          AddMacro( "MAC", Types.MacroInfo.MacroType.MACRO );
          AddMacro( "ENDM", Types.MacroInfo.MacroType.END );
          AddMacro( "!TEXT", Types.MacroInfo.MacroType.TEXT );
          AddMacro( "!TX", Types.MacroInfo.MacroType.TEXT );
          AddMacro( "!SCR", Types.MacroInfo.MacroType.TEXT_SCREEN );
          AddMacro( "RORG", Types.MacroInfo.MacroType.PSEUDO_PC );
          AddMacro( "REND", Types.MacroInfo.MacroType.REAL_PC );
          AddMacro( "!BANK", Types.MacroInfo.MacroType.BANK );
          AddMacro( "!CONVTAB", Types.MacroInfo.MacroType.CONVERSION_TAB );
          AddMacro( "!CT", Types.MacroInfo.MacroType.CONVERSION_TAB );
          AddMacro( "INCBIN", Types.MacroInfo.MacroType.INCLUDE_BINARY );
          AddMacro( "INCLUDE", Types.MacroInfo.MacroType.INCLUDE_SOURCE );
          AddMacro( "!TO", Types.MacroInfo.MacroType.COMPILE_TARGET );
          AddMacro( "SUBROUTINE", Types.MacroInfo.MacroType.ZONE );
          AddMacro( "!ERROR", Types.MacroInfo.MacroType.ERROR );
          AddMacro( "!IFDEF", Types.MacroInfo.MacroType.IFDEF );
          AddMacro( "!IFNDEF", Types.MacroInfo.MacroType.IFNDEF );
          AddMacro( "IF", Types.MacroInfo.MacroType.IF );
          AddMacro( "ELSE", Types.MacroInfo.MacroType.ELSE );
          AddMacro( "ENDIF", Types.MacroInfo.MacroType.END_IF );
          AddMacro( "DS.Z", Types.MacroInfo.MacroType.FILL );
          AddMacro( "DS", Types.MacroInfo.MacroType.FILL );
          AddMacro( "DS.B", Types.MacroInfo.MacroType.FILL );
          AddMacro( "ALIGN", Types.MacroInfo.MacroType.ALIGN_DASM );
          AddMacro( "!ENDOFFILE", Types.MacroInfo.MacroType.END_OF_FILE );

          AddMacro( "REPEAT", Types.MacroInfo.MacroType.LOOP_START );
          AddMacro( "REPEND", Types.MacroInfo.MacroType.LOOP_END );

          AddMacro( "PROCESSOR", Types.MacroInfo.MacroType.IGNORE );
          AddMacro( "ORG", Types.MacroInfo.MacroType.ORG );
          AddMacro( "SEG", Types.MacroInfo.MacroType.SEG );
          AddMacro( "SEG.U", Types.MacroInfo.MacroType.SEG );

          LabelPostfix = ":";
          MacroFunctionCallPrefix = ":";
          GlobalLabelsAutoZone = false;
          DefineSeparatorKeywords.Add( "SET" );
          DefineSeparatorKeywords.Add( "=" );
          MacroIsZone = true;
          MacrosHaveVariableNumberOfArguments = true;
          IncludeExpectsStringLiteral = false;
          break;
        case Types.AssemblerType.PDS:
          AllowedTokenStartChars[Types.TokenInfo.TokenType.LABEL_GLOBAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzÄÖÜäöü";
          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_GLOBAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_äöüÄÖÜß.";
          AllowedTokenEndChars[Types.TokenInfo.TokenType.LABEL_GLOBAL] = "#";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.LABEL_LOCAL] = ".!";
          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_LOCAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_äöüÄÖÜß.";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.LITERAL_CHAR] = "'";
          AllowedTokenEndChars[Types.TokenInfo.TokenType.LITERAL_CHAR] = "'";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.LITERAL_STRING] = "\"";
          AllowedTokenEndChars[Types.TokenInfo.TokenType.LITERAL_STRING] = "\"";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.LITERAL_NUMBER] = "0123456789abcdefABCDEF$%";
          AllowedTokenChars[Types.TokenInfo.TokenType.LITERAL_NUMBER] = "0123456789abcdefABCDEFx";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.COMMENT] = ";";
          AllowedTokenStartChars[Types.TokenInfo.TokenType.COMMENT_IF_FIRST_CHAR] = "*";

          /*
          AllowedTokenStartChars[Types.TokenInfo.TokenType.MACRO] = "!";
          AllowedTokenChars[Types.TokenInfo.TokenType.MACRO] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
          */

          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_INTERNAL] = "+-";

          AllowedSingleTokens = ",#(){}*";

          AddMacro( "DC.B", Types.MacroInfo.MacroType.TEXT );
          AddMacro( "DC.V", Types.MacroInfo.MacroType.TEXT );
          AddMacro( "DB", Types.MacroInfo.MacroType.TEXT );
          AddMacro( "DH", Types.MacroInfo.MacroType.HIGH_BYTE );
          AddMacro( "DL", Types.MacroInfo.MacroType.LOW_BYTE );
          AddMacro( "DW", Types.MacroInfo.MacroType.WORD );
          AddMacro( "HEX", Types.MacroInfo.MacroType.HEX );
          AddMacro( "!WORD", Types.MacroInfo.MacroType.WORD );
          AddMacro( "!WO", Types.MacroInfo.MacroType.WORD );
          AddMacro( "!16", Types.MacroInfo.MacroType.WORD );
          AddMacro( "!TEXT", Types.MacroInfo.MacroType.TEXT );
          AddMacro( "!TX", Types.MacroInfo.MacroType.TEXT );
          AddMacro( "!SCR", Types.MacroInfo.MacroType.TEXT_SCREEN );
          AddMacro( "CBM", Types.MacroInfo.MacroType.TEXT_SCREEN );
          AddMacro( "!PSEUDOPC", Types.MacroInfo.MacroType.PSEUDO_PC );
          AddMacro( "!REALPC", Types.MacroInfo.MacroType.REAL_PC );
          AddMacro( "!BANK", Types.MacroInfo.MacroType.BANK );
          AddMacro( "!CONVTAB", Types.MacroInfo.MacroType.CONVERSION_TAB );
          AddMacro( "!CT", Types.MacroInfo.MacroType.CONVERSION_TAB );
          AddMacro( "!BINARY", Types.MacroInfo.MacroType.INCLUDE_BINARY );
          AddMacro( "!BIN", Types.MacroInfo.MacroType.INCLUDE_BINARY );
          AddMacro( "!BI", Types.MacroInfo.MacroType.INCLUDE_BINARY );
          AddMacro( "!SOURCE", Types.MacroInfo.MacroType.INCLUDE_SOURCE );
          AddMacro( "!SRC", Types.MacroInfo.MacroType.INCLUDE_SOURCE );
          AddMacro( "!TO", Types.MacroInfo.MacroType.COMPILE_TARGET );
          AddMacro( "!ZONE", Types.MacroInfo.MacroType.ZONE );
          AddMacro( "!ZN", Types.MacroInfo.MacroType.ZONE );
          AddMacro( "!ERROR", Types.MacroInfo.MacroType.ERROR );
          AddMacro( "!IFDEF", Types.MacroInfo.MacroType.IFDEF );
          AddMacro( "!IFNDEF", Types.MacroInfo.MacroType.IFNDEF );
          AddMacro( "!IF", Types.MacroInfo.MacroType.IF );
          AddMacro( "!FILL", Types.MacroInfo.MacroType.FILL );
          AddMacro( "DS", Types.MacroInfo.MacroType.FILL );
          AddMacro( "!FI", Types.MacroInfo.MacroType.FILL );
          AddMacro( "!ALIGN", Types.MacroInfo.MacroType.ALIGN );
          AddMacro( "!ENDOFFILE", Types.MacroInfo.MacroType.END_OF_FILE );
          AddMacro( "DO", Types.MacroInfo.MacroType.LOOP_START );
          AddMacro( "LOOP", Types.MacroInfo.MacroType.LOOP_END );

          AddMacro( "PROCESSOR", Types.MacroInfo.MacroType.IGNORE );
          AddMacro( "ORG", Types.MacroInfo.MacroType.ORG );
          AddMacro( "FREE", Types.MacroInfo.MacroType.IGNORE );
          AddMacro( "SEND", Types.MacroInfo.MacroType.IGNORE );
          AddMacro( "SKIP", Types.MacroInfo.MacroType.IGNORE );
          AddMacro( "END", Types.MacroInfo.MacroType.END_OF_FILE );

          LabelPostfix = ":";
          GlobalLabelsAutoZone = true;
          DefineSeparatorKeywords.Add( "EQU" );
          DefineSeparatorKeywords.Add( "=" );
          CaseSensitive = false;
          IncludeExpectsStringLiteral = true;
          StatementSeparatorChars.Add( ':' );
          break;
        case Types.AssemblerType.C64ASM:
          AllowedTokenStartChars[Types.TokenInfo.TokenType.LABEL_GLOBAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzÄÖÜäöü";
          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_GLOBAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_äöüÄÖÜß.";
          AllowedTokenEndChars[Types.TokenInfo.TokenType.LABEL_GLOBAL] = "#";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.LABEL_LOCAL] = "_";
          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_LOCAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_äöüÄÖÜß.";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.LITERAL_CHAR] = "'";
          AllowedTokenEndChars[Types.TokenInfo.TokenType.LITERAL_CHAR] = "'";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.LITERAL_STRING] = "\"";
          AllowedTokenEndChars[Types.TokenInfo.TokenType.LITERAL_STRING] = "\"";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.LITERAL_NUMBER] = "0123456789abcdefABCDEF$%";
          AllowedTokenChars[Types.TokenInfo.TokenType.LITERAL_NUMBER] = "0123456789abcdefABCDEFx";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.COMMENT] = ";";
          //AllowedTokenStartChars[Types.TokenInfo.TokenType.COMMENT_IF_FIRST_CHAR] = "*";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.MACRO] = ".";
          AllowedTokenChars[Types.TokenInfo.TokenType.MACRO] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_INTERNAL] = "+-";

          AllowedSingleTokens = ",#(){}*";

          MacroPrefix = ".";

          AddMacro( ".BYTE", Types.MacroInfo.MacroType.BYTE );
          AddMacro( ".WORD", Types.MacroInfo.MacroType.WORD );

          GlobalLabelsAutoZone = true;
          DefineSeparatorKeywords.Add( "=" );
          CaseSensitive = false;
          IncludeExpectsStringLiteral = true;
          break;
      }
    }

  };
}
