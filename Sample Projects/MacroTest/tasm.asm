* = $2000

a = 1

.if a=1
GNU:          lda #$20
.else
GNA:          lda #$01

.fi

GNORK: = * + 1
          sta $d020


-
                              lda #0
                            -
                              lda #1
                              jmp -
                              jmp --

                              jmp +
                              jmp ++

                            +
                              lda #2
                            +
                              lda #3
          rts
