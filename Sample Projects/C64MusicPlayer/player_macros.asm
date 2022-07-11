  ;------------------------------------------------------------------------------------
  ; 
  ; Please note this was mostly to learn the process 
  ;   also note O'Dog of Laxity you and your group are twats
  ; 
  ;------------------------------------------------------------------------------------


; macro to peek at the current command
!macro PeekCommand VAD
  lda VAD,x 
  sta VPEEK 
  lda VAD+1,x 
  sta VPEEK+1
  ldy #$00 
  lda (VPEEK),y
!end

; bump the command pointer
!macro NextCommand VAD
  inc VAD,x
  bne +
  inc VAD+1,x
+
!end