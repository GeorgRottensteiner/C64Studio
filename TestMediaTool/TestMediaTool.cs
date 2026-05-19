using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace TestProject
{
  [TestClass]
  public class TestMediaTool
  {
    [TestMethod]
    //[DataRow( new string[] { "EXPORTGRAPHICSCREEN", "WIDTH=40", "HEIGHT=25" }, "testdata/MediaTool/ExportGraphicScreen/40x25.png", "testdata/MediaTool/ExportGraphicScreen/40x25.bin", true )]
    public void TestMediaToolExport( string[] args, string inputFile, string expectedOutputFileContentHex, bool expectedSuccess )
    {
      var tool = new MediaTool.Manager();

      int result = tool.Handle( args );

      Assert.AreEqual( expectedSuccess ? 0 : 1, result );
    }




  }
}
