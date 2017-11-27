!to "kiri-macros.prg",cbm

test_dez = 42
test_hex =$2a
test_bin = %00101010

!macro mactest
          inc $d020
!end

!macro mactest2 base,idx,dest
          ldy #idx
          lda base,y
          sta dest
!end

*=$0801
loadaddress
!basic
          +mactest2 test_dez,test_dez,$d020-42+test_bin
          inc $d020
          jmp +
          rts
          
          +mactest 
          inx
-         +mactest
          inc $d021
          
+
          jmp -
          