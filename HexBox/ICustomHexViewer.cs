using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Be.Windows.Forms
{
  public interface ICustomHexViewer
  {
    void PaintHexData( HexBox Box, Graphics graphics, long _startByte, long intern_endByte, Rectangle _recHex );
  }
}
