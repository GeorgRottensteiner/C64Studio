;
;
;  PDS Pc1.21 :000: (c) P.D.Systems Ltd 1985-88
;
;
;       File  " IN-GAME ROUTINES! "
;    Version          09.64
;         By  "THIS IS STILL JAZ.R"
;
;     Created on Sat the 14th of May 1988
;        Last update 08:36 on 25/04/90
;

	ORG	$0801 ;ENDCODE

!G    DB    "TEXT STRING"

CMP #"J"

SCOL1	DB	0,6,14,3,13,7,10,8,9,0
SCOL2	DB	0,6,14,3,13,7,10,8,9,0
SDEL1	DB	7,7,7,7,0,8,8,6
SDEL2	DB	8,8,8,8,7,8,8,6
SDEL4	DB	8,0,13

;SDEL3	DB	0,4,8,6,7,8,7,9
SDEL3	DB	0,6,6,6,6,7,7,7
SCOL5	DB	0,0,11,12,15,7,1,1
SCOL6	DB	0,0,8,10,15,7,1,1
SCOL7	DB	14,4,6
SCOL8	DB	0,6,4

SDEL5	DB	7,8,11
SCOL9	DB	10,8,9
SCOL10	DB	0,0,4,14,3,13,1,1

SDEL3B	DB	2,8,6,5,6,6,7,6
SDEL2B	DB	19,7,7
SCOL9B	DB	0,9,8

IK EQU 5
ST EQU 17

LABEL     DB    IK+2,IK+3,IK+4,IK+5,IK+6,IK+7,IK,IK+1,0

SLOPEHI    DH    ST,ST+8,ST+16,ST+8
SLOPELO    DL    ST,ST+8,ST+16,ST+8

SMUSIC	LDY #7
!L2	LDA SCOL1,Y
	STA $D020
	STA $D021
	LDX SDEL1,Y
!L1	DEX
	BPL !L1

VALUE EQU 5
LDA    VALUE,Y
        STA    !TX+1
!TX   ADC #$FF

	ORG $2000

	DB 1,2,3,4,5,6,7,8,9,10

	ORG $3000

	DB 1,2,3,4,5,6,7,8,9,10

	ORG $3050

	DB 17,17



COLOR EQU 7

	ORG $3100

          lda #COLOR
          sta 53280
          rts

COLOR EQU COLOR + 1

	ORG $3200
          lda #COLOR
          sta 53280
          rts


COLOR EQU COLOR + 1

	ORG $3300
          lda #COLOR
          sta 53280
          rts


	ORG $4000

FIRST_LINE
ROW EQU 0
          DO 3
          lda #ROW
          sta 1024 + ROW
ROW EQU ROW + 1
          LOOP
LAST_LINE
          rts


