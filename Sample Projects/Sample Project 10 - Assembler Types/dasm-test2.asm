processor   6502

xyz SET 1105
xyz2 SET 1106

MAC GNI
        lda #5
        sta 1064
ENDM

ORG $2000



REPEAT 3
        lda #1
        sta 1025
:GNI
        lda #2
        sta xyz
REPEND

HULLO
        lda #0
        sta 1024

REPEAT 3
        lda #1
        sta xyz2
REPEND

        rts