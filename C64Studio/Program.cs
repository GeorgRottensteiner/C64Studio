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
      // Forum64 checksummer attempt
      int x = 0;
      int fd = 0;
      byte quoteFlag = 0;
      byte fb = 0;
      byte fc = 0;
      var memory = new ByteBuffer( "0A009900" );
      while ( true )
      {
        fd = memory.ByteAt( x );
        if ( fd == 0 )
        {
          //DisplayCheckSum( fb, fc );
          break;
        }
        if ( ( quoteFlag & 0x80 ) != 0 )
        {
          if ( fd == 0x22 ) // ASCII for "
          {
            quoteFlag ^= 0xFF;
          }
        }
        else
        {
          if ( fd == 0x20 ) // ASCII for space
          {
            x++;
            continue;
          }
          if ( fd == 0x22 ) // ASCII for "

          {
            quoteFlag ^= 0xFF;
          }
        }
        for ( int y = 8; y > 0; y-- )
        {
          fd <<= 1;
          fd |= ( ( fb & 0x80 ) >> 7 );
          fb <<= 1;
          fb |= (byte)( ( fc & 0x80 ) >> 7 );
          fc <<= 1;
          if ( ( fd & 0x100 ) != 0 )
          {
            fc ^= 0x68;
          }
        }
        x++;
      }
            /*
            CPCDSK  disk = new CPCDSK();

            disk.CreateEmptyMedia();

            ByteBuffer  file = new ByteBuffer( "0D000A00BF2268656C6C6F22000000" );

            ByteBuffer  fileToInsert = GR.IO.File.ReadAllBytes( @"C:\Users\Georg\Desktop\CPC\FRUITY.BIN" );
            //disk.WriteFile( new ByteBuffer( "42415349432020204E424153" ), file, Types.FileType.PRG );
            disk.WriteFile( new ByteBuffer( "46525549545920204E42494E" ), fileToInsert, Types.FileType.PRG );
            Debug.Log( disk.LastError );
            disk.Save( @"C:\Users\Georg\Desktop\CPC\diskwithfilefruitybin.dsk" );*/


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
