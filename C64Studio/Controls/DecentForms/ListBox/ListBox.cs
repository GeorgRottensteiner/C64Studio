using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;



namespace DecentForms
{
  public partial class ListBox : ControlBase
  {
    private VScrollBar    _ScrollBar = new VScrollBar();
    private HScrollBar    _ScrollBarH = new HScrollBar();
    private bool          _ScrollAlwaysVisible = false;
    private int           _MouseOverItem = -1;
    private int           _SelectedIndex = -1;

    // cache max item width (since that's resource heavy, -1 means required to recalc)
    private int           _CachedMaxItemWidth = -1;

    public event EventHandler       SelectedIndexChanged;



    public ListBox()
    {
      SelectedIndices   = new ListBoxItemIndexCollection( this );
      SelectedItems     = new ListBoxSelectedItemCollection( this );
      Items             = new ListBoxItemCollection( this );

      BorderStyle       = BorderStyle.SUNKEN;

      

      Controls.Add( _ScrollBar );
      Controls.Add( _ScrollBarH );
      _ScrollBar.Scroll += _ScrollBar_Scroll;
      _ScrollBarH.Scroll += _ScrollBarH_Scroll;
      UpdateScrollbarState();
    }



    private void _ScrollBar_Scroll( ControlBase Sender )
    {
      Invalidate();
    }



    private void _ScrollBarH_Scroll( ControlBase Sender )
    {
      _DisplayOffsetX = _ScrollBarH.Value;
      Invalidate();
    }



    protected override bool IsInputKey( Keys keyData )
    {
      return true;
    }



    public int ItemHeight { get; set; } = 15;
    public ListBoxItemCollection          Items { get; private set; }
    public ListBoxItemIndexCollection     SelectedIndices { get; private set; }
    public ListBoxSelectedItemCollection  SelectedItems { get; private set; }
    public SelectionMode                  SelectionMode { get; set; }




    public bool ScrollAlwaysVisible 
    {
      get
      {
        return _ScrollAlwaysVisible;
      }
      set 
      {
        _ScrollAlwaysVisible = value;
        UpdateScrollbarState();
        Invalidate(); 
      } 
    }



    public int FirstVisibleItemIndex
    {
      get
      {
        return _ScrollBar.Value;
      }
    }



    private void UpdateScrollbarState()
    {
      bool    needVerticalScrollbar   = VerticalScrollbarRequired();
      bool    needHorizontalScrollbar = HorizontalScrollbarRequired();

      int     potentialVScrollWidth = needVerticalScrollbar ? _ScrollBar.Width : 0;

      if ( MaxItemWidth > ClientSize.Width - potentialVScrollWidth )
      {
        _ScrollBarH.Visible = true;
        _ScrollBarH.Bounds = new System.Drawing.Rectangle( 0, ClientSize.Height - _ScrollBarH.Height, ClientSize.Width - potentialVScrollWidth, _ScrollBarH.Height );

        _ScrollBarH.Maximum = MaxItemWidth - ( ClientSize.Width - potentialVScrollWidth );

        float factor = ( ClientSize.Width - potentialVScrollWidth ) / (float)MaxItemWidth;
        _ScrollBarH.SetSliderSize( (int)( ( _ScrollBarH.Width - 2 * 17 ) * factor ) );

      }
      else
      {
        _ScrollBarH.Visible = false;
      }

      if ( needVerticalScrollbar )
      {
        _ScrollBar.Visible  = true;

        int usableHeight = ClientSize.Height;
        if ( needHorizontalScrollbar )
        {
          usableHeight -= _ScrollBarH.Height;
        }
        _ScrollBar.Bounds = new System.Drawing.Rectangle( ClientSize.Width - _ScrollBar.Width, 0, _ScrollBar.Width, usableHeight );

        int   visibleItemCount = 1;
        if ( ItemHeight == 0 )
        {
          visibleItemCount = 1;
        }
        else if ( needHorizontalScrollbar )
        {
          visibleItemCount = ( ClientSize.Height - _ScrollBarH.Height ) / ItemHeight;
        }
        else
        {
          visibleItemCount = ClientSize.Height / ItemHeight;
        }
        int     newMax = Items.Count - visibleItemCount;
        if ( Items.Count == 0 )
        {
          _ScrollBar.Maximum = 0;
          _ScrollBar.SetSliderSize( _ScrollBar.Height - 2 * 17 );
        }
        else
        {

          if ( _ScrollBar.Value > newMax )
          {
            _ScrollBar.Value = newMax;
          }
          _ScrollBar.Maximum = newMax;

          float factor = usableHeight / ( (float)Items.Count * ItemHeight );
          _ScrollBar.SetSliderSize( (int)( ( _ScrollBar.Height - 2 * 17 ) * factor ) );
        }
      }
      else
      {
        if ( !_ScrollAlwaysVisible )
        {
          _ScrollBar.Visible = false;
        }
        _ScrollBar.Value = 0;
        _ScrollBar.Maximum = 0;
      }

      _ActualWorkWidth = 0;
      _ActualWorkHeight = 0;
      if ( needVerticalScrollbar )
      {
        _ActualWorkWidth = ClientSize.Width - _ScrollBar.Width;
      }
      if ( needHorizontalScrollbar )
      {
        _ActualWorkHeight = ClientSize.Height - _ScrollBarH.Height;
      }
    }



    private bool HorizontalScrollbarRequired()
    {
      if ( VerticalScrollbarRequired() )
      {
        return MaxItemWidth > ClientSize.Width - _ScrollBar.Width;
      }
      return MaxItemWidth > ClientSize.Width;
    }



    protected override void OnSizeChanged( System.EventArgs e )
    {
      base.OnSizeChanged( e );

      UpdateScrollbarState();
    }



    private bool VerticalScrollbarRequired()
    {
      if ( Items == null )
      {
        return false;
      }
      return ( Items.Count > VisibleItemCount ) 
          || ( _ScrollAlwaysVisible );
    }



    public int VisibleItemCount
    {
      get
      {
        if ( ItemHeight == 0 )
        {
          return 1;
        }
        if ( _ScrollBarH.Visible )
        {
          return ( ClientSize.Height - _ScrollBarH.Height ) / ItemHeight;
        }
        return ClientSize.Height / ItemHeight;
      }
    }



    public int UsableItemWidth
    {
      get
      {
        if ( !_ScrollBar.Visible )
        {
          return Math.Max( MaxItemWidth, ClientSize.Width );
        }
        return Math.Max( MaxItemWidth, ClientSize.Width - _ScrollBar.Width );
      }
    }



    public int MouseOverItem
    {
      get
      {
        return _MouseOverItem;
      }
    }



    public ListBoxItem SelectedItem
    {
      get
      {
        if ( _SelectedIndex == -1 )
        {
          return null;
        }
        return Items[_SelectedIndex];
      }
      set
      {
        if ( value == null )
        {
          SelectedIndex = -1;
          return;
        }
        int index = Items.IndexOf( value );
        if ( index == -1 )
        {
          throw new IndexOutOfRangeException( $"SelectedItem, {value} is not in item list" );
        }
        SelectedIndex = index;
      }
    }



    public int SelectedIndex 
    {
      get
      {
        return _SelectedIndex;
      }
      set
      {
        if ( ( value < -1 )
        ||   ( value >= Items.Count ) )
        {
          throw new IndexOutOfRangeException( $"SelectedIndex {value} is invalid, only {Items.Count} items exist" );
        }
        // TODO - clear multi selection! 
        if ( _SelectedIndex != value )
        {
          SelectedItems.Clear();

          _SelectedIndex = value;
          if ( _SelectedIndex != -1 )
          {
            Items[_SelectedIndex].Selected = true;
          }

          SelectedIndexChanged?.Invoke( this );
          if ( _SelectedIndex == -1 )
          {
            return;
          }
          if ( _SelectedIndex < FirstVisibleItemIndex )
          {
            _ScrollBar.Value = _SelectedIndex;
            Invalidate();
          }
          else if ( _SelectedIndex >= FirstVisibleItemIndex + VisibleItemCount )
          {
            _ScrollBar.Value = Math.Max( 0, _SelectedIndex - VisibleItemCount + 1 );
            Invalidate();
          }
          else if ( _SelectedIndex != -1 )
          {
            Invalidate( GetItemRect( _SelectedIndex ) );
          }
        }
      }
    }



    protected int MaxItemWidth 
    {
      get
      {
        if ( _CachedMaxItemWidth != -1 )
        {
          return _CachedMaxItemWidth;
        }
        RecalcMaxItemWidth();
        return _CachedMaxItemWidth;
      }
    }



    private void RecalcMaxItemWidth()
    {
      if ( Items == null )
      {
        _CachedMaxItemWidth = 0;
        return;
      }

      int     curMaxWidth = 0;

      for ( int i = 0; i < Items.Count; ++i )
      {
        var textSize = TextRenderer.MeasureText( Items[i].Text, Font );

        curMaxWidth = Math.Max( textSize.Width, curMaxWidth );
      }
      _CachedMaxItemWidth = curMaxWidth;
    }



    protected override void OnControlEvent( ControlEvent Event )
    {
      switch ( Event.Type )
      {
        case ControlEvent.EventType.FOCUS_LOST:
          Invalidate();
          break;
        case ControlEvent.EventType.FOCUSED:
          Invalidate();
          break;
        case ControlEvent.EventType.MOUSE_ENTER:
          Invalidate();
          break;
        case ControlEvent.EventType.MOUSE_WHEEL:
          if ( ( _ScrollBarH.Visible )
          &&   ( _ScrollBarH.Bounds.Contains( Event.MouseX, Event.MouseY ) ) )
          {
            _ScrollBarH.RaiseControlEvent( Event );
          }
          else if ( _ScrollBar.Visible )
          {
            _ScrollBar.RaiseControlEvent( Event );
          }
          break;
        case ControlEvent.EventType.MOUSE_UPDATE:
          {
            int   itemBelow = ItemIndexFromPosition( Event.MouseX, Event.MouseY );
            if ( itemBelow != _MouseOverItem )
            {
              if ( _MouseOverItem != -1 )
              {
                Invalidate( GetItemRect( _MouseOverItem ) );
              }
              _MouseOverItem = itemBelow;
              if ( _MouseOverItem != -1 )
              {
                Invalidate( GetItemRect( _MouseOverItem ) );
              }
            }
            
          }
          break;
        case ControlEvent.EventType.MOUSE_LEAVE:
          if ( _MouseOverItem != -1 )
          {
            Invalidate( GetItemRect( _MouseOverItem ) );
            _MouseOverItem = -1;
          }
          Invalidate();
          break;
        case ControlEvent.EventType.MOUSE_DOWN:
          Focus();
          if ( _MouseOverItem != SelectedIndex )
          {
            SelectedIndex = _MouseOverItem;
          }
          Capture     = true;
          break;
        case ControlEvent.EventType.MOUSE_UP:
          Capture = false;
          Invalidate();
          break;
        case ControlEvent.EventType.KEY_DOWN:
          if ( Event.Key == Keys.Down )
          {
            int   newIndex = _SelectedIndex;
            if ( ( _SelectedIndex == -1 )
            &&   ( Items.Count > 0 ) )
            {
              newIndex = 0;
            }
            else if ( _SelectedIndex + 1 < Items.Count )
            {
              newIndex = _SelectedIndex + 1;
            }
            if ( newIndex != _SelectedIndex )
            {
              SelectedIndex = newIndex;
            }
          }
          else if ( Event.Key == Keys.Up )
          {
            int   newIndex = _SelectedIndex;
            if ( ( _SelectedIndex == -1 )
            &&   ( Items.Count > 0 ) )
            {
              newIndex = Items.Count - 1;
            }
            else if ( _SelectedIndex > 0 )
            {
              newIndex = _SelectedIndex - 1;
            }
            if ( newIndex != _SelectedIndex )
            {
              SelectedIndex = newIndex;
            }
          }
          else if ( Event.Key == Keys.PageUp )
          {
            int   newIndex = _SelectedIndex;
            if ( ( _SelectedIndex == -1 )
            &&   ( Items.Count > 0 ) )
            {
              newIndex = Items.Count - 1;
            }
            else
            {
              newIndex = Math.Max( 0, _SelectedIndex - VisibleItemCount + 1 );
            }
            if ( newIndex != _SelectedIndex )
            {
              SelectedIndex = newIndex;
            }
          }
          else if ( Event.Key == Keys.PageDown )
          {
            int   newIndex = _SelectedIndex;
            if ( ( _SelectedIndex == -1 )
            &&   ( Items.Count > 0 ) )
            {
              newIndex = 0;
            }
            else
            {
              newIndex = Math.Min( Items.Count - 1, _SelectedIndex + VisibleItemCount - 1 );
            }
            if ( newIndex != _SelectedIndex )
            {
              SelectedIndex = newIndex;
            }
          }
          else if ( Event.Key == Keys.Home )
          {
            if ( Items.Count > 0 )
            {
              if ( _SelectedIndex != 0 )
              {
                SelectedIndex = 0;
              }
            }
          }
          else if ( Event.Key == Keys.End )
          {
            if ( Items.Count > 0 )
            {
              if ( _SelectedIndex + 1 != Items.Count )
              {
                SelectedIndex = Items.Count - 1;
              }
            }
          }
          break;
        case ControlEvent.EventType.KEY_UP:
          if ( Focused )
          {
            if ( Event.Key == Keys.Space )
            {
            }
          }
          break;
      }
      base.OnControlEvent( Event );
    }



    private int ItemIndexFromPosition( int X, int Y )
    {
      if ( ( X < 0 )
      ||   ( X >= UsableItemWidth )
      ||   ( Y < 0 )
      ||   ( Y >= ClientSize.Height ) )
      {
        return -1;
      }
      int   itemIndex = FirstVisibleItemIndex + Y / ItemHeight;
      if ( itemIndex >= Items.Count )
      {
        return -1;
      }
      return itemIndex;
    }



    protected override void OnPaint( ControlRenderer Renderer )
    {
      Renderer.RenderListBox();
    }



    private int IndexOfItem( ListBoxItem Item )
    {
      int     index = 0;
      foreach ( var item in Items )
      {
        if ( item == Item )
        {
          return index;
        }
        ++index;
      }
      return -1;
    }



    private void SelectItem( ListBoxItem Item )
    {
      if ( !SelectedItems.Contains( Item ) )
      {
        SelectedItems.Add( Item );
      }
      if ( !SelectedIndices.Contains( Item.Index ) )
      {
        SelectedIndices.Add( Item.Index );
      }
      if ( _SelectedIndex == -1 )
      {
        _SelectedIndex = Item.Index;
      }
      Invalidate( GetItemRect( Item.Index ) );
    }



    private void UnselectItem( ListBoxItem Item )
    {
      if ( SelectedItems.Contains( Item ) )
      {
        SelectedItems.Remove( Item );
      }
      else
      {
        Debug.Log( "was not inside SelectedItems!" );
      }
      if ( SelectedIndices.Contains( Item.Index ) )
      {
        SelectedIndices.Remove( Item.Index );
      }
      else
      {
        Debug.Log( "was not inside SelectedIndices!" );
      }
      Invalidate( GetItemRect( Item.Index ) );
    }



    private void ItemModified( ListBoxItem Item )
    {
      _CachedMaxItemWidth = -1;
      Invalidate( GetItemRect( Item.Index ) );
    }



    internal Rectangle GetItemRect( int ItemIndex )
    {
      if ( ( ItemIndex < FirstVisibleItemIndex )
      ||   ( ItemIndex >= Items.Count ) )
      {
        return new Rectangle();
      }
      // TODO - check, multi column
      return new Rectangle( 0, ( ItemIndex - FirstVisibleItemIndex ) * ItemHeight, UsableItemWidth, ItemHeight );
    }



    private void FixItemIndices()
    {
      int   itemIndex = 0;
      foreach ( var item in Items )
      {
        item._Index = itemIndex;
        ++itemIndex;
      }
    }



    private void ItemsModified()
    {
      FixItemIndices();
      _CachedMaxItemWidth = -1;
      UpdateScrollbarState();
      Invalidate();
    }



  }



}