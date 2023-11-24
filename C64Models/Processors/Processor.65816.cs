using System;
using System.Collections.Generic;
using System.Text;
using static Tiny64.Opcode;

namespace Tiny64
{
  public partial class Processor
  {
    public static Processor Create65816()
    {
      // 65C02 plus 65816 specifics
      var  sys = new Processor( "65816" );

      var empty = new List<ValidValue>();

      sys.AddOpcode( "brk", 0x00, 0, AddressingType.IMPLICIT, 7 );                // BRK
      sys.AddOpcode( "ora", 0x01, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );     // ORA ($ll,X)
      sys.AddOpcode( "cop", 0x02, 1, AddressingType.ZEROPAGE, 7 );                // COP $ll     s (functionally actually IMPLICIT, but has add. byte)
      sys.AddOpcode( "ora", 0x03, 1, AddressingType.STACK_RELATIVE, 4 );          // ora $ll, sp
      sys.AddOpcode( "tsb", 0x04, 1, AddressingType.ZEROPAGE, 5 );                // tsb $12
      sys.AddOpcode( "ora", 0x05, 1, AddressingType.ZEROPAGE, 3 );                // ORA $ll
      sys.AddOpcode( "asl", 0x06, 1, AddressingType.ZEROPAGE, 5 );                // ASL $ll
      sys.AddOpcode( "ora", 0x07, 1, AddressingType.ZEROPAGE_INDIRECT_LONG, 6 );  // ORA [$ll]
      sys.AddOpcode( "php", 0x08, 0, AddressingType.IMPLICIT, 3 );                // PHP
      sys.AddOpcode( "ora", 0x09, 1, AddressingType.IMMEDIATE_ACCU, 2 );               // ORA #$nn
      sys.AddOpcode( "asl", 0x0a, 0, AddressingType.IMPLICIT, 2 );                // ASL
      sys.AddOpcode( "phd", 0x0b, 0, AddressingType.IMPLICIT, 4 );                // PHD
      sys.AddOpcode( "tsb", 0x0C, 2, AddressingType.ABSOLUTE, 6 );                // tsb $1234
      sys.AddOpcode( "ora", 0x0D, 2, AddressingType.ABSOLUTE, 4 );                // ORA $hhll
      sys.AddOpcode( "asl", 0x0e, 2, AddressingType.ABSOLUTE, 6 );                // ASL $hhll
      sys.AddOpcode( "ora", 0x0f, 3, AddressingType.ABSOLUTE_LONG, 5 );           // ora $123456

      sys.AddOpcode( "bpl", 0x10, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );       // BPL $hhll
      sys.AddOpcode( "ora", 0x11, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );  // ORA ($ll), Y
      sys.AddOpcode( "ora", 0x12, 1, AddressingType.ZEROPAGE_INDIRECT, 5 );       // ora ($12)
      sys.AddOpcode( "ora", 0x13, 1, AddressingType.ZEROPAGE_INDIRECT_SP_Y, 7 );  // ORA ($ll,SP),Y
      sys.AddOpcode( "trb", 0x14, 1, AddressingType.ZEROPAGE, 5 );                // trb $12
      sys.AddOpcode( "ora", 0x15, 1, AddressingType.ZEROPAGE_X, 4 );              // ORA $ll, X
      sys.AddOpcode( "asl", 0x16, 1, AddressingType.ZEROPAGE_X, 6 );              // ASL $ll, X
      sys.AddOpcode( "ora", 0x17, 1, AddressingType.ZEROPAGE_INDIRECT_Y_LONG, 6 );// ORA ($ll), Y
      sys.AddOpcode( "clc", 0x18, 0, AddressingType.IMPLICIT, 2 );                // CLC
      sys.AddOpcode( "ora", 0x19, 2, AddressingType.ABSOLUTE_Y, 4, 1 );           // ORA $hhll, Y
      sys.AddOpcode( "inc", 0x1A, 0, AddressingType.IMPLICIT, 2 );                // INC
      sys.AddOpcode( "tcs", 0x1b, 0, AddressingType.IMPLICIT, 2 );                // TCS
      sys.AddOpcode( "trb", 0x1C, 2, AddressingType.ABSOLUTE, 6 );                // trb $1234
      sys.AddOpcode( "ora", 0x1D, 2, AddressingType.ABSOLUTE_X, 4, 1 );           // ORA $hhll, X
      sys.AddOpcode( "asl", 0x1e, 2, AddressingType.ABSOLUTE_X, 6, 1 );           // ASL $hhll, X
      sys.AddOpcode( "ora", 0x1f, 3, AddressingType.ABSOLUTE_LONG_X, 5 );         // ORA $hhmmll, X

      sys.AddOpcode( "jsr", 0x20, 2, AddressingType.ABSOLUTE, 6 );                // JSR $hhll
      sys.AddOpcode( "and", 0x21, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );     // AND ($ll,X)
      sys.AddOpcode( "jsr", 0x22, 3, AddressingType.ABSOLUTE_LONG, 8 );           // JSR $hhmmll
      sys.AddOpcode( "jsl", 0x22, 3, AddressingType.ABSOLUTE_LONG, 8 );           // JSR $hhmmll
      sys.AddOpcode( "and", 0x23, 1, AddressingType.STACK_RELATIVE, 4 );          // AND $ll, SP
      sys.AddOpcode( "bit", 0x24, 1, AddressingType.ZEROPAGE, 3 );                // BIT $ll
      sys.AddOpcode( "and", 0x25, 1, AddressingType.ZEROPAGE, 3 );                // AND $ll
      sys.AddOpcode( "rol", 0x26, 1, AddressingType.ZEROPAGE, 5 );                // ROL $ll
      sys.AddOpcode( "and", 0x27, 1, AddressingType.ZEROPAGE_INDIRECT_LONG, 6 );  // AND [$ll]
      sys.AddOpcode( "plp", 0x28, 0, AddressingType.IMPLICIT, 4 );                // PLP
      sys.AddOpcode( "and", 0x29, 1, AddressingType.IMMEDIATE_ACCU, 2 );               // AND #$nn
      sys.AddOpcode( "rol", 0x2A, 0, AddressingType.IMPLICIT, 2 );                // ROL
      sys.AddOpcode( "pld", 0x2b, 0, AddressingType.IMPLICIT, 5 );                // PLD
      sys.AddOpcode( "bit", 0x2c, 2, AddressingType.ABSOLUTE, 4 );                // BIT $hhll
      sys.AddOpcode( "and", 0x2d, 2, AddressingType.ABSOLUTE, 4 );                // AND $hhll
      sys.AddOpcode( "rol", 0x2E, 2, AddressingType.ABSOLUTE, 6 );                // ROL $hhll
      sys.AddOpcode( "and", 0x2f, 3, AddressingType.ABSOLUTE_LONG, 5 );           // AND $hhmmll

      sys.AddOpcode( "bmi", 0x30, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );       // BMI $hhll
      sys.AddOpcode( "and", 0x31, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // AND ($ll), Y
      sys.AddOpcode( "and", 0x32, 1, AddressingType.ZEROPAGE_INDIRECT, 5 );      // and ($12)
      sys.AddOpcode( "and", 0x33, 1, AddressingType.ZEROPAGE_INDIRECT_SP_Y, 7 );  // AND ($ll,SP),Y
      sys.AddOpcode( "bit", 0x34, 1, AddressingType.ZEROPAGE_X, 2 );             // bit $12,x
      sys.AddOpcode( "and", 0x35, 1, AddressingType.ZEROPAGE_X, 4 );         // AND $ll, X
      sys.AddOpcode( "rol", 0x36, 1, AddressingType.ZEROPAGE_X, 6 );         // ROL $ll, X
      sys.AddOpcode( "and", 0x37, 1, AddressingType.ZEROPAGE_INDIRECT_Y_LONG, 6 ); // AND [$ll],Y
      sys.AddOpcode( "sec", 0x38, 0, AddressingType.IMPLICIT, 2 );           // SEC
      sys.AddOpcode( "and", 0x39, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // AND $hhll, Y
      sys.AddOpcode( "dec", 0x3A, 0, AddressingType.IMPLICIT, 2 );               // dec
      sys.AddOpcode( "tsc", 0x3b, 0, AddressingType.IMPLICIT, 2 );                // TSC
      sys.AddOpcode( "bit", 0x3C, 2, AddressingType.ABSOLUTE_X, 4, 1 );          // bit $1234,x
      sys.AddOpcode( "and", 0x3d, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // AND $hhll, X
      sys.AddOpcode( "rol", 0x3E, 2, AddressingType.ABSOLUTE_X, 6, 1 );      // ROL $hhll, X
      sys.AddOpcode( "and", 0x3f, 3, AddressingType.ABSOLUTE_LONG_X, 5 );         // AND $hhmmll,X

      sys.AddOpcode( "rti", 0x40, 0, AddressingType.IMPLICIT, 6 );           // RTI
      sys.AddOpcode( "eor", 0x41, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // EOR ($ll,X)
      sys.AddOpcode( "wdm", 0x42, 0, AddressingType.IMPLICIT, 2 );                // WDM
      sys.AddOpcode( "eor", 0x43, 1, AddressingType.STACK_RELATIVE, 4 );          // EOR $ll,SP
      sys.AddOpcode( "mvp", 0x440000, 3, AddressingType.BLOCK_MOVE_XYC, 7 )         // MVP $ll, $ll
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT, empty, 8 ) } );

      sys.AddOpcode( "eor", 0x45, 1, AddressingType.ZEROPAGE, 3 );           // EOR $ll
      sys.AddOpcode( "lsr", 0x46, 1, AddressingType.ZEROPAGE, 5 );           // LSR $ll
      sys.AddOpcode( "eor", 0x47, 1, AddressingType.ZEROPAGE_INDIRECT_LONG, 6 );  // EOR [$ll]
      sys.AddOpcode( "pha", 0x48, 0, AddressingType.IMPLICIT, 3 );           // PHA
      sys.AddOpcode( "eor", 0x49, 1, AddressingType.IMMEDIATE_ACCU, 2 );          // EOR #$nn
      sys.AddOpcode( "lsr", 0x4A, 0, AddressingType.IMPLICIT, 2 );           // LSR
      sys.AddOpcode( "phk", 0x4b, 0, AddressingType.IMPLICIT, 3 );                // PHK
      sys.AddOpcode( "jmp", 0x4C, 2, AddressingType.ABSOLUTE, 3 );           // JMP $hhll
      sys.AddOpcode( "eor", 0x4D, 2, AddressingType.ABSOLUTE, 4 );           // EOR $hhll
      sys.AddOpcode( "lsr", 0x4E, 2, AddressingType.ABSOLUTE, 6 );           // LSR $hhll
      sys.AddOpcode( "eor", 0x4f, 3, AddressingType.ABSOLUTE_LONG, 5 );           // EOR $hhmmll

      sys.AddOpcode( "bvc", 0x50, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BVC $hhll
      sys.AddOpcode( "eor", 0x51, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // EOR ($ll), Y
      sys.AddOpcode( "eor", 0x52, 1, AddressingType.ZEROPAGE_INDIRECT, 5 );      // eor ($12)
      sys.AddOpcode( "eor", 0x53, 1, AddressingType.ZEROPAGE_INDIRECT_SP_Y, 7 );      // EOR ($ll,SP),Y
      sys.AddOpcode( "mvn", 0x540000, 3, AddressingType.BLOCK_MOVE_XYC, 7 )               // MVN $ll,$ll
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT, empty, 8 ) } );

      sys.AddOpcode( "eor", 0x55, 1, AddressingType.ZEROPAGE_X, 4 );         // EOR $ll, X
      sys.AddOpcode( "lsr", 0x56, 1, AddressingType.ZEROPAGE_X, 6 );         // LSR $ll, X
      sys.AddOpcode( "eor", 0x57, 1, AddressingType.ZEROPAGE_INDIRECT_Y_LONG, 6 );    // EOR [$ll],Y
      sys.AddOpcode( "cli", 0x58, 0, AddressingType.IMPLICIT, 2 );           // CLI
      sys.AddOpcode( "eor", 0x59, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // EOR $hhll, Y
      sys.AddOpcode( "phy", 0x5A, 0, AddressingType.IMPLICIT, 3 );               // phy
      sys.AddOpcode( "tcd", 0x5b, 0, AddressingType.IMPLICIT, 2 );                    // TCD
      sys.AddOpcode( "jmp", 0x5C, 3, AddressingType.ABSOLUTE_LONG, 4 );               // JMP $hhmmll
      sys.AddOpcode( "eor", 0x5D, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // EOR $hhll, X
      sys.AddOpcode( "lsr", 0x5E, 2, AddressingType.ABSOLUTE_X, 6, 1 );      // LSR $hhll, X
      sys.AddOpcode( "eor", 0x5f, 3, AddressingType.ABSOLUTE_LONG_X, 5 );             // EOR $hhmmll, X

      sys.AddOpcode( "rts", 0x60, 0, AddressingType.IMPLICIT, 6 );           // RTS
      sys.AddOpcode( "adc", 0x61, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6, 1 );      // ADC ($ll,X)
      sys.AddOpcode( "per", 0x62, 2, AddressingType.RELATIVE_16, 6 );                 // PER $hhll
      sys.AddOpcode( "adc", 0x63, 1, AddressingType.STACK_RELATIVE, 4 );              // ADC $ll,SP
      sys.AddOpcode( "stz", 0x64, 1, AddressingType.ZEROPAGE, 3 );                    // stz $12
      sys.AddOpcode( "adc", 0x65, 1, AddressingType.ZEROPAGE, 3, 1, 1, 0 );  // ADC $ll
      sys.AddOpcode( "ror", 0x66, 1, AddressingType.ZEROPAGE, 5 );           // ROR $ll
      sys.AddOpcode( "adc", 0x67, 1, AddressingType.ZEROPAGE_INDIRECT_LONG, 6 );      // ADC [$ll]
      sys.AddOpcode( "pla", 0x68, 0, AddressingType.IMPLICIT, 4 );           // PLA
      sys.AddOpcode( "adc", 0x69, 1, AddressingType.IMMEDIATE_ACCU, 2, 1 );       // ADC #$nn
      sys.AddOpcode( "ror", 0x6A, 0, AddressingType.IMPLICIT, 2 );           // ROR
      sys.AddOpcode( "rtl", 0x6b, 0, AddressingType.IMPLICIT, 6 );                    // RTL
      sys.AddOpcode( "jmp", 0x6C, 2, AddressingType.INDIRECT, 6 );           // JMP ($hhll)
      sys.AddOpcode( "adc", 0x6d, 2, AddressingType.ABSOLUTE, 4, 1 );        // ADC $hhll
      sys.AddOpcode( "ror", 0x6E, 2, AddressingType.ABSOLUTE, 6 );           // ROR $hhll
      sys.AddOpcode( "adc", 0x6f, 3, AddressingType.ABSOLUTE_LONG, 5 );               // ADC $hhmmll

      sys.AddOpcode( "bvs", 0x70, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BVS $hhll
      sys.AddOpcode( "adc", 0x71, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1, 1, 0 );// ADC ($ll), Y
      sys.AddOpcode( "adc", 0x72, 1, AddressingType.ZEROPAGE_INDIRECT, 5, 1 );   // adc ($12)
      sys.AddOpcode( "adc", 0x73, 1, AddressingType.ZEROPAGE_INDIRECT_SP_Y, 7 );      // ADC ($ll,SP),Y
      sys.AddOpcode( "stz", 0x74, 1, AddressingType.ZEROPAGE_X, 4 );             // stz $12,x
      sys.AddOpcode( "adc", 0x75, 1, AddressingType.ZEROPAGE_X, 4, 1 );      // ADC $ll, X
      sys.AddOpcode( "ror", 0x76, 1, AddressingType.ZEROPAGE_X, 6 );         // ROR $ll, X
      sys.AddOpcode( "adc", 0x77, 1, AddressingType.ZEROPAGE_INDIRECT_Y_LONG, 6 );    // ADC [$LL],Y
      sys.AddOpcode( "sei", 0x78, 0, AddressingType.IMPLICIT, 2 );           // SEI
      sys.AddOpcode( "adc", 0x79, 2, AddressingType.ABSOLUTE_Y, 4, 1, 1, 0 );// ADC $hhll, Y
      sys.AddOpcode( "ply", 0x7A, 0, AddressingType.IMPLICIT, 4 );               // ply
      sys.AddOpcode( "tdc", 0x7b, 0, AddressingType.IMPLICIT, 2 );                    // TDC
      sys.AddOpcode( "jmp", 0x7C, 2, AddressingType.ABSOLUTE_INDIRECT_X, 6 );    // jmp ($1234,x)
      sys.AddOpcode( "adc", 0x7d, 2, AddressingType.ABSOLUTE_X, 4, 1, 1, 0 );// ADC $hhll, X
      sys.AddOpcode( "ror", 0x7E, 2, AddressingType.ABSOLUTE_X, 6, 1 );      // ROR $hhll, X
      sys.AddOpcode( "adc", 0x7f, 3, AddressingType.ABSOLUTE_LONG_X, 5 );             // ADC $hhmmll,X

      sys.AddOpcode( "bra", 0x80, 1, AddressingType.RELATIVE, 3, 1 );            // bra label
      sys.AddOpcode( "sta", 0x81, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // STA ($ll,X)
      sys.AddOpcode( "brl", 0x82, 2, AddressingType.RELATIVE_16, 4 );                 // BRL $hhll
      sys.AddOpcode( "sta", 0x83, 1, AddressingType.STACK_RELATIVE, 4 );              // sta $ll,SP
      sys.AddOpcode( "sty", 0x84, 1, AddressingType.ZEROPAGE, 3 );           // STY $ll
      sys.AddOpcode( "sta", 0x85, 1, AddressingType.ZEROPAGE, 3 );           // STA $ll
      sys.AddOpcode( "stx", 0x86, 1, AddressingType.ZEROPAGE, 3 );           // STX $ll
      sys.AddOpcode( "sta", 0x87, 1, AddressingType.ZEROPAGE_INDIRECT_LONG, 2 );       // sta [$ll]
      sys.AddOpcode( "dey", 0x88, 0, AddressingType.IMPLICIT, 2 );           // DEY
      sys.AddOpcode( "bit", 0x89, 1, AddressingType.IMMEDIATE_ACCU, 2 );              // bit #$12
      sys.AddOpcode( "txa", 0x8A, 0, AddressingType.IMPLICIT, 2 );           // TXA
      sys.AddOpcode( "phb", 0x8b, 0, AddressingType.IMPLICIT, 3 );                    // PHB
      sys.AddOpcode( "sty", 0x8C, 2, AddressingType.ABSOLUTE, 4 );           // STY $hhll
      sys.AddOpcode( "sta", 0x8D, 2, AddressingType.ABSOLUTE, 4 );           // STA $hhll
      sys.AddOpcode( "stx", 0x8E, 2, AddressingType.ABSOLUTE, 4 );           // STX $hhll
      sys.AddOpcode( "sta", 0x8f, 3, AddressingType.ABSOLUTE_LONG, 5 );               // STA $hhmmll

      sys.AddOpcode( "bcc", 0x90, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BCC $hhll
      sys.AddOpcode( "sta", 0x91, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 6 );         // STA ($ll), Y
      sys.AddOpcode( "sta", 0x92, 1, AddressingType.ZEROPAGE_INDIRECT, 5 );      // sta ($12)
      sys.AddOpcode( "sta", 0x93, 1, AddressingType.ZEROPAGE_INDIRECT_SP_Y, 7 );      // STA ($ll,SP), Y
      sys.AddOpcode( "sty", 0x94, 1, AddressingType.ZEROPAGE_X, 4 );         // STY $ll, X
      sys.AddOpcode( "sta", 0x95, 1, AddressingType.ZEROPAGE_X, 4 );         // STA $ll, X
      sys.AddOpcode( "stx", 0x96, 1, AddressingType.ZEROPAGE_Y, 4 );         // STX $ll, Y
      sys.AddOpcode( "sta", 0x97, 1, AddressingType.ZEROPAGE_INDIRECT_Y_LONG, 6 );    // sta [$ll], Y
      sys.AddOpcode( "tya", 0x98, 0, AddressingType.IMPLICIT, 2 );           // TYA
      sys.AddOpcode( "sta", 0x99, 2, AddressingType.ABSOLUTE_Y, 5 );         // STA $hhll, Y
      sys.AddOpcode( "txs", 0x9A, 0, AddressingType.IMPLICIT, 2 );           // TXS
      sys.AddOpcode( "txy", 0x9b, 0, AddressingType.IMPLICIT, 2 );                    // TXY
      sys.AddOpcode( "stz", 0x9c, 2, AddressingType.ABSOLUTE, 4 );               // stz $1234
      sys.AddOpcode( "sta", 0x9D, 2, AddressingType.ABSOLUTE_X, 5 );         // STA $hhll, X
      sys.AddOpcode( "stz", 0x9e, 2, AddressingType.ABSOLUTE_X, 5 );             // stz $1234,x
      sys.AddOpcode( "sta", 0x9f, 3, AddressingType.ABSOLUTE_LONG_X, 5 );             // STA $hhmmll,X

      sys.AddOpcode( "ldy", 0xA0, 1, AddressingType.IMMEDIATE_REGISTER, 2 );          // LDY #$nn
      sys.AddOpcode( "lda", 0xA1, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // LDA ($ll,X)
      sys.AddOpcode( "ldx", 0xA2, 1, AddressingType.IMMEDIATE_REGISTER, 2 );          // LDX #$nn
      sys.AddOpcode( "lda", 0xa3, 1, AddressingType.STACK_RELATIVE, 4 );              // LDA $LL,SP
      sys.AddOpcode( "ldy", 0xA4, 1, AddressingType.ZEROPAGE, 3 );           // LDY $ll
      sys.AddOpcode( "lda", 0xA5, 1, AddressingType.ZEROPAGE, 3 );           // LDA $ll
      sys.AddOpcode( "ldx", 0xA6, 1, AddressingType.ZEROPAGE, 3 );           // LDX $ll
      sys.AddOpcode( "lda", 0xa7, 1, AddressingType.ZEROPAGE_INDIRECT_LONG, 6 );      // LDA [$ll]
      sys.AddOpcode( "tay", 0xA8, 0, AddressingType.IMPLICIT, 2 );           // TAY
      sys.AddOpcode( "lda", 0xA9, 1, AddressingType.IMMEDIATE_ACCU, 2 );          // LDA #$nn
      sys.AddOpcode( "tax", 0xAA, 0, AddressingType.IMPLICIT, 2 );           // TAX
      sys.AddOpcode( "plb", 0xab, 0, AddressingType.IMPLICIT, 4 );                    // PLB
      sys.AddOpcode( "ldy", 0xAC, 2, AddressingType.ABSOLUTE, 4 );           // LDY $hhll
      sys.AddOpcode( "lda", 0xAD, 2, AddressingType.ABSOLUTE, 4 );           // LDA $hhll
      sys.AddOpcode( "ldx", 0xAE, 2, AddressingType.ABSOLUTE, 4 );           // LDX $hhll
      sys.AddOpcode( "lda", 0xaf, 3, AddressingType.ABSOLUTE_LONG, 5 );               // LDA $hhmmll

      sys.AddOpcode( "bcs", 0xB0, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BCS $hhll
      sys.AddOpcode( "lda", 0xB1, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // LDA ($ll), Y
      sys.AddOpcode( "lda", 0xb2, 1, AddressingType.ZEROPAGE_INDIRECT, 5 );      // lda ($12)
      sys.AddOpcode( "lda", 0xb3, 1, AddressingType.ZEROPAGE_INDIRECT_SP_Y, 7 );      // LDA ($ll,SP),Y
      sys.AddOpcode( "ldy", 0xB4, 1, AddressingType.ZEROPAGE_X, 4 );         // LDY $ll, X
      sys.AddOpcode( "lda", 0xB5, 1, AddressingType.ZEROPAGE_X, 4 );         // LDA $ll, X
      sys.AddOpcode( "ldx", 0xB6, 1, AddressingType.ZEROPAGE_Y, 4 );         // LDX $ll, Y
      sys.AddOpcode( "lda", 0xb7, 1, AddressingType.ZEROPAGE_INDIRECT_Y_LONG, 6 );    // LDA [$ll],Y
      sys.AddOpcode( "clv", 0xB8, 0, AddressingType.IMPLICIT, 2 );           // CLV
      sys.AddOpcode( "lda", 0xB9, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // LDA $hhll, Y
      sys.AddOpcode( "tsx", 0xBA, 0, AddressingType.IMPLICIT, 2 );           // TSX
      sys.AddOpcode( "tyx", 0xbb, 0, AddressingType.IMPLICIT, 2 );                    // TYX
      sys.AddOpcode( "ldy", 0xBC, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // LDY $hhll, X
      sys.AddOpcode( "lda", 0xBD, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // LDA $hhll, X
      sys.AddOpcode( "ldx", 0xBE, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // LDX $hhll, Y
      sys.AddOpcode( "lda", 0xbf, 3, AddressingType.ABSOLUTE_LONG_X, 5 );             // LDA $hhmmll,X

      sys.AddOpcode( "cpy", 0xC0, 1, AddressingType.IMMEDIATE_REGISTER, 2 );          // CPY #$nn
      sys.AddOpcode( "cmp", 0xC1, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6 );         // CMP ($ll,X)
      sys.AddOpcode( "rep", 0xC2, 1, AddressingType.IMMEDIATE_8BIT, 3 );              // REP #$ll
      sys.AddOpcode( "cmp", 0xc3, 1, AddressingType.STACK_RELATIVE, 4 );              // CMP #$ll,SP
      sys.AddOpcode( "cpy", 0xC4, 1, AddressingType.ZEROPAGE, 3 );           // CPY $ll
      sys.AddOpcode( "cmp", 0xC5, 1, AddressingType.ZEROPAGE, 3 );           // CMP $ll
      sys.AddOpcode( "dec", 0xC6, 1, AddressingType.ZEROPAGE, 5 );           // DEC $ll
      sys.AddOpcode( "cmp", 0xc7, 1, AddressingType.ZEROPAGE_INDIRECT_LONG, 6 );      // CMP [$ll]
      sys.AddOpcode( "iny", 0xC8, 0, AddressingType.IMPLICIT, 2 );           // INY
      sys.AddOpcode( "cmp", 0xC9, 1, AddressingType.IMMEDIATE_ACCU, 2 );          // CMP #$nn
      sys.AddOpcode( "dex", 0xCA, 0, AddressingType.IMPLICIT, 2 );           // DEX
      sys.AddOpcode( "wai", 0xcb, 0, AddressingType.IMPLICIT, 3 );                    // WAI
      sys.AddOpcode( "cpy", 0xCC, 2, AddressingType.ABSOLUTE, 4 );           // CPY $hhll
      sys.AddOpcode( "cmp", 0xCD, 2, AddressingType.ABSOLUTE, 4 );           // CMP $hhll
      sys.AddOpcode( "dec", 0xCE, 2, AddressingType.ABSOLUTE, 6 );           // DEC $hhll
      sys.AddOpcode( "cmp", 0xcf, 3, AddressingType.ABSOLUTE_LONG, 5 );               // CMP $hhmmll

      sys.AddOpcode( "bne", 0xD0, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BNE $hhll
      sys.AddOpcode( "cmp", 0xD1, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1 );      // CMP ($ll), Y
      sys.AddOpcode( "cmp", 0xd2, 1, AddressingType.ZEROPAGE_INDIRECT, 5 );      // cmp ($12)
      sys.AddOpcode( "cmp", 0xd3, 1, AddressingType.ZEROPAGE_INDIRECT_SP_Y, 7 );      // CMP ($ll,SP),Y
      sys.AddOpcode( "pei", 0xD4, 1, AddressingType.ZEROPAGE_INDIRECT, 6 );                    // PEI ($ll)
      sys.AddOpcode( "cmp", 0xD5, 1, AddressingType.ZEROPAGE_X, 4 );         // CMP $ll, X
      sys.AddOpcode( "dec", 0xD6, 1, AddressingType.ZEROPAGE_X, 6 );         // DEC $ll, X
      sys.AddOpcode( "cmp", 0xd7, 1, AddressingType.ZEROPAGE_INDIRECT_Y_LONG, 2 );    // CMP [$ll],Y
      sys.AddOpcode( "cld", 0xD8, 0, AddressingType.IMPLICIT, 2 );           // CLD
      sys.AddOpcode( "cmp", 0xD9, 2, AddressingType.ABSOLUTE_Y, 4, 1 );      // CMP $hhll, Y
      sys.AddOpcode( "phx", 0xDA, 0, AddressingType.IMPLICIT, 3 );               // phx
      sys.AddOpcode( "stp", 0xDB, 0, AddressingType.IMPLICIT, 3 );                    // STP
      sys.AddOpcode( "jmp", 0xdc, 2, AddressingType.ABSOLUTE_INDIRECT_LONG, 6 );      // JMP [$hhll]
      sys.AddOpcode( "jml", 0xdc, 2, AddressingType.ABSOLUTE_INDIRECT_LONG, 6 );      // JML [$hhll]
      sys.AddOpcode( "cmp", 0xDD, 2, AddressingType.ABSOLUTE_X, 4, 1 );      // CMP $hhll, X
      sys.AddOpcode( "dec", 0xDE, 2, AddressingType.ABSOLUTE_X, 7 );         // DEC $hhll, X
      sys.AddOpcode( "cmp", 0xdf, 3, AddressingType.ABSOLUTE_LONG_X, 5 );             // CMP $hhmmll,X

      sys.AddOpcode( "cpx", 0xE0, 1, AddressingType.IMMEDIATE_REGISTER, 2 );          // CPX #$nn
      sys.AddOpcode( "sbc", 0xE1, 1, AddressingType.ZEROPAGE_INDIRECT_X, 6, 1 );      // SBC ($ll,X)
      sys.AddOpcode( "sep", 0xE2, 1, AddressingType.IMMEDIATE_8BIT, 3 );              // SEP #$ll
      sys.AddOpcode( "sbc", 0xe3, 1, AddressingType.STACK_RELATIVE, 4 );              // SBC #$ll,SP
      sys.AddOpcode( "cpx", 0xE4, 1, AddressingType.ZEROPAGE, 3 );           // CPX $ll
      sys.AddOpcode( "sbc", 0xE5, 1, AddressingType.ZEROPAGE, 3, 1 );        // SBC $ll
      sys.AddOpcode( "inc", 0xE6, 1, AddressingType.ZEROPAGE, 5 );           // INC $ll
      sys.AddOpcode( "sbc", 0xe7, 1, AddressingType.ZEROPAGE_INDIRECT_LONG, 6 );    // SBC [$ll]
      sys.AddOpcode( "inx", 0xE8, 0, AddressingType.IMPLICIT, 2 );           // INX
      sys.AddOpcode( "sbc", 0xE9, 1, AddressingType.IMMEDIATE_ACCU, 2, 1 );       // SBC #$nn
      sys.AddOpcode( "nop", 0xEA, 0, AddressingType.IMPLICIT, 2 );           // NOP
      sys.AddOpcode( "xba", 0xeb, 0, AddressingType.IMPLICIT, 3 );                    // XBA
      sys.AddOpcode( "cpx", 0xEC, 2, AddressingType.ABSOLUTE, 4 );           // CPX $hhll
      sys.AddOpcode( "sbc", 0xED, 2, AddressingType.ABSOLUTE, 4, 1 );        // SBC $hhll
      sys.AddOpcode( "inc", 0xEE, 2, AddressingType.ABSOLUTE, 6 );           // INC $hhll
      sys.AddOpcode( "sbc", 0xef, 3, AddressingType.ABSOLUTE_LONG, 5 );               // SBC $hhmmll

      sys.AddOpcode( "beq", 0xF0, 1, AddressingType.RELATIVE, 2, 0, 1, 2 );  // BEQ $hhll
      sys.AddOpcode( "sbc", 0xF1, 1, AddressingType.ZEROPAGE_INDIRECT_Y, 5, 1, 1, 0 );// SBC ($ll), Y
      sys.AddOpcode( "sbc", 0xf2, 1, AddressingType.ZEROPAGE_INDIRECT, 5, 1 );   // sbc ($12)
      sys.AddOpcode( "sbc", 0xf3, 1, AddressingType.ZEROPAGE_INDIRECT_SP_Y, 7 );      // SBC ($ll,SP),Y
      sys.AddOpcode( "pea", 0xF4, 2, AddressingType.ABSOLUTE, 5 );                    // PEA $hhll
      sys.AddOpcode( "sbc", 0xF5, 1, AddressingType.ZEROPAGE_X, 4, 1 );      // SBC $ll, X
      sys.AddOpcode( "inc", 0xF6, 1, AddressingType.ZEROPAGE_X, 6 );         // INC $ll, X
      sys.AddOpcode( "sbc", 0xf7, 1, AddressingType.ZEROPAGE_INDIRECT_Y_LONG, 6 );    // SBC [$ll],Y
      sys.AddOpcode( "sed", 0xF8, 0, AddressingType.IMPLICIT, 2 );           // SED
      sys.AddOpcode( "sbc", 0xF9, 2, AddressingType.ABSOLUTE_Y, 4, 1, 1, 0 );// SBC $hhll, Y
      sys.AddOpcode( "plx", 0xfA, 0, AddressingType.IMPLICIT, 4 );               // plx
      sys.AddOpcode( "xce", 0xfb, 0, AddressingType.IMPLICIT, 2 );                    // XCE
      sys.AddOpcode( "jsr", 0xFC, 2, AddressingType.ABSOLUTE_INDIRECT_X, 8 );         // JSR ( $hhll, X )
      sys.AddOpcode( "sbc", 0xFD, 2, AddressingType.ABSOLUTE_X, 4, 1, 1, 0 );// SBC $hhll, X
      sys.AddOpcode( "inc", 0xFE, 2, AddressingType.ABSOLUTE_X, 7 );         // INC $hhll, X
      sys.AddOpcode( "sbc", 0xff, 3, AddressingType.ABSOLUTE_LONG_X, 5 );             // SBC $hhmmll,X

      return sys;
    }



  }
}
