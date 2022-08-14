!to "codepart3.bin",plain

* = $2000
          ldx #0
-
          lda PART1_TEXT,x
          sta $0400,x

          inx
          cpx #40 * 5
          bne -

          rts


PART1_TEXT
          !media "part3.charscreen",CHAR