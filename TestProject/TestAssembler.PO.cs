using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
  public partial class TestAssembler
  {
    [TestMethod]
    public void TestAssemblyExtMathSin()
    {
      string      source = @"* = $1000

                        !byte math.sin(90)
                        !byte math.sin(180)";

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "00100100", assembly.ToString() );
    }



    [TestMethod]
    public void TestAssemblyExtMathCos()
    {
      string      source = @"* = $1000

                        !byte math.cos(90)
                        !byte math.cos(180)";

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "001000FF", assembly.ToString() );
    }



    [TestMethod]
    public void TestAssemblyExtMathTan()
    {
      string      source = @"* = $1000

                        !byte math.tan(20)
                        !byte math.tan(45)";

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "00100001", assembly.ToString() );
    }



    [TestMethod]
    public void TestAssemblyExtToDegreees()
    {
      string      source = @"* = $1000

                        !byte math.todegrees(3.1415926)";

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "0010B3", assembly.ToString() );
    }



    [TestMethod]
    public void TestAssemblyExtToRadian()
    {
      string      source = @"* = $1000

                        !byte math.toradians(180)";

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "001003", assembly.ToString() );
    }



    [TestMethod]
    public void TestAssemblyExtToFloor()
    {
      string      source = @"* = $1000

                        !byte math.floor( 3.1415926 )";

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "001003", assembly.ToString() );
    }



    [TestMethod]
    public void TestAssemblyExtToCeiling()
    {
      string      source = @"* = $1000

                        !byte math.ceiling( 3.1415926 )";

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "001004", assembly.ToString() );
    }



    [TestMethod]
    public void TestAssemblyExtMin()
    {
      string      source = @"* = $1000

                        !byte math.min( 3, 10 )
                        !byte math.min( 50, 10 )";

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "0010030A", assembly.ToString() );
    }



    [TestMethod]
    public void TestAssemblyExtMax()
    {
      string      source = @"* = $1000

                        !byte math.max( 3, 10 )
                        !byte math.max( 50, 10 )";

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "00100A32", assembly.ToString() );
    }
  }
}
