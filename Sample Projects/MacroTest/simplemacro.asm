*=$0801
;!basic


;!list off

!macro name {

  lda #0
  beq +
   lda #1
   jmp ++
  +
   lda #2
  ++


} ;!end

;!list on


+name