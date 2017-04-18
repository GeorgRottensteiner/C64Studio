using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace C64Studio
{
  public class HexPanel : CustomAutoScrollPanel.ScrollablePanel
  {
    public HexPanel()
    {

      // Set the value of the double-buffering style bits to true.
      SetStyle( ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true );
      UpdateStyles();

    }

  }
}
