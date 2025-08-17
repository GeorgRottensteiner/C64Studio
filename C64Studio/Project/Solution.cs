using RetroDevStudio.Types;
using RetroDevStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RetroDevStudio
{
  public class Solution
  {
    public List<Project>    Projects = new List<Project>();
    public string           Name = "";
    public string           Filename = "";
    public string           ActiveProject = "";
    public bool             Modified = false;
    public bool             DuringLoad = false;   // to avoid modification
    public MainForm         MainForm = null;
    public List<string>     ExpandedNodes = new List<string>();




    public Solution( MainForm Main )
    {
      MainForm = Main;
    }



    public bool FilenameUsed( string Filename )
    {
      foreach ( var project in Projects )
      {
        if ( project.GetElementByFilename( Filename ) != null )
        {
          return true;
        }
      }
      return false;
    }



    public bool FilenameUsed( string Filename, out Project Project )
    {
      Project = null;
      foreach ( var project in Projects )
      {
        if ( project.GetElementByFilename( Filename ) != null )
        {
          Project = project;
          return true;
        }
      }
      return false;
    }



    public GR.Memory.ByteBuffer ToBuffer()
    {
      GR.IO.FileChunk   chunkSolution = new GR.IO.FileChunk( FileChunkConstants.SOLUTION );

      GR.IO.FileChunk   chunkSolutionInfo = new GR.IO.FileChunk( FileChunkConstants.SOLUTION_INFO );
      if ( string.IsNullOrEmpty( Name ) )
      {
        Name = GR.Path.GetFileNameWithoutExtension( Filename );
      }
      chunkSolutionInfo.AppendString( Name );
      chunkSolutionInfo.AppendString( this.Filename );
      chunkSolutionInfo.AppendString( ActiveProject );

      chunkSolution.Append( chunkSolutionInfo.ToBuffer() );

      GR.IO.FileChunk   chunkExpandedNodes = new GR.IO.FileChunk( FileChunkConstants.SOLUTION_NODES );
      foreach ( string key in ExpandedNodes )
      {
        chunkExpandedNodes.AppendString( key );
      }
      chunkSolution.Append( chunkExpandedNodes.ToBuffer() );

      foreach ( Project project in Projects )
      {
        GR.IO.FileChunk chunkSolutionProject = new GR.IO.FileChunk( FileChunkConstants.SOLUTION_PROJECT );
        chunkSolutionProject.AppendString( GR.Path.RelativePathTo( Filename, false, project.Settings.Filename, false ) );

        chunkSolution.Append( chunkSolutionProject.ToBuffer() );
      }

      return chunkSolution.ToBuffer();
    }



    public bool FromBuffer( GR.Memory.ByteBuffer SolutionData, string FromFile )
    {
      GR.IO.MemoryReader memIn = new GR.IO.MemoryReader( SolutionData );

      GR.IO.FileChunk           chunk = new GR.IO.FileChunk();

      while ( chunk.ReadFromStream( memIn ) )
      {
        if ( chunk.Type != FileChunkConstants.SOLUTION )
        {
          return false;
        }
        GR.IO.MemoryReader memChunk = chunk.MemoryReader();

        GR.IO.FileChunk subChunk = new GR.IO.FileChunk();

        while ( subChunk.ReadFromStream( memChunk ) )
        {
          GR.IO.MemoryReader memSubChunk = subChunk.MemoryReader();

          switch ( subChunk.Type )
          {
            case FileChunkConstants.SOLUTION_INFO:
              Name = memSubChunk.ReadString();
              Filename = memSubChunk.ReadString();
              ActiveProject = memSubChunk.ReadString();

              Filename = FromFile;
              Name = GR.Path.GetFileNameWithoutExtension( FromFile );
              break;
            case FileChunkConstants.SOLUTION_PROJECT:
              {
                string filename = memSubChunk.ReadString();

                filename = GR.Path.Normalize( GR.Path.Append( GR.Path.GetDirectoryName( Filename ), filename ), false );

                Project project = MainForm.OpenProject( filename );
              }
              break;
            case FileChunkConstants.SOLUTION_NODES:
              while ( memSubChunk.DataAvailable )
              {
                string  node = memSubChunk.ReadString();

                if ( !ExpandedNodes.Contains( node ) )
                {
                  ExpandedNodes.Add( node );
                }
              }
              break;
          }
        }
      }
      return true;
    }



    internal Project GetProjectByName( string ProjectName )
    {
      foreach ( var project in Projects )
      {
        if ( project.Settings.Name.ToUpper() == ProjectName.ToUpper() )
        {
          return project;
        }
      }
      return null;
    }



    internal Project GetProjectByFilename( string Filename )
    {
      foreach ( var project in Projects )
      {
        if ( GR.Path.IsPathEqual( project.Settings.Filename, Filename ) )
        {
          return project;
        }
      }
      return null;
    }



    internal bool IsValidProjectName( string NewName )
    {
      if ( string.IsNullOrEmpty( NewName ) )
      {
        return false;
      }
      // name already in use!
      if ( GetProjectByName( NewName ) != null )
      {
        return false;
      }
      var invalidChars = System.IO.Path.GetInvalidFileNameChars();
      foreach ( char c in NewName )
      {
        for ( int i = 0; i < invalidChars.Length; ++i )
        {
          if ( c == invalidChars[i] )
          {
            return false;
          }
        }
      }
      return true;
    }



    internal void RenameProject( Project Project, string NewName )
    {
      if ( Project == null )
      {
        return;
      }

      var oldProjectFilename = Project.Settings.Filename;
      var newProjectFilename = GR.Path.RenameFilenameWithoutExtension( oldProjectFilename, NewName );
      if ( System.IO.File.Exists( newProjectFilename ) )
      {
        MainForm.StudioCore.Notification.MessageBox( "Cannot rename project", $"The new project filename '{newProjectFilename}' already exists!" );
        return;
      }
      System.IO.File.Move( oldProjectFilename, newProjectFilename );
      Project.Settings.Filename = newProjectFilename;

      // adjust references
      foreach ( var project in Projects )
      {
        foreach ( var element in project.Elements )
        {
          foreach ( var perConfigSettings in element.Settings.Values )
          {
            foreach ( var buildChainEntry in perConfigSettings.PreBuildChain.Entries )
            {
              if ( buildChainEntry.ProjectName == Project.Settings.Name )
              {
                buildChainEntry.ProjectName = NewName;
              }
            }
            foreach ( var buildChainEntry in perConfigSettings.PostBuildChain.Entries )
            {
              if ( buildChainEntry.ProjectName == Project.Settings.Name )
              {
                buildChainEntry.ProjectName = NewName;
              }
            }
          }
        }
      }

      Project.Settings.Name = NewName;
      if ( Project.Node != null )
      {
        Project.Node.Text = NewName;
      }
    }



    internal void RemoveProject( Project ProjectToClose )
    {
      // adjust references
      foreach ( var project in Projects )
      {
        if ( project != ProjectToClose )
        {
          foreach ( var element in project.Elements )
          {
            foreach ( var perConfigSettings in element.Settings.Values )
            {
              var  entriesToRemove = new List<BuildChainEntry>();
              foreach ( var buildChainEntry in perConfigSettings.PreBuildChain.Entries )
              {
                if ( buildChainEntry.ProjectName == ProjectToClose.Settings.Name )
                {
                  entriesToRemove.Add( buildChainEntry );
                }
              }
              foreach ( var entryToRemove in entriesToRemove )
              {
                perConfigSettings.PreBuildChain.Entries.Remove( entryToRemove );
              }

              entriesToRemove.Clear();
              foreach ( var buildChainEntry in perConfigSettings.PostBuildChain.Entries )
              {
                if ( buildChainEntry.ProjectName == ProjectToClose.Settings.Name )
                {
                  entriesToRemove.Add( buildChainEntry );
                }
              }
              foreach ( var entryToRemove in entriesToRemove )
              {
                perConfigSettings.PostBuildChain.Entries.Remove( entryToRemove );
              }
            }
          }
        }
      }


      Projects.Remove( ProjectToClose );
    }



    internal void RemoveElement( ProjectElement Element )
    {
      // adjust references
      foreach ( var project in Projects )
      {
        foreach ( var element in project.Elements )
        {
          foreach ( var perConfigSettings in element.Settings.Values )
          {
            var  entriesToRemove = new List<BuildChainEntry>();
            foreach ( var buildChainEntry in perConfigSettings.PreBuildChain.Entries )
            {
              if ( buildChainEntry.ProjectName == Element.DocumentInfo.Project.Settings.Name )
              {
                entriesToRemove.Add( buildChainEntry );
              }
            }
            foreach ( var entryToRemove in entriesToRemove )
            {
              perConfigSettings.PreBuildChain.Entries.Remove( entryToRemove );
            }

            entriesToRemove.Clear();
            foreach ( var buildChainEntry in perConfigSettings.PostBuildChain.Entries )
            {
              if ( buildChainEntry.ProjectName == Element.DocumentInfo.Project.Settings.Name )
              {
                entriesToRemove.Add( buildChainEntry );
              }
            }
            foreach ( var entryToRemove in entriesToRemove )
            {
              perConfigSettings.PostBuildChain.Entries.Remove( entryToRemove );
            }
          }
        }
      }

      if ( Element != null )
      {
        Element.DocumentInfo.Project.RemoveElement( Element );
        Element.Node.Remove();
      }
      Modified = true;
    }



    internal void RenameElement( ProjectElement Element, string OldFilename, string NewFilename )
    {
      if ( Element != null )
      {
        // adjust references
        foreach ( var project in Projects )
        {
          foreach ( var element in project.Elements )
          {
            foreach ( var dependency in element.ForcedDependency.DependentOnFile )
            {
              var depProject = GetProjectByName( dependency.Project );
              string  fullPath = depProject.FullPath( dependency.Filename );
              if ( GR.Path.IsPathEqual( fullPath, OldFilename ) )
              {
                dependency.Filename = GR.Path.GetFileName( NewFilename );
              }
            }
            foreach ( var perConfigSettings in element.Settings.Values )
            {
              foreach ( var buildChainEntry in perConfigSettings.PreBuildChain.Entries )
              {
                if ( ( Element != null )
                &&   ( buildChainEntry.ProjectName == Element.DocumentInfo.Project.Settings.Name )
                &&   ( GR.Path.GetFileName( buildChainEntry.DocumentFilename ).ToUpper() == GR.Path.GetFileName( OldFilename ).ToUpper() ) )
                {
                  buildChainEntry.DocumentFilename = GR.Path.GetFileName( NewFilename );
                }
              }
              foreach ( var buildChainEntry in perConfigSettings.PostBuildChain.Entries )
              {
                if ( ( Element != null )
                &&   ( buildChainEntry.ProjectName == Element.DocumentInfo.Project.Settings.Name )
                &&   ( GR.Path.GetFileName( buildChainEntry.DocumentFilename ).ToUpper() == GR.Path.GetFileName( OldFilename ).ToUpper() ) )
                {
                  buildChainEntry.DocumentFilename = GR.Path.GetFileName( NewFilename );
                }
              }
            }
          }
        }

        // set new values
        Element.Name = GR.Path.GetFileNameWithoutExtension( NewFilename );

        string    newPath = GR.Path.RenameFile( Element.DocumentInfo.DocumentFilename, GR.Path.GetFileName( NewFilename ) );

        Element.DocumentInfo.DocumentFilename = newPath;
        if ( Element.Document != null )
        {
          Element.Document.SetDocumentFilename( newPath );
          Element.Filename = newPath;
          Element.Document.SetModified();
        }
        else
        {
          // keep path intact!
          Element.Filename = GR.Path.RelativePathTo( Element.DocumentInfo.Project.Settings.BasePath, true, NewFilename, false );
        }
      }
    }



    internal void ExpandNode( Project Project )
    {
      if ( DuringLoad )
      {
        return;
      }

      string key = "Project*" + Project.Settings.Name;

      if ( !ExpandedNodes.Contains( key ) )
      {
        ExpandedNodes.Add( key );
        Modified = true;
      }
    }



    internal void ExpandNode( ProjectElement Element )
    {
      if ( DuringLoad )
      {
        return;
      }

      string key = "Element*" + Element.DocumentInfo.Project.Settings.Name + "*" + Element.Name;

      if ( !ExpandedNodes.Contains( key ) )
      {
        ExpandedNodes.Add( key );
        Modified = true;
      }
    }



    internal void CollapseNode( Project Project )
    {
      if ( DuringLoad )
      {
        return;
      }

      string key = "Project*" + Project.Settings.Name;

      if ( ExpandedNodes.Contains( key ) )
      {
        ExpandedNodes.Remove( key );
        Modified = true;
      }
    }



    internal void CollapseNode( ProjectElement Element )
    {
      if ( DuringLoad )
      {
        return;
      }

      string key = "Element*" + Element.DocumentInfo.Project.Settings.Name + "*" + Element.Name;

      if ( ExpandedNodes.Contains( key ) )
      {
        ExpandedNodes.Remove( key );
        Modified = true;
      }
    }



    internal bool IsNodeExpanded( ProjectElement Element )
    {
      string key = "Element*" + Element.DocumentInfo.Project.Settings.Name + "*" + Element.Name;

      return ExpandedNodes.Contains( key );
    }



    internal bool IsNodeExpanded( Project Project )
    {
      string key = "Project*" + Project.Settings.Name;

      return ExpandedNodes.Contains( key );
    }



  }
}
