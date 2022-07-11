using System.Collections.Generic;
using RetroDevStudio.Formats;
using RetroDevStudio.Documents;



namespace RetroDevStudio.Undo
{
  public class UndoCharscreenCharsetChange : UndoTask
  {
    public CharsetScreenProject   Project = null;
    public CharsetScreenEditor    Editor = null;


    public List<CharData>         CharsetData = null;


    public UndoCharscreenCharsetChange( CharsetScreenProject Project, CharsetScreenEditor Editor )
    {
      this.Project = Project;
      this.Editor = Editor;

      CharsetData = new List<CharData>();
      for ( int i = 0; i < Project.CharSet.ExportNumCharacters; ++i )
      {
        var Char = new CharData();
        Char.Tile.Data = new GR.Memory.ByteBuffer( Project.CharSet.Characters[i].Tile.Data );
        Char.Tile.CustomColor = Project.CharSet.Characters[i].Tile.CustomColor;
        Char.Category = Project.CharSet.Characters[i].Category;
        Char.Index = i;

        CharsetData.Add( Char );
      }
    }




    public override string Description
    {
      get
      {
        return "Charset screen charset change";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoCharscreenCharsetChange( Project, Editor );
    }



    public override void Apply()
    {
      foreach ( var singleChar in CharsetData )
      {
        singleChar.Tile.Data.CopyTo( Project.CharSet.Characters[singleChar.Index].Tile.Data );
        Project.CharSet.Characters[singleChar.Index].Tile.CustomColor = singleChar.Tile.CustomColor;
        Project.CharSet.Characters[singleChar.Index].Category = singleChar.Category;
      }
      Editor.CharsetChanged();
      Editor.SetModified();
    }
  }
}
