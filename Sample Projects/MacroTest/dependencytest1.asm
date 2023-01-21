* = $0801

!src "dependencytest2.asm"

!basic
          lda MAIN_COLOR
          sta $d020
          rts
