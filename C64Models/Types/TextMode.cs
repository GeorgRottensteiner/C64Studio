using System;
using System.ComponentModel;

namespace RetroDevStudio
{
  public enum TextMode
  {
    UNKNOWN = -1,
    [Description( "Commodore 40x25 HiRes" )]
    COMMODORE_40_X_25_HIRES = 0,
    [Description( "Commodore 40x25 Multicolor" )]
    COMMODORE_40_X_25_MULTICOLOR,
    [Description( "Commodore 40x25 ECM" )]
    COMMODORE_40_X_25_ECM,
    [Description( "Mega 65 80x25 HiRes" )]
    MEGA65_80_X_25_HIRES,
    [Description( "Mega 65 80x25 Multicolor" )]
    MEGA65_80_X_25_MULTICOLOR,
    [Description( "Mega 65 80x25 ECM" )]
    MEGA65_80_X_25_ECM,
    [Description( "Mega 65 40x25 FCM" )]
    MEGA65_40_X_25_FCM,
    [Description( "Mega 65 40x25 FCM 16bit Chars" )]
    MEGA65_40_X_25_FCM_16BIT,
    [Description( "Mega 65 80x25 FCM" )]
    MEGA65_80_X_25_FCM,
    [Description( "Mega 65 80x25 FCM 16bit Chars" )]
    MEGA65_80_X_25_FCM_16BIT,
    [Description( "Commodore VC20 22x23" )]
    COMMODORE_VC20_22_X_23
  }

}