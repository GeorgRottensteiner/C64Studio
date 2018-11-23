* = $0801

!basic

gnu: inc $d020 : dec $d020


label:  lda #0
        sta $d020
        rts
        
label2        