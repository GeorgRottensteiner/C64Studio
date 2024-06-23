* = $2000
Routine1
          lda #1
          rts

Routine2
          lda #2
          rts

JUMP_LIST
          !jumplist Routine1
          !jumplist Routine2

OFFSET_DATA
          !byte JUMP_LIST.Routine1
          !byte $17
          !byte JUMP_LIST.Routine2