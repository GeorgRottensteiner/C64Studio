using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;



namespace DecentForms
{
  public class VScrollBar : ControlBase
  {
    public enum SBDisplayType
    {
      RAISED,
      FLAT
    }

    private Button    _TopButton = new Button();
    private Button    _BottomButton = new Button();

    private int       _ButtonHeight = 16;
    private int       _SliderHeight = 40;
    private int       _WheelFactor = 3;

    private bool      _MouseOverSlider = false;
    private bool      _SliderPushed = false;
    private int       _SliderDragOffset = 0;
    private int       _SliderDragPos = 0;

    private int       _Value = 0;



    public int Minimum { get; set; } = 0;
    public int Maximum { get; set; } = 100;



    public int Value 
    {
      get
      {
        return _Value;
      }
      set
      {
        int     newValue = value;
        if ( newValue < Minimum )
        {
          newValue = Minimum;
        }
        if ( newValue > Maximum )
        {
          newValue = Maximum;
        }
        if ( newValue != _Value )
        {
          _Value = newValue;
          Invalidate();
        }
      }
    }



    public int SmallChange { get; set; } = 1;
    public int LargeChange { get; set; } = 10;



    public VScrollBar()
    {
      Width = 17;
      DisplayType = SBDisplayType.RAISED;
      BorderStyle = BorderStyle.NONE;

      _TopButton.CanBeFocused = false;
      _TopButton.Click += OnScrollUp;
      _TopButton.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top; 
      _TopButton.CustomDraw += _TopButton_CustomDraw;

      _BottomButton.CanBeFocused = false;
      _BottomButton.Click += OnScrollDown;
      _BottomButton.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
      _BottomButton.CustomDraw += _BottomButton_CustomDraw;

      Controls.Add( _TopButton );
      Controls.Add( _BottomButton );
    }



    private void _TopButton_CustomDraw( ControlRenderer Renderer )
    {
      Renderer.RenderButton();

      /*
      int arrowWidth = 8;
      int arrowHeight = 5;
      int arrowX = ( _TopButton.Width - arrowWidth ) / 2;
      int arrowY = _TopButton.Height / 2 - 2;*/

      var rect = _TopButton.Bounds;

      Renderer.DrawArrowUp( rect.X, rect.Y, rect.Width, rect.Height, Enabled );
    }



    private void _BottomButton_CustomDraw( ControlRenderer Renderer )
    {
      Renderer.RenderButton();

      int arrowWidth = 8;
      int arrowHeight = 4;
      int arrowX = ( _TopButton.Width - arrowWidth ) / 2;
      int arrowY = _TopButton.Height / 2 - 2;

      Renderer.DrawArrowDown( arrowX, arrowY, arrowWidth, arrowHeight, Enabled );
    }



    private void OnScrollUp( ControlBase Sender )
    {
      if ( Value > Minimum )
      {
        --Value;
        Invalidate();

        Scroll?.Invoke( this );
      }
    }



    private void OnScrollDown( ControlBase Sender )
    {
      if ( Value < Maximum )
      {
        ++Value;
        Invalidate();

        Scroll?.Invoke( this );
      }
    }



    public event EventHandler Scroll;



    public SBDisplayType DisplayType { get; set; }



    protected override void OnSizeChanged( System.EventArgs e )
    {
      base.OnSizeChanged( e );

      if ( _TopButton.Width == 0 )
      {
        _TopButton.Bounds     = new System.Drawing.Rectangle( 0, 0, ClientSize.Width, _ButtonHeight );
        _BottomButton.Bounds  = new System.Drawing.Rectangle( 0, ClientSize.Height - _ButtonHeight, ClientSize.Width, _ButtonHeight );
      }
      _ButtonHeight = _TopButton.Height;
      if ( ClientSize.Height > 2 * _ButtonHeight )
      {
        _SliderHeight = 40;
      }
      else
      {
        _SliderHeight = 20;
      }
      Invalidate();
    }



    protected override void OnControlEvent( ControlEvent Event )
    {
      switch ( Event.Type )
      {
        case ControlEvent.EventType.MOUSE_WHEEL:
          if ( Value - Event.MouseWheelDelta * _WheelFactor < Minimum )
          {
            if ( Value > Minimum )
            {
              Value = Minimum;
              Invalidate();
              Scroll?.Invoke( this );
            }
          }
          else if ( Value - Event.MouseWheelDelta * _WheelFactor > Maximum )
          {
            if ( Value < Maximum )
            {
              Value = Maximum;
              Invalidate();
              Scroll?.Invoke( this );
            }
          }
          else
          {
            Value -= Event.MouseWheelDelta * _WheelFactor;
            Invalidate();
            Scroll?.Invoke( this );
          }
          UpdateMouseOverSlider( Event.MouseX, Event.MouseY );
          break;
        case ControlEvent.EventType.MOUSE_LEAVE:
          if ( _MouseOverSlider )
          {
            _MouseOverSlider = false;
            Invalidate();
          }
          break;
        case ControlEvent.EventType.MOUSE_UPDATE:
          _ButtonHeight = _TopButton.Height;
          if ( _SliderPushed )
          {
            // we're capturing
            int   newSliderPos = Event.MouseY - _ButtonHeight - _SliderDragOffset;
            if ( newSliderPos < 0 )
            {
              newSliderPos = 0;
            }
            if ( newSliderPos + _SliderHeight > ClientSize.Height - 2 * _ButtonHeight )
            {
              newSliderPos = ClientSize.Height - 2 * _ButtonHeight - _SliderHeight;
            }
            bool sliderPosChanged = _SliderDragPos != newSliderPos;
            _SliderDragPos = newSliderPos;
            int   newScrollPos = ( newSliderPos * ( Maximum - Minimum ) ) / ( ClientSize.Height - 2 * _ButtonHeight - _SliderHeight );
            if ( newScrollPos < Minimum )
            {
              newScrollPos = Minimum;
            }
            if ( newScrollPos > Maximum )
            {
              newScrollPos = Maximum;
            }

            if ( sliderPosChanged )
            {
              Invalidate();
              if ( Value != newScrollPos )
              {
                Value = newScrollPos;
                Scroll?.Invoke( this );
              }
            }
            break;
          }
          UpdateMouseOverSlider( Event.MouseX, Event.MouseY );
          break;
        case ControlEvent.EventType.MOUSE_DOWN:
          {
            _ButtonHeight = _TopButton.Height;

            int   sliderYPos = GetSliderRect().Top;

            Focus();

            if ( _MouseOverSlider )
            {
              if ( !_SliderPushed )
              {
                _SliderDragPos    = sliderYPos - _ButtonHeight;
                _SliderDragOffset = Event.MouseY - sliderYPos;
                _SliderPushed     = true;
                Invalidate();
              }
              Capture = true;
            }
            else if ( Event.MouseY < sliderYPos )
            {
              // above slider
              ScrollBy( -LargeChange );
            }
            else
            {
              // below slider
              ScrollBy( LargeChange );
            }
          }
          break;
        case ControlEvent.EventType.MOUSE_UP:
          if ( _SliderPushed )
          {
            _ButtonHeight = _TopButton.Height;

            _SliderPushed = false;
            Capture = false;
            if ( !GetSliderRect().Contains( Event.MouseX, Event.MouseY ) )
            {
              _MouseOverSlider = false;
            }
            Invalidate();
          }
          break;
        case ControlEvent.EventType.KEY_DOWN:
          if ( Focused )
          {
            if ( Event.Key == Keys.Down )
            {
              OnScrollDown( null );
            }
            else if ( Event.Key == Keys.PageDown )
            {
              ScrollBy( LargeChange );
            }
            else if ( Event.Key == Keys.Home )
            {
              ScrollTo( 0 );
            }
            else if ( Event.Key == Keys.Up )
            {
              OnScrollUp( null );
            }
            else if ( Event.Key == Keys.PageUp )
            {
              ScrollBy( -LargeChange );
            }
            else if ( Event.Key == Keys.End )
            {
              ScrollTo( Maximum );
            }
          }
          break;
          /*
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
            break;*/
      }
      base.OnControlEvent( Event );
    }



    public void ScrollBy( int Delta )
    {
      if ( Value + Delta > Maximum )
      {
        if ( Value != Maximum )
        {
          Value = Maximum;
          Invalidate();
          Scroll?.Invoke( this );
        }
      }
      else
      {
        Value += Delta;
        Invalidate();
        Scroll?.Invoke( this );
      }
    }



    public void ScrollTo( int TargetValue )
    {
      if ( TargetValue < 0 )
      {
        TargetValue = 0;
      }
      if ( TargetValue > Maximum )
      {
        TargetValue = Maximum;
      }
      if ( TargetValue != Value )
      {
        Value = TargetValue;
        Invalidate();
        Scroll?.Invoke( this );
      }
    }



    private void UpdateMouseOverSlider( int X, int Y )
    {
      if ( GetSliderRect().Contains( X, Y ) )
      {
        if ( !_MouseOverSlider )
        {
          _MouseOverSlider = true;
          Invalidate();
        }
      }
      else
      {
        if ( _MouseOverSlider )
        {
          _MouseOverSlider = false;
          Invalidate();
        }
      }
    }



    protected Rectangle GetSliderRect()
    {
      int     sliderYPos = 0;

      if ( _SliderPushed )
      {
        // during dragging the slider has a better resolution
        sliderYPos = _SliderDragPos;
      }
      else if ( Maximum - Minimum != 0 )
      {
        sliderYPos = ( ClientSize.Height - 2 * _ButtonHeight - _SliderHeight ) * Value / ( Maximum - Minimum );
      }

      return new Rectangle( 0, _ButtonHeight + sliderYPos, ClientSize.Width, _SliderHeight );
    }



    protected override void OnPaint( ControlRenderer Renderer )
    {
      if ( !Enabled )
      {
        return;
      }
      var sliderRect = GetSliderRect();
      Renderer.RenderSlider( sliderRect.Left, sliderRect.Top, sliderRect.Width, sliderRect.Height, _MouseOverSlider, _SliderPushed );
    }



    public void SetSliderSize( int SliderSize )
    {
      _SliderHeight = SliderSize;
      Invalidate();
    }



  }
}