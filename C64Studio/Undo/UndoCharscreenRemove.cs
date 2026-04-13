using RetroDevStudio.Documents;
using RetroDevStudio.Formats;



namespace RetroDevStudio.Undo
{
  public class UndoCharscreenRemove : UndoTask
  {
    public CharsetScreenEditor    _Editor = null;
    public CharsetScreenProject   _Project = null;
    public CharsetScreen          _RemovedScreen = null;    
    public int                    _ScreenIndex = -1;



    public UndoCharscreenRemove( CharsetScreenEditor editor, CharsetScreenProject project, int screenIndex )
    {
      _Editor         = editor;
      _ScreenIndex    = screenIndex;
      _Project        = project;
      _RemovedScreen  = _Project.Screens[_ScreenIndex];
    }




    public override string Description
    {
      get
      {
        return "Remove Screen";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoCharscreenAdd( _Editor, _Project, _ScreenIndex );
    }



    public override void Apply()
    {
      _Editor.AddScreen( _ScreenIndex, _RemovedScreen );
    }



  }
}
