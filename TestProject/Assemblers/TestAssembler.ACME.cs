using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
  public partial class TestAssembler
  {
    private GR.Memory.ByteBuffer TestAssembleACME( string Source )
    {
      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.ACME );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.ACME;

      bool parseResult = parser.Parse( Source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo );
      if ( !parseResult )
      {
        Debug.Log( "Testassemble failed:" );
        foreach ( var msg in asmFileInfo.Messages.Values )
        {
          Debug.Log( msg.Message );
        }
      }


      Assert.IsTrue( parseResult );
      Assert.IsTrue( parser.Assemble( config ) );

      return parser.AssembledOutput.Assembly;
    }



    [TestMethod]
    public void TestOperatorPrecedence()
    {
      string      source = @"* = $2000
                              lda #>$c000 + 1";

      // ACME evaluates $c000 + 1 first, then the >
      var assembly = TestAssembleACME( source );

      Assert.AreEqual( "0020A9C0", assembly.ToString() );
    }



  }
}
