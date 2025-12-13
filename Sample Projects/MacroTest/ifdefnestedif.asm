*=$0801
!basic

!list off

!macro poke address, value {
  lda #value
  sta address
}

!list on


rts