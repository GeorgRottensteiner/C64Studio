﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using RetroDevStudio.Parser;
using GR.Memory;
using RetroDevStudio;
using System.Linq;
using RetroDevStudio.Types;
using System.Diagnostics;
using RetroDevStudio.CheckSummer;
using System.Reflection;
using System.Net;



namespace RetroDevStudio.Parser.BASIC
{
  public partial class BasicFileParser : ParserBase
  {
    public enum ExtractLineResult
    {
      OK = 0,
      END_OF_CODE,
      ERROR
    }

    public class ParserSettings
    {
      public bool         StripSpaces = true;
      public bool         StripREM = false;
      public bool         UpperCaseMode = true;
      public bool         UseC64Font = true;
      public Dialect      BASICDialect = null;
    }



    public class ParseContext
    {
      public ParserSettings     Settings = new ParserSettings();
      public CompileTargetType  TargetType = CompileTargetType.NONE;
      public MachineType        KeyboardMachineType = MachineType.C64;      
    }



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
      INDIRECT_KEY,

      // plus 4
      FLASH_ON,
      FLASH_OFF
    };

    public enum RenumberResult
    {
      OK = 0,
      TOO_MANY_LINES,
      NOTHING_TO_DO,
      INVALID_VALUES,
      OVERLAP_AT_START,
      OVERLAP_AT_END
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
        HARD_COMMENT,
        TEXT_LABEL
      };

      public Type       TokenType = Type.DIRECT_TOKEN;
      public string     Content = "";
      public int        ByteValue = 0;
      public int        StartIndex = 0;
    };

    internal class LineInfo
    {
      public int                      LineIndex = 0;
      public int                      LineNumber = -1;
      public string                   CheckSum = "";
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

    public GR.Collections.Map<Token.Type, string>     AllowedTokenStartChars = new GR.Collections.Map<Token.Type, string>();
    public GR.Collections.Map<Token.Type, string>     AllowedTokenChars = new GR.Collections.Map<Token.Type, string>();
    public GR.Collections.Map<Token.Type, string>     AllowedTokenEndChars = new GR.Collections.Map<Token.Type, string>();
    public string                                     AllowedSingleTokens;

    private string[]                                  _Lines;
    private string                                    _LastLineNumberDocument  = "";
    private int                                       _LastLineNumberDocLineIndex  = -1;

    public Dictionary<int, int>                       LabelLineMapping = new Dictionary<int, int>();

    private ParseContext                              _ParseContext = null;



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

      AllowedTokenStartChars[Token.Type.BASIC_TOKEN] = "ABCDEFGHIJKLMNOPQRSTUVWXYZ@";

      // TODO - these are C64 specific!
      // 0xeee1 = arrow up
      // 0xee5e = PI
      // 0xee1f = arrow left
      AllowedSingleTokens = "()+-,;:<>=!?'&/^{}*#$%[]" + (char)0xee1e + (char)0xee5e + (char)0xee1f;

      SetBasicDialect( Settings.BASICDialect );

      m_CompileConfig = new CompileConfig();

      _ParseContext = new ParseContext()
      {
        Settings    = Settings,
        TargetType  = CompileTargetType.PRG
      };
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
    }



    public void SetBasicDialect( Dialect Dialect )
    {
      if ( Dialect == null )
      {
        Dialect = Dialect.BASICV2;
      }
      Settings.BASICDialect = Dialect;

      _ParseContext = new ParseContext()
      {
        Settings    = Settings,
        TargetType  = Dialect.CompileType
      };

      var emptyOpcode = new Opcode( "", -1 );

      if ( Dialect != null )
      {
        if ( !string.IsNullOrEmpty( Dialect.HexPrefix ) )
        {
          if ( !AllowedTokenStartChars[Token.Type.NUMERIC_LITERAL].Contains( Dialect.HexPrefix ) )
          {
            AllowedTokenStartChars[Token.Type.NUMERIC_LITERAL] += Dialect.HexPrefix;
            AllowedTokenChars[Token.Type.NUMERIC_LITERAL] += "ABCDEF";
          }
        }
        if ( !string.IsNullOrEmpty( Dialect.BinPrefix ) )
        {
          if ( !AllowedTokenStartChars[Token.Type.NUMERIC_LITERAL].Contains( Dialect.BinPrefix ) )
          {
            AllowedTokenStartChars[Token.Type.NUMERIC_LITERAL] += Dialect.BinPrefix;
          }
        }
      }

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
      AddActionToken( TokenValue.FLASH_ON, "{flash on}", 194 );
      AddActionToken( TokenValue.FLASH_OFF, "{flash off}", 196 );
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
      m_CompileTarget     = new CompileTarget();
      m_CompileTargetFile = null;

      AssembledOutput = null;
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



    private void CreatePreProcessedFile( string SourceFile, string[] Lines, Types.ASM.FileInfo FileInfo )
    {
      try
      {
        if ( Lines == null )
        {
          return;
        }
        string pathLog = GR.Path.RenameExtension( SourceFile, ".dump" );

        using ( var writer = System.IO.File.CreateText( pathLog ) )
        {
          int     numLineDigits = Lines.Length.ToString().Length;

          string  formatString = "D" + numLineDigits.ToString();

          for ( int i = 0; i < Lines.Length; ++i )
          {
            if ( FileInfo.LineInfo.ContainsKey( i ) )
            {
              var     info = FileInfo.LineInfo[i];
              if ( info != null )
              {
                if ( info.HideInPreprocessedOutput )
                {
                  continue;
                }

                writer.Write( i.ToString( formatString ) );
                writer.Write( "  " );
                if ( info.AddressStart < 0 )
                {
                  writer.Write( " ----" );
                }
                else
                {
                  writer.Write( "$" + info.AddressStart.ToString( "X4" ) );
                }

                writer.Write( "  " );
                if ( info.LineData != null )
                {
                  int     numBytesToPrint = (int)info.LineData.Length;
                  int     numLettersPrinted = 0;

                  for ( int j = 0; j < numBytesToPrint; ++j )
                  {
                    writer.Write( info.LineData.ByteAt( j ).ToString( "X2" ) );
                    numLettersPrinted += 2;
                    if ( j + 1 < numBytesToPrint )
                    {
                      writer.Write( ' ' );
                      ++numLettersPrinted;
                    }
                  }
                  for ( int j = numLettersPrinted; j < 10; ++j )
                  {
                    writer.Write( ' ' );
                  }
                  //writer.Write( info.LineData.ToString() );
                }
                else
                {
                  writer.Write( "          " );
                }
                writer.Write( "  " );
                writer.WriteLine( Lines[i].TrimStart() );
              }
              else
              {
                writer.Write( i.ToString( formatString ) );
                writer.Write( "  " );

                writer.Write( "????              " );
                writer.WriteLine( Lines[i].TrimStart() );
              }
            }
            else
            {
              writer.Write( i.ToString( formatString ) );
              writer.Write( "  " );

              writer.Write( "????              " );
              writer.WriteLine( Lines[i].TrimStart() );
            }
          }
          writer.Close();
        }

        ParseMessage message = new ParseMessage( ParseMessage.LineType.MESSAGE, Types.ErrorCode.OK, "Preprocessed file written to " + pathLog );
        message.AlternativeFile = pathLog;
        message.AlternativeLineIndex = 0;
        m_ASMFileInfo.Messages.Add( -1, message );
        ++m_Messages;
      }
      catch ( Exception ex )
      {
        AddWarning( -1, Types.ErrorCode.E1401_INTERNAL_ERROR, "Can't write preprocessed file:" + ex.Message, 0, 0 );
      }
    }



    public override bool Parse( string Content, ProjectConfig Configuration, CompileConfig Config, string AdditionalPredefines, out Types.ASM.FileInfo ASMFileInfo )
    {
      m_ASMFileInfo = new Types.ASM.FileInfo();
      ASMFileInfo   = m_ASMFileInfo;

      m_CompileConfig = Config;

      if ( m_CompileConfig.TargetType == CompileTargetType.NONE )
      {
        m_CompileConfig.TargetType = Settings.BASICDialect.CompileType;
      }
      m_CompileTarget.Type = m_CompileConfig.TargetType;

      switch ( m_CompileTarget.Type )
      {
        case CompileTargetType.P_ZX81:
          m_DefaultTargetExtension = ".p";
          break;
        default:
          m_DefaultTargetExtension = ".prg";
          break;
      }

      m_LineInfos.Clear();

      if ( Settings.BASICDialect.Opcodes.Count == 0 )
      {
        AddError( -1, Types.ErrorCode.E1401_INTERNAL_ERROR, "An unsupported BASIC version '" + Settings.BASICDialect.Name + "' was chosen" );
        return false;
      }

      string[] lines = Content.Split( '\n' );

      CleanLines( lines );

      m_ASMFileInfo.Clear();
      IncludePreviousSymbols();

      var sourceInfo = new Types.ASM.SourceInfo();
      sourceInfo.Filename         = m_CompileConfig.InputFile;
      sourceInfo.GlobalStartLine  = 0;
      sourceInfo.LineCount        = lines.Length;
      sourceInfo.FullPath         = m_CompileConfig.InputFile;

      m_ASMFileInfo.SourceInfo.Clear();
      m_ASMFileInfo.SourceInfo.Add( sourceInfo.GlobalStartLine, sourceInfo );

      _ParseContext = new ParseContext()
      {
        Settings            = Settings,
        TargetType          = Config.TargetType,
        KeyboardMachineType = FindBestKeyboardMachineType( Settings.BASICDialect )
      };

      ProcessLines( lines, LabelMode );

      DumpSourceInfos();

      if ( m_ErrorMessages > 0 )
      {
        return false;
      }

      return true;
    }



    public static MachineType FindBestKeyboardMachineType( Dialect dialect )
    {
      foreach ( var machine in dialect.MachineTypes )
      {
        if ( ConstantData.AllPhysicalKeyInfos.ContainsKey( machine ) )
        {
          return machine;
        }
      }
      // fallback
      return MachineType.C64;
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
            &&   ( entry.Key.StartsWith( ASMFileParser.INTERNAL_LOCAL_LABEL_PREFIX ) ) )
            {
              // do not pass on internal local labels
              continue;
            }
            var symbol            = new SymbolInfo()
            {
              AddressOrValue    = entry.Value.AddressOrValue,
              DocumentFilename  = entry.Value.DocumentFilename, 
              LocalLineIndex    = entry.Value.LocalLineIndex,
              Name              = entry.Value.Name,
              Type              = entry.Value.Type,
              Zone              = entry.Value.Zone,
              FromDependency    = true, 
              Info              = entry.Value.Info
            };

            symbol.References.Add( entry.Value.LineIndex,
              new SymbolReference()
              {
                GlobalLineIndex = entry.Value.LineIndex,
                TokenInfo = new TokenInfo() { StartPos = entry.Value.CharIndex, Length = entry.Value.Length, OriginatingString = entry.Value.String }
              } );

            m_ASMFileInfo.Labels.Add( entry.Key, symbol );
          }
        }
      }
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
      if ( ( ( TokenValue >= 0x30 )
      &&     ( TokenValue <= 0x39 ) )
      ||   ( TokenValue == '.' ) )
      {
        // numeric, maybe combine with token before?
        if ( ( Info.Tokens.Count > 0 )
        &&   ( Info.Tokens[Info.Tokens.Count - 1].TokenType == Token.Type.NUMERIC_LITERAL )
        &&   ( Info.Tokens[Info.Tokens.Count - 1].ByteValue >= 0x30 )
        &&   ( Info.Tokens[Info.Tokens.Count - 1].ByteValue <= 0x39 ) )
        {
          if ( ( ( TokenValue == '.' )
          &&     ( Info.Tokens[Info.Tokens.Count - 1].Content.IndexOf( '.' ) == -1 ) )
          ||   ( TokenValue != '.' ) )
          {
            // attach to previous token
            Info.Tokens[Info.Tokens.Count - 1].Content += ConstantData.PetSCIIToChar[TokenValue].CharValue;
            Info.Tokens[Info.Tokens.Count - 1].TokenType = Token.Type.NUMERIC_LITERAL;

            Info.LineData.AppendU8( MapTokenToByteValue( TokenValue ) );
            return;
          }
        }
        Token numericToken = new Token()
        {
          TokenType  = Token.Type.NUMERIC_LITERAL,
          ByteValue  = TokenValue,
          Content    = "" + ConstantData.PetSCIIToChar[TokenValue].CharValue,
          StartIndex = StartIndex
        };

        Info.Tokens.Add( numericToken );

        Info.LineData.AppendU8( MapTokenToByteValue( TokenValue ) );
        return;
      }

      if ( Info.Tokens.Count > 0 )
      {
        AssembleTokenCompleted( Info );
      }

      Token basicToken = new Token();
      basicToken.TokenType = Token.Type.DIRECT_TOKEN;
      basicToken.ByteValue = TokenValue;
      basicToken.Content = "" + ConstantData.PetSCIIToChar[TokenValue].CharValue;
      basicToken.StartIndex = StartIndex;
      Info.Tokens.Add( basicToken );

      Info.LineData.AppendU8( MapTokenToByteValue( TokenValue ) );
    }



    private string TokensToExpression( List<Token> Tokens, int StartIndex, int Count )
    {
      var sb = new StringBuilder();

      for ( int i = 0; i < Count; ++i )
      {
        sb.Append( Tokens[StartIndex + i].Content );

        if ( ( i + 1 < Count )
        &&   ( Tokens[StartIndex + i].StartIndex + Tokens[StartIndex + i].Content.Length < Tokens[StartIndex + i + 1].StartIndex ) )
        {
          // requires spaces
          int numSpaces = Tokens[StartIndex + i + 1].StartIndex - ( Tokens[StartIndex + i].StartIndex + Tokens[StartIndex + i].Content.Length );
          for ( int j = 0; j < numSpaces; ++j )
          {
            sb.Append( ' ' );
          }
        }
      }
      return sb.ToString();
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
      bool      insideMacro = false;
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
        else if ( insideMacro )
        {
          if ( nextByte == '}' )
          {
            insideMacro = false;
            currentToken.Content = Line.Substring( tokenStartPos, posInLine - tokenStartPos + 1 );
            currentToken = null;

            tokenStartPos = posInLine + 1;
          }
          ++posInLine;
          continue;
        }

        if ( nextByte == '{' )
        {
          insideMacro = true;
          if ( ( currentToken != null )
          &&   ( currentToken.TokenType != Token.Type.MACRO ) )
          {
            // end of previous token
            currentToken.Content = Line.Substring( tokenStartPos, posInLine - tokenStartPos );
            currentToken = null;
          }

          currentToken = new Token();
          currentToken.TokenType = Token.Type.MACRO;
          currentToken.StartIndex = posInLine;

          tokenStartPos = posInLine;
          lineInfo.Tokens.Add( currentToken );

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
          &&   ( IsBASICTokenStartingHere( Line, posInLine, out string Token, out int tokenValue ) ) )
          {
            if ( currentToken != null )
            {
              currentToken.Content = Line.Substring( tokenStartPos, posInLine - tokenStartPos );
              currentToken = null;
            }
            var basicToken = new Token();
            basicToken.TokenType = BasicFileParser.Token.Type.BASIC_TOKEN;
            basicToken.StartIndex = posInLine;
            basicToken.Content = Token;
            basicToken.ByteValue = tokenValue;

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
              break;
              //return lineInfo;
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
            if ( ( currentToken.TokenType == BasicFileParser.Token.Type.NUMERIC_LITERAL )
            &&   ( Line.Substring( currentToken.StartIndex, posInLine - currentToken.StartIndex ).Contains( "." ) )
            &&   ( nextByte >= 'A' )
            &&   ( nextByte <= 'F' ) )
            {
              // in case of hex literals they are not allowed after a dot!
            }
            else if ( AllowedTokenChars[currentToken.TokenType].IndexOf( nextByte ) != -1 )
            {
              ++posInLine;
              continue;
            }
            // char is not allowed in this token!
            // the last char of a token
            currentToken.Content = Line.Substring( tokenStartPos, posInLine - tokenStartPos );
            currentToken = null;
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

            numericToken.TokenType = Token.Type.DIRECT_TOKEN;
            numericToken.StartIndex = posInLine;
            numericToken.Content = "" + nextByte;
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

        if ( nextByte == ' ' )
        {
          Token numericToken      = new Token();

          numericToken.TokenType = Token.Type.DIRECT_TOKEN;
          numericToken.StartIndex = posInLine;
          numericToken.Content = " ";
          numericToken.ByteValue = 0x20;
          lineInfo.Tokens.Add( numericToken );
          tokenStartPos = posInLine + 1;
        }

        ++posInLine;
        continue;
      }

      if ( currentToken != null )
      {
        currentToken.Content = Line.Substring( tokenStartPos );
      }

      // sanitize
      SanitizeTokens( lineInfo );
      return lineInfo;
    }



    private void SanitizeTokens( LineInfo lineInfo )
    {
      if ( ( lineInfo.Tokens.Count > 0 )
      &&   ( !LabelMode )
      &&   ( lineInfo.Tokens[0].TokenType == Token.Type.NUMERIC_LITERAL ) )
      {
        lineInfo.Tokens[0].TokenType = Token.Type.LINE_NUMBER;
        // line numbers are only decimal numbers, split non integer parts off
        for ( int pos = 0; pos < lineInfo.Tokens[0].Content.Length; ++pos )
        {
          if ( !char.IsNumber( lineInfo.Tokens[0].Content[pos] ) )
          {
            var otherToken              = new Token() { TokenType = Token.Type.DIRECT_TOKEN };
            otherToken.Content          = lineInfo.Tokens[0].Content.Substring( pos );
            otherToken.StartIndex       = lineInfo.Tokens[0].StartIndex + pos;
            lineInfo.Tokens[0].Content  = lineInfo.Tokens[0].Content.Substring( 0, pos );
            lineInfo.Tokens.Insert( 1, otherToken );
            break;
          }
        }
      }

      if ( Settings.BASICDialect.HasTextLabels )
      {
        // variables after a token, or a separator and at the end or directly in front of an separator are labels (we hope)
        int   startIndex = 1;
        if ( LabelMode )
        {
          startIndex = 0;
        }
        for ( int i = startIndex; i < lineInfo.Tokens.Count; i++ )
        {
          int   prevTokenIndex = FindPrevToken( lineInfo.Tokens, i );
          // previous token found the line number
          if ( ( prevTokenIndex != -1 )
          &&   ( prevTokenIndex < startIndex ) )
          {
            prevTokenIndex = -1;
          }
          int   nextTokenIndex = FindNextToken( lineInfo.Tokens, i );

          // a variable
          if ( ( ( lineInfo.Tokens[i].TokenType == Token.Type.VARIABLE )
          ||     ( lineInfo.Tokens[i].Content == "." ) )

          // at the start of a statement (no EXEC/PROC)
          &&   ( ( i == startIndex )
          ||     ( prevTokenIndex == -1 )
          ||     ( ( prevTokenIndex != -1 )
          &&       ( lineInfo.Tokens[prevTokenIndex].TokenType == Token.Type.BASIC_TOKEN )
          &&       ( IsPreLabelToken( lineInfo.Tokens[prevTokenIndex] ) ) )
          ||     ( ( prevTokenIndex != -1 )
          &&       ( lineInfo.Tokens[prevTokenIndex].Content == ":" ) ) )

          // no assignment, so probably really a label
          &&   ( ( nextTokenIndex == -1 )
          ||     ( lineInfo.Tokens[nextTokenIndex].Content != "=" ) ) )
          {
            // look up until next separator
            int   lastValidTokenIndex = i;
            bool  notALabel = false;

            while ( ( lastValidTokenIndex + 1 < lineInfo.Tokens.Count )
            &&      ( lineInfo.Tokens[lastValidTokenIndex + 1].Content != ":" ) )
            {
              if ( lineInfo.Tokens[lastValidTokenIndex + 1].Content == "=" )
              {
                // an set expression!
                notALabel = true;
                break;
              }
              ++lastValidTokenIndex;
            }

            if ( !notALabel )
            {
              string  fullLabel = TokensToExpression( lineInfo.Tokens, i, lastValidTokenIndex - i + 1 );

              lineInfo.Tokens[i].TokenType = Token.Type.TEXT_LABEL;
              lineInfo.Tokens[i].Content = fullLabel;

              lineInfo.Tokens.RemoveRange( i + 1, lastValidTokenIndex - i );

              ++i;
            }
          }
        }
      }

      UpdateLineNumberReferences( lineInfo );
      for ( int i = 0; i < lineInfo.Tokens.Count; ++i )
      {
        var token = lineInfo.Tokens[i];
        if ( ( token.TokenType == Token.Type.VARIABLE )
        &&   ( token.Content.StartsWith( "LABEL" ) )
        &&   ( token.Content.Length > 5 )
        &&   ( char.IsDigit( token.Content[5] ) ) )
        {

          token.TokenType = Token.Type.EX_BASIC_TOKEN;
          token.ByteValue = 240;

          var numberToken = new Token()
          {
            Content = token.Content.Substring( 5 ),
            TokenType = Token.Type.NUMERIC_LITERAL,
            StartIndex = token.StartIndex + 5
          };
          lineInfo.Tokens.Insert( i + 1, numberToken );
          token.Content = token.Content.Substring( 0, 5 );
        }
      }
    }



    private bool IsPreLabelToken( Token Token )
    {
      if ( !Settings.BASICDialect.OpcodesFromByte.ContainsKey( (ushort)Token.ByteValue ) )
      {
        return false;
      }
      return Settings.BASICDialect.OpcodesFromByte[(ushort)Token.ByteValue].IsPreLabelToken;
    }



    private bool IsBASICTokenStartingHere( string Line, int PosInLine, out string Token, out int TokenValue )
    {
      // find best match
      Token = null;
      TokenValue = -1;
      foreach ( var token in Settings.BASICDialect.Opcodes.Keys )
      {
        if ( ( !Settings.BASICDialect.Opcodes[token].ReverseOnly )
        &&   ( PosInLine + token.Length <= Line.Length )
        &&   ( string.Compare( Line, PosInLine, token, 0, token.Length ) == 0 ) )
        {
          Token = token;
          TokenValue = Settings.BASICDialect.Opcodes[token].InsertionValue;
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
      bool      insideDataStatementHadNonSpace = false;
      bool      insideREMStatement = false;
      int       macroStartPos = -1;
      GR.Memory.ByteBuffer tempData = new GR.Memory.ByteBuffer();

      int numCharsSkipped = TranslateCharactersToPETSCII( Line, LineIndex, endOfDigitPos, ref posInLine, ref insideMacro, ref macroStartPos, tempData );

      string collapsedText = CollapseTokens( Line, Settings.BASICDialect, !Settings.UseC64Font );
      if ( collapsedText.Length + endOfDigitPos + 1 > Settings.BASICDialect.SafeLineLength )
      //if ( tempData.Length + endOfDigitPos + 1 > Settings.BASICDialect.SafeLineLength )
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
            if ( Settings.BASICDialect.ExtendedTokensRecognizedInsideComment )
            {
              // check for extended tokens
              bool entryFound = DeterminePotentialTokenAtLocation( tempData, bytePos, out Opcode potentialOpcode );
              if ( ( entryFound )
              &&   ( potentialOpcode.InsertionValue > 255 ) )
              {
                // it's an extended token!
                if ( info.Tokens.Count > 0 )
                {
                  AssembleTokenCompleted( info );
                }
                info.LineData.AppendU16NetworkOrder( (ushort)potentialOpcode.InsertionValue );

                Token basicToken = new Token();
                basicToken.TokenType = Token.Type.BASIC_TOKEN;
                basicToken.ByteValue = potentialOpcode.InsertionValue;
                basicToken.Content = potentialOpcode.Command;
                basicToken.StartIndex = bytePos;
                info.Tokens.Add( basicToken );

                bytePos += potentialOpcode.Command.Length;
                continue;
              }
            }

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
            if ( ( !Settings.StripSpaces )
            ||   ( ( insideDataStatement )
            &&     ( insideDataStatementHadNonSpace ) ) )
            {
              AddDirectToken( info, nextByte, bytePos );
            }
            ++bytePos;
            continue;
          }

          // is there a token now?
          if ( ( !insideDataStatement )
          &&   ( FindOpcode( tempData, ref bytePos, info, ref insideDataStatement, ref insideREMStatement ) ) )
          {
            if ( insideREMStatement )
            {
              if ( Settings.StripREM )
              {
                // break out so we update references
                break;
              }

              if ( info.Tokens.Count > 0 )
              {
                AssembleTokenCompleted( info );
              }

              Token basicToken = new Token();
              basicToken.TokenType = Token.Type.COMMENT;
              basicToken.StartIndex = bytePos;

              while ( bytePos < tempData.Length )
              {
                basicToken.Content += ConstantData.PetSCIIToChar[tempData.ByteAt( bytePos )].CharValue;
                info.LineData.AppendU8( tempData.ByteAt( bytePos ) );

                ++bytePos;
              }

              info.Tokens.Add( basicToken );
            }
            continue;
          }
          if ( nextByte == 34 )
          {
            if ( info.Tokens.Count > 0 )
            {
              AssembleTokenCompleted( info );
            }

            // Anführungszeichen, abschliessendes Anführungszeichen suchen
            string stringLiteral = "";
            int     startIndex = bytePos;
            do
            {
              if ( m_CompileConfig.DoNotExpandStringLiterals )
              {
                stringLiteral += (char)nextByte;
              }
              else
              {
                var c64Key = FindKeyByCharacter( nextByte );
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
              }
              info.LineData.AppendU8( MapTokenToByteValue( nextByte ) );
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
              info.LineData.AppendU8( MapTokenToByteValue( nextByte ) );
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
            if ( nextByte != ' ' )
            {
              insideDataStatementHadNonSpace = true;
            }
            if ( nextByte == ',' )
            {
              insideDataStatementHadNonSpace = false;
            }

            AddDirectToken( info, nextByte, bytePos );
            ++bytePos;
            continue;
          }
          // TODO - that is commodore special!
          if ( nextByte == 0x3f )
          {
            if ( info.Tokens.Count > 0 )
            {
              AssembleTokenCompleted( info );
            }

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
                if ( info.Tokens.Count > 0 )
                {
                  AssembleTokenCompleted( info );
                }

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
                insideDataStatement = ( foundOpcode.Command == "DATA" );
                if ( insideDataStatement )
                {
                  insideDataStatementHadNonSpace = false;
                }

                if ( insideREMStatement )
                {
                  Token basicToken = new Token();
                  basicToken.TokenType = Token.Type.COMMENT;
                  basicToken.StartIndex = bytePos;

                  while ( bytePos < tempData.Length )
                  {
                    basicToken.Content += ConstantData.PetSCIIToChar[tempData.ByteAt( bytePos )].CharValue;
                    info.LineData.AppendU8( tempData.ByteAt( bytePos ) );

                    ++bytePos;
                  }

                  info.Tokens.Add( basicToken );
                  continue;
                }
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

      if ( info.Tokens.Count > 0 )
      {
        AssembleTokenCompleted( info );
      }

      // DATA - last entry trims spaces at end
      if ( ( Settings.StripSpaces )
      &&   ( insideDataStatement ) )
      {
        while ( ( info.Tokens.Count > 0 )
        &&      ( info.Tokens.Last().TokenType == Token.Type.DIRECT_TOKEN )
        &&      ( info.Tokens.Last().Content == " " ) )
        {
          info.Tokens.RemoveAt( info.Tokens.Count - 1 );
          info.LineData.Resize( info.LineData.Length - 1 );
        }
      }

      if ( info.LineData.Length > _ParseContext.Settings.BASICDialect.MaxLineLength )
      {
        AddError( LineIndex, Types.ErrorCode.E3006_BASIC_LINE_TOO_LONG, $"Line is too long, max. {_ParseContext.Settings.BASICDialect.MaxLineLength} bytes possible" );
      }

      UpdateLineNumberReferences( info );
      
      // offset by line number
      for ( int i = 1; i < info.Tokens.Count; ++i )
      {
        info.Tokens[i].StartIndex += endOfDigitPos + 1 + numCharsSkipped;
      }
      return info;
    }



    private void UpdateLineNumberReferences( LineInfo info )
    {
      // update line number references
      for ( int i = 0; i < info.Tokens.Count; ++i )
      {
        Token token = info.Tokens[i];

        if ( ( token.TokenType == Token.Type.BASIC_TOKEN )
        &&   ( Settings.BASICDialect.Opcodes.ContainsKey( token.Content ) )
        &&   ( Settings.BASICDialect.Opcodes[token.Content].AllowsSeveralLineNumbers ) )
        {
          // e.g. GOTO, GOSUB
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
        &&   ( Settings.BASICDialect.Opcodes.ContainsKey( token.Content ) )
        &&   ( Settings.BASICDialect.Opcodes[token.Content].GoTokenToMayFollow ) )
        {
          int nextTokenIndex = FindNextToken( info.Tokens, i );
          if ( ( nextTokenIndex != -1 )
          &&   ( info.Tokens[nextTokenIndex].TokenType == Token.Type.BASIC_TOKEN )
          &&   ( info.Tokens[nextTokenIndex].Content == "TO" ) )
          {
            // GO, followed by TO has one line number
            nextTokenIndex = FindNextToken( info.Tokens, nextTokenIndex );
            if ( ( nextTokenIndex != -1 )
            &&   ( info.Tokens[nextTokenIndex].TokenType == Token.Type.NUMERIC_LITERAL ) )
            {
              int referencedLineNumber = GR.Convert.ToI32( info.Tokens[nextTokenIndex].Content );
              info.ReferencedLineNumbers.Add( referencedLineNumber );
            }
          }
        }

        if ( ( token.TokenType == Token.Type.BASIC_TOKEN )
        &&   ( Settings.BASICDialect.Opcodes.ContainsKey( token.Content ) )
        &&   ( Settings.BASICDialect.Opcodes[token.Content].LineListRange ) )
        {
          int   nextTokenIndex =  FindNextToken( info.Tokens, i );
          if ( nextTokenIndex != -1 )
          {
            if ( info.Tokens[nextTokenIndex].TokenType == Token.Type.NUMERIC_LITERAL )
            {
              int referencedLineNumber = GR.Convert.ToI32( info.Tokens[nextTokenIndex].Content );

              info.ReferencedLineNumbers.Add( referencedLineNumber );

              nextTokenIndex = FindNextToken( info.Tokens, nextTokenIndex );
              if ( nextTokenIndex != -1 )
              {
                if ( info.Tokens[nextTokenIndex].Content == "-" )
                {
                  // a to part
                  nextTokenIndex = FindNextToken( info.Tokens, nextTokenIndex );
                  if ( nextTokenIndex != -1 )
                  {
                    if ( info.Tokens[nextTokenIndex].TokenType == Token.Type.NUMERIC_LITERAL )
                    {
                      referencedLineNumber = GR.Convert.ToI32( info.Tokens[nextTokenIndex].Content );
                      info.ReferencedLineNumbers.Add( referencedLineNumber );
                    }
                  }
                }
              }
            }
            else if ( info.Tokens[nextTokenIndex].Content == "-" )
            {
              // a to part
              nextTokenIndex = FindNextToken( info.Tokens, nextTokenIndex );
              if ( nextTokenIndex != -1 )
              {
                if ( info.Tokens[nextTokenIndex].TokenType == Token.Type.NUMERIC_LITERAL )
                {
                  int referencedLineNumber = GR.Convert.ToI32( info.Tokens[nextTokenIndex].Content );

                  info.ReferencedLineNumbers.Add( referencedLineNumber );
                }
              }
            }
          }
        }

        if ( ( token.TokenType == Token.Type.BASIC_TOKEN )
        &&   ( Settings.BASICDialect.Opcodes.ContainsKey( token.Content ) )
        &&   ( Settings.BASICDialect.Opcodes[token.Content].ArgumentIndexOfExpectedLineNumber >= 0 ) )
        {
          // only one line number
          // look up next token, is it a line number? (spaces are ignored)
          int argIndex = Settings.BASICDialect.Opcodes[token.Content].ArgumentIndexOfExpectedLineNumber;
          int nextTokenIndex = FindNextToken( info.Tokens, i );
          while ( ( nextTokenIndex != -1 )
          &&      ( nextTokenIndex < info.Tokens.Count ) )
          {
            if ( argIndex == 0 )
            {
              if ( ( info.Tokens[nextTokenIndex].TokenType == Token.Type.DIRECT_TOKEN )
              &&   ( info.Tokens[nextTokenIndex].Content == " " ) )
              {
                ++nextTokenIndex;
                continue;
              }
              if ( info.Tokens[nextTokenIndex].TokenType == Token.Type.NUMERIC_LITERAL )
              {
                int referencedLineNumber = GR.Convert.ToI32( info.Tokens[nextTokenIndex].Content );

                info.ReferencedLineNumbers.Add( referencedLineNumber );
              }
              break;
            }
            else if ( info.Tokens[nextTokenIndex].Content == ":" )
            {
              break;
            }
            else if ( info.Tokens[nextTokenIndex].Content == "," )
            {
              --argIndex;
            }
            ++nextTokenIndex;
          }
        }
      }
    }



    private bool ProcessMetaData( int LineIndex, string MetaData, LineInfo lineInfo, out bool hadError )
    {
      int     sepPos = MetaData.IndexOf( ':' );
      string  metaDataType;
      string  metaDataParams = "";
      hadError = false;
      if ( sepPos == -1 )
      {
        metaDataType = MetaData;
      }
      else
      {
        metaDataType = MetaData.Substring( sepPos + 1 );
      }
      sepPos = metaDataType.IndexOf( ' ' );
      if ( sepPos != -1 )
      {
        metaDataParams = metaDataType.Substring( sepPos + 1 ).Trim();
        metaDataType = metaDataType.Substring( 0, sepPos );
      }

      if ( m_CompileConfig.DoNotExpandStringLiterals )
      {
        return true;
      }

      switch ( metaDataType.ToUpper() )
      {
        case "RETRODEVSTUDIO.METADATA.BASIC":
        case "C64STUDIO.METADATA.BASIC":
          // ok, but nothing to do here
          return true;
        case "INCLUDE":
          if ( !MetaDataInclude( LineIndex, MetaData, metaDataParams ) )
          {
            hadError = true;
          }
          // include ends up with an empty line
          lineInfo.Tokens.Clear();
          return true;
        case "BINDATA":
          if ( !MetaDataBinaryData( LineIndex, MetaData, metaDataParams ) )
          {
            hadError = true;
          }
          return true;
        case "SPRITEDATA":
          if ( !MetaDataSpriteData( LineIndex, MetaData, metaDataParams ) )
          {
            hadError = true;
          }
          return true;
        case "CHARDATA":
          if ( !MetaDataCharData( LineIndex, MetaData, metaDataParams ) )
          {
            hadError = true;
          }
          return true;
      }
      return false;
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

      if ( m_CompileConfig.DoNotExpandStringLiterals )
      {
        for ( int i = posInLine; i < Line.Length; ++i )
        {
          ++numCharsSkipped;
          ++posInLine;
          tempData.AppendU8( (byte)Line[i] );
        }
        return numCharsSkipped;
      }

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
            bool  foundMacro = false;

            macro = DetermineMacroCount( macro, out macroCount );
            if ( macro.Length == 0 )
            {
              // the macro was a pure number, replace with PETSCII char
              byte  petsciiValue = (byte)macroCount;
              if ( ( petsciiValue >= 192 )
              &&   ( petsciiValue <= 254 ) )
              {
                petsciiValue -= 64;
              }
              else if ( petsciiValue == 255 )
              {
                petsciiValue = 126;
              }

              tempData.AppendU8( petsciiValue );
              foundMacro = true;
            }
            if ( !foundMacro )
            {
              foreach ( var key in ConstantData.AllPhysicalKeyInfos[_ParseContext.KeyboardMachineType] )
              {
                if ( key.Replacements.Contains( macro ) )
                {
                  for ( int i = 0; i < macroCount; ++i )
                  {
                    tempData.AppendU8( key.NativeValue );
                  }
                  foundMacro = true;
                  break;
                }
              }
            }
            if ( !foundMacro )
            {
              // a inbuilt expression?
              if ( macro.StartsWith( "DATE" ) )
              {
                string    details = "yyyy-MM-dd";
                int       sepPos = macro.IndexOf( ':' );
                if ( sepPos != -1 )
                {
                  details = Line.Substring( macroStartPos + 1, posInLine - macroStartPos - 1 ).Substring( sepPos + 1 );
                  if ( string.IsNullOrEmpty( details ) )
                  {
                    details = "yyyy-MM-dd";
                  }
                }
                DateTime today = DateTime.Now;

                string    result = today.ToString( details );
                for ( int i = 0; i < result.Length; ++i )
                {
                  tempData.AppendU8( (byte)result[i] );
                }

                foundMacro = true;
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
                if ( m_ASMFileInfo.Labels.ContainsKey( macro ) )
                {
                  string value = m_ASMFileInfo.Labels[macro].AddressOrValue.ToString();

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
          if ( !foundKey.HasNative )
          {
            AddError( LineIndex, Types.ErrorCode.E3002_BASIC_UNSUPPORTED_CHARACTER, "Unsupported character " + (int)curChar + " encountered",
                      posInLine, 1 );
          }
          else
          {
            if ( ( curChar != 32 )
            ||   ( posInLine > endOfDigitPos + 1 ) )
            {
              // strip spaces after line numbers

              // TODO - lower case petscii!
              tempData.AppendU8( foundKey.LowerCaseNative );
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
      ||   ( !ConstantData.CharToC64Char[curChar].HasNative ) )
      {
        AddError( LineIndex, Types.ErrorCode.E3002_BASIC_UNSUPPORTED_CHARACTER, "Unsupported character " + (int)curChar + " encountered", posInLine, 1 );
      }
      else
      {
        byte    petsciiValue = ConstantData.CharToC64Char[curChar].NativeValue;

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



    internal static bool IsValidChar( bool UpperCaseMode, char Character )
    {
      if ( !UpperCaseMode )
      {
        // lower case mode
        if ( ConstantData.LowerCaseCharTo64Char.ContainsKey( Character ) )
        {
          return ConstantData.LowerCaseCharTo64Char[Character].HasNative;
        }
      }
      if ( ( !ConstantData.CharToC64Char.ContainsKey( Character ) )
      ||   ( !ConstantData.CharToC64Char[Character].HasNative ) )
      {
        return false;
      }
      return true;
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
          var msg = AddError( LineIndex,
                    Types.ErrorCode.E3001_BASIC_INVALID_LINE_NUMBER,
                    "Line number not increasing, must be higher than the previous line number",
                    lineNumberToken.StartIndex, lineNumberToken.Content.Length );
          msg.AddMessage( $"Previous line number was {LastLineNumber}, new line number is {info.LineNumber}",
                          _LastLineNumberDocument, _LastLineNumberDocLineIndex );
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

      bool entryFound = DeterminePotentialTokenAtLocation( TempData, BytePos, out Opcode potentialOpcode );
      if ( potentialOpcode != null )
      {
        if ( Info.Tokens.Count > 0 )
        {
          AssembleTokenCompleted( Info );
        }

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
        if ( potentialOpcode.IsComment )
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
          if ( Info.Tokens.Count > 0 )
          {
            AssembleTokenCompleted( Info );
          }

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



    private bool DeterminePotentialTokenAtLocation( ByteBuffer TempData, int BytePos, out Opcode PotentialOpcode )
    {
      bool entryFound = false;
      PotentialOpcode = null;

      foreach ( var opcodeEntry in Settings.BASICDialect.Opcodes )
      {
        Opcode  opcode = opcodeEntry.Value;
        if ( opcode.ReverseOnly )
        {
          continue;
        }

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
          if ( ( PotentialOpcode == null )
          ||   ( opcode.Command.Length > PotentialOpcode.Command.Length ) )
          {
            PotentialOpcode = opcode;
          }
        }
      }
      return PotentialOpcode != null;
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
      _Lines = lines;

      int lineIndex                   = -1;
      int lastLineNumber              = -1;

      while ( lineIndex + 1 < _Lines.Length )
      {
        ++lineIndex;

        string    lineArg = _Lines[lineIndex];

        ProcessLine( lineIndex, lineArg, ref lastLineNumber );
      }

      CheckForAmbigiousVariables();
      CheckForMissingReferencedLineNumbers();
    }



    private void ProcessLine( int lineIndex, string lineArg, ref int lastLineNumber )
    {
      if ( lineArg.Length == 0 )
      {
        return;
      }
      string line = lineArg.Trim();
      if ( line.Length == 0 )
      {
        return;
      }

      if ( !Settings.UpperCaseMode )
      {
        line = MakeUpperCase( line, !Settings.UseC64Font );
      }

      var info = TokenizeLine( line, lineIndex, ref lastLineNumber );

      var pureInfo = PureTokenizeLine( line );
      pureInfo.LineNumber = lastLineNumber;
      pureInfo.LineIndex  = lineIndex;
      if ( LabelMode )
      {
        pureInfo.ReferencedLineNumbers.Clear();
      }

      // remember last line source if a line number was present
      if ( ( pureInfo.Tokens.Count > 0 )
      &&   ( pureInfo.Tokens[0].TokenType == Token.Type.LINE_NUMBER ) )
      {
        if ( LabelMode )
        {
          AddError( lineIndex, ErrorCode.E3008_BASIC_DOES_NOT_MATCH_LABEL_MODE, "The code contains line numbers, but label mode was expected" );
          return;
        }
        m_ASMFileInfo.FindTrueLineSource( lineIndex, out _LastLineNumberDocument, out _LastLineNumberDocLineIndex );
      }

      if ( ( pureInfo.Tokens.Count == 1 )
      &&   ( pureInfo.Tokens[0].TokenType == Token.Type.HARD_COMMENT ) )
      {
        if ( ProcessMetaData( lineIndex, pureInfo.Tokens[0].Content.Substring( 1 ), pureInfo, out bool hadError ) )
        {
          if ( hadError )
          {
            return;
          }
          if ( pureInfo.Tokens.Count == 0 )
          {
            return;
          }
        }
      }

      int   tokenIndex = 0;
      bool  insideDataStatement = false;
      bool  insideStringLiteral = false;
      foreach ( var variable in pureInfo.Tokens )
      {
        if ( ( variable.TokenType == Token.Type.BASIC_TOKEN )
        &&   ( Settings.BASICDialect.OpcodesFromByte[(ushort)variable.ByteValue].Command == "DATA" ) )
        {
          insideDataStatement = true;
        }
        else if ( variable.Content == ":" )
        {
          if ( !insideStringLiteral )
          {
            insideDataStatement = false;
          }
        }
        else if ( variable.Content == "\"" )
        {
          insideStringLiteral = !insideStringLiteral;
        }

        if ( variable.TokenType == Token.Type.VARIABLE )
        {
          var     symbolType = SymbolInfo.Types.VARIABLE_NUMBER;
          string  varName = variable.Content;
          string  origName = varName;

          m_ASMFileInfo.OriginalVariables.Add( varName );

          // verify next token
          bool varNameCutShort = false;

          if ( variable.Content.EndsWith( "$" ) )
          {
            symbolType = SymbolInfo.Types.VARIABLE_STRING;
            if ( varName.Length > 3 )
            {
              // cut to signifact characters
              varName = varName.Substring( 0, 2 ) + "$";
              varNameCutShort = true;
            }
          }
          else if ( variable.Content.EndsWith( "%" ) )
          {
            symbolType = SymbolInfo.Types.VARIABLE_INTEGER;
            if ( varName.Length > 3 )
            {
              // cut to signifact characters
              varName = varName.Substring( 0, 2 ) + "%";
              varNameCutShort = true;
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
                varNameCutShort = true;
              }
              varName += "(";
            }
            else if ( varName.Length > 2 )
            {
              // cut to signifact characters
              varName = varName.Substring( 0, 2 );
              varNameCutShort = true;
            }
          }
          else
          {
            if ( varName.Length > 2 )
            {
              // cut to signifact characters
              varName = varName.Substring( 0, 2 );
              varNameCutShort = true;
            }
          }

          if ( ( origName != varName )
          &&   ( varNameCutShort )
          &&   ( !insideDataStatement ) )
          {
            if ( !m_ASMFileInfo.MappedVariables.ContainsKey( varName ) )
            {
              m_ASMFileInfo.MappedVariables.Add( varName, new List<SymbolInfo>() );
            }
            // name is cut short
            if ( !m_ASMFileInfo.Labels.ContainsKey( origName ) )
            {
              var symbolInfo              = new SymbolInfo();
              symbolInfo.AddressOrValue   = 0;
              symbolInfo.CharIndex        = variable.StartIndex;
              symbolInfo.Name             = varName;
              symbolInfo.DocumentFilename = m_CompileConfig.InputFile;
              symbolInfo.Length           = origName.Length;
              symbolInfo.LineIndex        = lineIndex;
              symbolInfo.LocalLineIndex   = lineIndex;
              symbolInfo.Type             = symbolType;
              symbolInfo.String           = origName;

              if ( !m_ASMFileInfo.MappedVariables[varName].Any( x => x.Name == origName ) )
              {
                m_ASMFileInfo.MappedVariables[varName].Add( symbolInfo );
              }
            }
          }

          if ( !m_ASMFileInfo.Labels.ContainsKey( varName ) )
          {
            var symbolInfo              = new SymbolInfo();
            symbolInfo.AddressOrValue   = 0;
            symbolInfo.CharIndex        = variable.StartIndex;
            symbolInfo.Name             = varName;
            symbolInfo.DocumentFilename = m_CompileConfig.InputFile;
            symbolInfo.Length           = varName.Length;
            symbolInfo.LineIndex        = lineIndex;
            symbolInfo.LocalLineIndex   = lineIndex;
            symbolInfo.Type             = symbolType;

            m_ASMFileInfo.Labels.Add( varName, symbolInfo );
          }
          var existingSymbolInfo = m_ASMFileInfo.Labels[varName];
          existingSymbolInfo.References.Add( lineIndex,
            new SymbolReference()
            {
              GlobalLineIndex = lineIndex,
              TokenInfo = new TokenInfo() { StartPos = variable.StartIndex, Length = variable.Content.Length, OriginatingString = info.Line }
            } );
            
        }
        else if ( variable.TokenType == Token.Type.TEXT_LABEL )
        {
          // PROC/EXEC labels
          var varName = variable.Content;
          if ( !m_ASMFileInfo.Labels.ContainsKey( varName ) )
          {
            var symbolInfo              = new SymbolInfo();
            symbolInfo.AddressOrValue   = 0;
            symbolInfo.CharIndex        = variable.StartIndex;
            symbolInfo.Name             = varName;
            symbolInfo.DocumentFilename = m_CompileConfig.InputFile;
            symbolInfo.Length           = varName.Length;
            symbolInfo.LineIndex        = lineIndex;
            symbolInfo.LocalLineIndex   = lineIndex;
            symbolInfo.Type             = SymbolInfo.Types.TEXT_LABEL;

            m_ASMFileInfo.Labels.Add( varName, symbolInfo );
          }
          var existingSymbolInfo = m_ASMFileInfo.Labels[varName];
          existingSymbolInfo.References.Add( lineIndex,
            new SymbolReference()
            {
              GlobalLineIndex = lineIndex,
              TokenInfo = new TokenInfo() { StartPos = variable.StartIndex, Length = variable.Content.Length, OriginatingString = info.Line }
            } );
        }

        ++tokenIndex;
      }

      if ( m_CompileConfig.DoNotExpandStringLiterals )
      {
        m_LineInfos[info.LineIndex] = pureInfo;
      }
      else
      {
        m_LineInfos[info.LineIndex] = info;
      }
    }



    public string RecalcCheckSum( string line, bool labelMode, string checkSummerClass )
    {
      ICheckSummer checkSummer = null;
      if ( string.IsNullOrEmpty( checkSummerClass ) )
      {
        return "";
      }
      checkSummer = (ICheckSummer)( Activator.CreateInstance( Assembly.GetExecutingAssembly().FullName, checkSummerClass ) ).Unwrap();

      int lastLineNumber = -1;
      if ( m_LineInfos.ContainsKey( -1 ) )
      {
        m_LineInfos[-1].Line = line;
        m_LineInfos[-1].LineNumber = -1;
        m_LineInfos[-1].LineData.Clear();
      }

      ProcessLine( -1, line, ref lastLineNumber );

      if ( !m_LineInfos.ContainsKey( -1 ) )
      {
        return "";
      }
      var info = m_LineInfos[-1];

      if ( info.LineNumber == -1 )
      {
        return "";
      }
      var bb = new ByteBuffer();
      bb.AppendU16( (ushort)info.LineNumber );
      bb += m_LineInfos[info.LineIndex].LineData;

      var checkSum = checkSummer.CheckSum( bb );
      //Debug.Log( $"CheckSum for {info.LineNumber} = {checkSum}" );

      return checkSum;
    }



    private void CheckForMissingReferencedLineNumbers()
    {
      var  allNumbers = m_LineInfos.Select( l => l.Value.LineNumber ).Where( l => l != -1 );
      foreach ( var info in m_LineInfos )
      {
        var  unmatchedLinenumbers = info.Value.ReferencedLineNumbers.Where( rl => !allNumbers.Contains( rl ) );
        foreach ( var lineNo in unmatchedLinenumbers )
        {
          AddSevereWarning( info.Key, ErrorCode.W1003_BASIC_REFERENCED_LINE_NUMBER_NOT_FOUND, $"Referencing missing line number {lineNo}" );
        }
      }
    }



    private void CheckForAmbigiousVariables()
    {
      if ( Settings.BASICDialect.VariableRelevantLength == -1 )
      {
        return;
      }

      GR.Collections.Set<string>   notifiedMappings = new GR.Collections.Set<string>();
      foreach ( var mappedVars in m_ASMFileInfo.MappedVariables )
      {
        var distinct = mappedVars.Value.Select( si => si.String ).Distinct();
        if ( distinct.Count() > 1 )
        {
          foreach ( var dist in distinct )
          {
            foreach ( var entry in mappedVars.Value )
            {
              if ( !notifiedMappings.ContainsValue( entry.Name + "_" + dist ) )
              {
                notifiedMappings.Add( entry.Name + "_" + dist );
                var warning = AddWarning( entry.LineIndex, Types.ErrorCode.W1002_BASIC_VARIABLE_POTENTIALLY_AMBIGUOUS, $"Variable name {entry.Name} truncated to {Settings.BASICDialect.VariableRelevantLength} characters is ambigious ({dist})",
                                          entry.CharIndex, entry.String.Length );
              }
            }
          }
        }
      }
    }



    public override bool Assemble( CompileConfig Config )
    {
      var result = new GR.Memory.ByteBuffer();

      _ParseContext.TargetType = Config.TargetType;

      int     startAddress = Config.StartAddress;
      if ( startAddress == -1 )
      {
        // TODO - C64 specific!
        startAddress = 0x0801;
      }

      ICheckSummer checkSummer = null;
      if ( !string.IsNullOrEmpty( Config.CheckSummerClass ) )
      {
        checkSummer = (ICheckSummer)( Activator.CreateInstance( Assembly.GetExecutingAssembly().FullName, Config.CheckSummerClass ) ).Unwrap();
      }

      AssembleHeader( result, startAddress );

      int     curAddress = startAddress;
      foreach ( LineInfo info in m_LineInfos.Values )
      {
        m_ASMFileInfo.LineInfo.Add( info.LineIndex, new Types.ASM.LineInfo() { LineIndex = info.LineIndex, AddressStart = curAddress } );

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
          m_ASMFileInfo.LineInfo[info.LineIndex].AddressStart = curAddress;

          uint   prevSize = result.Length;
          AssembleLine( result, curAddress, info );

          m_ASMFileInfo.LineInfo[info.LineIndex].LineData = result.SubBuffer( (int)prevSize );

          curAddress += (int)( result.Length - prevSize );
        }

        // build check sum
        if ( info.LineNumber != -1 )
        {
          var bb = new ByteBuffer();
          bb.AppendU16( (ushort)info.LineNumber );
          bb += m_LineInfos[info.LineIndex].LineData;

          var checkSum = "";
          if ( checkSummer != null )
          {
            checkSum = checkSummer.CheckSum( bb );
          }
          m_ASMFileInfo.LineInfo[info.LineIndex].CheckSum = checkSum;
        }
      }
      AssembleTrailer( result );

      int     originalSize = (int)result.Length - 2;

      string    outputPureFilename = "HURZ";
      try
      {
        outputPureFilename = GR.Path.GetFileNameWithoutExtension( Config.OutputFile );
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
        Formats.T64 t64 = new RetroDevStudio.Formats.T64();

        Formats.T64.FileRecord  record = new RetroDevStudio.Formats.T64.FileRecord();

        record.Filename = Util.ToFilename( Formats.MediaFilenameType.COMMODORE, outputPureFilename );
        record.StartAddress = (ushort)fileStartAddress;
        record.Data = result;

        t64.TapeInfo.Description = "C64S tape file\r\nDemo tape";
        t64.TapeInfo.UserDescription = "USERDESC";
        t64.FileRecords.Add( record );

        AssembledOutput.Assembly = t64.Compile();
      }
      else if ( Config.TargetType == Types.CompileTargetType.D64 )
      {
        Formats.D64 d64 = new RetroDevStudio.Formats.D64();

        d64.CreateEmptyMedia();

        GR.Memory.ByteBuffer    bufName = Util.ToFilename( Formats.MediaFilenameType.COMMODORE, outputPureFilename );
        d64.WriteFile( bufName, AssembledOutput.Assembly, FileTypeNative.COMMODORE_PRG );

        AssembledOutput.Assembly = d64.Compile();
      }
      else if ( Config.TargetType == Types.CompileTargetType.D81 )
      {
        Formats.D81 d81 = new RetroDevStudio.Formats.D81();

        d81.CreateEmptyMedia();

        GR.Memory.ByteBuffer    bufName = Util.ToFilename( Formats.MediaFilenameType.COMMODORE, outputPureFilename );
        d81.WriteFile( bufName, AssembledOutput.Assembly, FileTypeNative.COMMODORE_PRG );

        AssembledOutput.Assembly = d81.Compile();
      }
      else if ( Config.TargetType == Types.CompileTargetType.TAP )
      {
        Formats.Tap tap = new RetroDevStudio.Formats.Tap();

        tap.WriteFile( Util.ToFilename( Formats.MediaFilenameType.COMMODORE, outputPureFilename ), AssembledOutput.Assembly, FileTypeNative.COMMODORE_PRG );
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
          string name = GR.Path.GetFileNameWithoutExtension( m_CompileTargetFile ).ToUpper();

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

      if ( Config.CreatePreProcesseFile )
      {
        CreatePreProcessedFile( Config.InputFile, _Lines, m_ASMFileInfo );
      }
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
        if ( lineInfo.Value.Tokens.Count == 0 )
        {
          continue;
        }
        if ( m_ASMFileInfo.FindTrueLineSource( lineInfo.Key, out string dummy, out int dummy2 , out Types.ASM.SourceInfo sourceInfo ) )
        {
          if ( sourceInfo.Source == Types.ASM.SourceInfo.SourceInfoSource.MEDIA_INCLUDE )
          {
            // skip media includes, they are rebuilt
            continue;
          }
        }

        // remember cut off length so we can properly fill up with blanks below
        int   lineStartLength = sb.Length;
        int   lineLengthOffset = lineInfo.Value.Tokens[0].Content.Length;
        if ( lineInfo.Value.Tokens.Count > 1 )
        {
          lineLengthOffset = lineInfo.Value.Tokens[1].StartIndex;
        }

        if ( ( lineNumberReference.ContainsKey( lineInfo.Value.LineNumber ) )
        &&   ( lineInfo.Value.Tokens[0].TokenType != Token.Type.HARD_COMMENT ) )
        {
          // something is referencing this line
          if ( sb.Length > 0 )
          {
            sb.AppendLine();
          }
          sb.Append( lineNumberReference[lineInfo.Value.LineNumber] + "\r\n" );
        }
        for ( int i = 0; i < lineInfo.Value.Tokens.Count; ++i )
        {
          Token token = lineInfo.Value.Tokens[i];

          if ( ( i == 0 )
          &&   ( token.TokenType == Token.Type.LINE_NUMBER )
          &&   ( i + 1 < lineInfo.Value.Tokens.Count )
          &&   ( lineInfo.Value.Tokens[i + 1].TokenType == Token.Type.DIRECT_TOKEN )
          &&   ( lineInfo.Value.Tokens[i + 1].Content == " " ) )
          {
            // skip first space after line number
            ++i;
            continue;
          }

          if ( ( i == 0 )
          &&   ( token.TokenType == Token.Type.NUMERIC_LITERAL ) )
          {
            continue;
          }
          if ( token.TokenType == Token.Type.LINE_NUMBER )
          {
            continue;
          }
          if ( token.TokenType == Token.Type.BASIC_TOKEN )
          {
            if ( Settings.BASICDialect.Opcodes[token.Content].GoTokenToMayFollow )
            {
              int nextTokenIndex = FindNextToken( lineInfo.Value.Tokens, i );
              if ( ( nextTokenIndex != -1 )
              &&   ( lineInfo.Value.Tokens[nextTokenIndex].TokenType == Token.Type.BASIC_TOKEN )
              &&   ( lineInfo.Value.Tokens[nextTokenIndex].Content == "TO" ) )
              {
                // GO, followed by TO has one line number
                nextTokenIndex = FindNextToken( lineInfo.Value.Tokens, nextTokenIndex );
                if ( ( nextTokenIndex != -1 )
                &&   ( lineInfo.Value.Tokens[nextTokenIndex].TokenType == Token.Type.NUMERIC_LITERAL ) )
                {
                  if ( ( nextTokenIndex != -1 )
                  &&   ( int.TryParse( lineInfo.Value.Tokens[nextTokenIndex].Content, out int refNo ) ) )
                  {
                    var nextToken = lineInfo.Value.Tokens[nextTokenIndex];
                    int refNo1 = GR.Convert.ToI32( nextToken.Content );
                    if ( !m_LineInfos.Any( li => li.Value.LineNumber == refNo1 ) )
                    {
                      //AddError( lineInfo.Value.LineIndex, Types.ErrorCode.W1003_BASIC_REFERENCED_LINE_NUMBER_NOT_FOUND, $"Referenced unknown line number {refNo1}" );
                      continue;
                    }
                    if ( !lineNumberReference.ContainsKey( refNo1 ) )
                    {
                      Debug.Log( "Error, found linenumber without reference " + refNo1 + " in line " + lineInfo.Value.LineNumber );
                      continue;
                    }
                    sb.Append( token.Content );

                    AppendGapTokens( sb, lineInfo.Value.Tokens, ref i, nextTokenIndex );
                    sb.Append( lineNumberReference[refNo].ToString() );

                    i = nextTokenIndex;
                    continue;
                  }
                }
              }
            }
            if ( Settings.BASICDialect.Opcodes[token.Content].LineListRange )
            {
              sb.Append( token.Content );
              int   nextTokenIndex = FindNextToken( lineInfo.Value.Tokens, i );
              if ( nextTokenIndex != -1 )
              {
                AppendGapTokens( sb, lineInfo.Value.Tokens, ref i, nextTokenIndex );
                if ( lineInfo.Value.Tokens[nextTokenIndex].TokenType == Token.Type.NUMERIC_LITERAL )
                {
                  ++i;
                  int refNo1 = GR.Convert.ToI32( lineInfo.Value.Tokens[nextTokenIndex].Content );
                  if ( !m_LineInfos.Any( li => li.Value.LineNumber == refNo1 ) )
                  {
                    //AddError( lineInfo.Value.LineIndex, Types.ErrorCode.W1003_BASIC_REFERENCED_LINE_NUMBER_NOT_FOUND, $"Referenced unknown line number {refNo1}" );
                    continue;
                  }
                  if ( !lineNumberReference.ContainsKey( refNo1 ) )
                  {
                    Debug.Log( "Error, found linenumber without reference " + refNo1 + " in line " + lineInfo.Value.LineNumber );
                    continue;
                  }
                  sb.Append( lineNumberReference[refNo1].ToString() );

                  nextTokenIndex = FindNextToken( lineInfo.Value.Tokens, nextTokenIndex );
                  if ( nextTokenIndex != -1 )
                  {
                    if ( lineInfo.Value.Tokens[nextTokenIndex].Content == "-" )
                    {
                      AppendGapTokens( sb, lineInfo.Value.Tokens, ref i, nextTokenIndex );
                      // a to part
                      nextTokenIndex = FindNextToken( lineInfo.Value.Tokens, nextTokenIndex );
                      if ( nextTokenIndex != -1 )
                      {
                        if ( lineInfo.Value.Tokens[nextTokenIndex].TokenType == Token.Type.NUMERIC_LITERAL )
                        {
                          AppendGapTokens( sb, lineInfo.Value.Tokens, ref i, nextTokenIndex );
                          refNo1 = GR.Convert.ToI32( lineInfo.Value.Tokens[nextTokenIndex].Content );
                          if ( !m_LineInfos.Any( li => li.Value.LineNumber == refNo1 ) )
                          {
                            //AddError( lineInfo.Value.LineIndex, Types.ErrorCode.W1003_BASIC_REFERENCED_LINE_NUMBER_NOT_FOUND, $"Referenced unknown line number {refNo1}" );
                            continue;
                          }
                          if ( !lineNumberReference.ContainsKey( refNo1 ) )
                          {
                            Debug.Log( "Error, found linenumber without reference " + refNo1 + " in line " + lineInfo.Value.LineNumber );
                            continue;
                          }
                          sb.Append( lineNumberReference[refNo1].ToString() );
                          i = nextTokenIndex;
                        }
                      }
                    }
                  }
                  continue;
                }
                else if ( lineInfo.Value.Tokens[nextTokenIndex].Content == "-" )
                {
                  // a to part
                  AppendGapTokens( sb, lineInfo.Value.Tokens, ref i, nextTokenIndex );
                  nextTokenIndex = FindNextToken( lineInfo.Value.Tokens, nextTokenIndex );
                  if ( nextTokenIndex != -1 )
                  {
                    if ( lineInfo.Value.Tokens[nextTokenIndex].TokenType == Token.Type.NUMERIC_LITERAL )
                    {
                      AppendGapTokens( sb, lineInfo.Value.Tokens, ref i, nextTokenIndex );
                      int refNo1 = GR.Convert.ToI32( lineInfo.Value.Tokens[nextTokenIndex].Content );
                      if ( !m_LineInfos.Any( li => li.Value.LineNumber == refNo1 ) )
                      {
                        //AddError( lineInfo.Value.LineIndex, Types.ErrorCode.W1003_BASIC_REFERENCED_LINE_NUMBER_NOT_FOUND, $"Referenced unknown line number {refNo1}" );
                        continue;
                      }
                      if ( !lineNumberReference.ContainsKey( refNo1 ) )
                      {
                        Debug.Log( "Error, found linenumber without reference " + refNo1 + " in line " + lineInfo.Value.LineNumber );
                        continue;
                      }
                      sb.Append( lineNumberReference[refNo1].ToString() );
                    }
                  }
                }
              }
              ++i;
              continue;
            }
            if ( Settings.BASICDialect.Opcodes[token.Content].ArgumentIndexOfExpectedLineNumber >= 0 )
            {
              // insert label instead of line number
              int   argIndex = Settings.BASICDialect.Opcodes[token.Content].ArgumentIndexOfExpectedLineNumber;
              int nextIndex = i + 1;
              bool labelWasInserted = false;
              bool tokenWasInserted = false;
              while ( nextIndex < lineInfo.Value.Tokens.Count )
              {
                Token nextToken = lineInfo.Value.Tokens[nextIndex];
                if ( nextToken.TokenType == Token.Type.NUMERIC_LITERAL )
                {
                  if ( argIndex == 0 )
                  {
                    if ( !tokenWasInserted )
                    {
                      tokenWasInserted = true;
                      sb.Append( token.Content );
                    }
                    // numeric!
                    int refNo = GR.Convert.ToI32( nextToken.Content );
                    if ( !m_LineInfos.Any( li => li.Value.LineNumber == refNo ) )
                    {
                      //AddError( lineInfo.Value.LineIndex, Types.ErrorCode.W1003_BASIC_REFERENCED_LINE_NUMBER_NOT_FOUND, $"Referenced unknown line number {refNo}" );
                      break;
                    }
                    if ( !lineNumberReference.ContainsKey( refNo ) )
                    {
                      Debug.Log( "Error, found linenumber without reference " + refNo + " in line " + lineInfo.Value.LineNumber );
                      break;
                    }
                    sb.Append( lineNumberReference[refNo].ToString() );

                    i = nextIndex;
                    labelWasInserted = true;
                    break;
                  }
                  sb.Append( nextToken.Content );
                }
                else if ( ( nextToken.TokenType == Token.Type.DIRECT_TOKEN )
                &&        ( nextToken.Content == " " ) )
                {
                  if ( !tokenWasInserted )
                  {
                    tokenWasInserted = true;
                    sb.Append( token.Content );
                  }
                  sb.Append( nextToken.Content );
                  ++nextIndex;
                  continue;
                }
                else if ( nextToken.Content == "," )
                {
                  // comma after comma, is valid
                  if ( !tokenWasInserted )
                  {
                    tokenWasInserted = true;
                    sb.Append( token.Content );
                  }
                  --argIndex;
                  sb.Append( ',' );
                  ++nextIndex;
                  continue;
                }
                else
                {
                  // error or end
                  // label was not inserted, but we want to skip to the next loop
                  if ( !tokenWasInserted )
                  {
                    tokenWasInserted = true;
                    sb.Append( token.Content );
                  }
                  i = nextIndex - 1;
                  labelWasInserted = true;
                  break;
                }
                ++nextIndex;
              }
              if ( labelWasInserted )
              {
                continue;
              }

              if ( i + 1 < lineInfo.Value.Tokens.Count )
              {
                int     refNo = -1;
                if ( int.TryParse( lineInfo.Value.Tokens[i + 1].Content, out refNo ) )
                {
                  if ( !m_LineInfos.Any( li => li.Value.LineNumber == refNo ) )
                  {
                    //AddError( lineInfo.Value.LineIndex, Types.ErrorCode.W1003_BASIC_REFERENCED_LINE_NUMBER_NOT_FOUND, $"Referenced unknown line number {refNo}" );
                    break;
                  }
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
            if ( Settings.BASICDialect.Opcodes[token.Content].AllowsSeveralLineNumbers )
            {
              // ON x GOTO/GOSUB can have more than one line number
              // insert label instead of line number
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
                    if ( !m_LineInfos.Any( li => li.Value.LineNumber == refNo ) )
                    {
                      //AddError( lineInfo.Value.LineIndex, Types.ErrorCode.W1003_BASIC_REFERENCED_LINE_NUMBER_NOT_FOUND, $"Referenced unknown line number {refNo}" );
                      break;
                    }
                    if ( !lineNumberReference.ContainsKey( refNo ) )
                    {
                      Debug.Log( "Error, found linenumber without reference " + refNo + " in line " + lineInfo.Value.LineNumber );
                      break;
                    }
                    sb.Append( lineNumberReference[refNo].ToString() );
                  }
                  else if ( ( nextToken.TokenType == Token.Type.DIRECT_TOKEN )
                  &&        ( nextToken.Content == " " ) )
                  {
                    sb.Append( nextToken.Content );
                    ++nextIndex;
                    continue;
                  }
                  else if ( nextToken.Content == "," )
                  {
                    // comma after comma, is valid
                    sb.Append( ',' );
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
                  ||   ( nextToken.Content != "," ) )
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



    private void AppendGapTokens( StringBuilder sb, List<Token> Tokens, ref int CurrentTokenIndex, int FirstTokenIndexAfterGap )
    {
      while ( CurrentTokenIndex + 1 < FirstTokenIndexAfterGap )
      {
        sb.Append( Tokens[CurrentTokenIndex + 1].Content );
        ++CurrentTokenIndex;
      }
    }



    public bool IsComment( Token token )
    {
      if ( Settings.BASICDialect.Opcodes.TryGetValue( token.Content, out var opcode ) )
      {
        return opcode.IsComment;
      }
      return false;
    }



    // finds prev non-space token or returns -1
    private int FindPrevToken( List<Token> Tokens, int StartIndex )
    {
      int curIndex = StartIndex - 1;

      while ( curIndex >= 0 )
      {
        if ( ( Tokens[curIndex].TokenType != Token.Type.DIRECT_TOKEN )
        ||   ( Tokens[curIndex].ByteValue != 0x20 ) )
        {
          // not a Space
          return curIndex;
        }
        --curIndex;
      }
      return -1;
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



    public string DecodeFromLabels( int StartLineNumber = 10, int LineStep = 10 )
    {
      StringBuilder sb = new StringBuilder();
      GR.Collections.Map<string,int>     labelToNumber = new GR.Collections.Map<string, int>();

      int     startLineNumber = StartLineNumber;
      int     lineNumberStep  = LineStep;
      int     lineNumber      = startLineNumber;

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
          }
          else
          {
            AddError( lineInfo.Value.LineIndex, Types.ErrorCode.E3003_BASIC_LABEL_MALFORMED, "Label malformed" );
          }
          continue;
        }

        lineNumber += lineNumberStep;
      }
      lineNumber = startLineNumber;
      foreach ( KeyValuePair<int, LineInfo> lineInfo in m_LineInfos )
      {
        if ( ( lineInfo.Value.Tokens.Count == 1 )
        &&   ( lineInfo.Value.Tokens[0].TokenType == Token.Type.HARD_COMMENT ) )
        {
          // leave as is
          sb.AppendLine( lineInfo.Value.Tokens[0].Content );
          continue;
        }

        if ( m_ASMFileInfo.FindTrueLineSource( lineInfo.Key, out string dummy, out int dummy2, out Types.ASM.SourceInfo sourceInfo ) )
        {
          if ( sourceInfo.Source == Types.ASM.SourceInfo.SourceInfoSource.MEDIA_INCLUDE )
          {
            // skip media includes, they are rebuilt, but parse the line number in front
            if ( lineInfo.Value.Tokens.Count > 0 )
            {
              int lineNumberFromMedia = GR.Convert.ToI32( lineInfo.Value.Tokens[0].Content );

              if ( lineNumberFromMedia > lineNumber )
              {
                lineNumber = lineNumberFromMedia + lineNumberStep;
              }
            }
            continue;
          }
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

        // remember cut off length so we can properly fill up with blanks below
        int   lineStartLength = sb.Length;
        int   lineLengthOffset = lineInfo.Value.Tokens[0].StartIndex;

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
          &&   ( Settings.BASICDialect.Opcodes[token.Content].GoTokenToMayFollow ) )
          {
            int nextTokenIndexC = FindNextToken( lineInfo.Value.Tokens, tokenIndex );
            if ( ( nextTokenIndexC != -1 )
            &&   ( lineInfo.Value.Tokens[nextTokenIndexC].TokenType == Token.Type.BASIC_TOKEN )
            &&   ( lineInfo.Value.Tokens[nextTokenIndexC].Content == "TO" ) )
            {
              // GO, followed by TO has one line number
              int nextTokenIndexA = FindNextToken( lineInfo.Value.Tokens, nextTokenIndexC );
              int nextTokenIndexB = FindNextToken( lineInfo.Value.Tokens, nextTokenIndexA );

              if ( ( lineInfo.Value.Tokens[nextTokenIndexA].TokenType == Token.Type.EX_BASIC_TOKEN )
              &&   ( lineInfo.Value.Tokens[nextTokenIndexA].ByteValue == Settings.BASICDialect.ExOpcodes["LABEL"].InsertionValue )
              &&   ( lineInfo.Value.Tokens[nextTokenIndexB].TokenType == Token.Type.NUMERIC_LITERAL ) )
              {
                sb.Append( token.Content );

                AppendGapTokens( sb, lineInfo.Value.Tokens, ref tokenIndex, nextTokenIndexA );
                /*
                while ( nextTokenIndexA - 1 > tokenIndex )
                {
                  // there were blanks in between
                  sb.Append( lineInfo.Value.Tokens[tokenIndex + 1].Content );
                  ++tokenIndex;
                }*/

                string label = "LABEL" + lineInfo.Value.Tokens[nextTokenIndexB].Content;

                if ( !labelToNumber.ContainsKey( label ) )
                {
                  AddError( lineInfo.Value.LineIndex, Types.ErrorCode.E3004_BASIC_MISSING_LABEL, "Unknown label " + label + " encountered" );
                }
                else
                {
                  sb.Append( labelToNumber[label].ToString() );
                }
                tokenIndex = nextTokenIndexB;
                continue;
              }
            }
          }

          if ( ( token.TokenType == Token.Type.BASIC_TOKEN )
          &&   ( ( Settings.BASICDialect.Opcodes[token.Content].AllowsSeveralLineNumbers )
          ||     ( Settings.BASICDialect.Opcodes[token.Content].ArgumentIndexOfExpectedLineNumber >= 0 ) ) )
          {
            nextTokenIndex = FindNextToken( lineInfo.Value.Tokens, tokenIndex );
            nextTokenIndex2 = FindNextToken( lineInfo.Value.Tokens, nextTokenIndex );

            if ( !tokenIsInserted )
            {
              tokenIsInserted = true;
              sb.Append( token.Content );

              AppendGapTokens( sb, lineInfo.Value.Tokens, ref tokenIndex, nextTokenIndex );
              /*
              while ( nextTokenIndex - 1 > tokenIndex )
              {
                // there were blanks in between
                sb.Append( lineInfo.Value.Tokens[tokenIndex + 1].Content );
                ++tokenIndex;
              }*/
            }
            while ( ( nextTokenIndex != -1 )
            &&      ( nextTokenIndex2 != -1 ) )
            {
              if ( ( lineInfo.Value.Tokens[nextTokenIndex].TokenType == Token.Type.DIRECT_TOKEN )
              &&   ( lineInfo.Value.Tokens[nextTokenIndex].Content == "," ) )
              {
                // comma is valid
                sb.Append( "," );
                AppendGapTokens( sb, lineInfo.Value.Tokens, ref nextTokenIndex, nextTokenIndex2 );
                /*
                while ( nextTokenIndex2 - nextTokenIndex > 1 )
                {
                  sb.Append( lineInfo.Value.Tokens[nextTokenIndex + 1].Content );
                  ++nextTokenIndex;
                }*/
                nextTokenIndex = nextTokenIndex2;
                nextTokenIndex2 = FindNextToken( lineInfo.Value.Tokens, nextTokenIndex );

                AppendGapTokens( sb, lineInfo.Value.Tokens, ref nextTokenIndex, nextTokenIndex2 );
                /*
                while ( nextTokenIndex2 - nextTokenIndex > 1 )
                {
                  sb.Append( lineInfo.Value.Tokens[nextTokenIndex + 1].Content );
                  ++nextTokenIndex;
                }*/
                continue;
              }


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
                  sb.Append( labelToNumber[label].ToString() );
                }
                tokenIndex = nextTokenIndex2;
                nextTokenIndex = FindNextToken( lineInfo.Value.Tokens, nextTokenIndex2 );
                if ( ( nextTokenIndex == -1 )
                ||   ( lineInfo.Value.Tokens[nextTokenIndex].Content != "," ) )
                {
                  break;
                }
                sb.Append( ',' );
                int nextTokenIndexB = FindNextToken( lineInfo.Value.Tokens, nextTokenIndex );

                AppendGapTokens( sb, lineInfo.Value.Tokens, ref nextTokenIndex, nextTokenIndexB );
                /*
                while ( nextTokenIndexB - nextTokenIndex > 1 )
                {
                  sb.Append( lineInfo.Value.Tokens[nextTokenIndex + 1].Content );
                  ++nextTokenIndex;
                }*/
                nextTokenIndex = nextTokenIndexB;
                nextTokenIndex2 = FindNextToken( lineInfo.Value.Tokens, nextTokenIndex );
              }
              else if ( ( lineInfo.Value.Tokens[nextTokenIndex].Content == ":" )
              ||        ( lineInfo.Value.Tokens[nextTokenIndex].TokenType == Token.Type.BASIC_TOKEN ) )
              {
                // we're leaving the scope of the token
                break;
              }
              else
              {
                sb.Append( lineInfo.Value.Tokens[nextTokenIndex].Content );
                tokenIndex = nextTokenIndex;
                nextTokenIndex = nextTokenIndex2;
                nextTokenIndex2 = FindNextToken( lineInfo.Value.Tokens, nextTokenIndex );
              }
            }
          }
          if ( ( token.TokenType == Token.Type.BASIC_TOKEN )
          &&   ( Settings.BASICDialect.Opcodes[token.Content].LineListRange ) )
          {
            sb.Append( token.Content );
            nextTokenIndex =  FindNextToken( lineInfo.Value.Tokens, tokenIndex );
            nextTokenIndex2 = FindNextToken( lineInfo.Value.Tokens, nextTokenIndex );
            if ( nextTokenIndex != -1 )
            {
              AppendGapTokens( sb, lineInfo.Value.Tokens, ref tokenIndex, nextTokenIndex );
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
                  sb.Append( labelToNumber[label].ToString() );
                }
                tokenIndex = nextTokenIndex2;

                nextTokenIndex = FindNextToken( lineInfo.Value.Tokens, nextTokenIndex2 );
                if ( nextTokenIndex != -1 )
                {
                  if ( lineInfo.Value.Tokens[nextTokenIndex].Content == "-" )
                  {
                    // a to part
                    AppendGapTokens( sb, lineInfo.Value.Tokens, ref tokenIndex, nextTokenIndex );
                    nextTokenIndex = FindNextToken( lineInfo.Value.Tokens, nextTokenIndex );
                    nextTokenIndex2 = FindNextToken( lineInfo.Value.Tokens, nextTokenIndex );
                    if ( nextTokenIndex != -1 )
                    {
                      if ( ( lineInfo.Value.Tokens[nextTokenIndex].TokenType == Token.Type.EX_BASIC_TOKEN )
                      &&   ( lineInfo.Value.Tokens[nextTokenIndex].ByteValue == Settings.BASICDialect.ExOpcodes["LABEL"].InsertionValue )
                      &&   ( lineInfo.Value.Tokens[nextTokenIndex2].TokenType == Token.Type.NUMERIC_LITERAL ) )
                      {
                        AppendGapTokens( sb, lineInfo.Value.Tokens, ref tokenIndex, nextTokenIndex );
                        label = "LABEL" + lineInfo.Value.Tokens[nextTokenIndex2].Content;

                        if ( !labelToNumber.ContainsKey( label ) )
                        {
                          AddError( lineInfo.Value.LineIndex, Types.ErrorCode.E3004_BASIC_MISSING_LABEL, "Unknown label " + label + " encountered" );
                        }
                        else
                        {
                          sb.Append( labelToNumber[label].ToString() );
                        }
                        tokenIndex = nextTokenIndex2;
                      }
                    }
                  }
                }
              }
              else if ( lineInfo.Value.Tokens[nextTokenIndex].Content == "-" )
              {
                // a to part
                AppendGapTokens( sb, lineInfo.Value.Tokens, ref tokenIndex, nextTokenIndex );
                nextTokenIndex = FindNextToken( lineInfo.Value.Tokens, nextTokenIndex );
                nextTokenIndex2 = FindNextToken( lineInfo.Value.Tokens, nextTokenIndex );
                if ( nextTokenIndex != -1 )
                {
                  if ( ( lineInfo.Value.Tokens[nextTokenIndex].TokenType == Token.Type.EX_BASIC_TOKEN )
                  &&   ( lineInfo.Value.Tokens[nextTokenIndex].ByteValue == Settings.BASICDialect.ExOpcodes["LABEL"].InsertionValue )
                  &&   ( lineInfo.Value.Tokens[nextTokenIndex2].TokenType == Token.Type.NUMERIC_LITERAL ) )
                  {
                    AppendGapTokens( sb, lineInfo.Value.Tokens, ref tokenIndex, nextTokenIndex );

                    string label = "LABEL" + lineInfo.Value.Tokens[nextTokenIndex2].Content;

                    if ( !labelToNumber.ContainsKey( label ) )
                    {
                      AddError( lineInfo.Value.LineIndex, Types.ErrorCode.E3004_BASIC_MISSING_LABEL, "Unknown label " + label + " encountered" );
                    }
                    else
                    {
                      sb.Append( labelToNumber[label].ToString() );
                    }
                    tokenIndex = nextTokenIndex2;
                  }
                }
              }
            }
            else
            {
              ++tokenIndex;
            }
            continue;
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
        lineNumber += lineNumberStep;
      }
      return sb.ToString();
    }



    public bool Disassemble( FileTypeNative nativeType, GR.Memory.ByteBuffer fullFileData, out List<string> Lines, out int startAddress  )
    {
      Lines         = new List<string>();
      startAddress  = -1;
      if ( ( fullFileData == null )
      ||   ( fullFileData.Length == 0 ) )
      {
        return false;
      }

      if ( !ExtractMachineSpecificData( fullFileData, nativeType, out var Data, out startAddress ) )
      {
        return false;
      }

      // ZX81:
      // Zeile = 
      //  ..

      // Commodore:
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
        var extractresult = ExtractLine( Data, ref dataPos, out var lineData, out int lineNumber );
        if ( extractresult == ExtractLineResult.ERROR )
        {
          return false;
        }
        if ( extractresult == ExtractLineResult.END_OF_CODE )
        {
          return true;
        }

        /*
        if ( dataPos + 2 > Data.Length )
        {
          Debug.Log( "no space for pointer" );
          return false;
        }
        if ( Data.UInt16At( dataPos ) == 0 )
        {
          // end
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
        */

        string    lineContent = lineNumber.ToString() + " ";
        bool      insideStringLiteral = false;
        bool      encounteredREM = false;

        int       lineDataPos = 0;
        while ( lineDataPos < lineData.Length )
        {
          byte    byteValue = lineData.ByteAt( lineDataPos );

          byteValue = MapNativeByteValueToChar( Settings.BASICDialect, byteValue );

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

            // Simons' BASIC/TSB can store extended tokens inside REMs (argh!)
            if ( Settings.BASICDialect.ExtendedTokensRecognizedInsideComment )
            {
              byte    nextValue = lineData.ByteAt( lineDataPos + 1 );
              ushort  extendedToken = (ushort)( ( byteValue << 8 ) + nextValue );

              if ( Settings.BASICDialect.OpcodesFromByte.ContainsKey( extendedToken ) )
              {
                lineContent += Settings.BASICDialect.OpcodesFromByte[extendedToken].Command;
                lineDataPos += 2;
                continue;
              }
            }

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
            ++lineDataPos;
            continue;
          }

          if ( insideStringLiteral )
          {
            var c64Key = FindKeyByCharacter( byteValue );
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
              if ( ( lineDataPos + 1 < lineData.Length )
              &&   ( lineData.ByteAt( lineDataPos + 1 ) != 0 ) )
              {
                byte    byteValue2 = lineData.ByteAt( lineDataPos + 1 );

                ushort tokenValue = (ushort)( byteValue2 | ( byteValue << 8 ) );

                if ( Settings.BASICDialect.OpcodesFromByte.ContainsKey( tokenValue ) )
                {
                  ++lineDataPos;
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
                if ( ConstantData.PETSCIIToUnicode.ContainsKey( byteValue ) )
                {
                  lineContent += ConstantData.PETSCIIToUnicode[byteValue];
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
          ++lineDataPos;
        }

        if ( dataPos >= Data.Length )
        {
          // reached the end, should not happen!
          Debug.Log( "reached wrong end" );
          return false;
        }
        //Debug.Log( "Line:" + lineContent );
        Lines.Add( lineContent );
        //++dataPos;
      }

      return false;
    }



    private SingleKeyInfo FindKeyByCharacter( byte byteValue )
    {
      switch ( _ParseContext.KeyboardMachineType )
      {
        case MachineType.ZX81:
          break;
      }
      if ( ( _ParseContext.KeyboardMachineType == MachineType.C64 )
      ||   ( _ParseContext.KeyboardMachineType == MachineType.C128 )
      ||   ( _ParseContext.KeyboardMachineType == MachineType.CBM )
      ||   ( _ParseContext.KeyboardMachineType == MachineType.COMMANDER_X16 )
      ||   ( _ParseContext.KeyboardMachineType == MachineType.MEGA65 )
      ||   ( _ParseContext.KeyboardMachineType == MachineType.PET )
      ||   ( _ParseContext.KeyboardMachineType == MachineType.PLUS4 )
      ||   ( _ParseContext.KeyboardMachineType == MachineType.VIC20 ) )
      {
        // Codes 192-223 map to Codes  96-127
        // Codes 224-254 map to Codes 160-190
        // Code  255     map to Code  126
        if ( ( byteValue >= 192 )
        &&   ( byteValue <= 223 ) )
        {
          byteValue -= 192 - 96;
        }
        else if ( ( byteValue >= 224 )
        &&        ( byteValue <= 254 ) )
        {
          byteValue -= 224 - 160;
        }
        else if ( byteValue == 255 )
        {
          byteValue = 126;
        }
      }
      return ConstantData.AllPhysicalKeyInfos[_ParseContext.KeyboardMachineType].FirstOrDefault( k => k.NativeValue == byteValue );
    }



    private byte MapCharToNativeByte( char character, CompileTargetType targetType )
    {
      switch ( targetType )
      {
        case CompileTargetType.P_ZX81:
          {
            var key = ConstantData.AllPhysicalKeyInfos[MachineType.ZX81].FirstOrDefault( ki => ki.CharValue == character );
            if ( key != default( SingleKeyInfo ) )
            {
              return key.NativeValue;
            }
            /*
            switch ( character )
            {
              case '"':
                return 0x0b;
              case '£':
                return 0x0c;
              case '$':
                return 0x0d;
              case ':':
                return 0x0e;
              case '?':
                return 0x0f;
              case '(':
                return 0x10;
              case ')':
                return 0x11;
              case '>':
                return 0x12;
              case '<':
                return 0x13;
              case '=':
                return 0x14;
              case '+':
                return 0x15;
              case '-':
                return 0x16;
              case '*':
                return 0x17;
              case '/':
                return 0x18;
              case ';':
                return 0x19;
              case ',':
                return 0x1a;
              case '.':
                return 0x1b;
              case '0':
                return 0x1c;
              case ' ':
                return 0;
            }
            if ( ( character >= '0' )
            && ( character <= '9' ) )
            {
              return (byte)( 0x1c + character - '0' );
            }
            if ( ( character >= 'A' )
            && ( character <= 'Z' ) )
            {
              return (byte)( 0x26 + character - 'A' );
            }*/
          }
          return (byte)character;
      }
      return (byte)character;
    }



    private byte MapNativeByteValueToChar( Dialect dialect, byte byteValue )
    {
      return byteValue;
    }



    private ExtractLineResult ExtractLine( ByteBuffer data, ref int dataPos, out ByteBuffer lineData, out int lineNumber )
    {
      lineData    = null;
      lineNumber  = -1;

      var machineType = MachineType.C64;
      if ( Settings.BASICDialect.MachineTypes.Count > 0 )
      {
        machineType = Settings.BASICDialect.MachineTypes[0];
      }

      switch ( machineType )
      {
        case MachineType.ZX81:
          {
            if ( dataPos >= data.Length )
            {
              return ExtractLineResult.ERROR;
            }
            if ( data[dataPos] == 0x76 )
            {
              return ExtractLineResult.END_OF_CODE;
            }
            if ( dataPos + 5 > data.Length )
            {
              return ExtractLineResult.ERROR;
            }
            lineNumber = data.UInt16NetworkOrderAt( dataPos );

            int   length = data.UInt16At( dataPos + 2 );

            if ( dataPos + 4 + length > data.Length )
            {
              return ExtractLineResult.ERROR;
            }
            if ( length == 0 )
            {
              lineData = new ByteBuffer();
            }
            else
            {
              lineData = data.SubBuffer( dataPos + 4, length - 1 );
            }
            // expect an EOL
            if ( data[dataPos + 4 + length - 1] != 0x76 )
            {
              return ExtractLineResult.ERROR;
            }
            dataPos += 4 + length;

            return ExtractLineResult.OK;
          }
        case MachineType.CBM:
        case MachineType.C128:
        case MachineType.C64:
        case MachineType.COMMANDER_X16:
        case MachineType.MEGA65:
        case MachineType.PET:
        case MachineType.PLUS4:
        case MachineType.VIC20:
          if ( dataPos + 2 > data.Length )
          {
            return ExtractLineResult.ERROR;
          }
          if ( data.UInt16At( dataPos ) == 0 )
          {
            // end
            return ExtractLineResult.END_OF_CODE;
          }

          // TODO - check pointer?
          dataPos += 2;

          // line number
          if ( dataPos + 2 > data.Length )
          {
            // no space for line number
            return ExtractLineResult.ERROR;
          }
          lineNumber = data.UInt16At( dataPos );

          {
            int endPos = data.Find( 0, dataPos + 2 );
            if ( endPos == -1 )
            {
              return ExtractLineResult.ERROR;
            }
            lineData = data.SubBuffer( dataPos + 2, endPos - dataPos - 2 );
            dataPos = endPos + 1;
          }
          return ExtractLineResult.OK;
      }
      lineData = null;
      return ExtractLineResult.ERROR;
    }



    private bool ExtractMachineSpecificData( ByteBuffer fullFileData, FileTypeNative nativeType, out ByteBuffer data, out int startAddress )
    {
      startAddress = -1;
      switch ( nativeType )
      {
        case FileTypeNative.COMMODORE_PRG:
          data          = fullFileData.SubBuffer( 2 );
          startAddress  = fullFileData.UInt16At( 0 );
          return true;
        case FileTypeNative.SPECTRUM_P:
          // strip off header/trailer
          data          = fullFileData.SubBuffer( 116 );
          // for ZX81 leave as is so the dialect default is used
          return true;
      }
      data = fullFileData;
      return true;
    }



    public RenumberResult CanRenumber( int LineStart, int LineStep, int firstLineNumber, int lastLineNumber, out string errorMessage )
    {
      errorMessage = "";
      if ( m_LineInfos.Count == 0 )
      {
        return RenumberResult.NOTHING_TO_DO;
      }
      if ( ( LineStart < 0 )
      ||   ( LineStart > Settings.BASICDialect.MaxLineNumber )
      ||   ( firstLineNumber < 0 )
      ||   ( firstLineNumber > Settings.BASICDialect.MaxLineNumber )
      ||   ( lastLineNumber < 0 )
      ||   ( lastLineNumber > Settings.BASICDialect.MaxLineNumber ) )
      {
        errorMessage = $"Invalid line numbers provided, must be >= and <= {Settings.BASICDialect.MaxLineNumber}";
        return RenumberResult.INVALID_VALUES;
      }

      var firstLine = m_LineInfos.FirstOrDefault( li => ( li.Value.LineNumber >= firstLineNumber ) && ( li.Value.LineNumber != -1 ) );
      var lastLine = m_LineInfos.LastOrDefault( li => ( li.Value.LineNumber <= lastLineNumber ) && ( li.Value.LineNumber != -1 ) );
      if ( ( firstLine.Value == null )
      ||   ( lastLine.Value == null ) )
      {
        if ( ( firstLineNumber == 0 )
        &&   ( lastLineNumber == Settings.BASICDialect.MaxLineNumber ) )
        {
          // no lines
          return RenumberResult.OK;
        }
        errorMessage = $"Invalid line number range ({firstLineNumber} to {lastLineNumber}) provided, no relevant lines found";
        return RenumberResult.INVALID_VALUES;
      }

      int  trueNumberOfLines = 0;
      for ( int i = firstLine.Key; i <= lastLine.Key; ++i )
      {
        if ( ( m_LineInfos.ContainsKey( i ) )
        &&   ( m_LineInfos[i].LineNumber != -1 ) )
        {
          ++trueNumberOfLines;
        }
      }

      if ( firstLine.Key > 0 )
      {
        // find previous line
        int   prevIndex = firstLine.Key - 1;
        while ( ( prevIndex >= 0 )
        &&      ( ( !m_LineInfos.ContainsKey( prevIndex ) )
        ||        ( m_LineInfos[prevIndex].LineNumber == -1 ) ) )
        {
          --prevIndex;
        }
        if ( prevIndex != -1 )
        {
          if ( m_LineInfos[prevIndex].LineNumber >= LineStart )
          {
            errorMessage = $"First line number {LineStart} is smaller or equal than a previous line {m_LineInfos[prevIndex].LineNumber}";
            return RenumberResult.OVERLAP_AT_START;
          }
        }
      }
      // find next line
      int   nextIndex = lastLine.Key + 1;
      while ( ( nextIndex < m_LineInfos.Last().Key )
      &&      ( ( !m_LineInfos.ContainsKey( nextIndex ) )
      ||        ( m_LineInfos[nextIndex].LineNumber == -1 ) ) )
      {
        ++nextIndex;
      }
      if ( ( nextIndex <= m_LineInfos.Last().Key )
      &&   ( m_LineInfos.ContainsKey( nextIndex ) )
      &&   ( m_LineInfos[nextIndex].LineNumber != -1 )
      &&   ( m_LineInfos[nextIndex].LineNumber <= LineStart + ( trueNumberOfLines - 1 ) * LineStep ) )
      {
        errorMessage = $"Last determined line number {LineStart + ( trueNumberOfLines - 1 ) * LineStep} is higher or equal than the next line {m_LineInfos[nextIndex].LineNumber}";
        return RenumberResult.OVERLAP_AT_END;
      }

      if ( LineStart + LineStep * ( trueNumberOfLines - 1 ) > Settings.BASICDialect.MaxLineNumber )
      {
        errorMessage = $"The final line number {LineStart + LineStep * ( trueNumberOfLines - 1 )} would result in a value greater than the maximum possible line number {Settings.BASICDialect.MaxLineNumber}";
        return RenumberResult.TOO_MANY_LINES;
      }
      return RenumberResult.OK;
    }



    public string Renumber( int LineStart, int LineStep, int FirstLineNumber, int LastLineNumber )
    {
      // UGLY HACK - > TODO make it clean later (as if) -> store stripping settings
      bool settingsStripSpaces  = Settings.StripSpaces;
      bool settingsStripREM     = Settings.StripREM;

      m_CompileConfig.DoNotExpandStringLiterals = true;

      Settings.StripSpaces = false;
      Settings.StripREM = false;

      StringBuilder sb = new StringBuilder();

      GR.Collections.Map<int, int> lineNumberReference = new GR.Collections.Map<int, int>();

      int curLine = LineStart;
      foreach ( KeyValuePair<int, LineInfo> lineInfo in m_LineInfos )
      {
        int   dummyLineNum = curLine;        var tokenLineInfo = TokenizeLine( lineInfo.Value.Line, 0, ref dummyLineNum );
        // skip hard comments
        if ( ( tokenLineInfo.Tokens.Count == 1 )
        &&   ( tokenLineInfo.Tokens[0].TokenType == Token.Type.HARD_COMMENT ) )
        {
          continue;
        }
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
        // weed out dynamic DATAs generated by BINDATA, SPRITEDATA, ... directives
        if ( m_ASMFileInfo.FindTrueLineSource( lineInfoOrig.Key, out string filename, out int localLineIndex, out Types.ASM.SourceInfo srcInfo ) )
        {
          if ( srcInfo.Source == Types.ASM.SourceInfo.SourceInfoSource.MEDIA_INCLUDE )
          {
            continue;
          }
        }

        var lineInfo = TokenizeLine( lineInfoOrig.Value.Line, 0, ref curLine );
        lineInfo = PureTokenizeLine( lineInfoOrig.Value.Line );
        for ( int i = 0; i < lineInfo.Tokens.Count; ++i )
        {
          Token token = lineInfo.Tokens[i];
          if ( ( token.TokenType == Token.Type.LINE_NUMBER )
          ||   ( ( i == 0 )
          &&     ( token.TokenType == Token.Type.NUMERIC_LITERAL ) ) )
          {
            // replace starting line number
            if ( ( token.TokenType == Token.Type.NUMERIC_LITERAL )
            ||   ( token.TokenType == Token.Type.LINE_NUMBER ) )
            {
              lineInfo.LineNumber = GR.Convert.ToI32( token.Content );
            }
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
            if ( Settings.BASICDialect.Opcodes[token.Content].GoTokenToMayFollow )
            {
              int nextTokenIndex = FindNextToken( lineInfo.Tokens, i );
              if ( ( nextTokenIndex != -1 )
              &&   ( lineInfo.Tokens[nextTokenIndex].TokenType == Token.Type.BASIC_TOKEN )
              &&   ( lineInfo.Tokens[nextTokenIndex].Content == "TO" ) )
              {
                // GO, followed by TO has one line number
                nextTokenIndex = FindNextToken( lineInfo.Tokens, nextTokenIndex );
                if ( ( nextTokenIndex != -1 )
                &&   ( lineInfo.Tokens[nextTokenIndex].TokenType == Token.Type.NUMERIC_LITERAL ) )
                {
                  if ( ( nextTokenIndex != -1 )
                  &&   ( int.TryParse( lineInfo.Tokens[nextTokenIndex].Content, out int refNo ) ) )
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
            }

            if ( Settings.BASICDialect.Opcodes[token.Content].ArgumentIndexOfExpectedLineNumber == 0 )
            {
              // insert label instead of line number
              if ( i + 1 < lineInfo.Tokens.Count )
              {
                int     refNo = -1;
                int     nextTokenIndex = FindNextToken( lineInfo.Tokens, i );

                if ( ( nextTokenIndex != -1 )
                &&   ( lineInfo.Tokens[nextTokenIndex].TokenType == Token.Type.DIRECT_TOKEN )
                &&   ( lineInfo.Tokens[nextTokenIndex].Content == " " ) )
                {
                  nextTokenIndex = FindNextToken( lineInfo.Tokens, nextTokenIndex );
                }

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
            if ( Settings.BASICDialect.Opcodes[token.Content].AllowsSeveralLineNumbers )
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
                    if ( ( nextIndex > 0 )
                    &&   ( lineInfo.Tokens[nextIndex - 1].StartIndex + lineInfo.Tokens[nextIndex - 1].Content.Length < nextToken.StartIndex ) )
                    {
                      // keep spaces
                      for ( int j = 0; j < nextToken.StartIndex - ( lineInfo.Tokens[nextIndex - 1].StartIndex + lineInfo.Tokens[nextIndex - 1].Content.Length ); ++j )
                      {
                        sb.Append( ' ' );
                      }
                    }

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
                    sb.Append( nextToken.Content );
                    // space is valid, skip
                    ++nextIndex;
                    continue;
                  }
                  else if ( ( nextToken.TokenType == Token.Type.DIRECT_TOKEN )
                  &&        ( nextToken.Content == "," ) )
                  {
                    // comma after comma is valid
                    sb.Append( nextToken.Content );
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
                  if ( ( nextToken.TokenType == Token.Type.DIRECT_TOKEN )
                  &&   ( nextToken.Content == " " ) )
                  {
                    // space is valid
                    sb.Append( nextToken.Content );
                    ++nextIndex;
                    i = nextIndex - 1;
                    continue;
                  }
                  else if ( ( nextToken.TokenType != Token.Type.DIRECT_TOKEN )
                  ||        ( nextToken.Content != "," ) )
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
          if ( ( i + 1 < lineInfo.Tokens.Count )
          &&   ( token.StartIndex + token.Content.Length < lineInfo.Tokens[i + 1].StartIndex ) )
          {
            // keep spaces intact
            for ( int j = 0; j < lineInfo.Tokens[i + 1].StartIndex - ( token.StartIndex + token.Content.Length ); ++j )
            {
              sb.Append( ' ' );
            }
          }
        }
        sb.Append( "\r\n" );
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



    /// <summary>
    /// renumbers only the actual line numbers in front
    /// </summary>
    /// <param name="lineStart"></param>
    /// <param name="lineStep"></param>
    /// <param name="firstLineNumber"></param>
    /// <param name="lastLineNumber"></param>
    /// <returns></returns>
    public string RenumberLineNumbers( int lineStart, int lineStep, int firstLineNumber, int lastLineNumber )
    {
      StringBuilder sb = new StringBuilder();

      if ( m_LineInfos.Count == 0 )
      {
        return "";
      }

      int curLine = lineStart;
      int curLineIndex = m_LineInfos.First().Key;
      foreach ( KeyValuePair<int, LineInfo> lineInfo in m_LineInfos )
      {
        // empty lines
        while ( curLineIndex < lineInfo.Key )
        {
          sb.AppendLine();
          ++curLineIndex;
        }
        ++curLineIndex;
        if ( ( lineInfo.Key < firstLineNumber )
        ||   ( lineInfo.Key > lastLineNumber ) )
        {
          sb.AppendLine( lineInfo.Value.Line );
          continue;
        }
        
        int   dummyLineNum = curLine;        
        var tokenLineInfo = TokenizeLine( lineInfo.Value.Line, 0, ref dummyLineNum );

        // skip hard comments
        if ( ( tokenLineInfo.Tokens.Count == 1 )
        &&   ( tokenLineInfo.Tokens[0].TokenType == Token.Type.HARD_COMMENT ) )
        {
          sb.AppendLine( lineInfo.Value.Line );
          continue;
        }

        if ( ( tokenLineInfo.Tokens.Count >= 1 )
        &&   ( tokenLineInfo.Tokens[0].TokenType == Token.Type.LINE_NUMBER ) )
        {
          sb.Append( curLine.ToString() );
          curLine += lineStep;

          if ( tokenLineInfo.Tokens.Count > 1 )
          {
            int numSpaces = tokenLineInfo.Tokens[1].StartIndex - ( tokenLineInfo.Tokens[0].StartIndex + tokenLineInfo.Tokens[0].Content.Length );

            sb.Append( ' ', numSpaces );
          }
          sb.AppendLine( TokensToExpression( tokenLineInfo.Tokens, 1, tokenLineInfo.Tokens.Count - 1 ) );
        }
        else
        {
          sb.AppendLine( lineInfo.Value.Line );
        }
      }
      // strip last line break
      if ( m_LineInfos.Count > 0 )
      {
        sb.Remove( sb.Length - 2, 2 );
      }
      return sb.ToString();
    }



    public static string ReplaceAllSymbolsByMacros( string BasicText, bool LowerCaseMode )
    {
      bool    insideQuotes = false;
      for ( int i = 0; i < BasicText.Length; ++i )
      {
        char    chartoCheck = BasicText[i];

        if ( chartoCheck == '"' )
        {
          insideQuotes = !insideQuotes;
        }
        else if ( chartoCheck == '\n' )
        {
          insideQuotes = false;
        }
        else if ( insideQuotes )
        {
          var c64Key = ConstantData.FindC64KeyByUnicode( chartoCheck );
          if ( c64Key != null )
          {
            if ( c64Key.Replacements.Count > 0 )
            {
              int     numCharsToReplace = 1;
              int     testPos = i;
              while ( ( testPos + 1 < BasicText.Length )
              && ( BasicText[testPos + 1] == chartoCheck ) )
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

              // special case, do not insert {SPACE} for single chars
              if ( ( chartoCheck == ' ' )
              && ( numCharsToReplace == 1 ) )
              {
                continue;
              }

              if ( LowerCaseMode )
              {
                replacement = replacement.ToLower();
              }
              BasicText = BasicText.Substring( 0, i ) + "{" + replacement + "}" + BasicText.Substring( i + numCharsToReplace );
              i += 2 + replacement.Length - 1;
            }
          }
          else if ( ( chartoCheck < 'A' )
          ||        ( chartoCheck > 'Z' ) )
          {
            Debug.Log( $"FindC64KeyByUnicode found no key for {chartoCheck}/{(ushort)chartoCheck}" );
          }
        }
      }
      return BasicText;
    }



    public static string ReplaceAllMacrosBySymbols( string BasicText, MachineType machine, out bool HadError )
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
          if ( curChar == '\n' )
          {
            // the macro was not closed
            insideMacro = false;

            // simply re-append
            string macro = BasicText.Substring( macroStartPos, posInLine - macroStartPos );
            sb.Append( macro );
            ++posInLine;
            continue;
          }
          if ( curChar == '}' )
          {
            insideMacro = false;

            string macro = BasicText.Substring( macroStartPos + 1, posInLine - macroStartPos - 1 ).ToUpper();
            int macroCount = 1;

            macro = BasicFileParser.DetermineMacroCount( macro, out macroCount );

            bool  foundMacro = false;
            foreach ( var key in ConstantData.AllPhysicalKeyInfos[machine] )
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
              // simply re-append
              sb.Append( '{' );
              sb.Append( BasicText.Substring( macroStartPos + 1, posInLine - macroStartPos - 1 ) );
              sb.Append( '}' );
              ++posInLine;
              continue;
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



    public static string ReplaceAllMacrosByPETSCIICode( string BasicText, GR.Collections.Map<byte, byte> CustomMapping, MachineType machine, out bool HadError )
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

            macro = DetermineMacroCount( macro, out macroCount );

            bool  foundMacro = false;
            foreach ( var key in ConstantData.AllPhysicalKeyInfos[machine] )
            {
              if ( key.Replacements.Contains( macro ) )
              {
                for ( int i = 0; i < macroCount; ++i )
                {
                  sb.Append( (char)key.NativeValue );
                }
                foundMacro = true;
                break;
              }
            }
            if ( !foundMacro )
            {
              // a inbuilt expression?
              if ( macro.StartsWith( "DATE" ) )
              {
                string    details = "yyyy-MM-dd";
                int       sepPos = macro.IndexOf( ':' );
                if ( sepPos != -1 )
                {
                  details = BasicText.Substring( macroStartPos + 1, posInLine - macroStartPos - 1 ).Substring( sepPos + 1 );
                  if ( string.IsNullOrEmpty( details ) )
                  {
                    details = "yyyy-MM-dd";
                  }
                }
                DateTime today = DateTime.Now;

                string    result = today.ToString( details );
                for ( int i = 0; i < result.Length; ++i )
                {
                  sb.Append( result[i] );
                }
                foundMacro = true;
              }
            }
            if ( !foundMacro )
            {
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
        if ( ( CustomMapping != null )
        &&   ( CustomMapping.ContainsKey( (byte)curChar ) ) )
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



    public static string CollapseTokens( string BasicText, BASIC.Dialect dialect, bool nonC64Font )
    {
      StringBuilder     sb = new StringBuilder();

      int               posInLine = 0;
      bool              insideStringLiteral = false;

      while ( posInLine < BasicText.Length )
      {
        char    curChar = BasicText[posInLine];
        if ( !insideStringLiteral )
        {
          if ( curChar == '\n' )
          {
            // the macro was not closed
            insideStringLiteral = false;
          }
          // TODO - check for BASIC tokens
          if ( ( curChar >= 'A' )
          &&   ( curChar <= 'Z' ) )
          {
            bool foundOpcode = false;
            foreach ( var opcode in dialect.Opcodes.Values )
            {
              if ( ( opcode.ShortCut != null )
              &&   ( opcode.ShortCut.Length > 0 )
              &&   ( string.Compare( opcode.Command, 0, BasicText, posInLine, opcode.Command.Length ) == 0 ) )
              {
                sb.Append( MakeUpperCase( opcode.ShortCut, nonC64Font ) );
                posInLine += opcode.Command.Length;
                foundOpcode = true;
                break;
              }
            }
            if ( foundOpcode )
            {
              continue;
            }
          }
        }
        if ( curChar == '"' )
        {
          insideStringLiteral = !insideStringLiteral;
        }
        // normal chars are passed on (also tabs, cr, lf)
        sb.Append( curChar );
        ++posInLine;
      }
      return sb.ToString();
    }



    public static string ExpandTokens( string BasicText, BASIC.Dialect dialect, bool nonC64Font )
    {
      StringBuilder     sb = new StringBuilder();

      int               posInLine = 0;
      bool              insideStringLiteral = false;

      while ( posInLine < BasicText.Length )
      {
        char    curChar = BasicText[posInLine];
        if ( !insideStringLiteral )
        {
          if ( curChar == '\n' )
          {
            // the macro was not closed
            insideStringLiteral = false;
          }
          // check for BASIC tokens
          if ( ( ( curChar >= 'A' )
          &&     ( curChar <= 'Z' ) )
          ||   ( curChar == '?' ) )
          {
            bool foundOpcode = false;
            foreach ( var opcode in dialect.Opcodes.Values )
            {
              if ( ( opcode.ShortCut != null )
              &&   ( opcode.ShortCut.Length > 0 )
              &&   ( string.Compare( MakeUpperCase( opcode.ShortCut, nonC64Font ), 0, BasicText, posInLine, opcode.ShortCut.Length ) == 0 ) )
              {
                sb.Append( opcode.Command );
                posInLine += opcode.ShortCut.Length;
                foundOpcode = true;
                break;
              }
            }
            if ( foundOpcode )
            {
              continue;
            }
          }
        }
        if ( curChar == '"' )
        {
          insideStringLiteral = !insideStringLiteral;
        }
        // normal chars are passed on (also tabs, cr, lf)
        sb.Append( curChar );
        ++posInLine;
      }
      return sb.ToString();
    }



    // make sure any non-ascii letters (native true type unicode) are mapped to their ASCII counterpart
    public static string NormalizeText( string BASICText, bool NonC64Font )
    {
      if ( NonC64Font )
      {
        return BASICText;
      }

      StringBuilder   sb = new StringBuilder( BASICText.Length );

      foreach ( var singleChar in BASICText )
      {
        if ( ( singleChar & 0xff00 ) == 0xef00 )
        {
          if ( ( singleChar >= 0xef41 )
          &&   ( singleChar <= 0xef41 + 25 ) )
          {
            // lower case A-Z
            sb.Append( (char)( singleChar - 0xef41 + (int)'a' ) );
            continue;
          }
          char    newChar = (char)( ( singleChar & 0x00ff ) | 0xee00 );
          if ( ( newChar >= 0xee01 )
          &&   ( newChar <= 0xee01 + 25 ) )
          {
            // upper case A-Z
            sb.Append( (char)( newChar - 0xee01 + 'A' ) );
          }
          else if ( ( newChar >= 0xee41 )
          &&        ( newChar <= 0xee41 + 25 ) )
          {
            // lower case A-Z
            sb.Append( (char)( newChar - 0xee41 + 'a' ) );
          }
          else
          {
            sb.Append( newChar );
          }
        }
        else
        {
          sb.Append( singleChar );
        }
      }
      return sb.ToString();
    }



    public static string MakeUpperCase( string BASICText, bool NonC64Font )
    {
      StringBuilder   sb = new StringBuilder( BASICText.Length );

      foreach ( var singleChar in BASICText )
      {
        if ( !NonC64Font )
        {
          if ( ( singleChar & 0xff00 ) == 0xef00 )
          {
            if ( ( singleChar >= 0xef41 )
            &&   ( singleChar <= 0xef41 + 25 ) )
            {
              // lower case A-Z
              sb.Append( (char)( singleChar - 0xef41 + 0xee41 ) );
              continue;
            }
            char    newChar = (char)( ( singleChar & 0x00ff ) | 0xee00 );
            if ( ( newChar >= 0xee01 )
            &&   ( newChar <= 0xee01 + 25 ) )
            {
              // upper case A-Z
              sb.Append( (char)( newChar - 0xee01 + 'A' ) );
            }
            else if ( ( newChar >= 0xee41 )
            &&        ( newChar <= 0xee41 + 25 ) )
            {
              // lower case A-Z
              sb.Append( (char)( newChar - 0xee41 + 'a' ) );
            }
            else if ( ( newChar >= 'A' )
            &&        ( newChar <= 'Z' ) )
            {
              sb.Append( (char)( newChar + 0xe000 ) );
            }
            else
            {
              sb.Append( newChar );
            }
          }
          else if ( ( singleChar >= 'A' )
          &&        ( singleChar <= 'Z' ) )
          {
            // regular uppercase to shifted letters
            sb.Append( (char)( singleChar - 'A' + 0xee41 ) );
          }
          else if ( ( singleChar >= 0x61 )
          &&        ( singleChar <= 0x7a ) )
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



    public static string MakeLowerCase( string BASICText, bool NonC64Font )
    {
      StringBuilder   sb = new StringBuilder( BASICText.Length );

      foreach ( var singleChar in BASICText )
      {
        if ( !NonC64Font )
        {
          if ( ( singleChar >= 0xee41 )
          &&   ( singleChar <= 0xee5a ) )
          {
            // shifted letters turn to upper case
            sb.Append( (char)( singleChar - 0xee41 + 'A' ) );
          }
          else if ( ( singleChar >= 'A' )
          &&        ( singleChar <= 'Z' ) )
          {
            // regular upper case to lower case
            sb.Append( (char)( singleChar + 'a' - 'A' ) );
          }
          else if ( ( singleChar >= 0xee01 )
          &&        ( singleChar <= 0xee1a ) )
          {
            // unicode upper case to regular lower case
            sb.Append( (char)( singleChar - 0xee01 + 'a' ) );
          }
          else if ( ( singleChar >= 0xee00 )
          &&        ( singleChar <= 0xeeff ) )
          {
            // plain lower case
            sb.Append( (char)( ( singleChar & 0x00ff ) | 0xef00 ) );
          }
          else
          {
            // keep as is
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



    private void InsertSourceInfo( Types.ASM.SourceInfo sourceInfo )
    {
      InsertSourceInfo( sourceInfo, true, false );
    }



    private void InsertSourceInfo( Types.ASM.SourceInfo sourceInfo, bool AllowShifting, bool OverwriteFirstLineOfOverlapping )
    {
      int lineCount = sourceInfo.LineCount;
      /*
      if ( OverwriteFirstLineOfOverlapping )
      {
        --lineCount;
      }*/

      // move zones
      foreach ( var zoneList in m_ASMFileInfo.Zones.Values )
      {
        foreach ( var zoneInfo in zoneList )
        {
          if ( zoneInfo.LineIndex >= sourceInfo.GlobalStartLine + lineCount )
          {
            zoneInfo.LineIndex += lineCount;
            continue;
          }
          else if ( zoneInfo.LineIndex < sourceInfo.GlobalStartLine )
          {
            continue;
          }
          // inside (simply grow) -> not split?
          zoneInfo.LineCount += lineCount;
        }
      }

      List<Types.ASM.SourceInfo> movedInfos = new List<Types.ASM.SourceInfo>();
      foreach ( Types.ASM.SourceInfo oldInfo in m_ASMFileInfo.SourceInfo.Values )
      {
        if ( !AllowShifting )
        {
          // only split
          if ( ( sourceInfo.GlobalStartLine > oldInfo.GlobalStartLine )
          && ( sourceInfo.GlobalStartLine + sourceInfo.LineCount <= oldInfo.GlobalStartLine + oldInfo.LineCount ) )
          {
            // inside split
            Types.ASM.SourceInfo secondHalf = new Types.ASM.SourceInfo();
            secondHalf.Filename = oldInfo.Filename;
            secondHalf.FilenameParent = oldInfo.FilenameParent;
            secondHalf.FullPath = oldInfo.FullPath;
            secondHalf.GlobalStartLine = sourceInfo.GlobalStartLine + sourceInfo.LineCount;
            secondHalf.LineCount = ( oldInfo.GlobalStartLine + oldInfo.LineCount ) - ( sourceInfo.GlobalStartLine + sourceInfo.LineCount );

            oldInfo.LineCount = sourceInfo.GlobalStartLine - oldInfo.GlobalStartLine;

            secondHalf.LocalStartLine = oldInfo.LocalStartLine + oldInfo.LineCount;
            movedInfos.Add( secondHalf );
            break;
          }
          else if ( ( oldInfo.GlobalStartLine < sourceInfo.GlobalStartLine )
          && ( oldInfo.GlobalStartLine + oldInfo.LineCount > sourceInfo.GlobalStartLine )
          && ( oldInfo.GlobalStartLine + oldInfo.LineCount <= sourceInfo.GlobalStartLine + sourceInfo.LineCount ) )
          {
            // oldInfo overlaps into new sourceinfo, split second half
            oldInfo.LineCount = sourceInfo.GlobalStartLine - oldInfo.GlobalStartLine;
          }
          else if ( ( oldInfo.GlobalStartLine >= sourceInfo.GlobalStartLine )
          && ( oldInfo.GlobalStartLine < sourceInfo.GlobalStartLine + sourceInfo.LineCount ) )
          {
            // oldInfo starts inside new sourceinfo, split first half
            oldInfo.LineCount -= sourceInfo.GlobalStartLine + sourceInfo.LineCount - oldInfo.GlobalStartLine;
            oldInfo.GlobalStartLine = sourceInfo.GlobalStartLine + sourceInfo.LineCount;

            movedInfos.Add( oldInfo );
            break;
          }
        }
        else
        {
          if ( oldInfo.GlobalStartLine >= sourceInfo.GlobalStartLine )
          {
            // shift down completely
            /*
            if ( OverwriteFirstLineOfOverlapping )
            {
              if ( lineCount > 1 )
              {
                oldInfo.GlobalStartLine += lineCount - 1;
                movedInfos.Add( oldInfo );
              }
            }
            else*/
            {
              oldInfo.GlobalStartLine += lineCount;
              movedInfos.Add( oldInfo );
            }
          }
          else if ( oldInfo.GlobalStartLine + oldInfo.LineCount > sourceInfo.GlobalStartLine )
          {
            // only split if snippets do not end at the same line
            //if ( sourceInfo.GlobalStartLine + sourceInfo.LineCount != oldInfo.GlobalStartLine + oldInfo.LineCount )
            {
              // split!
              Types.ASM.SourceInfo secondHalf = new Types.ASM.SourceInfo();
              secondHalf.Filename = oldInfo.Filename;
              secondHalf.FilenameParent = oldInfo.FilenameParent;
              secondHalf.FullPath = oldInfo.FullPath;
              secondHalf.GlobalStartLine = sourceInfo.GlobalStartLine + sourceInfo.LineCount;
              secondHalf.LineCount = oldInfo.LineCount - ( sourceInfo.GlobalStartLine - oldInfo.GlobalStartLine );

              oldInfo.LineCount -= secondHalf.LineCount;

              secondHalf.LocalStartLine = oldInfo.LocalStartLine + oldInfo.LineCount;

              /*
              if ( OverwriteFirstLineOfOverlapping )
              {
                // BREAKING CHANGE !!! -> second half gets first line removed!!
                ++secondHalf.LocalStartLine;
                --secondHalf.LineCount;
              }*/
              movedInfos.Add( secondHalf );
            }
            /*
            else
            {
              oldInfo.LineCount -= sourceInfo.LineCount;
            }*/
          }
        }
      }
      foreach ( Types.ASM.SourceInfo oldInfo in movedInfos )
      {
        foreach ( int key in m_ASMFileInfo.SourceInfo.Keys )
        {
          if ( m_ASMFileInfo.SourceInfo[key] == oldInfo )
          {
            m_ASMFileInfo.SourceInfo.Remove( key );
            break;
          }
        }
      }

      bool    dumpInfos = false;

      if ( m_ASMFileInfo.SourceInfo.ContainsKey( sourceInfo.GlobalStartLine ) )
      {
        Debug.Log( "Source Info already exists at global line index " + sourceInfo.GlobalStartLine );
        return;
      }

      m_ASMFileInfo.SourceInfo.Add( sourceInfo.GlobalStartLine, sourceInfo );
      foreach ( Types.ASM.SourceInfo oldInfo in movedInfos )
      {
        if ( oldInfo.LineCount != 0 )
        {
          if ( m_ASMFileInfo.SourceInfo.ContainsKey( oldInfo.GlobalStartLine ) )
          {
            Debug.Log( "Trying to insert duplicate source info at global line index " + oldInfo.GlobalStartLine );
            dumpInfos = true;
          }
          else
          {
            m_ASMFileInfo.SourceInfo.Add( oldInfo.GlobalStartLine, oldInfo );
          }
        }
      }

      if ( dumpInfos )
      {
        // dump source infos
        int fullLines = 0;
        foreach ( var pair in m_ASMFileInfo.SourceInfo )
        {
          var info = pair.Value;
          //Debug.Log( "Key " + pair.Key + ": Source from " + info.GlobalStartLine + ", " + info.LineCount + " lines, from file " + info.Filename + " at offset " + info.LocalStartLine );
          Debug.Log( "From " + info.GlobalStartLine + " to " + ( info.GlobalStartLine + info.LineCount - 1 ) + ", " + info.LineCount + " lines, from file " + GR.Path.GetFileNameWithoutExtension( info.Filename ) + " at offset " + info.LocalStartLine );
          fullLines += info.LineCount;
        }
        Debug.Log( "Total " + fullLines + " lines" );
      }
    }



    public bool ParseLineInParameters( List<Token> lineTokenInfos, int Offset, int Count, int LineIndex, bool AllowEmptyParams, out List<List<Token>> lineParams )
    {
      int     paramStartIndex = Offset;
      int     bracketStackDepth = 0;

      lineParams = new List<List<Token>>();

      for ( int i = 0; i < Count; ++i )
      {
        var token = lineTokenInfos[Offset + i];

        if ( token.Content == "(" )
        {
          ++bracketStackDepth;
          continue;
        }
        if ( token.Content == ")" )
        {
          --bracketStackDepth;
          continue;
        }
        if ( bracketStackDepth > 0 )
        {
          continue;
        }
        if ( token.Content == "," )
        {
          if ( ( !AllowEmptyParams )
          &&   ( Offset + i == paramStartIndex ) )
          {
            // empty?
            AddError( LineIndex, ErrorCode.E1000_SYNTAX_ERROR, "Empty Parameter, expected a value or expression", token.StartIndex, token.Content.Length );
            return false;
          }
          lineParams.Add( lineTokenInfos.GetRange( paramStartIndex, Offset + i - paramStartIndex ) );

          paramStartIndex = Offset + i + 1;
          continue;
        }
      }
      if ( ( !AllowEmptyParams )
      &&   ( paramStartIndex >= Offset + Count ) )
      {
        // empty?
        AddError( LineIndex, ErrorCode.E1000_SYNTAX_ERROR, "Empty Parameter, expected a value or expression", lineTokenInfos[lineTokenInfos.Count - 1].StartIndex, 1 );
        return false;
      }
      lineParams.Add( lineTokenInfos.GetRange( paramStartIndex, Offset + Count - paramStartIndex ) );

      return true;
    }




  }
}
