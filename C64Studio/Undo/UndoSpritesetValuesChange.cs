using System;
using System.Collections.Generic;
using System.Text;
using C64Studio.Formats;



namespace C64Studio.Undo
{
  public class UndoSpritesetValuesChange : UndoTask
  {
    public SpriteEditor           Editor = null;
    public SpriteProject          Project = null;
    public int                    BGColor = -1;
    public int                    MultiColor1 = -1;
    public int                    MultiColor2 = -1;



    public UndoSpritesetValuesChange( SpriteEditor Editor, SpriteProject Project )
    {
      this.Editor = Editor;
      this.Project = Project;

      BGColor = Project.BackgroundColor;
      MultiColor1 = Project.MultiColor1;
      MultiColor2 = Project.MultiColor2;
    }




    public string Description
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
      Project.BackgroundColor = BGColor;
      Project.MultiColor1 = MultiColor1;
      Project.MultiColor2 = MultiColor2;

      Editor.ColorsChanged();
    }
  }
}
