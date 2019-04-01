using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio.Debugger
{
  public class MemoryView
  {
    public GR.Memory.ByteBuffer      RAM = new GR.Memory.ByteBuffer( 65536 );
    public bool[]                    RAMChanged = new bool[65536];

    // offset,length
    public Dictionary<int,int>       ValidMemory = new Dictionary<int, int>();
  }

}
