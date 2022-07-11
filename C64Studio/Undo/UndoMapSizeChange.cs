using RetroDevStudio.Documents;
using RetroDevStudio.Formats;



namespace RetroDevStudio.Undo
{
  public class UndoMapSizeChange : UndoTask
  {
    public MapProject.Map         AffectedMap = null;
    public MapEditor              MapEditor = null;



    public int        Width = 0;
    public int        Height = 0;

    public GR.Game.Layer<int>     ChangedData = new GR.Game.Layer<int>();



    public UndoMapSizeChange( MapEditor Editor, MapProject.Map Map, int Width, int Height )
    {
      this.Width = Width;
      this.Height = Height;
      MapEditor = Editor;
      AffectedMap = Map;

      ChangedData.Resize( Width, Height );

      for ( int i = 0; i < Width; ++i )
      {
        for ( int j = 0; j < Height; ++j )
        {
          ChangedData[i, j] = Map.Tiles[i,j];
        }
      }
    }




    public override string Description
    {
      get
      {
        return "Map Size Change";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoMapSizeChange( MapEditor, AffectedMap, AffectedMap.Tiles.Width, AffectedMap.Tiles.Height );
    }



    public override void Apply()
    {
      AffectedMap.Tiles.Resize( Width, Height );
      for ( int i = 0; i < Width; ++i )
      {
        for ( int j = 0; j < Height; ++j )
        {
          AffectedMap.Tiles[i, j] = ChangedData[i, j];
        }
      }
      MapEditor.UpdateArea( 0, 0, Width, Height );
      MapEditor.InvalidateCurrentMap();
    }
  }
}
