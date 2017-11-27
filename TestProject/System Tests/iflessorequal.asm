RED=2
GREEN=5


*=$0801

!BASIC


!IF 5<=2 {

  lda #GREEN

} else {

  lda #RED

}

  sta $D021

  rts