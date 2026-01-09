using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RetroDevStudio
{
  public class Zone : SymbolInfo
  {
    public long   SizeInBytes = 0;



    public override string ToString()
    {
      return Name + $" ({SizeInBytes} bytes)";
    }
  }


}