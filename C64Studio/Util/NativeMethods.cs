using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RetroDevStudio
{
  public class NativeMethods
  {
    // Windows message for clipboard update
    private const int WM_CLIPBOARDUPDATE = 0x031D;
    private static IntPtr HWND_MESSAGE = new IntPtr(-3);

    // Import required WinAPI functions
    [DllImport( "user32.dll", SetLastError = true )]
    public static extern bool AddClipboardFormatListener( IntPtr hwnd );



    [DllImport( "user32.dll", SetLastError = true )]
    public static extern bool RemoveClipboardFormatListener( IntPtr hwnd );

  }
}
