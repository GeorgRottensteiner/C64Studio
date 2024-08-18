* = $2000

!macro memcpy .source_hi, .source_lo, .target_hi, .target_lo, .length_hi, .length_lo

  lda .source_hi
  sta $400
  lda .source_lo
  sta $401

!end


+memcpy #$04, #$00, #$10, #$00, #$01, #$00

rts
