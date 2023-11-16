!cpu 68000

* = $00

          adda.w #$0001,a0            ;D0 FC 00 01
          adda.w #$00FF,a1            ;D2 FC 00 FF
          adda.w #$0100,a2            ;D4 FC 01 00
          adda.w #$FF00,a3            ;D6 FC FF 00
          adda.w #LATE_0001,a4        ;D8 FC 00 01
          adda.w #LATE_00FF,a5        ;DA FC 00 FF
          adda.w #LATE_0100,a6        ;DC FC 01 00
          adda.w #LATE_FF00,a7        ;DE FC FF 00
          adda.w #$1234,sp            ;DE FC 12 34


          adda.w ($0001).w, a0        ;D0 F8 00 01
          adda.w ($00FF).w, a1        ;D2 F8 00 FF
          adda.w ($0100).w, a2        ;D4 F8 01 00
          adda.w ($FF00).w, a3        ;D6 F8 FF 00
          adda.w (LATE_0001).w, a4    ;D8 F8 00 01
          adda.w (LATE_00FF).w, a5    ;DA F8 00 FF
          adda.w (LATE_0100).w, a6    ;DC F8 01 00
          adda.w (LATE_FF00).w, a7    ;DE F8 FF 00
          adda.w ($1234).w, sp        ;DE F8 12 34

          adda.l ($000001).l, a0        ;D1 F9 00 00 01
          adda.l ($00FF00).l, a1        ;D3 F9 00 FF 00
          adda.l ($010000).l, a2        ;D5 F9 01 00 00
          adda.l ($FF0000).l, a3        ;D7 F9 FF 00 00
          adda.l (LATE_000001).l, a4    ;D9 F9 00 00 01
          adda.l (LATE_00FF00).l, a5    ;DB F9 00 FF 00
          adda.l (LATE_010000).l, a6    ;DD F9 01 00 00
          adda.l (LATE_FF0000).l, a7    ;DF F9 FF 00 00
          adda.l ($123456).l, sp        ;DF F9 12 34 56

          adda.w d7, a0                 ;D0 F8
          adda.w d6, a1                 ;D2 F0
          adda.w d5, a2                 ;D4 E8
          adda.w d4, a3                 ;D6 E0
          adda.w d3, a4                 ;D8 D8
          adda.w d2, a5                 ;DA D0
          adda.w d1, a6                 ;DC C8
          adda.w d0, a7                 ;DE C0
          adda.w d0, sp                 ;DE C0

          adda.w (sp), a0               ;D0 D7
          adda.w (a7), a1               ;D2 D7
          adda.w (a6), a2               ;D4 D6
          adda.w (a5), a3               ;D6 D5
          adda.w (a4), a4               ;D8 D4
          adda.w (a3), a5               ;DA D3
          adda.w (a2), a6               ;DC D2
          adda.w (a1), a7               ;DE D1
          adda.w (a0), sp               ;DE D0

          adda.w $0001(a0), a7          ;DE E8 00 01
          adda.w $00FF(a1), a6          ;DC E9 00 FF
          adda.w $0100(a2), a5          ;DA EA 01 00
          adda.w $FF00(a3), a4          ;D8 EB FF 00
          adda.w LATE_0001(a4), a3      ;D6 EC 00 01
          adda.w LATE_00FF(a5), a2      ;D4 ED 00 FF
          adda.w LATE_0100(a6), a1      ;D2 EE 01 00
          adda.w LATE_FF00(a7), a0      ;D0 EF FF 00

          adda.w (sp)+, a0              ;D0 DF
          adda.w (a7)+, a1              ;D2 DF
          adda.w (a6)+, a2              ;D4 DE
          adda.w (a5)+, a3              ;D6 DD
          adda.w (a4)+, a4              ;D8 DC
          adda.w (a3)+, a5              ;DA DB
          adda.w (a2)+, a6              ;DC DA
          adda.w (a1)+, a7              ;DE D9
          adda.w (a0)+, sp              ;DE D8

          adda.w -(sp), a0              ;D0 E7
          adda.w -(a7), a1              ;D2 E7
          adda.w -(a6), a2              ;D4 E6
          adda.w -(a5), a3              ;D6 E5
          adda.w -(a4), a4              ;D8 E4
          adda.w -(a3), a5              ;DA E3
          adda.w -(a2), a6              ;DC E2
          adda.w -(a1), a7              ;DE E1
          adda.w -(a0), sp              ;DE E0

          adda.w (a0,d7.w), a7          ;DE F0 70 00
          adda.w (a1,d6.w), a6          ;DC F1 60 00
          adda.w (a2,d5.w), a5          ;DA F2 50 00
          adda.w (a3,d4.w), a4          ;D8 F3 40 00
          adda.w (a4,d3.w), a3          ;D6 F4 30 00
          adda.w (a5,d2.w), a2          ;D4 F5 20 00
          adda.w (a6,d1.w), a1          ;D2 F6 10 00
          adda.w (a7,d0.w), a0          ;D0 F7 00 00

          adda.w $01(a0,d7.w), a7       ;DE F0 70 01
          adda.w $FF(a1,d6.w), a6       ;DC F1 60 FF
          adda.w LATE_01(a2,d5.w), a5   ;DA F2 50 01
          adda.w LATE_FF(a3,d4.w), a4   ;D8 F3 40 FF
          adda.w $01(a4,d3.w), a3       ;D6 F4 30 01
          adda.w $FF(a5,d2.w), a2       ;D4 F5 20 FF
          adda.w LATE_01(a6,d1.w), a1   ;D2 F6 10 01
          adda.w LATE_FF(a7,d0.w), a0   ;D0 F7 00 FF

          ;addi
          addi.b #$01,d0                ;06 00 00 01
          addi.b #$FF,d1                ;06 01 00 FF
          addi.b #$01,d2                ;06 02 00 01
          addi.b #$FF,d3                ;06 03 00 FF
          addi.b #LATE_01,d4            ;06 04 00 01
          addi.b #LATE_FF,d5            ;06 05 00 FF
          addi.b #LATE_01,d6            ;06 06 00 01
          addi.b #LATE_FF,d7            ;06 07 00 FF

          addi.l #$00000001,d0          ;06 80 00 00 00 01
          addi.l #$000000FF,d1          ;06 81 00 00 00 FF
          addi.l #$00000100,d2          ;06 82 00 00 01 00
          addi.l #$0000FF00,d3          ;06 83 00 00 FF 00
          addi.l #$00010000,d4          ;06 84 00 01 00 00
          addi.l #$00FF0000,d5          ;06 85 00 FF 00 00
          addi.l #$01000000,d6          ;06 86 01 00 00 00
          addi.l #$FF000000,d7          ;06 87 FF 00 00 00

          addi.w #$0001, ($FF00).w          ;06 78 00 01 FF 00
          addi.w #$00FF, ($0100).w          ;06 78 00 FF 01 00
          addi.w #$0100, (LATE_00FF).w      ;06 78 01 00 00 FF
          addi.w #$FF00, (LATE_0001).w      ;06 78 FF 00 00 01
          addi.w #LATE_0001, ($FF00).w      ;06 78 00 01 FF 00
          addi.w #LATE_00FF, ($0100).w      ;06 78 00 FF 01 00
          addi.w #LATE_0100, (LATE_00FF).w  ;06 78 01 00 00 FF
          addi.w #LATE_FF00, (LATE_0001).w  ;06 78 FF 00 00 01

          addi.w #$0001, ($FF000000).l          ;06 79 00 01 FF 00 00 00
          addi.w #$00FF, ($00010000).l          ;06 79 00 FF 00 01 00 00
          addi.w #$0100, (LATE_0000FF00).l      ;06 79 01 00 00 00 FF 00
          addi.w #$FF00, (LATE_00000001).l      ;06 79 FF 00 00 00 00 01
          addi.w #LATE_0001, ($FF000000).l      ;06 79 00 01 FF 00 00 00
          addi.w #LATE_00FF, ($00010000).l      ;06 79 00 FF 00 01 00 00
          addi.w #LATE_0100, (LATE_0000FF00).l  ;06 79 01 00 00 00 FF 00
          addi.w #LATE_FF00, (LATE_00000001).l  ;06 79 FF 00 00 00 00 01

          addi.l #$00000001, ($FF00).w          ;06 B8 00 00 00 01 FF 00
          addi.l #$0000FF00, ($0100).w          ;06 B8 00 00 FF 00 01 00
          addi.l #$00010000, (LATE_00FF).w      ;06 B8 00 01 00 01 00 FF
          addi.l #$FF000000, (LATE_0001).w      ;06 B8 FF 00 00 01 00 01
          addi.l #LATE_00000001, ($FF00).w      ;06 B8 00 00 00 01 FF 00
          addi.l #LATE_0000FF00, ($0100).w      ;06 B8 00 00 FF 00 01 00
          addi.l #LATE_00010000, (LATE_00FF).w  ;06 B8 00 01 00 00 00 FF
          addi.l #LATE_FF000000, (LATE_0001).w  ;06 B8 FF 00 00 00 00 01

          addi.b #$01, (a7)                     ;06 17 00 01
          addi.b #$02, (a6)                     ;06 16 00 02
          addi.b #$FE, (a5)                     ;06 15 00 FE
          addi.b #$FF, (a4)                     ;06 14 00 FF
          addi.b #LATE_01, (a3)                 ;06 13 00 01
          addi.b #LATE_02, (a2)                 ;06 12 00 02
          addi.b #LATE_FE, (a1)                 ;06 11 00 FE
          addi.b #LATE_FF, (a0)                 ;06 10 00 FF

          addi.w #$0001, $FF(a0)          ;06 68 00 01 00 FF
          addi.w #$00FF, $FE(a1)          ;06 69 00 FF 00 FE
          addi.w #$0100, LATE_02(a2)      ;06 6A 01 00 00 02
          addi.w #$FF00, LATE_01(a3)      ;06 6B FF 00 00 01
          addi.w #LATE_0001, $FF(a4)      ;06 6C 00 01 00 FF
          addi.w #LATE_00FF, $FE(a5)      ;06 6D 00 FF 00 FE
          addi.w #LATE_0100, LATE_02(a6)  ;06 6E 01 00 00 02
          addi.w #LATE_FF00, LATE_01(a7)  ;06 6F FF 00 00 01

          addi.l #$00000001, $FF(a0)          ;06 A8 00 00 00 01 00 FF
          addi.l #$0000FF00, $01(a1)          ;06 A9 00 00 FF 00 00 01
          addi.l #$00010000, LATE_FF(a2)      ;06 AA 00 01 00 00 00 FF
          addi.l #$FF000000, LATE_01(a3)      ;06 AB FF 00 00 00 00 01
          addi.l #LATE_00000001, $FF(a4)      ;06 AC 00 00 00 01 00 FF
          addi.l #LATE_0000FF00, $01(a5)      ;06 AD 00 00 FF 00 00 01
          addi.l #LATE_00010000, LATE_FF(a6)  ;06 AE 00 01 00 00 00 FF
          addi.l #LATE_FF000000, LATE_01(a7)  ;06 AF FF 00 00 00 00 01

          addi.b #$01, (a0)+              ;06 18 00 01
          addi.b #$02, (a1)+              ;06 19 00 02
          addi.b #$FE, (a2)+              ;06 1A 00 FE
          addi.b #$FF, (a3)+              ;06 1B 00 FF
          addi.b #LATE_01, (a4)+          ;06 1C 00 01
          addi.b #LATE_02, (a5)+          ;06 1D 00 02
          addi.b #LATE_FE, (a6)+          ;06 1E 00 FE
          addi.b #LATE_FF, (a7)+          ;06 1F 00 FF

          addi.b #$01, -(a0)              ;06 28 00 01
          addi.b #$02, -(a1)              ;06 29 00 02
          addi.b #$FE, -(a2)              ;06 2A 00 FE
          addi.b #$FF, -(a3)              ;06 2B 00 FF
          addi.b #LATE_01, -(a4)          ;06 2C 00 01
          addi.b #LATE_02, -(a5)          ;06 2D 00 02
          addi.b #LATE_FE, -(a6)          ;06 2E 00 FE
          addi.b #LATE_FF, -(a7)          ;06 2F 00 FF

          addq.b #1, ($0001).w            ;52 38 00 01
          addq.b #2, ($00FF).w            ;54 38 00 FF
          addq.b #3, ($0100).w            ;56 38 01 00
          addq.b #4, ($FF00).w            ;58 38 FF 00
          addq.b #5, (LATE_0001).w        ;5A 38 00 01
          addq.b #6, (LATE_00FF).w        ;5C 38 00 FF
          addq.b #7, (LATE_0100).w        ;5E 38 01 00
          addq.b #8, (LATE_FF00).w        ;50 38 FF 00

          addq.b #1, d7                   ;52 07
          addq.b #2, d6                   ;54 06
          addq.b #3, d5                   ;56 05
          addq.b #4, d4                   ;58 04
          addq.b #5, d3                   ;5A 03
          addq.b #6, d2                   ;5C 02
          addq.b #7, d1                   ;5E 01
          addq.b #8, d0                   ;50 00

          addq.w #1, d7                   ;52 47
          addq.w #2, d6                   ;54 46
          addq.w #3, d5                   ;56 45
          addq.w #4, d4                   ;58 44
          addq.w #5, d3                   ;5A 43
          addq.w #6, d2                   ;5C 42
          addq.w #7, d1                   ;5E 41
          addq.w #8, d0                   ;50 40

          addq.w #1, (a7)                 ;52 57
          addq.w #2, (a6)                 ;54 56
          addq.w #3, (a5)                 ;56 55
          addq.w #4, (a4)                 ;58 54
          addq.w #5, (a3)                 ;5A 53
          addq.w #6, (a2)                 ;5C 52
          addq.w #7, (a1)                 ;5E 51
          addq.w #8, (a0)                 ;50 50

          addq.b #1, $01(a7)              ;52 2F 00 01
          addq.b #2, $02(a6)              ;54 2E 00 02
          addq.b #3, $FE(a5)              ;56 2D 00 FE
          addq.b #4, $FF(a4)              ;58 2C 00 FF
          addq.b #5, LATE_01(a3)          ;5A 2B 00 01
          addq.b #6, LATE_02(a2)          ;5C 2A 00 02
          addq.b #7, LATE_FE(a1)          ;5E 29 00 FE
          addq.b #8, LATE_FF(a0)          ;50 28 00 FF

          addq.w #1, $01(a7)              ;52 6F 00 01
          addq.w #2, $02(a6)              ;54 6E 00 02
          addq.w #3, $FE(a5)              ;56 6D 00 FE
          addq.w #4, $FF(a4)              ;58 6C 00 FF
          addq.w #5, LATE_01(a3)          ;5A 6B 00 01
          addq.w #6, LATE_02(a2)          ;5C 6A 00 02
          addq.w #7, LATE_FE(a1)          ;5E 69 00 FE
          addq.w #8, LATE_FF(a0)          ;50 68 00 FF

          addq.w #1, (a7)+                ;52 5F
          addq.w #2, (a6)+                ;54 5E
          addq.w #3, (a5)+                ;56 5D
          addq.w #4, (a4)+                ;58 5C
          addq.w #5, (a3)+                ;5A 5B
          addq.w #6, (a2)+                ;5C 5A
          addq.w #7, (a1)+                ;5E 59
          addq.w #8, (a0)+                ;50 58

          addq.w #1, -(a7)                ;52 67
          addq.w #2, -(a6)                ;54 66
          addq.w #3, -(a5)                ;56 65
          addq.w #4, -(a4)                ;58 64
          addq.w #5, -(a3)                ;5A 63
          addq.w #6, -(a2)                ;5C 62
          addq.w #7, -(a1)                ;5E 61
          addq.w #8, -(a0)                ;50 60

          addq.w #1, a7                   ;52 4F
          addq.w #2, a6                   ;54 4E
          addq.w #3, a5                   ;56 4D
          addq.w #4, a4                   ;58 4C
          addq.w #5, a3                   ;5A 4B
          addq.w #6, a2                   ;5C 4A
          addq.w #7, a1                   ;5E 49
          addq.w #8, a0                   ;50 48

          addq.l #1, a7                   ;52 8F
          addq.l #2, a6                   ;54 8E
          addq.l #3, a5                   ;56 8D
          addq.l #4, a4                   ;58 8C
          addq.l #5, a3                   ;5A 8B
          addq.l #6, a2                   ;5C 8A
          addq.l #7, a1                   ;5E 89
          addq.l #8, a0                   ;50 88

          add.w d0,d7             ;DE 40
          add.w d1,d6             ;DC 41
          add.w d2,d5             ;DA 42
          add.w d3,d4             ;D8 43
          add.w d4,d3             ;D6 44
          add.w d5,d2             ;D4 45
          add.w d6,d1             ;D2 46
          add.w d7,d0             ;D0 47

          add.w ($0001).w, d0     ;D0 78 00 01
          add.w ($00FF).w, d1     ;D2 78 00 FF
          add.w ($0100).w, d2     ;D4 78 01 00
          add.w ($FF00).w, d3     ;D6 78 FF 00
          add.w (LATE_0001).w, d4 ;D8 78 00 01
          add.w (LATE_00FF).w, d5 ;DA 78 00 FF
          add.w (LATE_0100).w, d6 ;DC 78 01 00
          add.w (LATE_FF00).w, d7 ;DE 78 FF 00

          add.w (a7), d0          ;D0 57
          add.w (a6), d1          ;D2 56
          add.w (a5), d2          ;D4 55
          add.w (a4), d3          ;D6 54
          add.w (a3), d4          ;D8 53
          add.w (a2), d5          ;DA 52
          add.w (a1), d6          ;DC 51
          add.w (a0), d7          ;DE 50

          add.b $01(a0),d7        ;DE 28 00 01
          add.b $02(a1),d6        ;DC 29 00 02
          add.b $FE(a2),d5        ;DA 2A 00 FE
          add.b $FF(a3),d4        ;D8 2B 00 FF
          add.b LATE_01(a4),d3    ;D6 2C 00 01
          add.b LATE_02(a5),d2    ;D4 2D 00 02
          add.b LATE_FE(a6),d1    ;D2 2E 00 FE
          add.b LATE_FF(a7),d0    ;D0 2F 00 FF

          add.w $01(a0),d7        ;DE 68 00 01
          add.w $02(a1),d6        ;DC 69 00 02
          add.w $FE(a2),d5        ;DA 6A 00 FE
          add.w $FF(a3),d4        ;D8 6B 00 FF
          add.w LATE_01(a4),d3    ;D6 6C 00 01
          add.w LATE_02(a5),d2    ;D4 6D 00 02
          add.w LATE_FE(a6),d1    ;D2 6E 00 FE
          add.w LATE_FF(a7),d0    ;D0 6F 00 FF

          add.w (a0)+,d7          ;DE 58
          add.w (a1)+,d6          ;DC 59
          add.w (a2)+,d5          ;DA 5A
          add.w (a3)+,d4          ;D8 5B
          add.w (a4)+,d3          ;D6 5C
          add.w (a5)+,d2          ;D4 5D
          add.w (a6)+,d1          ;D2 5E
          add.w (a7)+,d0          ;D0 5F

          add.w -(a0),d7          ;DE 60
          add.w -(a1),d6          ;DC 61
          add.w -(a2),d5          ;DA 62
          add.w -(a3),d4          ;D8 63
          add.w -(a4),d3          ;D6 64
          add.w -(a5),d2          ;D4 65
          add.w -(a6),d1          ;D2 66
          add.w -(a7),d0          ;D0 67

          add.l a0,d7             ;DE 88
          add.l a1,d6             ;DC 89
          add.l a2,d5             ;DA 8A
          add.l a3,d4             ;D8 8B
          add.l a4,d3             ;D6 8C
          add.l a5,d2             ;D4 8D
          add.l a6,d1             ;D2 8E
          add.l a7,d0             ;D0 8F

          add.w d0, ($0001).w     ;D1 78 00 01
          add.w d1, ($00FF).w     ;D3 78 00 FF
          add.w d2, ($0100).w     ;D5 78 01 00
          add.w d3, ($FF00).w     ;D7 78 FF 00
          add.w d4, (LATE_0001).w ;D9 78 00 01
          add.w d5, (LATE_00FF).w ;DB 78 00 FF
          add.w d6, (LATE_0100).w ;DD 78 01 00
          add.w d7, (LATE_FF00).w ;DF 78 FF 00

          add.w d0,(a7)           ;D1 57
          add.w d1,(a6)           ;D3 56
          add.w d2,(a5)           ;D5 55
          add.w d3,(a4)           ;D7 54
          add.w d4,(a3)           ;D9 53
          add.w d5,(a2)           ;DB 52
          add.w d6,(a1)           ;DD 51
          add.w d7,(a0)           ;DF 50

          add.w d0, $01(a7)       ;D1 6F 00 01
          add.w d1, $02(a6)       ;D3 6E 00 02
          add.w d2, $FE(a5)       ;D5 6D 00 FE
          add.w d3, $FF(a4)       ;D7 6C 00 FF
          add.w d4, LATE_01(a3)   ;D9 6B 00 01
          add.w d5, LATE_02(a2)   ;DB 6A 00 02
          add.w d6, LATE_FE(a1)   ;DD 69 00 FE
          add.w d7, LATE_FF(a0)   ;DF 68 00 FF

          add.l d0, $01(a7)       ;D1 AF 00 01
          add.l d1, $02(a6)       ;D3 AE 00 02
          add.l d2, $FE(a5)       ;D5 AD 00 FE
          add.l d3, $FF(a4)       ;D7 AC 00 FF
          add.l d4, LATE_01(a3)   ;D9 AB 00 01
          add.l d5, LATE_02(a2)   ;DB AA 00 02
          add.l d6, LATE_FE(a1)   ;DD A9 00 FE
          add.l d7, LATE_FF(a0)   ;DF A8 00 FF

          add.l d0,(a7)+          ;D1 9F
          add.l d1,(a6)+          ;D3 9E
          add.l d2,(a5)+          ;D5 9D
          add.l d3,(a4)+          ;D7 9C
          add.l d4,(a3)+          ;D9 9B
          add.l d5,(a2)+          ;DB 9A
          add.l d6,(a1)+          ;DD 99
          add.l d7,(a0)+          ;DF 98

          add.w d0,-(a7)          ;D1 6F
          add.w d1,-(a6)          ;D3 6E
          add.w d2,-(a5)          ;D5 6D
          add.w d3,-(a4)          ;D7 6C
          add.w d4,-(a3)          ;D9 6B
          add.w d5,-(a2)          ;DB 6A
          add.w d6,-(a1)          ;DD 69
          add.w d7,-(a0)          ;DF 68


          andi.b #$01, d7         ;02 07 00 01
          andi.b #$02, d6         ;02 06 00 02
          andi.b #$FE, d5         ;02 05 00 FE
          andi.b #$FF, d4         ;02 04 00 FF
          andi.b #LATE_01, d3     ;02 03 00 01
          andi.b #LATE_02, d2     ;02 02 00 02
          andi.b #LATE_FE, d1     ;02 01 00 FE
          andi.b #LATE_FF, d0     ;02 00 00 FF

          andi.w #$0001, d7       ;02 47 00 01
          andi.w #$00FF, d6       ;02 46 00 FF
          andi.w #$0100, d5       ;02 45 01 00
          andi.w #$FF00, d4       ;02 44 FF 00
          andi.w #LATE_0001, d3   ;02 43 00 01
          andi.w #LATE_00FF, d2   ;02 42 00 FF
          andi.w #LATE_0100, d1   ;02 41 01 00
          andi.w #LATE_FF00, d0   ;02 40 FF 00

          andi.l #$00000001, d7       ;02 87 00 00 00 01
          andi.l #$0000FF00, d6       ;02 86 00 00 FF 00
          andi.l #$00010000, d5       ;02 85 00 01 00 00
          andi.l #$FF000000, d4       ;02 84 FF 00 00 00
          andi.l #LATE_00000001, d3   ;02 83 00 00 00 01
          andi.l #LATE_0000FF00, d2   ;02 82 00 00 FF 00
          andi.l #LATE_00010000, d1   ;02 81 00 01 00 00
          andi.l #LATE_FF000000, d0   ;02 80 FF 00 00 00

          andi.b #$01, ($0001).w          ;02 38 00 01 00 01
          andi.b #$02, ($00FF).w          ;02 38 00 02 00 FF
          andi.b #$FE, (LATE_0100).w      ;02 38 00 FE 01 00
          andi.b #$FF, (LATE_FF00).w      ;02 38 00 FF FF 00
          andi.b #LATE_01, ($0001).w      ;02 38 00 01 00 01
          andi.b #LATE_02, ($00FF).w      ;02 38 00 02 00 FF
          andi.b #LATE_FE, (LATE_0100).w  ;02 38 00 FE 01 00
          andi.b #LATE_FF, (LATE_FF00).w  ;02 38 00 FF FF 00

          andi.w #$0001, ($0001).w          ;02 78 00 01 00 01
          andi.w #$00FF, ($00FF).w          ;02 78 00 FF 00 FF
          andi.w #$0100, (LATE_0100).w      ;02 78 01 00 01 00
          andi.w #$FF00, (LATE_FF00).w      ;02 78 FF 00 FF 00
          andi.w #LATE_0001, ($0001).w      ;02 78 00 01 00 01
          andi.w #LATE_00FF, ($00FF).w      ;02 78 00 FF 00 FF
          andi.w #LATE_0100, (LATE_0100).w  ;02 78 01 00 01 00
          andi.w #LATE_FF00, (LATE_FF00).w  ;02 78 FF 00 FF 00

          andi.b #$01, (a7)       ;02 17 00 01
          andi.b #$02, (a6)       ;02 16 00 02
          andi.b #$FE, (a5)       ;02 15 00 FE
          andi.b #$FF, (a4)       ;02 14 00 FF
          andi.b #LATE_01, (a3)   ;02 13 00 01
          andi.b #LATE_02, (a2)   ;02 12 00 02
          andi.b #LATE_FE, (a1)   ;02 11 00 FE
          andi.b #LATE_FF, (a0)   ;02 10 00 FF

          andi.b #$01, $ff(a7)           ;02 2F 00 01 00 FF
          andi.b #$02, $fe(a6)           ;02 2E 00 02 00 FE
          andi.b #$FE, $02(a5)           ;02 2D 00 FE 00 02
          andi.b #$FF, $01(a4)           ;02 2C 00 FF 00 01
          andi.b #LATE_01, LATE_FF(a3)   ;02 2B 00 01 00 FF
          andi.b #LATE_02, LATE_FE(a2)   ;02 2A 00 02 00 FE
          andi.b #LATE_FE, LATE_02(a1)   ;02 29 00 FE 00 02
          andi.b #LATE_FF, LATE_01(a0)   ;02 28 00 FF 00 01

          andi.b #$01, (a7)+      ;02 1F 00 01
          andi.b #$02, (a6)+      ;02 1E 00 02
          andi.b #$FE, (a5)+      ;02 1D 00 FE
          andi.b #$FF, (a4)+      ;02 1C 00 FF
          andi.b #LATE_01, (a3)+  ;02 1B 00 01
          andi.b #LATE_02, (a2)+  ;02 1A 00 02
          andi.b #LATE_FE, (a1)+  ;02 19 00 FE
          andi.b #LATE_FF, (a0)+  ;02 18 00 FF

          andi.b #$01, -(a7)      ;02 27 00 01
          andi.b #$02, -(a6)      ;02 26 00 02
          andi.b #$FE, -(a5)      ;02 25 00 FE
          andi.b #$FF, -(a4)      ;02 24 00 FF
          andi.b #LATE_01, -(a3)  ;02 23 00 01
          andi.b #LATE_02, -(a2)  ;02 22 00 02
          andi.b #LATE_FE, -(a1)  ;02 21 00 FE
          andi.b #LATE_FF, -(a0)  ;02 20 00 FF

          andi.w #$0001, $ff(a7)           ;02 6F 00 01 00 FF
          andi.w #$00FF, $fe(a6)           ;02 6E 00 FF 00 FE
          andi.w #$0100, $02(a5)           ;02 6D 01 00 00 02
          andi.w #$FF00, $01(a4)           ;02 6C FF 00 00 01
          andi.w #LATE_0001, LATE_FF(a3)   ;02 6B 00 01 00 FF
          andi.w #LATE_00FF, LATE_FE(a2)   ;02 6A 00 FF 00 FE
          andi.w #LATE_0100, LATE_02(a1)   ;02 69 01 00 00 02
          andi.w #LATE_FF00, LATE_01(a0)   ;02 68 FF 00 00 01

          andi.b #$01,(a0,d7.w)       ;02 30 00 01 70 00
          andi.b #$FF,(a1,d6.w)       ;02 31 00 FF 60 00
          andi.b #LATE_01,(a2,d5.w)   ;02 32 00 01 50 00
          andi.b #LATE_FF,(a3,d4.w)   ;02 33 00 FF 40 00
          andi.b #$01,(a4,d3.w)       ;02 34 00 01 30 00
          andi.b #$FF,(a5,d2.w)       ;02 35 00 FF 20 00
          andi.b #LATE_01,(a6,d1.w)   ;02 36 00 01 10 00
          andi.b #LATE_FF,(a7,d0.w)   ;02 37 00 FF 00 00

          andi.w #$0001, $FF(a0,d7.w)           ;02 70 00 01 70 FF
          andi.w #$00FF, $01(a1,d6.w)           ;02 71 00 FF 60 01
          andi.w #$0100, LATE_FF(a2,d5.w)       ;02 72 01 00 50 FF
          andi.w #$FF00, LATE_01(a3,d4.w)       ;02 73 FF 00 40 01
          andi.w #LATE_0001, $FF(a4,d3.w)       ;02 74 00 01 30 FF
          andi.w #LATE_00FF, $01(a5,d2.w)       ;02 75 00 FF 20 01
          andi.w #LATE_0100, LATE_FF(a6,d1.w)   ;02 76 01 00 10 FF
          andi.w #LATE_FF00, LATE_01(a7,d0.w)   ;02 77 FF 00 00 01

          andi #$0001,sr      ;02 7C 00 01
          andi #$00FF,sr      ;02 7C 00 FF
          andi #$0100,sr      ;02 7C 01 00
          andi #$FF00,sr      ;02 7C FF 00
          andi #LATE_0001,sr  ;02 7C 00 01
          andi #LATE_00FF,sr  ;02 7C 00 FF
          andi #LATE_0100,sr  ;02 7C 01 00
          andi #LATE_FF00,sr  ;02 7C FF 00

          and.b d0,d7             ;CE 00
          and.b d1,d6             ;CC 01
          and.b d2,d5             ;CA 02
          and.b d3,d4             ;C8 03
          and.b d4,d3             ;C6 04
          and.b d5,d2             ;C4 05
          and.b d6,d1             ;C2 06
          and.b d7,d0             ;C0 07

          and.b ($0001).w, d7       ;CE 38 00 01
          and.b ($00FF).w, d6       ;CC 38 00 FF
          and.b ($0100).w, d5       ;CA 38 01 00
          and.b ($FF00).w, d4       ;C8 38 FF 00
          and.b (LATE_0001).w, d3   ;C6 38 00 01
          and.b (LATE_00FF).w, d2   ;C4 38 00 FF
          and.b (LATE_0100).w, d1   ;C2 38 01 00
          and.b (LATE_FF00).w, d0   ;C0 38 FF 00

          and.w ($0001).w, d7       ;CE 78 00 01
          and.w ($00FF).w, d6       ;CC 78 00 FF
          and.w ($0100).w, d5       ;CA 78 01 00
          and.w ($FF00).w, d4       ;C8 78 FF 00
          and.w (LATE_0001).w, d3   ;C6 78 00 01
          and.w (LATE_00FF).w, d2   ;C4 78 00 FF
          and.w (LATE_0100).w, d1   ;C2 78 01 00
          and.w (LATE_FF00).w, d0   ;C0 78 FF 00

          and.w (a7), d0            ;C0 57
          and.w (a6), d1            ;C2 56
          and.w (a5), d2            ;C4 55
          and.w (a4), d3            ;C6 54
          and.w (a3), d4            ;C8 53
          and.w (a2), d5            ;CA 52
          and.w (a1), d6            ;CC 51
          and.w (a0), d7            ;CE 50

          and.w $01(a7), d0         ;C0 6F 00 01
          and.w $02(a6), d1         ;C2 6E 00 02
          and.w $FE(a5), d2         ;C4 6D 00 FE
          and.w $FF(a4), d3         ;C6 6C 00 FF
          and.w LATE_01(a3), d4     ;C8 6B 00 01
          and.w LATE_02(a2), d5     ;CA 6A 00 02
          and.w LATE_FE(a1), d6     ;CC 69 00 FE
          and.w LATE_FF(a0), d7     ;CE 68 00 FF

          and.w (a7)+, d0           ;C0 5F
          and.w (a6)+, d1           ;C2 5E
          and.w (a5)+, d2           ;C4 5D
          and.w (a4)+, d3           ;C6 5C
          and.w (a3)+, d4           ;C8 5B
          and.w (a2)+, d5           ;CA 5A
          and.w (a1)+, d6           ;CC 59
          and.w (a0)+, d7           ;CE 58

          and.w -(a7), d0           ;C0 67
          and.w -(a6), d1           ;C2 66
          and.w -(a5), d2           ;C4 65
          and.w -(a4), d3           ;C6 64
          and.w -(a3), d4           ;C8 63
          and.w -(a2), d5           ;CA 62
          and.w -(a1), d6           ;CC 61
          and.w -(a0), d7           ;CE 60

          and.w d0, ($0001).w       ;C1 78 00 01
          and.w d1, ($00FF).w       ;C3 78 00 FF
          and.w d2, ($0100).w       ;C5 78 01 00
          and.w d3, ($FF00).w       ;C7 78 FF 00
          and.w d4, (LATE_0001).w   ;C9 78 00 01
          and.w d5, (LATE_00FF).w   ;CB 78 00 FF
          and.w d6, (LATE_0100).w   ;CD 78 01 00
          and.w d7, (LATE_FF00).w   ;CF 78 FF 00

          and.w d0,(a7)             ;C1 57
          and.w d1,(a6)             ;C3 56
          and.w d2,(a5)             ;C5 55
          and.w d3,(a4)             ;C7 54
          and.w d4,(a3)             ;C9 53
          and.w d5,(a2)             ;CB 52
          and.w d6,(a1)             ;CD 51
          and.w d7,(a0)             ;CF 50

          and.w d0, $01(a7)         ;C1 6F 00 01
          and.w d1, $02(a6)         ;C3 6E 00 02
          and.w d2, $FE(a5)         ;C5 6D 00 FE
          and.w d3, $FF(a4)         ;C7 6C 00 FF
          and.w d4, LATE_01(a3)     ;C9 6B 00 01
          and.w d5, LATE_02(a2)     ;CB 6A 00 02
          and.w d6, LATE_FE(a1)     ;CD 69 00 FE
          and.w d7, LATE_FF(a0)     ;CF 68 00 FF

          and.w d0,(a7)+            ;C1 5F
          and.w d1,(a6)+            ;C3 5E
          and.w d2,(a5)+            ;C5 5D
          and.w d3,(a4)+            ;C7 5C
          and.w d4,(a3)+            ;C9 5B
          and.w d5,(a2)+            ;CB 5A
          and.w d6,(a1)+            ;CD 59
          and.w d7,(a0)+            ;CF 58

          and.w d0,-(a7)            ;C1 67
          and.w d1,-(a6)            ;C3 66
          and.w d2,-(a5)            ;C5 65
          and.w d3,-(a4)            ;C7 64
          and.w d4,-(a3)            ;C9 63
          and.w d5,-(a2)            ;CB 62
          and.w d6,-(a1)            ;CD 61
          and.w d7,-(a0)            ;CF 60

-
          bra.s -                  ;60 FE
          bra.s +                  ;60 00
+

-
          bra.w -                  ;60 00 FE FE
          bra.w +                  ;60 00 00 02
+

-
          bsr.s -                  ;61 FE
          bsr.s +                  ;61 00
+
-
          bhi.s -                  ;62 FE
          bhi.s +                  ;62 00
+
-
          bls.s -                  ;63 FE
          bls.s +                  ;63 00
+
-
          bcc.s -                  ;64 FE
          bcc.s +                  ;64 00
+
-
          bcs.s -                  ;65 FE
          bcs.s +                  ;65 00
+
-
          bne.s -                  ;66 FE
          bne.s +                  ;66 00
+
-
          beq.s -                  ;67 FE
          beq.s +                  ;67 00
+
-
          bvc.s -                  ;68 FE
          bvc.s +                  ;68 00
+
-
          bvs.s -                  ;69 FE
          bvs.s +                  ;69 00
+
-
          bpl.s -                  ;6A FE
          bpl.s +                  ;6A 00
+
-
          bmi.s -                  ;6B FE
          bmi.s +                  ;6B 00
+
-
          bge.s -                  ;6C FE
          bge.s +                  ;6C 00
+
-
          blt.s -                  ;6D FE
          blt.s +                  ;6D 00
+
-
          bgt.s -                  ;6E FE
          bgt.s +                  ;6E 00
+
-
          ble.s -                  ;6F FE
          ble.s +                  ;6F 00
+


          btst #0, ($FF00).w        ;08 38 00 00 FF 00
          btst #1, ($0100).w        ;08 38 00 01 01 00
          btst #2, ($00FF).w        ;08 38 00 02 00 FF
          btst #3, ($0001).w        ;08 38 00 03 00 01
          btst #4, (LATE_FF00).w    ;08 38 00 04 FF 00
          btst #5, (LATE_0100).w    ;08 38 00 05 01 00
          btst #6, (LATE_00FF).w    ;08 38 00 06 00 FF
          btst #7, (LATE_0001).w    ;08 38 00 07 00 01

          btst #0, d7               ;08 07 00 00
          btst #1, d6               ;08 06 00 01
          btst #2, d5               ;08 05 00 02
          btst #3, d4               ;08 04 00 03
          btst #4, d3               ;08 03 00 04
          btst #5, d2               ;08 02 00 05
          btst #6, d1               ;08 01 00 06
          btst #7, d0               ;08 00 00 07

          btst #0, (a7)             ;08 17 00 00
          btst #1, (a6)             ;08 16 00 01
          btst #2, (a5)             ;08 15 00 02
          btst #3, (a4)             ;08 14 00 03
          btst #4, (a3)             ;08 13 00 04
          btst #5, (a2)             ;08 12 00 05
          btst #6, (a1)             ;08 11 00 06
          btst #7, (a0)             ;08 10 00 07

          btst #0, $FF(a7)          ;08 2F 00 00 00 FF
          btst #1, $FE(a6)          ;08 2E 00 01 00 FE
          btst #2, $02(a5)          ;08 2D 00 02 00 02
          btst #3, $01(a4)          ;08 2C 00 03 00 01
          btst #4, LATE_FF(a3)      ;08 2B 00 04 00 FF
          btst #5, LATE_FE(a2)      ;08 2A 00 05 00 FE
          btst #6, LATE_02(a1)      ;08 29 00 06 00 02
          btst #7, LATE_01(a0)      ;08 28 00 07 00 01

          btst d0, ($FF00).w        ;01 38 FF 00
          btst d1, ($0100).w        ;03 38 01 00
          btst d2, ($00FF).w        ;05 38 00 FF
          btst d3, ($0001).w        ;07 38 00 01
          btst d4, (LATE_FF00).w    ;09 38 FF 00
          btst d5, (LATE_0100).w    ;0B 38 01 00
          btst d6, (LATE_00FF).w    ;0D 38 00 FF
          btst d7, (LATE_0001).w    ;0F 38 00 01

          btst d0, d7               ;01 07
          btst d1, d6               ;03 06
          btst d2, d5               ;05 05
          btst d3, d4               ;07 04
          btst d4, d3               ;09 03
          btst d5, d2               ;0B 02
          btst d6, d1               ;0D 01
          btst d7, d0               ;0F 00

          btst d0, (a7)             ;01 17
          btst d1, (a6)             ;03 16
          btst d2, (a5)             ;05 15
          btst d3, (a4)             ;07 14
          btst d4, (a3)             ;09 13
          btst d5, (a2)             ;0B 12
          btst d6, (a1)             ;0D 11
          btst d7, (a0)             ;0F 10

          btst d0, $FF(a7)          ;01 2F 00 FF
          btst d1, $FE(a6)          ;03 2E 00 FE
          btst d2, $02(a5)          ;05 2D 00 02
          btst d3, $01(a4)          ;07 2C 00 01
          btst d4, LATE_FF(a3)      ;09 2B 00 FF
          btst d5, LATE_FE(a2)      ;0B 2A 00 FE
          btst d6, LATE_02(a1)      ;0D 29 00 02
          btst d7, LATE_01(a0)      ;0F 28 00 01


          bclr #0, ($FF00).w        ;08 B8 00 00 FF 00
          bclr #1, ($0100).w        ;08 B8 00 01 01 00
          bclr #2, ($00FF).w        ;08 B8 00 02 00 FF
          bclr #3, ($0001).w        ;08 B8 00 03 00 01
          bclr #4, (LATE_FF00).w    ;08 B8 00 04 FF 00
          bclr #5, (LATE_0100).w    ;08 B8 00 05 01 00
          bclr #6, (LATE_00FF).w    ;08 B8 00 06 00 FF
          bclr #7, (LATE_0001).w    ;08 B8 00 07 00 01

          bclr #0, d7               ;08 87 00 00
          bclr #1, d6               ;08 86 00 01
          bclr #2, d5               ;08 85 00 02
          bclr #3, d4               ;08 84 00 03
          bclr #4, d3               ;08 83 00 04
          bclr #5, d2               ;08 82 00 05
          bclr #6, d1               ;08 81 00 06
          bclr #7, d0               ;08 80 00 07

          bclr #0, (a7)             ;08 97 00 00
          bclr #1, (a6)             ;08 96 00 01
          bclr #2, (a5)             ;08 95 00 02
          bclr #3, (a4)             ;08 94 00 03
          bclr #4, (a3)             ;08 93 00 04
          bclr #5, (a2)             ;08 92 00 05
          bclr #6, (a1)             ;08 91 00 06
          bclr #7, (a0)             ;08 90 00 07

          bclr #0, $FF(a7)          ;08 AF 00 00 00 FF
          bclr #1, $FE(a6)          ;08 AE 00 01 00 FE
          bclr #2, $02(a5)          ;08 AD 00 02 00 02
          bclr #3, $01(a4)          ;08 AC 00 03 00 01
          bclr #4, LATE_FF(a3)      ;08 AB 00 04 00 FF
          bclr #5, LATE_FE(a2)      ;08 AA 00 05 00 FE
          bclr #6, LATE_02(a1)      ;08 A9 00 06 00 02
          bclr #7, LATE_01(a0)      ;08 A8 00 07 00 01

          bclr d0, ($FF00).w        ;01 B8 FF 00
          bclr d1, ($0100).w        ;03 B8 01 00
          bclr d2, ($00FF).w        ;05 B8 00 FF
          bclr d3, ($0001).w        ;07 B8 00 01
          bclr d4, (LATE_FF00).w    ;09 B8 FF 00
          bclr d5, (LATE_0100).w    ;0B B8 01 00
          bclr d6, (LATE_00FF).w    ;0D B8 00 FF
          bclr d7, (LATE_0001).w    ;0F B8 00 01

          bclr d0, d7               ;01 87
          bclr d1, d6               ;03 86
          bclr d2, d5               ;05 85
          bclr d3, d4               ;07 84
          bclr d4, d3               ;09 83
          bclr d5, d2               ;0B 82
          bclr d6, d1               ;0D 81
          bclr d7, d0               ;0F 80

          bclr d0, (a7)             ;01 97
          bclr d1, (a6)             ;03 96
          bclr d2, (a5)             ;05 95
          bclr d3, (a4)             ;07 94
          bclr d4, (a3)             ;09 93
          bclr d5, (a2)             ;0B 92
          bclr d6, (a1)             ;0D 91
          bclr d7, (a0)             ;0F 90

          bclr d0, $FF(a7)          ;01 AF 00 FF
          bclr d1, $FE(a6)          ;03 AE 00 FE
          bclr d2, $02(a5)          ;05 AD 00 02
          bclr d3, $01(a4)          ;07 AC 00 01
          bclr d4, LATE_FF(a3)      ;09 AB 00 FF
          bclr d5, LATE_FE(a2)      ;0B AA 00 FE
          bclr d6, LATE_02(a1)      ;0D A9 00 02
          bclr d7, LATE_01(a0)      ;0F A8 00 01


          bset #0, ($FF00).w        ;08 F8 00 00 FF 00
          bset #1, ($0100).w        ;08 F8 00 01 01 00
          bset #2, ($00FF).w        ;08 F8 00 02 00 FF
          bset #3, ($0001).w        ;08 F8 00 03 00 01
          bset #4, (LATE_FF00).w    ;08 F8 00 04 FF 00
          bset #5, (LATE_0100).w    ;08 F8 00 05 01 00
          bset #6, (LATE_00FF).w    ;08 F8 00 06 00 FF
          bset #7, (LATE_0001).w    ;08 F8 00 07 00 01

          bset #0, d7               ;08 C7 00 00
          bset #1, d6               ;08 C6 00 01
          bset #2, d5               ;08 C5 00 02
          bset #3, d4               ;08 C4 00 03
          bset #4, d3               ;08 C3 00 04
          bset #5, d2               ;08 C2 00 05
          bset #6, d1               ;08 C1 00 06
          bset #7, d0               ;08 C0 00 07

          bset #0, (a7)             ;08 D7 00 00
          bset #1, (a6)             ;08 D6 00 01
          bset #2, (a5)             ;08 D5 00 02
          bset #3, (a4)             ;08 D4 00 03
          bset #4, (a3)             ;08 D3 00 04
          bset #5, (a2)             ;08 D2 00 05
          bset #6, (a1)             ;08 D1 00 06
          bset #7, (a0)             ;08 D0 00 07

          bset #0, $FF(a7)          ;08 EF 00 00 00 FF
          bset #1, $FE(a6)          ;08 EE 00 01 00 FE
          bset #2, $02(a5)          ;08 ED 00 02 00 02
          bset #3, $01(a4)          ;08 EC 00 03 00 01
          bset #4, LATE_FF(a3)      ;08 EB 00 04 00 FF
          bset #5, LATE_FE(a2)      ;08 EA 00 05 00 FE
          bset #6, LATE_02(a1)      ;08 E9 00 06 00 02
          bset #7, LATE_01(a0)      ;08 E8 00 07 00 01

          bset d0, ($FF00).w        ;01 F8 FF 00
          bset d1, ($0100).w        ;03 F8 01 00
          bset d2, ($00FF).w        ;05 F8 00 FF
          bset d3, ($0001).w        ;07 F8 00 01
          bset d4, (LATE_FF00).w    ;09 F8 FF 00
          bset d5, (LATE_0100).w    ;0B F8 01 00
          bset d6, (LATE_00FF).w    ;0D F8 00 FF
          bset d7, (LATE_0001).w    ;0F F8 00 01

          bset d0, d7               ;01 C7
          bset d1, d6               ;03 C6
          bset d2, d5               ;05 C5
          bset d3, d4               ;07 C4
          bset d4, d3               ;09 C3
          bset d5, d2               ;0B C2
          bset d6, d1               ;0D C1
          bset d7, d0               ;0F C0

          bset d0, (a7)             ;01 D7
          bset d1, (a6)             ;03 D6
          bset d2, (a5)             ;05 D5
          bset d3, (a4)             ;07 D4
          bset d4, (a3)             ;09 D3
          bset d5, (a2)             ;0B D2
          bset d6, (a1)             ;0D D1
          bset d7, (a0)             ;0F D0

          bset d0, $FF(a7)          ;01 EF 00 FF
          bset d1, $FE(a6)          ;03 EE 00 FE
          bset d2, $02(a5)          ;05 ED 00 02
          bset d3, $01(a4)          ;07 EC 00 01
          bset d4, LATE_FF(a3)      ;09 EB 00 FF
          bset d5, LATE_FE(a2)      ;0B EA 00 FE
          bset d6, LATE_02(a1)      ;0D E9 00 02
          bset d7, LATE_01(a0)      ;0F E8 00 01


          bchg #0, ($FF00).w        ;08 78 00 00 FF 00
          bchg #1, ($0100).w        ;08 78 00 01 01 00
          bchg #2, ($00FF).w        ;08 78 00 02 00 FF
          bchg #3, ($0001).w        ;08 78 00 03 00 01
          bchg #4, (LATE_FF00).w    ;08 78 00 04 FF 00
          bchg #5, (LATE_0100).w    ;08 78 00 05 01 00
          bchg #6, (LATE_00FF).w    ;08 78 00 06 00 FF
          bchg #7, (LATE_0001).w    ;08 78 00 07 00 01

          bchg #0, d7               ;08 47 00 00
          bchg #1, d6               ;08 46 00 01
          bchg #2, d5               ;08 45 00 02
          bchg #3, d4               ;08 44 00 03
          bchg #4, d3               ;08 43 00 04
          bchg #5, d2               ;08 42 00 05
          bchg #6, d1               ;08 41 00 06
          bchg #7, d0               ;08 40 00 07

          bchg #0, (a7)             ;08 57 00 00
          bchg #1, (a6)             ;08 56 00 01
          bchg #2, (a5)             ;08 55 00 02
          bchg #3, (a4)             ;08 54 00 03
          bchg #4, (a3)             ;08 53 00 04
          bchg #5, (a2)             ;08 52 00 05
          bchg #6, (a1)             ;08 51 00 06
          bchg #7, (a0)             ;08 50 00 07

          bchg #0, $FF(a7)          ;08 6F 00 00 00 FF
          bchg #1, $FE(a6)          ;08 6E 00 01 00 FE
          bchg #2, $02(a5)          ;08 6D 00 02 00 02
          bchg #3, $01(a4)          ;08 6C 00 03 00 01
          bchg #4, LATE_FF(a3)      ;08 6B 00 04 00 FF
          bchg #5, LATE_FE(a2)      ;08 6A 00 05 00 FE
          bchg #6, LATE_02(a1)      ;08 69 00 06 00 02
          bchg #7, LATE_01(a0)      ;08 68 00 07 00 01

          bchg d0, ($FF00).w        ;01 78 FF 00
          bchg d1, ($0100).w        ;03 78 01 00
          bchg d2, ($00FF).w        ;05 78 00 FF
          bchg d3, ($0001).w        ;07 78 00 01
          bchg d4, (LATE_FF00).w    ;09 78 FF 00
          bchg d5, (LATE_0100).w    ;0B 78 01 00
          bchg d6, (LATE_00FF).w    ;0D 78 00 FF
          bchg d7, (LATE_0001).w    ;0F 78 00 01

          bchg d0, d7               ;01 47
          bchg d1, d6               ;03 46
          bchg d2, d5               ;05 45
          bchg d3, d4               ;07 44
          bchg d4, d3               ;09 43
          bchg d5, d2               ;0B 42
          bchg d6, d1               ;0D 41
          bchg d7, d0               ;0F 40

          bchg d0, (a7)             ;01 57
          bchg d1, (a6)             ;03 56
          bchg d2, (a5)             ;05 55
          bchg d3, (a4)             ;07 54
          bchg d4, (a3)             ;09 53
          bchg d5, (a2)             ;0B 52
          bchg d6, (a1)             ;0D 51
          bchg d7, (a0)             ;0F 50

          bchg d0, $FF(a7)          ;01 6F 00 FF
          bchg d1, $FE(a6)          ;03 6E 00 FE
          bchg d2, $02(a5)          ;05 6D 00 02
          bchg d3, $01(a4)          ;07 6C 00 01
          bchg d4, LATE_FF(a3)      ;09 6B 00 FF
          bchg d5, LATE_FE(a2)      ;0B 6A 00 FE
          bchg d6, LATE_02(a1)      ;0D 69 00 02
          bchg d7, LATE_01(a0)      ;0F 68 00 01

          clr.w d0                  ;42 40
          clr.w d1                  ;42 41
          clr.w d2                  ;42 42
          clr.w d3                  ;42 43
          clr.w d4                  ;42 44
          clr.w d5                  ;42 45
          clr.w d6                  ;42 46
          clr.w d7                  ;42 47

          clr.l d0                  ;42 80
          clr.l d1                  ;42 81
          clr.l d2                  ;42 82
          clr.l d3                  ;42 83
          clr.l d4                  ;42 84
          clr.l d5                  ;42 85
          clr.l d6                  ;42 86
          clr.l d7                  ;42 87

          clr.w ($0001).w           ;42 78 00 01
          clr.w ($00FF).w           ;42 78 00 FF
          clr.w ($0100).w           ;42 78 01 00
          clr.w ($FF00).w           ;42 78 FF 00
          clr.w (LATE_0001).w       ;42 78 00 01
          clr.w (LATE_00FF).w       ;42 78 00 FF
          clr.w (LATE_0100).w       ;42 78 01 00
          clr.w (LATE_FF00).w       ;42 78 FF 00

          clr.b $01(a7)             ;42 2F 00 01
          clr.b $02(a6)             ;42 2E 00 02
          clr.b $FE(a5)             ;42 2D 00 FE
          clr.b $FF(a4)             ;42 2C 00 FF
          clr.b LATE_01(a3)         ;42 2B 00 01
          clr.b LATE_02(a2)         ;42 2A 00 02
          clr.b LATE_FE(a1)         ;42 29 00 FE
          clr.b LATE_FF(a0)         ;42 28 00 FF

          clr.w $01(a7)             ;42 6F 00 01
          clr.w $02(a6)             ;42 6E 00 02
          clr.w $FE(a5)             ;42 6D 00 FE
          clr.w $FF(a4)             ;42 6C 00 FF
          clr.w LATE_01(a3)         ;42 6B 00 01
          clr.w LATE_02(a2)         ;42 6A 00 02
          clr.w LATE_FE(a1)         ;42 69 00 FE
          clr.w LATE_FF(a0)         ;42 68 00 FF

          clr.l (a7)                ;42 97
          clr.l (a6)                ;42 96
          clr.l (a5)                ;42 95
          clr.l (a4)                ;42 94
          clr.l (a3)                ;42 93
          clr.l (a2)                ;42 92
          clr.l (a1)                ;42 91
          clr.l (a0)                ;42 90

          clr.l $01(a7)             ;42 AF 00 01
          clr.l $02(a6)             ;42 AE 00 02
          clr.l $FE(a5)             ;42 AD 00 FE
          clr.l $FF(a4)             ;42 AC 00 FF
          clr.l LATE_01(a3)         ;42 AB 00 01
          clr.l LATE_02(a2)         ;42 AA 00 02
          clr.l LATE_FE(a1)         ;42 A9 00 FE
          clr.l LATE_FF(a0)         ;42 A8 00 FF

          clr.l (a7)+               ;42 9F
          clr.l (a6)+               ;42 9E
          clr.l (a5)+               ;42 9D
          clr.l (a4)+               ;42 9C
          clr.l (a3)+               ;42 9B
          clr.l (a2)+               ;42 9A
          clr.l (a1)+               ;42 99
          clr.l (a0)+               ;42 98

          clr.l -(a7)               ;42 A7
          clr.l -(a6)               ;42 A6
          clr.l -(a5)               ;42 A5
          clr.l -(a4)               ;42 A4
          clr.l -(a3)               ;42 A3
          clr.l -(a2)               ;42 A2
          clr.l -(a1)               ;42 A1
          clr.l -(a0)               ;42 A0

          cmpa.w #$0001,a7          ;BE FC 00 01
          cmpa.w #$00FF,a6          ;BC FC 00 FF
          cmpa.w #$0100,a5          ;BA FC 01 00
          cmpa.w #$FF00,a4          ;B8 FC FF 00
          cmpa.w #LATE_0001,a3      ;B6 FC 00 01
          cmpa.w #LATE_00FF,a2      ;B4 FC 00 FF
          cmpa.w #LATE_0100,a1      ;B2 FC 01 00
          cmpa.w #LATE_FF00,a0      ;B0 FC FF 00

          cmpa.l #$00000001,a7      ;BF FC 00 00 00 01
          cmpa.l #$0000FF00,a6      ;BD FC 00 00 FF 00
          cmpa.l #$00010000,a5      ;BB FC 00 01 00 00
          cmpa.l #$FF000000,a4      ;B9 FC FF 00 00 00
          cmpa.l #LATE_00000001,a3  ;B7 FC 00 00 00 01
          cmpa.l #LATE_0000FF00,a2  ;B5 FC 00 00 FF 00
          cmpa.l #LATE_00010000,a1  ;B3 FC 00 01 00 00
          cmpa.l #LATE_FF000000,a0  ;B1 FC FF 00 00 00

          cmpa.l ($0001).w,a7       ;BF F8 00 01
          cmpa.l ($00FF).w,a6       ;BD F8 00 FF
          cmpa.l ($0100).w,a5       ;BB F8 01 00
          cmpa.l ($FF00).w,a4       ;B9 F8 FF 00
          cmpa.l (LATE_0001).w,a3   ;B7 F8 00 01
          cmpa.l (LATE_00FF).w,a2   ;B5 F8 00 FF
          cmpa.l (LATE_0100).w,a1   ;B3 F8 01 00
          cmpa.l (LATE_FF00).w,a0   ;B1 F8 FF 00

          cmpa.w d7, a0             ;B0 C7
          cmpa.w d6, a1             ;B2 C6
          cmpa.w d5, a2             ;B4 C5
          cmpa.w d4, a3             ;B6 C4
          cmpa.w d3, a4             ;B8 C3
          cmpa.w d2, a5             ;BA C2
          cmpa.w d1, a6             ;BC C1
          cmpa.w d0, a7             ;BE C0

          cmpa.l (a7), a0           ;B1 DF
          cmpa.l (a6), a1           ;B3 DE
          cmpa.l (a5), a2           ;B5 DD
          cmpa.l (a4), a3           ;B7 DC
          cmpa.l (a3), a4           ;B9 DB
          cmpa.l (a2), a5           ;BB DA
          cmpa.l (a1), a6           ;BD D9
          cmpa.l (a0), a7           ;BF D8

          cmpa.l $01(a7), a0        ;B1 EF 00 01
          cmpa.l $02(a6), a1        ;B3 EE 00 02
          cmpa.l $FE(a5), a2        ;B5 ED 00 FE
          cmpa.l $FF(a4), a3        ;B7 EC 00 FF
          cmpa.l LATE_01(a3), a4    ;B9 EB 00 01
          cmpa.l LATE_02(a2), a5    ;BB EA 00 02
          cmpa.l LATE_FE(a1), a6    ;BD E9 00 FE
          cmpa.l LATE_FF(a0), a7    ;BF E8 00 FFs

          cmpa.l a7, a0             ;B1 CF
          cmpa.l a6, a1             ;B3 CE
          cmpa.l a5, a2             ;B5 CD
          cmpa.l a4, a3             ;B7 CC
          cmpa.l a3, a4             ;B9 CB
          cmpa.l a2, a5             ;BB CA
          cmpa.l a1, a6             ;BD C9
          cmpa.l a0, a7             ;BF C8

          cmpi.b #$01, ($FF00).w            ;0C 38 00 01 FF 00
          cmpi.b #$02, ($0100).w            ;0C 38 00 02 01 00
          cmpi.b #$FE, (LATE_00FF).w        ;0C 38 00 FE 00 FF
          cmpi.b #$FF, (LATE_0001).w        ;0C 38 00 FF 00 01
          cmpi.b #LATE_01, ($FF00).w        ;0C 38 00 01 FF 00
          cmpi.b #LATE_02, ($0100).w        ;0C 38 00 02 01 00
          cmpi.b #LATE_FE, (LATE_00FF).w    ;0C 38 00 FE 00 FF
          cmpi.b #LATE_FF, (LATE_0001).w    ;0C 38 00 FF 00 01

          cmpi.w #$0001, ($FF00).w          ;0C 78 00 01 FF 00
          cmpi.w #$00FF, ($0100).w          ;0C 78 00 FF 01 00
          cmpi.w #$0100, (LATE_00FF).w      ;0C 78 01 00 00 FF
          cmpi.w #$FF00, (LATE_0001).w      ;0C 78 FF 00 00 01
          cmpi.w #LATE_0001, ($FF00).w      ;0C 78 00 01 FF 00
          cmpi.w #LATE_00FF, ($0100).w      ;0C 78 00 FF 01 00
          cmpi.w #LATE_0100, (LATE_00FF).w  ;0C 78 01 00 00 FF
          cmpi.w #LATE_FF00, (LATE_0001).w  ;0C 78 FF 00 00 01

          cmpi.l #$00000001, ($FF00).w          ;0C B8 00 00 00 01 FF 00
          cmpi.l #$0000FF00, ($0100).w          ;0C B8 00 00 FF 00 01 00
          cmpi.l #$00010000, (LATE_00FF).w      ;0C B8 00 01 00 00 00 FF
          cmpi.l #$FF000000, (LATE_0001).w      ;0C B8 FF 00 00 00 00 01
          cmpi.l #LATE_00000001, ($FF00).w      ;0C B8 00 00 00 01 FF 00
          cmpi.l #LATE_0000FF00, ($0100).w      ;0C B8 00 00 FF 00 01 00
          cmpi.l #LATE_00010000, (LATE_00FF).w  ;0C B8 00 01 00 00 00 FF
          cmpi.l #LATE_FF000000, (LATE_0001).w  ;0C B8 FF 00 00 00 00 01

          cmpi.w #$01, ($FF000000).l            ;0C 79 00 01 FF 00 00 00
          cmpi.w #$02, ($00010000).l            ;0C 79 00 02 00 01 00 00
          cmpi.w #$FE, (LATE_0000FF00).l        ;0C 79 00 FE 00 00 FF 00
          cmpi.w #$FF, (LATE_00000001).l        ;0C 79 00 FF 00 00 00 01
          cmpi.w #LATE_01, ($FF000000).l        ;0C 79 00 01 FF 00 00 00
          cmpi.w #LATE_02, ($00010000).l        ;0C 79 00 02 00 01 00 00
          cmpi.w #LATE_FE, (LATE_0000FF00).l    ;0C 79 00 FE 00 00 FF 00
          cmpi.w #LATE_FF, (LATE_00000001).l    ;0C 79 00 FF 00 00 00 01

          cmpi.b #$01, d7             ;0C 07 00 01
          cmpi.b #$02, d6             ;0C 06 00 02
          cmpi.b #$FE, d5             ;0C 05 00 FE
          cmpi.b #$FF, d4             ;0C 04 00 FF
          cmpi.b #LATE_01, d3         ;0C 03 00 01
          cmpi.b #LATE_02, d2         ;0C 02 00 02
          cmpi.b #LATE_FE, d1         ;0C 01 00 FE
          cmpi.b #LATE_FF, d0         ;0C 00 00 FF

          cmpi.w #$0001, d7           ;0C 47 00 01
          cmpi.w #$00FF, d6           ;0C 46 00 FF
          cmpi.w #$0100, d5           ;0C 45 01 00
          cmpi.w #$FF00, d4           ;0C 44 FF 00
          cmpi.w #LATE_0001, d3       ;0C 43 00 01
          cmpi.w #LATE_00FF, d2       ;0C 42 00 FF
          cmpi.w #LATE_0100, d1       ;0C 41 01 00
          cmpi.w #LATE_FF00, d0       ;0C 40 FF 00

          cmpi.l #$00000001, d7       ;0C 87 00 00 00 01
          cmpi.l #$0000FF00, d6       ;0C 86 00 00 FF 00
          cmpi.l #$00010000, d5       ;0C 85 00 01 00 00
          cmpi.l #$FF000000, d4       ;0C 84 FF 00 00 00
          cmpi.l #LATE_00000001, d3   ;0C 83 00 00 00 01
          cmpi.l #LATE_0000FF00, d2   ;0C 82 00 00 FF 00
          cmpi.l #LATE_00010000, d1   ;0C 81 00 01 00 00
          cmpi.l #LATE_FF000000, d0   ;0C 80 FF 00 00 00

          cmpi.b #$01, (a7)           ;0C 17 00 01
          cmpi.b #$02, (a6)           ;0C 16 00 02
          cmpi.b #$FE, (a5)           ;0C 15 00 FE
          cmpi.b #$FF, (a4)           ;0C 14 00 FF
          cmpi.b #LATE_01, (a3)       ;0C 13 00 01
          cmpi.b #LATE_02, (a2)       ;0C 12 00 02
          cmpi.b #LATE_FE, (a1)       ;0C 11 00 FE
          cmpi.b #LATE_FF, (a0)       ;0C 10 00 FF

          cmpi.b #$01, $FF(a7)           ;0C 2F 00 01 00 FF
          cmpi.b #$02, $FE(a6)           ;0C 2E 00 02 00 FE
          cmpi.b #$FE, LATE_02(a5)       ;0C 2D 00 FE 00 02
          cmpi.b #$FF, LATE_01(a4)       ;0C 2C 00 FF 00 01
          cmpi.b #LATE_01, $FF(a3)       ;0C 2B 00 01 00 FF
          cmpi.b #LATE_02, $FE(a2)       ;0C 2A 00 02 00 FE
          cmpi.b #LATE_FE, LATE_02(a1)   ;0C 29 00 FE 00 02
          cmpi.b #LATE_FF, LATE_01(a0)   ;0C 28 00 FF 00 01

          cmpi.w #$0001, $FF(a7)         ;0C 6F 00 01 00 FF
          cmpi.w #$00FF, $FE(a6)         ;0C 6E 00 FF 00 FE
          cmpi.w #$0100, LATE_02(a5)     ;0C 6D 01 00 00 02
          cmpi.w #$FF00, LATE_01(a4)     ;0C 6C FF 00 00 01
          cmpi.w #LATE_0001, $FF(a3)     ;0C 6B 00 01 00 FF
          cmpi.w #LATE_00FF, $FE(a2)     ;0C 6A 00 FF 00 FE
          cmpi.w #LATE_0100, LATE_02(a1) ;0C 69 01 00 00 02
          cmpi.w #LATE_FF00, LATE_01(a0) ;0C 68 FF 00 00 01

          cmpi.b #$01, (a7)+          ;0C 1F 00 01
          cmpi.b #$02, (a6)+          ;0C 1E 00 02
          cmpi.b #$FE, (a5)+          ;0C 1D 00 FE
          cmpi.b #$FF, (a4)+          ;0C 1C 00 FF
          cmpi.b #LATE_01, (a3)+      ;0C 1B 00 01
          cmpi.b #LATE_02, (a2)+      ;0C 1A 00 02
          cmpi.b #LATE_FE, (a1)+      ;0C 19 00 FE
          cmpi.b #LATE_FF, (a0)+      ;0C 18 00 FF

          cmpi.b #$01, -(a7)          ;0C 27 00 01
          cmpi.b #$02, -(a6)          ;0C 26 00 02
          cmpi.b #$FE, -(a5)          ;0C 25 00 FE
          cmpi.b #$FF, -(a4)          ;0C 24 00 FF
          cmpi.b #LATE_01, -(a3)      ;0C 23 00 01
          cmpi.b #LATE_02, -(a2)      ;0C 22 00 02
          cmpi.b #LATE_FE, -(a1)      ;0C 21 00 FE
          cmpi.b #LATE_FF, -(a0)      ;0C 20 00 FF

          cmp.b d0, d7                ;BE 00
          cmp.b d1, d6                ;BC 01
          cmp.b d2, d5                ;BA 02
          cmp.b d3, d4                ;B8 03
          cmp.b d4, d3                ;B6 04
          cmp.b d5, d2                ;B4 05
          cmp.b d6, d1                ;B2 06
          cmp.b d7, d0                ;B0 07

          cmp.l d0, d7                ;BE 80
          cmp.l d1, d6                ;BC 81
          cmp.l d2, d5                ;BA 82
          cmp.l d3, d4                ;B8 83
          cmp.l d4, d3                ;B6 84
          cmp.l d5, d2                ;B4 85
          cmp.l d6, d1                ;B2 86
          cmp.l d7, d0                ;B0 87

          cmp.w ($0001).w, d0         ;B0 78 00 01
          cmp.w ($00FF).w, d1         ;B2 78 00 FF
          cmp.w ($0100).w, d2         ;B4 78 01 00
          cmp.w ($FF00).w, d3         ;B6 78 FF 00
          cmp.w (LATE_0001).w, d4     ;B8 78 00 01
          cmp.w (LATE_00FF).w, d5     ;BA 78 00 FF
          cmp.w (LATE_0100).w, d6     ;BC 78 01 00
          cmp.w (LATE_FF00).w, d7     ;BE 78 FF 00

          cmp.l ($0001).w, d0         ;B0 B8 00 01
          cmp.l ($00FF).w, d1         ;B2 B8 00 FF
          cmp.l ($0100).w, d2         ;B4 B8 01 00
          cmp.l ($FF00).w, d3         ;B6 B8 FF 00
          cmp.l (LATE_0001).w, d4     ;B8 B8 00 01
          cmp.l (LATE_00FF).w, d5     ;BA B8 00 FF
          cmp.l (LATE_0100).w, d6     ;BC B8 01 00
          cmp.l (LATE_FF00).w, d7     ;BE B8 FF 00

          cmp.w (a7), d0              ;B0 57
          cmp.w (a6), d1              ;B2 56
          cmp.w (a5), d2              ;B4 55
          cmp.w (a4), d3              ;B6 54
          cmp.w (a3), d4              ;B8 53
          cmp.w (a2), d5              ;BA 52
          cmp.w (a1), d6              ;BC 51
          cmp.w (a0), d7              ;BE 50

          cmp.b $01(a0),d7            ;BE 28 00 01
          cmp.b $02(a1),d6            ;BC 29 00 02
          cmp.b $FE(a2),d5            ;BA 2A 00 FE
          cmp.b $FF(a3),d4            ;B8 2B 00 FF
          cmp.b LATE_01(a4),d3        ;B6 2C 00 01
          cmp.b LATE_02(a5),d2        ;B4 2D 00 02
          cmp.b LATE_FE(a6),d1        ;B2 2E 00 FE
          cmp.b LATE_FF(a7),d0        ;B0 2F 00 FF

          cmp.w $01(a0),d7            ;BE 68 00 01
          cmp.w $02(a1),d6            ;BC 69 00 02
          cmp.w $FE(a2),d5            ;BA 6A 00 FE
          cmp.w $FF(a3),d4            ;B8 6B 00 FF
          cmp.w LATE_01(a4),d3        ;B6 6C 00 01
          cmp.w LATE_02(a5),d2        ;B4 6D 00 02
          cmp.w LATE_FE(a6),d1        ;B2 6E 00 FE
          cmp.w LATE_FF(a7),d0        ;B0 6F 00 FF

          cmp.w (a7)+, d0             ;B0 5F
          cmp.w (a6)+, d1             ;B2 5E
          cmp.w (a5)+, d2             ;B4 5D
          cmp.w (a4)+, d3             ;B6 5C
          cmp.w (a3)+, d4             ;B8 5B
          cmp.w (a2)+, d5             ;BA 5A
          cmp.w (a1)+, d6             ;BC 59
          cmp.w (a0)+, d7             ;BE 58

          cmp.b -(a7), d0             ;B0 27
          cmp.b -(a6), d1             ;B2 26
          cmp.b -(a5), d2             ;B4 25
          cmp.b -(a4), d3             ;B6 24
          cmp.b -(a3), d4             ;B8 23
          cmp.b -(a2), d5             ;BA 22
          cmp.b -(a1), d6             ;BC 21
          cmp.b -(a0), d7             ;BE 20

-
          dbt d7, -               ;50 C7 FF FE
          dbt d6, -               ;50 C6 FF FA
          dbt d5, -               ;50 C5 FF F6
          dbt d4, -               ;50 C4 FF F2
          dbt d3, +               ;50 C3 00 0E
          dbt d2, +               ;50 C2 00 0A
          dbt d1, +               ;50 C1 00 06
          dbt d0, +               ;50 C0 00 02
+

-
          dbf d7, -               ;51 CF 00 01
          dbf d6, -               ;51 CE 00 FF
          dbf d5, -               ;51 CD 01 00
          dbf d4, -               ;51 CC FF 00
          dbf d3, +               ;51 CB 00 01
          dbf d2, +               ;51 CA 00 00
          dbf d1, +               ;51 C9 01 00
          dbf d0, +               ;51 C8 FF 00
+

-
          dbhi d7, -              ;52 D7 00 01
          dbhi d6, -              ;52 D6 00 FF
          dbhi d5, -              ;52 D5 01 00
          dbhi d4, -              ;52 D4 FF 00
          dbhi d3, +              ;52 D3 00 01
          dbhi d2, +              ;52 D2 00 00
          dbhi d1, +              ;52 D1 01 00
          dbhi d0, +              ;52 D0 FF 00
+
-
          dbls d7, -              ;53 DF 00 01
          dbls d6, -              ;53 DE 00 FF
          dbls d5, -              ;53 DD 01 00
          dbls d4, -              ;53 DC FF 00
          dbls d3, +              ;53 DB 00 01
          dbls d2, +              ;53 DA 00 00
          dbls d1, +              ;53 D9 01 00
          dbls d0, +              ;53 D8 FF 00
+
-
          dbcc d7, -              ;54 DF 00 01
          dbcc d6, -              ;54 DE 00 FF
          dbcc d5, -              ;54 DD 01 00
          dbcc d4, -              ;54 DC FF 00
          dbcc d3, +              ;54 DB 00 01
          dbcc d2, +              ;54 DA 00 00
          dbcc d1, +              ;54 D9 01 00
          dbcc d0, +              ;54 D8 FF 00
+
-
          dbcs d7, -              ;55 DF 00 01
          dbcs d6, -              ;55 DE 00 FF
          dbcs d5, -              ;55 DD 01 00
          dbcs d4, -              ;55 DC FF 00
          dbcs d3, +              ;55 DB 00 01
          dbcs d2, +              ;55 DA 00 00
          dbcs d1, +              ;55 D9 01 00
          dbcs d0, +              ;55 D8 FF 00
+
-
          dbne d7, -              ;56 DF 00 01
          dbne d6, -              ;56 DE 00 FF
          dbne d5, -              ;56 DD 01 00
          dbne d4, -              ;56 DC FF 00
          dbne d3, +              ;56 DB 00 01
          dbne d2, +              ;56 DA 00 00
          dbne d1, +              ;56 D9 01 00
          dbne d0, +              ;56 D8 FF 00
+

-
          dbeq d7, -              ;57 CF 00 01
          dbeq d6, -              ;57 CE 00 FF
          dbeq d5, -              ;57 CD 01 00
          dbeq d4, -              ;57 CC FF 00
          dbeq d3, +              ;57 CB 00 01
          dbeq d2, +              ;57 CA 00 00
          dbeq d1, +              ;57 C9 01 00
          dbeq d0, +              ;57 C8 FF 00
+
-
          dbvc d7, -              ;58 DF 00 01
          dbvc d6, -              ;58 DE 00 FF
          dbvc d5, -              ;58 DD 01 00
          dbvc d4, -              ;58 DC FF 00
          dbvc d3, +              ;58 DB 00 01
          dbvc d2, +              ;58 DA 00 00
          dbvc d1, +              ;58 D9 01 00
          dbvc d0, +              ;58 D8 FF 00
+
-
          dbvs d7, -              ;59 DF 00 01
          dbvs d6, -              ;59 DE 00 FF
          dbvs d5, -              ;59 DD 01 00
          dbvs d4, -              ;59 DC FF 00
          dbvs d3, +              ;59 DB 00 01
          dbvs d2, +              ;59 DA 00 00
          dbvs d1, +              ;59 D9 01 00
          dbvs d0, +              ;59 D8 FF 00
+
-
          dbpl d7, -              ;5A DF 00 01
          dbpl d6, -              ;5A DE 00 FF
          dbpl d5, -              ;5A DD 01 00
          dbpl d4, -              ;5A DC FF 00
          dbpl d3, +              ;5A DB 00 01
          dbpl d2, +              ;5A DA 00 00
          dbpl d1, +              ;5A D9 01 00
          dbpl d0, +              ;5A D8 FF 00
+
-
          dbmi d7, -              ;5B CF 00 01
          dbmi d6, -              ;5B CE 00 FF
          dbmi d5, -              ;5B CD 01 00
          dbmi d4, -              ;5B CC FF 00
          dbmi d3, +              ;5B CB 00 01
          dbmi d2, +              ;5B CA 00 00
          dbmi d1, +              ;5B C9 01 00
          dbmi d0, +              ;5B C8 FF 00
+
-
          dbge d7, -              ;5C DF 00 01
          dbge d6, -              ;5C DE 00 FF
          dbge d5, -              ;5C DD 01 00
          dbge d4, -              ;5C DC FF 00
          dbge d3, +              ;5C DB 00 01
          dbge d2, +              ;5C DA 00 00
          dbge d1, +              ;5C D9 01 00
          dbge d0, +              ;5C D8 FF 00
+
-
          dblt d7, -              ;5D DF 00 01
          dblt d6, -              ;5D DE 00 FF
          dblt d5, -              ;5D DD 01 00
          dblt d4, -              ;5D DC FF 00
          dblt d3, +              ;5D DB 00 01
          dblt d2, +              ;5D DA 00 00
          dblt d1, +              ;5D D9 01 00
          dblt d0, +              ;5D D8 FF 00
+
-
          dbgt d7, -              ;5E DF 00 01
          dbgt d6, -              ;5E DE 00 FF
          dbgt d5, -              ;5E DD 01 00
          dbgt d4, -              ;5E DC FF 00
          dbgt d3, +              ;5E DB 00 01
          dbgt d2, +              ;5E DA 00 00
          dbgt d1, +              ;5E D9 01 00
          dbgt d0, +              ;5E D8 FF 00
+
-
          dble d7, -              ;5F DF 00 01
          dble d6, -              ;5F DE 00 FF
          dble d5, -              ;5F DD 01 00
          dble d4, -              ;5F DC FF 00
          dble d3, +              ;5F DB 00 01
          dble d2, +              ;5F DA 00 00
          dble d1, +              ;5F D9 01 00
          dble d0, +              ;5F D8 FF 00
+

          divs.w #$01, d7         ;8F FC 00 01
          divs.w #$02, d6         ;8D FC 00 02
          divs.w #$FE, d5         ;8B FC 00 FE
          divs.w #$FF, d4         ;89 FC 00 FF
          divs.w #LATE_01, d3     ;87 FC 00 01
          divs.w #LATE_02, d2     ;85 FC 00 02
          divs.w #LATE_FE, d1     ;83 FC 00 FE
          divs.w #LATE_FF, d0     ;81 FC 00 FF

          divu.w ($0001).w, d7    ;8E F8 00 01
          divu.w ($00FF).w, d6    ;8C F8 00 FF
          divu.w ($0100).w, d5    ;8A F8 01 00
          divu.w ($FF00).w, d4    ;88 F8 FF 00
          divu.w ($0001).w, d3    ;86 F8 00 01
          divu.w ($00FF).w, d2    ;84 F8 00 FF
          divu.w ($0100).w, d1    ;82 F8 01 00
          divu.w ($FF00).w, d0    ;80 F8 F0 00

          divu.w d0, d7           ;8E C0
          divu.w d1, d6           ;8C C1
          divu.w d2, d5           ;8A C2
          divu.w d3, d4           ;88 C3
          divu.w d4, d3           ;86 C4
          divu.w d5, d2           ;84 C5
          divu.w d6, d1           ;82 C6
          divu.w d7, d0           ;80 C7

          divu.w (a0), d7         ;8E D0
          divu.w (a1), d6         ;8C D1
          divu.w (a2), d5         ;8A D2
          divu.w (a3), d4         ;88 D3
          divu.w (a4), d3         ;86 D4
          divu.w (a5), d2         ;84 D5
          divu.w (a6), d1         ;82 D6
          divu.w (a7), d0         ;80 D7

          divu.w $01(a0), d7      ;8E E8 00 01
          divu.w $02(a1), d6      ;8C E9 00 02
          divu.w $FE(a2), d5      ;8A EA 00 FE
          divu.w $FF(a3), d4      ;88 EB 00 FF
          divu.w LATE_01(a4), d3  ;86 EC 00 01
          divu.w LATE_02(a5), d2  ;84 ED 00 02
          divu.w LATE_FE(a6), d1  ;82 EE 00 FE
          divu.w LATE_FF(a7), d0  ;80 EF 00 FF

          divu.w (a0)+, d7        ;8E D8
          divu.w (a1)+, d6        ;8C D9
          divu.w (a2)+, d5        ;8A DA
          divu.w (a3)+, d4        ;88 DB
          divu.w (a4)+, d3        ;86 DC
          divu.w (a5)+, d2        ;84 DD
          divu.w (a6)+, d1        ;82 DE
          divu.w (a7)+, d0        ;80 DF

          divu.w -(a0), d7        ;8E E0
          divu.w -(a1), d6        ;8C E1
          divu.w -(a2), d5        ;8A E2
          divu.w -(a3), d4        ;88 E3
          divu.w -(a4), d3        ;86 E4
          divu.w -(a5), d2        ;84 E5
          divu.w -(a6), d1        ;82 E6
          divu.w -(a7), d0        ;80 E7

          eori.w #$0001, d7       ;0A 47 00 01
          eori.w #$00FF, d6       ;0A 46 00 FF
          eori.w #$0100, d5       ;0A 45 01 00
          eori.w #$FF00, d4       ;0A 44 FF 00
          eori.w #LATE_0001, d3   ;0A 43 00 01
          eori.w #LATE_00FF, d2   ;0A 42 00 FF
          eori.w #LATE_0100, d1   ;0A 41 01 00
          eori.w #LATE_FF00, d0   ;0A 40 FF 00

          eori.b #$01, ($0001).w          ;0A 38 00 01 00 01
          eori.b #$02, ($00FF).w          ;0A 38 00 02 00 FF
          eori.b #$FE, (LATE_0100).w      ;0A 38 00 FE 01 00
          eori.b #$FF, (LATE_FF00).w      ;0A 38 00 FF FF 00
          eori.b #LATE_01, ($0001).w      ;0A 38 00 01 00 01
          eori.b #LATE_02, ($00FF).w      ;0A 38 00 02 00 FF
          eori.b #LATE_FE, (LATE_0100).w  ;0A 38 00 FE 01 00
          eori.b #LATE_FF, (LATE_FF00).w  ;0A 38 00 FF FF 00

          eori.w #$0001, ($FF00).w          ;0A 78 00 01 FF 00
          eori.w #$00FF, ($0100).w          ;0A 78 00 FF 01 00
          eori.w #$0100, (LATE_00FF).w      ;0A 78 01 00 00 FF
          eori.w #$FF00, (LATE_0001).w      ;0A 78 FF 00 00 01
          eori.w #LATE_0001, ($FF00).w      ;0A 78 00 01 FF 00
          eori.w #LATE_00FF, ($0100).w      ;0A 78 00 FF 01 00
          eori.w #LATE_0100, (LATE_00FF).w  ;0A 78 01 00 00 FF
          eori.w #LATE_FF00, (LATE_0001).w  ;0A 78 FF 00 00 01

          eori.l #$00000001,d0            ;0A 80 00 00 00 01
          eori.l #$0000FF00,d1            ;0A 81 00 00 FF 00
          eori.l #$00010000,d2            ;0A 82 00 01 00 00
          eori.l #$FF000000,d3            ;0A 83 FF 00 00 00
          eori.l #LATE_00000001,d4        ;0A 84 00 00 00 01
          eori.l #LATE_0000FF00,d5        ;0A 85 00 00 FF 00
          eori.l #LATE_00010000,d6        ;0A 86 00 01 00 00
          eori.l #LATE_FF000000,d7        ;0A 87 FF 00 00 00

          eori.l #$0001, (a7)             ;0A 97 00 00 00 01
          eori.l #$00FF, (a6)             ;0A 96 00 00 00 FF
          eori.l #$0100, (a5)             ;0A 95 00 00 01 00
          eori.l #$FF00, (a4)             ;0A 94 00 00 FF 00
          eori.l #LATE_0001, (a3)         ;0A 93 00 00 00 01
          eori.l #LATE_00FF, (a2)         ;0A 92 00 00 00 FF
          eori.l #LATE_0100, (a1)         ;0A 91 00 00 01 00
          eori.l #LATE_FF00, (a0)         ;0A 90 00 00 FF 00

          eori.l #$0001, $FF(a0)          ;0A A8 00 00 00 01 00 FF
          eori.l #$00FF, $FE(a1)          ;0A A9 00 00 00 FF 00 FE
          eori.l #$0100, LATE_02(a2)      ;0A AA 00 00 01 00 00 02
          eori.l #$FF00, LATE_01(a3)      ;0A AB 00 00 FF 00 00 01
          eori.l #LATE_0001, $FF(a4)      ;0A AC 00 00 00 01 00 FF
          eori.l #LATE_00FF, $FE(a5)      ;0A AD 00 00 00 FF 00 FE
          eori.l #LATE_0100, LATE_02(a6)  ;0A AE 00 00 01 00 00 02
          eori.l #LATE_FF00, LATE_01(a7)  ;0A AF 00 00 FF 00 00 01

          eori.l #$0001, (a7)+            ;0A 9F 00 00 00 01
          eori.l #$00FF, (a6)+            ;0A 9E 00 00 00 FF
          eori.l #$0100, (a5)+            ;0A 9D 00 00 01 00
          eori.l #$FF00, (a4)+            ;0A 9C 00 00 FF 00
          eori.l #LATE_0001, (a3)+        ;0A 9B 00 00 00 01
          eori.l #LATE_00FF, (a2)+        ;0A 9A 00 00 00 FF
          eori.l #LATE_0100, (a1)+        ;0A 99 00 00 01 00
          eori.l #LATE_FF00, (a0)+        ;0A 98 00 00 FF 00

          eori.l #$0001, -(a7)            ;0A A7 00 00 00 01
          eori.l #$00FF, -(a6)            ;0A A6 00 00 00 FF
          eori.l #$0100, -(a5)            ;0A A5 00 00 01 00
          eori.l #$FF00, -(a4)            ;0A A4 00 00 FF 00
          eori.l #LATE_0001, -(a3)        ;0A A3 00 00 00 01
          eori.l #LATE_00FF, -(a2)        ;0A A2 00 00 00 FF
          eori.l #LATE_0100, -(a1)        ;0A A1 00 00 01 00
          eori.l #LATE_FF00, -(a0)        ;0A A0 00 00 FF 00

          eori #$0001, sr               ;0A 7C 00 01
          eori #$FF00, sr               ;0A 7C FF 00
          eori #LATE_0001, sr           ;0A 7C 00 01
          eori #LATE_FF00, sr           ;0A 7C FF 00

          eor.b d0,d7             ;BF 00
          eor.b d1,d6             ;BD 01
          eor.b d2,d5             ;BB 02
          eor.b d3,d4             ;B9 03
          eor.b d4,d3             ;B7 04
          eor.b d5,d2             ;B5 05
          eor.b d6,d1             ;B3 06
          eor.b d7,d0             ;B1 07

          eor.w d0,d7             ;BF 40
          eor.w d1,d6             ;BD 41
          eor.w d2,d5             ;BB 42
          eor.w d3,d4             ;B9 43
          eor.w d4,d3             ;B7 44
          eor.w d5,d2             ;B5 45
          eor.w d6,d1             ;B3 46
          eor.w d7,d0             ;B1 47

          exg d0,d7               ;CF 40
          exg d1,d6               ;CD 41
          exg d2,d5               ;CB 42
          exg d3,d4               ;C9 43
          exg d4,d3               ;C7 44
          exg d5,d2               ;C5 45
          exg d6,d1               ;C3 46
          exg d7,d0               ;C1 47

          exg a0,a7               ;CF 48
          exg a1,a6               ;CD 49
          exg a2,a5               ;CB 4A
          exg a3,a4               ;C9 4B
          exg a4,a3               ;C7 4C
          exg a5,a2               ;C5 4D
          exg a6,a1               ;C3 4E
          exg a7,a0               ;C1 4F

          exg d0,a7               ;CF 88
          exg d1,a6               ;CD 89
          exg d2,a5               ;CB 8A
          exg d3,a4               ;C9 8B
          exg d4,a3               ;C7 8C
          exg d5,a2               ;C5 8D
          exg d6,a1               ;C3 8E
          exg d7,a0               ;C1 8F

          ext.w d0                ;48 80
          ext.w d1                ;48 81
          ext.w d2                ;48 82
          ext.w d3                ;48 83
          ext.w d4                ;48 84
          ext.w d5                ;48 85
          ext.w d6                ;48 86
          ext.w d7                ;48 87

          ext.l d0                ;48 C0
          ext.l d1                ;48 C1
          ext.l d2                ;48 C2
          ext.l d3                ;48 C3
          ext.l d4                ;48 C4
          ext.l d5                ;48 C5
          ext.l d6                ;48 C6
          ext.l d7                ;48 C7

          illegal                 ;4A FC

          jmp (a0)                ;4E D0
          jmp (a1)                ;4E D1
          jmp (a2)                ;4E D2
          jmp (a3)                ;4E D3
          jmp (a4)                ;4E D4
          jmp (a5)                ;4E D5
          jmp (a6)                ;4E D6
          jmp (a7)                ;4E D7

          jmp $01(a0)             ;4E E8 00 01
          jmp $02(a1)             ;4E E9 00 02
          jmp $FE(a2)             ;4E EA 00 FE
          jmp $FF(a3)             ;4E EB 00 FF
          jmp LATE_01(a4)         ;4E EC 00 01
          jmp LATE_02(a5)         ;4E ED 00 02
          jmp LATE_FE(a6)         ;4E EE 00 FE
          jmp LATE_FF(a7)         ;4E EF 00 FF

          jmp $123456             ;4E F9 00 12 34 56
          jmp LATE_123456         ;4E F9 00 12 34 56

          jsr $123456             ;4E B9 00 12 34 56
          jsr LATE_123456         ;4E B9 00 12 34 56


          lea ($0001).w, a0         ;41 F8 00 01
          lea ($00FF).w, a1         ;43 F8 00 FF
          lea ($0100).w, a2         ;45 F8 01 00
          lea ($FF00).w, a3         ;47 F8 FF 00
          lea (LATE_0001).w, a4     ;49 F8 00 01
          lea (LATE_00FF).w, a5     ;4B F8 00 FF
          lea (LATE_0100).w, a6     ;4D F8 01 00
          lea (LATE_FF00).w, a7     ;4F F8 FF 00
          lea (LATE_FF00).w, sp     ;4F F8 FF 00

          lea ($000001).l, a0       ;41 F9 00 00 00 01
          lea ($00FF00).l, a1       ;43 F9 00 00 FF 00
          lea ($010000).l, a2       ;45 F9 00 01 00 00
          lea ($FF0000).l, a3       ;47 F9 00 FF 00 00
          lea (LATE_000001).l, a4   ;49 F9 00 00 00 01
          lea (LATE_00FF00).l, a5   ;4B F9 00 00 FF 00
          lea (LATE_010000).l, a6   ;4D F9 00 01 00 00
          lea (LATE_FF0000).l, a7   ;4F F9 00 FF 00 00
          lea ($123456).l, sp       ;4F F9 00 12 34 56

          lea d7, a0                ;41 C7
          lea d6, a1                ;43 C6
          lea d5, a2                ;45 C5
          lea d4, a3                ;47 C4
          lea d3, a4                ;49 C3
          lea d2, a5                ;4B C2
          lea d1, a6                ;4D C1
          lea d0, a7                ;4F C0
          lea d0, sp                ;4F C0

          lea (sp), a0              ;41 D7
          lea (a7), a1              ;43 D7
          lea (a6), a2              ;45 D6
          lea (a5), a3              ;47 D5
          lea (a4), a4              ;49 D4
          lea (a3), a5              ;4B D3
          lea (a2), a6              ;4D D2
          lea (a1), a7              ;4F D1
          lea (a0), sp              ;4F D0

          lea $0001(a0), a7         ;4F E8 00 01
          lea $00FF(a1), a6         ;4D E9 00 FF
          lea $0100(a2), a5         ;4B EA 01 00
          lea $FF00(a3), a4         ;49 EB FF 00
          lea LATE_0001(a4), a3     ;47 EC 00 01
          lea LATE_00FF(a5), a2     ;45 ED 00 FF
          lea LATE_0100(a6), a1     ;43 EE 01 00
          lea LATE_FF00(a7), a0     ;41 EF FF 00

          lea (a0,d7.w), a7         ;4F F0 70 00
          lea (a1,d6.w), a6         ;4D F1 60 00
          lea (a2,d5.w), a5         ;4B F2 50 00
          lea (a3,d4.w), a4         ;49 F3 40 00
          lea (a4,d3.w), a3         ;47 F4 30 00
          lea (a5,d2.w), a2         ;45 F5 20 00
          lea (a6,d1.w), a1         ;43 F6 10 00
          lea (a7,d0.w), a0         ;41 F7 00 00

          lea $01(a0,d7.w), a7      ;4F F0 70 01
          lea $FF(a1,d6.w), a6      ;4D F1 60 FF
          lea LATE_01(a2,d5.w), a5  ;4B F2 50 01
          lea LATE_FF(a3,d4.w), a4  ;49 F3 40 FF
          lea $01(a4,d3.w), a3      ;47 F4 30 01
          lea $FF(a5,d2.w), a2      ;45 F5 20 FF
          lea LATE_01(a6,d1.w), a1  ;43 F6 10 01
          lea LATE_FF(a7,d0.w), a0  ;41 F7 00 FF

          lea $01(pc,d7.w), a7      ;4F FB 70 01
          lea $FF(pc,d6.w), a6      ;4D FB 60 FF
          lea LATE_01(pc,d5.w), a5  ;4B FB 50 01
          lea LATE_FF(pc,d4.w), a4  ;49 FB 40 FF
          lea $01(pc,d3.w), a3      ;47 FB 30 01
          lea $FF(pc,d2.w), a2      ;45 FB 20 FF
          lea LATE_01(pc,d1.w), a1  ;43 FB 10 01
          lea LATE_FF(pc,d0.w), a0  ;41 FB 00 FF

          link a0, #$0001           ;4E 50 00 01
          link a1, #$00FF           ;4E 51 00 FF
          link a2, #$0100           ;4E 52 01 00
          link a3, #$FF00           ;4E 53 FF 00
          link a4, #LATE_0001       ;4E 54 00 01
          link a5, #LATE_00FF       ;4E 55 00 FF
          link a6, #LATE_0100       ;4E 56 01 00
          link a7, #LATE_FF00       ;4E 57 FF 00

          lsl.b #1, d7          ;E3 0F
          lsl.b #2, d6          ;E5 0E
          lsl.b #3, d5          ;E7 0D
          lsl.b #4, d4          ;E9 0C
          lsl.b #5, d3          ;EB 0B
          lsl.b #6, d2          ;ED 0A
          lsl.b #7, d1          ;EF 09
          lsl.b #8, d0          ;E1 08

          lsr.w #1, d7          ;E2 4F
          lsr.w #2, d6          ;E4 4E
          lsr.w #3, d5          ;E6 4D
          lsr.w #4, d4          ;E8 4C
          lsr.w #5, d3          ;EA 4B
          lsr.w #6, d2          ;EC 4A
          lsr.w #7, d1          ;EE 49
          lsr.w #8, d0          ;E0 48

          lsr.w d0, d7          ;E0 6F
          lsr.w d1, d6          ;E2 6E
          lsr.w d2, d5          ;E4 6D
          lsr.w d3, d4          ;E6 6C
          lsr.w d4, d3          ;E8 6B
          lsr.w d5, d2          ;EA 6A
          lsr.w d6, d1          ;EC 69
          lsr.w d7, d0          ;EE 68

          lsl.l #1, d7          ;E3 8F
          lsl.l #2, d6          ;E5 8E
          lsl.l #3, d5          ;E7 8D
          lsl.l #4, d4          ;E9 8C
          lsl.l #5, d3          ;EB 8B
          lsl.l #6, d2          ;ED 8A
          lsl.l #7, d1          ;EF 89
          lsl.l #8, d0          ;E1 88

          asr.l #1, d7          ;E2 87
          asr.l #2, d6          ;E4 86
          asr.l #3, d5          ;E6 85
          asr.l #4, d4          ;E8 84
          asr.l #5, d3          ;EA 83
          asr.l #6, d2          ;EC 82
          asr.l #7, d1          ;EE 81
          asr.l #8, d0          ;E0 80

          asr $01(a7)           ;E0 EF 00 01
          asr $02(a6)           ;E0 EE 00 02
          asr $FE(a5)           ;E0 ED 00 FE
          asr $FF(a4)           ;E0 EC 00 FF
          asr LATE_01(a3)       ;E0 EB 00 01
          asr LATE_02(a2)       ;E0 EA 00 02
          asr LATE_FE(a1)       ;E0 E9 00 FE
          asr LATE_FF(a0)       ;E0 E8 00 FF

          move.b #$01, ($0001).w            ;11 FC 00 01 00 01
          move.b #$02, ($00FF).w            ;11 FC 00 02 00 FF
          move.b #$FE, (LATE_0100).w        ;11 FC 00 FE 01 00
          move.b #$FF, (LATE_FF00).w        ;11 FC 00 FF FF 00
          move.b #LATE_01, ($0001).w        ;11 FC 00 01 00 01
          move.b #LATE_02, ($00FF).w        ;11 FC 00 02 00 FF
          move.b #LATE_FE, (LATE_0100).w    ;11 FC 00 FE 01 00
          move.b #LATE_FF, (LATE_FF00).w    ;11 FC 00 FF FF 00

          move.w #$0001, ($0001).w          ;31 FC 00 01 00 01
          move.w #$00FF, ($00FF).w          ;31 FC 00 FF 00 FF
          move.w #$0100, (LATE_0100).w      ;31 FC 01 00 01 00
          move.w #$FF00, (LATE_FF00).w      ;31 FC FF 00 FF 00
          move.w #LATE_0001, ($0001).w      ;31 FC 00 01 00 01
          move.w #LATE_00FF, ($00FF).w      ;31 FC 00 FF 00 FF
          move.w #LATE_0100, (LATE_0100).w  ;31 FC 01 00 01 00
          move.w #LATE_FF00, (LATE_FF00).w  ;31 FC FF 00 FF 00

          move.l #$00000001, ($FF00).w          ;21 FC 00 00 00 01 FF 00
          move.l #$0000FF00, ($0100).w          ;21 FC 00 00 FF 00 01 00
          move.l #$00010000, (LATE_00FF).w      ;21 FC 00 01 00 01 00 FF
          move.l #$FF000000, (LATE_0001).w      ;21 FC FF 00 00 01 00 01
          move.l #LATE_00000001, ($FF00).w      ;21 FC 00 00 00 01 FF 00
          move.l #LATE_0000FF00, ($0100).w      ;21 FC 00 00 FF 00 01 00
          move.l #LATE_00010000, (LATE_00FF).w  ;21 FC 00 01 00 00 00 FF
          move.l #LATE_FF000000, (LATE_0001).w  ;21 FC FF 00 00 00 00 01

          move.b #$01, d7             ;1E 3C 00 01
          move.b #$02, d6             ;1C 3C 00 02
          move.b #$FE, d5             ;1A 3C 00 FE
          move.b #$FF, d4             ;18 3C 00 FF
          move.b #LATE_01, d3         ;16 3C 00 01
          move.b #LATE_02, d2         ;14 3C 00 02
          move.b #LATE_FE, d1         ;12 3C 00 FE
          move.b #LATE_FF, d0         ;10 3C 00 FF

          move.l #$00000001, d7       ;2E 3C 00 00 00 01
          move.l #$0000FF00, d6       ;2C 3C 00 00 FF 00
          move.l #$00010000, d5       ;2A 3C 00 01 00 00
          move.l #$FF000000, d4       ;28 3C FF 00 00 00
          move.l #LATE_00000001, d3   ;26 3C 00 00 00 01
          move.l #LATE_0000FF00, d2   ;24 3C 00 00 FF 00
          move.l #LATE_00010000, d1   ;22 3C 00 01 00 00
          move.l #LATE_FF000000, d0   ;20 3C FF 00 00 00

          move.w #$0001, (a7)         ;3E BC 00 01
          move.w #$00FF, (a6)         ;3C BC 00 FF
          move.w #$0100, (a5)         ;3A BC 01 00
          move.w #$FF00, (a4)         ;38 BC FF 00
          move.w #LATE_0001, (a3)     ;36 BC 00 01
          move.w #LATE_00FF, (a2)     ;34 BC 00 FF
          move.w #LATE_0100, (a1)     ;32 BC 01 00
          move.w #LATE_FF00, (a0)     ;30 BC FF 00

          move.b #$01, $ff(a7)           ;1F 7C 00 01 00 FF
          move.b #$02, $fe(a6)           ;1D 7C 00 02 00 FE
          move.b #$FE, $02(a5)           ;1B 7C 00 FE 00 02
          move.b #$FF, $01(a4)           ;19 7C 00 FF 00 01
          move.b #LATE_01, LATE_FF(a3)   ;17 7C 00 01 00 FF
          move.b #LATE_02, LATE_FE(a2)   ;15 7C 00 02 00 FE
          move.b #LATE_FE, LATE_02(a1)   ;13 7C 00 FE 00 02
          move.b #LATE_FF, LATE_01(a0)   ;11 7C 00 FF 00 01

          move.w #$0001, $FF(a0)         ;31 7C 00 01 00 FF
          move.w #$00FF, $FE(a1)         ;33 7C 00 FF 00 FE
          move.w #$0100, LATE_02(a2)     ;35 7C 01 00 00 02
          move.w #$FF00, LATE_01(a3)     ;37 7C FF 00 00 01
          move.w #LATE_0001, $FF(a4)     ;39 7C 00 01 00 FF
          move.w #LATE_00FF, $FE(a5)     ;3B 7C 00 FF 00 FE
          move.w #LATE_0100, LATE_02(a6) ;3D 7C 01 00 00 02
          move.w #LATE_FF00, LATE_01(a7) ;3F 7C FF 00 00 01

          move.w #$0001, (a7)+           ;3E FC 00 01
          move.w #$00FF, (a6)+           ;3C FC 00 FF
          move.w #$0100, (a5)+           ;3A FC 01 00
          move.w #$FF00, (a4)+           ;38 FC FF 00
          move.w #LATE_0001, (a3)+       ;36 FC 00 01
          move.w #LATE_00FF, (a2)+       ;34 FC 00 FF
          move.w #LATE_0100, (a1)+       ;32 FC 01 00
          move.w #LATE_FF00, (a0)+       ;30 FC FF 00

          move.w #$0001, -(a7)           ;3F 3C 00 01
          move.w #$00FF, -(a6)           ;3D 3C 00 FF
          move.w #$0100, -(a5)           ;3B 3C 01 00
          move.w #$FF00, -(a4)           ;39 3C FF 00
          move.w #LATE_0001, -(a3)       ;37 3C 00 01
          move.w #LATE_00FF, -(a2)       ;35 3C 00 FF
          move.w #LATE_0100, -(a1)       ;33 3C 01 00
          move.w #LATE_FF00, -(a0)       ;31 3C FF 00

          move.w ($0001).w, ($FF00).w           ;31 F8 00 01 FF 00
          move.w ($00FF).w, ($0100).w           ;31 F8 00 FF 01 00
          move.w ($0100).w, (LATE_00FF).w       ;31 F8 01 00 00 FF
          move.w ($FF00).w, (LATE_0001).w       ;31 F8 FF 00 00 01
          move.w (LATE_0001).w, ($FF00).w       ;31 F8 00 01 FF 00
          move.w (LATE_00FF).w, ($0100).w       ;31 F8 00 FF 01 00
          move.w (LATE_0100).w, (LATE_00FF).w   ;31 F8 01 00 00 FF
          move.w (LATE_FF00).w, (LATE_0001).w   ;31 F8 FF 00 00 01

          move.w ($0001).w, ($FF000000).l           ;33 F8 00 01 FF 00 00 00
          move.w ($00FF).w, ($00010000).l           ;33 F8 00 FF 00 01 00 00
          move.w ($0100).w, (LATE_0000FF00).l       ;33 F8 01 00 00 00 FF 00
          move.w ($FF00).w, (LATE_00000001).l       ;33 F8 FF 00 00 00 00 01
          move.w (LATE_0001).w, ($FF000000).l       ;33 F8 00 01 FF 00 00 00
          move.w (LATE_00FF).w, ($00010000).l       ;33 F8 00 FF 00 01 00 00
          move.w (LATE_0100).w, (LATE_0000FF00).l   ;33 F8 01 00 00 00 FF 00
          move.w (LATE_FF00).w, (LATE_00000001).l   ;33 F8 FF 00 00 00 00 01

          move.w ($00000001).l, ($FF00).w           ;31 F9 00 00 00 01 FF 00
          move.w ($0000FF00).l, ($0100).w           ;31 F9 00 00 FF 00 01 00
          move.w ($00010000).l, (LATE_00FF).w       ;31 F9 00 01 00 00 00 FF
          move.w ($FF000000).l, (LATE_0001).w       ;31 F9 FF 00 00 00 00 01
          move.w (LATE_00000001).l, ($FF00).w       ;31 F9 00 00 00 01 FF 00
          move.w (LATE_0000FF00).l, ($0100).w       ;31 F9 00 00 FF 00 01 00
          move.w (LATE_00010000).l, (LATE_00FF).w   ;31 F9 00 01 00 00 00 FF
          move.w (LATE_FF000000).l, (LATE_0001).w   ;31 F9 FF 00 00 00 00 01

          move.w ($0001).w, d0        ;30 38 00 01
          move.w ($00FF).w, d1        ;32 38 00 FF
          move.w ($0100).w, d2        ;34 38 01 00
          move.w ($FF00).w, d3        ;36 38 FF 00
          move.w (LATE_0001).w, d4    ;38 38 00 01
          move.w (LATE_00FF).w, d5    ;3A 38 00 FF
          move.w (LATE_0100).w, d6    ;3C 38 01 00
          move.w (LATE_FF00).w, d7    ;3E 38 FF 00

          move.b ($0001).w, (a0)      ;10 B8 00 01
          move.b ($00FF).w, (a1)      ;12 B8 00 FF
          move.b ($0100).w, (a2)      ;14 B8 01 00
          move.b ($FF00).w, (a3)      ;16 B8 FF 00
          move.b (LATE_0001).w, (a4)  ;18 B8 00 01
          move.b (LATE_00FF).w, (a5)  ;1A B8 00 FF
          move.b (LATE_0100).w, (a6)  ;1C B8 01 00
          move.b (LATE_FF00).w, (a7)  ;1E B8 FF 00

          move.b ($0001).w, $FF(a0)         ;11 78 00 01 00 FF
          move.b ($00FF).w, $01(a1)         ;13 78 00 FF 00 01
          move.b ($0100).w, LATE_FF(a2)     ;15 78 01 00 00 FF
          move.b ($FF00).w, LATE_01(a3)     ;17 78 FF 00 00 01
          move.b (LATE_0001).w, $FF(a4)     ;19 78 00 01 00 FF
          move.b (LATE_00FF).w, $01(a5)     ;1B 78 00 FF 00 01
          move.b (LATE_0100).w, LATE_FF(a6) ;1D 78 01 00 00 FF
          move.b (LATE_FF00).w, LATE_01(a7) ;1F 78 FF 00 00 01

          move.b ($0001).w, (a0)+     ;10 F8 00 01
          move.b ($00FF).w, (a1)+     ;12 F8 00 FF
          move.b ($0100).w, (a2)+     ;14 F8 01 00
          move.b ($FF00).w, (a3)+     ;16 F8 FF 00
          move.b (LATE_0001).w, (a4)+ ;18 F8 00 01
          move.b (LATE_00FF).w, (a5)+ ;1A F8 00 FF
          move.b (LATE_0100).w, (a6)+ ;1C F8 01 00
          move.b (LATE_FF00).w, (a7)+ ;1E F8 FF 00

          move.b ($0001).w, -(a0)     ;11 38 00 01
          move.b ($00FF).w, -(a1)     ;13 38 00 FF
          move.b ($0100).w, -(a2)     ;15 38 01 00
          move.b ($FF00).w, -(a3)     ;17 38 FF 00
          move.b (LATE_0001).w, -(a4) ;19 38 00 01
          move.b (LATE_00FF).w, -(a5) ;1B 38 00 FF
          move.b (LATE_0100).w, -(a6) ;1D 38 01 00
          move.b (LATE_FF00).w, -(a7) ;1F 38 FF 00

          move.b d0, ($0001).w        ;11 C0 00 01
          move.b d1, ($00FF).w        ;11 C1 00 FF
          move.b d2, ($0100).w        ;11 C2 01 00
          move.b d3, ($FF00).w        ;11 C3 FF 00
          move.b d4, (LATE_0001).w    ;11 C4 00 01
          move.b d5, (LATE_00FF).w    ;11 C5 00 FF
          move.b d6, (LATE_0100).w    ;11 C6 01 00
          move.b d7, (LATE_FF00).w    ;11 C7 FF 00

          move.l d0, ($00000001).l        ;23 C0 00 00 00 01
          move.l d1, ($0000FF00).l        ;23 C1 00 00 FF 00
          move.l d2, ($00010000).l        ;23 C2 00 01 00 00
          move.l d3, ($FF000000).l        ;23 C3 FF 00 00 00
          move.l d4, (LATE_00000001).l    ;23 C4 00 00 00 01
          move.l d5, (LATE_0000FF00).l    ;23 C5 00 00 FF 00
          move.l d6, (LATE_00010000).l    ;23 C6 00 01 00 00
          move.l d7, (LATE_FF000000).l    ;23 C7 FF 00 00 00

          move.w d0, d7                ;3E 00
          move.w d1, d6                ;3C 01
          move.w d2, d5                ;3A 02
          move.w d3, d4                ;38 03
          move.w d4, d3                ;36 04
          move.w d5, d2                ;34 05
          move.w d6, d1                ;32 06
          move.w d7, d0                ;30 07

          move.l d0, d7                ;2E 00
          move.l d1, d6                ;2C 01
          move.l d2, d5                ;2A 02
          move.l d3, d4                ;28 03
          move.l d4, d3                ;26 04
          move.l d5, d2                ;24 05
          move.l d6, d1                ;22 06
          move.l d7, d0                ;20 07

          move.w d0, (a7)              ;3E 80
          move.w d1, (a6)              ;3C 81
          move.w d2, (a5)              ;3A 82
          move.w d3, (a4)              ;38 83
          move.w d4, (a3)              ;36 84
          move.w d5, (a2)              ;34 85
          move.w d6, (a1)              ;32 86
          move.w d7, (a0)              ;30 87

          move.w d0, $01(a7)           ;1F 40 00 01
          move.w d1, $FF(a6)           ;1D 41 00 FF
          move.w d2, LATE_01(a5)       ;1B 42 00 01
          move.w d3, LATE_FF(a4)       ;19 43 00 FF
          move.w d4, $01(a3)           ;17 44 00 01
          move.w d5, $FF(a2)           ;15 45 00 FF
          move.w d6, LATE_01(a1)       ;13 46 00 01
          move.w d7, LATE_FF(a0)       ;11 47 00 FF

          move.w d0, (a7)+             ;3E C0
          move.w d1, (a6)+             ;3C C1
          move.w d2, (a5)+             ;3A C2
          move.w d3, (a4)+             ;38 C3
          move.w d4, (a3)+             ;36 C4
          move.w d5, (a2)+             ;34 C5
          move.w d6, (a1)+             ;32 C6
          move.w d7, (a0)+             ;30 C7

          move.w d0, -(a7)             ;3F 00
          move.w d1, -(a6)             ;3D 01
          move.w d2, -(a5)             ;3B 02
          move.w d3, -(a4)             ;39 03
          move.w d4, -(a3)             ;37 04
          move.w d5, -(a2)             ;35 05
          move.w d6, -(a1)             ;33 06
          move.w d7, -(a0)             ;31 07

          move.b (a0), d7              ;1E 10
          move.b (a1), d6              ;1C 11
          move.b (a2), d5              ;1A 12
          move.b (a3), d4              ;18 13
          move.b (a4), d3              ;16 14
          move.b (a5), d2              ;14 15
          move.b (a6), d1              ;12 16
          move.b (a7), d0              ;10 17

          move.b (a0), (a7)            ;1E 90
          move.b (a1), (a6)            ;1C 91
          move.b (a2), (a5)            ;1A 92
          move.b (a3), (a4)            ;18 93
          move.b (a4), (a3)            ;16 94
          move.b (a5), (a2)            ;14 95
          move.b (a6), (a1)            ;12 96
          move.b (a7), (a0)            ;10 97

          move.b (a0), $01(a7)         ;1F 50 00 01
          move.b (a1), $FF(a6)         ;1D 51 00 FF
          move.b (a2), LATE_01(a5)     ;1B 52 00 01
          move.b (a3), LATE_FF(a4)     ;19 53 00 FF
          move.b (a4), $01(a3)         ;17 54 00 01
          move.b (a5), $FF(a2)         ;15 55 00 FF
          move.b (a6), LATE_01(a1)     ;13 56 00 01
          move.b (a7), LATE_FF(a0)     ;11 57 00 FF

          move.l (a0), $01(a7)         ;2F 50 00 01
          move.l (a1), $FF(a6)         ;2D 51 00 FF
          move.l (a2), LATE_01(a5)     ;2B 52 00 01
          move.l (a3), LATE_FF(a4)     ;29 53 00 FF
          move.l (a4), $01(a3)         ;27 54 00 01
          move.l (a5), $FF(a2)         ;25 55 00 FF
          move.l (a6), LATE_01(a1)     ;23 56 00 01
          move.l (a7), LATE_FF(a0)     ;21 57 00 FF

          move.b (a0), (a7)+           ;1E D0
          move.b (a1), (a6)+           ;1C D1
          move.b (a2), (a5)+           ;1A D2
          move.b (a3), (a4)+           ;18 D3
          move.b (a4), (a3)+           ;16 D4
          move.b (a5), (a2)+           ;14 D5
          move.b (a6), (a1)+           ;12 D6
          move.b (a7), (a0)+           ;10 D7

          move.b (a0), -(a7)           ;1F 10
          move.b (a1), -(a6)           ;1D 11
          move.b (a2), -(a5)           ;1B 12
          move.b (a3), -(a4)           ;19 13
          move.b (a4), -(a3)           ;17 14
          move.b (a5), -(a2)           ;15 15
          move.b (a6), -(a1)           ;13 16
          move.b (a7), -(a0)           ;11 17

          move.w $01(a7), ($FF00).w           ;31 EF 00 01 FF 00
          move.w $02(a6), ($0100).w           ;31 EE 00 02 01 00
          move.w $FE(a5), ($00FF).w           ;31 ED 00 FE 00 FF
          move.w $FF(a4), ($0001).w           ;31 EC 00 FF 00 01
          move.w LATE_01(a3), (LATE_FF00).w   ;31 EB 00 01 FF 00
          move.w LATE_02(a2), (LATE_0100).w   ;31 EA 00 02 01 00
          move.w LATE_FE(a1), (LATE_00FF).w   ;31 E9 00 FE 00 FF
          move.w LATE_FF(a0), (LATE_0001).w   ;31 E8 00 FF 00 01

          move.b $01(a0),d7        ;1E 28 00 01
          move.b $02(a1),d6        ;1C 29 00 02
          move.b $FE(a2),d5        ;1A 2A 00 FE
          move.b $FF(a3),d4        ;18 2B 00 FF
          move.b LATE_01(a4),d3    ;16 2C 00 01
          move.b LATE_02(a5),d2    ;14 2D 00 02
          move.b LATE_FE(a6),d1    ;12 2E 00 FE
          move.b LATE_FF(a7),d0    ;10 2F 00 FF

          move.l $01(a0),d7        ;2E 28 00 01
          move.l $02(a1),d6        ;2C 29 00 02
          move.l $FE(a2),d5        ;2A 2A 00 FE
          move.l $FF(a3),d4        ;28 2B 00 FF
          move.l LATE_01(a4),d3    ;26 2C 00 01
          move.l LATE_02(a5),d2    ;24 2D 00 02
          move.l LATE_FE(a6),d1    ;22 2E 00 FE
          move.l LATE_FF(a7),d0    ;20 2F 00 FF

          move.b $01(a0), (a7)        ;1E A8 00 01
          move.b $02(a1), (a6)        ;1C A9 00 02
          move.b $FE(a2), (a5)        ;1A AA 00 FE
          move.b $FF(a3), (a4)        ;18 AB 00 FF
          move.b LATE_01(a4), (a3)    ;16 AC 00 01
          move.b LATE_02(a5), (a2)    ;14 AD 00 02
          move.b LATE_FE(a6), (a1)    ;12 AE 00 FE
          move.b LATE_FF(a7), (a0)    ;10 AF 00 FF

          move.b $01(a0), $FF(a7)           ;1F 68 00 01 00 FF
          move.b $02(a1), $FE(a6)           ;1D 69 00 02 00 FE
          move.b $FE(a2), LATE_02(a5)       ;1B 6A 00 FE 00 02
          move.b $FF(a3), LATE_01(a4)       ;19 6B 00 FF 00 01
          move.b LATE_01(a4), $FF(a3)       ;17 6C 00 01 00 FF
          move.b LATE_02(a5), $FE(a2)       ;15 6D 00 02 00 FE
          move.b LATE_FE(a6), LATE_02(a1)   ;13 6E 00 FE 00 02
          move.b LATE_FF(a7), LATE_01(a0)   ;11 6F 00 FF 00 01

          move.w $01(a0), $FF(a7)           ;3F 68 00 01 00 FF
          move.w $02(a1), $FE(a6)           ;3D 69 00 02 00 FE
          move.w $FE(a2), LATE_02(a5)       ;3B 6A 00 FE 00 02
          move.w $FF(a3), LATE_01(a4)       ;39 6B 00 FF 00 01
          move.w LATE_01(a4), $FF(a3)       ;37 6C 00 01 00 FF
          move.w LATE_02(a5), $FE(a2)       ;35 6D 00 02 00 FE
          move.w LATE_FE(a6), LATE_02(a1)   ;33 6E 00 FE 00 02
          move.w LATE_FF(a7), LATE_01(a0)   ;31 6F 00 FF 00 01

          move.l $01(a0), $FF(a7)           ;2F 68 00 01 00 FF
          move.l $02(a1), $FE(a6)           ;2D 69 00 02 00 FE
          move.l $FE(a2), LATE_02(a5)       ;2B 6A 00 FE 00 02
          move.l $FF(a3), LATE_01(a4)       ;29 6B 00 FF 00 01
          move.l LATE_01(a4), $FF(a3)       ;27 6C 00 01 00 FF
          move.l LATE_02(a5), $FE(a2)       ;25 6D 00 02 00 FE
          move.l LATE_FE(a6), LATE_02(a1)   ;23 6E 00 FE 00 02
          move.l LATE_FF(a7), LATE_01(a0)   ;21 6F 00 FF 00 01

          move.b $01(a0), (a7)+         ;1E E8 00 01
          move.b $02(a1), (a6)+         ;1C E9 00 02
          move.b $FE(a2), (a5)+         ;1A EA 00 FE
          move.b $FF(a3), (a4)+         ;18 EB 00 FF
          move.b LATE_01(a4), (a3)+     ;16 EC 00 01
          move.b LATE_02(a5), (a2)+     ;14 ED 00 02
          move.b LATE_FE(a6), (a1)+     ;12 EE 00 FE
          move.b LATE_FF(a7), (a0)+     ;10 EF 00 FF

          move.b $01(a0), -(a7)         ;1F 28 00 01
          move.b $02(a1), -(a6)         ;1D 29 00 02
          move.b $FE(a2), -(a5)         ;1B 2A 00 FE
          move.b $FF(a3), -(a4)         ;19 2B 00 FF
          move.b LATE_01(a4), -(a3)     ;17 2C 00 01
          move.b LATE_02(a5), -(a2)     ;15 2D 00 02
          move.b LATE_FE(a6), -(a1)     ;13 2E 00 FE
          move.b LATE_FF(a7), -(a0)     ;11 2F 00 FF

          move.b (a0)+, ($0001).w       ;11 D8 00 01
          move.b (a1)+, ($00FF).w       ;11 D9 00 FF
          move.b (a2)+, ($0100).w       ;11 DA 01 00
          move.b (a3)+, ($FF00).w       ;11 DB FF 00
          move.b (a4)+, (LATE_0001).w   ;11 DC 00 01
          move.b (a5)+, (LATE_00FF).w   ;11 DD 00 FF
          move.b (a6)+, (LATE_0100).w   ;11 DE 01 00
          move.b (a7)+, (LATE_FF00).w   ;11 DF FF 00

          move.b (a7)+, d0          ;10 1F
          move.b (a6)+, d1          ;12 1E
          move.b (a5)+, d2          ;14 1D
          move.b (a4)+, d3          ;16 1C
          move.b (a3)+, d4          ;18 1B
          move.b (a2)+, d5          ;1A 1A
          move.b (a1)+, d6          ;1C 19
          move.b (a0)+, d7          ;1E 18

          move.l (a7)+, d0          ;20 1F
          move.l (a6)+, d1          ;22 1E
          move.l (a5)+, d2          ;24 1D
          move.l (a4)+, d3          ;26 1C
          move.l (a3)+, d4          ;28 1B
          move.l (a2)+, d5          ;2A 1A
          move.l (a1)+, d6          ;2C 19
          move.l (a0)+, d7          ;2E 18

          move.l (a0)+, (a7)        ;2E 98
          move.l (a1)+, (a6)        ;2C 99
          move.l (a2)+, (a5)        ;2A 9A
          move.l (a3)+, (a4)        ;28 9B
          move.l (a4)+, (a3)        ;26 9C
          move.l (a5)+, (a2)        ;24 9D
          move.l (a6)+, (a1)        ;22 9E
          move.l (a7)+, (a0)        ;20 9F

          move.l (a0)+, $01(a7)         ;2F 58 00 01
          move.l (a1)+, $FF(a6)         ;2D 59 00 FF
          move.l (a2)+, LATE_01(a5)     ;2B 5A 00 01
          move.l (a3)+, LATE_FF(a4)     ;29 5B 00 FF
          move.l (a4)+, $01(a3)         ;27 5C 00 01
          move.l (a5)+, $FF(a2)         ;25 5D 00 FF
          move.l (a6)+, LATE_01(a1)     ;23 5E 00 01
          move.l (a7)+, LATE_FF(a0)     ;21 5F 00 FF

          move.l (a0)+, (a7)+       ;2E D8
          move.l (a1)+, (a6)+       ;2C D9
          move.l (a2)+, (a5)+       ;2A DA
          move.l (a3)+, (a4)+       ;28 DB
          move.l (a4)+, (a3)+       ;26 DC
          move.l (a5)+, (a2)+       ;24 DD
          move.l (a6)+, (a1)+       ;22 DE
          move.l (a7)+, (a0)+       ;20 DF

          move.b -(a7), d0          ;10 27
          move.b -(a6), d1          ;12 26
          move.b -(a5), d2          ;14 25
          move.b -(a4), d3          ;16 24
          move.b -(a3), d4          ;18 23
          move.b -(a2), d5          ;1A 22
          move.b -(a1), d6          ;1C 21
          move.b -(a0), d7          ;1E 20

          move.w -(a7), d0          ;30 27
          move.w -(a6), d1          ;32 26
          move.w -(a5), d2          ;34 25
          move.w -(a4), d3          ;36 24
          move.w -(a3), d4          ;38 23
          move.w -(a2), d5          ;3A 22
          move.w -(a1), d6          ;3C 21
          move.w -(a0), d7          ;3E 20

          move.w -(a7), -(a0)       ;31 27
          move.w -(a6), -(a1)       ;33 26
          move.w -(a5), -(a2)       ;35 25
          move.w -(a4), -(a3)       ;37 24
          move.w -(a3), -(a4)       ;39 23
          move.w -(a2), -(a5)       ;3B 22
          move.w -(a1), -(a6)       ;3D 21
          move.w -(a0), -(a7)       ;3F 20

          move.l a0, ($0001).w      ;21 C8 00 01
          move.l a1, ($00FF).w      ;21 C9 00 FF
          move.l a2, ($0100).w      ;21 CA 01 00
          move.l a3, ($FF00).w      ;21 CB FF 00
          move.l a4, (LATE_0001).w  ;21 CC 00 01
          move.l a5, (LATE_00FF).w  ;21 CD 00 FF
          move.l a6, (LATE_0100).w  ;21 CE 01 00
          move.l a7, (LATE_FF00).w  ;21 CF FF 00

          move.l a7, d0             ;20 0F
          move.l a6, d1             ;22 0E
          move.l a5, d2             ;24 0D
          move.l a4, d3             ;26 0C
          move.l a3, d4             ;28 0B
          move.l a2, d5             ;2A 0A
          move.l a1, d6             ;2C 09
          move.l a0, d7             ;2E 08

          move.l a7, (a0)           ;20 8F
          move.l a6, (a1)           ;22 8E
          move.l a5, (a2)           ;24 8D
          move.l a4, (a3)           ;26 8C
          move.l a3, (a4)           ;28 8B
          move.l a2, (a5)           ;2A 8A
          move.l a1, (a6)           ;2C 89
          move.l a0, (a7)           ;2E 88

          move.w a0, $01(a7)        ;3F 48 00 01
          move.w a1, $FF(a6)        ;3D 49 00 FF
          move.w a2, LATE_01(a5)    ;3B 4A 00 01
          move.w a3, LATE_FF(a4)    ;39 4B 00 FF
          move.w a4, $01(a3)        ;37 4C 00 01
          move.w a5, $FF(a2)        ;35 4D 00 FF
          move.w a6, LATE_01(a1)    ;33 4E 00 01
          move.w a7, LATE_FF(a0)    ;31 4F 00 FF

          move.l a2, (sp)           ;2E 8A
          move.l a2, (sp)+          ;2E CA
          move.l a2, -(sp)          ;2F 0A

          move.b $01(a0,d7.w), ($FF00).w    ;11 F0 70 01 FF 00
          move.b $02(a1,d6.w), ($0100).w    ;11 F1 60 02 01 00
          move.b $FE(a2,d5.w), ($00FF).w    ;11 F2 50 FE 00 FF
          move.b $FF(a3,d4.w), ($0001).w    ;11 F3 40 FF 00 01
          move.b $01(a4,d3.w), ($FF00).w    ;11 F4 30 01 FF 00
          move.b $02(a5,d2.w), ($0100).w    ;11 F5 20 02 01 00
          move.b $FE(a6,d1.w), ($00FF).w    ;11 F6 10 FE 00 FF
          move.b $FF(a7,d0.w), ($0001).w    ;11 F7 00 FF 00 01

          move.b (a0,d7.w), d0   ;10 30 70 00
          move.b (a1,d6.w), d1   ;12 31 60 00
          move.b (a2,d5.w), d2   ;14 32 50 00
          move.b (a3,d4.w), d3   ;16 33 40 00
          move.b (a4,d3.w), d4   ;18 34 30 00
          move.b (a5,d2.w), d5   ;1A 35 20 00
          move.b (a6,d1.w), d6   ;1C 36 10 00
          move.b (a7,d0.w), d7   ;1E 37 00 00

          move.w $01(a0,d7.w), d0       ;30 30 70 01
          move.w $02(a1,d6.w), d1       ;32 31 60 02
          move.w $FE(a2,d5.w), d2       ;34 32 50 FE
          move.w $FF(a3,d4.w), d3       ;36 33 40 FF
          move.w LATE_01(a4,d3.w), d4   ;38 34 30 01
          move.w LATE_02(a5,d2.w), d5   ;3A 35 20 02
          move.w LATE_FE(a6,d1.w), d6   ;3C 36 10 FE
          move.w LATE_FF(a7,d0.w), d7   ;3E 37 00 FF

          move.b $01(a0,d7.w), d0       ;10 30 70 01
          move.b $02(a1,d6.w), d1       ;12 31 60 02
          move.b $FE(a2,d5.w), d2       ;14 32 50 FE
          move.b $FF(a3,d4.w), d3       ;16 33 40 FF
          move.b LATE_01(a4,d3.w), d4   ;18 34 30 01
          move.b LATE_02(a5,d2.w), d5   ;1A 35 20 02
          move.b LATE_FE(a6,d1.w), d6   ;1C 36 10 FE
          move.b LATE_FF(a7,d0.w), d7   ;1E 37 00 FF

          move.b (a0,d7.w), (a0)        ;10 B0 70 00
          move.b (a1,d6.w), (a1)        ;12 B1 60 00
          move.b (a2,d5.w), (a2)        ;14 B2 50 00
          move.b (a3,d4.w), (a3)        ;16 B3 40 00
          move.b (a4,d3.w), (a4)        ;18 B4 30 00
          move.b (a5,d2.w), (a5)        ;1A B5 20 00
          move.b (a6,d1.w), (a6)        ;1C B6 10 00
          move.b (a7,d0.w), (a7)        ;1E B7 00 00

          move.b $01(a0,d7.w), $FF(a0)            ;11 70 70 01 00 FF
          move.b $02(a1,d6.w), $FE(a1)            ;13 71 60 02 00 FE
          move.b $FE(a2,d5.w), LATE_02(a2)        ;15 72 50 FE 00 02
          move.b $FF(a3,d4.w), LATE_01(a3)        ;17 73 40 FF 00 01
          move.b LATE_01(a4,d3.w), $FF(a4)        ;19 74 30 01 00 FF
          move.b LATE_02(a5,d2.w), $FE(a5)        ;1B 75 20 02 00 FE
          move.b LATE_FE(a6,d1.w), LATE_02(a6)    ;1D 76 10 FE 00 02
          move.b LATE_FF(a7,d0.w), LATE_01(a7)    ;1F 77 00 FF 00 01

          move.b (a0,d7.w), (a0)+       ;10 F0 70 00
          move.b (a1,d6.w), (a1)+       ;12 F1 60 00
          move.b (a2,d5.w), (a2)+       ;14 F2 50 00
          move.b (a3,d4.w), (a3)+       ;16 F3 40 00
          move.b (a4,d3.w), (a4)+       ;18 F4 30 00
          move.b (a5,d2.w), (a5)+       ;1A F5 20 00
          move.b (a6,d1.w), (a6)+       ;1C F6 10 00
          move.b (a7,d0.w), (a7)+       ;1E F7 00 00

          move.b (a0,d7.w), -(a0)       ;11 30 70 00
          move.b (a1,d6.w), -(a1)       ;13 31 60 00
          move.b (a2,d5.w), -(a2)       ;15 32 50 00
          move.b (a3,d4.w), -(a3)       ;17 33 40 00
          move.b (a4,d3.w), -(a4)       ;19 34 30 00
          move.b (a5,d2.w), -(a5)       ;1B 35 20 00
          move.b (a6,d1.w), -(a6)       ;1D 36 10 00
          move.b (a7,d0.w), -(a7)       ;1F 37 00 00

          move.b $01(pc,d7.w), ($FF00).w    ;11 FB 70 01 FF 00
          move.b $02(pc,d6.w), ($0100).w    ;11 FB 60 02 01 00
          move.b $FE(pc,d5.w), ($00FF).w    ;11 FB 50 FE 00 FF
          move.b $FF(pc,d4.w), ($0001).w    ;11 FB 40 FF 00 01
          move.b $01(pc,d3.w), ($FF00).w    ;11 FB 30 01 FF 00
          move.b $02(pc,d2.w), ($0100).w    ;11 FB 20 02 01 00
          move.b $FE(pc,d1.w), ($00FF).w    ;11 FB 10 FE 00 FF
          move.b $FF(pc,d0.w), ($0001).w    ;11 FB 00 FF 00 01

          move.b $01(pc,d7.w), d0       ;10 3B 70 01
          move.b $02(pc,d6.w), d1       ;12 3B 60 02
          move.b $FE(pc,d5.w), d2       ;14 3B 50 FE
          move.b $FF(pc,d4.w), d3       ;16 3B 40 FF
          move.b LATE_01(pc,d3.w), d4   ;18 3B 30 01
          move.b LATE_02(pc,d2.w), d5   ;1A 3B 20 02
          move.b LATE_FE(pc,d1.w), d6   ;1C 3B 10 FE
          move.b LATE_FF(pc,d0.w), d7   ;1E 3B 00 FF

          move.b $01(pc,d7.w), (a0)     ;10 BB 70 01
          move.b $02(pc,d6.w), (a1)     ;12 BB 60 02
          move.b $FE(pc,d5.w), (a2)     ;14 BB 50 FE
          move.b $FF(pc,d4.w), (a3)     ;16 BB 40 FF
          move.b LATE_01(pc,d3.w), (a4) ;18 BB 30 01
          move.b LATE_02(pc,d2.w), (a5) ;1A BB 20 02
          move.b LATE_FE(pc,d1.w), (a6) ;1C BB 10 FE
          move.b LATE_FF(pc,d0.w), (a7) ;1E BB 00 FF

          move.b $01(pc,d7.w), $FF(a0)          ;11 7B 70 01 00 FF
          move.b $02(pc,d6.w), $FE(a1)          ;13 7B 60 02 00 FE
          move.b $FE(pc,d5.w), LATE_02(a2)      ;15 7B 50 FE 00 02
          move.b $FF(pc,d4.w), LATE_01(a3)      ;17 7B 40 FF 00 01
          move.b LATE_01(pc,d3.w), $FF(a4)      ;19 7B 30 01 00 FF
          move.b LATE_02(pc,d2.w), $FE(a5)      ;1B 7B 20 02 00 FE
          move.b LATE_FE(pc,d1.w), LATE_02(a6)  ;1D 7B 10 FE 00 02
          move.b LATE_FF(pc,d0.w), LATE_01(a7)  ;1F 7B 00 FF 00 01

          move #$0001,sr                ;46 FC 00 01
          move #$FF00,sr                ;46 FC FF 00
          move #LATE_0001,sr            ;46 FC 00 01
          move #LATE_FF00,sr            ;46 FC FF 00

          movea.w #$0001,a0             ;30 7C 00 01
          movea.w #$00FF,a1             ;32 7C 00 FF
          movea.w #$0100,a2             ;34 7C 01 00
          movea.w #$FF00,a3             ;36 7C FF 00
          movea.w #LATE_0001,a4         ;38 7C 00 01
          movea.w #LATE_00FF,a5         ;3A 7C 00 FF
          movea.w #LATE_0100,a6         ;3C 7C 01 00
          movea.w #LATE_FF00,a7         ;3E 7C FF 00

          movea.w ($0001).w,a7          ;3E 78 00 01
          movea.w ($00FF).w,a6          ;3C 78 00 FF
          movea.w ($0100).w,a5          ;3A 78 01 00
          movea.w ($FF00).w,a4          ;38 78 FF 00
          movea.w (LATE_0001).w,a3      ;36 78 00 01
          movea.w (LATE_00FF).w,a2      ;34 78 00 FF
          movea.w (LATE_0100).w,a1      ;32 78 01 00
          movea.w (LATE_FF00).w,a0      ;30 78 FF 00

          movea.w d7, a0                ;30 47
          movea.w d6, a1                ;32 46
          movea.w d5, a2                ;34 45
          movea.w d4, a3                ;36 44
          movea.w d3, a4                ;38 43
          movea.w d2, a5                ;3A 42
          movea.w d1, a6                ;3C 41
          movea.w d0, a7                ;3E 40
          movea.w d0, sp                ;3E 40

          movea.l a7, a0                ;20 4F
          movea.l a6, a1                ;22 4E
          movea.l a5, a2                ;24 4D
          movea.l a4, a3                ;26 4C
          movea.l a3, a4                ;28 4B
          movea.l a2, a5                ;2A 4A
          movea.l a1, a6                ;2C 49
          movea.l a0, a7                ;2E 48

          movea.w (a7), a0              ;30 57
          movea.w (a6), a1              ;32 56
          movea.w (a5), a2              ;34 55
          movea.w (a4), a3              ;36 54
          movea.w (a3), a4              ;38 53
          movea.w (a2), a5              ;3A 52
          movea.w (a1), a6              ;3C 51
          movea.w (a0), a7              ;3E 50

          movea.w $01(a7), a0           ;30 6F 00 01
          movea.w $02(a6), a1           ;32 6E 00 02
          movea.w $FE(a5), a2           ;34 6D 00 FE
          movea.w $FF(a4), a3           ;36 6C 00 FF
          movea.w LATE_01(a3), a4       ;38 6B 00 01
          movea.w LATE_02(a2), a5       ;3A 6A 00 02
          movea.w LATE_FE(a1), a6       ;3C 69 00 FE
          movea.w LATE_FF(a0), a7       ;3E 68 00 FF

          movea.l (a7)+, a0             ;20 5F
          movea.l (a6)+, a1             ;22 5E
          movea.l (a5)+, a2             ;24 5D
          movea.l (a4)+, a3             ;26 5C
          movea.l (a3)+, a4             ;28 5B
          movea.l (a2)+, a5             ;2A 5A
          movea.l (a1)+, a6             ;2C 59
          movea.l (a0)+, a7             ;2E 58

          movea.l -(a7), a0             ;20 67
          movea.l -(a6), a1             ;22 66
          movea.l -(a5), a2             ;24 65
          movea.l -(a4), a3             ;26 64
          movea.l -(a3), a4             ;28 63
          movea.l -(a2), a5             ;2A 62
          movea.l -(a1), a6             ;2C 61
          movea.l -(a0), a7             ;2E 60

          movea.l (sp), a0              ;20 57
          movea.l (sp), a1              ;22 57
          movea.l (sp), a2              ;24 57
          movea.l (sp), a3              ;26 57
          movea.l (sp), a4              ;28 57
          movea.l (sp), a5              ;2A 57
          movea.l (sp), a6              ;2C 57
          movea.l (sp), a7              ;2E 57

          movea.l (sp)+, a0             ;20 5F
          movea.l (sp)+, a1             ;22 5E
          movea.l (sp)+, a2             ;24 5D
          movea.l (sp)+, a3             ;26 5C
          movea.l (sp)+, a4             ;28 5B
          movea.l (sp)+, a5             ;2A 5A
          movea.l (sp)+, a6             ;2C 59
          movea.l (sp)+, a7             ;2E 58

          movea.l -(sp), a0             ;20 67
          movea.l -(sp), a1             ;22 67
          movea.l -(sp), a2             ;24 67
          movea.l -(sp), a3             ;26 67
          movea.l -(sp), a4             ;28 67
          movea.l -(sp), a5             ;2A 67
          movea.l -(sp), a6             ;2C 67
          movea.l -(sp), a7             ;2E 67

          movea.l (a0,d7.w), a7          ;2E 70 70 00
          movea.l (a1,d6.w), a6          ;2C 71 60 00
          movea.l (a2,d5.w), a5          ;2A 72 50 00
          movea.l (a3,d4.w), a4          ;28 73 40 00
          movea.l (a4,d3.w), a3          ;26 74 30 00
          movea.l (a5,d2.w), a2          ;24 75 20 00
          movea.l (a6,d1.w), a1          ;22 76 10 00
          movea.l (a7,d0.w), a0          ;20 77 00 00

          movea.l $01(a0,d7.w), a7       ;2E 70 70 01
          movea.l $FF(a1,d6.w), a6       ;2C 71 60 FF
          movea.l LATE_01(a2,d5.w), a5   ;2A 72 50 01
          movea.l LATE_FF(a3,d4.w), a4   ;28 73 40 FF
          movea.l $01(a4,d3.w), a3       ;26 74 30 01
          movea.l $FF(a5,d2.w), a2       ;24 75 20 FF
          movea.l LATE_01(a6,d1.w), a1   ;22 76 10 01
          movea.l LATE_FF(a7,d0.w), a0   ;20 77 00 FF

          movep.w $0001(a0),d7           ;0F 08 00 01
          movep.w $00FF(a1),d6           ;0D 09 00 FF
          movep.w $0100(a2),d5           ;0B 0A 01 00
          movep.w $FF00(a3),d4           ;09 0B FF 00
          movep.w LATE_0001(a4),d3       ;07 0C 00 01
          movep.w LATE_00FF(a5),d2       ;05 0D 00 FF
          movep.w LATE_0100(a6),d1       ;03 0E 01 00
          movep.w LATE_FF00(a7),d0       ;01 0F FF 00

          movep.l $0001(a0),d7           ;0F 48 00 01
          movep.l $00FF(a1),d6           ;0D 49 00 FF
          movep.l $0100(a2),d5           ;0B 4A 01 00
          movep.l $FF00(a3),d4           ;09 4B FF 00
          movep.l LATE_0001(a4),d3       ;07 4C 00 01
          movep.l LATE_00FF(a5),d2       ;05 4D 00 FF
          movep.l LATE_0100(a6),d1       ;03 4E 01 00
          movep.l LATE_FF00(a7),d0       ;01 4F FF 00

          movep.w d7,$0001(a0)           ;0F 88 00 01
          movep.w d6,$00FF(a1)           ;0D 89 00 FF
          movep.w d5,$0100(a2)           ;0B 8A 01 00
          movep.w d4,$FF00(a3)           ;09 8B FF 00
          movep.w d3,LATE_0001(a4)       ;07 8C 00 01
          movep.w d2,LATE_00FF(a5)       ;05 8D 00 FF
          movep.w d1,LATE_0100(a6)       ;03 8E 01 00
          movep.w d0,LATE_FF00(a7)       ;01 8F FF 00

          movep.l d7,$0001(a0)           ;0F C8 00 01
          movep.l d6,$00FF(a1)           ;0D C9 00 FF
          movep.l d5,$0100(a2)           ;0B CA 01 00
          movep.l d4,$FF00(a3)           ;09 CB FF 00
          movep.l d3,LATE_0001(a4)       ;07 CC 00 01
          movep.l d2,LATE_00FF(a5)       ;05 CD 00 FF
          movep.l d1,LATE_0100(a6)       ;03 CE 01 00
          movep.l d0,LATE_FF00(a7)       ;01 CF FF 00

          moveq #$01, d7        ;7E 01
          moveq #$02, d6        ;7C 02
          moveq #$FE, d5        ;7A FE
          moveq #$FF, d4        ;78 FF
          moveq #LATE_01, d3    ;76 01
          moveq #LATE_02, d2    ;74 02
          moveq #LATE_FE, d1    ;72 FE
          moveq #LATE_FF, d0    ;70 FF

          muls.w #$01, d7       ;CF FC 00 01
          muls.w #$02, d6       ;CD FC 00 02
          muls.w #$FE, d5       ;CB FC 00 FE
          muls.w #$FF, d4       ;C9 FC 00 FF
          muls.w #LATE_01, d3   ;C7 FC 00 01
          muls.w #LATE_02, d2   ;C5 FC 00 02
          muls.w #LATE_FE, d1   ;C3 FC 00 FE
          muls.w #LATE_FF, d0   ;C1 FC 00 FF

          muls.w ($0001).w, d7       ;CF F8 00 01
          muls.w ($00FF).w, d6       ;CD F8 00 FF
          muls.w ($0100).w, d5       ;CB F8 01 00
          muls.w ($FF00).w, d4       ;C9 F8 FF 00
          muls.w (LATE_0001).w, d3   ;C7 F8 00 01
          muls.w (LATE_00FF).w, d2   ;C5 F8 00 FF
          muls.w (LATE_0100).w, d1   ;C3 F8 01 00
          muls.w (LATE_FF00).w, d0   ;C1 F8 FF 00

          muls.w d0,d7               ;CF C0
          muls.w d1,d6               ;CD C1
          muls.w d2,d5               ;CB C2
          muls.w d3,d4               ;C9 C3
          muls.w d4,d3               ;C7 C4
          muls.w d5,d2               ;C5 C5
          muls.w d6,d1               ;C3 C6
          muls.w d7,d0               ;C1 C7

          muls.w (a7), d0            ;C1 D7
          muls.w (a6), d1            ;C3 D6
          muls.w (a5), d2            ;C5 D5
          muls.w (a4), d3            ;C7 D4
          muls.w (a3), d4            ;C9 D3
          muls.w (a2), d5            ;CB D2
          muls.w (a1), d6            ;CD D1
          muls.w (a0), d7            ;CF D0

          muls.w $01(a7), d0         ;C1 EF 00 01
          muls.w $02(a6), d1         ;C3 EE 00 02
          muls.w $FE(a5), d2         ;C5 ED 00 FE
          muls.w $FF(a4), d3         ;C7 EC 00 FF
          muls.w LATE_01(a3), d4     ;C9 EB 00 01
          muls.w LATE_02(a2), d5     ;CB EA 00 02
          muls.w LATE_FE(a1), d6     ;CD E9 00 FE
          muls.w LATE_FF(a0), d7     ;CF E8 00 FF

          muls.w (a7)+, d0           ;C1 DF
          muls.w (a6)+, d1           ;C3 DE
          muls.w (a5)+, d2           ;C5 DD
          muls.w (a4)+, d3           ;C7 DC
          muls.w (a3)+, d4           ;C9 DB
          muls.w (a2)+, d5           ;CB DA
          muls.w (a1)+, d6           ;CD D9
          muls.w (a0)+, d7           ;CF D8

          muls.w -(a7), d0           ;C1 E7
          muls.w -(a6), d1           ;C3 E6
          muls.w -(a5), d2           ;C5 E5
          muls.w -(a4), d3           ;C7 E4
          muls.w -(a3), d4           ;C9 E3
          muls.w -(a2), d5           ;CB E2
          muls.w -(a1), d6           ;CD E1
          muls.w -(a0), d7           ;CF E0


          mulu.w #$01, d7            ;CE FC 00 01
          mulu.w #$02, d6            ;CC FC 00 02
          mulu.w #$FE, d5            ;CA FC 00 FE
          mulu.w #$FF, d4            ;C8 FC 00 FF
          mulu.w #LATE_01, d3        ;C6 FC 00 01
          mulu.w #LATE_02, d2        ;C4 FC 00 02
          mulu.w #LATE_FE, d1        ;C2 FC 00 FE
          mulu.w #LATE_FF, d0        ;C0 FC 00 FF

          mulu.w ($0001).w, d7       ;CE F8 00 01
          mulu.w ($00FF).w, d6       ;CC F8 00 FF
          mulu.w ($0100).w, d5       ;CA F8 01 00
          mulu.w ($FF00).w, d4       ;C8 F8 FF 00
          mulu.w (LATE_0001).w, d3   ;C6 F8 00 01
          mulu.w (LATE_00FF).w, d2   ;C4 F8 00 FF
          mulu.w (LATE_0100).w, d1   ;C2 F8 01 00
          mulu.w (LATE_FF00).w, d0   ;C0 F8 FF 00

          mulu.w d0,d7               ;CE C0
          mulu.w d1,d6               ;CC C1
          mulu.w d2,d5               ;CA C2
          mulu.w d3,d4               ;C8 C3
          mulu.w d4,d3               ;C6 C4
          mulu.w d5,d2               ;C4 C5
          mulu.w d6,d1               ;C2 C6
          mulu.w d7,d0               ;C0 C7

          mulu.w (a7), d0            ;C0 D7
          mulu.w (a6), d1            ;C2 D6
          mulu.w (a5), d2            ;C4 D5
          mulu.w (a4), d3            ;C6 D4
          mulu.w (a3), d4            ;C8 D3
          mulu.w (a2), d5            ;CA D2
          mulu.w (a1), d6            ;CC D1
          mulu.w (a0), d7            ;CE D0

          mulu.w $01(a7), d0         ;C0 EF 00 01
          mulu.w $02(a6), d1         ;C2 EE 00 02
          mulu.w $FE(a5), d2         ;C4 ED 00 FE
          mulu.w $FF(a4), d3         ;C6 EC 00 FF
          mulu.w LATE_01(a3), d4     ;C8 EB 00 01
          mulu.w LATE_02(a2), d5     ;CA EA 00 02
          mulu.w LATE_FE(a1), d6     ;CC E9 00 FE
          mulu.w LATE_FF(a0), d7     ;CE E8 00 FF

          mulu.w (a7)+, d0           ;C0 DF
          mulu.w (a6)+, d1           ;C2 DE
          mulu.w (a5)+, d2           ;C4 DD
          mulu.w (a4)+, d3           ;C6 DC
          mulu.w (a3)+, d4           ;C8 DB
          mulu.w (a2)+, d5           ;CA DA
          mulu.w (a1)+, d6           ;CC D9
          mulu.w (a0)+, d7           ;CE D8

          mulu.w -(a7), d0           ;C0 E7
          mulu.w -(a6), d1           ;C2 E6
          mulu.w -(a5), d2           ;C4 E5
          mulu.w -(a4), d3           ;C6 E4
          mulu.w -(a3), d4           ;C8 E3
          mulu.w -(a2), d5           ;CA E2
          mulu.w -(a1), d6           ;CC E1
          mulu.w -(a0), d7           ;CE E0

          neg.l d0              ;44 80
          neg.l d1              ;44 81
          neg.l d2              ;44 82
          neg.l d3              ;44 83
          neg.l d4              ;44 84
          neg.l d5              ;44 85
          neg.l d6              ;44 86
          neg.l d7              ;44 87

          neg.w (a0)            ;44 50
          neg.w (a1)            ;44 51
          neg.w (a2)            ;44 52
          neg.w (a3)            ;44 53
          neg.w (a4)            ;44 54
          neg.w (a5)            ;44 55
          neg.w (a6)            ;44 56
          neg.w (a7)            ;44 57

          neg.w $01(a0)         ;44 68
          neg.w $02(a1)         ;44 69
          neg.w $FE(a2)         ;44 6A
          neg.w $FF(a3)         ;44 6B
          neg.w LATE_01(a4)     ;44 6C
          neg.w LATE_02(a5)     ;44 6D
          neg.w LATE_FE(a6)     ;44 6E
          neg.w LATE_FF(a7)     ;44 6F

          neg.w (a0)+           ;44 58
          neg.w (a1)+           ;44 59
          neg.w (a2)+           ;44 5A
          neg.w (a3)+           ;44 5B
          neg.w (a4)+           ;44 5C
          neg.w (a5)+           ;44 5D
          neg.w (a6)+           ;44 5E
          neg.w (a7)+           ;44 5F

          neg.w -(a0)           ;44 60
          neg.w -(a1)           ;44 61
          neg.w -(a2)           ;44 62
          neg.w -(a3)           ;44 63
          neg.w -(a4)           ;44 64
          neg.w -(a5)           ;44 65
          neg.w -(a6)           ;44 66
          neg.w -(a7)           ;44 67

          nop                   ;4E 71

          not.w ($0001).w       ;46 78 00 01
          not.w ($00FF).w       ;46 78 00 FF
          not.w ($0100).w       ;46 78 01 00
          not.w ($FF00).w       ;46 78 FF 00
          not.w (LATE_0001).w   ;46 78 00 01
          not.w (LATE_00FF).w   ;46 78 00 FF
          not.w (LATE_0100).w   ;46 78 01 00
          not.w (LATE_FF00).w   ;46 78 FF 00

          not.l d0              ;46 80
          not.l d1              ;46 81
          not.l d2              ;46 82
          not.l d3              ;46 83
          not.l d4              ;46 84
          not.l d5              ;46 85
          not.l d6              ;46 86
          not.l d7              ;46 87

          not.w (a0)            ;46 50
          not.w (a1)            ;46 51
          not.w (a2)            ;46 52
          not.w (a3)            ;46 53
          not.w (a4)            ;46 54
          not.w (a5)            ;46 55
          not.w (a6)            ;46 56
          not.w (a7)            ;46 57

          not.w $01(a0)         ;46 68
          not.w $02(a1)         ;46 69
          not.w $FE(a2)         ;46 6A
          not.w $FF(a3)         ;46 6B
          not.w LATE_01(a4)     ;46 6C
          not.w LATE_02(a5)     ;46 6D
          not.w LATE_FE(a6)     ;46 6E
          not.w LATE_FF(a7)     ;46 6F

          not.w (a0)+           ;46 58
          not.w (a1)+           ;46 59
          not.w (a2)+           ;46 5A
          not.w (a3)+           ;46 5B
          not.w (a4)+           ;46 5C
          not.w (a5)+           ;46 5D
          not.w (a6)+           ;46 5E
          not.w (a7)+           ;46 5F

          not.w -(a0)           ;46 60
          not.w -(a1)           ;46 61
          not.w -(a2)           ;46 62
          not.w -(a3)           ;46 63
          not.w -(a4)           ;46 64
          not.w -(a5)           ;46 65
          not.w -(a6)           ;46 66
          not.w -(a7)           ;46 67

          ori.b #$01, d7             ;00 07 00 01
          ori.b #$02, d6             ;00 06 00 02
          ori.b #$FE, d5             ;00 05 00 FE
          ori.b #$FF, d4             ;00 04 00 FF
          ori.b #LATE_01, d3         ;00 03 00 01
          ori.b #LATE_02, d2         ;00 02 00 02
          ori.b #LATE_FE, d1         ;00 01 00 FE
          ori.b #LATE_FF, d0         ;00 00 00 FF

          ori.w #$0001, d7           ;00 47 00 01
          ori.w #$00FF, d6           ;00 46 00 FF
          ori.w #$0100, d5           ;00 45 01 00
          ori.w #$FF00, d4           ;00 44 FF 00
          ori.w #LATE_0001, d3       ;00 43 00 01
          ori.w #LATE_00FF, d2       ;00 42 00 FF
          ori.w #LATE_0100, d1       ;00 41 01 00
          ori.w #LATE_FF00, d0       ;00 40 FF 00

          ori.l #$00000001, d7       ;00 87 00 00 00 01
          ori.l #$0000FF00, d6       ;00 86 00 00 FF 00
          ori.l #$00010000, d5       ;00 85 00 01 00 00
          ori.l #$FF000000, d4       ;00 84 FF 00 00 00
          ori.l #LATE_00000001, d3   ;00 83 00 00 00 01
          ori.l #LATE_0000FF00, d2   ;00 82 00 00 FF 00
          ori.l #LATE_00010000, d1   ;00 81 00 01 00 00
          ori.l #LATE_FF000000, d0   ;00 80 FF 00 00 00


LATE_01 = $01
LATE_02 = $02
LATE_FE = $FE
LATE_FF = $FF

LATE_0001 = $0001
LATE_00FF = $00FF
LATE_0100 = $0100
LATE_FF00 = $FF00
LATE_000001 = $000001
LATE_00FF00 = $00FF00
LATE_010000 = $010000
LATE_FF0000 = $FF0000
LATE_123456 = $123456

LATE_00000001 = $00000001
LATE_0000FF00 = $0000FF00
LATE_00010000 = $00010000
LATE_FF000000 = $FF000000
