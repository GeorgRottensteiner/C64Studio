* = 16384
          inc $d020
          ldx #0
          lda #1
ANOTHER_LABEL          
          sta $0400,x
          inx
          bne ANOTHER_LABEL
          rts