using System;
using System.Collections.Generic;
using System.Text;
using C64Studio.Formats;



namespace C64Studio.Undo
{
  public class UndoCharsetValuesChange : UndoTask
  {
    public CharsetEditor          Editor = null;
    public CharsetProject         Project = null;
    public int                    BGColor = -1;
    public int                    MultiColor1 = -1;
    public int                    MultiColor2 = -1;
    public int                    BGColor4 = -1;



    public UndoCharsetValuesChange( CharsetEditor Editor, CharsetProject Project )
    {
      this.Editor = Editor;
      this.Project = Project;

      BGColor = Project.BackgroundColor;
      MultiColor1 = Project.MultiColor1;
      MultiColor2 = Project.MultiColor2;
      BGColor4 = Project.BGColor4;
    }




    public string Description
    {
      get
      {
        return "Charset Values Change";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoCharsetValuesChange( Editor, Project );
    }



    public override void Apply()
    {
      Project.BackgroundColor = BGColor;
      Project.MultiColor1     = MultiColor1;
      Project.MultiColor2     = MultiColor2;
      Project.BGColor4        = BGColor4;

      Editor.ColorsChanged();
    }
  }
}
