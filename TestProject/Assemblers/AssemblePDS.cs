using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
  [TestClass]
  public class AssemblePDS
  {
    private GR.Memory.ByteBuffer TestAssemblePDS( string Source )
    {
      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.PDS );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.PDS;

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



    [TestMethod]
    public void TestPDSBinaryOrExclamationMark()
    {
      // do not shift the lines, they need to be on the very left
      string      source = @"  ORG $d000
LEV=3
  IF LEV=2 ! LEV=4 
     lda #2
  ENDIF";

      var assembly = TestAssemblePDS( source );

      Assert.AreEqual( "00D0", assembly.ToString() );
    }



    [TestMethod]
    public void TestPDSAndPrefix()
    {
      // & can be used as hex prefix
      string      source = @"  ORG $d000
JIFFY
TOGGLETAB EQU 5
            ADC #TOGGLETAB+1&255
!2SYNCH LDA JIFFY
  GNU EQU (&BF00-&0C00)+5
            ";

      var assembly = TestAssemblePDS( source );

      Assert.AreEqual( "00D06906AD00D0", assembly.ToString() );
    }




  }
}
