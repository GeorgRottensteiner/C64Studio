using System;
using System.Collections.Generic;
using System.Text;
using static Tiny64.Opcode;

namespace Tiny64
{
  public partial class Processor
  {
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
      sys.AddOpcode( "adc", 0x69, 1, AddressingType.IMMEDIATE_ACCU, 2, 1 );       // ADC #$nn
      sys.AddOpcode( "and", 0x2d, 2, AddressingType.ABSOLUTE, 4 );           // AND $hhll
      sys.AddOpcode( "and", 0x3d, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // AND $hhll, X
      sys.AddOpcode( "and", 0x39, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // AND $hhll, Y
      sys.AddOpcode( "and", 0x25, 1, AddressingType.ZEROPAGE, 3 );           // AND $ll
      sys.AddOpcode( "and", 0x35, 1, AddressingType.ZEROPAGE_X, 4 );         // AND $ll, X
      sys.AddOpcode( "and", 0x31, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // AND ($ll), Y
      sys.AddOpcode( "and", 0x21, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // AND ($ll,X)
      sys.AddOpcode( "and", 0x29, 1, AddressingType.IMMEDIATE_ACCU, 2 );          // AND #$nn
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
      sys.AddOpcode( "cmp", 0xC9, 1, AddressingType.IMMEDIATE_ACCU, 2 );          // CMP #$nn
      sys.AddOpcode( "cpx", 0xEC, 2, AddressingType.ABSOLUTE, 4 );           // CPX $hhll
      sys.AddOpcode( "cpx", 0xE4, 1, AddressingType.ZEROPAGE, 3 );           // CPX $ll
      sys.AddOpcode( "cpx", 0xE0, 1, AddressingType.IMMEDIATE_REGISTER, 2 );          // CPX #$nn
      sys.AddOpcode( "cpy", 0xCC, 2, AddressingType.ABSOLUTE, 4 );           // CPY $hhll
      sys.AddOpcode( "cpy", 0xC4, 1, AddressingType.ZEROPAGE, 3 );           // CPY $ll
      sys.AddOpcode( "cpy", 0xC0, 1, AddressingType.IMMEDIATE_REGISTER, 2 );          // CPY #$nn
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
      sys.AddOpcode( "eor", 0x49, 1, AddressingType.IMMEDIATE_ACCU, 2 );          // EOR #$nn
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
      sys.AddOpcode( "lda", 0xA9, 1, AddressingType.IMMEDIATE_ACCU, 2 );          // LDA #$nn
      sys.AddOpcode( "ldx", 0xAE, 2, AddressingType.ABSOLUTE, 4 );           // LDX $hhll
      sys.AddOpcode( "ldx", 0xBE, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // LDX $hhll, Y
      sys.AddOpcode( "ldx", 0xA6, 1, AddressingType.ZEROPAGE, 3 );           // LDX $ll
      sys.AddOpcode( "ldx", 0xB6, 1, AddressingType.ZEROPAGE_Y, 4 );         // LDX $ll, Y
      sys.AddOpcode( "ldx", 0xA2, 1, AddressingType.IMMEDIATE_REGISTER, 2 );          // LDX #$nn
      sys.AddOpcode( "ldy", 0xAC, 2, AddressingType.ABSOLUTE, 4 );           // LDY $hhll
      sys.AddOpcode( "ldy", 0xBC, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // LDY $hhll, X
      sys.AddOpcode( "ldy", 0xA4, 1, AddressingType.ZEROPAGE, 3 );           // LDY $ll
      sys.AddOpcode( "ldy", 0xB4, 1, AddressingType.ZEROPAGE_X, 4 );         // LDY $ll, X
      sys.AddOpcode( "ldy", 0xA0, 1, AddressingType.IMMEDIATE_REGISTER, 2 );          // LDY #$nn
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
      sys.AddOpcode( "ora", 0x09, 1, AddressingType.IMMEDIATE_ACCU, 2 );          // ORA #$nn
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
      sys.AddOpcode( "sbc", 0xE9, 1, AddressingType.IMMEDIATE_ACCU, 2, 1 );       // SBC #$nn
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

      sys.AddOpcode( "bit", 0x89, 1, AddressingType.IMMEDIATE_ACCU, 2 );              // bit #$12
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

      sys.AddOpcode( "cpz", 0xC2, 1, AddressingType.IMMEDIATE_REGISTER, 2 );     // cpz #xx
      sys.AddOpcode( "cpz", 0xD4, 1, AddressingType.ZEROPAGE, 3 );      // cpz $zp
      sys.AddOpcode( "cpz", 0xDC, 2, AddressingType.ABSOLUTE, 4 );      // cpz $nnnn

      sys.AddOpcode( "dew", 0xc3, 1, AddressingType.ZEROPAGE, 6 );      // dew $zp
      sys.AddOpcode( "dez", 0x3b, 0, AddressingType.IMPLICIT, 2 );      // dez
      sys.AddOpcode( "inw", 0xe3, 1, AddressingType.ZEROPAGE, 6 );      // inw $zp
      sys.AddOpcode( "inz", 0x1b, 0, AddressingType.IMPLICIT, 2 );      // inz

      sys.AddOpcode( "jsr", 0x22, 2, AddressingType.INDIRECT, 6 );   // jsr ($nnnn)
      sys.AddOpcode( "jsr", 0x23, 2, AddressingType.ABSOLUTE_INDIRECT_X, 6 );  // jsr ($nnnn,x)
      sys.AddOpcode( "lda", 0xE2, 1, AddressingType.ZEROPAGE_INDIRECT_SP_Y, 7 ); // lda ($zp,S),y
      sys.AddOpcode( "ldz", 0xa3, 1, AddressingType.IMMEDIATE_REGISTER, 2 );      // ldz #$nn
      sys.AddOpcode( "ldz", 0xab, 2, AddressingType.ABSOLUTE, 4 );       // ldz $nnnn
      sys.AddOpcode( "ldz", 0xbb, 2, AddressingType.ABSOLUTE_X, 4, 1 );     // ldz $nnnn,x

      sys.AddOpcode( "aug", 0x5C, 0, AddressingType.IMPLICIT, 2 );       // aug (4-byte NOP RFU?), also called MAP
      sys.AddOpcode( "neg", 0x42, 0, AddressingType.IMPLICIT, 2 );       // neg

      sys.AddOpcode( "phw", 0xF4, 2, AddressingType.IMMEDIATE_16BIT, 4 );   // phw #$nnnn
      sys.AddOpcode( "phw", 0xFC, 2, AddressingType.ABSOLUTE, 4 );       // phw $nnnn

      sys.AddOpcode( "phz", 0xdb, 0, AddressingType.IMPLICIT, 3 );    // phz
      sys.AddOpcode( "plz", 0xfb, 0, AddressingType.IMPLICIT, 4 );    // plz

      sys.AddOpcode( "row", 0xeb, 2, AddressingType.ABSOLUTE, 7 );       // row $nnnn
      sys.AddOpcode( "rtn", 0x62, 1, AddressingType.IMMEDIATE_ACCU, 6 );      // rtn #$nn

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



  }
}
