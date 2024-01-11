using System;
using System.Diagnostics;
using System.Linq;
using LibGit2Sharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RetroDevStudio.Types;

namespace TestProject
{
  [TestClass]
  public partial class TestAssembler
  {
    [TestMethod]
    public void TestAssemblyOpcodeImplicitNoGarbage()
    {
      string      source = @"* = $1000
                            GNU = *
                            iny GNU";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      bool parseResult = parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo );
      Assert.IsFalse( parseResult );
    }



    [TestMethod]
    public void TestAssemblyOpcodeDetectionAbsolute()
    {
      string      source = @"* = $1000
                            lda $1234";

      var assembly = TestAssembleC64Studio( source, out RetroDevStudio.Types.ASM.FileInfo info );

      Assert.AreEqual( "0010AD3412", assembly.ToString() );
    }



    [TestMethod]
    public void TestAssemblyOpcodeDetectionAbsoluteNotIndirect()
    {
      string      source = @"* = $1000
                            cmp ($1234 + 4 ) * 10";

      var assembly = TestAssembleC64Studio( source, out RetroDevStudio.Types.ASM.FileInfo info );

      Assert.AreEqual( "0010CD30B6", assembly.ToString() );
    }



    [TestMethod]
    public void TestAssemblyOpcodeDetectionAbsoluteSet()
    {
      string      source = @"!set * = $1000
                            lda $1234";

      var assembly = TestAssembleC64Studio( source, out RetroDevStudio.Types.ASM.FileInfo info );

      Assert.AreEqual( "0010AD3412", assembly.ToString() );
    }



    [TestMethod]
    public void TestAssemblyOpcodeDetectionZeropage()
    {
      string      source = @"* = $1000
                            lda $12";

      var assembly = TestAssembleC64Studio( source, out RetroDevStudio.Types.ASM.FileInfo info );

      Assert.AreEqual( "0010A512", assembly.ToString() );
    }



    [TestMethod]
    public void TestAssemblyOpcodeDetectionZeropageX()
    {
      string      source = @"* = $1000
                            lda $12,x";

      var assembly = TestAssembleC64Studio( source, out RetroDevStudio.Types.ASM.FileInfo info );

      Assert.AreEqual( "0010B512", assembly.ToString() );
    }



    [TestMethod]
    public void TestAssemblyOpcodeDetectionNoZeropageY()
    {
      string      source = @"* = $1000
                            lda $12,y";

      var assembly = TestAssembleC64Studio( source, out RetroDevStudio.Types.ASM.FileInfo info );

      Assert.AreEqual( "0010B91200", assembly.ToString() );
    }



    [TestMethod]
    public void TestAssemblyOpcodeDetectionAbsoluteX()
    {
      string      source = @"* = $1000
                            lda $1234,x";

      var assembly = TestAssembleC64Studio( source, out RetroDevStudio.Types.ASM.FileInfo info );

      Assert.AreEqual( "0010BD3412", assembly.ToString() );
    }



    [TestMethod]
    public void TestAssemblyOpcodeDetectionNoIndirectAbsoluteX()
    {
      string      source = @"* = $1000
                            lda ($1234),x";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      bool parseResult = parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo );
      Assert.IsFalse( parseResult );
    }



    [TestMethod]
    public void TestOpcodeDetectionZeropageIndirectX()
    {
      string      source = @"* = $1000
                            adc($12, x )";

      var assembly = TestAssembleC64Studio( source, out RetroDevStudio.Types.ASM.FileInfo info );

      Assert.AreEqual( "00106112", assembly.ToString() );
    }



    [TestMethod]
    public void TestOpcodeDetectionNoZeropageIndirectX()
    {
      string      source = @"* = $1000
                            lda ($12),x";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      bool parseResult = parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo );
      Assert.IsFalse( parseResult );
    }



    [TestMethod]
    public void TestOpcodeDetectionNoZeropageIndirectX2()
    {
      string      source = @"LABEL = $12
                              * = $1000
                            sta (LABEL),x";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      bool parseResult = parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo );
      Assert.IsFalse( parseResult );
    }



    [TestMethod]
    public void TestAssemblyOpcodeDetectionZeropageRelativeY()
    {
      string      source = @"* = $1000
                            lda ($12),y";

      var assembly = TestAssembleC64Studio( source, out RetroDevStudio.Types.ASM.FileInfo info );

      Assert.AreEqual( "0010B112", assembly.ToString() );
    }



    [TestMethod]
    public void TestAssemblyOpcodeDetectionNoZeropageRelativeY()
    {
      string      source = @"* = $1000
                            lda ($1234),y";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsFalse( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );
    }



    [TestMethod]
    public void TestAssemblyOpcodeEvaluationOpcodeOffsets()
    {
      string      source = @"!cpu 6510
                        !to ""cpu6510.bin"",plain

                        LABEL_VALUE = 128

                        * = $1000

                        sta LABEL_VALUE - 1,x
                        sta LABEL_VALUE + 1,y
                        sta ( LABEL_VALUE - 1 ),y
                        sta ( LABEL_VALUE + 1, x )";

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "0010957F998100917F8181", assembly.ToString() );
    }



    private GR.Memory.ByteBuffer TestAssembleC64Studio( string Source, out RetroDevStudio.Types.ASM.FileInfo Info )
    {
      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

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



    private GR.Memory.ByteBuffer TestAssembleC64Studio( string Source, out GR.Collections.MultiMap<int, RetroDevStudio.Parser.ParserBase.ParseMessage> Messages )
    {
      return TestAssembleC64Studio( Source, out Messages, out RetroDevStudio.Types.ASM.FileInfo info );
    }



    private GR.Memory.ByteBuffer TestAssembleC64Studio( string Source, out GR.Collections.MultiMap<int, RetroDevStudio.Parser.ParserBase.ParseMessage> Messages, out RetroDevStudio.Types.ASM.FileInfo Info )
    {
      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      bool parseResult = parser.Parse( Source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo );
      Messages = asmFileInfo.Messages;
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

      Info = asmFileInfo;

      return parser.AssembledOutput.Assembly;
    }



    private GR.Memory.ByteBuffer TestAssembleC64Studio( string Source )
    {
      return TestAssembleC64Studio( Source, out RetroDevStudio.Types.ASM.FileInfo Info );
    }



    [TestMethod]
    public void TestStringLiteralWithMoreThanOneCharacter()
    {
      string      source = @"* = $0801
                !byte 2,""http://www.georg-rottensteiner.de/test/haus1.html""
                !byte ""http://www.georg-rottensteiner.de/test/haus1.html""";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsFalse( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      Assert.AreEqual( 2, parser.Errors );
      Assert.AreEqual( RetroDevStudio.Parser.ParserBase.ParseMessage.LineType.ERROR, asmFileInfo.Messages.Values[0].Type );
      Assert.AreEqual( RetroDevStudio.Types.ErrorCode.E1000_SYNTAX_ERROR, asmFileInfo.Messages.Values[0].Code );
      Assert.IsTrue( asmFileInfo.Messages.Values[0].Message.Contains( "More than one" ) );

      Assert.AreEqual( RetroDevStudio.Parser.ParserBase.ParseMessage.LineType.ERROR, asmFileInfo.Messages.Values[1].Type );
      Assert.AreEqual( RetroDevStudio.Types.ErrorCode.E1000_SYNTAX_ERROR, asmFileInfo.Messages.Values[1].Code );
      Assert.IsTrue( asmFileInfo.Messages.Values[1].Message.Contains( "More than one" ) );
    }



    [TestMethod]
    public void TestOverlappingSegmentWarning()
    {
      string      source = @"* = $2000

                             !fill 256
                             * = $20fe
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

      Assert.AreEqual( 1, parser.Warnings );
      Assert.AreEqual( RetroDevStudio.Parser.ParserBase.ParseMessage.LineType.SEVERE_WARNING, asmFileInfo.Messages.Values[0].Type  );
      Assert.AreEqual( RetroDevStudio.Types.ErrorCode.W0001_SEGMENT_OVERLAP, asmFileInfo.Messages.Values[0].Code );

      Assert.AreEqual( "002000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000006000", assembly.Assembly.ToString() );
    }



    [TestMethod]
    public void TestVirtualSegment()
    {
      // assembly starts at $2000 since all before is virtual
      string      source = @"* = $2

          VALUE_1   
            !byte ?
          VALUE_2   
            !word ?

        * = $0801
              lda VALUE_1
              sta VALUE_2
              rts";

      var assembly = TestAssembleC64Studio( source, out RetroDevStudio.Types.ASM.FileInfo info );

      Assert.AreEqual( "0108A502850360", assembly.ToString() );
      Assert.AreEqual( 0x0002, info.Labels["VALUE_1"].AddressOrValue );
      Assert.AreEqual( 0x0003, info.Labels["VALUE_2"].AddressOrValue );
    }



    [TestMethod]
    public void TestVirtualSegmentTurningNonVirtual()
    {
      // assembly would start at $2000 but doesn't anymore
      // the first actual value is at $0005, so that's where the assembly is put at
      string      source = @"* = $2

          VALUE_1   
            !byte ?
          VALUE_2   
            !word ?
          GNU
            !byte 1

        * = $0801
              lda VALUE_1
              sta VALUE_2
              rts";

      var assembly = TestAssembleC64Studio( source, out RetroDevStudio.Types.ASM.FileInfo info );

      Assert.AreEqual( "050001000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000A502850360", assembly.ToString() );
      Assert.AreEqual( 0x0002, info.Labels["VALUE_1"].AddressOrValue );
      Assert.AreEqual( 0x0003, info.Labels["VALUE_2"].AddressOrValue );
    }



    [TestMethod]
    public void TestPotentialWarningForIndirectJMP()
    {
      string      source = @"* = $2000

                             JUMP_ADDRESS = $3fff

                             jmp (JUMP_ADDRESS )";

      var assembly = TestAssembleC64Studio( source, out GR.Collections.MultiMap<int, RetroDevStudio.Parser.ParserBase.ParseMessage> Messages, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo );

      Assert.AreEqual( 1, Messages.Count );
      Assert.AreEqual( RetroDevStudio.Parser.ParserBase.ParseMessage.LineType.WARNING, asmFileInfo.Messages.Values[0].Type );
      Assert.AreEqual( RetroDevStudio.Types.ErrorCode.W0007_POTENTIAL_PROBLEM, asmFileInfo.Messages.Values[0].Code );

      Assert.AreEqual( "00206CFF3F", assembly.ToString() );
    }



    [TestMethod]
    public void TestPotentialWarningForIndirectJMPWithLateEvaluation()
    {
      string      source = @"* = $2000

                             jmp (JUMP_ADDRESS )

                            JUMP_ADDRESS = $3fff";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( 1, parser.Warnings );
      Assert.AreEqual( RetroDevStudio.Parser.ParserBase.ParseMessage.LineType.WARNING, asmFileInfo.Messages.Values[0].Type );
      Assert.AreEqual( RetroDevStudio.Types.ErrorCode.W0007_POTENTIAL_PROBLEM, asmFileInfo.Messages.Values[0].Code );

      Assert.AreEqual( "00206CFF3F", assembly.Assembly.ToString() );
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

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( 1, parser.Errors );
      Assert.AreEqual( RetroDevStudio.Parser.ParserBase.ParseMessage.LineType.ERROR, parser.Messages.Values[0].Type );
      Assert.AreEqual( RetroDevStudio.Types.ErrorCode.E1106_SEGMENT_OUT_OF_BOUNDS, parser.Messages.Values[0].Code );*/
    }



    [TestMethod]
    public void TestValueOutOfBoundsLiteral()
    {
      string      source = @"* = $ff00
                             jmp $12345";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsFalse( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      Assert.AreEqual( 1, parser.Errors );
      Assert.AreEqual( RetroDevStudio.Parser.ParserBase.ParseMessage.LineType.ERROR, asmFileInfo.Messages.Values[0].Type );
      Assert.AreEqual( RetroDevStudio.Types.ErrorCode.E1003_VALUE_OUT_OF_BOUNDS_WORD, asmFileInfo.Messages.Values[0].Code );
    }



    [TestMethod]
    public void TestValueOutOfBoundsLabel()
    {
      string      source = @"* = $ffff
                             lda #$ff
                             jmp Zeropage_Routine2
                             Zeropage_Routine2";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsFalse( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      Assert.AreEqual( 1, parser.Errors );
      Assert.AreEqual( RetroDevStudio.Parser.ParserBase.ParseMessage.LineType.ERROR, asmFileInfo.Messages.Values[0].Type );
      Assert.AreEqual( RetroDevStudio.Types.ErrorCode.E1003_VALUE_OUT_OF_BOUNDS_WORD, asmFileInfo.Messages.Values[0].Code );
    }



    [TestMethod]
    public void TestHugeFile()
    {
      string      source = @"* = $2000
                             !fill $80000";

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( 0x80002, (int)assembly.Length );
    }



    [TestMethod]
    public void TestInternalLabelInLoop()
    {
      string      source = @"* = $2000
                              SCREEN_RAM = $0400
                              MAP
  
                              !zone ShiftMap
                              ShiftMap
                                !for i=0 to 25
                                  ldy #$00
                              -
                                  lda SCREEN_RAM + $28 * i + 1,y
                                  sta SCREEN_RAM + $28 * i + 0,y
                                  iny
                                  cmp #$27
                                  bne -
                                  lda MAP + $100 * i + $28,x
                                  sta SCREEN_RAM + $28 * i + $27
                                !end
                                rts";

      var assembly = TestAssembleC64Studio( source, out RetroDevStudio.Types.ASM.FileInfo Info );

      Assert.AreEqual( "0020A000B90104990004C8C927D0F5BD28208D2704A000B92904992804C8C927D0F5BD28218D4F04A000B95104995004C8C927D0F5BD28228D7704A000B97904997804C8C927D0F5BD28238D9F04A000B9A10499A004C8C927D0F5BD28248DC704A000B9C90499C804C8C927D0F5BD28258DEF04A000B9F10499F004C8C927D0F5BD28268D1705A000B91905991805C8C927D0F5BD28278D3F05A000B94105994005C8C927D0F5BD28288D6705A000B96905996805C8C927D0F5BD28298D8F05A000B99105999005C8C927D0F5BD282A8DB705A000B9B90599B805C8C927D0F5BD282B8DDF05A000B9E10599E005C8C927D0F5BD282C8D0706A000B90906990806C8C927D0F5BD282D8D2F06A000B93106993006C8C927D0F5BD282E8D5706A000B95906995806C8C927D0F5BD282F8D7F06A000B98106998006C8C927D0F5BD28308DA706A000B9A90699A806C8C927D0F5BD28318DCF06A000B9D10699D006C8C927D0F5BD28328DF706A000B9F90699F806C8C927D0F5BD28338D1F07A000B92107992007C8C927D0F5BD28348D4707A000B94907994807C8C927D0F5BD28358D6F07A000B97107997007C8C927D0F5BD28368D9707A000B99907999807C8C927D0F5BD28378DBF07A000B9C10799C007C8C927D0F5BD28388DE707A000B9E90799E807C8C927D0F5BD28398D0F0860", assembly.ToString() );
    }


    [TestMethod]
    public void TestIfLocalLabels()
    {
      string      source = @"*=$c000
                            !zone mathlib
                            .TRUE = 1=1
                            .FALSE = 1=0
                            .UNKNOWN = 2

                            .C_is_set = .TRUE
                            .C_is_set = .TRUE
                            ;debug
                            !message ""DEBUG.C_is_set = "", .C_is_set  ;it has a value,
                            !message ""DEBUG .TRUE="", .TRUE

                            !ifndef.C_is_set {
                                    ; it's defined, 
                              !message "".C_is_set is not defined""
                            }

                                  !if .C_is_set {
                                    ; yet, E1001 could not evaluate
                                    !message "".C_is_set is TRUE""
                                  }
                                  ";

      var assembly = TestAssembleC64Studio( source, out GR.Collections.MultiMap<int, RetroDevStudio.Parser.ParserBase.ParseMessage> messages );

      Assert.AreEqual( 3, messages.Count );
      Assert.AreEqual( "DEBUG.C_is_set = 255/$FF", messages.Values[0].Message );
      Assert.AreEqual( "DEBUG .TRUE=255/$FF", messages.Values[1].Message );
      Assert.AreEqual( ".C_is_set is TRUE", messages.Values[2].Message );

      Assert.AreEqual( "00C0", assembly.ToString() );
    }



    


    [TestMethod]
    public void TestIfnDefLocalLabels()
    {
      string      source = @"* = $c000
                        !zone library
                        .local = 2
                        !message ""library.test = "", library.local
                        !message ""(library.)test = "", .local
                        !ifndef library.local {
                              !message ""not defined""
                        } else {
                        !message ""defined""
                        }
                        !ifndef.local {
                        !message ""not defined""
                        } else {
                          !message ""defined""
                        }";

      var assembly = TestAssembleC64Studio( source, out GR.Collections.MultiMap<int, RetroDevStudio.Parser.ParserBase.ParseMessage> messages );

      Assert.AreEqual( 4, messages.Count );
      Assert.AreEqual( "library.test = 2/$2", messages.Values[0].Message );
      Assert.AreEqual( "(library.)test = 2/$2", messages.Values[1].Message );
      Assert.AreEqual( "defined", messages.Values[2].Message );
      Assert.AreEqual( "defined", messages.Values[3].Message );

      Assert.AreEqual( "00C0", assembly.ToString() );
    }



    [TestMethod]
    public void TestNestedLoops()
    {
      string      source = @"ColorRAM      = $d800
                              Map.ColorMap  = $c000
                              Map.MapWidth  = 40

                              * = $2000
                              !for r = 0 to 1 ;11
                              !for c = 0 to 1 ;38
                              lda Map.ColorMap + c + r* Map.MapWidth, x
                              sta ColorRAM + 40 + c + r* 40
                              !end
                              !end";

      var assembly = TestAssembleC64Studio( source, out RetroDevStudio.Types.ASM.FileInfo info );
      Assert.AreEqual( "0020BD00C08D28D8BD01C08D29D8BD28C08D50D8BD29C08D51D8", assembly.ToString() );
    }



    [TestMethod]
    public void TestLoopWithModifiedLabel()
    {
      string      source = @"a = 15
                              * = $2000
                              !for r = 0 to 9
                              lda #a
                              a = a - 1
                              !end";

      var assembly = TestAssembleC64Studio( source, out RetroDevStudio.Types.ASM.FileInfo info );
      Assert.AreEqual( "0020A90FA90EA90DA90CA90BA90AA909A908A907A906", assembly.ToString() );
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

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );
      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( "0040A914200040E800000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000A914200041E800000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000A914200042E800000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000A914200043E800000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000A914200044E800000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000A914200045E8", assembly.Assembly.ToString() );
    }



    [TestMethod]
    public void TestLoByteWithExpressionSettings1C64Studio()
    {
      string      source = @"  * = $c000
                             P_SCREEN = $b400
                          lda #<P_SCREEN + ( 4 * 10 )";

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "00C0A928", assembly.ToString() );
    }



    [TestMethod]
    public void TestHiByteWithExpressionSettings1C64Studio()
    {
      string      source = @"  * = $c000
                             P_SCREEN = $b400
                          lda #>P_SCREEN + ( 4 * 10 )";

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "00C0A9DC", assembly.ToString() );
    }



    [TestMethod]
    public void TestNoDiscardVirtualSection()
    {
      string      source = @"* = $1f00
                                        !byte $17
                              * = $2000  ";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.crt";
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );
      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( "001F17", assembly.Assembly.ToString() );
    }



    [TestMethod]
    public void TestNoVirtualSectionAt840()
    {
      string source = @"!to ""lady-bug.prg"",cbm
        *=$0801   : !BASIC          ; Basiczeile erzeugen

        BLOCK     = 33            ; Spriteblock -> Block*64 = Spritedaten
        MATRIX    = $0400         ; Bildschirmspeicher
        POINTER_0   = MATRIX+$03F8        ; Spritepointer

              SEI           ; IRQ sperren

              LDA #$B0          ; X-Koordinate
              STA $D000         ; Sprite 0

              LDA #$81          ; Y-Koordinate
              STA $D001         ; Sprite 0

              LDA #BLOCK          ; Spriteblock
              STA POINTER_0       ; Spritepointer 0

              LDA #%00000001        ; Sprite Nr. 0
              STA $D015         ; Sprite-DMA Register

        -     JMP -           ; Warten

              !FILL $19,$EA       ; Code bis $083E bis 083F = Kein Sprite

        *=BLOCK*64                ; Adresse der Spritedaten -> Ballon

              !BYTE %00000011,%11111000,%00000000
              !BYTE %00001111,%11111110,%00000000
              !BYTE %00011111,%11111111,%00000000
              !BYTE %00011111,%00111111,%00000000
              !BYTE %00111110,%11001111,%10000000
              !BYTE %00111110,%11111111,%10000000
              !BYTE %00111110,%11001111,%10000000
              !BYTE %00011111,%00111111,%00000000
              !BYTE %00011111,%11111111,%00000000
              !BYTE %00011111,%11111111,%00000000
              !BYTE %00010111,%11111101,%00000000
              !BYTE %00001011,%11111010,%00000000
              !BYTE %00001001,%11110010,%00000000
              !BYTE %00000100,%11100100,%00000000
              !BYTE %00000100,%11100100,%00000000
              !BYTE %00000010,%01001000,%00000000
              !BYTE %00000010,%01001000,%00000000
              !BYTE %00000001,%11110000,%00000000
              !BYTE %00000001,%11110000,%00000000
              !BYTE %00000001,%11110000,%00000000
              !BYTE %00000000,%11100000,%00000000
              !BYTE $00";

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "01080B080A009E3230363100000078A9B08D00D0A9818D01D0A9218DF807A9018D15D04C2208EAEAEAEAEAEAEAEAEAEAEAEAEAEAEAEAEAEAEAEAEAEAEAEAEA000003F8000FFE001FFF001F3F003ECF803EFF803ECF801F3F001FFF001FFF0017FD000BFA0009F20004E40004E40002480002480001F00001F00001F00000E00000", assembly.ToString() );
    }



    [TestMethod]
    public void TestAssemblyStringLiterals()
    {
      // assembles as raw mapping of the first character

      string      source = @"* = $1000
                            lda #""i""
                            !byte ""i""";

      var assembly = TestAssembleC64Studio( source, out RetroDevStudio.Types.ASM.FileInfo info );

      Assert.AreEqual( "0010A96969", assembly.ToString() );
    }



    [TestMethod]
    public void TestAssemblyStringLiteralsTakeOnlyFirstChar()
    {
      // assembles as raw mapping of the first character

      string      source = @"* = $1000
                            lda #""hello""";

      var assembly = TestAssembleC64Studio( source, out RetroDevStudio.Types.ASM.FileInfo info );

      Assert.AreEqual( "0010A968", assembly.ToString() );
    }



    [TestMethod]
    public void TestAssemblyCharLiterals()
    {
      // assembles as raw mapping of the first character

      string      source = @"* = $1000
                            lda #'a'
                            !byte 'b'";

      var assembly = TestAssembleC64Studio( source, out RetroDevStudio.Types.ASM.FileInfo info );

      Assert.AreEqual( "0010A96162", assembly.ToString() );
    }



    [TestMethod]
    public void TestAssemblyBinaryLiterals()
    {
      string source = @"* = $2000
              !BYTE %00000000,%00000001,%00000010,%00000011
              !BYTE %00000100,%00000101,%00000110,%00000111 
              !BYTE %00001000,%00001001,%00001010,%00001011
              !BYTE %00001100,%00001101,%00001110,%00001111 
              !BYTE %00010000,%00010001,%00010010,%00010011
              !BYTE %00010100,%00010101,%00010110,%00010111 
              !BYTE %00011000,%00011001,%00011010,%00011011
              !BYTE %00011100,%00011101,%00011110,%00011111 
              !BYTE %00100000,%00100001,%00100010,%00100011
              !BYTE %00100100,%00100101,%00100110,%00100111 
              !BYTE %00101000,%00101001,%00101010,%00101011
              !BYTE %00101100,%00101101,%00101110,%00101111 
              !BYTE %00110000,%00110001,%00110010,%00110011
              !BYTE %00110100,%00110101,%00110110,%00110111 
              !BYTE %00111000,%00111001,%00111010,%00111011
              !BYTE %00111100,%00111101,%00111110,%00111111 
              !BYTE %01000000,%01000001,%01000010,%01000011
              !BYTE %01000100,%01000101,%01000110,%01000111 
              !BYTE %01001000,%01001001,%01001010,%01001011
              !BYTE %01001100,%01001101,%01001110,%01001111 
              !BYTE %01010000,%01010001,%01010010,%01010011
              !BYTE %01010100,%01010101,%01010110,%01010111 
              !BYTE %01011000,%01011001,%01011010,%01011011
              !BYTE %01011100,%01011101,%01011110,%01011111 
              !BYTE %01100000,%01100001,%01100010,%01100011
              !BYTE %01100100,%01100101,%01100110,%01100111 
              !BYTE %01101000,%01101001,%01101010,%01101011
              !BYTE %01101100,%01101101,%01101110,%01101111 
              !BYTE %01110000,%01110001,%01110010,%01110011
              !BYTE %01110100,%01110101,%01110110,%01110111 
              !BYTE %01111000,%01111001,%01111010,%01111011
              !BYTE %01111100,%01111101,%01111110,%01111111 
              !BYTE %10000000,%10000001,%10000010,%10000011
              !BYTE %10000100,%10000101,%10000110,%10000111 
              !BYTE %10001000,%10001001,%10001010,%10001011
              !BYTE %10001100,%10001101,%10001110,%10001111 
              !BYTE %10010000,%10010001,%10010010,%10010011
              !BYTE %10010100,%10010101,%10010110,%10010111 
              !BYTE %10011000,%10011001,%10011010,%10011011
              !BYTE %10011100,%10011101,%10011110,%10011111 
              !BYTE %10100000,%10100001,%10100010,%10100011
              !BYTE %10100100,%10100101,%10100110,%10100111 
              !BYTE %10101000,%10101001,%10101010,%10101011
              !BYTE %10101100,%10101101,%10101110,%10101111 
              !BYTE %10110000,%10110001,%10110010,%10110011
              !BYTE %10110100,%10110101,%10110110,%10110111 
              !BYTE %10111000,%10111001,%10111010,%10111011
              !BYTE %10111100,%10111101,%10111110,%10111111 
              !BYTE %11000000,%11000001,%11000010,%11000011
              !BYTE %11000100,%11000101,%11000110,%11000111 
              !BYTE %11001000,%11001001,%11001010,%11001011
              !BYTE %11001100,%11001101,%11001110,%11001111 
              !BYTE %11010000,%11010001,%11010010,%11010011
              !BYTE %11010100,%11010101,%11010110,%11010111 
              !BYTE %11011000,%11011001,%11011010,%11011011
              !BYTE %11011100,%11011101,%11011110,%11011111 
              !BYTE %11100000,%11100001,%11100010,%11100011
              !BYTE %11100100,%11100101,%11100110,%11100111 
              !BYTE %11101000,%11101001,%11101010,%11101011
              !BYTE %11101100,%11101101,%11101110,%11101111 
              !BYTE %11110000,%11110001,%11110010,%11110011
              !BYTE %11110100,%11110101,%11110110,%11110111 
              !BYTE %11111000,%11111001,%11111010,%11111011
              !BYTE %11111100,%11111101,%11111110,%11111111";

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "0020000102030405060708090A0B0C0D0E0F101112131415161718191A1B1C1D1E1F202122232425262728292A2B2C2D2E2F303132333435363738393A3B3C3D3E3F404142434445464748494A4B4C4D4E4F505152535455565758595A5B5C5D5E5F606162636465666768696A6B6C6D6E6F707172737475767778797A7B7C7D7E7F808182838485868788898A8B8C8D8E8F909192939495969798999A9B9C9D9E9FA0A1A2A3A4A5A6A7A8A9AAABACADAEAFB0B1B2B3B4B5B6B7B8B9BABBBCBDBEBFC0C1C2C3C4C5C6C7C8C9CACBCCCDCECFD0D1D2D3D4D5D6D7D8D9DADBDCDDDEDFE0E1E2E3E4E5E6E7E8E9EAEBECEDEEEFF0F1F2F3F4F5F6F7F8F9FAFBFCFDFEFF", assembly.ToString() );
    }



    [TestMethod]
    public void TestAssemblyBinaryLiterals2()
    {
      string source = @"* = $2000
              COLLISION_FILL        = %00010000; $01
              COLLISION_FILLABLE    = %00100000; $02
              COLLISION_NOTFILLABLE = %00110000; $03
              COLLISION_DEATH       = %11110000; $0f

              !byte COLLISION_FILL
              !byte COLLISION_FILLABLE
              !byte COLLISION_NOTFILLABLE
              !byte COLLISION_DEATH";

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "0020102030F0", assembly.ToString() );
    }



    [TestMethod]
    public void TestAssemblyInvalidBinaryLiterals()
    {
      string      source = @"* = $2000
                !byte %1234
                !byte %111111111";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PRG;
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;

      Assert.IsFalse( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );

      Assert.AreEqual( 2, parser.Errors );
      Assert.AreEqual( RetroDevStudio.Parser.ParserBase.ParseMessage.LineType.ERROR, asmFileInfo.Messages.Values[0].Type );
      Assert.AreEqual( RetroDevStudio.Types.ErrorCode.E1001_FAILED_TO_EVALUATE_EXPRESSION, asmFileInfo.Messages.Values[0].Code );
      Assert.IsTrue( asmFileInfo.Messages.Values[0].Message.Contains( "Cannot evaluate expression" ) );

      Assert.AreEqual( RetroDevStudio.Parser.ParserBase.ParseMessage.LineType.ERROR, asmFileInfo.Messages.Values[1].Type );
      Assert.AreEqual( RetroDevStudio.Types.ErrorCode.E1002_VALUE_OUT_OF_BOUNDS_BYTE, asmFileInfo.Messages.Values[1].Code );
      Assert.IsTrue( asmFileInfo.Messages.Values[1].Message.Contains( "Value out of bounds" ) );
    }




  }
}
