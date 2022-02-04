* = $2000

;lab0 lda #$ff
;sta $0400
;lab1 lda #$ff
;sta $0400
;lab2 lda #$ff
;sta $0400


!macro mymacro z {
lab##z lda #$ff
sta $0400
}

!for i = 0 TO 2
mymacro i
!end

jmp lab0