
TASKCHARBASE = 12
TB_Reserved = TASKCHARBASE+TB_Reserved_Char*8
TB_Reserved_Char = 118

*=$c000
blubb     lda #$01
adress=*+1
store     sta $0400
          rts