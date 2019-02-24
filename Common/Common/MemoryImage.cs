using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace GR.Image
{
  public class MemoryImage : IImage
  {
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

    [DllImport( "Kernel32.dll", EntryPoint = "CopyMemory" )]
    static extern void CopyMemory( IntPtr dest, IntPtr src, uint length );
    [DllImport( "kernel32.dll" )]
    static extern IntPtr GlobalLock( IntPtr hMem );
    [DllImport( "kernel32.dll" )]
    [return: MarshalAs( UnmanagedType.Bool )]
    static extern bool GlobalUnlock( IntPtr hMem );

    private GR.Memory.ByteBuffer                  m_ImageData = new GR.Memory.ByteBuffer();
    private int                                   m_Width = 0;
    private int                                   m_Height = 0;
    private System.Drawing.Imaging.PixelFormat    m_PixelFormat = System.Drawing.Imaging.PixelFormat.Undefined;
    private GR.Memory.ByteBuffer                  m_PaletteData = new GR.Memory.ByteBuffer();



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



    public System.Drawing.Imaging.PixelFormat PixelFormat
    {
      get
      {
        return m_PixelFormat;
      }
    }



    public IntPtr PinData()
    {
      return m_ImageData.PinData();
    }



    public void UnpinData()
    {
      m_ImageData.UnpinData();
    }



    public MemoryImage( int Width, int Height, System.Drawing.Imaging.PixelFormat PixelFormat )
    {
      Create( Width, Height, PixelFormat );
    }



    public MemoryImage( MemoryImage SourceImage )
    {
      Create( SourceImage.Width, SourceImage.Height, SourceImage.PixelFormat );

      for ( int i = 0; i < PaletteEntryCount; ++i )
      {
        SetPaletteColor( i, SourceImage.PaletteRed( i ), SourceImage.PaletteGreen( i ), SourceImage.PaletteBlue( i ) );
      }
    }



    public void Clear()
    {
      m_PixelFormat = System.Drawing.Imaging.PixelFormat.Undefined;
      m_Width = 0;
      m_Height = 0;
      m_PaletteData.Clear();
    }



    public void Create( int Width, int Height, System.Drawing.Imaging.PixelFormat PixelFormat )
    {
      Clear();

      m_Width       = Width;
      m_Height      = Height;
      m_PixelFormat = PixelFormat;

      int   bytesPerLine = BitsPerPixel * Width / 8;
      m_ImageData.Resize( (uint)( bytesPerLine * Height ) );

      switch ( m_PixelFormat )
      {
        case System.Drawing.Imaging.PixelFormat.Format1bppIndexed:
          m_PaletteData = new GR.Memory.ByteBuffer( 2 * 3 );
          break;
        case System.Drawing.Imaging.PixelFormat.Format4bppIndexed:
          m_PaletteData = new GR.Memory.ByteBuffer( 16 * 3 );
          break;
        case System.Drawing.Imaging.PixelFormat.Format8bppIndexed:
          m_PaletteData = new GR.Memory.ByteBuffer( 256 * 3 );
          break;
        case System.Drawing.Imaging.PixelFormat.Format16bppRgb555:
        case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
        case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
        case System.Drawing.Imaging.PixelFormat.Format32bppRgb:
          break;
        default:
          throw new NotSupportedException( "Pixelformat " + PixelFormat + " not supported" );
      }
    }



    public void Box( int X, int Y, int Width, int Height, uint Value )
    {
      if ( ( X + Width <= 0 )
      || ( X >= m_Width )
      || ( Y + Height <= 0 )
      || ( Y >= m_Height ) )
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
      switch ( BitsPerPixel )
      {
        case 8:
          for ( int i = 0; i < Height; ++i )
          {
            for ( int j = 0; j < Width; ++j )
            {
              m_ImageData.SetU8At( j + X + BytesPerLine * ( Y + i ), (byte)Value );
            }
          }
          break;
        case 24:
          for ( int i = 0; i < Height; ++i )
          {
            for ( int j = 0; j < Width; ++j )
            {
              m_ImageData.SetU8At( ( j + X ) * 3 + BytesPerLine * ( Y + i ), (byte)( Value & 0xff ) );
              m_ImageData.SetU8At( ( j + X ) * 3 + BytesPerLine * ( Y + i ) + 1, (byte)( ( Value & 0xff00 ) >> 8 ) );
              m_ImageData.SetU8At( ( j + X ) * 3 + BytesPerLine * ( Y + i ) + 2, (byte)( ( Value & 0xff0000 ) >> 16 ) );
            }
          }
          break;
        case 32:
          for ( int i = 0; i < Height; ++i )
          {
            for ( int j = 0; j < Width; ++j )
            {
              m_ImageData.SetU32At( ( j + X ) * 4 + BytesPerLine * ( Y + i ), Value );
            }
          }
          break;
        default:
          throw new NotSupportedException( "Bitdepth " + BitsPerPixel + " not supported yet" );
      }
    }



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

      int copyWidth   = DrawWidth;
      int copyHeight  = DrawHeight;

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
        SourceX -= X;
        copyWidth += X;
        X = 0;
      }
      if ( X + copyWidth >= TargetImage.Width )
      {
        copyWidth = TargetImage.Width - X;
      }
      if ( Y < 0 )
      {
        SourceY -= Y;
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
          byte*     pTargetPos = (byte*)TargetImage.PinData();

          pTargetPos += TargetImage.BytesPerLine * Y + X * TargetImage.BitsPerPixel / 8;

          byte*     pSourcePos = (byte*)PinData();

          pSourcePos += BytesPerLine * SourceY + SourceX * BitsPerPixel / 8;
          for ( int y = 0; y < copyHeight; ++y )
          {
            CopyMemory( new IntPtr( pTargetPos ), new IntPtr( pSourcePos ), (uint)( copyWidth * BitsPerPixel / 8 ) );

            pTargetPos += TargetImage.BytesPerLine;
            pSourcePos += BytesPerLine;
          }
          TargetImage.UnpinData();
          UnpinData();
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

    
    
    public int BitsPerPixel
    {
      get
      {
        switch ( m_PixelFormat )
        {
          case System.Drawing.Imaging.PixelFormat.Format8bppIndexed:
            return 8;
          case System.Drawing.Imaging.PixelFormat.Format4bppIndexed:
            return 4;
          case System.Drawing.Imaging.PixelFormat.Format16bppArgb1555:
          case System.Drawing.Imaging.PixelFormat.Format16bppGrayScale:
          case System.Drawing.Imaging.PixelFormat.Format16bppRgb555:
          case System.Drawing.Imaging.PixelFormat.Format16bppRgb565:
            return 16;
          case System.Drawing.Imaging.PixelFormat.Format1bppIndexed:
            return 1;
          case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
            return 24;
          case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
          case System.Drawing.Imaging.PixelFormat.Format32bppPArgb:
          case System.Drawing.Imaging.PixelFormat.Format32bppRgb:
            return 32;
        }
        return 0;
      }
    }



    public int BytesPerLine
    {
      get
      {
        switch ( m_PixelFormat )
        {
          case System.Drawing.Imaging.PixelFormat.Format8bppIndexed:
            return Width;
          case System.Drawing.Imaging.PixelFormat.Format4bppIndexed:
            if ( ( Width % 4 ) != 0 )
            {
              return Width / 4 + 1;
            }
            return Width / 4;
          case System.Drawing.Imaging.PixelFormat.Format16bppArgb1555:
          case System.Drawing.Imaging.PixelFormat.Format16bppGrayScale:
          case System.Drawing.Imaging.PixelFormat.Format16bppRgb555:
          case System.Drawing.Imaging.PixelFormat.Format16bppRgb565:
            return Width * 2;
          case System.Drawing.Imaging.PixelFormat.Format1bppIndexed:
            if ( ( Width % 8 ) != 0 )
            {
              return Width / 8 + 1;
            }
            return Width / 8;
          case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
            return Width * 3;
          case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
          case System.Drawing.Imaging.PixelFormat.Format32bppPArgb:
          case System.Drawing.Imaging.PixelFormat.Format32bppRgb:
            return Width * 4;
        }
        return 0;
      }
    }

    
    
    public void SetPixel( int X, int Y, uint Value )
    {
      if ( ( X < 0 )
      ||   ( X >= m_Width )
      ||   ( Y < 0 )
      ||   ( Y >= m_Height ) )
      {
        return;
      }
      if ( m_ImageData.Empty() )
      {
        return;
      }
      switch ( BitsPerPixel )
      {
        case 1:
          unsafe
          {
            int   pitch = ( Width + 7 ) / 8;
            byte  origValue = m_ImageData.ByteAt( Y * pitch + X / 8 );
            byte  newValue = origValue;

            if ( Value != 0 )
            {
              newValue = (byte)( origValue | ( 128 >> ( X % 8 ) ) );
            }
            else
            {
              newValue = (byte)( origValue & ( ~( 128 >> ( X % 8 ) ) ) );
            }
            m_ImageData.SetU8At( Y * pitch + X / 8, newValue );
          }
          break;
        case 4:
          unsafe
          {
            int   pitch = Width / 2;

            byte  origValue = m_ImageData.ByteAt( Y * pitch + X / 2 );
            byte  newValue = 0;

            if ( ( X % 2 ) == 0 )
            {
              newValue = (byte)( ( origValue & 0x0f ) | ( (byte)Value << 4 ) );
            }
            else
            {
              newValue = (byte)( ( origValue & 0xf0 ) | (byte)Value );
            }

            m_ImageData.SetU8At( Y * pitch + X / 2, newValue );
          };
          break;
        case 8:
          m_ImageData.SetU8At( Y * m_Width + X, (byte)Value );
          break;
        case 16:
          m_ImageData.SetU16At( 2 * ( Y * m_Width + X ),
                                (ushort)( ( ( ( Value & 0xff0000 ) >> 19 ) << 10 )
                                        + ( ( ( Value & 0x00ff00 ) >> 11 ) << 5 )
                                        + ( ( ( Value & 0x0000ff ) >> 3 ) ) ) );
          break;
        case 24:
          m_ImageData.SetU8At( 3 * ( Y * m_Width + X ) + 0, (byte)( Value & 0xff ) );
          m_ImageData.SetU8At( 3 * ( Y * m_Width + X ) + 1, (byte)( ( Value & 0xff00 ) >> 8 ) );
          m_ImageData.SetU8At( 3 * ( Y * m_Width + X ) + 2, (byte)( ( Value & 0xff0000 ) >> 16 ) );          
          break;
        case 32:
          m_ImageData.SetU32At( 4 * ( Y * m_Width + X ), Value );
          break;
        default:
          throw new NotSupportedException( "Bitdepth " + BitsPerPixel + " not supported yet" );
      }
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
      || ( Index >= (int)m_PaletteData.Length / 3 ) )
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
      || ( Index >= (int)m_PaletteData.Length / 3 ) )
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
      || ( Index >= (int)m_PaletteData.Length / 3 ) )
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
    }



    public uint GetPixel( int X, int Y )
    {
      if ( ( X < 0 )
      ||   ( X >= m_Width )
      ||   ( Y < 0 )
      ||   ( Y >= m_Height ) )
      {
        return 0;
      }
      uint    resultValue = 0;
      unsafe
      {
        byte*   pDataSource = (byte*)PinData();
        switch ( m_PixelFormat )
        {
          case System.Drawing.Imaging.PixelFormat.Format1bppIndexed:
            unsafe
            {
              int   pitch = ( Width + 7 ) / 8;

              return (uint)( ( pDataSource[Y * pitch + X / 8] >> ( X % 8 ) ) & 1 );
            };
          case System.Drawing.Imaging.PixelFormat.Format4bppIndexed:
            {
              int   pitch = Width / 2;

              if ( ( X % 2 ) == 0 )
              {
                resultValue = (uint)( pDataSource[Y * pitch + X / 2] >> 4 );
              }
              else
              {
                resultValue = (uint)( pDataSource[Y * pitch + X / 2] & 0x0f );
              }
            }
            break;
          case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
            {
              resultValue = (uint)( pDataSource[3 * ( Y * m_Width + X )] )
                         +  (uint)( pDataSource[3 * ( Y * m_Width + X ) + 1] << 8 )
                         +  (uint)( pDataSource[3 * ( Y * m_Width + X ) + 2] << 16 );

            }
            break;
          case System.Drawing.Imaging.PixelFormat.Format16bppRgb555:
            {
              ushort* pData = (ushort*)pDataSource;
              resultValue = pData[Y * m_Width + X];
              resultValue = (uint)( ( ( ( ( resultValue & 0x7c00 ) >> 10 ) * 255 / 31 ) << 16 )
                                  + ( ( ( ( resultValue & 0x03e0 ) >> 5 ) * 255 / 31 ) << 8 )
                                  + ( ( ( ( resultValue & 0x001f ) ) * 255 / 31 ) ) );
            }
            break;
          case System.Drawing.Imaging.PixelFormat.Format32bppRgb:
          case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
            {
              uint* pData = (uint*)pDataSource;
              resultValue = pData[Y * m_Width + X];
            }
            break;
          case System.Drawing.Imaging.PixelFormat.Format8bppIndexed:
            {
              resultValue = pDataSource[Y * m_Width + X];
            }
            break;
          default:
            UnpinData();
            throw new NotSupportedException( "GetPixel: PixelFormat currently not supported" );
        }
        UnpinData();
      }
      return resultValue;
    }



    public MemoryImage GetImage( int X, int Y, int ImageWidth, int ImageHeight )
    {
      GR.Image.MemoryImage subImage = new MemoryImage( ImageWidth, ImageHeight, m_PixelFormat );

      for ( int i = 0; i < PaletteEntryCount; ++i )
      {
        subImage.SetPaletteColor( i, PaletteRed( i ), PaletteGreen( i ), PaletteBlue( i ) );
      }

      // clip
      if ( ( X >= Width )
      || ( Y >= Height )
      || ( X + ImageWidth < 0 )
      || ( Y + ImageHeight < 0 ) )
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
      && ( BitsPerPixel >= 8 ) )
      {
        unsafe
        {
          byte* pTargetPos = (byte*)subImage.PinData();

          byte*     pSourcePos = (byte*)PinData();
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
          UnpinData();
          subImage.UnpinData();
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
                  ( (byte*)pData )[i + ( Height - j - 1 ) * iLO] = (byte)GetPixel( i, j );
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
                  ( (byte*)pData )[i + ( Height - j - 1 ) * iLO] = (byte)GetPixel( i, j );
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
                  ( (ushort*)pData )[i + ( Height - j - 1 ) * iLO / 2] = (ushort)GetPixel( i, j );
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
                  ( (uint*)pData )[i + ( Height - j - 1 ) * iLO] = GetPixel( i, j );
                }
              }
            }
            break;
          default:
            Debug.Log( "CreateHDIBAsBuffer unsupported depth " + bi.biBitCount );
            break;
        }

        byte*   pDIBData = (byte*)lpbi;
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



    public GR.Memory.ByteBuffer GetAsData()
    {
      GR.Memory.ByteBuffer data = new GR.Memory.ByteBuffer( m_ImageData.Data() );
      return data;
    }



    public bool SetData( GR.Memory.ByteBuffer ImageData )
    {
      if ( ImageData.Length != m_ImageData.Length )
      {
        return false;
      }
      ImageData.CopyTo( m_ImageData );
      return true;
    }



    public System.Drawing.Bitmap GetAsBitmap()
    {
      GR.Image.FastImage    fastImage = new FastImage( Width, Height, PixelFormat );

      for ( int i = 0; i < PaletteEntryCount; ++i )
      {
        fastImage.SetPaletteColor( i, PaletteRed( i ), PaletteGreen( i ), PaletteBlue( i ) );
      }

      for ( int x = 0; x < Width; ++x )
      {
        for ( int y = 0; y < Height; ++y )
        {
          fastImage.SetPixel( x, y, GetPixel( x, y ) );
        }
      }
      System.Drawing.Bitmap   bitmap = fastImage.GetAsBitmap();

      fastImage.Dispose();
      
      return bitmap;
    }



    public GR.Memory.ByteBuffer ToBuffer()
    {
      GR.Memory.ByteBuffer    result = new GR.Memory.ByteBuffer();

      result.AppendI32( Width );
      result.AppendI32( Height );
      result.AppendU32( (uint)PixelFormat );

      result.AppendI32( PaletteEntryCount );
      for ( int i = 0; i < PaletteEntryCount; ++i )
      {
        result.AppendU8( PaletteRed( i ) );
        result.AppendU8( PaletteGreen( i ) );
        result.AppendU8( PaletteBlue( i ) );
      }
      result.Append( m_ImageData );
      return result;
    }



    public void FromBuffer( GR.Memory.ByteBuffer Data )
    {
      if ( Data == null )
      {
        return;
      }
      GR.IO.MemoryReader    memIn = Data.MemoryReader();

      int  w = memIn.ReadInt32();
      int  h = memIn.ReadInt32();
      System.Drawing.Imaging.PixelFormat pf = (System.Drawing.Imaging.PixelFormat)memIn.ReadUInt32();

      Create( w, h, pf );

      int     numEntries = memIn.ReadInt32();
      for ( int i = 0; i < numEntries; ++i )
      {
        byte  r = memIn.ReadUInt8();
        byte  g = memIn.ReadUInt8();
        byte  b = memIn.ReadUInt8();

        SetPaletteColor( i, r, g, b );
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



    public void Rectangle( int X, int Y, int Width, int Height, uint Value )
    {
      Box( X, Y, Width, 1, Value );
      Box( X, Y + Height - 1, Width, 1, Value );
      for ( int i = 1; i < Height - 1; ++i )
      {
        SetPixel( X, Y + i, Value );
        SetPixel( X + Width - 1, Y + i, Value );
      }
    }

  }
}
