using System;
using System.ComponentModel;

namespace RetroDevStudioModels
{
  public enum TextMode
  {
    [Description( "Unknown" )]
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



  static class TextCharModeUtil
  {
    public static int NumberOfColorsInCharacter( TextCharMode Mode )
    {
      switch ( Mode )
      {
        case TextCharMode.COMMODORE_ECM:
        case TextCharMode.COMMODORE_HIRES:
        case TextCharMode.COMMODORE_MULTICOLOR:
        case TextCharMode.VC20:
          return 16;
        case TextCharMode.MEGA65_FCM:
        case TextCharMode.MEGA65_FCM_16BIT:
          return 256;
        default:
          Debug.Log( "NumberOfColorsInCharacter unsupported Mode " + Mode );
          return 16;
      }
    }



    public static int NumBytesOfSingleCharacter( TextCharMode Mode )
    {
      switch ( Mode )
      {
        case TextCharMode.COMMODORE_ECM:
        case TextCharMode.COMMODORE_HIRES:
        case TextCharMode.COMMODORE_MULTICOLOR:
        case TextCharMode.VC20:
          return 8;
        case TextCharMode.MEGA65_FCM:
        case TextCharMode.MEGA65_FCM_16BIT:
          return 64;
        default:
          Debug.Log( "NumBytesOfSingleCharacter unsupported Mode " + Mode );
          return 8;
      }
    }



    public static TextCharMode FromTextMode( TextMode Mode )
    {
      switch ( Mode )
      {
        case TextMode.COMMODORE_40_X_25_ECM:
        case TextMode.MEGA65_80_X_25_ECM:
          return TextCharMode.COMMODORE_ECM;
        case TextMode.COMMODORE_40_X_25_HIRES:
        case TextMode.MEGA65_80_X_25_HIRES:
          return TextCharMode.COMMODORE_HIRES;
        case TextMode.COMMODORE_40_X_25_MULTICOLOR:
        case TextMode.MEGA65_80_X_25_MULTICOLOR:
          return TextCharMode.COMMODORE_MULTICOLOR;
        case TextMode.MEGA65_40_X_25_FCM:
        case TextMode.MEGA65_80_X_25_FCM:
          return TextCharMode.MEGA65_FCM;
        case TextMode.MEGA65_40_X_25_FCM_16BIT:
        case TextMode.MEGA65_80_X_25_FCM_16BIT:
          return TextCharMode.MEGA65_FCM_16BIT;
        case TextMode.COMMODORE_VC20_22_X_23:
          return TextCharMode.VC20;
        default:
          Debug.Log( "FromTextMode unsupported Mode " + Mode );
          return TextCharMode.COMMODORE_HIRES;
      }
    }



    internal static int NumCharactersForMode( TextCharMode Mode )
    {
      switch ( Mode )
      {
        case TextCharMode.COMMODORE_ECM:
        case TextCharMode.COMMODORE_HIRES:
        case TextCharMode.COMMODORE_MULTICOLOR:
        case TextCharMode.MEGA65_FCM:
        case TextCharMode.VC20:
          return 256;
        case TextCharMode.MEGA65_FCM_16BIT:
          return 65536;
        default:
          Debug.Log( "NumCharactersForMode unsupported Mode " + Mode );
          return 256;
      }
    }



    internal static bool RequiresCustomColorForCharacter( TextCharMode Mode )
    {
      switch ( Mode )
      {
        case TextCharMode.COMMODORE_ECM:
        case TextCharMode.COMMODORE_HIRES:
        case TextCharMode.COMMODORE_MULTICOLOR:
        case TextCharMode.VC20:
          return false;
        case TextCharMode.MEGA65_FCM:
        case TextCharMode.MEGA65_FCM_16BIT:
          return true;
        default:
          Debug.Log( "RequiresCustomColorForCharacter unsupported Mode " + Mode );
          return false;
      }
    }



    internal static bool HasCustomPalette( TextCharMode Mode )
    {
      switch ( Mode )
      {
        case TextCharMode.COMMODORE_ECM:
        case TextCharMode.COMMODORE_HIRES:
        case TextCharMode.COMMODORE_MULTICOLOR:
        case TextCharMode.VC20:
          return false;
        case TextCharMode.MEGA65_FCM:
        case TextCharMode.MEGA65_FCM_16BIT:
          return true;
        default:
          Debug.Log( "HasCustomPalette unsupported Mode " + Mode );
          return false;
      }
    }



  }

}