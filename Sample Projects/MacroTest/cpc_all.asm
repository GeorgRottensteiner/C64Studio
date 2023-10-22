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

;same without apostrophe
          ld b,b    ;40
          ld b,c    ;41
          ld b,d
          ld b,e
          ld b,h
          ld b,l    ;45
          ld b,a    ;47

          ld c,b    ;48
          ld c,c
          ld c,d
          ld c,e
          ld c,h
          ld c,l
          ld c,a

          ld d,b
          ld d,c
          ld d,d
          ld d,e
          ld d,h
          ld d,l
          ld d,a

          ld e,b
          ld e,c
          ld e,d
          ld e,e
          ld e,h
          ld e,l
          ld e,a

          ld h,b
          ld h,c
          ld h,d
          ld h,e
          ld h,h
          ld h,l
          ld h,a

          ld l,b
          ld l,c
          ld l,d
          ld l,e
          ld l,h
          ld l,l
          ld l,a

          ld a,b
          ld a,c
          ld a,d
          ld a,e
          ld a,h
          ld a,l    ;7d
          ld a,a    ;7f

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

;INC ss
          inc BC        ;03
          inc DE        ;13
          inc HL        ;23
          inc SP        ;33

          inc IX        ;DD 23
          inc IY        ;FD 23

;DEC ss
          dec BC        ;0B
          dec DE        ;1B
          dec HL        ;2B
          dec SP        ;3B

          dec IX        ;DD 2B
          dec IY        ;FD 2B


          rlca          ;07
          rla           ;17
          rrca          ;0F
          rra           ;1F

;RLC r
          rlc b         ;CB 00
          rlc c         ;CB 01
          rlc d         ;CB 02
          rlc e         ;CB 03
          rlc h         ;CB 04
          rlc l         ;CB 05
          rlc a         ;CB 07

          rlc (HL)      ;CB 06

;RLC (IX+d)
          rlc (IX + $00)      ;DD CB 00 06
          rlc (IX + $01)      ;DD CB 01 06
          rlc (IX + $fe)      ;DD CB FE 06
          rlc (IX + $ff)      ;DD CB FF 06

          rlc (IX + LATE_00)  ;DD CB 00 06
          rlc (IX + LATE_01)  ;DD CB 01 06
          rlc (IX + LATE_FE)  ;DD CB FE 06
          rlc (IX + LATE_FF)  ;DD CB FF 06

          rlc (IY + $00)      ;FD CB 00 06
          rlc (IY + $01)      ;FD CB 01 06
          rlc (IY + $fe)      ;FD CB FE 06
          rlc (IY + $ff)      ;FD CB FF 06

          rlc (IY + LATE_00)  ;FD CB 00 06
          rlc (IY + LATE_01)  ;FD CB 01 06
          rlc (IY + LATE_FE)  ;FD CB FE 06
          rlc (IY + LATE_FF)  ;FD CB FF 06


;rl r
          rl b         ;CB 10
          rl c         ;CB 11
          rl d         ;CB 12
          rl e         ;CB 13
          rl h         ;CB 14
          rl l         ;CB 15
          rl a         ;CB 17

          rl (HL)      ;CB 16

;rl (IX+d)
          rl (IX + $00)      ;DD CB 00 16
          rl (IX + $01)      ;DD CB 01 16
          rl (IX + $fe)      ;DD CB FE 16
          rl (IX + $ff)      ;DD CB FF 16

          rl (IX + LATE_00)  ;DD CB 00 16
          rl (IX + LATE_01)  ;DD CB 01 16
          rl (IX + LATE_FE)  ;DD CB FE 16
          rl (IX + LATE_FF)  ;DD CB FF 16

          rl (IY + $00)      ;FD CB 00 16
          rl (IY + $01)      ;FD CB 01 16
          rl (IY + $fe)      ;FD CB FE 16
          rl (IY + $ff)      ;FD CB FF 16

          rl (IY + LATE_00)  ;FD CB 00 16
          rl (IY + LATE_01)  ;FD CB 01 16
          rl (IY + LATE_FE)  ;FD CB FE 16
          rl (IY + LATE_FF)  ;FD CB FF 16


;rrc r
          rrc b         ;CB 08
          rrc c         ;CB 09
          rrc d         ;CB 0A
          rrc e         ;CB 0B
          rrc h         ;CB 0C
          rrc l         ;CB 0D
          rrc a         ;CB 0F

          rrc (HL)      ;CB 0E

;rrc (IX+d)
          rrc (IX + $00)      ;DD CB 00 0E
          rrc (IX + $01)      ;DD CB 01 0E
          rrc (IX + $fe)      ;DD CB FE 0E
          rrc (IX + $ff)      ;DD CB FF 0E

          rrc (IX + LATE_00)  ;DD CB 00 0E
          rrc (IX + LATE_01)  ;DD CB 01 0E
          rrc (IX + LATE_FE)  ;DD CB FE 0E
          rrc (IX + LATE_FF)  ;DD CB FF 0E

          rrc (IY + $00)      ;FD CB 00 0E
          rrc (IY + $01)      ;FD CB 01 0E
          rrc (IY + $fe)      ;FD CB FE 0E
          rrc (IY + $ff)      ;FD CB FF 0E

          rrc (IY + LATE_00)  ;FD CB 00 0E
          rrc (IY + LATE_01)  ;FD CB 01 0E
          rrc (IY + LATE_FE)  ;FD CB FE 0E
          rrc (IY + LATE_FF)  ;FD CB FF 0E

;rr r
          rr b         ;CB 18
          rr c         ;CB 19
          rr d         ;CB 1A
          rr e         ;CB 1B
          rr h         ;CB 1C
          rr l         ;CB 1D
          rr a         ;CB 1F

          rr (HL)      ;CB 1E

;rr (IX+d)
          rr (IX + $00)      ;DD CB 00 1E
          rr (IX + $01)      ;DD CB 01 1E
          rr (IX + $fe)      ;DD CB FE 1E
          rr (IX + $ff)      ;DD CB FF 1E

          rr (IX + LATE_00)  ;DD CB 00 1E
          rr (IX + LATE_01)  ;DD CB 01 1E
          rr (IX + LATE_FE)  ;DD CB FE 1E
          rr (IX + LATE_FF)  ;DD CB FF 1E

          rr (IY + $00)      ;FD CB 00 1E
          rr (IY + $01)      ;FD CB 01 1E
          rr (IY + $fe)      ;FD CB FE 1E
          rr (IY + $ff)      ;FD CB FF 1E

          rr (IY + LATE_00)  ;FD CB 00 1E
          rr (IY + LATE_01)  ;FD CB 01 1E
          rr (IY + LATE_FE)  ;FD CB FE 1E
          rr (IY + LATE_FF)  ;FD CB FF 1E


;sla r
          sla b         ;CB 20
          sla c         ;CB 21
          sla d         ;CB 22
          sla e         ;CB 23
          sla h         ;CB 24
          sla l         ;CB 25
          sla a         ;CB 27

          sla (HL)      ;CB 26

;sla (IX+d)
          sla (IX + $00)      ;DD CB 00 26
          sla (IX + $01)      ;DD CB 01 26
          sla (IX + $fe)      ;DD CB FE 26
          sla (IX + $ff)      ;DD CB FF 26

          sla (IX + LATE_00)  ;DD CB 00 26
          sla (IX + LATE_01)  ;DD CB 01 26
          sla (IX + LATE_FE)  ;DD CB FE 26
          sla (IX + LATE_FF)  ;DD CB FF 26

          sla (IY + $00)      ;FD CB 00 26
          sla (IY + $01)      ;FD CB 01 26
          sla (IY + $fe)      ;FD CB FE 26
          sla (IY + $ff)      ;FD CB FF 26

          sla (IY + LATE_00)  ;FD CB 00 26
          sla (IY + LATE_01)  ;FD CB 01 26
          sla (IY + LATE_FE)  ;FD CB FE 26
          sla (IY + LATE_FF)  ;FD CB FF 26

;sra r
          sra b         ;CB 28
          sra c         ;CB 29
          sra d         ;CB 2A
          sra e         ;CB 2B
          sra h         ;CB 2C
          sra l         ;CB 2D
          sra a         ;CB 2F

          sra (HL)      ;CB 2E

;sra (IX+d)
          sra (IX + $00)      ;DD CB 00 2E
          sra (IX + $01)      ;DD CB 01 2E
          sra (IX + $fe)      ;DD CB FE 2E
          sra (IX + $ff)      ;DD CB FF 2E

          sra (IX + LATE_00)  ;DD CB 00 2E
          sra (IX + LATE_01)  ;DD CB 01 2E
          sra (IX + LATE_FE)  ;DD CB FE 2E
          sra (IX + LATE_FF)  ;DD CB FF 2E

          sra (IY + $00)      ;FD CB 00 2E
          sra (IY + $01)      ;FD CB 01 2E
          sra (IY + $fe)      ;FD CB FE 2E
          sra (IY + $ff)      ;FD CB FF 2E

          sra (IY + LATE_00)  ;FD CB 00 2E
          sra (IY + LATE_01)  ;FD CB 01 2E
          sra (IY + LATE_FE)  ;FD CB FE 2E
          sra (IY + LATE_FF)  ;FD CB FF 2E


;srl r
          srl b         ;CB 38
          srl c         ;CB 39
          srl d         ;CB 3A
          srl e         ;CB 3B
          srl h         ;CB 3C
          srl l         ;CB 3D
          srl a         ;CB 3F

          srl (HL)      ;CB 3E

;srl (IX+d)
          srl (IX + $00)      ;DD CB 00 3E
          srl (IX + $01)      ;DD CB 01 3E
          srl (IX + $fe)      ;DD CB FE 3E
          srl (IX + $ff)      ;DD CB FF 3E

          srl (IX + LATE_00)  ;DD CB 00 3E
          srl (IX + LATE_01)  ;DD CB 01 3E
          srl (IX + LATE_FE)  ;DD CB FE 3E
          srl (IX + LATE_FF)  ;DD CB FF 3E

          srl (IY + $00)      ;FD CB 00 3E
          srl (IY + $01)      ;FD CB 01 3E
          srl (IY + $fe)      ;FD CB FE 3E
          srl (IY + $ff)      ;FD CB FF 3E

          srl (IY + LATE_00)  ;FD CB 00 3E
          srl (IY + LATE_01)  ;FD CB 01 3E
          srl (IY + LATE_FE)  ;FD CB FE 3E
          srl (IY + LATE_FF)  ;FD CB FF 3E


          rld           ;ED 6F
          rrd           ;ED 67

;BIT b,r
          bit $00,b         ;CB 40
          bit $01,c         ;CB 49
          bit $02,d         ;CB 52
          bit $03,e         ;CB 5B
          bit $04,h         ;CB 64
          bit $05,l         ;CB 6D
          bit $06,a         ;CB 77
          bit $07,a         ;CB 7F

          bit LATE_00,b     ;CB 40
          bit LATE_01,c     ;CB 49
          bit LATE_02,d     ;CB 52
          bit LATE_03,e     ;CB 5B
          bit LATE_04,h     ;CB 64
          bit LATE_05,l     ;CB 6D
          bit LATE_06,a     ;CB 77
          bit LATE_07,a     ;CB 7F

          bit $00,(HL)      ;CB 46
          bit $01,(HL)      ;CB 4E
          bit $02,(HL)      ;CB 56
          bit $03,(HL)      ;CB 5E
          bit $04,(HL)      ;CB 66
          bit $05,(HL)      ;CB 6E
          bit $06,(HL)      ;CB 76
          bit $07,(HL)      ;CB 7E

          bit LATE_00,(HL)  ;CB 46
          bit LATE_01,(HL)  ;CB 4E
          bit LATE_02,(HL)  ;CB 56
          bit LATE_03,(HL)  ;CB 5E
          bit LATE_04,(HL)  ;CB 66
          bit LATE_05,(HL)  ;CB 6E
          bit LATE_06,(HL)  ;CB 76
          bit LATE_07,(HL)  ;CB 7E

;BIT b,(IX+d)
          bit $00,(IX+$FF)  ;DD CB FF 46
          bit $01,(IX+$FE)  ;DD CB FE 4E
          bit $02,(IX+$FD)  ;DD CB FD 56
          bit $03,(IX+$04)  ;DD CB 04 5E
          bit $04,(IX+$03)  ;DD CB 03 66
          bit $05,(IX+$02)  ;DD CB 02 6E
          bit $06,(IX+$01)  ;DD CB 01 76
          bit $07,(IX+$00)  ;DD CB 00 7E

          bit LATE_00,(IX+LATE_FF)  ;DD CB FF 46
          bit LATE_01,(IX+LATE_FE)  ;DD CB FE 4E
          bit LATE_02,(IX+LATE_FD)  ;DD CB FD 56
          bit LATE_03,(IX+LATE_04)  ;DD CB 04 5E
          bit LATE_04,(IX+LATE_03)  ;DD CB 03 66
          bit LATE_05,(IX+LATE_02)  ;DD CB 02 6E
          bit LATE_06,(IX+LATE_01)  ;DD CB 01 76
          bit LATE_07,(IX+LATE_00)  ;DD CB 00 7E

;BIT b,(IY+d)
          bit $00,(IY+$FF)  ;FD CB FF 46
          bit $01,(IY+$FE)  ;FD CB FE 4E
          bit $02,(IY+$FD)  ;FD CB FD 56
          bit $03,(IY+$04)  ;FD CB 04 5E
          bit $04,(IY+$03)  ;FD CB 03 66
          bit $05,(IY+$02)  ;FD CB 02 6E
          bit $06,(IY+$01)  ;FD CB 01 76
          bit $07,(IY+$00)  ;FD CB 00 7E

          bit LATE_00,(IY+LATE_FF)  ;FD CB FF 46
          bit LATE_01,(IY+LATE_FE)  ;FD CB FE 4E
          bit LATE_02,(IY+LATE_FD)  ;FD CB FD 56
          bit LATE_03,(IY+LATE_04)  ;FD CB 04 5E
          bit LATE_04,(IY+LATE_03)  ;FD CB 03 66
          bit LATE_05,(IY+LATE_02)  ;FD CB 02 6E
          bit LATE_06,(IY+LATE_01)  ;FD CB 01 76
          bit LATE_07,(IY+LATE_00)  ;FD CB 00 7E


;set b,r
          set $00,b         ;CB C0
          set $01,c         ;CB C9
          set $02,d         ;CB D2
          set $03,e         ;CB DB
          set $04,h         ;CB E4
          set $05,l         ;CB ED
          set $06,a         ;CB F7
          set $07,a         ;CB FF

          set LATE_00,b     ;CB C0
          set LATE_01,c     ;CB C9
          set LATE_02,d     ;CB D2
          set LATE_03,e     ;CB DB
          set LATE_04,h     ;CB E4
          set LATE_05,l     ;CB ED
          set LATE_06,a     ;CB F7
          set LATE_07,a     ;CB FF

          set $00,(HL)      ;CB C6
          set $01,(HL)      ;CB CE
          set $02,(HL)      ;CB D6
          set $03,(HL)      ;CB DE
          set $04,(HL)      ;CB E6
          set $05,(HL)      ;CB EE
          set $06,(HL)      ;CB F6
          set $07,(HL)      ;CB FE

          set LATE_00,(HL)  ;CB C6
          set LATE_01,(HL)  ;CB CE
          set LATE_02,(HL)  ;CB D6
          set LATE_03,(HL)  ;CB DE
          set LATE_04,(HL)  ;CB E6
          set LATE_05,(HL)  ;CB EE
          set LATE_06,(HL)  ;CB F6
          set LATE_07,(HL)  ;CB FE

;set b,(IX+d)
          set $00,(IX+$FF)  ;DD CB FF C6
          set $01,(IX+$FE)  ;DD CB FE CE
          set $02,(IX+$FD)  ;DD CB FD D6
          set $03,(IX+$04)  ;DD CB 04 DE
          set $04,(IX+$03)  ;DD CB 03 E6
          set $05,(IX+$02)  ;DD CB 02 EE
          set $06,(IX+$01)  ;DD CB 01 F6
          set $07,(IX+$00)  ;DD CB 00 FE

          set LATE_00,(IX+LATE_FF)  ;DD CB FF C6
          set LATE_01,(IX+LATE_FE)  ;DD CB FE CE
          set LATE_02,(IX+LATE_FD)  ;DD CB FD D6
          set LATE_03,(IX+LATE_04)  ;DD CB 04 DE
          set LATE_04,(IX+LATE_03)  ;DD CB 03 E6
          set LATE_05,(IX+LATE_02)  ;DD CB 02 EE
          set LATE_06,(IX+LATE_01)  ;DD CB 01 F6
          set LATE_07,(IX+LATE_00)  ;DD CB 00 FE

;set b,(IY+d)
          set $00,(IY+$FF)  ;FD CB FF C6
          set $01,(IY+$FE)  ;FD CB FE CE
          set $02,(IY+$FD)  ;FD CB FD D6
          set $03,(IY+$04)  ;FD CB 04 DE
          set $04,(IY+$03)  ;FD CB 03 E6
          set $05,(IY+$02)  ;FD CB 02 EE
          set $06,(IY+$01)  ;FD CB 01 F6
          set $07,(IY+$00)  ;FD CB 00 FE

          set LATE_00,(IY+LATE_FF)  ;FD CB FF C6
          set LATE_01,(IY+LATE_FE)  ;FD CB FE CE
          set LATE_02,(IY+LATE_FD)  ;FD CB FD D6
          set LATE_03,(IY+LATE_04)  ;FD CB 04 DE
          set LATE_04,(IY+LATE_03)  ;FD CB 03 E6
          set LATE_05,(IY+LATE_02)  ;FD CB 02 EE
          set LATE_06,(IY+LATE_01)  ;FD CB 01 F6
          set LATE_07,(IY+LATE_00)  ;FD CB 00 FE


;res b,r
          res $00,b         ;CB 80
          res $01,c         ;CB 89
          res $02,d         ;CB 92
          res $03,e         ;CB 9B
          res $04,h         ;CB A4
          res $05,l         ;CB AD
          res $06,a         ;CB B7
          res $07,a         ;CB BF

          res LATE_00,b     ;CB 80
          res LATE_01,c     ;CB 89
          res LATE_02,d     ;CB 92
          res LATE_03,e     ;CB 9B
          res LATE_04,h     ;CB A4
          res LATE_05,l     ;CB AD
          res LATE_06,a     ;CB B7
          res LATE_07,a     ;CB BF

          res $00,(HL)      ;CB 86
          res $01,(HL)      ;CB 8E
          res $02,(HL)      ;CB 96
          res $03,(HL)      ;CB 9E
          res $04,(HL)      ;CB A6
          res $05,(HL)      ;CB AE
          res $06,(HL)      ;CB B6
          res $07,(HL)      ;CB BE

          res LATE_00,(HL)  ;CB 86
          res LATE_01,(HL)  ;CB 8E
          res LATE_02,(HL)  ;CB 96
          res LATE_03,(HL)  ;CB 9E
          res LATE_04,(HL)  ;CB A6
          res LATE_05,(HL)  ;CB AE
          res LATE_06,(HL)  ;CB B6
          res LATE_07,(HL)  ;CB BE

;res b,(IX+d)
          res $00,(IX+$FF)  ;DD CB FF 86
          res $01,(IX+$FE)  ;DD CB FE 8E
          res $02,(IX+$FD)  ;DD CB FD 96
          res $03,(IX+$04)  ;DD CB 04 9E
          res $04,(IX+$03)  ;DD CB 03 A6
          res $05,(IX+$02)  ;DD CB 02 AE
          res $06,(IX+$01)  ;DD CB 01 B6
          res $07,(IX+$00)  ;DD CB 00 BE

          res LATE_00,(IX+LATE_FF)  ;DD CB FF 86
          res LATE_01,(IX+LATE_FE)  ;DD CB FE 8E
          res LATE_02,(IX+LATE_FD)  ;DD CB FD 96
          res LATE_03,(IX+LATE_04)  ;DD CB 04 9E
          res LATE_04,(IX+LATE_03)  ;DD CB 03 A6
          res LATE_05,(IX+LATE_02)  ;DD CB 02 AE
          res LATE_06,(IX+LATE_01)  ;DD CB 01 B6
          res LATE_07,(IX+LATE_00)  ;DD CB 00 BE

;res b,(IY+d)
          res $00,(IY+$FF)  ;FD CB FF 86
          res $01,(IY+$FE)  ;FD CB FE 8E
          res $02,(IY+$FD)  ;FD CB FD 96
          res $03,(IY+$04)  ;FD CB 04 9E
          res $04,(IY+$03)  ;FD CB 03 A6
          res $05,(IY+$02)  ;FD CB 02 AE
          res $06,(IY+$01)  ;FD CB 01 B6
          res $07,(IY+$00)  ;FD CB 00 BE

          res LATE_00,(IY+LATE_FF)  ;FD CB FF 86
          res LATE_01,(IY+LATE_FE)  ;FD CB FE 8E
          res LATE_02,(IY+LATE_FD)  ;FD CB FD 96
          res LATE_03,(IY+LATE_04)  ;FD CB 04 9E
          res LATE_04,(IY+LATE_03)  ;FD CB 03 A6
          res LATE_05,(IY+LATE_02)  ;FD CB 02 AE
          res LATE_06,(IY+LATE_01)  ;FD CB 01 B6
          res LATE_07,(IY+LATE_00)  ;FD CB 00 BE

;JP nn
          jp $0001
          jp $00FF
          jp $0100
          jp $FF00

          jp LATE_0001
          jp LATE_00FF
          jp LATE_0100
          jp LATE_FF00

;JP cc,nn
          jp NZ,$0001       ;C2 01 00
          jp z,$00FF        ;CA FF 00
          jp nc,$0100       ;D2 00 01
          jp c,$FF00        ;DA 00 FF

          jp po,LATE_0001   ;E2 01 00
          jp pe,LATE_00FF   ;EA FF 00
          jp p,LATE_0100    ;F2 00 01
          jp m,LATE_FF00    ;FA 00 FF

;JR e
POS
          jr POS + 2          ;18 00
          jr POS + 2 - $56    ;18 A8
          jr POS + 2 + $56    ;18 52

          jr NEXT_POS + 2     ;18 06
          jr NEXT_POS - $56   ;18 AC
          jr NEXT_POS + $56   ;18 56
NEXT_POS


;JR C,e
POS2
          jr C,POS2 + 2           ;38 00
          jr C,POS2 + 2 - $56     ;38 A8
          jr C,POS2 + 2 + $56     ;38 52

          jr C,NEXT_POS2 + 2      ;38 06
          jr C,NEXT_POS2 - $56    ;38 AC
          jr C,NEXT_POS2 + $56    ;38 56
NEXT_POS2


;JR NC,e
POS3
          jr nc,POS3 + 2           ;30 00
          jr nc,POS3 + 2 - $56     ;30 A8
          jr nc,POS3 + 2 + $56     ;30 52

          jr nc,NEXT_POS3 + 2      ;30 06
          jr nc,NEXT_POS3 - $56    ;30 AC
          jr nc,NEXT_POS3 + $56    ;30 56
NEXT_POS3


          jp (HL)                   ;E9
          jp (IX)                   ;DD E9
          jp (IY)                   ;FD E9

POS4
          djnz POS4 + 2             ;10 00
          djnz POS4 + 2 - $56       ;10 A8
          djnz POS4 + 2 + $56       ;10 52

          djnz NEXT_POS4 + 2    ;10 06
          djnz NEXT_POS4 - $56  ;10 AC
          djnz NEXT_POS4 + $56  ;10 56
NEXT_POS4


;CALL nn
          call $0001          ;CD 01 00
          call $00FE          ;CD FE 00
          call $0100          ;CD 00 01
          call $FF00          ;CD 00 FF

          call LATE_0001      ;CD 01 00
          call LATE_00FE      ;CD FE 00
          call LATE_0100      ;CD 00 01
          call LATE_FF00      ;CD 00 FF

;CALL cc,nn
          call nz,$0001       ;C4 01 00
          call z,$00FE        ;CC FE 00
          call nc,$0100       ;D4 00 01
          call c,$FF00        ;DC 00 FF

          call po,LATE_0001   ;E4 01 00
          call pe,LATE_00FE   ;EC FE 00
          call p,LATE_0100    ;F4 00 01
          call m,LATE_FF00    ;FC 00 FF

          ret         ;C9

;RET cc
          ret nz      ;C0
          ret z       ;C8
          ret nc      ;D0
          ret c       ;D8
          ret po      ;E0
          ret pe      ;E8
          ret p       ;F0
          ret m       ;F8

          reti        ;ED 4D
          retn        ;ED 45

;RST p
          rst $00     ;C7
          rst $08     ;CF
          rst $10     ;D7
          rst $18     ;DF
          rst $20     ;E7
          rst $28     ;EF
          rst $30     ;F7
          rst $38     ;FF

;IN A,(n)
          in A,($00)      ;DB 00
          in A,($01)      ;DB 01
          in A,($FE)      ;DB FE
          in A,($FF)      ;DB FF

          in A,(LATE_00)  ;DB 00
          in A,(LATE_01)  ;DB 01
          in A,(LATE_FE)  ;DB FE
          in A,(LATE_FF)  ;DB FF

;IN r,(C)
          in b,(C)     ;ED 40
          in c,(C)     ;ED 48
          in d,(C)     ;ED 50
          in e,(C)     ;ED 58
          in h,(C)     ;ED 60
          in l,(C)     ;ED 68
          in a,(C)     ;ED 78

          ini           ;ED A2
          inir          ;ED B2

          ind           ;ED AA
          indr          ;ED BA

;OUT (n),A
          out ($00),A      ;D3 00
          out ($01),A      ;D3 01
          out ($FE),A      ;D3 FE
          out ($FF),A      ;D3 FF

          out (LATE_00),A  ;D3 00
          out (LATE_01),A  ;D3 01
          out (LATE_FE),A  ;D3 FE
          out (LATE_FF),A  ;D3 FF

;OUT (C),r
          out (C),b     ;ED 41
          out (C),c     ;ED 49
          out (C),d     ;ED 51
          out (C),e     ;ED 59
          out (C),h     ;ED 61
          out (C),l     ;ED 69
          out (C),a     ;ED 79

          outi          ;ED A3
          otir          ;ED B3
          outd          ;ED AB
          otdr          ;ED BB


;undocumented opcodes (there's more, but the doc of undocumented opcodes is bad)
          sll b         ;CB 30
          sll c         ;CB 31
          sll d         ;CB 32
          sll e         ;CB 33
          sll h         ;CB 34
          sll l         ;CB 35
          sll a         ;CB 37

          sll (HL)      ;CB 36


LATE_FF = $ff
LATE_FE = $fe
LATE_FD = $fd
LATE_07 = $07
LATE_06 = $06
LATE_05 = $05
LATE_04 = $04
LATE_03 = $03
LATE_02 = $02
LATE_01 = $01
LATE_00 = $00
LATE_0000 = $0000
LATE_0001 = $0001
LATE_00FE = $00FE
LATE_00FF = $00FF
LATE_0100 = $0100
LATE_FF00 = $ff00
LATE_BBCC = $bbcc