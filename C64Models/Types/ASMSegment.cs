using System;
using System.Collections.Generic;
using System.Text;

namespace RetroDevStudio.Types
{
  public class ASMSegment
  {
    public int        StartAddress = -1;

    public int        GlobalLineIndex = -1;
    public int        LocalLineIndex = -1;
    public string     Filename = "";
    public GR.Memory.ByteBuffer     Data = new GR.Memory.ByteBuffer();



    public int Length
    {
      get
      {
        if ( Data == null )
        {
          return 0;
        }
        return (int)Data.Length;
      }
    }



    public bool Overlaps( ASMSegment OtherSegment )
    {
      if ( ( Data == null )
      ||   ( OtherSegment.Data == null ) )
      {
        return false;
      }
      if ( ( Length == 0 )
      ||   ( OtherSegment.Length == 0 ) )
      {
        return false;
      }
      return ( ( StartAddress + Length > OtherSegment.StartAddress )
      &&       ( StartAddress < OtherSegment.StartAddress + OtherSegment.Length ) );
    }

  }
}
