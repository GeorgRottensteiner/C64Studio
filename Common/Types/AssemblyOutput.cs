using System;
using System.Collections.Generic;
using System.Text;

namespace RetroDevStudio.Parser
{
  public class AssemblyOutput
  {
    public GR.Memory.ByteBuffer       Assembly = null;
    public int                        OriginalAssemblyStartAddress = -1;
    public int                        OriginalAssemblySize = 0;

    public Types.MemoryMap            MemoryMap = null;
  }
}
