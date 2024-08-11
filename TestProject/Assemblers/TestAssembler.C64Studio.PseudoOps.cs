using System;
using System.Linq;
using System.Web.Services.Description;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RetroDevStudio.Types;

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

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      string    file;
      int       lineIndex;
      asmFileInfo.DocumentAndLineFromAddress( 0x2000, out file, out lineIndex );

      Assert.IsTrue( asmFileInfo.Labels.ContainsKey( "CALLEDLED_MACRO" ) );
      Assert.IsTrue( asmFileInfo.Labels.ContainsKey( "CALLEDLED_MACRO_END" ) );

      Assert.AreEqual( 28, lineIndex );

      var label = asmFileInfo.Labels["CALLEDLED_MACRO"];
      var label2 = asmFileInfo.Labels["CALLEDLED_MACRO_END"];

      Assert.AreEqual( 30, label.LocalLineIndex );
      Assert.AreEqual( 32, label2.LocalLineIndex );

      Assert.AreEqual( 0x2005, label.AddressOrValue );
      Assert.AreEqual( 0x201e, label2.AddressOrValue );

      var tokenInfo = asmFileInfo.TokenInfoFromName( "CALLEDLED_MACRO", "", "" );
      Assert.IsNotNull( tokenInfo );
      Assert.AreEqual( 30, tokenInfo.LocalLineIndex );

      tokenInfo = asmFileInfo.TokenInfoFromName( "CALLEDLED_MACRO_END", "", "" );
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


      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

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


      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

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

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );
      Assert.IsTrue( parser.Assemble( config ) );

      Assert.AreEqual( 7, (int)parser.AssembledOutput.Assembly.Length );
      Assert.AreEqual( "00200000000000", parser.AssembledOutput.Assembly.ToString() );
    }



    [TestMethod]
    public void TestFill2Params()
    {
      string      source = @"* = $2000
                             !fill 5,$17";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );
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
    public void TestExpressionSubtract()
    {
      string      source = @"  * = $1000
                               lda #$50 - 8";

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "0010A948", assembly.ToString() );
    }



    [TestMethod]
    public void TestExpressionNegate()
    {
      string      source = @"  * = $1000
                               OF_ON_GROUND = 2
                               OF_ON_PLATFORM = 1
                               lda #~( OF_ON_GROUND | OF_ON_PLATFORM )";

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "0010A9FC", assembly.ToString() );
    }



    [TestMethod]
    public void TestExpressionNegative()
    {
      string      source = @"  * = $1000
                               !byte -( 8 + 4 )";

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "0010F4", assembly.ToString() );
    }



    [TestMethod]
    public void TestExpressionAdd()
    {
      // 100.35 is truncated to 100
      string      source = @"  * = $1000
                               lda #$50 + 20.35";

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "0010A964", assembly.ToString() );
    }



    [TestMethod]
    public void TestAssignmentPlusOperator()
    {
      string      source = @"  * = $1000
                               label1 = 5
                               label1 += 4

                                !byte label1";

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "001009", assembly.ToString() );
    }



    [TestMethod]
    public void TestAssignmentAndOperator()
    {
      string      source = @"  * = $1000
                               label1 = 255
                               label1 &= 129

                                !byte label1";

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "001081", assembly.ToString() );
    }



    [TestMethod]
    public void TestAssignmentMinusOperator()
    {
      string      source = @"  * = $1000
                               label1 = 5
                               label1 -= 4

                                !byte label1";

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "001001", assembly.ToString() );
    }



    // cannot use, ambigious with * = xxx
    /*
    [TestMethod]
    public void TestAssignmentMultiplyOperator()
    {
      string      source = @"  * = $1000
                               label1 = 5
                               label1 *= 4

                                !byte label1";

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "001014", assembly.ToString() );
    }*/



    [TestMethod]
    public void TestAssignmentDivideOperator()
    {
      string      source = @"  * = $1000
                               label1 = 9
                               label1 /= 4

                                !byte label1";

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "001002", assembly.ToString() );
    }



    [TestMethod]
    public void TestAssignmentModulusOperator()
    {
      string      source = @"  * = $1000
                               label1 = 5
                               label1 %= 3

                                !byte label1";

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "001002", assembly.ToString() );
    }



    [TestMethod]
    public void TestAssignmentShiftLeftOperator()
    {
      string      source = @"  * = $1000
                               label1 = 5
                               label1 <<= 2

                                !byte label1";

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "001014", assembly.ToString() );
    }



    [TestMethod]
    public void TestAssignmentShiftRightOperator()
    {
      string      source = @"  * = $1000
                               label1 = 5
                               label1 >>= 2

                                !byte label1";

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "001001", assembly.ToString() );
    }



    [TestMethod]
    public void TestTempLabelLateResolution()
    {
      // 100.35 is truncated to 100
      string      source = @"  * = $1000
                               lda latelabel

                          latelabel
                               asl
                          latelabel = latelabel + 3
                              !byte 50";

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "0010AD03100A32", assembly.ToString() );
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

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.crt";
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );
      Assert.IsTrue( parser.Assemble( config ) );
      Assert.AreEqual( 0, asmFileInfo.Messages.Count );  // no warnings regarding overlapped segments

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( "008014000000000000000000000000000000000000000000000000000000000000001E0000000000000000000000000000000000000000000000000000000000000028", assembly.Assembly.ToString() );
    }



    [TestMethod]
    public void TestPseudoOpZone()
    {
      // nested zone end restores global zone
      string      source = @"* = $1000 
                             !zone gnu
                             
                             jmp .temp

                              !zone inner {

                              .temp !byte 0

                              }

                              .temp
                              !byte 1";

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "00104C04100001", assembly.ToString() );
    }



    [TestMethod]
    public void TestPseudoOpSkip()
    {
      // nested zone end restores global zone
      string      source = @"* = $1000 
                             !skip 5
                          
                             !byte 1";

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "051001", assembly.ToString() );
    }



    [TestMethod]
    public void TestPseudoOpByteWordDWord()
    {
      string      source = @"!to ""po_byte_word_dword.prg"",cbm
                * = $2000
                          !byte 1
                          !byte 2,3,4,5,6,LATE_EXPRESSION_7
                          !byte %...##...
                          !byte ""$"",""$""+128

                          !word 1
                          !16 2,3,4,5,6,LATE_EXPRESSION_7
                          !le16 2,3,4,5,6,LATE_EXPRESSION_7
                          !be16 1
                          !be16 2,3,4,5,6,LATE_EXPRESSION_7
                          !word ""$"",""$""+128
          
                          !dword $01020304
                          !32 $05060708,$090a0b0c,$ffeeddcc
                          !le32 2,3,4,5,6,LATE_EXPRESSION_7
                          !be32 1
                          !dword ""$"",""$""+128

      LATE_EXPRESSION_7 = 7";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.crt";
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );
      Assert.IsTrue( parser.Assemble( config ) );
      Assert.AreEqual( 0, asmFileInfo.Messages.Count );  // no warnings regarding overlapped segments

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( "0020010203040506071824A4010002000300040005000600070002000300040005000600070000010002000300040005000600072400A40004030201080706050C0B0A09CCDDEEFF0200000003000000040000000500000006000000070000000000000124000000A4000000", assembly.Assembly.ToString() );
    }



    [TestMethod]
    public void TestPseudoOpJumpList()
    {
      string      source = @"* = $2000
                          Routine1
                            lda #1
                            rts

                          Routine2
                            lda #2
                            rts

                          JUMP_LIST
                            !jumplist Routine1
                            !jumplist Routine2

                          OFFSET_DATA
                            !byte JUMP_LIST.Routine1
                            !byte $17
                            !byte JUMP_LIST.Routine2";

      var assembly = TestAssembleC64Studio( source );
      Assert.AreEqual( "0020A90160A9026000200320001702", assembly.ToString() );
  
    }



    [TestMethod]
    public void TestPseudoOpJumpListLateEval()
    {
      string      source = @"* = $2000
                          JUMP_LIST
                            !jumplist Routine1
                            !jumplist Routine2

                          OFFSET_DATA
                            !byte JUMP_LIST.Routine1
                            !byte $17
                            !byte JUMP_LIST.Routine2

                          Routine1
                            lda #1
                            rts

                          Routine2
                            lda #2
                            rts";

      var assembly = TestAssembleC64Studio( source );
      Assert.AreEqual( "002007200A20001702A90160A90260", assembly.ToString() );
    }



    [TestMethod]
    public void TestPseudoOpJumpListLateEval2()
    {
      string      source = @"* = $2000
                          OFFSET_DATA
                            !byte JUMP_LIST.Routine1
                            !byte $17
                            !byte JUMP_LIST.Routine2

                          JUMP_LIST
                            !jumplist Routine1
                            !jumplist Routine2

                          Routine1
                            lda #1
                            rts

                          Routine2
                            lda #2
                            rts";

      var assembly = TestAssembleC64Studio( source );
      Assert.AreEqual( "002000170207200A20A90160A90260", assembly.ToString() );
    }



    [TestMethod]
    public void TestPOWhile()
    {
      string source = @"* = $2000
                        a = 5
                        !while a > 0 {
                                  !byte a
                                  a = a - 1
                        }";

      var assembly = TestAssembleC64Studio( source, out GR.Collections.MultiMap<int, RetroDevStudio.Parser.ParserBase.ParseMessage> messages );

      Assert.AreEqual( "00200504030201", assembly.ToString() );
    }



    [TestMethod]
    public void TestPseudoOpMessage()
    {
      string      source = @"* = $2000
                          !zone zone
                          .local = 1
                          !message ""#1 "", .local
                          !message ""#2 "", zone.local";

      var assembly = TestAssembleC64Studio( source, out GR.Collections.MultiMap<int, RetroDevStudio.Parser.ParserBase.ParseMessage> messages );

      Assert.AreEqual( 2, messages.Count );
      Assert.AreEqual( "#1 1/$1", messages.Values.First().Message );
      Assert.AreEqual( "#2 1/$1", messages.Values[1].Message );

      Assert.AreEqual( "0020", assembly.ToString() );
    }



    [TestMethod]
    public void TestPseudoOpMessageLabelArithmetic()
    {
      string      source = @"n1 = ""a""
                            m1 = n1 + 1
                            !message ""m1='"", m1, ""'""

                            n2 = ""abc""
                            m2 = n2 + 1
                            !message ""m2='"", m2, ""'""

                            ; 'a1/$1'
                            o = 1
                            !message ""o='"", o, ""'""";

      var assembly = TestAssembleC64Studio( source, out GR.Collections.MultiMap<int, RetroDevStudio.Parser.ParserBase.ParseMessage> messages );

      Assert.AreEqual( 3, messages.Count );
      Assert.AreEqual( "m1='b'", messages.Values[0].Message );
      Assert.AreEqual( "m2='abc1'", messages.Values[1].Message );
      Assert.AreEqual( "o='1/$1'", messages.Values[2].Message );

      Assert.AreEqual( "0000", assembly.ToString() );
    }




    [TestMethod]
    public void TestMacroOverloading()
    {
      // do not shift the lines, they need to be on the very left
      string      source = @"* = $1000
  !macro callme arg1, arg2
            lda #arg1
            sta arg2
  !end

  !macro callme arg1
            lda #0
            sta arg1
  !end

            +callme 14, $d020
            +callme $d020
            rts";

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "0010A90E8D20D0A9008D20D060", assembly.ToString() );
    }



    [TestMethod]
    public void TestMacroC64Studio()
    {
      // do not shift the lines, they need to be on the very left
      string      source = @"  * = $c000
                          !macro lsmf
                               lda #2
                          !end
          
                          +lsmf";

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "00C0A902", assembly.ToString() );
    }



    [TestMethod]
    public void TestPseudoOpIfDefChainAvoidingEvaluationInElseBranches()
    {
      string    source = @"*=$c000
        !macro mult8_snippet2 .mand {
          ;load mand into Y
          !ifndef .mand {
            !message ""address with forward reference""
            ldy.mand 
          } else if .mand != ""Y"" {
            !message ""address with non-forward reference""
            ldy.mand ;3/4 cycles
          } else {
            ; mand is Y
            !message ""mand is Y""
            }; !ifndef mand
          } ; !macro mult8_snippet2

        +mult8_snippet2 addr_forward
        addr_forward: nop";

      var assembly = TestAssembleC64Studio( source, out GR.Collections.MultiMap<int, RetroDevStudio.Parser.ParserBase.ParseMessage> messages );

      Assert.AreEqual( 3, messages.Count );
      Assert.AreEqual( "address with forward reference", messages.Values[0].Message );
      Assert.AreEqual( "Unused label ldy.mand", messages.Values[1].Message );
      Assert.AreEqual( "Unused label addr_forward", messages.Values[2].Message );

      Assert.AreEqual( "00C0EA", assembly.ToString() );
    }



    [TestMethod]
    public void TestPseudoOpIfDefChainAvoidingEvaluationInElseBranches2()
    {
      string    source = @"*=$c000
        !macro mult8_snippet2 .mand {
          ;load mand into Y
          !ifndef .mand {
            !message ""address with forward reference""
            ldy.mand 
          } else if .mand != ""X"" {
            !message ""address with non-forward reference 1""
            ldy.mand ;3/4 cycles
          } else if .mand != ""Y"" {
            !message ""address with non-forward reference 2""
            ldy.mand ;3/4 cycles
          } else {
            ; mand is Y
            !message ""mand is Y""
            }; !ifndef mand
          } ; !macro mult8_snippet2

        +mult8_snippet2 addr_forward
        addr_forward: nop";

      var assembly = TestAssembleC64Studio( source, out GR.Collections.MultiMap<int, RetroDevStudio.Parser.ParserBase.ParseMessage> messages );

      Assert.AreEqual( 3, messages.Count );
      Assert.AreEqual( "address with forward reference", messages.Values[0].Message );
      Assert.AreEqual( "Unused label ldy.mand", messages.Values[1].Message );
      Assert.AreEqual( "Unused label addr_forward", messages.Values[2].Message );

      Assert.AreEqual( "00C0EA", assembly.ToString() );
    }


  }
}
