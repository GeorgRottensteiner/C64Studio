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
      IMPLICIT,
      IMMEDIATE,
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

    public string Mnemonic = "";
    public int ByteValue = -1;
    public int NumOperands = -1;
    public AddressingType Addressing = AddressingType.UNKNOWN;
    public int NumCycles = 0;
    public int NumPenaltyCycles = 0;
    public int PageBoundaryCycles = 0;
    public int BranchSamePagePenalty = 0;
    public int BranchOtherPagePenalty = 0;

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
