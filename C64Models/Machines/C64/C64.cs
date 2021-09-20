namespace RetroDevStudioModels
{
  public static partial class Machines
  {
    //public static Machine C64 = new Machine() { Type = MachineType.C64, InitialBreakpointAddress = 0xE178, InitialBreakpointAddressCartridge = 0x8000 };

    public static Machine C64 = new Machine()
    {
      Type                              = MachineType.C64,
      BASICDefaultStartAddress          = 0x0801,
      InitialBreakpointAddress          = 0xA871,
      InitialBreakpointAddressCartridge = 0x8000,

      TextModes = new System.Collections.Generic.List<TextMode> {
                    TextMode.COMMODORE_40_X_25_HIRES,
                    TextMode.COMMODORE_40_X_25_MULTICOLOR,
                    TextMode.COMMODORE_40_X_25_ECM
                  },

      GraphicModes = new System.Collections.Generic.List<GraphicMode> {
                       GraphicMode.COMMODORE_320_X_200_HIRES,
                       GraphicMode.COMMODORE_320_X_200_MULTICOLOR
                     }
    };



  }
}
