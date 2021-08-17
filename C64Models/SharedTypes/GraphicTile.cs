using GR.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace C64Models.SharedTypes
{
  class GraphicTile
  {
    public int            TransparentColorIndex = -1;
    public int            Width = 8;
    public int            Height = 8;

    public int            PaletteIndex = -1;
    public int            CustomColor = -1;

    public ByteBuffer     Data = new ByteBuffer();
  }
}
