using GR.Memory;
using RetroDevStudio.Formats;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RetroDevStudio
{
  static class Program
  {
    [System.Runtime.InteropServices.DllImport( "user32.dll" )]
    private static extern bool SetProcessDPIAware();

    public static bool            s_Exiting = false;



    [STAThread]
    static void Main( string[] args )
    {
      CPCDSK  disk = new CPCDSK();

      disk.CreateEmptyMedia();

      ByteBuffer  file = new ByteBuffer( "0D000A00BF2268656C6C6F22000000" );

      disk.WriteFile( new ByteBuffer( "42415349432020204E424153" ), file, Types.FileType.PRG );
      disk.Save( "diskwithfile.dsk" );

      Debug.Log( disk.LastError );

#if !DEBUG
      try
      {
#endif
        if ( Environment.OSVersion.Version.Major >= 6 )
        {
          SetProcessDPIAware();
        }
        Application.EnableVisualStyles();
#if NET6_0
        Application.SetHighDpiMode(HighDpiMode.SystemAware);
#endif
        Application.SetCompatibleTextRenderingDefault( false );
        Application.Run( new MainForm( args ) );
#if !DEBUG
      }
      catch ( Exception ex )
      {
        if ( !s_Exiting )
        {
          string    exceptionInfo = ex.ToString();
          System.Windows.Forms.Clipboard.SetText( exceptionInfo );
          System.Windows.Forms.MessageBox.Show( "I'm terribly sorry, an unexpected error occurred.\r\nPlease forward the text of this message box (already copied to the clipboard) to the developer to get the problem fixed.\r\n\r\nThank you for using C64 Studio!\r\n\r\n" + ex.ToString(), "An unexpected error occurred!", MessageBoxButtons.OK );
        }      
      }
#endif
    }

  }
}
