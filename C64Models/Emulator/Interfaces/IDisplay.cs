using System;
using System.Collections.Generic;
using System.Text;


namespace Tiny64
{
	public interface IDisplay
	{
    unsafe void SetPixel( int X, int Y, byte Color );

    unsafe void Flush();
  }

}
