using RetroDevStudio.Formats;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;



namespace RetroDevStudio
{
  static class Lookup
  {
    internal static Dictionary<CompileTargetType,string>  CompileTargetModeToKeyword = new Dictionary<CompileTargetType, string>();

    internal static Dictionary<MediaFormatType,Type>      MediaFormatToType = new Dictionary<MediaFormatType, Type>();
    internal static Dictionary<MediaFormatType,GR.Generic.Tupel<MediaType, string>>    MediaFormatCategories = new Dictionary<MediaFormatType, GR.Generic.Tupel<MediaType, string>>();



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
      CompileTargetModeToKeyword[CompileTargetType.CARTRIDGE_MAGICDESK_BIN_64K] = "MAGICDESKBIN";
      CompileTargetModeToKeyword[CompileTargetType.CARTRIDGE_MAGICDESK_CRT_64K] = "MAGICDESKCRT";
      CompileTargetModeToKeyword[CompileTargetType.CARTRIDGE_MAGICDESK_BIN_32K] = "MAGICDESK32BIN";
      CompileTargetModeToKeyword[CompileTargetType.CARTRIDGE_MAGICDESK_CRT_32K] = "MAGICDESK32CRT";
      CompileTargetModeToKeyword[CompileTargetType.CARTRIDGE_MAGICDESK_BIN_128K] = "MAGICDESK128BIN";
      CompileTargetModeToKeyword[CompileTargetType.CARTRIDGE_MAGICDESK_CRT_128K] = "MAGICDESK128CRT";
      CompileTargetModeToKeyword[CompileTargetType.CARTRIDGE_MAGICDESK_BIN_256K] = "MAGICDESK256BIN";
      CompileTargetModeToKeyword[CompileTargetType.CARTRIDGE_MAGICDESK_CRT_256K] = "MAGICDESK256CRT";
      CompileTargetModeToKeyword[CompileTargetType.CARTRIDGE_MAGICDESK_BIN_512K] = "MAGICDESK512BIN";
      CompileTargetModeToKeyword[CompileTargetType.CARTRIDGE_MAGICDESK_CRT_512K] = "MAGICDESK512CRT";
      CompileTargetModeToKeyword[CompileTargetType.CARTRIDGE_MAGICDESK_BIN_1M] = "MAGICDESK1MBIN";
      CompileTargetModeToKeyword[CompileTargetType.CARTRIDGE_MAGICDESK_CRT_1M] = "MAGICDESK1MCRT";
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
      CompileTargetModeToKeyword[CompileTargetType.DSK] = "DSK";

      EnumerateMediaTypes();
    }



    private static void EnumerateMediaTypes()
    {
      var mediaTypes = Assembly.GetExecutingAssembly().GetTypes()
              .Where( t => t.IsSubclassOf( typeof( MediaFormat ) ) )
              .Where( t => String.Equals( t.Namespace, "RetroDevStudio.Formats", StringComparison.Ordinal ) );
      foreach ( var mediaType in mediaTypes )
      {
        var allAttributes = mediaType.GetCustomAttributes( false );

        string categoryOfEnum = "";

        MediaType         mediaTypeAtt = MediaType.UNKNOWN;
        var mediaFormats = new List<MediaFormatType>();

        foreach ( var attribute in allAttributes )
        {
          if ( attribute is MediaFormatAttribute )
          {
            var mediaTypeOfType = attribute as MediaFormatAttribute;
            mediaFormats.Add( mediaTypeOfType.Type );
            MediaFormatToType.Add( mediaTypeOfType.Type, mediaType );
          }
          if ( attribute is MediaTypeAttribute )
          {
            var mediaTypeAttribute = attribute as MediaTypeAttribute;
            mediaTypeAtt = mediaTypeAttribute.Type;
          }
          if ( attribute is CategoryAttribute )
          {
            var category = attribute as CategoryAttribute;
            categoryOfEnum = category.Category;
          }
        }
        foreach ( var mediaFormat in mediaFormats )
        {
          MediaFormatCategories.Add( mediaFormat, new GR.Generic.Tupel<MediaType, string>( mediaTypeAtt, categoryOfEnum ) );
        }
      }
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
        case TextCharMode.X16_HIRES:
        case TextCharMode.COMMODORE_128_VDC_HIRES:
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
        case TextCharMode.X16_HIRES:
        case TextCharMode.COMMODORE_128_VDC_HIRES:
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
        case TextCharMode.MEGA65_NCM:
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
        case GraphicTileMode.COMMODORE_MULTICOLOR_CHARACTERS:
        case GraphicTileMode.COMMODORE_MULTICOLOR_SPRITES:
        case GraphicTileMode.COMMANDERX16_HIRES:
          return ( ( Width + 7 ) / 8 ) * Height;
        case GraphicTileMode.MEGA65_NCM_CHARACTERS:
        case GraphicTileMode.MEGA65_NCM_SPRITES:
        case GraphicTileMode.COMMANDERX16_16_COLORS:
          return ( ( Width + 1 ) / 2 ) * Height;
        case GraphicTileMode.MEGA65_FCM_256_COLORS:
        case GraphicTileMode.COMMANDERX16_256_COLORS:
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
        case TextMode.COMMODORE_128_VDC_80_X_25_HIRES:
          return TextCharMode.COMMODORE_128_VDC_HIRES;
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
        case TextMode.MEGA65_80_X_25_NCM:
        case TextMode.MEGA65_40_X_25_NCM:
          return TextCharMode.MEGA65_NCM;
        case TextMode.X16_20_X_15:
        case TextMode.X16_20_X_30:
        case TextMode.X16_40_X_15:
        case TextMode.X16_40_X_30:
        case TextMode.X16_40_X_60:
        case TextMode.X16_80_X_30:
        case TextMode.X16_80_X_60:
          return TextCharMode.X16_HIRES;
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
        case TextCharMode.X16_HIRES:
        case TextCharMode.COMMODORE_128_VDC_HIRES:
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
        case TextCharMode.X16_HIRES:
        case TextCharMode.COMMODORE_128_VDC_HIRES:
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
        case GraphicTileMode.COMMODORE_MULTICOLOR_SPRITES:
        case GraphicTileMode.COMMODORE_MULTICOLOR_CHARACTERS:
        case GraphicTileMode.COMMODORE_128_VDC_HIRES:
          return false;
        case GraphicTileMode.MEGA65_NCM_CHARACTERS :
        case GraphicTileMode.MEGA65_NCM_SPRITES:
        case GraphicTileMode.MEGA65_FCM_256_COLORS:
        case GraphicTileMode.COMMANDERX16_HIRES:
        case GraphicTileMode.COMMANDERX16_16_COLORS:
        case GraphicTileMode.COMMANDERX16_256_COLORS:
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
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_8_16_COLORS:
          return 32;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_8_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_16_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_8_256_COLORS:
          return 64;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_16_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_8_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_32_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_16_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_8_256_COLORS:
          return 128;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_16_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_32_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_8_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_64_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_32_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_16_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_8_256_COLORS:
          return 256;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_16_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_32_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_64_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_64_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_32_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_16_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_8_256_COLORS:
          return 512;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_32_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_64_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_64_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_32_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_16_256_COLORS:
          return 1024;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_64_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_64_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_32_256_COLORS:
          return 2048;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_64_256_COLORS:
          return 4096;
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
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_8_16_COLORS:
          return 32;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_8_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_16_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_8_256_COLORS:
          return 64;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_16_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_8_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_32_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_16_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_8_256_COLORS:
          return 128;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_16_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_32_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_8_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_64_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_32_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_16_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_8_256_COLORS:
          return 256;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_16_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_32_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_64_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_64_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_32_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_16_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_8_256_COLORS:
          return 512;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_32_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_64_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_64_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_32_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_16_256_COLORS:
          return 1024;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_64_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_64_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_32_256_COLORS:
          return 2048;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_64_256_COLORS:
          return 4096;
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
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_8_16_COLORS:
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
          return GraphicTileMode.COMMODORE_MULTICOLOR_CHARACTERS;
        case TextCharMode.MEGA65_NCM:
          return GraphicTileMode.MEGA65_NCM_CHARACTERS;
        case TextCharMode.MEGA65_FCM:
        case TextCharMode.MEGA65_FCM_16BIT:
          return GraphicTileMode.MEGA65_FCM_256_COLORS;
        case TextCharMode.X16_HIRES:
          return GraphicTileMode.COMMANDERX16_HIRES;
        case TextCharMode.COMMODORE_128_VDC_HIRES:
          return GraphicTileMode.COMMODORE_128_VDC_HIRES;
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
          return GraphicTileMode.COMMODORE_MULTICOLOR_SPRITES;
        case SpriteDisplayMode.MEGA65_8_X_21_16_COLORS:
        case SpriteDisplayMode.MEGA65_16_X_21_16_COLORS:
          return GraphicTileMode.MEGA65_NCM_SPRITES;
      }
    }



    internal static GraphicTileMode GraphicTileModeFromSpriteProjectMode( SpriteProject.SpriteProjectMode Mode )
    {
      switch ( Mode )
      {
        case SpriteProject.SpriteProjectMode.COMMODORE_24_X_21_HIRES_OR_MC:
        case SpriteProject.SpriteProjectMode.MEGA65_64_X_21_HIRES_OR_MC:
          return GraphicTileMode.COMMODORE_HIRES;
        case SpriteProject.SpriteProjectMode.MEGA65_16_X_21_16_COLORS:
          return GraphicTileMode.MEGA65_NCM_SPRITES;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_8_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_16_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_32_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_64_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_8_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_16_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_32_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_64_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_8_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_16_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_32_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_64_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_8_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_16_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_32_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_64_16_COLORS:
          return GraphicTileMode.COMMANDERX16_16_COLORS;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_8_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_16_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_32_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_64_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_8_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_16_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_32_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_64_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_8_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_16_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_32_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_64_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_8_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_16_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_32_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_64_256_COLORS:
          return GraphicTileMode.COMMANDERX16_256_COLORS;
        default:
          Debug.Log( "GraphicTileModeFromSpriteProjectMode unsupported mode " + Mode );
          return GraphicTileMode.COMMODORE_HIRES;
      }
    }



    internal static bool HaveCustomSpriteColor( SpriteProject.SpriteProjectMode Mode )
    {
      switch ( Mode )
      {
        /*
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_8_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_16_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_32_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_64_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_8_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_16_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_32_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_64_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_8_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_16_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_32_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_64_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_8_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_16_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_32_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_64_16_COLORS:*/
        case SpriteProject.SpriteProjectMode.COMMODORE_24_X_21_HIRES_OR_MC:
          return true;
      }
      return false;
    }



    internal static SpriteMode SpriteModeFromSpriteProjectMode( SpriteProject.SpriteProjectMode Mode )
    {
      switch ( Mode )
      {
        default:
          Debug.Log( "SpriteModeFromSpriteProjectMode unsupported mode " + Mode );
          return SpriteMode.COMMODORE_24_X_21_HIRES;
        case SpriteProject.SpriteProjectMode.COMMODORE_24_X_21_HIRES_OR_MC:
          // TODO - not correct!
          return SpriteMode.COMMODORE_24_X_21_HIRES;
        case SpriteProject.SpriteProjectMode.MEGA65_64_X_21_HIRES_OR_MC:
          // TODO - not correct as well!
          return SpriteMode.MEGA65_64_X_21_16_HIRES;
        case SpriteProject.SpriteProjectMode.MEGA65_16_X_21_16_COLORS:
          return SpriteMode.MEGA65_16_X_21_16_COLORS;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_8_16_COLORS:
          return SpriteMode.COMMANDER_X16_8_8_16_COLORS;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_8_16_COLORS:
          return SpriteMode.COMMANDER_X16_16_8_16_COLORS;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_8_16_COLORS:
          return SpriteMode.COMMANDER_X16_32_8_16_COLORS;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_8_16_COLORS:
          return SpriteMode.COMMANDER_X16_64_8_16_COLORS;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_16_16_COLORS:
          return SpriteMode.COMMANDER_X16_8_16_16_COLORS;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_16_16_COLORS:
          return SpriteMode.COMMANDER_X16_16_16_16_COLORS;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_16_16_COLORS:
          return SpriteMode.COMMANDER_X16_32_16_16_COLORS;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_16_16_COLORS:
          return SpriteMode.COMMANDER_X16_64_16_16_COLORS;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_32_16_COLORS:
          return SpriteMode.COMMANDER_X16_8_32_16_COLORS;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_32_16_COLORS:
          return SpriteMode.COMMANDER_X16_16_32_16_COLORS;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_32_16_COLORS:
          return SpriteMode.COMMANDER_X16_32_32_16_COLORS;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_32_16_COLORS:
          return SpriteMode.COMMANDER_X16_64_32_16_COLORS;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_64_16_COLORS:
          return SpriteMode.COMMANDER_X16_8_64_16_COLORS;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_64_16_COLORS:
          return SpriteMode.COMMANDER_X16_16_64_16_COLORS;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_64_16_COLORS:
          return SpriteMode.COMMANDER_X16_32_64_16_COLORS;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_64_16_COLORS:
          return SpriteMode.COMMANDER_X16_64_64_16_COLORS;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_8_256_COLORS:
          return SpriteMode.COMMANDER_X16_8_8_256_COLORS;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_8_256_COLORS:
          return SpriteMode.COMMANDER_X16_16_8_256_COLORS;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_8_256_COLORS:
          return SpriteMode.COMMANDER_X16_32_8_256_COLORS;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_8_256_COLORS:
          return SpriteMode.COMMANDER_X16_64_8_256_COLORS;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_16_256_COLORS:
          return SpriteMode.COMMANDER_X16_8_16_256_COLORS;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_16_256_COLORS:
          return SpriteMode.COMMANDER_X16_16_16_256_COLORS;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_16_256_COLORS:
          return SpriteMode.COMMANDER_X16_32_16_256_COLORS;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_16_256_COLORS:
          return SpriteMode.COMMANDER_X16_64_16_256_COLORS;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_32_256_COLORS:
          return SpriteMode.COMMANDER_X16_8_32_256_COLORS;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_32_256_COLORS:
          return SpriteMode.COMMANDER_X16_16_32_256_COLORS;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_32_256_COLORS:
          return SpriteMode.COMMANDER_X16_32_32_256_COLORS;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_32_256_COLORS:
          return SpriteMode.COMMANDER_X16_64_32_256_COLORS;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_64_256_COLORS:
          return SpriteMode.COMMANDER_X16_8_64_256_COLORS;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_64_256_COLORS:
          return SpriteMode.COMMANDER_X16_16_64_256_COLORS;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_64_256_COLORS:
          return SpriteMode.COMMANDER_X16_32_64_256_COLORS;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_64_256_COLORS:
          return SpriteMode.COMMANDER_X16_64_64_256_COLORS;
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
        case GraphicTileMode.MEGA65_NCM_CHARACTERS:
          return 16;
        case GraphicTileMode.MEGA65_FCM_256_COLORS:
        case GraphicTileMode.COMMODORE_HIRES:
        case GraphicTileMode.COMMODORE_ECM:
        case GraphicTileMode.COMMODORE_MULTICOLOR_CHARACTERS:
        case GraphicTileMode.COMMANDERX16_HIRES:
        case GraphicTileMode.COMMODORE_128_VDC_HIRES:
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
        case GraphicTileMode.MEGA65_NCM_CHARACTERS:
        case GraphicTileMode.MEGA65_FCM_256_COLORS:
        case GraphicTileMode.COMMODORE_HIRES:
        case GraphicTileMode.COMMODORE_ECM:
        case GraphicTileMode.COMMODORE_MULTICOLOR_CHARACTERS:
        case GraphicTileMode.COMMANDERX16_HIRES:
        case GraphicTileMode.COMMODORE_128_VDC_HIRES:
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
        case GraphicTileMode.COMMODORE_MULTICOLOR_CHARACTERS:
        case GraphicTileMode.COMMODORE_MULTICOLOR_SPRITES:
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



    internal static int ScreenWidthInCharacters( TextMode Mode )
    {
      switch ( Mode )
      {
        case TextMode.COMMODORE_40_X_25_ECM:
        case TextMode.COMMODORE_40_X_25_HIRES:
        case TextMode.COMMODORE_40_X_25_MULTICOLOR:
        case TextMode.MEGA65_40_X_25_NCM:
        case TextMode.MEGA65_40_X_25_ECM:
        case TextMode.MEGA65_40_X_25_FCM:
        case TextMode.MEGA65_40_X_25_FCM_16BIT:
        case TextMode.MEGA65_40_X_25_HIRES:
        case TextMode.MEGA65_40_X_25_MULTICOLOR:
        case TextMode.X16_40_X_15:
        case TextMode.X16_40_X_30:
        case TextMode.X16_40_X_60:
          return 40;
        case TextMode.COMMODORE_VIC20_22_X_23:
          return 22;
        case TextMode.MEGA65_80_X_25_NCM:
        case TextMode.MEGA65_80_X_25_ECM:
        case TextMode.MEGA65_80_X_25_FCM:
        case TextMode.MEGA65_80_X_25_FCM_16BIT:
        case TextMode.MEGA65_80_X_25_HIRES:
        case TextMode.MEGA65_80_X_25_MULTICOLOR:
        case TextMode.X16_80_X_30:
        case TextMode.X16_80_X_60:
        case TextMode.COMMODORE_128_VDC_80_X_25_HIRES:
          return 80;
        case TextMode.X16_20_X_15:
        case TextMode.X16_20_X_30:
          return 20;
        default:
          Debug.Log( "ScreenWidthInCharacters unsupported mode " + Mode );
          return 40;
      }
    }



    internal static int ScreenHeightInCharacters( TextMode Mode )
    {
      switch ( Mode )
      {
        case TextMode.COMMODORE_40_X_25_ECM:
        case TextMode.COMMODORE_40_X_25_HIRES:
        case TextMode.COMMODORE_40_X_25_MULTICOLOR:
        case TextMode.MEGA65_40_X_25_NCM:
        case TextMode.MEGA65_40_X_25_ECM:
        case TextMode.MEGA65_40_X_25_FCM:
        case TextMode.MEGA65_40_X_25_FCM_16BIT:
        case TextMode.MEGA65_40_X_25_HIRES:
        case TextMode.MEGA65_40_X_25_MULTICOLOR:
        case TextMode.MEGA65_80_X_25_NCM:
        case TextMode.MEGA65_80_X_25_ECM:
        case TextMode.MEGA65_80_X_25_FCM:
        case TextMode.MEGA65_80_X_25_FCM_16BIT:
        case TextMode.MEGA65_80_X_25_HIRES:
        case TextMode.MEGA65_80_X_25_MULTICOLOR:
        case TextMode.COMMODORE_128_VDC_80_X_25_HIRES:
          return 25;
        case TextMode.COMMODORE_VIC20_22_X_23:
          return 23;
        case TextMode.X16_20_X_15:
        case TextMode.X16_40_X_15:
          return 15;
        case TextMode.X16_20_X_30:
        case TextMode.X16_40_X_30:
        case TextMode.X16_80_X_30:
          return 30;
        case TextMode.X16_40_X_60:
        case TextMode.X16_80_X_60:
          return 60;
        default:
          Debug.Log( "ScreenWidthInCharacters unsupported mode " + Mode );
          return 25;
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
        case GraphicTileMode.COMMODORE_MULTICOLOR_SPRITES:
          return SpriteMode.COMMODORE_24_X_21_MULTICOLOR;
        case GraphicTileMode.MEGA65_NCM_SPRITES:
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
          return GraphicTileMode.COMMODORE_MULTICOLOR_SPRITES;
        case SpriteMode.MEGA65_16_X_21_16_COLORS:
        case SpriteMode.MEGA65_64_X_21_16_HIRES:
          return GraphicTileMode.MEGA65_NCM_SPRITES;
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



    internal static MachineType MachineTypeFromTextMode( TextMode Mode )
    {
      switch ( Mode )
      {
        case TextMode.X16_20_X_15:
        case TextMode.X16_20_X_30:
        case TextMode.X16_40_X_15:
        case TextMode.X16_40_X_30:
        case TextMode.X16_40_X_60:
        case TextMode.X16_80_X_30:
        case TextMode.X16_80_X_60:
          return MachineType.COMMANDER_X16;
        case TextMode.COMMODORE_40_X_25_ECM:
        case TextMode.COMMODORE_40_X_25_HIRES:
        case TextMode.COMMODORE_40_X_25_MULTICOLOR:
          return MachineType.C64;
        case TextMode.MEGA65_40_X_25_NCM:
        case TextMode.MEGA65_40_X_25_ECM:
        case TextMode.MEGA65_40_X_25_FCM:
        case TextMode.MEGA65_40_X_25_FCM_16BIT:
        case TextMode.MEGA65_40_X_25_HIRES:
        case TextMode.MEGA65_40_X_25_MULTICOLOR:
        case TextMode.MEGA65_80_X_25_NCM:
        case TextMode.MEGA65_80_X_25_ECM:
        case TextMode.MEGA65_80_X_25_FCM:
        case TextMode.MEGA65_80_X_25_FCM_16BIT:
        case TextMode.MEGA65_80_X_25_HIRES:
        case TextMode.MEGA65_80_X_25_MULTICOLOR:
          return MachineType.MEGA65;
        case TextMode.COMMODORE_VIC20_22_X_23:
          return MachineType.VIC20;
        default:
          Debug.Log( "MachineTypeFromTextMode unsupported mode " + Mode );
          return MachineType.C64;
      }
    }



    internal static bool TextModeInterleavesCharAndColor( TextMode Mode )
    {
      switch ( Mode )
      {
        case TextMode.X16_20_X_15:
        case TextMode.X16_20_X_30:
        case TextMode.X16_40_X_15:
        case TextMode.X16_40_X_30:
        case TextMode.X16_40_X_60:
        case TextMode.X16_80_X_30:
        case TextMode.X16_80_X_60:
          return true;
        default:
          return false;
      }
    }



    internal static int SpriteWidth( SpriteProject.SpriteProjectMode Mode )
    {
      switch ( Mode )
      {
        case SpriteProject.SpriteProjectMode.COMMODORE_24_X_21_HIRES_OR_MC:
          return 24;
        case SpriteProject.SpriteProjectMode.MEGA65_16_X_21_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_8_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_16_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_32_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_64_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_8_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_16_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_32_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_64_256_COLORS:
          return 16;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_8_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_16_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_32_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_64_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_8_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_16_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_32_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_64_256_COLORS:
          return 32;
        case SpriteProject.SpriteProjectMode.MEGA65_64_X_21_HIRES_OR_MC:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_8_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_16_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_32_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_64_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_8_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_16_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_32_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_64_256_COLORS:
          return 64;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_8_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_16_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_32_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_64_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_8_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_16_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_32_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_64_256_COLORS:
          return 8;
        default:
          Debug.Log( "Lookup.SpriteWidth, unsupported mode " + Mode );
          return 64;
      }
    }



    internal static int SpriteHeight( SpriteProject.SpriteProjectMode Mode )
    {
      switch ( Mode )
      {
        case SpriteProject.SpriteProjectMode.COMMODORE_24_X_21_HIRES_OR_MC:
        case SpriteProject.SpriteProjectMode.MEGA65_16_X_21_16_COLORS:
        case SpriteProject.SpriteProjectMode.MEGA65_64_X_21_HIRES_OR_MC:
          return 21;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_8_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_8_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_8_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_8_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_8_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_8_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_8_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_8_256_COLORS:
          return 8;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_16_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_16_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_16_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_16_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_16_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_16_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_16_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_16_256_COLORS:
          return 16;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_32_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_32_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_32_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_32_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_32_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_32_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_32_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_32_256_COLORS:
          return 32;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_64_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_64_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_64_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_64_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_64_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_64_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_64_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_64_256_COLORS:
          return 64;
        default:
          Debug.Log( "Lookup.SpriteHeight, unsupported mode " + Mode );
          return 21;
      }
    }



    internal static GraphicType GraphicImportTypeFromMode( SpriteProject.SpriteProjectMode Mode )
    {
      switch ( Mode )
      {
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_8_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_16_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_32_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_64_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_8_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_16_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_32_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_64_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_8_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_16_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_32_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_64_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_8_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_16_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_32_16_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_64_16_COLORS:
        case SpriteProject.SpriteProjectMode.MEGA65_16_X_21_16_COLORS:
          return GraphicType.SPRITES_16_COLORS;
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_8_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_16_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_32_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_8_64_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_8_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_16_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_32_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_16_64_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_8_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_16_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_32_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_32_64_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_8_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_16_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_32_256_COLORS:
        case SpriteProject.SpriteProjectMode.COMMANDER_X16_64_64_256_COLORS:
          return GraphicType.SPRITES_256_COLORS;
        case SpriteProject.SpriteProjectMode.MEGA65_64_X_21_HIRES_OR_MC:
        case SpriteProject.SpriteProjectMode.COMMODORE_24_X_21_HIRES_OR_MC:
          return GraphicType.SPRITES;
        default:
          Debug.Log( "Lookup.GraphicImportTypeFromMode, unsupported mode " + Mode );
          return GraphicType.SPRITES;
      }
    }



  }

}