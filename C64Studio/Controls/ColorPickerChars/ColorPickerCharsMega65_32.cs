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
  public partial class ColorPickerCharsMega65_32 : ColorPickerCharsBase
  {
    public ColorPickerCharsMega65_32() :
      base( null, null, 0, 1 )
    { 
    }



    public ColorPickerCharsMega65_32( StudioCore Core, CharsetProject Charset, ushort CurrentChar, byte CustomColor ) :
      base( Core, Charset, CurrentChar, CustomColor )
    {
      _Charset = Charset;

      InitializeComponent();

      panelCharColors.AutoResize = false;
      panelCharColors.DisplayPage.Create( 256, 8, GR.Drawing.PixelFormat.Format32bppRgb );
      panelCharColors.SetImageSize( 256, 8 );
    }



    public override void Redraw()
    {
      if ( _Charset == null )
      {
        return;
      }

      for ( byte i = 0; i < 32; ++i )
      {
        Displayer.CharacterDisplayer.DisplayChar( _Charset, SelectedChar, panelCharColors.DisplayPage, i * 8, 0, i );
      }
      panelCharColors.Invalidate();
    }



    private void panelCharColors_PostPaint( GR.Image.FastImage TargetBuffer )
    {
      int     x1 = SelectedColor * TargetBuffer.Width / 32;
      int     x2 = ( SelectedColor + 1 ) * TargetBuffer.Width / 32;

      if ( Core != null )
      {
        uint  selColor = Core.Settings.FGColor( ColorableElement.SELECTION_FRAME );

        TargetBuffer.Rectangle( x1, 0, x2 - x1, TargetBuffer.Height, selColor );
      }
    }



    private void panelCharColors_MouseDown( object sender, MouseEventArgs e )
    {
      HandleMouseOnColorChooser( e.X, e.Y, e.Button );
    }



    private void panelCharColors_MouseMove( object sender, MouseEventArgs e )
    {
      HandleMouseOnColorChooser( e.X, e.Y, e.Button );
    }



    private void HandleMouseOnColorChooser( int X, int Y, MouseButtons Buttons )
    {
      if ( ( X < 0 )
      ||   ( X >= panelCharColors.ClientSize.Width ) )
      {
        return;
      }

      if ( ( Buttons & MouseButtons.Left ) == MouseButtons.Left )
      {
        int colorIndex = (int)( ( 32 * X ) / panelCharColors.ClientSize.Width );
        SelectedColor = (byte)colorIndex;
        Redraw();
        RaiseColorSelectedEvent();
      }
    }



  }
}
