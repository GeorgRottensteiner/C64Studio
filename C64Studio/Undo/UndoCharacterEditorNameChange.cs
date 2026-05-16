using RetroDevStudio.Controls;
using RetroDevStudio.Formats;
using RetroDevStudio;
using RetroDevStudio.Types;

namespace RetroDevStudio.Undo
{
  public class UndoCharacterEditorNameChange : UndoTask
  {
    public CharacterEditor        Editor = null;
    public string                 Name = "";



    public UndoCharacterEditorNameChange( CharacterEditor editor )
    {
      Name    = editor.CharacterSet.Name;
      Editor  = editor;
    }




    public override string Description
    {
      get
      {
        return "Charset Name Change";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoCharacterEditorNameChange( Editor );
    }



    public override void Apply()
    {
      Editor.CharacterSet.Name = Name;

      Editor.NameChanged();
    }
  }
}
