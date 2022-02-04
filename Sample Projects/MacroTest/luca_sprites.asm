
* = $0801

!basic

lda #1
sta $d015 ; enable sprite 0
lda #80
sta $d000 ; sprite 0 x
sta $d001 ; sprite 0 y
lda #7
sta $d027 ; sprite 0 color
lda #128
sta $07f8 ; sprite 0 frame (data block)
rts

* = $2000 - 64
!media "sss.spriteproject", sprite, 0, 2
