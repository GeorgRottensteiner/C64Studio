using C64Studio.Types;
using GR.Collections;
using RetroDevStudio;
using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio.Tasks
{
  public class TaskCompile : Task
  {
    private DocumentInfo    m_DocumentToBuild;
    private DocumentInfo    m_DocumentToDebug;
    private DocumentInfo    m_DocumentToRun;
    private DocumentInfo    m_ActiveDocument;
    private Solution        m_Solution;
    private bool            CreatePreProcessedFile = false;



    public TaskCompile( DocumentInfo DocumentToBuild, DocumentInfo DocumentToDebug, DocumentInfo DocumentToRun, DocumentInfo ActiveDocumentInfo, Solution Solution, bool CreatePreProcessedFile )
    {
      this.CreatePreProcessedFile = CreatePreProcessedFile;
      m_DocumentToBuild = DocumentToBuild;
      m_DocumentToDebug = DocumentToDebug;
      m_DocumentToRun = DocumentToRun;
      m_ActiveDocument = ActiveDocumentInfo;
      m_Solution = Solution;
    }



    protected override bool ProcessTask()
    {
      Core.SetStatus( "Building..." );
      Core.ClearOutput();
      Core.Compiling.m_RebuiltFiles.Clear();
      Core.Compiling.m_RebuiltBuildConfigFiles.Clear();
      Core.Compiling.m_BuildChainStack.Clear();
      bool needsRebuild = Core.Compiling.NeedsRebuild( m_DocumentToBuild );
      if ( needsRebuild )
      {
        Core.Compiling.m_BuildIsCurrent = false;
        if ( m_DocumentToBuild.Project != null )
        {
          if ( !Core.MainForm.SaveProject( m_DocumentToBuild.Project ) )
          {
            Core.SetStatus( "Failed to save project" );
            Core.MainForm.AppState = Types.StudioState.NORMAL;
            return false;
          }
        }
        Core.MainForm.SaveAllDocuments();
        if ( m_DocumentToBuild.Project != null )
        {
          if ( !Core.MainForm.SaveProject( m_DocumentToBuild.Project ) )
          {
            Core.SetStatus( "Failed to save project" );
            Core.MainForm.AppState = Types.StudioState.NORMAL;
            return false;
          }
        }
      }

      DocumentInfo baseDoc = m_DocumentToBuild;
      if ( baseDoc == null )
      {
        Core.SetStatus( "No active document" );
        Core.MainForm.AppState = Types.StudioState.NORMAL;
        return false;
      }
      if ( ( baseDoc.Element == null )
      &&   ( !baseDoc.Compilable ) )
      {
        Core.AddToOutput( "Document is not part of project, cannot build" + System.Environment.NewLine );
        Core.SetStatus( "Document is not part of project, cannot build" );
        Core.MainForm.AppState = Types.StudioState.NORMAL;
        return false;
      }
      Core.AddToOutput( "Determined " + baseDoc.DocumentFilename + " as active document" + System.Environment.NewLine );

      Types.BuildInfo buildInfo = new C64Studio.Types.BuildInfo();
      if ( !Core.Compiling.m_BuildIsCurrent )
      {
        C64Studio.Types.ASM.FileInfo    dummyInfo;

        string  configSetting = null;
        if ( baseDoc.Project != null )
        {
          configSetting = baseDoc.Project.Settings.CurrentConfig.Name;
        }

        if ( !BuildElement( baseDoc, configSetting, null, true, out buildInfo, out dummyInfo ) )
        {
          Core.SetStatus( "Build failed" );
          Core.MainForm.AppState = Types.StudioState.NORMAL;
          return false;
        }

        Core.Compiling.m_LastBuildInfo = buildInfo;
        Core.Compiling.m_BuildIsCurrent = true;
      }
      else
      {
        if ( baseDoc.Element != null )
        {
          buildInfo.TargetType = baseDoc.Element.TargetType;
        }
        else
        {
          buildInfo = Core.Compiling.m_LastBuildInfo;
        }
        if ( buildInfo.TargetType == C64Studio.Types.CompileTargetType.NONE )
        {
          buildInfo.TargetType = Core.Compiling.m_LastBuildInfo.TargetType;
        }

        Core.AddToOutput( "Build is current" + System.Environment.NewLine );
      }
      if ( Core.Navigating.DetermineASMFileInfo( baseDoc ) == Core.Navigating.DetermineASMFileInfo( m_ActiveDocument ) )
      {
        //Debug.Log( "m_Outline.RefreshFromDocument after compile" );
        Core.MainForm.m_Outline.RefreshFromDocument( baseDoc.BaseDoc );
      }
      Core.SetStatus( "Build successful" );

      switch ( Core.MainForm.AppState )
      {
        case StudioState.COMPILE:
        case StudioState.BUILD:
        case StudioState.BUILD_PRE_PROCESSED_FILE:
          if ( Core.Settings.PlaySoundOnSuccessfulBuild )
          {
            System.Media.SystemSounds.Asterisk.Play();

            if ( Core.MainForm.AppState == StudioState.BUILD_PRE_PROCESSED_FILE )
            {
              Core.MainForm.AppState = Types.StudioState.NORMAL;

              string pathLog = System.IO.Path.Combine( System.IO.Path.GetDirectoryName( m_DocumentToBuild.FullPath ), System.IO.Path.GetFileNameWithoutExtension( m_DocumentToBuild.FullPath ) + ".dump" );
              //Core.MainForm.OpenFile( pathLog );
              Core.Navigating.OpenDocumentAndGotoLine( null, Core.Navigating.FindDocumentInfoByPath( pathLog ), 0 );
            }
          }
          Core.MainForm.AppState = Types.StudioState.NORMAL;
          break;
        case Types.StudioState.BUILD_AND_RUN:
          // run program
          {
            Types.CompileTargetType targetType = buildInfo.TargetType;
            if ( m_DocumentToRun.Element != null )
            {
              if ( m_DocumentToRun.Element.TargetType != C64Studio.Types.CompileTargetType.NONE )
              {
                targetType = m_DocumentToRun.Element.TargetType;
              }
              ProjectElement.PerConfigSettings  configSetting = m_DocumentToRun.Element.Settings[m_DocumentToRun.Project.Settings.CurrentConfig.Name];
              if ( !string.IsNullOrEmpty( configSetting.DebugFile ) )
              {
                targetType = configSetting.DebugFileType;
              }
            }
            if ( !Core.MainForm.RunCompiledFile( m_DocumentToRun, targetType ) )
            {
              Core.MainForm.AppState = Types.StudioState.NORMAL;
              return false;
            }
          }
          break;
        case Types.StudioState.BUILD_AND_DEBUG:
          // run program
          if ( !Core.Debugging.DebugCompiledFile( m_DocumentToDebug, m_DocumentToRun ) )
          {
            Core.MainForm.AppState = Types.StudioState.NORMAL;
            return false;
          }
          break;
        default:
          Core.MainForm.AppState = Types.StudioState.NORMAL;
          break;
      }

      return true;
    }



    bool BuildElement( DocumentInfo Doc, string ConfigSetting, string AdditionalPredefines, bool OutputMessages, out Types.BuildInfo BuildInfo, out Types.ASM.FileInfo FileInfo )
    {
      BuildInfo = new C64Studio.Types.BuildInfo();
      BuildInfo.TargetFile = "";
      BuildInfo.TargetType = Types.CompileTargetType.NONE;

      FileInfo = null;

      Types.ASM.FileInfo combinedFileInfo = null;

      try
      {
        if ( Doc.Element != null )
        {
          Doc.Element.CompileTarget = Types.CompileTargetType.NONE;
          Doc.Element.CompileTargetFile = null;

          // check dependencies
          foreach ( var dependency in Doc.Element.ForcedDependency.DependentOnFile )
          {
            var project = Core.Navigating.Solution.GetProjectByName( dependency.Project );
            if ( project == null )
            {
              Core.AddToOutput( "Could not find project \"" + dependency.Project + "\" for dependency \"" + dependency.Filename + "\" for \"" + Doc.DocumentFilename + "\"" + System.Environment.NewLine );
              if ( Core.Settings.PlaySoundOnBuildFailure )
              {
                System.Media.SystemSounds.Exclamation.Play();
              }
            }

            ProjectElement elementDependency = project.GetElementByFilename( dependency.Filename );
            if ( elementDependency == null )
            {
              Core.AddToOutput( "Could not find dependency \"" + dependency.Filename + "\" in project \"" + dependency.Project + "\" for \"" + Doc.DocumentFilename + "\"" + System.Environment.NewLine );
              if ( Core.Settings.PlaySoundOnBuildFailure )
              {
                System.Media.SystemSounds.Exclamation.Play();
              }
              return false;
            }

            Types.ASM.FileInfo    dependencyFileInfo = null;

            // skip building if not required
            if ( !Core.Compiling.NeedsRebuild( elementDependency.DocumentInfo, ConfigSetting ) )
            {
              Core.AddToOutput( "Dependency " + dependency.Filename + " is current for config " + ConfigSetting + System.Environment.NewLine );

              if ( ( Doc.Type == ProjectElement.ElementType.ASM_SOURCE )
              ||   ( Doc.Type == ProjectElement.ElementType.BASIC_SOURCE ) )
              {
                dependencyFileInfo = elementDependency.DocumentInfo.ASMFileInfo;
                //Debug.Log( "Doc " + Doc.Text + " receives " + dependencyFileInfo.Labels.Count + " dependency labels from dependency " + dependency.Filename );
              }
            }
            else
            {
              Types.BuildInfo tempInfo = new C64Studio.Types.BuildInfo();

              if ( !BuildElement( elementDependency.DocumentInfo, ConfigSetting, null, false, out tempInfo, out dependencyFileInfo ) )
              {
                return false;
              }
            }
            // include symbols from dependency
            if ( dependency.IncludeSymbols )
            {
              if ( combinedFileInfo == null )
              {
                combinedFileInfo = new C64Studio.Types.ASM.FileInfo();
              }
              // merge label info
              if ( dependencyFileInfo != null )
              {
                foreach ( var entry in dependencyFileInfo.Labels )
                {
                  if ( !combinedFileInfo.Labels.ContainsKey( entry.Key ) )
                  {
                    combinedFileInfo.Labels.Add( entry.Key, entry.Value );
                  }
                }
                //Debug.Log( "Doc " + Doc.Text + " receives " + dependencyFileInfo.Labels.Count + " dependency labels from dependency " + dependency.Filename );
              }
            }
          }
        }

        if ( !Doc.Compilable )
        {
          // not buildable 
          // TODO - Autoexport?
          return true;
        }

        ToolInfo tool = Core.DetermineTool( Doc, false );

        ProjectElement.PerConfigSettings configSetting = null;

        Parser.ParserBase parser = Core.DetermineParser( Doc );

        GR.Collections.Map<string, DateTime>    currentBuildState;

        if ( ConfigSetting != null )
        {
          if ( !Doc.DeducedDependency.ContainsKey( ConfigSetting ) )
          {
            Doc.DeducedDependency.Add( ConfigSetting, new DependencyBuildState() );
          }
          Doc.DeducedDependency[ConfigSetting].BuildState.Add( Doc.FullPath, Core.Compiling.FileLastWriteTime( Doc.FullPath ) );

          currentBuildState = new GR.Collections.Map<string,DateTime>( Doc.DeducedDependency[ConfigSetting].BuildState );

          if ( Doc.Element != null )
          {
            if ( !Doc.Element.Settings.ContainsKey( ConfigSetting ) )
            {
              Doc.Element.Settings.Add( ConfigSetting, new ProjectElement.PerConfigSettings() );
            }
            configSetting = Doc.Element.Settings[ConfigSetting];

            if ( !string.IsNullOrEmpty( configSetting.PreBuild ) )
            {
              Core.AddToOutput( "Running pre build step on " + Doc.Element.Name + System.Environment.NewLine );
              if ( !Core.Executing.RunCommand( Doc, "pre build", configSetting.PreBuild ) )
              {
                return false;
              }
            }
            if ( configSetting.PreBuildChain.Active )
            {
              if ( !BuildChain( configSetting.PreBuildChain, "pre build chain", ConfigSetting, OutputMessages ) )
              {
                return false;
              }
              AppendBuildStates( currentBuildState, Doc.DeducedDependency[ConfigSetting].BuildState );
            }
            Core.AddToOutput( "Running build on " + Doc.Element.Name + " with configuration " + ConfigSetting + System.Environment.NewLine );
          }
          else
          {
            Core.AddToOutput( "Running build on " + Doc.DocumentFilename + System.Environment.NewLine );
          }
        }
        else
        {
          currentBuildState = new GR.Collections.Map<string, DateTime>();
          Core.AddToOutput( "Running build on " + Doc.DocumentFilename + System.Environment.NewLine );
        }

        // include previous symbols
        string    additionalPredefines = null;
        if ( parser is Parser.ASMFileParser )
        {
          ( (Parser.ASMFileParser)parser ).InitialFileInfo = combinedFileInfo;
          if ( combinedFileInfo != null )
          {
            //Debug.Log( "Doc " + Doc.Text + " receives " + combinedFileInfo.Labels.Count + " initial labels" );
          }
          additionalPredefines = AdditionalPredefines;

          /*
          // add pre/post build chain entries as externally included files to validate their time stamps
          if ( ( configSetting != null )
          &&   ( configSetting.PreBuildChain.Active ) )
          {
            foreach ( var chainEntry in configSetting.PreBuildChain.Entries )
            {
              var chainProject = Core.Navigating.Solution.GetProjectByName( chainEntry.ProjectName );
              if ( chainProject != null )
              {
                ( (Parser.ASMFileParser)parser ).ExternallyIncludedFiles.Add( chainProject.FullPath( chainEntry.DocumentFilename ) );
              }
            }
          }
          if ( ( configSetting != null )
          &&   ( configSetting.PostBuildChain.Active ) )
          {
            foreach ( var chainEntry in configSetting.PostBuildChain.Entries )
            {
              var chainProject = Core.Navigating.Solution.GetProjectByName( chainEntry.ProjectName );
              if ( chainProject != null )
              {
                ( (Parser.ASMFileParser)parser ).ExternallyIncludedFiles.Add( chainProject.FullPath( chainEntry.DocumentFilename ) );
              }
            }
          }*/
        }
        else if ( parser is Parser.BasicFileParser )
        {
          // BASIC may receive symbols from assembly
          ( (Parser.BasicFileParser)parser ).InitialFileInfo = combinedFileInfo;
          ( (Parser.BasicFileParser)parser ).SetBasicDialect( ( (Parser.BasicFileParser)parser ).Settings.BASICDialect );
          if ( Doc.BaseDoc != null )
          {
            ( (Parser.BasicFileParser)parser ).Settings.UpperCaseMode = !( (SourceBasicEx)Doc.BaseDoc ).m_LowerCaseMode;
          }
          if ( combinedFileInfo != null )
          {
            //Debug.Log( "Doc " + Doc.Text + " receives " + combinedFileInfo.Labels.Count + " initial labels" );
          }
          Doc.ASMFileInfo = combinedFileInfo;
        }

        parser.AssembledOutput = null;

        if ( ( configSetting != null )
        &&   ( !string.IsNullOrEmpty( configSetting.CustomBuild ) ) )
        {
          Core.AddToOutput( "Running custom build step on " + Doc.Element.Name + " with configuration " + ConfigSetting + System.Environment.NewLine );
          if ( !Core.Executing.RunCommand( Doc, "custom build", configSetting.CustomBuild ) )
          {
            return false;
          }
          BuildInfo.TargetFile = Doc.Element.TargetFilename;
          BuildInfo.TargetType = Doc.Element.TargetType;
        }
        else
        {
          ProjectConfig config = null;
          if ( Doc.Project != null )
          {
            config = Doc.Project.Settings.Configuration( ConfigSetting );
          }

          int   startAddress = -1;
          if ( ( Doc.Type == ProjectElement.ElementType.BASIC_SOURCE )
          &&   ( Doc.BaseDoc != null ) )
          {
            // BASIC files bring a start address
            startAddress = ( (SourceBasicEx)Doc.BaseDoc ).StartAddress;
            ( (Parser.BasicFileParser)parser ).SetBasicDialect( ( (SourceBasicEx)Doc.BaseDoc ).BASICDialect );
          }

          if ( ( !Core.MainForm.ParseFile( parser, Doc, config, additionalPredefines, OutputMessages, CreatePreProcessedFile ) )
          ||   ( !parser.Assemble( new C64Studio.Parser.CompileConfig()
                                        {
                                          TargetType = Core.DetermineTargetType( Doc, parser ),
                                          OutputFile = Core.DetermineTargetFilename( Doc, parser ),
                                          AutoTruncateLiteralValues = Core.Settings.ASMAutoTruncateLiteralValues,
                                          StartAddress = startAddress,
                                          EnabledHacks = Core.Settings.EnabledC64StudioHacks,
                                          Encoding = Core.Settings.SourceFileEncoding
                                        } ) )
          ||   ( parser.Errors > 0 ) )
          {
            Core.MainForm.AddOutputMessages( parser );

            Core.AddToOutput( "Build failed, " + parser.Warnings.ToString() + " warnings, " + parser.Errors.ToString() + " errors encountered" + System.Environment.NewLine );
            // always show messages if we fail!
            //if ( OutputMessages )
            {
              Core.Navigating.UpdateFromMessages( parser.Messages,
                                                        ( parser is Parser.ASMFileParser ) ? ( (Parser.ASMFileParser)parser ).ASMFileInfo : null,
                                                        Doc.Project );
              Core.MainForm.m_CompileResult.UpdateFromMessages( parser, Doc.Project );
            }
            Core.ShowDocument( Core.MainForm.m_CompileResult );
            Core.MainForm.AppState = Types.StudioState.NORMAL;

            if ( Core.Settings.PlaySoundOnBuildFailure )
            {
              System.Media.SystemSounds.Exclamation.Play();
            }
            return false;
          }

          if ( ConfigSetting != null )
          {
            AppendBuildStates( currentBuildState, Doc.DeducedDependency[ConfigSetting].BuildState );
          }

          Core.MainForm.AddOutputMessages( parser );

          var compileTarget = Core.DetermineTargetType( Doc, parser );
          string compileTargetFile = Core.DetermineTargetFilename( Doc, parser );
          if ( Doc.Element != null )
          {
            Doc.Element.CompileTargetFile = compileTargetFile;
          }

          if ( compileTargetFile == null )
          {
            if ( parser is Parser.ASMFileParser )
            {
              parser.AddError( -1, Types.ErrorCode.E0001_NO_OUTPUT_FILENAME, "No output filename was given, missing element setting or !to <Filename>,<FileType> macro?" );
            }
            else
            {
              parser.AddError( -1, Types.ErrorCode.E0001_NO_OUTPUT_FILENAME, "No output filename was given, missing element setting" );
            }
            if ( OutputMessages )
            {
              Core.Navigating.UpdateFromMessages( parser.Messages,
                                              ( parser is Parser.ASMFileParser ) ? ( (Parser.ASMFileParser)parser ).ASMFileInfo : null,
                                              Doc.Project );

              Core.MainForm.m_CompileResult.UpdateFromMessages( parser, Doc.Project );
            }
            Core.ShowDocument( Core.MainForm.m_CompileResult );
            Core.MainForm.AppState = Types.StudioState.NORMAL;

            if ( Core.Settings.PlaySoundOnBuildFailure )
            {
              System.Media.SystemSounds.Exclamation.Play();
            }
            return false;
          }
          BuildInfo.TargetFile = compileTargetFile;
          BuildInfo.TargetType = compileTarget;

          if ( parser.Warnings > 0 )
          {
            if ( OutputMessages )
            {
              Core.Navigating.UpdateFromMessages( parser.Messages,
                                              ( parser is Parser.ASMFileParser ) ? ( (Parser.ASMFileParser)parser ).ASMFileInfo : null,
                                              Doc.Project );

              Core.MainForm.m_CompileResult.UpdateFromMessages( parser, Doc.Project );
            }
            Core.ShowDocument( Core.MainForm.m_CompileResult );
          }
        }

        if ( string.IsNullOrEmpty( BuildInfo.TargetFile ) )
        {
          Core.AddToOutput( "No target file name specified" + System.Environment.NewLine );
          Core.MainForm.AppState = Types.StudioState.NORMAL;
          if ( Core.Settings.PlaySoundOnBuildFailure )
          {
            System.Media.SystemSounds.Exclamation.Play();
          }
          return false;
        }
        // write output if applicable
        if ( ( parser.AssembledOutput != null )
        &&   ( parser.AssembledOutput.Assembly != null ) )
        {
          try
          {
            System.IO.File.WriteAllBytes( BuildInfo.TargetFile, parser.AssembledOutput.Assembly.Data() );
          }
          catch ( System.Exception ex )
          {
            Core.AddToOutput( "Build failed, Could not create output file " + parser.CompileTargetFile + System.Environment.NewLine );
            Core.AddToOutput( ex.ToString() + System.Environment.NewLine );
            Core.MainForm.AppState = Types.StudioState.NORMAL;
            if ( Core.Settings.PlaySoundOnBuildFailure )
            {
              System.Media.SystemSounds.Exclamation.Play();
            }
            return false;
          }
          Core.AddToOutput( "Build successful, " + parser.Warnings.ToString() + " warnings, 0 errors encountered" + System.Environment.NewLine );

          int assemblyEndAddress = parser.AssembledOutput.OriginalAssemblyStartAddress + parser.AssembledOutput.OriginalAssemblySize - 1;
          if ( parser.AssembledOutput.OriginalAssemblySize == 0 )
          {
            ++assemblyEndAddress;
          }

          Core.AddToOutput( "Start address $" + parser.AssembledOutput.OriginalAssemblyStartAddress.ToString( "X4" )
                            + " to $" + assemblyEndAddress.ToString( "X4" )
                            + ", size " + parser.AssembledOutput.OriginalAssemblySize + " bytes" + System.Environment.NewLine );
          Core.AddToOutput( "Memory Map:" + System.Environment.NewLine );
          if ( parser.AssembledOutput.MemoryMap != null )
          {
            foreach ( var mapEntry in parser.AssembledOutput.MemoryMap.Entries )
            {
              int     endAddress = mapEntry.StartAddress + mapEntry.Length - 1;
              if ( mapEntry.Length == 0 )
              {
                endAddress = mapEntry.StartAddress;
              }
              if ( string.IsNullOrEmpty( mapEntry.Description ) )
              {
                Core.AddToOutput( "  $" + mapEntry.StartAddress.ToString( "X4" ) + " - $" + endAddress.ToString( "X4" ) + " - unnamed section" + System.Environment.NewLine );
              }
              else
              {
                Core.AddToOutput( "  $" + mapEntry.StartAddress.ToString( "X4" ) + " - $" + endAddress.ToString( "X4" ) + " - " + mapEntry.Description + System.Environment.NewLine );
              }
            }
          }
          Core.AddToOutput( "Compiled to file " + BuildInfo.TargetFile + ", " + parser.AssembledOutput.Assembly.Length + " bytes" + System.Environment.NewLine );

          //Debug.Log( "File " + Doc.DocumentFilename + " was rebuilt for config " + ConfigSetting + " this round" );
        }

        if ( ( configSetting != null )
        &&   ( configSetting.PostBuildChain.Active ) )
        {
          if ( !BuildChain( configSetting.PostBuildChain, "post build chain", ConfigSetting, OutputMessages ) )
          {
            return false;
          }
          AppendBuildStates( currentBuildState, Doc.DeducedDependency[ConfigSetting].BuildState );
        }

        // store combined build state info in document
        if ( ConfigSetting != null )
        {
          Doc.DeducedDependency[ConfigSetting].BuildState = currentBuildState;
          Doc.DeducedDependency[ConfigSetting].BuildState.Remove( Doc.FullPath );
        }

        if ( ( configSetting != null )
        &&   ( !string.IsNullOrEmpty( configSetting.PostBuild ) ) )
        {
          Core.ShowDocument( Core.MainForm.m_Output );
          Core.AddToOutput( "Running post build step on " + Doc.Element.Name + System.Environment.NewLine );
          if ( !Core.Executing.RunCommand( Doc, "post build", configSetting.PostBuild ) )
          {
            return false;
          }
        }

        Doc.HasBeenSuccessfullyBuilt = true;

        Types.ASM.FileInfo   fileInfo = null;
        List<AutoCompleteItemInfo> knownTokens = null;
        MultiMap<string,SymbolInfo> knownTokenInfo = null;
        if ( parser is Parser.ASMFileParser )
        {
          fileInfo = ( (Parser.ASMFileParser)parser ).ASMFileInfo;
          // update symbols in main asm file
          knownTokens = parser.KnownTokens();
          knownTokenInfo = parser.KnownTokenInfo();
        }
        else if ( parser is Parser.BasicFileParser )
        {
          fileInfo = ( (Parser.BasicFileParser)parser ).ASMFileInfo;
          // update symbols in main asm file
          knownTokens = parser.KnownTokens();
          knownTokenInfo = parser.KnownTokenInfo();
          //Doc.SetASMFileInfo( fileInfo, parser.KnownTokens(), parser.KnownTokenInfo() );
        }

        if ( ( fileInfo != null )
        &&   ( Doc.Project == null ) )
        {
          // not part of project, spread known tokens to all participating files
          foreach ( var document in Core.MainForm.DocumentInfos )
          {
            if ( document != null )
            {
              if ( fileInfo.ContainsFile( document.DocumentFilename ) )
              {
                document.SetASMFileInfo( fileInfo, knownTokens, knownTokenInfo );
              }
            }
          }
        }

        if ( fileInfo != null )
        {
          if ( !string.IsNullOrEmpty( fileInfo.LabelDumpFile ) )
          {
            Core.MainForm.DumpLabelFile( fileInfo );
          }
        }

        Core.Compiling.m_RebuiltFiles.Add( Doc.DocumentFilename );

        return true;
      }
      catch ( Exception ex )
      {
        Core.AddToOutput( "An error occurred during building an element\r\n" + ex.ToString() );
        return false;
      }
    }



    private void AppendBuildStates( Map<string, DateTime> CurrentBuildState, Map<string, DateTime> NewBuildState )
    {
      foreach ( var entry in NewBuildState )
      {
        CurrentBuildState[entry.Key] = entry.Value;
      }
    }



    bool BuildChain( Types.BuildChain BuildChain, string BuildChainDescription, string ParentDocumentConfigSetting, bool OutputMessages )
    {
      if ( Core.Compiling.m_BuildChainStack.Contains( BuildChain ) )
      {
        // already on stack, silent "success"
        return true;
      }

      Core.AddToOutput( "Running " + BuildChainDescription + System.Environment.NewLine );
      Core.Compiling.m_BuildChainStack.Push( BuildChain );
      foreach ( var entry in BuildChain.Entries )
      {
        BuildInfo                     buildInfo;
        Types.ASM.FileInfo            fileInfo;

        string  buildInfoKey = entry.ProjectName + "/" + entry.DocumentFilename + "/" + entry.Config;

        Core.AddToOutput( "Building " + buildInfoKey + System.Environment.NewLine );
        if ( Core.Compiling.m_RebuiltBuildConfigFiles.ContainsValue( buildInfoKey ) )
        {
          Core.AddToOutput( "-already built, skipping step" + System.Environment.NewLine );
          continue;
        }

        var project = m_Solution.GetProjectByName( entry.ProjectName );
        if ( project == null )
        {
          Core.AddToOutput( "-could not find referenced project " + entry.ProjectName + System.Environment.NewLine );
          Core.Compiling.m_BuildChainStack.Pop();
          return false;
        }

        var element = project.GetElementByFilename( entry.DocumentFilename );
        if ( element == null )
        {
          Core.AddToOutput( "-could not find document " + entry.DocumentFilename + " in project " + entry.ProjectName + System.Environment.NewLine );
          Core.Compiling.m_BuildChainStack.Pop();
          return false;
        }

        // ugly hack to force rebuild -> problem: we do not check output file timestamps if we need to recompile -> can't have build chain with same file in different configs!
        Core.MainForm.MarkAsDirty( element.DocumentInfo );

        // consolidate build chain and project config pre defines
        //var config = project.Settings.GetConfigurationByName( entry.Config );
        string preDefines = entry.PreDefines;
        /*
        if ( config != null )
        {
          preDefines += config.Defines;
        }*/

        if ( !BuildElement( element.DocumentInfo, entry.Config, preDefines, OutputMessages, out buildInfo, out fileInfo ) )
        {
          Core.Compiling.m_BuildChainStack.Pop();
          return false;
        }
        Core.Compiling.m_RebuiltBuildConfigFiles.Add( buildInfoKey );

        m_DocumentToBuild.DeducedDependency[ParentDocumentConfigSetting].BuildState[element.DocumentInfo.FullPath] = Core.Compiling.FileLastWriteTime( element.DocumentInfo.FullPath );
      }
      Core.AddToOutput( "Running " + BuildChainDescription + " completed successfully" + System.Environment.NewLine );
      Core.Compiling.m_BuildChainStack.Pop();
      return true;
    }



    public override string ToString()
    {
      return base.ToString() + " - " + m_DocumentToBuild.DocumentFilename;
    }




  }
}
