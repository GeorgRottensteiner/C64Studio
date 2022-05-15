using System;
using System.Collections.Generic;
using System.Text;
using RetroDevStudio.Formats;



namespace RetroDevStudio.Undo
{
  public class UndoSpritesetSpriteChange : UndoTask
  {
    public SpriteEditor           Editor = null;
    public SpriteProject          Project = null;
    public int                    SpriteIndex = 0;
    public SpriteProject.SpriteData   Sprite = null;
    public int                    ActivePalette = 0;



    public UndoSpritesetSpriteChange( SpriteEditor Editor, SpriteProject Project, int SpriteIndex )
    {
      this.Editor = Editor;
      this.Project = Project;
      this.SpriteIndex = SpriteIndex;

      ActivePalette = Project.Sprites[SpriteIndex].Tile.Colors.ActivePalette;

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
      Project.Sprites[SpriteIndex].Tile.Colors.ActivePalette = ActivePalette;

      Editor.SpriteChanged( SpriteIndex );
    }
  }
}
