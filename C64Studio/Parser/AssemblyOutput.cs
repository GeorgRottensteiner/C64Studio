using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio.Parser
{
  public class AssemblyOutput
  {
    public GR.Memory.ByteBuffer       Assembly = null;
    public int                        OriginalAssemblyStartAddress = -1;
    public int                        OriginalAssemblySize = 0;
  }
}
