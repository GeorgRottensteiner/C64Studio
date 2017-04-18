using C64Studio.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio
{
  public class Solution
  {
    public List<Project>    Projects = new List<Project>();
    public string           Name = "";
    public string           Filename = "";
    public bool             Modified = false;
    public MainForm         MainForm = null;



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



    public GR.Memory.ByteBuffer ToBuffer( string Filename )
    {
      this.Filename = Filename;
      GR.IO.FileChunk   chunkSolution = new GR.IO.FileChunk( Types.FileChunk.SOLUTION );

      GR.IO.FileChunk   chunkSolutionInfo = new GR.IO.FileChunk( Types.FileChunk.SOLUTION_INFO );
      if ( string.IsNullOrEmpty( Name ) )
      {
        Name = System.IO.Path.GetFileNameWithoutExtension( Filename );
      }
      chunkSolutionInfo.AppendString( Name );
      chunkSolutionInfo.AppendString( this.Filename );

      chunkSolution.Append( chunkSolutionInfo.ToBuffer() );

      foreach ( Project project in Projects )
      {
        GR.IO.FileChunk chunkSolutionProject = new GR.IO.FileChunk( Types.FileChunk.SOLUTION_PROJECT );
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
        if ( chunk.Type != Types.FileChunk.SOLUTION )
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
            case Types.FileChunk.SOLUTION_INFO:
              Name = memSubChunk.ReadString();
              Filename = memSubChunk.ReadString();
              Filename = FromFile;
              break;
            case Types.FileChunk.SOLUTION_PROJECT:
              {
                string filename = memSubChunk.ReadString();

                filename = GR.Path.Normalize( System.IO.Path.Combine( System.IO.Path.GetDirectoryName( Filename ), filename ), false );

                Project project = MainForm.OpenProject( filename );
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
      return true;
    }



    internal void RenameProject( Project Project, string NewName )
    {
      if ( Project == null )
      {
        return;
      }
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

      Element.DocumentInfo.Project.RemoveElement( Element );
      Element.Node.Remove();
      Modified = true;
    }



    internal void RenameElement( ProjectElement Element, string OldFilename, string NewFilename )
    {
      // adjust references
      foreach ( var project in Projects )
      {
        foreach ( var element in project.Elements )
        {
          foreach ( var perConfigSettings in element.Settings.Values )
          {
            foreach ( var buildChainEntry in perConfigSettings.PreBuildChain.Entries )
            {
              if ( ( buildChainEntry.ProjectName == Element.DocumentInfo.Project.Settings.Name )
              &&   ( System.IO.Path.GetFileName( buildChainEntry.DocumentFilename ).ToUpper() == System.IO.Path.GetFileName( OldFilename ).ToUpper() ) )
              {
                buildChainEntry.DocumentFilename = System.IO.Path.GetFileName( NewFilename );
              }
            }
            foreach ( var buildChainEntry in perConfigSettings.PostBuildChain.Entries )
            {
              if ( ( buildChainEntry.ProjectName == Element.DocumentInfo.Project.Settings.Name )
              &&   ( System.IO.Path.GetFileName( buildChainEntry.DocumentFilename ).ToUpper() == System.IO.Path.GetFileName( OldFilename ).ToUpper() ) )
              {
                buildChainEntry.DocumentFilename = System.IO.Path.GetFileName( NewFilename );
              }
            }
          }
        }
      }

      // set new values
      Element.Name = System.IO.Path.GetFileNameWithoutExtension( NewFilename );
      Element.DocumentInfo.DocumentFilename = System.IO.Path.GetFileName( NewFilename );
      if ( Element.Document != null )
      {
        Element.Document.SetDocumentFilename( System.IO.Path.GetFileName( NewFilename ) );
        Element.Filename = System.IO.Path.GetFileName( NewFilename );
        Element.Document.SetModified();
      }
      else
      {
        Element.Filename = System.IO.Path.GetFileName( NewFilename );
      }

    }
  }
}
