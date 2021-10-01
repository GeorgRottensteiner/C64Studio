using System;
using System.Collections.Generic;
using System.Text;
using C64Studio.Formats;



namespace C64Studio.Undo
{
  public class UndoSpritesetSpriteChange : UndoTask
  {
    public SpriteEditor           Editor = null;
    public SpriteProject          Project = null;
    public int                    SpriteIndex = 0;
    public SpriteProject.SpriteData   Sprite = null;



    public UndoSpritesetSpriteChange( SpriteEditor Editor, SpriteProject Project, int SpriteIndex )
    {
      this.Editor = Editor;
      this.Project = Project;
      this.SpriteIndex = SpriteIndex;

      Sprite = new SpriteProject.SpriteData( Project.Sprites[SpriteIndex] );
    }




    public override string Description
    {
      get
      {
        return "Spriteset Sprite Change";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoSpritesetSpriteChange( Editor, Project, SpriteIndex );
    }



    public override void Apply()
    {
      Project.Sprites[SpriteIndex] = new SpriteProject.SpriteData( Sprite );

      Editor.SpriteChanged( SpriteIndex );
    }
  }
}
