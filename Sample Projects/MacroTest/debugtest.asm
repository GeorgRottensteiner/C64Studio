*= $0801

!basic
          jmp fgna

DUMMY = $1000



* = $2000

fgna
clc
lda #6
adc #3 ; Breakpoint hier!
adc #250
          adc #$A
sta $1000

jsr test

.gnu



ldx #0
-
stx $d020
inx


lsmf
bne -



jmp fgna
rts


!realign $100

!source "debugtest2.asm"