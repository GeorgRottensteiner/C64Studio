* = $2000

SCREEN_CHAR = $0400

          ;expect error: opcode does not support indirect addressing with ,x outside (remove braces)
          lda (SCREEN_CHAR),x

          ;expect error: opcode does not support indirect addressing (remove braces)
          sta (SCREEN_CHAR + 2)
          rts