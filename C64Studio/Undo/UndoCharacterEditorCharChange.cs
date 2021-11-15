using C64Studio.Controls;
using C64Studio.Formats;
using System.Collections.Generic;

namespace C64Studio.Undo
{
  public class UndoCharacterEditorCharChange : UndoTask
  {
    public CharacterEditor        Editor = null;
    public CharsetProject         Project = null;
    public int                    CharIndex = 0;
    public int                    CharCount = 0;
    public List<CharData>         Chars = new List<CharData>();



    public UndoCharacterEditorCharChange( CharacterEditor Editor, CharsetProject Project, int CharIndex, int Count )
    {
      this.Editor = Editor;
      this.Project = Project;
      this.CharIndex = CharIndex;
      this.CharCount = Count;

      for ( int i = 0; i < Count; ++i )
      {
        var charData = new CharData();
        charData.Tile.Data        = new GR.Memory.ByteBuffer( Project.Characters[CharIndex + i].Tile.Data );
        charData.Tile.CustomColor = Project.Characters[CharIndex + i].Tile.CustomColor;
        charData.Category         = Project.Characters[CharIndex + i].Category;
        charData.Index            = CharIndex + i;

        Chars.Add( charData );
      }
    }



    public override string Description
    {
      get
      {
        return "CharacterEditor Char Change";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoCharacterEditorCharChange( Editor, Project, CharIndex, CharCount );
    }



    public override void Apply()
    {
      for ( int i = 0; i < CharCount; ++i )
      {
        var charData = Chars[i];

        Project.Characters[CharIndex].Tile.Data = new GR.Memory.ByteBuffer( charData.Tile.Data );
        Project.Characters[CharIndex].Tile.CustomColor = charData.Tile.CustomColor;
        Project.Characters[CharIndex].Category = charData.Category;
        Project.Characters[CharIndex].Index = charData.Index;
      }
      Editor.CharacterChanged( CharIndex, CharCount );
    }
  }
}
