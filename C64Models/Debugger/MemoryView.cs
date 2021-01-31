using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio.Debugger
{
  public class MemoryView
  {
    [Flags]
    public enum RAMFlag : byte
    {
      VALUE_KNOWN   = 1,
      VALUE_CHANGED = 2
    };

    public GR.Memory.ByteBuffer      RAM            = new GR.Memory.ByteBuffer( 65536 );
    public RAMFlag[]                 Flags       = new RAMFlag[65536];
  }

}
