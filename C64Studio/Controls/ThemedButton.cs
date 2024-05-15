using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace RetroDevStudio.Controls
{
  public class ThemedButton : Button
  {
    public Color DisabledTextColor { get; set; }



    public ThemedButton()
    {
      this.SetStyle( ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true );

      DisabledTextColor = SystemColors.GrayText;
    }



    protected override void OnPaint( PaintEventArgs e )
    {
      /*
      if ( Enabled )
      {
        base.OnPaint( e );
        return;
      }*/
      var colorText = ForeColor;
      var colorBG   = BackColor;

      if ( !Enabled )
      {
        colorText = DisabledTextColor;
      }

      using ( var sf = new StringFormat() )
      using ( var brushText = new SolidBrush( colorText ) )
      using ( var brushBG = new SolidBrush( BackColor ) )
      using ( var penBorder = new Pen( Color.Black ) )
      {
        e.Graphics.DrawRectangle( penBorder, ClientRectangle );

        ClientRectangle.Inflate( -1, -1 );
        e.Graphics.FillRectangle( brushBG, ClientRectangle );
        sf.Alignment = StringAlignment.Center;
        sf.LineAlignment = StringAlignment.Center;

        e.Graphics.DrawString( Text, Font, brushText, ClientRectangle, sf );
      }
    }
  }
}
