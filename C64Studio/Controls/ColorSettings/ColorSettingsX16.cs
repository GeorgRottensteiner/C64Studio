using RetroDevStudio.Dialogs;
using RetroDevStudio.Types;
using System;
using System.Windows.Forms;



namespace RetroDevStudio.Controls
{
  public partial class ColorSettingsX16 : ColorSettingsBase
  {
    public override byte CustomColor
    {
      get
      {
        return _CustomColor;
      }
      set
      {
        _CustomColor = value;
        if ( comboCharColor != null )
        {
          comboCharColor.SelectedIndex = _CustomColor & 0x0f;
        }
        if ( comboBackground != null )
        {
          comboBackground.SelectedIndex = ( _CustomColor >> 4 ) & 0x0f;
        }
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
        _CurrentColorType = value;
        if ( _CurrentColorType == ColorType.BACKGROUND )
        {
          radioBackground.Checked = true;
        }
        else
        {
          radioCharColor.Checked = true;
        }
        comboCharColor.SelectedIndex = _CustomColor & 0x0f;
        comboBackground.SelectedIndex = ( _CustomColor >> 4 ) & 0x0f;
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
    
    
    
    public ColorSettingsX16() :
      base( null, null, 0 )
    { 
    }



    public ColorSettingsX16( StudioCore Core, ColorSettings Colors, byte CustomColor ) :
      base( Core, Colors, CustomColor )
    {
      InitializeComponent();

      _AvailableColors.Add( ColorType.BACKGROUND );
      _AvailableColors.Add( ColorType.CUSTOM_COLOR );

      // we only use the first 16 colors of the full palette!
      for ( int i = 0; i < 16; ++i )
      {
        comboCharColor.Items.Add( i.ToString( "d2" ) );
        comboBackground.Items.Add( i.ToString( "d2" ) );
      }
      comboBackground.SelectedIndex = Colors.BackgroundColor % 16;
      comboCharColor.SelectedIndex = CustomColor % 16;

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

      Core?.Theming.DrawSingleColorComboBox( combo, e, Colors.Palette );
    }



    private void comboBackground_SelectedIndexChanged( object sender, EventArgs e )
    {
      CustomColor = (byte)( ( comboBackground.SelectedIndex << 4 ) | comboCharColor.SelectedIndex );
      radioBackground.Checked = true;
      RaiseColorsModifiedEvent( ColorType.CUSTOM_COLOR );
    }



    private void comboCharColor_SelectedIndexChanged( object sender, EventArgs e )
    {
      CustomColor = (byte)( ( comboBackground.SelectedIndex << 4 ) | comboCharColor.SelectedIndex );
      radioCharColor.Checked = true;
      RaiseColorsModifiedEvent( ColorType.CUSTOM_COLOR );
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


  }
}
