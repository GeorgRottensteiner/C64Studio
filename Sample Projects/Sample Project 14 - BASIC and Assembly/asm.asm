* = 16384
ASM_START
          inc $d020
          ldx #0
          ldy #1
          lda #1
ANOTHERLABEL
          sta $0400,x
          inx
          bne ANOTHERLABEL
          rts


TEXT
          !text "what? me worry?"