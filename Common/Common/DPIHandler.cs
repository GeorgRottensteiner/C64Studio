using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GR.Image
{
  public static class DPIHandler
  {
    public static bool DPIRatioIsOne = true;
    public static float DPIX = 96.0f;
    public static float DPIY = 96.0f;



    static DPIHandler()
    {
      Graphics g = Graphics.FromHwnd( IntPtr.Zero );
      IntPtr desktop = g.GetHdc();

      DPIX = (float)GetDeviceCaps( desktop, (int)DeviceCap.LOGPIXELSX );
      DPIY = (float)GetDeviceCaps( desktop, (int)DeviceCap.LOGPIXELSY );

      DPIRatioIsOne = ( DPIX == 96 );
    }



    [DllImport( "gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true )]
    public static extern int GetDeviceCaps( IntPtr hDC, int nIndex );

    public enum DeviceCap
    {
      /// <summary>
      /// Logical pixels inch in X
      /// </summary>
      LOGPIXELSX = 88,
      /// <summary>
      /// Logical pixels inch in Y
      /// </summary>
      LOGPIXELSY = 90

      // Other constants may be founded on pinvoke.net
    }



    public static IEnumerable<IDisposable> AdjustControlsThroughDPI( this Control.ControlCollection controls )
    {
      if ( DPIRatioIsOne )
      {
        return new IDisposable[0]; // No need to adjust on DPI One
      }

      var list = new List<IDisposable>();
      foreach ( Control control in controls )
      {
        if ( control == null )
        { continue; }

        var button = control as ButtonBase;
        if ( button != null )
        {
          button.AdjustControlsThroughDPI( list );
          continue;
        }

        // Here more controls tahn button can be adjusted if needed...

        // Recursive
        var nestedControls = control.Controls;
        if ( nestedControls.Count == 0 )
        { continue; }
        var disposables = nestedControls.AdjustControlsThroughDPI();
        list.AddRange( disposables );
      }
      return list;
    }



    public static void AdjustForDPI( this ButtonBase button )
    {
      var image = button.Image;
      if ( image == null )
      {
        return;
      }

      var imageStretched = image.GetImageStretchedDPI();
      button.Image = imageStretched;
    }



    private static void AdjustControlsThroughDPI( this ButtonBase button, IList<IDisposable> list )
    {
      var image = button.Image;
      if ( image == null )
      {
        return;
      }

      var imageStretched = image.GetImageStretchedDPI();
      button.Image = imageStretched;
      list.Add( imageStretched );
    }



    public static System.Drawing.Image GetImageStretchedDPI( this System.Drawing.Image imageIn )
    {
      var newWidth  = (int)( imageIn.Width * DPIX / 96.0f );
      var newHeight = (int)( imageIn.Height * DPIY / 96.0f );
      var newBitmap = new Bitmap( newWidth, newHeight );

      using ( var g = Graphics.FromImage( newBitmap ) )
      {
        // According to this blog post http://blogs.msdn.com/b/visualstudio/archive/2014/03/19/improving-high-dpi-support-for-visual-studio-2013.aspx
        // NearestNeighbor is more adapted for 200% and 200%+ DPI
        var interpolationMode = InterpolationMode.HighQualityBicubic;
        if ( DPIX >= 2.0f )
        {
          interpolationMode = InterpolationMode.NearestNeighbor;
        }
        g.InterpolationMode = interpolationMode;
        g.DrawImage( imageIn, new Rectangle( 0, 0, newWidth, newHeight ) );
      }

      imageIn.Dispose();
      return newBitmap;
    }
  }



}
