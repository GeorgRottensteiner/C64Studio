* = $0801

LINE_NO = 2018

!basic 2018," HALLO WELT!",STARTME

;!byte <LAST_LINE
;!byte >LAST_LINE
;!byte <LINE_NO
;!byte >LINE_NO
;!byte $9e   ;SYS
;!byte $30 + ( STARTME / 1000 )
;!byte $30 + ( STARTME / 100 ) % 10
;!byte $30 + ( STARTME / 10 ) % 10
;!byte $30 + STARTME % 10
;
;!byte ':'
;!byte 0x8F    ;REM
;!byte 20
;!byte 20
;!byte 20
;!byte 20
;!text "HALLO WELT!"


;LAST_LINE
;          !byte 0,0,0

STARTME
          inc $d020
          ;jmp STARTME
          rts