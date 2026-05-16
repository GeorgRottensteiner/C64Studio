using RetroDevStudio.Controls;
using RetroDevStudio.Formats;
using RetroDevStudio;
using RetroDevStudio.Types;

namespace RetroDevStudio.Undo
{
  public class UndoCharacterEditorCharacterNameChange : UndoTask
  {
    public CharacterEditor        Editor = null;
    public string                 Name = "";
    public int                    CharacterIndex = 0;          



    public UndoCharacterEditorCharacterNameChange( CharacterEditor editor, int charIndex )
    {
      Name            = editor.CharacterSet.Characters[charIndex].Name;
      Editor          = editor;
      CharacterIndex  = charIndex;
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
      return new UndoCharacterEditorCharacterNameChange( Editor, CharacterIndex );
    }



    public override void Apply()
    {
      Editor.CharacterSet.Characters[CharacterIndex].Name = Name;

      Editor.CharacterNameChanged( CharacterIndex );
    }
  }
}
