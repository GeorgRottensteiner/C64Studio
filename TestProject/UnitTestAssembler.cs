using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
  [TestClass]
  public class UnitTestAssembler
  {
    [TestMethod]
    public void TestOverlappingSegmentWarning()
    {
      string      source = @"* = $2000

                             !fill 256
                             * = $20fe
                             rts";

      C64Studio.Parser.ASMFileParser      parser = new C64Studio.Parser.ASMFileParser();
      parser.SetAssemblerType( C64Studio.Types.AssemblerType.C64_STUDIO );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( 1, parser.Warnings );
      Assert.AreEqual( C64Studio.Parser.ParserBase.ParseMessage.LineType.SEVERE_WARNING, parser.Messages.Values[0].Type  );
      Assert.AreEqual( C64Studio.Types.ErrorCode.W0001_SEGMENT_OVERLAP, parser.Messages.Values[0].Code );

      Assert.AreEqual( "002000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000006000", assembly.Assembly.ToString() );
    }



    [TestMethod]
    public void TestPotentialWarningForIndirectJMP()
    {
      string      source = @"* = $2000

                             JUMP_ADDRESS = $3fff

                             jmp (JUMP_ADDRESS )";

      C64Studio.Parser.ASMFileParser      parser = new C64Studio.Parser.ASMFileParser();
      parser.SetAssemblerType( C64Studio.Types.AssemblerType.C64_STUDIO );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( 1, parser.Warnings );
      Assert.AreEqual( C64Studio.Parser.ParserBase.ParseMessage.LineType.WARNING, parser.Messages.Values[0].Type );
      Assert.AreEqual( C64Studio.Types.ErrorCode.W0007_POTENTIAL_PROBLEM, parser.Messages.Values[0].Code );

      Assert.AreEqual( "00206CFF3F", assembly.Assembly.ToString() );
    }



    [TestMethod]
    public void TestPotentialWarningForIndirectJMPWithLateEvaluation()
    {
      string      source = @"* = $2000

                             jmp (JUMP_ADDRESS )

                            JUMP_ADDRESS = $3fff";

      C64Studio.Parser.ASMFileParser      parser = new C64Studio.Parser.ASMFileParser();
      parser.SetAssemblerType( C64Studio.Types.AssemblerType.C64_STUDIO );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( 1, parser.Warnings );
      Assert.AreEqual( C64Studio.Parser.ParserBase.ParseMessage.LineType.WARNING, parser.Messages.Values[0].Type );
      Assert.AreEqual( C64Studio.Types.ErrorCode.W0007_POTENTIAL_PROBLEM, parser.Messages.Values[0].Code );

      Assert.AreEqual( "00206CFF3F", assembly.Assembly.ToString() );
    }



    [TestMethod]
    public void TestDASMSyntax()
    {
      string      source = @"ORG $2000

                             LABEL
                             dc.b  1,2,3,4,5,6,7,8";

      C64Studio.Parser.ASMFileParser      parser = new C64Studio.Parser.ASMFileParser();
      parser.SetAssemblerType( C64Studio.Types.AssemblerType.PDS );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.DASM;

      Assert.IsTrue( parser.Parse( source, null, config ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( 1, parser.Warnings );
      Assert.AreEqual( C64Studio.Parser.ParserBase.ParseMessage.LineType.WARNING, parser.Messages.Values[0].Type );
      Assert.AreEqual( C64Studio.Types.ErrorCode.W1000_UNUSED_LABEL, parser.Messages.Values[0].Code );

      Assert.AreEqual( "00200102030405060708", assembly.Assembly.ToString() );
    }


    [TestMethod]
    public void TestSourceInfo1()
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

      Assert.IsTrue( parser.Parse( source, null, config ) );

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

      Assert.IsTrue( parser.Parse( source, null, config ) );
      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( "01080B080A009E32303631000000A900186D00208D0020A9016D01208D012060", assembly.Assembly.ToString() );
    }



    [TestMethod]
    public void TestSegmentOutOfBounds()
    {
      // this test is probably obsolete as you can assemble files bigger than $ffff
      /*
      string      source = @"* = $ffff
                            lda #$ff
                            Zeropage_Routine = $0020
                            jmp Zeropage_Routine
                            !byte 50+100";

      C64Studio.Parser.ASMFileParser      parser = new C64Studio.Parser.ASMFileParser();
      parser.SetAssemblerType( C64Studio.Types.AssemblerType.C64_STUDIO );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( 1, parser.Errors );
      Assert.AreEqual( C64Studio.Parser.ParserBase.ParseMessage.LineType.ERROR, parser.Messages.Values[0].Type );
      Assert.AreEqual( C64Studio.Types.ErrorCode.E1106_SEGMENT_OUT_OF_BOUNDS, parser.Messages.Values[0].Code );*/
    }



    [TestMethod]
    public void TestValueOutOfBoundsLiteral()
    {
      string      source = @"* = $ff00
                             jmp $12345";

      C64Studio.Parser.ASMFileParser      parser = new C64Studio.Parser.ASMFileParser();
      parser.SetAssemblerType( C64Studio.Types.AssemblerType.C64_STUDIO );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;

      Assert.IsFalse( parser.Parse( source, null, config ) );

      Assert.AreEqual( 1, parser.Errors );
      Assert.AreEqual( C64Studio.Parser.ParserBase.ParseMessage.LineType.ERROR, parser.Messages.Values[0].Type );
      Assert.AreEqual( C64Studio.Types.ErrorCode.E1003_VALUE_OUT_OF_BOUNDS_WORD, parser.Messages.Values[0].Code );
    }



    [TestMethod]
    public void TestValueOutOfBoundsLabel()
    {
      string      source = @"* = $ffff
                             lda #$ff
                             jmp Zeropage_Routine2
                             Zeropage_Routine2";

      C64Studio.Parser.ASMFileParser      parser = new C64Studio.Parser.ASMFileParser();
      parser.SetAssemblerType( C64Studio.Types.AssemblerType.C64_STUDIO );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;

      Assert.IsFalse( parser.Parse( source, null, config ) );

      Assert.AreEqual( 1, parser.Errors );
      Assert.AreEqual( C64Studio.Parser.ParserBase.ParseMessage.LineType.ERROR, parser.Messages.Values[0].Type );
      Assert.AreEqual( C64Studio.Types.ErrorCode.E1003_VALUE_OUT_OF_BOUNDS_WORD, parser.Messages.Values[0].Code );
    }



    [TestMethod]
    public void TestHugeFile()
    {
      string      source = @"* = $2000
                             !fill $80000";

      C64Studio.Parser.ASMFileParser      parser = new C64Studio.Parser.ASMFileParser();
      parser.SetAssemblerType( C64Studio.Types.AssemblerType.C64_STUDIO );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config ) );
      Assert.IsTrue( parser.Assemble( config ) );

      Assert.AreEqual( 0x80002, (int)parser.AssembledOutput.Assembly.Length );
    }



    [TestMethod]
    public void TestLocalLabelInLoop()
    {
      string      source = @"!for j = 0 to 5
           *=$4000+256*j
.Einsprung   Lda #20
;.Zweisprung  Ldx $2000+8*j
            jsr .Einsprung
            inx
!end";

      C64Studio.Parser.ASMFileParser      parser = new C64Studio.Parser.ASMFileParser();
      parser.SetAssemblerType( C64Studio.Types.AssemblerType.C64_STUDIO );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config ) );
      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( "0040A914200040E800000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000A914200041E800000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000A914200042E800000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000A914200043E800000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000A914200044E800000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000A914200045E8", assembly.Assembly.ToString() );
    }

  }
}
