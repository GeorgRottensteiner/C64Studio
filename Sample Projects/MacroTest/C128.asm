* = $1c01

!basic

          lda #1
          sta $0400
          ;rts

STEP
          lda #1
          sta $d020

          jmp STEP2




!src "multiinclude_include2.asm"


STEP2
          lda #2
          sta $d020

          jmp *