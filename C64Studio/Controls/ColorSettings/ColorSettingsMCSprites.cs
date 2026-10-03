using RetroDevStudio;
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
  public partial class ColorSettingsMCSprites : ColorSettingsBase
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
        if ( ( comboCustomColor != null )
        &&   ( value >= 0 )
        &&   ( value < comboCustomColor.Items.Count ) )
        {
          comboCustomColor.SelectedIndex = value;
        }
      }
    }



    public override bool MultiColorEnabled
    {
      get
      {
        return checkMulticolor.Checked;
      }
      set
      {
        checkMulticolor.Checked = value;
      }
    }
    
    
    
    public ColorSettingsMCSprites() :
      base( null, null, 0 )
    { 
    }



    public ColorSettingsMCSprites( StudioCore Core, ColorSettings Colors, byte CustomColor, bool MulticolorEnabled ) :
      base( Core, Colors, CustomColor )
    {
      InitializeComponent();

      _AvailableColors.Add( ColorType.BACKGROUND );
      _AvailableColors.Add( ColorType.CUSTOM_COLOR );

      for ( int i = 0; i < Colors.Palette.NumColors; ++i )
      {
        comboCustomColor.Items.Add( i.ToString( "d2" ) );
        comboBackground.Items.Add( i.ToString( "d2" ) );
        comboMulticolor1.Items.Add( i.ToString( "d2" ) );
        comboMulticolor2.Items.Add( i.ToString( "d2" ) );
      }
      comboBackground.SelectedIndex = Colors.BackgroundColor % comboBackground.Items.Count;
      comboMulticolor1.SelectedIndex = Colors.MultiColor1 % comboMulticolor1.Items.Count;
      comboMulticolor2.SelectedIndex = Colors.MultiColor2 % comboMulticolor2.Items.Count;
      comboCustomColor.SelectedIndex = CustomColor % comboCustomColor.Items.Count;

      checkMulticolor.Checked = MulticolorEnabled;
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



    private void comboMulticolor1_SelectedIndexChanged( object sender, EventArgs e )
    {
      Colors.MultiColor1 = comboMulticolor1.SelectedIndex;
      RaiseColorsModifiedEvent( ColorType.MULTICOLOR_1 );
    }



    private void comboMulticolor2_SelectedIndexChanged( object sender, EventArgs e )
    {
      Colors.MultiColor2 = comboMulticolor2.SelectedIndex;
      RaiseColorsModifiedEvent( ColorType.MULTICOLOR_2 );
    }



    private void comboColor_SelectedIndexChanged( object sender, EventArgs e )
    {
      CustomColor = (byte)comboCustomColor.SelectedIndex;
      RaiseColorsModifiedEvent( ColorType.CUSTOM_COLOR );
    }



    private void btnExchangeColors_Click( DecentForms.ControlBase Sender )
    {
      contextMenuExchangeColors.Show( btnExchangeColors, new Point( 0, btnExchangeColors.Height ) );
    }



    public override void ColorChanged( ColorType Color, int Value )
    {
      switch ( Color )
      {
        case ColorType.BACKGROUND:
          comboBackground.SelectedIndex = Value;
          break;
        case ColorType.MULTICOLOR_1:
          comboMulticolor1.SelectedIndex = Value;
          break;
        case ColorType.MULTICOLOR_2:
          comboMulticolor2.SelectedIndex = Value;
          break;
        case ColorType.CUSTOM_COLOR:
          comboCustomColor.SelectedIndex = Value;
          break;
      }
    }



    private void exchangeMultiColor1WithMultiColor2ToolStripMenuItem_Click( object sender, EventArgs e )
    {
      RaiseColorsExchangedEvent( ColorType.MULTICOLOR_1, ColorType.MULTICOLOR_2 );
    }



    private void exchangeMultiColor1WithBGColorToolStripMenuItem_Click( object sender, EventArgs e )
    {
      RaiseColorsExchangedEvent( ColorType.MULTICOLOR_1, ColorType.BACKGROUND );
    }



    private void exchangeMultiColor2WithBGColorToolStripMenuItem_Click( object sender, EventArgs e )
    {
      RaiseColorsExchangedEvent( ColorType.MULTICOLOR_2, ColorType.BACKGROUND );
    }



    private void exchangeCharColorWithBGColorToolStripMenuItem_Click( object sender, EventArgs e )
    {
      RaiseColorsExchangedEvent( ColorType.CUSTOM_COLOR, ColorType.BACKGROUND );
    }



    private void exchangeCustomColorWithMultiColor1ToolStripMenuItem_Click( object sender, EventArgs e )
    {
      RaiseColorsExchangedEvent( ColorType.MULTICOLOR_1, ColorType.CUSTOM_COLOR );
    }



    private void exchangeCustomColorWithMultiColor2ToolStripMenuItem_Click( object sender, EventArgs e )
    {
      RaiseColorsExchangedEvent( ColorType.MULTICOLOR_2, ColorType.CUSTOM_COLOR );
    }



    private void checkMulticolor_CheckedChanged( object sender, EventArgs e )
    {
      labelMulticolor1.Enabled = checkMulticolor.Checked;
      labelMulticolor2.Enabled = checkMulticolor.Checked;
      comboMulticolor1.Enabled = checkMulticolor.Checked;
      comboMulticolor2.Enabled = checkMulticolor.Checked;
      if ( !checkMulticolor.Checked )
      {
        _AvailableColors.Remove( ColorType.MULTICOLOR_1 );
        _AvailableColors.Remove( ColorType.MULTICOLOR_2 );
      }
      else
      {
        _AvailableColors.Insert( 1, ColorType.MULTICOLOR_2 );
        _AvailableColors.Insert( 1, ColorType.MULTICOLOR_1 );
      }
      RaiseMulticolorFlagChanged();
    }



  }
}
