!sl "labels.txt",IGNORE_UNUSED_LABELS | IGNORE_ASSEMBLER_ID_LABELS | IGNORE_INTERNAL_LABELS |  IGNORE_LOCAL_LABELS 

!macro gnu
.y
          lda #$02
          bne .y
!end       

* = $2000 
Routine1
          lda #1
          beq +
          rts

Routine2
+
          lda #2
          rts
          
+gnu

JUMP_LIST
          !jumplist Routine1
          !jumplist Routine2

OFFSET_DATA
          !byte JUMP_LIST.Routine1
          !byte $17
          !byte JUMP_LIST.Routine2 
          
          
   