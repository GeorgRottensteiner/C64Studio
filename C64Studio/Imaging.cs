using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using GR.Image;
using WeifenLuo.WinFormsUI.Docking;


namespace RetroDevStudio
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
        var koalaImage = RetroDevStudio.Converter.KoalaToBitmap.BitmapFromKoala( Filename );
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



    internal void ImageToClipboardData( IImage Image, int X, int Y, int Width, int Height, System.Windows.Forms.DataObject Data )
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

      Data.SetData( "DeviceIndependentBitmap", ms2 );
    }



    internal void ImageToClipboard( MemoryImage Image, Rectangle Selection )
    {
      if ( ( Image == null )
      ||   ( Selection == Rectangle.Empty ) )
      {
        return;
      }
      ImageToClipboard( Image, Selection.X, Selection.Y, Selection.Width, Selection.Height );
    }



    internal void FloodFill( IImage Image, int X, int Y, byte FillColor )
    {
      if ( Image == null )
      {
        return;
      }

      if ( ( X < 0 )
      ||   ( X >= Image.Width )
      ||   ( Y < 0 )
      ||   ( Y >= Image.Height ) )
      {
        return;
      }

      uint targetColor = Image.GetPixel( X, Y );
      if ( targetColor == FillColor )
      {
        return;
      }

      Stack<Point> pixels = new Stack<Point>();

      pixels.Push( new Point( X, Y ) );
      while ( pixels.Count != 0 )
      {
        Point temp = pixels.Pop();
        int y1 = temp.Y;
        while ( y1 >= 0 && Image.GetPixel( temp.X, y1 ) == targetColor )
        {
          y1--;
        }
        y1++;
        bool spanLeft = false;
        bool spanRight = false;
        while ( y1 < Image.Height && Image.GetPixel( temp.X, y1 ) == targetColor )
        {
          Image.SetPixel( temp.X, y1, FillColor );

          if ( !spanLeft && temp.X > 0 && Image.GetPixel( temp.X - 1, y1 ) == targetColor )
          {
            pixels.Push( new Point( temp.X - 1, y1 ) );
            spanLeft = true;
          }
          else if ( spanLeft && temp.X - 1 == 0 && Image.GetPixel( temp.X - 1, y1 ) != targetColor )
          {
            spanLeft = false;
          }
          if ( !spanRight && temp.X < Image.Width - 1 && Image.GetPixel( temp.X + 1, y1 ) == targetColor )
          {
            pixels.Push( new Point( temp.X + 1, y1 ) );
            spanRight = true;
          }
          else if ( spanRight && temp.X < Image.Width - 1 && Image.GetPixel( temp.X + 1, y1 ) != targetColor )
          {
            spanRight = false;
          }
          y1++;
        }

      }
    }



    public Palette PaletteFromMachine( MachineType Machine )
    {
      switch ( Machine )
      {
        case MachineType.C64:
        default:
          return Core.Settings.Palettes[PaletteType.C64][0];
        case MachineType.C128:
          return Core.Settings.Palettes[PaletteType.C128_VDC][0];
        case MachineType.MEGA65:
          return Core.Settings.Palettes[PaletteType.MEGA65][0];
        case MachineType.VIC20:
          return Core.Settings.Palettes[PaletteType.VIC20][0];
        case MachineType.COMMANDER_X16:
          return Core.Settings.Palettes[PaletteType.COMMANDER_X16][0];
      }
    }



  }
}
