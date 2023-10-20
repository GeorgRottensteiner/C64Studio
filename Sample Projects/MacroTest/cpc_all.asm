!cpu z80

* = $00

;LD r,r';
          ld b,b'    ;40
          ld b,c'    ;41
          ld b,d'
          ld b,e'
          ld b,h'
          ld b,l'    ;45
          ld b,a'    ;47

          ld c,b'    ;48
          ld c,c'
          ld c,d'
          ld c,e'
          ld c,h'
          ld c,l'
          ld c,a'

          ld d,b'
          ld d,c'
          ld d,d'
          ld d,e'
          ld d,h'
          ld d,l'
          ld d,a'

          ld e,b'
          ld e,c'
          ld e,d'
          ld e,e'
          ld e,h'
          ld e,l'
          ld e,a'

          ld h,b'
          ld h,c'
          ld h,d'
          ld h,e'
          ld h,h'
          ld h,l'
          ld h,a'

          ld l,b'
          ld l,c'
          ld l,d'
          ld l,e'
          ld l,h'
          ld l,l'
          ld l,a'

          ld a,b'
          ld a,c'
          ld a,d'
          ld a,e'
          ld a,h'
          ld a,l'    ;7d
          ld a,a'    ;7f

;LD r,n

          ld b,0    ;06 00
          ld c,1    ;0e 01
          ld d,2    ;16 02
          ld e,3    ;1e 03
          ld h,$fd  ;26 fd
          ld l,$fe  ;2e fe
          ld a,$ff  ;3e ff

          ld b,LATE_00  ;06 00
          ld c,LATE_01  ;0e 01
          ld d,LATE_02  ;16 02
          ld e,LATE_03  ;1e 03
          ld h,LATE_FD  ;26 fd
          ld l,LATE_FE  ;2e fe
          ld a,LATE_FF  ;3e ff

;LD r,(HL)
          ld b,(HL) ;46
          ld c,(HL) ;4e
          ld d,(HL) ;56
          ld e,(HL) ;5e
          ld h,(HL) ;66
          ld l,(HL) ;6e
          ld a,(HL) ;7e

;LD r,(IX+d)
          ld b,(IX + 0)        ;DD 46 00
          ld c,(IX + 1)        ;DD 4E 01
          ld d,(IX + 2)        ;DD 56 02
          ld e,(IX + 3)        ;DD 5E 03
          ld h,(IX + 0xfd)     ;DD 66 FD
          ld l,(IX + 0xfe)     ;DD 6E FE
          ld a,(IX + 0xff)     ;DD 7E FF

          ld b,(IX + LATE_00)  ;DD 46 00
          ld c,(IX + LATE_01)  ;DD 4E 01
          ld d,(IX + LATE_02)  ;DD 56 02
          ld e,(IX + LATE_03)  ;DD 5E 03
          ld h,(IX + LATE_FD)  ;DD 66 FD
          ld l,(IX + LATE_FE)  ;DD 6E FE
          ld a,(IX + LATE_FF)  ;DD 7E FF

;LD r,(IY+d)
          ld b,(IY + 0)   ;FD 46 00
          ld c,(IY + 1)   ;FD 4E 01
          ld d,(IY + 2)   ;FD 56 02
          ld e,(IY + 3)   ;FD 5E 03
          ld h,(IY + 4)   ;FD 66 04
          ld l,(IY + 5)   ;FD 6E 05
          ld a,(IY + 6)   ;FD 7E 06

          ld b,(IY + LATE_00)  ;FD 46 00
          ld c,(IY + LATE_01)  ;FD 4E 01
          ld d,(IY + LATE_02)  ;FD 56 02
          ld e,(IY + LATE_03)  ;FD 5E 03
          ld h,(IY + LATE_FD)  ;FD 66 FD
          ld l,(IY + LATE_FE)  ;FD 6E FE
          ld a,(IY + LATE_FF)  ;FD 7E FF


;LD (HL),r
          ld (HL),b       ;70
          ld (HL),c       ;71
          ld (HL),d       ;72
          ld (HL),e       ;73
          ld (HL),h       ;74
          ld (HL),l       ;75
          ld (HL),a       ;77

;LD (IX+d),r
          ld (IX + 0),b     ;DD 70 00
          ld (IX + 1),c     ;DD 71 01
          ld (IX + 2),d     ;DD 72 02
          ld (IX + 3),e     ;DD 73 03
          ld (IX + $fd),h   ;DD 74 FD
          ld (IX + $fe),l   ;DD 75 FE
          ld (IX + $ff),a   ;DD 77 FF

          ld (IX + LATE_00),b  ;DD 70 00
          ld (IX + LATE_01),c  ;DD 71 01
          ld (IX + LATE_02),d  ;DD 72 02
          ld (IX + LATE_03),e  ;DD 73 03
          ld (IX + LATE_FD),h  ;DD 74 FD
          ld (IX + LATE_FE),l  ;DD 75 FE
          ld (IX + LATE_FF),a  ;DD 77 FF

;LD (IY+d),r
          ld (IY + 0),b     ;FD 70 00
          ld (IY + 1),c     ;FD 71 01
          ld (IY + 2),d     ;FD 72 02
          ld (IY + 3),e     ;FD 73 03
          ld (IY + $fd),h   ;FD 74 FD
          ld (IY + $fe),l   ;FD 75 FE
          ld (IY + $ff),a   ;FD 77 FF

          ld (IY + LATE_00),b  ;FD 70 00
          ld (IY + LATE_01),c  ;FD 71 01
          ld (IY + LATE_02),d  ;FD 72 02
          ld (IY + LATE_03),e  ;FD 73 03
          ld (IY + LATE_FD),h  ;FD 74 FD
          ld (IY + LATE_FE),l  ;FD 75 FE
          ld (IY + LATE_FF),a  ;FD 77 FF


;LD (HL),n
          ld (HL),0       ;36 00
          ld (HL),1       ;36 01
          ld (HL),2       ;36 02
          ld (HL),3       ;36 03
          ld (HL),$fd     ;36 FD
          ld (HL),$fe     ;36 FE
          ld (HL),$ff     ;36 FF

;LD (IX+d),n
          ld ( IX + 0 ), $ff    ;DD3600FF
          ld ( IX + 1 ), $fe    ;DD3601FE
          ld ( IX + 2 ), $fd    ;DD3602FD
          ld ( IX + 3 ), 3      ;DD360303
          ld ( IX + $fd ), 2    ;DD36FD02
          ld ( IX + $fe ), 1    ;DD36FE01
          ld ( IX + $ff ), 0    ;DD36FF00

          ld ( IX + LATE_00 ), $ff  ;DD3600FF
          ld ( IX + LATE_01 ), $fe  ;DD3601FE
          ld ( IX + LATE_02 ), $fd  ;DD3602FD
          ld ( IX + LATE_03 ), 3    ;DD360303
          ld ( IX + LATE_FD ), 2    ;DD36FD02
          ld ( IX + LATE_FE ), 1    ;DD36FE01
          ld ( IX + LATE_FF ), 0    ;DD36FF00

;LD (IY+d),n
          ld ( IY + 0 ), $ff    ;FD3600FF
          ld ( IY + 1 ), $fe    ;FD3601FE
          ld ( IY + 2 ), $fd    ;FD3602FD
          ld ( IY + 3 ), 3      ;FD360303
          ld ( IY + $fd ), 2    ;FD36FD02
          ld ( IY + $fe ), 1    ;FD36FE01
          ld ( IY + $ff ), 0    ;FD36FF00

          ld ( IY + LATE_00 ), $ff  ;FD3600FF
          ld ( IY + LATE_01 ), $fe  ;FD3601FE
          ld ( IY + LATE_02 ), $fd  ;FD3602FD
          ld ( IY + LATE_03 ), 3    ;FD360303
          ld ( IY + LATE_FD ), 2    ;FD36FD02
          ld ( IY + LATE_FE ), 1    ;FD36FE01
          ld ( IY + LATE_FF ), 0    ;FD36FF00

          ld A,(BC)                ;0A
          ld A,(DE)                ;1A

;LD A,(nn)
          ld A,($0000)              ;3A0000
          ld A,($0001)              ;3A0100
          ld A,($00ff)              ;3AFF00
          ld A,($0100)              ;3A0001
          ld A,($ff00)              ;3A00FF
          ld A,($bbcc)              ;3ACCBB

          ld A,(LATE_00)            ;3A0000
          ld A,(LATE_01)            ;3A0100
          ld A,(LATE_FF)            ;3AFF00
          ld A,(LATE_0100)          ;3A0001
          ld A,(LATE_FF00)          ;3A00FF
          ld A,(LATE_BBCC)          ;3ACCBB

          ld (BC),A                 ;02
          ld (DE),A                 ;12

;LD (nn),A
          ld ($0000),A              ;320000
          ld ($0001),A              ;320100
          ld ($00ff),A              ;32FF00
          ld ($0100),A              ;320001
          ld ($ff00),A              ;3200FF
          ld ($bbcc),A              ;32CCBB

          ld (LATE_00),A            ;320000
          ld (LATE_01),A            ;320100
          ld (LATE_FF),A            ;32FF00
          ld (LATE_0100),A          ;320001
          ld (LATE_FF00),A          ;3200FF
          ld (LATE_BBCC),A          ;32CCBB

          ld A,I                    ;ED 57
          ld A,R                    ;ED 5F
          ld I,A                    ;ED 47
          ld R,A                    ;ED 4F

;LD dd,nn
          ld BC,$0001               ;01 01 00
          ld DE,$00ff               ;11 FF 00
          ld HL,$0100               ;21 00 01
          ld SP,$ff00               ;31 00 FF

          ld BC,LATE_01             ;01 01 00
          ld DE,LATE_FF             ;11 FF 00
          ld HL,LATE_0100           ;21 00 01
          ld SP,LATE_FF00           ;31 00 FF

;LD IX,nn
          ld IX,$0001               ;DD 21 01 00
          ld IX,$00ff               ;DD 21 FF 00
          ld IX,$0100               ;DD 21 00 01
          ld IX,$ff00               ;DD 21 00 FF

          ld IX,LATE_01             ;DD 21 01 00
          ld IX,LATE_FF             ;DD 21 FF 00
          ld IX,LATE_0100           ;DD 21 00 01
          ld IX,LATE_FF00           ;DD 21 00 FF

;LD IY,nn
          ld IY,$0001               ;FD 21 01 00
          ld IY,$00ff               ;FD 21 FF 00
          ld IY,$0100               ;FD 21 00 01
          ld IY,$ff00               ;FD 21 00 FF

          ld IY,LATE_01             ;FD 21 01 00
          ld IY,LATE_FF             ;FD 21 FF 00
          ld IY,LATE_0100           ;FD 21 00 01
          ld IY,LATE_FF00           ;FD 21 00 FF

;LD HL,(nn)
          ld HL,($0001)             ;2A 01 00
          ld HL,($00ff)             ;2A FF 00
          ld HL,($0100)             ;2A 00 01
          ld HL,($ff00)             ;2A 00 FF

          ld HL,(LATE_01)           ;2A 01 00
          ld HL,(LATE_FF)           ;2A FF 00
          ld HL,(LATE_0100)         ;2A 00 01
          ld HL,(LATE_FF00)         ;2A 00 FF

;LD dd,(nn)
          ld BC,($0001)             ;ED 4B 01 00
          ld DE,($00ff)             ;ED 5B FF 00
          ld HL,($0100)             ;2A 00 01  <- is not using LD dd,(nn) form
          ld SP,($ff00)             ;ED 7B 00 FF

          ld BC,(LATE_01)           ;ED 4B 01 00
          ld DE,(LATE_FF)           ;ED 5B FF 00
          ld HL,(LATE_0100)         ;2A 00 01  <- is not using LD dd,(nn) form
          ld SP,(LATE_FF00)         ;ED 7B 00 FF

;LD IX,(nn)
          ld IX,($0001)             ;DD 2A 01 00
          ld IX,($00ff)             ;DD 2A FF 00
          ld IX,($0100)             ;DD 2A 00 01
          ld IX,($ff00)             ;DD 2A 00 FF

          ld IX,(LATE_01)           ;DD 2A 01 00
          ld IX,(LATE_FF)           ;DD 2A FF 00
          ld IX,(LATE_0100)         ;DD 2A 00 01
          ld IX,(LATE_FF00)         ;DD 2A 00 FF

;LD IY,(nn)
          ld IY,($0001)             ;FD 2A 01 00
          ld IY,($00ff)             ;FD 2A FF 00
          ld IY,($0100)             ;FD 2A 00 01
          ld IY,($ff00)             ;FD 2A 00 FF

          ld IY,(LATE_01)           ;FD 2A 01 00
          ld IY,(LATE_FF)           ;FD 2A FF 00
          ld IY,(LATE_0100)         ;FD 2A 00 01
          ld IY,(LATE_FF00)         ;FD 2A 00 FF

;LD (nn),HL
          ld ($0001),HL             ;22 01 00
          ld ($00ff),HL             ;22 FF 00
          ld ($0100),HL             ;22 00 01
          ld ($ff00),HL             ;22 00 FF

          ld (LATE_01),HL           ;22 01 00
          ld (LATE_FF),HL           ;22 FF 00
          ld (LATE_0100),HL         ;22 00 01
          ld (LATE_FF00),HL         ;22 00 FF

;LD (nn),dd
          ld ($0001),BC             ;ED 43 01 00
          ld ($00ff),DE             ;ED 53 FF 00
          ld ($0100),HL             ;22 00 01  <- is not using LD (nn),dd form
          ld ($ff00),SP             ;ED 73 00 FF

          ld (LATE_01),BC           ;ED 43 01 00
          ld (LATE_FF),DE           ;ED 53 FF 00
          ld (LATE_0100),HL         ;22 00 01  <- is not using LD (nn),dd form
          ld (LATE_FF00),SP         ;ED 73 00 FF

;LD (nn),IX
          ld ($0001),IX             ;DD 22 01 00
          ld ($00ff),IX             ;DD 22 FF 00
          ld ($0100),IX             ;DD 22 00 01
          ld ($ff00),IX             ;DD 22 00 FF

          ld (LATE_01),IX           ;DD 22 01 00
          ld (LATE_FF),IX           ;DD 22 FF 00
          ld (LATE_0100),IX         ;DD 22 00 01
          ld (LATE_FF00),IX         ;DD 22 00 FF

;LD (nn),IY
          ld ($0001),IY             ;FD 22 01 00
          ld ($00ff),IY             ;FD 22 FF 00
          ld ($0100),IY             ;FD 22 00 01
          ld ($ff00),IY             ;FD 22 00 FF

          ld (LATE_01),IY           ;FD 22 01 00
          ld (LATE_FF),IY           ;FD 22 FF 00
          ld (LATE_0100),IY         ;FD 22 00 01
          ld (LATE_FF00),IY         ;FD 22 00 FF

          ld SP,HL                  ;F9
          ld SP,ix                  ;DDF9
          ld SP,iy                  ;FDF9


;PUSH qq
          push BC                   ;C5
          push DE                   ;D5
          push HL                   ;E5
          push AF                   ;F5

          push IX                   ;DD E5
          push IY                   ;FD E5

          pop BC                    ;C1
          pop DE                    ;D1
          pop HL                    ;E1
          pop AF                    ;F1

          pop ix                    ;DD E1
          pop iy                    ;FD E1

          ex DE,HL                  ;EB
          ex AF,AF'                 ;08
          exx                       ;D9

          ex (SP),HL                ;E3
          ex (SP),IX                ;DD E3
          ex (SP),IY                ;FD E3

          ldi                       ;ED A0
          ldir                      ;ED B0
          ldd                       ;ED A8
          lddr                      ;ED B8

          cpi                       ;ED A1
          cpir                      ;ED B1
          cpd                       ;ED A9
          cpdr                      ;ED B9

;ADD A,r
          add A,b                   ;80
          add A,c                   ;81
          add A,d                   ;82
          add A,e                   ;83
          add A,h                   ;84
          add A,l                   ;85
          add A,a                   ;87

;ADD A,n
          add A,$01                 ;C6 01
          add A,$02                 ;C6 02
          add A,$FE                 ;C6 FE
          add A,$FF                 ;C6 FF

          add A,LATE_01             ;C6 01
          add A,LATE_02             ;C6 02
          add A,LATE_FE             ;C6 FE
          add A,LATE_FF             ;C6 FF

          add A,(HL)                ;86

;ADD A,(IX+d)
          add A,(IX + $00)          ;DD 86 00
          add A,(IX + $01)          ;DD 86 01
          add A,(IX + $FE)          ;DD 86 FE
          add A,(IX + $FD)          ;DD 86 FD

          add A,(IX + LATE_00)      ;DD 86 00
          add A,(IX + LATE_01)      ;DD 86 01
          add A,(IX + LATE_FE)      ;DD 86 FE
          add A,(IX + LATE_FD)      ;DD 86 FD

;ADD A,(IY+d)
          add A,(IY + $00)          ;FD 86 00
          add A,(IY + $01)          ;FD 86 01
          add A,(IY + $FE)          ;FD 86 FE
          add A,(IY + $FD)          ;FD 86 FD

          add A,(IY + LATE_00)      ;FD 86 00
          add A,(IY + LATE_01)      ;FD 86 01
          add A,(IY + LATE_FE)      ;FD 86 FE
          add A,(IY + LATE_FD)      ;FD 86 FD

;ADC A,r
          adc A,b                   ;88
          adc A,c                   ;89
          adc A,d                   ;8A
          adc A,e                   ;8B
          adc A,h                   ;8C
          adc A,l                   ;8D
          adc A,a                   ;8F

;ADC A,n
          adc A,$01                 ;CE 01
          adc A,$02                 ;CE 02
          adc A,$FE                 ;CE FE
          adc A,$FF                 ;CE FF

          adc A,LATE_01             ;CE 01
          adc A,LATE_02             ;CE 02
          adc A,LATE_FE             ;CE FE
          adc A,LATE_FF             ;CE FF

          adc A,(HL)                ;8E

;ADC A,(IX+d)
          adc A,(IX + $00)          ;DD 8E 00
          adc A,(IX + $01)          ;DD 8E 01
          adc A,(IX + $FE)          ;DD 8E FE
          adc A,(IX + $FD)          ;DD 8E FD

          adc A,(IX + LATE_00)      ;DD 8E 00
          adc A,(IX + LATE_01)      ;DD 8E 01
          adc A,(IX + LATE_FE)      ;DD 8E FE
          adc A,(IX + LATE_FD)      ;DD 8E FD

;ADC A,(IY+d)
          adc A,(IY + $00)          ;FD 8E 00
          adc A,(IY + $01)          ;FD 8E 01
          adc A,(IY + $FE)          ;FD 8E FE
          adc A,(IY + $FD)          ;FD 8E FD

          adc A,(IY + LATE_00)      ;FD 8E 00
          adc A,(IY + LATE_01)      ;FD 8E 01
          adc A,(IY + LATE_FE)      ;FD 8E FE
          adc A,(IY + LATE_FD)      ;FD 8E FD

;sub r
          sub b                     ;90
          sub c                     ;91
          sub d                     ;92
          sub e                     ;93
          sub h                     ;94
          sub l                     ;95
          sub a                     ;97

;sub n
          sub $01                   ;D6 01
          sub $02                   ;D6 02
          sub $FE                   ;D6 FE
          sub $FF                   ;D6 FF

          sub LATE_01               ;D6 01
          sub LATE_02               ;D6 02
          sub LATE_FE               ;D6 FE
          sub LATE_FF               ;D6 FF

          sub (HL)                  ;96

;SUB (IX+d)
          sub (IX + $00)            ;DD 96 00
          sub (IX + $01)            ;DD 96 01
          sub (IX + $FE)            ;DD 96 FE
          sub (IX + $FD)            ;DD 96 FD

          sub (IX + LATE_00)        ;DD 96 00
          sub (IX + LATE_01)        ;DD 96 01
          sub (IX + LATE_FE)        ;DD 96 FE
          sub (IX + LATE_FD)        ;DD 96 FD

;sub (IY+d)
          sub (IY + $00)            ;FD 96 00
          sub (IY + $01)            ;FD 96 01
          sub (IY + $FE)            ;FD 96 FE
          sub (IY + $FD)            ;FD 96 FD

          sub (IY + LATE_00)        ;FD 96 00
          sub (IY + LATE_01)        ;FD 96 01
          sub (IY + LATE_FE)        ;FD 96 FE
          sub (IY + LATE_FD)        ;FD 96 FD


;sbc A,r
          sbc A,b                   ;98
          sbc A,c                   ;99
          sbc A,d                   ;9A
          sbc A,e                   ;9B
          sbc A,h                   ;9C
          sbc A,l                   ;9D
          sbc A,a                   ;9F

;sbc A,n
          sbc A,$01                 ;DE 01
          sbc A,$02                 ;DE 02
          sbc A,$FE                 ;DE FE
          sbc A,$FF                 ;DE FF

          sbc A,LATE_01             ;DE 01
          sbc A,LATE_02             ;DE 02
          sbc A,LATE_FE             ;DE FE
          sbc A,LATE_FF             ;DE FF

          sbc A,(HL)                ;9E

;sbc A,(IX+d)
          sbc A,(IX + $00)          ;DD 9E 00
          sbc A,(IX + $01)          ;DD 9E 01
          sbc A,(IX + $FE)          ;DD 9E FE
          sbc A,(IX + $FD)          ;DD 9E FD

          sbc A,(IX + LATE_00)      ;DD 9E 00
          sbc A,(IX + LATE_01)      ;DD 9E 01
          sbc A,(IX + LATE_FE)      ;DD 9E FE
          sbc A,(IX + LATE_FD)      ;DD 9E FD

;sbc A,(IY+d)
          sbc A,(IY + $00)          ;FD 9E 00
          sbc A,(IY + $01)          ;FD 9E 01
          sbc A,(IY + $FE)          ;FD 9E FE
          sbc A,(IY + $FD)          ;FD 9E FD

          sbc A,(IY + LATE_00)      ;FD 9E 00
          sbc A,(IY + LATE_01)      ;FD 9E 01
          sbc A,(IY + LATE_FE)      ;FD 9E FE
          sbc A,(IY + LATE_FD)      ;FD 9E FD


;and r
          and b                     ;A0
          and c                     ;A1
          and d                     ;A2
          and e                     ;A3
          and h                     ;A4
          and l                     ;A5
          and a                     ;A7

;and n
          and $01                   ;E6 01
          and $02                   ;E6 02
          and $FE                   ;E6 FE
          and $FF                   ;E6 FF

          and LATE_01               ;E6 01
          and LATE_02               ;E6 02
          and LATE_FE               ;E6 FE
          and LATE_FF               ;E6 FF

          and (HL)                  ;A6

;and (IX+d)
          and (IX + $00)            ;DD A6 00
          and (IX + $01)            ;DD A6 01
          and (IX + $FE)            ;DD A6 FE
          and (IX + $FD)            ;DD A6 FD

          and (IX + LATE_00)        ;DD A6 00
          and (IX + LATE_01)        ;DD A6 01
          and (IX + LATE_FE)        ;DD A6 FE
          and (IX + LATE_FD)        ;DD A6 FD

;and (IY+d)
          and (IY + $00)            ;FD A6 00
          and (IY + $01)            ;FD A6 01
          and (IY + $FE)            ;FD A6 FE
          and (IY + $FD)            ;FD A6 FD

          and (IY + LATE_00)        ;FD A6 00
          and (IY + LATE_01)        ;FD A6 01
          and (IY + LATE_FE)        ;FD A6 FE
          and (IY + LATE_FD)        ;FD A6 FD


;or r
          or b                      ;B0
          or c                      ;B1
          or d                      ;B2
          or e                      ;B3
          or h                      ;B4
          or l                      ;B5
          or a                      ;B7

;or n
          or $01                    ;F6 01
          or $02                    ;F6 02
          or $FE                    ;F6 FE
          or $FF                    ;F6 FF

          or LATE_01                ;F6 01
          or LATE_02                ;F6 02
          or LATE_FE                ;F6 FE
          or LATE_FF                ;F6 FF

          or (HL)                   ;B6

;or (IX+d)
          or (IX + $00)             ;DD B6 00
          or (IX + $01)             ;DD B6 01
          or (IX + $FE)             ;DD B6 FE
          or (IX + $FD)             ;DD B6 FD

          or (IX + LATE_00)         ;DD B6 00
          or (IX + LATE_01)         ;DD B6 01
          or (IX + LATE_FE)         ;DD B6 FE
          or (IX + LATE_FD)         ;DD B6 FD

;or (IY+d)
          or (IY + $00)             ;FD B6 00
          or (IY + $01)             ;FD B6 01
          or (IY + $FE)             ;FD B6 FE
          or (IY + $FD)             ;FD B6 FD

          or (IY + LATE_00)         ;FD B6 00
          or (IY + LATE_01)         ;FD B6 01
          or (IY + LATE_FE)         ;FD B6 FE
          or (IY + LATE_FD)         ;FD B6 FD


;xor r
          xor b                     ;A8
          xor c                     ;A9
          xor d                     ;AA
          xor e                     ;AB
          xor h                     ;AC
          xor l                     ;AD
          xor a                     ;AF

;xor n
          xor $01                   ;EE 01
          xor $02                   ;EE 02
          xor $FE                   ;EE FE
          xor $FF                   ;EE FF

          xor LATE_01               ;EE 01
          xor LATE_02               ;EE 02
          xor LATE_FE               ;EE FE
          xor LATE_FF               ;EE FF

          xor (HL)                  ;AE

;xor (IX+d)
          xor (IX + $00)            ;DD AE 00
          xor (IX + $01)            ;DD AE 01
          xor (IX + $FE)            ;DD AE FE
          xor (IX + $FD)            ;DD AE FD

          xor (IX + LATE_00)        ;DD AE 00
          xor (IX + LATE_01)        ;DD AE 01
          xor (IX + LATE_FE)        ;DD AE FE
          xor (IX + LATE_FD)        ;DD AE FD

;xor (IY+d)
          xor (IY + $00)            ;FD AE 00
          xor (IY + $01)            ;FD AE 01
          xor (IY + $FE)            ;FD AE FE
          xor (IY + $FD)            ;FD AE FD

          xor (IY + LATE_00)        ;FD AE 00
          xor (IY + LATE_01)        ;FD AE 01
          xor (IY + LATE_FE)        ;FD AE FE
          xor (IY + LATE_FD)        ;FD AE FD


;cp r
          cp b                      ;B8
          cp c                      ;B9
          cp d                      ;BA
          cp e                      ;BB
          cp h                      ;BC
          cp l                      ;BD
          cp a                      ;BF

;cp n
          cp $01                    ;FE 01
          cp $02                    ;FE 02
          cp $FE                    ;FE FE
          cp $FF                    ;FE FF

          cp LATE_01                ;FE 01
          cp LATE_02                ;FE 02
          cp LATE_FE                ;FE FE
          cp LATE_FF                ;FE FF

          cp (HL)                   ;BE

;cp (IX+d)
          cp (IX + $00)             ;DD BE 00
          cp (IX + $01)             ;DD BE 01
          cp (IX + $FE)             ;DD BE FE
          cp (IX + $FD)             ;DD BE FD

          cp (IX + LATE_00)         ;DD BE 00
          cp (IX + LATE_01)         ;DD BE 01
          cp (IX + LATE_FE)         ;DD BE FE
          cp (IX + LATE_FD)         ;DD BE FD

;cp (IY+d)
          cp (IY + $00)             ;FD BE 00
          cp (IY + $01)             ;FD BE 01
          cp (IY + $FE)             ;FD BE FE
          cp (IY + $FD)             ;FD BE FD

          cp (IY + LATE_00)         ;FD BE 00
          cp (IY + LATE_01)         ;FD BE 01
          cp (IY + LATE_FE)         ;FD BE FE
          cp (IY + LATE_FD)         ;FD BE FD


;inc r
          inc b                     ;04
          inc c                     ;0C
          inc d                     ;14
          inc e                     ;1C
          inc h                     ;24
          inc l                     ;2C
          inc a                     ;3C

          inc (HL)                  ;34

;inc (IX+d)
          inc (IX + $00)            ;DD 34 00
          inc (IX + $01)            ;DD 34 01
          inc (IX + $FE)            ;DD 34 FE
          inc (IX + $FD)            ;DD 34 FD

          inc (IX + LATE_00)        ;DD 34 00
          inc (IX + LATE_01)        ;DD 34 01
          inc (IX + LATE_FE)        ;DD 34 FE
          inc (IX + LATE_FD)        ;DD 34 FD

;inc (IY+d)
          inc (IY + $00)            ;FD 34 00
          inc (IY + $01)            ;FD 34 01
          inc (IY + $FE)            ;FD 34 FE
          inc (IY + $FD)            ;FD 34 FD

          inc (IY + LATE_00)        ;FD 34 00
          inc (IY + LATE_01)        ;FD 34 01
          inc (IY + LATE_FE)        ;FD 34 FE
          inc (IY + LATE_FD)        ;FD 34 FD


;dec r
          dec b                     ;05
          dec c                     ;0D
          dec d                     ;15
          dec e                     ;1D
          dec h                     ;25
          dec l                     ;2D
          dec a                     ;3D

          dec (HL)                  ;35

;dec (IX+d)
          dec (IX + $00)            ;DD 35 00
          dec (IX + $01)            ;DD 35 01
          dec (IX + $FE)            ;DD 35 FE
          dec (IX + $FD)            ;DD 35 FD

          dec (IX + LATE_00)        ;DD 35 00
          dec (IX + LATE_01)        ;DD 35 01
          dec (IX + LATE_FE)        ;DD 35 FE
          dec (IX + LATE_FD)        ;DD 35 FD

;dec (IY+d)
          dec (IY + $00)            ;FD 35 00
          dec (IY + $01)            ;FD 35 01
          dec (IY + $FE)            ;FD 35 FE
          dec (IY + $FD)            ;FD 35 FD

          dec (IY + LATE_00)        ;FD 35 00
          dec (IY + LATE_01)        ;FD 35 01
          dec (IY + LATE_FE)        ;FD 35 FE
          dec (IY + LATE_FD)        ;FD 35 FD

          daa                       ;27
          cpl                       ;2F
          neg                       ;ED 44
          ccf                       ;3F
          scf                       ;37
          nop                       ;00
          halt                      ;76
          di                        ;F3
          ei                        ;FB
          im 0                      ;ED 46
          im 1                      ;ED 56
          im 2                      ;ED 5E

;ADD HL,ss
          add HL,BC     ;09
          add HL,DE     ;19
          add HL,HL     ;29
          add HL,SP     ;39

;ADC HL,ss
          adc HL,BC     ;ED 4A
          adc HL,DE     ;ED 5A
          adc HL,HL     ;ED 6A
          adc HL,SP     ;ED 7A

;SBC HL,ss
          sbc HL,BC     ;ED 42
          sbc HL,DE     ;ED 52
          sbc HL,HL     ;ED 62
          sbc HL,SP     ;ED 72

;ADD IX,pp
          add IX,BC     ;DD 09
          add IX,DE     ;DD 19
          add IX,IX     ;DD 29
          add IX,SP     ;DD 39

;ADD IY,rr
          add IY,BC     ;FD 09
          add IY,DE     ;FD 19
          add IY,IY     ;FD 29
          add IY,SP     ;FD 39


LATE_FF = $ff
LATE_FE = $fe
LATE_FD = $fd
LATE_03 = $03
LATE_02 = $02
LATE_01 = $01
LATE_00 = $00
LATE_0100 = $0100
LATE_FF00 = $ff00
LATE_BBCC = $bbcc