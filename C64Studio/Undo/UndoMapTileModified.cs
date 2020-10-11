using System;
using System.Collections.Generic;
using System.Text;
using C64Studio.Formats;



namespace C64Studio.Undo
{
  public class UndoMapTileModified : UndoTask
  {
    public MapEditor              MapEditor = null;
    private MapProject            Project = null;
    private MapProject.Tile       Tile = null;
    public int                    TileIndex = -1;



    public UndoMapTileModified( MapEditor Editor, MapProject Project, int TileIndex )
    {
      this.TileIndex = TileIndex;
      this.Project = Project;
      MapEditor = Editor;


      var Tile = Project.Tiles[TileIndex];

      this.Tile = new MapProject.Tile();
      this.Tile.Chars = new GR.Game.Layer<MapProject.TileChar>();
      this.Tile.Chars.Resize( Tile.Chars.Width, Tile.Chars.Height );
      for ( int i = 0; i < Tile.Chars.Width; ++i )
      {
        for ( int j = 0; j < Tile.Chars.Height; ++j )
        {
          this.Tile.Chars[i, j] = new MapProject.TileChar();
          this.Tile.Chars[i, j].Character = Tile.Chars[i, j].Character;
          this.Tile.Chars[i, j].Color     = Tile.Chars[i, j].Color;
        }
      }

      this.Tile.Name = Tile.Name;
    }




    public override string Description
    {
      get
      {
        return "Tile Change";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoMapTileModified( MapEditor, Project, TileIndex );
    }



    public override void Apply()
    {
      var newTile = Project.Tiles[TileIndex];

      newTile.Chars.Resize( Tile.Chars.Width, Tile.Chars.Height );
      for ( int i = 0; i < Tile.Chars.Width; ++i )
      {
        for ( int j = 0; j < Tile.Chars.Height; ++j )
        {
          newTile.Chars[i, j] = new MapProject.TileChar();
          newTile.Chars[i, j].Character = Tile.Chars[i, j].Character;
          newTile.Chars[i, j].Color = Tile.Chars[i, j].Color;
        }
      }

      newTile.Name = Tile.Name;
      MapEditor.TileModified( TileIndex );
    }
  }
}
