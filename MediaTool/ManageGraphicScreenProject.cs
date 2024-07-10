using GR.Image;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaTool
{
  public partial class Manager
  {
    internal GR.Image.MemoryImage LoadImageFromFile( string Filename )
    {
      string                extension = System.IO.Path.GetExtension( Filename ).ToUpper();

      if ( ( extension == ".KOA" )
      ||   ( extension == ".KLA" ) )
      {
        return RetroDevStudio.Converter.KoalaToBitmap.BitmapFromKoala( Filename );
      }
      // TODO - get rid of System.Drawing.Bitmap
      System.Drawing.Bitmap bmpImage = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile( Filename );
      var fastImage = FastImage.FromImage( bmpImage );
      bmpImage.Dispose();

      var newImage = new MemoryImage( fastImage.Width, fastImage.Height, fastImage.PixelFormat );
      for ( int i = 0; i < fastImage.PaletteEntryCount; ++i )
      {
        newImage.SetPaletteColor( i, fastImage.PaletteRed( i ), fastImage.PaletteGreen( i ), fastImage.PaletteBlue( i ) );
      }

      fastImage.DrawImage( newImage, 0, 0 );
      fastImage.Dispose();

      return newImage;
    }



    private int HandleGraphicscreenFile( GR.Text.ArgumentParser ArgParser )
    {
      string    exportType = ArgParser.Parameter( "TYPE" );

      if ( !ValidateExportType( "graphicscreen file", exportType, 
          new string[] { "MULTICOLORBITMAPSCREENCOLOR", "MULTICOLORBITMAPCOLORSCREEN", "MULTICOLORBITMAPSCREEN", "MULTICOLORBITMAPCOLOR", "MULTICOLORBITMAP",
                         "HIRESBITMAPSCREENCOLOR", "HIRESBITMAPCOLORSCREEN", "HIRESBITMAPSCREEN", "HIRESBITMAPCOLOR", "HIRESBITMAP" } ) )
      {
        return 1;
      }

      string    inputFile = ArgParser.Parameter( "GRAPHICSCREEN" );

      GR.Memory.ByteBuffer    data = GR.IO.File.ReadAllBytes( inputFile );
      if ( data == null )
      {
        System.Console.WriteLine( "Couldn't read binary char file " + inputFile );
        return 1;
      }

      var graphicScreen = new RetroDevStudio.Formats.GraphicScreenProject();
      if ( !graphicScreen.ReadFromBuffer( data ) )
      {
        System.Console.WriteLine( "Couldn't read graphicscreen project from file " + inputFile );
        return 1;
      }

      // import
      if ( ArgParser.IsParameterSet( "IMPORTIMAGE" ) )
      {
        var image = LoadImageFromFile( ArgParser.Parameter( "IMPORTIMAGE" ) );
        if ( image == null )
        {
          System.Console.WriteLine( "Couldn't read image from file " + ArgParser.Parameter( "IMPORTIMAGE" ) );
          return 1;
        }
        if ( ( image.Width > graphicScreen.ScreenWidth )
        ||   ( image.Height > graphicScreen.ScreenHeight ) )
        {
          System.Console.WriteLine( "Couldn't read image from file " + ArgParser.Parameter( "IMPORTIMAGE" ) );
          return 1;
        }
        if ( image.PixelFormat != GR.Drawing.PixelFormat.Format8bppIndexed )
        {
          image.Dispose();
          Console.WriteLine( "Image format invalid!\nNeeds to be 8bit index" );
          return 1;
        }
        Console.WriteLine( "Image Import is currently not supported\n" );
        return 1;
      }

      // export
      int     x = 0;
      int     y = 0;
      int     width = -1;
      int     height = -1;
      if ( ArgParser.IsParameterSet( "AREA" ) )
      {
        string      rangeInfo = ArgParser.Parameter( "AREA" );
        string[]    rangeParts = rangeInfo.Split( ',' );
        if ( rangeParts.Length != 4 )
        {
          System.Console.WriteLine( "AREA is invalid, expected four values separated by comma: x,y,width,height" );
          return 1;
        }
        x = GR.Convert.ToI32( rangeParts[0] );
        y = GR.Convert.ToI32( rangeParts[1] );
        width = GR.Convert.ToI32( rangeParts[2] );
        height = GR.Convert.ToI32( rangeParts[3] );

        if ( ( width <= 0 )
        ||   ( height <= 0 )
        ||   ( x < 0 )
        ||   ( y < 0 )
        ||   ( x + width > graphicScreen.ScreenWidth )
        ||   ( y + height > graphicScreen.ScreenHeight ) )
        {
          System.Console.WriteLine( "AREA values are out of bounds or invalid, expected four values separated by comma: x,y,width,height" );
          return 1;
        }
      }
      else
      {
        width = graphicScreen.ScreenWidth;
        height = graphicScreen.ScreenHeight;
      }

      bool    exportMC = exportType.Contains( "MULTICOLOR" );
      bool    exportScreen = exportType.Contains( "SCREEN" );
      bool    exportColors = exportType.Contains( "COLORS" );
      bool    exportBitmap = exportType.Contains( "BITMAP" );

      GR.Memory.ByteBuffer screenChar   = new GR.Memory.ByteBuffer();
      GR.Memory.ByteBuffer screenColor  = new GR.Memory.ByteBuffer();
      GR.Memory.ByteBuffer bitmapData   = new GR.Memory.ByteBuffer();

      bool[,]   errornousChars = new bool[( width + 7 ) / 8, ( height + 7 ) / 8];
      var       charData = new List<RetroDevStudio.Formats.CharData>( errornousChars.Length );
      int       numErrors = 0;

      if ( !exportMC )
      {
        // HIRES
        numErrors = graphicScreen.ImageToHiresBitmapData( graphicScreen.ColorMapping, charData, errornousChars, x / 8, y / 8, width / 8, height / 8, out bitmapData, out screenChar, out screenColor );
      }
      else
      {
        // MC
        numErrors = graphicScreen.ImageToMCBitmapData( graphicScreen.ColorMapping, charData, errornousChars, x / 8, y / 8, width / 8, height / 8, out bitmapData, out screenChar, out screenColor );
      }
      if ( numErrors > 0 )
      {
        System.Console.WriteLine( "Format did not match expectations (check for color clashes)" );
        return 1;
      }

      // build export data
      GR.Memory.ByteBuffer exportData = new GR.Memory.ByteBuffer();

      if ( exportType.Contains( "BITMAPSCREENCOLORS" ) )
      {
        exportData.Append( bitmapData );
        exportData.Append( screenChar );
        exportData.Append( screenColor );
      }
      else if ( exportType.Contains( "BITMAPCOLORSSCREEN" ) )
      {
        exportData.Append( bitmapData );
        exportData.Append( screenColor );
        exportData.Append( screenChar );
      }
      else if ( exportType.Contains( "BITMAPCOLORS" ) )
      {
        exportData.Append( bitmapData );
        exportData.Append( screenColor );
      }
      else if ( exportType.Contains( "BITMAPSCREEN" ) )
      {
        exportData.Append( bitmapData );
        exportData.Append( screenChar );
      }
      else if ( exportType.Contains( "BITMAP" ) )
      {
        exportData.Append( bitmapData );
      }

      if ( !GR.IO.File.WriteAllBytes( ArgParser.Parameter( "EXPORT" ), exportData ) )
      {
        Console.WriteLine( "Could not write to file " + ArgParser.Parameter( "EXPORT" ) );
        return 1;
      }

      return 0;
    }

  }
}
