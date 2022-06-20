;to set the proper CPU
!cpu m65

;to include VIC constants
!source <c64.asm>

;to include Mega65 constants VIC3 and VIC4
!source <mega65.asm>


SCREEN_CHAR   = $0800

;for 40x25 the lower 1000 bytes of the color RAM are mapped here
SCREEN_COLOR  = $d800

;we use 16bit characters
ROW_SIZE_BYTES    = 40 * 2


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

          ;Turn on FCM mode and 16bit per char number
          lda #$07
          sta VIC4.VIC4DIS

          ;Set logical row width
          ;bytes per screen row (16 bit value in $d058-$d059)
          lda #<ROW_SIZE_BYTES
          sta VIC4.CHARSTEP_LO
          lda #>ROW_SIZE_BYTES
          sta VIC4.CHARSTEP_HI

          lda #0
          sta VIC.BACKGROUND_COLOR

          lda #<$C0
          sta SCREEN_CHAR
          lda #>$C0
          sta SCREEN_CHAR + 1

          lda #$08
          sta SCREEN_COLOR
          lda #$0
          sta SCREEN_COLOR + 1

          ldx #2
-
          lda #$0
          sta SCREEN_COLOR,x
          sta SCREEN_COLOR + 1,x

          lda #<$C0
          sta SCREEN_CHAR,x
          lda #>$C0
          sta SCREEN_CHAR + 1,x

          inx
          inx
          cpx #80
          bne -

          jmp *



* = $3000

          !hex 1032547698badcfe
          !hex 1032547698badcfe
          !hex 1032547698badcfe
          !hex 1032547698badcfe
          !hex 1032547698badcfe
          !hex 1032547698badcfe
          !hex 1032547698badcfe
          !hex 1032547698badcfe
