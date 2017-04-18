using System;
using System.Collections.Generic;
using System.Text;
using C64Studio.Formats;



namespace C64Studio.Undo
{
  public class UndoCharscreenCharChange : UndoTask
  {
    public CharsetScreenProject   Project = null;
    public CharsetScreenEditor    Editor = null;



    public int        X = 0;
    public int        Y = 0;
    public int        Width = 0;
    public int        Height = 0;

    public GR.Game.Layer<ushort>     ChangedData = new GR.Game.Layer<ushort>();



    public UndoCharscreenCharChange( CharsetScreenProject Project, CharsetScreenEditor Editor, int X, int Y, int Width, int Height )
    {
      this.X = X;
      this.Y = Y;
      this.Width = Width;
      this.Height = Height;
      this.Editor = Editor;
      this.Project = Project;

      ChangedData.Resize( Width, Height );

      for ( int i = 0; i < Width; ++i )
      {
        for ( int j = 0; j < Height; ++j )
        {
          ChangedData[i, j] = Project.Chars[X + i + ( Y + j ) * Project.ScreenWidth];
        }
      }
    }




    public string Description
    {
      get
      {
        return "Charset screen change";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoCharscreenCharChange( Project, Editor, X, Y, Width, Height );
    }



    public override void Apply()
    {
      for ( int i = 0; i < Width; ++i )
      {
        for ( int j = 0; j < Height; ++j )
        {
          Project.Chars[X + i + ( Y + j ) * Project.ScreenWidth] = ChangedData[i, j];
        }
      }
      Editor.UpdateArea( X, Y, Width, Height );
      Editor.SetModified();
    }
  }
}
