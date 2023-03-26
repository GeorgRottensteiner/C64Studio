using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;

namespace RetroDevStudio.Controls
{
  [ToolboxItem( false )]
  public partial class PopupControl : ToolStripDropDown
  {
    private Control   _Content = null;
    private Control   _AttachControl = null;
    private Size      _MinimumSize = new Size( 20, 20 );
    private Point     _OriginalAttachControlLocation;



    public PopupControl( Control Content )
    {
      _Content = Content;
      if ( _Content.MinimumSize.Width > 0 )
      {
        _MinimumSize = _Content.MinimumSize;
      }
      else
      {
        _MinimumSize = _Content.Size;
      }
      MinimumSize = _MinimumSize;
      ClientSize = _Content.Size;
      Padding = new Padding( 0 );
      InitializeComponent();

      var host = new ToolStripControlHost( _Content );
      Items.Add( host );
    }



    protected override void OnSizeChanged( EventArgs e )
    {
      if ( _Content != null )
      {
        _Content.MinimumSize  = Size;
        _Content.MaximumSize  = Size;
        _Content.Size         = Size;
        _Content.Location     = Point.Empty;
      }
      base.OnSizeChanged( e );
    }



    protected override CreateParams CreateParams
    {
      get
      {
        CreateParams cp = base.CreateParams;
        cp.ExStyle |= 0x08000000; // NativeMethods.WS_EX_NOACTIVATE;
        //if ( NonInteractive ) cp.ExStyle |= NativeMethods.WS_EX_TRANSPARENT | NativeMethods.WS_EX_LAYERED | NativeMethods.WS_EX_TOOLWINDOW;
        return cp;
      }
    }



    public void Show( Control control )
    {
      _AttachControl = control;
      if ( control == null )
      {
        throw new ArgumentNullException( "control" );
      }
      _AttachControl.LostFocus += _AttachControl_LostFocus;
      _AttachControl.Move += _AttachControl_Move;
      _AttachControl.LocationChanged += _AttachControl_LocationChanged;
      _OriginalAttachControlLocation = _AttachControl.PointToScreen( _AttachControl.Location );
      _AttachControl.Width = _AttachControl.Width;
      Show( control, control.ClientRectangle );
    }



    protected override void OnLostFocus( EventArgs e )
    {
      base.OnLostFocus( e );

      if ( ( _AttachControl != null )
      &&   ( !_AttachControl.Focused )
      &&   ( !ContainsFocus ) )
      {
        _AttachControl.LostFocus -= _AttachControl_LostFocus;
        _AttachControl.Move -= _AttachControl_Move;
        _AttachControl.LocationChanged -= _AttachControl_LocationChanged;

        Parent = null;
        Visible = false;
        //_AttachControl.Parent = null;
        //_AttachControl.Visible = false;

        _AttachControl = null;
        //OnClosed( new ToolStripDropDownClosedEventArgs( ToolStripDropDownCloseReason.AppFocusChange ) );

        /*
        Debug.Log( "predisp" );
        Dispose();
        Debug.Log( "postdisp" );*/
        return;
      }
    }



    private void _AttachControl_LostFocus( object sender, EventArgs e )
    {
      OnLostFocus( e );
    }



    private void _AttachControl_LocationChanged( object sender, EventArgs e )
    {
      var newPos = _AttachControl.PointToScreen( _AttachControl.Location );
      var delta = new Point( newPos.X - _OriginalAttachControlLocation.X, newPos.Y - _OriginalAttachControlLocation.Y );
      _OriginalAttachControlLocation = newPos;

      Location = new Point( Location.X + delta.X, Location.Y + delta.Y );
    }



    private void _AttachControl_Move( object sender, EventArgs e )
    {
      var newPos = _AttachControl.PointToScreen( _AttachControl.Location );
      var delta = new Point( newPos.X - _OriginalAttachControlLocation.X, newPos.Y - _OriginalAttachControlLocation.Y );
      _OriginalAttachControlLocation = newPos;

      Location = new Point( Location.X + delta.X, Location.Y + delta.Y );
    }



    public void Show( Control control, Rectangle area )
    {
      if ( control == null )
      {
        throw new ArgumentNullException( "control" );
      }
      //SetOwnerItem( control );

      //_resizableTop = _resizableLeft = false;
      Point location = control.PointToScreen(new Point(area.Left, area.Top + area.Height));
      Rectangle screen = Screen.FromControl(control).WorkingArea;
      if ( location.X + Size.Width > ( screen.Left + screen.Width ) )
      {
        //_resizableLeft = true;
        location.X = ( screen.Left + screen.Width ) - Size.Width;
      }
      if ( location.Y + Size.Height > ( screen.Top + screen.Height ) )
      {
        //_resizableTop = true;
        location.Y -= Size.Height + area.Height;
      }
      location = control.PointToClient( location );
      Show( control, location, ToolStripDropDownDirection.BelowRight );
    }
    
    
    
    protected override void Dispose( bool disposing )
    {
      if ( disposing )
      {
        if ( components != null )
        {
          components.Dispose();
        }
        /*
        if ( _Content != null )
        {
          System.Windows.Forms.Control _content = _Content;
          _Content = null;
          _content.Dispose();
        }*/
      }
      base.Dispose( disposing );
    }



  }
}
