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
        Repository.Init( System.IO.Path.Combine( Folder, ".git" ), false );

        using ( var repo = new Repository( Folder ) )
        {
          repo.CreateBranch( "master" );
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
        var state = _GITRepo.RetrieveStatus();

        return state.Any();
        /*
        return state.Added.Any() || state.Removed.Any() || state.Modified.Any() || state.RenamedInIndex.Any() || state.RenamedInWorkDir.Any();

        state.Any(

        bool hadAdded = _GITRepo.Diff.Compare<TreeChanges>().Added.Any();
        bool hadModified = _GITRepo.Diff.Compare<TreeChanges>().Modified.Any();
        bool hadDeleted = _GITRepo.Diff.Compare<TreeChanges>().Deleted.Any();
        bool hadRenamed = _GITRepo.Diff.Compare<TreeChanges>().Renamed.Any();
        return _GITRepo.Diff.Compare<TreeChanges>().Any();*/
#else
        return false;
#endif
      }
    }



    public List<FileInfo> GetCurrentRepositoryState()
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
          if ( !files.Any( f => f.Filename == item.Path ) )
          {
            files.Add( new FileInfo() { Filename = item.Path, FileState = FileState.Unaltered } );
          }
        }
      }
#endif
      return files;
    }



    public bool AddFileToRepository( string FullPath )
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



    public bool StageChanges( IEnumerable<string> Files )
    {
#if NET6_0_OR_GREATER
      try
      {
        foreach ( var filePath in Files )
        {
          _GITRepo.Index.Add( filePath );
        }
        _GITRepo.Index.Write();
        return true;
      }
      catch ( Exception ex )
      {
        Console.WriteLine( "Exception:RepoActions:StageChanges " + ex.Message );
      }
#endif
      return false;
    }



    public bool StageAllChanges()
    {
#if NET6_0_OR_GREATER
      try
      {
        RepositoryStatus status = _GITRepo.RetrieveStatus();
        List<string> filePaths = status.Modified.Select(mods => mods.FilePath).ToList();

        foreach ( var filePath in filePaths ) 
        { 
          _GITRepo.Index.Add( filePath ); 
        }
        _GITRepo.Index.Write();
        return true;
      }
      catch ( Exception ex )
      {
        Console.WriteLine( "Exception:RepoActions:StageAllChanges " + ex.Message );
      }
#endif
      return false;
    }



    public bool CommitAllChanges( string Author, string Email, string CommitMessage )
    {
#if NET6_0_OR_GREATER
      try
      {
        var author = new Signature( Author, Email, DateTime.Now );
        var committer = author;
        var commit = _GITRepo.Commit( CommitMessage, author, committer );

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



    public bool RevertChanges( IEnumerable<string> Files )
    {
#if NET6_0_OR_GREATER
      try
      {
        foreach ( var filePath in Files )
        {
          var state = _GITRepo.RetrieveStatus( filePath );
          /*
           * 
If your file is already staged (happens when you do a git add etc after the file is edited) to unstage your changes.

Use

git reset HEAD <file>
Then

git checkout <file>
If not already staged, just use

git checkout <file>*/

          //_GITRepo.CheckoutPaths( "", filePath, new CheckoutOptions(){ CheckoutModifiers.
          //_GITRepo.Revert(.Index.Add( filePath );
        }
        //_GITRepo.Index.Write();
        return true;
      }
      catch ( Exception ex )
      {
        Console.WriteLine( "Exception:RepoActions:RevertChanges " + ex.Message );
      }
#endif
      return false;
    }




  }
}