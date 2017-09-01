using System;
using System.Collections.Generic;
using System.Text;

namespace Tiny64Cmd
{
  class Breakpoint
  {
    public int       Address = -1;
    public bool      OnWrite = true;
    public bool      OnRead = true;
    public bool      OnExecute = true;
  }
}
