using RetroDevStudio;
using RetroDevStudio.Dialogs;
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
  public partial class ColorPickerGraphicFullPalette : ColorPickerGraphicBase
  {
    public ColorPickerGraphicFullPalette() :
      base( null, 1 )
    { 
    }



    public ColorPickerGraphicFullPalette( StudioCore Core, byte CustomColor ) :
      base( Core, CustomColor )
    {
      InitializeComponent();

      _Colors.Palette = Core.Imaging.PaletteFromMachine( MachineType.MEGA65 );
      panelCharColors.DisplayPage.Create( 128, 64, GR.Drawing.PixelFormat.Format32bppRgb );
    }



    public override void Redraw()
    {
      for ( byte i = 0; i < 16; ++i )
      {
        for ( byte j = 0; j < 16; ++j )
        {
          panelCharColors.DisplayPage.Box( i * 8, j * 4, 8, 4, _Colors.Palette.ColorValues[i + j * 16] );
        }
      }
      panelCharColors.Invalidate();
    }



    private void panelCharColors_PostPaint( GR.Image.FastImage TargetBuffer )
    {
      int     x1 = ( SelectedColor % 16 ) * TargetBuffer.Width / 16;
      int     x2 = ( ( SelectedColor % 16 ) + 1 ) * TargetBuffer.Width / 16;
      int     y1 = ( SelectedColor / 16 ) * TargetBuffer.Height / 16;
      int     y2 = ( ( SelectedColor / 16 ) + 1 ) * TargetBuffer.Height / 16;

      if ( Core != null )
      {
        uint  selColor = Core.Settings.FGColor( ColorableElement.SELECTION_FRAME );

        TargetBuffer.Rectangle( x1, y1, x2 - x1, y2 - y1, selColor );
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
        int colorIndex = (int)( ( 16 * X ) / panelCharColors.ClientSize.Width );
        int colorIndexY = (int)( ( 16 * Y ) / panelCharColors.ClientSize.Height );
        SelectedColor = (byte)( colorIndex + 16 * colorIndexY );
        Redraw();
        RaiseColorSelectedEvent();
      }
    }



    public override void UpdatePalette( Palette palette )
    {
      _Colors.Palette = palette;
      Redraw();
    }



    private void btnEditPalette_Click( DecentForms.ControlBase Sender )
    {
      var dlgPalette = new DlgPaletteEditor( Core, _Colors );
      if ( dlgPalette.ShowDialog() == DialogResult.OK )
      {
        UpdatePalette( dlgPalette.Colors.Palette );
        RaisePaletteModifiedEvent( _Colors.Palette );
      }
    }



  }
}
