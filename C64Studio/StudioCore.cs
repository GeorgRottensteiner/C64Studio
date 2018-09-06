using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace C64Studio
{
  public class StudioCore
  {
    public MainForm           MainForm = null;
    public Types.StudioState  State = Types.StudioState.NORMAL;
    public StudioSettings     Settings = new StudioSettings();
    public Debugging          Debugging;
    public Imaging            Imaging;
    public Compiling          Compiling;
    public Searching          Searching;
    public Navigating         Navigating;
    public Executing          Executing;
    public Tasks.TaskManager  TaskManager;
    public bool               ShuttingDown = false;
    public static string      StudioVersion = "5.8";

    public static StudioCore  StaticCore = null;



    delegate void AddToOutputCallback( string Text );
    delegate void delSetStatus( string Text, bool ProgressVisible, int ProgressValue );


    public StudioCore()
    {
      Compiling = new Compiling( this );
      Searching = new Searching( this );
      Navigating = new Navigating( this );
      TaskManager = new Tasks.TaskManager( this );
      Debugging = new Debugging( this );
      Executing = new Executing( this );
      Imaging = new Imaging( this );

      StaticCore = this;
    }



    public ToolInfo DetermineTool( DocumentInfo Document, bool Run )
    {
      foreach ( ToolInfo tool in Settings.ToolInfos )
      {
        if ( ( Run )
        && ( tool.Type == ToolInfo.ToolType.EMULATOR )
        && ( tool.Name.ToUpper() == Settings.EmulatorToRun ) )
        {
          //AddToOutput( "Determined tool to run = " + tool.Name );
          return tool;
        }
      }

      // fallback
      foreach ( ToolInfo tool in Settings.ToolInfos )
      {
        if ( ( Run )
        && ( tool.Type == ToolInfo.ToolType.EMULATOR ) )
        {
          //AddToOutput( "fallback emulator = " + tool.Name );
          return tool;
        }
        if ( ( !Run )
        && ( tool.Type == ToolInfo.ToolType.ASSEMBLER ) )
        {
          return tool;
        }
      }
      return null;
    }



    public Parser.ParserBase DetermineParser( DocumentInfo Doc )
    {
      if ( Doc.Type == ProjectElement.ElementType.ASM_SOURCE )
      {
        return Compiling.ParserASM;
      }
      if ( Doc.Type == ProjectElement.ElementType.BASIC_SOURCE )
      {
        return Compiling.ParserBasic;
      }
      return null;
    }



    public Types.CompileTargetType DetermineTargetType( DocumentInfo Doc, Parser.ParserBase Parser )
    {
      // compile target
      Types.CompileTargetType   compileTarget = C64Studio.Types.CompileTargetType.NONE;
      if ( Doc.Element != null )
      {
        compileTarget = Doc.Element.TargetType;
      }
      if ( compileTarget == C64Studio.Types.CompileTargetType.NONE )
      {
        compileTarget = Parser.CompileTarget;
      }
      return compileTarget;
    }



    public string DetermineTargetFilename( DocumentInfo Doc, Parser.ParserBase Parser )
    {
      if ( ( String.IsNullOrEmpty( Parser.CompileTargetFile ) )
      && ( ( Doc.Element == null )
      || ( String.IsNullOrEmpty( Doc.Element.TargetFilename ) ) ) )
      {
        // default to same name.prg and cbm
        if ( Doc.Project == null )
        {
          return System.IO.Path.Combine( System.IO.Path.GetDirectoryName( Doc.FullPath ), System.IO.Path.GetFileNameWithoutExtension( Doc.FullPath ) ) + ".prg";
        }
        return System.IO.Path.Combine( Doc.Project.Settings.BasePath, System.IO.Path.GetFileNameWithoutExtension( Doc.FullPath ) + ".prg" );
      }
      if ( ( Doc.Element != null )
      && ( !String.IsNullOrEmpty( Doc.Element.TargetFilename ) ) )
      {
        return GR.Path.Append( Doc.Project.Settings.BasePath, Doc.Element.TargetFilename );
      }
      return Parser.CompileTargetFile;
    }



    public void ShowDocument( BaseDocument Doc )
    {
      if ( Doc.InvokeRequired )
      {
        Doc.Invoke( new MainForm.DocCallback( ShowDocument ), new object[] { Doc } );
        return;
      }
      if ( !Doc.Visible )
      {
        //Doc.Show();
        Doc.Activate();
      }
      if ( Doc.Pane != null )
      {
        if ( Doc.Pane.ActiveContent != Doc )
        {
          Doc.Pane.ActiveContent = Doc;
        }
      }
    }



    public void AddToOutput( string Text )
    {
      if ( MainForm.InvokeRequired )
      {
        try
        {
          MainForm.Invoke( new AddToOutputCallback( AddToOutput ), new object[] { Text } );
        }
        catch ( System.ObjectDisposedException )
        {
        }
      }
      else
      {
        MainForm.m_Output.AppendText( Text );
      }
    }



    public void SetStatus( string Text, bool ProgressVisible, int ProgressValue )
    {
      if ( MainForm.InvokeRequired )
      {
        MainForm.Invoke( new delSetStatus( SetStatus ), new object[] { Text, ProgressVisible, ProgressValue } );
      }
      else
      {
        MainForm.statusProgress.Visible = ProgressVisible;
        MainForm.statusProgress.Value = ProgressValue;
        MainForm.statusLabelInfo.Text = Text;
      }
    }



    public void SetStatus( string Text )
    {
      if ( MainForm.InvokeRequired )
      {
        MainForm.Invoke( new delSetStatus( SetStatus ), new object[] { Text, false, 0 } );
      }
      else
      {
        MainForm.statusLabelInfo.Text = Text;
      }
    }



    internal void ClearOutput()
    {
      if ( MainForm.InvokeRequired )
      {
        MainForm.Invoke( new MainForm.ParameterLessCallback( ClearOutput ) );
        return;
      }
      MainForm.m_Output.SetText( "" );
    }



  }
}
