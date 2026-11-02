* = $0801

!basic

          lda #$0
          sta 53280
          sta 53281

          lda #$31
          jsr DrawChar
          rts


LEVEL_1_DATA
        !byte 22

LEVEL_2_DATA
        !byte 22

LEVEL_3_DATA
        !byte 22

LEVEL_4_DATA
        !byte 22

LEVEL_5_DATA
        !byte 22


* = $8000
!bin "../Main/main.prg",,2