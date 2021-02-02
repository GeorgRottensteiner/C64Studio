using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio.Formats
{
  public class CharData
  {
    public GR.Memory.ByteBuffer Data = new GR.Memory.ByteBuffer( 8 );
    public Types.CharsetMode    Mode = C64Studio.Types.CharsetMode.HIRES;
    public int                  Color = 1;
    public GR.Image.MemoryImage Image = null;
    public int                  Category = 0;
    public CharData             Replacement = null;
    public string               Error = "";
    public int                  Index = 0;


    public CharData()
    {
      Image = new GR.Image.MemoryImage( 8, 8, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );
    }

    public CharData Clone()
    {
      CharData copy = new CharData();

      copy.Mode = Mode;
      copy.Color = Color;
      copy.Data = new GR.Memory.ByteBuffer( Data );
      copy.Image = new GR.Image.MemoryImage( Image );
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

    public int            BackgroundColor = 0;
    public int            MultiColor1 = 0;
    public int            MultiColor2 = 0;
    public int            BGColor4 = 0;

    public string         Name = "";
    public string         ExportFilename = "";

    public uint           UsedTiles = 0;
    public int            StartCharacter = 256;
    public int            NumCharacters = 256;
    public bool           ShowGrid = false;

    public List<ushort>   PlaygroundChars = new List<ushort>( 16 * 16 );



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
        PlaygroundChars.Add( 0x100 | 0x20 );
      }
    }



    public GR.Memory.ByteBuffer CharacterData()
    {
      GR.Memory.ByteBuffer projectFile = new GR.Memory.ByteBuffer( (uint)( NumCharacters * 8 ) );

      for ( int i = 0; i < NumCharacters; ++i )
      {
        Characters[i].Data.CopyTo( projectFile, 0, 8, i * 8 );
      }
      return projectFile;
    }



    public GR.Memory.ByteBuffer SaveCharsetToBuffer()
    {
      var charData = new GR.Memory.ByteBuffer( 2048 );
      for ( int i = 0; i < 256; ++i )
      {
        charData.Append( Characters[i].Data );
      }
      return charData;
    }



    public GR.Memory.ByteBuffer SaveToBuffer()
    {
      GR.Memory.ByteBuffer projectFile = new GR.Memory.ByteBuffer();

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
      string name = memIn.ReadString();
      string charsetFilename = memIn.ReadString();
      for ( int i = 0; i < 256; ++i )
      {
        Characters[i].Color = memIn.ReadInt32();
      }
      for ( int i = 0; i < 256; ++i )
      {
        Characters[i].Mode = (Types.CharsetMode)memIn.ReadUInt8();
      }
      BackgroundColor = memIn.ReadInt32();
      MultiColor1 = memIn.ReadInt32();
      MultiColor2 = memIn.ReadInt32();


      for ( int i = 0; i < 256; ++i )
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
      memIn.ReadBlock( charsetData, 2048 );

      for ( int i = 0; i < 256; ++i )
      {
        Characters[i].Data = charsetData.SubBuffer( i * 8, 8 );
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
      for ( int i = 0; i < 256; ++i )
      {
        Characters[i].Category = memIn.ReadInt32();
        if ( ( Characters[i].Category < 0 )
        ||   ( Characters[i].Category >= Categories.Count ) )
        {
          Characters[i].Category = 0;
        }
      }
      NumCharacters = memIn.ReadInt32();
      if ( NumCharacters == 0 )
      {
        NumCharacters = 256;
      }
      ShowGrid = ( memIn.ReadInt32() != 0 );
      StartCharacter = memIn.ReadInt32();
      BGColor4 = memIn.ReadInt32();

      // playground
      int   w = memIn.ReadInt32();
      int   h = memIn.ReadInt32();
      if ( w * h < 256 )
      {
        w = 16;
        h = 16;
      }
      PlaygroundChars = new List<ushort>( w * h );
      for ( int i = 0; i < w * h; ++i )
      {
        PlaygroundChars.Add( memIn.ReadUInt16() );
      }
      return true;
    }



  }
}
