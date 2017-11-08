using System;
using System.Collections.Generic;
using System.Text;



namespace Tiny64
{
  public class Disassembler
  {
    public static string DisassembleMnemonicToString( Tiny64.Opcode Opcode, Tiny64.Memory Memory, int CodePos, GR.Collections.Set<ushort> AccessedAddresses, GR.Collections.Map<int, string> NamedLabels )
    {
      string output = Opcode.Mnemonic.ToLower();

      ushort targetAddress = 0;
      bool    twoBytes = true;

      switch ( Opcode.Addressing )
      {
        case Tiny64.Opcode.AddressingType.IMPLICIT:
          break;
        case Tiny64.Opcode.AddressingType.ABSOLUTE:
          targetAddress = Memory.ReadWordDirect( (ushort)( CodePos + 1 ) );
          break;
        case Tiny64.Opcode.AddressingType.ABSOLUTE_X:
          targetAddress = Memory.ReadWordDirect( (ushort)( CodePos + 1 ) );
          break;
        case Tiny64.Opcode.AddressingType.ABSOLUTE_Y:
          targetAddress = Memory.ReadWordDirect( (ushort)( CodePos + 1 ) );
          break;
        case Tiny64.Opcode.AddressingType.IMMEDIATE:
          targetAddress = Memory.ReadByteDirect( (ushort)( CodePos + 1 ) );
          twoBytes = false;
          break;
        case Tiny64.Opcode.AddressingType.INDIRECT:
          targetAddress = Memory.ReadWordDirect( (ushort)( CodePos + 1 ) );
          break;
        case Tiny64.Opcode.AddressingType.INDIRECT_X:
          targetAddress = Memory.ReadByteDirect( (ushort)( CodePos + 1 ) );
          twoBytes = false;
          break;
        case Tiny64.Opcode.AddressingType.INDIRECT_Y:
          targetAddress = Memory.ReadByteDirect( (ushort)( CodePos + 1 ) );
          twoBytes = false;
          break;
        case Tiny64.Opcode.AddressingType.RELATIVE:
          {
            // int delta = value - lineInfo.AddressStart - 2;
            sbyte relValue = (sbyte)Memory.ReadByteDirect( (ushort)( CodePos + 1 ) );

            targetAddress = (ushort)( relValue + 2 + CodePos );
          }
          break;
        case Tiny64.Opcode.AddressingType.ZEROPAGE:
          targetAddress = Memory.ReadByteDirect( (ushort)( CodePos + 1 ) );
          twoBytes = false;
          break;
        case Tiny64.Opcode.AddressingType.ZEROPAGE_X:
          targetAddress = Memory.ReadByteDirect( (ushort)( CodePos + 1 ) );
          twoBytes = false;
          break;
        case Tiny64.Opcode.AddressingType.ZEROPAGE_Y:
          targetAddress = Memory.ReadByteDirect( (ushort)( CodePos + 1 ) );
          twoBytes = false;
          break;
      }
      string    addressPlacement;

      if ( twoBytes )
      {
        addressPlacement = "$" + targetAddress.ToString( "x4" );
      }
      else
      {
        addressPlacement = "$" + targetAddress.ToString( "x2" );
      }

      if ( AccessedAddresses.ContainsValue( targetAddress ) )
      {
        addressPlacement = "label_" + targetAddress.ToString( "x4" );
      }

      if ( NamedLabels.ContainsKey( targetAddress ) )
      {
        addressPlacement = NamedLabels[targetAddress];
      }

      switch ( Opcode.Addressing )
      {
        case Tiny64.Opcode.AddressingType.IMPLICIT:
          break;
        case Tiny64.Opcode.AddressingType.ABSOLUTE:
          output += " " + addressPlacement;
          break;
        case Tiny64.Opcode.AddressingType.ABSOLUTE_X:
          output += " " + addressPlacement + ", x";
          break;
        case Tiny64.Opcode.AddressingType.ABSOLUTE_Y:
          output += " " + addressPlacement + ", y";
          break;
        case Tiny64.Opcode.AddressingType.IMMEDIATE:
          output += " #" + addressPlacement;
          break;
        case Tiny64.Opcode.AddressingType.INDIRECT:
          output += " ( " + addressPlacement + " )";
          break;
        case Tiny64.Opcode.AddressingType.INDIRECT_X:
          output += " ( " + addressPlacement + ", x)";
          break;
        case Tiny64.Opcode.AddressingType.INDIRECT_Y:
          output += " ( " + addressPlacement + " ), y";
          break;
        case Tiny64.Opcode.AddressingType.RELATIVE:
          {
            // int delta = value - lineInfo.AddressStart - 2;

            output += " " + addressPlacement;
            //output += " (" + delta.ToString( "X2" ) + ")";
          }
          break;
        case Tiny64.Opcode.AddressingType.ZEROPAGE:
          output += " " + addressPlacement;
          break;
        case Tiny64.Opcode.AddressingType.ZEROPAGE_X:
          output += " " + addressPlacement + ", x";
          break;
        case Tiny64.Opcode.AddressingType.ZEROPAGE_Y:
          output += " " + addressPlacement + ", y";
          break;
      }
      return output;
    }



  }
}
