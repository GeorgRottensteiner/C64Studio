using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif



namespace GR.Image
{
  public interface IDPIHandlerResize
  {
    void ResizeControl();
  }

#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
#endif
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
        { 
          continue; 
        }

        var button = control as ButtonBase;
        if ( button != null )
        {
          button.AdjustControlsThroughDPI( list );
          continue;
        }

        // Here more controls than button can be adjusted if needed...

        // Recursive
        var nestedControls = control.Controls;
        if ( nestedControls.Count == 0 )
        { 
          continue; 
        }
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



    public static void ResizeControlsForDPI( Control Control )
    {
      if ( DPIRatioIsOne )
      {
        return;
      }

      Control.SuspendLayout();

      if ( Control is IDPIHandlerResize )
      {
        var dpiHandler = Control as IDPIHandlerResize;

        dpiHandler.ResizeControl();
      }
      else
      {
        AdjustControlSize( Control );
        AdjustControlChildsSize( Control );
        
      }
      Control.ResumeLayout();
    }



    public static void AdjustControlChildsSize( Control Control )
    {
      foreach ( Control control in Control.Controls )
      {
        ResizeControlsForDPI( control );
      }

      if ( Control is ToolStrip )
      {
        var ts = Control as ToolStrip;

        foreach ( ToolStripItem item in ts.Items )
        {
          ResizeControlsForDPI( item );
        }
      }
    }



    public static void AdjustControlSize( Control Control )
    {
      int     newWidth  = (int)( Control.Width * DPIX / 96.0f + 0.5f );
      int     newHeight = (int)( Control.Height * DPIY / 96.0f + 0.5f );
      int     newX      = (int)( Control.Location.X * DPIX / 96.0f + 0.5f );
      int     newY      = (int)( Control.Location.Y * DPIY / 96.0f + 0.5f );

      Control.SetBounds( newX, newY, newWidth, newHeight );

      if ( Control is ListBox )
      {
        var lb = Control as ListBox;
        lb.ItemHeight = (int)( lb.ItemHeight * DPIY / 96.0f + 0.5f );
      }
      if ( Control is ToolStrip )
      {
        var ts = Control as ToolStrip;
        ts.ImageScalingSize = new Size( (int)( ts.ImageScalingSize.Width * DPIY / 96.0f ), (int)( ts.ImageScalingSize.Height * DPIY / 96.0f ) );
      }
    }



    private static void ResizeControlsForDPI( ToolStripItem Item )
    {
      if ( DPIRatioIsOne )
      {
        return;
      }

      if ( Item is ToolStripMenuItem )
      {
        var ts = Item as ToolStripMenuItem;

        foreach ( ToolStripItem item in ts.DropDownItems )
        {
          ResizeControlsForDPI( item );
        }
      }

      // change parent later so childs are properly handled during OnSize
      int     newWidth  = (int)( Item.Width * DPIX / 96.0f + 0.5f );
      int     newHeight = (int)( Item.Height * DPIY / 96.0f + 0.5f );

      Item.Size = new Size( newWidth, newHeight );
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
      if ( imageIn == null )
      {
        return null;
      }
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
