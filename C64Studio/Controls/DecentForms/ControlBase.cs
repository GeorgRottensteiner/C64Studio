using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Automation;
using System.Windows.Automation.Provider;
using System.Windows.Forms;



namespace DecentForms
{
  public class ControlBase : Control, IRawElementProviderSimple
  {
    private bool          _MouseOver = false;
    private bool          _MouseDown = false;
    private int           _AccumulatedMouseWheelDelta = 0;
    private Point         _LastMousePos = new Point();

    private BorderStyle   _BorderStyle = BorderStyle.FLAT;

    public bool           CanBeFocused = true;

    protected int         _DisplayOffsetX = 0;
    protected int         _DisplayOffsetY = 0;
    protected int         _ActualWorkWidth = 0;
    protected int         _ActualWorkHeight = 0;



    public ControlBase()
    {
      TabStop = true;
      DoubleBuffered = true;
      AutoSize = false;
      this.SetStyle( ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true );
    }



    public event CustomDrawEventHandler    CustomDraw;



    public int DisplayOffsetX
    {
      get
      {
        return _DisplayOffsetX;
      }
    }



    public int DisplayOffsetY
    {
      get
      {
        return _DisplayOffsetY;
      }
    }



    public bool MouseOver
    {
      get
      {
        return _MouseOver;
      }
    }



    public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
    public static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
    public static readonly IntPtr HWND_TOP = new IntPtr(0);
    public static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

    [DllImport( "user32.dll", SetLastError = true )]
    public static extern bool SetWindowPos( IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, UInt32 uFlags );



    public static IntPtr GetWindowLong( IntPtr hWnd, int nIndex )
    {
      if ( IntPtr.Size == 4 )
      {
        return GetWindowLong32( hWnd, nIndex );
      }
      return GetWindowLongPtr64( hWnd, nIndex );
    }


    [DllImport( "user32.dll", EntryPoint = "GetWindowLong", CharSet = CharSet.Auto )]
    private static extern IntPtr GetWindowLong32( IntPtr hWnd, int nIndex );

    [DllImport( "user32.dll", EntryPoint = "GetWindowLongPtr", CharSet = CharSet.Auto )]
    private static extern IntPtr GetWindowLongPtr64( IntPtr hWnd, int nIndex );

    // This static method is required because legacy OSes do not support
    // SetWindowLongPtr
    public static IntPtr SetWindowLongPtr( IntPtr hWnd, int nIndex, IntPtr dwNewLong )
    {
      if ( IntPtr.Size == 8 )
        return SetWindowLongPtr64( hWnd, nIndex, dwNewLong );
      else
        return new IntPtr( SetWindowLong32( hWnd, nIndex, dwNewLong.ToInt32() ) );
    }

    [DllImport( "user32.dll", EntryPoint = "SetWindowLong" )]
    private static extern int SetWindowLong32( IntPtr hWnd, int nIndex, int dwNewLong );

    [DllImport( "user32.dll", EntryPoint = "SetWindowLongPtr" )]
    private static extern IntPtr SetWindowLongPtr64( IntPtr hWnd, int nIndex, IntPtr dwNewLong );

    public enum WindowLongFlags : int
    {
      GWL_EXSTYLE = -20,
      GWLP_HINSTANCE = -6,
      GWLP_HWNDPARENT = -8,
      GWL_ID = -12,
      GWL_STYLE = -16,
      GWL_USERDATA = -21,
      GWL_WNDPROC = -4,
      DWLP_USER = 0x8,
      DWLP_MSGRESULT = 0x0,
      DWLP_DLGPROC = 0x4
    }

    public enum WindowStyleFlags : long
    {
      WS_VISIBLE    = 0x10000000L,
      WS_BORDER     = 0x00800000L,
      WS_CAPTION    = 0x00C00000L,
      WS_SYSMENU    = 0x00080000L,
      WM_CAPTION    = 0x00C00000L,
      WS_DLGFRAME   = 0x00400000L
    }

    public enum ExtendedWindowStyleFlags : long
    {
      WS_EX_CLIENTEDGE = 0x00000200L
    }



    public static class SetWindowPosFlags
    {
      public static readonly uint NOSIZE = 0x0001,
                                  NOMOVE = 0x0002,
                                  NOZORDER = 0x0004,
                                  NOREDRAW = 0x0008,
                                  NOACTIVATE = 0x0010,
                                  DRAWFRAME = 0x0020,
                                  FRAMECHANGED = 0x0020,
                                  SHOWWINDOW = 0x0040,
                                  HIDEWINDOW = 0x0080,
                                  NOCOPYBITS = 0x0100,
                                  NOOWNERZORDER = 0x0200,
                                  NOREPOSITION = 0x0200,
                                  NOSENDCHANGING = 0x0400,
                                  DEFERERASE = 0x2000,
                                  ASYNCWINDOWPOS = 0x4000;
    }

    public BorderStyle BorderStyle
    {
      get
      {
        return _BorderStyle;
      }
      set
      {
        if ( _BorderStyle != value )
        {
          _BorderStyle = value;

          uint  windowStyles = (uint)GetWindowLong( Handle, (int)WindowLongFlags.GWL_STYLE );
          uint  windowStylesEx = (uint)GetWindowLong( Handle, (int)WindowLongFlags.GWL_EXSTYLE );

          switch ( _BorderStyle )
          {
            case BorderStyle.FLAT:
              windowStylesEx &= ~(uint)ExtendedWindowStyleFlags.WS_EX_CLIENTEDGE;
              windowStyles |= (uint)WindowStyleFlags.WS_BORDER;
              windowStyles &= ~(uint)WindowStyleFlags.WS_DLGFRAME;
              break;
            case BorderStyle.NONE:
              windowStylesEx &= ~(uint)ExtendedWindowStyleFlags.WS_EX_CLIENTEDGE;
              windowStyles &= ~(uint)( WindowStyleFlags.WS_DLGFRAME | WindowStyleFlags.WS_BORDER );
              break;
          }
          SetWindowLongPtr( Handle, (int)WindowLongFlags.GWL_STYLE, (IntPtr)windowStyles );
          SetWindowLongPtr( Handle, (int)WindowLongFlags.GWL_EXSTYLE, (IntPtr)windowStylesEx );

          // force repaint/recalc client size
          SetWindowPos( Handle, HWND_TOP, 0, 0, 0, 0, SetWindowPosFlags.DRAWFRAME
                | SetWindowPosFlags.NOACTIVATE
                | SetWindowPosFlags.NOMOVE
                | SetWindowPosFlags.NOSIZE
                | SetWindowPosFlags.NOZORDER );

          Invalidate( true );
        }
      }
    }



    public void Invalidate( GR.Math.Rectangle rect )
    {
      rect.Offset( -_DisplayOffsetX, -_DisplayOffsetY );

      base.Invalidate( new System.Drawing.Rectangle( rect.Left, rect.Top, rect.Width, rect.Height ) );
    }



    public int ActualWorkWidth
    {
      get
      {
        if ( _ActualWorkWidth == 0 )
        {
          return ClientSize.Width;
        }
        return _ActualWorkWidth;
      }
    }



    public int ActualWorkHeight
    {
      get
      {
        if ( _ActualWorkHeight == 0 )
        {
          return ClientSize.Height;
        }
        return _ActualWorkHeight;
      }
    }



    public bool IsOutside( int X, int Y )
    {
      if ( ( X < 0 )
      || ( X >= Width )
      || ( Y < 0 )
      || ( Y >= Height ) )
      {
        return true;
      }
      return false;
    }



    public bool RaiseControlEvent( ControlEvent Event )
    {
      OnControlEvent( Event );

      return Event.Handled;
    }



    public bool RaiseControlEvent( ControlEvent.EventType Type, int MouseX = -1, int MouseY = -1, uint MouseButtons = 0, int MouseWheelDelta = 0 )
    {
      var newEvent = new ControlEvent()
      {
        Type            = Type,
        MouseX          = MouseX,
        MouseY          = MouseY,
        MouseButtons    = MouseButtons,
        MouseWheelDelta = MouseWheelDelta
      };
      OnControlEvent( newEvent );

      return newEvent.Handled;
    }



    public bool RaiseControlEvent( ControlEvent.EventType Type, Keys Key )
    {
      var newEvent = new ControlEvent()
      {
        Type  = Type,
        Key   = Key
      };

      OnControlEvent( newEvent );

      return newEvent.Handled;
    }



    protected virtual void OnControlEvent( ControlEvent Event )
    {
      switch ( Event.Type )
      {
        case ControlEvent.EventType.MOUSE_LEAVE:
          if ( _MouseOver )
          {
            _MouseOver = false;
            Invalidate();
          }
          break;
        case ControlEvent.EventType.MOUSE_UPDATE:
          if ( IsOutside( Event.MouseX, Event.MouseY ) )
          {
            if ( _MouseOver )
            {
              _MouseOver = false;
              RaiseControlEvent( ControlEvent.EventType.MOUSE_LEAVE, Event.MouseX, Event.MouseY, Event.MouseButtons );
              Invalidate();
            }
          }
          else
          {
            if ( !_MouseOver )
            {
              _MouseOver = true;
              RaiseControlEvent( ControlEvent.EventType.MOUSE_ENTER, Event.MouseX, Event.MouseY, Event.MouseButtons );
            }
            if ( ( Event.MouseButtons & 1 ) == 1 )
            {
              if ( _MouseOver )
              {
                if ( !_MouseDown )
                {
                  _MouseDown = true;
                  RaiseControlEvent( ControlEvent.EventType.MOUSE_DOWN, Event.MouseX, Event.MouseY, Event.MouseButtons );
                }
              }
            }
          }

          if ( ( Event.MouseButtons & 1 ) == 0 )
          {
            if ( _MouseDown )
            {
              _MouseDown = false;
              RaiseControlEvent( ControlEvent.EventType.MOUSE_UP, Event.MouseX, Event.MouseY, Event.MouseButtons );
            }
          }
          break;
      }
    }



    protected override void OnTextChanged( EventArgs e )
    {
      Invalidate();
      base.OnTextChanged( e );
    }



    protected sealed override void OnPaint( PaintEventArgs e )
    {
      var renderer = new ControlRenderer( e.Graphics, this );
      if ( CustomDraw != null )
      {
        CustomDraw.Invoke( renderer );
      }
      else
      {
        OnPaint( renderer );
      }
    }



    protected virtual void OnPaint( ControlRenderer Renderer )
    {
    }



    protected override void OnMouseDown( MouseEventArgs e )
    {
      if ( !_MouseDown )
      {
        _MouseDown = true;
        _LastMousePos = e.Location;
        RaiseControlEvent( ControlEvent.EventType.MOUSE_DOWN, e.X, e.Y, ToButtonBitMask( e.Button ) );
      }
      base.OnMouseDown( e );
    }



    protected override void OnMouseUp( MouseEventArgs e )
    {
      if ( _MouseDown )
      {
        _MouseDown = false;
        _LastMousePos = e.Location;
        RaiseControlEvent( ControlEvent.EventType.MOUSE_UP, e.X, e.Y, ToButtonBitMask( e.Button ) );
      }
      base.OnMouseDown( e );
    }



    protected override void OnMouseEnter( EventArgs e )
    {
      if ( !_MouseOver )
      {
        _MouseOver = true;
        RaiseControlEvent( ControlEvent.EventType.MOUSE_ENTER );
      }
      base.OnMouseEnter( e );
    }



    protected override void OnMouseLeave( EventArgs e )
    {
      if ( _MouseOver )
      {
        _MouseOver = false;
        RaiseControlEvent( ControlEvent.EventType.MOUSE_LEAVE );
      }
      base.OnMouseLeave( e );
    }



    protected override void OnMouseClick( MouseEventArgs e )
    {
      _LastMousePos = e.Location;
      RaiseControlEvent( ControlEvent.EventType.MOUSE_UPDATE, e.X, e.Y, ToButtonBitMask( e.Button ) );
      base.OnMouseClick( e );
    }



    protected override void OnMouseDoubleClick( MouseEventArgs e )
    {
      _LastMousePos = e.Location;
      RaiseControlEvent( ControlEvent.EventType.MOUSE_DOUBLE_CLICK, e.X, e.Y, ToButtonBitMask( e.Button ) );
      base.OnMouseDoubleClick( e );
    }



    protected override void OnMouseMove( MouseEventArgs e )
    {
      _LastMousePos = e.Location;
      RaiseControlEvent( ControlEvent.EventType.MOUSE_UPDATE, e.X, e.Y, ToButtonBitMask( e.Button ) );
      base.OnMouseMove( e );
    }


    protected override void OnKeyDown( KeyEventArgs e )
    {
      e.Handled = RaiseControlEvent( ControlEvent.EventType.KEY_DOWN, (Keys)e.KeyValue );
      base.OnKeyDown( e );
    }



    protected override void OnKeyUp( KeyEventArgs e )
    {
      e.Handled = RaiseControlEvent( ControlEvent.EventType.KEY_UP, (Keys)e.KeyValue );
      base.OnKeyUp( e );
    }



    protected override void OnKeyPress( KeyPressEventArgs e )
    {
      e.Handled = RaiseControlEvent( ControlEvent.EventType.KEY_PRESS, (Keys)e.KeyChar );
      base.OnKeyPress( e );
    }



    protected override void OnGotFocus( EventArgs e )
    {
      RaiseControlEvent( ControlEvent.EventType.FOCUSED );
      base.OnGotFocus( e );
    }



    protected override void OnLostFocus( EventArgs e )
    {
      RaiseControlEvent( ControlEvent.EventType.FOCUS_LOST );
      base.OnLostFocus( e );
    }



    protected override bool IsInputKey( Keys keyData )
    {
      if ( ( keyData == Keys.Tab )
      || ( keyData == Keys.Escape )   // escape here, without that CancelButtons do not work!
      || ( keyData == ( Keys.Shift | Keys.Tab ) ) )
      {
        return false;
      }
      return true;
    }



    protected override void OnMouseWheel( MouseEventArgs e )
    {
      _AccumulatedMouseWheelDelta += e.Delta;

      while ( _AccumulatedMouseWheelDelta >= SystemInformation.MouseWheelScrollDelta )
      {
        RaiseControlEvent( ControlEvent.EventType.MOUSE_WHEEL, e.X, e.Y, (uint)e.Button, 1 );
        _AccumulatedMouseWheelDelta -= SystemInformation.MouseWheelScrollDelta;
      }
      while ( _AccumulatedMouseWheelDelta <= -SystemInformation.MouseWheelScrollDelta )
      {
        RaiseControlEvent( ControlEvent.EventType.MOUSE_WHEEL, e.X, e.Y, (uint)e.Button, -1 );
        _AccumulatedMouseWheelDelta += SystemInformation.MouseWheelScrollDelta;
      }

      // do not pass wheel event to parent
      var handledEvent = (HandledMouseEventArgs)e;

      handledEvent.Handled = true;

      base.OnMouseWheel( e );
    }



    private uint ToButtonBitMask( MouseButtons Button )
    {
      uint  mask = 0;

      if ( ( Button & MouseButtons.Left ) == MouseButtons.Left )
      {
        mask |= 1;
      }
      if ( ( Button & MouseButtons.Right ) == MouseButtons.Right )
      {
        mask |= 2;
      }
      if ( ( Button & MouseButtons.Middle ) == MouseButtons.Middle )
      {
        mask |= 4;
      }
      return mask;
    }



    // The NativeUIA class provides access to the native UIA provider functions and data.
    public class NativeUIA
    {
      [DllImport( "UIAutomationCore.dll", EntryPoint = "UiaReturnRawElementProvider", CharSet = CharSet.Unicode )]
      public static extern IntPtr UiaReturnRawElementProvider(
          IntPtr hwnd, IntPtr wParam, IntPtr lParam, IRawElementProviderSimple el );

      [DllImport( "UIAutomationCore.dll", EntryPoint = "UiaHostProviderFromHwnd", CharSet = CharSet.Unicode )]
      public static extern int UiaHostProviderFromHwnd(
          IntPtr hwnd,
          [MarshalAs( UnmanagedType.Interface )] out IRawElementProviderSimple provider );

      public const int WM_GETOBJECT = 0x003D;
      public static IntPtr UiaRootObjectId = (IntPtr)(-25);
    }

    public class NativeWin32
    {
      public const int WM_SETCURSOR = 0x0020;
    }

    public enum CursorType
    {
      CURSOR_DEFAULT = 0,   // arrow
      CURSOR_HAND,
      CURSOR_HOURGLASS,
      CURSOR_FORBIDDEN,
      CURSOR_SIZE_NS,
      CURSOR_SIZE_WE,
      CURSOR_SIZE_NWSE,
      CURSOR_SIZE_NESW,
      CURSOR_SIZEALL,
      CURSOR_TEXT_EDIT
    };

    CursorType  _cursor = CursorType.CURSOR_DEFAULT;

    protected void SetCursor( CursorType cursor )
    {
      if ( cursor == _cursor )
      {
        return;
      }
      _cursor = cursor;

      Cursor  newCursor = Cursors.Arrow;
      switch ( cursor )
      {
        case CursorType.CURSOR_DEFAULT:
        default:
          newCursor = Cursors.Arrow;
          break;
        case CursorType.CURSOR_HOURGLASS:
          newCursor = Cursors.WaitCursor;
          break;
        case CursorType.CURSOR_FORBIDDEN:
          newCursor = Cursors.No;
          break;
        case CursorType.CURSOR_HAND:
          newCursor = Cursors.Hand;
          break;
        case CursorType.CURSOR_SIZEALL:
          newCursor = Cursors.SizeAll;
          break;
        case CursorType.CURSOR_SIZE_NESW:
          newCursor = Cursors.SizeNESW;
          break;
        case CursorType.CURSOR_SIZE_WE:
          newCursor = Cursors.SizeWE;
          break;
        case CursorType.CURSOR_SIZE_NS:
          newCursor = Cursors.SizeNS;
          break;
        case CursorType.CURSOR_SIZE_NWSE:
          newCursor = Cursors.SizeNWSE;
          break;
        case CursorType.CURSOR_TEXT_EDIT:
          newCursor = Cursors.IBeam;
          break;
      }
      this.Cursor = newCursor;
      Cursor.Current = newCursor;
    }



    protected override void WndProc( ref Message m )
    {
      switch ( m.Msg )
      {
        case NativeWin32.WM_SETCURSOR:
          if ( ( m.LParam.ToInt32() & 0xFFFF ) == 1 ) // LOWORD(lParam) == 1 means client area
          {
            if ( RaiseControlEvent( ControlEvent.EventType.SET_CURSOR, _LastMousePos.X, _LastMousePos.Y ) )
            {
              m.Result = (IntPtr)1;
              return;
            }
            SetCursor( CursorType.CURSOR_DEFAULT );
            m.Result = (IntPtr)1;
            return;
          }
          break;
        case NativeUIA.WM_GETOBJECT:
          {
            // If the window is being asked for a UIA provider, return ourselves.
            if ( m.LParam == NativeUIA.UiaRootObjectId )
            {
              m.Result = NativeUIA.UiaReturnRawElementProvider( this.Handle, m.WParam, m.LParam, this );

              return;
            }

            break;
          }
      }

      base.WndProc( ref m );
    }



    public IRawElementProviderSimple HostRawElementProvider
    {
      get
      {
        IRawElementProviderSimple result;

        NativeUIA.UiaHostProviderFromHwnd( Handle, out result );

        return result;
        //return null;
      }
    }



    public ProviderOptions ProviderOptions
    {
      get
      {
        var options = (ProviderOptions)( (int)ProviderOptions.ServerSideProvider | 32 ); //ProviderOptions.UseComThreading;

        options |= ProviderOptions.OverrideProvider;

        return options;
      }
    }



    public object GetPatternProvider( int patternId )
    {
      //System.Windows.Automation.Provider.IInvokeProvider
      /*
      if ( ( patternId == GridItemPatternIdentifiers.Pattern.Id ) 
      ||   ( patternId == TableItemPatternIdentifiers.Pattern.Id ) 
      ||   ( patternId == ValuePatternIdentifiers.Pattern.Id ) )
      {
        return this;
      }
      if ( patternId == ValuePatternIdentifiers.Pattern.Id )
      {
        return this;
      }*/

      return null;
    }



    public object GetPropertyValue( int propertyId )
    {
      // By default, the element gets exposed through the Control view of the UIA tree,
      // so we don't need to react to IsControlElementProperty here.

      // For this demo, this element is not keyboard focusable. If it were, then it would
      // need to return true for IsKeyboardFocusableProperty, and either true for 
      // HasKeyboardFocusProperty if it has focus now.

      if ( propertyId == AutomationElementIdentifiers.NameProperty.Id )
      {
        return this.Name;
      }
      else if ( propertyId == AutomationElementIdentifiers.LocalizedControlTypeProperty.Id )
      {
        return "Garden Thing";
      }

      return null;
    }



  }
}
