using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio.Tasks
{
  public class TaskBuildElement : Task
  {
    private DocumentInfo    m_Document;
    private ProjectConfig   m_Configuration;



    public TaskBuildElement( DocumentInfo Document, ProjectConfig Configuration )
    {
      m_Document = Document;
      m_Configuration = Configuration;
    }



    protected override bool ProcessTask()
    {
      Parser.ASMFileParser parser = new Parser.ASMFileParser();

      var compileConfig = new C64Studio.Parser.CompileConfig();
      compileConfig.Assembler = m_Document.Element.AssemblerType;

      parser.ParseFile( m_Document, m_Configuration, compileConfig );

      return true;
    }



    bool BuildElement( DocumentInfo Doc, string ConfigSetting, string AdditionalPredefines, out Types.BuildInfo BuildInfo, out Types.ASM.FileInfo FileInfo )
    {
      BuildInfo = new C64Studio.Types.BuildInfo();

      BuildInfo.TargetFile = "";
      BuildInfo.TargetType = Types.CompileTargetType.NONE;

      FileInfo = null;

      Types.ASM.FileInfo combinedFileInfo = null;

      if ( Doc.Element != null )
      {
        Doc.Element.CompileTarget = Types.CompileTargetType.NONE;
        Doc.Element.CompileTargetFile = null;

        // check dependencies
        foreach ( var dependency in Doc.Element.ForcedDependency.DependentOnFile )
        {
          ProjectElement elementDependency = Doc.Project.GetElementByFilename( dependency.Filename );
          if ( elementDependency == null )
          {
            Core.AddToOutput( "Could not find dependency for " + dependency.Filename + System.Environment.NewLine );
            return false;
          }

          Types.ASM.FileInfo    dependencyFileInfo = null;

          // skip building if not required
          if ( !Core.Compiling.NeedsRebuild( elementDependency.DocumentInfo, ConfigSetting ) )
          {
            Core.AddToOutput( "Dependency " + dependency.Filename + " is current for config " + ConfigSetting + System.Environment.NewLine );

            if ( Doc.Type == ProjectElement.ElementType.ASM_SOURCE )
            {
              dependencyFileInfo = elementDependency.DocumentInfo.ASMFileInfo;
              //Debug.Log( "Doc " + Doc.Text + " receives " + dependencyFileInfo.Labels.Count + " dependency labels from dependency " + dependency.Filename );
            }
          }
          else
          {
            Types.BuildInfo tempInfo = new C64Studio.Types.BuildInfo();

            if ( !BuildElement( elementDependency.DocumentInfo, ConfigSetting, null, out tempInfo, out dependencyFileInfo ) )
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

      if ( !Doc.Compilable )
      {
        // not buildable 
        // TODO - Autoexport?
        return true;
      }

      ToolInfo tool = Core.DetermineTool( Doc, false );

      ProjectElement.PerConfigSettings configSetting = null;

      Parser.ParserBase parser = Core.DetermineParser( Doc );

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
          if ( !Main.RunCommand( Doc, "pre build", configSetting.PreBuild ) )
          {
            return false;
          }
        }
        if ( configSetting.PreBuildChain.Active )
        {
          if ( !Main.BuildChain( configSetting.PreBuildChain, "pre build chain" ) )
          {
            return false;
          }
        }
        Core.AddToOutput( "Running build on " + Doc.Element.Name + " with configuration " + ConfigSetting + System.Environment.NewLine );
      }
      else
      {
        Core.AddToOutput( "Running build on " + Doc.DocumentFilename + System.Environment.NewLine );
      }

      // include previous symbols
      if ( parser is Parser.ASMFileParser )
      {
        ( (Parser.ASMFileParser)parser ).InitialFileInfo = combinedFileInfo;
        if ( combinedFileInfo != null )
        {
          //Debug.Log( "Doc " + Doc.Text + " receives " + combinedFileInfo.Labels.Count + " initial labels" );
        }
        if ( !string.IsNullOrEmpty( AdditionalPredefines ) )
        {
          ( (Parser.ASMFileParser)parser ).ParseAndAddPreDefines( AdditionalPredefines );
        }
      }

      if ( ( configSetting != null )
      && ( !string.IsNullOrEmpty( configSetting.CustomBuild ) ) )
      {
        Core.AddToOutput( "Running custom build step on " + Doc.Element.Name + " with configuration " + ConfigSetting + System.Environment.NewLine );
        if ( !Main.RunCommand( Doc, "custom build", configSetting.CustomBuild ) )
        {
          return false;
        }
      }
      else
      {
        //EnsureFileIsParsed();
        //AddTask( new C64Studio.Tasks.TaskParseFile( Doc, config ) );

        ProjectConfig config = null;
        if ( Doc.Project != null )
        {
          config = Doc.Project.Settings.Configs[ConfigSetting];
        }

        if ( ( !Main.ParseFile( parser, Doc, config ) )
        ||   ( !parser.Assemble( new C64Studio.Parser.CompileConfig()
        {
          TargetType = Core.DetermineTargetType( Doc, parser ),
          OutputFile = Core.DetermineTargetFilename( Doc, parser ),
          AutoTruncateLiteralValues = Core.Settings.ASMAutoTruncateLiteralValues
        } ) )
        || ( parser.Errors > 0 ) )
        {
          Main.AddOutputMessages( parser );

          Core.AddToOutput( "Build failed, " + parser.Warnings.ToString() + " warnings, " + parser.Errors.ToString() + " errors encountered" + System.Environment.NewLine );
          Core.Navigating.UpdateFromMessages( parser.Messages,
                                                    ( parser is Parser.ASMFileParser ) ? ( (Parser.ASMFileParser)parser ).ASMFileInfo : null,
                                                    Doc.Project );
          Main.m_CompileResult.UpdateFromMessages( parser, Doc.Project );
          if ( !Main.m_CompileResult.Visible )
          {
            Main.m_CompileResult.Show();
          }
          Main.AppState = Types.StudioState.NORMAL;

          if ( Core.Settings.PlaySoundOnBuildFailure )
          {
            System.Media.SystemSounds.Exclamation.Play();
          }
          return false;
        }
        Main.AddOutputMessages( parser );

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
          Core.Navigating.UpdateFromMessages( parser.Messages,
                                          ( parser is Parser.ASMFileParser ) ? ( (Parser.ASMFileParser)parser ).ASMFileInfo : null,
                                          Doc.Project );

          Main.m_CompileResult.UpdateFromMessages( parser, Doc.Project );
          if ( !Main.m_CompileResult.Visible )
          {
            Main.m_CompileResult.Show();
          }
          Main.AppState = Types.StudioState.NORMAL;

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
          Core.Navigating.UpdateFromMessages( parser.Messages,
                                          ( parser is Parser.ASMFileParser ) ? ( (Parser.ASMFileParser)parser ).ASMFileInfo : null,
                                          Doc.Project );

          Main.m_CompileResult.UpdateFromMessages( parser, Doc.Project );

          if ( !Main.m_CompileResult.Visible )
          {
            Main.m_CompileResult.Show();
          }
        }
      }

      if ( string.IsNullOrEmpty( BuildInfo.TargetFile ) )
      {
        Core.AddToOutput( "No target file name specified" + System.Environment.NewLine );
        Main.AppState = Types.StudioState.NORMAL;
        if ( Core.Settings.PlaySoundOnBuildFailure )
        {
          System.Media.SystemSounds.Exclamation.Play();
        }
        return false;
      }
      // write output if applicable
      if ( parser.Assembly != null )
      {
        try
        {
          System.IO.File.WriteAllBytes( BuildInfo.TargetFile, parser.Assembly.Data() );
        }
        catch ( System.Exception ex )
        {
          Core.AddToOutput( "Build failed, Could not create output file " + parser.CompileTargetFile + System.Environment.NewLine );
          Core.AddToOutput( ex.ToString() + System.Environment.NewLine );
          Main.AppState = Types.StudioState.NORMAL;
          if ( Core.Settings.PlaySoundOnBuildFailure )
          {
            System.Media.SystemSounds.Exclamation.Play();
          }
          return false;
        }
        Core.AddToOutput( "Build successful, " + parser.Warnings.ToString() + " warnings, 0 errors encountered, compiled to file " + BuildInfo.TargetFile + ", " + parser.Assembly.Length + " bytes" + System.Environment.NewLine );

        //Debug.Log( "File " + Doc.DocumentFilename + " was rebuilt for config " + ConfigSetting + " this round" );
      }

      if ( ( configSetting != null )
      &&   ( configSetting.PostBuildChain.Active ) )
      {
        if ( !Main.BuildChain( configSetting.PostBuildChain, "post build chain" ) )
        {
          return false;
        }
      }


      if ( ( configSetting != null )
      && ( !string.IsNullOrEmpty( configSetting.PostBuild ) ) )
      {
        Main.m_Output.Show();

        Core.AddToOutput( "Running post build step on " + Doc.Element.Name + System.Environment.NewLine );
        if ( !Main.RunCommand( Doc, "post build", configSetting.PostBuild ) )
        {
          return false;
        }
      }

      Doc.HasBeenSuccessfullyBuilt = true;

      if ( parser is Parser.ASMFileParser )
      {
        FileInfo = ( (Parser.ASMFileParser)parser ).ASMFileInfo;
        // update symbols in main asm file
        Doc.SetASMFileInfo( FileInfo, parser.KnownTokens(), parser.KnownTokenInfo() );
        //Debug.Log( "Doc " + Doc.Text + " gets " + ( (SourceASM)Doc ).ASMFileInfo.Labels.Count + " labels" );
      }

      if ( FileInfo != null )
      {
        if ( !string.IsNullOrEmpty( FileInfo.LabelDumpFile ) )
        {
          Main.DumpLabelFile( FileInfo );
        }
      }

      Core.Compiling.m_RebuiltFiles.Add( Doc.DocumentFilename );
      return true;
    }

  }
}
