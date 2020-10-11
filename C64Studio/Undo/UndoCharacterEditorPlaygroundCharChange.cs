using System;
using System.Collections.Generic;
using System.Text;
using C64Studio.Controls;
using C64Studio.Formats;



namespace C64Studio.Undo
{
  public class UndoCharacterEditorPlaygroundCharChange : UndoTask
  {
    public CharacterEditor        Editor = null;
    public CharsetProject         Project = null;
    public int                    X = 0;
    public int                    Y = 0;
    public ushort                 Char = 0;



    public UndoCharacterEditorPlaygroundCharChange( CharacterEditor Editor, CharsetProject Project, int X, int Y )
    {
      this.Editor = Editor;
      this.Project = Project;
      this.X = X;
      this.Y = Y;

      Char = Project.PlaygroundChars[X + Y * 16];
    }




    public override string Description
    {
      get
      {
        return "Charset Playground Char Change";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoCharacterEditorPlaygroundCharChange( Editor, Project, X, Y );
    }



    public override void Apply()
    {
      Project.PlaygroundChars[X + Y * 16] = Char;

      Editor.PlaygroundCharacterChanged( X, Y );
    }
  }
}
