using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RetroDevStudio.Parser
{
  public class AssemblerSettings
  {
    public enum Hacks
    {
      [Description( "Allows .byte/.word pseudo op additionally to the proper !byte/!word pseudo op" )]
      ALLOW_DOT_BYTE_INSTRUCTION    = 1
    }

    public GR.Collections.Map<string,int>                           OperatorPrecedence = new GR.Collections.Map<string,int>();
    public GR.Collections.Map<Types.TokenInfo.TokenType, string>    AllowedTokenStartChars = new GR.Collections.Map<Types.TokenInfo.TokenType, string>();
    public GR.Collections.Map<Types.TokenInfo.TokenType, string>    AllowedTokenChars = new GR.Collections.Map<Types.TokenInfo.TokenType, string>();
    public GR.Collections.Map<Types.TokenInfo.TokenType, string>    AllowedTokenEndChars = new GR.Collections.Map<Types.TokenInfo.TokenType, string>();
    public string                                                   AllowedSingleTokens;
    public GR.Collections.Set<Types.MacroInfo.PseudoOpType>         RestOfLineAsSingleToken = new GR.Collections.Set<Types.MacroInfo.PseudoOpType>();
    public string                                                   OpenBracketChars = "";
    public string                                                   CloseBracketChars = "";
    public string                                                   LineSeparatorChars = "";
    public GR.Collections.Map<string, Types.MacroInfo>              PseudoOps = new GR.Collections.Map<string, Types.MacroInfo>();
    public string                                                   POPrefix = "";
    public string                                                   LabelPostfix = "";
    public List<string>                                             MacroFunctionCallPrefix = new List<string>();
    public bool                                                     GlobalLabelsAutoZone = false;
    public bool                                                     MacroIsZone = false;
    public bool                                                     MacrosHaveVariableNumberOfArguments = false;
    public bool                                                     MacrosCanBeOverloaded = false;
    public bool                                                     MacroKeywordAfterName = false;
    public bool                                                     MacrosUseCheapLabelsAsParameters = false;
    public bool                                                     DoWithoutParameterIsUntil = false;
    public bool                                                     LabelsMustBeAtStartOfLine = false;
    public bool                                                     CaseSensitive = true;
    public bool                                                     IncludeExpectsStringLiteral = true;
    public bool                                                     LoopEndHasNoScope = false;
    public bool                                                     IncludeHasOnlyFilename = false;
    public bool                                                     IncludeSourceIsAlwaysUsingLibraryPathAndFile = false;
    public bool                                                     HasBinaryNot = true;          // PDS has IF LEV=2 ! LEV=3 ! LEV=5
    public bool                                                     GreaterOrLessThanAtBeginningAffectFullExpression = false;
    public bool                                                     MessageAutoIncludesBlanksBetweenParameters = false;
    public bool                                                     AllowsCustomTextMappings = false;
    public bool                                                     IfWithoutBrackets = false;
    public bool                                                     LocalLabelStacking = false;   // if true, TASM mode: ++ refers to 2nd + below, --- to 3rd - above, etc.
    public GR.Collections.Set<string>                               DefineSeparatorKeywords = new GR.Collections.Set<string>();
    public GR.Collections.Set<string>                               PlainAssignmentOperators = new GR.Collections.Set<string>();
    public GR.Collections.Set<char>                                 StatementSeparatorChars = new GR.Collections.Set<char>();
    public GR.Collections.Set<Hacks>                                EnabledHacks = new GR.Collections.Set<Hacks>();

    public Types.AssemblerType                                      AssemblerType = Types.AssemblerType.AUTO;

    public Types.CompileTargetType                                  DefaultTargetType = Types.CompileTargetType.PRG;
    public string                                                   DefaultTargetExtension = ".prg";

    public string                                                   OpcodeSizeIdentifierSeparator = ".";
    public List<string>                                             OpcodeSizeIdentifierOneByteOperands = new List<string>();
    public List<string>                                             OpcodeSizeIdentifierTwoByteOperands = new List<string>();



    public const string                 INTERNAL_OPENING_BRACE = "╚";
    public const string                 INTERNAL_CLOSING_BRACE = "╝";
    public const string                 INTERNAL_LOCAL_LOOP_LABEL_PREFIX = "_C64STUDIO_LL_";
    public const string                 SQUARE_BRACKETS_OPEN = "[";
    public const string                 SQUARE_BRACKETS_CLOSE = "]";



    public void AddPseudoOp( string PseudoOp, Types.MacroInfo.PseudoOpType Type )
    {
      PseudoOps[PseudoOp] = new Types.MacroInfo();
      PseudoOps[PseudoOp].Keyword = PseudoOp;
      PseudoOps[PseudoOp].Type = Type;
    }



    public void SetAssemblerType( Types.AssemblerType Type )
    {
      // set default settings
      AllowedTokenChars.Clear();
      AllowedTokenEndChars.Clear();
      AllowedTokenStartChars.Clear();
      AllowedSingleTokens = "";
      RestOfLineAsSingleToken.Clear();
      OpenBracketChars = "";
      CloseBracketChars = "";
      LineSeparatorChars = "";
      PseudoOps.Clear();
      POPrefix = "";
      LabelPostfix = "";
      MacroFunctionCallPrefix.Clear();
      GlobalLabelsAutoZone = false;
      MacroIsZone = false;
      MacrosHaveVariableNumberOfArguments = false;
      MacrosCanBeOverloaded = false;
      MacroKeywordAfterName = false;
      MacrosUseCheapLabelsAsParameters = false;
      DoWithoutParameterIsUntil = false;
      LabelsMustBeAtStartOfLine = false;
      CaseSensitive = true;
      IncludeExpectsStringLiteral = true;
      LoopEndHasNoScope = false;
      IncludeHasOnlyFilename = false;
      IncludeSourceIsAlwaysUsingLibraryPathAndFile = false;
      HasBinaryNot = true;
      GreaterOrLessThanAtBeginningAffectFullExpression = false;
      MessageAutoIncludesBlanksBetweenParameters = false;
      AllowsCustomTextMappings = false;
      IfWithoutBrackets = false;
      LocalLabelStacking = false;
      DefineSeparatorKeywords.Clear();
      PlainAssignmentOperators.Clear();
      StatementSeparatorChars.Clear();

      AssemblerType = Type;

      DefaultTargetType = Types.CompileTargetType.PRG;
      DefaultTargetExtension = ".prg";

      OperatorPrecedence.Clear();
      OperatorPrecedence["-"] = 0;
      OperatorPrecedence["+"] = 0;
      OperatorPrecedence["/"] = 1;
      OperatorPrecedence["*"] = 1;
      OperatorPrecedence["%"] = 1;
      OperatorPrecedence["EOR"] = 3;
      OperatorPrecedence["eor"] = 3;
      OperatorPrecedence["XOR"] = 3;
      OperatorPrecedence["xor"] = 3;
      OperatorPrecedence["OR"] = 4;
      OperatorPrecedence["or"] = 4;
      OperatorPrecedence["|"] = 4;
      OperatorPrecedence["AND"] = 5;
      OperatorPrecedence["and"] = 5;
      OperatorPrecedence["&"] = 5;

      OperatorPrecedence[">>="] = 6;
      OperatorPrecedence["<<="] = 6;
      OperatorPrecedence["+="] = 6;
      OperatorPrecedence["-="] = 6;
      //OperatorPrecedence["*="] = 6;
      OperatorPrecedence["/="] = 6;
      OperatorPrecedence["%="] = 6;
      OperatorPrecedence["&="] = 6;

      OperatorPrecedence["^"] = 7;
      OperatorPrecedence[">>"] = 7;
      OperatorPrecedence["<<"] = 7;
      OperatorPrecedence["<>"] = 7;
      OperatorPrecedence[">="] = 7;
      OperatorPrecedence["<="] = 7;
      OperatorPrecedence["!="] = 7;
      OperatorPrecedence["="] = 7;
      OperatorPrecedence["=="] = 7;
      OperatorPrecedence[">"] = 8;
      OperatorPrecedence["<"] = 8;
      OperatorPrecedence["!"] = 8;
      OperatorPrecedence["~"] = 8;

      switch ( Type )
      {
        case Types.AssemblerType.C64_STUDIO:
          AllowedTokenStartChars[Types.TokenInfo.TokenType.LABEL_GLOBAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzÄÖÜäöü_";
          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_GLOBAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_äöüÄÖÜß.";
          AllowedTokenEndChars[Types.TokenInfo.TokenType.LABEL_GLOBAL] = "#'";

          OpenBracketChars = "(" + INTERNAL_OPENING_BRACE + SQUARE_BRACKETS_OPEN;
          CloseBracketChars = ")" + INTERNAL_CLOSING_BRACE + SQUARE_BRACKETS_CLOSE;
          OpcodeSizeIdentifierSeparator = "+";
          OpcodeSizeIdentifierOneByteOperands = new List<string>() { "1" };
          OpcodeSizeIdentifierTwoByteOperands = new List<string>() { "2" };

          LineSeparatorChars = ":";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.LABEL_LOCAL] = ".";
          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_LOCAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_äöüÄÖÜß.";
          AllowedTokenEndChars[Types.TokenInfo.TokenType.LABEL_LOCAL] = "#";
          AllowedTokenStartChars[Types.TokenInfo.TokenType.LABEL_CHEAP_LOCAL] = "@";
          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_CHEAP_LOCAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_äöüÄÖÜß.";
          AllowedTokenEndChars[Types.TokenInfo.TokenType.LABEL_CHEAP_LOCAL] = "#";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.LITERAL_CHAR] = "'";
          AllowedTokenEndChars[Types.TokenInfo.TokenType.LITERAL_CHAR] = "'";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.LITERAL_STRING] = "\"";
          AllowedTokenEndChars[Types.TokenInfo.TokenType.LITERAL_STRING] = "\"";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.LITERAL_NUMBER] = "0123456789abcdefABCDEF$%&";
          AllowedTokenChars[Types.TokenInfo.TokenType.LITERAL_NUMBER] = "0123456789abcdefABCDEFx";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.COMMENT] = ";";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.PSEUDO_OP] = "!";
          AllowedTokenChars[Types.TokenInfo.TokenType.PSEUDO_OP] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_INTERNAL] = "+-";

          AllowedSingleTokens = ",#{}*?" + OpenBracketChars + CloseBracketChars;

          AddPseudoOp( "!ADDR", Types.MacroInfo.PseudoOpType.ADDRESS );
          AddPseudoOp( "!ADDRESS", Types.MacroInfo.PseudoOpType.ADDRESS );
          AddPseudoOp( "!BYTE", Types.MacroInfo.PseudoOpType.BYTE );
          AddPseudoOp( "!BY", Types.MacroInfo.PseudoOpType.BYTE );
          AddPseudoOp( "!BASIC", Types.MacroInfo.PseudoOpType.BASIC );
          AddPseudoOp( "!8", Types.MacroInfo.PseudoOpType.BYTE );
          AddPseudoOp( "!08", Types.MacroInfo.PseudoOpType.BYTE );
          AddPseudoOp( "!WORD", Types.MacroInfo.PseudoOpType.WORD );
          AddPseudoOp( "!WO", Types.MacroInfo.PseudoOpType.WORD );
          AddPseudoOp( "!16", Types.MacroInfo.PseudoOpType.WORD );
          AddPseudoOp( "!LE16", Types.MacroInfo.PseudoOpType.WORD );
          AddPseudoOp( "!BE16", Types.MacroInfo.PseudoOpType.WORD_BE );
          AddPseudoOp( "!DWORD", Types.MacroInfo.PseudoOpType.DWORD );
          AddPseudoOp( "!32", Types.MacroInfo.PseudoOpType.DWORD );
          AddPseudoOp( "!LE32", Types.MacroInfo.PseudoOpType.DWORD );
          AddPseudoOp( "!BE32", Types.MacroInfo.PseudoOpType.DWORD_BE );
          AddPseudoOp( "!TEXT", Types.MacroInfo.PseudoOpType.TEXT );
          AddPseudoOp( "!TX", Types.MacroInfo.PseudoOpType.TEXT );
          AddPseudoOp( "!SCR", Types.MacroInfo.PseudoOpType.TEXT_SCREEN );
          AddPseudoOp( "!SCRXOR", Types.MacroInfo.PseudoOpType.TEXT_SCREEN_XOR );
          AddPseudoOp( "!PET", Types.MacroInfo.PseudoOpType.TEXT_PET );
          AddPseudoOp( "!RAW", Types.MacroInfo.PseudoOpType.TEXT_RAW );
          AddPseudoOp( "!PSEUDOPC", Types.MacroInfo.PseudoOpType.PSEUDO_PC );
          AddPseudoOp( "!REALPC", Types.MacroInfo.PseudoOpType.REAL_PC );
          AddPseudoOp( "!BANK", Types.MacroInfo.PseudoOpType.BANK );
          AddPseudoOp( "!CONVTAB", Types.MacroInfo.PseudoOpType.CONVERSION_TAB );
          AddPseudoOp( "!CT", Types.MacroInfo.PseudoOpType.CONVERSION_TAB );
          AddPseudoOp( "!BINARY", Types.MacroInfo.PseudoOpType.INCLUDE_BINARY );
          AddPseudoOp( "!BIN", Types.MacroInfo.PseudoOpType.INCLUDE_BINARY );
          AddPseudoOp( "!BI", Types.MacroInfo.PseudoOpType.INCLUDE_BINARY );
          AddPseudoOp( "!SOURCE", Types.MacroInfo.PseudoOpType.INCLUDE_SOURCE );
          AddPseudoOp( "!SRC", Types.MacroInfo.PseudoOpType.INCLUDE_SOURCE );
          AddPseudoOp( "!TO", Types.MacroInfo.PseudoOpType.COMPILE_TARGET );
          AddPseudoOp( "!ZONE", Types.MacroInfo.PseudoOpType.ZONE );
          AddPseudoOp( "!ZN", Types.MacroInfo.PseudoOpType.ZONE );
          AddPseudoOp( "!LZONE", Types.MacroInfo.PseudoOpType.LZONE );
          AddPseudoOp( "!ERROR", Types.MacroInfo.PseudoOpType.ERROR );
          AddPseudoOp( "!SERIOUS", Types.MacroInfo.PseudoOpType.ERROR );
          AddPseudoOp( "!WARN", Types.MacroInfo.PseudoOpType.WARN );
          AddPseudoOp( "!MESSAGE", Types.MacroInfo.PseudoOpType.MESSAGE );
          AddPseudoOp( "!IFDEF", Types.MacroInfo.PseudoOpType.IFDEF );
          AddPseudoOp( "!IFNDEF", Types.MacroInfo.PseudoOpType.IFNDEF );
          AddPseudoOp( "!IF", Types.MacroInfo.PseudoOpType.IF );
          AddPseudoOp( "!FILL", Types.MacroInfo.PseudoOpType.FILL );
          AddPseudoOp( "!FI", Types.MacroInfo.PseudoOpType.FILL );
          AddPseudoOp( "!ALIGN", Types.MacroInfo.PseudoOpType.ALIGN );
          AddPseudoOp( "!REALIGN", Types.MacroInfo.PseudoOpType.ALIGN_DASM );
          AddPseudoOp( "!ENDOFFILE", Types.MacroInfo.PseudoOpType.END_OF_FILE );
          AddPseudoOp( "!EOF", Types.MacroInfo.PseudoOpType.END_OF_FILE );
          AddPseudoOp( "!NOWARN", Types.MacroInfo.PseudoOpType.NO_WARNING );
          AddPseudoOp( "!FOR", Types.MacroInfo.PseudoOpType.FOR );
          AddPseudoOp( "!END", Types.MacroInfo.PseudoOpType.END );
          AddPseudoOp( "!MACRO", Types.MacroInfo.PseudoOpType.MACRO );
          AddPseudoOp( "!TRACE", Types.MacroInfo.PseudoOpType.TRACE );
          AddPseudoOp( "!MEDIA", Types.MacroInfo.PseudoOpType.INCLUDE_MEDIA );
          AddPseudoOp( "!MEDIASRC", Types.MacroInfo.PseudoOpType.INCLUDE_MEDIA_SOURCE );
          AddPseudoOp( "!HEX", Types.MacroInfo.PseudoOpType.HEX );
          AddPseudoOp( "!H", Types.MacroInfo.PseudoOpType.HEX );
          AddPseudoOp( "!SL", Types.MacroInfo.PseudoOpType.LABEL_FILE );
          AddPseudoOp( "!CPU", Types.MacroInfo.PseudoOpType.CPU );
          AddPseudoOp( "!SET", Types.MacroInfo.PseudoOpType.SET );
          AddPseudoOp( "!LIST", Types.MacroInfo.PseudoOpType.PREPROCESSED_LIST );
          AddPseudoOp( "!AL", Types.MacroInfo.PseudoOpType.ASSUME_16BIT_ACCUMULATOR_65816 );
          AddPseudoOp( "!AS", Types.MacroInfo.PseudoOpType.ASSUME_8BIT_ACCUMULATOR_65816 );
          AddPseudoOp( "!RL", Types.MacroInfo.PseudoOpType.ASSUME_16BIT_REGISTERS_65816 );
          AddPseudoOp( "!RS", Types.MacroInfo.PseudoOpType.ASSUME_8BIT_REGISTERS_65816 );
          AddPseudoOp( "!SKIP", Types.MacroInfo.PseudoOpType.SKIP );
          AddPseudoOp( "!WHILE", Types.MacroInfo.PseudoOpType.WHILE );

          // helper pseudo ops from ACME to generate some address vs. value warnings
          //AddMacro( "!ADDR", Types.MacroInfo.MacroType.IGNORE );
          //AddMacro( "!ADDRESS", Types.MacroInfo.MacroType.IGNORE );

          POPrefix = "!";
          MacroFunctionCallPrefix.Add( "+" );
          MacrosCanBeOverloaded = true;
          GlobalLabelsAutoZone = false;
          DefineSeparatorKeywords.AddRange( new string[] { "=", ">>=", "<<=", "+=", "-=", "*=", "/=", "%=", "&=" }  );
          PlainAssignmentOperators.AddRange( new string[] { "=" } );
          IncludeExpectsStringLiteral = true;
          StatementSeparatorChars.Add( ':' );
          break;
        case Types.AssemblerType.ACME:
          AllowedTokenStartChars[Types.TokenInfo.TokenType.LABEL_GLOBAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzÄÖÜäöü_";
          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_GLOBAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_äöüÄÖÜß.";
          AllowedTokenEndChars[Types.TokenInfo.TokenType.LABEL_GLOBAL] = "#";

          OpenBracketChars = "(" + INTERNAL_OPENING_BRACE + SQUARE_BRACKETS_OPEN;
          CloseBracketChars = ")" + INTERNAL_CLOSING_BRACE + SQUARE_BRACKETS_CLOSE;

          OpcodeSizeIdentifierSeparator = "+";
          OpcodeSizeIdentifierOneByteOperands = new List<string>() { "1" };
          OpcodeSizeIdentifierTwoByteOperands = new List<string>() { "2" };

          LineSeparatorChars = ":";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.LABEL_LOCAL] = ".";
          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_LOCAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_äöüÄÖÜß.";
          AllowedTokenEndChars[Types.TokenInfo.TokenType.LABEL_LOCAL] = "#";
          AllowedTokenStartChars[Types.TokenInfo.TokenType.LABEL_CHEAP_LOCAL] = "@";
          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_CHEAP_LOCAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_äöüÄÖÜß.";
          AllowedTokenEndChars[Types.TokenInfo.TokenType.LABEL_CHEAP_LOCAL] = "#";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.LITERAL_CHAR] = "'";
          AllowedTokenEndChars[Types.TokenInfo.TokenType.LITERAL_CHAR] = "'";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.LITERAL_STRING] = "\"";
          AllowedTokenEndChars[Types.TokenInfo.TokenType.LITERAL_STRING] = "\"";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.LITERAL_NUMBER] = "0123456789abcdefABCDEF$%";
          AllowedTokenChars[Types.TokenInfo.TokenType.LITERAL_NUMBER] = "0123456789abcdefABCDEFx";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.COMMENT] = ";";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.PSEUDO_OP] = "!";
          AllowedTokenChars[Types.TokenInfo.TokenType.PSEUDO_OP] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_INTERNAL] = "+-";

          AllowedSingleTokens = ",#{}*?" + OpenBracketChars + CloseBracketChars;

          AddPseudoOp( "!ADDR", Types.MacroInfo.PseudoOpType.ADDRESS );
          AddPseudoOp( "!ADDRESS", Types.MacroInfo.PseudoOpType.ADDRESS );
          AddPseudoOp( "!BYTE", Types.MacroInfo.PseudoOpType.BYTE );
          AddPseudoOp( "!BY", Types.MacroInfo.PseudoOpType.BYTE );
          AddPseudoOp( "!BASIC", Types.MacroInfo.PseudoOpType.BASIC );
          AddPseudoOp( "!8", Types.MacroInfo.PseudoOpType.BYTE );
          AddPseudoOp( "!08", Types.MacroInfo.PseudoOpType.BYTE );
          AddPseudoOp( "!WORD", Types.MacroInfo.PseudoOpType.WORD );
          AddPseudoOp( "!WO", Types.MacroInfo.PseudoOpType.WORD );
          AddPseudoOp( "!16", Types.MacroInfo.PseudoOpType.WORD );
          AddPseudoOp( "!LE16", Types.MacroInfo.PseudoOpType.WORD );
          AddPseudoOp( "!BE16", Types.MacroInfo.PseudoOpType.WORD_BE );
          AddPseudoOp( "!DWORD", Types.MacroInfo.PseudoOpType.DWORD );
          AddPseudoOp( "!32", Types.MacroInfo.PseudoOpType.DWORD );
          AddPseudoOp( "!LE32", Types.MacroInfo.PseudoOpType.DWORD );
          AddPseudoOp( "!BE32", Types.MacroInfo.PseudoOpType.DWORD_BE );
          AddPseudoOp( "!TEXT", Types.MacroInfo.PseudoOpType.TEXT );
          AddPseudoOp( "!TX", Types.MacroInfo.PseudoOpType.TEXT );
          AddPseudoOp( "!SCR", Types.MacroInfo.PseudoOpType.TEXT_SCREEN );
          AddPseudoOp( "!PET", Types.MacroInfo.PseudoOpType.TEXT_PET );
          AddPseudoOp( "!RAW", Types.MacroInfo.PseudoOpType.TEXT_RAW );
          AddPseudoOp( "!PSEUDOPC", Types.MacroInfo.PseudoOpType.PSEUDO_PC );
          AddPseudoOp( "!REALPC", Types.MacroInfo.PseudoOpType.REAL_PC );
          AddPseudoOp( "!CONVTAB", Types.MacroInfo.PseudoOpType.CONVERSION_TAB );
          AddPseudoOp( "!CT", Types.MacroInfo.PseudoOpType.CONVERSION_TAB );
          AddPseudoOp( "!BINARY", Types.MacroInfo.PseudoOpType.INCLUDE_BINARY );
          AddPseudoOp( "!BIN", Types.MacroInfo.PseudoOpType.INCLUDE_BINARY );
          AddPseudoOp( "!BI", Types.MacroInfo.PseudoOpType.INCLUDE_BINARY );
          AddPseudoOp( "!SOURCE", Types.MacroInfo.PseudoOpType.INCLUDE_SOURCE );
          AddPseudoOp( "!SRC", Types.MacroInfo.PseudoOpType.INCLUDE_SOURCE );
          AddPseudoOp( "!TO", Types.MacroInfo.PseudoOpType.COMPILE_TARGET );
          AddPseudoOp( "!ZONE", Types.MacroInfo.PseudoOpType.ZONE );
          AddPseudoOp( "!ZN", Types.MacroInfo.PseudoOpType.ZONE );
          AddPseudoOp( "!ERROR", Types.MacroInfo.PseudoOpType.ERROR );
          AddPseudoOp( "!SERIOUS", Types.MacroInfo.PseudoOpType.ERROR );
          AddPseudoOp( "!WARN", Types.MacroInfo.PseudoOpType.WARN );
          AddPseudoOp( "!MESSAGE", Types.MacroInfo.PseudoOpType.MESSAGE );
          AddPseudoOp( "!IFDEF", Types.MacroInfo.PseudoOpType.IFDEF );
          AddPseudoOp( "!IFNDEF", Types.MacroInfo.PseudoOpType.IFNDEF );
          AddPseudoOp( "!IF", Types.MacroInfo.PseudoOpType.IF );
          AddPseudoOp( "!FILL", Types.MacroInfo.PseudoOpType.FILL );
          AddPseudoOp( "!FI", Types.MacroInfo.PseudoOpType.FILL );
          AddPseudoOp( "!ALIGN", Types.MacroInfo.PseudoOpType.ALIGN );
          AddPseudoOp( "!REALIGN", Types.MacroInfo.PseudoOpType.ALIGN_DASM );
          AddPseudoOp( "!ENDOFFILE", Types.MacroInfo.PseudoOpType.END_OF_FILE );
          AddPseudoOp( "!EOF", Types.MacroInfo.PseudoOpType.END_OF_FILE );
          AddPseudoOp( "!NOWARN", Types.MacroInfo.PseudoOpType.NO_WARNING );
          AddPseudoOp( "!FOR", Types.MacroInfo.PseudoOpType.FOR );
          AddPseudoOp( "!END", Types.MacroInfo.PseudoOpType.END );
          AddPseudoOp( "!MACRO", Types.MacroInfo.PseudoOpType.MACRO );
          AddPseudoOp( "!HEX", Types.MacroInfo.PseudoOpType.HEX );
          AddPseudoOp( "!H", Types.MacroInfo.PseudoOpType.HEX );
          AddPseudoOp( "!SL", Types.MacroInfo.PseudoOpType.LABEL_FILE );
          AddPseudoOp( "!CPU", Types.MacroInfo.PseudoOpType.CPU );
          AddPseudoOp( "!SET", Types.MacroInfo.PseudoOpType.SET );
          AddPseudoOp( "!AL", Types.MacroInfo.PseudoOpType.ASSUME_16BIT_ACCUMULATOR_65816 );
          AddPseudoOp( "!AS", Types.MacroInfo.PseudoOpType.ASSUME_8BIT_ACCUMULATOR_65816 );
          AddPseudoOp( "!RL", Types.MacroInfo.PseudoOpType.ASSUME_16BIT_REGISTERS_65816 );
          AddPseudoOp( "!RS", Types.MacroInfo.PseudoOpType.ASSUME_8BIT_REGISTERS_65816 );

          // helper pseudo ops from ACME to generate some address vs. value warnings
          //AddMacro( "!ADDR", Types.MacroInfo.MacroType.IGNORE );
          //AddMacro( "!ADDRESS", Types.MacroInfo.MacroType.IGNORE );

          POPrefix = "!";
          MacroFunctionCallPrefix.Add( "+" );
          MacrosCanBeOverloaded = true;
          GlobalLabelsAutoZone = false;
          DefineSeparatorKeywords.Add( "=" );
          PlainAssignmentOperators.Add( "=" );
          IncludeExpectsStringLiteral = true;
          StatementSeparatorChars.Add( ':' );
          GreaterOrLessThanAtBeginningAffectFullExpression = true;
          break;
        case Types.AssemblerType.DASM:
          AllowedTokenStartChars[Types.TokenInfo.TokenType.LABEL_GLOBAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzÄÖÜäöü_";
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

          AllowedTokenStartChars[Types.TokenInfo.TokenType.PSEUDO_OP] = "!";
          AllowedTokenChars[Types.TokenInfo.TokenType.PSEUDO_OP] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.CALL_MACRO] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_äöüÄÖÜß";
          AllowedTokenChars[Types.TokenInfo.TokenType.CALL_MACRO] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_äöüÄÖÜß.";

          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_INTERNAL] = "+-";

          AllowedSingleTokens = ",#*" + OpenBracketChars + CloseBracketChars + "\\{}[]";

          AddPseudoOp( "DC.B", Types.MacroInfo.PseudoOpType.TEXT );
          AddPseudoOp( "DC.W", Types.MacroInfo.PseudoOpType.WORD );
          AddPseudoOp( ".WORD", Types.MacroInfo.PseudoOpType.WORD );
          AddPseudoOp( ".BYTE", Types.MacroInfo.PseudoOpType.BYTE );
          AddPseudoOp( "MACRO", Types.MacroInfo.PseudoOpType.MACRO );
          AddPseudoOp( "MAC", Types.MacroInfo.PseudoOpType.MACRO );
          AddPseudoOp( "ENDM", Types.MacroInfo.PseudoOpType.END );
          AddPseudoOp( "RORG", Types.MacroInfo.PseudoOpType.PSEUDO_PC );
          AddPseudoOp( "REND", Types.MacroInfo.PseudoOpType.REAL_PC );
          AddPseudoOp( "INCBIN", Types.MacroInfo.PseudoOpType.INCLUDE_BINARY );
          AddPseudoOp( "INCLUDE", Types.MacroInfo.PseudoOpType.INCLUDE_SOURCE );
          AddPseudoOp( "SUBROUTINE", Types.MacroInfo.PseudoOpType.ZONE );
          AddPseudoOp( "ERR", Types.MacroInfo.PseudoOpType.ERROR );
          AddPseudoOp( "IFCONST", Types.MacroInfo.PseudoOpType.IFDEF );
          AddPseudoOp( "IFNCONST", Types.MacroInfo.PseudoOpType.IFNDEF );
          AddPseudoOp( "IF", Types.MacroInfo.PseudoOpType.IF );
          AddPseudoOp( "ELSE", Types.MacroInfo.PseudoOpType.ELSE );
          AddPseudoOp( "ENDIF", Types.MacroInfo.PseudoOpType.END_IF );
          AddPseudoOp( "DS.Z", Types.MacroInfo.PseudoOpType.FILL );
          AddPseudoOp( "DS", Types.MacroInfo.PseudoOpType.FILL );
          AddPseudoOp( "DS.B", Types.MacroInfo.PseudoOpType.FILL );
          AddPseudoOp( "ALIGN", Types.MacroInfo.PseudoOpType.ALIGN_DASM );
          AddPseudoOp( "ECHO", Types.MacroInfo.PseudoOpType.MESSAGE );

          AddPseudoOp( "REPEAT", Types.MacroInfo.PseudoOpType.LOOP_START );
          AddPseudoOp( "REPEND", Types.MacroInfo.PseudoOpType.LOOP_END );

          AddPseudoOp( "PROCESSOR", Types.MacroInfo.PseudoOpType.IGNORE );
          AddPseudoOp( "ORG", Types.MacroInfo.PseudoOpType.ORG );
          AddPseudoOp( "SEG", Types.MacroInfo.PseudoOpType.SEG );
          AddPseudoOp( "SEG.U", Types.MacroInfo.PseudoOpType.SEG_VIRTUAL );
          AddPseudoOp( "INCDIR", Types.MacroInfo.PseudoOpType.ADD_INCLUDE_SOURCE );

          RestOfLineAsSingleToken.Add( Types.MacroInfo.PseudoOpType.ADD_INCLUDE_SOURCE );

          LabelPostfix = ":";
          MacroFunctionCallPrefix.Add( ":" );
          GlobalLabelsAutoZone = false;
          DefineSeparatorKeywords.AddRange( new string[] { "SET", "EQU", "=" } );
          PlainAssignmentOperators.AddRange( new string[] { "SET", "EQU", "=" } );
          MacroIsZone = true;
          MacrosHaveVariableNumberOfArguments = true;
          IncludeExpectsStringLiteral = false;
          IncludeHasOnlyFilename = true;
          IncludeSourceIsAlwaysUsingLibraryPathAndFile = true;
          CaseSensitive = false;
          LoopEndHasNoScope = true;
          MessageAutoIncludesBlanksBetweenParameters = true;
          DefaultTargetType       = Types.CompileTargetType.PLAIN;
          DefaultTargetExtension  = ".bin";
          LabelsMustBeAtStartOfLine = true;

          OpcodeSizeIdentifierSeparator = ".";
          OpcodeSizeIdentifierOneByteOperands = new List<string> () { "b", "d", "z" };
          OpcodeSizeIdentifierTwoByteOperands = new List<string>() { "w", "wx", "wy" };
          break;
        case Types.AssemblerType.TASM:
          // 64tass, TASM
          AllowedTokenStartChars[Types.TokenInfo.TokenType.LABEL_GLOBAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzÄÖÜäöü_";
          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_GLOBAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_äöüÄÖÜß.";
          AllowedTokenEndChars[Types.TokenInfo.TokenType.LABEL_GLOBAL] = ":";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.LABEL_LOCAL] = "_";
          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_LOCAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_äöüÄÖÜß.";
          AllowedTokenEndChars[Types.TokenInfo.TokenType.LABEL_LOCAL] = ":";

          OpenBracketChars = "(" + INTERNAL_OPENING_BRACE;
          CloseBracketChars = ")" + INTERNAL_CLOSING_BRACE;

          AllowedTokenStartChars[Types.TokenInfo.TokenType.LITERAL_CHAR] = "'";
          AllowedTokenEndChars[Types.TokenInfo.TokenType.LITERAL_CHAR] = "'";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.LITERAL_STRING] = "\"";
          AllowedTokenEndChars[Types.TokenInfo.TokenType.LITERAL_STRING] = "\"";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.LITERAL_NUMBER] = "0123456789abcdefABCDEF$%";
          AllowedTokenChars[Types.TokenInfo.TokenType.LITERAL_NUMBER] = "0123456789abcdefABCDEFx";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.COMMENT] = ";";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.PSEUDO_OP] = ".";
          AllowedTokenChars[Types.TokenInfo.TokenType.PSEUDO_OP] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.CALL_MACRO] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_äöüÄÖÜß";
          AllowedTokenChars[Types.TokenInfo.TokenType.CALL_MACRO] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_äöüÄÖÜß.";

          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_INTERNAL] = "+-";

          AllowedSingleTokens = ",#*" + OpenBracketChars + CloseBracketChars + "\\{}[]";

          AddPseudoOp( ".WORD", Types.MacroInfo.PseudoOpType.WORD );
          AddPseudoOp( ".ADDR", Types.MacroInfo.PseudoOpType.WORD );
          AddPseudoOp( ".BYTE", Types.MacroInfo.PseudoOpType.BYTE );
          AddPseudoOp( ".LOGICAL", Types.MacroInfo.PseudoOpType.PSEUDO_PC );
          AddPseudoOp( ".HERE", Types.MacroInfo.PseudoOpType.REAL_PC );
          AddPseudoOp( ".ALIGN", Types.MacroInfo.PseudoOpType.ALIGN_DASM );
          AddPseudoOp( ".MACRO", Types.MacroInfo.PseudoOpType.MACRO );
          AddPseudoOp( ".ENDM", Types.MacroInfo.PseudoOpType.END );
          AddPseudoOp( ".O", Types.MacroInfo.PseudoOpType.END );
          AddPseudoOp( ".INCLUDE", Types.MacroInfo.PseudoOpType.INCLUDE_SOURCE );
          AddPseudoOp( ".TEXT", Types.MacroInfo.PseudoOpType.TEXT );
          AddPseudoOp( ".FILL", Types.MacroInfo.PseudoOpType.FILL );
          AddPseudoOp( ".ENC", Types.MacroInfo.PseudoOpType.CONVERSION_TAB_TASS );
          AddPseudoOp( ".CDEF", Types.MacroInfo.PseudoOpType.CONVERSION_TAB_TASS_ENTRY );
          AddPseudoOp( ".AL", Types.MacroInfo.PseudoOpType.ASSUME_16BIT_ACCUMULATOR_65816);
          AddPseudoOp( ".AS", Types.MacroInfo.PseudoOpType.ASSUME_8BIT_ACCUMULATOR_65816);
          AddPseudoOp( ".XL", Types.MacroInfo.PseudoOpType.ASSUME_16BIT_REGISTERS_65816);
          AddPseudoOp( ".XS", Types.MacroInfo.PseudoOpType.ASSUME_8BIT_REGISTERS_65816);
          AddPseudoOp( ".TARGET", Types.MacroInfo.PseudoOpType.CPU );
          AddPseudoOp( ".IF", Types.MacroInfo.PseudoOpType.IF );
          AddPseudoOp( ".FI", Types.MacroInfo.PseudoOpType.END_IF );
          AddPseudoOp( ".ELSE", Types.MacroInfo.PseudoOpType.ELSE );

          RestOfLineAsSingleToken.Add( Types.MacroInfo.PseudoOpType.ADD_INCLUDE_SOURCE );
          POPrefix = ".";
          LabelPostfix = ":";
          MacroFunctionCallPrefix.Add( "#" );
          MacroFunctionCallPrefix.Add( "." );
          GlobalLabelsAutoZone = true;
          DefineSeparatorKeywords.Add( ".VAR" );
          DefineSeparatorKeywords.Add( "=" );
          PlainAssignmentOperators.AddRange( new string[] { ".VAR", "=" } );
          MacroIsZone = true;
          MacrosHaveVariableNumberOfArguments = true;
          CaseSensitive = false;
          LoopEndHasNoScope = true;
          DefaultTargetType = Types.CompileTargetType.PLAIN;
          DefaultTargetExtension = ".bin";
          GreaterOrLessThanAtBeginningAffectFullExpression  = true;
          AllowsCustomTextMappings                          = true;
          IfWithoutBrackets                                 = true;
          LocalLabelStacking                                = true;
          break;
        case Types.AssemblerType.PDS:
          AllowedTokenStartChars[Types.TokenInfo.TokenType.LABEL_GLOBAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzÄÖÜäöü";
          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_GLOBAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_äöüÄÖÜß";
          AllowedTokenEndChars[Types.TokenInfo.TokenType.LABEL_GLOBAL] = "#";

          // besides proper local labels we also misuse cheap labels as macro parameters
          AllowedTokenStartChars[Types.TokenInfo.TokenType.LABEL_CHEAP_LOCAL] = "@";
          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_CHEAP_LOCAL] = "0123456789";

          OpenBracketChars = "([" + INTERNAL_OPENING_BRACE;
          CloseBracketChars = ")]" + INTERNAL_CLOSING_BRACE;

          AllowedTokenStartChars[Types.TokenInfo.TokenType.LABEL_LOCAL] = "!:";
          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_LOCAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_äöüÄÖÜß!";

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

          AddPseudoOp( "DC.B", Types.MacroInfo.PseudoOpType.TEXT );
          AddPseudoOp( "DC.V", Types.MacroInfo.PseudoOpType.TEXT );
          AddPseudoOp( "DB", Types.MacroInfo.PseudoOpType.TEXT );
          AddPseudoOp( "DFB", Types.MacroInfo.PseudoOpType.TEXT );
          AddPseudoOp( "DEFB", Types.MacroInfo.PseudoOpType.TEXT );
          AddPseudoOp( "BYTE", Types.MacroInfo.PseudoOpType.TEXT );
          AddPseudoOp( "EQUB", Types.MacroInfo.PseudoOpType.TEXT );
          AddPseudoOp( ".BYTE", Types.MacroInfo.PseudoOpType.TEXT );
          AddPseudoOp( ".ASCII", Types.MacroInfo.PseudoOpType.TEXT );
          AddPseudoOp( ".TEXT", Types.MacroInfo.PseudoOpType.TEXT );
          AddPseudoOp( "ASC", Types.MacroInfo.PseudoOpType.TEXT );
          AddPseudoOp( "STR", Types.MacroInfo.PseudoOpType.TEXT );
          AddPseudoOp( "DEFM", Types.MacroInfo.PseudoOpType.TEXT );
          AddPseudoOp( "DM", Types.MacroInfo.PseudoOpType.TEXT );
          AddPseudoOp( "DFM", Types.MacroInfo.PseudoOpType.TEXT );
          AddPseudoOp( "DATA", Types.MacroInfo.PseudoOpType.TEXT );

          AddPseudoOp( "DH", Types.MacroInfo.PseudoOpType.HIGH_BYTE );
          AddPseudoOp( "DL", Types.MacroInfo.PseudoOpType.LOW_BYTE );
          AddPseudoOp( "DW", Types.MacroInfo.PseudoOpType.WORD );
          AddPseudoOp( "HEX", Types.MacroInfo.PseudoOpType.HEX );
          AddPseudoOp( "CBM", Types.MacroInfo.PseudoOpType.TEXT_SCREEN );
          AddPseudoOp( "INCBIN", Types.MacroInfo.PseudoOpType.INCLUDE_BINARY );
          AddPseudoOp( "INCLUDE", Types.MacroInfo.PseudoOpType.INCLUDE_SOURCE );
          AddPseudoOp( "ERROR", Types.MacroInfo.PseudoOpType.ERROR );
          AddPseudoOp( "IF", Types.MacroInfo.PseudoOpType.IF );
          AddPseudoOp( "ENDIF", Types.MacroInfo.PseudoOpType.END_IF );
          AddPseudoOp( "FI", Types.MacroInfo.PseudoOpType.END_IF );
          AddPseudoOp( "ELSE", Types.MacroInfo.PseudoOpType.ELSE );
          AddPseudoOp( "DS", Types.MacroInfo.PseudoOpType.FILL );
          AddPseudoOp( "DO", Types.MacroInfo.PseudoOpType.LOOP_START );
          AddPseudoOp( "LOOP", Types.MacroInfo.PseudoOpType.LOOP_END );
          AddPseudoOp( "UNTIL", Types.MacroInfo.PseudoOpType.LOOP_END );
          AddPseudoOp( "MACRO", Types.MacroInfo.PseudoOpType.MACRO );
          AddPseudoOp( "ENDM", Types.MacroInfo.PseudoOpType.END );

          AddPseudoOp( "PROCESSOR", Types.MacroInfo.PseudoOpType.IGNORE );
          AddPseudoOp( "ORG", Types.MacroInfo.PseudoOpType.ORG );
          AddPseudoOp( "FREE", Types.MacroInfo.PseudoOpType.IGNORE );
          AddPseudoOp( "SEND", Types.MacroInfo.PseudoOpType.IGNORE );
          AddPseudoOp( "SKIP", Types.MacroInfo.PseudoOpType.IGNORE );
          AddPseudoOp( "INFO", Types.MacroInfo.PseudoOpType.IGNORE );
          AddPseudoOp( "RUN", Types.MacroInfo.PseudoOpType.IGNORE );

          AddPseudoOp( "EXEC", Types.MacroInfo.PseudoOpType.IGNORE );
          AddPseudoOp( "START", Types.MacroInfo.PseudoOpType.IGNORE );

          AddPseudoOp( "DSECT", Types.MacroInfo.PseudoOpType.IGNORE );
          AddPseudoOp( "DEND", Types.MacroInfo.PseudoOpType.IGNORE );

          AddPseudoOp( "MSW", Types.MacroInfo.PseudoOpType.IGNORE );

          AddPseudoOp( "END", Types.MacroInfo.PseudoOpType.END_OF_FILE );
          AddPseudoOp( "REPEAT", Types.MacroInfo.PseudoOpType.REPEAT );

          OperatorPrecedence.Remove( "!=" );
          OperatorPrecedence["!"] = 4;

          LabelPostfix = ":";
          GlobalLabelsAutoZone = true;
          DefineSeparatorKeywords.Add( "EQU" );
          DefineSeparatorKeywords.Add( "=" );
          PlainAssignmentOperators.AddRange( new string[] { "EQU", "=" } );
          CaseSensitive = false;
          IncludeExpectsStringLiteral = false;
          IncludeHasOnlyFilename = true;
          //StatementSeparatorChars.Add( ':' );
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

          AllowedTokenStartChars[Types.TokenInfo.TokenType.PSEUDO_OP] = ".";
          AllowedTokenChars[Types.TokenInfo.TokenType.PSEUDO_OP] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_INTERNAL] = "+-";

          AllowedSingleTokens = ",#{}*" + OpenBracketChars + CloseBracketChars;

          POPrefix = ".";

          AddPseudoOp( ".BYTE", Types.MacroInfo.PseudoOpType.BYTE );
          AddPseudoOp( ".WORD", Types.MacroInfo.PseudoOpType.WORD );

          GlobalLabelsAutoZone = true;
          DefineSeparatorKeywords.Add( "=" );
          PlainAssignmentOperators.AddRange( new string[] { "=" } );
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

          AllowedTokenStartChars[Types.TokenInfo.TokenType.PSEUDO_OP] = "!";
          AllowedTokenChars[Types.TokenInfo.TokenType.PSEUDO_OP] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.CALL_MACRO] = ":";
          AllowedTokenChars[Types.TokenInfo.TokenType.CALL_MACRO] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_äöüÄÖÜß.";

          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_INTERNAL] = "+-";

          AllowedSingleTokens = ",#*" + OpenBracketChars + CloseBracketChars;

          AddPseudoOp( "DC.B", Types.MacroInfo.PseudoOpType.TEXT );
          AddPseudoOp( "DC.W", Types.MacroInfo.PseudoOpType.WORD );
          AddPseudoOp( "MAC", Types.MacroInfo.PseudoOpType.MACRO );
          AddPseudoOp( "ENDM", Types.MacroInfo.PseudoOpType.END );
          AddPseudoOp( "!TEXT", Types.MacroInfo.PseudoOpType.TEXT );
          AddPseudoOp( "!TX", Types.MacroInfo.PseudoOpType.TEXT );
          AddPseudoOp( "!SCR", Types.MacroInfo.PseudoOpType.TEXT_SCREEN );
          AddPseudoOp( "RORG", Types.MacroInfo.PseudoOpType.PSEUDO_PC );
          AddPseudoOp( "REND", Types.MacroInfo.PseudoOpType.REAL_PC );
          AddPseudoOp( "!BANK", Types.MacroInfo.PseudoOpType.BANK );
          AddPseudoOp( "!CONVTAB", Types.MacroInfo.PseudoOpType.CONVERSION_TAB );
          AddPseudoOp( "!CT", Types.MacroInfo.PseudoOpType.CONVERSION_TAB );
          AddPseudoOp( "INCBIN", Types.MacroInfo.PseudoOpType.INCLUDE_BINARY );
          AddPseudoOp( "INCLUDE", Types.MacroInfo.PseudoOpType.INCLUDE_SOURCE );
          AddPseudoOp( "!TO", Types.MacroInfo.PseudoOpType.COMPILE_TARGET );
          AddPseudoOp( "SUBROUTINE", Types.MacroInfo.PseudoOpType.ZONE );
          AddPseudoOp( "!ERROR", Types.MacroInfo.PseudoOpType.ERROR );
          AddPseudoOp( "!IFDEF", Types.MacroInfo.PseudoOpType.IFDEF );
          AddPseudoOp( "!IFNDEF", Types.MacroInfo.PseudoOpType.IFNDEF );
          AddPseudoOp( "IF", Types.MacroInfo.PseudoOpType.IF );
          AddPseudoOp( "ELSE", Types.MacroInfo.PseudoOpType.ELSE );
          AddPseudoOp( "ENDIF", Types.MacroInfo.PseudoOpType.END_IF );
          AddPseudoOp( "DS.Z", Types.MacroInfo.PseudoOpType.FILL );
          AddPseudoOp( "DS", Types.MacroInfo.PseudoOpType.FILL );
          AddPseudoOp( "DS.B", Types.MacroInfo.PseudoOpType.FILL );
          AddPseudoOp( "ALIGN", Types.MacroInfo.PseudoOpType.ALIGN_DASM );
          AddPseudoOp( "!ENDOFFILE", Types.MacroInfo.PseudoOpType.END_OF_FILE );

          AddPseudoOp( "REPEAT", Types.MacroInfo.PseudoOpType.LOOP_START );
          AddPseudoOp( "REPEND", Types.MacroInfo.PseudoOpType.LOOP_END );

          AddPseudoOp( "PROCESSOR", Types.MacroInfo.PseudoOpType.IGNORE );
          AddPseudoOp( "ORG", Types.MacroInfo.PseudoOpType.ORG );
          AddPseudoOp( "SEG", Types.MacroInfo.PseudoOpType.SEG );
          AddPseudoOp( "SEG.U", Types.MacroInfo.PseudoOpType.SEG );
          AddPseudoOp( "BYTE", Types.MacroInfo.PseudoOpType.BYTE );

          LabelPostfix = ":";
          MacroFunctionCallPrefix.Add( ":" );
          GlobalLabelsAutoZone = false;
          DefineSeparatorKeywords.Add( "SET" );
          DefineSeparatorKeywords.Add( "=" );
          PlainAssignmentOperators.AddRange( new string[] { "SET", "=" } );
          MacroIsZone = true;
          MacrosHaveVariableNumberOfArguments = true;
          IncludeExpectsStringLiteral = false;
          break;
        case Types.AssemblerType.KICKASSEMBLER:
          AllowedTokenStartChars[Types.TokenInfo.TokenType.LABEL_GLOBAL] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyzÄÖÜäöü_";
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

          AllowedTokenStartChars[Types.TokenInfo.TokenType.PSEUDO_OP] = ".";
          AllowedTokenChars[Types.TokenInfo.TokenType.PSEUDO_OP] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

          AllowedTokenStartChars[Types.TokenInfo.TokenType.CALL_MACRO] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_äöüÄÖÜß";
          AllowedTokenChars[Types.TokenInfo.TokenType.CALL_MACRO] = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_äöüÄÖÜß.";

          AllowedTokenChars[Types.TokenInfo.TokenType.LABEL_INTERNAL] = "+-";

          AllowedSingleTokens = ",#*" + OpenBracketChars + CloseBracketChars + "\\{}[]";

          AddPseudoOp( ".WORD", Types.MacroInfo.PseudoOpType.WORD );
          AddPseudoOp( ".BYTE", Types.MacroInfo.PseudoOpType.BYTE );
          AddPseudoOp( ".BY", Types.MacroInfo.PseudoOpType.BYTE );
          AddPseudoOp( ".BREAK", Types.MacroInfo.PseudoOpType.BREAK_POINT );

          LabelPostfix = ":";
          MacroFunctionCallPrefix.Add( ":" );
          GlobalLabelsAutoZone = false;
          DefineSeparatorKeywords.Add( "=" );
          PlainAssignmentOperators.AddRange( new string[] { "=" } );
          MacroIsZone = true;
          MacrosHaveVariableNumberOfArguments = true;
          IncludeExpectsStringLiteral = false;
          IncludeHasOnlyFilename = true;
          IncludeSourceIsAlwaysUsingLibraryPathAndFile = true;
          CaseSensitive = false;
          LoopEndHasNoScope = true;
          MessageAutoIncludesBlanksBetweenParameters = true;
          DefaultTargetType = Types.CompileTargetType.PLAIN;
          DefaultTargetExtension = ".bin";
          LabelsMustBeAtStartOfLine = true;
          break;
      }
    }

  };
}
