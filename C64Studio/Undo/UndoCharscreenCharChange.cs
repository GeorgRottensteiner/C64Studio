using RetroDevStudio.Documents;
using RetroDevStudio.Formats;



namespace RetroDevStudio.Undo
{
  public class UndoCharscreenCharChange : UndoTask
  {
    public CharsetScreenProject   Project = null;
    public CharsetScreenEditor    Editor = null;



    public int        X = 0;
    public int        Y = 0;
    public int        Width = 0;
    public int        Height = 0;
    public int        ScreenIndex = 0; 

    public GR.Game.Layer<uint>     ChangedData = new GR.Game.Layer<uint>();



    public UndoCharscreenCharChange( CharsetScreenProject Project, CharsetScreenEditor Editor, int screenIndex, int X, int Y, int Width, int Height )
    {
      this.X = X;
      this.Y = Y;
      this.Width = Width;
      this.Height = Height;
      this.Editor = Editor;
      this.Project = Project;
      ScreenIndex = screenIndex;

      var screen = Project.Screens[ScreenIndex];

      ChangedData.Resize( Width, Height );

      for ( int i = 0; i < Width; ++i )
      {
        for ( int j = 0; j < Height; ++j )
        {
          ChangedData[i, j] = screen.Chars[X + i + ( Y + j ) * screen.Width];
        }
      }
    }



    public UndoCharscreenCharChange( CharsetScreenProject Project, CharsetScreenEditor Editor, int screenIndex, System.Drawing.Rectangle Area )
    {
      X           = Area.X;
      Y           = Area.Y;
      Width       = Area.Width;
      Height      = Area.Height;
      ScreenIndex = screenIndex;
      this.Editor = Editor;
      this.Project = Project;

      var screen = Project.Screens[ScreenIndex];

      ChangedData.Resize( Width, Height );

      for ( int i = 0; i < Width; ++i )
      {
        for ( int j = 0; j < Height; ++j )
        {
          ChangedData[i, j] = screen.Chars[X + i + ( Y + j ) * screen.Width];
        }
      }
    }



    public override string Description
    {
      get
      {
        return "Charset screen change";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoCharscreenCharChange( Project, Editor, ScreenIndex, X, Y, Width, Height );
    }



    public override void Apply()
    {
      var screen = Project.Screens[ScreenIndex];
      for ( int i = 0; i < Width; ++i )
      {
        for ( int j = 0; j < Height; ++j )
        {
          screen.Chars[X + i + ( Y + j ) * screen.Width] = ChangedData[i, j];
        }
      }
      Editor.UpdateArea( X, Y, Width, Height );
      Editor.SetModified();
    }
  }
}
