using RetroDevStudio.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using WeifenLuo.WinFormsUI.Docking;
using static System.Net.Mime.MediaTypeNames;



namespace DecentForms
{
  public class ControlRenderer : IControlRenderer
  {
    private Graphics        _G;
    private ControlBase     _Control;

    private int             _DisplayOffsetX = 0;
    private int             _DisplayOffsetY = 0;
    private int             _SubAreaDisplayOffsetX = 0;
    private int             _SubAreaDisplayOffsetY = 0;
    private Rectangle       _OriginalBounds = Rectangle.Empty; 


    private static Dictionary<System.Drawing.Image,System.Drawing.Image>    _GrayscaledImageCache = new Dictionary<System.Drawing.Image, System.Drawing.Image>();

    private static string   _BitmapArrowUpData    = "424D760200000000000036000000280000000C0000000C00000001002000000000000000000000000000000000000000000000000000DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FFDADADA00DADADA00DADADA00DADADA00DADADA00000000FF000000FF000000FF000000FF000000FF000000FFDADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00000000FF000000FF000000FF000000FFDADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00000000FF000000FFDADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00";
    private static string   _BitmapArrowDownData  = "424D760200000000000036000000280000000C0000000C00000001002000000000000000000000000000000000000000000000000000DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00000000FF000000FFDADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00000000FF000000FF000000FF000000FFDADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00000000FF000000FF000000FF000000FF000000FF000000FFDADADA00DADADA00DADADA00DADADA00DADADA00000000FF000000FF000000FF000000FF000000FF000000FF000000FF000000FFDADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00";
    private static string   _BitmapArrowLeftData  = "424D760200000000000036000000280000000C0000000C00000001002000000000000000000000000000000000000000000000000000DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00000000FFDADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00000000FF000000FFDADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00000000FF000000FF000000FFDADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00000000FF000000FF000000FF000000FFDADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00000000FF000000FF000000FF000000FFDADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00000000FF000000FF000000FFDADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00000000FF000000FFDADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00000000FFDADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00";
    private static string   _BitmapArrowRightData = "424D760200000000000036000000280000000C0000000C00000001002000000000000000000000000000000000000000000000000000DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00000000FFDADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00000000FF000000FFDADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00000000FF000000FF000000FFDADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00000000FF000000FF000000FF000000FFDADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00000000FF000000FF000000FF000000FFDADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00000000FF000000FF000000FFDADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00000000FF000000FFDADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00000000FFDADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00DADADA00";

    private static Bitmap   _BitmapArrowUp;
    private static Bitmap   _BitmapArrowDown;
    private static Bitmap   _BitmapArrowLeft;
    private static Bitmap   _BitmapArrowRight;



    static ControlRenderer()
    {
      _BitmapArrowUp    = ImageFromHex( _BitmapArrowUpData );
      _BitmapArrowDown  = ImageFromHex( _BitmapArrowDownData );
      _BitmapArrowLeft  = ImageFromHex( _BitmapArrowLeftData );
      _BitmapArrowRight = ImageFromHex( _BitmapArrowRightData );
    }



    public static Bitmap ImageFromHex( string ImageData )
    {
      byte[] imageBytes = StringToByteArray( ImageData );
      return ByteArrayToBitmap( imageBytes );
    }



    public static byte[] StringToByteArray( String HexString )
    {
      int numberOfChars = HexString.Length / 2;
      byte[] byteArray = new byte[numberOfChars];
      using ( var sr = new System.IO.StringReader( HexString ) )
      {
        for ( int i = 0; i < numberOfChars; i++ )
        {
          byteArray[i] = Convert.ToByte( new string( new char[2] { (char)sr.Read(), (char)sr.Read() } ), 16 );
        }
      }
      return byteArray;
    }



    public static Bitmap ByteArrayToBitmap( byte[] pData )
    {
      var mStream = new MemoryStream();
      mStream.Write( pData, 0, Convert.ToInt32( pData.Length ) );
      Bitmap bm = new Bitmap( mStream, false );
      mStream.Dispose();

      // HACK clone bitmap to keep alpha working (Bitmap from stream doesn't set 32bit with alpha format!)
      var clearBitmap = new Bitmap( bm.Width, bm.Height, PixelFormat.Format32bppArgb );

      // Lock the bitmap's bits.  
      Rectangle rect = new Rectangle( 0, 0, bm.Width, bm.Height );

      System.Drawing.Imaging.BitmapData bmpDataSource = bm.LockBits( rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, bm.PixelFormat );
      System.Drawing.Imaging.BitmapData bmpDataTarget = clearBitmap.LockBits( rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, clearBitmap.PixelFormat );

      // Get the address of the first line.
      IntPtr ptrSource = bmpDataSource.Scan0;
      IntPtr ptrTarget = bmpDataTarget.Scan0;
      int bytes  = Math.Abs( bmpDataTarget.Stride ) * clearBitmap.Height;
      byte[] rgbValues = new byte[bytes];

      // Copy the RGB values into the array.
      System.Runtime.InteropServices.Marshal.Copy( ptrSource, rgbValues, 0, bytes );
      System.Runtime.InteropServices.Marshal.Copy( rgbValues, 0, ptrTarget, bytes );

      // Unlock the bits.
      bm.UnlockBits( bmpDataSource );
      clearBitmap.UnlockBits( bmpDataTarget );

      bm.Dispose();

      return clearBitmap;
    }



    public ControlRenderer( Graphics G, ControlBase Control )
    {
      _G = G;
      _Control = Control;

      _DisplayOffsetX = Control.DisplayOffsetX;
      _DisplayOffsetY = Control.DisplayOffsetY;
      _OriginalBounds = new Rectangle( (int)_G.ClipBounds.X, (int)_G.ClipBounds.Y, (int)_G.ClipBounds.Width, (int)_G.ClipBounds.Height );
    }



    static public uint      ColorControlInActiveBackground { get; set; }  = 0xfff0f0f0;
    static public uint      ColorControlActiveBackground { get; set; }    = 0xffffffff;
    static public uint      ColorControlBackground { get; set; }          = 0xffc0c0c0;
    static public uint      ColorControlBackgroundMouseOver { get; set; } = 0xff8080ff;
    static public uint      ColorControlBackgroundSelected { get; set; }  = 0xff80ff80;
    static public uint      ColorControlText { get; set; }                = 0xff000000;
    static public uint      ColorControlTextMouseOver { get; set; }       = 0xff000000;
    static public uint      ColorControlTextSelected { get; set; }        = 0xffffffff;
    static public uint      ColorControlBorderFlat { get; set; }          = 0xff000000;



    internal Color ToColor( uint ColorValue )
    {
      return Color.FromArgb( unchecked( (int)ColorValue ) );
    }



    internal uint DarkenColor( uint OrigColor )
    {
      float   darkFactor = 0.85f;
      return ( OrigColor & 0xff000000 ) 
           | ( (uint)( ( ( OrigColor & 0xff0000 ) >> 16 ) * darkFactor ) << 16 )
           | ( (uint)( ( ( OrigColor & 0x00ff00 ) >>  8 ) * darkFactor ) <<  8 )
           | ( (uint)( ( ( OrigColor & 0x0000ff ) >>  0 ) * darkFactor ) <<  0 );
    }



    internal uint Lighten( uint Value, float Factor )
    {
      Value = (uint)( Value * Factor );
      if ( Value > 255 )
      {
        Value = 255;
      }
      return Value;
    }



    internal uint LightenColor( uint OrigColor )
    {
      float   lightFactor = 1.25f;
      return ( OrigColor & 0xff000000 )
           | ( Lighten( ( OrigColor & 0xff0000 ) >> 16, lightFactor ) << 16 )
           | ( Lighten( ( OrigColor & 0x00ff00 ) >>  8, lightFactor ) <<  8 )
           | ( Lighten( ( OrigColor & 0x0000ff ) >>  0, lightFactor ) <<  0 );
    }



    public void DrawRaisedRectangle( int X, int Y, int Width, int Height, uint BaseColor )
    {
      var   regularPen    = ColoredPen( BaseColor );
      var   highlightPen  = ColoredPen( LightenColor( BaseColor ) );
      var   darkPen       = ColoredPen( DarkenColor( BaseColor ) );
      var   darkShadowPen = ColoredPen( DarkenColor( DarkenColor( BaseColor ) ) );

      X -= _DisplayOffsetX;
      Y -= _DisplayOffsetY;

      _G.DrawLine( regularPen, X, Y, X + Width - 2, Y );
      _G.DrawLine( regularPen, X, Y, X, Y + Height - 2 );
      _G.DrawLine( regularPen, X, Y + Height - 1, X + 1, Y + Height - 2 );
      _G.DrawLine( regularPen, X + Width - 2, Y + 1, X + Width - 1, Y );

      _G.DrawLine( highlightPen, X + 1, Y + 1, X + Width - 3, Y + 1 );
      _G.DrawLine( highlightPen, X + 1, Y + 1, X + 1, Y + Height - 3 );
      _G.DrawLine( darkPen, X + Width - 2, Y + 2, X + Width - 2, Y + Height - 2 );
      _G.DrawLine( darkPen, X + 2, Y + Height - 2, X + Width - 3, Y + Height - 2 );
      _G.DrawLine( darkShadowPen, X + Width - 1, Y + 1, X + Width - 1, Y + Height - 1 );
      _G.DrawLine( darkShadowPen, X + 1, Y + Height - 1, X + Width - 2, Y + Height - 1 );
    }



    public void FillRaisedRectangle( int X, int Y, int Width, int Height, uint BaseColor )
    {
      var   highlightPen  = ColoredPen( LightenColor( BaseColor ) );
      var   darkPen       = ColoredPen( DarkenColor( BaseColor ) );
      var   darkShadowPen = ColoredPen( DarkenColor( DarkenColor( BaseColor ) ) );
      var   fillBrush     = new SolidBrush( Color.FromArgb( (int)BaseColor ) );

      X -= _DisplayOffsetX;
      Y -= _DisplayOffsetY;

      _G.FillRectangle( fillBrush, X, Y, Width, Height );
      _G.DrawLine( highlightPen, X + 1, Y + 1, X + Width - 3, Y + 1 );
      _G.DrawLine( highlightPen, X + 1, Y + 1, X + 1, Y + Height - 3 );
      _G.DrawLine( darkPen, X + Width - 2, Y + 2, X + Width - 2, Y + Height - 2 );
      _G.DrawLine( darkPen, X + 2, Y + Height - 2, X + Width - 3, Y + Height - 2 );
      _G.DrawLine( darkShadowPen, X + Width - 1, Y + 1, X + Width - 1, Y + Height - 1 );
      _G.DrawLine( darkShadowPen, X + 1, Y + Height - 1, X + Width - 2, Y + Height - 1 );
    }



    public void DrawSunkenRectangle( int X, int Y, int Width, int Height, uint BaseColor )
    {
      var   regularPen    = ColoredPen( BaseColor );
      var   highlightPen  = ColoredPen( LightenColor( BaseColor ) );
      var   darkPen       = ColoredPen( DarkenColor( BaseColor ) );
      var   darkShadowPen = ColoredPen( DarkenColor( DarkenColor( BaseColor ) ) );

      X -= _DisplayOffsetX;
      Y -= _DisplayOffsetY;

      _G.DrawLine( highlightPen, X, Y + Height - 1, X + Width - 2, Y + Height - 1 );
      _G.DrawLine( highlightPen, X + Width - 1, Y, X + Width - 1, Y + Height - 1 );
      _G.DrawLine( regularPen, X, Y + Height - 1, X + 1, Y + Height - 2 );
      _G.DrawLine( regularPen, X + Width - 2, Y + 1, X + Width - 1, Y );

      _G.DrawLine( darkShadowPen, X, Y, X + Width - 2, Y );
      _G.DrawLine( darkShadowPen, X, Y, X, Y + Height - 2 );
      _G.DrawLine( darkPen, X + 1, Y + 1, X + Width - 3, Y + 1 );
      _G.DrawLine( darkPen, X + 1, Y + 1, X + 1, Y + Height - 3 );
      _G.DrawLine( regularPen, X + Width - 2, Y + 2, X + Width - 2, Y + Height - 2 );
      _G.DrawLine( regularPen, X + 2, Y + Height - 2, X + Width - 3, Y + Height - 2 );
    }



    public void FillSunkenRectangle( int X, int Y, int Width, int Height, uint BaseColor )
    {
      var   highlightPen  = ColoredPen( LightenColor( BaseColor ) );
      var   darkPen       = ColoredPen( DarkenColor( BaseColor ) );
      var   darkShadowPen = ColoredPen( DarkenColor( DarkenColor( BaseColor ) ) );
      var   fillBrush     = new SolidBrush( Color.FromArgb( (int)BaseColor ) );

      X -= _DisplayOffsetX;
      Y -= _DisplayOffsetY;

      _G.FillRectangle( fillBrush, X, Y, Width, Height );
      _G.DrawLine( darkShadowPen, X, Y, X + Width - 2, Y );
      _G.DrawLine( darkShadowPen, X, Y, X, Y + Height - 2 );
      _G.DrawLine( darkPen, X + 1, Y + 1, X + Width - 3, Y + 1 );
      _G.DrawLine( darkPen, X + 1, Y + 1, X + 1, Y + Height - 3 );
      _G.DrawLine( highlightPen, X, Y + Height - 1, X + Width - 2, Y + Height - 1 );
      _G.DrawLine( highlightPen, X + Width - 1, Y, X + Width - 1, Y + Height - 1 );
    }



    public void DrawLine( int X1, int Y1, int X2, int Y2, uint BaseColor )
    {
      var   borderPen       = ColoredPen( BaseColor );

      X1 -= _DisplayOffsetX;
      Y1 -= _DisplayOffsetY;
      X2 -= _DisplayOffsetX;
      Y2 -= _DisplayOffsetY;

      _G.DrawLine( borderPen, X1, Y1, X2, Y2 );
    }



    public void DrawRectangle( Rectangle bounds, uint BaseColor )
    {
      DrawRectangle( bounds.Left, bounds.Top, bounds.Width, bounds.Height, BaseColor );
    }



    public void DrawRectangle( int X, int Y, int Width, int Height, uint BaseColor )
    {
      var   borderPen       = ColoredPen( BaseColor );

      X -= _DisplayOffsetX;
      Y -= _DisplayOffsetY;

      _G.DrawRectangle( borderPen, X, Y, Width - 1, Height - 1 );
    }



    public void FillRectangle( Rectangle bounds, uint BaseColor )
    {
      FillRectangle( bounds.Left, bounds.Top, bounds.Width, bounds.Height, BaseColor );
    }



    public void FillRectangle( int X, int Y, int Width, int Height, uint BaseColor )
    {
      var   fillBrush = new SolidBrush( Color.FromArgb( (int)BaseColor ) );

      X -= _DisplayOffsetX;
      Y -= _DisplayOffsetY;

      _G.FillRectangle( fillBrush, X, Y, Width, Height );
    }



    private Pen ColoredPen( uint BaseColor )
    {
      return new Pen( ToColor( BaseColor ) );
    }



    public void DrawText( System.Drawing.Font Font, string Text, int BoundsX, int BoundsY, int Width, int Height, TextAlignment Alignment, int DX = 0, int DY = 0 )
    {
      BoundsX -= _DisplayOffsetX;
      BoundsY -= _DisplayOffsetY;

      TextRenderer.DrawText( _G, Text, Font, new Rectangle( BoundsX + DX, BoundsY + DY, Width, Height ), ToColor( ColorControlText ), MapAlignmentToFlags( Alignment ) | TextFormatFlags.PreserveGraphicsClipping );
    }



    public void DrawText( string Text, int BoundsX, int BoundsY, int Width, int Height, TextAlignment Alignment, int DX = 0, int DY = 0 )
    {
      DrawText( _Control.Font, Text, BoundsX, BoundsY, Width, Height, Alignment, DX, DY );
    }



    public void DrawText( System.Drawing.Font Font, string Text, int BoundsX, int BoundsY, int Width, int Height, TextAlignment Alignment, uint Color )
    {
      BoundsX -= _DisplayOffsetX;
      BoundsY -= _DisplayOffsetY;

      TextRenderer.DrawText( _G, Text, Font, new Rectangle( BoundsX, BoundsY, Width, Height ), ToColor( Color ), MapAlignmentToFlags( Alignment ) | TextFormatFlags.PreserveGraphicsClipping );
    }



    public void DrawText( string Text, int BoundsX, int BoundsY, int Width, int Height, TextAlignment Alignment, uint Color )
    {
      DrawText( _Control.Font, Text, BoundsX, BoundsY, Width, Height, Alignment, Color );
    }



    private TextFormatFlags MapAlignmentToFlags( TextAlignment Alignment )
    {
      var flags = TextFormatFlags.Default;
      if ( ( Alignment & TextAlignment.LEFT ) != 0 )
      {
        flags |= TextFormatFlags.Left;
      }
      if ( ( Alignment & TextAlignment.RIGHT ) != 0 )
      {
        flags |= TextFormatFlags.Right;
      }
      if ( ( Alignment & TextAlignment.TOP ) != 0 )
      {
        flags |= TextFormatFlags.Top;
      }
      if ( ( Alignment & TextAlignment.BOTTOM ) != 0 )
      {
        flags |= TextFormatFlags.Bottom;
      }
      if ( ( Alignment & TextAlignment.CENTERED_H ) != 0 )
      {
        flags |= TextFormatFlags.HorizontalCenter;
      }
      if ( ( Alignment & TextAlignment.CENTERED_V ) != 0 )
      {
        flags |= TextFormatFlags.VerticalCenter;
      }

      return flags;
    }



    public void DrawDisabledText( string Text, int BoundsX, int BoundsY, int Width, int Height, TextAlignment Alignment, int DX = 0, int DY = 0 )
    {
      BoundsX -= _DisplayOffsetX;
      BoundsY -= _DisplayOffsetY;

      //TextRenderer.DrawText( _G, Text, _Control.Font, new Rectangle( BoundsX + DX, BoundsY + DY, Width, Height ), ToColor( ColorControlText ) );
      //TextRenderer.DrawText( _G, Text, _Control.Font, new Rectangle( BoundsX + DX + 1, BoundsY + DY, Width, Height ), ToColor( LightenColor( ColorControlBackground ) ) );
      TextRenderer.DrawText( _G, Text, _Control.Font, new Rectangle( BoundsX + DX, BoundsY + DY, Width, Height ), ToColor( DarkenColor( DarkenColor( ColorControlBackground ) ) ) );
    }



    public void DrawFocusRect( int X, int Y, int Width, int Height, uint Color )
    {
      var   focusRectPen = ColoredPen( Color );
      focusRectPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

      X -= _DisplayOffsetX;
      Y -= _DisplayOffsetY;

      _G.DrawRectangle( focusRectPen, X, Y, Width - 1, Height - 1 );
    }



    public void RenderButton()
    {
      if ( _Control is Button )
      {
        var button = (Button)_Control;
        RenderButton( button.MouseOver, button.Pushed, button.IsDefault, button.ButtonBorder );
      }
    }



    public void RenderButton( bool MouseOver, bool Pushed, bool IsDefault, Button.ButtonStyle Style )
    {
      RenderButton( MouseOver, Pushed, IsDefault, _Control.Focused, Style, new Rectangle( 0, 0, _Control.Width, _Control.Height ), _Control.Text );
    }



    internal void RenderButton( bool MouseOver, bool Pushed, bool IsDefault, bool focused, Button.ButtonStyle Style, Rectangle Rect, string text )
    {
      Rect.Offset( -_DisplayOffsetX, -_DisplayOffsetY );

      System.Drawing.Image   imageToDraw = null;
      if ( _Control is Button )
      {
        imageToDraw = ( (Button)_Control ).Image;
      }
      else if ( _Control is RadioButton )
      {
        imageToDraw = ( (RadioButton)_Control ).Image;
      }
      else if ( _Control is CheckBox )
      {
        imageToDraw = ( (CheckBox)_Control ).Image;
      }
      if ( ( imageToDraw != null ) 
      &&   ( !_Control.Enabled ) )
      {
        imageToDraw = GetGrayScaleBitmap( imageToDraw );
      }

      switch ( Style )
      {
        case Button.ButtonStyle.RAISED:
          if ( !_Control.Enabled )
          {
            if ( Pushed )
            {
              FillSunkenRectangle( Rect.Left, Rect.Top, Rect.Width, Rect.Height, ColorControlBackgroundSelected );
            }
            else
            {
              FillRaisedRectangle( Rect.Left, Rect.Top, Rect.Width, Rect.Height, ColorControlBackground );
            }
            if ( imageToDraw != null )
            {
              DrawImageCentered( imageToDraw, Rect );
            }
            DrawDisabledText( text, Rect.Left, Rect.Top, Rect.Width, Rect.Height, TextAlignment.CENTERED );
          }
          else if ( Pushed )
          {
            FillSunkenRectangle( Rect.Left, Rect.Top, Rect.Width, Rect.Height, ColorControlBackgroundSelected );
            if ( imageToDraw != null )
            {
              DrawImageCentered( imageToDraw, Rect, 0, 1 );
            }
            DrawText( text, Rect.Left, Rect.Top, Rect.Width, Rect.Height, TextAlignment.CENTERED, 0, 1 );
          }
          else if ( MouseOver )
          {
            DrawRaisedRectangle( Rect.Left, Rect.Top, Rect.Width, Rect.Height, ColorControlBackground );
            FillRectangle( Rect.Left + 2, Rect.Top + 2, Rect.Width - 4, Rect.Height - 4, ColorControlBackgroundMouseOver );
            if ( imageToDraw != null )
            {
              DrawImageCentered( imageToDraw, Rect );
            }
            DrawText( text, Rect.Left, Rect.Top, Rect.Width, Rect.Height, TextAlignment.CENTERED );
          }
          else
          {
            FillRaisedRectangle( Rect.Left, Rect.Top, Rect.Width, Rect.Height, ColorControlBackground );
            if ( imageToDraw != null )
            {
              DrawImageCentered( imageToDraw, Rect );
            }
            DrawText( text, Rect.Left, Rect.Top, Rect.Width, Rect.Height, TextAlignment.CENTERED );
          }
          if ( IsDefault )
          {
            DrawRectangle( Rect.Left, Rect.Top, Rect.Width, Rect.Height, LightenColor( ColorControlText ) );
          }
          break;
        case Button.ButtonStyle.FLAT:
          if ( !_Control.Enabled )
          {
            DrawRectangle( Rect.Left, Rect.Top, Rect.Width, Rect.Height, DarkenColor( ColorControlBackground ) );
            FillRectangle( Rect.Left + 1, Rect.Top + 1, _Control.Width - 2, _Control.Height - 2, ColorControlBackground );
            if ( imageToDraw != null )
            {
              DrawImageCentered( imageToDraw, Rect );
            }
            DrawDisabledText( text, Rect.Left, Rect.Top, Rect.Width, Rect.Height, TextAlignment.CENTERED );
          }
          else if ( Pushed )
          {
            DrawRectangle( Rect.Left, Rect.Top, Rect.Width, Rect.Height, DarkenColor( ColorControlBackgroundSelected ) );
            FillRectangle( Rect.Left + 1, Rect.Top + 1, Rect.Width - 2, Rect.Height - 2, ColorControlBackgroundSelected );
            if ( imageToDraw != null )
            {
              DrawImageCentered( imageToDraw, Rect, 0, 1 );
            }
            DrawText( text, Rect.Left, Rect.Top, Rect.Width, Rect.Height, TextAlignment.CENTERED, 0, 1 );
          }
          else if ( MouseOver )
          {
            DrawRectangle( Rect.Left, Rect.Top, Rect.Width, Rect.Height, DarkenColor( ColorControlBackgroundMouseOver ) );
            FillRectangle( Rect.Left + 1, Rect.Top + 1, Rect.Width - 2, Rect.Height - 2, ColorControlBackgroundMouseOver );
            if ( imageToDraw != null )
            {
              DrawImageCentered( imageToDraw, Rect );
            }
            DrawText( text, Rect.Left, Rect.Top, Rect.Width, Rect.Height, TextAlignment.CENTERED );
          }
          else
          {
            DrawRectangle( Rect.Left, Rect.Top, Rect.Width, Rect.Height, DarkenColor( ColorControlBackground ) );
            FillRectangle( Rect.Left + 1, Rect.Top + 1, Rect.Width - 2, Rect.Height - 2, ColorControlBackground );
            if ( imageToDraw != null )
            {
              DrawImageCentered( imageToDraw, Rect );
            }
            DrawText( text, Rect.Left, Rect.Top, Rect.Width, Rect.Height, TextAlignment.CENTERED );
          }
          if ( IsDefault )
          {
            DrawRectangle( Rect.Left, Rect.Top, Rect.Width, Rect.Height, LightenColor( ColorControlText ) );
          }
          break;
      }

      if ( focused )
      {
        DrawFocusRect( Rect.Left + 3, Rect.Top + 3, Rect.Width - 6, Rect.Height - 6, ColorControlText );
      }
    }



    public void RenderCheckBox( string Text, ContentAlignment Alignment, bool MouseOver, bool Pushed, bool Checked )
    {
      var checkBox = (CheckBox)_Control;

      if ( checkBox.Appearance == Appearance.Normal )
      {
        var checkRect = checkBox.GetCheckRect();
        DrawCheckBox( checkRect, MouseOver, Checked );

        var textRect = checkBox.GetTextRect();
        _G.SetClip( textRect );
        DrawText( Text, textRect.Left, textRect.Top, textRect.Width, textRect.Height, TextAlignment.CENTERED, ColorControlText );
      }
      else
      {
        RenderButton( MouseOver, Checked, false, Button.ButtonStyle.RAISED );
      }
    }



    private System.Drawing.Image GetGrayScaleBitmap( System.Drawing.Image Image )
    {
      if ( _GrayscaledImageCache.TryGetValue( Image, out System.Drawing.Image cachedImage ) )
      {
        return cachedImage;
      }

      var newBitmap = new Bitmap( Image );

      // Lock the bitmap's bits.  
      Rectangle rect = new Rectangle(0, 0, newBitmap.Width, newBitmap.Height);
      System.Drawing.Imaging.BitmapData bmpData =
            newBitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
            newBitmap.PixelFormat);

      // Get the address of the first line.
      IntPtr ptr = bmpData.Scan0;

      // Declare an array to hold the bytes of the bitmap.
      int bytes  = Math.Abs( bmpData.Stride ) * newBitmap.Height;
      byte[] rgbValues = new byte[bytes];

      // Copy the RGB values into the array.
      System.Runtime.InteropServices.Marshal.Copy( ptr, rgbValues, 0, bytes );

      for ( int counter = 0; counter < rgbValues.Length; counter += 4 )
      {
        /*
        if ( ( rgbValues[counter + 3] == 0xff )
        &&   ( rgbValues[counter + 0] == 0xda )
        &&   ( rgbValues[counter + 1] == 0xda )
        &&   ( rgbValues[counter + 2] == 0xda ) )
        {
          // make transparent
          rgbValues[counter + 3] = 0;
        } 
        else*/
        if ( rgbValues[counter + 3] > 0 ) 
        {
          byte  r = rgbValues[counter + 0];
          byte  g = rgbValues[counter + 1];
          byte  b = rgbValues[counter + 2];

          byte finalValue = 0xc0;

          rgbValues[counter + 0] = (byte)( ( finalValue * 3 + r ) / 4 );
          rgbValues[counter + 1] = (byte)( ( finalValue * 3 + g ) / 4 );
          rgbValues[counter + 2] = (byte)( ( finalValue * 3 + b ) / 4 );
        }
      }

      // Copy the RGB values back to the bitmap
      System.Runtime.InteropServices.Marshal.Copy( rgbValues, 0, ptr, bytes );

      // Unlock the bits.
      newBitmap.UnlockBits( bmpData );
      /*
      using ( Graphics g = Graphics.FromImage( newBitmap ) )
      {
        ColorMatrix colorMatrix = new ColorMatrix(new float[][]
        {
            new float[] { 0.3f, 0.3f, 0.3f, 0, 0 },
            new float[] { 0.59f, 0.59f, 0.59f, 0, 0 },
            new float[] { 0.11f, 0.11f, 0.11f, 0, 0 },
            new float[] { 0, 0, 0, 1, 0 },
            new float[] { 0, 0, 0, 0, 1 }
        });

        using ( ImageAttributes attributes = new ImageAttributes() )
        {
          attributes.SetColorMatrix( colorMatrix );
          g.DrawImage( Image, new Rectangle( 0, 0, Image.Width, Image.Height ), 0, 0, Image.Width, Image.Height, GraphicsUnit.Pixel, attributes );
        }
      }*/
      _GrayscaledImageCache.Add( Image, newBitmap );

      return newBitmap;
    }



    public void RenderRadioButton( string Text, ContentAlignment Alignment, bool MouseOver, bool Pushed, bool Checked )
    {
      var checkBox = (RadioButton)_Control;

      var checkRect = checkBox.GetRadioRect();
      DrawRadioButton( checkRect, MouseOver, Checked );

      var textRect = checkBox.GetTextRect();
      _G.SetClip( textRect );
      DrawText( Text, textRect.Left, textRect.Top, textRect.Width, textRect.Height, TextAlignment.CENTERED, ColorControlText );
    }



    private void DrawRadioButton( Rectangle CheckRect, bool MouseOver, bool Checked )
    {
      uint    color = ( MouseOver ? ColorControlBackgroundMouseOver: ColorControlBorderFlat );
      DrawCircle( CheckRect.Left, CheckRect.Top, CheckRect.Width, CheckRect.Height, color );
      if ( Checked )
      {
        FillCircle( CheckRect.Left + 2, CheckRect.Top + 2, CheckRect.Width - 5, CheckRect.Height - 5, color );
      }
    }



    public void RenderSlider( int X, int Y, int Width, int Height, bool MouseOver, bool Pushed )
    {
      X -= _DisplayOffsetX;
      Y -= _DisplayOffsetY;
      if ( Pushed )
      {
        FillSunkenRectangle( X, Y, Width, Height, ColorControlBackgroundSelected );
      }
      else if ( MouseOver )
      {
        FillRaisedRectangle( X, Y, Width, Height, ColorControlBackgroundMouseOver );
      }
      else
      {
        FillRaisedRectangle( X, Y, Width, Height, ColorControlBackground );
      }
    }



    public void DrawImage( System.Drawing.Image Image, int X, int Y )
    {
      _G.DrawImage( Image, X - _DisplayOffsetX, Y - _DisplayOffsetY, Image.Width, Image.Height );
    }



    public void DrawImage( System.Drawing.Image Image, int X, int Y, int Width, int Height )
    {
      if ( Image == null )
      {
        DrawRectangle( X, Y, Width, Height, 0xffff00ff );
        return;
      }
      _G.DrawImage( Image, X - _DisplayOffsetX, Y - _DisplayOffsetY, Width, Height );
    }



    public void DrawImage( System.Drawing.Image Image, Rectangle ImageRect )
    {
      _G.DrawImage( Image, ImageRect.X - _DisplayOffsetX, ImageRect.Y - _DisplayOffsetY, Image.Width, Image.Height );
    }



    private void DrawImageCentered( System.Drawing.Image Image, Rectangle ImageRect, int DX = 0, int DY = 0 )
    {
      _G.DrawImage( Image, 
                    ImageRect.X + ( ImageRect.Width - Image.Width ) / 2 + DX - _DisplayOffsetX,
                    ImageRect.Y + ( ImageRect.Height - Image.Height ) / 2 + DY - _DisplayOffsetY,
                    Image.Width, Image.Height );
    }



    public void RenderTreeViewExpansionToggle( bool IsExpanded, Rectangle Rect )
    {
      int     rectSize = Rect.Height;

      DrawRectangle( Rect.Left, Rect.Top, Rect.Width, Rect.Height, ColorControlBorderFlat );
      DrawLine( Rect.Left + 2, Rect.Top + rectSize / 2, Rect.Right - 3, Rect.Top + rectSize / 2, ColorControlBorderFlat );

      if ( !IsExpanded )
      {
        DrawLine( Rect.Left + Rect.Width / 2, Rect.Top + 2, Rect.Left + Rect.Width / 2, Rect.Bottom - 3, ColorControlBorderFlat );
      }
    }



    internal void RenderBorder()
    {
      switch ( _Control.BorderStyle )
      {
        case BorderStyle.RAISED:
          DrawRaisedRectangle( 0, 0, _Control.Width, _Control.Height, ColorControlBackground );
          break;
        case BorderStyle.SUNKEN:
          DrawSunkenRectangle( 0, 0, _Control.Width, _Control.Height, ColorControlBackground );
          break;
        case BorderStyle.FLAT:
          DrawRectangle( 0, 0, _Control.Width, _Control.Height, ColorControlBorderFlat );
          break;
      }
    }



    /*
    internal void RenderTabControl()
    {
      var tabControl = (TabControl)_Control;

      int   X = 0;
      int   Y = tabControl._TabHeight;
      int   Width = tabControl.ClientSize.Width;
      int   Height = tabControl.ClientSize.Height - tabControl._TabHeight;

      // bottom, left, right border
      uint BaseColor = ColorControlActiveBackground;
      var   regularPen    = ColoredPen( BaseColor );
      var   highlightPen  = ColoredPen( LightenColor( BaseColor ) );
      var   darkPen       = ColoredPen( DarkenColor( BaseColor ) );
      var   darkShadowPen = ColoredPen( DarkenColor( DarkenColor( BaseColor ) ) );

      X -= _DisplayOffsetX;
      Y -= _DisplayOffsetY;

      _G.DrawLine( regularPen, X, Y, X, Y + Height - 2 );
      _G.DrawLine( regularPen, X, Y + Height - 1, X + 1, Y + Height - 2 );

      _G.DrawLine( highlightPen, X + 1, Y + 1, X + 1, Y + Height - 3 );
      _G.DrawLine( darkPen, X + Width - 2, Y + 2, X + Width - 2, Y + Height - 2 );
      _G.DrawLine( darkPen, X + 2, Y + Height - 2, X + Width - 3, Y + Height - 2 );
      _G.DrawLine( darkShadowPen, X + Width - 1, Y + 1, X + Width - 1, Y + Height - 1 );
      _G.DrawLine( darkShadowPen, X + 1, Y + Height - 1, X + Width - 2, Y + Height - 1 );

      int     rightestX = 0;
      for ( int i = 0; i < tabControl.TabPages.Count; ++i )
      {
        var rect = tabControl.GetTabRect( i );
        rightestX = rect.Right;

        RenderTabControlTab( rect, tabControl.TabPages[i].Text, tabControl.MouseOverItem == i, tabControl.SelectedIndex == i );
      }

      // top border of tab control body to the right of tabs
      if ( rightestX < tabControl.ClientSize.Width )
      {
        _G.DrawLine( highlightPen, rightestX + 1, Y - 1, tabControl.ClientSize.Width, Y - 1 );
      }
    }
    */



    private void RenderTabControlTab( Rectangle Rect, string Text, bool MouseOver, bool Selected )
    {
      if ( Selected )
      {
        int   X = Rect.Left;
        int   Y = Rect.Top;
        int   Width = Rect.Width;
        int   Height = Rect.Height;

        uint baseColor      = ColorControlActiveBackground;
        var   regularPen    = ColoredPen( baseColor );
        var   highlightPen  = ColoredPen( LightenColor( baseColor ) );
        var   darkPen       = ColoredPen( DarkenColor( baseColor ) );
        var   darkShadowPen = ColoredPen( DarkenColor( DarkenColor( baseColor ) ) );

        X -= _DisplayOffsetX;
        Y -= _DisplayOffsetY;

        FillRectangle( X, Y, Width, Height, baseColor );

        _G.DrawLine( regularPen, X, Y, X + Width - 2, Y );
        _G.DrawLine( regularPen, X, Y, X, Y + Height - 2 );
        _G.DrawLine( regularPen, X, Y + Height - 1, X + 1, Y + Height - 2 );
        _G.DrawLine( regularPen, X + Width - 2, Y + 1, X + Width - 1, Y );

        _G.DrawLine( highlightPen, X + 1, Y + 1, X + Width - 3, Y + 1 );
        _G.DrawLine( highlightPen, X + 1, Y + 1, X + 1, Y + Height - 3 );
        _G.DrawLine( darkPen, X + Width - 2, Y + 2, X + Width - 2, Y + Height - 2 );
        _G.DrawLine( darkShadowPen, X + Width - 1, Y + 1, X + Width - 1, Y + Height - 1 );

        DrawText( Text, Rect.Left, Rect.Top, Rect.Width, Rect.Height, TextAlignment.CENTERED );
      }
      else
      {
        int   X = Rect.Left;
        int   Y = Rect.Top;
        int   Width = Rect.Width;
        int   Height = Rect.Height;

        uint baseColor      = ColorControlActiveBackground;
        var   regularPen    = ColoredPen( baseColor );
        var   highlightPen  = ColoredPen( LightenColor( baseColor ) );
        var   darkPen       = ColoredPen( DarkenColor( baseColor ) );
        var   darkShadowPen = ColoredPen( DarkenColor( DarkenColor( baseColor ) ) );

        X -= _DisplayOffsetX;
        Y -= _DisplayOffsetY;

        if ( MouseOver )
        {
          FillRectangle( X, Y, Width, Height, ColorControlBackgroundMouseOver );
        }
        else
        {
          FillRectangle( X, Y, Width, Height, DarkenColor( baseColor ) );
        }

        _G.DrawLine( regularPen, X, Y, X + Width - 2, Y );
        _G.DrawLine( regularPen, X, Y, X, Y + Height - 2 );
        _G.DrawLine( regularPen, X, Y + Height - 1, X + 1, Y + Height - 2 );
        _G.DrawLine( regularPen, X + Width - 2, Y + 1, X + Width - 1, Y );

        _G.DrawLine( highlightPen, X + 1, Y + 1, X + Width - 3, Y + 1 );
        _G.DrawLine( highlightPen, X + 1, Y + 1, X + 1, Y + Height - 3 );
        _G.DrawLine( darkPen, X + Width - 2, Y + 2, X + Width - 2, Y + Height - 2 );
        _G.DrawLine( darkShadowPen, X + Width - 1, Y + 1, X + Width - 1, Y + Height - 1 );

        DrawText( Text, Rect.Left, Rect.Top, Rect.Width, Rect.Height, TextAlignment.CENTERED, 0, 2 );
      }
    }



    /*
    internal void RenderCheckBox( string Text, ContentAlignment Alignment, bool MouseOver, bool Pushed, bool Checked )
    {
      var checkBox = (CheckBox)_Control;

      var checkRect = checkBox.GetCheckRect();
      DrawCheckBox( checkRect, MouseOver, Checked );

      var textRect = checkBox.GetTextRect();
      _G.SetClip( textRect );
      DrawText( Text, textRect.Left, textRect.Top, textRect.Width, textRect.Height, TextAlignment.CENTERED, ColorControlText );
    }
    */



    private void DrawCheckBox( Rectangle CheckRect, bool MouseOver, bool Checked )
    {
      uint    color = ColorControlBorderFlat;
      uint    bgColor = ( MouseOver ? ColorControlBackgroundMouseOver: ColorControlActiveBackground );
      DrawRectangle( CheckRect.Left, CheckRect.Top, CheckRect.Width, CheckRect.Height, color );
      FillRectangle( CheckRect.Left + 1, CheckRect.Top + 1, CheckRect.Width - 2, CheckRect.Height - 2, bgColor );
      if ( Checked )
      {
        DrawLine( CheckRect.Left, CheckRect.Top, CheckRect.Right - 1, CheckRect.Bottom - 1, color );
        DrawLine( CheckRect.Left, CheckRect.Bottom - 1, CheckRect.Right - 1, CheckRect.Top, color );
      }
    }



    public void DrawCircle( int X, int Y, int Width, int Height, uint BaseColor )
    {
      var   borderPen       = ColoredPen( BaseColor );

      X -= _DisplayOffsetX;
      Y -= _DisplayOffsetY;

      _G.DrawArc( borderPen, X, Y, Width - 1, Height - 1, 0, 360 );
    }



    public void FillCircle( int X, int Y, int Width, int Height, uint BaseColor )
    {
      var   fillBrush = new SolidBrush( Color.FromArgb( (int)BaseColor ) );

      _G.FillPie( fillBrush, X, Y, Width, Height, 0, 360 );
    }



    public void DrawDashedLine( int X1, int Y1, int X2, int Y2, uint Color )
    {
      /*
      using ( var separatorPen = new Pen( new Das Color ) )
      {
        pevent.Graphics.DrawLine( separatorPen, lineX, lineYFrom, lineX, lineYTo );
      }*/
    }



    public void DrawArrowUp( int X, int Y, int Width, int Height, bool Enabled )
    {
      var imageToDraw = GetStateBitmap( _BitmapArrowUp, Enabled );

      DrawImageCentered( imageToDraw, new Rectangle( X, Y, Width, Height ) );
    }



    private System.Drawing.Image GetStateBitmap( Bitmap Bitmap, bool Enabled )
    {
      if ( Enabled )
      {
        return Bitmap;
      }
      return GetGrayScaleBitmap( Bitmap );
    }



    public void DrawArrowDown( int X, int Y, int Width, int Height, bool Enabled )
    {
      var imageToDraw = GetStateBitmap( _BitmapArrowDown, Enabled );

      DrawImageCentered( imageToDraw, new Rectangle( X, Y, Width, Height ) );
    }



    public void DrawArrowLeft( int X, int Y, int Width, int Height, bool Enabled )
    {
      var imageToDraw = GetStateBitmap( _BitmapArrowLeft, Enabled );

      DrawImageCentered( imageToDraw, new Rectangle( X, Y, Width, Height ) );
    }



    public void DrawArrowRight( int X, int Y, int Width, int Height, bool Enabled )
    {
      var imageToDraw = GetStateBitmap( _BitmapArrowRight, Enabled );

      DrawImageCentered( imageToDraw, new Rectangle( X, Y, Width, Height ) );
    }



    public void DrawTreeViewNodeImage( TreeView.TreeNode Node )
    {
      var treeView = (TreeView)_Control;

      var imageRect = treeView.GetImageRect( Node );
      int imageIndex = Node.ImageIndex;
      if ( ( imageIndex < 0 )
      ||   ( imageIndex >= treeView.ImageList.Images.Count ) )
      {
        imageIndex = 0;
      }

      DrawImageCentered( treeView.ImageList.Images[imageIndex], imageRect );
    }



    public void DrawTreeViewNode( TreeView.TreeNode Node )
    {
      var treeView = (TreeView)_Control;

      var rect = Node.Bounds;

      // expand toggle
      if ( Node.Nodes.Count > 0 )
      {
        var toggleRect = treeView.GetToggleRect( Node );

        RenderTreeViewExpansionToggle( Node.IsExpanded, toggleRect );
      }
      if ( ( treeView.ImageList != null )
      &&   ( treeView.ImageList.Images.Count > 0 ) )
      {
        treeView.RenderNodeImage( this, Node );
      }

      RenderTreeViewNodeText( Node, rect );
    }



    public void RenderTreeView()
    {
      var treeView = (TreeView)_Control;

      FillRectangle( 0, 0, _Control.ClientSize.Width, _Control.ClientSize.Height, ColorControlActiveBackground );

      _G.Clip = new Region( new Rectangle( 0, 0, _Control.ActualWorkWidth, _Control.ActualWorkHeight ) );

      var node = treeView.FirstVisibleNode;

      for ( int i = 0; i <= treeView.VisibleItemCount; ++i )
      {
        if ( node == null )
        {
          break;
        }

        treeView.RenderNode( this, node );

        node = TreeView.GetNextVisibleNode( node );
      }
    }



    public void RenderTreeViewNodeText( TreeView.TreeNode Node, Rectangle Rect )
    {
      var treeView = (TreeView)_Control;

      var fontToUse = treeView.Font;
      if ( Node.NodeFont != null )
      {
        fontToUse = Node.NodeFont;
      }

      if ( Node == treeView.SelectedNode )
      {
        FillRectangle( Rect.Left, Rect.Top, Rect.Width, Rect.Height, ColorControlBackgroundSelected );

        DrawText( fontToUse, Node.Text, Rect.Left, Rect.Top, Rect.Width, Rect.Height, TextAlignment.LEFT | TextAlignment.CENTERED_V, ColorControlTextSelected );
        DrawFocusRect( Rect.Left, Rect.Top, Rect.Width, Rect.Height, ColorControlText );
      }
      else if ( Node == treeView.MouseOverNode )
      {
        FillRectangle( Rect.Left, Rect.Top, Rect.Width, Rect.Height, ColorControlBackgroundMouseOver );
        DrawText( fontToUse, Node.Text, Rect.Left, Rect.Top, Rect.Width, Rect.Height, TextAlignment.LEFT | TextAlignment.CENTERED_V, ColorControlTextMouseOver );
      }
      else
      {
        DrawText( fontToUse, Node.Text, Rect.Left, Rect.Top, Rect.Width, Rect.Height, TextAlignment.LEFT | TextAlignment.CENTERED_V );
      }

      /*
      // If the node has focus, draw the focus rectangle large, making
      // it large enough to include the text of the node tag, if present.
      if ( e.State & TreeNodeStates.Focused ) != 0 )
      {
        using ( Pen focusPen = new Pen( GR.Color.Helper.FromARGB( Core.Settings.FGColor( ColorableElement.CONTROL_TEXT ) ) ) )
        {
          focusPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
          bounds.Size = new Size( bgBounds.Width - 1, bgBounds.Height - 1 );
          bounds.Offset( -3, 0 );
          e.Graphics.DrawRectangle( focusPen, bounds );
        }
      }*/
    }



    public void RenderListBox()
    {
      var listBox = (ListBox)_Control;

      FillRectangle( 0, 0, _Control.ClientSize.Width, _Control.ClientSize.Height, ColorControlActiveBackground );

      _G.Clip = new Region( new Rectangle( 0, 0, _Control.ActualWorkWidth, _Control.ActualWorkHeight ) );

      int   firstItem = listBox.FirstVisibleItemIndex;

      for ( int i = 0; i <= listBox.VisibleItemCount; ++i )
      {
        int   realIndex = firstItem + i;
        if ( realIndex >= listBox.Items.Count )
        {
          break;
        }
        var item = listBox.Items[realIndex];
        var rect = listBox.GetItemRect( realIndex );

        if ( realIndex == listBox.SelectedIndex )
        {
          FillRectangle( rect.Left, rect.Top, rect.Width, rect.Height, ColorControlBackgroundSelected );

          DrawText( item.Text, rect.Left, rect.Top, rect.Width, rect.Height, TextAlignment.LEFT, ColorControlTextSelected );
          DrawFocusRect( rect.Left, rect.Top, rect.Width, rect.Height, ColorControlText );
        }
        else if ( realIndex == listBox.MouseOverItem )
        {
          FillRectangle( rect.Left, rect.Top, rect.Width, rect.Height, ColorControlBackgroundMouseOver );
          DrawText( item.Text, rect.Left, rect.Top, rect.Width, rect.Height, TextAlignment.LEFT, ColorControlTextMouseOver );
        }
        else
        {
          DrawText( item.Text, rect.Left, rect.Top, rect.Width, rect.Height, TextAlignment.LEFT );
        }

      }
    }



    public void RenderGridList()
    {
      var gridList = (GridList)_Control;

      FillRectangle( 0, 0, _Control.ClientSize.Width, _Control.ClientSize.Height, ColorControlActiveBackground );

      _G.Clip = new Region( new Rectangle( 0, 0, _Control.ActualWorkWidth, _Control.ActualWorkHeight ) );

      int   firstItem = gridList.FirstVisibleItemIndex;

      for ( int i = 0; i <= gridList.VisibleItemCount; ++i )
      {
        int   realIndex = firstItem + i;
        if ( realIndex >= gridList.Items.Count )
        {
          break;
        }

        gridList.RenderItem( this, gridList.Items[realIndex], gridList.GetItemRect( realIndex ) );
      }
    }



    internal void RenderGridListItem( GridList.GridListItem item, Rectangle bounds )
    {
      var gridList = (GridList)_Control;

      int realIndex = item.Index;

      if ( realIndex == gridList.SelectedIndex )
      {
        FillRectangle( bounds.Left, bounds.Top, bounds.Width, bounds.Height, ColorControlBackgroundSelected );

        DrawText( item.Text, bounds.Left, bounds.Top, bounds.Width, bounds.Height, TextAlignment.LEFT, ColorControlTextSelected );
        DrawFocusRect( bounds.Left, bounds.Top, bounds.Width, bounds.Height, ColorControlText );
      }
      else if ( realIndex == gridList.MouseOverItem )
      {
        FillRectangle( bounds.Left, bounds.Top, bounds.Width, bounds.Height, ColorControlBackgroundMouseOver );
        DrawText( item.Text, bounds.Left, bounds.Top, bounds.Width, bounds.Height, TextAlignment.LEFT, ColorControlTextMouseOver );
      }
      else
      {
        DrawText( item.Text, bounds.Left, bounds.Top, bounds.Width, bounds.Height, TextAlignment.LEFT );
      }
    }



    public void SetClip( GR.Math.Rectangle rect )
    {
      SetClip( rect.Left, rect.Top, rect.Width, rect.Height );
    }



    public void SetClip( int x, int y, int width, int height, int offsetX = 0, int offsetY = 0 )
    {
      // TODO - clip against control bounds
      _DisplayOffsetX = -offsetX - _SubAreaDisplayOffsetX;
      _DisplayOffsetY = -offsetY - _SubAreaDisplayOffsetY;
      _SubAreaDisplayOffsetX = -offsetX;
      _SubAreaDisplayOffsetY = -offsetY;
      _G.SetClip( new Rectangle( x + offsetX, y + offsetY, width, height ) );
    }



    public GR.Math.Rectangle GetClipRect()
    {
      var bounds = _G.ClipBounds;
      return new GR.Math.Rectangle( (int)_G.ClipBounds.X - _DisplayOffsetX, (int)_G.ClipBounds.Y - _DisplayOffsetY, (int)_G.ClipBounds.Width, (int)_G.ClipBounds.Height );
    }



    internal void RestoreClip()
    {
      _DisplayOffsetX -= _SubAreaDisplayOffsetX;
      _DisplayOffsetY -= _SubAreaDisplayOffsetY;
      _SubAreaDisplayOffsetX = 0;
      _SubAreaDisplayOffsetY = 0;

      _G.SetClip( _OriginalBounds );
    }



    internal void RenderListControl()
    {
      var listControl = (ListControl)_Control;  

      // show header
      var oldClip = GetClipRect();
      for ( int i = 0; i < listControl.Columns.Count; ++i )
      {
        var rc = listControl.GetHeaderRect( i );

        SetClip( rc );

        FillRaisedRectangle( rc.Left, rc.Top, rc.Width, rc.Height, ColorControlBackground );

        if ( listControl.Font != null )
        {
          DrawText( listControl.Columns[i].Name, rc.Left, rc.Top, rc.Width, rc.Height, listControl.Columns[i].Alignment | DecentForms.TextAlignment.CENTERED_V,
                    ColorControlText );
        }
      }
      SetClip( oldClip );

      if ( listControl.Columns.Count > 0 )
      {
        int iItem = listControl.FirstVisibleItemIndex;

        bool bDone = false;

        Rectangle rcItem;

        if ( listControl.Items.Count > 0 )
        {
          do
          {
            for ( int column = 0; column < listControl.Columns.Count; ++column )
            {
              if ( !listControl.GetItemRect( iItem, column, out rcItem ) )
              {
                bDone = true;
                break;
              }
              if ( listControl.Font != null )
              {
                uint color = ColorControlText;
                if ( listControl.Items[iItem].Selected )
                {
                  color = ColorControlTextSelected;
                  FillRectangle( rcItem, ColorControlBackgroundSelected );
                }
                else if ( listControl.MouseOverItem == iItem )
                {
                  color = ColorControlTextMouseOver;
                  FillRectangle( rcItem, ColorControlBackgroundMouseOver );
                }
                DrawText( listControl.Items[iItem].SubItems[column].Text, rcItem.Left, rcItem.Top, rcItem.Width, rcItem.Height,
                          listControl.Columns[column].Alignment,
                          color );
              }
            }
            ++iItem;
          }
          while ( !bDone );
        }
      }
    }



    /*
    internal void RenderGroupBox( string Text )
    {
      var groupBox = (GroupBox)_Control;

      var clientRect = groupBox.ClientRectangle;
      clientRect.Inflate( -8, -8 );

      DrawRectangle( clientRect.Left, clientRect.Top, clientRect.Width, clientRect.Height, ColorControlBackground );

      if ( !groupBox.HasCheckBox )
      {
        var textRect = groupBox.GetTextRect();
        FillRectangle( textRect.Left, textRect.Top, textRect.Width, textRect.Height, ColorControlInActiveBackground );
        DrawText( Text, textRect.Left, textRect.Top, textRect.Width, textRect.Height, TextAlignment.CENTERED_H, ColorControlText );
      }
    }
    */



  }
}
