* = $0801

          !basic

!macro callme arg1, arg2
          lda #arg1
          sta arg2
!end

!macro callme arg1
          lda #0
          sta arg1
!end

          +callme 14, $d020
          +callme $d020
          rts
