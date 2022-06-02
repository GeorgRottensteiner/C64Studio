using C64Models.BASIC;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.Text;
using WeifenLuo.WinFormsUI.Docking;

namespace RetroDevStudio
{
  public class Compiling
  {
    public StudioCore                 Core = null;

    public bool                       m_BuildIsCurrent = false;

    public Dictionary<string,SingleBuildInfo>            m_LastBuildInfo = new Dictionary<string, SingleBuildInfo>();

    public Stack<BuildChain>          m_BuildChainStack = new Stack<BuildChain>();

    public GR.Collections.Set<string> m_RebuiltFiles = new GR.Collections.Set<string>();
    public GR.Collections.Set<string> m_RebuiltBuildConfigFiles = new GR.Collections.Set<string>();


    public Parser.ASMFileParser       ParserASM = new RetroDevStudio.Parser.ASMFileParser();
    public Parser.BasicFileParser     ParserBasic = new RetroDevStudio.Parser.BasicFileParser( new Parser.BasicFileParser.ParserSettings() );

    public Dictionary<string,Dialect> BASICDialects = new Dictionary<string, Dialect>();



    public Compiling( StudioCore Core )
    {
      this.Core = Core;
    }



    public void Initialise()
    {
      InitDialects();
    }



    private bool InitDialects()
    {

      // hard code BASIC V2, this one simply MUST exist
      BASICDialects.Add( "BASIC V2", Dialect.BASICV2 );

      try
      {
        string basePath = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;

        if ( basePath.ToUpper().StartsWith( "FILE:///" ) )
        {
          basePath = basePath.Substring( 8 );
        }
        string dialectFilePath = System.IO.Path.Combine( System.IO.Path.GetDirectoryName( basePath ), "BASIC Dialects" );

        var files = System.IO.Directory.GetFiles( dialectFilePath, "*.txt" );

        foreach ( var file in files )
        {
          try
          {
            ReadBASICDialect( file );
          }
          catch ( Exception ex )
          {
            Core.AddToOutput( "Exception reading BASIC dialect file " + file + ": " + ex.Message + System.Environment.NewLine );
          }
        }
      }
      catch ( Exception ex )
      {
        Core.AddToOutput( "Exception reading BASIC dialect files: " + ex.Message + System.Environment.NewLine );
      }
      return true;
    }



    private Dialect ReadBASICDialect( string File )
    {
      var dialect = new Dialect();
      using ( var reader = new GR.IO.BinaryReader( File ) )
      {
        string    line;
        bool      firstLine = true;
        int       lineIndex = 0;
        bool      exOpcodes = false;

        while ( reader.ReadLine( out line ) )
        {
          ++lineIndex;
          line = line.Trim();
          if ( ( string.IsNullOrEmpty( line ) )
          ||   ( line.StartsWith( "#" ) ) )
          {
            continue;
          }
          if ( line.StartsWith( "StartAddress=" ) )
          {
            dialect.DefaultStartAddress = line.Substring( 13 );
            continue;
          }
          else if ( line.StartsWith( "SafeLineLength=" ) )
          {
            dialect.SafeLineLength = GR.Convert.ToI32( line.Substring( 15 ) );
            continue;
          }
          else if ( line.StartsWith( "HexPrefix=" ) )
          {
            dialect.HexPrefix = line.Substring( 10 );
            continue;
          }

          // skip header
          if ( firstLine )
          {
            firstLine = false;
            continue;
          }
          

          if ( line == "ExOpcodes" )
          {
            exOpcodes = true;
            continue;
          }

          string[] parts = line.Split( ';' );
          if ( ( parts.Length != 3 )
          &&   ( parts.Length != 4 ) )
          {
            Core.AddToOutput( "Invalid BASIC format file '" + File + "', expected three or four columns in line " + lineIndex + System.Environment.NewLine );
            return null;
          }
          if ( exOpcodes )
          {
            dialect.AddExOpcode( parts[0], GR.Convert.ToI32( parts[1], 16 ) );
          }
          else
          {
            var opCode = dialect.AddOpcode( parts[0], GR.Convert.ToI32( parts[1], 16 ), parts[2] );

            if ( parts.Length == 4 )
            {
              string[]    extraInfo = parts[3].Split( ',' );

              for ( int i = 0; i < extraInfo.Length; ++i )
              {
                if ( string.Compare( extraInfo[i], "COMMENT", true ) == 0 )
                {
                  opCode.IsComment = true;
                }
              }
            }
          }
        }
      }
      dialect.Name = System.IO.Path.GetFileNameWithoutExtension( File );
      BASICDialects.Add( dialect.Name, dialect );

      return dialect;
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

      lock ( DocInfo.DeducedDependency )
      {
        // actual parsing and deducing dependencies if a rebuild is necessary!
        foreach ( IDockContent dockContent in Core.MainForm.panelMain.Documents )
        {
          BaseDocument baseDoc = (BaseDocument)dockContent;

          if ( baseDoc.Modified )
          {
            Core.AddToOutput( "Component '" + baseDoc.DocumentInfo.DocumentFilename + "' needs rebuilding." + System.Environment.NewLine );
            return true;
          }
        }

        if ( DocInfo.Element != null )
        {
          foreach ( var dependency in DocInfo.Element.ForcedDependency.DependentOnFile )
          {
            var project = Core.Navigating.Solution.GetProjectByName( dependency.Project );
            if ( project == null )
            {
              Core.AddToOutput( "Could not find dependency project " + dependency.Project + " for " + dependency + System.Environment.NewLine );
              return true;
            }

            ProjectElement elementDependency = project.GetElementByFilename( dependency.Filename );
            if ( elementDependency == null )
            {
              Core.AddToOutput( "Could not find dependency " + dependency.Filename + " in project " + dependency.Project + " for " + dependency + System.Environment.NewLine );
              return true;
            }
            if ( NeedsRebuild( elementDependency.DocumentInfo, ConfigSetting ) )
            {
              Core.AddToOutput( "Dependency '" + elementDependency.DocumentInfo.DocumentFilename + "' needs rebuilding." + System.Environment.NewLine );
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
            // custom build overrides output file -> always rebuild
            if ( ( DocInfo.Element.Settings.ContainsKey( ConfigSetting ) )
            &&   ( !string.IsNullOrEmpty( DocInfo.Element.Settings[ConfigSetting].CustomBuild ) )
            &&   ( !string.IsNullOrEmpty( DocInfo.Element.TargetFilename ) ) )
            {
              Core.AddToOutput( "Custom build always requires a rebuild" + System.Environment.NewLine );
              return true;
            }

            foreach ( var dependency in DocInfo.Element.ExternalDependencies.DependentOnFile )
            {
              string      fullPath = BuildFullPath( DocInfo.Project.Settings.BasePath, dependency.Filename );

              var dependencyBuildInfo = DocInfo.LastBuildInfo;// DocInfo.DeducedDependency[ConfigSetting].BuildState[fullPath];
              if ( ( dependencyBuildInfo != null )
              &&   ( !string.IsNullOrEmpty( dependencyBuildInfo.TargetFile ) )
              &&   ( !System.IO.File.Exists( dependencyBuildInfo.TargetFile ) ) )
              {
                Core.AddToOutput( $"Dependency target {dependencyBuildInfo.TargetFile} is missing, need to rebuild dependent element " + DocInfo.DocumentFilename + System.Environment.NewLine );
                return true;
              }

              var fileTime = FileLastWriteTime( fullPath );
              if ( fileTime != DocInfo.DeducedDependency[ConfigSetting].BuildState[fullPath].TimeStampOfTargetFile )
              {
                Core.AddToOutput( "External Dependency " + fullPath + " was modified, need to rebuild dependent element " + DocInfo.DocumentFilename + System.Environment.NewLine );
                return true;
              }
            }
          }
          else
          {
            // no build time stored yet, needs rebuild
            DocInfo.DeducedDependency[ConfigSetting] = new DependencyBuildState();

            Core.AddToOutput( "No last build time found for configuration '" + ConfigSetting + "', need rebuilding." + System.Environment.NewLine );
            return true;
          }

          // check indirect dependencies from pre build chains
          if ( !DocInfo.Element.Settings.ContainsKey( ConfigSetting ) )
          {
            DocInfo.Element.Settings.Add( ConfigSetting, new ProjectElement.PerConfigSettings() );
          }
          var configSettingInner = DocInfo.Element.Settings[ConfigSetting];
          if ( configSettingInner.PreBuildChain.Active )
          {
            foreach ( var chainEntry in configSettingInner.PreBuildChain.Entries )
            {
              var chainProject = Core.Navigating.Solution.GetProjectByName( chainEntry.ProjectName );
              if ( chainProject != null )
              {
                string      fullPath = BuildFullPath( chainProject.Settings.BasePath, chainEntry.DocumentFilename );
                var prebuildDoc = chainProject.GetElementByFilename( fullPath );
                if ( prebuildDoc != null )
                {
                  if ( NeedsRebuild( prebuildDoc.DocumentInfo ) )
                  {
                    return true;
                  }
                }

                var fileTime = FileLastWriteTime( fullPath );
                if ( fileTime != DocInfo.DeducedDependency[ConfigSetting].BuildState[fullPath].TimeStampOfSourceFile )
                {
                  if ( DocInfo.DeducedDependency[ConfigSetting].BuildState[fullPath].TimeStampOfSourceFile == default( DateTime ) )
                  {
                    Core.AddToOutput( $"PreBuild chain entry {fullPath} was modified {fileTime} , need to rebuild dependent element {DocInfo.DocumentFilename}" + System.Environment.NewLine );
                  }
                  else
                  {
                    Core.AddToOutput( $"PreBuild chain entry {fullPath} was modified {fileTime} != {DocInfo.DeducedDependency[ConfigSetting].BuildState[fullPath]}, need to rebuild dependent element {DocInfo.DocumentFilename}" + System.Environment.NewLine );
                  }
                  return true;
                }
              }
            }
          }
          if ( configSettingInner.PostBuildChain.Active )
          {
            foreach ( var chainEntry in configSettingInner.PostBuildChain.Entries )
            {
              var chainProject = Core.Navigating.Solution.GetProjectByName( chainEntry.ProjectName );
              if ( chainProject != null )
              {
                string      fullPath = BuildFullPath( chainProject.Settings.BasePath, chainEntry.DocumentFilename );
                var fileTime = FileLastWriteTime( fullPath );
                if ( fileTime != DocInfo.DeducedDependency[ConfigSetting].BuildState[fullPath].TimeStampOfSourceFile )
                {
                  if ( DocInfo.DeducedDependency[ConfigSetting].BuildState[fullPath].TimeStampOfSourceFile == default( DateTime ) )
                  {
                    Core.AddToOutput( $"PostBuild chain entry {fullPath} was modified {fileTime} , need to rebuild dependent element {DocInfo.DocumentFilename}" + System.Environment.NewLine );
                  }
                  else
                  {
                    Core.AddToOutput( $"PostBuild chain entry {fullPath} was modified {fileTime} != {DocInfo.DeducedDependency[ConfigSetting].BuildState[fullPath]}, need to rebuild dependent element {DocInfo.DocumentFilename}" + System.Environment.NewLine );
                  }
                  return true;
                }
              }
            }
          }
        }
        if ( DocInfo.Compilable )
        {
          if ( !DocInfo.HasBeenSuccessfullyBuilt )
          {
            Core.AddToOutput( "Element '" + DocInfo.DocumentFilename + "' was not built successfully last time." + System.Environment.NewLine );
            return true;
          }
        }
        if ( DocInfo.Project == null )
        {
          Core.AddToOutput( "Element '" + DocInfo.DocumentFilename + "' has no project, therefor needs rebuilding." + System.Environment.NewLine );
          return true;
        }
        if ( DocInfo.DeducedDependency[ConfigSetting] == null )
        {
          // no build time stored yet, needs rebuild
          DocInfo.DeducedDependency[ConfigSetting] = new DependencyBuildState();
          Core.AddToOutput( "No build time stored for '" + ConfigSetting + "' yet, therefor needs rebuilding." + System.Environment.NewLine );
          return true;
        }
        foreach ( var dependency in DocInfo.DeducedDependency[ConfigSetting].BuildState )
        {
          var fileTime = FileLastWriteTime( dependency.Key );
          if ( fileTime != dependency.Value.TimeStampOfSourceFile )
          {
            //Debug.Log( "File time differs for " + dependency.Key );
            Core.AddToOutput( "File '" + dependency.Key + "' was modified, therefor needs rebuilding." + System.Environment.NewLine );
            return true;
          }
        }

        var buildInfo = DocInfo.LastBuildInfo;
        if ( ( buildInfo != null )
        &&   ( !string.IsNullOrEmpty( buildInfo.TargetFile ) )
        &&   ( !System.IO.File.Exists( buildInfo.TargetFile ) ) )
        {
          Core.AddToOutput( $"Dependency target {buildInfo.TargetFile} is missing, need to rebuild dependent element " + DocInfo.DocumentFilename + System.Environment.NewLine );
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
      if ( DocumentToBuild.Project.Settings.CurrentConfig == null )
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
        Core.MainForm.AddTask( new RetroDevStudio.Tasks.TaskParseFile( Document.DocumentInfo, Document.DocumentInfo.Project.Settings.CurrentConfig ) );
      }
      else
      {
        Core.MainForm.AddTask( new RetroDevStudio.Tasks.TaskParseFile( Document.DocumentInfo, null ) );
      }
    }



    internal DateTime FileLastWriteTime( string FullPath )
    {
      DateTime  fileTime = DateTime.Now;
      try
      {
        fileTime = System.IO.File.GetLastWriteTime( FullPath );
      }
      catch ( Exception ex )
      {
        Debug.Log( "FileLastWriteTime failed: " + ex.ToString() );
      }
      return fileTime;
    }



  }
}
