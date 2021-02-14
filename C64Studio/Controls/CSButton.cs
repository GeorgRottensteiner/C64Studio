using GR.Image;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace C64Studio.Controls
{
  public class CSButton : Button
  {
    public new Image Image 
    {
      get
      {
        return base.Image;
      }
      set
      {
        base.Image = value.GetImageStretchedDPI();
      }
    }
  }



}
