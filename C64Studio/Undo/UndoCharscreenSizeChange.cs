using RetroDevStudio.Documents;
using RetroDevStudio.Formats;



namespace RetroDevStudio.Undo
{
  public class UndoCharscreenSizeChange : UndoTask
  {
    public CharsetScreenProject   Project = null;
    public CharsetScreenEditor    Editor = null;



    public int        Width = 0;
    public int        Height = 0;

    public GR.Game.Layer<uint>     ChangedData = new GR.Game.Layer<uint>();



    public UndoCharscreenSizeChange( CharsetScreenProject Project, CharsetScreenEditor Editor, int Width, int Height )
    {
      this.Width = Width;
      this.Height = Height;
      this.Editor = Editor;
      this.Project = Project;

      ChangedData.Resize( Width, Height );

      for ( int i = 0; i < Width; ++i )
      {
        for ( int j = 0; j < Height; ++j )
        {
          ChangedData[i, j] = Project.Chars[i + j * Width];
        }
      }
    }




    public override string Description
    {
      get
      {
        return "Charset screen size change";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoCharscreenSizeChange( Project, Editor, Project.ScreenWidth, Project.ScreenHeight );
    }



    public override void Apply()
    {
      Editor.SetScreenSize( Width, Height );
      for ( int i = 0; i < Width; ++i )
      {
        for ( int j = 0; j < Height; ++j )
        {
          Project.Chars[i + j * Width] = ChangedData[i, j];
        }
      }
      Editor.UpdateArea( 0, 0, Width, Height );
      Editor.SetModified();
    }

  }
}
