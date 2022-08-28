using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;



namespace GR.Forms
{
  public class ImageListbox : CustomAutoScrollPanel.ScrollablePanel
  {
    public class ImageListItem
    {
      ImageListbox          m_Container = null;

      public ImageListItem( ImageListbox Container )
      {
        m_Container = Container;
      }


      public object Value
      {
        get;
        set;
      }

      public System.Drawing.Image Image
      {
        get;
        set;
      }

      public GR.Image.MemoryImage MemoryImage
      {
        get;
        set;
      }
    }

    public class ObjectCollection : List<ImageListItem>
    {

      private ImageListbox    Container = null;


      public ObjectCollection( ImageListbox Container )
      {
        this.Container = Container;
      }


      public new int Add( ImageListItem Item )
      {
        base.Add( Item );
        Container.ItemsModified();
        return base.Count - 1;
      }



      public int Add( object Object )
      {
        ImageListItem item = new ImageListItem( Container );
        item.Value = Object;
        base.Add( item );
        Container.ItemsModified();
        return base.Count - 1;
      }



      public int Add( object Object, System.Drawing.Image Image )
      {
        ImageListItem item = new ImageListItem( Container );
        item.Value = Object;
        item.Image = Image;
        base.Add( item );
        Container.ItemsModified();
        return base.Count - 1;
      }



      public int Add( object Object, GR.Image.MemoryImage Image )
      {
        ImageListItem item = new ImageListItem( Container );
        item.Value = Object;
        item.MemoryImage = Image;
        base.Add( item );
        Container.ItemsModified();
        return base.Count - 1;
      }



      public new void AddRange( IEnumerable<ImageListItem> Items )
      {
        InsertRange( 0, Items );
        Container.AdjustScrollbars();
      }


      public new void RemoveRange( int Index, int Count )
      {
        base.RemoveRange( Index, Count );
        Container.AdjustScrollbars();
      }
    }



    public ObjectCollection       Items = null;
    private int                   m_ItemWidth = -1;
    private int                   m_ItemHeight = 13;
    private int                   m_ItemsPerLine = 0;
    private int                   m_Offset = 0;
    private int                   m_ItemUnderMouse = -1;
    private int                   m_SelectedItem = -1;
    private int                   m_SelectionAnchor = -1;
    private bool                  m_SelectionIsRange = true;
    private GR.Image.FastImage    m_DisplayPage = new GR.Image.FastImage();
    private GR.Drawing.PixelFormat  m_PixelFormat = GR.Drawing.PixelFormat.Undefined;
    private int                   m_DisplayWidth = -1;
    private int                   m_DisplayHeight = -1;
    private bool                  m_UpdateLockActive = false;


    public event System.Windows.Forms.DrawItemEventHandler    DrawItem;
    public event EventHandler                                 SelectedIndexChanged;
    public event EventHandler                                 SelectedAnchorChanged;
    public event EventHandler                                 SelectedRangeChanged;
    public event EventHandler                                 SelectionTypeChanged;
    public event EventHandler                                 SelectionChanged;



    public UInt32 HottrackColor
    {
      get;
      set;
    }



    public GR.Image.FastImage     DisplayPage
    {
      get
      {
        return m_DisplayPage;
      }
    }



    public int ItemsPerLine
    {
      get
      {
        return m_ItemsPerLine;
      }
    }



    public GR.Drawing.PixelFormat PixelFormat
    {
      get
      {
        return m_PixelFormat;
      }
      set
      {
        m_PixelFormat = value;
        if ( m_DisplayWidth == -1 )
        {
          m_DisplayPage.Create( ClientRectangle.Width, ClientRectangle.Height, m_PixelFormat );
        }
        else
        {
          m_DisplayPage.Create( m_DisplayWidth, m_DisplayHeight, m_PixelFormat );
        }
      }
    }

    
    
    public int ItemWidth
    {
      get
      {
        return m_ItemWidth;
      }
      set
      {
        m_ItemWidth = value;

        int   actualWidth = ClientRectangle.Width;
        if ( m_DisplayWidth != -1 )
        {
          actualWidth = m_DisplayWidth;
        }
        if ( VisibleAutoScrollVertical )
        {
          actualWidth -= System.Windows.Forms.SystemInformation.VerticalScrollBarWidth;
        }

        if ( m_ItemWidth == -1 )
        {
          m_ItemWidth = ClientRectangle.Width;
        }
        m_ItemsPerLine = actualWidth / m_ItemWidth;
        if ( m_ItemsPerLine == 0 )
        {
          m_ItemsPerLine = 1;
        }
        AdjustScrollbars();
      }
    }



    public int ItemHeight
    {
      get
      {
        return m_ItemHeight;
      }
      set
      {
        m_ItemHeight = value;
        AdjustScrollbars();
      }
    }



    public bool IsSelectionColumnBased
    {
      get
      {
        if ( ( m_SelectedItem == -1 )
        ||   ( m_SelectionAnchor == -1 ) )
        {
          return false;
        }
        return !m_SelectionIsRange;
      }
    }



    public int SelectedIndex
    {
      get
      {
        return m_SelectedItem;
      }
      set
      {
        SetSelection( value, false, -1, false );
      }
    }



    public List<int> SelectedIndices
    {
      get
      {
        List<int> selectedIndices = new List<int>();

        if ( m_SelectedItem == -1 )
        {
          return selectedIndices;
        }
        if ( m_SelectionAnchor == -1 )
        {
          selectedIndices.Add( m_SelectedItem );
        }
        else if ( m_SelectionIsRange )
        {
          int i1 = m_SelectedItem;
          int i2 = m_SelectionAnchor;
          if ( i2 < i1 )
          {
            i1 = i2;
            i2 = m_SelectedItem;
          }
          for ( int i = i1; i <= i2; ++i )
          {
            selectedIndices.Add( i );
          }
        }
        else
        {
          // column selection
          int i1 = m_SelectionAnchor;
          int i2 = m_SelectedItem;
          if ( i2 < i1 )
          {
            i1 = m_SelectedItem;
            i2 = m_SelectionAnchor;
          }

          int x1 = i1 % m_ItemsPerLine;
          int x2 = i2 % m_ItemsPerLine;
          if ( x2 < x1 )
          {
            x1 = x2;
            x2 = i1 % m_ItemsPerLine;
          }

          int y1 = m_SelectionAnchor / m_ItemsPerLine;
          int y2 = m_SelectedItem / m_ItemsPerLine;
          if ( y2 < y1 )
          {
            y1 = y2;
            y2 = m_SelectionAnchor / m_ItemsPerLine;
          }
          i1 = y1 * m_ItemsPerLine + x1;
          i2 = y2 * m_ItemsPerLine + x2;

          for ( int i = i1; i <= i2; ++i )
          {
            if ( ( i % m_ItemsPerLine >= x1 )
            &&   ( i % m_ItemsPerLine <= x2 ) )
            {
              selectedIndices.Add( i );
            }
          }
        }
        return selectedIndices;
      }
    }



    public new event KeyEventHandler KeyDown;
    public event KeyPressEventHandler KeyPressed;
    public new event KeyEventHandler KeyUp;



    public ImageListbox()
    {
      Items = new ObjectCollection( this );

      HottrackColor = 0x804040ff;

      // Set the value of the double-buffering style bits to true.
      SetStyle( ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.Selectable, true );
      UpdateStyles();
    }



    protected override void OnKeyDown( KeyEventArgs e )
    {
      if ( KeyDown != null )
      {
        KeyDown( this, e );
      }
    }



    protected override void OnKeyUp( KeyEventArgs e )
    {
      if ( KeyUp != null )
      {
        KeyUp( this, e );
      }
    }



    protected override void OnKeyPress( KeyPressEventArgs e )
    {
      if ( KeyPressed != null )
      {
        KeyPressed( this, e );
      }
    }



    public void SetDisplaySize( int Width, int Height )
    {
      m_DisplayWidth  = Width;
      m_DisplayHeight = Height;

      m_DisplayPage.Create( m_DisplayWidth, m_DisplayHeight, m_PixelFormat );

      if ( m_ItemWidth == -1 )
      {
        m_ItemWidth = ClientRectangle.Width;
      }
      int   itemWidth = m_ItemWidth;
      if ( m_DisplayPage.Width != ClientRectangle.Width )
      {
        itemWidth *= ( ClientRectangle.Width / m_DisplayPage.Width );
      }
      m_ItemsPerLine = ClientRectangle.Width / itemWidth;
      if ( m_ItemsPerLine == 0 )
      {
        m_ItemsPerLine = 1;
      }

      AdjustScrollbars();
    }



    protected override void OnSizeChanged( EventArgs e )
    {
      base.OnSizeChanged( e );

      if ( m_DisplayWidth == -1 )
      {
        m_DisplayPage.Create( ClientRectangle.Width, ClientRectangle.Height, m_PixelFormat );
      }
      if ( m_ItemWidth == -1 )
      {
        m_ItemWidth = ClientRectangle.Width;
      }
      if ( m_ItemWidth <= 0 )
      {
        m_ItemWidth = 20;
      }
      int   itemWidth = m_ItemWidth;
      if ( m_DisplayPage.Width != ClientRectangle.Width )
      {
        itemWidth = ( itemWidth * ClientRectangle.Width / m_DisplayPage.Width );
      }
      if ( itemWidth <= 0 )
      {
        itemWidth = 20;
      }

      int   actualWidth = ClientRectangle.Width;
      if ( m_DisplayWidth != -1 )
      {
        actualWidth = m_DisplayWidth;
      }
      if ( VisibleAutoScrollVertical )
      {
        actualWidth -= System.Windows.Forms.SystemInformation.VerticalScrollBarWidth;
      }

      if ( m_ItemWidth == -1 )
      {
        m_ItemWidth = ClientRectangle.Width;
      }
      m_ItemsPerLine = actualWidth / m_ItemWidth;

      //m_ItemsPerLine = ClientRectangle.Width / itemWidth;
      if ( m_ItemsPerLine == 0 )
      {
        m_ItemsPerLine = 1;
      }

      while ( m_Offset * m_ItemsPerLine >= Items.Count )
      {
        m_Offset -= m_ItemsPerLine;
      }
      if ( m_Offset < 0 )
      {
        m_Offset = 0;
      }

      AdjustScrollbars();
      Invalidate();
    }



    private bool _InsideAdjustScrollbars = false;

    private void AdjustScrollbars()
    {
      if ( _InsideAdjustScrollbars )
      {
        return;
      }

      _InsideAdjustScrollbars = true;
      if ( ( m_ItemHeight == 0 )
      ||   ( m_DisplayHeight == -1 ) )
      {
        VisibleAutoScrollVertical = false;
        AutoScrollVPos = 0;

        _InsideAdjustScrollbars = false;
        return;
      }
      int   actualWidth = ClientRectangle.Width;
      if ( m_DisplayWidth != -1 )
      {
        actualWidth = m_DisplayWidth;
      }
      int visibleItems = m_DisplayHeight / m_ItemHeight;

      if ( VisibleAutoScrollVertical )
      {
        // TODO - verify if we could fit all items without the scrollbar
        actualWidth += System.Windows.Forms.SystemInformation.VerticalScrollBarWidth;
        int   potentialItemsPerLine = actualWidth / m_ItemWidth;
        if ( potentialItemsPerLine <= 0 )
        {
          potentialItemsPerLine = 1;
        }
        int scrollLength = ( Items.Count / potentialItemsPerLine ) - visibleItems;
        if ( scrollLength <= 0 )
        {
          m_ItemsPerLine = potentialItemsPerLine;
          VisibleAutoScrollVertical = false;
          m_Offset = 0;
          AutoScrollVPos = 0;
          _InsideAdjustScrollbars = false;
          return;
        }
        actualWidth -= System.Windows.Forms.SystemInformation.VerticalScrollBarWidth;
      }
      m_ItemsPerLine = actualWidth / m_ItemWidth;
      if ( m_ItemsPerLine <= 0 )
      {
        m_ItemsPerLine = 1;
      }
      int scrollLength2 = ( Items.Count / m_ItemsPerLine ) - visibleItems;

      if ( ( Items.Count % m_ItemsPerLine ) != 0 )
      {
        ++scrollLength2;
      }
      if ( scrollLength2 <= 0 )
      {
        VisibleAutoScrollVertical = false;
        m_Offset = 0;
        AutoScrollVPos = 0;
      }
      else
      {
        AutoScrollVerticalMaximum = scrollLength2;
        if ( AutoScrollVPos > AutoScrollVerticalMaximum )
        {
          AutoScrollVerticalMaximum = 0;
        }
        VisibleAutoScrollVertical = true;
      }
      _InsideAdjustScrollbars = false;
    }



    private void ItemsModified()
    {
      if ( m_UpdateLockActive )
      {
        return;
      }
      AdjustScrollbars();
      Invalidate();
    }



    public int ItemAtLocation( int X, int Y )
    {
      float     itemWidth = (float)m_ItemWidth;
      float     itemHeight = (float)m_ItemHeight;

      if ( m_DisplayPage.Width != ClientRectangle.Width )
      {
        itemWidth *= ( (float)ClientRectangle.Width / m_DisplayPage.Width );
        itemHeight *= ( (float)ClientRectangle.Height / m_DisplayPage.Height );
      }
      int     numItemsX = m_DisplayPage.Width / m_ItemWidth;
      int     numItemsY = m_DisplayPage.Height / m_ItemHeight;

      int     itemX = -1;
      for ( int i = 0; i < numItemsX; ++i )
      {
        if ( ( X >= i * itemWidth )
        &&   ( X < ( i + 1 ) * itemWidth ) )
        {
          itemX = i;
          break;
        }
      }
      int     itemY = -1;
      for ( int i = 0; i < numItemsY; ++i )
      {
        if ( ( Y >= i * itemHeight )
        &&   ( Y < ( i + 1 ) * itemHeight ) )
        {
          itemY = i;
          break;
        }
      }

      if ( ( itemX >= m_ItemsPerLine )
      ||   ( itemX < 0 ) )
      {
        return -1;
      }
      if ( ( itemY < 0 )
      ||   ( itemY * m_ItemsPerLine + AutoScrollVPos * m_ItemsPerLine >= Items.Count ) )
      {
        return -1;
      }
      int     newIndex = ( m_Offset + itemY ) * m_ItemsPerLine + itemX;
      if ( newIndex >= Items.Count )
      {
        return -1;
      }
      return newIndex;
    }



    protected void InvalidateSelectedItems()
    {
      if ( ( m_SelectionIsRange )
      &&   ( m_SelectionAnchor != -1 ) )
      {
        int i1 = m_SelectedItem;
        int i2 = m_SelectionAnchor;
        if ( i2 < i1 )
        {
          i1 = m_SelectionAnchor;
          i2 = m_SelectedItem;
        }
        for ( int i = i1; i <= i2; ++i )
        {
          InvalidateItemRect( i );
        }
      }
      else if ( ( !m_SelectionIsRange )
      &&        ( m_SelectionAnchor != -1 ) )
      {
        int i1 = m_SelectionAnchor;
        int i2 = m_SelectedItem;
        if ( i2 < i1 )
        {
          i1 = m_SelectedItem;
          i2 = m_SelectionAnchor;
        }

        int x1 = i1 % m_ItemsPerLine;
        int x2 = i2 % m_ItemsPerLine;
        if ( x2 < x1 )
        {
          x1 = x2;
          x2 = i1 % m_ItemsPerLine;
        }

        int y1 = m_SelectionAnchor / m_ItemsPerLine;
        int y2 = m_SelectedItem / m_ItemsPerLine;
        if ( y2 < y1 )
        {
          y1 = y2;
          y2 = m_SelectionAnchor / m_ItemsPerLine;
        }
        i1 = y1 * m_ItemsPerLine + x1;
        i2 = y2 * m_ItemsPerLine + x2;

        for ( int i = i1; i <= i2; ++i )
        {
          if ( ( i % m_ItemsPerLine >= x1 )
          && ( i % m_ItemsPerLine <= x2 ) )
          {
            InvalidateItemRect( i );
          }
        }
      }
      else if ( m_SelectedItem != -1 )
      {
        InvalidateItemRect( m_SelectedItem );
      }
    }



    protected void SetSelection( int SelectedIndex, bool SelectionIsRange, int AnchorIndex, bool ToggleSelectionState )
    {
      bool  selectedIndexChanged = ( m_SelectedItem != SelectedIndex );
      bool  selectionIsRangeChanged = ( m_SelectionIsRange != SelectionIsRange );
      bool  selectionAnchorChanged = ( m_SelectionAnchor != AnchorIndex );

      int     oldItem = m_SelectedItem;

      if ( ( selectedIndexChanged )
      ||   ( selectionIsRangeChanged )
      ||   ( selectionAnchorChanged ) )
      {
        InvalidateSelectedItems();
      }

      m_SelectionIsRange = SelectionIsRange;
      m_SelectionAnchor = AnchorIndex;
      m_SelectedItem = SelectedIndex;

      if ( ( selectedIndexChanged )
      ||   ( selectionIsRangeChanged )
      ||   ( selectionAnchorChanged ) )
      {
        InvalidateSelectedItems();
        if ( SelectionChanged != null )
        {
          SelectionChanged( this, new EventArgs() );
        }
      }

      if ( ( SelectionIsRange )
      ||   ( selectionAnchorChanged ) )
      {
        if ( SelectionTypeChanged != null )
        {
          SelectionTypeChanged( this, new EventArgs() );
        }
      }


      if ( ( selectedIndexChanged )
      &&   ( SelectedIndexChanged != null ) )
      {
        SelectedIndexChanged( this, new EventArgs() );
      }
      if ( ( selectionAnchorChanged )
      &&   ( SelectedAnchorChanged != null ) )
      {
        SelectedAnchorChanged( this, new EventArgs() );
      }
      if ( ( selectionIsRangeChanged )
      &&   ( SelectedRangeChanged != null ) )
      {
        SelectedRangeChanged( this, new EventArgs() );
      }

      if ( m_SelectedItem != -1 )
      {
        while ( m_SelectedItem < AutoScrollVPos * m_ItemsPerLine )
        {
          OnScroll( new ScrollEventArgs( ScrollEventType.SmallDecrement, AutoScrollVPos, AutoScrollVPos - 1, ScrollOrientation.VerticalScroll ) );
        }
        int visibleItems = m_DisplayHeight / m_ItemHeight;

        while ( ( m_SelectedItem >= ( visibleItems + AutoScrollVPos ) * m_ItemsPerLine )
        &&      ( AutoScrollVPos < AutoScrollVerticalMaximum ) )
        {
          OnScroll( new ScrollEventArgs( ScrollEventType.SmallIncrement, AutoScrollVPos, AutoScrollVPos + 1, ScrollOrientation.VerticalScroll ) );
        }
      }
    }



    protected override void OnMouseLeave( EventArgs e )
    {
      if ( m_ItemUnderMouse != -1 )
      {
        m_ItemUnderMouse = -1;
        Invalidate();
      }

      base.OnMouseLeave( e );
    }



    protected override void OnMouseDown( MouseEventArgs e )
    {
      base.OnMouseDown( e );
      int oldIndex = SelectedIndex;
      int newIndex = ItemAtLocation( e.X, e.Y );

      bool toggleSelection = ( ( System.Windows.Forms.Control.ModifierKeys & Keys.Control ) != 0 );

      if ( ( System.Windows.Forms.Control.ModifierKeys & Keys.Shift ) != 0 )
      {
        // range-selection
        SetSelection( newIndex, true, oldIndex, toggleSelection );
      }
      else if ( ( System.Windows.Forms.Control.ModifierKeys & Keys.Alt ) != 0 )
      {
        if ( m_SelectionAnchor == -1 )
        {
          SetSelection( newIndex, false, oldIndex, toggleSelection );
        }
        else
        {
          SetSelection( newIndex, false, m_SelectionAnchor, toggleSelection );
        }
      }
      else
      {
        // single item selected
        SetSelection( newIndex, false, -1, toggleSelection );
      }
      Focus();
    }



    protected override void OnMouseMove( MouseEventArgs e )
    {
      int value = ItemAtLocation( e.X, e.Y );
      if ( m_ItemUnderMouse != value )
      {
        InvalidateItemRect( m_ItemUnderMouse );
        InvalidateItemRect( value );
        m_ItemUnderMouse = value;
      }
      base.OnMouseMove( e );
    }



    public System.Drawing.Rectangle ItemRect( int ItemIndex )
    {
      System.Drawing.Rectangle  trueRect = InternalItemRect( ItemIndex );

      //if ( ClientRectangle.Width != m_DisplayPage.Width )
      {
        float     factorX = (float)ClientRectangle.Width / m_DisplayPage.Width;
        float     factorY = (float)ClientRectangle.Height / m_DisplayPage.Height;

        trueRect.X = (int)( trueRect.X * factorX );
        trueRect.Y = (int)( trueRect.Y * factorY );
        trueRect.Width = (int)( trueRect.Width * factorX );
        trueRect.Height = (int)( trueRect.Height * factorY );
      }

      return trueRect;
    }



    private System.Drawing.Rectangle InternalItemRect( int ItemIndex )
    {
      if ( ( ItemIndex < 0 )
      ||   ( ItemIndex >= Items.Count )
      ||   ( ItemIndex < m_Offset * m_ItemsPerLine )
      ||   ( ItemIndex >= m_Offset * m_ItemsPerLine + ( ClientRectangle.Height / m_ItemHeight ) * m_ItemsPerLine ) )
      {
        return new System.Drawing.Rectangle();
      }
      int xoffset = ( ItemIndex - m_Offset * m_ItemsPerLine ) % m_ItemsPerLine;
      int yoffset = ( ItemIndex - m_Offset * m_ItemsPerLine ) / m_ItemsPerLine;
      return new System.Drawing.Rectangle( xoffset * m_ItemWidth, yoffset * m_ItemHeight, m_ItemWidth, m_ItemHeight );
    }



    protected override void OnPaint( PaintEventArgs e )
    {
      if ( m_DisplayPage.BitsPerPixel == 0 )
      {
        // not initialised yet
        e.Graphics.FillRectangle( System.Drawing.SystemBrushes.Control, e.ClipRectangle );
        return;
      }

      System.Drawing.SolidBrush hottrackBrush = new System.Drawing.SolidBrush( GR.Color.Helper.FromARGB( HottrackColor ) );

      int     itemIndex = m_Offset * m_ItemsPerLine;
      int     itemInLine = 0;
      bool    hasNativeImages = false;
      System.Drawing.Rectangle itemRect = new System.Drawing.Rectangle( 0, 0, ClientSize.Width, 1 );
      while ( itemIndex < Items.Count )
      {
        if ( itemIndex < 0 )
        {
          ++itemIndex;
          itemInLine = ( ( itemInLine + 1 ) % m_ItemsPerLine );

          continue;
        }
        int     xoffset = ( itemIndex - m_Offset * m_ItemsPerLine ) % m_ItemsPerLine;
        int     yoffset = ( itemIndex - m_Offset * m_ItemsPerLine ) / m_ItemsPerLine;
        itemRect = new System.Drawing.Rectangle( xoffset * m_ItemWidth, yoffset * m_ItemHeight, m_ItemWidth, m_ItemHeight );

        if ( DrawItem != null )
        {
          e.Graphics.FillRectangle( System.Drawing.SystemBrushes.Control, e.ClipRectangle );
          DrawItemEventArgs args = new DrawItemEventArgs( e.Graphics, Font, itemRect, itemIndex, DrawItemState.Default );
          DrawItem( this, args );
        }
        else if ( Items[itemIndex].Image != null )
        {
          e.Graphics.FillRectangle( System.Drawing.SystemBrushes.Control, itemRect );// e.ClipRectangle );
          e.Graphics.DrawImage( Items[itemIndex].Image, itemRect );
          hasNativeImages = true;
        }
        else if ( Items[itemIndex].MemoryImage != null )
        {
          m_DisplayPage.DrawImage( Items[itemIndex].MemoryImage, itemRect.X, itemRect.Y );
        }
        ++itemIndex;
        itemInLine = ( ( itemInLine + 1 ) % m_ItemsPerLine );
      }
      if ( itemInLine < m_ItemsPerLine )
      {
        // clear rest of display
        m_DisplayPage.Box( itemRect.Right, itemRect.Y, ClientSize.Width - itemRect.Right, m_ItemHeight, 0 );
      }
      m_DisplayPage.Box( 0, itemRect.Bottom, ClientSize.Width, ClientSize.Height - itemRect.Bottom, 0 );

      if ( !hasNativeImages )
      {
        IntPtr hdcPage = e.Graphics.GetHdc();
        m_DisplayPage.DrawToHDC( hdcPage, ClientRectangle );
        e.Graphics.ReleaseHdc( hdcPage );
      }

      if ( m_ItemUnderMouse != -1 )
      {
        e.Graphics.FillRectangle( hottrackBrush, ItemRect( m_ItemUnderMouse ) );
      }
      if ( m_SelectedItem != -1 )
      {
        if ( m_SelectionAnchor != -1 )
        {
          if ( m_SelectionIsRange )
          {
            int firstItem = m_SelectionAnchor;
            int lastItem = m_SelectedItem;
            if ( lastItem < firstItem )
            {
              lastItem = firstItem;
              firstItem = m_SelectedItem;
            }
            for ( int i = firstItem; i <= lastItem; ++i )
            {
              e.Graphics.FillRectangle( hottrackBrush, ItemRect( i ) );
            }
          }
          else
          {
            int i1 = m_SelectionAnchor;
            int i2 = m_SelectedItem;
            if ( i2 < i1 )
            {
              i1 = m_SelectedItem;
              i2 = m_SelectionAnchor;
            }

            int x1 = i1 % m_ItemsPerLine;
            int x2 = i2 % m_ItemsPerLine;
            if ( x2 < x1 )
            {
              x1 = x2;
              x2 = i1 % m_ItemsPerLine;
            }

            int y1 = m_SelectionAnchor / m_ItemsPerLine;
            int y2 = m_SelectedItem / m_ItemsPerLine;
            if ( y2 < y1 )
            {
              y1 = y2;
              y2 = m_SelectionAnchor / m_ItemsPerLine;
            }
            i1 = y1 * m_ItemsPerLine + x1;
            i2 = y2 * m_ItemsPerLine + x2;

            for ( int i = i1; i <= i2; ++i )
            {
              if ( ( i % m_ItemsPerLine >= x1 )
              &&   ( i % m_ItemsPerLine <= x2 ) )
              {
                e.Graphics.FillRectangle( hottrackBrush, ItemRect( i ) );
              }
            }
          }
        }
        else
        {
          e.Graphics.FillRectangle( hottrackBrush, ItemRect( m_SelectedItem ) );
        }
      }
      hottrackBrush.Dispose();
    }



    protected override void OnScroll( ScrollEventArgs se )
    {
      base.OnScroll( se );


      if ( se.ScrollOrientation == ScrollOrientation.VerticalScroll )
      {
        int newValue = m_Offset;
        int visibleItems = m_DisplayHeight / m_ItemHeight;
        int maxValue = ( Items.Count / m_ItemsPerLine ) - visibleItems;
        if ( ( Items.Count % m_ItemsPerLine ) != 0 )
        {
          ++maxValue;
        }

        switch ( se.Type )
        {
          case ScrollEventType.First:newValue = 0;
            break;
          case ScrollEventType.Last:
            newValue = maxValue;
            break;
          case ScrollEventType.ThumbPosition:
          case ScrollEventType.ThumbTrack:
            newValue = se.NewValue;
            break;
          case ScrollEventType.SmallDecrement:
            newValue--;
            break;
          case ScrollEventType.SmallIncrement:
            newValue++;
            break;
          case ScrollEventType.LargeDecrement:
            newValue -= ( ClientRectangle.Height / m_ItemHeight );
            break;
          case ScrollEventType.LargeIncrement:
            newValue += ( ClientRectangle.Height / m_ItemHeight );
            break;
          case ScrollEventType.EndScroll:
            newValue = se.NewValue;
            break;
        }
        if ( newValue < 0 )
        {
          newValue = 0;
        }
        if ( newValue > maxValue )
        {
          newValue = maxValue;
        }
        if ( m_Offset != newValue )
        {
          se.NewValue = newValue;
          m_Offset = newValue;
          AutoScrollVPos = newValue;
          Invalidate();
        }
      }
    }



    public void InvalidateItemRect( int ItemIndex )
    {
      if ( ItemIndex == -1 )
      {
        return;
      }
      Invalidate( ItemRect( ItemIndex ) );
    }



    protected override bool ProcessCmdKey( ref Message msg, Keys keyData )
    {
      if ( keyData == ( Keys.Up | Keys.Shift ) )
      {
        if ( ( SelectedIndex == -1 )
        &&   ( Items.Count > 0 ) )
        {
          SetSelection( 0, false, 0, false );
        }
        if ( SelectedIndex >= m_ItemsPerLine )
        {
          if ( m_SelectionAnchor != -1 )
          {
            SetSelection( SelectedIndex - m_ItemsPerLine, true, m_SelectionAnchor, false );
          }
          else
          {
            SetSelection( SelectedIndex - m_ItemsPerLine, true, SelectedIndex, false );
          }
        }
        return true;
      }
      else if ( keyData == Keys.Up )
      {
        if ( ( SelectedIndex == -1 )
        &&   ( Items.Count > 0 ) )
        {
          SetSelection( 0, false, -1, false );
        }
        if ( SelectedIndex >= m_ItemsPerLine )
        {
          SetSelection( SelectedIndex - m_ItemsPerLine, false, -1, false );
        }
        return true;
      }
      else if ( keyData == ( Keys.Left | Keys.Shift ) )
      {
        if ( ( SelectedIndex == -1 )
        &&   ( Items.Count > 0 ) )
        {
          SetSelection( 0, false, 0, false );
        }
        if ( SelectedIndex > 0 )
        {
          if ( m_SelectionAnchor != -1 )
          {
            SetSelection( SelectedIndex - 1, true, m_SelectionAnchor, false );
          }
          else
          {
            SetSelection( SelectedIndex - 1, true, SelectedIndex, false );
          }
        }
        return true;
      }
      else if ( keyData == Keys.Left )
      {
        if ( ( SelectedIndex == -1 )
        &&   ( Items.Count > 0 ) )
        {
          SetSelection( 0, false, -1, false );
        }
        if ( SelectedIndex > 0 )
        {
          SetSelection( SelectedIndex - 1, false, -1, false );
        }
        return true;
      }
      else if ( keyData == ( Keys.Right | Keys.Shift ) )
      {
        if ( ( SelectedIndex == -1 )
        &&   ( Items.Count > 0 ) )
        {
          SetSelection( Items.Count - 1, true, -1, false );
          return true;
        }
        if ( SelectedIndex + 1 < Items.Count )
        {
          if ( m_SelectionAnchor != -1 )
          {
            SetSelection( SelectedIndex + 1, true, m_SelectionAnchor, false );
          }
          else
          {
            SetSelection( SelectedIndex + 1, true, SelectedIndex, false );
          }
        }
        return true;
      }
      else if ( keyData == Keys.Right )
      {
        if ( ( SelectedIndex == -1 )
        &&   ( Items.Count > 0 ) )
        {
          SetSelection( Items.Count - 1, false, -1, false );
        }
        if ( SelectedIndex + 1 < Items.Count )
        {
          SetSelection( SelectedIndex + 1, false, -1, false );
        }
        return true;
      }
      else if ( keyData == ( Keys.Down | Keys.Shift ) )
      {
        if ( ( SelectedIndex == -1 )
        &&   ( Items.Count > 0 ) )
        {
          SetSelection( Items.Count - 1, false, -1, false );
        }
        if ( SelectedIndex + m_ItemsPerLine < Items.Count )
        {
          if ( m_SelectionAnchor == -1 )
          {
            SetSelection( SelectedIndex + m_ItemsPerLine, true, SelectedIndex, false );
          }
          else
          {
            SetSelection( SelectedIndex + m_ItemsPerLine, true, m_SelectionAnchor, false );
          }
        }
        return true;
      }
      else if ( keyData == Keys.Down )
      {
        if ( ( SelectedIndex == -1 )
        &&   ( Items.Count > 0 ) )
        {
          SetSelection( Items.Count - 1, false, -1, false );
        }
        if ( SelectedIndex + m_ItemsPerLine < Items.Count )
        {
          SetSelection( SelectedIndex + m_ItemsPerLine, false, -1, false );
        }
        return true;
      }
      return base.ProcessCmdKey( ref msg, keyData );
    }



    public void BeginUpdate()
    {
      m_UpdateLockActive = true;
    }



    public void EndUpdate()
    {
      if ( m_UpdateLockActive )
      {
        m_UpdateLockActive = false;
        ItemsModified();
      }
    }



  }
}
