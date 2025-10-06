using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;



namespace DecentForms
{
  public partial class GridList : ControlBase
  {
    private VScrollBar    _ScrollBarV = new VScrollBar();
    private HScrollBar    _ScrollBarH = new HScrollBar();
    private bool          _ScrollAlwaysVisible = false;
    private int           _MouseOverItem = -1;
    private int           _SelectedIndex = -1;

    private bool          _UpdateLocked = false;
    private bool          _RedrawRequired= false;

    public event EventHandler       SelectedIndexChanged;

    public delegate void DrawGridListItemEventHandler( DecentForms.ControlBase Sender, GridListItemEventArgs e );
    public delegate void ForwardedEventHandler( DecentForms.ControlBase Sender, GridListItem item, DecentForms.ControlEvent e );

    public event DrawGridListItemEventHandler   DrawItem;

    [Description( "Called when CustomMouseHandling is true and mouse/keyboard events occur" )]
    public event ForwardedEventHandler          CustomEventHandler;



    public GridList()
    {
      SelectedIndices   = new GridListItemIndexCollection( this );
      SelectedItems     = new GridListSelectedItemCollection( this );
      Items             = new GridListItemCollection( this );

      BorderStyle       = BorderStyle.SUNKEN;

      

      Controls.Add( _ScrollBarV );
      Controls.Add( _ScrollBarH );
      _ScrollBarV.Dock = DockStyle.Right;
      _ScrollBarV.Scroll += _ScrollBar_Scroll;
      _ScrollBarH.Scroll += _ScrollBarH_Scroll;
      AdjustScrollbars();
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



    public int ItemWidth { get; set; } = 32;
    public int ItemHeight { get; set; } = 15;

    [Description( "If set to true, mouse handling (hover, click) is not done by the control, but sent as events")]
    public bool CustomMouseHandling { get; set; } = false;

    private int _itemsPerLine = 1;

    public int ItemsPerLine
    {
      get
      {
        return _itemsPerLine;
      }
    }

    public GridListItemCollection          Items { get; private set; }
    public GridListItemIndexCollection     SelectedIndices { get; private set; }
    public GridListSelectedItemCollection  SelectedItems { get; private set; }
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
        AdjustScrollbars();
        Invalidate(); 
      } 
    }



    public int FirstVisibleItemIndex
    {
      get
      {
        return _ScrollBarV.Value;
      }
    }



    private bool _InsideAdjustScrollbars = false;

    private void AdjustScrollbars()
    {
      if ( _InsideAdjustScrollbars )
      {
        return;
      }

      _InsideAdjustScrollbars = true;
      if ( ItemHeight == 0 )
      {
        _ScrollBarV.Visible = false;
        _ScrollBarV.Value = 0;

        _InsideAdjustScrollbars = false;
        return;
      }
      int   actualWidth = ClientRectangle.Width;
      int   actualHeight = ClientRectangle.Height;
      if ( _ScrollBarH.Visible )
      {
        actualHeight -= _ScrollBarH.Height;
      }
      int visibleItemsVertical = actualHeight / ItemHeight;
      if ( visibleItemsVertical <= 0 )
      {
        visibleItemsVertical = 1;
      }

      if ( _ScrollBarV.Visible )
      {
        // verify if we could fit all items without the scrollbar
        actualWidth += System.Windows.Forms.SystemInformation.VerticalScrollBarWidth;
        int   potentialItemsPerLine = actualWidth / ItemWidth;
        if ( potentialItemsPerLine <= 0 )
        {
          potentialItemsPerLine = 1;
        }
        int scrollLength = ( ( Items.Count + potentialItemsPerLine - 1 ) / potentialItemsPerLine ) - visibleItemsVertical;
        if ( scrollLength <= 0 )
        {
          _itemsPerLine = potentialItemsPerLine;
          _ScrollBarV.Visible = false;
          _ScrollBarV.Value = 0;
          _InsideAdjustScrollbars = false;
          return;
        }
        actualWidth -= System.Windows.Forms.SystemInformation.VerticalScrollBarWidth;
      }
      _itemsPerLine = actualWidth / ItemWidth;
      if ( _itemsPerLine <= 0 )
      {
        _itemsPerLine = 1;
      }
      int scrollLength2 = ( ( Items.Count + _itemsPerLine - 1 ) / _itemsPerLine ) - visibleItemsVertical;

      if ( scrollLength2 <= 0 )
      {
        _ScrollBarV.Visible = false;
        _ScrollBarV.Value = 0;
      }
      else
      {
        _ScrollBarV.Maximum = scrollLength2;
        if ( _ScrollBarV.Value > _ScrollBarV.Maximum )
        {
          _ScrollBarV.Maximum = 0;
        }
        _ScrollBarV.Visible = true;
      }
      _InsideAdjustScrollbars = false;
    }



    private bool HorizontalScrollbarRequired()
    {
      if ( VerticalScrollbarRequired() )
      {
        return ItemWidth > ClientSize.Width - _ScrollBarV.Width;
      }
      return ItemWidth > ClientSize.Width;
    }



    protected override void OnSizeChanged( System.EventArgs e )
    {
      base.OnSizeChanged( e );

      AdjustScrollbars();
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
          return ItemsPerLine * 1;
        }
        if ( _ScrollBarH.Visible )
        {
          return ItemsPerLine * ( ClientSize.Height - _ScrollBarH.Height ) / ItemHeight;
        }
        return ItemsPerLine * ClientSize.Height / ItemHeight;
      }
    }



    public int UsableItemWidth
    {
      get
      {
        if ( !_ScrollBarV.Visible )
        {
          return Math.Max( 1, ClientSize.Width / ItemWidth * ItemWidth );
        }
        return Math.Max( 1, ( ClientSize.Width - _ScrollBarV.Width  ) / ItemWidth * ItemWidth );
      }
    }



    public int MouseOverItem
    {
      get
      {
        return _MouseOverItem;
      }
    }



    public GridListItem SelectedItem
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



    public new void Invalidate()
    {
      if ( _UpdateLocked )
      {
        _RedrawRequired = true;
      }
      else
      {
        base.Invalidate();
      }
    }



    public new void Invalidate( Rectangle rect )
    {
      if ( _UpdateLocked )
      {
        _RedrawRequired = true;
      }
      else
      {
        base.Invalidate( rect );
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
          SelectedIndices.Clear();

          _SelectedIndex = value;
          if ( _SelectedIndex != -1 )
          {
            Items[_SelectedIndex]._Selected = true;
            SelectedItems.Add( Items[_SelectedIndex] );
            SelectedIndices.Add( _SelectedIndex );
          }

          SelectedIndexChanged?.Invoke( this );
          if ( _SelectedIndex == -1 )
          {
            return;
          }
          if ( _SelectedIndex < FirstVisibleItemIndex )
          {
            _ScrollBarV.Value = _SelectedIndex;
            Invalidate();
          }
          else if ( _SelectedIndex >= FirstVisibleItemIndex + VisibleItemCount )
          {
            _ScrollBarV.Value = Math.Max( 0, _SelectedIndex - VisibleItemCount + 1 );
            Invalidate();
          }
          else if ( _SelectedIndex != -1 )
          {
            Invalidate( GetItemRect( _SelectedIndex ) );
          }
        }
      }
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
          _MouseOverItem = ItemIndexFromPosition( Event.MouseX, Event.MouseY );
          if ( _MouseOverItem != -1 )
          {
            Invalidate( GetItemRect( _MouseOverItem ) );
            if ( CustomMouseHandling )
            {
              CustomEventHandler?.Invoke( this, Items[_MouseOverItem], Event );
            }
          }
          Invalidate();
          break;
        case ControlEvent.EventType.MOUSE_WHEEL:
          if ( ( CustomMouseHandling )
          &&   ( _MouseOverItem != -1 ) )
          {
            CustomEventHandler?.Invoke( this, Items[_MouseOverItem], Event );
            break;
          }

          if ( ( _ScrollBarH.Visible )
          &&   ( _ScrollBarH.Bounds.Contains( Event.MouseX, Event.MouseY ) ) )
          {
            _ScrollBarH.RaiseControlEvent( Event );
          }
          else if ( _ScrollBarV.Visible )
          {
            _ScrollBarV.RaiseControlEvent( Event );
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
            if ( ( CustomMouseHandling )
            &&   ( _MouseOverItem != -1 ) )
            {
              CustomEventHandler?.Invoke( this, Items[_MouseOverItem], Event );
              break;
            }
          }
          break;
        case ControlEvent.EventType.MOUSE_LEAVE:
          if ( _MouseOverItem != -1 )
          {
            if ( CustomMouseHandling )
            {
              CustomEventHandler?.Invoke( this, Items[_MouseOverItem], Event );
            }
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
          if ( ( CustomMouseHandling )
          &&   ( _MouseOverItem != -1 ) )
          {
            CustomEventHandler?.Invoke( this, Items[_MouseOverItem], Event );
          }
          Capture     = true;
          break;
        case ControlEvent.EventType.MOUSE_UP:
          Capture = false;
          Invalidate();
          if ( ( CustomMouseHandling )
          &&   ( _MouseOverItem != -1 ) )
          {
            CustomEventHandler?.Invoke( this, Items[_MouseOverItem], Event );
          }
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

          if ( ( CustomMouseHandling )
          &&   ( SelectedIndex != -1 ) )
          {
            CustomEventHandler?.Invoke( this, Items[SelectedIndex], Event );
          }
          break;
        case ControlEvent.EventType.KEY_UP:
          if ( Focused )
          {
            if ( ( CustomMouseHandling )
            &&   ( SelectedIndex != -1 ) )
            {
              CustomEventHandler?.Invoke( this, Items[SelectedIndex], Event );
            }
          }
          break;
        case ControlEvent.EventType.SET_CURSOR:
          if ( ( CustomMouseHandling )
          &&   ( _MouseOverItem != -1 ) )
          {
            CustomEventHandler?.Invoke( this, Items[_MouseOverItem], Event );
            if ( Event.Handled )
            {
              return;
            }
          }
          break;
      }
      base.OnControlEvent( Event );
    }



    public int ItemIndexFromPosition( int X, int Y )
    {
      if ( ( X < 0 )
      ||   ( X >= UsableItemWidth )
      ||   ( Y < 0 )
      ||   ( Y >= ClientSize.Height ) )
      {
        return -1;
      }
      int   localIndex = ( X / ItemWidth ) + ( Y / ItemHeight ) * ItemsPerLine;
      int   itemIndex = FirstVisibleItemIndex + localIndex;
      if ( itemIndex >= Items.Count )
      {
        return -1;
      }
      return itemIndex;
    }



    protected override void OnPaint( ControlRenderer Renderer )
    {
      Renderer.RenderGridList();
    }



    internal void RenderItem( ControlRenderer renderer, GridListItem item, Rectangle bounds )
    {
      if ( DrawItem != null )
      {
        renderer.SetClip( 0, 0, bounds.Width, bounds.Height, bounds.X, bounds.Y );
        var localBounds = new Rectangle( 0, 0, bounds.Width, bounds.Height );

        DrawItem( this, new GridListItemEventArgs( renderer, item, localBounds ) );

        renderer.RestoreClip();
        return;
      }

      renderer.RenderGridListItem( item, bounds );
    }



    private int IndexOfItem( GridListItem Item )
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



    private void SelectItem( GridListItem Item )
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



    private void UnselectItem( GridListItem Item )
    {
      if ( SelectedItems.Contains( Item ) )
      {
        SelectedItems.Remove( Item );
      }
      if ( SelectedIndices.Contains( Item.Index ) )
      {
        SelectedIndices.Remove( Item.Index );
      }
      Invalidate( GetItemRect( Item.Index ) );
    }



    private void ItemModified( GridListItem Item )
    {
      RebuildVisibleItemIndices();
      Invalidate( GetItemRect( Item.Index ) );
    }



    private void RebuildVisibleItemIndices()
    {
      int   visibleIndex = 0;

      foreach ( var item in Items )
      {
        if ( item.Visible )
        {
          item._VisibleIndex = visibleIndex;
          ++visibleIndex;
        }
        else
        {
          item._VisibleIndex = -1;
        }
      }
    }



    internal Rectangle GetItemRect( int ItemIndex )
    {
      if ( ( ItemIndex < FirstVisibleItemIndex )
      ||   ( ItemIndex >= Items.Count )
      ||   ( !Items[ItemIndex].Visible ) )
      {
        return Rectangle.Empty;
      }
      int  localIndex = Items[ItemIndex]._VisibleIndex - FirstVisibleItemIndex;

      return new Rectangle( ( localIndex % ItemsPerLine ) * ItemWidth, 
                            localIndex / ItemsPerLine * ItemHeight,
                            ItemWidth, ItemHeight );
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
      RebuildVisibleItemIndices();
      AdjustScrollbars();
      Invalidate();
    }



    public void BeginUpdate()
    {
      _UpdateLocked = true;
      _RedrawRequired = false;
    }



    public void EndUpdate()
    {
      if ( _UpdateLocked )
      {
        _UpdateLocked = false;
        if ( _RedrawRequired )
        {
          Invalidate();
        }
      }
    }



  }



  public class GridListItemEventArgs : EventArgs
  {
    private readonly ControlRenderer        _renderer;

    private readonly GridList.GridListItem  _item;

    private readonly Rectangle              _bounds;



    public GridList.GridListItem Item => _item;

    public ControlRenderer Renderer => _renderer;

    public Rectangle Bounds => _bounds;



    public GridListItemEventArgs( ControlRenderer Renderer, GridList.GridListItem Item, Rectangle bounds )
    {
      _renderer = Renderer;
      _item     = Item;
      _bounds   = bounds;
    }
  }



}