* = 49152


!macro set_poke_pos poke_positionX {
  poke_position = poke_positionX
}

+set_poke_pos 1024

lda #65
sta poke_position
rts