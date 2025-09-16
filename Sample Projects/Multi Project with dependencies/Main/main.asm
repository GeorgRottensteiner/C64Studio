* = $0801

!basic

          lda #$0
          sta 53280
          sta 53281
          rts


!lzone DrawChar
          lda #$1
          sta $0400
          rts