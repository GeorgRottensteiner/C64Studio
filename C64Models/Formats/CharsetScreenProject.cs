using RetroDevStudio;
using RetroDevStudio.Formats;
using System;
using System.Collections.Generic;
using System.Text;



namespace RetroDevStudio.Formats
{
  public class CharsetScreenProject
  {
    public int                          ScreenOffsetX = 0;
    public int                          ScreenOffsetY = 0;

    // used to add onto char values on export
    public int                          CharOffset = 0;

    public string                       ExternalCharset = "";
    private TextMode                    _Mode = TextMode.COMMODORE_40_X_25_HIRES;

    public Formats.CharsetProject       CharSet = new RetroDevStudio.Formats.CharsetProject();

    public List<CharsetScreen>          Screens = new List<CharsetScreen>() {  new CharsetScreen() };



    public CharsetScreenProject()
    {
    }



    public TextMode Mode
    {
      get
      {
        return _Mode;
      }
      set
      {
        _Mode         = value;
        CharSet.Mode  = Lookup.TextCharModeFromTextMode( _Mode );
        foreach ( var screen in Screens )
        {
          screen.Mode = value;
        }
      }
    }



    public GR.Memory.ByteBuffer SaveToBuffer()
    {
      GR.Memory.ByteBuffer projectFile = new GR.Memory.ByteBuffer();

      GR.IO.FileChunk chunkScreenProjectInfo = new GR.IO.FileChunk( FileChunkConstants.CHARSET_SCREEN_PROJECT_INFO );
      // version
      chunkScreenProjectInfo.AppendI32( 1 );
      chunkScreenProjectInfo.AppendI32( 0 ); // was width, unused now
      chunkScreenProjectInfo.AppendI32( 0 ); // was height, unused now
      chunkScreenProjectInfo.AppendString( "" );
      chunkScreenProjectInfo.AppendI32( (int)Mode );
      chunkScreenProjectInfo.AppendI32( ScreenOffsetX );
      chunkScreenProjectInfo.AppendI32( ScreenOffsetY );
      chunkScreenProjectInfo.AppendI32( CharOffset );

      projectFile.Append( chunkScreenProjectInfo.ToBuffer() );

      GR.IO.FileChunk chunkCharSet = new GR.IO.FileChunk( FileChunkConstants.CHARSET_DATA );
      chunkCharSet.Append( CharSet.SaveToBuffer() );
      projectFile.Append( chunkCharSet.ToBuffer() );

      GR.IO.FileChunk chunkScreenMultiColorData = new GR.IO.FileChunk( FileChunkConstants.MULTICOLOR_DATA );
      chunkScreenMultiColorData.AppendU8( (byte)Mode );
      chunkScreenMultiColorData.AppendU8( (byte)CharSet.Colors.BackgroundColor );
      chunkScreenMultiColorData.AppendU8( (byte)CharSet.Colors.MultiColor1 );
      chunkScreenMultiColorData.AppendU8( (byte)CharSet.Colors.MultiColor2 );
      projectFile.Append( chunkScreenMultiColorData.ToBuffer() );

      foreach ( var screen in Screens )
      {
        var chunkScreen = new GR.IO.FileChunk( FileChunkConstants.CHARSET_SCREEN );

        var chunkScreenInfo = new GR.IO.FileChunk( FileChunkConstants.CHARSET_SCREEN_INFO );
        chunkScreenInfo.AppendString( screen.Name );
        chunkScreenInfo.AppendI32( screen.Width );
        chunkScreenInfo.AppendI32( screen.Height );
        chunkScreenInfo.AppendI32( (int)screen.Mode );
        chunkScreen.Append( chunkScreenInfo.ToBuffer() );

        var chunkScreenCharData = new GR.IO.FileChunk( FileChunkConstants.SCREEN_CHAR_DATA );
        if ( Lookup.NumBytesOfSingleCharacter( Lookup.TextCharModeFromTextMode( screen.Mode ) ) == 2 )
        {
          for ( int i = 0; i < screen.Chars.Count; ++i )
          {
            chunkScreenCharData.AppendU16( (ushort)( screen.Chars[i] & 0xffff ) );
          }
        }
        else
        {
          for ( int i = 0; i < screen.Chars.Count; ++i )
          {
            chunkScreenCharData.AppendU8( (byte)( screen.Chars[i] & 0xffff ) );
          }
        }
        chunkScreen.Append( chunkScreenCharData.ToBuffer() );

        GR.IO.FileChunk chunkScreenColorData = new GR.IO.FileChunk( FileChunkConstants.SCREEN_COLOR_DATA );
        if ( Lookup.NumBytesOfSingleCharacter( Lookup.TextCharModeFromTextMode( screen.Mode ) ) == 2 )
        {
          for ( int i = 0; i < screen.Chars.Count; ++i )
          {
            chunkScreenColorData.AppendU16( (ushort)( screen.Chars[i] >> 16 ) );
          }
        }
        else
        {
          for ( int i = 0; i < screen.Chars.Count; ++i )
          {
            chunkScreenColorData.AppendU8( (byte)( screen.Chars[i] >> 16 ) );
          }
        }
        chunkScreen.Append( chunkScreenColorData.ToBuffer() );

        projectFile.Append( chunkScreen.ToBuffer() );
      }

      return projectFile;
    }



    public bool ReadFromBuffer( GR.Memory.ByteBuffer ProjectFile )
    {
      Screens.Clear();

      GR.IO.MemoryReader    memReader = new GR.IO.MemoryReader( ProjectFile );

      GR.IO.FileChunk chunk = new GR.IO.FileChunk();

      while ( chunk.ReadFromStream( memReader ) )
      {
        GR.IO.MemoryReader chunkReader = chunk.MemoryReader();
        switch ( chunk.Type )
        {
          case FileChunkConstants.CHARSET_SCREEN_PROJECT_INFO:
            {
              int version = chunkReader.ReadInt32();
              if ( version == 0 )
              {
                int screenWidth = chunkReader.ReadInt32();
                int screenHeight = chunkReader.ReadInt32();
                ExternalCharset = chunkReader.ReadString();
                _Mode = (TextMode)chunkReader.ReadInt32();
                ScreenOffsetX = chunkReader.ReadInt32();
                ScreenOffsetY = chunkReader.ReadInt32();
                CharOffset = chunkReader.ReadInt32();

                Screens.Add( new CharsetScreen() { Mode = Mode, Width = screenWidth, Height = screenHeight } );

                Screens[0].Chars = new List<uint>();
                for ( int i = 0; i < screenWidth * screenHeight; ++i )
                {
                  Screens[0].Chars.Add( (uint)0x010020 );
                }
              }
              else if ( version == 1 )
              {
                int unusedScreenWidth = chunkReader.ReadInt32();
                int unusedScreenHeight = chunkReader.ReadInt32();
                ExternalCharset = chunkReader.ReadString();
                _Mode = (TextMode)chunkReader.ReadInt32();
                ScreenOffsetX = chunkReader.ReadInt32();
                ScreenOffsetY = chunkReader.ReadInt32();
                CharOffset = chunkReader.ReadInt32();
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
            for ( int i = 0; i < Screens[0].Chars.Count; ++i )
            {
              if ( Lookup.NumBytesOfSingleCharacter( Lookup.TextCharModeFromTextMode( Mode ) ) == 1 )
              {
                Screens[0].Chars[i] = (uint)( ( Screens[0].Chars[i] & 0xffff0000 ) | chunkReader.ReadUInt8() );
              }
              else
              {
                Screens[0].Chars[i] = (uint)( ( Screens[0].Chars[i] & 0xffff0000 ) | chunkReader.ReadUInt16() );
              }
            }
            break;
          case FileChunkConstants.SCREEN_COLOR_DATA:
            for ( int i = 0; i < Screens[0].Chars.Count; ++i )
            {
              if ( Lookup.NumBytesOfSingleCharacter( Lookup.TextCharModeFromTextMode( Mode ) ) == 1 )
              {
                Screens[0].Chars[i] = (uint)( ( Screens[0].Chars[i] & 0xffff ) | ( (uint)chunkReader.ReadUInt8() << 16 ) );
              }
              else
              {
                Screens[0].Chars[i] = (uint)( ( Screens[0].Chars[i] & 0xffff ) | ( (uint)chunkReader.ReadUInt16() << 16 ) );
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
          case FileChunkConstants.CHARSET_SCREEN:
            {
              GR.IO.FileChunk chunkData = new GR.IO.FileChunk();

              var screen = new CharsetScreen();
              Screens.Add( screen );

              while ( chunkData.ReadFromStream( chunkReader ) )
              {
                GR.IO.MemoryReader subChunkReader = chunkData.MemoryReader();
                switch ( chunkData.Type )
                {
                  case FileChunkConstants.CHARSET_SCREEN_INFO:
                    screen.Name         = subChunkReader.ReadString();
                    screen.Width  = subChunkReader.ReadInt32();
                    screen.Height = subChunkReader.ReadInt32();
                    screen.Mode         = (TextMode)subChunkReader.ReadInt32();

                    screen.SetScreenSize( screen.Width, screen.Height );
                    break;
                  case FileChunkConstants.SCREEN_CHAR_DATA:
                    for ( int i = 0; i < screen.Chars.Count; ++i )
                    {
                      if ( Lookup.NumBytesOfSingleCharacter( Lookup.TextCharModeFromTextMode( screen.Mode ) ) == 1 )
                      {
                        screen.Chars[i] = (uint)( ( screen.Chars[i] & 0xffff0000 ) | subChunkReader.ReadUInt8() );
                      }
                      else
                      {
                        screen.Chars[i] = (uint)( ( screen.Chars[i] & 0xffff0000 ) | subChunkReader.ReadUInt16() );
                      }
                    }
                    break;
                  case FileChunkConstants.SCREEN_COLOR_DATA:
                    for ( int i = 0; i < screen.Chars.Count; ++i )
                    {
                      if ( Lookup.NumBytesOfSingleCharacter( Lookup.TextCharModeFromTextMode( screen.Mode ) ) == 1 )
                      {
                        screen.Chars[i] = (uint)( ( screen.Chars[i] & 0xffff ) | ( (uint)subChunkReader.ReadUInt8() << 16 ) );
                      }
                      else
                      {
                        screen.Chars[i] = (uint)( ( screen.Chars[i] & 0xffff ) | ( (uint)subChunkReader.ReadUInt16() << 16 ) );
                      }
                    }
                    break;
                }
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
      var affectedScreen = Screens[0];

      Info.ScreenCharData   = new GR.Memory.ByteBuffer();
      Info.ScreenColorData  = new GR.Memory.ByteBuffer();
      Info.CharsetData      = new GR.Memory.ByteBuffer( CharSet.CharacterData() );

      int numBytesPerChar = Lookup.NumBytesOfSingleCharacter( Lookup.TextCharModeFromTextMode( Mode ) );
      int numBytesPerCharImage = Lookup.NumBytesOfSingleCharacterBitmap( Lookup.TextCharModeFromTextMode( Mode ) );

      if ( ( Info.Data == ExportCharsetScreenInfo.ExportData.CHARSET )
      &&   ( Info.SelectedCharactersInCharset.Count > 0 ) )
      {
        // we have a list of characters
        var newData = new GR.Memory.ByteBuffer( (uint)( numBytesPerCharImage * Info.SelectedCharactersInCharset.Count ) );
        for ( int i = 0; i < Info.SelectedCharactersInCharset.Count; ++i )
        {
          Info.CharsetData.CopyTo( newData, Info.SelectedCharactersInCharset[i] * numBytesPerCharImage, numBytesPerCharImage, i * numBytesPerCharImage );
        }
        Info.CharsetData = newData;
      }

      

      // NCM mode, one character covers two "slots"
      if ( ( Mode == TextMode.MEGA65_40_X_25_NCM )
      ||   ( Mode == TextMode.MEGA65_80_X_25_NCM ) )
      {
        Info.Area.X = ( Info.Area.X + 1 ) / 2;
        Info.Area.Width = ( Info.Area.Width / 2 );
      }

      if ( Info.RowByRow )
      {
        // row by row
        for ( int i = 0; i < Info.Area.Height; ++i )
        {
          for ( int x = 0; x < Info.Area.Width; ++x )
          {
            // "Smart" way of choosing either one
            byte newColor   = (byte)( affectedScreen.ColorAt( Info.Area.X + x, Info.Area.Y + i )
                                    + affectedScreen.PaletteMappingAt( Info.Area.X + x, Info.Area.Y + i ) );
            ushort newChar  = affectedScreen.CharacterAt( Info.Area.X + x, Info.Area.Y + i );

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
                Info.ScreenColorData.AppendU8( newColor );
              }
              else if ( ( Mode == TextMode.MEGA65_40_X_25_NCM )
              ||        ( Mode == TextMode.MEGA65_80_X_25_NCM ) )
              {
                // set the NCM bit in color byte 0
                Info.ScreenColorData.AppendU16NetworkOrder( 0x0800 );
              }
              else
              {
                Info.ScreenColorData.AppendU8( newColor );
              }
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
            byte newColor   = (byte)( affectedScreen.ColorAt( Info.Area.X + x, Info.Area.Y + i )
                                    + affectedScreen.PaletteMappingAt( Info.Area.X + x, Info.Area.Y + i ) );

            ushort newChar = affectedScreen.CharacterAt( x + Info.Area.X, i + Info.Area.Y );

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
                Info.ScreenColorData.AppendU8( newColor );
              }
              else if ( ( Mode == TextMode.MEGA65_40_X_25_NCM )
              ||        ( Mode == TextMode.MEGA65_80_X_25_NCM ) )
              {
                // set the NCM bit in color byte 0
                Info.ScreenColorData.AppendU16NetworkOrder( 0x0800 );
              }
              else
              {
                Info.ScreenColorData.AppendU8( newColor );
              }
            }
          }
        }
      }
      return true;
    }



  }
}
