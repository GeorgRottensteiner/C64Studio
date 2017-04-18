using System;
using System.Collections.Generic;
using System.Text;

namespace MediaManager
{
  class Program
  {
    static int Main( string[] args )
    {
      var manager = new Manager();

      return manager.Handle( args );
    }
  }
}
