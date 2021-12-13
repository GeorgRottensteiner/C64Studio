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

      C64Studio.Parser.ASMFileParser      parser = new C64Studio.Parser.ASMFileParser();
      parser.SetAssemblerType( C64Studio.Types.AssemblerType.C64_STUDIO );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.bin";
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;
      config.TargetType = C64Studio.Types.CompileTargetType.PLAIN;

      Assert.IsTrue( parser.Parse( source, null, config, null ) );
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

      C64Studio.Parser.ASMFileParser      parser = new C64Studio.Parser.ASMFileParser();
      parser.SetAssemblerType( C64Studio.Types.AssemblerType.C64_STUDIO );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.bin";
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;
      config.TargetType = C64Studio.Types.CompileTargetType.PLAIN;

      Assert.IsTrue( parser.Parse( source, null, config, null ) );
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

      C64Studio.Parser.ASMFileParser      parser = new C64Studio.Parser.ASMFileParser();
      parser.SetAssemblerType( C64Studio.Types.AssemblerType.C64_STUDIO );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.bin";
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;
      config.TargetType = C64Studio.Types.CompileTargetType.PLAIN;

      Assert.IsTrue( parser.Parse( source, null, config, null ) );
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

      C64Studio.Parser.ASMFileParser      parser = new C64Studio.Parser.ASMFileParser();
      parser.SetAssemblerType( C64Studio.Types.AssemblerType.C64_STUDIO );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.bin";
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;
      config.TargetType = C64Studio.Types.CompileTargetType.PLAIN;

      Assert.IsTrue( parser.Parse( source, null, config, null ) );
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

      C64Studio.Parser.ASMFileParser      parser = new C64Studio.Parser.ASMFileParser();
      parser.SetAssemblerType( C64Studio.Types.AssemblerType.C64_STUDIO );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.bin";
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;
      config.TargetType = C64Studio.Types.CompileTargetType.PLAIN;

      Assert.IsTrue( parser.Parse( source, null, config, null ) );
      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( "6D34127D3412793412651275127112611269122D34123D3412393412251235123112211229120A0E34121E3412061216129001B0FFF0FD2C3412241230F6D0F410F20050EF70ED18D858B8CD3412DD3412D93412C512D512D112C112C912EC3412EC3412E412E012CC3412C412C012CE3412DE3412C612D612CA884D34125D341259341245125512511241124912EE3412FE3412E612F612E8C84C34126C3412203412AD3412BD3412B93412A512B512B112A112A912AE3412BE3412A612B612A212AC3412BC3412A412B412A0124A4E34125E341246125612EA0D34121D341219341205121512111201120912480868282A2E34123E3412261236126A6E34127E3412661276124060ED3412FD3412F93412E512F512F112E112E91238F8788D34129D341299341285129512911281128E3412861296128C341284129412AAA8BA8A9A9872123212D2125212B2121212F2129212891234123C34123A1A7C341280D2DA5AFA7A641274129C34129E341214121C341204120C34120F12B71F12B42F12B13F12AE4F12AB5F12A86F12A57F12A28F129F9F129CAF1299BF1296CF1293DF1290EF128DFF128A0712171227123712471257126712771287129712A712B712C712D712E712F7121B3B4B6BA312AB3412BB3412C212D412DBDC3412FB02030B2B134F00334C00534900634600734300834000933D00B33A00D33700F3340022341223341282128B34129B3412E2124243441254125B5C62127BC312CB3412E312EB3412F43412FC3412", assembly.Assembly.ToString() );
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
                              phw $1234";

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

      C64Studio.Parser.ASMFileParser      parser = new C64Studio.Parser.ASMFileParser();
      parser.SetAssemblerType( C64Studio.Types.AssemblerType.C64_STUDIO );

      C64Studio.Parser.CompileConfig config = new C64Studio.Parser.CompileConfig();
      config.OutputFile = "test.bin";
      config.Assembler = C64Studio.Types.AssemblerType.C64_STUDIO;
      config.TargetType = C64Studio.Types.CompileTargetType.PLAIN;

      Assert.IsTrue( parser.Parse( source, null, config, null ) );
      Assert.IsTrue( parser.Assemble( config ) );

      var assembly = parser.AssembledOutput;

      Assert.AreEqual( "6D34127D3412793412651275127112611269122D34123D3412393412251235123112211229120A0E34121E3412061216129001B0FFF0FD2C3412241230F6D0F410F20050EF70ED18D858B8CD3412DD3412D93412C512D512D112C112C912EC3412EC3412E412E012CC3412C412C012CE3412DE3412C612D612CA884D34125D341259341245125512511241124912EE3412FE3412E612F612E8C84C34126C3412203412AD3412BD3412B93412A512B512B112A112A912AE3412BE3412A612B612A212AC3412BC3412A412B412A0124A4E34125E3412461256125CEA0D34121D341219341205121512111201120912480868282A2E34123E3412261236126A6E34127E3412661276124060ED3412FD3412F93412E512F512F112E112E91238F8788D34129D341299341285129512911281128E3412861296128C341284129412AAA8BA8A9A9872123212D2125212B2121212F2129212891234123C34123A1A7C341280D1DA5AFA7A641274129C34129E341214121C341204120C34120F12B61F12B32F12B03F12AD4F12AA5F12A76F12A47F12A18F129E9F129BAF1298BF1295CF1292DF128FEF128CFF12890712171227123712471257126712771287129712A712B712C712D712E712F7121B3B4B6BA312AB3412BB3412C212D412DBDC3412FB02030B2B134E00334B00534800634500734200833F00933C00B33900D33600F3330022341223341282128B34129B3412E2124243441254125B62127BC312CB3412E312EB3412F43412FC3412424205124242061242420A42420D341242420E3412424212124242161242421A42421E341242422412424225124242261242422A42422C341242422D341242422E3412424232124242361242423A42423E341242424342424412424245124242461242424A42424D341242424E341242425212424254124242561242425E3412424265124242661242426A42426D341242426E341242427212424276124242851242428D3412424292124242A5124242AD34124242B1124242B2124242B5124242B934124242BD34124242C5124242C6124242CD34124242CE34124242D2124242D6124242DE34124242E2124242E5124242E6124242ED34124242EE34124242F2124242F6124242FE3412EA1212EA3212EA5212EA7212EA9212EAB212EAD212EAF2124242EA12124242EA32124242EA52124242EA72124242EA92124242EAB2124242EAD2124242EAF212", assembly.Assembly.ToString() );
    }

  }
}
