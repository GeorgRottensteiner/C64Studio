using RetroDevStudio.Dialogs;
using RetroDevStudio.Types;
using System;
using System.Windows.Forms;



namespace RetroDevStudio.Controls
{
  public partial class ColorSettingsMega6516Colors : ColorSettingsBase
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
          comboCharColor.SelectedIndex = value;
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
        if ( _CurrentColorType == ColorType.CUSTOM_COLOR )
        {
          radioCharColor.Checked = true;
        }
        else
        {
          radioBackground.Checked = true;
        }
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
    
    
    
    public ColorSettingsMega6516Colors() :
      base( null, null, 0 )
    { 
    }



    public ColorSettingsMega6516Colors( StudioCore Core, ColorSettings Colors, byte CustomColor ) :
      base( Core, Colors, CustomColor )
    {
      InitializeComponent();

      for ( int i = 0; i < Colors.Palette.NumColors; ++i )
      {
        if ( i < 16 )
        {
          comboCharColor.Items.Add( i.ToString( "d2" ) );
        }
        comboBackground.Items.Add( i.ToString( "d2" ) );
      }
      comboBackground.SelectedIndex = Colors.BackgroundColor;
      comboCharColor.SelectedIndex = CustomColor;

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
      Colors.BackgroundColor = comboBackground.SelectedIndex;
      radioBackground.Checked = true;
      RaiseColorsModifiedEvent( ColorType.BACKGROUND );
    }



    private void comboCharColor_SelectedIndexChanged( object sender, EventArgs e )
    {
      SelectedCustomColor = (byte)comboCharColor.SelectedIndex;
      radioCharColor.Checked = true;
      RaiseCustomColorSelectedEvent();
    }



    private void btnEditPalette_Click( DecentForms.ControlBase Sender )
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


  }
}
