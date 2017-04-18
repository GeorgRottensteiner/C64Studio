using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio.Formats
{
  public class MapProject
  {
    public class TileChar
    {
      public byte      Character = 0;
      public byte      Color = 1;
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
      public Types.CharsetMode  AlternativeMode = C64Studio.Types.CharsetMode.INVALID;
    };


    public List<Tile>                  Tiles = new List<Tile>();

    public List<Map>                   Maps = new List<Map>();

    public string                      ExternalCharset = "";
    public int                         BackgroundColor = 0;
    public int                         MultiColor1 = 0;
    public int                         MultiColor2 = 0;
    public int                         BGColor4 = 0;
    public Types.CharsetMode           Mode = C64Studio.Types.CharsetMode.HIRES;
    public CharsetProject              Charset = new C64Studio.Formats.CharsetProject();



    public MapProject()
    {
      for ( int i = 0; i < 256; ++i )
      {
        for ( int j = 0; j < 8; ++j )
        {
          Charset.Characters[i].Color = 1;
          Charset.Characters[i].Data.SetU8At( j, Types.ConstantData.UpperCaseCharset.ByteAt( i * 8 + j ) );
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

      GR.IO.FileChunk chunkProjectInfo = new GR.IO.FileChunk( Types.FileChunk.MAP_PROJECT_INFO );
      // version
      chunkProjectInfo.AppendU32( 0 );
      chunkProjectInfo.AppendString( ExternalCharset );
      projectFile.Append( chunkProjectInfo.ToBuffer() );

      GR.IO.FileChunk chunkCharset = new GR.IO.FileChunk( Types.FileChunk.MAP_CHARSET );
      chunkCharset.Append( Charset.SaveToBuffer() );
      projectFile.Append( chunkCharset.ToBuffer() );

      GR.IO.FileChunk chunkProjectData = new GR.IO.FileChunk( Types.FileChunk.MAP_PROJECT_DATA );

      GR.IO.FileChunk chunkMCData = new GR.IO.FileChunk( Types.FileChunk.MULTICOLOR_DATA );
      chunkMCData.AppendU8( (byte)Mode );
      chunkMCData.AppendU8( (byte)BackgroundColor );
      chunkMCData.AppendU8( (byte)MultiColor1 );
      chunkMCData.AppendU8( (byte)MultiColor2 );
      chunkMCData.AppendU8( (byte)BGColor4 );
      chunkProjectData.Append( chunkMCData.ToBuffer() );

      foreach ( Tile tile in Tiles )
      {
        GR.IO.FileChunk chunkTile = new GR.IO.FileChunk( Types.FileChunk.MAP_TILE );

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
        GR.IO.FileChunk chunkMap = new GR.IO.FileChunk( Types.FileChunk.MAP );

        GR.IO.FileChunk chunkMapInfo = new GR.IO.FileChunk( Types.FileChunk.MAP_INFO );

        chunkMapInfo.AppendString( map.Name );
        chunkMapInfo.AppendI32( map.TileSpacingX );
        chunkMapInfo.AppendI32( map.TileSpacingY );
        chunkMapInfo.AppendI32( map.AlternativeMultiColor1 + 1 );
        chunkMapInfo.AppendI32( map.AlternativeMultiColor2 + 1 );
        chunkMapInfo.AppendI32( map.AlternativeBackgroundColor + 1 );
        chunkMapInfo.AppendI32( map.AlternativeBGColor4 + 1 );
        chunkMapInfo.AppendI32( (int)map.AlternativeMode + 1 );
        chunkMap.Append( chunkMapInfo.ToBuffer() );

        GR.IO.FileChunk chunkMapData = new GR.IO.FileChunk( Types.FileChunk.MAP_DATA );
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
          GR.IO.FileChunk chunkMapExtraData = new GR.IO.FileChunk( Types.FileChunk.MAP_EXTRA_DATA_TEXT );

          chunkMapExtraData.AppendString( map.ExtraDataText );

          chunkMap.Append( chunkMapExtraData.ToBuffer() );
        }
        if ( map.ExtraDataOld.Length > 0 )
        {
          GR.IO.FileChunk chunkMapExtraData = new GR.IO.FileChunk( Types.FileChunk.MAP_EXTRA_DATA );

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
          case Types.FileChunk.MAP_PROJECT_INFO:
            {
              uint version  = chunkReader.ReadUInt32();
              importedCharSet = chunkReader.ReadString();
            }
            break;
          case Types.FileChunk.MAP_CHARSET:
            {
              GR.Memory.ByteBuffer    data = new GR.Memory.ByteBuffer();
              chunkReader.ReadBlock( data, (uint)( chunkReader.Size - chunkReader.Position ) );

              Charset.ReadFromBuffer( data );
            }
            break;
          case Types.FileChunk.MAP_PROJECT_DATA:
            {
              GR.IO.FileChunk chunkData = new GR.IO.FileChunk();

              while ( chunkData.ReadFromStream( chunkReader ) )
              {
                GR.IO.MemoryReader subChunkReader = chunkData.MemoryReader();
                switch ( chunkData.Type )
                {
                  case Types.FileChunk.MULTICOLOR_DATA:
                    Mode = (Types.CharsetMode)subChunkReader.ReadUInt8();
                    BackgroundColor = subChunkReader.ReadUInt8();
                    MultiColor1 = subChunkReader.ReadUInt8();
                    MultiColor2 = subChunkReader.ReadUInt8();
                    BGColor4 = subChunkReader.ReadUInt8();
                    break;
                  case Types.FileChunk.MAP_TILE:
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
                  case Types.FileChunk.MAP:
                    {
                      GR.IO.FileChunk mapChunk = new GR.IO.FileChunk();

                      Map map = new Map();

                      while ( mapChunk.ReadFromStream( subChunkReader ) )
                      {
                        GR.IO.MemoryReader mapChunkReader = mapChunk.MemoryReader();
                        switch ( mapChunk.Type )
                        {
                          case Types.FileChunk.MAP_INFO:
                            map.Name = mapChunkReader.ReadString();
                            map.TileSpacingX = mapChunkReader.ReadInt32();
                            map.TileSpacingY = mapChunkReader.ReadInt32();
                            map.AlternativeMultiColor1 = mapChunkReader.ReadInt32() - 1;
                            map.AlternativeMultiColor2 = mapChunkReader.ReadInt32() - 1;
                            map.AlternativeBackgroundColor = mapChunkReader.ReadInt32() - 1;
                            map.AlternativeBGColor4 = mapChunkReader.ReadInt32() - 1;
                            map.AlternativeMode = (C64Studio.Types.CharsetMode)( mapChunkReader.ReadInt32() - 1 );
                            break;
                          case Types.FileChunk.MAP_DATA:
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
                          case Types.FileChunk.MAP_EXTRA_DATA:
                            {
                              uint len = mapChunkReader.ReadUInt32();

                              mapChunkReader.ReadBlock( map.ExtraDataOld, len );

                              map.ExtraDataText = map.ExtraDataOld.ToString();
                              map.ExtraDataOld.Clear();
                            }
                            break;
                          case Types.FileChunk.MAP_EXTRA_DATA_TEXT:
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


      Charset.MultiColor1 = MultiColor1;
      Charset.MultiColor2 = MultiColor2;
      Charset.BGColor4 = BGColor4;
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
        sbTileCharLo.Append( " <" + LabelPrefix + "TILE_CHAR_" + tile.Name.ToUpper() + Environment.NewLine );
        sbTileCharHi.Append( DataByteDirective );
        sbTileCharHi.Append( " >" + LabelPrefix + "TILE_CHAR_" + tile.Name.ToUpper() + Environment.NewLine );

        sbTileColorLo.Append( DataByteDirective );
        sbTileColorLo.Append( " <" + LabelPrefix + "TILE_COLOR_" + tile.Name.ToUpper() + Environment.NewLine );
        sbTileColorHi.Append( DataByteDirective );
        sbTileColorHi.Append( " >" + LabelPrefix + "TILE_COLOR_" + tile.Name.ToUpper() + Environment.NewLine );

        sbTileChars.Append( LabelPrefix + "TILE_CHAR_" + tile.Name.ToUpper() + Environment.NewLine );
        sbTileChars.Append( DataByteDirective );
        sbTileChars.Append( ' ' );
        for ( int j = 0; j < tile.Chars.Height; ++j )
        {
          for ( int i = 0; i < tile.Chars.Width; ++i )
          {
            sbTileChars.Append( tile.Chars[i, j].Character.ToString() );
            if ( i + j * tile.Chars.Width + 1 < tile.Chars.Width * tile.Chars.Height )
            {
              sbTileChars.Append( ',' );
            }
            else
            {
              sbTileChars.Append( Environment.NewLine );
            }
          }
        }
        sbTileColors.Append( LabelPrefix + "TILE_COLOR_" + tile.Name.ToUpper() + Environment.NewLine );
        sbTileColors.Append( DataByteDirective );
        sbTileColors.Append( ' ' );
        for ( int j = 0; j < tile.Chars.Height; ++j )
        {
          for ( int i = 0; i < tile.Chars.Width; ++i )
          {
            sbTileColors.Append( tile.Chars[i, j].Color.ToString() );
            if ( i + j * tile.Chars.Width + 1 < tile.Chars.Width * tile.Chars.Height )
            {
              sbTileColors.Append( ',' );
            }
            else
            {
              sbTileColors.Append( Environment.NewLine );
            }
          }
        }
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

      StringBuilder[] sbTileChars = new StringBuilder[maxTileWidth * maxTileHeight];
      StringBuilder[] sbTileColors = new StringBuilder[maxTileWidth * maxTileHeight];
      for ( int i = 0; i < sbTileChars.Length; ++i )
      {
        sbTileChars[i] = new StringBuilder();
        sbTileColors[i] = new StringBuilder();

        sbTileChars[i].Append( LabelPrefix + "TILE_CHARS_" );
        sbTileChars[i].Append( ( i % maxTileWidth ) );
        sbTileChars[i].Append( "_" );
        sbTileChars[i].Append( ( i / maxTileWidth ) );
        sbTileChars[i].AppendLine();
        sbTileChars[i].Append( DataByteDirective );
        sbTileChars[i].Append( ' ' );

        sbTileColors[i].Append( LabelPrefix + "TILE_COLORS_" );
        sbTileColors[i].Append( ( i % maxTileWidth ) );
        sbTileColors[i].Append( "_" );
        sbTileColors[i].Append( ( i / maxTileWidth ) );
        sbTileColors[i].AppendLine();
        sbTileColors[i].Append( DataByteDirective );
        sbTileColors[i].Append( ' ' );
      }

      int tileIndex = 0;
      foreach ( var tile in Tiles )
      {
        for ( int j = 0; j < maxTileHeight; ++j )
        {
          for ( int i = 0; i < maxTileWidth; ++i )
          {
            int byteSetIndex = i + j * maxTileWidth;
            if ( ( i < tile.Chars.Width )
            &&   ( j < tile.Chars.Height ) )
            {
              sbTileChars[byteSetIndex].Append( "$" );
              sbTileChars[byteSetIndex].Append( tile.Chars[i, j].Character.ToString( "X2" ) );
              sbTileColors[byteSetIndex].Append( "$" );
              sbTileColors[byteSetIndex].Append( tile.Chars[i, j].Color.ToString( "X2" ) );
            }
            else
            {
              sbTileChars[byteSetIndex].Append( "$00" );
              sbTileColors[byteSetIndex].Append( "$00" );
            }
            if ( tileIndex + 1 < Tiles.Count )
            {
              sbTileChars[byteSetIndex].Append( "," );
              sbTileColors[byteSetIndex].Append( "," );
            }
            else
            {
              sbTileChars[byteSetIndex].AppendLine();
              sbTileColors[byteSetIndex].AppendLine();
            }
          }
        }
        ++tileIndex;
      }

      StringBuilder sb = new StringBuilder();

      sb.AppendLine( LabelPrefix + "NUM_TILES = " + Tiles.Count );

      for ( int j = 0; j < maxTileHeight; ++j )
      {
        for ( int i = 0; i < maxTileWidth; ++i )
        {
          sb.AppendLine( sbTileChars[i + j * maxTileWidth].ToString() );
        }
      }
      for ( int j = 0; j < maxTileHeight; ++j )
      {
        for ( int i = 0; i < maxTileWidth; ++i )
        {
          sb.AppendLine( sbTileColors[i + j * maxTileWidth].ToString() );
        }
      }
      TileData = sb.ToString();
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
        sbMaps.AppendLine( "<" + LabelPrefix + "MAP_" + Maps[i].Name.ToUpper().Replace( ' ', '_' ) );
      }
      sbMaps.AppendLine();
      sbMaps.Append( LabelPrefix );
      sbMaps.AppendLine( "MAP_LIST_HI" );
      for ( int i = 0; i < Maps.Count; ++i )
      {
        sbMaps.Append( DataByteDirective );
        sbMaps.Append( ' ' );
        sbMaps.AppendLine( ">" + LabelPrefix + "MAP_" + Maps[i].Name.ToUpper().Replace( ' ', '_' ) );
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
          sbMaps.AppendLine( "<" + LabelPrefix + "MAP_EXTRA_DATA_" + Maps[i].Name.ToUpper().Replace( ' ', '_' ) );
        }
        sbMaps.Append( LabelPrefix );
        sbMaps.AppendLine( "MAP_EXTRA_DATA_LIST_HI" );
        for ( int i = 0; i < Maps.Count; ++i )
        {
          sbMaps.Append( DataByteDirective );
          sbMaps.Append( ' ' );
          sbMaps.AppendLine( ">" + LabelPrefix + "MAP_EXTRA_DATA_" + Maps[i].Name.ToUpper().Replace( ' ', '_' ) );
        }
        sbMaps.AppendLine();
      }


      for ( int i = 0; i < Maps.Count; ++i )
      {
        var map = Maps[i];

        sbMaps.AppendLine();
        sbMaps.Append( LabelPrefix );
        sbMaps.AppendLine( "MAP_" + map.Name.ToUpper().Replace( ' ', '_' ) );

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
          sbMaps.AppendLine( "MAP_EXTRA_DATA_" + map.Name.ToUpper().Replace( ' ', '_' ) );

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
