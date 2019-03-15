using C64Studio.Types;
using System;
using System.Collections.Generic;
using System.Text;
using WeifenLuo.WinFormsUI.Docking;

namespace C64Studio
{
  public class Compiling
  {
    public StudioCore                 Core = null;

    public bool                       m_BuildIsCurrent = false;

    public Types.BuildInfo            m_LastBuildInfo = null;

    public Stack<BuildChain>          m_BuildChainStack = new Stack<BuildChain>();

    public GR.Collections.Set<string> m_RebuiltFiles = new GR.Collections.Set<string>();
    public GR.Collections.Set<string> m_RebuiltBuildConfigFiles = new GR.Collections.Set<string>();


    public Parser.ASMFileParser       ParserASM = new C64Studio.Parser.ASMFileParser();
    public Parser.BasicFileParser     ParserBasic = new C64Studio.Parser.BasicFileParser();



    public Compiling( StudioCore Core )
    {
      this.Core = Core;
    }



    private string BuildFullPath( string ParentPath, string SubFilename )
    {
      if ( System.IO.Path.IsPathRooted( SubFilename ) )
      {
        return SubFilename;
      }
      return GR.Path.Append( ParentPath, SubFilename );
    }



    public bool NeedsRebuild( DocumentInfo DocInfo, string ConfigSetting )
    {
      if ( DocInfo == null )
      {
        return false;
      }
      // actual parsing and deducing dependencies if a rebuild is necessary!
      foreach ( IDockContent dockContent in Core.MainForm.panelMain.Documents )
      {
        BaseDocument baseDoc = (BaseDocument)dockContent;

        if ( baseDoc.Modified )
        {
          return true;
        }
      }

      if ( DocInfo.Element != null )
      {
        foreach ( var dependency in DocInfo.Element.ForcedDependency.DependentOnFile )
        {
          ProjectElement elementDependency = DocInfo.Project.GetElementByFilename( dependency.Filename );
          if ( elementDependency == null )
          {
            Core.AddToOutput( "Could not find dependency for " + dependency + System.Environment.NewLine );
            return true;
          }
          if ( NeedsRebuild( elementDependency.DocumentInfo, ConfigSetting ) )
          {
            return true;
          }
          foreach ( var rebuildFile in m_RebuiltFiles )
          {
            if ( GR.Path.IsPathEqual( elementDependency.DocumentInfo.DocumentFilename, rebuildFile ) )
            {
              Core.AddToOutput( "Dependency " + elementDependency.DocumentInfo.DocumentFilename + " was rebuilt in this cycle, need to rebuild dependent element " + DocInfo.DocumentFilename + System.Environment.NewLine );
              return true;
            }
          }
        }
        if ( DocInfo.DeducedDependency[ConfigSetting] != null )
        {
          foreach ( var dependency in DocInfo.Element.ExternalDependencies.DependentOnFile )
          {
            string      fullPath = BuildFullPath( DocInfo.Project.Settings.BasePath, dependency.Filename );

            DateTime    fileTime = new DateTime();

            try
            {
              fileTime = System.IO.File.GetLastWriteTime( fullPath );
            }
            catch
            {
            }

            if ( fileTime != DocInfo.DeducedDependency[ConfigSetting].BuildState[fullPath] )
            {
              Core.AddToOutput( "External Dependency " + fullPath + " was modified, need to rebuild dependent element " + DocInfo.DocumentFilename + System.Environment.NewLine );

              DocInfo.DeducedDependency[ConfigSetting].BuildState.Add( fullPath, fileTime );
              return true;
            }
          }
        }
        else
        {
          // no build time stored yet, needs rebuild
          DocInfo.DeducedDependency[ConfigSetting] = new DependencyBuildState();
          return true;
        }
      }
      if ( DocInfo.Compilable )
      {
        if ( !DocInfo.HasBeenSuccessfullyBuilt )
        {
          return true;
        }
      }
      if ( DocInfo.Project == null )
      {
        return true;
      }
      if ( DocInfo.DeducedDependency[ConfigSetting] == null )
      {
        // no build time stored yet, needs rebuild
        DocInfo.DeducedDependency[ConfigSetting] = new DependencyBuildState();
        return true;
      }
      foreach ( KeyValuePair<string, DateTime> dependency in DocInfo.DeducedDependency[ConfigSetting].BuildState )
      {
        DateTime    fileTime = new DateTime();

        try
        {
          fileTime = System.IO.File.GetLastWriteTime( dependency.Key );
        }
        catch
        {
        }
        if ( fileTime != dependency.Value )
        {
          //Debug.Log( "File time differs for " + dependency.Key );
          return true;
        }
      }
      return false;
    }



    internal bool NeedsRebuild( DocumentInfo DocumentToBuild )
    {
      if ( DocumentToBuild == null )
      {
        return true;
      }
      if ( DocumentToBuild.Project == null )
      {
        return true;
      }
      return NeedsRebuild( DocumentToBuild, DocumentToBuild.Project.Settings.CurrentConfig.Name );
    }



    internal bool IsCurrentlyBuilding()
    {
      if ( ( Core.State == StudioState.BUILD )
      ||   ( Core.State == StudioState.BUILD_AND_DEBUG )
      ||   ( Core.State == StudioState.BUILD_AND_RUN )
      ||   ( Core.State == StudioState.BUILD_PRE_PROCESSED_FILE )
      ||   ( Core.State == StudioState.COMPILE ) )
      {
        return true;
      }
      return false;
    }



    internal void PreparseDocument( BaseDocument Document )
    {
      if ( Document.DocumentInfo.Project != null )
      {
        Core.MainForm.AddTask( new C64Studio.Tasks.TaskParseFile( Document.DocumentInfo, Document.DocumentInfo.Project.Settings.CurrentConfig ) );
      }
      else
      {
        Core.MainForm.AddTask( new C64Studio.Tasks.TaskParseFile( Document.DocumentInfo, null ) );
      }
    }



  }
}
