using System;
using System.Collections.Generic;
using System.Text;

namespace Tiny64
{
  public class SID
  {
    byte[]        Registers = new byte[64];


    public void Init()
    {
      Registers = new byte[64];
    }



    public void WriteByte( byte Address, byte Value )
    {
      if ( Address >= 0x40 )
      {
        throw new NotImplementedException( "SID only supports 0x40 registers!" );
      }
      Registers[Address] = Value;
    }



    public byte ReadByte( byte Address )
    {
      if ( Address >= 0x40 )
      {
        throw new NotImplementedException( "SID only supports 0x40 registers!" );
      }
      return Registers[Address];
    }



  }
}
