using RetroDevStudio.Displayer;
using RetroDevStudio;
using System.Drawing;
using System.Runtime.InteropServices;
using System;
using System.Windows.Forms;



namespace RetroDevStudio
{
  public class FocusSupport
  {
    [DllImport( "user32.dll" )]
    static extern IntPtr GetFocus();

    public static Control GetFocusedControl()
    {
      IntPtr  wndHandle = GetFocus();
      return Control.FromChildHandle( wndHandle );
    }



    public enum FocusControlReason
    {
      COPY_PASTE,
      ESCAPE
    }



    /// <summary>
    /// looks whether the focused control is a child of the passed control, 
    ///   and is not a control that could use a key for the given reason 
    ///       copy/paste (currently only TextBox?)
    ///       escape (currently only ComboBox?)
    /// </summary>
    /// <param name="Control"></param>
    /// <returns>true if control is child and capable of handling copy/paste</returns>
    public static bool IsFocusOnChildOfAndCouldAffectReason( Control Control, FocusControlReason Reason )
    {
      var focusedControl = GetFocusedControl();
      bool isTextBox = ( focusedControl is TextBox );
      bool isComboBox = ( focusedControl is ComboBox );

      while ( focusedControl != null )
      {
        if ( focusedControl == Control )
        {
          switch ( Reason )
          {
            case FocusControlReason.COPY_PASTE:
              // text box does copy/paste
              return isTextBox;
            case FocusControlReason.ESCAPE:
              // combo box does escape (close popup)
              return isComboBox;
          }
          return isTextBox;
        }
        focusedControl = focusedControl.Parent;
      }
      // not a child of our check container
      return true;
    }

  }

}
