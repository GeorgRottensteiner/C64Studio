!to "nested_for.prg",cbm

;dx = 0

* = $2000

!for dx=0 to 1
 !for dy=0 to 1 
 lda $4000+dx+dy
 ;ldx #dx
 ;ldy #dy
 sta $6000+dx-dy
 !end
 !end
GNI
        lda #17
        sta 53280
        rts

!for dx=1 to 0 step - 1
 !for dy=0 to 1
 lda $4000+dx+dy 
 ;ldx #dx
 ;ldy #dy
 sta $6000+dx-dy
 !end
 !end
        
* = $4000
        !byte 0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15
        !byte 0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15
        !byte 0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15
        !byte 0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15
        !byte 0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15
        !byte 0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15
        !byte 0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15

 
GNA          

;!source "include_nested_for.asm"

!zone Hinz
Hinz
.Kunz
