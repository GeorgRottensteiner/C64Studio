using System;
using System.Collections.Generic;
using System.Text;
using C64Studio.Formats;



namespace C64Studio.Undo
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
      for ( int i = 0; i < Project.CharSet.NumCharacters; ++i )
      {
        var Char = new CharData();
        Char.Data = new GR.Memory.ByteBuffer( Project.CharSet.Characters[i].Data );
        Char.Color = Project.CharSet.Characters[i].Color;
        Char.Category = Project.CharSet.Characters[i].Category;
        Char.Index = i;
        Char.Mode = Project.CharSet.Characters[i].Mode;

        CharsetData.Add( Char );
      }
    }




    public string Description
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
        singleChar.Data.CopyTo( Project.CharSet.Characters[singleChar.Index].Data );
        Project.CharSet.Characters[singleChar.Index].Color    = singleChar.Color;
        Project.CharSet.Characters[singleChar.Index].Category = singleChar.Category;
        Project.CharSet.Characters[singleChar.Index].Mode     = singleChar.Mode;
      }
      Editor.CharsetChanged();
      Editor.SetModified();
    }
  }
}
