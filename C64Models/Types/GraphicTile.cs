using RetroDevStudio.Converter;
using GR.Generic;
using GR.Memory;
using System;
using System.Collections.Generic;
using System.Text;



namespace RetroDevStudio.Types
{
  public class GraphicTile
  {
    public ColorSettings        Colors = new ColorSettings();
    public GraphicTileMode      Mode = GraphicTileMode.COMMODORE_HIRES;
    public int                  TransparentColorIndex = -1;
    public int                  Width = 8;
    public int                  Height = 8;
    public byte                 CustomColor = 1;
    public ByteBuffer           Data = new ByteBuffer( 8 );
    public GR.Image.MemoryImage Image = new GR.Image.MemoryImage( 8, 8, GR.Drawing.PixelFormat.Format32bppRgb );



    public GraphicTile()
    {
    }



    public GraphicTile( GraphicTile OtherTile )
    {
      Width                 = OtherTile.Width;
      Height                = OtherTile.Height;
      Mode                  = OtherTile.Mode;
      Colors                = OtherTile.Colors;
      CustomColor           = OtherTile.CustomColor;
      TransparentColorIndex = OtherTile.TransparentColorIndex;
      Data                  = new ByteBuffer( OtherTile.Data );
      Image                 = new GR.Image.MemoryImage( OtherTile.Image );
    }



    public GraphicTile( int Width, int Height, GraphicTileMode Mode, ColorSettings Color )
    {
      this.Width = Width;
      this.Height = Height;
      this.Mode = Mode;
      Colors = Color;

      Data  = new ByteBuffer( (uint)Lookup.NumBytes( Width, Height, Mode ) );
      Image = new GR.Image.MemoryImage( Width, Height, GR.Drawing.PixelFormat.Format32bppRgb );
    }



    public bool SetPixel( int X, int Y, Tupel<ColorType,byte> Color )
    {
      if ( !IsInside( X, Y ) )
      {
        return false;
      }

      switch ( Mode )
      {
        case GraphicTileMode.COMMODORE_ECM:
        case GraphicTileMode.COMMODORE_HIRES:
        case GraphicTileMode.COMMANDERX16_HIRES:
        case GraphicTileMode.COMMODORE_128_VDC_HIRES:
          {
            int bytePos = Y * ( ( Width + 7 ) / 8 ) + X / 8;
            int   byteValue = Data.ByteAt( bytePos );
            if ( ( byteValue & ( 1 << ( 7 - ( X % 8 ) ) ) ) == 0 )
            {
              if ( Color.first != ColorType.BACKGROUND )
              {
                Data.SetU8At( bytePos, (byte)( byteValue | ( 1 << ( 7 - ( X % 8 ) ) ) ) );
                return true;
              }
            }
            else if ( Color.first == ColorType.BACKGROUND )
            {
              Data.SetU8At( bytePos, (byte)( byteValue & ~( 1 << ( 7 - ( X % 8 ) ) ) ) );
              return true;
            }
          }
          break;
        case GraphicTileMode.COMMODORE_MULTICOLOR_CHARACTERS:
          {
            int bytePos = Y * ( ( Width + 7 ) / 8 ) + X / 8;

            // mc mode
            X = ( X % 8 ) / 2;
            X = 3 - X;

            byte newByte = (byte)( Data.ByteAt( bytePos ) & ~( 3 << ( 2 * X ) ) );

            int     replacementBytes = 0;

            switch ( Color.first )
            {
              case ColorType.CUSTOM_COLOR:
                replacementBytes = 3;
                break;
              case ColorType.MULTICOLOR_1:
                replacementBytes = 1;
                break;
              case ColorType.MULTICOLOR_2:
                replacementBytes = 2;
                break;
            }
            newByte |= (byte)( replacementBytes << ( 2 * X ) );

            if ( Data.ByteAt( bytePos ) != newByte )
            {
              Data.SetU8At( bytePos, newByte );
              return true;
            }
          }
          break;
        case GraphicTileMode.COMMODORE_MULTICOLOR_SPRITES:
          {
            int bytePos = Y * ( ( Width + 7 ) / 8 ) + X / 8;

            // mc mode
            X = ( X % 8 ) / 2;
            X = 3 - X;

            byte newByte = (byte)( Data.ByteAt( bytePos ) & ~( 3 << ( 2 * X ) ) );

            int     replacementBytes = 0;

            switch ( Color.first )
            {
              case ColorType.CUSTOM_COLOR:
                replacementBytes = 2;
                break;
              case ColorType.MULTICOLOR_1:
                replacementBytes = 1;
                break;
              case ColorType.MULTICOLOR_2:
                replacementBytes = 3;
                break;
            }
            newByte |= (byte)( replacementBytes << ( 2 * X ) );

            if ( Data.ByteAt( bytePos ) != newByte )
            {
              Data.SetU8At( bytePos, newByte );
              return true;
            }
          }
          break;
        case GraphicTileMode.NES:
          {
            bool changed = false;

            int bytePos = Y * ( ( Width + 7 ) / 8 ) + X / 8;
            int bitShift = ( 7 - ( X % 8 ) );
            int bitValue = 1 << bitShift;

            // plane 1
            int   byteValue = Data.ByteAt( bytePos );
            if ( ( Color.second & 0x01 ) != ( ( byteValue & bitValue ) >> bitShift ) )
            {
              changed = true;
              byteValue &= ~bitValue;
              if ( ( Color.second & 0x01 ) != 0 )
              {
                byteValue |= bitValue;
              }
              Data.SetU8At( bytePos, (byte)byteValue );
            }
            // plane 1
            bytePos += Lookup.NumBytesOfSingleCharacterBitmap( TextCharMode.NES ) / 2;
            byteValue = Data.ByteAt( bytePos );
            if ( ( ( Color.second & 0x02 ) >> 1 ) != ( ( byteValue & bitValue ) >> bitShift ) )
            {
              changed = true;
              byteValue &= ~bitValue;
              if ( ( Color.second & 0x02 ) != 0 )
              {
                byteValue |= bitValue;
              }
              Data.SetU8At( bytePos, (byte)byteValue );
            }
            return changed;
          }
        case GraphicTileMode.MEGA65_NCM_CHARACTERS:
          {
            // Mega65 NCM char mode has nybbles swapped
            byte    newColor = Color.second;

            int     bytePos = X / 2 + Y * ( ( Width + 1 ) / 2 );
            byte pixelValue = Data.ByteAt( bytePos );
            if ( ( X % 2 ) != 0 )
            {
              if ( ( pixelValue >> 4 ) != newColor )
              {
                pixelValue &= 0x0f;
                pixelValue |= (byte)( newColor << 4 );

                Data.SetU8At( bytePos, pixelValue );
                return true;
              }
            }
            else
            {
              if ( ( pixelValue & 0x0f ) != newColor )
              {
                pixelValue &= 0xf0;
                pixelValue |= (byte)newColor;

                Data.SetU8At( bytePos, pixelValue );
                return true;
              }
            }
            return false;
          }
        case GraphicTileMode.COMMANDERX16_16_COLORS:
        case GraphicTileMode.MEGA65_NCM_SPRITES:
          {
            byte    newColor = Color.second;

            int     bytePos = X / 2 + Y * ( ( Width + 1 ) / 2 );
            byte pixelValue = Data.ByteAt( bytePos );
            if ( ( X % 2 ) == 0 )
            {
              if ( ( pixelValue >> 4 ) != newColor )
              {
                pixelValue &= 0x0f;
                pixelValue |= (byte)( newColor << 4 );

                Data.SetU8At( bytePos, pixelValue );
                return true;
              }
            }
            else
            {
              if ( ( pixelValue & 0x0f ) != newColor )
              {
                pixelValue &= 0xf0;
                pixelValue |= newColor;

                Data.SetU8At( bytePos, pixelValue );
                return true;
              }
            }
            return false;
          }
        case GraphicTileMode.COMMANDERX16_256_COLORS:
        case GraphicTileMode.MEGA65_FCM_256_COLORS:
          if ( Data.ByteAt( X + Y * Width ) != Color.second )
          {
            Data.SetU8At( X + Y * Width, Color.second );
            return true;
          }
          break;
        default:
          Debug.Log( "GraphicTile.SetPixel, unsupported mode " + Mode );
          break;
      }
      return false;
    }



    public Tupel<ColorType,byte> GetPixel( int X, int Y )
    {
      if ( !IsInside( X, Y ) )
      {
        return new Tupel<ColorType, byte>( ColorType.BACKGROUND, 0 );
      }
      switch ( Mode )
      {
        case GraphicTileMode.COMMODORE_ECM:
        case GraphicTileMode.COMMODORE_HIRES:
        case GraphicTileMode.COMMANDERX16_HIRES:
        case GraphicTileMode.COMMODORE_128_VDC_HIRES:
          {
            if ( ( Data.ByteAt( Y * ( ( Width + 7 ) / 8 ) + X / 8 ) & ( 1 << ( 7 - ( X % 8 ) ) ) ) != 0 )
            {
              return new Tupel<ColorType, byte>( ColorType.CUSTOM_COLOR, 1 );
            }
            return new Tupel<ColorType, byte>( ColorType.BACKGROUND, 0 );
          }
        case GraphicTileMode.COMMODORE_MULTICOLOR_CHARACTERS:
          {
            // multi color
            int innerX = ( X % 8 ) / 2;
            innerX = 3 - innerX;

            int   bitPattern = Data.ByteAt( Y * ( ( Width + 7 ) / 8 ) + X / 8 ) & ( 3 << ( 2 * innerX ) );
            bitPattern >>= innerX * 2;

            switch ( bitPattern )
            {
              case 0x00:
                return new Tupel<ColorType, byte>( ColorType.BACKGROUND, 0 );
              case 0x01:
                return new Tupel<ColorType, byte>( ColorType.MULTICOLOR_1, 1 );
              case 0x02:
                return new Tupel<ColorType, byte>( ColorType.MULTICOLOR_2, 2 );
              case 0x03:
              default:
                return new Tupel<ColorType, byte>( ColorType.CUSTOM_COLOR, 3 );
            }
          }
        case GraphicTileMode.COMMODORE_MULTICOLOR_SPRITES:
          {
            // multi color
            int innerX = ( X % 8 ) / 2;
            innerX = 3 - innerX;

            int   bitPattern = Data.ByteAt( Y * ( ( Width + 7 ) / 8 ) + X / 8 ) & ( 3 << ( 2 * innerX ) );
            bitPattern >>= innerX * 2;

            switch ( bitPattern )
            {
              case 0x00:
                return new Tupel<ColorType, byte>( ColorType.BACKGROUND, 0 );
              case 0x01:
                return new Tupel<ColorType, byte>( ColorType.MULTICOLOR_1, 1 );
              case 0x03:
                return new Tupel<ColorType, byte>( ColorType.MULTICOLOR_2, 3 );
              case 0x02:
              default:
                return new Tupel<ColorType, byte>( ColorType.CUSTOM_COLOR, 2 );
            }
          }
        case GraphicTileMode.NES:
          {
            int bytePos = Y * ( ( Width + 7 ) / 8 ) + X / 8;
            int bitShift = ( 7 - ( X % 8 ) );
            int bitValue = 1 << bitShift;

            byte colValue = (byte)( ( Data.ByteAt( bytePos ) & bitValue ) >> bitShift );

            bytePos += Lookup.NumBytesOfSingleCharacterBitmap( TextCharMode.NES ) / 2;
            colValue |= (byte)( ( ( Data.ByteAt( bytePos ) & bitValue ) >> bitShift ) * 2 );

            return new Tupel<ColorType, byte>( ColorType.CUSTOM_COLOR, colValue );
          }
        case GraphicTileMode.MEGA65_NCM_CHARACTERS:
          {
            byte color = 0;
            if ( ( X % 2 ) != 1 )
            {
              color = (byte)( Data.ByteAt( X / 2 + Y * ( ( Width + 1 ) / 2 ) ) & 0x0f );
            }
            else
            {
              color = (byte)( Data.ByteAt( X / 2 + Y * ( ( Width + 1 ) / 2 ) ) >> 4 );
            }

            if ( color == 0 )
            {
              return new Tupel<ColorType, byte>( ColorType.BACKGROUND, 0 );
            }
            return new Tupel<ColorType, byte>( ColorType.CUSTOM_COLOR, color );
          }
        case GraphicTileMode.COMMANDERX16_16_COLORS:
        case GraphicTileMode.MEGA65_NCM_SPRITES:
          {
            byte color = 0;
            if ( ( X % 2 ) == 1 )
            {
              color = (byte)( Data.ByteAt( X / 2 + Y * ( ( Width + 1 ) / 2 ) ) & 0x0f );
            }
            else
            {
              color = (byte)( Data.ByteAt( X / 2 + Y * ( ( Width + 1 ) / 2 ) ) >> 4 );
            }

            if ( color == 0 )
            {
              return new Tupel<ColorType, byte>( ColorType.BACKGROUND, 0 );
            }
            return new Tupel<ColorType, byte>( ColorType.CUSTOM_COLOR, color );
          }
        case GraphicTileMode.MEGA65_FCM_256_COLORS:
        case GraphicTileMode.COMMANDERX16_256_COLORS:
          {
            byte  colorPixel = Data.ByteAt( X + Y * Width );
            if ( colorPixel == 0 )
            {
              return new Tupel<ColorType, byte>( ColorType.BACKGROUND, 0 );
            }
            return new Tupel<ColorType, byte>( ColorType.CUSTOM_COLOR, colorPixel );
          }
        default:
          Debug.Log( "GraphicTile.GetPixel, unsupported mode " + Mode );
          return new Tupel<ColorType, byte>( ColorType.BACKGROUND, 0 );
      }
    }



    private bool IsInside( int X, int Y )
    {
      if ( ( X < 0 )
      ||   ( X >= Width )
      ||   ( Y < 0 )
      ||   ( Y >= Height ) )
      {
        return false;
      }
      return true;
    }



    public int MapColor( uint Color )
    {
      return 0;
    }



    public Tupel<ColorType,byte> MapPixelColor( int X, int Y, GraphicTile TargetTile )
    {
      var     pixelValue = GetPixel( X, Y );
      if ( Mode == TargetTile.Mode )
      {
        return pixelValue;
      }
      // now things are getting funny
      uint  pixelColor = GetColorFromValue( pixelValue.second );

      var potentialColors = new List<uint>();
      var potentialColorTypes =new List<ColorType>();

      switch ( TargetTile.Mode )
      {
        case GraphicTileMode.COMMODORE_HIRES:
          potentialColors.Add( Colors.Palette.ColorValues[Colors.BackgroundColor] );
          potentialColorTypes.Add( ColorType.BACKGROUND );

          // TODO - variable color!
          potentialColors.Add( Colors.Palette.ColorValues[CustomColor] );
          potentialColorTypes.Add( ColorType.CUSTOM_COLOR );
          break;
        case GraphicTileMode.COMMODORE_MULTICOLOR_SPRITES:
        case GraphicTileMode.COMMODORE_MULTICOLOR_CHARACTERS:
          potentialColors.Add( Colors.Palette.ColorValues[Colors.BackgroundColor] );
          potentialColorTypes.Add( ColorType.BACKGROUND );
          potentialColors.Add( Colors.Palette.ColorValues[Colors.MultiColor1] );
          potentialColorTypes.Add( ColorType.MULTICOLOR_1 );
          potentialColors.Add( Colors.Palette.ColorValues[Colors.MultiColor2] );
          potentialColorTypes.Add( ColorType.MULTICOLOR_2 );

          // TODO - variable color!
          potentialColors.Add( Colors.Palette.ColorValues[CustomColor] );
          potentialColorTypes.Add( ColorType.CUSTOM_COLOR );
          break;
        case GraphicTileMode.COMMODORE_ECM:
          potentialColors.Add( Colors.Palette.ColorValues[Colors.BackgroundColor] );
          potentialColorTypes.Add( ColorType.BACKGROUND );
          potentialColors.Add( Colors.Palette.ColorValues[Colors.BGColor4] );
          potentialColorTypes.Add( ColorType.BGCOLOR4 );

          // TODO - variable colors!
          potentialColors.Add( Colors.Palette.ColorValues[CustomColor] );
          potentialColorTypes.Add( ColorType.CUSTOM_COLOR );
          break;
        case GraphicTileMode.MEGA65_NCM_SPRITES:
        case GraphicTileMode.MEGA65_NCM_CHARACTERS:
        case GraphicTileMode.MEGA65_FCM_256_COLORS:
          for ( int i = 0; i < Colors.Palette.NumColors; ++i )
          {
            potentialColors.Add( Colors.Palette.ColorValues[i] );
            potentialColorTypes.Add( (ColorType)i );
          }
          break;
      }

      int bestMatch = FindClosestEntryInPalette( pixelColor, potentialColorTypes, potentialColors );

      return new Tupel<ColorType, byte>( ColorType.CUSTOM_COLOR, (byte)bestMatch );
    }



    private int FindClosestEntryInPalette( uint PixelColor, List<ColorType> ColorTypes, List<uint> PotentialColors )
    {
      int   index = 0;
      foreach ( var color in PotentialColors )
      {
        if ( PixelColor == color )
        {
          return (int)ColorTypes[index];
        }
        ++index;
      }

      index = ColorMatcher.MatchColor( ColorMatchType.HUE_DISTANCE, (byte)( PixelColor >> 16 ), (byte)( PixelColor >> 8 ), (byte)PixelColor, PotentialColors );

      return (int)ColorTypes[index];
    }



    private uint GetColorFromValue( int PixelValue )
    {
      if ( Lookup.HasCustomPalette( Mode ) )
      {
        return Colors.Palette.ColorValues[PixelValue];
      }
      switch ( (ColorType)PixelValue )
      {
        case ColorType.BACKGROUND:
          return Colors.Palette.ColorValues[Colors.BackgroundColor];
        case ColorType.MULTICOLOR_1:
          return Colors.Palette.ColorValues[Colors.MultiColor1];
        case ColorType.MULTICOLOR_2:
          return Colors.Palette.ColorValues[Colors.MultiColor2];
        case ColorType.CUSTOM_COLOR:
          return Colors.Palette.ColorValues[CustomColor];
        case ColorType.BGCOLOR4:
          return Colors.Palette.ColorValues[Colors.BGColor4];
      }
      return 0;
    }



    public bool Fill( int X, int Y, Tupel<ColorType,byte> NewColor )
    {
      if ( ( X < 0 )
      ||   ( X >= Width )
      ||   ( Y < 0 )
      ||   ( Y >= Height ) )
      {
        return false;
      }

      var   origColor = GetPixel( X, Y );
      if ( IsSameColor( origColor, NewColor ) )
      {
        return false;
      }

      List<GR.Math.Point>      pointsToCheck = new List<GR.Math.Point>();

      pointsToCheck.Add( new GR.Math.Point( X, Y ) );

      int     pixelWidth = Lookup.PixelWidth( Mode );

      while ( pointsToCheck.Count != 0 )
      {
        GR.Math.Point    point = pointsToCheck[pointsToCheck.Count - 1];
        pointsToCheck.RemoveAt( pointsToCheck.Count - 1 );

        if ( !IsSameColor( GetPixel( point.X, point.Y ), NewColor ) )
        {
          SetPixel( point.X, point.Y, NewColor );

          if ( ( point.X - pixelWidth >= 0 )
          &&   ( IsSameColor( GetPixel( point.X - pixelWidth, point.Y ), origColor ) ) )
          {
            pointsToCheck.Add( new GR.Math.Point( point.X - pixelWidth, point.Y ) );
          }
          if ( ( point.X + pixelWidth < Width )
          &&   ( IsSameColor( GetPixel( point.X + pixelWidth, point.Y ), origColor ) ) )
          {
            pointsToCheck.Add( new GR.Math.Point( point.X + pixelWidth, point.Y ) );
          }
          if ( ( point.Y > 0 )
          &&   ( IsSameColor( GetPixel( point.X, point.Y - 1 ), origColor ) ) )
          {
            pointsToCheck.Add( new GR.Math.Point( point.X, point.Y - 1 ) );
          }
          if ( ( point.Y + 1 < Height )
          &&   ( IsSameColor( GetPixel( point.X, point.Y + 1 ), origColor ) ) )
          {
            pointsToCheck.Add( new GR.Math.Point( point.X, point.Y + 1 ) );
          }
        }
      }
      return true;
    }



    private bool IsSameColor( Tupel<ColorType, byte> Color1, Tupel<ColorType, byte> Color2 )
    {
      if ( Lookup.HasCustomPalette( Mode ) )
      {
        return Color1 == Color2;
      }
      return Color1.first == Color2.first;
    }



  }
}
