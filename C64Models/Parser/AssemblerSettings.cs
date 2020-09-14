using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio.Parser
{
  public class AssemblerSettings
  {
    public GR.Collections.Map<string,int>                           OperatorPrecedence = new GR.Collections.Map<string,int>();
    public GR.Collections.Map<Types.TokenInfo.TokenType, string>    AllowedTokenStartChars = new GR.Collections.Map<Types.TokenInfo.TokenType, string>();
    public GR.Collections.Map<Types.TokenInfo.TokenType, string>    AllowedTokenChars = new GR.Collections.Map<Types.TokenInfo.TokenType, string>();
    public GR.Collections.Map<Types.TokenInfo.TokenType, string>    AllowedTokenEndChars = new GR.Collections.Map<Types.TokenInfo.TokenType, string>();
    public string                                                   AllowedSingleTokens;
    public GR.Collections.Set<Types.MacroInfo.MacroType>            RestOfLineAsSingleToken = new GR.Collections.Set<Types.MacroInfo.MacroType>();
    public string                                                   OpenBracketChars = "";
    public string                                                   CloseBracketChars = "";
    public string                                                   LineSeparatorChars = "";
    public GR.Collections.Map<string, Types.MacroInfo>              Macros = new GR.Collections.Map<string, Types.MacroInfo>();
    public string                                                   MacroPrefix = "";
    public string                                                   LabelPostfix = "";
    public string                                                   MacroFunctionCallPrefix = "";                                               
    public bool                                                     GlobalLabelsAutoZone = false;
    public bool                                                     MacroIsZone = false;
    public bool                                                     MacrosHaveVariableNumberOfArguments = false;
    public bool                                                     MacroKeywordAfterName = false;
    public bool                                                     MacrosUseCheapLabelsAsParameters = false;
    public bool                                                     DoWithoutParameterIsUntil = false;
    public bool                                                     LabelsMustBeAtStartOfLine = false;
    public GR.Collections.Set<string>                               DefineSeparatorKeywords = new GR.Collections.Set<string>();
    public bool                                                     CaseSensitive = true;
    public bool                                                     IncludeExpectsStringLiteral = true;
    public bool                                                     IncludeHasOnlyFilename = false;
    public bool                                                     IncludeSourceIsAlwaysUsingLibraryPathAndFile = false;
    public bool                                                     HasBinaryNot = true;
    public bool                                                     GreaterOrLessThanAtBeginningAffectFullExpression = false;
    public GR.Collections.Set<char>                                 StatementSeparatorChars = new GR.Collections.Set<char>();

    public const string                 INTERNAL_OPENING_BRACE = "╚";
    public const string                 INTERNAL_CLOSING_BRACE = "╝";
    public const string                 INTERNAL_LOCAL_LOOP_LABEL_PREFIX = "_C64STUDIO_LL_";
    public const string                 SQUARE_BRACKETS_OPEN = "[";
    public const string                 SQUARE_BRACKETS_CLOSE = "]";



    public void AddMacro( string Macro, Types.MacroInfo.MacroType Type )
    {
      Macros[Macro] = new Types.MacroInfo();
      Macros[Macro].Keyword = Macro;
      Macros[Macro].Type = Type;
    }



    public void SetAssemblerType( Types.AssemblerType Type )
    {
      // set default settings
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
      IncludeExpectsStringLiteral = true;
      IncludeHasOnlyFilename = false;
      HasBinaryNot = true;
      MacroKeywordAfterName = false;
      MacrosUseCheapLabelsAsParameters = false;
      LabelsMustBeAtStartOfLine = false;
      GreaterOrLessThanAtBeginningAffectFullExpression = false;

      OperatorPrecedence.Clear();
      OperatorPrecedence["-"] = 0;
      OperatorPrecedence["+"] = 0;
      OperatorPrecedence["/"] = 1;
      OperatorPrecedence["*"] = 1;
      OperatorPrecedence["%"] = 2;
      OperatorPrecedence["EOR"] = 3;
      OperatorPrecedence["eor"] = 3;
      OperatorPrecedence["XOR"] = 3;
      OperatorPrecedence["xor"] = 3;
      OperatorPrecedence["^"] = 6;
      OperatorPrecedence["OR"] = 4;
      OperatorPrecedence["or"] = 4;
      OperatorPrecedence["|"] = 4;
      OperatorPrecedence["AND"] = 5;
      OperatorPrecedence["and"] = 5;
      OperatorPrecedence["&"] = 5;
      OperatorPrecedence[">>"] = 6;
      OperatorPrecedence["<<"] = 6;
      OperatorPrecedence["<>"] = 6;
      OperatorPrecedence[">="] = 6;
      OperatorPrecedence["<="] = 6;
      OperatorPrecedence["!="] = 6;
      OperatorPrecedence["="] = 6;
      OperatorPrecedence[">"] = 7;
      OperatorPrecedence["<"] = 7;
      OperatorPrecedence["!"] = 7;
      OperatorPrecedence["~"] = 7;

      switch ( Type )
      {
        case Types.AssemblerType.C64_STUDIO:
          AllowedTokenStartChars[Types.TokenInfo.TokenType.LABEL_GLOBAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzÄÖÜäöü_";
          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_GLOBAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_äöüÄÖÜß.";
          AllowedTokenEndChars[Types.TokenInfo.TokenType.LABEL_GLOBAL] = "#";

          OpenBracketChars = "(" + INTERNAL_OPENING_BRACE + SQUARE_BRACKETS_OPEN;
          CloseBracketChars = ")" + INTERNAL_CLOSING_BRACE + SQUARE_BRACKETS_CLOSE;

          LineSeparatorChars = ":";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.LABEL_LOCAL] = ".";
          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_LOCAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_äöüÄÖÜß.";
          AllowedTokenStartChars[Types.TokenInfo.TokenType.LABEL_CHEAP_LOCAL] = "@";
          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_CHEAP_LOCAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_äöüÄÖÜß.";

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

          AllowedSingleTokens = ",#{}*" + OpenBracketChars + CloseBracketChars;

          AddMacro( "!ADDR", Types.MacroInfo.MacroType.ADDRESS );
          AddMacro( "!ADDRESS", Types.MacroInfo.MacroType.ADDRESS );
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
          AddMacro( "!REALIGN", Types.MacroInfo.MacroType.ALIGN_DASM );
          AddMacro( "!ENDOFFILE", Types.MacroInfo.MacroType.END_OF_FILE );
          AddMacro( "!EOF", Types.MacroInfo.MacroType.END_OF_FILE );
          AddMacro( "!NOWARN", Types.MacroInfo.MacroType.NO_WARNING );
          AddMacro( "!FOR", Types.MacroInfo.MacroType.FOR );
          AddMacro( "!END", Types.MacroInfo.MacroType.END );
          AddMacro( "!MACRO", Types.MacroInfo.MacroType.MACRO );
          AddMacro( "!TRACE", Types.MacroInfo.MacroType.TRACE );
          AddMacro( "!MEDIA", Types.MacroInfo.MacroType.INCLUDE_MEDIA );
          AddMacro( "!MEDIASRC", Types.MacroInfo.MacroType.INCLUDE_MEDIA_SOURCE );
          AddMacro( "!HEX", Types.MacroInfo.MacroType.HEX );
          AddMacro( "!H", Types.MacroInfo.MacroType.HEX );
          AddMacro( "!SL", Types.MacroInfo.MacroType.LABEL_FILE );
          AddMacro( "!CPU", Types.MacroInfo.MacroType.IGNORE );
          AddMacro( "!SET", Types.MacroInfo.MacroType.SET );

          // helper pseudo ops from ACME to generate some address vs. value warnings
          //AddMacro( "!ADDR", Types.MacroInfo.MacroType.IGNORE );
          //AddMacro( "!ADDRESS", Types.MacroInfo.MacroType.IGNORE );

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

          OpenBracketChars = "(" + INTERNAL_OPENING_BRACE;
          CloseBracketChars = ")" + INTERNAL_CLOSING_BRACE;

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

          AllowedSingleTokens = ",#*" + OpenBracketChars + CloseBracketChars + "\\";

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
          AddMacro( "INCDIR", Types.MacroInfo.MacroType.ADD_INCLUDE_SOURCE );

          RestOfLineAsSingleToken.Add( Types.MacroInfo.MacroType.ADD_INCLUDE_SOURCE );

          LabelPostfix = ":";
          MacroFunctionCallPrefix = ":";
          GlobalLabelsAutoZone = false;
          DefineSeparatorKeywords.Add( "SET" );
          DefineSeparatorKeywords.Add( "EQU" );
          DefineSeparatorKeywords.Add( "=" );
          MacroIsZone = true;
          MacrosHaveVariableNumberOfArguments = true;
          IncludeExpectsStringLiteral = false;
          IncludeHasOnlyFilename = true;
          IncludeSourceIsAlwaysUsingLibraryPathAndFile = true;
          CaseSensitive = false;
          break;
        case Types.AssemblerType.PDS:
          AllowedTokenStartChars[Types.TokenInfo.TokenType.LABEL_GLOBAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzÄÖÜäöü";
          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_GLOBAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_äöüÄÖÜß";
          AllowedTokenEndChars[Types.TokenInfo.TokenType.LABEL_GLOBAL] = "#";

          OpenBracketChars = "([" + INTERNAL_OPENING_BRACE;
          CloseBracketChars = ")]" + INTERNAL_CLOSING_BRACE;

          AllowedTokenStartChars[Types.TokenInfo.TokenType.LABEL_LOCAL] = "!:";
          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_LOCAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_äöüÄÖÜß!";

          // we misuse cheap labels as macro parameters
          AllowedTokenStartChars[Types.TokenInfo.TokenType.LABEL_CHEAP_LOCAL] = "@";
          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_CHEAP_LOCAL] = "0123456789";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.LITERAL_CHAR] = "'";
          AllowedTokenEndChars[Types.TokenInfo.TokenType.LITERAL_CHAR] = "'";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.LITERAL_STRING] = "\"";
          AllowedTokenEndChars[Types.TokenInfo.TokenType.LITERAL_STRING] = "\"";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.LITERAL_NUMBER] = "0123456789abcdefABCDEF&$%";
          AllowedTokenChars[Types.TokenInfo.TokenType.LITERAL_NUMBER] = "0123456789abcdefABCDEFx";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.COMMENT] = ";";
          AllowedTokenStartChars[Types.TokenInfo.TokenType.COMMENT_IF_FIRST_CHAR] = "*";

          AllowedTokenChars[Types.TokenInfo.TokenType.SINGLE_CHAR] = "\\.";
          AllowedTokenStartChars[Types.TokenInfo.TokenType.SINGLE_CHAR] = "\\.";

          /*
          AllowedTokenStartChars[Types.TokenInfo.TokenType.MACRO] = "!";
          AllowedTokenChars[Types.TokenInfo.TokenType.MACRO] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
          */

          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_INTERNAL] = "+-";

          AllowedSingleTokens = ",#*" + OpenBracketChars + CloseBracketChars;

          AddMacro( "DC.B", Types.MacroInfo.MacroType.TEXT );
          AddMacro( "DC.V", Types.MacroInfo.MacroType.TEXT );
          AddMacro( "DB", Types.MacroInfo.MacroType.TEXT );
          AddMacro( "DEFM", Types.MacroInfo.MacroType.TEXT );
          AddMacro( "DM", Types.MacroInfo.MacroType.TEXT );
          AddMacro( "DFM", Types.MacroInfo.MacroType.TEXT );
          AddMacro( "DH", Types.MacroInfo.MacroType.HIGH_BYTE );
          AddMacro( "DL", Types.MacroInfo.MacroType.LOW_BYTE );
          AddMacro( "DW", Types.MacroInfo.MacroType.WORD );
          AddMacro( "HEX", Types.MacroInfo.MacroType.HEX );
          AddMacro( "CBM", Types.MacroInfo.MacroType.TEXT_SCREEN );
          AddMacro( "INCBIN", Types.MacroInfo.MacroType.INCLUDE_BINARY );
          AddMacro( "INCLUDE", Types.MacroInfo.MacroType.INCLUDE_SOURCE );
          AddMacro( "ERROR", Types.MacroInfo.MacroType.ERROR );
          AddMacro( "IF", Types.MacroInfo.MacroType.IF );
          AddMacro( "ENDIF", Types.MacroInfo.MacroType.END_IF );
          AddMacro( "ELSE", Types.MacroInfo.MacroType.ELSE );
          AddMacro( "DS", Types.MacroInfo.MacroType.FILL );
          AddMacro( "DO", Types.MacroInfo.MacroType.LOOP_START );
          AddMacro( "LOOP", Types.MacroInfo.MacroType.LOOP_END );
          AddMacro( "UNTIL", Types.MacroInfo.MacroType.LOOP_END );
          AddMacro( "MACRO", Types.MacroInfo.MacroType.MACRO );
          AddMacro( "ENDM", Types.MacroInfo.MacroType.END );

          AddMacro( "PROCESSOR", Types.MacroInfo.MacroType.IGNORE );
          AddMacro( "ORG", Types.MacroInfo.MacroType.ORG );
          AddMacro( "FREE", Types.MacroInfo.MacroType.IGNORE );
          AddMacro( "SEND", Types.MacroInfo.MacroType.IGNORE );
          AddMacro( "SKIP", Types.MacroInfo.MacroType.IGNORE );
          AddMacro( "INFO", Types.MacroInfo.MacroType.IGNORE );
          AddMacro( "RUN", Types.MacroInfo.MacroType.IGNORE );

          AddMacro( "EXEC", Types.MacroInfo.MacroType.IGNORE );
          AddMacro( "START", Types.MacroInfo.MacroType.IGNORE );

          AddMacro( "DSECT", Types.MacroInfo.MacroType.IGNORE );
          AddMacro( "DEND", Types.MacroInfo.MacroType.IGNORE );

          AddMacro( "MSW", Types.MacroInfo.MacroType.IGNORE );

          AddMacro( "END", Types.MacroInfo.MacroType.END_OF_FILE );
          AddMacro( "REPEAT", Types.MacroInfo.MacroType.REPEAT );

          OperatorPrecedence["!"] = 4;

          LabelPostfix = ":";
          GlobalLabelsAutoZone = true;
          DefineSeparatorKeywords.Add( "EQU" );
          DefineSeparatorKeywords.Add( "=" );
          CaseSensitive = false;
          IncludeExpectsStringLiteral = false;
          IncludeHasOnlyFilename = true;
          StatementSeparatorChars.Add( ':' );
          MacroKeywordAfterName = true;
          DoWithoutParameterIsUntil = true;
          MacrosHaveVariableNumberOfArguments = true;
          MacrosUseCheapLabelsAsParameters = true;
          HasBinaryNot = false;
          LabelsMustBeAtStartOfLine = true;
          GreaterOrLessThanAtBeginningAffectFullExpression = true;
          break;
        case Types.AssemblerType.C64ASM:
          AllowedTokenStartChars[Types.TokenInfo.TokenType.LABEL_GLOBAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzÄÖÜäöü";
          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_GLOBAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_äöüÄÖÜß.";
          AllowedTokenEndChars[Types.TokenInfo.TokenType.LABEL_GLOBAL] = "#";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.LABEL_LOCAL] = "_";
          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_LOCAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_äöüÄÖÜß.";

          OpenBracketChars = "(" + INTERNAL_OPENING_BRACE;
          CloseBracketChars = ")" + INTERNAL_CLOSING_BRACE;

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

          AllowedSingleTokens = ",#{}*" + OpenBracketChars + CloseBracketChars;

          MacroPrefix = ".";

          AddMacro( ".BYTE", Types.MacroInfo.MacroType.BYTE );
          AddMacro( ".WORD", Types.MacroInfo.MacroType.WORD );

          GlobalLabelsAutoZone = true;
          DefineSeparatorKeywords.Add( "=" );
          CaseSensitive = false;
          IncludeExpectsStringLiteral = true;
          break;
        case Types.AssemblerType.CBMPRGSTUDIO:
          AllowedTokenStartChars[Types.TokenInfo.TokenType.LABEL_GLOBAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzÄÖÜäöü";
          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_GLOBAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_äöüÄÖÜß.";
          AllowedTokenEndChars[Types.TokenInfo.TokenType.LABEL_GLOBAL] = "#:";

          OpenBracketChars = "(" + INTERNAL_OPENING_BRACE;
          CloseBracketChars = ")" + INTERNAL_CLOSING_BRACE;

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

          AllowedSingleTokens = ",#*" + OpenBracketChars + CloseBracketChars;

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
          AddMacro( "BYTE", Types.MacroInfo.MacroType.BYTE );

          LabelPostfix = ":";
          MacroFunctionCallPrefix = ":";
          GlobalLabelsAutoZone = false;
          DefineSeparatorKeywords.Add( "SET" );
          DefineSeparatorKeywords.Add( "=" );
          MacroIsZone = true;
          MacrosHaveVariableNumberOfArguments = true;
          IncludeExpectsStringLiteral = false;
          break;
      }
    }

  };
}
