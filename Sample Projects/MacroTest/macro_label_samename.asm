* = $0801

!basic


!macro SelectControl index
                lda #index
                sta Param
                jsr SelectControl
!end


SelectControl
                lda Param
                sta $d020
                rts

+SelectControl  5
                rts

Param
                !byte 0