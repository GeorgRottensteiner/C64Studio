using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
  [TestClass]
  public class UnitTestBASICParser
  {
    private GR.Memory.ByteBuffer TestCompile( string Source )
    {
      C64Studio.Parser.BasicFileParser    parser = new C64Studio.Parser.BasicFileParser();

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( Source, null, config ) );
      Assert.IsTrue( parser.Assemble( config ) );

      return parser.AssembledOutput.Assembly;
    }



    [TestMethod]
    public void TestRenumberWithSpaces()
    {
      string      source = @"20 ifa=1then 20";

      C64Studio.Parser.BasicFileParser    parser = new C64Studio.Parser.BasicFileParser();

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;
      Assert.IsTrue( parser.Parse( source, null, config ) );

      string result = parser.Renumber( 10, 3 );

      Assert.AreEqual( "10IFA=1THEN10", result );
    }



    [TestMethod]
    public void TestRenumberOverLines()
    {
      string      source = @"10 goto 300
                          300 goto 10";

      C64Studio.Parser.BasicFileParser    parser = new C64Studio.Parser.BasicFileParser();

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;
      Assert.IsTrue( parser.Parse( source, null, config ) );

      string result = parser.Renumber( 10, 3 );

      Assert.AreEqual( @"10GOTO13
13GOTO10", result );
    }



    [TestMethod]
    public void TestRenumberOnGosub()
    {
      string      source = @"10 onxgosub100,400,700
                          100printa
                          400 printb
                          700 printc";

      C64Studio.Parser.BasicFileParser    parser = new C64Studio.Parser.BasicFileParser();

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;
      Assert.IsTrue( parser.Parse( source, null, config ) );

      string result = parser.Renumber( 10, 3 );

      Assert.AreEqual( @"10ONXGOSUB13,16,19
13PRINTA
16PRINTB
19PRINTC", result );
    }



    [TestMethod]
    public void TestRenumberWithStatementAfterGotoGosub()
    {
      string      source = @"10 onxgosub100,400,700:goto2000
                          100printa
                          400 printb
                          700 printc
                          2000 printd";

      C64Studio.Parser.BasicFileParser    parser = new C64Studio.Parser.BasicFileParser();

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;
      Assert.IsTrue( parser.Parse( source, null, config ) );

      string result = parser.Renumber( 10, 3 );

      Assert.AreEqual( @"10ONXGOSUB13,16,19:GOTO22
13PRINTA
16PRINTB
19PRINTC
22PRINTD", result );
    }

  }
}
