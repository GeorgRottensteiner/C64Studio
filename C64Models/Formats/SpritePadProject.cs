using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio.Formats
{
  public class SpritePadProject
  {
    public class SpriteData
    {
      public GR.Memory.ByteBuffer       Data = new GR.Memory.ByteBuffer( 63 );
      public bool                       Multicolor = false;
      public int                        Color = 1;
      public GR.Image.MemoryImage       Image = null;


      public SpriteData()
      {
        Image = new GR.Image.MemoryImage( 24, 21, System.Drawing.Imaging.PixelFormat.Format8bppIndexed );
      }


      public SpriteData Clone()
      {
        SpriteData copy = new SpriteData();

        copy.Multicolor = Multicolor;
        copy.Color = Color;
        copy.Data = new GR.Memory.ByteBuffer( Data );
        copy.Image = new GR.Image.MemoryImage( Image );
        return copy;
      }
    }



    public class LayerSprite
    {
      public int        X = 0;
      public int        Y = 0;
      public int        Color = 0;
      public int        Index = 0;
      public bool       ExpandX = false;
      public bool       ExpandY = false;
    }

    public class Layer
    {
      public string             Name = "Default";
      public List<LayerSprite>  Sprites = new List<LayerSprite>();
      public int                BackgroundColor = 0;
    }



    public List<SpriteData>      Sprites = new List<SpriteData>( 256 );
    public List<Layer>           SpriteLayers = new List<Layer>();

    public int            BackgroundColor = 0;
    public int            MultiColor1 = 0;
    public int            MultiColor2 = 0;

    public string         Name = "";
    public string         ExportFilename = "";

    public uint           UsedSprites = 0;
    public int            StartIndex = 0;
    public int            NumSprites = 256;
    public bool           ShowGrid = false;



    public SpritePadProject()
    {
      for ( int i = 0; i < 256; ++i )
      {
        Sprites.Add( new SpriteData() );

        CustomRenderer.PaletteManager.ApplyPalette( Sprites[i].Image );
      }
    }



    public GR.Memory.ByteBuffer SaveToBuffer()
    {
      GR.Memory.ByteBuffer projectFile = new GR.Memory.ByteBuffer();

      /*
      // version
      projectFile.AppendU32( 1 );
      projectFile.AppendI32( Sprites.Count );
      // Name
      projectFile.AppendString( Name );
      for ( int i = 0; i < Sprites.Count; ++i )
      {
        projectFile.AppendI32( Sprites[i].Color );
      }
      for ( int i = 0; i < Sprites.Count; ++i )
      {
        projectFile.AppendU8( Sprites[i].Multicolor ? (byte)1 : (byte)0 );
      }
      projectFile.AppendI32( BackgroundColor );
      projectFile.AppendI32( MultiColor1 );
      projectFile.AppendI32( MultiColor2 );
      // generic MC
      projectFile.AppendU32( 0 );
      for ( int i = 0; i < Sprites.Count; ++i )
      {
        projectFile.Append( Sprites[i].Data );
        projectFile.AppendU8( (byte)Sprites[i].Color );
      }
      projectFile.AppendU32( UsedSprites );

      // export name
      projectFile.AppendString( ExportFilename );

      // exportpath
      projectFile.AppendString( "" );

      // desc
      for ( int i = 0; i < Sprites.Count; ++i )
      {
        projectFile.AppendString( "" );
      }

      // testbed (not used anymore, write 0 as number of sprites)
      projectFile.AppendI32( 0 );


      foreach ( var layer in SpriteLayers )
      {
        GR.IO.FileChunk   chunkLayer = new GR.IO.FileChunk( Types.FileChunk.SPRITESET_LAYER );

        GR.IO.FileChunk   chunkLayerInfo = new GR.IO.FileChunk( Types.FileChunk.SPRITESET_LAYER_INFO );
        chunkLayerInfo.AppendString( layer.Name );
        chunkLayerInfo.AppendU8( (byte)layer.BackgroundColor );
        chunkLayer.Append( chunkLayerInfo.ToBuffer() );

        foreach ( var sprite in layer.Sprites )
        {
          GR.IO.FileChunk   chunkLayerSprite = new GR.IO.FileChunk( Types.FileChunk.SPRITESET_LAYER_ENTRY );
          chunkLayerSprite.AppendI32( sprite.Index );
          chunkLayerSprite.AppendU8( (byte)sprite.Color );
          chunkLayerSprite.AppendI32( sprite.X );
          chunkLayerSprite.AppendI32( sprite.Y );
          chunkLayerSprite.AppendU8( (byte)( sprite.ExpandX ? 1 : 0 ) );
          chunkLayerSprite.AppendU8( (byte)( sprite.ExpandY ? 1 : 0 ) );

          chunkLayer.Append( chunkLayerSprite.ToBuffer() );
        }
        projectFile.Append( chunkLayer.ToBuffer() );
      }
      */
      return projectFile;
    }



    public bool ReadFromBuffer( GR.Memory.ByteBuffer DataIn )
    {
      if ( DataIn == null )
      {
        return false;
      }
      SpriteLayers.Clear();

      GR.IO.MemoryReader memIn = DataIn.MemoryReader();

      GR.Memory.ByteBuffer    header = new GR.Memory.ByteBuffer();

      if ( memIn.ReadBlock( header, 9 ) != 9 )
      {
        return false;
      }
      if ( ( header.ByteAt( 0 ) != 0x53 )
      ||   ( header.ByteAt( 1 ) != 0x50 )
      ||   ( header.ByteAt( 2 ) != 0x44 ) )
      {
        // no SPD
        return false;
      }

      NumSprites  = header.ByteAt( 4 ) + 1;
      int     numAnims    = header.ByteAt( 5 ) + 1;

      BackgroundColor = header.ByteAt( 6 );
      MultiColor1     = header.ByteAt( 7 );
      MultiColor2     = header.ByteAt( 8 );

      Sprites = new List<SpriteData>();

      GR.Memory.ByteBuffer    tempData = new GR.Memory.ByteBuffer();
      for ( int i = 0; i < NumSprites; ++i )
      {
        Sprites.Add( new SpriteData() );
        CustomRenderer.PaletteManager.ApplyPalette( Sprites[i].Image );

        tempData.Clear();
        memIn.ReadBlock( tempData, 63 );
        tempData.CopyTo( Sprites[i].Data, 0, 63 );

        Sprites[i].Color = memIn.ReadUInt8();
        Sprites[i].Multicolor = ( ( ( Sprites[i].Color ) & 0x80 ) != 0 );
        Sprites[i].Color &= 0x0f;
      }

      if ( numAnims > 0 )
      {
        GR.Memory.ByteBuffer    animFrom = new GR.Memory.ByteBuffer();
        GR.Memory.ByteBuffer    animTo = new GR.Memory.ByteBuffer();
        GR.Memory.ByteBuffer    animNumFrames = new GR.Memory.ByteBuffer();
        GR.Memory.ByteBuffer    animAttributes = new GR.Memory.ByteBuffer();

        memIn.ReadBlock( animFrom, (uint)numAnims );
        memIn.ReadBlock( animTo, (uint)numAnims );
        memIn.ReadBlock( animNumFrames, (uint)numAnims );
        memIn.ReadBlock( animAttributes, (uint)numAnims );
      }
      UsedSprites = (uint)NumSprites;
      return true;
    }

  }
}
