using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace C64Studio
{
  static class Program
  {
    [System.Runtime.InteropServices.DllImport( "user32.dll" )]
    private static extern bool SetProcessDPIAware();



    [STAThread]
    static void Main( string[] args )
    {
#if !DEBUG
      try
      {
#endif
        if ( Environment.OSVersion.Version.Major >= 6 )
        {
          SetProcessDPIAware();
        }

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault( false );
        Application.Run( new MainForm( args ) );
#if !DEBUG
      }
      catch ( Exception ex )
      {
        string    exceptionInfo = ex.ToString();
        System.Windows.Forms.Clipboard.SetText( exceptionInfo );
        System.Windows.Forms.MessageBox.Show( "I'm terribly sorry, an unexpected error occurred.\r\nPlease forward the text of this message box (already copied to the clipboard) to the developer to get the problem fixed.\r\n\r\nThank you for using C64 Studio!\r\n\r\n" + ex.ToString(), "An unexpected error occurred!", MessageBoxButtons.OK );
      }
#endif
    }

  }
}
