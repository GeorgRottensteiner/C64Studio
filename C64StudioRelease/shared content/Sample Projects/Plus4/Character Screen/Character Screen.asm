SCREEN_LOCATION = $0c00
COLOR_LOCATION  = $0800


;BASIC header
* = $1001
!basic
          ;set background color to black
          lda #0
          sta $ff15

          ;copy char and color data
          ldx #0
-
          lda CHAR_DATA,x
          sta SCREEN_LOCATION,x
          lda CHAR_DATA + 1 * 250,x
          sta SCREEN_LOCATION + 1 * 250,x
          lda CHAR_DATA + 2 * 250,x
          sta SCREEN_LOCATION + 2 * 250,x
          lda CHAR_DATA + 3 * 250,x
          sta SCREEN_LOCATION + 3 * 250,x

          lda COLOR_DATA,x
          sta COLOR_LOCATION,x
          lda COLOR_DATA + 1 * 250,x
          sta COLOR_LOCATION + 1 * 250,x
          lda COLOR_DATA + 2 * 250,x
          sta COLOR_LOCATION + 2 * 250,x
          lda COLOR_DATA + 3 * 250,x
          sta COLOR_LOCATION + 3 * 250,x

          inx
          cpx #250
          bne -

          ;endless loop
-
          jmp -



CHAR_DATA
COLOR_DATA = CHAR_DATA + 1000

        ;this includes 1000 bytes of characters plus 1000 bytes of colors
        !media "sample.charscreen",charcolor