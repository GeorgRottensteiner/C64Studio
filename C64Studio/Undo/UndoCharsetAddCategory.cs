using System;
using System.Collections.Generic;
using System.Text;
using C64Studio.Controls;
using C64Studio.Formats;



namespace C64Studio.Undo
{
  public class UndoCharsetAddCategory : UndoTask
  {
    private CharacterEditor       Editor = null;
    private CharsetProject        Project = null;
    private int                   CategoryIndex = -1;
    private List<int>             CharCategories = new List<int>();



    public UndoCharsetAddCategory( CharacterEditor Editor, CharsetProject Project, int CategoryIndex )
    {
      this.Editor = Editor;
      this.Project = Project;
      this.CategoryIndex = CategoryIndex;

      foreach ( var charData in Project.Characters )
      {
        CharCategories.Add( charData.Category );
      }
    }



    public override string Description
    {
      get
      {
        return "Charset Category Add";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoCharsetRemoveCategory( Editor, Project, CategoryIndex );
    }



    public override void Apply()
    {
      int index = 0;
      foreach ( var charCategory in CharCategories )
      {
        Project.Characters[index].Category = charCategory;
        ++index;
      }
      Editor.RemoveCategory( CategoryIndex );
      MarkParentAsModified();
    }
  }
}
