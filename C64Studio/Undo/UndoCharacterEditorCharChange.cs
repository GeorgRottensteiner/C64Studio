using C64Studio.Controls;
using C64Studio.Formats;



namespace C64Studio.Undo
{
  public class UndoCharacterEditorCharChange : UndoTask
  {
    public CharacterEditor        Editor = null;
    public CharsetProject         Project = null;
    public int                    CharIndex = 0;
    public CharData               Char = null;

    public int                    CharColor = 0;



    public UndoCharacterEditorCharChange( CharacterEditor Editor, CharsetProject Project, int CharIndex )
    {
      this.Editor = Editor;
      this.Project = Project;
      this.CharIndex = CharIndex;

      Char = new CharData();
      Char.Tile.Data        = new GR.Memory.ByteBuffer( Project.Characters[CharIndex].Tile.Data );
      Char.Tile.CustomColor = Project.Characters[CharIndex].Tile.CustomColor;
      Char.Category         = Project.Characters[CharIndex].Category;
      Char.Index            = CharIndex;
    }



    public override string Description
    {
      get
      {
        return "CharacterEditor Char Change";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoCharacterEditorCharChange( Editor, Project, CharIndex );
    }



    public override void Apply()
    {
      Project.Characters[CharIndex].Tile.Data = new GR.Memory.ByteBuffer( Char.Tile.Data );
      Project.Characters[CharIndex].Tile.CustomColor = Char.Tile.CustomColor;
      Project.Characters[CharIndex].Category = Char.Category;
      Project.Characters[CharIndex].Index = Char.Index;

      Editor.CharacterChanged( CharIndex );
    }
  }
}
