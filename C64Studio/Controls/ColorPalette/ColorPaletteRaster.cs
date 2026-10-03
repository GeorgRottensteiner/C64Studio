using GR.Generic;
using RetroDevStudio;
using RetroDevStudio.Formats;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RetroDevStudio.Controls
{
  public partial class ColorPaletteRaster : ColorPaletteBase
  {
    private int     _NumColorsPerLine = 1;
    private int     _NumColorsPerColumn = 1;



    public ColorPaletteRaster() :
      base( null, null, null, 0 )
    { 
    }



    public ColorPaletteRaster( StudioCore Core, ColorSettings colors, List<ColorEntry> colorIndices, byte selectedColor, int numColorsPerLine = 1 ) :
      base( Core, colors, colorIndices, selectedColor )
    {
      _NumColorsPerLine = numColorsPerLine;
      _NumColorsPerColumn = ( _ColorIndices.Count + _NumColorsPerLine - 1 ) / _NumColorsPerLine;

      InitializeComponent();

      _Colors.Palette             = colors.Palette;
      _Colors.PaletteOffset       = colors.PaletteOffset;
      _Colors.PaletteMappingIndex = colors.PaletteMappingIndex;
      int colorEntrySizeW = 16;
      int colorEntrySizeH = 16;
      if ( numColorsPerLine > 2 )
      {
        colorEntrySizeW /= 2;
        colorEntrySizeH /= 2;
      }

      var newSize = new Size( 2 * SystemInformation.Border3DSize.Width + colorEntrySizeW * _NumColorsPerLine,
                              2 * SystemInformation.Border3DSize.Height + colorEntrySizeH * _NumColorsPerColumn );
      Size = newSize;
      panelCharColors.Size = newSize;
      panelCharColors.DisplayPage.Create( 8 * _NumColorsPerLine, 8 * _NumColorsPerColumn, GR.Drawing.PixelFormat.Format32bppRgb );
    }



    public override void Redraw()
    {
      for ( int i = 0; i < _ColorIndices.Count; ++i )
      {
        int x = i / _NumColorsPerColumn;
        int y = i % _NumColorsPerColumn;

        panelCharColors.DisplayPage.Box( x * 8, y * 8, 8, 8, _Colors.Palette.ColorValues[_Colors.PaletteOffset + _ColorIndices[i].ColorIndex] );
      }
      panelCharColors.Invalidate();
    }



    private void panelCharColors_PostPaint( GR.Image.FastImage TargetBuffer )
    {
      int x = _SelectedColor / _NumColorsPerColumn;
      int y = _SelectedColor % _NumColorsPerColumn;

      int x1 = TargetBuffer.Width * x / _NumColorsPerLine;
      int y1 = TargetBuffer.Height * y / _NumColorsPerColumn;
      int x2 = TargetBuffer.Width * ( x + 1 ) / _NumColorsPerLine;
      int y2 = TargetBuffer.Height * ( y + 1 ) / _NumColorsPerColumn;

      if ( Core != null )
      {
        uint  selColor = Core.Settings.FGColor( ColorableElement.SELECTION_FRAME );

        TargetBuffer.Rectangle( x1, y1, x2 - x1, y2 - y1, selColor );
        TargetBuffer.Rectangle( x1 + 1, y1 + 1, x2 - x1 - 2, y2 - y1 - 2, 0 );
      }
    }



    private void panelCharColors_MouseDown( object sender, MouseEventArgs e )
    {
      HandleMouseOnColorChooser( e.X, e.Y, e.Button );
    }



    private void panelCharColors_MouseMove( object sender, MouseEventArgs e )
    {
      HandleMouseOnColorChooser( e.X, e.Y, e.Button );
    }



    private void HandleMouseOnColorChooser( int X, int Y, MouseButtons Buttons )
    {
      if ( ( X < 0 )
      ||   ( X >= panelCharColors.ClientSize.Width )
      ||   ( Y < 0 )
      ||   ( Y >= panelCharColors.ClientSize.Height ) )
      {
        return;
      }

      if ( ( Buttons & MouseButtons.Left ) == MouseButtons.Left )
      {
        int colorIndexX = X / ( panelCharColors.ClientSize.Width / _NumColorsPerLine );
        int colorIndexY = Y / ( panelCharColors.ClientSize.Height / _NumColorsPerColumn );

        _SelectedColor = (byte)( colorIndexX * _NumColorsPerColumn + colorIndexY );
        _LastSelectedEntry = _ColorIndices[_SelectedColor];
        Redraw();
        RaiseColorSelectedEvent();
      }
    }



    public override void UpdatePalette( GR.Image.Palette palette )
    {
      _Colors.Palette = palette;
      Redraw();
    }



  }
}
