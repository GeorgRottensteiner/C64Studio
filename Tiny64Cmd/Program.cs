using System;
using System.Collections.Generic;
using System.Text;
using Tiny64;



namespace Tiny64Cmd
{
  class Program
  {
    [STAThreadAttribute]
    static int Main( string[] args )
    {
      var frameWork = new EmulatorFramework();

      return frameWork.Run();
    }
  }
}
