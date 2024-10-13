using RetroDevStudio;
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
using GR.Forms;
using GR.Image;



namespace RetroDevStudio.Controls
{
  public partial class ColorChooserX16 : ColorChooserBase
  {
    private bool                        m_ColorChooserPopupActive = false;



    public ColorChooserX16() :
      base( null, null, 0, 1 )
    { 
    }



    public ColorChooserX16( StudioCore Core, CharsetProject Charset, ushort CurrentChar, byte CustomColor ) :
      base( Core, Charset, CurrentChar, CustomColor )
    {
      _Charset = Charset;

      InitializeComponent();

      panelCharColors.DisplayPage.Create( 128, 8, GR.Drawing.PixelFormat.Format32bppRgb );
    }



    public override void Redraw()
    {
      if ( _Charset == null )
      {
        return;
      }

      Displayer.CharacterDisplayer.DisplayChar( _Charset, SelectedChar, panelCharColors.DisplayPage, 0, 0, SelectedColor );
                                                 
      // "click for more"
      Displayer.CharacterDisplayer.DisplayChar( _Charset, 32, panelCharColors.DisplayPage, 8, 0, 1 );
      Displayer.CharacterDisplayer.DisplayChar( _Charset, 3, panelCharColors.DisplayPage, 16, 0, 1 );
      Displayer.CharacterDisplayer.DisplayChar( _Charset, 12, panelCharColors.DisplayPage, 24, 0, 1 );
      Displayer.CharacterDisplayer.DisplayChar( _Charset, 9, panelCharColors.DisplayPage, 32, 0, 1 );
      Displayer.CharacterDisplayer.DisplayChar( _Charset, 3, panelCharColors.DisplayPage, 40, 0, 1 );
      Displayer.CharacterDisplayer.DisplayChar( _Charset, 11, panelCharColors.DisplayPage, 48, 0, 1 );
      Displayer.CharacterDisplayer.DisplayChar( _Charset, 32, panelCharColors.DisplayPage, 56, 0, 1 );
      Displayer.CharacterDisplayer.DisplayChar( _Charset, 6, panelCharColors.DisplayPage, 64, 0, 1 );
      Displayer.CharacterDisplayer.DisplayChar( _Charset, 15, panelCharColors.DisplayPage, 72, 0, 1 );
      Displayer.CharacterDisplayer.DisplayChar( _Charset, 18, panelCharColors.DisplayPage, 80, 0, 1 );
      Displayer.CharacterDisplayer.DisplayChar( _Charset, 32, panelCharColors.DisplayPage, 88, 0, 1 );
      Displayer.CharacterDisplayer.DisplayChar( _Charset, 13, panelCharColors.DisplayPage, 96, 0, 1 );
      Displayer.CharacterDisplayer.DisplayChar( _Charset, 15, panelCharColors.DisplayPage, 104, 0, 1 );
      Displayer.CharacterDisplayer.DisplayChar( _Charset, 18, panelCharColors.DisplayPage, 112, 0, 1 );
      Displayer.CharacterDisplayer.DisplayChar( _Charset, 5, panelCharColors.DisplayPage, 120, 0, 1 );

      panelCharColors.Invalidate();
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
      ||   ( m_ColorChooserPopupActive )
      ||   ( X >= panelCharColors.ClientSize.Width ) )
      {
        return;
      }

      if ( ( Buttons == MouseButtons.Left )
      &&   ( panelCharColors.ClientRectangle.Contains( X, Y ) ) )
      {
        var popupControl = new FastPictureBox();
        popupControl.DisplayPage = new FastImage( 128, 128 );
        popupControl.Size = new Size( 256, 256 );

        var popup = new SingleActionPopupControl( popupControl );
        var screenPos = panelCharColors.Parent.PointToScreen( panelCharColors.Location );
        popup.Location = new Point( screenPos.X, screenPos.Y - popup.Height + panelCharColors.Height );
        popup.ClientSize = new Size( 256, 256 );

        // build all variations
        for ( byte i = 0; i < 16; ++i )
        {
          for ( byte j = 0; j < 16; ++j )
          {
            Displayer.CharacterDisplayer.DisplayChar( _Charset, SelectedChar, popupControl.DisplayPage,
              i * 8, j * 8, j * 16 + i );
          }
        }

        popup.Clicked += m_ColorChoserPopup_Clicked;
        popup.HandleDestroyed += Popup_HandleDestroyed;
        popup.Show();
        popup.Focus();
        m_ColorChooserPopupActive = true;
        return;
      }
      if ( ( Buttons & MouseButtons.Left ) == MouseButtons.Left )
      {
        int colorIndex = (int)( ( 16 * X ) / panelCharColors.ClientSize.Width );
        SelectedColor = (byte)colorIndex;
        Redraw();
        RaiseColorSelectedEvent();
      }
    }



    private void Popup_HandleDestroyed( object sender, EventArgs e )
    {
      m_ColorChooserPopupActive = false;
    }



    private void m_ColorChoserPopup_Clicked( int X, int Y )
    {
      int colorIndex = ( X / 16 ) + ( Y / 16 ) * 16;
      SelectedColor = (byte)colorIndex;
      RaiseColorSelectedEvent();
      Redraw();
    }



  }
}
