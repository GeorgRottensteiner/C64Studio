            processor   6502

CYCLES_PER_LINE_PAL       = 63


NUMOFSPR	SET 62
;NUMOFSPR	SET 32

LowestSprPos	SET 43

		MAC	random
		lda seed
		beq .doEor
		asl
		beq .noEor ;if the input was $80, skip the EOR
		bcc .noEor
.doEor	eor #$1d
.noEor	sta	seed
		ENDM

		seg.u   bss
		org     $00
ZEROZERO	ds.z	1
ZEROONE	ds.z	1
AS		ds.z	1
XS		ds.z	1
YS		ds.z	1
TEMP	ds.z	1
;CO		ds.z	1
YTAB		ds.z	NUMOFSPR
YORDER		ds.z	NUMOFSPR
XTAB		ds.z	NUMOFSPR
UD			ds.z	NUMOFSPR

		org     $0100
BIGTAB		ds	$100


		org     $07f8
POINTERS	ds	0



		seg     code
		org	$0801
		dc.b $0c,$08,$d0,$07,$9e,$20,$32,$30,$36,$34,$00,$00,$00,$00	; SYS 2064 in "Basic-Code"
		org $0810
START		subroutine
		SEI
		CLD
		LDX	#$FF
		TXS
		LDA	#$35
		STA	$01


		LDA	#0
		STA	$D011
		STA	$D015
		STA	$D019
		STA	$D01A
		STA	$D020
		STA	$D021
		LDX	#2
.1		STA	$00,X
		INX
		BNE	.1
		LDA	#INT&255
		STA	$FFFE
		LDA	#INT/256
		STA	$FFFF
		LDA	#NMI&255
		STA	$FFFA
		LDA	#NMI/256
		STA	$FFFB
		LDA	#127
		STA	$DC0D
		LDA	#1
		STA	$D01A
        STA $D019

		LDA	#255
		STA	$D015
		LDX	#0
		LDA	#$FF
POP		STA	BIGTAB,X
		DEX
		BNE	POP
		LDX	#63
POP3		STA	$03C0,X
		DEX
		BPL	POP3
		LDX	#NUMOFSPR-1
        LDA #LowestSprPos
		JMP .1stpass
POP2
		TXA
		AND #31
		TAY
		LDA YTAB+1,X
		CLC
		ADC YTABINC,Y
.1stpass
		STA	YTAB,X
.rndloopX
		:random
		CMP #148
		BCS .rndloopX
		CMP #128-12
		BCS .noneed

		ASL
		CLC
		ADC #24
		STA XTAB,x
		LDA #0
		BEQ .STAMSB
.noneed
		SEC
		SBC #128-12
		ASL
		STA XTAB,X
		LDA #1
.STAMSB STA MSB,x
		;LDA	XTAB2,x
        ;STA	XTAB,x

		TXA
		AND #7
		EOR #7
.polp	;CMP #6
		;BCC .break1
		;SBC #6
		;BNE .polp

.break1
		ASL
		ASL
		ASL
		STA TEMP
		ASL
		CLC
		ADC TEMP
		CLC
		ADC #24
		STA XTAB,x
		LDA #0
		STA MSB,x
.rndLR
:random
		LSR
		and #7
		beq .rndLR
		STA LR,X
:random
		LSR
		and #1
		beq .nolr
		LDA LR,X
		ORA #128
		STA LR,X
.nolr
		LDA	UD2,x
		STA	UD,x
		TXA
		STA YORDER,X
.rndloop
:random
		LSR
		AND #15
		BEQ .rndloop
		STA COL,X
		DEX
		bmi	.setupfin
		JMP POP2
.setupfin
		LDA #$FA
		STA	$D012
		CLI
		LDA #0
DOB		CMP #0
		BEQ DOB


INT		DEC	$D020
		STA	AS
		STX	XS
		STY	YS
		LDY #0
		STY	$D011
		INY
		STY	$D019

		LDA	#<PLEXOR
		STA	$FFFE
		LDA	#>PLEXOR
		STA	$FFFF
		LDA	#<Plexor3
		STA	PlexJump+1
		LDA	#>Plexor3
		STA	PlexJump+2

		LDX	#$1C
		LDA	#$FC
NOBD	CMP	$D012
		BNE	NOBD
		STX	$D011

		DEC	$D020
		;JSR	MOVER2
		JSR MOVERBUBBLESORT
		LDA #0
		STA $D015
		DEC	$D020
		JSR BubbleYSort
		;JSR	YSORT
		DEC	$D020
FIRST
		LDA #255
		STA $D015

		LDX	YORDER+NUMOFSPR-1
		LDA	YTAB,X
		STA	$D001
		LDA	XTAB,X
		STA	$D000
		LDA	DEF,X
		STA	POINTERS+0
		LDY	MSB,X		;Y register holds what will end up in $d010
		LDA	COL,X
		STA	$D027

		LDX	YORDER+NUMOFSPR-2
		LDA	YTAB,X
		STA	$D003
		LDA	XTAB,X
		STA	$D002
		LDA	DEF,X
		STA	POINTERS+1
		LDA	MSB,X
		BEQ	MO1
		INY
		INY
MO1		LDA	COL,X
		STA	$D028

		LDX	YORDER+NUMOFSPR-3
		LDA	YTAB,X
		STA	$D005
		LDA	XTAB,X
		STA	$D004
		LDA	DEF,X
		STA	POINTERS+2
		LDA	MSB,X
		BEQ	MO2
		TYA
		ORA	#4
		TAY
MO2		LDA	COL,X
		STA	$D029

		LDX	YORDER+NUMOFSPR-4
		LDA	YTAB,X
		STA	$D007
		LDA	XTAB,X
		STA	$D006
		LDA	DEF,X
		STA	POINTERS+3
		LDA	MSB,X
		BEQ	MO3
		TYA
		ORA	#8
		TAY
MO3		LDA	COL,X
		STA	$D02A

		LDX	YORDER+NUMOFSPR-5
		LDA	YTAB,X
		STA	$D009
		LDA	XTAB,X
		STA	$D008
		LDA	DEF,X
		STA	POINTERS+4
		LDA	MSB,X
		BEQ	MO4
		TYA
		ORA	#16
		TAY
MO4		LDA	COL,X
		STA	$D02B

		LDX	YORDER+NUMOFSPR-6
		LDA	YTAB,X
		STA	$D00B
		LDA	XTAB,X
		STA	$D00A
		LDA	DEF,X
		STA	POINTERS+5
		LDA	MSB,X
		BEQ	MO5
		TYA
		ORA	#32
		TAY
MO5		LDA	COL,X
		STA	$D02C

		LDX	YORDER+NUMOFSPR-7
		LDA	YTAB,X
		STA	$D00D
		LDA	XTAB,X
		STA	$D00C
		LDA	DEF,X
		STA	POINTERS+6
		LDA	MSB,X
		BEQ	MO6
		TYA
		ORA #64
		TAY
MO6		LDA	COL,X
		STA	$D02D

		LDX	YORDER+NUMOFSPR-8
		LDA	YTAB,X
		STA	$D00F
		LDA	XTAB,X
		STA	$D00E
		LDA	DEF,X
		STA	POINTERS+7
		LDA	MSB,X
		BEQ	MO7
		TYA
		ORA #128
		TAY
MO7
		STY	$D010
		LDA	COL,X
		STA	$D02E


RasterWait
;                        LDA $d012
;						BNE RasterWait

        LDA $D001

		clc
		adc #22
		sta $d012

		LDA	#0
		STA	$D020
		LDA	AS
		LDX	XS
		LDY	YS
		RTI


	MAC	YPOSWRITE
	LDA	YTAB+{1}
	LSR
	TAX
	TXS
.1	PLA
	BPL	.1
	LDA	#{1}
	PHA
	ENDM

	MAC	YPOSREAD
.1	PLA
	BMI	.1
	STA	YORDER+{1}
	TXA
	PHA
	PLA
	ENDM


YSORT		TSX
		STX	YV+1
		LDX	#20
		TXS
		LDX	#$80
Y		SET	NUMOFSPR
		REPEAT	NUMOFSPR
:YPOSREAD		Y
Y		SET	Y - 1
		REPEND
YV		LDX	#0
		TXS
		RTS











INTEND
	LDA	#INT&255
	STA	$FFFE
	LDA	#INT/256
	STA	$FFFF
	LDA	#$FA
	STA	$D012
INTOVER	LDA	#0
	STA	$D020
	LDA	AS
	LDX	XS
	LDY	YS
NMI	RTI




		MAC	MoveWithYSortMacro
		LDA	LR+{1}
		BMI	.RIGHT
		LDA	XTAB+{1}
		SEC
		SBC	LR+{1}
		STA	XTAB+{1}
		BCC	.SETMSB0
		CMP	#25
		BCS .NXT2
		LDA MSB+{1}
		BNE .NXT3
		BCC	.CHGXDIR
.SETMSB0
		LDA	#0
		STA	MSB+{1}
		BEQ	.NXT3
.SETMSB1
		LDA	#1
		STA	MSB+{1}
		BNE	.NXT2

.RGTMSB	CMP	#65
		BCC	.NXT3

.CHGXDIR
		LDA	LR+{1}
		EOR	#128
		STA	LR+{1}
		BNE .NXT2
.CHG2	LDX #$FB>>1
		LDA	#255
		STA	UD+{1}
		BNE	.CHG
.CHG3	LDX #42>>1
		LDA	#1
		STA	UD+{1}
		BNE	.CHG

.RIGHT	AND	#127
		CLC
		ADC	XTAB+{1}
		STA	XTAB+{1}
		BCS	.SETMSB1
		LDX	MSB+{1}
		BNE	.RGTMSB

.NXT2	CLC
.NXT3	LDA	UD+{1}
		ADC	YTAB+{1}
		STA	YTAB+{1}
		CMP	#LowestSprPos
		BCC	.CHG3
		CMP	#$fa
		BCS	.CHG2
		; Now take the Y pos and use it as an index into the stack bucket table
		; Divide it by 2 cos we only want to use the bottom $80 bytes
		LSR
		TAX
.CHG	TXS
.POT	PLA
		BPL	.POT
		LDA #{1}
		PHA

		ENDM


MOVER2	TSX
		STX	.XR+1

Y		SET	NUMOFSPR-1
		REPEAT	NUMOFSPR
:MoveWithYSortMacro		Y
Y		SET	Y - 1
		REPEND

.XR		LDX	#0
		TXS
		RTS

PLEXOR
		STA	AS
		STX	XS
		STY	YS
		LDA	#1
		STA	$D019
		STA	$D020
;		LDY	CO
		SEC
PlexJump
		JMP	PlexJump


		MAC	SpriteSplitMacro
		LDX	YORDER,Y
		LDA	YTAB,X
		STA	$d001+{1}
		LDA	XTAB,X
		STA	$d000+{1}
		LDA	DEF,X
		STA	POINTERS+{2}
		LDA	MSB,X
		BEQ	.MO0
		LDA	#1<<{2}
		ORA	$D010
		BNE	.BA0
.MO0	LDA	#~(1<<{2})
		AND	$D010
.BA0	STA	$D010
		LDA	COL,X
		STA	$D027+{2}
		DEY
		BMI	.OC0
.SlightDelay
		LDA	$D012
		SBC	#22
		SBC	$d001+{3}
		BCS	.IT1
		cmp #255
		bcs .SlightDelay
		LDA	$d001+{3}
		ADC #22
		;EOR	#255
		;ADC	#1
		;ADC	$D012
		STA	$D012

		LDA	#<.IT1
		STA	PlexJump+1
		LDA	#>.IT1
		STA	PlexJump+2

;		STY	CO
		LDA	#0
		STA	$D020
		LDA	AS
		LDX	XS
		LDY	YS
		RTI
.OC0	JMP	INTEND
.IT1
		ENDM


Plexor2
Y		SET	0			;Index to color, msb shift, definition
X		SET 0			;Index to sprite position registers
Z		SET 2			;Index to the next sprite y position register to check to use
		REPEAT	8
:SpriteSplitMacro		X,Y,Z
Y		SET	Y + 1
Y		SET	(Y&7)
X		SET	X + 2
X		SET	(X&15)
Z		SET	Z + 2
Z		SET (Z&15)
		REPEND
		JMP Plexor2






		MAC	BubbleYSortMacro
			LDY	YORDER+1+{1}
			LDA YTAB,Y
			LDX	YORDER+{1}
			CMP	YTAB,X
			BCC	.NoNeedToSwap
			STX	YORDER+1+{1}
			STY YORDER+{1}
.NoNeedToSwap
		ENDM

BubbleYSort
Y		SET	NUMOFSPR-2
		REPEAT	NUMOFSPR-1
:BubbleYSortMacro		Y
Y		SET	Y - 1
		REPEND
		RTS


		MAC	MoveMacro
		LDA	LR+{1}
		BMI	.RIGHT
		LDA	XTAB+{1}
		SEC
		SBC	LR+{1}
		STA	XTAB+{1}
		BCC	.SETMSB0
		CMP	#25
		BCS .NXT2
		LDA MSB+{1}
		BNE .NXT3
		BCC	.CHGXDIR
.SETMSB0	LDA	#0
		STA	MSB+{1}
		BEQ	.NXT3
.SETMSB1
		LDA	#1
		STA	MSB+{1}
		BNE	.NXT2

.RGTMSB	CMP	#65
		BCC	.NXT3

.CHGXDIR	LDA	LR+{1}
		EOR	#128
		STA	LR+{1}
		BNE .NXT2
.CHG2	LDA	#255
		STA	UD+{1}
		BNE	.CHG
.CHG3	LDA	#1
		STA	UD+{1}
		BNE	.CHG

.RIGHT	AND	#127
		CLC
		ADC	XTAB+{1}
		STA	XTAB+{1}
		BCS	.SETMSB1
		LDX	MSB+{1}
		BNE	.RGTMSB

.NXT2	CLC
.NXT3	LDA	UD+{1}
		ADC	YTAB+{1}
		STA	YTAB+{1}
		CMP	#LowestSprPos
		BCC	.CHG3
		CMP	#$fa
		BCS	.CHG2
.CHG
		ENDM


MOVERBUBBLESORT

Y		SET	NUMOFSPR-1
		REPEAT	NUMOFSPR
:MoveMacro		Y
Y		SET	Y - 1
		REPEND

		RTS





		MAC	SpriteSplitMacro3
		LDX	YORDER+{4}
		LDA	YTAB,X
		STA	$d001+{1}
		LDA	XTAB,X
		STA	$d000+{1}
		LDA	DEF,X
		STA	POINTERS+{2}
		LDA	MSB,X
		BEQ	.MO0
		LDA	#1<<{2}
		ORA	$D010
		BNE	.BA0
.MO0	LDA	#~(1<<{2})
		AND	$D010
.BA0	STA	$D010
		LDA	COL,X
		STA	$D027+{2}
.SlightDelay
		LDA	$D012
		SBC	#22
		SBC	$d001+{3}
		BCS	.IT1
		cmp #255
		bcs .SlightDelay
		LDA	$d001+{3}
		ADC #22
		;EOR	#255
		;ADC	#1
		;ADC	$D012
		STA	$D012

		LDA	#<.IT1
		STA	PlexJump+1
		LDA	#>.IT1
		STA	PlexJump+2

		LDA	#0
		STA	$D020
		LDA	AS
		LDX	XS
		LDY	YS
		RTI
.IT1
		ENDM


		MAC	SpriteSplitMacro4
		LDX	YORDER+{4}
		LDA	YTAB,X
		STA	$d001+{1}
		LDA	XTAB,X
		STA	$d000+{1}
		LDA	DEF,X
		STA	POINTERS+{2}
		LDA	MSB,X
		BEQ	.MO0
		LDA	#1<<{2}
		ORA	$D010
		BNE	.BA0
.MO0	LDA	#~(1<<{2})
		AND	$D010
.BA0	STA	$D010
		LDA	COL,X
		STA	$D027+{2}
		ENDM


Plexor3
Y		SET	0			;Index to color, msb shift, definition
X		SET 0			;Index to sprite position registers
Z		SET 2			;Index to the next sprite y position register to check to use
W		SET NUMOFSPR-9
		REPEAT	NUMOFSPR-9
:SpriteSplitMacro3		X,Y,Z,W
Y		SET	Y + 1
Y		SET	(Y&7)
X		SET	X + 2
X		SET	(X&15)
Z		SET	Z + 2
Z		SET (Z&15)
W		SET W - 1
		REPEND
:SpriteSplitMacro4		X,Y,Z,W
		JMP	INTEND






seed	dc.b	43


LR
	dc.b	2,129,3,130,1,131,2,129,3,130,1,131,2,129,3,130,1,135,5,129,4,133,1,134,2,133,6,130,1,130,7,133,3,130,7,129,2,131,4,133,4,132,2,129,1,131,1,129,3,130,7,129,2,131,4,133,4,132,2,129,1,131,1,129,3,130,7,129,2,131,4,133,4,132,2,129,1,131,1,129
COL
	dc.b	1,2,3,4,5,6,7,8
	dc.b	1,2,3,4,5,6,7,8
	dc.b	1,2,3,4,5,6,7,8
	dc.b	1,2,3,4,5,6,7,8
	dc.b	1,2,3,4,5,6,7,8
	dc.b	1,2,3,4,5,6,7,8
	dc.b	1,2,3,4,5,6,7,8
	dc.b	1,2,3,4,5,6,7,8

DEF
	dc.b	15,15,15,15,15,15,15,15
	dc.b	15,15,15,15,15,15,15,15
	dc.b	15,15,15,15,15,15,15,15
	dc.b	15,15,15,15,15,15,15,15
	dc.b	15,15,15,15,15,15,15,15
	dc.b	15,15,15,15,15,15,15,15
	dc.b	15,15,15,15,15,15,15,15
	dc.b	15,15,15,15,15,15,15,15

MSB
	dc.b	1,1,0,1,0,0,0,1
	dc.b	1,1,0,1,0,0,0,1
	dc.b	1,1,0,1,0,0,0,1
	dc.b	1,1,0,1,0,0,0,1
	dc.b	1,1,0,1,0,0,0,1
	dc.b	1,1,0,1,0,0,0,1
	dc.b	1,1,0,1,0,0,0,1
	dc.b	1,1,0,1,0,0,0,1

UD2
	dc.b	255,1,255,1,255,1,255,1
	dc.b	255,1,255,1,255,1,255,1
	dc.b	255,1,255,1,255,1,255,1
	dc.b	255,1,255,1,255,1,255,1
	dc.b	255,1,255,1,255,1,255,1
	dc.b	255,1,255,1,255,1,255,1
	dc.b	255,1,255,1,255,1,255,1
	dc.b	255,1,255,1,255,1,255,1



YTABINC
		dc.b 3,4,3,4
		dc.b 3,3,4,3
		dc.b 3,4,3,4
		dc.b 3,3,4,3
		dc.b 3,4,3,4
		dc.b 3,3,4,3
		dc.b 3,4,3,4
		dc.b 3,3,4,3






