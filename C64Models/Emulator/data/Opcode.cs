using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tiny64
{
  public class Opcode
  {
    public enum OpcodePartialExpression
    {
      UNUSED,
      EXPRESSION_7BIT,                // seriously, Motorola?
      EXPRESSION_8BIT,
      EXPRESSION_8BIT_RELATIVE,
      ENCAPSULATED_EXPRESSION_8BIT,   // e.g. LD r,>(IX+d)<
      EXPRESSION_15BIT,               // seriously, Motorola? #2
      EXPRESSION_16BIT,
      EXPRESSION_16BIT_RELATIVE,
      ENCAPSULATED_EXPRESSION_16BIT,  // e.g. LD A,(>nn<)
      EXPRESSION_24BIT,
      ENCAPSULATED_EXPRESSION_24BIT,  // e.g. adda.w ($fffff100).w,a2
      EXPRESSION_32BIT,
      ENCAPSULATED_EXPRESSION_32BIT,  // e.g. adda.l #$12345678,d0
      VALUE_FROM_LIST,                // e.g. lda $f0,x    ld D,B
      ENCAPSULATED_VALUE_FROM_LIST,   // e.g. adda.b (a0)
      TOKEN_LIST,                     // e.g. (HL)
      COMPLEX,                        // mostly value from list, but with expression in front or in the middle
      EMPTY,                          // none (RET), to differ from RET cc
      COMBINATION                     // seriously, Motorola? #3 - Major WTF (register range mapped to bits)
    }



    public class ValidValue
    {
      public string   Key = "";
      public ulong    ReplacementValue = ulong.MaxValue;

      public ValidValue( string Key )
      {
        this.Key = Key;
      }

      public ValidValue( string Key, ulong ReplacementValue )
      {
        this.Key              = Key;
        this.ReplacementValue = ReplacementValue;
      }

    }

    public class ValidValueGroup
    {
      public OpcodePartialExpression  Expression = OpcodePartialExpression.UNUSED;
      public List<ValidValue>         ValidValues = new List<ValidValue>();
      public int                      ReplacementValueShift = 0;

      public ValidValueGroup( List<ValidValue> Values, int ReplacementValueShift = 0 )
      {
        ValidValues                 = Values;
        this.ReplacementValueShift  = ReplacementValueShift;
      }

      public ValidValueGroup( List<ValidValue> Values, OpcodePartialExpression ExpressionType, int ReplacementValueShift = 0 )
      {
        ValidValues                 = Values;
        Expression                  = ExpressionType;
        this.ReplacementValueShift  = ReplacementValueShift;
      }
    }

    public class OpcodeExpression
    {
      public OpcodePartialExpression    Type = OpcodePartialExpression.UNUSED;
      public List<ValidValueGroup>      ValidValues = new List<ValidValueGroup>();
      public int                        ReplacementValueShift = 0;
      public ulong                      ResultingReplacementValue = 0;

      public OpcodeExpression( OpcodePartialExpression Type )
      {
        this.Type = Type;
      }

      public OpcodeExpression( OpcodePartialExpression Type, List<ValidValue> ValidValues ) 
      {
        this.Type         = Type;
        this.ValidValues.Add( new ValidValueGroup( ValidValues ) );
      }

      public OpcodeExpression( OpcodePartialExpression Type, List<ValidValue> ValidValues, int ReplacementValueShift )
      {
        this.Type                   = Type;
        this.ValidValues.Add( new ValidValueGroup( ValidValues, ReplacementValueShift ) );
        this.ReplacementValueShift  = ReplacementValueShift;
      }

      public OpcodeExpression( OpcodePartialExpression Type, List<ValidValue> ValidValues, List<ValidValue> ValidValues2 )
      {
        this.Type         = Type;
        this.ValidValues.Add( new ValidValueGroup( ValidValues ) );
        this.ValidValues.Add( new ValidValueGroup( ValidValues2 ) );
      }

      public OpcodeExpression( OpcodePartialExpression Type, List<ValidValue> ValidValues, List<ValidValue> ValidValues2, int ReplacementValueShift )
      {
        this.Type                   = Type;
        this.ValidValues.Add( new ValidValueGroup( ValidValues ) );
        this.ValidValues.Add( new ValidValueGroup( ValidValues2 ) );
        this.ReplacementValueShift  = ReplacementValueShift;
      }

      public OpcodeExpression( OpcodePartialExpression Type, List<ValidValue> ValidValues, List<ValidValue> ValidValues2, List<ValidValue> ValidValues3, int ReplacementValueShift )
      {
        this.Type = Type;
        this.ValidValues.Add( new ValidValueGroup( ValidValues ) );
        this.ValidValues.Add( new ValidValueGroup( ValidValues2 ) );
        this.ValidValues.Add( new ValidValueGroup( ValidValues3 ) );
        this.ReplacementValueShift = ReplacementValueShift;
      }
    }



    public enum AddressingType
    {
      UNKNOWN,
      IMPLICIT,                 // e.g. lsr a, dex
      IMMEDIATE_ACCU,           // e.g. lda #02
      IMMEDIATE_REGISTER,       // e.g. ldy #02
      IMMEDIATE_8BIT,           // always 8bit, sep and rep (65816)
      ABSOLUTE,
      ABSOLUTE_X,
      ABSOLUTE_Y,
      ZEROPAGE,                 // e.g. lda $01
      ZEROPAGE_X,               // lda $12,x
      ZEROPAGE_Y,               // lda $12,y
      INDIRECT,                 // jmp ($1234), jsr ($1234)
      ZEROPAGE_INDIRECT_X,      // adc ($12,x)
      ZEROPAGE_INDIRECT_Y,      // lda ($12),y
      RELATIVE,

      // WD65C02
      ZEROPAGE_INDIRECT,        // e.g. lda ($12)
      ABSOLUTE_INDIRECT_X,      // e.g. jmp ($1234,x)
      ZEROPAGE_RELATIVE,        // e.g. BBR0 $12,LABEL

      // 65SC02
      ZEROPAGE_INDIRECT_Z,      // lda ($12),z
      RELATIVE_16,              // e.g. LBNE 16BIT_DELTA    rl
      ZEROPAGE_INDIRECT_SP_Y,   // lda ($zp,SP),y           (d,s),y
      IMMEDIATE_16BIT,             // phw #$nnnn

      // 65816
      ABSOLUTE_LONG_X,          // adc $123456,x            al,x
      ABSOLUTE_LONG,            // adc $123456              al
      ABSOLUTE_INDIRECT_LONG,   // jmp [$hhll]
      BLOCK_MOVE_XYC,           // mv $12,$34
      ZEROPAGE_INDIRECT_SP,     // (d,s)
      ZEROPAGE_INDIRECT_LONG,   // [d]
      ZEROPAGE_INDIRECT_Y_LONG, // [d],y
      STACK_RELATIVE,           // d,s

      // 68000
      IMMEDIATE_32BIT           // addi.l #$12345678,d0
    }

    public string                         Mnemonic = "";
    public ulong                          ByteValue = ulong.MaxValue;
    public int                            OpcodeSize = -1;
    public AddressingType                 Addressing = AddressingType.UNKNOWN;
    public int                            NumCycles = 0;
    public int                            NumPenaltyCycles = 0;
    public int                            PageBoundaryCycles = 0;
    public int                            BranchSamePagePenalty = 0;
    public int                            BranchOtherPagePenalty = 0;
    public int                            NumNopsToPrefix = 0;

    public List<OpcodeExpression>         ParserExpressions = new List<OpcodeExpression>();



    public int StartingTokenCount 
    {
      get
      {
        switch ( Addressing )
        {
          case AddressingType.ABSOLUTE:
          case AddressingType.ABSOLUTE_X:
          case AddressingType.ABSOLUTE_Y:
          case AddressingType.IMMEDIATE_ACCU:
          case AddressingType.IMMEDIATE_16BIT:
          case AddressingType.IMPLICIT:
          case AddressingType.RELATIVE:
          case AddressingType.RELATIVE_16:
          case AddressingType.ZEROPAGE:
          case AddressingType.ZEROPAGE_RELATIVE:
          case AddressingType.ZEROPAGE_X:
          case AddressingType.ZEROPAGE_Y:
            return 0;
          case AddressingType.ABSOLUTE_INDIRECT_X:
          case AddressingType.INDIRECT:
          case AddressingType.ZEROPAGE_INDIRECT:
          case AddressingType.ZEROPAGE_INDIRECT_SP_Y:
          case AddressingType.ZEROPAGE_INDIRECT_Z:
          case AddressingType.ZEROPAGE_INDIRECT_Y_LONG:
          case AddressingType.ZEROPAGE_INDIRECT_Y:
            // jmp ($1234,x)
            // jsr ($1234)
            // and ($12)
            // lda($zp, SP ),y
            // lda ($12),z
            // BBR0 $12,LABEL
            // lda $12,x
            // lda $12,y
            return 1;
          
        }
        return 0;
      }
    }



    public int TrailingTokenCount 
    {
      get
      {
        switch ( Addressing )
        {
          case AddressingType.ZEROPAGE_INDIRECT:
          case AddressingType.INDIRECT:
            return 1;
          case AddressingType.ABSOLUTE_X:
          case AddressingType.ZEROPAGE_X:
          case AddressingType.ABSOLUTE_Y:
          case AddressingType.ZEROPAGE_Y:
          case AddressingType.ABSOLUTE_LONG_X:
          case AddressingType.STACK_RELATIVE:
            // use ,<something>
            return 2;
          case AddressingType.ZEROPAGE_INDIRECT_Y_LONG:
          case AddressingType.ZEROPAGE_INDIRECT_Y:
          case AddressingType.ZEROPAGE_INDIRECT_Z:
          case AddressingType.ABSOLUTE_INDIRECT_X:
            return 3;
          case AddressingType.ZEROPAGE_INDIRECT_SP_Y:
            return 5;
        }
        return 0;
      }
    }



    public Opcode( string Mnemonic, ulong ByteValue, AddressingType Addressing )
    {
      this.Mnemonic = Mnemonic;
      this.ByteValue = ByteValue;
      OpcodeSize = 0;
      this.Addressing = Addressing;
    }

    public Opcode( string Mnemonic, ulong ByteValue, AddressingType Addressing, int NumCycles, int PageBoundaryCycles )
    {
      this.Mnemonic = Mnemonic;
      this.ByteValue = ByteValue;
      OpcodeSize = 0;
      this.Addressing = Addressing;
      this.NumCycles = NumCycles;
      this.PageBoundaryCycles = PageBoundaryCycles;
      NumPenaltyCycles = PageBoundaryCycles;
    }

    public Opcode( string Mnemonic, ulong ByteValue, int NumOperands, AddressingType Addressing )
    {
      this.Mnemonic = Mnemonic;
      this.ByteValue = ByteValue;
      this.OpcodeSize = NumOperands;
      this.Addressing = Addressing;
    }

    public Opcode( string Mnemonic, ulong ByteValue, int NumOperands, AddressingType Addressing, int NumCycles, int PageBoundaryCycles )
    {
      this.Mnemonic = Mnemonic;
      this.ByteValue = ByteValue;
      this.OpcodeSize = NumOperands;
      this.Addressing = Addressing;
      this.NumCycles = NumCycles;
      this.PageBoundaryCycles = PageBoundaryCycles;
      NumPenaltyCycles = PageBoundaryCycles;
    }

    public Opcode( string Mnemonic, ulong ByteValue, int NumOperands, AddressingType Addressing, int NumCycles, int PageBoundaryCycles, int BranchSamePagePenalty, int BranchOtherPagePenalty )
    {
      this.Mnemonic = Mnemonic;
      this.ByteValue = ByteValue;
      this.OpcodeSize = NumOperands;
      this.Addressing = Addressing;
      this.NumCycles = NumCycles;
      this.PageBoundaryCycles = PageBoundaryCycles;
      this.BranchOtherPagePenalty = BranchOtherPagePenalty;
      this.BranchSamePagePenalty = BranchSamePagePenalty;
      NumPenaltyCycles = PageBoundaryCycles;

      NumPenaltyCycles += Math.Max( BranchOtherPagePenalty, BranchSamePagePenalty );
    }
  }
}
