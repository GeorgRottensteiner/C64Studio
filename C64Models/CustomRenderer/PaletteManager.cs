using System;

namespace RetroDevStudio
{
  internal class PaletteManager
  {
    public static Palette PaletteFromMachine( MachineType Machine )
    {
      switch ( Machine )
      {
        case MachineType.C64:
        case MachineType.C128:
        default:
          return ConstantData.PaletteC64();
        case MachineType.MEGA65:
          return ConstantData.PaletteMega65();
        case MachineType.VC20:
          return ConstantData.PaletteVC20();
      }
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



    internal static Palette PaletteFromNumColors( int NumColors )
    {
      switch ( NumColors )
      {
        case 16:
        default:
          return ConstantData.PaletteC64();
        case 256:
          return ConstantData.PaletteMega65();
      }
    }



  }
}
