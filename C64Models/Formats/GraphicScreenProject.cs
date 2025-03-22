﻿using System.Collections.Generic;
using System.ComponentModel;
using RetroDevStudio;
using RetroDevStudio.Types;

namespace RetroDevStudio.Formats
{
  public class GraphicScreenProject
  {
    public enum CheckType
    {
      [Description( "Hires Bitmap" )]
      HIRES_BITMAP,
      [Description( "Multicolor Bitmap" )]
      MULTICOLOR_BITMAP,
      [Description( "Mega65 Bitmap" )]
      MEGA65_BITMAP,
      [Description( "Hires Charset" )]
      HIRES_CHARSET,
      [Description( "Multicolor Charset" )]
      MULTICOLOR_CHARSET,
      [Description( "Mega65 FCM Charset" )]
      MEGA65_FCM_CHARSET,
      [Description( "Mega65 FCM 16bit Charset" )]
      MEGA65_FCM_CHARSET_16BIT,
      [Description( "VIC20 Charset" )]
      VIC20_CHARSET,
      [Description( "VIC20 Charset 8x16" )]
      VIC20_CHARSET_8X16
    };

    public enum ColorMappingTarget
    {
      [Description( "00 (Background)" )]
      BITS_00 = 0,
      [Description( "01 (Hi-Nibble of Screen)" )]
      BITS_01 = 1,
      [Description( "10 (Lo-Nibble of Screen)" )]
      BITS_10 = 2,
      [Description( "11 (Color RAM)" )]
      BITS_11 = 3,
      [Description( "Any" )]
      ANY = 4,

      // HiRes
      [Description( "0 (Color 1)" )]
      COLOR_1 = 5,
      [Description( "1 (Color 2)" )]
      COLOR_2 = 6
    };



    public ColorSettings                Colors = new ColorSettings();
    public bool                         MultiColor = false;
    public GR.Image.MemoryImage         Image = new GR.Image.MemoryImage( 320, 200, GR.Drawing.PixelFormat.Format8bppIndexed );

    public CheckType                    SelectedCheckType = CheckType.MULTICOLOR_BITMAP;

    public int                          ScreenOffsetX = 0;
    public int                          ScreenOffsetY = 0;

    public int                          ScreenWidth = 320;
    public int                          ScreenHeight = 200;

    public Dictionary<int,List<ColorMappingTarget>>   ColorMapping = new Dictionary<int, List<ColorMappingTarget>>();




    public GraphicScreenProject()
    {
      PaletteManager.ApplyPalette( Image );

      for ( int i = 0; i < 16; ++i )
      {
        ColorMapping.Add( i, new List<ColorMappingTarget> { ColorMappingTarget.ANY } );
      }
      Colors.Palette = ConstantData.DefaultPaletteC64();
    }



    private int BlockWidth
    {
      get
      {
        return ( ScreenWidth + 7 ) / 8;
      }
    }



    private int BlockHeight
    {
      get
      {
        return ( ScreenHeight + 7 ) / 8;
      }
    }



    public GR.Memory.ByteBuffer SaveToBuffer()
    {
      GR.Memory.ByteBuffer data = new GR.Memory.ByteBuffer();

      data.Reserve( ScreenHeight * ScreenWidth * 8 );

      GR.IO.FileChunk   chunkScreenInfo = new GR.IO.FileChunk( FileChunkConstants.GRAPHIC_SCREEN_INFO );
      chunkScreenInfo.AppendU32( (uint)SelectedCheckType );
      chunkScreenInfo.AppendI32( ScreenOffsetX );
      chunkScreenInfo.AppendI32( ScreenOffsetY );
      chunkScreenInfo.AppendI32( ScreenWidth );
      chunkScreenInfo.AppendI32( ScreenHeight );
      data.Append( chunkScreenInfo.ToBuffer() );

      GR.IO.FileChunk   chunkGraphicData = new GR.IO.FileChunk( FileChunkConstants.GRAPHIC_DATA );
      chunkGraphicData.AppendI32( Image.Width );
      chunkGraphicData.AppendI32( Image.Height );
      chunkGraphicData.AppendI32( (int)Image.PixelFormat );
      chunkGraphicData.AppendI32( Image.PaletteEntryCount );
      for ( int i = 0; i < Image.PaletteEntryCount; ++i )
      {
        chunkGraphicData.AppendU8( Image.PaletteRed( i ) );
        chunkGraphicData.AppendU8( Image.PaletteGreen( i ) );
        chunkGraphicData.AppendU8( Image.PaletteBlue( i ) );
      }
      GR.Memory.ByteBuffer  imageData = Image.GetAsData();
      chunkGraphicData.AppendU32( imageData.Length );
      chunkGraphicData.Append( imageData );
      data.Append( chunkGraphicData.ToBuffer() );

      GR.IO.FileChunk chunkScreenMultiColorData = new GR.IO.FileChunk( FileChunkConstants.MULTICOLOR_DATA );
      chunkScreenMultiColorData.AppendU8( (byte)( MultiColor ? 1 : 0 ) );
      chunkScreenMultiColorData.AppendU8( (byte)Colors.BackgroundColor );
      chunkScreenMultiColorData.AppendU8( (byte)Colors.MultiColor1 );
      chunkScreenMultiColorData.AppendU8( (byte)Colors.MultiColor2 );
      chunkScreenMultiColorData.AppendI32( Colors.ActivePalette );
      data.Append( chunkScreenMultiColorData.ToBuffer() );

      foreach ( var pal in Colors.Palettes )
      {
        data.Append( pal.ToBuffer() );
      }

      GR.IO.FileChunk chunkColorMapping = new GR.IO.FileChunk( FileChunkConstants.GRAPHIC_COLOR_MAPPING );
      chunkColorMapping.AppendI32( ColorMapping.Count );
      for ( int i = 0; i < ColorMapping.Count; ++i )
      {
        var     mappings = ColorMapping[i];

        chunkColorMapping.AppendI32( mappings.Count );
        for ( int j = 0; j < mappings.Count; ++j )
        {
          chunkColorMapping.AppendU8( (byte)mappings[j] );
        }
      }
      data.Append( chunkColorMapping.ToBuffer() );
      return data;
    }



    public bool ReadFromBuffer( GR.Memory.ByteBuffer ProjectFile )
    {
      ColorMapping.Clear();
      for ( int i = 0; i < 16; ++i )
      {
        ColorMapping.Add( i, new List<ColorMappingTarget> { ColorMappingTarget.ANY } );
      }
      Colors.Palettes.Clear();

      GR.IO.MemoryReader memReader = new GR.IO.MemoryReader( ProjectFile );

      GR.IO.FileChunk chunk = new GR.IO.FileChunk();

      while ( chunk.ReadFromStream( memReader ) )
      {
        GR.IO.MemoryReader chunkReader = chunk.MemoryReader();

        switch ( chunk.Type )
        {
          case FileChunkConstants.GRAPHIC_SCREEN_INFO:
            SelectedCheckType = (CheckType)chunkReader.ReadUInt32();
            ScreenOffsetX = chunkReader.ReadInt32();
            ScreenOffsetY = chunkReader.ReadInt32();
            ScreenWidth = chunkReader.ReadInt32();
            ScreenHeight = chunkReader.ReadInt32();
            if ( ( ScreenWidth == 0 )
            ||   ( ScreenHeight == 0 ) )
            {
              ScreenWidth = 320;
              ScreenHeight = 200;
            }
            break;
          case FileChunkConstants.GRAPHIC_COLOR_MAPPING:
            {
              ColorMapping.Clear();

              int     numEntries = chunkReader.ReadInt32();

              for ( int i = 0; i < numEntries; ++i )
              {
                ColorMapping.Add( i, new List<ColorMappingTarget>() );

                int     numMappings = chunkReader.ReadInt32();

                for ( int j = 0; j < numMappings; ++j )
                {
                  ColorMappingTarget    mappingTarget = (ColorMappingTarget)chunkReader.ReadUInt8();

                  ColorMapping[i].Add( mappingTarget );
                }
              }
            }
            break;
          case FileChunkConstants.GRAPHIC_DATA:
            {
              int width = chunkReader.ReadInt32();
              int height = chunkReader.ReadInt32();
              GR.Drawing.PixelFormat format = (GR.Drawing.PixelFormat)chunkReader.ReadInt32();
              int paletteCount = chunkReader.ReadInt32();
              Image.Create( width, height, format );
              for ( int i = 0; i < paletteCount; ++i )
              {
                byte r = chunkReader.ReadUInt8();
                byte g = chunkReader.ReadUInt8();
                byte b = chunkReader.ReadUInt8();

                Image.SetPaletteColor( i, r, g, b );
              }
              uint dataSize = chunkReader.ReadUInt32();
              GR.Memory.ByteBuffer imageData = new GR.Memory.ByteBuffer();
              chunkReader.ReadBlock( imageData, dataSize );
              Image.SetData( imageData );
            }
            break;
          case FileChunkConstants.MULTICOLOR_DATA:
            MultiColor = ( chunkReader.ReadUInt8() == 1 );
            Colors.BackgroundColor = chunkReader.ReadUInt8();
            Colors.MultiColor1 = chunkReader.ReadUInt8();
            Colors.MultiColor2 = chunkReader.ReadUInt8();
            Colors.ActivePalette = chunkReader.ReadInt32();
            if ( ( Colors.MultiColor1 < 0 )
            ||   ( Colors.MultiColor1 >= 16 ) )
            {
              Colors.MultiColor1 = 0;
            }
            if ( ( Colors.MultiColor2 < 0 )
            ||   ( Colors.MultiColor2 >= 16 ) )
            {
              Colors.MultiColor2 = 0;
            }
            break;
          case FileChunkConstants.PALETTE:
            Colors.Palettes.Add( Palette.Read( chunkReader ) );
            break;
        }
      }
      memReader.Close();

      if ( Colors.Palettes.Count == 0 )
      {
        Colors.Palettes.Add( ConstantData.DefaultPaletteC64() );
      }
      return true;
    }




    public int ImageToHiresBitmapData( Dictionary<int, List<ColorMappingTarget>> ForceBitPattern, List<Formats.CharData> Chars, bool[,] ErrornousBlocks, int CharX, int CharY, int WidthChars, int HeightChars, out GR.Memory.ByteBuffer bitmapData, out GR.Memory.ByteBuffer screenChar, out GR.Memory.ByteBuffer screenColor )
    {
      screenChar  = null;
      screenColor = null;
      bitmapData  = null;

      if ( ( CharX < 0 )
      ||   ( CharX >= BlockWidth )
      ||   ( CharY < 0 )
      ||   ( CharY >= BlockHeight )
      ||   ( WidthChars < 0 )
      ||   ( HeightChars < 0 )
      ||   ( CharX + WidthChars > BlockWidth )
      ||   ( CharY + HeightChars > BlockHeight ) )
      {
        return 1;
      }

      int numErrors = 0;
      screenChar  = new GR.Memory.ByteBuffer( (uint)( WidthChars * HeightChars ) );
      screenColor = new GR.Memory.ByteBuffer( (uint)( WidthChars * HeightChars ) );
      bitmapData  = new GR.Memory.ByteBuffer( (uint)( 8 * WidthChars * HeightChars ) );

      GR.Collections.Map<byte, byte> usedColors = new GR.Collections.Map<byte, byte>();

      for ( int y = CharY; y < HeightChars; ++y )
      {
        for ( int x = CharX; x < WidthChars; ++x )
        {
          // ein zeichen-block
          usedColors.Clear();
          if ( ErrornousBlocks != null )
          {
            ErrornousBlocks[x, y] = false;
          }
          for ( int charY = 0; charY < 8; ++charY )
          {
            for ( int charX = 0; charX < 8; ++charX )
            {
              byte colorIndex = (byte)Image.GetPixel( x * 8 + charX, y * 8 + charY );
              if ( colorIndex >= 16 )
              {
                if ( Chars != null )
                {
                  Chars[x + y * BlockWidth].Error = "Color index >= 16 (" + colorIndex + ") at " + ( x * 8 + charX ).ToString() + ", " + ( y * 8 + charY ).ToString() + " (" + charX + "," + charY + ")";
                }
                if ( ErrornousBlocks != null )
                {
                  ErrornousBlocks[x, y] = true;
                }
                ++numErrors;
              }
              //if ( colorIndex != Colors.BackgroundColor )
              {
                // remember used color
                usedColors.Add( colorIndex, 0 );
              }
            }
          }
          // more than 2 colors?
          if ( usedColors.Count > 2 )
          {
            if ( Chars != null )
            {
              Chars[x + y * BlockWidth].Error = "Too many colors used";
            }
            if ( ErrornousBlocks != null )
            {
              ErrornousBlocks[x, y] = true;
            }
            ++numErrors;
          }
          else
          {
            int     firstColorIndex = -1;
            if ( usedColors.Count > 0 )
            {
              int colorTarget = 0;
              List<byte> keys = new List<byte>( usedColors.Keys );

              // only one color, that means, the other was background -> force the same bit pattern
              if ( ( usedColors.Count == 1 )
              &&   ( usedColors[0] != Colors.BackgroundColor ) )
              {
                colorTarget = 1;
                firstColorIndex = Colors.BackgroundColor;
              }
              // only one color, and was background? -> force the same bit pattern
              if ( ( usedColors.Count == 1 )
              &&   ( usedColors[0] == Colors.BackgroundColor ) )
              {
                colorTarget     = 1;
              }
              // check for overlaps - two colors are used that would map to the same target pattern?
              Dictionary<int,ColorMappingTarget>       recommendedPattern = new Dictionary<int, ColorMappingTarget>();

              numErrors += DetermineBestMapping( keys, x, y, ForceBitPattern, recommendedPattern, ErrornousBlocks );

              foreach ( byte colorIndex in keys )
              {
                if ( recommendedPattern.ContainsKey( colorIndex ) )
                {
                  if ( recommendedPattern[colorIndex] == ColorMappingTarget.COLOR_1 )
                  {
                    colorTarget = 0;
                  }
                  else if ( recommendedPattern[colorIndex] == ColorMappingTarget.COLOR_2 )
                  {
                    colorTarget = 1;
                  }
                }

                if ( colorTarget == 0 )
                {
                  // upper screen char nibble
                  byte value = screenChar.ByteAt( x + y * WidthChars );
                  value &= 0x0f;
                  value |= (byte)( colorIndex << 4 );

                  screenChar.SetU8At( x + y * WidthChars, value );
                  usedColors[colorIndex] = 1;
                  firstColorIndex = colorIndex;
                }
                else if ( colorTarget == 1 )
                {
                  // lower nibble in screen char
                  byte value = screenChar.ByteAt( x + y * WidthChars );
                  value &= 0xf0;
                  value |= (byte)( colorIndex );

                  screenChar.SetU8At( x + y * WidthChars, value );
                  usedColors[colorIndex] = 2;
                }

                if ( recommendedPattern.ContainsKey( colorIndex ) )
                {
                  if ( recommendedPattern[colorIndex] == ColorMappingTarget.COLOR_2 )
                  {
                    firstColorIndex = colorIndex;
                  }
                }

                ++colorTarget;
              }
            }
            // write out bits
            for ( int charY = 0; charY < 8; ++charY )
            {
              for ( int charX = 0; charX < 8; ++charX )
              {
                byte colorIndex = (byte)Image.GetPixel( x * 8 + charX, y * 8 + charY );
                if ( colorIndex == firstColorIndex )
                {
                  // other color
                  byte colorValue = usedColors[colorIndex];
                  int bitmapIndex = x * 8 + y * WidthChars * 8 + charY;

                  byte value = bitmapData.ByteAt( bitmapIndex );
                  int mask = ( 1 << ( 7 - charX ) );

                  value &= (byte)( 0xff ^ mask );
                  value |= (byte)mask;
                  bitmapData.SetU8At( bitmapIndex, value );
                }
              }
            }
          }
        }
      }
      return numErrors;
    }



    public int ImageToMCBitmapData( Dictionary<int, List<ColorMappingTarget>> ForceBitPattern, List<Formats.CharData> Chars, bool[,] ErrornousBlocks, int CharX, int CharY, int WidthChars, int HeightChars, out GR.Memory.ByteBuffer bitmapData, out GR.Memory.ByteBuffer screenChar, out GR.Memory.ByteBuffer screenColor )
    {
      int numErrors = 0;

      ColorMappingTarget[] bitPattern = new ColorMappingTarget[3] { ColorMappingTarget.BITS_01, ColorMappingTarget.BITS_10, ColorMappingTarget.BITS_11 };
      var  usedBitPattern = new GR.Collections.Set<ColorMappingTarget>();

      Dictionary<int,GR.Collections.Set<ColorMappingTarget>>    usedPatterns = new Dictionary<int, GR.Collections.Set<ColorMappingTarget>>();

      screenChar = new GR.Memory.ByteBuffer( (uint)( WidthChars * HeightChars ) );
      screenColor = new GR.Memory.ByteBuffer( (uint)( WidthChars * HeightChars ) );
      bitmapData = new GR.Memory.ByteBuffer( (uint)( 8 * WidthChars * HeightChars ) );

      GR.Collections.Map<byte, ColorMappingTarget> usedColors = new GR.Collections.Map<byte, ColorMappingTarget>();

      for ( int y = 0; y < HeightChars; ++y )
      {
        for ( int x = 0; x < WidthChars; ++x )
        {
          // ein zeichen-block
          usedColors.Clear();
          usedBitPattern.Clear();
          if ( ErrornousBlocks != null )
          {
            ErrornousBlocks[x, y] = false;
          }
          for ( int charY = 0; charY < 8; ++charY )
          {
            for ( int charX = 0; charX < 4; ++charX )
            {
              byte colorIndex = (byte)Image.GetPixel( x * 8 + charX * 2, y * 8 + charY );
              if ( colorIndex >= 16 )
              {
                if ( Chars != null )
                {
                  Chars[x + y * BlockWidth].Error = "Color index >= 16 (" + colorIndex + ") at " + ( x * 8 + charX * 2 ).ToString() + ", " + ( y * 8 + charY ).ToString() + " (" + charX + "," + charY + ")";
                }
                if ( ErrornousBlocks != null )
                {
                  ErrornousBlocks[x, y] = true;
                }
                ++numErrors;
              }
              if ( colorIndex != Colors.BackgroundColor )
              {
                // remember used color
                usedColors.Add( colorIndex, 0 );
              }
            }
          }
          // more than 3 colors?
          if ( usedColors.Count > 3 )
          {
            if ( Chars != null )
            {
              Chars[x + y * BlockWidth].Error = "Too many colors used";
            }
            if ( ErrornousBlocks != null )
            {
              ErrornousBlocks[x, y] = true;
            }
            ++numErrors;
          }
          else
          {
            if ( usedColors.Count > 0 )
            {
              int colorTarget = 0;
              List<byte> keys = new List<byte>( usedColors.Keys );

              // check for overlaps - two colors are used that would map to the same target pattern?
              Dictionary<int,ColorMappingTarget>       recommendedPattern = new Dictionary<int, ColorMappingTarget>();

              numErrors += DetermineBestMapping( keys, x, y, ForceBitPattern, recommendedPattern, ErrornousBlocks );

              foreach ( byte colorIndex in keys )
              {
                if ( recommendedPattern.ContainsKey( colorIndex ) )
                {
                  usedColors[colorIndex] = recommendedPattern[colorIndex];

                  if ( !usedPatterns.ContainsKey( colorIndex ) )
                  {
                    usedPatterns.Add( colorIndex, new GR.Collections.Set<ColorMappingTarget>() );
                  }
                  usedPatterns[colorIndex].Add( recommendedPattern[colorIndex] );
                  usedBitPattern.Add( recommendedPattern[colorIndex] );

                  switch ( recommendedPattern[colorIndex] )
                  {
                    case ColorMappingTarget.BITS_01:
                      {
                        // upper screen char nibble
                        byte value = screenChar.ByteAt( x + y * WidthChars );
                        value &= 0x0f;
                        value |= (byte)( colorIndex << 4 );

                        screenChar.SetU8At( x + y * WidthChars, value );
                      }
                      break;
                    case ColorMappingTarget.BITS_10:
                      {
                        // lower nibble in screen char
                        byte value = screenChar.ByteAt( x + y * WidthChars );
                        value &= 0xf0;
                        value |= (byte)( colorIndex );

                        screenChar.SetU8At( x + y * WidthChars, value );
                      }
                      break;
                    case ColorMappingTarget.BITS_11:
                      // color ram
                      screenColor.SetU8At( x + y * WidthChars, colorIndex );
                      break;
                  }
                  continue;
                }

                if ( !usedPatterns.ContainsKey( colorIndex ) )
                {
                  usedPatterns.Add( colorIndex, new GR.Collections.Set<ColorMappingTarget>() );
                }
                usedPatterns[colorIndex].Add( bitPattern[colorTarget] );

                colorTarget = 0;
                while ( ( colorTarget < 3 )
                &&      ( usedBitPattern.ContainsValue( bitPattern[colorTarget] ) ) )
                {
                  ++colorTarget;
                }
                usedBitPattern.Add( bitPattern[colorTarget] );

                if ( colorTarget == 0 )
                {
                  // upper screen char nibble
                  byte value = screenChar.ByteAt( x + y * WidthChars );
                  value &= 0x0f;
                  value |= (byte)( colorIndex << 4 );

                  screenChar.SetU8At( x + y * WidthChars, value );
                  usedColors[colorIndex] = ColorMappingTarget.BITS_01;
                }
                else if ( colorTarget == 1 )
                {
                  // lower nibble in screen char
                  byte value = screenChar.ByteAt( x + y * WidthChars );
                  value &= 0xf0;
                  value |= (byte)( colorIndex );

                  screenChar.SetU8At( x + y * WidthChars, value );
                  usedColors[colorIndex] = ColorMappingTarget.BITS_10;
                }
                else if ( colorTarget == 2 )
                {
                  // color ram
                  screenColor.SetU8At( x + y * WidthChars, colorIndex );
                  usedColors[colorIndex] = ColorMappingTarget.BITS_11;
                }
                ++colorTarget;
              }
            }
            // write out bits
            /*
            Debug.Log( "For Char " + x + "," + y );
            foreach ( var usedColor in usedColors )
            {
              Debug.Log( " Color " + usedColor.Key + " = " + usedColor.Value );
            }*/
            for ( int charY = 0; charY < 8; ++charY )
            {
              for ( int charX = 0; charX < 4; ++charX )
              {
                byte colorIndex = (byte)Image.GetPixel( x * 8 + charX * 2, y * 8 + charY );
                if ( colorIndex != Colors.BackgroundColor )
                {
                  // other color
                  byte colorValue = 0;
                  
                  switch ( usedColors[colorIndex] )
                  {
                    case ColorMappingTarget.BITS_01:
                      colorValue = 0x01;
                      break;
                    case ColorMappingTarget.BITS_10:
                      colorValue = 0x02;
                      break;
                    case ColorMappingTarget.BITS_11:
                      colorValue = 0x03;
                      break;
                  }
                  int bitmapIndex = x * 8 + y * 8 * WidthChars + charY;

                  byte value = bitmapData.ByteAt( bitmapIndex );
                  if ( charX == 0 )
                  {
                    value &= 0x3f;
                    value |= (byte)( colorValue << 6 );
                  }
                  else if ( charX == 1 )
                  {
                    value &= 0xcf;
                    value |= (byte)( colorValue << 4 );
                  }
                  else if ( charX == 2 )
                  {
                    value &= 0xf3;
                    value |= (byte)( colorValue << 2 );
                  }
                  else
                  {
                    value &= 0xfc;
                    value |= colorValue;
                  }
                  bitmapData.SetU8At( bitmapIndex, value );
                }
              }
            }
          }
        }
      }

      /*
      Debug.Log( "Used patterns:" );
      foreach ( var entry in usedPatterns )
      {
        Debug.Log( "Index " + entry.Key );
        foreach ( var pattern in entry.Value )
        {
          Debug.Log( " used " + pattern );
        }
      }*/
      return numErrors;
    }



    private int DetermineBestMapping( List<byte> keys, int x, int y, Dictionary<int, List<ColorMappingTarget>> ForceBitPattern, Dictionary<int, ColorMappingTarget> RecommendedPattern, bool[,] ErrornousBlocks )
    {
      int   numErrors = 0;
      Dictionary<int,ColorMappingTarget>    potentialMapping = new Dictionary<int, ColorMappingTarget>();
      Dictionary<int,int>                   potentialMappingIndices = new Dictionary<int,int>();

      int                                   numForcedPatterns = 0;

      foreach ( byte colorIndex in keys )
      {
        if ( ForceBitPattern.ContainsKey( colorIndex ) )
        {
          ++numForcedPatterns;
          potentialMappingIndices[colorIndex] = 0;
        }
      }
      // nothing to map?
      if ( numForcedPatterns == 0 )
      {
        return numErrors;
      }

      bool doneDetermination = false;

      do
      {
        // set current variant
        potentialMapping.Clear();
        foreach ( var entry in potentialMappingIndices )
        {
          potentialMapping[entry.Key] = ForceBitPattern[entry.Key][entry.Value];
        }

        // check current variant for overlaps
        bool    hasDuplicate = false;
        foreach ( var entry in potentialMapping )
        {
          // check if duplicate exists already for this mapping
          foreach ( var otherEntry in potentialMapping )
          {
            if ( otherEntry.Key == entry.Key )
            {
              continue;
            }
            if ( ( entry.Value == otherEntry.Value )
            &&   ( entry.Value != ColorMappingTarget.ANY ) )
            {
              hasDuplicate = true;
            }
          }
        }
        if ( !hasDuplicate )
        {
          // valid variant!
          foreach ( var entry in potentialMapping )
          {
            if ( entry.Value != ColorMappingTarget.ANY )
            {
              RecommendedPattern.Add( entry.Key, entry.Value );
            }
          }
          return 0;
        }

        // next variant
        bool    couldUpdate = false;
        List<int> mappingKeys = new List<int>( potentialMappingIndices.Keys );

        foreach ( var entry in mappingKeys )
        {
          ++potentialMappingIndices[entry];
          if ( potentialMappingIndices[entry] < ForceBitPattern[entry].Count )
          {
            // updated indices, carry on
            couldUpdate = true;
            break;
          }
          potentialMappingIndices[entry] = 0;
        }
        if ( couldUpdate )
        {
          doneDetermination = true;
        }
      }
      while ( !doneDetermination );

      if ( ( ErrornousBlocks != null )
      &&   ( !ErrornousBlocks[x, y] ) )
      {
        ErrornousBlocks[x, y] = true;
        ++numErrors;
      }
      return numErrors;
    }

  }
}
