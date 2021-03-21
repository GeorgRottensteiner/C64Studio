using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
  public partial class TestAssembler
  {
    [TestMethod]
    public void TestC64StudioPseudoOpMacroAndSourceInfo1()
    {
      string   source = @"
                !to ""macro_macro.prg"",cbm

            ;* = $1000
            ;MACRO_START

            !macro fill5bytes v1,v2,v3,v4,v5
                      lda #v1
                      sta 1024
                      lda #v2
                      sta 1025
                      lda #v3
                      sta 1026
                      lda #v4
                      sta 1027
                      lda #v5
                      sta 1028
            !end



            ;MACRO_END

            ;!if ( MACRO_START != MACRO_END ) {
            ;!error Macro has size!
            ;}

            * = $2000
                      lda #$01
                      sta 53281
            CALLEDLED_MACRO
                      +fill5bytes 10,20,30,40,50
            CALLEDLED_MACRO_END
                      inc 53280
                      +fill5bytes 1,2,3,4,5

                      rts";

      C64Studio.Parser.ASMFileParser      parser = new C64Studio.Parser.ASMFileParser();
      parser.SetAssemblerType( C64Studio.Types.AssemblerType.C64_STUDIO );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      string    file;
      int       lineIndex;
      parser.ASMFileInfo.DocumentAndLineFromAddress( 0x2000, out file, out lineIndex );

      Assert.IsTrue( parser.ASMFileInfo.Labels.ContainsKey( "CALLEDLED_MACRO" ) );
      Assert.IsTrue( parser.ASMFileInfo.Labels.ContainsKey( "CALLEDLED_MACRO_END" ) );

      Assert.AreEqual( 28, lineIndex );

      var label = parser.ASMFileInfo.Labels["CALLEDLED_MACRO"];
      var label2 = parser.ASMFileInfo.Labels["CALLEDLED_MACRO_END"];

      Assert.AreEqual( 30, label.LocalLineIndex );
      Assert.AreEqual( 32, label2.LocalLineIndex );

      Assert.AreEqual( 0x2005, label.AddressOrValue );
      Assert.AreEqual( 0x201e, label2.AddressOrValue );

      var tokenInfo = parser.ASMFileInfo.TokenInfoFromName( "CALLEDLED_MACRO", "", "" );
      Assert.IsNotNull( tokenInfo );
      Assert.AreEqual( 30, tokenInfo.LocalLineIndex );

      tokenInfo = parser.ASMFileInfo.TokenInfoFromName( "CALLEDLED_MACRO_END", "", "" );
      Assert.IsNotNull( tokenInfo );
      Assert.AreEqual( 32, tokenInfo.LocalLineIndex );
    }



    [TestMethod]
    public void TestMacroWithLocalLabels()
    {
      string   source = @"* = $0801

        !basic

        LABEL_POS = $2000

        ; add immediate 16bit value to memory 
        !macro add16im .dest, .val {
              lda #<.val 
        clc
        adc .dest
        sta .dest
        lda #>.val 
        adc .dest + 1
        sta .dest + 1
        } 


        +add16im LABEL_POS, 256
        rts";


      C64Studio.Parser.ASMFileParser      parser = new C64Studio.Parser.ASMFileParser();
      parser.SetAssemblerType( C64Studio.Types.AssemblerType.C64_STUDIO );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;

      var assembly = TestAssembleC64Studio( source );
      //Assert.IsTrue( parser.Parse( source, null, config, null ) );
      //Assert.IsTrue( parser.Assemble( config ) );

      //var assembly = parser.AssembledOutput;

      Assert.AreEqual( "01080B080A009E32303631000000A900186D00208D0020A9016D01208D012060", assembly.ToString() );
    }

    

    [TestMethod]
    public void TestMacroWithImmediateForwardLabels()
    {
      string   source =@"*=$0801
                        !basic
                        !macro name
                          lda #0
                          beq +
                           lda #1
                           jmp ++
                          +
                           lda #2
                          ++
                        !end
                        +name";


      C64Studio.Parser.ASMFileParser      parser = new C64Studio.Parser.ASMFileParser();
      parser.SetAssemblerType( C64Studio.Types.AssemblerType.C64_STUDIO );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;

      var assembly = TestAssembleC64Studio( source );
      //Assert.IsTrue( parser.Parse( source, null, config, null ) );
      //Assert.IsTrue( parser.Assemble( config ) );

      //var assembly = parser.AssembledOutput;

      Assert.AreEqual( "01080B080A009E32303631000000A900F005A9014C1808A902", assembly.ToString() );
    }



    [TestMethod]
    public void TestFill1Param()
    {
      string      source = @"* = $2000
                             !fill 5";

      C64Studio.Parser.ASMFileParser      parser = new C64Studio.Parser.ASMFileParser();
      parser.SetAssemblerType( C64Studio.Types.AssemblerType.C64_STUDIO );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null ) );
      Assert.IsTrue( parser.Assemble( config ) );

      Assert.AreEqual( 7, (int)parser.AssembledOutput.Assembly.Length );
      Assert.AreEqual( "00200000000000", parser.AssembledOutput.Assembly.ToString() );
    }



    [TestMethod]
    public void TestFill2Params()
    {
      string      source = @"* = $2000
                             !fill 5,$17";

      C64Studio.Parser.ASMFileParser      parser = new C64Studio.Parser.ASMFileParser();
      parser.SetAssemblerType( C64Studio.Types.AssemblerType.C64_STUDIO );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null ) );
      Assert.IsTrue( parser.Assemble( config ) );

      Assert.AreEqual( 7, (int)parser.AssembledOutput.Assembly.Length );
      Assert.AreEqual( "00201717171717", parser.AssembledOutput.Assembly.ToString() );
    }



    [TestMethod]
    public void TestHex()
    {
      string      source = @"  * = $c000
                             !hex ""0123456789ABCDEF""";

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "00C00123456789ABCDEF", assembly.ToString() );
    }



    [TestMethod]
    public void TestPseudoOpBank()
    {
      string      source = @"!bank 0, $20
                              !pseudopc $8000

                              !byte 20

                              !bank 1, $20
                              !pseudopc $8000

                              !byte 30

                              !bank 2, $20
                              !pseudopc $8000

                              !byte 40";

      C64Studio.Parser.ASMFileParser      parser = new C64Studio.Parser.ASMFileParser();
      parser.SetAssemblerType( C64Studio.Types.AssemblerType.C64_STUDIO );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.crt";
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null ) );
      Assert.IsTrue( parser.Assemble( config ) );
      Assert.AreEqual( 0, parser.Messages.Count );  // no warnings regarding overlapped segments

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( "008014000000000000000000000000000000000000000000000000000000000000001E0000000000000000000000000000000000000000000000000000000000000028", assembly.Assembly.ToString() );
    }

  }
}
