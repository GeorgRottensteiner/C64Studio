;to set the proper CPU
!cpu m65

;to include VIC constants
!source <c64.asm>

;to include Mega65 constants VIC3 and VIC4
!source <mega65.asm>


;Mega65s default sprite pointer address
SPRITE_POINTER_BASE = $0ff8



* = $2001

          ;this must follow after !cpu m65, because it adds a BANK to the BASIC upstart
          !basic

          sei

          +EnableVIC4Registers

          ;Turn off CIA interrupts
          lda #$7f
          sta CIA1.IRQ_CONTROL
          sta CIA2.NMI_CONTROL

          ;Turn off raster interrupts, used by C65 rom
          lda #$00
          sta VIC.IRQ_MASK

          cli

          lda #1
          tsb VIC.SPRITE_ENABLE
          sta VIC.SPRITE_COLOR

          lda #150
          sta VIC.SPRITE_X_POS
          sta VIC.SPRITE_Y_POS

          ;enable wide sprite mode (now we're having 64x21 pixel)
          lda #$01
          tsb VIC4.SPRX64EN

          lda #SPRITE_DATA / 64
          sta SPRITE_POINTER_BASE

          ;endless loop
          jmp *

;sprite data must start at a multiple of 64
!realign 64
SPRITE_DATA
          !media "Sprites 64x21.spriteproject",SPRITE,0,1
