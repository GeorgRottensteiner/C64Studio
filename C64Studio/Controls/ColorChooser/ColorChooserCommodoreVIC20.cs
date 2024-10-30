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
  public partial class ColorChooserCommodoreVIC20 : ColorChooserBase
  {
    public ColorChooserCommodoreVIC20() :
      base( null, null )
    { 
    }



    public ColorChooserCommodoreVIC20( StudioCore Core, ColorSettings Colors ) :
      base( Core, Colors )
    {
      InitializeComponent();

      for ( int i = 0; i < Colors.Palette.NumColors; ++i )
      {
        comboBackground.Items.Add( i.ToString( "d2" ) );
        comboBorderColor.Items.Add( i.ToString( "d2" ) );
        comboAuxColor.Items.Add( i.ToString( "d2" ) );
      }
      comboBackground.SelectedIndex = Colors.BackgroundColor;
      comboBorderColor.SelectedIndex = Colors.MultiColor1;
      comboAuxColor.SelectedIndex = Colors.MultiColor2;
    }



    private void comboColor_DrawItem( object sender, DrawItemEventArgs e )
    {
      ComboBox combo = (ComboBox)sender;

      Core?.Theming.DrawSingleColorComboBox( combo, e, _Colors.Palette );
    }



    private void comboBackground_SelectedIndexChanged( object sender, EventArgs e )
    {
      _Colors.BackgroundColor = comboBackground.SelectedIndex;
      RaiseColorsModifiedEvent( ColorType.BACKGROUND );
    }



    private void comboMulticolor1_SelectedIndexChanged( object sender, EventArgs e )
    {
      _Colors.MultiColor1 = comboBorderColor.SelectedIndex;
      RaiseColorsModifiedEvent( ColorType.MULTICOLOR_1 );
    }



    private void comboMulticolor2_SelectedIndexChanged( object sender, EventArgs e )
    {
      _Colors.MultiColor2 = comboAuxColor.SelectedIndex;
      RaiseColorsModifiedEvent( ColorType.MULTICOLOR_2 );
    }



    public override void ColorChanged( ColorType Color, int Value )
    {
      switch ( Color )
      {
        case ColorType.BACKGROUND:
          comboBackground.SelectedIndex = Value;
          break;
        case ColorType.MULTICOLOR_1:
          comboBorderColor.SelectedIndex = Value;
          break;
        case ColorType.MULTICOLOR_2:
          comboAuxColor.SelectedIndex = Value;
          break;
      }
    }



  }
}
