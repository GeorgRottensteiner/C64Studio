using System;
using System.Collections.Generic;
using System.Text;
using RetroDevStudio.Formats;



namespace RetroDevStudio.Undo
{
  public class UndoMapTileAdd : UndoTask
  {
    public MapEditor              _MapEditor = null;
    public MapProject             _MapProject = null;
    public int                    _TileIndex = -1;



    public UndoMapTileAdd( MapEditor Editor, MapProject Project, int TileIndex )
    {
      _MapEditor  = Editor;
      _MapProject = Project;
      _TileIndex  = TileIndex;
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
