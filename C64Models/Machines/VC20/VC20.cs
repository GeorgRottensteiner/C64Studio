namespace RetroDevStudioModels
{
  public static partial class Machines
  {
    public static Machine VC20 = new Machine()
    {
      Type                              = MachineType.VC20,
      BASICDefaultStartAddress          = 0x1001,   // default memory
      InitialBreakpointAddress          = 0xFD3C,
      InitialBreakpointAddressCartridge = 0xA000,

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
