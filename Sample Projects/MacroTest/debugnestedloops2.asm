!lzone Scroll

          lda #1
          lda #2
          lda #3

!for y=12 to 13
          ldy $0400 + y * 40
!for i=1 to 5
          lda $0400 + ( y * 40 ) + i
          sta $0400 + ( y * 40 ) + i - 1
!end
          sty $0400 + y * 40 + 39
!end

          lda #4
          lda #5
          lda #6
          lda #7
          lda #8
          rts
