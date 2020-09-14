*=$080d  
  
  lda #$01
  asl ; *2
  asl ; *4
  asl ; *8
  asl ; *16
  tax
  lda GameStrings,x
  sta $0400
  nop
  
  
  rts
  
;*=$0900  
!realign $10
GameStrings
  !scr "hello"
  !realign $10
  !scr "this"
  !realign $10
  !scr "you!"