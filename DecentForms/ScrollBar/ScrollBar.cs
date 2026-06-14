using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;



namespace DecentForms
{
  public abstract class ScrollBar : ControlBase
  {
    public enum SBDisplayType
    {
      RAISED,
      FLAT
    }

    protected bool      _MouseOverSlider = false;
    protected bool      _SliderPushed = false;
    protected int       _SliderDragOffset = 0;
    protected int       _SliderDragPos = 0;
    protected int       _MouseWheelFactor = 3;

    protected int       _Value = 0;
    protected int       _Minimum = 0;
    protected int       _Maximum = 100;



    public int Minimum
    {
      get
      {
        return _Minimum;
      }
      set
      {
        if ( value < Maximum )
        {
          _Minimum = value;
          Value = _Value;
          Invalidate();
        }
      }
    }



    public int Maximum 
    {
      get
      {
        return _Maximum;
      }
      set
      {
        if ( value >= Minimum )
        {
          _Maximum = value;
          Value = _Value;
          Invalidate();
        }
      }
    }



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



    public ScrollBar()
    {
      DisplayType = SBDisplayType.RAISED;
      BorderStyle = BorderStyle.NONE;
    }



    protected void OnScroll()
    {
      Scroll?.Invoke( this );
    }



    public event EventHandler Scroll;



    public SBDisplayType DisplayType { get; set; }



    protected override void OnControlEvent( ControlEvent Event )
    {
      switch ( Event.Type )
      {
        case ControlEvent.EventType.MOUSE_WHEEL:
          if ( Value - Event.MouseWheelDelta * _MouseWheelFactor < Minimum )
          {
            if ( Value > Minimum )
            {
              Value = Minimum;
              Invalidate();
              Scroll?.Invoke( this );
            }
          }
          else if ( Value - Event.MouseWheelDelta * _MouseWheelFactor > Maximum )
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
            Value -= Event.MouseWheelDelta * _MouseWheelFactor;
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
        case ControlEvent.EventType.KEY_DOWN:
          if ( Focused )
          {
            if ( Event.Key == Keys.Down )
            {
              ScrollBy( 1 );
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
              ScrollBy( -1 );
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



    protected void UpdateMouseOverSlider( int X, int Y )
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



    protected abstract Rectangle GetSliderRect();



    protected override void OnPaint( ControlRenderer Renderer )
    {
      if ( !Enabled )
      {
        return;
      }
      var sliderRect = GetSliderRect();
      Renderer.RenderSlider( sliderRect.Left, sliderRect.Top, sliderRect.Width, sliderRect.Height, _MouseOverSlider, _SliderPushed );
    }



  }
}