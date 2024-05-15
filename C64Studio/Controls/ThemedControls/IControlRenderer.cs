using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace DecentForms
{
  public interface IControlRenderer
  {
    void DrawRaisedRectangle( int X, int Y, int Width, int Height, uint BaseColor );
  }
}
