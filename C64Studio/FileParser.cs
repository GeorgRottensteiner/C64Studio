using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio
{
  public class FileParser
  {
    public class LineInfo
    {
      public string AddressSource = "";
      public int    AddressStart = -1;
      public int    PseudoPCOffset = -1;
      public int    NumBytes = 0;
      public int    LineIndex = -1;
      public string Line = "";
      public string Zone = "";
      public List<string> NeededParsedExpression = null;
      public Opcode     Opcode = null;
      public GR.Memory.ByteBuffer     LineData = null;
    };

    public class SourceInfo
    {
      public string     Filename = "";
      public string     FilenameParent = "";
      public int        GlobalStartLine = 0;
      public int        LocalStartLine = 0;
      public int        LineCount = 0;
    };

    public class TokenInfo
    {
      public enum Types
      {
        UNKNOWN = 0,
        LABEL,
        CONSTANT_1,
        CONSTANT_2
      };

      public Types      Type = Types.UNKNOWN;
      public string     Name = "";
      public int        AddressOrValue = -1;
      public int        LineIndex = -1;
      public bool       Used = false;
    };

    public class UnparsedEvalInfo
    {
      public string     Name = "";
      public string     ToEval = "";
      public int        LineIndex = -1;
      public bool       Used = false;
    };

    public class ParseMessage
    {
      public enum LineType
      {
        NONE,
        WARNING,
        ERROR
      };

      public ParseMessage( LineType Type, string Message )
      {
        this.Type     = Type;
        this.Message  = Message;
      }

      public LineType     Type = LineType.NONE;
      public string       Message = null;
    };

    public enum CompileTargetType
    {
      NONE,
      PLAIN,
      PRG,
      T64,
      CARTRIDGE_8K_BIN,
      CARTRIDGE_8K_CRT,
      CARTRIDGE_16K_BIN,
      CARTRIDGE_16K_CRT
    };

    public class Opcode
    {
      public enum AddressingType
      {
        UNKNOWN,
        IMPLICIT,
        DIRECT,
        ABSOLUTE,
        ABSOLUTE_X,
        ABSOLUTE_Y,
        ZEROPAGE,
        ZEROPAGE_X,
        ZEROPAGE_Y,
        INDIRECT,
        INDIRECT_X,
        INDIRECT_Y,
        RELATIVE
      };

      public string         Mnemonic = "";
      public int            ByteValue = -1;
      public int            NumOperands = -1;
      public AddressingType Addressing = AddressingType.UNKNOWN;

      public Opcode( string Mnemonic, int ByteValue, AddressingType Addressing )
      {
        this.Mnemonic   = Mnemonic;
        this.ByteValue  = ByteValue;
        NumOperands     = 0;
        this.Addressing = Addressing;
      }

      public Opcode( string Mnemonic, int ByteValue, int NumOperands, AddressingType Addressing )
      {
        this.Mnemonic     = Mnemonic;
        this.ByteValue    = ByteValue;
        this.NumOperands  = NumOperands;
        this.Addressing   = Addressing;
      }
    };

    private string                      tokenStartAllowedChars = ".ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_!$%#*+-";

    private string                      tokenAllowedChars = ".ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_#";

    private string                      tokenAllowedExpressionOperators = "=,()+-/*%&|<>~{}";

    private List<string>                m_OperatorPrecedence = new List<string> { "=", "!=", "<", ">", "<=", ">=", "<<", ">>", "&", "AND", "|", "OR", "^", "EOR", "*", "/", "+", "-" };

    private Dictionary<string,List<Opcode>> m_Opcodes = new Dictionary<string,List<Opcode>>();


    private Dictionary<string, UnparsedEvalInfo> m_UnparsedLabels = new Dictionary<string, UnparsedEvalInfo>();
    private Dictionary<string, TokenInfo> m_Labels = new Dictionary<string, TokenInfo>();

    public Dictionary<int, LineInfo>    m_LineInfo = new Dictionary<int, LineInfo>();
    private Dictionary<string, int>     m_LoadedFiles = new Dictionary<string, int>();
    public Dictionary<int,int>          AddressToLine = new Dictionary<int,int>();

    public Dictionary<int,SourceInfo>   m_SourceInfo = new Dictionary<int, SourceInfo>();

    public GR.Collections.MultiMap<int, ParseMessage> Messages = new GR.Collections.MultiMap<int, ParseMessage>();

    private int                         m_ErrorMessages = 0;
    private int                         m_WarningMessages = 0;

    private string                      m_DocBasePath = "";
    private string                      m_Filename = "";

    public GR.Memory.ByteBuffer         Assembly = null;

    private CompileTargetType           m_CompileTarget = CompileTargetType.PRG;

    private string                      m_CompileTargetFile = null;

    private int                         m_CompileCurrentAddress = -1;

    private bool                        m_TextModeIsRaw = true;

    public GR.Collections.Set<string>   ExternallyIncludedFiles = new GR.Collections.Set<string>();



    public FileParser()
    {
      AddOpcode( "ADC", 0x6d, 2, Opcode.AddressingType.ABSOLUTE );      // ADC $hhll
      AddOpcode( "ADC", 0x7d, 2, Opcode.AddressingType.ABSOLUTE_X );      // ADC $hhll, X
      AddOpcode( "ADC", 0x79, 2, Opcode.AddressingType.ABSOLUTE_Y );      // ADC $hhll, Y
      AddOpcode( "ADC", 0x65, 1, Opcode.AddressingType.ZEROPAGE );      // ADC $ll
      AddOpcode( "ADC", 0x75, 1, Opcode.AddressingType.ZEROPAGE_X );      // ADC $ll, X
      AddOpcode( "ADC", 0x71, 1, Opcode.AddressingType.INDIRECT_Y );      // ADC ($ll), Y
      AddOpcode( "ADC", 0x61, 1, Opcode.AddressingType.INDIRECT_X );      // ADC ($ll,X)
      AddOpcode( "ADC", 0x69, 1, Opcode.AddressingType.DIRECT );      // ADC #$nn
      AddOpcode( "AND", 0x2d, 2, Opcode.AddressingType.ABSOLUTE );      // AND $hhll
      AddOpcode( "AND", 0x3d, 2, Opcode.AddressingType.ABSOLUTE_X );      // AND $hhll, X
      AddOpcode( "AND", 0x39, 2, Opcode.AddressingType.ABSOLUTE_Y );      // AND $hhll, Y
      AddOpcode( "AND", 0x25, 1, Opcode.AddressingType.ZEROPAGE );      // AND $ll
      AddOpcode( "AND", 0x35, 1, Opcode.AddressingType.ZEROPAGE_X );      // AND $ll, X
      AddOpcode( "AND", 0x31, 1, Opcode.AddressingType.INDIRECT_Y );      // AND ($ll), Y
      AddOpcode( "AND", 0x21, 1, Opcode.AddressingType.INDIRECT_X );      // AND ($ll,X)
      AddOpcode( "AND", 0x29, 1, Opcode.AddressingType.DIRECT );      // AND #$nn
      AddOpcode( "ASL", 0x0a, 0, Opcode.AddressingType.IMPLICIT );      // ASL
      AddOpcode( "ASL", 0x0e, 2, Opcode.AddressingType.ABSOLUTE );      // ASL $hhll
      AddOpcode( "ASL", 0x1e, 2, Opcode.AddressingType.ABSOLUTE_X );      // ASL $hhll, X
      AddOpcode( "ASL", 0x06, 1, Opcode.AddressingType.ZEROPAGE );      // ASL $ll
      AddOpcode( "ASL", 0x16, 1, Opcode.AddressingType.ZEROPAGE_X );      // ASL $ll, X
      AddOpcode( "BCC", 0x90, 1, Opcode.AddressingType.RELATIVE );      // BCC $hhll
      AddOpcode( "BCS", 0xB0, 1, Opcode.AddressingType.RELATIVE );      // BCS $hhll
      AddOpcode( "BEQ", 0xF0, 1, Opcode.AddressingType.RELATIVE );      // BEQ $hhll
      AddOpcode( "BIT", 0x2c, 2, Opcode.AddressingType.ABSOLUTE );      // BIT $hhll
      AddOpcode( "BIT", 0x24, 1, Opcode.AddressingType.ZEROPAGE );      // BIT $ll
      AddOpcode( "BMI", 0x30, 1, Opcode.AddressingType.RELATIVE );      // BMI $hhll
      AddOpcode( "BNE", 0xD0, 1, Opcode.AddressingType.RELATIVE );      // BNE $hhll
      AddOpcode( "BPL", 0x10, 1, Opcode.AddressingType.RELATIVE );      // BPL $hhll
      AddOpcode( "BRK", 0x00, 0, Opcode.AddressingType.IMPLICIT );      // BRK
      AddOpcode( "BVC", 0x50, 1, Opcode.AddressingType.RELATIVE );      // BVC $hhll
      AddOpcode( "BVS", 0x70, 1, Opcode.AddressingType.RELATIVE );      // BVS $hhll
      AddOpcode( "CLC", 0x18, 0, Opcode.AddressingType.IMPLICIT );      // CLC
      AddOpcode( "CLD", 0xD8, 0, Opcode.AddressingType.IMPLICIT );      // CLD
      AddOpcode( "CLI", 0x58, 0, Opcode.AddressingType.IMPLICIT );      // CLI
      AddOpcode( "CLV", 0xB8, 0, Opcode.AddressingType.IMPLICIT );      // CLV
      AddOpcode( "CMP", 0xCD, 2, Opcode.AddressingType.ABSOLUTE );      // CMP $hhll
      AddOpcode( "CMP", 0xDD, 2, Opcode.AddressingType.ABSOLUTE_X );      // CMP $hhll, X
      AddOpcode( "CMP", 0xD9, 2, Opcode.AddressingType.ABSOLUTE_Y );      // CMP $hhll, Y
      AddOpcode( "CMP", 0xC5, 1, Opcode.AddressingType.ZEROPAGE );      // CMP $ll
      AddOpcode( "CMP", 0xD5, 1, Opcode.AddressingType.ZEROPAGE_X );      // CMP $ll, X
      AddOpcode( "CMP", 0xD1, 1, Opcode.AddressingType.INDIRECT_Y );      // CMP ($ll), Y
      AddOpcode( "CMP", 0xC1, 1, Opcode.AddressingType.INDIRECT_X );      // CMP ($ll,X)
      AddOpcode( "CMP", 0xC9, 1, Opcode.AddressingType.DIRECT );      // CMP #$nn
      AddOpcode( "CPX", 0xEC, 2, Opcode.AddressingType.ABSOLUTE );      // CPX $hhll
      AddOpcode( "CPX", 0xE4, 1, Opcode.AddressingType.ZEROPAGE );      // CPX $ll
      AddOpcode( "CPX", 0xE0, 1, Opcode.AddressingType.DIRECT );      // CPX #$nn
      AddOpcode( "CPY", 0xCC, 2, Opcode.AddressingType.ABSOLUTE );      // CPY $hhll
      AddOpcode( "CPY", 0xC4, 1, Opcode.AddressingType.ZEROPAGE );      // CPY $ll
      AddOpcode( "CPY", 0xC0, 1, Opcode.AddressingType.DIRECT );      // CPY #$nn
      AddOpcode( "DEC", 0xCE, 2, Opcode.AddressingType.ABSOLUTE );      // DEC $hhll
      AddOpcode( "DEC", 0xDE, 2, Opcode.AddressingType.ABSOLUTE_X );      // DEC $hhll, X
      AddOpcode( "DEC", 0xC6, 1, Opcode.AddressingType.ZEROPAGE );      // DEC $ll
      AddOpcode( "DEC", 0xD6, 1, Opcode.AddressingType.ZEROPAGE_X );      // DEC $ll, X
      AddOpcode( "DEX", 0xCA, 0, Opcode.AddressingType.IMPLICIT );      // DEX
      AddOpcode( "DEY", 0x88, 0, Opcode.AddressingType.IMPLICIT );      // DEY
      AddOpcode( "EOR", 0x4D, 2, Opcode.AddressingType.ABSOLUTE );      // EOR $hhll
      AddOpcode( "EOR", 0x5D, 2, Opcode.AddressingType.ABSOLUTE_X );      // EOR $hhll, X
      AddOpcode( "EOR", 0x59, 2, Opcode.AddressingType.ABSOLUTE_Y );      // EOR $hhll, Y
      AddOpcode( "EOR", 0x45, 1, Opcode.AddressingType.ZEROPAGE );      // EOR $ll
      AddOpcode( "EOR", 0x55, 1, Opcode.AddressingType.ZEROPAGE_X );      // EOR $ll, X
      AddOpcode( "EOR", 0x51, 1, Opcode.AddressingType.INDIRECT_Y );      // EOR ($ll), Y
      AddOpcode( "EOR", 0x41, 1, Opcode.AddressingType.INDIRECT_X );      // EOR ($ll,X)
      AddOpcode( "EOR", 0x49, 1, Opcode.AddressingType.DIRECT );      // EOR #$nn
      AddOpcode( "INC", 0xEE, 2, Opcode.AddressingType.ABSOLUTE );      // INC $hhll
      AddOpcode( "INC", 0xFE, 2, Opcode.AddressingType.ABSOLUTE_X );      // INC $hhll, X
      AddOpcode( "INC", 0xE6, 1, Opcode.AddressingType.ZEROPAGE );      // INC $ll
      AddOpcode( "INC", 0xF6, 1, Opcode.AddressingType.ZEROPAGE_X );      // INC $ll, X
      AddOpcode( "INX", 0xE8, 0, Opcode.AddressingType.IMPLICIT );      // INX
      AddOpcode( "INY", 0xC8, 0, Opcode.AddressingType.IMPLICIT );      // INY
      AddOpcode( "JMP", 0x4C, 2, Opcode.AddressingType.ABSOLUTE );      // JMP $hhll
      AddOpcode( "JMP", 0x6C, 2, Opcode.AddressingType.INDIRECT );      // JMP ($hhll)
      AddOpcode( "JSR", 0x20, 2, Opcode.AddressingType.ABSOLUTE );      // JSR $hhll
      AddOpcode( "LDA", 0xAD, 2, Opcode.AddressingType.ABSOLUTE );      // LDA $hhll
      AddOpcode( "LDA", 0xBD, 2, Opcode.AddressingType.ABSOLUTE_X );      // LDA $hhll, X
      AddOpcode( "LDA", 0xB9, 2, Opcode.AddressingType.ABSOLUTE_Y );      // LDA $hhll, Y
      AddOpcode( "LDA", 0xA5, 1, Opcode.AddressingType.ZEROPAGE );      // LDA $ll
      AddOpcode( "LDA", 0xB5, 1, Opcode.AddressingType.ZEROPAGE_X );      // LDA $ll, X
      AddOpcode( "LDA", 0xB1, 1, Opcode.AddressingType.INDIRECT_Y );      // LDA ($ll), Y
      AddOpcode( "LDA", 0xA1, 1, Opcode.AddressingType.INDIRECT_X );      // LDA ($ll,X)
      AddOpcode( "LDA", 0xA9, 1, Opcode.AddressingType.DIRECT );      // LDA #$nn
      AddOpcode( "LDX", 0xAE, 2, Opcode.AddressingType.ABSOLUTE );      // LDX $hhll
      AddOpcode( "LDX", 0xBE, 2, Opcode.AddressingType.ABSOLUTE_Y );      // LDX $hhll, Y
      AddOpcode( "LDX", 0xA6, 1, Opcode.AddressingType.ZEROPAGE );      // LDX $ll
      AddOpcode( "LDX", 0xB6, 1, Opcode.AddressingType.ZEROPAGE_Y );      // LDX $ll, Y
      AddOpcode( "LDX", 0xA2, 1, Opcode.AddressingType.DIRECT );      // LDX #$nn
      AddOpcode( "LDY", 0xAC, 2, Opcode.AddressingType.ABSOLUTE );      // LDY $hhll
      AddOpcode( "LDY", 0xBC, 2, Opcode.AddressingType.ABSOLUTE_X );      // LDY $hhll, X
      AddOpcode( "LDY", 0xA4, 1, Opcode.AddressingType.ZEROPAGE );      // LDY $ll
      AddOpcode( "LDY", 0xB4, 1, Opcode.AddressingType.ZEROPAGE_X );      // LDY $ll, X
      AddOpcode( "LDY", 0xA0, 1, Opcode.AddressingType.DIRECT );      // LDY #$nn
      AddOpcode( "LSR", 0x4A, 0, Opcode.AddressingType.IMPLICIT );      // LSR
      AddOpcode( "LSR", 0x4E, 2, Opcode.AddressingType.ABSOLUTE );      // LSR $hhll
      AddOpcode( "LSR", 0x5E, 2, Opcode.AddressingType.ABSOLUTE_X );      // LSR $hhll, X
      AddOpcode( "LSR", 0x46, 1, Opcode.AddressingType.ZEROPAGE );      // LSR $ll
      AddOpcode( "LSR", 0x56, 1, Opcode.AddressingType.ZEROPAGE_X );      // LSR $ll, X
      AddOpcode( "NOP", 0xEA, 0, Opcode.AddressingType.IMPLICIT );      // NOP
      AddOpcode( "ORA", 0x0D, 2, Opcode.AddressingType.ABSOLUTE );      // ORA $hhll
      AddOpcode( "ORA", 0x1D, 2, Opcode.AddressingType.ABSOLUTE_X );      // ORA $hhll, X
      AddOpcode( "ORA", 0x19, 2, Opcode.AddressingType.ABSOLUTE_Y );      // ORA $hhll, Y
      AddOpcode( "ORA", 0x05, 1, Opcode.AddressingType.ZEROPAGE );      // ORA $ll
      AddOpcode( "ORA", 0x15, 1, Opcode.AddressingType.ZEROPAGE_X );      // ORA $ll, X
      AddOpcode( "ORA", 0x11, 1, Opcode.AddressingType.INDIRECT_Y );      // ORA ($ll), Y
      AddOpcode( "ORA", 0x01, 1, Opcode.AddressingType.INDIRECT_X );      // ORA ($ll,X)
      AddOpcode( "ORA", 0x09, 1, Opcode.AddressingType.DIRECT );      // ORA #$nn
      AddOpcode( "PHA", 0x48, 0, Opcode.AddressingType.IMPLICIT );      // PHA
      AddOpcode( "PHP", 0x08, 0, Opcode.AddressingType.IMPLICIT );      // PHP
      AddOpcode( "PLA", 0x68, 0, Opcode.AddressingType.IMPLICIT );      // PLA
      AddOpcode( "PLP", 0x28, 0, Opcode.AddressingType.IMPLICIT );      // PLP
      AddOpcode( "ROL", 0x2A, 0, Opcode.AddressingType.IMPLICIT );      // ROL
      AddOpcode( "ROL", 0x2E, 2, Opcode.AddressingType.ABSOLUTE );      // ROL $hhll
      AddOpcode( "ROL", 0x3E, 2, Opcode.AddressingType.ABSOLUTE_X );      // ROL $hhll, X
      AddOpcode( "ROL", 0x26, 1, Opcode.AddressingType.ZEROPAGE );      // ROL $ll
      AddOpcode( "ROL", 0x36, 1, Opcode.AddressingType.ZEROPAGE_X );      // ROL $ll, X
      AddOpcode( "ROR", 0x6A, 0, Opcode.AddressingType.IMPLICIT );      // ROR
      AddOpcode( "ROR", 0x6E, 2, Opcode.AddressingType.ABSOLUTE );      // ROR $hhll
      AddOpcode( "ROR", 0x7E, 2, Opcode.AddressingType.ABSOLUTE_X );      // ROR $hhll, X
      AddOpcode( "ROR", 0x66, 1, Opcode.AddressingType.ZEROPAGE );      // ROR $ll
      AddOpcode( "ROR", 0x76, 1, Opcode.AddressingType.ZEROPAGE_X );      // ROR $ll, X
      AddOpcode( "RTI", 0x40, 0, Opcode.AddressingType.IMPLICIT );      // RTI
      AddOpcode( "RTS", 0x60, 0, Opcode.AddressingType.IMPLICIT );      // RTS
      AddOpcode( "SBC", 0xED, 2, Opcode.AddressingType.ABSOLUTE );      // SBC $hhll
      AddOpcode( "SBC", 0xFD, 2, Opcode.AddressingType.ABSOLUTE_X );      // SBC $hhll, X
      AddOpcode( "SBC", 0xF9, 2, Opcode.AddressingType.ABSOLUTE_Y );      // SBC $hhll, Y
      AddOpcode( "SBC", 0xE5, 1, Opcode.AddressingType.ZEROPAGE );      // SBC $ll
      AddOpcode( "SBC", 0xF5, 1, Opcode.AddressingType.ZEROPAGE_X );      // SBC $ll, X
      AddOpcode( "SBC", 0xF1, 1, Opcode.AddressingType.INDIRECT_Y );      // SBC ($ll), Y
      AddOpcode( "SBC", 0xE1, 1, Opcode.AddressingType.INDIRECT_X );      // SBC ($ll,X)
      AddOpcode( "SBC", 0xE9, 1, Opcode.AddressingType.DIRECT );      // SBC #$nn
      AddOpcode( "SEC", 0x38, 0, Opcode.AddressingType.IMPLICIT );      // SEC
      AddOpcode( "SED", 0xF8, 0, Opcode.AddressingType.IMPLICIT );      // SED
      AddOpcode( "SEI", 0x78, 0, Opcode.AddressingType.IMPLICIT );      // SEI
      AddOpcode( "STA", 0x8D, 2, Opcode.AddressingType.ABSOLUTE );      // STA $hhll
      AddOpcode( "STA", 0x9D, 2, Opcode.AddressingType.ABSOLUTE_X );      // STA $hhll, X
      AddOpcode( "STA", 0x99, 2, Opcode.AddressingType.ABSOLUTE_Y );      // STA $hhll, Y
      AddOpcode( "STA", 0x85, 1, Opcode.AddressingType.ZEROPAGE );      // STA $ll
      AddOpcode( "STA", 0x95, 1, Opcode.AddressingType.ZEROPAGE_X );      // STA $ll, X
      AddOpcode( "STA", 0x91, 1, Opcode.AddressingType.INDIRECT_Y );      // STA ($ll), Y
      AddOpcode( "STA", 0x81, 1, Opcode.AddressingType.INDIRECT_X );      // STA ($ll,X)
      AddOpcode( "STX", 0x8E, 2, Opcode.AddressingType.ABSOLUTE );      // STX $hhll
      AddOpcode( "STX", 0x86, 1, Opcode.AddressingType.ZEROPAGE );      // STX $ll
      AddOpcode( "STX", 0x96, 1, Opcode.AddressingType.ZEROPAGE_Y );      // STX $ll, Y
      AddOpcode( "STY", 0x8C, 2, Opcode.AddressingType.ABSOLUTE );      // STY $hhll
      AddOpcode( "STY", 0x84, 1, Opcode.AddressingType.ZEROPAGE );      // STY $ll
      AddOpcode( "STY", 0x94, 1, Opcode.AddressingType.ZEROPAGE_X );      // STY $ll, X
      AddOpcode( "TAX", 0xAA, 0, Opcode.AddressingType.IMPLICIT );      // TAX
      AddOpcode( "TAY", 0xA8, 0, Opcode.AddressingType.IMPLICIT );      // TAY
      AddOpcode( "TSX", 0xBA, 0, Opcode.AddressingType.IMPLICIT );      // TSX
      AddOpcode( "TXA", 0x8A, 0, Opcode.AddressingType.IMPLICIT );      // TXA
      AddOpcode( "TXS", 0x9A, 0, Opcode.AddressingType.IMPLICIT );      // TXS
      AddOpcode( "TYA", 0x98, 0, Opcode.AddressingType.IMPLICIT );      // TYA
    }



    public CompileTargetType CompileTarget
    {
      get
      {
        return m_CompileTarget;
      }
    }



    public string CompileTargetFile
    {
      get
      {
        return m_CompileTargetFile;
      }
    }



    public int Errors
    {
      get
      {
        return m_ErrorMessages;
      }
    }



    public int Warnings
    {
      get
      {
        return m_WarningMessages;
      }
    }



    private void AddOpcode( string Opcode, int ByteValue, int NumOperands, Opcode.AddressingType Addressing )
    {
      if ( !m_Opcodes.ContainsKey( Opcode ) )
      {
        m_Opcodes.Add( Opcode, new List<Opcode>() );
      }
      m_Opcodes[Opcode].Add( new Opcode( Opcode, ByteValue, NumOperands, Addressing ) );
    }



    public void AddError( int Line, string Text )
    {
      ParseMessage ErrorMessage = new ParseMessage( ParseMessage.LineType.ERROR, Text );
      Messages.Add( Line, ErrorMessage );
      ++m_ErrorMessages;
    }



    public void AddWarning( int Line, string Text )
    {
      ParseMessage WarningMessage = new ParseMessage( ParseMessage.LineType.WARNING, Text );
      Messages.Add( Line, WarningMessage );
      ++m_WarningMessages;
    }



    public void AddLabel( string Name, int Value, int SourceLine )
    {
      if ( !m_Labels.ContainsKey( Name ) )
      {
        TokenInfo token = new TokenInfo();
        token.Type            = TokenInfo.Types.LABEL;
        token.AddressOrValue  = Value;
        token.Name            = Name;
        token.LineIndex       = SourceLine;
        m_Labels.Add( Name, token );
      }
      else
      {
        if ( m_Labels[Name].AddressOrValue != Value )
        {
          if ( Name != "*" )
          {
            AddError( SourceLine, "Redefinition of label " + Name );
          }
        }
        m_Labels[Name].AddressOrValue = Value;
      }
    }



    public void AddConstant( string Name, int Value, int SourceLine )
    {
      if ( !m_Labels.ContainsKey( Name ) )
      {
        TokenInfo token = new TokenInfo();
        token.Type            = TokenInfo.Types.CONSTANT_2;
        token.AddressOrValue  = Value;
        token.Name            = Name;
        token.LineIndex       = SourceLine;
        token.Used            = true;

        if ( Value < 256 )
        {
          token.Type = TokenInfo.Types.CONSTANT_1;
        }

        m_Labels.Add( Name, token );
      }
      else
      {
        if ( m_Labels[Name].AddressOrValue != Value )
        {
          if ( Name != "*" )
          {
            //Debug.Log( "add constant error" );
            AddError( SourceLine, "Redefinition of constant " + Name );
          }
        }
        m_Labels[Name].AddressOrValue = Value;
      }
    }



    public void AddUnparsedLabel( string Name, string Value, int SourceLine )
    {
      if ( !m_UnparsedLabels.ContainsKey( Name ) )
      {
        UnparsedEvalInfo evalInfo = new UnparsedEvalInfo();
        evalInfo.Name       = Name;
        evalInfo.LineIndex  = SourceLine;
        evalInfo.ToEval     = Value;
        m_UnparsedLabels.Add( Name, evalInfo );
      }
      else
      {
        if ( ( String.IsNullOrEmpty( Value ) )
        &&   ( Name != "*" ) )
        {
          AddError( SourceLine, "Redefinition of label " + Name );
        }
        m_UnparsedLabels[Name].ToEval = Value;
      }
    }



    public bool ParseValue( string Value, out int Result )
    {
      Result = -1;
      if ( Value.StartsWith( "$" ) )
      {
        try
        {
          Result = System.Convert.ToInt32( Value.Substring( 1 ), 16 );
          return true;
        }
        catch ( Exception )
        {
          return false;
        }
      }
      else if ( Value.StartsWith( "0x" ) )
      {
        try
        {
          Result = System.Convert.ToInt32( Value.Substring( 2 ), 16 );
          return true;
        }
        catch ( Exception )
        {
          return false;
        }
      }
      else if ( Value.StartsWith( "%" ) )
      {
        try
        {
          Result = System.Convert.ToInt32( Value.Substring( 1 ), 2 );
          return true;
        }
        catch ( Exception )
        {
          string temp = Value.Substring( 1 ).Replace( '#', '1' ).Replace( '.', '0' );

          try
          {
            Result = System.Convert.ToInt32( temp, 2 );
            return true;
          }
          catch ( Exception )
          {
            return false;
          }
        }
      }
      else if ( Value.StartsWith( "'" ) )
      {
        if ( !Value.EndsWith( "'" ) )
        {
          return false;
        }
        if ( Value.Length != 3 )
        {
          return false;
        }
        char  dummy;
        if ( char.TryParse( Value.Substring( 1, 1 ), out dummy ) )
        {
          Result = dummy;
          return true;
        }
        return false;
      }

      if ( Value == "*" )
      {
        if ( m_CompileCurrentAddress == -1 )
        {
          // cannot evaluate yet
          return false;
        }
        Result = m_CompileCurrentAddress;
        return true;
      }

      int   resultValue = 0;
      if ( int.TryParse( Value, out resultValue ) )
      {
        Result = resultValue;
        return true;
      }
      // parse labels
      if ( !m_Labels.ContainsKey( Value ) )
      {
        if ( m_UnparsedLabels.ContainsKey( Value ) )
        {
          m_UnparsedLabels[Value].Used = true;
        }
        return false;
      }
      Result = m_Labels[Value].AddressOrValue;
      m_Labels[Value].Used = true;
      return true;
    }



    private int CountChar( string Source, char Char )
    {
      int count = 0;

      for ( int i = 0; i < Source.Length; ++i )
      {
        if ( Source[i] == Char )
        {
          ++count;
        }
      }
      return count;
    }



    private bool EvaluateLabel( string LabelContent, out int Result )
    {
      List<string>  tokens = ParseExpressionTokens( LabelContent );

      //dh.Log( "Eval Label (" + LabelContent + ") = " + tokens.Count + " parts" );
      return EvaluateTokens( tokens, out Result );
    }



    private bool HandleOperator( string Operator, string Token1String, string Token2String, out int Result )
    {
      Result = -1;

      int Token1 = -1;
      int Token2 = -1;

      if ( ( !ParseValue( Token1String, out Token1 ) )
      ||   ( !ParseValue( Token2String, out Token2 ) ) )
      {
        return false;
      }


      if ( Operator == "*" )
      {
        Result = Token1 * Token2;
        return true;
      }
      else if ( Operator == "/" )
      {
        Result = Token1 / Token2;
        return true;
      }
      else if ( Operator == "+" )
      {
        Result = Token1 + Token2;
        return true;
      }
      else if ( Operator == "-" )
      {
        Result = Token1 - Token2;
        return true;
      }
      else if ( ( Operator == "&" )
      ||        ( Operator == "AND" ) )
      {
        Result = Token1 & Token2;
        return true;
      }
      else if ( ( Operator == "EOR" )
      ||        ( Operator == "^" ) )
      {
        Result = Token1 ^ Token2;
        return true;
      }
      else if ( ( Operator == "|" )
      ||        ( Operator == "OR" ) )
      {
        Result = Token1 | Token2;
        return true;
      }
      else if ( Operator == ">>" )
      {
        Result = Token1 >> Token2;
        return true;
      }
      else if ( Operator == "<<" )
      {
        Result = Token1 << Token2;
        return true;
      }
      else if ( Operator == "=" )
      {
        if ( Token1 == Token2 )
        {
          Result = 1;
        }
        else
        {
          Result = 0;
        }
        return true;
      }
      else if ( Operator == "!=" )
      {
        if ( Token1 != Token2 )
        {
          Result = 1;
        }
        else
        {
          Result = 0;
        }
        return true;
      }
      else if ( Operator == ">" )
      {
        if ( Token1 > Token2 )
        {
          Result = 1;
        }
        else
        {
          Result = 0;
        }
        return true;
      }
      else if ( Operator == "<" )
      {
        if ( Token1 < Token2 )
        {
          Result = 1;
        }
        else
        {
          Result = 0;
        }
        return true;
      }
      else if ( Operator == ">=" )
      {
        if ( Token1 >= Token2 )
        {
          Result = 1;
        }
        else
        {
          Result = 0;
        }
        return true;
      }
      else if ( Operator == "<=" )
      {
        if ( Token1 > Token2 )
        {
          Result = 1;
        }
        else
        {
          Result = 0;
        }
        return true;
      }
      return false;
    }



    private bool EvaluateTokens( List<string> Tokens, out int Result )
    {
      Result = -1;
      if ( Tokens.Count == 0 )
      {
        return false;
      }

      if ( Tokens.Count == 1 )
      {
        return ParseValue( Tokens[0], out Result );
      }

      if ( Tokens[0] == "<" )
      {
        int     value = -1;
        if ( EvaluateTokens( Tokens.GetRange( 1, Tokens.Count - 1 ), out value ) )
        {
          Result = ( value & 0x00ff );
          return true;
        }
        return false;
      }
      else if ( Tokens[0] == ">" )
      {
        int value = -1;
        if ( EvaluateTokens( Tokens.GetRange( 1, Tokens.Count - 1 ), out value ) )
        {
          Result = ( value & 0xff00 ) >> 8;
          return true;
        }
        return false;
      }

      if ( Tokens.Count == 2 )
      {
        if ( Tokens[0] == "-" )
        {
          int     value = -1;
          
          if ( EvaluateTokens( Tokens.GetRange( 1, Tokens.Count - 1 ), out value ) )
          {
            Result = -value;
            return true;
          }
        }
      }

      if ( Tokens.Count < 3 )
      {
        return false;
      }

      bool  evaluatedPart = false;

      do
      {
        evaluatedPart = false;
        // check brackets first

        // find bracket pair
        int     bracketStartPos = -1;
        int     bracketEndPos = -1;

        for ( int i = 0; i < Tokens.Count; ++i )
        {
          if ( Tokens[i] == "(" )
          {
            bracketStartPos = i;
          }
          else if ( Tokens[i] == ")" )
          {
            if ( bracketStartPos == -1 )
            {
              // syntax error!
              // closing bracket without opening bracket
              return false;
            }
            bracketEndPos = i;
            break;
          }
        }
        if ( ( bracketStartPos != -1 )
        &&   ( bracketEndPos == -1 ) )
        {
          // syntax error
          // opening bracket without closing bracket
          return false;
        }

        if ( ( bracketStartPos != -1 )
        &&   ( bracketEndPos != -1 ) )
        {
          int     resultValue = -1;
          if ( !EvaluateTokens( Tokens.GetRange( bracketStartPos + 1, bracketEndPos - bracketStartPos - 1 ), out resultValue ) )
          {
            return false;
          }
          Tokens.RemoveRange( bracketStartPos, bracketEndPos - bracketStartPos + 1 );
          Tokens.Insert( bracketStartPos, resultValue.ToString() );
          evaluatedPart = true;
        }
        if ( Tokens.Count >= 3 )
        {
          foreach ( string oper in m_OperatorPrecedence )
          {
            for ( int tokenIndex = 1; tokenIndex < Tokens.Count - 1; ++tokenIndex )
            {
              if ( Tokens[tokenIndex] == oper )
              {
                // evaluate token now!
                int result = -1;
                if ( HandleOperator( Tokens[tokenIndex], Tokens[tokenIndex - 1], Tokens[tokenIndex + 1], out result ) )
                {
                  Tokens.RemoveRange( tokenIndex - 1, 3 );
                  Tokens.Insert( tokenIndex - 1, result.ToString() );
                  evaluatedPart = true;
                }

                goto operatordone;
              }
            }
          }
          operatordone:;

        }
      }
      while ( evaluatedPart );

      if ( Tokens.Count == 1 )
      {
        return ParseValue( Tokens[0], out Result );
      }
      return false;
    }



    private byte MapTextCharacter( byte Source )
    {
      if ( m_TextModeIsRaw )
      {
        return Source;
      }
	    if ( ( Source >= (byte)'a' )
      &&   ( Source <= (byte)'z' ) )
      {
        return (byte)( Source - 96 );
      }
      if ( ( Source >= (byte)'[' )
      &&   ( Source <= (byte)'_' ) )
      {
        return (byte)( Source - 64 );
      }
      if ( Source >= (byte)'`' ) 
      {
        return (byte)64;
      }
      if ( Source >= (byte)'@' ) 
      {
        return 0;
      }
      return Source;
    }



    private bool DetermineUnparsedLabels()
    {
      bool newLabelDetermined = false;

      // recalc all line start addresses
      int     trueCompileCurrentAddress = -1;
      m_CompileCurrentAddress = -1;
      string    zoneName = "";
      foreach ( int lineIndex in m_LineInfo.Keys )
      {
        LineInfo lineInfo = m_LineInfo[lineIndex];

        if ( m_CompileCurrentAddress == -1 )
        {
          if ( ( lineInfo.NumBytes == 0 )
          &&   ( lineInfo.AddressSource != "*" ) )
          {
            // defines before program counter are allowed
            continue;
          }
          if ( lineInfo.AddressStart == -1 )
          {
            AddError( lineIndex, "Code without start address encountered (missing *=)" );
            return false;
          }
          m_CompileCurrentAddress = lineInfo.AddressStart;
          trueCompileCurrentAddress = m_CompileCurrentAddress;
        }
        if ( lineInfo.AddressSource == "*" )
        {
          // set program counter
          m_CompileCurrentAddress = lineInfo.AddressStart;
          trueCompileCurrentAddress = m_CompileCurrentAddress;
        }
        if ( lineInfo.PseudoPCOffset == -2 )
        {
          m_CompileCurrentAddress = trueCompileCurrentAddress;
        }
        else if ( lineInfo.PseudoPCOffset != -1 )
        {
          m_CompileCurrentAddress = lineInfo.PseudoPCOffset;
        }

        zoneName = lineInfo.Zone;

        if ( lineInfo.AddressStart == -1 )
        {
          lineInfo.AddressStart = m_CompileCurrentAddress;

          string  realLabelname = lineInfo.AddressSource;
          if ( realLabelname.StartsWith( "." ) )
          {
            realLabelname = zoneName + realLabelname;
          }

          if ( m_UnparsedLabels.ContainsKey( realLabelname ) )
          {
            //Debug.Log( "Set unparsed label " + realLabelname );//+ " to " + currentAddress + ",$" + currentAddress.ToString( "x" ) );
            TokenInfo token = new TokenInfo();
            token.Type = TokenInfo.Types.LABEL;
            if ( m_UnparsedLabels[realLabelname].ToEval.Length > 0 )
            {
              List<string>    toEvalToken = ParseTokens( m_UnparsedLabels[realLabelname].ToEval );
              int             evalValue = -1;
              if ( !EvaluateTokens( toEvalToken, out evalValue ) )
              {
                m_CompileCurrentAddress += lineInfo.NumBytes;
                continue;
              }
              token.AddressOrValue = evalValue;
              //EvaluateTokens( toEvalToken );
            }
            else
            {
              token.AddressOrValue = m_CompileCurrentAddress;
            }
            token.Name = realLabelname;
            token.LineIndex = m_UnparsedLabels[realLabelname].LineIndex;
            token.Used = m_UnparsedLabels[realLabelname].Used;
            if ( realLabelname != "*" )
            {
              if ( !m_Labels.ContainsKey( realLabelname ) )
              {
                m_Labels.Add( realLabelname, token );
              }
              else
              {
                AddError( token.LineIndex, "Redefinition of label " + realLabelname );
              }
            }
            m_UnparsedLabels.Remove( realLabelname );
          }
        }
        m_CompileCurrentAddress += lineInfo.NumBytes;
        trueCompileCurrentAddress += lineInfo.NumBytes;
      }

      do
      {
        newLabelDetermined = false;

        redo:;
        foreach ( string label in m_UnparsedLabels.Keys )
        {
          //dh.Log( "Unparsed label:" + label + ", " + m_UnparsedLabels[label].ToEval );
          int     result = -1;
          if ( EvaluateLabel( m_UnparsedLabels[label].ToEval, out result ) )
          {
            //dh.Log( "evaluated unparsed label " + label + " to " + result );
            TokenInfo token = new TokenInfo();
            token.Type            = TokenInfo.Types.LABEL;
            token.AddressOrValue  = result;
            token.Name            = label;
            token.LineIndex       = m_UnparsedLabels[label].LineIndex;
            token.Used            = true;
            m_Labels.Add( label, token );

            m_UnparsedLabels.Remove( label );
            newLabelDetermined = true;
            goto redo;
          }
        }
      }
      while ( newLabelDetermined );

      foreach ( int lineIndex in m_LineInfo.Keys )
      {
        LineInfo lineInfo = m_LineInfo[lineIndex];

        if ( lineInfo.NeededParsedExpression != null )
        {
          if ( lineInfo.NeededParsedExpression.Count == 0 )
          {
            AddError( lineIndex, "Syntax Error" );
            return false;
          }
          // strip prefixed #
          if ( lineInfo.NeededParsedExpression[0].StartsWith( "#" ) )
          {
            if ( lineInfo.NeededParsedExpression[0].Length == 1 )
            {
              lineInfo.NeededParsedExpression.RemoveAt( 0 );
            }
            else
            {
              lineInfo.NeededParsedExpression[0] = lineInfo.NeededParsedExpression[0].Substring( 1 );
            }
          }
          string    lineToCheck = lineInfo.Line;

          if ( !lineToCheck.StartsWith( "!" ) )
          {
            int   spacePos = lineToCheck.IndexOf( " " );
            if ( spacePos != -1 )
            {
              lineToCheck = lineToCheck.Substring( spacePos + 1 ).Trim();
            }
          }

          if ( lineToCheck.StartsWith( "!" ) )
          {
            string startToken = "";
            int spacePos = lineToCheck.IndexOf( ' ' );
            if ( spacePos == -1 )
            {
              startToken = lineToCheck.ToUpper();
            }
            else
            {
              startToken = lineToCheck.Substring( 0, spacePos ).ToUpper();
            }
            if ( ( startToken == "!BYTE" )
            ||   ( startToken == "!BY" )
            ||   ( startToken == "!8" )
            ||   ( startToken == "!08" )
            ||   ( startToken == "!16" )
            ||   ( startToken == "!WO" )
            ||   ( startToken == "!WORD" ) )
            {
              bool    isByte =  ( startToken == "!BYTE" ) || ( startToken == "!BY" ) || ( startToken == "!8" ) || ( startToken == "!08" );

              GR.Memory.ByteBuffer lineData = new GR.Memory.ByteBuffer();

              int tokenIndex = 0;
              int expressionStartIndex = 0;

              do
              {
                if ( lineInfo.NeededParsedExpression[tokenIndex] == "," )
                {
                  // found an expression
                  List<string>    partialList = lineInfo.NeededParsedExpression.GetRange( expressionStartIndex, tokenIndex - expressionStartIndex );
                  int value = -1;

                  if ( !EvaluateTokens( partialList, out value ) )
                  {
                    string errorInfo = "";
                    foreach ( string part in partialList )
                    {
                      errorInfo += part;
                    }
                    AddError( lineIndex, "Could not evaluate " + errorInfo );
                    //return false;
                  }
                  if ( isByte )
                  {
                    lineData.AppendU8( (byte)value );
                  }
                  else
                  {
                    lineData.AppendU16( (ushort)value );
                  }
                  expressionStartIndex = tokenIndex + 1;
                }
                ++tokenIndex;
                if ( tokenIndex == lineInfo.NeededParsedExpression.Count )
                {
                  if ( expressionStartIndex <= tokenIndex - 1 )
                  {
                    // there's still data to evaluate
                    List<string>    partialList = lineInfo.NeededParsedExpression.GetRange( expressionStartIndex, tokenIndex - expressionStartIndex );
                    int value = -1;
                    if ( !EvaluateTokens( partialList, out value ) )
                    {
                      string errorInfo = "";
                      foreach ( string part in partialList )
                      {
                        errorInfo += part;
                      }
                      AddError( lineIndex, "Could not evaluate " + errorInfo );
                      //return false;
                    }
                    if ( isByte )
                    {
                      lineData.AppendU8( (byte)value );
                    }
                    else
                    {
                      lineData.AppendU16( (ushort)value );
                    }
                  }
                }
              }
              while ( tokenIndex < lineInfo.NeededParsedExpression.Count );

              lineInfo.LineData = lineData;
            }
            else if ( ( startToken == "!TEXT" )
            ||        ( startToken == "!TX" )
            ||        ( startToken == "!SCR" ) )
            {
              GR.Memory.ByteBuffer lineData = new GR.Memory.ByteBuffer();

              int tokenIndex = 0;
              int expressionStartIndex = 0;

              do
              {
                if ( lineInfo.NeededParsedExpression[tokenIndex] == "," )
                {
                  // found an expression
                  List<string> partialList = lineInfo.NeededParsedExpression.GetRange( expressionStartIndex, tokenIndex - expressionStartIndex );
                  if ( partialList.Count == 1 )
                  {
                    if ( ( partialList[0].StartsWith( "\"" ) )
                    &&   ( partialList[0].Length > 1 )
                    &&   ( partialList[0].EndsWith( "\"" ) ) )
                    {
                      // a text
                      foreach ( char aChar in partialList[0].Substring( 1, partialList[0].Length - 2 ) )
                      {
                        // TODO - map to PETSCII!
                        lineData.AppendU8( MapTextCharacter( (byte)aChar ) );
                      }
                    }
                    else
                    {
                      int value = -1;
                      if ( !EvaluateTokens( partialList, out value ) )
                      {
                        AddError( lineIndex, "Syntax Error1" );
                        return false;
                      }
                      lineData.AppendU8( MapTextCharacter( (byte)value ) );
                    }
                  }
                  else
                  {
                    int value = -1;
                    if ( !EvaluateTokens( partialList, out value ) )
                    {
                      AddError( lineIndex, "Syntax Error2" );
                      return false;
                    }
                    lineData.AppendU8( MapTextCharacter( (byte)value ) );
                  }
                  expressionStartIndex = tokenIndex + 1;
                }
                ++tokenIndex;
                if ( tokenIndex == lineInfo.NeededParsedExpression.Count )
                {
                  if ( expressionStartIndex <= tokenIndex - 1 )
                  {
                    // there's still data to evaluate
                    List<string> partialList = lineInfo.NeededParsedExpression.GetRange( expressionStartIndex, tokenIndex - expressionStartIndex );
                    if ( partialList.Count == 1 )
                    {
                      if ( ( partialList[0].StartsWith( "\"" ) )
                      &&   ( partialList[0].Length > 1 )
                      &&   ( partialList[0].EndsWith( "\"" ) ) )
                      {
                        // a text
                        foreach ( char aChar in partialList[0].Substring( 1, partialList[0].Length - 2 ) )
                        {
                          // TODO - map to PETSCII!
                          lineData.AppendU8( MapTextCharacter( (byte)aChar ) );
                        }
                      }
                      else
                      {
                        int value = -1;
                        if ( !EvaluateTokens( partialList, out value ) )
                        {
                          AddError( lineIndex, "Syntax Error3" );
                          return false;
                        }
                        lineData.AppendU8( MapTextCharacter( (byte)value ) );
                      }
                    }
                    else
                    {
                      int value = -1;
                      if ( !EvaluateTokens( partialList, out value ) )
                      {
                        AddError( lineIndex, "Syntax Error4" );
                        return false;
                      }
                      lineData.AppendU8( MapTextCharacter( (byte)value ) );
                    }
                  }
                }
              }
              while ( tokenIndex < lineInfo.NeededParsedExpression.Count );

              lineInfo.LineData = lineData;
            }
            else if ( ( startToken == "!FILL" )
            ||        ( startToken == "!FI" ) )
            {
              int tokenCommaIndex = -1;

              for ( int i = 0; i < lineInfo.NeededParsedExpression.Count; ++i )
              {
                if ( lineInfo.NeededParsedExpression[i] == "," )
                {
                  tokenCommaIndex = i;
                  break;
                }
              }
              if ( tokenCommaIndex == -1 )
              {
                AddError( lineIndex, "Could not evaluate !fill expression" );
                return false;
              }

              List<string>    fillCount = lineInfo.NeededParsedExpression.GetRange( 0, tokenCommaIndex );
              List<string>    fillType = lineInfo.NeededParsedExpression.GetRange( tokenCommaIndex + 1, lineInfo.NeededParsedExpression.Count - tokenCommaIndex - 1 );

              int     count = -1;
              int     value = -1;

              if ( !EvaluateTokens( fillCount, out count ) )
              {
                string    expression = "";
                foreach ( string part in fillCount )
                {
                  expression += part;
                }
                AddError( lineIndex, "Could not evaluate " + expression );
              }
              if ( !EvaluateTokens( fillType, out value ) )
              {
                string    expression = "";
                foreach ( string part in fillCount )
                {
                  expression += part;
                }
                AddError( lineIndex, "Could not evaluate " + expression );
              }
              GR.Memory.ByteBuffer lineData = new GR.Memory.ByteBuffer();
              for ( int i = 0; i < count; ++i )
              {
                lineData.AppendU8( (byte)value );
              }
              lineInfo.LineData = lineData;
            }
          }
          else
          {
            int value = -1;
            if ( lineInfo.NeededParsedExpression.Count == 1 )
            {
              if ( lineInfo.NeededParsedExpression[0].StartsWith( "+" ) )
              {
                // special case of forward local label
                string    closestLabel = "";
                int       closestLine = 5000000;
                foreach ( string label in m_Labels.Keys )
                {
                  if ( label.StartsWith( "c64_local_label" + lineInfo.NeededParsedExpression[0] ) )
                  {
                    int lineNo = -1;
                    if ( int.TryParse( label.Substring( ( "c64_local_label" + lineInfo.NeededParsedExpression[0] ).Length ), out lineNo ) )
                    {
                      if ( ( lineNo >= lineIndex )
                      &&   ( lineNo < closestLine ) )
                      {
                        closestLine   = lineNo;
                        closestLabel  = label;
                      }
                    }
                  }
                }
                if ( closestLine != 5000000 )
                {
                  lineInfo.NeededParsedExpression[0] = closestLabel;
                }
              }
            }
            if ( !EvaluateTokens( lineInfo.NeededParsedExpression, out value ) )
            {
              string    fullEvaluation = "";
              Debug.Log( "need to assemble unparsed expression:" );
              foreach ( string extraToken in lineInfo.NeededParsedExpression )
              {
                Debug.Log( "-: " + extraToken );
                fullEvaluation += extraToken;
              }
              Debug.Log( "=> Could not parse!" );
              Debug.Log( "=> from line: " + lineInfo.Line );

              AddError( lineIndex, "Syntax error: Could not evaluate " + fullEvaluation );
            }
            else
            {
              //dh.Log( "Parsed to: " + value );
              if ( lineInfo.Opcode != null )
              {
                if ( lineInfo.Opcode.Addressing == Opcode.AddressingType.RELATIVE )
                {
                  int delta = value - lineInfo.AddressStart - 2;
                  if ( ( delta < -128 )
                  ||   ( delta > 127 ) )
                  {
                    AddError( lineIndex, "Relative jump too far in line " + ( lineIndex + 1 ) );
                    return false;
                  }
                  else
                  {
                    lineInfo.LineData.AppendU8( (byte)delta );
                  }
                }
                else if ( lineInfo.Opcode.NumOperands == 1 )
                {
                  lineInfo.LineData.AppendU8( (byte)value );
                }
                else if ( lineInfo.Opcode.NumOperands == 2 )
                {
                  lineInfo.LineData.AppendU16( (ushort)value );
                }
              }
              lineInfo.NeededParsedExpression = null;
            }
          }
        }
      }

      foreach ( string label in m_UnparsedLabels.Keys )
      {
        //dh.Log( "Still unevaluated label:" + label + ", " + m_UnparsedLabels[label].ToEval );
        AddError( m_UnparsedLabels[label].LineIndex, "Syntax error: Failed to evaluate " + m_UnparsedLabels[label].ToEval );
      }
      if ( m_ErrorMessages > 0 )
      {
        return false;
      }
      return ( m_UnparsedLabels.Count == 0 );
    }



    private void CleanLines( string[] Lines )
    {
      for ( int i = 0; i < Lines.Length; ++i )
      {
        string tempLine = Lines[i].Trim();
        tempLine = tempLine.Replace( '\t', ' ' );

        // truncate comments
        int commentPosTemp = tempLine.IndexOf( ';' );
        if ( commentPosTemp != -1 )
        {
          tempLine = tempLine.Substring( 0, commentPosTemp );
        }
        Lines[i] = tempLine;
      }
    }



    private string[] PreProcess( string[] Lines, string ParentFilename, int LineOffset, ProjectConfig Configuration )
    {
      Stack<bool>               stackDefineBlocks = new Stack<bool>();

      label:;

      //Debug.Log( "PreProcess restart in " + ParentFilename );
      m_Labels.Clear();
      m_UnparsedLabels.Clear();
      stackDefineBlocks.Clear();
      Messages.Clear();
      m_WarningMessages = 0;
      m_ErrorMessages = 0;
      ParsePreDefines( Configuration );


      int   lineIndex = 0;
      foreach ( string line in Lines )
      {
        string parseLine = line;
        int     commentPos = parseLine.IndexOf( ';' );
        if ( commentPos != -1 )
        {
          parseLine = parseLine.Substring( 0, commentPos );
        }

        if ( !parseLine.StartsWith( "}" ) )
        {
          if ( ( stackDefineBlocks.Count > 0 )
          &&   ( !stackDefineBlocks.Peek() ) )
          {
            // defined away
            ++lineIndex;
            continue;
            //parseLine = ";" + parseLine;
          }
        }

        List<string>    lineTokens = ParseTokens( parseLine );

        if ( lineTokens.Count == 0 )
        {
          ++lineIndex;
          continue;
        }

        string upToken = lineTokens[0].ToUpper();
        string labelInFront = "";

        if ( ( lineTokens.Count >= 3 )
        &&   ( lineTokens[1] == "=" ) )
        {
          // a define
          int equPos = parseLine.IndexOf( '=' );
          string defineName = parseLine.Substring( 0, equPos ).Trim();
          string defineValue = parseLine.Substring( equPos + 1 ).Trim();
          List<string>  valueTokens = ParseTokens( defineValue );
          int address = -1;
          if ( !EvaluateTokens( valueTokens, out address ) )
          {
            AddUnparsedLabel( defineName, defineValue, lineIndex );
          }
          else if ( ( address >= 0 )
          && ( address <= 0xffff ) )
          {
            AddConstant( defineName, address, lineIndex );
          }
          else
          {
            AddError( lineIndex, "Evaluated constant out of bounds, " + address + " must be >= 0 and <= 65535" );
          }
          ++lineIndex;
          continue;
        }
        else if ( ( !m_Opcodes.ContainsKey( upToken ) )
        &&        ( !upToken.StartsWith( "!" ) ) )
        {
          // not a token, not a macro, must be a label in front
          if ( upToken == "}" )
          {
            if ( stackDefineBlocks.Count == 0 )
            {
              AddError( lineIndex, "Missing opening brace" );
            }
            else
            {
              if ( ( lineTokens.Count == 3 )
              && ( lineTokens[0] == "}" )
              && ( lineTokens[2] == "{" )
              && ( lineTokens[1].ToUpper() == "ELSE" ) )
              {
                stackDefineBlocks.Push( !stackDefineBlocks.Pop() );
              }
              else if ( lineTokens.Count == 1 )
              {
                stackDefineBlocks.Pop();
              }
              else
              {
                AddError( lineIndex, "Malformed block close statement, expecting single \"}\" or \"} else {\"" );
              }
            }
            ++lineIndex;
            continue;
          }
          labelInFront = lineTokens[0];
          lineTokens.RemoveAt( 0 );
          if ( lineTokens.Count == 0 )
          {
            ++lineIndex;
            continue;
          }
          upToken = lineTokens[0].ToUpper();
        }

        if ( upToken.StartsWith( "!" ) )
        {
          // a macro
          string    macro = upToken;
          if ( macro == "!SOURCE" )
          {
            if ( lineTokens.Count != 2 )
            {
              AddError( lineIndex, "Expecting file name" );
              return null;
            }
            if ( ( !lineTokens[1].StartsWith( "\"" ) )
            ||   ( !lineTokens[1].EndsWith( "\"" ) ) )
            {
              AddError( lineIndex, "File name incomplete" );
              return null;
            }
            string    subFilename = lineTokens[1].Substring( 1, lineTokens[1].Length - 2 );

            if ( m_LoadedFiles.ContainsKey( subFilename ) )
            {
              AddError( lineIndex, "Circular inclusion in line " + lineIndex );
              return null;
            }
            m_LoadedFiles.Add( subFilename, 1 );

            string[]  subFile = null;
            string    subFilenameFull = GR.Path.Append( m_DocBasePath, subFilename );

            ExternallyIncludedFiles.Add( subFilenameFull );
            //Debug.Log( "Read subfile " + subFilename );
            try
            {
              subFile = System.IO.File.ReadAllLines( subFilenameFull );
            }
            catch ( System.IO.IOException )
            {
              AddError( lineIndex, "Could not read file " + subFilenameFull );
              return null;
            }

            CleanLines( subFile );


            //Debug.Log( "Add subfile section at " + ( LineOffset + lineIndex ) + " for " + subFilename + " with " + subFile.Length + " lines" );
            SourceInfo sourceInfo = new SourceInfo();
            sourceInfo.Filename = subFilenameFull;
            sourceInfo.GlobalStartLine = LineOffset + lineIndex;
            sourceInfo.LineCount = subFile.Length;
            sourceInfo.FilenameParent = ParentFilename;

            List<SourceInfo>    movedInfos = new List<SourceInfo>();
            foreach ( SourceInfo oldInfo in m_SourceInfo.Values )
            {
              if ( oldInfo.GlobalStartLine >= sourceInfo.GlobalStartLine )
              {
                // shift down completely
                oldInfo.GlobalStartLine += subFile.Length;
                movedInfos.Add( oldInfo );
              }
              else if ( oldInfo.GlobalStartLine + oldInfo.LineCount > sourceInfo.GlobalStartLine )
              {
                // split!
                SourceInfo    secondHalf = new SourceInfo();
                secondHalf.Filename = oldInfo.Filename;
                secondHalf.FilenameParent = oldInfo.FilenameParent;
                secondHalf.GlobalStartLine = sourceInfo.GlobalStartLine + sourceInfo.LineCount;
                secondHalf.LineCount = oldInfo.LineCount - ( sourceInfo.GlobalStartLine - oldInfo.GlobalStartLine );

                oldInfo.LineCount -= secondHalf.LineCount;

                secondHalf.LocalStartLine = oldInfo.LocalStartLine + oldInfo.LineCount + 1;

                //m_SourceInfo.Add( secondHalf.GlobalStartLine, secondHalf );
                movedInfos.Add( secondHalf );
              }
            }
            foreach ( SourceInfo oldInfo in movedInfos )
            {
              foreach ( int key in m_SourceInfo.Keys )
              {
                if ( m_SourceInfo[key] == oldInfo )
                {
                  m_SourceInfo.Remove( key );
                  break;
                }
              }
            }

            m_SourceInfo.Add( sourceInfo.GlobalStartLine, sourceInfo );
            foreach ( SourceInfo oldInfo in movedInfos )
            {
              m_SourceInfo.Add( oldInfo.GlobalStartLine, oldInfo );
            }

            //Debug.Log( "Preprocess subfile " + subFilenameFull );
            //subFile = PreProcess( subFile, subFilenameFull, LineOffset + lineIndex );
            //Debug.Log( "Preprocess subfile " + subFilenameFull + " done" );

            //Debug.Log( "build full line list" );

            string[] result = new string[Lines.Length + subFile.Length - 1];

            System.Array.Copy( Lines, 0, result, 0, lineIndex );
            System.Array.Copy( subFile, 0, result, lineIndex, subFile.Length );
            System.Array.Copy( Lines, lineIndex + 1, result, lineIndex + subFile.Length, Lines.Length - lineIndex - 1 );

            //Debug.Log( "build full line list done" );

            /*
            foreach ( SourceInfo oldInfo in m_SourceInfo.Values )
            {
              if ( oldInfo.Filename == ParentFilename )
              {
                // is this the one that needs to be split?
                if ( ( oldInfo.GlobalStartLine < LineOffset + lineIndex )
                &&   ( oldInfo.GlobalStartLine + oldInfo.LineCount >= LineOffset + lineIndex ) )
                {
                  // yes!
                  SourceInfo sourceInfoSplit = new SourceInfo();
                  sourceInfoSplit.Filename = oldInfo.Filename;
                  sourceInfoSplit.GlobalStartLine = LineOffset + lineIndex + subFile.Length;//-1;
                  sourceInfoSplit.LocalStartLine += lineIndex;
                  sourceInfoSplit.LineCount = oldInfo.LineCount - lineIndex;
                  sourceInfoSplit.FilenameParent = oldInfo.FilenameParent;

                  m_SourceInfo.Add( sourceInfoSplit.GlobalStartLine, sourceInfoSplit );

                  // modify old info
                  oldInfo.LineCount -= oldInfo.LineCount - lineIndex;
                  break;
                }
              }
            }*/

            DumpSourceInfos();
            //Debug.Log( "replace lines" );
            Lines = result;
            //Debug.Log( "replace lines done" );
            //Debug.Log( "Restart after !source" );
            goto label;
          }
          else if ( ( macro == "!BINARY" )
          ||        ( macro == "!BIN" )
          ||        ( macro == "!BI" ) )
          {
            List<string> tokens = lineTokens;

            int             paramPos = 0;
            List<string>    paramsFile = new List<string>();
            List<string>    paramsSize = new List<string>();
            List<string>    paramsSkip = new List<string>();
            for ( int i = 1; i < tokens.Count; ++i )
            {
              if ( tokens[i] == "," )
              {
                ++paramPos;
                if ( paramPos > 2 )
                {
                  AddError( lineIndex, "Macro not formatted as expected. Expected !binary <Filename>,<Size>,<Skip>" );
                  return null;
                }
              }
              else
              {
                switch ( paramPos )
                {
                  case 0:
                    paramsFile.Add( tokens[i] );
                    break;
                  case 1:
                    paramsSize.Add( tokens[i] );
                    break;
                  case 2:
                    paramsSkip.Add( tokens[i] );
                    break;
                }
              }
            }
            if ( ( paramPos > 2 )
            ||   ( paramsFile.Count != 1 ) )
            {
              AddError( lineIndex, "Macro not formatted as expected. Expected !binary <Filename>,<Size>,<Skip>" );
              return null;
            }
            if ( ( !paramsFile[0].StartsWith( "\"" ) )
            ||   ( !paramsFile[0].EndsWith( "\"" ) ) )
            {
              AddError( lineIndex, "Expected proper file name between apostrophes" );
              return null;
            }
            string    subFilename = paramsFile[0].Substring( 1, paramsFile[0].Length - 2 );

            if ( m_LoadedFiles.ContainsKey( subFilename ) )
            {
              AddError( lineIndex, "Circular inclusion for " + subFilename );
              return null;
            }
            m_LoadedFiles.Add( subFilename, 1 );

            int     fileSize = -1;
            int     fileSkip = -1;
            bool    fileSizeValid = EvaluateTokens( paramsSize, out fileSize );
            bool    fileSkipValid = EvaluateTokens( paramsSkip, out fileSkip );

            byte[] subFile = null;

            try
            {
              subFile = System.IO.File.ReadAllBytes( GR.Path.Append( m_DocBasePath, subFilename ) );
            }
            catch ( System.IO.IOException )
            {
              AddError( lineIndex, "Could not read file " + GR.Path.Append( m_DocBasePath, subFilename ) );
              return null;
            }
            ExternallyIncludedFiles.Add( GR.Path.Append( m_DocBasePath, subFilename ) );

            StringBuilder   builder = new StringBuilder( subFile.Length * 4 );

            int             maxBytes = subFile.Length;
            if ( fileSizeValid )
            {
              maxBytes = fileSize;
            }
            if ( !fileSkipValid )
            {
              fileSkip = 0;
            }
            if ( fileSkip > maxBytes )
            {
              AddError( lineIndex, "Trying to skip more bytes than the file " + GR.Path.Append( m_DocBasePath, subFilename ) + " holds" );
              return null;
            }

            if ( labelInFront.Length > 0 )
            {
              builder.Append( labelInFront + " " );
            }
            builder.Append( "!byte " );
            for ( int i = fileSkip; i < maxBytes; ++i )
            {
              builder.Append( "$" + subFile[i].ToString( "x" ) );
              if ( i < subFile.Length - 1 )
              {
                builder.Append( "," );
              }
            }
            string resultingLine = builder.ToString();

            Lines[lineIndex] = resultingLine;
            /*
            string[] result = new string[Lines.Length];

            System.Array.Copy( Lines, 0, result, 0, lineIndex );
            result[lineIndex] = resultingLine;
            System.Array.Copy( Lines, lineIndex + 1, result, lineIndex + 1, Lines.Length - lineIndex - 1 );

            Lines = result;
            Debug.Log( "FIXME! Restart after !binary" );
            goto label;*/
          }
          else if ( ( macro == "!CT" )
          ||        ( macro == "!CONVTAB" ) )
          {
            if ( lineTokens.Count != 2 )
            {
              AddError( lineIndex, "Expected !CT <Type = raw or scr>" );
              return null;
            }
            if ( lineTokens[1].ToUpper() == "RAW" )
            {
              m_TextModeIsRaw = true;
            }
            else if ( lineTokens[1].ToUpper() == "SCR" )
            {
              m_TextModeIsRaw = false;
            }
            else
            {
              AddError( lineIndex, "Expected !CT <Type = raw or scr>" );
              return null;
            }
          }
          else if ( macro == "!TO" )
          {
            // !to targetfilename,outputtype
            //     outputtype = cbm (default) oder plain

            List<string> tokens = lineTokens;
            tokens.RemoveAt( 0 );

            if ( ( tokens.Count != 3 )
            || ( tokens[1] != "," ) )
            {
              AddError( lineIndex, "Expected !to <Filename>,<Type = cbm or plain>" );
              return null;
            }
            if ( ( tokens[0].Length < 2 )
            ||   ( !tokens[0].StartsWith( "\"" ) )
            ||   ( !tokens[0].EndsWith( "\"" ) ) )
            {
              AddError( lineIndex, "File name not formatted between apostrophes" );
              return null;
            }
            if ( ( tokens[2].ToUpper() != "CBM" )
            &&   ( tokens[2].ToUpper() != "PLAIN" )
            &&   ( tokens[2].ToUpper() != "CART8BIN" )
            &&   ( tokens[2].ToUpper() != "CART8CRT" )
            &&   ( tokens[2].ToUpper() != "CART16BIN" )
            &&   ( tokens[2].ToUpper() != "CART16CRT" )
            &&   ( tokens[2].ToUpper() != "T64" ) )
            {
              AddError( lineIndex, "Unsupported target type " + tokens[2] + ", only cbm or plain supported" );
              return null;
            }
            tokens[0] = tokens[0].Substring( 1, tokens[0].Length - 2 );
            m_CompileTargetFile = GR.Path.Append( m_DocBasePath, tokens[0] );
            if ( tokens[2].ToUpper() == "CBM" )
            {
              m_CompileTarget = CompileTargetType.PRG;
            }
            else if ( tokens[2].ToUpper() == "PLAIN" )
            {
              m_CompileTarget = CompileTargetType.PLAIN;
            }
            else if ( tokens[2].ToUpper() == "T64" )
            {
              m_CompileTarget = CompileTargetType.T64;
            }
            else if ( tokens[2].ToUpper() == "CART8BIN" )
            {
              m_CompileTarget = CompileTargetType.CARTRIDGE_8K_BIN;
            }
            else if ( tokens[2].ToUpper() == "CART8CRT" )
            {
              m_CompileTarget = CompileTargetType.CARTRIDGE_8K_CRT;
            }
            else if ( tokens[2].ToUpper() == "CART16BIN" )
            {
              m_CompileTarget = CompileTargetType.CARTRIDGE_16K_BIN;
            }
            else if ( tokens[2].ToUpper() == "CART16CRT" )
            {
              m_CompileTarget = CompileTargetType.CARTRIDGE_16K_CRT;
            }
          }
          else if ( macro == "!IFDEF" )
          {
            // !ifdef MUSIC_ON {
            int startBracket = parseLine.IndexOf( "{" );
            if ( startBracket == -1 )
            {
              AddError( lineIndex, "Missing opening brace" );
            }
            else
            {
              string defineCheck = parseLine.Substring( 6, startBracket - 6 ).Trim();

              List<string> tokens = ParseTokens( defineCheck );

              int defineResult = -1;
              if ( ( !EvaluateTokens( tokens, out defineResult ) )
              || ( defineResult == 0 ) )
              {
                stackDefineBlocks.Push( false );
                //dh.Log( "Define out block for " + defineCheck );
              }
              else
              {
                stackDefineBlocks.Push( true );
                //dh.Log( "Define in block for " + defineCheck );
              }
            }
          }
          else if ( macro == "!IFNDEF" )
          {
            int startBracket = parseLine.IndexOf( "{" );
            if ( startBracket == -1 )
            {
              AddError( lineIndex, "Missing opening brace" );
            }
            else
            {
              string defineCheck = parseLine.Substring( 7, startBracket - 7 ).Trim();

              List<string> tokens = ParseTokens( defineCheck );

              int defineResult = -1;
              if ( ( !EvaluateTokens( tokens, out defineResult ) )
              || ( defineResult == 0 ) )
              {
                stackDefineBlocks.Push( true );
              }
              else
              {
                stackDefineBlocks.Push( false );
              }
            }
          }
          else if ( macro == "!IF" )
          {
            // !ifdef MUSIC_ON {
            int startBracket = parseLine.IndexOf( "{" );
            if ( startBracket == -1 )
            {
              AddError( lineIndex, "Missing opening brace" );
            }
            else
            {
              string expressionCheck = parseLine.Substring( 3, startBracket - 3 ).Trim();

              List<string> tokens = ParseTokens( expressionCheck );

              int defineResult = -1;
              if ( !EvaluateTokens( tokens, out defineResult ) )
              {
                //AddError( lineIndex, "Could not evaluate expression: " + expressionCheck );
                stackDefineBlocks.Push( true );
              }
              else if ( defineResult == 0 )
              {
                stackDefineBlocks.Push( false );
              }
              else
              {
                stackDefineBlocks.Push( true );
              }
            }
          }
          if ( ( macro != "!BYTE" )
          &&   ( macro != "!BY" )
          &&   ( macro != "!8" )
          &&   ( macro != "!08" )
          &&   ( macro != "!WORD" )
          &&   ( macro != "!WO" )
          &&   ( macro != "!16" )
          &&   ( macro != "!TEXT" )
          &&   ( macro != "!TX" )
          &&   ( macro != "!SCR" )
          &&   ( macro != "!PSEUDOPC" )
          &&   ( macro != "!REALPC" )
          &&   ( macro != "!CT" )
          &&   ( macro != "!CONVTAB" )
          &&   ( macro != "!BINARY" )
          &&   ( macro != "!BIN" )
          &&   ( macro != "!BI" )
          &&   ( macro != "!SOURCE" )
          &&   ( macro != "!TO" )
          &&   ( macro != "!ZONE" )
          &&   ( macro != "!ZN" )
          &&   ( macro != "!ERROR" )
          &&   ( macro != "!IFDEF" )
          &&   ( macro != "!IFNDEF" )
          &&   ( macro != "!IF" )
          &&   ( macro != "!FI" )
          &&   ( macro != "!FILL" ) )
          {
            AddWarning( lineIndex, "Unsupported macro " + macro + ", this might result in a broken build" );
          }

        }

        ++lineIndex;
      }
      //Debug.Log( "PreProcess done" );
      return Lines;
    }



    private void PopulateAddressToLine()
    {
      foreach ( int lineIndex in m_LineInfo.Keys )
      {
        if ( m_LineInfo[lineIndex].AddressStart != -1 )
        {
          if ( ( AddressToLine.ContainsKey( m_LineInfo[lineIndex].AddressStart ) )
          &&   ( lineIndex > AddressToLine[m_LineInfo[lineIndex].AddressStart] ) )
          {
            AddressToLine[m_LineInfo[lineIndex].AddressStart] = lineIndex;
          }
          else if ( ( AddressToLine.ContainsKey( m_LineInfo[lineIndex].AddressStart ) )
          &&        ( m_LineInfo[lineIndex].NumBytes == 0 ) )
          {
            AddressToLine[m_LineInfo[lineIndex].AddressStart] = lineIndex;
          }
          else if ( !AddressToLine.ContainsKey( m_LineInfo[lineIndex].AddressStart ) )
          {
            AddressToLine.Add( m_LineInfo[lineIndex].AddressStart, lineIndex );
          }
        }
      }
    }



    public void Clear()
    {
      m_CompileTarget     = CompileTargetType.PRG;
      m_CompileTargetFile = null;
      m_CompileCurrentAddress = -1;
      ExternallyIncludedFiles.Clear();

      Assembly = null;
      Messages.Clear();
      m_ErrorMessages = 0;
      m_WarningMessages = 0;
      m_LineInfo.Clear();
      m_UnparsedLabels.Clear();
      m_Labels.Clear();
      AddressToLine.Clear();
      m_SourceInfo.Clear();
      m_LoadedFiles.Clear();
      m_Filename = "";
      m_TextModeIsRaw = true;
    }



    public List<string> KnownTokens()
    {
      List<string>    knownTokens = new List<string>();

      knownTokens.AddRange( m_Labels.Keys );
      knownTokens.AddRange( m_UnparsedLabels.Keys );

      knownTokens.Sort();

      return knownTokens;
    }



    private void ParsePreDefines( ProjectConfig Configuration )
    {
      string[]    makros = Configuration.Defines.Split( '\n' );

      foreach ( string makro in makros )
      {
        string singleMakro = makro.Trim();

        List<string> lineTokens = ParseTokens( singleMakro );

        if ( lineTokens.Count == 1 )
        {
          AddLabel( lineTokens[0], 1, -1 );
        }
        else if ( ( lineTokens.Count >= 3 )
        &&        ( lineTokens[1] == "=" ) )
        {
          // a define
          int equPos = singleMakro.IndexOf( '=' );
          string defineName = singleMakro.Substring( 0, equPos ).Trim();
          string defineValue = singleMakro.Substring( equPos + 1 ).Trim();
          List<string>  valueTokens = ParseTokens( defineValue );
          int address = -1;
          if ( !EvaluateTokens( valueTokens, out address ) )
          {
            AddError( -1, "Cannot evaluate predefine expression " + defineValue );
          }
          else if ( ( address >= 0 )
          &&        ( address <= 0xffff ) )
          {
            AddConstant( defineName, address, -1 );
          }
        }
      }
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



    public bool ParseFile( string Filename, ProjectConfig Configuration )
    {
      Clear();

      m_Filename = Filename;

      m_DocBasePath = GR.Path.RemoveFileSpec( Filename );

      string text = "";

      //Debug.Log( "Start " + Filename );
      try
      {
        text = System.IO.File.ReadAllText( Filename );
      }
      catch ( System.IO.IOException )
      {
        AddError( -1, "Could not open file " + Filename );
        return false;
      }

      //Debug.Log( "Fileread" );

      string[] lines = text.Split( '\n' );

      CleanLines( lines );
      //Debug.Log( "Filesplit" );

      SourceInfo    sourceInfo = new SourceInfo();
      sourceInfo.Filename = Filename;
      sourceInfo.GlobalStartLine = 0;
      sourceInfo.LineCount = lines.Length;

      m_SourceInfo.Add( sourceInfo.GlobalStartLine, sourceInfo );

      //Debug.Log( "PreProcess" );
      lines = PreProcess( lines, Filename, 0, Configuration );
      if ( lines == null )
      {
        return false;
      }

      //System.IO.File.WriteAllLines( "preprocessed.txt", lines );

      //Debug.Log( "2nd pass" );
      SecondPass( lines );
      //Debug.Log( "2nd pass done" );

      DetermineUnparsedLabels();

      //Debug.Log( "DetermineUnparsedLabels done" );

      PopulateAddressToLine();

      //Debug.Log( "PopulateAddressToLine done" );
      DumpSourceInfos();

      //dh.Log( "Constructed " + lines.Length + " lines" );
      foreach ( TokenInfo token in m_Labels.Values )
      {
        if ( ( !token.Used )
        &&   ( token.Name != "*" ) )
        {
          AddWarning( token.LineIndex, "Unused label " + token.Name );
        }
      }

      if ( ( m_UnparsedLabels.Count > 0 )
      ||   ( m_ErrorMessages > 0 ) )
      {
        return false;
      }

      return true;
    }



    public bool Assemble( CompileTargetType CompileType )
    {
      GR.Memory.ByteBuffer result = new GR.Memory.ByteBuffer();

      int     currentAddress = -1;
      bool    startBytesSet = false;
      int     fileStartAddress = -1;
      foreach ( LineInfo line in m_LineInfo.Values )
      {
        if ( currentAddress == -1 )
        {
          if ( line.AddressStart != -1 )
          {
            currentAddress = line.AddressStart;
            if ( !startBytesSet )
            {
              startBytesSet = true;
              fileStartAddress = currentAddress;

              if ( CompileType == CompileTargetType.PRG )
              {
                result.AppendU16( (ushort)currentAddress );
              }
            }
          }
        }
        if ( ( line.AddressStart != -1 )
        &&   ( line.AddressStart != currentAddress ) )
        {
          /*
          Debug.Log( "Address jump from " + currentAddress.ToString( "x" ) + " to " + line.AddressStart.ToString( "x" ) );
          Debug.Log( "-at line " + line.Line );
          if ( line.LineData != null )
          {
            Debug.Log( "-at line index " + line.LineData.ToString() );
          }*/
          if ( line.AddressStart < currentAddress )
          {
            if ( line.PseudoPCOffset == -1 )
            {
              AddWarning( line.LineIndex, "Segment starts inside another one, overwriting it" );
            }
          }
          else if ( line.PseudoPCOffset != -2 )
          {
            // fill gap if not caused by pseudo offset
            result.Append( new GR.Memory.ByteBuffer( (uint)( line.AddressStart - currentAddress ) ) );
          }
          currentAddress = line.AddressStart;
        }
        if ( line.LineData != null )
        {
          result.Append( line.LineData );
          currentAddress += (int)line.LineData.Length;
        }
      }

      /*
      foreach ( string label in m_Labels.Keys )
      {
        dh.Log( "Label " + label + " = " + m_Labels[label].AddressOrValue.ToString( "x" ) );
      }
       */

      Assembly = result;
      if ( CompileType == CompileTargetType.T64 )
      {
        Formats.T64 t64 = new C64Studio.Formats.T64();

        Formats.T64.FileRecord  record = new C64Studio.Formats.T64.FileRecord();

        record.Filename = "HURZ";
        record.StartAddress = (ushort)fileStartAddress;

        t64.TapeInfo.Description = "C64S tape file\r\nDemo tape";
        t64.TapeInfo.UserDescription = "USERDESC";
        t64.FileRecords.Add( record );
        t64.Files.Add( result );

        Assembly = t64.Compile();
      }
      else if ( ( CompileType == CompileTargetType.CARTRIDGE_8K_BIN )
      ||        ( CompileType == CompileTargetType.CARTRIDGE_8K_CRT ) )
      {
        if ( result.Length < 8192 )
        {
          // fill up
          Assembly = result + new GR.Memory.ByteBuffer( 8192 - result.Length );
        }
      }
      else if ( ( CompileType == CompileTargetType.CARTRIDGE_16K_BIN )
      ||        ( CompileType == CompileTargetType.CARTRIDGE_16K_CRT ) )
      {
        if ( result.Length < 16384 )
        {
          // fill up
          result = result + new GR.Memory.ByteBuffer( 16384 - result.Length );
        }
        if ( CompileType == CompileTargetType.CARTRIDGE_16K_CRT )
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

          Assembly = header + chip;
        }
      }

      return true;
    }



    private List<string> ParseTokens( string Source )
    {
      List<string> result = new List<string>();

      if ( String.IsNullOrEmpty( Source ) )
      {
        return result;
      }

      int charPos = 0;

      bool      insideToken = false;
      bool      insideApostrophe = false;
      bool      insideSingleApostrophe = false;
      string    currentToken = "";

      Source += "\n";

      while ( charPos < Source.Length )
      {
        char    curChar = Source[charPos];

        if ( insideToken )
        {
          if ( tokenAllowedChars.IndexOf( curChar ) != -1 )
          {
            currentToken += curChar;
          }
          else
          {
            insideToken = false;
            result.Add( currentToken );
            currentToken = "";
          }
        }
        else if ( insideSingleApostrophe )
        {
          currentToken += curChar;
          if ( curChar == '\'' )
          {
            insideSingleApostrophe = false;
            result.Add( currentToken );
            currentToken = "";
            ++charPos;
            continue;
          }
        }
        else if ( insideApostrophe )
        {
          currentToken += curChar;
          if ( curChar == '"' )
          {
            insideApostrophe = false;
            result.Add( currentToken );
            currentToken = "";
            ++charPos;
            continue;
          }
        }
        if ( ( !insideApostrophe )
        &&   ( !insideSingleApostrophe )
        &&   ( !insideToken ) )
        {
          if ( curChar == '"' )
          {
            insideApostrophe = true;
            currentToken = "\"";
          }
          else if ( curChar == '\'' )
          {
            insideSingleApostrophe = true;
            currentToken = "\'";
          }
          else if ( tokenAllowedExpressionOperators.IndexOf( curChar ) != -1 )
          {
            result.Add( "" + curChar );
          }
          else if ( tokenStartAllowedChars.IndexOf( curChar ) != -1 )
          {
            insideToken = true;
            currentToken = "" + curChar;
          }
        }
        ++charPos;
      }

      // normalize tokens
      bool  tokenNormalized = false;

      do
      {
        tokenNormalized = false;

        for ( int i = 0; i < result.Count - 1 ; ++i )
        {
          if ( ( result[i] == "$" )
          ||   ( result[i] == "%" ) )
          {
            result[i] += result[i + 1];
            result.RemoveAt( i + 1 );
            tokenNormalized = true;
            break;
          }
          if ( ( result[i] == "<" )
          &&   ( result[i + 1] == "<" ) )
          {
            result[i] += result[i + 1];
            result.RemoveAt( i + 1 );
            tokenNormalized = true;
            break;
          }
          if ( ( result[i] == ">" )
          &&   ( result[i + 1] == ">" ) )
          {
            result[i] += result[i + 1];
            result.RemoveAt( i + 1 );
            tokenNormalized = true;
            break;
          }
          if ( ( result[i].StartsWith( "+" ) )
          &&   ( result[i + 1] == "+" ) )
          {
            result[i] += result[i + 1];
            result.RemoveAt( i + 1 );
            tokenNormalized = true;
            break;
          }
          if ( ( result[i].StartsWith( "-" ) )
          &&   ( result[i + 1] == "-" ) )
          {
            result[i] += result[i + 1];
            result.RemoveAt( i + 1 );
            tokenNormalized = true;
            break;
          }
        }
      }
      while ( tokenNormalized );

      return result;
    }



    private List<string> ParseExpressionTokens( string Source )
    {
      List<string> result = new List<string>();

      if ( String.IsNullOrEmpty( Source ) )
      {
        return result;
      }

      int charPos = 0;

      bool      insideToken = false;
      bool      insideApostrophe = false;
      string    currentToken = "";

      Source += "\n";

      while ( charPos < Source.Length )
      {
        char    curChar = Source[charPos];

        if ( insideToken )
        {
          if ( tokenAllowedChars.IndexOf( curChar ) != -1 )
          {
            currentToken += curChar;
          }
          else
          {
            insideToken = false;
            result.Add( currentToken );
            currentToken = "";
          }
        }
        else if ( insideApostrophe )
        {
          currentToken += curChar;
          if ( curChar == '"' )
          {
            insideApostrophe = false;
            result.Add( currentToken );
            currentToken = "";
            ++charPos;
            continue;
          }
        }
        if ( ( !insideApostrophe )
        &&   ( !insideToken ) )
        {
          if ( curChar == '"' )
          {
            insideApostrophe = true;
            currentToken = "\"";
          }
          else if ( tokenAllowedExpressionOperators.IndexOf( curChar ) != -1 )
          {
            result.Add( "" + curChar );
          }
          else if ( tokenStartAllowedChars.IndexOf( curChar ) != -1 )
          {
            insideToken = true;
            currentToken = "" + curChar;
          }
        }
        ++charPos;
      }
      return result;
    }



    private Opcode EstimateOpcode( List<string> LineTokens, List<Opcode> PossibleOpcodes, ref LineInfo info )
    {
      // lineTokens[0] contains the mnemonic
      if ( LineTokens.Count == 0 )
      {
        // can't be, error!
        return null;
      }
      bool endsWithCommaX = false;
      bool endsWithCommaY = false;
      bool oneParamInBrackets = false;
      bool twoParamsInBrackets = false;
      int numBytesFirstParam = 0;
      if ( LineTokens.Count >= 2 )
      {
        if ( ( LineTokens[LineTokens.Count - 2] == "," )
        &&   ( LineTokens[LineTokens.Count - 1].ToUpper() == "X" ) )
        {
          endsWithCommaX = true;
        }
        if ( ( LineTokens[LineTokens.Count - 2] == "," )
        &&   ( LineTokens[LineTokens.Count - 1].ToUpper() == "Y" ) )
        {
          endsWithCommaY = true;
        }
        if ( LineTokens[1] == "(" )
        {
          int tokenPos = 2;
          while ( ( tokenPos < LineTokens.Count )
          &&      ( LineTokens[tokenPos] != ")" ) )
          {
            if ( LineTokens[tokenPos] == "," )
            {
              twoParamsInBrackets = true;
              break;
            }
            ++tokenPos;
          }
          if ( !twoParamsInBrackets )
          {
            oneParamInBrackets = true;
          }
        }
        else
        {
          // an expression or identifier or address
          // TODO - expressions are built from one or several parts!
          int value = -1;
          if ( ParseValue( LineTokens[1], out value ) )
          {
            if ( ( value & 0xff00 ) != 0 )
            {
              numBytesFirstParam = 2;
            }
            else
            {
              numBytesFirstParam = 1;
            }
          }
          else
          {
            // TODO!!!!
          }
        }
      }
      Opcode.AddressingType addressing = Opcode.AddressingType.UNKNOWN;

      if ( LineTokens.Count == 1 )
      {
        addressing = Opcode.AddressingType.IMPLICIT;
      }
      else if ( ( LineTokens.Count >= 2 )
      &&        ( LineTokens[1].StartsWith( "#" ) ) )
      {
        addressing = Opcode.AddressingType.DIRECT;

        if ( LineTokens.Count > 2 )
        {
          List<string> extraTokens = new List<string>();

          if ( LineTokens[1].Length > 1 )
          {
            extraTokens.Add( LineTokens[1].Substring( 1 ) );
          }
          if ( LineTokens.Count > 2 )
          {
            for ( int i = 2; i < LineTokens.Count; ++i )
            {
              extraTokens.Add( LineTokens[i] );
            }
          }
          if ( extraTokens.Count > 0 )
          {
            int     expressionResult = -1;
            if ( EvaluateTokens( extraTokens, out expressionResult ) )
            {
              LineTokens.RemoveRange( 2, LineTokens.Count - 2 );
              if ( LineTokens[1].Length > 1 )
              {
                LineTokens[1] = LineTokens[1].Substring( 0, 1 );
              }
              LineTokens.Add( expressionResult.ToString() );
            }
            else
            {
              info.NeededParsedExpression = extraTokens;
            }
          }
        }
      }
      else if ( ( !oneParamInBrackets )
      &&        ( !twoParamsInBrackets ) )
      {
        if ( endsWithCommaX )
        {
          if ( numBytesFirstParam == 1 )
          {
            addressing = Opcode.AddressingType.ZEROPAGE_X;
          }
          else
          {
            addressing = Opcode.AddressingType.ABSOLUTE_X;
          }
        }
        else if ( endsWithCommaY )
        {
          if ( numBytesFirstParam == 1 )
          {
            addressing = Opcode.AddressingType.ZEROPAGE_Y;
          }
          else
          {
            addressing = Opcode.AddressingType.ABSOLUTE_Y;
          }
        }
        else
        {
          if ( numBytesFirstParam == 1 )
          {
            addressing = Opcode.AddressingType.ZEROPAGE;
          }
          else
          {
            addressing = Opcode.AddressingType.ABSOLUTE;
          }
        }
      }
      else if ( oneParamInBrackets )
      {
        if ( endsWithCommaX )
        {
          // wrong! cannot be (address),x
          return null;
          //addressing = Opcode.AddressingType.INDIRECT_X;
        }
        else if ( endsWithCommaY )
        {
          addressing = Opcode.AddressingType.INDIRECT_Y;
        }
        else
        {
          addressing = Opcode.AddressingType.INDIRECT;
        }
      }
      else if ( twoParamsInBrackets )
      {
        addressing = Opcode.AddressingType.INDIRECT_X;
      }
      else
      {
        addressing = Opcode.AddressingType.IMPLICIT;
      }

      foreach ( Opcode opcode in PossibleOpcodes )
      {
        if ( opcode.Addressing == addressing )
        {
          info.NumBytes = opcode.NumOperands + 1;
          return opcode;
        }
      }
      return null;
    }



    private void ReplaceToken( ref List<string> Tokens, string TokenOld, string TokenNew )
    {
      for ( int i = 0; i < Tokens.Count; ++i )
      {
        if ( Tokens[i] == TokenOld )
        {
          Tokens[i] = TokenNew;
        }
      }
    }



    private void SecondPass( string[] Lines )
    {
      GR.Memory.ByteBuffer      result = new GR.Memory.ByteBuffer();
      int                       programStepPos = -1;
      int                       lineIndex = 0;
      string                    zoneName = "";
      Stack<bool>               stackDefineBlocks = new Stack<bool>();

      Dictionary<string,string> previousMinusLabel = new Dictionary<string,string>();

      foreach ( string line in Lines )
      {
        string parseLine = line;

        if ( !parseLine.StartsWith( "}" ) )
        {
          if ( ( stackDefineBlocks.Count > 0 )
          &&   ( !stackDefineBlocks.Peek() ) )
          {
            // defined away
            ++lineIndex;
            continue;
            //parseLine = ";" + parseLine;
          }
        }
        if ( parseLine.Length == 0 )
        {
          ++lineIndex;
          continue;
        }

        LineInfo info = new LineInfo();
        info.LineIndex = lineIndex;
        info.Zone = zoneName;

        m_LineInfo.Add( lineIndex, info );

        List<string> lineTokens = ParseTokens( parseLine );

        if ( lineTokens.Count > 0 )
        {
          if ( lineTokens[0].StartsWith( "-" ) )
          {
            if ( !previousMinusLabel.ContainsKey( lineTokens[0] ) )
            {
              previousMinusLabel.Add( lineTokens[0], "c64_local_label" + lineTokens[0] + lineIndex.ToString() );
            }
            else
            {
              previousMinusLabel[lineTokens[0]] = "c64_local_label" + lineTokens[0] + lineIndex.ToString();
            }
          }
          if ( ( lineTokens[0].StartsWith( "-" ) )
          ||   ( lineTokens[0].StartsWith( "+" ) ) )
          {
            lineTokens[0] = "c64_local_label" + lineTokens[0] + lineIndex.ToString();
          }
        }

        for ( int i = 0; i < lineTokens.Count; ++i )
        {
          string    token = lineTokens[i];

          if ( token.StartsWith( "." ) )
          {
            lineTokens[i] = zoneName + token;
          }
        }

        if ( lineTokens.Count > 0 )
        {
          string upToken = lineTokens[0].ToUpper();

          if ( ( lineTokens.Count >= 3 )
          &&   ( lineTokens[1] == "=" ) )
          {
            // a define
            int equPos = parseLine.IndexOf( '=' );
            string defineName = parseLine.Substring( 0, equPos ).Trim();
            string defineValue = parseLine.Substring( equPos + 1 ).Trim();

            List<string>  valueTokens = ParseTokens( defineValue );
            int address = -1;
            if ( !EvaluateTokens( valueTokens, out address ) )
            {
              AddUnparsedLabel( defineName, defineValue, lineIndex );
            }
            else if ( ( address >= 0 )
            &&        ( address <= 0xffff ) )
            {
              //AddConstant( defineName, address, lineIndex );
            }
            else
            {
              AddError( lineIndex, "Evaluated constant out of bounds, " + address + " must be >= 0 and <= 65535" );
            }

            if ( defineName == "*" )
            {
              // set program step
              List<string>    tokens = ParseExpressionTokens( defineValue );
              if ( !EvaluateTokens( tokens, out programStepPos ) )
              {
                AddError( lineIndex, "Could not evaluate * position value" );
              }

              info.AddressStart = programStepPos;
              info.Line = parseLine;
              info.AddressSource = "*";
            }
            else
            {
              //info.AddressSource = defineValue;
              info.AddressSource = defineName;
              info.Line = parseLine;
            }
            ++lineIndex;
            continue;
          }
          else if ( ( !m_Opcodes.ContainsKey( upToken ) )
          &&        ( !upToken.StartsWith( "!" ) ) )
          {
            // not a token, not a macro, must be a label in front
            if ( upToken == "}" )
            {
              if ( stackDefineBlocks.Count == 0 )
              {
                AddError( lineIndex, "Missing opening brace" );
              }
              else
              {
                if ( ( lineTokens.Count == 3 )
                &&   ( lineTokens[0] == "}" )
                &&   ( lineTokens[2] == "{" )
                &&   ( lineTokens[1].ToUpper() == "ELSE" ) )
                {
                  stackDefineBlocks.Push( !stackDefineBlocks.Pop() );
                }
                else if ( lineTokens.Count == 1 )
                {
                  stackDefineBlocks.Pop();
                }
                else
                {
                  AddError( lineIndex, "Malformed block close statement, expecting single \"}\" or \"} else {\"" );
                }
              }
              ++lineIndex;
              continue;
            }
            if ( lineTokens[0].StartsWith( "." ) )
            {
              AddUnparsedLabel( zoneName + lineTokens[0], "", lineIndex );
            }
            else
            {
              AddUnparsedLabel( lineTokens[0], "", lineIndex );
            }
            info.NumBytes = 0;
            info.AddressSource = lineTokens[0];
            
            lineTokens.RemoveAt( 0 );
            if ( lineTokens.Count == 0 )
            {
              ++lineIndex;
              continue;
            }
            upToken = lineTokens[0].ToUpper();
          }

          if ( m_Opcodes.ContainsKey( upToken ) )
          {
            List<Opcode> possibleOpcodes = m_Opcodes[upToken];

            if ( possibleOpcodes.Count == 1 )
            {
              // must be that one
              info.Opcode   = possibleOpcodes[0];
              info.NumBytes = info.Opcode.NumOperands + 1;
              info.Line     = parseLine;
              info.LineData = new GR.Memory.ByteBuffer();
              info.LineData.AppendU8( (byte)info.Opcode.ByteValue );

              if ( info.Opcode.NumOperands > 0 )
              {
                info.NeededParsedExpression = lineTokens.GetRange( 1, lineTokens.Count - 1 );
              }
              //dh.Log( "Found Token " + opcode.Mnemonic + ", size " + info.NumBytes.ToString() + " in line " + parseLine );
            }
            else
            {
              // TODO - match possible opcode
              info.Line = parseLine;

              Opcode opcode = EstimateOpcode( lineTokens, possibleOpcodes, ref info );
              if ( opcode != null )
              {
                //dh.Log( "Found Token " + opcode.Mnemonic + ", size " + info.NumBytes.ToString() + " in line " + parseLine );
                info.NumBytes = opcode.NumOperands + 1;
                info.Opcode   = opcode;
              }
              else
              {
                AddError( lineIndex, "Could not determine correct opcode for " + lineTokens[0] );
              }
            }

            if ( info.Opcode != null )
            {
              if ( info.Opcode.NumOperands == 0 )
              {
                info.LineData = new GR.Memory.ByteBuffer();
                info.LineData.AppendU8( (byte)info.Opcode.ByteValue );
                info.NeededParsedExpression = null;
              }
              else if ( info.Opcode.NumOperands == 1 )
              {
                info.LineData = new GR.Memory.ByteBuffer();
                info.LineData.AppendU8( (byte)info.Opcode.ByteValue );
                int byteValue = -1;
                if ( EvaluateTokens( lineTokens.GetRange( 1, lineTokens.Count - 1 ), out byteValue ) )
                {
                  info.LineData.AppendU8( (byte)byteValue );
                  info.NeededParsedExpression = null;
                }
                else
                {
                  info.NeededParsedExpression = lineTokens.GetRange( 1, lineTokens.Count - 1 );
                }
              }
              else if ( info.Opcode.NumOperands == 2 )
              {
                info.LineData = new GR.Memory.ByteBuffer();
                info.LineData.AppendU8( (byte)info.Opcode.ByteValue );
                int byteValue = -1;
                if ( EvaluateTokens( lineTokens.GetRange( 1, lineTokens.Count - 1 ), out byteValue ) )
                {
                  info.LineData.AppendU16( (ushort)byteValue );
                  info.NeededParsedExpression = null;
                }
                else
                {
                  info.NeededParsedExpression = lineTokens.GetRange( 1, lineTokens.Count - 1 );
                }
              }
              if ( info.NeededParsedExpression != null )
              {
                // remove unneeded tokens, depending on opcode
                switch ( info.Opcode.Addressing )
                {
                  case Opcode.AddressingType.ABSOLUTE_X:
                  case Opcode.AddressingType.ZEROPAGE_X:
                    if ( ( info.NeededParsedExpression.Count < 2 )
                    ||   ( info.NeededParsedExpression[info.NeededParsedExpression.Count - 2] != "," )
                    ||   ( info.NeededParsedExpression[info.NeededParsedExpression.Count - 1].ToUpper() != "X" ) )
                    {
                      AddError( lineIndex, "Expected trailing ,x" );
                    }
                    else
                    {
                      info.NeededParsedExpression.RemoveRange( info.NeededParsedExpression.Count - 2, 2 );
                    }
                    break;
                  case Opcode.AddressingType.INDIRECT_X:
                    if ( ( info.NeededParsedExpression.Count < 4 )
                    ||   ( info.NeededParsedExpression[0] != "(" )
                    ||   ( info.NeededParsedExpression[info.NeededParsedExpression.Count - 3] != "," )
                    ||   ( info.NeededParsedExpression[info.NeededParsedExpression.Count - 2].ToUpper() != "X" )
                    ||   ( info.NeededParsedExpression[info.NeededParsedExpression.Count - 1] != ")" ) )
                    {
                      if ( ( info.NeededParsedExpression.Count < 4 )
                      ||   ( info.NeededParsedExpression[0] != "(" )
                      ||   ( info.NeededParsedExpression[info.NeededParsedExpression.Count - 3] != ")" )
                      ||   ( info.NeededParsedExpression[info.NeededParsedExpression.Count - 2] != "," )
                      ||   ( info.NeededParsedExpression[info.NeededParsedExpression.Count - 1].ToUpper() != "X" ) )
                      {
                        AddError( lineIndex, "Expected round brackets and trailing ,x" );
                      }
                      else
                      {
                        info.NeededParsedExpression.RemoveRange( info.NeededParsedExpression.Count - 3, 3 );
                        info.NeededParsedExpression.RemoveAt( 0 );
                      }
                    }
                    else
                    {
                      info.NeededParsedExpression.RemoveRange( info.NeededParsedExpression.Count - 3, 3 );
                      info.NeededParsedExpression.RemoveAt( 0 );
                    }
                    break;
                  case Opcode.AddressingType.ABSOLUTE_Y:
                  case Opcode.AddressingType.ZEROPAGE_Y:
                  case Opcode.AddressingType.INDIRECT_Y:
                    // in case of case Opcode.AddressingType.INDIRECT_Y the brackets are parsed out already!
                    if ( ( info.NeededParsedExpression.Count < 2 )
                    ||   ( info.NeededParsedExpression[info.NeededParsedExpression.Count - 2] != "," )
                    ||   ( info.NeededParsedExpression[info.NeededParsedExpression.Count - 1].ToUpper() != "Y" ) )
                    {
                      AddError( lineIndex, "Expected trailing ,y" );
                    }
                    else
                    {
                      info.NeededParsedExpression.RemoveRange( info.NeededParsedExpression.Count - 2, 2 );
                    }
                    break;
                }
                if ( ( info.NeededParsedExpression.Count == 1 )
                &&   ( info.NeededParsedExpression[0].StartsWith( "-" ) ) )
                {
                  if ( previousMinusLabel.ContainsKey( info.NeededParsedExpression[0] ) )
                  {
                    info.NeededParsedExpression[0] = previousMinusLabel[info.NeededParsedExpression[0]];
                  }
                }
              }
            }
          }
          else if ( upToken.StartsWith( "!" ) )
          {
            // Macro
            if ( upToken == "!ERROR" )
            {
              AddError( lineIndex, parseLine.Substring( 6 ).Trim() );
            }
            else if ( upToken == "!PSEUDOPC" )
            {
              int pseudoStepPos = -1;
              if ( !EvaluateTokens( lineTokens.GetRange( 1, lineTokens.Count - 1 ), out pseudoStepPos ) )
              {
                string    expressionCheck = parseLine.Substring( 9 ).Trim();

                AddError( lineIndex, "Could not evaluate expression " + expressionCheck );
              }
              else
              {
                info.PseudoPCOffset = pseudoStepPos;
              }
            }
            else if ( upToken == "!REALPC" )
            {
              info.PseudoPCOffset = -2;
            }
            else if ( upToken == "!IFDEF" )
            {
              // !ifdef MUSIC_ON {
              int  startBracket = parseLine.IndexOf( "{" );
              if ( startBracket == -1 )
              {
                AddError( lineIndex, "Missing opening brace" );
              }
              else
              {
                string    defineCheck = parseLine.Substring( 6, startBracket - 6 ).Trim();

                List<string>    tokens = ParseTokens( defineCheck );

                int     defineResult = -1;
                if ( ( !EvaluateTokens( tokens, out defineResult ) )
                || ( defineResult == 0 ) )
                {
                  stackDefineBlocks.Push( false );
                  //dh.Log( "Define out block for " + defineCheck );
                }
                else
                {
                  stackDefineBlocks.Push( true );
                  //dh.Log( "Define in block for " + defineCheck );
                }
              }
            }
            else if ( upToken == "!IFNDEF" )
            {
              int startBracket = parseLine.IndexOf( "{" );
              if ( startBracket == -1 )
              {
                AddError( lineIndex, "Missing opening brace" );
              }
              else
              {
                string defineCheck = parseLine.Substring( 7, startBracket - 7 ).Trim();

                List<string> tokens = ParseTokens( defineCheck );

                int defineResult = -1;
                if ( ( !EvaluateTokens( tokens, out defineResult ) )
                ||   ( defineResult == 0 ) )
                {
                  stackDefineBlocks.Push( true );
                }
                else
                {
                  stackDefineBlocks.Push( false );
                }
              }
            }
            else if ( upToken == "!IF" )
            {
              // !ifdef MUSIC_ON {
              int  startBracket = parseLine.IndexOf( "{" );
              if ( startBracket == -1 )
              {
                AddError( lineIndex, "Missing opening brace" );
              }
              else
              {
                string    expressionCheck = parseLine.Substring( 3, startBracket - 3 ).Trim();

                List<string>    tokens = ParseTokens( expressionCheck );

                int     defineResult = -1;
                if ( !EvaluateTokens( tokens, out defineResult ) )
                {
                  AddError( lineIndex, "Could not evaluate expression: " + expressionCheck );
                  stackDefineBlocks.Push( true );
                }
                else if ( defineResult == 0 )
                {
                  stackDefineBlocks.Push( false );
                }
                else
                {
                  stackDefineBlocks.Push( true );
                }
              }
            }
            else if ( ( upToken == "!BYTE" )
            || ( upToken == "!BY" )
            || ( upToken == "!8" )
            || ( upToken == "!08" ) )
            {
              GR.Memory.ByteBuffer    data = new GR.Memory.ByteBuffer();

              int   commaCount = 0;
              bool  parseFailed = false;
              for ( int tokenIndex = 1; tokenIndex < lineTokens.Count; ++tokenIndex )
              {
                string token = lineTokens[tokenIndex];

                if ( ( tokenIndex == 1 )
                &&   ( token == "#" ) )
                {
                  // direct value?
                  if ( ( lineTokens.Count > 2 )
                  &&   ( lineTokens[2] != "#" )
                  &&   ( lineTokens[2] != "." ) )
                  {
                    // not a binary value
                    continue;
                  }
                }

                if ( token == "," )
                {
                  ++commaCount;
                }
                else if ( !parseFailed )
                {
                  int     aByte = -1;

                  if ( ParseValue( token, out aByte ) )
                  {
                    data.AppendU8( (byte)aByte );
                  }
                  else if ( token == "." )
                  {
                    data.AppendU8( 0 );
                  }
                  else if ( token == "#" )
                  {
                    data.AppendU8( 1 );
                  }
                  else
                  {
                    // could not fully parse
                    //dh.Log( "Could not fully parse !byte line: " + parseLine );
                    info.NeededParsedExpression = lineTokens.GetRange( 1, lineTokens.Count - 1 );
                    parseFailed = true;
                  }
                }
              }
              if ( info.NeededParsedExpression == null )
              {
                info.LineData = data;
              }
              info.NumBytes = commaCount + 1;
              info.Line = parseLine;
            }
            else if ( ( upToken == "!WORD" )
            || ( upToken == "!WO" )
            || ( upToken == "!16" ) )
            {
              GR.Memory.ByteBuffer    data = new GR.Memory.ByteBuffer();

              int   commaCount = 0;
              bool  parseFailed = false;
              for ( int tokenIndex = 1; tokenIndex < lineTokens.Count; ++tokenIndex )
              {
                string token = lineTokens[tokenIndex];
                if ( ( tokenIndex == 1 )
                &&   ( token == "#" ) )
                {
                  // direct value?
                  if ( ( lineTokens.Count > 2 )
                  &&   ( lineTokens[2] != "#" )
                  &&   ( lineTokens[2] != "." ) )
                  {
                    // not a binary value
                    continue;
                  }
                }

                if ( token == "," )
                {
                  ++commaCount;
                }
                else if ( !parseFailed )
                {
                  int     aByte = -1;
                  if ( ParseValue( token, out aByte ) )
                  {
                    data.AppendU16( (ushort)aByte );
                  }
                  else
                  {
                    // could not fully parse
                    info.NeededParsedExpression = lineTokens.GetRange( 1, lineTokens.Count - 1 );
                    parseFailed = true;
                  }
                }
              }
              if ( info.NeededParsedExpression == null )
              {
                info.LineData = data;
              }
              info.NumBytes = 2 * ( 1 + commaCount );
              info.Line = parseLine;
            }
            else if ( ( upToken == "!ZONE" )
            ||        ( upToken == "!ZN" ) )
            {
              if ( lineTokens.Count != 2 )
              {
                AddError( lineIndex, "Expected single zone descriptor" );
                return;
              }
              zoneName = lineTokens[1];
            }
            else if ( ( upToken == "!TEXT" )
            ||        ( upToken == "!TX" )
            ||        ( upToken == "!SCR" ) )
            {
              int numBytes = 0;

              bool firstToken = true;
              foreach ( string token in lineTokens )
              {
                if ( firstToken )
                {
                  firstToken = false;
                  continue;
                }
                if ( ( token.StartsWith( "\"" ) )
                &&   ( token.EndsWith( "\"" ) ) )
                {
                  numBytes += token.Length - 2;
                }
                else if ( token != "," )
                {
                  ++numBytes;
                }
              }
              info.NumBytes = numBytes;
              info.Line = parseLine;
              info.NeededParsedExpression = lineTokens.GetRange( 1, lineTokens.Count - 1 );
            }
            else if ( ( upToken == "!FILL" )
            ||        ( upToken == "!FI" ) )
            {
              string fillNumberToken = "";
              bool firstToken = true;
              int tokenIndex = 0;
              foreach ( string token in lineTokens )
              {
                if ( token == "," )
                {
                  int numBytes = -1;
                  List<string> tokens = ParseTokens( fillNumberToken );
                  if ( !EvaluateTokens( tokens, out numBytes ) )
                  {
                    AddError( lineIndex, "Could not determine fill parameter " + fillNumberToken );
                    return;
                  }
                  info.NumBytes = numBytes;
                  info.Line = parseLine;
                  info.NeededParsedExpression = lineTokens.GetRange( 1, lineTokens.Count - 1 );
                  break;
                }
                if ( firstToken )
                {
                  firstToken = false;
                }
                else
                {
                  fillNumberToken += token;
                }
                ++tokenIndex;
              }
              if ( info.NumBytes == 0 )
              {
                AddError( lineIndex, "Macro malformed, expect !FILL <Count>,<Type>" );
                return;
              }
            }
          }
          else if ( lineTokens.Count == 1 )
          {
            // a label
            if ( lineTokens[0].StartsWith( "." ) )
            {
              AddUnparsedLabel( zoneName + lineTokens[0], "", lineIndex );
            }
            else
            {
              AddUnparsedLabel( lineTokens[0], "", lineIndex );
            }

            info.NumBytes = 0;
            info.AddressSource = lineTokens[0];
          }
          else
          {
            AddError( lineIndex, "Syntax error" );
          }
          programStepPos += info.NumBytes;
        }
        ++lineIndex;
      }
    }



    public int FindLineAddress( int LineIndex )
    {
      /*
      foreach ( int line in m_LineInfo.Keys )
      {
        dh.Log( "Line " + line.ToString() + " starts at " + m_LineInfo[line].AddressStart );
      }*/

      if ( !m_LineInfo.ContainsKey( LineIndex ) )
      {
        // ugly lower bounds hack
        while ( LineIndex > 0 )
        {
          --LineIndex;
          if ( m_LineInfo.ContainsKey( LineIndex ) )
          {
            return m_LineInfo[LineIndex].AddressStart;
          }
        }
        return -1;
      }
      return m_LineInfo[LineIndex].AddressStart;
    }



    public bool FindTrueLineSource( int LineIndex, string DebugFile, out string Filename, out int LocalLineIndex )
    {
      Filename        = "";
      LocalLineIndex  = -1;

      //dh.Log( "FindTrueLineSource for " + LineIndex );
      foreach ( SourceInfo sourceInfo in m_SourceInfo.Values )
      {
        if ( ( LineIndex >= sourceInfo.GlobalStartLine )
        &&   ( LineIndex < sourceInfo.GlobalStartLine + sourceInfo.LineCount ) )
        {
          Filename        = sourceInfo.Filename;
          //LocalLineIndex = LineIndex + sourceInfo.LocalStartLine - sourceInfo.GlobalStartLine;
          LocalLineIndex = LineIndex + sourceInfo.LocalStartLine - sourceInfo.GlobalStartLine;
          return true;
        }
      }
      //Debug.Log( "FindTrueLineSource for " + LineIndex + " failed" );
      return false;
    }



    public bool FindGlobalLineIndex( int LineIndex, string Filename, out int GlobalLineIndex )
    {
      GlobalLineIndex = -1;

      //dh.Log( "FindTrueLineSource for " + LineIndex );
      foreach ( SourceInfo sourceInfo in m_SourceInfo.Values )
      {
        if ( ( Filename.ToUpper() == sourceInfo.Filename.ToUpper() )
        &&   ( LineIndex >= sourceInfo.LocalStartLine )
        &&   ( LineIndex < sourceInfo.LocalStartLine + sourceInfo.LineCount ) )
        {
          GlobalLineIndex = LineIndex + sourceInfo.GlobalStartLine - sourceInfo.LocalStartLine;
          return true;
        }
      }
      //Debug.Log( "FindTrueLineSource for " + LineIndex + " failed" );
      return false;
    }



    public bool DocumentAndLineFromAddress( int Address, out string DocumentFile, out int DocumentLine )
    {
      DocumentFile = "";
      DocumentLine = -1;
      if ( !AddressToLine.ContainsKey( Address ) )
      {
        return false;
      }
      int     globalLine = AddressToLine[Address];

      return FindTrueLineSource( globalLine, m_Filename, out DocumentFile, out DocumentLine );
    }



    public bool DocumentAndLineFromGlobalLine( int GlobalLine, out string DocumentFile, out int DocumentLine )
    {
      return FindTrueLineSource( GlobalLine, m_Filename, out DocumentFile, out DocumentLine );
    }



    public int AddressFromToken( string Token )
    {
      if ( !m_Labels.ContainsKey( Token ) )
      {
        return -1;
      }
      return m_Labels[Token].AddressOrValue;
    }



    public TokenInfo TokenInfoFromName( string Token, string Zone )
    {
      if ( !m_Labels.ContainsKey( Token ) )
      {
        if ( m_Labels.ContainsKey( Zone + Token ) )
        {
          return m_Labels[Zone + Token];
        }
        return null;
      }
      return m_Labels[Token];
    }



    public void DumpLabels()
    {
      foreach ( string label in m_Labels.Keys )
      {
        Debug.Log( "Label " + label + " = " + m_Labels[label].AddressOrValue.ToString( "x" ) );
      }
    }

  }
}
