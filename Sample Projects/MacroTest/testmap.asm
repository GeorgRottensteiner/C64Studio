* = $2000

!macro PRINT_VAR xpos,ypos,val
          ldx #xpos
          ldy #ypos
          lda val
          sta $0400
          rts
!end

!macro PRINT_VAR val
          lda val
          sta $0400
          rts
!end


!mediasrc "testmap.mapproject", MAPDATA_, tileelements
!mediasrc "testmap.mapproject", MAPDATA_, map

+PRINT_VAR 1,2,3
          +PRINT_VAR

lda #MAPDATA_TILE_NAME_555
lda #MAPDATA_TILE_NAME_BRICKWALL
lda #MAPDATA_TILE_NAME_TILE_1AOU
lda #MAPDATA_TILE_NAME_TILE_2UOAß

MAP