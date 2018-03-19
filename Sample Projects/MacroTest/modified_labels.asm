!to "modified_labels.prg",cbm

COLOR = 7

* = $2000

          lda #COLOR
          sta 53280
          rts

COLOR = COLOR + 1

* = $3000
          lda #COLOR
          sta 53280
          rts


COLOR = COLOR + 1

* = $4000
          lda #COLOR
          sta 53280
          rts

                     