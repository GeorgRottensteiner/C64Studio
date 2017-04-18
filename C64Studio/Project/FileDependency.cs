using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio
{
  public class FileDependency
  {
    public class DependencyInfo
    {
      public string       Filename = "";
      public bool         Dependent = true;
      public bool         IncludeSymbols = false;



      public DependencyInfo( string Filename, bool Dependent, bool IncludeSymbols )
      {
        this.Filename       = Filename;
        this.Dependent      = Dependent;
        this.IncludeSymbols = IncludeSymbols;
      }
    }



    public string             File = "";
    public List<DependencyInfo>       DependentOnFile = new List<DependencyInfo>();


    public bool DependsOn( string Filename )
    {
      foreach ( var entry in DependentOnFile )
      {
        if ( entry.Filename == Filename )
        {
          return entry.Dependent;
        }
      }
      return false;
    }



    public void RemoveDependency( string Filename )
    {
      for ( int i = 0; i < DependentOnFile.Count; ++i )
      {
        if ( DependentOnFile[i].Filename == Filename )
        {
          DependentOnFile.RemoveAt( i );
          return;
        }
      }
    }



    public DependencyInfo FindDependency( string Filename )
    {
      foreach ( var entry in DependentOnFile )
      {
        if ( ( entry.Filename == Filename )
        &&   ( entry.Dependent ) )
        {
          return entry;
        }
      }
      return null;
    }

  }
}
