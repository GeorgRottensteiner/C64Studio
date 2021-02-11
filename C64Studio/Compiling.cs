using C64Models.BASIC;
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
    public Parser.BasicFileParser     ParserBasic = new C64Studio.Parser.BasicFileParser( new Parser.BasicFileParser.ParserSettings() );

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
          if ( parts.Length != 3 )
          {
            Core.AddToOutput( "Invalid BASIC format file '" + File + "', expected three columns in line " + lineIndex + System.Environment.NewLine );
            return null;
          }
          if ( exOpcodes )
          {
            dialect.AddExOpcode( parts[0], GR.Convert.ToI32( parts[1], 16 ) );
          }
          else
          {
            dialect.AddOpcode( parts[0], GR.Convert.ToI32( parts[1], 16 ), parts[2] );
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
          var project = Core.MainForm.m_Solution.GetProjectByName( dependency.Project );
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
          if ( ( !string.IsNullOrEmpty( DocInfo.Element.Settings[ConfigSetting].CustomBuild ) )
          &&   ( !string.IsNullOrEmpty( DocInfo.Element.TargetFilename ) ) )
          {
            Core.AddToOutput( "Custom build always requires a rebuild" + System.Environment.NewLine );
            return true;
          }
          
          foreach ( var dependency in DocInfo.Element.ExternalDependencies.DependentOnFile )
          {
            string      fullPath = BuildFullPath( DocInfo.Project.Settings.BasePath, dependency.Filename );

            DateTime    fileTime = new DateTime();

            try
            {
              if ( System.IO.File.Exists( fullPath ) )
              {
                fileTime = System.IO.File.GetLastWriteTime( fullPath );
              }
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
        }}
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
        Core.MainForm.AddTask( new C64Studio.Tasks.TaskParseFile( Document.DocumentInfo, Document.DocumentInfo.Project.Settings.CurrentConfig ) );
      }
      else
      {
        Core.MainForm.AddTask( new C64Studio.Tasks.TaskParseFile( Document.DocumentInfo, null ) );
      }
    }



  }
}
