using System;
using System.Collections.Generic;
using System.Text;

namespace Tiny64
{
  public class VIC
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
        throw new NotImplementedException( "VIC only supports 0x40 registers!" );
      }
      if ( Address == 0x12 )
      {
        // raster pos for IRQ
      }
      else
      {
        Registers[Address] = Value;
        Debug.Log( "VIC 0xd0" + Address.ToString( "X2" ) + "=" + Value.ToString( "X2" ) );
      }
    }



    public byte ReadByte( byte Address )
    {
      if ( Address >= 0x40 )
      {
        throw new NotImplementedException( "VIC only supports 0x40 registers!" );
      }
      return Registers[Address];
    }



    internal void SetRasterPos( int RasterPos )
    {
      Registers[0x12] = (byte)( RasterPos & 0xff );
      Registers[0x11] = (byte)( ( Registers[0x11] & 0x7f ) + ( RasterPos >= 256 ? 0x80 : 0 ) );
    }



  }
}
