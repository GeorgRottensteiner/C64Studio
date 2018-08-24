* = $2000


COLOR_SOURCE = $0400
COLOR_TARGET = $d800

!for COL = 0 TO 1
!for ROW = 0 to 1
          lda COLOR_SOURCE + ROW * 40 + COL
          sta COLOR_TARGET + ROW * 40 + COL
!end
!end
