using RetroDevStudio.Formats;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;



namespace RetroDevStudio
{
  static class Lookup
  {
    internal static Dictionary<CompileTargetType,string>     CompileTargetModeToKeyword = new Dictionary<CompileTargetType, string>();



    static Lookup()
    {
      CompileTargetModeToKeyword[CompileTargetType.PRG] = "CBM";
      CompileTargetModeToKeyword[CompileTargetType.PLAIN] = "PLAIN";
      CompileTargetModeToKeyword[CompileTargetType.T64] = "T64";
      CompileTargetModeToKeyword[CompileTargetType.TAP] = "TAP";
      CompileTargetModeToKeyword[CompileTargetType.D64] = "D64";
      CompileTargetModeToKeyword[CompileTargetType.D81] = "D81";
      CompileTargetModeToKeyword[CompileTargetType.CARTRIDGE_8K_BIN] = "CART8BIN";
      CompileTargetModeToKeyword[CompileTargetType.CARTRIDGE_8K_CRT] = "CART8CRT";
      CompileTargetModeToKeyword[CompileTargetType.CARTRIDGE_16K_BIN] = "CART16BIN";
      CompileTargetModeToKeyword[CompileTargetType.CARTRIDGE_16K_CRT] = "CART16CRT";
      CompileTargetModeToKeyword[CompileTargetType.CARTRIDGE_MAGICDESK_BIN] = "MAGICDESKBIN";
      CompileTargetModeToKeyword[CompileTargetType.CARTRIDGE_MAGICDESK_CRT] = "MAGICDESKCRT";
      CompileTargetModeToKeyword[CompileTargetType.CARTRIDGE_RGCD_BIN] = "RGCDBIN";
      CompileTargetModeToKeyword[CompileTargetType.CARTRIDGE_RGCD_CRT] = "RGCDCRT";
      CompileTargetModeToKeyword[CompileTargetType.CARTRIDGE_EASYFLASH_BIN] = "EASYFLASHBIN";
      CompileTargetModeToKeyword[CompileTargetType.CARTRIDGE_EASYFLASH_CRT] = "EASYFLASHCRT";
      CompileTargetModeToKeyword[CompileTargetType.CARTRIDGE_GMOD2_BIN] = "GMOD2BIN";
      CompileTargetModeToKeyword[CompileTargetType.CARTRIDGE_GMOD2_CRT] = "GMOD2CRT";
      CompileTargetModeToKeyword[CompileTargetType.CARTRIDGE_ULTIMAX_4K_BIN] = "ULTIMAX4BIN";
      CompileTargetModeToKeyword[CompileTargetType.CARTRIDGE_ULTIMAX_4K_CRT] = "ULTIMAX4CRT";
      CompileTargetModeToKeyword[CompileTargetType.CARTRIDGE_ULTIMAX_8K_BIN] = "ULTIMAX8BIN";
      CompileTargetModeToKeyword[CompileTargetType.CARTRIDGE_ULTIMAX_8K_CRT] = "ULTIMAX8CRT";
      CompileTargetModeToKeyword[CompileTargetType.CARTRIDGE_ULTIMAX_16K_BIN] = "ULTIMAX16BIN";
      CompileTargetModeToKeyword[CompileTargetType.CARTRIDGE_ULTIMAX_16K_CRT] = "ULTIMAX16CRT";
    }



    public static int NumberOfColorsInCharacter( TextCharMode Mode )
    {
      switch ( Mode )
      {
        case TextCharMode.COMMODORE_ECM:
        case TextCharMode.COMMODORE_HIRES:
        case TextCharMode.COMMODORE_MULTICOLOR:
        case TextCharMode.VIC20:
        case TextCharMode.MEGA65_NCM:
          return 16;
        case TextCharMode.MEGA65_HIRES:
        case TextCharMode.MEGA65_ECM:
        case TextCharMode.MEGA65_FCM:
        case TextCharMode.MEGA65_FCM_16BIT:
          return 256;
        default:
          Debug.Log( "NumberOfColorsInCharacter unsupported Mode " + Mode );
          return 16;
      }
    }



    public static int NumBytesOfSingleCharacterBitmap( TextCharMode Mode )
    {
      switch ( Mode )
      {
        case TextCharMode.COMMODORE_ECM:
        case TextCharMode.COMMODORE_HIRES:
        case TextCharMode.COMMODORE_MULTICOLOR:
        case TextCharMode.VIC20:
        case TextCharMode.MEGA65_ECM:
        case TextCharMode.MEGA65_HIRES:
          return 8;
        case TextCharMode.MEGA65_FCM:
        case TextCharMode.MEGA65_FCM_16BIT:
        case TextCharMode.MEGA65_NCM:
          return 64;
        default:
          Debug.Log( "NumBytesOfSingleCharacter unsupported Mode " + Mode );
          return 8;
      }
    }



    public static int NumBytesOfSingleCharacter( TextCharMode Mode )
    {
      switch ( Mode )
      {
        case TextCharMode.MEGA65_FCM_16BIT:
          return 2;
        default:
          return 1;
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
        case GraphicTileMode.MEGA65_NCM:
          return ( ( Width + 1 ) / 2 ) * Height;
        case GraphicTileMode.MEGA65_FCM_256_COLORS:
          return Width * Height;
        default:
          Debug.Log( "Lookup.NumBytes unsupported mode " + Mode );
          return 0;
      }
    }



    public static TextCharMode TextCharModeFromTextMode( TextMode Mode )
    {
      switch ( Mode )
      {
        case TextMode.COMMODORE_40_X_25_ECM:
          return TextCharMode.COMMODORE_ECM;
        case TextMode.MEGA65_80_X_25_ECM:
        case TextMode.MEGA65_40_X_25_ECM:
          return TextCharMode.MEGA65_ECM;
        case TextMode.MEGA65_40_X_25_HIRES:
        case TextMode.MEGA65_80_X_25_HIRES:
          return TextCharMode.MEGA65_HIRES;
        case TextMode.COMMODORE_40_X_25_HIRES:
          return TextCharMode.COMMODORE_HIRES;
        case TextMode.MEGA65_40_X_25_MULTICOLOR:
        case TextMode.MEGA65_80_X_25_MULTICOLOR:
        case TextMode.COMMODORE_40_X_25_MULTICOLOR:
          return TextCharMode.COMMODORE_MULTICOLOR;
        case TextMode.MEGA65_40_X_25_FCM:
        case TextMode.MEGA65_80_X_25_FCM:
          return TextCharMode.MEGA65_FCM;
        case TextMode.MEGA65_40_X_25_FCM_16BIT:
        case TextMode.MEGA65_80_X_25_FCM_16BIT:
          return TextCharMode.MEGA65_FCM_16BIT;
        case TextMode.COMMODORE_VIC20_22_X_23:
          return TextCharMode.VIC20;
        default:
          Debug.Log( "TextCharModeFromTextMode unsupported Mode " + Mode );
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
        case TextCharMode.MEGA65_ECM:
        case TextCharMode.MEGA65_HIRES:
        case TextCharMode.MEGA65_FCM:
        case TextCharMode.VIC20:
          return 256;
        case TextCharMode.MEGA65_FCM_16BIT:
        case TextCharMode.MEGA65_NCM:
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
        case TextCharMode.MEGA65_ECM:
        case TextCharMode.MEGA65_HIRES:
        case TextCharMode.VIC20:
          return true;
        case TextCharMode.MEGA65_FCM:
        case TextCharMode.MEGA65_FCM_16BIT:
        case TextCharMode.MEGA65_NCM:
          return false;
        default:
          Debug.Log( "RequiresCustomColorForCharacter unsupported Mode " + Mode );
          return false;
      }
    }



    internal static bool TextModeUsesColor( TextMode Mode )
    {
      switch ( Mode )
      {
        case TextMode.MEGA65_40_X_25_FCM:
        case TextMode.MEGA65_40_X_25_FCM_16BIT:
        case TextMode.MEGA65_80_X_25_FCM:
        case TextMode.MEGA65_80_X_25_FCM_16BIT:
          return false;
        default:
          return true;
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
        case GraphicTileMode.MEGA65_NCM:
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
        case SpriteProject.SpriteProjectMode.MEGA65_16_X_21_16_COLORS:
        case SpriteProject.SpriteProjectMode.MEGA65_64_X_21_HIRES_OR_MC:
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
        case SpriteProject.SpriteProjectMode.MEGA65_16_X_21_16_COLORS:
        case SpriteProject.SpriteProjectMode.MEGA65_64_X_21_HIRES_OR_MC:
          return 192;
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
        case SpriteProject.SpriteProjectMode.MEGA65_64_X_21_HIRES_OR_MC:
        case SpriteProject.SpriteProjectMode.MEGA65_16_X_21_16_COLORS:
          return 16;
        default:
          Debug.Log( "NumberOfColorsInSprite unsupported Mode " + Mode );
          return 16;
      }
    }



    internal static GraphicTileMode GraphicTileModeFromTextCharMode( TextCharMode Mode, int CustomColor )
    {
      switch ( Mode )
      {
        case TextCharMode.COMMODORE_ECM:
        case TextCharMode.MEGA65_ECM:
          return GraphicTileMode.COMMODORE_ECM;
        case TextCharMode.COMMODORE_HIRES:
        case TextCharMode.MEGA65_HIRES:
          return GraphicTileMode.COMMODORE_HIRES;
        case TextCharMode.COMMODORE_MULTICOLOR:
        case TextCharMode.VIC20:
          if ( CustomColor < 8 )
          {
            return GraphicTileMode.COMMODORE_HIRES;
          }
          return GraphicTileMode.COMMODORE_MULTICOLOR;
        case TextCharMode.MEGA65_NCM:
          return GraphicTileMode.MEGA65_NCM;
        case TextCharMode.MEGA65_FCM:
        case TextCharMode.MEGA65_FCM_16BIT:
          return GraphicTileMode.MEGA65_FCM_256_COLORS;
        default:
          Debug.Log( "GraphicTileModeFromTextCharMode unsupported mode " + Mode );
          return GraphicTileMode.COMMODORE_HIRES;
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
          return GraphicTileMode.MEGA65_NCM;
      }
    }



    internal static GraphicTileMode GraphicTileModeFromSpriteProjectMode( SpriteProject.SpriteProjectMode Mode )
    {
      switch ( Mode )
      {
        case SpriteProject.SpriteProjectMode.COMMODORE_24_X_21_HIRES_OR_MC:
        case SpriteProject.SpriteProjectMode.MEGA65_64_X_21_HIRES_OR_MC:
        default:
          return GraphicTileMode.COMMODORE_HIRES;
        case SpriteProject.SpriteProjectMode.MEGA65_16_X_21_16_COLORS:
          return GraphicTileMode.MEGA65_NCM;
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
        case SpriteProject.SpriteProjectMode.MEGA65_64_X_21_HIRES_OR_MC:
          // TODO - not correct as well!
          return SpriteMode.MEGA65_64_X_21_16_HIRES;
        case SpriteProject.SpriteProjectMode.MEGA65_16_X_21_16_COLORS:
          return SpriteMode.MEGA65_16_X_21_16_COLORS;
      }
    }



    internal static bool SpriteModeSupportsMulticolorFlag( SpriteProject.SpriteProjectMode Mode )
    {
      if ( ( Mode == SpriteProject.SpriteProjectMode.COMMODORE_24_X_21_HIRES_OR_MC )
      ||   ( Mode == SpriteProject.SpriteProjectMode.MEGA65_64_X_21_HIRES_OR_MC ) )
      {
        return true;
      }
      return false;
    }



    internal static bool SpriteModeHasMulticolorEnabled( SpriteMode Mode )
    {
      if ( ( Mode == SpriteMode.COMMODORE_24_X_21_MULTICOLOR )
      ||   ( Mode == SpriteMode.MEGA65_64_X_21_16_MULTICOLOR ) )
      {
        return true;
      }
      return false;
    }



    internal static int CharacterWidthInPixel( GraphicTileMode Mode )
    {
      switch ( Mode )
      {
        case GraphicTileMode.MEGA65_NCM:
          return 16;
        case GraphicTileMode.MEGA65_FCM_256_COLORS:
        case GraphicTileMode.COMMODORE_HIRES:
        case GraphicTileMode.COMMODORE_ECM:
        case GraphicTileMode.COMMODORE_MULTICOLOR:
          return 8;
        default:
          Debug.Log( "Lookup.CharacterWidthInPixel - unsupported mode " + Mode );
          return 0;
      }
    }



    internal static int CharacterHeightInPixel( GraphicTileMode Mode )
    {
      switch ( Mode )
      {
        case GraphicTileMode.MEGA65_NCM:
        case GraphicTileMode.MEGA65_FCM_256_COLORS:
        case GraphicTileMode.COMMODORE_HIRES:
        case GraphicTileMode.COMMODORE_ECM:
        case GraphicTileMode.COMMODORE_MULTICOLOR:
          return 8;
        default:
          Debug.Log( "Lookup.CharacterHeightInPixel - unsupported mode " + Mode );
          return 0;
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



    internal static TextMode TextModeFromTextCharMode( TextCharMode Mode )
    {
      switch ( Mode )
      {
        case TextCharMode.MEGA65_ECM:
          return TextMode.MEGA65_40_X_25_ECM;
        case TextCharMode.MEGA65_HIRES:
          return TextMode.MEGA65_40_X_25_HIRES;
        case TextCharMode.COMMODORE_ECM:
          return TextMode.COMMODORE_40_X_25_ECM;
        case TextCharMode.COMMODORE_HIRES:
          return TextMode.COMMODORE_40_X_25_HIRES;
        case TextCharMode.COMMODORE_MULTICOLOR:
          return TextMode.COMMODORE_40_X_25_MULTICOLOR;
        case TextCharMode.MEGA65_FCM:
          return TextMode.MEGA65_40_X_25_FCM;
        case TextCharMode.MEGA65_FCM_16BIT:
          return TextMode.MEGA65_40_X_25_FCM_16BIT;
        case TextCharMode.VIC20:
          return TextMode.COMMODORE_VIC20_22_X_23;
        default:
          Debug.Log( "TextModeFromTextCharMode unsupported mode " + Mode );
          return TextMode.COMMODORE_40_X_25_HIRES;
      }
    }



    internal static SpriteMode SpriteModeFromTileMode( GraphicTileMode Mode )
    {
      switch ( Mode )
      {
        case GraphicTileMode.COMMODORE_ECM:
        case GraphicTileMode.COMMODORE_HIRES:
        default:
          return SpriteMode.COMMODORE_24_X_21_HIRES;
        case GraphicTileMode.COMMODORE_MULTICOLOR:
          return SpriteMode.COMMODORE_24_X_21_MULTICOLOR;
        case GraphicTileMode.MEGA65_NCM:
        case GraphicTileMode.MEGA65_FCM_256_COLORS:
          return SpriteMode.MEGA65_16_X_21_16_COLORS;
      }
    }



    internal static GraphicTileMode GraphicTileModeFromSpriteMode( SpriteMode Mode )
    {
      switch ( Mode )
      {
        case SpriteMode.COMMODORE_24_X_21_HIRES:
          return GraphicTileMode.COMMODORE_HIRES;
        case SpriteMode.COMMODORE_24_X_21_MULTICOLOR:
          return GraphicTileMode.COMMODORE_MULTICOLOR;
        case SpriteMode.MEGA65_16_X_21_16_COLORS:
        case SpriteMode.MEGA65_64_X_21_16_HIRES:
          return GraphicTileMode.MEGA65_NCM;
        default:
          return GraphicTileMode.COMMODORE_HIRES;
      }
    }



    public static bool SpriteHasMulticolorEnabled( SpriteMode Mode )
    {
      if ( ( Mode == SpriteMode.COMMODORE_24_X_21_MULTICOLOR )
      ||   ( Mode == SpriteMode.MEGA65_64_X_21_16_MULTICOLOR ) )
      {
        return true;
      }
      return false;
    }



    public static int NumberOfColorsInDisplayMode( GraphicScreenProject.CheckType CheckType )
    {
      switch ( CheckType )
      {
        case GraphicScreenProject.CheckType.MEGA65_FCM_CHARSET:
        case GraphicScreenProject.CheckType.MEGA65_FCM_CHARSET_16BIT:
          return 256;
        default:
          return 16;
      }
    }



    public static TextCharMode CharacterModeFromCheckType( GraphicScreenProject.CheckType CheckType )
    {
      switch ( CheckType )
      {
        case GraphicScreenProject.CheckType.MEGA65_FCM_CHARSET:
          return TextCharMode.MEGA65_FCM;
        case GraphicScreenProject.CheckType.MEGA65_FCM_CHARSET_16BIT:
          return TextCharMode.MEGA65_FCM_16BIT;
        case GraphicScreenProject.CheckType.HIRES_BITMAP:
        case GraphicScreenProject.CheckType.HIRES_CHARSET:
        default:
          return TextCharMode.COMMODORE_HIRES;
        case GraphicScreenProject.CheckType.MULTICOLOR_BITMAP:
        case GraphicScreenProject.CheckType.MULTICOLOR_CHARSET:
          return TextCharMode.COMMODORE_MULTICOLOR;
      }
    }
  }

}