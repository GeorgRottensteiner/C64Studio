namespace RetroDevStudio
{
  public static partial class Machines
  {
    public static Machine C128 = new Machine()
    {
      Type                              = MachineType.C128,
      BASICDefaultStartAddress          = 0x1C01,
      InitialBreakpointAddress          = 0x5a9b, // 0xAF7B, //0x4710, //0xA871,
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
