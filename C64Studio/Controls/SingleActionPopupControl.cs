using GR.Image;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace RetroDevStudio.Controls
{
  public class SingleActionPopupControl : Control, IMessageFilter, IDisposable
  {
    public delegate void delClicked( int X, int Y );



    public event delClicked     Clicked;



    public SingleActionPopupControl()
    {
      SetTopLevel( true );
      Application.AddMessageFilter( this );
    }



    ~SingleActionPopupControl()
    {
      this.Dispose( false );
    }



    public SingleActionPopupControl( Control ChildControl )
    {
      SetTopLevel( true );
      Controls.Add( ChildControl );
      Application.AddMessageFilter( this );
    }



    protected override void OnLostFocus( EventArgs e )
    {
      DestroyHandle();
    }



    protected override CreateParams CreateParams
    {
      get
      {
        CreateParams p = base.CreateParams;

        // WS_CAPTION | WS_SYSMENU
        p.Style &= ~( 0x00C00000 | 0x00080000 );

        // WS_BORDER | WS_POPUP
        p.Style |= 0x00800000 | unchecked((int)0x80000000);

        return p;
      }
    }



    protected override void OnMouseLeave( EventArgs e )
    {
      base.OnMouseLeave( e );
      DestroyHandle();
    }



    bool entered;
    public bool PreFilterMessage( ref Message m )
    {
      if ( m.Msg == 0x2a3 && entered ) return true;//discard the default MouseLeave inside         

      if ( m.Msg == 0x0201 )
      {
        // WM_LBUTTONDOWN
        Focus();
        return true;
      }
      if ( m.Msg == 0x0202 )
      {
        // WM_LBUTTONUP
        int x = unchecked( (short)(long)m.LParam );
        int y = unchecked( (short)( (ulong)m.LParam >> 16 ) );

        Control c = Control.FromHandle( m.HWnd );
        if ( c != null )
        {
          var p = c.PointToScreen( new Point( x, y ) );
          p = this.PointToClient( p );
          if ( Clicked != null )
          {
            Clicked( p.X, p.Y );
          }
        }
        Dispose();
        return false;
      }

      if ( m.Msg == 0x200 )
      {
        Control c = Control.FromHandle(m.HWnd);
        if ( Contains( c ) || c == this )
        {
          if ( !entered )
          {
            OnMouseEnter( EventArgs.Empty );
            entered = true;
          }
        }
        else if ( entered )
        {
          OnMouseLeave( EventArgs.Empty );
          entered = false;
        }
      }
      return false;
    }



    protected override void Dispose( bool disposing )
    {
      Application.RemoveMessageFilter( this );
      base.Dispose( disposing );
    }



  }



}
