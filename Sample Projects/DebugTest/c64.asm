*=$0801
!basic
            ldx #21
  .loop     txa
            sta $1e00,x
            lda #0
            sta $9600,x
            dex
            bpl .loop
            rts