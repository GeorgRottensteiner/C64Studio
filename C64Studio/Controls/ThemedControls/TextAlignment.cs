using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace DecentForms
{
  [Flags]
  public enum TextAlignment
  {
    LEFT                = 0x00000000,
    TOP                 = 0x00000000,
    RIGHT               = 0x00000001,
    CENTERED_H          = 0x00000002,
    BOTTOM              = 0x00000004,
    CENTERED_V          = 0x00000008,

    CENTERED            = CENTERED_H | CENTERED_V
  }
}
