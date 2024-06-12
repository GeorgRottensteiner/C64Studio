* = $0801

!basic
            lda #$02
            sta $0d02

            rts

!pseudopc $2000 {
            lda $2000

!pseudopc $3000 {
            lda $3000
}
MY_ADDRESS
            lda $2006
}


            rts

!if MY_ADDRESS != $2006 {
!error "Das ist nicht richtig!"
}