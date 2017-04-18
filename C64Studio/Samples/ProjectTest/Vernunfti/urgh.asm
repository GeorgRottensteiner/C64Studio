!to "urgh.prg",cbm

* = $0801

ldx #0
ldy #0
lp_1:
lda $d012
cmp #255
bne lp_1
lp_11:
lda $d012
cmp #250
bne lp_11
lda #42
sta $0400,x
sta $04ff,x
sta $05f9,x
sta $06e8,x
inx
iny
cpy #8
bne lp_2
ldy #0
lp_2:
sty $d016
jmp lp_1
