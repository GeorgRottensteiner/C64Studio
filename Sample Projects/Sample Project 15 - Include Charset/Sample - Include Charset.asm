* = $0801

!basic
          ;set charset to $2000
          lda #$19
          sta $D018
          
          lda #0
          ldx #0
-          
          sta 1024,x
          inx
          cpx #40
          bne -

          rts

* = $2000
!media "Charset.charsetproject",CHAR