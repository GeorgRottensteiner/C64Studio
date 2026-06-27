* = $2000

!macro set16 address, value
          lda #<value
          sta address
          lda #>value
          sta address + 1
!end

!basic
          jsr zone.code1
          jsr zone.code2
          rts

!zone zone
.code1
          +set16 @1 + 1 , $0400
@1
          lda $ffff,x
          rts

.code2
          lda #$03
          sta @1 + 1
          lda #15
@1
          sta $ff
          rts
