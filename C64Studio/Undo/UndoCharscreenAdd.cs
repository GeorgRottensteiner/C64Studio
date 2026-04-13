using RetroDevStudio.Documents;
using RetroDevStudio.Formats;



namespace RetroDevStudio.Undo
{
  public class UndoCharscreenAdd : UndoTask
  {
    public CharsetScreenEditor    _Editor = null;
    public CharsetScreenProject   _Project = null;
    public int                    _ScreenIndex = -1;



    public UndoCharscreenAdd( CharsetScreenEditor editor, CharsetScreenProject project, int screenIndex )
    {
      _Editor       = editor;
      _Project      = project;
      _ScreenIndex  = screenIndex;
    }




    public override string Description
    {
      get
      {
        return "Add Screen";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoCharscreenRemove( _Editor, _Project, _ScreenIndex );
    }



    public override void Apply()
    {
      _Editor.RemoveScreen( _ScreenIndex );
    }



  }
}
