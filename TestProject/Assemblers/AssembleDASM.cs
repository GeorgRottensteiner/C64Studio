using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
  [TestClass]
  public class AssembleDASM
  {
    [TestMethod]
    public void TestDASMSyntax()
    {
      string      source = @"  ORG $2000
LABEL
                             dc.b  1,2,3,4,5,6,7,8";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.DASM );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.DASM;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( 1, parser.Warnings );
      Assert.AreEqual( RetroDevStudio.Parser.ParserBase.ParseMessage.LineType.WARNING, asmFileInfo.Messages.Values[0].Type );
      Assert.AreEqual( RetroDevStudio.Types.ErrorCode.W1000_UNUSED_LABEL, asmFileInfo.Messages.Values[0].Code );

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

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.DASM );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.DASM;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( 1, parser.Warnings );
      Assert.AreEqual( RetroDevStudio.Parser.ParserBase.ParseMessage.LineType.WARNING, asmFileInfo.Messages.Values[0].Type );
      Assert.AreEqual( RetroDevStudio.Types.ErrorCode.W1000_UNUSED_LABEL, asmFileInfo.Messages.Values[0].Code );

      Assert.AreEqual( "0020EE00D04C0020", assembly.Assembly.ToString() );
    }


  }
}
