using RetroDevStudio.Controls;
using RetroDevStudio;
using System;
using System.Collections.Generic;
using System.Text;



namespace RetroDevStudio.Displayer
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
          Context.Graphics.FillRectangle( Context.Palette.ColorBrushes[colorIndex], 
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
          Context.Graphics.FillRectangle( Context.Palette.ColorBrushes[pixelValue],
                                          ( i * Context.Bounds.Width ) / 4,
                                          ( j * Context.Bounds.Height ) / 8,
                                          ( ( i + 1 ) * Context.Bounds.Width ) / 4 - ( i * Context.Bounds.Width ) / 4,
                                          ( ( j + 1 ) * Context.Bounds.Height ) / 8 - ( j * Context.Bounds.Height ) / 8 );
        }
      }
    }



    public static void DisplayVIC20Char( GR.Memory.ByteBuffer Data, int BGColor, int BorderColor, int AuxiliaryColor, int CharColor, CustomDrawControlContext Context )
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
              pixelValue = BorderColor;
              break;
            case 2:
              pixelValue = charColor;
              break;
            case 3:
              pixelValue = AuxiliaryColor;
              break;
          }
          Context.Graphics.FillRectangle( Context.Palette.ColorBrushes[pixelValue],
                                          ( i * Context.Bounds.Width ) / 4,
                                          ( j * Context.Bounds.Height ) / 8,
                                          ( ( i + 1 ) * Context.Bounds.Width ) / 4 - ( i * Context.Bounds.Width ) / 4,
                                          ( ( j + 1 ) * Context.Bounds.Height ) / 8 - ( j * Context.Bounds.Height ) / 8 );
        }
      }
    }



    public static void DisplayMega65FCMChar( GR.Memory.ByteBuffer Data, int BGColor, int CharColor, CustomDrawControlContext Context )
    {
      // single color
      for ( int j = 0; j < 8; ++j )
      {
        for ( int i = 0; i < 8; ++i )
        {
          int colorIndex = Data.ByteAt( i + j * 8 );
          if ( colorIndex < Context.Palette.ColorBrushes.Length )
          {
            if ( colorIndex == 0 )
            {
              colorIndex = BGColor;
            }
            Context.Graphics.FillRectangle( Context.Palette.ColorBrushes[colorIndex],
                                            ( i * Context.Bounds.Width ) / 8,
                                            ( j * Context.Bounds.Height ) / 8,
                                            ( ( i + 1 ) * Context.Bounds.Width ) / 8 - ( i * Context.Bounds.Width ) / 8,
                                            ( ( j + 1 ) * Context.Bounds.Height ) / 8 - ( j * Context.Bounds.Height ) / 8 );
          }
        }
      }
    }



    public static void DisplayMega65NCMChar( GR.Memory.ByteBuffer Data, int BGColor, int CharColor, CustomDrawControlContext Context )
    {
      // single color
      for ( int j = 0; j < 8; ++j )
      {
        for ( int i = 0; i < 16; ++i )
        {
          int  byteValue = Data.ByteAt( i / 2 + j * 8 );
          if ( ( i % 2 ) != 0 )
          {
            byteValue >>= 4;
          }
          else
          {
            byteValue &= 0x0f;
          }
          if ( byteValue < Context.Palette.ColorBrushes.Length )
          {
            if ( byteValue == 0 )
            {
              byteValue = BGColor;
            }
            Context.Graphics.FillRectangle( Context.Palette.ColorBrushes[byteValue],
                                            ( i * Context.Bounds.Width ) / 16,
                                            ( j * Context.Bounds.Height ) / 8,
                                            ( ( i + 1 ) * Context.Bounds.Width ) / 16 - ( i * Context.Bounds.Width ) / 16,
                                            ( ( j + 1 ) * Context.Bounds.Height ) / 8 - ( j * Context.Bounds.Height ) / 8 );
          }
        }
      }
    }



    public static void DisplayHiResChar( GR.Memory.ByteBuffer Data, Palette Palette, int BGColor, int CharColor, GR.Image.IImage TargetImage, int X, int Y )
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

          uint color = Palette.ColorValues[colorIndex];
          TargetImage.SetPixel( X + i, Y + j, color );
        }
      }
    }



    public static void DisplayMultiColorChar( GR.Memory.ByteBuffer Data, Palette Palette, int BGColor, int MColor1, int MColor2, int CharColor, GR.Image.IImage TargetImage, int X, int Y )
    {
      // multicolor
      if ( CharColor < 8 )
      {
        DisplayHiResChar( Data, Palette, BGColor, CharColor, TargetImage, X, Y );
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

          uint color = Palette.ColorValues[pixelValue];
          TargetImage.SetPixel( X + i * 2, Y + j, color );
          TargetImage.SetPixel( X + i * 2 + 1, Y + j, color );
        }
      }
    }



    public static void DisplayVIC20Char( GR.Memory.ByteBuffer Data, Palette Palette, int BGColor, int MColor1, int MColor2, int CharColor, GR.Image.IImage TargetImage, int X, int Y )
    {
      // multicolor
      if ( CharColor < 8 )
      {
        DisplayHiResChar( Data, Palette, BGColor, CharColor, TargetImage, X, Y );
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
              // border color(!)
              pixelValue = MColor1;
              break;
            case 2:
              pixelValue = charColor;
              break;
            case 3:
              pixelValue = MColor2;
              break;
          }

          uint color = Palette.ColorValues[pixelValue];
          TargetImage.SetPixel( X + i * 2, Y + j, color );
          TargetImage.SetPixel( X + i * 2 + 1, Y + j, color );
        }
      }
    }



    public static void DisplayMega65FCMChar( GR.Memory.ByteBuffer Data, Palette Palette, int BGColor, int CharColor, GR.Image.IImage TargetImage, int X, int Y )
    {
      for ( int j = 0; j < 8; ++j )
      {
        for ( int i = 0; i < 8; ++i )
        {
          int colorIndex = Data.ByteAt( i + j * 8 );
          if ( colorIndex == 0 )
          {
            colorIndex = BGColor;
          }
          TargetImage.SetPixel( X + i, Y + j, Palette.ColorValues[colorIndex] );
        }
      }
    }



    public static void DisplayMega65NCMChar( GR.Memory.ByteBuffer Data, Palette Palette, int BGColor, int CharColor, GR.Image.IImage TargetImage, int X, int Y )
    {
      for ( int j = 0; j < 8; ++j )
      {
        for ( int i = 0; i < 16; ++i )
        {
          int  colorIndex = Data.ByteAt( i / 2 + j * 8 );
          if ( ( i % 2 ) != 0 )
          {
            colorIndex >>= 4;
          }
          else
          {
            colorIndex &= 0x0f;
          }
          if ( colorIndex == 0 )
          {
            colorIndex = BGColor;
          }
          TargetImage.SetPixel( X + i, Y + j, Palette.ColorValues[colorIndex] );
        }
      }
    }



    public static void DisplayChar( Formats.CharsetProject Charset, int CharIndex, CustomDrawControlContext Context )
    {
      if ( CharIndex >= Charset.Characters.Count )
      {
        Debug.Log( $"Trying to display character {CharIndex} of {Charset.Characters.Count}" );
        return;
      }

      Formats.CharData Char = Charset.Characters[CharIndex];

      DisplayChar( Charset, CharIndex, Context, Char.Tile.CustomColor );
    }



    public static void DisplayChar( Formats.CharsetProject Charset, int CharIndex, CustomDrawControlContext Context, int AlternativeColor )
    {
      DisplayChar( Charset, CharIndex, Context, AlternativeColor, Charset.Colors.BackgroundColor, Charset.Colors.MultiColor1, Charset.Colors.MultiColor2, Charset.Colors.BGColor4 );
    }




    public static void DisplayChar( Formats.CharsetProject Charset, int CharIndex, CustomDrawControlContext Context, int AlternativeColor, int AltBGColor, int AltMColor1, int AltMColor2, int AltBGColor4 )
    {
      Formats.CharData Char = Charset.Characters[CharIndex];

      DisplayChar( Charset, CharIndex, Context, AlternativeColor, AltBGColor, AltMColor1, AltMColor2, AltBGColor4, Charset.Mode );
    }



    public static void DisplayChar( Formats.CharsetProject Charset, int CharIndex, CustomDrawControlContext Context, int AlternativeColor, int AltBGColor, int AltMColor1, int AltMColor2, int AltBGColor4, TextCharMode AlternativeMode )
    {
      Formats.CharData Char = Charset.Characters[CharIndex];

      if ( ( AlternativeMode == TextCharMode.COMMODORE_ECM )
      ||   ( AlternativeMode == TextCharMode.MEGA65_ECM ) )
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
        DisplayHiResChar( origChar.Tile.Data, bgColor, AlternativeColor, Context );
      }
      else if ( AlternativeMode == TextCharMode.COMMODORE_MULTICOLOR )
      {
        DisplayMultiColorChar( Char.Tile.Data, AltBGColor, AltMColor1, AltMColor2, AlternativeColor, Context );
      }
      else if ( ( AlternativeMode == TextCharMode.COMMODORE_HIRES )
      ||        ( AlternativeMode == TextCharMode.MEGA65_HIRES ) )
      {
        DisplayHiResChar( Char.Tile.Data, AltBGColor, AlternativeColor, Context );
      }
      else if ( ( AlternativeMode == TextCharMode.MEGA65_FCM )
      ||        ( AlternativeMode == TextCharMode.MEGA65_FCM_16BIT ) )
      {
        DisplayMega65FCMChar( Char.Tile.Data, AltBGColor, AlternativeColor, Context );
      }
      else if ( AlternativeMode == TextCharMode.MEGA65_NCM )
      {
        DisplayMega65NCMChar( Char.Tile.Data, AltBGColor, AlternativeColor, Context );
      }
      else if ( AlternativeMode == TextCharMode.VIC20 )
      {
        DisplayVIC20Char( Char.Tile.Data, AltBGColor, AltMColor1, AltMColor2, AlternativeColor, Context );
      }
      else if ( AlternativeMode == TextCharMode.X16_HIRES )
      {
        DisplayHiResChar( Char.Tile.Data, ( AlternativeColor >> 4 ) & 0x0f, AlternativeColor & 0x0f, Context );
      }
      else
      {
        Debug.Log( "DisplayChar unsupported mode " + AlternativeMode );
      }
    }



    public static void DisplayChar( Formats.CharsetProject Charset, Palette Palette, int CharIndex, GR.Image.IImage TargetImage, int X, int Y )
    {
      Formats.CharData Char = Charset.Characters[CharIndex];

      DisplayChar( Charset, Palette, CharIndex, TargetImage, X, Y, Char.Tile.CustomColor );
    }



    public static void DisplayChar( Formats.CharsetProject Charset, Palette Palette, int CharIndex, GR.Image.IImage TargetImage, int X, int Y, int AlternativeColor )
    {
      DisplayChar( Charset, Palette, CharIndex, TargetImage, X, Y, AlternativeColor, Charset.Colors.BackgroundColor, Charset.Colors.MultiColor1, Charset.Colors.MultiColor2, Charset.Colors.BGColor4 );
    }




    public static void DisplayChar( Formats.CharsetProject Charset, Palette Palette, int CharIndex, GR.Image.IImage TargetImage, int X, int Y, int AlternativeColor, int AltBGColor, int AltMColor1, int AltMColor2, int AltBGColor4 )
    {
      if ( CharIndex >= Charset.Characters.Count )
      {
        return;
      }

      Formats.CharData Char = Charset.Characters[CharIndex];

      DisplayChar( Charset, Palette, CharIndex, TargetImage, X, Y, AlternativeColor, AltBGColor, AltMColor1, AltMColor2, AltBGColor4, Charset.Mode );
    }



    public static void DisplayChar( Formats.CharsetProject Charset, Palette Palette, int CharIndex, GR.Image.IImage TargetImage, int X, int Y, int AlternativeColor, int AltBGColor, int AltMColor1, int AltMColor2, int AltBGColor4, TextCharMode AlternativeMode )
    {
      Formats.CharData Char = Charset.Characters[CharIndex];

      if ( ( AlternativeMode == TextCharMode.COMMODORE_ECM )
      ||   ( AlternativeMode == TextCharMode.MEGA65_ECM ) )
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
        DisplayHiResChar( origChar.Tile.Data, Palette, bgColor, AlternativeColor, TargetImage, X, Y );
      }
      else if ( AlternativeMode == TextCharMode.COMMODORE_MULTICOLOR )
      {
        DisplayMultiColorChar( Char.Tile.Data, Palette, AltBGColor, AltMColor1, AltMColor2, AlternativeColor, TargetImage, X, Y );
      }
      else if ( ( AlternativeMode == TextCharMode.COMMODORE_HIRES )
      ||        ( AlternativeMode == TextCharMode.MEGA65_HIRES ) )
      {
        DisplayHiResChar( Char.Tile.Data, Palette, AltBGColor, AlternativeColor, TargetImage, X, Y );
      }
      else if ( ( AlternativeMode == TextCharMode.MEGA65_FCM )
      ||        ( AlternativeMode == TextCharMode.MEGA65_FCM_16BIT ) )
      {
        DisplayMega65FCMChar( Char.Tile.Data, Palette, AltBGColor, AlternativeColor, TargetImage, X, Y );
      }
      else if ( AlternativeMode == TextCharMode.MEGA65_NCM )
      {
        DisplayMega65NCMChar( Char.Tile.Data, Palette, AltBGColor, AlternativeColor, TargetImage, X, Y );
      }
      else if ( AlternativeMode == TextCharMode.VIC20 )
      {
        DisplayVIC20Char( Char.Tile.Data, Palette, AltBGColor, AltMColor1, AltMColor2, AlternativeColor, TargetImage, X, Y );
      }
      else if ( AlternativeMode == TextCharMode.X16_HIRES )
      {
        DisplayHiResChar( Char.Tile.Data, Palette, ( AlternativeColor >> 4 ) & 0x0f, AlternativeColor & 0x0f, TargetImage, X, Y );
      }
      else
      {
        Debug.Log( "DisplayChar #2 unsupported mode " + AlternativeMode );
      }
    }

  }
}
