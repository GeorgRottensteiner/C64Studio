using RetroDevStudio.Documents;
using RetroDevStudio.Formats;



namespace RetroDevStudio.Undo
{
  public class UndoCharscreenNameChange : UndoTask
  {
    public CharsetScreenProject   Project = null;
    public CharsetScreenEditor    Editor = null;
    public int                    ScreenIndex = 0;  
    public string                 OldName = "";        



    public UndoCharscreenNameChange( CharsetScreenProject project, CharsetScreenEditor editor, int screenIndex )
    {
      OldName = project.Screens[screenIndex].Name;
    }




    public override string Description
    {
      get
      {
        return "Charset screen name change";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoCharscreenNameChange( Project, Editor, ScreenIndex );
    }



    public override void Apply()
    {
      Project.Screens[ScreenIndex].Name = OldName;

      Editor.comboScreens.Items[ScreenIndex] = OldName;
      Editor.comboScreens.SelectedIndex = ScreenIndex;
    }

  }
}
