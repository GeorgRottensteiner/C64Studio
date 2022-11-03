using RetroDevStudio.Dialogs;
using RetroDevStudio.Types;
using System;
using System.Windows.Forms;



namespace RetroDevStudio.Controls
{
  public partial class ColorSettingsX16Sprites16 : ColorSettingsBase
  {
    public override byte SelectedCustomColor
    {
      get
      {
        return _SelectedCustomColor;
      }
      set
      {
        _SelectedCustomColor = value;
        if ( comboCharColor != null )
        {
          comboCharColor.SelectedIndex = _SelectedCustomColor;
        }
      }
    }



    public override int PaletteOffset 
    {
      get
      {
        return base.PaletteOffset;
      }
      set
      {
        base.PaletteOffset = value;

        comboPaletteOffset.SelectedIndex = PaletteOffset / 16;
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
        if ( value == ColorType.BACKGROUND )
        {
          radioBackground.Checked = true;
          _CurrentColorType = value;
          return;
        }

        radioCharColor.Checked = true;
        _CurrentColorType = ColorType.CUSTOM_COLOR;
      }
    }



    public override int ActivePalette
    {
      get
      {
        return Colors.ActivePalette;
      }
      set
      {
        Colors.ActivePalette = value;
        comboActivePalette.SelectedIndex = Colors.ActivePalette;
      }
    }
    
    
    
    public ColorSettingsX16Sprites16() :
      base( null, null, 0 )
    { 
    }



    public ColorSettingsX16Sprites16( StudioCore Core, ColorSettings Colors, byte CustomColor ) :
      base( Core, Colors, CustomColor )
    {
      InitializeComponent();

      // we only use 16 colors of the full palette! TODO - offset!
      for ( int i = 0; i < 16; ++i )
      {
        comboCharColor.Items.Add( i.ToString( "d2" ) );
        comboBackground.Items.Add( i.ToString( "d2" ) );
        comboPaletteOffset.Items.Add( ( i * 16 ).ToString() );
      }
      comboBackground.SelectedIndex = Colors.BackgroundColor;
      comboCharColor.SelectedIndex = CustomColor;
      comboPaletteOffset.SelectedIndex = 0;

      radioCharColor.Checked = true;

      foreach ( var pal in Colors.Palettes )
      {
        comboActivePalette.Items.Add( pal.Name );
      }
      comboActivePalette.SelectedIndex = Colors.ActivePalette;
    }



    private void comboColor_DrawItem( object sender, DrawItemEventArgs e )
    {
      ComboBox combo = (ComboBox)sender;

      Core?.Theming.DrawSingleColorComboBox( combo, e, Colors.Palette );
    }



    private void comboCharColor_DrawItem( object sender, DrawItemEventArgs e )
    {
      ComboBox combo = (ComboBox)sender;

      Core?.Theming.DrawSingleColorComboBox( combo, e, Colors.Palette, comboPaletteOffset.SelectedIndex * 16 );
    }



    private void comboBackground_SelectedIndexChanged( object sender, EventArgs e )
    {
      Colors.BackgroundColor = comboBackground.SelectedIndex;
      radioBackground.Checked = true;
      RaiseColorsModifiedEvent( ColorType.BACKGROUND );
    }



    private void comboCharColor_SelectedIndexChanged( object sender, EventArgs e )
    {
      _SelectedCustomColor = (byte)comboCharColor.SelectedIndex;
      radioCharColor.Checked = true;
      RaiseCustomColorSelectedEvent();
    }



    private void btnEditPalette_Click( object sender, EventArgs e )
    {
      var dlgPalette = new DlgPaletteEditor( Core, Colors );
      if ( dlgPalette.ShowDialog() == DialogResult.OK )
      {
        Colors.Palettes = dlgPalette.Colors.Palettes;
        if ( Colors.ActivePalette >= Colors.Palettes.Count )
        {
          Colors.ActivePalette = Colors.Palettes.Count - 1;
        }

        RaisePaletteModifiedEvent( dlgPalette.PaletteMapping );

        comboActivePalette.BeginUpdate();
        comboActivePalette.Items.Clear();
        foreach ( var pal in Colors.Palettes )
        {
          comboActivePalette.Items.Add( pal.Name );
        }
        comboActivePalette.SelectedIndex = Colors.ActivePalette;
        comboActivePalette.EndUpdate();

        comboCharColor.Invalidate();
      }
    }



    private void radioBackground_CheckedChanged( object sender, EventArgs e )
    {
      _CurrentColorType = ColorType.BACKGROUND;
      RaiseColorSelectedEvent();
    }



    private void radioCharColor_CheckedChanged( object sender, EventArgs e )
    {
      _CurrentColorType = ColorType.CUSTOM_COLOR;
      RaiseColorSelectedEvent();
    }



    public override void ColorChanged( ColorType Color, int Value )
    {
      switch ( Color )
      {
        case ColorType.BACKGROUND:
          comboBackground.SelectedIndex = Value;
          break;
        case ColorType.CUSTOM_COLOR:
          comboCharColor.SelectedIndex = Value;
          break;
      }
    }



    private void comboActivePalette_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( comboActivePalette.SelectedIndex != Colors.ActivePalette )
      {
        Colors.ActivePalette = comboActivePalette.SelectedIndex;
        RaisePaletteSelectedEvent();
      }
    }



    public override void PalettesChanged()
    {
      if ( comboActivePalette.Items.Count != Colors.Palettes.Count )
      {
        comboActivePalette.BeginUpdate();
        comboActivePalette.Items.Clear();
        foreach ( var pal in Colors.Palettes )
        {
          comboActivePalette.Items.Add( pal.Name );
        }
        comboActivePalette.SelectedIndex = 0;
        comboActivePalette.EndUpdate();
      }
    }



    private void comboPaletteOffset_SelectedIndexChanged( object sender, EventArgs e )
    {
      if ( PaletteOffset != comboPaletteOffset.SelectedIndex * 16 )
      {
        PaletteOffset = comboPaletteOffset.SelectedIndex * 16;
        Colors.PaletteOffset = comboPaletteOffset.SelectedIndex * 16;
        comboCharColor.Invalidate();
        RaisePaletteSelectedEvent();
      }
    }



  }
}
