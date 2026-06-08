namespace GR.Win32
{
  using System;
  using System.Windows.Forms;
  using System.Runtime.InteropServices;
  public class KeyboardInfo
  {
    private KeyboardInfo()
    {
    }
    [DllImport( "user32" )]
    private static extern short GetKeyState( int vKey );
    public static KeyStateInfo GetKeyState( Keys key )
    {
      short keyState = GetKeyState( (int)key );
      byte[] bits = BitConverter.GetBytes( keyState );
      bool toggled = bits[0] > 0, pressed = bits[1] > 0;
      return new KeyStateInfo( key, pressed, toggled );
    }
  }


  public struct KeyStateInfo
  {
    Keys _key;
    bool _isPressed,
            _isToggled;
    public KeyStateInfo( Keys key,
                    bool ispressed,
                    bool istoggled )
    {
      _key = key;
      _isPressed = ispressed;
      _isToggled = istoggled;
    }
    public static KeyStateInfo Default
    {
      get
      {
        return new KeyStateInfo( Keys.None,
                                    false,
                                    false );
      }
    }
    public Keys Key
    {
      get
      {
        return _key;
      }
    }
    public bool IsPressed
    {
      get
      {
        return _isPressed;
      }
    }
    public bool IsToggled
    {
      get
      {
        return _isToggled;
      }
    }
  }
}
