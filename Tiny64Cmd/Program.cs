using System;
using System.Collections.Generic;
using System.Text;



namespace Tiny64Cmd
{
  class Program
  {
    static void Main( string[] args )
    {
      var emu = new Emulator();

      emu.Run();
    }
  }
}
