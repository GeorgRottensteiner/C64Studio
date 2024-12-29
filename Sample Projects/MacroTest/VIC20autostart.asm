* = $0326

!byte <init,>init
!hex 70f7f5f1eff3d2fe49f585f6

* = $1001
!basic

init
          stx .TEMP
          ldx #1
          stx $1e00

          ldx .TEMP
          jmp $f27a

.TEMP
          !byte 0