using RetroDevStudio.Documents;
using RetroDevStudio.Formats;



namespace RetroDevStudio.Undo
{
  public class UndoCharscreenSizeChange : UndoTask
  {
    public CharsetScreenProject   Project = null;
    public CharsetScreenEditor    Editor = null;
    public int                    ScreenIndex = 0;  



    public int        Width = 0;
    public int        Height = 0;

    public GR.Game.Layer<uint>     ChangedData = new GR.Game.Layer<uint>();



    public UndoCharscreenSizeChange( CharsetScreenProject project, CharsetScreenEditor editor, int width, int height, int screenIndex )
    {
      Width         = width;
      Height        = height;
      Editor        = editor;
      Project       = project ;
      ScreenIndex   = screenIndex;

      ChangedData.Resize( Width, Height );

      for ( int i = 0; i < Width; ++i )
      {
        for ( int j = 0; j < Height; ++j )
        {
          ChangedData[i, j] = Project.Screens[screenIndex].Chars[i + j * Width];
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
      return new UndoCharscreenSizeChange( Project, 
                                           Editor, 
                                           Project.Screens[ScreenIndex].ScreenWidth, 
                                           Project.Screens[ScreenIndex].ScreenHeight, 
                                           ScreenIndex );
    }



    public override void Apply()
    {
      Editor.SetScreenSize( Width, Height, ScreenIndex );
      for ( int i = 0; i < Width; ++i )
      {
        for ( int j = 0; j < Height; ++j )
        {
          Project.Screens[ScreenIndex].Chars[i + j * Width] = ChangedData[i, j];
        }
      }
      Editor.UpdateArea( 0, 0, Width, Height );
      Editor.SetModified();
    }

  }
}
