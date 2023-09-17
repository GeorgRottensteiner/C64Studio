* = $1000
          !zone gnu

          jmp .temp

          !zone inner {

          .temp !byte 0

          }

          .temp
          !byte 1