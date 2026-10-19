using RetroDevStudio.Dialogs;
using RetroDevStudio.Types;
using System;
using System.Windows.Forms;



namespace RetroDevStudio.Controls
{
  public partial class ColorSettingsMega65 : ColorSettingsBase
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
    
    
    
    public ColorSettingsMega65() :
      base( null, null, 0 )
    { 
    }



    public ColorSettingsMega65( StudioCore Core, ColorSettings Colors, byte CustomColor ) :
      base( Core, Colors, CustomColor )
    {
      InitializeComponent();

      _AvailableColors.Add( ColorType.CUSTOM_COLOR );

      for ( int i = 0; i < Colors.Palette.NumColors; ++i )
      {
        comboBackground.Items.Add( i.ToString( "d2" ) );
      }
      comboBackground.SelectedIndex = Colors.BackgroundColor % comboBackground.Items.Count;

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


  }
}
