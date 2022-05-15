using System;
using System.Collections.Generic;
using System.Text;
using RetroDevStudio.Formats;



namespace RetroDevStudio.Undo
{
  public class UndoGraphicScreenImageChange : UndoTask
  {
    public GraphicScreenProject   Project = null;
    public GraphicScreenEditor    Editor = null;



    public int        X = 0;
    public int        Y = 0;
    public int        Width = 0;
    public int        Height = 0;

    public GR.Image.MemoryImage       ChangedSection = null;



    public UndoGraphicScreenImageChange( GraphicScreenProject Project, GraphicScreenEditor Editor, int X, int Y, int Width, int Height )
    {
      this.X = X;
      this.Y = Y;
      this.Width = Width;
      this.Height = Height;
      this.Editor = Editor;
      this.Project = Project;

      ChangedSection = Project.Image.GetImage( X, Y, Width, Height ) as GR.Image.MemoryImage;
    }




    public override string Description
    {
      get
      {
        return "Graphic screen image change";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoGraphicScreenImageChange( Project, Editor, X, Y, Project.ScreenWidth, Project.ScreenHeight );
    }



    public override void Apply()
    {
      ChangedSection.DrawTo( Project.Image, X, Y );
      Editor.UpdateArea( 0, 0, Width, Height );
      Editor.SetModified();
    }

  }
}
