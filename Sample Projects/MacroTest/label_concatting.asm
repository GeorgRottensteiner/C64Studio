* = $0801

VALUE = 3

MYLABEL##VALUE = 5


!for ROW = 2 to 7 

.TEMP##ROW
        lda #ROW
        sta 1024 + ROW
!end