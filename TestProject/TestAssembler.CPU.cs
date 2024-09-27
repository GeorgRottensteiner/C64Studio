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

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "6D34127D3412793412651275127112611269122D34123D3412393412251235123112211229120A0E34121E3412061216129001B0FFF0FD2C3412241230F6D0F410F20050EF70ED18D858B8CD3412DD3412D93412C512D512D112C112C912EC3412EC3412E412E012CC3412C412C012CE3412DE3412C612D612CA884D34125D341259341245125512511241124912EE3412FE3412E612F612E8C84C34126C3412203412AD3412BD3412B93412A512B512B112A112A912AE3412BE3412A612B612A212AC3412BC3412A412B412A0124A4E34125E341246125612EA0D34121D341219341205121512111201120912480868282A2E34123E3412261236126A6E34127E3412661276124060ED3412FD3412F93412E512F512F112E112E91238F8788D34129D341299341285129512911281128E3412861296128C341284129412AAA8BA8A9A980207121712031213120F34121F34121B341227123712231233122F34123F34123B341247125712431253124F34125F34125B341267127712631273126F34127F34127B34128712971283128F3412A712B712A312B312AF3412BF3412C712D712C312D312CF3412DF3412DB3412E712F712E312F312EF3412FF3412FB34120B128B12AB12CB1293129F34129C34129E34129B3412BB3412CB12801204120C34121C3412", assembly.ToString() );
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

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "6D34127D3412793412651275127112611269122D34123D3412393412251235123112211229120A0E34121E3412061216129001B0FFF0FD2C3412241230F6D0F410F20050EF70ED18D858B8CD3412DD3412D93412C512D512D112C112C912EC3412EC3412E412E012CC3412C412C012CE3412DE3412C612D612CA884D34125D341259341245125512511241124912EE3412FE3412E612F612E8C84C34126C3412203412AD3412BD3412B93412A512B512B112A112A912AE3412BE3412A612B612A212AC3412BC3412A412B412A0124A4E34125E341246125612EA0D34121D341219341205121512111201120912480868282A2E34123E3412261236126A6E34127E3412661276124060ED3412FD3412F93412E512F512F112E112E91238F8788D34129D341299341285129512911281128E3412861296128C341284129412AAA8BA8A9A9872123212D2125212B2121212F2129212891234123C34123A1A7C341280D2DA5AFA7A641274129C34129E341214121C341204120C3412", assembly.ToString() );
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

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "6D34127D3412793412651275127112611269122D34123D3412393412251235123112211229120A0E34121E3412061216129001B0FFF0FD2C3412241230F6D0F410F20050EF70ED18D858B8CD3412DD3412D93412C512D512D112C112C912EC3412EC3412E412E012CC3412C412C012CE3412DE3412C612D612CA884D34125D341259341245125512511241124912EE3412FE3412E612F612E8C84C34126C3412203412AD3412BD3412B93412A512B512B112A112A912AE3412BE3412A612B612A212AC3412BC3412A412B412A0124A4E34125E341246125612EA0D34121D341219341205121512111201120912480868282A2E34123E3412261236126A6E34127E3412661276124060ED3412FD3412F93412E512F512F112E112E91238F8788D34129D341299341285129512911281128E3412861296128C341284129412AAA8BA8A9A9872123212D2125212B2121212F2129212891234123C34123A1A7C341280D2DA5AFA7A641274129C34129E341214121C341204120C34120F12B71F12B42F12B13F12AE4F12AB5F12A86F12A57F12A28F129F9F129CAF1299BF1296CF1293DF1290EF128DFF128A0712171227123712471257126712771287129712A712B712C712D712E712F712", assembly.ToString() );
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

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "6D34127D3412793412651275127112611269122D34123D3412393412251235123112211229120A0E34121E3412061216129001B0FFF0FD2C3412241230F6D0F410F20050EF70ED18D858B8CD3412DD3412D93412C512D512D112C112C912EC3412EC3412E412E012CC3412C412C012CE3412DE3412C612D612CA884D34125D341259341245125512511241124912EE3412FE3412E612F612E8C84C34126C3412203412AD3412BD3412B93412A512B512B112A112A912AE3412BE3412A612B612A212AC3412BC3412A412B412A0124A4E34125E341246125612EA0D34121D341219341205121512111201120912480868282A2E34123E3412261236126A6E34127E3412661276124060ED3412FD3412F93412E512F512F112E112E91238F8788D34129D341299341285129512911281128E3412861296128C341284129412AAA8BA8A9A9872123212D2125212B2121212F2129212891234123C34123A1A7C341280D2DA5AFA7A641274129C34129E341214121C341204120C34120F12B71F12B42F12B13F12AE4F12AB5F12A86F12A57F12A28F129F9F129CAF1299BF1296CF1293DF1290EF128DFF128A0712171227123712471257126712771287129712A712B712C712D712E712F712DBCB", assembly.ToString() );
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

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "6D34127D3412793412651275127112611269122D34123D3412393412251235123112211229120A0E34121E3412061216129001B0FFF0FD2C3412241230F6D0F410F20050EF70ED18D858B8CD3412DD3412D93412C512D512D112C112C912EC3412EC3412E412E012CC3412C412C012CE3412DE3412C612D612CA884D34125D341259341245125512511241124912EE3412FE3412E612F612E8C84C34126C3412203412AD3412BD3412B93412A512B512B112A112A912AE3412BE3412A612B612A212AC3412BC3412A412B412A0124A4E34125E341246125612EA0D34121D341219341205121512111201120912480868282A2E34123E3412261236126A6E34127E3412661276124060ED3412FD3412F93412E512F512F112E112E91238F8788D34129D341299341285129512911281128E3412861296128C341284129412AAA8BA8A9A9872123212D2125212B2121212F2129212891234123C34123A1A7C341280D2DA5AFA7A641274129C34129E341214121C341204120C34120F12B71F12B42F12B13F12AE4F12AB5F12A86F12A57F12A28F129F9F129CAF1299BF1296CF1293DF1290EF128DFF128A0712171227123712471257126712771287129712A712B712C712D712E712F7121B3B4B6BA312AB3412BB3412C212D412DBDC3412FB02030B2B134F00334C00534900634600734300834000933D00B33A00D33700F3340022341223341282128B34129B3412E2124243441254125B5C62127BC312CB3412E312EB3412F43412FC3412", assembly.ToString() );
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

      var assembly = TestAssembleC64Studio( source );

      Assert.AreEqual( "000101050506050809090A0D0E0D0E0E0D1000111115151615181919191D1E1D1E1E1D200E0D21012405250526052829092A2C0E0D2D0E0D2E0E0D3000311135153615383919193D1E1D3E1E1D404101450546054849094A4C0E0D4D0E0D4E0E0D5000511155155615585919195D1E1D5E1E1D606101650566056869096A6C6C6C6D0E0D6E0E0D7000711175157615787919197D1E1D7E1E1D8101840585058605888A8C0E0D8D0E0D8E0E0D90009111941595159696989919199A9D1E1DA009A101A209A405A505A605A8A909AAAC0E0DAD0E0DAE0E0DB000B111B415B515B696B8B91919BABC1E1DBD1E1DBE1919C009C101C405C505C605C8C909CACC0E0DCD0E0DCE0E0DD000D111D515D615D8D91919DD1E1DDE1E1DE009E101E405E505E605E8E909EAEC0E0DED0E0DEE0E0DF000F111F515F615F8F91919FD1E1DFE1E1D04040C0C0C121214041A1C0C0C321234343A3C3C3C52125A6404721274347A7C7C7C8000898992129C0C0C9E3C3CB212D212DAF212FA0202030307070B0F0F0F0F131317171B1F1F1F1F220F0F0F230327072B2F0F0F0F331337173B3F1F1F1F42430344544447074B4F0F0F0F531354544457175B5C0F0F0F5F1F1F1F620000630367076B6F0F0F0F731377177B7F1F1F1F820000830387078B8F0F0F0F931397179B9F1F1F1FA303A707ABAF0F0F0FB313B717BBBF1F1F1FC2C2C303C707CBCF0F0F0FD313D4D4D717DBDCDCDCDF1F1F1FE2E2E303E707EBEF0F0F0FF313F4F4F4F717FBFCFCFCFF1F1F1F090900290900490900690900A009A209A90900C009C90900E009E90900898900C2C2E2E20909290949096909A00900A20900A909C00900C909E00900E9098989C2C2E2E2090900290900490900690900A00900A20900A90900C00900C90900E00900E90900898900C2C2E2E2", assembly.ToString() );
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

      var assembly = TestAssembleC64Studio( source );
      Assert.AreEqual( "6D34127D3412793412651275127112611269122D34123D3412393412251235123112211229120A0E34121E3412061216129001B0FFF0FD2C3412241230F6D0F410F20050EF70ED18D858B8CD3412DD3412D93412C512D512D112C112C912EC3412EC3412E412E012CC3412C412C012CE3412DE3412C612D612CA884D34125D341259341245125512511241124912EE3412FE3412E612F612E8C84C34126C3412203412AD3412BD3412B93412A512B512B112A112A912AE3412BE3412A612B612A212AC3412BC3412A412B412A0124A4E34125E3412461256125CEA0D34121D341219341205121512111201120912480868282A2E34123E3412261236126A6E34127E3412661276124060ED3412FD3412F93412E512F512F112E112E91238F8788D34129D341299341285129512911281128E3412861296128C341284129412AAA8BA8A9A9872123212D2125212B2121212F2129212891234123C34123A1A7C341280D1DA5AFA7A641274129C34129E341214121C341204120C34120F12B61F12B32F12B03F12AD4F12AA5F12A76F12A47F12A18F129E9F129BAF1298BF1295CF1292DF128FEF128CFF12890712171227123712471257126712771287129712A712B712C712D712E712F7121B3B4B6BA312AB3412BB3412C212D412DBDC3412FB02030B2B134E00334B00534800634500734200833F00933C00B33900D33600F3330022341223341282128B34129B3412E2124243441254125B62127BC312CB3412E312EB3412F43412FC3412424205124242061242420A42420D341242420E3412424212124242161242421A42421E341242422412424225124242261242422A42422C341242422D341242422E3412424232124242361242423A42423E341242424342424412424245124242461242424A42424D341242424E341242425212424254124242561242425E3412424265124242661242426A42426D341242426E341242427212424276124242851242428D3412424292124242A5124242AD34124242B1124242B2124242B5124242B934124242BD34124242C5124242C6124242CD34124242CE34124242D2124242D6124242DE34124242E2124242E5124242E6124242ED34124242EE34124242F2124242F6124242FE3412EA1212EA3212EA5212EA7212EA9212EAB212EAD212EAF2124242EA12124242EA32124242EA52124242EA72124242EA92124242EAB2124242EAD2124242EAF212", assembly.ToString() );
    }



    [TestMethod]
    public void TestAssemblyZ80Opcodes()
    {
      string      source = @"!cpu z80

                    * = $00

;ld c,LATE_01  ;0e 01

;!if 0 {

                    ;LD r,r';
                              ld b,b'    ;40 
                              ld b,c'    ;41
                              ld b,d'
                              ld b,e'
                              ld b,h'
                              ld b,l'    ;45
                              ld b,a'    ;47

                              ld c,b'    ;48
                              ld c,c'
                              ld c,d'
                              ld c,e'
                              ld c,h'
                              ld c,l'
                              ld c,a'

                              ld d,b'
                              ld d,c'
                              ld d,d'
                              ld d,e'
                              ld d,h'
                              ld d,l'
                              ld d,a'

                              ld e,b'
                              ld e,c'
                              ld e,d'
                              ld e,e'
                              ld e,h'
                              ld e,l'
                              ld e,a'

                              ld h,b'
                              ld h,c'
                              ld h,d'
                              ld h,e'
                              ld h,h'
                              ld h,l'
                              ld h,a'

                              ld l,b'
                              ld l,c'
                              ld l,d'
                              ld l,e'
                              ld l,h'
                              ld l,l'
                              ld l,a'

                              ld a,b'
                              ld a,c'
                              ld a,d'
                              ld a,e'
                              ld a,h'
                              ld a,l'    ;7d
                              ld a,a'    ;7f

                    ;same without apostrophe
                              ld b,b    ;40
                              ld b,c    ;41
                              ld b,d
                              ld b,e
                              ld b,h
                              ld b,l    ;45
                              ld b,a    ;47

                              ld c,b    ;48
                              ld c,c
                              ld c,d
                              ld c,e
                              ld c,h
                              ld c,l
                              ld c,a

                              ld d,b
                              ld d,c
                              ld d,d
                              ld d,e
                              ld d,h
                              ld d,l
                              ld d,a

                              ld e,b
                              ld e,c
                              ld e,d
                              ld e,e
                              ld e,h
                              ld e,l
                              ld e,a

                              ld h,b
                              ld h,c
                              ld h,d
                              ld h,e
                              ld h,h
                              ld h,l
                              ld h,a

                              ld l,b
                              ld l,c
                              ld l,d
                              ld l,e
                              ld l,h
                              ld l,l
                              ld l,a

                              ld a,b
                              ld a,c
                              ld a,d
                              ld a,e
                              ld a,h
                              ld a,l    ;7d
                              ld a,a    ;7f

                    ;LD r,n

                              ld b,0    ;06 00
                              ld c,1    ;0e 01
                              ld d,2    ;16 02
                              ld e,3    ;1e 03
                              ld h,$fd  ;26 fd
                              ld l,$fe  ;2e fe
                              ld a,$ff  ;3e ff

                              ld b,LATE_00  ;06 00
                              ld c,LATE_01  ;0e 01
                              ld d,LATE_02  ;16 02
                              ld e,LATE_03  ;1e 03
                              ld h,LATE_FD  ;26 fd
                              ld l,LATE_FE  ;2e fe
                              ld a,LATE_FF  ;3e ff

                    ;LD r,(HL)
                              ld b,(HL) ;46
                              ld c,(HL) ;4e
                              ld d,(HL) ;56
                              ld e,(HL) ;5e
                              ld h,(HL) ;66
                              ld l,(HL) ;6e
                              ld a,(HL) ;7e
                    ;LD r,(IX+d)
                              ld b,(IX + 0)        ;DD 46 00
                              ld c,(IX + 1)        ;DD 4E 01
                              ld d,(IX + 2)        ;DD 56 02
                              ld e,(IX + 3)        ;DD 5E 03
                              ld h,(IX + 0xfd)     ;DD 66 FD
                              ld l,(IX + 0xfe)     ;DD 6E FE
                              ld a,(IX + 0xff)     ;DD 7E FF

                              ld b,(IX + LATE_00)  ;DD 46 00
                              ld c,(IX + LATE_01)  ;DD 4E 01
                              ld d,(IX + LATE_02)  ;DD 56 02
                              ld e,(IX + LATE_03)  ;DD 5E 03
                              ld h,(IX + LATE_FD)  ;DD 66 FD
                              ld l,(IX + LATE_FE)  ;DD 6E FE
                              ld a,(IX + LATE_FF)  ;DD 7E FF

                    ;LD r,(IY+d)
                              ld b,(IY + 0)   ;FD 46 00
                              ld c,(IY + 1)   ;FD 4E 01
                              ld d,(IY + 2)   ;FD 56 02
                              ld e,(IY + 3)   ;FD 5E 03
                              ld h,(IY + 4)   ;FD 66 04
                              ld l,(IY + 5)   ;FD 6E 05
                              ld a,(IY + 6)   ;FD 7E 06

                              ld b,(IY + LATE_00)  ;FD 46 00
                              ld c,(IY + LATE_01)  ;FD 4E 01
                              ld d,(IY + LATE_02)  ;FD 56 02
                              ld e,(IY + LATE_03)  ;FD 5E 03
                              ld h,(IY + LATE_FD)  ;FD 66 FD
                              ld l,(IY + LATE_FE)  ;FD 6E FE
                              ld a,(IY + LATE_FF)  ;FD 7E FF


                    ;LD (HL),r
                              ld (HL),b       ;70
                              ld (HL),c       ;71
                              ld (HL),d       ;72
                              ld (HL),e       ;73
                              ld (HL),h       ;74
                              ld (HL),l       ;75
                              ld (HL),a       ;77
                    ;LD (IX+d),r
                              ld (IX + 0),b     ;DD 70 00
                              ld (IX + 1),c     ;DD 71 01
                              ld (IX + 2),d     ;DD 72 02
                              ld (IX + 3),e     ;DD 73 03
                              ld (IX + $fd),h   ;DD 74 FD
                              ld (IX + $fe),l   ;DD 75 FE
                              ld (IX + $ff),a   ;DD 77 FF

                              ld (IX + LATE_00),b  ;DD 70 00
                              ld (IX + LATE_01),c  ;DD 71 01
                              ld (IX + LATE_02),d  ;DD 72 02
                              ld (IX + LATE_03),e  ;DD 73 03
                              ld (IX + LATE_FD),h  ;DD 74 FD
                              ld (IX + LATE_FE),l  ;DD 75 FE
                              ld (IX + LATE_FF),a  ;DD 77 FF

                    ;LD (IY+d),r
                              ld (IY + 0),b     ;FD 70 00
                              ld (IY + 1),c     ;FD 71 01
                              ld (IY + 2),d     ;FD 72 02
                              ld (IY + 3),e     ;FD 73 03
                              ld (IY + $fd),h   ;FD 74 FD
                              ld (IY + $fe),l   ;FD 75 FE
                              ld (IY + $ff),a   ;FD 77 FF

                              ld (IY + LATE_00),b  ;FD 70 00
                              ld (IY + LATE_01),c  ;FD 71 01
                              ld (IY + LATE_02),d  ;FD 72 02
                              ld (IY + LATE_03),e  ;FD 73 03
                              ld (IY + LATE_FD),h  ;FD 74 FD
                              ld (IY + LATE_FE),l  ;FD 75 FE
                              ld (IY + LATE_FF),a  ;FD 77 FF


                    ;LD (HL),n
                              ld (HL),0       ;36 00
                              ld (HL),1       ;36 01
                              ld (HL),2       ;36 02
                              ld (HL),3       ;36 03
                              ld (HL),$fd     ;36 FD
                              ld (HL),$fe     ;36 FE
                              ld (HL),$ff     ;36 FF

                    ;LD (IX+d),n
                              ld ( IX + 0 ), $ff    ;DD3600FF
                              ld ( IX + 1 ), $fe    ;DD3601FE
                              ld ( IX + 2 ), $fd    ;DD3602FD
                              ld ( IX + 3 ), 3      ;DD360303
                              ld ( IX + $fd ), 2    ;DD36FD02
                              ld ( IX + $fe ), 1    ;DD36FE01
                              ld ( IX + $ff ), 0    ;DD36FF00

                              ld ( IX + LATE_00 ), $ff  ;DD3600FF
                              ld ( IX + LATE_01 ), $fe  ;DD3601FE
                              ld ( IX + LATE_02 ), $fd  ;DD3602FD
                              ld ( IX + LATE_03 ), 3    ;DD360303
                              ld ( IX + LATE_FD ), 2    ;DD36FD02
                              ld ( IX + LATE_FE ), 1    ;DD36FE01
                              ld ( IX + LATE_FF ), 0    ;DD36FF00

                    ;LD (IY+d),n
                              ld ( IY + 0 ), $ff    ;FD3600FF
                              ld ( IY + 1 ), $fe    ;FD3601FE
                              ld ( IY + 2 ), $fd    ;FD3602FD
                              ld ( IY + 3 ), 3      ;FD360303
                              ld ( IY + $fd ), 2    ;FD36FD02
                              ld ( IY + $fe ), 1    ;FD36FE01
                              ld ( IY + $ff ), 0    ;FD36FF00

                              ld ( IY + LATE_00 ), $ff  ;FD3600FF
                              ld ( IY + LATE_01 ), $fe  ;FD3601FE
                              ld ( IY + LATE_02 ), $fd  ;FD3602FD
                              ld ( IY + LATE_03 ), 3    ;FD360303
                              ld ( IY + LATE_FD ), 2    ;FD36FD02
                              ld ( IY + LATE_FE ), 1    ;FD36FE01
                              ld ( IY + LATE_FF ), 0    ;FD36FF00

                              ld A,(BC)                ;0A
                              ld A,(DE)                ;1A

                    ;LD A,(nn)
                              ld A,($0000)              ;3A0000
                              ld A,($0001)              ;3A0100
                              ld A,($00ff)              ;3AFF00
                              ld A,($0100)              ;3A0001
                              ld A,($ff00)              ;3A00FF
                              ld A,($bbcc)              ;3ACCBB

                              ld A,(LATE_00)            ;3A0000
                              ld A,(LATE_01)            ;3A0100
                              ld A,(LATE_FF)            ;3AFF00
                              ld A,(LATE_0100)          ;3A0001
                              ld A,(LATE_FF00)          ;3A00FF
                              ld A,(LATE_BBCC)          ;3ACCBB

                              ld (BC),A                 ;02
                              ld (DE),A                 ;12

                    ;LD (nn),A
                              ld ($0000),A              ;320000
                              ld ($0001),A              ;320100
                              ld ($00ff),A              ;32FF00
                              ld ($0100),A              ;320001
                              ld ($ff00),A              ;3200FF
                              ld ($bbcc),A              ;32CCBB

                              ld (LATE_00),A            ;320000
                              ld (LATE_01),A            ;320100
                              ld (LATE_FF),A            ;32FF00
                              ld (LATE_0100),A          ;320001
                              ld (LATE_FF00),A          ;3200FF
                              ld (LATE_BBCC),A          ;32CCBB

                              ld A,I                    ;ED 57
                              ld A,R                    ;ED 5F
                              ld I,A                    ;ED 47
                              ld R,A                    ;ED 4F

                    ;LD dd,nn
                              ld BC,$0001               ;01 01 00
                              ld DE,$00ff               ;11 FF 00
                              ld HL,$0100               ;21 00 01
                              ld SP,$ff00               ;31 00 FF

                              ld BC,LATE_01             ;01 01 00
                              ld DE,LATE_FF             ;11 FF 00
                              ld HL,LATE_0100           ;21 00 01
                              ld SP,LATE_FF00           ;31 00 FF

                    ;LD IX,nn
                              ld IX,$0001               ;DD 21 01 00
                              ld IX,$00ff               ;DD 21 FF 00
                              ld IX,$0100               ;DD 21 00 01
                              ld IX,$ff00               ;DD 21 00 FF

                              ld IX,LATE_01             ;DD 21 01 00
                              ld IX,LATE_FF             ;DD 21 FF 00
                              ld IX,LATE_0100           ;DD 21 00 01
                              ld IX,LATE_FF00           ;DD 21 00 FF

                    ;LD IY,nn
                              ld IY,$0001               ;FD 21 01 00
                              ld IY,$00ff               ;FD 21 FF 00
                              ld IY,$0100               ;FD 21 00 01
                              ld IY,$ff00               ;FD 21 00 FF

                              ld IY,LATE_01             ;FD 21 01 00
                              ld IY,LATE_FF             ;FD 21 FF 00
                              ld IY,LATE_0100           ;FD 21 00 01
                              ld IY,LATE_FF00           ;FD 21 00 FF

                    ;LD HL,(nn)
                              ld HL,($0001)             ;2A 01 00
                              ld HL,($00ff)             ;2A FF 00
                              ld HL,($0100)             ;2A 00 01
                              ld HL,($ff00)             ;2A 00 FF

                              ld HL,(LATE_01)           ;2A 01 00
                              ld HL,(LATE_FF)           ;2A FF 00
                              ld HL,(LATE_0100)         ;2A 00 01
                              ld HL,(LATE_FF00)         ;2A 00 FF

                    ;LD dd,(nn)
                              ld BC,($0001)             ;ED 4B 01 00
                              ld DE,($00ff)             ;ED 5B FF 00
                              ld HL,($0100)             ;2A 00 01  <- is not using LD dd,(nn) form
                              ld SP,($ff00)             ;ED 7B 00 FF

                              ld BC,(LATE_01)           ;ED 4B 01 00
                              ld DE,(LATE_FF)           ;ED 5B FF 00
                              ld HL,(LATE_0100)         ;2A 00 01  <- is not using LD dd,(nn) form
                              ld SP,(LATE_FF00)         ;ED 7B 00 FF

                    ;LD IX,(nn)
                              ld IX,($0001)             ;DD 2A 01 00
                              ld IX,($00ff)             ;DD 2A FF 00
                              ld IX,($0100)             ;DD 2A 00 01
                              ld IX,($ff00)             ;DD 2A 00 FF

                              ld IX,(LATE_01)           ;DD 2A 01 00
                              ld IX,(LATE_FF)           ;DD 2A FF 00
                              ld IX,(LATE_0100)         ;DD 2A 00 01
                              ld IX,(LATE_FF00)         ;DD 2A 00 FF

                    ;LD IY,(nn)
                              ld IY,($0001)             ;FD 2A 01 00
                              ld IY,($00ff)             ;FD 2A FF 00
                              ld IY,($0100)             ;FD 2A 00 01
                              ld IY,($ff00)             ;FD 2A 00 FF

                              ld IY,(LATE_01)           ;FD 2A 01 00
                              ld IY,(LATE_FF)           ;FD 2A FF 00
                              ld IY,(LATE_0100)         ;FD 2A 00 01
                              ld IY,(LATE_FF00)         ;FD 2A 00 FF

                    ;LD (nn),HL
                              ld ($0001),HL             ;22 01 00
                              ld ($00ff),HL             ;22 FF 00
                              ld ($0100),HL             ;22 00 01
                              ld ($ff00),HL             ;22 00 FF

                              ld (LATE_01),HL           ;22 01 00
                              ld (LATE_FF),HL           ;22 FF 00
                              ld (LATE_0100),HL         ;22 00 01
                              ld (LATE_FF00),HL         ;22 00 FF

                    ;LD (nn),dd
                              ld ($0001),BC             ;ED 43 01 00
                              ld ($00ff),DE             ;ED 53 FF 00
                              ld ($0100),HL             ;22 00 01  <- is not using LD (nn),dd form
                              ld ($ff00),SP             ;ED 73 00 FF

                              ld (LATE_01),BC           ;ED 43 01 00
                              ld (LATE_FF),DE           ;ED 53 FF 00
                              ld (LATE_0100),HL         ;22 00 01  <- is not using LD (nn),dd form
                              ld (LATE_FF00),SP         ;ED 73 00 FF

                    ;LD (nn),IX
                              ld ($0001),IX             ;DD 22 01 00
                              ld ($00ff),IX             ;DD 22 FF 00
                              ld ($0100),IX             ;DD 22 00 01
                              ld ($ff00),IX             ;DD 22 00 FF

                              ld (LATE_01),IX           ;DD 22 01 00
                              ld (LATE_FF),IX           ;DD 22 FF 00
                              ld (LATE_0100),IX         ;DD 22 00 01
                              ld (LATE_FF00),IX         ;DD 22 00 FF

                    ;LD (nn),IY
                              ld ($0001),IY             ;FD 22 01 00
                              ld ($00ff),IY             ;FD 22 FF 00
                              ld ($0100),IY             ;FD 22 00 01
                              ld ($ff00),IY             ;FD 22 00 FF

                              ld (LATE_01),IY           ;FD 22 01 00
                              ld (LATE_FF),IY           ;FD 22 FF 00
                              ld (LATE_0100),IY         ;FD 22 00 01
                              ld (LATE_FF00),IY         ;FD 22 00 FF

                              ld SP,HL                  ;F9
                              ld SP,ix                  ;DDF9
                              ld SP,iy                  ;FDF9


                    ;PUSH qq
                              push BC                   ;C5
                              push DE                   ;D5
                              push HL                   ;E5
                              push AF                   ;F5

                              push IX                   ;DD E5
                              push IY                   ;FD E5

                              pop BC                    ;C1
                              pop DE                    ;D1
                              pop HL                    ;E1
                              pop AF                    ;F1

                              pop ix                    ;DD E1
                              pop iy                    ;FD E1

                              ex DE,HL                  ;EB
                              ex AF,AF'                 ;08
                              exx                       ;D9

                              ex (SP),HL                ;E3
                              ex (SP),IX                ;DD E3
                              ex (SP),IY                ;FD E3

                              ldi                       ;ED A0
                              ldir                      ;ED B0
                              ldd                       ;ED A8
                              lddr                      ;ED B8

                              cpi                       ;ED A1
                              cpir                      ;ED B1
                              cpd                       ;ED A9
                              cpdr                      ;ED B9

                    ;ADD A,r
                              add A,b                   ;80
                              add A,c                   ;81
                              add A,d                   ;82
                              add A,e                   ;83
                              add A,h                   ;84
                              add A,l                   ;85
                              add A,a                   ;87

                    ;ADD A,n
                              add A,$01                 ;C6 01
                              add A,$02                 ;C6 02
                              add A,$FE                 ;C6 FE
                              add A,$FF                 ;C6 FF

                              add A,LATE_01             ;C6 01
                              add A,LATE_02             ;C6 02
                              add A,LATE_FE             ;C6 FE
                              add A,LATE_FF             ;C6 FF

                              add A,(HL)                ;86

                    ;ADD A,(IX+d)
                              add A,(IX + $00)          ;DD 86 00
                              add A,(IX + $01)          ;DD 86 01
                              add A,(IX + $FE)          ;DD 86 FE
                              add A,(IX + $FD)          ;DD 86 FD

                              add A,(IX + LATE_00)      ;DD 86 00
                              add A,(IX + LATE_01)      ;DD 86 01
                              add A,(IX + LATE_FE)      ;DD 86 FE
                              add A,(IX + LATE_FD)      ;DD 86 FD

                    ;ADD A,(IY+d)
                              add A,(IY + $00)          ;FD 86 00
                              add A,(IY + $01)          ;FD 86 01
                              add A,(IY + $FE)          ;FD 86 FE
                              add A,(IY + $FD)          ;FD 86 FD

                              add A,(IY + LATE_00)      ;FD 86 00
                              add A,(IY + LATE_01)      ;FD 86 01
                              add A,(IY + LATE_FE)      ;FD 86 FE
                              add A,(IY + LATE_FD)      ;FD 86 FD

                    ;ADC A,r
                              adc A,b                   ;88
                              adc A,c                   ;89
                              adc A,d                   ;8A
                              adc A,e                   ;8B
                              adc A,h                   ;8C
                              adc A,l                   ;8D
                              adc A,a                   ;8F

                    ;ADC A,n
                              adc A,$01                 ;CE 01
                              adc A,$02                 ;CE 02
                              adc A,$FE                 ;CE FE
                              adc A,$FF                 ;CE FF

                              adc A,LATE_01             ;CE 01
                              adc A,LATE_02             ;CE 02
                              adc A,LATE_FE             ;CE FE
                              adc A,LATE_FF             ;CE FF

                              adc A,(HL)                ;8E

                    ;ADC A,(IX+d)
                              adc A,(IX + $00)          ;DD 8E 00
                              adc A,(IX + $01)          ;DD 8E 01
                              adc A,(IX + $FE)          ;DD 8E FE
                              adc A,(IX + $FD)          ;DD 8E FD

                              adc A,(IX + LATE_00)      ;DD 8E 00
                              adc A,(IX + LATE_01)      ;DD 8E 01
                              adc A,(IX + LATE_FE)      ;DD 8E FE
                              adc A,(IX + LATE_FD)      ;DD 8E FD

                    ;ADC A,(IY+d)
                              adc A,(IY + $00)          ;FD 8E 00
                              adc A,(IY + $01)          ;FD 8E 01
                              adc A,(IY + $FE)          ;FD 8E FE
                              adc A,(IY + $FD)          ;FD 8E FD

                              adc A,(IY + LATE_00)      ;FD 8E 00
                              adc A,(IY + LATE_01)      ;FD 8E 01
                              adc A,(IY + LATE_FE)      ;FD 8E FE
                              adc A,(IY + LATE_FD)      ;FD 8E FD

                    ;sub r
                              sub b                     ;90
                              sub c                     ;91
                              sub d                     ;92
                              sub e                     ;93
                              sub h                     ;94
                              sub l                     ;95
                              sub a                     ;97

                    ;sub n
                              sub $01                   ;D6 01
                              sub $02                   ;D6 02
                              sub $FE                   ;D6 FE
                              sub $FF                   ;D6 FF

                              sub LATE_01               ;D6 01
                              sub LATE_02               ;D6 02
                              sub LATE_FE               ;D6 FE
                              sub LATE_FF               ;D6 FF

                              sub (HL)                  ;96

                    ;SUB (IX+d)
                              sub (IX + $00)            ;DD 96 00
                              sub (IX + $01)            ;DD 96 01
                              sub (IX + $FE)            ;DD 96 FE
                              sub (IX + $FD)            ;DD 96 FD

                              sub (IX + LATE_00)        ;DD 96 00
                              sub (IX + LATE_01)        ;DD 96 01
                              sub (IX + LATE_FE)        ;DD 96 FE
                              sub (IX + LATE_FD)        ;DD 96 FD

                    ;sub (IY+d)
                              sub (IY + $00)            ;FD 96 00
                              sub (IY + $01)            ;FD 96 01
                              sub (IY + $FE)            ;FD 96 FE
                              sub (IY + $FD)            ;FD 96 FD

                              sub (IY + LATE_00)        ;FD 96 00
                              sub (IY + LATE_01)        ;FD 96 01
                              sub (IY + LATE_FE)        ;FD 96 FE
                              sub (IY + LATE_FD)        ;FD 96 FD


                    ;sbc A,r
                              sbc A,b                   ;98
                              sbc A,c                   ;99
                              sbc A,d                   ;9A
                              sbc A,e                   ;9B
                              sbc A,h                   ;9C
                              sbc A,l                   ;9D
                              sbc A,a                   ;9F

                    ;sbc A,n
                              sbc A,$01                 ;DE 01
                              sbc A,$02                 ;DE 02
                              sbc A,$FE                 ;DE FE
                              sbc A,$FF                 ;DE FF

                              sbc A,LATE_01             ;DE 01
                              sbc A,LATE_02             ;DE 02
                              sbc A,LATE_FE             ;DE FE
                              sbc A,LATE_FF             ;DE FF

                              sbc A,(HL)                ;9E

                    ;sbc A,(IX+d)
                              sbc A,(IX + $00)          ;DD 9E 00
                              sbc A,(IX + $01)          ;DD 9E 01
                              sbc A,(IX + $FE)          ;DD 9E FE
                              sbc A,(IX + $FD)          ;DD 9E FD

                              sbc A,(IX + LATE_00)      ;DD 9E 00
                              sbc A,(IX + LATE_01)      ;DD 9E 01
                              sbc A,(IX + LATE_FE)      ;DD 9E FE
                              sbc A,(IX + LATE_FD)      ;DD 9E FD

                    ;sbc A,(IY+d)
                              sbc A,(IY + $00)          ;FD 9E 00
                              sbc A,(IY + $01)          ;FD 9E 01
                              sbc A,(IY + $FE)          ;FD 9E FE
                              sbc A,(IY + $FD)          ;FD 9E FD

                              sbc A,(IY + LATE_00)      ;FD 9E 00
                              sbc A,(IY + LATE_01)      ;FD 9E 01
                              sbc A,(IY + LATE_FE)      ;FD 9E FE
                              sbc A,(IY + LATE_FD)      ;FD 9E FD


                    ;and r
                              and b                     ;A0
                              and c                     ;A1
                              and d                     ;A2
                              and e                     ;A3
                              and h                     ;A4
                              and l                     ;A5
                              and a                     ;A7

                    ;and n
                              and $01                   ;E6 01
                              and $02                   ;E6 02
                              and $FE                   ;E6 FE
                              and $FF                   ;E6 FF

                              and LATE_01               ;E6 01
                              and LATE_02               ;E6 02
                              and LATE_FE               ;E6 FE
                              and LATE_FF               ;E6 FF

                              and (HL)                  ;A6

                    ;and (IX+d)
                              and (IX + $00)            ;DD A6 00
                              and (IX + $01)            ;DD A6 01
                              and (IX + $FE)            ;DD A6 FE
                              and (IX + $FD)            ;DD A6 FD

                              and (IX + LATE_00)        ;DD A6 00
                              and (IX + LATE_01)        ;DD A6 01
                              and (IX + LATE_FE)        ;DD A6 FE
                              and (IX + LATE_FD)        ;DD A6 FD

                    ;and (IY+d)
                              and (IY + $00)            ;FD A6 00
                              and (IY + $01)            ;FD A6 01
                              and (IY + $FE)            ;FD A6 FE
                              and (IY + $FD)            ;FD A6 FD

                              and (IY + LATE_00)        ;FD A6 00
                              and (IY + LATE_01)        ;FD A6 01
                              and (IY + LATE_FE)        ;FD A6 FE
                              and (IY + LATE_FD)        ;FD A6 FD


                    ;or r
                              or b                      ;B0
                              or c                      ;B1
                              or d                      ;B2
                              or e                      ;B3
                              or h                      ;B4
                              or l                      ;B5
                              or a                      ;B7

                    ;or n
                              or $01                    ;F6 01
                              or $02                    ;F6 02
                              or $FE                    ;F6 FE
                              or $FF                    ;F6 FF

                              or LATE_01                ;F6 01
                              or LATE_02                ;F6 02
                              or LATE_FE                ;F6 FE
                              or LATE_FF                ;F6 FF

                              or (HL)                   ;B6

                    ;or (IX+d)
                              or (IX + $00)             ;DD B6 00
                              or (IX + $01)             ;DD B6 01
                              or (IX + $FE)             ;DD B6 FE
                              or (IX + $FD)             ;DD B6 FD

                              or (IX + LATE_00)         ;DD B6 00
                              or (IX + LATE_01)         ;DD B6 01
                              or (IX + LATE_FE)         ;DD B6 FE
                              or (IX + LATE_FD)         ;DD B6 FD

                    ;or (IY+d)
                              or (IY + $00)             ;FD B6 00
                              or (IY + $01)             ;FD B6 01
                              or (IY + $FE)             ;FD B6 FE
                              or (IY + $FD)             ;FD B6 FD

                              or (IY + LATE_00)         ;FD B6 00
                              or (IY + LATE_01)         ;FD B6 01
                              or (IY + LATE_FE)         ;FD B6 FE
                              or (IY + LATE_FD)         ;FD B6 FD


                    ;xor r
                              xor b                     ;A8
                              xor c                     ;A9
                              xor d                     ;AA
                              xor e                     ;AB
                              xor h                     ;AC
                              xor l                     ;AD
                              xor a                     ;AF

                    ;xor n
                              xor $01                   ;EE 01
                              xor $02                   ;EE 02
                              xor $FE                   ;EE FE
                              xor $FF                   ;EE FF

                              xor LATE_01               ;EE 01
                              xor LATE_02               ;EE 02
                              xor LATE_FE               ;EE FE
                              xor LATE_FF               ;EE FF

                              xor (HL)                  ;AE

                    ;xor (IX+d)
                              xor (IX + $00)            ;DD AE 00
                              xor (IX + $01)            ;DD AE 01
                              xor (IX + $FE)            ;DD AE FE
                              xor (IX + $FD)            ;DD AE FD

                              xor (IX + LATE_00)        ;DD AE 00
                              xor (IX + LATE_01)        ;DD AE 01
                              xor (IX + LATE_FE)        ;DD AE FE
                              xor (IX + LATE_FD)        ;DD AE FD

                    ;xor (IY+d)
                              xor (IY + $00)            ;FD AE 00
                              xor (IY + $01)            ;FD AE 01
                              xor (IY + $FE)            ;FD AE FE
                              xor (IY + $FD)            ;FD AE FD

                              xor (IY + LATE_00)        ;FD AE 00
                              xor (IY + LATE_01)        ;FD AE 01
                              xor (IY + LATE_FE)        ;FD AE FE
                              xor (IY + LATE_FD)        ;FD AE FD


                    ;cp r
                              cp b                      ;B8
                              cp c                      ;B9
                              cp d                      ;BA
                              cp e                      ;BB
                              cp h                      ;BC
                              cp l                      ;BD
                              cp a                      ;BF

                    ;cp n
                              cp $01                    ;FE 01
                              cp $02                    ;FE 02
                              cp $FE                    ;FE FE
                              cp $FF                    ;FE FF

                              cp LATE_01                ;FE 01
                              cp LATE_02                ;FE 02
                              cp LATE_FE                ;FE FE
                              cp LATE_FF                ;FE FF

                              cp (HL)                   ;BE

                    ;cp (IX+d)
                              cp (IX + $00)             ;DD BE 00
                              cp (IX + $01)             ;DD BE 01
                              cp (IX + $FE)             ;DD BE FE
                              cp (IX + $FD)             ;DD BE FD

                              cp (IX + LATE_00)         ;DD BE 00
                              cp (IX + LATE_01)         ;DD BE 01
                              cp (IX + LATE_FE)         ;DD BE FE
                              cp (IX + LATE_FD)         ;DD BE FD

                    ;cp (IY+d)
                              cp (IY + $00)             ;FD BE 00
                              cp (IY + $01)             ;FD BE 01
                              cp (IY + $FE)             ;FD BE FE
                              cp (IY + $FD)             ;FD BE FD

                              cp (IY + LATE_00)         ;FD BE 00
                              cp (IY + LATE_01)         ;FD BE 01
                              cp (IY + LATE_FE)         ;FD BE FE
                              cp (IY + LATE_FD)         ;FD BE FD


                    ;inc r
                              inc b                     ;04
                              inc c                     ;0C
                              inc d                     ;14
                              inc e                     ;1C
                              inc h                     ;24
                              inc l                     ;2C
                              inc a                     ;3C

                              inc (HL)                  ;34

                    ;inc (IX+d)
                              inc (IX + $00)            ;DD 34 00
                              inc (IX + $01)            ;DD 34 01
                              inc (IX + $FE)            ;DD 34 FE
                              inc (IX + $FD)            ;DD 34 FD

                              inc (IX + LATE_00)        ;DD 34 00
                              inc (IX + LATE_01)        ;DD 34 01
                              inc (IX + LATE_FE)        ;DD 34 FE
                              inc (IX + LATE_FD)        ;DD 34 FD

                    ;inc (IY+d)
                              inc (IY + $00)            ;FD 34 00
                              inc (IY + $01)            ;FD 34 01
                              inc (IY + $FE)            ;FD 34 FE
                              inc (IY + $FD)            ;FD 34 FD

                              inc (IY + LATE_00)        ;FD 34 00
                              inc (IY + LATE_01)        ;FD 34 01
                              inc (IY + LATE_FE)        ;FD 34 FE
                              inc (IY + LATE_FD)        ;FD 34 FD


                    ;dec r
                              dec b                     ;05
                              dec c                     ;0D
                              dec d                     ;15
                              dec e                     ;1D
                              dec h                     ;25
                              dec l                     ;2D
                              dec a                     ;3D

                              dec (HL)                  ;35

                    ;dec (IX+d)
                              dec (IX + $00)            ;DD 35 00
                              dec (IX + $01)            ;DD 35 01
                              dec (IX + $FE)            ;DD 35 FE
                              dec (IX + $FD)            ;DD 35 FD

                              dec (IX + LATE_00)        ;DD 35 00
                              dec (IX + LATE_01)        ;DD 35 01
                              dec (IX + LATE_FE)        ;DD 35 FE
                              dec (IX + LATE_FD)        ;DD 35 FD

                    ;dec (IY+d)
                              dec (IY + $00)            ;FD 35 00
                              dec (IY + $01)            ;FD 35 01
                              dec (IY + $FE)            ;FD 35 FE
                              dec (IY + $FD)            ;FD 35 FD

                              dec (IY + LATE_00)        ;FD 35 00
                              dec (IY + LATE_01)        ;FD 35 01
                              dec (IY + LATE_FE)        ;FD 35 FE
                              dec (IY + LATE_FD)        ;FD 35 FD

                              daa                       ;27
                              cpl                       ;2F
                              neg                       ;ED 44
                              ccf                       ;3F
                              scf                       ;37
                              nop                       ;00
                              halt                      ;76
                              di                        ;F3
                              ei                        ;FB
                              im 0                      ;ED 46
                              im 1                      ;ED 56
                              im 2                      ;ED 5E

                    ;ADD HL,ss
                              add HL,BC     ;09
                              add HL,DE     ;19
                              add HL,HL     ;29
                              add HL,SP     ;39

                    ;ADC HL,ss
                              adc HL,BC     ;ED 4A
                              adc HL,DE     ;ED 5A
                              adc HL,HL     ;ED 6A
                              adc HL,SP     ;ED 7A

                    ;SBC HL,ss
                              sbc HL,BC     ;ED 42
                              sbc HL,DE     ;ED 52
                              sbc HL,HL     ;ED 62
                              sbc HL,SP     ;ED 72

                    ;ADD IX,pp
                              add IX,BC     ;DD 09
                              add IX,DE     ;DD 19
                              add IX,IX     ;DD 29
                              add IX,SP     ;DD 39

                    ;ADD IY,rr
                              add IY,BC     ;FD 09
                              add IY,DE     ;FD 19
                              add IY,IY     ;FD 29
                              add IY,SP     ;FD 39

                    ;INC ss
                              inc BC        ;03
                              inc DE        ;13
                              inc HL        ;23
                              inc SP        ;33

                              inc IX        ;DD 23
                              inc IY        ;FD 23

                    ;DEC ss
                              dec BC        ;0B
                              dec DE        ;1B
                              dec HL        ;2B
                              dec SP        ;3B

                              dec IX        ;DD 2B
                              dec IY        ;FD 2B


                              rlca          ;07
                              rla           ;17
                              rrca          ;0F
                              rra           ;1F

                    ;RLC r
                              rlc b         ;CB 00
                              rlc c         ;CB 01
                              rlc d         ;CB 02
                              rlc e         ;CB 03
                              rlc h         ;CB 04
                              rlc l         ;CB 05
                              rlc a         ;CB 07

                              rlc (HL)      ;CB 06

                    ;RLC (IX+d)
                              rlc (IX + $00)      ;DD CB 00 06
                              rlc (IX + $01)      ;DD CB 01 06
                              rlc (IX + $fe)      ;DD CB FE 06
                              rlc (IX + $ff)      ;DD CB FF 06

                              rlc (IX + LATE_00)  ;DD CB 00 06
                              rlc (IX + LATE_01)  ;DD CB 01 06
                              rlc (IX + LATE_FE)  ;DD CB FE 06
                              rlc (IX + LATE_FF)  ;DD CB FF 06

                              rlc (IY + $00)      ;FD CB 00 06
                              rlc (IY + $01)      ;FD CB 01 06
                              rlc (IY + $fe)      ;FD CB FE 06
                              rlc (IY + $ff)      ;FD CB FF 06

                              rlc (IY + LATE_00)  ;FD CB 00 06
                              rlc (IY + LATE_01)  ;FD CB 01 06
                              rlc (IY + LATE_FE)  ;FD CB FE 06
                              rlc (IY + LATE_FF)  ;FD CB FF 06


                    ;rl r
                              rl b         ;CB 10
                              rl c         ;CB 11
                              rl d         ;CB 12
                              rl e         ;CB 13
                              rl h         ;CB 14
                              rl l         ;CB 15
                              rl a         ;CB 17

                              rl (HL)      ;CB 16

                    ;rl (IX+d)
                              rl (IX + $00)      ;DD CB 00 16
                              rl (IX + $01)      ;DD CB 01 16
                              rl (IX + $fe)      ;DD CB FE 16
                              rl (IX + $ff)      ;DD CB FF 16

                              rl (IX + LATE_00)  ;DD CB 00 16
                              rl (IX + LATE_01)  ;DD CB 01 16
                              rl (IX + LATE_FE)  ;DD CB FE 16
                              rl (IX + LATE_FF)  ;DD CB FF 16

                              rl (IY + $00)      ;FD CB 00 16
                              rl (IY + $01)      ;FD CB 01 16
                              rl (IY + $fe)      ;FD CB FE 16
                              rl (IY + $ff)      ;FD CB FF 16

                              rl (IY + LATE_00)  ;FD CB 00 16
                              rl (IY + LATE_01)  ;FD CB 01 16
                              rl (IY + LATE_FE)  ;FD CB FE 16
                              rl (IY + LATE_FF)  ;FD CB FF 16


                    ;rrc r
                              rrc b         ;CB 08
                              rrc c         ;CB 09
                              rrc d         ;CB 0A
                              rrc e         ;CB 0B
                              rrc h         ;CB 0C
                              rrc l         ;CB 0D
                              rrc a         ;CB 0F

                              rrc (HL)      ;CB 0E

                    ;rrc (IX+d)
                              rrc (IX + $00)      ;DD CB 00 0E
                              rrc (IX + $01)      ;DD CB 01 0E
                              rrc (IX + $fe)      ;DD CB FE 0E
                              rrc (IX + $ff)      ;DD CB FF 0E

                              rrc (IX + LATE_00)  ;DD CB 00 0E
                              rrc (IX + LATE_01)  ;DD CB 01 0E
                              rrc (IX + LATE_FE)  ;DD CB FE 0E
                              rrc (IX + LATE_FF)  ;DD CB FF 0E

                              rrc (IY + $00)      ;FD CB 00 0E
                              rrc (IY + $01)      ;FD CB 01 0E
                              rrc (IY + $fe)      ;FD CB FE 0E
                              rrc (IY + $ff)      ;FD CB FF 0E

                              rrc (IY + LATE_00)  ;FD CB 00 0E
                              rrc (IY + LATE_01)  ;FD CB 01 0E
                              rrc (IY + LATE_FE)  ;FD CB FE 0E
                              rrc (IY + LATE_FF)  ;FD CB FF 0E

                    ;rr r
                              rr b         ;CB 18
                              rr c         ;CB 19
                              rr d         ;CB 1A
                              rr e         ;CB 1B
                              rr h         ;CB 1C
                              rr l         ;CB 1D
                              rr a         ;CB 1F

                              rr (HL)      ;CB 1E

                    ;rr (IX+d)
                              rr (IX + $00)      ;DD CB 00 1E
                              rr (IX + $01)      ;DD CB 01 1E
                              rr (IX + $fe)      ;DD CB FE 1E
                              rr (IX + $ff)      ;DD CB FF 1E

                              rr (IX + LATE_00)  ;DD CB 00 1E
                              rr (IX + LATE_01)  ;DD CB 01 1E
                              rr (IX + LATE_FE)  ;DD CB FE 1E
                              rr (IX + LATE_FF)  ;DD CB FF 1E

                              rr (IY + $00)      ;FD CB 00 1E
                              rr (IY + $01)      ;FD CB 01 1E
                              rr (IY + $fe)      ;FD CB FE 1E
                              rr (IY + $ff)      ;FD CB FF 1E

                              rr (IY + LATE_00)  ;FD CB 00 1E
                              rr (IY + LATE_01)  ;FD CB 01 1E
                              rr (IY + LATE_FE)  ;FD CB FE 1E
                              rr (IY + LATE_FF)  ;FD CB FF 1E


                    ;sla r
                              sla b         ;CB 20
                              sla c         ;CB 21
                              sla d         ;CB 22
                              sla e         ;CB 23
                              sla h         ;CB 24
                              sla l         ;CB 25
                              sla a         ;CB 27

                              sla (HL)      ;CB 26

                    ;sla (IX+d)
                              sla (IX + $00)      ;DD CB 00 26
                              sla (IX + $01)      ;DD CB 01 26
                              sla (IX + $fe)      ;DD CB FE 26
                              sla (IX + $ff)      ;DD CB FF 26

                              sla (IX + LATE_00)  ;DD CB 00 26
                              sla (IX + LATE_01)  ;DD CB 01 26
                              sla (IX + LATE_FE)  ;DD CB FE 26
                              sla (IX + LATE_FF)  ;DD CB FF 26

                              sla (IY + $00)      ;FD CB 00 26
                              sla (IY + $01)      ;FD CB 01 26
                              sla (IY + $fe)      ;FD CB FE 26
                              sla (IY + $ff)      ;FD CB FF 26

                              sla (IY + LATE_00)  ;FD CB 00 26
                              sla (IY + LATE_01)  ;FD CB 01 26
                              sla (IY + LATE_FE)  ;FD CB FE 26
                              sla (IY + LATE_FF)  ;FD CB FF 26

                    ;sra r
                              sra b         ;CB 28
                              sra c         ;CB 29
                              sra d         ;CB 2A
                              sra e         ;CB 2B
                              sra h         ;CB 2C
                              sra l         ;CB 2D
                              sra a         ;CB 2F

                              sra (HL)      ;CB 2E

                    ;sra (IX+d)
                              sra (IX + $00)      ;DD CB 00 2E
                              sra (IX + $01)      ;DD CB 01 2E
                              sra (IX + $fe)      ;DD CB FE 2E
                              sra (IX + $ff)      ;DD CB FF 2E

                              sra (IX + LATE_00)  ;DD CB 00 2E
                              sra (IX + LATE_01)  ;DD CB 01 2E
                              sra (IX + LATE_FE)  ;DD CB FE 2E
                              sra (IX + LATE_FF)  ;DD CB FF 2E

                              sra (IY + $00)      ;FD CB 00 2E
                              sra (IY + $01)      ;FD CB 01 2E
                              sra (IY + $fe)      ;FD CB FE 2E
                              sra (IY + $ff)      ;FD CB FF 2E

                              sra (IY + LATE_00)  ;FD CB 00 2E
                              sra (IY + LATE_01)  ;FD CB 01 2E
                              sra (IY + LATE_FE)  ;FD CB FE 2E
                              sra (IY + LATE_FF)  ;FD CB FF 2E


                    ;srl r
                              srl b         ;CB 38
                              srl c         ;CB 39
                              srl d         ;CB 3A
                              srl e         ;CB 3B
                              srl h         ;CB 3C
                              srl l         ;CB 3D
                              srl a         ;CB 3F

                              srl (HL)      ;CB 3E

                    ;srl (IX+d)
                              srl (IX + $00)      ;DD CB 00 3E
                              srl (IX + $01)      ;DD CB 01 3E
                              srl (IX + $fe)      ;DD CB FE 3E
                              srl (IX + $ff)      ;DD CB FF 3E

                              srl (IX + LATE_00)  ;DD CB 00 3E
                              srl (IX + LATE_01)  ;DD CB 01 3E
                              srl (IX + LATE_FE)  ;DD CB FE 3E
                              srl (IX + LATE_FF)  ;DD CB FF 3E

                              srl (IY + $00)      ;FD CB 00 3E
                              srl (IY + $01)      ;FD CB 01 3E
                              srl (IY + $fe)      ;FD CB FE 3E
                              srl (IY + $ff)      ;FD CB FF 3E

                              srl (IY + LATE_00)  ;FD CB 00 3E
                              srl (IY + LATE_01)  ;FD CB 01 3E
                              srl (IY + LATE_FE)  ;FD CB FE 3E
                              srl (IY + LATE_FF)  ;FD CB FF 3E


                              rld           ;ED 6F
                              rrd           ;ED 67

                    ;BIT b,r
                              bit $00,b         ;CB 40
                              bit $01,c         ;CB 49
                              bit $02,d         ;CB 52
                              bit $03,e         ;CB 5B
                              bit $04,h         ;CB 64
                              bit $05,l         ;CB 6D
                              bit $06,a         ;CB 77
                              bit $07,a         ;CB 7F

                              bit LATE_00,b     ;CB 40
                              bit LATE_01,c     ;CB 49
                              bit LATE_02,d     ;CB 52
                              bit LATE_03,e     ;CB 5B
                              bit LATE_04,h     ;CB 64
                              bit LATE_05,l     ;CB 6D
                              bit LATE_06,a     ;CB 77
                              bit LATE_07,a     ;CB 7F

                              bit $00,(HL)      ;CB 46
                              bit $01,(HL)      ;CB 4E
                              bit $02,(HL)      ;CB 56
                              bit $03,(HL)      ;CB 5E
                              bit $04,(HL)      ;CB 66
                              bit $05,(HL)      ;CB 6E
                              bit $06,(HL)      ;CB 76
                              bit $07,(HL)      ;CB 7E

                              bit LATE_00,(HL)  ;CB 46
                              bit LATE_01,(HL)  ;CB 4E
                              bit LATE_02,(HL)  ;CB 56
                              bit LATE_03,(HL)  ;CB 5E
                              bit LATE_04,(HL)  ;CB 66
                              bit LATE_05,(HL)  ;CB 6E
                              bit LATE_06,(HL)  ;CB 76
                              bit LATE_07,(HL)  ;CB 7E

                    ;BIT b,(IX+d)
                              bit $00,(IX+$FF)  ;DD CB FF 46
                              bit $01,(IX+$FE)  ;DD CB FE 4E
                              bit $02,(IX+$FD)  ;DD CB FD 56
                              bit $03,(IX+$04)  ;DD CB 04 5E
                              bit $04,(IX+$03)  ;DD CB 03 66
                              bit $05,(IX+$02)  ;DD CB 02 6E
                              bit $06,(IX+$01)  ;DD CB 01 76
                              bit $07,(IX+$00)  ;DD CB 00 7E

                              bit LATE_00,(IX+LATE_FF)  ;DD CB FF 46
                              bit LATE_01,(IX+LATE_FE)  ;DD CB FE 4E
                              bit LATE_02,(IX+LATE_FD)  ;DD CB FD 56
                              bit LATE_03,(IX+LATE_04)  ;DD CB 04 5E
                              bit LATE_04,(IX+LATE_03)  ;DD CB 03 66
                              bit LATE_05,(IX+LATE_02)  ;DD CB 02 6E
                              bit LATE_06,(IX+LATE_01)  ;DD CB 01 76
                              bit LATE_07,(IX+LATE_00)  ;DD CB 00 7E

                    ;BIT b,(IY+d)
                              bit $00,(IY+$FF)  ;FD CB FF 46
                              bit $01,(IY+$FE)  ;FD CB FE 4E
                              bit $02,(IY+$FD)  ;FD CB FD 56
                              bit $03,(IY+$04)  ;FD CB 04 5E
                              bit $04,(IY+$03)  ;FD CB 03 66
                              bit $05,(IY+$02)  ;FD CB 02 6E
                              bit $06,(IY+$01)  ;FD CB 01 76
                              bit $07,(IY+$00)  ;FD CB 00 7E

                              bit LATE_00,(IY+LATE_FF)  ;FD CB FF 46
                              bit LATE_01,(IY+LATE_FE)  ;FD CB FE 4E
                              bit LATE_02,(IY+LATE_FD)  ;FD CB FD 56
                              bit LATE_03,(IY+LATE_04)  ;FD CB 04 5E
                              bit LATE_04,(IY+LATE_03)  ;FD CB 03 66
                              bit LATE_05,(IY+LATE_02)  ;FD CB 02 6E
                              bit LATE_06,(IY+LATE_01)  ;FD CB 01 76
                              bit LATE_07,(IY+LATE_00)  ;FD CB 00 7E


                    ;set b,r
                              set $00,b         ;CB C0
                              set $01,c         ;CB C9
                              set $02,d         ;CB D2
                              set $03,e         ;CB DB
                              set $04,h         ;CB E4
                              set $05,l         ;CB ED
                              set $06,a         ;CB F7
                              set $07,a         ;CB FF

                              set LATE_00,b     ;CB C0
                              set LATE_01,c     ;CB C9
                              set LATE_02,d     ;CB D2
                              set LATE_03,e     ;CB DB
                              set LATE_04,h     ;CB E4
                              set LATE_05,l     ;CB ED
                              set LATE_06,a     ;CB F7
                              set LATE_07,a     ;CB FF

                              set $00,(HL)      ;CB C6
                              set $01,(HL)      ;CB CE
                              set $02,(HL)      ;CB D6
                              set $03,(HL)      ;CB DE
                              set $04,(HL)      ;CB E6
                              set $05,(HL)      ;CB EE
                              set $06,(HL)      ;CB F6
                              set $07,(HL)      ;CB FE

                              set LATE_00,(HL)  ;CB C6
                              set LATE_01,(HL)  ;CB CE
                              set LATE_02,(HL)  ;CB D6
                              set LATE_03,(HL)  ;CB DE
                              set LATE_04,(HL)  ;CB E6
                              set LATE_05,(HL)  ;CB EE
                              set LATE_06,(HL)  ;CB F6
                              set LATE_07,(HL)  ;CB FE

                    ;set b,(IX+d)
                              set $00,(IX+$FF)  ;DD CB FF C6
                              set $01,(IX+$FE)  ;DD CB FE CE
                              set $02,(IX+$FD)  ;DD CB FD D6
                              set $03,(IX+$04)  ;DD CB 04 DE
                              set $04,(IX+$03)  ;DD CB 03 E6
                              set $05,(IX+$02)  ;DD CB 02 EE
                              set $06,(IX+$01)  ;DD CB 01 F6
                              set $07,(IX+$00)  ;DD CB 00 FE

                              set LATE_00,(IX+LATE_FF)  ;DD CB FF C6
                              set LATE_01,(IX+LATE_FE)  ;DD CB FE CE
                              set LATE_02,(IX+LATE_FD)  ;DD CB FD D6
                              set LATE_03,(IX+LATE_04)  ;DD CB 04 DE
                              set LATE_04,(IX+LATE_03)  ;DD CB 03 E6
                              set LATE_05,(IX+LATE_02)  ;DD CB 02 EE
                              set LATE_06,(IX+LATE_01)  ;DD CB 01 F6
                              set LATE_07,(IX+LATE_00)  ;DD CB 00 FE

                    ;set b,(IY+d)
                              set $00,(IY+$FF)  ;FD CB FF C6
                              set $01,(IY+$FE)  ;FD CB FE CE
                              set $02,(IY+$FD)  ;FD CB FD D6
                              set $03,(IY+$04)  ;FD CB 04 DE
                              set $04,(IY+$03)  ;FD CB 03 E6
                              set $05,(IY+$02)  ;FD CB 02 EE
                              set $06,(IY+$01)  ;FD CB 01 F6
                              set $07,(IY+$00)  ;FD CB 00 FE

                              set LATE_00,(IY+LATE_FF)  ;FD CB FF C6
                              set LATE_01,(IY+LATE_FE)  ;FD CB FE CE
                              set LATE_02,(IY+LATE_FD)  ;FD CB FD D6
                              set LATE_03,(IY+LATE_04)  ;FD CB 04 DE
                              set LATE_04,(IY+LATE_03)  ;FD CB 03 E6
                              set LATE_05,(IY+LATE_02)  ;FD CB 02 EE
                              set LATE_06,(IY+LATE_01)  ;FD CB 01 F6
                              set LATE_07,(IY+LATE_00)  ;FD CB 00 FE


                    ;res b,r
                              res $00,b         ;CB 80
                              res $01,c         ;CB 89
                              res $02,d         ;CB 92
                              res $03,e         ;CB 9B
                              res $04,h         ;CB A4
                              res $05,l         ;CB AD
                              res $06,a         ;CB B7
                              res $07,a         ;CB BF

                              res LATE_00,b     ;CB 80
                              res LATE_01,c     ;CB 89
                              res LATE_02,d     ;CB 92
                              res LATE_03,e     ;CB 9B
                              res LATE_04,h     ;CB A4
                              res LATE_05,l     ;CB AD
                              res LATE_06,a     ;CB B7
                              res LATE_07,a     ;CB BF

                              res $00,(HL)      ;CB 86
                              res $01,(HL)      ;CB 8E
                              res $02,(HL)      ;CB 96
                              res $03,(HL)      ;CB 9E
                              res $04,(HL)      ;CB A6
                              res $05,(HL)      ;CB AE
                              res $06,(HL)      ;CB B6
                              res $07,(HL)      ;CB BE

                              res LATE_00,(HL)  ;CB 86
                              res LATE_01,(HL)  ;CB 8E
                              res LATE_02,(HL)  ;CB 96
                              res LATE_03,(HL)  ;CB 9E
                              res LATE_04,(HL)  ;CB A6
                              res LATE_05,(HL)  ;CB AE
                              res LATE_06,(HL)  ;CB B6
                              res LATE_07,(HL)  ;CB BE

                    ;res b,(IX+d)
                              res $00,(IX+$FF)  ;DD CB FF 86
                              res $01,(IX+$FE)  ;DD CB FE 8E
                              res $02,(IX+$FD)  ;DD CB FD 96
                              res $03,(IX+$04)  ;DD CB 04 9E
                              res $04,(IX+$03)  ;DD CB 03 A6
                              res $05,(IX+$02)  ;DD CB 02 AE
                              res $06,(IX+$01)  ;DD CB 01 B6
                              res $07,(IX+$00)  ;DD CB 00 BE

                              res LATE_00,(IX+LATE_FF)  ;DD CB FF 86
                              res LATE_01,(IX+LATE_FE)  ;DD CB FE 8E
                              res LATE_02,(IX+LATE_FD)  ;DD CB FD 96
                              res LATE_03,(IX+LATE_04)  ;DD CB 04 9E
                              res LATE_04,(IX+LATE_03)  ;DD CB 03 A6
                              res LATE_05,(IX+LATE_02)  ;DD CB 02 AE
                              res LATE_06,(IX+LATE_01)  ;DD CB 01 B6
                              res LATE_07,(IX+LATE_00)  ;DD CB 00 BE

                    ;res b,(IY+d)
                              res $00,(IY+$FF)  ;FD CB FF 86
                              res $01,(IY+$FE)  ;FD CB FE 8E
                              res $02,(IY+$FD)  ;FD CB FD 96
                              res $03,(IY+$04)  ;FD CB 04 9E
                              res $04,(IY+$03)  ;FD CB 03 A6
                              res $05,(IY+$02)  ;FD CB 02 AE
                              res $06,(IY+$01)  ;FD CB 01 B6
                              res $07,(IY+$00)  ;FD CB 00 BE

                              res LATE_00,(IY+LATE_FF)  ;FD CB FF 86
                              res LATE_01,(IY+LATE_FE)  ;FD CB FE 8E
                              res LATE_02,(IY+LATE_FD)  ;FD CB FD 96
                              res LATE_03,(IY+LATE_04)  ;FD CB 04 9E
                              res LATE_04,(IY+LATE_03)  ;FD CB 03 A6
                              res LATE_05,(IY+LATE_02)  ;FD CB 02 AE
                              res LATE_06,(IY+LATE_01)  ;FD CB 01 B6
                              res LATE_07,(IY+LATE_00)  ;FD CB 00 BE

                    ;JP nn
                              jp $0001
                              jp $00FF
                              jp $0100
                              jp $FF00

                              jp LATE_0001
                              jp LATE_00FF
                              jp LATE_0100
                              jp LATE_FF00

                    ;JP cc,nn
                              jp NZ,$0001       ;C2 01 00
                              jp z,$00FF        ;CA FF 00
                              jp nc,$0100       ;D2 00 01
                              jp c,$FF00        ;DA 00 FF

                              jp po,LATE_0001   ;E2 01 00
                              jp pe,LATE_00FF   ;EA FF 00
                              jp p,LATE_0100    ;F2 00 01
                              jp m,LATE_FF00    ;FA 00 FF

                    ;JR e
                    POS
                              jr POS + 2          ;18 00
                              jr POS + 2 - $56    ;18 A8
                              jr POS + 2 + $56    ;18 52

                              jr NEXT_POS + 2     ;18 06
                              jr NEXT_POS - $56   ;18 AC
                              jr NEXT_POS + $56   ;18 56
                    NEXT_POS


                    ;JR C,e
                    POS2
                              jr C,POS2 + 2           ;38 00
                              jr C,POS2 + 2 - $56     ;38 A8
                              jr C,POS2 + 2 + $56     ;38 52

                              jr C,NEXT_POS2 + 2      ;38 06
                              jr C,NEXT_POS2 - $56    ;38 AC
                              jr C,NEXT_POS2 + $56    ;38 56
                    NEXT_POS2


                    ;JR NC,e
                    POS3
                              jr nc,POS3 + 2           ;30 00
                              jr nc,POS3 + 2 - $56     ;30 A8
                              jr nc,POS3 + 2 + $56     ;30 52

                              jr nc,NEXT_POS3 + 2      ;30 06
                              jr nc,NEXT_POS3 - $56    ;30 AC
                              jr nc,NEXT_POS3 + $56    ;30 56
                    NEXT_POS3


                              jp (HL)                   ;E9
                              jp (IX)                   ;DD E9
                              jp (IY)                   ;FD E9

                    POS4
                              djnz POS4 + 2             ;10 00
                              djnz POS4 + 2 - $56       ;10 A8
                              djnz POS4 + 2 + $56       ;10 52

                              djnz NEXT_POS4 + 2    ;10 06
                              djnz NEXT_POS4 - $56  ;10 AC
                              djnz NEXT_POS4 + $56  ;10 56
                    NEXT_POS4


                    ;CALL nn
                              call $0001          ;CD 01 00
                              call $00FE          ;CD FE 00
                              call $0100          ;CD 00 01
                              call $FF00          ;CD 00 FF

                              call LATE_0001      ;CD 01 00
                              call LATE_00FE      ;CD FE 00
                              call LATE_0100      ;CD 00 01
                              call LATE_FF00      ;CD 00 FF

                    ;CALL cc,nn
                              call nz,$0001       ;C4 01 00
                              call z,$00FE        ;CC FE 00
                              call nc,$0100       ;D4 00 01
                              call c,$FF00        ;DC 00 FF

                              call po,LATE_0001   ;E4 01 00
                              call pe,LATE_00FE   ;EC FE 00
                              call p,LATE_0100    ;F4 00 01
                              call m,LATE_FF00    ;FC 00 FF

                              ret         ;C9

                    ;RET cc
                              ret nz      ;C0
                              ret z       ;C8
                              ret nc      ;D0
                              ret c       ;D8
                              ret po      ;E0
                              ret pe      ;E8
                              ret p       ;F0
                              ret m       ;F8

                              reti        ;ED 4D
                              retn        ;ED 45

                    ;RST p
                              rst $00     ;C7
                              rst $08     ;CF
                              rst $10     ;D7
                              rst $18     ;DF
                              rst $20     ;E7
                              rst $28     ;EF
                              rst $30     ;F7
                              rst $38     ;FF

                    ;IN A,(n)
                              in A,($00)      ;DB 00
                              in A,($01)      ;DB 01
                              in A,($FE)      ;DB FE
                              in A,($FF)      ;DB FF

                              in A,(LATE_00)  ;DB 00
                              in A,(LATE_01)  ;DB 01
                              in A,(LATE_FE)  ;DB FE
                              in A,(LATE_FF)  ;DB FF

                    ;IN r,(C)
                              in b,(C)     ;ED 40
                              in c,(C)     ;ED 48
                              in d,(C)     ;ED 50
                              in e,(C)     ;ED 58
                              in h,(C)     ;ED 60
                              in l,(C)     ;ED 68
                              in a,(C)     ;ED 78

                              ini           ;ED A2
                              inir          ;ED B2

                              ind           ;ED AA
                              indr          ;ED BA

                    ;OUT (n),A
                              out ($00),A      ;D3 00
                              out ($01),A      ;D3 01
                              out ($FE),A      ;D3 FE
                              out ($FF),A      ;D3 FF

                              out (LATE_00),A  ;D3 00
                              out (LATE_01),A  ;D3 01
                              out (LATE_FE),A  ;D3 FE
                              out (LATE_FF),A  ;D3 FF

                    ;OUT (C),r
                              out (C),b     ;ED 41
                              out (C),c     ;ED 49
                              out (C),d     ;ED 51
                              out (C),e     ;ED 59
                              out (C),h     ;ED 61
                              out (C),l     ;ED 69
                              out (C),a     ;ED 79

                              outi          ;ED A3
                              otir          ;ED B3
                              outd          ;ED AB
                              otdr          ;ED BB


                    ;undocumented opcodes (there's more, but the doc of undocumented opcodes is bad)
                              sll b         ;CB 30
                              sll c         ;CB 31
                              sll d         ;CB 32
                              sll e         ;CB 33
                              sll h         ;CB 34
                              sll l         ;CB 35
                              sll a         ;CB 37

                              sll (HL)      ;CB 36

;}

                    LATE_FF = $ff
                    LATE_FE = $fe
                    LATE_FD = $fd
                    LATE_07 = $07
                    LATE_06 = $06
                    LATE_05 = $05
                    LATE_04 = $04
                    LATE_03 = $03
                    LATE_02 = $02
                    LATE_01 = $01
                    LATE_00 = $00
                    LATE_0000 = $0000
                    LATE_0001 = $0001
                    LATE_00FE = $00FE
                    LATE_00FF = $00FF
                    LATE_0100 = $0100
                    LATE_FF00 = $ff00
                    LATE_BBCC = $bbcc";

      var assembly = TestAssembleC64Studio( source );
      Assert.AreEqual( "00004041424344454748494A4B4C4D4F5051525354555758595A5B5C5D5F6061626364656768696A6B6C6D6F78797A7B7C7D7F4041424344454748494A4B4C4D4F5051525354555758595A5B5C5D5F6061626364656768696A6B6C6D6F78797A7B7C7D7F06000E0116021E0326FD2EFE3EFF06000E0116021E0326FD2EFE3EFF464E565E666E7EDD4600DD4E01DD5602DD5E03DD66FDDD6EFEDD7EFFDD4600DD4E01DD5602DD5E03DD66FDDD6EFEDD7EFFFD4600FD4E01FD5602FD5E03FD6604FD6E05FD7E06FD4600FD4E01FD5602FD5E03FD66FDFD6EFEFD7EFF70717273747577DD7000DD7101DD7202DD7303DD74FDDD75FEDD77FFDD7000DD7101DD7202DD7303DD74FDDD75FEDD77FFFD7000FD7101FD7202FD7303FD74FDFD75FEFD77FFFD7000FD7101FD7202FD7303FD74FDFD75FEFD77FF360036013602360336FD36FE36FFDD3600FFDD3601FEDD3602FDDD360303DD36FD02DD36FE01DD36FF00DD3600FFDD3601FEDD3602FDDD360303DD36FD02DD36FE01DD36FF00FD3600FFFD3601FEFD3602FDFD360303FD36FD02FD36FE01FD36FF00FD3600FFFD3601FEFD3602FDFD360303FD36FD02FD36FE01FD36FF000A1A3A00003A01003AFF003A00013A00FF3ACCBB3A00003A01003AFF003A00013A00FF3ACCBB021232000032010032FF003200013200FF32CCBB32000032010032FF003200013200FF32CCBBED57ED5FED47ED4F01010011FF002100013100FF01010011FF002100013100FFDD210100DD21FF00DD210001DD2100FFDD210100DD21FF00DD210001DD2100FFFD210100FD21FF00FD210001FD2100FFFD210100FD21FF00FD210001FD2100FF2A01002AFF002A00012A00FF2A01002AFF002A00012A00FFED4B0100ED5BFF002A0001ED7B00FFED4B0100ED5BFF002A0001ED7B00FFDD2A0100DD2AFF00DD2A0001DD2A00FFDD2A0100DD2AFF00DD2A0001DD2A00FFFD2A0100FD2AFF00FD2A0001FD2A00FFFD2A0100FD2AFF00FD2A0001FD2A00FF22010022FF002200012200FF22010022FF002200012200FFED430100ED53FF00220001ED7300FFED430100ED53FF00220001ED7300FFDD220100DD22FF00DD220001DD2200FFDD220100DD22FF00DD220001DD2200FFFD220100FD22FF00FD220001FD2200FFFD220100FD22FF00FD220001FD2200FFF9DDF9FDF9C5D5E5F5DDE5FDE5C1D1E1F1DDE1FDE1EB08D9E3DDE3FDE3EDA0EDB0EDA8EDB8EDA1EDB1EDA9EDB980818283848587C601C602C6FEC6FFC601C602C6FEC6FF86DD8600DD8601DD86FEDD86FDDD8600DD8601DD86FEDD86FDFD8600FD8601FD86FEFD86FDFD8600FD8601FD86FEFD86FD88898A8B8C8D8FCE01CE02CEFECEFFCE01CE02CEFECEFF8EDD8E00DD8E01DD8EFEDD8EFDDD8E00DD8E01DD8EFEDD8EFDFD8E00FD8E01FD8EFEFD8EFDFD8E00FD8E01FD8EFEFD8EFD90919293949597D601D602D6FED6FFD601D602D6FED6FF96DD9600DD9601DD96FEDD96FDDD9600DD9601DD96FEDD96FDFD9600FD9601FD96FEFD96FDFD9600FD9601FD96FEFD96FD98999A9B9C9D9FDE01DE02DEFEDEFFDE01DE02DEFEDEFF9EDD9E00DD9E01DD9EFEDD9EFDDD9E00DD9E01DD9EFEDD9EFDFD9E00FD9E01FD9EFEFD9EFDFD9E00FD9E01FD9EFEFD9EFDA0A1A2A3A4A5A7E601E602E6FEE6FFE601E602E6FEE6FFA6DDA600DDA601DDA6FEDDA6FDDDA600DDA601DDA6FEDDA6FDFDA600FDA601FDA6FEFDA6FDFDA600FDA601FDA6FEFDA6FDB0B1B2B3B4B5B7F601F602F6FEF6FFF601F602F6FEF6FFB6DDB600DDB601DDB6FEDDB6FDDDB600DDB601DDB6FEDDB6FDFDB600FDB601FDB6FEFDB6FDFDB600FDB601FDB6FEFDB6FDA8A9AAABACADAFEE01EE02EEFEEEFFEE01EE02EEFEEEFFAEDDAE00DDAE01DDAEFEDDAEFDDDAE00DDAE01DDAEFEDDAEFDFDAE00FDAE01FDAEFEFDAEFDFDAE00FDAE01FDAEFEFDAEFDB8B9BABBBCBDBFFE01FE02FEFEFEFFFE01FE02FEFEFEFFBEDDBE00DDBE01DDBEFEDDBEFDDDBE00DDBE01DDBEFEDDBEFDFDBE00FDBE01FDBEFEFDBEFDFDBE00FDBE01FDBEFEFDBEFD040C141C242C3C34DD3400DD3401DD34FEDD34FDDD3400DD3401DD34FEDD34FDFD3400FD3401FD34FEFD34FDFD3400FD3401FD34FEFD34FD050D151D252D3D35DD3500DD3501DD35FEDD35FDDD3500DD3501DD35FEDD35FDFD3500FD3501FD35FEFD35FDFD3500FD3501FD35FEFD35FD272FED443F370076F3FBED46ED56ED5E09192939ED4AED5AED6AED7AED42ED52ED62ED72DD09DD19DD29DD39FD09FD19FD29FD3903132333DD23FD230B1B2B3BDD2BFD2B07170F1FCB00CB01CB02CB03CB04CB05CB07CB06DDCB0006DDCB0106DDCBFE06DDCBFF06DDCB0006DDCB0106DDCBFE06DDCBFF06FDCB0006FDCB0106FDCBFE06FDCBFF06FDCB0006FDCB0106FDCBFE06FDCBFF06CB10CB11CB12CB13CB14CB15CB17CB16DDCB0016DDCB0116DDCBFE16DDCBFF16DDCB0016DDCB0116DDCBFE16DDCBFF16FDCB0016FDCB0116FDCBFE16FDCBFF16FDCB0016FDCB0116FDCBFE16FDCBFF16CB08CB09CB0ACB0BCB0CCB0DCB0FCB0EDDCB000EDDCB010EDDCBFE0EDDCBFF0EDDCB000EDDCB010EDDCBFE0EDDCBFF0EFDCB000EFDCB010EFDCBFE0EFDCBFF0EFDCB000EFDCB010EFDCBFE0EFDCBFF0ECB18CB19CB1ACB1BCB1CCB1DCB1FCB1EDDCB001EDDCB011EDDCBFE1EDDCBFF1EDDCB001EDDCB011EDDCBFE1EDDCBFF1EFDCB001EFDCB011EFDCBFE1EFDCBFF1EFDCB001EFDCB011EFDCBFE1EFDCBFF1ECB20CB21CB22CB23CB24CB25CB27CB26DDCB0026DDCB0126DDCBFE26DDCBFF26DDCB0026DDCB0126DDCBFE26DDCBFF26FDCB0026FDCB0126FDCBFE26FDCBFF26FDCB0026FDCB0126FDCBFE26FDCBFF26CB28CB29CB2ACB2BCB2CCB2DCB2FCB2EDDCB002EDDCB012EDDCBFE2EDDCBFF2EDDCB002EDDCB012EDDCBFE2EDDCBFF2EFDCB002EFDCB012EFDCBFE2EFDCBFF2EFDCB002EFDCB012EFDCBFE2EFDCBFF2ECB38CB39CB3ACB3BCB3CCB3DCB3FCB3EDDCB003EDDCB013EDDCBFE3EDDCBFF3EDDCB003EDDCB013EDDCBFE3EDDCBFF3EFDCB003EFDCB013EFDCBFE3EFDCBFF3EFDCB003EFDCB013EFDCBFE3EFDCBFF3EED6FED67CB40CB49CB52CB5BCB64CB6DCB77CB7FCB40CB49CB52CB5BCB64CB6DCB77CB7FCB46CB4ECB56CB5ECB66CB6ECB76CB7ECB46CB4ECB56CB5ECB66CB6ECB76CB7EDDCBFF46DDCBFE4EDDCBFD56DDCB045EDDCB0366DDCB026EDDCB0176DDCB007EDDCBFF46DDCBFE4EDDCBFD56DDCB045EDDCB0366DDCB026EDDCB0176DDCB007EFDCBFF46FDCBFE4EFDCBFD56FDCB045EFDCB0366FDCB026EFDCB0176FDCB007EFDCBFF46FDCBFE4EFDCBFD56FDCB045EFDCB0366FDCB026EFDCB0176FDCB007ECBC0CBC9CBD2CBDBCBE4CBEDCBF7CBFFCBC0CBC9CBD2CBDBCBE4CBEDCBF7CBFFCBC6CBCECBD6CBDECBE6CBEECBF6CBFECBC6CBCECBD6CBDECBE6CBEECBF6CBFEDDCBFFC6DDCBFECEDDCBFDD6DDCB04DEDDCB03E6DDCB02EEDDCB01F6DDCB00FEDDCBFFC6DDCBFECEDDCBFDD6DDCB04DEDDCB03E6DDCB02EEDDCB01F6DDCB00FEFDCBFFC6FDCBFECEFDCBFDD6FDCB04DEFDCB03E6FDCB02EEFDCB01F6FDCB00FEFDCBFFC6FDCBFECEFDCBFDD6FDCB04DEFDCB03E6FDCB02EEFDCB01F6FDCB00FECB80CB89CB92CB9BCBA4CBADCBB7CBBFCB80CB89CB92CB9BCBA4CBADCBB7CBBFCB86CB8ECB96CB9ECBA6CBAECBB6CBBECB86CB8ECB96CB9ECBA6CBAECBB6CBBEDDCBFF86DDCBFE8EDDCBFD96DDCB049EDDCB03A6DDCB02AEDDCB01B6DDCB00BEDDCBFF86DDCBFE8EDDCBFD96DDCB049EDDCB03A6DDCB02AEDDCB01B6DDCB00BEFDCBFF86FDCBFE8EFDCBFD96FDCB049EFDCB03A6FDCB02AEFDCB01B6FDCB00BEFDCBFF86FDCBFE8EFDCBFD96FDCB049EFDCB03A6FDCB02AEFDCB01B6FDCB00BEC30100C3FF00C30001C300FFC30100C3FF00C30001C300FFC20100CAFF00D20001DA00FFE20100EAFF00F20001FA00FF180018A81852180618AC1856380038A83852380638AC3856300030A83052300630AC3056E9DDE9FDE9100010A81052100610AC1056CD0100CDFE00CD0001CD00FFCD0100CDFE00CD0001CD00FFC40100CCFE00D40001DC00FFE40100ECFE00F40001FC00FFC9C0C8D0D8E0E8F0F8ED4DED45C7CFD7DFE7EFF7FFDB00DB01DBFEDBFFDB00DB01DBFEDBFFED40ED48ED50ED58ED60ED68ED78EDA2EDB2EDAAEDBAD300D301D3FED3FFD300D301D3FED3FFED41ED49ED51ED59ED61ED69ED79EDA3EDB3EDABEDBBCB30CB31CB32CB33CB34CB35CB37CB36", assembly.ToString() );
    }

    [TestMethod]
    public void TestAssemblyZ80Opcodes2()
    {
      string      source = @"!cpu z80

                    * = $00

                      ld A,($0100)              ;3A0001
                      ;ld A,($ff00)              ;3A00FF
                      ;ld A,($bbcc)              ;3ACCBB

                      LATE_0100 = $0100";

      var assembly = TestAssembleC64Studio( source );
      Assert.AreEqual( "00003A0001", assembly.ToString() );
    }

  }
}
