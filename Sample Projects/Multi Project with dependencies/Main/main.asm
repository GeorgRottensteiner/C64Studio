* = $8000

;writes character in A to screen
!lzone DrawChar
          sta $0400
          rts