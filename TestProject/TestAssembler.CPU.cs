using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
  public partial class TestAssembler
  {
    [TestMethod]
    public void TestAssembly6510Opcodes()
    {
      string      source = @"!cpu 6510
                        !to ""cpu6510.bin"",plain

                        * = $1000

                        adc $1234
                        adc $1234,x
                        adc $1234,y
                        adc $12
                        adc $12,x
                        adc($12 ),y
                        adc($12, x )
                        adc #$12

                        and $1234
                        and $1234,x
                        and $1234,y
                        and $12
                        and $12,x
                        and($12 ),y
                        and($12, x )
                        and #$12

                        asl
                        asl $1234
                        asl $1234,x
                        asl $12
                        asl $12,x

                        bcc $1034
                        bcs $1034
                        beq $1034
                        bit $1234
                        bit $12

                        bmi $1034
                        bne $1034
                        bpl $1034
                        brk

                        bvc $1034
                        bvs $1034

                        clc
                        cld
                        cli
                        clv

                        cmp $1234
                        cmp $1234,x
                        cmp $1234,y
                        cmp $12
                        cmp $12,x
                        cmp($12 ),y
                        cmp($12, x )
                        cmp #$12

                        cpx $1234

                        cpx $1234
                        cpx $12
                        cpx #$12

                        cpy $1234
                        cpy $12
                        cpy #$12

                        dec $1234
                        dec $1234,x
                        dec $12
                        dec $12,x

                        dex
                        dey

                        eor $1234
                        eor $1234,x
                        eor $1234,y
                        eor $12
                        eor $12,x
                        eor($12 ),y
                        eor($12, x )
                        eor #$12

                        inc $1234
                        inc $1234,x
                        inc $12
                        inc $12,x

                        inx
                        iny

                        jmp $1234
                        jmp($1234 )
                        jsr $1234

                        lda $1234
                        lda $1234,x
                        lda $1234,y
                        lda $12
                        lda $12,x
                        lda($12 ),y
                        lda($12, x )
                        lda #$12

                        ldx $1234
                        ldx $1234,y
                        ldx $12
                        ldx $12,y
                        ldx #$12

                        ldy $1234
                        ldy $1234,x
                        ldy $12
                        ldy $12,x
                        ldy #$12

                        lsr
                        lsr $1234
                        lsr $1234,x
                        lsr $12
                        lsr $12,x

                        nop

                        ora $1234
                        ora $1234,x
                        ora $1234,y
                        ora $12
                        ora $12,x
                        ora($12 ),y
                        ora($12, x )
                        ora #$12

                        pha
                        php
                        pla
                        plp

                        rol
                        rol $1234
                        rol $1234,x
                        rol $12
                        rol $12,x
                        ror
                        ror $1234
                        ror $1234,x
                        ror $12
                        ror $12,x

                        rti
                        rts

                        sbc $1234
                        sbc $1234,x
                        sbc $1234,y
                        sbc $12
                        sbc $12,x
                        sbc($12 ),y
                        sbc($12, x )
                        sbc #$12

                        sec
                        sed
                        sei

                        sta $1234
                        sta $1234,x
                        sta $1234,y
                        sta $12
                        sta $12,x
                        sta($12 ),y
                        sta($12, x )

                        stx $1234
                        stx $12
                        stx $12,y
                        sty $1234
                        sty $12
                        sty $12,x

                        tax
                        tay
                        tsx
                        txa
                        txs
                        tya

                        jam

                        ;
                              illegal
                             slo $12
                        slo $12,x
                        slo($12, x )
                        slo($12 ),y
                        slo $1234
                        slo $1234,x
                        slo $1234,y

                        rla $12
                        rla $12,x
                        rla($12, x )
                        rla($12 ),y
                        rla $1234
                        rla $1234,x
                        rla $1234,y

                        sre $12
                        sre $12,x
                        sre($12, x )
                        sre($12 ),y
                        sre $1234
                        sre $1234,x
                        sre $1234,y

                        rra $12
                        rra $12,x
                        rra($12, x )
                        rra($12 ),y
                        rra $1234
                        rra $1234,x
                        rra $1234,y

                        sax $12
                        sax $12,y
                        sax($12, x )
                        sax $1234

                        lax $12
                        lax $12,y
                        lax($12, x )
                        lax($12 ),y
                        lax $1234
                        lax $1234,y

                        dcp $12
                        dcp $12,x
                        dcp($12, x )
                        dcp($12 ),y
                        dcp $1234
                        dcp $1234,x
                        dcp $1234,y

                        isc $12
                        isc $12,x
                        isc($12, x )
                        isc($12 ),y
                        isc $1234
                        isc $1234,x
                        isc $1234,y

                        anc #$12

                        ane #$12
                        lxa #$12
                        sbx #$12
                        sha($12),y
                        sha $1234,y
                        shy $1234,x
                        shx $1234,y
                        tas $1234,y
                        las $1234,y

                        axs #$12    ;alternative to sbx

                        nop #$12
                        nop $12
                        nop $1234
                        nop $1234,x";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.bin";
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PLAIN;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );
      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( "6D34127D3412793412651275127112611269122D34123D3412393412251235123112211229120A0E34121E3412061216129001B0FFF0FD2C3412241230F6D0F410F20050EF70ED18D858B8CD3412DD3412D93412C512D512D112C112C912EC3412EC3412E412E012CC3412C412C012CE3412DE3412C612D612CA884D34125D341259341245125512511241124912EE3412FE3412E612F612E8C84C34126C3412203412AD3412BD3412B93412A512B512B112A112A912AE3412BE3412A612B612A212AC3412BC3412A412B412A0124A4E34125E341246125612EA0D34121D341219341205121512111201120912480868282A2E34123E3412261236126A6E34127E3412661276124060ED3412FD3412F93412E512F512F112E112E91238F8788D34129D341299341285129512911281128E3412861296128C341284129412AAA8BA8A9A980207121712031213120F34121F34121B341227123712231233122F34123F34123B341247125712431253124F34125F34125B341267127712631273126F34127F34127B34128712971283128F3412A712B712A312B312AF3412BF3412C712D712C312D312CF3412DF3412DB3412E712F712E312F312EF3412FF3412FB34120B128B12AB12CB1293129F34129C34129E34129B3412BB3412CB12801204120C34121C3412", assembly.Assembly.ToString() );
    }



    [TestMethod]
    public void TestAssembly65C02Opcodes()
    {
      string      source = @"!cpu 65c02
                            !to ""cpu65c02.bin"",plain

                            * = $1000

                            adc $1234
                            adc $1234,x
                            adc $1234,y
                            adc $12
                            adc $12,x
                            adc($12 ),y
                            adc($12, x )
                            adc #$12

                            and $1234
                            and $1234,x
                            and $1234,y
                            and $12
                            and $12,x
                            and($12 ),y
                            and($12, x )
                            and #$12

                            asl
                            asl $1234
                            asl $1234,x
                            asl $12
                            asl $12,x

                            bcc $1034
                            bcs $1034
                            beq $1034
                            bit $1234
                            bit $12

                            bmi $1034
                            bne $1034
                            bpl $1034
                            brk

                            bvc $1034
                            bvs $1034

                            clc
                            cld
                            cli
                            clv

                            cmp $1234
                            cmp $1234,x
                            cmp $1234,y
                            cmp $12
                            cmp $12,x
                            cmp($12 ),y
                            cmp($12, x )
                            cmp #$12

                            cpx $1234

                            cpx $1234
                            cpx $12
                            cpx #$12

                            cpy $1234
                            cpy $12
                            cpy #$12

                            dec $1234
                            dec $1234,x
                            dec $12
                            dec $12,x

                            dex
                            dey

                            eor $1234
                            eor $1234,x
                            eor $1234,y
                            eor $12
                            eor $12,x
                            eor($12 ),y
                            eor($12, x )
                            eor #$12

                            inc $1234
                            inc $1234,x
                            inc $12
                            inc $12,x

                            inx
                            iny

                            jmp $1234
                            jmp($1234 )
                            jsr $1234

                            lda $1234
                            lda $1234,x
                            lda $1234,y
                            lda $12
                            lda $12,x
                            lda($12 ),y
                            lda($12, x )
                            lda #$12

                            ldx $1234
                            ldx $1234,y
                            ldx $12
                            ldx $12,y
                            ldx #$12

                            ldy $1234
                            ldy $1234,x
                            ldy $12
                            ldy $12,x
                            ldy #$12

                            lsr
                            lsr $1234
                            lsr $1234,x
                            lsr $12
                            lsr $12,x

                            nop

                            ora $1234
                            ora $1234,x
                            ora $1234,y
                            ora $12
                            ora $12,x
                            ora($12 ),y
                            ora($12, x )
                            ora #$12

                            pha
                            php
                            pla
                            plp

                            rol
                            rol $1234
                            rol $1234,x
                            rol $12
                            rol $12,x
                            ror
                            ror $1234
                            ror $1234,x
                            ror $12
                            ror $12,x

                            rti
                            rts

                            sbc $1234
                            sbc $1234,x
                            sbc $1234,y
                            sbc $12
                            sbc $12,x
                            sbc($12 ),y
                            sbc($12, x )
                            sbc #$12

                            sec
                            sed
                            sei

                            sta $1234
                            sta $1234,x
                            sta $1234,y
                            sta $12
                            sta $12,x
                            sta($12 ),y
                            sta($12, x )

                            stx $1234
                            stx $12
                            stx $12,y
                            sty $1234
                            sty $12
                            sty $12,x

                            tax
                            tay
                            tsx
                            txa
                            txs
                            tya

                            adc($12 )
                            and($12 )
                            cmp($12 )
                            eor($12 )
                            lda($12 )
                            ora($12 )
                            sbc($12 )
                            sta($12 )

                            bit #$12
                            bit $12,x
                            bit $1234,x

                            dec
                            inc

                            jmp($1234, x )

                            bra $1134

                            phx
                            phy
                            plx
                            ply

                            stz $12
                            stz $12,x
                            stz $1234
                            stz $1234,x

                            trb $12
                            trb $1234

                            tsb $12
                            tsb $1234";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.bin";
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PLAIN;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );
      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( "6D34127D3412793412651275127112611269122D34123D3412393412251235123112211229120A0E34121E3412061216129001B0FFF0FD2C3412241230F6D0F410F20050EF70ED18D858B8CD3412DD3412D93412C512D512D112C112C912EC3412EC3412E412E012CC3412C412C012CE3412DE3412C612D612CA884D34125D341259341245125512511241124912EE3412FE3412E612F612E8C84C34126C3412203412AD3412BD3412B93412A512B512B112A112A912AE3412BE3412A612B612A212AC3412BC3412A412B412A0124A4E34125E341246125612EA0D34121D341219341205121512111201120912480868282A2E34123E3412261236126A6E34127E3412661276124060ED3412FD3412F93412E512F512F112E112E91238F8788D34129D341299341285129512911281128E3412861296128C341284129412AAA8BA8A9A9872123212D2125212B2121212F2129212891234123C34123A1A7C341280D2DA5AFA7A641274129C34129E341214121C341204120C3412", assembly.Assembly.ToString() );
    }



    [TestMethod]
    public void TestAssemblyR65C02Opcodes()
    {
      string      source = @"!cpu r65c02
                                !to ""cpu_r65c02.prg"",plain

                                * = $1000

                                adc $1234
                                adc $1234,x
                                adc $1234,y
                                adc $12
                                adc $12,x
                                adc($12 ),y
                                adc($12, x )
                                adc #$12

                                and $1234
                                and $1234,x
                                and $1234,y
                                and $12
                                and $12,x
                                and($12 ),y
                                and($12, x )
                                and #$12

                                asl
                                asl $1234
                                asl $1234,x
                                asl $12
                                asl $12,x

                                bcc $1034
                                bcs $1034
                                beq $1034
                                bit $1234
                                bit $12

                                bmi $1034
                                bne $1034
                                bpl $1034
                                brk

                                bvc $1034
                                bvs $1034

                                clc
                                cld
                                cli
                                clv

                                cmp $1234
                                cmp $1234,x
                                cmp $1234,y
                                cmp $12
                                cmp $12,x
                                cmp($12 ),y
                                cmp($12, x )
                                cmp #$12

                                cpx $1234

                                cpx $1234
                                cpx $12
                                cpx #$12

                                cpy $1234
                                cpy $12
                                cpy #$12

                                dec $1234
                                dec $1234,x
                                dec $12
                                dec $12,x

                                dex
                                dey

                                eor $1234
                                eor $1234,x
                                eor $1234,y
                                eor $12
                                eor $12,x
                                eor($12 ),y
                                eor($12, x )
                                eor #$12

                                inc $1234
                                inc $1234,x
                                inc $12
                                inc $12,x

                                inx
                                iny

                                jmp $1234
                                jmp($1234 )
                                jsr $1234

                                lda $1234
                                lda $1234,x
                                lda $1234,y
                                lda $12
                                lda $12,x
                                lda($12 ),y
                                lda($12, x )
                                lda #$12

                                ldx $1234
                                ldx $1234,y
                                ldx $12
                                ldx $12,y
                                ldx #$12

                                ldy $1234
                                ldy $1234,x
                                ldy $12
                                ldy $12,x
                                ldy #$12

                                lsr
                                lsr $1234
                                lsr $1234,x
                                lsr $12
                                lsr $12,x

                                nop

                                ora $1234
                                ora $1234,x
                                ora $1234,y
                                ora $12
                                ora $12,x
                                ora($12 ),y
                                ora($12, x )
                                ora #$12

                                pha
                                php
                                pla
                                plp

                                rol
                                rol $1234
                                rol $1234,x
                                rol $12
                                rol $12,x
                                ror
                                ror $1234
                                ror $1234,x
                                ror $12
                                ror $12,x

                                rti
                                rts

                                sbc $1234
                                sbc $1234,x
                                sbc $1234,y
                                sbc $12
                                sbc $12,x
                                sbc($12 ),y
                                sbc($12, x )
                                sbc #$12

                                sec
                                sed
                                sei

                                sta $1234
                                sta $1234,x
                                sta $1234,y
                                sta $12
                                sta $12,x
                                sta($12 ),y
                                sta($12, x )

                                stx $1234
                                stx $12
                                stx $12,y
                                sty $1234
                                sty $12
                                sty $12,x

                                tax
                                tay
                                tsx
                                txa
                                txs
                                tya

                                adc($12 )
                                and($12 )
                                cmp($12 )
                                eor($12 )
                                lda($12 )
                                ora($12 )
                                sbc($12 )
                                sta($12 )

                                bit #$12
                                bit $12,x
                                bit $1234,x

                                dec
                                inc

                                jmp($1234, x )

                                bra $1134

                                phx
                                phy
                                plx
                                ply

                                stz $12
                                stz $12,x
                                stz $1234
                                stz $1234,x

                                trb $12
                                trb $1234

                                tsb $12
                                tsb $1234

                                bbr0 $12,$1134
                                bbr1 $12,$1134
                                bbr2 $12,$1134
                                bbr3 $12,$1134
                                bbr4 $12,$1134
                                bbr5 $12,$1134
                                bbr6 $12,$1134
                                bbr7 $12,$1134
                                bbs0 $12,$1134
                                bbs1 $12,$1134
                                bbs2 $12,$1134
                                bbs3 $12,$1134
                                bbs4 $12,$1134
                                bbs5 $12,$1134
                                bbs6 $12,$1134
                                bbs7 $12,$1134

                                rmb0 $12
                                rmb1 $12
                                rmb2 $12
                                rmb3 $12
                                rmb4 $12
                                rmb5 $12
                                rmb6 $12
                                rmb7 $12
                                smb0 $12
                                smb1 $12
                                smb2 $12
                                smb3 $12
                                smb4 $12
                                smb5 $12
                                smb6 $12
                                smb7 $12";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.bin";
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PLAIN;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );
      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( "6D34127D3412793412651275127112611269122D34123D3412393412251235123112211229120A0E34121E3412061216129001B0FFF0FD2C3412241230F6D0F410F20050EF70ED18D858B8CD3412DD3412D93412C512D512D112C112C912EC3412EC3412E412E012CC3412C412C012CE3412DE3412C612D612CA884D34125D341259341245125512511241124912EE3412FE3412E612F612E8C84C34126C3412203412AD3412BD3412B93412A512B512B112A112A912AE3412BE3412A612B612A212AC3412BC3412A412B412A0124A4E34125E341246125612EA0D34121D341219341205121512111201120912480868282A2E34123E3412261236126A6E34127E3412661276124060ED3412FD3412F93412E512F512F112E112E91238F8788D34129D341299341285129512911281128E3412861296128C341284129412AAA8BA8A9A9872123212D2125212B2121212F2129212891234123C34123A1A7C341280D2DA5AFA7A641274129C34129E341214121C341204120C34120F12B71F12B42F12B13F12AE4F12AB5F12A86F12A57F12A28F129F9F129CAF1299BF1296CF1293DF1290EF128DFF128A0712171227123712471257126712771287129712A712B712C712D712E712F712", assembly.Assembly.ToString() );
    }



    [TestMethod]
    public void TestAssemblyW65C02Opcodes()
    {
      string      source = @"!cpu w65c02
                                !to ""cpu_w65c02.prg"",plain

                                * = $1000

                                adc $1234
                                adc $1234,x
                                adc $1234,y
                                adc $12
                                adc $12,x
                                adc($12 ),y
                                adc($12, x )
                                adc #$12

                                and $1234
                                and $1234,x
                                and $1234,y
                                and $12
                                and $12,x
                                and($12 ),y
                                and($12, x )
                                and #$12

                                asl
                                asl $1234
                                asl $1234,x
                                asl $12
                                asl $12,x

                                bcc $1034
                                bcs $1034
                                beq $1034
                                bit $1234
                                bit $12

                                bmi $1034
                                bne $1034
                                bpl $1034
                                brk

                                bvc $1034
                                bvs $1034

                                clc
                                cld
                                cli
                                clv

                                cmp $1234
                                cmp $1234,x
                                cmp $1234,y
                                cmp $12
                                cmp $12,x
                                cmp($12 ),y
                                cmp($12, x )
                                cmp #$12

                                cpx $1234

                                cpx $1234
                                cpx $12
                                cpx #$12

                                cpy $1234
                                cpy $12
                                cpy #$12

                                dec $1234
                                dec $1234,x
                                dec $12
                                dec $12,x

                                dex
                                dey

                                eor $1234
                                eor $1234,x
                                eor $1234,y
                                eor $12
                                eor $12,x
                                eor($12 ),y
                                eor($12, x )
                                eor #$12

                                inc $1234
                                inc $1234,x
                                inc $12
                                inc $12,x

                                inx
                                iny

                                jmp $1234
                                jmp($1234 )
                                jsr $1234

                                lda $1234
                                lda $1234,x
                                lda $1234,y
                                lda $12
                                lda $12,x
                                lda($12 ),y
                                lda($12, x )
                                lda #$12

                                ldx $1234
                                ldx $1234,y
                                ldx $12
                                ldx $12,y
                                ldx #$12

                                ldy $1234
                                ldy $1234,x
                                ldy $12
                                ldy $12,x
                                ldy #$12

                                lsr
                                lsr $1234
                                lsr $1234,x
                                lsr $12
                                lsr $12,x

                                nop

                                ora $1234
                                ora $1234,x
                                ora $1234,y
                                ora $12
                                ora $12,x
                                ora($12 ),y
                                ora($12, x )
                                ora #$12

                                pha
                                php
                                pla
                                plp

                                rol
                                rol $1234
                                rol $1234,x
                                rol $12
                                rol $12,x
                                ror
                                ror $1234
                                ror $1234,x
                                ror $12
                                ror $12,x

                                rti
                                rts

                                sbc $1234
                                sbc $1234,x
                                sbc $1234,y
                                sbc $12
                                sbc $12,x
                                sbc($12 ),y
                                sbc($12, x )
                                sbc #$12

                                sec
                                sed
                                sei

                                sta $1234
                                sta $1234,x
                                sta $1234,y
                                sta $12
                                sta $12,x
                                sta($12 ),y
                                sta($12, x )

                                stx $1234
                                stx $12
                                stx $12,y
                                sty $1234
                                sty $12
                                sty $12,x

                                tax
                                tay
                                tsx
                                txa
                                txs
                                tya

                                adc($12 )
                                and($12 )
                                cmp($12 )
                                eor($12 )
                                lda($12 )
                                ora($12 )
                                sbc($12 )
                                sta($12 )

                                bit #$12
                                bit $12,x
                                bit $1234,x

                                dec
                                inc

                                jmp($1234, x )

                                bra $1134

                                phx
                                phy
                                plx
                                ply

                                stz $12
                                stz $12,x
                                stz $1234
                                stz $1234,x

                                trb $12
                                trb $1234

                                tsb $12
                                tsb $1234

                                bbr0 $12,$1134
                                bbr1 $12,$1134
                                bbr2 $12,$1134
                                bbr3 $12,$1134
                                bbr4 $12,$1134
                                bbr5 $12,$1134
                                bbr6 $12,$1134
                                bbr7 $12,$1134
                                bbs0 $12,$1134
                                bbs1 $12,$1134
                                bbs2 $12,$1134
                                bbs3 $12,$1134
                                bbs4 $12,$1134
                                bbs5 $12,$1134
                                bbs6 $12,$1134
                                bbs7 $12,$1134

                                rmb0 $12
                                rmb1 $12
                                rmb2 $12
                                rmb3 $12
                                rmb4 $12
                                rmb5 $12
                                rmb6 $12
                                rmb7 $12
                                smb0 $12
                                smb1 $12
                                smb2 $12
                                smb3 $12
                                smb4 $12
                                smb5 $12
                                smb6 $12
                                smb7 $12

                                stp
                                wai";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.bin";
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PLAIN;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );
      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( "6D34127D3412793412651275127112611269122D34123D3412393412251235123112211229120A0E34121E3412061216129001B0FFF0FD2C3412241230F6D0F410F20050EF70ED18D858B8CD3412DD3412D93412C512D512D112C112C912EC3412EC3412E412E012CC3412C412C012CE3412DE3412C612D612CA884D34125D341259341245125512511241124912EE3412FE3412E612F612E8C84C34126C3412203412AD3412BD3412B93412A512B512B112A112A912AE3412BE3412A612B612A212AC3412BC3412A412B412A0124A4E34125E341246125612EA0D34121D341219341205121512111201120912480868282A2E34123E3412261236126A6E34127E3412661276124060ED3412FD3412F93412E512F512F112E112E91238F8788D34129D341299341285129512911281128E3412861296128C341284129412AAA8BA8A9A9872123212D2125212B2121212F2129212891234123C34123A1A7C341280D2DA5AFA7A641274129C34129E341214121C341204120C34120F12B71F12B42F12B13F12AE4F12AB5F12A86F12A57F12A28F129F9F129CAF1299BF1296CF1293DF1290EF128DFF128A0712171227123712471257126712771287129712A712B712C712D712E712F712DBCB", assembly.Assembly.ToString() );
    }



    [TestMethod]
    public void TestAssembly65CE02Opcodes()
    {
      string      source = @"!cpu 65ce02

                            !to ""cpu_65ce02.prg"",plain

                            * = $1000

                            adc $1234
                            adc $1234,x
                            adc $1234,y
                            adc $12
                            adc $12,x
                            adc($12 ),y
                            adc($12, x )
                            adc #$12

                            and $1234
                            and $1234,x
                            and $1234,y
                            and $12
                            and $12,x
                            and($12 ),y
                            and($12, x )
                            and #$12

                            asl
                            asl $1234
                            asl $1234,x
                            asl $12
                            asl $12,x

                            bcc $1034
                            bcs $1034
                            beq $1034
                            bit $1234
                            bit $12

                            bmi $1034
                            bne $1034
                            bpl $1034
                            brk

                            bvc $1034
                            bvs $1034

                            clc
                            cld
                            cli
                            clv

                            cmp $1234
                            cmp $1234,x
                            cmp $1234,y
                            cmp $12
                            cmp $12,x
                            cmp($12 ),y
                            cmp($12, x )
                            cmp #$12

                            cpx $1234

                            cpx $1234
                            cpx $12
                            cpx #$12

                            cpy $1234
                            cpy $12
                            cpy #$12

                            dec $1234
                            dec $1234,x
                            dec $12
                            dec $12,x

                            dex
                            dey

                            eor $1234
                            eor $1234,x
                            eor $1234,y
                            eor $12
                            eor $12,x
                            eor($12 ),y
                            eor($12, x )
                            eor #$12

                            inc $1234
                            inc $1234,x
                            inc $12
                            inc $12,x

                            inx
                            iny

                            jmp $1234
                            jmp($1234 )
                            jsr $1234

                            lda $1234
                            lda $1234,x
                            lda $1234,y
                            lda $12
                            lda $12,x
                            lda($12 ),y
                            lda($12, x )
                            lda #$12

                            ldx $1234
                            ldx $1234,y
                            ldx $12
                            ldx $12,y
                            ldx #$12

                            ldy $1234
                            ldy $1234,x
                            ldy $12
                            ldy $12,x
                            ldy #$12

                            lsr
                            lsr $1234
                            lsr $1234,x
                            lsr $12
                            lsr $12,x

                            nop

                            ora $1234
                            ora $1234,x
                            ora $1234,y
                            ora $12
                            ora $12,x
                            ora($12 ),y
                            ora($12, x )
                            ora #$12

                            pha
                            php
                            pla
                            plp

                            rol
                            rol $1234
                            rol $1234,x
                            rol $12
                            rol $12,x
                            ror
                            ror $1234
                            ror $1234,x
                            ror $12
                            ror $12,x

                            rti
                            rts

                            sbc $1234
                            sbc $1234,x
                            sbc $1234,y
                            sbc $12
                            sbc $12,x
                            sbc($12 ),y
                            sbc($12, x )
                            sbc #$12

                            sec
                            sed
                            sei

                            sta $1234
                            sta $1234,x
                            sta $1234,y
                            sta $12
                            sta $12,x
                            sta($12 ),y
                            sta($12, x )

                            stx $1234
                            stx $12
                            stx $12,y
                            sty $1234
                            sty $12
                            sty $12,x

                            tax
                            tay
                            tsx
                            txa
                            txs
                            tya

                            adc($12 ),z
                            and($12 ),z
                            cmp($12 ),z
                            eor($12 ),z
                            lda($12 ),z
                            ora($12 ),z
                            sbc($12 ),z
                            sta($12 ),z

                            bit #$12
                            bit $12,x
                            bit $1234,x

                            dec
                            inc

                            jmp($1234, x )

                            bra $1134

                            phx
                            phy
                            plx
                            ply

                            stz $12
                            stz $12,x
                            stz $1234
                            stz $1234,x

                            trb $12
                            trb $1234

                            tsb $12
                            tsb $1234

                            bbr0 $12,$1134
                            bbr1 $12,$1134
                            bbr2 $12,$1134
                            bbr3 $12,$1134
                            bbr4 $12,$1134
                            bbr5 $12,$1134
                            bbr6 $12,$1134
                            bbr7 $12,$1134
                            bbs0 $12,$1134
                            bbs1 $12,$1134
                            bbs2 $12,$1134
                            bbs3 $12,$1134
                            bbs4 $12,$1134
                            bbs5 $12,$1134
                            bbs6 $12,$1134
                            bbs7 $12,$1134

                            rmb0 $12
                            rmb1 $12
                            rmb2 $12
                            rmb3 $12
                            rmb4 $12
                            rmb5 $12
                            rmb6 $12
                            rmb7 $12
                            smb0 $12
                            smb1 $12
                            smb2 $12
                            smb3 $12
                            smb4 $12
                            smb5 $12
                            smb6 $12
                            smb7 $12

                            inz
                            dez
                            taz
                            tza
                            ldz #$12
                            ldz $1234
                            ldz $1234,x
                            cpz #$12
                            cpz $12
                            phz
                            cpz $1234
                            plz

                            cle
                            see
                            tsy
                            tys

                            lbpl $1234
                            lbmi $1234
                            lbvc $1234
                            bsr $1234
                            lbvs $1234
                            lbra $1234
                            lbcc $1234
                            lbcs $1234
                            lbne $1234
                            lbeq $1234

                            jsr($1234 )
                            jsr($1234, x )
                            sta($12, s ),y
                             sty $1234,x
                             stx $1234,y
                            lda($12, s ),y

                             neg
                            asr
                            asr $12
                            asr $12,x
                            tab
                            aug
                            rtn #$12
                            tba
                            dew $12
                            asw $1234
                            inw $12
                            row $1234
                            phw #$1234
                            phw $1234";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.bin";
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PLAIN;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );
      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( "6D34127D3412793412651275127112611269122D34123D3412393412251235123112211229120A0E34121E3412061216129001B0FFF0FD2C3412241230F6D0F410F20050EF70ED18D858B8CD3412DD3412D93412C512D512D112C112C912EC3412EC3412E412E012CC3412C412C012CE3412DE3412C612D612CA884D34125D341259341245125512511241124912EE3412FE3412E612F612E8C84C34126C3412203412AD3412BD3412B93412A512B512B112A112A912AE3412BE3412A612B612A212AC3412BC3412A412B412A0124A4E34125E341246125612EA0D34121D341219341205121512111201120912480868282A2E34123E3412261236126A6E34127E3412661276124060ED3412FD3412F93412E512F512F112E112E91238F8788D34129D341299341285129512911281128E3412861296128C341284129412AAA8BA8A9A9872123212D2125212B2121212F2129212891234123C34123A1A7C341280D2DA5AFA7A641274129C34129E341214121C341204120C34120F12B71F12B42F12B13F12AE4F12AB5F12A86F12A57F12A28F129F9F129CAF1299BF1296CF1293DF1290EF128DFF128A0712171227123712471257126712771287129712A712B712C712D712E712F7121B3B4B6BA312AB3412BB3412C212D412DBDC3412FB02030B2B134F00334C00534900634600734300834000933D00B33A00D33700F3340022341223341282128B34129B3412E2124243441254125B5C62127BC312CB3412E312EB3412F43412FC3412", assembly.Assembly.ToString() );
    }



    [TestMethod]
    public void TestAssembly65816Opcodes()
    {
      string      source = @"!cpu 65816

                    !to ""cpu_65816.prg"",plain

                    * = $1000

	                  ; all the standard 6502 instructions:
		                brk		; 00
		                ora ($01, x)	; 01
		                ora $05		; 05
		                asl $05		; 06
		                php		; 08
		                ora #$09	; 09
		                asl		; 0a
		                ora $0d0e	; 0d
		                asl $0d0e	; 0e
		                bpl * + 2	; 10
		                ora ($11), y	; 11
		                ora $15, x	; 15
		                asl $15, x	; 16
		                clc		; 18
		                ora $1919, y	; 19
		                ora $1d1e, x	; 1d
		                asl $1d1e, x	; 1e
		                jsr $0d0e	; 20
		                and ($01, x)	; 21
		                bit $05		; 24
		                and $05		; 25
		                rol $05		; 26
		                plp		; 28
		                and #$09	; 29
		                rol		; 2a
		                bit $0d0e	; 2c
		                and $0d0e	; 2d
		                rol $0d0e	; 2e
		                bmi * + 2	; 30
		                and ($11), y	; 31
		                and $15, x	; 35
		                rol $15, x	; 36
		                sec		; 38
		                and $1919, y	; 39
		                and $1d1e, x	; 3d
		                rol $1d1e, x	; 3e
		                rti		; 40
		                eor ($01, x)	; 41
		                eor $05		; 45
		                lsr $05		; 46
		                pha		; 48
		                eor #$09	; 49
		                lsr		; 4a
		                jmp $0d0e	; 4c
		                eor $0d0e	; 4d
		                lsr $0d0e	; 4e
		                bvc * + 2	; 50
		                eor ($11), y	; 51
		                eor $15, x	; 55
		                lsr $15, x	; 56
		                cli		; 58
		                eor $1919, y	; 59
		                eor $1d1e, x	; 5d
		                lsr $1d1e, x	; 5e
		                rts		; 60
		                adc ($01, x)	; 61
		                adc $05		; 65
		                ror $05		; 66
		                pla		; 68
		                adc #$09	; 69
		                ror		; 6a
		                jmp ($6c6c)	; 6c
		                adc $0d0e	; 6d
		                ror $0d0e	; 6e
		                bvs * + 2	; 70
		                adc ($11), y	; 71
		                adc $15, x	; 75
		                ror $15, x	; 76
		                sei		; 78
		                adc $1919, y	; 79
		                adc $1d1e, x	; 7d
		                ror $1d1e, x	; 7e
		                sta ($01, x)	; 81
		                sty $05		; 84
		                sta $05		; 85
		                stx $05		; 86
		                dey		; 88
		                txa		; 8a
		                sty $0d0e	; 8c
		                sta $0d0e	; 8d
		                stx $0d0e	; 8e
		                bcc * + 2	; 90
		                sta ($11), y	; 91
		                sty $15, x	; 94
		                sta $15, x	; 95
		                stx $96, y	; 96
		                tya		; 98
		                sta $1919, y	; 99
		                txs		; 9a
		                sta $1d1e, x	; 9d
		                ldy #$09	; a0
		                lda ($01, x)	; a1
		                ldx #$09	; a2
		                ldy $05		; a4
		                lda $05		; a5
		                ldx $05		; a6
		                tay		; a8
		                lda #$09	; a9
		                tax		; aa
		                ldy $0d0e	; ac
		                lda $0d0e	; ad
		                ldx $0d0e	; ae
		                bcs * + 2	; b0
		                lda ($11), y	; b1
		                ldy $15, x	; b4
		                lda $15, x	; b5
		                ldx $96, y	; b6
		                clv		; b8
		                lda $1919, y	; b9
		                tsx		; ba
		                ldy $1d1e, x	; bc
		                lda $1d1e, x	; bd
		                ldx $1919, y	; be
		                cpy #$09	; c0
		                cmp ($01, x)	; c1
		                cpy $05		; c4
		                cmp $05		; c5
		                dec $05		; c6
		                iny		; c8
		                cmp #$09	; c9
		                dex		; ca
		                cpy $0d0e	; cc
		                cmp $0d0e	; cd
		                dec $0d0e	; ce
		                bne * + 2	; d0
		                cmp ($11), y	; d1
		                cmp $15, x	; d5
		                dec $15, x	; d6
		                cld		; d8
		                cmp $1919, y	; d9
		                cmp $1d1e, x	; dd
		                dec $1d1e, x	; de
		                cpx #$09	; e0
		                sbc ($01, x)	; e1
		                cpx $05		; e4
		                sbc $05		; e5
		                inc $05		; e6
		                inx		; e8
		                sbc #$09	; e9
	                !ifndef M65 {
		                nop		; ea	(m65 re-uses this opcode as a prefix)
	                }
		                cpx $0d0e	; ec
		                sbc $0d0e	; ed
		                inc $0d0e	; ee
		                beq * + 2	; f0
		                sbc ($11), y	; f1
		                sbc $15, x	; f5
		                inc $15, x	; f6
		                sed		; f8
		                sbc $1919, y	; f9
		                sbc $1d1e, x	; fd
		                inc $1d1e, x	; fe

	                  ; extensions in CMOS re-design (65c02):
		                tsb $04		; 04
		                tsb $0c0c	; 0c
		                ora ($12)	; 12
		                trb $04		; 14
		                inc		; 1a
		                trb $0c0c	; 1c
		                and ($12)	; 32
		                bit $34, x	; 34
		                dec		; 3a
		                bit $3c3c, x	; 3c
		                eor ($12)	; 52
		                phy		; 5a
		                stz $04		; 64
		                adc ($12)	; 72
		                stz $34, x	; 74
		                ply		; 7a
		                jmp ($7c7c, x)	; 7c
		                bra * + 2	; 80
		                bit #$89	; 89
		                sta ($12)	; 92
		                stz $0c0c	; 9c
		                stz $3c3c, x	; 9e
		                lda ($12)	; b2
		                cmp ($12)	; d2
		                phx		; da
		                sbc ($12)	; f2
		                plx		; fa

	                  ; new instructions of 65816:
		                cop $02		; 02
		                ora $03, s	; 03
		                ora [$07]	; 07
		                phd		; 0b
		                ora $0f0f0f	; 0f
		                ora ($13, s), y	; 13
		                ora [$17], y	; 17
		                tcs		; 1b
		                ora $1f1f1f, x	; 1f
		                jsr $0f0f0f	; 22
		                and $03, s	; 23
		                and [$07]	; 27
		                pld		; 2b
		                and $0f0f0f	; 2f
		                and ($13, s), y	; 33
		                and [$17], y	; 37
		                tsc		; 3b
		                and $1f1f1f, x	; 3f
		                wdm		; 42
		                eor $03, s	; 43
		                mvp $44, $54	; 44
		                eor [$07]	; 47
		                phk		; 4b
		                eor $0f0f0f	; 4f
		                eor ($13, s), y	; 53
		                mvn $44, $54	; 54
		                eor [$17], y	; 57
		                tcd		; 5b
		                jmp $0f0f0f	; 5c
		                eor $1f1f1f, x	; 5f
		                per * + 3	; 62
		                adc $03, s	; 63
		                adc [$07]	; 67
		                rtl		; 6b
		                adc $0f0f0f	; 6f
		                adc ($13, s), y	; 73
		                adc [$17], y	; 77
		                tdc		; 7b
		                adc $1f1f1f, x	; 7f
		                brl * + 3	; 82
		                sta $03, s	; 83
		                sta [$07]	; 87
		                phb		; 8b
		                sta $0f0f0f	; 8f
		                sta ($13, s), y	; 93
		                sta [$17], y	; 97
		                txy		; 9b
		                sta $1f1f1f, x	; 9f
		                lda $03, s	; a3
		                lda [$07]	; a7
		                plb		; ab
		                lda $0f0f0f	; af
		                lda ($13, s), y	; b3
		                lda [$17], y	; b7
		                tyx		; bb
		                lda $1f1f1f, x	; bf
		                rep #$c2	; c2, see below
		                cmp $03, s	; c3
		                cmp [$07]	; c7
		                wai		; cb
		                cmp $0f0f0f	; cf
		                cmp ($13, s), y	; d3
		                pei ($d4)	; d4
		                cmp [$17], y	; d7
		                stp		; db
		                jmp [$dcdc]	; dc
		                cmp $1f1f1f, x	; df
		                sep #$e2	; e2, see below
		                sbc $03, s	; e3
		                sbc [$07]	; e7
		                xba		; eb
		                sbc $0f0f0f	; ef
		                sbc ($13, s), y	; f3
		                pea $f4f4	; f4
		                sbc [$17], y	; f7
		                xce		; fb
		                jsr ($fcfc, x)	; fc
		                sbc $1f1f1f, x	; ff

                ; check sizes of immediate arguments:
                !macro immediates {
				                ;	arg size depends on:
                ; from 6502:
		                ora #$09	; 09	accumulator size
		                and #$09	; 29	accumulator size
		                eor #$09	; 49	accumulator size
		                adc #$09	; 69	accumulator size
		                ldy #$09	; a0	index register size
		                ldx #$09	; a2	index register size
		                lda #$09	; a9	accumulator size
		                cpy #$09	; c0	index register size
		                cmp #$09	; c9	accumulator size
		                cpx #$09	; e0	index register size
		                sbc #$09	; e9	accumulator size
                ; from 65c02:
		                bit #$89	; 89	accumulator size
                ; from 65816:
		                rep #$c2	; c2	always 8 bits
		                sep #$e2	; e2	always 8 bits
                }
		                ; before this, all sizes were 8 bits
		                !al
		                +immediates	; now repeat immediates with long accumulator
		                !as
		                !rl
		                +immediates	; repeat immediates with short A and long index regs
		                !al
		                !rl
		                +immediates	; repeat immediates with long A and long index regs ";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.bin";
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PLAIN;

      Assert.IsTrue( parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo ) );
      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( "000101050506050809090A0D0E0D0E0E0D1000111115151615181919191D1E1D1E1E1D200E0D21012405250526052829092A2C0E0D2D0E0D2E0E0D3000311135153615383919193D1E1D3E1E1D404101450546054849094A4C0E0D4D0E0D4E0E0D5000511155155615585919195D1E1D5E1E1D606101650566056869096A6C6C6C6D0E0D6E0E0D7000711175157615787919197D1E1D7E1E1D8101840585058605888A8C0E0D8D0E0D8E0E0D90009111941595159696989919199A9D1E1DA009A101A209A405A505A605A8A909AAAC0E0DAD0E0DAE0E0DB000B111B415B515B696B8B91919BABC1E1DBD1E1DBE1919C009C101C405C505C605C8C909CACC0E0DCD0E0DCE0E0DD000D111D515D615D8D91919DD1E1DDE1E1DE009E101E405E505E605E8E909EAEC0E0DED0E0DEE0E0DF000F111F515F615F8F91919FD1E1DFE1E1D04040C0C0C121214041A1C0C0C321234343A3C3C3C52125A6404721274347A7C7C7C8000898992129C0C0C9E3C3CB212D212DAF212FA0202030307070B0F0F0F0F131317171B1F1F1F1F220F0F0F230327072B2F0F0F0F331337173B3F1F1F1F42430344544447074B4F0F0F0F531354544457175B5C0F0F0F5F1F1F1F620000630367076B6F0F0F0F731377177B7F1F1F1F820000830387078B8F0F0F0F931397179B9F1F1F1FA303A707ABAF0F0F0FB313B717BBBF1F1F1FC2C2C303C707CBCF0F0F0FD313D4D4D717DBDCDCDCDF1F1F1FE2E2E303E707EBEF0F0F0FF313F4F4F4F717FBFCFCFCFF1F1F1F090900290900490900690900A009A209A90900C009C90900E009E90900898900C2C2E2E20909290949096909A00900A20900A909C00900C909E00900E9098989C2C2E2E2090900290900490900690900A00900A20900A90900C00900C90900E00900E90900898900C2C2E2E2", assembly.Assembly.ToString() );
    }



    [TestMethod]
    public void TestAssembly4502Opcodes()
    {
      string      source = @"!cpu 4502

                        * = $1000

                        adc $1234
                        adc $1234,x
                        adc $1234,y
                        adc $12
                        adc $12,x
                        adc($12 ),y
                        adc($12, x )
                        adc #$12

                        and $1234
                        and $1234,x
                        and $1234,y
                        and $12
                        and $12,x
                        and($12 ),y
                        and($12, x )
                        and #$12

                        asl
                        asl $1234
                        asl $1234,x
                        asl $12
                        asl $12,x

                        bcc $1034
                        bcs $1034
                        beq $1034
                        bit $1234
                        bit $12

                        bmi $1034
                        bne $1034
                        bpl $1034
                        brk

                        bvc $1034
                        bvs $1034

                        clc
                        cld
                        cli
                        clv

                        cmp $1234
                        cmp $1234,x
                        cmp $1234,y
                        cmp $12
                        cmp $12,x
                        cmp($12 ),y
                        cmp($12, x )
                        cmp #$12

                        cpx $1234

                        cpx $1234
                        cpx $12
                        cpx #$12

                        cpy $1234
                        cpy $12
                        cpy #$12

                        dec $1234
                        dec $1234,x
                        dec $12
                        dec $12,x

                        dex
                        dey

                        eor $1234
                        eor $1234,x
                        eor $1234,y
                        eor $12
                        eor $12,x
                        eor($12 ),y
                        eor($12, x )
                        eor #$12

                        inc $1234
                        inc $1234,x
                        inc $12
                        inc $12,x

                        inx
                        iny

                        jmp $1234
                        jmp($1234 )
                        jsr $1234

                        lda $1234
                        lda $1234,x
                        lda $1234,y
                        lda $12
                        lda $12,x
                        lda($12 ),y
                        lda($12, x )
                        lda #$12

                        ldx $1234
                        ldx $1234,y
                        ldx $12
                        ldx $12,y
                        ldx #$12

                        ldy $1234
                        ldy $1234,x
                        ldy $12
                        ldy $12,x
                        ldy #$12

                        lsr
                        lsr $1234
                        lsr $1234,x
                        lsr $12
                        lsr $12,x

                        map
                        eom

                        ora $1234
                        ora $1234,x
                        ora $1234,y
                        ora $12
                        ora $12,x
                        ora($12 ),y
                        ora($12, x )
                        ora #$12

                        pha
                        php
                        pla
                        plp

                        rol
                        rol $1234
                        rol $1234,x
                        rol $12
                        rol $12,x
                        ror
                        ror $1234
                        ror $1234,x
                        ror $12
                        ror $12,x

                        rti
                        rts

                        sbc $1234
                        sbc $1234,x
                        sbc $1234,y
                        sbc $12
                        sbc $12,x
                        sbc($12 ),y
                        sbc($12, x )
                        sbc #$12

                        sec
                        sed
                        sei

                        sta $1234
                        sta $1234,x
                        sta $1234,y
                        sta $12
                        sta $12,x
                        sta($12 ),y
                        sta($12, x )

                        stx $1234
                        stx $12
                        stx $12,y
                        sty $1234
                        sty $12
                        sty $12,x

                        tax
                        tay
                        tsx
                        txa
                        txs
                        tya

                        adc($12 ),z
                        and($12 ),z
                        cmp($12 ),z
                        eor($12 ),z
                        lda($12 ),z
                        ora($12 ),z
                        sbc($12 ),z
                        sta($12 ),z

                        bit #$12
                        bit $12,x
                        bit $1234,x

                        dec
                        inc

                        jmp($1234, x )

                        bra $1134

                        phx
                        phy
                        plx
                        ply

                        stz $12
                        stz $12,x
                        stz $1234
                        stz $1234,x

                        trb $12
                        trb $1234

                        tsb $12
                        tsb $1234

                        bbr0 $12,$1134
                        bbr1 $12,$1134
                        bbr2 $12,$1134
                        bbr3 $12,$1134
                        bbr4 $12,$1134
                        bbr5 $12,$1134
                        bbr6 $12,$1134
                        bbr7 $12,$1134
                        bbs0 $12,$1134
                        bbs1 $12,$1134
                        bbs2 $12,$1134
                        bbs3 $12,$1134
                        bbs4 $12,$1134
                        bbs5 $12,$1134
                        bbs6 $12,$1134
                        bbs7 $12,$1134

                        rmb0 $12
                        rmb1 $12
                        rmb2 $12
                        rmb3 $12
                        rmb4 $12
                        rmb5 $12
                        rmb6 $12
                        rmb7 $12
                        smb0 $12
                        smb1 $12
                        smb2 $12
                        smb3 $12
                        smb4 $12
                        smb5 $12
                        smb6 $12
                        smb7 $12

                        inz
                        dez
                        taz
                        tza
                        ldz #$12
                        ldz $1234
                        ldz $1234,x
                        cpz #$12
                        cpz $12
                        phz
                        cpz $1234
                        plz

                        cle
                        see
                        tsy
                        tys

                        lbpl $1234
                        lbmi $1234
                        lbvc $1234
                        bsr $1234
                        lbvs $1234
                        lbra $1234
                        lbcc $1234
                        lbcs $1234
                        lbne $1234
                        lbeq $1234      ;F3 33 00
  
                        jsr($1234 )     ;$22 34 12
                        jsr($1234, x )  ;$23 34 12
                        sta($12, s ),y  ;$82 12
                        sty $1234,x     ;$8B 34 12
                        stx $1234,y     ;$9B 34 12
                        lda($12, s ),y  ;$E2 12

                        neg             ;$42
                        asr             ;$43
                        asr $12         ;$44 12
                        asr $12,x       ;$54 12
                        tab             ;$5B
                        rtn #$12        ;$62 12
                        tba             ;$7B
                        dew $12         ;$C3 12
                        asw $1234       ;$CB 34 12
                        inw $12         ;$E3 12
                        row $1234       ;$EB 34 12
                        phw #$1234      ;$F4 34 12
                        phw $1234       ;$FC 34 12";

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "00106D34127D3412793412651275127112611269122D34123D3412393412251235123112211229120A0E34121E3412061216129001B0FFF0FD2C3412241230F6D0F410F20050EF70ED18D858B8CD3412DD3412D93412C512D512D112C112C912EC3412EC3412E412E012CC3412C412C012CE3412DE3412C612D612CA884D34125D341259341245125512511241124912EE3412FE3412E612F612E8C84C34126C3412203412AD3412BD3412B93412A512B512B112A112A912AE3412BE3412A612B612A212AC3412BC3412A412B412A0124A4E34125E3412461256125CEA0D34121D341219341205121512111201120912480868282A2E34123E3412261236126A6E34127E3412661276124060ED3412FD3412F93412E512F512F112E112E91238F8788D34129D341299341285129512911281128E3412861296128C341284129412AAA8BA8A9A9872123212D2125212B2121212F2129212891234123C34123A1A7C341280D1DA5AFA7A641274129C34129E341214121C341204120C34120F12B61F12B32F12B03F12AD4F12AA5F12A76F12A47F12A18F129E9F129BAF1298BF1295CF1292DF128FEF128CFF12890712171227123712471257126712771287129712A712B712C712D712E712F7121B3B4B6BA312AB3412BB3412C212D412DBDC3412FB02030B2B134E00334B00534800634500734200833F00933C00B33900D33600F3330022341223341282128B34129B3412E2124243441254125B62127BC312CB3412E312EB3412F43412FC3412", assembly.ToString() );
    }



    [TestMethod]
    public void TestAssemblyM65Opcodes()
    {
      string      source = @"!cpu m65

                            !to ""cpu_m65.prg"",plain

                            * = $1000

                            adc $1234
                            adc $1234,x
                            adc $1234,y
                            adc $12
                            adc $12,x
                            adc($12 ),y
                            adc($12, x )
                            adc #$12

                            and $1234
                            and $1234,x
                            and $1234,y
                            and $12
                            and $12,x
                            and($12 ),y
                            and($12, x )
                            and #$12

                            asl
                            asl $1234
                            asl $1234,x
                            asl $12
                            asl $12,x

                            bcc $1034
                            bcs $1034
                            beq $1034
                            bit $1234
                            bit $12

                            bmi $1034
                            bne $1034
                            bpl $1034
                            brk

                            bvc $1034
                            bvs $1034

                            clc
                            cld
                            cli
                            clv

                            cmp $1234
                            cmp $1234,x
                            cmp $1234,y
                            cmp $12
                            cmp $12,x
                            cmp($12 ),y
                            cmp($12, x )
                            cmp #$12

                            cpx $1234

                            cpx $1234
                            cpx $12
                            cpx #$12

                            cpy $1234
                            cpy $12
                            cpy #$12

                            dec $1234
                            dec $1234,x
                            dec $12
                            dec $12,x

                            dex
                            dey

                            eor $1234
                            eor $1234,x
                            eor $1234,y
                            eor $12
                            eor $12,x
                            eor($12 ),y
                            eor($12, x )
                            eor #$12

                            inc $1234
                            inc $1234,x
                            inc $12
                            inc $12,x

                            inx
                            iny

                            jmp $1234
                            jmp($1234 )
                            jsr $1234

                            lda $1234
                            lda $1234,x
                            lda $1234,y
                            lda $12
                            lda $12,x
                            lda($12 ),y
                            lda($12, x )
                            lda #$12

                            ldx $1234
                            ldx $1234,y
                            ldx $12
                            ldx $12,y
                            ldx #$12

                            ldy $1234
                            ldy $1234,x
                            ldy $12
                            ldy $12,x
                            ldy #$12

                            lsr
                            lsr $1234
                            lsr $1234,x
                            lsr $12
                            lsr $12,x

                            map
                            eom

                            ora $1234
                            ora $1234,x
                            ora $1234,y
                            ora $12
                            ora $12,x
                            ora($12 ),y
                            ora($12, x )
                            ora #$12

                            pha
                            php
                            pla
                            plp

                            rol
                            rol $1234
                            rol $1234,x
                            rol $12
                            rol $12,x
                            ror
                            ror $1234
                            ror $1234,x
                            ror $12
                            ror $12,x

                            rti
                            rts

                            sbc $1234
                            sbc $1234,x
                            sbc $1234,y
                            sbc $12
                            sbc $12,x
                            sbc($12 ),y
                            sbc($12, x )
                            sbc #$12

                            sec
                            sed
                            sei

                            sta $1234
                            sta $1234,x
                            sta $1234,y
                            sta $12
                            sta $12,x
                            sta($12 ),y
                            sta($12, x )

                            stx $1234
                            stx $12
                            stx $12,y
                            sty $1234
                            sty $12
                            sty $12,x

                            tax
                            tay
                            tsx
                            txa
                            txs
                            tya

                            adc($12 ),z
                            and($12 ),z
                            cmp($12 ),z
                            eor($12 ),z
                            lda($12 ),z
                            ora($12 ),z
                            sbc($12 ),z
                            sta($12 ),z

                            bit #$12
                            bit $12,x
                            bit $1234,x

                            dec
                            inc

                            jmp($1234, x )

                            bra $1134

                            phx
                            phy
                            plx
                            ply

                            stz $12
                            stz $12,x
                            stz $1234
                            stz $1234,x

                            trb $12
                            trb $1234

                            tsb $12
                            tsb $1234

                            bbr0 $12,$1134
                            bbr1 $12,$1134
                            bbr2 $12,$1134
                            bbr3 $12,$1134
                            bbr4 $12,$1134
                            bbr5 $12,$1134
                            bbr6 $12,$1134
                            bbr7 $12,$1134
                            bbs0 $12,$1134
                            bbs1 $12,$1134
                            bbs2 $12,$1134
                            bbs3 $12,$1134
                            bbs4 $12,$1134
                            bbs5 $12,$1134
                            bbs6 $12,$1134
                            bbs7 $12,$1134

                            rmb0 $12
                            rmb1 $12
                            rmb2 $12
                            rmb3 $12
                            rmb4 $12
                            rmb5 $12
                            rmb6 $12
                            rmb7 $12
                            smb0 $12
                            smb1 $12
                            smb2 $12
                            smb3 $12
                            smb4 $12
                            smb5 $12
                            smb6 $12
                            smb7 $12

                            inz
                            dez
                            taz
                            tza
                            ldz #$12
                            ldz $1234
                            ldz $1234,x
                            cpz #$12
                            cpz $12
                            phz
                            cpz $1234
                            plz

                            cle
                            see
                            tsy
                            tys

                            lbpl $1234
                            lbmi $1234
                            lbvc $1234
                            bsr $1234
                            lbvs $1234
                            lbra $1234
                            lbcc $1234
                            lbcs $1234
                            lbne $1234
                            lbeq $1234

                            jsr($1234 )
                            jsr($1234, x )
                            sta($12, s ),y
                             sty $1234,x
                             stx $1234,y
                            lda($12, s ),y

                             neg
                            asr
                            asr $12
                            asr $12,x
                            tab
                            rtn #$12
                            tba
                            dew $12
                            asw $1234
                            inw $12
                            row $1234
                            phw #$1234
                            phw $1234

                            ;extensions
                            orq $12
                            aslq $12
                            aslq
                            orq $1234
                            aslq $1234
                            orq ($12)
                            aslq $12,x
                            inq
                            aslq $1234,x
                            bitq $12
                            andq $12
                            rolq $12
                            rolq
                            bitq $1234
                            andq $1234
                            rolq $1234
                            andq ($12)
                            rolq $12,x
                            deq
                            rolq $1234,x

                            asrq
                            asrq $12
                            eorq $12
                            lsrq $12
                            lsrq
                            eorq $1234
                            lsrq $1234
                            eorq ($12)
                            asrq $12,x
                            lsrq $12,x
                            lsrq $1234,x

                            adcq $12
                            rorq $12
                            rorq
                            adcq $1234
                            rorq $1234
                            adcq ($12)
                            rorq $12,x

                            stq $12
                            stq $1234
                            stq ($12)

                            ldq $12
                            ldq $1234
                            ldq ($12),y
                            ldq ($12)
                            ldq $12,x
                            ldq $1234,y
                            ldq $1234,x

                            cpq $12
                            deq $12
                            cpq $1234
                            deq $1234
                            cpq ($12)
                            deq $12,x
                            deq $1234,x

                            ldq ($12,s),y
                            sbcq $12
                            inq $12
                            sbcq $1234
                            inq $1234
                            sbcq ($12)
                            inq $12,x
                            inq $1234,x

                            ora [$12],z
                            and [$12],z
                            eor [$12],z
                            adc [$12],z
                            sta [$12],z
                            lda [$12],z
                            cmp [$12],z
                            sbc [$12],z

                            orq [$12]
                            andq [$12]
                            eorq [$12]
                            adcq [$12]
                            stq [$12]
                            ldq [$12]
                            cpq [$12]
                            sbcq [$12]
                            ";

      RetroDevStudio.Parser.ASMFileParser      parser = new RetroDevStudio.Parser.ASMFileParser();
      parser.SetAssemblerType( RetroDevStudio.Types.AssemblerType.C64_STUDIO );

      RetroDevStudio.Parser.CompileConfig config = new RetroDevStudio.Parser.CompileConfig();
      config.OutputFile = "test.bin";
      config.Assembler = RetroDevStudio.Types.AssemblerType.C64_STUDIO;
      config.TargetType = RetroDevStudio.Types.CompileTargetType.PLAIN;

      bool parseResult = parser.Parse( source, null, config, null, out RetroDevStudio.Types.ASM.FileInfo asmFileInfo );
      if ( !parseResult )
      {
        Debug.Log( "Testassemble failed:" );
        foreach ( var msg in asmFileInfo.Messages.Values )
        {
          Debug.Log( msg.Message );
        }
      }

      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( "6D34127D3412793412651275127112611269122D34123D3412393412251235123112211229120A0E34121E3412061216129001B0FFF0FD2C3412241230F6D0F410F20050EF70ED18D858B8CD3412DD3412D93412C512D512D112C112C912EC3412EC3412E412E012CC3412C412C012CE3412DE3412C612D612CA884D34125D341259341245125512511241124912EE3412FE3412E612F612E8C84C34126C3412203412AD3412BD3412B93412A512B512B112A112A912AE3412BE3412A612B612A212AC3412BC3412A412B412A0124A4E34125E3412461256125CEA0D34121D341219341205121512111201120912480868282A2E34123E3412261236126A6E34127E3412661276124060ED3412FD3412F93412E512F512F112E112E91238F8788D34129D341299341285129512911281128E3412861296128C341284129412AAA8BA8A9A9872123212D2125212B2121212F2129212891234123C34123A1A7C341280D1DA5AFA7A641274129C34129E341214121C341204120C34120F12B61F12B32F12B03F12AD4F12AA5F12A76F12A47F12A18F129E9F129BAF1298BF1295CF1292DF128FEF128CFF12890712171227123712471257126712771287129712A712B712C712D712E712F7121B3B4B6BA312AB3412BB3412C212D412DBDC3412FB02030B2B134E00334B00534800634500734200833F00933C00B33900D33600F3330022341223341282128B34129B3412E2124243441254125B62127BC312CB3412E312EB3412F43412FC3412424205124242061242420A42420D341242420E3412424212124242161242421A42421E341242422412424225124242261242422A42422C341242422D341242422E3412424232124242361242423A42423E341242424342424412424245124242461242424A42424D341242424E341242425212424254124242561242425E3412424265124242661242426A42426D341242426E341242427212424276124242851242428D3412424292124242A5124242AD34124242B1124242B2124242B5124242B934124242BD34124242C5124242C6124242CD34124242CE34124242D2124242D6124242DE34124242E2124242E5124242E6124242ED34124242EE34124242F2124242F6124242FE3412EA1212EA3212EA5212EA7212EA9212EAB212EAD212EAF2124242EA12124242EA32124242EA52124242EA72124242EA92124242EAB2124242EAD2124242EAF212", assembly.Assembly.ToString() );
    }

  }
}
