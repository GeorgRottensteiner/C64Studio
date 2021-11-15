;to set the proper CPU
!cpu m65

;to include VIC constants
!source <c64.asm>

;to include Mega65 constants VIC3 and VIC4
!source <mega65.asm>


SCREEN_CHAR   = $0800

;for 40x25 the lower 1000 bytes of the color RAM are mapped here
SCREEN_COLOR  = $d800


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

          ;80 bytes per screen line
          lda #<80
          sta VIC4.CHARSTEP_LO
          lda #>80
          sta VIC4.CHARSTEP_HI

          ;Turn on FCM mode (and 16bit per char number)
          lda #$07
          sta VIC4.VIC4DIS

          lda #0
          sta VIC.BACKGROUND_COLOR

          ldx #0
          ldy #0
-
          ;we need to add offset to the character data
          lda TEXT_SCREEN_DATA,x
          clc
          adc #TILE_DATA / 64
          sta SCREEN_CHAR,y
          lda #0

          iny

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

!media "Text Screen.charscreen",CHARSET,0,1
        ;!byte 7,7,7,7,7,7,7,7
        ;!byte 7,3,3,3,3,3,3,3
        ;!byte 7,3,7,7,7,7,7,7
        ;!byte 7,3,7,3,3,3,3,3
        ;!byte 7,3,7,3,3,3,3,3
        ;!byte 7,3,7,3,3,3,3,3
        ;!byte 7,3,7,3,3,3,3,3
        ;!byte 7,3,7,3,3,3,3,3

