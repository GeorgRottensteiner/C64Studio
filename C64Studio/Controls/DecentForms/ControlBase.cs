using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;



namespace DecentForms
{
  public class ControlBase : Control
  {
    private bool          _MouseOver = false;
    private bool          _MouseDown = false;
    private int           _AccumulatedMouseWheelDelta = 0;

    private BorderStyle   _BorderStyle = BorderStyle.FLAT;

    public bool           CanBeFocused = true;

    protected int         _DisplayOffsetX = 0;
    protected int         _DisplayOffsetY = 0;
    protected int         _ActualWorkWidth = 0;
    protected int         _ActualWorkHeight = 0;



    public ControlBase()
    {
      DoubleBuffered  = true;
      AutoSize        = false;
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



    static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
    static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
    static readonly IntPtr HWND_TOP = new IntPtr(0);
    static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

    [DllImport( "user32.dll", SetLastError = true )]
    static extern bool SetWindowPos( IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, UInt32 uFlags );

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
      ||   ( X >= Width )
      ||   ( Y < 0 )
      ||   ( Y >= Height ) )
      {
        return true;
      }
      return false;
    }



    public void RaiseControlEvent( ControlEvent Event )
    {
      OnControlEvent( Event );
    }



    public void RaiseControlEvent( ControlEvent.EventType Type, int MouseX = -1, int MouseY = -1, uint MouseButtons = 0, int MouseWheelDelta = 0 )
    {
      OnControlEvent( new ControlEvent()
      {
        Type = Type,
        MouseX = MouseX,
        MouseY = MouseY,
        MouseButtons = MouseButtons,
        MouseWheelDelta = MouseWheelDelta
      } );
    }



    public void RaiseControlEvent( ControlEvent.EventType Type, Keys Key )
    {
      OnControlEvent( new ControlEvent()
      {
        Type = Type,
        Key = Key
      } );
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
        RaiseControlEvent( ControlEvent.EventType.MOUSE_DOWN, e.X, e.Y, ToButtonBitMask( e.Button ) );
      }
      base.OnMouseDown( e );
    }



    protected override void OnMouseUp( MouseEventArgs e )
    {
      if ( _MouseDown )
      {
        _MouseDown = false;
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
      RaiseControlEvent( ControlEvent.EventType.MOUSE_UPDATE, e.X, e.Y, ToButtonBitMask( e.Button ) );
      base.OnMouseClick( e );
    }



    protected override void OnMouseDoubleClick( MouseEventArgs e )
    {
      RaiseControlEvent( ControlEvent.EventType.MOUSE_DOUBLE_CLICK, e.X, e.Y, ToButtonBitMask( e.Button ) );
      base.OnMouseDoubleClick( e );
    }



    protected override void OnMouseMove( MouseEventArgs e )
    {
      RaiseControlEvent( ControlEvent.EventType.MOUSE_UPDATE, e.X, e.Y, ToButtonBitMask( e.Button ) );
      base.OnMouseMove( e );
    }


    protected override void OnKeyDown( KeyEventArgs e )
    {
      RaiseControlEvent( ControlEvent.EventType.KEY_DOWN, (Keys)e.KeyValue );
      base.OnKeyDown( e );
    }



    protected override void OnKeyUp( KeyEventArgs e )
    {
      RaiseControlEvent( ControlEvent.EventType.KEY_UP, (Keys)e.KeyValue );
      base.OnKeyDown( e );
    }



    protected override void OnKeyPress( KeyPressEventArgs e )
    {
      RaiseControlEvent( ControlEvent.EventType.KEY_PRESS, (Keys)e.KeyChar );
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
      return true;
      //return base.IsInputKey( keyData );
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



  }
}
