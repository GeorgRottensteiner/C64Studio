using System;

namespace CustomAutoScrollPanel
{
  using System;
  using System.Windows.Forms;
  using System.Runtime.InteropServices;



  public class ScrollablePanel : System.Windows.Forms.Panel
  {

    public event System.Windows.Forms.ScrollEventHandler ScrollHorizontal;
    public event System.Windows.Forms.ScrollEventHandler ScrollVertical;
    public event System.Windows.Forms.MouseEventHandler  ScrollMouseWheel;


    private const int SB_LINEUP = 0;
    private const int SB_LINEDOWN = 1;
    private const int SB_PAGEUP = 2;
    private const int SB_PAGEDOWN = 3;
    private const int SB_THUMBPOSITION = 4;
    private const int SB_THUMBTRACK = 5;
    private const int SB_TOP = 6;
    private const int SB_BOTTOM = 7;
    private const int SB_ENDSCROLL = 8;

    private const int WM_HSCROLL = 0x114;
    private const int WM_VSCROLL = 0x115;
    private const int WM_MOUSEWHEEL = 0x020A;
    private const int WM_NCCALCSIZE =0x0083;
    private const int WM_PAINT =0x000F;
    private const int WM_SIZE =0x0005;

    private const uint SB_HORZ = 0;
    private const uint SB_VERT = 1;
    private const uint SB_CTL = 2;
    private const uint SB_BOTH = 3;

    private const uint  ESB_DISABLE_BOTH = 0x3;
    private const uint  ESB_ENABLE_BOTH = 0x0;

    private const int  MK_LBUTTON = 0x01;
    private const int  MK_RBUTTON = 0x02;
    private const int  MK_SHIFT = 0x04;
    private const int  MK_CONTROL = 0x08;
    private const int  MK_MBUTTON = 0x10;
    private const int  MK_XBUTTON1 = 0x0020;
    private const int  MK_XBUTTON2 = 0x0040;



    private bool enableAutoHorizontal = true;
    private bool enableAutoVertical = true;
    private bool visibleAutoHorizontal = true;
    private bool visibleAutoVertical = true;

    private int autoScrollHorizontalMinimum = 0;
    private int autoScrollHorizontalMaximum = 100;

    private int autoScrollVerticalMinimum = 0;
    private int autoScrollVerticalMaximum = 100;



    public ScrollablePanel()
    {
      Click += new EventHandler( ScrollablePanel_Click );
      AutoScroll = true;
    }



    public int AutoScrollHPos
    {
      get { return GetScrollPos( Handle, (int)SB_HORZ ); }
      set { SetScrollPos( Handle, (int)SB_HORZ, value, true ); }
    }

    public int AutoScrollVPos
    {
      get { return GetScrollPos( Handle, (int)SB_VERT ); }
      set { SetScrollPos( Handle, (int)SB_VERT, value, true ); }
    }

    public int AutoScrollHorizontalMinimum
    {
      get { return autoScrollHorizontalMinimum; }
      set
      {
        autoScrollHorizontalMinimum = value;
        SetScrollRange( Handle, (int)SB_HORZ, autoScrollHorizontalMinimum, autoScrollHorizontalMaximum, true );
      }
    }

    public int AutoScrollHorizontalMaximum
    {
      get { return autoScrollHorizontalMaximum; }
      set
      {
        autoScrollHorizontalMaximum = value;
        SetScrollRange( Handle, (int)SB_HORZ, autoScrollHorizontalMinimum, autoScrollHorizontalMaximum, true );
      }
    }

    public int AutoScrollVerticalMinimum
    {
      get { return autoScrollVerticalMinimum; }
      set
      {
        autoScrollVerticalMinimum = value;
        SetScrollRange( Handle, (int)SB_VERT, autoScrollVerticalMinimum, AutoScrollVerticalMaximum, true );
      }
    }

    public int AutoScrollVerticalMaximum
    {
      get { return autoScrollVerticalMaximum; }
      set
      {
        autoScrollVerticalMaximum = value;
        SetScrollRange( Handle, (int)SB_VERT, autoScrollVerticalMinimum, AutoScrollVerticalMaximum, true );
      }
    }


    public bool EnableAutoScrollHorizontal
    {
      get { return enableAutoHorizontal; }
      set
      {
        enableAutoHorizontal = value;
        if ( value )
          EnableScrollBar( Handle, SB_HORZ, ESB_ENABLE_BOTH );
        else
          EnableScrollBar( Handle, SB_HORZ, ESB_DISABLE_BOTH );
      }
    }

    public bool EnableAutoScrollVertical
    {
      get { return enableAutoVertical; }
      set
      {
        enableAutoVertical = value;
        if ( value )
          EnableScrollBar( Handle, SB_VERT, ESB_ENABLE_BOTH );
        else
          EnableScrollBar( Handle, SB_VERT, ESB_DISABLE_BOTH );
      }
    }

    public bool VisibleAutoScrollHorizontal
    {
      get { return visibleAutoHorizontal; }
      set
      {
        visibleAutoHorizontal = value;
        ShowScrollBar( Handle, (int)SB_HORZ, value );
      }
    }

    public bool VisibleAutoScrollVertical
    {
      get { return visibleAutoVertical; }
      set
      {
        visibleAutoVertical = value;
        ShowScrollBar( Handle, (int)SB_VERT, value );
      }
    }



    private int GetSBFromScrollEventType( ScrollEventType type )
    {
      int res = -1;
      switch ( type )
      {
        case ScrollEventType.SmallDecrement:
          res = SB_LINEUP;
          break;
        case ScrollEventType.SmallIncrement:
          res = SB_LINEDOWN;
          break;
        case ScrollEventType.LargeDecrement:
          res = SB_PAGEUP;
          break;
        case ScrollEventType.LargeIncrement:
          res = SB_PAGEDOWN;
          break;
        case ScrollEventType.ThumbTrack:
          res = SB_THUMBTRACK;
          break;
        case ScrollEventType.First:
          res = SB_TOP;
          break;
        case ScrollEventType.Last:
          res = SB_BOTTOM;
          break;
        case ScrollEventType.ThumbPosition:
          res = SB_THUMBPOSITION;
          break;
        case ScrollEventType.EndScroll:
          res = SB_ENDSCROLL;
          break;
        default:
          break;
      }
      return res;
    }



    private ScrollEventType GetScrollEventType( System.IntPtr wParam )
    {
      ScrollEventType res = 0;
      switch ( LoWord( wParam ) )
      {
        case SB_LINEUP:
          res = ScrollEventType.SmallDecrement;
          break;
        case SB_LINEDOWN:
          res = ScrollEventType.SmallIncrement;
          break;
        case SB_PAGEUP:
          res = ScrollEventType.LargeDecrement;
          break;
        case SB_PAGEDOWN:
          res = ScrollEventType.LargeIncrement;
          break;
        case SB_THUMBTRACK:
          res = ScrollEventType.ThumbTrack;
          break;
        case SB_TOP:
          res = ScrollEventType.First;
          break;
        case SB_BOTTOM:
          res = ScrollEventType.Last;
          break;
        case SB_THUMBPOSITION:
          res = ScrollEventType.ThumbPosition;
          break;
        case SB_ENDSCROLL:
          res = ScrollEventType.EndScroll;
          break;
        default:
          res = ScrollEventType.EndScroll;
          break;
      }
      return res;
    }



    protected override void WndProc( ref Message msg )
    {
      if ( ( msg.Msg != WM_VSCROLL )
      &&   ( msg.Msg != WM_HSCROLL ) )
      {
        base.WndProc( ref msg );
      }
      if ( msg.HWnd != Handle )
      {
        return;
      }
      switch ( msg.Msg )
      {
        case WM_MOUSEWHEEL:
          if ( !VisibleAutoScrollVertical )
          {
            return;
          }
          try
          {
            int zDelta = HiWord( msg.WParam );
            //int zDelta = unchecked( (short)( unchecked( (int)msg.WParam ) >> 16 ) );

            int y = HiWord( msg.LParam);
            int x = LoWord( msg.LParam);
            System.Windows.Forms.MouseButtons butt;
            switch ( LoWord( msg.WParam ) )
            {
              case MK_LBUTTON:
                butt = System.Windows.Forms.MouseButtons.Left;
                break;
              case MK_MBUTTON:
                butt = System.Windows.Forms.MouseButtons.Middle;
                break;
              case MK_RBUTTON:
                butt = System.Windows.Forms.MouseButtons.Right;
                break;
              case MK_XBUTTON1:
                butt = System.Windows.Forms.MouseButtons.XButton1;
                break;
              case MK_XBUTTON2:
                butt = System.Windows.Forms.MouseButtons.XButton2;
                break;
              default:
                butt = System.Windows.Forms.MouseButtons.None;
                break;
            }
            if ( ScrollMouseWheel != null )
            {
              System.Windows.Forms.MouseEventArgs arg0 = new System.Windows.Forms.MouseEventArgs( butt, 1, x, y, zDelta );
              ScrollMouseWheel( this, arg0 );
            }

            ScrollEventType   type = ScrollEventType.SmallDecrement;
            if ( zDelta < 0 )
            {
              type = ScrollEventType.SmallIncrement;
            }
            for ( int i = 0; i < 3; ++i )
            {
              ScrollEventArgs arg = new ScrollEventArgs( type, 0, ScrollOrientation.VerticalScroll );
              OnScroll( arg );
            }
          }
          catch ( Exception )
          {
          }
          break;
        case WM_VSCROLL:
          try
          {
            ScrollEventType type = GetScrollEventType( msg.WParam );
            int     value = GetScrollPos( Handle, (int)SB_VERT );
            if ( ( type == ScrollEventType.ThumbPosition )
            || ( type == ScrollEventType.ThumbTrack ) )
            {
              value = HiWord( msg.WParam );
            }
            ScrollEventArgs arg = new ScrollEventArgs( type, value, ScrollOrientation.VerticalScroll );
            if ( ScrollVertical != null )
            {
              ScrollVertical( this, arg );
            }
            OnScroll( arg );
          }
          catch ( Exception ) { }

          break;

        case WM_HSCROLL:

          try
          {
            ScrollEventType type = GetScrollEventType(msg.WParam);
            int value = GetScrollPos( Handle, (int)SB_HORZ );
            if ( ( type == ScrollEventType.ThumbPosition )
            || ( type == ScrollEventType.ThumbTrack ) )
            {
              value = HiWord( msg.WParam );
            }
            ScrollEventArgs arg = new ScrollEventArgs( type, value );
            if ( ScrollHorizontal != null )
            {
              ScrollHorizontal( this, arg );
            }
            OnScroll( arg );
          }
          catch ( Exception ) { }

          break;

        default:
          break;
      }
    }



    public void PerformScrollHorizontal( ScrollEventType type )
    {
      int param = GetSBFromScrollEventType( type );
      if ( param == -1 )
      {
        return;
      }
      SendMessage( Handle, (uint)WM_HSCROLL, (System.UIntPtr)param, (System.IntPtr)0 );
    }



    public void PerformScrollVertical( ScrollEventType type )
    {
      int param = GetSBFromScrollEventType( type );
      if ( param == -1 )
      {
        return;
      }
      SendMessage( Handle, (uint)WM_VSCROLL, (System.UIntPtr)param, (System.IntPtr)0 );
    }



    private void ScrollablePanel_Click( object sender, EventArgs e )
    {
      Focus();
    }



    [DllImport( "user32.dll", CharSet = CharSet.Auto )]
    static public extern int GetSystemMetrics( int code );

    [DllImport( "user32.dll" )]
    static public extern bool EnableScrollBar( System.IntPtr hWnd, uint wSBflags, uint wArrows );

    [DllImport( "user32.dll" )]
    static public extern int SetScrollRange( System.IntPtr hWnd, int nBar, int nMinPos, int nMaxPos, bool bRedraw );

    [DllImport( "user32.dll" )]
    static public extern int SetScrollPos( System.IntPtr hWnd, int nBar, int nPos, bool bRedraw );

    [DllImport( "user32.dll" )]
    static public extern int GetScrollPos( System.IntPtr hWnd, int nBar );


    [DllImport( "user32.dll" )]
    static public extern bool ShowScrollBar( System.IntPtr hWnd, int wBar, bool bShow );

    [DllImport( "user32.dll" )]
    static extern IntPtr SendMessage( IntPtr hWnd, uint Msg, UIntPtr wParam, IntPtr lParam );



    static int MakeLong( int LoWord, int HiWord )
    {
      return ( HiWord << 16 ) | ( LoWord & 0xffff );
    }



    static IntPtr MakeLParam( int LoWord, int HiWord )
    {
      return (IntPtr)( ( HiWord << 16 ) | ( LoWord & 0xffff ) );
    }



    static short HiWord( IntPtr number )
    {
      return unchecked((short)( (long)number >> 16 ));
    }



    static int LoWord( IntPtr number )
    {
      return unchecked((short)(long)number);
    }



  }
}
