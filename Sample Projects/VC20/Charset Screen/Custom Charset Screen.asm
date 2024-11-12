!source <vc20.asm>

;using regular VIC20 screen mode, for unexpanded memory

* = $1001

SCREEN_CHAR       = $1e00
CHARSET_LOCATION  = $1000

!basic
          jmp InitCode

SCREEN_DATA
          !media "ghostbusters.charscreen",CHARCOLOR

* = $1800
InitCode
          ;copy screen data
          ldx #0
-
          lda SCREEN_DATA,x
          sta SCREEN_CHAR,x
          lda SCREEN_DATA + 253,x
          sta SCREEN_CHAR + 253,x

          lda SCREEN_DATA + 23 * 22,x
          sta VIC.COLOR_RAM,x
          lda SCREEN_DATA + 23 * 22 + 253,x
          sta VIC.COLOR_RAM + 253,x

          inx
          cpx #253
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

          lda #( $00 << 4 ) | $02 | $08
          sta VIC.COLORS

          lda #$10
          sta VIC.AUX_COLOR_VOLUME

          jmp *


CHARSET_DATA
          !media "ghostbusters.charscreen",CHARSET,0,143
CHARSET_SIZE = * - CHARSET_DATA