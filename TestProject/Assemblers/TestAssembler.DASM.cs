using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
  public partial class TestAssembler
  {
    [TestMethod]
    public void TestDASMSyntax()
    {
      string      source = @"  ORG $2000
LABEL
                             dc.b  1,2,3,4,5,6,7,8";

      C64Studio.Parser.ASMFileParser      parser = new C64Studio.Parser.ASMFileParser();
      parser.SetAssemblerType( C64Studio.Types.AssemblerType.PDS );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.DASM;

      Assert.IsTrue( parser.Parse( source, null, config, null ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( 1, parser.Warnings );
      Assert.AreEqual( C64Studio.Parser.ParserBase.ParseMessage.LineType.WARNING, parser.Messages.Values[0].Type );
      Assert.AreEqual( C64Studio.Types.ErrorCode.W1000_UNUSED_LABEL, parser.Messages.Values[0].Code );

      Assert.AreEqual( "00200102030405060708", assembly.Assembly.ToString() );
    }



    [TestMethod]
    public void TestDASMLocalLabelScope()
    {
      string      source = "  ORG $2000\r\n"
                        +  "LABEL subroutine\r\n"
                        +  ".locallabel\r\n"
                        +  "  inc $d000\r\n"
                        +  "  jmp .locallabel";

      C64Studio.Parser.ASMFileParser      parser = new C64Studio.Parser.ASMFileParser();
      parser.SetAssemblerType( C64Studio.Types.AssemblerType.DASM );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.DASM;

      Assert.IsTrue( parser.Parse( source, null, config, null ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( 1, parser.Warnings );
      Assert.AreEqual( C64Studio.Parser.ParserBase.ParseMessage.LineType.WARNING, parser.Messages.Values[0].Type );
      Assert.AreEqual( C64Studio.Types.ErrorCode.W1000_UNUSED_LABEL, parser.Messages.Values[0].Code );

      Assert.AreEqual( "0020EE00D04C0020", assembly.Assembly.ToString() );
    }


  }
}
