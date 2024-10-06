using GR.Forms;
using RetroDevStudio.Dialogs;
using RetroDevStudio.Types;
using System;
using System.Collections.Generic;
using System.Windows.Forms;



namespace RetroDevStudio.Controls
{
  public partial class ColorSettingsNES : ColorSettingsBase
  {
    private List<FastPictureBox>    _Palettes = new List<FastPictureBox>();

    // A to D
    private int                     _PaletteMappingGroup = 0;        



    public override byte SelectedCustomColor
    {
      get
      {
        return _SelectedCustomColor;
      }
      set
      {
        _SelectedCustomColor = value;

        _Palettes[Colors.PaletteMappingIndex % 4].Invalidate();
      }
    }



    public override ColorType SelectedColor
    {
      get
      {
        return _CurrentColorType;
      }
      set
      {
        _CurrentColorType = ColorType.CUSTOM_COLOR;
      }
    }



    public ColorSettingsNES() :
      base( null, null, 0 )
    { 
    }



    public ColorSettingsNES( StudioCore Core, ColorSettings Colors, byte CustomColor ) :
      base( Core, Colors, CustomColor )
    {
      InitializeComponent();

      _Palettes.Add( picPalette1 );
      _Palettes.Add( picPalette2 );
      _Palettes.Add( picPalette3 );
      _Palettes.Add( picPalette4 );

      picPalette1.DisplayPage.Create( 64, 16, GR.Drawing.PixelFormat.Format32bppRgb );
      picPalette1.Tag = 0;
      picPalette2.DisplayPage.Create( 64, 16, GR.Drawing.PixelFormat.Format32bppRgb );
      picPalette2.Tag = 1;
      picPalette3.DisplayPage.Create( 64, 16, GR.Drawing.PixelFormat.Format32bppRgb );
      picPalette3.Tag = 2;
      picPalette4.DisplayPage.Create( 64, 16, GR.Drawing.PixelFormat.Format32bppRgb );
      picPalette4.Tag = 3;
      picFullPalette.DisplayPage.Create( 64, 16, GR.Drawing.PixelFormat.Format32bppRgb );

      RedrawPaletteGroups();
      for ( int i = 0; i < 4; ++i )
      {
        for ( int j = 0; j < 16; ++j )
        {
          picFullPalette.DisplayPage.Box( j * 4, i * 4, 4, 4, Colors.Palette.ColorValues[i * 16 + j] );
        }
      }
    }



    private void RedrawPaletteGroups()
    {
      for ( int i = 0; i < 4; ++i )
      {
        picPalette1.DisplayPage.Box( i * 16, 0, 16, 16, Colors.Palette.ColorValues[Colors.PaletteIndexMapping[_PaletteMappingGroup * 4 + 0][i]] );
        picPalette2.DisplayPage.Box( i * 16, 0, 16, 16, Colors.Palette.ColorValues[Colors.PaletteIndexMapping[_PaletteMappingGroup * 4 + 1][i]] );
        picPalette3.DisplayPage.Box( i * 16, 0, 16, 16, Colors.Palette.ColorValues[Colors.PaletteIndexMapping[_PaletteMappingGroup * 4 + 2][i]] );
        picPalette4.DisplayPage.Box( i * 16, 0, 16, 16, Colors.Palette.ColorValues[Colors.PaletteIndexMapping[_PaletteMappingGroup * 4 + 3][i]] );
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
      if ( !( (Control)sender  ).Focused )
      {
        buttons = 0;
      }
      HandleMouseOnPaletteChooser( e.X, e.Y, buttons, picPalette1 );
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
        int palIndex = _PaletteMappingGroup * 4 + (int)Control.Tag;

        if ( Colors.PaletteMappingIndex != palIndex )
        {
          int   oldIndex = _PaletteMappingGroup * 4 + Colors.PaletteMappingIndex;
          Colors.PaletteMappingIndex = palIndex;
          picFullPalette.Invalidate();
          _Palettes[oldIndex % 4].Invalidate();
          _Palettes[Colors.PaletteMappingIndex % 4].Invalidate();
          RaisePaletteMappingModifiedEvent();
        }
        byte    newColor = (byte)( ( 4 * X ) / Control.ClientSize.Width );
        if ( SelectedCustomColor != newColor )
        {
          SelectedCustomColor = newColor;
          picFullPalette.Invalidate();
          RaiseColorSelectedEvent();
        }
        if ( CustomColor != newColor )
        {
          CustomColor = newColor;
          _Palettes[Colors.PaletteMappingIndex % 4].Invalidate();
          RaiseColorsModifiedEvent( ColorType.CUSTOM_COLOR );
        }
      }
    }



    private void PalettePostPaint( Control Control, GR.Image.FastImage TargetBuffer )
    {
      if ( Core != null )
      {
        int palIndex =_PaletteMappingGroup * 4 + (int)Control.Tag;
        if ( palIndex == Colors.PaletteMappingIndex )
        {
          uint  selColor = Core.Settings.FGColor( ColorableElement.SELECTION_FRAME );
          int   x1 = _SelectedCustomColor * TargetBuffer.Width / 4;
          int   x2 = ( _SelectedCustomColor + 1 ) * TargetBuffer.Width / 4;

          TargetBuffer.Rectangle( x1, 0, x2 - x1, TargetBuffer.Height, selColor );
        }
      }
    }



    private void picPalette1_PostPaint( GR.Image.FastImage TargetBuffer )
    {
      PalettePostPaint( picPalette1, TargetBuffer );
    }



    private void picPalette2_PostPaint( GR.Image.FastImage TargetBuffer )
    {
      PalettePostPaint( picPalette2, TargetBuffer );
    }



    private void picPalette3_PostPaint( GR.Image.FastImage TargetBuffer )
    {
      PalettePostPaint( picPalette3, TargetBuffer );
    }



    private void picPalette4_PostPaint( GR.Image.FastImage TargetBuffer )
    {
      PalettePostPaint( picPalette4, TargetBuffer );
    }



    private void picFullPalette_MouseDown( object sender, MouseEventArgs e )
    {
      HandleMouseOnFullPalette( e.X, e.Y, e.Button );
    }



    private void picFullPalette_MouseMove( object sender, MouseEventArgs e )
    {
      MouseButtons    buttons = e.Button;
      if ( !picFullPalette.Focused )
      {
        buttons = 0;
      }
      HandleMouseOnFullPalette( e.X, e.Y, buttons );
    }



    private void HandleMouseOnFullPalette( int X, int Y, MouseButtons Buttons )
    {
      if ( !picFullPalette.ClientRectangle.Contains( X, Y ) )
      {
        return;
      }
      if ( ( Buttons & MouseButtons.Left ) == MouseButtons.Left )
      {
        // there has to be a better way to get this exact
        int     x = 0;
        for ( int i = 0; i < 16; ++i )
        {
          int   x1 = i * picFullPalette.ClientSize.Width / 16;
          int   x2 = ( i + 1 ) * picFullPalette.ClientSize.Width / 16;

          if ( ( X >= x1 )
          &&   ( X <= x2 ) )
          {
            x = i;
            break;
          }
        }
        int     y = 0;
        for ( int i = 0; i < 4; ++i )
        {
          int   y1 = i * picFullPalette.ClientSize.Height / 4;
          int   y2 = ( i + 1 ) * picFullPalette.ClientSize.Height / 4 - 1;

          if ( ( Y >= y1 )
          &&   ( Y <= y2 ) )
          {
            y = i;
            break;
          }
        }

        int colIndex = x + y * 16;

        if ( Colors.PaletteIndexMapping[Colors.PaletteMappingIndex][_SelectedCustomColor] != colIndex )
        {
          if ( ( _SelectedCustomColor % 4 ) == 0 )
          {
            // 0 is shared!
            for ( int i = 0; i < 4; ++i )
            {
              Colors.PaletteIndexMapping[_PaletteMappingGroup * 4 + i][0] = colIndex;
              _Palettes[i].Invalidate();
              _Palettes[i].DisplayPage.Box( 0 * 16, 0, 16, 16, Colors.Palette.ColorValues[Colors.PaletteIndexMapping[i][0]] );
            }
          }
          else
          {
            Colors.PaletteIndexMapping[Colors.PaletteMappingIndex][_SelectedCustomColor] = colIndex;
            _Palettes[Colors.PaletteMappingIndex % 4].Invalidate();
            _Palettes[Colors.PaletteMappingIndex % 4].DisplayPage.Box( _SelectedCustomColor * 16, 0, 16, 16,
                                                                   Colors.Palette.ColorValues[Colors.PaletteIndexMapping[Colors.PaletteMappingIndex][_SelectedCustomColor]] );
          }

          picFullPalette.Invalidate();
          RaisePaletteMappingModifiedEvent();
        }
      }
    }



    private void picFullPalette_PostPaint( GR.Image.FastImage TargetBuffer )
    {
      if ( Core != null )
      {
        uint  selColor = Core.Settings.FGColor( ColorableElement.SELECTION_FRAME );
        int   colIndex = Colors.PaletteIndexMapping[Colors.PaletteMappingIndex][_SelectedCustomColor];
        int   x1 = ( colIndex % 16 ) * TargetBuffer.Width / 16;
        int   x2 = ( ( colIndex % 16 ) + 1 ) * TargetBuffer.Width / 16;
        int   y1 = ( colIndex / 16 ) * TargetBuffer.Height / 4;
        int   y2 = ( ( colIndex / 16 ) + 1 ) * TargetBuffer.Height / 4 - 1;

        TargetBuffer.Rectangle( x1, y1, x2 - x1 + 1, y2 - y1 + 1, selColor );
      }
    }



    private void checkPaletteSet1_CheckedChanged( DecentForms.ControlBase Sender )
    {
      if ( checkPaletteSet1.Checked )
      {
        checkPaletteSet2.Checked = false;
        checkPaletteSet2.Enabled = true;
        checkPaletteSet3.Checked = false;
        checkPaletteSet3.Enabled = true;
        checkPaletteSet4.Checked = false;
        checkPaletteSet4.Enabled = true;

        checkPaletteSet1.Enabled = false;
        _PaletteMappingGroup = 0;
        Colors.PaletteMappingIndex = _PaletteMappingGroup * 4 + ( Colors.PaletteMappingIndex % 4 );
        picFullPalette.Invalidate();
        RaisePaletteMappingModifiedEvent();
        RedrawPaletteGroups();
        RaisePaletteMappingModifiedEvent();
      }
    }



    private void checkPaletteSet2_CheckedChanged( DecentForms.ControlBase Sender )
    {
      if ( checkPaletteSet2.Checked )
      {
        checkPaletteSet1.Checked = false;
        checkPaletteSet1.Enabled = true;
        checkPaletteSet3.Checked = false;
        checkPaletteSet3.Enabled = true;
        checkPaletteSet4.Checked = false;
        checkPaletteSet4.Enabled = true;

        checkPaletteSet2.Enabled = false;
        _PaletteMappingGroup = 1;
        Colors.PaletteMappingIndex = _PaletteMappingGroup * 4 + ( Colors.PaletteMappingIndex % 4 );
        picFullPalette.Invalidate();
        RaisePaletteMappingModifiedEvent();
        RedrawPaletteGroups();
        RaisePaletteMappingModifiedEvent();
      }
    }



    private void checkPaletteSet3_CheckedChanged( DecentForms.ControlBase Sender )
    {
      if ( checkPaletteSet3.Checked )
      {
        checkPaletteSet2.Checked = false;
        checkPaletteSet2.Enabled = true;
        checkPaletteSet1.Checked = false;
        checkPaletteSet1.Enabled = true;
        checkPaletteSet4.Checked = false;
        checkPaletteSet4.Enabled = true;

        checkPaletteSet3.Enabled = false;
        _PaletteMappingGroup = 2;
        Colors.PaletteMappingIndex = _PaletteMappingGroup * 4 + ( Colors.PaletteMappingIndex % 4 );
        picFullPalette.Invalidate();
        RaisePaletteMappingModifiedEvent();
        RedrawPaletteGroups();
        RaisePaletteMappingModifiedEvent();
      }
    }



    private void checkPaletteSet4_CheckedChanged( DecentForms.ControlBase Sender )
    {
      if ( checkPaletteSet4.Checked )
      {
        checkPaletteSet2.Checked = false;
        checkPaletteSet2.Enabled = true;
        checkPaletteSet3.Checked = false;
        checkPaletteSet3.Enabled = true;
        checkPaletteSet1.Checked = false;
        checkPaletteSet1.Enabled = true;

        checkPaletteSet4.Enabled = false;
        _PaletteMappingGroup = 3;
        Colors.PaletteMappingIndex = _PaletteMappingGroup * 4 + ( Colors.PaletteMappingIndex % 4 );
        picFullPalette.Invalidate();
        RaisePaletteMappingModifiedEvent();

        RedrawPaletteGroups();
        RaisePaletteMappingModifiedEvent();
      }
    }



  }
}
