using System;
using System.Collections.Generic;
using System.Text;

namespace RetroDevStudioModels
{
  public static partial class Machines
  {
    public static Machine VC20 = new Machine() 
    { 
      Type                              = MachineType.VC20, 
      BASICDefaultStartAddress          = 0x1001,   // default memory
      InitialBreakpointAddress          = 0xFD3C, 
      InitialBreakpointAddressCartridge = 0xA000 
    };

    public static Machine C128 = new Machine() 
    { 
      Type                              = MachineType.C128, 
      BASICDefaultStartAddress          = 0x1C01,
      InitialBreakpointAddress          = 0xA871, 
      InitialBreakpointAddressCartridge = 0x8000 
    };
  }
}
