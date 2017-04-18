using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;



namespace C64Studio
{
  public class ImageListbox : CustomAutoScrollPanel.ScrollablePanel
  {
    public class ImageListItem : IDisposable
    {
      //object                m_Value = null;
      //System.Drawing.Image  m_Image = null;
      private GR.Image.FastImage    m_FastImage = null;
      private ImageListbox          m_Container = null;
      private bool                  m_ImageOwner = true;


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

      public GR.Image.FastImage FastImage
      {
        get
        {
          return m_FastImage;
        }
        set
        {
          m_FastImage = value;
          m_ImageOwner = false;
        }
      }

      public void Dispose()
      {
        if ( ( m_ImageOwner )
        &&   ( m_FastImage != null ) )
        {
          m_FastImage.Dispose();
          m_FastImage = null;
        }
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

      public int Add( object Object, GR.Image.FastImage Image )
      {
        ImageListItem item = new ImageListItem( Container );
        item.Value = Object;
        item.FastImage = Image;
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

    public event System.Windows.Forms.DrawItemEventHandler    DrawItem;
    public event EventHandler                                 SelectedIndexChanged;



    protected override void Dispose( bool disposing )
    {
      foreach ( object obj in Items )
      {
        ImageListItem  imageItem = (ImageListItem)obj;
        imageItem.Dispose();
      }

      base.Dispose( disposing );
    }



    public int                    ItemWidth
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
        m_ItemsPerLine = ClientRectangle.Width / m_ItemWidth;
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



    public int SelectedIndex
    {
      get
      {
        return m_SelectedItem;
      }
      set
      {
        if ( m_SelectedItem != value )
        {
          InvalidateItemRect( m_SelectedItem );
          InvalidateItemRect( value );
          m_SelectedItem = value;
          if ( SelectedIndexChanged != null )
          {
            SelectedIndexChanged( this, new EventArgs() );
          }
        }
      }
    }



    public new event KeyEventHandler KeyDown;
    public event KeyPressEventHandler KeyPressed;
    public new event KeyEventHandler KeyUp;



    public ImageListbox()
    {
      Items = new ObjectCollection( this );

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



    protected override void OnSizeChanged( EventArgs e )
    {
      base.OnSizeChanged( e );

      if ( m_ItemWidth == -1 )
      {
        m_ItemWidth = ClientRectangle.Width;
      }
      m_ItemsPerLine = ClientRectangle.Width / m_ItemWidth;
      if ( m_ItemsPerLine == 0 )
      {
        m_ItemsPerLine = 1;
      }

      AdjustScrollbars();
    }



    private void AdjustScrollbars()
    {
      int   scrollLength = ( Items.Count / m_ItemsPerLine ) - ( ClientRectangle.Height / m_ItemHeight );
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
      int xoffset = X / m_ItemWidth;
      if ( xoffset >= m_ItemsPerLine )
      {
        return -1;
      }
      int yoffset = Y / m_ItemHeight;
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



    protected override void OnMouseDown( MouseEventArgs e )
    {
      base.OnMouseDown( e );
      SelectedIndex = ItemAtLocation( e.X, e.Y );
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
      System.Drawing.SolidBrush hottrackBrush = new System.Drawing.SolidBrush( GR.Color.Helper.FromARGB( 0x804040ff ) );

      int     itemIndex = m_Offset * m_ItemsPerLine;
      while ( itemIndex < Items.Count )
      {
        int     xoffset = ( itemIndex - m_Offset * m_ItemsPerLine ) % m_ItemsPerLine;
        int     yoffset = ( itemIndex - m_Offset * m_ItemsPerLine ) / m_ItemsPerLine;
        System.Drawing.Rectangle itemRect = new System.Drawing.Rectangle( xoffset * m_ItemWidth, yoffset * m_ItemHeight, m_ItemWidth, m_ItemHeight );

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
        else if ( Items[itemIndex].FastImage != null )
        {
          IntPtr hdc = e.Graphics.GetHdc();
          Items[itemIndex].FastImage.DrawToHDC( hdc, itemRect );
          e.Graphics.ReleaseHdc( hdc );
        }
        if ( ( itemIndex == m_ItemUnderMouse )
        ||   ( itemIndex == m_SelectedItem ) )
        {
          e.Graphics.FillRectangle( hottrackBrush, itemRect );
        }
        ++itemIndex;
      }
    }



    protected override void OnScroll( ScrollEventArgs se )
    {
      base.OnScroll( se );


      if ( se.ScrollOrientation == ScrollOrientation.VerticalScroll )
      {
        int newValue = m_Offset;
        int maxValue = ( Items.Count / m_ItemsPerLine ) - ( ClientRectangle.Height / m_ItemHeight );
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

  }
}
