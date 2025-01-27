!cpu Z80

!to "zx81-helloworld.p",plain

!src <zx81.asm>

;prepares the BASIC startup and REM line containing the assembled code
+Line1

          ld hl,DISPLAY_TEXT

.DisplayChar
          ;load A with a character at HL
          ld a,(hl)
          cp #$76

          ;if so, then jump to end
          jp z, .DisplayComplete
          rst $10
P_ZX81
          inc hl
          jr .DisplayChar

.DisplayComplete
          ret


DISPLAY_TEXT
          !byte $2d,$2a,$31,$31,$34,$00,$3c,$34,$37,$31,$29,$76


;finishes the BASIC starter (closes line 1, and adds a second line with RAND USR call)
+Line2

