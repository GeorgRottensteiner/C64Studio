* = $2000
                             !zone Zone1
                              @cheap
                             lda #$ff
                             jmp @cheap

                             !zone Zone2
                              @cheap
                               inc $d020
                                jmp @cheap