* = $0801

!src <c64.asm>

!basic

          lda #$18
          sta VIC.MEMORY_CONTROL

          lda #240
-
          cmp VIC.RASTER_POS
          bne -

          lda #$0b
          sta VIC.CONTROL_1

          lda #$18
          sta VIC.CONTROL_2

          lda #8
          sta VIC.CHARSET_MULTICOLOR_1
          lda #9
          sta VIC.CHARSET_MULTICOLOR_2
          lda #0
          sta VIC.BACKGROUND_COLOR
          sta VIC.BORDER_COLOR

          ldx #0
          txa
-
          sta $0400,x
          sta $0400 + 250,x
          sta $0400 + 2 * 250,x
          sta $0400 + 3 * 250,x

          inx
          cpx #250
          bne -

!for ROW = 0 to 20
          ldx #0
-
          lda SCREEN_DATA + ROW * 16,x
          sta $0400 + ( 2 + ROW ) * 40 + 12,x
          lda COLOR_DATA + ROW * 16,x
          sta $d800 + ( 2 + ROW ) * 40 + 12,x

          inx
          cpx #16
          bne -
!end

          lda #240
-
          cmp VIC.RASTER_POS
          bne -

          lda #$1b
          sta VIC.CONTROL_1

          jmp *





SCREEN_DATA
          !media "grgameslogo.charscreen",CHAR

COLOR_DATA
          !media "grgameslogo.charscreen",COLOR

* = $2000
CHARSET
          !media "grgameslogo.charscreen",CHARSET