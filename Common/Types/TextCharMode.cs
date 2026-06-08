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
    MEGA65_NCM,
    [Description( "Commander X16 HiRes" )]
    X16_HIRES,
    [Description( "C128 VDC HiRes" )]
    COMMODORE_128_VDC_HIRES,
    [Description( "NES" )]
    NES,
    [Description( "VIC20 8x16 Text Mode" )]
    VIC20_8X16
  }

}
