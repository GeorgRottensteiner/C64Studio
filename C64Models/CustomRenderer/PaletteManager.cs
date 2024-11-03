using RetroDevStudio.Formats;
using System;

namespace RetroDevStudio
{
  internal class PaletteManager
  {
    public static Palette PaletteFromType( PaletteType Type )
    {
      switch ( Type )
      {
        case PaletteType.COMMANDER_X16:
          return ConstantData.DefaultPaletteCommanderX16();
        case PaletteType.MEGA65:
          return ConstantData.DefaultPaletteMega65_256();
        case PaletteType.C64:
          return ConstantData.DefaultPaletteC64();
        case PaletteType.VIC20:
          return ConstantData.DefaultPaletteVIC20();
        case PaletteType.C128_VDC:
          return ConstantData.DefaultPaletteC128();
        case PaletteType.NES:
          return ConstantData.DefaultPaletteNES();
      }
      return ConstantData.DefaultPaletteC64();
    }



    public static Palette PaletteFromMode( TextCharMode Mode )
    {
      switch ( Mode )
      {
        case TextCharMode.X16_HIRES:
          return ConstantData.DefaultPaletteCommanderX16();
        case TextCharMode.MEGA65_NCM:
        case TextCharMode.MEGA65_FCM:
        case TextCharMode.MEGA65_FCM_16BIT:
        case TextCharMode.MEGA65_HIRES:
        case TextCharMode.MEGA65_ECM:
          return ConstantData.DefaultPaletteMega65_256();
        case TextCharMode.COMMODORE_ECM:
        case TextCharMode.COMMODORE_HIRES:
        case TextCharMode.COMMODORE_MULTICOLOR:
          return ConstantData.DefaultPaletteC64();
        case TextCharMode.VIC20:
        case TextCharMode.VIC20_8X16:
          return ConstantData.DefaultPaletteVIC20();
        case TextCharMode.COMMODORE_128_VDC_HIRES:
          return ConstantData.DefaultPaletteC128();
        case TextCharMode.NES:
          return ConstantData.DefaultPaletteNES();
        default:
          Debug.Log( $"PaletteFromMode unsupported mode {Mode}" );
          break;
      }
      return ConstantData.DefaultPaletteC64();
    }



    public static void ApplyPalette( GR.Image.IImage Image )
    {
      ApplyPalette( Image, ConstantData.Palette );
    }



    public static void ApplyPalette( GR.Image.IImage Image, Palette Palette )
    {
      for ( int i = 0; i < Palette.NumColors; ++i )
      {
        Image.SetPaletteColor( i,
                               (byte)( ( Palette.ColorValues[i] & 0x00ff0000 ) >> 16 ),
                               (byte)( ( Palette.ColorValues[i] & 0x0000ff00 ) >> 8 ),
                               (byte)( Palette.ColorValues[i] & 0xff ) );
      }
    }



    public static Palette PaletteFromMode( SpriteProject.SpriteProjectMode Mode )
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
          return ConstantData.DefaultPaletteCommanderX16();
        case SpriteProject.SpriteProjectMode.MEGA65_16_X_21_16_COLORS:
        case SpriteProject.SpriteProjectMode.MEGA65_64_X_21_HIRES_OR_MC:
          return ConstantData.DefaultPaletteMega65_256();
        case SpriteProject.SpriteProjectMode.COMMODORE_24_X_21_HIRES_OR_MC:
          return ConstantData.DefaultPaletteC64();
      }
      return ConstantData.DefaultPaletteC64();
    }



  }
}
