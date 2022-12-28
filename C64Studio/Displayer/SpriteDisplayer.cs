using RetroDevStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RetroDevStudio.Displayer
{
  public class SpriteDisplayer
  {
    public static void DisplayHiResSprite( GR.Memory.ByteBuffer Data, Palette Palette, int Width, int Height, int BGColor, int SpriteColor, GR.Image.IImage TargetImage, int X, int Y )
    {
      DisplayHiResSprite( Data, Palette, Width, Height, BGColor, SpriteColor, TargetImage, X, Y, false, false, false );
    }



    public static void DisplayHiResSprite( GR.Memory.ByteBuffer Data, Palette Palette, int Width, int Height, int BGColor, int SpriteColor, GR.Image.IImage Target, int X, int Y, bool ExpandX, bool ExpandY, bool TransparentBackground )
    {
      int     pixelStepX = 1;
      int     pixelStepY = 1;
      if ( ExpandX )
      {
        pixelStepX = 2;
      }
      if ( ExpandY )
      {
        pixelStepY = 2;
      }

      // single color
      for ( int j = 0; j < Height; ++j )
      {
        for ( int pp = 0; pp < pixelStepY; ++pp )
        {
          for ( int k = 0; k < Width / 8; ++k )
          {
            for ( int i = 0; i < 8; ++i )
            {
              if ( ( Data.ByteAt( j * Width / 8 + k ) & ( 1 << ( 7 - i ) ) ) != 0 )
              {
                uint color = Palette.ColorValues[SpriteColor];
                Target.SetPixel( X + ( k * 8 + i ) * pixelStepX, Y + j * pixelStepY + pp, color );
                if ( pixelStepX == 2 )
                {
                  Target.SetPixel( X + ( k * 8 + i ) * pixelStepX + 1, Y + j * pixelStepY + pp, color );
                }
              }
              else if ( !TransparentBackground )
              {
                uint color = Palette.ColorValues[BGColor];

                Target.SetPixel( X + ( k * 8 + i ) * pixelStepX, Y + j * pixelStepY + pp, color );
                if ( pixelStepX == 2 )
                {
                  Target.SetPixel( X + ( k * 8 + i ) * pixelStepX + 1, Y + j * pixelStepY + pp, color );
                }
              }
            }
          }
        }
      }
    }



    public static void DisplayMultiColorSprite( GR.Memory.ByteBuffer Data, Palette Palette, int Width, int Height, int BGColor, int MColor1, int MColor2, int SpriteColor, GR.Image.IImage TargetImage, int X, int Y )
    {
      DisplayMultiColorSprite( Data, Palette, Width, Height, BGColor, MColor1, MColor2, SpriteColor, TargetImage, X, Y, false, false, false );
    }



    public static void DisplayMultiColorSprite( GR.Memory.ByteBuffer Data, Palette Palette, int Width, int Height, int BGColor, int MColor1, int MColor2, int SpriteColor, GR.Image.IImage TargetImage, int X, int Y, bool TransparentBackground )
    {
      DisplayMultiColorSprite( Data, Palette, Width, Height, BGColor, MColor1, MColor2, SpriteColor, TargetImage, X, Y, false, false, TransparentBackground );
    }



    public static void DisplayMultiColorSprite( GR.Memory.ByteBuffer Data, Palette Palette, int Width, int Height, int BGColor, int MColor1, int MColor2, int SpriteColor, GR.Image.IImage Target, int X, int Y, bool ExpandX, bool ExpandY, bool TransparentBackground )
    {
      int     pixelStepX = 1;
      int     pixelStepY = 1;
      if ( ExpandX )
      {
        pixelStepX = 2;
      }
      if ( ExpandY )
      {
        pixelStepY = 2;
      }

      // multicolor
      for ( int j = 0; j < Height; ++j )
      {
        for ( int pp = 0; pp < pixelStepY; ++pp )
        {
          for ( int k = 0; k < Width / 8; ++k )
          {
            for ( int i = 0; i < 4; ++i )
            {
              int pixelValue = ( Data.ByteAt( j * Width / 8 + k ) & ( 3 << ( ( 3 - i ) * 2 ) ) ) >> ( ( 3 - i ) * 2 );

              switch ( pixelValue )
              {
                case 0:
                  if ( TransparentBackground )
                  {
                    continue;
                  }
                  pixelValue = BGColor;
                  break;
                case 1:
                  pixelValue = MColor1;
                  break;
                case 3:
                  pixelValue = MColor2;
                  break;
                case 2:
                  pixelValue = SpriteColor;
                  break;
              }

              if ( ( pixelValue >= 0 )
              &&   ( pixelValue < Palette.ColorValues.Length ) )
              {
                uint color = Palette.ColorValues[pixelValue];

                Target.SetPixel( X + k * 8 * pixelStepX + i * 2 * pixelStepX, Y + j * pixelStepY + pp, color );
                Target.SetPixel( X + k * 8 * pixelStepX + i * 2 * pixelStepX + 1, Y + j * pixelStepY + pp, color );
                if ( pixelStepX == 2 )
                {
                  Target.SetPixel( X + k * 8 * pixelStepX + i * 2 * pixelStepX + 2, Y + j * pixelStepY + pp, color );
                  Target.SetPixel( X + k * 8 * pixelStepX + i * 2 * pixelStepX + 3, Y + j * pixelStepY + pp, color );
                }
              }
            }
          }
        }
      }
    }



    public static void DisplayNCMSprite( GR.Memory.ByteBuffer Data, Palette Palette, int Width, int Height, int BGColor, GR.Image.IImage Target, int X, int Y, bool ExpandX, bool ExpandY, bool TransparentBackground )
    {
      int     pixelStepX = 1;
      int     pixelStepY = 1;
      if ( ExpandX )
      {
        pixelStepX = 2;
      }
      if ( ExpandY )
      {
        pixelStepY = 2;
      }

      int   lineBytes = ( Width + 1 ) / 2;
      for ( int j = 0; j < Height; ++j )
      {
        for ( int pp = 0; pp < pixelStepY; ++pp )
        {
          for ( int i = 0; i < Width; i += 2 )
          {
            byte  pixelDuo = Data.ByteAt( j * lineBytes + i / 2 );

            byte  colorToUse = (byte)BGColor;
            if ( ( pixelDuo >> 4 ) != 0 )
            {
              colorToUse = (byte)( pixelDuo >> 4 );
            }
            else if ( TransparentBackground )
            {
              continue;
            }

            uint color = Palette.ColorValues[colorToUse];

            Target.SetPixel( ( X + i )  * pixelStepX, Y + j * pixelStepY + pp, color );
            if ( pixelStepX == 2 )
            {
              Target.SetPixel( ( X + i ) * pixelStepX + 1, Y + j * pixelStepY + pp, color );
            }

            colorToUse = (byte)BGColor;
            if ( ( pixelDuo & 0x0f ) != 0 )
            {
              colorToUse = (byte)( pixelDuo & 0x0f );
            }

            color = Palette.ColorValues[colorToUse];
            Target.SetPixel( ( X + i + 1 ) * pixelStepX, Y + j * pixelStepY + pp, color );
            if ( pixelStepX == 2 )
            {
              Target.SetPixel( ( X + i + 1 ) * pixelStepX + 1, Y + j * pixelStepY + pp, color );
            }
          }
        }
      }
    }



    public static void DisplayX1616ColorSprite( GR.Memory.ByteBuffer Data, Palette Palette, int PaletteOffset, int Width, int Height, int BGColor, GR.Image.IImage Target, int X, int Y, bool TransparentBackground )
    {
      int     pixelStepX = 1;
      int     pixelStepY = 1;

      int   lineBytes = ( Width + 1 ) / 2;
      for ( int j = 0; j < Height; ++j )
      {
        for ( int pp = 0; pp < pixelStepY; ++pp )
        {
          for ( int i = 0; i < Width; i += 2 )
          {
            byte  pixelDuo = Data.ByteAt( j * lineBytes + i / 2 );

            byte  colorToUse = (byte)BGColor;
            if ( ( pixelDuo >> 4 ) != 0 )
            {
              colorToUse = (byte)( pixelDuo >> 4 );
            }
            else if ( TransparentBackground )
            {
              continue;
            }

            uint color = Palette.ColorValues[PaletteOffset + colorToUse];
            if ( colorToUse == 0 )
            {
              color = Palette.ColorValues[0];
            }

            Target.SetPixel( ( X + i ) * pixelStepX, Y + j * pixelStepY + pp, color );
            if ( pixelStepX == 2 )
            {
              Target.SetPixel( ( X + i ) * pixelStepX + 1, Y + j * pixelStepY + pp, color );
            }

            colorToUse = (byte)BGColor;
            if ( ( pixelDuo & 0x0f ) != 0 )
            {
              colorToUse = (byte)( pixelDuo & 0x0f );
            }

            if ( colorToUse == 0 )
            {
              color = Palette.ColorValues[0];
            }
            else
            {
              color = Palette.ColorValues[PaletteOffset + colorToUse];
            }

            Target.SetPixel( ( X + i + 1 ) * pixelStepX, Y + j * pixelStepY + pp, color );
            if ( pixelStepX == 2 )
            {
              Target.SetPixel( ( X + i + 1 ) * pixelStepX + 1, Y + j * pixelStepY + pp, color );
            }
          }
        }
      }
    }



    public static void DisplayX16256ColorSprite( GR.Memory.ByteBuffer Data, Palette Palette, int PaletteOffset, int Width, int Height, int BGColor, GR.Image.IImage Target, int X, int Y, bool TransparentBackground )
    {
      int   lineBytes = Width;
      for ( int j = 0; j < Height; ++j )
      {
        for ( int i = 0; i < Width; ++i )
        {
          byte  pixel = Data.ByteAt( j * lineBytes + i );

          if ( ( pixel == 0 )
          &&   ( TransparentBackground ) )
          {
            continue;
          }

          // palette offset only affects colors 0 to 15
          if ( ( pixel < 16 )
          &&   ( pixel > 0 ) )
          {
            pixel = (byte)( pixel + PaletteOffset );
          }

          Target.SetPixel( X + i, Y + j, Palette.ColorValues[pixel] );
        }
      }
    }

  }
}
