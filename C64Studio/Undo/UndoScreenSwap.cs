using RetroDevStudio.Documents;
using RetroDevStudio.Formats;



namespace RetroDevStudio.Undo
{
  public class UndoScreenSwap : UndoTask
  {
    public CharsetScreenEditor    _Editor = null;
    public CharsetScreenProject   _Project = null;
    public int                    _ScreenIndex1 = -1;
    public int                    _ScreenIndex2 = -1;



    public UndoScreenSwap( CharsetScreenEditor editor, CharsetScreenProject project, int screenIndex1, int screenIndex2 )
    {
      _Editor         = editor;
      _Project        = project;
      _ScreenIndex1   = screenIndex1;
      _ScreenIndex2   = screenIndex2;
    }




    public override string Description
    {
      get
      {
        return "Swap Screen";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoScreenSwap( _Editor, _Project, _ScreenIndex1, _ScreenIndex2 );
    }



    public override void Apply()
    {
      _Editor.SwapScreens( _ScreenIndex1, _ScreenIndex2 );
    }
  }
}
