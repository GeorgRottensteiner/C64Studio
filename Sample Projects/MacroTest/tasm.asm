* = $2000

a = 1

.if a=1
GNU:          lda #$20
.else
GNA:          lda #$01

.fi

GNORK: = * + 1
          sta $d020
          rts
