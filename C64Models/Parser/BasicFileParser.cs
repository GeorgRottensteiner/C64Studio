using System;
using System.Collections.Generic;
using System.Text;
using GR.Memory;

namespace C64Studio.Parser
{
  public class BasicFileParser : ParserBase
  {
    public enum BasicVersion
    {
      C64_BASIC_V2 = 0, //  Anpassung des VIC BASIC V2 für VC10, C64, C128 (C64-Modus), SX64, PET 64.
      V1_0,             // Version 1.0	Noch recht fehlerbehaftet, erste Versionen im PET 2001
      V2_0,             // August  Version 2.0	Fehlerkorrekturen, eingebaut in weitere Versionen des PET
      V3_0,             // Version 3.0	Leichte Fehlerkorrekturen und Integration des Maschinensprachemonitors TIM, schnelle Garbage Collection. CBM 3000 Serie (Standard) und PET 2001 (angezeigte Versionsnummer dort ist allerdings noch 2).
      V4_0,             // Version 4.0	Neue Befehle für leichtere Handhabung für Diskettenlaufwerke für CBM 4000. Auch durch ROM-Austausch für CBM 3000 Serie und PET 2001 verfügbar.
      V4_1,             // Version 4.1	Fehlerkorrigierte Fassung der Version 4.0 mit erweiterter Bildschirmeditor für CBM 8000. Auch durch ROM-Austausch für CBM 3000/4000 Serie verfügbar.
      V4_2,             // Version 4.2	Geänderte und ergänzte Befehle für den CBM 8096.
      VIC_BASIC_V2,     //  Funktionsumfang von V 2.0 mit Korrekturen aus der Version-4-Linie. Einsatz im VC20.
      V4_PLUS,          // (intern bis 4.75)	Neue Befehle und RAM-Unterstützung bis 256 KByte für CBM-II-Serie (CBM 500, 6x0, 7x0). Fortsetzung und Ende der Version-4-Linie.
      V3_5,             // Version 3.5	Neue Befehle für die Heimcomputer C16/116 und Plus/4. Zusammenführung aus C64 BASIC V2 und Version-4-Linie.
      V3_6,             // Version 3.6	Neue Befehle für LCD-Prototypen.
      V7_0,             // Version 7.0	Neue Befehle für den C128/D/DCR. Weiterentwicklung des C16/116 BASIC 3.5 .
      V10_0,            // Version 10 Neue Befehle für C65, beinhaltet sehr viele Fehler, kam aus dem Entwicklungsstadium nicht heraus. Weiterentwicklung des BASIC 7.0.
      BASIC_LIGHTNING,  // BASIC extension
      LASER_BASIC,      // BASIC extension
      SIMONS_BASIC,     // Simons Basic
      SIMONS_BASIC_2    // Simons Basic Extended (SBX)
    }

    public class ParserSettings
    {
      public bool         StripSpaces = true;
      public bool         StripREM = false;
      public BasicVersion Version = BasicVersion.C64_BASIC_V2;
      public bool         UpperCaseMode = true;
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

    internal class Token
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
        VARIABLE
      };
      public Type       TokenType = Type.DIRECT_TOKEN;
      public string     Content = "";
      public int        ByteValue = 0;
      public int        StartIndex = 0;
    };


    public class Opcode
    {
      public string         Command = "";
      public int            InsertionValue = -1;
      public string         ShortCut = null;

      public Opcode( string Command, int InsertionValue )
      {
        this.Command        = Command;
        this.InsertionValue = InsertionValue;
      }

      public Opcode( string Command, int InsertionValue, string ShortCut )
      {
        this.Command        = Command;
        this.InsertionValue = InsertionValue;
        this.ShortCut       = ShortCut;
      }
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

    public Dictionary<string, Opcode>     m_Opcodes = new Dictionary<string, Opcode>();
    public SortedDictionary<ushort, Opcode> m_OpcodesFromByte = new SortedDictionary<ushort, Opcode>();
    public Dictionary<string, Opcode>     m_ExOpcodes = new Dictionary<string, Opcode>();

    private GR.Collections.Map<int, LineInfo> m_LineInfos = new GR.Collections.Map<int, LineInfo>();

    public GR.Collections.Map<string, ActionToken>     ActionTokens = new GR.Collections.Map<string, ActionToken>();
    public GR.Collections.Map<TokenValue, ActionToken> ActionTokenByValue = new GR.Collections.Map<TokenValue, ActionToken>();
    public GR.Collections.Map<byte, ActionToken>       ActionTokenByByteValue = new GR.Collections.Map<byte,ActionToken>();
    public ParserSettings       Settings = new ParserSettings();

    public Types.ASM.FileInfo           ASMFileInfo = new C64Studio.Types.ASM.FileInfo();
    public Types.ASM.FileInfo           InitialFileInfo = null;



    public BasicFileParser( ParserSettings Settings )
    {
      LabelMode = false;
      this.Settings = Settings;
      SetBasicVersion( Settings.Version );
    }



    public BasicFileParser( ParserSettings Settings, string Filename )
    {
      this.Settings = Settings;
      SetBasicVersion( Settings.Version );
      LabelMode   = false;
      m_Filename  = Filename;
    }



    private void AddOpcode( string Opcode, int ByteValue )
    {
      var opcode = new Opcode( Opcode, ByteValue );
      m_Opcodes[Opcode] = opcode;
      m_OpcodesFromByte[(ushort)ByteValue] = opcode;
    }



    private void AddOpcode( string Opcode, int ByteValue, string ShortCut )
    {
      var opcode = new Opcode( Opcode, ByteValue, ShortCut );
      m_Opcodes[Opcode] = opcode;
      m_OpcodesFromByte[(ushort)ByteValue] = opcode;
    }



    private void AddExOpcode( string Opcode, int ByteValue )
    {
      m_ExOpcodes[Opcode] = new Opcode( Opcode, ByteValue );
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



    public void SetBasicVersion( BasicVersion Version )
    {
      ActionTokens.Clear();
      ActionTokenByByteValue.Clear();
      ActionTokenByValue.Clear();
      m_ExOpcodes.Clear();
      m_Opcodes.Clear();
      m_OpcodesFromByte.Clear();

      if ( ( Version == BasicVersion.C64_BASIC_V2 )
      ||   ( Version == BasicVersion.VIC_BASIC_V2 )
      ||   ( Version == BasicVersion.SIMONS_BASIC )
      ||   ( Version == BasicVersion.SIMONS_BASIC_2 )
      ||   ( Version == BasicVersion.BASIC_LIGHTNING )
      ||   ( Version == BasicVersion.LASER_BASIC )
      ||   ( Version == BasicVersion.V3_5 )
      ||   ( Version == BasicVersion.V7_0 ) )
      {
        // default BASIC V2 tokens
        AddOpcode( "END", 0x80, "eN" );
        AddOpcode( "FOR", 0x81, "fO" );
        AddOpcode( "NEXT", 0x82, "nE" );
        AddOpcode( "DATA", 0x83, "dA" );
        AddOpcode( "INPUT#", 0x84, "iN" );
        AddOpcode( "INPUT", 0x85 );
        AddOpcode( "DIM", 0x86, "dI" );
        AddOpcode( "READ", 0x87, "rE" );
        AddOpcode( "LET", 0x88, "lE" );
        AddOpcode( "GOTO", 0x89, "gO" );
        AddOpcode( "RUN", 0x8A, "rU" );
        AddOpcode( "IF", 0x8B );
        AddOpcode( "RESTORE", 0x8C, "reS" );
        AddOpcode( "GOSUB", 0x8D, "goS" );
        AddOpcode( "RETURN", 0x8E, "reT" );
        AddOpcode( "REM", 0x8F );
        AddOpcode( "STOP", 0x90, "sT" );
        AddOpcode( "ON", 0x91 );
        AddOpcode( "WAIT", 0x92, "wA" );
        AddOpcode( "LOAD", 0x93, "lO" );
        AddOpcode( "SAVE", 0x94, "sA" );
        AddOpcode( "VERIFY", 0x95, "vE" );
        AddOpcode( "DEF", 0x96, "dE" );
        AddOpcode( "POKE", 0x97, "pO" );
        AddOpcode( "PRINT#", 0x98, "pR" );
        AddOpcode( "?", 0x99 );
        AddOpcode( "PRINT", 0x99 );
        AddOpcode( "CONT", 0x9A, "cO" );
        AddOpcode( "LIST", 0x9B, "lI" );
        AddOpcode( "CLR", 0x9C, "cL" );
        AddOpcode( "CMD", 0x9D, "cM" );
        AddOpcode( "SYS", 0x9E, "sY" );
        AddOpcode( "OPEN", 0x9F, "oP" );
        AddOpcode( "CLOSE", 0xA0, "clO" );
        AddOpcode( "GET", 0xA1, "gE" );
        AddOpcode( "NEW", 0xA2 );
        AddOpcode( "TAB(", 0xA3, "tA" );
        AddOpcode( "TO", 0xA4 );
        AddOpcode( "FN", 0xA5 );
        AddOpcode( "SPC(", 0xA6, "sP" );
        AddOpcode( "THEN", 0xA7, "tH" );
        AddOpcode( "NOT", 0xA8, "nO" );
        AddOpcode( "STEP", 0xA9, "stE" );
        AddOpcode( "+", 0xAA );
        AddOpcode( "-", 0xAB );
        AddOpcode( "*", 0xAC );
        AddOpcode( "/", 0xAD );
        //AddOpcode( "" + (char)0xee1e, 0xAE );
        AddOpcode( "^", 0xAE );
        AddOpcode( "AND", 0xAF, "aN" );
        AddOpcode( "OR", 0xB0 );
        AddOpcode( ">", 0xB1 );
        AddOpcode( "=", 0xB2 );
        AddOpcode( "<", 0xB3 );
        AddOpcode( "SGN", 0xB4, "sG" );
        AddOpcode( "INT", 0xB5 );
        AddOpcode( "ABS", 0xB6, "aB" );
        AddOpcode( "USR", 0xB7, "uS" );
        AddOpcode( "FRE", 0xB8, "fE" );
        AddOpcode( "POS", 0xB9 );
        AddOpcode( "SQR", 0xBA, "sQ" );
        AddOpcode( "RND", 0xBB, "rN" );
        AddOpcode( "LOG", 0xBC );
        AddOpcode( "EXP", 0xBD, "eX" );
        AddOpcode( "COS", 0xBE );
        AddOpcode( "SIN", 0xBF, "sI" );
        AddOpcode( "TAN", 0xC0 );
        AddOpcode( "ATN", 0xC1, "aT" );
        AddOpcode( "PEEK", 0xC2, "pE" );
        AddOpcode( "LEN", 0xC3 );
        AddOpcode( "STR$", 0xC4, "stR" );
        AddOpcode( "VAL", 0xC5, "vA" );
        AddOpcode( "ASC", 0xC6, "aS" );
        AddOpcode( "CHR$", 0xC7, "cH" );
        AddOpcode( "LEFT$", 0xC8, "leF" );
        AddOpcode( "RIGHT$", 0xC9, "rI" );
        AddOpcode( "MID$", 0xCA, "mI" );
        //AddOpcode( "GO", 0xCB );

        // C64Studio extension
        AddExOpcode( "LABEL", 0xF0 );
      }

      if ( ( Version == BasicVersion.V3_5 )
      ||   ( Version == BasicVersion.V7_0 ) )
      {
        AddOpcode( "RGR", 0xcc );
        AddOpcode( "RCLR", 0xcd );
        AddOpcode( "RLUM", 0xce );
        AddOpcode( "JOY", 0xcf );
        AddOpcode( "RDOT", 0xd0 );
        AddOpcode( "DEC", 0xd1 );
        AddOpcode( "HEX$", 0xd2 );
        AddOpcode( "ERR$", 0xd3 );
        AddOpcode( "INSTR", 0xd4 );
        AddOpcode( "ELSE", 0xd5 );
        AddOpcode( "RESUME", 0xd6 );
        AddOpcode( "TRAP", 0xd7 );
        AddOpcode( "TRON", 0xd8 );
        AddOpcode( "TROFF", 0xd9 );
        AddOpcode( "SOUND", 0xda );
        AddOpcode( "VOL", 0xdb );
        AddOpcode( "AUTO", 0xdc );
        AddOpcode( "PUDEF", 0xdd );
        AddOpcode( "GRAPHIC", 0xde );
        AddOpcode( "PAINT", 0xdf );
        AddOpcode( "CHAR", 0xe0 );
        AddOpcode( "BOX", 0xe1 );
        AddOpcode( "CIRCLE", 0xe2 );
        AddOpcode( "GSHAPE", 0xe3 );
        AddOpcode( "SSHAPE", 0xe4 );
        AddOpcode( "DRAW", 0xe5 );
        AddOpcode( "LOCATE", 0xe6 );
        AddOpcode( "COLOR", 0xe7 );
        AddOpcode( "SCNCLR", 0xe8 );
        AddOpcode( "SCALE", 0xe9 );
        AddOpcode( "HELP", 0xea );
        AddOpcode( "DO", 0xeb );
        AddOpcode( "LOOP", 0xec );
        AddOpcode( "EXIT", 0xed );
        AddOpcode( "DIRECTORY", 0xee );
        AddOpcode( "DSAVE", 0xef );
        AddOpcode( "DLOAD", 0xf0 );
        AddOpcode( "HEADER", 0xf1 );
        AddOpcode( "SCRATCH", 0xf2 );
        AddOpcode( "COLLECT", 0xf3 );
        AddOpcode( "COPY", 0xf4 );
        AddOpcode( "RENAME", 0xf5 );
        AddOpcode( "BACKUP", 0xf6 );
        AddOpcode( "DELETE", 0xf7 );
        AddOpcode( "RENUMBER", 0xf8 );
        AddOpcode( "KEY", 0xf9 );
        AddOpcode( "MONITOR", 0xfa );
        AddOpcode( "USING", 0xfb );
        AddOpcode( "UNTIL", 0xfc );
        AddOpcode( "WHILE", 0xfd );
      }

      if ( Version == BasicVersion.V7_0 )
      {
        // override without or different short cuts
        AddOpcode( "END", 0x80 );
        AddOpcode( "FOR", 0x81 );
        AddOpcode( "NEXT", 0x82 );
        AddOpcode( "READ", 0x87, "reA" );
        AddOpcode( "STOP", 0x90, "stO" );
        AddOpcode( "POKE", 0x97, "poK" );
        AddOpcode( "CONT", 0x9A );
        AddOpcode( "SYS", 0x9E );
        AddOpcode( "SPC(", 0xA6 );
        AddOpcode( "PEEK", 0xC2, "peE" );

        m_Opcodes.Remove( "RLUM" );
        m_OpcodesFromByte.Remove( 0xce );


        // new commands
        AddOpcode( "POT", 0xce02 );
        AddOpcode( "BUMP", 0xce03 );
        AddOpcode( "PEN", 0xce04 );
        AddOpcode( "RSPPOS", 0xce05 );
        AddOpcode( "RSPRITE", 0xce06 );
        AddOpcode( "RSPCOLOR", 0xce07 );
        AddOpcode( "XOR", 0xce08 );
        AddOpcode( "RWINDOW", 0xce09 );
        AddOpcode( "POINTER", 0xce0a );

        AddOpcode( "BANK", 0xfe02 );
        AddOpcode( "FILTER", 0xfe03 );
        AddOpcode( "PLAY", 0xfe04 );
        AddOpcode( "TEMPO", 0xfe05 );
        AddOpcode( "MOVSPR", 0xfe06 );
        AddOpcode( "SPRITE", 0xfe07 );
        AddOpcode( "SPRCOLOR", 0xfe08 );
        AddOpcode( "RREG", 0xfe09 );
        AddOpcode( "ENVELOPE", 0xfe0a );
        AddOpcode( "SLEEP", 0xfe0b );
        AddOpcode( "CATALOG", 0xfe0c );
        AddOpcode( "DOPEN", 0xfe0d );
        AddOpcode( "APPEND", 0xfe0e );
        AddOpcode( "DCLOSE", 0xfe0f );
        AddOpcode( "BSAVE", 0xfe10 );
        AddOpcode( "BLOAD", 0xfe11 );
        AddOpcode( "RECORD", 0xfe12 );
        AddOpcode( "CONCAT", 0xfe13 );
        AddOpcode( "DVERIFY", 0xfe14 );
        AddOpcode( "DCLEAR", 0xfe15 );
        AddOpcode( "SPRSAV", 0xfe16 );
        AddOpcode( "COLLISION", 0xfe17 );
        AddOpcode( "BEGIN", 0xfe18 );
        AddOpcode( "BEND", 0xfe19 );
        AddOpcode( "WINDOW", 0xfe1a );
        AddOpcode( "BOOT", 0xfe1b );
        AddOpcode( "WIDTH", 0xfe1c );
        AddOpcode( "SPRDEF", 0xfe1d );
        AddOpcode( "QUIT", 0xfe1e );
        AddOpcode( "STASH", 0xfe1f );
        AddOpcode( "FETCH", 0xfe21 );
        AddOpcode( "SWAP", 0xfe23 );
        AddOpcode( "OFF", 0xfe24 );
        AddOpcode( "FAST", 0xfe25 );
        AddOpcode( "SLOW", 0xfe26 );
      }

      if ( ( Version == BasicVersion.LASER_BASIC )
      ||   ( Version == BasicVersion.BASIC_LIGHTNING ) )
      {
        // override BASIC V2 without short cuts
        AddOpcode( "END", 0x80 );
        AddOpcode( "E.", 0x80 );
        AddOpcode( "FOR", 0x81 );
        AddOpcode( "F.", 0x81 );
        AddOpcode( "NEXT", 0x82 );
        AddOpcode( "N.", 0x82 );
        AddOpcode( "DATA", 0x83 );
        AddOpcode( "D.", 0x83 );
        AddOpcode( "INPUT#", 0x84 );
        AddOpcode( "I.", 0x84 );
        AddOpcode( "INPUT", 0x85 );
        AddOpcode( "DIM", 0x86 );
        AddOpcode( "DI.", 0x86 );
        AddOpcode( "READ", 0x87 );
        AddOpcode( "R.", 0x87 );
        AddOpcode( "LET", 0x88 );
        AddOpcode( "L.", 0x88 );
        AddOpcode( "GOTO", 0x89 );
        AddOpcode( "G.", 0x89 );
        AddOpcode( "RUN", 0x8A );
        AddOpcode( "RU.", 0x8A );
        AddOpcode( "IF", 0x8B );
        AddOpcode( "RESTORE", 0x8C );
        AddOpcode( "RES.", 0x8C );
        AddOpcode( "GOSUB", 0x8D );
        AddOpcode( "GOS.", 0x8D );
        AddOpcode( "RETURN", 0x8E );
        AddOpcode( "RET.", 0x8E );
        AddOpcode( "REM", 0x8F );
        AddOpcode( "STOP", 0x90 );
        AddOpcode( "S.", 0x90 );
        AddOpcode( "ON", 0x91 );
        AddOpcode( "O.", 0x91 );
        AddOpcode( "WAIT", 0x92 );
        AddOpcode( "LOAD", 0x93 );
        AddOpcode( "LO.", 0x93 );
        AddOpcode( "SAVE", 0x94 );
        AddOpcode( "SA.", 0x94 );
        AddOpcode( "VERIFY", 0x95 );
        AddOpcode( "V.", 0x95 );
        AddOpcode( "DEF", 0x96 );
        AddOpcode( "DE.", 0x96 );
        AddOpcode( "POKE", 0x97 );
        AddOpcode( "P.", 0x97 );
        AddOpcode( "PRINT#", 0x98 );
        AddOpcode( "PR.", 0x98 );
        AddOpcode( "?", 0x99 );
        AddOpcode( "PRINT", 0x99 );
        AddOpcode( "CONT", 0x9A );
        AddOpcode( "C.", 0x9A );
        AddOpcode( "LIST", 0x9B );
        AddOpcode( "LI.", 0x9B );
        AddOpcode( "CLR", 0x9C );
        AddOpcode( "CL.", 0x9C );
        AddOpcode( "CMD", 0x9D );
        AddOpcode( "CM.", 0x9D );
        AddOpcode( "SYS", 0x9E );
        AddOpcode( "SY.", 0x9E );
        AddOpcode( "OPEN", 0x9F );
        AddOpcode( "OP.", 0x9F );
        AddOpcode( "CLOSE", 0xA0 );
        AddOpcode( "CLO.", 0xA0 );
        AddOpcode( "GET", 0xA1 );
        AddOpcode( "GE.", 0xA1 );
        AddOpcode( "NEW", 0xA2 );
        AddOpcode( "TAB(", 0xA3 );
        AddOpcode( "T.", 0xA3 );
        AddOpcode( "TO", 0xA4 );
        AddOpcode( "FN", 0xA5 );
        AddOpcode( "SPC(", 0xA6 );
        AddOpcode( "SP.", 0xA6 );
        AddOpcode( "THEN", 0xA7 );
        AddOpcode( "TH.", 0xA7 );
        AddOpcode( "NOT", 0xA8 );
        AddOpcode( "NO.", 0xA8 );
        AddOpcode( "STEP", 0xA9 );
        AddOpcode( "S.", 0xA9 );
        AddOpcode( "+", 0xAA );
        AddOpcode( "-", 0xAB );
        AddOpcode( "*", 0xAC );
        AddOpcode( "/", 0xAD );
        //AddOpcode( "" + (char)0xee1e, 0xAE );
        AddOpcode( "^", 0xAE );
        AddOpcode( "AND", 0xAF );
        AddOpcode( "A.", 0xAF );
        AddOpcode( "OR", 0xB0 );
        AddOpcode( ">", 0xB1 );
        AddOpcode( "=", 0xB2 );
        AddOpcode( "<", 0xB3 );
        AddOpcode( "SGN", 0xB4 );
        AddOpcode( "SG.", 0xB4 );
        AddOpcode( "INT", 0xB5 );
        AddOpcode( "ABS", 0xB6 );
        AddOpcode( "AB.", 0xB6 );
        AddOpcode( "USR", 0xB7 );
        AddOpcode( "U.", 0xB7 );
        AddOpcode( "FRE", 0xB8 );
        AddOpcode( "FR.", 0xB8 );
        AddOpcode( "POS", 0xB9 );
        AddOpcode( "SQR", 0xBA );
        AddOpcode( "SQ.", 0xBA );
        AddOpcode( "RND", 0xBB );
        AddOpcode( "RN.", 0xBB );
        AddOpcode( "LOG", 0xBC );
        AddOpcode( "EXP", 0xBD );
        AddOpcode( "EX.", 0xBD );
        AddOpcode( "COS", 0xBE );
        AddOpcode( "SIN", 0xBF );
        AddOpcode( "SI.", 0xBF );
        AddOpcode( "TAN", 0xC0 );
        AddOpcode( "ATN", 0xC1 );
        AddOpcode( "AT.", 0xC1 );
        AddOpcode( "PEEK", 0xC2 );
        AddOpcode( "PE.", 0xC2 );
        AddOpcode( "LEN", 0xC3 );
        AddOpcode( "STR$", 0xC4 );
        AddOpcode( "STR.", 0xC4 );
        AddOpcode( "VAL", 0xC5 );
        AddOpcode( "VA.", 0xC5 );
        AddOpcode( "ASC", 0xC6 );
        AddOpcode( "AS.", 0xC6 );
        AddOpcode( "CHR$", 0xC7 );
        AddOpcode( "CH.", 0xC7 );
        AddOpcode( "LEFT$", 0xC8 );
        AddOpcode( "LEF.", 0xC8 );
        AddOpcode( "RIGHT$", 0xC9 );
        AddOpcode( "RI.", 0xC9 );
        AddOpcode( "MID$", 0xCA );
        AddOpcode( "M.", 0xCA );
        AddOpcode( "GO", 0xCB );

        // override without or different short cuts
        AddOpcode( "ELSE", 0xcc );
        AddOpcode( "EL.", 0xcc );
        AddOpcode( "HEX$", 0xcd );
        AddOpcode( "H.", 0xcd );
        AddOpcode( "DEEK", 0xce );
        AddOpcode( "DEE.", 0xce );
        AddOpcode( "TRUE", 0xcf );
        AddOpcode( "TR.", 0xcf );
        AddOpcode( "IMPORT", 0xd0 );
        AddOpcode( "IM.", 0xd0 );
        AddOpcode( "CFN", 0xd1 );
        AddOpcode( "CF.", 0xd1 );
        AddOpcode( "SIZE", 0xd2 );
        AddOpcode( "SIZ.", 0xd2 );
        AddOpcode( "FALSE", 0xd3 );
        AddOpcode( "FA.", 0xd3 );
        AddOpcode( "SFRE", 0xd4 );
        AddOpcode( "SF.", 0xd4 );
        AddOpcode( "LPX", 0xd5 );
        AddOpcode( "LP.", 0xd5 );
        AddOpcode( "LPY", 0xd6 );
        AddOpcode( "COMMON%", 0xd7 );
        AddOpcode( "COM.", 0xd7 );
        AddOpcode( "CROW", 0xd8 );
        AddOpcode( "CR.", 0xd8 );
        AddOpcode( "CCOL", 0xd9 );
        AddOpcode( "CC.", 0xd9 );
        AddOpcode( "ATR", 0xda );
        AddOpcode( "INC", 0xdb );
        AddOpcode( "NUM", 0xdc );
        AddOpcode( "NU.", 0xdc );
        AddOpcode( "ROW2", 0xdd );
        AddOpcode( "RO.", 0xdd );
        AddOpcode( "COL2", 0xde );
        AddOpcode( "COL.", 0xde );
        AddOpcode( "SPN2", 0xdf );
        AddOpcode( "SPN.", 0xdf );
        AddOpcode( "HGT", 0xe0 );
        AddOpcode( "HG.", 0xe0 );
        AddOpcode( "WID", 0xe1 );
        AddOpcode( "WI.", 0xe1 );
        AddOpcode( "ROW", 0xe2 );
        AddOpcode( "COL", 0xe3 );
        AddOpcode( "SPN", 0xe4 );
        AddOpcode( "TASK", 0xe5 );
        AddOpcode( "TAS.", 0xe5 );
        AddOpcode( "HALT", 0xe6 );
        AddOpcode( "HA.", 0xe6 );
        AddOpcode( "REPEAT", 0xe7 );
        AddOpcode( "REP.", 0xe7 );
        AddOpcode( "UNTIL", 0xe8 );
        AddOpcode( "UN.", 0xe8 );
        AddOpcode( "WHILE", 0xe9 );
        AddOpcode( "WH.", 0xe9 );
        AddOpcode( "WEND", 0xea );
        AddOpcode( "WE.", 0xea );
        AddOpcode( "CIF", 0xeb );
        AddOpcode( "CI.", 0xeb );
        AddOpcode( "CELSE", 0xec );
        AddOpcode( "CE.", 0xec );
        AddOpcode( "CEND", 0xed );
        AddOpcode( "CEN.", 0xed );
        AddOpcode( "LABEL", 0xee );
        AddOpcode( "LA.", 0xee );
        AddOpcode( "DOKE", 0xef );
        AddOpcode( "DO.", 0xef );
        AddOpcode( "EXIT", 0xf0 );
        AddOpcode( "EXI.", 0xf0 );
        AddOpcode( "ALLOCATE", 0xf1 );
        AddOpcode( "AL.", 0xf1 );
        AddOpcode( "DISABLE", 0xf2 );
        AddOpcode( "DIS.", 0xf2 );
        AddOpcode( "PULL", 0xf3 );
        AddOpcode( "PU.", 0xf3 );
        AddOpcode( "DLOAD", 0xf4 );
        AddOpcode( "DL.", 0xf4 );
        AddOpcode( "DSAVE", 0xf5 );
        AddOpcode( "DS.", 0xf5 );
        AddOpcode( "VAR", 0xf6 );
        AddOpcode( "LOCAL", 0xf7 );
        AddOpcode( "LOC.", 0xf7 );
        AddOpcode( "PROCEND", 0xf8 );
        AddOpcode( "PRO.", 0xf8 );
        AddOpcode( "PROC", 0xf9 );
        AddOpcode( "CASEND", 0xfa );
        AddOpcode( "CA.", 0xfa );
        AddOpcode( "OF", 0xfb );
        AddOpcode( "CASE", 0xfc );
        AddOpcode( "RPT", 0xfd );
        AddOpcode( "RP.", 0xfd );
        AddOpcode( "SETATR", 0xfe );
        AddOpcode( "SE.", 0xfe );
        AddOpcode( "PI", 0xff );

        AddOpcode( "SCLR", 0x0101 );
        AddOpcode( "SC.", 0x0101 );
        AddOpcode( "SPRITE", 0x0102 );
        AddOpcode( "SPR.", 0x0102 );
        AddOpcode( "WIPE", 0x0103 );
        AddOpcode( "WIP.", 0x0103 );
        AddOpcode( "RESET", 0x0104 );
        AddOpcode( "RESE.", 0x0104 );
        AddOpcode( "H38COL", 0x0105 );
        AddOpcode( "H3.", 0x0105 );
        AddOpcode( "LORES", 0x0106 );
        AddOpcode( "LOR.", 0x0106 );
        AddOpcode( "HIRES", 0x0107 );
        AddOpcode( "HI.", 0x0107 );
        AddOpcode( "PLOT", 0x0108 );
        AddOpcode( "PL.", 0x0108 );
        AddOpcode( "BOX", 0x0109 );
        AddOpcode( "B.", 0x0109 );
        AddOpcode( "POLY", 0x010a );
        AddOpcode( "POL.", 0x010a );
        AddOpcode( "DRAW", 0x010b );
        AddOpcode( "DR.", 0x010b );
        AddOpcode( "MODE", 0x010c );
        AddOpcode( "S2COL", 0x010d );
        AddOpcode( "S2.", 0x010d );
        AddOpcode( "S4COL", 0x010e );
        AddOpcode( "S4.", 0x010e );
        AddOpcode( "H40COL", 0x010f );
        AddOpcode( "H4.", 0x010f );
        AddOpcode( "SCRX", 0x0110 );
        AddOpcode( "SCR.", 0x0110 );
        AddOpcode( "WRR1", 0x0111 );
        AddOpcode( "WR.", 0x0111 );
        AddOpcode( "WRL1", 0x0112 );
        AddOpcode( "WRL.", 0x0112 );
        AddOpcode( "SCR1", 0x0113 );
        AddOpcode( "SCL1", 0x0114 );
        AddOpcode( "WRR2", 0x0115 );
        AddOpcode( "WRL2", 0x0116 );
        AddOpcode( "SCR2", 0x0117 );
        AddOpcode( "SCL2", 0x0118 );
        AddOpcode( "WRR8", 0x0119 );
        AddOpcode( "WRL8", 0x011a );
        AddOpcode( "SCR8", 0x011b );
        AddOpcode( "SCL8", 0x011c );
        AddOpcode( "ATTR", 0x011d );
        AddOpcode( "ATT.", 0x011d );
        AddOpcode( "ATTL", 0x011e );
        AddOpcode( "ATTUP", 0x011f );
        AddOpcode( "ATTU.", 0x011f );
        AddOpcode( "ATTDN", 0x0120 );
        AddOpcode( "ATTD.", 0x0120 );
        AddOpcode( "CHAR", 0x0121 );
        AddOpcode( "CHA.", 0x0121 );
        AddOpcode( "WINDOW", 0x0122 );
        AddOpcode( "WIN.", 0x0122 );
        AddOpcode( "MULTI", 0x0123 );
        AddOpcode( "MU.", 0x0123 );
        AddOpcode( "MONO", 0x0124 );
        AddOpcode( "MON.", 0x0124 );
        AddOpcode( "TBORDER", 0x0125 );
        AddOpcode( "TB.", 0x0125 );
        AddOpcode( "HBORDER", 0x0126 );
        AddOpcode( "HB.", 0x0126 );
        AddOpcode( "TPAPER", 0x0127 );
        AddOpcode( "TP.", 0x0127 );
        AddOpcode( "HPAPER", 0x0128 );
        AddOpcode( "HP.", 0x0128 );
        AddOpcode( "WRAP", 0x0129 );
        AddOpcode( "WRA.", 0x0129 );
        AddOpcode( "SCROLL", 0x012a );
        AddOpcode( "SCRO.", 0x012a );
        AddOpcode( "INK", 0x012b );
        AddOpcode( "SETA", 0x012c );
        AddOpcode( "ATTGET", 0x012d );
        AddOpcode( "ATTG.", 0x012d );
        AddOpcode( "ATT2ON", 0x012e );
        AddOpcode( "ATT2.", 0x012e );
        AddOpcode( "ATTON", 0x012f );
        AddOpcode( "ATTO.", 0x012f );
        AddOpcode( "ATTOFF", 0x0130 );
        AddOpcode( "ATTOF.", 0x0130 );
        AddOpcode( "MIR", 0x0131 );
        AddOpcode( "MAR", 0x0132 );
        AddOpcode( "MA.", 0x0132 );
        AddOpcode( "WCLR", 0x0133 );
        AddOpcode( "WC.", 0x0133 );
        AddOpcode( "INV", 0x0134 );
        AddOpcode( "SPIN", 0x0135 );
        AddOpcode( "SPI.", 0x0135 );
        AddOpcode( "MOVBLK", 0x0136 );
        AddOpcode( "MOV.", 0x0136 );
        AddOpcode( "MOVXOR", 0x0137 );
        AddOpcode( "MOVX.", 0x0137 );
        AddOpcode( "MOVAND", 0x0138 );
        AddOpcode( "MOVA.", 0x0138 );
        AddOpcode( "MOVOR", 0x0139 );
        AddOpcode( "MOVO.", 0x0139 );
        AddOpcode( "MOVATT", 0x013a );
        AddOpcode( "MOVAT.", 0x013a );
        AddOpcode( "EXX", 0x013b );
        AddOpcode( "EXY", 0x013c );
        AddOpcode( "GETBLK", 0x013d );
        AddOpcode( "GETB.", 0x013d );
        AddOpcode( "PUTBLK", 0x013e );
        AddOpcode( "PUT.", 0x013e );
        AddOpcode( "CPYBLK", 0x013f );
        AddOpcode( "CP.", 0x013f );
        AddOpcode( "GETXOR", 0x0140 );
        AddOpcode( "GETX.", 0x0140 );
        AddOpcode( "PUTXOR", 0x0141 );
        AddOpcode( "PUTX.", 0x0141 );
        AddOpcode( "CPYXOR", 0x0142 );
        AddOpcode( "CPYX.", 0x0142 );
        AddOpcode( "GETOR", 0x0143 );
        AddOpcode( "GETO.", 0x0143 );
        AddOpcode( "PUTOR", 0x0144 );
        AddOpcode( "PUTO.", 0x0144 );
        AddOpcode( "CPYOR", 0x0145 );
        AddOpcode( "CPYO.", 0x0145 );
        AddOpcode( "GETAND", 0x0146 );
        AddOpcode( "GETA.", 0x0146 );
        AddOpcode( "PUTAND", 0x0147 );
        AddOpcode( "PUTA.", 0x0147 );
        AddOpcode( "CPYAND", 0x0148 );
        AddOpcode( "CPYA.", 0x0148 );
        AddOpcode( "DBLANK", 0x0149 );
        AddOpcode( "DB.", 0x0149 );
        AddOpcode( "DSHOW", 0x014a );
        AddOpcode( "DSH.", 0x014a );
        AddOpcode( "PUTCHR", 0x014b );
        AddOpcode( "PUT.", 0x014b );
        AddOpcode( "LCASE", 0x014c );
        AddOpcode( "LC.", 0x014c );
        AddOpcode( "UCASE", 0x014d );
        AddOpcode( "UC.", 0x014d );
        AddOpcode( "CONV", 0x014e );
        AddOpcode( "HON", 0x014f );
        AddOpcode( "HO.", 0x014f );
        AddOpcode( "HOFF", 0x0150 );
        AddOpcode( "HOF.", 0x0150 );
        AddOpcode( "HSET", 0x0151 );
        AddOpcode( "HS.", 0x0151 );
        AddOpcode( "FLIPA", 0x0152  );
        AddOpcode( "FL.", 0x0152 );
        AddOpcode( "H4COL", 0x0153  );
        AddOpcode( "H4C.", 0x0153 );
        AddOpcode( "H2COL", 0x0154  );
        AddOpcode( "H2.", 0x0154 );
        AddOpcode( "H1COL", 0x0155  );
        AddOpcode( "H1.", 0x0155 );
        AddOpcode( "H3COL", 0x0156  );
        AddOpcode( "H3C.", 0x0156 );
        AddOpcode( "HEXX", 0x0157 );
        AddOpcode( "HSHX", 0x0158 );
        AddOpcode( "HSH.", 0x0158 );
        AddOpcode( "HEXY", 0x0159 );
        AddOpcode( "HSHY", 0x015a );
        AddOpcode( "HX", 0x015b );
        AddOpcode( "HY", 0x015c );
        AddOpcode( "HCOL", 0x015d );
        AddOpcode( "HC.", 0x015d );
        AddOpcode( "OVER", 0x015e );
        AddOpcode( "OV.", 0x015e );
        AddOpcode( "UNDER", 0x015f );
        AddOpcode( "UND.", 0x015f );
        AddOpcode( "SWAPATT", 0x0160 );
        AddOpcode( "SW.", 0x0160 );
        AddOpcode( "DTCTON", 0x0161 );
        AddOpcode( "DT.", 0x0161 );
        AddOpcode( "DTCTOFF", 0x0162 );
        AddOpcode( "DTCTOF.", 0x0162 );
        AddOpcode( "BLK%BLK", 0x0163 );
        AddOpcode( "BL.", 0x0163 );
        AddOpcode( "OR%BLK", 0x0164 );
        AddOpcode( "OR%.", 0x0164 );
        AddOpcode( "AND%BLK", 0x0165 );
        AddOpcode( "AND%.", 0x0165 );
        AddOpcode( "XOR%BLK", 0x0166 );
        AddOpcode( "XO.", 0x0166 );
        AddOpcode( "BLK%OR", 0x0167 );
        AddOpcode( "BLK%O.", 0x0167 );
        AddOpcode( "OR%OR", 0x0168 );
        AddOpcode( "OR%O.", 0x0168 );
        AddOpcode( "AND%OR", 0x0169 );
        AddOpcode( "AND%O.", 0x0169 );
        AddOpcode( "XOR%OR", 0x016a );
        AddOpcode( "XOR%O.", 0x016a );
        AddOpcode( "BLK%AND", 0x016b );
        AddOpcode( "BLK%A.", 0x016b );
        AddOpcode( "OR%AND", 0x016c );
        AddOpcode( "OR%A.", 0x016c );
        AddOpcode( "AND%AND", 0x016d );
        AddOpcode( "AND%A.", 0x016d );
        AddOpcode( "XOR%AND", 0x016e );
        AddOpcode( "XOR%A.", 0x016e );
        AddOpcode( "BLK%XOR", 0x016f );
        AddOpcode( "BLK%X.", 0x016f );
        AddOpcode( "OR%XOR", 0x0170 );
        AddOpcode( "OR%X.", 0x0170 );
        AddOpcode( "AND%XOR", 0x0171 );
        AddOpcode( "AND%X.", 0x0171 );
        AddOpcode( "XOR%XOR", 0x0172 );
        AddOpcode( "XOR%X.", 0x0172 );
        AddOpcode( "TEXT", 0x0173 );
        AddOpcode( "TE.", 0x0173 );
        AddOpcode( "FLIP", 0x0174 );
        AddOpcode( "HIT", 0x0175 );
        AddOpcode( "SCAN", 0x0176 );
        AddOpcode( "SCA.", 0x0176 );
        AddOpcode( "POINT", 0x0177 );
        AddOpcode( "POI.", 0x0177 );
        AddOpcode( "DFA", 0x0178 );
        AddOpcode( "DF.", 0x0178 );
        AddOpcode( "AFA2", 0x0179 );
        AddOpcode( "AF.", 0x0179 );
        AddOpcode( "AFA", 0x017a );
        AddOpcode( "KB", 0x017b );
        AddOpcode( "FIRE1", 0x017c );
        AddOpcode( "FI.", 0x017c );
        AddOpcode( "FIRE2", 0x017d );
        AddOpcode( "JS1", 0x017e );
        AddOpcode( "J.", 0x017e );
        AddOpcode( "JS2", 0x017f );

        AddOpcode( "BLACK", 0x0201 );
        AddOpcode( "BLA.", 0x0201 );
        AddOpcode( "WHITE", 0x0202 );
        AddOpcode( "WHIT.", 0x0202 );
        AddOpcode( "RED", 0x0203 );
        AddOpcode( "CYAN", 0x0204 );
        AddOpcode( "CY.", 0x0204 );
        AddOpcode( "PURPLE", 0x0205 );
        AddOpcode( "PUR.", 0x0205 );
        AddOpcode( "GREEN", 0x0206 );
        AddOpcode( "GR.", 0x0206 );
        AddOpcode( "BLUE", 0x0207 );
        AddOpcode( "BLU.", 0x0207 );
        AddOpcode( "YELLOW", 0x0208 );
        AddOpcode( "Y.", 0x0208 );
        AddOpcode( "ORANGE", 0x0209 );
        AddOpcode( "ORA.", 0x0209 );
        AddOpcode( "BROWN", 0x020a );
        AddOpcode( "BR.", 0x020a );
        AddOpcode( ".RED", 0x020b );
        AddOpcode( ".R.", 0x020b );
        AddOpcode( "GRAY1", 0x020c );
        AddOpcode( "GRA.", 0x020c );
        AddOpcode( "GRAY2", 0x020d );
        AddOpcode( ".GREEN", 0x020e );
        AddOpcode( ".G.", 0x020e );
        AddOpcode( ".BLUE", 0x020f );
        AddOpcode( ".B.", 0x020f );
        AddOpcode( "GRAY3", 0x0210 );
        AddOpcode( "OSC", 0x0211 );
        AddOpcode( "OS.", 0x0211 );
        AddOpcode( "ENV", 0x0212 );
        AddOpcode( "FRQ", 0x0213 );
        AddOpcode( "NOISE", 0x0214 );
        AddOpcode( "NOI.", 0x0214 );
        AddOpcode( "PULSE", 0x0215 );
        AddOpcode( "PULS.", 0x0215 );
        AddOpcode( "SAW", 0x0216 );
        AddOpcode( "TRI", 0x0217 );
        AddOpcode( "RING", 0x0218 );
        AddOpcode( "RIN.", 0x0218 );
        AddOpcode( "SYNC", 0x0219 );
        AddOpcode( "SYN.", 0x0219 );
        AddOpcode( "MUSIC", 0x021a );
        AddOpcode( "MUS.", 0x021a );
        AddOpcode( "ADSR", 0x021b );
        AddOpcode( "AD.", 0x021b );
        AddOpcode( "FILTER", 0x021c );
        AddOpcode( "FIL.", 0x021c );
        AddOpcode( "MUTE", 0x021d );
        AddOpcode( "MUT.", 0x021d );
        AddOpcode( "VOLUME", 0x021e );
        AddOpcode( "VO.", 0x021e );
        AddOpcode( "CUTOFF", 0x021f );
        AddOpcode( "CU.", 0x021f );
        AddOpcode( "RESONANCE", 0x0220 );
        AddOpcode( "RESO.", 0x0220 );
        AddOpcode( "PASS", 0x0221 );
        AddOpcode( "PA.", 0x0221 );
        AddOpcode( "SCRY", 0x0222 );
        AddOpcode( "SCR.", 0x0222 );
        AddOpcode( "RECALL", 0x0223 );
        AddOpcode( "REC.", 0x0223 );
        AddOpcode( "STORE", 0x0224 );
        AddOpcode( "STOR.", 0x0224 );
        AddOpcode( "SIDCLR", 0x0225 );
        AddOpcode( "SID.", 0x0225 );
        AddOpcode( "MERGE", 0x0226 );
        AddOpcode( "ME.", 0x0226 );
        AddOpcode( "RESEQ", 0x0227 );

        if ( Version == BasicVersion.LASER_BASIC )
        {
          AddOpcode( "RESERVE", 0x0228 );
          AddOpcode( "RESER.", 0x0228 );
        }
        else if ( Version == BasicVersion.BASIC_LIGHTNING )
        {
          AddOpcode( "MEM", 0x0228 );
        }
        
        AddOpcode( "OLD", 0x0229 );
        AddOpcode( "OL.", 0x0229 );
        AddOpcode( "DIR", 0x022a );
        AddOpcode( "DSTORE", 0x022b );
        AddOpcode( "DST.", 0x022b );
        AddOpcode( "DRECALL", 0x022c );
        AddOpcode( "DRE.", 0x022c );
        AddOpcode( "DMERGE", 0x022d );
        AddOpcode( "DM.", 0x022d );

        AddOpcode( "MOVE", 0x022f );
        AddOpcode( "PLAY", 0x0230 );
        AddOpcode( "PLA.", 0x0230 );
        AddOpcode( "RPLAY", 0x231 );
        AddOpcode( "RPL.", 0x231 );
        AddOpcode( "TRACK", 0x022e );
        AddOpcode( "TRA.", 0x022e );
      }

      if ( Version == BasicVersion.LASER_BASIC )
      {
        AddOpcode( "AUTO", 0x0232 );
        AddOpcode( "AU.", 0x0232 );
        AddOpcode( "RENUM", 0x0233 );
        AddOpcode( "REN.", 0x0233 );
        AddOpcode( "CSPRITE", 0x0235 );
        AddOpcode( "CS.", 0x0235 );
        AddOpcode( "CPUT", 0x0236 );
        AddOpcode( "CPU.", 0x0236 );
        AddOpcode( "CGET", 0x0237 );
        AddOpcode( "CG.", 0x0237 );
        AddOpcode( "CSWAP", 0x0238 );
        AddOpcode( "CSW.", 0x0238 );
        AddOpcode( "FILL", 0x0239 );
        AddOpcode( "RASTER", 0x023a );
        AddOpcode( "RA.", 0x023a );
        AddOpcode( "EBACK", 0x023b );
        AddOpcode( "EB.", 0x023b );
        AddOpcode( "BG0", 0x023c );
        AddOpcode( "BG.", 0x023c );
        AddOpcode( "BG1", 0x023d );
        AddOpcode( "BG2", 0x023e );
        AddOpcode( "BG3", 0x023f );
        AddOpcode( "SWITCH", 0x0240 );
        AddOpcode( "SWI.", 0x0240 );
        AddOpcode( "NORM", 0x0241 );
        AddOpcode( "NOR.", 0x0241 );
        AddOpcode( "KEYOFF", 0x0242 );
        AddOpcode( "KEYOF.", 0x0242 );
        AddOpcode( "KEYON", 0x0243 );
        AddOpcode( "KE.", 0x0243 );
        AddOpcode( "MCOL1", 0x0244 );
        AddOpcode( "MC.", 0x0244 );
        AddOpcode( "MCOL2", 0x0245 );
        AddOpcode( "MCOL3", 0x0246 );
        AddOpcode( "FGND", 0x0247 );
        AddOpcode( "FG.", 0x0247 );
        AddOpcode( "BGND", 0x0248 );
        AddOpcode( "BGN.", 0x0248 );
        AddOpcode( "EI", 0x0249 );
        AddOpcode( "DI", 0x024a );
        AddOpcode( "UNSYNC", 0x024b );
        AddOpcode( "UNS.", 0x024b );
        AddOpcode( "RSYNC", 0x024c );
        AddOpcode( "RS.", 0x024c );
        AddOpcode( "INIT", 0x024d );
        AddOpcode( "INI.", 0x024d );
      }

      if ( Version == BasicVersion.SIMONS_BASIC )
      {
        // new commands
        AddOpcode( "HIRES", 0x6401 );
        AddOpcode( "PLOT", 0x6402 );
        AddOpcode( "LINE", 0x6403 );
        AddOpcode( "BLOCK", 0x6404 );
        AddOpcode( "FCHR", 0x6405 );
        AddOpcode( "FCOL", 0x6406 );
        AddOpcode( "FILL", 0x6407 );
        AddOpcode( "REC", 0x6408 );
        AddOpcode( "ROT", 0x6409 );
        AddOpcode( "DRAW", 0x640a );
        AddOpcode( "CHAR", 0x640b );
        AddOpcode( "HI COL", 0x640c );
        AddOpcode( "HICOL", 0x640c );
        AddOpcode( "INV", 0x640d );
        AddOpcode( "FRAC", 0x640e );
        AddOpcode( "MOVE", 0x640f );
        AddOpcode( "PLACE", 0x6410 );
        AddOpcode( "UPB", 0x6411 );
        AddOpcode( "UPW", 0x6412 );
        AddOpcode( "LEFTW", 0x6413 );
        AddOpcode( "LEFTB", 0x6414 );
        AddOpcode( "DOWNB", 0x6415 );
        AddOpcode( "DOWNW", 0x6416 );
        AddOpcode( "RIGHTB", 0x6417 );
        AddOpcode( "RIGHTW", 0x6418 );
        AddOpcode( "MULTI", 0x6419 );
        AddOpcode( "COLOUR", 0x641a );
        AddOpcode( "MMOB", 0x641b );
        AddOpcode( "BFLASH", 0x641c );
        AddOpcode( "MOB SET", 0x641d );
        AddOpcode( "MOBSET", 0x641d );
        AddOpcode( "MUSIC", 0x641e );
        AddOpcode( "FLASH", 0x641f );
        AddOpcode( "REPEAT", 0x6420 );
        AddOpcode( "PLAY", 0x6421 );
        AddOpcode( "CENTRE", 0x6423 );
        AddOpcode( "ENVELOPE", 0x6424 );
        AddOpcode( "CGOTO", 0x6425 );
        AddOpcode( "WAVE", 0x6426 );
        AddOpcode( "FETCH", 0x6427 );
        AddOpcode( "AT(", 0x6428 );
        AddOpcode( "UNTIL", 0x6429 );
        AddOpcode( "USE", 0x642c );
        AddOpcode( "GLOBAL", 0x642e );

        AddOpcode( "RESET", 0x6430 );
        AddOpcode( "PROC", 0x6431 );
        AddOpcode( "CALL", 0x6432 );
        AddOpcode( "EXEC", 0x6433 );
        AddOpcode( "END PROC", 0x6434 );
        AddOpcode( "EXIT", 0x6435 );
        AddOpcode( "END LOOP", 0x6436 );
        AddOpcode( "ON KEY", 0x6437 );
        AddOpcode( "DISABLE", 0x6438 );
        AddOpcode( "RESUME", 0x6439 );
        AddOpcode( "LOOP", 0x643a );
        AddOpcode( "DELAY", 0x643b );

        AddOpcode( "SECURE", 0x6440 );
        AddOpcode( "DISAPA", 0x6441 );
        AddOpcode( "CIRCLE", 0x6442 );
        AddOpcode( "ON ERROR", 0x6443 );
        AddOpcode( "NO ERROR", 0x6444 );
        AddOpcode( "LOCAL", 0x6445 );
        AddOpcode( "RCOMP", 0x6446 );
        AddOpcode( "ELSE", 0x6447 );
        AddOpcode( "RETRACE", 0x6448 );
        AddOpcode( "TRACE", 0x6449 );
        AddOpcode( "DIR", 0x644a );
        AddOpcode( "PAGE", 0x644b );
        AddOpcode( "DUMP", 0x644c );
        AddOpcode( "FIND", 0x644d );
        AddOpcode( "OPTION", 0x644e );
        AddOpcode( "AUTO", 0x644f );

        AddOpcode( "OLD", 0x6450 );
        AddOpcode( "JOY", 0x6451 );
        AddOpcode( "MOD", 0x6452 );
        AddOpcode( "DIV", 0x6453 );
        AddOpcode( "DUP", 0x6455 );
        AddOpcode( "INKEY", 0x6456 );
        AddOpcode( "INST", 0x6457 );
        AddOpcode( "TEST", 0x6458 );
        AddOpcode( "LIN", 0x6459 );
        AddOpcode( "EXOR", 0x645a );
        AddOpcode( "INSERT", 0x645b );
        AddOpcode( "POT", 0x645c );
        AddOpcode( "PENX", 0x645d );
        AddOpcode( "PENY", 0x645f );

        AddOpcode( "SOUND", 0x6460 );
        AddOpcode( "GRAPHICS", 0x6461 );
        AddOpcode( "DESIGN", 0x6462 );
        AddOpcode( "RLOCMOB", 0x6463 );
        AddOpcode( "CMOB", 0x6464 );
        AddOpcode( "BCKGNDS", 0x6465 );
        AddOpcode( "PAUSE", 0x6466 );
        AddOpcode( "NRM", 0x6467 );
        AddOpcode( "MOB OFF", 0x6468 );
        AddOpcode( "OFF", 0x6469 );
        AddOpcode( "ANGL", 0x646a );
        AddOpcode( "ARC", 0x646b );
        AddOpcode( "COLD", 0x646c );
        AddOpcode( "SCRSV", 0x646d );
        AddOpcode( "SCRLD", 0x646e );
        AddOpcode( "TEXT", 0x646f );

        AddOpcode( "CSET", 0x6470 );
        AddOpcode( "VOL", 0x6471 );
        AddOpcode( "DISK", 0x6472 );
        AddOpcode( "HRDCPY", 0x6473 );
        AddOpcode( "KEY", 0x6474 );
        AddOpcode( "PAINT", 0x6475 );
        AddOpcode( "LOW COL", 0x6476 );
        AddOpcode( "COPY", 0x6477 );
        AddOpcode( "MERGE", 0x6478 );
        AddOpcode( "RENUMBER", 0x6479 );
        AddOpcode( "MEM", 0x647a );
        AddOpcode( "DETECT", 0x647b );
        AddOpcode( "CHECK", 0x647c );
        AddOpcode( "DISPLAY", 0x647d );
        AddOpcode( "ERR", 0x647e );
        AddOpcode( "ERRLN", 0x647e );
        AddOpcode( "ERRN", 0x647e );
        AddOpcode( "OUT", 0x647f );
      }

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
      AddActionToken( TokenValue.INDIRECT_KEY, "{SHIFT-ARROWLEFT}", 0x5f );
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
      //m_CompileCurrentAddress = -1;

      AssembledOutput = null;
      Messages.Clear();
      m_LineInfos.Clear();
      m_ErrorMessages = 0;
      m_WarningMessages = 0;
      m_Filename = "";
    }



    public override GR.Collections.MultiMap<string, Types.SymbolInfo> KnownTokenInfo()
    {
      GR.Collections.MultiMap<string, Types.SymbolInfo> knownTokens = new GR.Collections.MultiMap<string, Types.SymbolInfo>();

      /*
      knownTokens.AddRange( m_Labels.Keys );
      knownTokens.AddRange( m_UnparsedLabels.Keys );
       */

      return knownTokens;
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
      if ( m_Opcodes.Count == 0 )
      {
        AddError( -1, Types.ErrorCode.E1401_INTERNAL_ERROR, "An unsupported BASIC version was chosen" );
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
          if ( ( entry.Value.Type != C64Studio.Types.SymbolInfo.Types.PREPROCESSOR_CONSTANT_1 )
          &&   ( entry.Value.Type != C64Studio.Types.SymbolInfo.Types.PREPROCESSOR_CONSTANT_2 )
          &&   ( entry.Value.Type != C64Studio.Types.SymbolInfo.Types.PREPROCESSOR_LABEL ) )
          {
            if ( ( entry.Value.Type == C64Studio.Types.SymbolInfo.Types.LABEL )
            &&   ( entry.Key.StartsWith( ASMFileParser.InternalLabelPrefix ) ) )
            {
              // do not pass on internal local labels
              continue;
            }
            C64Studio.Types.SymbolInfo    symbol = new C64Studio.Types.SymbolInfo();
            symbol.AddressOrValue = entry.Value.AddressOrValue;
            symbol.DocumentFilename = entry.Value.DocumentFilename;
            symbol.LocalLineIndex = entry.Value.LocalLineIndex;
            symbol.Name = entry.Value.Name;
            symbol.Type = entry.Value.Type;
            symbol.Used = true;
            symbol.Zone = entry.Value.Zone;
            symbol.FromDependency = true;
            symbol.Info = entry.Value.Info;

            ASMFileInfo.Labels.Add( entry.Key, symbol );
          }
        }
      }
    }



    private void ParseToken( string CurToken, LineInfo Info, ref bool lastOpcodeWasReferencingLineNumber )
    {
      string token2 = CurToken.Trim().ToUpper();
      if ( m_Opcodes.ContainsKey( token2 ) )
      {
        Opcode opCode = m_Opcodes[token2];

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
      else if ( m_ExOpcodes.ContainsKey( token2 ) )
      {
        Opcode opCode = m_ExOpcodes[token2];

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
        foreach ( var key in Types.ConstantData.AllPhysicalKeyInfos )
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
            AddError( LineIndex, Types.ErrorCode.E1301_MACRO_UNKNOWN, "Unknown macro " + macro );
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
          Info.Tokens[Info.Tokens.Count - 1].Content += Types.ConstantData.PetSCIIToChar[TokenValue].CharValue;
          Info.Tokens[Info.Tokens.Count - 1].TokenType = Token.Type.NUMERIC_LITERAL;

          Info.LineData.AppendU8( TokenValue );
          return;
        }
        Token numericToken = new Token();
        numericToken.TokenType = Token.Type.NUMERIC_LITERAL;
        numericToken.ByteValue = TokenValue;
        numericToken.Content = "" + Types.ConstantData.PetSCIIToChar[TokenValue].CharValue;
        numericToken.StartIndex = StartIndex;
        Info.Tokens.Add( numericToken );

        Info.LineData.AppendU8( TokenValue );
        return;
      }
      Token basicToken = new Token();
      basicToken.TokenType = Token.Type.DIRECT_TOKEN;
      basicToken.ByteValue = TokenValue;
      basicToken.Content = "" + Types.ConstantData.PetSCIIToChar[TokenValue].CharValue;
      basicToken.StartIndex = StartIndex;
      Info.Tokens.Add( basicToken );

      Info.LineData.AppendU8( TokenValue );
    }



    internal LineInfo PureTokenizeLine( string Line, int LineIndex )
    {
      //line = ReplaceMacros( lineIndex, line );
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
      if ( !LabelMode )
      {
        if ( endOfDigitPos == -1 )
        {
          AddError( LineIndex, Types.ErrorCode.E3000_BASIC_MISSING_LINE_NUMBER, "Missing line number" );
        }
        else
        {
          Token lineNumberToken = new Token();
          lineNumberToken.TokenType   = Token.Type.LINE_NUMBER;
          lineNumberToken.ByteValue   = GR.Convert.ToI32( Line.Substring( 0, endOfDigitPos + 1 ) );
          lineNumberToken.Content     = Line.Substring( 0, endOfDigitPos + 1 ).Trim();
          lineNumberToken.StartIndex  = 0;
          info.Tokens.Add( lineNumberToken );
          info.LineNumber = lineNumberToken.ByteValue;
          info.Line       = Line.Substring( endOfDigitPos + 1 ).Trim();
        }
      }

      int       posInLine = endOfDigitPos + 1;
      bool      insideMacro = false;
      bool      insideDataStatement = false;
      bool      insideStringLiteral = false;
      int       lastCharStartPos = -1;
      int       stringLiteralStartPos = -1;
      int       macroStartPos = -1;
      int       tempDataStartPos = -1;
      GR.Memory.ByteBuffer tempData = new GR.Memory.ByteBuffer();

      // translate line to actual PETSCII codes
      while ( posInLine < Line.Length )
      {
        char    curChar = Line[posInLine];

        if ( insideMacro )
        {
          if ( curChar == '}' )
          {
            insideMacro = false;

            if ( !insideStringLiteral )
            {
              string macro = Line.Substring( macroStartPos + 1, posInLine - macroStartPos - 1 ).ToUpper();

              Token tokenMacro = new Token();
              tokenMacro.TokenType = Token.Type.MACRO;
              tokenMacro.Content = macro;
              tokenMacro.StartIndex = macroStartPos;
              info.Tokens.Add( tokenMacro );
            }
          }
          ++posInLine;
          continue;
        }
        if ( curChar == '{' )
        {
          if ( tempData.Length > 0 )
          {
            ConsolidateTokens( info, tempData, tempDataStartPos );
            tempData.Clear();
            tempDataStartPos = -1;
          }
          insideMacro = true;
          macroStartPos = posInLine;
          ++posInLine;
          continue;
        }
        if ( curChar == '"' )
        {
          insideStringLiteral = !insideStringLiteral;
          if ( !insideStringLiteral )
          {
            Token tokenStringLiteral      = new Token();
            tokenStringLiteral.TokenType  = Token.Type.STRING_LITERAL;
            tokenStringLiteral.Content    = Line.Substring( stringLiteralStartPos, posInLine - stringLiteralStartPos + 1 );
            tokenStringLiteral.StartIndex = stringLiteralStartPos;
            info.Tokens.Add( tokenStringLiteral );
            ++posInLine;
            continue;
          }
          else
          {
            if ( tempData.Length > 0 )
            {
              ConsolidateTokens( info, tempData, tempDataStartPos );
              tempData.Clear();
              tempDataStartPos = -1;
            }
            stringLiteralStartPos = posInLine;
          }
        }
        if ( !insideStringLiteral )
        {
          if ( ( curChar >= 'A' )
          &&   ( curChar <= 'Z' ) )
          {
            if ( lastCharStartPos == -1 )
            {
              // start of chars, potential BASIC code
              if ( tempData.Length > 0 )
              {
                ConsolidateTokens( info, tempData, tempDataStartPos );
                tempData.Clear();
                tempDataStartPos = -1;
              }
              lastCharStartPos = posInLine;
            }
          }
          else
          {
            lastCharStartPos = -1;
          }

          if ( !Types.ConstantData.CharToC64Char.ContainsKey( curChar ) )
          {
            if ( !Types.ConstantData.CharToC64Char.ContainsKey( char.ToUpper( curChar ) ) )
            {
              AddError( LineIndex, Types.ErrorCode.E3002_BASIC_UNSUPPORTED_CHARACTER, "Unsupported character " + (int)curChar + " encountered" );
            }
            else if ( !Types.ConstantData.CharToC64Char[char.ToUpper( curChar )].HasPetSCII )
            {
              AddError( LineIndex, Types.ErrorCode.E3002_BASIC_UNSUPPORTED_CHARACTER, "Unsupported character " + (int)curChar + " encountered" );
            }
            else
            {
              if ( tempDataStartPos == -1 )
              {
                tempDataStartPos = posInLine;
              }
              tempData.AppendU8( Types.ConstantData.CharToC64Char[char.ToUpper( curChar )].PetSCIIValue );
            }
          }
          else if ( !Types.ConstantData.CharToC64Char[curChar].HasPetSCII )
          {
            AddError( LineIndex, Types.ErrorCode.E3002_BASIC_UNSUPPORTED_CHARACTER, "Unsupported character " + (int)curChar + " encountered" );
          }
          else
          {
            //if ( posInLine > endOfDigitPos + 1 )
            {
              if ( tempDataStartPos == -1 )
              {
                tempDataStartPos = posInLine;
              }
              tempData.AppendU8( Types.ConstantData.CharToC64Char[curChar].PetSCIIValue );
            }
          }
          // is there a BASIC token in there?
          if ( tempData.Length > 0 )
          {
            byte    nextByte = tempData.ByteAt( 0 );

            if ( nextByte == 32 )
            {
              // Space
              if ( tempData.Length > 0 )
              {
                ConsolidateTokens( info, tempData, tempDataStartPos );
                tempData.Clear();
                tempDataStartPos = -1;
              }
              ++posInLine;
              continue;
            }
            if ( nextByte == 0x3f )
            {
              // ? durch PRINT ersetzen
              /*
              if ( tempData.Length > 0 )
              {
                ConsolidateTokens( info, tempData, tempDataStartPos );
                tempData.Clear();
                tempDataStartPos = -1;
              }*/
              nextByte = 0x99;

              Token basicToken2      = new Token();
              basicToken2.TokenType  = Token.Type.BASIC_TOKEN;
              basicToken2.ByteValue  = nextByte;
              basicToken2.Content    = "" + Types.ConstantData.PetSCIIToChar[0x3f].CharValue;
              basicToken2.StartIndex = posInLine;
              info.Tokens.Add( basicToken2 );

              tempData.Clear();
              tempDataStartPos = -1;
              ++posInLine;
              continue;
            }
            // is there a token now?
            bool entryFound = true;
            bool potentialToken = false;
            foreach ( KeyValuePair<ushort, Opcode> opcodeEntry in m_OpcodesFromByte )
            {
              Opcode  opcode = opcodeEntry.Value;

              entryFound = true;
              for ( int i = 0; i < opcode.Command.Length; ++i )
              {
                if ( i >= tempData.Length )
                {
                  // can't be this token, length not complete
                  entryFound = false;
                  if ( i > 0 )
                  {
                    potentialToken = true;
                  }
                  break;
                }
                if ( tempData.ByteAt( i ) != (byte)opcode.Command[i] )
                {
                  entryFound = false;
                  break;
                }
              }
              if ( entryFound )
              {
                Token basicToken4 = new Token();
                basicToken4.TokenType  = Token.Type.BASIC_TOKEN;
                basicToken4.ByteValue  = (byte)opcode.InsertionValue;
                basicToken4.Content    = opcode.Command;
                basicToken4.StartIndex = tempDataStartPos;
                info.Tokens.Add( basicToken4 );

                tempData.Clear();
                tempDataStartPos = -1;
                insideDataStatement = ( opcode.Command == "DATA" );

                if ( IsComment( opcode ) )
                {
                  if ( basicToken4.StartIndex + 3 < Line.Length )
                  {
                    // everything else is comment
                    Token comment       = new Token();
                    comment.TokenType   = Token.Type.COMMENT;
                    comment.Content     = Line.Substring( basicToken4.StartIndex + 3 );
                    comment.StartIndex  = basicToken4.StartIndex + 3;
                    info.Tokens.Add( comment );

                    return info;
                  }
                }
                break;
              }
            }
            if ( entryFound )
            {
              ++posInLine;
              continue;
            }
            // is it an extended token?
            entryFound = true;
            foreach ( KeyValuePair<string, Opcode> opcodeEntry in m_ExOpcodes )
            {
              Opcode opcode = opcodeEntry.Value;

              entryFound = true;
              for ( int i = 0; i < opcode.Command.Length; ++i )
              {
                if ( i >= tempData.Length )
                {
                  // can't be this token
                  if ( i > 0 )
                  {
                    potentialToken = true;
                  }
                  entryFound = false;
                  break;
                }
                if ( tempData.ByteAt( i ) != (byte)opcode.Command[i] )
                {
                  entryFound = false;
                  break;
                }
              }
              if ( entryFound )
              {
                Token basicToken5 = new Token();
                basicToken5.TokenType  = Token.Type.EX_BASIC_TOKEN;
                basicToken5.ByteValue  = (byte)opcode.InsertionValue;
                basicToken5.Content    = opcode.Command;
                basicToken5.StartIndex = tempDataStartPos;
                info.Tokens.Add( basicToken5 );

                tempData.Clear();
                tempDataStartPos = -1;

                insideDataStatement = false;
                break;
              }
            }
            if ( entryFound )
            {
              ++posInLine;
              continue;
            }
            // not a token, add directly, but only if not a potential BASIC token!!
            if ( !potentialToken )
            {
              /*
              ConsolidateTokens( info, tempData, tempDataStartPos );
              tempData.Clear();
              tempDataStartPos = -1;*/
            }
          }
        }

        ++posInLine;
      }

      if ( ( tempDataStartPos != -1 )
      &&   ( tempDataStartPos < Line.Length ) )
      {
        // all the rest is one token
        ConsolidateTokens( info, tempData, tempDataStartPos );
      }

      return info;
    }



    private bool IsComment( Opcode Opcode )
    {
      if ( Opcode.Command == "REM" )
      {
        return true;
      }
      if ( ( Opcode.Command == "'" )
      &&   ( ( Settings.Version == BasicVersion.LASER_BASIC )
      ||     ( Settings.Version == BasicVersion.BASIC_LIGHTNING ) ) )
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
              basicToken6.Content += "" + Types.ConstantData.PetSCIIToChar[tempData.ByteAt( j )].CharValue;
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
              basicToken1.Content += "" + Types.ConstantData.PetSCIIToChar[tempData.ByteAt( j )].CharValue;
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
          basicToken2.Content     = "" + Types.ConstantData.PetSCIIToChar[nextByte].CharValue;
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
          basicToken3.Content += "" + Types.ConstantData.PetSCIIToChar[tempData.ByteAt( j )].CharValue;
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
          basicToken.Content += "" + Types.ConstantData.PetSCIIToChar[tempData.ByteAt( j )].CharValue;
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
          basicToken2.Content     = "" + Types.ConstantData.PetSCIIToChar[tempData.ByteAt( i )].CharValue;
          basicToken2.StartIndex  = TempDataStartPos + i;
          info.Tokens.Add( basicToken2 );
          lastTokenStartPos = i + 1;
        }
      }
    }



    internal LineInfo TokenizeLine( string Line, int LineIndex, ref int LastLineNumber )
    {
      //line = ReplaceMacros( lineIndex, line );
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

      TranslateCharactersToPETSCII( Line, LineIndex, endOfDigitPos, ref posInLine, ref insideMacro, ref macroStartPos, tempData );

      
      if ( tempData.Length + endOfDigitPos + 1 > 80 )
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
              var c64Key = Types.ConstantData.FindC64KeyByPETSCII( nextByte );
              if ( ( c64Key != null )
              && ( nextByte != 32 )   // do not replace for Space
              && ( c64Key.Replacements.Count > 0 ) )
              {
                stringLiteral += "{" + c64Key.Replacements[0] + "}";
              }
              else
              {
                stringLiteral += Types.ConstantData.PetSCIIToChar[nextByte].CharValue;
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
              stringLiteral += Types.ConstantData.PetSCIIToChar[nextByte].CharValue;
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
            basicToken.Content = "" + Types.ConstantData.PetSCIIToChar[0x3f].CharValue;
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

          if ( ( Settings.Version == BasicVersion.BASIC_LIGHTNING )
          ||   ( Settings.Version == BasicVersion.LASER_BASIC ) )
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
                    basicToken.Content = "" + Types.ConstantData.PetSCIIToChar[(byte)basicToken.ByteValue].CharValue;
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
          if ( ( nextByte == 39 )
          &&   ( ( Settings.Version == BasicVersion.BASIC_LIGHTNING )
          ||     ( Settings.Version == BasicVersion.LASER_BASIC ) ) )
          {
            insideREMStatement = true;
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
      return info;
    }



    private void TranslateCharactersToPETSCII( string Line, int LineIndex, int endOfDigitPos, ref int posInLine, ref bool insideMacro, ref int macroStartPos, ByteBuffer tempData )
    {
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
            foreach ( var key in Types.ConstantData.AllPhysicalKeyInfos )
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
                  AddError( LineIndex, Types.ErrorCode.E1301_MACRO_UNKNOWN, "Unknown macro " + macro );
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
                  AddError( LineIndex, Types.ErrorCode.E1301_MACRO_UNKNOWN, "Unknown macro " + macro );
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
        MapCharacterToActualKey( LineIndex, curChar, posInLine, endOfDigitPos, tempData );
        ++posInLine;
      }
    }



    private void MapCharacterToActualKey( int LineIndex, char curChar, int posInLine, int endOfDigitPos, ByteBuffer tempData )
    {
      if ( !Settings.UpperCaseMode )
      {
        // lower case mode
        if ( Types.ConstantData.LowerCaseCharTo64Char.ContainsKey( curChar ) )
        {
          var foundKey = Types.ConstantData.LowerCaseCharTo64Char[curChar];
          if ( !foundKey.HasPetSCII )
          {
            AddError( LineIndex, Types.ErrorCode.E3002_BASIC_UNSUPPORTED_CHARACTER, "Unsupported character " + (int)curChar + " encountered" );
          }
          else
          {
            if ( ( curChar != 32 )
            ||   ( posInLine > endOfDigitPos + 1 ) )
            {
              // strip spaces after line numbers

              // TODO - lower case petscii!
              tempData.AppendU8( foundKey.LowerCasePETSCII );
            }
          }
          return;
        }
      }
      if ( ( !Types.ConstantData.CharToC64Char.ContainsKey( curChar ) )
      ||   ( !Types.ConstantData.CharToC64Char[curChar].HasPetSCII ) )
      {
        AddError( LineIndex, Types.ErrorCode.E3002_BASIC_UNSUPPORTED_CHARACTER, "Unsupported character " + (int)curChar + " encountered" );
      }
      else
      {
        byte    petsciiValue = Types.ConstantData.CharToC64Char[curChar].PetSCIIValue;

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
      }
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
      if ( ( Settings.Version == BasicVersion.LASER_BASIC )
      ||   ( Settings.Version == BasicVersion.BASIC_LIGHTNING ) )
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
          if ( ( Info.Tokens[prevTokenIndex].ByteValue == m_Opcodes["PROC"].InsertionValue )
          ||   ( Info.Tokens[prevTokenIndex].ByteValue == m_Opcodes["LABEL"].InsertionValue ) )
          {
            // previous token was PROC or LABEL
            return false;
          }
          break;
        }

        // special behavior - no token after TASK<number>,
        if ( ( Info.Tokens.Count > 3 )
        &&   ( Info.Tokens[Info.Tokens.Count - 3].ByteValue == m_Opcodes["TASK"].InsertionValue )
        &&   ( Info.Tokens[Info.Tokens.Count - 2].TokenType == Token.Type.NUMERIC_LITERAL )
        &&   ( Info.Tokens[Info.Tokens.Count - 1].ByteValue == ',' ) )
        {
          // previous token was TASK<number>,
          return false;
        }
      }

      bool entryFound = true;
      foreach ( var opcodeEntry in m_Opcodes )
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
          Token basicToken = new Token();
          basicToken.TokenType = Token.Type.BASIC_TOKEN;
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

          InsideDataStatement = ( opcode.Command == "DATA" );
          if ( IsComment( opcode ) )
          {
            InsideREMStatement = true;

            if ( Settings.StripREM )
            {
              Info.LineData.Truncate( 1 );
              if ( opcode.InsertionValue > 255 )
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
          break;
        }
      }
      if ( entryFound )
      {
        return true;
      }

      // is it an extended token?
      entryFound = true;
      foreach ( KeyValuePair<string, Opcode> opcodeEntry in m_ExOpcodes )
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
      foreach ( var opcodeEntry in m_Opcodes )
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
      foreach ( KeyValuePair<string, Opcode> opcodeEntry in m_ExOpcodes )
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

        m_LineInfos[info.LineIndex] = info;
      }
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
          char replacement = Types.ConstantData.PetSCIIToChar[(byte)value].CharValue;
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
        bool  hadREM = false;

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
            if ( IsComment( token ) )
            {
              hadREM = true;
            }

            if ( ( token.ByteValue == m_Opcodes["RUN"].InsertionValue )
            ||   ( token.ByteValue == m_Opcodes["THEN"].InsertionValue ) )
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
            if ( ( token.ByteValue == m_Opcodes["GOTO"].InsertionValue )
            ||   ( token.ByteValue == m_Opcodes["GOSUB"].InsertionValue ) )
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
          if ( ( token.TokenType == Token.Type.BASIC_TOKEN )
          ||   ( token.TokenType == Token.Type.NUMERIC_LITERAL )
          ||   ( token.TokenType == Token.Type.EX_BASIC_TOKEN ) )
          {
            if ( ( !IsComment( token ) )
            &&   ( hadREM ) )
            {
              sb.Append( " " );
            }
          }
        }
        sb.AppendLine();
      }
      return sb.ToString();
    }



    private bool IsComment( Token token )
    {
      if ( token.ByteValue == m_Opcodes["REM"].InsertionValue )
      {
        return true;
      }
      if ( ( Settings.Version == BasicVersion.BASIC_LIGHTNING )
      ||   ( Settings.Version == BasicVersion.LASER_BASIC ) )
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
        if ( ( lineInfo.Value.Tokens.Count > 0 )
        &&   ( lineInfo.Value.Tokens[0].TokenType == Token.Type.EX_BASIC_TOKEN )
        &&   ( lineInfo.Value.Tokens[0].ByteValue == m_ExOpcodes["LABEL"].InsertionValue ) )
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
          &&   ( lineInfo.Value.Tokens[nextTokenIndex].ByteValue == m_ExOpcodes["LABEL"].InsertionValue )
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
          &&   ( ( token.ByteValue == m_Opcodes["GOTO"].InsertionValue )
          ||     ( token.ByteValue == m_Opcodes["GOSUB"].InsertionValue )
          ||     ( token.ByteValue == m_Opcodes["THEN"].InsertionValue )
          ||     ( token.ByteValue == m_Opcodes["RUN"].InsertionValue ) ) )
          {
            bool    isGotoOrGosub = ( token.ByteValue == m_Opcodes["GOTO"].InsertionValue ) | ( token.ByteValue == m_Opcodes["GOSUB"].InsertionValue );
            nextTokenIndex = FindNextToken( lineInfo.Value.Tokens, tokenIndex );
            nextTokenIndex2 = FindNextToken( lineInfo.Value.Tokens, nextTokenIndex );

            while ( ( nextTokenIndex != -1 )
            &&      ( nextTokenIndex2 != -1 ) )
            {
              if ( ( lineInfo.Value.Tokens[nextTokenIndex].TokenType == Token.Type.EX_BASIC_TOKEN )
              &&   ( lineInfo.Value.Tokens[nextTokenIndex].ByteValue == m_ExOpcodes["LABEL"].InsertionValue )
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

            // REM is only remark, no opcode parsing anymore
            if ( m_OpcodesFromByte.ContainsKey( byteValue ) )
            {
              lineContent += m_OpcodesFromByte[byteValue].Command;
            }
            else if ( Types.ConstantData.PETSCIIToUnicode.ContainsKey( byteValue ) )
            {
              char charToUse = Types.ConstantData.PETSCIIToUnicode[byteValue];
              lineContent += charToUse;
            }
            ++dataPos;
            continue;
          }

          if ( byteValue == 34 )
          {
            insideStringLiteral = !insideStringLiteral;
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

            var c64Key = Types.ConstantData.FindC64KeyByPETSCII( byteValue );
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
            if ( ( !m_OpcodesFromByte.ContainsKey( byteValue ) )
            ||   ( m_OpcodesFromByte[byteValue].Command.StartsWith( "{" ) ) )
            {
              //if ( KeymapEntryExists( System.Windows.Forms.InputLanguage.CurrentInputLanguage, System.Windows.Forms.Keys
              if ( Types.ConstantData.PETSCIIToUnicode.ContainsKey( byteValue ) )
              {
                char charToUse = Types.ConstantData.PETSCIIToUnicode[byteValue];
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
              if ( m_OpcodesFromByte[byteValue].InsertionValue == 0x8F )
              {
                encounteredREM = true;
              }
              lineContent += m_OpcodesFromByte[byteValue].Command;
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
            continue;
          }
          if ( token.TokenType == Token.Type.BASIC_TOKEN )
          {
            if ( ( token.ByteValue == m_Opcodes["RUN"].InsertionValue )
            ||   ( token.ByteValue == m_Opcodes["THEN"].InsertionValue ) )
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
            if ( ( token.ByteValue == m_Opcodes["GOTO"].InsertionValue )
            ||   ( token.ByteValue == m_Opcodes["GOSUB"].InsertionValue ) )
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
      return sb.ToString();
    }



    public string ReplaceAllSymbolsByMacros( string BasicText )
    {
      for ( int i = 0; i < BasicText.Length; ++i )
      {
        char    chartoCheck = BasicText[i];

        if ( chartoCheck > (char)255 )
        {
          var c64Key = Types.ConstantData.FindC64KeyByUnicode( chartoCheck );
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



    public string ReplaceAllMacrosBySymbols( string BasicText, out bool HadError )
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
            foreach ( var key in Types.ConstantData.AllPhysicalKeyInfos )
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
              Debug.Log( "Unknown macro " + macro );
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



    public static string MakeUpperCase( string BASICText )
    {
      StringBuilder   sb = new StringBuilder( BASICText.Length );

      foreach ( var singleChar in BASICText )
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
        &&        ( singleChar <= 0x7a ) )
        {
          sb.Append( (char)( singleChar - 0x20 + 0xe000 ) );
        }
        else
        {
          sb.Append( singleChar );
        }
      }
      return sb.ToString();
    }



    public static string MakeLowerCase( string BASICText )
    {
      StringBuilder   sb = new StringBuilder( BASICText.Length );

      foreach ( var singleChar in BASICText )
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
      return sb.ToString();
    }


  }
}
