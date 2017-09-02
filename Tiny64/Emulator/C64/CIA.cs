using System;
using System.Collections.Generic;
using System.Text;

namespace Tiny64
{
  public class CIA
  {
    byte[]        Registers = new byte[16];


    public void Init()
    {
      Registers = new byte[16];
    }



    public void WriteByte( byte Address, byte Value )
    {
      if ( Address >= 16 )
      {
        throw new NotImplementedException( "CIA only supports 16 registers!" );
      }
      Registers[Address] = Value;
    }



    public byte ReadByte( byte Address )
    {
      if ( Address >= 16 )
      {
        throw new NotImplementedException( "CIA only supports 16 registers!" );
      }
      return Registers[Address];
    }



  }
}
