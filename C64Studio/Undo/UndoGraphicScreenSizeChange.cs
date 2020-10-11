using System;
using System.Collections.Generic;
using System.Text;
using C64Studio.Formats;



namespace C64Studio.Undo
{
  public class UndoGraphicScreenSizeChange : UndoTask
  {
    public GraphicScreenProject   Project = null;
    public GraphicScreenEditor    Editor = null;



    public int        Width = 0;
    public int        Height = 0;

    public GR.Image.MemoryImage       ChangedSection = null;



    public UndoGraphicScreenSizeChange( GraphicScreenProject Project, GraphicScreenEditor Editor, int Width, int Height )
    {
      this.Width = Width;
      this.Height = Height;
      this.Editor = Editor;
      this.Project = Project;

      ChangedSection = Project.Image.GetImage( 0, 0, Width, Height ) as GR.Image.MemoryImage;
    }




    public override string Description
    {
      get
      {
        return "Graphic screen size change";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoGraphicScreenSizeChange( Project, Editor, Project.ScreenWidth, Project.ScreenHeight );
    }



    public override void Apply()
    {
      Editor.SetScreenSize( Width, Height );
      ChangedSection.DrawTo( Project.Image, 0, 0 );
      Editor.UpdateArea( 0, 0, Width, Height );
      Editor.SetModified();
    }

  }
}
