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
      EXPRESSION_8BIT,
      EXPRESSION_16BIT,
      EXPRESSION_24BIT,
      EXPRESSION_32BIT,
      PARENTHESIS_OPEN,
      PARENTHESIS_CLOSE,
      COMMA,
      VALUE_FROM_LIST,                // e.g. lda $f0,x    ld D,B
      TOKEN_LIST,                     // e.g. (HL)
      ENCAPSULATED_EXPRESSION_8BIT,   // e.g. LD r,>(IX+d)<
      ENCAPSULATED_EXPRESSION_16BIT   // e.g. LD A,(>nn<)
    }



    public class ValidValue
    {
      public string   Key = "";
      public uint     ReplacementValue = uint.MaxValue;

      public ValidValue( string Key )
      {
        this.Key = Key;
      }

      public ValidValue( string Key, uint ReplacementValue )
      {
        this.Key              = Key;
        this.ReplacementValue = ReplacementValue;
      }

    }

    public class OpcodeExpression
    {
      public OpcodePartialExpression    Type = OpcodePartialExpression.UNUSED;
      public List<ValidValue>           ValidValues = new List<ValidValue>();
      public List<ValidValue>           ValidValues2 = new List<ValidValue>();
      public int                        ReplacementValueShift = 0;
      public int                        ResultingReplacementValue = 0;

      public OpcodeExpression( OpcodePartialExpression Type )
      {
        this.Type = Type;
      }

      public OpcodeExpression( OpcodePartialExpression Type, List<ValidValue> ValidValues ) 
      {
        this.Type         = Type;
        this.ValidValues  = ValidValues;
      }

      public OpcodeExpression( OpcodePartialExpression Type, List<ValidValue> ValidValues, int ReplacementValueShift )
      {
        this.Type                   = Type;
        this.ValidValues            = ValidValues;
        this.ReplacementValueShift  = ReplacementValueShift;
      }

      public OpcodeExpression( OpcodePartialExpression Type, List<ValidValue> ValidValues, List<ValidValue> ValidValues2 )
      {
        this.Type         = Type;
        this.ValidValues  = ValidValues;
        this.ValidValues2 = ValidValues2;
      }

      public OpcodeExpression( OpcodePartialExpression Type, List<ValidValue> ValidValues, List<ValidValue> ValidValues2, int ReplacementValueShift )
      {
        this.Type                   = Type;
        this.ValidValues            = ValidValues;
        this.ValidValues2           = ValidValues2;
        this.ReplacementValueShift  = ReplacementValueShift;
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
      IMMEDIATE_16,             // phw #$nnnn

      // 65816
      ABSOLUTE_LONG_X,          // adc $123456,x            al,x
      ABSOLUTE_LONG,            // adc $123456              al
      ABSOLUTE_INDIRECT_LONG,   // jmp [$hhll]
      BLOCK_MOVE_XYC,           // mv $12,$34
      ZEROPAGE_INDIRECT_SP,     // (d,s)
      ZEROPAGE_INDIRECT_LONG,   // [d]
      ZEROPAGE_INDIRECT_Y_LONG, // [d],y
      STACK_RELATIVE,           // d,s

      // Z80
      REGISTER_TO_REGISTER,       // LD r,r'
      IMMEDIATE_TO_REGISTER,      // LD,n
      HL_INDIRECT_TO_REGISTER,    // LD r,(HL)
      IX_D_INDIRECT_TO_REGISTER,  // LD r,(IX+d)
      IY_D_INDIRECT_TO_REGISTER,  // LD r,(IY+d)
      REGISTER_TO_HL_INDIRECT,    // LD (HL),r
      REGISTER_TO_IX_D_INDIRECT,  // LD (IX+d),r
      REGISTER_TO_IY_D_INDIRECT,  // LD (IY+d),r
      IMMEDIATE_TO_HL_INDIRECT,   // LD (HL),n
      IMMEDIATE_TO_IX_D_INDIRECT, // LD (IX+d),n
      IMMEDIATE_TO_IY_D_INDIRECT, // LD (IY+d),n
      BC_INDIRECT_TO_A,           // LD A,(BC)
      DE_INDIRECT_TO_A,           // LD A,(DE)
      IMMEDIATE_INDIRECT_TO_A,    // LD A,(nn)
      A_TO_BC_INDIRECT,           // LD (BC),A
      A_TO_DE_INDIRECT,           // LD (DE),A
      A_TO_IMMEDIATE_INDIRECT,    // LD (nn),A
      I_TO_A,                     // LD A,I
      R_TO_A,                     // LD A,R
      A_TO_I,                     // LD I,A
      A_TO_R,                     // LD R,A
      IMMEDIATE_TO_REGISTER_DD,   // LD dd,nn
      IMMEDIATE_TO_IX,            // LD IX,nn
      IMMEDIATE_TO_IY,            // LD IY,nn
      IMMEDIATE_INDIRECT_TO_HL,   // LD HL,(nn)
      IMMEDIATE_INDIRECT_TO_REGISTER_DD,    // LD dd,(nn)
      IMMEDIATE_INDIRECT_TO_IX,   // LD IX,(nn)
      IMMEDIATE_INDIRECT_TO_IY,   // LD IY,(nn)
      HL_TO_IMMEDIATE_INDIRECT,   // LD (nn),HL
      REGISTER_DD_TO_IMMEDIATE_INDIRECT,    // LD (nn),dd
      IX_TO_IMMEDIATE_INDIRECT,   // LD (nn),IX
      IY_TO_IMMEDIATE_INDIRECT,   // LD (nn),IY
      HL_TO_SP,                   // LD SP,HL
      IX_TO_SP,                   // LD SP,IX
      IY_TO_SP,                   // LD SP,IY
      REGISTER_QQ,                // PUSH qq
      REGISTER_IX,                // PUSH IX
      REGISTER_IY,                // PUSH IY
      REGISTER_HL_TO_DE,          // EX DE,HL
      REGISTER_AF_TO_AF,          // EX AF,AF'
      REGISTER_HL_TO_SP_INDIRECT, // EX (SP),HL
      REGISTER_IX_TO_SP_INDIRECT, // EX (SP),IX
      REGISTER_IY_TO_SP_INDIRECT, // EX (SP),IY
      REGISTER_TO_A,              // ADD A,r
      IMMEDIATE_TO_A,             // ADD A,n
      HL_INDIRECT_TO_A,           // ADD A,(HL)
      IX_D_INDIRECT_TO_A,         // ADD A,(IX+d)
      IY_D_INDIRECT_TO_A,         // ADD A,(IY+d)
      REGISTER,                   // INC r
      REGISTER_HL_INDIRECT,       // INC (HL)
      REGISTER_IX_D_INDIRECT,     // INC (IX+d)
      REGISTER_IY_D_INDIRECT,     // INC (IY+d)
      IMMEDIATE_0,                // IM 0
      IMMEDIATE_1,                // IM 1
      IMMEDIATE_2,                // IM 2
      REGISTER_DD_TO_HL,          // ADD HL,ss
      REGISTER_PP_TO_IX,          // ADD IX,pp
      REGISTER_PP_TO_IY,          // ADD IY,pp
      REGISTER_DD,                // INC ss
      HL_INDIRECT,                // RLC (HL)
      IX_D_INDIRECT,              // RLC (IX+d) this on is nasty, it has a fixed byte value AFTER the operand
      IY_D_INDIRECT,              // RLC (IX+d) this on is nasty, it has a fixed byte value AFTER the operand
      REGISTER_TO_BIT,            // BIT b,r
      HL_INDIRECT_TO_BIT,         // BIT b,(HL)
      IX_D_INDIRECT_TO_BIT,       // BIT b,(IX+d) this on is nasty, it has a fixed byte value AFTER the operand
      IY_D_INDIRECT_TO_BIT,       // BIT b,(IY+d) this on is nasty, it has a fixed byte value AFTER the operand
      ABSOLUTE_CONDITION,         // JP cc,nn
      IX_INDIRECT,                // JP (IX)
      IY_INDIRECT,                // JP (IY)
      IMPLICIT_CC,                // RET cc
      IMPLICIT_P,                 // RST p
      IMMEDIATE_INDIRECT_TO_A_8BIT, // IN A,(n)
      INDIRECT_C_TO_REGISTER,     // IN r,(C)
      A_TO_IMMEDIATE_INDIRECT_8BIT, // OUT (n),A
      REGISTER_TO_C_INDIRECT      // OUT (C),r
    }

    public string                         Mnemonic = "";
    public uint                           ByteValue = uint.MaxValue;
    public int                            NumOperands = -1;
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
          case AddressingType.IMMEDIATE_16:
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



    public Opcode( string Mnemonic, uint ByteValue, AddressingType Addressing )
    {
      this.Mnemonic = Mnemonic;
      this.ByteValue = ByteValue;
      NumOperands = 0;
      this.Addressing = Addressing;
    }

    public Opcode( string Mnemonic, uint ByteValue, AddressingType Addressing, int NumCycles, int PageBoundaryCycles )
    {
      this.Mnemonic = Mnemonic;
      this.ByteValue = ByteValue;
      NumOperands = 0;
      this.Addressing = Addressing;
      this.NumCycles = NumCycles;
      this.PageBoundaryCycles = PageBoundaryCycles;
      NumPenaltyCycles = PageBoundaryCycles;
    }

    public Opcode( string Mnemonic, uint ByteValue, int NumOperands, AddressingType Addressing )
    {
      this.Mnemonic = Mnemonic;
      this.ByteValue = ByteValue;
      this.NumOperands = NumOperands;
      this.Addressing = Addressing;
    }

    public Opcode( string Mnemonic, uint ByteValue, int NumOperands, AddressingType Addressing, int NumCycles, int PageBoundaryCycles )
    {
      this.Mnemonic = Mnemonic;
      this.ByteValue = ByteValue;
      this.NumOperands = NumOperands;
      this.Addressing = Addressing;
      this.NumCycles = NumCycles;
      this.PageBoundaryCycles = PageBoundaryCycles;
      NumPenaltyCycles = PageBoundaryCycles;
    }

    public Opcode( string Mnemonic, uint ByteValue, int NumOperands, AddressingType Addressing, int NumCycles, int PageBoundaryCycles, int BranchSamePagePenalty, int BranchOtherPagePenalty )
    {
      this.Mnemonic = Mnemonic;
      this.ByteValue = ByteValue;
      this.NumOperands = NumOperands;
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
