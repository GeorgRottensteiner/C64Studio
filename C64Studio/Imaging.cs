using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
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

      if ( ( extension == "KOA" )
      ||   ( extension == "KLA" ) )
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
  }
}
