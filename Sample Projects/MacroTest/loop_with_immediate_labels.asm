* = $2000

SCREEN_RAM = $0400

MAP
  
!zone ShiftMap
ShiftMap
  !for i=0 to 25
    ldy #$00
-
    lda SCREEN_RAM + $28 * i + 1,y
    sta SCREEN_RAM + $28 * i + 0,y
    iny
    cmp #$27
    bne -
    lda MAP + $100 * i + $28,x
    sta SCREEN_RAM + $28 * i + $27
  !end
  rts