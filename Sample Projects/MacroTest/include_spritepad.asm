* = $0801

!src <c64.asm>

!basic
          lda #100
          sta VIC.SPRITE_X_POS
          sta VIC.SPRITE_Y_POS

          lda #128
          sta 2040

          lda #1
          sta VIC.SPRITE_ENABLE
          rts



!align 63,0
!media "end_sprites.spd",SPRITE,0,4