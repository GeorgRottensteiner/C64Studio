;C64Studio - Sample Project VIC20 #2
;
;expects a VIC20 with unexpanded RAM
;
;creates a BASIC stub and display a char screen from a project

!source <vc20.asm>

SCREEN_LOCATION = $1e00

* = $1001

          ;BASIC stub
!basic

          ;copy characters and colors in screen/color RAM
          ;since the screen is 22*23 (=506) chars we do
          ;two writes at once so the counter can stay below 256
          ;(253 = 506 / 2)

          ldx #0

-
          ;copy to first half of screen characters
          lda SCREEN_DATA,x
          sta SCREEN_LOCATION,x
          ;copy to second half of screen characters
          lda SCREEN_DATA + 253,x
          sta SCREEN_LOCATION + 253,x

          ;copy to first half of screen colors
          lda SCREEN_DATA + 23 * 22,x
          sta VIC.COLOR_RAM,x
          lda SCREEN_DATA + 23 * 22 + 253,x
          sta VIC.COLOR_RAM + 253,x

          inx
          cpx #253
          bne -

          ;set up colors
          ;  $5x = background color 6
          ;  $x3 = border color 3
          ;  $08 = normal mode
          lda #$63 | $08
          sta VIC.COLORS

          ;set up aux color
          ;  $ex = aux color 14
          ;  $x0 = volume 0
          lda #$e0
          sta VIC.AUX_COLOR_VOLUME

          ;endless loop
          jmp *


;label to access the included binary data
SCREEN_DATA

          ;this statement includes the data of "Text Screen.charscreen"
          ;for VIC20 it is 22 * 23 bytes of characters, followed by 22 * 23 bytes of the colors

          !media "Text Screen.charscreen",CHARCOLOR