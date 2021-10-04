namespace RetroDevStudio
{
  public static partial class Machines
  {
    public static Machine VIC20 = new Machine()
    {
      Type                              = MachineType.VIC20,
      BASICDefaultStartAddress          = 0x1001,   // default memory
      InitialBreakpointAddress          = 0xFD3C,
      InitialBreakpointAddressCartridge = 0xA000,

      TextModes = new System.Collections.Generic.List<TextMode> {
                    TextMode.COMMODORE_40_X_25_HIRES,
                    TextMode.COMMODORE_40_X_25_MULTICOLOR
                  },

      GraphicModes = new System.Collections.Generic.List<GraphicMode> {
                     }
    };



  }
}
