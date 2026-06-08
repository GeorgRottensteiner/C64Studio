using System;
using System.Collections.Generic;
using System.Text;

namespace Tiny64
{
  public class CIA2 : CIA
  {
    public CIA2( Machine Machine ) : base( Machine )
    {
    }



    public new void WriteByte( byte Address, byte Value )
    {
      Address &= 0x0f;

      if ( base.WriteByte( Address, Value ) )
      {
        return;
      }

      switch ( Address )
      {
        case Register.PRA_DATA_PORT_A:
          Registers[Address] = Value;
          /*
          the_vic.ChangedVA( (UInt16)( ~( pra | ~ddra ) & 3 ) );
          byte old_lines = IECLines;
          IECLines = (byte)( ( ~abyte << 2 ) & 0x80 // DATA
              | ( ~abyte << 2 ) & 0x40    // CLK
              | ( ~abyte << 1 ) & 0x10 );   // ATN

          if ( ( ( IECLines ^ old_lines ) & 0x10 ) > 0 )
          { // ATN changed
            the_cpu_1541.NewATNState();
            if ( ( old_lines & 0x10 ) > 0 )       // ATN 1->0
              the_cpu_1541.IECInterrupt();
          }*/
          break;
        case Register.PRB_DATA_PORT_B:
          Registers[Address] = Value;
          break;
        case Register.DDRA_DATA_DIR_PORT_A:
          Registers[Address] = Value;
          // the_vic.ChangedVA((UInt16)(~(pra | ~ddra) & 3));
          break;
        case Register.DDRB_DATA_DIR_PORT_B:
          Registers[Address] = Value;
          break;
        default:
          Debug.Log( "Unsupported CIA2 register write " + Address + " = " + Value );
          break;
      }
    }



    public new byte ReadByte( byte Address )
    {
      Address &= 0x0f;

      switch ( Address )
      {
        case Register.PRA_DATA_PORT_A:
          //return (byte)( ( Registers[Address] | ~Registers[Address] ) & 0x3f | IECLines & the_cpu_1541.IECLines );
          return (byte)( ( Registers[Address] | ~Registers[Address] ) & 0x3f );
          /*
        default:
          Debug.Log( "Unsupported CIA2 register read " + Address );
          return base.ReadByte( Address );*/
      }

      return base.ReadByte( Address );
    }



  }
}
