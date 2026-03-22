          lda .locallabel, x  : sta hurz : ora lsmf,ddd
          ;x without comment is not colored as opcode

          sta SCREEN_COLOR + 23 * 5, y
          ;y is colored as opcode correctly

