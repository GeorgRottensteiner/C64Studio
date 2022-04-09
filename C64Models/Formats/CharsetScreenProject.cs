using RetroDevStudio;
using RetroDevStudio.Formats;
using System;
using System.Collections.Generic;
using System.Text;



namespace C64Studio.Formats
{
  public class CharsetScreenProject
  {
    public int                          ScreenWidth = 40;
    public int                          ScreenHeight = 25;

    public int                          ScreenOffsetX = 0;
    public int                          ScreenOffsetY = 0;

    // used to add onto char values on export
    public int                          CharOffset = 0;

    public List<uint>                   Chars = new List<uint>( 40 * 25 );

    public string                       ExternalCharset = "";
    private TextMode                    _Mode = TextMode.COMMODORE_40_X_25_HIRES;

    public Formats.CharsetProject       CharSet = new C64Studio.Formats.CharsetProject();



    public CharsetScreenProject()
    {
      for ( int j = 0; j < ScreenHeight * ScreenWidth; ++j )
      {
        // spaces with white color
        Chars.Add( 0x010020 );
      }
    }



    public TextMode Mode
    {
      get
      {
        return _Mode;
      }
      set
      {
        _Mode = value;
        CharSet.Mode = Lookup.TextCharModeFromTextMode( _Mode );
      }
    }



    public GR.Memory.ByteBuffer SaveToBuffer()
    {
      GR.Memory.ByteBuffer projectFile = new GR.Memory.ByteBuffer();

      GR.IO.FileChunk chunkScreenInfo = new GR.IO.FileChunk( FileChunkConstants.CHARSET_SCREEN_INFO );
      // version
      chunkScreenInfo.AppendI32( 0 );
      // width
      chunkScreenInfo.AppendI32( ScreenWidth );
      // height
      chunkScreenInfo.AppendI32( ScreenHeight );
      chunkScreenInfo.AppendString( "" );
      chunkScreenInfo.AppendI32( (int)Mode );
      chunkScreenInfo.AppendI32( ScreenOffsetX );
      chunkScreenInfo.AppendI32( ScreenOffsetY );
      chunkScreenInfo.AppendI32( CharOffset );

      projectFile.Append( chunkScreenInfo.ToBuffer() );

      GR.IO.FileChunk chunkCharSet = new GR.IO.FileChunk( FileChunkConstants.CHARSET_DATA );
      chunkCharSet.Append( CharSet.SaveToBuffer() );
      projectFile.Append( chunkCharSet.ToBuffer() );

      GR.IO.FileChunk chunkScreenMultiColorData = new GR.IO.FileChunk( FileChunkConstants.MULTICOLOR_DATA );
      chunkScreenMultiColorData.AppendU8( (byte)Mode );
      chunkScreenMultiColorData.AppendU8( (byte)CharSet.Colors.BackgroundColor );
      chunkScreenMultiColorData.AppendU8( (byte)CharSet.Colors.MultiColor1 );
      chunkScreenMultiColorData.AppendU8( (byte)CharSet.Colors.MultiColor2 );
      projectFile.Append( chunkScreenMultiColorData.ToBuffer() );

      GR.IO.FileChunk chunkScreenCharData = new GR.IO.FileChunk( FileChunkConstants.SCREEN_CHAR_DATA );
      if ( Lookup.NumBytesOfSingleCharacter( Lookup.TextCharModeFromTextMode( Mode ) ) == 2 )
      {
        for ( int i = 0; i < Chars.Count; ++i )
        {
          chunkScreenCharData.AppendU16( (ushort)( Chars[i] & 0xffff ) );
        }
      }
      else
      {
        for ( int i = 0; i < Chars.Count; ++i )
        {
          chunkScreenCharData.AppendU8( (byte)( Chars[i] & 0xffff ) );
        }
      }
      projectFile.Append( chunkScreenCharData.ToBuffer() );

      GR.IO.FileChunk chunkScreenColorData = new GR.IO.FileChunk( FileChunkConstants.SCREEN_COLOR_DATA );
      if ( Lookup.NumBytesOfSingleCharacter( Lookup.TextCharModeFromTextMode( Mode ) ) == 2 )
      {
        for ( int i = 0; i < Chars.Count; ++i )
        {
          chunkScreenColorData.AppendU16( (ushort)( Chars[i] >> 16 ) );
        }
      }
      else
      {
        for ( int i = 0; i < Chars.Count; ++i )
        {
          chunkScreenColorData.AppendU8( (byte)( Chars[i] >> 16 ) );
        }
      }
      projectFile.Append( chunkScreenColorData.ToBuffer() );

      return projectFile;
    }



    public ushort CharacterAt( int X, int Y )
    {
      if ( ( X < 0 )
      ||   ( X >= ScreenWidth )
      ||   ( Y < 0 )
      ||   ( Y >= ScreenHeight ) )
      {
        return 0;
      }

      return (ushort)( Chars[Y * ScreenWidth + X] & 0xffff );
    }



    public ushort ColorAt( int X, int Y )
    {
      if ( ( X < 0 )
      ||   ( X >= ScreenWidth )
      ||   ( Y < 0 )
      ||   ( Y >= ScreenHeight ) )
      {
        return 0;
      }

      return (ushort)( Chars[Y * ScreenWidth + X] >> 16 );
    }



    public bool ReadFromBuffer( GR.Memory.ByteBuffer ProjectFile )
    {
      GR.IO.MemoryReader    memReader = new GR.IO.MemoryReader( ProjectFile );

      GR.IO.FileChunk chunk = new GR.IO.FileChunk();

      while ( chunk.ReadFromStream( memReader ) )
      {
        GR.IO.MemoryReader chunkReader = chunk.MemoryReader();
        switch ( chunk.Type )
        {
          case FileChunkConstants.CHARSET_SCREEN_INFO:
            {
              int version = chunkReader.ReadInt32();
              ScreenWidth = chunkReader.ReadInt32();
              ScreenHeight = chunkReader.ReadInt32();
              ExternalCharset = chunkReader.ReadString();
              _Mode = (TextMode)chunkReader.ReadInt32();
              ScreenOffsetX = chunkReader.ReadInt32();
              ScreenOffsetY = chunkReader.ReadInt32();
              CharOffset = chunkReader.ReadInt32();

              Chars = new List<uint>();
              for ( int i = 0; i < ScreenWidth * ScreenHeight; ++i )
              {
                Chars.Add( (uint)0x010020 );
              }
            }
            break;
          case FileChunkConstants.MULTICOLOR_DATA:
            _Mode = (TextMode)chunkReader.ReadUInt8();
            CharSet.Colors.BackgroundColor = chunkReader.ReadUInt8();
            CharSet.Colors.MultiColor1 = chunkReader.ReadUInt8();
            CharSet.Colors.MultiColor2 = chunkReader.ReadUInt8();
            break;
          case FileChunkConstants.SCREEN_CHAR_DATA:
            for ( int i = 0; i < Chars.Count; ++i )
            {
              if ( Lookup.NumBytesOfSingleCharacter( Lookup.TextCharModeFromTextMode( Mode ) ) == 1 )
              {
                Chars[i] = (uint)( ( Chars[i] & 0xffff0000 ) | chunkReader.ReadUInt8() );
              }
              else
              {
                Chars[i] = (uint)( ( Chars[i] & 0xffff0000 ) | chunkReader.ReadUInt16() );
              }
            }
            break;
          case FileChunkConstants.SCREEN_COLOR_DATA:
            for ( int i = 0; i < Chars.Count; ++i )
            {
              if ( Lookup.NumBytesOfSingleCharacter( Lookup.TextCharModeFromTextMode( Mode ) ) == 1 )
              {
                Chars[i] = (uint)( ( Chars[i] & 0xffff ) | ( (uint)chunkReader.ReadUInt8() << 16 ) );
              }
              else
              {
                Chars[i] = (uint)( ( Chars[i] & 0xffff ) | ( (uint)chunkReader.ReadUInt16() << 16 ) );
              }
            }
            break;
          case FileChunkConstants.CHARSET_DATA:
            {
              if ( !CharSet.ReadFromBuffer( chunk ) )
              {
                return false;
              }
            }
            break;
        }
      }
      memReader.Close();

      return true;
    }



    public bool ExportToBuffer( ExportCharsetScreenInfo Info )
    {
      Info.ScreenCharData   = new GR.Memory.ByteBuffer();
      Info.ScreenColorData  = new GR.Memory.ByteBuffer();
      Info.CharsetData      = new GR.Memory.ByteBuffer( CharSet.CharacterData() );

      int numBytesPerChar = Lookup.NumBytesOfSingleCharacter( Lookup.TextCharModeFromTextMode( Mode ) );

      if ( Info.RowByRow )
      {
        // row by row
        for ( int i = 0; i < Info.Area.Height; ++i )
        {
          for ( int x = 0; x < Info.Area.Width; ++x )
          {
            byte newColor = (byte)( ( Chars[( Info.Area.Y + i ) * ScreenWidth + Info.Area.X + x] & 0xff0000 ) >> 16 );
            ushort newChar = (ushort)( Chars[( Info.Area.Y + i ) * ScreenWidth + Info.Area.X + x] & 0xffff );

            newChar = (ushort)( newChar + CharOffset );

            if ( numBytesPerChar == 2 )
            {
              Info.ScreenCharData.AppendU16( newChar );
            }
            else
            {
              Info.ScreenCharData.AppendU8( (byte)newChar );
            }
            if ( Lookup.TextModeUsesColor( Mode ) )
            {
              if ( ( Mode == TextMode.MEGA65_80_X_25_HIRES )
              ||   ( Mode == TextMode.MEGA65_40_X_25_HIRES ) )
              {
                // colors >= 16 and < 32 need to be shifted up
                if ( newColor >= 16 )
                {
                  newColor += 64 - 16;
                }
              }
              Info.ScreenColorData.AppendU8( newColor );
            }
          }
        }
      }
      else
      {
        for ( int x = 0; x < Info.Area.Width; ++x )
        {
          for ( int i = 0; i < Info.Area.Height; ++i )
          {
            byte newColor = (byte)( ( Chars[( Info.Area.Y + i ) * ScreenWidth + Info.Area.X + x] & 0xff0000 ) >> 16 );
            ushort newChar = (ushort)( Chars[( Info.Area.Y + i ) * ScreenWidth + Info.Area.X + x] & 0xffff );

            newChar = (ushort)( newChar + CharOffset );

            if ( numBytesPerChar == 2 )
            {
              Info.ScreenCharData.AppendU16( newChar );
            }
            else
            {
              Info.ScreenCharData.AppendU8( (byte)newChar );
            }
            if ( Lookup.TextModeUsesColor( Mode ) )
            {
              if ( ( Mode == TextMode.MEGA65_80_X_25_HIRES )
              ||   ( Mode == TextMode.MEGA65_40_X_25_HIRES ) )
              {
                // colors >= 16 and < 32 need to be shifted up
                if ( newColor >= 16 )
                {
                  newColor += 64 - 16;
                }
              }
              Info.ScreenColorData.AppendU8( newColor );
            }
          }
        }
      }
      return true;
    }



    public void SetScreenSize( int Width, int Height )
    {
      if ( ( Width == ScreenWidth )
      &&   ( Height == ScreenHeight ) )
      {
        // nothing to do
        return;
      }
      int     oldWidth = ScreenWidth;
      int     oldHeight = ScreenHeight;

      ScreenWidth = Width;
      ScreenHeight = Height;

      List<uint>    newChars = new List<uint>();

      int     copyWidth = Math.Min( oldWidth, Width );
      int     copyHeight = Math.Min( oldHeight, Height );

      newChars = new List<uint>();
      for ( int i = 0; i < ScreenWidth * ScreenHeight; ++i )
      {
        newChars.Add( (uint)0x010020 );
      }
      // copy over orig chars if possible
      for ( int i = 0; i < copyWidth; ++i )
      {
        for ( int j = 0; j < copyHeight; ++j )
        {
          newChars[i + Width * j] = Chars[i + oldWidth * j];
        }
      }
      Chars = newChars;
    }



  }
}
