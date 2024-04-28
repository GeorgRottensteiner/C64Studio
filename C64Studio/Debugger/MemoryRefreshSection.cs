using GR.Memory;
using RetroDevStudio;
using System;
using System.Collections.Generic;



namespace RetroDevStudio
{
  public class MemoryRefreshSection
  {
    public int          StartAddress = 0;
    public int          Size = 32;
    public MemorySource Source = MemorySource.AS_CPU;
  }


}
