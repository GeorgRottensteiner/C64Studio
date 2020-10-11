using System;
using System.Collections.Generic;
using System.Text;
using C64Studio.Formats;



namespace C64Studio.Undo
{
  public class UndoMapRemove : UndoTask
  {
    private MapEditor               _MapEditor = null;
    private MapProject.Map          _RemovedMap = null;
    private MapProject              _MapProject = null;
    private int                     _MapIndex = -1;



    public UndoMapRemove( MapEditor Editor, MapProject Project, int MapIndex )
    {
      _MapEditor  = Editor;
      _MapIndex   = MapIndex;
      _MapProject = Project;
      _RemovedMap = _MapProject.Maps[_MapIndex];
    }




    public override string Description
    {
      get
      {
        return "Remove Map";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoMapAdd( _MapEditor, _MapProject, _MapIndex );
    }



    public override void Apply()
    {
      _MapEditor.AddMap( _MapIndex, _RemovedMap );
    }
  }
}
