using RetroDevStudio.Types;
using GR.Memory;
using RetroDevStudio;
using RetroDevStudio.Formats;
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
  public partial class ExportMapAsBinaryFile : ExportMapFormBase
  {
    public ExportMapAsBinaryFile() :
      base( null )
    { 
    }



    public ExportMapAsBinaryFile( StudioCore Core ) :
      base( Core )
    {
      InitializeComponent();
    }



    public override bool HandleExport( ExportMapInfo Info, TextBox EditOutput, DocumentInfo DocInfo )
    {
      System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();

      saveDlg.Title = "Save data as";
      saveDlg.Filter = "Map Data|*.map|Binary Data|*.bin|All Files|*.*";
      //saveDlg.InitialDirectory = GR.Path.GetDirectoryName( Info.Map.File

      if ( saveDlg.ShowDialog() != System.Windows.Forms.DialogResult.OK )
      {
        return false;
      }

      // prepare data
      GR.Memory.ByteBuffer tileData = new GR.Memory.ByteBuffer();
      GR.Memory.ByteBuffer mapData = new GR.Memory.ByteBuffer();

      GR.Memory.ByteBuffer finalData = null;

      switch ( Info.ExportType )
      {
        case MapExportType.TILE_DATA:
          Info.Map.ExportTilesAsBuffer( Info.RowByRow, out tileData );
          finalData = tileData;
          break;
        case MapExportType.TILE_AND_MAP_DATA:
          Info.Map.ExportTilesAsBuffer( Info.RowByRow, out tileData );
          mapData = Info.Map.ExportMapsAsBuffer( Info.RowByRow );
          finalData = tileData + mapData;
          break;
        case MapExportType.MAP_DATA:
          {
            bool    vertical = !Info.RowByRow;

            finalData = new ByteBuffer();
            foreach ( var map in Info.Map.Maps )
            {
              GR.Memory.ByteBuffer      selectionData = new GR.Memory.ByteBuffer();

              if ( vertical )
              {
                // select all
                for ( int i = 0; i < map.Tiles.Width; ++i )
                {
                  for ( int j = 0; j < map.Tiles.Height; ++j )
                  {
                    selectionData.AppendU8( (byte)map.Tiles[i, j] );
                  }
                }
              }
              else
              {
                // select all
                for ( int j = 0; j < map.Tiles.Height; ++j )
                {
                  for ( int i = 0; i < map.Tiles.Width; ++i )
                  {
                    selectionData.AppendU8( (byte)map.Tiles[i, j] );
                  }
                }
              }
              finalData += selectionData;
            }
          }
          break;
        case MapExportType.MAP_DATA_SELECTION:
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
              finalData = selectionData;
            }
          }
          break;
        default:
          Core.Notification.MessageBox( "Export type not supported", "The export type " + Info.ExportType + " is not supported for binary export." );
          return false;
      }
      if ( finalData != null )
      {
        if ( checkPrefixLoadAddress.Checked )
        {
          ushort address = GR.Convert.ToU16( editPrefixLoadAddress.Text, 16 );

          var addressData = new ByteBuffer();
          addressData.AppendU16( address );
          finalData = addressData + finalData;
        }
        GR.IO.File.WriteAllBytes( saveDlg.FileName, finalData );
      }
      return true;
    }



    private void checkPrefixLoadAddress_CheckedChanged(object sender, EventArgs e)
    {
      editPrefixLoadAddress.Enabled = checkPrefixLoadAddress.Checked;
    }



  }
}
