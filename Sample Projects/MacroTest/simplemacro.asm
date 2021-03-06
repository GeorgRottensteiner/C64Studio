*=$0801
!basic
!macro name
  lda #0
  beq +
   lda #1
   jmp ++
  +
   lda #2
  ++
!end
+name