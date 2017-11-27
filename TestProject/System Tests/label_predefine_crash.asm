
* = $2000


    lda #1
PACKED_DATA
PACKED_DATA_SIZE

!if PACKED_DATA = 1 {

    ldx #0
}
  
    sta PACKED_DATA
    rts
    