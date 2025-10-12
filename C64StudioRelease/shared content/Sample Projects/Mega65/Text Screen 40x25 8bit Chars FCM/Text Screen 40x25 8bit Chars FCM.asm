;to set the proper CPU
!cpu m65

;to include VIC constants
!source <c64.asm>

;to include Mega65 constants VIC3 and VIC4
!source <mega65.asm>


SCREEN_CHAR   = $0800

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

          ;set 40x25 mode
          lda #$80
          trb VIC3.VICDIS

          ;Turn on FCM mode (not 16bit per char number)
          lda #$06
          sta VIC4.VIC4DIS

          ;number of chars per row
          lda #40
          sta VIC4.CHRCOUNT

          lda #0
          sta VIC.BACKGROUND_COLOR

          ldx #0
-
          ;we need to add offset to the character data
          lda TEXT_SCREEN_DATA,x
          clc
          adc #TILE_DATA / 64
          sta SCREEN_CHAR,x
          lda TEXT_SCREEN_DATA + 1 * 250,x
          clc
          adc #TILE_DATA / 64
          sta SCREEN_CHAR + 1 * 250,x
          lda TEXT_SCREEN_DATA + 2 * 250,x
          clc
          adc #TILE_DATA / 64
          sta SCREEN_CHAR + 2 * 250,x
          lda TEXT_SCREEN_DATA + 3 * 250,x
          clc
          adc #TILE_DATA / 64
          sta SCREEN_CHAR + 3 * 250,x

          inx
          cpx #250
          bne -

          ;endless loop
          jmp *



TEXT_SCREEN_DATA
          ;contains 40x25 character bytes
          !media "Text Screen.charscreen",CHAR


* = $3000
TILE_DATA

          ;this statement includes the charset data of 2 FCM characters (2 * 8x8 = 128 bytes)
          !media "Text Screen.charscreen",CHARSET,0,2
