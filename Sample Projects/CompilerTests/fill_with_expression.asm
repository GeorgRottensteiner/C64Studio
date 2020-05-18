* = $0801

!basic

          ldx #0
-          
          lda CHAR_TABLE,x
          sta $0400,x
          
          inx
          cpx #5
          bne -
          
          rts



CHAR_TABLE
          !fill 5,[i * 8]+[i*8]