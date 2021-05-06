* = $0801



!byte run/1000 % 10 + '0'

!basic

run
        lda #7
        sta $d020
        rts