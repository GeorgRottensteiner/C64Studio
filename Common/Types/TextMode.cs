using System;
using System.ComponentModel;

namespace RetroDevStudio
{
  public enum TextMode
  {
    UNKNOWN = -1,
    [Description( "Commodore 40x25 HiRes" )]
    [CharsetLayout( Types.CharlistLayout.PETSCII_EDITOR )]
    COMMODORE_40_X_25_HIRES = 0,
    [Description( "Commodore 40x25 Multicolor" )]
    [CharsetLayout( Types.CharlistLayout.PETSCII_EDITOR )]
    COMMODORE_40_X_25_MULTICOLOR,
    [Description( "Commodore 40x25 ECM" )]
    [CharsetLayout( Types.CharlistLayout.PETSCII_EDITOR )]
    COMMODORE_40_X_25_ECM,
    [Description( "Mega 65 80x25 HiRes" )]
    [CharsetLayout( Types.CharlistLayout.PETSCII_EDITOR )]
    MEGA65_80_X_25_HIRES,
    [Description( "Mega 65 80x25 Multicolor" )]
    [CharsetLayout( Types.CharlistLayout.PETSCII_EDITOR )]
    MEGA65_80_X_25_MULTICOLOR,
    [Description( "Mega 65 80x25 ECM" )]
    [CharsetLayout( Types.CharlistLayout.PETSCII_EDITOR )]
    MEGA65_80_X_25_ECM,
    [Description( "Mega 65 40x25 FCM" )]
    [CharsetLayout( Types.CharlistLayout.PETSCII_EDITOR )]
    MEGA65_40_X_25_FCM,
    [Description( "Mega 65 40x25 FCM 16bit Chars" )]
    MEGA65_40_X_25_FCM_16BIT,
    [Description( "Mega 65 80x25 FCM" )]
    [CharsetLayout( Types.CharlistLayout.PETSCII_EDITOR )]
    MEGA65_80_X_25_FCM,
    [Description( "Mega 65 80x25 FCM 16bit Chars" )]
    MEGA65_80_X_25_FCM_16BIT,
    [Description( "Commodore VIC20" )]
    [CharsetLayout( Types.CharlistLayout.PETSCII_EDITOR )]
    COMMODORE_VIC20_8_X_8,
    [Description( "Mega 65 40x25 ECM" )]
    [CharsetLayout( Types.CharlistLayout.PETSCII_EDITOR )]
    MEGA65_40_X_25_ECM,
    [Description( "Mega 65 40x25 HiRes" )]
    [CharsetLayout( Types.CharlistLayout.PETSCII_EDITOR )]
    MEGA65_40_X_25_HIRES,
    [Description( "Mega 65 40x25 Multicolor" )]
    [CharsetLayout( Types.CharlistLayout.PETSCII_EDITOR )]
    MEGA65_40_X_25_MULTICOLOR,
    [Description( "Mega 65 40x25 NCM" )]
    MEGA65_40_X_25_NCM,
    [Description( "Mega 65 80x25 NCM" )]
    MEGA65_80_X_25_NCM,
    [Description( "Commander X16 80x60" )]
    [CharsetLayout( Types.CharlistLayout.PETSCII_EDITOR )]
    X16_80_X_60,
    [Description( "Commander X16 80x30" )]
    [CharsetLayout( Types.CharlistLayout.PETSCII_EDITOR )]
    X16_80_X_30,
    [Description( "Commander X16 40x60" )]
    [CharsetLayout( Types.CharlistLayout.PETSCII_EDITOR )]
    X16_40_X_60,
    [Description( "Commander X16 40x30" )]
    [CharsetLayout( Types.CharlistLayout.PETSCII_EDITOR )]
    X16_40_X_30,
    [Description( "Commander X16 40x15" )]
    [CharsetLayout( Types.CharlistLayout.PETSCII_EDITOR )]
    X16_40_X_15,
    [Description( "Commander X16 20x30" )]
    [CharsetLayout( Types.CharlistLayout.PETSCII_EDITOR )]
    X16_20_X_30,
    [Description( "Commander X16 20x15" )]
    [CharsetLayout( Types.CharlistLayout.PETSCII_EDITOR )]
    X16_20_X_15,
    [Description( "Commodore C128 80x25 HiRes" )]
    [CharsetLayout( Types.CharlistLayout.PETSCII_EDITOR )]
    COMMODORE_128_VDC_80_X_25_HIRES,
    [Description( "NES 32x30" )]
    NES,
    [Description( "Commodore VIC20 8x16" )]
    COMMODORE_VIC20_8_X_16
  }

}