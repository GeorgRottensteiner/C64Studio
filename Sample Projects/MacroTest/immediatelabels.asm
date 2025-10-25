          !macro gnu
          lda #12
          rts
          !end


* = $2000

--        +gnu

          +gnu




Main
-
          lda #$ff
          jmp -





NotMain
          bne +
          inc $d020
+
          jmp -


          ldx #true
--        lda VICRaster
-         cmp VICRaster
          beq -
          bmi --
          cmp #55
          beq +
          ldx #False
+         stx .IsPal

x         lda #12




