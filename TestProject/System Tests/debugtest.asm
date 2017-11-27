*= $2000 ;start with SYS8192 (8192 = $2000)

clc
lda #6
adc #3 ; Breakpoint hier!
adc #250
adc #$A
rts