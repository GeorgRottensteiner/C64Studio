LSMF .EQU &1234
HURZ EQU $0f ~ -$fe
BL_SPR  EQU (&B780-&8000)/64  ; blank sprite after pannel
SHOULD_BE_1 = 2 + 7 & 3
SHOULD_BE_5 = 7 & 3 + 2

GNU EQU &2000
  ORG GNU

	.BLOCK 1,2
	DS 1,2
  ADC #(TOGGLETAB+1)&255

  ADC #TOGGLETAB&255
  ADC #TOGGLETAB+1&255


!1:  STA TOGGLETAB,x

  ADC #TOGGLETAB+1&255


  TOGGLETAB EQU $17

    JIFFY EQU $12
!2SYNCH LDA JIFFY
  bcs !2synch


  BYTE  ((╚SPRT_XW╝*$10)&$F0) ! ((╚BAD_STNG╝*$02)&$0E) ! (0&$01)

YCENT EQU STND_YA-$05

STND_YA EQU $08





FX1: EQU $500



  .BYTE $60,$70


FXBASEVECT:  .WORD FX1+$000,FX1+$040,FX1+$080,FX1+$0C0
  .WORD FX1+$100,FX1+$140,FX1+$180,FX1+$1C0
  .WORD FX1+$200,FX1+$240,FX1+$280,FX1+$2C0
  .WORD FX1+$300,FX1+$340,FX1+$380,FX1+$3C0
  .WORD FX1+$400

MTABETAB:  MACRO   ; exit table address
  .WORD @1

  BYTE  ((@1*$10)&$F0) ! ((@2*$02)&$0E) ! (@3&$01)
  ENDM

  MTABETAB SPRT_XW, BAD_STNG, 0

SPRT_XW .BLOCK $0C,$00

BAD_STNG  STRING "WORDSINTHESTRING"

BULL_TABL: DLOW  12 ,13
  DLOW  14,15

  bne !2SYNCH

!2SYNCH

  SBC #XCENT





XCENT = 5