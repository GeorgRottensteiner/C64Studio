SCREEN_LOCATION = $0400
COLOR_LOCATION  = $d800


;BASIC header
* = $0801
!basic

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