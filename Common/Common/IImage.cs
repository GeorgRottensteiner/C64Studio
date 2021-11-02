using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Image
{
  public interface IImage : IDisposable
  {
    void SetPixel( int X, int Y, uint Color );
    uint GetPixel( int X, int Y );

    void Box( int X, int Y, int Width, int Height, uint Color );
    void Rectangle( int X, int Y, int Width, int Height, uint Color );
    void Line( int X1, int Y1, int X2, int Y2, uint Color );

    void DrawTo( IImage TargetImage, int X, int Y );
    void DrawTo( IImage TargetImage, int X, int Y, int Width, int Height );
    void DrawTo( IImage TargetImage, int X, int Y, int SourceX, int SourceY, int DrawWidth, int DrawHeight );

    IntPtr PinData();
    void UnpinData();


    int BitsPerPixel
    {
      get;
    }



    int Width
    {
      get;
    }



    int Height
    {
      get;
    }



    System.Drawing.Imaging.PixelFormat PixelFormat
    {
      get;
    }



    int BytesPerLine
    {
      get;
    }



    IImage GetImage( int X, int Y, int ImageWidth, int ImageHeight );

    GR.Memory.ByteBuffer CreateHDIBAsBuffer();

    void SetPaletteColor( int Index, byte Red, byte Green, byte Blue );
    uint PaletteColor( int Index );

  }
}
