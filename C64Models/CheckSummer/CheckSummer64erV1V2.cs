using GR.Memory;
using System.ComponentModel;


namespace RetroDevStudio.CheckSummer
{
  [Description( "64er V1/V2" )]
  public class CheckSummer64erV1V2 : ICheckSummer
  {
    public string CheckSum( ByteBuffer data )
    {
      int checkSum = 0;

      for ( uint i = 0; i < data.Length; ++i )
      {
        var b = data.ByteAt( (int)i );
        if ( b == 0x20 )
        {
          continue;
        }
        checkSum = ( checkSum + b ) & 0xff;
      }
      return checkSum.ToString( "D3" );
    }



  }



}