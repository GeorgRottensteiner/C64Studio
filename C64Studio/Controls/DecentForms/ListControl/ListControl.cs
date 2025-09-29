using RetroDevStudio.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;



namespace DecentForms
{
  public partial class ListControl : ControlBase
  {
    private VScrollBar    _ScrollBarV = new VScrollBar();
    private HScrollBar    _ScrollBarH = new HScrollBar();
    private bool          _ScrollAlwaysVisible = false;
    private int           _MouseOverItem = -1;
    private int           _MouseOverColumn = -1;
    private int           _SelectedIndex = -1;
    private bool          _draggingColumn = false;
    private int           _selectedColumn = -1;
    private bool          _pushedColumn = false;    

    // cache max item width (since that's resource heavy, -1 means required to recalc)
    private int           _CachedMaxItemWidth = -1;

    private bool          _UpdateLocked = false;
    private bool          _RedrawRequired= false;
    private int           _HeaderHeight = 24;
    private bool          _HasHeader = true;

    private List<int>     _sortedItems = new List<int>();
    private int           _sortColumn = -1;
    private bool          _sortAscending = true;




    private ColumnCollection _Columns = null;



    public event EventHandler       SelectedIndexChanged;
    public event EventHandler       Scrolled;



    public enum HitTestResult
    {
      NOWHERE            = 0,
      ITEM               = 1,
      HEADER_COLUMN      = 2,
      HEADER_SIZE_LEFT   = 3,      // on size bar to the left of column
      HEADER_SIZE_RIGHT  = 4       // on size bar to the right of column
    };



    public ListControl()
    {
      _Columns          = new ColumnCollection( this );
      SelectedIndices   = new ListControlItemIndexCollection( this );
      SelectedItems     = new ListControlSelectedItemCollection( this );
      Items             = new ListControlItemCollection( this );

      BorderStyle       = BorderStyle.SUNKEN;

      

      Controls.Add( _ScrollBarV );
      Controls.Add( _ScrollBarH );
      _ScrollBarV.Scroll += _ScrollBar_Scroll;
      _ScrollBarH.Scroll += _ScrollBarH_Scroll;
      AdjustScrollBars();
    }



    public int SortColumn
    {
      get
      {
        return _sortColumn;
      }
      set
      {
        Sort( value, _sortAscending );
      }
    }



    public void Sort( int columnIndex, bool ascending = true )
    {
      if ( ( columnIndex < 0 )
      ||   ( columnIndex >= Columns.Count ) )
      {
        columnIndex = 0;
      }
      _sortColumn     = columnIndex;
      _sortAscending  = ascending;
      SortItems();
    }



    private void SortItems()
    {
      if ( ( _sortColumn < 0 )
      ||   ( _sortColumn >= Columns.Count ) )
      {
        return;
      }
      _sortedItems = Items.OrderBy( i => i.SubItems[_sortColumn].Text ).Select( i => i.Index ).ToList();
    }



    bool HitTest( Point position, out HitTestResult hitResult, out int item, out int subItem )
    {
      item      = -1;
      subItem   = -1;
      hitResult = HitTestResult.NOWHERE;

      int   x = 0;

      if ( ( _HasHeader )
      &&   ( position.Y < _HeaderHeight ) )
      {
        int   headerDragWidth = 5;

        x = 0;        
        for ( int i = 0; i < _Columns.Count; ++i )
        {
          var column = _Columns[i];

          if ( ( position.X + _DisplayOffsetX >= x )
          &&   ( position.X + _DisplayOffsetX < x + _Columns[i].Width ) )
          {
            subItem = i;
            if ( ( i > 0 )
            &&   ( position.X + _DisplayOffsetX < x + headerDragWidth )
            &&   ( _Columns[i - 1].Sizable ) )
            {
              hitResult = HitTestResult.HEADER_SIZE_LEFT;
            }
            else if ( ( position.X + _DisplayOffsetX >= x + _Columns[i].Width - headerDragWidth )
            &&        ( _Columns[i].Sizable ) )  
            {
              hitResult = HitTestResult.HEADER_SIZE_RIGHT;
            }
            else
            {
              hitResult = HitTestResult.HEADER_COLUMN;
            }
            break;
          }
          x += column.Width;
        }
        return hitResult != HitTestResult.NOWHERE;
      }

      item = position.Y / ItemHeight + FirstVisibleItemIndex;
      if ( HasHeader )
      {
        item = ( position.Y - HeaderHeight ) / ItemHeight + FirstVisibleItemIndex;
      }
      if ( item >= Items.Count )
      {
        item = -1;
      }

      x = 0;
      for ( int i = 0; i < _Columns.Count; ++i )
      {
        if ( ( position.X - _DisplayOffsetX >= x )
        &&   ( position.X - _DisplayOffsetX < x + _Columns[i].Width ) )
        {
          subItem = i;
          hitResult = HitTestResult.ITEM;
          break;
        }
        x += _Columns[i].Width;
      }
      return ( subItem != -1 );
    }



    public ColumnCollection Columns
    {
      get
      {
        return _Columns;
      }
    }



    public int HeaderHeight
    {
      get
      {
        if ( HasHeader )
        {
          return _HeaderHeight;
        }
        return 0;
      }
      set
      {
        _HeaderHeight = value;
        Invalidate();
      }
    }



    public bool HasHeader
    {
      get
      {
        return _HasHeader;
      }
      set
      {
        if ( _HasHeader != value )
        {
          _HasHeader = value;
          Invalidate();
        }
      }
    }



    public GR.Math.Rectangle GetHeaderRect( int ColumnIndex )
    {
      if ( ColumnIndex >= Columns.Count )
      {
        return GR.Math.Rectangle.Empty;
      }
      GR.Math.Rectangle rect;

      if ( HasHeader )
      {
        rect = new GR.Math.Rectangle( 0, 0, Columns[ColumnIndex].Width, HeaderHeight );
      }
      else
      {
        rect = new GR.Math.Rectangle( 0, 0, ClientRectangle.Width, 0 );
      }
      for ( int i = 0; i < ColumnIndex; ++i )
      {
        rect.Offset( Columns[i].Width, 0 );
      }
      rect.Offset( -_DisplayOffsetX, 0 );

      // -1 means use the rest of the list
      if ( Columns[ColumnIndex].Width == -1 )
      {
        rect.Width = ClientRectangle.Width - rect.Left;
      }
      return rect;
    }



    public bool GetItemRect( int iItem, int iColumn, out Rectangle rectItem )
    {
      rectItem = Rectangle.Empty;
      if ( ( iItem >= Items.Count )
      ||   ( iItem < FirstVisibleItemIndex ) )
      {
        return false;
      }
      if ( ( iColumn >= Columns.Count )
      &&   ( iColumn != -1 ) )
      {
        return false;
      }

      int     iX = 0;

      if ( iColumn != -1 )
      {
        int  iTempColumn = 0;

        while ( iTempColumn < iColumn )
        {
          iX += Columns[iTempColumn].Width;
          ++iTempColumn;
        }
        rectItem = new Rectangle( iX, (int)( iItem - FirstVisibleItemIndex ) * ItemHeight, Columns[iTempColumn].Width, ItemHeight );
      }
      else
      {
        rectItem.X = 0;
        rectItem.Y = (int)( iItem - FirstVisibleItemIndex ) * ItemHeight;
      }

      if ( rectItem.Bottom < 0 )
      {
        rectItem = new Rectangle();
        return false;
      }
      if ( rectItem.Top >= ClientRectangle.Height )
      {
        rectItem = new Rectangle();
        return false;
      }
      if ( rectItem.Top < 0 )
      {
        rectItem = new Rectangle(  rectItem.Left, 0, rectItem.Width, rectItem.Height + rectItem.Top );
      }
      rectItem.Offset( -_DisplayOffsetX, HeaderHeight );

      return true;
    }



    Rectangle GetItemRect( int iItem )
    {
      var rectItem = Rectangle.Empty;
      if ( ( iItem >= Items.Count )
      ||   ( iItem < FirstVisibleItemIndex ) )
      {
        return rectItem;
      }
      rectItem.X      = 0;
      rectItem.Y      = (int)( iItem - FirstVisibleItemIndex ) * ItemHeight;
      rectItem.Width  = ClientSize.Width;
      rectItem.Height = ItemHeight;

      if ( rectItem.Bottom < 0 )
      {
        return Rectangle.Empty;
      }
      if ( rectItem.Top >= ClientRectangle.Height )
      {
        return Rectangle.Empty;
      }
      if ( rectItem.Top < 0 )
      {
        rectItem = new Rectangle( rectItem.Left, 0, rectItem.Width, rectItem.Height + rectItem.Top );
      }
      rectItem.Offset( -_DisplayOffsetX, HeaderHeight );

      return rectItem;
    }



    private void _ScrollBar_Scroll( ControlBase Sender )
    {
      Scrolled?.Invoke( this );
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
    public ListControlItemCollection          Items { get; private set; }
    public ListControlItemIndexCollection     SelectedIndices { get; private set; }
    public ListControlSelectedItemCollection  SelectedItems { get; private set; }
    // used for sorting
    public List<int>                          ItemIndices { get; private set; } = new List<int>();
    public SelectionMode                      SelectionMode { get; set; }




    public bool ScrollAlwaysVisible 
    {
      get
      {
        return _ScrollAlwaysVisible;
      }
      set 
      {
        _ScrollAlwaysVisible = value;
        AdjustScrollBars();
        Invalidate(); 
      } 
    }



    public int FirstVisibleItemIndex
    {
      get
      {
        return _ScrollBarV.Value;
      }
      set
      {
        if ( value < 0 )
        {
          value = 0;
        }
        if ( value >= Items.Count )
        {
          value = Items.Count - 1;
          if ( value < 0 )
          {
            value = 0;
          }
        }
        if ( _ScrollBarV.Value != value )
        {
          _ScrollBarV.Value = value;
          Invalidate();
        }
      }
    }



    private void AdjustScrollBars()
    {
      bool    needVerticalScrollbar   = VerticalScrollbarRequired();
      bool    needHorizontalScrollbar = HorizontalScrollbarRequired();

      int     potentialVScrollWidth = needVerticalScrollbar ? _ScrollBarV.Width : 0;
      int     fullWidth = FullWidth();

      if ( fullWidth > ClientSize.Width - potentialVScrollWidth )
      {
        _ScrollBarH.Visible = true;
        _ScrollBarH.Bounds = new System.Drawing.Rectangle( 0, ClientSize.Height - _ScrollBarH.Height, ClientSize.Width - potentialVScrollWidth, _ScrollBarH.Height );

        _ScrollBarH.Maximum = fullWidth - ( ClientSize.Width - potentialVScrollWidth );

        float factor = ( ClientSize.Width - potentialVScrollWidth ) / (float)fullWidth;
        _ScrollBarH.SetSliderSize( (int)( ( _ScrollBarH.Width - 2 * 17 ) * factor ) );

      }
      else
      {
        _ScrollBarH.Visible = false;
        _DisplayOffsetX     = 0;
      }

      if ( needVerticalScrollbar )
      {
        _ScrollBarV.Visible  = true;

        int usableHeight = ClientSize.Height;
        if ( needHorizontalScrollbar )
        {
          usableHeight -= _ScrollBarH.Height;
        }
        _ScrollBarV.Bounds = new System.Drawing.Rectangle( ClientSize.Width - _ScrollBarV.Width, 0, _ScrollBarV.Width, usableHeight );

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
          _ScrollBarV.Maximum = 0;
          _ScrollBarV.SetSliderSize( _ScrollBarV.Height - 2 * 17 );
        }
        else
        {

          if ( _ScrollBarV.Value > newMax )
          {
            _ScrollBarV.Value = newMax;
          }
          _ScrollBarV.Maximum = newMax;

          float factor = usableHeight / ( (float)Items.Count * ItemHeight );
          _ScrollBarV.SetSliderSize( (int)( ( _ScrollBarV.Height - 2 * 17 ) * factor ) );
        }
      }
      else
      {
        if ( !_ScrollAlwaysVisible )
        {
          _ScrollBarV.Visible = false;
        }
        _ScrollBarV.Value = 0;
        _ScrollBarV.Maximum = 0;
      }

      _ActualWorkWidth = 0;
      _ActualWorkHeight = 0;
      if ( needVerticalScrollbar )
      {
        _ActualWorkWidth = ClientSize.Width - _ScrollBarV.Width;
      }
      if ( needHorizontalScrollbar )
      {
        _ActualWorkHeight = ClientSize.Height - _ScrollBarH.Height;
      }
    }



    private bool HorizontalScrollbarRequired()
    {
      int fullWidth = FullWidth();  
      if ( VerticalScrollbarRequired() )
      {
        return fullWidth > ClientSize.Width - _ScrollBarV.Width;
      }
      return fullWidth > ClientSize.Width;
    }



    protected override void OnSizeChanged( System.EventArgs e )
    {
      base.OnSizeChanged( e );

      AdjustScrollBars();
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
        int fullWidth = FullWidth();
        if ( !_ScrollBarV.Visible )
        {
          return Math.Max( fullWidth, ClientSize.Width );
        }
        return Math.Max( fullWidth, ClientSize.Width - _ScrollBarV.Width );
      }
    }



    public int MouseOverItem
    {
      get
      {
        return _MouseOverItem;
      }
    }



    public ListControlItem SelectedItem
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



    private int FullWidth()
    {
      return _Columns.Sum( c => c.Width );
    }



    protected override void OnControlEvent( ControlEvent Event )
    {
      switch ( Event.Type )
      {
        case ControlEvent.EventType.SET_CURSOR:
          {
            if ( HitTest( new Point( Event.MouseX, Event.MouseY ), out var hitTestResult, out var item, out var subItem ) )
            {
              if ( ( hitTestResult == HitTestResult.HEADER_SIZE_LEFT )
              ||   ( hitTestResult == HitTestResult.HEADER_SIZE_RIGHT ) )
              {
                SetCursor( CursorType.CURSOR_SIZE_WE );
                Event.Handled = true;
                return;
              }
            }
          }
          break;
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
          else if ( _ScrollBarV.Visible )
          {
            _ScrollBarV.RaiseControlEvent( Event );
          }
          break;
        case ControlEvent.EventType.MOUSE_UPDATE:
          {
            if ( HitTest( new Point( Event.MouseX, Event.MouseY ), out var hitTestResult, out var item, out var subItem ) )
            {
              if ( hitTestResult == HitTestResult.HEADER_COLUMN )
              {
                _MouseOverColumn = subItem;
              }
              else
              {
                _MouseOverColumn = -1;
              }
              if ( _draggingColumn )
              {
                if ( _selectedColumn != -1 )
                {
                  int   newWidth = Event.MouseX - GetHeaderRect( _selectedColumn ).Left + _DisplayOffsetX;
                  if ( newWidth < 5 )
                  {
                    newWidth = 5;
                  }
                  Columns[_selectedColumn].Width = newWidth;
                  AdjustScrollBars();
                  Invalidate();
                }
              }
              else
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
          {
            Focus();
            if ( HitTest( new Point( Event.MouseX, Event.MouseY ), out var hitTestResult, out var item, out var subItem ) )
            {
              if ( ( hitTestResult == HitTestResult.HEADER_SIZE_LEFT )
              ||   ( hitTestResult == HitTestResult.HEADER_SIZE_RIGHT ) )
              {
                Capture = true;
                _draggingColumn = true;
                _selectedColumn = subItem;
                if ( hitTestResult == HitTestResult.HEADER_SIZE_LEFT )
                {
                  --_selectedColumn;
                }
                return;
              }
              else if ( hitTestResult == HitTestResult.HEADER_COLUMN )
              {
                _selectedColumn = subItem;
                _pushedColumn   = true;
                return;
              }
              else  // item

              if ( hitTestResult == HitTestResult.ITEM )
              {
                if ( _MouseOverItem != SelectedIndex )
                {
                  SelectedIndex = _MouseOverItem;
                }
                Capture = true;
              }
            }
          }
          break;
        case ControlEvent.EventType.MOUSE_UP:
          {
            Capture = false;
            if ( HitTest( new Point( Event.MouseX, Event.MouseY ), out var hitTestResult, out var item, out var subItem ) )
            {
              if ( _pushedColumn )
              {
                _pushedColumn = false;
                if ( ( hitTestResult == HitTestResult.HEADER_COLUMN )
                &&   ( _selectedColumn == _MouseOverColumn ) )
                {
                  OnColumnClicked( _selectedColumn );
                }
                _selectedColumn = -1;
                Invalidate();
                return;
              }
              if ( _draggingColumn )
              {
                _draggingColumn = false;
                _selectedColumn = -1;
                AdjustScrollBars();
                Invalidate();
                return;
              }
              Invalidate();
            }
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



    private void OnColumnClicked( int selectedColumn )
    {
      // TODO sort
      Debug.Log( $"Sort by column {selectedColumn} clicked" );
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
      if ( Y < HeaderHeight )
      {
        return -1;
      }
      int   itemIndex = FirstVisibleItemIndex + ( Y - HeaderHeight ) / ItemHeight;
      if ( itemIndex >= Items.Count )
      {
        return -1;
      }
      return itemIndex;
    }



    protected override void OnPaint( ControlRenderer Renderer )
    {
      Renderer.RenderListControl();
    }



    private int IndexOfItem( ListControlItem Item )
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



    internal void SelectItem( ListControlItem Item )
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



    internal void UnselectItem( ListControlItem Item )
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



    internal void ItemModified( int itemIndex )
    {
      _CachedMaxItemWidth = -1;
      Invalidate( GetItemRect( itemIndex ) );
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



    internal void ItemsModified()
    {
      FixItemIndices();
      _CachedMaxItemWidth = -1;
      SortItems();
      AdjustScrollBars();
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



}