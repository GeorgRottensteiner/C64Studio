using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using GR.Image;
using WeifenLuo.WinFormsUI.Docking;


namespace C64Studio
{
  public class Imaging
  {
    StudioCore        Core;



    public Imaging( StudioCore Core )
    {
      this.Core = Core;
    }



    internal GR.Image.FastImage LoadImageFromFile( string Filename )
    {
      string                extension = System.IO.Path.GetExtension( Filename ).ToUpper();
      GR.Image.FastImage    newImage;

      if ( ( extension == ".KOA" )
      ||   ( extension == ".KLA" ) )
      {
        var koalaImage = C64Studio.Converter.KoalaToBitmap.BitmapFromKoala( Filename );
        var bitmap = koalaImage.GetAsBitmap();

        newImage = GR.Image.FastImage.FromImage( bitmap );

        bitmap.Dispose();
      }
      else
      {
        System.Drawing.Bitmap bmpImage = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile( Filename );
        newImage = GR.Image.FastImage.FromImage( bmpImage );
        bmpImage.Dispose();
      }

      return newImage;
    }



    internal void ImageToClipboard( GR.Image.IImage Image )
    {
      if ( Image == null )
      {
        return;
      }
      ImageToClipboard( Image, 0, 0, Image.Width, Image.Height );
    }



    internal void ImageToClipboard( IImage Image, int X, int Y, int Width, int Height )
    {
      if ( Image == null )
      {
        return;
      }
      if ( ( X >= Image.Width )
      ||   ( Width <= 0 )
      ||   ( Height <= 0 )
      ||   ( X + Width < X )
      ||   ( Y >= Image.Height )
      ||   ( Y + Height < Y ) )
      {
        return;
      }
      GR.Memory.ByteBuffer      dibData2 = Image.GetImage( X, Y, Width, Height ).CreateHDIBAsBuffer();

      System.IO.MemoryStream    ms2 = dibData2.MemoryStream();

      System.Windows.Forms.Clipboard.SetData( "DeviceIndependentBitmap", ms2 );
    }



    internal void ImageToClipboard( MemoryImage Image, Rectangle Selection )
    {
      if ( ( Image == null )
      ||   ( Selection == null ) )
      {
        return;
      }
      ImageToClipboard( Image, Selection.X, Selection.Y, Selection.Width, Selection.Height );
    }



  }
}
