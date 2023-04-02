using System;
using System.Collections.Generic;
#if NET6_0_OR_GREATER
using LibGit2Sharp;
#endif

namespace SourceControl
{
  public class Controller
  {
    public static bool IsFolderUnderSourceControl( string Folder )
    {
#if NET6_0_OR_GREATER
      try
      {
        using ( var repo = new Repository( Folder ) )
        {
        }
        return true;
      }
      catch ( Exception ex ) 
      {
        return false;
      }
#else
      return false;
#endif
    }



#if NET6_0_OR_GREATER
    private Repository    _GITRepo = null;
#endif



    public Controller( string Folder )
    {
#if NET6_0_OR_GREATER
      try
      {
        _GITRepo = new Repository( Folder );
      }
      catch ( Exception )
      {
      }
#endif
    }



    public List<string> CurrentAddedFiles()
    {
      var files = new List<string>();
#if NET6_0_OR_GREATER
      foreach ( var item in _GITRepo.RetrieveStatus() ) 
      {
        files.Add( item.FilePath + " - " + item.State.ToString() );
      }
#endif
      return files;
    }



  }
}