using System;
using System.Collections.Generic;
using System.Text;
using static Tiny64.Opcode;

namespace Tiny64
{
  public class Processor
  {
    public string         Name = "";
    public Dictionary<string, List<Opcode>>           Opcodes = new Dictionary<string, List<Opcode>>();
    public Dictionary<byte, Opcode>                   OpcodeByValue = new Dictionary<byte, Opcode>();

    public byte           Accu = 0;
    public byte           X = 0;
    public byte           Y = 0;
    public byte           Flags = 0;

    public ushort         PC = 0;
    public byte           StackPointer = 0xff;



    public Processor( string Name )
    {
      this.Name = Name;
    }



    public bool FlagNegative 
    {
      get
      {
        return ( Flags & 0x80 ) == 0x80;
      }
      set
      {
        if ( value )
        {
          Flags |= 0x80;
        }
        else
        {
          Flags &= 0x7f;
        }
      }
    }



    public bool FlagOverflow
    {
      get
      {
        return ( Flags & 0x40 ) == 0x40;
      }
      set
      {
        if ( value )
        {
          Flags |= 0x40;
        }
        else
        {
          Flags &= 0xbf;
        }
      }
    }



    public bool FlagDecimal
    {
      get
      {
        return ( Flags & 0x08 ) == 0x08;
      }
      set
      {
        if ( value )
        {
          Flags |= 0x08;
        }
        else
        {
          Flags &= 0xf7;
        }
      }
    }



    public bool FlagIRQ
    {
      get
      {
        return ( Flags & 0x04 ) == 0x04;
      }
      set
      {
        if ( value )
        {
          Flags |= 0x04;
        }
        else
        {
          Flags &= 0xfb;
        }
      }
    }



    public bool FlagZero
    {
      get
      {
        return ( Flags & 0x02 ) == 0x02;
      }
      set
      {
        if ( value )
        {
          Flags |= 0x02;
        }
        else
        {
          Flags &= 0xfd;
        }
      }
    }

    public bool FlagCarry
    {
      get
      {
        return ( Flags & 0x01 ) == 0x01;
      }
      set
      {
        if ( value )
        {
          Flags |= 0x01;
        }
        else
        {
          Flags &= 0xfe;
        }
      }
    }



    internal void Initialize()
    {
      Accu          = 0;
      X             = 0;
      Y             = 0;
      Flags         = 0;
      PC            = 0;
      StackPointer  = 0xff;
    }




    internal void CheckFlagZero()
    {
      FlagZero = ( Accu == 0 );
    }



    internal void CheckFlagNegative()
    {
      FlagNegative = ( ( Accu & 0x80 ) != 0 );
    }



    internal void CheckFlagZeroY()
    {
      FlagZero = ( Y == 0 );
    }



    internal void CheckFlagNegativeY()
    {
      FlagNegative = ( ( Y & 0x80 ) != 0 );
    }



    internal void CheckFlagZeroX()
    {
      FlagZero = ( X == 0 );
    }



    internal void CheckFlagNegativeX()
    {
      FlagNegative = ( ( X & 0x80 ) != 0 );
    }



    public Opcode AddOpcode( string Opcode, int ByteValue, int NumOperands, AddressingType Addressing, int NumCycles )
    {
      return AddOpcode( Opcode, ByteValue, NumOperands, Addressing, NumCycles, 0, 0, 0 );
    }



    public Opcode AddOpcode( string Opcode, int ByteValue, int NumOperands, AddressingType Addressing, int NumCycles, int PageBoundaryCycles )
    {
      return AddOpcode( Opcode, ByteValue, NumOperands, Addressing, NumCycles, PageBoundaryCycles, 0, 0 );
    }



    public Opcode AddOpcode( string Opcode, int ByteValue, int NumOperands, AddressingType Addressing, int NumCycles, int PageBoundaryCycles, int BranchSamePagePenalty, int BranchOtherPagePenalty )
    {
      if ( !Opcodes.ContainsKey( Opcode ) )
      {
        Opcodes.Add( Opcode, new List<Opcode>() );
      }
      Opcode opcode = new Opcode( Opcode, ByteValue, NumOperands, Addressing, NumCycles, PageBoundaryCycles, BranchSamePagePenalty, BranchOtherPagePenalty );
      Opcodes[Opcode].Add( opcode );
      if ( !OpcodeByValue.ContainsKey( (byte)ByteValue ) )
      {
        OpcodeByValue.Add( (byte)ByteValue, opcode );
      }
      return opcode;
    }



    public void AddOpcodeForDisassembly( string Opcode, int ByteValue, int NumOperands, AddressingType Addressing )
    {
      Opcode opcode = new Opcode( Opcode, ByteValue, NumOperands, Addressing );
      OpcodeByValue.Add( (byte)ByteValue, opcode );
    }



    public static Processor Create6502()
    {
      // 6510 without illegal opcodes
      var  sys = new Processor( "6502" );

      sys.AddOpcode( "adc", 0x6d, 2, AddressingType.ABSOLUTE, 4 );           // ADC $hhll
      sys.AddOpcode( "adc", 0x7d, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // ADC $hhll, X
      sys.AddOpcode( "adc", 0x79, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // ADC $hhll, Y
      sys.AddOpcode( "adc", 0x65, 1, AddressingType.ZEROPAGE, 3 );           // ADC $ll
      sys.AddOpcode( "adc", 0x75, 1, AddressingType.ZEROPAGE_X, 4 );         // ADC $ll, X
      sys.AddOpcode( "adc", 0x71, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // ADC ($ll), Y
      sys.AddOpcode( "adc", 0x61, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // ADC ($ll,X)
      sys.AddOpcode( "adc", 0x69, 1, AddressingType.IMMEDIATE, 2 );          // ADC #$nn
      sys.AddOpcode( "and", 0x2d, 2, AddressingType.ABSOLUTE, 4 );           // AND $hhll
      sys.AddOpcode( "and", 0x3d, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // AND $hhll, X
      sys.AddOpcode( "and", 0x39, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // AND $hhll, Y
      sys.AddOpcode( "and", 0x25, 1, AddressingType.ZEROPAGE, 3 );           // AND $ll
      sys.AddOpcode( "and", 0x35, 1, AddressingType.ZEROPAGE_X, 4 );         // AND $ll, X
      sys.AddOpcode( "and", 0x31, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // AND ($ll), Y
      sys.AddOpcode( "and", 0x21, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // AND ($ll,X)
      sys.AddOpcode( "and", 0x29, 1, AddressingType.IMMEDIATE, 2 );          // AND #$nn
      sys.AddOpcode( "asl", 0x0a, 0, AddressingType.IMPLICIT, 2 );           // ASL
      sys.AddOpcode( "asl", 0x0e, 2, AddressingType.ABSOLUTE, 6 );           // ASL $hhll
      sys.AddOpcode( "asl", 0x1e, 2, AddressingType.ABSOLUTE_X, 7 );         // ASL $hhll, X
      sys.AddOpcode( "asl", 0x06, 1, AddressingType.ZEROPAGE, 5 );           // ASL $ll
      sys.AddOpcode( "asl", 0x16, 1, AddressingType.ZEROPAGE_X, 6 );         // ASL $ll, X
      sys.AddOpcode( "bcc", 0x90, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BCC $hhll
      sys.AddOpcode( "bcs", 0xB0, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BCS $hhll
      sys.AddOpcode( "beq", 0xF0, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BEQ $hhll
      sys.AddOpcode( "bit", 0x2c, 2, AddressingType.ABSOLUTE, 4 );           // BIT $hhll
      sys.AddOpcode( "bit", 0x24, 1, AddressingType.ZEROPAGE, 3 );           // BIT $ll
      sys.AddOpcode( "bmi", 0x30, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BMI $hhll
      sys.AddOpcode( "bne", 0xD0, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BNE $hhll
      sys.AddOpcode( "bpl", 0x10, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BPL $hhll
      sys.AddOpcode( "brk", 0x00, 0, AddressingType.IMPLICIT, 7 );           // BRK
      sys.AddOpcode( "bvc", 0x50, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BVC $hhll
      sys.AddOpcode( "bvs", 0x70, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BVS $hhll
      sys.AddOpcode( "clc", 0x18, 0, AddressingType.IMPLICIT, 2 );           // CLC
      sys.AddOpcode( "cld", 0xD8, 0, AddressingType.IMPLICIT, 2 );           // CLD
      sys.AddOpcode( "cli", 0x58, 0, AddressingType.IMPLICIT, 2 );           // CLI
      sys.AddOpcode( "clv", 0xB8, 0, AddressingType.IMPLICIT, 2 );           // CLV
      sys.AddOpcode( "cmp", 0xCD, 2, AddressingType.ABSOLUTE, 4 );           // CMP $hhll
      sys.AddOpcode( "cmp", 0xDD, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // CMP $hhll, X
      sys.AddOpcode( "cmp", 0xD9, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // CMP $hhll, Y
      sys.AddOpcode( "cmp", 0xC5, 1, AddressingType.ZEROPAGE, 3 );           // CMP $ll
      sys.AddOpcode( "cmp", 0xD5, 1, AddressingType.ZEROPAGE_X, 4 );         // CMP $ll, X
      sys.AddOpcode( "cmp", 0xD1, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // CMP ($ll), Y
      sys.AddOpcode( "cmp", 0xC1, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // CMP ($ll,X)
      sys.AddOpcode( "cmp", 0xC9, 1, AddressingType.IMMEDIATE, 2 );          // CMP #$nn
      sys.AddOpcode( "cpx", 0xEC, 2, AddressingType.ABSOLUTE, 4 );           // CPX $hhll
      sys.AddOpcode( "cpx", 0xE4, 1, AddressingType.ZEROPAGE, 3 );           // CPX $ll
      sys.AddOpcode( "cpx", 0xE0, 1, AddressingType.IMMEDIATE, 2 );          // CPX #$nn
      sys.AddOpcode( "cpy", 0xCC, 2, AddressingType.ABSOLUTE, 4 );           // CPY $hhll
      sys.AddOpcode( "cpy", 0xC4, 1, AddressingType.ZEROPAGE, 3 );           // CPY $ll
      sys.AddOpcode( "cpy", 0xC0, 1, AddressingType.IMMEDIATE, 2 );          // CPY #$nn
      sys.AddOpcode( "dec", 0xCE, 2, AddressingType.ABSOLUTE, 6 );           // DEC $hhll
      sys.AddOpcode( "dec", 0xDE, 2, AddressingType.ABSOLUTE_X, 7 );         // DEC $hhll, X
      sys.AddOpcode( "dec", 0xC6, 1, AddressingType.ZEROPAGE, 5 );           // DEC $ll
      sys.AddOpcode( "dec", 0xD6, 1, AddressingType.ZEROPAGE_X, 6 );         // DEC $ll, X
      sys.AddOpcode( "dex", 0xCA, 0, AddressingType.IMPLICIT, 2 );           // DEX
      sys.AddOpcode( "dey", 0x88, 0, AddressingType.IMPLICIT, 2 );           // DEY
      sys.AddOpcode( "eor", 0x4D, 2, AddressingType.ABSOLUTE, 4 );           // EOR $hhll
      sys.AddOpcode( "eor", 0x5D, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // EOR $hhll, X
      sys.AddOpcode( "eor", 0x59, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // EOR $hhll, Y
      sys.AddOpcode( "eor", 0x45, 1, AddressingType.ZEROPAGE, 3 );           // EOR $ll
      sys.AddOpcode( "eor", 0x55, 1, AddressingType.ZEROPAGE_X, 4 );         // EOR $ll, X
      sys.AddOpcode( "eor", 0x51, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // EOR ($ll), Y
      sys.AddOpcode( "eor", 0x41, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // EOR ($ll,X)
      sys.AddOpcode( "eor", 0x49, 1, AddressingType.IMMEDIATE, 2 );          // EOR #$nn
      sys.AddOpcode( "inc", 0xEE, 2, AddressingType.ABSOLUTE, 6 );           // INC $hhll
      sys.AddOpcode( "inc", 0xFE, 2, AddressingType.ABSOLUTE_X, 7 );         // INC $hhll, X
      sys.AddOpcode( "inc", 0xE6, 1, AddressingType.ZEROPAGE, 5 );           // INC $ll
      sys.AddOpcode( "inc", 0xF6, 1, AddressingType.ZEROPAGE_X, 6 );         // INC $ll, X
      sys.AddOpcode( "inx", 0xE8, 0, AddressingType.IMPLICIT, 2 );           // INX
      sys.AddOpcode( "iny", 0xC8, 0, AddressingType.IMPLICIT, 2 );           // INY
      sys.AddOpcode( "jmp", 0x4C, 2, AddressingType.ABSOLUTE, 3 );           // JMP $hhll
      sys.AddOpcode( "jmp", 0x6C, 2, AddressingType.INDIRECT, 5 );           // JMP ($hhll)
      sys.AddOpcode( "jsr", 0x20, 2, AddressingType.ABSOLUTE, 6 );           // JSR $hhll
      sys.AddOpcode( "lda", 0xAD, 2, AddressingType.ABSOLUTE, 4 );           // LDA $hhll
      sys.AddOpcode( "lda", 0xBD, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // LDA $hhll, X
      sys.AddOpcode( "lda", 0xB9, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // LDA $hhll, Y
      sys.AddOpcode( "lda", 0xA5, 1, AddressingType.ZEROPAGE, 3 );           // LDA $ll
      sys.AddOpcode( "lda", 0xB5, 1, AddressingType.ZEROPAGE_X, 4 );         // LDA $ll, X
      sys.AddOpcode( "lda", 0xB1, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // LDA ($ll), Y
      sys.AddOpcode( "lda", 0xA1, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // LDA ($ll,X)
      sys.AddOpcode( "lda", 0xA9, 1, AddressingType.IMMEDIATE, 2 );          // LDA #$nn
      sys.AddOpcode( "ldx", 0xAE, 2, AddressingType.ABSOLUTE, 4 );           // LDX $hhll
      sys.AddOpcode( "ldx", 0xBE, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // LDX $hhll, Y
      sys.AddOpcode( "ldx", 0xA6, 1, AddressingType.ZEROPAGE, 3 );           // LDX $ll
      sys.AddOpcode( "ldx", 0xB6, 1, AddressingType.ZEROPAGE_Y, 4 );         // LDX $ll, Y
      sys.AddOpcode( "ldx", 0xA2, 1, AddressingType.IMMEDIATE, 2 );          // LDX #$nn
      sys.AddOpcode( "ldy", 0xAC, 2, AddressingType.ABSOLUTE, 4 );           // LDY $hhll
      sys.AddOpcode( "ldy", 0xBC, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // LDY $hhll, X
      sys.AddOpcode( "ldy", 0xA4, 1, AddressingType.ZEROPAGE, 3 );           // LDY $ll
      sys.AddOpcode( "ldy", 0xB4, 1, AddressingType.ZEROPAGE_X, 4 );         // LDY $ll, X
      sys.AddOpcode( "ldy", 0xA0, 1, AddressingType.IMMEDIATE, 2 );          // LDY #$nn
      sys.AddOpcode( "lsr", 0x4A, 0, AddressingType.IMPLICIT, 2 );           // LSR
      sys.AddOpcode( "lsr", 0x4E, 2, AddressingType.ABSOLUTE, 6 );           // LSR $hhll
      sys.AddOpcode( "lsr", 0x5E, 2, AddressingType.ABSOLUTE_X, 7 );         // LSR $hhll, X
      sys.AddOpcode( "lsr", 0x46, 1, AddressingType.ZEROPAGE, 5 );           // LSR $ll
      sys.AddOpcode( "lsr", 0x56, 1, AddressingType.ZEROPAGE_X, 6 );         // LSR $ll, X
      sys.AddOpcode( "nop", 0xEA, 0, AddressingType.IMPLICIT, 2 );           // NOP
      sys.AddOpcode( "ora", 0x0D, 2, AddressingType.ABSOLUTE, 4 );           // ORA $hhll
      sys.AddOpcode( "ora", 0x1D, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // ORA $hhll, X
      sys.AddOpcode( "ora", 0x19, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // ORA $hhll, Y
      sys.AddOpcode( "ora", 0x05, 1, AddressingType.ZEROPAGE, 3 );           // ORA $ll
      sys.AddOpcode( "ora", 0x15, 1, AddressingType.ZEROPAGE_X, 4 );         // ORA $ll, X
      sys.AddOpcode( "ora", 0x11, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // ORA ($ll), Y
      sys.AddOpcode( "ora", 0x01, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // ORA ($ll,X)
      sys.AddOpcode( "ora", 0x09, 1, AddressingType.IMMEDIATE, 2 );          // ORA #$nn
      sys.AddOpcode( "pha", 0x48, 0, AddressingType.IMPLICIT, 3 );           // PHA
      sys.AddOpcode( "php", 0x08, 0, AddressingType.IMPLICIT, 3 );           // PHP
      sys.AddOpcode( "pla", 0x68, 0, AddressingType.IMPLICIT, 4 );           // PLA
      sys.AddOpcode( "plp", 0x28, 0, AddressingType.IMPLICIT, 4 );           // PLP
      sys.AddOpcode( "rol", 0x2A, 0, AddressingType.IMPLICIT, 2 );           // ROL
      sys.AddOpcode( "rol", 0x2E, 2, AddressingType.ABSOLUTE, 6 );           // ROL $hhll
      sys.AddOpcode( "rol", 0x3E, 2, AddressingType.ABSOLUTE_X, 7 );         // ROL $hhll, X
      sys.AddOpcode( "rol", 0x26, 1, AddressingType.ZEROPAGE, 5 );           // ROL $ll
      sys.AddOpcode( "rol", 0x36, 1, AddressingType.ZEROPAGE_X, 6 );         // ROL $ll, X
      sys.AddOpcode( "ror", 0x6A, 0, AddressingType.IMPLICIT, 2 );           // ROR
      sys.AddOpcode( "ror", 0x6E, 2, AddressingType.ABSOLUTE, 6 );           // ROR $hhll
      sys.AddOpcode( "ror", 0x7E, 2, AddressingType.ABSOLUTE_X, 7 );         // ROR $hhll, X
      sys.AddOpcode( "ror", 0x66, 1, AddressingType.ZEROPAGE, 5 );           // ROR $ll
      sys.AddOpcode( "ror", 0x76, 1, AddressingType.ZEROPAGE_X, 6 );         // ROR $ll, X
      sys.AddOpcode( "rti", 0x40, 0, AddressingType.IMPLICIT, 6 );           // RTI
      sys.AddOpcode( "rts", 0x60, 0, AddressingType.IMPLICIT, 6 );           // RTS
      sys.AddOpcode( "sbc", 0xED, 2, AddressingType.ABSOLUTE, 4 );           // SBC $hhll
      sys.AddOpcode( "sbc", 0xFD, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // SBC $hhll, X
      sys.AddOpcode( "sbc", 0xF9, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // SBC $hhll, Y
      sys.AddOpcode( "sbc", 0xE5, 1, AddressingType.ZEROPAGE, 3 );           // SBC $ll
      sys.AddOpcode( "sbc", 0xF5, 1, AddressingType.ZEROPAGE_X, 4 );         // SBC $ll, X
      sys.AddOpcode( "sbc", 0xF1, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // SBC ($ll), Y
      sys.AddOpcode( "sbc", 0xE1, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // SBC ($ll,X)
      sys.AddOpcode( "sbc", 0xE9, 1, AddressingType.IMMEDIATE, 2 );          // SBC #$nn
      sys.AddOpcode( "sec", 0x38, 0, AddressingType.IMPLICIT, 2 );           // SEC
      sys.AddOpcode( "sed", 0xF8, 0, AddressingType.IMPLICIT, 2 );           // SED
      sys.AddOpcode( "sei", 0x78, 0, AddressingType.IMPLICIT, 2 );           // SEI
      sys.AddOpcode( "sta", 0x8D, 2, AddressingType.ABSOLUTE, 4 );           // STA $hhll
      sys.AddOpcode( "sta", 0x9D, 2, AddressingType.ABSOLUTE_X, 5 );         // STA $hhll, X
      sys.AddOpcode( "sta", 0x99, 2, AddressingType.ABSOLUTE_Y, 5 );         // STA $hhll, Y
      sys.AddOpcode( "sta", 0x85, 1, AddressingType.ZEROPAGE, 3 );           // STA $ll
      sys.AddOpcode( "sta", 0x95, 1, AddressingType.ZEROPAGE_X, 4 );         // STA $ll, X
      sys.AddOpcode( "sta", 0x91, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 6 );         // STA ($ll), Y
      sys.AddOpcode( "sta", 0x81, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // STA ($ll,X)
      sys.AddOpcode( "stx", 0x8E, 2, AddressingType.ABSOLUTE, 4 );           // STX $hhll
      sys.AddOpcode( "stx", 0x86, 1, AddressingType.ZEROPAGE, 3 );           // STX $ll
      sys.AddOpcode( "stx", 0x96, 1, AddressingType.ZEROPAGE_Y, 4 );         // STX $ll, Y
      sys.AddOpcode( "sty", 0x8C, 2, AddressingType.ABSOLUTE, 4 );           // STY $hhll
      sys.AddOpcode( "sty", 0x84, 1, AddressingType.ZEROPAGE, 3 );           // STY $ll
      sys.AddOpcode( "sty", 0x94, 1, AddressingType.ZEROPAGE_X, 4 );         // STY $ll, X
      sys.AddOpcode( "tax", 0xAA, 0, AddressingType.IMPLICIT, 2 );           // TAX
      sys.AddOpcode( "tay", 0xA8, 0, AddressingType.IMPLICIT, 2 );           // TAY
      sys.AddOpcode( "tsx", 0xBA, 0, AddressingType.IMPLICIT, 2 );           // TSX
      sys.AddOpcode( "txa", 0x8A, 0, AddressingType.IMPLICIT, 2 );           // TXA
      sys.AddOpcode( "txs", 0x9A, 0, AddressingType.IMPLICIT, 2 );           // TXS
      sys.AddOpcode( "tya", 0x98, 0, AddressingType.IMPLICIT, 2 );           // TYA

      sys.AddOpcode( "jam", 0x02, 0, AddressingType.IMPLICIT, 0 );           // JAM
      sys.AddOpcodeForDisassembly( "jam", 0x12, 0, AddressingType.IMPLICIT );      // JAM
      sys.AddOpcodeForDisassembly( "jam", 0x22, 0, AddressingType.IMPLICIT );      // JAM
      sys.AddOpcodeForDisassembly( "jam", 0x32, 0, AddressingType.IMPLICIT );      // JAM
      sys.AddOpcodeForDisassembly( "jam", 0x42, 0, AddressingType.IMPLICIT );      // JAM
      sys.AddOpcodeForDisassembly( "jam", 0x52, 0, AddressingType.IMPLICIT );      // JAM
      sys.AddOpcodeForDisassembly( "jam", 0x62, 0, AddressingType.IMPLICIT );      // JAM
      sys.AddOpcodeForDisassembly( "jam", 0x72, 0, AddressingType.IMPLICIT );      // JAM
      sys.AddOpcodeForDisassembly( "jam", 0x92, 0, AddressingType.IMPLICIT );      // JAM
      sys.AddOpcodeForDisassembly( "jam", 0xb2, 0, AddressingType.IMPLICIT );      // JAM
      sys.AddOpcodeForDisassembly( "jam", 0xd2, 0, AddressingType.IMPLICIT );      // JAM
      sys.AddOpcodeForDisassembly( "jam", 0xf2, 0, AddressingType.IMPLICIT );      // JAM

      //sys.AddOpcodeForDisassembly( "nop", 0x04, 1, AddressingType.ZEROPAGE );      // NOP $zp
      sys.AddOpcode( "nop", 0x04, 1, AddressingType.ZEROPAGE, 3 );      // NOP $zp
      //sys.AddOpcodeForDisassembly( "nop", 0x0C, 2, AddressingType.ABSOLUTE );      // NOP $abcd
      sys.AddOpcode( "nop", 0x0C, 2, AddressingType.ABSOLUTE, 4 );      // NOP $abcd
      //sys.AddOpcodeForDisassembly( "nop", 0x14, 1, AddressingType.ZEROPAGE_X );    // NOP $zp,x
      sys.AddOpcode( "nop", 0x14, 1, AddressingType.ZEROPAGE_X, 4 );    // NOP $zp,x
      sys.AddOpcodeForDisassembly( "nop", 0x1A, 0, AddressingType.IMPLICIT );      // NOP
      //sys.AddOpcodeForDisassembly( "nop", 0x1C, 2, AddressingType.ABSOLUTE_X );    // NOP $abcd,x
      sys.AddOpcode( "nop", 0x1C, 2, AddressingType.ABSOLUTE_X, 4, 1 );    // NOP $abcd,x
      sys.AddOpcodeForDisassembly( "nop", 0x34, 1, AddressingType.ZEROPAGE_X );    // NOP $zp,x
      sys.AddOpcodeForDisassembly( "nop", 0x3A, 0, AddressingType.IMPLICIT );      // NOP
      sys.AddOpcodeForDisassembly( "nop", 0x3C, 2, AddressingType.ABSOLUTE_X );    // NOP $abcd,x
      sys.AddOpcodeForDisassembly( "nop", 0x44, 1, AddressingType.ZEROPAGE );      // NOP $zp
      sys.AddOpcodeForDisassembly( "nop", 0x54, 1, AddressingType.ZEROPAGE_X );    // NOP $zp,x
      sys.AddOpcodeForDisassembly( "nop", 0x5A, 0, AddressingType.IMPLICIT );      // NOP
      sys.AddOpcodeForDisassembly( "nop", 0x5C, 2, AddressingType.ABSOLUTE_X );    // NOP $abcd,x
      sys.AddOpcodeForDisassembly( "nop", 0x64, 1, AddressingType.ZEROPAGE );      // NOP $zp
      sys.AddOpcodeForDisassembly( "nop", 0x74, 1, AddressingType.ZEROPAGE_X );    // NOP $zp,x
      sys.AddOpcodeForDisassembly( "nop", 0x7A, 0, AddressingType.IMPLICIT );      // NOP
      sys.AddOpcodeForDisassembly( "nop", 0x7C, 2, AddressingType.ABSOLUTE_X );    // NOP $abcd,x
      //sys.AddOpcodeForDisassembly( "nop", 0x80, 1, AddressingType.IMMEDIATE );        // NOP #xx
      sys.AddOpcode( "nop", 0x80, 1, AddressingType.IMMEDIATE, 2 );        // NOP #xx
      sys.AddOpcodeForDisassembly( "nop", 0x82, 1, AddressingType.IMMEDIATE );        // NOP #xx
      sys.AddOpcodeForDisassembly( "nop", 0x89, 1, AddressingType.IMMEDIATE );        // NOP #xx
      sys.AddOpcodeForDisassembly( "nop", 0xC2, 1, AddressingType.IMMEDIATE );        // NOP #xx
      sys.AddOpcodeForDisassembly( "nop", 0xD4, 1, AddressingType.ZEROPAGE_X );    // NOP $zp,x
      sys.AddOpcodeForDisassembly( "nop", 0xDA, 0, AddressingType.IMPLICIT );      // NOP
      sys.AddOpcodeForDisassembly( "nop", 0xDC, 2, AddressingType.ABSOLUTE_X );    // NOP $abcd,x
      sys.AddOpcodeForDisassembly( "nop", 0xE2, 1, AddressingType.IMMEDIATE );        // NOP #xx
      sys.AddOpcodeForDisassembly( "nop", 0xF4, 1, AddressingType.ZEROPAGE_X );    // NOP $zp,x
      sys.AddOpcodeForDisassembly( "nop", 0xFA, 0, AddressingType.IMPLICIT );      // NOP
      sys.AddOpcodeForDisassembly( "nop", 0xFC, 2, AddressingType.ABSOLUTE_X );    // NOP $abcd,x

      return sys;
    }



    public static Processor Create6510()
    {
      var  sys = new Processor( "6510" );

      sys.AddOpcode( "adc", 0x6d, 2, AddressingType.ABSOLUTE, 4 );           // ADC $hhll
      sys.AddOpcode( "adc", 0x7d, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // ADC $hhll, X
      sys.AddOpcode( "adc", 0x79, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // ADC $hhll, Y
      sys.AddOpcode( "adc", 0x65, 1, AddressingType.ZEROPAGE, 3 );           // ADC $ll
      sys.AddOpcode( "adc", 0x75, 1, AddressingType.ZEROPAGE_X, 4 );         // ADC $ll, X
      sys.AddOpcode( "adc", 0x71, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // ADC ($ll), Y
      sys.AddOpcode( "adc", 0x61, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // ADC ($ll,X)
      sys.AddOpcode( "adc", 0x69, 1, AddressingType.IMMEDIATE, 2 );          // ADC #$nn
      sys.AddOpcode( "and", 0x2d, 2, AddressingType.ABSOLUTE, 4 );           // AND $hhll
      sys.AddOpcode( "and", 0x3d, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // AND $hhll, X
      sys.AddOpcode( "and", 0x39, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // AND $hhll, Y
      sys.AddOpcode( "and", 0x25, 1, AddressingType.ZEROPAGE, 3 );           // AND $ll
      sys.AddOpcode( "and", 0x35, 1, AddressingType.ZEROPAGE_X, 4 );         // AND $ll, X
      sys.AddOpcode( "and", 0x31, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // AND ($ll), Y
      sys.AddOpcode( "and", 0x21, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // AND ($ll,X)
      sys.AddOpcode( "and", 0x29, 1, AddressingType.IMMEDIATE, 2 );          // AND #$nn
      sys.AddOpcode( "asl", 0x0a, 0, AddressingType.IMPLICIT, 2 );           // ASL
      sys.AddOpcode( "asl", 0x0e, 2, AddressingType.ABSOLUTE, 6 );           // ASL $hhll
      sys.AddOpcode( "asl", 0x1e, 2, AddressingType.ABSOLUTE_X, 7 );         // ASL $hhll, X
      sys.AddOpcode( "asl", 0x06, 1, AddressingType.ZEROPAGE, 5 );           // ASL $ll
      sys.AddOpcode( "asl", 0x16, 1, AddressingType.ZEROPAGE_X, 6 );         // ASL $ll, X
      sys.AddOpcode( "bcc", 0x90, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BCC $hhll
      sys.AddOpcode( "bcs", 0xB0, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BCS $hhll
      sys.AddOpcode( "beq", 0xF0, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BEQ $hhll
      sys.AddOpcode( "bit", 0x2c, 2, AddressingType.ABSOLUTE, 4 );           // BIT $hhll
      sys.AddOpcode( "bit", 0x24, 1, AddressingType.ZEROPAGE, 3 );           // BIT $ll
      sys.AddOpcode( "bmi", 0x30, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BMI $hhll
      sys.AddOpcode( "bne", 0xD0, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BNE $hhll
      sys.AddOpcode( "bpl", 0x10, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BPL $hhll
      sys.AddOpcode( "brk", 0x00, 0, AddressingType.IMPLICIT, 7 );           // BRK
      sys.AddOpcode( "bvc", 0x50, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BVC $hhll
      sys.AddOpcode( "bvs", 0x70, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BVS $hhll
      sys.AddOpcode( "clc", 0x18, 0, AddressingType.IMPLICIT, 2 );           // CLC
      sys.AddOpcode( "cld", 0xD8, 0, AddressingType.IMPLICIT, 2 );           // CLD
      sys.AddOpcode( "cli", 0x58, 0, AddressingType.IMPLICIT, 2 );           // CLI
      sys.AddOpcode( "clv", 0xB8, 0, AddressingType.IMPLICIT, 2 );           // CLV
      sys.AddOpcode( "cmp", 0xCD, 2, AddressingType.ABSOLUTE, 4 );           // CMP $hhll
      sys.AddOpcode( "cmp", 0xDD, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // CMP $hhll, X
      sys.AddOpcode( "cmp", 0xD9, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // CMP $hhll, Y
      sys.AddOpcode( "cmp", 0xC5, 1, AddressingType.ZEROPAGE, 3 );           // CMP $ll
      sys.AddOpcode( "cmp", 0xD5, 1, AddressingType.ZEROPAGE_X, 4 );         // CMP $ll, X
      sys.AddOpcode( "cmp", 0xD1, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // CMP ($ll), Y
      sys.AddOpcode( "cmp", 0xC1, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // CMP ($ll,X)
      sys.AddOpcode( "cmp", 0xC9, 1, AddressingType.IMMEDIATE, 2 );          // CMP #$nn
      sys.AddOpcode( "cpx", 0xEC, 2, AddressingType.ABSOLUTE, 4 );           // CPX $hhll
      sys.AddOpcode( "cpx", 0xE4, 1, AddressingType.ZEROPAGE, 3 );           // CPX $ll
      sys.AddOpcode( "cpx", 0xE0, 1, AddressingType.IMMEDIATE, 2 );          // CPX #$nn
      sys.AddOpcode( "cpy", 0xCC, 2, AddressingType.ABSOLUTE, 4 );           // CPY $hhll
      sys.AddOpcode( "cpy", 0xC4, 1, AddressingType.ZEROPAGE, 3 );           // CPY $ll
      sys.AddOpcode( "cpy", 0xC0, 1, AddressingType.IMMEDIATE, 2 );          // CPY #$nn
      sys.AddOpcode( "dec", 0xCE, 2, AddressingType.ABSOLUTE, 6 );           // DEC $hhll
      sys.AddOpcode( "dec", 0xDE, 2, AddressingType.ABSOLUTE_X, 7 );         // DEC $hhll, X
      sys.AddOpcode( "dec", 0xC6, 1, AddressingType.ZEROPAGE, 5 );           // DEC $ll
      sys.AddOpcode( "dec", 0xD6, 1, AddressingType.ZEROPAGE_X, 6 );         // DEC $ll, X
      sys.AddOpcode( "dex", 0xCA, 0, AddressingType.IMPLICIT, 2 );           // DEX
      sys.AddOpcode( "dey", 0x88, 0, AddressingType.IMPLICIT, 2 );           // DEY
      sys.AddOpcode( "eor", 0x4D, 2, AddressingType.ABSOLUTE, 4 );           // EOR $hhll
      sys.AddOpcode( "eor", 0x5D, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // EOR $hhll, X
      sys.AddOpcode( "eor", 0x59, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // EOR $hhll, Y
      sys.AddOpcode( "eor", 0x45, 1, AddressingType.ZEROPAGE, 3 );           // EOR $ll
      sys.AddOpcode( "eor", 0x55, 1, AddressingType.ZEROPAGE_X, 4 );         // EOR $ll, X
      sys.AddOpcode( "eor", 0x51, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // EOR ($ll), Y
      sys.AddOpcode( "eor", 0x41, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // EOR ($ll,X)
      sys.AddOpcode( "eor", 0x49, 1, AddressingType.IMMEDIATE, 2 );          // EOR #$nn
      sys.AddOpcode( "inc", 0xEE, 2, AddressingType.ABSOLUTE, 6 );           // INC $hhll
      sys.AddOpcode( "inc", 0xFE, 2, AddressingType.ABSOLUTE_X, 7 );         // INC $hhll, X
      sys.AddOpcode( "inc", 0xE6, 1, AddressingType.ZEROPAGE, 5 );           // INC $ll
      sys.AddOpcode( "inc", 0xF6, 1, AddressingType.ZEROPAGE_X, 6 );         // INC $ll, X
      sys.AddOpcode( "inx", 0xE8, 0, AddressingType.IMPLICIT, 2 );           // INX
      sys.AddOpcode( "iny", 0xC8, 0, AddressingType.IMPLICIT, 2 );           // INY
      sys.AddOpcode( "jmp", 0x4C, 2, AddressingType.ABSOLUTE, 3 );           // JMP $hhll
      sys.AddOpcode( "jmp", 0x6C, 2, AddressingType.INDIRECT, 5 );           // JMP ($hhll)
      sys.AddOpcode( "jsr", 0x20, 2, AddressingType.ABSOLUTE, 6 );           // JSR $hhll
      sys.AddOpcode( "lda", 0xAD, 2, AddressingType.ABSOLUTE, 4 );           // LDA $hhll
      sys.AddOpcode( "lda", 0xBD, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // LDA $hhll, X
      sys.AddOpcode( "lda", 0xB9, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // LDA $hhll, Y
      sys.AddOpcode( "lda", 0xA5, 1, AddressingType.ZEROPAGE, 3 );           // LDA $ll
      sys.AddOpcode( "lda", 0xB5, 1, AddressingType.ZEROPAGE_X, 4 );         // LDA $ll, X
      sys.AddOpcode( "lda", 0xB1, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // LDA ($ll), Y
      sys.AddOpcode( "lda", 0xA1, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // LDA ($ll,X)
      sys.AddOpcode( "lda", 0xA9, 1, AddressingType.IMMEDIATE, 2 );          // LDA #$nn
      sys.AddOpcode( "ldx", 0xAE, 2, AddressingType.ABSOLUTE, 4 );           // LDX $hhll
      sys.AddOpcode( "ldx", 0xBE, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // LDX $hhll, Y
      sys.AddOpcode( "ldx", 0xA6, 1, AddressingType.ZEROPAGE, 3 );           // LDX $ll
      sys.AddOpcode( "ldx", 0xB6, 1, AddressingType.ZEROPAGE_Y, 4 );         // LDX $ll, Y
      sys.AddOpcode( "ldx", 0xA2, 1, AddressingType.IMMEDIATE, 2 );          // LDX #$nn
      sys.AddOpcode( "ldy", 0xAC, 2, AddressingType.ABSOLUTE, 4 );           // LDY $hhll
      sys.AddOpcode( "ldy", 0xBC, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // LDY $hhll, X
      sys.AddOpcode( "ldy", 0xA4, 1, AddressingType.ZEROPAGE, 3 );           // LDY $ll
      sys.AddOpcode( "ldy", 0xB4, 1, AddressingType.ZEROPAGE_X, 4 );         // LDY $ll, X
      sys.AddOpcode( "ldy", 0xA0, 1, AddressingType.IMMEDIATE, 2 );          // LDY #$nn
      sys.AddOpcode( "lsr", 0x4A, 0, AddressingType.IMPLICIT, 2 );           // LSR
      sys.AddOpcode( "lsr", 0x4E, 2, AddressingType.ABSOLUTE, 6 );           // LSR $hhll
      sys.AddOpcode( "lsr", 0x5E, 2, AddressingType.ABSOLUTE_X, 7 );         // LSR $hhll, X
      sys.AddOpcode( "lsr", 0x46, 1, AddressingType.ZEROPAGE, 5 );           // LSR $ll
      sys.AddOpcode( "lsr", 0x56, 1, AddressingType.ZEROPAGE_X, 6 );         // LSR $ll, X
      sys.AddOpcode( "nop", 0xEA, 0, AddressingType.IMPLICIT, 2 );           // NOP
      sys.AddOpcode( "ora", 0x0D, 2, AddressingType.ABSOLUTE, 4 );           // ORA $hhll
      sys.AddOpcode( "ora", 0x1D, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // ORA $hhll, X
      sys.AddOpcode( "ora", 0x19, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // ORA $hhll, Y
      sys.AddOpcode( "ora", 0x05, 1, AddressingType.ZEROPAGE, 3 );           // ORA $ll
      sys.AddOpcode( "ora", 0x15, 1, AddressingType.ZEROPAGE_X, 4 );         // ORA $ll, X
      sys.AddOpcode( "ora", 0x11, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // ORA ($ll), Y
      sys.AddOpcode( "ora", 0x01, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // ORA ($ll,X)
      sys.AddOpcode( "ora", 0x09, 1, AddressingType.IMMEDIATE, 2 );          // ORA #$nn
      sys.AddOpcode( "pha", 0x48, 0, AddressingType.IMPLICIT, 3 );           // PHA
      sys.AddOpcode( "php", 0x08, 0, AddressingType.IMPLICIT, 3 );           // PHP
      sys.AddOpcode( "pla", 0x68, 0, AddressingType.IMPLICIT, 4 );           // PLA
      sys.AddOpcode( "plp", 0x28, 0, AddressingType.IMPLICIT, 4 );           // PLP
      sys.AddOpcode( "rol", 0x2A, 0, AddressingType.IMPLICIT, 2 );           // ROL
      sys.AddOpcode( "rol", 0x2E, 2, AddressingType.ABSOLUTE, 6 );           // ROL $hhll
      sys.AddOpcode( "rol", 0x3E, 2, AddressingType.ABSOLUTE_X, 7 );         // ROL $hhll, X
      sys.AddOpcode( "rol", 0x26, 1, AddressingType.ZEROPAGE, 5 );           // ROL $ll
      sys.AddOpcode( "rol", 0x36, 1, AddressingType.ZEROPAGE_X, 6 );         // ROL $ll, X
      sys.AddOpcode( "ror", 0x6A, 0, AddressingType.IMPLICIT, 2 );           // ROR
      sys.AddOpcode( "ror", 0x6E, 2, AddressingType.ABSOLUTE, 6 );           // ROR $hhll
      sys.AddOpcode( "ror", 0x7E, 2, AddressingType.ABSOLUTE_X, 7 );         // ROR $hhll, X
      sys.AddOpcode( "ror", 0x66, 1, AddressingType.ZEROPAGE, 5 );           // ROR $ll
      sys.AddOpcode( "ror", 0x76, 1, AddressingType.ZEROPAGE_X, 6 );         // ROR $ll, X
      sys.AddOpcode( "rti", 0x40, 0, AddressingType.IMPLICIT, 6 );           // RTI
      sys.AddOpcode( "rts", 0x60, 0, AddressingType.IMPLICIT, 6 );           // RTS
      sys.AddOpcode( "sbc", 0xED, 2, AddressingType.ABSOLUTE, 4 );           // SBC $hhll
      sys.AddOpcode( "sbc", 0xFD, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // SBC $hhll, X
      sys.AddOpcode( "sbc", 0xF9, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // SBC $hhll, Y
      sys.AddOpcode( "sbc", 0xE5, 1, AddressingType.ZEROPAGE, 3 );           // SBC $ll
      sys.AddOpcode( "sbc", 0xF5, 1, AddressingType.ZEROPAGE_X, 4 );         // SBC $ll, X
      sys.AddOpcode( "sbc", 0xF1, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // SBC ($ll), Y
      sys.AddOpcode( "sbc", 0xE1, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // SBC ($ll,X)
      sys.AddOpcode( "sbc", 0xE9, 1, AddressingType.IMMEDIATE, 2 );          // SBC #$nn
      sys.AddOpcode( "sec", 0x38, 0, AddressingType.IMPLICIT, 2 );           // SEC
      sys.AddOpcode( "sed", 0xF8, 0, AddressingType.IMPLICIT, 2 );           // SED
      sys.AddOpcode( "sei", 0x78, 0, AddressingType.IMPLICIT, 2 );           // SEI
      sys.AddOpcode( "sta", 0x8D, 2, AddressingType.ABSOLUTE, 4 );           // STA $hhll
      sys.AddOpcode( "sta", 0x9D, 2, AddressingType.ABSOLUTE_X, 5 );         // STA $hhll, X
      sys.AddOpcode( "sta", 0x99, 2, AddressingType.ABSOLUTE_Y, 5 );         // STA $hhll, Y
      sys.AddOpcode( "sta", 0x85, 1, AddressingType.ZEROPAGE, 3 );           // STA $ll
      sys.AddOpcode( "sta", 0x95, 1, AddressingType.ZEROPAGE_X, 4 );         // STA $ll, X
      sys.AddOpcode( "sta", 0x91, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 6 );         // STA ($ll), Y
      sys.AddOpcode( "sta", 0x81, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // STA ($ll,X)
      sys.AddOpcode( "stx", 0x8E, 2, AddressingType.ABSOLUTE, 4 );           // STX $hhll
      sys.AddOpcode( "stx", 0x86, 1, AddressingType.ZEROPAGE, 3 );           // STX $ll
      sys.AddOpcode( "stx", 0x96, 1, AddressingType.ZEROPAGE_Y, 4 );         // STX $ll, Y
      sys.AddOpcode( "sty", 0x8C, 2, AddressingType.ABSOLUTE, 4 );           // STY $hhll
      sys.AddOpcode( "sty", 0x84, 1, AddressingType.ZEROPAGE, 3 );           // STY $ll
      sys.AddOpcode( "sty", 0x94, 1, AddressingType.ZEROPAGE_X, 4 );         // STY $ll, X
      sys.AddOpcode( "tax", 0xAA, 0, AddressingType.IMPLICIT, 2 );           // TAX
      sys.AddOpcode( "tay", 0xA8, 0, AddressingType.IMPLICIT, 2 );           // TAY
      sys.AddOpcode( "tsx", 0xBA, 0, AddressingType.IMPLICIT, 2 );           // TSX
      sys.AddOpcode( "txa", 0x8A, 0, AddressingType.IMPLICIT, 2 );           // TXA
      sys.AddOpcode( "txs", 0x9A, 0, AddressingType.IMPLICIT, 2 );           // TXS
      sys.AddOpcode( "tya", 0x98, 0, AddressingType.IMPLICIT, 2 );           // TYA

      sys.AddOpcode( "jam", 0x02, 0, AddressingType.IMPLICIT, 0 );           // JAM
      sys.AddOpcodeForDisassembly( "jam", 0x12, 0, AddressingType.IMPLICIT );      // JAM
      sys.AddOpcodeForDisassembly( "jam", 0x22, 0, AddressingType.IMPLICIT );      // JAM
      sys.AddOpcodeForDisassembly( "jam", 0x32, 0, AddressingType.IMPLICIT );      // JAM
      sys.AddOpcodeForDisassembly( "jam", 0x42, 0, AddressingType.IMPLICIT );      // JAM
      sys.AddOpcodeForDisassembly( "jam", 0x52, 0, AddressingType.IMPLICIT );      // JAM
      sys.AddOpcodeForDisassembly( "jam", 0x62, 0, AddressingType.IMPLICIT );      // JAM
      sys.AddOpcodeForDisassembly( "jam", 0x72, 0, AddressingType.IMPLICIT );      // JAM
      sys.AddOpcodeForDisassembly( "jam", 0x92, 0, AddressingType.IMPLICIT );      // JAM
      sys.AddOpcodeForDisassembly( "jam", 0xb2, 0, AddressingType.IMPLICIT );      // JAM
      sys.AddOpcodeForDisassembly( "jam", 0xd2, 0, AddressingType.IMPLICIT );      // JAM
      sys.AddOpcodeForDisassembly( "jam", 0xf2, 0, AddressingType.IMPLICIT );      // JAM

      //sys.AddOpcodeForDisassembly( "nop", 0x04, 1, AddressingType.ZEROPAGE );      // NOP $zp
      sys.AddOpcode( "nop", 0x04, 1, AddressingType.ZEROPAGE, 3 );      // NOP $zp
      //sys.AddOpcodeForDisassembly( "nop", 0x0C, 2, AddressingType.ABSOLUTE );      // NOP $abcd
      sys.AddOpcode( "nop", 0x0C, 2, AddressingType.ABSOLUTE, 4 );      // NOP $abcd
      //sys.AddOpcodeForDisassembly( "nop", 0x14, 1, AddressingType.ZEROPAGE_X );    // NOP $zp,x
      sys.AddOpcode( "nop", 0x14, 1, AddressingType.ZEROPAGE_X, 4 );    // NOP $zp,x
      sys.AddOpcodeForDisassembly( "nop", 0x1A, 0, AddressingType.IMPLICIT );      // NOP
      //sys.AddOpcodeForDisassembly( "nop", 0x1C, 2, AddressingType.ABSOLUTE_X );    // NOP $abcd,x
      sys.AddOpcode( "nop", 0x1C, 2, AddressingType.ABSOLUTE_X, 4, 1 );    // NOP $abcd,x
      sys.AddOpcodeForDisassembly( "nop", 0x34, 1, AddressingType.ZEROPAGE_X );    // NOP $zp,x
      sys.AddOpcodeForDisassembly( "nop", 0x3A, 0, AddressingType.IMPLICIT );      // NOP
      sys.AddOpcodeForDisassembly( "nop", 0x3C, 2, AddressingType.ABSOLUTE_X );    // NOP $abcd,x
      sys.AddOpcodeForDisassembly( "nop", 0x44, 1, AddressingType.ZEROPAGE );      // NOP $zp
      sys.AddOpcodeForDisassembly( "nop", 0x54, 1, AddressingType.ZEROPAGE_X );    // NOP $zp,x
      sys.AddOpcodeForDisassembly( "nop", 0x5A, 0, AddressingType.IMPLICIT );      // NOP
      sys.AddOpcodeForDisassembly( "nop", 0x5C, 2, AddressingType.ABSOLUTE_X );    // NOP $abcd,x
      sys.AddOpcodeForDisassembly( "nop", 0x64, 1, AddressingType.ZEROPAGE );      // NOP $zp
      sys.AddOpcodeForDisassembly( "nop", 0x74, 1, AddressingType.ZEROPAGE_X );    // NOP $zp,x
      sys.AddOpcodeForDisassembly( "nop", 0x7A, 0, AddressingType.IMPLICIT );      // NOP
      sys.AddOpcodeForDisassembly( "nop", 0x7C, 2, AddressingType.ABSOLUTE_X );    // NOP $abcd,x
      //sys.AddOpcodeForDisassembly( "nop", 0x80, 1, AddressingType.IMMEDIATE );        // NOP #xx
      sys.AddOpcode( "nop", 0x80, 1, AddressingType.IMMEDIATE, 2 );        // NOP #xx
      sys.AddOpcodeForDisassembly( "nop", 0x82, 1, AddressingType.IMMEDIATE );        // NOP #xx
      sys.AddOpcodeForDisassembly( "nop", 0x89, 1, AddressingType.IMMEDIATE );        // NOP #xx
      sys.AddOpcodeForDisassembly( "nop", 0xC2, 1, AddressingType.IMMEDIATE );        // NOP #xx
      sys.AddOpcodeForDisassembly( "nop", 0xD4, 1, AddressingType.ZEROPAGE_X );    // NOP $zp,x
      sys.AddOpcodeForDisassembly( "nop", 0xDA, 0, AddressingType.IMPLICIT );      // NOP
      sys.AddOpcodeForDisassembly( "nop", 0xDC, 2, AddressingType.ABSOLUTE_X );    // NOP $abcd,x
      sys.AddOpcodeForDisassembly( "nop", 0xE2, 1, AddressingType.IMMEDIATE );        // NOP #xx
      sys.AddOpcodeForDisassembly( "nop", 0xF4, 1, AddressingType.ZEROPAGE_X );    // NOP $zp,x
      sys.AddOpcodeForDisassembly( "nop", 0xFA, 0, AddressingType.IMPLICIT );      // NOP
      sys.AddOpcodeForDisassembly( "nop", 0xFC, 2, AddressingType.ABSOLUTE_X );    // NOP $abcd,x

      // Illegal opcodes
      sys.AddOpcode( "slo", 0x07, 1, AddressingType.ZEROPAGE, 5 );                 // SLO zp
      sys.AddOpcode( "slo", 0x17, 1, AddressingType.ZEROPAGE_X, 6 );               // SLO zp,x
      sys.AddOpcode( "slo", 0x03, 1, AddressingType.ZEROPAGE_INDIRECT_X, 8 );               // SLO ($ll,X)
      sys.AddOpcode( "slo", 0x13, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 8 );               // SLO ($ll), Y
      sys.AddOpcode( "slo", 0x0f, 2, AddressingType.ABSOLUTE, 6 );                 // SLO $hhll
      sys.AddOpcode( "slo", 0x1f, 2, AddressingType.ABSOLUTE_X, 7 );               // SLO $hhll, X
      sys.AddOpcode( "slo", 0x1b, 2, AddressingType.ABSOLUTE_Y, 7 );               // SLO $hhll, Y

      sys.AddOpcode( "rla", 0x27, 1, AddressingType.ZEROPAGE, 5 );      // RLA zp
      sys.AddOpcode( "rla", 0x37, 1, AddressingType.ZEROPAGE_X, 6 );    // RLA zp,x
      sys.AddOpcode( "rla", 0x23, 1, AddressingType.ZEROPAGE_INDIRECT_X, 8 );    // RLA ($ll,X)
      sys.AddOpcode( "rla", 0x33, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 8 );    // RLA ($ll), Y
      sys.AddOpcode( "rla", 0x2f, 2, AddressingType.ABSOLUTE, 6 );      // RLA $hhll
      sys.AddOpcode( "rla", 0x3f, 2, AddressingType.ABSOLUTE_X, 7 );    // RLA $hhll, X
      sys.AddOpcode( "rla", 0x3b, 2, AddressingType.ABSOLUTE_Y, 7 );    // RLA $hhll, Y

      sys.AddOpcode( "sre", 0x47, 1, AddressingType.ZEROPAGE, 5 );      // SRE zp
      sys.AddOpcode( "sre", 0x57, 1, AddressingType.ZEROPAGE_X, 6 );    // SRE zp,x
      sys.AddOpcode( "sre", 0x43, 1, AddressingType.ZEROPAGE_INDIRECT_X, 8 );    // SRE ($ll,X)
      sys.AddOpcode( "sre", 0x53, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 8 );    // SRE ($ll), Y
      sys.AddOpcode( "sre", 0x4f, 2, AddressingType.ABSOLUTE, 6 );      // SRE $hhll
      sys.AddOpcode( "sre", 0x5f, 2, AddressingType.ABSOLUTE_X, 7 );    // SRE $hhll, X
      sys.AddOpcode( "sre", 0x5b, 2, AddressingType.ABSOLUTE_Y, 7 );    // SRE $hhll, Y

      sys.AddOpcode( "rra", 0x67, 1, AddressingType.ZEROPAGE, 5 );      // RRA zp
      sys.AddOpcode( "rra", 0x77, 1, AddressingType.ZEROPAGE_X, 6 );    // RRA zp,x
      sys.AddOpcode( "rra", 0x63, 1, AddressingType.ZEROPAGE_INDIRECT_X, 8 );    // RRA ($ll,X)
      sys.AddOpcode( "rra", 0x73, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 8 );    // RRA ($ll), Y
      sys.AddOpcode( "rra", 0x6f, 2, AddressingType.ABSOLUTE, 6 );      // RRA $hhll
      sys.AddOpcode( "rra", 0x7f, 2, AddressingType.ABSOLUTE_X, 7 );    // RRA $hhll, X
      sys.AddOpcode( "rra", 0x7b, 2, AddressingType.ABSOLUTE_Y, 7 );    // RRA $hhll, Y

      sys.AddOpcode( "sax", 0x87, 1, AddressingType.ZEROPAGE, 3 );      // SAX zp
      sys.AddOpcode( "sax", 0x97, 1, AddressingType.ZEROPAGE_Y, 4 );    // SAX $ll, Y
      sys.AddOpcode( "sax", 0x83, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );    // SAX ($ll,X)
      sys.AddOpcode( "sax", 0x8f, 2, AddressingType.ABSOLUTE, 4 );      // SAX $hhll

      sys.AddOpcode( "lax", 0xa7, 1, AddressingType.ZEROPAGE, 3 );      // LAX zp
      sys.AddOpcode( "lax", 0xb7, 1, AddressingType.ZEROPAGE_Y, 4 );    // LAX $ll, Y
      sys.AddOpcode( "lax", 0xa3, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );    // LAX ($ll,X)
      sys.AddOpcode( "lax", 0xb3, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 ); // LAX ($ll), Y
      sys.AddOpcode( "lax", 0xaf, 2, AddressingType.ABSOLUTE, 4 );      // LAX $hhll
      sys.AddOpcode( "lax", 0xbf, 2, AddressingType.ABSOLUTE_Y, 4, 1 ); // LAX $hhll, Y

      sys.AddOpcode( "dcp", 0xc7, 1, AddressingType.ZEROPAGE, 5 );      // DCP zp
      sys.AddOpcode( "dcp", 0xd7, 1, AddressingType.ZEROPAGE_X, 6 );    // DCP zp,x
      sys.AddOpcode( "dcp", 0xc3, 1, AddressingType.ZEROPAGE_INDIRECT_X, 8 );    // DCP ($ll,X)
      sys.AddOpcode( "dcp", 0xd3, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 8 );    // DCP ($ll), Y
      sys.AddOpcode( "dcp", 0xcf, 2, AddressingType.ABSOLUTE, 6 );      // DCP $hhll
      sys.AddOpcode( "dcp", 0xdf, 2, AddressingType.ABSOLUTE_X, 7 );    // DCP $hhll, X
      sys.AddOpcode( "dcp", 0xdb, 2, AddressingType.ABSOLUTE_Y, 7 );    // DCP $hhll, Y

      sys.AddOpcode( "isc", 0xe7, 1, AddressingType.ZEROPAGE, 5 );      // ISC zp
      sys.AddOpcode( "isc", 0xf7, 1, AddressingType.ZEROPAGE_X, 6 );    // ISC zp,x
      sys.AddOpcode( "isc", 0xe3, 1, AddressingType.ZEROPAGE_INDIRECT_X, 8 );    // ISC ($ll,X)
      sys.AddOpcode( "isc", 0xf3, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 8 );    // ISC ($ll), Y
      sys.AddOpcode( "isc", 0xef, 2, AddressingType.ABSOLUTE, 6 );      // ISC $hhll
      sys.AddOpcode( "isc", 0xff, 2, AddressingType.ABSOLUTE_X, 7 );    // ISC $hhll, X
      sys.AddOpcode( "isc", 0xfb, 2, AddressingType.ABSOLUTE_Y, 7 );    // ISC $hhll, Y

      sys.AddOpcode( "anc", 0x0b, 1, AddressingType.IMMEDIATE, 2 );        // anc #$nn
      sys.AddOpcode( "anc", 0x2b, 1, AddressingType.IMMEDIATE, 2 );        // anc #$nn

      sys.AddOpcode( "alr", 0x4b, 1, AddressingType.IMMEDIATE, 2 );        // alr #$nn
      sys.AddOpcode( "arr", 0x6b, 1, AddressingType.IMMEDIATE, 2 );        // arr #$nn

      // highly unstable!
      sys.AddOpcode( "ane", 0x8b, 1, AddressingType.IMMEDIATE, 2 );        // ane #$nn
      sys.AddOpcode( "xaa", 0x8b, 1, AddressingType.IMMEDIATE, 2 );        // xaa #$nn
      // highly unstable!
      sys.AddOpcode( "lxa", 0xab, 1, AddressingType.IMMEDIATE, 2 );        // lxa #$nn
      sys.AddOpcode( "lax", 0xab, 1, AddressingType.IMMEDIATE, 2 );        // lax #$nn

      sys.AddOpcode( "sbx", 0xcb, 1, AddressingType.IMMEDIATE, 2 );        // sbx #$nn
      sys.AddOpcode( "axs", 0xcb, 1, AddressingType.IMMEDIATE, 2 );        // axs #$nn  (alternative to sbx)
      sys.AddOpcodeForDisassembly( "sbc", 0xeb, 1, AddressingType.IMMEDIATE );        // sbc #$nn

      // unstable!
      sys.AddOpcode( "sha", 0x93, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5 );    // sha ($ll),Y
      sys.AddOpcode( "ahx", 0x93, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5 );    // sha ($ll),Y
      // unstable!
      sys.AddOpcode( "sha", 0x9f, 2, AddressingType.ABSOLUTE_Y, 5 );    // sha $hhll, Y
      sys.AddOpcode( "ahx", 0x9f, 2, AddressingType.ABSOLUTE_Y, 5 );    // sha $hhll, Y

      // unstable!
      sys.AddOpcode( "shy", 0x9c, 2, AddressingType.ABSOLUTE_X, 5 );    // shy $hhll, X
      // unstable!
      sys.AddOpcode( "shx", 0x9e, 2, AddressingType.ABSOLUTE_Y, 5 );    // shx $hhll, Y
      // unstable!
      sys.AddOpcode( "tas", 0x9b, 2, AddressingType.ABSOLUTE_Y, 5 );    // tas $hhll, Y

      sys.AddOpcode( "las", 0xbb, 2, AddressingType.ABSOLUTE_Y, 4, 1 );    // las $hhll, Y
      return sys;
    }



    public static Processor Create65C02()
    {
      // 6502 plus BRA,PHX/Y,PLX/Y,STZ,TRB/TSB
      var  sys = new Processor( "65C02" );

      sys.AddOpcode( "adc", 0x6d, 2, AddressingType.ABSOLUTE, 4, 1 );        // ADC $hhll
      sys.AddOpcode( "adc", 0x7d, 2, AddressingType.ABSOLUTE_X, 4, 1, 1, 0 );// ADC $hhll, X
      sys.AddOpcode( "adc", 0x79, 2, AddressingType.ABSOLUTE_Y, 4, 1, 1, 0 );// ADC $hhll, Y
      sys.AddOpcode( "adc", 0x65, 1, AddressingType.ZEROPAGE, 3, 1, 1, 0 );  // ADC $ll
      sys.AddOpcode( "adc", 0x75, 1, AddressingType.ZEROPAGE_X, 4, 1 );      // ADC $ll, X
      sys.AddOpcode( "adc", 0x71, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1, 1, 0 );// ADC ($ll), Y
      sys.AddOpcode( "adc", 0x61, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6, 1 );      // ADC ($ll,X)
      sys.AddOpcode( "adc", 0x69, 1, AddressingType.IMMEDIATE, 2, 1 );       // ADC #$nn
      sys.AddOpcode( "and", 0x2d, 2, AddressingType.ABSOLUTE, 4 );           // AND $hhll
      sys.AddOpcode( "and", 0x3d, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // AND $hhll, X
      sys.AddOpcode( "and", 0x39, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // AND $hhll, Y
      sys.AddOpcode( "and", 0x25, 1, AddressingType.ZEROPAGE, 3 );           // AND $ll
      sys.AddOpcode( "and", 0x35, 1, AddressingType.ZEROPAGE_X, 4 );         // AND $ll, X
      sys.AddOpcode( "and", 0x31, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // AND ($ll), Y
      sys.AddOpcode( "and", 0x21, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // AND ($ll,X)
      sys.AddOpcode( "and", 0x29, 1, AddressingType.IMMEDIATE, 2 );          // AND #$nn
      sys.AddOpcode( "asl", 0x0a, 0, AddressingType.IMPLICIT, 2 );           // ASL
      sys.AddOpcode( "asl", 0x0e, 2, AddressingType.ABSOLUTE, 6 );           // ASL $hhll
      sys.AddOpcode( "asl", 0x1e, 2, AddressingType.ABSOLUTE_X, 6, 1 );      // ASL $hhll, X
      sys.AddOpcode( "asl", 0x06, 1, AddressingType.ZEROPAGE, 5 );           // ASL $ll
      sys.AddOpcode( "asl", 0x16, 1, AddressingType.ZEROPAGE_X, 6 );         // ASL $ll, X
      sys.AddOpcode( "bcc", 0x90, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BCC $hhll
      sys.AddOpcode( "bcs", 0xB0, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BCS $hhll
      sys.AddOpcode( "beq", 0xF0, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BEQ $hhll
      sys.AddOpcode( "bit", 0x2c, 2, AddressingType.ABSOLUTE, 4 );           // BIT $hhll
      sys.AddOpcode( "bit", 0x24, 1, AddressingType.ZEROPAGE, 3 );           // BIT $ll
      sys.AddOpcode( "bmi", 0x30, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BMI $hhll
      sys.AddOpcode( "bne", 0xD0, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BNE $hhll
      sys.AddOpcode( "bpl", 0x10, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BPL $hhll
      sys.AddOpcode( "brk", 0x00, 0, AddressingType.IMPLICIT, 7 );           // BRK
      sys.AddOpcode( "bvc", 0x50, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BVC $hhll
      sys.AddOpcode( "bvs", 0x70, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BVS $hhll
      sys.AddOpcode( "clc", 0x18, 0, AddressingType.IMPLICIT, 2 );           // CLC
      sys.AddOpcode( "cld", 0xD8, 0, AddressingType.IMPLICIT, 2 );           // CLD
      sys.AddOpcode( "cli", 0x58, 0, AddressingType.IMPLICIT, 2 );           // CLI
      sys.AddOpcode( "clv", 0xB8, 0, AddressingType.IMPLICIT, 2 );           // CLV
      sys.AddOpcode( "cmp", 0xCD, 2, AddressingType.ABSOLUTE, 4 );           // CMP $hhll
      sys.AddOpcode( "cmp", 0xDD, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // CMP $hhll, X
      sys.AddOpcode( "cmp", 0xD9, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // CMP $hhll, Y
      sys.AddOpcode( "cmp", 0xC5, 1, AddressingType.ZEROPAGE, 3 );           // CMP $ll
      sys.AddOpcode( "cmp", 0xD5, 1, AddressingType.ZEROPAGE_X, 4 );         // CMP $ll, X
      sys.AddOpcode( "cmp", 0xD1, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // CMP ($ll), Y
      sys.AddOpcode( "cmp", 0xC1, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // CMP ($ll,X)
      sys.AddOpcode( "cmp", 0xC9, 1, AddressingType.IMMEDIATE, 2 );          // CMP #$nn
      sys.AddOpcode( "cpx", 0xEC, 2, AddressingType.ABSOLUTE, 4 );           // CPX $hhll
      sys.AddOpcode( "cpx", 0xE4, 1, AddressingType.ZEROPAGE, 3 );           // CPX $ll
      sys.AddOpcode( "cpx", 0xE0, 1, AddressingType.IMMEDIATE, 2 );          // CPX #$nn
      sys.AddOpcode( "cpy", 0xCC, 2, AddressingType.ABSOLUTE, 4 );           // CPY $hhll
      sys.AddOpcode( "cpy", 0xC4, 1, AddressingType.ZEROPAGE, 3 );           // CPY $ll
      sys.AddOpcode( "cpy", 0xC0, 1, AddressingType.IMMEDIATE, 2 );          // CPY #$nn
      sys.AddOpcode( "dec", 0xCE, 2, AddressingType.ABSOLUTE, 6 );           // DEC $hhll
      sys.AddOpcode( "dec", 0xDE, 2, AddressingType.ABSOLUTE_X, 7 );         // DEC $hhll, X
      sys.AddOpcode( "dec", 0xC6, 1, AddressingType.ZEROPAGE, 5 );           // DEC $ll
      sys.AddOpcode( "dec", 0xD6, 1, AddressingType.ZEROPAGE_X, 6 );         // DEC $ll, X
      sys.AddOpcode( "dex", 0xCA, 0, AddressingType.IMPLICIT, 2 );           // DEX
      sys.AddOpcode( "dey", 0x88, 0, AddressingType.IMPLICIT, 2 );           // DEY
      sys.AddOpcode( "eor", 0x4D, 2, AddressingType.ABSOLUTE, 4 );           // EOR $hhll
      sys.AddOpcode( "eor", 0x5D, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // EOR $hhll, X
      sys.AddOpcode( "eor", 0x59, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // EOR $hhll, Y
      sys.AddOpcode( "eor", 0x45, 1, AddressingType.ZEROPAGE, 3 );           // EOR $ll
      sys.AddOpcode( "eor", 0x55, 1, AddressingType.ZEROPAGE_X, 4 );         // EOR $ll, X
      sys.AddOpcode( "eor", 0x51, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // EOR ($ll), Y
      sys.AddOpcode( "eor", 0x41, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // EOR ($ll,X)
      sys.AddOpcode( "eor", 0x49, 1, AddressingType.IMMEDIATE, 2 );          // EOR #$nn
      sys.AddOpcode( "inc", 0xEE, 2, AddressingType.ABSOLUTE, 6 );           // INC $hhll
      sys.AddOpcode( "inc", 0xFE, 2, AddressingType.ABSOLUTE_X, 7 );         // INC $hhll, X
      sys.AddOpcode( "inc", 0xE6, 1, AddressingType.ZEROPAGE, 5 );           // INC $ll
      sys.AddOpcode( "inc", 0xF6, 1, AddressingType.ZEROPAGE_X, 6 );         // INC $ll, X
      sys.AddOpcode( "inx", 0xE8, 0, AddressingType.IMPLICIT, 2 );           // INX
      sys.AddOpcode( "iny", 0xC8, 0, AddressingType.IMPLICIT, 2 );           // INY
      sys.AddOpcode( "jmp", 0x4C, 2, AddressingType.ABSOLUTE, 3 );           // JMP $hhll
      sys.AddOpcode( "jmp", 0x6C, 2, AddressingType.INDIRECT, 6 );           // JMP ($hhll)
      sys.AddOpcode( "jsr", 0x20, 2, AddressingType.ABSOLUTE, 6 );           // JSR $hhll
      sys.AddOpcode( "lda", 0xAD, 2, AddressingType.ABSOLUTE, 4 );           // LDA $hhll
      sys.AddOpcode( "lda", 0xBD, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // LDA $hhll, X
      sys.AddOpcode( "lda", 0xB9, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // LDA $hhll, Y
      sys.AddOpcode( "lda", 0xA5, 1, AddressingType.ZEROPAGE, 3 );           // LDA $ll
      sys.AddOpcode( "lda", 0xB5, 1, AddressingType.ZEROPAGE_X, 4 );         // LDA $ll, X
      sys.AddOpcode( "lda", 0xB1, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // LDA ($ll), Y
      sys.AddOpcode( "lda", 0xA1, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // LDA ($ll,X)
      sys.AddOpcode( "lda", 0xA9, 1, AddressingType.IMMEDIATE, 2 );          // LDA #$nn
      sys.AddOpcode( "ldx", 0xAE, 2, AddressingType.ABSOLUTE, 4 );           // LDX $hhll
      sys.AddOpcode( "ldx", 0xBE, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // LDX $hhll, Y
      sys.AddOpcode( "ldx", 0xA6, 1, AddressingType.ZEROPAGE, 3 );           // LDX $ll
      sys.AddOpcode( "ldx", 0xB6, 1, AddressingType.ZEROPAGE_Y, 4 );         // LDX $ll, Y
      sys.AddOpcode( "ldx", 0xA2, 1, AddressingType.IMMEDIATE, 2 );          // LDX #$nn
      sys.AddOpcode( "ldy", 0xAC, 2, AddressingType.ABSOLUTE, 4 );           // LDY $hhll
      sys.AddOpcode( "ldy", 0xBC, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // LDY $hhll, X
      sys.AddOpcode( "ldy", 0xA4, 1, AddressingType.ZEROPAGE, 3 );           // LDY $ll
      sys.AddOpcode( "ldy", 0xB4, 1, AddressingType.ZEROPAGE_X, 4 );         // LDY $ll, X
      sys.AddOpcode( "ldy", 0xA0, 1, AddressingType.IMMEDIATE, 2 );          // LDY #$nn
      sys.AddOpcode( "lsr", 0x4A, 0, AddressingType.IMPLICIT, 2 );           // LSR
      sys.AddOpcode( "lsr", 0x4E, 2, AddressingType.ABSOLUTE, 6 );           // LSR $hhll
      sys.AddOpcode( "lsr", 0x5E, 2, AddressingType.ABSOLUTE_X, 6, 1 );      // LSR $hhll, X
      sys.AddOpcode( "lsr", 0x46, 1, AddressingType.ZEROPAGE, 5 );           // LSR $ll
      sys.AddOpcode( "lsr", 0x56, 1, AddressingType.ZEROPAGE_X, 6 );         // LSR $ll, X
      sys.AddOpcode( "nop", 0xEA, 0, AddressingType.IMPLICIT, 2 );           // NOP
      sys.AddOpcode( "ora", 0x0D, 2, AddressingType.ABSOLUTE, 4 );           // ORA $hhll
      sys.AddOpcode( "ora", 0x1D, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // ORA $hhll, X
      sys.AddOpcode( "ora", 0x19, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // ORA $hhll, Y
      sys.AddOpcode( "ora", 0x05, 1, AddressingType.ZEROPAGE, 3 );           // ORA $ll
      sys.AddOpcode( "ora", 0x15, 1, AddressingType.ZEROPAGE_X, 4 );         // ORA $ll, X
      sys.AddOpcode( "ora", 0x11, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // ORA ($ll), Y
      sys.AddOpcode( "ora", 0x01, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // ORA ($ll,X)
      sys.AddOpcode( "ora", 0x09, 1, AddressingType.IMMEDIATE, 2 );          // ORA #$nn
      sys.AddOpcode( "pha", 0x48, 0, AddressingType.IMPLICIT, 3 );           // PHA
      sys.AddOpcode( "php", 0x08, 0, AddressingType.IMPLICIT, 3 );           // PHP
      sys.AddOpcode( "pla", 0x68, 0, AddressingType.IMPLICIT, 4 );           // PLA
      sys.AddOpcode( "plp", 0x28, 0, AddressingType.IMPLICIT, 4 );           // PLP
      sys.AddOpcode( "rol", 0x2A, 0, AddressingType.IMPLICIT, 2 );           // ROL
      sys.AddOpcode( "rol", 0x2E, 2, AddressingType.ABSOLUTE, 6 );           // ROL $hhll
      sys.AddOpcode( "rol", 0x3E, 2, AddressingType.ABSOLUTE_X, 6, 1 );      // ROL $hhll, X
      sys.AddOpcode( "rol", 0x26, 1, AddressingType.ZEROPAGE, 5 );           // ROL $ll
      sys.AddOpcode( "rol", 0x36, 1, AddressingType.ZEROPAGE_X, 6 );         // ROL $ll, X
      sys.AddOpcode( "ror", 0x6A, 0, AddressingType.IMPLICIT, 2 );           // ROR
      sys.AddOpcode( "ror", 0x6E, 2, AddressingType.ABSOLUTE, 6 );           // ROR $hhll
      sys.AddOpcode( "ror", 0x7E, 2, AddressingType.ABSOLUTE_X, 6, 1 );      // ROR $hhll, X
      sys.AddOpcode( "ror", 0x66, 1, AddressingType.ZEROPAGE, 5 );           // ROR $ll
      sys.AddOpcode( "ror", 0x76, 1, AddressingType.ZEROPAGE_X, 6 );         // ROR $ll, X
      sys.AddOpcode( "rti", 0x40, 0, AddressingType.IMPLICIT, 6 );           // RTI
      sys.AddOpcode( "rts", 0x60, 0, AddressingType.IMPLICIT, 6 );           // RTS
      sys.AddOpcode( "sbc", 0xED, 2, AddressingType.ABSOLUTE, 4, 1 );        // SBC $hhll
      sys.AddOpcode( "sbc", 0xFD, 2, AddressingType.ABSOLUTE_X, 4, 1, 1, 0 );// SBC $hhll, X
      sys.AddOpcode( "sbc", 0xF9, 2, AddressingType.ABSOLUTE_Y, 4, 1, 1, 0 );// SBC $hhll, Y
      sys.AddOpcode( "sbc", 0xE5, 1, AddressingType.ZEROPAGE, 3, 1 );        // SBC $ll
      sys.AddOpcode( "sbc", 0xF5, 1, AddressingType.ZEROPAGE_X, 4, 1 );      // SBC $ll, X
      sys.AddOpcode( "sbc", 0xF1, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1, 1, 0 );// SBC ($ll), Y
      sys.AddOpcode( "sbc", 0xE1, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6, 1 );      // SBC ($ll,X)
      sys.AddOpcode( "sbc", 0xE9, 1, AddressingType.IMMEDIATE, 2, 1 );       // SBC #$nn
      sys.AddOpcode( "sec", 0x38, 0, AddressingType.IMPLICIT, 2 );           // SEC
      sys.AddOpcode( "sed", 0xF8, 0, AddressingType.IMPLICIT, 2 );           // SED
      sys.AddOpcode( "sei", 0x78, 0, AddressingType.IMPLICIT, 2 );           // SEI
      sys.AddOpcode( "sta", 0x8D, 2, AddressingType.ABSOLUTE, 4 );           // STA $hhll
      sys.AddOpcode( "sta", 0x9D, 2, AddressingType.ABSOLUTE_X, 5 );         // STA $hhll, X
      sys.AddOpcode( "sta", 0x99, 2, AddressingType.ABSOLUTE_Y, 5 );         // STA $hhll, Y
      sys.AddOpcode( "sta", 0x85, 1, AddressingType.ZEROPAGE, 3 );           // STA $ll
      sys.AddOpcode( "sta", 0x95, 1, AddressingType.ZEROPAGE_X, 4 );         // STA $ll, X
      sys.AddOpcode( "sta", 0x91, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 6 );         // STA ($ll), Y
      sys.AddOpcode( "sta", 0x81, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // STA ($ll,X)
      sys.AddOpcode( "stx", 0x8E, 2, AddressingType.ABSOLUTE, 4 );           // STX $hhll
      sys.AddOpcode( "stx", 0x86, 1, AddressingType.ZEROPAGE, 3 );           // STX $ll
      sys.AddOpcode( "stx", 0x96, 1, AddressingType.ZEROPAGE_Y, 4 );         // STX $ll, Y
      sys.AddOpcode( "sty", 0x8C, 2, AddressingType.ABSOLUTE, 4 );           // STY $hhll
      sys.AddOpcode( "sty", 0x84, 1, AddressingType.ZEROPAGE, 3 );           // STY $ll
      sys.AddOpcode( "sty", 0x94, 1, AddressingType.ZEROPAGE_X, 4 );         // STY $ll, X
      sys.AddOpcode( "tax", 0xAA, 0, AddressingType.IMPLICIT, 2 );           // TAX
      sys.AddOpcode( "tay", 0xA8, 0, AddressingType.IMPLICIT, 2 );           // TAY
      sys.AddOpcode( "tsx", 0xBA, 0, AddressingType.IMPLICIT, 2 );           // TSX
      sys.AddOpcode( "txa", 0x8A, 0, AddressingType.IMPLICIT, 2 );           // TXA
      sys.AddOpcode( "txs", 0x9A, 0, AddressingType.IMPLICIT, 2 );           // TXS
      sys.AddOpcode( "tya", 0x98, 0, AddressingType.IMPLICIT, 2 );           // TYA


      // pageboundary cycle add is WRONG, it's for decimal mode
      sys.AddOpcode( "adc", 0x72, 1, AddressingType.ZEROPAGE_INDIRECT, 5, 1 );   // adc ($12)
      sys.AddOpcode( "and", 0x32, 1, AddressingType.ZEROPAGE_INDIRECT, 5 );      // and ($12)
      sys.AddOpcode( "cmp", 0xd2, 1, AddressingType.ZEROPAGE_INDIRECT, 5 );      // cmp ($12)
      sys.AddOpcode( "eor", 0x52, 1, AddressingType.ZEROPAGE_INDIRECT, 5 );      // eor ($12)
      sys.AddOpcode( "lda", 0xb2, 1, AddressingType.ZEROPAGE_INDIRECT, 5 );      // lda ($12)
      sys.AddOpcode( "ora", 0x12, 1, AddressingType.ZEROPAGE_INDIRECT, 5 );      // ora ($12)
      sys.AddOpcode( "sbc", 0xf2, 1, AddressingType.ZEROPAGE_INDIRECT, 5, 1 );   // sbc ($12)
      sys.AddOpcode( "sta", 0x92, 1, AddressingType.ZEROPAGE_INDIRECT, 5 );      // sta ($12)

      sys.AddOpcode( "bit", 0x89, 1, AddressingType.IMMEDIATE, 2 );              // bit #$12
      sys.AddOpcode( "bit", 0x34, 1, AddressingType.ZEROPAGE_X, 2 );             // bit $12,x
      sys.AddOpcode( "bit", 0x3C, 2, AddressingType.ABSOLUTE_X, 4, 1 );          // bit $1234,x

      sys.AddOpcode( "dec", 0x3A, 0, AddressingType.IMPLICIT, 2 );               // dec
      sys.AddOpcode( "inc", 0x1A, 0, AddressingType.IMPLICIT, 2 );               // inc

      sys.AddOpcode( "jmp", 0x7C, 2, AddressingType.ABSOLUTE_INDIRECT_X, 6 );    // jmp ($1234,x)

      sys.AddOpcode( "bra", 0x80, 1, AddressingType.RELATIVE, 3, 1 );            // bra label

      sys.AddOpcode( "phx", 0xDA, 0, AddressingType.IMPLICIT, 3 );               // phx
      sys.AddOpcode( "phy", 0x5A, 0, AddressingType.IMPLICIT, 3 );               // phy
      sys.AddOpcode( "plx", 0xfA, 0, AddressingType.IMPLICIT, 4 );               // plx
      sys.AddOpcode( "ply", 0x7A, 0, AddressingType.IMPLICIT, 4 );               // ply

      sys.AddOpcode( "stz", 0x64, 1, AddressingType.ZEROPAGE, 3 );               // stz $12
      sys.AddOpcode( "stz", 0x74, 1, AddressingType.ZEROPAGE_X, 4 );             // stz $12,x
      sys.AddOpcode( "stz", 0x9c, 2, AddressingType.ABSOLUTE, 4 );               // stz $1234
      sys.AddOpcode( "stz", 0x9e, 2, AddressingType.ABSOLUTE_X, 5 );             // stz $1234,x

      sys.AddOpcode( "trb", 0x14, 1, AddressingType.ZEROPAGE, 5 );               // trb $12
      sys.AddOpcode( "trb", 0x1C, 2, AddressingType.ABSOLUTE, 6 );               // trb $1234

      sys.AddOpcode( "tsb", 0x04, 1, AddressingType.ZEROPAGE, 5 );               // tsb $12
      sys.AddOpcode( "tsb", 0x0C, 2, AddressingType.ABSOLUTE, 6 );               // tsb $1234

      sys.AddOpcodeForDisassembly( "nop", 0x0f, 2, AddressingType.ZEROPAGE_RELATIVE );     // bbr0 $12,LABEL
      sys.AddOpcodeForDisassembly( "nop", 0x1f, 2, AddressingType.ZEROPAGE_RELATIVE );     // bbr1 $12,LABEL
      sys.AddOpcodeForDisassembly( "nop", 0x2f, 2, AddressingType.ZEROPAGE_RELATIVE );     // bbr2 $12,LABEL
      sys.AddOpcodeForDisassembly( "nop", 0x3f, 2, AddressingType.ZEROPAGE_RELATIVE );     // bbr3 $12,LABEL
      sys.AddOpcodeForDisassembly( "nop", 0x4f, 2, AddressingType.ZEROPAGE_RELATIVE );     // bbr4 $12,LABEL
      sys.AddOpcodeForDisassembly( "nop", 0x5f, 2, AddressingType.ZEROPAGE_RELATIVE );     // bbr5 $12,LABEL
      sys.AddOpcodeForDisassembly( "nop", 0x6f, 2, AddressingType.ZEROPAGE_RELATIVE );     // bbr6 $12,LABEL
      sys.AddOpcodeForDisassembly( "nop", 0x7f, 2, AddressingType.ZEROPAGE_RELATIVE );     // bbr7 $12,LABEL
      sys.AddOpcodeForDisassembly( "nop", 0x8f, 2, AddressingType.ZEROPAGE_RELATIVE );     // bbs0 $12,LABEL
      sys.AddOpcodeForDisassembly( "nop", 0x9f, 2, AddressingType.ZEROPAGE_RELATIVE );     // bbs1 $12,LABEL
      sys.AddOpcodeForDisassembly( "nop", 0xaf, 2, AddressingType.ZEROPAGE_RELATIVE );     // bbs2 $12,LABEL
      sys.AddOpcodeForDisassembly( "nop", 0xbf, 2, AddressingType.ZEROPAGE_RELATIVE );     // bbs3 $12,LABEL
      sys.AddOpcodeForDisassembly( "nop", 0xcf, 2, AddressingType.ZEROPAGE_RELATIVE );     // bbs4 $12,LABEL
      sys.AddOpcodeForDisassembly( "nop", 0xdf, 2, AddressingType.ZEROPAGE_RELATIVE );     // bbs5 $12,LABEL
      sys.AddOpcodeForDisassembly( "nop", 0xef, 2, AddressingType.ZEROPAGE_RELATIVE );     // bbs6 $12,LABEL
      sys.AddOpcodeForDisassembly( "nop", 0xff, 2, AddressingType.ZEROPAGE_RELATIVE );     // bbs7 $12,LABEL

      sys.AddOpcodeForDisassembly( "nop", 0x07, 2, AddressingType.ZEROPAGE_RELATIVE );     // rmb0 $12,LABEL
      sys.AddOpcodeForDisassembly( "nop", 0x17, 2, AddressingType.ZEROPAGE_RELATIVE );     // rmb1 $12,LABEL
      sys.AddOpcodeForDisassembly( "nop", 0x27, 2, AddressingType.ZEROPAGE_RELATIVE );     // rmb2 $12,LABEL
      sys.AddOpcodeForDisassembly( "nop", 0x37, 2, AddressingType.ZEROPAGE_RELATIVE );     // rmb3 $12,LABEL
      sys.AddOpcodeForDisassembly( "nop", 0x47, 2, AddressingType.ZEROPAGE_RELATIVE );     // rmb4 $12,LABEL
      sys.AddOpcodeForDisassembly( "nop", 0x57, 2, AddressingType.ZEROPAGE_RELATIVE );     // rmb5 $12,LABEL
      sys.AddOpcodeForDisassembly( "nop", 0x67, 2, AddressingType.ZEROPAGE_RELATIVE );     // rmb6 $12,LABEL
      sys.AddOpcodeForDisassembly( "nop", 0x77, 2, AddressingType.ZEROPAGE_RELATIVE );     // rmb7 $12,LABEL
      sys.AddOpcodeForDisassembly( "nop", 0x87, 2, AddressingType.ZEROPAGE_RELATIVE );     // smb0 $12,LABEL
      sys.AddOpcodeForDisassembly( "nop", 0x97, 2, AddressingType.ZEROPAGE_RELATIVE );     // smb1 $12,LABEL
      sys.AddOpcodeForDisassembly( "nop", 0xa7, 2, AddressingType.ZEROPAGE_RELATIVE );     // smb2 $12,LABEL
      sys.AddOpcodeForDisassembly( "nop", 0xb7, 2, AddressingType.ZEROPAGE_RELATIVE );     // smb3 $12,LABEL
      sys.AddOpcodeForDisassembly( "nop", 0xc7, 2, AddressingType.ZEROPAGE_RELATIVE );     // smb4 $12,LABEL
      sys.AddOpcodeForDisassembly( "nop", 0xd7, 2, AddressingType.ZEROPAGE_RELATIVE );     // smb5 $12,LABEL
      sys.AddOpcodeForDisassembly( "nop", 0xe7, 2, AddressingType.ZEROPAGE_RELATIVE );     // smb6 $12,LABEL
      sys.AddOpcodeForDisassembly( "nop", 0xf7, 2, AddressingType.ZEROPAGE_RELATIVE );     // smb7 $12,LABEL

      sys.AddOpcodeForDisassembly( "nop", 0xdb, 0, AddressingType.IMPLICIT );               // STP

      sys.AddOpcodeForDisassembly( "nop", 0xcb, 0, AddressingType.IMPLICIT );               // wai

      sys.AddOpcodeForDisassembly( "nop", 0x02, 1, AddressingType.IMPLICIT );           // JAM
      sys.AddOpcodeForDisassembly( "nop", 0x22, 1, AddressingType.IMPLICIT );      // JAM
      sys.AddOpcodeForDisassembly( "nop", 0x42, 1, AddressingType.IMPLICIT );      // JAM
      sys.AddOpcodeForDisassembly( "nop", 0x62, 1, AddressingType.IMPLICIT );      // JAM
      sys.AddOpcodeForDisassembly( "nop", 0x82, 1, AddressingType.IMMEDIATE );        // NOP #xx
      sys.AddOpcodeForDisassembly( "nop", 0xC2, 1, AddressingType.IMMEDIATE );        // NOP #xx
      sys.AddOpcodeForDisassembly( "nop", 0xE2, 1, AddressingType.IMMEDIATE );        // NOP #xx

      sys.AddOpcodeForDisassembly( "nop", 0x03, 0, AddressingType.ZEROPAGE_INDIRECT_X );
      sys.AddOpcodeForDisassembly( "nop", 0x13, 0, AddressingType.ZEROPAGE_INDIRECT_Y );               // SLO ($ll), Y
      sys.AddOpcodeForDisassembly( "nop", 0x23, 0, AddressingType.ZEROPAGE_INDIRECT_X );    // RLA ($ll,X)
      sys.AddOpcodeForDisassembly( "nop", 0x33, 0, AddressingType.ZEROPAGE_INDIRECT_Y );    // RLA ($ll), Y
      sys.AddOpcodeForDisassembly( "nop", 0x43, 0, AddressingType.ZEROPAGE_INDIRECT_X );    // SRE ($ll,X)
      sys.AddOpcodeForDisassembly( "nop", 0x53, 0, AddressingType.ZEROPAGE_INDIRECT_Y );    // SRE ($ll), Y
      sys.AddOpcodeForDisassembly( "nop", 0x63, 0, AddressingType.ZEROPAGE_INDIRECT_X );    // RRA ($ll,X)
      sys.AddOpcodeForDisassembly( "nop", 0x73, 0, AddressingType.ZEROPAGE_INDIRECT_Y );    // RRA ($ll), Y
      sys.AddOpcodeForDisassembly( "nop", 0x83, 0, AddressingType.ZEROPAGE_INDIRECT_X );    // SAX ($ll,X)
      sys.AddOpcodeForDisassembly( "nop", 0x93, 0, AddressingType.ABSOLUTE_X );    // ahx $hhll, X
      sys.AddOpcodeForDisassembly( "nop", 0xa3, 0, AddressingType.ZEROPAGE_INDIRECT_X );    // LAX ($ll,X)
      sys.AddOpcodeForDisassembly( "nop", 0xb3, 0, AddressingType.ZEROPAGE_INDIRECT_Y ); // LAX ($ll), Y
      sys.AddOpcodeForDisassembly( "nop", 0xc3, 0, AddressingType.ZEROPAGE_INDIRECT_X );    // DCP ($ll,X)
      sys.AddOpcodeForDisassembly( "nop", 0xd3, 0, AddressingType.ZEROPAGE_INDIRECT_Y );    // DCP ($ll), Y
      sys.AddOpcodeForDisassembly( "nop", 0xe3, 0, AddressingType.ZEROPAGE_INDIRECT_X );    // ISC ($ll,X)
      sys.AddOpcodeForDisassembly( "nop", 0xf3, 0, AddressingType.ZEROPAGE_INDIRECT_Y );    // ISC ($ll), Y

      sys.AddOpcodeForDisassembly( "nop", 0x44, 1, AddressingType.ZEROPAGE );      // NOP $zp
      sys.AddOpcodeForDisassembly( "nop", 0x54, 1, AddressingType.ZEROPAGE_X );    // NOP $zp,x
      sys.AddOpcodeForDisassembly( "nop", 0xD4, 1, AddressingType.ZEROPAGE_X );    // NOP $zp,x
      sys.AddOpcodeForDisassembly( "nop", 0xF4, 1, AddressingType.ZEROPAGE_X );    // NOP $zp,x

      sys.AddOpcodeForDisassembly( "nop", 0x0b, 0, AddressingType.IMMEDIATE );        // anc #$nn
      sys.AddOpcodeForDisassembly( "nop", 0x1b, 0, AddressingType.ABSOLUTE_Y );               // SLO $hhll, Y
      sys.AddOpcodeForDisassembly( "nop", 0x2b, 0, AddressingType.IMMEDIATE );        // anc #$nn
      sys.AddOpcodeForDisassembly( "nop", 0x3b, 0, AddressingType.ABSOLUTE_Y );    // RLA $hhll, Y
      sys.AddOpcodeForDisassembly( "nop", 0x4b, 0, AddressingType.IMMEDIATE );        // alr #$nn
      sys.AddOpcodeForDisassembly( "nop", 0x5b, 0, AddressingType.ABSOLUTE_Y );    // SRE $hhll, Y
      sys.AddOpcodeForDisassembly( "nop", 0x6b, 0, AddressingType.IMMEDIATE );        // arr #$nn
      sys.AddOpcodeForDisassembly( "nop", 0x7b, 0, AddressingType.ABSOLUTE_Y );    // RRA $hhll, Y
      sys.AddOpcodeForDisassembly( "nop", 0x8b, 0, AddressingType.IMMEDIATE );        // xaa #$nn
      sys.AddOpcodeForDisassembly( "nop", 0x9b, 0, AddressingType.ABSOLUTE_Y );    // tas $hhll, Y
      sys.AddOpcodeForDisassembly( "nop", 0xab, 0, AddressingType.IMMEDIATE );        // lax #$nn
      sys.AddOpcodeForDisassembly( "nop", 0xbb, 0, AddressingType.ABSOLUTE_Y );    // las $hhll, Y
      sys.AddOpcodeForDisassembly( "nop", 0xeb, 0, AddressingType.IMMEDIATE );        // sbc #$nn
      sys.AddOpcodeForDisassembly( "nop", 0xfb, 0, AddressingType.ABSOLUTE_Y );    // ISC $hhll, Y

      sys.AddOpcodeForDisassembly( "nop", 0x5C, 2, AddressingType.ABSOLUTE_X );    // NOP $abcd,x
      sys.AddOpcodeForDisassembly( "nop", 0xDC, 2, AddressingType.ABSOLUTE_X );    // NOP $abcd,x
      sys.AddOpcodeForDisassembly( "nop", 0xFC, 2, AddressingType.ABSOLUTE_X );    // NOP $abcd,x

      return sys;
    }



    public static Processor CreateR65C02()
    {
      // 65c02 plus BBRx, BBSx, RMBx, SMBx
      var  sys = new Processor( "R65C02" );

      sys.AddOpcode( "adc", 0x6d, 2, AddressingType.ABSOLUTE, 4, 1 );        // ADC $hhll
      sys.AddOpcode( "adc", 0x7d, 2, AddressingType.ABSOLUTE_X, 4, 1, 1, 0 );// ADC $hhll, X
      sys.AddOpcode( "adc", 0x79, 2, AddressingType.ABSOLUTE_Y, 4, 1, 1, 0 );// ADC $hhll, Y
      sys.AddOpcode( "adc", 0x65, 1, AddressingType.ZEROPAGE, 3, 1, 1, 0 );  // ADC $ll
      sys.AddOpcode( "adc", 0x75, 1, AddressingType.ZEROPAGE_X, 4, 1 );      // ADC $ll, X
      sys.AddOpcode( "adc", 0x71, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1, 1, 0 );// ADC ($ll), Y
      sys.AddOpcode( "adc", 0x61, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6, 1 );      // ADC ($ll,X)
      sys.AddOpcode( "adc", 0x69, 1, AddressingType.IMMEDIATE, 2, 1 );       // ADC #$nn
      sys.AddOpcode( "and", 0x2d, 2, AddressingType.ABSOLUTE, 4 );           // AND $hhll
      sys.AddOpcode( "and", 0x3d, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // AND $hhll, X
      sys.AddOpcode( "and", 0x39, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // AND $hhll, Y
      sys.AddOpcode( "and", 0x25, 1, AddressingType.ZEROPAGE, 3 );           // AND $ll
      sys.AddOpcode( "and", 0x35, 1, AddressingType.ZEROPAGE_X, 4 );         // AND $ll, X
      sys.AddOpcode( "and", 0x31, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // AND ($ll), Y
      sys.AddOpcode( "and", 0x21, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // AND ($ll,X)
      sys.AddOpcode( "and", 0x29, 1, AddressingType.IMMEDIATE, 2 );          // AND #$nn
      sys.AddOpcode( "asl", 0x0a, 0, AddressingType.IMPLICIT, 2 );           // ASL
      sys.AddOpcode( "asl", 0x0e, 2, AddressingType.ABSOLUTE, 6 );           // ASL $hhll
      sys.AddOpcode( "asl", 0x1e, 2, AddressingType.ABSOLUTE_X, 6, 1 );      // ASL $hhll, X
      sys.AddOpcode( "asl", 0x06, 1, AddressingType.ZEROPAGE, 5 );           // ASL $ll
      sys.AddOpcode( "asl", 0x16, 1, AddressingType.ZEROPAGE_X, 6 );         // ASL $ll, X
      sys.AddOpcode( "bcc", 0x90, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BCC $hhll
      sys.AddOpcode( "bcs", 0xB0, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BCS $hhll
      sys.AddOpcode( "beq", 0xF0, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BEQ $hhll
      sys.AddOpcode( "bit", 0x2c, 2, AddressingType.ABSOLUTE, 4 );           // BIT $hhll
      sys.AddOpcode( "bit", 0x24, 1, AddressingType.ZEROPAGE, 3 );           // BIT $ll
      sys.AddOpcode( "bmi", 0x30, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BMI $hhll
      sys.AddOpcode( "bne", 0xD0, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BNE $hhll
      sys.AddOpcode( "bpl", 0x10, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BPL $hhll
      sys.AddOpcode( "brk", 0x00, 0, AddressingType.IMPLICIT, 7 );           // BRK
      sys.AddOpcode( "bvc", 0x50, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BVC $hhll
      sys.AddOpcode( "bvs", 0x70, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BVS $hhll
      sys.AddOpcode( "clc", 0x18, 0, AddressingType.IMPLICIT, 2 );           // CLC
      sys.AddOpcode( "cld", 0xD8, 0, AddressingType.IMPLICIT, 2 );           // CLD
      sys.AddOpcode( "cli", 0x58, 0, AddressingType.IMPLICIT, 2 );           // CLI
      sys.AddOpcode( "clv", 0xB8, 0, AddressingType.IMPLICIT, 2 );           // CLV
      sys.AddOpcode( "cmp", 0xCD, 2, AddressingType.ABSOLUTE, 4 );           // CMP $hhll
      sys.AddOpcode( "cmp", 0xDD, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // CMP $hhll, X
      sys.AddOpcode( "cmp", 0xD9, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // CMP $hhll, Y
      sys.AddOpcode( "cmp", 0xC5, 1, AddressingType.ZEROPAGE, 3 );           // CMP $ll
      sys.AddOpcode( "cmp", 0xD5, 1, AddressingType.ZEROPAGE_X, 4 );         // CMP $ll, X
      sys.AddOpcode( "cmp", 0xD1, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // CMP ($ll), Y
      sys.AddOpcode( "cmp", 0xC1, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // CMP ($ll,X)
      sys.AddOpcode( "cmp", 0xC9, 1, AddressingType.IMMEDIATE, 2 );          // CMP #$nn
      sys.AddOpcode( "cpx", 0xEC, 2, AddressingType.ABSOLUTE, 4 );           // CPX $hhll
      sys.AddOpcode( "cpx", 0xE4, 1, AddressingType.ZEROPAGE, 3 );           // CPX $ll
      sys.AddOpcode( "cpx", 0xE0, 1, AddressingType.IMMEDIATE, 2 );          // CPX #$nn
      sys.AddOpcode( "cpy", 0xCC, 2, AddressingType.ABSOLUTE, 4 );           // CPY $hhll
      sys.AddOpcode( "cpy", 0xC4, 1, AddressingType.ZEROPAGE, 3 );           // CPY $ll
      sys.AddOpcode( "cpy", 0xC0, 1, AddressingType.IMMEDIATE, 2 );          // CPY #$nn
      sys.AddOpcode( "dec", 0xCE, 2, AddressingType.ABSOLUTE, 6 );           // DEC $hhll
      sys.AddOpcode( "dec", 0xDE, 2, AddressingType.ABSOLUTE_X, 7 );         // DEC $hhll, X
      sys.AddOpcode( "dec", 0xC6, 1, AddressingType.ZEROPAGE, 5 );           // DEC $ll
      sys.AddOpcode( "dec", 0xD6, 1, AddressingType.ZEROPAGE_X, 6 );         // DEC $ll, X
      sys.AddOpcode( "dex", 0xCA, 0, AddressingType.IMPLICIT, 2 );           // DEX
      sys.AddOpcode( "dey", 0x88, 0, AddressingType.IMPLICIT, 2 );           // DEY
      sys.AddOpcode( "eor", 0x4D, 2, AddressingType.ABSOLUTE, 4 );           // EOR $hhll
      sys.AddOpcode( "eor", 0x5D, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // EOR $hhll, X
      sys.AddOpcode( "eor", 0x59, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // EOR $hhll, Y
      sys.AddOpcode( "eor", 0x45, 1, AddressingType.ZEROPAGE, 3 );           // EOR $ll
      sys.AddOpcode( "eor", 0x55, 1, AddressingType.ZEROPAGE_X, 4 );         // EOR $ll, X
      sys.AddOpcode( "eor", 0x51, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // EOR ($ll), Y
      sys.AddOpcode( "eor", 0x41, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // EOR ($ll,X)
      sys.AddOpcode( "eor", 0x49, 1, AddressingType.IMMEDIATE, 2 );          // EOR #$nn
      sys.AddOpcode( "inc", 0xEE, 2, AddressingType.ABSOLUTE, 6 );           // INC $hhll
      sys.AddOpcode( "inc", 0xFE, 2, AddressingType.ABSOLUTE_X, 7 );         // INC $hhll, X
      sys.AddOpcode( "inc", 0xE6, 1, AddressingType.ZEROPAGE, 5 );           // INC $ll
      sys.AddOpcode( "inc", 0xF6, 1, AddressingType.ZEROPAGE_X, 6 );         // INC $ll, X
      sys.AddOpcode( "inx", 0xE8, 0, AddressingType.IMPLICIT, 2 );           // INX
      sys.AddOpcode( "iny", 0xC8, 0, AddressingType.IMPLICIT, 2 );           // INY
      sys.AddOpcode( "jmp", 0x4C, 2, AddressingType.ABSOLUTE, 3 );           // JMP $hhll
      sys.AddOpcode( "jmp", 0x6C, 2, AddressingType.INDIRECT, 6 );           // JMP ($hhll)
      sys.AddOpcode( "jsr", 0x20, 2, AddressingType.ABSOLUTE, 6 );           // JSR $hhll
      sys.AddOpcode( "lda", 0xAD, 2, AddressingType.ABSOLUTE, 4 );           // LDA $hhll
      sys.AddOpcode( "lda", 0xBD, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // LDA $hhll, X
      sys.AddOpcode( "lda", 0xB9, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // LDA $hhll, Y
      sys.AddOpcode( "lda", 0xA5, 1, AddressingType.ZEROPAGE, 3 );           // LDA $ll
      sys.AddOpcode( "lda", 0xB5, 1, AddressingType.ZEROPAGE_X, 4 );         // LDA $ll, X
      sys.AddOpcode( "lda", 0xB1, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // LDA ($ll), Y
      sys.AddOpcode( "lda", 0xA1, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // LDA ($ll,X)
      sys.AddOpcode( "lda", 0xA9, 1, AddressingType.IMMEDIATE, 2 );          // LDA #$nn
      sys.AddOpcode( "ldx", 0xAE, 2, AddressingType.ABSOLUTE, 4 );           // LDX $hhll
      sys.AddOpcode( "ldx", 0xBE, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // LDX $hhll, Y
      sys.AddOpcode( "ldx", 0xA6, 1, AddressingType.ZEROPAGE, 3 );           // LDX $ll
      sys.AddOpcode( "ldx", 0xB6, 1, AddressingType.ZEROPAGE_Y, 4 );         // LDX $ll, Y
      sys.AddOpcode( "ldx", 0xA2, 1, AddressingType.IMMEDIATE, 2 );          // LDX #$nn
      sys.AddOpcode( "ldy", 0xAC, 2, AddressingType.ABSOLUTE, 4 );           // LDY $hhll
      sys.AddOpcode( "ldy", 0xBC, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // LDY $hhll, X
      sys.AddOpcode( "ldy", 0xA4, 1, AddressingType.ZEROPAGE, 3 );           // LDY $ll
      sys.AddOpcode( "ldy", 0xB4, 1, AddressingType.ZEROPAGE_X, 4 );         // LDY $ll, X
      sys.AddOpcode( "ldy", 0xA0, 1, AddressingType.IMMEDIATE, 2 );          // LDY #$nn
      sys.AddOpcode( "lsr", 0x4A, 0, AddressingType.IMPLICIT, 2 );           // LSR
      sys.AddOpcode( "lsr", 0x4E, 2, AddressingType.ABSOLUTE, 6 );           // LSR $hhll
      sys.AddOpcode( "lsr", 0x5E, 2, AddressingType.ABSOLUTE_X, 6, 1 );      // LSR $hhll, X
      sys.AddOpcode( "lsr", 0x46, 1, AddressingType.ZEROPAGE, 5 );           // LSR $ll
      sys.AddOpcode( "lsr", 0x56, 1, AddressingType.ZEROPAGE_X, 6 );         // LSR $ll, X
      sys.AddOpcode( "nop", 0xEA, 0, AddressingType.IMPLICIT, 2 );           // NOP
      sys.AddOpcode( "ora", 0x0D, 2, AddressingType.ABSOLUTE, 4 );           // ORA $hhll
      sys.AddOpcode( "ora", 0x1D, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // ORA $hhll, X
      sys.AddOpcode( "ora", 0x19, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // ORA $hhll, Y
      sys.AddOpcode( "ora", 0x05, 1, AddressingType.ZEROPAGE, 3 );           // ORA $ll
      sys.AddOpcode( "ora", 0x15, 1, AddressingType.ZEROPAGE_X, 4 );         // ORA $ll, X
      sys.AddOpcode( "ora", 0x11, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // ORA ($ll), Y
      sys.AddOpcode( "ora", 0x01, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // ORA ($ll,X)
      sys.AddOpcode( "ora", 0x09, 1, AddressingType.IMMEDIATE, 2 );          // ORA #$nn
      sys.AddOpcode( "pha", 0x48, 0, AddressingType.IMPLICIT, 3 );           // PHA
      sys.AddOpcode( "php", 0x08, 0, AddressingType.IMPLICIT, 3 );           // PHP
      sys.AddOpcode( "pla", 0x68, 0, AddressingType.IMPLICIT, 4 );           // PLA
      sys.AddOpcode( "plp", 0x28, 0, AddressingType.IMPLICIT, 4 );           // PLP
      sys.AddOpcode( "rol", 0x2A, 0, AddressingType.IMPLICIT, 2 );           // ROL
      sys.AddOpcode( "rol", 0x2E, 2, AddressingType.ABSOLUTE, 6 );           // ROL $hhll
      sys.AddOpcode( "rol", 0x3E, 2, AddressingType.ABSOLUTE_X, 6, 1 );      // ROL $hhll, X
      sys.AddOpcode( "rol", 0x26, 1, AddressingType.ZEROPAGE, 5 );           // ROL $ll
      sys.AddOpcode( "rol", 0x36, 1, AddressingType.ZEROPAGE_X, 6 );         // ROL $ll, X
      sys.AddOpcode( "ror", 0x6A, 0, AddressingType.IMPLICIT, 2 );           // ROR
      sys.AddOpcode( "ror", 0x6E, 2, AddressingType.ABSOLUTE, 6 );           // ROR $hhll
      sys.AddOpcode( "ror", 0x7E, 2, AddressingType.ABSOLUTE_X, 6, 1 );      // ROR $hhll, X
      sys.AddOpcode( "ror", 0x66, 1, AddressingType.ZEROPAGE, 5 );           // ROR $ll
      sys.AddOpcode( "ror", 0x76, 1, AddressingType.ZEROPAGE_X, 6 );         // ROR $ll, X
      sys.AddOpcode( "rti", 0x40, 0, AddressingType.IMPLICIT, 6 );           // RTI
      sys.AddOpcode( "rts", 0x60, 0, AddressingType.IMPLICIT, 6 );           // RTS
      sys.AddOpcode( "sbc", 0xED, 2, AddressingType.ABSOLUTE, 4, 1 );        // SBC $hhll
      sys.AddOpcode( "sbc", 0xFD, 2, AddressingType.ABSOLUTE_X, 4, 1, 1, 0 );// SBC $hhll, X
      sys.AddOpcode( "sbc", 0xF9, 2, AddressingType.ABSOLUTE_Y, 4, 1, 1, 0 );// SBC $hhll, Y
      sys.AddOpcode( "sbc", 0xE5, 1, AddressingType.ZEROPAGE, 3, 1 );        // SBC $ll
      sys.AddOpcode( "sbc", 0xF5, 1, AddressingType.ZEROPAGE_X, 4, 1 );      // SBC $ll, X
      sys.AddOpcode( "sbc", 0xF1, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1, 1, 0 );// SBC ($ll), Y
      sys.AddOpcode( "sbc", 0xE1, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6, 1 );      // SBC ($ll,X)
      sys.AddOpcode( "sbc", 0xE9, 1, AddressingType.IMMEDIATE, 2, 1 );       // SBC #$nn
      sys.AddOpcode( "sec", 0x38, 0, AddressingType.IMPLICIT, 2 );           // SEC
      sys.AddOpcode( "sed", 0xF8, 0, AddressingType.IMPLICIT, 2 );           // SED
      sys.AddOpcode( "sei", 0x78, 0, AddressingType.IMPLICIT, 2 );           // SEI
      sys.AddOpcode( "sta", 0x8D, 2, AddressingType.ABSOLUTE, 4 );           // STA $hhll
      sys.AddOpcode( "sta", 0x9D, 2, AddressingType.ABSOLUTE_X, 5 );         // STA $hhll, X
      sys.AddOpcode( "sta", 0x99, 2, AddressingType.ABSOLUTE_Y, 5 );         // STA $hhll, Y
      sys.AddOpcode( "sta", 0x85, 1, AddressingType.ZEROPAGE, 3 );           // STA $ll
      sys.AddOpcode( "sta", 0x95, 1, AddressingType.ZEROPAGE_X, 4 );         // STA $ll, X
      sys.AddOpcode( "sta", 0x91, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 6 );         // STA ($ll), Y
      sys.AddOpcode( "sta", 0x81, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // STA ($ll,X)
      sys.AddOpcode( "stx", 0x8E, 2, AddressingType.ABSOLUTE, 4 );           // STX $hhll
      sys.AddOpcode( "stx", 0x86, 1, AddressingType.ZEROPAGE, 3 );           // STX $ll
      sys.AddOpcode( "stx", 0x96, 1, AddressingType.ZEROPAGE_Y, 4 );         // STX $ll, Y
      sys.AddOpcode( "sty", 0x8C, 2, AddressingType.ABSOLUTE, 4 );           // STY $hhll
      sys.AddOpcode( "sty", 0x84, 1, AddressingType.ZEROPAGE, 3 );           // STY $ll
      sys.AddOpcode( "sty", 0x94, 1, AddressingType.ZEROPAGE_X, 4 );         // STY $ll, X
      sys.AddOpcode( "tax", 0xAA, 0, AddressingType.IMPLICIT, 2 );           // TAX
      sys.AddOpcode( "tay", 0xA8, 0, AddressingType.IMPLICIT, 2 );           // TAY
      sys.AddOpcode( "tsx", 0xBA, 0, AddressingType.IMPLICIT, 2 );           // TSX
      sys.AddOpcode( "txa", 0x8A, 0, AddressingType.IMPLICIT, 2 );           // TXA
      sys.AddOpcode( "txs", 0x9A, 0, AddressingType.IMPLICIT, 2 );           // TXS
      sys.AddOpcode( "tya", 0x98, 0, AddressingType.IMPLICIT, 2 );           // TYA


      // pageboundary cycle add is WRONG, it's for decimal mode
      sys.AddOpcode( "adc", 0x72, 1, AddressingType.ZEROPAGE_INDIRECT, 5, 1 );   // adc ($12)
      sys.AddOpcode( "and", 0x32, 1, AddressingType.ZEROPAGE_INDIRECT, 5 );      // and ($12)
      sys.AddOpcode( "cmp", 0xd2, 1, AddressingType.ZEROPAGE_INDIRECT, 5 );      // cmp ($12)
      sys.AddOpcode( "eor", 0x52, 1, AddressingType.ZEROPAGE_INDIRECT, 5 );      // eor ($12)
      sys.AddOpcode( "lda", 0xb2, 1, AddressingType.ZEROPAGE_INDIRECT, 5 );      // lda ($12)
      sys.AddOpcode( "ora", 0x12, 1, AddressingType.ZEROPAGE_INDIRECT, 5 );      // ora ($12)
      sys.AddOpcode( "sbc", 0xf2, 1, AddressingType.ZEROPAGE_INDIRECT, 5, 1 );   // sbc ($12)
      sys.AddOpcode( "sta", 0x92, 1, AddressingType.ZEROPAGE_INDIRECT, 5 );      // sta ($12)

      sys.AddOpcode( "bit", 0x89, 1, AddressingType.IMMEDIATE, 2 );              // bit #$12
      sys.AddOpcode( "bit", 0x34, 1, AddressingType.ZEROPAGE_X, 2 );             // bit $12,x
      sys.AddOpcode( "bit", 0x3C, 2, AddressingType.ABSOLUTE_X, 4, 1 );          // bit $1234,x

      sys.AddOpcode( "dec", 0x3A, 0, AddressingType.IMPLICIT, 2 );               // dec
      sys.AddOpcode( "inc", 0x1A, 0, AddressingType.IMPLICIT, 2 );               // inc

      sys.AddOpcode( "jmp", 0x7C, 2, AddressingType.ABSOLUTE_INDIRECT_X, 6 );    // jmp ($1234,x)

      sys.AddOpcode( "bra", 0x80, 1, AddressingType.RELATIVE, 3, 1 );            // bra label

      sys.AddOpcode( "phx", 0xDA, 0, AddressingType.IMPLICIT, 3 );               // phx
      sys.AddOpcode( "phy", 0x5A, 0, AddressingType.IMPLICIT, 3 );               // phy
      sys.AddOpcode( "plx", 0xfA, 0, AddressingType.IMPLICIT, 4 );               // plx
      sys.AddOpcode( "ply", 0x7A, 0, AddressingType.IMPLICIT, 4 );               // ply

      sys.AddOpcode( "stz", 0x64, 1, AddressingType.ZEROPAGE, 3 );               // stz $12
      sys.AddOpcode( "stz", 0x74, 1, AddressingType.ZEROPAGE_X, 4 );             // stz $12,x
      sys.AddOpcode( "stz", 0x9c, 2, AddressingType.ABSOLUTE, 4 );               // stz $1234
      sys.AddOpcode( "stz", 0x9e, 2, AddressingType.ABSOLUTE_X, 5 );             // stz $1234,x

      sys.AddOpcode( "trb", 0x14, 1, AddressingType.ZEROPAGE, 5 );               // trb $12
      sys.AddOpcode( "trb", 0x1C, 2, AddressingType.ABSOLUTE, 6 );               // trb $1234

      sys.AddOpcode( "tsb", 0x04, 1, AddressingType.ZEROPAGE, 5 );               // tsb $12
      sys.AddOpcode( "tsb", 0x0C, 2, AddressingType.ABSOLUTE, 6 );               // tsb $1234

      sys.AddOpcode( "bbr0", 0x0f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr0 $12,LABEL
      sys.AddOpcode( "bbr1", 0x1f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr1 $12,LABEL
      sys.AddOpcode( "bbr2", 0x2f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr2 $12,LABEL
      sys.AddOpcode( "bbr3", 0x3f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr3 $12,LABEL
      sys.AddOpcode( "bbr4", 0x4f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr4 $12,LABEL
      sys.AddOpcode( "bbr5", 0x5f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr5 $12,LABEL
      sys.AddOpcode( "bbr6", 0x6f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr6 $12,LABEL
      sys.AddOpcode( "bbr7", 0x7f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr7 $12,LABEL
      sys.AddOpcode( "bbs0", 0x8f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs0 $12,LABEL
      sys.AddOpcode( "bbs1", 0x9f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs1 $12,LABEL
      sys.AddOpcode( "bbs2", 0xaf, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs2 $12,LABEL
      sys.AddOpcode( "bbs3", 0xbf, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs3 $12,LABEL
      sys.AddOpcode( "bbs4", 0xcf, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs4 $12,LABEL
      sys.AddOpcode( "bbs5", 0xdf, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs5 $12,LABEL
      sys.AddOpcode( "bbs6", 0xef, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs6 $12,LABEL
      sys.AddOpcode( "bbs7", 0xff, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs7 $12,LABEL

      sys.AddOpcode( "rmb0", 0x07, 1, AddressingType.ZEROPAGE, 5 );     // rmb0 $12
      sys.AddOpcode( "rmb1", 0x17, 1, AddressingType.ZEROPAGE, 5 );     // rmb1 $12
      sys.AddOpcode( "rmb2", 0x27, 1, AddressingType.ZEROPAGE, 5 );     // rmb2 $12
      sys.AddOpcode( "rmb3", 0x37, 1, AddressingType.ZEROPAGE, 5 );     // rmb3 $12
      sys.AddOpcode( "rmb4", 0x47, 1, AddressingType.ZEROPAGE, 5 );     // rmb4 $12
      sys.AddOpcode( "rmb5", 0x57, 1, AddressingType.ZEROPAGE, 5 );     // rmb5 $12
      sys.AddOpcode( "rmb6", 0x67, 1, AddressingType.ZEROPAGE, 5 );     // rmb6 $12
      sys.AddOpcode( "rmb7", 0x77, 1, AddressingType.ZEROPAGE, 5 );     // rmb7 $12
      sys.AddOpcode( "smb0", 0x87, 1, AddressingType.ZEROPAGE, 5 );     // smb0 $12
      sys.AddOpcode( "smb1", 0x97, 1, AddressingType.ZEROPAGE, 5 );     // smb1 $12
      sys.AddOpcode( "smb2", 0xa7, 1, AddressingType.ZEROPAGE, 5 );     // smb2 $12
      sys.AddOpcode( "smb3", 0xb7, 1, AddressingType.ZEROPAGE, 5 );     // smb3 $12
      sys.AddOpcode( "smb4", 0xc7, 1, AddressingType.ZEROPAGE, 5 );     // smb4 $12
      sys.AddOpcode( "smb5", 0xd7, 1, AddressingType.ZEROPAGE, 5 );     // smb5 $12
      sys.AddOpcode( "smb6", 0xe7, 1, AddressingType.ZEROPAGE, 5 );     // smb6 $12
      sys.AddOpcode( "smb7", 0xf7, 1, AddressingType.ZEROPAGE, 5 );     // smb7 $12

      sys.AddOpcodeForDisassembly( "nop", 0xdb, 0, AddressingType.IMPLICIT );               // STP

      sys.AddOpcodeForDisassembly( "nop", 0xcb, 0, AddressingType.IMPLICIT );               // wai

      sys.AddOpcodeForDisassembly( "nop", 0x02, 1, AddressingType.IMPLICIT );           // JAM
      sys.AddOpcodeForDisassembly( "nop", 0x22, 1, AddressingType.IMPLICIT );      // JAM
      sys.AddOpcodeForDisassembly( "nop", 0x42, 1, AddressingType.IMPLICIT );      // JAM
      sys.AddOpcodeForDisassembly( "nop", 0x62, 1, AddressingType.IMPLICIT );      // JAM
      sys.AddOpcodeForDisassembly( "nop", 0x82, 1, AddressingType.IMMEDIATE );        // NOP #xx
      sys.AddOpcodeForDisassembly( "nop", 0xC2, 1, AddressingType.IMMEDIATE );        // NOP #xx
      sys.AddOpcodeForDisassembly( "nop", 0xE2, 1, AddressingType.IMMEDIATE );        // NOP #xx

      sys.AddOpcodeForDisassembly( "nop", 0x03, 0, AddressingType.ZEROPAGE_INDIRECT_X );
      sys.AddOpcodeForDisassembly( "nop", 0x13, 0, AddressingType.ZEROPAGE_INDIRECT_Y );               // SLO ($ll), Y
      sys.AddOpcodeForDisassembly( "nop", 0x23, 0, AddressingType.ZEROPAGE_INDIRECT_X );    // RLA ($ll,X)
      sys.AddOpcodeForDisassembly( "nop", 0x33, 0, AddressingType.ZEROPAGE_INDIRECT_Y );    // RLA ($ll), Y
      sys.AddOpcodeForDisassembly( "nop", 0x43, 0, AddressingType.ZEROPAGE_INDIRECT_X );    // SRE ($ll,X)
      sys.AddOpcodeForDisassembly( "nop", 0x53, 0, AddressingType.ZEROPAGE_INDIRECT_Y );    // SRE ($ll), Y
      sys.AddOpcodeForDisassembly( "nop", 0x63, 0, AddressingType.ZEROPAGE_INDIRECT_X );    // RRA ($ll,X)
      sys.AddOpcodeForDisassembly( "nop", 0x73, 0, AddressingType.ZEROPAGE_INDIRECT_Y );    // RRA ($ll), Y
      sys.AddOpcodeForDisassembly( "nop", 0x83, 0, AddressingType.ZEROPAGE_INDIRECT_X );    // SAX ($ll,X)
      sys.AddOpcodeForDisassembly( "nop", 0x93, 0, AddressingType.ABSOLUTE_X );    // ahx $hhll, X
      sys.AddOpcodeForDisassembly( "nop", 0xa3, 0, AddressingType.ZEROPAGE_INDIRECT_X );    // LAX ($ll,X)
      sys.AddOpcodeForDisassembly( "nop", 0xb3, 0, AddressingType.ZEROPAGE_INDIRECT_Y ); // LAX ($ll), Y
      sys.AddOpcodeForDisassembly( "nop", 0xc3, 0, AddressingType.ZEROPAGE_INDIRECT_X );    // DCP ($ll,X)
      sys.AddOpcodeForDisassembly( "nop", 0xd3, 0, AddressingType.ZEROPAGE_INDIRECT_Y );    // DCP ($ll), Y
      sys.AddOpcodeForDisassembly( "nop", 0xe3, 0, AddressingType.ZEROPAGE_INDIRECT_X );    // ISC ($ll,X)
      sys.AddOpcodeForDisassembly( "nop", 0xf3, 0, AddressingType.ZEROPAGE_INDIRECT_Y );    // ISC ($ll), Y

      sys.AddOpcodeForDisassembly( "nop", 0x44, 1, AddressingType.ZEROPAGE );      // NOP $zp
      sys.AddOpcodeForDisassembly( "nop", 0x54, 1, AddressingType.ZEROPAGE_X );    // NOP $zp,x
      sys.AddOpcodeForDisassembly( "nop", 0xD4, 1, AddressingType.ZEROPAGE_X );    // NOP $zp,x
      sys.AddOpcodeForDisassembly( "nop", 0xF4, 1, AddressingType.ZEROPAGE_X );    // NOP $zp,x

      sys.AddOpcodeForDisassembly( "nop", 0x0b, 0, AddressingType.IMMEDIATE );        // anc #$nn
      sys.AddOpcodeForDisassembly( "nop", 0x1b, 0, AddressingType.ABSOLUTE_Y );               // SLO $hhll, Y
      sys.AddOpcodeForDisassembly( "nop", 0x2b, 0, AddressingType.IMMEDIATE );        // anc #$nn
      sys.AddOpcodeForDisassembly( "nop", 0x3b, 0, AddressingType.ABSOLUTE_Y );    // RLA $hhll, Y
      sys.AddOpcodeForDisassembly( "nop", 0x4b, 0, AddressingType.IMMEDIATE );        // alr #$nn
      sys.AddOpcodeForDisassembly( "nop", 0x5b, 0, AddressingType.ABSOLUTE_Y );    // SRE $hhll, Y
      sys.AddOpcodeForDisassembly( "nop", 0x6b, 0, AddressingType.IMMEDIATE );        // arr #$nn
      sys.AddOpcodeForDisassembly( "nop", 0x7b, 0, AddressingType.ABSOLUTE_Y );    // RRA $hhll, Y
      sys.AddOpcodeForDisassembly( "nop", 0x8b, 0, AddressingType.IMMEDIATE );        // xaa #$nn
      sys.AddOpcodeForDisassembly( "nop", 0x9b, 0, AddressingType.ABSOLUTE_Y );    // tas $hhll, Y
      sys.AddOpcodeForDisassembly( "nop", 0xab, 0, AddressingType.IMMEDIATE );        // lax #$nn
      sys.AddOpcodeForDisassembly( "nop", 0xbb, 0, AddressingType.ABSOLUTE_Y );    // las $hhll, Y
      sys.AddOpcodeForDisassembly( "nop", 0xeb, 0, AddressingType.IMMEDIATE );        // sbc #$nn
      sys.AddOpcodeForDisassembly( "nop", 0xfb, 0, AddressingType.ABSOLUTE_Y );    // ISC $hhll, Y

      sys.AddOpcodeForDisassembly( "nop", 0x5C, 2, AddressingType.ABSOLUTE_X );    // NOP $abcd,x
      sys.AddOpcodeForDisassembly( "nop", 0xDC, 2, AddressingType.ABSOLUTE_X );    // NOP $abcd,x
      sys.AddOpcodeForDisassembly( "nop", 0xFC, 2, AddressingType.ABSOLUTE_X );    // NOP $abcd,x

      return sys;
    }



    public static Processor CreateWDC65C02()
    {
      var  sys = new Processor( "WDC 65C02" );

      sys.AddOpcode( "adc", 0x6d, 2, AddressingType.ABSOLUTE, 4, 1 );        // ADC $hhll
      sys.AddOpcode( "adc", 0x7d, 2, AddressingType.ABSOLUTE_X, 4, 1, 1, 0 );// ADC $hhll, X
      sys.AddOpcode( "adc", 0x79, 2, AddressingType.ABSOLUTE_Y, 4, 1, 1, 0 );// ADC $hhll, Y
      sys.AddOpcode( "adc", 0x65, 1, AddressingType.ZEROPAGE, 3, 1, 1, 0 );  // ADC $ll
      sys.AddOpcode( "adc", 0x75, 1, AddressingType.ZEROPAGE_X, 4, 1 );      // ADC $ll, X
      sys.AddOpcode( "adc", 0x71, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1, 1, 0 );// ADC ($ll), Y
      sys.AddOpcode( "adc", 0x61, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6, 1 );      // ADC ($ll,X)
      sys.AddOpcode( "adc", 0x69, 1, AddressingType.IMMEDIATE, 2, 1 );       // ADC #$nn
      sys.AddOpcode( "and", 0x2d, 2, AddressingType.ABSOLUTE, 4 );           // AND $hhll
      sys.AddOpcode( "and", 0x3d, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // AND $hhll, X
      sys.AddOpcode( "and", 0x39, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // AND $hhll, Y
      sys.AddOpcode( "and", 0x25, 1, AddressingType.ZEROPAGE, 3 );           // AND $ll
      sys.AddOpcode( "and", 0x35, 1, AddressingType.ZEROPAGE_X, 4 );         // AND $ll, X
      sys.AddOpcode( "and", 0x31, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // AND ($ll), Y
      sys.AddOpcode( "and", 0x21, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // AND ($ll,X)
      sys.AddOpcode( "and", 0x29, 1, AddressingType.IMMEDIATE, 2 );          // AND #$nn
      sys.AddOpcode( "asl", 0x0a, 0, AddressingType.IMPLICIT, 2 );           // ASL
      sys.AddOpcode( "asl", 0x0e, 2, AddressingType.ABSOLUTE, 6 );           // ASL $hhll
      sys.AddOpcode( "asl", 0x1e, 2, AddressingType.ABSOLUTE_X, 6, 1 );      // ASL $hhll, X
      sys.AddOpcode( "asl", 0x06, 1, AddressingType.ZEROPAGE, 5 );           // ASL $ll
      sys.AddOpcode( "asl", 0x16, 1, AddressingType.ZEROPAGE_X, 6 );         // ASL $ll, X
      sys.AddOpcode( "bcc", 0x90, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BCC $hhll
      sys.AddOpcode( "bcs", 0xB0, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BCS $hhll
      sys.AddOpcode( "beq", 0xF0, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BEQ $hhll
      sys.AddOpcode( "bit", 0x2c, 2, AddressingType.ABSOLUTE, 4 );           // BIT $hhll
      sys.AddOpcode( "bit", 0x24, 1, AddressingType.ZEROPAGE, 3 );           // BIT $ll
      sys.AddOpcode( "bmi", 0x30, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BMI $hhll
      sys.AddOpcode( "bne", 0xD0, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BNE $hhll
      sys.AddOpcode( "bpl", 0x10, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BPL $hhll
      sys.AddOpcode( "brk", 0x00, 0, AddressingType.IMPLICIT, 7 );           // BRK
      sys.AddOpcode( "bvc", 0x50, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BVC $hhll
      sys.AddOpcode( "bvs", 0x70, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BVS $hhll
      sys.AddOpcode( "clc", 0x18, 0, AddressingType.IMPLICIT, 2 );           // CLC
      sys.AddOpcode( "cld", 0xD8, 0, AddressingType.IMPLICIT, 2 );           // CLD
      sys.AddOpcode( "cli", 0x58, 0, AddressingType.IMPLICIT, 2 );           // CLI
      sys.AddOpcode( "clv", 0xB8, 0, AddressingType.IMPLICIT, 2 );           // CLV
      sys.AddOpcode( "cmp", 0xCD, 2, AddressingType.ABSOLUTE, 4 );           // CMP $hhll
      sys.AddOpcode( "cmp", 0xDD, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // CMP $hhll, X
      sys.AddOpcode( "cmp", 0xD9, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // CMP $hhll, Y
      sys.AddOpcode( "cmp", 0xC5, 1, AddressingType.ZEROPAGE, 3 );           // CMP $ll
      sys.AddOpcode( "cmp", 0xD5, 1, AddressingType.ZEROPAGE_X, 4 );         // CMP $ll, X
      sys.AddOpcode( "cmp", 0xD1, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // CMP ($ll), Y
      sys.AddOpcode( "cmp", 0xC1, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // CMP ($ll,X)
      sys.AddOpcode( "cmp", 0xC9, 1, AddressingType.IMMEDIATE, 2 );          // CMP #$nn
      sys.AddOpcode( "cpx", 0xEC, 2, AddressingType.ABSOLUTE, 4 );           // CPX $hhll
      sys.AddOpcode( "cpx", 0xE4, 1, AddressingType.ZEROPAGE, 3 );           // CPX $ll
      sys.AddOpcode( "cpx", 0xE0, 1, AddressingType.IMMEDIATE, 2 );          // CPX #$nn
      sys.AddOpcode( "cpy", 0xCC, 2, AddressingType.ABSOLUTE, 4 );           // CPY $hhll
      sys.AddOpcode( "cpy", 0xC4, 1, AddressingType.ZEROPAGE, 3 );           // CPY $ll
      sys.AddOpcode( "cpy", 0xC0, 1, AddressingType.IMMEDIATE, 2 );          // CPY #$nn
      sys.AddOpcode( "dec", 0xCE, 2, AddressingType.ABSOLUTE, 6 );           // DEC $hhll
      sys.AddOpcode( "dec", 0xDE, 2, AddressingType.ABSOLUTE_X, 7 );         // DEC $hhll, X
      sys.AddOpcode( "dec", 0xC6, 1, AddressingType.ZEROPAGE, 5 );           // DEC $ll
      sys.AddOpcode( "dec", 0xD6, 1, AddressingType.ZEROPAGE_X, 6 );         // DEC $ll, X
      sys.AddOpcode( "dex", 0xCA, 0, AddressingType.IMPLICIT, 2 );           // DEX
      sys.AddOpcode( "dey", 0x88, 0, AddressingType.IMPLICIT, 2 );           // DEY
      sys.AddOpcode( "eor", 0x4D, 2, AddressingType.ABSOLUTE, 4 );           // EOR $hhll
      sys.AddOpcode( "eor", 0x5D, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // EOR $hhll, X
      sys.AddOpcode( "eor", 0x59, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // EOR $hhll, Y
      sys.AddOpcode( "eor", 0x45, 1, AddressingType.ZEROPAGE, 3 );           // EOR $ll
      sys.AddOpcode( "eor", 0x55, 1, AddressingType.ZEROPAGE_X, 4 );         // EOR $ll, X
      sys.AddOpcode( "eor", 0x51, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // EOR ($ll), Y
      sys.AddOpcode( "eor", 0x41, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // EOR ($ll,X)
      sys.AddOpcode( "eor", 0x49, 1, AddressingType.IMMEDIATE, 2 );          // EOR #$nn
      sys.AddOpcode( "inc", 0xEE, 2, AddressingType.ABSOLUTE, 6 );           // INC $hhll
      sys.AddOpcode( "inc", 0xFE, 2, AddressingType.ABSOLUTE_X, 7 );         // INC $hhll, X
      sys.AddOpcode( "inc", 0xE6, 1, AddressingType.ZEROPAGE, 5 );           // INC $ll
      sys.AddOpcode( "inc", 0xF6, 1, AddressingType.ZEROPAGE_X, 6 );         // INC $ll, X
      sys.AddOpcode( "inx", 0xE8, 0, AddressingType.IMPLICIT, 2 );           // INX
      sys.AddOpcode( "iny", 0xC8, 0, AddressingType.IMPLICIT, 2 );           // INY
      sys.AddOpcode( "jmp", 0x4C, 2, AddressingType.ABSOLUTE, 3 );           // JMP $hhll
      sys.AddOpcode( "jmp", 0x6C, 2, AddressingType.INDIRECT, 6 );           // JMP ($hhll)
      sys.AddOpcode( "jsr", 0x20, 2, AddressingType.ABSOLUTE, 6 );           // JSR $hhll
      sys.AddOpcode( "lda", 0xAD, 2, AddressingType.ABSOLUTE, 4 );           // LDA $hhll
      sys.AddOpcode( "lda", 0xBD, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // LDA $hhll, X
      sys.AddOpcode( "lda", 0xB9, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // LDA $hhll, Y
      sys.AddOpcode( "lda", 0xA5, 1, AddressingType.ZEROPAGE, 3 );           // LDA $ll
      sys.AddOpcode( "lda", 0xB5, 1, AddressingType.ZEROPAGE_X, 4 );         // LDA $ll, X
      sys.AddOpcode( "lda", 0xB1, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // LDA ($ll), Y
      sys.AddOpcode( "lda", 0xA1, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // LDA ($ll,X)
      sys.AddOpcode( "lda", 0xA9, 1, AddressingType.IMMEDIATE, 2 );          // LDA #$nn
      sys.AddOpcode( "ldx", 0xAE, 2, AddressingType.ABSOLUTE, 4 );           // LDX $hhll
      sys.AddOpcode( "ldx", 0xBE, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // LDX $hhll, Y
      sys.AddOpcode( "ldx", 0xA6, 1, AddressingType.ZEROPAGE, 3 );           // LDX $ll
      sys.AddOpcode( "ldx", 0xB6, 1, AddressingType.ZEROPAGE_Y, 4 );         // LDX $ll, Y
      sys.AddOpcode( "ldx", 0xA2, 1, AddressingType.IMMEDIATE, 2 );          // LDX #$nn
      sys.AddOpcode( "ldy", 0xAC, 2, AddressingType.ABSOLUTE, 4 );           // LDY $hhll
      sys.AddOpcode( "ldy", 0xBC, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // LDY $hhll, X
      sys.AddOpcode( "ldy", 0xA4, 1, AddressingType.ZEROPAGE, 3 );           // LDY $ll
      sys.AddOpcode( "ldy", 0xB4, 1, AddressingType.ZEROPAGE_X, 4 );         // LDY $ll, X
      sys.AddOpcode( "ldy", 0xA0, 1, AddressingType.IMMEDIATE, 2 );          // LDY #$nn
      sys.AddOpcode( "lsr", 0x4A, 0, AddressingType.IMPLICIT, 2 );           // LSR
      sys.AddOpcode( "lsr", 0x4E, 2, AddressingType.ABSOLUTE, 6 );           // LSR $hhll
      sys.AddOpcode( "lsr", 0x5E, 2, AddressingType.ABSOLUTE_X, 6, 1 );      // LSR $hhll, X
      sys.AddOpcode( "lsr", 0x46, 1, AddressingType.ZEROPAGE, 5 );           // LSR $ll
      sys.AddOpcode( "lsr", 0x56, 1, AddressingType.ZEROPAGE_X, 6 );         // LSR $ll, X
      sys.AddOpcode( "nop", 0xEA, 0, AddressingType.IMPLICIT, 2 );           // NOP
      sys.AddOpcode( "ora", 0x0D, 2, AddressingType.ABSOLUTE, 4 );           // ORA $hhll
      sys.AddOpcode( "ora", 0x1D, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // ORA $hhll, X
      sys.AddOpcode( "ora", 0x19, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // ORA $hhll, Y
      sys.AddOpcode( "ora", 0x05, 1, AddressingType.ZEROPAGE, 3 );           // ORA $ll
      sys.AddOpcode( "ora", 0x15, 1, AddressingType.ZEROPAGE_X, 4 );         // ORA $ll, X
      sys.AddOpcode( "ora", 0x11, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // ORA ($ll), Y
      sys.AddOpcode( "ora", 0x01, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // ORA ($ll,X)
      sys.AddOpcode( "ora", 0x09, 1, AddressingType.IMMEDIATE, 2 );          // ORA #$nn
      sys.AddOpcode( "pha", 0x48, 0, AddressingType.IMPLICIT, 3 );           // PHA
      sys.AddOpcode( "php", 0x08, 0, AddressingType.IMPLICIT, 3 );           // PHP
      sys.AddOpcode( "pla", 0x68, 0, AddressingType.IMPLICIT, 4 );           // PLA
      sys.AddOpcode( "plp", 0x28, 0, AddressingType.IMPLICIT, 4 );           // PLP
      sys.AddOpcode( "rol", 0x2A, 0, AddressingType.IMPLICIT, 2 );           // ROL
      sys.AddOpcode( "rol", 0x2E, 2, AddressingType.ABSOLUTE, 6 );           // ROL $hhll
      sys.AddOpcode( "rol", 0x3E, 2, AddressingType.ABSOLUTE_X, 6, 1 );      // ROL $hhll, X
      sys.AddOpcode( "rol", 0x26, 1, AddressingType.ZEROPAGE, 5 );           // ROL $ll
      sys.AddOpcode( "rol", 0x36, 1, AddressingType.ZEROPAGE_X, 6 );         // ROL $ll, X
      sys.AddOpcode( "ror", 0x6A, 0, AddressingType.IMPLICIT, 2 );           // ROR
      sys.AddOpcode( "ror", 0x6E, 2, AddressingType.ABSOLUTE, 6 );           // ROR $hhll
      sys.AddOpcode( "ror", 0x7E, 2, AddressingType.ABSOLUTE_X, 6, 1 );      // ROR $hhll, X
      sys.AddOpcode( "ror", 0x66, 1, AddressingType.ZEROPAGE, 5 );           // ROR $ll
      sys.AddOpcode( "ror", 0x76, 1, AddressingType.ZEROPAGE_X, 6 );         // ROR $ll, X
      sys.AddOpcode( "rti", 0x40, 0, AddressingType.IMPLICIT, 6 );           // RTI
      sys.AddOpcode( "rts", 0x60, 0, AddressingType.IMPLICIT, 6 );           // RTS
      sys.AddOpcode( "sbc", 0xED, 2, AddressingType.ABSOLUTE, 4, 1 );        // SBC $hhll
      sys.AddOpcode( "sbc", 0xFD, 2, AddressingType.ABSOLUTE_X, 4, 1, 1, 0 );// SBC $hhll, X
      sys.AddOpcode( "sbc", 0xF9, 2, AddressingType.ABSOLUTE_Y, 4, 1, 1, 0 );// SBC $hhll, Y
      sys.AddOpcode( "sbc", 0xE5, 1, AddressingType.ZEROPAGE, 3, 1 );        // SBC $ll
      sys.AddOpcode( "sbc", 0xF5, 1, AddressingType.ZEROPAGE_X, 4, 1 );      // SBC $ll, X
      sys.AddOpcode( "sbc", 0xF1, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1, 1, 0 );// SBC ($ll), Y
      sys.AddOpcode( "sbc", 0xE1, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6, 1 );      // SBC ($ll,X)
      sys.AddOpcode( "sbc", 0xE9, 1, AddressingType.IMMEDIATE, 2, 1 );       // SBC #$nn
      sys.AddOpcode( "sec", 0x38, 0, AddressingType.IMPLICIT, 2 );           // SEC
      sys.AddOpcode( "sed", 0xF8, 0, AddressingType.IMPLICIT, 2 );           // SED
      sys.AddOpcode( "sei", 0x78, 0, AddressingType.IMPLICIT, 2 );           // SEI
      sys.AddOpcode( "sta", 0x8D, 2, AddressingType.ABSOLUTE, 4 );           // STA $hhll
      sys.AddOpcode( "sta", 0x9D, 2, AddressingType.ABSOLUTE_X, 5 );         // STA $hhll, X
      sys.AddOpcode( "sta", 0x99, 2, AddressingType.ABSOLUTE_Y, 5 );         // STA $hhll, Y
      sys.AddOpcode( "sta", 0x85, 1, AddressingType.ZEROPAGE, 3 );           // STA $ll
      sys.AddOpcode( "sta", 0x95, 1, AddressingType.ZEROPAGE_X, 4 );         // STA $ll, X
      sys.AddOpcode( "sta", 0x91, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 6 );         // STA ($ll), Y
      sys.AddOpcode( "sta", 0x81, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // STA ($ll,X)
      sys.AddOpcode( "stx", 0x8E, 2, AddressingType.ABSOLUTE, 4 );           // STX $hhll
      sys.AddOpcode( "stx", 0x86, 1, AddressingType.ZEROPAGE, 3 );           // STX $ll
      sys.AddOpcode( "stx", 0x96, 1, AddressingType.ZEROPAGE_Y, 4 );         // STX $ll, Y
      sys.AddOpcode( "sty", 0x8C, 2, AddressingType.ABSOLUTE, 4 );           // STY $hhll
      sys.AddOpcode( "sty", 0x84, 1, AddressingType.ZEROPAGE, 3 );           // STY $ll
      sys.AddOpcode( "sty", 0x94, 1, AddressingType.ZEROPAGE_X, 4 );         // STY $ll, X
      sys.AddOpcode( "tax", 0xAA, 0, AddressingType.IMPLICIT, 2 );           // TAX
      sys.AddOpcode( "tay", 0xA8, 0, AddressingType.IMPLICIT, 2 );           // TAY
      sys.AddOpcode( "tsx", 0xBA, 0, AddressingType.IMPLICIT, 2 );           // TSX
      sys.AddOpcode( "txa", 0x8A, 0, AddressingType.IMPLICIT, 2 );           // TXA
      sys.AddOpcode( "txs", 0x9A, 0, AddressingType.IMPLICIT, 2 );           // TXS
      sys.AddOpcode( "tya", 0x98, 0, AddressingType.IMPLICIT, 2 );           // TYA


      // pageboundary cycle add is WRONG, it's for decimal mode
      sys.AddOpcode( "adc", 0x72, 1, AddressingType.ZEROPAGE_INDIRECT, 5, 1 );   // adc ($12)
      sys.AddOpcode( "and", 0x32, 1, AddressingType.ZEROPAGE_INDIRECT, 5 );      // and ($12)
      sys.AddOpcode( "cmp", 0xd2, 1, AddressingType.ZEROPAGE_INDIRECT, 5 );      // cmp ($12)
      sys.AddOpcode( "eor", 0x52, 1, AddressingType.ZEROPAGE_INDIRECT, 5 );      // eor ($12)
      sys.AddOpcode( "lda", 0xb2, 1, AddressingType.ZEROPAGE_INDIRECT, 5 );      // lda ($12)
      sys.AddOpcode( "ora", 0x12, 1, AddressingType.ZEROPAGE_INDIRECT, 5 );      // ora ($12)
      sys.AddOpcode( "sbc", 0xf2, 1, AddressingType.ZEROPAGE_INDIRECT, 5, 1 );   // sbc ($12)
      sys.AddOpcode( "sta", 0x92, 1, AddressingType.ZEROPAGE_INDIRECT, 5 );      // sta ($12)

      sys.AddOpcode( "bit", 0x89, 1, AddressingType.IMMEDIATE, 2 );              // bit #$12
      sys.AddOpcode( "bit", 0x34, 1, AddressingType.ZEROPAGE_X, 2 );             // bit $12,x
      sys.AddOpcode( "bit", 0x3C, 2, AddressingType.ABSOLUTE_X, 4, 1 );          // bit $1234,x

      sys.AddOpcode( "dec", 0x3A, 0, AddressingType.IMPLICIT, 2 );               // dec
      sys.AddOpcode( "inc", 0x1A, 0, AddressingType.IMPLICIT, 2 );               // inc

      sys.AddOpcode( "jmp", 0x7C, 2, AddressingType.ABSOLUTE_INDIRECT_X, 6 );    // jmp ($1234,x)

      sys.AddOpcode( "bra", 0x80, 1, AddressingType.RELATIVE, 3, 1 );            // bra label

      sys.AddOpcode( "phx", 0xDA, 0, AddressingType.IMPLICIT, 3 );               // phx
      sys.AddOpcode( "phy", 0x5A, 0, AddressingType.IMPLICIT, 3 );               // phy
      sys.AddOpcode( "plx", 0xfA, 0, AddressingType.IMPLICIT, 4 );               // plx
      sys.AddOpcode( "ply", 0x7A, 0, AddressingType.IMPLICIT, 4 );               // ply

      sys.AddOpcode( "stz", 0x64, 1, AddressingType.ZEROPAGE, 3 );               // stz $12
      sys.AddOpcode( "stz", 0x74, 1, AddressingType.ZEROPAGE_X, 4 );             // stz $12,x
      sys.AddOpcode( "stz", 0x9c, 2, AddressingType.ABSOLUTE, 4 );               // stz $1234
      sys.AddOpcode( "stz", 0x9e, 2, AddressingType.ABSOLUTE_X, 5 );             // stz $1234,x

      sys.AddOpcode( "trb", 0x14, 1, AddressingType.ZEROPAGE, 5 );               // trb $12
      sys.AddOpcode( "trb", 0x1C, 2, AddressingType.ABSOLUTE, 6 );               // trb $1234

      sys.AddOpcode( "tsb", 0x04, 1, AddressingType.ZEROPAGE, 5 );               // tsb $12
      sys.AddOpcode( "tsb", 0x0C, 2, AddressingType.ABSOLUTE, 6 );               // tsb $1234

      sys.AddOpcode( "bbr0", 0x0f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr0 $12,LABEL
      sys.AddOpcode( "bbr1", 0x1f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr1 $12,LABEL
      sys.AddOpcode( "bbr2", 0x2f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr2 $12,LABEL
      sys.AddOpcode( "bbr3", 0x3f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr3 $12,LABEL
      sys.AddOpcode( "bbr4", 0x4f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr4 $12,LABEL
      sys.AddOpcode( "bbr5", 0x5f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr5 $12,LABEL
      sys.AddOpcode( "bbr6", 0x6f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr6 $12,LABEL
      sys.AddOpcode( "bbr7", 0x7f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr7 $12,LABEL
      sys.AddOpcode( "bbs0", 0x8f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs0 $12,LABEL
      sys.AddOpcode( "bbs1", 0x9f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs1 $12,LABEL
      sys.AddOpcode( "bbs2", 0xaf, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs2 $12,LABEL
      sys.AddOpcode( "bbs3", 0xbf, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs3 $12,LABEL
      sys.AddOpcode( "bbs4", 0xcf, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs4 $12,LABEL
      sys.AddOpcode( "bbs5", 0xdf, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs5 $12,LABEL
      sys.AddOpcode( "bbs6", 0xef, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs6 $12,LABEL
      sys.AddOpcode( "bbs7", 0xff, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs7 $12,LABEL

      sys.AddOpcode( "rmb0", 0x07, 1, AddressingType.ZEROPAGE, 5 );     // rmb0 $12
      sys.AddOpcode( "rmb1", 0x17, 1, AddressingType.ZEROPAGE, 5 );     // rmb1 $12
      sys.AddOpcode( "rmb2", 0x27, 1, AddressingType.ZEROPAGE, 5 );     // rmb2 $12
      sys.AddOpcode( "rmb3", 0x37, 1, AddressingType.ZEROPAGE, 5 );     // rmb3 $12
      sys.AddOpcode( "rmb4", 0x47, 1, AddressingType.ZEROPAGE, 5 );     // rmb4 $12
      sys.AddOpcode( "rmb5", 0x57, 1, AddressingType.ZEROPAGE, 5 );     // rmb5 $12
      sys.AddOpcode( "rmb6", 0x67, 1, AddressingType.ZEROPAGE, 5 );     // rmb6 $12
      sys.AddOpcode( "rmb7", 0x77, 1, AddressingType.ZEROPAGE, 5 );     // rmb7 $12
      sys.AddOpcode( "smb0", 0x87, 1, AddressingType.ZEROPAGE, 5 );     // smb0 $12
      sys.AddOpcode( "smb1", 0x97, 1, AddressingType.ZEROPAGE, 5 );     // smb1 $12
      sys.AddOpcode( "smb2", 0xa7, 1, AddressingType.ZEROPAGE, 5 );     // smb2 $12
      sys.AddOpcode( "smb3", 0xb7, 1, AddressingType.ZEROPAGE, 5 );     // smb3 $12
      sys.AddOpcode( "smb4", 0xc7, 1, AddressingType.ZEROPAGE, 5 );     // smb4 $12
      sys.AddOpcode( "smb5", 0xd7, 1, AddressingType.ZEROPAGE, 5 );     // smb5 $12
      sys.AddOpcode( "smb6", 0xe7, 1, AddressingType.ZEROPAGE, 5 );     // smb6 $12
      sys.AddOpcode( "smb7", 0xf7, 1, AddressingType.ZEROPAGE, 5 );     // smb7 $12

      sys.AddOpcode( "stp", 0xdb, 0, AddressingType.IMPLICIT, 3 );               // STP

      sys.AddOpcode( "wai", 0xcb, 0, AddressingType.IMPLICIT, 3 );               // wai

      sys.AddOpcodeForDisassembly( "nop", 0x02, 1, AddressingType.IMPLICIT );           // JAM
      sys.AddOpcodeForDisassembly( "nop", 0x22, 1, AddressingType.IMPLICIT );      // JAM
      sys.AddOpcodeForDisassembly( "nop", 0x42, 1, AddressingType.IMPLICIT );      // JAM
      sys.AddOpcodeForDisassembly( "nop", 0x62, 1, AddressingType.IMPLICIT );      // JAM
      sys.AddOpcodeForDisassembly( "nop", 0x82, 1, AddressingType.IMMEDIATE );        // NOP #xx
      sys.AddOpcodeForDisassembly( "nop", 0xC2, 1, AddressingType.IMMEDIATE );        // NOP #xx
      sys.AddOpcodeForDisassembly( "nop", 0xE2, 1, AddressingType.IMMEDIATE );        // NOP #xx

      sys.AddOpcodeForDisassembly( "nop", 0x03, 0, AddressingType.ZEROPAGE_INDIRECT_X );
      sys.AddOpcodeForDisassembly( "nop", 0x13, 0, AddressingType.ZEROPAGE_INDIRECT_Y );               // SLO ($ll), Y
      sys.AddOpcodeForDisassembly( "nop", 0x23, 0, AddressingType.ZEROPAGE_INDIRECT_X );    // RLA ($ll,X)
      sys.AddOpcodeForDisassembly( "nop", 0x33, 0, AddressingType.ZEROPAGE_INDIRECT_Y );    // RLA ($ll), Y
      sys.AddOpcodeForDisassembly( "nop", 0x43, 0, AddressingType.ZEROPAGE_INDIRECT_X );    // SRE ($ll,X)
      sys.AddOpcodeForDisassembly( "nop", 0x53, 0, AddressingType.ZEROPAGE_INDIRECT_Y );    // SRE ($ll), Y
      sys.AddOpcodeForDisassembly( "nop", 0x63, 0, AddressingType.ZEROPAGE_INDIRECT_X );    // RRA ($ll,X)
      sys.AddOpcodeForDisassembly( "nop", 0x73, 0, AddressingType.ZEROPAGE_INDIRECT_Y );    // RRA ($ll), Y
      sys.AddOpcodeForDisassembly( "nop", 0x83, 0, AddressingType.ZEROPAGE_INDIRECT_X );    // SAX ($ll,X)
      sys.AddOpcodeForDisassembly( "nop", 0x93, 0, AddressingType.ABSOLUTE_X );    // ahx $hhll, X
      sys.AddOpcodeForDisassembly( "nop", 0xa3, 0, AddressingType.ZEROPAGE_INDIRECT_X );    // LAX ($ll,X)
      sys.AddOpcodeForDisassembly( "nop", 0xb3, 0, AddressingType.ZEROPAGE_INDIRECT_Y ); // LAX ($ll), Y
      sys.AddOpcodeForDisassembly( "nop", 0xc3, 0, AddressingType.ZEROPAGE_INDIRECT_X );    // DCP ($ll,X)
      sys.AddOpcodeForDisassembly( "nop", 0xd3, 0, AddressingType.ZEROPAGE_INDIRECT_Y );    // DCP ($ll), Y
      sys.AddOpcodeForDisassembly( "nop", 0xe3, 0, AddressingType.ZEROPAGE_INDIRECT_X );    // ISC ($ll,X)
      sys.AddOpcodeForDisassembly( "nop", 0xf3, 0, AddressingType.ZEROPAGE_INDIRECT_Y );    // ISC ($ll), Y

      sys.AddOpcodeForDisassembly( "nop", 0x44, 1, AddressingType.ZEROPAGE );      // NOP $zp
      sys.AddOpcodeForDisassembly( "nop", 0x54, 1, AddressingType.ZEROPAGE_X );    // NOP $zp,x
      sys.AddOpcodeForDisassembly( "nop", 0xD4, 1, AddressingType.ZEROPAGE_X );    // NOP $zp,x
      sys.AddOpcodeForDisassembly( "nop", 0xF4, 1, AddressingType.ZEROPAGE_X );    // NOP $zp,x

      sys.AddOpcodeForDisassembly( "nop", 0x0b, 0, AddressingType.IMMEDIATE );        // anc #$nn
      sys.AddOpcodeForDisassembly( "nop", 0x1b, 0, AddressingType.ABSOLUTE_Y );               // SLO $hhll, Y
      sys.AddOpcodeForDisassembly( "nop", 0x2b, 0, AddressingType.IMMEDIATE );        // anc #$nn
      sys.AddOpcodeForDisassembly( "nop", 0x3b, 0, AddressingType.ABSOLUTE_Y );    // RLA $hhll, Y
      sys.AddOpcodeForDisassembly( "nop", 0x4b, 0, AddressingType.IMMEDIATE );        // alr #$nn
      sys.AddOpcodeForDisassembly( "nop", 0x5b, 0, AddressingType.ABSOLUTE_Y );    // SRE $hhll, Y
      sys.AddOpcodeForDisassembly( "nop", 0x6b, 0, AddressingType.IMMEDIATE );        // arr #$nn
      sys.AddOpcodeForDisassembly( "nop", 0x7b, 0, AddressingType.ABSOLUTE_Y );    // RRA $hhll, Y
      sys.AddOpcodeForDisassembly( "nop", 0x8b, 0, AddressingType.IMMEDIATE );        // xaa #$nn
      sys.AddOpcodeForDisassembly( "nop", 0x9b, 0, AddressingType.ABSOLUTE_Y );    // tas $hhll, Y
      sys.AddOpcodeForDisassembly( "nop", 0xab, 0, AddressingType.IMMEDIATE );        // lax #$nn
      sys.AddOpcodeForDisassembly( "nop", 0xbb, 0, AddressingType.ABSOLUTE_Y );    // las $hhll, Y
      sys.AddOpcodeForDisassembly( "nop", 0xeb, 0, AddressingType.IMMEDIATE );        // sbc #$nn
      sys.AddOpcodeForDisassembly( "nop", 0xfb, 0, AddressingType.ABSOLUTE_Y );    // ISC $hhll, Y

      sys.AddOpcodeForDisassembly( "nop", 0x5C, 2, AddressingType.ABSOLUTE_X );    // NOP $abcd,x
      sys.AddOpcodeForDisassembly( "nop", 0xDC, 2, AddressingType.ABSOLUTE_X );    // NOP $abcd,x
      sys.AddOpcodeForDisassembly( "nop", 0xFC, 2, AddressingType.ABSOLUTE_X );    // NOP $abcd,x
      
      return sys;
    }



    public static Processor Create65CE02()
    {
      var  sys = new Processor( "65CE02" );

      sys.AddOpcode( "adc", 0x6d, 2, AddressingType.ABSOLUTE, 4, 1 );        // ADC $hhll
      sys.AddOpcode( "adc", 0x7d, 2, AddressingType.ABSOLUTE_X, 4, 1, 1, 0 );// ADC $hhll, X
      sys.AddOpcode( "adc", 0x79, 2, AddressingType.ABSOLUTE_Y, 4, 1, 1, 0 );// ADC $hhll, Y
      sys.AddOpcode( "adc", 0x65, 1, AddressingType.ZEROPAGE, 3, 1, 1, 0 );  // ADC $ll
      sys.AddOpcode( "adc", 0x75, 1, AddressingType.ZEROPAGE_X, 4, 1 );      // ADC $ll, X
      sys.AddOpcode( "adc", 0x71, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1, 1, 0 );// ADC ($ll), Y
      sys.AddOpcode( "adc", 0x61, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6, 1 );      // ADC ($ll,X)
      sys.AddOpcode( "adc", 0x69, 1, AddressingType.IMMEDIATE, 2, 1 );       // ADC #$nn
      sys.AddOpcode( "and", 0x2d, 2, AddressingType.ABSOLUTE, 4 );           // AND $hhll
      sys.AddOpcode( "and", 0x3d, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // AND $hhll, X
      sys.AddOpcode( "and", 0x39, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // AND $hhll, Y
      sys.AddOpcode( "and", 0x25, 1, AddressingType.ZEROPAGE, 3 );           // AND $ll
      sys.AddOpcode( "and", 0x35, 1, AddressingType.ZEROPAGE_X, 4 );         // AND $ll, X
      sys.AddOpcode( "and", 0x31, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // AND ($ll), Y
      sys.AddOpcode( "and", 0x21, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // AND ($ll,X)
      sys.AddOpcode( "and", 0x29, 1, AddressingType.IMMEDIATE, 2 );          // AND #$nn
      sys.AddOpcode( "asl", 0x0a, 0, AddressingType.IMPLICIT, 2 );           // ASL
      sys.AddOpcode( "asl", 0x0e, 2, AddressingType.ABSOLUTE, 6 );           // ASL $hhll
      sys.AddOpcode( "asl", 0x1e, 2, AddressingType.ABSOLUTE_X, 6, 1 );      // ASL $hhll, X
      sys.AddOpcode( "asl", 0x06, 1, AddressingType.ZEROPAGE, 5 );           // ASL $ll
      sys.AddOpcode( "asl", 0x16, 1, AddressingType.ZEROPAGE_X, 6 );         // ASL $ll, X
      sys.AddOpcode( "bcc", 0x90, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BCC $hhll
      sys.AddOpcode( "bcs", 0xB0, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BCS $hhll
      sys.AddOpcode( "beq", 0xF0, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BEQ $hhll
      sys.AddOpcode( "bit", 0x2c, 2, AddressingType.ABSOLUTE, 4 );           // BIT $hhll
      sys.AddOpcode( "bit", 0x24, 1, AddressingType.ZEROPAGE, 3 );           // BIT $ll
      sys.AddOpcode( "bmi", 0x30, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BMI $hhll
      sys.AddOpcode( "bne", 0xD0, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BNE $hhll
      sys.AddOpcode( "bpl", 0x10, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BPL $hhll
      sys.AddOpcode( "brk", 0x00, 0, AddressingType.IMPLICIT, 7 );           // BRK
      sys.AddOpcode( "bvc", 0x50, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BVC $hhll
      sys.AddOpcode( "bvs", 0x70, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BVS $hhll
      sys.AddOpcode( "clc", 0x18, 0, AddressingType.IMPLICIT, 2 );           // CLC
      sys.AddOpcode( "cld", 0xD8, 0, AddressingType.IMPLICIT, 2 );           // CLD
      sys.AddOpcode( "cli", 0x58, 0, AddressingType.IMPLICIT, 2 );           // CLI
      sys.AddOpcode( "clv", 0xB8, 0, AddressingType.IMPLICIT, 2 );           // CLV
      sys.AddOpcode( "cmp", 0xCD, 2, AddressingType.ABSOLUTE, 4 );           // CMP $hhll
      sys.AddOpcode( "cmp", 0xDD, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // CMP $hhll, X
      sys.AddOpcode( "cmp", 0xD9, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // CMP $hhll, Y
      sys.AddOpcode( "cmp", 0xC5, 1, AddressingType.ZEROPAGE, 3 );           // CMP $ll
      sys.AddOpcode( "cmp", 0xD5, 1, AddressingType.ZEROPAGE_X, 4 );         // CMP $ll, X
      sys.AddOpcode( "cmp", 0xD1, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // CMP ($ll), Y
      sys.AddOpcode( "cmp", 0xC1, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // CMP ($ll,X)
      sys.AddOpcode( "cmp", 0xC9, 1, AddressingType.IMMEDIATE, 2 );          // CMP #$nn
      sys.AddOpcode( "cpx", 0xEC, 2, AddressingType.ABSOLUTE, 4 );           // CPX $hhll
      sys.AddOpcode( "cpx", 0xE4, 1, AddressingType.ZEROPAGE, 3 );           // CPX $ll
      sys.AddOpcode( "cpx", 0xE0, 1, AddressingType.IMMEDIATE, 2 );          // CPX #$nn
      sys.AddOpcode( "cpy", 0xCC, 2, AddressingType.ABSOLUTE, 4 );           // CPY $hhll
      sys.AddOpcode( "cpy", 0xC4, 1, AddressingType.ZEROPAGE, 3 );           // CPY $ll
      sys.AddOpcode( "cpy", 0xC0, 1, AddressingType.IMMEDIATE, 2 );          // CPY #$nn
      sys.AddOpcode( "dec", 0xCE, 2, AddressingType.ABSOLUTE, 6 );           // DEC $hhll
      sys.AddOpcode( "dec", 0xDE, 2, AddressingType.ABSOLUTE_X, 7 );         // DEC $hhll, X
      sys.AddOpcode( "dec", 0xC6, 1, AddressingType.ZEROPAGE, 5 );           // DEC $ll
      sys.AddOpcode( "dec", 0xD6, 1, AddressingType.ZEROPAGE_X, 6 );         // DEC $ll, X
      sys.AddOpcode( "dex", 0xCA, 0, AddressingType.IMPLICIT, 2 );           // DEX
      sys.AddOpcode( "dey", 0x88, 0, AddressingType.IMPLICIT, 2 );           // DEY
      sys.AddOpcode( "eor", 0x4D, 2, AddressingType.ABSOLUTE, 4 );           // EOR $hhll
      sys.AddOpcode( "eor", 0x5D, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // EOR $hhll, X
      sys.AddOpcode( "eor", 0x59, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // EOR $hhll, Y
      sys.AddOpcode( "eor", 0x45, 1, AddressingType.ZEROPAGE, 3 );           // EOR $ll
      sys.AddOpcode( "eor", 0x55, 1, AddressingType.ZEROPAGE_X, 4 );         // EOR $ll, X
      sys.AddOpcode( "eor", 0x51, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // EOR ($ll), Y
      sys.AddOpcode( "eor", 0x41, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // EOR ($ll,X)
      sys.AddOpcode( "eor", 0x49, 1, AddressingType.IMMEDIATE, 2 );          // EOR #$nn
      sys.AddOpcode( "inc", 0xEE, 2, AddressingType.ABSOLUTE, 6 );           // INC $hhll
      sys.AddOpcode( "inc", 0xFE, 2, AddressingType.ABSOLUTE_X, 7 );         // INC $hhll, X
      sys.AddOpcode( "inc", 0xE6, 1, AddressingType.ZEROPAGE, 5 );           // INC $ll
      sys.AddOpcode( "inc", 0xF6, 1, AddressingType.ZEROPAGE_X, 6 );         // INC $ll, X
      sys.AddOpcode( "inx", 0xE8, 0, AddressingType.IMPLICIT, 2 );           // INX
      sys.AddOpcode( "iny", 0xC8, 0, AddressingType.IMPLICIT, 2 );           // INY
      sys.AddOpcode( "jmp", 0x4C, 2, AddressingType.ABSOLUTE, 3 );           // JMP $hhll
      sys.AddOpcode( "jmp", 0x6C, 2, AddressingType.INDIRECT, 6 );           // JMP ($hhll)
      sys.AddOpcode( "jsr", 0x20, 2, AddressingType.ABSOLUTE, 6 );           // JSR $hhll
      sys.AddOpcode( "lda", 0xAD, 2, AddressingType.ABSOLUTE, 4 );           // LDA $hhll
      sys.AddOpcode( "lda", 0xBD, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // LDA $hhll, X
      sys.AddOpcode( "lda", 0xB9, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // LDA $hhll, Y
      sys.AddOpcode( "lda", 0xA5, 1, AddressingType.ZEROPAGE, 3 );           // LDA $ll
      sys.AddOpcode( "lda", 0xB5, 1, AddressingType.ZEROPAGE_X, 4 );         // LDA $ll, X
      sys.AddOpcode( "lda", 0xB1, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // LDA ($ll), Y
      sys.AddOpcode( "lda", 0xA1, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // LDA ($ll,X)
      sys.AddOpcode( "lda", 0xA9, 1, AddressingType.IMMEDIATE, 2 );          // LDA #$nn
      sys.AddOpcode( "ldx", 0xAE, 2, AddressingType.ABSOLUTE, 4 );           // LDX $hhll
      sys.AddOpcode( "ldx", 0xBE, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // LDX $hhll, Y
      sys.AddOpcode( "ldx", 0xA6, 1, AddressingType.ZEROPAGE, 3 );           // LDX $ll
      sys.AddOpcode( "ldx", 0xB6, 1, AddressingType.ZEROPAGE_Y, 4 );         // LDX $ll, Y
      sys.AddOpcode( "ldx", 0xA2, 1, AddressingType.IMMEDIATE, 2 );          // LDX #$nn
      sys.AddOpcode( "ldy", 0xAC, 2, AddressingType.ABSOLUTE, 4 );           // LDY $hhll
      sys.AddOpcode( "ldy", 0xBC, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // LDY $hhll, X
      sys.AddOpcode( "ldy", 0xA4, 1, AddressingType.ZEROPAGE, 3 );           // LDY $ll
      sys.AddOpcode( "ldy", 0xB4, 1, AddressingType.ZEROPAGE_X, 4 );         // LDY $ll, X
      sys.AddOpcode( "ldy", 0xA0, 1, AddressingType.IMMEDIATE, 2 );          // LDY #$nn
      sys.AddOpcode( "lsr", 0x4A, 0, AddressingType.IMPLICIT, 2 );           // LSR
      sys.AddOpcode( "lsr", 0x4E, 2, AddressingType.ABSOLUTE, 6 );           // LSR $hhll
      sys.AddOpcode( "lsr", 0x5E, 2, AddressingType.ABSOLUTE_X, 6, 1 );      // LSR $hhll, X
      sys.AddOpcode( "lsr", 0x46, 1, AddressingType.ZEROPAGE, 5 );           // LSR $ll
      sys.AddOpcode( "lsr", 0x56, 1, AddressingType.ZEROPAGE_X, 6 );         // LSR $ll, X
      sys.AddOpcode( "nop", 0xEA, 0, AddressingType.IMPLICIT, 2 );           // NOP
      sys.AddOpcode( "ora", 0x0D, 2, AddressingType.ABSOLUTE, 4 );           // ORA $hhll
      sys.AddOpcode( "ora", 0x1D, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // ORA $hhll, X
      sys.AddOpcode( "ora", 0x19, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // ORA $hhll, Y
      sys.AddOpcode( "ora", 0x05, 1, AddressingType.ZEROPAGE, 3 );           // ORA $ll
      sys.AddOpcode( "ora", 0x15, 1, AddressingType.ZEROPAGE_X, 4 );         // ORA $ll, X
      sys.AddOpcode( "ora", 0x11, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // ORA ($ll), Y
      sys.AddOpcode( "ora", 0x01, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // ORA ($ll,X)
      sys.AddOpcode( "ora", 0x09, 1, AddressingType.IMMEDIATE, 2 );          // ORA #$nn
      sys.AddOpcode( "pha", 0x48, 0, AddressingType.IMPLICIT, 3 );           // PHA
      sys.AddOpcode( "php", 0x08, 0, AddressingType.IMPLICIT, 3 );           // PHP
      sys.AddOpcode( "pla", 0x68, 0, AddressingType.IMPLICIT, 4 );           // PLA
      sys.AddOpcode( "plp", 0x28, 0, AddressingType.IMPLICIT, 4 );           // PLP
      sys.AddOpcode( "rol", 0x2A, 0, AddressingType.IMPLICIT, 2 );           // ROL
      sys.AddOpcode( "rol", 0x2E, 2, AddressingType.ABSOLUTE, 6 );           // ROL $hhll
      sys.AddOpcode( "rol", 0x3E, 2, AddressingType.ABSOLUTE_X, 6, 1 );      // ROL $hhll, X
      sys.AddOpcode( "rol", 0x26, 1, AddressingType.ZEROPAGE, 5 );           // ROL $ll
      sys.AddOpcode( "rol", 0x36, 1, AddressingType.ZEROPAGE_X, 6 );         // ROL $ll, X
      sys.AddOpcode( "ror", 0x6A, 0, AddressingType.IMPLICIT, 2 );           // ROR
      sys.AddOpcode( "ror", 0x6E, 2, AddressingType.ABSOLUTE, 6 );           // ROR $hhll
      sys.AddOpcode( "ror", 0x7E, 2, AddressingType.ABSOLUTE_X, 6, 1 );      // ROR $hhll, X
      sys.AddOpcode( "ror", 0x66, 1, AddressingType.ZEROPAGE, 5 );           // ROR $ll
      sys.AddOpcode( "ror", 0x76, 1, AddressingType.ZEROPAGE_X, 6 );         // ROR $ll, X
      sys.AddOpcode( "rti", 0x40, 0, AddressingType.IMPLICIT, 6 );           // RTI
      sys.AddOpcode( "rts", 0x60, 0, AddressingType.IMPLICIT, 6 );           // RTS
      sys.AddOpcode( "sbc", 0xED, 2, AddressingType.ABSOLUTE, 4, 1 );        // SBC $hhll
      sys.AddOpcode( "sbc", 0xFD, 2, AddressingType.ABSOLUTE_X, 4, 1, 1, 0 );// SBC $hhll, X
      sys.AddOpcode( "sbc", 0xF9, 2, AddressingType.ABSOLUTE_Y, 4, 1, 1, 0 );// SBC $hhll, Y
      sys.AddOpcode( "sbc", 0xE5, 1, AddressingType.ZEROPAGE, 3, 1 );        // SBC $ll
      sys.AddOpcode( "sbc", 0xF5, 1, AddressingType.ZEROPAGE_X, 4, 1 );      // SBC $ll, X
      sys.AddOpcode( "sbc", 0xF1, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1, 1, 0 );// SBC ($ll), Y
      sys.AddOpcode( "sbc", 0xE1, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6, 1 );      // SBC ($ll,X)
      sys.AddOpcode( "sbc", 0xE9, 1, AddressingType.IMMEDIATE, 2, 1 );       // SBC #$nn
      sys.AddOpcode( "sec", 0x38, 0, AddressingType.IMPLICIT, 2 );           // SEC
      sys.AddOpcode( "sed", 0xF8, 0, AddressingType.IMPLICIT, 2 );           // SED
      sys.AddOpcode( "sei", 0x78, 0, AddressingType.IMPLICIT, 2 );           // SEI
      sys.AddOpcode( "sta", 0x8D, 2, AddressingType.ABSOLUTE, 4 );           // STA $hhll
      sys.AddOpcode( "sta", 0x9D, 2, AddressingType.ABSOLUTE_X, 5 );         // STA $hhll, X
      sys.AddOpcode( "sta", 0x99, 2, AddressingType.ABSOLUTE_Y, 5 );         // STA $hhll, Y
      sys.AddOpcode( "sta", 0x85, 1, AddressingType.ZEROPAGE, 3 );           // STA $ll
      sys.AddOpcode( "sta", 0x95, 1, AddressingType.ZEROPAGE_X, 4 );         // STA $ll, X
      sys.AddOpcode( "sta", 0x91, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 6 );         // STA ($ll), Y
      sys.AddOpcode( "sta", 0x81, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // STA ($ll,X)
      sys.AddOpcode( "stx", 0x8E, 2, AddressingType.ABSOLUTE, 4 );           // STX $hhll
      sys.AddOpcode( "stx", 0x86, 1, AddressingType.ZEROPAGE, 3 );           // STX $ll
      sys.AddOpcode( "stx", 0x96, 1, AddressingType.ZEROPAGE_Y, 4 );         // STX $ll, Y
      sys.AddOpcode( "sty", 0x8C, 2, AddressingType.ABSOLUTE, 4 );           // STY $hhll
      sys.AddOpcode( "sty", 0x84, 1, AddressingType.ZEROPAGE, 3 );           // STY $ll
      sys.AddOpcode( "sty", 0x94, 1, AddressingType.ZEROPAGE_X, 4 );         // STY $ll, X
      sys.AddOpcode( "tax", 0xAA, 0, AddressingType.IMPLICIT, 2 );           // TAX
      sys.AddOpcode( "tay", 0xA8, 0, AddressingType.IMPLICIT, 2 );           // TAY
      sys.AddOpcode( "tsx", 0xBA, 0, AddressingType.IMPLICIT, 2 );           // TSX
      sys.AddOpcode( "txa", 0x8A, 0, AddressingType.IMPLICIT, 2 );           // TXA
      sys.AddOpcode( "txs", 0x9A, 0, AddressingType.IMPLICIT, 2 );           // TXS
      sys.AddOpcode( "tya", 0x98, 0, AddressingType.IMPLICIT, 2 );           // TYA


      // pageboundary cycle add is WRONG, it's for decimal mode
      sys.AddOpcode( "adc", 0x72, 1, AddressingType.ZEROPAGE_INDIRECT_Z, 5, 1 ); // adc ($12),z
      sys.AddOpcode( "and", 0x32, 1, AddressingType.ZEROPAGE_INDIRECT_Z, 5 );    // and ($12),z
      sys.AddOpcode( "cmp", 0xd2, 1, AddressingType.ZEROPAGE_INDIRECT_Z, 5 );    // cmp ($12),z
      sys.AddOpcode( "eor", 0x52, 1, AddressingType.ZEROPAGE_INDIRECT_Z, 5 );    // eor ($12),z
      sys.AddOpcode( "lda", 0xb2, 1, AddressingType.ZEROPAGE_INDIRECT_Z, 5 );    // lda ($12),z
      sys.AddOpcode( "ora", 0x12, 1, AddressingType.ZEROPAGE_INDIRECT_Z, 5 );    // ora ($12),z
      sys.AddOpcode( "sbc", 0xf2, 1, AddressingType.ZEROPAGE_INDIRECT_Z, 5, 1 ); // sbc ($12),z
      sys.AddOpcode( "sta", 0x92, 1, AddressingType.ZEROPAGE_INDIRECT_Z, 5 );    // sta ($12),z

      sys.AddOpcode( "bit", 0x89, 1, AddressingType.IMMEDIATE, 2 );              // bit #$12
      sys.AddOpcode( "bit", 0x34, 1, AddressingType.ZEROPAGE_X, 4 );             // bit $12,x
      sys.AddOpcode( "bit", 0x3C, 2, AddressingType.ABSOLUTE_X, 4, 1 );          // bit $1234,x

      sys.AddOpcode( "dec", 0x3A, 0, AddressingType.IMPLICIT, 2 );               // dec
      sys.AddOpcode( "inc", 0x1A, 0, AddressingType.IMPLICIT, 2 );               // inc

      sys.AddOpcode( "jmp", 0x7C, 2, AddressingType.ABSOLUTE_INDIRECT_X, 6 );    // jmp ($1234,x)

      sys.AddOpcode( "bra", 0x80, 1, AddressingType.RELATIVE, 3, 1 );            // bra label

      sys.AddOpcode( "phx", 0xDA, 0, AddressingType.IMPLICIT, 3 );               // phx
      sys.AddOpcode( "phy", 0x5A, 0, AddressingType.IMPLICIT, 3 );               // phy
      sys.AddOpcode( "plx", 0xfA, 0, AddressingType.IMPLICIT, 4 );               // plx
      sys.AddOpcode( "ply", 0x7A, 0, AddressingType.IMPLICIT, 4 );               // ply

      sys.AddOpcode( "stz", 0x64, 1, AddressingType.ZEROPAGE, 3 );               // stz $12
      sys.AddOpcode( "stz", 0x74, 1, AddressingType.ZEROPAGE_X, 4 );             // stz $12,x
      sys.AddOpcode( "stz", 0x9c, 2, AddressingType.ABSOLUTE, 4 );               // stz $1234
      sys.AddOpcode( "stz", 0x9e, 2, AddressingType.ABSOLUTE_X, 5 );             // stz $1234,x

      sys.AddOpcode( "trb", 0x14, 1, AddressingType.ZEROPAGE, 5 );               // trb $12
      sys.AddOpcode( "trb", 0x1C, 2, AddressingType.ABSOLUTE, 6 );               // trb $1234

      sys.AddOpcode( "tsb", 0x04, 1, AddressingType.ZEROPAGE, 5 );               // tsb $12
      sys.AddOpcode( "tsb", 0x0C, 2, AddressingType.ABSOLUTE, 6 );               // tsb $1234

      sys.AddOpcode( "bbr0", 0x0f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr0 $12,LABEL
      sys.AddOpcode( "bbr1", 0x1f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr1 $12,LABEL
      sys.AddOpcode( "bbr2", 0x2f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr2 $12,LABEL
      sys.AddOpcode( "bbr3", 0x3f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr3 $12,LABEL
      sys.AddOpcode( "bbr4", 0x4f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr4 $12,LABEL
      sys.AddOpcode( "bbr5", 0x5f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr5 $12,LABEL
      sys.AddOpcode( "bbr6", 0x6f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr6 $12,LABEL
      sys.AddOpcode( "bbr7", 0x7f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr7 $12,LABEL
      sys.AddOpcode( "bbs0", 0x8f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs0 $12,LABEL
      sys.AddOpcode( "bbs1", 0x9f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs1 $12,LABEL
      sys.AddOpcode( "bbs2", 0xaf, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs2 $12,LABEL
      sys.AddOpcode( "bbs3", 0xbf, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs3 $12,LABEL
      sys.AddOpcode( "bbs4", 0xcf, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs4 $12,LABEL
      sys.AddOpcode( "bbs5", 0xdf, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs5 $12,LABEL
      sys.AddOpcode( "bbs6", 0xef, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs6 $12,LABEL
      sys.AddOpcode( "bbs7", 0xff, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs7 $12,LABEL

      sys.AddOpcode( "rmb0", 0x07, 1, AddressingType.ZEROPAGE, 5 );     // rmb0 $12
      sys.AddOpcode( "rmb1", 0x17, 1, AddressingType.ZEROPAGE, 5 );     // rmb1 $12
      sys.AddOpcode( "rmb2", 0x27, 1, AddressingType.ZEROPAGE, 5 );     // rmb2 $12
      sys.AddOpcode( "rmb3", 0x37, 1, AddressingType.ZEROPAGE, 5 );     // rmb3 $12
      sys.AddOpcode( "rmb4", 0x47, 1, AddressingType.ZEROPAGE, 5 );     // rmb4 $12
      sys.AddOpcode( "rmb5", 0x57, 1, AddressingType.ZEROPAGE, 5 );     // rmb5 $12
      sys.AddOpcode( "rmb6", 0x67, 1, AddressingType.ZEROPAGE, 5 );     // rmb6 $12
      sys.AddOpcode( "rmb7", 0x77, 1, AddressingType.ZEROPAGE, 5 );     // rmb7 $12
      sys.AddOpcode( "smb0", 0x87, 1, AddressingType.ZEROPAGE, 5 );     // smb0 $12
      sys.AddOpcode( "smb1", 0x97, 1, AddressingType.ZEROPAGE, 5 );     // smb1 $12
      sys.AddOpcode( "smb2", 0xa7, 1, AddressingType.ZEROPAGE, 5 );     // smb2 $12
      sys.AddOpcode( "smb3", 0xb7, 1, AddressingType.ZEROPAGE, 5 );     // smb3 $12
      sys.AddOpcode( "smb4", 0xc7, 1, AddressingType.ZEROPAGE, 5 );     // smb4 $12
      sys.AddOpcode( "smb5", 0xd7, 1, AddressingType.ZEROPAGE, 5 );     // smb5 $12
      sys.AddOpcode( "smb6", 0xe7, 1, AddressingType.ZEROPAGE, 5 );     // smb6 $12
      sys.AddOpcode( "smb7", 0xf7, 1, AddressingType.ZEROPAGE, 5 );     // smb7 $12

      sys.AddOpcode( "asw", 0xcb, 2, AddressingType.ABSOLUTE, 7 );               // asw

      sys.AddOpcode( "asr", 0x43, 0, AddressingType.IMPLICIT, 2 );      // asr, asr a
      sys.AddOpcode( "asr", 0x44, 1, AddressingType.ZEROPAGE, 5 );      // asr $zp
      sys.AddOpcode( "asr", 0x54, 1, AddressingType.ZEROPAGE_X, 6 );    // asr $zp,x
      sys.AddOpcode( "lbcc", 0x93, 2, AddressingType.RELATIVE_16, 3, 1 );   // bcc $nnnn
      sys.AddOpcode( "lbcs", 0xb3, 2, AddressingType.RELATIVE_16, 3, 1 );   // bcs $nnnn
      sys.AddOpcode( "lbeq", 0xf3, 2, AddressingType.RELATIVE_16, 3, 1 );   // beq $nnnn
      sys.AddOpcode( "lbmi", 0x33, 2, AddressingType.RELATIVE_16, 3, 1 );   // bmi $nnnn
      sys.AddOpcode( "lbne", 0xd3, 2, AddressingType.RELATIVE_16, 3, 1 );   // bne $nnnn
      sys.AddOpcode( "lbpl", 0x13, 2, AddressingType.RELATIVE_16, 3, 1 );   // bpl $nnnn
      sys.AddOpcode( "lbra", 0x83, 2, AddressingType.RELATIVE_16, 4 );   // bra $nnnn
      sys.AddOpcode( "bsr", 0x63, 2, AddressingType.RELATIVE_16, 3, 1 );   // bsr $nnnn
      sys.AddOpcode( "lbvc", 0x53, 2, AddressingType.RELATIVE_16, 3, 1 );   // bvc $nnnn
      sys.AddOpcode( "lbvs", 0x73, 2, AddressingType.RELATIVE_16, 3, 1 );   // bvs $nnnn

      sys.AddOpcode( "cle", 0x02, 0, AddressingType.IMPLICIT, 2 );      // cle

      sys.AddOpcode( "cpz", 0xC2, 1, AddressingType.IMMEDIATE, 2 );     // cpz #xx
      sys.AddOpcode( "cpz", 0xD4, 1, AddressingType.ZEROPAGE, 3 );      // cpz $zp
      sys.AddOpcode( "cpz", 0xDC, 2, AddressingType.ABSOLUTE, 4 );      // cpz $nnnn

      sys.AddOpcode( "dew", 0xc3, 1, AddressingType.ZEROPAGE, 6 );      // dew $zp
      sys.AddOpcode( "dez", 0x3b, 0, AddressingType.IMPLICIT, 2 );      // dez
      sys.AddOpcode( "inw", 0xe3, 1, AddressingType.ZEROPAGE, 6 );      // inw $zp
      sys.AddOpcode( "inz", 0x1b, 0, AddressingType.IMPLICIT, 2 );      // inz

      sys.AddOpcode( "jsr", 0x22, 2, AddressingType.INDIRECT, 6 );   // jsr ($nnnn)
      sys.AddOpcode( "jsr", 0x23, 2, AddressingType.ABSOLUTE_INDIRECT_X, 6 );  // jsr ($nnnn,x)
      sys.AddOpcode( "lda", 0xE2, 1, AddressingType.ZEROPAGE_INDIRECT_SP_Y, 7 ); // lda ($zp,S),y
      sys.AddOpcode( "ldz", 0xa3, 1, AddressingType.IMMEDIATE, 2 );      // ldz #$nn
      sys.AddOpcode( "ldz", 0xab, 2, AddressingType.ABSOLUTE, 4 );       // ldz $nnnn
      sys.AddOpcode( "ldz", 0xbb, 2, AddressingType.ABSOLUTE_X, 4, 1 );     // ldz $nnnn,x

      sys.AddOpcode( "aug", 0x5C, 0, AddressingType.IMPLICIT, 2 );       // aug (4-byte NOP RFU?), also called MAP
      sys.AddOpcode( "neg", 0x42, 0, AddressingType.IMPLICIT, 2 );       // neg

      sys.AddOpcode( "phw", 0xF4, 2, AddressingType.IMMEDIATE_16, 4 );   // phw #$nnnn
      sys.AddOpcode( "phw", 0xFC, 2, AddressingType.ABSOLUTE, 4 );       // phw $nnnn

      sys.AddOpcode( "phz", 0xdb, 0, AddressingType.IMPLICIT, 3 );    // phz
      sys.AddOpcode( "plz", 0xfb, 0, AddressingType.IMPLICIT, 4 );    // plz

      sys.AddOpcode( "row", 0xeb, 2, AddressingType.ABSOLUTE, 7 );       // row $nnnn
      sys.AddOpcode( "rtn", 0x62, 1, AddressingType.IMMEDIATE, 6 );      // rtn #$nn

      sys.AddOpcode( "see", 0x03, 0, AddressingType.IMPLICIT, 2 );       // see

      sys.AddOpcode( "sta", 0x82, 1, AddressingType.ZEROPAGE_INDIRECT_SP_Y, 7 ); // sta ($zp,S),y
      sys.AddOpcode( "stx", 0x9b, 2, AddressingType.ABSOLUTE_Y, 5 );     // stx $nnnn,y
      sys.AddOpcode( "sty", 0x8b, 2, AddressingType.ABSOLUTE_X, 5 );     // sty $nnnn,x
      sys.AddOpcode( "tab", 0x5b, 0, AddressingType.IMPLICIT, 2 );       // tab
      sys.AddOpcode( "taz", 0x4b, 0, AddressingType.IMPLICIT, 2 );       // taz
      sys.AddOpcode( "tba", 0x7b, 0, AddressingType.IMPLICIT, 2 );       // tba
      sys.AddOpcode( "tsy", 0x0b, 0, AddressingType.IMPLICIT, 2 );       // tsy
      sys.AddOpcode( "tys", 0x2b, 0, AddressingType.IMPLICIT, 2 );       // tys
      sys.AddOpcode( "tza", 0x6b, 0, AddressingType.IMPLICIT, 2 );       // tza

      return sys;
    }



    public static Processor Create4502()
    {
      var  sys = new Processor( "4502" );

      sys.AddOpcode( "adc", 0x6d, 2, AddressingType.ABSOLUTE, 4, 1 );        // ADC $hhll
      sys.AddOpcode( "adc", 0x7d, 2, AddressingType.ABSOLUTE_X, 4, 1, 1, 0 );// ADC $hhll, X
      sys.AddOpcode( "adc", 0x79, 2, AddressingType.ABSOLUTE_Y, 4, 1, 1, 0 );// ADC $hhll, Y
      sys.AddOpcode( "adc", 0x65, 1, AddressingType.ZEROPAGE, 3, 1, 1, 0 );  // ADC $ll
      sys.AddOpcode( "adc", 0x75, 1, AddressingType.ZEROPAGE_X, 4, 1 );      // ADC $ll, X
      sys.AddOpcode( "adc", 0x71, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1, 1, 0 );// ADC ($ll), Y
      sys.AddOpcode( "adc", 0x61, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6, 1 );      // ADC ($ll,X)
      sys.AddOpcode( "adc", 0x69, 1, AddressingType.IMMEDIATE, 2, 1 );       // ADC #$nn
      sys.AddOpcode( "and", 0x2d, 2, AddressingType.ABSOLUTE, 4 );           // AND $hhll
      sys.AddOpcode( "and", 0x3d, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // AND $hhll, X
      sys.AddOpcode( "and", 0x39, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // AND $hhll, Y
      sys.AddOpcode( "and", 0x25, 1, AddressingType.ZEROPAGE, 3 );           // AND $ll
      sys.AddOpcode( "and", 0x35, 1, AddressingType.ZEROPAGE_X, 4 );         // AND $ll, X
      sys.AddOpcode( "and", 0x31, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // AND ($ll), Y
      sys.AddOpcode( "and", 0x21, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // AND ($ll,X)
      sys.AddOpcode( "and", 0x29, 1, AddressingType.IMMEDIATE, 2 );          // AND #$nn
      sys.AddOpcode( "asl", 0x0a, 0, AddressingType.IMPLICIT, 2 );           // ASL
      sys.AddOpcode( "asl", 0x0e, 2, AddressingType.ABSOLUTE, 6 );           // ASL $hhll
      sys.AddOpcode( "asl", 0x1e, 2, AddressingType.ABSOLUTE_X, 6, 1 );      // ASL $hhll, X
      sys.AddOpcode( "asl", 0x06, 1, AddressingType.ZEROPAGE, 5 );           // ASL $ll
      sys.AddOpcode( "asl", 0x16, 1, AddressingType.ZEROPAGE_X, 6 );         // ASL $ll, X
      sys.AddOpcode( "bcc", 0x90, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BCC $hhll
      sys.AddOpcode( "bcs", 0xB0, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BCS $hhll
      sys.AddOpcode( "beq", 0xF0, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BEQ $hhll
      sys.AddOpcode( "bit", 0x2c, 2, AddressingType.ABSOLUTE, 4 );           // BIT $hhll
      sys.AddOpcode( "bit", 0x24, 1, AddressingType.ZEROPAGE, 3 );           // BIT $ll
      sys.AddOpcode( "bmi", 0x30, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BMI $hhll
      sys.AddOpcode( "bne", 0xD0, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BNE $hhll
      sys.AddOpcode( "bpl", 0x10, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BPL $hhll
      sys.AddOpcode( "brk", 0x00, 0, AddressingType.IMPLICIT, 7 );           // BRK
      sys.AddOpcode( "bvc", 0x50, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BVC $hhll
      sys.AddOpcode( "bvs", 0x70, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BVS $hhll
      sys.AddOpcode( "clc", 0x18, 0, AddressingType.IMPLICIT, 2 );           // CLC
      sys.AddOpcode( "cld", 0xD8, 0, AddressingType.IMPLICIT, 2 );           // CLD
      sys.AddOpcode( "cli", 0x58, 0, AddressingType.IMPLICIT, 2 );           // CLI
      sys.AddOpcode( "clv", 0xB8, 0, AddressingType.IMPLICIT, 2 );           // CLV
      sys.AddOpcode( "cmp", 0xCD, 2, AddressingType.ABSOLUTE, 4 );           // CMP $hhll
      sys.AddOpcode( "cmp", 0xDD, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // CMP $hhll, X
      sys.AddOpcode( "cmp", 0xD9, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // CMP $hhll, Y
      sys.AddOpcode( "cmp", 0xC5, 1, AddressingType.ZEROPAGE, 3 );           // CMP $ll
      sys.AddOpcode( "cmp", 0xD5, 1, AddressingType.ZEROPAGE_X, 4 );         // CMP $ll, X
      sys.AddOpcode( "cmp", 0xD1, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // CMP ($ll), Y
      sys.AddOpcode( "cmp", 0xC1, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // CMP ($ll,X)
      sys.AddOpcode( "cmp", 0xC9, 1, AddressingType.IMMEDIATE, 2 );          // CMP #$nn
      sys.AddOpcode( "cpx", 0xEC, 2, AddressingType.ABSOLUTE, 4 );           // CPX $hhll
      sys.AddOpcode( "cpx", 0xE4, 1, AddressingType.ZEROPAGE, 3 );           // CPX $ll
      sys.AddOpcode( "cpx", 0xE0, 1, AddressingType.IMMEDIATE, 2 );          // CPX #$nn
      sys.AddOpcode( "cpy", 0xCC, 2, AddressingType.ABSOLUTE, 4 );           // CPY $hhll
      sys.AddOpcode( "cpy", 0xC4, 1, AddressingType.ZEROPAGE, 3 );           // CPY $ll
      sys.AddOpcode( "cpy", 0xC0, 1, AddressingType.IMMEDIATE, 2 );          // CPY #$nn
      sys.AddOpcode( "dec", 0xCE, 2, AddressingType.ABSOLUTE, 6 );           // DEC $hhll
      sys.AddOpcode( "dec", 0xDE, 2, AddressingType.ABSOLUTE_X, 7 );         // DEC $hhll, X
      sys.AddOpcode( "dec", 0xC6, 1, AddressingType.ZEROPAGE, 5 );           // DEC $ll
      sys.AddOpcode( "dec", 0xD6, 1, AddressingType.ZEROPAGE_X, 6 );         // DEC $ll, X
      sys.AddOpcode( "dex", 0xCA, 0, AddressingType.IMPLICIT, 2 );           // DEX
      sys.AddOpcode( "dey", 0x88, 0, AddressingType.IMPLICIT, 2 );           // DEY
      sys.AddOpcode( "eor", 0x4D, 2, AddressingType.ABSOLUTE, 4 );           // EOR $hhll
      sys.AddOpcode( "eor", 0x5D, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // EOR $hhll, X
      sys.AddOpcode( "eor", 0x59, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // EOR $hhll, Y
      sys.AddOpcode( "eor", 0x45, 1, AddressingType.ZEROPAGE, 3 );           // EOR $ll
      sys.AddOpcode( "eor", 0x55, 1, AddressingType.ZEROPAGE_X, 4 );         // EOR $ll, X
      sys.AddOpcode( "eor", 0x51, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // EOR ($ll), Y
      sys.AddOpcode( "eor", 0x41, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // EOR ($ll,X)
      sys.AddOpcode( "eor", 0x49, 1, AddressingType.IMMEDIATE, 2 );          // EOR #$nn
      sys.AddOpcode( "inc", 0xEE, 2, AddressingType.ABSOLUTE, 6 );           // INC $hhll
      sys.AddOpcode( "inc", 0xFE, 2, AddressingType.ABSOLUTE_X, 7 );         // INC $hhll, X
      sys.AddOpcode( "inc", 0xE6, 1, AddressingType.ZEROPAGE, 5 );           // INC $ll
      sys.AddOpcode( "inc", 0xF6, 1, AddressingType.ZEROPAGE_X, 6 );         // INC $ll, X
      sys.AddOpcode( "inx", 0xE8, 0, AddressingType.IMPLICIT, 2 );           // INX
      sys.AddOpcode( "iny", 0xC8, 0, AddressingType.IMPLICIT, 2 );           // INY
      sys.AddOpcode( "jmp", 0x4C, 2, AddressingType.ABSOLUTE, 3 );           // JMP $hhll
      sys.AddOpcode( "jmp", 0x6C, 2, AddressingType.INDIRECT, 6 );           // JMP ($hhll)
      sys.AddOpcode( "jsr", 0x20, 2, AddressingType.ABSOLUTE, 6 );           // JSR $hhll
      sys.AddOpcode( "lda", 0xAD, 2, AddressingType.ABSOLUTE, 4 );           // LDA $hhll
      sys.AddOpcode( "lda", 0xBD, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // LDA $hhll, X
      sys.AddOpcode( "lda", 0xB9, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // LDA $hhll, Y
      sys.AddOpcode( "lda", 0xA5, 1, AddressingType.ZEROPAGE, 3 );           // LDA $ll
      sys.AddOpcode( "lda", 0xB5, 1, AddressingType.ZEROPAGE_X, 4 );         // LDA $ll, X
      sys.AddOpcode( "lda", 0xB1, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // LDA ($ll), Y
      sys.AddOpcode( "lda", 0xA1, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // LDA ($ll,X)
      sys.AddOpcode( "lda", 0xA9, 1, AddressingType.IMMEDIATE, 2 );          // LDA #$nn
      sys.AddOpcode( "ldx", 0xAE, 2, AddressingType.ABSOLUTE, 4 );           // LDX $hhll
      sys.AddOpcode( "ldx", 0xBE, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // LDX $hhll, Y
      sys.AddOpcode( "ldx", 0xA6, 1, AddressingType.ZEROPAGE, 3 );           // LDX $ll
      sys.AddOpcode( "ldx", 0xB6, 1, AddressingType.ZEROPAGE_Y, 4 );         // LDX $ll, Y
      sys.AddOpcode( "ldx", 0xA2, 1, AddressingType.IMMEDIATE, 2 );          // LDX #$nn
      sys.AddOpcode( "ldy", 0xAC, 2, AddressingType.ABSOLUTE, 4 );           // LDY $hhll
      sys.AddOpcode( "ldy", 0xBC, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // LDY $hhll, X
      sys.AddOpcode( "ldy", 0xA4, 1, AddressingType.ZEROPAGE, 3 );           // LDY $ll
      sys.AddOpcode( "ldy", 0xB4, 1, AddressingType.ZEROPAGE_X, 4 );         // LDY $ll, X
      sys.AddOpcode( "ldy", 0xA0, 1, AddressingType.IMMEDIATE, 2 );          // LDY #$nn
      sys.AddOpcode( "lsr", 0x4A, 0, AddressingType.IMPLICIT, 2 );           // LSR
      sys.AddOpcode( "lsr", 0x4E, 2, AddressingType.ABSOLUTE, 6 );           // LSR $hhll
      sys.AddOpcode( "lsr", 0x5E, 2, AddressingType.ABSOLUTE_X, 6, 1 );      // LSR $hhll, X
      sys.AddOpcode( "lsr", 0x46, 1, AddressingType.ZEROPAGE, 5 );           // LSR $ll
      sys.AddOpcode( "lsr", 0x56, 1, AddressingType.ZEROPAGE_X, 6 );         // LSR $ll, X
      sys.AddOpcode( "eom", 0xEA, 0, AddressingType.IMPLICIT, 2 );           // eom
      sys.AddOpcode( "ora", 0x0D, 2, AddressingType.ABSOLUTE, 4 );           // ORA $hhll
      sys.AddOpcode( "ora", 0x1D, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // ORA $hhll, X
      sys.AddOpcode( "ora", 0x19, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // ORA $hhll, Y
      sys.AddOpcode( "ora", 0x05, 1, AddressingType.ZEROPAGE, 3 );           // ORA $ll
      sys.AddOpcode( "ora", 0x15, 1, AddressingType.ZEROPAGE_X, 4 );         // ORA $ll, X
      sys.AddOpcode( "ora", 0x11, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // ORA ($ll), Y
      sys.AddOpcode( "ora", 0x01, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // ORA ($ll,X)
      sys.AddOpcode( "ora", 0x09, 1, AddressingType.IMMEDIATE, 2 );          // ORA #$nn
      sys.AddOpcode( "pha", 0x48, 0, AddressingType.IMPLICIT, 3 );           // PHA
      sys.AddOpcode( "php", 0x08, 0, AddressingType.IMPLICIT, 3 );           // PHP
      sys.AddOpcode( "pla", 0x68, 0, AddressingType.IMPLICIT, 4 );           // PLA
      sys.AddOpcode( "plp", 0x28, 0, AddressingType.IMPLICIT, 4 );           // PLP
      sys.AddOpcode( "rol", 0x2A, 0, AddressingType.IMPLICIT, 2 );           // ROL
      sys.AddOpcode( "rol", 0x2E, 2, AddressingType.ABSOLUTE, 6 );           // ROL $hhll
      sys.AddOpcode( "rol", 0x3E, 2, AddressingType.ABSOLUTE_X, 6, 1 );      // ROL $hhll, X
      sys.AddOpcode( "rol", 0x26, 1, AddressingType.ZEROPAGE, 5 );           // ROL $ll
      sys.AddOpcode( "rol", 0x36, 1, AddressingType.ZEROPAGE_X, 6 );         // ROL $ll, X
      sys.AddOpcode( "ror", 0x6A, 0, AddressingType.IMPLICIT, 2 );           // ROR
      sys.AddOpcode( "ror", 0x6E, 2, AddressingType.ABSOLUTE, 6 );           // ROR $hhll
      sys.AddOpcode( "ror", 0x7E, 2, AddressingType.ABSOLUTE_X, 6, 1 );      // ROR $hhll, X
      sys.AddOpcode( "ror", 0x66, 1, AddressingType.ZEROPAGE, 5 );           // ROR $ll
      sys.AddOpcode( "ror", 0x76, 1, AddressingType.ZEROPAGE_X, 6 );         // ROR $ll, X
      sys.AddOpcode( "rti", 0x40, 0, AddressingType.IMPLICIT, 6 );           // RTI
      sys.AddOpcode( "rts", 0x60, 0, AddressingType.IMPLICIT, 6 );           // RTS
      sys.AddOpcode( "sbc", 0xED, 2, AddressingType.ABSOLUTE, 4, 1 );        // SBC $hhll
      sys.AddOpcode( "sbc", 0xFD, 2, AddressingType.ABSOLUTE_X, 4, 1, 1, 0 );// SBC $hhll, X
      sys.AddOpcode( "sbc", 0xF9, 2, AddressingType.ABSOLUTE_Y, 4, 1, 1, 0 );// SBC $hhll, Y
      sys.AddOpcode( "sbc", 0xE5, 1, AddressingType.ZEROPAGE, 3, 1 );        // SBC $ll
      sys.AddOpcode( "sbc", 0xF5, 1, AddressingType.ZEROPAGE_X, 4, 1 );      // SBC $ll, X
      sys.AddOpcode( "sbc", 0xF1, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1, 1, 0 );// SBC ($ll), Y
      sys.AddOpcode( "sbc", 0xE1, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6, 1 );      // SBC ($ll,X)
      sys.AddOpcode( "sbc", 0xE9, 1, AddressingType.IMMEDIATE, 2, 1 );       // SBC #$nn
      sys.AddOpcode( "sec", 0x38, 0, AddressingType.IMPLICIT, 2 );           // SEC
      sys.AddOpcode( "sed", 0xF8, 0, AddressingType.IMPLICIT, 2 );           // SED
      sys.AddOpcode( "sei", 0x78, 0, AddressingType.IMPLICIT, 2 );           // SEI
      sys.AddOpcode( "sta", 0x8D, 2, AddressingType.ABSOLUTE, 4 );           // STA $hhll
      sys.AddOpcode( "sta", 0x9D, 2, AddressingType.ABSOLUTE_X, 5 );         // STA $hhll, X
      sys.AddOpcode( "sta", 0x99, 2, AddressingType.ABSOLUTE_Y, 5 );         // STA $hhll, Y
      sys.AddOpcode( "sta", 0x85, 1, AddressingType.ZEROPAGE, 3 );           // STA $ll
      sys.AddOpcode( "sta", 0x95, 1, AddressingType.ZEROPAGE_X, 4 );         // STA $ll, X
      sys.AddOpcode( "sta", 0x91, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 6 );         // STA ($ll), Y
      sys.AddOpcode( "sta", 0x81, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // STA ($ll,X)
      sys.AddOpcode( "stx", 0x8E, 2, AddressingType.ABSOLUTE, 4 );           // STX $hhll
      sys.AddOpcode( "stx", 0x86, 1, AddressingType.ZEROPAGE, 3 );           // STX $ll
      sys.AddOpcode( "stx", 0x96, 1, AddressingType.ZEROPAGE_Y, 4 );         // STX $ll, Y
      sys.AddOpcode( "sty", 0x8C, 2, AddressingType.ABSOLUTE, 4 );           // STY $hhll
      sys.AddOpcode( "sty", 0x84, 1, AddressingType.ZEROPAGE, 3 );           // STY $ll
      sys.AddOpcode( "sty", 0x94, 1, AddressingType.ZEROPAGE_X, 4 );         // STY $ll, X
      sys.AddOpcode( "tax", 0xAA, 0, AddressingType.IMPLICIT, 2 );           // TAX
      sys.AddOpcode( "tay", 0xA8, 0, AddressingType.IMPLICIT, 2 );           // TAY
      sys.AddOpcode( "tsx", 0xBA, 0, AddressingType.IMPLICIT, 2 );           // TSX
      sys.AddOpcode( "txa", 0x8A, 0, AddressingType.IMPLICIT, 2 );           // TXA
      sys.AddOpcode( "txs", 0x9A, 0, AddressingType.IMPLICIT, 2 );           // TXS
      sys.AddOpcode( "tya", 0x98, 0, AddressingType.IMPLICIT, 2 );           // TYA


      // pageboundary cycle add is WRONG, it's for decimal mode
      sys.AddOpcode( "adc", 0x72, 1, AddressingType.ZEROPAGE_INDIRECT_Z, 5, 1 ); // adc ($12),z
      sys.AddOpcode( "and", 0x32, 1, AddressingType.ZEROPAGE_INDIRECT_Z, 5 );    // and ($12),z
      sys.AddOpcode( "cmp", 0xd2, 1, AddressingType.ZEROPAGE_INDIRECT_Z, 5 );    // cmp ($12),z
      sys.AddOpcode( "eor", 0x52, 1, AddressingType.ZEROPAGE_INDIRECT_Z, 5 );    // eor ($12),z
      sys.AddOpcode( "lda", 0xb2, 1, AddressingType.ZEROPAGE_INDIRECT_Z, 5 );    // lda ($12),z
      sys.AddOpcode( "ora", 0x12, 1, AddressingType.ZEROPAGE_INDIRECT_Z, 5 );    // ora ($12),z
      sys.AddOpcode( "sbc", 0xf2, 1, AddressingType.ZEROPAGE_INDIRECT_Z, 5, 1 ); // sbc ($12),z
      sys.AddOpcode( "sta", 0x92, 1, AddressingType.ZEROPAGE_INDIRECT_Z, 5 );    // sta ($12),z

      sys.AddOpcode( "bit", 0x89, 1, AddressingType.IMMEDIATE, 2 );              // bit #$12
      sys.AddOpcode( "bit", 0x34, 1, AddressingType.ZEROPAGE_X, 4 );             // bit $12,x
      sys.AddOpcode( "bit", 0x3C, 2, AddressingType.ABSOLUTE_X, 4, 1 );          // bit $1234,x

      sys.AddOpcode( "dec", 0x3A, 0, AddressingType.IMPLICIT, 2 );               // dec
      sys.AddOpcode( "inc", 0x1A, 0, AddressingType.IMPLICIT, 2 );               // inc

      sys.AddOpcode( "jmp", 0x7C, 2, AddressingType.ABSOLUTE_INDIRECT_X, 6 );    // jmp ($1234,x)

      sys.AddOpcode( "bra", 0x80, 1, AddressingType.RELATIVE, 3, 1 );            // bra label

      sys.AddOpcode( "phx", 0xDA, 0, AddressingType.IMPLICIT, 3 );               // phx
      sys.AddOpcode( "phy", 0x5A, 0, AddressingType.IMPLICIT, 3 );               // phy
      sys.AddOpcode( "plx", 0xfA, 0, AddressingType.IMPLICIT, 4 );               // plx
      sys.AddOpcode( "ply", 0x7A, 0, AddressingType.IMPLICIT, 4 );               // ply

      sys.AddOpcode( "stz", 0x64, 1, AddressingType.ZEROPAGE, 3 );               // stz $12
      sys.AddOpcode( "stz", 0x74, 1, AddressingType.ZEROPAGE_X, 4 );             // stz $12,x
      sys.AddOpcode( "stz", 0x9c, 2, AddressingType.ABSOLUTE, 4 );               // stz $1234
      sys.AddOpcode( "stz", 0x9e, 2, AddressingType.ABSOLUTE_X, 5 );             // stz $1234,x

      sys.AddOpcode( "trb", 0x14, 1, AddressingType.ZEROPAGE, 5 );               // trb $12
      sys.AddOpcode( "trb", 0x1C, 2, AddressingType.ABSOLUTE, 6 );               // trb $1234

      sys.AddOpcode( "tsb", 0x04, 1, AddressingType.ZEROPAGE, 5 );               // tsb $12
      sys.AddOpcode( "tsb", 0x0C, 2, AddressingType.ABSOLUTE, 6 );               // tsb $1234

      sys.AddOpcode( "bbr0", 0x0f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr0 $12,LABEL
      sys.AddOpcode( "bbr1", 0x1f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr1 $12,LABEL
      sys.AddOpcode( "bbr2", 0x2f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr2 $12,LABEL
      sys.AddOpcode( "bbr3", 0x3f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr3 $12,LABEL
      sys.AddOpcode( "bbr4", 0x4f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr4 $12,LABEL
      sys.AddOpcode( "bbr5", 0x5f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr5 $12,LABEL
      sys.AddOpcode( "bbr6", 0x6f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr6 $12,LABEL
      sys.AddOpcode( "bbr7", 0x7f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr7 $12,LABEL
      sys.AddOpcode( "bbs0", 0x8f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs0 $12,LABEL
      sys.AddOpcode( "bbs1", 0x9f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs1 $12,LABEL
      sys.AddOpcode( "bbs2", 0xaf, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs2 $12,LABEL
      sys.AddOpcode( "bbs3", 0xbf, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs3 $12,LABEL
      sys.AddOpcode( "bbs4", 0xcf, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs4 $12,LABEL
      sys.AddOpcode( "bbs5", 0xdf, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs5 $12,LABEL
      sys.AddOpcode( "bbs6", 0xef, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs6 $12,LABEL
      sys.AddOpcode( "bbs7", 0xff, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs7 $12,LABEL

      sys.AddOpcode( "rmb0", 0x07, 1, AddressingType.ZEROPAGE, 5 );     // rmb0 $12
      sys.AddOpcode( "rmb1", 0x17, 1, AddressingType.ZEROPAGE, 5 );     // rmb1 $12
      sys.AddOpcode( "rmb2", 0x27, 1, AddressingType.ZEROPAGE, 5 );     // rmb2 $12
      sys.AddOpcode( "rmb3", 0x37, 1, AddressingType.ZEROPAGE, 5 );     // rmb3 $12
      sys.AddOpcode( "rmb4", 0x47, 1, AddressingType.ZEROPAGE, 5 );     // rmb4 $12
      sys.AddOpcode( "rmb5", 0x57, 1, AddressingType.ZEROPAGE, 5 );     // rmb5 $12
      sys.AddOpcode( "rmb6", 0x67, 1, AddressingType.ZEROPAGE, 5 );     // rmb6 $12
      sys.AddOpcode( "rmb7", 0x77, 1, AddressingType.ZEROPAGE, 5 );     // rmb7 $12
      sys.AddOpcode( "smb0", 0x87, 1, AddressingType.ZEROPAGE, 5 );     // smb0 $12
      sys.AddOpcode( "smb1", 0x97, 1, AddressingType.ZEROPAGE, 5 );     // smb1 $12
      sys.AddOpcode( "smb2", 0xa7, 1, AddressingType.ZEROPAGE, 5 );     // smb2 $12
      sys.AddOpcode( "smb3", 0xb7, 1, AddressingType.ZEROPAGE, 5 );     // smb3 $12
      sys.AddOpcode( "smb4", 0xc7, 1, AddressingType.ZEROPAGE, 5 );     // smb4 $12
      sys.AddOpcode( "smb5", 0xd7, 1, AddressingType.ZEROPAGE, 5 );     // smb5 $12
      sys.AddOpcode( "smb6", 0xe7, 1, AddressingType.ZEROPAGE, 5 );     // smb6 $12
      sys.AddOpcode( "smb7", 0xf7, 1, AddressingType.ZEROPAGE, 5 );     // smb7 $12

      sys.AddOpcode( "asw", 0xcb, 2, AddressingType.ABSOLUTE, 7 );               // asw

      sys.AddOpcode( "asr", 0x43, 0, AddressingType.IMPLICIT, 2 );      // asr, asr a
      sys.AddOpcode( "asr", 0x44, 1, AddressingType.ZEROPAGE, 5 );      // asr $zp
      sys.AddOpcode( "asr", 0x54, 1, AddressingType.ZEROPAGE_X, 6 );    // asr $zp,x
      sys.AddOpcode( "lbcc", 0x93, 2, AddressingType.RELATIVE_16, 3, 1 );   // bcc $nnnn
      sys.AddOpcode( "lbcs", 0xb3, 2, AddressingType.RELATIVE_16, 3, 1 );   // bcs $nnnn
      sys.AddOpcode( "lbeq", 0xf3, 2, AddressingType.RELATIVE_16, 3, 1 );   // beq $nnnn
      sys.AddOpcode( "lbmi", 0x33, 2, AddressingType.RELATIVE_16, 3, 1 );   // bmi $nnnn
      sys.AddOpcode( "lbne", 0xd3, 2, AddressingType.RELATIVE_16, 3, 1 );   // bne $nnnn
      sys.AddOpcode( "lbpl", 0x13, 2, AddressingType.RELATIVE_16, 3, 1 );   // bpl $nnnn
      sys.AddOpcode( "lbra", 0x83, 2, AddressingType.RELATIVE_16, 4 );   // bra $nnnn
      sys.AddOpcode( "bsr", 0x63, 2, AddressingType.RELATIVE_16, 3, 1 );   // bsr $nnnn
      sys.AddOpcode( "lbvc", 0x53, 2, AddressingType.RELATIVE_16, 3, 1 );   // bvc $nnnn
      sys.AddOpcode( "lbvs", 0x73, 2, AddressingType.RELATIVE_16, 3, 1 );   // bvs $nnnn

      sys.AddOpcode( "cle", 0x02, 0, AddressingType.IMPLICIT, 2 );      // cle

      sys.AddOpcode( "cpz", 0xC2, 1, AddressingType.IMMEDIATE, 2 );     // cpz #xx
      sys.AddOpcode( "cpz", 0xD4, 1, AddressingType.ZEROPAGE, 3 );      // cpz $zp
      sys.AddOpcode( "cpz", 0xDC, 2, AddressingType.ABSOLUTE, 4 );      // cpz $nnnn

      sys.AddOpcode( "dew", 0xc3, 1, AddressingType.ZEROPAGE, 6 );      // dew $zp
      sys.AddOpcode( "dez", 0x3b, 0, AddressingType.IMPLICIT, 2 );      // dez
      sys.AddOpcode( "inw", 0xe3, 1, AddressingType.ZEROPAGE, 6 );      // inw $zp
      sys.AddOpcode( "inz", 0x1b, 0, AddressingType.IMPLICIT, 2 );      // inz

      sys.AddOpcode( "jsr", 0x22, 2, AddressingType.INDIRECT, 6 );   // jsr ($nnnn)
      sys.AddOpcode( "jsr", 0x23, 2, AddressingType.ABSOLUTE_INDIRECT_X, 6 );  // jsr ($nnnn,x)
      sys.AddOpcode( "lda", 0xE2, 1, AddressingType.ZEROPAGE_INDIRECT_SP_Y, 7 ); // lda ($zp,SP),y
      sys.AddOpcode( "ldz", 0xa3, 1, AddressingType.IMMEDIATE, 2 );      // ldz #$nn
      sys.AddOpcode( "ldz", 0xab, 2, AddressingType.ABSOLUTE, 4 );       // ldz $nnnn
      sys.AddOpcode( "ldz", 0xbb, 2, AddressingType.ABSOLUTE_X, 4, 1 );     // ldz $nnnn,x

      sys.AddOpcode( "map", 0x5C, 0, AddressingType.IMPLICIT, 2 );       // map
      sys.AddOpcode( "neg", 0x42, 0, AddressingType.IMPLICIT, 2 );       // neg

      sys.AddOpcode( "phw", 0xF4, 2, AddressingType.IMMEDIATE_16, 4 );   // phw #$nnnn
      sys.AddOpcode( "phw", 0xFC, 2, AddressingType.ABSOLUTE, 4 );       // phw $nnnn

      sys.AddOpcode( "phz", 0xdb, 0, AddressingType.IMPLICIT, 3 );    // phz
      sys.AddOpcode( "plz", 0xfb, 0, AddressingType.IMPLICIT, 4 );    // plz

      sys.AddOpcode( "row", 0xeb, 2, AddressingType.ABSOLUTE, 7 );       // row $nnnn
      sys.AddOpcode( "rtn", 0x62, 1, AddressingType.IMMEDIATE, 6 );      // rtn #$nn

      sys.AddOpcode( "see", 0x03, 0, AddressingType.IMPLICIT, 2 );       // see

      sys.AddOpcode( "sta", 0x82, 1, AddressingType.ZEROPAGE_INDIRECT_SP_Y, 7 ); // sta ($zp,SP),y
      sys.AddOpcode( "stx", 0x9b, 2, AddressingType.ABSOLUTE_Y, 5 );     // stx $nnnn,y
      sys.AddOpcode( "sty", 0x8b, 2, AddressingType.ABSOLUTE_X, 5 );     // sty $nnnn,x
      sys.AddOpcode( "tab", 0x5b, 0, AddressingType.IMPLICIT, 2 );       // tab
      sys.AddOpcode( "taz", 0x4b, 0, AddressingType.IMPLICIT, 2 );       // taz
      sys.AddOpcode( "tba", 0x7b, 0, AddressingType.IMPLICIT, 2 );       // tba
      sys.AddOpcode( "tsy", 0x0b, 0, AddressingType.IMPLICIT, 2 );       // tsy
      sys.AddOpcode( "tys", 0x2b, 0, AddressingType.IMPLICIT, 2 );       // tys
      sys.AddOpcode( "tza", 0x6b, 0, AddressingType.IMPLICIT, 2 );       // tza

      return sys;
    }



    public static Processor CreateM65()
    {
      var  sys = new Processor( "M65" );

      sys.AddOpcode( "adc", 0x6d, 2, AddressingType.ABSOLUTE, 4, 1 );        // ADC $hhll
      sys.AddOpcode( "adc", 0x7d, 2, AddressingType.ABSOLUTE_X, 4, 1, 1, 0 );// ADC $hhll, X
      sys.AddOpcode( "adc", 0x79, 2, AddressingType.ABSOLUTE_Y, 4, 1, 1, 0 );// ADC $hhll, Y
      sys.AddOpcode( "adc", 0x65, 1, AddressingType.ZEROPAGE, 3, 1, 1, 0 );  // ADC $ll
      sys.AddOpcode( "adc", 0x75, 1, AddressingType.ZEROPAGE_X, 4, 1 );      // ADC $ll, X
      sys.AddOpcode( "adc", 0x71, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1, 1, 0 );// ADC ($ll), Y
      sys.AddOpcode( "adc", 0x61, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6, 1 );      // ADC ($ll,X)
      sys.AddOpcode( "adc", 0x69, 1, AddressingType.IMMEDIATE, 2, 1 );       // ADC #$nn
      sys.AddOpcode( "and", 0x2d, 2, AddressingType.ABSOLUTE, 4 );           // AND $hhll
      sys.AddOpcode( "and", 0x3d, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // AND $hhll, X
      sys.AddOpcode( "and", 0x39, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // AND $hhll, Y
      sys.AddOpcode( "and", 0x25, 1, AddressingType.ZEROPAGE, 3 );           // AND $ll
      sys.AddOpcode( "and", 0x35, 1, AddressingType.ZEROPAGE_X, 4 );         // AND $ll, X
      sys.AddOpcode( "and", 0x31, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // AND ($ll), Y
      sys.AddOpcode( "and", 0x21, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // AND ($ll,X)
      sys.AddOpcode( "and", 0x29, 1, AddressingType.IMMEDIATE, 2 );          // AND #$nn
      sys.AddOpcode( "asl", 0x0a, 0, AddressingType.IMPLICIT, 2 );           // ASL
      sys.AddOpcode( "asl", 0x0e, 2, AddressingType.ABSOLUTE, 6 );           // ASL $hhll
      sys.AddOpcode( "asl", 0x1e, 2, AddressingType.ABSOLUTE_X, 6, 1 );      // ASL $hhll, X
      sys.AddOpcode( "asl", 0x06, 1, AddressingType.ZEROPAGE, 5 );           // ASL $ll
      sys.AddOpcode( "asl", 0x16, 1, AddressingType.ZEROPAGE_X, 6 );         // ASL $ll, X
      sys.AddOpcode( "bcc", 0x90, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BCC $hhll
      sys.AddOpcode( "bcs", 0xB0, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BCS $hhll
      sys.AddOpcode( "beq", 0xF0, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BEQ $hhll
      sys.AddOpcode( "bit", 0x2c, 2, AddressingType.ABSOLUTE, 4 );           // BIT $hhll
      sys.AddOpcode( "bit", 0x24, 1, AddressingType.ZEROPAGE, 3 );           // BIT $ll
      sys.AddOpcode( "bmi", 0x30, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BMI $hhll
      sys.AddOpcode( "bne", 0xD0, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BNE $hhll
      sys.AddOpcode( "bpl", 0x10, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BPL $hhll
      sys.AddOpcode( "brk", 0x00, 0, AddressingType.IMPLICIT, 7 );           // BRK
      sys.AddOpcode( "bvc", 0x50, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BVC $hhll
      sys.AddOpcode( "bvs", 0x70, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BVS $hhll
      sys.AddOpcode( "clc", 0x18, 0, AddressingType.IMPLICIT, 2 );           // CLC
      sys.AddOpcode( "cld", 0xD8, 0, AddressingType.IMPLICIT, 2 );           // CLD
      sys.AddOpcode( "cli", 0x58, 0, AddressingType.IMPLICIT, 2 );           // CLI
      sys.AddOpcode( "clv", 0xB8, 0, AddressingType.IMPLICIT, 2 );           // CLV
      sys.AddOpcode( "cmp", 0xCD, 2, AddressingType.ABSOLUTE, 4 );           // CMP $hhll
      sys.AddOpcode( "cmp", 0xDD, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // CMP $hhll, X
      sys.AddOpcode( "cmp", 0xD9, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // CMP $hhll, Y
      sys.AddOpcode( "cmp", 0xC5, 1, AddressingType.ZEROPAGE, 3 );           // CMP $ll
      sys.AddOpcode( "cmp", 0xD5, 1, AddressingType.ZEROPAGE_X, 4 );         // CMP $ll, X
      sys.AddOpcode( "cmp", 0xD1, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // CMP ($ll), Y
      sys.AddOpcode( "cmp", 0xC1, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // CMP ($ll,X)
      sys.AddOpcode( "cmp", 0xC9, 1, AddressingType.IMMEDIATE, 2 );          // CMP #$nn
      sys.AddOpcode( "cpx", 0xEC, 2, AddressingType.ABSOLUTE, 4 );           // CPX $hhll
      sys.AddOpcode( "cpx", 0xE4, 1, AddressingType.ZEROPAGE, 3 );           // CPX $ll
      sys.AddOpcode( "cpx", 0xE0, 1, AddressingType.IMMEDIATE, 2 );          // CPX #$nn
      sys.AddOpcode( "cpy", 0xCC, 2, AddressingType.ABSOLUTE, 4 );           // CPY $hhll
      sys.AddOpcode( "cpy", 0xC4, 1, AddressingType.ZEROPAGE, 3 );           // CPY $ll
      sys.AddOpcode( "cpy", 0xC0, 1, AddressingType.IMMEDIATE, 2 );          // CPY #$nn
      sys.AddOpcode( "dec", 0xCE, 2, AddressingType.ABSOLUTE, 6 );           // DEC $hhll
      sys.AddOpcode( "dec", 0xDE, 2, AddressingType.ABSOLUTE_X, 7 );         // DEC $hhll, X
      sys.AddOpcode( "dec", 0xC6, 1, AddressingType.ZEROPAGE, 5 );           // DEC $ll
      sys.AddOpcode( "dec", 0xD6, 1, AddressingType.ZEROPAGE_X, 6 );         // DEC $ll, X
      sys.AddOpcode( "dex", 0xCA, 0, AddressingType.IMPLICIT, 2 );           // DEX
      sys.AddOpcode( "dey", 0x88, 0, AddressingType.IMPLICIT, 2 );           // DEY
      sys.AddOpcode( "eor", 0x4D, 2, AddressingType.ABSOLUTE, 4 );           // EOR $hhll
      sys.AddOpcode( "eor", 0x5D, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // EOR $hhll, X
      sys.AddOpcode( "eor", 0x59, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // EOR $hhll, Y
      sys.AddOpcode( "eor", 0x45, 1, AddressingType.ZEROPAGE, 3 );           // EOR $ll
      sys.AddOpcode( "eor", 0x55, 1, AddressingType.ZEROPAGE_X, 4 );         // EOR $ll, X
      sys.AddOpcode( "eor", 0x51, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // EOR ($ll), Y
      sys.AddOpcode( "eor", 0x41, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // EOR ($ll,X)
      sys.AddOpcode( "eor", 0x49, 1, AddressingType.IMMEDIATE, 2 );          // EOR #$nn
      sys.AddOpcode( "inc", 0xEE, 2, AddressingType.ABSOLUTE, 6 );           // INC $hhll
      sys.AddOpcode( "inc", 0xFE, 2, AddressingType.ABSOLUTE_X, 7 );         // INC $hhll, X
      sys.AddOpcode( "inc", 0xE6, 1, AddressingType.ZEROPAGE, 5 );           // INC $ll
      sys.AddOpcode( "inc", 0xF6, 1, AddressingType.ZEROPAGE_X, 6 );         // INC $ll, X
      sys.AddOpcode( "inx", 0xE8, 0, AddressingType.IMPLICIT, 2 );           // INX
      sys.AddOpcode( "iny", 0xC8, 0, AddressingType.IMPLICIT, 2 );           // INY
      sys.AddOpcode( "jmp", 0x4C, 2, AddressingType.ABSOLUTE, 3 );           // JMP $hhll
      sys.AddOpcode( "jmp", 0x6C, 2, AddressingType.INDIRECT, 6 );           // JMP ($hhll)
      sys.AddOpcode( "jsr", 0x20, 2, AddressingType.ABSOLUTE, 6 );           // JSR $hhll
      sys.AddOpcode( "lda", 0xAD, 2, AddressingType.ABSOLUTE, 4 );           // LDA $hhll
      sys.AddOpcode( "lda", 0xBD, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // LDA $hhll, X
      sys.AddOpcode( "lda", 0xB9, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // LDA $hhll, Y
      sys.AddOpcode( "lda", 0xA5, 1, AddressingType.ZEROPAGE, 3 );           // LDA $ll
      sys.AddOpcode( "lda", 0xB5, 1, AddressingType.ZEROPAGE_X, 4 );         // LDA $ll, X
      sys.AddOpcode( "lda", 0xB1, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // LDA ($ll), Y
      sys.AddOpcode( "lda", 0xA1, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // LDA ($ll,X)
      sys.AddOpcode( "lda", 0xA9, 1, AddressingType.IMMEDIATE, 2 );          // LDA #$nn
      sys.AddOpcode( "ldx", 0xAE, 2, AddressingType.ABSOLUTE, 4 );           // LDX $hhll
      sys.AddOpcode( "ldx", 0xBE, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // LDX $hhll, Y
      sys.AddOpcode( "ldx", 0xA6, 1, AddressingType.ZEROPAGE, 3 );           // LDX $ll
      sys.AddOpcode( "ldx", 0xB6, 1, AddressingType.ZEROPAGE_Y, 4 );         // LDX $ll, Y
      sys.AddOpcode( "ldx", 0xA2, 1, AddressingType.IMMEDIATE, 2 );          // LDX #$nn
      sys.AddOpcode( "ldy", 0xAC, 2, AddressingType.ABSOLUTE, 4 );           // LDY $hhll
      sys.AddOpcode( "ldy", 0xBC, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // LDY $hhll, X
      sys.AddOpcode( "ldy", 0xA4, 1, AddressingType.ZEROPAGE, 3 );           // LDY $ll
      sys.AddOpcode( "ldy", 0xB4, 1, AddressingType.ZEROPAGE_X, 4 );         // LDY $ll, X
      sys.AddOpcode( "ldy", 0xA0, 1, AddressingType.IMMEDIATE, 2 );          // LDY #$nn
      sys.AddOpcode( "lsr", 0x4A, 0, AddressingType.IMPLICIT, 2 );           // LSR
      sys.AddOpcode( "lsr", 0x4E, 2, AddressingType.ABSOLUTE, 6 );           // LSR $hhll
      sys.AddOpcode( "lsr", 0x5E, 2, AddressingType.ABSOLUTE_X, 6, 1 );      // LSR $hhll, X
      sys.AddOpcode( "lsr", 0x46, 1, AddressingType.ZEROPAGE, 5 );           // LSR $ll
      sys.AddOpcode( "lsr", 0x56, 1, AddressingType.ZEROPAGE_X, 6 );         // LSR $ll, X
      sys.AddOpcode( "eom", 0xEA, 0, AddressingType.IMPLICIT, 2 );           // eom
      sys.AddOpcode( "ora", 0x0D, 2, AddressingType.ABSOLUTE, 4 );           // ORA $hhll
      sys.AddOpcode( "ora", 0x1D, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // ORA $hhll, X
      sys.AddOpcode( "ora", 0x19, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // ORA $hhll, Y
      sys.AddOpcode( "ora", 0x05, 1, AddressingType.ZEROPAGE, 3 );           // ORA $ll
      sys.AddOpcode( "ora", 0x15, 1, AddressingType.ZEROPAGE_X, 4 );         // ORA $ll, X
      sys.AddOpcode( "ora", 0x11, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // ORA ($ll), Y
      sys.AddOpcode( "ora", 0x01, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // ORA ($ll,X)
      sys.AddOpcode( "ora", 0x09, 1, AddressingType.IMMEDIATE, 2 );          // ORA #$nn
      sys.AddOpcode( "pha", 0x48, 0, AddressingType.IMPLICIT, 3 );           // PHA
      sys.AddOpcode( "php", 0x08, 0, AddressingType.IMPLICIT, 3 );           // PHP
      sys.AddOpcode( "pla", 0x68, 0, AddressingType.IMPLICIT, 4 );           // PLA
      sys.AddOpcode( "plp", 0x28, 0, AddressingType.IMPLICIT, 4 );           // PLP
      sys.AddOpcode( "rol", 0x2A, 0, AddressingType.IMPLICIT, 2 );           // ROL
      sys.AddOpcode( "rol", 0x2E, 2, AddressingType.ABSOLUTE, 6 );           // ROL $hhll
      sys.AddOpcode( "rol", 0x3E, 2, AddressingType.ABSOLUTE_X, 6, 1 );      // ROL $hhll, X
      sys.AddOpcode( "rol", 0x26, 1, AddressingType.ZEROPAGE, 5 );           // ROL $ll
      sys.AddOpcode( "rol", 0x36, 1, AddressingType.ZEROPAGE_X, 6 );         // ROL $ll, X
      sys.AddOpcode( "ror", 0x6A, 0, AddressingType.IMPLICIT, 2 );           // ROR
      sys.AddOpcode( "ror", 0x6E, 2, AddressingType.ABSOLUTE, 6 );           // ROR $hhll
      sys.AddOpcode( "ror", 0x7E, 2, AddressingType.ABSOLUTE_X, 6, 1 );      // ROR $hhll, X
      sys.AddOpcode( "ror", 0x66, 1, AddressingType.ZEROPAGE, 5 );           // ROR $ll
      sys.AddOpcode( "ror", 0x76, 1, AddressingType.ZEROPAGE_X, 6 );         // ROR $ll, X
      sys.AddOpcode( "rti", 0x40, 0, AddressingType.IMPLICIT, 6 );           // RTI
      sys.AddOpcode( "rts", 0x60, 0, AddressingType.IMPLICIT, 6 );           // RTS
      sys.AddOpcode( "sbc", 0xED, 2, AddressingType.ABSOLUTE, 4, 1 );        // SBC $hhll
      sys.AddOpcode( "sbc", 0xFD, 2, AddressingType.ABSOLUTE_X, 4, 1, 1, 0 );// SBC $hhll, X
      sys.AddOpcode( "sbc", 0xF9, 2, AddressingType.ABSOLUTE_Y, 4, 1, 1, 0 );// SBC $hhll, Y
      sys.AddOpcode( "sbc", 0xE5, 1, AddressingType.ZEROPAGE, 3, 1 );        // SBC $ll
      sys.AddOpcode( "sbc", 0xF5, 1, AddressingType.ZEROPAGE_X, 4, 1 );      // SBC $ll, X
      sys.AddOpcode( "sbc", 0xF1, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1, 1, 0 );// SBC ($ll), Y
      sys.AddOpcode( "sbc", 0xE1, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6, 1 );      // SBC ($ll,X)
      sys.AddOpcode( "sbc", 0xE9, 1, AddressingType.IMMEDIATE, 2, 1 );       // SBC #$nn
      sys.AddOpcode( "sec", 0x38, 0, AddressingType.IMPLICIT, 2 );           // SEC
      sys.AddOpcode( "sed", 0xF8, 0, AddressingType.IMPLICIT, 2 );           // SED
      sys.AddOpcode( "sei", 0x78, 0, AddressingType.IMPLICIT, 2 );           // SEI
      sys.AddOpcode( "sta", 0x8D, 2, AddressingType.ABSOLUTE, 4 );           // STA $hhll
      sys.AddOpcode( "sta", 0x9D, 2, AddressingType.ABSOLUTE_X, 5 );         // STA $hhll, X
      sys.AddOpcode( "sta", 0x99, 2, AddressingType.ABSOLUTE_Y, 5 );         // STA $hhll, Y
      sys.AddOpcode( "sta", 0x85, 1, AddressingType.ZEROPAGE, 3 );           // STA $ll
      sys.AddOpcode( "sta", 0x95, 1, AddressingType.ZEROPAGE_X, 4 );         // STA $ll, X
      sys.AddOpcode( "sta", 0x91, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 6 );         // STA ($ll), Y
      sys.AddOpcode( "sta", 0x81, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // STA ($ll,X)
      sys.AddOpcode( "stx", 0x8E, 2, AddressingType.ABSOLUTE, 4 );           // STX $hhll
      sys.AddOpcode( "stx", 0x86, 1, AddressingType.ZEROPAGE, 3 );           // STX $ll
      sys.AddOpcode( "stx", 0x96, 1, AddressingType.ZEROPAGE_Y, 4 );         // STX $ll, Y
      sys.AddOpcode( "sty", 0x8C, 2, AddressingType.ABSOLUTE, 4 );           // STY $hhll
      sys.AddOpcode( "sty", 0x84, 1, AddressingType.ZEROPAGE, 3 );           // STY $ll
      sys.AddOpcode( "sty", 0x94, 1, AddressingType.ZEROPAGE_X, 4 );         // STY $ll, X
      sys.AddOpcode( "tax", 0xAA, 0, AddressingType.IMPLICIT, 2 );           // TAX
      sys.AddOpcode( "tay", 0xA8, 0, AddressingType.IMPLICIT, 2 );           // TAY
      sys.AddOpcode( "tsx", 0xBA, 0, AddressingType.IMPLICIT, 2 );           // TSX
      sys.AddOpcode( "txa", 0x8A, 0, AddressingType.IMPLICIT, 2 );           // TXA
      sys.AddOpcode( "txs", 0x9A, 0, AddressingType.IMPLICIT, 2 );           // TXS
      sys.AddOpcode( "tya", 0x98, 0, AddressingType.IMPLICIT, 2 );           // TYA


      // pageboundary cycle add is WRONG, it's for decimal mode
      sys.AddOpcode( "adc", 0x72, 1, AddressingType.ZEROPAGE_INDIRECT_Z, 5, 1 ); // adc ($12),z
      sys.AddOpcode( "and", 0x32, 1, AddressingType.ZEROPAGE_INDIRECT_Z, 5 );    // and ($12),z
      sys.AddOpcode( "cmp", 0xd2, 1, AddressingType.ZEROPAGE_INDIRECT_Z, 5 );    // cmp ($12),z
      sys.AddOpcode( "eor", 0x52, 1, AddressingType.ZEROPAGE_INDIRECT_Z, 5 );    // eor ($12),z
      sys.AddOpcode( "lda", 0xb2, 1, AddressingType.ZEROPAGE_INDIRECT_Z, 5 );    // lda ($12),z
      sys.AddOpcode( "ora", 0x12, 1, AddressingType.ZEROPAGE_INDIRECT_Z, 5 );    // ora ($12),z
      sys.AddOpcode( "sbc", 0xf2, 1, AddressingType.ZEROPAGE_INDIRECT_Z, 5, 1 ); // sbc ($12),z
      sys.AddOpcode( "sta", 0x92, 1, AddressingType.ZEROPAGE_INDIRECT_Z, 5 );    // sta ($12),z

      sys.AddOpcode( "bit", 0x89, 1, AddressingType.IMMEDIATE, 2 );              // bit #$12
      sys.AddOpcode( "bit", 0x34, 1, AddressingType.ZEROPAGE_X, 4 );             // bit $12,x
      sys.AddOpcode( "bit", 0x3C, 2, AddressingType.ABSOLUTE_X, 4, 1 );          // bit $1234,x

      sys.AddOpcode( "dec", 0x3A, 0, AddressingType.IMPLICIT, 2 );               // dec
      sys.AddOpcode( "inc", 0x1A, 0, AddressingType.IMPLICIT, 2 );               // inc

      sys.AddOpcode( "jmp", 0x7C, 2, AddressingType.ABSOLUTE_INDIRECT_X, 6 );    // jmp ($1234,x)

      sys.AddOpcode( "bra", 0x80, 1, AddressingType.RELATIVE, 3, 1 );            // bra label

      sys.AddOpcode( "phx", 0xDA, 0, AddressingType.IMPLICIT, 3 );               // phx
      sys.AddOpcode( "phy", 0x5A, 0, AddressingType.IMPLICIT, 3 );               // phy
      sys.AddOpcode( "plx", 0xfA, 0, AddressingType.IMPLICIT, 4 );               // plx
      sys.AddOpcode( "ply", 0x7A, 0, AddressingType.IMPLICIT, 4 );               // ply

      sys.AddOpcode( "stz", 0x64, 1, AddressingType.ZEROPAGE, 3 );               // stz $12
      sys.AddOpcode( "stz", 0x74, 1, AddressingType.ZEROPAGE_X, 4 );             // stz $12,x
      sys.AddOpcode( "stz", 0x9c, 2, AddressingType.ABSOLUTE, 4 );               // stz $1234
      sys.AddOpcode( "stz", 0x9e, 2, AddressingType.ABSOLUTE_X, 5 );             // stz $1234,x

      sys.AddOpcode( "trb", 0x14, 1, AddressingType.ZEROPAGE, 5 );               // trb $12
      sys.AddOpcode( "trb", 0x1C, 2, AddressingType.ABSOLUTE, 6 );               // trb $1234

      sys.AddOpcode( "tsb", 0x04, 1, AddressingType.ZEROPAGE, 5 );               // tsb $12
      sys.AddOpcode( "tsb", 0x0C, 2, AddressingType.ABSOLUTE, 6 );               // tsb $1234

      sys.AddOpcode( "bbr0", 0x0f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr0 $12,LABEL
      sys.AddOpcode( "bbr1", 0x1f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr1 $12,LABEL
      sys.AddOpcode( "bbr2", 0x2f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr2 $12,LABEL
      sys.AddOpcode( "bbr3", 0x3f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr3 $12,LABEL
      sys.AddOpcode( "bbr4", 0x4f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr4 $12,LABEL
      sys.AddOpcode( "bbr5", 0x5f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr5 $12,LABEL
      sys.AddOpcode( "bbr6", 0x6f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr6 $12,LABEL
      sys.AddOpcode( "bbr7", 0x7f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbr7 $12,LABEL
      sys.AddOpcode( "bbs0", 0x8f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs0 $12,LABEL
      sys.AddOpcode( "bbs1", 0x9f, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs1 $12,LABEL
      sys.AddOpcode( "bbs2", 0xaf, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs2 $12,LABEL
      sys.AddOpcode( "bbs3", 0xbf, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs3 $12,LABEL
      sys.AddOpcode( "bbs4", 0xcf, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs4 $12,LABEL
      sys.AddOpcode( "bbs5", 0xdf, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs5 $12,LABEL
      sys.AddOpcode( "bbs6", 0xef, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs6 $12,LABEL
      sys.AddOpcode( "bbs7", 0xff, 2, AddressingType.ZEROPAGE_RELATIVE, 5 );     // bbs7 $12,LABEL

      sys.AddOpcode( "rmb0", 0x07, 1, AddressingType.ZEROPAGE, 5 );     // rmb0 $12
      sys.AddOpcode( "rmb1", 0x17, 1, AddressingType.ZEROPAGE, 5 );     // rmb1 $12
      sys.AddOpcode( "rmb2", 0x27, 1, AddressingType.ZEROPAGE, 5 );     // rmb2 $12
      sys.AddOpcode( "rmb3", 0x37, 1, AddressingType.ZEROPAGE, 5 );     // rmb3 $12
      sys.AddOpcode( "rmb4", 0x47, 1, AddressingType.ZEROPAGE, 5 );     // rmb4 $12
      sys.AddOpcode( "rmb5", 0x57, 1, AddressingType.ZEROPAGE, 5 );     // rmb5 $12
      sys.AddOpcode( "rmb6", 0x67, 1, AddressingType.ZEROPAGE, 5 );     // rmb6 $12
      sys.AddOpcode( "rmb7", 0x77, 1, AddressingType.ZEROPAGE, 5 );     // rmb7 $12
      sys.AddOpcode( "smb0", 0x87, 1, AddressingType.ZEROPAGE, 5 );     // smb0 $12
      sys.AddOpcode( "smb1", 0x97, 1, AddressingType.ZEROPAGE, 5 );     // smb1 $12
      sys.AddOpcode( "smb2", 0xa7, 1, AddressingType.ZEROPAGE, 5 );     // smb2 $12
      sys.AddOpcode( "smb3", 0xb7, 1, AddressingType.ZEROPAGE, 5 );     // smb3 $12
      sys.AddOpcode( "smb4", 0xc7, 1, AddressingType.ZEROPAGE, 5 );     // smb4 $12
      sys.AddOpcode( "smb5", 0xd7, 1, AddressingType.ZEROPAGE, 5 );     // smb5 $12
      sys.AddOpcode( "smb6", 0xe7, 1, AddressingType.ZEROPAGE, 5 );     // smb6 $12
      sys.AddOpcode( "smb7", 0xf7, 1, AddressingType.ZEROPAGE, 5 );     // smb7 $12

      sys.AddOpcode( "asw", 0xcb, 2, AddressingType.ABSOLUTE, 7 );               // asw

      sys.AddOpcode( "asr", 0x43, 0, AddressingType.IMPLICIT, 2 );      // asr, asr a
      sys.AddOpcode( "asr", 0x44, 1, AddressingType.ZEROPAGE, 5 );      // asr $zp
      sys.AddOpcode( "asr", 0x54, 1, AddressingType.ZEROPAGE_X, 6 );    // asr $zp,x
      sys.AddOpcode( "lbcc", 0x93, 2, AddressingType.RELATIVE_16, 3, 1 );   // bcc $nnnn
      sys.AddOpcode( "lbcs", 0xb3, 2, AddressingType.RELATIVE_16, 3, 1 );   // bcs $nnnn
      sys.AddOpcode( "lbeq", 0xf3, 2, AddressingType.RELATIVE_16, 3, 1 );   // beq $nnnn
      sys.AddOpcode( "lbmi", 0x33, 2, AddressingType.RELATIVE_16, 3, 1 );   // bmi $nnnn
      sys.AddOpcode( "lbne", 0xd3, 2, AddressingType.RELATIVE_16, 3, 1 );   // bne $nnnn
      sys.AddOpcode( "lbpl", 0x13, 2, AddressingType.RELATIVE_16, 3, 1 );   // bpl $nnnn
      sys.AddOpcode( "lbra", 0x83, 2, AddressingType.RELATIVE_16, 4 );   // bra $nnnn
      sys.AddOpcode( "bsr", 0x63, 2, AddressingType.RELATIVE_16, 3, 1 );   // bsr $nnnn
      sys.AddOpcode( "lbvc", 0x53, 2, AddressingType.RELATIVE_16, 3, 1 );   // bvc $nnnn
      sys.AddOpcode( "lbvs", 0x73, 2, AddressingType.RELATIVE_16, 3, 1 );   // bvs $nnnn

      sys.AddOpcode( "cle", 0x02, 0, AddressingType.IMPLICIT, 2 );      // cle

      sys.AddOpcode( "cpz", 0xC2, 1, AddressingType.IMMEDIATE, 2 );     // cpz #xx
      sys.AddOpcode( "cpz", 0xD4, 1, AddressingType.ZEROPAGE, 3 );      // cpz $zp
      sys.AddOpcode( "cpz", 0xDC, 2, AddressingType.ABSOLUTE, 4 );      // cpz $nnnn

      sys.AddOpcode( "dew", 0xc3, 1, AddressingType.ZEROPAGE, 6 );      // dew $zp
      sys.AddOpcode( "dez", 0x3b, 0, AddressingType.IMPLICIT, 2 );      // dez
      sys.AddOpcode( "inw", 0xe3, 1, AddressingType.ZEROPAGE, 6 );      // inw $zp
      sys.AddOpcode( "inz", 0x1b, 0, AddressingType.IMPLICIT, 2 );      // inz

      sys.AddOpcode( "jsr", 0x22, 2, AddressingType.INDIRECT, 6 );   // jsr ($nnnn)
      sys.AddOpcode( "jsr", 0x23, 2, AddressingType.ABSOLUTE_INDIRECT_X, 6 );  // jsr ($nnnn,x)
      sys.AddOpcode( "lda", 0xE2, 1, AddressingType.ZEROPAGE_INDIRECT_SP_Y, 7 ); // lda ($zp,SP),y
      sys.AddOpcode( "ldz", 0xa3, 1, AddressingType.IMMEDIATE, 2 );      // ldz #$nn
      sys.AddOpcode( "ldz", 0xab, 2, AddressingType.ABSOLUTE, 4 );       // ldz $nnnn
      sys.AddOpcode( "ldz", 0xbb, 2, AddressingType.ABSOLUTE_X, 4, 1 );     // ldz $nnnn,x

      sys.AddOpcode( "map", 0x5C, 0, AddressingType.IMPLICIT, 2 );       // map
      sys.AddOpcode( "neg", 0x42, 0, AddressingType.IMPLICIT, 2 );       // neg

      sys.AddOpcode( "phw", 0xF4, 2, AddressingType.IMMEDIATE_16, 4 );   // phw #$nnnn
      sys.AddOpcode( "phw", 0xFC, 2, AddressingType.ABSOLUTE, 4 );       // phw $nnnn

      sys.AddOpcode( "phz", 0xdb, 0, AddressingType.IMPLICIT, 3 );    // phz
      sys.AddOpcode( "plz", 0xfb, 0, AddressingType.IMPLICIT, 4 );    // plz

      sys.AddOpcode( "row", 0xeb, 2, AddressingType.ABSOLUTE, 7 );       // row $nnnn
      sys.AddOpcode( "rtn", 0x62, 1, AddressingType.IMMEDIATE, 6 );      // rtn #$nn

      sys.AddOpcode( "see", 0x03, 0, AddressingType.IMPLICIT, 2 );       // see

      sys.AddOpcode( "sta", 0x82, 1, AddressingType.ZEROPAGE_INDIRECT_SP_Y, 7 ); // sta ($zp,SP),y
      sys.AddOpcode( "stx", 0x9b, 2, AddressingType.ABSOLUTE_Y, 5 );     // stx $nnnn,y
      sys.AddOpcode( "sty", 0x8b, 2, AddressingType.ABSOLUTE_X, 5 );     // sty $nnnn,x
      sys.AddOpcode( "tab", 0x5b, 0, AddressingType.IMPLICIT, 2 );       // tab
      sys.AddOpcode( "taz", 0x4b, 0, AddressingType.IMPLICIT, 2 );       // taz
      sys.AddOpcode( "tba", 0x7b, 0, AddressingType.IMPLICIT, 2 );       // tba
      sys.AddOpcode( "tsy", 0x0b, 0, AddressingType.IMPLICIT, 2 );       // tsy
      sys.AddOpcode( "tys", 0x2b, 0, AddressingType.IMPLICIT, 2 );       // tys
      sys.AddOpcode( "tza", 0x6b, 0, AddressingType.IMPLICIT, 2 );       // tza

      // quad variants -> override other values, so must be added after all regular ones
      sys.AddOpcode( "orq", 0x05, 1, AddressingType.ZEROPAGE, 3 ).NumNopsToPrefix = 2;           // ORQ $ll
      sys.AddOpcode( "aslq", 0x06, 1, AddressingType.ZEROPAGE, 5 ).NumNopsToPrefix = 2;          // ASLQ $ll
      sys.AddOpcode( "aslq", 0x0a, 0, AddressingType.IMPLICIT, 2 ).NumNopsToPrefix = 2;          // ASLQ
      sys.AddOpcode( "orq", 0x0D, 2, AddressingType.ABSOLUTE, 4 ).NumNopsToPrefix = 2;           // ORQ $hhll
      sys.AddOpcode( "aslq", 0x0e, 2, AddressingType.ABSOLUTE, 6 ).NumNopsToPrefix = 2;          // ASLQ $hhll
      sys.AddOpcode( "orq", 0x12, 1, AddressingType.ZEROPAGE_INDIRECT, 5 ).NumNopsToPrefix = 2;  // orq ($12)
      sys.AddOpcode( "aslq", 0x16, 1, AddressingType.ZEROPAGE_X, 6 ).NumNopsToPrefix = 2;        // ASLQ $ll, X
      sys.AddOpcode( "inq", 0x1A, 0, AddressingType.IMPLICIT, 2 ).NumNopsToPrefix = 2;           // inq
      sys.AddOpcode( "aslq", 0x1e, 2, AddressingType.ABSOLUTE_X, 6, 1 ).NumNopsToPrefix = 2;     // ASLQ $hhll, X
      sys.AddOpcode( "bitq", 0x24, 1, AddressingType.ZEROPAGE, 3 ).NumNopsToPrefix = 2;          // BITQ $ll
      sys.AddOpcode( "andq", 0x25, 1, AddressingType.ZEROPAGE, 3 ).NumNopsToPrefix = 2;          // ANDQ $ll
      sys.AddOpcode( "rolq", 0x26, 1, AddressingType.ZEROPAGE, 5 ).NumNopsToPrefix = 2;          // ROLQ $ll
      sys.AddOpcode( "rolq", 0x2A, 0, AddressingType.IMPLICIT, 2 ).NumNopsToPrefix = 2;          // ROLQ
      sys.AddOpcode( "bitq", 0x2c, 2, AddressingType.ABSOLUTE, 4 ).NumNopsToPrefix = 2;          // BITQ $hhll
      sys.AddOpcode( "andq", 0x2d, 2, AddressingType.ABSOLUTE, 4 ).NumNopsToPrefix = 2;          // ANDQ $hhll
      sys.AddOpcode( "rolq", 0x2E, 2, AddressingType.ABSOLUTE, 6 ).NumNopsToPrefix = 2;          // ROLQ $hhll
      sys.AddOpcode( "andq", 0x32, 1, AddressingType.ZEROPAGE_INDIRECT, 5 ).NumNopsToPrefix = 2; // andq ($12)
      sys.AddOpcode( "rolq", 0x36, 1, AddressingType.ZEROPAGE_X, 6 ).NumNopsToPrefix = 2;        // ROLQ $ll, X
      sys.AddOpcode( "deq", 0x3A, 0, AddressingType.IMPLICIT, 2 ).NumNopsToPrefix = 2;           // deq
      sys.AddOpcode( "rolq", 0x3E, 2, AddressingType.ABSOLUTE_X, 6, 1 ).NumNopsToPrefix = 2;     // ROLQ $hhll, X
      sys.AddOpcode( "asrq", 0x43, 0, AddressingType.IMPLICIT, 2 ).NumNopsToPrefix = 2;          // asrq, asrq a
      sys.AddOpcode( "asrq", 0x44, 1, AddressingType.ZEROPAGE, 5 ).NumNopsToPrefix = 2;          // asrq $zp
      sys.AddOpcode( "eorq", 0x45, 1, AddressingType.ZEROPAGE, 3 ).NumNopsToPrefix = 2;           // EORQ $ll
      sys.AddOpcode( "lsrq", 0x46, 1, AddressingType.ZEROPAGE, 5 ).NumNopsToPrefix = 2;           // LSRQ $ll
      sys.AddOpcode( "lsrq", 0x4A, 0, AddressingType.IMPLICIT, 2 ).NumNopsToPrefix = 2;           // LSRQ
      sys.AddOpcode( "eorq", 0x4D, 2, AddressingType.ABSOLUTE, 4 ).NumNopsToPrefix = 2;           // EORQ $hhll
      sys.AddOpcode( "lsrq", 0x4E, 2, AddressingType.ABSOLUTE, 6 ).NumNopsToPrefix = 2;           // LSRQ $hhll
      sys.AddOpcode( "eorq", 0x52, 1, AddressingType.ZEROPAGE_INDIRECT, 5 ).NumNopsToPrefix = 2;  // eorq ($12),z
      sys.AddOpcode( "asrq", 0x54, 1, AddressingType.ZEROPAGE_X, 6 ).NumNopsToPrefix = 2;         // asrq $zp,x
      sys.AddOpcode( "lsrq", 0x56, 1, AddressingType.ZEROPAGE_X, 6 ).NumNopsToPrefix = 2;         // LSRQ $ll, X
      sys.AddOpcode( "lsrq", 0x5E, 2, AddressingType.ABSOLUTE_X, 6, 1 ).NumNopsToPrefix = 2;      // LSRQ $hhll, X
      sys.AddOpcode( "adcq", 0x65, 1, AddressingType.ZEROPAGE, 3, 1, 1, 0 ).NumNopsToPrefix = 2;  // ADCQ $ll
      sys.AddOpcode( "rorq", 0x66, 1, AddressingType.ZEROPAGE, 5 ).NumNopsToPrefix = 2;           // RORQ $ll
      sys.AddOpcode( "rorq", 0x6A, 0, AddressingType.IMPLICIT, 2 ).NumNopsToPrefix = 2;           // RORQ
      sys.AddOpcode( "adcq", 0x6d, 2, AddressingType.ABSOLUTE, 4, 1 ).NumNopsToPrefix = 2;        // ADCQ $hhll
      sys.AddOpcode( "rorq", 0x6E, 2, AddressingType.ABSOLUTE, 6 ).NumNopsToPrefix = 2;           // RORQ $hhll
      sys.AddOpcode( "adcq", 0x72, 1, AddressingType.ZEROPAGE_INDIRECT, 5, 1 ).NumNopsToPrefix = 2; // adcq ($12)
      sys.AddOpcode( "rorq", 0x76, 1, AddressingType.ZEROPAGE_X, 6 ).NumNopsToPrefix = 2;         // RORQ $ll, X
      sys.AddOpcode( "rorq", 0x7E, 2, AddressingType.ABSOLUTE_X, 6, 1 ).NumNopsToPrefix = 2;      // RORQ $hhll, X
      sys.AddOpcode( "stq", 0x85, 1, AddressingType.ZEROPAGE, 3 ).NumNopsToPrefix = 2;            // STQ $ll
      sys.AddOpcode( "stq", 0x8D, 2, AddressingType.ABSOLUTE, 4 ).NumNopsToPrefix = 2;            // STQ $hhll
      sys.AddOpcode( "stq", 0x92, 1, AddressingType.ZEROPAGE_INDIRECT, 5 ).NumNopsToPrefix = 2;   // stq ($12)
      sys.AddOpcode( "ldq", 0xA5, 1, AddressingType.ZEROPAGE, 3 ).NumNopsToPrefix = 2;            // LDq $ll
      sys.AddOpcode( "ldq", 0xAD, 2, AddressingType.ABSOLUTE, 4 ).NumNopsToPrefix = 2;            // LDq $hhll
      sys.AddOpcode( "ldq", 0xB1, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 ).NumNopsToPrefix = 2; // LDQ ($ll), Y
      sys.AddOpcode( "ldq", 0xb2, 1, AddressingType.ZEROPAGE_INDIRECT, 5 ).NumNopsToPrefix = 2;   // ldq ($12)
      sys.AddOpcode( "ldq", 0xB5, 1, AddressingType.ZEROPAGE_X, 4 ).NumNopsToPrefix = 2;          // LDQ $ll, X
      sys.AddOpcode( "ldq", 0xB9, 2, AddressingType.ABSOLUTE_Y, 4, 1 ).NumNopsToPrefix = 2;       // LDQ $hhll, Y
      sys.AddOpcode( "ldq", 0xBD, 2, AddressingType.ABSOLUTE_X, 4, 1 ).NumNopsToPrefix = 2;       // LDQ $hhll, X
      sys.AddOpcode( "cpq", 0xC5, 1, AddressingType.ZEROPAGE, 3 ).NumNopsToPrefix = 2;            // CPQ $ll
      sys.AddOpcode( "deq", 0xC6, 1, AddressingType.ZEROPAGE, 5 ).NumNopsToPrefix = 2;            // DEQ $ll
      sys.AddOpcode( "cpq", 0xCD, 2, AddressingType.ABSOLUTE, 4 ).NumNopsToPrefix = 2;            // CPQ $hhll
      sys.AddOpcode( "deq", 0xCE, 2, AddressingType.ABSOLUTE, 6 ).NumNopsToPrefix = 2;            // DEQ $hhll
      sys.AddOpcode( "cpq", 0xd2, 1, AddressingType.ZEROPAGE_INDIRECT, 5 ).NumNopsToPrefix = 2;   // cpq ($12)
      sys.AddOpcode( "deq", 0xD6, 1, AddressingType.ZEROPAGE_X, 6 ).NumNopsToPrefix = 2;          // DEQ $ll, X
      sys.AddOpcode( "deq", 0xDE, 2, AddressingType.ABSOLUTE_X, 7 ).NumNopsToPrefix = 2;          // DEQ $hhll, X
      sys.AddOpcode( "ldq", 0xE2, 1, AddressingType.ZEROPAGE_INDIRECT_SP_Y, 7 ).NumNopsToPrefix = 2; // ldq ($zp,SP),y
      sys.AddOpcode( "sbcq", 0xE5, 1, AddressingType.ZEROPAGE, 3, 1 ).NumNopsToPrefix = 2;        // SBCQ $ll
      sys.AddOpcode( "inq", 0xE6, 1, AddressingType.ZEROPAGE, 5 ).NumNopsToPrefix = 2;            // INQ $ll
      sys.AddOpcode( "sbcq", 0xED, 2, AddressingType.ABSOLUTE, 4, 1 ).NumNopsToPrefix = 2;        // SBCQ $hhll
      sys.AddOpcode( "inq", 0xEE, 2, AddressingType.ABSOLUTE, 6 ).NumNopsToPrefix = 2;            // INQ $hhll
      sys.AddOpcode( "sbcq", 0xf2, 1, AddressingType.ZEROPAGE_INDIRECT, 5, 1 ).NumNopsToPrefix = 2; // sbcq ($12)
      sys.AddOpcode( "inq", 0xF6, 1, AddressingType.ZEROPAGE_X, 6 ).NumNopsToPrefix = 2;          // INQ $ll, X
      sys.AddOpcode( "inq", 0xFE, 2, AddressingType.ABSOLUTE_X, 7 ).NumNopsToPrefix = 2;          // INQ $hhll, X

      return sys;
    }

  }
}
