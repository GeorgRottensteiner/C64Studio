;inner for loop "forgets" to shift zones (source infos?)

;!for dx=20 to 0 step - 1
!for dx=0 to 1
 !for dy=0 to 1
 lda $4000+dx+dy
 ;ldx #dx
 ;ldy #dy
 sta $6000+dx-dy
 !end
 !end
 
oh nop
 
!zone Hinz2
Hinz2
.Kunz2
