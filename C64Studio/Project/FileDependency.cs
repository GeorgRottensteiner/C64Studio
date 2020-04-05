using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio
{
  public class FileDependency
  {
    public class DependencyInfo
    {
      // empty string means the same project (when saved/loaded)
      public string       Project = "";
      public string       Filename = "";
      public bool         Dependent = true;
      public bool         IncludeSymbols = false;



      public DependencyInfo( string Project, string Filename, bool Dependent, bool IncludeSymbols )
      {
        this.Project        = Project;
        this.Filename       = Filename;
        this.Dependent      = Dependent;
        this.IncludeSymbols = IncludeSymbols;
      }
    }



    public string             File = "";
    public List<DependencyInfo>       DependentOnFile = new List<DependencyInfo>();


    public bool DependsOn( string Project, string Filename )
    {
      foreach ( var entry in DependentOnFile )
      {
        if ( ( entry.Project == Project )
        &&   ( entry.Filename == Filename ) )
        {
          return entry.Dependent;
        }
      }
      return false;
    }



    public void RemoveDependency( string Project, string Filename )
    {
      for ( int i = 0; i < DependentOnFile.Count; ++i )
      {
        if ( ( DependentOnFile[i].Project == Project )
        &&   ( DependentOnFile[i].Filename == Filename ) )
        {
          DependentOnFile.RemoveAt( i );
          return;
        }
      }
    }



    public DependencyInfo FindDependency( string Project, string Filename )
    {
      foreach ( var entry in DependentOnFile )
      {
        if ( ( entry.Project == Project )
        &&   ( entry.Filename == Filename )
        &&   ( entry.Dependent ) )
        {
          return entry;
        }
      }
      return null;
    }

  }
}
