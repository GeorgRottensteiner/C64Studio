* = $1000

MAP_TILES
SCREEN_RAM = $0400
SCREEN_COLOR = $d800

!zone MapLoader
.DrawMap:
  TileScreenLocations:
  !byte 0,1,40,41
  
  ldy #$00
-   
  lda MAP_TILES + 6 * 4,y         ; 0-3
  ldx TileScreenLocations,y
  sta SCREEN_RAM,x
  lda #$01
  sta SCREEN_COLOR,x
  
  iny
  cpy #$04
  bne -
  rts
  