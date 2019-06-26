using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio.Formats
{
  public class CharsetScreenProject
  {
    public int                          BackgroundColor = 0;
    public int                          MultiColor1 = 0;
    public int                          MultiColor2 = 0;
    public int                          BGColor4 = 0;

    public int                          ScreenWidth = 40;
    public int                          ScreenHeight = 25;

    public int                          ScreenOffsetX = 0;
    public int                          ScreenOffsetY = 0;

    public List<ushort>                 Chars = new List<ushort>( 40 * 25 );

    public string                       ExternalCharset = "";
    public Types.CharsetMode            Mode = C64Studio.Types.CharsetMode.HIRES;

    public Formats.CharsetProject       CharSet = new C64Studio.Formats.CharsetProject();



    public CharsetScreenProject()
    {
      for ( int j = 0; j < ScreenHeight * ScreenWidth; ++j )
      {
        // spaces with white color
        Chars.Add( (ushort)0x0120 );
      }
    }



    public GR.Memory.ByteBuffer SaveToBuffer()
    {
      GR.Memory.ByteBuffer projectFile = new GR.Memory.ByteBuffer();

      GR.IO.FileChunk chunkScreenInfo = new GR.IO.FileChunk( Types.FileChunk.CHARSET_SCREEN_INFO );
      // version
      chunkScreenInfo.AppendU32( 0 );
      // width
      chunkScreenInfo.AppendI32( ScreenWidth );
      // height
      chunkScreenInfo.AppendI32( ScreenHeight );
      chunkScreenInfo.AppendString( "" );
      chunkScreenInfo.AppendI32( (int)Mode );
      chunkScreenInfo.AppendI32( ScreenOffsetX );
      chunkScreenInfo.AppendI32( ScreenOffsetY );

      projectFile.Append( chunkScreenInfo.ToBuffer() );

      GR.IO.FileChunk chunkCharSet = new GR.IO.FileChunk( Types.FileChunk.CHARSET_DATA );
      chunkCharSet.Append( CharSet.SaveToBuffer() );
      projectFile.Append( chunkCharSet.ToBuffer() );

      GR.IO.FileChunk chunkScreenMultiColorData = new GR.IO.FileChunk( Types.FileChunk.MULTICOLOR_DATA );
      chunkScreenMultiColorData.AppendU8( (byte)Mode );
      chunkScreenMultiColorData.AppendU8( (byte)BackgroundColor );
      chunkScreenMultiColorData.AppendU8( (byte)MultiColor1 );
      chunkScreenMultiColorData.AppendU8( (byte)MultiColor2 );
      projectFile.Append( chunkScreenMultiColorData.ToBuffer() );

      GR.IO.FileChunk chunkScreenCharData = new GR.IO.FileChunk( Types.FileChunk.SCREEN_CHAR_DATA );
      for ( int i = 0; i < Chars.Count; ++i )
      {
        chunkScreenCharData.AppendU8( (byte)( Chars[i] & 0xff ) );
      }
      projectFile.Append( chunkScreenCharData.ToBuffer() );

      GR.IO.FileChunk chunkScreenColorData = new GR.IO.FileChunk( Types.FileChunk.SCREEN_COLOR_DATA );
      for ( int i = 0; i < Chars.Count; ++i )
      {
        chunkScreenColorData.AppendU8( (byte)( Chars[i] >> 8 ) );
      }
      projectFile.Append( chunkScreenColorData.ToBuffer() );

      return projectFile;
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
          case Types.FileChunk.CHARSET_SCREEN_INFO:
            {
              uint version  = chunkReader.ReadUInt32();
              ScreenWidth = chunkReader.ReadInt32();
              ScreenHeight = chunkReader.ReadInt32();
              ExternalCharset = chunkReader.ReadString();
              Mode = (C64Studio.Types.CharsetMode)chunkReader.ReadInt32();
              ScreenOffsetX = chunkReader.ReadInt32();
              ScreenOffsetY = chunkReader.ReadInt32();

              Chars = new List<ushort>();
              for ( int i = 0; i < ScreenWidth * ScreenHeight; ++i )
              {
                Chars.Add( (ushort)0x0120 );
              }
            }
            break;
          case Types.FileChunk.MULTICOLOR_DATA:
            Mode = (C64Studio.Types.CharsetMode)chunkReader.ReadUInt8();
            BackgroundColor = chunkReader.ReadUInt8();
            MultiColor1 = chunkReader.ReadUInt8();
            MultiColor2 = chunkReader.ReadUInt8();
            break;
          case Types.FileChunk.SCREEN_CHAR_DATA:
            for ( int i = 0; i < Chars.Count; ++i )
            {
              Chars[i] = (ushort)( ( Chars[i] & 0xff00 ) | chunkReader.ReadUInt8() );
            }
            break;
          case Types.FileChunk.SCREEN_COLOR_DATA:
            for ( int i = 0; i < Chars.Count; ++i )
            {
              Chars[i] = (ushort)( ( Chars[i] & 0x00ff ) | ( chunkReader.ReadUInt8() << 8 ) );
            }
            break;
          case Types.FileChunk.CHARSET_DATA:
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



    public bool ExportToBuffer( out GR.Memory.ByteBuffer CharData, out GR.Memory.ByteBuffer ColorData, out GR.Memory.ByteBuffer CharSetData, int X, int Y, int Width, int Height, bool RowByRow )
    {
      CharData = new GR.Memory.ByteBuffer();
      ColorData = new GR.Memory.ByteBuffer();

      CharSetData = new GR.Memory.ByteBuffer( CharSet.CharacterData() );

      if ( RowByRow )
      {
        // row by row
        for ( int i = 0; i < Height; ++i )
        {
          for ( int x = 0; x < Width; ++x )
          {
            byte newColor = (byte)( ( Chars[( Y + i ) * ScreenWidth + X + x] & 0xff00 ) >> 8 );
            byte newChar = (byte)( Chars[( Y + i ) * ScreenWidth + X + x] & 0xff );

            CharData.AppendU8( newChar );
            ColorData.AppendU8( newColor );
          }
        }
      }
      else
      {
        for ( int x = 0; x < Width; ++x )
        {
          for ( int i = 0; i < Height; ++i )
          {
            byte newColor = (byte)( ( Chars[( Y + i ) * ScreenWidth + X + x] & 0xff00 ) >> 8 );
            byte newChar = (byte)( Chars[( Y + i ) * ScreenWidth + X + x] & 0xff );

            CharData.AppendU8( newChar );
            ColorData.AppendU8( newColor );
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

      List<ushort>    newChars = new List<ushort>();

      int     copyWidth = Math.Min( oldWidth, Width );
      int     copyHeight = Math.Min( oldHeight, Height );

      newChars = new List<ushort>();
      for ( int i = 0; i < ScreenWidth * ScreenHeight; ++i )
      {
        newChars.Add( (ushort)0x0120 );
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
