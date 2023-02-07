;Easyflash cartridges start in Ultimax Mode
;the lower bank is mapped at $8000
;the higher bank is mapped at $e000
;Kernal is switched off!
;this means for the launcher, it has to put a vector
;to the startup code at $fffc!

!to "easyflash.crt",easyflashcrt

* = $8000

;first lower bank

!bank 0,$2000

          !word Init
          !word Init
          !byte $c3 ;c
          !byte $c2 ;b
          !byte $cd ;m
          !byte $38 ;8
          !byte $30 ;0

Init
        sei


-
        inc $d020
        jmp -


;first upper bank
!bank 1,$2000
!pseudopc $e000

          !fill $fffc - $e000,0
          !word Init

!realpc
