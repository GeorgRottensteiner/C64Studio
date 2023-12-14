* = $2000

a = 5
!while a > 0 {
          !byte a
          a = a - 1
}

SCREEN_CHAR = $0400
a = 5
!while a > 0 {
        lda #a
        sta SCREEN_CHAR + a
        a = a - 1
      }
