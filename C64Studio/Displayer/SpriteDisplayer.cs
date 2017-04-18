using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio.Displayer
{
  public class SpriteDisplayer
  {
    public static void DisplayHiResSprite( GR.Memory.ByteBuffer Data, int BGColor, int SpriteColor, GR.Image.IImage TargetImage, int X, int Y )
    {
      DisplayHiResSprite( Data, BGColor, SpriteColor, TargetImage, X, Y, false, false );
    }



    public static void DisplayHiResSprite( GR.Memory.ByteBuffer Data, int BGColor, int SpriteColor, GR.Image.IImage Target, int X, int Y, bool ExpandX, bool ExpandY )
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
      for ( int j = 0; j < 21; ++j )
      {
        for ( int pp = 0; pp < pixelStepY; ++pp )
        {
          for ( int k = 0; k < 3; ++k )
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
            }
          }
        }
      }
    }



    public static void DisplayMultiColorSprite( GR.Memory.ByteBuffer Data, int BGColor, int MColor1, int MColor2, int SpriteColor, GR.Image.IImage TargetImage, int X, int Y )
    {
      DisplayMultiColorSprite( Data, BGColor, MColor1, MColor2, SpriteColor, TargetImage, X, Y, false, false );
    }



    public static void DisplayMultiColorSprite( GR.Memory.ByteBuffer Data, int BGColor, int MColor1, int MColor2, int SpriteColor, GR.Image.IImage Target, int X, int Y, bool ExpandX, bool ExpandY )
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
      for ( int j = 0; j < 21; ++j )
      {
        for ( int pp = 0; pp < pixelStepY; ++pp )
        {
          for ( int k = 0; k < 3; ++k )
          {
            for ( int i = 0; i < 4; ++i )
            {
              int pixelValue = ( Data.ByteAt( j * 3 + k ) & ( 3 << ( ( 3 - i ) * 2 ) ) ) >> ( ( 3 - i ) * 2 );

              switch ( pixelValue )
              {
                case 0:
                  //pixelValue = BackgroundColor;
                  continue;
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

  }
}
