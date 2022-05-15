using System;
using System.Collections.Generic;
using System.Text;
using RetroDevStudio.Controls;
using RetroDevStudio.Formats;



namespace RetroDevStudio.Undo
{
  public class UndoCharacterEditorSwapCategories : UndoTask
  {
    private CharacterEditor       Editor = null;
    private CharsetProject        Project = null;
    private int                   CategoryIndex1 = -1;
    private int                   CategoryIndex2 = -1;



    public UndoCharacterEditorSwapCategories( CharacterEditor Editor, CharsetProject Project, int CategoryIndex1, int CategoryIndex2 )
    {
      this.Editor = Editor;
      this.Project = Project;
      this.CategoryIndex1 = CategoryIndex1;
      this.CategoryIndex2 = CategoryIndex2;
    }



    public override string Description
    {
      get
      {
        return "Charset Category Swap";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoCharacterEditorSwapCategories( Editor, Project, CategoryIndex1, CategoryIndex2 );
    }



    public override void Apply()
    {
      Editor.SwapCategories( CategoryIndex1, CategoryIndex2 );
      MarkParentAsModified();
    }
  }
}
