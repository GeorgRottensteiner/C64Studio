using System;
using System.Collections.Generic;
using System.Linq;
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
        Repository.Init( System.IO.Path.Combine( Folder, ".git" ), false );
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



    public bool HasChanges
    {
      get
      {
#if NET6_0_OR_GREATER
        return _GITRepo.Diff.Compare<TreeChanges>().Any();
#else
        return false;
#endif
      }
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
        foreach ( var item in _GITRepo.Index )
        {
          files.Add( new FileInfo() { Filename = item.Path, FileState = FileState.Unaltered } );
        }
      }
#endif
      return files;
    }



    public bool AddFileToIndex( string FullPath )
    {
#if NET6_0_OR_GREATER
      try
      {
        _GITRepo.Index.Add( FullPath );
        _GITRepo.Index.Write();
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



    public FileState GetFileState( string FullPath )
    {
#if NET6_0_OR_GREATER
      try
      {
        return (FileState)(int)_GITRepo.RetrieveStatus( FullPath );
      }
      catch ( Exception )
      {
        return FileState.Nonexistent;
      }
#else
      return FileState.Nonexistent;
#endif
    }



    public bool RemoveFileFromIndex( string FullPath )
    {
#if NET6_0_OR_GREATER
      try
      {
        _GITRepo.Index.Remove( FullPath );
        _GITRepo.Index.Write();
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



    public bool Ignore( string FullPath )
    {
#if NET6_0_OR_GREATER
      try
      {
        //_GITRepo.Ignore.AddTemporaryRules( new List<string>() { FullPath }  );
        //_GITRepo.Ignore.Write();
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



    public bool CommitChanges()
    {
#if NET6_0_OR_GREATER
      try
      {
        var author = new Signature( "ich", "email", DateTime.Now );
        var committer = author;

        var commit = _GITRepo.Commit( "Commit message", author, committer );

        return ( commit != null );
      }
      catch ( Exception )
      {
        return false;
      }
#else
      return false;
#endif
    }



  }
}