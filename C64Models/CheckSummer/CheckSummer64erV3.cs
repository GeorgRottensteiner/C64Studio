using GR.Memory;
using System.ComponentModel;


namespace RetroDevStudio.CheckSummer
{
  [Description( "64er V3" )]
  public class CheckSummer64erV3 : ICheckSummer
  {
    public int CheckSum( ByteBuffer data )
    {
      int checkSum = 0;
      int x = 0;

      for ( uint i = 0; i < data.Length; ++i )
      {
        var b = data.ByteAt( (int)i );
        if ( b == 0x20 )
        {
          continue;
        }
        x &= 7;
        b = (byte)( ( ( b << x ) & 0xff ) | ( b >> ( 8 - x ) ) );
        ++x;

        checkSum = ( b + checkSum ) & 0xff;
      }
      return checkSum;
    }



  }



}