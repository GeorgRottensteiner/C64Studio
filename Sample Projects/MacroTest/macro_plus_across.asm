* = $0801

!macro MY_MACRO
          nop
          beq +
          gnu
          nop
+
!end


          !basic

START

     LDA #$00
     BEQ +

     +MY_MACRO
+
          RTS


