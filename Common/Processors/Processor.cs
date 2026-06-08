using System;
using System.Collections.Generic;
using System.Text;
using static Tiny64.Opcode;

namespace Tiny64
{
  public partial class Processor
  {
    public string         Name = "";
    public Dictionary<string, List<Opcode>>           Opcodes = new Dictionary<string, List<Opcode>>();
    public Dictionary<byte, Opcode>                   OpcodeByValue = new Dictionary<byte, Opcode>();

    public bool           LittleEndian = true;



    public Processor( string Name )
    {
      this.Name = Name;
    }



    public Opcode AddOpcode( string Opcode, ulong ByteValue, int NumOperands, AddressingType Addressing, int NumCycles )
    {
      return AddOpcode( Opcode, ByteValue, NumOperands, Addressing, NumCycles, 0, 0, 0 );
    }



    public Opcode AddOpcode( string Opcode, ulong ByteValue, int NumOperands, AddressingType Addressing, int NumCycles, int PageBoundaryCycles )
    {
      return AddOpcode( Opcode, ByteValue, NumOperands, Addressing, NumCycles, PageBoundaryCycles, 0, 0 );
    }



    public Opcode AddOpcode( string Opcode, ulong ByteValue, int NumOperands, AddressingType Addressing, int NumCycles, int PageBoundaryCycles, int BranchSamePagePenalty, int BranchOtherPagePenalty )
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



    public void AddOpcodeForDisassembly( string Opcode, uint ByteValue, int NumOperands, AddressingType Addressing )
    {
      Opcode opcode = new Opcode( Opcode, ByteValue, NumOperands, Addressing );
      OpcodeByValue.Add( (byte)ByteValue, opcode );
    }



  }
}
