* = $0801

!basic

LABEL_POS = $2000

; add immediate 16bit value to memory 
!macro add16im .dest, .val { 
lda #<.val 
clc 
adc .dest 
sta .dest 
lda #>.val 
adc .dest+1 
sta .dest+1 
} 


+add16im LABEL_POS, 256

+add16im LABEL_POS, 1717
rts 