using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace C64Studio
{
  public class FastPictureBox : System.Windows.Forms.PictureBox
  {
    [DllImport( "user32.dll" )]
    static extern bool ValidateRect( IntPtr hWnd, IntPtr lpRect );

    private GR.Image.FastImage m_Image = new GR.Image.FastImage();

    //public IntPtr         m_PixelData = IntPtr.Zero;
    //private IntPtr        m_Bitmap    = IntPtr.Zero;
    private int           m_Width     = 0;
    private int           m_Height    = 0;
    private int           m_ImageWidth     = 0;
    private int           m_ImageHeight    = 0;

    private int           m_DirtyRectLeft = -1;
    private int           m_DirtyRectTop = -1;
    private int           m_DirtyRectRight = -1;
    private int           m_DirtyRectBottom = -1;

    private bool          m_ExternalImage = false;



    public FastPictureBox()
    {
      //this.SetStyle( ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer, true );
      this.SetStyle( ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true );
    }



    protected override void Dispose( bool disposing )
    {
      base.Dispose( disposing );
      if ( ( !m_ExternalImage )
      &&   ( m_Image != null ) )
      {
        m_Image.Dispose();
      }
      m_Image = null;
    }



    public GR.Image.FastImage FastImage
    {
      get
      {
        return m_Image;
      }
      set
      {
        m_Image         = value;
        m_ExternalImage = true;
        Invalidate();
      }
    }



    public void SetImageSize( int Width, int Height )
    {
      if ( m_ExternalImage )
      {
        return;
      }

      if ( ( m_ImageWidth != Width )
      ||   ( m_ImageHeight != Height ) )
      {
        m_ImageWidth = Width;
        m_ImageHeight = Height;

        m_DirtyRectLeft   = 0;
        m_DirtyRectTop    = 0;
        m_DirtyRectRight  = Width;
        m_DirtyRectBottom = Height;

        m_Image.Resize( Width, Height );
        //CreateBitmap();
      }
    }



    protected override void OnSizeChanged( EventArgs e )
    {
      int newWidth = ClientRectangle.Width;
      int newHeight = ClientRectangle.Width;

      if ( ( newWidth != m_Width )
      ||   ( newHeight != m_Height ) )
      {
        if ( m_ImageWidth == 0 )
        {
          m_ImageWidth  = newWidth;
          m_ImageHeight = newHeight;

          m_DirtyRectLeft   = 0;
          m_DirtyRectTop    = 0;
          m_DirtyRectRight  = m_ImageWidth;
          m_DirtyRectBottom = m_ImageHeight;
        }
        if ( !m_ExternalImage )
        {
          m_Image.Resize( newWidth, newHeight );
        }
        m_Width = newWidth;
        m_Height = newHeight;
      }
      base.OnSizeChanged( e );
    }



    public void Draw()
    {
      Invalidate();
      m_Image.Draw( Handle, ClientRectangle );
      Update();
    }



    protected override void WndProc( ref Message m )
    {
      if ( m.Msg == 0x000f )
      {
        //Debug.Log( "WM_PAINT" );
        m_Image.Draw( Handle, ClientRectangle );
        return;
      }
      base.WndProc( ref m );
    }



    public void MarkDirty( int X, int Y )
    {
      if ( m_DirtyRectLeft == -1 )
      {
        m_DirtyRectLeft   = X;
        m_DirtyRectTop    = Y;
        m_DirtyRectRight  = X + 1;
        m_DirtyRectBottom = Y + 1;
      }
      else
      {
        m_DirtyRectLeft   = Math.Min( X, m_DirtyRectLeft );
        m_DirtyRectTop    = Math.Min( Y, m_DirtyRectTop );
        m_DirtyRectRight  = Math.Max( X + 1, m_DirtyRectRight );
        m_DirtyRectBottom = Math.Max( Y + 1, m_DirtyRectBottom );
      }
    }

  }
}

