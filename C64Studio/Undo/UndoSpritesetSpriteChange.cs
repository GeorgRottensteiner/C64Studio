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

      Sprite = new SpriteProject.SpriteData();

      Sprite.Color = Project.Sprites[SpriteIndex].Color;
      Sprite.Data = new GR.Memory.ByteBuffer( Project.Sprites[SpriteIndex].Data );
      Sprite.Multicolor = Project.Sprites[SpriteIndex].Multicolor;
    }




    public string Description
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
      Project.Sprites[SpriteIndex].Data = new GR.Memory.ByteBuffer( Sprite.Data );
      Project.Sprites[SpriteIndex].Color = Sprite.Color;
      Project.Sprites[SpriteIndex].Multicolor = Sprite.Multicolor;

      Editor.SpriteChanged( SpriteIndex );
    }
  }
}
