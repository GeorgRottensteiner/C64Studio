using System;
using System.Collections.Generic;
using System.Text;

namespace C64Studio.Types
{
  public class Palette
  {
    public string                   Name = "";
    public System.Drawing.Color[]   Colors = new System.Drawing.Color[17]; 
    public System.Drawing.Brush[]   ColorBrushes = new System.Drawing.Brush[17];
    public uint[]                   ColorValues = new uint[17];
  }
}
