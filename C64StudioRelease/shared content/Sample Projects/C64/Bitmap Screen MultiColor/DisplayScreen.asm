!src <c64.asm>

* = $0801

SCREEN_CHAR     = $9000 - $0400;
BITMAP_LOCATION = $a000

!basic
          lda #0
          sta VIC.BORDER_COLOR
          sta VIC.BACKGROUND_COLOR

          ;location for bitmap and screen char
          lda #( ( BITMAP_LOCATION % $4000 ) / $400 ) + ( ( SCREEN_CHAR % $4000 ) / 1024 ) * 16 + 1
          sta VIC.MEMORY_CONTROL

          ;Enable bitmap multicolour
          lda #$18
          sta VIC.CONTROL_2

          ;set bitmap mode
          lda #$3B
          sta VIC.CONTROL_1

          ;set VIC to bank 2 ($8000 to $bfff)
          lda #$15
          sta CIA2.DATA_PORT_A

          ldx #0
.CopyLoop
          lda $4000 + 0 * 250,X
          sta SCREEN_CHAR + 0 * 250,X
          lda $4000 + 1 * 250,X
          sta SCREEN_CHAR + 1 * 250,X
          lda $4000 + 2 * 250,X
          sta SCREEN_CHAR + 2 * 250,X
          lda $4000 + 3 * 250,X
          sta SCREEN_CHAR + 3 * 250,X

          lda $4400 + 0 * 250,X
          sta $D800 + 0 * 250,X
          lda $4400 + 1 * 250,X
          sta $D800 + 1 * 250,X
          lda $4400 + 2 * 250,X
          sta $D800 + 2 * 250,X
          lda $4400 + 1 * 250,X
          sta $D800 + 1 * 250,X

!for ROW = 0 TO 31
          lda $2000 + ROW * 250,x
          sta BITMAP_LOCATION + ROW * 250,x
!end
          inx
          cpx #250
          beq .Done

          jmp .CopyLoop
.Done

EndlessLoop
          jmp EndlessLoop


;bitmap data
* = $2000
!media "multicolor.graphicscreen",BITMAP

;charscreen colors
* = $4000
!media "multicolor.graphicscreen",SCREEN

;color ram colors
* = $4400
!media "multicolor.graphicscreen",COLOR
