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
    [Description( "Commodore VIC20 22x23" )]
    COMMODORE_VIC20_22_X_23,
    [Description( "Mega 65 40x25 ECM" )]
    MEGA65_40_X_25_ECM,
    [Description( "Mega 65 40x25 HiRes" )]
    MEGA65_40_X_25_HIRES,
    [Description( "Mega 65 40x25 Multicolor" )]
    MEGA65_40_X_25_MULTICOLOR,
    [Description( "Mega 65 40x25 NCM" )]
    MEGA65_40_X_25_NCM,
    [Description( "Mega 65 80x25 NCM" )]
    MEGA65_80_X_25_NCM,
    [Description( "Commander X16 80x60" )]
    X16_80_X_60,
    [Description( "Commander X16 80x30" )]
    X16_80_X_30,
    [Description( "Commander X16 40x60" )]
    X16_40_X_60,
    [Description( "Commander X16 40x30" )]
    X16_40_X_30,
    [Description( "Commander X16 40x15" )]
    X16_40_X_15,
    [Description( "Commander X16 20x30" )]
    X16_20_X_30,
    [Description( "Commander X16 20x15" )]
    X16_20_X_15,
    [Description( "Commodore C128 80x25 HiRes" )]
    COMMODORE_128_VDC_80_X_25_HIRES,
  }

}