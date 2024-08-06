* = $0801

!zone Map

.Height = 11


!for y = 0 to .Height - 1
  !word y * 229 * .Height
!end




lda #<-2 * 256