using System;
using System.Runtime.InteropServices;

	/// <summary>
	/// Zusammenfassung für DebugLog.
	/// </summary>
	public class Debug
	{
    [DllImport( "user32.dll", EntryPoint="FindWindow" ) ]
    private static extern int FindWindow(string _ClassName, string _WindowName);

    [DllImport("user32.dll",EntryPoint="SendMessage")]
    private static extern int SendMessage(int _WindowHandler, int _WM_USER, IntPtr _data, IntPtr _id );

    struct tagCOPYDATASTRUCT 
    {
      public IntPtr dwData;
      public IntPtr cbData;
      public System.IntPtr pData;
    };

    //[System.Diagnostics.Conditional( "DEBUG" )]  // When needed, uncomment this line to remove debug logging from Release builds
    static public void Log( string sText )
    {
      int   iWnd = FindWindow( null, "DebugHost V2" );
      if ( iWnd != 0 )
      {
        sText += "\n";
        tagCOPYDATASTRUCT    cds = new tagCOPYDATASTRUCT();
        cds.pData = System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi( sText );
        cds.cbData = (IntPtr)( sText.Length + 1 );
        cds.dwData = IntPtr.Zero;

        System.IntPtr   iPtr = System.Runtime.InteropServices.Marshal.AllocHGlobal( System.Runtime.InteropServices.Marshal.SizeOf( typeof( tagCOPYDATASTRUCT ) ) );

        System.Runtime.InteropServices.Marshal.StructureToPtr( cds, iPtr, true );

        SendMessage( iWnd, 0x04a, IntPtr.Zero, iPtr );

        System.Runtime.InteropServices.Marshal.FreeHGlobal( iPtr );
        System.Runtime.InteropServices.Marshal.FreeHGlobal( cds.pData );
      }
      else
      {
        System.Diagnostics.Trace.WriteLine( sText );
      }
    }
	}
