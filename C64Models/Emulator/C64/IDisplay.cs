using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif


namespace Tiny64
{
	public interface IDisplay
	{
    unsafe void SetPixel( int X, int Y, byte Color );

    unsafe void Flush();
  }

}
