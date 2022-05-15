using RetroDevStudio;
using System;
using System.Collections.Generic;
using System.Text;



namespace RetroDevStudio.Formats
{
  public class MapProject
  {
    public class TileChar
    {
      public byte       Character = 0;
      public byte       Color = 1;
    };

    public class Tile
    {
      public GR.Game.Layer<TileChar> Chars = new GR.Game.Layer<TileChar>();
      public string       Name = "";
      public int          Index = 0;

      public Tile()
      {
        Chars.InvalidTile = new TileChar();
      }
    };

    public class Map
    {
      public GR.Game.Layer<int> Tiles = new GR.Game.Layer<int>();
      public string             Name = "";
      public int                TileSpacingX = 2;
      public int                TileSpacingY = 2;
      public GR.Memory.ByteBuffer   ExtraDataOld = new GR.Memory.ByteBuffer();
      public string             ExtraDataText = "";
      public int                AlternativeMultiColor1 = -1;
      public int                AlternativeMultiColor2 = -1;
      public int                AlternativeBackgroundColor = -1;
      public int                AlternativeBGColor4 = -1;

      /// <summary>
      /// overrides Project.Mode when set (e.g. display MC instead of hires)
      /// </summary>
      public TextCharMode       AlternativeMode = TextCharMode.UNKNOWN;
    };


    public List<Tile>                   Tiles = new List<Tile>();

    public List<Map>                    Maps = new List<Map>();

    public string                       ExternalCharset = "";
    public int                          BackgroundColor = 0;
    public int                          MultiColor1 = 0;
    public int                          MultiColor2 = 0;
    public int                          BGColor4 = 0;

    /// <summary>
    /// This mode is used to display/build the tiles
    /// </summary>
    public TextMode                     Mode = TextMode.COMMODORE_40_X_25_HIRES;
    public CharsetProject               Charset = new Formats.CharsetProject();
    public bool                         ShowGrid = false;



    public MapProject()
    {
      for ( int i = 0; i < 256; ++i )
      {
        for ( int j = 0; j < 8; ++j )
        {
          Charset.Characters[i].Tile.CustomColor = 1;
          Charset.Characters[i].Tile.Data.SetU8At( j, ConstantData.UpperCaseCharsetC64.ByteAt( i * 8 + j ) );
        }
      }
    }



    public void Clear()
    {
      Tiles.Clear();
      Maps.Clear();
      ExternalCharset = "";
    }



    public GR.Memory.ByteBuffer SaveToBuffer()
    {
      GR.Memory.ByteBuffer projectFile = new GR.Memory.ByteBuffer();

      GR.IO.FileChunk chunkProjectInfo = new GR.IO.FileChunk( FileChunkConstants.MAP_PROJECT_INFO );
      // version
      chunkProjectInfo.AppendU32( 0 );
      chunkProjectInfo.AppendString( ExternalCharset );
      chunkProjectInfo.AppendI32( ShowGrid ? 1 : 0 );
      projectFile.Append( chunkProjectInfo.ToBuffer() );

      GR.IO.FileChunk chunkCharset = new GR.IO.FileChunk( FileChunkConstants.MAP_CHARSET );
      chunkCharset.Append( Charset.SaveToBuffer() );
      projectFile.Append( chunkCharset.ToBuffer() );

      GR.IO.FileChunk chunkProjectData = new GR.IO.FileChunk( FileChunkConstants.MAP_PROJECT_DATA );

      GR.IO.FileChunk chunkMCData = new GR.IO.FileChunk( FileChunkConstants.MULTICOLOR_DATA );
      chunkMCData.AppendU8( (byte)Mode );
      chunkMCData.AppendU8( (byte)BackgroundColor );
      chunkMCData.AppendU8( (byte)MultiColor1 );
      chunkMCData.AppendU8( (byte)MultiColor2 );
      chunkMCData.AppendU8( (byte)BGColor4 );
      chunkProjectData.Append( chunkMCData.ToBuffer() );

      foreach ( Tile tile in Tiles )
      {
        GR.IO.FileChunk chunkTile = new GR.IO.FileChunk( FileChunkConstants.MAP_TILE );

        chunkTile.AppendString( tile.Name );
        chunkTile.AppendI32( tile.Chars.Width );
        chunkTile.AppendI32( tile.Chars.Height );
        for ( int j = 0; j < tile.Chars.Height; ++j )
        {
          for ( int i = 0; i < tile.Chars.Width; ++i )
          {
            TileChar    tChar = tile.Chars[i, j];
            chunkTile.AppendU8( tChar.Character );
            chunkTile.AppendU8( tChar.Color );
          }
        }
        chunkProjectData.Append( chunkTile.ToBuffer() );
      }
      foreach ( Map map in Maps )
      {
        GR.IO.FileChunk chunkMap = new GR.IO.FileChunk( FileChunkConstants.MAP );

        GR.IO.FileChunk chunkMapInfo = new GR.IO.FileChunk( FileChunkConstants.MAP_INFO );

        chunkMapInfo.AppendString( map.Name );
        chunkMapInfo.AppendI32( map.TileSpacingX );
        chunkMapInfo.AppendI32( map.TileSpacingY );
        chunkMapInfo.AppendI32( map.AlternativeMultiColor1 + 1 );
        chunkMapInfo.AppendI32( map.AlternativeMultiColor2 + 1 );
        chunkMapInfo.AppendI32( map.AlternativeBackgroundColor + 1 );
        chunkMapInfo.AppendI32( map.AlternativeBGColor4 + 1 );
        chunkMapInfo.AppendI32( (int)map.AlternativeMode + 1 );
        chunkMap.Append( chunkMapInfo.ToBuffer() );

        GR.IO.FileChunk chunkMapData = new GR.IO.FileChunk( FileChunkConstants.MAP_DATA );
        chunkMapData.AppendI32( map.Tiles.Width );
        chunkMapData.AppendI32( map.Tiles.Height );
        for ( int j = 0; j < map.Tiles.Height; ++j )
        {
          for ( int i = 0; i < map.Tiles.Width; ++i )
          {
            chunkMapData.AppendI32( map.Tiles[i, j] );
          }
        }
        chunkMap.Append( chunkMapData.ToBuffer() );

        if ( map.ExtraDataText.Length > 0 )
        {
          GR.IO.FileChunk chunkMapExtraData = new GR.IO.FileChunk( FileChunkConstants.MAP_EXTRA_DATA_TEXT );

          chunkMapExtraData.AppendString( map.ExtraDataText );

          chunkMap.Append( chunkMapExtraData.ToBuffer() );
        }
        if ( map.ExtraDataOld.Length > 0 )
        {
          GR.IO.FileChunk chunkMapExtraData = new GR.IO.FileChunk( FileChunkConstants.MAP_EXTRA_DATA );

          chunkMapExtraData.AppendU32( map.ExtraDataOld.Length );
          chunkMapExtraData.Append( map.ExtraDataOld );

          chunkMap.Append( chunkMapExtraData.ToBuffer() );
        }
        chunkProjectData.Append( chunkMap.ToBuffer() );
      }

      projectFile.Append( chunkProjectData.ToBuffer() );
      return projectFile;
    }



    public bool ReadFromBuffer( GR.Memory.ByteBuffer ProjectFile )
    {
      if ( ProjectFile == null )
      {
        return false;
      }

      GR.IO.MemoryReader    memReader = new GR.IO.MemoryReader( ProjectFile );

      GR.IO.FileChunk chunk = new GR.IO.FileChunk();

      string importedCharSet = "";
      while ( chunk.ReadFromStream( memReader ) )
      {
        GR.IO.MemoryReader chunkReader = chunk.MemoryReader();
        switch ( chunk.Type )
        {
          case FileChunkConstants.MAP_PROJECT_INFO:
            {
              uint version  = chunkReader.ReadUInt32();
              importedCharSet = chunkReader.ReadString();

              ShowGrid = ( chunkReader.ReadInt32() == 1 );
            }
            break;
          case FileChunkConstants.MAP_CHARSET:
            {
              GR.Memory.ByteBuffer    data = new GR.Memory.ByteBuffer();
              chunkReader.ReadBlock( data, (uint)( chunkReader.Size - chunkReader.Position ) );

              Charset.ReadFromBuffer( data );
            }
            break;
          case FileChunkConstants.MAP_PROJECT_DATA:
            {
              GR.IO.FileChunk chunkData = new GR.IO.FileChunk();

              while ( chunkData.ReadFromStream( chunkReader ) )
              {
                GR.IO.MemoryReader subChunkReader = chunkData.MemoryReader();
                switch ( chunkData.Type )
                {
                  case FileChunkConstants.MULTICOLOR_DATA:
                    Mode = (TextMode)subChunkReader.ReadUInt8();
                    BackgroundColor = subChunkReader.ReadUInt8();
                    MultiColor1 = subChunkReader.ReadUInt8();
                    MultiColor2 = subChunkReader.ReadUInt8();
                    BGColor4 = subChunkReader.ReadUInt8();
                    break;
                  case FileChunkConstants.MAP_TILE:
                    {
                      Tile tile = new Tile();
                      tile.Name = subChunkReader.ReadString();

                      int w = subChunkReader.ReadInt32();
                      int h = subChunkReader.ReadInt32();

                      tile.Chars.Resize( w, h );
                      for ( int j = 0; j < tile.Chars.Height; ++j )
                      {
                        for ( int i = 0; i < tile.Chars.Width; ++i )
                        {
                          tile.Chars[i, j].Character = subChunkReader.ReadUInt8();
                          tile.Chars[i, j].Color = subChunkReader.ReadUInt8();
                        }
                      }
                      Tiles.Add( tile );
                      tile.Index = Tiles.Count - 1;
                    }
                    break;
                  case FileChunkConstants.MAP:
                    {
                      GR.IO.FileChunk mapChunk = new GR.IO.FileChunk();

                      Map map = new Map();

                      while ( mapChunk.ReadFromStream( subChunkReader ) )
                      {
                        GR.IO.MemoryReader mapChunkReader = mapChunk.MemoryReader();
                        switch ( mapChunk.Type )
                        {
                          case FileChunkConstants.MAP_INFO:
                            map.Name = mapChunkReader.ReadString();
                            map.TileSpacingX = mapChunkReader.ReadInt32();
                            map.TileSpacingY = mapChunkReader.ReadInt32();
                            map.AlternativeMultiColor1 = mapChunkReader.ReadInt32() - 1;
                            map.AlternativeMultiColor2 = mapChunkReader.ReadInt32() - 1;
                            map.AlternativeBackgroundColor = mapChunkReader.ReadInt32() - 1;
                            map.AlternativeBGColor4 = mapChunkReader.ReadInt32() - 1;
                            map.AlternativeMode = (TextCharMode)( mapChunkReader.ReadInt32() - 1 );
                            break;
                          case FileChunkConstants.MAP_DATA:
                            {
                              int w = mapChunkReader.ReadInt32();
                              int h = mapChunkReader.ReadInt32();

                              map.Tiles.Resize( w, h );
                              for ( int j = 0; j < map.Tiles.Height; ++j )
                              {
                                for ( int i = 0; i < map.Tiles.Width; ++i )
                                {
                                  map.Tiles[i, j] = mapChunkReader.ReadInt32();
                                }
                              }
                            }
                            break;
                          case FileChunkConstants.MAP_EXTRA_DATA:
                            {
                              uint len = mapChunkReader.ReadUInt32();

                              mapChunkReader.ReadBlock( map.ExtraDataOld, len );

                              map.ExtraDataText = map.ExtraDataOld.ToString();
                              map.ExtraDataOld.Clear();
                            }
                            break;
                          case FileChunkConstants.MAP_EXTRA_DATA_TEXT:
                            {
                              map.ExtraDataText = mapChunkReader.ReadString();
                            }
                            break;
                        }
                      }

                      Maps.Add( map );
                    }
                    break;
                }
              }
            }
            break;
        }
      }
      memReader.Close();


      Charset.Colors.MultiColor1 = MultiColor1;
      Charset.Colors.MultiColor2 = MultiColor2;
      Charset.Colors.BGColor4    = BGColor4;
      return true;
    }



    public GR.Memory.ByteBuffer ExportAsTiles()
    {
      GR.Memory.ByteBuffer    tileData = new GR.Memory.ByteBuffer();

      // find max tile size
      int     tileW = 1;
      int     tileH = 1;

      foreach ( var tile in Tiles )
      {
        if ( tile.Chars.Width > tileW )
        {
          tileW = tile.Chars.Width;
        }
        if ( tile.Chars.Height > tileH )
        {
          tileH = tile.Chars.Height;
        }
      }

      for ( int j = 0; j < tileH; ++j )
      {
        for ( int i = 0; i < tileW; ++i )
        {
          foreach ( Formats.MapProject.Tile tile in Tiles )
          {
            if ( ( i < tile.Chars.Width )
            &&   ( j < tile.Chars.Height ) )
            {
              tileData.AppendU8( (byte)tile.Chars[i,j].Character );
            }
            else
            {
              tileData.AppendU8( 0 );
            }
            if ( ( i < tile.Chars.Width )
            &&   ( j < tile.Chars.Height ) )
            {
              tileData.AppendU8( (byte)tile.Chars[i, j].Color );
            }
            else
            {
              tileData.AppendU8( 0 );
            }
          }
        }
      }
      return tileData;
    }



    public GR.Memory.ByteBuffer ExportMapsAsBuffer( bool RowByRow )
    {
      GR.Memory.ByteBuffer    mapData = new GR.Memory.ByteBuffer();


      foreach ( var map in Maps )
      {
        mapData.Append( ExportMapAsBuffer( map, RowByRow ) );
      }
      return mapData;
    }



    public string ExportMapsAsAssembly( string Prefix, bool RowByRow )
    {
      StringBuilder   sb = new StringBuilder();

      foreach ( var map in Maps )
      {
        GR.Memory.ByteBuffer      mapData = ExportMapAsBuffer( map, RowByRow );

        sb.Append( Prefix );
        sb.AppendLine( map.Name );

      }
      return sb.ToString();
    }



    public GR.Memory.ByteBuffer ExportMapAsBuffer( Map Map, bool RowByRow )
    {
      GR.Memory.ByteBuffer mapDataBuffer = new GR.Memory.ByteBuffer( (uint)( Map.Tiles.Width * Map.Tiles.Height ) );

      if ( RowByRow )
      {
        for ( int y = 0; y < Map.Tiles.Height; ++y )
        {
          for ( int x = 0; x < Map.Tiles.Width; ++x )
          {
            mapDataBuffer.SetU8At( x + y * Map.Tiles.Width, (byte)Map.Tiles[x, y] );
          }
        }
      }
      else
      {
        for ( int x = 0; x < Map.Tiles.Width; ++x )
        {
          for ( int y = 0; y < Map.Tiles.Height; ++y )          
          {
            mapDataBuffer.SetU8At( x + y * Map.Tiles.Width, (byte)Map.Tiles[x, y] );
          }
        }
      }
      return mapDataBuffer;
    }



    public bool ExportTilesAsElements( out string TileData, string LabelPrefix, bool WrapData, int WrapByteCount, string DataByteDirective )
    {
      GR.Memory.ByteBuffer tileDataW = new GR.Memory.ByteBuffer();
      GR.Memory.ByteBuffer tileDataH = new GR.Memory.ByteBuffer();

      StringBuilder sbTileCharLo = new StringBuilder();
      StringBuilder sbTileCharHi = new StringBuilder();
      StringBuilder sbTileColorLo = new StringBuilder();
      StringBuilder sbTileColorHi = new StringBuilder();
      StringBuilder sbTileChars = new StringBuilder();
      StringBuilder sbTileColors = new StringBuilder();

      GR.Memory.ByteBuffer tileDataChars = new GR.Memory.ByteBuffer();
      GR.Memory.ByteBuffer tileDataHi = new GR.Memory.ByteBuffer();

      foreach ( Formats.MapProject.Tile tile in Tiles )
      {
        tileDataW.AppendU8( (byte)tile.Chars.Width );
        tileDataH.AppendU8( (byte)tile.Chars.Height );

        sbTileCharLo.Append( DataByteDirective );
        sbTileCharLo.Append( " <" + LabelPrefix + "TILE_CHAR_" + NormalizeAsLabel( tile.Name.ToUpper() ) + Environment.NewLine );
        sbTileCharHi.Append( DataByteDirective );
        sbTileCharHi.Append( " >" + LabelPrefix + "TILE_CHAR_" + NormalizeAsLabel( tile.Name.ToUpper() ) + Environment.NewLine );

        sbTileColorLo.Append( DataByteDirective );
        sbTileColorLo.Append( " <" + LabelPrefix + "TILE_COLOR_" + NormalizeAsLabel( tile.Name.ToUpper() ) + Environment.NewLine );
        sbTileColorHi.Append( DataByteDirective );
        sbTileColorHi.Append( " >" + LabelPrefix + "TILE_COLOR_" + NormalizeAsLabel( tile.Name.ToUpper() ) + Environment.NewLine );

        sbTileChars.Append( LabelPrefix + "TILE_CHAR_" + NormalizeAsLabel( tile.Name.ToUpper() ) + Environment.NewLine );

        var tileCharData = new GR.Memory.ByteBuffer();
        for ( int j = 0; j < tile.Chars.Height; ++j )
        {
          for ( int i = 0; i < tile.Chars.Width; ++i )
          {
            tileCharData.AppendU8( tile.Chars[i, j].Character );
          }
        }
        sbTileChars.AppendLine( Util.ToASMData( tileCharData, WrapData, WrapByteCount, DataByteDirective ) );


        sbTileColors.Append( LabelPrefix + "TILE_COLOR_" + NormalizeAsLabel( tile.Name.ToUpper() ) + Environment.NewLine );

        var tileColorData = new GR.Memory.ByteBuffer();
        for ( int j = 0; j < tile.Chars.Height; ++j )
        {
          for ( int i = 0; i < tile.Chars.Width; ++i )
          {
            tileColorData.AppendU8( tile.Chars[i, j].Color );
          }
        }
        sbTileColors.AppendLine( Util.ToASMData( tileColorData, WrapData, WrapByteCount, DataByteDirective ) );

      }
      TileData = LabelPrefix + "NUM_TILES = " + Tiles.Count + System.Environment.NewLine
                + LabelPrefix + "TILE_WIDTH" + System.Environment.NewLine + Util.ToASMData( tileDataW, WrapData, WrapByteCount, DataByteDirective ) + System.Environment.NewLine
                + LabelPrefix + "TILE_HEIGHT" + System.Environment.NewLine + Util.ToASMData( tileDataH, WrapData, WrapByteCount, DataByteDirective ) + System.Environment.NewLine
                + LabelPrefix + "TILE_CHARS_LO" + System.Environment.NewLine
                + sbTileCharLo.ToString() + System.Environment.NewLine
                + LabelPrefix + "TILE_CHARS_HI" + System.Environment.NewLine
                + sbTileCharHi.ToString() + System.Environment.NewLine
                + LabelPrefix + "TILE_COLORS_LO" + System.Environment.NewLine
                + sbTileColorLo.ToString() + System.Environment.NewLine
                + LabelPrefix + "TILE_COLORS_HI" + System.Environment.NewLine
                + sbTileColorHi.ToString() + System.Environment.NewLine
                + sbTileChars.ToString() + System.Environment.NewLine
                + sbTileColors.ToString() + System.Environment.NewLine;
      return true;
    }



    public bool ExportTilesAsAssembly( out string TileData, string LabelPrefix, bool WrapData, int WrapByteCount, string DataByteDirective )
    {
      int   maxTileWidth = 0;
      int   maxTileHeight = 0;
      foreach ( var tile in Tiles )
      {
        if ( tile.Chars.Width > maxTileWidth )
        {
          maxTileWidth = tile.Chars.Width;
        }
        if ( tile.Chars.Height > maxTileHeight )
        {
          maxTileHeight = tile.Chars.Height;
        }
      }

      GR.Memory.ByteBuffer[]  tileCharData = new GR.Memory.ByteBuffer[maxTileWidth * maxTileHeight];
      GR.Memory.ByteBuffer[]  tileColorData = new GR.Memory.ByteBuffer[maxTileWidth * maxTileHeight];
      for ( int j = 0; j < maxTileHeight; ++j )
      {
        for ( int i = 0; i < maxTileWidth; ++i )
        {
          tileCharData[i + j * maxTileWidth] = new GR.Memory.ByteBuffer( (uint)Tiles.Count );
          tileColorData[i + j * maxTileWidth] = new GR.Memory.ByteBuffer( (uint)Tiles.Count );
        }
      }


      int tileIndex = 0;
      
      for ( int j = 0; j < maxTileHeight; ++j )
      {
        for ( int i = 0; i < maxTileWidth; ++i )
        {
          tileIndex = 0;
          foreach ( var tile in Tiles )
          {
            if ( ( i < tile.Chars.Width )
            &&   ( j < tile.Chars.Height ) )
            {
              tileCharData[i + j * maxTileWidth].SetU8At( tileIndex, tile.Chars[i, j].Character );
              tileColorData[i + j * maxTileWidth].SetU8At( tileIndex, tile.Chars[i, j].Color );
            }
            ++tileIndex;
          }
        }
      }

      StringBuilder sb = new StringBuilder();

      sb.AppendLine( LabelPrefix + "NUM_TILES = " + Tiles.Count );

      for ( int j = 0; j < maxTileHeight; ++j )
      {
        for ( int i = 0; i < maxTileWidth; ++i )
        {
          sb.Append( LabelPrefix + "TILE_CHARS_" );
          sb.Append( i );
          sb.Append( "_" );
          sb.Append( j );
          sb.AppendLine();

          sb.AppendLine( Util.ToASMData( tileCharData[i + j * maxTileWidth], WrapData, WrapByteCount, DataByteDirective ) );
          sb.AppendLine();
        }
      }
      for ( int j = 0; j < maxTileHeight; ++j )
      {
        for ( int i = 0; i < maxTileWidth; ++i )
        {
          sb.Append( LabelPrefix + "TILE_COLORS_" );
          sb.Append( i );
          sb.Append( "_" );
          sb.Append( j );
          sb.AppendLine();

          sb.AppendLine( Util.ToASMData( tileColorData[i + j * maxTileWidth], WrapData, WrapByteCount, DataByteDirective ) );
          sb.AppendLine();
        }
      }
      TileData = sb.ToString();
      return true;
    }



    public bool ExportTileDataAsAssembly( out string TileData, string LabelPrefix, bool WrapData, int WrapByteCount, string DataByteDirective )
    {
      var sbTileChars = new StringBuilder();
      var sbTileColors = new StringBuilder();

      int tileIndex = 0;
      foreach ( var tile in Tiles )
      {
        sbTileChars.Append( LabelPrefix );
        sbTileChars.Append( '_' );
        sbTileChars.Append( tileIndex );
        sbTileChars.AppendLine( "_CHARS" );
        sbTileChars.Append( DataByteDirective );
        sbTileChars.Append( ' ' );

        sbTileColors.Append( LabelPrefix );
        sbTileColors.Append( '_' );
        sbTileColors.Append( tileIndex );
        sbTileColors.AppendLine( "_COLORS" );
        sbTileColors.Append( DataByteDirective );
        sbTileColors.Append( ' ' );
        for ( int j = 0; j < tile.Chars.Height; ++j )
        {
          for ( int i = 0; i < tile.Chars.Width; ++i )
          {
            sbTileChars.Append( "$" );
            sbTileChars.Append( tile.Chars[i, j].Character.ToString( "X2" ) );
            sbTileColors.Append( "$" );
            sbTileColors.Append( tile.Chars[i, j].Color.ToString( "X2" ) );

            if ( ( i + 1 < tile.Chars.Width )
            ||   ( j + 1 < tile.Chars.Height ) )
            {
              sbTileChars.Append( ',' );
              sbTileColors.Append( ',' );
            }
          }
        }
        sbTileChars.AppendLine();
        sbTileColors.AppendLine();
        ++tileIndex;
      }

      TileData = sbTileChars.ToString() + sbTileColors.ToString();
      return true;
    }



    public bool ExportMapsAsAssembly( bool Vertical, out string MapData, string LabelPrefix, bool WrapData, int WrapByteCount, string DataByteDirective )
    {
      bool hasExtraData = false;
      foreach ( var map in Maps )
      {
        if ( map.ExtraDataText.Length > 0 )
        {
          hasExtraData = true;
          break;
        }
      }

      StringBuilder sbMaps = new StringBuilder();

      sbMaps.Append( LabelPrefix );
      sbMaps.Append( "NUM_MAPS = " );
      sbMaps.AppendLine( Maps.Count.ToString() );

      sbMaps.Append( LabelPrefix );
      sbMaps.AppendLine( "MAP_LIST_LO" );
      for ( int i = 0; i < Maps.Count; ++i )
      {
        sbMaps.Append( DataByteDirective );
        sbMaps.Append( ' ' );
        sbMaps.AppendLine( "<" + LabelPrefix + "MAP_" + NormalizeAsLabel( Maps[i].Name.ToUpper() ) );
      }
      sbMaps.AppendLine();
      sbMaps.Append( LabelPrefix );
      sbMaps.AppendLine( "MAP_LIST_HI" );
      for ( int i = 0; i < Maps.Count; ++i )
      {
        sbMaps.Append( DataByteDirective );
        sbMaps.Append( ' ' );
        sbMaps.AppendLine( ">" + LabelPrefix + "MAP_" + NormalizeAsLabel( Maps[i].Name.ToUpper() ) );
      }
      sbMaps.AppendLine();

      if ( hasExtraData )
      {
        sbMaps.Append( LabelPrefix );
        sbMaps.AppendLine( "MAP_EXTRA_DATA_LIST_LO" );
        for ( int i = 0; i < Maps.Count; ++i )
        {
          sbMaps.Append( DataByteDirective );
          sbMaps.Append( ' ' );
          sbMaps.AppendLine( "<" + LabelPrefix + "MAP_EXTRA_DATA_" + NormalizeAsLabel( Maps[i].Name.ToUpper() ) );
        }
        sbMaps.Append( LabelPrefix );
        sbMaps.AppendLine( "MAP_EXTRA_DATA_LIST_HI" );
        for ( int i = 0; i < Maps.Count; ++i )
        {
          sbMaps.Append( DataByteDirective );
          sbMaps.Append( ' ' );
          sbMaps.AppendLine( ">" + LabelPrefix + "MAP_EXTRA_DATA_" + NormalizeAsLabel( Maps[i].Name.ToUpper() ) );
        }
        sbMaps.AppendLine();
      }


      for ( int i = 0; i < Maps.Count; ++i )
      {
        var map = Maps[i];

        sbMaps.AppendLine();
        sbMaps.Append( LabelPrefix );
        sbMaps.AppendLine( "MAP_" + NormalizeAsLabel( map.Name.ToUpper() ) );

        GR.Memory.ByteBuffer mapDataBuffer = new GR.Memory.ByteBuffer( (uint)( map.Tiles.Width * map.Tiles.Height ) );

        if ( Vertical )
        {
          for ( int y = 0; y < map.Tiles.Height; ++y )
          {
            for ( int x = 0; x < map.Tiles.Width; ++x )
            {
              mapDataBuffer.SetU8At( x * map.Tiles.Height + y, (byte)map.Tiles[x, y] );
            }
          }
        }
        else
        {
          for ( int y = 0; y < map.Tiles.Height; ++y )
          {
            for ( int x = 0; x < map.Tiles.Width; ++x )
            {
              mapDataBuffer.SetU8At( x + y * map.Tiles.Width, (byte)map.Tiles[x, y] );
            }
          }
        }
        sbMaps.AppendLine();
        sbMaps.Append( Util.ToASMData( mapDataBuffer, WrapData, WrapByteCount, DataByteDirective ) );
        if ( ( hasExtraData )
        &&   ( map.ExtraDataText.Length > 0 ) )
        {
          sbMaps.AppendLine( ";extra data" );
          sbMaps.Append( LabelPrefix );
          sbMaps.AppendLine( "MAP_EXTRA_DATA_" + NormalizeAsLabel( map.Name.ToUpper() ) );

          // clean extra data
          GR.Memory.ByteBuffer    extraData = new GR.Memory.ByteBuffer();
          string[]  lines = map.ExtraDataText.Split( new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries );
          foreach ( string line in lines )
          {
            string    tempLine = line.Trim().Replace( " ", "" );
            if ( ( !tempLine.StartsWith( ";" ) )
            &&   ( !tempLine.StartsWith( "#" ) )
            &&   ( !tempLine.StartsWith( "//" ) ) )
            {
              extraData.AppendHex( tempLine );
            }
          }

          sbMaps.Append( Util.ToASMData( extraData, WrapData, WrapByteCount, DataByteDirective ) );
          sbMaps.AppendLine();
        }
      }

      MapData = sbMaps.ToString();
      return true;
    }



    private string NormalizeAsLabel( string Label )
    {
      StringBuilder   sb = new StringBuilder();

      foreach ( var c in Label )
      {
        if ( ( !char.IsDigit( c ) )
        &&   ( !char.IsLetter( c ) )
        &&   ( c != '_' ) )
        {
          sb.Append( '_' );
        }
        else
        {
          sb.Append( c );
        }
      }
      return sb.ToString();
    }



    public bool ExportMapExtraDataAsAssembly( out string MapData, string LabelPrefix, bool WrapData, int WrapByteCount, string DataByteDirective )
    {
      bool hasExtraData = false;
      foreach ( var map in Maps )
      {
        if ( map.ExtraDataText.Length > 0 )
        {
          hasExtraData = true;
          break;
        }
      }

      StringBuilder sbMaps = new StringBuilder();

      sbMaps.Append( LabelPrefix );
      sbMaps.Append( "NUM_MAPS = " );
      sbMaps.AppendLine( Maps.Count.ToString() );

      if ( hasExtraData )
      {
        sbMaps.Append( LabelPrefix );
        sbMaps.AppendLine( "MAP_EXTRA_DATA_LIST_LO" );
        for ( int i = 0; i < Maps.Count; ++i )
        {
          sbMaps.Append( DataByteDirective );
          sbMaps.Append( ' ' );
          sbMaps.AppendLine( "<" + LabelPrefix + "MAP_EXTRA_DATA_" + NormalizeAsLabel( Maps[i].Name.ToUpper() ) );
        }
        sbMaps.Append( LabelPrefix );
        sbMaps.AppendLine( "MAP_EXTRA_DATA_LIST_HI" );
        for ( int i = 0; i < Maps.Count; ++i )
        {
          sbMaps.Append( DataByteDirective );
          sbMaps.Append( ' ' );
          sbMaps.AppendLine( ">" + LabelPrefix + "MAP_EXTRA_DATA_" + NormalizeAsLabel( Maps[i].Name.ToUpper() ) );
        }
        sbMaps.AppendLine();
      }


      for ( int i = 0; i < Maps.Count; ++i )
      {
        var map = Maps[i];

        if ( ( hasExtraData )
        &&   ( map.ExtraDataText.Length > 0 ) )
        {
          sbMaps.AppendLine( ";extra data" );
          sbMaps.Append( LabelPrefix );
          sbMaps.AppendLine( "MAP_EXTRA_DATA_" + NormalizeAsLabel( map.Name.ToUpper() ) );

          // clean extra data
          GR.Memory.ByteBuffer    extraData = new GR.Memory.ByteBuffer();
          string[]  lines = map.ExtraDataText.Split( new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries );
          foreach ( string line in lines )
          {
            string    tempLine = line.Trim().Replace( " ", "" );
            if ( ( !tempLine.StartsWith( ";" ) )
            &&   ( !tempLine.StartsWith( "#" ) )
            &&   ( !tempLine.StartsWith( "//" ) ) )
            {
              extraData.AppendHex( tempLine );
            }
          }

          sbMaps.Append( Util.ToASMData( extraData, WrapData, WrapByteCount, DataByteDirective ) );
          sbMaps.AppendLine();
        }
      }

      MapData = sbMaps.ToString();
      return true;
    }



    internal void ExportTilesAsBuffer( bool RowByRow, out GR.Memory.ByteBuffer TileData )
    {
      TileData = new GR.Memory.ByteBuffer();

      foreach ( Formats.MapProject.Tile tile in Tiles )
      {
        if ( RowByRow )
        {
          for ( int j = 0; j < tile.Chars.Height; ++j )
          {
            for ( int i = 0; i < tile.Chars.Width; ++i )
            {
              TileData.AppendU8( (byte)tile.Chars[i, j].Character );
              TileData.AppendU8( (byte)tile.Chars[i, j].Color );
            }
          }
        }
        else
        {
          for ( int i = 0; i < tile.Chars.Width; ++i )
          {
            for ( int j = 0; j < tile.Chars.Height; ++j )            
            {
              TileData.AppendU8( (byte)tile.Chars[i, j].Character );
              TileData.AppendU8( (byte)tile.Chars[i, j].Color );
            }
          }
        }
      }
    }

  }
}
