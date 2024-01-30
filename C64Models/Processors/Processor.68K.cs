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
      var adRegisters = new List<ValidValue>() { new ValidValue( "d0", 0x8000 ),
                                       new ValidValue( "d1", 0x4000 ),
                                       new ValidValue( "d2", 0x2000 ),
                                       new ValidValue( "d3", 0x1000 ),
                                       new ValidValue( "d4", 0x0800 ),
                                       new ValidValue( "d5", 0x0400 ),
                                       new ValidValue( "d6", 0x0200 ),
                                       new ValidValue( "d7", 0x0100 ),
                                       new ValidValue( "a0", 0x0080 ),
                                       new ValidValue( "a1", 0x0040 ),
                                       new ValidValue( "a2", 0x0020 ),
                                       new ValidValue( "a3", 0x0010 ),
                                       new ValidValue( "a4", 0x0008 ),
                                       new ValidValue( "a5", 0x0004 ),
                                       new ValidValue( "a6", 0x0002 ),
                                       new ValidValue( "a7", 0x0001 ),
                                       new ValidValue( "sp", 0x0001 ) };
      var adRegistersReverse = new List<ValidValue>() { new ValidValue( "d0", 0x0001 ),
                                       new ValidValue( "d1", 0x0002 ),
                                       new ValidValue( "d2", 0x0004 ),
                                       new ValidValue( "d3", 0x0008 ),
                                       new ValidValue( "d4", 0x0010 ),
                                       new ValidValue( "d5", 0x0020 ),
                                       new ValidValue( "d6", 0x0040 ),
                                       new ValidValue( "d7", 0x0080 ),
                                       new ValidValue( "a0", 0x0100 ),
                                       new ValidValue( "a1", 0x0200 ),
                                       new ValidValue( "a2", 0x0400 ),
                                       new ValidValue( "a3", 0x0800 ),
                                       new ValidValue( "a4", 0x1000 ),
                                       new ValidValue( "a5", 0x2000 ),
                                       new ValidValue( "a6", 0x4000 ),
                                       new ValidValue( "a7", 0x8000 ),
                                       new ValidValue( "sp", 0x8000 ) };
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
      var sp = new List<ValidValue>() { new ValidValue( "sp", 7 ) };
      var pc = new List<ValidValue>() { new ValidValue( "pc" ) };
      var rangeToken = new List<ValidValue>() { new ValidValue( "-" ) };
      var separatorToken = new List<ValidValue>() { new ValidValue( "/" ) };

      var openingParenthesis = new List<ValidValue>() { new ValidValue( "(" ) };
      var openingParenthesisMinus = new List<ValidValue>() { new ValidValue( "-" ), new ValidValue( "(" ) };
      var closingParenthesisDotW = new List<ValidValue>() { new ValidValue( ")" ), new ValidValue( ".w" ) };
      var closingParenthesisDotL = new List<ValidValue>() { new ValidValue( ")" ), new ValidValue( ".l" ) };
      var closingParenthesis = new List<ValidValue>() { new ValidValue( ")" ) };
      var closingParenthesisPlus = new List<ValidValue>() { new ValidValue( ")" ), new ValidValue( "+" ) };

      // adda.w
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
      sys.AddOpcode( "adda", 0xD0F00000, 0, AddressingType.INDIRECT, 0 )                      // adda (a3,d1.w), a3
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

      sys.AddOpcode( "adda.w", 0xD0D0, 2, AddressingType.INDIRECT, 0 )                     // adda.w (a0), a1
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 9 ) } );

      sys.AddOpcode( "adda", 0xD0D0, 2, AddressingType.INDIRECT, 0 )                     // adda.w (a0), a1
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 9 ) } );



      sys.AddOpcode( "adda.w", 0xD0FC0000, 2, AddressingType.IMMEDIATE_16BIT, 0 )          // adda.w #$20, a0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 25 ) } );
      sys.AddOpcode( "adda", 0xD0FC0000, 2, AddressingType.IMMEDIATE_16BIT, 0 )          // adda #$20, a0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 25 ) } );


      sys.AddOpcode( "adda.w", 0xD0F80000, 2, AddressingType.INDIRECT, 0 )              // adda.w ($1234).w, a0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 25 ) } );
      sys.AddOpcode( "adda.w", 0xD0F80000, 2, AddressingType.INDIRECT, 0 )              // adda.w ($1234), a0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 25 ) } );
      sys.AddOpcode( "adda", 0xD0F80000, 2, AddressingType.INDIRECT, 0 )              // adda ($1234).w, a0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 25 ) } );
      sys.AddOpcode( "adda", 0xD0F80000, 2, AddressingType.INDIRECT, 0 )              // adda ($1234), a0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 25 ) } );


      sys.AddOpcode( "adda.l", 0xD1F900000000u, 6, AddressingType.INDIRECT, 0 )              // adda.l ($123456).l, a0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_24BIT, openingParenthesis, closingParenthesisDotL, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 33 ) } );
      sys.AddOpcode( "adda.l", 0xD1F900000000u, 6, AddressingType.INDIRECT, 0 )              // adda.l ($123456).l, a0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_24BIT, openingParenthesis, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 33 ) } );
      sys.AddOpcode( "adda", 0xD1F900000000u, 6, AddressingType.INDIRECT, 0 )              // adda ($123456).l, a0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_24BIT, openingParenthesis, closingParenthesisDotL, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 33 ) } );
      sys.AddOpcode( "adda", 0xD1F900000000u, 6, AddressingType.INDIRECT, 0 )              // adda ($123456), a0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_24BIT, openingParenthesis, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 33 ) } );


      sys.AddOpcode( "adda.w", 0xD0C0, 2, AddressingType.IMPLICIT, 0 )                     // adda.w d0,a2
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 9 ) } );
      sys.AddOpcode( "adda", 0xD0C0, 2, AddressingType.IMPLICIT, 0 )                     // adda d0,a2
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 9 ) } );


      sys.AddOpcode( "adda.w", 0xD0E0, 2, AddressingType.INDIRECT, 0 )                     // adda.w -(a0), a1
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 9 ) } );
      sys.AddOpcode( "adda", 0xD0E0, 2, AddressingType.INDIRECT, 0 )                     // adda -(a0), a1
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 9 ) } );


      sys.AddOpcode( "adda.w", 0xD0E80000, 4, AddressingType.INDIRECT, 0 )                      // adda.w $10(a0), a1
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
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 25 ) } );
      sys.AddOpcode( "adda", 0xD0E80000, 4, AddressingType.INDIRECT, 0 )                      // adda $10(a0), a1
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
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 25 ) } );


      sys.AddOpcode( "adda.w", 0xD0D8, 0, AddressingType.INDIRECT, 0 )                          // adda.w (a0)+,a1
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 9 ) } );
      sys.AddOpcode( "adda", 0xD0D8, 0, AddressingType.INDIRECT, 0 )                          // adda (a0)+,a1
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 9 ) } );


      sys.AddOpcode( "adda.w", 0xD0F00000, 2, AddressingType.INDIRECT, 0 )                      // adda.w $12(a3,d1.w), a3
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_7BIT ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 16 ),
              new ValidValueGroup( comma ),
              new ValidValueGroup( dRegistersDotW, OpcodePartialExpression.TOKEN_LIST, 12 ),
              new ValidValueGroup( closingParenthesis )
            }
          },
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 25 ) } );
      sys.AddOpcode( "adda", 0xD0F00000, 2, AddressingType.INDIRECT, 0 )                      // adda $12(a3,d1.w), a3
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_7BIT ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 16 ),
              new ValidValueGroup( comma ),
              new ValidValueGroup( dRegistersDotW, OpcodePartialExpression.TOKEN_LIST, 12 ),
              new ValidValueGroup( closingParenthesis )
            }
          },
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 25 ) } );
      sys.AddOpcode( "adda.w", 0xD0F00000, 2, AddressingType.INDIRECT, 0 )                      // adda.w $12(a3,d1), a3
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_7BIT ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 16 ),
              new ValidValueGroup( comma ),
              new ValidValueGroup( dRegisters, OpcodePartialExpression.TOKEN_LIST, 12 ),
              new ValidValueGroup( closingParenthesis )
            }
          },
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 25 ) } );
      sys.AddOpcode( "adda", 0xD0F00000, 2, AddressingType.INDIRECT, 0 )                      // adda $12(a3,d1), a3
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_7BIT ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 16 ),
              new ValidValueGroup( comma ),
              new ValidValueGroup( dRegisters, OpcodePartialExpression.TOKEN_LIST, 12 ),
              new ValidValueGroup( closingParenthesis )
            }
          },
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 25 ) } );


      // addi #$x, dx
      sys.AddOpcode( "addi.b", 0x06000000, 1, AddressingType.IMMEDIATE_8BIT, 0 )          // addi.b #$20, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 16 ) } );

      sys.AddOpcode( "addi.w", 0x068000000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )          // addi.w #$1234, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 16 ) } );
      sys.AddOpcode( "addi", 0x068000000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )          // addi #$1234, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 16 ) } );


      sys.AddOpcode( "addi.l", 0x068000000000, 4, AddressingType.IMMEDIATE_32BIT, 0 )          // addi.l #$12345678, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 32 ) } );


      // addi $x, ($x)
      sys.AddOpcode( "addi", 0x06200000, 2, AddressingType.IMPLICIT, 0 )              // addi #$01, -(a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 16 ) } );

      sys.AddOpcode( "addi", 0x06100000, 2, AddressingType.IMMEDIATE_8BIT, 0 )             // addi #$01, (a7)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 16 ) } );

      sys.AddOpcode( "addi.w", 0x067800000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )             // addi.w #$1234, ($1234).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );
      sys.AddOpcode( "addi", 0x067800000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )             // addi #$1234, ($1234).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );
      sys.AddOpcode( "addi.w", 0x067800000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )             // addi.w #$1234, ($1234)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ) } );

      sys.AddOpcode( "addi.w", 0x0679000000000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )             // addi.w #$1234, ($12345678).l
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 32 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT, openingParenthesis, closingParenthesisDotL, 0 ) } );
      sys.AddOpcode( "addi", 0x0679000000000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )             // addi #$1234, ($12345678).l
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 32 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT, openingParenthesis, closingParenthesisDotL, 0 ) } );
      sys.AddOpcode( "addi", 0x0679000000000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )             // addi #$1234, ($12345678)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 32 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT, openingParenthesis, closingParenthesis, 0 ) } );

      sys.AddOpcode( "addi.l", 0x06B8000000000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )             // addi.l #$00000001, ($FF00).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );
      sys.AddOpcode( "addi.l", 0x06B8000000000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )             // addi.l #$00000001, ($FF00)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ) } );


      sys.AddOpcode( "addi.b", 0x06100000, 2, AddressingType.IMMEDIATE_8BIT, 0 )             // addi.b #$01, (a7)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 16 ) } );

      sys.AddOpcode( "addi.w", 0x066800000000, 4, AddressingType.INDIRECT, 0 )               // adda.w #$12, $34(a1)
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
      sys.AddOpcode( "addi", 0x066800000000, 4, AddressingType.INDIRECT, 0 )               // adda.w #$12, $34(a1)
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
      sys.AddOpcode( "addi", 0x06180000, 2, AddressingType.IMPLICIT, 0 )              // addi #$01, (a0)+
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 16 ) } );

      sys.AddOpcode( "addi.b", 0x06200000, 2, AddressingType.IMPLICIT, 0 )              // addi.b #$01, -(a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 16 ) } );

      sys.AddOpcode( "addq", 0x5050, 0, AddressingType.IMMEDIATE_8BIT, 0 )        // addq #1, (a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, immediatePrefix, quickAdd, empty, 9 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ) } );

      sys.AddOpcode( "addq.b", 0x50380000, 2, AddressingType.IMMEDIATE_16BIT, 0 )        // addq.b #1, ($0001).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, immediatePrefix, quickAdd, empty, 25 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW ) } );
      sys.AddOpcode( "addq", 0x50380000, 2, AddressingType.IMMEDIATE_16BIT, 0 )        // addq #1, ($0001).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, immediatePrefix, quickAdd, empty, 25 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW ) } );
      sys.AddOpcode( "addq.b", 0x50380000, 2, AddressingType.IMMEDIATE_16BIT, 0 )        // addq.b #1, ($0001)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, immediatePrefix, quickAdd, empty, 25 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis ) } );
      sys.AddOpcode( "addq", 0x50380000, 2, AddressingType.IMMEDIATE_16BIT, 0 )        // addq #1, ($0001)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, immediatePrefix, quickAdd, empty, 25 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis ) } );

      sys.AddOpcode( "addq.b", 0x5000, 0, AddressingType.IMMEDIATE_8BIT, 0 )        // addq.b #1, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, immediatePrefix, quickAdd, empty, 9 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters ) } );

      sys.AddOpcode( "addq.w", 0x5040, 0, AddressingType.IMMEDIATE_8BIT, 0 )        // addq.w #1, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, immediatePrefix, quickAdd, empty, 9 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters ) } );
      sys.AddOpcode( "addq", 0x5040, 0, AddressingType.IMMEDIATE_8BIT, 0 )        // addq #1, d0
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
      sys.AddOpcode( "addq", 0x5060, 1, AddressingType.IMMEDIATE_8BIT, 0 )        // addq #1, -(a0)
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
      sys.AddOpcode( "addq", 0x50680000, 2, AddressingType.IMPLICIT, 0 )          // addq #1, $01(a7)
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
      sys.AddOpcode( "addq", 0x5058, 0, AddressingType.IMMEDIATE_8BIT, 0 )        // addq #1, (a0)+
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
      sys.AddOpcode( "addq", 0x5088, 0, AddressingType.IMMEDIATE_8BIT, 0 )        // addq #1, a0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, immediatePrefix, quickAdd, empty, 9 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters ) } );

      sys.AddOpcode( "add.w", 0xD040, 0, AddressingType.IMPLICIT, 0 )               // add.w d0,d1
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );
      sys.AddOpcode( "add", 0xD040, 0, AddressingType.IMPLICIT, 0 )               // add d0,d1
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "add.w", 0xD050, 0, AddressingType.INDIRECT, 0 )               // add.w (a0), d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );
      sys.AddOpcode( "add", 0xD050, 0, AddressingType.INDIRECT, 0 )               // add (a0), d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "add.w", 0xD0780000, 2, AddressingType.INDIRECT, 0 )           // add.w ($1234).w, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );
      sys.AddOpcode( "add.w", 0xD0780000, 2, AddressingType.INDIRECT, 0 )           // add.w ($1234), d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );
      sys.AddOpcode( "add", 0xD0780000, 2, AddressingType.INDIRECT, 0 )           // add ($1234).w, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );
      sys.AddOpcode( "add", 0xD0780000, 2, AddressingType.INDIRECT, 0 )           // add ($1234), d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );

      sys.AddOpcode( "add", 0xD060, 0, AddressingType.IMPLICIT, 0 )                     // add -(a0),d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 0 ),
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
      sys.AddOpcode( "add", 0xD0280000, 2, AddressingType.IMMEDIATE_8BIT, 0 )                      // add $01(a0),d0
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
      sys.AddOpcode( "add", 0xD058, 0, AddressingType.IMPLICIT, 0 )                     // add (a0)+,d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "add.l", 0xD088, 0, AddressingType.IMPLICIT, 0 )               // add.l a0,d1
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );
      sys.AddOpcode( "add.w", 0xD088, 0, AddressingType.IMPLICIT, 0 )               // add.w a0,d1
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );
      sys.AddOpcode( "add", 0xD088, 0, AddressingType.IMPLICIT, 0 )               // add a0,d1
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "add.l", 0x5080, 0, AddressingType.IMMEDIATE_8BIT, 0 )         // add.l #1, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, immediatePrefix, quickAdd, empty, 9 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters ) } );
      sys.AddOpcode( "add", 0x5080, 0, AddressingType.IMMEDIATE_8BIT, 0 )         // add #1, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, immediatePrefix, quickAdd, empty, 9 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters ) } );
      sys.AddOpcode( "add.w", 0x5040, 0, AddressingType.IMMEDIATE_8BIT, 0 )         // add.w #1, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, immediatePrefix, quickAdd, empty, 9 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters ) } );
      sys.AddOpcode( "add.b", 0x5000, 0, AddressingType.IMMEDIATE_8BIT, 0 )         // add.b #1, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, immediatePrefix, quickAdd, empty, 9 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters ) } );


      sys.AddOpcode( "add.w", 0xD150, 0, AddressingType.INDIRECT, 0 )               // add.w d0,(a7)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ) } );
      sys.AddOpcode( "add", 0xD150, 0, AddressingType.INDIRECT, 0 )               // add d0,(a7)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ) } );

      sys.AddOpcode( "add.w", 0xD1780000, 2, AddressingType.INDIRECT, 0 )           // add.w d0, ($0001).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );
      sys.AddOpcode( "add", 0xD1780000, 2, AddressingType.INDIRECT, 0 )           // add d0, ($0001).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );
      sys.AddOpcode( "add.w", 0xD1780000, 2, AddressingType.INDIRECT, 0 )           // add.w d0, ($0001).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ) } );
      sys.AddOpcode( "add", 0xD1780000, 2, AddressingType.INDIRECT, 0 )           // add d0, ($0001)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ) } );

      sys.AddOpcode( "add.w", 0xD160, 0, AddressingType.INDIRECT, 0 )               // add.w d0,-(a7)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 0 ) } );
      sys.AddOpcode( "add", 0xD160, 0, AddressingType.INDIRECT, 0 )               // add d0,-(a7)
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
      sys.AddOpcode( "add", 0xD1A80000, 2, AddressingType.INDIRECT, 0 )          // add d0, $01(a7)
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
      sys.AddOpcode( "add", 0xD198, 0, AddressingType.INDIRECT, 0 )               // add d0,(a7)+
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
      sys.AddOpcode( "andi", 0x02400000, 2, AddressingType.IMMEDIATE_16BIT, 0 )    // andi #$1234, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 16 ) } );

      sys.AddOpcode( "andi.l", 0x028000000000, 4, AddressingType.IMMEDIATE_32BIT, 0 )    // andi.l #$00000001, d7
      .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 32 ) } );

      sys.AddOpcode( "andi.b", 0x02100000, 2, AddressingType.IMMEDIATE_8BIT, 0 )             // andi.b #$01, (a7)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 16 ) } );
      sys.AddOpcode( "andi", 0x02100000, 2, AddressingType.IMMEDIATE_8BIT, 0 )             // andi #$01, (a7)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 16 ) } );


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
      sys.AddOpcode( "andi", 0x023000000000, 4, AddressingType.IMMEDIATE_8BIT, 0 )         // andi #$01,(a0,d7.w)       ;02 37 00 01 70 00
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
      sys.AddOpcode( "andi.b", 0x023000000000, 4, AddressingType.IMMEDIATE_8BIT, 0 )         // andi.b #$01,(a0,d7)       ;02 37 00 01 70 00
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 32 ),
              new ValidValueGroup( comma ),
              new ValidValueGroup( dRegisters, OpcodePartialExpression.TOKEN_LIST, 12 ),
              new ValidValueGroup( closingParenthesis )
            }
          } } );


      sys.AddOpcode( "andi.b", 0x023800000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )             // andi.b #$12, ($1234).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );
      sys.AddOpcode( "andi.b", 0x023800000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )             // andi.b #$12, ($1234)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ) } );

      sys.AddOpcode( "andi.w", 0x027800000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )             // andi.w #$1234, ($1234).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );
      sys.AddOpcode( "andi.w", 0x027800000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )             // andi.w #$1234, ($1234)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ) } );
      sys.AddOpcode( "andi", 0x027800000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )             // andi #$1234, ($1234)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ) } );

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


      sys.AddOpcode( "andi.w", 0x027000000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )    // andi.w #$0001, $FF(a0,d7.w)           ;02 70 00 01 70 FF
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_7BIT ),
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
      sys.AddOpcode( "and", 0xC000, 0, AddressingType.IMPLICIT, 0 )            // and d0,d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );


      sys.AddOpcode( "and.w", 0xC060, 0, AddressingType.INDIRECT, 0 )         // and.w -(a7), d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );
      sys.AddOpcode( "and", 0xC060, 0, AddressingType.INDIRECT, 0 )         // and -(a7), d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );


      sys.AddOpcode( "and.w", 0xC160, 0, AddressingType.INDIRECT, 0 )         // and.w d0,-(a7)            ;C1 67
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 0 ) } );
      sys.AddOpcode( "and", 0xC160, 0, AddressingType.INDIRECT, 0 )         // and d0,-(a7)            ;C1 67
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 0 ) } );

      sys.AddOpcode( "and.b", 0xC0380000, 2, AddressingType.INDIRECT, 0 )       // and.b ($0001).w, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );
      sys.AddOpcode( "and.b", 0xC0380000, 2, AddressingType.INDIRECT, 0 )       // and.b ($0001), d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );

      sys.AddOpcode( "and.w", 0xC050, 0, AddressingType.INDIRECT, 0 )           // and.w (a7), d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );
      sys.AddOpcode( "and", 0xC050, 0, AddressingType.INDIRECT, 0 )           // and (a7), d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );


      sys.AddOpcode( "and.w", 0xC0780000, 2, AddressingType.INDIRECT, 0 )       // and.w ($0001).w, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );
      sys.AddOpcode( "and.w", 0xC0780000, 2, AddressingType.INDIRECT, 0 )       // and.w ($0001), d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );
      sys.AddOpcode( "and", 0xC0780000, 2, AddressingType.INDIRECT, 0 )       // and ($0001).w, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );
      sys.AddOpcode( "and", 0xC0780000, 2, AddressingType.INDIRECT, 0 )       // and ($0001), d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );

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
      sys.AddOpcode( "and", 0xC0680000, 2, AddressingType.INDIRECT, 0 )      // and $01(a7), d0
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
      sys.AddOpcode( "and", 0xC058, 0, AddressingType.INDIRECT, 0 )         // and (a7)+, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "and.w", 0xC150, 0, AddressingType.INDIRECT, 0 )         // and.w d0,(a7)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ) } );
      sys.AddOpcode( "and", 0xC150, 0, AddressingType.INDIRECT, 0 )         // and d0,(a7)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ) } );

      sys.AddOpcode( "and.w", 0xC1780000, 2, AddressingType.INDIRECT, 0 )     // and.w d0, ($0001).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );
      sys.AddOpcode( "and.w", 0xC1780000, 2, AddressingType.INDIRECT, 0 )     // and.w d0, ($0001)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ) } );
      sys.AddOpcode( "and", 0xC1780000, 2, AddressingType.INDIRECT, 0 )     // and.w d0, ($0001).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );
      sys.AddOpcode( "and", 0xC1780000, 2, AddressingType.INDIRECT, 0 )     // and.w d0, ($0001)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ) } );

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
      sys.AddOpcode( "and", 0xC1680000, 2, AddressingType.INDIRECT, 0 )     // and d0, $01(a7)
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
      sys.AddOpcode( "and", 0xC158, 0, AddressingType.INDIRECT, 0 )         // and d0,(a7)+            ;C1 5F
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 0 ) } );

      sys.AddOpcode( "bra.s", 0x6000, 1, AddressingType.RELATIVE, 0 )        // bra.s label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE ) } );
      sys.AddOpcode( "bra", 0x6000, 1, AddressingType.RELATIVE, 0 )        // bra label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE ) } );

      sys.AddOpcode( "bra.w", 0x60000000, 1, AddressingType.RELATIVE_16, 0 ) // bra.w label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_16BIT_RELATIVE ) } );

      sys.AddOpcode( "bsr.s", 0x6100, 1, AddressingType.RELATIVE, 0 )        // bsr.s label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE ) } );
      sys.AddOpcode( "bsr", 0x6100, 1, AddressingType.RELATIVE, 0 )        // bsr label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE ) } );

      sys.AddOpcode( "bhi.s", 0x6200, 1, AddressingType.RELATIVE, 0 )        // bhi.s label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE ) } );
      sys.AddOpcode( "bhi", 0x6200, 1, AddressingType.RELATIVE, 0 )        // bhi label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE ) } );

      sys.AddOpcode( "bls.s", 0x6300, 1, AddressingType.RELATIVE, 0 )        // bls.s label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE ) } );
      sys.AddOpcode( "bls", 0x6300, 1, AddressingType.RELATIVE, 0 )        // bls label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE ) } );

      sys.AddOpcode( "bcc.s", 0x6400, 1, AddressingType.RELATIVE, 0 )        // bcc.s label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE ) } );
      sys.AddOpcode( "bcc", 0x6400, 1, AddressingType.RELATIVE, 0 )        // bcc label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE ) } );

      sys.AddOpcode( "bcs.s", 0x6500, 1, AddressingType.RELATIVE, 0 )        // bcs.s label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE ) } );
      sys.AddOpcode( "bcs", 0x6500, 1, AddressingType.RELATIVE, 0 )        // bcs label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE ) } );

      sys.AddOpcode( "bne.s", 0x6600, 1, AddressingType.RELATIVE, 0 )        // bne.s label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE ) } );
      sys.AddOpcode( "bne", 0x6600, 1, AddressingType.RELATIVE, 0 )        // bne label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE ) } );

      sys.AddOpcode( "beq.s", 0x6700, 1, AddressingType.RELATIVE, 0 )        // beq.s label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE ) } );
      sys.AddOpcode( "beq", 0x6700, 1, AddressingType.RELATIVE, 0 )        // beq label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE ) } );

      sys.AddOpcode( "bvc.s", 0x6800, 1, AddressingType.RELATIVE, 0 )        // bvc.s label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE ) } );
      sys.AddOpcode( "bvc", 0x6800, 1, AddressingType.RELATIVE, 0 )        // bvc label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE ) } );

      sys.AddOpcode( "bvs.s", 0x6900, 1, AddressingType.RELATIVE, 0 )        // bvs.s label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE ) } );
      sys.AddOpcode( "bvs", 0x6900, 1, AddressingType.RELATIVE, 0 )        // bvs label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE ) } );

      sys.AddOpcode( "bpl.s", 0x6A00, 1, AddressingType.RELATIVE, 0 )        // bpl.s label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE ) } );
      sys.AddOpcode( "bpl", 0x6A00, 1, AddressingType.RELATIVE, 0 )        // bpl label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE ) } );

      sys.AddOpcode( "bmi.s", 0x6B00, 1, AddressingType.RELATIVE, 0 )        // bmi.s label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE ) } );
      sys.AddOpcode( "bmi", 0x6B00, 1, AddressingType.RELATIVE, 0 )        // bmi label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE ) } );

      sys.AddOpcode( "bge.s", 0x6C00, 1, AddressingType.RELATIVE, 0 )        // bge.s label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE ) } );
      sys.AddOpcode( "bge", 0x6C00, 1, AddressingType.RELATIVE, 0 )        // bge label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE ) } );

      sys.AddOpcode( "blt.s", 0x6D00, 1, AddressingType.RELATIVE, 0 )        // blt.s label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE ) } );
      sys.AddOpcode( "blt", 0x6D00, 1, AddressingType.RELATIVE, 0 )        // blt label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE ) } );

      sys.AddOpcode( "bgt.s", 0x6E00, 1, AddressingType.RELATIVE, 0 )        // bgt.s label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE ) } );
      sys.AddOpcode( "bgt", 0x6E00, 1, AddressingType.RELATIVE, 0 )        // bgt label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE ) } );

      sys.AddOpcode( "ble.s", 0x6F00, 1, AddressingType.RELATIVE, 0 )        // ble.s label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE ) } );
      sys.AddOpcode( "ble", 0x6F00, 1, AddressingType.RELATIVE, 0 )        // ble label
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_8BIT_RELATIVE ) } );

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

      sys.AddOpcode( "btst", 0x0839000000000000, 8, AddressingType.INDIRECT, 0 )          // btst #0, $123456   ;08 39 00 00 00 00 00 01
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, bits, empty, 32 ),
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_24BIT ) } );

      sys.AddOpcode( "btst", 0x0110, 0, AddressingType.INDIRECT, 0 )    // btst d0, (a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ) } );

      sys.AddOpcode( "btst", 0x01380000, 2, AddressingType.IMMEDIATE_16BIT, 0 )    // btst d0, ($FF00).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );
      sys.AddOpcode( "btst", 0x01380000, 2, AddressingType.IMMEDIATE_16BIT, 0 )    // btst d0, ($FF00)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ) } );

      sys.AddOpcode( "btst", 0x0100, 0, AddressingType.IMPLICIT, 0 )    // btst d0, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ) } );

      sys.AddOpcode( "btst", 0x01280000, 2, AddressingType.IMMEDIATE_8BIT, 0 )          // btst d0, $01(a7)
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

      sys.AddOpcode( "bclr", 0x08900000, 2, AddressingType.IMMEDIATE_8BIT, 0 )    // bclr #0, (a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, bits, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 16 ) } );

      sys.AddOpcode( "bclr", 0x08B800000000, 2, AddressingType.IMMEDIATE_16BIT, 0 )    // bclr #0, ($FF00).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, bits, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );
      sys.AddOpcode( "bclr", 0x08B800000000, 2, AddressingType.IMMEDIATE_16BIT, 0 )    // bclr #0, ($FF00)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, bits, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ) } );

      sys.AddOpcode( "bclr", 0x08800000, 2, AddressingType.INDIRECT, 0 )    // bclr #0, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, bits, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 16 ) } );

      sys.AddOpcode( "bclr", 0x08A800000000, 2, AddressingType.INDIRECT, 0 )          // bclr #0, $01(a7)
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

      sys.AddOpcode( "bclr", 0x0190, 0, AddressingType.INDIRECT, 0 )    // bclr d0, (a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ) } );

      sys.AddOpcode( "bclr", 0x01B80000, 2, AddressingType.IMMEDIATE_16BIT, 0 )    // bclr d0, ($FF00).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );
      sys.AddOpcode( "bclr", 0x01B80000, 2, AddressingType.IMMEDIATE_16BIT, 0 )    // bclr d0, ($FF00)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ) } );

      sys.AddOpcode( "bclr", 0x0180, 0, AddressingType.IMPLICIT, 0 )    // bclr d0, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ) } );

      sys.AddOpcode( "bclr", 0x01A80000, 2, AddressingType.IMMEDIATE_8BIT, 0 )          // bclr d0, $01(a7)
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


      sys.AddOpcode( "bset", 0x08D00000, 2, AddressingType.IMMEDIATE_8BIT, 0 )    // bset #0, (a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, bits, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 16 ) } );

      sys.AddOpcode( "bset", 0x08F800000000, 2, AddressingType.IMMEDIATE_16BIT, 0 )    // bset #0, ($FF00).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, bits, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );
      sys.AddOpcode( "bset", 0x08F800000000, 2, AddressingType.IMMEDIATE_16BIT, 0 )    // bset #0, ($FF00)  
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, bits, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ) } );

      sys.AddOpcode( "bset", 0x08C00000, 2, AddressingType.INDIRECT, 0 )    // bset #0, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, bits, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 16 ) } );

      sys.AddOpcode( "bset", 0x08E800000000, 2, AddressingType.INDIRECT, 0 )          // bset #0, $01(a7)
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

      sys.AddOpcode( "bset", 0x01D0, 0, AddressingType.INDIRECT, 0 )    // bset d0, (a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ) } );

      sys.AddOpcode( "bset", 0x01F80000, 2, AddressingType.IMMEDIATE_16BIT, 0 )    // bset d0, ($FF00).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );
      sys.AddOpcode( "bset", 0x01F80000, 2, AddressingType.IMMEDIATE_16BIT, 0 )    // bset d0, ($FF00)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ) } );

      sys.AddOpcode( "bset", 0x01C0, 0, AddressingType.IMPLICIT, 0 )    // bset d0, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ) } );

      sys.AddOpcode( "bset", 0x01E80000, 2, AddressingType.IMMEDIATE_8BIT, 0 )          // bset d0, $01(a7)
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

      sys.AddOpcode( "bchg", 0x08500000, 2, AddressingType.IMMEDIATE_8BIT, 0 )    // bchg #0, (a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, bits, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 16 ) } );

      sys.AddOpcode( "bchg", 0x087800000000, 2, AddressingType.IMMEDIATE_16BIT, 0 )    // bchg #0, ($FF00).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, bits, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );
      sys.AddOpcode( "bchg", 0x087800000000, 2, AddressingType.IMMEDIATE_16BIT, 0 )    // bchg #0, ($FF00)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, bits, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ) } );

      sys.AddOpcode( "bchg", 0x08400000, 2, AddressingType.INDIRECT, 0 )    // bchg #0, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, bits, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 16 ) } );

      sys.AddOpcode( "bchg", 0x086800000000, 2, AddressingType.INDIRECT, 0 )          // bchg #0, $01(a7)
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

      sys.AddOpcode( "bchg", 0x0150, 0, AddressingType.INDIRECT, 0 )    // bchg d0, (a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ) } );

      sys.AddOpcode( "bchg", 0x01780000, 2, AddressingType.IMMEDIATE_16BIT, 0 )    // bchg d0, ($FF00).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );
      sys.AddOpcode( "bchg", 0x01780000, 2, AddressingType.IMMEDIATE_16BIT, 0 )    // bchg d0, ($FF00)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ) } );

      sys.AddOpcode( "bchg", 0x0140, 0, AddressingType.IMPLICIT, 0 )    // bchg d0, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ) } );

      sys.AddOpcode( "bchg", 0x01680000, 2, AddressingType.IMMEDIATE_8BIT, 0 )          // bchg d0, $01(a7)
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

      sys.AddOpcode( "clr.w", 0x4240, 0, AddressingType.IMPLICIT, 0 )    // clr.w d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ) } );
      sys.AddOpcode( "clr", 0x4240, 0, AddressingType.IMPLICIT, 0 )    // clr d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ) } );

      sys.AddOpcode( "clr.l", 0x4280, 0, AddressingType.IMPLICIT, 0 )    // clr.l d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ) } );

      sys.AddOpcode( "clr.l", 0x4290, 0, AddressingType.IMPLICIT, 0 )    // clr.l (a7)   
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ) } );
      sys.AddOpcode( "clr", 0x4290, 0, AddressingType.IMPLICIT, 0 )    // clr (a7)   
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ) } );

      sys.AddOpcode( "clr.w", 0x42780000, 2, AddressingType.INDIRECT, 0 )    // clr.w ($1234).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );
      sys.AddOpcode( "clr.w", 0x42780000, 2, AddressingType.INDIRECT, 0 )    // clr.w ($1234)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ) } );
      sys.AddOpcode( "clr", 0x42780000, 2, AddressingType.INDIRECT, 0 )    // clr ($1234)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ) } );
      sys.AddOpcode( "clr", 0x42780000, 2, AddressingType.INDIRECT, 0 )    // clr ($1234).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );

      sys.AddOpcode( "clr.b", 0x42280000, 2, AddressingType.INDIRECT, 0 )    // clr.b $01( a7 )
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
          } } );

      sys.AddOpcode( "clr.w", 0x42680000, 2, AddressingType.INDIRECT, 0 )    // clr.w $01( a7 )
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
          } } );

      sys.AddOpcode( "clr.l", 0x4298, 0, AddressingType.IMPLICIT, 0 )    // clr.l (a7)+ 
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 0 ) } );
      sys.AddOpcode( "clr", 0x4298, 0, AddressingType.IMPLICIT, 0 )    // clr (a7)+ 
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 0 ) } );

      sys.AddOpcode( "clr.l", 0x42A0, 0, AddressingType.IMPLICIT, 0 )    // clr.l -(a7)   
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 0 ) } );
      sys.AddOpcode( "clr", 0x42A0, 0, AddressingType.IMPLICIT, 0 )    // clr -(a7)   
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 0 ) } );

      sys.AddOpcode( "clr.l", 0x42A80000, 2, AddressingType.INDIRECT, 0 )    // clr.l $12(a7)   
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
          } } );
      sys.AddOpcode( "clr", 0x42A80000, 2, AddressingType.INDIRECT, 0 )    // clr $12(a7)   
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
          } } );

      sys.AddOpcode( "cmpa.w", 0xB0FC0000, 2, AddressingType.IMMEDIATE_16BIT, 0 )     // cmpa.w #$0001,a7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 25 ) } );
      sys.AddOpcode( "cmpa", 0xB0FC0000, 2, AddressingType.IMMEDIATE_16BIT, 0 )     // cmpa #$0001,a7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 25 ) } );

      sys.AddOpcode( "cmpa.l", 0xB1FC00000000, 4, AddressingType.IMMEDIATE_32BIT, 0 ) // cmpa.l #$00000001,a7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 41 ) } );

      sys.AddOpcode( "cmpa.l", 0xB1F80000, 2, AddressingType.IMMEDIATE_32BIT, 0 )   // cmpa.l ($0001).w,a7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 25 ) } );

      sys.AddOpcode( "cmpa.w", 0xB0C0, 0, AddressingType.IMPLICIT, 0 )    // cmpa.w d7, a0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 9 ) } );
      sys.AddOpcode( "cmpa", 0xB0C0, 0, AddressingType.IMPLICIT, 0 )    // cmpa d7, a0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 9 ) } );

      sys.AddOpcode( "cmpa.l", 0xB1C8, 0, AddressingType.IMPLICIT, 0 )    // cmpa.w a7, a0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 9 ) } );
      sys.AddOpcode( "cmpa", 0xB1C8, 0, AddressingType.IMPLICIT, 0 )    // cmpa a7, a0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 9 ) } );

      sys.AddOpcode( "cmpa.l", 0xB1D0, 0, AddressingType.IMPLICIT, 0 )    // cmpa.l (a7), a0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 9 ) } );
      sys.AddOpcode( "cmpa", 0xB1D0, 0, AddressingType.IMPLICIT, 0 )    // cmpa (a7), a0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 9 ) } );

      sys.AddOpcode( "cmpa.l", 0xB1E80000, 2, AddressingType.INDIRECT, 0 )    // cmpa.l $01(a7), a0
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
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 25 ) } );
      sys.AddOpcode( "cmpa", 0xB1E80000, 2, AddressingType.INDIRECT, 0 )    // cmpa $01(a7), a0
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
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 25 ) } );

      sys.AddOpcode( "cmpi.b", 0x0C100000, 2, AddressingType.IMMEDIATE_8BIT, 0 )    // cmpi.b #$01, (a7)           ;0C 17 00 01
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 16 ) } );
      sys.AddOpcode( "cmpi", 0x0C100000, 2, AddressingType.IMMEDIATE_8BIT, 0 )    // cmpi #$01, (a7)           ;0C 17 00 01
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 16 ) } );


      sys.AddOpcode( "cmpi.b", 0x0C3800000000, 2, AddressingType.IMMEDIATE_8BIT, 0 )     // cmpi.b #$12, ($1234).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );
      sys.AddOpcode( "cmpi.b", 0x0C3800000000, 2, AddressingType.IMMEDIATE_8BIT, 0 )     // cmpi.b #$12, ($1234)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ) } );

      sys.AddOpcode( "cmpi.w", 0x0C7800000000, 2, AddressingType.IMMEDIATE_16BIT, 0 )    // cmpi.w #$1234, ($1234).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );
      sys.AddOpcode( "cmpi.w", 0x0C7800000000, 2, AddressingType.IMMEDIATE_16BIT, 0 )    // cmpi.w #$1234, ($1234)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ) } );
      sys.AddOpcode( "cmpi", 0x0C7800000000, 2, AddressingType.IMMEDIATE_16BIT, 0 )    // cmpi.w #$1234, ($1234).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );
      sys.AddOpcode( "cmpi", 0x0C7800000000, 2, AddressingType.IMMEDIATE_16BIT, 0 )    // cmpi.w #$1234, ($1234)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ) } );

      sys.AddOpcode( "cmpi.l", 0x0CB8000000000000, 4, AddressingType.IMMEDIATE_32BIT, 0 )    // cmpi.l #$12345678, ($1234).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );
      sys.AddOpcode( "cmpi.l", 0x0CB8000000000000, 4, AddressingType.IMMEDIATE_32BIT, 0 )    // cmpi.l #$12345678, ($1234)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ) } );

      sys.AddOpcode( "cmpi.w", 0x0C79000000000000, 4, AddressingType.IMMEDIATE_8BIT, 0 )    // cmpi.w #$12, ($12345678).l
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 32 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT, openingParenthesis, closingParenthesisDotL, 0 ) } );
      sys.AddOpcode( "cmpi.w", 0x0C79000000000000, 4, AddressingType.IMMEDIATE_8BIT, 0 )    // cmpi.w #$12, ($12345678)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 32 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT, openingParenthesis, closingParenthesis, 0 ) } );
      sys.AddOpcode( "cmpi", 0x0C79000000000000, 4, AddressingType.IMMEDIATE_8BIT, 0 )    // cmpi #$12, ($12345678).l
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 32 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT, openingParenthesis, closingParenthesisDotL, 0 ) } );
      sys.AddOpcode( "cmpi", 0x0C79000000000000, 4, AddressingType.IMMEDIATE_8BIT, 0 )    // cmpi #$12, ($12345678)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 32 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT, openingParenthesis, closingParenthesis, 0 ) } );

      sys.AddOpcode( "cmpi.b", 0x0C000000, 2, AddressingType.IMMEDIATE_8BIT, 0 )    // cmpi.b #$01, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 16 ) } );

      sys.AddOpcode( "cmpi.w", 0x0C400000, 2, AddressingType.IMMEDIATE_16BIT, 0 )   // cmpi.w #$1234, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 16 ) } );
      sys.AddOpcode( "cmpi", 0x0C400000, 2, AddressingType.IMMEDIATE_16BIT, 0 )   // cmpi #$1234, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 16 ) } );

      sys.AddOpcode( "cmpi.l", 0x0C8000000000, 4, AddressingType.IMMEDIATE_32BIT, 0 )   // cmpi.l #$12345678, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 32 ) } );

      sys.AddOpcode( "cmpi.b", 0x0C200000, 2, AddressingType.IMMEDIATE_8BIT, 0 )  // cmpi.b #$01, -(a7)          ;0C 1F 00 01
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 16 ) } );
      sys.AddOpcode( "cmpi", 0x0C200000, 2, AddressingType.IMMEDIATE_8BIT, 0 )  // cmpi #$01, -(a7)          ;0C 1F 00 01
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 16 ) } );

      sys.AddOpcode( "cmpi.b", 0x0C2800000000, 2, AddressingType.INDIRECT, 0 )    // cmpi.b #$01, $FF(a7)           ;0C 2F 00 01 00 FF
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

      sys.AddOpcode( "cmpi.w", 0x0C6800000000, 2, AddressingType.INDIRECT, 0 )    // cmpi.w #$0001, $FF(a7)         ;0C 6F 00 01 00 FF
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
      sys.AddOpcode( "cmpi", 0x0C6800000000, 2, AddressingType.INDIRECT, 0 )    // cmpi #$0001, $FF(a7)         ;0C 6F 00 01 00 FF
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

      sys.AddOpcode( "cmpi.b", 0x0C180000, 2, AddressingType.IMMEDIATE_8BIT, 0 )  // cmpi.b #$01, (a7)+          ;0C 1F 00 01
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 16 ) } );
      sys.AddOpcode( "cmpi", 0x0C180000, 2, AddressingType.IMMEDIATE_8BIT, 0 )  // cmpi #$01, (a7)+          ;0C 1F 00 01
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 16 ) } );


      sys.AddOpcode( "cmp.b", 0xB000, 0, AddressingType.IMPLICIT, 0 )    // cmp.b d0, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );
      sys.AddOpcode( "cmp.w", 0xB000, 0, AddressingType.IMPLICIT, 0 )    // cmp.w d0, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );
      sys.AddOpcode( "cmp", 0xB000, 0, AddressingType.IMPLICIT, 0 )    // cmp d0, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "cmp.l", 0xB080, 0, AddressingType.IMPLICIT, 0 )    // cmp.l d0, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );


      sys.AddOpcode( "cmp.w", 0xB050, 0, AddressingType.INDIRECT, 0 )       // cmp.w (a7), d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );
      sys.AddOpcode( "cmp", 0xB050, 0, AddressingType.INDIRECT, 0 )       // cmp (a7), d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );


      sys.AddOpcode( "cmp.w", 0xB0780000, 2, AddressingType.INDIRECT, 0 )   // cmp.w ($0001).w, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );
      sys.AddOpcode( "cmp.w", 0xB0780000, 2, AddressingType.INDIRECT, 0 )   // cmp.w ($0001), d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );
      sys.AddOpcode( "cmp", 0xB0780000, 2, AddressingType.INDIRECT, 0 )   // cmp ($0001).w, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );
      sys.AddOpcode( "cmp", 0xB0780000, 2, AddressingType.INDIRECT, 0 )   // cmp ($0001), d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );

      sys.AddOpcode( "cmp.l", 0xB0B80000, 2, AddressingType.INDIRECT, 0 )   // cmp.l ($0001).w, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );
      sys.AddOpcode( "cmp.l", 0xB0B80000, 2, AddressingType.INDIRECT, 0 )   // cmp.l ($0001), d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );



      sys.AddOpcode( "cmp.w", 0xB058, 0, AddressingType.INDIRECT, 0 )       // cmp.w (a7)+, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );
      sys.AddOpcode( "cmp.b", 0xB058, 0, AddressingType.INDIRECT, 0 )       // cmp.b (a7)+, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );
      sys.AddOpcode( "cmp", 0xB058, 0, AddressingType.INDIRECT, 0 )       // cmp (a7)+, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );


      sys.AddOpcode( "cmp.b", 0xB020, 0, AddressingType.INDIRECT, 0 )       // cmp.b -(a7), d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );
      sys.AddOpcode( "cmp.w", 0xB020, 0, AddressingType.INDIRECT, 0 )       // cmp.w -(a7), d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );
      sys.AddOpcode( "cmp", 0xB020, 0, AddressingType.INDIRECT, 0 )       // cmp -(a7), d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );


      sys.AddOpcode( "cmp.b", 0xB0280000, 2, AddressingType.IMMEDIATE_8BIT, 0 )   // cmp.b $01(a0),d0
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


      sys.AddOpcode( "cmp.w", 0xB0680000, 2, AddressingType.IMMEDIATE_8BIT, 0 )   // cmp.w $01(a0),d0
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
      sys.AddOpcode( "cmp", 0xB0680000, 2, AddressingType.IMMEDIATE_8BIT, 0 )   // cmp $01(a0),d0
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


      sys.AddOpcode( "dbt", 0x50C80000, 4, AddressingType.RELATIVE_16, 0 )    // dbt d0, $1234
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 16 ),
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_16BIT_RELATIVE ) } );

      sys.AddOpcode( "dbf", 0x51C80000, 4, AddressingType.RELATIVE_16, 0 )    // dbf d0, $1234
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 16 ),
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_16BIT_RELATIVE ) } );

      sys.AddOpcode( "dbhi", 0x52C80000, 4, AddressingType.RELATIVE_16, 0 )    // dbhi d0, $1234
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 16 ),
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_16BIT_RELATIVE ) } );

      sys.AddOpcode( "dbls", 0x53C80000, 4, AddressingType.RELATIVE_16, 0 )    // dbls d0, $1234
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 16 ),
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_16BIT_RELATIVE ) } );

      sys.AddOpcode( "dbcc", 0x54C80000, 4, AddressingType.RELATIVE_16, 0 )    // dbcc d0, $1234
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 16 ),
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_16BIT_RELATIVE ) } );

      sys.AddOpcode( "dbcs", 0x55C80000, 4, AddressingType.RELATIVE_16, 0 )    // dbcs d0, $1234
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 16 ),
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_16BIT_RELATIVE ) } );

      sys.AddOpcode( "dbne", 0x56C80000, 4, AddressingType.RELATIVE_16, 0 )   // dbne d0, $1234
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 16 ),
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_16BIT_RELATIVE ) } );

      sys.AddOpcode( "dbeq", 0x57C80000, 4, AddressingType.RELATIVE_16, 0 )   // dbeq d0, $1234
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 16 ),
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_16BIT_RELATIVE ) } );

      sys.AddOpcode( "dbvc", 0x58C80000, 4, AddressingType.RELATIVE_16, 0 )   // dbvc d0, $1234
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 16 ),
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_16BIT_RELATIVE ) } );

      sys.AddOpcode( "dbvs", 0x59C80000, 4, AddressingType.RELATIVE_16, 0 )   // dbvs d0, $1234
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 16 ),
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_16BIT_RELATIVE ) } );

      sys.AddOpcode( "dbpl", 0x5AC80000, 4, AddressingType.RELATIVE_16, 0 )   // dbpl d0, $1234
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 16 ),
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_16BIT_RELATIVE ) } );

      sys.AddOpcode( "dbmi", 0x5BC80000, 4, AddressingType.RELATIVE_16, 0 )   // dbmi d0, $1234
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 16 ),
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_16BIT_RELATIVE ) } );

      sys.AddOpcode( "dbge", 0x5CC80000, 4, AddressingType.RELATIVE_16, 0 )   // dbg d0, $1234
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 16 ),
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_16BIT_RELATIVE ) } );

      sys.AddOpcode( "dblt", 0x5DC80000, 4, AddressingType.RELATIVE_16, 0 )   // dblt d0, $1234
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 16 ),
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_16BIT_RELATIVE ) } );

      sys.AddOpcode( "dbgt", 0x5EC80000, 4, AddressingType.RELATIVE_16, 0 )   // dbgt d0, $1234
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 16 ),
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_16BIT_RELATIVE ) } );

      sys.AddOpcode( "dble", 0x5FC80000, 4, AddressingType.RELATIVE_16, 0 )   // dble d0, $1234
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 16 ),
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_16BIT_RELATIVE ) } );


      sys.AddOpcode( "divs.w", 0x81FC0000, 1, AddressingType.IMMEDIATE_8BIT, 0 )    // divs.w #$12, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );
      sys.AddOpcode( "divs", 0x81FC0000, 1, AddressingType.IMMEDIATE_8BIT, 0 )    // divs #$12, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );


      sys.AddOpcode( "divu.w", 0x80D0, 2, AddressingType.IMPLICIT, 0 )         // divu.w (a0), d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );
      sys.AddOpcode( "divu", 0x80D0, 2, AddressingType.IMPLICIT, 0 )         // divu (a0), d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );


      sys.AddOpcode( "divu.w", 0x80F80000, 4, AddressingType.INDIRECT, 0 )    // divu.w ($0001).w, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );
      sys.AddOpcode( "divu.w", 0x80F80000, 4, AddressingType.INDIRECT, 0 )    // divu.w ($0001), d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );
      sys.AddOpcode( "divu", 0x80F80000, 4, AddressingType.INDIRECT, 0 )    // divu.w ($0001).w, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );
      sys.AddOpcode( "divu", 0x80F80000, 4, AddressingType.INDIRECT, 0 )    // divu.w ($0001), d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );



      sys.AddOpcode( "divu.w", 0x80C0, 2, AddressingType.IMPLICIT, 0 )         // divu.w d0, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );
      sys.AddOpcode( "divu", 0x80C0, 2, AddressingType.IMPLICIT, 0 )         // divu d0, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );


      sys.AddOpcode( "divu.w", 0x80D8, 2, AddressingType.IMPLICIT, 0 )         // divu.w (a0)+, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );
      sys.AddOpcode( "divu", 0x80D8, 2, AddressingType.IMPLICIT, 0 )         // divu (a0)+, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );


      sys.AddOpcode( "divu.w", 0x80E0, 2, AddressingType.IMPLICIT, 0 )         // divu.w -(a0), d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );
      sys.AddOpcode( "divu", 0x80E0, 2, AddressingType.IMPLICIT, 0 )         // divu -(a0), d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "divu.w", 0x80E80000, 4, AddressingType.IMMEDIATE_8BIT, 0 )     // divu.w $01(a0), d7
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
      sys.AddOpcode( "divu", 0x80E80000, 4, AddressingType.IMMEDIATE_8BIT, 0 )     // divu $01(a0), d7
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


      sys.AddOpcode( "eori.w", 0x0A400000, 2, AddressingType.IMMEDIATE_16BIT, 0 )    // eori.w #$0001, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 16 ) } );
      // TODO - Fallback 
      /*
      sys.AddOpcode( "eori", 0x0A400000, 2, AddressingType.IMMEDIATE_16BIT, 0 )    // eori #$0001, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 16 ) } );
      */

      sys.AddOpcode( "eori.l", 0x0A9000000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )          // eori.l #$1234, (a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 32 ) } );
      sys.AddOpcode( "eori", 0x0A9000000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )          // eori #$1234, (a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 32 ) } );


      sys.AddOpcode( "eori.b", 0x0A3800000000, 4, AddressingType.IMMEDIATE_8BIT, 0 )             // eori.b #$12, ($1234).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );
      sys.AddOpcode( "eori.b", 0x0A3800000000, 4, AddressingType.IMMEDIATE_8BIT, 0 )             // eori.b #$12, ($1234)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ) } );

      sys.AddOpcode( "eori.w", 0x0A7800000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )            // eori.w #$1234, ($1234).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );
      sys.AddOpcode( "eori.w", 0x0A7800000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )            // eori.w #$1234, ($1234)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ) } );
      sys.AddOpcode( "eori", 0x0A7800000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )            // eori #$1234, ($1234).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );
      sys.AddOpcode( "eori", 0x0A7800000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )            // eori #$1234, ($1234)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ) } );


      sys.AddOpcode( "eori.l", 0x0A8000000000, 4, AddressingType.IMMEDIATE_32BIT, 0 )          // eori.l #$12345678, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 32 ) } );
      sys.AddOpcode( "eori", 0x0A8000000000, 4, AddressingType.IMMEDIATE_32BIT, 0 )          // eori #$12345678, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 32 ) } );

      sys.AddOpcode( "eori.l", 0x0A9800000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )          // eori.l #$1234, (a0)+
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 32 ) } );
      sys.AddOpcode( "eori", 0x0A9800000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )          // eori #$1234, (a0)+
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 32 ) } );

      sys.AddOpcode( "eori.l", 0x0AA000000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )          // eori.l #$1234, -(a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 32 ) } );
      sys.AddOpcode( "eori", 0x0AA000000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )          // eori #$1234, -(a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 32 ) } );

      sys.AddOpcode( "eori.l", 0x0AA8000000000000, 2, AddressingType.IMMEDIATE_16BIT, 0 )     // eori.l #$1234, $01(a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 16 ),
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
      sys.AddOpcode( "eori", 0x0AA8000000000000, 2, AddressingType.IMMEDIATE_16BIT, 0 )     // eori #$1234, $01(a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 16 ),
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

      sys.AddOpcode( "eori", 0x0A7C0000, 2, AddressingType.IMMEDIATE_16BIT, 0 )          // eori #$1234, sr
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, sr ) } );

      sys.AddOpcode( "eor.b", 0xB100, 0, AddressingType.IMPLICIT, 0 )            // eor.b d0,d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ) } );

      sys.AddOpcode( "eor.w", 0xB140, 0, AddressingType.IMPLICIT, 0 )            // eor.w d0,d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ) } );
      sys.AddOpcode( "eor", 0xB140, 0, AddressingType.IMPLICIT, 0 )            // eor d0,d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ) } );


      sys.AddOpcode( "exg", 0xC140, 0, AddressingType.IMPLICIT, 0 )               // exg d0,d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ) } );

      sys.AddOpcode( "exg", 0xC148, 0, AddressingType.IMPLICIT, 0 )            // exg a0,a7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 9 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 0 ) } );

      sys.AddOpcode( "exg", 0xC188, 0, AddressingType.IMPLICIT, 0 )            // exg d0,a7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 0 ) } );


      sys.AddOpcode( "ext.w", 0x4880, 0, AddressingType.IMPLICIT, 0 )          // ext.w d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ) } );
      sys.AddOpcode( "ext", 0x4880, 0, AddressingType.IMPLICIT, 0 )          // ext d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ) } );

      sys.AddOpcode( "ext.l", 0x48C0, 0, AddressingType.IMPLICIT, 0 )          // ext.l d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ) } );

      sys.AddOpcode( "illegal", 0x4AFC, 0, AddressingType.IMPLICIT, 0 );        // illegal

      var oc = sys.AddOpcode( "jmp", 0x4ED0, 0, AddressingType.IMPLICIT, 0 );             // jmp (a0)
      oc.Flags = OpcodeFlag.IS_END_OF_CODE;
      oc.ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ) } );

      oc = sys.AddOpcode( "jmp", 0x4EE80000, 1, AddressingType.INDIRECT, 0 );         // jmp $12(a0)
      oc.Flags = OpcodeFlag.IS_END_OF_CODE;
      oc.ParserExpressions.AddRange( new List<OpcodeExpression>() {
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

      oc = sys.AddOpcode( "jmp", 0x4EF900000000, 3, AddressingType.ABSOLUTE_LONG, 0 );  // jmp $123456
      oc.Flags = OpcodeFlag.IS_END_OF_CODE;
      oc.ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_24BIT ) } );

      sys.AddOpcode( "jsr", 0x4EB900000000, 3, AddressingType.ABSOLUTE_LONG, 0 )  // jsr $123456
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_24BIT ) }  );

      sys.AddOpcode( "lea", 0x41F80000, 2, AddressingType.INDIRECT, 0 )           // lea ($1234).w, a0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 25 ) } );

      sys.AddOpcode( "lea", 0x41F900000000u, 4, AddressingType.INDIRECT, 0 )        // lea ($12345678).l, a0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT, openingParenthesis, closingParenthesisDotL, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 41 ) } );

      sys.AddOpcode( "lea", 0x41C0, 0, AddressingType.IMPLICIT, 0 )               // lea d0,a2
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 9 ) } );

      sys.AddOpcode( "lea", 0x41D0, 0, AddressingType.INDIRECT, 0 )               // lea (a0),a1
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 9 ) } );

      sys.AddOpcode( "lea", 0x41E80000, 2, AddressingType.INDIRECT, 0 )           // lea $1234(a0), a1
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_15BIT ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 16 ),
              new ValidValueGroup( closingParenthesis )
            }
          },
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 25 ) } );

      sys.AddOpcode( "lea", 0x41F00000, 0, AddressingType.INDIRECT, 0 )          // lea (a3,d1.w), a3
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
      sys.AddOpcode( "lea", 0x41F00000, 0, AddressingType.INDIRECT, 0 )          // lea (a3,d1), a3
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 16 ),
              new ValidValueGroup( comma ),
              new ValidValueGroup( dRegisters, OpcodePartialExpression.TOKEN_LIST, 12 ),
              new ValidValueGroup( closingParenthesis )
            }
          },
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 25 ) } );

      sys.AddOpcode( "lea", 0x41F00000, 4, AddressingType.INDIRECT, 0 )          // lea $12(a3,d1.w), a3
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_7BIT, 0 ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 16 ),
              new ValidValueGroup( comma ),
              new ValidValueGroup( dRegistersDotW, OpcodePartialExpression.TOKEN_LIST, 12 ),
              new ValidValueGroup( closingParenthesis )
            }
          },
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 25 ) } );
      sys.AddOpcode( "lea", 0x41F00000, 4, AddressingType.INDIRECT, 0 )          // lea $12(a3,d1), a3
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_7BIT, 0 ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 16 ),
              new ValidValueGroup( comma ),
              new ValidValueGroup( dRegisters, OpcodePartialExpression.TOKEN_LIST, 12 ),
              new ValidValueGroup( closingParenthesis )
            }
          },
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 25 ) } );

      sys.AddOpcode( "lea", 0x41FB0000, 4, AddressingType.INDIRECT, 0 )        // lea $01(pc,d7.w), a7      ;4F FB 70 01
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_7BIT, 0 ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( pc, OpcodePartialExpression.TOKEN_LIST, 16 ),
              new ValidValueGroup( comma ),
              new ValidValueGroup( dRegistersDotW, OpcodePartialExpression.TOKEN_LIST, 12 ),
              new ValidValueGroup( closingParenthesis )
            }
          },
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 25 ) } );
      sys.AddOpcode( "lea", 0x41FB0000, 4, AddressingType.INDIRECT, 0 )        // lea $01(pc,d7), a7      ;4F FB 70 01
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_7BIT, 0 ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( pc, OpcodePartialExpression.TOKEN_LIST, 16 ),
              new ValidValueGroup( comma ),
              new ValidValueGroup( dRegisters, OpcodePartialExpression.TOKEN_LIST, 12 ),
              new ValidValueGroup( closingParenthesis )
            }
          },
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 25 ) } );

      sys.AddOpcode( "lea", 0x41F900000000, 6, AddressingType.ABSOLUTE_LONG, 0 )          // lea $123456, ax
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_24BIT ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 41 ) } );


      sys.AddOpcode( "link", 0x4E500000, 2, AddressingType.IMMEDIATE_16BIT, 0 )          // link a0, #$0001           ;4E 50 00 01
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 0 ) } );

      sys.AddOpcode( "lsl.b", 0xE108, 0, AddressingType.IMMEDIATE_8BIT, 0 )        // lsl.b #1, d7          ;E3 0F
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, immediatePrefix, quickAdd, empty, 9 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters ) } );

      sys.AddOpcode( "lsr.w", 0xE048, 0, AddressingType.IMMEDIATE_8BIT, 0 )        // lsr.w #1, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, immediatePrefix, quickAdd, empty, 9 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters ) } );
      sys.AddOpcode( "lsr", 0xE048, 0, AddressingType.IMMEDIATE_8BIT, 0 )        // lsr #1, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, immediatePrefix, quickAdd, empty, 9 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters ) } );

      sys.AddOpcode( "lsr.w", 0xE068, 0, AddressingType.IMPLICIT, 0 )    // lsr.w d0, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ) } );
      sys.AddOpcode( "lsr", 0xE068, 0, AddressingType.IMPLICIT, 0 )    // lsr d0, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ) } );

      sys.AddOpcode( "lsl.l", 0xE188, 0, AddressingType.IMMEDIATE_8BIT, 0 )        // lsl.l #1, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, immediatePrefix, quickAdd, empty, 9 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters ) } );
      sys.AddOpcode( "lsl", 0xE188, 0, AddressingType.IMMEDIATE_8BIT, 0 )        // lsl #1, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, immediatePrefix, quickAdd, empty, 9 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters ) } );

      sys.AddOpcode( "asr.l", 0xE080, 0, AddressingType.IMMEDIATE_8BIT, 0 )        // asr.l #1, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, immediatePrefix, quickAdd, empty, 9 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters ) } );
      sys.AddOpcode( "asr", 0xE080, 0, AddressingType.IMMEDIATE_8BIT, 0 )        // asr #1, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, immediatePrefix, quickAdd, empty, 9 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters ) } );

      sys.AddOpcode( "asr", 0xE0E80000, 2, AddressingType.INDIRECT, 0 )          // asr $12(a3)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_8BIT, 0 ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 16 ),
              new ValidValueGroup( closingParenthesis )
            }
          } } );


      sys.AddOpcode( "move.l", 0x2108, 0, AddressingType.IMPLICIT, 0 )    // move.l a2, -(sp)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, sp, closingParenthesis, 9 ) } );

      sys.AddOpcode( "move.w", 0x30BC0000, 2, AddressingType.IMMEDIATE_16BIT, 0 )    // move.w #$0001, (a7)         ;3E BC 00 01
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 25 ) } );
      sys.AddOpcode( "move", 0x30BC0000, 2, AddressingType.IMMEDIATE_16BIT, 0 )    // move #$0001, (a7)         ;3E BC 00 01
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 25 ) } );

      sys.AddOpcode( "move.b", 0x11FC00000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )             // move.b #$12, ($1234).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );
      sys.AddOpcode( "move.b", 0x11FC00000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )             // move.b #$12, ($1234)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ) } );

      sys.AddOpcode( "move.w", 0x31FC00000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )             // move.w #$1234, ($1234).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );
      sys.AddOpcode( "move.w", 0x31FC00000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )             // move.w #$1234, ($1234)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ) } );
      sys.AddOpcode( "move", 0x31FC00000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )             // move #$1234, ($1234).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );
      sys.AddOpcode( "move", 0x31FC00000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )             // move #$1234, ($1234)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ) } );

      sys.AddOpcode( "move.l", 0x21FC000000000000, 4, AddressingType.IMMEDIATE_32BIT, 0 )    // move.l #$00000001, ($FF00).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );
      sys.AddOpcode( "move.l", 0x21FC000000000000, 4, AddressingType.IMMEDIATE_32BIT, 0 )    // move.l #$00000001, ($FF00)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 0 ) } );

      sys.AddOpcode( "move.b", 0x103C0000, 1, AddressingType.IMMEDIATE_8BIT, 0 )      // move.b #$01, d7             ;1E 3C 00 01
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );

      sys.AddOpcode( "move.l", 0x203C00000000, 4, AddressingType.IMMEDIATE_32BIT, 0 )    // move.l #$00000001, d7       ;2E 3C 00 00 00 01
      .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 41 ) } );
      sys.AddOpcode( "move.w", 0x303C0000, 4, AddressingType.IMMEDIATE_16BIT, 0 )    // move.w #$0001, d7       ;3E 3C 00 00 00 01
      .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );
      sys.AddOpcode( "move", 0x303C0000, 4, AddressingType.IMMEDIATE_16BIT, 0 )    // move #$0001, d7       ;3E 3C 00 00 00 01
      .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );



      sys.AddOpcode( "move.w", 0x30FC0000, 2, AddressingType.IMMEDIATE_16BIT, 0 )          // move.w #$1234, (a0)+
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 25 ) } );
      sys.AddOpcode( "move", 0x30FC0000, 2, AddressingType.IMMEDIATE_16BIT, 0 )          // move #$1234, (a0)+
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 25 ) } );

      sys.AddOpcode( "move.w", 0x313C0000, 2, AddressingType.IMMEDIATE_16BIT, 0 )          // move.w #$1234, -(a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 25 ) } );
      sys.AddOpcode( "move", 0x313C0000, 2, AddressingType.IMMEDIATE_16BIT, 0 )          // move #$1234, -(a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 25 ) } );


      sys.AddOpcode( "move.b", 0x117C00000000, 2, AddressingType.INDIRECT, 0 )       // move.b #$12, $34(a1)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_8BIT ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 41 ),
              new ValidValueGroup( closingParenthesis )
            }
          } } );

      sys.AddOpcode( "move.l", 0x21400000, 4, AddressingType.INDIRECT, 0 )       // move.l d0, $7FFF(a1)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 16 ),
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_15BIT ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 25 ),
              new ValidValueGroup( closingParenthesis )
            }
          } } );

      sys.AddOpcode( "move.w", 0x317C00000000, 6, AddressingType.INDIRECT, 0 )       // move.w #$1234, $34(a1)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_8BIT ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 41 ),
              new ValidValueGroup( closingParenthesis )
            }
          } } );
      sys.AddOpcode( "move", 0x317C00000000, 6, AddressingType.INDIRECT, 0 )       // move #$1234, $34(a1)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_8BIT ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 41 ),
              new ValidValueGroup( closingParenthesis )
            }
          } } );

      sys.AddOpcode( "move.w", 0x31F800000000, 2, AddressingType.IMMEDIATE_16BIT, 0 )   // move.w ($1234).w, ($1234).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );
      sys.AddOpcode( "move.w", 0x31F800000000, 2, AddressingType.IMMEDIATE_16BIT, 0 )   // move.w ($1234), ($1234).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );
      sys.AddOpcode( "move", 0x31F800000000, 2, AddressingType.IMMEDIATE_16BIT, 0 )   // move ($1234), ($1234).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );

      sys.AddOpcode( "move.w", 0x33F8000000000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )   // move.w ($1234).w, ($12345678).l
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 32 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT, openingParenthesis, closingParenthesisDotL, 0 ) } );
      sys.AddOpcode( "move.w", 0x33F8000000000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )   // move.w ($1234).w, ($12345678)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 32 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT, openingParenthesis, closingParenthesis, 0 ) } );
      sys.AddOpcode( "move", 0x33F8000000000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )   // move ($1234).w, ($12345678)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 32 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT, openingParenthesis, closingParenthesis, 0 ) } );
      sys.AddOpcode( "move", 0x33F8000000000000, 4, AddressingType.IMMEDIATE_16BIT, 0 )   // move ($1234), ($12345678)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesis, 32 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT, openingParenthesis, closingParenthesis, 0 ) } );

      sys.AddOpcode( "move.w", 0x31F900000000, 4, AddressingType.IMMEDIATE_32BIT, 0 )   // move.w ($12345678).l, ($1234).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT, openingParenthesis, closingParenthesisDotL, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );

      sys.AddOpcode( "move.w", 0x30380000, 2, AddressingType.INDIRECT, 0 )   // move.w ($0001).w, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );

      sys.AddOpcode( "move.b", 0x10B80000, 2, AddressingType.INDIRECT, 0 )   // move.b ($0001).w, (a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 25 ) } );

      sys.AddOpcode( "move.b", 0x10F80000, 2, AddressingType.INDIRECT, 0 )   // move.b ($0001).w, (a0)+
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 25 ) } );

      sys.AddOpcode( "move.b", 0x11380000, 2, AddressingType.INDIRECT, 0 )   // move.b ($0001).w, -(a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 25 ) } );

      sys.AddOpcode( "move.b", 0x117800000000, 2, AddressingType.INDIRECT, 0 )   // move.b ($0001).w, $FF(a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 16 ),
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_8BIT ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 41 ),
              new ValidValueGroup( closingParenthesis )
            }
          } } );

      sys.AddOpcode( "move.b", 0x11C00000, 2, AddressingType.INDIRECT, 0 )           // move.b d0, ($0001).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );

      sys.AddOpcode( "move.l", 0x23C000000000, 4, AddressingType.INDIRECT, 0 )       // move.l d0, ($00000001).l
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT, openingParenthesis, closingParenthesisDotL, 0 ) } );

      sys.AddOpcode( "move.w", 0x3000, 0, AddressingType.IMPLICIT, 0 )    // move.w d0, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "move.l", 0x2000, 0, AddressingType.IMPLICIT, 0 )    // move.l d0, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "move.w", 0x3080, 0, AddressingType.IMPLICIT, 0 )    // move.w d0, (a7)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 9 ) } );

      sys.AddOpcode( "move.w", 0x30C0, 0, AddressingType.IMPLICIT, 0 )    // move.w d0, (a7)+
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 9 ) } );

      sys.AddOpcode( "move.w", 0x3100, 0, AddressingType.IMPLICIT, 0 )    // move.w d0, -(a7)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 9 ) } );

      sys.AddOpcode( "move.w", 0x31400000, 4, AddressingType.INDIRECT, 0 )     // move.w d0, $7FFF(a7)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 16 ),
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_15BIT ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 25 ),
              new ValidValueGroup( closingParenthesis )
            }
          } } );
      sys.AddOpcode( "move", 0x31400000, 4, AddressingType.INDIRECT, 0 )     // move d0, $7FFF(a7)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 16 ),
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_15BIT ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 25 ),
              new ValidValueGroup( closingParenthesis )
            }
          } } );

      sys.AddOpcode( "move.b", 0x11400000, 2, AddressingType.INDIRECT, 0 )    // move.b d0, $01(a7)           ;1F 40 00 01
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 16 ),
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_8BIT ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 25 ),
              new ValidValueGroup( closingParenthesis )
            }
          } } );

      sys.AddOpcode( "move.b", 0x1010, 0, AddressingType.INDIRECT, 0 )     // move.b (a0), d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "move.b", 0x1090, 0, AddressingType.INDIRECT, 0 )     // move.b (a0), (a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 9 ) } );

      sys.AddOpcode( "move.b", 0x10D0, 0, AddressingType.INDIRECT, 0 )     // move.b (a0), (a0)+
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 9 ) } );

      sys.AddOpcode( "move.b", 0x1110, 0, AddressingType.INDIRECT, 0 )     // move.b (a0), -(a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 9 ) } );

      sys.AddOpcode( "move.b", 0x11500000, 2, AddressingType.INDIRECT, 0 )    // move.b (a0), $10(a1)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 16 ),
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_16BIT ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 25 ),
              new ValidValueGroup( closingParenthesis )
            }
          } } );

      sys.AddOpcode( "move.l", 0x21500000, 2, AddressingType.INDIRECT, 0 )    // move.l (a0), $10(a1)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 16 ),
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_16BIT ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 25 ),
              new ValidValueGroup( closingParenthesis )
            }
          } } );

      sys.AddOpcode( "move.w", 0x31E800000000, 2, AddressingType.INDIRECT, 0 )    // move.w $01(a7), ($FF00).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_8BIT, 16 ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 32 ),
              new ValidValueGroup( closingParenthesis )
            }
          },
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );

      sys.AddOpcode( "move.b", 0x1020, 0, AddressingType.IMPLICIT, 0 )    // move.b -(a7), d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "move.w", 0x3120, 0, AddressingType.IMPLICIT, 0 )    // move.l -(a7), -(a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 9 ) } );

      sys.AddOpcode( "move.b", 0x10280000, 2, AddressingType.INDIRECT, 0 )    // move.b $01(a7), d0
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

      sys.AddOpcode( "move.l", 0x20280000, 2, AddressingType.INDIRECT, 0 )    // move.l $01(a7), d0
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

      sys.AddOpcode( "move.b", 0x10A80000, 2, AddressingType.INDIRECT, 0 )    // move.b $01(a0), (a7)
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
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 25 ) } );

      sys.AddOpcode( "move.b", 0x10E80000, 2, AddressingType.INDIRECT, 0 )    // move.b $01(a0), (a7)+
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
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 25 ) } );

      sys.AddOpcode( "move.b", 0x11280000, 2, AddressingType.INDIRECT, 0 )    // move.b $01(a0), -(a7)
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
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 25 ) } );

      sys.AddOpcode( "move.b", 0x116800000000, 2, AddressingType.INDIRECT, 0 )    // move.b $01(a0), $34(a7)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_8BIT, 16 ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 32 ),
              new ValidValueGroup( closingParenthesis )
            }
          },
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_8BIT ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 41 ),
              new ValidValueGroup( closingParenthesis )
            }
          } } );

      sys.AddOpcode( "move.w", 0x316800000000, 2, AddressingType.INDIRECT, 0 )    // move.w $01(a0), $34(a7)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_8BIT, 16 ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 32 ),
              new ValidValueGroup( closingParenthesis )
            }
          },
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_8BIT ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 41 ),
              new ValidValueGroup( closingParenthesis )
            }
          } } );

      sys.AddOpcode( "move.l", 0x216800000000, 2, AddressingType.INDIRECT, 0 )    // move.l $01(a0), $34(a7)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_8BIT, 16 ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 32 ),
              new ValidValueGroup( closingParenthesis )
            }
          },
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_8BIT ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 41 ),
              new ValidValueGroup( closingParenthesis )
            }
          } } );

      sys.AddOpcode( "move.b", 0x11D80000, 2, AddressingType.INDIRECT, 0 )    // move.b (a0)+, ($0001).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );

      sys.AddOpcode( "move.b", 0x1018, 0, AddressingType.IMPLICIT, 0 )    // move.b (a7)+, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "move.l", 0x2018, 0, AddressingType.IMPLICIT, 0 )    // move.l (a7)+, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "move.l", 0x2098, 0, AddressingType.IMPLICIT, 0 )    // move.l (a7)+, (a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 9 ) } );

      sys.AddOpcode( "move.l", 0x21580000, 2, AddressingType.INDIRECT, 0 )    // move.l (a0)+, $01(a7)         ;2F 58 00 01
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 16 ),
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_8BIT ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 25 ),
              new ValidValueGroup( closingParenthesis )
            }
          } } );

      sys.AddOpcode( "move.l", 0x20D8, 0, AddressingType.IMPLICIT, 0 )    // move.l (a7)+, (a0)+
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 9 ) } );

      sys.AddOpcode( "move.w", 0x3020, 0, AddressingType.IMPLICIT, 0 )    // move.w -(a7), d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "move.l", 0x21C80000, 2, AddressingType.INDIRECT, 0 )    // move.l a0, ($0001).w      ;21 C8 00 01
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );

      sys.AddOpcode( "move.l", 0x2008, 0, AddressingType.IMPLICIT, 0 )    // move.l a7, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "move.l", 0x2088, 0, AddressingType.IMPLICIT, 0 )    // move.l a7, (a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 9 ) } );

      sys.AddOpcode( "move.w", 0x31480000, 2, AddressingType.INDIRECT, 0 )    // move.w a0, $01(a7)        ;3F 48 00 01
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 16 ),
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_8BIT ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 25 ),
              new ValidValueGroup( closingParenthesis )
            }
          } } );

      sys.AddOpcode( "move.l", 0x2088, 0, AddressingType.IMPLICIT, 0 )    // move.l a2, (sp)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, sp, closingParenthesis, 9 ) } );

      sys.AddOpcode( "move.l", 0x20C8, 0, AddressingType.IMPLICIT, 0 )    // move.l a2, (sp)+
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, sp, closingParenthesisPlus, 9 ) } );

      sys.AddOpcode( "move.b", 0x11F000000000, 4, AddressingType.INDIRECT, 0 )  // move.b $01(a0,d7.w), ($FF00).w    ;11 F0 70 01 FF 00
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_7BIT, 16 ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 32 ),
              new ValidValueGroup( comma ),
              new ValidValueGroup( dRegistersDotW, OpcodePartialExpression.TOKEN_LIST, 28 ),
              new ValidValueGroup( closingParenthesis )
            }
          },
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );

      sys.AddOpcode( "move.b", 0x10300000, 0, AddressingType.INDIRECT, 0 )  // move.b (a0,d7.w), d0   ;10 30 70 00
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
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );

      sys.AddOpcode( "move.w", 0x30300000, 1, AddressingType.INDIRECT, 0 )  // move.w $01(a0,d7.w), d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_7BIT, 0 ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 16 ),
              new ValidValueGroup( comma ),
              new ValidValueGroup( dRegistersDotW, OpcodePartialExpression.TOKEN_LIST, 12 ),
              new ValidValueGroup( closingParenthesis )
            }
          },
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );

      sys.AddOpcode( "move.b", 0x10300000, 1, AddressingType.INDIRECT, 0 )  // move.b $01(a0,d7.w), d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_7BIT, 0 ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 16 ),
              new ValidValueGroup( comma ),
              new ValidValueGroup( dRegistersDotW, OpcodePartialExpression.TOKEN_LIST, 12 ),
              new ValidValueGroup( closingParenthesis )
            }
          },
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );

      sys.AddOpcode( "move.b", 0x10B00000, 0, AddressingType.INDIRECT, 0 )  // move.b (a0,d7.w), (a7)
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
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 25 ) } );

      sys.AddOpcode( "move.b", 0x117000000000, 1, AddressingType.INDIRECT, 0 )  // move.b $01(a0,d7.w), $FF(a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_7BIT, 16 ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 32 ),
              new ValidValueGroup( comma ),
              new ValidValueGroup( dRegistersDotW, OpcodePartialExpression.TOKEN_LIST, 28 ),
              new ValidValueGroup( closingParenthesis )
            }
          },
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_8BIT, 0 ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 41 ),
              new ValidValueGroup( closingParenthesis )
            }
          } } );

      sys.AddOpcode( "move.b", 0x10F00000, 0, AddressingType.INDIRECT, 0 )  // move.b (a0,d7.w), (a0)+
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
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 25 ) } );

      sys.AddOpcode( "move.b", 0x11300000, 0, AddressingType.INDIRECT, 0 )  // move.b (a0,d7.w), -(a0)
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
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 25 ) } );

      sys.AddOpcode( "move.b", 0x11FB00000000, 2, AddressingType.INDIRECT, 0 )  // move.b $01(pc,d7.w), ($1234).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_7BIT, 16 ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( pc, OpcodePartialExpression.TOKEN_LIST, 32 ),
              new ValidValueGroup( comma ),
              new ValidValueGroup( dRegistersDotW, OpcodePartialExpression.TOKEN_LIST, 28 ),
              new ValidValueGroup( closingParenthesis )
            }
          },
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );

      sys.AddOpcode( "move.b", 0x103B0000, 1, AddressingType.INDIRECT, 0 )  // move.b $01(pc,d7.w), d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_7BIT, 0 ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( pc ),
              new ValidValueGroup( comma ),
              new ValidValueGroup( dRegistersDotW, OpcodePartialExpression.TOKEN_LIST, 12 ),
              new ValidValueGroup( closingParenthesis )
            }
          },
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );

      sys.AddOpcode( "move.b", 0x10BB0000, 1, AddressingType.INDIRECT, 0 )  // move.b $01(pc,d7.w), (a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_7BIT, 0 ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( pc ),
              new ValidValueGroup( comma ),
              new ValidValueGroup( dRegistersDotW, OpcodePartialExpression.TOKEN_LIST, 12 ),
              new ValidValueGroup( closingParenthesis )
            }
          },
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 25 ) } );

      sys.AddOpcode( "move.b", 0x117B00000000, 1, AddressingType.INDIRECT, 0 )  // move.b $01(pc,d7.w), $12(a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_7BIT, 16 ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( pc ),
              new ValidValueGroup( comma ),
              new ValidValueGroup( dRegistersDotW, OpcodePartialExpression.TOKEN_LIST, 28 ),
              new ValidValueGroup( closingParenthesis )
            }
          },
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_7BIT, 0 ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 41 ),
              new ValidValueGroup( closingParenthesis )
            }
          } } );

      sys.AddOpcode( "move", 0x46FC0000, 2, AddressingType.IMPLICIT, 0 )    // move #$0001,sr                ;46 FC FC 00 01
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, sr ) } );

      sys.AddOpcode( "move.l", 0x23C000000000, 6, AddressingType.INDIRECT, 0 )       // move.l d0, $123456
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 32 ),
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_24BIT ) } );

      sys.AddOpcode( "move.w", 0x33C000000000, 6, AddressingType.INDIRECT, 0 )       // move.w d0, $123456
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 32 ),
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_24BIT ) } );
      sys.AddOpcode( "move", 0x33C000000000, 6, AddressingType.INDIRECT, 0 )       // move d0, $123456
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 32 ),
          new OpcodeExpression( OpcodePartialExpression.EXPRESSION_24BIT ) } );


      sys.AddOpcode( "movea.w", 0x307C0000, 2, AddressingType.IMMEDIATE_16BIT, 0 )    // movea.w #$1234, a4
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 25 ) } );

      sys.AddOpcode( "movea.w", 0x30780000, 2, AddressingType.INDIRECT, 0 )    // movea.w ($1234).w, a4
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 25 ) } );

      sys.AddOpcode( "movea.w", 0x3040, 0, AddressingType.IMPLICIT, 0 )    // movea.w d7, a0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 9 ) } );

      sys.AddOpcode( "movea.l", 0x2048, 0, AddressingType.IMPLICIT, 0 )    // movea.w a7, a0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 9 ) } );

      sys.AddOpcode( "movea.w", 0x3050, 0, AddressingType.IMPLICIT, 0 )    // movea.w (a7), a0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 9 ) } );

      sys.AddOpcode( "movea.w", 0x30680000, 2, AddressingType.IMPLICIT, 0 )    // movea.w $01(a7), a0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_8BIT, 0 ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 16 ),
              new ValidValueGroup( closingParenthesis )
            }
          },
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 25 ) } );

      sys.AddOpcode( "movea.l", 0x2058, 0, AddressingType.IMPLICIT, 0 )    // movea.l (a7)+, a0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 9 ) } );

      sys.AddOpcode( "movea.l", 0x2060, 0, AddressingType.IMPLICIT, 0 )    // movea.l -(a7), a0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 9 ) } );

      sys.AddOpcode( "movea.l", 0x2050, 0, AddressingType.IMPLICIT, 0 )    // movea.l (sp), a0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, sp, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 9 ) } );

      sys.AddOpcode( "movea.l", 0x20700000, 0, AddressingType.IMPLICIT, 0 )    // movea.l (a0,d7.w), a7        
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

      sys.AddOpcode( "movea.l", 0x20700000, 1, AddressingType.IMPLICIT, 0 )    // movea.l $12(a0,d7.w), a7        
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_7BIT, 0 ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 16 ),
              new ValidValueGroup( comma ),
              new ValidValueGroup( dRegistersDotW, OpcodePartialExpression.TOKEN_LIST, 12 ),
              new ValidValueGroup( closingParenthesis )
            }
          },
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 25 ) } );

      sys.AddOpcode( "movep.w", 0x01080000, 2, AddressingType.INDIRECT, 0 )   // movep.w $0001(a0),d7
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

      sys.AddOpcode( "movep.l", 0x01480000, 2, AddressingType.INDIRECT, 0 )   // movep.l $0001(a0),d7
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

      sys.AddOpcode( "movep.w", 0x01880000, 2, AddressingType.INDIRECT, 0 )   // movep.w d7,$0001(a0)
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

      sys.AddOpcode( "movep.l", 0x01C80000, 2, AddressingType.INDIRECT, 0 )   // movep.l d7,$0001(a0)
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

      sys.AddOpcode( "moveq", 0x7000, 1, AddressingType.IMMEDIATE_8BIT, 0 )       // moveq #$01, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "movem.l", 0x48E00000, 4, AddressingType.INDIRECT, 0 )       //       movem.l d0,-( a7 )
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMBINATION, adRegisters, rangeToken, separatorToken, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 16 ) } );
      sys.AddOpcode( "movem.w", 0x48A00000, 4, AddressingType.INDIRECT, 0 )       //       movem.w d0,-( a7 )
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMBINATION, adRegisters, rangeToken, separatorToken, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 16 ) } );
      sys.AddOpcode( "movem", 0x48A00000, 4, AddressingType.INDIRECT, 0 )       //       movem d0,-( a7 )
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMBINATION, adRegisters, rangeToken, separatorToken, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 16 ) } );

      sys.AddOpcode( "movem.l", 0x4CD80000, 4, AddressingType.INDIRECT, 0 )       //       movem.l (ax)+,d0/a1-a2
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 16 ),
          new OpcodeExpression( OpcodePartialExpression.COMBINATION, adRegistersReverse, rangeToken, separatorToken, 0 ) } );
      sys.AddOpcode( "movem.w", 0x4C980000, 4, AddressingType.INDIRECT, 0 )       //       movem.w (ax)+,d0/a1-a2
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 16 ),
          new OpcodeExpression( OpcodePartialExpression.COMBINATION, adRegistersReverse, rangeToken, separatorToken, 0 ) } );
      sys.AddOpcode( "movem", 0x4C980000, 4, AddressingType.INDIRECT, 0 )       //       movem (ax)+,d0/a1-a2
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 16 ),
          new OpcodeExpression( OpcodePartialExpression.COMBINATION, adRegistersReverse, rangeToken, separatorToken, 0 ) } );

      sys.AddOpcode( "muls.w", 0xC1FC0000, 1, AddressingType.IMMEDIATE_8BIT, 0 )  // muls.w #$01, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );

      sys.AddOpcode( "muls.w", 0xC1F80000, 2, AddressingType.INDIRECT, 0 )  // muls.w ($0001).w, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );

      sys.AddOpcode( "muls.w", 0xC1C0, 0, AddressingType.IMPLICIT, 0 )      // muls.w d0,d2
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "muls.w", 0xC1D0, 0, AddressingType.IMPLICIT, 0 )      // muls.w (a0),d2
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "muls.w", 0xC1D8, 0, AddressingType.IMPLICIT, 0 )      // muls.w (a0)+,d2
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "muls.w", 0xC1E0, 0, AddressingType.IMPLICIT, 0 )      // muls.w -(a0),d2
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );


      sys.AddOpcode( "muls.w", 0xC1E80000, 2, AddressingType.IMPLICIT, 0 )    // muls.w $01(a7), d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_8BIT, 0 ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 16 ),
              new ValidValueGroup( closingParenthesis )
            }
          },
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );

      sys.AddOpcode( "mulu.w", 0xC0FC0000, 1, AddressingType.IMMEDIATE_8BIT, 0 )  // mulu.w #$01, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );

      sys.AddOpcode( "mulu.w", 0xC0F80000, 2, AddressingType.INDIRECT, 0 )  // mulu.w ($0001).w, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );

      sys.AddOpcode( "mulu.w", 0xC0C0, 0, AddressingType.IMPLICIT, 0 )      // mulu.w d0,d2
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "mulu.w", 0xC0D0, 0, AddressingType.IMPLICIT, 0 )      // mulu.w (a0),d2
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "mulu.w", 0xC0D8, 0, AddressingType.IMPLICIT, 0 )      // mulu.w (a0)+,d2
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "mulu.w", 0xC0E0, 0, AddressingType.IMPLICIT, 0 )      // mulu.w -(a0),d2
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "mulu.w", 0xC0E80000, 2, AddressingType.IMPLICIT, 0 )    // mulu.w $01(a7), d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_8BIT, 0 ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 16 ),
              new ValidValueGroup( closingParenthesis )
            }
          },
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );

      sys.AddOpcode( "neg.l", 0x4480, 0, AddressingType.IMPLICIT, 0 )       // neg.l d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ) } );

      sys.AddOpcode( "neg.w", 0x4450, 0, AddressingType.IMPLICIT, 0 )       // neg.w (a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ) } );

      sys.AddOpcode( "neg.w", 0x4460, 0, AddressingType.IMPLICIT, 0 )       // neg.w -(a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 0 ) } );

      sys.AddOpcode( "neg.w", 0x44680000, 2, AddressingType.IMPLICIT, 0 )    // neg.w $01(a7)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_8BIT, 0 ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 16 ),
              new ValidValueGroup( closingParenthesis )
            }
          } } );

      sys.AddOpcode( "neg.w", 0x4458, 0, AddressingType.IMPLICIT, 0 )       // neg.w (a0)+
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 0 ) } );

      sys.AddOpcode( "nop", 0x4E71, 0, AddressingType.IMPLICIT, 0 );

      sys.AddOpcode( "not.w", 0x46780000, 2, AddressingType.IMPLICIT, 0 )       // not.w ($0001).w       ;46 78 00 01
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );

      sys.AddOpcode( "not.l", 0x4680, 0, AddressingType.IMPLICIT, 0 )       // not.l d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ) } );

      sys.AddOpcode( "not.w", 0x4650, 0, AddressingType.IMPLICIT, 0 )       // not.w (a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ) } );

      sys.AddOpcode( "not.w", 0x4660, 0, AddressingType.IMPLICIT, 0 )       // not.w -(a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 0 ) } );

      sys.AddOpcode( "not.w", 0x46680000, 2, AddressingType.IMPLICIT, 0 )    // not.w $01(a7)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_8BIT, 0 ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 16 ),
              new ValidValueGroup( closingParenthesis )
            }
          } } );

      sys.AddOpcode( "not.w", 0x4658, 0, AddressingType.IMPLICIT, 0 )       // not.w (a0)+
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 0 ) } );

      sys.AddOpcode( "ori.b", 0x00000000, 4, AddressingType.IMMEDIATE_32BIT, 0 )  // ori.b #$01, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 16 ) } );

      sys.AddOpcode( "ori.w", 0x00400000, 4, AddressingType.IMMEDIATE_32BIT, 0 )  // ori.w #$1234, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 16 ) } );

      sys.AddOpcode( "ori.l", 0x008000000000, 6, AddressingType.IMMEDIATE_32BIT, 0 )  // ori.l #$123456, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_24BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 32 ) } );

      sys.AddOpcode( "ori.b", 0x003800000000, 6, AddressingType.IMMEDIATE_16BIT, 0 )   // ori.b #$12, ($1234).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );

      sys.AddOpcode( "ori.w", 0x007800000000, 6, AddressingType.IMMEDIATE_16BIT, 0 )   // ori.w #$1234, ($1234).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );

      sys.AddOpcode( "ori.b", 0x00100000, 4, AddressingType.IMMEDIATE_8BIT, 0 )        // ori.b #$01, (a7)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 16 ) } );

      sys.AddOpcode( "ori.b", 0x00200000, 4, AddressingType.IMMEDIATE_8BIT, 0 )             // ori.b #$01, -(a7)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 16 ) } );

      sys.AddOpcode( "ori.b", 0x002800000000, 6, AddressingType.INDIRECT, 0 )          // ori.b #$12, $34(a1)
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

      sys.AddOpcode( "ori.b", 0x00180000, 4, AddressingType.IMMEDIATE_8BIT, 0 )             // ori.b #$01, (a7)+
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 16 ) } );

      sys.AddOpcode( "ori.w", 0x006800000000, 6, AddressingType.INDIRECT, 0 )          // ori.w #$1234, $34(a1)
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

      sys.AddOpcode( "ori.b", 0x003000000000, 6, AddressingType.IMMEDIATE_8BIT, 0 )         // ori.b #$01,(a0,d7.w)
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

      sys.AddOpcode( "ori.w", 0x007000000000, 6, AddressingType.IMMEDIATE_16BIT, 0 )    // ori.w #$0001, $FF(a0,d7.w)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_7BIT ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 32 ),
              new ValidValueGroup( comma ),
              new ValidValueGroup( dRegistersDotW, OpcodePartialExpression.TOKEN_LIST, 12 ),
              new ValidValueGroup( closingParenthesis )
            }
          } } );

      sys.AddOpcode( "ori", 0x007C0000, 4, AddressingType.IMMEDIATE_16BIT, 0 )     // ori #$0001,sr
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.TOKEN_LIST, sr ) } );

      sys.AddOpcode( "or.b", 0x8000, 2, AddressingType.IMPLICIT, 0 )    // or.b d0, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "or.b", 0x80380000, 2, AddressingType.INDIRECT, 0 )       // or.b ($0001).w, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );

      sys.AddOpcode( "or.w", 0x80780000, 2, AddressingType.INDIRECT, 0 )       // or.w ($0001).w, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );

      sys.AddOpcode( "or.w", 0x8050, 2, AddressingType.IMPLICIT, 0 )         // or.w (a0), d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "or.w", 0x8058, 2, AddressingType.IMPLICIT, 0 )         // or.w (a0)+, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "or.w", 0x8060, 2, AddressingType.IMPLICIT, 0 )         // or.w -(a0), d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "or.w", 0x80680000, 4, AddressingType.IMPLICIT, 0 )    // or.w $01(a7), d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_8BIT, 0 ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 16 ),
              new ValidValueGroup( closingParenthesis )
            }
          },
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );

      sys.AddOpcode( "or.w", 0x81780000, 4, AddressingType.INDIRECT, 0 )           // or.w d0, ($0001).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );

      sys.AddOpcode( "or.w", 0x8150, 2, AddressingType.INDIRECT, 0 )               // or.w d0,(a7)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ) } );

      sys.AddOpcode( "or.w", 0x8158, 2, AddressingType.INDIRECT, 0 )               // or.w d0,(a7)+
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 0 ) } );

      sys.AddOpcode( "or.w", 0x8160, 2, AddressingType.INDIRECT, 0 )               // or.w d0,-(a7)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 0 ) } );

      sys.AddOpcode( "or.w", 0x81680000, 4, AddressingType.INDIRECT, 0 )          // or.w d0, $01(a7)
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

      sys.AddOpcode( "pea", 0x48780000, 4, AddressingType.INDIRECT, 0 )        // pea ($0001).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW ) } );

      // does not actually exist?
      /*
      sys.AddOpcode( "pea", 0x4840, 2, AddressingType.IMPLICIT, 0 )            // pea d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ) } );*/

      sys.AddOpcode( "pea", 0x4850, 2, AddressingType.IMPLICIT, 0 )            // pea (a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ) } );

      sys.AddOpcode( "pea", 0x48680000, 4, AddressingType.INDIRECT, 0 )    // pea $12(a7)   
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
          } } );

      sys.AddOpcode( "rol.w", 0xE178, 2, AddressingType.IMPLICIT, 0 )    // rol.w d0, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ) } );

      sys.AddOpcode( "ror.b", 0xE018, 2, AddressingType.IMMEDIATE_8BIT, 0 )        // ror.b #1, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, immediatePrefix, quickAdd, empty, 9 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters ) } );

      sys.AddOpcode( "ror.l", 0xE098, 2, AddressingType.IMMEDIATE_8BIT, 0 )        // ror.l #1, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, immediatePrefix, quickAdd, empty, 9 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters ) } );

      sys.AddOpcode( "rol.w", 0xE7F80000, 4, AddressingType.INDIRECT, 0 )        // rol.w ($0001).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW ) } );

      sys.AddOpcode( "rte", 0x4E73, 0, AddressingType.IMPLICIT, 0 ).Flags = OpcodeFlag.IS_END_OF_CODE;  // rte
      sys.AddOpcode( "rtr", 0x4E77, 0, AddressingType.IMPLICIT, 0 ).Flags = OpcodeFlag.IS_END_OF_CODE;  // rtr
      sys.AddOpcode( "rts", 0x4E75, 0, AddressingType.IMPLICIT, 0 ).Flags = OpcodeFlag.IS_END_OF_CODE;  // rts

      sys.AddOpcode( "st", 0x50F80000, 4, AddressingType.INDIRECT, 0 )        // st ($0001).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW ) } );

      sys.AddOpcode( "sf", 0x51F80000, 4, AddressingType.INDIRECT, 0 )        // sf ($0001).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW ) } );

      sys.AddOpcode( "shi", 0x52F80000, 4, AddressingType.INDIRECT, 0 )        // shi ($0001).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW ) } );

      sys.AddOpcode( "sls", 0x53F80000, 4, AddressingType.INDIRECT, 0 )        // sls ($0001).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW ) } );

      sys.AddOpcode( "scc", 0x54F80000, 4, AddressingType.INDIRECT, 0 )        // scc ($0001).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW ) } );

      sys.AddOpcode( "scs", 0x55F80000, 4, AddressingType.INDIRECT, 0 )        // scs ($0001).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW ) } );

      sys.AddOpcode( "sne", 0x56F80000, 4, AddressingType.INDIRECT, 0 )        // sne ($0001).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW ) } );

      sys.AddOpcode( "seq", 0x57F80000, 4, AddressingType.INDIRECT, 0 )        // seq ($0001).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW ) } );

      sys.AddOpcode( "svc", 0x58F80000, 4, AddressingType.INDIRECT, 0 )        // svc ($0001).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW ) } );

      sys.AddOpcode( "svs", 0x59F80000, 4, AddressingType.INDIRECT, 0 )        // svs ($0001).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW ) } );

      sys.AddOpcode( "spl", 0x5AF80000, 4, AddressingType.INDIRECT, 0 )        // spl ($0001).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW ) } );

      sys.AddOpcode( "smi", 0x5BF80000, 4, AddressingType.INDIRECT, 0 )        // smi ($0001).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW ) } );

      sys.AddOpcode( "sge", 0x5CF80000, 4, AddressingType.INDIRECT, 0 )        // sge ($0001).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW ) } );

      sys.AddOpcode( "slt", 0x5DF80000, 4, AddressingType.INDIRECT, 0 )        // slt ($0001).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW ) } );

      sys.AddOpcode( "sgt", 0x5EF80000, 4, AddressingType.INDIRECT, 0 )        // sgt ($0001).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW ) } );

      sys.AddOpcode( "sle", 0x5FF80000, 4, AddressingType.INDIRECT, 0 )        // sle ($0001).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW ) } );

      sys.AddOpcode( "stop", 0x4E720000, 4, AddressingType.ABSOLUTE, 0 )       // stop #$1234
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty ) } );

      sys.AddOpcode( "suba.w", 0x90FC0000, 4, AddressingType.IMMEDIATE_16BIT, 0 )          // suba.w #$20, a0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 25 ) } );

      sys.AddOpcode( "suba.w", 0x90F80000, 4, AddressingType.INDIRECT, 0 )              // suba.w ($1234).w, a0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 25 ) } );

      sys.AddOpcode( "suba.l", 0x91F900000000u, 4, AddressingType.INDIRECT, 0 )              // suba.l ($12345678).l, a0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT, openingParenthesis, closingParenthesisDotL, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 33 ) } );

      sys.AddOpcode( "suba.w", 0x90C0, 2, AddressingType.IMPLICIT, 0 )    // suba.w d7, a0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 9 ) } );

      sys.AddOpcode( "suba.w", 0x90D0, 2, AddressingType.IMPLICIT, 0 )                     // suba.w (a0), a1
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 9 ) } );

      sys.AddOpcode( "suba.w", 0x90E0, 2, AddressingType.IMPLICIT, 0 )                     // suba.w -(a0), a1
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 9 ) } );

      sys.AddOpcode( "suba.w", 0x90E80000, 4, AddressingType.INDIRECT, 0 )                      // suba.w $1234(a0), a1
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_15BIT ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 16 ),
              new ValidValueGroup( closingParenthesis )
            }
          },
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 25 ) } );

      sys.AddOpcode( "suba.w", 0x90D8, 2, AddressingType.IMPLICIT, 0 )                          // suba.w (a0)+,a1
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 9 ) } );

      sys.AddOpcode( "suba.w", 0x90F00000, 4, AddressingType.IMPLICIT, 0 )                      // suba.w (a3,d1.w), a3
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

      sys.AddOpcode( "suba.w", 0x90F00000, 4, AddressingType.INDIRECT, 0 )                      // suba.w $12(a3,d1.w), a3
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_7BIT ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 16 ),
              new ValidValueGroup( comma ),
              new ValidValueGroup( dRegistersDotW, OpcodePartialExpression.TOKEN_LIST, 12 ),
              new ValidValueGroup( closingParenthesis )
            }
          },
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 25 ) } );

      sys.AddOpcode( "subi.b", 0x04000000, 4, AddressingType.IMMEDIATE_8BIT, 0 )    // subi.b #$01, d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 16 ) } );

      sys.AddOpcode( "subi.l", 0x048000000000, 6, AddressingType.IMMEDIATE_32BIT, 0 )    // subi.l #$00000001, d7
      .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 32 ) } );

      sys.AddOpcode( "subi.w", 0x047800000000, 6, AddressingType.IMMEDIATE_16BIT, 0 )             // subi.w #$1234, ($1234).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );

      sys.AddOpcode( "subi.w", 0x0479000000000000, 8, AddressingType.IMMEDIATE_16BIT, 0 )             // subi.w #$1234, ($12345678).l
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, immediatePrefix, empty, 32 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT, openingParenthesis, closingParenthesisDotL, 0 ) } );

      sys.AddOpcode( "subi.l", 0x04B8000000000000, 8, AddressingType.IMMEDIATE_16BIT, 0 )             // subi.l #$00000001, ($FF00).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_32BIT, immediatePrefix, empty, 16 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );

      sys.AddOpcode( "subi.b", 0x04100000, 4, AddressingType.IMMEDIATE_8BIT, 0 )             // subi.b #$01, (a7)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 0 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 16 ) } );

      sys.AddOpcode( "subi.w", 0x046800000000, 6, AddressingType.INDIRECT, 0 )               // subi.i #$12, $34(a1)
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

      sys.AddOpcode( "subi.l", 0x04A8000000000000, 8, AddressingType.INDIRECT, 0 )              // subi.l #$00000001, $FF(a0)
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

      sys.AddOpcode( "subi.b", 0x04180000, 4, AddressingType.IMPLICIT, 0 )              // subi.b #$01, (a0)+
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 16 ) } );

      sys.AddOpcode( "subi.b", 0x04200000, 4, AddressingType.IMPLICIT, 0 )              // subi.b #$01, -(a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 16 ) } );

      sys.AddOpcode( "subq.b", 0x51380000, 4, AddressingType.IMMEDIATE_16BIT, 0 )        // subq.b #1, ($0001).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, immediatePrefix, quickAdd, empty, 25 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW ) } );

      sys.AddOpcode( "subq.b", 0x5100, 2, AddressingType.IMMEDIATE_8BIT, 0 )        // subq.b #1, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, immediatePrefix, quickAdd, empty, 9 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters ) } );

      sys.AddOpcode( "subq.w", 0x5140, 2, AddressingType.IMMEDIATE_8BIT, 0 )        // subq.w #1, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, immediatePrefix, quickAdd, empty, 9 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters ) } );

      sys.AddOpcode( "subq.w", 0x5150, 2, AddressingType.IMMEDIATE_8BIT, 0 )        // subq.w #1, (a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, immediatePrefix, quickAdd, empty, 9 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ) } );

      sys.AddOpcode( "subq.w", 0x5160, 2, AddressingType.IMMEDIATE_8BIT, 0 )        // subq.w #1, -(a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_8BIT, immediatePrefix, empty, 9 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 0 ) } );

      sys.AddOpcode( "subq.b", 0x51280000, 4, AddressingType.IMPLICIT, 0 )          // subq.b #1, $01(a7)
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

      sys.AddOpcode( "subq.w", 0x51680000, 4, AddressingType.IMPLICIT, 0 )          // subq.w #1, $01(a7)
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

      sys.AddOpcode( "subq.w", 0x5158, 2, AddressingType.IMMEDIATE_8BIT, 0 )        // subq.w #1, (a0)+
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, immediatePrefix, quickAdd, empty, 9 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 0 ) } );

      sys.AddOpcode( "subq.w", 0x5148, 2, AddressingType.IMMEDIATE_8BIT, 0 )        // subq.w #1, a0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, immediatePrefix, quickAdd, empty, 9 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters ) } );

      sys.AddOpcode( "subq.l", 0x5188, 2, AddressingType.IMMEDIATE_8BIT, 0 )        // subq.l #1, a0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, immediatePrefix, quickAdd, empty, 9 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters ) } );

      sys.AddOpcode( "sub.w", 0x9040, 2, AddressingType.IMPLICIT, 0 )               // sub.w d0,d1
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "sub.w", 0x90780000, 4, AddressingType.INDIRECT, 0 )           // sub.w ($1234).w, d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ) } );

      sys.AddOpcode( "sub.w", 0x9050, 2, AddressingType.IMPLICIT, 0 )               // sub.w (a0), d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "sub.b", 0x90280000, 4, AddressingType.IMMEDIATE_8BIT, 0 )                      // sub.b $01(a0),d0
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

      sys.AddOpcode( "sub.w", 0x9060, 2, AddressingType.IMPLICIT, 0 )                     // sub.w -(a0),d7
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "sub.w", 0x90680000, 4, AddressingType.IMMEDIATE_8BIT, 0 )                      // sub.w $01(a0),d0
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

      sys.AddOpcode( "sub.w", 0x9058, 2, AddressingType.IMPLICIT, 0 )                     // sub.w (a0)+,d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "sub.l", 0x9088, 2, AddressingType.IMPLICIT, 0 )               // sub.l a0,d1
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 0 ),
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ) } );

      sys.AddOpcode( "sub.w", 0x91780000, 4, AddressingType.INDIRECT, 0 )           // sub.w d0, ($0001).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 25 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );

      sys.AddOpcode( "sub.w", 0x9150, 2, AddressingType.INDIRECT, 0 )               // sub.w d0,(a7)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ) } );

      sys.AddOpcode( "sub.w", 0x9160, 2, AddressingType.INDIRECT, 0 )               // sub.w d0,-(a7)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesisMinus, aRegisters, closingParenthesis, 0 ) } );

      sys.AddOpcode( "sub.w", 0x91680000, 4, AddressingType.INDIRECT, 0 )          // sub.w d0, $01(a7)
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

      sys.AddOpcode( "sub.l", 0x91A80000, 4, AddressingType.INDIRECT, 0 )          // sub.l d0, $01(a7)
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

      sys.AddOpcode( "sub.l", 0x9198, 2, AddressingType.INDIRECT, 0 )               // sub.l d0,(a7)+
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 9 ),
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesisPlus, 0 ) } );

      sys.AddOpcode( "swap", 0x4840, 2, AddressingType.IMPLICIT, 0 )               // swap d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ) } );

      sys.AddOpcode( "tas.b", 0x4AF80000, 4, AddressingType.INDIRECT, 0 )    // tas.b ($1234).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );

      sys.AddOpcode( "tas.b", 0x4AC0, 2, AddressingType.IMPLICIT, 0 )               // tas.b d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ) } );

      sys.AddOpcode( "tas.b", 0x4AD0, 2, AddressingType.IMPLICIT, 0 )            // tas.b (a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ) } );

      sys.AddOpcode( "tas.b", 0x4AE80000, 4, AddressingType.INDIRECT, 0 )    // tas.b $12(a7)   
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
          } } );

      sys.AddOpcode( "trap", 0x4E40, 2, AddressingType.IMMEDIATE_8BIT, 0 )        // trap #1
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, immediatePrefix, quickAdd, empty, 0 ) } );

      sys.AddOpcode( "trapv", 0x4E76, 0, AddressingType.IMPLICIT, 0 );       // trap #1

      sys.AddOpcode( "tst.b", 0x4A00, 2, AddressingType.IMPLICIT, 0 )               // tst.b d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ) } );

      sys.AddOpcode( "tst.l", 0x4A80, 2, AddressingType.IMPLICIT, 0 )               // tstl d0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, dRegisters, 0 ) } );

      sys.AddOpcode( "tst.b", 0x4A380000, 4, AddressingType.INDIRECT, 0 )    // tst.b ($1234).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );

      sys.AddOpcode( "tst.w", 0x4A780000, 4, AddressingType.INDIRECT, 0 )    // tst.w ($1234).w
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_16BIT, openingParenthesis, closingParenthesisDotW, 0 ) } );

      sys.AddOpcode( "tst.w", 0x4A7900000000, 6, AddressingType.IMMEDIATE_32BIT, 0 )  // tst.w ($123456).l
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_24BIT, openingParenthesis, closingParenthesisDotL, 0 ) } );

      sys.AddOpcode( "tst.l", 0x4AB900000000, 6, AddressingType.IMMEDIATE_32BIT, 0 )  // tst.l ($123456).l
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_EXPRESSION_24BIT, openingParenthesis, closingParenthesisDotL, 0 ) } );

      sys.AddOpcode( "tst.w", 0x4A50, 2, AddressingType.IMPLICIT, 0 )       // tst.w (a0)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.ENCAPSULATED_VALUE_FROM_LIST, openingParenthesis, aRegisters, closingParenthesis, 0 ) } );

      sys.AddOpcode( "tst.w", 0x4A680000, 4, AddressingType.IMPLICIT, 0 )    // tst.w $01(a7)
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.COMPLEX )
          {
            ValidValues = new List<ValidValueGroup>()
            {
              new ValidValueGroup( empty, OpcodePartialExpression.EXPRESSION_8BIT, 0 ),
              new ValidValueGroup( openingParenthesis ),
              new ValidValueGroup( aRegisters, OpcodePartialExpression.TOKEN_LIST, 16 ),
              new ValidValueGroup( closingParenthesis )
            }
          } } );

      sys.AddOpcode( "unlk", 0x4E58, 2, AddressingType.IMPLICIT, 0 )               // unlk a0
        .ParserExpressions.AddRange( new List<OpcodeExpression>() {
          new OpcodeExpression( OpcodePartialExpression.VALUE_FROM_LIST, aRegisters, 0 ) } );


      return sys;
    }



  }
}
