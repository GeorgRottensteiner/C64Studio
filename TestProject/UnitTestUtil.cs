using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RetroDevStudio;
using Tiny64;

namespace TestProject
{
  [TestClass]
  public class UnitTestUtil
  {
    [DataRow( "010203", 10, 10, 0, 80, true, -1, "10 DATA 01,02,03" )]
    [DataRow( "010203", 10, 10, 0, 80, true, 4, "10 DATA 0001,0002,0003" )]
    [DataRow( "010203", 10, 10, 10, 80, true, 4, "10 DATA 0001,0002,0003" )]
    [DataRow( "010203", 10, 10, 0, 2, true, 3, "10 DATA 001,002\r\n20 DATA 003" )]
    [DataRow( "010203", 10, 10, 3, 0, true, -1, "10 DATA 01,02,03" )]
    [TestMethod]
    public void TestToBASICHexData( string hexData, int startLine, int lineStep, int wrapChars, int wrapBytes, bool insertSpaces, int padCount, string expectedContent )
    {
      var result = Util.ToBASICHexData( new GR.Memory.ByteBuffer( hexData ),
                                        startLine, lineStep,
                                        wrapBytes,
                                        wrapChars,
                                        insertSpaces,
                                        padCount ).Trim();

      Assert.AreEqual( expectedContent, result );
    }

  }
}
