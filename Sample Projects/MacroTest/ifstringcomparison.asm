RED=2
GREEN=5

VARIABLE='def' ; works ok

*=$0801



;doesn't work
!if(VARIABLE='abc'){
  lda #GREEN

} else {

  lda #RED

}

  sta $D021

  rts

!text VARIABLE