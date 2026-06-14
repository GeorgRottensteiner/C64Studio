using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using GR.Image;



namespace DecentForms
{
  public class CheckBox : ControlBase
  {
    private bool              _Pushed       = false;
    private bool              _WasPushed    = false;
    private bool              _PushedByKey  = false;
    private bool              _Checked      = false;
    private ContentAlignment  _CheckAlignment = ContentAlignment.MiddleLeft;

    [DefaultValue( Appearance.Normal )]
    private Appearance        _Appearance   = Appearance.Normal;

    private System.Drawing.Image    _Image = null;

    private int               _CheckBoxSize = 13;



    public CheckBox()
    {
      BorderStyle = BorderStyle.NONE;
    }



    public event EventHandler CheckedChanged;



    public ContentAlignment CheckAlign
    {
      get
      {
        return _CheckAlignment;
      }
      set
      {
        if ( _CheckAlignment != value )
        {
          _CheckAlignment = value;
          Invalidate();
        }
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



    public Appearance Appearance
    {
      get
      {
        return _Appearance;
      }
      set
      {
        if ( _Appearance != value )
        {
          _Appearance = value;
          Invalidate();
        }
      }
    }



    public bool Pushed
    {
      get
      {
        return _Pushed | _PushedByKey;
      }
    }



    public bool Checked
    {
      get
      {
        return _Checked;
      }
      set
      {
        if ( _Checked != value )
        {
          _Checked = value;
          CheckedChanged?.Invoke( this );
          if ( Appearance == Appearance.Normal )
          {
            Invalidate( GetCheckRect() );
          }
          else
          {
            Invalidate();
          }
        }
      }
    }



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
              _Checked = !_Checked;
              CheckedChanged?.Invoke( this );
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
                _Checked = !_Checked;
                Invalidate();
                CheckedChanged?.Invoke( this );
              }
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
            }
          }
          break;
      }
      base.OnControlEvent( Event );
    }



    protected override void OnPaint( ControlRenderer Renderer )
    {
      Renderer.RenderCheckBox( Text, CheckAlign, MouseOver, Pushed, Checked );
    }



    public Rectangle GetCheckRect()
    {
      int     x = 0;
      int     y = 0;

      int     offsetFromBorder = 3;

      switch ( _CheckAlignment )
      {
        case ContentAlignment.TopLeft:
        case ContentAlignment.MiddleLeft:
        case ContentAlignment.BottomLeft:
          x = 0;
          break;
        case ContentAlignment.TopCenter:
        case ContentAlignment.MiddleCenter:
        case ContentAlignment.BottomCenter:
          x = ( ClientSize.Width - _CheckBoxSize ) / 2;
          break;
        case ContentAlignment.TopRight:
        case ContentAlignment.MiddleRight:
        case ContentAlignment.BottomRight:
          x = ClientSize.Width - _CheckBoxSize;
          break;
      }
      switch ( _CheckAlignment )
      {
        case ContentAlignment.TopLeft:
        case ContentAlignment.TopCenter:
        case ContentAlignment.TopRight:
          y = offsetFromBorder;
          break;
        case ContentAlignment.MiddleLeft:
        case ContentAlignment.MiddleCenter:
        case ContentAlignment.MiddleRight:
          y = ( ClientSize.Height - _CheckBoxSize ) / 2;
          break;
        case ContentAlignment.BottomLeft:
        case ContentAlignment.BottomCenter:
        case ContentAlignment.BottomRight:
          y = ClientSize.Height - offsetFromBorder - _CheckBoxSize;
          break;
      }

      return new Rectangle( x, y, _CheckBoxSize, _CheckBoxSize );
    }



    public Rectangle GetTextRect()
    {
      int     x = 0;
      int     y = 0;
      int     width = ClientSize.Width;
      int     height = ClientSize.Height;

      int     offsetFromBorder = 3;

      switch ( _CheckAlignment )
      {
        case ContentAlignment.TopLeft:
        case ContentAlignment.MiddleLeft:
        case ContentAlignment.BottomLeft:
          x = offsetFromBorder + _CheckBoxSize + offsetFromBorder;
          width -= 2 * offsetFromBorder + _CheckBoxSize;
          break;
        case ContentAlignment.TopCenter:
        case ContentAlignment.MiddleCenter:
        case ContentAlignment.BottomCenter:
          //x = ( ClientSize.Width - _CheckBoxSize ) / 2;
          break;
        case ContentAlignment.TopRight:
        case ContentAlignment.MiddleRight:
        case ContentAlignment.BottomRight:
          width -= 2 * offsetFromBorder + _CheckBoxSize;
          break;
      }
      switch ( _CheckAlignment )
      {
        case ContentAlignment.TopLeft:
        case ContentAlignment.TopCenter:
        case ContentAlignment.TopRight:
          y = offsetFromBorder + _CheckBoxSize;
          height -= 2 * offsetFromBorder + _CheckBoxSize;
          break;
        case ContentAlignment.MiddleLeft:
        case ContentAlignment.MiddleCenter:
        case ContentAlignment.MiddleRight:
          //y = ( ClientSize.Height - _CheckBoxSize ) / 2;
          break;
        case ContentAlignment.BottomLeft:
        case ContentAlignment.BottomCenter:
        case ContentAlignment.BottomRight:
          height -= offsetFromBorder + _CheckBoxSize;
          break;
      }

      return new Rectangle( x, y, width, height );
    }



  }
}