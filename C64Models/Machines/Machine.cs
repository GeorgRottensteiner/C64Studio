using System;
using System.Collections.Generic;
using System.Text;

namespace RetroDevStudio
{
  public class Machine
  {
    public MachineType        Type;
    public int                BASICDefaultStartAddress;
    public int                InitialBreakpointAddress;           // common initial breakpoint address before jumping to program start (inside Kernal)
    public int                InitialBreakpointAddressCartridge;  // initial breakpoint address before jumping to cartridge start
    public List<TextMode>     TextModes;
    public List<GraphicMode>  GraphicModes;



    internal static Machine FromType( MachineType Type )
    {
      switch ( Type )
      {
        case MachineType.C64:
          return Machines.C64;
        case MachineType.VC20:
          return Machines.VC20;
        case MachineType.C128:
          return Machines.C128;
        case MachineType.MEGA65:
          return Machines.Mega65;
        default:
          // fallback to C64
          return Machines.C64;
      }
    }
  }
}
