using System;
using System.Collections.Generic;
using System.Text;

namespace RetroDevStudio
{
  public class DisassemblerSettings
  {
    public bool     AddLineAddresses = false;
    public bool     AddAssembledBytes = false;
    public bool     StopAtReturns = false;
    public bool     OnlyAddUsedLabels = false;
  }
}
