* = $0801

ZEROPAGE_POINTER_H = $e0
ZEROPAGE_POINTER_L = $e2
ZEROPAGE_POINTER_D = $e4

ZEROPAGE_POINTER_H1 = $e0
ZEROPAGE_POINTER_L1 = $e2
ZEROPAGE_POINTER_D1 = $e4
ZEROPAGE_POINTER_H2 = $e6
ZEROPAGE_POINTER_L2 = $e8
ZEROPAGE_POINTER_D2 = $ea
ZEROPAGE_POINTER_H3 = $ec
ZEROPAGE_POINTER_L3 = $ee
ZEROPAGE_POINTER_D3 = $f0

!basic


;*******CONTROLLING MULTIPLE VOICES WITH INDEPENDENT DURATION******
;****CHAPTER 33***********

          ;10 FOR S=54272TO54296:POKES,0:NEXT
          lda #0
          ldx #0
-
          sta 54272,x
          inx
          cpx #( 54296 - 54272 )
          bne -

M = 54272

;30 V(0)=17:V(1)=17:V(2)=17

;40 REM ***LOAD ARRAY****


;100 FOR K=0 TO 2
          lda #0
          sta K

          sta .READ_POS

K_LOOP
;110 I=0

          ldy K
          lda H_START_LO,y
          sta ZEROPAGE_POINTER_H
          lda H_START_HI,y
          sta ZEROPAGE_POINTER_H + 1
          lda L_START_LO,y
          sta ZEROPAGE_POINTER_L
          lda L_START_HI,y
          sta ZEROPAGE_POINTER_L + 1
          lda D_START_LO,y
          sta ZEROPAGE_POINTER_D
          lda D_START_HI,y
          sta ZEROPAGE_POINTER_D + 1

          lda #0
          sta I

.Goto120
;120 READ N,DR
          ;N lo
          ldy .READ_POS
          lda LESS_DATA,y
          sta N_LO

          ;N hi
          ldy .READ_POS
          lda LESS_DATA + 1,y
          sta N_HI

          ;DR
          ldy .READ_POS
          lda LESS_DATA + 2,y
          sta DR

;130 IF DR=0 THEN 200
          beq .NextK

;140 WF=17:WX=WF-1

;150 HF%=N/256:LF%=N-256*HF%
          lda N_HI
          sta HF
          lda N_LO
          sta LF

;160 IF DR=1 THEN H(I,K)= HF%:L(I,K) = LF%:D(I,K)=17:I=I+1:GOTO120
          lda DR
          cmp #1
          bne .ContinueAt170

          ldy #0
          lda HF
          sta (ZEROPAGE_POINTER_H),y
          lda LF
          sta (ZEROPAGE_POINTER_L),y
          lda #17
          sta (ZEROPAGE_POINTER_D),y

          jsr IncI

          inc .READ_POS
          inc .READ_POS
          inc .READ_POS
          jmp .Goto120

.ContinueAt170
;170 FOR J=1TODR-1:H(I,K)= HF%:L(I,K) = LF%:D(I,K)=17:I=I+1:NEXT
          lda #1
          sta J

.NextJ
          ldy #0
          lda HF
          sta (ZEROPAGE_POINTER_H),y
          lda LF
          sta (ZEROPAGE_POINTER_L),y
          lda #17
          sta (ZEROPAGE_POINTER_D),y

          jsr IncI

;NEXT J
          inc J
          lda J
          cmp DR
          bne .NextJ

;180 H(I,K)= HF%:L(I,K) = LF%:D(I,K)=16
          ldy #0
          lda HF
          sta (ZEROPAGE_POINTER_H),y
          lda LF
          sta (ZEROPAGE_POINTER_L),y
          lda #16
          sta (ZEROPAGE_POINTER_D),y

;190 I=I+1:GOTO120
          jsr IncI

          inc .READ_POS
          inc .READ_POS
          inc .READ_POS
          jmp .Goto120


;200 NEXT K
.NextK
          inc K
          lda K
          cmp #3
          beq .KLoopDone

          ldy K
          lda H_START_LO,y
          sta ZEROPAGE_POINTER_H
          lda H_START_HI,y
          sta ZEROPAGE_POINTER_H + 1
          lda L_START_LO,y
          sta ZEROPAGE_POINTER_L
          lda L_START_HI,y
          sta ZEROPAGE_POINTER_L + 1
          lda D_START_LO,y
          sta ZEROPAGE_POINTER_D
          lda D_START_HI,y
          sta ZEROPAGE_POINTER_D + 1

          inc .READ_POS
          inc .READ_POS
          inc .READ_POS

          jmp K_LOOP
.KLoopDone


;250 REM ****SOUND SETTINGS****
;300 POKE 54277,0:POKE 54278,255
          lda #0
          sta 54277
          lda #255
          sta 54278

;310 POKE 54284,0:POKE 54285,255
          lda #0
          sta 54284
          lda #255
          sta 54285

;320 POKE 54291,0:POKE 54292,255
          lda #0
          sta 54291
          lda #255
          sta 54292

;330 POKE 54296,15
          lda #15
          sta 54296

;399 REM ******FACING THE MUSIC******
;400 P1=0:P2=0:P3=0
          lda #<H
          sta ZEROPAGE_POINTER_H1
          sta ZEROPAGE_POINTER_H2
          sta ZEROPAGE_POINTER_H3
          lda #>H
          sta ZEROPAGE_POINTER_H1 + 1
          sta ZEROPAGE_POINTER_H2 + 1
          sta ZEROPAGE_POINTER_H3 + 1
          lda #<L
          sta ZEROPAGE_POINTER_L1
          sta ZEROPAGE_POINTER_L2
          sta ZEROPAGE_POINTER_L3
          lda #>L
          sta ZEROPAGE_POINTER_L1 + 1
          sta ZEROPAGE_POINTER_L2 + 1
          sta ZEROPAGE_POINTER_L3 + 1
          lda #<D
          sta ZEROPAGE_POINTER_D1
          sta ZEROPAGE_POINTER_D2
          sta ZEROPAGE_POINTER_D3
          lda #>D
          sta ZEROPAGE_POINTER_D1 + 1
          sta ZEROPAGE_POINTER_D2 + 1
          sta ZEROPAGE_POINTER_D3 + 1

.Goto410
;410 POKE 54272,L(P1,0):POKE 54273,H(P1,0)
          ldy #0
          lda (ZEROPAGE_POINTER_L1),y
          sta 54272
          lda (ZEROPAGE_POINTER_H1),y
          sta 54273

;420 POKE 54279,L(P2,1):POKE 54280,H(P2,1)
          lda (ZEROPAGE_POINTER_L2),y
          sta 54279
          lda (ZEROPAGE_POINTER_H2),y
          sta 54280

;430 POKE 54286,L(P3,2):POKE 54287,H(P3,2)
          lda (ZEROPAGE_POINTER_L3),y
          sta 54286
          lda (ZEROPAGE_POINTER_H3),y
          sta 54287

;440 POKE 54276,D(P1,0):POKE 54283,D(P2,1):POKE 54290,D(P3,2)
          lda (ZEROPAGE_POINTER_D1),y
          sta 54276
          lda (ZEROPAGE_POINTER_D2),y
          sta 54283
          lda (ZEROPAGE_POINTER_D3),y
          sta 54290

;445 FOR T=1 TO 3:NEXT T
          ;delay
          ldx #0
          ldy #150
-
          dex
          bne -

          dey
          bne -

;450 P1=P1+1:P2=P2+1:P3=P3+1
          inc ZEROPAGE_POINTER_H1
          bne +
          inc ZEROPAGE_POINTER_H1 + 1
+
          inc ZEROPAGE_POINTER_H2
          bne +
          inc ZEROPAGE_POINTER_H2 + 1
+
          inc ZEROPAGE_POINTER_H3
          bne +
          inc ZEROPAGE_POINTER_H3 + 1
+
          inc ZEROPAGE_POINTER_L1
          bne +
          inc ZEROPAGE_POINTER_L1 + 1
+
          inc ZEROPAGE_POINTER_L2
          bne +
          inc ZEROPAGE_POINTER_L2 + 1
+
          inc ZEROPAGE_POINTER_L3
          bne +
          inc ZEROPAGE_POINTER_L3 + 1
+
          inc ZEROPAGE_POINTER_D1
          bne +
          inc ZEROPAGE_POINTER_D1 + 1
+
          inc ZEROPAGE_POINTER_D2
          bne +
          inc ZEROPAGE_POINTER_D2 + 1
+
          inc ZEROPAGE_POINTER_D3
          bne +
          inc ZEROPAGE_POINTER_D3 + 1
+

;455 IF D(P1,0)=0 THEN2000
          ldy #0
          lda (ZEROPAGE_POINTER_D1),y
          bne +
          jmp .Goto2000
+

.Goto460
;460 IF D(P2,1)=0 THEN2500
          ldy #0
          lda (ZEROPAGE_POINTER_D2),y
          bne +
          jmp .Goto2500
+

.Goto465
;465 IF D(P3,2)=0 THEN3000
          ldy #0
          lda (ZEROPAGE_POINTER_D3),y
          bne +
          jmp .Goto3000
+
;470 GOTO 410
          jmp .Goto410



;600 REM ***** LESS DATA! *******
.READ_POS
          !byte 0

LESS_DATA
;610 DATA 0,1,1432,2,3406,2,3215,2,2864,2,3215,2,3406,2
          !word 0
          !byte 1
          !word 1432
          !byte 2
          !word 3406
          !byte 2
          !word 3215
          !byte 2
          !word 2864
          !byte 2
          !word 3215
          !byte 2
          !word 3406
          !byte 2


;620 DATA 2145,12
          !word 2145
          !byte 12

;630 DATA 1432,2,3406,2,3215,2,2864,2,3215,2,3406,2
          !word 1432
          !byte 2
          !word 3406
          !byte 2
          !word 3215
          !byte 2
          !word 2864
          !byte 2
          !word 3215
          !byte 2
          !word 3406
          !byte 2


;640 DATA 4291,6,2145,6,0,0
          !word 4291
          !byte 6
          !word 2145
          !byte 6
          !word 0
          !byte 0

;700 DATA 0,1,0,48,5728,2,6218,2,5103,2,6430,2,5728,2,6812,2
          !word 0
          !byte 1
          !word 0
          !byte 48
          !word 5728
          !byte 2
          !word 6218
          !byte 2
          !word 5103
          !byte 2
          !word 6430
          !byte 2
          !word 5728
          !byte 2
          !word 6812
          !byte 2

;710 DATA 8583,2,7647,2,6812,2,6430,2,5728,2,5103,2,0,0
          !word 8583
          !byte 2
          !word 7647
          !byte 2
          !word 6812
          !byte 2
          !word 6430
          !byte 2
          !word 5728
          !byte 2
          !word 5103
          !byte 2
          !word 0
          !byte 0

;800 DATA 0,1,0,96,17167,40,11457,4,17167,4,15294,4,11457,4,15294,40,11457,4
          !word 0
          !byte 1
          !word 0
          !byte 96
          !word 17167
          !byte 40
          !word 11457
          !byte 4
          !word 17167
          !byte 4
          !word 15294
          !byte 4
          !word 11457
          !byte 4
          !word 15294
          !byte 40
          !word 11457
          !byte 4

;810 DATA 15294,4,13625,24,11457,24,12860,40,11457,4,9634,4,8583,44,11457,4
          !word 15294
          !byte 4
          !word 13625
          !byte 24
          !word 11457
          !byte 24
          !word 12860
          !byte 40
          !word 11457
          !byte 4
          !word 9634
          !byte 4
          !word 8583
          !byte 44
          !word 11457
          !byte 4

;820 DATA 12860,40,11457,4,8583,4,7647,48,0,0
          !word 12860
          !byte 40
          !word 11457
          !byte 4
          !word 8583
          !byte 4
          !word 7647
          !byte 48
          !word 0
          !byte 0

.Goto2000
;2000 P1=1:GOTO460
          ldy #0
          lda H_START_LO,y
          sta ZEROPAGE_POINTER_H1
          lda H_START_HI,y
          sta ZEROPAGE_POINTER_H1 + 1
          lda L_START_LO,y
          sta ZEROPAGE_POINTER_L1
          lda L_START_HI,y
          sta ZEROPAGE_POINTER_L1 + 1
          lda D_START_LO,y
          sta ZEROPAGE_POINTER_D1
          lda D_START_HI,y
          sta ZEROPAGE_POINTER_D1 + 1

          inc ZEROPAGE_POINTER_H1
          bne +
          inc ZEROPAGE_POINTER_H1 + 1
+
          inc ZEROPAGE_POINTER_L1
          bne +
          inc ZEROPAGE_POINTER_L1 + 1
+
          inc ZEROPAGE_POINTER_D1
          bne +
          inc ZEROPAGE_POINTER_D1 + 1
+
          jmp .Goto460


.Goto2500
;2500 P2=49:GOTO 465
          ldy #1
          lda H_START_LO,y
          sta ZEROPAGE_POINTER_H2
          lda H_START_HI,y
          sta ZEROPAGE_POINTER_H2 + 1
          lda L_START_LO,y
          sta ZEROPAGE_POINTER_L2
          lda L_START_HI,y
          sta ZEROPAGE_POINTER_L2 + 1
          lda D_START_LO,y
          sta ZEROPAGE_POINTER_D2
          lda D_START_HI,y
          sta ZEROPAGE_POINTER_D2 + 1

          lda ZEROPAGE_POINTER_H2
          clc
          adc #49
          sta ZEROPAGE_POINTER_H2
          bcc +
          inc ZEROPAGE_POINTER_H2 + 1
+

          lda ZEROPAGE_POINTER_L2
          clc
          adc #49
          sta ZEROPAGE_POINTER_L2
          bcc +
          inc ZEROPAGE_POINTER_L2 + 1
+
          lda ZEROPAGE_POINTER_D2
          clc
          adc #49
          sta ZEROPAGE_POINTER_D2
          bcc +
          inc ZEROPAGE_POINTER_D2 + 1
+
          jmp .Goto465


.Goto3000
;3000 P3=1:P2=1:RESTORE:GOTO410
          ldy #1
          lda H_START_LO,y
          sta ZEROPAGE_POINTER_H2
          lda H_START_HI,y
          sta ZEROPAGE_POINTER_H2 + 1
          lda L_START_LO,y
          sta ZEROPAGE_POINTER_L2
          lda L_START_HI,y
          sta ZEROPAGE_POINTER_L2 + 1
          lda D_START_LO,y
          sta ZEROPAGE_POINTER_D2
          lda D_START_HI,y
          sta ZEROPAGE_POINTER_D2 + 1

          lda ZEROPAGE_POINTER_H2
          clc
          adc #1
          sta ZEROPAGE_POINTER_H2
          bcc +
          inc ZEROPAGE_POINTER_H2 + 1
+

          lda ZEROPAGE_POINTER_L2
          clc
          adc #1
          sta ZEROPAGE_POINTER_L2
          bcc +
          inc ZEROPAGE_POINTER_L2 + 1
+
          lda ZEROPAGE_POINTER_D2
          clc
          adc #1
          sta ZEROPAGE_POINTER_D2
          bcc +
          inc ZEROPAGE_POINTER_D2 + 1
+

          ;:P2=1
          ldy #2
          lda H_START_LO,y
          sta ZEROPAGE_POINTER_H3
          lda H_START_HI,y
          sta ZEROPAGE_POINTER_H3 + 1
          lda L_START_LO,y
          sta ZEROPAGE_POINTER_L3
          lda L_START_HI,y
          sta ZEROPAGE_POINTER_L3 + 1
          lda D_START_LO,y
          sta ZEROPAGE_POINTER_D3
          lda D_START_HI,y
          sta ZEROPAGE_POINTER_D3 + 1

          lda ZEROPAGE_POINTER_H3
          clc
          adc #1
          sta ZEROPAGE_POINTER_H3
          bcc +
          inc ZEROPAGE_POINTER_H3 + 1
+

          lda ZEROPAGE_POINTER_L3
          clc
          adc #1
          sta ZEROPAGE_POINTER_L3
          bcc +
          inc ZEROPAGE_POINTER_L3 + 1
+
          lda ZEROPAGE_POINTER_D3
          clc
          adc #1
          sta ZEROPAGE_POINTER_D3
          bcc +
          inc ZEROPAGE_POINTER_D3 + 1
+
          jmp .Goto410


IncI
          inc ZEROPAGE_POINTER_D
          bne +
          inc ZEROPAGE_POINTER_D + 1
+
          inc ZEROPAGE_POINTER_H
          bne +
          inc ZEROPAGE_POINTER_H + 1
+
          inc ZEROPAGE_POINTER_L
          bne +
          inc ZEROPAGE_POINTER_L + 1
+
          rts


I
          !byte 0

J
          !byte 0

K
          !byte 0

DR
          !byte 0

N_LO
          !byte 0
N_HI
          !byte 0

HF
          !byte 0

LF
          !byte 0


;20 DIM H(500,2),L(500,2),D(500,2)
H
          !fill 500 * 3

L
          !fill 500 * 3

D
          !fill 500 * 3


H_START_LO
          !byte <H
          !byte <( H + 500 )
          !byte <( H + 500 * 2 )

H_START_HI
          !byte >H
          !byte >( H + 500 )
          !byte >( H + 500 * 2 )

L_START_LO
          !byte <L
          !byte <( L + 500 )
          !byte <( L + 500 * 2 )

L_START_HI
          !byte >L
          !byte >( L + 500 )
          !byte >( L + 500 * 2 )

D_START_LO
          !byte <D
          !byte <( D + 500 )
          !byte <( D + 500 * 2 )

D_START_HI
          !byte >D
          !byte >( D + 500 )
          !byte >( D + 500 * 2 )
