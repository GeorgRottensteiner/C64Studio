* = $0400

SCREEN_CHAR 
          !byte ?
SCREEN_CHAR_2          
          !byte ?
SCREEN_CHAR_3
          !word ?

* = $0801

!basic
          lda #1
          sta SCREEN_CHAR
          rts
          
          ;!byte ?
          
          
!byte 2          