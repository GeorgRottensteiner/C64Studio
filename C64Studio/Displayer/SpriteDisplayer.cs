using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio.Displayer
{
  public class SpriteDisplayer
  {
    public static void DisplayHiResSprite( GR.Memory.ByteBuffer Data, int Width, int Height, int BGColor, int SpriteColor, GR.Image.IImage TargetImage, int X, int Y )
    {
      DisplayHiResSprite( Data, Width, Height, BGColor, SpriteColor, TargetImage, X, Y, false, false );
    }



    public static void DisplayHiResSprite( GR.Memory.ByteBuffer Data, int Width, int Height, int BGColor, int SpriteColor, GR.Image.IImage Target, int X, int Y, bool ExpandX, bool ExpandY )
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
              if ( ( Data.ByteAt( j * 3 + k ) & ( 1 << ( 7 - i ) ) ) != 0 )
              {
                //Data.Image.SetPixel( k * 8 + i, j, m_ColorValues[Data.Color] );
                Target.SetPixel( X + ( k * 8 + i ) * pixelStepX, Y + j * pixelStepY + pp, (uint)SpriteColor );
                if ( pixelStepX == 2 )
                {
                  Target.SetPixel( X + ( k * 8 + i ) * pixelStepX + 1, Y + j * pixelStepY + pp, (uint)SpriteColor );
                }
              }
              else
              {
                Target.SetPixel( X + ( k * 8 + i ) * pixelStepX, Y + j * pixelStepY + pp, (uint)BGColor );
                if ( pixelStepX == 2 )
                {
                  Target.SetPixel( X + ( k * 8 + i ) * pixelStepX + 1, Y + j * pixelStepY + pp, (uint)BGColor );
                }
              }
            }
          }
        }
      }
    }



    public static void DisplayMultiColorSprite( GR.Memory.ByteBuffer Data, int Width, int Height, int BGColor, int MColor1, int MColor2, int SpriteColor, GR.Image.IImage TargetImage, int X, int Y )
    {
      DisplayMultiColorSprite( Data, Width, Height, BGColor, MColor1, MColor2, SpriteColor, TargetImage, X, Y, false, false );
    }



    public static void DisplayMultiColorSprite( GR.Memory.ByteBuffer Data, int Width, int Height, int BGColor, int MColor1, int MColor2, int SpriteColor, GR.Image.IImage Target, int X, int Y, bool ExpandX, bool ExpandY )
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
              int pixelValue = ( Data.ByteAt( j * 3 + k ) & ( 3 << ( ( 3 - i ) * 2 ) ) ) >> ( ( 3 - i ) * 2 );

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
              Target.SetPixel( X + k * 8 * pixelStepX + i * 2 * pixelStepX, Y + j * pixelStepY + pp, (uint)pixelValue );
              Target.SetPixel( X + k * 8 * pixelStepX + i * 2 * pixelStepX + 1, Y + j * pixelStepY + pp, (uint)pixelValue );
              if ( pixelStepX == 2 )
              {
                Target.SetPixel( X + k * 8 * pixelStepX + i * 2 * pixelStepX + 2, Y + j * pixelStepY + pp, (uint)pixelValue );
                Target.SetPixel( X + k * 8 * pixelStepX + i * 2 * pixelStepX + 3, Y + j * pixelStepY + pp, (uint)pixelValue );
              }
            }
          }
        }
      }
    }



    public static void DisplayFCMSprite( GR.Memory.ByteBuffer Data, int Width, int Height, int BGColor, GR.Image.IImage Target, int X, int Y, bool ExpandX, bool ExpandY )
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
            Target.SetPixel( ( X + i )  * pixelStepX, Y + j * pixelStepY + pp, colorToUse );
            if ( pixelStepX == 2 )
            {
              Target.SetPixel( ( X + i ) * pixelStepX + 1, Y + j * pixelStepY + pp, colorToUse );
            }

            colorToUse = (byte)BGColor;
            if ( ( pixelDuo & 0x0f ) != 0 )
            {
              colorToUse = (byte)( pixelDuo & 0x0f );
            }
            Target.SetPixel( ( X + i + 1 ) * pixelStepX, Y + j * pixelStepY + pp, colorToUse );
            if ( pixelStepX == 2 )
            {
              Target.SetPixel( ( X + i + 1 ) * pixelStepX + 1, Y + j * pixelStepY + pp, colorToUse );
            }
          }
        }
      }
    }



  }
}
