!source <vc20.asm>

;using 8x16 VIC20 screen mode, for 16k memory
;screen is at $1000
;custom charset is also at $1000 !
;  <- therefor charset data begins at $1100, which allows
;     custom chars usage from index $10 to $ff (using RAM from $1100 to $1fff)

* = $1201

!basic

SCREEN_CHAR       = $1000
CHARSET_LOCATION  = $1100

          jmp InitCode

* = $1400
CHARSET_DATA
          !media "Text Screen.charscreen",CHARSET,0,193
CHARSET_SIZE = * - CHARSET_DATA

SCREEN_DATA
          !media "Text Screen.charscreen",CHARCOLOR

;* = $1B00
* = $2500
InitCode
          ;copy screen data
          ldx #0
-
          lda SCREEN_DATA,x
          clc
          adc #16
          sta SCREEN_CHAR,x

          lda SCREEN_DATA + 20 * 12,x
          sta VIC.COLOR_RAM_EXPANDED,x

          inx
          cpx #240
          bne -

          ;copy charset
          ldx #0
          ldy #( CHARSET_SIZE + 255 ) / 256

-
.SOURCE = * + 1
          lda CHARSET_DATA,x
.TARGET = * + 1
          sta CHARSET_LOCATION,x
          inx
          bne -

          inc .SOURCE + 1
          inc .TARGET + 1

          dey
          bne -

          lda #$cc
          sta VIC.CHAR_MEMORY

          lda #$00 | ( 20 )
          sta VIC.COLUMNS_ADDRESS

          ;0 = background color
BG_COLOR      = 0
BORDER_COLOR  = 6
AUX_COLOR     = 5
          lda #( BG_COLOR << 4 ) | BORDER_COLOR | $08
          sta VIC.COLORS

          lda #( AUX_COLOR << 4 )
          sta VIC.AUX_COLOR_VOLUME

          ;12 rows, double height chars
          lda #( 12 << 1 ) | $01
          sta VIC.ROWS_WIDE_CHARS

          ;move screen to center
          lda #$0e
          sta VIC.HORIZONTAL_CENTERING

          jmp *

