* = $0801

!basic
          jmp STEP

!src "multiinclude_include.asm"


STEP
          lda #1
          sta $d020

          jmp STEP2




!src "multiinclude_include2.asm"


STEP2
          lda #2
          sta $d020

          jmp *
