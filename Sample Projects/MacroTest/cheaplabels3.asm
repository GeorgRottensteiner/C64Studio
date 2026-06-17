* = $2000

!zone Zone1
code1
          lda #$02
          sta @1 + 1
          lda #15
@1
          sta $ff
          rts

code2
          lda #$03
          sta @1 + 1
          lda #15
@1
          sta $ff
          rts