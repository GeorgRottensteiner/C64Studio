using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;



namespace C64Studio
{
  public class Executing
  {
    [DllImport( "USER32.DLL" )]
    public static extern bool SetForegroundWindow( IntPtr hWnd );



    public StudioCore                   Core = null;

    public System.Diagnostics.Process   RunProcess = null;



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
  }
}
