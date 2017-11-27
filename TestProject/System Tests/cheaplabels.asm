* = $2000
Main
@cheap
          lda #$ff
          jmp @cheap

NotMain
@cheap
          inc $d020
          jmp @cheap