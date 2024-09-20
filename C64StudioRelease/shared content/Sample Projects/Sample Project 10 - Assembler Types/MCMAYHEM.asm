; NOTE: THIS FILE ASSEMBLES BUT DOES NOT RUN DIRECTLY

;
;
;  PDS Pc1.21 :000: (c) P.D.Systems Ltd 1985-88
;
;
;       File  "FRONT END FOR GAME!"
;    Version          11.21
;         By  "JAZ.R. FROM APEX'88"
;
;     Created on Sat the 14th of May 1988
;        Last update 00:24 on 01/01/80
;

	SKIP	7
	ORG	$2200
	SEND	COMPUTER2
	FREE	ENDCODE

;blank sprite: $4D, inverse sprite: $4E

SCR	EQU	$D021
BDR	EQU	$D020
JOY2	EQU	$DC00
;TT	EQU	$86
TT	EQU	$39	;up to + 29 ? - +23,24,25,26 - temp for any code
TA	EQU	$7E	;up to + 17
TJ	EQU	$C6	;up to + 15



DELAY	DS	50,$EA	;must be on same page
	RTS

CRAST	JSR DELAY+30	;+39 with ROM in
	JSR COLSPLIT
	RTS		;leave in!

COLSPLIT	LDA #7
	STA $49
	LDY #0
COLTAB	LDA $48FF,Y
	STA SCR
COLBDR	STA $FFFF
	TYA
	AND $49
	BEQ !DMA
	LDX #6
!D	DEX
	BPL !D
!DMA	INY
COLMAX	CPY #$FF
	BNE COLTAB
	RTS
COLBYTE	DB	0


BOTFADE	LDX #3
!1	LDY C3DEL,X
!D	DEY
	BPL !D
	LDA C3,X
	STA BDR
	STA SCR
	DEX
	BPL !1
	RTS

C3DEL	HEX	08070810
;	HEX	0706070F






NOBORDERS	LDA $D011	;call at #$F9 (raster position)
	AND #$F7
	STA $D011
!0	LDA $D012
	BNE !0
	LDA $D011
	AND #$7F
	ORA #8
	STA $D011
	RTS

JOYSTICK2	LDA JOY2	;0:pushing in direction (JOY 2)
	STA !L+1
	LDA #$10
	STA !A+1
	LDX #4
!L	LDA #$FF
!A	AND #$FF
	STA JDATAB,X
	LDA !A+1
	LSR
	STA !A+1
	DEX
	BPL !L
	RTS

DETECT	STA $DC00	;LDA (col) / CMP (row)
!NK	LDA $DC01
	CMP $DC01
	BNE !NK
	RTS


CLS	STX !COL+1
	STA !CHAR+1
	LDX #251
!CHAR	LDA #$FF
	STA $4400-1,X
	STA $4400+249,X
	STA $4400+499,X
	STA $4400+749,X
!COL	LDA #$FF
	STA $D800-1,X
	STA $D800+249,X
	STA $D800+499,X
	STA $D800+749,X
	DEX
	BNE !CHAR
	RTS


RASTER	LDA $D019
	AND #1
	BNE !DOR
	JMP ENDR
!DOR	LDA #1
	STA $D019
RAST	JSR $FFFF
	STA $D012
	STY RAST+1
	STX RAST+2
ENDR	;LDX #10
!L	;DEX
	;BPL !L
	JMP $EA7E


SCREWMEM	LDA #0
!P1	STA $040A
!P2	STA $400A
	CLC
	LDA !P1+1
	ADC #1
	STA !P1+1
	STA !P2+1
	BCC SCREWMEM
	INC !P1+2
	INC !P2+2
	JMP SCREWMEM





FE51	PLA
	TAY
	PLA
	TAX
	PLA
NR2	RTI

RCONT2	SEI		;A: $01 value
	STA $01	;CARRY: set = raster2, clear = raster3
	STX SM10+1
	STY SM10+2
	LDA #$7F
	STA $DC0D
	STA $DD0D
	LDA $DC0D
	LDA $DD0D
	BCC !R3
	LDX #>RASTER2
	LDY #<RASTER2
	JMP !SR
!R3	LDX #>RASTER3
	LDY #<RASTER3
!SR	STX $0314
	STX $FFFE
	STY $0315
	STY $FFFF
EAVAL	LDA #$7E
	STA NORAST+1
EAVAL2	LDA #$EA
	STA NORAST+2
RASTERSET2	LDA #129
	STA $D01A
	STA $D019
	LDA #1*8+49	;$F9
	STA $D012
	LDA $D011
	AND #127
	STA $D011
	LDA $DC0E
	AND #254
	STA $DC0E
	CLI
	RTS

RASTER2	CLC
	PHA
	TXA
	PHA
	TYA
	PHA
RASTER3	CLD
	LDA $D019
	AND #$01
	BEQ NORAST
	LDA #$01
	STA $D019
SM10	JSR $FFFF
	STA $D012
	STX SM10+1
	STY SM10+2
NORAST	JMP $EA7E

FE51SET	LDA #>FE51
	STA EAVAL+1
	LDA #<FE51
	STA EAVAL2+1
	RTS

EA7ESET	LDA #$7E
	STA EAVAL+1
	LDA #$EA
	STA EAVAL2+1
	RTS





GO	SEI
	LDA #$36
	STA $01
	LDA #0
	STA $D9
	STA $D015
	STA $D01C
	STA BDR
	STA SCR

	LDA #193
	STA 792

	LDA $D011
	AND #239
	STA $D011

	LDA $DD02
	ORA #$03
	STA $DD02
	LDA $DD00
	AND #$FC
	ORA #$02
	STA $DD00
	LDA #$14
	STA $D018

	LDY #7		;download music player
	LDX #0
!L1	LDA $1A00,X
!L2	STA $0400,X
	LDA #$FF
!L3	STA $1A00,X
	INX
	BNE !L1
	INC !L1+2
	INC !L2+2
	INC !L3+2
	DEY
	BPL !L1
	LDA #$2C
	STA CA11
 	LDA #0
	STA MOD

	LDY #127
	LDX #0
!D	JSR DELAY
	DEX
	BNE !D
	DEY
	BNE !D

	JMP INTRO

THEBIZ	SEI
	LDA #$0B
	STA $D011
	LDY #10
	LDX #0
!D	JSR DELAY
	DEX
	BNE !D
	DEY
	BNE !D

;	LDA #$FF
;	LDX #0
;	JSR CLS

SELECTM	LDA #0
	STA $D418
	STA $D015
	STA $7FFF

	LDX #15
!POS	STA $D000,X
	DEX
	BPL !POS

	LDA #$EE
	LDX #0
	JSR CLS

	LDX #0
!B1	LDA BANKIN,X
	STA $5640,X
	DEX
	BNE !B1
	LDX #$3F
!B2	LDA BANKIN+$100,X
	STA $5740,X
	DEX
	BPL !B2

	LDA #$50
	STA TA+1
	LDA #$00
	STA TA
	LDX #2
	LDY #0
!INV1	LDA (TA),Y
	EOR #$FF
	STA (TA),Y
	DEY
	BNE !INV1
	INC TA+1
	DEX
	BPL !INV1
	LDY #$3F
!INV2	LDA (TA),Y
	EOR #$FF
	STA (TA),Y
	DEY
	BPL !INV2

	LDX #8
	STX $D022
	INX
	STX $D023

	LDX #39
!C1	LDA #1
	STA $DB20,X
	LDA #13
	STA $DB48,X
	STA $DB70,X
	STA $DB98,X
;	LDA #15
	STA $DBC0,X
	LDA #$FF
	STA $4428,X
	STA $4450,X
	DEX
	BPL !C1

	LDX #200
!W1	LDA LANDSCAPE-1,X
	STA $471F,X
	DEX
	BNE !W1

	JSR STARSET
	JSR DEMOSETMES

	LDA #$1B
	STA $D011
	LDA #$C8+16
	STA $D016

	LDY #0
	JSR NEWLIST

;	LDA #SRAST1&255
;	STA RAST+1
;	LDA #SRAST1/256
;	STA RAST+2
;	LDA $DC0E
;	AND #$FE
;	STA $DC0E
;	LDA $D011
;	AND #$7F
;	STA $D011
;	LDA #$2F
;	STA $D012
;	LDA #RASTER&255
;	STA $0314
;	LDA #RASTER/256
;	STA $0315
;	LDA $D01A
;	ORA #$01
;	STA $D01A
;	CLI

	JSR FE51SET
	LDX #>SRAST2
	LDY #<SRAST2
	LDA #$35
	SEC
	JSR RCONT2


LOOP	LDA #$7F
	STA $DC01
	STA $DC00

!CO
;	LDA $DC01
;	AND #$10
;	BNE !ND
;	LDA #0
;	STA $D418
;	JMP $1600

!ND	LDA CODEFLAG
	BEQ LOOP
	DEC CODEFLAG


	LDA CONTROL
	BEQ !KEYS
	JSR CONTINUE
;	JMP !NGOT

!KEYS	LDA LIMIT
;	BNE !NGOT
	BEQ !CHK
	JMP !NGOT
!CHK	LDA #$FE
	JSR DETECT
	CMP #$EF
	BNE !NF1
	LDY #0	;cyberdyne
	JSR NOCONTROL
	JSR NEWLIST
	JMP !NF
!NF1	LDA #$FE
	JSR DETECT
	CMP #$DF
	BNE !NF3
	LDY #1	;retrograde
	JSR NOCONTROL
	JSR NEWLIST
	JMP !NF
!NF3	LDA #$FE
	JSR DETECT
	CMP #$BF
	BNE !NF5
	LDY #2	;creatures
	JSR NOCONTROL
	JSR NEWLIST
	JMP !NF
!NF5	LDA #$FE
	JSR DETECT
	CMP #$F7
	BNE !NF
	LDY #3	;information
	JSR NOCONTROL
	JSR NEWLIST

!NF	LDX GAME
	LDY GAMEKEYS,X
!K	LDA KEYCOL,Y
	JSR DETECT
	CMP KEYROW,Y
	BEQ !GOT
	DEY
	BPL !K
	BMI !NGOT
!GOT	LDA CONTROL
	STA TA
	JSR NOCONTROL
	LDA #1
	STA !MSET+1
	LDA STOPF
	BNE !GET
	CPY CURRENTM
;	BEQ !NGOT
	BNE !GET
	LDA TA
	BEQ !NGOT
	LDA #0
	STA !MSET+1
!GET	STY CURRENTM
	JSR BARPOS
!MSET	LDA #$FF
	BEQ !NMSET
	JSR MUSICSTART
!NMSET	JSR DINOSOFF
	STX DINOCOL+2

!NGOT	LDX #$7F
	STX $DC00
	STX $DC01

	JSR TEXTANI

	JSR DEMOSIZE
	JSR DEMOANI

	LDX PULSEO
	LDA UNI4
	BNE !P2
	DEX
	BPL !P
	LDX #PULSEV
!P	STX PULSEO
!P2	LDA PULSETAB,X
	JSR MPULSE

!NP	LDA GAME
	BEQ !MAY
	LDA BDR
	AND #15
	BNE !NMAY
!MAY	;DEC BDR
	JSR MAY1		;andy's stuff
	JSR MAY2
	JSR MAY3		;text titles
	;INC BDR
!NMAY
	JSR TWINKLE

;	LDY CURRENTM
;	CPY #5
;	BEQ !NL
;	LDA LOOPYN,Y
;	BEQ !NL
;	LDA CFFF
;	BNE !NL
;	LDA MUSICLO,Y
;	LDX MUSICHI,Y
;	JSR MUSICSET

!NM	;LDA $DC01
	;AND #$80
	;BNE !NSTOP
	;JSR STOPMUSIC

!NSTOP
	LDA STOPF
	BNE !NMP
	JSR MUSICPLY

!NMP
;	LDA STOPF
;	BNE !NTIME
;	DEC TIME2
;	BNE !NT
;	LDA #100
;	STA TIME2
;	INC TIME
;!NT	LDA TIME
;	TAX
;	AND #15
;	TAY
;	LDA TEMPCHARS,Y
;	STA $4401
;	TXA
;	LSR
;	LSR
;	LSR
;	LSR
;	TAY
;	LDA TEMPCHARS,Y
;	STA $4400
;	LDA #1
;	STA $D800
;	STA $D801
;!NTIME
	JMP LOOP


NOCONTROL	LDA #0
	STA CONTROL
	RTS

DINOSOFF	LDX #4
!DINOS	LDA #0
	STA DINOCOL,X
	LDA SCOLSRE,X
	STA SCOLS+2,X
	DEX
	BPL !DINOS
	RTS


BARPOS	LDA MUSICPOSL.L,X	    ;new pulse position - X:GAME, Y:CURRENTM
	STA TA
	LDA MUSICPOSL.H,X
	STA TA+1
	LDA MUSICPOSH.L,X
	STA TA+2
	LDA MUSICPOSH.H,X
	STA TA+3
	CLC
	LDA (TA),Y
	ADC #80	;bar offset from data
	STA MPOS+1
	LDA (TA+2),Y
	ADC #0
	STA MPOS+2
	RTS

MPULSE	LDX #18	;A:colour
MPOS	STA $D800,X
	DEX
	BPL MPOS
	RTS


TEXTANI	LDA GAME
	BNE !RC
	JSR CSTORE2
	JMP RASTDEL
!RC	JMP CSTORE

;	LDX #39
;!COLS	LDA #5
;	;STA $D8C8,X
;	;STA $D8F0,X
;	STA $D918,X
;	STA $D940,X
;	STA $D968,X
;	STA $D990,X
;	STA $D9B8,X
;	STA $D9E0,X
;	STA $DA08,X
;	STA $DA30,X
;	STA $DA58,X
;	DEX
;	BPL !COLS
;	RTS



;TT-TT+??		positions
;TJ,TJ+1		current position (LOOP copies positions into TJ to calc.)
;TJ+2,TJ+3 	colour position

TWINKNUM	EQU	18	;No. of twinkle-stars
;TWINKPOS	HEX	0044 0045 0046 0044 0045 0046
;	HEX	0044 0045 0046 0044 0045 0046
;	HEX	0044 ;0045 0046 0044 0045 0046
TWINKHI	HEX	44
TWINKX	HEX	00 73 D2 A9 F5 21
	HEX	44 97 CD 58 B1 E2
	HEX	39 15 6B DF 8D C4
TWINKDEL	DS	TWINKNUM,0
TWINKRAN	HEX	09


;TWINKSET	LDX #TWINKNUM-1

;	LDY #TWINKNUM*2-1
;!1	LDA TWINKPOS,Y
;	STA TT,Y
;	DEY
;	BPL !1
;!2

;	LDY TWINKX,X
;	LDA (TT),Y
;	CMP #$EE
;	BNE !NO
;!A	LDA #$F0
;	STA (TT),Y
;!NO	CLC
;	LDA !A+1
;	ADC #1
;	CMP #$FF
;	BCC !OK
;	LDA #$F0
;!OK	STA !A+1
;	DEX
;	BPL !2
;TWINKR	RTS


TWINKLE	LDA #$44
	STA TWINKHI
	LDX #0
!LOOP
;	TXA
;	ASL
;	TAX
;	LDA TT,X		;get colour pos. from screen pos. in TJ
	LDA #0
	STA TJ
	STA TJ+2
	CLC
;	LDA TT+1,X
	LDA TWINKHI
	ADC #1
	CMP #$47
	BCC !N47
	LDA #$44
!N47	STA TWINKHI
	STA TJ+1
	CLC
	ADC #$94
	STA TJ+3
;	TXA
;	LSR
;	TAX

	LDY TWINKDEL,X
	BEQ !ANI
	DEY
	TYA
	STA TWINKDEL,X
	BNE !ELOOP
	LDY TWINKX,X
	LDA (TJ),Y	;
	CMP #$F0	;
	BCC !NO	;
	CMP #$FF	;
	BNE !F0	;
!NO	LDA #1	;
	BNE !GETPOS	;
!F0	LDA #$F0
	STA (TJ),Y
	BNE !COL

!ANI	LDY TWINKX,X
	LDA (TJ),Y
	CMP #$F0
	BCC !NEWPOS
	CMP #$FF
	BEQ !NEWPOS
	CLC
	ADC FC
;	BEQ !ELOOP	;!NEW
	CMP #$FF		;last ani. +1
	BEQ !NEW
	CMP #$F0
	BCC !ELOOP
	STA (TJ),Y
!COL	LDA TWINKX,X		;colour
	AND #15
	BNE !OKC
	LDA #7		;if black, make yellow
!OKC	STA (TJ+2),Y
	BNE !ELOOP


!NEWPOS	LDA #0		;text at pos.
	BEQ !GETPOS

!NEW	LDA #$EE		;blank out finished star
	STA (TJ),Y
;!NEWPOS
!GETPOS	STA !DEL+1
	LDA TWINKRAN		;get new pos...
	ASL
	ASL
	CLC
	ADC #$51
	ADC TWINKRAN
	STA TWINKRAN
	STA TWINKX,X
	TAY
	LDA (TJ),Y		;... and set up ani.
	CMP #$EE
	BNE !ELOOP	;!NEWPOS
;	LDA #1
;	STA (TJ+2),Y
!DEL	LDA #$FF
	BEQ !SET
	LDA TWINKRAN		;if text was at pos. setup delay
	AND #15		;
	STA TWINKDEL,X		;
	JMP !ELOOP		;	BNE !ELOOP
!SET	LDA #$F0
	STA (TJ),Y
	BNE !COL

!ELOOP
	LDA !NUM+1
	CMP #TWINKNUM
	BEQ !I
	DEC TWINKD
	BPL !I
	LDA #10
	STA TWINKD
	INC !NUM+1
!I	INX
!NUM	CPX #0
	BCS !END
	JMP !LOOP
!END	RTS

TWINKD	DB	0


MUSICSTART	LDX GAME		;new music
	LDY CURRENTM
MSTART2	LDA MUSICTABL.L,X
	STA TA
	LDA MUSICTABL.H,X
	STA TA+1
	LDA MUSICTABH.L,X
	STA TA+2
	LDA MUSICTABH.H,X
	STA TA+3
	LDA (TA),Y
	PHA
	LDA (TA+2),Y
	TAX
	PLA
	JSR MUSICSET
	LDA #0
	STA STOPF

;	LDA #0		;TEMP
;	STA TIME
;	LDA #100
;	STA TIME2		;
	RTS

;TIME	DB	0		;
;TIME2	DB	0		;
;TEMPCHARS	HEX	AAABACADAEAFB0B1B2B3 909192939495


M.STOP	LDA #$FF
M.S	STA STOPF
	LDA #0
	STA $D418
	STA CONTROL
	RTS

M.PAUSE	LDA #1
	BNE M.S

M.PLAY	LDA #0
	STA CONTROL
	LDA STOPF
	BMI !SET
	LDA #0
	STA STOPF
	RTS
!SET	LDY CURRENTM
	BPL !M
	LDY #0
	STY CURRENTM
	LDX GAME
	JSR BARPOS
!M	JMP MUSICSTART

M.CONT	LDX #1
M.RC	STX !SAME+1
	LDA CONTROL
	BMI !RAND
!SAME	CMP #$FF
	BEQ !R
!RAND	STX CONTROL
	LDA STOPF
	CMP #1
	BNE !OK
	DEC STOPF
	RTS
!OK	LDA #1
	STA TIMER
	STA TIMERDEL
	STA CONTNEXT
	LDX CONTROL
	BMI !DEC
	LDX CURRENTM
	BPL !R
!DEC	DEC CONTNEXT
!R	RTS

M.RAND	LDX #0
	STX $D418
	DEX
	BNE M.RC


CONTINUE	LDA TIMER
	CMP #$FF
	BEQ !CFFF
	DEC TIMERDEL
	BNE !R
	LDA #100
	STA TIMERDEL
	DEC TIMER
	BEQ !GETONE
!R	RTS
!CFFF	LDA CFFF
	BNE !R

!GETONE	LDX GAME
	LDY CURRENTM
	LDA CONTNEXT
	BEQ !CALC
	DEC CONTNEXT
	BEQ !OKTUNE2

!CALC	LDA CONTROL
	BPL !NEXT
!AGAIN	LDA DEMOSX		;random play
	AND #$3F
	LSR
	SEC
	SBC SRASTC+1
	SEC
	SBC SRASTC+1
	EOR SRASTC+1
!0	LDX #0
!L	CMP GAMETOTALS,X
	BCC !RAND
	INX
	CPX #3
	BCC !L
	LSR
	JMP !0
!RAND	CPX #0
	BEQ !05
	SEC
	SBC GAMETOTALS-1,X

!05	STX !RX+1
;	LDX #7
;!HIS	CMP HISTORY,X
	CMP HISTORY
	BEQ !AGAIN
;	DEX
;	BPL !HIS
;	PHA
;	LDY #6
;!SHIFT	LDA HISTORY,Y
;	STA HISTORY+1,Y
;	DEY
;	BPL !SHIFT
;	PLA
	STA HISTORY
!RX	LDX #$FF

	TAY
	CPX GAME
	BEQ !OKTUNE
	STY !Y+1
	TXA
	TAY
	JSR NEWLIST
!Y	LDY #$FF
	LDX GAME
	BPL !OKTUNE


!NEXT	INY			;continuous play
	TYA
	CMP GAMETUNES,X
	BCC !OKTUNE
	INX		;new game!
	CPX #3
	BCC !OKGAME
	LDX #0
!OKGAME	TXA
	TAY
	JSR NEWLIST
	LDX GAME
	LDY #0

!OKTUNE	STY CURRENTM
!OKTUNE2	LDA TIMERLO,X
	STA TT
	LDA TIMERHI,X
	STA TT+1
	LDA (TT),Y
	STA TIMER
	JSR BARPOS
	JMP MSTART2


;RANDOM	JMP CONTINUE


CONTROL	DB	0	;0:keys, 1:continuous, FF:random
TIMER	DB	0	;length of each tune, FF:wait for finish
TIMERDEL	DB	0	;unit for each
CONTNEXT	DB	0
;CONTROLRAN	DB	$A7	;random generator

TIMERLO	DL	TIMES1,TIMES2,TIMES3
TIMERHI	DH	TIMES1,TIMES2,TIMES3

TIMES1	HEX	FF FF FF FF FF	;FF:wait for finish
TIMES2	HEX	FF 0F 5C 2A 32 52 FF FF 5C 2A 41 30 45 41 3B 32 5E
TIMES3	HEX	FF 17 FF FF FF FF 52 24 4D 4A 49 4A 10 10 10 5C 64 98

HISTORY	DB	0
;	DS	8,255



COLSET	LDA CSPLITL,X
	STA COLTAB+1
	LDA CSPLITZ,X
	STA COLMAX+1
	LDA CSPLITB,X
	BNE !BDR
	LDA #<COLBYTE
	LDY #>COLBYTE
	JMP !S
!BDR	LDA #$D0
	LDY #$20
!S	STA COLBDR+2
	STY COLBDR+1
	RTS

CSPLITL	DL	C1,C2,C3
CSPLITZ	HEX	11 08 04
CSPLITB	HEX	00 01 01		;00:screen, 01:screen + border




CDATA	HEX 08090A0B0C0D0E0F10111213101512171019121B1C161D671E1F20212223242526FF27FF28FF29FF2A2B2C2D2EFF2FFF303133343036
	HEX 333738393A3B3C3D3E3F40414243444645FF4748494A4B4C4D4E4F505152535455565A5B57585D5E5F6061626364656667688B8C8D8E
	HEX 69186A1A6B6C6D6E6F707172737475767778797A7B7C7D7E7F808182838485868788898A06FF07FFFFFF05FFFFFF04FF0001020314FF
	HEX 05FF32FF35FF5CFF59FFFFFFFFFF


LTABLO	DL	GAME1,GAME2,GAME3
LTABHI	DH	GAME1,GAME2,GAME3

GAME1	DB	"          INFORMATION",255
GAME2	DB	"          RETROGRADE",255
GAME3	DB	"           CREATURES",255

LT.CHAR	DB	"!.,?:"
LT.SIZE	DB	0,0,0,1,0		;0=1, 1=2 chars wide

; LTABLO
LTPRINT	STA LTPOS+2		;Y: offset, A:hi pos, X:lo pos
	STX LTPOS+1
	LDA LTABLO,Y		;for '"' use: '
	STA !DATA+1		;and end data with '255'
	LDA LTABHI,Y
	STA !DATA+2
	LDY #0

!LOOP	STY !REY+1
	LDA #0
	STA MW
	STA MW+1
!DATA	LDA $FFFF,Y
	CMP #$FF
	BNE !6
	JMP !OUT
!6	CMP #" "
	BNE !NSP
	LDX #1
	STX !S+1
	JMP !SPACE
!NSP	LDY #4
!YL	CMP LT.CHAR,Y
	BEQ !GOT
	DEY
	BPL !YL
	BMI !NGOT
!GOT	TYA
	CLC
	ADC #$25
	LDX LT.SIZE,Y
	BNE !NI
	BEQ !0
!NGOT	CMP #$40	;the end (hiscore)
	BNE !NE
	LDA #$32
	BNE !NI
!NE	CMP #$3C	;del.
	BNE !ND
	LDA #$31
	BNE !NI
!ND	CMP #"A"
	BCS !N0TO9
	ADC #$2B
	JMP !NI
!N0TO9	CMP #"M"
	BNE !NM
	LDY #1
	BNE !WM
!NM	CMP #"W"
	BNE !NW
	LDY #$FF
!WM	STY MW
	BNE !NI
!NW	CMP #"J"
	BEQ !0
	CMP #"L"
	BEQ !0
	CMP #"I"
	BNE !NI
	BEQ !0

!B0	LDY #0
	STY MW
!0	LDX #1	;jump here for 1 width
	BNE !SS
!NI	LDX #2	;or here for 2
!SS	STX !S+1
	SEC
	SBC #1
	ASL
	ASL
	TAX
	LDY #0
	LDA CDATA,X
	JSR LTPOS
	LDA CDATA+1,X
	JSR LTPOS
	LDY #40
	LDA CDATA+2,X
	JSR LTPOS
	LDA CDATA+3,X
	JSR LTPOS
!SPACE	CLC
	LDA LTPOS+1
!S	ADC #2
	STA LTPOS+1
	BCC !MW
	INC LTPOS+2
!MW	LDA MW
	BEQ !REY
	BMI !W
	LDA #$2A	;!M
	BNE !B0
!W	LDA #$2B	;!W
	BNE !B0
!REY	LDY #$FF
	INY
!JL	JMP !LOOP
!OUT	RTS

LTPOS	STA $FFFF,Y
	INY
	RTS

MW	DB	0,0



;RLO2	;*DL	SCOL1+1,SCOL2+1,SCOL3,SCOL4
;RHI2	;*DH	SCOL1+1,SCOL2+1,SCOL3,SCOL4
;RSZ	;*DB	9,8,20,15


CODEFLAG	DB	0
GAME	DB	1	;current game selected (0-2)
JDATAB	DS	5,0



;*COLOFF	DB	0
;*COLLO	HEX	E0 08 30 58 80 A8 D0
;*COLHI	HEX	D9 DA DA DA DA DA DA
;*COLTAB	DB	14,12,11,15,9,9,15,11,12,14

PULSEF	DB	0





;CHECKTAB1	HEX	7F 7F FD FD FB FB F7
;CHECKTAB2	HEX	FE F7 FE F7 FE F7 FE

KEYCOL	HEX	FDF7FBFBFDFBF7F7EFEFEFDFEFEFEFDF7FFBFDFBF7F7FDFBF7FD
KEYROW	HEX	FBEFEFFBBFDFFBDFFDFBDFFBEF7FBFFDBFFDDFBFBF7FFD7FFDEF

MUSICTABL.L	DL MUSICLO1,MUSICLO2,MUSICLO3
MUSICTABL.H	DH MUSICLO1,MUSICLO2,MUSICLO3
MUSICTABH.L	DL MUSICHI1,MUSICHI2,MUSICHI3
MUSICTABH.H	DH MUSICHI1,MUSICHI2,MUSICHI3

MUSICLO1	DL	CW.MUSIC,CW.MUSIC+$4FD,CW.MUSIC+$611,CW.MUSIC+$C0D,CW.MUSIC+$B23
MUSICHI1	DH	CW.MUSIC,CW.MUSIC+$4FD,CW.MUSIC+$611,CW.MUSIC+$C0D,CW.MUSIC+$B23
MUSICLO2	DL	R.TITLEM,R.GETREADYM,R.SHOPM,R.WELLDONE,R.GAMEOVERM
	DL	R.HIGHM,R.DINTRO,R.DBIT,R.LOADM,R.MOTHER1,R.MOTHER2,R.MOTHER3
	DL	R.MOTHER4,R.MOTHER5,R.MOTHER6,R.MOTHER7,R.GCOMP
MUSICHI2	DH	R.TITLEM,R.GETREADYM,R.SHOPM,R.WELLDONE,R.GAMEOVERM
	DH	R.HIGHM,R.DINTRO,R.DBIT,R.LOADM,R.MOTHER1,R.MOTHER2,R.MOTHER3
	DH	R.MOTHER4,R.MOTHER5,R.MOTHER6,R.MOTHER7,R.GCOMP
MUSICLO3	DL	C.TITLEM,C.GRMUSIC,C.SHOPM,C.LCOMPM,C.TIMERM,C.DEATHM
	DL	C.HIGHM,C.INTROM,C.GC.MUSIC,C.INGAMEM,C.INGAMEM2,C.INGAMEM3
	DL	C.ENDLEVELM,C.ENDLEVELM2,C.ENDLEVELM3,C.TMUSIC1
	DL	C.TMUSIC2,C.TMUSIC3
MUSICHI3	DH	C.TITLEM,C.GRMUSIC,C.SHOPM,C.LCOMPM,C.TIMERM,C.DEATHM
	DH	C.HIGHM,C.INTROM,C.GC.MUSIC,C.INGAMEM,C.INGAMEM2,C.INGAMEM3
	DH	C.ENDLEVELM,C.ENDLEVELM2,C.ENDLEVELM3,C.TMUSIC1
	DH	C.TMUSIC2,C.TMUSIC3


MUSICPOSL.L	DL PULSELO1,PULSELO2,PULSELO3
MUSICPOSL.H	DH PULSELO1,PULSELO2,PULSELO3
MUSICPOSH.L	DL PULSEHI1,PULSEHI2,PULSEHI3
MUSICPOSH.H	DH PULSEHI1,PULSEHI2,PULSEHI3

PULSEHI1	HEX	D8 D8 D9 D9 D9
PULSELO1	HEX	D4 FC 24 4C 74
;				  **
PULSEHI2	HEX	D8 D8 D9 D9 D9 D9 D9 D9 DA D8 D9 D9 D9 D9 D9 D9 D9
PULSELO2	HEX	CA F2 1A 42 6A 92 BA E2 0A DD 05 2D 55 7D A5 CD F5
;PULSEHI2	HEX	D8 D8 D9 D9 D9 D9 D9 D9 D8 D9 D9 D9 D9 D9 D9 D9
;PULSELO2	HEX	CA F2 1A 42 6A 92 BA E2 DD 05 2D 55 7D A5 CD F5

PULSEHI3	HEX	D8 D8 D9 D9 D9 D9 D9 D9 DA D8 D9 D9 D9 D9 D9 D9 D9 DA
PULSELO3	HEX	CA F2 1A 42 6A 92 BA E2 0A DD 05 2D 55 7D A5 CD F5 1D


FC	DB	0
FC2	DB	0
SELECTO	DB	0
PULSEO	DB	0
CURRENTM	DB	255
STOPF	DB	255	;0:play, 1:pause, FF:stop



;STOPM	HEX 11001E000001010F2503232F0000A65019C300011E0001232F00009A5019C300
;	HEX 011E0001232F0000A15019C300011E0341202002080A0A04080E4115D224



BANKIN	HEX 00C0E8FDDB0000000000008010000000000000000000000000000000000004000000000046EFEF4600010377FFFFFB7100E3F7FFFFFF
	HEX F7E1E0F8FCFDFFFFFDF80000E0F0F6FFFFE6000000000040E4400000000000000000000000000000000000000000000000000000095D
	HEX 08000000020609192566959600408090A0A468A9010004001100410000008000A0006800000000000000000000000000100058000000
	HEX 000000000001555A555A556A556A6AAA5AAA5AAA56AA45409590A5A4A9A96A005A005A00560001008400A000A9001A00160056005501
	HEX 02068919A565959555AA55AA55AA55AA56AA55AA55AA55AAAAAAAAAAAAAA6AAA56409590A5A4A9A9AA02A909A526959656069999A5A5
	HEX 69A95555555555565556AA99669955451144AAA9669956451144AAAA6A9966591144
	DS	16,0

LANDSCAPE	HEX C8C9CACBCCCDCECFD0D1D2D3D4D5C8C9CACBCCCDCECFD0D1D2D3D4D5C8C9CACBCCCDCECFD0D1D2D3
	HEX D6D7D8D9DADBDCD6D7D8D9DADBDCD6D7D8D9DADBDCD6D7D8D9DADBDCD6D7D8D9DADBDCD6D7D8D9DA
	HEX DDDEDFE0E1E2E3DDDEDFE0E1E2E3DDDEDFE0E1E2E3DDDEDFE0E1E2E3DDDEDFE0E1E2E3DDDEDFE0E1
	HEX E4E5E6E7E8E9EAE4E5E6E7E8E9EAE4E5E6E7E8E9EAE4E5E6E7E8E9EAE4E5E6E7E8E9EAE4E5E6E7E8
	HEX EBECEDEBECEDEBECEDEBECEDEBECEDEBECEDEBECEDEBECEDEBECEDEBECEDEBECEDEBECEDEBECEDEB



TITLE.CW	HEX 101161620C0D101944461015616230361019FFFF5A5B5C0809444644462638394446
	HEX 121363640E0F121B45FF121763643337121BFFFF5758590A0B45FF45FF273A3B45FF

TITLE.R	HEX FFFFFFFFFFFFFF444610194B4C444638391E1F4446080910151019FFFFFFFFFFFFFF
	HEX FFFFFFFFFFFFFF45FF121B4D4E45FF3A3B202145FF0A0B1217121BFFFFFFFFFFFFFF

TITLE.C	HEX FFFFFFFFFFFFFFFF10114446101908094B4C4F50444610194748FFFFFFFFFFFFFFFF
	HEX FFFFFFFFFFFFFFFF121345FF121B0A0B4D4E515245FF121B494AFFFFFFFFFFFFFFFF

TITLE.I	HEX FFFFFFFFFFFFFF2630361C163839444630313208094B4C2638393036FFFFFFFFFFFF
	HEX FFFFFFFFFFFFFF2733371D673A3B45FF3334350A0B4D4E273A3B3337FFFFFFFFFFFF


TITLE.LO	DL	TITLE.CW,TITLE.R,TITLE.C,TITLE.I
TITLE.HI	DH	TITLE.CW,TITLE.R,TITLE.C,TITLE.I
TITLENGTH	EQU	34	;window size/2
TITXPOS	EQU	3	;spaces across from left edge of screen


****************************************
*	andy's bits		*
****************************************


MAY3	LDX POS
	LDA MODELO,X
	STA !GET+1
	LDA MODEHI,X
	STA !GET+2
	LDY MODEO
	LDA UNI2
	BNE !X
	DEY
	BPL !Y
	LDY #MODEV
!Y	STY MODEO
!X	LDX #14
!GET	LDA $FFFF,X
	BEQ !R
	CMP #" "
	BNE !NSPC
	LDA #$EE
	BNE !PUT
!NSPC	CLC
	ADC #$4F
!PUT	STA $4705,X
	LDA MODECOLS,Y
	STA $DB05,X
	DEX
	BPL !GET
!R	RTS

MODELO	DL	MODE.1,MODE.2,MODE.3,MODE.4,MODE.5
MODEHI	DH	MODE.1,MODE.2,MODE.3,MODE.4,MODE.5
MODE.1	DB	"CONTINUOUS PLAY"
MODE.2	DB	"     STOP      "
MODE.3	DB	"     PLAY      "
MODE.4	DB	"     PAUSE     "
MODE.5	DB	"  RANDOM PLAY  "

MODEO	DB	0,0,6
MODECOLS	HEX	0000000B050D0101010D050B
MODEC
MODEV	EQU MODEC-MODECOLS-1


MAY1	lda limit
	bne !ROOT	;!loop

	lda jump
	bne !ROOT	;!loop

;	lda joy2
;	and #1
;	bne !nup
;
;!up	jmp !ndown
;
;!nup	lda joy2
;	and #2
;	bne !ndown
;
;!down
;
;!ndown
	lda joy2
	and #4
	bne !nleft

!left	lda pos
	beq !nright
	lda #255
	sta rolyorn
	sta anidir
	sta anidirs
	inc limit
	jmp !nright

!nleft	lda joy2
	and #8
	bne !nright

!right	lda pos
	cmp #4
	beq !nright
	lda #1
	sta rolyorn
	sta anidir
	sta anidirs
	inc limit

!nright	lda joy2
	and #16
	bne !nfire

!fire	LDA LIMIT
	BNE !ROOT
	lda #jumpval
	sta jump

!nfire
!root	;jmp !loop
	RTS





;codeflag	db	0
;fc	db	0
pos	db	2	;start in centre
limit	db	0
rolyorn	db	0
anidir	db	0
anidirs	db	0
msb	HEX	40



MAY2	jsr jumper
	jsr animate

;	inc bdr
	jsr toproll4
	jsr toproll1
	jsr toproll3

;	inc bdr
	jsr movedino

	lda limit
	beq !nol
	inc limit
	cmp #28
	bne !over

	lda anidirs
	bmi !lf
	inc pos
	bne !nol
!lf	dec pos

!nol	lda #0
	sta rolyorn
	sta anidir
	sta limit

!over
;	inc bdr
;	jsr sprsplit

;	lda #3
;	sta bdr

;	lda #24*8+49
;	ldx #<rast2a
;	ldy #>rast2a
	rts



toproll1	lda fc
	bne !skip

	lda rolyorn
	beq !skip
	bmi !left

	ldx #7
!l0	lda $5000+($c8*8),x
	asl a
	rol $5000+($d5*8),x
	rol $5000+($d4*8),x
	rol $5000+($d3*8),x
	rol $5000+($d2*8),x
	rol $5000+($d1*8),x
	rol $5000+($d0*8),x
	rol $5000+($cf*8),x
	rol $5000+($ce*8),x
	rol $5000+($cd*8),x
	rol $5000+($cc*8),x
	rol $5000+($cb*8),x
	rol $5000+($ca*8),x
	rol $5000+($c9*8),x
	rol $5000+($c8*8),x
	dex
	bpl !l0
!skip	rts

!left	ldx #7
!ll0	lda $5000+($d5*8),x
	lsr a
	ror $5000+($c8*8),x
	ror $5000+($c9*8),x
	ror $5000+($ca*8),x
	ror $5000+($cb*8),x
	ror $5000+($cc*8),x
	ror $5000+($cd*8),x
	ror $5000+($ce*8),x
	ror $5000+($cf*8),x
	ror $5000+($d0*8),x
	ror $5000+($d1*8),x
	ror $5000+($d2*8),x
	ror $5000+($d3*8),x
	ror $5000+($d4*8),x
	ror $5000+($d5*8),x
	dex
	bpl !ll0
	rts


toproll3	lda fc
	BEQ !skip1
	lda rolyorn
	beq !skip1
	bmi !lefty

	ldy #1
!mm0	ldx #7
!l1	lda $5000+($d6*8),x
	asl a
	rol $5000+($dc*8),x
	rol $5000+($db*8),x
	rol $5000+($da*8),x
	rol $5000+($d9*8),x
	rol $5000+($d8*8),x
	rol $5000+($d7*8),x
	rol $5000+($d6*8),x

	lda $5000+($dd*8),x
	asl a
	rol $5000+($e3*8),x
	rol $5000+($e2*8),x
	rol $5000+($e1*8),x
	rol $5000+($e0*8),x
	rol $5000+($df*8),x
	rol $5000+($de*8),x
	rol $5000+($dd*8),x

	lda $5000+($e4*8),x
	asl a
	rol $5000+($ea*8),x
	rol $5000+($e9*8),x
	rol $5000+($e8*8),x
	rol $5000+($e7*8),x
	rol $5000+($e6*8),x
	rol $5000+($e5*8),x
	rol $5000+($e4*8),x
	dex
	bpl !l1
	dey
	bpl !mm0
!skip1	rts

!lefty	ldy #1
!m0	ldx #7
!ll1	lda $5000+($dc*8),x
	lsr a
	ror $5000+($d6*8),x
	ror $5000+($d7*8),x
	ror $5000+($d8*8),x
	ror $5000+($d9*8),x
	ror $5000+($da*8),x
	ror $5000+($db*8),x
	ror $5000+($dc*8),x

	lda $5000+($e3*8),x
	lsr a
	ror $5000+($dd*8),x
	ror $5000+($de*8),x
	ror $5000+($df*8),x
	ror $5000+($e0*8),x
	ror $5000+($e1*8),x
	ror $5000+($e2*8),x
	ror $5000+($e3*8),x

	lda $5000+($ea*8),x
	lsr a
	ror $5000+($e4*8),x
	ror $5000+($e5*8),x
	ror $5000+($e6*8),x
	ror $5000+($e7*8),x
	ror $5000+($e8*8),x
	ror $5000+($e9*8),x
	ror $5000+($ea*8),x
	dex
	bpl !ll1
	dey
	bpl !m0
	rts


toproll4	lda rolyorn
	beq !skip4
	bmi !lefty2

	ldy #1
!mum2	ldx #7
!l6	lda $5000+($eb*8),x
	asl a
	rol $5000+($ed*8),x
	rol $5000+($ec*8),x
	rol $5000+($eb*8),x
	dex
	bpl !l6
	dey
	bpl !mum2
!skip4	rts

!lefty2	ldy #1
!m2	ldx #7
!ll6	lda $5000+($ed*8),x
	lsr a
	ror $5000+($eb*8),x
	ror $5000+($ec*8),x
	ror $5000+($ed*8),x
	dex
	bpl !ll6
	dey
	bpl !m2
	rts

;pltext	rts

animate	;lda fc
	;bne !no

	LDY #4
!DUCKYN	LDX DINODEL,Y
	BEQ !NDUCK
	DEX
	BNE !DINODEL
	SEC
	LDA SPOINT+2,Y
	SBC #5
	STA SPOINT+2,Y
!DINODEL	TXA
	STA DINODEL,Y

!NDUCK	LDA DINOCOL,Y
	BEQ !NCOL
	LDX DINOCOLO
	DEX
	BPL !DC
	LDX #DINOCV
!DC	STX DINOCOLO
	LDA SCOLLO,Y
	STA !COL+1
	LDA SCOLHI,Y
	STA !COL+2
!COL	LDA $FFFF,X
	STA SCOLS+2,Y
!NCOL	DEY
	BPL !DUCKYN

	lda jump
	bne !jmp
	LDA UNI2
	BNE !NO
	lda anidir
	beq !stance
	bmi !mayl

!mayr	ldx anbyte
	lda mayrh,x
	sta apoint1
	lda mayrm,x
	sta apoint2
	bne !jo

!mayl	ldx anbyte
	lda maylh,x
	sta apoint1
	lda maylm,x
	sta apoint2

!jo	dec anbyte
	bpl !no
	lda #7
	sta anbyte
!no	rts

!jmp	lda anidirs
	bmi !jmpl
	lda mayjr
	ldx mayjr+1
	bne !store
!jmpl	lda mayjl
	ldx mayjl+1
	bne !store

!stance	lda anidirs
	bmi !stl
!str	lda maysr
	ldx maysr+1
	bne !store
!stl	lda maysl
	ldx maysl+1
!store	sta apoint1
	stx apoint2
	rts

anbyte	db	7

mayrh	hex	6e6c6a6866646260
mayrm	hex	6f6d6b6967656361

maylh	hex	82807e7c7a787674
maylm	hex	83817f7d7b797775

maysr	hex	7071
maysl	hex	8485

mayjr	hex	7273
mayjl	hex	8687


movedino	lda rolyorn
	beq !rts

	ldx #4
!loop	ldy #0
	lda dinlo,x
	sta ta
	lda dinhi,x
	sta ta+1

	lda rolyorn
	bmi !mleft

	sec
	lda (ta),y
	sbc #2
	sta (ta),y
	bcs !ok
	jmp !z

!mleft	clc
	lda (ta),y
	adc #2
	sta (ta),y
	bcc !ok

!z	lda msb
	eor dinmsb,x
	sta msb
!ok	dex
	bpl !loop
!rts	rts


dinlo	dL	din1,din2,din3,din4,din5
dinhi	dH	din1,din2,din3,din4,din5

dinmsb	hex	0408102040

jumper	ldx jump
	beq !rts
	LDA JUMPT-1,X
	CMP #$80
	BEQ !HIT
	CLC
	ADC SPOS+1
	sta spos+1
	sta spos+3
!DEX	dex
	stx jump
!rts	rts
!HIT	STX !X+1
	LDY POS
	LDX #4
!COL	LDA SCOLSRE,X
	STA SCOLS+2,X
	LDA #0
	STA DINOCOL,X
	DEX
	BPL !COL
	LDA #10
	STA DINODEL,Y
	STA DINOCOL,Y
	CLC
	LDA SPOINT+2,Y
	ADC #5
	STA SPOINT+2,Y
	LDA MODECODEL,Y
	STA !JSR+1
	LDA MODECODEH,Y
	STA !JSR+2
!JSR	JSR $FFFF
!X	LDX #$FF
	JMP !DEX

;jumpval	db	27
jump	db	0

jumpt	HEX	03030203020201020101000100
	HEX	000000FF00FFFFFE
	HEX	8001020101000100
	HEX	000000FF00FFFFFFFEFFFEFEFDFEFDFD
JUMPC
JUMPVAL	EQU	JUMPC-JUMPT

DINOCOL	DS	5,0
DINODEL	DS	5,0
MODECODEL	DL	M.CONT,M.STOP,M.PLAY,M.PAUSE,M.RAND
MODECODEH	DH	M.CONT,M.STOP,M.PLAY,M.PAUSE,M.RAND


MAYSPLIT	LDY #2
	ldx #1
!s1	lda spoint,x
	sta $47f8,x
	lda spos,Y
	sta $d000,Y
	LDA SPOS+1
	STA $D001,Y
	lda scols,x
	sta $d027,x
	DEY
	DEY
	dex
	bpl !s1

	lda #0
	sta smc2
	STA BIGY
	STA BIGX
	lda #7
	sta smc1
	RTS

sprsplit	LDY #10
	ldx #5
!s1	lda spoint+2,x
	sta $47fA,x
	lda spos+4,Y
	sta $d004,Y
	LDA SPOS+5,Y
	STA $D005,Y
	lda scols+2,x
	sta $d029,x
	DEY
	DEY
	dex
	bpl !s1

	lda msb
	sta $d010
	rts


SCOLLO	DL	DCOL1,DCOL2,DCOL3,DCOL4,DCOL1
SCOLHI	DH	DCOL1,DCOL2,DCOL3,DCOL4,DCOL1

DCOL1	HEX	0B0B0C0C0F0F0C0C
DCOL2	HEX	02020A0A0F0F0A0A
DCOL3	HEX	0B0B05050D0D0505
DCOL4	HEX	06060E0E03030E0E
DCC
DINOCV	EQU	DCC-DCOL4-1

DINOCOLO	DB	0

SCOLSRE	HEX	     0C0A050E0C
scols	hex	000a 0C0A050E0C00
spos	hex	ace6ace6
din1	hex	3ce5
din2	hex	74e5
din3	hex	ace5
din4	hex	e4e5
din5	hex	1ce5
	hex	00E5
spoint	hex	84858a8b8c8d8e4D


smc1	EQU	$d025
smc2	EQU	$d026
bigx	EQU	$d01d
bigy	EQU	$d017
apoint1	EQU	spoint
apoint2	EQU	spoint+1






c23	HEX	0402080A0707010D03050E040B06
	HEX	0402080A0707010D
;	hex	0902080a0f07010d03050e040b06
;	hex	0902080a0f07010d

leftloc	=	$d8ca+80	;top left position (3rd char in)
rightloc	=	$d8dc+80	;top right
midloc	=	$d924

loff	db	8
roff	db	21
moff	db	4

cstore	lda fc
	bne !c2

	LDA GAME
	CMP #3
	BNE !NINFO
	LDA #>LOFF
	LDY #<LOFF
	BNE !C
!NINFO	LDA #>ROFF
	LDY #<ROFF
!C	STA !P1+1
	STY !P1+2
	ldx #19
!p1	ldy roff
	lda c23,y
	sta rightloc,x
	dey
	lda c23,y
	sta rightloc+40,x
	dey
	lda c23,y
	sta rightloc+80,x
	dey
	lda c23,y
	sta rightloc+120,x
	dey
	lda c23,y
	sta rightloc+160,x
	dey
	lda c23,y
	sta rightloc+200,x
	dey
	lda c23,y
	sta rightloc+240,x
	dey
	lda c23,y
	sta rightloc+280,x
	dey
	lda c23,y
	sta rightloc+320,x
	dex
	bpl !p1

	lda roff
	cmp #8
	bne !o
	lda #21
	sta roff
!rt	rts
!o	dec roff
	rts

!c2	ldx #17
!p2	ldy loff
	lda c23,y
	sta leftloc,x
	dey
	lda c23,y
	sta leftloc+40,x
	dey
	lda c23,y
	sta leftloc+80,x
	dey
	lda c23,y
	sta leftloc+120,x
	dey
	lda c23,y
	sta leftloc+160,x
	dey
	lda c23,y
	sta leftloc+200,x
	dey
	lda c23,y
	sta leftloc+240,x
	dey
	lda c23,y
	sta leftloc+280,x
	dey
	lda c23,y
	sta leftloc+320,x
	dex
	bpl !p2

	lda loff
	cmp #21
	bne !ok
	lda #8
	sta loff
	rts
!ok	inc loff
	rts


cstore2	lda fc
	bne !out

	ldx #16
!p3	ldy moff
	lda c23,y
	sta midloc,x
	dey
	lda c23,y
	sta midloc+40,x
	dey
	lda c23,y
	sta midloc+80,x
	dey
	lda c23,y
	sta midloc+120,x
	dey
	lda c23,y
	sta midloc+160,x
	dex
	bpl !p3

	lda moff
	cmp #17
	bne !ok
	lda #4
	sta moff
	rts
!ok	inc moff
!out	rts








;
;  PDS Pc1.21 :000: (c) P.D.Systems Ltd 1985-88
;
;
;       File  " IN-GAME ROUTINES! "
;    Version          11.20
;         By  "THIS IS STILL JAZ.R"
;
;     Created on Sat the 14th of May 1988
;        Last update 00:24 on 01/01/80
;


NEWLIST			;Y:game offset (0-2)
	CPY GAME
	BNE !NEW
	RTS
!NEW	STY GAME
	LDA CONTROL
	BNE !NOFF
	LDA CURRENTM
	BMI !NOFF
	LDA #<COLBUFFER
	STA MPOS+2
	LDA #>COLBUFFER
	STA MPOS+1
	LDA #$FF
	STA CURRENTM
	JSR M.STOP

!NOFF	LDX #79
	LDA #$FF
!1	STA $4428,X
	DEX
	BPL !1

;	LDA #$44
;	LDX #$28
;	LDY GAME
;	JSR LTPRINT
;	JMP $1600

	LDY GAME
	CLC
	LDA TITLE.LO,Y
	STA !TIT+1
	ADC #TITLENGTH
	STA !TIT2+1
	LDA TITLE.HI,Y
	STA !TIT+2
	ADC #0
	STA !TIT2+2
	LDX #TITLENGTH-1
!TIT	LDA $FFFF,X
	STA $4428+TITXPOS,X
!TIT2	LDA $FFFF,X
	STA $4450+TITXPOS,X
	DEX
	BPL !TIT

	LDX GAME
	LDA HIGHX,X
	STA GAMEXY

	LDX #0
!5	LDA #$EE
	STA $44C8,X
	STA $45C8,X
	DEX
	BNE !5

	LDA CONTROL
	BNE !NDOFF
	JSR DINOSOFF
!NDOFF
	LDX GAME
	LDA #$45	;GAMELISTPH,X
	STA TA+3
	LDA GAMELISTPL,X
	STA TA+2
	LDA GAMELISTH,X
	STA TA+1
	LDA GAMELISTL,X
	STA TA
!M	LDY #0
	LDA (TA),Y
	BEQ !E
	CMP #" "
	BNE !NSPC
	LDA #$EE
	BNE !P
!NSPC	CMP #"."
	BNE !NDOT
	LDA #$B4
	BNE !P
!NDOT	CMP #"("
	BNE !NBL
	LDA #$B6
	BNE !P
!NBL	CMP #")"
	BNE !NBR
	LDA #$B7
	BNE !P
!NBR	CMP #":"
	BNE !NCOL
	LDA #$B5
	BNE !P
!NCOL	CMP #"`"
	BNE !NPOU
	LDA #$C6
	BNE !P
!NPOU	CMP #"0"
	BCC !NONUM
	CMP #"9"+1
	BCS !NONUM
	ADC #$7A
	BCC !P
!NONUM	CMP #1
	BNE !ADC
	INY
	LDA (TA),Y
	CLC
	ADC TA+2
	STA TA+2
	BCC !OK
	INC TA+3
!OK	DEY
	JSR NEWADDM
	JSR NEWADDM
	JMP !M

!ADC	CLC
	ADC #$4F
!P	STA (TA+2),Y
	JSR NEWADDM
	CLC
	LDA TA+2
	ADC #1
	STA TA+2
	BCC !3
	INC TA+3
!3	JMP !M
!E
!R	RTS

NEWADDM	CLC
	LDA TA
	ADC #1
	STA TA
	BCC !R
	INC TA+1
!R	RTS



COLOFF	DB	0
COLOFFT	DB	255
STARCOLO	DB	0
STARCOLEOR	DB	0
STARCOLS	HEX	0604040E0E03030D0D0101
;	HEX	0B0C0C0C0C0F0F0F070101
STC
STCV	EQU	STC-STARCOLS-1

****************************************
*	raster list		*
****************************************


SRASTOP	LDA #$FF
	STA $D015
	LDA UNI2
	BNE !NC
	LDY #1
!L	LDX MODEO+1,Y
	LDA MODECOLS,X
	STA GAMEC+6,Y
	DEX
	BPL !OK
	LDX #MODEV
!OK	TXA
	STA MODEO+1,Y
	DEY
	BPL !L
!NC	LDA #1*8+49
	LDX #>SRAST2
	LDY #<SRAST2
	RTS


SRAST2	;JSR CRAST
	JSR DELAY+18
SRASTC	LDA #6
	STA SCR
	JSR TLC1

	LDY STARCOLO
	BEQ !0
	DEC STARCOLO
!0	LDX #5	;7
!C	LDA STARCOLS,Y	;DEMOCOLS,X
	STA $D029,X
	DEX
	BPL !C
;	LDY COLOFF
;	DEY
;	BPL !Y
;	LDY #15
;!Y	STY COLOFF
;	LDX #5
;!C	LDA THCOLS,Y
;	STA $D029,X
;	INY
;	DEX
;	BPL !C

	STX $D01B
	INX
	STX $D01C
	STX $D01D
	STX $D017
	LDA #8*3+50
	LDX #>SRAST2A
	LDY #<SRAST2A
	RTS

SRAST2A	LDA #0
	STA SCR
	JMP TLC3

;star rasts here...

SRAST2B	LDX #1
	JSR COLSET
	JSR MAYSPLIT
	LDA #$C7
	LDX #>SRAST2C
	LDY #<SRAST2C
	RTS

SRAST2C	lda #$fe
	sta $d01c
	ldX #$ff
	stX $d015
	INX
	STX $D01B
	LDA #19*8+49
	LDX #>SRAST3
	LDY #<SRAST3
	RTS

SRAST3	JSR CRAST
	LDA $D016
	ORA #$10
	STA $D016
	JSR SPRSPLIT

	LDA #1
	STA CODEFLAG
	LDA FC
	EOR #1
	STA FC
	BNE !2
	LDA FC2
	EOR #1
	STA FC2
!2	JSR UNI
	DEC UNI7
	BPL !N6
	LDA #7
	STA UNI7
!N6
	LDA #$F1
	LDX #>SRASTB
	LDY #<SRASTB
	RTS

SRASTB	LDA #10
	STA SCR
	LDA #$F9
	LDX #>SRASTB2
	LDY #<SRASTB2
	RTS

SRASTB2	JSR NOBORDERS
	JSR BOTFADE

	LDA #0
	STA $D015
	LDX #7
!SIC	LDA GAMEI,X
	STA $47F8,X
	LDA GAMEC,X
	STA $D027,X
	DEX
	BPL !SIC
	LDX #15
!SXY	LDA GAMEXY,X
	STA $D000,X
	DEX
	BPL !SXY
	LDA #11
	STA $D025
	LDA #7
	STA $D026
	LDA #%00100000
	STA $D01C
	LDA #%00000001
	STA $D01D
	STA $D017
;	LDA #0
;	STA $D015
	LDA #0
	STA $D010
	STA $D01B

	LDA UNI2	;FC
	BNE !NP
	LDX SELECTO
	DEX
	BPL !P
	LDX #SELV
!P	LDA SELECTAB,X
	STA GAMEC
	STX SELECTO
!NP
	LDA $D016
	AND #$EF
	STA $D016

	LDX #0
	JSR COLSET

	LDA FC	;UNI3
	BNE !NTIT
	LDX COLOFFT
	DEX
	BPL !X
	LDX #TITV
!X	STX COLOFFT
!CL	LDA TITCOLS,X
	STA SRASTC+1
	STX COLOFFT
!NTIT
	JSR DEMOSMODX
	JSR DEMOSMODY

	LDA #0
	LDX #>SRASTOP
	LDY #<SRASTOP
	RTS



TITCOLS	HEX	0606040E05030D010107070F0A080209
;	HEX	02040A0D0E0406
TITC
TITV	EQU	TITC-TITCOLS-1



;ROLLRASTS	LDA RLO2,X	;x  : offset
;	STA $54	;sec: down
;	LDA RHI2,X	;clc: up
;	STA $55
;	LDY RSZ,X	;length
;	STY $56
;	BCS !DOWN
;
;!UP	LDA ($54),Y
;	PHA
;	LDY $56
;	DEY
;!UL	LDA ($54),Y
;	INY
;	STA ($54),Y
;	DEY
;	DEY
;	BPL !UL
;	INY
;	PLA
;	STA ($54),Y
;	RTS
;
;!DOWN	LDY #0
;	LDA ($54),Y
;	PHA
;	INY
;!RU	LDA ($54),Y
;	DEY
;	STA ($54),Y
;	INY
;	INY
;	CPY $56
;	BCC !RU
;	DEC $56
;	LDY $56
;	PLA
;	STA ($54),Y
;	RTS


GAMEI	HEX	99C0C1C2C3C4C5C6	;AAABACADAEAF 0F0F	;A2A3A4A5A6A7 0F0F
GAMEC	HEX	010C0B0E060A0E0D
GAMEXY	HEX	4B0E 6120 6120 9320 9320 C520 F720 F720
;	HEX	640E 7A20 7A20 AC20 AC20 DE20 0000 0000

HIGHX	HEX	4B7DAFE2
;	HEX	6496C8

SELECTAB	HEX	010101010101010101010101010101010101010F0C0B00000B0C0F
;	HEX	0006040E030D01010101010101070F0A0809
SELE
SELV	EQU SELE-SELECTAB-1

PULSETAB	HEX	000101
;	HEX	0B0C0F01010101010101010F0C0B0000
;	HEX	06040E030D03050B00
PULSEE
PULSEV	EQU PULSEE-PULSETAB-1

GAMELISTL	DL	GL1,GL2,GL3,GL4
GAMELISTH	DH	GL1,GL2,GL3,GL4
GAMELISTPH	;HEX	45 45 45 45	;44 44 44
GAMELISTPL	HEX	24 1A 1A 1A	;D4 CA CA
GAMEKEYS	DB	4,16,17,17	;total No. of tunes-1
GAMETUNES	DB	5,17,18,18	;total No. of tunes
GAMETOTALS	DB	5,22,40,40	;running totals (used for CONTROLRAN checks)

RASTDEL	LDA FC	;TAKE THIS OUT on 2nd version
	BEQ !R	;
	LDX #7	;
!D	JSR DELAY	;
	DEX		;
	BNE !D	;
!R	RTS		;


;		*                   *                   *
GL1	DB	"A: THEME",1,32
	DB	"B: GET READY",1,28
	DB	"C: SHOP",1,33
	DB	"D: LEVEL COMPLETE",1,23
	DB	"E: GAME COMPLETE",0

GL2	DB	"A: THEME",1,11,"J: NERVE CENTRE 1",1,4
	DB	"B: GET READY",1,7,"K: NERVE CENTRE 2",1,4
	DB	"C: SHOP",1,12,"L: NERVE CENTRE 3",1,4
	DB	"D: LEVEL COMPLETE  M: NERVE CENTRE 4",1,4
	DB	"E: GAME OVER",1,7,"N: NERVE CENTRE 5",1,4
	DB	"F: HIGH SCORE",1,6,"O: NERVE CENTRE 6",1,4
	DB	"G: DISK INTRO",1,6,"P: NERVE CENTRE 7",1,4
	DB	"H: LOADING THEME",1,3,"Q: GAME COMPLETE",1,5
	DB	"I: TAPE LEVEL LOAD",0

GL3	DB	"A: THEME (WAIT...) J: LEVEL 1",1,11
	DB	"B: GET READY",1,7,"K: LEVEL 2",1,11
	DB	"C: SHOP",1,12,"L: LEVEL 3",1,11
	DB	"D: LEVEL COMPLETE  M: END OF LEVEL 1",1,4
	DB	"E: TIME LOW",1,8,"N: END OF LEVEL 2",1,4
	DB	"F: CLYDES DEATH",1,4,"O: END OF LEVEL 3",1,4
	DB	"G: HIGH SCORE",1,6,"P: TORTURE SCREEN 1  "
	DB	"H: DISK INTRO",1,6,"Q: TORTURE SCREEN 2  "
	DB	"I: GAME COMPLETE",1,3,"R: TORTURE SCREEN 3",0

GL4	DB	" MAYHEM IN MONSTERLAND IS CURRENTLY",1,5
	DB	1,11,"AVAILABLE FROM:",1,14
	DB	1,40
	DB	1,4,"APEX COMPUTER PRODUCTIONS LTD.",1,6
	DB	1,13,"PO BOX 100",1,17
	DB	1,12,"SOUTH OCKENDON",1,15
	DB	1,9,"ESSEX.    RM15 5HR",1,13
	DB	1,40
	DB	1,4,"CASS: `8.99",1,6,"DISK: `9.99",0




MTV	HEX 41008A000001010F250102010A11038195091902020A0A0A0D2C0C010F0122A9B50BB50BB518B5280B00029C0A0D03810DE80C030F01
	HEX 22DD090ED99019010000190000011E000102010A11038115D20E5004080B0116140D2C0C01228CA10919020F010341020A0A0AA90BA9
	HEX 0BA918A9280B00029C0A0D03810DC80C000F01228C090E959619010000190000011E000102010A1A03810B0516520DA00C0F22CAADF1
	HEX 190102CA0A0AC5180F01020AB20BB20BB218B2280F00029C0A0D03810D2C0C010F0122B9090EBB90190100011E


ENDCODEA	;upto $4300



	ORG	$4800 ;if adjusted, adjust COLTAB

;		*	   *										*
C1	HEX	060B040E030D01010101070F0A080209 00
C2	HEX	060B040E0E030303
C3	HEX	00060B04

THCOLS	HEX	0606040E05030D010107070F0A080209
	HEX	0606040E05030D010107070F0A080209

;	HEX	0402080A0A0707010D03050E0E040B06
;	HEX	0402080A0A0707010D03050E0E040B06


	HEX	0606040E05030D010107070F0A080209
	HEX	0B0B0B0C0C0F0F070101090B080A0F07 00

	HEX	0E0E0E0E0E0E0E02020202020208080A
	HEX	08080A080A0A080A0A0A0F0A0F0F070F
	HEX	07070F07070707070701070707010701
	HEX	010701

	HEX	00000009090202080A0F070707010101
;$0F73
	HEX	00060E030D010101
	HEX	01010D030E0600000000060E030D0101
	HEX	0101010D030E0600

	HEX	00000B0C0F0701010101070F0C0B0000
;	HEX	0000060405030D0101070F0A08090000
;	HEX	000B0C0505030503030D030D0D010101
;	HEX	0101010D0D030D0303050305050C0B00
;$0FA4



ENDCODE2








****************************************
*	   ANDY's intro		*
****************************************


	ORG	$71C0

aipos	=	$45e0
acpos	=	$d9b8

PLOTBYTE	DB	4
OPENYET	DB	0

INTRO	jsr mcoloff

	ldx #0
	lda #$EE
	jsr cls

	lda #$0
	sta smc2
	lda #$7
	sta smc1


letprin	ldx #39
!lp	lda aiwin,x
	sta aipos,x
	lda aiwin+40,x
	sta aipos+40,x
	dex
	bpl !lp

	lda #$e7
	sta aipos-40
	lda #$eb
	sta aipos-20
	lda #$e8
	sta aipos+80
	lda #$ec
	sta aipos+100

!co	ldy plotbyte	;mc mayhem
	jsr tplot
	dec plotbyte
	bpl !co

	LDA #$7F
	STA JOY2

	lda $d011
;	and #191
	ORA #$10
	sta $d011

	LDA #>MTV
	LDX #<MTV
	JSR MUSICSET

	jsr ea7eset
	ldx #>aitoprast
	ldy #<aitoprast
	lda #$36
	clc
	jsr rcont2

;CODELOOP 2

canloop	lda codeflag
	beq canloop
	dec codeflag

	lda fc
	eor #1
	sta fc

	JSR MUSICPLY

;	LDA $DC01
;	AND #$10
;	BNE !ND
;	JMP $1600
;!ND

;	lda joy1
;	and #$10
;	bne ne
;	ldx #$ff
;	sei
;	txs
;	cld
;	stx $d016
;	lda #$37
;	sta $01
;	jsr $fda3
;	jsr $fd15
;	jsr $ff5b
;	cli
;	jmp $1000

ne	lda #1
	beq exit

	LDA OPENYET
	BEQ EXIT
	lda joy2
	and #$10
	bne exit

	lda #0
	sta plotcheck
	sta ne+1

	lda #45	;60
	sta dindel
	lda #255
	sta din

exit	lda #1
	bne canloop
	JMP THEBIZ

!nofire	jmp canloop

;--------------------------
;RASTER LIST

aitoprast	lda #0
	sta scr
	sta bdr

;	dec bdr

	lda rsplit
	bmi mcras
	beq !over

	ldy #0
	jsr SPRSPLIT2

;	inc bdr

	lda #10*8+51
	ldx #>airast2
	ldy #<airast2
	rts

;-------

!over	ldy #2
	jsr SPRSPLIT2

;	inc bdr

	lda #14*8+54
	ldx #>airast3
	ldy #<airast3
	rts

;-------

mcras	lda #1
	beq !noplot
	jmp mcsetup

!noplot	ldy #3
	jsr SPRSPLIT2
	lda smcmod
	sta smc1

	lda #1
	sta CODEFLAG

	jsr ufade		;check installed
	jsr colplot2	;check installed
	jsr droll		;* - always called
	jsr droll2		;*
	jsr mingle		;*
	jsr colplot	;check installed
	jsr anin		;check installed

plotout	;inc bdr

	lda #33
	ldx #>aitoprast
	ldy #<aitoprast
	rts

;--------------------------

airast2
;	dec bdr

	ldy #1
	jsr SPRSPLIT2

;	inc bdr

	lda #14*8+54
	ldx #>airast3
	ldy #<airast3
	rts

;--------------------------

airast3	;dec bdr	;call at #14*8+54
	;DEC SCR

	LDA #0
	STA BIGY
	LDX #7
	LDA #$4D
!OFF	STA $47F8,X
	DEX
	BPL !OFF

	lda rsplit
	beq !z

	jsr anex
	jsr flash
	jsr spfade
	jsr scfade
	jmp !jumphere

!z	jsr mtvon

!jumphere	;;jsr musicplay

	lda #1
	sta CODEFLAG

	;inc bdr

	lda #0*8+49
	ldx #>aitoprast
	ldy #<aitoprast
	rts

;--------------------------

ango	db	1	;1=do anex
flashgo	db	0	;1=do flash

rsplit	db	0	;rasterplit check
anibdel	db	30,6,6,12	;ACP appear delays

flashdel	db	40	;delay before flash
mtvdel	db	100	;delay before MTV fadeup

sponb	db	14
spon	hex	ffffff7f7f3f3f1f1f0f0f07070303
sponmsb	hex	01ff02

flash	lda flashgo
	beq !arty

	lda flashdel
	beq !dg
	dec flashdel
	rts

!dg	ldx #2
!l	clc
	lda s2x,x
	adc #24
	sta s2x,x
	bcc !ok
	lda split2+2
	eor sponmsb,x
	sta split2+2
!ok	dex
	dex
	bpl !l
	ldy sponb
	dec sponb
	bne !on
	lda #0
	sta flashgo
	jsr colprin
	lda #$ff
	sta split2+1
	lda #10		;sfval
	sta sfok
	rts

!on	lda spon,y
	sta split2+5
!arty	rts


colprin	lda #1
COLPRINJ	ldx #160
!lp	sta acpos-1,x
	dex
	bne !lp
	rts


SPRSPLIT2	lda splitablo,y
	sta ta
	lda splitabhi,y
	sta ta+1
	ldy #37		;LENGTH of table
	ldx #7
!sz	lda (ta),y
	sta $47f8,x
	dey
	dex
	bpl !sz
	ldx #15
!sa	lda (ta),y
	sta $d000,x
	dey
	dex
	bpl !sa
	ldx #7
!sb	lda (ta),y
	sta $d027,x
	dey
	dex
	bpl !sb
	lda (ta),y
	sta $d015
	dey
	lda (ta),y
	sta bigy
	dey
	lda (ta),y
	sta bigx
	dey
	lda (ta),y
	sta $d010
	dey
	lda (ta),y
	sta $d01b
	dey
	lda (ta),y
	sta $d01c
hart	rts


anex	lda fc
	beq hart
	lda ango
	beq hart		;above

	lda anibdel
	beq !d
	dec anibdel
	rts
!d	ldx anib
	lda anima,x
	beq !n1
	sta s1p
	lda animc,x
	sta s1c
	dec anib

!n1	lda anibdel+1
	beq !d1
	dec anibdel+1
	rts
!d1	ldx anib+1
	lda animp,x
	beq !n2
	sta s1p+1
	lda animc,x
	sta s1c+1
	dec anib+1

!n2	lda anibdel+2
	beq !d2
	dec anibdel+2
	rts
!d2	ldx anib+2
	lda anime,x
	beq !n3
	sta s1p+2
	lda animc,x
	sta s1c+2
	dec anib+2

!n3	lda anibdel+3
	beq !d3
	dec anibdel+3
	rts
!d3	ldx anib+3
	lda animx,x
	beq !rtex
	sta s1p+3
	lda animc,x
	sta s1c+3
	dec anib+3
!rt	rts

!rtex	lda #0
	sta ango
	lda #1
	sta flashgo
	rts


spfade	lda fc
	bne !out
	ldx sfok
	beq !out
	lda sftab,x
	ldy #7
!pl	sta s2c,y
	dey
	bpl !pl
	dec sfok
	beq !set
!out	rts

!set	lda #8
	sta scfb
	rts


scfade	lda fc
	beq !rts
	ldx scfb
	beq !rts

	lda fodel
	beq !co
	dec fodel
!rts	rts

!co	lda scfcols,x
	jsr COLPRINJ
	ldx #3
!ss	sta s1c,x
	dex
	bpl !ss
	dec scfb
	beq !outt
!ts	rts

!outt	lda #$ff
	sta rsplit
	rts


mtvon	lda fc
	bne !rs
	lda mtvdel
	beq !con
	dec mtvdel
!rs	rts

!con	ldx mtvb
	lda mtvcol,x
	bmi !setdel
	ldy #5
!mpl	sta s3c,y
	dey
	bpl !mpl
	dec mtvb
	beq !srs
	rts

!setdel	dec mtvb
	lda #100
	sta mtvdel
	rts

!srs	lda #1
	sta rsplit
	sta ango
	rts


splitablo	dl	split1,split2,split3,split4
splitabhi	dh	split1,split2,split3,split4

;		m p m b b s
;		c r s i i p
;		h i b g g r
;		r o   x y o

split1	hex	00000000000f
s1c	hex	0101010100000000
	hex	7c6c9c6cbc6cdc6c0000000000000000
s1p	hex	4d4d4d4d00000000

split2	hex	000082fc0003
s2c	hex	0101010101010101
s2x	hex	0091e891189148917891a891d8910891
s2p	hex	4f4e4e4e4e4e4e4e

split3	hex	00000000003f
s3c	hex	0000000000000000
	hex	9a6cb26cca6c9a81b281ca8100000000
	hex	bcbdbebf50510000
		; 3c
split4	hex	0200003c3c3f
s4c	hex	0000000000000000
s4x	hex	ab86ab8688728887b872b88700000000
s4p	hex	88894e4e4e4e0000

smcmod	db	0

anib	db	14,14,14,14

anima	hex	009a9b9c9d9e9fa0a1a1bababbbbbb
animp	hex	00a2a3a4a5a6a7a8a9a9bababbbbbb
anime	hex	00aaabacadaeafb0b1b1bababbbbbb
animx	hex	00b2b3b4b5b6b7b8b9b9bababbbbbb
animc	hex	000101010f0f0f0c0c0c0c0b0b0b0b

;sfval	db	10
sfok	db	0
sftab	hex	00000b0c0f0f0101010101

mtvb	db	16
mtvcol	hex	00000b0b0c0c0f0fff010f0f0c0c0b0b00

fodel	db	150	;before ACP fades out
scfb	db	0
scfcols	hex	000006060e0e030301

cirbank	hex	0000000001030F1C00031F7CE0800000
	hex	3FFFC00000000000F8FF070000000000
	hex	0080F07C0F030100000000000080E070
	hex	000000010303060E3870C0C080000000
	hex	381C060703010000000000008080C0E0
	hex	0C181830303060606030301818180C0C
	hex	6060C0C0C0C0C0C00C0C060606060606
	hex	C0C0C0C0C060606006060606060C0C0C
	hex	6030303018180C0E0C181818303060E0
	hex	0603030100000000000080C0C070381C
	hex	00010307061C3870C080800000000000
	hex	0F030100000000000080E07C1F030000
	hex	00000000C0FF3F000000000007FFF800
	hex	01030F7CF0800000E080000000000000

aiwin	hex	E5EEC8EECACBEECEEED0EED2EED4EECEEEE1E2EE
	hex	E9EECEEEC8EED7EED0EED9EED2DBDCC8EEDDEEDF
	hex	E6EEC9EECCCDEECFEED1EED3EED5EED6EEE3E4EE
	hex	EAEED6EEC9EED8EED1EEDAEED3DBDCC9EEDEEEE0

;---------------------------------------------

cirdest	=	$5640
cpos	=	$4550

mcsetup	ldx #225
!bnk	lda cirbank-1,x
	sta cirdest-1,x
	dex
	bne !bnk

printc	ldx #7
!rp	lda pw0,x
	sta cpos,x
	lda pw1,x
	sta cpos+40,x
	lda pw2,x
	sta cpos+80,x
	lda pw3,x
	sta cpos+120,x
	lda pw4,x
	sta cpos+160,x
	lda pw5,x
	sta cpos+200,x
	lda pw6,x
	sta cpos+240,x
	lda pw7,x
	sta cpos+280,x
	dex
	bpl !rp

	lda #0
	sta mcras+1
	jmp plotout

droll	lda fc
	beq !nc

	ldy colours
	ldx #0
!l	lda colours+1,x
	sta colours,x
	inx
	cpx #27
	bcc !l
	sty colours+27
	rts

!nc	ldy colours3
	ldx #0
!m	lda colours3+1,x
	sta colours3,x
	inx
	cpx #9
	bcc !m
	sty colours3+9
!xit	rts

droll2	lda fc
	beq !nc

	ldx #0
!l	lda cr1+1,x
	sta cr1,x
	inx
	cpx #23
	bcc !l

	ldx #0
!ll	lda cr2+1,x
	sta cr2,x
	inx
	cpx #22
	bcc !ll
	rts

!nc	ldx #27
!zl	lda cr3,x
	sta cr3+1,x
	dex
	bpl !zl

	ldx #26
!lz	lda cr4,x
	sta cr4+1,x
	dex
	bpl !lz
!rt	rts


colplot	ldx #30
	lda $daf1
!cp	sta $db25,x
	dex
	bpl !cp
	rts

colb	db	13
cr1	=	$d8d1
cr2	=	$d8d1+40
cr3	=	$daad
cr4	=	$daae+40

colplot2	lda fc
	bne !no
	lda plotyorn
	beq !no

	ldx colb
	beq !check
!nocheck	lda colours2,x
	sta cr1+23
	sta cr2+22
	sta cr3
	sta cr4
	dex
	bpl !k
	ldx #13
!k	stx colb
!no	rts

!check	ldy plotcheck
	bne !nocheck
	lda #0
	sta cr1+23
	sta cr2+22
	sta cr3
	sta cr4
	sta plotyorn
	rts

plotcheck	db	1
plotyorn	db	0


colours	hex	09020a0701010d030e0600000000
	hex	09020a0701010d030e0600000000
colours2	hex	0006040e0503070d0703050e0406
colours3	hex	010f0f0c0c0b0000000000

colcount	db	27
colcount2	db	9

ccol	=	$d950
ccol2	=	$da68
ccol3	=	$d9a2

coff	db	41,40,80,120,160,200,240,241
	db	1,2,3,4,5,6
	db	246,247,207,167,127,87,47,46,6,5,4,3,2,1
coff2	db	1,2,43,83,123,122,121,120,80,40

mingle	ldx colcount
!lp1	lda coff,x
	tay
	lda colours,x
	sta ccol,y
	dex
	cpx #13
	bne !lp1

!lp2	lda coff,x
	tay
	lda colours,x
	sta ccol2,y
	dex
	cpx #7
	bne !lp2

!lp3	lda coff,x
	tay
	lda colours,x
	sta ccol,y
	dex
	bpl !lp3

	ldx colcount2
!lp4	lda coff2,x
	tay
	lda colours3,x
	sta ccol3,y
	dex
	bpl !lp4

	lda ccol3+2
	sta ccol3+3
	lda ccol3+40
	sta ccol3+41
	lda ccol3+80
	sta ccol3+81
	lda ccol3+43
	sta ccol3+42
	rts


ubyte	db	15
ubyte2	db	0
udel	db	50	;initial delay

ucols	hex	0a0a0a0a0f070101010f0f0c0c0b00
ucols2	hex	0009080a0f070101010f0f0c0c0b00
ucols3	hex	0707070707070101010f0f0c0c0b00

ufade	lda fc
	beq !rts
	lda udel
	beq !cont
	dec udel
	rts

!cont	ldx ubyte
	beq !rts
	bmi !close

	lda ucols-1,x
	sta s4c+1
	lda ucols2-1,x
	sta s4c
	lda ucols3-1,x
	sta smcmod
	dec ubyte
	beq !set
!rts	rts

!set	lda #1
	sta din
	rts

!close	ldx ubyte2
	lda ucols,x
	sta s4c+1
	lda ucols2,x
	sta s4c
	lda ucols3,x
	sta smcmod
	inx
	cpx #15
	beq !x
	stx ubyte2
	rts

!x	lda #0
	sta ubyte
	sta exit+1
;;	lda #2
;;	sta aitoprast+1
	rts

din	db	0
dindel	db	100

anin	lda fc
	bne !sddl
	lda dindel
	beq !nodel
	dec dindel
!sddl	rts

!nodel	lda din
	beq !sddl
	bmi !min

!mout	lda s4x+4
	cmp #$68
	bne !itsok
	lda #0
	sta din
	lda #1
	sta plotyorn
	STA OPENYET
;;	lda #255		;
;;	sta dinmod+1	;
	rts

!itsok	dec s4x+4
	dec s4x+6
	inc s4x+8
	inc s4x+10
	rts

!min	lda s4x+4
	cmp #$88
	bne !ok2
	lda #0
	sta din

	lda #$ff
	sta ubyte
	lda #10
	sta udel

;;	lda #1		;
;;	sta dinmod+1	;
	rts

!ok2	inc s4x+4
	inc s4x+6
	dec s4x+8
	dec s4x+10
	rts


tplot	lda btlo,y
	sta ta
	lda bthi,y
	sta ta+1

	lda loclo,y
	sta ta+2
	lda lochi,y
	sta ta+3

	lda btlen,y
	tay
!pl	lda (ta),y
	sta (ta+2),y
	dey
	bpl !pl
	rts


loclo	dl	tloc1,tloc1a,tloc2,tloc2a,tloc3
lochi	dh	tloc1,tloc1a,tloc2,tloc2a,tloc3

tloc1	=	$44cf
tloc1a	=	$44cf+40
tloc2	=	$46af
tloc2a	=	$46af+40
tloc3	=	$4725

btlo	dl	bt1,bt1a,bt2,bt2a,bt3
bthi	dh	bt1,bt1a,bt2,bt2a,bt3

btlen	db	26,26,26,26,30

bt1	hex	fefe3031321011FE3031320809616222231019303132044748fefe
bt1a	hex	fefe3334351213FE3334350A0B63642425121B333435FE494Afefe

bt2	hex	30313210191E1F0809FE3031324F504748261011FE303132265D5E
bt2a	hex	333435121B20210A0BFE3334355152494A271213FE333435275F60

bt3	hex	959490A3A4A1989D96EEA39794EE9C
	hex	9E9DA2A394A19B909D93EE9F9EA2A294

pw0	hex	EEC8C9CACBCCCDEE
pw1	hex	CECFEEEEEEEED0D1
pw2	hex	D2EEEEB8B9C5EED3
pw3	hex	D4EEBABBBCBDEED5
pw4	hex	D6EEBEBFEEC0EED7
pw5	hex	D8EEC1C2C3C4EED9
pw6	hex	DADBEEEEEEEEDCDD
pw7	hex	EEDEDFE0E1E2E3EE

;-----------------------------------------------------------------------

;cls	stx !fc+1
;	sta !cl+1
;	ldx #251
;!cl	lda #$ff
;	sta $d800-1,x
;	sta $d800+249,x
;	sta $d800+499,x
;	sta $d800+749,x
;!fc	lda #$ff
;	sta $4400-1,x
;	sta $4400+249,x
;	sta $4400+499,x
;	sta $4400+749,x
;	dex
;	bne !cl
;	rts


mcolon	lda $d016
	ora #$10
	sta $d016
	rts

mcoloff	lda $d016
	and #$ef
	sta $d016
	rts



ENDINTRO













;
;  PDS Pc1.21 :000: (c) P.D.Systems Ltd 1985-88
;
;
;       File  "IN-GAME ALIEN CODE!"
;    Version          11.19
;         By  "IT'S STILL ME-JAZ.R"
;
;     Created on Sat the 14th of May 1988
;        Last update 00:24 on 01/01/80
;
;**************************************************************************
;*  APEX MUSIC PLAYER : COPYRIGHT JOHN ROWLANDS 1988                      *
;**************************************************************************

	ORG	$1A00,$0400
MSTART

CFFF	HEX 00


C823	INC CD74	;music play
C826	LDA CD74
C829	CMP CD73
C82C	BCC C836	;C833
C82E	LDA #0
C830	STA CD74
C836	CLC
C833	LDA CF40
C837	ADC CD90
C83A	STA CF40
C83D	CMP #8
C83F	BCC C847
C841	AND #7
C843	STA CF40
C846	SEC
C847	LDA CF41
C84A	ADC CD91
C84D	STA CF41

C850	LDX #2	;x:number of voices to be played
C852	LDY CE17,X
C855	STY $FE
C85A	CLC
C857	LDA CDBA,X
C85B	ADC #1
C85D	CMP CD8D,X
C860	BCC C876
C862	INC CDBD,X
C868	CLC
C865	LDA CDAB,X
C869	ADC #1
C86B	CMP #3
C86D	BCC C871
C86F	LDA #0
C871	STA CDAB,X
C874	LDA #0
C876	STA CDBA,X
C879	LDA CE14,X
C87C	STA $FC
C87E	LDA CE11,X
C881	STA $FD
C883	LDA CD74
C886	BNE C8C4
C888	INC CD9F,X
C88B	LDA CDC3,X
C88E	CMP #$FF
C890	BEQ C89D
C892	CMP #0
C894	BNE C8C4
C896	LDA CE0E,X
C899	CMP #1
C89B	BNE C8C4

C89D	LDY #0
C8A1	SEC
C89F	LDA ($FC),Y
C8A2	SBC #1
C8A4	CMP #$25
C8A6	BCS C8C4
;C8A8	ASL
C8A9	TAY
C8AA	LDA CEE1,Y
C8AD	STA C8BC+1
C8B0	LDA CEE2,Y
C8B3	STA C8BC+2
C8B6	LDY #1
C8B8	LDA ($FC),Y
C8BA	LDY $FE
C8BC	JSR $FFFF
C8BF	JSR CD60	;CD5E
C8C2	BNE C89D

C8C4	LDY $FE
C8C6	LDA CD7B,X	;rest/duration of note?
C8C9	BEQ C930
C8CB	LDA CD93,X
C8CE	BEQ C8DE
C8D0	LDA CD9F,X
C8D3	CMP CD9C,X
C8D6	BCC C8DE

C8D8	LDA CD93,X
C8DB	JSR CCB4
C8DE	LDA CDA2,X
C8E1	BEQ C8E6
C8E3	JSR CC4E


C8E6	LDA CD87,X	;WAVEFLIP?
C8E9	BEQ C920	;NO WAVEFLIP
C8EB	CMP #6	;1ST TIMER?
C8ED	BCC C8FE	;YES

C8EF	LDA CDBD,X
C8F2	AND #1
C8F4	BNE C918

C8F6	LDA CD87,X
C8F9	SEC
C8FA	SBC #5
C8FC	JMP C912	;BPL

C8FE	LDA CD9F,X
C901	CMP #1
C903	BCC C918

C905	LDA CD8A,X
C908	BEQ C90F
C90A	CMP CD9F,X
C90D	BCC C918

C90F	LDA CD87,X
C912	TAY
C913	LDA CE1B,Y	;DELAY ?
C916	BNE C91B

C918	LDA CD7E,X
C91B	LDY $FE
C91D	STA CF2F,Y	;EO WAVEFLIP


C920	LDA CD84,X
C923	BEQ C928
C925	JSR CC1F

C928	LDA CDEA,X
C92B	BEQ C930
C92D	JSR CBCD

C930	LDA CD74
C933	BNE C938
C935	JSR C94A

C938	LDA $FC
C93A	STA CE14,X
C93D	LDA $FD
C93F	STA CE11,X
C942	DEX
;C943	CPX #$FF
C945	BMI C96F
C947	JMP C852
C94A	LDA CE0E,X
C94D	SEC
C94E	SBC #1
C950	STA CE0E,X
C953	BCS C958
C955	DEC CDC3,X
C958	LDA CDC3,X
C95B	BNE C96F
C95D	LDA CE0E,X
C960	BEQ C970
C962	CMP CD81,X
C965	BCS C96F
C967	LDA CF2F,Y
C96A	AND #$FE
C96C	STA CF2F,Y
C96F	RTS

C970	STA CF2D,Y
C973	STA CD9F,X
C976	STA CDC6,X
C979	STA CDCC,X
C97C	STA CDC9,X
C97F	STA CDBA,X
C982	STA CDBD,X
C985	STA CDAB,X
C988	CPX CD72
C98B	BNE C996
C98D	STA CF40
C990	LDA CD92
C993	STA CF41
C996	LDA CDE7,X
C999	STA CF30,Y
C99C	LDA CDDE,X
C99F	STA CF31,Y
C9A2	LDA CDE4,X
C9A5	STA CF2E,Y
C9A8	LDA CDA8,X
C9AB	STA CDA5,X
C9AE	LDY #0
C9B0	LDA ($FC),Y
C9B2	STA CD7B,X
C9B5	BEQ C9DB
C9B7	STA $FF
C9B9	LDA CDB1,X
C9BC	BEQ C9CB
C9BE	CLC
C9BF	ADC $FF
C9C1	JSR CD46
C9C4	;STA CDB4,X		;not needed
C9C7	TYA
C9C8	STA CDB7,X
C9CB	LDA CDAE,X
C9CE	BEQ C9D6
C9D0	CLC
C9D1	ADC $FF
C9D3	JSR CB75
C9D6	LDA $FF
C9D8	JSR CD46
C9DB	PHA
C9DC	TYA
C9DD	LDY $FE
C9DF	STA CF2C,Y
C9E2	STA CD78,X
C9E5	PLA
C9E6	STA CF2B,Y
C9E9	STA CD75,X
C9EC	LDY #1
C9EE	LDA ($FC),Y
C9F0	LDY $FE
C9F2	STA CE0E,X
C9F5	JSR CD60

C9F8	LDA CDE1,X	;SETUP ORIGINAL WAVEFORM
C9FB	STA CF2F,Y
C9FE	STA CD7E,X

CA01	LDY #0
CA03	LDA ($FC),Y
CA05	CMP #$19
CA07	BNE CA11+1
CA09	INY
CA0A	LDA ($FC),Y
CA0C	PHA
CA0D	JSR CD60
CA10	PLA
CA11	BIT $00A9	;CA11 : BIT $00A9
MOD	DB	0
CA14	STA CDC3,X
CA17	RTS

CA18	LDA #0	;music set up
CA1A	LDY #$9B
CA1C	STA CD72-1,Y
CA1F	DEY
CA20	BNE CA1C
CA22	LDY #2
CA2F	LDA #2
CA31	STA CD81,Y
CA34	LDA #0	;initial delay
CA36	STA CDC3,Y
CA24	LDA #1
CA26	STA CE0E,Y
CA29	STA CE0B,Y
CA39	DEY
CA3A	BPL CA2F	;CA24
CA2C	STA CD73
CA3C	LDA $FC
CA3E	STA CE1A
CA41	CLC
CA42	ADC #4
CA44	STA CE14
CA47	LDA $FD
CA49	STA CE1B
CA4C	ADC #0
CA4E	STA CE11
CA51	LDX #$7E
CA53	LDY #0
CA55	LDA ($FC),Y
CA57	CLC
CA58	ADC CE14
CA5B	STA CD97,X
CA5E	PHP
CA5F	INY
CA60	PLP
CA61	LDA ($FC),Y
CA63	ADC CE11
CA66	STA CD94,X
CA69	INY
CA6A	INX
CA6B	BPL CA55
CA6D	JSR CA81
CA70	JSR C814
CA73	JSR C814
;CA76	LDA #1
CA78	INC CFFF
CA7B	RTS
CA7C	LDA #0
CA7E	STA CFFF
CA81	LDX #$17
;CA83	LDA #9	;?
;CA85	STA CF2B,X
CA88	LDA #0
CA8A	STA CF2B,X
CA90	DEX
CA91	BPL CA88
CA8D	STA $D418
CA93	RTS

CAAC	STA $FF
CAAE	TXA
CAAF	TAY
CAB0	LDA CDED,Y
CAB3	CLC
CAB4	ADC #1
CAB6	STA CDED,Y
CAB9	CMP $FF
CABB	BCS CAC7
CABD	LDA CDF3,Y
CAC0	STA $FC
CAC2	LDA CDF9,Y
CAC5	STA $FD
CAC7	RTS

CAC8	TXA
CAC9	TAY
CACA	LDA $FC
CACC	STA CDF3,Y
CACF	LDA $FD
CAD1	STA CDF9,Y
CAD4	LDA #0
CAD6	STA CDED,Y
CAD9	RTS

CADA	STA $FF
CADC	TXA
CADD	CLC
CADE	ADC #3
CAE0	BNE CAAF
CAE2	TXA
CAE3	CLC
CAE4	ADC #3
CAE6	BNE CAC9
CAE8	STA CDD5,X
CAEB	RTS

CAFC	CMP #$64
CAFE	BCC CB06
CB00	SBC #$64
CB02	STA CD81,X
CB05	RTS
CB06	STA CD8A,X
CB09	RTS

CB0E	CMP #$63
CB10	BCC CB18
CB12	SBC #$64
CB14	STA CD72
CB17	RTS
CB18	STA CD87,X
CB1B	RTS

CB2C	PHA
CB2D	AND #$0F
CB2F	STA CDD2,X
CB32	PLA
CB33	LSR
CB34	LSR
CB35	LSR
CB36	LSR
CB37	STA CDCF,X
CB3A	RTS
CB3B	STA CE0B,X
CB3E	RTS
CB3F	STA CF42
CB42	RTS
CB43	STA CD92
CB46	RTS

CB4F	CLC		;volume
CB50	ADC CF43
CB53	STA CF43
CB56	RTS


CBCD	LDA CDBA,X
CBD0	BNE CC0B	;rts
CBD2	LDA CDCF,X
CBD5	BEQ CC0C
CBD7	LDA CDCC,X
CBDA	BEQ CBF9
CBDC	LDA CF2D,Y
CBDF	SEC
CBE0	SBC CDEA,X
CBE3	STA CF2D,Y
CBE6	BCS CC1E	;rts
CBE8	LDA CF2E,Y
CBEB	SBC #0
CBED	STA CF2E,Y
CBF0	CMP CDD2,X
CBF3	BCS CC1E	;rts
CBF5	LDA #0
CBF7	BEQ CC08
CBF9	LDA CF2E,Y
CBFC	CMP CDCF,X
CBFF	BCC CC0C
CC01	CMP CDD2,X
CC04	BCC CC1E
CC06	LDA #1
CC08	STA CDCC,X
CC0B	RTS
CC0C	LDA CF2D,Y
CC0F	CLC
CC10	ADC CDEA,X
CC13	STA CF2D,Y
CC16	LDA CF2E,Y
CC19	ADC #0
CC1B	STA CF2E,Y
CC1E	RTS

CAA0	STA CDE1,X
CAA3	RTS

CC1F	CMP #1
CC21	BNE CC35
CC23	LDA CDBD,X
CC26	LSR
CC27	BCS CC4B
CC29	LDA CD96,X
CC2C	STA CF2C,Y
CC2F	LDA CD99,X
CC32	JMP CC47
CC35	LDA CDAB,X
CC38	BEQ CC4B
CC3A	CMP #1
CC3C	BEQ CC29
CC3E	LDA CDB7,X
CC41	STA CF2C,Y
CC44	LDA CDB4,X
CC47	STA CF2B,Y
CC4A	RTS
CC4B	JMP CD39
CC4E	LDA CDA5,X
CC51	BEQ CC57
CC53	DEC CDA5,X
CC56	RTS
CC57	LDA CDC9,X
CC5A	CMP #1
CC5C	BEQ CC68
CC5E	CMP #2
CC60	BEQ CC68
CC62	JSR CC86
CC65	JMP CC6B
CC68	JSR CC9B
CC6B	LDA CDC6,X
CC6E	CLC
CC6F	ADC #1
CC71	CMP CE0B,X
CC74	BCC CC82
CC76	LDA CDC9,X
CC79	ADC #0
CC7B	AND #3
CC7D	STA CDC9,X
CC80	LDA #0
CC82	STA CDC6,X
CC85	RTS

CB57	STA CDB1,X
CB5A	RTS

CB5B	PHA
CB5C	LSR
CB5D	LSR
CB5E	LSR
CB5F	STA CD91
CB62	PLA
CB63	AND #7
CB65	STA CD90
CB68	RTS

CBC9	STA CD73
CBCC	RTS

CB69	STA CDAE,X
CB6C	RTS

CB47	CLC
CB48	ADC CDD5,X
CB4B	STA CDD5,X
CB4E	RTS

CB71	STA CDA8,X
CB74	RTS

CBB0	LDA CDFF,X
CBB3	STA $FC
CBB5	LDA CE05,X
CBB8	STA $FD
CBBA	LDA CE02,X
CBBD	STA CDFF,X
CBC0	LDA CE08,X
CBC3	STA CE05,X
CBC6	JMP CD60

CB6D	STA CDC0,X
CB70	RTS

CC86	LDA CD75,X
CC89	CLC
CC8A	ADC CDA2,X
CC8D	STA CD75,X
CC90	STA CF2B,Y
CC93	LDA CD78,X
CC96	ADC #0
CC98	JMP CCAD
CC9B	LDA CD75,X
CC9E	SEC
CC9F	SBC CDA2,X
CCA2	STA CD75,X
CCA5	STA CF2B,Y
CCA8	LDA CD78,X
CCAB	SBC #0
CCAD	STA CD78,X
CCB0	STA CF2C,Y
CCB3	RTS


CD46	AND #$7F	;get note to be played
CD48	CLC
CD49	ADC CDD5,X
CD4C	ASL
CD4D	TAY
CD4E	LDA CE21,Y
CD51	CLC
CD52	ADC CDC0,X
CD55	PHA
CD56	LDA CE22,Y
CD59	ADC #0	;frequency offset
CD5B	TAY
CD5C	PLA
CD5D	RTS

CAF0	STA CDDB,X
CAF3	RTS

CCB4	CMP #1	;vibrato/note flip
CCB6	BEQ CCB8
CD30	CMP #3	;slide down
CD32	BEQ CD1A
CD34	CMP #2	;slide up
CD36	BEQ CD07
CD38	RTS

CCB8	LDA CD96,X	;note limit - vibrate/note flip
CCBB	CMP CD78,X
CCBE	BCC CCF0
CCC0	BNE CCCC
CCC2	LDA CD99,X
CCC5	CMP CD75,X
CCC8	BEQ CD38
CCCA	BCC CCF0
CCCC	JSR CD07
CCCF	LDA CD78,X
CCD2	CMP CD96,X
CCD5	BCC CCB3
CCD7	BNE CCE1
CCD9	LDA CD75,X
CCDC	CMP CD99,X
CCDF	BCC CCB3
CCE1	LDA CD99,X
CCE4	STA CD75,X
CCE7	LDA CD96,X
CCEA	STA CD78,X
CCED	JMP CD39

CCF0	JSR CD1A
CCF3	LDA CD78,X
CCF6	CMP CD96,X
CCF9	BCC CCE1
CCFB	BNE CCB3
CCFD	LDA CD75,X
CD00	CMP CD99,X
CD03	BCS CCB3
CD05	BCC CCE1

CD07	CLC		;add to note - slide up
CD0A	LDA CD75,X
CD0B	ADC CDD8,X
CD0E	STA CD75,X
CD11	LDA CD78,X
CD14	ADC CDDB,X
CD17	JMP CD2A

CD1A	SEC		;subtract from note - slide down
CD1D	LDA CD75,X
CD1E	SBC CDD8,X
CD21	STA CD75,X
CD24	LDA CD78,X
CD27	SBC CDDB,X
CD2A	STA CD78,X
;CD2D	JMP CD39

CD39	LDA CD78,X	;chuck values into voice frequencies
CD3C	STA CF2C,Y
CD3F	LDA CD75,X
CD42	STA CF2B,Y
CD45	RTS

CAEC	STA CDEA,X
CAEF	RTS

CD60	CLC
CD5E	LDA $FC
CD61	ADC #2
CD63	STA $FC
CD65	BCC CD69
CD67	INC $FD
CD69	RTS

MUSICSET	STA $FC
	STX $FD
	LDA #0
C805	STA CFFF
C80C	JMP CA18	;set up


CB28	STA CD9C,X
CB2B	RTS

CB24	STA CDA2,X
CB27	RTS

CB20	STA CD93,X
CB23	RTS

CB1C	STA CD84,X
CB1F	RTS
;
CB0A	STA CD8D,X
CB0D	RTS
;
CAA4	STA CDE7,X
CAA7	RTS

CA9C	STA CF43	;volume
CA9F	RTS


CB75	JSR CD46
CB78	STA CD99,X
CB7B	TYA
CB7C	STA CD96,X
CB7F	RTS
CB80	PHA
CB81	LDA CDFF,X
CB84	STA CE02,X
CB87	LDA CE05,X
CB8A	STA CE08,X
CB8D	LDA $FC
CB8F	STA CDFF,X
CB92	LDA $FD
CB94	STA CE05,X
CB97	LDY #2
CB99	LDA ($FC),Y
CB9B	STA $FF
CB9D	PLA
CB9E	CLC
CB9F	ADC CE1A
CBA2	STA $FC
CBA4	LDA $FF
CBA6	ADC CE1B
CBA9	STA $FD
CBAB	PLA
CBAC	PLA
CBAD	JMP C89D

CAA8	STA CDDE,X
CAAB	RTS

CA94	JSR CA7C
CA97	PLA
CA98	PLA
CA99	PLA
CA9A	PLA
CA9B	RTS

MUSICPLY	LDA CFFF
C812	BEQ C822
C814	JSR C823	;play
NUMFREE	LDY #$18	;y:number of voices to be played / 0-7,8-15,16-23
C819	LDA CF2B,Y	;	                         24-vol
C81C	STA $D400,Y	;sid vars
C81F	DEY
C820	BPL C819
VOLUME	NOP
	LDA CF2B+24
	STA $D418
C822	RTS

CAF4	STA CDD8,X
CAF7	RTS

CF44	HEX 41
CF45	HEX AD
CF46	HEX 02
CF47	HEX 03
CF48	HEX 11
CF49	HEX 08
CF4A	HEX 01
CF4B	HEX 07
CF4C	HEX 00
CF4D	HEX 0F
CF4E	HEX 00
CF4F	HEX 24
CF50	HEX 0F
CF51	HEX 03
CF52	HEX 07
CF53	HEX 00
CF54	HEX 08
CF55	HEX 00
CF56	HEX 03
CF57	HEX 41
CF58	HEX B2
CF59	HEX 02
CF5A	HEX 03
CF5B	HEX 11
CF5C	HEX 08
CF5D	HEX 01
CF5E	HEX 07
CF5F	HEX 00
CF60	HEX 0F
CF61	HEX 00
CF62	HEX 24
CF63	HEX 00
CF64	HEX A5
CF65	HEX 00
CF66	HEX 78
CF67	HEX 01
CF68	HEX 00
CF69	HEX 01
CF6A	HEX 25
CF6B	HEX 02
CF6C	HEX 01
CF6D	HEX 0F
CF6E	HEX 03
CF6F	HEX 41
CF70	HEX 04
CF71	HEX 0A
CF72	HEX 15
CF73	HEX FA
CF74	HEX 0E
CF75	HEX 64
CF76	HEX 02
CF77	HEX 0C
CF78	HEX 0A
CF79	HEX AD
CF7A	HEX 0D
CF7B	HEX 0A
CF7C	HEX 0C
CF7D	HEX 00
CF7E	HEX 0F
CF7F	HEX 01
CF80	HEX 22
CF81	HEX 9A
CF82	HEX 95
CF83	HEX 80
CF84	HEX 0F
CF85	HEX 00
CF86	HEX 02
CF87	HEX 0A
CF88	HEX 0A
CF89	HEX 08
CF8A	HEX 23
CF8B	HEX 5F
CF8C	HEX 04
CF8D	HEX 00
CF8E	HEX 23
CF8F	HEX 5F
CF90	HEX 04
CF91	HEX 00
CF92	HEX 23
CF93	HEX 5F
CF94	HEX 04
CF95	HEX 00
CF96	HEX 23
CF97	HEX 5F
CF98	HEX 04
CF99	HEX 00
CF9A	HEX 14
CF9B	HEX 02
CF9C	HEX 23
CF9D	HEX 5F
CF9E	HEX 04
CF9F	HEX 00
CFA0	HEX 23
CFA1	HEX 5F
CFA2	HEX 04
CFA3	HEX 00
CFA4	HEX 14
CFA5	HEX 00
CFA6	HEX 1F
CFA7	HEX 0C
CFA8	HEX A6
CFA9	HEX 20
CFAA	HEX 02
CFAB	HEX 09
CFAC	HEX 13
CFAD	HEX 00
CFAE	HEX 23
CFAF	HEX CD
CFB0	HEX 04
CFB1	HEX 00
CFB2	HEX 12
CFB3	HEX 0A
CFB4	HEX 14
CFB5	HEX 0C
CFB6	HEX 02
CFB7	HEX 0C
CFB8	HEX 9A
CFB9	HEX 20
CFBA	HEX 00
CFBB	HEX 60
CFBC	HEX 19
CFBD	HEX 00
CFBE	HEX 02
CFBF	HEX 09
CFC0	HEX 11
CFC1	HEX 00
CFC2	HEX 9A
CFC3	HEX 08
CFC4	HEX 9D
CFC5	HEX 08
CFC6	HEX 9A
CFC7	HEX 08
CFC8	HEX 9D
CFC9	HEX 08
CFCA	HEX 9A
CFCB	HEX 08
CFCC	HEX 9D
CFCD	HEX 08
CFCE	HEX 9A
CFCF	HEX 08
CFD0	HEX 95
CFD1	HEX 08
CFD2	HEX 10
CFD3	HEX 04
CFD4	HEX 23
CFD5	HEX CD
CFD6	HEX 04
CFD7	HEX 00
CFD8	HEX 14
CFD9	HEX 18
CFDA	HEX 23
CFDB	HEX CD
CFDC	HEX 04
CFDD	HEX 00
CFDE	HEX 23
CFDF	HEX CD
CFE0	HEX 04
CFE1	HEX 00
CFE2	HEX 14
CFE3	HEX 0C
CFE4	HEX 23
CFE5	HEX CD
CFE6	HEX 04
CFE7	HEX 00
CFE8	HEX 14
CFE9	HEX 00
CFEA	HEX 02
CFEB	HEX 0A
CFEC	HEX 23
CFED	HEX 5F
CFEE	HEX 04
CFEF	HEX 00
CFF0	HEX 23
CFF1	HEX 5F
CFF2	HEX 04
CFF3	HEX 00
CFF4	HEX 23
CFF5	HEX 5F
CFF6	HEX 04
CFF7	HEX 00
CFF8	HEX 14
CFF9	HEX 02
CFFA	HEX 23
CFFB	HEX 5F
CFFC	HEX 04
CFFD	HEX 00
CFFE	HEX 23


CAF8	STA CDE4,X
CAFB	RTS

;CD6A	DB	0,0,0,0,0,0,0,0	;don't need
;	DB	0,0,0

CD72	HEX 00

CD73	HEX 00	;tempo
CD74	HEX 00	;tempo cnt


CD75	HEX 00
CD76	HEX 00
CD77	HEX 00

CD78	HEX 00
CD79	HEX 00
CD7A	HEX 00

CD7B	HEX 00
CD7C	HEX 00
CD7D	HEX 00

CD7E	HEX 00	;waveform ?
CD7F	HEX 00
CD80	HEX 00

CD81	HEX 00
CD82	HEX 00
CD83	HEX 00

CD84	HEX 00
CD85	HEX 00
CD86	HEX 00
CD87	HEX 00
CD88	HEX 00
CD89	HEX 00

CD8A	HEX 00	;ORIGINAL WAVEFLIP TIME (00-99)
CD8B	HEX 00
CD8C	HEX 00

CD8D	HEX 00
CD8E	HEX 00
CD8F	HEX 00

CD90	HEX 00
CD91	HEX 00
CD92	HEX 00

CD93	HEX 00
CD94	HEX 00
CD95	HEX 00

CD96	HEX 00
CD97	HEX 00
CD98	HEX 00

CD99	HEX 00
CD9A	HEX 00
CD9B	HEX 00

CD9C	HEX 00
CD9D	HEX 00
CD9E	HEX 00

CD9F	HEX 00	;WAVEFLIP DELAY (MODIFIED)
CDA0	HEX 00
CDA1	HEX 00

CDA2	HEX 00
CDA3	HEX 00
CDA4	HEX 00

CDA5	HEX 00
CDA6	HEX 00
CDA7	HEX 00

CDA8	HEX 00
CDA9	HEX 00
CDAA	HEX 00

CDAB	HEX 00
CDAC	HEX 00
CDAD	HEX 00

CDAE	HEX 00
CDAF	HEX 00
CDB0	HEX 00

CDB1	HEX 00
CDB2	HEX 00
CDB3	HEX 00

CDB4	HEX 00	;notes	v1
CDB5	HEX 00	;	v2
CDB6	HEX 00	;	v3
CDB7	HEX 00	;	v1
CDB8	HEX 00	;	v2
CDB9	HEX 00	;	v3

CDBA	HEX 00
CDBB	HEX 00
CDBC	HEX 00

CDBD	HEX 00
CDBE	HEX 00
CDBF	HEX 00

CDC0	HEX 00
CDC1	HEX 00
CDC2	HEX 00

CDC3	HEX 00
CDC4	HEX 00
CDC5	HEX 00

CDC6	HEX 00
CDC7	HEX 00
CDC8	HEX 00

CDC9	HEX 00
CDCA	HEX 00
CDCB	HEX 00

CDCC	HEX 00
CDCD	HEX 00
CDCE	HEX 00

CDCF	HEX 00
CDD0	HEX 00
CDD1	HEX 00

CDD2	HEX 00
CDD3	HEX 00
CDD4	HEX 00

CDD5	HEX 00
CDD6	HEX 00
CDD7	HEX 00

CDD8	HEX 00	;slide speed lo
CDD9	HEX 00
CDDA	HEX 00

CDDB	HEX 00	;slide speed hi
CDDC	HEX 00
CDDD	HEX 00

CDDE	HEX 00
CDDF	HEX 00
CDE0	HEX 00

CDE1	HEX 00	;waveform SETUP
CDE2	HEX 00
CDE3	HEX 00

CDE4	HEX 00
CDE5	HEX 00
CDE6	HEX 00
CDE7	HEX 00
CDE8	HEX 00
CDE9	HEX 00
CDEA	HEX 00
CDEB	HEX 00
CDEC	HEX 00
CDED	HEX 00
CDEE	HEX 00
CDEF	HEX 00
CDF0	HEX 00
CDF1	HEX 00
CDF2	HEX 00
CDF3	HEX 00
CDF4	HEX 00
CDF5	HEX 00
CDF6	HEX 00
CDF7	HEX 00
CDF8	HEX 00
CDF9	HEX 00
CDFA	HEX 00
CDFB	HEX 00
CDFC	HEX 00
CDFD	HEX 00
CDFE	HEX 00
CDFF	HEX 00
CE00	HEX 00
CE01	HEX 00
CE02	HEX 00
CE03	HEX 00
CE04	HEX 00
CE05	HEX 00
CE06	HEX 00
CE07	HEX 00
CE08	HEX 00
CE09	HEX 00
CE0A	HEX 00
CE0B	HEX 00
CE0C	HEX 00
CE0D	HEX 00

CE0E	HEX 00
CE0F	HEX 00
CE10	HEX 00

CE11	HEX 00
CE12	HEX 00
CE13	HEX 00

CE14	HEX 00
CE15	HEX 00
CE16	HEX 00

CE17	HEX 00
CE18	HEX 07
CE19	HEX 0E

CE1A	HEX 00
CE1B	HEX 00

CE1C	HEX 41	;waveform values
CE1D	HEX 21
CE1E	HEX 11
CE1F	HEX 81
CE20	HEX 51


CF2B	HEX 00	;sid vars (copied over $D400-$D418 / 50th)
CF2C	HEX 00
CF2D	HEX 00
CF2E	HEX 00
CF2F	HEX 00	;waveform
CF30	HEX 00
CF31	HEX 00
CF32	HEX 00

CF33	HEX 00
CF34	HEX 00
CF35	HEX 00
CF36	HEX 00
CF37	HEX 00	;waveform
CF38	HEX 00
CF39	HEX 00
CF3A	HEX 00

CF3B	HEX 00
CF3C	HEX 00
CF3D	HEX 00
CF3E	HEX 00
CF3F	HEX 00	;waveform
CF40	HEX 00
CF41	HEX 00
CF42	HEX 00

CF43	HEX 00



CEE1	DL  CA9C	;look-up table LO
CEE3	DL  CAA4
CEE5	DL  CAA0
CEE7	DL  CAF8
CEE9	DL  CAFC
CEEB	DL  CB0A
CEED	DL  CB0E
CEEF	DL  CB1C
CEF1	DL  CB28
CEF3	DL  CAA8
CEF5	DL  CB24
CEF7	DL  CAF0
CEF9	DL  CAF4
CEFB	DL  CAEC
CEFD	DL  CB20
CEFF	DL  CAAC
CF01	DL  CAC8
CF03	DL  CADA
CF05	DL  CAE2
CF07	DL  CAE8
CF09	DL  CB2C
CF0B	DL  CB3B
CF0D	DL  CB3F
CF0F	DL  CB43
CF11	DL  CB6C
CF13	DL  CB47
CF15	DL  CB4F	;volume
CF17	DL  CB57
CF19	DL  CB5B
CF1B	DL  CA94
CF1D	DL  CB69
CF1F	DL  CB6D
CF21	DL  CB71
CF23	DL  CB75
CF25	DL  CB80
CF27	DL  CBB0
CF29	DL  CBC9

CEE2	DH  CA9C	;look-up table HI
CEE4	DH  CAA4
CEE6	DH  CAA0
CEE8	DH  CAF8
CEEA	DH  CAFC
CEEC	DH  CB0A
CEEE	DH  CB0E
CEF0	DH  CB1C
CEF2	DH  CB28
CEF4	DH  CAA8
CEF6	DH  CB24
CEF8	DH  CAF0
CEFA	DH  CAF4
CEFC	DH  CAEC
CEFE	DH  CB20
CF00	DH  CAAC
CF02	DH  CAC8
CF04	DH  CADA
CF06	DH  CAE2
CF08	DH  CAE8
CF0A	DH  CB2C
CF0C	DH  CB3B
CF0E	DH  CB3F
CF10	DH  CB43
CF12	DH  CB6C
CF14	DH  CB47
CF16	DH  CB4F
CF18	DH  CB57
CF1A	DH  CB5B
CF1C	DH  CA94
CF1E	DH  CB69
CF20	DH  CB6D
CF22	DH  CB71
CF24	DH  CB75
CF26	DH  CB80
CF28	DH  CBB0
CF2A	DH  CBC9


CE21	HEX 06	;note table
CE22	HEX 01
CE23	HEX 16
CE24	HEX 01
CE25	HEX 27
CE26	HEX 01
CE27	HEX 38
CE28	HEX 01
CE29	HEX 4B
CE2A	HEX 01
CE2B	HEX 5E
CE2C	HEX 01
CE2D	HEX 73
CE2E	HEX 01
CE2F	HEX 89
CE30	HEX 01
CE31	HEX A1
CE32	HEX 01
CE33	HEX BA
CE34	HEX 01
CE35	HEX D4
CE36	HEX 01
CE37	HEX F0
CE38	HEX 01
CE39	HEX 0D
CE3A	HEX 02
CE3B	HEX 2C
CE3C	HEX 02
CE3D	HEX 4E
CE3E	HEX 02
CE3F	HEX 71
CE40	HEX 02
CE41	HEX 96
CE42	HEX 02
CE43	HEX BD
CE44	HEX 02
CE45	HEX E7
CE46	HEX 02
CE47	HEX 13
CE48	HEX 03
CE49	HEX 42
CE4A	HEX 03
CE4B	HEX 74
CE4C	HEX 03
CE4D	HEX A8
CE4E	HEX 03
CE4F	HEX E0
CE50	HEX 03
CE51	HEX 1B
CE52	HEX 04
CE53	HEX 59
CE54	HEX 04
CE55	HEX 9C
CE56	HEX 04
CE57	HEX E2
CE58	HEX 04
CE59	HEX 2C
CE5A	HEX 05
CE5B	HEX 7B
CE5C	HEX 05
CE5D	HEX CE
CE5E	HEX 05
CE5F	HEX 27
CE60	HEX 06
CE61	HEX 84
CE62	HEX 06
CE63	HEX E8
CE64	HEX 06
CE65	HEX 51
CE66	HEX 07
CE67	HEX C0
CE68	HEX 07
CE69	HEX 36
CE6A	HEX 08
CE6B	HEX B3
CE6C	HEX 08
CE6D	HEX 38
CE6E	HEX 09
CE6F	HEX C4
CE70	HEX 09
CE71	HEX 59
CE72	HEX 0A
CE73	HEX F6
CE74	HEX 0A
CE75	HEX 9D
CE76	HEX 0B
CE77	HEX 4E
CE78	HEX 0C
CE79	HEX 09
CE7A	HEX 0D
CE7B	HEX D0
CE7C	HEX 0D
CE7D	HEX A2
CE7E	HEX 0E
CE7F	HEX 81
CE80	HEX 0F
CE81	HEX 6D
CE82	HEX 10
CE83	HEX 67
CE84	HEX 11
CE85	HEX 70
CE86	HEX 12
CE87	HEX 88
CE88	HEX 13
CE89	HEX B2
CE8A	HEX 14
CE8B	HEX ED
CE8C	HEX 15
CE8D	HEX 3A
CE8E	HEX 17
CE8F	HEX 9C
CE90	HEX 18
CE91	HEX 13
CE92	HEX 1A
CE93	HEX A0
CE94	HEX 1B
CE95	HEX 44
CE96	HEX 1D
CE97	HEX 02
CE98	HEX 1F
CE99	HEX DA
CE9A	HEX 20
CE9B	HEX CE
CE9C	HEX 22
CE9D	HEX E0
CE9E	HEX 24
CE9F	HEX 11
CEA0	HEX 27
CEA1	HEX 64
CEA2	HEX 29
CEA3	HEX DA
CEA4	HEX 2B
CEA5	HEX 75
CEA6	HEX 2E
CEA7	HEX 38
CEA8	HEX 31
CEA9	HEX 26
CEAA	HEX 34
CEAB	HEX 40
CEAC	HEX 37
CEAD	HEX 89
CEAE	HEX 3A
CEAF	HEX 04
CEB0	HEX 3E
CEB1	HEX B4
CEB2	HEX 41
CEB3	HEX 9C
CEB4	HEX 45
CEB5	HEX C0
CEB6	HEX 49
CEB7	HEX 22
CEB8	HEX 4E
CEB9	HEX C8
CEBA	HEX 52
CEBB	HEX B4
CEBC	HEX 57
CEBD	HEX EB
CEBE	HEX 5C
CEBF	HEX 71
CEC0	HEX 62
CEC1	HEX 4C
CEC2	HEX 68
CEC3	HEX 80
CEC4	HEX 6E
CEC5	HEX 12
CEC6	HEX 75
CEC7	HEX 08
CEC8	HEX 7C
CEC9	HEX 68
CECA	HEX 83
CECB	HEX 39
CECC	HEX 8B
CECD	HEX 80
CECE	HEX 93
CECF	HEX 45
CED0	HEX 9C
CED1	HEX 90
CED2	HEX A5
CED3	HEX 69
CED4	HEX AF
CED5	HEX D7
CED6	HEX B9
CED7	HEX E3
CED8	HEX C4
CED9	HEX 99
CEDA	HEX D0
CEDB	HEX 00
CEDC	HEX DD
CEDD	HEX 25
CEDE	HEX EA
CEDF	HEX 10
CEE0	HEX F8

MEND






;
;  PDS Pc1.21 :000: (c) P.D.Systems Ltd 1985-88
;
;
;       File  "IN GAME CODE PART 2"
;    Version          11.20
;         By  "WELL ITS NOT STEVE!"
;
;     Created on Sat the 14th of May 1988
;        Last update 00:24 on 01/01/80
;

;chars
	ORG	$5000

	HEX 0F3F7C7838000007C0E0181C1C3CFCF80F07000307070300F8E00080C0C080000000387C7C3C38700000387C7C3800007CFEFEFEFE7C
	HEX 7C383810387C7C38000000000F3F7870F0F00000DCFE7E3E3E3EF0F87F7F3F0F00003C7CFCFCFCD8000070F8F9FFFE7878780000E0F8
	HEX 3C1C1E1E787E7F7F773300003E7EFCFCF8E0000000000F3F7870F0F00000E0F83C180000F8FC7F7F3F0F00001C7EFEFCFCF000000000
	HEX 0000387C7C381C3E3EFEFE3C3C3CF0F8F8F8F000E0C03CFCFCFCDC980000C0E0E0E0E0E0E0E00000E0F8FC7C78E0C0C0C0C0C0808080
	HEX 0C3EFEFCFCF00000000F7F1F03030F1F0307070F0F0F0F070000071F3E7C7C7C0000F6FE180C0E0E7E3F3F0F070E0E071EFCFCF0E070
	HEX 70E070F8F8F9FB7F7E7C0000E0F8FCFE7E3E7C787838303000003E3E3E3E3E1C00003C7E7E3C003C7E7E7E7E3C3C3C1800003C7E7E3C
	HEX 003C7E7E7E7E7E7C7CFCF8703C7E7E7E7E7F7F3F000000387CF8E0C03F3D3D1818180000E0F0FCFE7E1C00003C7E7E7E7E7E7E3C3C3C
	HEX 3C1818180000000078FDFFFFFEFC0000F1FBFFFFFEFC0000F0F8F8FCFCFCFC787878303000007C7E7E7E7E3C00007E7E7E7E7E3C0000
	HEX 000078FCFCFE7E7E3E3E3E3E3E1C000000000F3F7E7CFCFC0000E0F81C0C0E0EFCFE7F7F3F0F00000E1EFCFCF8E00000000071FBFFFF
	HEX FCF80000E0F8FCFC3E1E78787C7F793830301E1C3CF8E0000000071F3E3E1C000003E0F81C0E0E1EFCF80F1F3F3F7F7F7F3FE080003C
	HEX FEFEFEFC00001E3F3F3F3F3F1E1E1E1E0C0C0000000078FCFCFC780000000F3F7F7F7F3F0000E0F0800080E01F070307FF7F0000F8FC
	HEX FCFCF8E00000070F0F7FFF7F0F07C0E0E0FCFEFCE0C00707070303030000C0C0C08080800000000018387070F0F8000030381C1C1E3E
	HEX F8FC7F7F3F0F00003E7EFCFCF8E00000000070F8F8F8FC7C00000C0E0E1C1C387E3F3F1F0F03000078F0F0E0C08000007E7F3F3F1F0E
	HEX 00003F3FFFDF8F0700001CB8F8F0E0800000000078FCFCFCFC7E00003C7E7E7E7E3F00000C0E0E0E1C1C000078FEFF7F1F0700000C1E
	HEX 3CF0E0F00F3D78F0F0600000F8FCFCFE7E3C0000000070F8FCFC7E7F00000C0E0E1C1C383F1F0F0367FFFF7CF8F0F0E0C08000000000
	HEX 3F7F3F0000030000F8FCFC78F0C0008080C0C0C0C0800000000000000000000000000000000000000000000000000000000000000000
	HEX 0000000000000000000000000000000000000000000000000000000000000000FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF
	HEX FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF000FFFF00FFFFC0FFFFF0
	HEX FFFFF8FFFFFCFFFFFEFFFFFEFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFEFFFFFCFFFFF8FFFFF0FFFFC0FFFF00FFF00000CE67
	HEX C3FC67C17867C03067C00067C00067C0006380006000006000006000006000007FFF007FFF000000000000DEC61FD8EE1FDEFE06D8FE
	HEX 06DED606DEC60600F00000F00000E00000000000600000600000600000600000600000600000600000E00000E00000000000000000BB
	HEX 8000BB80001B00001F00000E00000E000000007AEEC6CEFE7600C0D8FCE6C6EEBC000078ECC0C6EE7C0006367ECEC6EE7A000078CCCC
	HEX F8E67C003CFE607860F0F0600076CCC6663C6C38C0D8FCEEC6C68600181800181818180018180018181838F0C0DCD8F0D8DECE003078
	HEX 787878303030006CFEFED6C6C60000DCFEEEC6C6C600007CC6C6C6FE7C0000DCFEC6C6E6DCC00076FEC6C6CE7606006CFEFEECC0C000
	HEX 007CE07C0E7EFC0030FC783030303000006CC6C6EEFE7C0000C6C6E66C7C380000C6C6D6FEFE6C0000C6EC383C6EC60000C6C6E67C1C
	HEX 38E0007C0E1C70FE7E00007CC6D6C6FE7C000018383838383000007C0E3C70FE7E00007C0E3C0EFE7C00003C6CCCFE1C180000FCE07C
	HEX 0E7EFC00007CE0FCCEE67C00007E0E1E3C3C1800007CCE7CCEFE7C00007CE67E0EFE7C00000000000030300000006060006060000018
	HEX 30303030180000180C0C0C0C1800000000070C18306000000080C0701F01000101030606070DC0808000000000800103030000000100
	HEX 800080C060C080C018183030180F0306C00000000000000060301818183030600C0F00000000000000FF00000000000000FF00000000
	HEX 0000C0800000000000000000000000000080003C60F860FEFC0000000000000000003C66E7E7E7E7E7E7E7E7E7E7E7E7663CE0E0F0F0
	HEX F8F8FCFC07070F0F1F1F3F3FEEEEE7E7E3E3E1E17777E7E7C7C78787FCEEE7E7E7E7E7E7E7E7E7EFFEE0E0E0E7E7E7E7E7E7E7E7E7E7
	HEX E7E7E7E7E77EFF3C3C3C3C3C3C3C3C3C3C3C3C3C3C3CFFE0E0E0E0E0E0FEE0E0E0E0E0E0E0FFE7E7E7EFFEEEE7E7FEEFE7E7E7E7E7E7
	HEX E7E7E7E7E7E7EFFE7EE7E7E7E0E0E0E0E0E0E0E0E7E7E77E0303030303030303C0C0C0C0C0C0C0C0E7E7F7FFEFE7E7E7E7E7E7E7E7E7
	HEX E7E77EE7E7E7E070381C0E070707E7E7E77E000000000000010300000000000080C001000000000000008000000000000000E7E7E7E0
	HEX E0E0E0E0E0E0E0E0E0E7E7E7000000000000007E7E00000000000000EEE7E7E7E7E7E7E7E7E7E7E7EFFEE0E000000000000000FCE000
	HEX 0000000000000000000000000000000000000000000000000000000000000000001000000000000010382800000000107C387C440000
	HEX 1038FE7C7CEE82001038FE7C7CEE82001038FE7C7CEE82001038FE7C7CEE82001038FE7C7CEE820000107C387C440000000010382800
	HEX 000000000010000000000000000000000000000000000000000000000000000000000000000000000000FFFFFFFFFFFFFFFF


	ORG	$5800	;standard sprs
;mayhem- walk R
;stance R
;jump R
;walk L
;stance L
;jump L
;thumbs up!
;dino x5
;dino crouch x5
;star animations x5
;high-light
;APEX letters

	HEX 0000000000C000F320010C200083E00044500388080408A802080C0124720243820440040242080D319C120C6221038221000C306018
	HEX 30F04C1DF81E07FFFF0000000000000000504000554000554000195001555001555001555400695401695401AA94016AA0006AA0056A
	HEX A4295AA4296A902AAA502A69500A695802A96A000000000000C000F320010C200083E00044500388080408A802080C01247202438204
	HEX 4004024208013188320C304E03D0421C08403E10603FE83D1F1807FFFC00000000000000005040005540005540001950015550015550
	HEX 01555400695401695401AA94016AA0006AA0116AA0155A9025AA9026A6902A9A502AAA5002AA68000000C000F320010C200083E00044
	HEX 500388080408A802080C012472024382044004024208013188010C306103C89200888C0848800468C007F07A0FD00FFFE00000000000
	HEX 504000554000554000195001555001555001555000695401695401AA94016AA0006AA0006AA0005AA0516A9055AA5095A950A6A6502A
	HEX AA90026940000000C000F320010C200083E00044500388080408A802080C012472024382044004024208013188010C34310BCC4E1088
	HEX 44109840117C400EFE3D01FC07FFF00000000000504000554000554000195001555001555001555000695401695401AA94016AA0006A
	HEX A0006AA8005AA0116A9015A95015A56826A6582AA5600269A0000000000000C000F320010C200083E00044500388080408A802080C01
	HEX 247202438204400402420E0D3189124C312487CA20840C2048382030781E80FC07FFFE00000000000000005040005540005540001950
	HEX 015550015550015554006954016A5401AA90016AA0006AA5056AA9156A98155A502699502AA9600AA59802A9A8000000000000C000F3
	HEX 20010C200083E00044500388080408A802080C01247202438204400402420801318C320C344C13C84810884009C84007E83D07F007FF
	HEX F800000000000000005040005540005540001950015550015550015554006954016A5401AA90016AA0006AA8116AA8155A9015A95026
	HEX A5502AA9902AA660026A80000000C000F320010C200083E00044500388080408A802080C012472024382044004024208013188010C30
	HEX 6203D09A0448840428800238C007E87A0FD00FFFE00000000000504000554000554000195001555001555001555000695401695401AA
	HEX 94016AA0006AA0006A90015AA0516A90556A5095A950AAA9502AA99002AA80000000C000F320010C200083E00044500388080408A802
	HEX 080C012472024382044004024208013188010C303203D04E003844000840191C603CEE3D7E1C07FFF800000000005040005540005540
	HEX 00195001555001555001555000695401695401AA94016AA0006AA0006A90015A90116A90256A9025A9502AA9582A995802AA80000000
	HEX 0000018001E6400218400107C00089200710100812900410180248E40487040880080484181263102C9870450FC841080840900820F8
	HEX 9819FC3C07FFFE0000000000000000414001554000554000594001555005655001656001A99401AA5405AA9001AAA0016AA011AA9015
	HEX AA90155A502699602AA9600AA5A002AAA0000000C000F320010C200083E00044500388A804080807080C08A472084382084005042249
	HEX 03118A028C340403C8780008800090803038687FF81FF87C00F01C000000000050400055400055400019500155500155500155540569
	HEX 54056A5406AA9502AAA501AAA401AA9801AA5015695055A5509A95402AA50002A0000000000000000003000004CF0004308007C1000A
	HEX 22001011C01510203010404E248041C240200220104240398CB046304841C08430008418060C320F0C781FB8FFFFE000000000000000
	HEX 01050001550001550005640005554005554015554015690015694016AA400AA9400AA9001AA9501AA56806A96805AAA80569A82569A0
	HEX A96A800000000003000004CF0004308007C1000A22001011C01510203010404E248041C240200220104240118C800C304C0BC0721038
	HEX 42087C0217FC0618F8BC3FFFE00000000000000001050001550001550005640005554005554015554015690015694016AA400AA9400A
	HEX A9000AA94406A55406AA58069A9805A6A805AAA829AA800003000004CF0004308007C1000A22001011C01510203010404E248041C240
	HEX 200220104240118C800C308013C0861100491210311620010FE0030BF05E07FFF0000000000105000155000155000564000555400555
	HEX 4005554015690015694016AA400AA9400AA9000AA9000AA50006A94505AA55056A56059A9A06AAA80169800003000004CF0004308007
	HEX C1000A22001011C01510203010404E248041C240200220104240118C802C308033D08C1108721908223E88027F70023F80BC0FFFE000
	HEX 00000001050001550001550005640005554005554005554015690015694016AA400AA9400AA9002AA9000AA50006A944056A54295A54
	HEX 259A98095AA80A69800000000003000004CF0004308007C1000A22001011C01510203010404E248041C240200220704240918CB08C32
	HEX 4853E1243021041C12041E0C043F01787FFFE00000000000000001050001550001550005640005554005554015554015690015A94006
	HEX AA400AA9405AA9006AA95026A95405A554056698096AA8265AA02A6A800000000003000004CF0004308007C1000A22001011C0151020
	HEX 3010404E248041C240200220104240318C802C304C13C83211081213900217E0020FE0BC1FFFE0000000000000000105000155000155
	HEX 0005640005554005554015554015690015A94006AA400AA9402AA9002AA94406A554056A54055A98066AA8099AA802A9800003000004
	HEX CF0004308007C1000A22001011C01510203010404E248041C240200220104240118C800C30800BC0461220591420211C400117E0030B
	HEX F05E07FFF00000000001050001550001550005640005554005554005554015690015694016AA400AA9400AA90006A9000AA54006A945
	HEX 05A955056A56056AAA066AA802AA800003000004CF0004308007C1000A22001011C01510203010404E248041C240200220104240118C
	HEX 800C30800BC04C1C0072100022389802773C06387EBC1FFFE00000000001050001550001550005640005554005554005554015690015
	HEX 694016AA400AA9400AA90006A90006A54006A94406A958056A58256AA82566A802AA800000000001800002678002184003E080049100
	HEX 0808E009481018082027124020E12010011018212008C6480E193413F0A2101082100902191F043C3F987FFFE0000000000000000141
	HEX 00015540015500016500055540055950095940166A4015AA4006AA500AAA400AA94006AA4406AA5405A554056698056AA809AAA00AAA
	HEX 800003000004CF0004308007C1000A22001511C01010203010E04E251041C210A002109244205188C02C314013C02010001E0900011C
	HEX 0C011FFE163E1FF8380F000000000001050001550001550005640005554005554015554015695015A95056AA905AAA801AAA4026AA40
	HEX 05AA40056954055A550156A6005AA8000A800000000000180000240000430000F1F0010C1002022002526006023809C4840838480402
	HEX 50121E48290C8426C18A523E5D4404293C04210402620641E20F03F41FFFF80000000000140000140000154000555001554001554002
	HEX 5650055A54056A9001AA9002AA9020AA94296A542969552955550155560156A80155A80A56A80AAAA00000000003C3C00DBE7035EB5C
	HEX 379ADC37AADCE7AADBE9AA6BEBAAEB3BAAEC3AFFAC0E96B03A55ACEA6DABED757B39756C396D6C3A55ACEF96FBEAD7ABD9BE67000000
	HEX 0003C3C00DBE7035EB5C379ADC37AADCE7AADBE9AA6BEBAAEB3BAAEC3AFFAC0E96B03A55ACEA69ABED7D7B397D6C39696C3A55ACEF96
	HEX FBEAD7ABD9BE670000000003C3C00DBE7035EB5C379ADC37AADCE7AADBE9AA6BEBAAEB3BAAEC3AFFAC0E96B03A55ACEA65ABED797B39
	HEX 7E6C39796C3A65ACEF96FBEAD7ABD9BE670000000003C3C00DBE7035EB5C379ADC37AADCE7AADBE9AA6BEBAAEB3BAAEC3AFFAC0E96B0
	HEX 3A55ACEA79ABED797B39796C39796C3A55ACEF96FBEAD7ABD9BE670000000003C3C00DBE7035EB5C379ADC37AADCE7AADBE9AA6BEBAA
	HEX EB3BAAEC3AFFAC0E96B03A55ACEA6DABED757B39756C39756C3A55ACEF96FBEAD7ABD9BE670000000000000003C3C00DBE7037EBDC37
	HEX 9ADC37AADCE5AA5BE9AA6BEAAAAB3AAEACEEBEBBEABAAB3A55AC3D6D7C39756C396D6C3A55ACEF96FBEAD7ABD9BE6700000000000000
	HEX 03C3C00DBE7037EBDC379ADC37AADCE5AA5BE9AA6BEAAAAB3AAEACEEBEBBEABAAB3A55AC3D697C397D6C39696C3A55ACEF96FBEAD7AB
	HEX D9BE670000000000000003C3C00DBE7037EBDC379ADC37AADCE5AA5BE9AA6BEAAAAB3AAEACEEBEBBEABAAB3A55AC3D797C397E6C3979
	HEX 6C3A55ACEF96FBEAD7ABD9BE670000000000000003C3C00DBE7037EBDC379ADC37AADCE5AA5BE9AA6BEAAAAB3AAEACEEBEBBEABAAB3A
	HEX 55AC3D797C39796C39796C3A55ACEF96FBEAD7ABD9BE670000000000000003C3C00DBE7037EBDC379ADC37AADCE5AA5BE9AA6BEAAAAB
	HEX 3AAEACEEBEBBEABAAB3A55AC3D6D7C39756C39756C3A55ACEF96FBEAD7ABD9BE67000000000000000000000000000000000000000000
	HEX 00001800003C0000FF0000FF00007E0000FF0000FF0000C3000000000000000000000000000000000000000000000000000000000000
	HEX 0000000000001800003C00003C0001FF8003FFC001FF8000FF0000FF0001FF8001E78000810000000000000000000000000000000000
	HEX 000000000000000000001800003C00003C00007E0007FFE00FFFF007FFE003FFC001FF8001FF8003FFC003FFC007E7E00781E0020040
	HEX 00000000000000000000000000001800003C00003C00007E00007E000FFFF01FFFF81FFFF80FFFF007FFE003FFC003FFC007FFE007FF
	HEX E00FFFF00FE7F00F81F006006000000000000000001800003C00007E00007E0000FF0000FF003FFFFC7FFFFE7FFFFE3FFFFC1FFFF80F
	HEX FFF007FFE00FFFF00FFFF01FFFF81FFFF83FE7FC3F81FC3E007C18001800000000000000000000000000000000000000000000000000
	HEX 007FFF004001004001004001004001004001004001004001004001004001004001004001007FFF0001FF8003C3C00781E00781E00781
	HEX E00781E00781E00781E00781E00781E00781E00781E00781E00781E00781E007FFE00781E00781E00781E00781E00781E00000000000
	HEX 0000007E0000E70001C38001C38001C38001C38001C38001C38001C38001C38001C38001C38001FF8001C38001C38001C38001C38000
	HEX 000000000000000000000000000000000000003C0000660000C30000C30000C30000C30000C30000C30000C30000FF0000C30000C300
	HEX 00C30000000000000000000000000000000000000000000000000000000000000000003C00006600006600006600006600006600007E
	HEX 0000660000660000000000000000000000000000000000000000000000000000000000000000000000000000000000001800003C0000
	HEX 2400002400003C00003C0000240000000000000000000000000000000000000000000000000000000000000000000000000000000000
	HEX 000000000000001800003C00003C00003C00003C00000000000000000000000000000000000000000000000000000000000000000000
	HEX 00000000000000000000000000000000000000001800003C00003C000000000000000000000000000000000000000000000000000000
	HEX 000000000000000000000000000000000000000000000000000000000000000000180000180000000000000000000000000000000000
	HEX 00000000000000000000000007FF800783C00781E00781E00781E00781E00781E00781E00781E00781E00781E00781E00781E00781E0
	HEX 0783C007FF800780000780000780000780000780000000000000000001FE0001C70001C38001C38001C38001C38001C38001C38001C3
	HEX 8001C38001C38001C70001FE0001C00001C00001C00001C0000000000000000000000000000000000000000000FC0000C60000C30000
	HEX C30000C30000C30000C30000C30000C60000FC0000C00000C00000C00000000000000000000000000000000000000000000000000000
	HEX 000000000000007C00006600006600006600006600006600007C00006000006000000000000000000000000000000000000000000000
	HEX 00000000000000000000000000000000000000003800003C00002400002400003C000038000020000000000000000000000000000000
	HEX 0000000000000000000000000000000000000000000000000000000000000000003800003C00003C0000380000300000000000000000
	HEX 000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000003800003C00003000
	HEX 000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
	HEX 000000000018000010000000000000000000000000000000000000000000000000000000000007FFE007800007800007800007800007
	HEX 800007800007800007800007800007FF8007800007800007800007800007800007800007800007800007800007FFE000000000000000
	HEX 01FF8001C00001C00001C00001C00001C00001C00001C00001FF0001C00001C00001C00001C00001C00001C00001C00001FF80000000
	HEX 0000000000000000000000000000000000FF0000C00000C00000C00000C00000C00000FE0000C00000C00000C00000C00000C00000FF
	HEX 0000000000000000000000000000000000000000000000000000000000000000007E00006000006000006000007C0000600000600000
	HEX 6000007E0000000000000000000000000000000000000000000000000000000000000000000000000000000000003C00003000003000
	HEX 003800003000003000003C00000000000000000000000000000000000000000000000000000000000000000000000000000000000000
	HEX 000000000038000030000038000030000038000000000000000000000000000000000000000000000000000000000000000000000000
	HEX 000000000000000000000000000000000000380000300000380000000000000000000000000000000000000000000000000000000000
	HEX 000000000000000000000000000000000000000000000000000000000000001800001800000000000000000000000000000000000000
	HEX 000000000000000000000781E00781E00781E00781E00381C001C38000E700007E00003C00001800003C00007E0000E70001C3800381
	HEX C00781E00781E00781E00781E00781E00781E00000000000000001C38001C38001C38001C38000E700007E00003C00001800003C0000
	HEX 7E0000E70001C38001C38001C38001C38001C38001C3800000000000000000000000000000000000000000C30000C30000C300006600
	HEX 003C00001800003C0000660000C30000C30000C30000C300000000000000000000000000000000000000000000000000000000000000
	HEX 00000000006600006600003C00001800001800003C000066000066000066000000000000000000000000000000000000000000000000
	HEX 0000000000000000000000000000000000003400003400003C00001800003C0000340000340000000000000000000000000000000000
	HEX 000000000000000000000000000000000000000000000000000000000000002400003C00001800003C00002400000000000000000000
	HEX 000000000000000000000000000000000000000000000000000000000000000000000000000000000028000038000010000038000028
	HEX 000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
	HEX 280000100000280000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
	HEX 000000000000000000000000001800000000000000000000000000000000000000000000000000000000000000000000000000000000
	HEX 000000000000000000000000000000000000000000000010000000000000000000000000000000000000000000000000000000000000
	HEX 00001FFFF81FFFFC18000E18000718000318000118000018000018000018000018000018000018000018000018000018000018000018
	HEX 001C18001E18001F18001B00007FFF00FFFF01C000038000870000CE0000FC00787801FC3007FC000FFC003FF800FFE003FF9C0FFFBC
	HEX 1FCFBE3F8F9E3E079F1CE78F01E78703E7878767C300E00000E0000060000060000060700060F80061F80061F80063F00063E00067C0
	HEX 006780006F80006F00005F00005E00003E00007C0000FC0000F80000F800000018001918001818001818001818001818001818001818
	HEX 00181800181800181800181FFFF81FFFF8000000000000C67336EEFB36FEDB37FED9E7D6F8C6C6D8C600



;game character sprs-
;cyberdyne
;retrograde
;creatures
;info x 2

	HEX 001000003800000C00002C000030000002000027000C07001CE20000DE0000160000680000F70000C70001060001818001C18000C180
	HEX 00410000C1C001E1E013003800007C00000E00007E000078007B3700FFBF007FFF801DFF0001FF80007F0000FF0001FF8001FF8003C7
	HEX 8003E78003E3C001E3C000E1E001F3E003F3E0390000000060000080000070000684000F6E000F06000079000FBC0007BD8000798001
	HEX 840001DC00008800030200038300038700078700048E00030C80078BC00000E00001F00001E80007F8001FFE001FFF003FFE001FFF00
	HEX 1FFF001FFFC00FFFC001FD8003FF0003FF0007DF8007CF8007CF800FDF800FDF800FDF001FDF80030000000501401945901A56900955
	HEX 8005994006EE4017FF5017775027FF6066EE646599646955A46995A46969A46555641656501A56901A9A901A56901A9A900000000000
	HEX FF0003F1C00FE0F01FE0F83FF1FC3FFFFC7FC7FE7F83FE7F83FE7F07FE7F07FE7F07FE7E0FFE3E0FFC3E0FFC1E03F80F07F003FFC000
	HEX FF0000000000000000000000000E00001F00001F00000E00000000003800007C00007C0000F80000F80000F80001F00001F00001F000
	HEX 01FC0000F80000000000000000000000

ENDSPRS	;intro ORG'd at this address ($71C0)





;
;  PDS Pc1.21 :000: (c) P.D.Systems Ltd 1985-88
;
;
;       File  "SHOP PROCEDURES ETC"
;    Version          11.20
;         By  " PROGRAMMED BY JAZ "
;
;     Created on Sat the 14th of May 1988
;        Last update 00:24 on 01/01/80
;



	ORG	$8000

	DL	SCREWMEM
	DH	SCREWMEM
	DL	SCREWMEM
	DH	SCREWMEM
	HEX	C3C2CD3830


STARTMUSIC

; cyberdyne warrior music

CW.MUSIC	HEX 6F00FA00000117C51878011F2502020D0A08034115D20E50
	HEX 04081F0708010D280C000F03CA801F00080002090A4A0DBC
	HEX 0C020F01228E237E0200020C130011001400236302001020
	HEX 11001403236302001008110014FE23630200100812030208
	HEX 228E237E0200020C110023630200101000011E0001020A0A
	HEX 39034104090EFF15D2070305021F0C0D820C00B280190123
	HEX 9B0200130023EA020008011F00228E02060A661100D620D9
	HEX 20D820D4201002110022D6BE10C11022D9C010BC1022D8BE
	HEX 10BC1022D4C110C01010041F0C0801BE00190123EA020008
	HEX 000341239B0200C1001901BC0019010A0CBE00190111001B
	HEX FF00101900100F0040190000011E00010D2C0C0102090A4D
	HEX 040815D20E9B0341A680A68003810F030A09110000081900
	HEX D1040004190010071100D10410030A371F00080122822303
	HEX 03001300CA04CA0423030300CA041100CA04CA0423030300
	HEX CA04100F1100C804C80423030300C80410041100CA04CA04
	HEX 23030300CA0410041100C804C80423030300C80410041100
	HEX CA04CA0423030300CA04100422CA1100BE04C10423030300
	HEX BE04C104C00423030300BC04100822C81100BC04C0042303
	HEX 0300BC04C004BE0423030300B904100222CA1100BE04C104
	HEX 23030300BE04C104C00423030300BC04100222C81100BC04
	HEX C00423030300BC04C004BE0423030300B904100222CA1100
	HEX BE04C10423030300BE04C104C00423030300BC0410022282
	HEX 080003811100CA04CA0423160300CA04103F1100CA041004
	HEX 03410F001100B204BE02B20223030300B202BE02B204B202
	HEX BE0223030300B20410101100CA04CA0423160300CA04101F
	HEX 1100CA041004120200011E229AAD08229DB204229AAD0498
	HEX 042298AD04229DB20422989804241100AD08B204AD08AD04
	HEX B2081007AD08B204AD04B204AD04B204B20424020A0A3913
	HEX 001100BE28BC08C504C304C104BC04B924B908BE08BE04B9
	HEX 04BC041002090C11000F0122BCB0280F00B408B704B504B4
	HEX 04B0040F0122BEB2240F00B508B908B704B504B904100212
	HEX 0224B5E808010311C108C008BC04BE04BCE8BC08BE08C504
	HEX C80424020F080003810F03D1040F00080103410206240381
	HEX 020F08000F03D10402062400070003810F03D60402062400
	HEX 0800100E07090801B204070023B703000709B204070023B7
	HEX 03000709B2040700080007090801B204B204B204B2040700
	HEX 08001205070908011100A604000419001005003819000001
	HEX 1E0D000C00070002080A0907090311040815640E64BE0207
	HEX 090DFA0C000341240D000C00070002080A09070903110408
	HEX 15640E64C00207090DFA0C000341240D000C00070002080A
	HEX 0907090311040815640E64C10207090DFA0C000341241300
	HEX 11009A049A049D0498041003A1049F049D049C0412039A04
	HEX 9A049D0498049A049A049D049804A104A1049F049F049D04
	HEX 9D049C049C042411009A049A0408010311B704034108009A
	HEX 049A049A049804980410039A049A0408010311B904034108
	HEX 009D049F04A104A1049D04249A049A049D049A029A049A04
	HEX 9A0298049D049A049A049D049A029A049A049A0298049504
	HEX 240F0122B2AF080F00B208B208B0040F0122B2AF0C0F00AD
	HEX 08B008B2080F0122B5B2080F00B008AD08B0080F0122B2AF
	HEX 080F00B008AD08B0080F0122B2AF200F00A620020BA1209A
	HEX 2002090F0122ADA9080F00AD08AD08B0040F0122B2AF0C0F
	HEX 00AD10B0080F0122B5B20822B0AD1022B2AF080F00B008AD
	HEX 080F0122B0AD100F000F0122B2AF200F00A620020BA1209A
	HEX 20020924004D00BE000001180017001D00250302090A0F03
	HEX 41040815FA0E64010011001B0100041900100F000419009A
	HEX 209A2011009A089A069A069A0498089A089A069A069A02A6
	HEX 029804A402980210039A0800011E000102090A0803110408
	HEX 15640E6408011F0C0DBC0C021100BE02BE02C102BE0223E5
	HEX 0000BE02C102BE02BC02BE02C102BE0223E50000C10223E5
	HEX 0000BC021004110023000100BE02C102BE0223E50000BE02
	HEX C102BE0223000100BE02C102BE0223E50000C10223E50000
	HEX BC02100600011E000102080A5D03410E6415FA1100B520B4
	HEX 20B240100211001BFF00051900100F00011E0F030DE80C03
	HEX 070908000341B5020311070008010DBC0C020F00240F0307
	HEX 0008000341AD020311080107000F0024BC97008601000125
	HEX 021402020B0A090341040815FA0E641F0C11009A049A089A
	HEX 089A0898049A049A089A049D049D049F049F04100311009A
	HEX 049A0423B102009A089A0423B1020098049A049A0423B102
	HEX 009D089C0423B1020098049A049A0423B102009A089A0423
	HEX B1020098049A049A0423B102009A049D0423B102009F049F
	HEX 04100911009A0823B10200980410029A0823B102009D049A
	HEX 2000011E0001010F02080A080311040815D20E6408011F0C
	HEX 05030040190014020D900C0113001100238B020000021900
	HEX BE04239E020000021900BE04238B020000021900BC04239E
	HEX 020000021900BC041003238B020000021900C102C102239E
	HEX 020000021900C102C102238B020000021900BC02BC02239E
	HEX 020000021900BC02BC021100238B020000021900BE04239E
	HEX 020000021900BE04238B020000021900BC04239E02000002
	HEX 1900BC041003238B020000021900C102C102239E02000002
	HEX 1900C102C102238B020000021900C302C302239E02000002
	HEX 1900C302C30212051100239E020000061900100700201900
	HEX 00011E0001020A0A0A03410E6415FA040F00401901140E11
	HEX 009A04A604A604A604A604A6049A04A6049A04A604A6049A
	HEX 04A604A604A604A6041005A604A6049A04A604A604A604A9
	HEX 04A904A904A904AB04AB04A904A804A604A404A904A804A6
	HEX 04A404A904A804A604A404A904A804A604A404AB04A904A8
	HEX 04AB04AD04AD04AD04AB04AD04AD04AD04AD04AB04AB04AD
	HEX 04AB04AD04AD04A904A105141A070304080DE60C00020CA9
	HEX 0BA814A80CA614A6200F01090822A9A420020811009A049A
	HEX 0498049A048E049A0498089A049A0498049A048E049D049C
	HEX 04980410039A0498049D0498049A04950498089A0498049D
	HEX 049A049A049D049F04A104020B9A00190100011E0F030700
	HEX 08000341A6020311080107020F00240F03070008000341AD
	HEX 020311080107020F002402080F0207090801AD0408000700
	HEX 0F00020B2408000341AD020311080107050F002402080F02
	HEX 07090801AD04080007000F00020B24BC5700100100012506
	HEX 14030DBC0C02011F02060A070341040815FA0E64076417C1
	HEX 1D6418C8110023B3010002091006140F110023D801001020
	HEX 141B23730100237301000209110023D801001008110023B3
	HEX 010010040000190100011E0001020D0A0A0341040515FA0E
	HEX 502018140F008019001100239401001002200C076402090A
	HEX 8A15D20DC80C0014031300110023640100A6029A04235901
	HEX 009A02236401009C0223640100A402980423590100980223
	HEX 6401009C0223640100A6029A04235901009A02236401009C
	HEX 0223640100A4029F04236401009D029C0298021006008019
	HEX 001100236401000006190023590100000219002364010000
	HEX 02190010081202020A0A0A9A80140F201811002394010010
	HEX 0400011E0001020D0A0A03410E2815FA0405140300801900
	HEX 1100239401001002140F110023F901002373010023730100
	HEX 100514030E5015D2008019000A8A11002394010010040001
	HEX 1E020A0F03A902020D0F0024020A0F030800A6020800020D
	HEX 0F0024020C0A090EFAA604A904A804A404A60CA404A608A9
	HEX 02A804A40AAB02A904A802249A189F089D149C0498089A24
	HEX A1049F049D02A1029A0498049504910295022413009A02A1
	HEX 02A402A602A102A602A4029A0212039A02A102A402A602A4
	HEX 02A902A6029D02249A02A102A6029A02A102A6029A02A102
	HEX 98029F02A40298029F02A40298029F0224020A0AAA03410E
	HEX 5015FA04059A1098109A1098049F029D049D029C0298029A
	HEX 1098109A1098049F029D049D029C02980224573100540000
	HEX 012503010F02090A0A0341040815FA0E6414020F010D6C0C
	HEX 07238D0000238D00000D900C011100238D0000102400011E
	HEX 00011402008019000341110023A800001002A6409A401100
	HEX 1BFF00181900100F00011E0001140E0080190103111F0C08
	HEX 010002190023A800000A0F1100A620A62010020088191300
	HEX 011E0F03A6020F00240F03AD020F00242298A602229A1300
	HEX A602A60412032298A604229DAD04229CA60424020A0AA804
	HEX 0C15D20EFFAD02AF02B02CAF06AD0AA640AD02AF02B02CB5
	HEX 06B20AB920B72024021900101B0020190000011E0F03A602
	HEX 0F00240F03AD020F002406DD009901D40100012502140201
	HEX 0F1F0C020C0A0C03410E5015D204080DBC0C021100233502
	HEX 002335020023560200233502002356020023560200234B02
	HEX 0023350200100211009A08236C02009A049A08236C020098
	HEX 04100413001100233502009A04236C020023350200233502
	HEX 009A04236C020098041002233502009A04236C0200233502
	HEX 002335020023350200236C0200234B0200233502009A0423
	HEX 6C020023350200233502002356020023560200234B020011
	HEX 0023350200A604236C02002335020023350200A604236C02
	HEX 00A40423350200A604236C020023350200A6042335020023
	HEX 6C0200A90423350200A604236C02002335020023350200A6
	HEX 04236C0200A40423350200A604236C020023350200233502
	HEX 0023350200236C0200234B020023400200A404236C020023
	HEX 40020023400200A404236C0200A30423400200A404236C02
	HEX 0023400200A40423400200236C0200A80423610200A10423
	HEX 6C02002361020023610200A104236C02009F0423610200A1
	HEX 04236C0200234B0200235602002356020023560200234B02
	HEX 00100314041202020E0A0C9A80190100011E000114020209
	HEX 0A8803410EDC15D2040A004019010DFA0C00237D02001300
	HEX 02090A881100237D020010020F00020E0A0C1404A6801901
	HEX 120200011E000103410EFF15D2040A14022018070305020D
	HEX FA0C0022B0091000401902130002090A881100B210B910BE
	HEX 10B910B210B9100F01BE200F00B010B710BC10B710AD10B4
	HEX 100F01BC200F001002020E0A0C1402B48019011404120200
	HEX 011E0F011F00229AAD040F00240F011F002298AD040F0024
	HEX 0F011F00229DAD040F00240F011F00229FAD040F00240F01
	HEX 1F002295AD040F00241F0C08010F030709B20407000F0008
	HEX 00240F0122B2A68022B0A44022ADA14024BCBCBCBC030303
	HEX 0303030303030016191A1916161A191A1911232423111124
	HEX 2324230010035281B42BCC9A7B4D80100000000000000000
	HEX 0000000000000000040F0F0F0FFAFBFCFDFEFF0F0F00030F
	HEX 0FFAFBFCFDFEFF0F0F0000FBFCFDFEFF01000F0F00FFB900

CW.ENDMUSIC









;
;  PDS Pc1.21 :000: (c) P.D.Systems Ltd 1985-88
;
;
;       File  "SPRSPLIT DATA ETC.."
;    Version          11.20
;         By  "JOHN 'JAZ' ROWLANDS"
;
;     Created on Sat the 14th of May 1988
;        Last update 00:24 on 01/01/80
;

;retrograde

R.MUSIC

R.TITLEM	HEX 210118020001140317C10764011F25018268190102DF0A0F0341040815D20EFF
	HEX 0D0A0C000F0122B218501D00AD960F00B24202CFB0C0BEC0BCC0B9C0B7C00E50
	HEX 9AC098C09AC098909F189D18020C0AAC0D2C0C0118FA1D0F13001100229A2327
	HEX 0300232703009A06232703002341030098069A0C233403009A06234103009806
	HEX 9A06229F9F069F06232703009F06234103009F06229D23340300233403002327
	HEX 03002341030095069806100C1100229A2334030022A623340300234103002327
	HEX 03002341030022A42334030022A1232703002341030022982327030023270300
	HEX 10081100229A23270300233403009A069A0C98069A0C9A0C9A069A0C98069A06
	HEX 9F069F0C9F069F0C9F069D0C229D9D0C9D062341030095069806100412200000
	HEX 190000011E0001140302080A68040815D20E50201003410DDC0C050503090C0F
	HEX 0122B2110022C5B20CB50CB20CB92422CAC51822C3B50CB20CB50CB72422C8C3
	HEX 1822C1B20CB50CB20CB52422C5C11822C0B50CB20CB50CB42422C3C01810060E
	HEX FF040F0DFA0C000F00230703001300110022BE0930BE54BC0CBE30C10CBE0CBC
	HEX 0CB90C0F01BCC00F0022BCB254B00CB230B50CB90CBE0CBC0C0F01BE60020609
	HEX 0022BEBC3022C1BE300F00020A1AF41002140323070300031104001100BE30B9
	HEX 30BC30B718BE18B930B530B730B218B91810020341230703001203B218110000
	HEX 6019001BFF10071100B218B518B418B0181BFF10080030190000011E00011403
	HEX 02090A68040815D20EFF0D280C0003411100B9C0B7C0B5C0B4C010030E50090C
	HEX 0F01228E0703050513001100B206B20CB20CB506B00CB206B20CB20CB506B00C
	HEX B706B70CB70CB906B70CB506B50CB50CB706B40C10061100B206B20623120300
	HEX B20CB50623120300B006B206B20623120300B20CB50623120300B006B7062312
	HEX 0300B706B70623120300B90623120300B706B506B50623120300B50623120300
	HEX B7062312030023120300100A050203531100B20CBE0CC10CBE0CB50CBC0CC00C
	HEX BC0C100403411100B206B006B20CB50CB206B506B706B20CB706B21810081220
	HEX 0000190000011E020D0A00A68019010A8A2403470700BE01B901C101BE01B901
	HEX C10107030341240F010381BE010341A6050F00240F010381BE010341A60B0F00
	HEX 240F010381C5010341A90B0F0024


R.SHOPM	HEX 4300A4000001140217C50764011F250102CA0A08040815D20E500D900C011850
	HEX 1D050381BE97CA8A02DFCA90020A0F0118E61D55110023D60100A60523D60100
	HEX AB0510FF00011E00011402A6B0190102080A48040815D20E5503810D840C030F
	HEX 03110000181900BE0C000C19001010110003170F001300BE0612040F030381BE
	HEX 0C03170F00BE06BE0603170F00BE12BE060F030381BE0C03170F00BE060F0303
	HEX 81BE0610FF00011E00011402826002AA0A0A15D20EA0031313001100BE01BC01
	HEX BB01B901B701B5011016BE0412028240020D0381CA8019010D580C0217C10381
	HEX 1100004C19010F0202CAB219001B190010021300070303410F00040402091100
	HEX 23CB010082051040110023CB0100B20523CB0100B20523CB0100820523CB0100
	HEX B20523CB0100820523CB0100820523CB0100B20523CB010082051008110023CB
	HEX 0100B20523CB0100B20523CB0100BE0523CB0100B20523CB0100BE0523CB0100
	HEX BC0523CB0100B20523CB0100BE0510100A5A03410700110023CB0100BE051010
	HEX 110023CB0100BC051010110023CB0100B90510201100C501BE0510101100C101
	HEX BC0510101100BE01B905102003410700040F0A8A1100B260B060ADB4B00C1002
	HEX A6C08240190212FF00011E03170700DD010703034124229A0381C5010341229A
	HEX A6050381C50103419A05229D0381C5010341A605229A0381C50103419805229D
	HEX 0381CA010341AB050381C50103419D050381C5010341A6050381CA01034124


R.HIGHM	HEX 6300E000000125021402010F020B0A090341040815FA0E501F0C130011000008
	HEX 1900237301000004190010079508237301002373010011009A04A60423730100
	HEX A6049A04A60423730100A6049804A40423730100A4049F04A10423730100A104
	HEX 101612FF00011E0001140202090A0A04080E5015D208011F0C0D2C0C01110023
	HEX 51010000041900236401000004190010101300110023510100BE0423640100BE
	HEX 0423510100BE0423640100BE0423510100BC0423640100BC0423510100B90423
	HEX 640100BC041003110023510100B90423640100BC041003110023640100100412
	HEX FF00011E0001020C0A0A15D20EFF14020DE60C000F0109080000190113000703
	HEX 04080500031122BEB9F80341BC08110023860100C30423860100BC04C110C010
	HEX BC10BE1010020700040F1100BC10BE101003C108C008BC08B908BC8003171100
	HEX BE20C110BE101004A600190112FF00011E0F03070008000341A9040311080107
	HEX 020F00240F0308000341AD04031108010F002402080F0207090801AD04080007
	HEX 000F00020B24BE08C504BE08BC04C10424


R.GAMEOVERM
	HEX 33006400000117C118FE1D0F0764011F2501020D0AAA04080E5015D203411300
	HEX 1100A6019A0B9A0C980C100AA601950B8E0C12FF00011E000102CD0A08828019
	HEX 0403571C0C08020D0A0C000F010930110022B9A680190122B7A48019011A0C10
	HEX 030000190000011E000102090A0A8280190103210DC80C000F03090613001400
	HEX 238F000014FE238F000012FF00011E1100CA019A0BC501A60B101024

R.MOTHER1	HEX 3900A4000001020C0A0A0341040815D20E5017C118E61D50011F25010DF40C01
	HEX 11001402230101002301010014052301010014032301010010FF00011E000102
	HEX 0A0A3915D20EFF040805001300070303411100140E23B1000023B10000141123
	HEX B10000140F23B1000010021100140223C60000140523C600001002110014F623
	HEX C6000014F923C60000100214020700B2C0B060B330B530B2C0B060AE30A930A6
	HEX 80190112FF00011E00010000190000011E9A0C9A0C9D069F0C9D06A10CA406A6
	HEX 0CA106980C24B206B506B206B206B506B206B206B506B206B706B206B506B906
	HEX B506B506B906B706BC06B906B206C106B206B206BE0CB506B20CB506B206B50C
	HEX 240381C50103410F01229AA60B0381C5010341A60B9D060F01229F0381C50103
	HEX 41AB0B0F009D060381C50103410F0122A1AB0B0F00A406A60CA1060381C50103
	HEX 410F012298A60B0F0024


R.WELLDONE	HEX 8B0000010001010F020C0A08034104060E5515D2076425030DF40C01228E18DC
	HEX 1D500F011100B2041BFE100817C1011F0F0011009F0410081100A1041008229A
	HEX 9A1025010F011100234C010022A6A60C234C010022A1A10C1002130014001100
	HEX 234C010022A6A60C234C010022A1A10C100414071100234C010022A6A60C234C
	HEX 010022A1A10C1405100212FF00011E000102090A06038104080E5515D20D580C
	HEX 021F0C0F031100B204100803410F0011009F04AB0410041100A104AD04100402
	HEX 0D0F0009080341A69019011300140002090A061F071100237B0100C30C237B01
	HEX 00BE0C100403410A660F0008001100233B01001002110014FE233B0100100212
	HEX FF00011E0001020A0A29038104080EFF15D21F0C080107099A02A103A606A615
	HEX 02020A7203411100A602AD02B002B2021008A6040019194B00011EAD0CAD0CAD
	HEX 0CAB0CAD0CAB0CA80CA60C24229AAD0C0381B90103419A0B0381C5010341AD11
	HEX 22A1A106AD060381B9010341A105A10C22A40381C5010341AD0B240F000341A6
	HEX 0C0F030311BE0CBC0CBE0CC10CBE0CC10C24


R.GETREADYM
	HEX 25003C00000117C118500764011F251802AF0AAA03410E5015D20D050C000F01
	HEX 229D9AA119FC00011E00010AAA03170DC80C000F0122BE11009D0110FF00011E
	HEX 00010A4403131100C101C00110FF00011E

R.MOTHER2	HEX 89003201000117C10764011F2501020D0A08040815D20E3C0D2C0C01034118F0
	HEX 1D3C0F011300233F0100A60400131900234A0100A604000719002298233F0100
	HEX 000B1900229A12081100229A1300233F0100A41100061900234A0100A90B2298
	HEX 233F0100A40B229A1203229D233F0100A40B233F0100A40B229F234A0100A90B
	HEX 9F06234A0100A90510FF00011E0001020915D20EFA0D500C000506095A0F0104
	HEX 08130022A60353040011009A18BE24BE2410080408034107030A881100BC0CBE
	HEX 0CB20CBE0CBE0CB20CC10CC30CBC0CBE0CB20CBE0CBE0CB20CC30CC10C100411
	HEX 0022B2B2C0B5C0B2C022B5B790B03010020080190107021100A6180311BE18C1
	HEX 18BE18C322002619000341AB18A6180311C318C118BC18BE22002619000341A9
	HEX 181004070303531100BE06BE0C102ABE0C12FF00011E00010000190000011E18
	HEX E60381C501034118E62418E60381CA01034118E624

R.MOTHER3	HEX 6F000A01000117C10764011F2501020D0ADA0341040815D20E5018F51D140D2C
	HEX 0C010F01229A13001100A606000E19001003A60600041900A606000419001208
	HEX 1100229A2317010023170100229D23170100229823170100229A231701002317
	HEX 0100229D23170100229F2317010010FF00011E00018240190102090A7A031104
	HEX 0715D20EFF0D740C590904050C20201100C528C80AC50AC314C528C314BE1410
	HEX 04070303410F0108011300110022C5B20AB50AB70AB50A22C8B20A22C5B50A22
	HEX C3B70AB90A22C5B50AB20AB50AB70A22C3B00AB40A22BEB90AB70A1004070511
	HEX 00B20AB50AB70AB50AB20AB50AB70AB90AB50AB20AB50AB70AB00AB40AB90AB7
	HEX 0A1004070312FF0000190000011E00010000190000011E0381C5010341A6099A
	HEX 0A0381C5010341A6090381D60103419A040381D60103419A040381C5010341A6
	HEX 090905A60A09000381C5010341A6090381D6010341A60924

R.MOTHER4	HEX 6900EE00000117C10764011F25018201020A0A08040815D20E500DF40C010F01
	HEX 18000313228E110023FB0000101018E61D5513001402110023FB000010301100
	HEX 1402231B0100231B0100231B0100231B01001405231B0100231B01001403231B
	HEX 0100231B0100100612FF00011E00011402A680190102090A0A0DB00C040F0303
	HEX 17230C010013001100C50CBE061005BE0612100F00020DB28019010209230C01
	HEX 0003810F010D840C03110000181900BE0C000C19001008110003170F00B206B2
	HEX 06B206B2060F030381BE0C0F000317B206B90603130F03C50CBE0C0F030381BE
	HEX 0C03170F00D1060F030381BE0610FF00011E00010000190000011E0381C50103
	HEX 11A6050381C50100051900241100C50203000004190003171080240381C50103
	HEX 41229AA6050381C50103419A05229D0381C5010341A605229A0381C501034198
	HEX 05229D0381CA010341AB050381CA0103419D050381C8010341AB050381C50103
	HEX 41AB0524

R.MOTHER5	HEX 6500DC000001140202090AAA0341040A15D20E5017C118DC1D3C0764011F2503
	HEX 11009A04A604001019009804A40410040D900C010F0113001402110023FE0000
	HEX 10041400110023FE0000100414FE110023FE0000100414FD110023FE00001004
	HEX 12FF0000190000011E0001140202050A49040F0EFF150C03531100A678A40810
	HEX 011300034114021100B238B008100414001100B238B508100414FE1100B238B0
	HEX 08100414FD1100B238B508100403211402110023E9000010101400110023E900
	HEX 00101014FE110023E90000101014FD110023E90000101012FF0000190000011E
	HEX 00010000190000011EB202AD02B902AD020381BE020321B202AD02A90224229A
	HEX A90422A6A604229AAD04229A9A04229DA906229C9C02AD022298980222A4A404
	HEX 229AA90422A6A604229AAD04229A9A042295A906229898022295AD0222989802
	HEX 22A4A402AD0224

R.MOTHER6	HEX 7500C4000001140202EF0A0A0341040E15D20E3217C118550764011F25039A80
	HEX 020911009A029A02A1029A029D029A0298029A02100C11009A02A6029A029A02
	HEX 00181900100413001402110023D1000010041400110023D10000100414FE1100
	HEX 23D10000100414FD110023D10000100412FF005019C300011E0001140202080A
	HEX 08034104080E6915D200401900110023F20000101813001402110023F2000010
	HEX 081400110023F20000100814FE110023F20000100814FD110023F20000100812
	HEX FF005019C300011E00010000190000011EAD02A402A902A602AD02A602AB02A9
	HEX 02A602A602A902A402AB02A402A802A402249A029A02A6029A02A6029A02A102
	HEX 9D0224

R.MOTHER7	HEX 5700A000000117C10764011F250102ED0A0A0341040815D20E509A8019019A00
	HEX 19039D00190318E61D5A020D110014FB23AD000023AD000014FD23AD000023AD
	HEX 000014FE23AD000023AD0000140023AD000023AD000010FF00011E000102F80A
	HEX 6A040315D20E7807050504201882C003419AC002081100140723E0000023E000
	HEX 00140923E0000023E00000140A23E0000023E00000140C23E0000023E0000010
	HEX FF00011E00010000190000011E9A06A606A1069A06A40CA60C9A0CA1069A06A4
	HEX 0CA60C9A06A606A1069A06A40CA60C9A069A06A1069A06A4069D06A6069D0624
	HEX 1300B206A606A606A9069A06AB0CA6069A06A606A9069A06AB069A06AD0C1202
	HEX 24

R.LOADM	HEX 7100F600000117C50764011F25018201020C0A0C040815D20E5018DC1D5A0DBC
	HEX 0C0211008206107A140F110003418E0603008206100303411100236A01001020
	HEX 0AFD14031100236A010010080F011100232901009A050381D101034122989805
	HEX 23290100AD050381D10103412298AD0510FF00011E0001140302DD0A0A04080E
	HEX FF15D20DC80C0003410B321603BE801901B98019010F0109BE22B0B280190122
	HEX B7B580190122B0B280190122B2B0801901020CA68019010B0013000313091811
	HEX 00BE601004090003411100237B0100BC0C237B0100C3060D0F0C270381BE0623
	HEX 7B0100BC0C237B010003810D0F0C27BE06BE06100812FF00011E00018202020F
	HEX 0AAF03410E3715D2040F140320241300BEC0B560B760B28019011100A6C0A9C0
	HEX A4C0A6C010FF00011E0381CA0103412298A9050381D6010341229A9A050381CA
	HEX 0103419A050381CA0103419A050381D1010341229DAD050381C5010341229AA9
	HEX 050381D6010341229F2498069A069A069A069D069A069F06980624031307000D
	HEX DC0C050209BE0CB206BE0603810DE80C03C502B902BE02020C0DDC0C050317BE
	HEX 0624


;R.MUSIC1	HEX 79000401000125021402011F17C118281DFF0764020C15D20EFF14021601040F
;	HEX 0F0122B90313000019011100CA080038190010041300090C0A6A0D900C011100
;	HEX 0313CA080341B908B508B008B50CB2140313CA080341B908B508B408B00CB214
;	HEX 1004091E0A8A0D500C00110022B0B240100322B5B230B710122000011E000114
;	HEX 0202090A0A04080E5015D2031108011F0C05030D2C0C01110023C90100000419
;	HEX 0023DC01000004190010101300110023C90100BE0423DC010008000702BE0423
;	HEX C90100B20423DC0100BE0423C90100B20423DC0100BE0423C9010008000702B7
;	HEX 0423DC0100B5041003110023C90100BE0423DC010023DC01001003110023DC01
;	HEX 00100412FF00011E00011402020B0A090341040815FA0E501F0C110000081900
;	HEX 23EB010000041900100611000004190023EB0100100323EB010023EB01001100
;	HEX 9A049A0423EB01009A049A049A0423EB01009C049D049D0423EB01009D049F04
;	HEX 9F0423EB01009D04100613001100A6049A0423EB01009A049A04A60423EB0100
;	HEX 9C04A9049D0423EB01009D04AB049F0423EB01009D04100811009A04A60423EB
;	HEX 01009A04A6049A0423EB0100A6049804A40423EB01009804A404980423EB0100
;	HEX A404100412FF00011E0F03070008000341A9040311080107020F00240F030800
;	HEX 0341AD04031108010F002402080F0207090801AD04080007000F00020B24
;
;
;R.MUSIC2	HEX 8701D601000117C1076401102501020A0A8A040815D20E3C0D900C01034118E6
;	HEX 1D41229A0F0111001B0100301900100F003019001100238F0200000519001080
;	HEX 1100238F0200A605238F020000051900238F020000051900238F020000051900
;	HEX 1020011F1100229D238F0200A60B9D069D0623960200AD059D0C238F0200A605
;	HEX 9D06238F0200A6052298238F0200A605238F02009A0523960200AD0B238F0200
;	HEX A605238F0200A605229A238F0200A60B9A0C23960200AD05238F0200000B1900
;	HEX 238F0200A605238F020000051900238F0200A605238F020000051900238F0200
;	HEX A60523960200AD0500061900238F0200A6059A06229F238F0200A60B9D069D06
;	HEX 23960200AD059D0C238F0200A6059D06238F0200A6052298238F0200A605238F
;	HEX 02009A0523960200AD0B238F0200A605238F0200A605229A238F0200A60B9A0C
;	HEX 23960200AD05238F0200000B1900238F0200A605238F020000051900238F0200
;	HEX A605238F020000051900238F0200A60523960200AD0500061900238F0200A605
;	HEX 9A0610FF0000190000011E000102090A0A040F15D20E9B0381229A09040DC80C
;	HEX 00034107031100061414032347020014FE237002001400234702002370020014
;	HEX 052347020014FE237002001400234702002370020010FF00011E000102090341
;	HEX 040015D20EFF0D2C0C010F000705050400001903130003410A4A1100B218B50C
;	HEX B206B20CB20CB206B50CB40CB218B50CAD06B00CB40CB006B40CB50C10040311
;	HEX 0AAA1100BE0CC10CBE06C10CBE06C10CBE06C30CBE06C10C10080080190112FF
;	HEX 0000190000011ECA01B205D101B205CA01BE05CA01B2050700D1010381C10503
;	HEX 410703CA01BE05CA01B205D101B20524D101BE05B206B206BE0607000381C106
;	HEX 03410703B206CA01BE05CA01BE05240381CA010341240381D101034124
;
;
;R.MUSIC3	HEX 8D002C010001140217C10764011F2501020A0A08040815D20E3C0D2C0C010341
;	HEX 18E61D550F011100239B0100A4050012190023A60100A90500061900239B0100
;	HEX A4050006190010081100229A1300239B0100A4110006190023A60100A90B2298
;	HEX 239B0100A40B229A1203229D239B0100A40B239B0100A40B229F23A60100A90B
;	HEX 9F0623A60100A90510FF0000190000011E0001140E02090A08040015D20E9B03
;	HEX 472020050313009A80190111009A8AA406A40CA40CA60CA60C9A8AA406A60CA6
;	HEX 0CA40CA40C10021100237F0100000C1900B00CB20CAD0CB20CB018B21E238801
;	HEX 00237F010000181900B20CAD0CB20CB518B21E2388010010021100237F010000
;	HEX 0C1900A40CA60CA90CA60CA418A61E23880100237F0100A90CA80CA60CAD0CA6
;	HEX 0CAB18AD1E23880100100212FF00011E0001140E02080A590341040415D20E9B
;	HEX 0D2C0C010F01229A09050708060613001100BE06B206B206A90623B10100A406
;	HEX B5061003C106B506B506AB0623B10100B70623CE010012FF0000190000011E9A
;	HEX 180E1407030353240E9B034707000400A406A40CA40CA60CA60C2418E60381C5
;	HEX 01034118E62418E60381CA01034118E6240F001F0C0341060007090801A60C08
;	HEX 000F010708060603411F00229A240F001F0C0341060007090801A60608000F01
;	HEX 0708060603411F00229A24
;
;
;R.MUSIC4	HEX 7900E60000012503011017C507641D00187811001B0100041900100F00041900
;	HEX 00401900130002080A08034104081100B904B904B206B206B204B208B904B904
;	HEX B206B212100404040EFF15D21100B204B902B902B202BE02B902B202BE04B902
;	HEX B202B904B902B202101012020030190011001BFF000C1900100F00011E00010A
;	HEX 6903410E3C15D204090D200C031C0C229A0F0113000D200C03020911002298AD
;	HEX 04229A23670100229AB204229A23670100229DAD04229A23670100229FB20422
;	HEX 9D23670100102002080D900C011100229AAD0200061900229AB2020006190010
;	HEX 081220005019C300011E00011F0C03410EFF15D2040E02080A8722CA13001100
;	HEX 2374010010080381020B0800110000081900C504000419001008034102080801
;	HEX 1100237401001008110023910100100811002391010014072391010014002391
;	HEX 0100BF02BE02BA02B902020C03810800C504080102080341BE02B90210041220
;	HEX 005019C300011E08020381B201034108009A03240801B202B202BE02B202020C
;	HEX 03810800C504080102080341BE02B20224B902B902B502BE02020C03810800C5
;	HEX 04080102080341B002BE0224
;
;
;R.MUSIC5	HEX DB0078010001015D02090A06040F0E6415D2140307641F0C17C11DFF03410DDC
;	HEX 0C059F089D0813001100A604A904A604A604A604AB04A604A404A604A904A604
;	HEX A604A604AB04AB04A90410021100A404100EAB04A90412021300239902002399
;	HEX 020023C20200120323990200020A040A130014F71D9B08001100BE08BE04BC04
;	HEX C104BE0CBE08C108BC04BE0CBE08BE04BC04C104BE0CBE08C108C308C508C314
;	HEX C30CC118C108BE08B208B204B20CB220100214031DFF239902002399020023C2
;	HEX 020023990200140F0801239902002399020023C202002399020012FF00011E00
;	HEX 01250214030A090341040815D20E641F0C02099F089DB09F109D080DC80C009A
;	HEX 3C9A269A1E98309F089D081300110098049A0898049A089A04980498049A0898
;	HEX 049F089D081002020C9840020912021300110098049A0898049A089A04980498
;	HEX 049A0898049F089D0810029704980897049808980497049704980897049D089C
;	HEX 0898049A0898049A089A04980498049A0898049F089D0812FF00011E00011403
;	HEX 02080A08040815640E6408011F0C0503001019000D2C0C011300110004002377
;	HEX 0200000419001026040823770200238A0200238A0200238A0200120211002377
;	HEX 020000041900101623770200238A0200238A0200238A02001300110023770200
;	HEX 000419001007238A020000041900120311002377020000041900238A02000004
;	HEX 1900101F23770200A602A902238A0200AD02B0021300110023770200B202B202
;	HEX 238A0200B202B2021008110023770200B002B002238A0200B002B00210041100
;	HEX 23770200B202B202238A0200B202B202100223770200B202B202238A0200238A
;	HEX 0200238A0200238A0200238A0200238A020012FF00011E0F03070008000341A6
;	HEX 040801070903110F00240F03070908010341AD0403110F0024B202A602A904A6
;	HEX 04B202B202A604AB04A604A404B202A602A904A604B202B202A604AB04AB04A9
;	HEX 0424B002A402A404A404B002B0021100A4041004B002A402A404A404B002B002
;	HEX A404A404AB04A90424
;
;
;R.MUSIC6	HEX F1011403000125021402010F1F0C020C0A9C03410E5015D204080D200C030000
;	HEX 190111000008190023850400000C1900238504000004190010079D04A9042385
;	HEX 0400A90423850400A80423850400A40411009A04A6042385040023A104002385
;	HEX 0400A4049A04A6042385040023A1040023850400A8049D04A90423850400A904
;	HEX 9D04A90423850400A9049C04A80423850400A8049C04A8042385040023940400
;	HEX 9A04A6042385040023A1040023850400A4049A04A6042385040023A104002385
;	HEX 0400A8049D04A90423850400A9049D04A90423850400A9049F04AB0423850400
;	HEX AB049F04AB04238504002394040010031100235E0400A60423850400A604235E
;	HEX 0400A60423850400A404235E0400A60423850400A604235E0400A60423850400
;	HEX A804236B0400A90423850400A904236B0400A90423850400A904235E0400A804
;	HEX 23850400A804235E0400A8042385040023940400235E0400A60423850400A604
;	HEX 235E0400A60423850400A404235E0400A60423850400A604235E0400A6042385
;	HEX 0400A804236B0400A90423850400A904236B0400A90423850400A90423780400
;	HEX AB0423850400AB0423780400AB042385040023940400101A1100235E0400A604
;	HEX 23850400235E0400235E0400A60423850400A404235E0400A60423850400235E
;	HEX 0400235E0400A6042385040023850400101000011E0001140202080A0803410E
;	HEX 9B15D2040A008019001F0C08011100B90400041900B50400041900B704000419
;	HEX 00B40400041900B50400041900B20400041900B40400041900B0040004190010
;	HEX 04110023A804001006020A110023A8040010081300140202060A071F0C110023
;	HEX A80400100423C90400080103411F0C11001402231005002310050014FD231005
;	HEX 00231005001400231005002310050014FB231005002310050010040700080002
;	HEX 080A071402110023A8040010040080190023C904001F0C08011100BE04BE08BE
;	HEX 04C504BE04BE08C508BE04C504BC04C104C504BE04BE04BE04BE04BE04C504BE
;	HEX 04BE08C508BE04C504C404C304C104C0041005120202AA0AAA080003410700B2
;	HEX 60B010B204AD04B004A904A68011009A709810100800011E00011402023B0A49
;	HEX 03410E9615D2040A08010703055A1F001100A604A604A404A90410041100232C
;	HEX 0400100C0800020D0ACD15E1034107000B001402B220B510B410130003410700
;	HEX 0B001402110023510400100302DD0A0F16020BFA2102B2C0020D040915D20EFF
;	HEX 140E07030ADD1F000DFA0C000F0122B50920A6400F000A0D07031100B208B004
;	HEX B208B508B504B204B504B208B504B704B504B404B208B004B208B708B908B708
;	HEX B908B508B404100405001100230B040010021203020B0A0803111F0C08011100
;	HEX B2801010020813001100BE04BC04C104C0041003BC04B904C004BE0412041100
;	HEX 1BFF00141900100F00011E07030341B22007000311AD20B020AB2007030341B2
;	HEX 2007000311AD20B020B5202413001F18A6041F1BA6041F18A4041F17A9041F1B
;	HEX A6041F1FA6041F24A4041F21A904120224B220B510B410B220B510B710240F01
;	HEX 1F00229AB0041F0C0F00240F011F00229DB0041F0C0F00240F011F00229FB004
;	HEX 1F0C0F002408010F030709B50407000F000800240F011F0022A8B2041F0C0F00
;	HEX 24A6049A04A60424B904BE04B504C104B704C304B404B204B504C104B204C004
;	HEX B404BC04B004BE0424020907030500034108001F001100B204B504B904BC04BE
;	HEX 04B904BC04BE041008080111001F1FB2041F18B5041F16B9041F10BC041F0FBE
;	HEX 041F11B9041F10BC041F0ABE04101024BE04B204BE04B20424
;
;
;R.MUSIC7	HEX ED009002000125031402011F026A0A660341040C15FA0EFF076417C11D191800
;	HEX 0D640C001D191800B910B710B510B210B510B4100F0122BE1D0F0908B2201F0C
;	HEX 026A0A061D3C1850110023E00300230D04001002110023380400238504001002
;	HEX 040F23CE04000EFF15D2040C08010F001402070305021DFA20000B0023C30300
;	HEX 1100B908C508B708C308B508C108B208BE08B508C108B408C008B210BE10B708
;	HEX C308B508C108B408C008B008BC08B508C108B408C008B210BE101003080023CE
;	HEX 040023E00300230D04001100233804002385040010021100BE08C104BE08C108
;	HEX C304BE08C104BE08C108BC04102000011E00011402020A0A0A03410E5015D204
;	HEX 080DBC0C0200701900950898081300110023730300A60423AA03002373030023
;	HEX 730300A60423AA0300A40423730300A60423AA030023730300A6042373030023
;	HEX AA0300A90423730300A60423AA03002373030023730300A60423AA0300A40423
;	HEX 730300A60423AA030023730300237303002373030023AA030023890300237E03
;	HEX 00A40423AA0300237E0300237E0300A40423AA0300A304237E0300A40423AA03
;	HEX 00237E0300A404237E030023AA0300A80423730300A60423AA03002373030023
;	HEX 730300A60423AA0300A60423730300A60423AA03002389030023940300239403
;	HEX 00239403002389030010041100237303002373030023AA0300237E0300237303
;	HEX 002373030023AA0300237E030010041100237E0300237E030023AA0300237E03
;	HEX 00237E0300237E030023AA0300237E030010021100237303002373030023AA03
;	HEX 00237E0300237303002373030023AA0300237E03001002120202090A19110023
;	HEX 7303009A029A029A049A04238903009D04A104A1029C022389030098049C049A
;	HEX 0423890300950498081004020A0A0A122000011E0001140202880A6603110EFF
;	HEX 15D2040820400D640C000BFF1601210AAD10AB10A910A610A910A8100F0122B9
;	HEX 0908A6200B00026A0A06034123F30400231C0500028C0A190F0003110B641602
;	HEX 211E1300140E110023C3030010031100C110C010BC10BE10C110C010BE201002
;	HEX 03412000070305020E500B00140221000000190102CA0AAA040F0E03A640A910
;	HEX A810A620A420A820A910A810A620190104080E50026A0A060B02211E140E0311
;	HEX 23C30300034102DD140200C01900A640026A23F30400231C0500110023C30300
;	HEX 10020000190111001BFF00181900100F00011E0F011F00229AAD040F00240F01
;	HEX 1F002298AD040F00240F011F00229DAD040F00240F011F00229FAD040F00240F
;	HEX 011F002295AD040F00241F0C08010D900C010F030709B20407000F000DBC0C02
;	HEX 080024B910B710B510B210B510B410B220B710B510B410B010B510B410B22024
;	HEX 1D3C1850B906B902B90800301900B906B902B90800101900BE02BE04BE02C104
;	HEX C102C302C304C304C104C30424B706B702B70800281900B708B906B902B90800
;	HEX 101900BE02BE04BE02C104C102C302C304C304C104C304241D3C1850B906B902
;	HEX B90608011D28BC02BE04BE08C108C108C104C004BE08B9041D500800B906B902
;	HEX B90408011D28B904BC08BE081D500800BE02BE04BE02C104C102C302C304C304
;	HEX C104C30424B706B702B70608011D28B902BC04BC08C008C008C004BE04BC081D
;	HEX 500800B704B906B902B90408011D28B904BC08BE081D500800BE02BE04BE02C1
;	HEX 04C102C302C304C304C104C304241100A604A604A404A90410081100A404A404
;	HEX A104A80410041100A604A604A404A904100424B206B202B20800301900B206B2
;	HEX 02B20800101900B902B904B902BC04BC02BE02BE04BE04BC04BE0424B006B002
;	HEX B00800281900B008B206B202B20800101900B902B904B902BC04BC02BE02BE04
;	HEX BE04BC04BE0424


;disk intro
R.DBIT	HEX AB00D002000117C10764011F25010A00040815D20E5018F01D3C0D900C010208
	HEX 0F01820103411100228E23D1040023D1040023D1040023F8040010061300020F
	HEX 0AAA1100229A23D1040023D10400229D23D10400229F23F80400229A23D10400
	HEX 23D10400229523D10400229823F804001007229A23F8040022A623D104002298
	HEX 23F8040022A423D1040022A623F8040022A423D1040022A123F80400229D1100
	HEX 0381C5010341A905100812FF00011E00018280190102060A2603810EFF15D204
	HEX 0F0705050813001F0C080123F0030007010A6612090D000C191F000800020A0F
	HEX 002080031122B223210400200003008250190123D3030014F413000311070504
	HEX 05CA0623C80300CA0623C80300CD0623C80300C806CA0623C80300C806CA0623
	HEX C80300CD0623C803001100CF03100412071100CA031006C8061100CD031006C8
	HEX 061100CF03100EC8061300CA06B206CA06BE06CD06B206C806CA06BE06C806CA
	HEX 06BE06CD06B2061100CF03100412131100CA031006C8061100CD031006C80611
	HEX 00CF03100EC80607000DC80C0002080A88034111000F0109A822C3BEC00960C1
	HEX C009A822C3C5C0096022C3C160091822C1C33022BCBE30100214001F0C080103
	HEX 81070523F0030023F00300130023F003000701120614F4040F0341070002090A
	HEX 66204008002321040020001F00040813000300829023D3030012040300828019
	HEX 0103170D980C080900020D0F0122B2140C1100B906B912030082060317B90603
	HEX 00000C19000317B906B90CB906030082060317B9060300000C19000317100C11
	HEX 00B906B9120300000619000317B906C10CB906B90CB9060300000619000317B9
	HEX 06C50C1008030013000050190123D30300120203410E5004080D900C0114F40F
	HEX 010DC80C000EFF040F0A8811001BFF09A822C3BEC01BFF0960C1C01BFF09A822
	HEX C3C5C01BFF096022C3C160091822C1C33022BCBE30100203000F0011001BFF82
	HEX C010041BFF0341BE60BC601BFFB980190100011E00018200190602080A860341
	HEX 0E9B15D2040E050C130007031100BE0CC10CBE06C10CC30CC106C30CC80CCA0C
	HEX 10040DE80C030020190103110F0122D10906B23C22A1D1240341130011002352
	HEX 040023210400020C0A0DA680190102080A6903008280190403411100B20CB20C
	HEX B50CB20CB20CB706B50CB506B20CB50CB50CB90CB50CB50CB706B506B206B006
	HEX AD06B006B20CB518B50CB724B00CB224AD0CB018B2181004030082F419020341
	HEX 02290A6AB70C1100B912B912B90CB712B712B70CB912B912B90CB212B212B70C
	HEX 100802080A8603008200190303411202AD60A960A600190300011E0BFF160221
	HEX 08BE8019010B0024070003008E0603110705240313020ABE0211001400BE011B
	HEX FE1AFA1007820101008226011F1400241100BE06B206C1061005B9061100C106
	HEX B506C5061005BC061100BE06B206C1061005B9061100BC06B006C0061005B706
	HEX 241100CA06C806CD061005CA061100CA06C806D1061005C8061100BE06BC06C1
	HEX 061005C5061100BC06B906C0061005BE06240D900C0109900700BE48C118C318
	HEX C1180F0122C1090AC318C31822BC095CBEC00F00BE48C118C318C118C318C10C
	HEX C306C8060F0122C3C590C1300F00BE48C118C318C1180F0122C1090AC318C318
	HEX 22C1095CBEC00F00BE48C118C318C118C318C10CC306C806C548C818CA18C806
	HEX CA0CCD06C806CA0CCF06C806CA0CD106240381BE010341A90B0381BE0103419A
	HEX 059A060341AD010381C50503419A060381BE010341A90B240381BE010341A90B
	HEX 0381BE0103419A05A9060341AD010381C50503419A060381C5010341AD050381
	HEX C3010341AB0524


R.DINTRO	HEX 5300C4000001186E17C525021402011011001B0100081900
	HEX 100F02DA0ABA03410E5015D20408000819000D1E0C000F01
	HEX 22A69A4019010F00A480A670A810A978A808A680021D0A0A
	HEX A66411001BFF000C1900100F00011E000102040A0603110D
	HEX 140C050F03090214021F0C00801900110023290100100611
	HEX 000D140C0513000801BE02B2020800B404B5040801C102B4
	HEX 020800B204B404BC02B502B40412030801BE02B2020800B4
	HEX 04B5040801C102B402B204C104C002BE02BC041005110023
	HEX 290100100A00011E0001140202080A080E6415D204080341
	HEX 0DBC0C020F01229A1100AD08AD08AD08AD2810031100AD08
	HEX AD08AD04AD04AD281002020A1100AD08AD08AD04AD04AD10
	HEX AD08AD08AD08100411001300AD041206AD10AD04AD04AD08
	HEX AD04AD04102000011EB204B404B504B404B204B404B504B4
	HEX 0424BCBCBCBCBCBC

;retro game complete - check end as this may be too much data!
R.GCOMP
	HEX A3005802000117C50764011F250182C0034104080E1915D21D00182802DC0A9A8E8019019A8019010EFF188C1100A6C0A4C0A9C0A8C0
	HEX 10030A0AA68019010A8AC5C0AD60B260020E0A0AB280190117C113000A8A11000209040C1DFF0E1E183CB20CB206B206B506B70CB20C
	HEX B20CB206B50CB70C100418D21D78020C0EFF0408BE18BC18C10CBC06C106BC0CBE3CB930BE18BC18B90CBC06BE06BC0CBE3CC118C318
	HEX 12FF00011E000102380A69040415D20E5A0D640C0022B20F0109100BFA1602110023200300BE1723200300B20B000C19002320030000
	HEX 0B190023200300BE0B23200300000B190023200300BE0B232003000017190023200300B2172320030000171900232003000017190010
	HEX 05110023200300BE1723200300B20BC10C232003000313CA0B23200300BE0B232003000313D10B23200300BE0B23200300C10B000C19
	HEX 0023200300B217232003000313C51723200300C10BC30C10100341229A0B000900020E0A0A04080DF40C01AD501901020A229D110023
	HEX 120300AD0B1002229F110023120300AD0B100213001100229A23120300AD0B231203009A0B229D23190300B00B229823120300980522
	HEX 9A231203009A0523120300AD05231203009A0B229D231203009D05229F23190300B00B229D23120300AD0B10141100229A23190300AD
	HEX 0B23190300000B190023120300A605000619002319030000051900231903000005190023190300AD0B23190300000B190023190300A6
	HEX 050006190023190300A60523190300B205100323190300AD0B23190300000B1900110023190300A6051004110023120300AD05231203
	HEX 00AD05100412FF00011E0001A6C102DA0A9D040815D20E140D2C0C01201803418E401902A6C00EFFA9C0A8C0ADC0ABC00E509DC09CC0
	HEX A1C0A4C00EFFB2C0B2C0B9C0BCC0BE001903020E0A091602A6001903020A130003210BFA0F00BE18BC18C10CBC06C106BC0CBE3CB930
	HEX BE18BC18B90CBC06BE06BC0CBE3CC818CA180F01110003410B0023FD0200BC0CB90C0F010341020A23FD0200BE0C0F010341020A22B9
	HEX BC0C100212FF00011E22BEBC18020803410F00B90CB906B90CB90CB906240381D6010341240381D1010341240381CA01035324A00B8C
	HEX 11D08C22D0C88C23D0A92C8D1E05A275A046200208A9968D00DDA9FC857FA9008D20D08D21D08D0C86857E8D1BD085738571857120B6
	HEX 84A90F20B684A2A0EAEAEAA9009DFFD7A9FF9DFF43CAD0F3868020FA857818A936A21AA0792016CCA91B8D11D0203A7DA9F38D1CD08D
	HEX 977CAD0F86F0FBA9008D0F8620907DA580A47ED01BC903D00DAD977CC91BF0E220DF854CDE78C906D006205A804CDE78C908D0CEC002
	HEX F0CA2024

R.ENDMUSIC











;
;  PDS Pc1.21 :000: (c) P.D.Systems Ltd 1985-88
;
;
;       File  "SFX PLAYER:uses X&Y"
;    Version          11.14
;         By  "JAZ : <C> 1987 APEX"
;
;     Created on Mon the 16th of May 1988
;        Last update 00:24 on 01/01/80
;

;creatures

C.MUSIC

C.INGAMEM	HEX 0900120000010000190000011E00010000190000011E000117C40766011F2501
	HEX 020A0A0A0341040815D20E500D900C0118FA1D14110023200200100413001400
	HEX 0D900C0109000F011100229AA60800081900229D9D08A60400041900229FAB08
	HEX 229595082298A60822999908229AA60800081900229F9F08A60400041900229D
	HEX AB08229595082298A6082299990810041100238301002383010014FE23830100
	HEX 140023830100100409080F002320020014032320020014052320020014072320
	HEX 02000F0109060DC80C00140C2349020014072349020014052349020014072349
	HEX 0200140C2349020014072349020014052349020014032349020014000F002349
	HEX 020023200200090011000D900C01234902002349020014FE2349020014009A10
	HEX 002819009A089A08000819009A089A08001019000F0109080D1E0C009F100F00
	HEX 0D900C0110020D100CA40F020904110014002383010014FE2383010010020DD0
	HEX 0C07110014002383010014FE23830100100314FD2383010014FB2383010012FF
	HEX 00011E229AA6080311B201B901B201B90182040341229D0381B90103419D07A6
	HEX 04000419000381C5010341AB010381C5010341A601229F9F0422959508229803
	HEX 81B9010341A60722999908229AA6080311B201B901B201B901B2010321BE01B9
	HEX 010381229F0381B90103419F07A604000419000381C5010341AB010381C50103
	HEX 41A601229D9D042295950822980381B9010341A60722990381B9010341990724
	HEX 9A08000819009D08000819009F089508980899089A08000819009F0800081900
	HEX 9D08950898089908249A08A6080381C50103419D07A6089F0895080381C50103
	HEX 41980799089A08A4080381C50103419F07A6089D0895080381C5010341980799
	HEX 0824

C.ENDLEVELM
	HEX 0900120000010000190000011E00010000190000011E0001076617C418FA1D0F
	HEX 011F2501020A0A0A04080E3715D20D900C010F012298110023CA0000AD071AFF
	HEX 1008140013001100229A23C30000A40723CA00009A0723C30000A907229A23CA
	HEX 00009A071003229D23C30000A40723CA00009D07229823C30000A90723CA0000
	HEX 98071100229A23C30000A40723CA00009A0723C30000A90723CA00009A039A04
	HEX 1003229D23C30000A40723CA0000AB07229F23C30000A90723C30000A9071220
	HEX 00011E0381BE010341240381CA01034124


C.GRMUSIC	HEX 6B008801000117C118F01D14011F2501020D0A0D04080E3715D203410F010D2C
	HEX 0C010F001100229A234C02002295234C0200229A23590200229F9F0C229D2359
	HEX 0200229E9E0C229F234C020022A1234C0200229F23590200229A9A0C22982359
	HEX 02002299990C10200000190000011E0001020C0A0C04080E0F15D2034105061F
	HEX F40DF40C0122AD00C0190011000800070003419A0C2381020000061900236602
	HEX 0000061900000C1900236602000012190023660200000619000800070003419F
	HEX 0C23810200000619002366020000061900000C19002366020000121900236602
	HEX 0000061900100110021100234502009A0C23810200BC0623660200BE06C106C3
	HEX 0623660200BE06BC06C10623660200BE06234502009F0C23810200C106236602
	HEX 00C506C306C10623660200C106C506C30623660200BE06234502009A0C238102
	HEX 00C50623660200C806C506C80623660200CA06CD06CF0623660200CA06234502
	HEX 009F0C23810200C80623660200CA06C806C50623660200C306C106BE06236602
	HEX 00C50610200000190000011E000102DA0A08040F0EFA15D205060DA00C0F8290
	HEX 0381BE30020907010F011100BE0100051900BE0100051900BE0100051900BE01
	HEX 00051900BE0100051900BE0100051900BE0100051900BE0100021900BE010002
	HEX 1900100C110022B2BE06BE0100051900BE0100051900BE06BE0100051900BE01
	HEX 00051900BE06BE0100021900BE010002190022B7BE06BE0100051900BE06BE01
	HEX 0005190022B9BE06BE0100051900BE06BE0100021900BE010002190010200000
	HEX 190000011E070003410800240381BE0103410F01A6170F00240381BE0103410F
	HEX 01A60B0F0024020C070008000F030381D1010341AB050F00080103410703020A
	HEX 24020C070008000F030381D1010341AB050381D1010341AB050381D1010341AB
	HEX 050F00080103410703020A24


C.DEATHM	HEX 510084000001011F2501020D0A0D0311040815D20E9B0D580C020F01090622CA
	HEX BE120DB00C0422A60900CA0C18FA1D1417C10F0082180341A70CAA0CA50CA70C
	HEX A20CA50CA00CA20CA70CAA0CA50C020AA74000011E0001020F0AFF0311040815
	HEX D20E411100CA01BE01B2011AFF1020020A0A0A14008230034199069B0C9E0C02
	HEX 0A990C9B4000011E00018206020D0A0A0311040E15D20EFF1100CA01BE01B201
	HEX 1AFF1010020A070205061F0C08011418A70CAA06A50CA506A70C040CA20CA506
	HEX A00CA0060408A206A2069B0C9E0C990C9B4000011E


C.HIGHM	HEX E9005201000117C10764011F250102CD0A0C040815D20E3C0DC80C000F010353
	HEX 18A01D4614021100CA01C801C501C101BE01C101C501C801102003411100229A
	HEX A6080018190023230200A60800081900A6080018190023230200001019001002
	HEX 13001100229AA6202323020023500200232302009A102298A620232302002350
	HEX 0200232302009A102295A6202323020023500200232302009A102298A6202323
	HEX 020023500200232302009A101005229A11009A20A6109A109A109A10A6109A10
	HEX 22981002229D9D20A6109D109D109D10AD109D10229CAD20AD10A9109C10A610
	HEX 232302002298981012FF00011E000102090A2A03110705050614021F0C080111
	HEX 001300B208B208BE08B908C108BC08BE08B90812021300AB08AB08B708B208BE
	HEX 08B908B708B90812021300A608A608B208AD08B908B208BC08B90812021300AB
	HEX 08AB08B708B208BE08B908BC08BE08120210FF00011E000114020341040F15D2
	HEX 0E9B0DC80C000F00229A9A00190402AA130011000A8AB210B020B220B5200A0A
	HEX B2400A8AB0100F0122B00910B220B220090022B20A0CADE00F000A8AB220B510
	HEX B220B520B9200A0ABE400A8ABC100F0122C00910C1200DF40C0122BCC1200900
	HEX 0DC80C0022BE0A0CBC0019010F0010020381020D04081100BE01C501A601CA01
	HEX BE019A01AD01C8011A01103F00081900034114021100020A07030508B210B710
	HEX B510B710B918AB10A910A80810041100A608A910AB08A910A908A808100812FF
	HEX 00011E0D900C01020C03810F03CA010341A90503810204C8010341A804038102
	HEX 04C5010341A9040F01020D0DC80C0024A610A6109A1024


C.TITLEM	HEX 4D002C02000117C10765011F82D019021D28020E0AEE0D2C0C010341040815D2
	HEX 0E3C1100140023500300235003001405235003002361030014FE235003002350
	HEX 03001400235003002361030010FF00011E00018220190102CF0D640C0003410F
	HEX 030311D1C00341040815D20EFF020D0F011100A601A401A1019A019801950110
	HEX 28040815D20EFA02080A8A0F010B2816021300076514000D2C0C011100237203
	HEX 00107E18F00300820982020764820982020311099222C3BCB0090022C1C35809
	HEX 5422C3C19822C10900C31822BCC140090022C3BC1822C1C358090022B003130D
	HEX 500C00BC5603110D580C02AE01BB010B3C032102090A0A1100140023FB030000
	HEX 04190023FB030023FB030023FB03000010190023FB03000004190023FB030023
	HEX FB030023FB03000010190014F923FB03000004190023FB030023FB030023FB03
	HEX 000010190023FB03000004190023FB030023FB030023FB03000010190014FE23
	HEX FB03000004190023FB030023FB030023FB03000010190023FB03000004190023
	HEX FB030023FB030023FB030000101900140523FB03000004190023FB030023FB03
	HEX 0023FB030000101900140C23FB03000004190023FB030023FB030023FB030000
	HEX 1019001400100407650A8A110023720300107E030018F0820982020764820982
	HEX 0203110D2C0C011100099222C3BCB0090022C1C358095422C3C19822C10900C3
	HEX 1822BCC140090022C3BC1822C1C358090022B003130D500C00BC5603110D580C
	HEX 02140CAE01BB010B3C1002120300011E000182E01901020E0A0D03811100B001
	HEX AF01AE01AF0110EB00031900020815500E9B13000A3A04030705050411000341
	HEX 140023B50300140523B50300000B1900B006B005B50BB50BB70BBE0BC30BC10B
	HEX C30BC10BBC0BBE0B000B1900BC0BB50BB70B1400000B1900B006B505B70BB50B
	HEX BC0BB90BBC0BC12100201900032123FB030023FB030010030311020D0A0A1100
	HEX 231C04001AFF101F000B190002DE1100231C04001A01102002080700040F0A5A
	HEX 03411100140023DE030023DE030014FE23DE0300140023DE03001002031114F4
	HEX 020D0A0A1100231C04001A011040020812031400020CB0C019020208090B0317
	HEX 0702050413001100B50BB00BBC0BB50BC10BB00BB50BBC0B10021BFF120F0001
	HEX 1E02070A300381D601034102060AA8249D0BA90B980BA90B9D0BA90B980BA90B
	HEX 249D0BA90B980BA90B9F0B9D0B9A0B980B2418C8000119001896000119001864
	HEX 00011900185000011900184600011900183C0001190018320001190018280001
	HEX 1900181E00011900181400011900180A0001190024000B1900B006B005B50BB5
	HEX 0BB70BB90BBE0BBC0B000B1900BE0BC10BBE06BC05BE0BBC0BB50BB70B24000B
	HEX 1900B00BB20BB00BB50BB20BB70BB50BB90BB716B516B016AD0B24BC01B901B5
	HEX 01B001B501B901BC01B901B501B001B501B901B501B001AD01A40224BC01C101
	HEX C301C801CD01C801C301C101BC01B901BC0124


C.TIMERM	HEX 0900120000010000190000011E00010000190000011E0001020A0341040815D2
	HEX 0EA517C4011F18FF1D2807660D640C000F0109071100235D0000A40C235D0000
	HEX A906235D0000AB05AE06235D0000AB04235D0000A90C1A01101000011E0311C8
	HEX 01034124

C.INGAMEM2	HEX 0900120000010000190000011E00010000190000011E0001140217C40766011F
	HEX 2501020C0A0A0341040815D20E500D900C0118F51D50110023700200229A8E06
	HEX 1004110023700200237002001004110023110200AD0500061900237002002370
	HEX 020023110200AD05237002002370020023700200100211000F00238B01009F0C
	HEX 238B0100000C1900238B01009F0C238B0100237002002370020010010F011300
	HEX 110023BE01009F0C23BE01002370020023700200100323BE01009F0C110014FD
	HEX 2370020014FE23700200237B0200237002001004140212021100140523BE0100
	HEX 9F0C140023BE01002298237002002370020010021402110023BE01009F0C23BE
	HEX 01002370020023700200100323BE01009F0C110014FD2370020014FE23700200
	HEX 237B020023700200100414021202228EA6C00F0003112344030003419D0C9D0C
	HEX 9F0C9F0C03212344030003419D0C9D0C9F0C9F0C110023860200238602002386
	HEX 020023E102001002110014FD2370020014FE23700200237B0200237002001004
	HEX 140212200000190000011E0311BC0103419A050311BE0103419A059A0C9D0C03
	HEX 11BE0103419D050311C10103419F059F0C0311BE0103419F059F069D0C24229A
	HEX 0311BC010341A9050311BE0103419A050381BE0103419A0B229D0381C5010341
	HEX AD0B0311BE0103419D05229F0311C1010341A90B0311BE010341A9050381BE01
	HEX 03419D0B229D0381C5010341AD0B229F24229A0311BC010341A9050311BE0103
	HEX 419A050381BE010341000B1900229D0381C5010341AD05000619000311BE0103
	HEX 4100051900229F0311000119000341A905000619000311BE010341A9050381BE
	HEX 010341000B1900229D0381C5010341240F010381C5010341AD05240F010321C5
	HEX 010341AD05240F01229A0381BE0103410DB40C00A40B0D900C0122AD0321AB06
	HEX 034122A6A606229A0381BE010341A60522A6A60C22A4A406229D0381BE010341
	HEX A60B032122ADAB0C034122A60381BE010341A605032122ABAD060341229F9F0C
	HEX 240F01229A0381BE0103410DB40C00A40B0D900C01032122ADAB06034122A6A6
	HEX 06229A0381BE010341A60522A6A60C229D0381BE010341A9050381BE010341A9
	HEX 0B0381BE010341A905229F0381BE010341A9050381BE010341A90B0381BE0103
	HEX 41A90B2411009A0C00061900A60600061900A60CA4069D0C000C1900A6060006
	HEX 19009F0C10039A0C0024190024


C.ENDLEVELM2
	HEX 0900120000010000190000011E00010000190000011E0001076617C418FE1D0A
	HEX 011F2501020D04080E4B15D20D900C010F0113002295110023F70000A6071A01
	HEX 100814001100229823F70000A60723F70000980723FE0000A90723F700009807
	HEX 23F70000A60723F70000980723FE0000A90323F70000980323FE0000A607229D
	HEX 23F70000A60723F700009D0723FE0000A90723F700009D07229B23F70000A607
	HEX 23F70000A60723FE0000A90323F700009F0323FE0000A6071007229823FE0000
	HEX A90723F70000980723FE0000A90723F70000980723FE0000A90723FE0000A907
	HEX 23FE0000A90323FE0000A60323FE0000A90712FF00011E0381C5010341240381
	HEX CA01034124

C.INGAMEM3	HEX 0F0018000381C501034100010000190000011E00010000190000011E000117C4
	HEX 0766011F2501020A0A0A0341040815D20E500D900C0118FA1D1E1100233A0200
	HEX 1004130014000D900C0109000F011100229AA6089A0822A6A608229AA608AB08
	HEX 22A4A408229AA6089A0822A1A608229A9A089A08229FA608229AAB089A08229D
	HEX A60822989808100411001400237F0100237F01001403237F01001402237F0100
	HEX 100409080F001400233A02001403233A02001405233A02001407233A02000F01
	HEX 09060DC80C00140C235B02001407235B02001405235B02001407235B0200140C
	HEX 235B02001407235B02001405235B02001403235B020014000F00235B0200233A
	HEX 0200090011000D900C01235B0200235B02001403235B020014009D089F080381
	HEX C5010341A407A610001819009D089F080381C5010341A407A610AB08A9101002
	HEX 0DC00C5D0F0209041100140C237F01001407237F010010020DD00C0709061100
	HEX 1400237F01001403237F010010031405237F01001407237F010012FF00011E22
	HEX 9A0381C5010341A6070381C50103419A07034122A60381B9010341A607229A03
	HEX 81C50103419A070381C5010341AB010381C5010341A6019F0422A40381C50103
	HEX 41A407229A0381B9010341A6070381C50103419A0722A10381C5010341A60703
	HEX 11B201B901B201B901B2010321BE01B9010381229A0381B90103419F07229F03
	HEX 81C5010341A6070381C5010341AB010381C5010341A601229A9F04229A0381C5
	HEX 0103419A070381229DB9010341A6070381B901034122989807249A089A08A608
	HEX 9A089A08A4089A089A08A1089A089A089F089A089A089D089808240311BE0103
	HEX 41A9079A08A6089A08A4089A080311BE010341A1070311BE0103419F070311BE
	HEX 0103419D0798089F089A089D089F080311BE010341A4070311BE010341A60724

C.ENDLEVELM3
	HEX 0900120000010000190000011E00010000190000011E0001076617C418FA1D0F
	HEX 011F2501020A0A0A04080E3715D20D900C010F012298110023CA0000AD071AFF
	HEX 1008140013001100229A23C30000A40723CA00009A0723C30000A907229A23CA
	HEX 00009A071003229D23C30000A40723CA00009D07229823C30000A90723CA0000
	HEX 98071100229A23C30000A40723CA00009A0723C30000A90723CA00009A039A04
	HEX 1003229D23C30000A40723CA0000AB07229F23C30000A90723C30000A9071220
	HEX 00011E0381BE010341240381CA01034124


C.TMUSIC1	HEX 9500E400000117C118FA1D1E011F2503020C0A4C034115D204080E500DE80C03
	HEX 0F01229A13000D900C0118F0110023A3010023BA010023A3010023CD0100100C
	HEX 0F0011009A029A0403119F02A104A1029F049D04980403419502980410021100
	HEX 9A029A0403219F02A104A1029F049D049804034195029804100211009A029A04
	HEX 9F02A104A1029F049D0498049502980410040F0112FF00011E00018200190102
	HEX 0A0341040F0EFF15B413001100B238AD0823F70100B038AB0823F70100B520B4
	HEX 04B508B404B508B40823F70100AD2CAB04AD08B00823F70100110023F7010010
	HEX 08140C12FF00011E000115D20EFF0408070302080A881300110000081900230E
	HEX 0200000E1900230E020000041900230E020010040341070505040402110023E0
	HEX 0100BC02BC02B902BE02230E0200BE04230E020023E00100BC02B902B502B202
	HEX 230E02000321CA02CC01CD010381C5010321D1010341100A11000341B202B202
	HEX BE02B202B202BE02B202B202BE02B202B202C102B202C102B202B202B202B202
	HEX BE02B202B202BE02B202B202BE02B202B202BC02B202B902BC02B202100412FF
	HEX 00011E229AA904A90222A6A602229DB00422A4A60222989A0224229AA90422A4
	HEX A402229D9D02B0049D02B00224229AA90422A4A402229898022295B006B00224
	HEX BE02BE02B202BE0207000381C50403410705B202B902240A0AA6280703BC04BE
	HEX 04BE04B904BC04BE0407000A8A240381C502034124


C.LCOMPM	HEX 7700E60000012502020C0A2D0341040C0EFF15D217C10764011F1F0C1300183C
	HEX 1D551100B204B204B908B708B404B208B204B708B508B208B004B218B508B218
	HEX B00410021D01185003411100B210B010AD10A6101003B210B510B710B9101204
	HEX 0A0D1100B940B740B540B24010021100001019001BFF100F00011E000102080A
	HEX 4A031104000E5515D21F0C08010D540C0B140C02090A08130011000353BE040F
	HEX 030317CA040F0003110801B504B00408000321B20403110801B504B204B00408
	HEX 001008080011000311B90423D3010023D30100B50423D3010023D30100B70423
	HEX D301001008122000011E000102090A6A0341040815D20E410D2C0C01229A1300
	HEX 11000DC80C000F012298A6040D2C0C010F009A0423DE01009D040F01A6040F00
	HEX 9A0423DE01009D040F01229DA9040F009F0423DE01009A040F01229FA6040F00
	HEX 9F0423DE010023DE010010030D960C000F012298A6040D2C0C010F009A0423DE
	HEX 01009D040F01A60423DE010023DE01009D040F01229D23DE01000F009F0423DE
	HEX 010023DE01009F0423DE010023DE010023DE01001220110023EB01009A029A04
	HEX 23DE01009D0423EB01009A0623DE01009D0423EB01009D029F0423DE01009A04
	HEX 23EB01009F0623DE010023DE010010FF00011E0F030317CA0403110F00240F01
	HEX 0381C5010341AD030F00240F03A6020F0024

C.TMUSIC2	HEX 95001602000117C118F51D14011F2501020E0AAE04080E3C15D2034114020F00
	HEX 950C980C990C110013009A0C000C1900950C000C190012039A0C950C980C990C
	HEX 100213000DF40C0111009A0CA10C950CA60C9A0CA10C950CA40C9A0CA10C950C
	HEX A60C9A0C950C980C990C10020F011100237B03001007230A04000F01110014F6
	HEX 237B0300100414021100237B03001004230A0400122000011E000102080A0803
	HEX 410DD00C07090522BE140215D20EFF05062315030004089A7419010A68233004
	HEX 00000619001100000C1900130023300400034100121900120714052330040023
	HEX 3004001402100213001100BE0C233004000006190023210400BE062330040023
	HEX 210400BE0C23300400B90623210400BC0623300400BD06BE0C23300400000619
	HEX 0023210400BE062330040023210400BE0C23300400B90623210400BC06140523
	HEX 300400233004001402BE0C233004000006190023210400BE0623300400232104
	HEX 00BE0C23300400B90623210400BC0623300400BD06BE0C233004002330040023
	HEX 210400BE062330040023210400C10C23210400C00623210400BC062321040023
	HEX 210400100707000341020C0A089AC00A680208AD60B0601100B2C0B560B76010
	HEX 021100B218BE06B20CBE06B20CBE06B212BE0CB218BE06B20CBE06B20CC106B2
	HEX 12C30CB518BE06B50CBE06B50CBE06B512BE0CB718BE06B70CBE06B70CC106B7
	HEX 12C30C10040E3C1100A6C0A160A448A51810040EFF122000011E000102080A8A
	HEX 0341040F15D20EFA0D2C0C011402050623150300130000801901070204001100
	HEX BE0600061900B20600061900B006B50600061900B506B20600061900233F0400
	HEX 10041100B90CB20600061900BC0C00061900B506BE0C233F0400B90CB2060006
	HEX 1900BE0C00061900B506BC0C233F040010040700020623280300232803001100
	HEX 0408B00CB218B00CB230B50CB20CB00CAD0CA60C001819000F01040F22B2AD0C
	HEX 232803000408AD0CB018AD0CB030AD0CB00CB50CB40CB20C001819000F01040F
	HEX 22B2AD0C232803001002040800001903110023280300232803002328030014F6
	HEX 2328030014021002040F0700120200DC190200011E0E41020D0408950C980C99
	HEX 0C040F0EFF0206240F0122B2B2060012190022B5B5060006190022B2B206000C
	HEX 190022B5B20C00061900B20C22B7B50C22B2B2060012190022B5B50600061900
	HEX 22B2B206000C190022B5B20C0006190022B7B50C22B5B70C0F0024229AA90C22
	HEX A1A10C2295AD0C22A6A606AD06229AA90C22A1A10C2295AD0C22A4A40C229AA9
	HEX 0C22A1A10C2295AD0C22A6A606AD06229AA90C2295950C2298AD0C2299990C22
	HEX 9AA90C22A1A10C2295AD0C22A6A606AD06229AA90C22A1A10C2295AD0C22A4A4
	HEX 0C229AA90C22A1A10C2295AD0C22A6A606AD06229AA9069A062295AD06950622
	HEX 98AD0C2299AD06AD06240A0E18281D030F01229AAD8019010F0018F51D140AAE
	HEX 24070003810F01BE060F00031707032403810700C5030351BE030703031724B2
	HEX 06B00600061900B006B1060006190024

C.TMUSIC3	HEX 55004001000117C10764011F18641D0125010A0D040815D20E3C0317020EBE40
	HEX 1902034118FF0DFA0C000F0123BC030023BC030011000DFA0C0023BC030023BC
	HEX 030023BC030014050DC20C0123BC03001400102010FF00011E000102099AC111
	HEX 000381C8010311BE01C501CA01C101C801237B0300000C190023700300000619
	HEX 0023700300000619002361030000061900000619000006190023700300237B03
	HEX 000006190023450300000519001004130011000381C8010311BE01C501CA01C1
	HEX 01C801237B0300234E0300234E030023700300B20623700300234E0300236103
	HEX 00000619001405234E0300234E0300140023700300237B03001405234E030014
	HEX 0023450300BC05101C110014F42388030014F623880300237003002388030023
	HEX 70030014F92388030014F623880300237003002388030000061900102C140012
	HEX FF00011E00010341040815D20EFF020D0F011100AD01A401A1019A0198019501
	HEX 10601601032104001300020B0A4A070505080F030D140C00093000741901B00C
	HEX 1100239D0300BE06C10CBC0CBE6022821F0C23F3030014002388030014022388
	HEX 0300140523880300140023880300239D0300C106C50CC10CC36022DD23F30300
	HEX 1405238803001405238803001402238803001400238803001002140223880300
	HEX 14000034190203111F000BFF070002EE0F010DAA0C0022BE09AAC38019010B00
	HEX 070505060209034100C019001100B00CB20CB20C00301900B20CB00CB20C000C
	HEX 1900B20C000C1900B20C000C1900B20C10041100B00CB20CB20CB518B718B20C
	HEX B00CB20CBC0CB20CB90CB20CB70CB20CB00CB20CB20CB718B518B20CB00CB20C
	HEX C10CB20CBC0CB206B206BE0CB206B20610021100B206B206B506B206B206B706
	HEX B206B206B906B206B206BC06B206B206BE06B206B206BE06B206B206BE06B206
	HEX B206BE06B206BC06BE06B206C106B206BE06C106B706C306B706C506B706C306
	HEX C506B706C506B706C306C506B706C506C806B706B706CA06B706C806CA06B706
	HEX C806C506CA06B706C806B706C506B706C806C50603511100140E238803001407
	HEX 23880300140223880300100AB00C238803001100140023880300140223880300
	HEX B00C2388030010101400020E0A0FB20019031202031307001100B224BE0C1BFF
	HEX 100F00011E0381CA0103110F00240F010381CA0103210F00B201B901BE01B202
	HEX 240F010381C8010311020DA6050209240F010381C5020311AD04240311CA01C5
	HEX 01CA01D101CA0224BC01C301C801CF01C802C301BC01C101C101BC02240341B2
	HEX 0CB70CB90CBC0CBE0CBC0CBE0CBC06B912B90CBE0CBC0CBE0CB90624229AA60C
	HEX 9A0C229DA90CA90C229FA60C9F0C229DA90C9D0C229AA60C9A0C229DA90CA60C
	HEX 229FA60C9F0C22A1A90C22A4A406229F9F06240F0102CA08010311070009000D
	HEX C80C00BE3009300D140C0007050800020B0F03031124




****************************************
*      music continues under ROM	*
****************************************

	ORG	$E000


C.GC.MUSIC	HEX 7D003C010001250117C1011F02090A090D2C0C0103110606038107641D051855
	HEX 0E50187D1100D6040008190010400A09140C0700060000801901130011000351
	HEX 020DA90C2364030018910381D604D10400041900187D23640300100211000351
	HEX 020DAB0C2364030018910381D604D104187D0004190023640300100212FF0001
	HEX 1E000102AA15D20ECD040013000A690B3C16020311140C1100B50CB00CB70CB0
	HEX 0CB50CB70C00181900B50CB00CB70CB00CB50CB70C00181900B50CB00CB70CB0
	HEX 0CB50CB70CB50CB918B718B518B018B20C10021300020A0E3203410702050822
	HEX B00B00160014001100B00CB00CB50CB70CB006B506B706B90CB706B50CB20CB7
	HEX 18B50CB90CB70CB50CAD0C10040705050003410341070002090A77140015D20E
	HEX FF1100B548BE18BC54BE0CB948B518B030B7301004B56000A0190212FF00011E
	HEX 0001200C15D20E3C0A680DC20C011100234F03009D0223350300000219002335
	HEX 030000021900230D03000006190023350300000519002335030000051900230D
	HEX 030023400300234F03009D0223400300230D0300230D03002335030000051900
	HEX 2335030000051900230D0300233503000002190023350300000219001007234F
	HEX 03009D02002A19001402234F030000081900234F030000081900234F03009D02
	HEX 234F03009D02234F03009D0814001100100413001100234F03009D0223350300
	HEX 9D05230D03009D06234F03009802233503009805230D030023400300234F0300
	HEX 9D0223400300230D0300230D0300234F03009802233503009805230D03002335
	HEX 03009805234F03009F02233503009F05230D03009F06234F0300980223350300
	HEX 9805230D030023400300234F03009F0223400300230D0300230D0300234F0300
	HEX 9802233503009A05234F03009C02233503009C051003234F03009D0223350300
	HEX 9D05230D03009D06234F03009802233503009805230D030023400300234F0300
	HEX 9D0223400300230D0300230D0300234F03009802233503009805230D03002335
	HEX 03009805234F03009F081407000C19001100234003001AFE10081400234F0300
	HEX 9808234F03009A0812FF00011E034102090A09BC01C1010321B501B0010351B0
	HEX 02034102060AAA24031302090A89C10602060AAA24020A0313D4010341020624
	HEX 0311C801BC01B501C101BC020341240408020C0381D1010341A901A401A10102
	HEX 06040624038102090A09C50C020D035124B530B030B730B030B518B018B724B5
	HEX 06B706B930B73024

C.INTROM	HEX 9100C20200012501010F82041300020C0A8C03170E3215D204080D0A0C000F02
	HEX 14020A0C0D780C050341140223870300238703000DF40C012387030023870300
	HEX 140523870300140023870300140223870300238703000DF40C01238703002387
	HEX 0300140523870300140023870300140223870300229A020A11000381C5010341
	HEX A90710030381C5010341A96812020000190000011E000182041402020A0A0A03
	HEX 1115D204000E9611002310040000081900231004000008190010081100231004
	HEX 0023100400232E0400231004002310040023100400232E040023230400232304
	HEX 002310040023100400232E0400231004002310040023100400232E0400231004
	HEX 00100203110ADD070505041405110023550400B20723550400B70723550400B9
	HEX 071005B2081400110023550400B20723550400B70723550400B9071005B20807
	HEX 0014021100233F0400233F0400232E0400233F0400234A0400233F0400232E04
	HEX 002323040023230400233F0400233F0400232E0400233F04001405233F040014
	HEX 02233F0400232E04001405233F040014021004034103110ADD07050504140511
	HEX 0023550400B20723550400B70723550400B9071005B2081400110023550400B2
	HEX 0723550400B70723550400B9071005B208070014020A09A640B210B008AD10B0
	HEX 08A410020CA6800341070505040D2C0C010A8A14021100A60800081900235C04
	HEX 00A6080008190000081900A61010041100A608A608235C0400A608A608000819
	HEX 00A608A608100414051100A608AB08235C0400AD08A608AB08AD1010021400A6
	HEX 08AB08235C0400AD08A6082375040023750400AD08A608110023750400100714
	HEX 021100A608B208235C0400A608A608B208235C0400B208100814051100A608B2
	HEX 08235C0400A608A608B208235C0400B208100214001100A608B208235C0400A6
	HEX 08A608B208235C0400B208100214020A09A640B210B008AD10B008A410020CA6
	HEX 0019FA00011E000182041300020A0A8D031715D20E41040F14020D0A0C000F03
	HEX 0A0D03410D2C0C010F01229A090C1100A60800081900A60800101900A6080010
	HEX 1900A60800101900A60800101900A910100402FF14051100BE01B901B201B901
	HEX 102014001100BE01B901B201B9011020020A14021100A608B208A608B010A608
	HEX B210A60800101900A60800101900A910A608B208A608B510A608B210A6080010
	HEX 1900A60800101900A91010020EFF020C0AAA0F00B580B0800A0AB20019011202
	HEX 0000190000011E229A0F010381C5010341A9070381C50103419A070381C50103
	HEX 41AD07229D0381C5010341A90F229F0381C5010341A907229D0381C5010341AD
	HEX 0722980381C50103419807229A0F010381C5010341A90F0381C5010341AD0722
	HEX 9D0381C5010341A90F229F0381C5010341A907229D0381C5010341AD070381C5
	HEX 0103412298AD030381C5010341AD03240381CA010311CA0100021900CA010003
	HEX 1900240381CA010311CA02BE01240381CA010311CA010381CA02B902C5022403
	HEX 81CA010311CA01BE06240381CA010311CA01BC06240381CA0103412407000381
	HEX CA010311CA010381C802C3020311BE020341070524040807000F010381CA0103
	HEX 41B207034107050F00040024

C.SHOPM	HEX 3B017602000117C10764011F250118FA1D238238020A0AFA0341040815D20E41
	HEX 0D2C0C0114FE11009A0E001C19009F0E000E19009F0E9D0E9F0E9A0E001C1900
	HEX 9F0E000E19009F0E9D0E9C0E10020F0113001100229A23170300A60D23170300
	HEX A6062325030000051900231E0300A9062325030000051900229F232503009F0C
	HEX 232503000005190023170300A60623170300A60D229D231E0300A90D229F2325
	HEX 03009F0523170300A606229A23170300A60D23170300A6062325030000051900
	HEX 231E0300A9062325030000051900229F232503009F0C23250300000519002317
	HEX 0300A60623170300A60D229D231E0300A90D229C232503009C0523250300A605
	HEX 1007229A23170300A60D00701900233003000007190023300300000719000311
	HEX B20E233003000311B20E23170300B20623170300B2061202140012FF00011E00
	HEX 0182F8190102090A680341040215D20E4B0DF40C01090514FE1100A607004D19
	HEX 00AB0700071900AB0700071900A607006919001002110023470300BE07BE07C5
	HEX 07BC07BE0700151900C307C307235A0300C307235A0300C30723470300BE07BE
	HEX 07C507BC07BE07000E1900C307C30700071900C30700071900C3070007190010
	HEX 0500E019001C0C22C809FF13001100234703000802BE07C10722CAC507000719
	HEX 00C50722C8C307BE07C107000E19000800235A030000071900235A0300B20723
	HEX 470300080222CAC107C50722CFC80700071900C80722CAC507C10722C8C307C1
	HEX 07BE0708000351AB07AD07B007B207100700E019001400032705070703120213
	HEX 001100B20EB50EB70EB907B00EB007B20EB50E0700B70E0703100E00E0190003
	HEX 471202070020400EFF23690300007019000A592369030000011E0001040915D2
	HEX 0E4B14FE0D900C01035102DF1100CA01A601BE01B201C501AD01B90110B5003F
	HEX 190002090A990B19160323300300000719002330030000071900233003002330
	HEX 0300031113000F002369030000381900038102EE0A0DBE3802090A99003A1903
	HEX 23300300000719002330030000071900233003002330030003110A5903410EFF
	HEX 120214F40B0011000A592369030000701900102000011E0381C5010341240381
	HEX D1010341240381D6010347BE010341240F010381CA010341B0020381C5010341
	HEX AD03000719002407000F000341A6070007190003510F0107032407000F000341
	HEX AB0703510F0107032402090F00C19ABE0EC11CC31CBEB6B90EBC0EBE0EC18CC3
	HEX 1CC11CC01C0A09BE7024

C.DCOMPM	HEX 8700CE00000117C10764011F2501020F0A0A0408034115D20E3C18FF1D030D90
	HEX 0C010DFA0C000F01130011002281A60E000E1900AB0700231900A60EAB070015
	HEX 19002286A60EA60EAB0700231900A60EAB07001519002281A60E000E1900AB07
	HEX 00231900A60EAB07001519002286A60EA60EAB0700231900A60EAB0700071900
	HEX AB070007190010FF00011E000102280A660341040015E80E6413001100140023
	HEX 6E0100A515A50E14F9236E0100A615A70E1400236E0100A515A50E14F9236E01
	HEX 00A615000E1900100600C0190112FF00011E000102050A4A038113001100CA01
	HEX 000D1900CA01000D19000208C50A020500041900CA0100061900C50400031900
	HEX CA0100061900C50400031900CA01000D19000208C50A020500041900CA01000D
	HEX 19001003CA01000D1900CA01000D19000208C50A020500041900CA0100061900
	HEX C50400031900CA0100061900C50400031900CA01000D19000208C50A00041900
	HEX C50A00041900020512FF00011E24000E1900A507A407A507A407A507A407A515
	HEX 24

C.ENDMUSIC










;
;  PDS Pc1.21 :000: (c) P.D.Systems Ltd 1985-88
;
;
;       File  "CYBERDYNE MAP : 0.5"
;    Version          11.16
;         By  "WHO ELSE?  ITS JAZ!"
;
;     Created on Sat the 14th of May 1988
;        Last update 00:24 on 01/01/80
;

	ORG	ENDCODEA


****************************************
*	demo setup code		*
****************************************

;added variables...

UNI1	DB	0
UNI2	DB	0
UNI3	DB	0
UNI4	DB	0
UNI5	DB	0
UNI7	DB	0

BTAB255	HEX	040810204080
;	HEX	0102040810204080

T.RT	DB	0
DEMOEOR	DB	0
DEMOLETO	DB	0

TEXTF	DB	0	;0:no stars, 1:stars

;T.LCOMP
;	JSR B35
;	JSR SCREENOFF
;	JSR MCOLON
;	JSR BLACK
;	JSR ZEROVARS
;	STA TJ+3
;	STA VOL
;	STA $D01D
;	STA $D017
;	STA $7FFF
;	STA TEXTF
;	LDX #$FF
;	LDA #1
;	JSR CLS
;	JSR T.DATA2
;	JSR DANCESWAP
;	LDX #11
;	JSR DATASWAP
;	LDA #$1B
;	STA $D018
;
;	LDX #6
;	STX BOFF
;	LDA #70
;	STA SMILECNT
;	LDX #5
;	JSR SPRDATA


STARSET	LDY #10	;14
	LDX #5	;7
!1	LDA DEMOSX,X
	STA $D004,Y
	LDA DEMOSY,X
	STA $D005,Y
	LDA #$0F
	STA $47FA,X
	LDA #$0C
	STA $D029,X
	DEY
	DEY
	DEX
	BPL !1
	LDA #1
	STA TEXTF
	RTS

;	LDX #63
;!STARS	LDA #0
;	STA $4000,X
;	DEX
;	BPL !STARS
;	LDX #$D5
;!S2	LDA DEMOSPRS-1,X
;	STA $7D80-1,X
;	LDA DEMOSPRS+$D4,X
;	STA $7E54,X
;	LDA DEMOSPRS+$1A9,X
;	STA $7F29,X
;	DEX
;	BNE !S2
;
;	JSR SNOWPRINT
;	LDA #$6A
;	STA TA+15
;

;	JSR SCREENON
;
;	JSR MRESET
;	LDX #<DMUSIC
;	LDY #>DMUSIC
;	JSR MUSICSET
;
;	JSR EA7ESET
;	LDX #>TLC1
;	LDY #<TLC1
;	LDA #$36
;	CLC
;	JSR RCONT2
;
;
;TLCLOOP	LDA CODEFLAG
;	BEQ TLCLOOP
;	DEC CODEFLAG
;
;	LDA JOY1
;	AND #$10
;	BNE !ND
;;	JMP DOWNLOAD
;
;!ND	;LDA JOY2
;	;AND #$10
;	;BNE !NF
;	;JMP T.DONETS
;
;!NF	JSR SNOWFALL

;	JSR TEXTPRINT

;	JSR MUSICPLY
;	JMP TLCLOOP



****************************************
*	demo rasters		*
****************************************


TLC1
;	JSR BLACK
	LDA #0
	STA T.RT
;	LDA FC
;	BEQ !E3
;	LDA #$FF
;	STA SMILEOFF
;	JSR BLINK
;	LDX #2
;	SEC
;	JSR ROLLRASTS
;!E3
	;JSR DEMOSMODY
	LDA DEMOSOX
	CMP #DTV/2
	BNE !NOLET
	LDA DEMOEOR
	EOR #1
	STA DEMOEOR
	BEQ !NOLET
	JSR DEMOLET
!NOLET	LDY #10	;14
	LDX #5	;7
!1	LDA DEMOSX,X
	STA $D004,Y
	LDA DEMOSY,X
	STA $D005,Y
	LDA DEMOANIS,X
	STA $47FA,X
	DEY
	DEY
	DEX
	BPL !1
	LDA DEMO15
	STA $D015
	RTS

;	LDA #$2F
;	LDX #>TLC2
;	LDY #<TLC2
;	RTS
;
;TLC2	LDA #11
;	STA BDR
;	STA SCR
;	JSR DELAY+17
;	INC BDR
;	INC SCR
;	JSR DELAY+16
;	LDA #15
;	STA TLCCOLX+1
;	JSR BLACK2
;	JSR TLCCOL
;	LDA #$43
;	LDX #>TLC3
;	LDY #<TLC3
;	RTS

TLC3
;	JSR TLCCOL	;1st colour split (under YIPEE)
	CLC
	LDA DEMORY
	ADC #8
	STA TLCSPRP+1
	LDX #>TLCA
	LDY #<TLCA
	RTS

TLCA	JSR TLCSPR
;	LDA #$53
;	LDX #>TLCB
;	LDY #<TLCB
;	RTS
;
;TLCB
;	JSR TLCCOL	;2
	LDA TLCSPRP+1
	LDX #>TLCC
	LDY #<TLCC
	RTS

TLCC	JSR TLCSPR
;	LDA #$65
;	LDX #>TLCD
;	LDY #<TLCD
;	RTS
;
;TLCD
;	JSR TLCCOL	;3
	LDA TLCSPRP+1
	LDX #>TLCE
	LDY #<TLCE
	RTS

TLCE	JSR TLCSPR
;	JSR TLCCOL	;4
;	LDA #$82
;	LDX #>TLCF
;	LDY #<TLCF
;	RTS
;
;TLCF
;	JSR TLCCOL	;5
	LDA TLCSPRP+1
	LDX #>TLCG
	LDY #<TLCG
	RTS

TLCG	JSR TLCSPR
;	LDA #$95
;	LDX #>TLCH
;	LDY #<TLCH
;	RTS
;
;TLCH
;	JSR TLCCOL	;6
;	LDA TLCSPRP+1
;	LDX #>TLCI
;	LDY #<TLCI
;	RTS
;
;TLCI	JSR TLCSPR
;	LDA #$A5
;	LDX #>TLCJ
;	LDY #<TLCJ
;	RTS
;
;TLCJ
;	JSR TLCCOL	;7
;	LDA TLCSPRP+1

;	LDX #>TLCK
;	LDY #<TLCK
;	RTS
;
;TLCK	JSR TLCSPR
;;	JSR TLCCOL	;8
;;	LDA #$C2
;;	LDX #>TLCL
;;	LDY #<TLCL
;;	RTS
;;
;;TLCL
;;	JSR TLCCOL	;9
;	LDA TLCSPRP+1
;	LDX #>TLCM
;	LDY #<TLCM
;	RTS
;
;TLCM	JSR TLCSPR

	JMP SRAST2B


;	LDA #$D5
;	LDX #>TLCN
;	LDY #<TLCN
;	RTS
;
;TLCN
;	JSR TLCCOL	;10
;	LDA #$E8
;	LDX #>TLCO
;	LDY #<TLCO
;	RTS
;
;TLCO
;	JSR TLCCOL	;11
;	LDA #$FA
;	LDX #>TLC9
;	LDY #<TLC9
;	RTS
;
;TLC9
;	LDA #12
;	STA BDR
;	STA SCR
;	JSR DELAY+17
;	DEC BDR
;	DEC SCR
;	JSR DELAY+15
;	LDA #0
;	JSR BLACK2
;;	JSR NOBORDERS
;	INC CODEFLAG
;	JSR FCEOR
;	LDA THCOLS
;	STA SMC1
;	LDA THCOLS+2
;	STA SMC2

;	LDA #0
;	LDX #>TLC1
;	LDY #<TLC1
;	RTS


;TLCCOL	;INC SCR
;TLCCOLX	LDX #15
;	;LDA MC1TAB,X
;	LDA THCOLS,X
;	STA MC1
;	LDA THCOLS-1,X
;	;LDA MC2TAB,X
;	STA MC2
;	DEX
;	;BPL !X
;	;LDX #15
;!X	STX TLCCOLX+1
;	;DEC SCR
;	RTS

TLCSPR	;DEC SCR
	LDX #10	;14
!1	CLC
	LDA $D005,X
	ADC #21
	STA $D005,X
	DEX
	DEX
	BPL !1
	;INC SCR
	LDX T.RT
	LDA DEMO15+1,X
	STA $D015
	INX
	;CPX #7
	;BCS TLC3E
	STX T.RT
	CLC
TLCSPRP	LDA #$FF
	ADC #21
	STA TLCSPRP+1
	;LDX #>TLC3
	;LDY #<TLC3
	RTS


UNI	DEC UNI5
	BPL !NU5
	LDA #5
	STA UNI5
!NU5	DEC UNI4
	BPL !NU4
	LDA #4
	STA UNI4
!NU4	DEC UNI3
	BPL !NU3
	LDA #3
	STA UNI3
!NU3	DEC UNI2
	BPL !NU2
	LDA #2
	STA UNI2
!NU2	RTS




****************************************
*	DEMO STUFF		*
****************************************


DEMOANIS	HEX	4D 4D 4D 4D 4D 4D ;00 00
DEMOANISD	HEX	21 22 23 24 25 26 ;27 28
DEMOCOLS	HEX	03 03 03 03 03 03

DEMOSX	HEX	70 88 A0 B8 D0 E8
	;HEX	58 70 88 A0 B8 D0 E8 00	;X pos
DEMOSY	DS	6,$54				;Y pos
DEMORY	HEX	54				;rast pos

DEMOSOX	DB	DTV,DTV,DTV,DTV,DTV,DTV ;,DTV,DTV				;X offset
DEMOSDY	HEX	E7 E6 E5 E4 E3 E2
	;HEX	E8 E7 E6 E5 E4 E3 E2 E1 ;Y delay
DEMOSOY	DS	6,0				;Y offset

DEMO15	DS	6,255	;8,255

DEMOXD	DB	30

DEMOSMODX	LDA TEXTF
	BNE !1
!NO	RTS
!1	LDA DEMOXD
	BEQ !YES
	LDA UNI5
	BNE !NO
	DEC DEMOXD
	RTS
!YES	LDY DEMOSOX	;,X
	DEY
	BPL !Y
	LDY #DTV
!Y	TYA
	STA DEMOSOX	;,X
	LDX #5		;7
!L	LDA DEMOSXL,X
	STA !D+1
	LDA DEMOSXH,X
	STA !D+2
!D	LDA $FFFF,Y
	BMI !LEFT
	CLC
	ADC DEMOSX,X
	STA DEMOSX,X
	BCC !OK
!255	LDA $D010
	EOR BTAB255,X
	STA $D010
!OK	DEX
	BPL !L
	BMI !SWAPYN
!LEFT	;JMP DOWNLOAD		;
	AND #$0F
	STA !S+1
	SEC
	LDA DEMOSX,X
!S	SBC #$FF
	STA DEMOSX,X
	BCC !255
	BCS !OK

!SWAPYN	LDA DEMOSOX
	CMP #DTV
	BNE !R
	LDA STARCOLEOR
	EOR #1
	STA STARCOLEOR
	BEQ !NS
	LDA #STCV
	STA STARCOLO
!NS	LDY #5		;7
!PH	LDA DEMOSX,Y
	PHA
	LDA DEMOSY,Y
	PHA
	LDA DEMOSOY,Y
	PHA
	LDA DEMOSDY,Y
	PHA
	DEY
	BPL !PH
	LDY #5		;7
!PL	PLA
	STA DEMOSDY,Y
	PLA
	STA DEMOSOY,Y
	PLA
	STA DEMOSY,Y
	PLA
	STA DEMOSX,Y
	DEY
	BPL !PL
	LDA #0
	STA TT
	LDX #5		;7
!DSWAP	LDA $D010
	AND BTAB255,X
	BEQ !0
	LDA TT
	ORA DTAB255,X
	STA TT
!0	DEX
	BPL !DSWAP
	LDA TT
	STA $D010
	LDY #5		;7
!D2	LDA #0
	STA TT
	LDX #5		;7
!D3	LDA DEMO15,Y
	AND BTAB255,X
	BEQ !00
	LDA TT
	ORA DTAB255,X
	STA TT
!00	DEX
	BPL !D3
	LDA TT
	STA DEMO15,Y
	DEY
	BPL !D2
!R	RTS

DTAB255	HEX	804020100804
;	HEX	8040201008040201

DEMOSXL	DL	DXT2,DXT3,DXT4,DXT5,DXT6,DXT7
	;DL	DXT1,DXT2,DXT3,DXT4,DXT5,DXT6,DXT7,DXT8
DEMOSXH	DH	DXT2,DXT3,DXT4,DXT5,DXT6,DXT7

;DXT1	HEX	00000100
;	HEX	01010201020203020303040304040504050506050606 0606
;	HEX	06060506050504050404030403030203020201020101
;	HEX	00010000

DXT2	HEX	00000001
	HEX	00000100010102010202030203030403040405040505 0404
	HEX	05050405040403040303020302020102010100010000
	HEX	01000000
DXC21
;DMV	EQU	DXC21-DXC1		;middle of each pattern
DTV	EQU	DXC21-DXT2-1	;total size of each pattern

DXT3	HEX	00000000
	HEX	00000000000000010001010201020203020303040304 0304
	HEX	04030403030203020201020101000100000100000000
	HEX	00000000

DXT4	HEX	00000000
	HEX	00000000000000000000000000000100010102010202 0102
	HEX	02020102010100010000010000000000000000000000
	HEX	00000000

;DXT8	HEX	0000F100
;	HEX	F1F1F2F1F2F2F3F2F3F3F4F3F4F4F5F4F5F5F6F5F6F6 F6F6
;	HEX	F6F6F5F6F5F5F4F5F4F4F3F4F3F3F2F3F2F2F1F2F1F1
;	HEX	00F10000

DXT7	HEX	000000F1
	HEX	0000F100F1F1F2F1F2F2F3F2F3F3F4F3F4F4F5F4F5F5 F4F4
	HEX	F5F5F4F5F4F4F3F4F3F3F2F3F2F2F1F2F1F100F10000
	HEX	F1000000

DXT6	HEX	00000000
	HEX	00000000000000F100F1F1F2F1F2F2F3F2F3F3F4F3F4 F3F4
	HEX	F4F3F4F3F3F2F3F2F2F1F2F1F100F10000F100000000
	HEX	00000000

DXT5	HEX	00000000
	HEX	0000000000000000000000000000F100F1F1F2F1F2F2 F1F2
	HEX	F2F2F1F2F1F100F10000F10000000000000000000000
	HEX	00000000


DEMOSMODY	LDA TEXTF
	BEQ !R
	LDX #5		;7
!1	LDA DEMOSDY,X
	BEQ !0
	LDA UNI7	;init. delay speed
	BNE !X
	DEC DEMOSDY,X
	BNE !X
!0	JSR !MOD
!X	DEX
	BPL !1
!R	RTS
!MOD	LDY DEMOSOY,X
	DEY
	BPL !Y
	LDY #DYV
!Y	TYA
	STA DEMOSOY,X
!OK	SEC
	LDA DEMOSY,X
	SBC DEMOSYTAB,Y
	STA DEMOSY,X
	RTS


DEMOSYTAB	HEX	0101010100010001000001
	HEX	0000 FF0000FF00FF00FFFFFFFF
	HEX	FFFFFFFF00FF00FF0000FF
	HEX	0000 0100000100010001010101
DYC
DYV	EQU	DYC-DEMOSYTAB-1

DEMOSETMES	LDA #>DEMOMESS
	STA TT+20
	LDA #<DEMOMESS
	STA TT+21
	LDY #0
	STY DEMOLETO
	RTS

DEMOLET	LDA TEXTF
	BEQ !R
;	LDX DEMOLETO
;!L	LDA DEMOMESS,X
	LDY DEMOLETO
!L	LDA (TT+20),Y
	CMP #$FF
	BNE !2
;	LDX #0
;	STX DEMOLETO
	JSR DEMOSETMES
	BEQ !L
!2	CMP #" "
	BNE !3
	LDX #4		;Y
	LDA DSPACE,X
!0	STA DEMO15,X
	DEX
	BPL !0
	BMI !E
!3	;CMP #"."
	;BNE !5
	;LDY #4
!A	;LDA DSTOP,Y
	;STA DEMO15,Y
	;DEY
	;BPL !A
	;BMI !E
!5	ASL
	ASL
	ASL
	TAY
!4	LDX #0
!1	LDA DEMOLETS-8,Y
	STA DEMO15,X
	INY
	INX
	CPX #5	;8
	BCC !1
	LDY DEMOLETO
!E	INY
	BNE !O
	INC TT+21
!O	STY DEMOLETO
!R	RTS

DEMOMESS	CBM	"WELCOME TO THE FIRST MAYHEM MEGA MUSIC MIX   "
	CBM	"MAYHEM GIVES YOU ACCESS TO FORTY BITS OF MUSIC "
	CBM	"FROM THE FIRST THREE GAMES BY APEX   "
	CBM	"NEXT MONTHS DEMO CONTAINS EVEN MORE MEGA TUNES  "
	CBM	"SO UNTIL THEN SIT BACK AND ENJOY THE MUSIC       ",255

DSPACE	DB	%11111111
	DB	%11111111
	DB	%11111111
	DB	%11111111
	DB	%11111111

;DSTOP	DB	%00000000
;	DB	%00000000
;	DB	%00000000
;	DB	%00011000
;	DB	%00011000




;TEXTF	DB	0

;	LDA #$44	;setup
;	STA TJ+3
;	LDA #$48
;	STA TJ+1
;	LDA #$D8
;	STA TJ+5
;	LDA #$00
;	STA TJ
;	STA TJ+2
;	STA TJ+4
;	STA TJ+6
;
;TEXTPRINT	LDA TEXTF
;	BNE !R
;	LDA UNI2
;	BNE !R
;	LDY TJ+6
;	LDA (TJ),Y
;	CMP #$FF
;	BEQ !FF
;	STA (TJ+2),Y
;	LDA #9		;colour
;	STA (TJ+4),Y
;!N	INC TJ+6
;	BNE !E
;	INC TJ+1
;	INC TJ+3
;	INC TJ+5
;!E	CPY #$E7
;	BNE !R
;	LDA TJ+3
;	CMP #$47
;	BNE !R
;	INC TEXTF
;!R	RTS
;!FF	LDA TEXTF
;	BNE !R
;	JSR !N
;	LDY TJ+6
;	LDA (TJ),Y
;	CMP #$FF
;	BEQ !FF
;	RTS

DEMOANIF	DB	0

DEMOANI	LDA TEXTF
	BEQ !R
	LDA DEMOANIF
	BNE !R
	LDA UNI2
	BNE !R
	LDX #5		;7
!L	LDA DEMOANISD,X
	BEQ !OK
	DEC DEMOANISD,X
	BPL !E
!OK	LDY DEMOANIS,X
	CPY #$4D	;$0F
	BNE !N4D
	LDA #$94
	BNE !A
!N4D	INY
	CPY #MIDSTAR+1
	BCS !E
	TYA
!A	STA DEMOANIS,X
!E	DEX
	BPL !L
	LDX #5		;7
!C	LDA DEMOANIS,X
	CMP #MIDSTAR
	BNE !R
	DEX
	BPL !C
	INC DEMOANIF
!R	RTS


MIDSTAR	EQU	$96	;$F9
STARCOL	HEX	09 0F 15 21 27 2D
;	HEX	00 09 12 1B 24 2D
STARHI	DH	STAR2,STAR3,STAR4,STAR5,STAR6,STAR7
	;DH	STAR1,STAR2,STAR3,STAR4,STAR5,STAR6,STAR7,STAR8
STARLO	DL	STAR2,STAR3,STAR4,STAR5,STAR6,STAR7

;94:small, 96:middle, 98:large
;ani's, col's
STAR7	HEX	96 95 95 94 95 95	0A 08 02 09 02 08
STAR6	HEX	96 96 95 95 95 96	0A 0A 08 02 08 0A
STAR5	HEX	96 96 96 95 96 96	0A 0A 0A 08 0A 0A
STAR4	HEX	96 96 96 97 96 96	0A 0A 0A 0A 0A 0A
STAR3	HEX	96 96 97 97 97 96	0A 0A 0A 07 0A 0A
STAR2	HEX	96 97 97 98 97 97	0A 0A 07 01 07 0A

;STAR7	HEX	96 95 95 94 95 95	03 0E 04 06 04 0E
;STAR6	HEX	96 96 95 95 95 96	03 03 0E 04 0E 03
;STAR5	HEX	96 96 96 95 96 96	03 03 03 0E 03 03
;STAR4	HEX	96 96 96 97 96 96	03 03 03 03 03 03
;STAR3	HEX	96 96 97 97 97 96	03 03 03 0D 03 03
;STAR2	HEX	96 97 97 98 97 97	03 03 0D 01 0D 03

;;STAR1	HEX	F9 F8 F7 F6 F7 F8
;STAR2	HEX	F9 F8 F7 F7 F7 F8
;STAR3	HEX	F9 F9 F8 F7 F8 F9
;STAR4	HEX	F9 F9 F9 F8 F9 F9
;STAR5	HEX	F9 F9 F9 FA F9 F9
;STAR6	HEX	F9 F9 FA FB FA F9
;STAR7	HEX	F9 FA FB FB FB FA
;;STAR8	HEX	F9 FA FB FC FB FA

DEMOSIZE	LDX #5	;7
	LDA DEMOSOX
	LDY #4	;5
!Y	CMP STARCOL,Y
	BEQ !GOT
	DEY
	BPL !Y
	RTS
!GOT	LDA STARHI,X
	STA !ANI+2
	LDA STARLO,X
	STA !ANI+1
;	JSR !ANI		;
!ANI	LDA $FFFF,Y
	STA DEMOANIS,X

;	TYA			;colours
;	CLC
;	ADC #6
;	TAY
;	JSR !ANI
;	STA DEMOCOLS,X
;	TYA
;	SEC
;	SBC #6
;	TAY			;

!E	DEX
	BPL !GOT
	RTS
;!ANI	LDA $FFFF,Y		;
;	RTS

DEMOLETS
	HEX 78CCFCCCCC000000F8CCF8CCF80000007CC0C0C07C000000F8CCCCCCF8000000FCC0F0C0FC000000FCC0F0C0C00000007CC0DCCC7C00
	HEX 0000CCCCFCCCCC000000FC303030FC0000000C0C0CCC78000000CCD8F0D8CC000000C0C0C0C0FC00000084CCFCB484000000CCECFCDC
	HEX CC00000078CCCCCC78000000F8CCF8C0C000000078CCCCDC7C000000F8CCF8CCCC0000007CC0780CF8000000FC30303030000000CCCC
	HEX CCCC78000000CCCCCC783000000084B4FCCC84000000CCCC78CCCC000000CCCC783030000000FC183060FC000000

;	HEX 78CCFCCCCC000000
;	HEX F8CCF8CCF8000000
;	HEX 7CC0C0C07C000000
;	HEX F8CCCCCCF8000000
;	HEX FCC0F0C0FC000000
;	HEX FCC0F0C0C0000000
;	HEX 7CC0DCCC7C000000
;	HEX CCCCFCCCCC000000
;	HEX FC303030FC000000
;	HEX 0C0C0CCC78000000
;	HEX CCD8F0D8CC000000
;	HEX C0C0C0C0FC000000
;	HEX 84CCFCB484000000
;	HEX CCECFCDCCC000000
;	HEX 78CCCCCC78000000
;	HEX F8CCF8C0C0000000
;	HEX 78CCCCDC7C000000
;	HEX F8CCF8CCCC000000
;	HEX 7CC0780CF8000000
;	HEX FC30303030000000
;	HEX CCCCCCCC78000000
;	HEX CCCCCC7830000000
;	HEX 84B4FCCC84000000
;	HEX CCCC78CCCC000000
;	HEX CCCC783030000000
;	HEX FC183060FC000000


;	HEX 0C1E3E7363FFFFC300000000000000000E3F7360E0F37F3E00000000000000003E7E607CFCE0FF7FFF000000000000FFFF0000000000
;	HEX 00FF3333737E7EE6E7E77E7E18183030FCFC00000000000000000000000000000000000000000000000036377F7F6BE3E3E300000000
;	HEX 000000000000000000000000000000000000000000000000000000003E3F637E7CE6E7E73E63607E3F07FF7E7E7E1818303070700000
;	HEX 00000000000000000000000000000000000000000000000000000000000066666E7C3830707042A94C20C0B5A94E


COLBUFFER	DS	20

	DB	"THIS IS A MESSAGE TO WARREN PILKINGTON: WE KNEW YOU'D"
	DB	"LOOK IN HERE YOU PREDICTABLE BASTARD! - THE BOYZ"

ENDCODE	;upto $4400




	END	GO















