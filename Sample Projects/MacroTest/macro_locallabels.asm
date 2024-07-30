!to "macro_locallabels.prg",cbm

!macro fooMacro .t
.m1
      inc $d020
      bne .m1
!end

!macro mactest
    lda #<+
+   rts
!end



* = $2000

CALL
+fooMacro 1
          rts

+mactest

hurz