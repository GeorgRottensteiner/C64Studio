!to "formain.prg",cbm


* = $2000
          lda #0
          sta 53280

!source "forsub.asm"


END_OF_FOR
          lda #1
          sta 53280
          rts                 