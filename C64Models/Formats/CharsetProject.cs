using RetroDevStudio;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.Text;



namespace C64Studio.Formats
{
  public class CharData
  {
    public int                  Category = 0;
    public CharData             Replacement = null;
    public string               Error = "";
    public int                  Index = 0;
    public GraphicTile          Tile = new GraphicTile();


    public CharData()
    {
    }



    public CharData Clone()
    {
      CharData copy = new CharData();

      copy.Tile = new GraphicTile( Tile );
      copy.Category = Category;
      copy.Replacement = Replacement;
      copy.Error = Error;
      copy.Index = Index;
      return copy;
    }
  }



  public class CharsetProject
  {
    public List<CharData> Characters = new List<CharData>( 256 );

    public List<string>   Categories = new List<string>();

    public string         Name = "";
    public string         ExportFilename = "";

    public uint           UsedTiles = 0;
    public int            ExportStartCharacter = 256;
    public int            ExportNumCharacters = 256;
    public bool           ShowGrid = false;
    public int            TotalNumberOfCharacters = 256;

    private TextCharMode  _Mode = TextCharMode.COMMODORE_HIRES;

    public ColorSettings  Colors = new ColorSettings();

    public List<uint>     PlaygroundChars = new List<uint>( 16 * 16 );
    public int            PlaygroundWidth = 16;
    public int            PlaygroundHeight = 16;



    public CharsetProject()
    {
      for ( int i = 0; i < 256; ++i )
      {
        Characters.Add( new CharData() );
      }
      Categories.Add( "Uncategorized" );
      for ( int i = 0; i < 16 * 16; ++i )
      {
        // white spaces
        PlaygroundChars.Add( 0x10000 | 0x20 );
      }
      Colors.Palette = PaletteManager.PaletteFromMachine( MachineType.C64 );
    }



    public TextCharMode Mode
    {
      get
      {
        return _Mode;
      }
      set
      {
        _Mode = value;
      }
    }



    public GR.Memory.ByteBuffer CharacterData()
    {
      return CharacterData( 0, ExportNumCharacters );
    }



    public GR.Memory.ByteBuffer CharacterData( int StartIndex, int Count )
    {
      if ( ( StartIndex < 0 )
      ||   ( StartIndex + Count >= TotalNumberOfCharacters ) )
      {
        return new GR.Memory.ByteBuffer();
      }

      GR.Memory.ByteBuffer projectFile = new GR.Memory.ByteBuffer( (uint)( Count * Lookup.NumBytesOfSingleCharacter( Mode ) ) );

      for ( int i = 0; i < Count; ++i )
      {
        Characters[StartIndex + i].Tile.Data.CopyTo( projectFile, 0, Lookup.NumBytesOfSingleCharacter( Mode ), i * Lookup.NumBytesOfSingleCharacter( Mode ) );
      }
      return projectFile;
    }



    public GR.Memory.ByteBuffer SaveCharsetToBuffer()
    {
      var charData = new GR.Memory.ByteBuffer( (uint)( TotalNumberOfCharacters * Lookup.NumBytesOfSingleCharacter( Mode ) ) );
      for ( int i = 0; i < TotalNumberOfCharacters; ++i )
      {
        charData.Append( Characters[i].Tile.Data );
      }
      return charData;
    }



    public GR.Memory.ByteBuffer SaveToBuffer()
    {
      GR.Memory.ByteBuffer projectFile = new GR.Memory.ByteBuffer();

      // version
      projectFile.AppendU32( 2 );

      var chunkCharsetProject = new GR.IO.FileChunk( RetroDevStudio.FileChunkConstants.CHARSET_PROJECT );


      var chunkCharsetInfo = new GR.IO.FileChunk( RetroDevStudio.FileChunkConstants.CHARSET_INFO );
      chunkCharsetInfo.AppendI32( (int)Mode );
      chunkCharsetInfo.AppendI32( TotalNumberOfCharacters );
      chunkCharsetInfo.AppendI32( ShowGrid ? 1 : 0 );
      chunkCharsetProject.Append( chunkCharsetInfo.ToBuffer() );


      var chunkColorSettings = new GR.IO.FileChunk( RetroDevStudio.FileChunkConstants.CHARSET_COLOR_SETTINGS );

      chunkColorSettings.AppendI32( Colors.BackgroundColor );
      chunkColorSettings.AppendI32( Colors.MultiColor1 );
      chunkColorSettings.AppendI32( Colors.MultiColor2 );
      chunkColorSettings.AppendI32( Colors.BGColor4 );

      chunkCharsetProject.Append( chunkColorSettings.ToBuffer() );

      var chunkPalette = new GR.IO.FileChunk( RetroDevStudio.FileChunkConstants.PALETTE );
      chunkPalette.AppendI32( Colors.Palette.NumColors );
      for ( int i = 0; i < Colors.Palette.NumColors; ++i )
      {
        chunkPalette.AppendU32( Colors.Palette.ColorValues[i] );
      }
      chunkCharsetProject.Append( chunkPalette.ToBuffer() );

      var chunkExport = new GR.IO.FileChunk( RetroDevStudio.FileChunkConstants.CHARSET_EXPORT );

      chunkExport.AppendI32( ExportStartCharacter );
      chunkExport.AppendI32( ExportNumCharacters );
      chunkExport.AppendString( ExportFilename );

      chunkCharsetProject.Append( chunkExport.ToBuffer() );


      foreach ( var character in Characters )
      {
        var chunkCharsetChar = new GR.IO.FileChunk( RetroDevStudio.FileChunkConstants.CHARSET_CHAR );

        //chunkCharsetChar.AppendI32( (int)character.Mode );
        chunkCharsetChar.AppendI32( 0 );    // was mode
        chunkCharsetChar.AppendI32( character.Tile.CustomColor );
        chunkCharsetChar.AppendI32( character.Category );
        chunkCharsetChar.AppendI32( (int)character.Tile.Data.Length );
        chunkCharsetChar.Append( character.Tile.Data );

        chunkCharsetProject.Append( chunkCharsetChar.ToBuffer() );
      }

      foreach ( var category in Categories )
      {
        var chunkCategory = new GR.IO.FileChunk( RetroDevStudio.FileChunkConstants.CHARSET_CATEGORY );
        chunkCategory.AppendString( category );

        chunkCharsetProject.Append( chunkCategory.ToBuffer() );
      }

      var chunkPlayground = new GR.IO.FileChunk( RetroDevStudio.FileChunkConstants.CHARSET_PLAYGROUND );

      chunkPlayground.AppendI32( PlaygroundWidth );
      chunkPlayground.AppendI32( PlaygroundHeight );
      for ( int i = 0; i < PlaygroundChars.Count; ++i )
      {
        // 16 bit index, 16 bit color
        chunkPlayground.AppendU32( PlaygroundChars[i] );
      }
      chunkCharsetProject.Append( chunkPlayground.ToBuffer() );

      projectFile.Append( chunkCharsetProject.ToBuffer() );

      /*
      // version
      projectFile.AppendU32( 1 );
      // Name
      projectFile.AppendString( System.IO.Path.GetFileNameWithoutExtension( Name ) );
      // charset Filename
      projectFile.AppendString( System.IO.Path.GetFileNameWithoutExtension( Name ) );

      for ( int i = 0; i < 256; ++i )
      {
        projectFile.AppendI32( Characters[i].Color );
      }
      for ( int i = 0; i < 256; ++i )
      {
        projectFile.AppendU8( (byte)Characters[i].Mode );
      }
      projectFile.AppendI32( BackgroundColor );
      projectFile.AppendI32( MultiColor1 );
      projectFile.AppendI32( MultiColor2 );

      for ( int i = 0; i < 256; ++i )
      {
        // Tile colors
        projectFile.AppendI32( 0 );
        projectFile.AppendI32( 0 );
        projectFile.AppendI32( 0 );
        projectFile.AppendI32( 0 );
        // Tile chars
        projectFile.AppendI32( 0 );
        projectFile.AppendI32( 0 );
        projectFile.AppendI32( 0 );
        projectFile.AppendI32( 0 );
      }

      // generic multi color
      projectFile.AppendI32( 0 );

      // test bed
      projectFile.Append( new GR.Memory.ByteBuffer( 64 ) );

      // charset data
      for ( int i = 0; i < 256; ++i )
      {
        projectFile.Append( Characters[i].Data );
      }

      // used tiles
      projectFile.AppendU32( UsedTiles );

      // export name
      projectFile.AppendString( ExportFilename );
      // export path block table
      projectFile.AppendString( "" );
      // export path charset
      projectFile.AppendString( "" );
      // export path editor tiles
      projectFile.AppendString( "" );
      // categories
      projectFile.AppendI32( Categories.Count );
      for ( int i = 0; i < Categories.Count; ++i )
      {
        projectFile.AppendI32( i );
        projectFile.AppendString( Categories[i] );
      }
      for ( int i = 0; i < 256; ++i )
      {
        projectFile.AppendI32( Characters[i].Category );
      }
      projectFile.AppendI32( NumCharacters );
      projectFile.AppendI32( ShowGrid ? 1 : 0 );
      projectFile.AppendI32( StartCharacter );
      projectFile.AppendI32( BGColor4 );

      // playground
      projectFile.AppendI32( 16 );  // w
      projectFile.AppendI32( 16 );  // h

      for ( int i = 0; i < PlaygroundChars.Count; ++i )
      {
        projectFile.AppendU16( PlaygroundChars[i] );
      }

      projectFile.AppendI32( (int)Mode );
      */
      return projectFile;
    }



    public bool ReadFromBuffer( GR.Memory.ByteBuffer DataIn )
    {
      if ( DataIn == null )
      {
        return false;
      }
      GR.IO.MemoryReader memIn = DataIn.MemoryReader();

      uint version = memIn.ReadUInt32();

      if ( version == 1 )
      {
        TotalNumberOfCharacters = 256;

        string name = memIn.ReadString();
        string charsetFilename = memIn.ReadString();
        for ( int i = 0; i < TotalNumberOfCharacters; ++i )
        {
          Characters[i].Tile.CustomColor = memIn.ReadInt32();
        }
        for ( int i = 0; i < TotalNumberOfCharacters; ++i )
        {
          memIn.ReadUInt8();
          //Characters[i].Mode = (TextCharMode)memIn.ReadUInt8();
        }
        Colors.BackgroundColor  = memIn.ReadInt32();
        Colors.MultiColor1      = memIn.ReadInt32();
        Colors.MultiColor2      = memIn.ReadInt32();


        for ( int i = 0; i < TotalNumberOfCharacters; ++i )
        {
          int tileColor1 = memIn.ReadInt32();
          int tileColor2 = memIn.ReadInt32();
          int tileColor3 = memIn.ReadInt32();
          int tileColor4 = memIn.ReadInt32();
          int tileChar1 = memIn.ReadInt32();
          int tileChar2 = memIn.ReadInt32();
          int tileChar3 = memIn.ReadInt32();
          int tileChar4 = memIn.ReadInt32();
        }

        bool genericMulticolor = ( memIn.ReadInt32() != 0 );
        GR.Memory.ByteBuffer testbed = new GR.Memory.ByteBuffer();
        memIn.ReadBlock( testbed, 64 );

        GR.Memory.ByteBuffer charsetData = new GR.Memory.ByteBuffer();
        memIn.ReadBlock( charsetData, (uint)( TotalNumberOfCharacters * 8 ) );

        for ( int i = 0; i < TotalNumberOfCharacters; ++i )
        {
          Characters[i].Tile.Data = charsetData.SubBuffer( i * 8, 8 );
        }

        UsedTiles = memIn.ReadUInt32();

        ExportFilename = memIn.ReadString();
        string exportPathBlockTable = memIn.ReadString();
        string exportPathCharset = memIn.ReadString();
        string exportPathEditorTiles = memIn.ReadString();

        // categories
        Categories.Clear();
        int categoryCount = memIn.ReadInt32();
        for ( int i = 0; i < categoryCount; ++i )
        {
          int catKey = memIn.ReadInt32();
          string catName = memIn.ReadString();

          Categories.Add( catName );
        }
        if ( Categories.Count == 0 )
        {
          // add default category
          Categories.Add( "Uncategorized" );
        }
        for ( int i = 0; i < TotalNumberOfCharacters; ++i )
        {
          Characters[i].Category = memIn.ReadInt32();
          if ( ( Characters[i].Category < 0 )
          || ( Characters[i].Category >= Categories.Count ) )
          {
            Characters[i].Category = 0;
          }
        }
        ExportNumCharacters = memIn.ReadInt32();
        if ( ExportNumCharacters < TotalNumberOfCharacters )
        {
          ExportNumCharacters = TotalNumberOfCharacters;
        }
        ShowGrid = ( memIn.ReadInt32() != 0 );
        ExportStartCharacter = memIn.ReadInt32();
        Colors.BGColor4 = memIn.ReadInt32();

        // playground
        int   w = memIn.ReadInt32();
        int   h = memIn.ReadInt32();
        if ( w * h < 256 )
        {
          w = 16;
          h = 16;
        }
        PlaygroundChars = new List<uint>( w * h );
        for ( int i = 0; i < w * h; ++i )
        {
          ushort  charInfo = memIn.ReadUInt16();
          PlaygroundChars.Add( (uint)( ( charInfo & 0xff ) | ( ( charInfo & 0xff00 ) << 8 ) ) );
        }

        Mode = (TextCharMode)memIn.ReadInt32();
      }
      else if ( version == 2 )
      {
        Characters.Clear();
        Categories.Clear();
        TotalNumberOfCharacters = 256;
        Mode = TextCharMode.COMMODORE_HIRES;

        var chunk = new GR.IO.FileChunk();

        while ( chunk.ReadFromStream( memIn ) )
        {
          if ( chunk.Type == FileChunkConstants.CHARSET_PROJECT )
          {
            var chunkIn = chunk.MemoryReader();

            var subChunk = new GR.IO.FileChunk();

            while ( subChunk.ReadFromStream( chunkIn ) )
            {
              var subMemIn = subChunk.MemoryReader();
              switch ( subChunk.Type )
              {
                case FileChunkConstants.CHARSET_INFO:
                  Mode = (TextCharMode)subMemIn.ReadInt32();
                  TotalNumberOfCharacters = subMemIn.ReadInt32();
                  ShowGrid = ( ( subMemIn.ReadInt32() & 1 ) == 1 );
                  break;
                case FileChunkConstants.CHARSET_COLOR_SETTINGS:
                  Colors.BackgroundColor = subMemIn.ReadInt32();
                  Colors.MultiColor1 = subMemIn.ReadInt32();
                  Colors.MultiColor2 = subMemIn.ReadInt32();
                  Colors.BGColor4 = subMemIn.ReadInt32();
                  break;
                case FileChunkConstants.PALETTE:
                  {
                    Colors.Palette = new Palette( subMemIn.ReadInt32() );
                    for ( int i = 0; i < Colors.Palette.NumColors; ++i )
                    {
                      Colors.Palette.ColorValues[i] = subMemIn.ReadUInt32();
                    }
                    Colors.Palette.CreateBrushes();
                  }
                  break;
                case FileChunkConstants.CHARSET_EXPORT:
                  ExportStartCharacter = subMemIn.ReadInt32();
                  ExportNumCharacters = subMemIn.ReadInt32();
                  ExportFilename = subMemIn.ReadString();
                  break;
                case FileChunkConstants.CHARSET_CHAR:
                  {
                    var charData = new CharData();
                    charData.Tile.Mode = Lookup.GraphicTileModeFromTextCharMode( Mode );

                    subMemIn.ReadInt32(); // was TextCharMode
                    charData.Tile.CustomColor = subMemIn.ReadInt32();
                    charData.Category = subMemIn.ReadInt32();

                    int dataLength = subMemIn.ReadInt32();
                    charData.Tile.Data = new GR.Memory.ByteBuffer();
                    subMemIn.ReadBlock( charData.Tile.Data, (uint)dataLength );

                    Characters.Add( charData );
                  }
                  break;
                case FileChunkConstants.CHARSET_CATEGORY:
                  Categories.Add( subMemIn.ReadString() );
                  break;
                case FileChunkConstants.CHARSET_PLAYGROUND:
                  PlaygroundWidth = subMemIn.ReadInt32();
                  PlaygroundHeight = subMemIn.ReadInt32();

                  PlaygroundChars = new List<uint>( PlaygroundWidth * PlaygroundHeight );

                  for ( int i = 0; i < PlaygroundWidth * PlaygroundHeight; ++i )
                  {
                    // 16 bit index, 16 bit color
                    PlaygroundChars.Add( subMemIn.ReadUInt32() );
                  }
                  break;
              }
            }
          }
        }
      }
      else
      {
        return false;
      }
      return true;
    }



  }
}
