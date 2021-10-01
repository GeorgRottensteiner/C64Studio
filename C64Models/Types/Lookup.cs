using C64Studio.Formats;
using System;

namespace RetroDevStudio
{
  static class Lookup
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



    internal static int NumBytes( int Width, int Height, GraphicTileMode Mode )
    {
      switch ( Mode )
      {
        case GraphicTileMode.COMMODORE_ECM:
        case GraphicTileMode.COMMODORE_HIRES:
        case GraphicTileMode.COMMODORE_MULTICOLOR:
          return ( ( Width + 7 ) / 8 ) * Height;
        case GraphicTileMode.MEGA65_FCM_16_COLORS:
          return ( ( Width + 1 ) / 2 ) * Height;
        case GraphicTileMode.MEGA65_FCM_256_COLORS:
          return Width * Height;
        default:
          Debug.Log( "Lookup.NumBytes unsupported mode " + Mode );
          return 0;
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
          return 8192;
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
          return true;
        case TextCharMode.MEGA65_FCM:
        case TextCharMode.MEGA65_FCM_16BIT:
          return false;
        default:
          Debug.Log( "RequiresCustomColorForCharacter unsupported Mode " + Mode );
          return false;
      }
    }



    internal static bool HasCustomPalette( GraphicTileMode Mode )
    {
      switch ( Mode )
      {
        case GraphicTileMode.COMMODORE_ECM:
        case GraphicTileMode.COMMODORE_HIRES:
        case GraphicTileMode.COMMODORE_MULTICOLOR:
          return false;
        case GraphicTileMode.MEGA65_FCM_16_COLORS:
        case GraphicTileMode.MEGA65_FCM_256_COLORS:
          return true;
        default:
          Debug.Log( "HasCustomPalette unsupported Mode " + Mode );
          return false;
      }
    }



    internal static int NumBytesOfSingleSprite( SpriteProject.SpriteProjectMode Mode )
    {
      switch ( Mode )
      {
        case SpriteProject.SpriteProjectMode.COMMODORE_24_X_21_HIRES_OR_MC:
          return 63;
        case SpriteProject.SpriteProjectMode.MEGA65_8_X_21_16_COLORS:
          return 84;
        case SpriteProject.SpriteProjectMode.MEGA65_16_X_21_16_COLORS:
          return 168;
        default:
          Debug.Log( "NumBytesOfSingleSprite unsupported Mode " + Mode );
          return 63;
      }
    }



    internal static int NumPaddedBytesOfSingleSprite( SpriteProject.SpriteProjectMode Mode )
    {
      switch ( Mode )
      {
        case SpriteProject.SpriteProjectMode.COMMODORE_24_X_21_HIRES_OR_MC:
          return 64;
        case SpriteProject.SpriteProjectMode.MEGA65_8_X_21_16_COLORS:
          return 128;
        case SpriteProject.SpriteProjectMode.MEGA65_16_X_21_16_COLORS:
          return 256;
        default:
          Debug.Log( "NumPaddedBytesOfSingleSprite unsupported Mode " + Mode );
          return 64;
      }
    }



    internal static int NumberOfColorsInSprite( SpriteProject.SpriteProjectMode Mode )
    {
      switch ( Mode )
      {
        case SpriteProject.SpriteProjectMode.COMMODORE_24_X_21_HIRES_OR_MC:
        case SpriteProject.SpriteProjectMode.MEGA65_8_X_21_16_COLORS:
        case SpriteProject.SpriteProjectMode.MEGA65_16_X_21_16_COLORS:
          return 16;
        default:
          Debug.Log( "NumberOfColorsInCharacter unsupported Mode " + Mode );
          return 16;
      }
    }



    internal static GraphicTileMode GraphicTileModeFromTextCharMode( TextCharMode Mode )
    {
      switch ( Mode )
      {
        case TextCharMode.COMMODORE_ECM:
          return GraphicTileMode.COMMODORE_ECM;
        case TextCharMode.COMMODORE_HIRES:
        default:
          return GraphicTileMode.COMMODORE_HIRES;
        case TextCharMode.COMMODORE_MULTICOLOR:
        case TextCharMode.VC20:
          return GraphicTileMode.COMMODORE_MULTICOLOR;
        case TextCharMode.MEGA65_FCM:
          return GraphicTileMode.MEGA65_FCM_256_COLORS;
      }
    }



    internal static GraphicTileMode GraphicTileModeFromSpriteDisplayMode( SpriteDisplayMode Mode )
    {
      switch ( Mode )
      {
        case SpriteDisplayMode.COMMODORE_24_X_21_HIRES:
        default:
          return GraphicTileMode.COMMODORE_HIRES;
        case SpriteDisplayMode.COMMODORE_24_X_21_MULTICOLOR:
          return GraphicTileMode.COMMODORE_MULTICOLOR;
        case SpriteDisplayMode.MEGA65_8_X_21_16_COLORS:
        case SpriteDisplayMode.MEGA65_16_X_21_16_COLORS:
          return GraphicTileMode.MEGA65_FCM_16_COLORS;
      }
    }



    internal static GraphicTileMode GraphicTileModeFromSpriteProjectMode( SpriteProject.SpriteProjectMode Mode )
    {
      switch ( Mode )
      {
        case SpriteProject.SpriteProjectMode.COMMODORE_24_X_21_HIRES_OR_MC:
        default:
          return GraphicTileMode.COMMODORE_HIRES;
        case SpriteProject.SpriteProjectMode.MEGA65_8_X_21_16_COLORS:
        case SpriteProject.SpriteProjectMode.MEGA65_16_X_21_16_COLORS:
          return GraphicTileMode.MEGA65_FCM_16_COLORS;
      }
    }



    internal static bool HaveCustomSpriteColor( SpriteProject.SpriteProjectMode Mode )
    {
      switch ( Mode )
      {
        case SpriteProject.SpriteProjectMode.COMMODORE_24_X_21_HIRES_OR_MC:
          return true;
      }
      return false;
    }



    internal static SpriteMode SpriteModeFromSpriteProjectMode( SpriteProject.SpriteProjectMode Mode )
    {
      switch ( Mode )
      {
        case SpriteProject.SpriteProjectMode.COMMODORE_24_X_21_HIRES_OR_MC:
        default:
          // TODO - not correct!
          return SpriteMode.COMMODORE_24_X_21_HIRES;
        case SpriteProject.SpriteProjectMode.MEGA65_8_X_21_16_COLORS:
          return SpriteMode.MEGA65_8_X_21_16_COLORS;
        case SpriteProject.SpriteProjectMode.MEGA65_16_X_21_16_COLORS:
          return SpriteMode.MEGA65_16_X_21_16_COLORS;
      }
    }



    internal static int PixelWidth( GraphicTileMode Mode )
    {
      switch ( Mode )
      {
        case GraphicTileMode.COMMODORE_MULTICOLOR:
          return 2;
      }
      return 1;
    }



  }

}