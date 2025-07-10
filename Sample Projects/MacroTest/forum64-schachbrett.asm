
*= $c000
        jsr $abf9       ; print a "?" and wait for input
        lda $0200       ; value of 1st character from input buffer
        ;and #$40        ; same as -64
        clc             ; prepare addition (can be removed)
        adc $0201       ; value of 2nd character from input buffer
        ;sec             ; prepare substraction (can be removed)
        ;sbc #$31        ;
        and #$01        ; even or odd?
        beq +
        lda #<text      ; if odd 1, 3, 5, 7 low byte of textpos
        !byte $2c       ; skip (bit $xxxx)
+       lda #<text      ; if even 2, 4, 6, 8 low byte of textpos
        ldy #>text      ; high byte of textpos
        jmp $ab1e       ; print text and exit

text
        !text "DARK",0
        !pet"light"
        !byte $00       ; endmark for text2