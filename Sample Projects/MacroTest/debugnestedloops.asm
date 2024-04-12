*= $0801

!basic
          jmp Work




!lzone Work

          lda #1
          lda #2
          lda #3

          jsr Scroll

          lda #4
          lda #5
          lda #6

          rts



!src "debugnestedloops2.asm"


          lda #1
          lda #2
          lda #3
