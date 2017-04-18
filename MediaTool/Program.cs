using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTool
{
  class Program
  {
    static int Main( string[] args )
    {
      var tool = new Manager();

      return tool.Handle( args );
    }
  }
}
