!cpu 65816

        !to "cpu_65816.prg",plain

        * = $1000

        ; all the standard 6502 instructions:
        brk   ; 00
        ora ($01, x)  ; 01
        ora $05   ; 05
        asl $05   ; 06
        php   ; 08
        ora #$09  ; 09
        asl   ; 0a
        ora $0d0e ; 0d
        asl $0d0e ; 0e
        bpl * + 2 ; 10
        ora ($11), y  ; 11
        ora $15, x  ; 15
        asl $15, x  ; 16
        clc   ; 18
        ora $1919, y  ; 19
        ora $1d1e, x  ; 1d
        asl $1d1e, x  ; 1e
        jsr $0d0e ; 20
        and ($01, x)  ; 21
        bit $05   ; 24
        and $05   ; 25
        rol $05   ; 26
        plp   ; 28
        and #$09  ; 29
        rol   ; 2a
        bit $0d0e ; 2c
        and $0d0e ; 2d
        rol $0d0e ; 2e
        bmi * + 2 ; 30
        and ($11), y  ; 31
        and $15, x  ; 35
        rol $15, x  ; 36
        sec   ; 38
        and $1919, y  ; 39
        and $1d1e, x  ; 3d
        rol $1d1e, x  ; 3e
        rti   ; 40
        eor ($01, x)  ; 41
        eor $05   ; 45
        lsr $05   ; 46
        pha   ; 48
        eor #$09  ; 49
        lsr   ; 4a
        jmp $0d0e ; 4c
        eor $0d0e ; 4d
        lsr $0d0e ; 4e
        bvc * + 2 ; 50
        eor ($11), y  ; 51
        eor $15, x  ; 55
        lsr $15, x  ; 56
        cli   ; 58
        eor $1919, y  ; 59
        eor $1d1e, x  ; 5d
        lsr $1d1e, x  ; 5e
        rts   ; 60
        adc ($01, x)  ; 61
        adc $05   ; 65
        ror $05   ; 66
        pla   ; 68
        adc #$09  ; 69
        ror   ; 6a
        jmp ($6c6c) ; 6c
        adc $0d0e ; 6d
        ror $0d0e ; 6e
        bvs * + 2 ; 70
        adc ($11), y  ; 71
        adc $15, x  ; 75
        ror $15, x  ; 76
        sei   ; 78
        adc $1919, y  ; 79
        adc $1d1e, x  ; 7d
        ror $1d1e, x  ; 7e
        sta ($01, x)  ; 81
        sty $05   ; 84
        sta $05   ; 85
        stx $05   ; 86
        dey   ; 88
        txa   ; 8a
        sty $0d0e ; 8c
        sta $0d0e ; 8d
        stx $0d0e ; 8e
        bcc * + 2 ; 90
        sta ($11), y  ; 91
        sty $15, x  ; 94
        sta $15, x  ; 95
        stx $96, y  ; 96
        tya   ; 98
        sta $1919, y  ; 99
        txs   ; 9a
        sta $1d1e, x  ; 9d
        ldy #$09  ; a0
        lda ($01, x)  ; a1
        ldx #$09  ; a2
        ldy $05   ; a4
        lda $05   ; a5
        ldx $05   ; a6
        tay   ; a8
        lda #$09  ; a9
        tax   ; aa
        ldy $0d0e ; ac
        lda $0d0e ; ad
        ldx $0d0e ; ae
        bcs * + 2 ; b0
        lda ($11), y  ; b1
        ldy $15, x  ; b4
        lda $15, x  ; b5
        ldx $96, y  ; b6
        clv   ; b8
        lda $1919, y  ; b9
        tsx   ; ba
        ldy $1d1e, x  ; bc
        lda $1d1e, x  ; bd
        ldx $1919, y  ; be
        cpy #$09  ; c0
        cmp ($01, x)  ; c1
        cpy $05   ; c4
        cmp $05   ; c5
        dec $05   ; c6
        iny   ; c8
        cmp #$09  ; c9
        dex   ; ca
        cpy $0d0e ; cc
        cmp $0d0e ; cd
        dec $0d0e ; ce
        bne * + 2 ; d0
        cmp ($11), y  ; d1
        cmp $15, x  ; d5
        dec $15, x  ; d6
        cld   ; d8
        cmp $1919, y  ; d9
        cmp $1d1e, x  ; dd
        dec $1d1e, x  ; de
        cpx #$09  ; e0
        sbc ($01, x)  ; e1
        cpx $05   ; e4
        sbc $05   ; e5
        inc $05   ; e6
        inx   ; e8
        sbc #$09  ; e9
      !ifndef M65 {
        nop   ; ea  (m65 re-uses this opcode as a prefix)
      }
        cpx $0d0e ; ec
        sbc $0d0e ; ed
        inc $0d0e ; ee
        beq * + 2 ; f0
        sbc ($11), y  ; f1
        sbc $15, x  ; f5
        inc $15, x  ; f6
        sed   ; f8
        sbc $1919, y  ; f9
        sbc $1d1e, x  ; fd
        inc $1d1e, x  ; fe

        ; extensions in CMOS re-design (65c02):
        tsb $04   ; 04
        tsb $0c0c ; 0c
        ora ($12) ; 12
        trb $04   ; 14
        inc   ; 1a
        trb $0c0c ; 1c
        and ($12) ; 32
        bit $34, x  ; 34
        dec   ; 3a
        bit $3c3c, x  ; 3c
        eor ($12) ; 52
        phy   ; 5a
        stz $04   ; 64
        adc ($12) ; 72
        stz $34, x  ; 74
        ply   ; 7a
        jmp ($7c7c, x)  ; 7c
        bra * + 2 ; 80
        bit #$89  ; 89
        sta ($12) ; 92
        stz $0c0c ; 9c
        stz $3c3c, x  ; 9e
        lda ($12) ; b2
        cmp ($12) ; d2
        phx   ; da
        sbc ($12) ; f2
        plx   ; fa

        ; new instructions of 65816:
        cop $02     ; 02
        ora $03, s  ; 03
        ora [$07] ; 07
        phd   ; 0b
        ora $0f0f0f ; 0f
        ora ($13, s), y ; 13
        ora [$17], y  ; 17
        tcs   ; 1b
        ora $1f1f1f, x  ; 1f
        jsr $0f0f0f ; 22
        and $03, s  ; 23
        and [$07] ; 27
        pld   ; 2b
        and $0f0f0f ; 2f
        and ($13, s), y ; 33
        and [$17], y  ; 37
        tsc   ; 3b
        and $1f1f1f, x  ; 3f
        wdm   ; 42
        eor $03, s  ; 43
        mvp $44, $54  ; 44
        eor [$07] ; 47
        phk   ; 4b
        eor $0f0f0f ; 4f
        eor ($13, s), y ; 53
        mvn $44, $54  ; 54
        eor [$17], y  ; 57
        tcd   ; 5b
        jmp $0f0f0f ; 5c
        eor $1f1f1f, x  ; 5f
        per * + 3 ; 62
        adc $03, s  ; 63
        adc [$07] ; 67
        rtl   ; 6b
        adc $0f0f0f ; 6f
        adc ($13, s), y ; 73
        adc [$17], y  ; 77
        tdc   ; 7b
        adc $1f1f1f, x  ; 7f
        brl * + 3 ; 82
        sta $03, s  ; 83
        sta [$07] ; 87
        phb   ; 8b
        sta $0f0f0f ; 8f
        sta ($13, s), y ; 93
        sta [$17], y  ; 97
        txy   ; 9b
        sta $1f1f1f, x  ; 9f
        lda $03, s  ; a3
        lda [$07] ; a7
        plb   ; ab
        lda $0f0f0f ; af
        lda ($13, s), y ; b3
        lda [$17], y  ; b7
        tyx   ; bb
        lda $1f1f1f, x  ; bf
        rep #$c2  ; c2, see below
        cmp $03, s  ; c3
        cmp [$07] ; c7
        wai   ; cb
        cmp $0f0f0f ; cf
        cmp ($13, s), y ; d3
        pei ($d4) ; d4
        cmp [$17], y  ; d7
        stp   ; db
        jmp [$dcdc] ; dc
        cmp $1f1f1f, x  ; df
        sep #$e2  ; e2, see below
        sbc $03, s  ; e3
        sbc [$07] ; e7
        xba   ; eb
        sbc $0f0f0f ; ef
        sbc ($13, s), y ; f3
        pea $f4f4 ; f4
        sbc [$17], y  ; f7
        xce   ; fb
        jsr ($fcfc, x)  ; fc
        sbc $1f1f1f, x  ; ff

    ; check sizes of immediate arguments:
    !macro immediates {
            ; arg size depends on:
    ; from 6502:
        ora #$09  ; 09  accumulator size
        and #$09  ; 29  accumulator size
        eor #$09  ; 49  accumulator size
        adc #$09  ; 69  accumulator size
        ldy #$09  ; a0  index register size
        ldx #$09  ; a2  index register size
        lda #$09  ; a9  accumulator size
        cpy #$09  ; c0  index register size
        cmp #$09  ; c9  accumulator size
        cpx #$09  ; e0  index register size
        sbc #$09  ; e9  accumulator size
    ; from 65c02:
        bit #$89  ; 89  accumulator size
    ; from 65816:
        rep #$c2  ; c2  always 8 bits
        sep #$e2  ; e2  always 8 bits
    }
        ; before this, all sizes were 8 bits
        !al
        +immediates ; now repeat immediates with long accumulator
        !as
        !rl
        +immediates ; repeat immediates with short A and long index regs
        !al
        !rl
        +immediates ; repeat immediates with long A and long index regs