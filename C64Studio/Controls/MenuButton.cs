using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace C64Studio.Controls
{
  public class MenuButton : Button
  {
    [DefaultValue( null )]
    public ContextMenuStrip Menu
    {
      get;
      set;
    }



    protected override void OnMouseDown( MouseEventArgs mevent )
    {
      base.OnMouseDown( mevent );

      if ( ( Menu != null )
      &&   ( mevent.Button == MouseButtons.Left ) )
      {
        System.Drawing.Point    ptMenu = new Point( 0, Height );

        
        if ( ptMenu.Y + Menu.Height >= Screen.FromControl( this ).WorkingArea.Height )
        {
          ptMenu = PointToScreen( new Point( 0, -Menu.Height ) );
        }
        Menu.Show( this, ptMenu );
      }
    }



    protected override void OnPaint( PaintEventArgs pevent )
    {
      base.OnPaint( pevent );

      int arrowX = ClientRectangle.Width - 14;
      int arrowY = ClientRectangle.Height / 2 - 1;

      Brush brush = Enabled ? SystemBrushes.ControlText : SystemBrushes.ButtonShadow;
      Point[] arrows = new Point[] { new Point( arrowX, arrowY ), new Point( arrowX + 7, arrowY ), new Point( arrowX + 3, arrowY + 4 ) };
      pevent.Graphics.FillPolygon( brush, arrows );
    }
  }
}
