using RetroDevStudio.Dialogs;
using RetroDevStudio.Types;
using System;
using System.Windows.Forms;



namespace RetroDevStudio.Controls
{
  public partial class ColorSettingsX16Sprites256 : ColorSettingsBase
  {
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
    
    
    
    public ColorSettingsX16Sprites256() :
      base( null, null, 0 )
    { 
    }



    public ColorSettingsX16Sprites256( StudioCore Core, ColorSettings Colors, byte CustomColor ) :
      base( Core, Colors, CustomColor )
    {
      InitializeComponent();

      _AvailableColors.Add( ColorType.BACKGROUND );
      _AvailableColors.Add( ColorType.CUSTOM_COLOR );

      for ( int i = 0; i < Colors.Palette.NumColors; ++i )
      {
        comboBackground.Items.Add( i.ToString( "d2" ) );
      }
      comboBackground.SelectedIndex = Colors.BackgroundColor % comboBackground.Items.Count;

      for ( int i = 0; i < 16; ++i )
      {
        comboPaletteOffset.Items.Add( ( i * 16 ).ToString() );
      }
      comboPaletteOffset.SelectedIndex = 0;

      foreach ( var pal in Colors.Palettes )
      {
        comboActivePalette.Items.Add( pal.Name );
      }
      comboActivePalette.SelectedIndex = Colors.ActivePalette % comboActivePalette.Items.Count;
    }



    private void comboColor_DrawItem( object sender, DrawItemEventArgs e )
    {
      ComboBox combo = (ComboBox)sender;

      Core?.Theming.DrawSingleColorComboBox( combo, e, Colors.Palette );
    }



    private void comboCharColor_DrawItem( object sender, DrawItemEventArgs e )
    {
      ComboBox combo = (ComboBox)sender;

      if ( e.Index < 16 )
      {
        Core?.Theming.DrawSingleColorComboBox( combo, e, Colors.Palette, comboPaletteOffset.SelectedIndex * 16 );
      }
      else
      {
        Core?.Theming.DrawSingleColorComboBox( combo, e, Colors.Palette );
      }
    }



    private void comboBackground_SelectedIndexChanged( object sender, EventArgs e )
    {
      Colors.BackgroundColor = comboBackground.SelectedIndex;
      RaiseColorsModifiedEvent( ColorType.BACKGROUND );
    }



    private void btnEditPalette_Click( DecentForms.ControlBase Sender )
    {
      var dlgPalette = new DlgPaletteEditor( Core, Colors );
      if ( dlgPalette.ShowDialog() == DialogResult.OK )
      {
        Colors.Palettes = dlgPalette.Colors.Palettes;
        RaisePaletteModifiedEvent( dlgPalette.PaletteMapping );

        comboActivePalette.BeginUpdate();
        comboActivePalette.Items.Clear();
        foreach ( var pal in Colors.Palettes )
        {
          comboActivePalette.Items.Add( pal.Name );
        }
        comboActivePalette.SelectedIndex = Colors.ActivePalette;
        comboActivePalette.EndUpdate();
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
        RaisePaletteSelectedEvent();
      }
    }



  }
}
