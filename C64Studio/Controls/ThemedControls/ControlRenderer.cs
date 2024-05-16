using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
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

    private static Dictionary<System.Drawing.Image,System.Drawing.Image>    _GrayscaledImageCache = new Dictionary<System.Drawing.Image, System.Drawing.Image>();



    public ControlRenderer( Graphics G, ControlBase Control )
    {
      _G = G;
      _Control = Control;

      _DisplayOffsetX = Control.DisplayOffsetX;
      _DisplayOffsetY = Control.DisplayOffsetY;
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



    public void DrawRectangle( int X, int Y, int Width, int Height, uint BaseColor )
    {
      var   borderPen       = ColoredPen( BaseColor );

      X -= _DisplayOffsetX;
      Y -= _DisplayOffsetY;

      _G.DrawRectangle( borderPen, X, Y, Width - 1, Height - 1 );
    }



    public void FillRectangle( int X, int Y, int Width, int Height, uint BaseColor )
    {
      var   fillBrush = new SolidBrush( Color.FromArgb( (int)BaseColor ) );

      _G.FillRectangle( fillBrush, X, Y, Width, Height );
    }



    private Pen ColoredPen( uint BaseColor )
    {
      return new Pen( ToColor( BaseColor ) );
    }



    public void DrawText( string Text, int BoundsX, int BoundsY, int Width, int Height, TextAlignment Alignment, int DX = 0, int DY = 0 )
    {
      BoundsX -= _DisplayOffsetX;
      BoundsY -= _DisplayOffsetY;

      TextRenderer.DrawText( _G, Text, _Control.Font, new Rectangle( BoundsX + DX, BoundsY + DY, Width, Height ), ToColor( ColorControlText ), MapAlignmentToFlags( Alignment ) | TextFormatFlags.PreserveGraphicsClipping );
    }



    public void DrawText( string Text, int BoundsX, int BoundsY, int Width, int Height, TextAlignment Alignment, uint Color )
    {
      BoundsX -= _DisplayOffsetX;
      BoundsY -= _DisplayOffsetY;

      TextRenderer.DrawText( _G, Text, _Control.Font, new Rectangle( BoundsX, BoundsY, Width, Height ), ToColor( Color ), MapAlignmentToFlags( Alignment ) | TextFormatFlags.PreserveGraphicsClipping );
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



    internal void RenderButton( bool MouseOver, bool Pushed, bool IsDefault, Button.ButtonStyle Style )
    {
      RenderButton( MouseOver, Pushed, IsDefault, Style, new Rectangle( 0, 0, _Control.Width, _Control.Height ) );
    }



    internal void RenderButton( bool MouseOver, bool Pushed, bool IsDefault, Button.ButtonStyle Style, Rectangle Rect )
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
            FillRaisedRectangle( Rect.Left, Rect.Top, Rect.Width, Rect.Height, ColorControlBackground );
            if ( imageToDraw != null )
            {
              DrawImageCentered( imageToDraw, Rect );
            }
            DrawDisabledText( _Control.Text, Rect.Left, Rect.Top, Rect.Width, Rect.Height, TextAlignment.CENTERED );
          }
          else if ( Pushed )
          {
            FillSunkenRectangle( Rect.Left, Rect.Top, Rect.Width, Rect.Height, ColorControlBackgroundSelected );
            if ( imageToDraw != null )
            {
              DrawImageCentered( imageToDraw, Rect, 0, 1 );
            }
            DrawText( _Control.Text, Rect.Left, Rect.Top, Rect.Width, Rect.Height, TextAlignment.CENTERED, 0, 1 );
          }
          else if ( MouseOver )
          {
            DrawRaisedRectangle( Rect.Left, Rect.Top, Rect.Width, Rect.Height, ColorControlBackground );
            FillRectangle( Rect.Left + 2, Rect.Top + 2, Rect.Width - 4, Rect.Height - 4, ColorControlBackgroundMouseOver );
            if ( imageToDraw != null )
            {
              DrawImageCentered( imageToDraw, Rect );
            }
            DrawText( _Control.Text, Rect.Left, Rect.Top, Rect.Width, Rect.Height, TextAlignment.CENTERED );
          }
          else
          {
            FillRaisedRectangle( Rect.Left, Rect.Top, Rect.Width, Rect.Height, ColorControlBackground );
            if ( imageToDraw != null )
            {
              DrawImageCentered( imageToDraw, Rect );
            }
            DrawText( _Control.Text, Rect.Left, Rect.Top, Rect.Width, Rect.Height, TextAlignment.CENTERED );
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
            DrawDisabledText( _Control.Text, Rect.Left, Rect.Top, Rect.Width, Rect.Height, TextAlignment.CENTERED );
          }
          else if ( Pushed )
          {
            DrawRectangle( Rect.Left, Rect.Top, Rect.Width, Rect.Height, DarkenColor( ColorControlBackgroundSelected ) );
            FillRectangle( Rect.Left + 1, Rect.Top + 1, Rect.Width - 2, Rect.Height - 2, ColorControlBackgroundSelected );
            if ( imageToDraw != null )
            {
              DrawImageCentered( imageToDraw, Rect, 0, 1 );
            }
            DrawText( _Control.Text, Rect.Left, Rect.Top, Rect.Width, Rect.Height, TextAlignment.CENTERED, 0, 1 );
          }
          else if ( MouseOver )
          {
            DrawRectangle( Rect.Left, Rect.Top, Rect.Width, Rect.Height, DarkenColor( ColorControlBackgroundMouseOver ) );
            FillRectangle( Rect.Left + 1, Rect.Top + 1, Rect.Width - 2, Rect.Height - 2, ColorControlBackgroundMouseOver );
            if ( imageToDraw != null )
            {
              DrawImageCentered( imageToDraw, Rect );
            }
            DrawText( _Control.Text, Rect.Left, Rect.Top, Rect.Width, Rect.Height, TextAlignment.CENTERED );
          }
          else
          {
            DrawRectangle( Rect.Left, Rect.Top, Rect.Width, Rect.Height, DarkenColor( ColorControlBackground ) );
            FillRectangle( Rect.Left + 1, Rect.Top + 1, Rect.Width - 2, Rect.Height - 2, ColorControlBackground );
            if ( imageToDraw != null )
            {
              DrawImageCentered( imageToDraw, Rect );
            }
            DrawText( _Control.Text, Rect.Left, Rect.Top, Rect.Width, Rect.Height, TextAlignment.CENTERED );
          }
          if ( IsDefault )
          {
            DrawRectangle( Rect.Left, Rect.Top, Rect.Width, Rect.Height, LightenColor( ColorControlText ) );
          }
          break;
      }

      if ( _Control.Focused )
      {
        DrawFocusRect( Rect.Left + 3, Rect.Top + 3, Rect.Width - 6, Rect.Height - 6, ColorControlText );
      }
    }



    private System.Drawing.Image GetGrayScaleBitmap( System.Drawing.Image Image )
    {
      if ( _GrayscaledImageCache.TryGetValue( Image, out System.Drawing.Image cachedImage ) )
      {
        return cachedImage;
      }

      var newBitmap = new Bitmap( Image.Width, Image.Height );

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
      }
      _GrayscaledImageCache.Add( Image, newBitmap );

      return newBitmap;
    }



    internal void RenderRadioButton( string Text, ContentAlignment Alignment, bool MouseOver, bool Pushed, bool Checked )
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



    internal void RenderSlider( int X, int Y, int Width, int Height, bool MouseOver, bool Pushed )
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


  
    /*
    internal void RenderListBox()
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



    internal void RenderTreeView()
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

        var rect = node.Bounds;

        // expand toggle
        if ( node.Nodes.Count > 0 )
        {
          var toggleRect = treeView.GetToggleRect( node );

          DrawTreeViewExpansionToggle( node.IsExpanded, toggleRect );
        }
        if ( ( treeView.ImageList != null )
        &&   ( treeView.ImageList.Images.Count > 0 ) )
        {
          var imageRect = treeView.GetImageRect( node );

          int imageIndex = node.ImageIndex;
          if ( ( imageIndex < 0 )
          ||   ( imageIndex >= treeView.ImageList.Images.Count ) )
          {
            imageIndex = 0;
          }
          DrawImage( treeView.ImageList.Images[imageIndex], imageRect );
        }


        if ( node == treeView.SelectedNode )
        {
          FillRectangle( rect.Left, rect.Top, rect.Width, rect.Height, ColorControlBackgroundSelected );

          DrawText( node.Text, rect.Left, rect.Top, rect.Width, rect.Height, TextAlignment.LEFT, ColorControlTextSelected );
          DrawFocusRect( rect.Left, rect.Top, rect.Width, rect.Height, ColorControlText );
        }
        else if ( node == treeView.MouseOverNode )
        {
          FillRectangle( rect.Left, rect.Top, rect.Width, rect.Height, ColorControlBackgroundMouseOver );
          DrawText( node.Text, rect.Left, rect.Top, rect.Width, rect.Height, TextAlignment.LEFT, ColorControlTextMouseOver );
        }
        else
        {
          DrawText( node.Text, rect.Left, rect.Top, rect.Width, rect.Height, TextAlignment.LEFT );
        }

        node = TreeView.GetNextVisibleNode( node );
      }
    }
    */



    private void DrawImage( System.Drawing.Image Image, Rectangle ImageRect )
    {
      _G.DrawImage( Image, ImageRect.X, ImageRect.Y, Image.Width, Image.Height );
    }



    private void DrawImageCentered( System.Drawing.Image Image, Rectangle ImageRect, int DX = 0, int DY = 0 )
    {
      _G.DrawImage( Image, 
                    ImageRect.X + ( ImageRect.Width - Image.Width ) / 2 + DX,
                    ImageRect.Y + ( ImageRect.Height - Image.Height ) / 2 + DY,
                    Image.Width, Image.Height );
    }



    private void DrawTreeViewExpansionToggle( bool IsExpanded, Rectangle Rect )
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
      uint    color = ( MouseOver ? ColorControlBackgroundMouseOver: ColorControlBorderFlat );
      DrawRectangle( CheckRect.Left, CheckRect.Top, CheckRect.Width, CheckRect.Height, color );
      FillRectangle( CheckRect.Left + 1, CheckRect.Top + 1, CheckRect.Width - 2, CheckRect.Height - 2, ColorControlActiveBackground );
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
