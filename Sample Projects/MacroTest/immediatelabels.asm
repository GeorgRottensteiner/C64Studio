* = $2000
Main
-
          lda #$ff
          jmp -


NotMain
          bne +
          inc $d020
+
          jmp -
