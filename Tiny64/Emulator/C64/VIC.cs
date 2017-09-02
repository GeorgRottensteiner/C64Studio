using System;
using System.Collections.Generic;
using System.Text;

namespace Tiny64
{
  public class VIC
  {
    byte[]        Registers = new byte[64];
    int           RasterLinePos = 0;
    int           IRQRasterLinePos = 0;



    public void Init()
    {
      Registers = new byte[64];
      RasterLinePos = 0;

      for ( int i = 47; i < 64; ++i )
      {
        Registers[i] = 0xff;
      }
    }



    public void WriteByte( byte Address, byte Value )
    {
      if ( Address >= 0x40 )
      {
        throw new NotImplementedException( "VIC only supports 0x40 registers!" );
      }

      if ( Address >= 47 )
      {
        // unused addresses can not be set
        return;
      }

      if ( Address == 0x12 )
      {
        // raster pos for IRQ (lower 8 bits)
        IRQRasterLinePos = ( IRQRasterLinePos & 0x100 ) | Value;
      }
      else
      {
        switch ( Address )
        {
          case 0x11:
            // msb is msb of IRQ raster pos
            IRQRasterLinePos = ( IRQRasterLinePos & 0xff );
            if ( ( Value & 0x80 ) != 0 )
            {
              IRQRasterLinePos |= 0x100;
            }
            break;
        }
        Registers[Address] = Value;
        //Debug.Log( "VIC 0xd0" + Address.ToString( "X2" ) + "=" + Value.ToString( "X2" ) );
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
      RasterLinePos = RasterPos;

      Registers[0x12] = (byte)( RasterPos & 0xff );
      Registers[0x11] = (byte)( ( Registers[0x11] & 0x7f ) + ( RasterPos >= 256 ? 0x80 : 0 ) );
    }



    public bool IsBadLine()
    {
      // bad line 

      // TODO - not if display is off
      // is bad line if lower 3 bits of $d011 match lower 3 bits of $d012
      return ( ( RasterLinePos & 0x07 ) == ( Registers[0x11] & 07 ) );
    }


  }
}
