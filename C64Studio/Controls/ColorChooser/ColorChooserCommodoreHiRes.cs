﻿using RetroDevStudio;
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

namespace RetroDevStudio.Controls
{
  public partial class ColorChooserCommodoreHiRes : ColorChooserBase
  {
    public ColorChooserCommodoreHiRes() :
      base( null, null )
    { 
    }



    public ColorChooserCommodoreHiRes( StudioCore Core, ColorSettings Colors ) :
      base( Core, Colors )
    {
      InitializeComponent();

      for ( int i = 0; i < Colors.Palette.NumColors; ++i )
      {
        comboBackground.Items.Add( i.ToString( "d2" ) );
      }
      comboBackground.SelectedIndex = Colors.BackgroundColor;
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



    public override void ColorChanged( ColorType Color, int Value )
    {
      switch ( Color )
      {
        case ColorType.BACKGROUND:
          comboBackground.SelectedIndex = Value;
          break;
      }
    }



  }
}
