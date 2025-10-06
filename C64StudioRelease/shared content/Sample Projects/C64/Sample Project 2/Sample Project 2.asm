;C64Studio - Sample Project #2
;
;creates a BASIC stub, display a text on screen and returns to BASIC



;the memory location we want to write the string to
;The C64 has the default screen at $0400
SCREEN_POS = $0400


      ;this line sets the start address (for C64 it's $0801)
* = $0801

      ;this line automatically creates a BASIC stub, (10 SYS <Start Address>)
!basic

      ;initialise the counter to 0
      ldx #0

-
      ;read the next character
      lda TEXT_LINE,x

      ;if it's zero, we reached the end of text byte mark
      beq .EndReached


      ;store the character on the screen
      sta SCREEN_POS,x

      ;increase counter
      inx

      ;go for the next character
      jmp -

.EndReached
      ;return from subroutine, returns to BASIC here
      rts


;the text we want to display, the label is used to reference it above
TEXT_LINE

;the !src directive stores the text in screen code, so we can directly copy it to the screen
;the 0 stores an addition 0 byte at the end which is used as end of text mark
      !scr "hello world",0