DEBUG

DEBUGVALUE = 10

!ifdef DEBUG {
  DEBUGVALUE = 11
}

  * = $2000
  lda #DEBUGVALUE * 5
  sta $1fff