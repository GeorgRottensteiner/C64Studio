ASM_START = $4000
* = ASM_START
          ;inc background color
          inc $d020

          ;fill screen top with 256 A's
          ldx #0
          ldy #1
          lda #1
ANOTHERLABEL
          sta $0400,x
          inx
          cpx #40
          bne ANOTHERLABEL

          rts


TEXT
          !text "byte;immerich"