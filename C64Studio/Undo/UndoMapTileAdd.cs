using RetroDevStudio.Documents;
using RetroDevStudio.Formats;
using System.Collections.Generic;



namespace RetroDevStudio.Undo
{
  public class UndoMapTileAdd : UndoTask
  {
    public MapEditor              _MapEditor = null;
    public MapProject             _MapProject = null;
    public int                    _TileIndex = -1;
    public List<Undo.UndoTask>    _InternalUndos = new List<UndoTask>();



    public UndoMapTileAdd( MapEditor Editor, MapProject Project, int TileIndex )
    {
      _MapEditor  = Editor;
      _MapProject = Project;
      _TileIndex  = TileIndex;

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
        return "Add Map Tile";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoMapTileRemove( _MapEditor, _MapProject, _TileIndex );
    }



    public override void Apply()
    {
      _MapEditor.RemoveTile( _TileIndex );
    }
  }
}
