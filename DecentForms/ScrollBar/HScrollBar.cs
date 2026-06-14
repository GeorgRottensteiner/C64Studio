using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;



namespace DecentForms
{
  public class HScrollBar : ScrollBar
  {
    private Button    _TopButton = new Button();
    private Button    _BottomButton = new Button();

    private int       _ButtonWidth = 16;
    private int       _SliderWidth = 40;



    public HScrollBar()
    {
      DisplayType = SBDisplayType.RAISED;
      BorderStyle = BorderStyle.NONE;
      Height = 17;

      _TopButton.CanBeFocused = false;
      _TopButton.Click += OnScrollUp;
      _TopButton.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
      _TopButton.CustomDraw += _TopButton_CustomDraw;

      _BottomButton.CanBeFocused = false;
      _BottomButton.Click += OnScrollDown;
      _BottomButton.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
      _BottomButton.CustomDraw += _BottomButton_CustomDraw;

      Controls.Add( _TopButton );
      Controls.Add( _BottomButton );
    }



    private void OnScrollUp( ControlBase Sender )
    {
      if ( Value > Minimum )
      {
        --Value;
        Invalidate();

        OnScroll();
      }
    }



    private void OnScrollDown( ControlBase Sender )
    {
      if ( Value < Maximum )
      {
        ++Value;
        Invalidate();

        OnScroll();
      }
    }



    private void _TopButton_CustomDraw( ControlRenderer Renderer )
    {
      Renderer.RenderButton();

      var rect = _TopButton.Bounds;

      Renderer.DrawArrowLeft( rect.X, rect.Y, rect.Width, rect.Height, Enabled );
    }



    private void _BottomButton_CustomDraw( ControlRenderer Renderer )
    {
      Renderer.RenderButton();

      int arrowWidth = 8;
      int arrowHeight = 4;
      int arrowX = ( _TopButton.Width - arrowWidth ) / 2;
      int arrowY = _TopButton.Height / 2 - 2;

      Renderer.DrawArrowRight( arrowX, arrowY, arrowWidth, arrowHeight, Enabled );
    }



    protected override void OnSizeChanged( System.EventArgs e )
    {
      base.OnSizeChanged( e );

      if ( _TopButton.Width == 0 )
      {
        _TopButton.Bounds = new System.Drawing.Rectangle( 0, 0, _ButtonWidth, ClientSize.Height );
        _BottomButton.Bounds = new System.Drawing.Rectangle( ClientSize.Width - _ButtonWidth, 0, _ButtonWidth, ClientSize.Height );
      }
      _ButtonWidth = _TopButton.Width;

      if ( ClientSize.Width > 2 * _ButtonWidth )
      {
        _SliderWidth = 40;
      }
      else
      {
        _SliderWidth = 20;
      }

      Invalidate();
    }



    protected override void OnControlEvent( ControlEvent Event )
    {
      switch ( Event.Type )
      {
        case ControlEvent.EventType.MOUSE_UPDATE:
          _ButtonWidth = _TopButton.Width;
          if ( _SliderPushed )
          {
            // we're capturing
            int   newSliderPos = Event.MouseX - _ButtonWidth - _SliderDragOffset;
            if ( newSliderPos < 0 )
            {
              newSliderPos = 0;
            }
            if ( newSliderPos + _SliderWidth > ClientSize.Width - 2 * _ButtonWidth )
            {
              newSliderPos = ClientSize.Width - 2 * _ButtonWidth - _SliderWidth;
            }
            bool sliderPosChanged = _SliderDragPos != newSliderPos;
            _SliderDragPos = newSliderPos;
            int   newScrollPos = ( newSliderPos * ( Maximum - Minimum ) ) / ( ClientSize.Width - 2 * _ButtonWidth - _SliderWidth );
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
                OnScroll();
              }
            }
            break;
          }
          UpdateMouseOverSlider( Event.MouseX, Event.MouseY );
          break;
        case ControlEvent.EventType.MOUSE_DOWN:
          {
            _ButtonWidth = _TopButton.Width;

            int   sliderXPos = GetSliderRect().Left;

            Focus();

            if ( _MouseOverSlider )
            {
              if ( !_SliderPushed )
              {
                _SliderDragPos    = sliderXPos - _ButtonWidth;
                _SliderDragOffset = Event.MouseX - sliderXPos;
                _SliderPushed = true;
                Invalidate(); 
              }
              Capture = true;
            }
            else if ( Event.MouseX < sliderXPos )
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
            _ButtonWidth = _TopButton.Width;
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
            if ( Event.Key == Keys.Right )
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
            else if ( Event.Key == Keys.Left )
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



    protected override Rectangle GetSliderRect()
    {
      int     sliderXPos = 0;
      if ( _SliderPushed )
      {
        // during dragging the slider has a better resolution
        sliderXPos = _SliderDragPos;
      }
      else if ( Maximum - Minimum != 0 )
      {
        sliderXPos = ( ClientSize.Width - 2 * _ButtonWidth - _SliderWidth ) * Value / ( Maximum - Minimum );
      }

      return new Rectangle( _ButtonWidth + sliderXPos, 0, _SliderWidth, ClientSize.Height );
    }



    public void SetSliderSize( int SliderSize )
    {
      if ( SliderSize < 20 )
      {
        SliderSize = 20;
      }
      _SliderWidth = SliderSize;
      Invalidate();
    }



  }
}