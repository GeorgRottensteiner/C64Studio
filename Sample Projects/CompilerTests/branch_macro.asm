* = $2000

!macro jne label
!if ( ( label - *) > 127 ) or ( ( * - label ) < ( -127 ) ) { 
 beq *+5 
 jmp label 
 } else { 
 bne label 
 } 
 !end 
 
.BG = $200b 
 
          lda #1
          +jne .BG
          sta $d020
          rts
          
;.BG
          sta $d021
          rts