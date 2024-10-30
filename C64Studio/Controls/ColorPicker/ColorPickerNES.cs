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
using GR.Forms;



namespace RetroDevStudio.Controls
{
  public partial class ColorPickerNES : ColorPickerBase
  {
    private List<FastPictureBox>    _Palettes = new List<FastPictureBox>();



    public ColorPickerNES() :
      base( null, null, 0, 1 )
    { 
    }



    public ColorPickerNES( StudioCore Core, CharsetProject Charset, ushort CurrentChar, byte CustomColor ) :
      base( Core, Charset, CurrentChar, CustomColor )
    {
      _Charset = Charset;

      InitializeComponent();

      _Palettes.Add( picPalette1 );
      _Palettes.Add( picPalette2 );
      _Palettes.Add( picPalette3 );
      _Palettes.Add( picPalette4 );

      picPalette1.DisplayPage.Create( 5 * 8, 8, GR.Drawing.PixelFormat.Format32bppRgb );
      picPalette1.Tag = 0;
      picPalette2.DisplayPage.Create( 5 * 8, 8, GR.Drawing.PixelFormat.Format32bppRgb );
      picPalette2.Tag = 1;
      picPalette3.DisplayPage.Create( 5 * 8, 8, GR.Drawing.PixelFormat.Format32bppRgb );
      picPalette3.Tag = 2;
      picPalette4.DisplayPage.Create( 5 * 8, 8, GR.Drawing.PixelFormat.Format32bppRgb );
      picPalette4.Tag = 3;
      RedrawPaletteGroups();
    }



    private void RedrawPaletteGroups()
    {
      for ( int i = 0; i < 4; ++i )
      {
        picPalette1.DisplayPage.Box( 8 + i * 8, 0, 8, 8, _Charset.Colors.Palette.ColorValues[_Charset.Colors.PaletteIndexMapping[_Charset.Colors.PaletteMappingIndex * 4 + 0][i]] );
        picPalette2.DisplayPage.Box( 8 + i * 8, 0, 8, 8, _Charset.Colors.Palette.ColorValues[_Charset.Colors.PaletteIndexMapping[_Charset.Colors.PaletteMappingIndex * 4 + 1][i]] );
        picPalette3.DisplayPage.Box( 8 + i * 8, 0, 8, 8, _Charset.Colors.Palette.ColorValues[_Charset.Colors.PaletteIndexMapping[_Charset.Colors.PaletteMappingIndex * 4 + 2][i]] );
        picPalette4.DisplayPage.Box( 8 + i * 8, 0, 8, 8, _Charset.Colors.Palette.ColorValues[_Charset.Colors.PaletteIndexMapping[_Charset.Colors.PaletteMappingIndex * 4 + 3][i]] );
      }
      picPalette1.Invalidate();
      picPalette2.Invalidate();
      picPalette3.Invalidate();
      picPalette4.Invalidate();
    }



    public override void Redraw()
    {
      if ( _Charset == null )
      {
        return;
      }
      for ( int i = 0; i < 4; ++i )
      {
        var altColors = new AlternativeColorSettings( _Charset.Colors );
        altColors.PaletteMappingIndex = i;
        Displayer.CharacterDisplayer.DisplayChar( _Charset, SelectedChar, _Palettes[i].DisplayPage, 0, 0, altColors );
      }
      picPalette1.Invalidate();
      picPalette2.Invalidate();
      picPalette3.Invalidate();
      picPalette4.Invalidate();
    }



    private void picPalette_MouseDown( object sender, MouseEventArgs e )
    {
      HandleMouseOnPaletteChooser( e.X, e.Y, e.Button, (Control)sender );
    }



    private void picPalette_MouseMove( object sender, MouseEventArgs e )
    {
      MouseButtons    buttons = e.Button;
      if ( !( (Control)sender ).Focused )
      {
        buttons = 0;
      }
      HandleMouseOnPaletteChooser( e.X, e.Y, buttons, (Control)sender );
    }



    private void HandleMouseOnPaletteChooser( int X, int Y, MouseButtons Buttons, Control Control )
    {
      if ( ( X < 0 )
      ||   ( X >= Control.ClientSize.Width ) )
      {
        return;
      }
      if ( ( Buttons & MouseButtons.Left ) == MouseButtons.Left )
      {
        int palIndex = (int)Control.Tag;

        if ( SelectedPaletteMapping != palIndex )
        {
          int   oldIndex = SelectedPaletteMapping;
          SelectedPaletteMapping = palIndex;
          _Palettes[oldIndex].Invalidate();
          _Palettes[SelectedPaletteMapping].Invalidate();
          RaisePaletteMappingSelectedEvent();
        }
      }
    }



    private void picPalette_PostPaint( Control Control, GR.Image.FastImage TargetBuffer )
    {
      if ( Core != null )
      {
        int palIndex =(int)Control.Tag;
        if ( palIndex == SelectedPaletteMapping )
        {
          uint  selColor = Core.Settings.FGColor( ColorableElement.SELECTION_FRAME );
          TargetBuffer.Rectangle( 0, 0, TargetBuffer.Width, TargetBuffer.Height, selColor );
        }
      }
    }



    private void picPalette1_PostPaint( GR.Image.FastImage TargetBuffer )
    {
      picPalette_PostPaint( picPalette1, TargetBuffer );
    }



    private void picPalette2_PostPaint( GR.Image.FastImage TargetBuffer )
    {
      picPalette_PostPaint( picPalette2, TargetBuffer );
    }



    private void picPalette3_PostPaint( GR.Image.FastImage TargetBuffer )
    {
      picPalette_PostPaint( picPalette3, TargetBuffer );
    }



    private void picPalette4_PostPaint( GR.Image.FastImage TargetBuffer )
    {
      picPalette_PostPaint( picPalette4, TargetBuffer );
    }



  }
}
