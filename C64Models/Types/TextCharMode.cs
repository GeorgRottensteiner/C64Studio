using System.ComponentModel;

namespace RetroDevStudio
{
  public enum TextCharMode
  {
    [Description( "Unknown" )]
    UNKNOWN = -1,
    [Description( "Commodore HiRes" )]
    COMMODORE_HIRES = 0,
    [Description( "Commodore Multicolor" )]
    COMMODORE_MULTICOLOR,
    [Description( "Commodore ECM" )]
    COMMODORE_ECM,
    [Description( "Mega65 Full Color Mode" )]
    MEGA65_FCM,
    [Description( "Mega65 Full Color Mode 16bit" )]
    MEGA65_FCM_16BIT,
    [Description( "VIC20 Text Mode" )]
    VIC20,
    [Description( "Mega65 HiRes (32 colors)" )]
    MEGA65_HIRES,
    [Description( "Mega65 ECM (32 colors)" )]
    MEGA65_ECM,
    [Description( "Mega65 NCM (16 colors, 16bit chars)" )]
    MEGA65_NCM
  }

}
