using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
  [TestClass]
  public class UnitTestBASICParser
  {
    private C64Studio.Parser.BasicFileParser CreateParser()
    {
      return new C64Studio.Parser.BasicFileParser( new C64Studio.Parser.BasicFileParser.ParserSettings() );
    }



    private GR.Memory.ByteBuffer TestCompile( string Source )
    {
      var parser = CreateParser();

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

      var parser = CreateParser();

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;
      Assert.IsTrue( parser.Parse( source, null, config ) );

      string result = parser.Renumber( 10, 3 );

      Assert.AreEqual( "10IFA=1THEN10", result );
    }



    [TestMethod]
    public void TestRenumberWithSpacesNoStripSpaces()
    {
      string      source = @"20 ifa=1then 20";

      var parser = CreateParser();
      parser.Settings.StripSpaces = false;

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;
      Assert.IsTrue( parser.Parse( source, null, config ) );

      string result = parser.Renumber( 10, 3 );

      Assert.AreEqual( "10IFA=1THEN 10", result );
    }



    [TestMethod]
    public void TestRenumberOverLines()
    {
      string      source = @"10 goto 300
                          300 goto 10";

      var parser = CreateParser();

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

      var parser = CreateParser();

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

      var parser = CreateParser();

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



    [TestMethod]
    public void TestEncodeToLabels()
    {
      string      source = @"10 print ""Hallo""
                          20 goto 10";

      var parser = CreateParser();

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config ) );

      string  encoded = parser.EncodeToLabels();
      Assert.AreEqual( @"
LABEL10
PRINT""HALLO""
GOTOLABEL10
", encoded );
    }



    [TestMethod]
    public void TestEncodeToLabelsNoStripSpaces()
    {
      string      source = @"10 print ""Hallo""
                          20 goto 10";

      var parser = CreateParser();

      parser.Settings.StripSpaces = false;

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config ) );

      string  encoded = parser.EncodeToLabels();
      Assert.AreEqual( @"
LABEL10
PRINT ""HALLO""
GOTO LABEL10
", encoded );
    }

  }
}
