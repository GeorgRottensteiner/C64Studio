using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace GR.Forms
{
  public class FastPictureBox : System.Windows.Forms.PictureBox
  {
    [DllImport( "user32.dll" )]
    static extern bool ValidateRect( IntPtr hWnd, IntPtr lpRect );

    private GR.Image.FastImage m_DisplayPage = new GR.Image.FastImage();
    private GR.Image.FastImage m_DisplayPageBuffer = new GR.Image.FastImage();

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

    private GR.Image.MemoryImage    m_MemoryImage = null;

    public delegate void PostPaintCallback( GR.Image.FastImage TargetBuffer );

    public event PostPaintCallback   PostPaint;


    public FastPictureBox()
    {
      //this.SetStyle( ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer, true );
      this.SetStyle( ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true );
    }



    public new GR.Image.MemoryImage     Image
    {
      get
      {
        return m_MemoryImage;
      }
      set
      {
        m_MemoryImage = value;
        Invalidate();
      }
    }



    public GR.Image.FastImage DisplayPage
    {
      get
      {
        return m_DisplayPage;
      }
      set
      {
        if ( m_DisplayPage != null )
        {
          m_DisplayPage.Dispose();
          m_DisplayPageBuffer.Dispose();
        }
        m_DisplayPage = value;
        m_DisplayPageBuffer = new GR.Image.FastImage( ClientRectangle.Width, ClientRectangle.Height, m_DisplayPage.PixelFormat );
        Invalidate();
      }
    }



    public void SetImageSize( int Width, int Height )
    {
      if ( ( m_ImageWidth != Width )
      ||   ( m_ImageHeight != Height ) )
      {
        m_ImageWidth = Width;
        m_ImageHeight = Height;

        m_DirtyRectLeft   = 0;
        m_DirtyRectTop    = 0;
        m_DirtyRectRight  = Width;
        m_DirtyRectBottom = Height;

        m_DisplayPage.Resize( Width, Height );
        Invalidate();
      }
    }



    protected override void OnSizeChanged( EventArgs e )
    {
      int newWidth = ClientRectangle.Width;
      int newHeight = ClientRectangle.Height;

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
        m_DisplayPage.Resize( newWidth, newHeight );
        m_DisplayPageBuffer.Resize( newWidth, newHeight );
        m_Width = newWidth;
        m_Height = newHeight;
      }
      base.OnSizeChanged( e );
    }


    /*
    public void Draw()
    {
      if ( ( m_DisplayPage.Width == 0 )
      ||   ( m_MemoryImage == null ) )
      {
        System.Drawing.Graphics g = CreateGraphics();
        g.FillRectangle( System.Drawing.SystemBrushes.ButtonFace, ClientRectangle );

        OnPaint( new PaintEventArgs( g, ClientRectangle ) );
        g.Dispose();
        return;
      }
      Invalidate();

      m_DisplayPage.DrawFromMemoryImage( m_MemoryImage, 0, 0 );
      System.Drawing.Graphics g2 = CreateGraphics();
      OnPaint( new PaintEventArgs( g2, ClientRectangle ) );
      g2.Dispose();
      Update();
    }
    */


    private void OnPostPaint( GR.Image.FastImage TargetBuffer )
    {
      if ( ( PostPaint != null )
      &&   ( PostPaint.Target != null ) )
      {
        PostPaint( TargetBuffer );
      }
    }



    protected override void WndProc( ref Message m )
    {
      if ( m.Msg == 0x000f )
      {
        //Debug.Log( "WM_PAINT" );
        if ( m_DisplayPage.Width == 0 )
        {
          System.Drawing.Graphics g = CreateGraphics();
          g.FillRectangle( System.Drawing.SystemBrushes.ButtonFace, ClientRectangle );
          OnPaint( new PaintEventArgs( g, ClientRectangle ) );
          g.Dispose();
          base.WndProc( ref m );
        }
        else
        {
          if ( m_MemoryImage != null )
          {
            m_DisplayPage.DrawFromMemoryImage( m_MemoryImage, 0, 0 );
          }
          for ( int i = 0; i < 256; ++i )
          {
            m_DisplayPageBuffer.SetPaletteColor( i, m_DisplayPage.PaletteRed( i ), m_DisplayPage.PaletteGreen( i ), m_DisplayPage.PaletteBlue( i ) );
          }
          m_DisplayPage.BlitTo( m_DisplayPageBuffer );

          OnPostPaint( m_DisplayPageBuffer );

          m_DisplayPageBuffer.Draw( Handle, ClientRectangle );

          if ( m_DisplayPageBuffer.Width < ClientSize.Width )
          {
            System.Drawing.Graphics g = CreateGraphics();

            g.FillRectangle( System.Drawing.SystemBrushes.ButtonFace, m_DisplayPageBuffer.Width, 0, ClientSize.Width - m_DisplayPageBuffer.Width, m_DisplayPageBuffer.Height );

            g.Dispose();
          }
          //m_DisplayPage.Draw( Handle, ClientRectangle );
        }
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
        m_DirtyRectLeft   = System.Math.Min( X, m_DirtyRectLeft );
        m_DirtyRectTop    = System.Math.Min( Y, m_DirtyRectTop );
        m_DirtyRectRight  = System.Math.Max( X + 1, m_DirtyRectRight );
        m_DirtyRectBottom = System.Math.Max( Y + 1, m_DirtyRectBottom );
      }
    }

  }
}

