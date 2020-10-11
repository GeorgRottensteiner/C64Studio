using System;
using System.Collections.Generic;
using System.Text;
using C64Studio.Formats;



namespace C64Studio.Undo
{
  public class UndoMapTileRemove : UndoTask
  {
    private MapEditor               _MapEditor = null;
    private MapProject.Tile         _RemovedTile = null;
    private MapProject              _MapProject = null;
    private int                     _TileIndex = -1;



    public UndoMapTileRemove( MapEditor Editor, MapProject Project, int TileIndex )
    {
      _MapEditor  = Editor;
      _TileIndex  = TileIndex;
      _MapProject = Project;
      _RemovedTile = _MapProject.Tiles[TileIndex];
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
