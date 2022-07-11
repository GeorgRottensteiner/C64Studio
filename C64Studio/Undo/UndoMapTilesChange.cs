using RetroDevStudio.Documents;
using RetroDevStudio.Formats;



namespace RetroDevStudio.Undo
{
  public class UndoMapTilesChange : UndoTask
  {
    public MapProject.Map         AffectedMap = null;
    public MapEditor              MapEditor = null;



    public int        X = 0;
    public int        Y = 0;
    public int        Width = 0;
    public int        Height = 0;

    public GR.Game.Layer<int>     ChangedData = new GR.Game.Layer<int>();



    public UndoMapTilesChange( MapEditor Editor, MapProject.Map Map, int X, int Y, int Width, int Height )
    {
      this.X = X;
      this.Y = Y;
      this.Width = Width;
      this.Height = Height;
      MapEditor = Editor;
      AffectedMap = Map;
      ChangedData.Resize( Width, Height );

      for ( int i = 0; i < Width; ++i )
      {
        for ( int j = 0; j < Height; ++j )
        {
          ChangedData[i, j] = Map.Tiles[X + i, Y + j];
        }
      }
    }




    public override string Description
    {
      get
      {
        return "Map Change";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoMapTilesChange( MapEditor, AffectedMap, X, Y, Width, Height );
    }



    public override void Apply()
    {
      for ( int i = 0; i < Width; ++i )
      {
        for ( int j = 0; j < Height; ++j )
        {
          AffectedMap.Tiles[X + i, Y + j] = ChangedData[i, j];
        }
      }
      MapEditor.UpdateArea( X, Y, Width, Height );
    }
  }
}
