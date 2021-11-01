using System;
using System.Collections.Generic;
using System.Text;
using C64Studio.Formats;
using RetroDevStudio;
using RetroDevStudio.Types;
using static C64Studio.Formats.SpriteProject;



namespace C64Studio.Undo
{
  public class UndoSpritesetValuesChange : UndoTask
  {
    public SpriteEditor           Editor = null;
    public SpriteProject          Project = null;
    public SpriteProjectMode      Mode = SpriteProjectMode.COMMODORE_24_X_21_HIRES_OR_MC;
    public ColorSettings          Colors = null;



    public UndoSpritesetValuesChange( SpriteEditor Editor, SpriteProject Project )
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
        return "Spriteset Values Change";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoSpritesetValuesChange( Editor, Project );
    }



    public override void Apply()
    {
      Project.Colors  = new ColorSettings( Colors );
      Project.Mode    = Mode;

      Editor.ColorsChanged();
    }



  }
}
