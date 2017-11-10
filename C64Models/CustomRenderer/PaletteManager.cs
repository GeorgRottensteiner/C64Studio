using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio.CustomRenderer
{
  internal class PaletteManager
  {
    public static void ApplyPalette( GR.Image.MemoryImage Image )
    {
      for ( int i = 0; i < 17; ++i )
      {
        Image.SetPaletteColor( i,
                               (byte)( ( Types.ConstantData.Palette.ColorValues[i] & 0x00ff0000 ) >> 16 ),
                               (byte)( ( Types.ConstantData.Palette.ColorValues[i] & 0x0000ff00 ) >> 8 ),
                               (byte)( Types.ConstantData.Palette.ColorValues[i] & 0xff ) );
      }
    }



    internal static void ApplyPalette( GR.Image.FastImage Image )
    {
      for ( int i = 0; i < 17; ++i )
      {
        Image.SetPaletteColor( i,
                               (byte)( ( Types.ConstantData.Palette.ColorValues[i] & 0x00ff0000 ) >> 16 ),
                               (byte)( ( Types.ConstantData.Palette.ColorValues[i] & 0x0000ff00 ) >> 8 ),
                               (byte)( Types.ConstantData.Palette.ColorValues[i] & 0xff ) );
      }
    }

  }
}
