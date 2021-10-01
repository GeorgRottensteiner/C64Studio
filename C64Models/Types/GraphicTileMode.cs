using System.ComponentModel;

namespace RetroDevStudio
{
  public enum GraphicTileMode
  {
    [Description( "Unknown" )]
    UNKNOWN = -1,
    [Description( "Commodore HiRes" )]
    COMMODORE_HIRES = 0,
    [Description( "Commodore Multicolor" )]
    COMMODORE_MULTICOLOR,
    [Description( "Commodore ECM" )]
    COMMODORE_ECM,
    [Description( "Mega65 Full Color Mode 16 colors" )]
    MEGA65_FCM_16_COLORS,
    [Description( "Mega65 Full Color Mode 256 colors" )]
    MEGA65_FCM_256_COLORS
  }

}
