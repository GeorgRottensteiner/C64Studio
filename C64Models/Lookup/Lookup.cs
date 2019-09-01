using C64Studio.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio.Lookup
{
  public static class Machines
  {
    static Machine C64 = new Machine() { Type = MachineType.C64, InitialBreakpointAddress = 0xE178, InitialBreakpointAddressCartridge = 0x8000 };
  }
}
