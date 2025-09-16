﻿using GR.Memory;
using RetroDevStudio;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace RetroDevStudio.Formats
{
  public class SpriteProject
  {
    public enum SpriteProjectMode
    {
      [Description( "C64/Mega65 24x21" )]
      COMMODORE_24_X_21_HIRES_OR_MC,
      [Description( "Mega65 64x21" )]
      MEGA65_64_X_21_HIRES_OR_MC,
      [Description( "Mega65 16x21 16 colors" )]
      MEGA65_16_X_21_16_COLORS,
      [Description( "Commander X16 8x8 16 colors" )]
      COMMANDER_X16_8_8_16_COLORS,
      [Description( "Commander X16 16x8 16 colors" )]
      COMMANDER_X16_16_8_16_COLORS,
      [Description( "Commander X16 32x8 16 colors" )]
      COMMANDER_X16_32_8_16_COLORS,
      [Description( "Commander X16 64x8 16 colors" )]
      COMMANDER_X16_64_8_16_COLORS,
      [Description( "Commander X16 8x16 16 colors" )]
      COMMANDER_X16_8_16_16_COLORS,
      [Description( "Commander X16 16x16 16 colors" )]
      COMMANDER_X16_16_16_16_COLORS,
      [Description( "Commander X16 32x16 16 colors" )]
      COMMANDER_X16_32_16_16_COLORS,
      [Description( "Commander X16 64x16 16 colors" )]
      COMMANDER_X16_64_16_16_COLORS,
      [Description( "Commander X16 8x32 16 colors" )]
      COMMANDER_X16_8_32_16_COLORS,
      [Description( "Commander X16 16x32 16 colors" )]
      COMMANDER_X16_16_32_16_COLORS,
      [Description( "Commander X16 32x32 16 colors" )]
      COMMANDER_X16_32_32_16_COLORS,
      [Description( "Commander X16 64x32 16 colors" )]
      COMMANDER_X16_64_32_16_COLORS,
      [Description( "Commander X16 8x64 16 colors" )]
      COMMANDER_X16_8_64_16_COLORS,
      [Description( "Commander X16 16x64 16 colors" )]
      COMMANDER_X16_16_64_16_COLORS,
      [Description( "Commander X16 32x64 16 colors" )]
      COMMANDER_X16_32_64_16_COLORS,
      [Description( "Commander X16 64x64 16 colors" )]
      COMMANDER_X16_64_64_16_COLORS,
      [Description( "Commander X16 8x8 256 colors" )]
      COMMANDER_X16_8_8_256_COLORS,
      [Description( "Commander X16 16x8 256 colors" )]
      COMMANDER_X16_16_8_256_COLORS,
      [Description( "Commander X16 32x8 256 colors" )]
      COMMANDER_X16_32_8_256_COLORS,
      [Description( "Commander X16 64x8 256 colors" )]
      COMMANDER_X16_64_8_256_COLORS,
      [Description( "Commander X16 8x16 256 colors" )]
      COMMANDER_X16_8_16_256_COLORS,
      [Description( "Commander X16 16x16 256 colors" )]
      COMMANDER_X16_16_16_256_COLORS,
      [Description( "Commander X16 32x16 256 colors" )]
      COMMANDER_X16_32_16_256_COLORS,
      [Description( "Commander X16 64x16 256 colors" )]
      COMMANDER_X16_64_16_256_COLORS,
      [Description( "Commander X16 8x32 256 colors" )]
      COMMANDER_X16_8_32_256_COLORS,
      [Description( "Commander X16 16x32 256 colors" )]
      COMMANDER_X16_16_32_256_COLORS,
      [Description( "Commander X16 32x32 256 colors" )]
      COMMANDER_X16_32_32_256_COLORS,
      [Description( "Commander X16 64x32 256 colors" )]
      COMMANDER_X16_64_32_256_COLORS,
      [Description( "Commander X16 8x64 256 colors" )]
      COMMANDER_X16_8_64_256_COLORS,
      [Description( "Commander X16 16x64 256 colors" )]
      COMMANDER_X16_16_64_256_COLORS,
      [Description( "Commander X16 32x64 256 colors" )]
      COMMANDER_X16_32_64_256_COLORS,
      [Description( "Commander X16 64x64 256 colors" )]
      COMMANDER_X16_64_64_256_COLORS
    }



    public class SpriteData
    {
      public GraphicTile                Tile = null;
      public SpriteMode                 Mode = SpriteMode.COMMODORE_24_X_21_HIRES;


      public SpriteData( ColorSettings Settings )
      {
        Tile = new GraphicTile( 24, 21, GraphicTileMode.COMMODORE_HIRES, Settings );
        Tile.CustomColor = 1;
      }


      public SpriteData( SpriteData Other )
      {
        Mode          = Other.Mode;
        Tile          = new GraphicTile( Other.Tile );
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
      public int                DelayMS = 0;
    }



    public List<SpriteData>       Sprites = new List<SpriteData>( 256 );
    public List<Layer>            SpriteLayers = new List<Layer>();

    public ColorSettings  Colors = new ColorSettings();

    public string         Name = "";
    public string         ExportFilename = "";

    public int            ExportSpriteCount = 0;
    public int            ExportStartIndex = 0;
    public int            TotalNumberOfSprites = 256;
    public bool           ShowGrid = false;

    public SpriteProjectMode    Mode = SpriteProjectMode.COMMODORE_24_X_21_HIRES_OR_MC;



    public SpriteProject()
    {
      Colors.Palette = ConstantData.DefaultPaletteC64();
      for ( int i = 0; i < TotalNumberOfSprites; ++i )
      {
        Sprites.Add( new SpriteData( Colors ) );
        PaletteManager.ApplyPalette( Sprites[i].Tile.Image );
      }
    }



    public GR.Memory.ByteBuffer SaveToBuffer()
    {
      GR.Memory.ByteBuffer projectFile = new GR.Memory.ByteBuffer();

      projectFile.AppendU32( 2 );

      var chunkProject = new GR.IO.FileChunk( FileChunkConstants.SPRITESET_PROJECT );

      var chunkInfo = new GR.IO.FileChunk( FileChunkConstants.SPRITESET_INFO );
      chunkInfo.AppendI32( TotalNumberOfSprites );
      chunkInfo.AppendString( Name );
      chunkInfo.AppendString( ExportFilename );
      chunkInfo.AppendI32( ExportStartIndex );
      chunkInfo.AppendI32( ExportSpriteCount );
      chunkProject.Append( chunkInfo.ToBuffer() );

      GR.IO.FileChunk chunkScreenMultiColorData = new GR.IO.FileChunk( FileChunkConstants.MULTICOLOR_DATA );
      chunkScreenMultiColorData.AppendI32( (byte)Mode );
      chunkScreenMultiColorData.AppendI32( (byte)Colors.BackgroundColor );
      chunkScreenMultiColorData.AppendI32( (byte)Colors.MultiColor1 );
      chunkScreenMultiColorData.AppendI32( (byte)Colors.MultiColor2 );
      chunkProject.Append( chunkScreenMultiColorData.ToBuffer() );

      foreach ( var pal in Colors.Palettes )
      {
        chunkProject.Append( pal.ToBuffer() );
      }

      foreach ( var sprite in Sprites )
      {
        GR.IO.FileChunk chunkSprite = new GR.IO.FileChunk( FileChunkConstants.SPRITESET_SPRITE );
        chunkSprite.AppendI32( (int)sprite.Mode );
        chunkSprite.AppendI32( (int)sprite.Tile.Mode );
        chunkSprite.AppendI32( (int)sprite.Tile.CustomColor );
        chunkSprite.AppendI32( sprite.Tile.Width );
        chunkSprite.AppendI32( sprite.Tile.Height );
        chunkSprite.AppendI32( (int)sprite.Tile.Data.Length );
        chunkSprite.Append( sprite.Tile.Data );
        chunkSprite.AppendI32( sprite.Tile.Colors.ActivePalette );
        chunkSprite.AppendI32( sprite.Tile.Colors.PaletteOffset );

        chunkProject.Append( chunkSprite.ToBuffer() );
      }

      foreach ( var layer in SpriteLayers )
      {
        GR.IO.FileChunk   chunkLayer = new GR.IO.FileChunk( FileChunkConstants.SPRITESET_LAYER );

        GR.IO.FileChunk   chunkLayerInfo = new GR.IO.FileChunk( FileChunkConstants.SPRITESET_LAYER_INFO );
        chunkLayerInfo.AppendString( layer.Name );
        chunkLayerInfo.AppendI32( (byte)layer.BackgroundColor );
        chunkLayerInfo.AppendI32( layer.DelayMS );
        chunkLayer.Append( chunkLayerInfo.ToBuffer() );

        foreach ( var sprite in layer.Sprites )
        {
          GR.IO.FileChunk   chunkLayerSprite = new GR.IO.FileChunk( FileChunkConstants.SPRITESET_LAYER_ENTRY );
          chunkLayerSprite.AppendI32( sprite.Index );
          chunkLayerSprite.AppendI32( (byte)sprite.Color );
          chunkLayerSprite.AppendI32( sprite.X );
          chunkLayerSprite.AppendI32( sprite.Y );
          chunkLayerSprite.AppendI32( (byte)( sprite.ExpandX ? 1 : 0 ) );
          chunkLayerSprite.AppendI32( (byte)( sprite.ExpandY ? 1 : 0 ) );

          chunkLayer.Append( chunkLayerSprite.ToBuffer() );
        }
        chunkProject.Append( chunkLayer.ToBuffer() );
      }
      projectFile.Append( chunkProject.ToBuffer() );

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
        projectFile.AppendU8( (byte)Sprites[i].Mode );
      }
      projectFile.AppendI32( Colors.BackgroundColor );
      projectFile.AppendI32( Colors.MultiColor1 );
      projectFile.AppendI32( Colors.MultiColor2 );
      // generic MC
      projectFile.AppendU32( 0 );
      for ( int i = 0; i < Sprites.Count; ++i )
      {
        projectFile.Append( Sprites[i].Tile.Data );
        projectFile.AppendU8( (byte)Sprites[i].Color );
      }
      projectFile.AppendU32( ExportSpriteCount );

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
        GR.IO.FileChunk   chunkLayer = new GR.IO.FileChunk( FileChunkConstants.SPRITESET_LAYER );

        GR.IO.FileChunk   chunkLayerInfo = new GR.IO.FileChunk( FileChunkConstants.SPRITESET_LAYER_INFO );
        chunkLayerInfo.AppendString( layer.Name );
        chunkLayerInfo.AppendU8( (byte)layer.BackgroundColor );
        chunkLayerInfo.AppendI32( layer.DelayMS );
        chunkLayer.Append( chunkLayerInfo.ToBuffer() );

        foreach ( var sprite in layer.Sprites )
        {
          GR.IO.FileChunk   chunkLayerSprite = new GR.IO.FileChunk( FileChunkConstants.SPRITESET_LAYER_ENTRY );
          chunkLayerSprite.AppendI32( sprite.Index );
          chunkLayerSprite.AppendU8( (byte)sprite.Color );
          chunkLayerSprite.AppendI32( sprite.X );
          chunkLayerSprite.AppendI32( sprite.Y );
          chunkLayerSprite.AppendU8( (byte)( sprite.ExpandX ? 1 : 0 ) );
          chunkLayerSprite.AppendU8( (byte)( sprite.ExpandY ? 1 : 0 ) );

          chunkLayer.Append( chunkLayerSprite.ToBuffer() );
        }
        projectFile.Append( chunkLayer.ToBuffer() );
      }*/
      
      return projectFile;
    }



    public bool ReadFromBuffer( GR.Memory.ByteBuffer DataIn )
    {
      if ( DataIn == null )
      {
        return false;
      }
      SpriteLayers.Clear();
      Sprites.Clear();

      GR.IO.MemoryReader memIn = DataIn.MemoryReader();

      uint     Version = memIn.ReadUInt32();

      if ( Version == 2 )
      {
        Colors.Palettes.Clear();

        GR.IO.FileChunk   chunkMain = new GR.IO.FileChunk();

        while ( chunkMain.ReadFromStream( memIn ) )
        {
          switch ( chunkMain.Type )
          {
            case FileChunkConstants.SPRITESET_PROJECT:
              {
                var    chunkReader = chunkMain.MemoryReader();

                GR.IO.FileChunk   subChunk = new GR.IO.FileChunk();

                while ( subChunk.ReadFromStream( chunkReader ) )
                {
                  var    subChunkReader = subChunk.MemoryReader();

                  switch ( subChunk.Type )
                  {
                    case FileChunkConstants.SPRITESET_INFO:
                      TotalNumberOfSprites  = subChunkReader.ReadInt32();
                      Name                  = subChunkReader.ReadString();
                      ExportFilename        = subChunkReader.ReadString();
                      ExportStartIndex      = subChunkReader.ReadInt32();
                      ExportSpriteCount     = subChunkReader.ReadInt32();
                      break;
                    case FileChunkConstants.MULTICOLOR_DATA:
                      Mode = (SpriteProjectMode)subChunkReader.ReadInt32();
                      Colors.BackgroundColor = subChunkReader.ReadInt32();
                      Colors.MultiColor1 = subChunkReader.ReadInt32();
                      Colors.MultiColor2 = subChunkReader.ReadInt32();
                      Colors.ActivePalette = 0;
                      break;
                    case FileChunkConstants.PALETTE:
                      Colors.Palettes.Add( Palette.Read( subChunkReader ) );
                      break;
                    case FileChunkConstants.SPRITESET_SPRITE:
                      {
                        var sprite = new SpriteData( new ColorSettings( Colors ) );

                        sprite.Mode = (SpriteMode)subChunkReader.ReadInt32();
                        sprite.Tile.Mode = (GraphicTileMode)subChunkReader.ReadInt32();
                        sprite.Tile.CustomColor = (byte)subChunkReader.ReadInt32();
                        sprite.Tile.Width = subChunkReader.ReadInt32();
                        sprite.Tile.Height = subChunkReader.ReadInt32();
                        int dataLength = subChunkReader.ReadInt32();
                        sprite.Tile.Data = new GR.Memory.ByteBuffer();
                        subChunkReader.ReadBlock( sprite.Tile.Data, (uint)dataLength );
                        if ( sprite.Tile.CustomColor == 255 )
                        {
                          sprite.Tile.CustomColor = 1;
                        }

                        sprite.Tile.Colors.ActivePalette = subChunkReader.ReadInt32();
                        sprite.Tile.Colors.PaletteOffset = subChunkReader.ReadInt32();
                        sprite.Tile.Image = new GR.Image.MemoryImage( sprite.Tile.Width, sprite.Tile.Height, GR.Drawing.PixelFormat.Format32bppRgb );

                        // bugfix - mega65 sprites have a different mode
                        if ( sprite.Tile.Mode == GraphicTileMode.MEGA65_NCM_CHARACTERS )
                        {
                          sprite.Tile.Mode = GraphicTileMode.MEGA65_NCM_SPRITES;
                        }

                        Sprites.Add( sprite );
                      }
                      break;
                    case FileChunkConstants.SPRITESET_LAYER:
                      {
                        Layer  layer = new Layer();

                        SpriteLayers.Add( layer );

                        GR.IO.FileChunk   subChunkL = new GR.IO.FileChunk();

                        while ( subChunkL.ReadFromStream( subChunkReader ) )
                        {
                          var    subChunkReaderL = subChunkL.MemoryReader();

                          if ( subChunkL.Type == FileChunkConstants.SPRITESET_LAYER_ENTRY )
                          {
                            LayerSprite sprite = new LayerSprite();

                            sprite.Index = subChunkReaderL.ReadInt32();
                            sprite.Color = subChunkReaderL.ReadInt32();
                            sprite.X = subChunkReaderL.ReadInt32();
                            sprite.Y = subChunkReaderL.ReadInt32();
                            sprite.ExpandX = ( subChunkReaderL.ReadInt32() != 0 );
                            sprite.ExpandY = ( subChunkReaderL.ReadInt32() != 0 );

                            layer.Sprites.Add( sprite );
                          }
                          else if ( subChunkL.Type == FileChunkConstants.SPRITESET_LAYER_INFO )
                          {
                            layer.Name = subChunkReaderL.ReadString();
                            layer.BackgroundColor = subChunkReaderL.ReadInt32();
                            layer.DelayMS = subChunkReaderL.ReadInt32();
                          }
                        }
                      }
                      break;
                  }
                }
              }
              break;
            default:
              Debug.Log( "SpriteProject.ReadFromBuffer unexpected chunk type " + chunkMain.Type.ToString( "X" ) );
              return false;
          }
        }

        return true;
      }

      int       numSprites = 256;
      if ( Version >= 1 )
      {
        numSprites = memIn.ReadInt32();
      }
      Sprites = new List<SpriteData>();
      for ( int i = 0; i < numSprites; ++i )
      {
        Sprites.Add( new SpriteData( Colors ) );
        PaletteManager.ApplyPalette( Sprites[i].Tile.Image );
      }

      string name = memIn.ReadString();
      for ( int i = 0; i < numSprites; ++i )
      {
        Sprites[i].Tile.CustomColor = (byte)memIn.ReadInt32();
        if ( Sprites[i].Tile.CustomColor == 255 )
        {
          Sprites[i].Tile.CustomColor = 1;
        }
      }
      for ( int i = 0; i < numSprites; ++i )
      {
        Sprites[i].Mode = (SpriteMode)memIn.ReadUInt8();
        Sprites[i].Tile.Mode = Lookup.GraphicTileModeFromSpriteMode( Sprites[i].Mode );
      }
      Colors.BackgroundColor = memIn.ReadInt32();
      Colors.MultiColor1 = memIn.ReadInt32();
      Colors.MultiColor2 = memIn.ReadInt32();

      bool genericMultiColor = ( memIn.ReadUInt32() != 0 );
      for ( int i = 0; i < numSprites; ++i )
      {
        GR.Memory.ByteBuffer tempBuffer = new GR.Memory.ByteBuffer();

        memIn.ReadBlock( tempBuffer, 64 );
        tempBuffer.CopyTo( Sprites[i].Tile.Data, 0, 63 );
      }

      ExportSpriteCount = memIn.ReadInt32();

      ExportFilename = memIn.ReadString();
      string exportPathSpriteFile = memIn.ReadString();
      for ( int i = 0; i < numSprites; ++i )
      {
        string desc = memIn.ReadString();
      }
      int     spriteTestCount = memIn.ReadInt32();
      for ( int i = 0; i < spriteTestCount; ++i )
      {
        int spriteIndex = memIn.ReadInt32();
        byte spriteColor = memIn.ReadUInt8();
        bool spriteMultiColor = ( memIn.ReadUInt8() != 0 );
        int spriteX = memIn.ReadInt32();
        int spriteY = memIn.ReadInt32();
      }

      GR.IO.FileChunk   chunk = new GR.IO.FileChunk();

      while ( chunk.ReadFromStream( memIn ) )
      {
        switch ( chunk.Type )
        {
          case FileChunkConstants.SPRITESET_LAYER:
            {
              Layer  layer = new Layer();

              SpriteLayers.Add( layer );

              var    chunkReader = chunk.MemoryReader();

              GR.IO.FileChunk   subChunk = new GR.IO.FileChunk();

              while ( subChunk.ReadFromStream( chunkReader ) )
              {
                var    subChunkReader = subChunk.MemoryReader();

                if ( subChunk.Type == FileChunkConstants.SPRITESET_LAYER_ENTRY )
                {
                  LayerSprite sprite = new LayerSprite();

                  sprite.Index = subChunkReader.ReadInt32();
                  sprite.Color = subChunkReader.ReadUInt8();
                  sprite.X = subChunkReader.ReadInt32();
                  sprite.Y = subChunkReader.ReadInt32();
                  sprite.ExpandX = ( subChunkReader.ReadUInt8() != 0 );
                  sprite.ExpandY = ( subChunkReader.ReadUInt8() != 0 );

                  layer.Sprites.Add( sprite );
                }
                else if ( subChunk.Type == FileChunkConstants.SPRITESET_LAYER_INFO )
                {
                  layer.Name            = subChunkReader.ReadString();
                  layer.BackgroundColor = subChunkReader.ReadUInt8();
                  layer.DelayMS         = subChunkReader.ReadInt32();
                }
              }
            }
            break;
        }
      }

      while ( Sprites.Count > 256 )
      {
        Sprites.RemoveAt( 256 );
      }

      return true;
    }



    public ByteBuffer GetPaletteExportData( int StartIndex, int NumColors, bool Swizzled, bool SortedByColorTriplets )
    {
      // get all palette datas, first all R, then all G, then all B
      var palData = new ByteBuffer();
      for ( int i = 0; i < Colors.Palettes.Count; ++i )
      {
        var curPal = Colors.Palettes[i];

        //Debug.Log( "orig pal data: " + curPal.GetExportData( 0, curPal.NumColors, false ).ToString() );

        palData.Append( curPal.GetExportData( 0, curPal.NumColors, Swizzled, SortedByColorTriplets ) );
      }

      //Debug.Log( "Total Pal Data: " + palData.ToString() );

      // pal data has rgbrgbrgb, we need to copy all r,g,bs behind each other

      var orderedPalData = new ByteBuffer();

      for ( int i = 0; i < 3; ++i )
      {
        for ( int j = 0; j < Colors.Palettes.Count; ++j )
        {
          orderedPalData.Append( palData.SubBuffer( ( i + j * 3 ) * Colors.Palettes[0].NumColors, Colors.Palettes[0].NumColors ) );
        }
      }

      var finalPalData = new ByteBuffer();
      int totalNumColors = Colors.Palettes.Count * Colors.Palettes[0].NumColors;

      // extract R, G and B
      finalPalData.Append( orderedPalData.SubBuffer( StartIndex, NumColors ) );
      finalPalData.Append( orderedPalData.SubBuffer( totalNumColors + StartIndex, NumColors ) );
      finalPalData.Append( orderedPalData.SubBuffer( 2 * totalNumColors + StartIndex, NumColors ) );

      //Debug.Log( "GetPaletteExportData: " + finalPalData.ToString() );
      return finalPalData;
    }



  }
}
