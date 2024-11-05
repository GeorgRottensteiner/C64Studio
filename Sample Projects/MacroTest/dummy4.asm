* = $0801

          lda Enemies.Frame,x
          rts

!zone Enemies

.Frame
          !byte 8

.Color
          !byte 8