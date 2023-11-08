using System;
using System.Collections.Generic;
using System.Text;
using static Tiny64.Opcode;

namespace Tiny64
{
  public partial class Processor
  {
    public static Processor Create68000()
    {
      var  sys = new Processor( "Motorola 68000" );

      sys.LittleEndian = false;


      var aRegisters = new List<ValidValue>() { new ValidValue( "a0", 0 ),
                                       new ValidValue( "a1", 1 ),
                                       new ValidValue( "a2", 2 ),
                                       new ValidValue( "a3", 3 ),
                                       new ValidValue( "a4", 4 ),
                                       new ValidValue( "a5", 5 ),
                                       new ValidValue( "a6", 6 ),
                                       new ValidValue( "a7", 7 ),
                                       new ValidValue( "sp", 7 ) };
      var dRegisters = new List<ValidValue>() { new ValidValue( "d0", 0 ),
                                       new ValidValue( "d1", 1 ),
                                       new ValidValue( "d2", 2 ),
                                       new ValidValue( "d3", 3 ),
                                       new ValidValue( "d4", 4 ),
                                       new ValidValue( "d5", 5 ),
                                       new ValidValue( "d6", 6 ),
                                       new ValidValue( "d7", 7 ) };
      var dRegistersDotW = new List<ValidValue>() { new ValidValue( "d0.w", 0 ),
                                       new ValidValue( "d1.w", 1 ),
                                       new ValidValue( "d2.w", 2 ),
                                       new ValidValue( "d3.w", 3 ),
                                       new ValidValue( "d4.w", 4 ),
                                       new ValidValue( "d5.w", 5 ),
                                       new ValidValue( "d6.w", 6 ),
                                       new ValidValue( "d7.w", 7 ) };
      var quickAdd = new List<ValidValue>() { new ValidValue( "1", 1 ),
                                       new ValidValue( "2", 2 ),
                                       new ValidValue( "3", 3 ),
                                       new ValidValue( "4", 4 ),
                                       new ValidValue( "5", 5 ),
                                       new ValidValue( "6", 6 ),
                                       new ValidValue( "7", 7 ),
                                       new ValidValue( "8", 0 ) };
      var bits = new List<ValidValue>() { new ValidValue( "0", 0 ),
                                       new ValidValue( "1", 1 ),
                                       new ValidValue( "2", 2 ),
                                       new ValidValue( "3", 3 ),
                                       new ValidValue( "4", 4 ),
                                       new ValidValue( "5", 5 ),
                                       new ValidValue( "6", 6 ),
                                       new ValidValue( "7", 7 ) };
      var immediatePrefix = new List<ValidValue>() { new ValidValue( "#" ) };
      var comma = new List<ValidValue>() { new ValidValue( "," ) };
      var empty = new List<ValidValue>();
      var sr = new List<ValidValue>() { new ValidValue( "sr" ) };

      var openingParenthesis = new List<ValidValue>() { new ValidValue( "(" ) };
      var openingParenthesisMinus = new List<ValidValue>() { new ValidValue( "-" ), new ValidValue( "(" ) };
      var closingParenthesisDotW = new List<ValidValue>() { new ValidValue( ")" ), new ValidValue( ".w" ) };
      var closingParenthesisDotL = new List<ValidValue>() { new ValidValue( ")" ), new ValidValue( ".l" ) };
      var closingParenthesis = new List<ValidValue>() { new ValidValue( ")" ) };
      var closingParenthesisPlus = new List<ValidValue>() { new ValidValue( ")" ), new ValidValue( "+" ) };

      // adda.w
      sys.AddOpcode( "adda.w", 0xD0FC0000, 2, AddressingType.IMMEDIATE_16BIT, 0 )          // adda.w #$20, a0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 25 ) } );

      sys.AddOpcode( "adda.w", 0xD0F80000, 2, AddressingType.INDIRECT, 0 )              // adda.w ($1234).w, a0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 25 ) } );

      sys.AddOpcode( "adda.l", 0xD1F9000000u, 3, AddressingType.INDIRECT, 0 )              // adda.l ($12345678).l, a0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_24BIT, openingParenthesis, closingParenthesisDotL, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 33 ) } );

      sys.AddOpcode( "adda.w", 0xD0C0, 0, AddressingType.IMPLICIT, 0 )                     // adda.w d0,a2
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 3 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 9 ) } );

      sys.AddOpcode( "adda.w", 0xD0D0, 0, AddressingType.INDIRECT, 0 )                     // adda.w (a0),a1
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 9 ) } );

      sys.AddOpcode( "adda.w", 0xD0E0, 0, AddressingType.INDIRECT, 0 )                     // adda.w -(a0),a1
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 9 ) } );

      sys.AddOpcode( "adda.w", 0xD0E80000, 2, AddressingType.INDIRECT, 0 )                      // adda.w $10(a0), a1
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_16BIT ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 16 ),
              new ValidValueGroup( closingParenthesis )
            }
          },
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 25 ) } );

      sys.AddOpcode( "adda.w", 0xD0D8, 0, AddressingType.INDIRECT, 0 )                          // adda.w (a0)+,a1
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 9 ) } );

      sys.AddOpcode( "adda.w", 0xD0F00000, 0, AddressingType.INDIRECT, 0 )                      // adda.w (a3,d1.w), a3
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 16 ),
              new ValidValueGroup( comma ),
              new ValidValueGroup( dRegistersDotW, OpcodePartialExpression.TOKEN_LIST, 12 ),
              new ValidValueGroup( closingParenthesis )
            }
          },
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 25 ) } );

      sys.AddOpcode( "adda.w", 0xD0F00000, 2, AddressingType.INDIRECT, 0 )                      // adda.w (a3,d1.w), a3
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_8BIT ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 16 ),
              new ValidValueGroup( comma ),
              new ValidValueGroup( dRegistersDotW, OpcodePartialExpression.TOKEN_LIST, 12 ),
              new ValidValueGroup( closingParenthesis )
            }
          },
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 25 ) } );

      // addi
      sys.AddOpcode( "addi.b", 0x06000000, 1, AddressingType.IMMEDIATE_8BIT, 0 )          // addi.w #$20, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 16 ) } );

      sys.AddOpcode( "addi.l", 0x068000000000, 4, AddressingType.IMMEDIATE_32BIT, 0 )          // addi.l #$12345678, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 32 ) } );

      sys.AddOpcode( "addi.w", 0x067800000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )             // addi.w #$1234, ($1234).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );

      sys.AddOpcode( "addi.w", 0x0679000000000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )             // addi.w #$1234, ($12345678).l
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 32 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT, openingParenthesis, closingParenthesisDotL, 0 ) } );

      sys.AddOpcode( "addi.l", 0x06B8000000000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )             // addi.l #$00000001, ($FF00).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );

      sys.AddOpcode( "addi.b", 0x06100000, 2, AddressingType.IMMEDIATE_8BIT, 0 )             // addi.b #$01, (a7)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 16 ) } );

      sys.AddOpcode( "addi.w", 0x066800000000, 4, AddressingType.INDIRECT, 0 )               // adda.i #$12, $34(a1)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_8BIT ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 32 ),
              new ValidValueGroup( closingParenthesis )
            }
          } } );

      sys.AddOpcode( "addi.l", 0x06A8000000000000, 4, AddressingType.INDIRECT, 0 )              // addi.l #$00000001, $FF(a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_8BIT ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 48 ),
              new ValidValueGroup( closingParenthesis )
            }
          } } );

      sys.AddOpcode( "addi.b", 0x06180000, 2, AddressingType.IMPLICIT, 0 )              // addi.b #$01, (a0)+
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 16 ) } );

      sys.AddOpcode( "addi.b", 0x06280000, 2, AddressingType.IMPLICIT, 0 )              // addi.b #$01, -(a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 16 ) } );

      sys.AddOpcode( "addq.b", 0x50380000, 2, AddressingType.IMMEDIATE_16BIT, 0 )        // addq.b #1, ($0001).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, immediatePrefix, quickAdd, empty, 25 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW ) } );

      sys.AddOpcode( "addq.b", 0x5000, 0, AddressingType.IMMEDIATE_8BIT, 0 )        // addq.b #1, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, immediatePrefix, quickAdd, empty, 9 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters ) } );

      sys.AddOpcode( "addq.w", 0x5040, 0, AddressingType.IMMEDIATE_8BIT, 0 )        // addq.w #1, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, immediatePrefix, quickAdd, empty, 9 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters ) } );

      sys.AddOpcode( "addq.w", 0x5050, 0, AddressingType.IMMEDIATE_8BIT, 0 )        // addq.w #1, (a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, immediatePrefix, quickAdd, empty, 9 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ) } );

      sys.AddOpcode( "addq.w", 0x5060, 1, AddressingType.IMMEDIATE_8BIT, 0 )        // addq.w #1, -(a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 9 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 0 ) } );

      sys.AddOpcode( "addq.b", 0x50280000, 2, AddressingType.IMPLICIT, 0 )          // addq.b #1, $01(a7)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, immediatePrefix, quickAdd, empty, 25 ),
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_8BIT ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 16 ),
              new ValidValueGroup( closingParenthesis )
            }
          } } );

      sys.AddOpcode( "addq.w", 0x50680000, 2, AddressingType.IMPLICIT, 0 )          // addq.w #1, $01(a7)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, immediatePrefix, quickAdd, empty, 25 ),
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_8BIT ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 16 ),
              new ValidValueGroup( closingParenthesis )
            }
          } } );

      sys.AddOpcode( "addq.w", 0x5058, 0, AddressingType.IMMEDIATE_8BIT, 0 )        // addq.w #1, (a0)+
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, immediatePrefix, quickAdd, empty, 9 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 0 ) } );

      sys.AddOpcode( "addq.w", 0x5048, 0, AddressingType.IMMEDIATE_8BIT, 0 )        // addq.w #1, a0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, immediatePrefix, quickAdd, empty, 9 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters ) } );

      sys.AddOpcode( "addq.l", 0x5088, 0, AddressingType.IMMEDIATE_8BIT, 0 )        // addq.l #1, a0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, immediatePrefix, quickAdd, empty, 9 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters ) } );

      sys.AddOpcode( "add.w", 0xD040, 0, AddressingType.IMPLICIT, 0 )               // add.w d0,d1
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "add.w", 0xD0780000, 2, AddressingType.INDIRECT, 0 )           // add.w ($1234).w, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );

      sys.AddOpcode( "add.w", 0xD050, 0, AddressingType.INDIRECT, 0 )               // add.w (a0), d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "add.b", 0xD0280000, 2, AddressingType.IMMEDIATE_8BIT, 0 )                      // add.b $01(a0),d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_8BIT ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 16 ),
              new ValidValueGroup( closingParenthesis )
            }
          },
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );

      sys.AddOpcode( "add.w", 0xD060, 0, AddressingType.IMPLICIT, 0 )                     // add.w -(a0),d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "add.w", 0xD0680000, 2, AddressingType.IMMEDIATE_8BIT, 0 )                      // add.w $01(a0),d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_8BIT ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 16 ),
              new ValidValueGroup( closingParenthesis )
            }
          },
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );

      sys.AddOpcode( "add.w", 0xD058, 0, AddressingType.IMPLICIT, 0 )                     // add.w (a0)+,d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "add.l", 0xD088, 0, AddressingType.IMPLICIT, 0 )               // add.l a0,d1
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "add.w", 0xD1780000, 2, AddressingType.INDIRECT, 0 )           // add.w d0, ($0001).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );

      sys.AddOpcode( "add.w", 0xD150, 0, AddressingType.INDIRECT, 0 )               // add.w d0,(a7)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ) } );

      sys.AddOpcode( "add.w", 0xD168, 0, AddressingType.INDIRECT, 0 )               // add.w d0,-(a7)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 0 ) } );

      sys.AddOpcode( "add.w", 0xD1680000, 2, AddressingType.INDIRECT, 0 )          // add.w d0, $01(a7)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ),
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_8BIT ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 16 ),
              new ValidValueGroup( closingParenthesis )
            }
          } } );

      sys.AddOpcode( "add.l", 0xD1A80000, 2, AddressingType.INDIRECT, 0 )          // add.l d0, $01(a7)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ),
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_8BIT ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 16 ),
              new ValidValueGroup( closingParenthesis )
            }
          } } );

      sys.AddOpcode( "add.l", 0xD198, 0, AddressingType.INDIRECT, 0 )               // add.l d0,(a7)+
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 0 ) } );

      
      sys.AddOpcode( "andi.b", 0x02000000, 1, AddressingType.IMMEDIATE_8BIT, 0 )    // andi.b #$01, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 16 ) } );

      sys.AddOpcode( "andi.w", 0x02400000, 2, AddressingType.IMMEDIATE_16BIT, 0 )    // andi.w #$1234, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 16 ) } );

      sys.AddOpcode( "andi.l", 0x028000000000, 4, AddressingType.IMMEDIATE_32BIT, 0 )    // andi.l #$00000001, d7
      .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 32 ) } );

      sys.AddOpcode( "andi.b", 0x023800000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )             // andi.b #$12, ($1234).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );

      sys.AddOpcode( "andi.w", 0x027800000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )             // andi.w #$1234, ($1234).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );

      sys.AddOpcode( "andi.b", 0x02100000, 2, AddressingType.IMMEDIATE_8BIT, 0 )             // andi.b #$01, (a7)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 16 ) } );

      sys.AddOpcode( "andi.b", 0x02200000, 2, AddressingType.IMMEDIATE_8BIT, 0 )             // andi.b #$01, -(a7)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 16 ) } );

      sys.AddOpcode( "andi.b", 0x022800000000, 4, AddressingType.IMMEDIATE_8BIT, 0 )               // andi.i #$12, $34(a1)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_8BIT ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 32 ),
              new ValidValueGroup( closingParenthesis )
            }
          } } );

      sys.AddOpcode( "andi.b", 0x02180000, 2, AddressingType.IMMEDIATE_8BIT, 0 )             // andi.b #$01, (a7)+
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 16 ) } );

      sys.AddOpcode( "andi.w", 0x026800000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )               // andi.i #$1234, $34(a1)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_8BIT ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 32 ),
              new ValidValueGroup( closingParenthesis )
            }
          } } );

      sys.AddOpcode( "andi.b", 0x023000000000, 4, AddressingType.IMMEDIATE_8BIT, 0 )         // andi.b #$01,(a0,d7.w)       ;02 37 00 01 70 00
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 32 ),
              new ValidValueGroup( comma ),
              new ValidValueGroup( dRegistersDotW, OpcodePartialExpression.TOKEN_LIST, 12 ),
              new ValidValueGroup( closingParenthesis )
            }
          } } );

      sys.AddOpcode( "andi.w", 0x027000000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )    // andi.w #$0001, $FF(a0,d7.w)           ;02 70 00 01 70 FF
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_8BIT ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 32 ),
              new ValidValueGroup( comma ),
              new ValidValueGroup( dRegistersDotW, OpcodePartialExpression.TOKEN_LIST, 12 ),
              new ValidValueGroup( closingParenthesis )
            }
          } } );

      sys.AddOpcode( "andi", 0x027C0000, 2, AddressingType.IMMEDIATE_16BIT, 0 )     // andi #$0001,sr      ;02 7C 00 01
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, sr ) } );

      sys.AddOpcode( "and.b", 0xC000, 0, AddressingType.IMPLICIT, 0 )            // and.b d0,d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "and.b", 0xC0380000, 2, AddressingType.INDIRECT, 0 )       // and.b ($0001).w, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );

      sys.AddOpcode( "and.w", 0xC0780000, 2, AddressingType.INDIRECT, 0 )       // and.w ($0001).w, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );

      sys.AddOpcode( "and.w", 0xC060, 0, AddressingType.INDIRECT, 0 )         // and.w -(a7), d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "and.w", 0xC050, 0, AddressingType.INDIRECT, 0 )           // and.w (a7), d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "and.w", 0xC0680000, 2, AddressingType.INDIRECT, 0 )      // and.w $01(a7), d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_8BIT ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 16 ),
              new ValidValueGroup( closingParenthesis )
            }
          },
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );
      
      sys.AddOpcode( "and.w", 0xC058, 0, AddressingType.INDIRECT, 0 )         // and.w (a7)+, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "and.w", 0xC1780000, 2, AddressingType.INDIRECT, 0 )     // and.w d0, ($0001).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );

      sys.AddOpcode( "and.w", 0xC150, 0, AddressingType.INDIRECT, 0 )         // and.w d0,(a7)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ) } );

      sys.AddOpcode( "and.w", 0xC160, 0, AddressingType.INDIRECT, 0 )         // and.w d0,-(a7)            ;C1 67
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 0 ) } );

      sys.AddOpcode( "and.w", 0xC1680000, 2, AddressingType.INDIRECT, 0 )     // and.w d0, $01(a7)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ),
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_8BIT ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 16 ),
              new ValidValueGroup( closingParenthesis )
            }
          } } );

      sys.AddOpcode( "and.w", 0xC158, 0, AddressingType.INDIRECT, 0 )         // and.w d0,(a7)+            ;C1 5F
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 0 ) } );

      sys.AddOpcode( "bra.s", 0x6000, 1, AddressingType.RELATIVE, 0 )        // bra.s label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT ) } );

      sys.AddOpcode( "bra.w", 0x60000000, 1, AddressingType.RELATIVE_16, 0 ) // bra.w label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_16BIT ) } );

      sys.AddOpcode( "bsr.s", 0x6100, 1, AddressingType.RELATIVE, 0 )        // bsr.s label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT ) } );

      sys.AddOpcode( "bhi.s", 0x6200, 1, AddressingType.RELATIVE, 0 )        // bhi.s label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT ) } );

      sys.AddOpcode( "bls.s", 0x6300, 1, AddressingType.RELATIVE, 0 )        // bls.s label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT ) } );

      sys.AddOpcode( "bcc.s", 0x6400, 1, AddressingType.RELATIVE, 0 )        // bcc.s label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT ) } );

      sys.AddOpcode( "bcs.s", 0x6500, 1, AddressingType.RELATIVE, 0 )        // bcs.s label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT ) } );

      sys.AddOpcode( "bne.s", 0x6600, 1, AddressingType.RELATIVE, 0 )        // bne.s label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT ) } );

      sys.AddOpcode( "beq.s", 0x6700, 1, AddressingType.RELATIVE, 0 )        // beq.s label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT ) } );

      sys.AddOpcode( "bvc.s", 0x6800, 1, AddressingType.RELATIVE, 0 )        // bvc.s label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT ) } );

      sys.AddOpcode( "bvs.s", 0x6900, 1, AddressingType.RELATIVE, 0 )        // bvs.s label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT ) } );

      sys.AddOpcode( "bpl.s", 0x6A00, 1, AddressingType.RELATIVE, 0 )        // bpl.s label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT ) } );

      sys.AddOpcode( "bmi.s", 0x6B00, 1, AddressingType.RELATIVE, 0 )        // bmi.s label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT ) } );

      sys.AddOpcode( "bge.s", 0x6C00, 1, AddressingType.RELATIVE, 0 )        // bge.s label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT ) } );

      sys.AddOpcode( "blt.s", 0x6D00, 1, AddressingType.RELATIVE, 0 )        // blt.s label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT ) } );

      sys.AddOpcode( "bgt.s", 0x6E00, 1, AddressingType.RELATIVE, 0 )        // bgt.s label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT ) } );

      sys.AddOpcode( "ble.s", 0x6F00, 1, AddressingType.RELATIVE, 0 )        // ble.s label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT ) } );

      sys.AddOpcode( "btst", 0x083800000000, 2, AddressingType.IMMEDIATE_16BIT, 0 )    // btst #0, ($FF00).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, bits, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );

      sys.AddOpcode( "btst", 0x08000000, 2, AddressingType.INDIRECT, 0 )    // btst #0, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, bits, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 16 ) } );

      sys.AddOpcode( "btst", 0x08100000, 2, AddressingType.IMMEDIATE_8BIT, 0 )    // btst #0, (a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, bits, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 16 ) } );

      sys.AddOpcode( "btst", 0x082800000000, 2, AddressingType.INDIRECT, 0 )          // btst #0, $01(a7)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, bits, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_8BIT ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 32 ),
              new ValidValueGroup( closingParenthesis )
            }
          } } );

      return sys;
    }



  }
}
