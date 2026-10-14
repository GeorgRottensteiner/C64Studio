!macro Init_Table {
Table.ROW1:
!fill 64,0
Table.ROW2:
!fill 64,0
Table.ROW3:
!fill 64,0
}

*=$C000

+Init_Table

!zone Test
TEST:
txa
asl a
tax

clc
lda Table.ROW1,x
adc Table.ROW2,x
sta Table.ROW3,x
lda Table.ROW1+1,x
adc Table.ROW2+1,x
sta Table.ROW3+1,x

rts
!zone