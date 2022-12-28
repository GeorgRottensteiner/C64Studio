using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif

namespace GR.Image
{
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
#endif
  public class FastImage : IDisposable, IImage
  {
    [StructLayout( LayoutKind.Sequential )]
    public struct RECT
    {
      private int _Left;
      private int _Top;
      private int _Right;
      private int _Bottom;

      public RECT( RECT Rectangle )
        : this( Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom )
      {
      }
      public RECT( int Left, int Top, int Right, int Bottom )
      {
        _Left = Left;
        _Top = Top;
        _Right = Right;
        _Bottom = Bottom;
      }

      public int X
      {
        get
        {
          return _Left;
        }
        set
        {
          _Left = value;
        }
      }
      public int Y
      {
        get
        {
          return _Top;
        }
        set
        {
          _Top = value;
        }
      }
      public int Left
      {
        get
        {
          return _Left;
        }
        set
        {
          _Left = value;
        }
      }
      public int Top
      {
        get
        {
          return _Top;
        }
        set
        {
          _Top = value;
        }
      }
      public int Right
      {
        get
        {
          return _Right;
        }
        set
        {
          _Right = value;
        }
      }
      public int Bottom
      {
        get
        {
          return _Bottom;
        }
        set
        {
          _Bottom = value;
        }
      }
      public int Height
      {
        get
        {
          return _Bottom - _Top;
        }
        set
        {
          _Bottom = value - _Top;
        }
      }
      public int Width
      {
        get
        {
          return _Right - _Left;
        }
        set
        {
          _Right = value + _Left;
        }
      }

      public static implicit operator System.Drawing.Rectangle( RECT Rectangle )
      {
        return new System.Drawing.Rectangle( Rectangle.Left, Rectangle.Top, Rectangle.Width, Rectangle.Height );
      }
      public static implicit operator RECT( System.Drawing.Rectangle Rectangle )
      {
        return new RECT( Rectangle.Left, Rectangle.Top, Rectangle.Right, Rectangle.Bottom );
      }
      public static bool operator ==( RECT Rectangle1, RECT Rectangle2 )
      {
        return Rectangle1.Equals( Rectangle2 );
      }
      public static bool operator !=( RECT Rectangle1, RECT Rectangle2 )
      {
        return !Rectangle1.Equals( Rectangle2 );
      }

      public override string ToString()
      {
        return "{Left: " + _Left + "; " + "Top: " + _Top + "; Right: " + _Right + "; Bottom: " + _Bottom + "}";
      }

      public override int GetHashCode()
      {
        return ToString().GetHashCode();
      }

      public bool Equals( RECT Rectangle )
      {
        return Rectangle.Left == _Left && Rectangle.Top == _Top && Rectangle.Right == _Right && Rectangle.Bottom == _Bottom;
      }

      public override bool Equals( object Object )
      {
        if ( Object is RECT )
        {
          return Equals( (RECT)Object );
        }
        else if ( Object is System.Drawing.Rectangle )
        {
          return Equals( new RECT( (System.Drawing.Rectangle)Object ) );
        }

        return false;
      }
    }


    [DllImport( "user32.dll" )]
    static extern bool InvalidateRect( IntPtr hWnd, IntPtr lpRect, bool bErase );
    [DllImport( "user32.dll" )]
    static extern bool ValidateRect( IntPtr hWnd, ref GR.Image.FastImage.RECT lpRect );

#if NET5_0_OR_GREATER
    [DllImport( "Kernel32.dll", EntryPoint = "RtlCopyMemory" )]
#else
    [DllImport( "Kernel32.dll", EntryPoint = "CopyMemory" )]
    #endif
    static extern void CopyMemory( IntPtr dest, IntPtr src, uint length );

    [DllImport( "user32.dll" )]
    static extern IntPtr GetDC( IntPtr hWnd );
    [DllImport( "user32.dll" )]
    static extern bool ReleaseDC( IntPtr hWnd, IntPtr hDC );
    [DllImport( "gdi32.dll", SetLastError = true )]
    static extern IntPtr CreateCompatibleDC( IntPtr hdc );
    [DllImport( "gdi32.dll" )]
    static extern bool DeleteDC( IntPtr hdc );
    [DllImport( "gdi32.dll", SetLastError = true )]
    static extern IntPtr CreateDIBSection( IntPtr hdc, [In] ref BITMAPINFO pbmi, uint pila, out IntPtr ppvBits, IntPtr hSection, uint dwOffset );

    [DllImport( "gdi32.dll" )]
    static extern bool DeleteObject( IntPtr hObject );
    [DllImport( "gdi32.dll", ExactSpelling = true, PreserveSig = true, SetLastError = true )]
    static extern IntPtr SelectObject( IntPtr hdc, IntPtr hgdiobj );
    [DllImport( "gdi32.dll" )]
    static extern IntPtr SelectPalette( IntPtr hdc, IntPtr hpal, bool ForceBackground );
    [DllImport( "gdi32.dll" )]
    static extern uint RealizePalette( IntPtr hdc );
    [DllImport( "gdi32.dll" )]
    unsafe static extern uint SetDIBColorTable( IntPtr hdc, uint uStartIndex, uint cEntries, RGBQUAD* pColors );

    [DllImport( "kernel32.dll" )]
    static extern IntPtr GlobalLock( IntPtr hMem );
    [DllImport( "kernel32.dll" )]
    [return: MarshalAs( UnmanagedType.Bool )]
    static extern bool GlobalUnlock( IntPtr hMem );



    [StructLayout( LayoutKind.Sequential )]
    public struct BITMAPINFO
    {
      public Int32 biSize;
      public Int32 biWidth;
      public Int32 biHeight;
      public Int16 biPlanes;
      public Int16 biBitCount;
      public Int32 biCompression;
      public Int32 biSizeImage;
      public Int32 biXPelsPerMeter;
      public Int32 biYPelsPerMeter;
      public Int32 biClrUsed;
      public Int32 biClrImportant;
      public Int32 colors;
    }

    [StructLayout( LayoutKind.Sequential, Pack = 1 )]
    public struct LOGPALETTE
    {
      public UInt16             wVersion,
                                wNumberOfEntries;
      public unsafe fixed Byte  colorEntries[256 * 4];
    }

    /// <summary>
    ///    Performs a bit-block transfer of the color data corresponding to a
    ///    rectangle of pixels from the specified source device context into
    ///    a destination device context.
    /// </summary>
    /// <param name="hdc">Handle to the destination device context.</param>
    /// <param name="nXDest">The leftmost x-coordinate of the destination rectangle (in pixels).</param>
    /// <param name="nYDest">The topmost y-coordinate of the destination rectangle (in pixels).</param>
    /// <param name="nWidth">The width of the source and destination rectangles (in pixels).</param>
    /// <param name="nHeight">The height of the source and the destination rectangles (in pixels).</param>
    /// <param name="hdcSrc">Handle to the source device context.</param>
    /// <param name="nXSrc">The leftmost x-coordinate of the source rectangle (in pixels).</param>
    /// <param name="nYSrc">The topmost y-coordinate of the source rectangle (in pixels).</param>
    /// <param name="dwRop">A raster-operation code.</param>
    /// <returns>
    ///    <c>true</c> if the operation succeeded, <c>false</c> otherwise.
    /// </returns>
    [DllImport("gdi32.dll")]
    static extern IntPtr CreatePalette( [In] ref LOGPALETTE lplgpl );
    [DllImport( "gdi32.dll" )]
    [return: MarshalAs( UnmanagedType.Bool )]
    static extern bool BitBlt( IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, TernaryRasterOperations dwRop );
    [DllImport( "gdi32.dll" )]
    static extern bool StretchBlt( IntPtr hdcDest, int nXOriginDest, int nYOriginDest,
        int nWidthDest, int nHeightDest,
        IntPtr hdcSrc, int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int nHeightSrc,
        TernaryRasterOperations dwRop );

    enum TernaryRasterOperations : uint
    {
      /// <summary>dest = source</summary>
      SRCCOPY = 0x00CC0020,
      /// <summary>dest = source OR dest</summary>
      SRCPAINT = 0x00EE0086,
      /// <summary>dest = source AND dest</summary>
      SRCAND = 0x008800C6,
      /// <summary>dest = source XOR dest</summary>
      SRCINVERT = 0x00660046,
      /// <summary>dest = source AND (NOT dest)</summary>
      SRCERASE = 0x00440328,
      /// <summary>dest = (NOT source)</summary>
      NOTSRCCOPY = 0x00330008,
      /// <summary>dest = (NOT src) AND (NOT dest)</summary>
      NOTSRCERASE = 0x001100A6,
      /// <summary>dest = (source AND pattern)</summary>
      MERGECOPY = 0x00C000CA,
      /// <summary>dest = (NOT source) OR dest</summary>
      MERGEPAINT = 0x00BB0226,
      /// <summary>dest = pattern</summary>
      PATCOPY = 0x00F00021,
      /// <summary>dest = DPSnoo</summary>
      PATPAINT = 0x00FB0A09,
      /// <summary>dest = pattern XOR dest</summary>
      PATINVERT = 0x005A0049,
      /// <summary>dest = (NOT dest)</summary>
      DSTINVERT = 0x00550009,
      /// <summary>dest = BLACK</summary>
      BLACKNESS = 0x00000042,
      /// <summary>dest = WHITE</summary>
      WHITENESS = 0x00FF0062,
      /// <summary>
      /// Capture window as seen on screen.  This includes layered windows 
      /// such as WPF windows with AllowsTransparency="true"
      /// </summary>
      CAPTUREBLT = 0x40000000
    }



    private IntPtr  m_ImageData = IntPtr.Zero;
    private int     m_Width = 0;
    private int     m_Height = 0;
    private IntPtr  m_Bitmap = IntPtr.Zero;
    private GR.Drawing.PixelFormat  m_PixelFormat = GR.Drawing.PixelFormat.Format32bppRgb;
    private GR.Memory.ByteBuffer    m_PaletteData = null;
    private IntPtr  m_Palette = IntPtr.Zero;



    public int Width
    {
      get
      {
        return m_Width;
      }
    }



    public int Height
    {
      get
      {
        return m_Height;
      }
    }



    public GR.Drawing.PixelFormat PixelFormat
    {
      get
      {
        return m_PixelFormat;
      }
    }



    public IntPtr PinData()
    {
      return m_ImageData;
    }



    public void UnpinData()
    {
    }



    public int BytesPerLine
    {
      get
      {
        switch ( m_PixelFormat )
        {
          case GR.Drawing.PixelFormat.Format8bppIndexed:
            if ( ( Width % 4 ) != 0 )
            {
              return ( ( Width + 3 ) / 4 ) * 4;
            }
            return Width;
          case GR.Drawing.PixelFormat.Format4bppIndexed:
            if ( ( Width % 4 ) != 0 )
            {
              return Width / 4 + 1;
            }
            return Width / 4;
          case GR.Drawing.PixelFormat.Format16bppArgb1555:
          case GR.Drawing.PixelFormat.Format16bppGrayScale:
          case GR.Drawing.PixelFormat.Format16bppRgb555:
          case GR.Drawing.PixelFormat.Format16bppRgb565:
            return Width * 2;
          case GR.Drawing.PixelFormat.Format1bppIndexed:
            return ( ( Width + 7 ) / 8 );
          case GR.Drawing.PixelFormat.Format24bppRgb:
            if ( ( ( Width * 3 ) % 4 ) != 0 )
            {
              return Width * 3 + ( 4 - ( ( Width * 3 ) % 4 ) );
            }
            return Width * 3;
          case GR.Drawing.PixelFormat.Format32bppArgb:
          case GR.Drawing.PixelFormat.Format32bppPArgb:
          case GR.Drawing.PixelFormat.Format32bppRgb:
            return Width * 4;
        }
        return 0;
      }
    }



    public int BitsPerPixel
    {
      get
      {
        switch ( m_PixelFormat )
        {
          case GR.Drawing.PixelFormat.Format8bppIndexed:
            return 8;
          case GR.Drawing.PixelFormat.Format4bppIndexed:
            return 4;
          case GR.Drawing.PixelFormat.Format16bppArgb1555:
          case GR.Drawing.PixelFormat.Format16bppGrayScale:
          case GR.Drawing.PixelFormat.Format16bppRgb555:
          case GR.Drawing.PixelFormat.Format16bppRgb565:
            return 16;
          case GR.Drawing.PixelFormat.Format1bppIndexed:
            return 1;
          case GR.Drawing.PixelFormat.Format24bppRgb:
            return 24;
          case GR.Drawing.PixelFormat.Format32bppArgb:
          case GR.Drawing.PixelFormat.Format32bppPArgb:
          case GR.Drawing.PixelFormat.Format32bppRgb:
            return 32;
        }
        return 0;
      }
    }



#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
#endif
    public System.Drawing.Imaging.BitmapData BitmapData
    {
      get
      {
        System.Drawing.Imaging.BitmapData bmpData = new System.Drawing.Imaging.BitmapData();

        bmpData.Width = Width;
        bmpData.Height = Height;
        bmpData.PixelFormat = (System.Drawing.Imaging.PixelFormat)m_PixelFormat;
        bmpData.Scan0 = m_ImageData;
        bmpData.Stride = BytesPerLine;

        return bmpData;
      }
    }



    public void Line( int X1, int Y1, int X2, int Y2, uint Color )
    {
      int dy = Y2 - Y1;
      int dx = X2 - X1;
      int stepx, stepy;

      if ( dy < 0 )
      {
        dy = -dy;
        stepy = -1;
      }
      else
      {
        stepy = 1;
      }
      if ( dx < 0 )
      {
        dx = -dx;
        stepx = -1;
      }
      else
      {
        stepx = 1;
      }

      dy <<= 1;
      dx <<= 1;

      SetPixel( X1, Y1, Color );
      if ( dx > dy )
      {
        int fraction = dy - ( dx >> 1 );

        while ( X1 != X2 )
        {
          if ( fraction >= 0 )
          {
            Y1 += stepy;
            fraction -= dx;
          }
          X1 += stepx;
          fraction += dy;
          SetPixel( X1, Y1, Color );
        }
      }
      else
      {
        int fraction = dx - ( dy >> 1 );

        while ( Y1 != Y2 )
        {
          if ( fraction >= 0 )
          {
            X1 += stepx;
            fraction -= dy;
          }
          Y1 += stepy;
          fraction += dx;
          SetPixel( X1, Y1, Color );
        }
      }
    }



    public FastImage()
    {
    }



    public FastImage( FastImage CloneSource )
    {
      m_Width   = CloneSource.m_Width;
      m_Height  = CloneSource.m_Height;

      CreateBitmap();

      CopyMemory( m_ImageData, CloneSource.m_ImageData, (uint)( BytesPerLine * m_Height ) );
    }



    public FastImage( MemoryImage Source )
    {
      m_Width = Source.Width;
      m_Height = Source.Height;
      m_PixelFormat = Source.PixelFormat;

      CreateBitmap();

      for ( int i = 0; i < Source.PaletteEntryCount; ++i )
      {
        SetPaletteColor( i, Source.PaletteRed( i ), Source.PaletteGreen( i ), Source.PaletteBlue( i ) );
      }

      DrawImage( Source, 0, 0 );
    }



    public FastImage( int Width, int Height, GR.Drawing.PixelFormat PixelFormat )
    {
      m_Width       = Width;
      m_Height      = Height;
      CreateBitmap( PixelFormat );
    }



    public FastImage( int Width, int Height )
    {
      m_Width   = Width;
      m_Height  = Height;

      CreateBitmap();
    }



    public void Dispose()
    {
      if ( m_Palette != IntPtr.Zero )
      {
        DeleteObject( m_Palette );
        m_Palette = IntPtr.Zero;
      }
      if ( m_Bitmap != IntPtr.Zero )
      {
        DeleteObject( m_Bitmap );
        m_Bitmap = IntPtr.Zero;
      }
    }



    public void Resize( int Width, int Height )
    {
      if ( ( m_Width != Width )
      ||   ( m_Height != Height ) )
      {
        m_Width = Width;
        m_Height = Height;
        CreateBitmap();
      }
    }



    public bool Create( int Width, int Height, GR.Drawing.PixelFormat PixelFormat )
    {
      m_Width   = Width;
      m_Height  = Height;
      return ( CreateBitmap( PixelFormat ) != IntPtr.Zero );
    }



    public void DrawToHDC( IntPtr HDC, System.Drawing.Rectangle Rect, int SourceX, int SourceY, int SourceWidth, int SourceHeight )
    {
      if ( m_Bitmap == IntPtr.Zero )
      {
        CreateBitmap();
      }
      IntPtr imageDC = CreateCompatibleDC( IntPtr.Zero );

      IntPtr oldObject = SelectObject( imageDC, m_Bitmap );
      IntPtr previousPalette = IntPtr.Zero;
      if ( m_Palette != IntPtr.Zero )
      {
        previousPalette = SelectPalette( HDC, m_Palette, false );
        RealizePalette( HDC );
      }

      StretchBlt( HDC, Rect.Left, Rect.Top, Rect.Width, Rect.Height, imageDC, SourceX, SourceY, SourceWidth, SourceHeight, TernaryRasterOperations.SRCCOPY );

      SelectObject( imageDC, oldObject );

      if ( m_Palette != IntPtr.Zero )
      {
        SelectPalette( HDC, previousPalette, false );
      }

      DeleteDC( imageDC );
    }



    public void DrawToHDC( IntPtr HDC, System.Drawing.Rectangle Rect )
    {
      DrawToHDC( HDC, Rect, 0, 0, m_Width, m_Height );
    }



    public void DrawImage( GR.Image.IImage Image, int X, int Y, int SourceX, int SourceY, int DrawWidth, int DrawHeight )
    {
      if ( Image.PixelFormat != PixelFormat )
      {
        if ( PixelFormat == GR.Drawing.PixelFormat.Undefined )
        {
          throw new NotSupportedException( "Pixelformat has not been set" );
        }
        /*
        else
        {
          throw new NotSupportedException( "Mismatching Pixelformat is not supported yet" );
        }*/
      }
      // clip to source
      if ( ( SourceX >= Image.Width )
      ||   ( SourceX + DrawWidth < 0 )
      ||   ( SourceY >= Image.Height )
      ||   ( SourceY + DrawHeight < 0 ) )
      {
        return;
      }
      if ( SourceX + DrawWidth > Image.Width )
      {
        DrawWidth = Image.Width - SourceX;
      }
      if ( SourceX < 0 )
      {
        DrawWidth += SourceX;
        X += SourceX;
        SourceX = 0;
      }
      if ( SourceY + DrawHeight > Image.Height )
      {
        DrawHeight = Image.Height - SourceY;
      }
      if ( SourceY < 0 )
      {
        DrawHeight += SourceY;
        Y += SourceY;
        SourceY = 0;
      }

      //int copyWidth = System.Math.Min( DrawWidth, Width );
      //int copyHeight = System.Math.Min( DrawHeight, Height );
      int copyWidth = DrawWidth;
      int copyHeight = DrawHeight;

      // clip to target
      if ( ( X >= Width )
      ||   ( Y >= Height )
      ||   ( X + copyWidth < 0 )
      ||   ( Y + copyHeight < 0 ) )
      {
        return;
      }

      if ( X < 0 )
      {
        copyWidth += X;
        SourceX -= X;
        X = 0;
      }
      if ( X + copyWidth >= Width )
      {
        copyWidth = Width - X;
      }
      if ( Y < 0 )
      {
        copyHeight += Y;
        SourceY -= Y;
        Y = 0;
      }
      if ( Y + copyHeight >= Height )
      {
        copyHeight = Height - Y;
      }

      if ( Image.PixelFormat != PixelFormat )
      {
        switch ( Image.PixelFormat )
        {
          case GR.Drawing.PixelFormat.Format8bppIndexed:
            if ( ( PixelFormat == GR.Drawing.PixelFormat.Format24bppRgb )
            ||   ( PixelFormat == GR.Drawing.PixelFormat.Format32bppRgb ) )
            {
              for ( int y = 0; y < copyHeight; ++y )
              {
                for ( int x = 0; x < copyWidth; ++x )
                {
                  uint      pixel = Image.GetPixel( SourceX + x, SourceY + y );

                  SetPixelData( X + x, Y + y, Image.PaletteColor( (int)pixel ) );
                }
              }
              return;
            }
            break;
        }
        throw new NotSupportedException( "This mismatching Pixelformat is not supported yet" );
      }

      if ( ( PixelFormat == GR.Drawing.PixelFormat.Format1bppIndexed )
      ||   ( PixelFormat == GR.Drawing.PixelFormat.Format4bppIndexed ) )
      {
        // less than 1 byte per pixel
        for ( int y = 0; y < copyHeight; ++y )
        {
          for ( int x = 0; x < copyWidth; ++x )
          {
            SetPixelData( X + x, Y + y, Image.GetPixel( SourceX + x, SourceY + y ) );
          }
        }
      }
      else
      {
        unsafe
        {
          byte*   sourceData = (byte*)Image.PinData();
          byte*   pTargetData = (byte*)m_ImageData;

          
          for ( int y = 0; y < copyHeight; ++y )
          {
            CopyMemory( (IntPtr)( pTargetData + X * BitsPerPixel / 8 + ( y + Y ) * BytesPerLine ),
                        (IntPtr)( sourceData + SourceX * Image.BitsPerPixel / 8 + ( y + SourceY ) * Image.BytesPerLine ),
                        (uint)( copyWidth * BitsPerPixel / 8 ) );
          }
          Image.UnpinData();
        }
      }
    }



    public void DrawImage( GR.Image.IImage Image, int X, int Y )
    {
      DrawImage( Image, X, Y, 0, 0, Image.Width, Image.Height );
    }



    public void BlitTo( GR.Image.FastImage Target )
    {
      if ( m_Bitmap == IntPtr.Zero )
      {
        CreateBitmap();
      }
      IntPtr dc = Target.GetDC();

      IntPtr imageDC = CreateCompatibleDC( IntPtr.Zero );

      IntPtr oldObject = SelectObject( imageDC, m_Bitmap );

      IntPtr previousPalette = IntPtr.Zero;
      if ( m_Palette != IntPtr.Zero )
      {
        previousPalette = SelectPalette( dc, m_Palette, false );
        RealizePalette( dc );
      }

      StretchBlt( dc, 0, 0, Target.Width, Target.Height, imageDC, 0, 0, m_Width, m_Height, TernaryRasterOperations.SRCCOPY );

      SelectObject( imageDC, oldObject );
      if ( m_Palette != IntPtr.Zero )
      {
        SelectPalette( dc, previousPalette, false );
      }
      DeleteDC( imageDC );
      Target.ReleaseDC( dc );
    }



    public void Draw( IntPtr Handle, System.Drawing.Rectangle Rect )
    {
      if ( m_Bitmap == IntPtr.Zero )
      {
        CreateBitmap();
      }

      IntPtr dc = GetDC( Handle );

      IntPtr imageDC = CreateCompatibleDC( IntPtr.Zero );

      IntPtr oldObject = SelectObject( imageDC, m_Bitmap );

      IntPtr previousPalette = IntPtr.Zero;
      if ( m_Palette != IntPtr.Zero )
      {
        previousPalette = SelectPalette( dc, m_Palette, false );
        RealizePalette( dc );
      }

      StretchBlt( dc, Rect.Left, Rect.Top, Rect.Width, Rect.Height, imageDC, 0, 0, m_Width, m_Height, TernaryRasterOperations.SRCCOPY );

      SelectObject( imageDC, oldObject );
      if ( m_Palette != IntPtr.Zero )
      {
        SelectPalette( dc, previousPalette, false );
      }
      DeleteDC( imageDC );
      ReleaseDC( Handle, dc );

      RECT rect = new RECT( Rect );

      ValidateRect( Handle, ref rect );
    }



    private IntPtr CreateBitmap()
    {
      return CreateBitmap( m_PixelFormat );
    }



    private void SetPalette( IntPtr PalObj )
    {
      if ( m_Palette != IntPtr.Zero )
      {
        DeleteObject( m_Palette );
        m_Palette = IntPtr.Zero;
      }
      m_Palette = PalObj;
    }



    private void RebuildPalette()
    {
      if ( ( m_PaletteData == null )
      ||   ( m_PaletteData.Length == 0 ) )
      {
        return;
      }

      LOGPALETTE    logPal = new LOGPALETTE();

      logPal.wNumberOfEntries = (ushort)( m_PaletteData.Length / 3 );
      logPal.wVersion = 0x0300;

      unsafe
      {
        for ( int i = 0; i < m_PaletteData.Length / 3; ++i )
        {
          logPal.colorEntries[i * 4 + 0] = m_PaletteData.ByteAt( i * 3 + 2 );
          logPal.colorEntries[i * 4 + 1] = m_PaletteData.ByteAt( i * 3 + 1 );
          logPal.colorEntries[i * 4 + 2] = m_PaletteData.ByteAt( i * 3 + 0 );
          logPal.colorEntries[i * 4 + 3] = (byte)PaletteEntryFlag.PC_NOCOLLAPSE;
        }
      }
      IntPtr dc = CreateCompatibleDC( IntPtr.Zero );
      IntPtr oldObj = SelectObject( dc, m_Bitmap );
      unsafe
      {
        SetDIBColorTable( dc, 0, m_PaletteData.Length / 3, (RGBQUAD*)logPal.colorEntries );
      }
      SelectObject( dc, oldObj );
      DeleteDC( dc );

      IntPtr  palObj = CreatePalette( ref logPal );
      if ( palObj != IntPtr.Zero )
      {
        SetPalette( palObj );
      }
    }



    private IntPtr CreateBitmap( GR.Drawing.PixelFormat PixelFormat )
    {
      if ( m_Bitmap != IntPtr.Zero )
      {
        DeleteObject( m_Bitmap );
        m_Bitmap = IntPtr.Zero;
      }
      if ( m_Palette != IntPtr.Zero )
      {
        DeleteObject( m_Palette );
        m_Palette = IntPtr.Zero;
      }
      bool    keptPalette = true;
      if ( m_PaletteData == null )
      {
        keptPalette = false;
      }
      if ( PixelFormat != m_PixelFormat )
      {
        keptPalette = false;
        if ( m_PaletteData != null )
        {
          m_PaletteData.Clear();
          m_PaletteData = null;
        }
      }

      BITMAPINFO bmInfo = new BITMAPINFO();

      m_PixelFormat = PixelFormat;
      bmInfo.biWidth        = m_Width;
      bmInfo.biHeight       = -m_Height;
      bmInfo.biSize         = 40; // sizeof( BITMAPINFOHEADER );
      bmInfo.biPlanes       = 1;
      bmInfo.biBitCount     = (short)BitsPerPixel;
      bmInfo.biCompression  = (int)BitmapCompression.BI_RGB;

      if ( !keptPalette )
      {
        switch ( m_PixelFormat )
        {
          case GR.Drawing.PixelFormat.Format1bppIndexed:
            m_PaletteData = new GR.Memory.ByteBuffer( 2 * 3 );
            break;
          case GR.Drawing.PixelFormat.Format4bppIndexed:
            m_PaletteData = new GR.Memory.ByteBuffer( 16 * 3 );
            break;
          case GR.Drawing.PixelFormat.Format8bppIndexed:
            m_PaletteData = new GR.Memory.ByteBuffer( 256 * 3 );
            break;
        }
      }

      IntPtr dc = GetDC( IntPtr.Zero );

      m_Bitmap = CreateDIBSection( dc,
                                   ref bmInfo,
                                   0, // DIB_PAL_COLORS
                                   out m_ImageData,
                                   IntPtr.Zero,
                                   0 );

      if ( m_Bitmap != IntPtr.Zero )
      {
        if ( ( m_PixelFormat == GR.Drawing.PixelFormat.Format8bppIndexed )
        ||   ( m_PixelFormat == GR.Drawing.PixelFormat.Format4bppIndexed )
        ||   ( m_PixelFormat == GR.Drawing.PixelFormat.Format1bppIndexed ) )
        {
          RebuildPalette();
        }
      }
      else if ( m_PixelFormat != GR.Drawing.PixelFormat.Undefined )
      {
        //System.Windows.Forms.MessageBox.Show( "Last error: " + Marshal.GetLastWin32Error() );
        if ( ( Width != 0 )
        &&   ( Height != 0 ) )
        {
          Debug.Log( "Failed to create bitmap!" );
        }
      }
      ReleaseDC( IntPtr.Zero, dc );
      return m_Bitmap;
    }



    public IntPtr GetDC()
    {
      IntPtr dc = CreateCompatibleDC( IntPtr.Zero );

      IntPtr oldBMP = SelectObject( dc, m_Bitmap );

      return dc;
    }



    public void ReleaseDC( IntPtr DC )
    {
      SelectObject( DC, IntPtr.Zero );
      DeleteDC( DC );
    }



    public void SetPixelData( int X, int Y, uint Value )
    {
      if ( ( X < 0 )
      ||   ( X >= m_Width )
      ||   ( Y < 0 )
      ||   ( Y >= m_Height ) )
      {
        return;
      }
      if ( m_ImageData == IntPtr.Zero )
      {
        CreateBitmap();
      }
      switch ( BitsPerPixel )
      { 
        case 1:
          unsafe
          {
            int   pitch = ( Width + 7 ) / 8;
            byte* pData = (byte*)m_ImageData;

            if ( Value != 0 )
            {
              pData[Y * pitch + X / 8] |= (byte)( 128 >> ( X % 8 ) );
            }
            else
            {
              pData[Y * pitch + X / 8] &= (byte)(~( 128 >> ( X % 8 ) ) );
            }
          }
          break;
        case 4:
          unsafe
          {
            int   pitch = Width / 2;
            byte* pData = (byte*)m_ImageData;

            if ( ( X % 2 ) == 0 )
            {
              pData[Y * pitch + X / 2] = (byte)( ( pData[Y * pitch + X / 2] & 0x0f ) | ( (byte)Value << 4 ) );
            }
            else
            {
              pData[Y * pitch + X / 2] = (byte)( ( pData[Y * pitch + X / 2] & 0xf0 ) | (byte)Value );
            }
          };
          break;
        case 8:
          unsafe
          {
            int pitch = Width;
            if ( ( pitch % 4 ) != 0 )
            {
              pitch += 4 - pitch % 4;
            }

            byte* pData = (byte*)m_ImageData;
            pData[Y * pitch + X] = (byte)Value;
          }
          break;
        case 16:
          unsafe
          {
            int pitch = Width * 2;
            if ( ( pitch % 4 ) != 0 )
            {
              pitch += 4 - pitch % 4;
            }

            ushort* pData = (ushort*)m_ImageData;
            pData[Y * pitch / 2 + X] = (ushort)Value;
          }
          break;
        case 24:
          unsafe
          {
            byte* pData = (byte*)m_ImageData;

            int pitch = Width * 3;
            if ( ( pitch % 4 ) != 0 )
            {
              pitch += 4 - pitch % 4;
            }
            pData[3 * X + Y * pitch + 0] = (byte)( Value & 0xff );
            pData[3 * X + Y * pitch + 1] = (byte)( ( Value & 0xff00 ) >> 8 );
            pData[3 * X + Y * pitch + 2] = (byte)( ( Value & 0xff0000 ) >> 16 );
          }
          break;
        case 32:
          unsafe
          {
            uint* pData = (uint*)m_ImageData;
            pData[Y * m_Width + X] = Value;
          }
          break;
        default:
          throw new NotSupportedException( "Bitdepth " + BitsPerPixel + " not supported yet" );
      }
    }



    public uint GetPixelData( int X, int Y )
    {
      if ( ( X < 0 )
      || ( X >= m_Width )
      || ( Y < 0 )
      || ( Y >= m_Height ) )
      {
        return 0;
      }
      if ( m_ImageData == IntPtr.Zero )
      {
        return 0;
      }
      switch ( BitsPerPixel )
      {
        case 4:
          unsafe
          {
            int   pitch = Width / 2;
            byte* pData = (byte*)m_ImageData;

            if ( ( X % 2 ) == 0 )
            {
              return (uint)( pData[Y * pitch + X / 2] >> 4 );
            }
            return (uint)( pData[Y * pitch + X / 2] & 0x0f );
          };
        case 8:
          unsafe
          {
            byte* pData = (byte*)m_ImageData;
            return pData[Y * BytesPerLine + X];
          }
        case 16:
          unsafe
          {
            ushort* pData = (ushort*)m_ImageData;
            return pData[Y * ( BytesPerLine / 2 ) + X];
          }
        case 24:
          unsafe
          {
            byte* pData = (byte*)m_ImageData;

            uint color = ( pData[Y * BytesPerLine + 3 * X + 0] )
                    + (uint)( ( pData[Y * BytesPerLine + 3 * X + 1] ) << 8 )
                    + (uint)( ( pData[Y * BytesPerLine + 3 * X + 2] ) << 16 );
            return color;
          }
        case 32:
          unsafe
          {
            uint* pData = (uint*)m_ImageData;
            return pData[Y * m_Width + X];
          }
        default:
          throw new NotSupportedException( "Bitdepth " + BitsPerPixel + " not supported yet" );
      }
    }



    public void SetPixel( int X, int Y, uint Color )
    {
      if ( ( X < 0 )
      ||   ( X >= m_Width )
      ||   ( Y < 0 )
      ||   ( Y >= m_Height ) )
      {
        return;
      }
      if ( m_ImageData == IntPtr.Zero )
      {
        CreateBitmap( m_PixelFormat );
      }
      unsafe
      {
        switch ( m_PixelFormat )
        {
          case GR.Drawing.PixelFormat.Format1bppIndexed:
            unsafe
            {
              int   pitch = ( Width + 7 ) / 8;
              byte* pData = (byte*)m_ImageData;

              if ( Color != 0 )
              {
                pData[Y * pitch + X / 8] |= (byte)( 128 >> ( 7 - ( X % 8 ) ) );
              }
              else
              {
                pData[Y * pitch + X / 8] &= (byte)( ~( 128 >> ( 7 - ( X % 8 ) ) ) );
              }
            }
            break;
          case GR.Drawing.PixelFormat.Format32bppRgb:
          case GR.Drawing.PixelFormat.Format32bppArgb:
            {
              uint* pData = (uint*)m_ImageData;
              pData[Y * m_Width + X] = Color;
            }
            break;
          case GR.Drawing.PixelFormat.Format16bppRgb555:
            {
              ushort* pData = (ushort*)m_ImageData;
              pData[Y * ( BytesPerLine / 2 ) + X] = (ushort)( ( ( ( Color & 0xff0000 ) >> 19 ) << 10 )
                                               + ( ( ( Color & 0x00ff00 ) >> 11 ) << 5 )
                                               + ( ( ( Color & 0x0000ff ) >> 3 ) ) );;
            }
            break;
          case GR.Drawing.PixelFormat.Format8bppIndexed:
            {
              byte* pData = (byte*)m_ImageData;
              pData[Y * BytesPerLine + X] = (byte)Color;
            }
            break;
          case GR.Drawing.PixelFormat.Format24bppRgb:
            unsafe
            {
              byte* pData = (byte*)m_ImageData;
              pData[Y * BytesPerLine + 3 * X + 0] = (byte)( Color & 0xff );
              pData[Y * BytesPerLine + 3 * X + 1] = (byte)( ( Color & 0xff00 ) >> 8 );
              pData[Y * BytesPerLine + 3 * X + 2] = (byte)( ( Color & 0xff0000 ) >> 16 );
            }
            break;
          default:
            throw new NotSupportedException();
        }
      }
    }



    public uint GetPixel( int X, int Y )
    {
      if ( ( X < 0 )
      || ( X >= m_Width )
      || ( Y < 0 )
      || ( Y >= m_Height ) )
      {
        return 0;
      }
      if ( m_ImageData == IntPtr.Zero )
      {
        CreateBitmap( m_PixelFormat );
      }
      unsafe
      {
        switch ( m_PixelFormat )
        {
          case GR.Drawing.PixelFormat.Format1bppIndexed :
            unsafe
            {
              int   pitch = ( Width + 7 ) / 8;
              byte* pData = (byte*)m_ImageData;

              return (uint)( ( pData[Y * pitch + X / 8] >> ( 7 - ( X % 8 ) ) ) & 1 );
            };
          case GR.Drawing.PixelFormat.Format4bppIndexed:
            unsafe
            {
              int   pitch = Width / 2;
              byte* pData = (byte*)m_ImageData;

              if ( ( X % 2 ) == 0 )
              {
                return (uint)( pData[Y * pitch + X / 2] >> 4 );
              }
              return (uint)( pData[Y * pitch + X / 2] & 0x0f );
            };
          case GR.Drawing.PixelFormat.Format32bppRgb:
          case GR.Drawing.PixelFormat.Format32bppArgb:
            {
              uint* pData = (uint*)m_ImageData;
              return pData[Y * m_Width + X];
            }
          case GR.Drawing.PixelFormat.Format16bppRgb555:
            {
              ushort* pData = (ushort*)m_ImageData;
              ushort  value = pData[Y * m_Width + X];
              return (uint)( ( ( ( ( value & 0x7c00 ) >> 10 ) * 255 / 31 ) << 16 )
                             + ( ( ( ( value & 0x03e0 ) >> 5 ) * 255 / 31 ) << 8 )
                             + ( ( ( ( value & 0x001f ) ) * 255 / 31 ) ) );
            }
          case GR.Drawing.PixelFormat.Format8bppIndexed:
            {
              byte* pData = (byte*)m_ImageData;
              return pData[Y * m_Width + X];
            }
          case GR.Drawing.PixelFormat.Format24bppRgb:
            unsafe
            {
              byte* pData = (byte*)m_ImageData;

              uint    color = ( pData[3 * ( Y * m_Width + X ) + 0] )
                      + (uint)( ( pData[3 * ( Y * m_Width + X ) + 1] ) << 8 )
                      + (uint)( ( pData[3 * ( Y * m_Width + X ) + 2] ) << 16 );
              return color;
            }
          default:
            throw new NotSupportedException( "GetPixel: PixelFormat currently not supported" );
        }
      }
    }



    public void CopyTo( FastImage Target )
    {
      DrawTo( Target, 0, 0 );
    }



    public int PaletteEntryCount
    {
      get
      {
        if ( m_PaletteData == null )
        {
          return 0;
        }
        return (int)m_PaletteData.Length / 3;
      }
    }



    public byte PaletteRed( int Index )
    {
      if ( m_PaletteData == null )
      {
        return 0;
      }
      if ( ( Index < 0 )
      ||   ( Index >= (int)m_PaletteData.Length / 3 ) )
      {
        return 0;
      }
      return m_PaletteData.ByteAt( Index * 3 );
    }



    public byte PaletteGreen( int Index )
    {
      if ( m_PaletteData == null )
      {
        return 0;
      }
      if ( ( Index < 0 )
      ||   ( Index >= (int)m_PaletteData.Length / 3 ) )
      {
        return 0;
      }
      return m_PaletteData.ByteAt( Index * 3 + 1 );
    }



    public byte PaletteBlue( int Index )
    {
      if ( m_PaletteData == null )
      {
        return 0;
      }
      if ( ( Index < 0 )
      ||   ( Index >= (int)m_PaletteData.Length / 3 ) )
      {
        return 0;
      }
      return m_PaletteData.ByteAt( Index * 3 + 2 );
    }



    public void SetPaletteColor( int Index, byte Red, byte Green, byte Blue )
    {
      if ( ( m_PaletteData == null )
      ||   ( Index < 0 )
      ||   ( Index >= m_PaletteData.Length / 3 ) )
      {
        return;
      }
      m_PaletteData.SetU8At( Index * 3, Red );
      m_PaletteData.SetU8At( Index * 3 + 1, Green );
      m_PaletteData.SetU8At( Index * 3 + 2, Blue );

      RebuildPalette();
    }



    static int DIBNumColors( BITMAPINFOHEADER InfoHeader )
    {
      if ( InfoHeader.biClrUsed > 0 )
      {
        return InfoHeader.biClrUsed;
      }
      switch ( InfoHeader.biBitCount )
      {
        case 1:
          return 2;
        case 4:
          return 16;
        case 8:
          return 256;
      }
      return 0;
    }



    static int DIBNumColors( byte[] dibBuffer )
    {
      BITMAPINFOHEADER infoHeader = BinaryStructConverter.FromByteArray<BITMAPINFOHEADER>( dibBuffer );

      /*  If this is a Windows-style DIB, the number of colors in the
       *  color table can be less than the number of bits per pixel
       *  allows for (i.e. lpbi->biClrUsed can be set to some value).
       *  If this is the case, return the appropriate value.
       */

      if ( infoHeader.biSize == System.Runtime.InteropServices.Marshal.SizeOf( infoHeader ) )
      {
        if ( infoHeader.biClrUsed > 0 )
        {
          return infoHeader.biClrUsed;
        }
      }

      /*  Calculate the number of colors in the color table based on
       *  the number of bits per pixel for the DIB.
       */
      int bitCount = 0;

      if ( infoHeader.biSize == System.Runtime.InteropServices.Marshal.SizeOf( infoHeader ) )
      {
        bitCount = infoHeader.biBitCount;
      }
      else
      {
        BITMAPCOREHEADER coreHeader = BinaryStructConverter.FromByteArray<BITMAPCOREHEADER>( dibBuffer );

        bitCount = coreHeader.bcBitCount;
      }

      /* return number of colors based on bits per pixel */
      switch ( bitCount )
      {
        case 1:
          return 2;
        case 4:
          return 16;
        case 8:
          return 256;
      }
      return 0;
    }


    [System.Runtime.InteropServices.StructLayout( System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1 )]
    public struct RGBQUAD
    {
      public byte rgbBlue;
      public byte rgbGreen;
      public byte rgbRed;
      public byte rgbReserved;
    }



    [System.Runtime.InteropServices.StructLayout( System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1 )]
    public struct RGBTRIPLE
    {
      public byte rgbBlue;
      public byte rgbGreen;
      public byte rgbRed;
    }



    static int PaletteSize( byte[] dibBuffer )
    {
      BITMAPINFOHEADER infoHeader = BinaryStructConverter.FromByteArray<BITMAPINFOHEADER>( dibBuffer );
      BITMAPCOREHEADER coreHeader = BinaryStructConverter.FromByteArray<BITMAPCOREHEADER>( dibBuffer );

      if ( infoHeader.biSize == System.Runtime.InteropServices.Marshal.SizeOf( infoHeader ) )
      {
        return DIBNumColors( dibBuffer ) * System.Runtime.InteropServices.Marshal.SizeOf( typeof( RGBQUAD ) );
      }
      return DIBNumColors( dibBuffer ) * System.Runtime.InteropServices.Marshal.SizeOf( typeof( RGBTRIPLE ) );
    }



    static int PaletteSize( BITMAPINFOHEADER InfoHeader )
    {
      return DIBNumColors( InfoHeader ) * System.Runtime.InteropServices.Marshal.SizeOf( typeof( RGBQUAD ) );
    }



    enum BitmapCompression
    {
      BI_RGB = 0,
      BI_RLE8 = 1,
      BI_RLE4 = 2,
      BI_BITFIELDS = 3
    }


    enum PaletteEntryFlag
    {
      PC_RESERVED = 0x01,
      PC_EXPLICIT = 0x02,
      PC_NOCOLLAPSE = 0x04
    };


    [System.Runtime.InteropServices.StructLayout( System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 2 )]
    private struct BITMAPFILEHEADER
    {
      public static readonly short BM = 0x4d42; // BM

      public short bfType;
      public int bfSize;
      public short bfReserved1;
      public short bfReserved2;
      public int bfOffBits;
    }

    [System.Runtime.InteropServices.StructLayout( System.Runtime.InteropServices.LayoutKind.Sequential )]
    private struct BITMAPCOREHEADER
    {
      public int  bcSize;
      public short bcWidth;
      public short bcHeight;
      public short bcPlanes;
      public short bcBitCount;
    }

    [System.Runtime.InteropServices.StructLayout( System.Runtime.InteropServices.LayoutKind.Sequential )]
    private struct BITMAPINFOHEADER
    {
      public int biSize;
      public int biWidth;
      public int biHeight;
      public short biPlanes;
      public short biBitCount;
      public int biCompression;
      public int biSizeImage;
      public int biXPelsPerMeter;
      public int biYPelsPerMeter;
      public int biClrUsed;
      public int biClrImportant;
    }



    private static class BinaryStructConverter
    {
      public static T FromByteArray<T>( byte[] bytes ) where T : struct
      {
        IntPtr ptr = IntPtr.Zero;
        try
        {
          int size = System.Runtime.InteropServices.Marshal.SizeOf( typeof( T ) );
          ptr = System.Runtime.InteropServices.Marshal.AllocHGlobal( size );
          System.Runtime.InteropServices.Marshal.Copy( bytes, 0, ptr, size );
          object obj = System.Runtime.InteropServices.Marshal.PtrToStructure( ptr, typeof( T ) );
          return (T)obj;
        }
        finally
        {
          if ( ptr != IntPtr.Zero )
            System.Runtime.InteropServices.Marshal.FreeHGlobal( ptr );
        }
      }

      public static byte[] ToByteArray<T>( T obj ) where T : struct
      {
        IntPtr ptr = IntPtr.Zero;
        try
        {
          int size = System.Runtime.InteropServices.Marshal.SizeOf( typeof( T ) );
          ptr = System.Runtime.InteropServices.Marshal.AllocHGlobal( size );
          System.Runtime.InteropServices.Marshal.StructureToPtr( obj, ptr, true );
          byte[] bytes = new byte[size];
          System.Runtime.InteropServices.Marshal.Copy( ptr, bytes, 0, size );
          return bytes;
        }
        finally
        {
          if ( ptr != IntPtr.Zero )
            System.Runtime.InteropServices.Marshal.FreeHGlobal( ptr );
        }
      }
    }



    public static GR.Image.FastImage CreateImageFromHDIB( object HDIB )
    {
      if ( HDIB == null )
      {
        return null;
      }

      System.IO.MemoryStream ms = System.Windows.Forms.Clipboard.GetData( "DeviceIndependentBitmap" ) as System.IO.MemoryStream;
      if ( ms == null )
      {
        return null;
      }

      GR.Image.FastImage    ImageData = new GR.Image.FastImage();

      unsafe
      {
        byte[] dibBuffer = new byte[ms.Length];
        ms.Read( dibBuffer, 0, dibBuffer.Length );

        BITMAPINFOHEADER infoHeader = BinaryStructConverter.FromByteArray<BITMAPINFOHEADER>( dibBuffer );

        int fileHeaderSize = System.Runtime.InteropServices.Marshal.SizeOf( typeof( BITMAPFILEHEADER ) );
        int infoHeaderSize = infoHeader.biSize;
        int fileSize = fileHeaderSize + infoHeader.biSize + infoHeader.biSizeImage;

        int     dataOffset = infoHeader.biSize + PaletteSize( dibBuffer );

        fixed ( byte* fixedPtr = dibBuffer )
        {
          byte*   pData = fixedPtr + infoHeader.biSize + PaletteSize( dibBuffer );

          uint[] dwMask = new uint[3];

          for ( int i = 0; i < 3; ++i )
          {
            dwMask[i] = 0;
          }

          if ( infoHeader.biCompression == (int)BitmapCompression.BI_BITFIELDS )
          {
            // dann sind noch 3 DWORDs (die Masken) mit drin
            dwMask[0] = ( (uint*)pData )[0];
            dwMask[1] = ( (uint*)pData )[1];
            dwMask[2] = ( (uint*)pData )[2];
            pData += 12;

            /*
            dh::Log( "Masks %x  %x  %x\n",
                dwMask[0], dwMask[1], dwMask[2] );
            */
          }


          switch ( infoHeader.biBitCount )
          {
            case 1:
              {
                int             iLO;

                iLO = infoHeader.biWidth / 8;
                if ( ( infoHeader.biWidth & 7 ) != 0 )
                {
                  iLO++;
                }
                if ( ( iLO % 4 ) != 0 )
                {
                  iLO += ( 4 - iLO % 4 );
                }

                int   iWidth = infoHeader.biWidth,
                      iHeight = infoHeader.biHeight;

                ImageData.Create( iWidth, iHeight, GR.Drawing.PixelFormat.Format1bppIndexed );
                for ( int j = 0; j < System.Math.Abs( iHeight ); j++ )
                {
                  for ( int i = 0; i < iWidth; i++ )
                  {
                    uint    colorValue = (uint)( ( pData[i / 8 + j * iLO] >> ( i % 8 ) ) & 1 );
                    if ( iHeight < 0 )
                    {
                      ImageData.SetPixelData( i, j, colorValue );
                    }
                    else
                    {
                      ImageData.SetPixelData( i, System.Math.Abs( iHeight ) - j - 1, colorValue );
                    }
                  }
                }

                // Palette holen
                {
                  RGBQUAD                 *bmiColor;

                  bmiColor = (RGBQUAD*)( fixedPtr + infoHeader.biSize );

                  for ( int i = 0; i < 2; i++ )
                  {
                    ImageData.SetPaletteColor( i, bmiColor[i].rgbRed, bmiColor[i].rgbGreen, bmiColor[i].rgbBlue );
                  }
                }
              }
              break;
            case 4:
              {
                int             iLO;



                iLO = infoHeader.biWidth / 2;
                if ( ( infoHeader.biWidth & 1 ) != 0 )
                {
                  iLO++;
                }
                if ( ( iLO % 4 ) != 0 )
                {
                  iLO += ( 4 - iLO % 4 );
                }

                int   iWidth = infoHeader.biWidth,
                      iHeight = infoHeader.biHeight;

                ImageData.Create( iWidth, iHeight, GR.Drawing.PixelFormat.Format4bppIndexed );
                for ( int j = 0; j < System.Math.Abs( iHeight ); j++ )
                {
                  for ( int i = 0; i < iWidth; i++ )
                  {
                    byte    value = (byte)( ( ( i % 2 ) == 0 ) ? ( pData[i / 2 + j * iLO] >> 4 ) : ( pData[i / 2 + j * iLO] & 0x0f ) );

                    if ( iHeight < 0 )
                    {
                      ImageData.SetPixelData( i, j, value );
                    }
                    else
                    {
                      ImageData.SetPixelData( i, System.Math.Abs( iHeight ) - j - 1, value );
                    }
                  }
                }

                // Palette holen
                {
                  RGBQUAD                 *bmiColor;

                  bmiColor = (RGBQUAD*)( fixedPtr + infoHeader.biSize );

                  for ( int i = 0; i < 16; i++ )
                  {
                    ImageData.SetPaletteColor( i, bmiColor[i].rgbRed, bmiColor[i].rgbGreen, bmiColor[i].rgbBlue );
                  }
                }
              }
              break;
            case 8:
              {
                int             iLO;



                iLO = infoHeader.biWidth;
                if ( ( iLO % 4 ) != 0 )
                {
                  iLO += ( 4 - iLO % 4 );
                }

                int   iWidth = infoHeader.biWidth,
                      iHeight = infoHeader.biHeight;

                ImageData.Create( iWidth, iHeight, GR.Drawing.PixelFormat.Format8bppIndexed );
                for ( int j = 0; j < System.Math.Abs( iHeight ); j++ )
                {
                  for ( int i = 0; i < iWidth; i++ )
                  {
                    if ( iHeight < 0 )
                    {
                      ImageData.SetPixelData( i, j, pData[i + j * iLO] );
                    }
                    else
                    {
                      ImageData.SetPixelData( i, System.Math.Abs( iHeight ) - j - 1, pData[i + j * iLO] );
                    }
                  }
                }

                // Palette holen
                {
                  RGBQUAD                 *bmiColor;

                  bmiColor = (RGBQUAD*)( fixedPtr + infoHeader.biSize );

                  for ( int i = 0; i < 256; i++ )
                  {
                    ImageData.SetPaletteColor( i, bmiColor[i].rgbRed, bmiColor[i].rgbGreen, bmiColor[i].rgbBlue );
                  }
                }
              }
              break;
            case 16:
              {
                int             i,
                                j,
                                iLO;



                iLO = infoHeader.biWidth * 2;
                if ( ( iLO % 4 ) != 0 )
                {
                  iLO += ( 4 - iLO % 4 );
                }

                int   iWidth = infoHeader.biWidth,
                      iHeight = infoHeader.biHeight;

                ImageData.Create( iWidth, iHeight, GR.Drawing.PixelFormat.Format16bppRgb555 );

                ushort    wDummy;

                for ( j = 0; j < System.Math.Abs( iHeight ); j++ )
                {
                  for ( i = 0; i < iWidth; i++ )
                  {
                    wDummy = ( (ushort*)pData )[i + j * iLO / 2];

                    if ( dwMask[0] == 0xf800 )
                    {
                      // von 16 auf 15 bit runter
                      wDummy = (ushort)( ( ( wDummy & 0xf800 ) >> 1 ) + ( ( wDummy & 0x7c0 ) >> 1 ) + ( wDummy & 0x1f ) );
                    }

                    if ( iHeight < 0 )
                    {
                      ImageData.SetPixelData( i, j, wDummy );
                    }
                    else
                    {
                      ImageData.SetPixelData( i, System.Math.Abs( iHeight ) - j - 1, wDummy );
                    }
                  }
                }
              }
              break;
            case 24:
              {
                int             i,
                                j,
                                iLO;



                iLO = infoHeader.biWidth * 3;
                if ( ( iLO % 4 ) != 0 )
                {
                  iLO += ( 4 - iLO % 4 );
                }

                int   iWidth = infoHeader.biWidth,
                      iHeight = infoHeader.biHeight;

                ImageData.Create( iWidth, iHeight, GR.Drawing.PixelFormat.Format24bppRgb );

                for ( j = 0; j < System.Math.Abs( iHeight ); j++ )
                {
                  for ( i = 0; i < iWidth; i++ )
                  {
                    uint    pixel = (uint)( ( pData[i * 3 + j * iLO + 0] ) + ( pData[i * 3 + j * iLO + 1] << 8 ) + ( pData[i * 3 + j * iLO + 2] << 16 ) );
                    if ( iHeight < 0 )
                    {
                      ImageData.SetPixelData( i, j, pixel );
                    }
                    else
                    {
                      ImageData.SetPixelData( i, System.Math.Abs( iHeight ) - j - 1, pixel );
                    }
                  }
                }
              }
              break;
            case 32:
              {
                int             i,
                                j,
                                iLO;



                iLO = infoHeader.biWidth * 4;

                int   iWidth = infoHeader.biWidth,
                      iHeight = infoHeader.biHeight;

                ImageData.Create( iWidth, iHeight, GR.Drawing.PixelFormat.Format32bppRgb );

                uint   dwDummy;

                for ( j = 0; j < System.Math.Abs( iHeight ); j++ )
                {
                  for ( i = 0; i < iWidth; i++ )
                  {
                    dwDummy = ( (uint*)pData )[i + j * iLO / 4];
                    if ( iHeight < 0 )
                    {
                      ImageData.SetPixelData( i, j, dwDummy );
                    }
                    else
                    {
                      ImageData.SetPixelData( i, System.Math.Abs( iHeight ) - j - 1, dwDummy );
                    }
                  }
                }
              }
              break;
            default:
              // nicht unterstützte Farbtiefe
              throw new NotSupportedException( "Unsupported HDIB bit count " + infoHeader.biBitCount );
          }
        }
      }
      return ImageData;
    }



    public IntPtr CreateHDIB()
    {
      BITMAPINFOHEADER    bi = new BITMAPINFOHEADER();
      int                 dwLen;
      IntPtr              hDIB;

      if ( ( BitsPerPixel != 1 )
      &&   ( BitsPerPixel != 2 )
      &&   ( BitsPerPixel != 4 )
      &&   ( BitsPerPixel != 8 )
      &&   ( BitsPerPixel != 15 )
      &&   ( BitsPerPixel != 16 )
      &&   ( BitsPerPixel != 24 )
      &&   ( BitsPerPixel != 32 ) )
      {
        // not supported depth
        return IntPtr.Zero;
      }

      bi.biSize     = System.Runtime.InteropServices.Marshal.SizeOf( bi );
      bi.biWidth    = Width;
      bi.biHeight   = Height;
      bi.biPlanes   = 1;
      bi.biBitCount = (short)BitsPerPixel;
      if ( bi.biBitCount == 15 )
      {
        bi.biBitCount = 16;
      }
      bi.biCompression = (int)BitmapCompression.BI_RGB;

      bi.biSizeImage = (int)( ( ( ( (uint)bi.biWidth * bi.biBitCount ) + 31 ) / 32 * 4 ) * bi.biHeight );
      bi.biXPelsPerMeter = 0;
      bi.biYPelsPerMeter = 0;
      bi.biClrUsed = 0;
      bi.biClrImportant = 0;

      // calculate size of memory block required to store BITMAPINFO
      dwLen = bi.biSize + PaletteSize( bi ) + bi.biSizeImage;

      hDIB = System.Runtime.InteropServices.Marshal.AllocHGlobal( dwLen );
      if ( hDIB == IntPtr.Zero )
      {
        // uh oh
        return IntPtr.Zero;
      }
      unsafe
      {
        // lock memory block and get pointer to it
        BITMAPINFOHEADER* lpbi = (BITMAPINFOHEADER*)GlobalLock( hDIB );

        // Daten in den Puffer kopieren
        *lpbi = bi;

        // Bild-Daten kopieren
        switch ( bi.biBitCount )
        {
          case 1:
            {
              // Palette in DC setzen
              if ( PaletteEntryCount > 0 )
              {
                RGBQUAD                 *bmiColor;

                bmiColor = (RGBQUAD*)( (byte*)lpbi + lpbi->biSize );

                for ( int i = 0; i < 2; i++ )
                {
                  bmiColor[i].rgbRed    = PaletteRed( i );
                  bmiColor[i].rgbGreen  = PaletteGreen( i );
                  bmiColor[i].rgbBlue   = PaletteBlue( i );
                  bmiColor[i].rgbReserved = 0;
                }
              }

              byte* pData = (byte*)lpbi + lpbi->biSize + PaletteSize( bi );

              int iLO = Width / 8;
              if ( ( Width & 7 ) != 0 )
              {
                iLO++;
              }
              if ( ( iLO % 4 ) != 0 )
              {
                iLO += 4 - ( iLO % 4 );
              }
              /*
              GR::Graphic::ContextDescriptor    cdImage( Image );
              GR::Graphic::ContextDescriptor    cdTarget;

              cdTarget.Attach( cdImage.Width(), cdImage.Height(), iLO, cdImage.ImageFormat(), pData );

              for ( int j = 0; j < Image.Height(); j++ )
              {
                cdTarget.HLine( 0, cdTarget.Width() - 1, j, 1 );
                cdTarget.HLine( 1, cdTarget.Width() - 2, j, 0 );
              }*/
            }
            break;
          case 4:
            {
              // Palette in DC setzen
              if ( PaletteEntryCount > 0 )
              {
                RGBQUAD*  bmiColor = (RGBQUAD*)( (byte*)lpbi + lpbi->biSize );

                for ( int i = 0; i < 16; i++ )
                {
                  bmiColor[i].rgbRed = PaletteRed( i );
                  bmiColor[i].rgbGreen  = PaletteGreen( i );
                  bmiColor[i].rgbBlue = PaletteBlue( i );
                  bmiColor[i].rgbReserved = 0;
                }
              }

              byte    *pData = (byte*)lpbi + lpbi->biSize + PaletteSize( bi );

              int iLO = Width / 2;
              if ( ( Width & 1 ) != 0 )
              {
                iLO++;
              }
              if ( ( iLO % 4 ) != 0 )
              {
                iLO += 4 - ( iLO % 4 );
              }
              /*
              GR::Graphic::ContextDescriptor    cdImage( Image );
              GR::Graphic::ContextDescriptor    cdTarget;

              cdTarget.Attach( cdImage.Width(), cdImage.Height(), iLO, cdImage.ImageFormat(), pData );

              for ( int j = 0; j < Image.Height(); j++ )
              {
                cdImage.CopyLine( 0, j, cdImage.Width(), 0, cdImage.Height() - j - 1, &cdTarget );
              }*/
            }
            break;
          case 8:
            {
              // Palette in DC setzen
              /*
              if ( PaletteEntryCount > 0 )
              {
                RGBQUAD*      bmiColor;

                bmiColor = (RGBQUAD*)( (byte*)lpbi + lpbi->biSize );

                for ( int i = 0; i < 256; i++ )
                {
                  bmiColor[i].rgbRed = PaletteRed( i );
                  bmiColor[i].rgbGreen = PaletteGreen( i );
                  bmiColor[i].rgbBlue = PaletteBlue( i );
                  bmiColor[i].rgbReserved = 0;
                }
              }*/

              byte    *pData = (byte*)lpbi + lpbi->biSize + PaletteSize( bi );

              int iLO = Width;
              if ( ( iLO % 4 ) != 0 )
              {
                iLO += 4 - ( iLO % 4 );
              }
              for ( int j = 0; j < Height; j++ )
              {
                for ( int i = 0; i < Width; i++ )
                {
                  ( (byte*)pData )[i + ( Height - j - 1 ) * iLO] = (byte)GetPixelData( i, j );
                }
              }
            }
            break;
          case 16:
            {
              byte* pData = (byte*)lpbi + lpbi->biSize + PaletteSize( bi );

              int iLO = Width * 2;
              if ( ( iLO % 4 ) != 0 )
              {
                iLO += 4 - ( iLO % 4 );
              }
              for ( int j = 0; j < Height; j++ )
              {
                for ( int i = 0; i < Width; i++ )
                {
                  ( (ushort*)pData )[i + ( Height - j - 1 ) * iLO / 2] = (ushort)GetPixelData( i, j );
                }
              }
            }
            break;
            /*
          case 24:
            {
              byte    *pData = (byte*)lpbi + lpbi->biSize + PaletteSize( bi );

              int iLO = Width * 3;
              if ( ( iLO % 4 ) != 0 )
              {
                iLO += 4 - ( iLO % 4 );
              }
              for ( int j = 0; j < Height; j++ )
              {
                for ( int i = 0; i < Width; i++ )
                {
                  ( (byte*)pData )[3 * i + ( Height - j - 1 ) * iLO] = (byte)*( (byte*)Image.Data() + 3 * ( i + j * Image.Width() ) );
                  ( (byte*)pData )[3 * i + ( Height - j - 1 ) * iLO + 1] = (byte)*( (byte*)Image.Data() + 3 * ( i + j * Image.Width() ) + 1 );
                  ( (byte*)pData )[3 * i + ( Height - j - 1 ) * iLO + 2] = (byte)*( (byte*)Image.Data() + 3 * ( i + j * Image.Width() ) + 2 );
                }
              }
            }
            break;*/
          case 32:
            {
              byte    *pData = (byte*)lpbi + lpbi->biSize + PaletteSize( bi );

              int iLO = Width;
              for ( int j = 0; j < Height; j++ )
              {
                for ( int i = 0; i < Width; i++ )
                {
                  ( (uint*)pData )[i + ( Height - j - 1 ) * iLO] = GetPixelData( i, j );
                }
              }
            }
            break;
          default:
            Debug.Log( "CreateHDIBFromImage unsupported depth " + bi.biBitCount );
            break;
        }

        //bi = *lpbi;
        GlobalUnlock( hDIB );
      }

      return hDIB;
    }



    public GR.Memory.ByteBuffer CreateHDIBAsBuffer()
    {
      GR.Memory.ByteBuffer    result = new GR.Memory.ByteBuffer();
      BITMAPINFOHEADER    bi = new BITMAPINFOHEADER();
      int                 dwLen;
      IntPtr              hDIB;

      if ( ( BitsPerPixel != 1 )
      && ( BitsPerPixel != 2 )
      && ( BitsPerPixel != 4 )
      && ( BitsPerPixel != 8 )
      && ( BitsPerPixel != 15 )
      && ( BitsPerPixel != 16 )
      && ( BitsPerPixel != 24 )
      && ( BitsPerPixel != 32 ) )
      {
        // not supported depth
        return null;
      }

      bi.biSize = System.Runtime.InteropServices.Marshal.SizeOf( bi );
      bi.biWidth = Width;
      bi.biHeight = Height;
      bi.biPlanes = 1;
      bi.biBitCount = (short)BitsPerPixel;
      if ( bi.biBitCount == 15 )
      {
        bi.biBitCount = 16;
      }
      bi.biCompression = (int)BitmapCompression.BI_RGB;

      bi.biSizeImage = (int)( ( ( ( (uint)bi.biWidth * bi.biBitCount ) + 31 ) / 32 * 4 ) * bi.biHeight );
      bi.biXPelsPerMeter = 0;
      bi.biYPelsPerMeter = 0;
      bi.biClrUsed = 0;
      bi.biClrImportant = 0;

      // calculate size of memory block required to store BITMAPINFO
      dwLen = bi.biSize + PaletteSize( bi ) + bi.biSizeImage;

      hDIB = System.Runtime.InteropServices.Marshal.AllocHGlobal( dwLen );
      if ( hDIB == IntPtr.Zero )
      {
        // uh oh
        return null;
      }
      unsafe
      {
        // lock memory block and get pointer to it
        BITMAPINFOHEADER* lpbi = (BITMAPINFOHEADER*)GlobalLock( hDIB );

        // Daten in den Puffer kopieren
        *lpbi = bi;

        // Bild-Daten kopieren
        switch ( bi.biBitCount )
        {
          case 1:
            {
              // Palette in DC setzen
              if ( PaletteEntryCount > 0 )
              {
                RGBQUAD                 *bmiColor;

                bmiColor = (RGBQUAD*)( (byte*)lpbi + lpbi->biSize );

                for ( int i = 0; i < 2; i++ )
                {
                  bmiColor[i].rgbRed = PaletteRed( i );
                  bmiColor[i].rgbGreen = PaletteGreen( i );
                  bmiColor[i].rgbBlue = PaletteBlue( i );
                  bmiColor[i].rgbReserved = 0;
                }
              }

              byte* pData = (byte*)lpbi + lpbi->biSize + PaletteSize( bi );

              int iLO = Width / 8;
              if ( ( Width & 7 ) != 0 )
              {
                iLO++;
              }
              if ( ( iLO % 4 ) != 0 )
              {
                iLO += 4 - ( iLO % 4 );
              }
              /*
              GR::Graphic::ContextDescriptor    cdImage( Image );
              GR::Graphic::ContextDescriptor    cdTarget;

              cdTarget.Attach( cdImage.Width(), cdImage.Height(), iLO, cdImage.ImageFormat(), pData );

              for ( int j = 0; j < Image.Height(); j++ )
              {
                cdTarget.HLine( 0, cdTarget.Width() - 1, j, 1 );
                cdTarget.HLine( 1, cdTarget.Width() - 2, j, 0 );
              }*/
            }
            break;
          case 4:
            {
              // Palette in DC setzen
              if ( PaletteEntryCount > 0 )
              {
                RGBQUAD*  bmiColor = (RGBQUAD*)( (byte*)lpbi + lpbi->biSize );

                for ( int i = 0; i < 16; i++ )
                {
                  bmiColor[i].rgbRed = PaletteRed( i );
                  bmiColor[i].rgbGreen = PaletteGreen( i );
                  bmiColor[i].rgbBlue = PaletteBlue( i );
                  bmiColor[i].rgbReserved = 0;
                }
              }

              byte    *pData = (byte*)lpbi + lpbi->biSize + PaletteSize( bi );

              int iLO = Width / 2;
              if ( ( Width & 1 ) != 0 )
              {
                iLO++;
              }
              if ( ( iLO % 4 ) != 0 )
              {
                iLO += 4 - ( iLO % 4 );
              }
              for ( int j = 0; j < Height; j++ )
              {
                for ( int i = 0; i < Width; i++ )
                {
                  ( (byte*)pData )[i + ( Height - j - 1 ) * iLO] = (byte)GetPixelData( i, j );
                }
              }

              /*
              GR::Graphic::ContextDescriptor    cdImage( Image );
              GR::Graphic::ContextDescriptor    cdTarget;

              cdTarget.Attach( cdImage.Width(), cdImage.Height(), iLO, cdImage.ImageFormat(), pData );

              for ( int j = 0; j < Image.Height(); j++ )
              {
                cdImage.CopyLine( 0, j, cdImage.Width(), 0, cdImage.Height() - j - 1, &cdTarget );
              }*/
            }
            break;
          case 8:
            {
              // Palette in DC setzen
              if ( PaletteEntryCount > 0 )
              {
                RGBQUAD*      bmiColor;

                bmiColor = (RGBQUAD*)( (byte*)lpbi + lpbi->biSize );

                for ( int i = 0; i < 256; i++ )
                {
                  bmiColor[i].rgbRed = PaletteRed( i );
                  bmiColor[i].rgbGreen = PaletteGreen( i );
                  bmiColor[i].rgbBlue = PaletteBlue( i );
                  bmiColor[i].rgbReserved = 0;
                }
              }

              byte    *pData = (byte*)lpbi + lpbi->biSize + PaletteSize( bi );

              int iLO = Width;
              if ( ( iLO % 4 ) != 0 )
              {
                iLO += 4 - ( iLO % 4 );
              }
              for ( int j = 0; j < Height; j++ )
              {
                for ( int i = 0; i < Width; i++ )
                {
                  ( (byte*)pData )[i + ( Height - j - 1 ) * iLO] = (byte)GetPixelData( i, j );
                }
              }
            }
            break;
          case 16:
            {
              byte* pData = (byte*)lpbi + lpbi->biSize + PaletteSize( bi );

              int iLO = Width * 2;
              if ( ( iLO % 4 ) != 0 )
              {
                iLO += 4 - ( iLO % 4 );
              }
              for ( int j = 0; j < Height; j++ )
              {
                for ( int i = 0; i < Width; i++ )
                {
                  ( (ushort*)pData )[i + ( Height - j - 1 ) * iLO / 2] = (ushort)GetPixelData( i, j );
                }
              }
            }
            break;
          /*
        case 24:
          {
            byte    *pData = (byte*)lpbi + lpbi->biSize + PaletteSize( bi );

            int iLO = Width * 3;
            if ( ( iLO % 4 ) != 0 )
            {
              iLO += 4 - ( iLO % 4 );
            }
            for ( int j = 0; j < Height; j++ )
            {
              for ( int i = 0; i < Width; i++ )
              {
                ( (byte*)pData )[3 * i + ( Height - j - 1 ) * iLO] = (byte)*( (byte*)Image.Data() + 3 * ( i + j * Image.Width() ) );
                ( (byte*)pData )[3 * i + ( Height - j - 1 ) * iLO + 1] = (byte)*( (byte*)Image.Data() + 3 * ( i + j * Image.Width() ) + 1 );
                ( (byte*)pData )[3 * i + ( Height - j - 1 ) * iLO + 2] = (byte)*( (byte*)Image.Data() + 3 * ( i + j * Image.Width() ) + 2 );
              }
            }
          }
          break;*/
          case 32:
            {
              byte    *pData = (byte*)lpbi + lpbi->biSize + PaletteSize( bi );

              int iLO = Width;
              for ( int j = 0; j < Height; j++ )
              {
                for ( int i = 0; i < Width; i++ )
                {
                  ( (uint*)pData )[i + ( Height - j - 1 ) * iLO] = GetPixelData( i, j );
                }
              }
            }
            break;
          default:
            Debug.Log( "CreateHDIBAsBuffer unsupported depth " + bi.biBitCount );
            break;
        }

        byte*   pDIBData = (byte*)lpbi;

        result.Reserve( bi.biSize + PaletteSize( bi ) + bi.biSizeImage );
        for ( int i = 0; i < bi.biSize + PaletteSize( bi ) + bi.biSizeImage; ++i )
        {
          result.AppendU8( pDIBData[i] );
        }

        //bi = *lpbi;
        GlobalUnlock( hDIB );

        System.Runtime.InteropServices.Marshal.FreeHGlobal( hDIB );
      }

      return result;
    }



    public System.Drawing.Bitmap GetAsBitmap()
    {
      if ( m_Bitmap == IntPtr.Zero )
      {
        CreateBitmap();
      }
      return System.Drawing.Bitmap.FromHbitmap( m_Bitmap );
    }



    /*
    public void DrawTo( GR.Image.IImage TargetImage, int X, int Y )
    {
      DrawTo( TargetImage, X, Y, 0, 0, Width, Height );
    }



    public void DrawTo( GR.Image.IImage TargetImage, int X, int Y, int SourceX, int SourceY, int DrawWidth, int DrawHeight )
    {
      // clip to source
      if ( ( SourceX >= Width )
      ||   ( SourceX + DrawWidth < 0 )
      ||   ( SourceY >= Height )
      ||   ( SourceY + DrawHeight < 0 ) )
      {
        return;
      }
      if ( SourceX + DrawWidth > Width )
      {
        DrawWidth = Width - SourceX;
      }
      if ( SourceX < 0 )
      {
        DrawWidth += SourceX;
        X += SourceX;
        SourceX = 0;
      }
      if ( SourceY + DrawHeight > Height )
      {
        DrawHeight = Height - SourceY;
      }
      if ( SourceY < 0 )
      {
        DrawHeight += SourceY;
        Y += SourceY;
        SourceY = 0;
      }

      int copyWidth = System.Math.Min( DrawWidth, TargetImage.Width );
      int copyHeight = System.Math.Min( DrawHeight, TargetImage.Height );

      // clip to target
      if ( ( X >= TargetImage.Width )
      ||   ( Y >= TargetImage.Height )
      ||   ( X + copyWidth < 0 )
      ||   ( Y + copyHeight < 0 ) )
      {
        return;
      }

      if ( X < 0 )
      {
        copyWidth += X;
        X = 0;
      }
      if ( X + copyWidth >= TargetImage.Width )
      {
        copyWidth = TargetImage.Width - X;
      }
      if ( Y < 0 )
      {
        copyHeight += Y;
        Y = 0;
      }
      if ( Y + copyHeight >= TargetImage.Height )
      {
        copyHeight = TargetImage.Height - Y;
      }

      if ( ( TargetImage.PixelFormat == PixelFormat )
      &&   ( BitsPerPixel >= 8 ) )
      {
        unsafe
        {
          byte*     pTargetPos = (byte*)TargetImage.m_ImageData;

          pTargetPos += TargetImage.BytesPerLine * Y + X * TargetImage.BitsPerPixel / 8;

          byte*     pSourcePos = (byte*)m_ImageData;

          pSourcePos += BytesPerLine * SourceY + SourceX * BitsPerPixel / 8;
          for ( int y = 0; y < copyHeight; ++y )
          {
            CopyMemory( new IntPtr( pTargetPos ), new IntPtr( pSourcePos ), (uint)( copyWidth * BitsPerPixel / 8 ) );

            pTargetPos += TargetImage.BytesPerLine;
            pSourcePos += BytesPerLine;
          }
        }
      }
      else
      {
        // safe (but slow) copy
        for ( int i = 0; i < copyWidth; ++i )
        {
          for ( int j = 0; j < copyHeight; ++j )
          {
            TargetImage.SetPixel( X + i, Y + j, GetPixel( SourceX + i, SourceY + j ) );
          }
        }
      }
    }
    */


    public void DrawTo( GR.Image.IImage TargetImage, int X, int Y )
    {
      DrawTo( TargetImage, X, Y, 0, 0, Width, Height );
    }



    public void DrawTo( GR.Image.IImage TargetImage, int X, int Y, int Width, int Height )
    {
      DrawTo( TargetImage, X, Y, 0, 0, Width, Height );
    }



    public void DrawTo( GR.Image.IImage TargetImage, int X, int Y, int SourceX, int SourceY, int DrawWidth, int DrawHeight )
    {
      // clip to source
      if ( ( SourceX >= Width )
      ||   ( SourceX + DrawWidth < 0 )
      ||   ( SourceY >= Height )
      ||   ( SourceY + DrawHeight < 0 ) )
      {
        return;
      }
      if ( SourceX + DrawWidth > Width )
      {
        DrawWidth = Width - SourceX;
      }
      if ( SourceX < 0 )
      {
        DrawWidth += SourceX;
        X += SourceX;
        SourceX = 0;
      }
      if ( SourceY + DrawHeight > Height )
      {
        DrawHeight = Height - SourceY;
      }
      if ( SourceY < 0 )
      {
        DrawHeight += SourceY;
        Y += SourceY;
        SourceY = 0;
      }

      int copyWidth = System.Math.Min( DrawWidth, TargetImage.Width );
      int copyHeight = System.Math.Min( DrawHeight, TargetImage.Height );

      // clip to target
      if ( ( X >= TargetImage.Width )
      || ( Y >= TargetImage.Height )
      || ( X + copyWidth < 0 )
      || ( Y + copyHeight < 0 ) )
      {
        return;
      }

      if ( X < 0 )
      {
        copyWidth += X;
        X = 0;
      }
      if ( X + copyWidth >= TargetImage.Width )
      {
        copyWidth = TargetImage.Width - X;
      }
      if ( Y < 0 )
      {
        copyHeight += Y;
        Y = 0;
      }
      if ( Y + copyHeight >= TargetImage.Height )
      {
        copyHeight = TargetImage.Height - Y;
      }

      if ( ( TargetImage.PixelFormat == PixelFormat )
      &&   ( BitsPerPixel >= 8 ) )
      {
        unsafe
        {
          byte* pTargetPos = (byte*)TargetImage.PinData();

          pTargetPos += TargetImage.BytesPerLine * Y + X * TargetImage.BitsPerPixel / 8;

          byte* pSourcePos = (byte*)m_ImageData;

          pSourcePos += BytesPerLine * SourceY + SourceX * BitsPerPixel / 8;
          for ( int y = 0; y < copyHeight; ++y )
          {
            CopyMemory( new IntPtr( pTargetPos ), new IntPtr( pSourcePos ), (uint)( copyWidth * BitsPerPixel / 8 ) );

            pTargetPos += TargetImage.BytesPerLine;
            pSourcePos += BytesPerLine;
          }
          TargetImage.UnpinData();
        }
      }
      else
      {
        // safe (but slow) copy
        for ( int i = 0; i < copyWidth; ++i )
        {
          for ( int j = 0; j < copyHeight; ++j )
          {
            TargetImage.SetPixel( X + i, Y + j, GetPixel( SourceX + i, SourceY + j ) );
          }
        }
      }
    }



    public IImage GetImage( int X, int Y, int ImageWidth, int ImageHeight )
    {
      GR.Image.FastImage subImage = new FastImage( ImageWidth, ImageHeight, m_PixelFormat );

      for ( int i = 0; i < PaletteEntryCount; ++i )
      {
        subImage.SetPaletteColor( i, PaletteRed( i ), PaletteGreen( i ), PaletteBlue( i ) );
      }

      // clip
      if ( ( X >= Width )
      ||   ( Y >= Height )
      ||   ( X + ImageWidth < 0 )
      ||   ( Y + ImageHeight < 0 ) )
      {
        return subImage;
      }
      int copyWidth   = subImage.Width;
      int copyHeight  = subImage.Height;

      if ( X < 0 )
      {
        copyWidth += X;
        X = 0;
      }
      if ( X + copyWidth >= Width )
      {
        copyWidth = Width - X;
      }
      if ( Y < 0 )
      {
        copyHeight += Y;
        Y = 0;
      }
      if ( Y + copyHeight >= Height )
      {
        copyHeight = Height - Y;
      }

      if ( ( subImage.PixelFormat == PixelFormat )
      &&   ( BitsPerPixel >= 8 ) )
      {
        unsafe
        {
          byte* pTargetPos = (byte*)subImage.m_ImageData;

          byte*     pSourcePos = (byte*)m_ImageData;
          pSourcePos += BytesPerLine * Y + X * BitsPerPixel / 8;

          for ( int y = 0; y < copyHeight; ++y )
          {
            CopyMemory( new IntPtr( pTargetPos ), new IntPtr( pSourcePos ), (uint)( copyWidth * BitsPerPixel / 8 ) );

            /*
            GR.Memory.ByteBuffer    lineData = new GR.Memory.ByteBuffer();
            for ( int i = 0; i < copyWidth * BitsPerPixel / 8; ++i )
            {
              lineData.AppendU8( pSourcePos[i] );
            }
            Debug.Log( lineData.ToString() );
             * */

            pTargetPos += subImage.BytesPerLine;
            pSourcePos += BytesPerLine;
          }
        }
      }
      else
      {
        // safe (but slow) copy
        for ( int i = 0; i < copyWidth; ++i )
        {
          for ( int j = 0; j < copyHeight; ++j )
          {
            subImage.SetPixel( X + i, Y + j, GetPixel( i, j ) );
          }
        }
      }
      return subImage;
    }



    public static FastImage FromImage( System.Drawing.Bitmap Bitmap )
    {
      GR.Image.FastImage image = new FastImage( Bitmap.Width, Bitmap.Height, (GR.Drawing.PixelFormat)Bitmap.PixelFormat );

      for ( int i = 0; i < Bitmap.Palette.Entries.Length; ++i )
      {
        image.SetPaletteColor( i, Bitmap.Palette.Entries[i].R, Bitmap.Palette.Entries[i].G, Bitmap.Palette.Entries[i].B );
      }
      System.IntPtr hbmSource = Bitmap.GetHbitmap();


      IntPtr bitmapDC = CreateCompatibleDC( IntPtr.Zero );
      IntPtr oldObject = SelectObject( bitmapDC, hbmSource );

      System.IntPtr hdcImage = image.GetDC();

      BitBlt( hdcImage, 0, 0, Bitmap.Width, Bitmap.Height, bitmapDC, 0, 0, TernaryRasterOperations.SRCCOPY );

      image.ReleaseDC( hdcImage );

      SelectObject( bitmapDC, oldObject );
      DeleteDC( bitmapDC );

      DeleteObject( hbmSource );

      return image;
    }



    public static FastImage FromImage( System.Drawing.Image Image )
    {
      System.Drawing.Bitmap bmp = new System.Drawing.Bitmap( Image );


      FastImage     img = FromImage( bmp );
      bmp.Dispose();
      return img;
    }



    public void Box( int X, int Y, int Width, int Height, uint Value )
    {
      if ( ( X + Width <= 0 )
      ||   ( X >= m_Width )
      ||   ( Y + Height <= 0 )
      ||   ( Y >= m_Height ) )
      {
        return;
      }
      if ( X < 0 )
      {
        Width += X;
        X = 0;
      }
      if ( X + Width >= m_Width )
      {
        Width = m_Width - X;
      }
      if ( Y < 0 )
      {
        Height += Y;
        Y = 0;
      }
      if ( Y + Height >= m_Height )
      {
        Height = m_Height - Y;
      }
      if ( m_ImageData == IntPtr.Zero )
      {
        CreateBitmap();
      }
      switch ( BitsPerPixel )
      {
        case 8:
          unsafe
          {
            byte* pData = (byte*)m_ImageData;
            pData += BytesPerLine * Y;

            for ( int i = 0; i < Height; ++i )
            {
              pData += X;
              for ( int j = 0; j < Width; ++j )
              {
                *pData = (byte)Value;
                ++pData;
              }
              pData += ( BytesPerLine - Width - X );
            }
          }
          break;
        case 16:
          unsafe
          {
            ushort* pData = (ushort*)m_ImageData;

            pData += BytesPerLine / 2 * Y;

            for ( int i = 0; i < Height; ++i )
            {
              pData += X;
              for ( int j = 0; j < Width; ++j )
              {
                *pData = (ushort)Value;
                ++pData;
              }
              pData += ( BytesPerLine / 2 - Width - X );
            }
          }
          break;
        case 24:
          unsafe
          {
            byte* pData = (byte*)m_ImageData;
            pData += BytesPerLine * Y;

            for ( int i = 0; i < Height; ++i )
            {
              pData += X * 3;
              for ( int j = 0; j < Width; ++j )
              {
                pData[0] = (byte)( Value & 0xff );
                pData[1] = (byte)( ( Value & 0xff00 ) >> 8 );
                pData[2] = (byte)( ( Value & 0xff0000 ) >> 16 );
                pData += 3;
              }
              pData += ( BytesPerLine - 3 * ( Width + X ) );
            }
          }
          break;
        case 32:
          unsafe
          {
            uint* pData = (uint*)m_ImageData;

            pData += BytesPerLine / 4 * Y;

            for ( int i = 0; i < Height; ++i )
            {
              pData += X;
              for ( int j = 0; j < Width; ++j )
              {
                *pData = Value;
                ++pData;
              }
              pData += ( BytesPerLine / 4 - Width - X );
            }
          }
          break;
        default:
          // defaul to safe but slow
          for ( int i = 0; i < Height; ++i )
          {
            for ( int j = 0; j < Width; ++j )
            {
              SetPixelData( i, j, Value );
            }
          }
          break;
      }
    }



    public void Rectangle( int X, int Y, int Width, int Height, uint Value )
    {
      for ( int i = 0; i < Width; ++i )
      {
        SetPixel( X + i, Y, Value );
        SetPixel( X + i, Y + Height - 1, Value );
      }
      for ( int i = 1; i < Height - 1; ++i )
      {
        SetPixel( X, Y + i, Value );
        SetPixel( X + Width - 1, Y + i, Value );
      }
    }



    public uint PaletteColor( int Index )
    {
      if ( ( m_PaletteData == null )
      ||   ( Index < 0 )
      ||   ( Index * 3 >= m_PaletteData.Length ) )
      {
        return 0;
      }

      return (uint)( ( m_PaletteData.ByteAt( Index * 3 ) << 16 )
        + ( m_PaletteData.ByteAt( Index * 3 + 1 ) << 8 )
        + m_PaletteData.ByteAt( Index * 3 + 2 ) );
    }



  }
}
