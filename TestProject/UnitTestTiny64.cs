using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tiny64;

namespace TestProject
{
  [TestClass]
  public class UnitTestTiny64
  {
    [TestMethod]
    public void TestStartup()
    {
      Tiny64.Machine    machine = new Tiny64.Machine();

      machine.Display = new Display( null );
      machine.RunCycle();
    }

  }
}
