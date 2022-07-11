using System.Collections.Generic;
using RetroDevStudio.Documents;
using RetroDevStudio.Formats;
using RetroDevStudio.Types;



namespace RetroDevStudio.Undo
{
  public class UndoGraphicScreenValuesChange : UndoTask
  {
    public GraphicScreenProject   Project = null;
    public GraphicScreenEditor    Editor = null;

    private Dictionary<int, List<GraphicScreenProject.ColorMappingTarget>> _ColorMapping;
    private ColorSettings         _Colors;



    public UndoGraphicScreenValuesChange( GraphicScreenProject Project, GraphicScreenEditor Editor )
    {
      this.Project = Project;
      this.Editor = Editor;

      _ColorMapping = new Dictionary<int, List<GraphicScreenProject.ColorMappingTarget>>();
      foreach ( var entry in Project.ColorMapping )
      {
        var newList = new List<GraphicScreenProject.ColorMappingTarget>();
        newList.AddRange( entry.Value );

        _ColorMapping.Add( entry.Key, newList );
      }

      _Colors       = new ColorSettings( Project.Colors );
    }




    public override string Description
    {
      get
      {
        return "Graphic screen values change";
      }
    }



    public override UndoTask CreateComplementaryTask()
    {
      return new UndoGraphicScreenValuesChange( Project, Editor );
    }



    public override void Apply()
    {
      Project.ColorMapping = new Dictionary<int, List<GraphicScreenProject.ColorMappingTarget>>();
      foreach ( var entry in _ColorMapping )
      {
        var newList = new List<GraphicScreenProject.ColorMappingTarget>();
        newList.AddRange( entry.Value );

        Project.ColorMapping.Add( entry.Key, newList );
      }
      Project.Colors        = new ColorSettings( _Colors );
      Editor.ColorValuesChanged();
      Editor.SetModified();
    }

  }
}
