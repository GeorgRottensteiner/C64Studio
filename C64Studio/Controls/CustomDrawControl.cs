using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace C64Studio.Controls
{
  public class CustomDrawControl : System.Windows.Forms.PictureBox
  {
    public CustomDrawControl()
    {
      this.SetStyle( ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true );
    }
  }



  public class CustomDrawControlContext
  {
    public System.Drawing.Graphics  Graphics;
    public System.Drawing.Rectangle Bounds;

    public CustomDrawControlContext( System.Drawing.Graphics Graphics, int Width, int Height )
    {
      this.Graphics = Graphics;
      Bounds = new System.Drawing.Rectangle( 0, 0, Width, Height );
    }
  };
}
