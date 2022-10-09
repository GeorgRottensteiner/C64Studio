* = $0801

PARTICLE_CHAR_COUNT = 20

!basic


!zone PlotPixel

  !for i = 0 to PARTICLE_CHAR_COUNT
.Plot:
    sta $c0de
    rts
  !end

   !for i = 0 to PARTICLE_CHAR_COUNT
.Plot2:
    sta $c0de
    rts
  !end