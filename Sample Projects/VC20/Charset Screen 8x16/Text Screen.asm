!source <vc20.asm>

;using 8x16 VIC20 screen mode, for 16k memory

* = $1201

!basic

SCREEN_CHAR       = $1e00
CHARSET_LOCATION  = $1000

          jmp InitCode

CHARSET_DATA
          !media "Text Screen.charscreen",CHARSET,0,143
CHARSET_SIZE = * - CHARSET_DATA

* = $1B00
InitCode
          ;copy screen data
          ldx #0
-
          lda SCREEN_DATA,x
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

          lda #$fc
          sta VIC.CHAR_MEMORY

          lda #$dd ;#$53 | $08
          sta VIC.COLORS

          lda #$80 ;#$e0
          sta VIC.AUX_COLOR_VOLUME

          ;12 rows, double height chars
          lda #( 12 << 1 ) | $01
          sta VIC.ROWS_WIDE_CHARS

          jmp *


* = $1e00
SCREEN_DATA
          !media "Text Screen.charscreen",CHARCOLOR
