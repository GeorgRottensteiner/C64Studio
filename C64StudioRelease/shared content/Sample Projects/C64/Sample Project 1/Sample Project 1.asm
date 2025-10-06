;C64Studio - Sample Project #1
;
;creates a BASIC stub, sets the color of the border to white and returns to BASIC



      ;this line sets the start address (for C64 it's $0801)
* = $0801

      ;this line automatically creates a BASIC stub, (10 SYS <Start Address>)
!basic

      ;load the literal number 1 into the A register
      lda #1

      ;store the value of the A register into the VIC border color register
      sta $d020

      ;return from subroutine, returns to BASIC here
      rts