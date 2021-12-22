using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
  [TestClass]
  public partial class TestAssembler
  {
    [TestMethod]
    public void TestAssemblyOpcodeDetectionAbsolute()
    {
      string      source = @"* = $1000
                            lda $1234";

      var assembly = TestAssembleC64Studio( source, out C64Studio.Types.ASM.FileInfo info );

      Assert.AreEqual( "0010AD3412", assembly.ToString() );
    }



    [TestMethod]
    public void TestAssemblyOpcodeDetectionZeropage()
    {
      string      source = @"* = $1000
                            lda $12";

      var assembly = TestAssembleC64Studio( source, out C64Studio.Types.ASM.FileInfo info );

      Assert.AreEqual( "0010A512", assembly.ToString() );
    }



    [TestMethod]
    public void TestAssemblyOpcodeDetectionZeropageX()
    {
      string      source = @"* = $1000
                            lda $12,x";

      var assembly = TestAssembleC64Studio( source, out C64Studio.Types.ASM.FileInfo info );

      Assert.AreEqual( "0010B512", assembly.ToString() );
    }



    [TestMethod]
    public void TestAssemblyOpcodeDetectionNoZeropageY()
    {
      string      source = @"* = $1000
                            lda $12,y";

      var assembly = TestAssembleC64Studio( source, out C64Studio.Types.ASM.FileInfo info );

      Assert.AreEqual( "0010B91200", assembly.ToString() );
    }



    [TestMethod]
    public void TestAssemblyOpcodeDetectionAbsoluteX()
    {
      string      source = @"* = $1000
                            lda $1234,x";

      var assembly = TestAssembleC64Studio( source, out C64Studio.Types.ASM.FileInfo info );

      Assert.AreEqual( "0010BD3412", assembly.ToString() );
    }



    

    [TestMethod]
    public void TestAssemblyOpcodeDetectionNoIndirectAbsoluteX()
    {
      string      source = @"* = $1000
                            lda ($1234),x";

      var assembly = TestAssembleC64Studio( source, out C64Studio.Types.ASM.FileInfo info );

      Assert.AreEqual( "0010BD3412", assembly.ToString() );
    }



    [TestMethod]
    public void TestOpcodeDetectionZeropageIndirectX()
    {
      string      source = @"* = $1000
                            adc($12, x )";

      var assembly = TestAssembleC64Studio( source, out C64Studio.Types.ASM.FileInfo info );

      Assert.AreEqual( "00106112", assembly.ToString() );
    }



    [TestMethod]
    public void TestOpcodeDetectionNoZeropageIndirectX()
    {
      string      source = @"* = $1000
                            lda ($12),x";

      var assembly = TestAssembleC64Studio( source, out C64Studio.Types.ASM.FileInfo info );

      Assert.AreEqual( "0010B512", assembly.ToString() );
    }



    [TestMethod]
    public void TestAssemblyOpcodeDetectionZeropageRelativeY()
    {
      string      source = @"* = $1000
                            lda ($12),y";

      var assembly = TestAssembleC64Studio( source, out C64Studio.Types.ASM.FileInfo info );

      Assert.AreEqual( "0010B112", assembly.ToString() );
    }



    [TestMethod]
    public void TestAssemblyOpcodeDetectionNoZeropageRelativeY()
    {
      string      source = @"* = $1000
                            lda ($1234),y";

      C64Studio.Parser.ASMFileParser      parser = new C64Studio.Parser.ASMFileParser();
      parser.SetAssemblerType( C64Studio.Types.AssemblerType.C64_STUDIO );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;

      bool parseResult = parser.Parse( source, null, config, null );
      Assert.IsFalse( parseResult );
    }



    private GR.Memory.ByteBuffer TestAssembleC64Studio( string Source, out C64Studio.Types.ASM.FileInfo Info )
    {
      C64Studio.Parser.ASMFileParser      parser = new C64Studio.Parser.ASMFileParser();
      parser.SetAssemblerType( C64Studio.Types.AssemblerType.C64_STUDIO );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;

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

      Info = parser.ASMFileInfo;

      return parser.AssembledOutput.Assembly;
    }



    private GR.Memory.ByteBuffer TestAssembleC64Studio( string Source )
    {
      return TestAssembleC64Studio( Source, out C64Studio.Types.ASM.FileInfo Info );
    }



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

      Assert.IsTrue( parser.Parse( source, null, config, null ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( 1, parser.Warnings );
      Assert.AreEqual( C64Studio.Parser.ParserBase.ParseMessage.LineType.SEVERE_WARNING, parser.Messages.Values[0].Type  );
      Assert.AreEqual( C64Studio.Types.ErrorCode.W0001_SEGMENT_OVERLAP, parser.Messages.Values[0].Code );

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

      var assembly = TestAssembleC64Studio( source, out C64Studio.Types.ASM.FileInfo info );

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

      var assembly = TestAssembleC64Studio( source, out C64Studio.Types.ASM.FileInfo info );

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

      C64Studio.Parser.ASMFileParser      parser = new C64Studio.Parser.ASMFileParser();
      parser.SetAssemblerType( C64Studio.Types.AssemblerType.C64_STUDIO );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null ) );

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

      Assert.IsTrue( parser.Parse( source, null, config, null ) );

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( 1, parser.Warnings );
      Assert.AreEqual( C64Studio.Parser.ParserBase.ParseMessage.LineType.WARNING, parser.Messages.Values[0].Type );
      Assert.AreEqual( C64Studio.Types.ErrorCode.W0007_POTENTIAL_PROBLEM, parser.Messages.Values[0].Code );

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

      Assert.IsFalse( parser.Parse( source, null, config, null ) );

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

      Assert.IsFalse( parser.Parse( source, null, config, null ) );

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

      Assert.IsTrue( parser.Parse( source, null, config, null ) );
      Assert.IsTrue( parser.Assemble( config ) );

      Assert.AreEqual( 0x80002, (int)parser.AssembledOutput.Assembly.Length );
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

      var assembly = TestAssembleC64Studio( source, out C64Studio.Types.ASM.FileInfo Info );

      Assert.AreEqual( "0020A000B90104990004C8C927D0F5BD28208D2704A000B92904992804C8C927D0F5BD28218D4F04A000B95104995004C8C927D0F5BD28228D7704A000B97904997804C8C927D0F5BD28238D9F04A000B9A10499A004C8C927D0F5BD28248DC704A000B9C90499C804C8C927D0F5BD28258DEF04A000B9F10499F004C8C927D0F5BD28268D1705A000B91905991805C8C927D0F5BD28278D3F05A000B94105994005C8C927D0F5BD28288D6705A000B96905996805C8C927D0F5BD28298D8F05A000B99105999005C8C927D0F5BD282A8DB705A000B9B90599B805C8C927D0F5BD282B8DDF05A000B9E10599E005C8C927D0F5BD282C8D0706A000B90906990806C8C927D0F5BD282D8D2F06A000B93106993006C8C927D0F5BD282E8D5706A000B95906995806C8C927D0F5BD282F8D7F06A000B98106998006C8C927D0F5BD28308DA706A000B9A90699A806C8C927D0F5BD28318DCF06A000B9D10699D006C8C927D0F5BD28328DF706A000B9F90699F806C8C927D0F5BD28338D1F07A000B92107992007C8C927D0F5BD28348D4707A000B94907994807C8C927D0F5BD28358D6F07A000B97107997007C8C927D0F5BD28368D9707A000B99907999807C8C927D0F5BD28378DBF07A000B9C10799C007C8C927D0F5BD28388DE707A000B9E90799E807C8C927D0F5BD28398D0F0860", assembly.ToString() );
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

      Assert.IsTrue( parser.Parse( source, null, config, null ) );
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

      C64Studio.Parser.ASMFileParser      parser = new C64Studio.Parser.ASMFileParser();
      parser.SetAssemblerType( C64Studio.Types.AssemblerType.C64_STUDIO );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.crt";
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null ) );
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

      C64Studio.Parser.ASMFileParser      parser = new C64Studio.Parser.ASMFileParser();
      parser.SetAssemblerType( C64Studio.Types.AssemblerType.C64_STUDIO );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( source, null, config, null ) );
      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( "01080B080A009E3230363100000078A9B08D00D0A9818D01D0A9218DF807A9018D15D04C2208EAEAEAEAEAEAEAEAEAEAEAEAEAEAEAEAEAEAEAEAEAEAEAEAEA000003F8000FFE001FFF001F3F003ECF803EFF803ECF801F3F001FFF001FFF0017FD000BFA0009F20004E40004E40002480002480001F00001F00001F00000E00000", assembly.Assembly.ToString() );
    }



    [TestMethod]
    public void TestAssemblyStringLiterals()
    {
      // assembles as raw mapping of the first character

      string      source = @"* = $1000
                            lda #""i""
                            !byte ""i""";

      var assembly = TestAssembleC64Studio( source, out C64Studio.Types.ASM.FileInfo info );

      Assert.AreEqual( "0010A96969", assembly.ToString() );
    }



  }
}
