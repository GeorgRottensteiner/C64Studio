* = $2000

GREEN = 5
RED = 2

TEST2

!IFDEF TEST=2 {

lda #GREEN

} else {

lda #RED

}

          sta 53280
          rts