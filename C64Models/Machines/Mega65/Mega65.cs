using System.Collections.Generic;

namespace RetroDevStudio
{
  public static partial class Machines
  {
    public static Machine Mega65 = new Machine()
    {
      Type                              = MachineType.MEGA65,
      BASICDefaultStartAddress          = 0x2001,
      InitialBreakpointAddress          = 0xA871,
      InitialBreakpointAddressCartridge = 0x8000,

      TextModes = new List<TextMode> {
                    TextMode.COMMODORE_40_X_25_HIRES,
                    TextMode.COMMODORE_40_X_25_MULTICOLOR,
                    TextMode.COMMODORE_40_X_25_ECM,
                    TextMode.MEGA65_80_X_25_HIRES,
                    TextMode.MEGA65_80_X_25_MULTICOLOR
                  },

      GraphicModes = new List<GraphicMode> {
                       GraphicMode.COMMODORE_320_X_200_HIRES,
                       GraphicMode.COMMODORE_320_X_200_MULTICOLOR
                     }
    };



  }
}
