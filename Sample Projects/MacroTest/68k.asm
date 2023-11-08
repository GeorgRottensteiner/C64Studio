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

LATE_00000001 = $00000001
LATE_0000FF00 = $0000FF00
LATE_00010000 = $00010000
LATE_FF000000 = $FF000000
