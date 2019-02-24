using System;
using System.Collections.Generic;
using System.Text;

namespace GR.Image
{
  public interface IImage
  {
    void SetPixel( int X, int Y, uint Color );

    void Box( int X, int Y, int Width, int Height, uint Color );
    void Rectangle( int X, int Y, int Width, int Height, uint Color );
    void Line( int X1, int Y1, int X2, int Y2, uint Color );

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






  }
}
