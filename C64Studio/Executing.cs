using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.ComponentModel;

namespace C64Studio
{
  public class Executing
  {
    [DllImport( "USER32.DLL" )]
    public static extern bool SetForegroundWindow( IntPtr hWnd );



    public StudioCore                   Core = null;

    public System.Diagnostics.Process   RunProcess = null;

    private System.Diagnostics.Process  m_ExternalProcess = null;

    private System.DateTime             m_LastReceivedOutputTime;



    public Executing( StudioCore Core )
    {
      this.Core = Core;
    }



    public void BringToForeground()
    {
      if ( ( RunProcess != null )
      &&   ( RunProcess.MainWindowHandle != IntPtr.Zero ) )
      {
        SetForegroundWindow( RunProcess.MainWindowHandle );
      }
    }




    internal bool StartProcess( ToolInfo toolRun, DocumentInfo Document )
    {
      bool  error = false;

      RunProcess = new System.Diagnostics.Process();
      RunProcess.StartInfo.FileName = toolRun.Filename;
      RunProcess.StartInfo.WorkingDirectory = Core.MainForm.FillParameters( toolRun.WorkPath, Document, true, out error );
      RunProcess.EnableRaisingEvents = true;

      return !error;
    }




    internal void BringStudioToForeground()
    {
      SetForegroundWindow( Core.MainForm.Handle );
    }



    public bool RunCommand( DocumentInfo Doc, string StepDesc, string Command )
    {
      if ( !RunExternalCommand( Doc, Command ) )
      {
        Core.AddToOutput( "-" + StepDesc + " step failed" + System.Environment.NewLine );
        return false;
      }
      Core.AddToOutput( "-" + StepDesc + " step successful" + System.Environment.NewLine );
      return true;
    }



    private bool RunExternalCommand( string Command, DocumentInfo CommandDocument )
    {
      m_LastReceivedOutputTime = System.DateTime.Now;

      string fullCommand = Command;
      string args = "";
      if ( Command.StartsWith( "\"" ) )
      {
        int nextQuote = Command.IndexOf('"', 1);
        if ( nextQuote == -1 )
        {
          // invalid file
          Core.AddToOutput( "Invalid command specified (" + Command + ")" );
          return false;
        }
        fullCommand = Command.Substring( 1, nextQuote - 1 );
        args = Command.Substring( nextQuote + 1 ).Trim();
      }
      else if ( Command.IndexOf( ' ' ) != -1 )
      {
        int spacePos = Command.IndexOf(' ');
        fullCommand = Command.Substring( 0, spacePos );
        args = Command.Substring( spacePos + 1 ).Trim();
      }

      fullCommand = "cmd.exe";

      bool error = false;
      bool errorAtArgs = false;

      string command = Core.MainForm.FillParameters( Command, CommandDocument, false, out error );
      if ( error )
      {
        return false;
      }
      args = "/C \"" + command + "\"";
      args = Core.MainForm.FillParameters( args, CommandDocument, false, out errorAtArgs );
      if ( ( error )
      ||   ( errorAtArgs ) )
      {
        return false;
      }

      Core.AddToOutput( command + System.Environment.NewLine );

      m_ExternalProcess = new System.Diagnostics.Process();
      m_ExternalProcess.StartInfo.FileName = fullCommand;
      m_ExternalProcess.StartInfo.WorkingDirectory = Core.MainForm.FillParameters( "$(BuildTargetPath)", CommandDocument, false, out error );

      if ( error )
      {
        return false;
      }
      if ( !System.IO.Directory.Exists( m_ExternalProcess.StartInfo.WorkingDirectory + "/" ) )
      {
        Core.AddToOutput( "The determined working directory \"" + m_ExternalProcess.StartInfo.WorkingDirectory + "\" does not exist" + System.Environment.NewLine );
        return false;
      }

      m_ExternalProcess.StartInfo.CreateNoWindow = true;
      m_ExternalProcess.EnableRaisingEvents = true;
      m_ExternalProcess.StartInfo.Arguments = args;
      m_ExternalProcess.StartInfo.UseShellExecute = false;

      m_ExternalProcess.OutputDataReceived += new System.Diagnostics.DataReceivedEventHandler( ExternalProcessOutputReceived );
      m_ExternalProcess.ErrorDataReceived += new System.Diagnostics.DataReceivedEventHandler( ExternalProcessOutputReceived );
      m_ExternalProcess.StartInfo.RedirectStandardError = true;
      m_ExternalProcess.StartInfo.RedirectStandardOutput = true;

      try
      {
        if ( !m_ExternalProcess.Start() )
        {
          m_ExternalProcess.Close();
          return false;
        }
        m_ExternalProcess.BeginOutputReadLine();
        m_ExternalProcess.BeginErrorReadLine();
      }
      catch ( Win32Exception ex )
      {
        m_ExternalProcess.Close();
        Core.AddToOutput( ex.Message + System.Environment.NewLine );
        return false;
      }

      while ( !m_ExternalProcess.WaitForExit( 5 ) )
      {
        Application.DoEvents();
      }
      // DO NOT REMOVE: final DoEvents to let the app clear its invoke queue to the output display, plus WaitForExit is required so all output is received
      Application.DoEvents();
      m_ExternalProcess.WaitForExit();

      bool success = (m_ExternalProcess.ExitCode == 0);
      if ( !success )
      {
        Core.AddToOutput( "External Command " + command + " exited with result code " + m_ExternalProcess.ExitCode.ToString() + System.Environment.NewLine );
      }
      m_ExternalProcess.Close();
      return success;
    }



    private bool RunExternalCommand( DocumentInfo Doc, string Command )
    {
      string[] commands = System.Text.RegularExpressions.Regex.Split(Command, System.Environment.NewLine);

      Core.MainForm.SetGUIForWaitOnExternalTool( true );
      foreach ( string command in commands )
      {
        if ( string.IsNullOrEmpty( command.Trim() ) )
        {
          continue;
        }
        if ( !RunExternalCommand( command, Doc ) )
        {
          Core.MainForm.SetGUIForWaitOnExternalTool( false );
          return false;
        }
      }
      Core.MainForm.SetGUIForWaitOnExternalTool( false );
      return true;
    }



    void ExternalProcessOutputReceived( object sender, System.Diagnostics.DataReceivedEventArgs e )
    {
      m_LastReceivedOutputTime = System.DateTime.Now;
      if ( !String.IsNullOrEmpty( e.Data ) )
      {
        Core.AddToOutput( e.Data + System.Environment.NewLine );
      }
    }



  }
}
