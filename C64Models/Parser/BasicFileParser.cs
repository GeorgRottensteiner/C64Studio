using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using C64Models.BASIC;
using GR.Memory;
using RetroDevStudio;

namespace C64Studio.Parser
{
  public class BasicFileParser : ParserBase
  {
    public enum BasicVersion
    {
      [Description( "BASIC V2" )]
      C64_BASIC_V2 = 0, //  Anpassung des VIC BASIC V2 für VC10, C64, C128 (C64-Modus), SX64, PET 64.
      V1_0,             // Version 1.0	Noch recht fehlerbehaftet, erste Versionen im PET 2001
      V2_0,             // August  Version 2.0	Fehlerkorrekturen, eingebaut in weitere Versionen des PET
      V3_0,             // Version 3.0	Leichte Fehlerkorrekturen und Integration des Maschinensprachemonitors TIM, schnelle Garbage Collection. CBM 3000 Serie (Standard) und PET 2001 (angezeigte Versionsnummer dort ist allerdings noch 2).
      V4_0,             // Version 4.0	Neue Befehle für leichtere Handhabung für Diskettenlaufwerke für CBM 4000. Auch durch ROM-Austausch für CBM 3000 Serie und PET 2001 verfügbar.
      V4_1,             // Version 4.1	Fehlerkorrigierte Fassung der Version 4.0 mit erweiterter Bildschirmeditor für CBM 8000. Auch durch ROM-Austausch für CBM 3000/4000 Serie verfügbar.
      V4_2,             // Version 4.2	Geänderte und ergänzte Befehle für den CBM 8096.
      VIC_BASIC_V2,     // Funktionsumfang von V 2.0 mit Korrekturen aus der Version-4-Linie. Einsatz im VC20.
      V4_PLUS,          // (intern bis 4.75)	Neue Befehle und RAM-Unterstützung bis 256 KByte für CBM-II-Serie (CBM 500, 6x0, 7x0). Fortsetzung und Ende der Version-4-Linie.
      [Description( "BASIC V3.5" )]
      V3_5,             // Version 3.5	Neue Befehle für die Heimcomputer C16/116 und Plus/4. Zusammenführung aus C64 BASIC V2 und Version-4-Linie.
      V3_6,             // Version 3.6	Neue Befehle für LCD-Prototypen.
      [Description( "BASIC V7.0" )]
      V7_0,             // Version 7.0	Neue Befehle für den C128/D/DCR. Weiterentwicklung des C16/116 BASIC 3.5 .
      V10_0,            // Version 10 Neue Befehle für C65, beinhaltet sehr viele Fehler, kam aus dem Entwicklungsstadium nicht heraus. Weiterentwicklung des BASIC 7.0.
      [Description( "BASIC Lightning" )]
      BASIC_LIGHTNING,  // BASIC extension
      [Description( "Laser BASIC" )]
      LASER_BASIC,      // BASIC extension
      [Description( "Simon's BASIC" )]
      SIMONS_BASIC,     // Simons Basic
      SIMONS_BASIC_2    // Simons Basic Extended (SBX)
    }

    public class ParserSettings
    {
      public bool         StripSpaces = true;
      public bool         StripREM = false;
      public bool         UpperCaseMode = true;
      public bool         UseC64Font = true;
      public Dialect      BASICDialect = null;
    };

    public enum TokenValue
    {
      BLACK = 0,
      WHITE = 1,
      RED,
      CYAN,
      PURPLE,
      GREEN,
      BLUE,
      YELLOW,
      ORANGE,
      BROWN,
      LIGHT_RED,
      DARK_GREY,
      GREY,
      LIGHT_GREEN,
      LIGHT_BLUE,
      LIGHT_GREY,
      REVERSE_ON,
      REVERSE_OFF,
      CURSOR_DOWN,
      CURSOR_UP,
      CURSOR_LEFT,
      CURSOR_RIGHT,
      DELETE,
      INSERT,
      CLEAR,
      HOME,
      F1,
      F3,
      F5,
      F7,
      F2,
      F4,
      F6,
      F8,
      INDIRECT_KEY
    };

    public enum RenumberResult
    {
      OK = 0,
      TOO_MANY_LINES,
      NOTHING_TO_DO,
      INVALID_VALUES
    };

    public class ActionToken
    {
      public string Replacement = "";
      public int ByteValue = 0;
      public TokenValue TokenValue = TokenValue.BLACK;
    };

    public class Token
    {
      public enum Type
      {
        LINE_NUMBER,
        BASIC_TOKEN,
        STRING_LITERAL,
        NUMERIC_LITERAL,
        DIRECT_TOKEN,
        EX_BASIC_TOKEN,
        MACRO,
        COMMENT,
        VARIABLE,
        HARD_COMMENT
      };

      public Type       TokenType = Type.DIRECT_TOKEN;
      public string     Content = "";
      public int        ByteValue = 0;
      public int        StartIndex = 0;
    };

    internal class LineInfo
    {
      public int                      LineIndex = 0;
      public int                      LineNumber = 0;
      public GR.Memory.ByteBuffer     LineData = new GR.Memory.ByteBuffer();
      public GR.Collections.Set<int>  ReferencedLineNumbers = new GR.Collections.Set<int>();
      public string                   Line = "";
      public System.Collections.Generic.List<Token>   Tokens = new List<Token>();
    };

    private GR.Collections.Map<int, LineInfo> m_LineInfos = new GR.Collections.Map<int, LineInfo>();

    public GR.Collections.Map<string, ActionToken>     ActionTokens = new GR.Collections.Map<string, ActionToken>();
    public GR.Collections.Map<TokenValue, ActionToken> ActionTokenByValue = new GR.Collections.Map<TokenValue, ActionToken>();
    public GR.Collections.Map<byte, ActionToken>       ActionTokenByByteValue = new GR.Collections.Map<byte,ActionToken>();
    public ParserSettings       Settings = new ParserSettings();

    public Types.ASM.FileInfo         ASMFileInfo = new C64Studio.Types.ASM.FileInfo();
    public Types.ASM.FileInfo         InitialFileInfo = null;

    public GR.Collections.Map<Token.Type, string>     AllowedTokenStartChars = new GR.Collections.Map<Token.Type, string>();
    public GR.Collections.Map<Token.Type, string>     AllowedTokenChars = new GR.Collections.Map<Token.Type, string>();
    public GR.Collections.Map<Token.Type, string>     AllowedTokenEndChars = new GR.Collections.Map<Token.Type, string>();
    public string                                     AllowedSingleTokens;

    private CompileConfig                             _Config = null;



    public BasicFileParser( ParserSettings Settings )
    {
      this.Settings = Settings;
      Setup();
    }



    public BasicFileParser( ParserSettings Settings, string Filename )
    {
      this.Settings = Settings;
      m_Filename = Filename;
      Setup();
    }



    private void Setup()
    {
      LabelMode = false;

      AllowedTokenStartChars[Token.Type.HARD_COMMENT] = "#";

      AllowedTokenStartChars[Token.Type.VARIABLE] = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
      AllowedTokenChars[Token.Type.VARIABLE] = "ABCDEFGHIJKLMNOPQRSTUVWXYZ01234567890";
      AllowedTokenEndChars[Token.Type.VARIABLE] = "%$";

      AllowedTokenStartChars[Token.Type.NUMERIC_LITERAL] = "0123456789.";
      AllowedTokenChars[Token.Type.NUMERIC_LITERAL] = "0123456789.";
      AllowedTokenEndChars[Token.Type.NUMERIC_LITERAL] = "";

      AllowedTokenChars[Token.Type.DIRECT_TOKEN] = "()+-,;:<>=!?'&/^{}";
      AllowedTokenEndChars[Token.Type.DIRECT_TOKEN] = "()+-,;:<>=!?'&/^{}";

      AllowedTokenStartChars[Token.Type.BASIC_TOKEN] = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

      AllowedSingleTokens = "()+-,;:<>=!?'&/^{}*";

      SetBasicDialect( Settings.BASICDialect );
    }



    public bool LabelMode
    {
      get;
      set;
    }



    private void AddActionToken( TokenValue Value, string Token, byte ByteValue )
    {
      ActionToken token = new ActionToken();
      token.Replacement = Token;
      token.ByteValue = ByteValue;
      token.TokenValue = Value;

      ActionTokens[Token.ToUpper()] = token;
      ActionTokenByByteValue[ByteValue] = token;
      ActionTokenByValue[Value] = token;

      /*
      if ( PETSCII.ContainsKey( (char)ByteValue ) )
      {
        Debug.Log( "!!AddActionToken overrides entry for " + ByteValue );
      }
      PETSCII[(char)ByteValue] = ByteValue;*/
    }



    public void SetBasicDialect( Dialect Dialect )
    {
      Settings.BASICDialect = Dialect;
      ActionTokens.Clear();
      ActionTokenByByteValue.Clear();
      ActionTokenByValue.Clear();

      AddActionToken( TokenValue.INDIRECT_KEY, "{CBM-A}", 0xb0 );
      AddActionToken( TokenValue.INDIRECT_KEY, "{CBM-B}", 0xbf );
      AddActionToken( TokenValue.INDIRECT_KEY, "{CBM-C}", 0xbc );
      AddActionToken( TokenValue.INDIRECT_KEY, "{CBM-D}", 0xac );
      AddActionToken( TokenValue.INDIRECT_KEY, "{CBM-E}", 0xb1 );
      AddActionToken( TokenValue.INDIRECT_KEY, "{CBM-F}", 0xbb );
      AddActionToken( TokenValue.INDIRECT_KEY, "{CBM-G}", 0xa5 );
      AddActionToken( TokenValue.INDIRECT_KEY, "{CBM-H}", 0xb4 );
      AddActionToken( TokenValue.INDIRECT_KEY, "{CBM-I}", 0xa2 );
      AddActionToken( TokenValue.INDIRECT_KEY, "{CBM-J}", 0xb5 );
      AddActionToken( TokenValue.INDIRECT_KEY, "{CBM-K}", 0xa1 );
      AddActionToken( TokenValue.INDIRECT_KEY, "{CBM-L}", 0xb6 );
      AddActionToken( TokenValue.INDIRECT_KEY, "{CBM-M}", 0xaa );
      AddActionToken( TokenValue.INDIRECT_KEY, "{CBM-N}", 0xa7 );
      AddActionToken( TokenValue.INDIRECT_KEY, "{CBM-O}", 0xb9 );
      AddActionToken( TokenValue.INDIRECT_KEY, "{CBM-P}", 0xaf );
      AddActionToken( TokenValue.INDIRECT_KEY, "{CBM-Q}", 0xab );
      AddActionToken( TokenValue.INDIRECT_KEY, "{CBM-R}", 0xb2 );
      AddActionToken( TokenValue.INDIRECT_KEY, "{CBM-S}", 0xae );
      AddActionToken( TokenValue.INDIRECT_KEY, "{CBM-T}", 0xa3 );
      AddActionToken( TokenValue.INDIRECT_KEY, "{CBM-U}", 0xb8 );
      AddActionToken( TokenValue.INDIRECT_KEY, "{CBM-V}", 0xbe );
      AddActionToken( TokenValue.INDIRECT_KEY, "{CBM-W}", 0xb3 );
      AddActionToken( TokenValue.INDIRECT_KEY, "{CBM-X}", 0xbd );
      AddActionToken( TokenValue.INDIRECT_KEY, "{CBM-Z}", 0xad );
      AddActionToken( TokenValue.INDIRECT_KEY, "{CBM-Y}", 0xb7 );
      AddActionToken( TokenValue.INDIRECT_KEY, "{CBM-+}", 0xa6 );
      AddActionToken( TokenValue.INDIRECT_KEY, "{CBM-@}", 0xa4 );
      AddActionToken( TokenValue.INDIRECT_KEY, "{CBM--}", 0x7c );

      AddActionToken( TokenValue.INDIRECT_KEY, "{SHIFT-A}", 0x61 );
      AddActionToken( TokenValue.INDIRECT_KEY, "{SHIFT-B}", 0x62 );
      AddActionToken( TokenValue.INDIRECT_KEY, "{SHIFT-C}", 0x63 );
      AddActionToken( TokenValue.INDIRECT_KEY, "{SHIFT-D}", 0x64 );
      AddActionToken( TokenValue.INDIRECT_KEY, "{SHIFT-E}", 0x65 );
      AddActionToken( TokenValue.INDIRECT_KEY, "{SHIFT-F}", 0x66 );
      AddActionToken( TokenValue.INDIRECT_KEY, "{SHIFT-G}", 0x67 );
      AddActionToken( TokenValue.INDIRECT_KEY, "{SHIFT-H}", 0x68 );
      AddActionToken( TokenValue.INDIRECT_KEY, "{SHIFT-I}", 0x69 );
      AddActionToken( TokenValue.INDIRECT_KEY, "{SHIFT-J}", 0x6a );
      AddActionToken( TokenValue.INDIRECT_KEY, "{SHIFT-K}", 0x6b );
      AddActionToken( TokenValue.INDIRECT_KEY, "{SHIFT-L}", 0x6c );
      AddActionToken( TokenValue.INDIRECT_KEY, "{SHIFT-M}", 0x6d );
      AddActionToken( TokenValue.INDIRECT_KEY, "{SHIFT-N}", 0x6e );
      AddActionToken( TokenValue.INDIRECT_KEY, "{SHIFT-O}", 0x6f );
      AddActionToken( TokenValue.INDIRECT_KEY, "{SHIFT-P}", 0x70 );
      AddActionToken( TokenValue.INDIRECT_KEY, "{SHIFT-Q}", 0x71 );
      AddActionToken( TokenValue.INDIRECT_KEY, "{SHIFT-R}", 0x72 );
      AddActionToken( TokenValue.INDIRECT_KEY, "{SHIFT-S}", 0x73 );
      AddActionToken( TokenValue.INDIRECT_KEY, "{SHIFT-T}", 0x74 );
      AddActionToken( TokenValue.INDIRECT_KEY, "{SHIFT-U}", 0x75 );
      AddActionToken( TokenValue.INDIRECT_KEY, "{SHIFT-V}", 0x76 );
      AddActionToken( TokenValue.INDIRECT_KEY, "{SHIFT-W}", 0x77 );
      AddActionToken( TokenValue.INDIRECT_KEY, "{SHIFT-X}", 0x78 );
      AddActionToken( TokenValue.INDIRECT_KEY, "{SHIFT-Y}", 0x79 );
      AddActionToken( TokenValue.INDIRECT_KEY, "{SHIFT-Z}", 0x7a );
      AddActionToken( TokenValue.INDIRECT_KEY, "{SHIFT-ARROWUP}", 0xff );   // PI
      //AddActionToken( TokenValue.INDIRECT_KEY, "{SHIFT-ARROWLEFT}", 0x5f );
      AddActionToken( TokenValue.INDIRECT_KEY, "{SHIFT-+}", 0x7b );
      AddActionToken( TokenValue.INDIRECT_KEY, "{SHIFT-*}", 0x60 );
      AddActionToken( TokenValue.INDIRECT_KEY, "{SHIFT--}", 0x7d );

      AddActionToken( TokenValue.INDIRECT_KEY, "{SHIFT-£}", 169 );
      AddActionToken( TokenValue.INDIRECT_KEY, "{CBM-£}", 168 );
      AddActionToken( TokenValue.INDIRECT_KEY, "{CBM-*}", 127 );

      AddActionToken( TokenValue.BLACK, "{black}", 144 );
      AddActionToken( TokenValue.BLACK, "{blk}", 144 );
      AddActionToken( TokenValue.WHITE, "{white}", 5 );
      AddActionToken( TokenValue.WHITE, "{wht}", 5 );
      AddActionToken( TokenValue.RED, "{red}", 28 );
      AddActionToken( TokenValue.CYAN, "{cyn}", 159 );
      AddActionToken( TokenValue.PURPLE, "{purple}", 156 );
      AddActionToken( TokenValue.PURPLE, "{pur}", 156 );
      AddActionToken( TokenValue.GREEN, "{green}", 30 );
      AddActionToken( TokenValue.GREEN, "{grn}", 30 );
      AddActionToken( TokenValue.BLUE, "{blue}", 31 );
      AddActionToken( TokenValue.BLUE, "{blu}", 31 );
      AddActionToken( TokenValue.YELLOW, "{yellow}", 158 );
      AddActionToken( TokenValue.YELLOW, "{yel}", 158 );
      AddActionToken( TokenValue.ORANGE, "{orange}", 129 );
      AddActionToken( TokenValue.ORANGE, "{orng}", 129 );
      AddActionToken( TokenValue.BROWN, "{brown}", 149 );
      AddActionToken( TokenValue.BROWN, "{brn}", 149 );
      AddActionToken( TokenValue.LIGHT_RED, "{light red}", 150 );
      AddActionToken( TokenValue.LIGHT_RED, "{lred}", 150 );
      AddActionToken( TokenValue.DARK_GREY, "{dark gray}", 151 );
      AddActionToken( TokenValue.DARK_GREY, "{dark grey}", 151 );
      AddActionToken( TokenValue.DARK_GREY, "{gry1}", 151 );
      AddActionToken( TokenValue.GREY, "{gray}", 152 );
      AddActionToken( TokenValue.GREY, "{grey}", 152 );
      AddActionToken( TokenValue.GREY, "{gry2}", 152 );
      AddActionToken( TokenValue.LIGHT_GREEN, "{light green}", 153 );
      AddActionToken( TokenValue.LIGHT_GREEN, "{lgrn}", 153 );
      AddActionToken( TokenValue.LIGHT_BLUE, "{light blue}", 154 );
      AddActionToken( TokenValue.LIGHT_BLUE, "{lblu}", 154 );
      AddActionToken( TokenValue.LIGHT_GREY, "{light gray}", 155 );
      AddActionToken( TokenValue.LIGHT_GREY, "{light grey}", 155 );
      AddActionToken( TokenValue.LIGHT_GREY, "{gry3}", 155 );
      AddActionToken( TokenValue.REVERSE_ON, "{reverse on}", 18 );
      AddActionToken( TokenValue.REVERSE_ON, "{rvson}", 18 );
      AddActionToken( TokenValue.REVERSE_ON, "{rvon}", 18 );
      AddActionToken( TokenValue.REVERSE_OFF, "{reverse off}", 146 );
      AddActionToken( TokenValue.REVERSE_OFF, "{rvsoff}", 146 );
      AddActionToken( TokenValue.REVERSE_OFF, "{rvof}", 146 );
      AddActionToken( TokenValue.CURSOR_DOWN, "{cursor down}", 17 );
      AddActionToken( TokenValue.CURSOR_DOWN, "{down}", 17 );
      AddActionToken( TokenValue.CURSOR_UP, "{cursor up}", 145 );
      AddActionToken( TokenValue.CURSOR_UP, "{up}", 145 );
      AddActionToken( TokenValue.CURSOR_LEFT, "{cursor left}", 157 );
      AddActionToken( TokenValue.CURSOR_LEFT, "{left}", 157 );
      AddActionToken( TokenValue.CURSOR_RIGHT, "{cursor right}", 29 );
      AddActionToken( TokenValue.CURSOR_RIGHT, "{rght}", 29 );
      AddActionToken( TokenValue.CURSOR_RIGHT, "{right}", 29 );
      AddActionToken( TokenValue.DELETE, "{del}", 20 );
      AddActionToken( TokenValue.INSERT, "{insert}", 148 );
      AddActionToken( TokenValue.INSERT, "{ins}", 148 );
      AddActionToken( TokenValue.CLEAR, "{clr}", 147 );
      AddActionToken( TokenValue.HOME, "{home}", 19 );
      AddActionToken( TokenValue.F1, "{F1}", 133 );
      AddActionToken( TokenValue.F3, "{F3}", 134 );
      AddActionToken( TokenValue.F5, "{F5}", 135 );
      AddActionToken( TokenValue.F7, "{F7}", 136 );
      AddActionToken( TokenValue.F2, "{F2}", 137 );
      AddActionToken( TokenValue.F4, "{F4}", 138 );
      AddActionToken( TokenValue.F6, "{F6}", 139 );
      AddActionToken( TokenValue.F8, "{F8}", 140 );
    }



    private void CleanLines( string[] Lines )
    {
      for ( int i = 0; i < Lines.Length; ++i )
      {
        string tempLine = Lines[i].Trim();
        tempLine = tempLine.Replace( '\t', ' ' );
        Lines[i] = tempLine;
      }
    }



    public override void Clear()
    {
      m_CompileTarget     = Types.CompileTargetType.PRG;
      m_CompileTargetFile = null;

      AssembledOutput = null;
      Messages.Clear();
      m_LineInfos.Clear();
      m_ErrorMessages = 0;
      m_WarningMessages = 0;
      m_Filename = "";
    }



    private void DumpSourceInfos()
    {
      /*
      // source infos
      foreach ( KeyValuePair<int,SourceInfo> pair in m_SourceInfo )
      {
        Debug.Log( "From line " + pair.Value.GlobalStartLine + " to " + ( pair.Value.GlobalStartLine + pair.Value.LineCount ) + ", local start " + pair.Value.LocalStartLine + " : " + pair.Value.Filename );
      }*/
    }



    public override bool Parse( string Content, ProjectConfig Configuration, CompileConfig Config, string AdditionalPredefines )
    {
      _Config = Config;

      if ( !Settings.UpperCaseMode )
      {
        Content = MakeUpperCase( Content, Settings.UseC64Font );
      }
      if ( Settings.BASICDialect.Opcodes.Count == 0 )
      {
        AddError( -1, Types.ErrorCode.E1401_INTERNAL_ERROR, "An unsupported BASIC version '" + Settings.BASICDialect.Name + "' was chosen" );
        return false;
      }

      string[] lines = Content.Split( '\n' );

      CleanLines( lines );

      ASMFileInfo.Labels.Clear();
      IncludePreviousSymbols();

      ProcessLines( lines, LabelMode );

      DumpSourceInfos();

      if ( m_ErrorMessages > 0 )
      {
        return false;
      }

      return true;
    }



    private void IncludePreviousSymbols()
    {
      // include previous symbols
      if ( InitialFileInfo != null )
      {
        foreach ( var entry in InitialFileInfo.Labels )
        {
          if ( ( entry.Value.Type != SymbolInfo.Types.PREPROCESSOR_CONSTANT_1 )
          &&   ( entry.Value.Type != SymbolInfo.Types.PREPROCESSOR_CONSTANT_2 )
          &&   ( entry.Value.Type != SymbolInfo.Types.PREPROCESSOR_LABEL ) )
          {
            if ( ( entry.Value.Type == SymbolInfo.Types.LABEL )
            &&   ( entry.Key.StartsWith( ASMFileParser.InternalLabelPrefix ) ) )
            {
              // do not pass on internal local labels
              continue;
            }
            var symbol = new SymbolInfo();
            symbol.AddressOrValue = entry.Value.AddressOrValue;
            symbol.DocumentFilename = entry.Value.DocumentFilename;
            symbol.LocalLineIndex = entry.Value.LocalLineIndex;
            symbol.Name = entry.Value.Name;
            symbol.Type = entry.Value.Type;
            symbol.Zone = entry.Value.Zone;
            symbol.FromDependency = true;
            symbol.Info = entry.Value.Info;
            symbol.References.Add( entry.Value.LineIndex );

            ASMFileInfo.Labels.Add( entry.Key, symbol );
          }
        }
      }
    }



    private void ParseToken( string CurToken, LineInfo Info, ref bool lastOpcodeWasReferencingLineNumber )
    {
      string token2 = CurToken.Trim().ToUpper();
      if ( Settings.BASICDialect.Opcodes.ContainsKey( token2 ) )
      {
        Opcode opCode = Settings.BASICDialect.Opcodes[token2];

        if ( opCode.InsertionValue > 255 )
        {
          // a two byte token
          Info.LineData.AppendU16NetworkOrder( (ushort)opCode.InsertionValue );
        }
        else
        {
          Info.LineData.AppendU8( (byte)opCode.InsertionValue );
        }

        Token opcodeToken = new Token();
        opcodeToken.TokenType = Token.Type.BASIC_TOKEN;
        opcodeToken.ByteValue = (byte)opCode.InsertionValue;
        opcodeToken.Content = opCode.Command;
        Info.Tokens.Add( opcodeToken );
      }
      else if ( Settings.BASICDialect.ExOpcodes.ContainsKey( token2 ) )
      {
        Opcode opCode = Settings.BASICDialect.ExOpcodes[token2];

        Info.LineData.AppendU8( (byte)opCode.InsertionValue );

        Token opcodeToken = new Token();
        opcodeToken.TokenType = Token.Type.EX_BASIC_TOKEN;
        opcodeToken.ByteValue = (byte)opCode.InsertionValue;
        opcodeToken.Content = opCode.Command;
        Info.Tokens.Add( opcodeToken );
      }
      else if ( token2.Length > 0 )
      {
        if ( lastOpcodeWasReferencingLineNumber )
        {
          int dummy = -1;
          if ( int.TryParse( token2, out dummy ) )
          {
            Info.ReferencedLineNumbers.Add( dummy );
          }
          lastOpcodeWasReferencingLineNumber = false;
        }
        //Debug.Log( "Token:" + token2 );
        Token literalToken = new Token();
        literalToken.TokenType = Token.Type.STRING_LITERAL;
        literalToken.Content = token2;
        Info.Tokens.Add( literalToken );
        for ( int i = 0; i < token2.Length; ++i )
        {
          Info.LineData.AppendU8( (byte)token2[i] );
        }
      }
    }



    private string ReplaceMacros( int LineIndex, string Line )
    {
      int bracketStartPos = Line.IndexOf( '{' );
      int bracketEndPos = -1;

      if ( bracketStartPos == -1 )
      {
        return Line;
      }
      int     lastStartPos = 0;
      string  result = "";

      while ( bracketStartPos != -1 )
      {
        if ( bracketStartPos > lastStartPos + 1 )
        {
          result += Line.Substring( lastStartPos, bracketStartPos - lastStartPos );
        }

        bracketEndPos = Line.IndexOf( '}', bracketStartPos );
        if ( bracketEndPos == -1 )
        {
          AddError( LineIndex, Types.ErrorCode.E1005_MISSING_CLOSING_BRACKET, "Missing closing bracket on macro" );
          return result;
        }
        string  macro = Line.Substring( bracketStartPos, bracketEndPos - bracketStartPos + 1 );
        int     macroCount = 1;

        macro = DetermineMacroCount( macro, out macroCount );


        bool  foundMacro = false;
        foreach ( var key in ConstantData.AllPhysicalKeyInfos )
        {
          if ( key.Replacements.Contains( macro ) )
          {
            for ( int i = 0; i < macroCount; ++i )
            {
              result += (char)key.PetSCIIValue;
            }
            foundMacro = true;
            break;
          }
        }
        if ( !foundMacro )
        {
          byte  petsciiValue = 0;
          if ( byte.TryParse( macro, out petsciiValue ) )
          {
            for ( int i = 0; i < macroCount; ++i )
            {
              result += (char)petsciiValue;
            }
          }
          else
          {
            AddError( LineIndex, Types.ErrorCode.E1301_PSEUDO_OPERATION, "Unknown macro " + macro );
          }
        }
        lastStartPos = bracketEndPos + 1;
        bracketStartPos = Line.IndexOf( '{', bracketEndPos );
      }
      if ( lastStartPos < Line.Length )
      {
        result += Line.Substring( lastStartPos );
      }
      return result;
    }



    /// <summary>
    /// looks for numbers at beginning of string, returns as separate entities (e.g. {4down} = "down" and 4}
    /// </summary>
    /// <param name="Macro"></param>
    /// <param name="MacroCount"></param>
    /// <returns></returns>
    public static string DetermineMacroCount( string Macro, out int MacroCount )
    {
      MacroCount = 1;
      if ( string.IsNullOrEmpty( Macro ) )
      {
        return Macro;
      }

      if ( !char.IsDigit( Macro[0] ) )
      {
        int     starPos = Macro.IndexOf( '*' );
        if ( starPos != -1 )
        {
          int     dummyCount = 0;
          if ( int.TryParse( Macro.Substring( starPos + 1 ), out dummyCount ) )
          {
            MacroCount = dummyCount;
            if ( ( MacroCount < 0 )
            ||   ( MacroCount > 999 ) )
            {
              MacroCount = 1;
            }
            return Macro.Substring( 0, starPos );
          }
        }
        return Macro;
      }

      int     lastDigitPos = 0;

      while ( ( lastDigitPos + 1 < Macro.Length )
      &&      ( char.IsDigit( Macro[lastDigitPos + 1] ) ) )
      {
        ++lastDigitPos;
      }

      int.TryParse( Macro.Substring( 0, lastDigitPos + 1 ), out MacroCount );

      if ( ( MacroCount < 0 )
      ||   ( MacroCount > 999 ) )
      {
        MacroCount = 1;
      }

      return Macro.Substring( lastDigitPos + 1 ).TrimStart();
    }



    private void AddDirectToken( LineInfo Info, byte TokenValue, int StartIndex )
    {
      if ( ( TokenValue >= 0x30 )
      &&   ( TokenValue <= 0x39 ) )
      {
        // numeric, maybe attach with token before?
        if ( ( Info.Tokens.Count > 0 )
        &&   ( Info.Tokens[Info.Tokens.Count - 1].TokenType == Token.Type.NUMERIC_LITERAL )
        &&   ( Info.Tokens[Info.Tokens.Count - 1].ByteValue >= 0x30 )
        &&   ( Info.Tokens[Info.Tokens.Count - 1].ByteValue <= 0x39 ) )
        {
          // attach to previous token
          Info.Tokens[Info.Tokens.Count - 1].Content += ConstantData.PetSCIIToChar[TokenValue].CharValue;
          Info.Tokens[Info.Tokens.Count - 1].TokenType = Token.Type.NUMERIC_LITERAL;

          Info.LineData.AppendU8( TokenValue );
          return;
        }
        Token numericToken = new Token();
        numericToken.TokenType = Token.Type.NUMERIC_LITERAL;
        numericToken.ByteValue = TokenValue;
        numericToken.Content = "" + ConstantData.PetSCIIToChar[TokenValue].CharValue;
        numericToken.StartIndex = StartIndex;
        Info.Tokens.Add( numericToken );

        Info.LineData.AppendU8( TokenValue );
        return;
      }
      Token basicToken = new Token();
      basicToken.TokenType = Token.Type.DIRECT_TOKEN;
      basicToken.ByteValue = TokenValue;
      basicToken.Content = "" + ConstantData.PetSCIIToChar[TokenValue].CharValue;
      basicToken.StartIndex = StartIndex;
      Info.Tokens.Add( basicToken );

      Info.LineData.AppendU8( TokenValue );
    }



    internal LineInfo PureTokenizeLine( string Line )
    {
      var  lineInfo = new LineInfo();
      lineInfo.Line = Line;

      if ( Line.Length == 0 )
      {
        return lineInfo;
      }

      if ( Line.StartsWith( "#" ) )
      {
        // hard comment
        lineInfo.Tokens.Add( new Token() { Content = Line, TokenType = Token.Type.HARD_COMMENT } );
        return lineInfo;
      }

      int       posInLine = 0;
      bool      insideDataStatement = false;
      int       tokenStartPos = 0;
      bool      insideStringLiteral = false;
      Token     currentToken = null;


      // now the real token crunching
      while ( posInLine < Line.Length )
      {
        char    nextByte = Line[posInLine];

        if ( insideStringLiteral )
        {
          if ( nextByte == '"' )
          {
            currentToken.Content = Line.Substring( tokenStartPos, posInLine - tokenStartPos + 1 );
            currentToken = null;
            insideStringLiteral = false;

            tokenStartPos = posInLine + 1;
          }
          ++posInLine;
          continue;
        }

        if ( nextByte == 34 )
        {
          if ( ( currentToken != null )
          &&   ( currentToken.TokenType != Token.Type.STRING_LITERAL ) )
          {
            // end of previous token
            currentToken.Content = Line.Substring( tokenStartPos, posInLine - tokenStartPos );
            currentToken = null;
          }

          currentToken = new Token();
          currentToken.TokenType = Token.Type.STRING_LITERAL;
          currentToken.StartIndex = posInLine;
          insideStringLiteral = true;

          tokenStartPos = posInLine;
          lineInfo.Tokens.Add( currentToken );

          ++posInLine;
          continue;
        }

        if ( AllowedTokenStartChars[Token.Type.BASIC_TOKEN].IndexOf( nextByte ) != -1 )
        {
          // BASIC tokens are always searched forward
          if ( ( !insideDataStatement )
          &&   ( IsBASICTokenStartingHere( Line, posInLine, out string Token ) ) )
          {
            if ( currentToken != null )
            {
              currentToken.Content = Line.Substring( tokenStartPos, posInLine - tokenStartPos );
              currentToken = null;
            }
            var basicToken = new Token();
            basicToken.TokenType  = BasicFileParser.Token.Type.BASIC_TOKEN;
            basicToken.StartIndex = posInLine;
            basicToken.Content    = Token;

            lineInfo.Tokens.Add( basicToken );

            posInLine += Token.Length;
            tokenStartPos = posInLine;

            if ( Settings.BASICDialect.IsComment( basicToken ) )
            {
              // the rest of the line is a comment
              var commentToken = new Token();
              commentToken.StartIndex = posInLine;
              commentToken.Content = Line.Substring( posInLine );
              commentToken.TokenType = BasicFileParser.Token.Type.COMMENT;

              lineInfo.Tokens.Add( commentToken );
              return lineInfo;
            }
            if ( Settings.BASICDialect.TokenDoesNotParseOtherTokens( basicToken ) )
            {
              insideDataStatement = true;
            }
            continue;
          }
          // potential variable
          if ( currentToken == null )
          {
            currentToken = new Token();
            currentToken.StartIndex = posInLine;
            currentToken.TokenType = BasicFileParser.Token.Type.VARIABLE;

            lineInfo.Tokens.Add( currentToken );
          }
          else
          {
            if ( AllowedTokenEndChars[currentToken.TokenType].IndexOf( nextByte ) != -1 )
            {
              // the last char of a token
              currentToken.Content = Line.Substring( tokenStartPos, posInLine - tokenStartPos + 1 );
              currentToken = null;
              ++posInLine;
              tokenStartPos = posInLine;
              continue;
            }
            if ( AllowedTokenChars[currentToken.TokenType].IndexOf( nextByte ) != -1 )
            {
              ++posInLine;
              continue;
            }
            // char is not allowed in this token!
            // the last char of a token
            currentToken.Content = Line.Substring( tokenStartPos, posInLine - tokenStartPos );
            currentToken = null;
            //++posInLine;
            tokenStartPos = posInLine;
            continue;
          }
          ++posInLine;
          continue;
        }

        if ( currentToken != null )
        {
          if ( AllowedTokenEndChars[currentToken.TokenType].IndexOf( nextByte ) != -1 )
          {
            // the last char of a token
            currentToken.Content = Line.Substring( tokenStartPos, posInLine - tokenStartPos + 1 );
            currentToken = null;
            ++posInLine;
            tokenStartPos = posInLine;
            continue;
          }
          if ( AllowedTokenChars[currentToken.TokenType].IndexOf( nextByte ) != -1 )
          {
            ++posInLine;
            continue;
          }
          // char is not allowed in this token!
          // the last char of a token
          currentToken.Content = Line.Substring( tokenStartPos, posInLine - tokenStartPos );
          currentToken = null;
          //++posInLine;
          tokenStartPos = posInLine;
          continue;
        }
        if ( currentToken == null )
        {
          if ( AllowedTokenStartChars[Token.Type.NUMERIC_LITERAL].IndexOf( nextByte ) != -1 )
          {
            currentToken = new Token();
            currentToken.TokenType = Token.Type.NUMERIC_LITERAL;
            currentToken.StartIndex = posInLine;
            lineInfo.Tokens.Add( currentToken );
            tokenStartPos = posInLine;

            ++posInLine;
            continue;
          }
          if ( AllowedSingleTokens.IndexOf( nextByte ) != -1 )
          {
            Token numericToken      = new Token();

            numericToken.TokenType  = Token.Type.DIRECT_TOKEN;
            numericToken.StartIndex = posInLine;
            numericToken.Content    = "" + nextByte;
            lineInfo.Tokens.Add( numericToken );
            tokenStartPos = posInLine;

            if ( nextByte == ':' )
            {
              insideDataStatement = false;
            }

            ++posInLine;
            tokenStartPos = posInLine;
            continue;
          }
        }
        /*
        if ( insideDataStatement )
        {
          // innerhalb von Data keine Token-Aufschlüsselung
          if ( nextByte == ':' )
          {
            insideDataStatement = false;
          }

          AddDirectToken( info, nextByte, bytePos );
          ++bytePos;
          continue;
        }
        */

        /*
        // alternative comment char
        if ( ( nextByte == 39 )
        &&   ( IsLightningOrLaserBASIC() ) )
        {
          insideREMStatement = true;
        }*/

        if ( nextByte == ' ' )
        {
          // skip white space
          ++tokenStartPos;
        }

        ++posInLine;
        continue;
      }

      if ( currentToken != null )
      {
        currentToken.Content = Line.Substring( tokenStartPos );
      }

      if ( ( lineInfo.Tokens.Count > 0 )
      &&   ( LabelMode )
      &&   ( lineInfo.Tokens[0].TokenType == Token.Type.NUMERIC_LITERAL )
      &&   ( lineInfo.Tokens[0].Content.IndexOf( '.' ) == -1 ) )
      {
        lineInfo.Tokens[0].TokenType = Token.Type.LINE_NUMBER;
      }
      
      return lineInfo;
    }



    private bool IsBASICTokenStartingHere( string Line, int PosInLine, out string Token )
    {
      // find best match
      Token = null;
      foreach ( var token in Settings.BASICDialect.Opcodes.Keys )
      {
        if ( ( PosInLine + token.Length <= Line.Length )
        &&   ( string.Compare( Line, PosInLine, token, 0, token.Length ) == 0 ) )
        {
          Token = token;
          return true;
        }
      }
      return false;
    }



    public bool IsComment( Opcode Opcode )
    {
      if ( Opcode.Command == "REM" )
      {
        return true;
      }
      if ( ( Opcode.Command == "'" )
      &&   ( ( Settings.BASICDialect.Name == "Laser BASIC" )
      ||     ( Settings.BASICDialect.Name == "BASIC Lightning" ) ) )
      {
        return true;
      }
      return false;
    }



    /// <summary>
    /// combines variable names and numeric literals from single chars
    /// </summary>
    /// <param name="info"></param>
    /// <param name="tempData"></param>
    /// <param name="tempDataStartPos"></param>
    private void ConsolidateTokens( LineInfo info, ByteBuffer tempData, int TempDataStartPos )
    {
      bool      isVariableName = false;
      bool      isNumeric = false;
      int       lastTokenStartPos = 0;
      for ( int i = 0; i < tempData.Length; ++i )
      {
        byte    nextByte = tempData.ByteAt( i );

        bool    isCharNumeric = ( ( nextByte >= '0' )
        &&                        ( nextByte <= '9' ) );
        bool    isCharAlpha = ( ( nextByte >= 'A' )
        &&                      ( nextByte <= 'Z' ) );


        if ( isVariableName )
        {
          // we already started with a variable
          if ( ( !isCharNumeric )
          &&   ( !isCharAlpha )
          &&   ( nextByte != '$' )
          &&   ( nextByte != '%' ) )
          {
            // variable name is done
            isVariableName = false;
            Token basicToken6     = new Token();
            basicToken6.TokenType = Token.Type.VARIABLE;
            for ( int j = lastTokenStartPos; j < i; ++j )
            {
              basicToken6.Content += "" + ConstantData.PetSCIIToChar[tempData.ByteAt( j )].CharValue;
            }
            basicToken6.StartIndex = TempDataStartPos + lastTokenStartPos;
            info.Tokens.Add( basicToken6 );

            lastTokenStartPos = i + 1;
          }
          else
          {
            continue;
          }
        }
        else if ( isNumeric )
        {
          if ( ( !isCharNumeric )
          &&   ( nextByte != '.' ) )
          {
            // numeric is done
            isNumeric = false;
            Token basicToken1       = new Token();
            basicToken1.TokenType = Token.Type.NUMERIC_LITERAL;
            for ( int j = lastTokenStartPos; j < i; ++j )
            {
              basicToken1.Content += "" + ConstantData.PetSCIIToChar[tempData.ByteAt( j )].CharValue;
            }
            basicToken1.StartIndex = TempDataStartPos + lastTokenStartPos;
            info.Tokens.Add( basicToken1 );

            lastTokenStartPos = i + 1;
          }
          else
          {
            continue;
          }
        }
        // variable begins?
        if ( isCharAlpha )
        {
          isVariableName = true;
        }
        else if ( isCharNumeric )
        {
          isNumeric = true;
        }
        else
        {
          Token basicToken2       = new Token();
          basicToken2.TokenType   = Token.Type.DIRECT_TOKEN;
          basicToken2.ByteValue   = nextByte;
          basicToken2.Content     = "" + ConstantData.PetSCIIToChar[nextByte].CharValue;
          basicToken2.StartIndex  = TempDataStartPos + i;
          info.Tokens.Add( basicToken2 );
          lastTokenStartPos = i + 1;
        }
      }
      if ( isVariableName )
      {
        Token basicToken3     = new Token();
        basicToken3.TokenType = Token.Type.VARIABLE;
        for ( int j = lastTokenStartPos; j < tempData.Length; ++j )
        {
          basicToken3.Content += "" + ConstantData.PetSCIIToChar[tempData.ByteAt( j )].CharValue;
        }
        basicToken3.StartIndex = TempDataStartPos + lastTokenStartPos;
        info.Tokens.Add( basicToken3 );
      }
      else if ( isNumeric )
      {
        Token basicToken       = new Token();
        basicToken.TokenType = Token.Type.NUMERIC_LITERAL;
        for ( int j = lastTokenStartPos; j < tempData.Length; ++j )
        {
          basicToken.Content += "" + ConstantData.PetSCIIToChar[tempData.ByteAt( j )].CharValue;
        }
        basicToken.StartIndex = TempDataStartPos + lastTokenStartPos;
        info.Tokens.Add( basicToken );
      }
      else
      {
        for ( int i = lastTokenStartPos; i < tempData.Length; ++i )
        {
          Token basicToken2       = new Token();
          basicToken2.TokenType   = Token.Type.DIRECT_TOKEN;
          basicToken2.Content     = "" + ConstantData.PetSCIIToChar[tempData.ByteAt( i )].CharValue;
          basicToken2.StartIndex  = TempDataStartPos + i;
          info.Tokens.Add( basicToken2 );
          lastTokenStartPos = i + 1;
        }
      }
    }



    internal LineInfo TokenizeLine( string Line, int LineIndex, ref int LastLineNumber )
    {
      if ( Line.Length == 0 )
      {
        var emptyInfo = new LineInfo();
        emptyInfo.LineIndex = LineIndex;
        emptyInfo.Line = Line;

        return emptyInfo;
      }

      if ( Line.StartsWith( "#" ) )
      {
        // hard comment
        var hardInfo = new LineInfo();
        hardInfo.Line = Line;
        hardInfo.Tokens.Add( new Token() { Content = Line, TokenType = Token.Type.HARD_COMMENT } );
        hardInfo.LineIndex = LineIndex;

        return hardInfo;
      }

      int     endOfDigitPos = -1;
      for ( int i = 0; i < Line.Length; ++i )
      {
        if ( ( Line[i] < '0' )
        ||   ( Line[i] > '9' ) )
        {
          break;
        }
        ++endOfDigitPos;
      }
      LineInfo info = new LineInfo();
      info.LineIndex = LineIndex;
      info.Line = Line;

      if ( !LabelMode )
      {
        LastLineNumber = TokenizeLineNumber( Line, LineIndex, LastLineNumber, endOfDigitPos, info );
      }

      int       posInLine = endOfDigitPos + 1;
      bool      insideMacro = false;
      bool      insideDataStatement = false;
      bool      insideREMStatement = false;
      int       macroStartPos = -1;
      GR.Memory.ByteBuffer tempData = new GR.Memory.ByteBuffer();

      int numCharsSkipped = TranslateCharactersToPETSCII( Line, LineIndex, endOfDigitPos, ref posInLine, ref insideMacro, ref macroStartPos, tempData );

      
      if ( tempData.Length + endOfDigitPos + 1 > Settings.BASICDialect.SafeLineLength )
      {
        AddWarning( LineIndex, Types.ErrorCode.W1001_BASIC_LINE_TOO_LONG_FOR_MANUAL_ENTRY, "Line " + LastLineNumber + " is too long for manual entry", 0, info.Line.Length );
      }

      // now the real token crunching
      int bytePos = 0;

      while ( bytePos < tempData.Length )
      {
        byte    nextByte = tempData.ByteAt( bytePos );
        if ( insideREMStatement )
        {
          if ( !Settings.StripREM )
          {
            AddDirectToken( info, nextByte, bytePos );
          }
          ++bytePos;
          continue;
        }
        if ( nextByte < 128 )
        {
          // kein Token
          if ( nextByte == 32 )
          {
            // Space, direkt einfügen
            if ( !Settings.StripSpaces )
            {
              AddDirectToken( info, nextByte, bytePos );
            }
            ++bytePos;
            continue;
          }
          if ( nextByte == 34 )
          {
            // Anführungszeichen, abschliessendes Anführungszeichen suchen
            string stringLiteral = "";
            int     startIndex = bytePos;
            do
            {
              var c64Key = ConstantData.FindC64KeyByPETSCII( nextByte );
              if ( ( c64Key != null )
              &&   ( nextByte != 32 )   // do not replace for Space
              &&   ( c64Key.Replacements.Count > 0 ) )
              {
                stringLiteral += "{" + c64Key.Replacements[0] + "}";
              }
              else
              {
                stringLiteral += ConstantData.PetSCIIToChar[nextByte].CharValue;
              }
              info.LineData.AppendU8( nextByte );
              ++bytePos;
              if ( bytePos < tempData.Length )
              {
                nextByte = tempData.ByteAt( bytePos );
              }
            }
            while ( ( bytePos < tempData.Length )
            &&      ( nextByte != 34 ) );
            if ( nextByte == 34 )
            {
              stringLiteral += ConstantData.PetSCIIToChar[nextByte].CharValue;
              info.LineData.AppendU8( nextByte );
            }
            Token basicToken = new Token();
            basicToken.TokenType = Token.Type.STRING_LITERAL;
            basicToken.Content = stringLiteral;
            basicToken.StartIndex = startIndex;
            info.Tokens.Add( basicToken );

            ++bytePos;
            continue;
          }
          if ( insideDataStatement )
          {
            // innerhalb von Data keine Token-Aufschlüsselung
            if ( nextByte == ':' )
            {
              insideDataStatement = false;
            }

            AddDirectToken( info, nextByte, bytePos );
            ++bytePos;
            continue;
          }
          if ( nextByte == 0x3f )
          {
            // ? durch PRINT ersetzen
            nextByte = 0x99;
            info.LineData.AppendU8( nextByte );

            Token basicToken = new Token();
            basicToken.TokenType = Token.Type.BASIC_TOKEN;
            basicToken.ByteValue = nextByte;
            basicToken.Content = "" + ConstantData.PetSCIIToChar[0x3f].CharValue;
            basicToken.StartIndex = bytePos;
            info.Tokens.Add( basicToken );
            ++bytePos;
            continue;
          }
          if ( ( nextByte >= 0x30 )
          &&   ( nextByte < 0x3c ) )
          {
            // numerisch bzw. Klammern?, direkt einsetzen
            AddDirectToken( info, nextByte, bytePos );
            ++bytePos;
            continue;
          }
          // is there a token now?
          if ( FindOpcode( tempData, ref bytePos, info, ref insideDataStatement, ref insideREMStatement ) )
          {
            continue;
          }

          // not a token, add directly
          AddDirectToken( info, nextByte, bytePos );

          if ( IsLightningOrLaserBASIC() )
          {
            // random hack -> avoid letters following letters forming tokens (e.g. s-or-t)
            if ( ( nextByte >= 'A' )
            &&   ( nextByte <= 'Z' ) )
            {
              // try to scan forward to deal with something like ONxGOSUB or PROCsort (not sORt)
              int     forwardPos = bytePos;
              while ( ( forwardPos + 1 < (int)tempData.Length )
              &&      ( ( ( tempData.ByteAt( forwardPos + 1 ) >= 'A' )
              &&          ( tempData.ByteAt( forwardPos + 1 ) <= 'Z' ) )
              ||        ( tempData.ByteAt( forwardPos + 1 ) == '%' ) ) )
              {
                ++forwardPos;
              }
              ++bytePos;

              Token  foundToken;
              Opcode foundOpcode;
              if ( FindOpcodeRightAligned( tempData, ref bytePos, forwardPos, info, ref insideDataStatement, ref insideREMStatement, out foundToken, out foundOpcode ) )
              {
                // inserted a new opcode, but maybe we need direct tokens in between?
                // TODO
                if ( info.Tokens[info.Tokens.Count - 1].StartIndex > bytePos )
                {
                  // yes, direct tokens!
                  for ( int i = bytePos; i < info.Tokens[info.Tokens.Count - 1].StartIndex; ++i )
                  {
                    Token basicToken = new Token();
                    basicToken.TokenType = Token.Type.DIRECT_TOKEN;
                    basicToken.ByteValue = tempData.ByteAt( i );
                    basicToken.Content = "" + ConstantData.PetSCIIToChar[(byte)basicToken.ByteValue].CharValue;
                    basicToken.StartIndex = i;

                    info.Tokens.Add( basicToken );

                    info.LineData.AppendU8( (byte)basicToken.ByteValue );
                  }
                }

                info.Tokens.Add( foundToken );
                if ( foundOpcode.InsertionValue > 255 )
                {
                  info.LineData.AppendU16NetworkOrder( (ushort)foundOpcode.InsertionValue );
                }
                else
                {
                  info.LineData.AppendU8( (byte)foundOpcode.InsertionValue );
                }
                //BytePos += opcode.Command.Length;
                insideDataStatement = ( foundOpcode.Command == "DATA" );
              }
              else
              {
                for ( int i = bytePos; i <= forwardPos; ++i )
                {
                  AddDirectToken( info, tempData.ByteAt( i ), i );
                }
              }
              bytePos = forwardPos + 1;
              continue;
            }
          }

          ++bytePos;
          continue;
        }
        else if ( nextByte == 0xff )
        {
          // PI
          AddDirectToken( info, nextByte, bytePos );
          ++bytePos;
          continue;
        }
        else
        {
          ++bytePos;
          continue;
        }
      }

      if ( info.LineData.Length > 250 )
      {
        AddError( LineIndex, Types.ErrorCode.E3006_BASIC_LINE_TOO_LONG, "Line is too long, max. 250 bytes possible" );
      }
      

      // update line number references
      for ( int i = 0; i < info.Tokens.Count; ++i )
      {
        Token token = info.Tokens[i];

        if ( ( token.TokenType == Token.Type.BASIC_TOKEN )
        &&   ( ( token.ByteValue == 0x89 )      // GOTO
        ||     ( token.ByteValue == 0x8d ) ) )  // GOSUB
        {
          // look up next token, is it a line number? (spaces are ignored)
          int nextTokenIndex = FindNextToken( info.Tokens, i );
          while ( ( nextTokenIndex != -1 )
          &&      ( nextTokenIndex < info.Tokens.Count ) )
          {
            if ( info.Tokens[nextTokenIndex].TokenType == Token.Type.NUMERIC_LITERAL )
            {
              int referencedLineNumber = GR.Convert.ToI32( info.Tokens[nextTokenIndex].Content );

              info.ReferencedLineNumbers.Add( referencedLineNumber );
              ++nextTokenIndex;
              if ( nextTokenIndex >= info.Tokens.Count )
              {
                break;
              }
              if ( info.Tokens[nextTokenIndex].Content != "," )
              {
                break;
              }
            }
            ++nextTokenIndex;
          }
        }
        if ( ( token.TokenType == Token.Type.BASIC_TOKEN )
        &&   ( ( token.ByteValue == 0xa7 )        // THEN
        ||     ( token.ByteValue == 0x8a ) ) )  // RUN
        {
          // only one line number
          // look up next token, is it a line number? (spaces are ignored)
          int nextTokenIndex = FindNextToken( info.Tokens, i );
          if ( ( nextTokenIndex != -1 )
          && ( nextTokenIndex < info.Tokens.Count ) )
          {
            if ( info.Tokens[nextTokenIndex].TokenType == Token.Type.NUMERIC_LITERAL )
            {
              int referencedLineNumber = GR.Convert.ToI32( info.Tokens[nextTokenIndex].Content );

              info.ReferencedLineNumbers.Add( referencedLineNumber );
            }
          }
        }
      }
      // offset by line number
      for ( int i = 1; i < info.Tokens.Count; ++i )
      {
        info.Tokens[i].StartIndex += endOfDigitPos + 1 + numCharsSkipped;
      }
      return info;
    }



    private bool IsLightningOrLaserBASIC()
    {
      if ( ( Settings.BASICDialect.Name == "Laser BASIC" )
      ||   ( Settings.BASICDialect.Name == "BASIC Lightning" ) )
      {
        return true;
      }
      return false;
    }



    private int TranslateCharactersToPETSCII( string Line, int LineIndex, int endOfDigitPos, ref int posInLine, ref bool insideMacro, ref int macroStartPos, ByteBuffer tempData )
    {
      int numCharsSkipped = 0;

      while ( posInLine < Line.Length )
      {
        char    curChar = Line[posInLine];

        if ( insideMacro )
        {
          if ( curChar == '}' )
          {
            insideMacro = false;

            string macro = Line.Substring( macroStartPos + 1, posInLine - macroStartPos - 1 ).ToUpper();
            int macroCount = 1;

            macro = DetermineMacroCount( macro, out macroCount );

            bool  foundMacro = false;
            foreach ( var key in ConstantData.AllPhysicalKeyInfos )
            {
              if ( key.Replacements.Contains( macro ) )
              {
                for ( int i = 0; i < macroCount; ++i )
                {
                  tempData.AppendU8( key.PetSCIIValue );
                }
                foundMacro = true;
                break;
              }
            }
            if ( !foundMacro )
            {
              byte  petsciiValue = 0;
              if ( byte.TryParse( macro, out petsciiValue ) )
              {
                for ( int i = 0; i < macroCount; ++i )
                {
                  tempData.AppendU8( petsciiValue );
                }
              }
              else if ( macro.StartsWith( "$" ) )
              {
                // a hex value?
                uint    hexValue = 0;

                if ( uint.TryParse( macro.Substring( 1 ), System.Globalization.NumberStyles.HexNumber, null, out hexValue ) )
                {
                  string    value = hexValue.ToString();

                  for ( int j = 0; j < macroCount; ++j )
                  {
                    for ( int i = 0; i < value.Length; ++i )
                    {
                      tempData.AppendU8( (byte)value[i] );
                    }
                  }
                }
                else
                {
                  AddError( LineIndex, Types.ErrorCode.E1301_PSEUDO_OPERATION, "Unknown macro " + macro );
                }
              }
              else
              {
                // do we have a label?
                if ( ASMFileInfo.Labels.ContainsKey( macro ) )
                {
                  string value = ASMFileInfo.Labels[macro].AddressOrValue.ToString();

                  for ( int j = 0; j < macroCount; ++j )
                  {
                    for ( int i = 0; i < value.Length; ++i )
                    {
                      tempData.AppendU8( (byte)value[i] );
                    }
                  }
                }
                else
                {
                  AddError( LineIndex, Types.ErrorCode.E1301_PSEUDO_OPERATION, "Unknown macro " + macro );
                }
              }
            }
          }
          ++posInLine;
          continue;
        }
        if ( curChar == '{' )
        {
          insideMacro = true;
          macroStartPos = posInLine;
          ++posInLine;
          continue;
        }
        // find actual byte value of char
        numCharsSkipped += MapCharacterToActualKey( LineIndex, curChar, posInLine, endOfDigitPos, tempData );
        ++posInLine;
      }

      if ( insideMacro )
      {
        // macro is missing closing }!
        AddError( LineIndex, Types.ErrorCode.E1301_PSEUDO_OPERATION, "Macro is missing closing brace", macroStartPos, 1 );
      }

      return numCharsSkipped;
    }



    private int MapCharacterToActualKey( int LineIndex, char curChar, int posInLine, int endOfDigitPos, ByteBuffer tempData )
    {
      int numCharsSkipped = 0;
      if ( !Settings.UpperCaseMode )
      {
        // lower case mode
        if ( ConstantData.LowerCaseCharTo64Char.ContainsKey( curChar ) )
        {
          var foundKey = ConstantData.LowerCaseCharTo64Char[curChar];
          if ( !foundKey.HasPetSCII )
          {
            AddError( LineIndex, Types.ErrorCode.E3002_BASIC_UNSUPPORTED_CHARACTER, "Unsupported character " + (int)curChar + " encountered" );
          }
          else
          {
            if ( ( curChar != 32 )
            || ( posInLine > endOfDigitPos + 1 ) )
            {
              // strip spaces after line numbers

              // TODO - lower case petscii!
              tempData.AppendU8( foundKey.LowerCasePETSCII );
            }
            else
            {
              ++numCharsSkipped;
            }
          }
          return numCharsSkipped;
        }
      }
      if ( ( !ConstantData.CharToC64Char.ContainsKey( curChar ) )
      ||   ( !ConstantData.CharToC64Char[curChar].HasPetSCII ) )
      {
        AddError( LineIndex, Types.ErrorCode.E3002_BASIC_UNSUPPORTED_CHARACTER, "Unsupported character " + (int)curChar + " encountered" );
      }
      else
      {
        byte    petsciiValue = ConstantData.CharToC64Char[curChar].PetSCIIValue;

        if ( ( petsciiValue >= 224 )
        &&   ( petsciiValue <= 254 ) )
        {
          petsciiValue -= 224 - 160;
        }
        else if ( ( petsciiValue >= 96 )
        &&        ( petsciiValue <= 127 ) )
        {
          petsciiValue += 192 - 96;
        }

        // strip spaces after line numbers - TODO here???
        if ( ( curChar != 32 )
        ||   ( posInLine > endOfDigitPos + 1 ) )
        {
          tempData.AppendU8( petsciiValue );
        }
        else
        {
          ++numCharsSkipped;
        }
      }
      return numCharsSkipped;
    }



    private bool VersionOnlyUsesUpperCase()
    {
      /*
      if ( ( Settings.Version != BasicVersion.LASER_BASIC )
      &&   ( Settings.Version != BasicVersion.BASIC_LIGHTNING ) )
      {
        return true;
      }*/
      return true;
    }



    private int TokenizeLineNumber( string Line, int LineIndex, int LastLineNumber, int endOfDigitPos, LineInfo info )
    {
      if ( endOfDigitPos == -1 )
      {
        AddError( LineIndex, Types.ErrorCode.E3000_BASIC_MISSING_LINE_NUMBER, "Missing line number" );
      }
      else
      {
        Token lineNumberToken = new Token();
        lineNumberToken.TokenType = Token.Type.LINE_NUMBER;
        lineNumberToken.ByteValue = GR.Convert.ToI32( Line.Substring( 0, endOfDigitPos + 1 ) );
        lineNumberToken.Content = Line.Substring( 0, endOfDigitPos + 1 ).Trim();
        lineNumberToken.StartIndex = 0;
        info.Tokens.Add( lineNumberToken );
        info.LineNumber = lineNumberToken.ByteValue;
        if ( ( info.LineNumber < 0 )
        ||   ( info.LineNumber > 63999 ) )
        {
          AddError( LineIndex, Types.ErrorCode.E3001_BASIC_INVALID_LINE_NUMBER, "Unsupported line number, must be in the range 0 to 63999",
                    lineNumberToken.StartIndex, lineNumberToken.Content.Length );
        }
        else if ( info.LineNumber <= LastLineNumber )
        {
          AddError( LineIndex,
                    Types.ErrorCode.E3001_BASIC_INVALID_LINE_NUMBER,
                    "Line number not increasing, must be higher than the previous line number",
                    lineNumberToken.StartIndex, lineNumberToken.Content.Length );
        }
        LastLineNumber = info.LineNumber;
      }

      return LastLineNumber;
    }



    private bool FindOpcode( ByteBuffer TempData, ref int BytePos, LineInfo Info, ref bool InsideDataStatement, ref bool InsideREMStatement )
    {
      if ( IsLightningOrLaserBASIC() )
      {
        // special behavior - no token after PROC and LABEL
        int   prevTokenIndex = Info.Tokens.Count - 1;

        while ( prevTokenIndex >= 0 )
        {
          if ( Info.Tokens[prevTokenIndex].Content == " " )
          {
            --prevTokenIndex;
            continue;
          }
          if ( ( Info.Tokens[prevTokenIndex].TokenType == Token.Type.DIRECT_TOKEN )
          &&   ( ( Info.Tokens[prevTokenIndex].ByteValue == Settings.BASICDialect.Opcodes["PROC"].InsertionValue )
          ||     ( Info.Tokens[prevTokenIndex].ByteValue == Settings.BASICDialect.Opcodes["LABEL"].InsertionValue ) ) )
          {
            // previous token was PROC or LABEL
            return false;
          }
          break;
        }

        // special behavior - no token after TASK<number>,
        if ( ( Info.Tokens.Count > 3 )
        &&   ( Info.Tokens[Info.Tokens.Count - 3].ByteValue == Settings.BASICDialect.Opcodes["TASK"].InsertionValue )
        &&   ( Info.Tokens[Info.Tokens.Count - 2].TokenType == Token.Type.NUMERIC_LITERAL )
        &&   ( Info.Tokens[Info.Tokens.Count - 1].ByteValue == ',' ) )
        {
          // previous token was TASK<number>,
          return false;
        }
      }

      bool entryFound = false;
      Opcode potentialOpcode = null;
      foreach ( var opcodeEntry in Settings.BASICDialect.Opcodes )
      {
        Opcode  opcode = opcodeEntry.Value;

        entryFound = true;
        for ( int i = 0; i < opcode.Command.Length; ++i )
        {
          if ( BytePos + i >= TempData.Length )
          {
            // can't be this token
            entryFound = false;
            break;
          }
          if ( TempData.ByteAt( BytePos + i ) != (byte)opcode.Command[i] )
          {
            entryFound = false;
            break;
          }
        }
        if ( entryFound )
        {
          if ( ( potentialOpcode == null )
          ||   ( opcode.Command.Length > potentialOpcode.Command.Length ) )
          {
            potentialOpcode = opcode;
          }
        }
      }
      if ( potentialOpcode != null )
      {
        Token basicToken = new Token();
        basicToken.TokenType = Token.Type.BASIC_TOKEN;
        basicToken.ByteValue = potentialOpcode.InsertionValue;
        basicToken.Content = potentialOpcode.Command;
        basicToken.StartIndex = BytePos;
        Info.Tokens.Add( basicToken );

        if ( potentialOpcode.InsertionValue > 255 )
        {
          Info.LineData.AppendU16NetworkOrder( (ushort)potentialOpcode.InsertionValue );
        }
        else
        {
          Info.LineData.AppendU8( (byte)potentialOpcode.InsertionValue );
        }
        BytePos += potentialOpcode.Command.Length;

        InsideDataStatement = ( potentialOpcode.Command == "DATA" );
        if ( IsComment( potentialOpcode ) )
        {
          InsideREMStatement = true;

          if ( Settings.StripREM )
          {
            Info.LineData.Truncate( 1 );
            if ( potentialOpcode.InsertionValue > 255 )
            {
              Info.LineData.Truncate( 1 );
            }
            if ( ( Info.LineData.Length > 0 )
            &&   ( Info.LineData.ByteAt( (int)Info.LineData.Length - 1 ) == ':' ) )
            {
              // remove optional separator before REM
              Info.LineData.Truncate( 1 );
            }
          }
        }
        return true;
      }

      // is it an extended token?
      entryFound = true;
      if ( Settings.BASICDialect.ExOpcodes.Count == 0 )
      {
        entryFound = false;
      }
      foreach ( KeyValuePair<string, Opcode> opcodeEntry in Settings.BASICDialect.ExOpcodes )
      {
        Opcode opcode = opcodeEntry.Value;

        entryFound = true;
        for ( int i = 0; i < opcode.Command.Length; ++i )
        {
          if ( BytePos + i >= TempData.Length )
          {
            // can't be this token
            entryFound = false;
            break;
          }
          if ( TempData.ByteAt( BytePos + i ) != (byte)opcode.Command[i] )
          {
            entryFound = false;
            break;
          }
        }
        if ( entryFound )
        {
          Token basicToken = new Token();
          basicToken.TokenType = Token.Type.EX_BASIC_TOKEN;
          basicToken.ByteValue = opcode.InsertionValue;
          basicToken.Content = opcode.Command;
          basicToken.StartIndex = BytePos;
          Info.Tokens.Add( basicToken );

          if ( opcode.InsertionValue > 255 )
          {
            Info.LineData.AppendU16NetworkOrder( (ushort)opcode.InsertionValue );
          }
          else
          {
            Info.LineData.AppendU8( (byte)opcode.InsertionValue );
          }
          BytePos += opcode.Command.Length;

          InsideDataStatement = false;
          break;
        }
      }
      return entryFound;
    }



    private bool FindOpcodeRightAligned( ByteBuffer TempData, ref int BytePos, int LastBytePos, LineInfo Info, ref bool InsideDataStatement, ref bool InsideREMStatement, out Token FoundToken, out Opcode FoundOpcode )
    {
      FoundToken = null;
      FoundOpcode = null;
      bool entryFound = true;
      if ( Settings.BASICDialect.Opcodes.Count == 0 )
      {
        entryFound = false;
      }
      foreach ( var opcodeEntry in Settings.BASICDialect.Opcodes )
      {
        Opcode  opcode = opcodeEntry.Value;

        entryFound = true;
        if ( opcode.Command.Length > LastBytePos - BytePos + 1 )
        {
          entryFound = false;
        }
        else
        {
          for ( int i = 0; i < opcode.Command.Length; ++i )
          {
            if ( TempData.ByteAt( LastBytePos - opcode.Command.Length + i + 1 ) != (byte)opcode.Command[i] )
            {
              entryFound = false;
              break;
            }
          }
        }
        if ( entryFound )
        {
          FoundToken = new Token();
          FoundToken.TokenType = Token.Type.BASIC_TOKEN;
          FoundToken.ByteValue = opcode.InsertionValue;
          FoundToken.Content = opcode.Command;
          FoundToken.StartIndex = LastBytePos - opcode.Command.Length;

          FoundOpcode = opcode;
          return true;
        }
      }
      if ( entryFound )
      {
        return true;
      }

      // is it an extended token?
      entryFound = true;
      if ( Settings.BASICDialect.ExOpcodes.Count == 0 )
      {
        entryFound = false;
      }
      foreach ( KeyValuePair<string, Opcode> opcodeEntry in Settings.BASICDialect.ExOpcodes )
      {
        Opcode opcode = opcodeEntry.Value;

        entryFound = true;
        for ( int i = 0; i < opcode.Command.Length; ++i )
        {
          if ( BytePos + i >= TempData.Length )
          {
            // can't be this token
            entryFound = false;
            break;
          }
          if ( TempData.ByteAt( BytePos + i ) != (byte)opcode.Command[i] )
          {
            entryFound = false;
            break;
          }
        }
        if ( entryFound )
        {
          FoundToken = new Token();
          FoundToken.TokenType = Token.Type.EX_BASIC_TOKEN;
          FoundToken.ByteValue = opcode.InsertionValue;
          FoundToken.Content = opcode.Command;
          FoundToken.StartIndex = BytePos;

          FoundOpcode = opcode;
          return true;
        }
      }
      return entryFound;
    }



    private void ProcessLines( string[] lines, bool LabelMode )
    {
      int lineIndex = -1;
      int lastLineNumber = -1;
      foreach ( string lineArg in lines )
      {
        ++lineIndex;
        if ( lineArg.Length == 0 )
        {
          continue;
        }
        string line = lineArg.Trim();
        if ( line.Length == 0 )
        {
          continue;
        }

        var info = TokenizeLine( line, lineIndex, ref lastLineNumber );

        var pureInfo = PureTokenizeLine( line );

        int   tokenIndex = 0;
        foreach ( var variable in pureInfo.Tokens )
        {
          if ( variable.TokenType == Token.Type.VARIABLE )
          {
            var     symbolType = SymbolInfo.Types.VARIABLE_NUMBER;
            string  varName = variable.Content;

            // verify next token
            if ( variable.Content.EndsWith( "$" ) )
            {
              symbolType = SymbolInfo.Types.VARIABLE_STRING;
              if ( varName.Length > 3 )
              {
                // cut to signifact characters
                varName = varName.Substring( 0, 2 ) + "$";
              }
            }
            else if ( variable.Content.EndsWith( "%" ) )
            {
              symbolType = SymbolInfo.Types.VARIABLE_INTEGER;
              if ( varName.Length > 3 )
              {
                // cut to signifact characters
                varName = varName.Substring( 0, 2 ) + "%";
              }
            }
            else if ( ( tokenIndex + 1 < pureInfo.Tokens.Count )
            &&        ( variable.StartIndex + variable.Content.Length == pureInfo.Tokens[tokenIndex + 1].StartIndex ) )
            {
              var nextToken = pureInfo.Tokens[tokenIndex + 1];
              if ( nextToken.Content == "(" )
              {
                symbolType = SymbolInfo.Types.VARIABLE_ARRAY;
                if ( varName.Length > 3 )
                {
                  // cut to signifact characters
                  varName = varName.Substring( 0, 2 );
                }
                varName += "(";
              }
              else if ( varName.Length > 2 )
              {
                // cut to signifact characters
                varName = varName.Substring( 0, 2 );
              }
            }
            else
            {
              if ( varName.Length > 2 )
              {
                // cut to signifact characters
                varName = varName.Substring( 0, 2 );
              }
            }

            if ( !ASMFileInfo.Labels.ContainsKey( varName ) )
            {
              var symbolInfo              = new SymbolInfo();
              symbolInfo.AddressOrValue   = 0;
              symbolInfo.CharIndex        = variable.StartIndex;
              symbolInfo.Name             = varName;
              symbolInfo.DocumentFilename = _Config.InputFile;
              symbolInfo.Info             = "Number";
              symbolInfo.Length           = varName.Length;
              symbolInfo.LineIndex        = lineIndex;
              symbolInfo.LocalLineIndex   = lineIndex;
              symbolInfo.Type             = symbolType;

              ASMFileInfo.Labels.Add( varName, symbolInfo );
            }
            var existingSymbolInfo = ASMFileInfo.Labels[varName];
            existingSymbolInfo.References.Add( lineIndex );
            
          }
          ++tokenIndex;
        }

        m_LineInfos[info.LineIndex] = info;
      }
    }



    public override GR.Collections.MultiMap<string, SymbolInfo> KnownTokenInfo()
    {
      GR.Collections.MultiMap<string, SymbolInfo> knownTokens = new GR.Collections.MultiMap<string, SymbolInfo>();

      foreach ( KeyValuePair<string, SymbolInfo> label in ASMFileInfo.Labels )
      {
        knownTokens.Add( label.Key, label.Value );
      }
      foreach ( KeyValuePair<string, Types.ASM.UnparsedEvalInfo> label in ASMFileInfo.UnparsedLabels )
      {
        var token = new SymbolInfo();

        token.Name = label.Key;
        knownTokens.Add( token.Name, token );
      }
      return knownTokens;
    }



    public override List<Types.AutoCompleteItemInfo> KnownTokens()
    {
      List<Types.AutoCompleteItemInfo> knownTokens = new List<Types.AutoCompleteItemInfo>();

      foreach ( var label in ASMFileInfo.Labels )
      {
        knownTokens.Add( new Types.AutoCompleteItemInfo() { Symbol = label.Value, Token = label.Key, ToolTipTitle = label.Key } );
      }
      return knownTokens;
    }
    
    
    
    public override bool Assemble( CompileConfig Config )
    {
      GR.Memory.ByteBuffer result = new GR.Memory.ByteBuffer();

      int     startAddress = Config.StartAddress;
      if ( startAddress == -1 )
      {
        // TODO - C64 specific!
        startAddress = 0x0801;
      }

      result.AppendU16( (ushort)startAddress );

      int     curAddress = startAddress;
      foreach ( LineInfo info in m_LineInfos.Values )
      {
        if ( info.LineData.Length == 0 )
        {
          // could be a line with stripped REM, so only the line number is left
          //  -> check whether the line is referenced, so either skip it or add a :
          if ( !IsLineNumberReferenced( info.LineNumber ) )
          {
            // skip this line
            continue;
          }
          // we need to keep the line, add a :
          info.LineData.AppendU8( (byte)':' );
        }


        if ( info.LineData.Length > 0 )
        {
          // pointer to next line
          result.AppendU16( (ushort)( curAddress + info.LineData.Length + 5 ) );
          result.AppendU16( (ushort)info.LineNumber );
          result.Append( info.LineData );

          // end of line
          result.AppendU8( 0 );

          curAddress += (int)info.LineData.Length + 5;
        }
      }
      result.AppendU16( 0 );

      int     originalSize = (int)result.Length - 2;


      //Debug.Log( "Compiled: " + result.ToString() );

      string    outputPureFilename = "HURZ";
      try
      {
        outputPureFilename = System.IO.Path.GetFileNameWithoutExtension( Config.OutputFile );
      }
      catch ( Exception )
      {
        // arghh exceptions!
      }
      int     fileStartAddress = -1;

      AssembledOutput = new AssemblyOutput();
      AssembledOutput.Assembly = result;
      if ( Config.TargetType == Types.CompileTargetType.T64 )
      {
        Formats.T64 t64 = new C64Studio.Formats.T64();

        Formats.T64.FileRecord  record = new C64Studio.Formats.T64.FileRecord();

        record.Filename = Util.ToFilename( outputPureFilename );
        record.StartAddress = (ushort)fileStartAddress;

        t64.TapeInfo.Description = "C64S tape file\r\nDemo tape";
        t64.TapeInfo.UserDescription = "USERDESC";
        t64.FileRecords.Add( record );
        t64.FileDatas.Add( result );

        AssembledOutput.Assembly = t64.Compile();
      }
      else if ( Config.TargetType == Types.CompileTargetType.D64 )
      {
        Formats.D64 d64 = new C64Studio.Formats.D64();

        d64.CreateEmptyMedia();

        GR.Memory.ByteBuffer    bufName = Util.ToFilename( outputPureFilename );
        d64.WriteFile( bufName, AssembledOutput.Assembly, C64Studio.Types.FileType.PRG );

        AssembledOutput.Assembly = d64.Compile();
      }
      else if ( Config.TargetType == Types.CompileTargetType.D81 )
      {
        Formats.D81 d81 = new C64Studio.Formats.D81();

        d81.CreateEmptyMedia();

        GR.Memory.ByteBuffer    bufName = Util.ToFilename( outputPureFilename );
        d81.WriteFile( bufName, AssembledOutput.Assembly, C64Studio.Types.FileType.PRG );

        AssembledOutput.Assembly = d81.Compile();
      }
      else if ( Config.TargetType == Types.CompileTargetType.TAP )
      {
        Formats.Tap tap = new C64Studio.Formats.Tap();

        tap.WriteFile( Util.ToFilename( outputPureFilename ), AssembledOutput.Assembly, C64Studio.Types.FileType.PRG );
        AssembledOutput.Assembly = tap.Compile();
      }
      else if ( ( Config.TargetType == Types.CompileTargetType.CARTRIDGE_8K_BIN )
      ||        ( Config.TargetType == Types.CompileTargetType.CARTRIDGE_8K_CRT ) )
      {
        if ( result.Length < 8192 )
        {
          // fill up
          AssembledOutput.Assembly = result + new GR.Memory.ByteBuffer( 8192 - result.Length );
        }
      }
      else if ( ( Config.TargetType == Types.CompileTargetType.CARTRIDGE_16K_BIN )
      ||        ( Config.TargetType == Types.CompileTargetType.CARTRIDGE_16K_CRT ) )
      {
        if ( result.Length < 16384 )
        {
          // fill up
          result = result + new GR.Memory.ByteBuffer( 16384 - result.Length );
        }
        if ( Config.TargetType == Types.CompileTargetType.CARTRIDGE_16K_CRT )
        {
          // build cartridge header
          GR.Memory.ByteBuffer    header = new GR.Memory.ByteBuffer();

          header.AppendHex( "43363420434152545249444745202020" ); // "C64 CARTRIDGE   "
          header.AppendU32NetworkOrder( 0x40 );
          header.AppendU16NetworkOrder( 0x0100 );
          header.AppendU16( 0 );
          header.AppendU8( 0 );
          header.AppendU8( 0 );

          // reserved
          header.AppendU8( 0 );
          header.AppendU8( 0 );
          header.AppendU8( 0 );
          header.AppendU8( 0 );
          header.AppendU8( 0 );
          header.AppendU8( 0 );   

          // cartridge name
          string name = System.IO.Path.GetFileNameWithoutExtension( m_CompileTargetFile ).ToUpper();

          if ( name.Length > 32 )
          {
            name = name.Substring( 0, 32 );
          }
          while ( name.Length < 32 )
          {
            name += (char)0;
          }
          foreach ( char aChar in name )
          {
            header.AppendU8( (byte)aChar );
          }

          GR.Memory.ByteBuffer chip = new GR.Memory.ByteBuffer();

          chip.AppendHex( "43484950" );   // chip
          uint length = 16 + result.Length;
          chip.AppendU32NetworkOrder( length );
          chip.AppendU16( 0 );  // ROM
          chip.AppendU16( 0 );  // Bank number
          chip.AppendU16NetworkOrder( 0x8000 ); // loading start address
          chip.AppendU16NetworkOrder( 0x4000 ); // rom size

          chip.Append( result );

          AssembledOutput.Assembly = header + chip;
        }
      }
      AssembledOutput.OriginalAssemblyStartAddress  = startAddress;
      AssembledOutput.OriginalAssemblySize          = originalSize;
      return true;
    }



    private bool IsLineNumberReferenced( int LineNumber )
    {
      foreach ( var otherLines in m_LineInfos.Values )
      {
        if ( otherLines.ReferencedLineNumbers.ContainsValue( LineNumber ) )
        {
          return true;
        }
      }
      return false;
    }



    private ByteBuffer PETSCIIify( ByteBuffer LineData )
    {
      ByteBuffer    result = new ByteBuffer( LineData );

      for ( int i = 0; i < result.Length; ++i )
      {
        byte    basicByte = result.ByteAt( i );

        if ( ( basicByte >= 0x60 )
        &&   ( basicByte <= 0x7f ) )
        {
          result.SetU8At( i, (byte)( basicByte + 192 - 96 ) );
        }
        else if ( ( basicByte >= 0xe0 )
        &&        ( basicByte <= 0xfe ) )
        {
          result.SetU8At( i, (byte)( basicByte + 160 - 224 ) );
        }
      }
      return result;
    }



    public override bool DocumentAndLineFromGlobalLine( int GlobalLine, out string DocumentFile, out int DocumentLine )
    {
      DocumentLine = GlobalLine;
      DocumentFile = m_Filename;
      return true;
    }



    private string DecompileStringLiteral( string Literal )
    {
      // replace value with char
      StringBuilder sbTemp = new StringBuilder();

      int     charPos = 0;
      bool    insideTag = false;

      while ( charPos < Literal.Length )
      {
        char  value = Literal[charPos];
        if ( value == '{' )
        {
          insideTag = true;
          sbTemp.Append( value );
        }
        else if ( value == '}' )
        {
          sbTemp.Append( value );
          insideTag = false;
        }
        else if ( insideTag )
        {
          sbTemp.Append( value );
        }
        else
        {
          char replacement = ConstantData.PetSCIIToChar[(byte)value].CharValue;
          sbTemp.Append( replacement );
        }
        ++charPos;
      }

      return sbTemp.ToString();
    }



    public string EncodeToLabels()
    {
      StringBuilder sb = new StringBuilder();
      GR.Collections.Map<int,string>     lineNumberReference = new GR.Collections.Map<int, string>();
      foreach ( KeyValuePair<int,LineInfo> lineInfo in m_LineInfos )
      {
        if ( lineInfo.Value.ReferencedLineNumbers.Count > 0 )
        {
          foreach ( int number in lineInfo.Value.ReferencedLineNumbers )
          {
            if ( !lineNumberReference.ContainsKey( number ) )
            {
              string    newLabel = "LABEL" + number.ToString();
              lineNumberReference[number] = newLabel;
            }
          }
        }
      }

      foreach ( KeyValuePair<int,LineInfo> lineInfo in m_LineInfos )
      {
        if ( lineNumberReference.ContainsKey( lineInfo.Value.LineNumber ) )
        {
          // something is referencing this line
          sb.AppendLine();
          sb.Append( lineNumberReference[lineInfo.Value.LineNumber] + "\r\n" );
        }
        for ( int i = 0; i < lineInfo.Value.Tokens.Count; ++i )
        {
          Token token = lineInfo.Value.Tokens[i];
          if ( token.TokenType == Token.Type.LINE_NUMBER )
          {
            continue;
          }
          /*
          // TODO - only use if we can re-merge single tokens from label mode
          if ( ( token.TokenType == Token.Type.DIRECT_TOKEN )
          &&   ( token.Content == ":" ) )
          {
            sb.AppendLine();
            continue;
          }
           */

          if ( token.TokenType == Token.Type.BASIC_TOKEN )
          {
            if ( ( token.ByteValue == Settings.BASICDialect.Opcodes["RUN"].InsertionValue )
            ||   ( token.ByteValue == Settings.BASICDialect.Opcodes["THEN"].InsertionValue ) )
            {
              // insert label instead of line number
              if ( i + 1 < lineInfo.Value.Tokens.Count )
              {
                int     refNo = -1;
                if ( int.TryParse( lineInfo.Value.Tokens[i + 1].Content, out refNo ) )
                {
                  if ( !lineNumberReference.ContainsKey( refNo ) )
                  {
                    Debug.Log( "Error, found linenumber without reference " + refNo + " in line " + lineInfo.Value.LineNumber );
                    ++i;
                    continue;
                  }

                  sb.Append( token.Content + " " + lineNumberReference[refNo].ToString() );
                  ++i;
                  continue;
                }
              }
            }
            if ( ( token.ByteValue == Settings.BASICDialect.Opcodes["GOTO"].InsertionValue )
            ||   ( token.ByteValue == Settings.BASICDialect.Opcodes["GOSUB"].InsertionValue ) )
            {
              // ON x GOTO/GOSUB can have more than one line number
              // insert label instead of line number
              //sb.Append( token.Content + " " );
              sb.Append( token.Content );

              int nextIndex = i + 1;
              bool mustBeComma = false;
              while ( nextIndex < lineInfo.Value.Tokens.Count )
              {
                Token nextToken = lineInfo.Value.Tokens[nextIndex];
                if ( !mustBeComma )
                {
                  if ( nextToken.TokenType == Token.Type.NUMERIC_LITERAL )
                  {
                    // numeric!
                    int refNo = GR.Convert.ToI32( nextToken.Content );
                    if ( !lineNumberReference.ContainsKey( refNo ) )
                    {
                      Debug.Log( "Error, found linenumber without reference " + refNo + " in line " + lineInfo.Value.LineNumber );
                      continue;
                    }
                    sb.Append( lineNumberReference[refNo].ToString() );
                  }
                  else if ( ( nextToken.TokenType == Token.Type.DIRECT_TOKEN )
                  &&        ( nextToken.Content == " " ) )
                  {
                    if ( !Settings.StripSpaces )
                    {
                      sb.Append( nextToken.Content );
                    }
                    ++nextIndex;
                    continue;
                  }
                  else
                  {
                    // error or end
                    break;
                  }
                  mustBeComma = true;
                }
                else
                {
                  if ( ( nextToken.TokenType != Token.Type.DIRECT_TOKEN )
                  ||   ( nextToken.ByteValue != ',' ) )
                  {
                    // error or end, not a comma
                    --nextIndex;
                    break;
                  }
                  sb.Append( ',' );
                  mustBeComma = false;
                }
                ++nextIndex;
              }
              i = nextIndex;
              continue;
            }
          }
          // if we got here there was no label inserted
          sb.Append( token.Content );
        }
        sb.AppendLine();
      }
      return sb.ToString();
    }



    public bool IsComment( Token token )
    {
      if ( token.ByteValue == Settings.BASICDialect.Opcodes["REM"].InsertionValue )
      {
        return true;
      }
      if ( IsLightningOrLaserBASIC() )
      {
        if ( token.Content == "'" )
        {
          return true;
        }
      }
      return false;
    }



    // finds next non-space token or returns -1
    private int FindNextToken( List<Token> Tokens, int StartIndex )
    {
      int curIndex = StartIndex + 1;

      while ( curIndex < Tokens.Count )
      {
        if ( ( Tokens[curIndex].TokenType != Token.Type.DIRECT_TOKEN )
        ||   ( Tokens[curIndex].ByteValue != 0x20 ) )
        {
          // not a Space
          return curIndex;
        }
        ++curIndex;
      }
      return -1;
    }



    public string DecodeFromLabels()
    {
      StringBuilder sb = new StringBuilder();
      GR.Collections.Map<string,int>     labelToNumber = new GR.Collections.Map<string, int>();

      int     lineNumber = 10;

      // collect labels
      foreach ( KeyValuePair<int,LineInfo> lineInfo in m_LineInfos )
      {
        if ( ( lineInfo.Value.Tokens.Count == 1 )
        &&   ( lineInfo.Value.Tokens[0].TokenType == Token.Type.HARD_COMMENT ) )
        {
          // leave as is
          continue;
        }

        if ( ( lineInfo.Value.Tokens.Count > 0 )
        &&   ( lineInfo.Value.Tokens[0].TokenType == Token.Type.EX_BASIC_TOKEN )
        &&   ( lineInfo.Value.Tokens[0].ByteValue == Settings.BASICDialect.ExOpcodes["LABEL"].InsertionValue ) )
        {
          // skip label definitions
          if ( lineInfo.Value.Tokens.Count > 1 )
          {
            string labelToReplace = "LABEL" + lineInfo.Value.Tokens[1].Content;

            labelToNumber[labelToReplace] = lineNumber;
            //Debug.Log( "Replace label " + labelToReplace + " with line " + lineNumber );
          }
          else
          {
            AddError( lineInfo.Value.LineIndex, Types.ErrorCode.E3003_BASIC_LABEL_MALFORMED, "Label malformed" );
          }
          continue;
        }
        lineNumber += 10;
      }
      lineNumber = 10;
      foreach ( KeyValuePair<int, LineInfo> lineInfo in m_LineInfos )
      {
        if ( ( lineInfo.Value.Tokens.Count == 1 )
        &&   ( lineInfo.Value.Tokens[0].TokenType == Token.Type.HARD_COMMENT ) )
        {
          // leave as is
          sb.AppendLine( lineInfo.Value.Tokens[0].Content );
          continue;
        }

        bool    hadREM = false;

        // is this a label definition?
        int nextTokenIndex = FindNextToken( lineInfo.Value.Tokens, -1 );
        int nextTokenIndex2 = FindNextToken( lineInfo.Value.Tokens, nextTokenIndex );
        int nextTokenIndex3 = FindNextToken( lineInfo.Value.Tokens, nextTokenIndex2 );
        if ( ( nextTokenIndex != -1 )
        &&   ( nextTokenIndex2 != -1 )
        &&   ( nextTokenIndex3 == -1 ) )
        {
          if ( ( lineInfo.Value.Tokens[nextTokenIndex].TokenType == Token.Type.EX_BASIC_TOKEN )
          &&   ( lineInfo.Value.Tokens[nextTokenIndex].ByteValue == Settings.BASICDialect.ExOpcodes["LABEL"].InsertionValue )
          &&   ( lineInfo.Value.Tokens[nextTokenIndex2].TokenType == Token.Type.NUMERIC_LITERAL ) )
          {
            // a label definition
            continue;
          }
        }

        sb.Append( lineNumber.ToString() + " " );
        for ( int tokenIndex = 0; tokenIndex < lineInfo.Value.Tokens.Count; ++tokenIndex  )
        {

          Token token = lineInfo.Value.Tokens[tokenIndex];

          if ( ( token.TokenType == Token.Type.DIRECT_TOKEN )
          &&   ( Settings.StripSpaces )
          &&   ( !hadREM )
          &&   ( token.Content.Trim().Length == 0 ) )
          {
            continue;
          }
          bool    tokenIsInserted = false;

          if ( IsComment( token ) )
          {
            hadREM = true;
          }


          if ( ( token.TokenType == Token.Type.BASIC_TOKEN )
          &&   ( ( token.ByteValue == Settings.BASICDialect.Opcodes["GOTO"].InsertionValue )
          ||     ( token.ByteValue == Settings.BASICDialect.Opcodes["GOSUB"].InsertionValue )
          ||     ( token.ByteValue == Settings.BASICDialect.Opcodes["THEN"].InsertionValue )
          ||     ( token.ByteValue == Settings.BASICDialect.Opcodes["RUN"].InsertionValue ) ) )
          {
            bool    isGotoOrGosub = ( token.ByteValue == Settings.BASICDialect.Opcodes["GOTO"].InsertionValue ) 
                                  | ( token.ByteValue == Settings.BASICDialect.Opcodes["GOSUB"].InsertionValue );
            nextTokenIndex = FindNextToken( lineInfo.Value.Tokens, tokenIndex );
            nextTokenIndex2 = FindNextToken( lineInfo.Value.Tokens, nextTokenIndex );

            while ( ( nextTokenIndex != -1 )
            &&      ( nextTokenIndex2 != -1 ) )
            {
              if ( ( lineInfo.Value.Tokens[nextTokenIndex].TokenType == Token.Type.EX_BASIC_TOKEN )
              &&   ( lineInfo.Value.Tokens[nextTokenIndex].ByteValue == Settings.BASICDialect.ExOpcodes["LABEL"].InsertionValue )
              &&   ( lineInfo.Value.Tokens[nextTokenIndex2].TokenType == Token.Type.NUMERIC_LITERAL ) )
              {
                string label = "LABEL" + lineInfo.Value.Tokens[nextTokenIndex2].Content;

                if ( !labelToNumber.ContainsKey( label ) )
                {
                  AddError( lineInfo.Value.LineIndex, Types.ErrorCode.E3004_BASIC_MISSING_LABEL, "Unknown label " + label + " encountered" );
                }
                else
                {
                  if ( !tokenIsInserted )
                  {
                    tokenIsInserted = true;
                    sb.Append( token.Content );

                    while ( nextTokenIndex - 1 > tokenIndex )
                    {
                      // there were blanks in between
                      sb.Append( lineInfo.Value.Tokens[tokenIndex + 1].Content );
                      ++tokenIndex;
                    }
                  }
                  sb.Append( labelToNumber[label].ToString() );
                }
                tokenIndex = nextTokenIndex2;
                if ( !isGotoOrGosub )
                {
                  break;
                }
                nextTokenIndex = FindNextToken( lineInfo.Value.Tokens, nextTokenIndex2 );
                if ( ( nextTokenIndex == -1 )
                ||   ( lineInfo.Value.Tokens[nextTokenIndex].Content != "," ) )
                {
                  break;
                }
                sb.Append( ',' );
                nextTokenIndex = FindNextToken( lineInfo.Value.Tokens, nextTokenIndex );
                nextTokenIndex2 = FindNextToken( lineInfo.Value.Tokens, nextTokenIndex );
              }
              else
              {
                break;
              }
            }
          }

          if ( token.TokenType == Token.Type.STRING_LITERAL )
          {
            if ( ( token.Content.StartsWith( "\"" ) )
            &&   ( token.Content.EndsWith( "\"" ) ) )
            {
              // replace value with char
              //token.Content = DecompileStringLiteral( token.Content );
            }
          }

          if ( !tokenIsInserted )
          {
            sb.Append( token.Content );
          }
        }
        sb.Append( "\r\n" );
        lineNumber += 10;
      }
      return sb.ToString();
    }



    public bool Disassemble( GR.Memory.ByteBuffer Data, out List<string> Lines  )
    {
      Lines = new List<string>();
      if ( ( Data == null )
      ||   ( Data.Length == 0 ) )
      {
        return false;
      }
      

      // Zeile =
      //  2 Bytes pointer auf nächste Zeile
      //  2 Bytes Zeilennummer
      //  x Bytes Zeileninhalt
      //  1 Byte 00 Zeilenende
      //  zum Abschluss ist der Pointer 00 00

      int     dataPos = 0;

      while ( dataPos < Data.Length )
      {
        // pointer to next line
        if ( dataPos + 2 > Data.Length )
        {
          Debug.Log( "no space for pointer" );
          return false;
        }
        if ( Data.UInt16At( dataPos ) == 0 )
        {
          // end
          Debug.Log( "end reached" );
          return true;
        }

        // TODO - check pointer?
        dataPos += 2;

        // line number
        if ( dataPos + 2 > Data.Length )
        {
          Debug.Log( "no space for line number" );
          return false;
        }
        ushort lineNumber = Data.UInt16At( dataPos );

        dataPos += 2;

        string    lineContent = lineNumber.ToString() + " ";
        bool      insideStringLiteral = false;
        bool      encounteredREM = false;

        while ( ( dataPos < Data.Length )
        &&      ( Data.ByteAt( dataPos ) != 0 ) )
        {
          byte    byteValue = Data.ByteAt( dataPos );

          if ( byteValue == 34 )
          {
            insideStringLiteral = !insideStringLiteral;
          }

          if ( encounteredREM )
          {
            if ( ( byteValue >= 192 )
            &&   ( byteValue <= 223 ) )
            {
              byteValue -= 192 - 96;
            }
            if ( ( byteValue >= 224 )
            &&   ( byteValue <= 254 ) )
            {
              byteValue -= 224 - 160;
            }
            if ( byteValue == 255 )
            {
              byteValue = 126;
            }

            // REM is only remark, no opcode parsing anymore, but only inside apostrophes!

            if ( ( !insideStringLiteral )
            &&   ( Settings.BASICDialect.OpcodesFromByte.ContainsKey( byteValue ) ) )
            {
              lineContent += Settings.BASICDialect.OpcodesFromByte[byteValue].Command;
            }
            else if ( ConstantData.PETSCIIToUnicode.ContainsKey( byteValue ) )
            {
              char charToUse = ConstantData.PETSCIIToUnicode[byteValue];
              lineContent += charToUse;
            }
            ++dataPos;
            continue;
          }

          if ( insideStringLiteral )
          {
            //if ( KeymapEntryExists( System.Windows.Forms.InputLanguage.CurrentInputLanguage, System.Windows.Forms.Keys
            // Codes 192-223 wie Codes  96-127
            // Codes 224-254 wie Codes 160-190
            // Code  255     wie Code  126
            if ( ( byteValue >= 192 )
            &&   ( byteValue <= 223 ) )
            {
              byteValue -= 192 - 96;
            }
            if ( ( byteValue >= 224 )
            &&   ( byteValue <= 254 ) )
            {
              byteValue -= 224 - 160;
            }
            if ( byteValue == 255 )
            {
              byteValue = 126;
            }

            var c64Key = ConstantData.FindC64KeyByPETSCII( byteValue );
            if ( c64Key != null )
            {
              lineContent += c64Key.CharValue;
            }
            else
            {
              Debug.Log( "Unknown byte value " + byteValue );
            }
          }
          else
          {
            bool    foundTwoByteToken = false;
            if ( !Settings.BASICDialect.OpcodesFromByte.ContainsKey( byteValue ) )
            {
              // could be a two byte token?
              if ( ( dataPos + 1 < Data.Length )
              &&   ( Data.ByteAt( dataPos + 1 ) != 0 ) )
              {
                byte    byteValue2 = Data.ByteAt( dataPos + 1 );

                ushort tokenValue = (ushort)( byteValue2 | ( byteValue << 8 ) );

                if ( Settings.BASICDialect.OpcodesFromByte.ContainsKey( tokenValue ) )
                {
                  ++dataPos;
                  lineContent += Settings.BASICDialect.OpcodesFromByte[tokenValue].Command;
                  foundTwoByteToken = true;
                }
              }
            }
            if ( !foundTwoByteToken )
            {
              if ( ( !Settings.BASICDialect.OpcodesFromByte.ContainsKey( byteValue ) )
              ||   ( Settings.BASICDialect.OpcodesFromByte[byteValue].Command.StartsWith( "{" ) ) )
              {
                //if ( KeymapEntryExists( System.Windows.Forms.InputLanguage.CurrentInputLanguage, System.Windows.Forms.Keys
                if ( ConstantData.PETSCIIToUnicode.ContainsKey( byteValue ) )
                {
                  char charToUse = ConstantData.PETSCIIToUnicode[byteValue];
                  /*
                  if ( KeyMap.KeymapEntryExists( (System.Windows.Forms.Keys)charToUse ) )
                  {
                    charToUse = KeyMap.GetKeymapEntry( (System.Windows.Forms.Keys)charToUse ).Normal;
                  }*/
                  lineContent += charToUse;
                }
                else if ( ActionTokenByByteValue.ContainsKey( byteValue ) )
                {
                  lineContent += ActionTokenByByteValue[byteValue].Replacement;
                }
                else
                {
                  Debug.Log( "Unknown byte value " + byteValue );
                }
              }
              else
              {
                if ( Settings.BASICDialect.OpcodesFromByte[byteValue].IsComment )
                {
                  encounteredREM = true;
                }
                lineContent += Settings.BASICDialect.OpcodesFromByte[byteValue].Command;
              }
            }
          }
          ++dataPos;
        }

        if ( dataPos >= Data.Length )
        {
          // reached the end, should not happen!
          Debug.Log( "reached wrong end" );
          return false;
        }
        Debug.Log( "Line:" + lineContent );
        Lines.Add( lineContent );
        ++dataPos;
      }

      return false;
    }



    public RenumberResult CanRenumber( int LineStart, int LineStep, int FirstLineNumber, int LastLineNumber )
    {
      if ( m_LineInfos.Count == 0 )
      {
        return RenumberResult.NOTHING_TO_DO;
      }
      if ( LineStart + LineStep * ( m_LineInfos.Count - 1 ) >= 64000 )
      {
        return RenumberResult.TOO_MANY_LINES;
      }
      if ( ( LineStart < 0 )
      ||   ( LineStart >= 64000 )
      ||   ( FirstLineNumber < 0 )
      ||   ( FirstLineNumber >= 64000 )
      ||   ( LastLineNumber < 0 )
      ||   ( LastLineNumber >= 64000 ) )
      {
        return RenumberResult.INVALID_VALUES;
      }
      return RenumberResult.OK;
    }



    public string Renumber( int LineStart, int LineStep, int FirstLineNumber, int LastLineNumber )
    {
      // UGLY HACK - > TODO make it clean later (as if) -> store stripping settings
      bool settingsStripSpaces = Settings.StripSpaces;
      bool settingsStripREM = Settings.StripREM;
      Settings.StripSpaces = false;
      Settings.StripREM = false;

      StringBuilder sb = new StringBuilder();

      GR.Collections.Map<int, int> lineNumberReference = new GR.Collections.Map<int, int>();

      int curLine = LineStart;
      foreach ( KeyValuePair<int, LineInfo> lineInfo in m_LineInfos )
      {
        if ( ( lineInfo.Value.LineNumber >= FirstLineNumber )
        &&   ( lineInfo.Value.LineNumber <= LastLineNumber ) )
        {
          lineNumberReference[lineInfo.Value.LineNumber] = curLine;
          curLine += LineStep;
        }
      }

      curLine = LineStart;

      int     firstLineIndex = -1;
      foreach ( KeyValuePair<int,LineInfo> lineInfoOrig in m_LineInfos )
      {
        ++firstLineIndex;
        // keep empty lines
        while ( firstLineIndex < lineInfoOrig.Key )
        {
          sb.Append( "\r\n" );
          ++firstLineIndex;
        }
        //var lineInfo = PureTokenizeLine( lineInfoOrig.Value.Line, lineInfoOrig.Value.LineNumber );
        var lineInfo = TokenizeLine( lineInfoOrig.Value.Line, 0, ref curLine );
        for ( int i = 0; i < lineInfo.Tokens.Count; ++i )
        {
          Token token = lineInfo.Tokens[i];
          if ( token.TokenType == Token.Type.LINE_NUMBER )
          {
            // replace starting line number
            if ( ( lineInfo.LineNumber >= FirstLineNumber )
            &&   ( lineInfo.LineNumber <= LastLineNumber ) )
            {
              sb.Append( lineNumberReference[lineInfo.LineNumber] );
            }
            else
            {
              sb.Append( lineInfo.LineNumber );
            }
            if ( ( i == 0 )
            &&   ( i + 1 < lineInfo.Tokens.Count ) )
            {
              if ( token.StartIndex + token.Content.Length < lineInfo.Tokens[i + 1].StartIndex )
              {
                // keep spaces after line numbers
                for ( int j = 0; j < lineInfo.Tokens[i + 1].StartIndex - ( token.StartIndex + token.Content.Length ); ++j )
                {
                  sb.Append( ' ' );
                }
              }
            }
            continue;
          }
          if ( token.TokenType == Token.Type.BASIC_TOKEN )
          {
            if ( ( token.ByteValue == Settings.BASICDialect.Opcodes["RUN"].InsertionValue )
            ||   ( token.ByteValue == Settings.BASICDialect.Opcodes["THEN"].InsertionValue ) )
            {
              // insert label instead of line number
              if ( i + 1 < lineInfo.Tokens.Count )
              {
                int     refNo = -1;
                int     nextTokenIndex = FindNextToken( lineInfo.Tokens, i );
                if ( ( nextTokenIndex != -1 )
                &&   ( int.TryParse( lineInfo.Tokens[nextTokenIndex].Content, out refNo ) ) )
                {
                  sb.Append( token.Content );

                  while ( i + 1 < nextTokenIndex )
                  {
                    sb.Append( lineInfo.Tokens[i + 1].Content );
                    ++i;
                  }
                  if ( ( refNo >= FirstLineNumber )
                  &&   ( refNo <= LastLineNumber ) )
                  {
                    sb.Append( lineNumberReference[refNo] );
                  }
                  else
                  {
                    sb.Append( refNo );
                  }
                  ++i;
                  continue;
                }
              }
            }
            if ( ( token.ByteValue == Settings.BASICDialect.Opcodes["GOTO"].InsertionValue )
            ||   ( token.ByteValue == Settings.BASICDialect.Opcodes["GOSUB"].InsertionValue ) )
            {
              // ON x GOTO/GOSUB can have more than one line number
              // insert label instead of line number
              sb.Append( token.Content );
              int nextIndex = i + 1;
              bool mustBeComma = false;
              while ( nextIndex < lineInfo.Tokens.Count )
              {
                Token nextToken = lineInfo.Tokens[nextIndex];
                if ( !mustBeComma )
                {
                  if ( nextToken.TokenType == Token.Type.NUMERIC_LITERAL )
                  {
                    // numeric!
                    int refNo = GR.Convert.ToI32( nextToken.Content );
                    if ( ( refNo >= FirstLineNumber )
                    &&   ( refNo <= LastLineNumber )
                    &&   ( lineNumberReference.ContainsKey( refNo ) ) )
                    {
                      sb.Append( lineNumberReference[refNo].ToString() );
                    }
                    else
                    {
                      // no reference found, keep old value
                      sb.Append( nextToken.Content );
                    }
                    ++nextIndex;
                  }
                  else if ( ( nextToken.TokenType == Token.Type.DIRECT_TOKEN )
                  &&        ( nextToken.Content == " " ) )
                  {
                    // space is valid, skip
                    ++nextIndex;
                    continue;
                  }
                  else
                  {
                    // error or end
                    break;
                  }
                  mustBeComma = true;
                }
                else
                {
                  if ( ( nextToken.TokenType != Token.Type.DIRECT_TOKEN )
                  ||   ( nextToken.ByteValue != ',' ) )
                  {
                    // error or end, not a comma
                    break;
                  }
                  sb.Append( nextToken.Content );
                  mustBeComma = false;
                  ++nextIndex;
                }
              }
              i = nextIndex - 1;
              continue;
            }
          }
          // if we got here there was no label inserted
          sb.Append( token.Content );
          if ( ( token.TokenType == Token.Type.BASIC_TOKEN )
          ||   ( token.TokenType == Token.Type.EX_BASIC_TOKEN ) )
          {
            if ( !IsComment( token ) )
            {
              //sb.Append( " " );
            }
          }
        }
        sb.Append( "\r\n" );
        //sb.Append( lineInfo.Value.Line + "\r\n" );
        // TODO - replace goto/numbers with label
      }

      // strip last line break
      if ( m_LineInfos.Count > 0 )
      {
        sb.Remove( sb.Length - 2, 2 );
      }

      // UGLY HACK part #2, restore settings
      Settings.StripSpaces  = settingsStripSpaces;
      Settings.StripREM     = settingsStripREM;
      return sb.ToString();
    }



    public string ReplaceAllSymbolsByMacros( string BasicText )
    {
      for ( int i = 0; i < BasicText.Length; ++i )
      {
        char    chartoCheck = BasicText[i];

        if ( chartoCheck > (char)255 )
        {
          var c64Key = ConstantData.FindC64KeyByUnicode( chartoCheck );
          if ( c64Key != null )
          {
            if ( c64Key.Replacements.Count > 0 )
            {
              int     numCharsToReplace = 1;
              int     testPos = i;
              while ( ( testPos + 1 < BasicText.Length )
              &&      ( BasicText[testPos + 1] == chartoCheck ) )
              {
                ++testPos;
              }
              numCharsToReplace = testPos - i + 1;

              string    replacement = c64Key.Replacements[0];
              if ( numCharsToReplace > 1 )
              {
                // numeric representation is "<count><Macro>"
                replacement = numCharsToReplace.ToString() + replacement;
              }


              BasicText = BasicText.Substring( 0, i ) + "{" + replacement + "}" + BasicText.Substring( i + numCharsToReplace );
              i += replacement.Length - numCharsToReplace;
            }
          }
        }
      }
      return BasicText;
    }



    public static string ReplaceAllMacrosBySymbols( string BasicText, out bool HadError )
    {
      StringBuilder     sb = new StringBuilder();

      int               posInLine = 0;
      int               macroStartPos = 0;
      bool              insideMacro = false;

      HadError = false;

      while ( posInLine < BasicText.Length )
      {
        char    curChar = BasicText[posInLine];
        if ( insideMacro )
        {
          if ( curChar == '}' )
          {
            insideMacro = false;

            string macro = BasicText.Substring( macroStartPos + 1, posInLine - macroStartPos - 1 ).ToUpper();
            int macroCount = 1;

            macro = Parser.BasicFileParser.DetermineMacroCount( macro, out macroCount );

            bool  foundMacro = false;
            foreach ( var key in ConstantData.AllPhysicalKeyInfos )
            {
              if ( key.Replacements.Contains( macro ) )
              {
                for ( int i = 0; i < macroCount; ++i )
                {
                  sb.Append( key.CharValue );
                }
                foundMacro = true;
                break;
              }
            }
            if ( !foundMacro )
            {
              //Debug.Log( "Unknown macro " + macro );
              HadError = true;
              return null;
            }
          }
          ++posInLine;
          continue;
        }
        if ( curChar == '{' )
        {
          insideMacro = true;
          macroStartPos = posInLine;
          ++posInLine;
          continue;
        }
        // normal chars are passed on (also tabs, cr, lf)
        sb.Append( curChar );
        ++posInLine;
      }
      return sb.ToString();
    }



    public static string ReplaceAllMacrosByPETSCIICode( string BasicText, GR.Collections.Map<byte, byte> CustomMapping, out bool HadError )
    {
      StringBuilder     sb = new StringBuilder();

      int               posInLine = 0;
      int               macroStartPos = 0;
      bool              insideMacro = false;

      HadError = false;

      while ( posInLine < BasicText.Length )
      {
        char    curChar = BasicText[posInLine];
        if ( insideMacro )
        {
          if ( curChar == '}' )
          {
            insideMacro = false;

            string macro = BasicText.Substring( macroStartPos + 1, posInLine - macroStartPos - 1 ).ToUpper();
            int macroCount = 1;

            macro = Parser.BasicFileParser.DetermineMacroCount( macro, out macroCount );

            bool  foundMacro = false;
            foreach ( var key in ConstantData.AllPhysicalKeyInfos )
            {
              if ( key.Replacements.Contains( macro ) )
              {
                for ( int i = 0; i < macroCount; ++i )
                {
                  sb.Append( (char)key.PetSCIIValue );
                }
                foundMacro = true;
                break;
              }
            }
            if ( !foundMacro )
            {
              //Debug.Log( "Unknown macro " + macro );
              HadError = true;
              return null;
            }
          }
          ++posInLine;
          continue;
        }
        if ( curChar == '{' )
        {
          insideMacro = true;
          macroStartPos = posInLine;
          ++posInLine;
          continue;
        }
        // normal chars are passed on (also tabs, cr, lf)
        if ( CustomMapping.ContainsKey( (byte)curChar ) )
        {
          sb.Append( (char)CustomMapping[(byte)curChar] );
        }
        else
        {
          sb.Append( curChar );
        }
        ++posInLine;
      }
      return sb.ToString();
    }



    public static string MakeUpperCase( string BASICText, bool C64Font )
    {
      StringBuilder   sb = new StringBuilder( BASICText.Length );

      foreach ( var singleChar in BASICText )
      {
        if ( C64Font )
        {
          if ( ( singleChar & 0xff00 ) == 0xef00 )
          {
            char    newChar = (char)( ( singleChar & 0x00ff ) | 0xee00 );
            if ( ( newChar >= 0xee01 )
            &&   ( newChar <= 0xee01 + 25 ) )
            {
              sb.Append( (char)( newChar - 0xee01 + 'A' ) );
            }
            else
            {
              sb.Append( newChar );
            }
          }
          else if ( ( singleChar >= 0x61 )
          && ( singleChar <= 0x7a ) )
          {
            sb.Append( (char)( singleChar - 0x20 ) );
          }
          else
          {
            sb.Append( singleChar );
          }
        }
        else
        {
          if ( ( singleChar >= 'A' )
          &&   ( singleChar <= 'Z' ) )
          {
            sb.Append( (char)( singleChar + 'a' - 'A' ) );
          }
          else if ( ( singleChar >= 'a' )
          &&        ( singleChar <= 'z' ) )
          {
            sb.Append( (char)( singleChar + 'A' - 'a' ) );
          }
          else
          {
            sb.Append( singleChar );
          }
        }
      }
      return sb.ToString();
    }



    public static string MakeLowerCase( string BASICText, bool C64Font )
    {
      StringBuilder   sb = new StringBuilder( BASICText.Length );

      foreach ( var singleChar in BASICText )
      {
        if ( C64Font )
        {
          if ( ( singleChar & 0xff00 ) == 0xee00 )
          {
            char    newChar = (char)( ( singleChar & 0x00ff ) | 0xef00 );
            if ( ( newChar >= 'A' )
            &&   ( newChar <= 'Z' ) )
            {
              sb.Append( (char)( newChar + 0xef01 - 'A' ) );
            }
            else
            {
              sb.Append( newChar );
            }
          }
          else if ( ( singleChar >= 'A' )
          &&        ( singleChar <= 'Z' ) )
          {
            sb.Append( (char)( singleChar + 0xef01 - 'A' ) );
          }
          else
          {
            sb.Append( singleChar );
          }
        }
        else
        {
          if ( ( singleChar >= 'A' )
          &&   ( singleChar <= 'Z' ) )
          {
            sb.Append( (char)( singleChar + 'a' - 'A' ) );
          }
          else if ( ( singleChar >= 'a' )
          &&        ( singleChar <= 'z' ) )
          {
            sb.Append( (char)( singleChar + 'A' - 'a' ) );
          }
          else
          {
            sb.Append( singleChar );
          }
        }
      }
      return sb.ToString();
    }



  }
}
