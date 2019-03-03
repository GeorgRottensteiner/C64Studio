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
    private System.Drawing.Imaging.PixelFormat    m_PixelFormat = System.Drawing.Imaging.PixelFormat.Undefined;
    private int                   m_DisplayWidth = -1;
    private int                   m_DisplayHeight = -1;


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



    public System.Drawing.Imaging.PixelFormat PixelFormat
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
        if ( m_ItemWidth == -1 )
        {
          m_ItemWidth = ClientRectangle.Width;
        }
        if ( m_DisplayWidth != -1 )
        {
          m_ItemsPerLine = m_DisplayWidth / m_ItemWidth;
        }
        else
        {
          m_ItemsPerLine = ClientRectangle.Width / m_ItemWidth;
        }
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
        SetSelection( value, false, -1 );
      }
    }



    /*
    private void SelectIndex( int Index )
    {
      InvalidateItemRect( m_SelectedItem );
      m_SelectedItem = Index;
      InvalidateItemRect( Index );
      if ( SelectedIndexChanged != null )
      {
        SelectedIndexChanged( this, new EventArgs() );
      }
    }*/



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



    private void AdjustScrollbars()
    {
      int visibleItems = m_DisplayHeight / m_ItemHeight;
      int scrollLength = ( Items.Count / m_ItemsPerLine ) - visibleItems;

      if ( ( Items.Count % m_ItemsPerLine ) != 0 )
      {
        ++scrollLength;
      }
      if ( scrollLength <= 0 )
      {
        VisibleAutoScrollVertical = false;
      }
      else
      {
        AutoScrollVerticalMaximum = scrollLength;
        VisibleAutoScrollVertical = true;
      }
    }



    private void ItemsModified()
    {
      AdjustScrollbars();
      Invalidate();
    }



    public int ItemAtLocation( int X, int Y )
    {
      int     itemWidth = m_ItemWidth;
      int     itemHeight = m_ItemHeight;

      if ( m_DisplayPage.Width != ClientRectangle.Width )
      {
        itemWidth *= ( ClientRectangle.Width / m_DisplayPage.Width );
        itemHeight *= ( ClientRectangle.Width / m_DisplayPage.Width );
      }

      int xoffset = X / itemWidth;
      if ( xoffset >= m_ItemsPerLine )
      {
        return -1;
      }
      int yoffset = Y / itemHeight;
      if ( yoffset * m_ItemsPerLine + AutoScrollVPos * m_ItemsPerLine >= Items.Count )
      {
        return -1;
      }
      int itemIndex = ( m_Offset + yoffset ) * m_ItemsPerLine + xoffset;
      if ( itemIndex >= Items.Count )
      {
        return -1;
      }
      return itemIndex;
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



    protected void SetSelection( int SelectedIndex, bool SelectionIsRange, int AnchorIndex )
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



    protected override void OnMouseDown( MouseEventArgs e )
    {
      base.OnMouseDown( e );
      int oldIndex = SelectedIndex;
      int newIndex = ItemAtLocation( e.X, e.Y );
      if ( ( System.Windows.Forms.Control.ModifierKeys & Keys.Shift ) != 0 )
      {
        // range-selection
        SetSelection( newIndex, true, oldIndex );
      }
      else if ( ( System.Windows.Forms.Control.ModifierKeys & Keys.Alt ) != 0 )
      {
        if ( m_SelectionAnchor == -1 )
        {
          SetSelection( newIndex, false, oldIndex );
        }
        else
        {
          SetSelection( newIndex, false, m_SelectionAnchor );
        }
      }
      else
      {
        // single item selected
        SetSelection( newIndex, false, -1 );
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

      if ( ClientRectangle.Width != m_DisplayPage.Width )
      {
        int     factor = ClientRectangle.Width / m_DisplayPage.Width;

        trueRect.X *= factor;
        trueRect.Y *= factor;
        trueRect.Width *= factor;
        trueRect.Height *= factor;
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
      System.Drawing.Rectangle itemRect = new System.Drawing.Rectangle();
      while ( itemIndex < Items.Count )
      {
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
          e.Graphics.FillRectangle( System.Drawing.SystemBrushes.Control, e.ClipRectangle );
          e.Graphics.DrawImage( Items[itemIndex].Image, itemRect );
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

      IntPtr hdcPage = e.Graphics.GetHdc();
      m_DisplayPage.DrawToHDC( hdcPage, ClientRectangle );
      e.Graphics.ReleaseHdc( hdcPage );

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
              && ( i % m_ItemsPerLine <= x2 ) )
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
      Invalidate( ItemRect( ItemIndex ) );
    }



    protected override bool ProcessCmdKey( ref Message msg, Keys keyData )
    {
      if ( keyData == ( Keys.Up | Keys.Shift ) )
      {
        if ( ( SelectedIndex == -1 )
        &&   ( Items.Count > 0 ) )
        {
          SetSelection( 0, false, 0 );
        }
        if ( SelectedIndex >= m_ItemsPerLine )
        {
          if ( m_SelectionAnchor != -1 )
          {
            SetSelection( SelectedIndex - m_ItemsPerLine, true, m_SelectionAnchor );
          }
          else
          {
            SetSelection( SelectedIndex - m_ItemsPerLine, true, SelectedIndex );
          }
        }
        return true;
      }
      else if ( keyData == Keys.Up )
      {
        if ( ( SelectedIndex == -1 )
        &&   ( Items.Count > 0 ) )
        {
          SetSelection( 0, false, -1 );
        }
        if ( SelectedIndex >= m_ItemsPerLine )
        {
          SetSelection( SelectedIndex - m_ItemsPerLine, false, -1 );
        }
        return true;
      }
      else if ( keyData == ( Keys.Left | Keys.Shift ) )
      {
        if ( ( SelectedIndex == -1 )
        &&   ( Items.Count > 0 ) )
        {
          SetSelection( 0, false, 0 );
        }
        if ( SelectedIndex > 0 )
        {
          if ( m_SelectionAnchor != -1 )
          {
            SetSelection( SelectedIndex - 1, true, m_SelectionAnchor );
          }
          else
          {
            SetSelection( SelectedIndex - 1, true, SelectedIndex );
          }
        }
        return true;
      }
      else if ( keyData == Keys.Left )
      {
        if ( ( SelectedIndex == -1 )
        &&   ( Items.Count > 0 ) )
        {
          SetSelection( 0, false, -1 );
        }
        if ( SelectedIndex > 0 )
        {
          SetSelection( SelectedIndex - 1, false, -1 );
        }
        return true;
      }
      else if ( keyData == ( Keys.Right | Keys.Shift ) )
      {
        if ( ( SelectedIndex == -1 )
        &&   ( Items.Count > 0 ) )
        {
          SetSelection( Items.Count - 1, true, -1 );
          return true;
        }
        if ( SelectedIndex + 1 < Items.Count )
        {
          if ( m_SelectionAnchor != -1 )
          {
            SetSelection( SelectedIndex + 1, true, m_SelectionAnchor );
          }
          else
          {
            SetSelection( SelectedIndex + 1, true, SelectedIndex );
          }
        }
        return true;
      }
      else if ( keyData == Keys.Right )
      {
        if ( ( SelectedIndex == -1 )
        &&   ( Items.Count > 0 ) )
        {
          SetSelection( Items.Count - 1, false, -1 );
        }
        if ( SelectedIndex + 1 < Items.Count )
        {
          SetSelection( SelectedIndex + 1, false, -1 );
        }
        return true;
      }
      else if ( keyData == ( Keys.Down | Keys.Shift ) )
      {
        if ( ( SelectedIndex == -1 )
        &&   ( Items.Count > 0 ) )
        {
          SetSelection( Items.Count - 1, false, -1 );
        }
        if ( SelectedIndex + m_ItemsPerLine < Items.Count )
        {
          if ( m_SelectionAnchor == -1 )
          {
            SetSelection( SelectedIndex + m_ItemsPerLine, true, SelectedIndex );
          }
          else
          {
            SetSelection( SelectedIndex + m_ItemsPerLine, true, m_SelectionAnchor );
          }
        }
        return true;
      }
      else if ( keyData == Keys.Down )
      {
        if ( ( SelectedIndex == -1 )
        &&   ( Items.Count > 0 ) )
        {
          SetSelection( Items.Count - 1, false, -1 );
        }
        if ( SelectedIndex + m_ItemsPerLine < Items.Count )
        {
          SetSelection( SelectedIndex + m_ItemsPerLine, false, -1 );
        }
        return true;
      }
      return base.ProcessCmdKey( ref msg, keyData );
    }


  }
}
