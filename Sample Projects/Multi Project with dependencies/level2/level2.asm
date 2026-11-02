* = $0801

!basic

          lda #$0
          sta 53280
          sta 53281

          lda #$32
          jsr DrawChar
          rts

LEVEL_START

LEVEL2_DATA
        !byte 55,66

LEVEL2_DATAB
        !byte 55,66

LEVEL2_DATAC
        !byte 55,66

LEVEL2_DATAD
        !byte 55,66

LEVEL2_DATAE
        !byte 55,66

LEVEL2_DATAF
          !byte 55, 66

* = $8000
!bin "../Main/main.prg",,2