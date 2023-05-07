using System;
using System.Collections.Generic;
#if NET6_0_OR_GREATER
using LibGit2Sharp;
#endif

namespace SourceControl
{
  public class Controller
  {
    public static bool IsFunctional
    {
      get
      {
#if NET6_0_OR_GREATER
        return true;
#else
        return false;
#endif
      }
    }



    public static bool CreateRepositoryInFolder( string Folder )
    {
#if NET6_0_OR_GREATER
      try
      {
        //System.IO.Directory.CreateDirectory( System.IO.Path.Combine( Folder, ".git" ) );
        Repository.Init( System.IO.Path.Combine( Folder, ".git" ), true );
        return true;
      }
      catch ( Exception ) 
      {
        return false;
      }
#else
      return false;
#endif
    }



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
      catch ( Exception ) 
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



    public List<FileInfo> CurrentAddedFiles()
    {
      var files = new List<FileInfo>();
#if NET6_0_OR_GREATER
      if ( !_GITRepo.Info.IsBare )
      {
        foreach ( var item in _GITRepo.RetrieveStatus() )
        {
          files.Add( new FileInfo() { Filename = item.FilePath, FileState = (FileState)(int)item.State } );
        }
      }
#endif
      return files;
    }



  }
}