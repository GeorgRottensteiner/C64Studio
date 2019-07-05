SCREEN_CHAR = $0400
BITMAP_LOCATION = $c000

* = $2000
@CopyLoop
          lda $4000 + 0 * 250,X
          sta SCREEN_CHAR + 0 * 250,X
          lda $4000 + 1 * 250,X
          sta SCREEN_CHAR + 1 * 250,X
          lda $4000 + 2 * 250,X
          sta SCREEN_CHAR + 2 * 250,X
          lda $4000 + 3 * 250,X
          sta SCREEN_CHAR + 3 * 250,X

          lda $4400 + 0 * 250,X
          sta $D800 + 0 * 250,X
          lda $4400 + 1 * 250,X
          sta $D800 + 1 * 250,X
          lda $4400 + 2 * 250,X
          sta $D800 + 2 * 250,X
          lda $4400 + 1 * 250,X
          sta $D800 + 1 * 250,X

!for ROW = 0 TO 31
          lda $2000 + ROW * 250,x
          sta BITMAP_LOCATION + ROW * 250,x
!end
          inx
          cpx #250
          beq @Done
          jmp @CopyLoop

@Done     lda @CopyLoop

EndlessLoop
          jmp EndlessLoop