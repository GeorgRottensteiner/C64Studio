;demonstrates including four of the exports of the Charpad demo file
;(Navy Seals - L1 - Harbour)



ZEROPAGE_POINTER_1  = $fb
ZEROPAGE_POINTER_2  = $fd
ZEROPAGE_POINTER_3  = $f0
ZEROPAGE_POINTER_4  = $f5

MAP_TILE_POS        = $02
SCREEN_X            = $f2
SCREEN_Y            = $f3
POS_IN_TILE_DATA    = $f4

SCREEN_LOCATION     = $0400
COLOR_LOCATION      = $d800




* = $0801

!basic
          lda #0
          sta $d020
          sta $d021

          ;enable mc
          lda #$18
          sta $d016


          ;set charset to $2000
          lda #$19
          sta $D018

          ;set charset colors
          lda #11
          sta $d022
          lda #9
          sta $d023

          ;display map (no offset)
          lda #<MAP_DATA
          sta ZEROPAGE_POINTER_1
          lda #>MAP_DATA
          sta ZEROPAGE_POINTER_1 + 1

          lda #<SCREEN_LOCATION
          sta ZEROPAGE_POINTER_2
          lda #>SCREEN_LOCATION
          sta ZEROPAGE_POINTER_2 + 1
          lda #<COLOR_LOCATION
          sta ZEROPAGE_POINTER_3
          lda #>COLOR_LOCATION
          sta ZEROPAGE_POINTER_3 + 1

          lda #0
          sta MAP_TILE_POS
          sta SCREEN_X
          sta SCREEN_Y

.NextTile
.NextLine
          ldy MAP_TILE_POS

          ;tile data is annoyingly placed in memory
          ;tiles >= 64 require an updated pointer to tile data, so we use ZEROPAGE_POINTER_4 for tile data
          ;this quick hack only checks if tile index >= 64 and increases the hi byte.
          ;     the hack does NOT work for tile indices >= 128!
          lda #<TILE_DATA
          sta ZEROPAGE_POINTER_4
          lda #>TILE_DATA
          sta ZEROPAGE_POINTER_4 + 1

          lda (ZEROPAGE_POINTER_1),y
          cmp #64
          bcc +

          inc ZEROPAGE_POINTER_4 + 1

+

          ;TILE_DATA has all four chars of a tile after each other, so we need to multiply tile index by 4
          asl
          asl
          tax
          stx POS_IN_TILE_DATA


          ;top left char
          ldy POS_IN_TILE_DATA
          lda (ZEROPAGE_POINTER_4),y
          tax

          ldy SCREEN_X
          inc SCREEN_X
          sta (ZEROPAGE_POINTER_2),y

          ;top left color
          tax
          lda CHAR_ATTRIBS,x
          sta (ZEROPAGE_POINTER_3),y

          ;top right char
          ;screen pos + 1
          inc POS_IN_TILE_DATA
          ldy POS_IN_TILE_DATA
          lda (ZEROPAGE_POINTER_4),y
          tax

          ldy SCREEN_X
          sta (ZEROPAGE_POINTER_2),y

          ;top right color
          tax
          lda CHAR_ATTRIBS,x
          sta (ZEROPAGE_POINTER_3),y

          ;bottom left char
          ;screen pos + 39
          lda SCREEN_X
          clc
          adc #39
          sta SCREEN_X

          inc POS_IN_TILE_DATA
          ldy POS_IN_TILE_DATA
          lda (ZEROPAGE_POINTER_4),y
          ldy SCREEN_X
          inc SCREEN_X
          sta (ZEROPAGE_POINTER_2),y

          ;bottom left color
          tax
          lda CHAR_ATTRIBS,x
          sta (ZEROPAGE_POINTER_3),y

          ;bottom right char
          ;screen pos + 1
          inc POS_IN_TILE_DATA
          ldy POS_IN_TILE_DATA
          lda (ZEROPAGE_POINTER_4),y
          tax
          ldy SCREEN_X
          sta (ZEROPAGE_POINTER_2),y

          ;bottom right color
          tax
          lda CHAR_ATTRIBS,x
          sta (ZEROPAGE_POINTER_3),y

          ;increase pointers to next tile/screen pos
          lda SCREEN_X
          sec
          sbc #39
          sta SCREEN_X
          inc MAP_TILE_POS

          lda SCREEN_X
          cmp #40
          bne .NextTile

          ;a line is complete, go to next line
          inc SCREEN_Y
          inc SCREEN_Y

          ;move screen/color pointers down two lines
          lda ZEROPAGE_POINTER_2
          clc
          adc #80
          sta ZEROPAGE_POINTER_2
          bcc +
          inc ZEROPAGE_POINTER_2 + 1
+

          lda ZEROPAGE_POINTER_3
          clc
          adc #80
          sta ZEROPAGE_POINTER_3
          bcc +
          inc ZEROPAGE_POINTER_3 + 1
+

          lda SCREEN_Y
          cmp #24
          beq .Done

          lda #0
          sta SCREEN_X

          ;map pos must go to next line
          lda #0
          sta MAP_TILE_POS

          lda ZEROPAGE_POINTER_1
          clc
          adc #100
          sta ZEROPAGE_POINTER_1
          bcc +
          inc ZEROPAGE_POINTER_1 + 1
+

          jmp .NextLine

.Done

          ;wait for button press
--
          lda #240
-
          cmp $d012
          bne -

          lda $dc00
          and #$10
          bne --

          rts

TILE_DATA
          !bin "tiles.bin"

CHAR_ATTRIBS
          !bin "charsAttribs.bin"

MAP_DATA
          !bin "map.bin"

* = $2000
          !bin "chars.bin"
