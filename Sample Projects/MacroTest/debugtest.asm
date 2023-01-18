;*= $0801

;!basic



* = $2000

fgna
clc
lda #6
adc #3 ; Breakpoint hier!
adc #250
adc #$A

jsr test

.gnu

ldx #0
-
inx

stx $d020
lsmf
bne -


jmp fgna
rts




!source "debugtest2.asm"