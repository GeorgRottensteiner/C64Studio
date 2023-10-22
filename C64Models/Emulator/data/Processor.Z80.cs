using System;
using System.Collections.Generic;
using System.Text;
using static Tiny64.Opcode;

namespace Tiny64
{
  public partial class Processor
  {
    public static Processor CreateZ80()
    {
      var  sys = new Processor( "Z80" );

      // r Reg. 
      //  000 B
      //  001 C
      //  010 D
      //  011 E
      //  100 H
      //  101 L
      //  111 A

      // dd,ss is any of the register pairs BC, DE, HL, SP
      //   00 BC
      //   01 DE
      //   10 HL
      //   11 SP
      // qq is any of the register pairs AF, BC, DE, HL
      //   00 BC
      //   01 DE
      //   10 HL
      //   11 AF
      // pp is any of the register pairs BC, DE, IX, SP 
      //   00 BC
      //   01 DE
      //   10 IX
      //   11 SP
      // rr is any of the register pairs BC, DE, IY, SP
      //   00 BC
      //   01 DE
      //   10 IY
      //   11 SP

      // b Bit Tested
      //  000 0
      //  001 1
      //  010 2
      //  011 3
      //  100 4
      //  101 5
      //  110 6
      //  111 7

      // cc Condition
      //   000 NZ none zero
      //   001 Z  zero
      //   010 NC non carry
      //   011 C  carry
      //   100 PO parity odd
      //   101 PE parity even
      //   110 P  sign positive
      //   111 M  sign negative

      var r = new List<ValidValue>() { new ValidValue( "b", 0 ),
                                       new ValidValue( "c", 1 ),
                                       new ValidValue( "d", 2 ),
                                       new ValidValue( "e", 3 ),
                                       new ValidValue( "h", 4 ),
                                       new ValidValue( "l", 5 ),
                                       new ValidValue( "a", 7 ) };
      var bits = new List<ValidValue>() { new ValidValue( "0", 0 ),
                                          new ValidValue( "1", 1 ),
                                          new ValidValue( "2", 2 ),
                                          new ValidValue( "3", 3 ),
                                          new ValidValue( "4", 4 ),
                                          new ValidValue( "5", 5 ),
                                          new ValidValue( "6", 6 ),
                                          new ValidValue( "7", 7 ) };
      var rApo = new List<ValidValue>() { new ValidValue( "b'", 0 ),
                                          new ValidValue( "c'", 1 ),
                                          new ValidValue( "d'", 2 ),
                                          new ValidValue( "e'", 3 ),
                                          new ValidValue( "h'", 4 ),
                                          new ValidValue( "l'", 5 ),
                                          new ValidValue( "a'",7 ) };
      var a = new List<ValidValue>() { new ValidValue( "a" ) };
      var i = new List<ValidValue>() { new ValidValue( "i" ) };
      var singleR = new List<ValidValue>() { new ValidValue( "r" ) };
      var c = new List<ValidValue>() { new ValidValue( "c" ) };
      var nc = new List<ValidValue>() { new ValidValue( "nc" ) };
      var z = new List<ValidValue>() { new ValidValue( "z" ) };
      var nz = new List<ValidValue>() { new ValidValue( "nz" ) };
      var ix = new List<ValidValue>() { new ValidValue( "ix" ) };
      var sp = new List<ValidValue>() { new ValidValue( "sp" ) };
      var hl = new List<ValidValue>() { new ValidValue( "hl" ) };
      var de = new List<ValidValue>() { new ValidValue( "de" ) };
      var af = new List<ValidValue>() { new ValidValue( "af" ) };
      var afApo = new List<ValidValue>() { new ValidValue( "af'" ) };
      var im = new List<ValidValue>() { new ValidValue( "0", 0 ),
                                        new ValidValue( "1", 2 ),
                                        new ValidValue( "2", 3 ) };
      var ixInParenthesis = new List<ValidValue>() { new ValidValue( "(" ),
                                                     new ValidValue( "ix" ),
                                                     new ValidValue( "+" ) };
      var iyInParenthesis = new List<ValidValue>() { new ValidValue( "(" ),
                                                     new ValidValue( "iy" ),
                                                     new ValidValue( "+" ) };
      var openingParenthesis = new List<ValidValue>() { new ValidValue( "(" ) };
      var closingParenthesis = new List<ValidValue>() { new ValidValue( ")" ) };
      var iy = new List<ValidValue>() { new ValidValue( "iy" ) };
      var cc = new List<ValidValue>() { new ValidValue( "nz", 0 ),
                                        new ValidValue( "z", 1 ),
                                        new ValidValue( "nc", 2 ),
                                        new ValidValue( "c", 3 ),
                                        new ValidValue( "po", 4 ),
                                        new ValidValue( "pe", 5 ),
                                        new ValidValue( "p", 6 ),
                                        new ValidValue( "m", 7 ) };
      var p = new List<ValidValue>() { new ValidValue( "0", 0 ),
                                       new ValidValue( "8", 1 ),
                                       new ValidValue( "16", 2 ),
                                       new ValidValue( "24", 3 ),
                                       new ValidValue( "32", 4 ),
                                       new ValidValue( "40", 5 ),
                                       new ValidValue( "48", 6 ),
                                       new ValidValue( "56", 7 ) };
      var qq = new List<ValidValue> { new ValidValue( "af", 3 ),
                                      new ValidValue( "bc", 0 ),
                                      new ValidValue( "de", 1 ),
                                      new ValidValue( "hl", 2 ) };
      var dd = new List<ValidValue> { new ValidValue( "bc", 0 ),
                                      new ValidValue( "de", 1 ),
                                      new ValidValue( "hl", 2 ),
                                      new ValidValue( "sp", 3 ) };
      var pp = new List<ValidValue> { new ValidValue( "bc", 0 ),
                                      new ValidValue( "de", 1 ),
                                      new ValidValue( "ix", 2 ),
                                      new ValidValue( "sp", 3 ) };
      var rr = new List<ValidValue> { new ValidValue( "bc", 0 ),
                                      new ValidValue( "de", 1 ),
                                      new ValidValue( "iy", 2 ),
                                      new ValidValue( "sp", 3 ) };

      var hlIndirect = new List<ValidValue>() { new ValidValue( "(" ),
                                        new ValidValue( "HL" ),
                                        new ValidValue( ")" ) };
      var bcIndirect = new List<ValidValue>() { new ValidValue( "(" ),
                                                new ValidValue( "BC" ),
                                                new ValidValue( ")" ) };
      var deIndirect = new List<ValidValue>() { new ValidValue( "(" ),
                                                new ValidValue( "DE" ),
                                                new ValidValue( ")" ) };
      var spIndirect = new List<ValidValue>() { new ValidValue( "(" ),
                                                new ValidValue( "SP" ),
                                                new ValidValue( ")" ) };
      var ixIndirect = new List<ValidValue>() { new ValidValue( "(" ),
                                                new ValidValue( "IX" ),
                                                new ValidValue( ")" ) };
      var iyIndirect = new List<ValidValue>() { new ValidValue( "(" ),
                                                new ValidValue( "IY" ),
                                                new ValidValue( ")" ) };
      var cIndirect = new List<ValidValue>() { new ValidValue( "(" ),
                                               new ValidValue( "C" ),
                                               new ValidValue( ")" ) };

      // 8bit group
      sys.AddOpcode( "ld", 0x40, 0, AddressingType.IMPLICIT, 4 )          // LD r,r'
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, r, 3 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, rApo, 0 ) } );

      sys.AddOpcode( "ld", 0x46, 0, AddressingType.IMPLICIT, 7 )         // LD r,(HL)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, r, 3 ),
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, hlIndirect ) } );

      sys.AddOpcode( "ld", 0xdd4600, 1, AddressingType.IX_D_INDIRECT_TO_REGISTER, 19 )    // LD r,(IX+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, r, 8 + 3 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, ixInParenthesis, closingParenthesis ) } );

      sys.AddOpcode( "ld", 0xfd4600, 1, AddressingType.IY_D_INDIRECT_TO_REGISTER, 19 )      // LD r,(IY+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, r, 8 + 3 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, iyInParenthesis, closingParenthesis ) } );

      sys.AddOpcode( "ld", 0x70, 0, AddressingType.REGISTER_TO_HL_INDIRECT, 7 )         // LD (HL),r
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, hlIndirect ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, r, 0 ) } );

      sys.AddOpcode( "ld", 0xdd7000, 1, AddressingType.REGISTER_TO_IX_D_INDIRECT, 19 )      // LD (IX+d),r
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, ixInParenthesis, closingParenthesis ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, r, 8 + 0 ) } );

      sys.AddOpcode( "ld", 0xfd7000, 1, AddressingType.REGISTER_TO_IY_D_INDIRECT, 19 )      // LD (IY+d),r
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, iyInParenthesis, closingParenthesis ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, r, 8 + 0 ) } );

      sys.AddOpcode( "ld", 0x3600, 1, AddressingType.IMMEDIATE_8BIT, 10 )       // LD (HL),n
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, hlIndirect, 8 + 0 ),
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT ) } );

      sys.AddOpcode( "ld", 0x0a, 0, AddressingType.IMPLICIT, 7 )                // LD A,(BC)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, a ),
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, bcIndirect ) } );

      sys.AddOpcode( "ld", 0x1a, 0, AddressingType.IMPLICIT, 7 )                // LD A,(DE)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, a ),
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, deIndirect ) } );

      sys.AddOpcode( "ld", 0x02, 0, AddressingType.IMPLICIT, 7 )                // LD (BC),A
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, bcIndirect ),
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, a ) } );

      sys.AddOpcode( "ld", 0x12, 0, AddressingType.IMPLICIT, 7 )                // LD (DE),A
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, deIndirect ),
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, a ) } );

      sys.AddOpcode( "ld", 0xdd360000, 1, AddressingType.IMMEDIATE_TO_IX_D_INDIRECT, 19 )   // LD (IX+d),n
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, ixInParenthesis, closingParenthesis, 8 ),
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT ) } );

      sys.AddOpcode( "ld", 0xfd360000, 1, AddressingType.IMMEDIATE_TO_IY_D_INDIRECT, 19 )   // LD (IY+d),n
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, iyInParenthesis, closingParenthesis, 8 ),
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT ) } );

      sys.AddOpcode( "ld", 0x3a0000, 2, AddressingType.IMMEDIATE_INDIRECT_TO_A, 13 )        // LD A,(nn)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, a ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ) } );

      sys.AddOpcode( "ld", 0x320000, 2, AddressingType.A_TO_IMMEDIATE_INDIRECT, 13 )        // LD (nn),A
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, a ) } );

      sys.AddOpcode( "ld", 0xed57, 0, AddressingType.IMPLICIT, 9 )                        // LD A,I
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, a ),
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, i ) } );

      sys.AddOpcode( "ld", 0xed5f, 0, AddressingType.IMPLICIT, 9 )                        // LD A,R
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, a ),
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, singleR ) } );

      sys.AddOpcode( "ld", 0xed47, 0, AddressingType.IMPLICIT, 9 )                        // LD I,A
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, i ),
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, a ) } );

      sys.AddOpcode( "ld", 0xed4f, 0, AddressingType.IMPLICIT, 9 )                        // LD R,A
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, singleR ),
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, a ) } );

      // opcodes with expressions need to be placed last
      sys.AddOpcode( "ld", 0x0600, 1, AddressingType.IMMEDIATE_8BIT, 7 )                  // LD r,n
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, r, 8 + 3 ),
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT ) } );

      sys.AddOpcode( "ld", 0xf9, 0, AddressingType.IMPLICIT, 6 )                        // LD SP,HL
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, sp ),
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, hl ) } );

      sys.AddOpcode( "ld", 0xddf9, 0, AddressingType.IMPLICIT, 10 )                     // LD SP,IX
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, sp ),
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, ix ) } );

      sys.AddOpcode( "ld", 0xfdf9, 0, AddressingType.IMPLICIT, 10 )                     // LD SP,IY
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, sp ),
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, iy ) } );

      // 16bit group
      sys.AddOpcode( "ld", 0x2a0000, 2, AddressingType.IMMEDIATE_INDIRECT_TO_HL, 16 )       // LD HL,(nn)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, hl ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ) } );

      sys.AddOpcode( "ld", 0xed4b0000, 2, AddressingType.IMMEDIATE_INDIRECT_TO_REGISTER_DD, 20 )  // LD dd,(nn)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dd, 16 + 4 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ) } );

      sys.AddOpcode( "ld", 0x010000, 2, AddressingType.IMMEDIATE_TO_REGISTER_DD, 10 )       // LD dd,nn
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dd, 16 + 4 ),
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_16BIT ) } );

      sys.AddOpcode( "ld", 0xdd2a0000, 2, AddressingType.IMMEDIATE_INDIRECT_TO_IX, 20 )     // LD IX,(nn)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, ix ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ) } );

      sys.AddOpcode( "ld", 0xdd210000, 2, AddressingType.IMMEDIATE_TO_IX, 14 )              // LD IX,nn
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, ix ),
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_16BIT ) } );

      sys.AddOpcode( "ld", 0xfd2a0000, 2, AddressingType.IMMEDIATE_INDIRECT_TO_IY, 20 )     // LD IY,(nn)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, iy ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ) } );

      sys.AddOpcode( "ld", 0xfd210000, 2, AddressingType.IMMEDIATE_TO_IY, 14 )              // LD IY,nn
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, iy ),
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_16BIT ) } );

      
      sys.AddOpcode( "ld", 0x220000, 2, AddressingType.HL_TO_IMMEDIATE_INDIRECT, 16 )       // LD (nn),HL
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, hl ) } );

      sys.AddOpcode( "ld", 0xed430000, 2, AddressingType.REGISTER_DD_TO_IMMEDIATE_INDIRECT, 20 )  // LD (nn),dd
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dd, 16 + 4 ) } );

      sys.AddOpcode( "ld", 0xdd220000, 2, AddressingType.IX_TO_IMMEDIATE_INDIRECT, 20 )     // LD (nn),IX
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, ix ) } );

      sys.AddOpcode( "ld", 0xfd220000, 2, AddressingType.IY_TO_IMMEDIATE_INDIRECT, 20 )     // LD (nn),IY
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, iy ) } );

      sys.AddOpcode( "push", 0xc5, 0, AddressingType.REGISTER_QQ, 11 )                  // PUSH qq
        .ParserExpressions.Add( new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, qq, 4 ) );
      sys.AddOpcode( "push", 0xdde5, 0, AddressingType.REGISTER_IX, 15 )                // PUSH IX
        .ParserExpressions.Add( new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, ix ) );
      sys.AddOpcode( "push", 0xfde5, 0, AddressingType.REGISTER_IY, 15 )                // PUSH IY
        .ParserExpressions.Add( new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, iy ) );
      sys.AddOpcode( "pop", 0xc1, 0, AddressingType.REGISTER_QQ, 10 )                   // POP qq
        .ParserExpressions.Add( new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, qq, 4 ) );
      sys.AddOpcode( "pop", 0xdde1, 0, AddressingType.REGISTER_IX, 14 )                 // POP IX
        .ParserExpressions.Add( new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, ix ) );
      sys.AddOpcode( "pop", 0xfde1, 0, AddressingType.REGISTER_IY, 14 )                 // POP IY
        .ParserExpressions.Add( new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, iy ) );

      // Exchange, block transfer, search
      sys.AddOpcode( "ex", 0xeb, 0, AddressingType.IMPLICIT, 4 )               // EX DE,HL
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, de ),
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, hl ) } );
      // rotated ex version, seems common
      sys.AddOpcode( "ex", 0xeb, 0, AddressingType.IMPLICIT, 4 )               // EX HL,DE
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, hl ),
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, de ) } );

      sys.AddOpcode( "ex", 0x08, 0, AddressingType.IMPLICIT, 4 )               // EX AF,AF'
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, af ),
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, afApo ) } );

      sys.AddOpcode( "exx", 0xd9, 0, AddressingType.IMPLICIT, 4 );                      // EXX

      sys.AddOpcode( "ex", 0xe3, 0, AddressingType.IMPLICIT, 19 )     // EX (SP),HL
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, spIndirect ),
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, hl ) } );

      sys.AddOpcode( "ex", 0xdde3, 0, AddressingType.IMPLICIT, 23 )   // EX (SP),IX
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, spIndirect ),
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, ix ) } );

      sys.AddOpcode( "ex", 0xfde3, 0, AddressingType.IMPLICIT, 23 )   // EX (SP),IY
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, spIndirect ),
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, iy ) } );

      sys.AddOpcode( "ldi", 0xeda0, 0, AddressingType.IMPLICIT, 16 );                   // LDI
      sys.AddOpcode( "ldir", 0xedb0, 0, AddressingType.IMPLICIT, 16, 5 );               // LDIR
      sys.AddOpcode( "ldd", 0xeda8, 0, AddressingType.IMPLICIT, 16 );                   // LDD
      sys.AddOpcode( "lddr", 0xedb8, 0, AddressingType.IMPLICIT, 16, 5 );               // LDDR
      sys.AddOpcode( "cpi", 0xeda1, 0, AddressingType.IMPLICIT, 16 );                   // CPI
      sys.AddOpcode( "cpir", 0xedb1, 0, AddressingType.IMPLICIT, 16, 5 );               // CPIR
      sys.AddOpcode( "cpd", 0xeda9, 0, AddressingType.IMPLICIT, 16 );                   // CPD
      sys.AddOpcode( "cpdr", 0xedb9, 0, AddressingType.IMPLICIT, 16, 5 );               // CPDR

      // 8bit arithmetic and logical
      sys.AddOpcode( "add", 0x80, 0, AddressingType.REGISTER_TO_A, 4 )                  // ADD A,r
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, a ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, r, 0 ) } );

      sys.AddOpcode( "add", 0x86, 0, AddressingType.HL_INDIRECT_TO_A, 7 )               // ADD A,(HL)
        .ParserExpressions.AddRange( new List<OpcodeExpression>(){
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, a ),
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, hlIndirect ) } );

      sys.AddOpcode( "add", 0xdd8600, 1, AddressingType.IX_D_INDIRECT_TO_A, 19 )          // ADD A,(IX+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, a ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, ixInParenthesis, closingParenthesis ) } );

      sys.AddOpcode( "add", 0xfd8600, 1, AddressingType.IY_D_INDIRECT_TO_A, 19 )          // ADD A,(IY+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, a ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, iyInParenthesis, closingParenthesis ) } );

      sys.AddOpcode( "add", 0xc600, 1, AddressingType.IMMEDIATE_8BIT, 7 )                 // ADD A,n
        .ParserExpressions.AddRange( new List<OpcodeExpression>(){ 
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, a ), 
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT ) } );

      sys.AddOpcode( "adc", 0x88, 0, AddressingType.REGISTER_TO_A, 4 )                  // ADC A,r
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, a ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, r, 0 ) } );

      sys.AddOpcode( "adc", 0x8e, 0, AddressingType.HL_INDIRECT_TO_A, 7 )               // ADC A,(HL)
        .ParserExpressions.AddRange( new List<OpcodeExpression>(){
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, a ),
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, hlIndirect ) } );

      sys.AddOpcode( "adc", 0xdd8e00, 1, AddressingType.IX_D_INDIRECT_TO_A, 19 )          // ADC A,(IX+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, a ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, ixInParenthesis, closingParenthesis ) } );

      sys.AddOpcode( "adc", 0xfd8e00, 1, AddressingType.IY_D_INDIRECT_TO_A, 19 )          // ADC A,(IY+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, a ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, iyInParenthesis, closingParenthesis ) } );

      sys.AddOpcode( "adc", 0xce00, 1, AddressingType.IMMEDIATE_8BIT, 7 )                 // ADC A,n
        .ParserExpressions.AddRange( new List<OpcodeExpression>(){
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, a ),
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT ) } );


      sys.AddOpcode( "sub", 0x90, 0, AddressingType.REGISTER_TO_A, 4 )                  // SUB r
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, r, 0 ) } );

      sys.AddOpcode( "sub", 0x96, 0, AddressingType.HL_INDIRECT_TO_A, 7 )               // SUB (HL)
        .ParserExpressions.AddRange( new List<OpcodeExpression>(){
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, hlIndirect ) } );

      sys.AddOpcode( "sub", 0xdd9600, 1, AddressingType.IX_D_INDIRECT_TO_A, 19 )          // SUB (IX+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, ixInParenthesis, closingParenthesis ) } );

      sys.AddOpcode( "sub", 0xfd9600, 1, AddressingType.IY_D_INDIRECT_TO_A, 19 )          // SUB (IY+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, iyInParenthesis, closingParenthesis ) } );

      sys.AddOpcode( "sub", 0xd600, 1, AddressingType.IMMEDIATE_TO_A, 7 )                 // SUB n
        .ParserExpressions.AddRange( new List<OpcodeExpression>(){
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT ) } );


      sys.AddOpcode( "sbc", 0x98, 0, AddressingType.REGISTER_TO_A, 4 )                    // SBC A,r
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, a ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, r, 0 ) } );

      sys.AddOpcode( "sbc", 0x9e, 0, AddressingType.HL_INDIRECT_TO_A, 7 )                 // SBC A,(HL)
        .ParserExpressions.AddRange( new List<OpcodeExpression>(){
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, a ),
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, hlIndirect ) } );

      sys.AddOpcode( "sbc", 0xdd9e00, 1, AddressingType.IX_D_INDIRECT_TO_A, 19 )          // SBC A,(IX+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, a ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, ixInParenthesis, closingParenthesis ) } );

      sys.AddOpcode( "sbc", 0xfd9e00, 1, AddressingType.IY_D_INDIRECT_TO_A, 19 )          // SBC A,(IY+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, a ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, iyInParenthesis, closingParenthesis ) } );

      sys.AddOpcode( "sbc", 0xde00, 1, AddressingType.IMMEDIATE_8BIT, 7 )                 // SBC A,n
        .ParserExpressions.AddRange( new List<OpcodeExpression>(){
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, a ),
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT ) } );


      sys.AddOpcode( "and", 0xa0, 0, AddressingType.IMPLICIT, 4 )                  // AND r
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, r, 0 ) } );

      sys.AddOpcode( "and", 0xa6, 0, AddressingType.IMPLICIT, 7 )               // AND (HL)
        .ParserExpressions.AddRange( new List<OpcodeExpression>(){
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, hlIndirect ) } );

      sys.AddOpcode( "and", 0xdda600, 1, AddressingType.IX_D_INDIRECT_TO_A, 19 )          // AND (IX+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, ixInParenthesis, closingParenthesis ) } );

      sys.AddOpcode( "and", 0xfda600, 1, AddressingType.IY_D_INDIRECT_TO_A, 19 )          // AND (IY+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, iyInParenthesis, closingParenthesis ) } );

      sys.AddOpcode( "and", 0xe600, 1, AddressingType.IMMEDIATE_8BIT, 7 )                 // AND n
        .ParserExpressions.AddRange( new List<OpcodeExpression>(){
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT ) } );



      sys.AddOpcode( "or", 0xb0, 0, AddressingType.REGISTER_TO_A, 4 )                   // OR r
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, r, 0 ) } );

      sys.AddOpcode( "or", 0xb6, 0, AddressingType.HL_INDIRECT_TO_A, 7 )                // OR (HL)
        .ParserExpressions.AddRange( new List<OpcodeExpression>(){
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, hlIndirect ) } );

      sys.AddOpcode( "or", 0xddb600, 1, AddressingType.IX_D_INDIRECT_TO_A, 19 )           // OR (IX+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, ixInParenthesis, closingParenthesis ) } );

      sys.AddOpcode( "or", 0xfdb600, 1, AddressingType.IY_D_INDIRECT_TO_A, 19 )           // OR (IY+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, iyInParenthesis, closingParenthesis ) } );

      sys.AddOpcode( "or", 0xf600, 1, AddressingType.IMMEDIATE_TO_A, 7 )                  // OR n
        .ParserExpressions.AddRange( new List<OpcodeExpression>(){
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT ) } );



      sys.AddOpcode( "xor", 0xa8, 0, AddressingType.IMPLICIT, 4 )                  // XOR r
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, r, 0 ) } );

      sys.AddOpcode( "xor", 0xae, 0, AddressingType.IMPLICIT, 7 )               // XOR (HL)
        .ParserExpressions.AddRange( new List<OpcodeExpression>(){
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, hlIndirect ) } );

      sys.AddOpcode( "xor", 0xddae00, 1, AddressingType.IX_D_INDIRECT_TO_A, 19 )          // XOR (IX+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, ixInParenthesis, closingParenthesis ) } );

      sys.AddOpcode( "xor", 0xfdae00, 1, AddressingType.IY_D_INDIRECT_TO_A, 19 )          // XOR (IY+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, iyInParenthesis, closingParenthesis ) } );

      sys.AddOpcode( "xor", 0xee00, 1, AddressingType.IMPLICIT, 7 )                 // XOR n
        .ParserExpressions.AddRange( new List<OpcodeExpression>(){
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT ) } );



      sys.AddOpcode( "cp", 0xb8, 0, AddressingType.REGISTER_TO_A, 4 )                   // CP r
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, r, 0 ) } );

      sys.AddOpcode( "cp", 0xbe, 0, AddressingType.HL_INDIRECT_TO_A, 7 )                // CP (HL)
        .ParserExpressions.AddRange( new List<OpcodeExpression>(){
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, hlIndirect ) } );

      sys.AddOpcode( "cp", 0xddbe00, 1, AddressingType.IX_D_INDIRECT_TO_A, 19 )           // CP (IX+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, ixInParenthesis, closingParenthesis ) } );

      sys.AddOpcode( "cp", 0xfdbe00, 1, AddressingType.IY_D_INDIRECT_TO_A, 19 )           // CP (IY+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, iyInParenthesis, closingParenthesis ) } );

      sys.AddOpcode( "cp", 0xfe00, 1, AddressingType.IMMEDIATE_TO_A, 7 )                  // CP n
        .ParserExpressions.AddRange( new List<OpcodeExpression>(){
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT ) } );


      sys.AddOpcode( "inc", 0x04, 0, AddressingType.IMPLICIT, 4 )                       // INC r
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, r, 3 ) } );

      sys.AddOpcode( "inc", 0x34, 0, AddressingType.IMPLICIT, 11 )          // INC (HL)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, hlIndirect ) } );

      sys.AddOpcode( "inc", 0xdd3400, 1, AddressingType.REGISTER_IX_D_INDIRECT, 23 )      // INC (IX+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, ixInParenthesis, closingParenthesis ) } );

      sys.AddOpcode( "inc", 0xfd3400, 1, AddressingType.REGISTER_IY_D_INDIRECT, 23 )      // INC (IY+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, iyInParenthesis, closingParenthesis ) } );


      sys.AddOpcode( "dec", 0x05, 0, AddressingType.REGISTER, 4 )                       // DEC r
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, r, 3 ) } );

      sys.AddOpcode( "dec", 0x35, 0, AddressingType.REGISTER_HL_INDIRECT, 11 )          // DEC (HL)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, hlIndirect ) } );

      sys.AddOpcode( "dec", 0xdd3500, 1, AddressingType.REGISTER_IX_D_INDIRECT, 23 )      // DEC (IX+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, ixInParenthesis, closingParenthesis ) } );

      sys.AddOpcode( "dec", 0xfd3500, 1, AddressingType.REGISTER_IY_D_INDIRECT, 23 )      // DEC (IY+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, iyInParenthesis, closingParenthesis ) } );


      // general purpose arithmetic and cpu control
      sys.AddOpcode( "daa", 0x27, 0, AddressingType.IMPLICIT, 4 );                      // DAA
      sys.AddOpcode( "cpl", 0x2f, 0, AddressingType.IMPLICIT, 4 );                      // CPL
      sys.AddOpcode( "neg", 0xed44, 0, AddressingType.IMPLICIT, 8 );                    // NEG
      sys.AddOpcode( "ccf", 0x3f, 0, AddressingType.IMPLICIT, 4 );                      // CCF
      sys.AddOpcode( "scf", 0x37, 0, AddressingType.IMPLICIT, 4 );                      // SCF
      sys.AddOpcode( "nop", 0x00, 0, AddressingType.IMPLICIT, 4 );                      // NOP
      sys.AddOpcode( "halt", 0x76, 0, AddressingType.IMPLICIT, 4 );                     // HALT
      sys.AddOpcode( "di", 0xf3, 0, AddressingType.IMPLICIT, 4 );                       // DI
      sys.AddOpcode( "ei", 0xfb, 0, AddressingType.IMPLICIT, 4 );                       // EI
      sys.AddOpcode( "im", 0xed46, 0, AddressingType.IMMEDIATE_0, 8 )                   // IM 0,1,2
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, im, 3 ) } );

      // 16bit arithmetic
      sys.AddOpcode( "add", 0x09, 0, AddressingType.IMPLICIT, 11 )             // ADD HL,dd
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, hl ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dd, 4 ) } );

      sys.AddOpcode( "adc", 0xed4a, 0, AddressingType.IMPLICIT, 15 )           // ADC HL,dd
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, hl ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dd, 4 ) } );

      sys.AddOpcode( "sbc", 0xed42, 0, AddressingType.IMPLICIT, 15 )           // SBC HL,dd
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, hl ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dd, 4 ) } );

      sys.AddOpcode( "add", 0xdd09, 0, AddressingType.REGISTER_PP_TO_IX, 15 )           // ADD IX,pp
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, ix ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, pp, 4 ) } );

      sys.AddOpcode( "add", 0xfd09, 0, AddressingType.REGISTER_PP_TO_IY, 15 )           // ADD IY,rr
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, iy ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, rr, 4 ) } );

      sys.AddOpcode( "inc", 0x03, 0, AddressingType.REGISTER_DD, 6 )                    // INC dd
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dd, 4 ) } );

      sys.AddOpcode( "inc", 0xdd23, 0, AddressingType.REGISTER_IX, 10 )                 // INC IX
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, ix, 4 ) } );

      sys.AddOpcode( "inc", 0xfd23, 0, AddressingType.REGISTER_IY, 10 )                 // INC IY
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, iy, 4 ) } );

      sys.AddOpcode( "dec", 0x0b, 0, AddressingType.REGISTER_DD, 6 )                    // DEC dd
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dd, 4 ) } );

      sys.AddOpcode( "dec", 0xdd2b, 0, AddressingType.REGISTER_IX, 10 )                 // DEC IX
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, ix, 4 ) } );
      sys.AddOpcode( "dec", 0xfd2b, 0, AddressingType.REGISTER_IY, 10 )                 // DEC IY
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, iy, 4 ) } );

      // rotate and shift group
      sys.AddOpcode( "rlca", 0x07, 0, AddressingType.IMPLICIT, 4 );                     // RLCA
      sys.AddOpcode( "rla", 0x17, 0, AddressingType.IMPLICIT, 4 );                      // RLA
      sys.AddOpcode( "rrca", 0x0f, 0, AddressingType.IMPLICIT, 4 );                     // RRCA
      sys.AddOpcode( "rra", 0x1f, 0, AddressingType.IMPLICIT, 4 );                      // RRA


      sys.AddOpcode( "rlc", 0xcb00, 0, AddressingType.REGISTER, 8 )                     // RLC r
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, r ) } );

      sys.AddOpcode( "rlc", 0xcb06, 0, AddressingType.HL_INDIRECT, 15 )                 // RLC (HL)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, hlIndirect ) } );

      sys.AddOpcode( "rlc", 0xddcb0006, 1, AddressingType.IX_D_INDIRECT, 23 )           // RLC (IX+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, ixInParenthesis, closingParenthesis, 8 ) } );

      sys.AddOpcode( "rlc", 0xfdcb0006, 1, AddressingType.IY_D_INDIRECT, 23 )           // RLC (IY+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, iyInParenthesis, closingParenthesis, 8 ) } );

      sys.AddOpcode( "rl", 0xcb10, 0, AddressingType.REGISTER, 8 )                      // RL r
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, r ) } );
      sys.AddOpcode( "rl", 0xcb16, 0, AddressingType.HL_INDIRECT, 15 )                  // RL (HL)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, hlIndirect ) } );
      sys.AddOpcode( "rl", 0xddcb0016, 1, AddressingType.IX_D_INDIRECT, 23 )            // RL (IX+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, ixInParenthesis, closingParenthesis, 8 ) } );
      sys.AddOpcode( "rl", 0xfdcb0016, 1, AddressingType.IY_D_INDIRECT, 23 )            // RL (IY+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, iyInParenthesis, closingParenthesis, 8 ) } );


      sys.AddOpcode( "rrc", 0xcb08, 0, AddressingType.REGISTER, 8 )                     // RRC r
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, r ) } );
      sys.AddOpcode( "rrc", 0xcb0e, 0, AddressingType.HL_INDIRECT, 15 )                 // RRC (HL)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, hlIndirect ) } );
      sys.AddOpcode( "rrc", 0xddcb000e, 1, AddressingType.IX_D_INDIRECT, 23 )           // RRC (IX+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, ixInParenthesis, closingParenthesis, 8 ) } );

      sys.AddOpcode( "rrc", 0xfdcb000e, 1, AddressingType.IY_D_INDIRECT, 23 )           // RRC (IY+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, iyInParenthesis, closingParenthesis, 8 ) } );

      sys.AddOpcode( "rr", 0xcb18, 0, AddressingType.REGISTER, 8 )                      // RR r
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, r ) } );
      sys.AddOpcode( "rr", 0xcb1e, 0, AddressingType.HL_INDIRECT, 15 )                  // RR (HL)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, hlIndirect ) } );
      sys.AddOpcode( "rr", 0xddcb001e, 1, AddressingType.IX_D_INDIRECT, 23 )            // RR (IX+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, ixInParenthesis, closingParenthesis, 8 ) } );

      sys.AddOpcode( "rr", 0xfdcb001e, 1, AddressingType.IY_D_INDIRECT, 23 )            // RR (IY+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, iyInParenthesis, closingParenthesis, 8 ) } );

      sys.AddOpcode( "sla", 0xcb20, 0, AddressingType.REGISTER, 8 )                     // SLA r
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, r ) } );
      sys.AddOpcode( "sla", 0xcb26, 0, AddressingType.HL_INDIRECT, 15 )                 // SLA (HL)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, hlIndirect ) } );
      sys.AddOpcode( "sla", 0xddcb0026, 1, AddressingType.IX_D_INDIRECT, 23 )           // SLA (IX+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, ixInParenthesis, closingParenthesis, 8 ) } );
      sys.AddOpcode( "sla", 0xfdcb0026, 1, AddressingType.IY_D_INDIRECT, 23 )           // SLA (IY+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, iyInParenthesis, closingParenthesis, 8 ) } );

      sys.AddOpcode( "sra", 0xcb28, 0, AddressingType.REGISTER, 8 )                     // SRA r
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, r ) } );
      sys.AddOpcode( "sra", 0xcb2e, 0, AddressingType.HL_INDIRECT, 15 )                 // SRA (HL)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, hlIndirect ) } );
      sys.AddOpcode( "sra", 0xddcb002e, 1, AddressingType.IX_D_INDIRECT, 23 )           // SRA (IX+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, ixInParenthesis, closingParenthesis, 8 ) } );
      sys.AddOpcode( "sra", 0xfdcb002e, 1, AddressingType.IY_D_INDIRECT, 23 )           // SRA (IY+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, iyInParenthesis, closingParenthesis, 8 ) } );

      sys.AddOpcode( "srl", 0xcb38, 0, AddressingType.REGISTER, 8 )                     // SRL r
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, r ) } );
      sys.AddOpcode( "srl", 0xcb3e, 0, AddressingType.HL_INDIRECT, 15 )                 // SRL (HL)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, hlIndirect ) } );
      sys.AddOpcode( "srl", 0xddcb003e, 1, AddressingType.IX_D_INDIRECT, 23 )           // SRL (IX+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, ixInParenthesis, closingParenthesis, 8 ) } );
      sys.AddOpcode( "srl", 0xfdcb003e, 1, AddressingType.IY_D_INDIRECT, 23 )           // SRL (IY+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, iyInParenthesis, closingParenthesis, 8 ) } );

      sys.AddOpcode( "rld", 0xed6f, 0, AddressingType.IMPLICIT, 18 );                   // RLD
      sys.AddOpcode( "rrd", 0xed67, 0, AddressingType.IMPLICIT, 18 );                   // RRD

      // bit set, reset, test group
      sys.AddOpcode( "bit", 0xcb40, 1, AddressingType.IMPLICIT, 8 )              // BIT b,r
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT, bits, 3 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, r ) } );

      sys.AddOpcode( "bit", 0xcb46, 1, AddressingType.HL_INDIRECT_TO_BIT, 12 )          // BIT b,(HL)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT, bits, 3 ),
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, hlIndirect ) } );

      sys.AddOpcode( "bit", 0xddcb0046, 1, AddressingType.IX_D_INDIRECT_TO_BIT, 20 )    // BIT b,(IX+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT, bits, 3 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, ixInParenthesis, closingParenthesis, 8 ) } );

      sys.AddOpcode( "bit", 0xfdcb0046, 1, AddressingType.IY_D_INDIRECT_TO_BIT, 20 )    // BIT b,(IY+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT, bits, 3 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, iyInParenthesis, closingParenthesis, 8 ) } );


      sys.AddOpcode( "set", 0xcbc0, 0, AddressingType.REGISTER_TO_BIT, 8 )              // SET b,r
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT, bits, 3 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, r ) } );
      sys.AddOpcode( "set", 0xcbc6, 1, AddressingType.HL_INDIRECT_TO_BIT, 15 )          // SET b,(HL)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT, bits, 3 ),
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, hlIndirect ) } );
      sys.AddOpcode( "set", 0xddcb00c6, 1, AddressingType.IX_D_INDIRECT_TO_BIT, 23 )    // SET b,(IX+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT, bits, 3 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, ixInParenthesis, closingParenthesis, 8 ) } );
      sys.AddOpcode( "set", 0xfdcb00c6, 1, AddressingType.IY_D_INDIRECT_TO_BIT, 23 )    // SET b,(IY+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT, bits, 3 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, iyInParenthesis, closingParenthesis, 8 ) } );


      sys.AddOpcode( "res", 0xcb80, 1, AddressingType.REGISTER_TO_BIT, 8 )              // RES b,r
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT, bits, 3 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, r ) } );
      sys.AddOpcode( "res", 0xcb86, 1, AddressingType.HL_INDIRECT_TO_BIT, 15 )          // RES b,(HL)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT, bits, 3 ),
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, hlIndirect ) } );
      sys.AddOpcode( "res", 0xddcb0086, 1, AddressingType.IX_D_INDIRECT_TO_BIT, 23 )    // RES b,(IX+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT, bits, 3 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, ixInParenthesis, closingParenthesis, 8 ) } );
      sys.AddOpcode( "res", 0xfdcb0086, 1, AddressingType.IY_D_INDIRECT_TO_BIT, 23 )    // RES b,(IY+d)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT, bits, 3 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, iyInParenthesis, closingParenthesis, 8 ) } );

      // jump group
      sys.AddOpcode( "jp", 0xc20000, 2, AddressingType.ABSOLUTE_CONDITION, 10 )            // JP cc,nn
        .ParserExpressions.AddRange( new List<OpcodeExpression>() { 
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, cc, 16 + 3 ), 
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_16BIT ) }  );

      sys.AddOpcode( "jr", 0x3800, 1, AddressingType.RELATIVE, 7, 5 )                     // JR C,e
        .ParserExpressions.AddRange( new List<OpcodeExpression>() { 
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, c ),
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT ) } );
      sys.AddOpcode( "jr", 0x3000, 1, AddressingType.RELATIVE, 7, 5 )                     // JR NC,e
        .ParserExpressions.AddRange( new List<OpcodeExpression>() { 
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, nc ),
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT ) } );
      sys.AddOpcode( "jr", 0x2800, 1, AddressingType.RELATIVE, 7, 5 )                     // JR Z,e
        .ParserExpressions.AddRange( new List<OpcodeExpression>() { 
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, z ),
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT ) } );
      sys.AddOpcode( "jr", 0x2000, 1, AddressingType.RELATIVE, 7, 5 )                     // JR NZ,e
        .ParserExpressions.AddRange( new List<OpcodeExpression>() { 
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, nz ),
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT ) } );

      sys.AddOpcode( "jp", 0xe9, 0, AddressingType.IMPLICIT, 4 )                          // JP (HL)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
        new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, hlIndirect ) } );
      sys.AddOpcode( "jp", 0xdde9, 0, AddressingType.IMPLICIT, 8 )                        // JP (IX)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
        new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, ixIndirect ) } );
      sys.AddOpcode( "jp", 0xfde9, 0, AddressingType.IMPLICIT, 8 )                        // JP (IY)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
        new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, iyIndirect ) } );

      sys.AddOpcode( "djnz", 0x1000, 1, AddressingType.RELATIVE, 8, 5 )                   // DJNZ e
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT ) } );

      sys.AddOpcode( "jr", 0x1800, 1, AddressingType.RELATIVE, 12 )                       // JR e
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT ) } );

      sys.AddOpcode( "jp", 0xc30000, 2, AddressingType.ABSOLUTE, 10 )                     // JP nn
        .ParserExpressions.Add( new OpcodeExpression( OpcodePartialExpression.EXPRESSION_16BIT ) );


      // call and return group
      sys.AddOpcode( "call", 0xc40000, 2, AddressingType.ABSOLUTE_CONDITION, 10, 7 )        // CALL cc,nn
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, cc, 16 + 3 ),
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_16BIT ) } );

      sys.AddOpcode( "call", 0xcd0000, 2, AddressingType.ABSOLUTE, 17 )                     // CALL nn
        .ParserExpressions.Add( new OpcodeExpression( OpcodePartialExpression.EXPRESSION_16BIT ) );

      sys.AddOpcode( "ret", 0xc0, 0, AddressingType.IMPLICIT, 5, 6 )               // RET cc
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, cc, 3 ) } );

      sys.AddOpcode( "ret", 0xc9, 0, AddressingType.IMPLICIT, 10 )                      // RET
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EMPTY ) } );

      sys.AddOpcode( "reti", 0xed4d, 0, AddressingType.IMPLICIT, 14 );                  // RETI
      sys.AddOpcode( "retn", 0xed45, 0, AddressingType.IMPLICIT, 14 );                  // RETN

      sys.AddOpcode( "rst", 0xc7, 1, AddressingType.IMPLICIT, 11 )                    // RST p
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT, p, 3 ) } );

      // input and output group
      sys.AddOpcode( "in", 0xed40, 0, AddressingType.IMPLICIT, 12 )       // IN r,(C)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, r, 3 ),
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, cIndirect ) } );

      sys.AddOpcode( "in", 0xdb00, 1, AddressingType.INDIRECT, 11 )   // IN A,(n)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, a ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, openingParenthesis, closingParenthesis ) } );

      sys.AddOpcode( "ini", 0xeda2, 0, AddressingType.IMPLICIT, 16 );                   // INI
      sys.AddOpcode( "inir", 0xedb2, 0, AddressingType.IMPLICIT, 16, 5 );               // INIR
      sys.AddOpcode( "ind", 0xedaa, 0, AddressingType.IMPLICIT, 16 );                   // IND
      sys.AddOpcode( "indr", 0xedba, 0, AddressingType.IMPLICIT, 16, 5 );               // INDR

      sys.AddOpcode( "out", 0xed41, 0, AddressingType.REGISTER_TO_C_INDIRECT, 12 )      // OUT (C),r
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, cIndirect ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, r, 3 ) } );

      sys.AddOpcode( "out", 0xd300, 1, AddressingType.A_TO_IMMEDIATE_INDIRECT_8BIT, 11 )  // OUT (n),A
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, openingParenthesis, closingParenthesis ),
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, a ) } );

      sys.AddOpcode( "outi", 0xeda3, 0, AddressingType.IMPLICIT, 16 );                  // OUTI
      sys.AddOpcode( "otir", 0xedb3, 0, AddressingType.IMPLICIT, 16, 5 );               // OTIR
      sys.AddOpcode( "outd", 0xedab, 0, AddressingType.IMPLICIT, 16 );                  // OUTD
      sys.AddOpcode( "otdr", 0xedbb, 0, AddressingType.IMPLICIT, 16, 5 );               // OTDR

      sys.AddOpcode( "sll", 0xCB30, 0, AddressingType.IMPLICIT, 77 )            // SLL,r
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, r ) } );
      sys.AddOpcode( "sll", 0xCB36, 0, AddressingType.IMPLICIT, 77 )            // SLL,(HL)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, hlIndirect ) } );


      return sys;
    }



  }
}
