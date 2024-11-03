using RetroDevStudio.Controls;
using RetroDevStudio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Policy;



namespace RetroDevStudio.Displayer
{
  public class CharacterDisplayer
  {
    public static void DisplayHiResChar( Types.GraphicTile Tile, int BGColor, int CharColor, CustomDrawControlContext Context )
    {
      DisplayHiResChar( Tile.Data, Tile.Width, Tile.Height, BGColor, CharColor, Context );  
    }



    public static void DisplayHiResChar( GR.Memory.ByteBuffer Data, int Width, int Height, int BGColor, int CharColor, CustomDrawControlContext Context )
    {
      // single color
      int colorIndex = 0;
      for ( int j = 0; j < Height; ++j )
      {
        for ( int i = 0; i < Width; ++i )
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
                                          ( i * Context.Bounds.Width ) / Width,
                                          ( j * Context.Bounds.Height ) / Height,
                                          ( ( i + 1 ) * Context.Bounds.Width ) / Width - ( i * Context.Bounds.Width ) / Width,
                                          ( ( j + 1 ) * Context.Bounds.Height ) / Height - ( j * Context.Bounds.Height ) / Height );
        }
      }
    }



    public static void DisplayMultiColorChar( Types.GraphicTile Tile, int BGColor, int MColor1, int MColor2, int CharColor, CustomDrawControlContext Context, int X, int Y )
    {
      DisplayMultiColorChar( Tile.Data, Tile.Width, Tile.Height, BGColor, MColor1, MColor2, CharColor, Context, X, Y );
    }



    public static void DisplayMultiColorChar( GR.Memory.ByteBuffer Data, int Width, int Height, int BGColor, int MColor1, int MColor2, int CharColor, CustomDrawControlContext Context, int X, int Y )
    {
      // multicolor
      if ( CharColor < 8 )
      {
        DisplayHiResChar( Data, Width, Height, BGColor, CharColor, Context );
        return;
      }

      // single color
      int charColor = CharColor - 8;

      for ( int j = 0; j < Height; ++j )
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
                                          ( j * Context.Bounds.Height ) / Height,
                                          ( ( i + 1 ) * Context.Bounds.Width ) / 4 - ( i * Context.Bounds.Width ) / 4,
                                          ( ( j + 1 ) * Context.Bounds.Height ) / Height - ( j * Context.Bounds.Height ) / Height );
        }
      }
    }



    public static void DisplayVIC20Char( Types.GraphicTile Tile, int BGColor, int BorderColor, int AuxiliaryColor, int CharColor, CustomDrawControlContext Context )
    {
      // multicolor
      if ( CharColor < 8 )
      {
        DisplayHiResChar( Tile, BGColor, CharColor, Context );
        return;
      }

      // single color
      int charColor = CharColor - 8;

      for ( int j = 0; j < Tile.Height; ++j )
      {
        for ( int i = 0; i < 4; ++i )
        {
          int pixelValue = ( Tile.Data.ByteAt( j ) & ( 3 << ( ( 3 - i ) * 2 ) ) ) >> ( ( 3 - i ) * 2 );

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
                                          ( j * Context.Bounds.Height ) / Tile.Height,
                                          ( ( i + 1 ) * Context.Bounds.Width ) / 4 - ( i * Context.Bounds.Width ) / 4,
                                          ( ( j + 1 ) * Context.Bounds.Height ) / Tile.Height - ( j * Context.Bounds.Height ) / Tile.Height );
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



    public static void DisplayHiResChar( Types.GraphicTile Tile, Palette Palette, int BGColor, int CharColor, GR.Image.IImage TargetImage, int X, int Y )
    {
      DisplayHiResChar( Tile.Data, Tile.Width, Tile.Height, Palette, BGColor, CharColor, TargetImage, X, Y );
    }



    public static void DisplayHiResChar( GR.Memory.ByteBuffer Data, int Width, int Height, Palette Palette, int BGColor, int CharColor, GR.Image.IImage TargetImage, int X, int Y )
    {
      // single color
      int colorIndex = 0;
      for ( int j = 0; j < Height; ++j )
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



    public static void DisplayMultiColorChar( Types.GraphicTile Tile, Palette Palette, int BGColor, int MColor1, int MColor2, int CharColor, GR.Image.IImage TargetImage, int X, int Y )
    {
      DisplayMultiColorChar( Tile.Data, Tile.Width, Tile.Height, Palette, BGColor, MColor1, MColor2, CharColor, TargetImage, X, Y );
    }



    public static void DisplayMultiColorChar( GR.Memory.ByteBuffer Data, int Width, int Height, Palette Palette, int BGColor, int MColor1, int MColor2, int CharColor, GR.Image.IImage TargetImage, int X, int Y )
    {
      // multicolor
      if ( CharColor < 8 )
      {
        DisplayHiResChar( Data, Width, Height, Palette, BGColor, CharColor, TargetImage, X, Y );
        return;
      }

      int charColor = CharColor - 8;

      for ( int j = 0; j < Height; ++j )
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



    public static void DisplayNESChar( GR.Memory.ByteBuffer Data, Palette Palette, List<List<int>> PaletteIndexMapping, int PaletteMappingIndex, GR.Image.IImage TargetImage, int X, int Y )
    {
      for ( int j = 0; j < 8; ++j )
      {
        for ( int i = 0; i < 8; ++i )
        {
          int pixelValue    = ( ( Data.ByteAt( j ) >> ( 7 - i ) ) & 0x01 )
                            | ( ( ( Data.ByteAt( 8 + j ) >> ( 7 - i ) ) & 0x01 ) * 2 );
          int realPalIndex  = PaletteIndexMapping[PaletteMappingIndex][pixelValue];
          uint color        = Palette.ColorValues[realPalIndex];
          TargetImage.SetPixel( X + i, Y + j, color );
        }
      }
    }



    public static void DisplayNESChar( GR.Memory.ByteBuffer Data, Types.ColorSettings Setup, CustomDrawControlContext Context )
    {
      for ( int j = 0; j < 8; ++j )
      {
        for ( int i = 0; i < 8; ++i )
        {
          int pixelValue    = ( ( Data.ByteAt( j ) >> ( 7 - i ) ) & 0x01 )
                            | ( ( ( Data.ByteAt( 8 + j ) >> ( 7 - i ) ) & 0x01 ) * 2 );

          int realPalIndex  = Setup.PaletteIndexMapping[Setup.PaletteMappingIndex][pixelValue];

          Context.Graphics.FillRectangle( Context.Palette.ColorBrushes[realPalIndex],
                                ( i * Context.Bounds.Width ) / 8,
                                ( j * Context.Bounds.Height ) / 8,
                                ( ( i + 1 ) * Context.Bounds.Width ) / 8 - ( i * Context.Bounds.Width ) / 8,
                                ( ( j + 1 ) * Context.Bounds.Height ) / 8 - ( j * Context.Bounds.Height ) / 8 );
        }
      }
    }



    public static void DisplayVIC20Char( Types.GraphicTile Tile, Palette Palette, int BGColor, int MColor1, int MColor2, int CharColor, GR.Image.IImage TargetImage, int X, int Y )
    {
      // multicolor
      if ( CharColor < 8 )
      {
        DisplayHiResChar( Tile, Palette, BGColor, CharColor, TargetImage, X, Y );
        return;
      }

      int charColor = CharColor - 8;

      for ( int j = 0; j < Tile.Height; ++j )
      {
        for ( int i = 0; i < 4; ++i )
        {
          int pixelValue = ( Tile.Data.ByteAt( j ) & ( 3 << ( ( 3 - i ) * 2 ) ) ) >> ( ( 3 - i ) * 2 );

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
      var alternativeSettings = new Types.AlternativeColorSettings( Charset.Colors );
      alternativeSettings.CustomColor = AlternativeColor;

      DisplayChar( Charset, CharIndex, Context, alternativeSettings );
    }




    public static void DisplayChar( Formats.CharsetProject Charset, int CharIndex, CustomDrawControlContext Context, Types.AlternativeColorSettings AlternativeSettings )
    {
      Formats.CharData Char = Charset.Characters[CharIndex];

      var palette   = Charset.Colors.Palette;
      var mode      = ( AlternativeSettings.CharMode == TextCharMode.UNKNOWN ) ? Charset.Mode : AlternativeSettings.CharMode;
      var bgColor   = ( AlternativeSettings.BackgroundColor == -1 ) ? Charset.Colors.BackgroundColor : AlternativeSettings.BackgroundColor;
      var bgColor4  = ( AlternativeSettings.BGColor4 == -1 ) ? Charset.Colors.BGColor4 : AlternativeSettings.BGColor4;
      var mColor1   = ( AlternativeSettings.MultiColor1 == -1 ) ? Charset.Colors.MultiColor1 : AlternativeSettings.MultiColor1;
      var mColor2   = ( AlternativeSettings.MultiColor2 == -1 ) ? Charset.Colors.MultiColor2 : AlternativeSettings.MultiColor2;
      var color     = AlternativeSettings.CustomColor;

      if ( ( mode == TextCharMode.COMMODORE_ECM )
      ||   ( mode == TextCharMode.MEGA65_ECM ) )
      {
        // ECM
        Formats.CharData origChar = Charset.Characters[CharIndex % 64];

        var ecmBGColor = bgColor;
        switch ( CharIndex / 64 )
        {
          case 1:
            ecmBGColor = mColor1;
            break;
          case 2:
            ecmBGColor = mColor2;
            break;
          case 3:
            ecmBGColor = bgColor4;
            break;
        }
        DisplayHiResChar( origChar.Tile, ecmBGColor, color, Context );
      }
      else if ( mode == TextCharMode.COMMODORE_MULTICOLOR )
      {
        DisplayMultiColorChar( Char.Tile, bgColor, mColor1, mColor2, color, Context, 0, 0 );
      }
      else if ( ( mode == TextCharMode.COMMODORE_HIRES )
      ||        ( mode == TextCharMode.MEGA65_HIRES )
      ||        ( mode == TextCharMode.COMMODORE_128_VDC_HIRES ) )
      {
        DisplayHiResChar( Char.Tile, bgColor, color, Context );
      }
      else if ( ( mode == TextCharMode.MEGA65_FCM )
      ||        ( mode == TextCharMode.MEGA65_FCM_16BIT ) )
      {
        DisplayMega65FCMChar( Char.Tile.Data, bgColor, color, Context );
      }
      else if ( mode == TextCharMode.MEGA65_NCM )
      {
        DisplayMega65NCMChar( Char.Tile.Data, bgColor, color, Context );
      }
      else if ( ( mode == TextCharMode.VIC20 )
      ||        ( mode == TextCharMode.VIC20_8X16 ) )
      {
        DisplayVIC20Char( Char.Tile, bgColor, mColor1, mColor2, color, Context );
      }
      else if ( mode == TextCharMode.X16_HIRES )
      {
        DisplayHiResChar( Char.Tile, ( color >> 4 ) & 0x0f, color & 0x0f, Context );
      }
      else if ( mode == TextCharMode.NES )
      {
        DisplayNESChar( Char.Tile.Data, Charset.Colors, Context );
      }
      else
      {
        Debug.Log( "DisplayChar unsupported mode " + mode );
      }
    }



    public static void DisplayChar( Formats.CharsetProject Charset, int CharIndex, GR.Image.IImage TargetImage, int X, int Y )
    {
      Formats.CharData Char = Charset.Characters[CharIndex];

      DisplayChar( Charset, CharIndex, TargetImage, X, Y, Char.Tile.CustomColor );
    }



    public static void DisplayChar( Formats.CharsetProject Charset, int CharIndex, GR.Image.IImage TargetImage, int X, int Y, int AlternativeColor )
    {
      var alternativeSettings = new Types.AlternativeColorSettings( Charset.Colors );

      alternativeSettings.CustomColor = AlternativeColor;
      DisplayChar( Charset, CharIndex, TargetImage, X, Y, alternativeSettings );
    }




    public static void DisplayChar( Formats.CharsetProject Charset, int CharIndex, GR.Image.IImage TargetImage, int X, int Y, Types.AlternativeColorSettings AlternativeSettings )
    {
      Formats.CharData Char = Charset.Characters[CharIndex];

      var palette = Charset.Colors.Palette;
      var mode = ( AlternativeSettings.CharMode == TextCharMode.UNKNOWN ) ? Charset.Mode : AlternativeSettings.CharMode;
      var bgColor = ( AlternativeSettings.BackgroundColor == -1 ) ? Charset.Colors.BackgroundColor : AlternativeSettings.BackgroundColor;
      var bgColor4 = ( AlternativeSettings.BGColor4 == -1 ) ? Charset.Colors.BGColor4 : AlternativeSettings.BGColor4;
      var mColor1 = ( AlternativeSettings.MultiColor1 == -1 ) ? Charset.Colors.MultiColor1 : AlternativeSettings.MultiColor1;
      var mColor2 = ( AlternativeSettings.MultiColor2 == -1 ) ? Charset.Colors.MultiColor2 : AlternativeSettings.MultiColor2;
      var color = AlternativeSettings.CustomColor;

      if ( ( mode == TextCharMode.COMMODORE_ECM )
      ||   ( mode == TextCharMode.MEGA65_ECM ) )
      {
        // ECM
        Formats.CharData origChar = Charset.Characters[CharIndex % 64];

        var altBGColor = bgColor;
        switch ( CharIndex / 64 )
        {
          case 1:
            altBGColor = mColor1;
            break;
          case 2:
            altBGColor = mColor2;
            break;
          case 3:
            altBGColor = bgColor4;
            break;
        }
        DisplayHiResChar( origChar.Tile, palette, altBGColor, color, TargetImage, X, Y );
      }
      else if ( mode == TextCharMode.COMMODORE_MULTICOLOR )
      {
        DisplayMultiColorChar( Char.Tile, palette, bgColor, mColor1, mColor2, color, TargetImage, X, Y );
      }
      else if ( ( mode == TextCharMode.COMMODORE_HIRES )
      ||        ( mode == TextCharMode.MEGA65_HIRES )
      ||        ( mode == TextCharMode.COMMODORE_128_VDC_HIRES ) )
      {
        DisplayHiResChar( Char.Tile, palette, bgColor, color, TargetImage, X, Y );
      }
      else if ( ( mode == TextCharMode.MEGA65_FCM )
      ||        ( mode == TextCharMode.MEGA65_FCM_16BIT ) )
      {
        DisplayMega65FCMChar( Char.Tile.Data, palette, bgColor, color, TargetImage, X, Y );
      }
      else if ( mode == TextCharMode.MEGA65_NCM )
      {
        DisplayMega65NCMChar( Char.Tile.Data, palette, bgColor, color, TargetImage, X, Y );
      }
      else if ( ( mode == TextCharMode.VIC20 )
      ||        ( mode == TextCharMode.VIC20_8X16 ) )
      {
        DisplayVIC20Char( Char.Tile, palette, bgColor, mColor1, mColor2, color, TargetImage, X, Y );
      }
      else if ( mode == TextCharMode.X16_HIRES )
      {
        DisplayHiResChar( Char.Tile, palette, ( color >> 4 ) & 0x0f, color & 0x0f, TargetImage, X, Y );
      }
      else if ( mode == TextCharMode.NES )
      {
        var paletteIndexMapping = Charset.Colors.PaletteIndexMapping;
        int paletteMappingIndex = Charset.Colors.PaletteMappingIndex;
        if ( AlternativeSettings.PaletteMappingIndex != -1 )
        {
          paletteIndexMapping = AlternativeSettings.PaletteIndexMapping;
          paletteMappingIndex = AlternativeSettings.PaletteMappingIndex;
        }
        DisplayNESChar( Char.Tile.Data, palette, paletteIndexMapping, paletteMappingIndex, TargetImage, X, Y );
      }
      else
      {
        Debug.Log( "DisplayChar #2 unsupported mode " + mode );
      }
    }

  }
}
