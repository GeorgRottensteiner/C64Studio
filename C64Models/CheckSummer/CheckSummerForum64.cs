using GR.Memory;
using System.ComponentModel;


namespace RetroDevStudio.CheckSummer
{
  [Description( "Forum 64 Summer" )]
  public class CheckSummerForum64 : ICheckSummer
  {
    public int CheckSum( ByteBuffer data )
    {
      ushort checkSum = data.UInt16At( 0 );
      bool insideQuotes = false;

      checkSum = (ushort)( checkSum ^ 0xffff );

      for ( uint i = 2; i < data.Length; ++i )
      {
        var b = data.ByteAt( (int)i );
        var origByte = b;
        if ( !insideQuotes )
        {
          // skip spaces outside of quotes
          if ( b == 0x20 )
          {
            continue;
          }
          // is REM?
          if ( b == ':' )
          {
            if ( ( ( i + 1 ) < data.Length ) 
            &&   ( data.ByteAt( (int)i + 1 ) == 0x8f ) )
            {
              // REM is not checksummed
              break;
            }
            if ( ( ( i + 1 ) < data.Length )
            &&   ( data.ByteAt( (int)i + 1 ) == 0x20 ) )
            {
              // space after colon
            }
            else
            {
              // multiple adjacent colons are ignored and treated as a single one
              while ( ( ( i + 1 ) < data.Length )
              &&      ( ( data.ByteAt( (int)i + 1 ) ) == ':' ) )
              {
                ++i;
              }
            }
          }
        }
        if ( b == 34 )
        {
          insideQuotes = !insideQuotes;
        }

        // couldn't quite make out what the code does, so manually recreated from assembly
        byte lfsr = (byte)( checkSum & 0xff );
        byte tmp = origByte;
        byte lfsr1 = (byte)( checkSum >> 8 );
        int y = 8;
        byte a = b;
        while ( y > 0 )
        {
          // asl tmp
          bool carrySet = ( tmp & 0x80 ) != 0;
          tmp <<= 1; 

          // rol
          a <<= 1;
          if ( carrySet )
          {
            a |= 1;
          }

          // eor lfsr
          a ^= lfsr;

          // lsr
          carrySet = ( ( a & 1 ) != 0 );
          a >>= 1;

          // bcc label_0398
          if ( carrySet )
          {
            // lda #$68
            lfsr1 ^= 0x68;
          }
          bool carrySet2 = ( ( lfsr1 & 1 ) != 0 );
          lfsr1 >>= 1;
          if ( carrySet )
          {
            lfsr1 |= 0x80;
          }
          carrySet = carrySet2;

          carrySet2 = ( ( lfsr & 1 ) != 0 );
          lfsr >>= 1;
          if ( carrySet )
          {
            lfsr |= 0x80;
          }
          carrySet = carrySet2;
          --y;
        }
        checkSum = (ushort)( ( lfsr1 << 8 ) | lfsr );
      }
      return checkSum;
    }



  }



}