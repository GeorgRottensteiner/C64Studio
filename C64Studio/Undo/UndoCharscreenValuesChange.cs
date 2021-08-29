using C64Studio.Formats;
using RetroDevStudioModels;



namespace C64Studio.Undo
{
  public class UndoCharscreenValuesChange : UndoTask
  {
    public CharsetScreenProject   Project = null;
    public CharsetScreenEditor    Editor = null;


    public MachineType          Machine = MachineType.UNKNOWN;
    public int                  BackgroundColor = 0;
    public int                  Multicolor1 = 0;
    public int                  Multicolor2 = 0;
    public int                  BGColor4 = 0;
    public TextMode             Mode = TextMode.COMMODORE_40_X_25_HIRES;


    public UndoCharscreenValuesChange( CharsetScreenProject Project, CharsetScreenEditor Editor )
    {
      this.Project = Project;
      this.Editor = Editor;
      BackgroundColor = Project.BackgroundColor;
      Multicolor1     = Project.MultiColor1;
      Multicolor2     = Project.MultiColor2;
      BGColor4        = Project.BGColor4;
      Mode            = Project.Mode;
    }




    public override string Description
    {
      get
      {
        return "Charset screen values change";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoCharscreenValuesChange( Project, Editor );
    }



    public override void Apply()
    {
      Project.BackgroundColor = BackgroundColor;
      Project.Mode            = Mode;
      Project.MultiColor1     = Multicolor1;
      Project.MultiColor2     = Multicolor2;
      Project.BGColor4        = BGColor4;

      Editor.ValuesChanged();
      Editor.SetModified();
    }
  }
}
