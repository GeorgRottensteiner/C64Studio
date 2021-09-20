using C64Studio.Formats;



namespace C64Studio.Undo
{
  public class UndoCharsetCharChange : UndoTask
  {
    public CharsetEditor          Editor = null;
    public CharsetProject         Project = null;
    public int                    CharIndex = 0;
    public CharData               Char = null;

    public int                    CharColor = 0;



    public UndoCharsetCharChange( CharsetEditor Editor, CharsetProject Project, int CharIndex )
    {
      this.Editor = Editor;
      this.Project = Project;
      this.CharIndex = CharIndex;

      Char = new CharData();
      Char.Data       = new GR.Memory.ByteBuffer( Project.Characters[CharIndex].Data );
      Char.Color      = Project.Characters[CharIndex].Color;
      Char.Category   = Project.Characters[CharIndex].Category;
      Char.Index      = CharIndex;
    }



    public override string Description
    {
      get
      {
        return "Charset Char Change";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoCharsetCharChange( Editor, Project, CharIndex );
    }



    public override void Apply()
    {
      Project.Characters[CharIndex].Data = new GR.Memory.ByteBuffer( Char.Data );
      Project.Characters[CharIndex].Color = Char.Color;
      Project.Characters[CharIndex].Category = Char.Category;
      Project.Characters[CharIndex].Index = Char.Index;

      Editor.CharacterChanged( CharIndex );
    }
  }
}
