using System;
using System.Collections.Generic;
using System.Text;

namespace Tiny64
{
  public class Opcode
  {
    public enum AddressingType
    {
      UNKNOWN,
      IMPLICIT,                 // e.g. lsr a, dex
      IMMEDIATE,                // e.g. lda #02
      ABSOLUTE,
      ABSOLUTE_X,
      ABSOLUTE_Y,
      ZEROPAGE,                 // e.g. lda $01
      ZEROPAGE_X,               // lda $12,x
      ZEROPAGE_Y,               // lda $12,y
      INDIRECT,                 // jmp ($1234), jsr ($1234)
      ZEROPAGE_INDIRECT_X,
      ZEROPAGE_INDIRECT_Y,
      RELATIVE,

      // WD65C02
      ZEROPAGE_INDIRECT,        // e.g. lda ($12)
      ABSOLUTE_INDIRECT_X,      // e.g. jmp ($1234,x)
      ZEROPAGE_RELATIVE,        // e.g. BBR0 $12,LABEL

      // 65SC02
      ZEROPAGE_INDIRECT_Z,      // lda ($12),z
      RELATIVE_16,              // e.g. LBNE 16BIT_DELTA
      ZEROPAGE_INDIRECT_SP_Y,   // lda ($zp,SP),y
      IMMEDIATE_16              // phw #$nnnn
    };

    public string Mnemonic = "";
    public int ByteValue = -1;
    public int NumOperands = -1;
    public AddressingType Addressing = AddressingType.UNKNOWN;
    public int NumCycles = 0;
    public int NumPenaltyCycles = 0;
    public int PageBoundaryCycles = 0;
    public int BranchSamePagePenalty = 0;
    public int BranchOtherPagePenalty = 0;
    public int NumNopsToPrefix = 0;



    public int StartingTokenCount 
    {
      get
      {
        switch ( Addressing )
        {
          case AddressingType.ABSOLUTE:
          case AddressingType.ABSOLUTE_X:
          case AddressingType.ABSOLUTE_Y:
          case AddressingType.IMMEDIATE:
          case AddressingType.IMMEDIATE_16:
          case AddressingType.IMPLICIT:
          case AddressingType.RELATIVE:
          case AddressingType.RELATIVE_16:
          case AddressingType.ZEROPAGE:
          case AddressingType.ZEROPAGE_RELATIVE:
            return 0;
          case AddressingType.ABSOLUTE_INDIRECT_X:
          case AddressingType.INDIRECT:
          case AddressingType.ZEROPAGE_INDIRECT:
          case AddressingType.ZEROPAGE_INDIRECT_SP_Y:
          case AddressingType.ZEROPAGE_INDIRECT_Z:
          case AddressingType.ZEROPAGE_X:
          case AddressingType.ZEROPAGE_Y:
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
        return 0;
      }
    }



    public Opcode( string Mnemonic, int ByteValue, AddressingType Addressing )
    {
      this.Mnemonic = Mnemonic;
      this.ByteValue = ByteValue;
      NumOperands = 0;
      this.Addressing = Addressing;
    }

    public Opcode( string Mnemonic, int ByteValue, AddressingType Addressing, int NumCycles, int PageBoundaryCycles )
    {
      this.Mnemonic = Mnemonic;
      this.ByteValue = ByteValue;
      NumOperands = 0;
      this.Addressing = Addressing;
      this.NumCycles = NumCycles;
      this.PageBoundaryCycles = PageBoundaryCycles;
      NumPenaltyCycles = PageBoundaryCycles;
    }

    public Opcode( string Mnemonic, int ByteValue, int NumOperands, AddressingType Addressing )
    {
      this.Mnemonic = Mnemonic;
      this.ByteValue = ByteValue;
      this.NumOperands = NumOperands;
      this.Addressing = Addressing;
    }

    public Opcode( string Mnemonic, int ByteValue, int NumOperands, AddressingType Addressing, int NumCycles, int PageBoundaryCycles )
    {
      this.Mnemonic = Mnemonic;
      this.ByteValue = ByteValue;
      this.NumOperands = NumOperands;
      this.Addressing = Addressing;
      this.NumCycles = NumCycles;
      this.PageBoundaryCycles = PageBoundaryCycles;
      NumPenaltyCycles = PageBoundaryCycles;
    }

    public Opcode( string Mnemonic, int ByteValue, int NumOperands, AddressingType Addressing, int NumCycles, int PageBoundaryCycles, int BranchSamePagePenalty, int BranchOtherPagePenalty )
    {
      this.Mnemonic = Mnemonic;
      this.ByteValue = ByteValue;
      this.NumOperands = NumOperands;
      this.Addressing = Addressing;
      this.NumCycles = NumCycles;
      this.PageBoundaryCycles = PageBoundaryCycles;
      this.BranchOtherPagePenalty = BranchOtherPagePenalty;
      this.BranchSamePagePenalty = BranchSamePagePenalty;
      NumPenaltyCycles = PageBoundaryCycles + BranchOtherPagePenalty + BranchSamePagePenalty;
    }
  }
}
