using RetroDevStudio;
using RetroDevStudio.Formats;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace RetroDevStudio.Controls
{
  public partial class ExportMapAsCArray : ExportMapFormBase
  {
    public ExportMapAsCArray() :
      base( null )
    { 
    }



    public ExportMapAsCArray( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    private void checkExportToDataWrap_CheckedChanged( object sender, EventArgs e )
    {
      editWrapByteCount.Enabled = checkExportToDataWrap.Checked;
    }



    private int GetExportWrapCount()
    {
      if ( checkExportToDataWrap.Checked )
      {
        int wrapByteCount = GR.Convert.ToI32( editWrapByteCount.Text );
        if ( wrapByteCount <= 0 )
        {
          wrapByteCount = 8;
        }
        return wrapByteCount;
      }
      return 80;
    }



    public override bool HandleExport( ExportMapInfo Info, TextBox EditOutput, DocumentInfo DocInfo )
    {
      int wrapByteCount = GetExportWrapCount();
      bool wrapData = checkExportToDataWrap.Checked;
      bool exportHex = checkExportHex.Checked;

      string tileData = "";
      string mapData = "";

      if ( Info.ExportType == MapExportType.TILE_DATA_AS_ELEMENTS )
      {
        Info.Map.ExportTilesAsCArrayElements( out tileData, "tiles_", wrapData, wrapByteCount, exportHex );
      }
      if ( ( Info.ExportType == MapExportType.TILE_DATA )
      ||   ( Info.ExportType == MapExportType.TILE_AND_MAP_DATA ) )
      {
        Info.Map.ExportTilesAsCArray( out tileData, wrapData, wrapByteCount, exportHex );
      }
      if ( Info.ExportType == MapExportType.MAP_DATA_SELECTION )
      {
        bool    vertical = !Info.RowByRow;

        if ( Info.CurrentMap != null )
        {
          GR.Memory.ByteBuffer      selectionData = new GR.Memory.ByteBuffer();
          bool                      hasSelection = false;

          if ( vertical )
          {
            for ( int i = 0; i < Info.CurrentMap.Tiles.Width; ++i )
            {
              for ( int j = 0; j < Info.CurrentMap.Tiles.Height; ++j )
              {
                if ( Info.SelectedTiles[i, j] )
                {
                  selectionData.AppendU8( (byte)Info.CurrentMap.Tiles[i, j] );
                  hasSelection = true;
                }
              }
            }
            if ( !hasSelection )
            {
              // select all
              for ( int i = 0; i < Info.CurrentMap.Tiles.Width; ++i )
              {
                for ( int j = 0; j < Info.CurrentMap.Tiles.Height; ++j )
                {
                  selectionData.AppendU8( (byte)Info.CurrentMap.Tiles[i, j] );
                }
              }
            }
          }
          else
          {
            for ( int j = 0; j < Info.CurrentMap.Tiles.Height; ++j )
            {
              for ( int i = 0; i < Info.CurrentMap.Tiles.Width; ++i )
              {
                if ( Info.SelectedTiles[i, j] )
                {
                  selectionData.AppendU8( (byte)Info.CurrentMap.Tiles[i, j] );
                  hasSelection = true;
                }
              }
            }
            if ( !hasSelection )
            {
              // select all
              for ( int j = 0; j < Info.CurrentMap.Tiles.Height; ++j )
              {
                for ( int i = 0; i < Info.CurrentMap.Tiles.Width; ++i )
                {
                  selectionData.AppendU8( (byte)Info.CurrentMap.Tiles[i, j] );
                }
              }
            }
          }
          mapData = Util.ToCArray( selectionData, wrapData, wrapByteCount, "mapdata_", exportHex );
        }
      }
      if ( ( Info.ExportType == MapExportType.MAP_DATA )
      ||   ( Info.ExportType == MapExportType.TILE_AND_MAP_DATA ) )
      {
        Info.Map.ExportMapsAsCArray( !Info.RowByRow, out mapData, "maps_", wrapData, wrapByteCount, exportHex );
      }

      string resultText = "";

      switch ( Info.ExportType )
      {
        case MapExportType.TILE_DATA:
        case MapExportType.TILE_DATA_AS_ELEMENTS:
          resultText = tileData;
          break;
        case MapExportType.MAP_DATA:
        case MapExportType.MAP_DATA_SELECTION:
          resultText = mapData;
          break;
        case MapExportType.TILE_AND_MAP_DATA:
          resultText = tileData + mapData;
          break;
      }

      EditOutput.Text = resultText;
      return true;
    }



  }
}
