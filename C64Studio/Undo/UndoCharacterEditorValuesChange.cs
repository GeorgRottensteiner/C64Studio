using RetroDevStudio.Controls;
using RetroDevStudio.Formats;
using RetroDevStudio;
using RetroDevStudio.Types;

namespace RetroDevStudio.Undo
{
  public class UndoCharacterEditorValuesChange : UndoTask
  {
    public CharacterEditor        Editor = null;
    public CharsetProject         Project = null;
    public ColorSettings          Colors = null;
    public TextCharMode           Mode = TextCharMode.UNKNOWN;



    public UndoCharacterEditorValuesChange( CharacterEditor Editor, CharsetProject Project )
    {
      this.Editor = Editor;
      this.Project = Project;
      Mode = Project.Mode;

      Colors = new ColorSettings( Project.Colors );
    }




    public override string Description
    {
      get
      {
        return "Charset Values Change";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoCharacterEditorValuesChange( Editor, Project );
    }



    public override void Apply()
    {
      Project.Colors  = Colors;
      Project.Mode    = Mode;

      Editor.ColorsChanged();
    }
  }
}
