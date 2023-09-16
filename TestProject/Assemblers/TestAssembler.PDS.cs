using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
  public partial class TestAssembler
  {
    private GR.Memory.ByteBuffer TestAssemblePDS( string Source )
    {
      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.PDS );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.PDS;

      bool parseResult = parser.Parse( Source, null, config, null );
      if ( !parseResult )
      {
        Debug.Log( "Testassemble failed:" );
        foreach ( var msg in parser.Messages.Values )
        {
          Debug.Log( msg.Message );
        }
      }


      Assert.IsTrue( parseResult );
      Assert.IsTrue( parser.Assemble( config ) );

      return parser.AssembledOutput.Assembly;
    }



    [TestMethod]
    public void TestLoByteWithExpressionSettings1PDS()
    {
      string      source = @"  ORG $c000
                             P_SCREEN = $b400
                          lda #<P_SCREEN + ( 40 * 10 )";

      var assembly = TestAssemblePDS( source );

      Assert.AreEqual( "00C0A990", assembly.ToString() );
    }



    [TestMethod]
    public void TestHiByteWithExpressionSettings1PDS()
    {
      string      source = @"  ORG $c000
                             P_SCREEN = $b400
                          lda #>P_SCREEN + ( 40 * 10 )";

      var assembly = TestAssemblePDS( source );

      Assert.AreEqual( "00C0A9B5", assembly.ToString() );
    }

    

    [TestMethod]
    public void TestMacroPDS()
    {
      // do not shift the lines, they need to be on the very left
      string      source = @"  ORG $c000
lsmf MACRO
     lda #2
     ENDM
          
lsmf";

      var assembly = TestAssemblePDS( source );

      Assert.AreEqual( "00C0A902", assembly.ToString() );
    }




  }
}
