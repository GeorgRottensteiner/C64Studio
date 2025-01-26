using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Text;
using GR.Image;
using RetroDevStudio.Types;
using WeifenLuo.WinFormsUI.Docking;


namespace RetroDevStudio
{
  public class Imaging
  {
    StudioCore        Core;

    private Dictionary<MachineType,System.Drawing.Text.PrivateFontCollection>  _FontCollectionCache = new Dictionary<MachineType, System.Drawing.Text.PrivateFontCollection>();



    public Imaging( StudioCore Core )
    {
      this.Core = Core;
    }



    internal GR.Image.FastImage LoadImageFromFile( string Filename )
    {
      string                extension = GR.Path.GetExtension( Filename ).ToUpper();
      GR.Image.FastImage    newImage;

      if ( ( extension == ".KOA" )
      ||   ( extension == ".KLA" ) )
      {
        var koalaImage = RetroDevStudio.Converter.KoalaToBitmap.BitmapFromKoala( Filename );
        var bitmap = koalaImage.GetAsBitmap();

        newImage = GR.Image.FastImage.FromImage( bitmap );

        bitmap.Dispose();
      }
      else if ( extension == ".IFF" )
      {
        var iffImage = RetroDevStudio.Converter.IFFToBitmap.BitmapFromIFF( Filename );
        var bitmap = iffImage.GetAsBitmap();

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
          return Core.Settings.Palettes[PaletteType.C64][0];
        case MachineType.C128:
          return Core.Settings.Palettes[PaletteType.C128_VDC][0];
        case MachineType.MEGA65:
          return Core.Settings.Palettes[PaletteType.MEGA65][0];
        case MachineType.VIC20:
          return Core.Settings.Palettes[PaletteType.VIC20][0];
        case MachineType.COMMANDER_X16:
          return Core.Settings.Palettes[PaletteType.COMMANDER_X16][0];
        case MachineType.NES:
          return Core.Settings.Palettes[PaletteType.NES][0];
        default:
          Debug.Log( $"Unsupported machine {Machine}" );
          return Core.Settings.Palettes[PaletteType.C64][0];
      }
    }



    public Font FontFromMachine( MachineType machine, float fontSize = 16, FontStyle style = FontStyle.Regular )
    {
      var machineToLookFor = machine;

      switch ( machine )
      {
        case MachineType.C64:
        case MachineType.C128:
        case MachineType.MEGA65:
        case MachineType.VIC20:
        case MachineType.COMMANDER_X16:
          machineToLookFor = MachineType.C64;
          break;
        default:
          machineToLookFor = MachineType.C64;
          break;
      }

      if ( !_FontCollectionCache.TryGetValue( machineToLookFor, out var fontCollection ) )
      {
        Debug.Log( $"No font found for {machineToLookFor}!" );
        return null;
      }

      return new System.Drawing.Font( fontCollection.Families[0], fontSize, style, System.Drawing.GraphicsUnit.Point );
    }



    public void ApplyPalette( PaletteType PalType, PaletteType OriginalType, ColorSettings Colors )
    {
      // only modify if applicable
      if ( ( PalType != OriginalType )
      ||   ( Lookup.AllowPaletteModification( PalType ) ) )
      {
        return;
      }
      var updatedPal = Core.Settings.Palettes[PalType][0];
      foreach ( var pal in Colors.Palettes )
      {
        for ( int i = 0; i < pal.NumColors; ++i )
        {
          pal.ColorValues[i] = updatedPal.ColorValues[i];
        }
        pal.CreateBrushes();
      }
    }



    internal void Initialize()
    {
      LoadFont( MachineType.C64, "C64_Pro_Mono_v1.0-STYLE.ttf" );
      LoadFont( MachineType.ZX81, "zx81.ttf" );
    }



    internal void LoadFont( MachineType machine, string fontFilename )
    {
      var fontCollectionCommodore = new System.Drawing.Text.PrivateFontCollection();
      try
      {
        string basePath = System.Reflection.Assembly.GetExecutingAssembly().Location;

        if ( basePath.ToUpper().StartsWith( "FILE:///" ) )
        {
          basePath = basePath.Substring( 8 );
        }
        if ( basePath.ToUpper().StartsWith( @"FILE:\\" ) )
        {
          basePath = basePath.Substring( 7 );
        }
        string fontPath = GR.Path.Append( GR.Path.GetDirectoryName( basePath ) + "/Fonts", fontFilename );

        fontCollectionCommodore.AddFontFile( fontPath );
      }
      catch ( Exception )
      {
        // fallback to straight path
        try
        {
          string fontPath = @"Fonts/{fontFilename}";

          fontCollectionCommodore.AddFontFile( fontPath );
        }
        catch ( Exception ex2 )
        {
          System.Windows.Forms.MessageBox.Show( $"C64Studio can't find or open the C64 true type font file {fontFilename}.\r\nMake sure it's in the Fonts sub folder of RetroDevStudio.exe.\r\n\r\n" + ex2.Message, "Can't load font" );
          throw new Exception( $"Missing font file '{fontFilename}'" );
        }
      }
      if ( fontCollectionCommodore.Families.Length == 0 )
      {
        System.Windows.Forms.MessageBox.Show( $"C64Studio loaded the true type font file {fontFilename}, but it does not properly work.\r\nMake sure it's in the Fonts sub folder of RetroDevStudio.exe.\r\n", "Can't load font" );
        throw new Exception( $"Failed to load font file '{fontFilename}'" );
      }

      _FontCollectionCache.Add( machine, fontCollectionCommodore );
    }



  }
}
