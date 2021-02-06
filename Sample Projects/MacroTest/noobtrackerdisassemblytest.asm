*=$0801
!basic
jsr getnewblock
nop
jmp *

rts; --- breakpoint ---
getnewblock
lda empty
ldx #0
stx setnewemptybit
dex
sec
loop 
 rol setnewemptybit
 lsr 
 inx 
 bcc loop
setnewemptybit=*+1
lda #0;placeholder
eor empty
sta empty
rts
empty
!byte %11111100
