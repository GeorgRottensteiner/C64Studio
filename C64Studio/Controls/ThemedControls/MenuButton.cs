using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DecentForms
{
  public class MenuButton : Button
  {
    [DefaultValue( null )]
    public ContextMenuStrip Menu
    {
      get;
      set;
    }



    [DefaultValue( true )]
    public bool ShowDropDownArrow { get; set; }

    [DefaultValue( false )]
    public bool ShowSplitBar { get; set; }

    [DefaultValue( false )]
    public bool Checked { get; set; }



    public event EventHandler CheckedChanged;



    protected override void OnMouseDown( MouseEventArgs mevent )
    {
      base.OnMouseDown( mevent );

      bool    hitMenu = false;
      int     lineX = ClientRectangle.Width - 18;
      int     menuShowX = 0;

      if ( ShowSplitBar )
      {
        if ( mevent.X >= lineX )
        {
          hitMenu   = true;
          menuShowX = lineX;
        }
      }
      else
      {
        hitMenu = true;
      }



      if ( ( Menu != null )
      &&   ( hitMenu )
      &&   ( mevent.Button == MouseButtons.Left ) )
      {
        System.Drawing.Point    ptMenu = new Point( menuShowX, Height );
        
        if ( ptMenu.Y + Menu.Height >= Screen.FromControl( this ).WorkingArea.Height )
        {
          ptMenu = PointToScreen( new Point( menuShowX, -Menu.Height ) );
        }
        Menu.Show( this, ptMenu );
      }
      else
      {
        if ( CheckedChanged != null )
        {
          CheckedChanged( this );
        }
        OnClick( new EventArgs() );
      }
    }



    protected override void OnPaint( PaintEventArgs pevent )
    {
      base.OnPaint( pevent );

      int arrowX = ClientRectangle.Width - 14;
      int arrowY = ClientRectangle.Height / 2 - 1;

      if ( ShowDropDownArrow )
      {
        Brush brush = Enabled ? SystemBrushes.ControlText : SystemBrushes.ButtonShadow;
        Point[] arrows = new Point[] { new Point( arrowX, arrowY ), new Point( arrowX + 7, arrowY ), new Point( arrowX + 3, arrowY + 4 ) };
        pevent.Graphics.FillPolygon( brush, arrows );
      }

      if ( ShowSplitBar )
      {
        // Draw a dashed separator on the left of the arrow
        int lineX = ClientRectangle.Width - 18;
        int lineYFrom = 4;
        int lineYTo = ClientRectangle.Height - 5;
        using ( var separatorPen = new Pen( Brushes.DarkGray ) )
        {
          pevent.Graphics.DrawLine( separatorPen, lineX, lineYFrom, lineX, lineYTo );
        }
      }
    }



  }
}
