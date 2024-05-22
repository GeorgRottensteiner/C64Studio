using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
  [TestClass]
  public class AssembleTASM
  {
    private GR.Memory.ByteBuffer TestAssembleTASM( string Source )
    {
      return TestAssembleTASM( Source, out RetroDevStudio.Types.ASM.FileInfo dummy );
    }



    private GR.Memory.ByteBuffer TestAssembleTASM( string Source, out RetroDevStudio.Types.ASM.FileInfo Info )
    {
      RetroDevStudio.Parser.ASMFileParser parser = new RetroDevStudio.Parser.ASMFileParser();
      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.TASM;

      bool parseResult = parser.Parse( Source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo );
      if ( !parseResult )
      {
        Debug.Log( "Testassemble failed:" );
        foreach ( var msg in asmFileInfo.Messages )
        {
          Debug.Log( msg.Value.Message + " in line " + asmFileInfo.LineInfo[msg.Key].Line );
        }
      }


      Assert.IsTrue( parseResult );
      Assert.IsTrue( parser.Assemble( config ) );

      Info = asmFileInfo;

      return parser.AssembledOutput.Assembly;
    }



    [TestMethod]
    public void TestTASMSyntax()
    {
      string      source = @"  * = $2000
LABEL
                             .byte  1,2,3,4,5,6,7,8";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.TASM );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler  = RetroDevStudio.Types.AssemblerType.TASM;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( 1, parser.Warnings );
      Assert.AreEqual( RetroDevStudio.Parser.ParserBase.ParseMessage.LineType.WARNING, asmFileInfo.Messages.Values[0].Type );
      Assert.AreEqual( RetroDevStudio.Types.ErrorCode.W1000_UNUSED_LABEL, asmFileInfo.Messages.Values[0].Code );

      Assert.AreEqual( "00200102030405060708", assembly.Assembly.ToString() );
    }



    [TestMethod]
    public void TestTASMLocalLabels()
    {
      string      source = @"  * = $2000
                            -
                              lda #0
                            -
                              lda #1
                              jmp -
                              jmp --

                              jmp +
                              jmp ++
  
                            + 
                              lda #2
                            +
                              lda #3";

      var assembly = TestAssembleTASM( source );

      Assert.AreEqual( "0020A900A9014C02204C00204C10204C1220A902A903", assembly.ToString() );
    }





  }
}
