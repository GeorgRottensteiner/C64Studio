using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace C64Studio.Controls
{
  public class ThemedButton : Button
  {
    public Color DisabledTextColor { get; set; }



    public ThemedButton()
    {
      DisabledTextColor = SystemColors.GrayText;
    }



    protected override void OnPaint( PaintEventArgs e )
    {
      base.OnPaint( e );
      if ( Enabled )
      {
        return;
      }

      SolidBrush drawBrush = null;

      StringFormat sf = new StringFormat();

      sf.Alignment      = StringAlignment.Center;
      sf.LineAlignment  = StringAlignment.Center;

      drawBrush = new SolidBrush( DisabledTextColor );
      e.Graphics.DrawString( Text, Font, drawBrush, e.ClipRectangle, sf );

      drawBrush.Dispose();

      sf.Dispose();
    }
  }
}
