;; ----
;; For easier testing, I added the following two lines for a BASIC launcher so it can auto-run from C64Studio. (But this only matters when the code can actually compile without an error.)
*=$0801
!basic $2000
;; ----
* = $2000

!macro jne label
!if ( ( label - *) > 127 ) or ( ( * - label ) < ( -127 ) ) { 
 beq *+5 
 jmp label 
 } else { 
 bne label 
 } 
 !end 
 
;; ----
;; Removed the following line by commenting it out:
;; .BG = $200b 
;; The correct address for the .BG label (ideally defined way at the bottom) is currently at $2008 instead of $200b noted in the line above:
;.BG = $2008
;; ----
 
          lda #1
          ;+jne .BG
          
 !if ( ( .BG - *) > 127 ) or ( ( * - .BG ) < ( -127 ) ) { 
 beq *+5 
 jmp .BG 
 } else { 
 bne .BG 
 } 
          sta $d020
          rts
          
;; ----
;; Added the following line back in by uncommenting it -- this causes the E1001 error, which seems like a bug:
;; ;.BG
.BG
;; ----
          sta $d021
          rts