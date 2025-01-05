!zone ZX81


!macro Line1
; Origin of a ZX81 file is always 16393

* = 16393

; System variables.

VERSN          !byte 0
E_PPC          !word 2
D_FILE         !word Display
DF_CC          !word Display + 1                ; First character of display
VARS           !word Variables
DEST           !word 0
E_LINE         !word BasicEnd
CH_ADD         !word BasicEnd + 4               ; Simulate SAVE "X"
X_PTR          !word 0
STKBOT         !word BasicEnd + 5
STKEND         !word BasicEnd + 5               ; Empty stack
BREG           !byte 0
MEM            !word MEMBOT
UNUSED1        !byte 0
DF_SZ          !byte 2
S_TOP          !word $0002                      ; Top program line number
LAST_K         !word $FDBF
DEBOUN         !byte 15
MARGIN         !byte 55
NXTLIN         !word Line2                      ; Next line address
OLDPPC         !word 0
FLAGX          !byte 0
STRLEN         !word 0
T_ADDR         !word $0C8D
SEED           !word 0
FRAMES         !word $F5A3
COORDS         !word 0
PR_CC          !byte $BC
S_POSN         !word $1821
CDFLAG         !byte $40
PRBUFF         !byte 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,$76  ; 32 Spaces + Newline
MEMBOT         !byte 0,0,0,0,0,0,0,0,0,0,$84,$20,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0      ; 30 zeros (no?)
UNUNSED2       !word 0

;end of system variables

Line1
          !byte $00,$01                    ; Line number 1
          !word Line1End - Line1Text       ; Line length

Line1Text
          !byte $EA                        ; REM

Program

!end



!macro Line2
; ===========================================================
; That's it.  End of user program
; ===========================================================

          !byte $76                        ; Newline
Line1End

          ; Line 2
Line2
          !byte $00,$02
          !word Line2End - Line2Text

Line2Text
                !byte $F9,$D4                    ; RAND USR
                !byte $1D,$22,$21,$1D,$20        ; 16514       ZX81'd value of Program
                !byte $7E                        ; Number
                !byte $8F,$01,$04,$00,$00        ; Numeric encoding
                !byte $76                        ; Newline
Line2End

Display
                !byte $76                        ; Newline
                !byte $76 ; Line 0
                !byte $76 ; Line 1
                !byte $76 ; Line 2
                !byte $76 ; Line 3
                !byte $76 ; Line 4
                !byte $76 ; Line 5
                !byte $76 ; Line 6
                !byte $76 ; Line 7
                !byte $76 ; Line 8
                !byte $76 ; Line 9
                !byte $76 ; Line 10
                !byte $76 ; Line 11
                !byte $76 ; Line 12
                !byte $76 ; Line 13
                !byte $76 ; Line 14
                !byte $76 ; Line 15
                !byte $76 ; Line 16
                !byte $76 ; Line 17
                !byte $76 ; Line 18
                !byte $76 ; Line 19
                !byte $76 ; Line 20
                !byte $76 ; Line 21
                !byte $76 ; Line 22
                !byte $76 ; Line 23

; Variables area (empty)
Variables
VariablesEnd
                !byte $80
BasicEnd
!end

!zone