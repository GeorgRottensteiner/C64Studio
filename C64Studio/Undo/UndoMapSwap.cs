using RetroDevStudio.Documents;
using RetroDevStudio.Formats;



namespace RetroDevStudio.Undo
{
  public class UndoMapSwap : UndoTask
  {
    public MapEditor              _MapEditor = null;
    public MapProject             _MapProject = null;
    public int                    _MapIndex1 = -1;
    public int                    _MapIndex2 = -1;



    public UndoMapSwap( MapEditor Editor, MapProject Project, int MapIndex1, int MapIndex2 )
    {
      _MapEditor  = Editor;
      _MapProject = Project;
      _MapIndex1  = MapIndex1;
      _MapIndex2  = MapIndex2;
    }




    public override string Description
    {
      get
      {
        return "Swap Map";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoMapSwap( _MapEditor, _MapProject, _MapIndex1, _MapIndex2 );
    }



    public override void Apply()
    {
      _MapEditor.SwapMap( _MapIndex1, _MapIndex2 );
    }
  }
}
