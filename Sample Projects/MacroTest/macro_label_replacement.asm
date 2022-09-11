!macro meins wert

        jmp label##wert

        lda #2
        sta $d020

label##wert
        lda #wert
        sta $d021
!end


* = $0801

!basic



!for row = 7 to 8
        +meins row
!end
        rts


