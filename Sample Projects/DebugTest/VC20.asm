*=$1001
!byte $0b,$08, $e2,$07, $9e, $20, $34, $31, $32, $38, $00,$00, $00
*=$1020
            ldx #21
  .loop     txa
            sta $1e00,x
            lda #0
            sta $9600,x
            dex
            bpl .loop
            rts