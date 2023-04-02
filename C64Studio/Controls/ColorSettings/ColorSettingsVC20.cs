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
  public partial class ColorSettingsVC20 : ColorSettingsBase
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
        switch ( value )
        {
          case ColorType.BACKGROUND:
            radioBackground.Checked = true;
            break;
          case ColorType.MULTICOLOR_1:
            radioMultiColor1.Checked = true;
            break;
          case ColorType.MULTICOLOR_2:
            radioCharColor.Checked = true;
            break;
          case ColorType.CUSTOM_COLOR:
            radioMulticolor2.Checked = true;
            break;
          default:
            return;
        }
        _CurrentColorType = value;
      }
    }



    public ColorSettingsVC20() :
      base( null, null, 0 )
    { 
    }



    public ColorSettingsVC20( StudioCore Core, ColorSettings Colors, byte CustomColor ) :
      base( Core, Colors, CustomColor )
    {
      InitializeComponent();

      for ( int i = 0; i < Colors.Palette.NumColors; ++i )
      {
        comboCharColor.Items.Add( i.ToString( "d2" ) );
        if ( i < 8 )
        {
          comboBackground.Items.Add( i.ToString( "d2" ) );
        }
        comboMulticolor1.Items.Add( i.ToString( "d2" ) );
        comboMulticolor2.Items.Add( i.ToString( "d2" ) );
      }
      comboBackground.SelectedIndex = Colors.BackgroundColor % 8;
      comboMulticolor1.SelectedIndex = Colors.MultiColor1;
      comboMulticolor2.SelectedIndex = Colors.MultiColor2;
      comboCharColor.SelectedIndex = CustomColor;

      radioCharColor.Checked = true;
      _CurrentColorType = ColorType.MULTICOLOR_2;
    }



    private void comboColor_DrawItem( object sender, DrawItemEventArgs e )
    {
      ComboBox combo = (ComboBox)sender;

      Core?.Theming.DrawSingleColorComboBox( combo, e, Colors.Palette );
    }



    private void comboCharColor_DrawItem( object sender, DrawItemEventArgs e )
    {
      ComboBox combo = (ComboBox)sender;

      Core?.Theming.DrawMultiColorComboBox( combo, e, Colors.Palette );
    }



    private void comboBackground_SelectedIndexChanged( object sender, EventArgs e )
    {
      Colors.BackgroundColor = comboBackground.SelectedIndex;
      radioBackground.Checked = true;
      RaiseColorsModifiedEvent( ColorType.BACKGROUND );
    }



    private void comboMulticolor1_SelectedIndexChanged( object sender, EventArgs e )
    {
      Colors.MultiColor1 = comboMulticolor1.SelectedIndex;
      radioMultiColor1.Checked = true;
      RaiseColorsModifiedEvent( ColorType.MULTICOLOR_1 );
    }



    private void comboMulticolor2_SelectedIndexChanged( object sender, EventArgs e )
    {
      Colors.MultiColor2 = comboMulticolor2.SelectedIndex;
      radioMulticolor2.Checked = true;
      RaiseColorsModifiedEvent( ColorType.MULTICOLOR_2 );
    }



    private void comboCharColor_SelectedIndexChanged( object sender, EventArgs e )
    {
      CustomColor = (byte)comboCharColor.SelectedIndex;
      radioCharColor.Checked = true;
      RaiseColorsModifiedEvent( ColorType.CUSTOM_COLOR );
    }



    private void btnExchangeColors_Click( object sender, EventArgs e )
    {
      contextMenuExchangeColors.Show( btnExchangeColors, new Point( 0, btnExchangeColors.Height ) );
    }



    private void radioBackground_CheckedChanged( object sender, EventArgs e )
    {
      _CurrentColorType = ColorType.BACKGROUND;
      RaiseColorSelectedEvent();
    }



    private void radioMultiColor1_CheckedChanged( object sender, EventArgs e )
    {
      _CurrentColorType = ColorType.MULTICOLOR_1;
      RaiseColorSelectedEvent();
    }



    private void radioMulticolor2_CheckedChanged( object sender, EventArgs e )
    {
      _CurrentColorType = ColorType.CUSTOM_COLOR;
      RaiseColorSelectedEvent();
    }



    private void radioCharColor_CheckedChanged( object sender, EventArgs e )
    {
      _CurrentColorType = ColorType.MULTICOLOR_2;
      RaiseColorSelectedEvent();
    }



    public override void ColorChanged( ColorType Color, int Value )
    {
      switch ( Color )
      {
        case ColorType.BACKGROUND:
          comboBackground.SelectedIndex = Value % comboBackground.Items.Count;
          break;
        case ColorType.MULTICOLOR_1:
          comboMulticolor1.SelectedIndex = Value % comboMulticolor1.Items.Count;
          break;
        case ColorType.MULTICOLOR_2:
          comboMulticolor2.SelectedIndex = Value % comboMulticolor2.Items.Count;
          break;
        case ColorType.CUSTOM_COLOR:
          comboCharColor.SelectedIndex = Value % comboCharColor.Items.Count;
          break;
      }
    }



    private void exchangeMultiColor1WithMultiColor2ToolStripMenuItem_Click( object sender, EventArgs e )
    {
      RaiseColorsExchangedEvent( ColorType.MULTICOLOR_1, ColorType.CUSTOM_COLOR );
    }



    private void exchangeMultiColor1WithBGColorToolStripMenuItem_Click( object sender, EventArgs e )
    {
      RaiseColorsExchangedEvent( ColorType.MULTICOLOR_1, ColorType.BACKGROUND );
    }



    private void exchangeMultiColor2WithBGColorToolStripMenuItem_Click( object sender, EventArgs e )
    {
      RaiseColorsExchangedEvent( ColorType.CUSTOM_COLOR, ColorType.BACKGROUND );
    }



  }
}
