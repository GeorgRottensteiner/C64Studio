using System;
using System.Collections.Generic;
using System.Text;

namespace Tiny64
{
  public class CIA1 : CIA
  {
    public byte     Joystick1 = 0;
    public byte     Joystick2 = 0;

    public byte[]   KeyMatrix = new byte[8];
    public  byte[]  RevMatrix = new byte[8];



    public CIA1( Machine Machine ) : base( Machine )
    {
      Reset();
    }



    public override void Reset()
    {
      base.Reset();

      // Clear keyboard matrix and joystick states
      for ( int i = 0; i < 8; i++ )
      {
        KeyMatrix[i] = 0xff;
        RevMatrix[i] = 0xff;
      }

      Joystick1 = 0xff;
      Joystick2 = 0xff;
      //prev_lp = 0x10;
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
          break;
        case Register.PRB_DATA_PORT_B:
          Registers[Address] = Value;
          // TODO - check lightpen
          break;
        case Register.DDRA_DATA_DIR_PORT_A:
          Registers[Address] = Value;
          break;
        case Register.DDRB_DATA_DIR_PORT_B:
          Registers[Address] = Value;
          // TODO - check lightpen
          break;
        default:
          Debug.Log( "Unsupported CIA1 register write " + Address + " = " + Value );
          break;
      }
    }



    public new byte ReadByte( byte Address )
    {
      Address &= 0x0f;

      switch ( Address )
      {
        case Register.PRA_DATA_PORT_A:
          {
            byte ret = (byte)( Registers[Register.PRA_DATA_PORT_A] | ~Registers[Register.DDRA_DATA_DIR_PORT_A] );
            byte tst = (byte)( ( Registers[Register.PRB_DATA_PORT_B] | ~Registers[Register.DDRB_DATA_DIR_PORT_B] ) & Joystick1 );

            // AND all active columns
            if ( !( ( tst & 0x01 ) > 0 ) )
            {
              ret &= RevMatrix[0];
            }
            if ( !( ( tst & 0x02 ) > 0 ) )
            {
              ret &= RevMatrix[1];
            }
            if ( !( ( tst & 0x04 ) > 0 ) )
            {
              ret &= RevMatrix[2];
            }
            if ( !( ( tst & 0x08 ) > 0 ) )
            {
              ret &= RevMatrix[3];
            }
            if ( !( ( tst & 0x10 ) > 0 ) )
            {
              ret &= RevMatrix[4];
            }
            if ( !( ( tst & 0x20 ) > 0 ) )
            {
              ret &= RevMatrix[5];
            }
            if ( !( ( tst & 0x40 ) > 0 ) )
            {
              ret &= RevMatrix[6];
            }
            if ( !( ( tst & 0x80 ) > 0 ) )
            {
              ret &= RevMatrix[7];
            }
            Debug.Log( "Read PRA" );
            return (byte)( ret & Joystick2 );
          }
        case Register.PRB_DATA_PORT_B:
          {
            byte ret = (byte)( ~Registers[Register.DDRB_DATA_DIR_PORT_B] );
            byte tst = (byte)( ( Registers[Register.PRA_DATA_PORT_A] | ~Registers[Register.DDRA_DATA_DIR_PORT_A] ) & Joystick2 );

            if ( !( ( tst & 0x01 ) > 0 ) )
            {
              ret &= KeyMatrix[0];  // AND all active rows
            }
            if ( !( ( tst & 0x02 ) > 0 ) )
            {
              ret &= KeyMatrix[1];
            }
            if ( !( ( tst & 0x04 ) > 0 ) )
            {
              ret &= KeyMatrix[2];
            }
            if ( !( ( tst & 0x08 ) > 0 ) )
            {
              ret &= KeyMatrix[3];
            }
            if ( !( ( tst & 0x10 ) > 0 ) )
            {
              ret &= KeyMatrix[4];
            }
            if ( !( ( tst & 0x20 ) > 0 ) )
            {
              ret &= KeyMatrix[5];
            }
            if ( !( ( tst & 0x40 ) > 0 ) )
            {
              ret &= KeyMatrix[6];
            }
            if ( !( ( tst & 0x80 ) > 0 ) )
            {
              ret &= KeyMatrix[7];
            }
            //Debug.Log( "Read PRB " + ret );
            return (byte)( ( ret | ( Registers[Register.PRB_DATA_PORT_B] & Registers[Register.DDRB_DATA_DIR_PORT_B] ) ) & Joystick1 );
          }
          /*
        default:
          Debug.Log( "Unsupported CIA1 register read " + Address );
          return base.ReadByte( Address );*/
      }

      return base.ReadByte( Address );
    }



  }
}
