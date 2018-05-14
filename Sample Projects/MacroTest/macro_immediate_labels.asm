* = $0801
  !macro SOME_MACRO
cmp #4
bne +
jmp $0870
+
!end


bne +
+SOME_MACRO
+ 

rts  