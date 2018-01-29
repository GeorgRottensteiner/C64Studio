ZEROPAGE_POINTER_1  = $fa
ZEROPAGE_POINTER_2  = $fc
ZEROPAGE_POINTER_3  = $fe

TILE_X              = $02
TILE_Y              = $03
CURRENT_MAP         = $04


* = $0801
!basic 
          ;set VIC to see the charset at $3000
          lda #$1c
          sta $D018
          
          lda #0
          sta CURRENT_MAP
@RedrawMap          
          jsr DrawMap
          
          
@EndlessLoop          
          ;space pressed?
          lda $dc01
          and #$10
          bne @EndlessLoop
        
@WaitForSpaceRelease        
          lda $dc01
          and #$10
          beq @WaitForSpaceRelease        
        
          ;toggle map
          lda CURRENT_MAP
          eor #$01
          sta CURRENT_MAP
          jmp @RedrawMap        
          
          
!zone DrawMap
;a = map index  
DrawMap
          tay
          lda SCREENS_MAP_LIST_LO,y
          sta ZEROPAGE_POINTER_1
          lda SCREENS_MAP_LIST_HI,y
          sta ZEROPAGE_POINTER_1 + 1
          
          lda #0
          sta TILE_X
          sta TILE_Y
          
          ;screen ram
          lda #<$0400
          sta ZEROPAGE_POINTER_2
          lda #>$0400
          sta ZEROPAGE_POINTER_2 + 1
          
          ;color ram
          lda #<$d800
          sta ZEROPAGE_POINTER_3
          lda #>$d800
          sta ZEROPAGE_POINTER_3 + 1
          
.DrawNextTile          
          ldy TILE_X
          ;fetch tile index
          lda (ZEROPAGE_POINTER_1),y
          tax
          
          lda TILE_X
          asl
          tay
          
          ;upper left char/color
          lda SCREENS_TILE_CHARS_0_0,x
          sta (ZEROPAGE_POINTER_2),y
          lda SCREENS_TILE_COLORS_0_0,x
          sta (ZEROPAGE_POINTER_3),y
          
          ;upper right char/color
          iny
          lda SCREENS_TILE_CHARS_1_0,x
          sta (ZEROPAGE_POINTER_2),y
          lda SCREENS_TILE_COLORS_1_0,x
          sta (ZEROPAGE_POINTER_3),y
          
          tya
          clc
          adc #39
          tay
          
          ;lower left char/color
          lda SCREENS_TILE_CHARS_0_1,x
          sta (ZEROPAGE_POINTER_2),y
          lda SCREENS_TILE_COLORS_0_1,x
          sta (ZEROPAGE_POINTER_3),y
          
          ;lower right char/color
          iny
          lda SCREENS_TILE_CHARS_1_1,x
          sta (ZEROPAGE_POINTER_2),y
          lda SCREENS_TILE_COLORS_1_1,x
          sta (ZEROPAGE_POINTER_3),y
          
          ;next tile
          inc TILE_X
          
          ;reached right end?
          lda TILE_X
          cmp #20
          bne .DrawNextTile
          
          ;next row
          lda #0
          sta TILE_X
          inc TILE_Y
          ;reached bottom?
          lda TILE_Y
          cmp #12
          bne .UpdatePointers
          
          ;done
          rts
          
.UpdatePointers
          ;since we draw 2x2 tiles we need to increase the map pointer by #20 (one tile row)
          ;and the screen/color pointers by #80 (2 rows of screen width)

          ;update source pointer
          lda ZEROPAGE_POINTER_1
          clc
          adc #20
          sta ZEROPAGE_POINTER_1
          bcc +
          inc ZEROPAGE_POINTER_1 + 1
+          

          ;update target pointers
          ;since both screen and color ram are aligned to 256 bytes we can "reuse" the lower byte and carry flag
          lda ZEROPAGE_POINTER_2
          clc
          adc #80
          sta ZEROPAGE_POINTER_2
          sta ZEROPAGE_POINTER_3
          bcc +
          inc ZEROPAGE_POINTER_2 + 1
          inc ZEROPAGE_POINTER_3 + 1
+          
          jmp .DrawNextTile




;place the charset data at $3000          
* = $3000
          !media "sample.charsetproject",CHAR


;this pseudo op includes
;  a list named SCREENS_MAP_LIST_LO/SCREENS_MAP_LIST_HI with pointer to the screen data
;  a list per tile char and color named 
;      SCREENS_TILE_CHARS_0_0, SCREENS_TILE_CHARS_1_0, SCREENS_TILE_CHARS_0_1 and SCREENS_TILE_CHARS_1_1
;      SCREENS_TILE_COLORS_0_0, SCREENS_TILE_COLORS_1_0, SCREENS_TILE_COLORS_0_1 and SCREENS_TILE_COLORS_1_1
MAP_DATA
          !mediasrc "sample.mapproject",SCREENS_,MAPTILE