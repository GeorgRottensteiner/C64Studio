using System;
using System.Collections.Generic;
using System.Text;
using C64Studio.Formats;



namespace C64Studio.Undo
{
  public class UndoCharsetRemoveCategory : UndoTask
  {
    private CharsetEditor         Editor = null;
    private CharsetProject        Project = null;
    private int                   CategoryIndex = -1;
    private string                CategoryName = "";
    private List<int>             CharCategories = new List<int>();



    public UndoCharsetRemoveCategory( CharsetEditor Editor, CharsetProject Project, int CategoryIndex )
    {
      this.Editor = Editor;
      this.Project = Project;
      this.CategoryIndex = CategoryIndex;
      this.CategoryName = Project.Categories[CategoryIndex];

      foreach ( var charData in Project.Characters )
      {
        CharCategories.Add( charData.Category );
      }
    }




    public string Description
    {
      get
      {
        return "Charset Remove Category";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoCharsetAddCategory( Editor, Project, CategoryIndex );
    }



    public override void Apply()
    {
      int index = 0;
      foreach ( var charCategory in CharCategories )
      {
        Project.Characters[index].Category = charCategory;
        ++index;
      }
      Editor.AddCategory( CategoryIndex, CategoryName );
    }
  }
}
