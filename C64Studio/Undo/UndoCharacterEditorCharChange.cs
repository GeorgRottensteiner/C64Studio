using RetroDevStudio.Controls;
using RetroDevStudio.Formats;
using System.Collections.Generic;

namespace RetroDevStudio.Undo
{
  public class UndoCharacterEditorCharChange : UndoTask
  {
    public CharacterEditor        Editor = null;
    public CharsetProject         Project = null;
    public List<int>              ModifiedCharacterIndices = new List<int>();
    public List<CharData>         Chars = new List<CharData>();



    public UndoCharacterEditorCharChange( CharacterEditor Editor, CharsetProject Project, int modifiedChar )
    {
      Setup( Editor, Project, new List<int> { modifiedChar } );
    }



    public UndoCharacterEditorCharChange( CharacterEditor Editor, CharsetProject Project, List<int> modifiedChars )
    {
      Setup( Editor, Project, modifiedChars );
    }



    private void Setup( CharacterEditor Editor, CharsetProject Project, List<int> modifiedChars )
    {
      this.Editor = Editor;
      this.Project = Project;
      ModifiedCharacterIndices = modifiedChars;

      foreach ( var charIndex in ModifiedCharacterIndices )
      {
        var charData = new CharData();
        charData.Tile.Data        = new GR.Memory.ByteBuffer( Project.Characters[charIndex].Tile.Data );
        charData.Tile.CustomColor = Project.Characters[charIndex].Tile.CustomColor;
        charData.Category         = Project.Characters[charIndex].Category;
        charData.Index            = charIndex;

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
      return new UndoCharacterEditorCharChange( Editor, Project, ModifiedCharacterIndices );
    }



    public override void Apply()
    {
      for ( int i = 0; i < ModifiedCharacterIndices.Count; ++i )
      {
        var charIndex = ModifiedCharacterIndices[i];
        var charData = Chars[i];

        Project.Characters[charIndex].Tile.Data         = new GR.Memory.ByteBuffer( charData.Tile.Data );
        Project.Characters[charIndex].Tile.CustomColor  = charData.Tile.CustomColor;
        Project.Characters[charIndex].Category          = charData.Category;
        Project.Characters[charIndex].Index             = charData.Index;
      }
      Editor.CharacterChanged( ModifiedCharacterIndices );
    }
  }
}
