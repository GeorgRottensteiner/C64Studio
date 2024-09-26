using System.Diagnostics;
using System.Windows.Forms;
using GR.Image;



namespace DecentForms
{
  public class Button : ControlBase, IButtonControl
  {
    public enum ButtonStyle
    {
      RAISED,
      FLAT
    }

    private bool          _Pushed           = false;
    private bool          _WasPushed        = false;
    private bool          _PushedByKey      = false;
    private bool          _IsDefaultButton  = false;

    private System.Drawing.Image    _Image = null;



    public Button()
    {
      AccessibleRole = AccessibleRole.PushButton;
    }



    public new event EventHandler     Click;



    public ButtonStyle ButtonBorder { get;  set; }



    public bool Pushed
    {
      get
      {
        return _Pushed | _PushedByKey;
      }
    }



    public bool IsDefault
    {
      get
      {
        return _IsDefaultButton;
      }
    }



    public System.Drawing.Image Image
    {
      get
      {
        return _Image;
      }
      set
      {
        _Image = value.GetImageStretchedDPI();
        Invalidate();
      }
    }



    public DialogResult DialogResult { get; set; } = DialogResult.OK;



    protected override void OnControlEvent( ControlEvent Event )
    {
      switch ( Event.Type )
      {
        case ControlEvent.EventType.FOCUS_LOST:
          if ( _PushedByKey )
          {
            _PushedByKey = false;
          }
          Invalidate();
          break;
        case ControlEvent.EventType.FOCUSED:
          Invalidate();
          break;
        case ControlEvent.EventType.MOUSE_ENTER:
          if ( _WasPushed )
          {
            _Pushed = true;
          }
          Invalidate();
          break;
        case ControlEvent.EventType.MOUSE_LEAVE:
          _Pushed = false;
          Invalidate();
          break;
        case ControlEvent.EventType.MOUSE_DOWN:
          if ( !_Pushed )
          {
            _Pushed     = true;
            _WasPushed  = true;
            if ( CanBeFocused )
            {
              Focus();
            }
            Capture = true;
            Invalidate();
          }
          break;
        case ControlEvent.EventType.MOUSE_UP:
          _WasPushed = false;
          if ( _Pushed )
          {
            _Pushed = false;
            Capture = false;
            Invalidate();
            if ( MouseOver )
            {
              Click?.Invoke( this );
            }
          }
          break;
        case ControlEvent.EventType.KEY_DOWN:
          if ( Focused )
          {
            if ( Event.Key == Keys.Space )
            {
              if ( !_PushedByKey )
              {
                _PushedByKey = true;
                Invalidate();
                Click?.Invoke( this );
                Event.Handled = true;
              }
            }
            else if ( Event.Key == Keys.Enter )
            {
              // continuous pressing is possible!
              Click?.Invoke( this );
              Event.Handled = true;
            }
          }
          break;
        case ControlEvent.EventType.KEY_UP:
          if ( Focused )
          {
            if ( Event.Key == Keys.Space )
            {
              if ( _PushedByKey )
              {
                _PushedByKey = false;
                Invalidate();
              }
              Event.Handled = true;
            }
          }
          break;
      }
      base.OnControlEvent( Event );
    }



    protected override void OnPaint( ControlRenderer Renderer )
    {
      Renderer.RenderButton( MouseOver, Pushed, _IsDefaultButton, ButtonBorder );
    }



    public void NotifyDefault( bool value )
    {
      _IsDefaultButton = value;
      Invalidate();
    }



    public void PerformClick()
    {
      Click?.Invoke( this );
    }



  }
}