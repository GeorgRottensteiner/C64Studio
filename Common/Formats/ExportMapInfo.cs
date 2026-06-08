using RetroDevStudio.Formats;
using System.Collections.Generic;
using System.ComponentModel;

namespace RetroDevStudio.Formats
{
  public class ExportMapInfo
  {
    public MapProject                 Map;
    public MapExportType              ExportType = MapExportType.TILE_AND_MAP_DATA;
    public bool                       RowByRow = true;
    public bool[,]                    SelectedTiles;
    public MapProject.Map             CurrentMap = null;
  }



  public enum MapExportType
  {
    [Description( "tile data as elements" )]
    TILE_DATA_AS_ELEMENTS,
    [Description( "tile data" )]
    TILE_DATA,
    [Description( "map data (all maps)" )]
    MAP_DATA,
    [Description( "tile data, then map data (all maps)" )]
    TILE_AND_MAP_DATA,
    [Description( "map data from selection (current map)" )]
    MAP_DATA_SELECTION
  }
}
