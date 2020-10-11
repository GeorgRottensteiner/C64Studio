using System;
using System.Collections.Generic;
using System.Text;
using C64Studio.Formats;



namespace C64Studio.Undo
{
  public class UndoMapTileExchange : UndoTask
  {
    public MapEditor              _MapEditor = null;
    public MapProject             _MapProject = null;
    public int                    _TileIndex1 = -1;
    public int                    _TileIndex2 = -1;



    public UndoMapTileExchange( MapEditor Editor, MapProject Project, int TileIndex1, int TileIndex2 )
    {
      _MapEditor    = Editor;
      _MapProject   = Project;
      _TileIndex1   = TileIndex1;
      _TileIndex2   = TileIndex2;
    }




    public override string Description
    {
      get
      {
        return "Exchange Map Tiles";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoMapTileExchange( _MapEditor, _MapProject, _TileIndex2, _TileIndex1 );
    }



    public override void Apply()
    {
      _MapEditor.SwapTiles( _TileIndex1, _TileIndex2 );
    }
  }
}
