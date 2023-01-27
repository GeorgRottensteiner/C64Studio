using RetroDevStudio.CustomRenderer;
using RetroDevStudio.Documents;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RetroDevStudio
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
    public const string       StudioVersion = "7.3.5";
    public StudioTheme        Theming;

    public static StudioCore  StaticCore = null;

    private int               SuppressOutputCount = 0;



    delegate void AddToOutputCallback( string Text );
    delegate void MessageBoxCallback( string Text, string Caption );
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
      Theming = new StudioTheme( this );

      StaticCore = this;
    }



    public ToolInfo DetermineTool( DocumentInfo Document, ToolInfo.ToolType RunType )
    {
      if ( Settings.EmulatorToRun == Settings.ToolTiny64.Name.ToUpper() )
      {
        return Settings.ToolTiny64;
      }

      foreach ( ToolInfo tool in Settings.ToolInfos )
      {
        if ( ( tool.Type == RunType )
        &&   ( tool.Name != null )
        &&   ( tool.Name.ToUpper() == Settings.EmulatorToRun ) )
        {
          //AddToOutput( "Determined tool to run = " + tool.Name );
          return tool;
        }
      }

      // fallback
      foreach ( ToolInfo tool in Settings.ToolInfos )
      {
        if ( tool.Type == RunType )
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
      Types.CompileTargetType   compileTarget = RetroDevStudio.Types.CompileTargetType.NONE;
      if ( Doc.Element != null )
      {
        compileTarget = Doc.Element.TargetType;
      }
      if ( compileTarget == RetroDevStudio.Types.CompileTargetType.NONE )
      {
        compileTarget = Parser.CompileTarget;
      }
      return compileTarget;
    }



    public string DetermineTargetFilename( DocumentInfo Doc, Parser.ParserBase Parser )
    {
      if ( ( String.IsNullOrEmpty( Parser.CompileTargetFile ) )
      &&   ( ( Doc.Element == null )
      ||     ( String.IsNullOrEmpty( Doc.Element.TargetFilename ) ) ) )
      {
        // default to same name.prg and cbm
        string    targetExtension = Parser.DefaultTargetExtension;
        if ( Doc.Project == null )
        {
          return System.IO.Path.Combine( System.IO.Path.GetDirectoryName( Doc.FullPath ), System.IO.Path.GetFileNameWithoutExtension( Doc.FullPath ) ) + targetExtension;
        }
        return System.IO.Path.Combine( Doc.Project.Settings.BasePath, System.IO.Path.GetFileNameWithoutExtension( Doc.FullPath ) + targetExtension );
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
      ShowDocument( Doc, true );
    }



    public void ShowDocument( BaseDocument Doc, bool Activate )
    {
      if ( Doc.InvokeRequired )
      {
        Doc.Invoke( new MainForm.DocShowCallback( ShowDocument ), new object[] { Doc, Activate } );
        return;
      }

      if ( Doc.IsHidden )
      {
        Doc.Visible = true;

        if ( Doc.DockHandler.DockPanel == null )
        {
          Doc.DockHandler.Form.Activate();
          return;
        }

        if ( Doc.DockHandler.Pane == null )
        {
          Doc.DockHandler.Show( Doc.DockHandler.DockPanel );
          return;
        }

        Doc.DockHandler.IsHidden = false;
        Doc.DockHandler.Pane.ActiveContent = Doc.DockHandler.Content;
        if ( Doc.DockHandler.DockState == WeifenLuo.WinFormsUI.Docking.DockState.Document && Doc.DockHandler.DockPanel.DocumentStyle == WeifenLuo.WinFormsUI.Docking.DocumentStyle.SystemMdi )
        {
          Doc.DockHandler.Form.Activate();
        }
        else if ( WeifenLuo.WinFormsUI.Docking.DockHelper.IsDockStateAutoHide( Doc.DockHandler.DockState ) && Doc.DockHandler.DockPanel.ActiveAutoHideContent != Doc.DockHandler.Content )
        {
          Doc.DockHandler.DockPanel.ActiveAutoHideContent = null;
        }
        else if ( !Doc.DockHandler.Form.ContainsFocus && !WeifenLuo.WinFormsUI.Docking.Win32Helper.IsRunningOnMono )
        {
          // did this steal the focus if the content was hidden?
          /*
          if ( Activate )
          {
            Doc.DockHandler.DockPanel.ContentFocusManager.Activate( Doc.DockHandler.Content );
          }*/
        }
      }
      if ( ( Activate )
      &&   ( !Doc.IsActivated ) )
      {
        Doc.Activate();
      }
      /*
      if ( !Doc.Visible )
      {
        Debug.Log( "Show for " + Doc.Name );
        Doc.Show();
      }*/
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
      if ( SuppressOutputCount > 0 )
      {
        return;
      }

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



    public void MessageBox( string Text, string Caption )
    {
      if ( SuppressOutputCount > 0 )
      {
        return;
      }

      if ( MainForm.InvokeRequired )
      {
        try
        {
          MainForm.Invoke( new MessageBoxCallback( MessageBox ), new object[] { Text, Caption } );
        }
        catch ( System.ObjectDisposedException )
        {
        }
      }
      else
      {
        System.Windows.Forms.MessageBox.Show( MainForm, Text, Caption );
      }
    }



    public void SetStatus( string Text, bool ProgressVisible, int ProgressValue )
    {
      try
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
      catch ( Exception )
      {
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
      if ( SuppressOutputCount > 0 )
      {
        return;
      }
      if ( MainForm.InvokeRequired )
      {
        MainForm.Invoke( new MainForm.ParameterLessCallback( ClearOutput ) );
        return;
      }
      MainForm.m_Output.SetText( "" );
    }



    public void Initialise()
    {
      Settings.Perspectives = new PerspectiveDetails( this );
      Compiling.Initialise();
    }



    public void SuppressOutput()
    {
      ++SuppressOutputCount;
    }



    public void UnsuppressOutput()
    {
      if ( SuppressOutputCount > 0 )
      {
        --SuppressOutputCount;
      }
    }



  }
}
