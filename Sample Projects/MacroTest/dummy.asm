ColorRAM      = $d800

Map.ColorMap  = $c000

Map.MapWidth  = 40


* = $2000
!for r = 0 to 1 ;11
!for c = 0 to 1 ;38
lda Map.ColorMap + c + r * Map.MapWidth,x
sta ColorRAM + 40 + c + r * 40
!end
!end
