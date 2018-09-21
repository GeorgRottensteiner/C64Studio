using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
  [TestClass]
  public class UnitTestParser
  {
    private GR.Memory.ByteBuffer TestAssemble( string Source )
    {
      C64Studio.Parser.ASMFileParser      parser = new C64Studio.Parser.ASMFileParser();
      parser.SetAssemblerType( C64Studio.Types.AssemblerType.C64_STUDIO );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;

      Assert.IsTrue( parser.Parse( Source, null, config ) );
      Assert.IsTrue( parser.Assemble( config ) );

      return parser.AssembledOutput.Assembly;
    }



    [TestMethod]
    public void TestOpcodeImmediateByteBounds()
    {
      string      source = @"* = $2000
                             lda #$ffff";

      C64Studio.Parser.ASMFileParser      parser = new C64Studio.Parser.ASMFileParser();
      parser.SetAssemblerType( C64Studio.Types.AssemblerType.C64_STUDIO );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;

      C64Studio.Types.ErrorCode  code = C64Studio.Types.ErrorCode.OK;

      Assert.IsFalse( parser.Parse( source, null, config ) );

      foreach ( var entry in parser.Messages.Values )
      {
        code = entry.Code;
        break;
      }

      Assert.AreEqual( 1, parser.Errors );
      Assert.AreEqual( C64Studio.Types.ErrorCode.E1002_VALUE_OUT_OF_BOUNDS_BYTE, code );
    }



    [TestMethod]
    public void TestOpcodeFallbackToAbsolute()
    {
      string      source = @"* = $2000
                             Zeropage_Routine = $0020
                            jmp Zeropage_Routine";

      C64Studio.Parser.ASMFileParser      parser = new C64Studio.Parser.ASMFileParser();
      parser.SetAssemblerType( C64Studio.Types.AssemblerType.C64_STUDIO );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;

      var assembly = TestAssemble( source );

      Assert.AreEqual( "00204C2000", assembly.ToString() );
    }



    [TestMethod]
    public void TestStartAddress1()
    {
      string      source = @"* = $2000";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "0020", assembly.ToString() );
    }



    [TestMethod]
    public void TestStartAddress2()
    {
      string      source = @"* = $ff17";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "17FF", assembly.ToString() );
    }



    [TestMethod]
    public void TestSimpleCompileWithoutProject()
    {
      string      source = "* = $2000\n"
                           + "loop   inc $d020\n"
                           + "jmp loop";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "0020EE20D04C0020", assembly.ToString() );
    }



    [TestMethod]
    public void TestMacro()
    {
      string      source = @"* = $0801

                              stab_irq_line = $30

                              !basic

                              ekran1 = $0400


                              !macro IRQ_SETUP irq_proc, irq_line
                                        lda #<(irq_line)
                                        sta $d012
                                        lda $d011
                                        and #$7f
                                        ora #(>(irq_line) << 7 )
                                        sta $d011
                                        lda #<irq_proc
                                        sta $fffe
                                        lda #>irq_proc
                                        sta $ffff
                              !end          
        



                              !macro FILL_MEM add, pages, val
                                          lda #<add
                                          sta $02
                                          lda #>add
                                          sta $03
                                          ldy #0
                                          lda #val
                                          ldx #pages
                              -           sta ($02),y
                                          iny
                                          bne -
                                          inc $03
                                          dex
                                          bne -
                              !end

                              !macro SYNC
                                          lda $d011
                                          bpl *-3
                                          lda $d011
                                          bmi *-3
                              !end

                                        +IRQ_SETUP stab_irq, stab_irq_line
          
                              stab_irq          

                                          sei
                                          +SYNC
                                          lda #$00
                                          sta $d011
            
                                          +FILL_MEM ekran1, 4, $1b
            
            
            
                              .Loop
                                          jmp .Loop";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "01080B080A009E32303631000000A9308D12D0AD11D0297F09008D11D0A9268DFEFFA9088DFFFF78AD11D010FBAD11D030FBA9008D11D0A9008502A9048503A000A91BA2049102C8D0FBE603CAD0F64C4E08", assembly.ToString() );
    }



    [TestMethod]
    public void TestMacroWithNestedLoopsWhichUsesMacroArgs()
    {
      string      source = @"* = $2000
                           CHARACTER_ROM   = 1024
                           CUSTOM_CHAR_SET = $4000
                           !macro copy_character_range_from_rom start, end 
                           ;removed some code for simplicity 
                           ; Copy char rom to ram 
                           !for char = start to end 
                           ldx #0 
                           - 
                           lda CHARACTER_ROM + char*8, x 
                           sta CUSTOM_CHAR_SET + char*8, x 
                           inx 
                           cpx #8 
                           bne - 
                           !end 
                           !end
                           +copy_character_range_from_rom 0, 2
                           rts";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "0020A200BD00049D0040E8E008D0F5A200BD08049D0840E8E008D0F5A200BD10049D1040E8E008D0F560", assembly.ToString() );
    }



    [TestMethod]
    public void TestTextModeText()
    {
      string      source = @"!to ""text-modes.prg"",cbm
                              * = $2000
                                      !text ""ABCabc123""";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "0020414243616263313233", assembly.ToString() );
    }



    [TestMethod]
    public void TestTextModePET()
    {
      string      source = @"!to ""text-modes.prg"",cbm
                              * = $3000
                                      !pet ""ABCabc123"" ";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "0030C1C2C3414243313233", assembly.ToString() );
    }



    [TestMethod]
    public void TestTextModeRaw()
    {
      string      source = @"!to ""text-modes.prg"",cbm
                              * = $5000
                                      !raw ""ABCabc123""";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "0050414243616263313233", assembly.ToString() );
    }



    [TestMethod]
    public void TestTextModeScreen()
    {
      string      source = @"!to ""text-modes.prg"",cbm
                              * = $4000
                                      !scr ""ABCabc123""";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "0040414243010203313233", assembly.ToString() );
    }



    [TestMethod]
    public void TestModifiedLabels()
    {
      string      source = @"COLOR = 7
                              * = $2000

                                        lda #COLOR
                                        sta 53280
                                        rts

                              COLOR = COLOR + 1

                              * = $2020
                                        lda #COLOR
                                        sta 53280
                                        rts


                              COLOR = COLOR + 1

                              * = $2040
                                        lda #COLOR
                                        sta 53280
                                        rts";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "0020A9078D20D0600000000000000000000000000000000000000000000000000000A9088D20D0600000000000000000000000000000000000000000000000000000A9098D20D060", assembly.ToString() );
    }



    [TestMethod]
    public void TestEmptyMacro()
    {
      string      source = @"* =$2000
                            !MACRO test v1, v2 {
                              }

                              main
                                +test 64, 255";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "0020", assembly.ToString() );
    }



    [TestMethod]
    public void TestMacroWithMultiPartParameters()
    {
      string      source = @"    * = $c000
                              !to ""test.prg"", cbm
                              ; ===============================
                              ; Makrodefinitionen
                              ; ===============================

                              ; lädt einen 16-Bit-Wert in .A und .X
                              ; Lo-Byte landet in .A
                              ; Hi-Byte landet in .X
                              !macro ldax16 value
                                lda #<value
                                ldx #>value
                              !end

                              ; schreibt einen 16-Bit-Wert in .A und .X an die angegebene Adresse
                              ; Lo-Byte in .A wird in address geschrieben
                              ; Hi-Byte in .X wird in address + 1 geschrieben
                              !macro stax16 address
                                sta address
                                stx address + 1
                              !end

                              ; ""POKE 16-Bit""
                              ; setzt in ptrAddress (und ptrAddress + 1) den 16-Bit-Wert in address
                              ; ändert nur .A
                              !macro setPtrA address, ptrAddress
                                lda #<address
                                sta ptrAddress
                                lda #>address
                                sta ptrAddress + 1
                              !end

                              offset_2 =  2
                              offset_8 =  8
                              offset40 = 40
                              zptrMemDest1 = $a7
                              testAdressFix = $8000
                              testAdressPreCalc = .testLongNamedAdr + 2 + offset_2

                              ; ===============================
                              ; Makros mit einem Parameter
                              ; ===============================

                              ; -------------------------------
                              ; ohne Arithmetik ($c000)
                              ; -------------------------------

                              ; feste Adresse, Zeropage
                              +ldax16 zptrMemDest1                            ; OK
                              +stax16 zptrMemDest1                            ; OK

                              ; feste Adresse (1), 16 bit
                              +ldax16 testAdressFix                           ; OK
                              +stax16 testAdressFix                           ; OK

                              ; feste Adresse (2), 16 bit
                              +ldax16 testAdressPreCalc                       ; OK
                              +stax16 testAdressPreCalc                       ; OK

                              ; dynamische Adresse, 16 bit
                              +ldax16 .testLongNamedAdr                       ; OK
                              +stax16 .testLongNamedAdr                       ; OK

                              ; -------------------------------
                              ; mit Arithmetik, ohne Klammern ($c026)
                              ; -------------------------------

                              ; feste Adresse, Zeropage
                              +ldax16 zptrMemDest1 + 2                        ; OK
                              +stax16 zptrMemDest1 + offset_2                 ; OK

                              ; feste Adresse (1), 16 bit
                              +ldax16 testAdressFix + 2                       ; OK
                              +stax16 testAdressFix + offset_2                ; OK

                              ; feste Adresse (2), 16 bit
                              +ldax16 testAdressPreCalc + 2                   ; OK
                              +stax16 testAdressPreCalc + offset_2            ; OK

                              ; dynamische Adresse, 16 bit
                              +ldax16 .testLongNamedAdr + 8                   ; OK
                              +stax16 .testLongNamedAdr + offset_8            ; OK

                              ;--------------------------------
                              ; mit Arithmetik, mit Klammern ($c04c)
                              ; -------------------------------

                              ; feste Adresse, Zeropage
                              +ldax16 (zptrMemDest1 + offset_2)               ; OK
                              +stax16 (zptrMemDest1 + 2)                      ; fast OK: 16 bit statt Zeropage

                              ; feste Adresse (1), 16 bit
                              +ldax16 (testAdressFix + offset_2)              ; OK
                              +stax16 (testAdressFix + 2)                     ; OK

                              ; feste Adresse (2), 16 bit
                              +ldax16 (testAdressPreCalc + offset_2)          ; OK
                              +stax16 (testAdressPreCalc + 2)                 ; OK

                              ; dynamische Adresse, 16 bit
                              +ldax16 (.testLongNamedAdr + offset_8)          ; OK
                              +stax16 (.testLongNamedAdr + 8)                 ; OK


                              ; ===============================
                              ; Makros mit zwei Parametern
                              ; ===============================

                              ; -------------------------------
                              ; ohne Arithmetik ($c074)
                              ; -------------------------------
                              +setPtrA testAdressFix, zptrMemDest1                                ; OK
                              +setPtrA .testLongNamedAdr, zptrMemDest1                            ; OK

                              ; -------------------------------
                              ; mit Arithmetik, ohne Klammern ($c084)
                              ; -------------------------------
                              +setPtrA testAdressFix + 40, zptrMemDest1                           ; OK
                              +setPtrA testAdressFix + 40, zptrMemDest1 + 2                       ; OK
                              +setPtrA testAdressFix + 40, zptrMemDest1 + offset_2                ; OK
                              nop
                              nop
                              ; funktioniert nicht: !align 3, $ea
                              ;!align 3, $ea

                              ; $c09e
                              +setPtrA testAdressFix + offset40, zptrMemDest1                     ; OK
                              +setPtrA testAdressFix + offset40, zptrMemDest1 + 2                 ; OK
                              +setPtrA testAdressFix + offset40, zptrMemDest1 + offset_2          ; OK
                              nop
                              nop

                              ; $c0b8
                              +setPtrA .testLongNamedAdr + 8, zptrMemDest1                        ; OK
                              +setPtrA .testLongNamedAdr + 8, zptrMemDest1 + 2                    ; OK
                              +setPtrA .testLongNamedAdr + 8, zptrMemDest1 + offset_2             ; OK
                              nop
                              nop

                              ; $c0d2
                              +setPtrA .testLongNamedAdr + offset_8, zptrMemDest1                 ; OK
                              +setPtrA .testLongNamedAdr + offset_8, zptrMemDest1 + 2             ; OK
                              +setPtrA .testLongNamedAdr + offset_8, zptrMemDest1 + offset_2      ; OK
                              nop
                              nop


                              ; -------------------------------
                              ; mit Arithmetik, mit Klammern
                              ; -------------------------------

                              ; $c0ec
                              +setPtrA (testAdressFix + 40), (zptrMemDest1)                       ;fast OK: 16 bit statt Zeropage
                              +setPtrA (testAdressFix + 40), (zptrMemDest1 + 2)                   ;fast OK: 16 bit statt Zeropage
                              +setPtrA (testAdressFix + 40), (zptrMemDest1 + offset_2)            ;fast OK: 16 bit statt Zeropage
                              nop
                              nop

                              ; $c10c
                              +setPtrA (testAdressFix + offset40), (zptrMemDest1)                 ;fast OK: 16 bit statt Zeropage
                              +setPtrA (testAdressFix + offset40), (zptrMemDest1 + 2)             ;fast OK: 16 bit statt Zeropage
                              +setPtrA (testAdressFix + offset40), (zptrMemDest1 + offset_2)      ;fast OK: 16 bit statt Zeropage
                              nop
                              nop

                              ; $c12c
                              +setPtrA (.testLongNamedAdr + 8), (zptrMemDest1)                    ;fast OK: 16 bit statt Zeropage
                              +setPtrA (.testLongNamedAdr + 8), (zptrMemDest1 + 2)                ;fast OK: 16 bit statt Zeropage
                              +setPtrA (.testLongNamedAdr + 8), (zptrMemDest1 + offset_2)         ;fast OK: 16 bit statt Zeropage
                              nop
                              nop

                              ; $c14c
                              +setPtrA (.testLongNamedAdr + offset_8), (zptrMemDest1)             ;fast OK: 16 bit statt Zeropage
                              +setPtrA (.testLongNamedAdr + offset_8), (zptrMemDest1 + 2)         ;fast OK: 16 bit statt Zeropage
                              +setPtrA (.testLongNamedAdr + offset_8), (zptrMemDest1 + offset_2)  ;fast OK: 16 bit statt Zeropage

                              rts



                              !align 255,0

                              .testLongNamedAdr:
                              !word $a0a0
                            ";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "00C0A9A7A20085A786A8A900A2808D00808E0180A904A2C28D04C28E05C2A900A2C28D00C28E01C2A9A9A20085A986AAA902A2808D02808E0380A906A2C28D06C28E07C2A908A2C28D08C28E09C2A9A9A20085A986AAA902A2808D02808E0380A906A2C28D06C28E07C2A908A2C28D08C28E09C2A90085A7A98085A8A90085A7A9C285A8A92885A7A98085A8A92885A9A98085AAA92885A9A98085AAEAEAA92885A7A98085A8A92885A9A98085AAA92885A9A98085AAEAEAA90885A7A9C285A8A90885A9A9C285AAA90885A9A9C285AAEAEAA90885A7A9C285A8A90885A9A9C285AAA90885A9A9C285AAEAEAA92885A7A98085A8A92885A9A98085AAA92885A9A98085AAEAEAA92885A7A98085A8A92885A9A98085AAA92885A9A98085AAEAEAA90885A7A9C285A8A90885A9A9C285AAA90885A9A9C285AAEAEAA90885A7A9C285A8A90885A9A9C285AAA90885A9A9C285AA6000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000A0A0", assembly.ToString() );
    }



    [TestMethod]
    public void TestOpcodeSizeDetectionZeroPage()
    {
      string      source = @"    * = $c000
                              !to ""test.prg"", cbm

                              VALUE_8_BIT = $17

                              lda VALUE_8_BIT         ;supposed to read from ZP
                              rts
                            ";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "00C0A51760", assembly.ToString() );
    }



    [TestMethod]
    public void TestOpcodeSizeDetectionForcedSizes()
    {
      string source = @"!to ""forcedsize.prg"",cbm
                    * = $2000

                    POS_1
                              lda #17
          
                    POS_2
                              ;should end as sta $xx
                              sta $02
          
                    POS_3
                              ;should end as sta $xxxx
                              sta $0002          
          
                    POS_4
                              ;should end as sta $xxxx
                              sta $1000
          
                    POS_5
                              ;should end as sta $xx
                              sta+1 $0002                    
          
                    POS_6
                              ;should end as sta $xxxx
                              sta+2 $02                              
          
                    POS_7
                              rts
          
                    !if ( ( POS_2 - POS_1 ) != 2 ) {
                    !error Opcode mismatch
                    }

                    !if ( ( POS_3 - POS_2 ) != 2 ) {
                    !error Opcode mismatch
                    }

                    !if ( ( POS_4 - POS_3 ) != 3 ) {
                    !error Opcode mismatch
                    }

                    !if ( ( POS_5 - POS_4 ) != 3 ) {
                    !error Opcode mismatch
                    }

                    !if ( ( POS_6 - POS_5 ) != 2 ) {
                    !error Opcode mismatch
                    }

                    !if ( ( POS_7 - POS_6 ) != 3 ) {
                    !error Opcode mismatch
                    }
                      ";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "0020A91185028D02008D001085028D020060", assembly.ToString() );
    }



    [TestMethod]
    public void TestOpcodeRelativeInsideZeroPage()
    {
      string      source = @"    * = $02
                              lsmf

                              lda #1
                              bne lsmf";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "0200A901D0FC", assembly.ToString() );
    }



    [TestMethod]
    public void TestBracesNotOnLineStart()
    {
      string      source = "    * = $c000\r\n!if 1 {\r\n!byte $17\r\n}\r\n\r\n    !if 2{\r\n    !byte $18\r\n    }";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "00C01718", assembly.ToString() );
    }



    [TestMethod]
    public void TestIfWithHiLoByteNotEqualAndLabelInFront()
    {
      string      source = @"*=$c000
                          s_Ausgleich nop
                          .wa3    !if (>(s_Ausgleich) <> >(.wa3)) {
                          .wa4    nop
                          }";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "00C0EA", assembly.ToString() );
    }



    [TestMethod]
    public void TestIfWithHiLoByteNotEqualAndLabelInFront2()
    {
      string      source = @"*=$c000
                          s_Ausgleich nop
                          .wa3    !if (>(s_Ausgleich) != >(.wa3)) {
                          .wa4    nop
                          }";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "00C0EA", assembly.ToString() );
    }



    [TestMethod]
    public void TestIfWithHiLoByteNotEqualAndLabelInFrontAndEqualsOperator()
    {
      string      source = @"*=$c000
                          s_Ausgleich nop
                          .wa3    !if (>s_Ausgleich = >.wa3) {
                          .wa4    nop
                          }";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "00C0EAEA", assembly.ToString() );
    }



    [TestMethod]
    public void TestBinaryLiterals()
    {
      string      source = @"    * = $c000

                              !byte %...##...
                              !byte %01000010";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "00C01842", assembly.ToString() );
    }



    [TestMethod]
    public void TestBinaryLiteralsOutOfBounds()
    {
      string      source = @"    * = $c000

                              !byte %#...##...
                              !byte %101000010";

      C64Studio.Parser.ASMFileParser      parser = new C64Studio.Parser.ASMFileParser();
      parser.SetAssemblerType( C64Studio.Types.AssemblerType.C64_STUDIO );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;

      C64Studio.Types.ErrorCode  code = C64Studio.Types.ErrorCode.OK;

      Assert.IsFalse( parser.Parse( source, null, config ) );

      foreach ( var entry in parser.Messages.Values )
      {
        code = entry.Code;
        break;
      }

      Assert.AreEqual( 2, parser.Errors );
      Assert.AreEqual( C64Studio.Types.ErrorCode.E1002_VALUE_OUT_OF_BOUNDS_BYTE, code );
    }



    [TestMethod]
    public void TestByteChain()
    {
      string      source = @"    * = $c000

                              !byte 1,2,3,4,5";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "00C00102030405", assembly.ToString() );
    }



    [TestMethod]
    public void TestWordChain()
    {
      string      source = @"    * = $c000

                              !word 1,2,3,4,5";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "00C001000200030004000500", assembly.ToString() );
    }



    [TestMethod]
    public void TestNoValueAfterSeparatorPOByte()
    {
      string      source = @"* = $2000
                             !byte 0,0,0,";

      C64Studio.Parser.ASMFileParser      parser = new C64Studio.Parser.ASMFileParser();
      parser.SetAssemblerType( C64Studio.Types.AssemblerType.C64_STUDIO );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;

      C64Studio.Types.ErrorCode  code = C64Studio.Types.ErrorCode.OK;

      Assert.IsFalse( parser.Parse( source, null, config ) );

      foreach ( var entry in parser.Messages.Values )
      {
        code = entry.Code;
        break;
      }

      Assert.AreEqual( 1, parser.Errors );
      Assert.AreEqual( C64Studio.Types.ErrorCode.E1000_SYNTAX_ERROR, code );
    }



    [TestMethod]
    public void TestNoValueAfterSeparatorPOWord()
    {
      string      source = @"* = $2000
                             !word 0,0,0,";

      C64Studio.Parser.ASMFileParser      parser = new C64Studio.Parser.ASMFileParser();
      parser.SetAssemblerType( C64Studio.Types.AssemblerType.C64_STUDIO );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;

      C64Studio.Types.ErrorCode  code = C64Studio.Types.ErrorCode.OK;

      Assert.IsFalse( parser.Parse( source, null, config ) );

      foreach ( var entry in parser.Messages.Values )
      {
        code = entry.Code;
        break;
      }

      Assert.AreEqual( 1, parser.Errors );
      Assert.AreEqual( C64Studio.Types.ErrorCode.E1000_SYNTAX_ERROR, code );
    }



    [TestMethod]
    public void TestMacroWithExpressionAsParameterAndOpcodeDetection()
    {
      string      source = @"text_ram = $0400

                              * = $2000

                              !macro print s,z,adr 
                              ldx #0 
                              lda adr,x 
                              sta text_ram+(z*40)+s,x 
                              !end 


                              +print 2,4,$3000+5

                              rts


                              * = $2020
                              !text ""lsmf""";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "0020A200BD05309DA2046000000000000000000000000000000000000000000000006C736D66", assembly.ToString() );
    }



    [TestMethod]
    public void TestMacroWithParameterAndOpcodeDetection()
    {
      string      source = @"text_ram = $0400

                              * = $2000

                              !macro print s,z,adr 
                              ldx #0 
                              lda adr,x 
                              sta text_ram+(z*40)+s,x 
                              !end 


                              +print 2,4,$3000

                              rts


                              * = $2020
                              !text ""lsmf""";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "0020A200BD00309DA2046000000000000000000000000000000000000000000000006C736D66", assembly.ToString() );
    }



    [TestMethod]
    public void TestMacroWithIfInside()
    {
      string      source = @"* = $2000

                          !macro jne label
                          !if ( ( label - *) > 127 ) or ( ( * - label ) < ( -127 ) ) { 
                           beq *+5 
                           jmp label 
                           } else { 
                           bne label 
                           } 
                           !end 
 
                          .BG = $200b 
 
                                    lda #1
                                    +jne .BG
                                    sta $d020
                                    rts
          
                          ;.BG
                                    sta $d021
                                    rts";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "0020A901D0078D20D0608D21D060", assembly.ToString() );
    }




    [TestMethod]
    public void TestLabelKnownAfterBytePseudoOp()
    {
      string      source = @"* = $2000

                              !byte 1,2,LABEL_LATE,4
                              LABEL_LATE = 3";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "002001020304", assembly.ToString() );
    }



    [TestMethod]
    public void TestPseudoPCPseudoOp()
    {
      string      source = @"* = $2000

                             jmp BOOT

                             !pseudopc $0400
                     LOADER 
                             lda #$01
                             rts
    
                             !realpc
                      BOOT             
                             jsr LOADER
                             rts";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "00204C0620A9016020000460", assembly.ToString() );
    }



    [TestMethod]
    public void TestPseudoPCPseudoOpWithBraces()
    {
      string      source = @"* = $2000

                             jmp BOOT

                             !pseudopc $0400 {
                     LOADER 
                             lda #$01
                             rts
    
                             }
                      BOOT             
                             jsr LOADER
                             rts";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "00204C0620A9016020000460", assembly.ToString() );
      //00204C0304A9016020000460
    }



    [TestMethod]
    public void TestIfDefPseudoOp()
    {
      string      source = @"* = $2000

                             FLAG

                             !ifdef FLAG {
                             !byte 0
                             } else {
                             !byte 255
                             }

                             rts";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "00200060", assembly.ToString() );
    }



    [TestMethod]
    public void TestIfDefElsePseudoOp()
    {
      string      source = @"* = $2000

                             ;FLAG

                             !ifdef FLAG {
                             !byte 0
                             } else {
                             !byte 255
                             }

                             rts";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "0020FF60", assembly.ToString() );
    }



    [TestMethod]
    public void TestNestedIfWithIndention()
    {
      string    source = "FACTOR_A = 5\r\n"
                        + "FACTOR_B = 6\r\n"
                        + "\r\n"
                        + "* = $2000\r\n"
                        + "\r\n"
                        + "         !byte 17\r\n"
                        + "\r\n"
                        + "\r\n"
                        + "\r\n"
                        + "   !if FACTOR_A = FACTOR_B {\r\n"
                        + "      !if FACTOR_B = 6 {\r\n"
                        + "          !byte 1\r\n"
                        + "        }\r\n"
                        + "    }\r\n"
+ "\r\n"
  + "\r\n"
  + "\r\n"
                         + "!if FACTOR_A != FACTOR_B {\r\n"
                         + "!if FACTOR_B = 6 {\r\n"
                         + "  !byte 2\r\n"
                         + "}\r\n"
                         + "}\r\n"
     + "\r\n"
                         +"      !byte 18";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "0020110212", assembly.ToString() );
    }



    [TestMethod]
    public void TestOperatorLessOrEqual()
    {
      string    source = @"RED=2
GREEN=5


*=$0801

!BASIC


!IF 5<=2 {

  lda #GREEN

} else {

  lda #RED

}

  sta $D021

  rts";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "01080B080A009E32303631000000A9028D21D060", assembly.ToString() );
    }



    [TestMethod]
    public void TestPseudoOpIfDefWithExpression()
    {
      string    source = @"* = $2000

          GREEN = 5
          RED = 2

          TEST2

          !IFDEF TEST=2 {

          lda #GREEN

          } else {

          lda #RED

          }

          sta 53280
          rts";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "0020A9028D20D060", assembly.ToString() );
    }


    [TestMethod]
    public void TestErrorOnIndirectAddressingForNOnSupportingOpCodes()
    {
      string    source = @"* = $2000

                          SCREEN = $0400


                                    lda (SCREEN)
                                    sta (SCREEN)
          
                                    jsr (TEST)
          
                                    lda #1
                                    bne (THEEND)
          
                          THEEND          
                          TEST          
                                    rts";

      C64Studio.Parser.ASMFileParser      parser = new C64Studio.Parser.ASMFileParser();
      parser.SetAssemblerType( C64Studio.Types.AssemblerType.C64_STUDIO );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.prg";
      config.TargetType = C64Studio.Types.CompileTargetType.PRG;
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;

      C64Studio.Types.ErrorCode  code = C64Studio.Types.ErrorCode.OK;

      Assert.IsFalse( parser.Parse( source, null, config ) );

      foreach ( var entry in parser.Messages.Values )
      {
        code = entry.Code;
        break;
      }

      Assert.AreEqual( 4, parser.Errors );
      Assert.AreEqual( C64Studio.Types.ErrorCode.E1105_INVALID_OPCODE, code );
    }



    [TestMethod]
    public void TestCheapLocals()
    {
      string      source = @"* = $2000
                             Main
                              @cheap
                             lda #$ff
                             jmp @cheap

                             NotMain
                              @cheap
                               inc $d020
                                jmp @cheap";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "0020A9FF4C0020EE20D04C0520", assembly.ToString() );
    }



    [TestMethod]
    public void TestMacroBinaryCollapseHang()
    {
      string      source = @"
                    !macro SBAHN_CHECK_SPRITE_COLLISION sprite_x_reg, sprite_pointer_reg, .additional_x_bit_mask {
                  lda sprite_pointer_reg
                  cmp #$E9
                  beq +                         ; kollision mit schienen interessiert nicht
                   lda $D010
                   and #%.additional_x_bit_mask
                  beq +                         ;extended bit gesetzt? Dann kollision nicht möglich
                  lda sprite_x_reg
                  cmp $D000
                  bcc +                         ;Sprite hinter figur? Dann kollision nicht möglich( schon forbei )
                  sec
                  sbc $D000
                  cmp #23                       ;Abstand über 23? Dann für Kollision noch zu früh
                  bcs +
                  lda #%10000000
                  sta STATUS
                  rts
                + nop
                  }";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "0000", assembly.ToString() );
    }


    [TestMethod]
    public void TestPseudoOpIfndefWithIndention()
    {
      string      source = @"idt1 = 1
                              !ifndef idt1 {
                                !error ""!!!""
                                }
                            ";
      var assembly = TestAssemble( source );

      Assert.AreEqual( "0000", assembly.ToString() );
    }



    [TestMethod]
    public void TestPseudoOpBASICWithoutArguments()
    {
      string      source = @"* = $0801
                              ;jump to directly after po
                              !basic
                              -
                                        inc $d020
                                        jmp -
                              ";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "01080B080A009E32303631000000EE20D04C0D08", assembly.ToString() );
    }


    [TestMethod]
    public void TestPseudoOpBASICWithLabel()
    {
      string      source = @"* = $0801
                            ;jump to label directly after po
                            !basic label
                            label
                            -
                                      inc $d020
                                      jmp -";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "01080B080A009E32303631000000EE20D04C0D08", assembly.ToString() );
    }



    [TestMethod]
    public void TestPseudoOpBASICWithLabelNotDirectlyAfterPO()
    {
      string      source = @"* = $0801
                            ;jump to label not directly after po
                            !basic label

                            label = $0820
                            * = $0820
                            -
                                      inc $d020
                                      jmp -";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "01080B080A009E3230383000000000000000000000000000000000000000000000EE20D04C2008", assembly.ToString() );
    }



    [TestMethod]
    public void TestPseudoOpBASICWithLabelNotDirectlyAfterPOAndMoreThan4Digits()
    {
      string      source = @"* = $0801
                            ;jump to label not directly after po with more than 4 digits
                            !basic label

                            label = $4000
                            * = $4000
                            -
                                      inc $d020
                                      jmp -";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "01080C080A009E3136333834000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000EE20D04C0040", assembly.ToString() );
    }


    [TestMethod]
    public void TestPseudoOpBASICWithLineNumberAndLabelDirectlyAfterPO()
    {
      string      source = @"* = $0801
                              ;jump with line number and label directly after PO
                              !basic 2018,label
                              label
                                        inc $d020
                                        rts";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "01080B08E2079E32303631000000EE20D060", assembly.ToString() );
    }



    [TestMethod]
    public void TestPseudoOpBASICWithLineNumberAndLabelNotDirectlyAfterPO()
    {
      string      source = @"* = $0801
                            ;jump with line number and label not directly after PO
                            !basic 2018,label
                            * = $820
                            label
                                      inc $d020
                                      rts";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "01080B08E2079E3230383000000000000000000000000000000000000000000000EE20D060", assembly.ToString() );
    }



    [TestMethod]
    public void TestPseudoOpBASICWithLineNumberAndLabelNotDirectlyAfterPOAndMoreThan4Digits()
    {
      string      source = @"* = $0801
                              ;jump with line number and label not directly after PO and more than 4 digits
                              !basic 2018,label
                              * = $4000
                              label
                                        inc $d020
                                        rts";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "01080C08E2079E3136333834000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000EE20D060", assembly.ToString() );
    }



    [TestMethod]
    public void TestPseudoOpBASICWithLineNumberCommentAndLabel()
    {
      string      source = @"* = $0801
                            ;jump with line number, comment and label directly after PO
                            !basic 2018,""COMMENT"",label
                            label
                                      inc $d020
                                      rts";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "01081208E2079E32303638434F4D4D454E54000000EE20D060", assembly.ToString() );
    }



    [TestMethod]
    public void TestMacroWithNumericArgumentAndNotEndingInUnparsedLabel()
    {
      string      source = @"* = 49152
                            !macro set_poke_pos poke_positionX {
                              poke_position = poke_positionX
                              }

                            +set_poke_pos 1024

                            lda #65
                            sta poke_position
                            rts";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "00C0A9418D000460", assembly.ToString() );
    }



    [TestMethod]
    public void TestMacroWithSetOrg()
    {
      string      source = @"!macro ENDIF {
                             curr_addr = *
                            * = STK_LVL_1 - 1
                            !byte ( curr_addr - STK_LVL_1 ) & 0xff
                              }
                            * = $2200
                            STK_LVL_1 = $2000
                            +ENDIF
                            rts";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "FF1F006000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000", assembly.ToString() );
    }


    [TestMethod]
    public void TestMacroWithLocalLabels()
    {
      string      source = @"*=$0801
        !basic
        A_REG  = 256
        X_REG  = 257
        Y_REG  = 258
        C_FLAG = 259
        Z_FLAG = 260
        N_FLAG = 261


        !macro IF value1, type1, condition, value2, type2, distance
           .error=1
           !if value1 = A_REG {
              !if value2 = X_REG {
                    .error=0
                    stx .cpyval+1
                 .cpyval
                    cpy #$ff
              }
        }

           !if .error=1 {
              !error 'E'
           }
        !end


        ;+IF value1, type1, condition, value2, type2, distance
        +IF A_REG, 0, 0, X_REG, 0, 0
        +IF A_REG, 0, 0, X_REG, 0, 0 ; sollte ich das hier auskommentieren, dann gehts
            rts";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "01080B080A009E323036310000008E1108C0FF8E1608C0FF60", assembly.ToString() );
    }



    [TestMethod]
    public void TestMacroNamedIf()
    {
      string      source = @"* = $2000
          REGISTER = 1
          A_REG = 2
          Y_REG = 3
          X_REG = 4

          IF_7_31_.error=1
          !if ╚REGISTER╝=REGISTER & ╚REGISTER╝=REGISTER {
          !if ╚A_REG╝=A_REG& Y_REG =X_REG {
          IF_7_31_.error=0
          stx IF_7_31_.tmp+1
          IF_7_31_.tmp:
          cmp #$ff
          }

          !if ╚A_REG╝=A_REG& Y_REG =Y_REG {
          IF_7_31_.error=0 ; <========================= HIER
          }


          }

          !if IF_7_31_.error=1 {
          !error ""Wrong Parameters in 'IF'""
          }
          rts";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "002060", assembly.ToString() );
    }



    [TestMethod]
    public void TestMacroUsingMacroWithArgument()
    {
      string      source = @"*=$c000
                              STACK_LEVEL0 = 0
                              GLOBAL_VAR = 3

                              !macro PUSH value
                                 ; some fancy code to move stack values down
                                 STACK_LEVEL0=value
                              !end

                              !macro SOME_MACRO_THAT_USES_PUSH
                                 +PUSH 7
                              !end

                              +SOME_MACRO_THAT_USES_PUSH";
      var assembly = TestAssemble( source );

      Assert.AreEqual( "00C0", assembly.ToString() );
    }



    [TestMethod]
    public void TestLineSeparationAtLabel()
    {
      string      source = @"*=$c000
                                ldx #0
                              LABEL1:
                                inc $d020
                                dex
                                bne LABEL1
                                rts";
      var assembly = TestAssemble( source );

      Assert.AreEqual( "00C0A200EE20D0CAD0FA60", assembly.ToString() );
    }



    [TestMethod]
    public void TestLineSeparation2()
    {
      string      source = @"*=$c000
                                ldx #0
                                ldy #$80
                              LABEL1: iny : dex : bne LABEL1
                                rts";
      var assembly = TestAssemble( source );

      Assert.AreEqual( "00C0A200A080C8CAD0FC60", assembly.ToString() );
    }



    [TestMethod]
    public void TestIfElseInsideMacro()
    {
      string      source = @"*=$0801
!basic ContrivedTest

!macro TestCase .someNumber {

  ;; Right below is how the code could have looked (logically less complex) but it didn't compile (!?)
  !if (.someNumber < 0) {
    !message ""Case X for "", .someNumber
  } else if (.someNumber >= 7) {  ; Line 9: E1001 -> ""Could not evaluate expression: (.someNumber >= 7)""
    !message ""Case Y for "", .someNumber
  } else if (.someNumber >= 4) {  ; Line 11: E1001 -> ""Could not evaluate expression: (.someNumber >= 4)""
    !message ""Case Z for "", .someNumber
} else {
    !message ""Unknown case for "", .someNumber
  }

  ;; But this is how the code had to be rewritten (logically more complex) for it to actually compile
  ;!if (.someNumber < 0) {
  ; !message ""Case X for "", .someNumber
 ;}
  ;!if (.someNumber >= 7) {
  ; !message ""Case Y for "", .someNumber
 ;}
  ;!if ((.someNumber < 7) and (.someNumber >= 4)) {
  ; !message ""Case Z for "", .someNumber
 ;}
  ;!if ((.someNumber < 4) and (.someNumber >= 0)) {
  ; !message ""Unknown case for "", .someNumber
 ;}

  ; Just for some fun... (not really affecting the test)
  lda #(.someNumber & 15)
  sta $d020; Set the border color
  ora #64    ; Turn into an printable character
  jsr $FFD2; Outputs character for number (CHROUT)
}

ContrivedTest:
  +TestCase -1
  +TestCase 0
  +TestCase 3
  +TestCase 4
  +TestCase 6
  +TestCase 7
  rts
";
      var assembly = TestAssemble( source );

      Assert.AreEqual( "01080B080A009E32303631000000A90F8D20D0094020D2FFA9008D20D0094020D2FFA9038D20D0094020D2FFA9048D20D0094020D2FFA9068D20D0094020D2FFA9078D20D0094020D2FF60", assembly.ToString() );
    }



    [TestMethod]
    public void TestIfElseChain1()
    {
      string      source = @"*=$0801
                             label = 3
                            
                              !if label < 5 {
                              lda #0
                              } else if label < 10 {
                              lda #1
                              } else {
                              lda #2
                              }
                              rts";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "0108A90060", assembly.ToString() );
    }



    [TestMethod]
    public void TestIfElseChain2()
    {
      string      source = @"*=$0801
                             label = 8
                            
                              !if label < 5 {
                              lda #0
                              } else if label < 10 {
                              lda #1
                              } else {
                              lda #2
                              }
                              rts";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "0108A90160", assembly.ToString() );
    }



    [TestMethod]
    public void TestIfElseChain3()
    {
      string      source = @"*=$0801
                             label = 18
                            
                              !if label < 5 {
                              lda #0
                              } else if label < 10 {
                              lda #1
                              } else {
                              lda #2
                              }
                              rts";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "0108A90260", assembly.ToString() );
    }


    
    [TestMethod]
    public void TestLineSeparators()
    {
      string      source = @"*=3000
                            RND=$de1b
                            lda #111:ldx #222:ldy #33      ; This works.  :)
                            lda 111:ldx 222:ldy 33         ; This works.  :)
                            lda $de1b:ldx $de1b:ldy $de1b  ; This works.  :)
                            lda RND:ldx RND:ldy RND        ; *** But this FAILS. ***  :(
                            lda RND :ldx RND :ldy RND      ; Oddly, this again works.
                            lda RND : ldx RND : ldy RND    ; This also works.
                            lda RND: ldx RND: ldy RND      ; *** But this again FAILS. ***";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "B80BA96FA2DEA021A56FA6DEA421AD1BDEAE1BDEAC1BDEAD1BDEAE1BDEAC1BDEAD1BDEAE1BDEAC1BDEAD1BDEAE1BDEAC1BDEAD1BDEAE1BDEAC1BDE", assembly.ToString() );
    }



    [TestMethod]
    public void TestIntEgdeCases()
    {
      string      source = @"*=$0801
            !basic

            inc $d020

            .theValue2 = -2147483647; This works.  :)
            .theValue3 = -2147483648; Was broken in 5.8 build of 2018 - 09 - 08, but fixed in build of 2018 - 09 - 15(is a valid 32 - bit integer )
            .theValue4 = -65536; This works.  :)
            .theValue5 = -256; This works.  :)
            .theValue6 = 256; This works.  :)
            .theValue7 = 65535; This works.  :)
            .theValue8 = 65536; This works.  :)
            .theValue9 = -2147483647; This works.  :)

            .theValue = 0
            .intMinVal = -2147483648

            !if ( 0 <= -2147483648 ) {
            }

            ; ***ERROR * **
            !if ( -2147483648 >= 0 ) {
            }

            ; ***ERROR * **
            !if (.theValue <= -2147483648 ) {
            }

            ; ***ERROR * **
            !if ( -2147483648 >= .theValue ) {
            }

            ; ***ERROR * **
            !if (.theValue <= .intMinVal )             {
            }
            
            ; OK in 2018 - 09 - 15 build( was broken in 2018 - 09 - 08 build )
            !if (.intMinVal >= .theValue ) {
            }
             
            ; OK in 2018 - 09 - 15 build( was broken in 2018 - 09 - 08 build )
            !if (.theValue <> 0 ) {
            }
            
            ; OK in 2018 - 09 - 15 build( was broken in 2018 - 09 - 08 build )
            !if (.theValue = 0 ) {
            }

            ; OK in 2018 - 09 - 15 build: Previous E1001 now fixed in 2018 - 09 - 15 build( when.theValue = -2147483648 )
            inc $d020

            rts";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "01080B080A009E32303631000000EE20D0EE20D060", assembly.ToString() );
    }


    [TestMethod]
    public void TestNegativeLiteralCollapsing()
    {
      string      source = @"!macro Fill1000 .startAddress
                            {
                              ldx #250
                            .loop
                              sta .startAddress-1+0,x
                              sta .startAddress-1+250,x
                              sta .startAddress-1+500,x
                              sta .startAddress-1+750,x
                              dex
                              bne .loop
                                }

                                SCREEN_RAM  = $0400  ; 1024

                              *=$0801
                              !basic

                              ldy #0
                            forever:
                              tya
                              +Fill1000 SCREEN_RAM
                              iny
                              jmp forever";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "01080B080A009E32303631000000A00098A2FA9DFF039DF9049DF3059DED06CAD0F1C84C0F08", assembly.ToString() );
    }



    [TestMethod]
    public void TestNegativeLiteralCollapsingNested()
    {
      string      source = @"    !macro Fill1000 .startAddress {
                                  ldx #250
                            .loop
                              sta .startAddress-1+0,x
                              sta .startAddress-1+250,x
                              sta .startAddress-1+500,x
                              sta .startAddress-1+750,x
                              dex
                              bne .loop
                                }

                            !macro Fill1000v2 .startAddress, .byteConstant {
                                  lda #.byteConstant
                              +Fill1000 .startAddress
                            }

                                SCREEN_RAM = $0400  ; 1024
                            SCREENCODE = 46

                              *=$0801
                              !basic

                              ldy #0
                            forever:
                              tya
                              +Fill1000 SCREEN_RAM
                              +Fill1000v2 SCREEN_RAM,32
                              +Fill1000v2 SCREEN_RAM, SCREENCODE
                              iny
                              jmp forever
                            ";

      var assembly = TestAssemble( source );

      Assert.AreEqual( "01080B080A009E32303631000000A00098A2FA9DFF039DF9049DF3059DED06CAD0F1A920A2FA9DFF039DF9049DF3059DED06CAD0F1A92EA2FA9DFF039DF9049DF3059DED06CAD0F1C84C0F08", assembly.ToString() );
    }


  }
}
