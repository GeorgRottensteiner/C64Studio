using RetroDevStudio.Types;
using GR.Collections;
using System;
using System.Collections.Generic;
using RetroDevStudio.Documents;
using GR.Memory;

namespace RetroDevStudio.Tasks
{
  public class TaskCompile : Task
  {
    private DocumentInfo    m_DocumentToBuild;
    private DocumentInfo    m_DocumentToDebug;
    private DocumentInfo    m_DocumentToRun;
    private DocumentInfo    m_ActiveDocument;
    private Solution        m_Solution;
    private bool            CreatePreProcessedFile = false;
    private bool            CreateRelocationFile = false;
    private bool            DisplayOutput = false;



    public TaskCompile( DocumentInfo DocumentToBuild, DocumentInfo DocumentToDebug, DocumentInfo DocumentToRun, DocumentInfo ActiveDocumentInfo, Solution Solution, bool CreatePreProcessedFile, bool CreateRelocationFile, bool DisplayOutput = true )
    {
      this.CreatePreProcessedFile = CreatePreProcessedFile;
      this.CreateRelocationFile = CreateRelocationFile;
      this.DisplayOutput = DisplayOutput;
      m_DocumentToBuild = DocumentToBuild;
      m_DocumentToDebug = DocumentToDebug;
      m_DocumentToRun = DocumentToRun;
      m_ActiveDocument = ActiveDocumentInfo;
      m_Solution = Solution;
    }



    protected bool DebugCompiledFile( DocumentInfo DocumentToDebug, DocumentInfo DocumentToRun )
    {
      if ( DocumentToDebug == null )
      {
        Core.AddToOutput( "Debug document not found, this is an internal error!" );
        return false;
      }

      if ( DocumentToDebug.Element == null )
      {
        Core.AddToOutput( "Debugging " + DocumentToDebug.DocumentFilename + System.Environment.NewLine );
      }
      else
      {
        Core.AddToOutput( "Debugging " + DocumentToDebug.Element.Name + System.Environment.NewLine );
      }

      ToolInfo toolRun = Core.DetermineTool( DocumentToRun, ToolInfo.ToolType.EMULATOR );
      if ( toolRun == null )
      {
        Core.MessageBox( "No emulator tool has been configured yet!", "Missing emulator tool" );
        Core.AddToOutput( "There is no emulator tool configured!" );
        return false;
      }

      Core.Debugging.SetupDebugger( toolRun );

      if ( !Core.Debugging.Debugger.CheckEmulatorVersion( toolRun ) )
      {
        return false;
      }

      Core.Debugging.DebuggedASMBase = DocumentToDebug;
      Core.Debugging.DebugBaseDocumentRun = DocumentToRun;

      Core.MainForm.m_DebugWatch.ReseatWatches( DocumentToDebug.ASMFileInfo );
      Core.Debugging.Debugger.ClearCaches();
      Core.Debugging.MemoryViews.ForEach( mv => mv.MarkAllMemoryAsUnknown() );
      Core.Debugging.ReseatBreakpoints( DocumentToDebug.ASMFileInfo );
      Core.Debugging.AddVirtualBreakpoints( DocumentToDebug.ASMFileInfo );
      Core.Debugging.Debugger.ClearAllBreakpoints();
      Core.Debugging.MarkedDocument = null;
      Core.Debugging.MarkedDocumentLine = -1;

      if ( !toolRun.IsInternal )
      {
        if ( !Core.Executing.PrepareStartProcess( toolRun, DocumentToRun ) )
        {
          return false;
        }
        if ( !System.IO.Directory.Exists( Core.Executing.RunProcess.StartInfo.WorkingDirectory.Trim( new char[] { '"' } ) ) )
        {
          Core.MessageBox( "The determined working directory '" + Core.Executing.RunProcess.StartInfo.WorkingDirectory + "' does not exist", "Misconfigured tool" );
          Core.AddToOutput( "The determined working directory '" + Core.Executing.RunProcess.StartInfo.WorkingDirectory + "' does not exist" + System.Environment.NewLine );
          return false;
        }
      }

      // determine debug target type
      Types.CompileTargetType targetType = RetroDevStudio.Types.CompileTargetType.NONE;
      if ( DocumentToRun.Element != null )
      {
        targetType = DocumentToRun.Element.TargetType;
      }

      string fileToRun = "";
      if ( DocumentToRun.Element != null )
      {
        fileToRun = DocumentToRun.Element.TargetFilename;
        ProjectElement.PerConfigSettings configSetting = DocumentToRun.Element.Settings[DocumentToRun.Project.Settings.CurrentConfig.Name];
        if ( !string.IsNullOrEmpty( configSetting.DebugFile ) )
        {
          targetType = configSetting.DebugFileType;
        }
      }

      if ( targetType == RetroDevStudio.Types.CompileTargetType.NONE )
      {
        var lastBuildInfoOfThisFile = Core.Compiling.m_LastBuildInfo[DocumentToRun.FullPath];

        targetType = lastBuildInfoOfThisFile.TargetType;
      }
      Core.Debugging.DebugType = targetType;

      string symbolFile = Core.Debugging.PrepareAfterStartBreakPoints();
      string breakPointFile = "";
      string command = toolRun.DebugArguments;

      if ( Parser.ASMFileParser.IsCartridge( targetType ) )
      {
        command = command.Replace( "-initbreak 0x$(DebugStartAddressHex) ", "" );
      }

      if ( ( toolRun.PassLabelsToEmulator )
      &&   ( Core.Debugging.DebuggedASMBase.ASMFileInfo != null ) )
      {
        symbolFile += Core.Debugging.DebuggedASMBase.ASMFileInfo.LabelsAsFile( EmulatorInfo.LabelFormat( toolRun ) );
      }
      if ( ( EmulatorInfo.LabelFormat( toolRun ) == Types.ASM.LabelFileFormat.C64DEBUGGER )
      &&   ( Core.Debugging.DebuggedASMBase.ASMFileInfo != null )
      &&   ( Core.Debugging.BreakPoints.Count > 0 ) )
      {
        breakPointFile = Core.Debugging.BreakpointsToFile();
      }

      if ( symbolFile.Length > 0 )
      {
        try
        {
          Core.Debugging.TempDebuggerStartupFilename = System.IO.Path.GetTempFileName();
          System.IO.File.WriteAllText( Core.Debugging.TempDebuggerStartupFilename, symbolFile );
          switch ( EmulatorInfo.LabelFormat( toolRun ) )
          {
            case Types.ASM.LabelFileFormat.VICE:
              command += " -moncommands \"" + Core.Debugging.TempDebuggerStartupFilename + "\"";
              break;
            case Types.ASM.LabelFileFormat.C64DEBUGGER:
              command += " -vicesymbols \"" + Core.Debugging.TempDebuggerStartupFilename + "\"";
              break;
          }
        }
        catch ( System.IO.IOException ioe )
        {
          Core.MessageBox( ioe.Message, "Error writing temporary file" );
          Core.AddToOutput( "Error writing temporary file" );
          Core.Debugging.TempDebuggerStartupFilename = "";
          return false;
        }
      }
      if ( breakPointFile.Length > 0 )
      {
        try
        {
          Core.Debugging.TempDebuggerBreakpointFilename = System.IO.Path.GetTempFileName();
          System.IO.File.WriteAllText( Core.Debugging.TempDebuggerBreakpointFilename, breakPointFile );
          switch ( EmulatorInfo.LabelFormat( toolRun ) )
          {
            case Types.ASM.LabelFileFormat.C64DEBUGGER:
              command += " -breakpoints \"" + Core.Debugging.TempDebuggerBreakpointFilename + "\"";
              break;
          }
        }
        catch ( System.IO.IOException ioe )
        {
          Core.MessageBox( ioe.Message, "Error writing temporary file" );
          Core.AddToOutput( "Error writing temporary file" );
          Core.Debugging.TempDebuggerBreakpointFilename = "";
          return false;
        }
      }

      // need to adjust initial breakpoint address for late added store/load breakpoints?

      Core.Debugging.InitialBreakpointIsTemporary = true;
      //if ( BreakpointsToAddAfterStartup.Count > 0 )
      {
        // yes
        Core.Debugging.LateBreakpointOverrideDebugStart = Core.Debugging.OverrideDebugStart;

        // special start addresses for different run types
        if ( Parser.ASMFileParser.IsCartridge( targetType ) )
        {
          Core.Debugging.OverrideDebugStart = Core.Debugging.Debugger.ConnectedMachine.InitialBreakpointAddressCartridge;
        }
        else
        {
          // directly after calling load from ram (as VICE does when autostarting a .prg file)
          // TODO - check with .t64, .tap, .d64
          Core.Debugging.OverrideDebugStart = Core.Debugging.Debugger.ConnectedMachine.InitialBreakpointAddress;
        }
      }
      if ( ( DocumentToDebug.Project != null )
      &&   ( Core.Debugging.LateBreakpointOverrideDebugStart == -1 )
      &&   ( !string.IsNullOrEmpty( DocumentToDebug.Project.Settings.CurrentConfig.DebugStartAddressLabel ) ) )
      {
        int debugStartAddress = -1;
        if ( !Core.MainForm.DetermineDebugStartAddress( DocumentToDebug, DocumentToDebug.Project.Settings.CurrentConfig.DebugStartAddressLabel, out debugStartAddress ) )
        {
          Core.AddToOutput( "Cannot determine value for debug start address from '" + DocumentToDebug.Project.Settings.CurrentConfig.DebugStartAddressLabel + "'" + System.Environment.NewLine );
          return false;
        }
        if ( debugStartAddress != 0 )
        {
          Core.Debugging.InitialBreakpointIsTemporary = false;
          Core.Debugging.OverrideDebugStart = debugStartAddress;
          Core.Debugging.LateBreakpointOverrideDebugStart = debugStartAddress;
        }
      }

      if ( Core.Settings.TrueDriveEnabled )
      {
        command = toolRun.TrueDriveOnArguments + " " + command;
      }
      else
      {
        command = toolRun.TrueDriveOffArguments + " " + command;
      }

      if ( !toolRun.IsInternal )
      {
        bool error = false;

        Core.Executing.RunProcess.StartInfo.Arguments = Core.MainForm.FillParameters( command, DocumentToRun, true, out error );
        if ( error )
        {
          return false;
        }

        if ( Parser.ASMFileParser.IsCartridge( targetType ) )
        {
          Core.Executing.RunProcess.StartInfo.Arguments += " " + Core.MainForm.FillParameters( toolRun.CartArguments, DocumentToRun, true, out error );
        }
        else
        {
          Core.Executing.RunProcess.StartInfo.Arguments += " " + Core.MainForm.FillParameters( toolRun.PRGArguments, DocumentToRun, true, out error );
        }
        if ( error )
        {
          return false;
        }
      }
      else
      {
        // this sets up the initial breakpoint!
        Core.Debugging.Debugger.AddBreakpoint( new Breakpoint() { Temporary = true, Address = Core.Debugging.OverrideDebugStart } );

        ByteBuffer injectFile;

        string  targetFilename = Core.MainForm.FillParameters( "$(BuildTargetFilename)", DocumentToRun, true, out bool error );
        if ( error )
        {
          return false;
        }

        // determine file to inject
        switch ( targetType )
        {
          case CompileTargetType.PRG:
            injectFile = GR.IO.File.ReadAllBytes( targetFilename );
            break;
          default:
            Core.MessageBox( $"The target file type '{targetType}' is not supported by the Tiny64 debugger!", "File type not supported" );
            return false;
        }
        ( (Tiny64Debugger)Core.Debugging.Debugger ).InjectFile( injectFile );
      }

      if ( toolRun.IsInternal )
      {
        Core.AddToOutput( "Calling internal Tiny64 emulator" + System.Environment.NewLine );
      }
      else
      {
        Core.AddToOutput( "Calling " + Core.Executing.RunProcess.StartInfo.FileName + " with " + Core.Executing.RunProcess.StartInfo.Arguments + System.Environment.NewLine );
        Core.Executing.RunProcess.Exited += new EventHandler( Core.MainForm.runProcess_Exited );
        Core.Executing.RunProcess.Exited += new EventHandler( OnProcessExited );
      }
      Core.SetStatus( "Running..." );
      Core.MainForm.SetGUIForWaitOnExternalTool( true );

      if ( ( toolRun.IsInternal )
      ||   ( Core.Executing.RunProcess.Start() ) )
      {
        DateTime    current = DateTime.Now;
        int   numConnectionAttempts = 1;

        if ( !toolRun.IsInternal )
        {
          Core.Executing.RunProcess.BeginErrorReadLine();
          Core.Executing.RunProcess.BeginOutputReadLine();

          // new GTK VICE opens up with console window (yuck) which nicely interferes with WaitForInputIdle -> give it 5 seconds to open main window
          bool        waitForInputIdleFailed = false;
          try
          {
            Core.Executing.RunProcess.WaitForInputIdle( 5000 );
          }
          catch ( Exception ex )
          {
            Debug.Log( "WaitForInputIdle failed: " + ex.ToString() );
            waitForInputIdleFailed = true;
          }

          // only connect with debugger if VICE
          if ( ( Core.Executing.RunProcess != null )
          &&   ( string.IsNullOrEmpty( Core.Executing.RunProcess.MainWindowTitle ) )
          &&   ( waitForInputIdleFailed ) )
          {
            // assume GTK VICE
            numConnectionAttempts = 10;
          }
        }

        if ( EmulatorInfo.SupportsDebugging( toolRun ) )
        {
          for ( int i = 0; i < numConnectionAttempts; ++i )
          {
            Core.AddToOutput( "Connection attempt " + ( i + 1 ).ToString() + Environment.NewLine );
            if ( Core.Debugging.Debugger.ConnectToEmulator( Parser.ASMFileParser.IsCartridge( targetType ) ) )
            {
              Core.AddToOutput( " succeeded" + System.Environment.NewLine );
              Core.MainForm.m_CurrentActiveTool = toolRun;
              Core.Debugging.DebuggedProject = DocumentToRun.Project;
              Core.MainForm.AppState = Types.StudioState.DEBUGGING_RUN;
              Core.MainForm.SetGUIForDebugging( true );
              break;
            }
            // wait a second
            for ( int j = 0; j < 20; ++j )
            {
              System.Threading.Thread.Sleep( 50 );
              System.Windows.Forms.Application.DoEvents();
            }
          }
          if ( Core.MainForm.AppState != Types.StudioState.DEBUGGING_RUN )
          {
            Core.AddToOutput( "failed " + numConnectionAttempts + " times, giving up" + System.Environment.NewLine );
            return false;
          }
        }
        else
        {
          Core.MainForm.m_CurrentActiveTool = toolRun;
          Core.Debugging.DebuggedProject = DocumentToRun.Project;
          Core.MainForm.AppState = Types.StudioState.DEBUGGING_RUN;
          Core.MainForm.SetGUIForDebugging( true );
        }
      }
      return true;
    }



    private void OnProcessExited( object sender, EventArgs e )
    {
    }



    protected override bool ProcessTask()
    {
      if ( !DisplayOutput )
      {
        Core.SuppressOutput();
      }

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

            if ( !DisplayOutput )
            {
              Core.UnsuppressOutput();
            }
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

            if ( !DisplayOutput )
            {
              Core.UnsuppressOutput();
            }
            return false;
          }
        }
      }

      DocumentInfo baseDoc = m_DocumentToBuild;
      if ( baseDoc == null )
      {
        Core.SetStatus( "No active document" );
        Core.MainForm.AppState = Types.StudioState.NORMAL;

        if ( !DisplayOutput )
        {
          Core.UnsuppressOutput();
        }
        return false;
      }
      if ( ( baseDoc.Element == null )
      &&   ( !baseDoc.Compilable ) )
      {
        Core.AddToOutput( "Document is not part of project, cannot build" + System.Environment.NewLine );
        Core.SetStatus( "Document is not part of project, cannot build" );
        Core.MainForm.AppState = Types.StudioState.NORMAL;

        if ( !DisplayOutput )
        {
          Core.UnsuppressOutput();
        }
        return false;
      }
      Core.AddToOutput( "Determined " + baseDoc.DocumentFilename + " as active document" + System.Environment.NewLine );

      var buildInfo = new SingleBuildInfo();

      SingleBuildInfo buildInfoFromLastBuildOfThisFile = null;
      //Core.Compiling.m_LastBuildInfo.TryGetValue( baseDoc.FullPath, out buildInfoFromLastBuildOfThisFile );
      buildInfoFromLastBuildOfThisFile = baseDoc.LastBuildInfo;

      if ( ( buildInfoFromLastBuildOfThisFile != null )
      &&   ( ( !System.IO.File.Exists( buildInfoFromLastBuildOfThisFile.TargetFile ) )
      ||     ( buildInfoFromLastBuildOfThisFile.TimeStampOfTargetFile != Core.Compiling.FileLastWriteTime( buildInfoFromLastBuildOfThisFile.TargetFile ) ) ) )
      {
        Core.AddToOutput( $"Target file {buildInfoFromLastBuildOfThisFile.TargetFile} was modified or is missing" + System.Environment.NewLine );
        Core.Compiling.m_BuildIsCurrent = false;
      }

      if ( !Core.Compiling.m_BuildIsCurrent )
      {
        RetroDevStudio.Types.ASM.FileInfo    dummyInfo;

        string  configSetting = null;
        if ( baseDoc.Project != null )
        {
          configSetting = baseDoc.Project.Settings.CurrentConfig.Name;
        }

        if ( !BuildElement( baseDoc, configSetting, null, true, out buildInfo, out dummyInfo ) )
        {
          Core.SetStatus( "Build failed" );
          Core.MainForm.AppState = Types.StudioState.NORMAL;

          if ( !DisplayOutput )
          {
            Core.UnsuppressOutput();
          }
          return false;
        }

        Core.Compiling.m_LastBuildInfo[baseDoc.FullPath] = buildInfo;
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
          buildInfo = Core.Compiling.m_LastBuildInfo[baseDoc.FullPath];
        }
        if ( buildInfo.TargetType == RetroDevStudio.Types.CompileTargetType.NONE )
        {
          buildInfo.TargetType = Core.Compiling.m_LastBuildInfo[baseDoc.FullPath].TargetType;
        }
        Core.AddToOutput( "Build is current" + System.Environment.NewLine );
      }
      if ( Core.Navigating.DetermineASMFileInfo( baseDoc ) == Core.Navigating.DetermineASMFileInfo( m_ActiveDocument ) )
      {
        Core.MainForm.m_Outline.RefreshFromDocument( baseDoc.BaseDoc );
      }
      Core.SetStatus( "Build successful" );

      switch ( Core.MainForm.AppState )
      {
        case StudioState.COMPILE:
        case StudioState.BUILD:
        case StudioState.BUILD_PRE_PROCESSED_FILE:
        case StudioState.BUILD_RELOCATION_FILE:
          if ( Core.Settings.PlaySoundOnSuccessfulBuild )
          {
            System.Media.SystemSounds.Asterisk.Play();

            if ( Core.MainForm.AppState == StudioState.BUILD_PRE_PROCESSED_FILE )
            {
              Core.MainForm.AppState = Types.StudioState.NORMAL;

              string pathLog = System.IO.Path.Combine( System.IO.Path.GetDirectoryName( m_DocumentToBuild.FullPath ), System.IO.Path.GetFileNameWithoutExtension( m_DocumentToBuild.FullPath ) + ".dump" );
              Core.Navigating.OpenDocumentAndGotoLine( null, Core.Navigating.FindDocumentInfoByPath( pathLog ), 0 );
            }
            else if ( Core.MainForm.AppState == StudioState.BUILD_RELOCATION_FILE )
            {
              Core.MainForm.AppState = Types.StudioState.NORMAL;

              string pathLog = System.IO.Path.Combine( System.IO.Path.GetDirectoryName( m_DocumentToBuild.FullPath ), System.IO.Path.GetFileNameWithoutExtension( m_DocumentToBuild.FullPath ) + ".loc" );
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
              if ( m_DocumentToRun.Element.TargetType != RetroDevStudio.Types.CompileTargetType.NONE )
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

              if ( !DisplayOutput )
              {
                Core.UnsuppressOutput();
              }
              return false;
            }
          }
          break;
        case Types.StudioState.BUILD_AND_DEBUG:
          // run program
          if ( !DebugCompiledFile( m_DocumentToDebug, m_DocumentToRun ) )
          {
            Core.MainForm.AppState = Types.StudioState.NORMAL;

            if ( !DisplayOutput )
            {
              Core.UnsuppressOutput();
            }
            return false;
          }
          break;
        default:
          Core.MainForm.AppState = Types.StudioState.NORMAL;
          break;
      }

      if ( !DisplayOutput )
      {
        Core.UnsuppressOutput();
      }
      return true;
    }



    bool BuildElement( DocumentInfo Doc, string ConfigSetting, string AdditionalPredefines, bool OutputMessages, out SingleBuildInfo BuildInfo, out Types.ASM.FileInfo FileInfo )
    {
      BuildInfo = new SingleBuildInfo();
      BuildInfo.TargetFile            = "";
      BuildInfo.TargetType            = Types.CompileTargetType.NONE;
      BuildInfo.TimeStampOfSourceFile = Core.Compiling.FileLastWriteTime( Doc.FullPath );
      BuildInfo.TimeStampOfTargetFile = default( DateTime );

      FileInfo          = null;
      Doc.LastBuildInfo = null;

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
              var tempInfo = new SingleBuildInfo();

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
                combinedFileInfo = new RetroDevStudio.Types.ASM.FileInfo();
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

        ToolInfo tool = Core.DetermineTool( Doc, ToolInfo.ToolType.ASSEMBLER );

        ProjectElement.PerConfigSettings configSetting = null;

        Parser.ParserBase parser = Core.DetermineParser( Doc );

        GR.Collections.Map<string, SingleBuildInfo>    currentBuildState;

        if ( ConfigSetting != null )
        {
          lock ( Doc.DeducedDependency )
          {
            if ( !Doc.DeducedDependency.ContainsKey( ConfigSetting ) )
            {
              Doc.DeducedDependency.Add( ConfigSetting, new DependencyBuildState() );
            }
            Doc.DeducedDependency[ConfigSetting].BuildState.Add( Doc.FullPath,
                new SingleBuildInfo()
                {
                  TimeStampOfSourceFile = Core.Compiling.FileLastWriteTime( Doc.FullPath )
                }
               );

            currentBuildState = new GR.Collections.Map<string, SingleBuildInfo>( Doc.DeducedDependency[ConfigSetting].BuildState );

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
        }
        else
        {
          currentBuildState = new GR.Collections.Map<string, SingleBuildInfo>();
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

          if ( ( !Core.MainForm.ParseFile( parser, Doc, config, additionalPredefines, OutputMessages, CreatePreProcessedFile, CreateRelocationFile ) )
          ||   ( !parser.Assemble( new RetroDevStudio.Parser.CompileConfig()
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
            Core.ShowDocument( Core.MainForm.m_CompileResult, false );
            Core.MainForm.AppState = Types.StudioState.NORMAL;

            if ( Core.Settings.PlaySoundOnBuildFailure )
            {
              System.Media.SystemSounds.Exclamation.Play();
            }
            return false;
          }

          FileInfo = Doc.ASMFileInfo;

          Core.MainForm.AddOutputMessages( parser );

          var compileTarget = Core.DetermineTargetType( Doc, parser );
          string compileTargetFile = Core.DetermineTargetFilename( Doc, parser );

          if ( ConfigSetting != null )
          {
            lock ( Doc.DeducedDependency )
            {
              currentBuildState[Doc.FullPath].TargetFile = compileTargetFile;
              currentBuildState[Doc.FullPath].TargetType = compileTarget;
              AppendBuildStates( currentBuildState, Doc.DeducedDependency[ConfigSetting].BuildState );
            }
          }
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
            Core.ShowDocument( Core.MainForm.m_CompileResult, false );
            Core.MainForm.AppState = Types.StudioState.NORMAL;

            if ( Core.Settings.PlaySoundOnBuildFailure )
            {
              System.Media.SystemSounds.Exclamation.Play();
            }
            return false;
          }
          BuildInfo.TargetFile = compileTargetFile;
          BuildInfo.TargetType = compileTarget;

          Doc.LastBuildInfo = BuildInfo;

          if ( parser.Warnings > 0 )
          {
            if ( OutputMessages )
            {
              Core.Navigating.UpdateFromMessages( parser.Messages,
                                              ( parser is Parser.ASMFileParser ) ? ( (Parser.ASMFileParser)parser ).ASMFileInfo : null,
                                              Doc.Project );

              Core.MainForm.m_CompileResult.UpdateFromMessages( parser, Doc.Project );
            }
            Core.ShowDocument( Core.MainForm.m_CompileResult, false );
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
          DisplayMemoryMap( parser, assemblyEndAddress );
          Core.AddToOutput( "Compiled to file " + BuildInfo.TargetFile + ", " + parser.AssembledOutput.Assembly.Length + " bytes" + System.Environment.NewLine );
        }

        if ( ( configSetting != null )
        &&   ( configSetting.PostBuildChain.Active ) )
        {
          if ( !BuildChain( configSetting.PostBuildChain, "post build chain", ConfigSetting, OutputMessages ) )
          {
            return false;
          }
          lock ( Doc.DeducedDependency )
          {
            AppendBuildStates( currentBuildState, Doc.DeducedDependency[ConfigSetting].BuildState );
          }
        }

        // store combined build state info in document
        if ( ConfigSetting != null )
        {
          lock ( Doc.DeducedDependency )
          {
            Doc.DeducedDependency[ConfigSetting].BuildState = currentBuildState;
            Doc.DeducedDependency[ConfigSetting].BuildState.Remove( Doc.FullPath );
          }
        }

        if ( ( configSetting != null )
        &&   ( !string.IsNullOrEmpty( configSetting.PostBuild ) ) )
        {
          Core.ShowDocument( Core.MainForm.m_Output, false );
          Core.AddToOutput( "Running post build step on " + Doc.Element.Name + System.Environment.NewLine );
          if ( !Core.Executing.RunCommand( Doc, "post build", configSetting.PostBuild ) )
          {
            return false;
          }
        }

        BuildInfo.TimeStampOfTargetFile = Core.Compiling.FileLastWriteTime( BuildInfo.TargetFile );
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

        if ( fileInfo != null )
        {
          // spread asm info to all participating files
          foreach ( var document in Core.MainForm.DocumentInfos )
          {
            if ( document != null )
            {
              if ( fileInfo.ContainsFile( document.FullPath ) )
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



    private void DisplayMemoryMap( Parser.ParserBase parser, int assemblyEndAddress )
    {
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
    }



    private void AppendBuildStates( Map<string, SingleBuildInfo> CurrentBuildState, Map<string, SingleBuildInfo> NewBuildState )
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
        SingleBuildInfo               buildInfo;
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

        lock ( m_DocumentToBuild.DeducedDependency )
        {
          if ( !m_DocumentToBuild.DeducedDependency[ParentDocumentConfigSetting].BuildState.ContainsKey( element.DocumentInfo.FullPath ) )
          {
            m_DocumentToBuild.DeducedDependency[ParentDocumentConfigSetting].BuildState.Add( element.DocumentInfo.FullPath, new SingleBuildInfo() );
          }
          m_DocumentToBuild.DeducedDependency[ParentDocumentConfigSetting].BuildState[element.DocumentInfo.FullPath].TimeStampOfSourceFile = Core.Compiling.FileLastWriteTime( element.DocumentInfo.FullPath );
        }
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
