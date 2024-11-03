using RetroDevStudio.Documents;
using RetroDevStudio.Formats;



namespace RetroDevStudio.Undo
{
  public class UndoSpritesetShiftSprites : UndoTask
  {
    public SpriteEditor           Editor = null;
    public SpriteProject          Project = null;
    public int[]                  NewToOld = null;
    public int[]                  OldToNew = null;



    public UndoSpritesetShiftSprites( SpriteEditor Editor, SpriteProject Project, int[] NewToOld, int[] OldToNew )
    {
      this.Editor = Editor;
      this.Project = Project;
      this.NewToOld = NewToOld;
      this.OldToNew = OldToNew;
    }




    public override string Description
    {
      get
      {
        return "Spriteset Shift Sprites";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoSpritesetShiftSprites( Editor, Project, OldToNew, NewToOld );
    }



    public override void Apply()
    {
      Editor.ShiftSprites( NewToOld, OldToNew );
    }
  }
}
