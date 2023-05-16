using System.Collections.Generic;
using RetroDevStudio.Formats;
using RetroDevStudio.Documents;



namespace RetroDevStudio.Undo
{
  public class UndoMapCharsetChange : UndoTask
  {
    public MapProject             Project = null;
    public MapEditor              Editor = null;


    public List<CharData>         CharsetData = null;


    public UndoMapCharsetChange( MapProject Project, MapEditor Editor )
    {
      this.Project = Project;
      this.Editor = Editor;

      CharsetData = new List<CharData>();
      for ( int i = 0; i < Project.Charset.ExportNumCharacters; ++i )
      {
        var Char = new CharData();
        Char.Tile.Data        = new GR.Memory.ByteBuffer( Project.Charset.Characters[i].Tile.Data );
        Char.Tile.CustomColor = Project.Charset.Characters[i].Tile.CustomColor;
        Char.Category         = Project.Charset.Characters[i].Category;
        Char.Index            = i;

        CharsetData.Add( Char );
      }
    }




    public override string Description
    {
      get
      {
        return "Map charset change";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoMapCharsetChange( Project, Editor );
    }



    public override void Apply()
    {
      foreach ( var singleChar in CharsetData )
      {
        singleChar.Tile.Data.CopyTo( Project.Charset.Characters[singleChar.Index].Tile.Data );
        Project.Charset.Characters[singleChar.Index].Tile.CustomColor = singleChar.Tile.CustomColor;
        Project.Charset.Characters[singleChar.Index].Category = singleChar.Category;
      }
      Editor.CharsetChanged();
      Editor.SetModified();
    }
  }
}
