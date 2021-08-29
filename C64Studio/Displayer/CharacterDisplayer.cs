using C64Studio.Controls;
using RetroDevStudioModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio.Displayer
{
  public class CharacterDisplayer
  {
    public static void DisplayHiResChar( GR.Memory.ByteBuffer Data, int BGColor, int CharColor, CustomDrawControlContext Context )
    {
      // single color
      int colorIndex = 0;
      for ( int j = 0; j < 8; ++j )
      {
        for ( int i = 0; i < 8; ++i )
        {
          if ( ( Data.ByteAt( j ) & ( 1 << ( 7 - i ) ) ) != 0 )
          {
            colorIndex = CharColor;
          }
          else
          {
            colorIndex = BGColor;
          }
          Context.Graphics.FillRectangle( Types.ConstantData.Palette.ColorBrushes[colorIndex], 
                                          ( i * Context.Bounds.Width ) / 8,
                                          ( j * Context.Bounds.Height ) / 8,
                                          ( ( i + 1 ) * Context.Bounds.Width ) / 8 - ( i * Context.Bounds.Width ) / 8,
                                          ( ( j + 1 ) * Context.Bounds.Height ) / 8 - ( j * Context.Bounds.Height ) / 8 );
        }
      }
    }



    public static void DisplayMultiColorChar( GR.Memory.ByteBuffer Data, int BGColor, int MColor1, int MColor2, int CharColor, CustomDrawControlContext Context )
    {
      // multicolor
      if ( CharColor < 8 )
      {
        DisplayHiResChar( Data, BGColor, CharColor, Context );
        return;
      }

      // single color
      int charColor = CharColor - 8;

      for ( int j = 0; j < 8; ++j )
      {
        for ( int i = 0; i < 4; ++i )
        {
          int pixelValue = ( Data.ByteAt( j ) & ( 3 << ( ( 3 - i ) * 2 ) ) ) >> ( ( 3 - i ) * 2 );

          switch ( pixelValue )
          {
            case 0:
              pixelValue = BGColor;
              break;
            case 1:
              pixelValue = MColor1;
              break;
            case 2:
              pixelValue = MColor2;
              break;
            case 3:
              pixelValue = charColor;
              break;
          }
          Context.Graphics.FillRectangle( Types.ConstantData.Palette.ColorBrushes[pixelValue],
                                          ( i * Context.Bounds.Width ) / 4,
                                          ( j * Context.Bounds.Height ) / 8,
                                          ( ( i + 1 ) * Context.Bounds.Width ) / 4 - ( i * Context.Bounds.Width ) / 4,
                                          ( ( j + 1 ) * Context.Bounds.Height ) / 8 - ( j * Context.Bounds.Height ) / 8 );
        }
      }
    }



    public static void DisplayHiResChar( GR.Memory.ByteBuffer Data, int BGColor, int CharColor, GR.Image.IImage TargetImage, int X, int Y )
    {
      // single color
      int colorIndex = 0;
      for ( int j = 0; j < 8; ++j )
      {
        for ( int i = 0; i < 8; ++i )
        {
          if ( ( Data.ByteAt( j ) & ( 1 << ( 7 - i ) ) ) != 0 )
          {
            colorIndex = CharColor;
          }
          else
          {
            colorIndex = BGColor;
          }
          TargetImage.SetPixel( X + i, Y + j, (uint)colorIndex );
        }
      }
    }



    public static void DisplayMultiColorChar( GR.Memory.ByteBuffer Data, int BGColor, int MColor1, int MColor2, int CharColor, GR.Image.IImage TargetImage, int X, int Y )
    {
      // multicolor
      if ( CharColor < 8 )
      {
        DisplayHiResChar( Data, BGColor, CharColor, TargetImage, X, Y );
        return;
      }

      int charColor = CharColor - 8;

      for ( int j = 0; j < 8; ++j )
      {
        for ( int i = 0; i < 4; ++i )
        {
          int pixelValue = ( Data.ByteAt( j ) & ( 3 << ( ( 3 - i ) * 2 ) ) ) >> ( ( 3 - i ) * 2 );

          switch ( pixelValue )
          {
            case 0:
              pixelValue = BGColor;
              break;
            case 1:
              pixelValue = MColor1;
              break;
            case 2:
              pixelValue = MColor2;
              break;
            case 3:
              pixelValue = charColor;
              break;
          }
          TargetImage.SetPixel( X + i * 2, Y + j, (uint)pixelValue );
          TargetImage.SetPixel( X + i * 2 + 1, Y + j, (uint)pixelValue );
        }
      }
    }



    public static void DisplayChar( Formats.CharsetProject Charset, int CharIndex, CustomDrawControlContext Context )
    {
      Formats.CharData Char = Charset.Characters[CharIndex];

      DisplayChar( Charset, CharIndex, Context, Char.Color );
    }



    public static void DisplayChar( Formats.CharsetProject Charset, int CharIndex, CustomDrawControlContext Context, int AlternativeColor )
    {
      DisplayChar( Charset, CharIndex, Context, AlternativeColor, Charset.BackgroundColor, Charset.MultiColor1, Charset.MultiColor2, Charset.BGColor4 );
    }




    public static void DisplayChar( Formats.CharsetProject Charset, int CharIndex, CustomDrawControlContext Context, int AlternativeColor, int AltBGColor, int AltMColor1, int AltMColor2, int AltBGColor4 )
    {
      Formats.CharData Char = Charset.Characters[CharIndex];

      DisplayChar( Charset, CharIndex, Context, AlternativeColor, AltBGColor, AltMColor1, AltMColor2, AltBGColor4, Char.Mode );
    }



    public static void DisplayChar( Formats.CharsetProject Charset, int CharIndex, CustomDrawControlContext Context, int AlternativeColor, int AltBGColor, int AltMColor1, int AltMColor2, int AltBGColor4, TextMode AlternativeMode )
    {
      Formats.CharData Char = Charset.Characters[CharIndex];

      if ( AlternativeMode == TextMode.COMMODORE_40_X_25_ECM )
      {
        // ECM
        Formats.CharData origChar = Charset.Characters[CharIndex % 64];

        int bgColor = AltBGColor;
        switch ( CharIndex / 64 )
        {
          case 1:
            bgColor = AltMColor1;
            break;
          case 2:
            bgColor = AltMColor2;
            break;
          case 3:
            bgColor = AltBGColor4;
            break;
        }
        DisplayHiResChar( origChar.Data, bgColor, AlternativeColor, Context );
      }
      else if ( AlternativeMode == TextMode.COMMODORE_40_X_25_MULTICOLOR )
      {
        DisplayMultiColorChar( Char.Data, AltBGColor, AltMColor1, AltMColor2, AlternativeColor, Context );
      }
      else if ( AlternativeMode == TextMode.COMMODORE_40_X_25_HIRES )
      {
        DisplayHiResChar( Char.Data, AltBGColor, AlternativeColor, Context );
      }
    }



    public static void DisplayChar( Formats.CharsetProject Charset, int CharIndex, GR.Image.IImage TargetImage, int X, int Y )
    {
      Formats.CharData Char = Charset.Characters[CharIndex];

      DisplayChar( Charset, CharIndex, TargetImage, X, Y, Char.Color );
    }



    public static void DisplayChar( Formats.CharsetProject Charset, int CharIndex, GR.Image.IImage TargetImage, int X, int Y, int AlternativeColor )
    {
      DisplayChar( Charset, CharIndex, TargetImage, X, Y, AlternativeColor, Charset.BackgroundColor, Charset.MultiColor1, Charset.MultiColor2, Charset.BGColor4 );
    }




    public static void DisplayChar( Formats.CharsetProject Charset, int CharIndex, GR.Image.IImage TargetImage, int X, int Y, int AlternativeColor, int AltBGColor, int AltMColor1, int AltMColor2, int AltBGColor4 )
    {
      Formats.CharData Char = Charset.Characters[CharIndex];

      DisplayChar( Charset, CharIndex, TargetImage, X, Y, AlternativeColor, AltBGColor, AltMColor1, AltMColor2, AltBGColor4, Char.Mode );
    }



    public static void DisplayChar( Formats.CharsetProject Charset, int CharIndex, GR.Image.IImage TargetImage, int X, int Y, int AlternativeColor, int AltBGColor, int AltMColor1, int AltMColor2, int AltBGColor4, TextMode AlternativeMode )
    {
      Formats.CharData Char = Charset.Characters[CharIndex];

      if ( AlternativeMode == TextMode.COMMODORE_40_X_25_ECM )
      {
        // ECM
        Formats.CharData origChar = Charset.Characters[CharIndex % 64];

        int bgColor = AltBGColor;
        switch ( CharIndex / 64 )
        {
          case 1:
            bgColor = AltMColor1;
            break;
          case 2:
            bgColor = AltMColor2;
            break;
          case 3:
            bgColor = AltBGColor4;
            break;
        }
        DisplayHiResChar( origChar.Data, bgColor, AlternativeColor, TargetImage, X, Y );
      }
      else if ( ( AlternativeMode == TextMode.COMMODORE_40_X_25_MULTICOLOR )
      ||        ( AlternativeMode == TextMode.MEGA65_80_X_25_MULTICOLOR ) )
      {
        DisplayMultiColorChar( Char.Data, AltBGColor, AltMColor1, AltMColor2, AlternativeColor, TargetImage, X, Y );
      }
      else if ( ( AlternativeMode == TextMode.COMMODORE_40_X_25_HIRES )
      ||        ( AlternativeMode == TextMode.MEGA65_80_X_25_HIRES ) )
      {
        DisplayHiResChar( Char.Data, AltBGColor, AlternativeColor, TargetImage, X, Y );
      }
      else
      {
        Debug.Log( "DisplayChar unsupported mode " + AlternativeMode );
      }
    }

  }
}
