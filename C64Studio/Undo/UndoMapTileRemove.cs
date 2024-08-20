using RetroDevStudio.Documents;
using RetroDevStudio.Formats;
using System.Collections.Generic;



namespace RetroDevStudio.Undo
{
  public class UndoMapTileRemove : UndoTask
  {
    private MapEditor               _MapEditor = null;
    private MapProject.Tile         _RemovedTile = null;
    private MapProject              _MapProject = null;
    private int                     _TileIndex = -1;
    public List<Undo.UndoTask>      _InternalUndos = new List<UndoTask>();



    public UndoMapTileRemove( MapEditor Editor, MapProject Project, int TileIndex )
    {
      _MapEditor  = Editor;
      _TileIndex  = TileIndex;
      _MapProject = Project;
      _RemovedTile = _MapProject.Tiles[TileIndex];

      foreach ( var map in Project.Maps )
      {
        for ( int i = 0; i < map.Tiles.Width; ++i )
        {
          for ( int j = 0; j < map.Tiles.Height; ++j )
          {
            if ( map.Tiles[i, j] >= TileIndex )
            {
              i = map.Tiles.Width;
              _InternalUndos.Add( new Undo.UndoMapTilesChange( _MapEditor, map, 0, 0, map.Tiles.Width, map.Tiles.Height ) );
              break;
            }
          }
        }
      }
    }




    public override string Description
    {
      get
      {
        return "Remove Map Tile";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoMapTileAdd( _MapEditor, _MapProject, _TileIndex );
    }



    public override void Apply()
    {
      _MapEditor.AddTile( _TileIndex, _RemovedTile );
    }
  }
}
