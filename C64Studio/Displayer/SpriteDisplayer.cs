using RetroDevStudio;
using System;
using System.Collections.Generic;
using System.Text;

namespace RetroDevStudio.Displayer
{
  public class SpriteDisplayer
  {
    public static void DisplayHiResSprite( GR.Memory.ByteBuffer Data, Palette Palette, int Width, int Height, int BGColor, int SpriteColor, GR.Image.IImage TargetImage, int X, int Y )
    {
      DisplayHiResSprite( Data, Palette, Width, Height, BGColor, SpriteColor, TargetImage, X, Y, false, false );
    }



    public static void DisplayHiResSprite( GR.Memory.ByteBuffer Data, Palette Palette, int Width, int Height, int BGColor, int SpriteColor, GR.Image.IImage Target, int X, int Y, bool ExpandX, bool ExpandY )
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
              else
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
      DisplayMultiColorSprite( Data, Palette, Width, Height, BGColor, MColor1, MColor2, SpriteColor, TargetImage, X, Y, false, false );
    }



    public static void DisplayMultiColorSprite( GR.Memory.ByteBuffer Data, Palette Palette, int Width, int Height, int BGColor, int MColor1, int MColor2, int SpriteColor, GR.Image.IImage Target, int X, int Y, bool ExpandX, bool ExpandY )
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



    public static void DisplayNCMSprite( GR.Memory.ByteBuffer Data, Palette Palette, int Width, int Height, int BGColor, GR.Image.IImage Target, int X, int Y, bool ExpandX, bool ExpandY )
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



  }
}
