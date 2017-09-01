using System;
using System.Runtime.InteropServices;

namespace MDIApp
{
	/// <summary>
	/// Zusammenfassung für DebugLog.
	/// </summary>
	public class dh
	{
    [DllImport( "user32.dll", EntryPoint="FindWindow" ) ]
    private static extern int FindWindow(string _ClassName, string _WindowName);

    [DllImport("user32.dll",EntryPoint="SendMessage")]
    private static extern int SendMessage(int _WindowHandler, int _WM_USER, int _data, int _id);

    struct tagCOPYDATASTRUCT 
    {
      public int dwData;
      public int cbData;
      public System.IntPtr pData;
    };

    static public void Log( string sText )
    {
      int   iWnd = FindWindow( null, "DebugHost V2" );
      if ( iWnd != 0 )
      {
        sText += "\n";
        tagCOPYDATASTRUCT    cds = new tagCOPYDATASTRUCT();
        cds.pData = System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi( sText );
        cds.cbData = sText.Length + 1;
        cds.dwData = 0;

        System.IntPtr   iPtr = System.Runtime.InteropServices.Marshal.AllocHGlobal( 20 );

        System.Runtime.InteropServices.Marshal.StructureToPtr( cds, iPtr, true );

        SendMessage( iWnd, 0x04a, 0, iPtr.ToInt32() );

        System.Runtime.InteropServices.Marshal.FreeHGlobal( iPtr );
        System.Runtime.InteropServices.Marshal.FreeHGlobal( cds.pData );
      }
    }
	}
}
