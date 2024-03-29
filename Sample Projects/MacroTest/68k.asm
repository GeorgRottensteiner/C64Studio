!cpu 68000

* = $00

          ;adda #$x, ax
          adda #$0001,a0              ;D0 FC 00 01 - 52 48 ??
          adda #$00FF,a1              ;D2 FC 00 FF
          adda #$0100,a2              ;D4 FC 01 00
          adda #$FF00,a3              ;D6 FC FF 00
          adda #LATE_0001,a4          ;D8 FC 00 01  -52 4C ??
          adda #LATE_00FF,a5          ;DA FC 00 FF
          adda #LATE_0100,a6          ;DC FC 01 00
          adda #LATE_FF00,a7          ;DE FC FF 00
          adda #$1234,sp              ;DE FC 12 34

          adda.w #$0001,a0            ;D0 FC 00 01 - 52 48 ??
          adda.w #$00FF,a1            ;D2 FC 00 FF
          adda.w #$0100,a2            ;D4 FC 01 00
          adda.w #$FF00,a3            ;D6 FC FF 00
          adda.w #LATE_0001,a4        ;D8 FC 00 01  -52 4C ??
          adda.w #LATE_00FF,a5        ;DA FC 00 FF
          adda.w #LATE_0100,a6        ;DC FC 01 00
          adda.w #LATE_FF00,a7        ;DE FC FF 00
          adda.w #$1234,sp            ;DE FC 12 34


          ;adda ($x), ax
          adda ($0001).w, a0        ;D0 F8 00 01
          adda ($00FF).w, a1        ;D2 F8 00 FF
          adda ($0100).w, a2        ;D4 F8 01 00
          adda ($FF00).w, a3        ;D6 F8 FF 00
          adda (LATE_0001).w, a4    ;D8 F8 00 01
          adda (LATE_00FF).w, a5    ;DA F8 00 FF
          adda (LATE_0100).w, a6    ;DC F8 01 00
          adda (LATE_FF00).w, a7    ;DE F8 FF 00
          adda ($1234).w, sp        ;DE F8 12 34

          adda.w ($0001).w, a0        ;D0 F8 00 01
          adda.w ($00FF).w, a1        ;D2 F8 00 FF
          adda.w ($0100).w, a2        ;D4 F8 01 00
          adda.w ($FF00).w, a3        ;D6 F8 FF 00
          adda.w (LATE_0001).w, a4    ;D8 F8 00 01
          adda.w (LATE_00FF).w, a5    ;DA F8 00 FF
          adda.w (LATE_0100).w, a6    ;DC F8 01 00
          adda.w (LATE_FF00).w, a7    ;DE F8 FF 00
          adda.w ($1234).w, sp        ;DE F8 12 34

          adda.l ($000001).l, a0        ;D1 F9 00 00 00 01
          adda.l ($00FF00).l, a1        ;D3 F9 00 00 FF 00
          adda.l ($010000).l, a2        ;D5 F9 00 01 00 00
          adda.l ($FF0000).l, a3        ;D7 F9 00 FF 00 00
          adda.l (LATE_000001).l, a4    ;D9 F9 00 00 00 01
          adda.l (LATE_00FF00).l, a5    ;DB F9 00 00 FF 00
          adda.l (LATE_010000).l, a6    ;DD F9 00 01 00 00
          adda.l (LATE_FF0000).l, a7    ;DF F9 00 FF 00 00
          adda.l ($123456).l, sp        ;DF F9 00 12 34 56


          ;adda dx, ax
          adda.w d7, a0                 ;D0 C7
          adda.w d6, a1                 ;D2 C6
          adda.w d5, a2                 ;D4 C5
          adda.w d4, a3                 ;D6 C4
          adda.w d3, a4                 ;D8 C3
          adda.w d2, a5                 ;DA C2
          adda.w d1, a6                 ;DC C1
          adda.w d0, a7                 ;DE C0
          adda.w d0, sp                 ;DE C0

          ;adda dx, ax
          adda d7, a0                 ;D0 C7
          adda d6, a1                 ;D2 C6
          adda d5, a2                 ;D4 C5
          adda d4, a3                 ;D6 C4
          adda d3, a4                 ;D8 C3
          adda d2, a5                 ;DA C2
          adda d1, a6                 ;DC C1
          adda d0, a7                 ;DE C0
          adda d0, sp                 ;DE C0


          ;adda (ax), ax
          adda.w (sp), a0               ;D0 D7
          adda.w (a7), a1               ;D2 D7
          adda.w (a6), a2               ;D4 D6
          adda.w (a5), a3               ;D6 D5
          adda.w (a4), a4               ;D8 D4
          adda.w (a3), a5               ;DA D3
          adda.w (a2), a6               ;DC D2
          adda.w (a1), a7               ;DE D1
          adda.w (a0), sp               ;DE D0

          ;adda (ax), ax
          adda (sp), a0               ;D0 D7
          adda (a7), a1               ;D2 D7
          adda (a6), a2               ;D4 D6
          adda (a5), a3               ;D6 D5
          adda (a4), a4               ;D8 D4
          adda (a3), a5               ;DA D3
          adda (a2), a6               ;DC D2
          adda (a1), a7               ;DE D1
          adda (a0), sp               ;DE D0


          ;adda $x(ax), ax
          adda.w $0001(a0), a7          ;DE E8 00 01
          adda.w $00FF(a1), a6          ;DC E9 00 FF
          adda.w LATE_01(a4), a3        ;D6 EC 00 01
          adda.w LATE_FF(a5), a2        ;D4 ED 00 FF

          adda $0001(a0), a7          ;DE E8 00 01
          adda $00FF(a1), a6          ;DC E9 00 FF
          adda LATE_01(a4), a3        ;D6 EC 00 01
          adda LATE_FF(a5), a2        ;D4 ED 00 FF


          ;adda (ax)+, ax
          adda.w (sp)+, a0              ;D0 DF
          adda.w (a7)+, a1              ;D2 DF
          adda.w (a6)+, a2              ;D4 DE
          adda.w (a5)+, a3              ;D6 DD
          adda.w (a4)+, a4              ;D8 DC
          adda.w (a3)+, a5              ;DA DB
          adda.w (a2)+, a6              ;DC DA
          adda.w (a1)+, a7              ;DE D9
          adda.w (a0)+, sp              ;DE D8

          adda (sp)+, a0              ;D0 DF
          adda (a7)+, a1              ;D2 DF
          adda (a6)+, a2              ;D4 DE
          adda (a5)+, a3              ;D6 DD
          adda (a4)+, a4              ;D8 DC
          adda (a3)+, a5              ;DA DB
          adda (a2)+, a6              ;DC DA
          adda (a1)+, a7              ;DE D9
          adda (a0)+, sp              ;DE D8


          ;adda -(ax), ax
          adda.w -(sp), a0              ;D0 E7
          adda.w -(a7), a1              ;D2 E7
          adda.w -(a6), a2              ;D4 E6
          adda.w -(a5), a3              ;D6 E5
          adda.w -(a4), a4              ;D8 E4
          adda.w -(a3), a5              ;DA E3
          adda.w -(a2), a6              ;DC E2
          adda.w -(a1), a7              ;DE E1
          adda.w -(a0), sp              ;DE E0

          adda -(sp), a0              ;D0 E7
          adda -(a7), a1              ;D2 E7
          adda -(a6), a2              ;D4 E6
          adda -(a5), a3              ;D6 E5
          adda -(a4), a4              ;D8 E4
          adda -(a3), a5              ;DA E3
          adda -(a2), a6              ;DC E2
          adda -(a1), a7              ;DE E1
          adda -(a0), sp              ;DE E0


          ;adda (ax,dx), ax
          adda.w (a0,d7.w), a7          ;DE F0 70 00
          adda.w (a1,d6.w), a6          ;DC F1 60 00
          adda.w (a2,d5.w), a5          ;DA F2 50 00
          adda.w (a3,d4.w), a4          ;D8 F3 40 00
          adda.w (a4,d3.w), a3          ;D6 F4 30 00
          adda.w (a5,d2.w), a2          ;D4 F5 20 00
          adda.w (a6,d1.w), a1          ;D2 F6 10 00
          adda.w (a7,d0.w), a0          ;D0 F7 00 00

          adda (a0,d7.w), a7          ;DE F0 70 00
          adda (a1,d6.w), a6          ;DC F1 60 00
          adda (a2,d5.w), a5          ;DA F2 50 00
          adda (a3,d4.w), a4          ;D8 F3 40 00
          adda (a4,d3.w), a3          ;D6 F4 30 00
          adda (a5,d2.w), a2          ;D4 F5 20 00
          adda (a6,d1.w), a1          ;D2 F6 10 00
          adda (a7,d0.w), a0          ;D0 F7 00 00

          ;adda $x(ax,dx) ax
          adda.w $01(a0,d7.w), a7       ;DE F0 70 01
          adda.w $7F(a1,d6.w), a6       ;DC F1 60 7F
          adda.w LATE_01(a2,d5.w), a5   ;DA F2 50 01
          adda.w LATE_7F(a3,d4.w), a4   ;D8 F3 40 7F
          adda.w $01(a4,d3.w), a3       ;D6 F4 30 01
          adda.w $7F(a5,d2.w), a2       ;D4 F5 20 7F
          adda.w LATE_01(a6,d1.w), a1   ;D2 F6 10 01
          adda.w LATE_7F(a7,d0.w), a0   ;D0 F7 00 7F

          adda $01(a0,d7.w), a7       ;DE F0 70 01
          adda $7F(a1,d6.w), a6       ;DC F1 60 7F
          adda LATE_01(a2,d5.w), a5   ;DA F2 50 01
          adda LATE_7F(a3,d4.w), a4   ;D8 F3 40 7F
          adda $01(a4,d3.w), a3       ;D6 F4 30 01
          adda $7F(a5,d2.w), a2       ;D4 F5 20 7F
          adda LATE_01(a6,d1.w), a1   ;D2 F6 10 01
          adda LATE_7F(a7,d0.w), a0   ;D0 F7 00 7F


          ;addi #$x, dx
          addi.b #$01,d0                ;06 00 00 01    - 5200 ??
          addi.b #$FF,d1                ;06 01 00 FF
          addi.b #$01,d2                ;06 02 00 01    - 5202 ??
          addi.b #$FF,d3                ;06 03 00 FF
          addi.b #LATE_01,d4            ;06 04 00 01    - 5204 ??
          addi.b #LATE_FF,d5            ;06 05 00 FF
          addi.b #LATE_01,d6            ;06 06 00 01    - 5206 ??
          addi.b #LATE_FF,d7            ;06 07 00 FF

          addi.l #$00000001,d0          ;06 80 00 00 00 01  -- 5280 ??
          addi.l #$000000FF,d1          ;06 81 00 00 00 FF
          addi.l #$00000100,d2          ;06 82 00 00 01 00
          addi.l #$0000FF00,d3          ;06 83 00 00 FF 00
          addi.l #$00010000,d4          ;06 84 00 01 00 00
          addi.l #$00FF0000,d5          ;06 85 00 FF 00 00
          addi.l #$01000000,d6          ;06 86 01 00 00 00
          addi.l #$FF000000,d7          ;06 87 FF 00 00 00

          addi.w #$0001,d0                ;06 00 00 01    - 5200 ??
          addi.w #$00FF,d1                ;06 01 00 FF
          addi.w #$0100,d2                ;06 02 00 01    - 5202 ??
          addi.w #$FF00,d3                ;06 03 00 FF
          addi.w #LATE_0001,d4            ;06 04 00 01    - 5204 ??
          addi.w #LATE_00FF,d5            ;06 05 00 FF
          addi.w #LATE_0100,d6            ;06 06 00 01    - 5206 ??
          addi.w #LATE_FF00,d7            ;06 07 00 FF

          addi.w #$00000001,d0          ;06 80 00 00 00 01  -- 5280 ??
          addi.w #$000000FF,d1          ;06 81 00 00 00 FF
          addi.w #$00000100,d2          ;06 82 00 00 01 00
          addi.w #$0000FF00,d3          ;06 83 00 00 FF 00

          addi #$01,d0                ;06 00 00 01    - 5200 ??
          addi #$FF,d1                ;06 01 00 FF
          addi #$01,d2                ;06 02 00 01    - 5202 ??
          addi #$FF,d3                ;06 03 00 FF
          addi #LATE_01,d4            ;06 04 00 01    - 5204 ??
          addi #LATE_FF,d5            ;06 05 00 FF
          addi #LATE_01,d6            ;06 06 00 01    - 5206 ??
          addi #LATE_FF,d7            ;06 07 00 FF

          addi #$00000001,d0          ;06 80 00 00 00 01  -- 5280 ??
          addi #$000000FF,d1          ;06 81 00 00 00 FF
          addi #$00000100,d2          ;06 82 00 00 01 00
          addi #$0000FF00,d3          ;06 83 00 00 FF 00


          ;addi #$x, ($x)
          addi.w #$0001, ($FF00).w          ;06 78 00 01 FF 00  - 5278 FF 00 ??
          addi.w #$00FF, ($0100).w          ;06 78 00 FF 01 00
          addi.w #$0100, (LATE_00FF).w      ;06 78 01 00 00 FF
          addi.w #$FF00, (LATE_0001).w      ;06 78 FF 00 00 01
          addi.w #LATE_0001, ($FF00).w      ;06 78 00 01 FF 00  - 5278 FF 00 ??
          addi.w #LATE_00FF, ($0100).w      ;06 78 00 FF 01 00
          addi.w #LATE_0100, (LATE_00FF).w  ;06 78 01 00 00 FF
          addi.w #LATE_FF00, (LATE_0001).w  ;06 78 FF 00 00 01

          addi #$0001, ($FF00).w          ;06 78 00 01 FF 00  - 5278 FF 00 ??
          addi #$00FF, ($0100).w          ;06 78 00 FF 01 00
          addi #$0100, (LATE_00FF).w      ;06 78 01 00 00 FF
          addi #$FF00, (LATE_0001).w      ;06 78 FF 00 00 01
          addi #LATE_0001, ($FF00).w      ;06 78 00 01 FF 00  - 5278 FF 00 ??
          addi #LATE_00FF, ($0100).w      ;06 78 00 FF 01 00
          addi #LATE_0100, (LATE_00FF).w  ;06 78 01 00 00 FF
          addi #LATE_FF00, (LATE_0001).w  ;06 78 FF 00 00 01

          addi.w #$0001, ($FF00)          ;06 78 00 01 FF 00  - 5278 FF 00 ??
          addi.w #$00FF, ($0100)          ;06 78 00 FF 01 00
          addi.w #$0100, (LATE_00FF)      ;06 78 01 00 00 FF
          addi.w #$FF00, (LATE_0001)      ;06 78 FF 00 00 01
          addi.w #LATE_0001, ($FF00)      ;06 78 00 01 FF 00  - 5278 FF 00 ??
          addi.w #LATE_00FF, ($0100)      ;06 78 00 FF 01 00
          addi.w #LATE_0100, (LATE_00FF)  ;06 78 01 00 00 FF
          addi.w #LATE_FF00, (LATE_0001)  ;06 78 FF 00 00 01

          addi #$0001, ($FF00)          ;06 78 00 01 FF 00  - 5278 FF 00 ??
          addi #$00FF, ($0100)          ;06 78 00 FF 01 00
          addi #$0100, (LATE_00FF)      ;06 78 01 00 00 FF
          addi #$FF00, (LATE_0001)      ;06 78 FF 00 00 01
          addi #LATE_0001, ($FF00)      ;06 78 00 01 FF 00  - 5278 FF 00 ??
          addi #LATE_00FF, ($0100)      ;06 78 00 FF 01 00
          addi #LATE_0100, (LATE_00FF)  ;06 78 01 00 00 FF
          addi #LATE_FF00, (LATE_0001)  ;06 78 FF 00 00 01

          addi.w #$0001, ($FF000000).l          ;06 79 00 01 FF 00 00 00  - 5279 FF000000 ??
          addi.w #$00FF, ($00010000).l          ;06 79 00 FF 00 01 00 00
          addi.w #$0100, (LATE_0000FF00).l      ;06 79 01 00 00 00 FF 00
          addi.w #$FF00, (LATE_00000001).l      ;06 79 FF 00 00 00 00 01
          addi.w #LATE_0001, ($FF000000).l      ;06 79 00 01 FF 00 00 00  - 5279 FF000000 ??
          addi.w #LATE_00FF, ($00010000).l      ;06 79 00 FF 00 01 00 00
          addi.w #LATE_0100, (LATE_0000FF00).l  ;06 79 01 00 00 00 FF 00
          addi.w #LATE_FF00, (LATE_00000001).l  ;06 79 FF 00 00 00 00 01

          addi #$0001, ($FF000000).l          ;06 79 00 01 FF 00 00 00
          addi #$00FF, ($00010000).l          ;06 79 00 FF 00 01 00 00
          addi #$0100, (LATE_0000FF00).l      ;06 79 01 00 00 00 FF 00
          addi #$FF00, (LATE_00000001).l      ;06 79 FF 00 00 00 00 01
          addi #LATE_0001, ($FF000000).l      ;06 79 00 01 FF 00 00 00
          addi #LATE_00FF, ($00010000).l      ;06 79 00 FF 00 01 00 00
          addi #LATE_0100, (LATE_0000FF00).l  ;06 79 01 00 00 00 FF 00
          addi #LATE_FF00, (LATE_00000001).l  ;06 79 FF 00 00 00 00 01

          addi #$0001, ($FF000000)          ;06 79 00 01 FF 00 00 00
          addi #$00FF, ($00010000)          ;06 79 00 FF 00 01 00 00
          addi #$0100, (LATE_0000FF00)      ;06 79 01 00 00 00 FF 00
          addi #$FF00, (LATE_00000001)      ;06 79 FF 00 00 00 00 01
          addi #LATE_0001, ($FF000000)      ;06 79 00 01 FF 00 00 00
          addi #LATE_00FF, ($00010000)      ;06 79 00 FF 00 01 00 00
          addi #LATE_0100, (LATE_0000FF00)  ;06 79 01 00 00 00 FF 00
          addi #LATE_FF00, (LATE_00000001)  ;06 79 FF 00 00 00 00 01

          addi.l #$00000001, ($FF00).w          ;06 B8 00 00 00 01 FF 00    - 52B8 FF00
          addi.l #$0000FF00, ($0100).w          ;06 B8 00 00 FF 00 01 00
          addi.l #$00010000, (LATE_00FF).w      ;06 B8 00 01 00 01 00 FF
          addi.l #$FF000000, (LATE_0001).w      ;06 B8 FF 00 00 01 00 01
          addi.l #LATE_00000001, ($FF00).w      ;06 B8 00 00 00 01 FF 00
          addi.l #LATE_0000FF00, ($0100).w      ;06 B8 00 00 FF 00 01 00
          addi.l #LATE_00010000, (LATE_00FF).w  ;06 B8 00 01 00 00 00 FF
          addi.l #LATE_FF000000, (LATE_0001).w  ;06 B8 FF 00 00 00 00 01


          ;addi #$x, (ax)
          addi.b #$01, (a7)                     ;06 17 00 01  - 5217
          addi.b #$02, (a6)                     ;06 16 00 02  - 5416
          addi.b #$FE, (a5)                     ;06 15 00 FE
          addi.b #$FF, (a4)                     ;06 14 00 FF
          addi.b #LATE_01, (a3)                 ;06 13 00 01  - 5213
          addi.b #LATE_02, (a2)                 ;06 12 00 02  - 5412
          addi.b #LATE_FE, (a1)                 ;06 11 00 FE
          addi.b #LATE_FF, (a0)                 ;06 10 00 FF

          addi #$01, (a7)                     ;06 17 00 01  - 5217
          addi #$02, (a6)                     ;06 16 00 02  - 5416
          addi #$FE, (a5)                     ;06 15 00 FE
          addi #$FF, (a4)                     ;06 14 00 FF
          addi #LATE_01, (a3)                 ;06 13 00 01  - 5213
          addi #LATE_02, (a2)                 ;06 12 00 02  - 5412
          addi #LATE_FE, (a1)                 ;06 11 00 FE
          addi #LATE_FF, (a0)                 ;06 10 00 FF


          ;addi #$x, $x(ax)
          addi.w #$0001, $FF(a0)          ;06 68 00 01 00 FF  - 5268 00FF
          addi.w #$00FF, $FE(a1)          ;06 69 00 FF 00 FE
          addi.w #$0100, LATE_02(a2)      ;06 6A 01 00 00 02
          addi.w #$FF00, LATE_01(a3)      ;06 6B FF 00 00 01
          addi.w #LATE_0001, $FF(a4)      ;06 6C 00 01 00 FF  - 526C 00FF
          addi.w #LATE_00FF, $FE(a5)      ;06 6D 00 FF 00 FE
          addi.w #LATE_0100, LATE_02(a6)  ;06 6E 01 00 00 02
          addi.w #LATE_FF00, LATE_01(a7)  ;06 6F FF 00 00 01

          addi.l #$00000001, $FF(a0)          ;06 A8 00 00 00 01 00 FF  - 52A8 00FF
          addi.l #$0000FF00, $01(a1)          ;06 A9 00 00 FF 00 00 01
          addi.l #$00010000, LATE_FF(a2)      ;06 AA 00 01 00 00 00 FF
          addi.l #$FF000000, LATE_01(a3)      ;06 AB FF 00 00 00 00 01
          addi.l #LATE_00000001, $FF(a4)      ;06 AC 00 00 00 01 00 FF
          addi.l #LATE_0000FF00, $01(a5)      ;06 AD 00 00 FF 00 00 01
          addi.l #LATE_00010000, LATE_FF(a6)  ;06 AE 00 01 00 00 00 FF
          addi.l #LATE_FF000000, LATE_01(a7)  ;06 AF FF 00 00 00 00 01

          addi #$0001, $FF(a0)          ;06 68 00 01 00 FF  - 5268 00FF
          addi #$00FF, $FE(a1)          ;06 69 00 FF 00 FE
          addi #$0100, LATE_02(a2)      ;06 6A 01 00 00 02
          addi #$FF00, LATE_01(a3)      ;06 6B FF 00 00 01
          addi #LATE_0001, $FF(a4)      ;06 6C 00 01 00 FF  - 526C 00FF
          addi #LATE_00FF, $FE(a5)      ;06 6D 00 FF 00 FE
          addi #LATE_0100, LATE_02(a6)  ;06 6E 01 00 00 02
          addi #LATE_FF00, LATE_01(a7)  ;06 6F FF 00 00 01


          ;addi #$x, (ax)+
          addi.b #$01, (a0)+              ;06 18 00 01  - 5258
          addi.b #$02, (a1)+              ;06 19 00 02  - 5459
          addi.b #$FE, (a2)+              ;06 1A 00 FE
          addi.b #$FF, (a3)+              ;06 1B 00 FF
          addi.b #LATE_01, (a4)+          ;06 1C 00 01
          addi.b #LATE_02, (a5)+          ;06 1D 00 02
          addi.b #LATE_FE, (a6)+          ;06 1E 00 FE
          addi.b #LATE_FF, (a7)+          ;06 1F 00 FF

          addi #$01, (a0)+              ;06 18 00 01  - 5258
          addi #$02, (a1)+              ;06 19 00 02  - 5459
          addi #$FE, (a2)+              ;06 1A 00 FE
          addi #$FF, (a3)+              ;06 1B 00 FF
          addi #LATE_01, (a4)+          ;06 1C 00 01
          addi #LATE_02, (a5)+          ;06 1D 00 02
          addi #LATE_FE, (a6)+          ;06 1E 00 FE
          addi #LATE_FF, (a7)+          ;06 1F 00 FF


          ;addi #$x, -(ax)
          addi.b #$01, -(a0)              ;06 20 00 01
          addi.b #$02, -(a1)              ;06 21 00 02
          addi.b #$FE, -(a2)              ;06 22 00 FE
          addi.b #$FF, -(a3)              ;06 23 00 FF
          addi.b #LATE_01, -(a4)          ;06 24 00 01
          addi.b #LATE_02, -(a5)          ;06 25 00 02
          addi.b #LATE_FE, -(a6)          ;06 26 00 FE
          addi.b #LATE_FF, -(a7)          ;06 27 00 FF

          addi #$01, -(a0)              ;06 20 00 01
          addi #$02, -(a1)              ;06 21 00 02
          addi #$FE, -(a2)              ;06 22 00 FE
          addi #$FF, -(a3)              ;06 23 00 FF
          addi #LATE_01, -(a4)          ;06 24 00 01
          addi #LATE_02, -(a5)          ;06 25 00 02
          addi #LATE_FE, -(a6)          ;06 26 00 FE
          addi #LATE_FF, -(a7)          ;06 27 00 FF


          ;addq #$x, ($x)
          addq.b #1, ($0001).w            ;52 38 00 01
          addq.b #2, ($00FF).w            ;54 38 00 FF
          addq.b #3, ($0100).w            ;56 38 01 00
          addq.b #4, ($FF00).w            ;58 38 FF 00
          addq.b #5, (LATE_0001).w        ;5A 38 00 01
          addq.b #6, (LATE_00FF).w        ;5C 38 00 FF
          addq.b #7, (LATE_0100).w        ;5E 38 01 00
          addq.b #8, (LATE_FF00).w        ;50 38 FF 00

          addq #1, ($0001).w            ;52 38 00 01
          addq #2, ($00FF).w            ;54 38 00 FF
          addq #3, ($0100).w            ;56 38 01 00
          addq #4, ($FF00).w            ;58 38 FF 00
          addq #5, (LATE_0001).w        ;5A 38 00 01
          addq #6, (LATE_00FF).w        ;5C 38 00 FF
          addq #7, (LATE_0100).w        ;5E 38 01 00
          addq #8, (LATE_FF00).w        ;50 38 FF 00

          addq.b #1, ($0001)            ;52 38 00 01
          addq.b #2, ($00FF)            ;54 38 00 FF
          addq.b #3, ($0100)            ;56 38 01 00
          addq.b #4, ($FF00)            ;58 38 FF 00
          addq.b #5, (LATE_0001)        ;5A 38 00 01
          addq.b #6, (LATE_00FF)        ;5C 38 00 FF
          addq.b #7, (LATE_0100)        ;5E 38 01 00
          addq.b #8, (LATE_FF00)        ;50 38 FF 00

          addq #1, ($0001)            ;52 38 00 01
          addq #2, ($00FF)            ;54 38 00 FF
          addq #3, ($0100)            ;56 38 01 00
          addq #4, ($FF00)            ;58 38 FF 00
          addq #5, (LATE_0001)        ;5A 38 00 01
          addq #6, (LATE_00FF)        ;5C 38 00 FF
          addq #7, (LATE_0100)        ;5E 38 01 00
          addq #8, (LATE_FF00)        ;50 38 FF 00

          ;addq #$x, dx
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

          addq #1, d7                   ;52 47
          addq #2, d6                   ;54 46
          addq #3, d5                   ;56 45
          addq #4, d4                   ;58 44
          addq #5, d3                   ;5A 43
          addq #6, d2                   ;5C 42
          addq #7, d1                   ;5E 41
          addq #8, d0                   ;50 40

          ;addq #$x, (ax)
          addq.w #1, (a7)                 ;52 57
          addq.w #2, (a6)                 ;54 56
          addq.w #3, (a5)                 ;56 55
          addq.w #4, (a4)                 ;58 54
          addq.w #5, (a3)                 ;5A 53
          addq.w #6, (a2)                 ;5C 52
          addq.w #7, (a1)                 ;5E 51
          addq.w #8, (a0)                 ;50 50

          addq #1, (a7)                 ;52 57
          addq #2, (a6)                 ;54 56
          addq #3, (a5)                 ;56 55
          addq #4, (a4)                 ;58 54
          addq #5, (a3)                 ;5A 53
          addq #6, (a2)                 ;5C 52
          addq #7, (a1)                 ;5E 51
          addq #8, (a0)                 ;50 50


          ;addq #$x, $x($x)
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

          addq #1, $01(a7)              ;52 6F 00 01
          addq #2, $02(a6)              ;54 6E 00 02
          addq #3, $FE(a5)              ;56 6D 00 FE
          addq #4, $FF(a4)              ;58 6C 00 FF
          addq #5, LATE_01(a3)          ;5A 6B 00 01
          addq #6, LATE_02(a2)          ;5C 6A 00 02
          addq #7, LATE_FE(a1)          ;5E 69 00 FE
          addq #8, LATE_FF(a0)          ;50 68 00 FF


          ;addq #$x, (ax)+
          addq.w #1, (a7)+                ;52 5F
          addq.w #2, (a6)+                ;54 5E
          addq.w #3, (a5)+                ;56 5D
          addq.w #4, (a4)+                ;58 5C
          addq.w #5, (a3)+                ;5A 5B
          addq.w #6, (a2)+                ;5C 5A
          addq.w #7, (a1)+                ;5E 59
          addq.w #8, (a0)+                ;50 58

          addq #1, (a7)+                ;52 5F
          addq #2, (a6)+                ;54 5E
          addq #3, (a5)+                ;56 5D
          addq #4, (a4)+                ;58 5C
          addq #5, (a3)+                ;5A 5B
          addq #6, (a2)+                ;5C 5A
          addq #7, (a1)+                ;5E 59
          addq #8, (a0)+                ;50 58


          ;addq #$x, -(ax)
          addq.w #1, -(a7)                ;52 67
          addq.w #2, -(a6)                ;54 66
          addq.w #3, -(a5)                ;56 65
          addq.w #4, -(a4)                ;58 64
          addq.w #5, -(a3)                ;5A 63
          addq.w #6, -(a2)                ;5C 62
          addq.w #7, -(a1)                ;5E 61
          addq.w #8, -(a0)                ;50 60

          addq #1, -(a7)                ;52 67
          addq #2, -(a6)                ;54 66
          addq #3, -(a5)                ;56 65
          addq #4, -(a4)                ;58 64
          addq #5, -(a3)                ;5A 63
          addq #6, -(a2)                ;5C 62
          addq #7, -(a1)                ;5E 61
          addq #8, -(a0)                ;50 60


          ;addq #$x, ax
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

          addq #1, a7                   ;52 8F
          addq #2, a6                   ;54 8E
          addq #3, a5                   ;56 8D
          addq #4, a4                   ;58 8C
          addq #5, a3                   ;5A 8B
          addq #6, a2                   ;5C 8A
          addq #7, a1                   ;5E 89
          addq #8, a0                   ;50 88


          ;add dx, dx
          add.w d0,d7             ;DE 40
          add.w d1,d6             ;DC 41
          add.w d2,d5             ;DA 42
          add.w d3,d4             ;D8 43
          add.w d4,d3             ;D6 44
          add.w d5,d2             ;D4 45
          add.w d6,d1             ;D2 46
          add.w d7,d0             ;D0 47

          add d0,d7             ;DE 40
          add d1,d6             ;DC 41
          add d2,d5             ;DA 42
          add d3,d4             ;D8 43
          add d4,d3             ;D6 44
          add d5,d2             ;D4 45
          add d6,d1             ;D2 46
          add d7,d0             ;D0 47


          ;add ($x), dx
          add.w ($0001).w, d0     ;D0 78 00 01
          add.w ($00FF).w, d1     ;D2 78 00 FF
          add.w ($0100).w, d2     ;D4 78 01 00
          add.w ($FF00).w, d3     ;D6 78 FF 00
          add.w (LATE_0001).w, d4 ;D8 78 00 01
          add.w (LATE_00FF).w, d5 ;DA 78 00 FF
          add.w (LATE_0100).w, d6 ;DC 78 01 00
          add.w (LATE_FF00).w, d7 ;DE 78 FF 00

          add.w ($0001), d0     ;D0 78 00 01
          add.w ($00FF), d1     ;D2 78 00 FF
          add.w ($0100), d2     ;D4 78 01 00
          add.w ($FF00), d3     ;D6 78 FF 00
          add.w (LATE_0001), d4 ;D8 78 00 01
          add.w (LATE_00FF), d5 ;DA 78 00 FF
          add.w (LATE_0100), d6 ;DC 78 01 00
          add.w (LATE_FF00), d7 ;DE 78 FF 00

          add ($0001).w, d0     ;D0 78 00 01
          add ($00FF).w, d1     ;D2 78 00 FF
          add ($0100).w, d2     ;D4 78 01 00
          add ($FF00).w, d3     ;D6 78 FF 00
          add (LATE_0001).w, d4 ;D8 78 00 01
          add (LATE_00FF).w, d5 ;DA 78 00 FF
          add (LATE_0100).w, d6 ;DC 78 01 00
          add (LATE_FF00).w, d7 ;DE 78 FF 00

          add ($0001), d0     ;D0 78 00 01
          add ($00FF), d1     ;D2 78 00 FF
          add ($0100), d2     ;D4 78 01 00
          add ($FF00), d3     ;D6 78 FF 00
          add (LATE_0001), d4 ;D8 78 00 01
          add (LATE_00FF), d5 ;DA 78 00 FF
          add (LATE_0100), d6 ;DC 78 01 00
          add (LATE_FF00), d7 ;DE 78 FF 00


          ;add (ax), dx
          add.w (a7), d0          ;D0 57
          add.w (a6), d1          ;D2 56
          add.w (a5), d2          ;D4 55
          add.w (a4), d3          ;D6 54
          add.w (a3), d4          ;D8 53
          add.w (a2), d5          ;DA 52
          add.w (a1), d6          ;DC 51
          add.w (a0), d7          ;DE 50

          add (a7), d0          ;D0 57
          add (a6), d1          ;D2 56
          add (a5), d2          ;D4 55
          add (a4), d3          ;D6 54
          add (a3), d4          ;D8 53
          add (a2), d5          ;DA 52
          add (a1), d6          ;DC 51
          add (a0), d7          ;DE 50


          ;add $x(ax), dx
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

          add $01(a0),d7        ;DE 28 00 01
          add $02(a1),d6        ;DC 29 00 02
          add $FE(a2),d5        ;DA 2A 00 FE
          add $FF(a3),d4        ;D8 2B 00 FF
          add LATE_01(a4),d3    ;D6 2C 00 01
          add LATE_02(a5),d2    ;D4 2D 00 02
          add LATE_FE(a6),d1    ;D2 2E 00 FE
          add LATE_FF(a7),d0    ;D0 2F 00 FF


          ;add (ax)+, dx
          add.w (a0)+,d7          ;DE 58
          add.w (a1)+,d6          ;DC 59
          add.w (a2)+,d5          ;DA 5A
          add.w (a3)+,d4          ;D8 5B
          add.w (a4)+,d3          ;D6 5C
          add.w (a5)+,d2          ;D4 5D
          add.w (a6)+,d1          ;D2 5E
          add.w (a7)+,d0          ;D0 5F

          add (a0)+,d7          ;DE 58
          add (a1)+,d6          ;DC 59
          add (a2)+,d5          ;DA 5A
          add (a3)+,d4          ;D8 5B
          add (a4)+,d3          ;D6 5C
          add (a5)+,d2          ;D4 5D
          add (a6)+,d1          ;D2 5E
          add (a7)+,d0          ;D0 5F

          ;add -(ax), dx
          add.w -(a0),d7          ;DE 60
          add.w -(a1),d6          ;DC 61
          add.w -(a2),d5          ;DA 62
          add.w -(a3),d4          ;D8 63
          add.w -(a4),d3          ;D6 64
          add.w -(a5),d2          ;D4 65
          add.w -(a6),d1          ;D2 66
          add.w -(a7),d0          ;D0 67

          add -(a0),d7          ;DE 60
          add -(a1),d6          ;DC 61
          add -(a2),d5          ;DA 62
          add -(a3),d4          ;D8 63
          add -(a4),d3          ;D6 64
          add -(a5),d2          ;D4 65
          add -(a6),d1          ;D2 66
          add -(a7),d0          ;D0 67


          ;add ax, dx
          add.l a0,d7             ;DE 88
          add.l a1,d6             ;DC 89
          add.l a2,d5             ;DA 8A
          add.l a3,d4             ;D8 8B
          add.l a4,d3             ;D6 8C
          add.l a5,d2             ;D4 8D
          add.l a6,d1             ;D2 8E
          add.l a7,d0             ;D0 8F

          add a0,d7             ;DE 88
          add a1,d6             ;DC 89
          add a2,d5             ;DA 8A
          add a3,d4             ;D8 8B
          add a4,d3             ;D6 8C
          add a5,d2             ;D4 8D
          add a6,d1             ;D2 8E
          add a7,d0             ;D0 8F

          add.w a0,d7             ;DE 88
          add.w a1,d6             ;DC 89
          add.w a2,d5             ;DA 8A
          add.w a3,d4             ;D8 8B
          add.w a4,d3             ;D6 8C
          add.w a5,d2             ;D4 8D
          add.w a6,d1             ;D2 8E
          add.w a7,d0             ;D0 8F

          ;add #$x, dx
          add.l #1, d7            ;52 87
          add.l #2, d6            ;54 86
          add.l #3, d5            ;56 85
          add.l #4, d4            ;58 84
          add.l #5, d3            ;5A 83
          add.l #6, d2            ;5C 82
          add.l #7, d1            ;5E 81
          add.l #8, d0            ;50 80

          add.w #1, d7            ;52 47
          add.w #2, d6            ;54 46
          add.w #3, d5            ;56 45
          add.w #4, d4            ;58 44
          add.w #5, d3            ;5A 43
          add.w #6, d2            ;5C 42
          add.w #7, d1            ;5E 41
          add.w #8, d0            ;50 40

          add.b #1, d7            ;52 07
          add.b #2, d6            ;54 06
          add.b #3, d5            ;56 05
          add.b #4, d4            ;58 04
          add.b #5, d3            ;5A 03
          add.b #6, d2            ;5C 02
          add.b #7, d1            ;5E 01
          add.b #8, d0            ;50 00

          add #1, d7            ;52 87
          add #2, d6            ;54 86
          add #3, d5            ;56 85
          add #4, d4            ;58 84
          add #5, d3            ;5A 83
          add #6, d2            ;5C 82
          add #7, d1            ;5E 81
          add #8, d0            ;50 80


          ;add dx, ($x)
          add.w d0, ($0001).w     ;D1 78 00 01
          add.w d1, ($00FF).w     ;D3 78 00 FF
          add.w d2, ($0100).w     ;D5 78 01 00
          add.w d3, ($FF00).w     ;D7 78 FF 00
          add.w d4, (LATE_0001).w ;D9 78 00 01
          add.w d5, (LATE_00FF).w ;DB 78 00 FF
          add.w d6, (LATE_0100).w ;DD 78 01 00
          add.w d7, (LATE_FF00).w ;DF 78 FF 00

          add.w d0, ($0001)     ;D1 78 00 01
          add.w d1, ($00FF)     ;D3 78 00 FF
          add.w d2, ($0100)     ;D5 78 01 00
          add.w d3, ($FF00)     ;D7 78 FF 00
          add.w d4, (LATE_0001) ;D9 78 00 01
          add.w d5, (LATE_00FF) ;DB 78 00 FF
          add.w d6, (LATE_0100) ;DD 78 01 00
          add.w d7, (LATE_FF00) ;DF 78 FF 00

          add d0, ($0001).w     ;D1 78 00 01
          add d1, ($00FF).w     ;D3 78 00 FF
          add d2, ($0100).w     ;D5 78 01 00
          add d3, ($FF00).w     ;D7 78 FF 00
          add d4, (LATE_0001).w ;D9 78 00 01
          add d5, (LATE_00FF).w ;DB 78 00 FF
          add d6, (LATE_0100).w ;DD 78 01 00
          add d7, (LATE_FF00).w ;DF 78 FF 00

          add d0, ($0001)     ;D1 78 00 01
          add d1, ($00FF)     ;D3 78 00 FF
          add d2, ($0100)     ;D5 78 01 00
          add d3, ($FF00)     ;D7 78 FF 00
          add d4, (LATE_0001) ;D9 78 00 01
          add d5, (LATE_00FF) ;DB 78 00 FF
          add d6, (LATE_0100) ;DD 78 01 00
          add d7, (LATE_FF00) ;DF 78 FF 00


          ;add dx, (ax)
          add.w d0,(a7)           ;D1 57
          add.w d1,(a6)           ;D3 56
          add.w d2,(a5)           ;D5 55
          add.w d3,(a4)           ;D7 54
          add.w d4,(a3)           ;D9 53
          add.w d5,(a2)           ;DB 52
          add.w d6,(a1)           ;DD 51
          add.w d7,(a0)           ;DF 50

          add d0,(a7)           ;D1 57
          add d1,(a6)           ;D3 56
          add d2,(a5)           ;D5 55
          add d3,(a4)           ;D7 54
          add d4,(a3)           ;D9 53
          add d5,(a2)           ;DB 52
          add d6,(a1)           ;DD 51
          add d7,(a0)           ;DF 50


          ;add dx, $x(ax)
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

          add d0, $01(a7)       ;D1 AF 00 01
          add d1, $02(a6)       ;D3 AE 00 02
          add d2, $FE(a5)       ;D5 AD 00 FE
          add d3, $FF(a4)       ;D7 AC 00 FF
          add d4, LATE_01(a3)   ;D9 AB 00 01
          add d5, LATE_02(a2)   ;DB AA 00 02
          add d6, LATE_FE(a1)   ;DD A9 00 FE
          add d7, LATE_FF(a0)   ;DF A8 00 FF


          ;add dx, (ax)+
          add.l d0,(a7)+          ;D1 9F
          add.l d1,(a6)+          ;D3 9E
          add.l d2,(a5)+          ;D5 9D
          add.l d3,(a4)+          ;D7 9C
          add.l d4,(a3)+          ;D9 9B
          add.l d5,(a2)+          ;DB 9A
          add.l d6,(a1)+          ;DD 99
          add.l d7,(a0)+          ;DF 98

          add d0,(a7)+          ;D1 9F
          add d1,(a6)+          ;D3 9E
          add d2,(a5)+          ;D5 9D
          add d3,(a4)+          ;D7 9C
          add d4,(a3)+          ;D9 9B
          add d5,(a2)+          ;DB 9A
          add d6,(a1)+          ;DD 99
          add d7,(a0)+          ;DF 98

          ;add dx, -(ax)
          add.w d0,-(a7)          ;D1 67
          add.w d1,-(a6)          ;D3 66
          add.w d2,-(a5)          ;D5 65
          add.w d3,-(a4)          ;D7 64
          add.w d4,-(a3)          ;D9 63
          add.w d5,-(a2)          ;DB 62
          add.w d6,-(a1)          ;DD 61
          add.w d7,-(a0)          ;DF 60

          add d0,-(a7)          ;D1 67
          add d1,-(a6)          ;D3 66
          add d2,-(a5)          ;D5 65
          add d3,-(a4)          ;D7 64
          add d4,-(a3)          ;D9 63
          add d5,-(a2)          ;DB 62
          add d6,-(a1)          ;DD 61
          add d7,-(a0)          ;DF 60


          ;andi #$x, dx
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

          andi #$0001, d7       ;02 47 00 01
          andi #$00FF, d6       ;02 46 00 FF
          andi #$0100, d5       ;02 45 01 00
          andi #$FF00, d4       ;02 44 FF 00
          andi #LATE_0001, d3   ;02 43 00 01
          andi #LATE_00FF, d2   ;02 42 00 FF
          andi #LATE_0100, d1   ;02 41 01 00
          andi #LATE_FF00, d0   ;02 40 FF 00


          ;andi #$x, ($x)
          andi.b #$01, ($0001).w          ;02 38 00 01 00 01
          andi.b #$02, ($00FF).w          ;02 38 00 02 00 FF
          andi.b #$FE, (LATE_0100).w      ;02 38 00 FE 01 00
          andi.b #$FF, (LATE_FF00).w      ;02 38 00 FF FF 00
          andi.b #LATE_01, ($0001).w      ;02 38 00 01 00 01
          andi.b #LATE_02, ($00FF).w      ;02 38 00 02 00 FF
          andi.b #LATE_FE, (LATE_0100).w  ;02 38 00 FE 01 00
          andi.b #LATE_FF, (LATE_FF00).w  ;02 38 00 FF FF 00

          andi.b #$01, ($0001)          ;02 38 00 01 00 01
          andi.b #$02, ($00FF)          ;02 38 00 02 00 FF
          andi.b #$FE, (LATE_0100)      ;02 38 00 FE 01 00
          andi.b #$FF, (LATE_FF00)      ;02 38 00 FF FF 00
          andi.b #LATE_01, ($0001)      ;02 38 00 01 00 01
          andi.b #LATE_02, ($00FF)      ;02 38 00 02 00 FF
          andi.b #LATE_FE, (LATE_0100)  ;02 38 00 FE 01 00
          andi.b #LATE_FF, (LATE_FF00)  ;02 38 00 FF FF 00

          andi.w #$0001, ($0001).w          ;02 78 00 01 00 01
          andi.w #$00FF, ($00FF).w          ;02 78 00 FF 00 FF
          andi.w #$0100, (LATE_0100).w      ;02 78 01 00 01 00
          andi.w #$FF00, (LATE_FF00).w      ;02 78 FF 00 FF 00
          andi.w #LATE_0001, ($0001).w      ;02 78 00 01 00 01
          andi.w #LATE_00FF, ($00FF).w      ;02 78 00 FF 00 FF
          andi.w #LATE_0100, (LATE_0100).w  ;02 78 01 00 01 00
          andi.w #LATE_FF00, (LATE_FF00).w  ;02 78 FF 00 FF 00

          andi.w #$0001, ($0001)          ;02 78 00 01 00 01
          andi.w #$00FF, ($00FF)          ;02 78 00 FF 00 FF
          andi.w #$0100, (LATE_0100)      ;02 78 01 00 01 00
          andi.w #$FF00, (LATE_FF00)      ;02 78 FF 00 FF 00
          andi.w #LATE_0001, ($0001)      ;02 78 00 01 00 01
          andi.w #LATE_00FF, ($00FF)      ;02 78 00 FF 00 FF
          andi.w #LATE_0100, (LATE_0100)  ;02 78 01 00 01 00
          andi.w #LATE_FF00, (LATE_FF00)  ;02 78 FF 00 FF 00

          andi #$0001, ($0001)          ;02 78 00 01 00 01
          andi #$00FF, ($00FF)          ;02 78 00 FF 00 FF
          andi #$0100, (LATE_0100)      ;02 78 01 00 01 00
          andi #$FF00, (LATE_FF00)      ;02 78 FF 00 FF 00
          andi #LATE_0001, ($0001)      ;02 78 00 01 00 01
          andi #LATE_00FF, ($00FF)      ;02 78 00 FF 00 FF
          andi #LATE_0100, (LATE_0100)  ;02 78 01 00 01 00
          andi #LATE_FF00, (LATE_FF00)  ;02 78 FF 00 FF 00


          ;andi #$x, (ax)
          andi.b #$01, (a7)       ;02 17 00 01
          andi.b #$02, (a6)       ;02 16 00 02
          andi.b #$FE, (a5)       ;02 15 00 FE
          andi.b #$FF, (a4)       ;02 14 00 FF
          andi.b #LATE_01, (a3)   ;02 13 00 01
          andi.b #LATE_02, (a2)   ;02 12 00 02
          andi.b #LATE_FE, (a1)   ;02 11 00 FE
          andi.b #LATE_FF, (a0)   ;02 10 00 FF

          andi #$01, (a7)       ;02 17 00 01
          andi #$02, (a6)       ;02 16 00 02
          andi #$FE, (a5)       ;02 15 00 FE
          andi #$FF, (a4)       ;02 14 00 FF
          andi #LATE_01, (a3)   ;02 13 00 01
          andi #LATE_02, (a2)   ;02 12 00 02
          andi #LATE_FE, (a1)   ;02 11 00 FE
          andi #LATE_FF, (a0)   ;02 10 00 FF


          ;andi #$x, $x(ax)
          andi.b #$01, $ff(a7)           ;02 2F 00 01 00 FF
          andi.b #$02, $fe(a6)           ;02 2E 00 02 00 FE
          andi.b #$FE, $02(a5)           ;02 2D 00 FE 00 02
          andi.b #$FF, $01(a4)           ;02 2C 00 FF 00 01
          andi.b #LATE_01, LATE_FF(a3)   ;02 2B 00 01 00 FF
          andi.b #LATE_02, LATE_FE(a2)   ;02 2A 00 02 00 FE
          andi.b #LATE_FE, LATE_02(a1)   ;02 29 00 FE 00 02
          andi.b #LATE_FF, LATE_01(a0)   ;02 28 00 FF 00 01


          ;andi #$x, (ax)+
          andi.b #$01, (a7)+      ;02 1F 00 01
          andi.b #$02, (a6)+      ;02 1E 00 02
          andi.b #$FE, (a5)+      ;02 1D 00 FE
          andi.b #$FF, (a4)+      ;02 1C 00 FF
          andi.b #LATE_01, (a3)+  ;02 1B 00 01
          andi.b #LATE_02, (a2)+  ;02 1A 00 02
          andi.b #LATE_FE, (a1)+  ;02 19 00 FE
          andi.b #LATE_FF, (a0)+  ;02 18 00 FF


          ;andi #$x, -(ax)
          andi.b #$01, -(a7)      ;02 27 00 01
          andi.b #$02, -(a6)      ;02 26 00 02
          andi.b #$FE, -(a5)      ;02 25 00 FE
          andi.b #$FF, -(a4)      ;02 24 00 FF
          andi.b #LATE_01, -(a3)  ;02 23 00 01
          andi.b #LATE_02, -(a2)  ;02 22 00 02
          andi.b #LATE_FE, -(a1)  ;02 21 00 FE
          andi.b #LATE_FF, -(a0)  ;02 20 00 FF


          ;andi #$x, $x(ax)
          andi.w #$0001, $ff(a7)           ;02 6F 00 01 00 FF
          andi.w #$00FF, $fe(a6)           ;02 6E 00 FF 00 FE
          andi.w #$0100, $02(a5)           ;02 6D 01 00 00 02
          andi.w #$FF00, $01(a4)           ;02 6C FF 00 00 01
          andi.w #LATE_0001, LATE_FF(a3)   ;02 6B 00 01 00 FF
          andi.w #LATE_00FF, LATE_FE(a2)   ;02 6A 00 FF 00 FE
          andi.w #LATE_0100, LATE_02(a1)   ;02 69 01 00 00 02
          andi.w #LATE_FF00, LATE_01(a0)   ;02 68 FF 00 00 01


          ;andi #$x, (ax, dx)
          andi.b #$01,(a0,d7.w)       ;02 30 00 01 70 00
          andi.b #$FF,(a1,d6.w)       ;02 31 00 FF 60 00
          andi.b #LATE_01,(a2,d5.w)   ;02 32 00 01 50 00
          andi.b #LATE_FF,(a3,d4.w)   ;02 33 00 FF 40 00
          andi.b #$01,(a4,d3.w)       ;02 34 00 01 30 00
          andi.b #$FF,(a5,d2.w)       ;02 35 00 FF 20 00
          andi.b #LATE_01,(a6,d1.w)   ;02 36 00 01 10 00
          andi.b #LATE_FF,(a7,d0.w)   ;02 37 00 FF 00 00

          andi.w #$0001, $7F(a0,d7.w)           ;02 70 00 01 70 7F
          andi.w #$00FF, $01(a1,d6.w)           ;02 71 00 FF 60 01
          andi.w #$0100, LATE_7F(a2,d5.w)       ;02 72 01 00 50 7F
          andi.w #$FF00, LATE_01(a3,d4.w)       ;02 73 FF 00 40 01
          andi.w #LATE_0001, $7F(a4,d3.w)       ;02 74 00 01 30 7F
          andi.w #LATE_00FF, $01(a5,d2.w)       ;02 75 00 FF 20 01
          andi.w #LATE_0100, LATE_7F(a6,d1.w)   ;02 76 01 00 10 7F
          andi.w #LATE_FF00, LATE_01(a7,d0.w)   ;02 77 FF 00 00 01

          andi.b #$01,(a0,d7)       ;02 30 00 01 70 00
          andi.b #$FF,(a1,d6)       ;02 31 00 FF 60 00
          andi.b #LATE_01,(a2,d5)   ;02 32 00 01 50 00
          andi.b #LATE_FF,(a3,d4)   ;02 33 00 FF 40 00
          andi.b #$01,(a4,d3)       ;02 34 00 01 30 00
          andi.b #$FF,(a5,d2)       ;02 35 00 FF 20 00
          andi.b #LATE_01,(a6,d1)   ;02 36 00 01 10 00
          andi.b #LATE_FF,(a7,d0)   ;02 37 00 FF 00 00

          andi #$01,(a0,d7.w)       ;02 30 00 01 70 00
          andi #$FF,(a1,d6.w)       ;02 31 00 FF 60 00
          andi #LATE_01,(a2,d5.w)   ;02 32 00 01 50 00
          andi #LATE_FF,(a3,d4.w)   ;02 33 00 FF 40 00
          andi #$01,(a4,d3.w)       ;02 34 00 01 30 00
          andi #$FF,(a5,d2.w)       ;02 35 00 FF 20 00
          andi #LATE_01,(a6,d1.w)   ;02 36 00 01 10 00
          andi #LATE_FF,(a7,d0.w)   ;02 37 00 FF 00 00


          ;andi #$x, sr
          andi #$0001,sr      ;02 7C 00 01
          andi #$00FF,sr      ;02 7C 00 FF
          andi #$0100,sr      ;02 7C 01 00
          andi #$FF00,sr      ;02 7C FF 00
          andi #LATE_0001,sr  ;02 7C 00 01
          andi #LATE_00FF,sr  ;02 7C 00 FF
          andi #LATE_0100,sr  ;02 7C 01 00
          andi #LATE_FF00,sr  ;02 7C FF 00


          ;and dx, dx
          and.b d0,d7             ;CE 00
          and.b d1,d6             ;CC 01
          and.b d2,d5             ;CA 02
          and.b d3,d4             ;C8 03
          and.b d4,d3             ;C6 04
          and.b d5,d2             ;C4 05
          and.b d6,d1             ;C2 06
          and.b d7,d0             ;C0 07

          and d0,d7             ;CE 00
          and d1,d6             ;CC 01
          and d2,d5             ;CA 02
          and d3,d4             ;C8 03
          and d4,d3             ;C6 04
          and d5,d2             ;C4 05
          and d6,d1             ;C2 06
          and d7,d0             ;C0 07


          ;and ($x), dx
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

          and.b ($0001), d7       ;CE 38 00 01
          and.b ($00FF), d6       ;CC 38 00 FF
          and.b ($0100), d5       ;CA 38 01 00
          and.b ($FF00), d4       ;C8 38 FF 00
          and.b (LATE_0001), d3   ;C6 38 00 01
          and.b (LATE_00FF), d2   ;C4 38 00 FF
          and.b (LATE_0100), d1   ;C2 38 01 00
          and.b (LATE_FF00), d0   ;C0 38 FF 00

          and.w ($0001), d7       ;CE 78 00 01
          and.w ($00FF), d6       ;CC 78 00 FF
          and.w ($0100), d5       ;CA 78 01 00
          and.w ($FF00), d4       ;C8 78 FF 00
          and.w (LATE_0001), d3   ;C6 78 00 01
          and.w (LATE_00FF), d2   ;C4 78 00 FF
          and.w (LATE_0100), d1   ;C2 78 01 00
          and.w (LATE_FF00), d0   ;C0 78 FF 00

          and ($0001), d7       ;CE 78 00 01
          and ($00FF), d6       ;CC 78 00 FF
          and ($0100), d5       ;CA 78 01 00
          and ($FF00), d4       ;C8 78 FF 00
          and (LATE_0001), d3   ;C6 78 00 01
          and (LATE_00FF), d2   ;C4 78 00 FF
          and (LATE_0100), d1   ;C2 78 01 00
          and (LATE_FF00), d0   ;C0 78 FF 00

          and ($0001).w, d7       ;CE 78 00 01
          and ($00FF).w, d6       ;CC 78 00 FF
          and ($0100).w, d5       ;CA 78 01 00
          and ($FF00).w, d4       ;C8 78 FF 00
          and (LATE_0001).w, d3   ;C6 78 00 01
          and (LATE_00FF).w, d2   ;C4 78 00 FF
          and (LATE_0100).w, d1   ;C2 78 01 00
          and (LATE_FF00).w, d0   ;C0 78 FF 00


          ;and (ax), dx
          and.w (a7), d0            ;C0 57
          and.w (a6), d1            ;C2 56
          and.w (a5), d2            ;C4 55
          and.w (a4), d3            ;C6 54
          and.w (a3), d4            ;C8 53
          and.w (a2), d5            ;CA 52
          and.w (a1), d6            ;CC 51
          and.w (a0), d7            ;CE 50

          and (a7), d0            ;C0 57
          and (a6), d1            ;C2 56
          and (a5), d2            ;C4 55
          and (a4), d3            ;C6 54
          and (a3), d4            ;C8 53
          and (a2), d5            ;CA 52
          and (a1), d6            ;CC 51
          and (a0), d7            ;CE 50


          ;and $x(ax), dx
          and.w $01(a7), d0         ;C0 6F 00 01
          and.w $02(a6), d1         ;C2 6E 00 02
          and.w $FE(a5), d2         ;C4 6D 00 FE
          and.w $FF(a4), d3         ;C6 6C 00 FF
          and.w LATE_01(a3), d4     ;C8 6B 00 01
          and.w LATE_02(a2), d5     ;CA 6A 00 02
          and.w LATE_FE(a1), d6     ;CC 69 00 FE
          and.w LATE_FF(a0), d7     ;CE 68 00 FF

          and $01(a7), d0         ;C0 6F 00 01
          and $02(a6), d1         ;C2 6E 00 02
          and $FE(a5), d2         ;C4 6D 00 FE
          and $FF(a4), d3         ;C6 6C 00 FF
          and LATE_01(a3), d4     ;C8 6B 00 01
          and LATE_02(a2), d5     ;CA 6A 00 02
          and LATE_FE(a1), d6     ;CC 69 00 FE
          and LATE_FF(a0), d7     ;CE 68 00 FF


          ;and (ax)+, dx
          and.w (a7)+, d0           ;C0 5F
          and.w (a6)+, d1           ;C2 5E
          and.w (a5)+, d2           ;C4 5D
          and.w (a4)+, d3           ;C6 5C
          and.w (a3)+, d4           ;C8 5B
          and.w (a2)+, d5           ;CA 5A
          and.w (a1)+, d6           ;CC 59
          and.w (a0)+, d7           ;CE 58

          and (a7)+, d0           ;C0 5F
          and (a6)+, d1           ;C2 5E
          and (a5)+, d2           ;C4 5D
          and (a4)+, d3           ;C6 5C
          and (a3)+, d4           ;C8 5B
          and (a2)+, d5           ;CA 5A
          and (a1)+, d6           ;CC 59
          and (a0)+, d7           ;CE 58


          ;and -(ax), dx
          and.w -(a7), d0           ;C0 67
          and.w -(a6), d1           ;C2 66
          and.w -(a5), d2           ;C4 65
          and.w -(a4), d3           ;C6 64
          and.w -(a3), d4           ;C8 63
          and.w -(a2), d5           ;CA 62
          and.w -(a1), d6           ;CC 61
          and.w -(a0), d7           ;CE 60

          and -(a7), d0           ;C0 67
          and -(a6), d1           ;C2 66
          and -(a5), d2           ;C4 65
          and -(a4), d3           ;C6 64
          and -(a3), d4           ;C8 63
          and -(a2), d5           ;CA 62
          and -(a1), d6           ;CC 61
          and -(a0), d7           ;CE 60


          ;and dx, ($x)
          and.w d0, ($0001).w       ;C1 78 00 01
          and.w d1, ($00FF).w       ;C3 78 00 FF
          and.w d2, ($0100).w       ;C5 78 01 00
          and.w d3, ($FF00).w       ;C7 78 FF 00
          and.w d4, (LATE_0001).w   ;C9 78 00 01
          and.w d5, (LATE_00FF).w   ;CB 78 00 FF
          and.w d6, (LATE_0100).w   ;CD 78 01 00
          and.w d7, (LATE_FF00).w   ;CF 78 FF 00

          and.w d0, ($0001)       ;C1 78 00 01
          and.w d1, ($00FF)       ;C3 78 00 FF
          and.w d2, ($0100)       ;C5 78 01 00
          and.w d3, ($FF00)       ;C7 78 FF 00
          and.w d4, (LATE_0001)   ;C9 78 00 01
          and.w d5, (LATE_00FF)   ;CB 78 00 FF
          and.w d6, (LATE_0100)   ;CD 78 01 00
          and.w d7, (LATE_FF00)   ;CF 78 FF 00

          and d0, ($0001).w       ;C1 78 00 01
          and d1, ($00FF).w       ;C3 78 00 FF
          and d2, ($0100).w       ;C5 78 01 00
          and d3, ($FF00).w       ;C7 78 FF 00
          and d4, (LATE_0001).w   ;C9 78 00 01
          and d5, (LATE_00FF).w   ;CB 78 00 FF
          and d6, (LATE_0100).w   ;CD 78 01 00
          and d7, (LATE_FF00).w   ;CF 78 FF 00

          and d0, ($0001)       ;C1 78 00 01
          and d1, ($00FF)       ;C3 78 00 FF
          and d2, ($0100)       ;C5 78 01 00
          and d3, ($FF00)       ;C7 78 FF 00
          and d4, (LATE_0001)   ;C9 78 00 01
          and d5, (LATE_00FF)   ;CB 78 00 FF
          and d6, (LATE_0100)   ;CD 78 01 00
          and d7, (LATE_FF00)   ;CF 78 FF 00


          ;and dx, (ax)
          and.w d0,(a7)             ;C1 57
          and.w d1,(a6)             ;C3 56
          and.w d2,(a5)             ;C5 55
          and.w d3,(a4)             ;C7 54
          and.w d4,(a3)             ;C9 53
          and.w d5,(a2)             ;CB 52
          and.w d6,(a1)             ;CD 51
          and.w d7,(a0)             ;CF 50

          and d0,(a7)             ;C1 57
          and d1,(a6)             ;C3 56
          and d2,(a5)             ;C5 55
          and d3,(a4)             ;C7 54
          and d4,(a3)             ;C9 53
          and d5,(a2)             ;CB 52
          and d6,(a1)             ;CD 51
          and d7,(a0)             ;CF 50


          ;and dx, $x(ax)
          and.w d0, $01(a7)         ;C1 6F 00 01
          and.w d1, $02(a6)         ;C3 6E 00 02
          and.w d2, $FE(a5)         ;C5 6D 00 FE
          and.w d3, $FF(a4)         ;C7 6C 00 FF
          and.w d4, LATE_01(a3)     ;C9 6B 00 01
          and.w d5, LATE_02(a2)     ;CB 6A 00 02
          and.w d6, LATE_FE(a1)     ;CD 69 00 FE
          and.w d7, LATE_FF(a0)     ;CF 68 00 FF

          and d0, $01(a7)         ;C1 6F 00 01
          and d1, $02(a6)         ;C3 6E 00 02
          and d2, $FE(a5)         ;C5 6D 00 FE
          and d3, $FF(a4)         ;C7 6C 00 FF
          and d4, LATE_01(a3)     ;C9 6B 00 01
          and d5, LATE_02(a2)     ;CB 6A 00 02
          and d6, LATE_FE(a1)     ;CD 69 00 FE
          and d7, LATE_FF(a0)     ;CF 68 00 FF


          ;and dx, (ax)+
          and.w d0,(a7)+            ;C1 5F
          and.w d1,(a6)+            ;C3 5E
          and.w d2,(a5)+            ;C5 5D
          and.w d3,(a4)+            ;C7 5C
          and.w d4,(a3)+            ;C9 5B
          and.w d5,(a2)+            ;CB 5A
          and.w d6,(a1)+            ;CD 59
          and.w d7,(a0)+            ;CF 58

          and d0,(a7)+            ;C1 5F
          and d1,(a6)+            ;C3 5E
          and d2,(a5)+            ;C5 5D
          and d3,(a4)+            ;C7 5C
          and d4,(a3)+            ;C9 5B
          and d5,(a2)+            ;CB 5A
          and d6,(a1)+            ;CD 59
          and d7,(a0)+            ;CF 58


          ;and dx, -(ax)
          and.w d0,-(a7)            ;C1 67
          and.w d1,-(a6)            ;C3 66
          and.w d2,-(a5)            ;C5 65
          and.w d3,-(a4)            ;C7 64
          and.w d4,-(a3)            ;C9 63
          and.w d5,-(a2)            ;CB 62
          and.w d6,-(a1)            ;CD 61
          and.w d7,-(a0)            ;CF 60

          and d0,-(a7)            ;C1 67
          and d1,-(a6)            ;C3 66
          and d2,-(a5)            ;C5 65
          and d3,-(a4)            ;C7 64
          and d4,-(a3)            ;C9 63
          and d5,-(a2)            ;CB 62
          and d6,-(a1)            ;CD 61
          and d7,-(a0)            ;CF 60



-
          bra.s -                  ;60 FE
          bra.s +                  ;60 00
+

-
          bra.w -                  ;60 00 FE FE
          bra.w +                  ;60 00 00 02
+

-
          bra -                  ;60 FE
          bra +                  ;60 00
+

-
          bsr.s -                  ;61 FE
          bsr.s +                  ;61 00
+
-
          bsr -                  ;61 FE
          bsr +                  ;61 00
+
-
          bhi.s -                  ;62 FE
          bhi.s +                  ;62 00
+
-
          bhi -                  ;62 FE
          bhi +                  ;62 00
+
-
          bls.s -                  ;63 FE
          bls.s +                  ;63 00
+
-
          bls -                  ;63 FE
          bls +                  ;63 00
+
-
          bcc.s -                  ;64 FE
          bcc.s +                  ;64 00
+
-
          bcc -                  ;64 FE
          bcc +                  ;64 00
+
-
          bcs.s -                  ;65 FE
          bcs.s +                  ;65 00
+
-
          bcs -                  ;65 FE
          bcs +                  ;65 00
+
-
          bne.s -                  ;66 FE
          bne.s +                  ;66 00
+
-
          bne -                  ;66 FE
          bne +                  ;66 00
+
-
          beq.s -                  ;67 FE
          beq.s +                  ;67 00
+
-
          beq -                  ;67 FE
          beq +                  ;67 00
+
-
          bvc.s -                  ;68 FE
          bvc.s +                  ;68 00
+
-
          bvc -                  ;68 FE
          bvc +                  ;68 00
+
-
          bvs.s -                  ;69 FE
          bvs.s +                  ;69 00
+
-
          bvs -                  ;69 FE
          bvs +                  ;69 00
+
-
          bpl.s -                  ;6A FE
          bpl.s +                  ;6A 00
+
-
          bpl -                  ;6A FE
          bpl +                  ;6A 00
+
-
          bmi.s -                  ;6B FE
          bmi.s +                  ;6B 00
+
-
          bmi -                  ;6B FE
          bmi +                  ;6B 00
+
-
          bge.s -                  ;6C FE
          bge.s +                  ;6C 00
+
-
          bge -                  ;6C FE
          bge +                  ;6C 00
+
-
          blt.s -                  ;6D FE
          blt.s +                  ;6D 00
+
-
          blt -                  ;6D FE
          blt +                  ;6D 00
+
-
          bgt.s -                  ;6E FE
          bgt.s +                  ;6E 00
+
-
          bgt -                  ;6E FE
          bgt +                  ;6E 00
+
-
          ble.s -                  ;6F FE
          ble.s +                  ;6F 00
+
-
          ble -                  ;6F FE
          ble +                  ;6F 00
+


          btst #0, ($FF00).w        ;08 38 00 00 FF 00
          btst #1, ($0100).w        ;08 38 00 01 01 00
          btst #2, ($00FF).w        ;08 38 00 02 00 FF
          btst #3, ($0001).w        ;08 38 00 03 00 01
          btst #4, (LATE_FF00).w    ;08 38 00 04 FF 00
          btst #5, (LATE_0100).w    ;08 38 00 05 01 00
          btst #6, (LATE_00FF).w    ;08 38 00 06 00 FF
          btst #7, (LATE_0001).w    ;08 38 00 07 00 01

          btst #0, ($FF00)        ;08 38 00 00 FF 00
          btst #1, ($0100)        ;08 38 00 01 01 00
          btst #2, ($00FF)        ;08 38 00 02 00 FF
          btst #3, ($0001)        ;08 38 00 03 00 01
          btst #4, (LATE_FF00)    ;08 38 00 04 FF 00
          btst #5, (LATE_0100)    ;08 38 00 05 01 00
          btst #6, (LATE_00FF)    ;08 38 00 06 00 FF
          btst #7, (LATE_0001)    ;08 38 00 07 00 01

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

          btst d0, ($FF00)        ;01 38 FF 00
          btst d1, ($0100)        ;03 38 01 00
          btst d2, ($00FF)        ;05 38 00 FF
          btst d3, ($0001)        ;07 38 00 01
          btst d4, (LATE_FF00)    ;09 38 FF 00
          btst d5, (LATE_0100)    ;0B 38 01 00
          btst d6, (LATE_00FF)    ;0D 38 00 FF
          btst d7, (LATE_0001)    ;0F 38 00 01

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

          btst #0, $000001          ;08 39 00 00 00 00 00 01
          btst #1, $0000FF          ;08 39 00 01 00 00 00 FF
          btst #2, $000100          ;08 39 00 02 00 00 01 00
          btst #3, $00FF00          ;08 39 00 03 00 00 FF 00
          btst #4, $010000          ;08 39 00 04 00 01 00 00
          btst #5, $FF0000          ;08 39 00 05 00 FF 00 00
          btst #6, $FF00FF          ;08 39 00 06 00 FF 00 FF
          btst #7, $FFFFFF          ;08 39 00 07 00 FF FF FF


          bclr #0, ($FF00).w        ;08 B8 00 00 FF 00
          bclr #1, ($0100).w        ;08 B8 00 01 01 00
          bclr #2, ($00FF).w        ;08 B8 00 02 00 FF
          bclr #3, ($0001).w        ;08 B8 00 03 00 01
          bclr #4, (LATE_FF00).w    ;08 B8 00 04 FF 00
          bclr #5, (LATE_0100).w    ;08 B8 00 05 01 00
          bclr #6, (LATE_00FF).w    ;08 B8 00 06 00 FF
          bclr #7, (LATE_0001).w    ;08 B8 00 07 00 01

          bclr #0, ($FF00)        ;08 B8 00 00 FF 00
          bclr #1, ($0100)        ;08 B8 00 01 01 00
          bclr #2, ($00FF)        ;08 B8 00 02 00 FF
          bclr #3, ($0001)        ;08 B8 00 03 00 01
          bclr #4, (LATE_FF00)    ;08 B8 00 04 FF 00
          bclr #5, (LATE_0100)    ;08 B8 00 05 01 00
          bclr #6, (LATE_00FF)    ;08 B8 00 06 00 FF
          bclr #7, (LATE_0001)    ;08 B8 00 07 00 01

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

          bclr d0, ($FF00)          ;01 B8 FF 00
          bclr d1, ($0100)          ;03 B8 01 00
          bclr d2, ($00FF)          ;05 B8 00 FF
          bclr d3, ($0001)          ;07 B8 00 01
          bclr d4, (LATE_FF00)      ;09 B8 FF 00
          bclr d5, (LATE_0100)      ;0B B8 01 00
          bclr d6, (LATE_00FF)      ;0D B8 00 FF
          bclr d7, (LATE_0001)      ;0F B8 00 01

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

          bset #0, ($FF00)          ;08 F8 00 00 FF 00
          bset #1, ($0100)          ;08 F8 00 01 01 00
          bset #2, ($00FF)          ;08 F8 00 02 00 FF
          bset #3, ($0001)          ;08 F8 00 03 00 01
          bset #4, (LATE_FF00)      ;08 F8 00 04 FF 00
          bset #5, (LATE_0100)      ;08 F8 00 05 01 00
          bset #6, (LATE_00FF)      ;08 F8 00 06 00 FF
          bset #7, (LATE_0001)      ;08 F8 00 07 00 01

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

          bset d0, ($FF00)          ;01 F8 FF 00
          bset d1, ($0100)          ;03 F8 01 00
          bset d2, ($00FF)          ;05 F8 00 FF
          bset d3, ($0001)          ;07 F8 00 01
          bset d4, (LATE_FF00)      ;09 F8 FF 00
          bset d5, (LATE_0100)      ;0B F8 01 00
          bset d6, (LATE_00FF)      ;0D F8 00 FF
          bset d7, (LATE_0001)      ;0F F8 00 01

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

          bchg #0, ($FF00)          ;08 78 00 00 FF 00
          bchg #1, ($0100)          ;08 78 00 01 01 00
          bchg #2, ($00FF)          ;08 78 00 02 00 FF
          bchg #3, ($0001)          ;08 78 00 03 00 01
          bchg #4, (LATE_FF00)      ;08 78 00 04 FF 00
          bchg #5, (LATE_0100)      ;08 78 00 05 01 00
          bchg #6, (LATE_00FF)      ;08 78 00 06 00 FF
          bchg #7, (LATE_0001)      ;08 78 00 07 00 01

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

          bchg d0, ($FF00)          ;01 78 FF 00
          bchg d1, ($0100)          ;03 78 01 00
          bchg d2, ($00FF)          ;05 78 00 FF
          bchg d3, ($0001)          ;07 78 00 01
          bchg d4, (LATE_FF00)      ;09 78 FF 00
          bchg d5, (LATE_0100)      ;0B 78 01 00
          bchg d6, (LATE_00FF)      ;0D 78 00 FF
          bchg d7, (LATE_0001)      ;0F 78 00 01

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

          clr d0                    ;42 40
          clr d1                    ;42 41
          clr d2                    ;42 42
          clr d3                    ;42 43
          clr d4                    ;42 44
          clr d5                    ;42 45
          clr d6                    ;42 46
          clr d7                    ;42 47

          clr.w ($0001).w           ;42 78 00 01
          clr.w ($00FF).w           ;42 78 00 FF
          clr.w ($0100).w           ;42 78 01 00
          clr.w ($FF00).w           ;42 78 FF 00
          clr.w (LATE_0001).w       ;42 78 00 01
          clr.w (LATE_00FF).w       ;42 78 00 FF
          clr.w (LATE_0100).w       ;42 78 01 00
          clr.w (LATE_FF00).w       ;42 78 FF 00

          clr.w ($0001)             ;42 78 00 01
          clr.w ($00FF)             ;42 78 00 FF
          clr.w ($0100)             ;42 78 01 00
          clr.w ($FF00)             ;42 78 FF 00
          clr.w (LATE_0001)         ;42 78 00 01
          clr.w (LATE_00FF)         ;42 78 00 FF
          clr.w (LATE_0100)         ;42 78 01 00
          clr.w (LATE_FF00)         ;42 78 FF 00

          clr ($0001).w             ;42 78 00 01
          clr ($00FF).w             ;42 78 00 FF
          clr ($0100).w             ;42 78 01 00
          clr ($FF00).w             ;42 78 FF 00
          clr (LATE_0001).w         ;42 78 00 01
          clr (LATE_00FF).w         ;42 78 00 FF
          clr (LATE_0100).w         ;42 78 01 00
          clr (LATE_FF00).w         ;42 78 FF 00

          clr ($0001)               ;42 78 00 01
          clr ($00FF)               ;42 78 00 FF
          clr ($0100)               ;42 78 01 00
          clr ($FF00)               ;42 78 FF 00
          clr (LATE_0001)           ;42 78 00 01
          clr (LATE_00FF)           ;42 78 00 FF
          clr (LATE_0100)           ;42 78 01 00
          clr (LATE_FF00)           ;42 78 FF 00


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

          clr $01(a7)               ;42 6F 00 01
          clr $02(a6)               ;42 6E 00 02
          clr $FE(a5)               ;42 6D 00 FE
          clr $FF(a4)               ;42 6C 00 FF
          clr LATE_01(a3)           ;42 6B 00 01
          clr LATE_02(a2)           ;42 6A 00 02
          clr LATE_FE(a1)           ;42 69 00 FE
          clr LATE_FF(a0)           ;42 68 00 FF

          clr.l (a7)                ;42 97
          clr.l (a6)                ;42 96
          clr.l (a5)                ;42 95
          clr.l (a4)                ;42 94
          clr.l (a3)                ;42 93
          clr.l (a2)                ;42 92
          clr.l (a1)                ;42 91
          clr.l (a0)                ;42 90

          clr (a7)                  ;42 97
          clr (a6)                  ;42 96
          clr (a5)                  ;42 95
          clr (a4)                  ;42 94
          clr (a3)                  ;42 93
          clr (a2)                  ;42 92
          clr (a1)                  ;42 91
          clr (a0)                  ;42 90

          clr.l $01(a7)             ;42 AF 00 01
          clr.l $02(a6)             ;42 AE 00 02
          clr.l $FE(a5)             ;42 AD 00 FE
          clr.l $FF(a4)             ;42 AC 00 FF
          clr.l LATE_01(a3)         ;42 AB 00 01
          clr.l LATE_02(a2)         ;42 AA 00 02
          clr.l LATE_FE(a1)         ;42 A9 00 FE
          clr.l LATE_FF(a0)         ;42 A8 00 FF

          clr $01(a7)               ;42 AF 00 01
          clr $02(a6)               ;42 AE 00 02
          clr $FE(a5)               ;42 AD 00 FE
          clr $FF(a4)               ;42 AC 00 FF
          clr LATE_01(a3)           ;42 AB 00 01
          clr LATE_02(a2)           ;42 AA 00 02
          clr LATE_FE(a1)           ;42 A9 00 FE
          clr LATE_FF(a0)           ;42 A8 00 FF

          clr.l (a7)+               ;42 9F
          clr.l (a6)+               ;42 9E
          clr.l (a5)+               ;42 9D
          clr.l (a4)+               ;42 9C
          clr.l (a3)+               ;42 9B
          clr.l (a2)+               ;42 9A
          clr.l (a1)+               ;42 99
          clr.l (a0)+               ;42 98

          clr (a7)+                 ;42 9F
          clr (a6)+                 ;42 9E
          clr (a5)+                 ;42 9D
          clr (a4)+                 ;42 9C
          clr (a3)+                 ;42 9B
          clr (a2)+                 ;42 9A
          clr (a1)+                 ;42 99
          clr (a0)+                 ;42 98

          clr.l -(a7)               ;42 A7
          clr.l -(a6)               ;42 A6
          clr.l -(a5)               ;42 A5
          clr.l -(a4)               ;42 A4
          clr.l -(a3)               ;42 A3
          clr.l -(a2)               ;42 A2
          clr.l -(a1)               ;42 A1
          clr.l -(a0)               ;42 A0

          clr -(a7)                 ;42 A7
          clr -(a6)                 ;42 A6
          clr -(a5)                 ;42 A5
          clr -(a4)                 ;42 A4
          clr -(a3)                 ;42 A3
          clr -(a2)                 ;42 A2
          clr -(a1)                 ;42 A1
          clr -(a0)                 ;42 A0

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

          cmpa #$0001,a7            ;BE FC 00 01
          cmpa #$00FF,a6            ;BC FC 00 FF
          cmpa #$0100,a5            ;BA FC 01 00
          cmpa #$FF00,a4            ;B8 FC FF 00
          cmpa #LATE_0001,a3        ;B6 FC 00 01
          cmpa #LATE_00FF,a2        ;B4 FC 00 FF
          cmpa #LATE_0100,a1        ;B2 FC 01 00
          cmpa #LATE_FF00,a0        ;B0 FC FF 00



          cmpa.l ($0001).w,a7       ;BF F8 00 01 ??
          cmpa.l ($00FF).w,a6       ;BD F8 00 FF ??
          cmpa.l ($0100).w,a5       ;BB F8 01 00 ??
          cmpa.l ($FF00).w,a4       ;B9 F8 FF 00 ??
          cmpa.l (LATE_0001).w,a3   ;B7 F8 00 01 ??
          cmpa.l (LATE_00FF).w,a2   ;B5 F8 00 FF ??
          cmpa.l (LATE_0100).w,a1   ;B3 F8 01 00 ??
          cmpa.l (LATE_FF00).w,a0   ;B1 F8 FF 00 ??



          cmpa.w d7, a0             ;B0 C7
          cmpa.w d6, a1             ;B2 C6
          cmpa.w d5, a2             ;B4 C5
          cmpa.w d4, a3             ;B6 C4
          cmpa.w d3, a4             ;B8 C3
          cmpa.w d2, a5             ;BA C2
          cmpa.w d1, a6             ;BC C1
          cmpa.w d0, a7             ;BE C0

          cmpa d7, a0             ;B0 C7
          cmpa d6, a1             ;B2 C6
          cmpa d5, a2             ;B4 C5
          cmpa d4, a3             ;B6 C4
          cmpa d3, a4             ;B8 C3
          cmpa d2, a5             ;BA C2
          cmpa d1, a6             ;BC C1
          cmpa d0, a7             ;BE C0

          cmpa.l (a7), a0           ;B1 D7
          cmpa.l (a6), a1           ;B3 D6
          cmpa.l (a5), a2           ;B5 D5
          cmpa.l (a4), a3           ;B7 D4
          cmpa.l (a3), a4           ;B9 D3
          cmpa.l (a2), a5           ;BB D2
          cmpa.l (a1), a6           ;BD D1
          cmpa.l (a0), a7           ;BF D0

          cmpa (a7), a0           ;B1 D7
          cmpa (a6), a1           ;B3 D6
          cmpa (a5), a2           ;B5 D5
          cmpa (a4), a3           ;B7 D4
          cmpa (a3), a4           ;B9 D3
          cmpa (a2), a5           ;BB D2
          cmpa (a1), a6           ;BD D1
          cmpa (a0), a7           ;BF D0

          cmpa.l $01(a7), a0        ;B1 EF 00 01
          cmpa.l $02(a6), a1        ;B3 EE 00 02
          cmpa.l $FE(a5), a2        ;B5 ED 00 FE
          cmpa.l $FF(a4), a3        ;B7 EC 00 FF
          cmpa.l LATE_01(a3), a4    ;B9 EB 00 01
          cmpa.l LATE_02(a2), a5    ;BB EA 00 02
          cmpa.l LATE_FE(a1), a6    ;BD E9 00 FE
          cmpa.l LATE_FF(a0), a7    ;BF E8 00 FF

          cmpa $01(a7), a0          ;B1 EF 00 01
          cmpa $02(a6), a1          ;B3 EE 00 02
          cmpa $FE(a5), a2          ;B5 ED 00 FE
          cmpa $FF(a4), a3          ;B7 EC 00 FF
          cmpa LATE_01(a3), a4      ;B9 EB 00 01
          cmpa LATE_02(a2), a5      ;BB EA 00 02
          cmpa LATE_FE(a1), a6      ;BD E9 00 FE
          cmpa LATE_FF(a0), a7      ;BF E8 00 FF

          cmpa.l a7, a0             ;B1 CF
          cmpa.l a6, a1             ;B3 CE
          cmpa.l a5, a2             ;B5 CD
          cmpa.l a4, a3             ;B7 CC
          cmpa.l a3, a4             ;B9 CB
          cmpa.l a2, a5             ;BB CA
          cmpa.l a1, a6             ;BD C9
          cmpa.l a0, a7             ;BF C8

          cmpa a7, a0               ;B1 CF
          cmpa a6, a1               ;B3 CE
          cmpa a5, a2               ;B5 CD
          cmpa a4, a3               ;B7 CC
          cmpa a3, a4               ;B9 CB
          cmpa a2, a5               ;BB CA
          cmpa a1, a6               ;BD C9
          cmpa a0, a7               ;BF C8

          cmpi.b #$01, ($FF00).w            ;0C 38 00 01 FF 00
          cmpi.b #$02, ($0100).w            ;0C 38 00 02 01 00
          cmpi.b #$FE, (LATE_00FF).w        ;0C 38 00 FE 00 FF
          cmpi.b #$FF, (LATE_0001).w        ;0C 38 00 FF 00 01
          cmpi.b #LATE_01, ($FF00).w        ;0C 38 00 01 FF 00
          cmpi.b #LATE_02, ($0100).w        ;0C 38 00 02 01 00
          cmpi.b #LATE_FE, (LATE_00FF).w    ;0C 38 00 FE 00 FF
          cmpi.b #LATE_FF, (LATE_0001).w    ;0C 38 00 FF 00 01

          cmpi.b #$01, ($FF00)              ;0C 38 00 01 FF 00
          cmpi.b #$02, ($0100)              ;0C 38 00 02 01 00
          cmpi.b #$FE, (LATE_00FF)          ;0C 38 00 FE 00 FF
          cmpi.b #$FF, (LATE_0001)          ;0C 38 00 FF 00 01
          cmpi.b #LATE_01, ($FF00)          ;0C 38 00 01 FF 00
          cmpi.b #LATE_02, ($0100)          ;0C 38 00 02 01 00
          cmpi.b #LATE_FE, (LATE_00FF)      ;0C 38 00 FE 00 FF
          cmpi.b #LATE_FF, (LATE_0001)      ;0C 38 00 FF 00 01

          cmpi.w #$0001, ($FF00).w          ;0C 78 00 01 FF 00
          cmpi.w #$00FF, ($0100).w          ;0C 78 00 FF 01 00
          cmpi.w #$0100, (LATE_00FF).w      ;0C 78 01 00 00 FF
          cmpi.w #$FF00, (LATE_0001).w      ;0C 78 FF 00 00 01
          cmpi.w #LATE_0001, ($FF00).w      ;0C 78 00 01 FF 00
          cmpi.w #LATE_00FF, ($0100).w      ;0C 78 00 FF 01 00
          cmpi.w #LATE_0100, (LATE_00FF).w  ;0C 78 01 00 00 FF
          cmpi.w #LATE_FF00, (LATE_0001).w  ;0C 78 FF 00 00 01

          cmpi.w #$0001, ($FF00)            ;0C 78 00 01 FF 00
          cmpi.w #$00FF, ($0100)            ;0C 78 00 FF 01 00
          cmpi.w #$0100, (LATE_00FF)        ;0C 78 01 00 00 FF
          cmpi.w #$FF00, (LATE_0001)        ;0C 78 FF 00 00 01
          cmpi.w #LATE_0001, ($FF00)        ;0C 78 00 01 FF 00
          cmpi.w #LATE_00FF, ($0100)        ;0C 78 00 FF 01 00
          cmpi.w #LATE_0100, (LATE_00FF)    ;0C 78 01 00 00 FF
          cmpi.w #LATE_FF00, (LATE_0001)    ;0C 78 FF 00 00 01

          cmpi.l #$00000001, ($FF00).w          ;0C B8 00 00 00 01 FF 00
          cmpi.l #$0000FF00, ($0100).w          ;0C B8 00 00 FF 00 01 00
          cmpi.l #$00010000, (LATE_00FF).w      ;0C B8 00 01 00 00 00 FF
          cmpi.l #$FF000000, (LATE_0001).w      ;0C B8 FF 00 00 00 00 01
          cmpi.l #LATE_00000001, ($FF00).w      ;0C B8 00 00 00 01 FF 00
          cmpi.l #LATE_0000FF00, ($0100).w      ;0C B8 00 00 FF 00 01 00
          cmpi.l #LATE_00010000, (LATE_00FF).w  ;0C B8 00 01 00 00 00 FF
          cmpi.l #LATE_FF000000, (LATE_0001).w  ;0C B8 FF 00 00 00 00 01

          cmpi.l #$00000001, ($FF00)            ;0C B8 00 00 00 01 FF 00
          cmpi.l #$0000FF00, ($0100)            ;0C B8 00 00 FF 00 01 00
          cmpi.l #$00010000, (LATE_00FF)        ;0C B8 00 01 00 00 00 FF
          cmpi.l #$FF000000, (LATE_0001)        ;0C B8 FF 00 00 00 00 01
          cmpi.l #LATE_00000001, ($FF00)        ;0C B8 00 00 00 01 FF 00
          cmpi.l #LATE_0000FF00, ($0100)        ;0C B8 00 00 FF 00 01 00
          cmpi.l #LATE_00010000, (LATE_00FF)    ;0C B8 00 01 00 00 00 FF
          cmpi.l #LATE_FF000000, (LATE_0001)    ;0C B8 FF 00 00 00 00 01

          cmpi #$0001, ($FF00)                  ;0C 78 00 01 FF 00
          cmpi #$00FF, ($0100)                  ;0C 78 00 FF 01 00
          cmpi #$0100, (LATE_00FF)              ;0C 78 01 00 00 FF
          cmpi #$FF00, (LATE_0001)              ;0C 78 FF 00 00 01
          cmpi #LATE_0001, ($FF00)              ;0C 78 00 01 FF 00
          cmpi #LATE_00FF, ($0100)              ;0C 78 00 FF 01 00
          cmpi #LATE_0100, (LATE_00FF)          ;0C 78 01 00 00 FF
          cmpi #LATE_FF00, (LATE_0001)          ;0C 78 FF 00 00 01


          cmpi.w #$01, ($FF000000).l            ;0C 79 00 01 FF 00 00 00
          cmpi.w #$02, ($00010000).l            ;0C 79 00 02 00 01 00 00
          cmpi.w #$FE, (LATE_0000FF00).l        ;0C 79 00 FE 00 00 FF 00
          cmpi.w #$FF, (LATE_00000001).l        ;0C 79 00 FF 00 00 00 01
          cmpi.w #LATE_01, ($FF000000).l        ;0C 79 00 01 FF 00 00 00
          cmpi.w #LATE_02, ($00010000).l        ;0C 79 00 02 00 01 00 00
          cmpi.w #LATE_FE, (LATE_0000FF00).l    ;0C 79 00 FE 00 00 FF 00
          cmpi.w #LATE_FF, (LATE_00000001).l    ;0C 79 00 FF 00 00 00 01

          ;TODO - fallback (from default .w to .l)
          ;cmpi.w #$01, ($FF000000)              ;0C 79 00 01 FF 00 00 00
          ;cmpi.w #$02, ($00010000)              ;0C 79 00 02 00 01 00 00
          cmpi.w #$FE, (LATE_0000FF00)          ;0C 79 00 FE 00 00 FF 00
          cmpi.w #$FF, (LATE_00000001)          ;0C 79 00 FF 00 00 00 01
          ;cmpi.w #LATE_01, ($FF000000)          ;0C 79 00 01 FF 00 00 00
          ;cmpi.w #LATE_02, ($00010000)          ;0C 79 00 02 00 01 00 00
          cmpi.w #LATE_FE, (LATE_0000FF00)      ;0C 79 00 FE 00 00 FF 00
          cmpi.w #LATE_FF, (LATE_00000001)      ;0C 79 00 FF 00 00 00 01

          cmpi #$01, ($FF000000).l              ;0C 79 00 01 FF 00 00 00
          cmpi #$02, ($00010000).l              ;0C 79 00 02 00 01 00 00
          cmpi #$FE, (LATE_0000FF00).l          ;0C 79 00 FE 00 00 FF 00
          cmpi #$FF, (LATE_00000001).l          ;0C 79 00 FF 00 00 00 01
          cmpi #LATE_01, ($FF000000).l          ;0C 79 00 01 FF 00 00 00
          cmpi #LATE_02, ($00010000).l          ;0C 79 00 02 00 01 00 00
          cmpi #LATE_FE, (LATE_0000FF00).l      ;0C 79 00 FE 00 00 FF 00
          cmpi #LATE_FF, (LATE_00000001).l      ;0C 79 00 FF 00 00 00 01

          ;cmpi #$01, ($FF000000)                ;0C 79 00 01 FF 00 00 00
          ;cmpi #$02, ($00010000)                ;0C 79 00 02 00 01 00 00
          cmpi #$FE, (LATE_0000FF00)            ;0C 79 00 FE 00 00 FF 00
          cmpi #$FF, (LATE_00000001)            ;0C 79 00 FF 00 00 00 01
          ;cmpi #LATE_01, ($FF000000)            ;0C 79 00 01 FF 00 00 00
          ;cmpi #LATE_02, ($00010000)            ;0C 79 00 02 00 01 00 00
          cmpi #LATE_FE, (LATE_0000FF00)        ;0C 79 00 FE 00 00 FF 00
          cmpi #LATE_FF, (LATE_00000001)        ;0C 79 00 FF 00 00 00 01

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

          cmpi #$0001, d7             ;0C 47 00 01
          cmpi #$00FF, d6             ;0C 46 00 FF
          cmpi #$0100, d5             ;0C 45 01 00
          cmpi #$FF00, d4             ;0C 44 FF 00
          cmpi #LATE_0001, d3         ;0C 43 00 01
          cmpi #LATE_00FF, d2         ;0C 42 00 FF
          cmpi #LATE_0100, d1         ;0C 41 01 00
          cmpi #LATE_FF00, d0         ;0C 40 FF 00



          cmpi.b #$01, (a7)           ;0C 17 00 01
          cmpi.b #$02, (a6)           ;0C 16 00 02
          cmpi.b #$FE, (a5)           ;0C 15 00 FE
          cmpi.b #$FF, (a4)           ;0C 14 00 FF
          cmpi.b #LATE_01, (a3)       ;0C 13 00 01
          cmpi.b #LATE_02, (a2)       ;0C 12 00 02
          cmpi.b #LATE_FE, (a1)       ;0C 11 00 FE
          cmpi.b #LATE_FF, (a0)       ;0C 10 00 FF

          cmpi #$01, (a7)             ;0C 17 00 01
          cmpi #$02, (a6)             ;0C 16 00 02
          cmpi #$FE, (a5)             ;0C 15 00 FE
          cmpi #$FF, (a4)             ;0C 14 00 FF
          cmpi #LATE_01, (a3)         ;0C 13 00 01
          cmpi #LATE_02, (a2)         ;0C 12 00 02
          cmpi #LATE_FE, (a1)         ;0C 11 00 FE
          cmpi #LATE_FF, (a0)         ;0C 10 00 FF


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

          cmpi #$0001, $FF(a7)           ;0C 6F 00 01 00 FF
          cmpi #$00FF, $FE(a6)           ;0C 6E 00 FF 00 FE
          cmpi #$0100, LATE_02(a5)       ;0C 6D 01 00 00 02
          cmpi #$FF00, LATE_01(a4)       ;0C 6C FF 00 00 01
          cmpi #LATE_0001, $FF(a3)       ;0C 6B 00 01 00 FF
          cmpi #LATE_00FF, $FE(a2)       ;0C 6A 00 FF 00 FE
          cmpi #LATE_0100, LATE_02(a1)   ;0C 69 01 00 00 02
          cmpi #LATE_FF00, LATE_01(a0)   ;0C 68 FF 00 00 01



          cmpi.b #$01, (a7)+          ;0C 1F 00 01
          cmpi.b #$02, (a6)+          ;0C 1E 00 02
          cmpi.b #$FE, (a5)+          ;0C 1D 00 FE
          cmpi.b #$FF, (a4)+          ;0C 1C 00 FF
          cmpi.b #LATE_01, (a3)+      ;0C 1B 00 01
          cmpi.b #LATE_02, (a2)+      ;0C 1A 00 02
          cmpi.b #LATE_FE, (a1)+      ;0C 19 00 FE
          cmpi.b #LATE_FF, (a0)+      ;0C 18 00 FF

          cmpi #$01, (a7)+            ;0C 1F 00 01
          cmpi #$02, (a6)+            ;0C 1E 00 02
          cmpi #$FE, (a5)+            ;0C 1D 00 FE
          cmpi #$FF, (a4)+            ;0C 1C 00 FF
          cmpi #LATE_01, (a3)+        ;0C 1B 00 01
          cmpi #LATE_02, (a2)+        ;0C 1A 00 02
          cmpi #LATE_FE, (a1)+        ;0C 19 00 FE
          cmpi #LATE_FF, (a0)+        ;0C 18 00 FF

          cmpi.b #$01, -(a7)          ;0C 27 00 01
          cmpi.b #$02, -(a6)          ;0C 26 00 02
          cmpi.b #$FE, -(a5)          ;0C 25 00 FE
          cmpi.b #$FF, -(a4)          ;0C 24 00 FF
          cmpi.b #LATE_01, -(a3)      ;0C 23 00 01
          cmpi.b #LATE_02, -(a2)      ;0C 22 00 02
          cmpi.b #LATE_FE, -(a1)      ;0C 21 00 FE
          cmpi.b #LATE_FF, -(a0)      ;0C 20 00 FF

          cmpi #$01, -(a7)            ;0C 27 00 01
          cmpi #$02, -(a6)            ;0C 26 00 02
          cmpi #$FE, -(a5)            ;0C 25 00 FE
          cmpi #$FF, -(a4)            ;0C 24 00 FF
          cmpi #LATE_01, -(a3)        ;0C 23 00 01
          cmpi #LATE_02, -(a2)        ;0C 22 00 02
          cmpi #LATE_FE, -(a1)        ;0C 21 00 FE
          cmpi #LATE_FF, -(a0)        ;0C 20 00 FF


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

          cmp.w d0, d7                ;BE 00
          cmp.w d1, d6                ;BC 01
          cmp.w d2, d5                ;BA 02
          cmp.w d3, d4                ;B8 03
          cmp.w d4, d3                ;B6 04
          cmp.w d5, d2                ;B4 05
          cmp.w d6, d1                ;B2 06
          cmp.w d7, d0                ;B0 07

          cmp d0, d7                  ;BE 00
          cmp d1, d6                  ;BC 01
          cmp d2, d5                  ;BA 02
          cmp d3, d4                  ;B8 03
          cmp d4, d3                  ;B6 04
          cmp d5, d2                  ;B4 05
          cmp d6, d1                  ;B2 06
          cmp d7, d0                  ;B0 07




          cmp.w ($0001).w, d0         ;B0 78 00 01
          cmp.w ($00FF).w, d1         ;B2 78 00 FF
          cmp.w ($0100).w, d2         ;B4 78 01 00
          cmp.w ($FF00).w, d3         ;B6 78 FF 00
          cmp.w (LATE_0001).w, d4     ;B8 78 00 01
          cmp.w (LATE_00FF).w, d5     ;BA 78 00 FF
          cmp.w (LATE_0100).w, d6     ;BC 78 01 00
          cmp.w (LATE_FF00).w, d7     ;BE 78 FF 00

          cmp.w ($0001), d0           ;B0 78 00 01
          cmp.w ($00FF), d1           ;B2 78 00 FF
          cmp.w ($0100), d2           ;B4 78 01 00
          cmp.w ($FF00), d3           ;B6 78 FF 00
          cmp.w (LATE_0001), d4       ;B8 78 00 01
          cmp.w (LATE_00FF), d5       ;BA 78 00 FF
          cmp.w (LATE_0100), d6       ;BC 78 01 00
          cmp.w (LATE_FF00), d7       ;BE 78 FF 00

          cmp ($0001), d0             ;B0 78 00 01
          cmp ($00FF), d1             ;B2 78 00 FF
          cmp ($0100), d2             ;B4 78 01 00
          cmp ($FF00), d3             ;B6 78 FF 00
          cmp (LATE_0001), d4         ;B8 78 00 01
          cmp (LATE_00FF), d5         ;BA 78 00 FF
          cmp (LATE_0100), d6         ;BC 78 01 00
          cmp (LATE_FF00), d7         ;BE 78 FF 00

          cmp ($0001).w, d0           ;B0 78 00 01
          cmp ($00FF).w, d1           ;B2 78 00 FF
          cmp ($0100).w, d2           ;B4 78 01 00
          cmp ($FF00).w, d3           ;B6 78 FF 00
          cmp (LATE_0001).w, d4       ;B8 78 00 01
          cmp (LATE_00FF).w, d5       ;BA 78 00 FF
          cmp (LATE_0100).w, d6       ;BC 78 01 00
          cmp (LATE_FF00).w, d7       ;BE 78 FF 00


          cmp.l ($0001).w, d0         ;B0 B8 00 01
          cmp.l ($00FF).w, d1         ;B2 B8 00 FF
          cmp.l ($0100).w, d2         ;B4 B8 01 00
          cmp.l ($FF00).w, d3         ;B6 B8 FF 00
          cmp.l (LATE_0001).w, d4     ;B8 B8 00 01
          cmp.l (LATE_00FF).w, d5     ;BA B8 00 FF
          cmp.l (LATE_0100).w, d6     ;BC B8 01 00
          cmp.l (LATE_FF00).w, d7     ;BE B8 FF 00

          cmp.l ($0001), d0           ;B0 B8 00 01
          cmp.l ($00FF), d1           ;B2 B8 00 FF
          cmp.l ($0100), d2           ;B4 B8 01 00
          cmp.l ($FF00), d3           ;B6 B8 FF 00
          cmp.l (LATE_0001), d4       ;B8 B8 00 01
          cmp.l (LATE_00FF), d5       ;BA B8 00 FF
          cmp.l (LATE_0100), d6       ;BC B8 01 00
          cmp.l (LATE_FF00), d7       ;BE B8 FF 00



          cmp.w (a7), d0              ;B0 57
          cmp.w (a6), d1              ;B2 56
          cmp.w (a5), d2              ;B4 55
          cmp.w (a4), d3              ;B6 54
          cmp.w (a3), d4              ;B8 53
          cmp.w (a2), d5              ;BA 52
          cmp.w (a1), d6              ;BC 51
          cmp.w (a0), d7              ;BE 50

          cmp (a7), d0                ;B0 57
          cmp (a6), d1                ;B2 56
          cmp (a5), d2                ;B4 55
          cmp (a4), d3                ;B6 54
          cmp (a3), d4                ;B8 53
          cmp (a2), d5                ;BA 52
          cmp (a1), d6                ;BC 51
          cmp (a0), d7                ;BE 50


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

          cmp $01(a0),d7              ;BE 68 00 01
          cmp $02(a1),d6              ;BC 69 00 02
          cmp $FE(a2),d5              ;BA 6A 00 FE
          cmp $FF(a3),d4              ;B8 6B 00 FF
          cmp LATE_01(a4),d3          ;B6 6C 00 01
          cmp LATE_02(a5),d2          ;B4 6D 00 02
          cmp LATE_FE(a6),d1          ;B2 6E 00 FE
          cmp LATE_FF(a7),d0          ;B0 6F 00 FF


          cmp.w (a7)+, d0             ;B0 5F
          cmp.w (a6)+, d1             ;B2 5E
          cmp.w (a5)+, d2             ;B4 5D
          cmp.w (a4)+, d3             ;B6 5C
          cmp.w (a3)+, d4             ;B8 5B
          cmp.w (a2)+, d5             ;BA 5A
          cmp.w (a1)+, d6             ;BC 59
          cmp.w (a0)+, d7             ;BE 58

          cmp.b (a7)+, d0             ;B0 5F
          cmp.b (a6)+, d1             ;B2 5E
          cmp.b (a5)+, d2             ;B4 5D
          cmp.b (a4)+, d3             ;B6 5C
          cmp.b (a3)+, d4             ;B8 5B
          cmp.b (a2)+, d5             ;BA 5A
          cmp.b (a1)+, d6             ;BC 59
          cmp.b (a0)+, d7             ;BE 58

          cmp (a7)+, d0               ;B0 5F
          cmp (a6)+, d1               ;B2 5E
          cmp (a5)+, d2               ;B4 5D
          cmp (a4)+, d3               ;B6 5C
          cmp (a3)+, d4               ;B8 5B
          cmp (a2)+, d5               ;BA 5A
          cmp (a1)+, d6               ;BC 59
          cmp (a0)+, d7               ;BE 58


          cmp.b -(a7), d0             ;B0 27
          cmp.b -(a6), d1             ;B2 26
          cmp.b -(a5), d2             ;B4 25
          cmp.b -(a4), d3             ;B6 24
          cmp.b -(a3), d4             ;B8 23
          cmp.b -(a2), d5             ;BA 22
          cmp.b -(a1), d6             ;BC 21
          cmp.b -(a0), d7             ;BE 20

          cmp.w -(a7), d0             ;B0 27
          cmp.w -(a6), d1             ;B2 26
          cmp.w -(a5), d2             ;B4 25
          cmp.w -(a4), d3             ;B6 24
          cmp.w -(a3), d4             ;B8 23
          cmp.w -(a2), d5             ;BA 22
          cmp.w -(a1), d6             ;BC 21
          cmp.w -(a0), d7             ;BE 20

          cmp -(a7), d0               ;B0 27
          cmp -(a6), d1               ;B2 26
          cmp -(a5), d2               ;B4 25
          cmp -(a4), d3               ;B6 24
          cmp -(a3), d4               ;B8 23
          cmp -(a2), d5               ;BA 22
          cmp -(a1), d6               ;BC 21
          cmp -(a0), d7               ;BE 20


-
          dbt d7, -               ;50 CF FF FE
          dbt d6, -               ;50 CE FF FA
          dbt d5, -               ;50 CD FF F6
          dbt d4, -               ;50 CC FF F2
          dbt d3, +               ;50 CB 00 0E
          dbt d2, +               ;50 CA 00 0A
          dbt d1, +               ;50 C9 00 06
          dbt d0, +               ;50 C8 00 02
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
          dbhi d7, -              ;52 CF 00 01
          dbhi d6, -              ;52 CE 00 FF
          dbhi d5, -              ;52 CD 01 00
          dbhi d4, -              ;52 CC FF 00
          dbhi d3, +              ;52 CB 00 01
          dbhi d2, +              ;52 CA 00 00
          dbhi d1, +              ;52 C9 01 00
          dbhi d0, +              ;52 C8 FF 00
+
-
          dbls d7, -              ;53 CF 00 01
          dbls d6, -              ;53 CE 00 FF
          dbls d5, -              ;53 CD 01 00
          dbls d4, -              ;53 CC FF 00
          dbls d3, +              ;53 CB 00 01
          dbls d2, +              ;53 CA 00 00
          dbls d1, +              ;53 C9 01 00
          dbls d0, +              ;53 C8 FF 00
+
-
          dbcc d7, -              ;54 CF 00 01
          dbcc d6, -              ;54 CE 00 FF
          dbcc d5, -              ;54 CD 01 00
          dbcc d4, -              ;54 CC FF 00
          dbcc d3, +              ;54 CB 00 01
          dbcc d2, +              ;54 CA 00 00
          dbcc d1, +              ;54 C9 01 00
          dbcc d0, +              ;54 C8 FF 00
+
-
          dbcs d7, -              ;55 CF 00 01
          dbcs d6, -              ;55 CE 00 FF
          dbcs d5, -              ;55 CD 01 00
          dbcs d4, -              ;55 CC FF 00
          dbcs d3, +              ;55 CB 00 01
          dbcs d2, +              ;55 CA 00 00
          dbcs d1, +              ;55 C9 01 00
          dbcs d0, +              ;55 C8 FF 00
+
-
          dbne d7, -              ;56 CF 00 01
          dbne d6, -              ;56 CE 00 FF
          dbne d5, -              ;56 CD 01 00
          dbne d4, -              ;56 CC FF 00
          dbne d3, +              ;56 CB 00 01
          dbne d2, +              ;56 CA 00 00
          dbne d1, +              ;56 C9 01 00
          dbne d0, +              ;56 C8 FF 00
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
          dbvc d7, -              ;58 CF 00 01
          dbvc d6, -              ;58 CE 00 FF
          dbvc d5, -              ;58 CD 01 00
          dbvc d4, -              ;58 CC FF 00
          dbvc d3, +              ;58 CB 00 01
          dbvc d2, +              ;58 CA 00 00
          dbvc d1, +              ;58 C9 01 00
          dbvc d0, +              ;58 C8 FF 00
+
-
          dbvs d7, -              ;59 CF 00 01
          dbvs d6, -              ;59 CE 00 FF
          dbvs d5, -              ;59 CD 01 00
          dbvs d4, -              ;59 CC FF 00
          dbvs d3, +              ;59 CB 00 01
          dbvs d2, +              ;59 CA 00 00
          dbvs d1, +              ;59 C9 01 00
          dbvs d0, +              ;59 C8 FF 00
+
-
          dbpl d7, -              ;5A CF 00 01
          dbpl d6, -              ;5A CE 00 FF
          dbpl d5, -              ;5A CD 01 00
          dbpl d4, -              ;5A CC FF 00
          dbpl d3, +              ;5A CB 00 01
          dbpl d2, +              ;5A CA 00 00
          dbpl d1, +              ;5A C9 01 00
          dbpl d0, +              ;5A C8 FF 00
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
          dbge d7, -              ;5C CF 00 01
          dbge d6, -              ;5C CE 00 FF
          dbge d5, -              ;5C CD 01 00
          dbge d4, -              ;5C CC FF 00
          dbge d3, +              ;5C CB 00 01
          dbge d2, +              ;5C CA 00 00
          dbge d1, +              ;5C C9 01 00
          dbge d0, +              ;5C C8 FF 00
+
-
          dblt d7, -              ;5D CF 00 01
          dblt d6, -              ;5D CE 00 FF
          dblt d5, -              ;5D CD 01 00
          dblt d4, -              ;5D CC FF 00
          dblt d3, +              ;5D CB 00 01
          dblt d2, +              ;5D CA 00 00
          dblt d1, +              ;5D C9 01 00
          dblt d0, +              ;5D C8 FF 00
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

          divs #$01, d7           ;8F FC 00 01
          divs #$02, d6           ;8D FC 00 02
          divs #$FE, d5           ;8B FC 00 FE
          divs #$FF, d4           ;89 FC 00 FF
          divs #LATE_01, d3       ;87 FC 00 01
          divs #LATE_02, d2       ;85 FC 00 02
          divs #LATE_FE, d1       ;83 FC 00 FE
          divs #LATE_FF, d0       ;81 FC 00 FF


          divu.w ($0001).w, d7    ;8E F8 00 01
          divu.w ($00FF).w, d6    ;8C F8 00 FF
          divu.w ($0100).w, d5    ;8A F8 01 00
          divu.w ($FF00).w, d4    ;88 F8 FF 00
          divu.w ($0001).w, d3    ;86 F8 00 01
          divu.w ($00FF).w, d2    ;84 F8 00 FF
          divu.w ($0100).w, d1    ;82 F8 01 00
          divu.w ($FF00).w, d0    ;80 F8 F0 00

          divu.w ($0001), d7      ;8E F8 00 01
          divu.w ($00FF), d6      ;8C F8 00 FF
          divu.w ($0100), d5      ;8A F8 01 00
          divu.w ($FF00), d4      ;88 F8 FF 00
          divu.w ($0001), d3      ;86 F8 00 01
          divu.w ($00FF), d2      ;84 F8 00 FF
          divu.w ($0100), d1      ;82 F8 01 00
          divu.w ($FF00), d0      ;80 F8 F0 00

          divu ($0001).w, d7      ;8E F8 00 01
          divu ($00FF).w, d6      ;8C F8 00 FF
          divu ($0100).w, d5      ;8A F8 01 00
          divu ($FF00).w, d4      ;88 F8 FF 00
          divu ($0001).w, d3      ;86 F8 00 01
          divu ($00FF).w, d2      ;84 F8 00 FF
          divu ($0100).w, d1      ;82 F8 01 00
          divu ($FF00).w, d0      ;80 F8 F0 00

          divu ($0001), d7        ;8E F8 00 01
          divu ($00FF), d6        ;8C F8 00 FF
          divu ($0100), d5        ;8A F8 01 00
          divu ($FF00), d4        ;88 F8 FF 00
          divu ($0001), d3        ;86 F8 00 01
          divu ($00FF), d2        ;84 F8 00 FF
          divu ($0100), d1        ;82 F8 01 00
          divu ($FF00), d0        ;80 F8 F0 00


          divu.w d0, d7           ;8E C0
          divu.w d1, d6           ;8C C1
          divu.w d2, d5           ;8A C2
          divu.w d3, d4           ;88 C3
          divu.w d4, d3           ;86 C4
          divu.w d5, d2           ;84 C5
          divu.w d6, d1           ;82 C6
          divu.w d7, d0           ;80 C7

          divu d0, d7             ;8E C0
          divu d1, d6             ;8C C1
          divu d2, d5             ;8A C2
          divu d3, d4             ;88 C3
          divu d4, d3             ;86 C4
          divu d5, d2             ;84 C5
          divu d6, d1             ;82 C6
          divu d7, d0             ;80 C7


          divu.w (a0), d7         ;8E D0
          divu.w (a1), d6         ;8C D1
          divu.w (a2), d5         ;8A D2
          divu.w (a3), d4         ;88 D3
          divu.w (a4), d3         ;86 D4
          divu.w (a5), d2         ;84 D5
          divu.w (a6), d1         ;82 D6
          divu.w (a7), d0         ;80 D7

          divu (a0), d7           ;8E D0
          divu (a1), d6           ;8C D1
          divu (a2), d5           ;8A D2
          divu (a3), d4           ;88 D3
          divu (a4), d3           ;86 D4
          divu (a5), d2           ;84 D5
          divu (a6), d1           ;82 D6
          divu (a7), d0           ;80 D7


          divu.w $01(a0), d7      ;8E E8 00 01
          divu.w $02(a1), d6      ;8C E9 00 02
          divu.w $FE(a2), d5      ;8A EA 00 FE
          divu.w $FF(a3), d4      ;88 EB 00 FF
          divu.w LATE_01(a4), d3  ;86 EC 00 01
          divu.w LATE_02(a5), d2  ;84 ED 00 02
          divu.w LATE_FE(a6), d1  ;82 EE 00 FE
          divu.w LATE_FF(a7), d0  ;80 EF 00 FF

          divu $01(a0), d7      ;8E E8 00 01
          divu $02(a1), d6      ;8C E9 00 02
          divu $FE(a2), d5      ;8A EA 00 FE
          divu $FF(a3), d4      ;88 EB 00 FF
          divu LATE_01(a4), d3  ;86 EC 00 01
          divu LATE_02(a5), d2  ;84 ED 00 02
          divu LATE_FE(a6), d1  ;82 EE 00 FE
          divu LATE_FF(a7), d0  ;80 EF 00 FF


          divu.w (a0)+, d7        ;8E D8
          divu.w (a1)+, d6        ;8C D9
          divu.w (a2)+, d5        ;8A DA
          divu.w (a3)+, d4        ;88 DB
          divu.w (a4)+, d3        ;86 DC
          divu.w (a5)+, d2        ;84 DD
          divu.w (a6)+, d1        ;82 DE
          divu.w (a7)+, d0        ;80 DF

          divu (a0)+, d7        ;8E D8
          divu (a1)+, d6        ;8C D9
          divu (a2)+, d5        ;8A DA
          divu (a3)+, d4        ;88 DB
          divu (a4)+, d3        ;86 DC
          divu (a5)+, d2        ;84 DD
          divu (a6)+, d1        ;82 DE
          divu (a7)+, d0        ;80 DF


          divu.w -(a0), d7        ;8E E0
          divu.w -(a1), d6        ;8C E1
          divu.w -(a2), d5        ;8A E2
          divu.w -(a3), d4        ;88 E3
          divu.w -(a4), d3        ;86 E4
          divu.w -(a5), d2        ;84 E5
          divu.w -(a6), d1        ;82 E6
          divu.w -(a7), d0        ;80 E7

          divu -(a0), d7        ;8E E0
          divu -(a1), d6        ;8C E1
          divu -(a2), d5        ;8A E2
          divu -(a3), d4        ;88 E3
          divu -(a4), d3        ;86 E4
          divu -(a5), d2        ;84 E5
          divu -(a6), d1        ;82 E6
          divu -(a7), d0        ;80 E7


          eori.w #$0001, d7       ;0A 47 00 01
          eori.w #$00FF, d6       ;0A 46 00 FF
          eori.w #$0100, d5       ;0A 45 01 00
          eori.w #$FF00, d4       ;0A 44 FF 00
          eori.w #LATE_0001, d3   ;0A 43 00 01
          eori.w #LATE_00FF, d2   ;0A 42 00 FF
          eori.w #LATE_0100, d1   ;0A 41 01 00
          eori.w #LATE_FF00, d0   ;0A 40 FF 00

          eori #$0001, d7         ;0A 47 00 01
          eori #$00FF, d6         ;0A 46 00 FF
          eori #$0100, d5         ;0A 45 01 00
          eori #$FF00, d4         ;0A 44 FF 00
          eori #LATE_0001, d3     ;0A 43 00 01
          eori #LATE_00FF, d2     ;0A 42 00 FF
          eori #LATE_0100, d1     ;0A 41 01 00
          eori #LATE_FF00, d0     ;0A 40 FF 00


          eori.b #$01, ($0001).w          ;0A 38 00 01 00 01
          eori.b #$02, ($00FF).w          ;0A 38 00 02 00 FF
          eori.b #$FE, (LATE_0100).w      ;0A 38 00 FE 01 00
          eori.b #$FF, (LATE_FF00).w      ;0A 38 00 FF FF 00
          eori.b #LATE_01, ($0001).w      ;0A 38 00 01 00 01
          eori.b #LATE_02, ($00FF).w      ;0A 38 00 02 00 FF
          eori.b #LATE_FE, (LATE_0100).w  ;0A 38 00 FE 01 00
          eori.b #LATE_FF, (LATE_FF00).w  ;0A 38 00 FF FF 00

          eori.b #$01, ($0001)            ;0A 38 00 01 00 01
          eori.b #$02, ($00FF)            ;0A 38 00 02 00 FF
          eori.b #$FE, (LATE_0100)        ;0A 38 00 FE 01 00
          eori.b #$FF, (LATE_FF00)        ;0A 38 00 FF FF 00
          eori.b #LATE_01, ($0001)        ;0A 38 00 01 00 01
          eori.b #LATE_02, ($00FF)        ;0A 38 00 02 00 FF
          eori.b #LATE_FE, (LATE_0100)    ;0A 38 00 FE 01 00
          eori.b #LATE_FF, (LATE_FF00)    ;0A 38 00 FF FF 00

          eori.w #$0001, ($FF00).w          ;0A 78 00 01 FF 00
          eori.w #$00FF, ($0100).w          ;0A 78 00 FF 01 00
          eori.w #$0100, (LATE_00FF).w      ;0A 78 01 00 00 FF
          eori.w #$FF00, (LATE_0001).w      ;0A 78 FF 00 00 01
          eori.w #LATE_0001, ($FF00).w      ;0A 78 00 01 FF 00
          eori.w #LATE_00FF, ($0100).w      ;0A 78 00 FF 01 00
          eori.w #LATE_0100, (LATE_00FF).w  ;0A 78 01 00 00 FF
          eori.w #LATE_FF00, (LATE_0001).w  ;0A 78 FF 00 00 01

          eori.w #$0001, ($FF00)            ;0A 78 00 01 FF 00
          eori.w #$00FF, ($0100)            ;0A 78 00 FF 01 00
          eori.w #$0100, (LATE_00FF)        ;0A 78 01 00 00 FF
          eori.w #$FF00, (LATE_0001)        ;0A 78 FF 00 00 01
          eori.w #LATE_0001, ($FF00)        ;0A 78 00 01 FF 00
          eori.w #LATE_00FF, ($0100)        ;0A 78 00 FF 01 00
          eori.w #LATE_0100, (LATE_00FF)    ;0A 78 01 00 00 FF
          eori.w #LATE_FF00, (LATE_0001)    ;0A 78 FF 00 00 01

          eori #$0001, ($FF00).w          ;0A 78 00 01 FF 00
          eori #$00FF, ($0100).w          ;0A 78 00 FF 01 00
          eori #$0100, (LATE_00FF).w      ;0A 78 01 00 00 FF
          eori #$FF00, (LATE_0001).w      ;0A 78 FF 00 00 01
          eori #LATE_0001, ($FF00).w      ;0A 78 00 01 FF 00
          eori #LATE_00FF, ($0100).w      ;0A 78 00 FF 01 00
          eori #LATE_0100, (LATE_00FF).w  ;0A 78 01 00 00 FF
          eori #LATE_FF00, (LATE_0001).w  ;0A 78 FF 00 00 01

          eori #$0001, ($FF00)              ;0A 78 00 01 FF 00
          eori #$00FF, ($0100)              ;0A 78 00 FF 01 00
          eori #$0100, (LATE_00FF)          ;0A 78 01 00 00 FF
          eori #$FF00, (LATE_0001)          ;0A 78 FF 00 00 01
          eori #LATE_0001, ($FF00)          ;0A 78 00 01 FF 00
          eori #LATE_00FF, ($0100)          ;0A 78 00 FF 01 00
          eori #LATE_0100, (LATE_00FF)      ;0A 78 01 00 00 FF
          eori #LATE_FF00, (LATE_0001)      ;0A 78 FF 00 00 01


          eori.l #$00000001,d0            ;0A 80 00 00 00 01
          eori.l #$0000FF00,d1            ;0A 81 00 00 FF 00
          eori.l #$00010000,d2            ;0A 82 00 01 00 00
          eori.l #$FF000000,d3            ;0A 83 FF 00 00 00
          eori.l #LATE_00000001,d4        ;0A 84 00 00 00 01
          eori.l #LATE_0000FF00,d5        ;0A 85 00 00 FF 00
          eori.l #LATE_00010000,d6        ;0A 86 00 01 00 00
          eori.l #LATE_FF000000,d7        ;0A 87 FF 00 00 00

          eori #$00000001,d0              ;0A 80 00 00 00 01
          eori #$0000FF00,d1              ;0A 81 00 00 FF 00
          eori #$00010000,d2              ;0A 82 00 01 00 00
          eori #$FF000000,d3              ;0A 83 FF 00 00 00
          eori #LATE_00000001,d4          ;0A 84 00 00 00 01
          eori #LATE_0000FF00,d5          ;0A 85 00 00 FF 00
          eori #LATE_00010000,d6          ;0A 86 00 01 00 00
          eori #LATE_FF000000,d7          ;0A 87 FF 00 00 00


          eori.l #$0001, (a7)             ;0A 97 00 00 00 01
          eori.l #$00FF, (a6)             ;0A 96 00 00 00 FF
          eori.l #$0100, (a5)             ;0A 95 00 00 01 00
          eori.l #$FF00, (a4)             ;0A 94 00 00 FF 00
          eori.l #LATE_0001, (a3)         ;0A 93 00 00 00 01
          eori.l #LATE_00FF, (a2)         ;0A 92 00 00 00 FF
          eori.l #LATE_0100, (a1)         ;0A 91 00 00 01 00
          eori.l #LATE_FF00, (a0)         ;0A 90 00 00 FF 00

          eori #$0001, (a7)               ;0A 97 00 00 00 01
          eori #$00FF, (a6)               ;0A 96 00 00 00 FF
          eori #$0100, (a5)               ;0A 95 00 00 01 00
          eori #$FF00, (a4)               ;0A 94 00 00 FF 00
          eori #LATE_0001, (a3)           ;0A 93 00 00 00 01
          eori #LATE_00FF, (a2)           ;0A 92 00 00 00 FF
          eori #LATE_0100, (a1)           ;0A 91 00 00 01 00
          eori #LATE_FF00, (a0)           ;0A 90 00 00 FF 00


          eori.l #$0001, $FF(a0)          ;0A A8 00 00 00 01 00 FF
          eori.l #$00FF, $FE(a1)          ;0A A9 00 00 00 FF 00 FE
          eori.l #$0100, LATE_02(a2)      ;0A AA 00 00 01 00 00 02
          eori.l #$FF00, LATE_01(a3)      ;0A AB 00 00 FF 00 00 01
          eori.l #LATE_0001, $FF(a4)      ;0A AC 00 00 00 01 00 FF
          eori.l #LATE_00FF, $FE(a5)      ;0A AD 00 00 00 FF 00 FE
          eori.l #LATE_0100, LATE_02(a6)  ;0A AE 00 00 01 00 00 02
          eori.l #LATE_FF00, LATE_01(a7)  ;0A AF 00 00 FF 00 00 01

          eori #$0001, $FF(a0)            ;0A A8 00 00 00 01 00 FF
          eori #$00FF, $FE(a1)            ;0A A9 00 00 00 FF 00 FE
          eori #$0100, LATE_02(a2)        ;0A AA 00 00 01 00 00 02
          eori #$FF00, LATE_01(a3)        ;0A AB 00 00 FF 00 00 01
          eori #LATE_0001, $FF(a4)        ;0A AC 00 00 00 01 00 FF
          eori #LATE_00FF, $FE(a5)        ;0A AD 00 00 00 FF 00 FE
          eori #LATE_0100, LATE_02(a6)    ;0A AE 00 00 01 00 00 02
          eori #LATE_FF00, LATE_01(a7)    ;0A AF 00 00 FF 00 00 01


          eori.l #$0001, (a7)+            ;0A 9F 00 00 00 01
          eori.l #$00FF, (a6)+            ;0A 9E 00 00 00 FF
          eori.l #$0100, (a5)+            ;0A 9D 00 00 01 00
          eori.l #$FF00, (a4)+            ;0A 9C 00 00 FF 00
          eori.l #LATE_0001, (a3)+        ;0A 9B 00 00 00 01
          eori.l #LATE_00FF, (a2)+        ;0A 9A 00 00 00 FF
          eori.l #LATE_0100, (a1)+        ;0A 99 00 00 01 00
          eori.l #LATE_FF00, (a0)+        ;0A 98 00 00 FF 00

          eori #$0001, (a7)+              ;0A 9F 00 00 00 01
          eori #$00FF, (a6)+              ;0A 9E 00 00 00 FF
          eori #$0100, (a5)+              ;0A 9D 00 00 01 00
          eori #$FF00, (a4)+              ;0A 9C 00 00 FF 00
          eori #LATE_0001, (a3)+          ;0A 9B 00 00 00 01
          eori #LATE_00FF, (a2)+          ;0A 9A 00 00 00 FF
          eori #LATE_0100, (a1)+          ;0A 99 00 00 01 00
          eori #LATE_FF00, (a0)+          ;0A 98 00 00 FF 00

          eori.l #$0001, -(a7)            ;0A A7 00 00 00 01
          eori.l #$00FF, -(a6)            ;0A A6 00 00 00 FF
          eori.l #$0100, -(a5)            ;0A A5 00 00 01 00
          eori.l #$FF00, -(a4)            ;0A A4 00 00 FF 00
          eori.l #LATE_0001, -(a3)        ;0A A3 00 00 00 01
          eori.l #LATE_00FF, -(a2)        ;0A A2 00 00 00 FF
          eori.l #LATE_0100, -(a1)        ;0A A1 00 00 01 00
          eori.l #LATE_FF00, -(a0)        ;0A A0 00 00 FF 00

          eori #$0001, -(a7)              ;0A A7 00 00 00 01
          eori #$00FF, -(a6)              ;0A A6 00 00 00 FF
          eori #$0100, -(a5)              ;0A A5 00 00 01 00
          eori #$FF00, -(a4)              ;0A A4 00 00 FF 00
          eori #LATE_0001, -(a3)          ;0A A3 00 00 00 01
          eori #LATE_00FF, -(a2)          ;0A A2 00 00 00 FF
          eori #LATE_0100, -(a1)          ;0A A1 00 00 01 00
          eori #LATE_FF00, -(a0)          ;0A A0 00 00 FF 00


          eori #$0001, sr                 ;0A 7C 00 01
          eori #$FF00, sr                 ;0A 7C FF 00
          eori #LATE_0001, sr             ;0A 7C 00 01
          eori #LATE_FF00, sr             ;0A 7C FF 00



          eor.b d0,d7             ;B1 07
          eor.b d1,d6             ;B3 06
          eor.b d2,d5             ;B5 05
          eor.b d3,d4             ;B7 04
          eor.b d4,d3             ;B9 03
          eor.b d5,d2             ;BB 02
          eor.b d6,d1             ;BD 01
          eor.b d7,d0             ;BF 00

          eor.w d0,d7             ;B1 47
          eor.w d1,d6             ;B3 46
          eor.w d2,d5             ;B5 45
          eor.w d3,d4             ;B7 44
          eor.w d4,d3             ;B9 43
          eor.w d5,d2             ;BB 42
          eor.w d6,d1             ;BD 41
          eor.w d7,d0             ;BF 40

          eor d0,d7               ;B1 47
          eor d1,d6               ;B3 46
          eor d2,d5               ;B5 45
          eor d3,d4               ;B7 44
          eor d4,d3               ;B9 43
          eor d5,d2               ;BB 42
          eor d6,d1               ;BD 41
          eor d7,d0               ;BF 40


          exg d0,d7               ;C1 47
          exg d1,d6               ;C3 46
          exg d2,d5               ;C5 45
          exg d3,d4               ;C7 44
          exg d4,d3               ;C9 43
          exg d5,d2               ;CB 42
          exg d6,d1               ;CD 41
          exg d7,d0               ;CF 40

          exg a0,a7               ;C1 4F
          exg a1,a6               ;C3 4E
          exg a2,a5               ;C5 4D
          exg a3,a4               ;C7 4C
          exg a4,a3               ;C9 4B
          exg a5,a2               ;CB 4A
          exg a6,a1               ;CD 49
          exg a7,a0               ;CF 48

          exg d0,a7               ;C1 8F
          exg d1,a6               ;C3 8E
          exg d2,a5               ;C5 8D
          exg d3,a4               ;C7 8C
          exg d4,a3               ;C9 8B
          exg d5,a2               ;CB 8A
          exg d6,a1               ;CD 89
          exg d7,a0               ;CF 88

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

          ext d0                  ;48 80
          ext d1                  ;48 81
          ext d2                  ;48 82
          ext d3                  ;48 83
          ext d4                  ;48 84
          ext d5                  ;48 85
          ext d6                  ;48 86
          ext d7                  ;48 87


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

          lea ($000001), a0         ;41 F9 00 00 00 01
          lea ($00FF00), a1         ;43 F9 00 00 FF 00
          lea ($010000), a2         ;45 F9 00 01 00 00
          lea ($FF0000), a3         ;47 F9 00 FF 00 00
          lea (LATE_000001), a4     ;49 F9 00 00 00 01
          lea (LATE_00FF00), a5     ;4B F9 00 00 FF 00
          lea (LATE_010000), a6     ;4D F9 00 01 00 00
          lea (LATE_FF0000), a7     ;4F F9 00 FF 00 00
          lea ($123456), sp         ;4F F9 00 12 34 56

          ;invalid addressing mode?
          ;lea d7, a0                ;41 C7
          ;lea d6, a1                ;43 C6
          ;lea d5, a2                ;45 C5
          ;lea d4, a3                ;47 C4
          ;lea d3, a4                ;49 C3
          ;lea d2, a5                ;4B C2
          ;lea d1, a6                ;4D C1
          ;lea d0, a7                ;4F C0
          ;lea d0, sp                ;4F C0

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
          lea $7F00(a3), a4         ;49 EB 7F 00
          lea LATE_0001(a4), a3     ;47 EC 00 01
          lea LATE_00FF(a5), a2     ;45 ED 00 FF
          lea LATE_0100(a6), a1     ;43 EE 01 00
          lea LATE_7F00(a7), a0     ;41 EF 7F 00


          lea (a0,d7.w), a7         ;4F F0 70 00
          lea (a1,d6.w), a6         ;4D F1 60 00
          lea (a2,d5.w), a5         ;4B F2 50 00
          lea (a3,d4.w), a4         ;49 F3 40 00
          lea (a4,d3.w), a3         ;47 F4 30 00
          lea (a5,d2.w), a2         ;45 F5 20 00
          lea (a6,d1.w), a1         ;43 F6 10 00
          lea (a7,d0.w), a0         ;41 F7 00 00

          lea (a0,d7), a7         ;4F F0 70 00
          lea (a1,d6), a6         ;4D F1 60 00
          lea (a2,d5), a5         ;4B F2 50 00
          lea (a3,d4), a4         ;49 F3 40 00
          lea (a4,d3), a3         ;47 F4 30 00
          lea (a5,d2), a2         ;45 F5 20 00
          lea (a6,d1), a1         ;43 F6 10 00
          lea (a7,d0), a0         ;41 F7 00 00


          lea $01(a0,d7.w), a7      ;4F F0 70 01
          lea $7F(a1,d6.w), a6      ;4D F1 60 7F
          lea LATE_01(a2,d5.w), a5  ;4B F2 50 01
          lea LATE_7F(a3,d4.w), a4  ;49 F3 40 7F
          lea $01(a4,d3.w), a3      ;47 F4 30 01
          lea $7F(a5,d2.w), a2      ;45 F5 20 7F
          lea LATE_01(a6,d1.w), a1  ;43 F6 10 01
          lea LATE_7F(a7,d0.w), a0  ;41 F7 00 7F

          lea $01(pc,d7.w), a7      ;4F FB 70 01 ? displacement out of range?
          lea $7F(pc,d6.w), a6      ;4D FB 60 7F ? displacement out of range?
          lea LATE_01(pc,d5.w), a5  ;4B FB 50 01 ? displacement out of range?
          lea LATE_7F(pc,d4.w), a4  ;49 FB 40 7F ? displacement out of range?
          lea $01(pc,d3.w), a3      ;47 FB 30 01 ? displacement out of range?
          lea $7F(pc,d2.w), a2      ;45 FB 20 7F ? displacement out of range?
          lea LATE_01(pc,d1.w), a1  ;43 FB 10 01 ? displacement out of range?
          lea LATE_7F(pc,d0.w), a0  ;41 FB 00 7F ? displacement out of range?

          lea $01(a0,d7), a7      ;4F F0 70 01
          lea $7F(a1,d6), a6      ;4D F1 60 7F
          lea LATE_01(a2,d5), a5  ;4B F2 50 01
          lea LATE_7F(a3,d4), a4  ;49 F3 40 7F
          lea $01(a4,d3), a3      ;47 F4 30 01
          lea $7F(a5,d2), a2      ;45 F5 20 7F
          lea LATE_01(a6,d1), a1  ;43 F6 10 01
          lea LATE_7F(a7,d0), a0  ;41 F7 00 7F

          lea $01(pc,d7), a7      ;4F FB 70 01 ? displacement out of range?
          lea $7F(pc,d6), a6      ;4D FB 60 7F ? displacement out of range?
          lea LATE_01(pc,d5), a5  ;4B FB 50 01 ? displacement out of range?
          lea LATE_7F(pc,d4), a4  ;49 FB 40 7F ? displacement out of range?
          lea $01(pc,d3), a3      ;47 FB 30 01 ? displacement out of range?
          lea $7F(pc,d2), a2      ;45 FB 20 7F ? displacement out of range?
          lea LATE_01(pc,d1), a1  ;43 FB 10 01 ? displacement out of range?
          lea LATE_7F(pc,d0), a0  ;41 FB 00 7F ? displacement out of range?

          lea $000001,a7        ;4F F9 00 00 00 01
          lea $0000FF,a6        ;4D F9 00 00 00 FF
          lea $000100,a5        ;4B F9 00 00 01 00
          lea $00FF00,a4        ;49 F9 00 00 FF 00
          lea $010000,a3        ;47 F9 00 01 00 00
          lea $FF0000,a2        ;45 F9 00 FF 00 00
          lea $FF00FF,a1        ;43 F9 00 FF 00 FF
          lea $123456,a0        ;41 F9 00 12 34 56

          link a0, #$0001           ;4E 50 00 01
          link a1, #$00FF           ;4E 51 00 FF
          link a2, #$0100           ;4E 52 01 00
          link a3, #$FF00           ;4E 53 FF 00    ? Immediate data exceeds 16 bits
          link a4, #LATE_0001       ;4E 54 00 01
          link a5, #LATE_00FF       ;4E 55 00 FF
          link a6, #LATE_0100       ;4E 56 01 00
          link a7, #LATE_FF00       ;4E 57 FF 00    ? Immediate data exceeds 16 bits

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

          lsr #1, d7            ;E2 4F
          lsr #2, d6            ;E4 4E
          lsr #3, d5            ;E6 4D
          lsr #4, d4            ;E8 4C
          lsr #5, d3            ;EA 4B
          lsr #6, d2            ;EC 4A
          lsr #7, d1            ;EE 49
          lsr #8, d0            ;E0 48


          lsr.w d0, d7          ;E0 6F
          lsr.w d1, d6          ;E2 6E
          lsr.w d2, d5          ;E4 6D
          lsr.w d3, d4          ;E6 6C
          lsr.w d4, d3          ;E8 6B
          lsr.w d5, d2          ;EA 6A
          lsr.w d6, d1          ;EC 69
          lsr.w d7, d0          ;EE 68

          lsr d0, d7            ;E0 6F
          lsr d1, d6            ;E2 6E
          lsr d2, d5            ;E4 6D
          lsr d3, d4            ;E6 6C
          lsr d4, d3            ;E8 6B
          lsr d5, d2            ;EA 6A
          lsr d6, d1            ;EC 69
          lsr d7, d0            ;EE 68

          lsl.l #1, d7          ;E3 8F
          lsl.l #2, d6          ;E5 8E
          lsl.l #3, d5          ;E7 8D
          lsl.l #4, d4          ;E9 8C
          lsl.l #5, d3          ;EB 8B
          lsl.l #6, d2          ;ED 8A
          lsl.l #7, d1          ;EF 89
          lsl.l #8, d0          ;E1 88

          lsl #1, d7            ;E3 8F
          lsl #2, d6            ;E5 8E
          lsl #3, d5            ;E7 8D
          lsl #4, d4            ;E9 8C
          lsl #5, d3            ;EB 8B
          lsl #6, d2            ;ED 8A
          lsl #7, d1            ;EF 89
          lsl #8, d0            ;E1 88

          asr.l #1, d7          ;E2 87
          asr.l #2, d6          ;E4 86
          asr.l #3, d5          ;E6 85
          asr.l #4, d4          ;E8 84
          asr.l #5, d3          ;EA 83
          asr.l #6, d2          ;EC 82
          asr.l #7, d1          ;EE 81
          asr.l #8, d0          ;E0 80

          asr #1, d7            ;E2 87
          asr #2, d6            ;E4 86
          asr #3, d5            ;E6 85
          asr #4, d4            ;E8 84
          asr #5, d3            ;EA 83
          asr #6, d2            ;EC 82
          asr #7, d1            ;EE 81
          asr #8, d0            ;E0 80

          asr $01(a7)           ;E0 EF 00 01
          asr $02(a6)           ;E0 EE 00 02
          asr $FE(a5)           ;E0 ED 00 FE
          asr $FF(a4)           ;E0 EC 00 FF
          asr LATE_01(a3)       ;E0 EB 00 01
          asr LATE_02(a2)       ;E0 EA 00 02
          asr LATE_FE(a1)       ;E0 E9 00 FE
          asr LATE_FF(a0)       ;E0 E8 00 FF

          movem.l d0,-(a7)                 ;48E7 8000
          movem.l d1,-(a6)                 ;48E6 4000
          movem.l d2,-(a5)                 ;48E5 2000
          movem.l d3,-(a4)                 ;48E4 1000
          movem.l d4,-(a3)                 ;48E3 0800
          movem.l d5,-(a2)                 ;48E2 0400
          movem.l d6,-(a1)                 ;48E1 0200
          movem.l d7,-(a0)                 ;48E0 0100
          movem.l a0,-(a7)                 ;48E7 0080
          movem.l a1,-(a6)                 ;48E6 0040
          movem.l a2,-(a5)                 ;48E5 0020
          movem.l a3,-(a4)                 ;48E4 0010
          movem.l a4,-(a3)                 ;48E3 0008
          movem.l a5,-(a2)                 ;48E2 0004
          movem.l a6,-(a1)                 ;48E1 0002
          movem.l a7,-(a0)                 ;48E0 0001
          movem.l d0-a7,-(a4)              ;48E4 FFFF
          movem.l d4-a4,-(a5)              ;48E5 0FF8
          movem.l d0-d2/d1-d3/a2/a7,-(a1)  ;48E1 F021

          movem.w d0,-(a7)                 ;48A7 8000
          movem.w d1,-(a6)                 ;48A6 4000
          movem.w d2,-(a5)                 ;48A5 2000
          movem.w d3,-(a4)                 ;48A4 1000
          movem.w d4,-(a3)                 ;48A3 0800
          movem.w d5,-(a2)                 ;48A2 0400
          movem.w d6,-(a1)                 ;48A1 0200
          movem.w d7,-(a0)                 ;48A0 0100
          movem.w a0,-(a7)                 ;48A7 0080
          movem.w a1,-(a6)                 ;48A6 0040
          movem.w a2,-(a5)                 ;48A5 0020
          movem.w a3,-(a4)                 ;48A4 0010
          movem.w a4,-(a3)                 ;48A3 0008
          movem.w a5,-(a2)                 ;48A2 0004
          movem.w a6,-(a1)                 ;48A1 0002
          movem.w a7,-(a0)                 ;48A0 0001
          movem.w d0-a7,-(a4)              ;48A4 FFFF
          movem.w d4-a4,-(a5)              ;48A5 0FF8
          movem.w d0-d2/d1-d3/a2/a7,-(a1)  ;48A1 F021

          movem d0,-(a7)                   ;48A7 8000
          movem d1,-(a6)                   ;48A6 4000
          movem d2,-(a5)                   ;48A5 2000
          movem d3,-(a4)                   ;48A4 1000
          movem d4,-(a3)                   ;48A3 0800
          movem d5,-(a2)                   ;48A2 0400
          movem d6,-(a1)                   ;48A1 0200
          movem d7,-(a0)                   ;48A0 0100
          movem a0,-(a7)                   ;48A7 0080
          movem a1,-(a6)                   ;48A6 0040
          movem a2,-(a5)                   ;48A5 0020
          movem a3,-(a4)                   ;48A4 0010
          movem a4,-(a3)                   ;48A3 0008
          movem a5,-(a2)                   ;48A2 0004
          movem a6,-(a1)                   ;48A1 0002
          movem a7,-(a0)                   ;48A0 0001
          movem d0-a7,-(a4)                ;48A4 FFFF
          movem d4-a4,-(a5)                ;48A5 0FF8
          movem d0-d2/d1-d3/a2/a7,-(a1)    ;48A1 F021


          movem.l (a7)+,d0                  ;4CDF 0001
          movem.l (a6)+,d1                  ;4CDE 0002
          movem.l (a5)+,d2                  ;4CDD 0004
          movem.l (a4)+,d3                  ;4CDC 0008
          movem.l (a3)+,d4                  ;4CDB 0010
          movem.l (a2)+,d5                  ;4CDA 0020
          movem.l (a1)+,d6                  ;4CD9 0040
          movem.l (a0)+,d7                  ;4CD8 0080
          movem.l (a7)+,a0                  ;4CDF 0100
          movem.l (a6)+,a1                  ;4CDE 0200
          movem.l (a5)+,a2                  ;4CDD 0400
          movem.l (a4)+,a3                  ;4CDC 0800
          movem.l (a3)+,a4                  ;4CDB 1000
          movem.l (a2)+,a5                  ;4CDA 2000
          movem.l (a1)+,a6                  ;4CD9 4000
          movem.l (a0)+,a7                  ;4CD8 8000
          movem.l (a4)+,d0-a7               ;4CDC FFFF
          movem.l (a5)+,d4-a4               ;4CDD 1FF0
          movem.l (a1)+,d0-d2/d1-d3/a2/a7   ;4CD9 840F

          movem.w (a7)+,d0                  ;4C9F 0001
          movem.w (a6)+,d1                  ;4C9E 0002
          movem.w (a5)+,d2                  ;4C9D 0004
          movem.w (a4)+,d3                  ;4C9C 0008
          movem.w (a3)+,d4                  ;4C9B 0010
          movem.w (a2)+,d5                  ;4C9A 0020
          movem.w (a1)+,d6                  ;4C99 0040
          movem.w (a0)+,d7                  ;4C98 0080
          movem.w (a7)+,a0                  ;4C9F 0100
          movem.w (a6)+,a1                  ;4C9E 0200
          movem.w (a5)+,a2                  ;4C9D 0400
          movem.w (a4)+,a3                  ;4C9C 0800
          movem.w (a3)+,a4                  ;4C9B 1000
          movem.w (a2)+,a5                  ;4C9A 2000
          movem.w (a1)+,a6                  ;4C99 4000
          movem.w (a0)+,a7                  ;4C98 8000
          movem.w (a4)+,d0-a7               ;4C9C FFFF
          movem.w (a5)+,d4-a4               ;4C9D 1FF0
          movem.w (a1)+,d0-d2/d1-d3/a2/a7   ;4C99 840F

          movem (a7)+,d0                    ;4C9F 0001
          movem (a6)+,d1                    ;4C9E 0002
          movem (a5)+,d2                    ;4C9D 0004
          movem (a4)+,d3                    ;4C9C 0008
          movem (a3)+,d4                    ;4C9B 0010
          movem (a2)+,d5                    ;4C9A 0020
          movem (a1)+,d6                    ;4C99 0040
          movem (a0)+,d7                    ;4C98 0080
          movem (a7)+,a0                    ;4C9F 0100
          movem (a6)+,a1                    ;4C9E 0200
          movem (a5)+,a2                    ;4C9D 0400
          movem (a4)+,a3                    ;4C9C 0800
          movem (a3)+,a4                    ;4C9B 1000
          movem (a2)+,a5                    ;4C9A 2000
          movem (a1)+,a6                    ;4C99 4000
          movem (a0)+,a7                    ;4C98 8000
          movem (a4)+,d0-a7                 ;4C9C FFFF
          movem (a5)+,d4-a4                 ;4C9D 1FF0
          movem (a1)+,d0-d2/d1-d3/a2/a7     ;4C99 840F


          move.b #$01, ($0001).w            ;11 FC 00 01 00 01
          move.b #$02, ($00FF).w            ;11 FC 00 02 00 FF
          move.b #$FE, (LATE_0100).w        ;11 FC 00 FE 01 00
          move.b #$FF, (LATE_FF00).w        ;11 FC 00 FF FF 00
          move.b #LATE_01, ($0001).w        ;11 FC 00 01 00 01
          move.b #LATE_02, ($00FF).w        ;11 FC 00 02 00 FF
          move.b #LATE_FE, (LATE_0100).w    ;11 FC 00 FE 01 00
          move.b #LATE_FF, (LATE_FF00).w    ;11 FC 00 FF FF 00

          move.b #$01, ($0001)              ;11 FC 00 01 00 01
          move.b #$02, ($00FF)              ;11 FC 00 02 00 FF
          move.b #$FE, (LATE_0100)          ;11 FC 00 FE 01 00 - 13FC 00FE 00000100
          move.b #$FF, (LATE_FF00)          ;11 FC 00 FF FF 00 - 13FC 00FF 0000FF00
          move.b #LATE_01, ($0001)          ;11 FC 00 01 00 01
          move.b #LATE_02, ($00FF)          ;11 FC 00 02 00 FF
          move.b #LATE_FE, (LATE_0100)      ;11 FC 00 FE 01 00 - 13FC 00FE 00000100
          move.b #LATE_FF, (LATE_FF00)      ;11 FC 00 FF FF 00 - 13FC 00FF 0000FF00

          move.w #$0001, ($0001).w          ;31 FC 00 01 00 01
          move.w #$00FF, ($00FF).w          ;31 FC 00 FF 00 FF
          move.w #$0100, (LATE_0100).w      ;31 FC 01 00 01 00
          move.w #$FF00, (LATE_FF00).w      ;31 FC FF 00 FF 00
          move.w #LATE_0001, ($0001).w      ;31 FC 00 01 00 01
          move.w #LATE_00FF, ($00FF).w      ;31 FC 00 FF 00 FF
          move.w #LATE_0100, (LATE_0100).w  ;31 FC 01 00 01 00
          move.w #LATE_FF00, (LATE_FF00).w  ;31 FC FF 00 FF 00

          move.w #$0001, ($0001)            ;31 FC 00 01 00 01
          move.w #$00FF, ($00FF)            ;31 FC 00 FF 00 FF
          move.w #$0100, (LATE_0100)        ;31 FC 01 00 01 00 - 33FC 0100 00000100
          move.w #$FF00, (LATE_FF00)        ;31 FC FF 00 FF 00 - 33FC FF00 0000FF00
          move.w #LATE_0001, ($0001)        ;31 FC 00 01 00 01
          move.w #LATE_00FF, ($00FF)        ;31 FC 00 FF 00 FF
          move.w #LATE_0100, (LATE_0100)    ;31 FC 01 00 01 00 - 33FC 0100 00000100
          move.w #LATE_FF00, (LATE_FF00)    ;31 FC FF 00 FF 00 - 33FC FF00 0000FF00

          move #$0001, ($0001)              ;31 FC 00 01 00 01
          move #$00FF, ($00FF)              ;31 FC 00 FF 00 FF
          move #$0100, (LATE_0100)          ;31 FC 01 00 01 00 - 33FC 0100 00000100
          move #$FF00, (LATE_FF00)          ;31 FC FF 00 FF 00 - 33FC FF00 0000FF00
          move #LATE_0001, ($0001)          ;31 FC 00 01 00 01
          move #LATE_00FF, ($00FF)          ;31 FC 00 FF 00 FF
          move #LATE_0100, (LATE_0100)      ;31 FC 01 00 01 00 - 33FC 0100 00000100
          move #LATE_FF00, (LATE_FF00)      ;31 FC FF 00 FF 00 - 33FC FF00 0000FF00



          move.l d0,$000001         ;23C0 00000001
          move.l d1,$0000FF         ;23C1 000000FF
          move.l d2,$00FF00         ;23C2 0000FF00
          move.l d3,$123456         ;23C3 00123456
          move.l d4,LATE_000001     ;23C4 00000001
          move.l d5,LATE_0000FF     ;23C5 000000FF
          move.l d6,LATE_00FF00     ;23C6 0000FF00
          move.l d7,LATE_123456     ;23C7 00123456

          move.w d0,$000001         ;33C0 00000001
          move.w d1,$0000FF         ;33C1 000000FF
          move.w d2,$00FF00         ;33C2 0000FF00
          move.w d3,$123456         ;33C3 00123456
          move.w d4,LATE_000001     ;33C4 00000001
          move.w d5,LATE_0000FF     ;33C5 000000FF
          move.w d6,LATE_00FF00     ;33C6 0000FF00
          move.w d7,LATE_123456     ;33C7 00123456

          move d0,$000001           ;33C0 00000001
          move d1,$0000FF           ;33C1 000000FF
          move d2,$00FF00           ;33C2 0000FF00
          move d3,$123456           ;33C3 00123456
          move d4,LATE_000001       ;33C4 00000001
          move d5,LATE_0000FF       ;33C5 000000FF
          move d6,LATE_00FF00       ;33C6 0000FF00
          move d7,LATE_123456       ;33C7 00123456


          move.w d0,$180(a0)        ;3140 0180
          move.w d0,$180(a7)        ;3F40 0180
          move.w d7,$180(a0)        ;3147 0180
          move.w d7,$7F80(a7)       ;3F47 7F80

          move d0,$180(a0)          ;3140 0180
          move d0,$180(a7)          ;3F40 0180
          move d7,$180(a0)          ;3147 0180
          move d7,$7F80(a7)         ;3F47 7F80


          move.l #$00000001, ($FF00).w          ;21 FC 00 00 00 01 FF 00
          move.l #$0000FF00, ($0100).w          ;21 FC 00 00 FF 00 01 00
          move.l #$00010000, (LATE_00FF).w      ;21 FC 00 01 00 01 00 FF
          move.l #$FF000000, (LATE_0001).w      ;21 FC FF 00 00 01 00 01
          move.l #LATE_00000001, ($FF00).w      ;21 FC 00 00 00 01 FF 00
          move.l #LATE_0000FF00, ($0100).w      ;21 FC 00 00 FF 00 01 00
          move.l #LATE_00010000, (LATE_00FF).w  ;21 FC 00 01 00 00 00 FF
          move.l #LATE_FF000000, (LATE_0001).w  ;21 FC FF 00 00 00 00 01

          move.l #$00000001, ($FF00)            ;23FC 00000001 0000FF00
          move.l #$0000FF00, ($0100)            ;21FC 0000FF00 0100
          move.l #$00010000, (LATE_00FF)        ;23FC 00010000 000000FF
          move.l #$FF000000, (LATE_0001)        ;23FC FF000000 00000001
          move.l #LATE_00000001, ($FF00)        ;23FC 00000001 0000FF00
          move.l #LATE_0000FF00, ($0100)        ;21FC 0000FF00 0100
          move.l #LATE_00010000, (LATE_00FF)    ;23FC 00010000 000000FF
          move.l #LATE_FF000000, (LATE_0001)    ;23FC FF000000 00000001

          move.w #$0001, ($FF00).w          ;31 FC 00 00 00 01 FF 00
          move.w #$00FF, ($0100).w          ;31 FC 00 00 FF 00 01 00
          move.w #$0100, (LATE_00FF).w      ;31 FC 00 01 00 01 00 FF
          move.w #$FF00, (LATE_0001).w      ;31 FC FF 00 00 01 00 01
          move.w #LATE_0001, ($FF00).w      ;31 FC 00 00 00 01 FF 00
          move.w #LATE_00FF, ($0100).w      ;31 FC 00 00 FF 00 01 00
          move.w #LATE_0100, (LATE_00FF).w  ;31 FC 00 01 00 00 00 FF
          move.w #LATE_FF00, (LATE_0001).w  ;31 FC FF 00 00 00 00 01

          move.w #$0001, ($FF00)            ;33FC 0001 0000FF00
          move.w #$00FF, ($0100)            ;31FC 00FF 0100
          move.w #$0100, (LATE_00FF)        ;33FC 0100 000000FF
          move.w #$FF00, (LATE_0001)        ;33FC FF00 00000001
          move.w #LATE_0001, ($FF00)        ;33FC 0001 0000FF00
          move.w #LATE_00FF, ($0100)        ;31FC 00FF 0100
          move.w #LATE_0100, (LATE_00FF)    ;33FC 0100 000000FF
          move.w #LATE_FF00, (LATE_0001)    ;33FC FF00 00000001

          move #$0001, ($FF00).w            ;31 FC 0001 FF00
          move #$00FF, ($0100).w            ;31 FC 00FF 0100
          move #$0100, (LATE_00FF).w        ;31 FC 0100 00FF
          move #$FF00, (LATE_0001).w        ;31 FC FF00 0001
          move #LATE_0001, ($FF00).w        ;31 FC 0001 FF00
          move #LATE_00FF, ($0100).w        ;31 FC 00FF 0100
          move #LATE_0100, (LATE_00FF).w    ;31 FC 0100 00FF
          move #LATE_FF00, (LATE_0001).w    ;31 FC FF00 0001

          move #$0001, ($FF00)              ;33FC 0001 0000FF00
          move #$00FF, ($0100)              ;31FC 00FF 0100
          move #$0100, (LATE_00FF)          ;33FC 0100 000000FF
          move #$FF00, (LATE_0001)          ;33FC FF00 00000001
          move #LATE_0001, ($FF00)          ;33FC 0001 0000FF00
          move #LATE_00FF, ($0100)          ;31FC 00FF 0100
          move #LATE_0100, (LATE_00FF)      ;33FC 0100 000000FF
          move #LATE_FF00, (LATE_0001)      ;33FC FF00 00000001



          move.b #$01, d7             ;1E 3C 00 01
          move.b #$02, d6             ;1C 3C 00 02
          move.b #$FE, d5             ;1A 3C 00 FE
          move.b #$FF, d4             ;18 3C 00 FF
          move.b #LATE_01, d3         ;16 3C 00 01
          move.b #LATE_02, d2         ;14 3C 00 02
          move.b #LATE_FE, d1         ;12 3C 00 FE
          move.b #LATE_FF, d0         ;10 3C 00 FF

          move.l #$00000001, d7       ;2E 3C 00 00 00 01    - 7E01
          move.l #$0000FF00, d6       ;2C 3C 00 00 FF 00
          move.l #$00010000, d5       ;2A 3C 00 01 00 00
          move.l #$FF000000, d4       ;28 3C FF 00 00 00
          move.l #LATE_00000001, d3   ;26 3C 00 00 00 01
          move.l #LATE_0000FF00, d2   ;24 3C 00 00 FF 00
          move.l #LATE_00010000, d1   ;22 3C 00 01 00 00
          move.l #LATE_FF000000, d0   ;20 3C FF 00 00 00

          move.w #$0001, d7           ;3E 3C 00 00 00 01
          move.w #$00FF, d6           ;3C 3C 00 00 FF 00
          move.w #$0100, d5           ;3A 3C 00 01 00 00
          move.w #$FF00, d4           ;38 3C FF 00 00 00
          move.w #LATE_0001, d3       ;36 3C 00 00 00 01
          move.w #LATE_00FF, d2       ;34 3C 00 00 FF 00
          move.w #LATE_0100, d1       ;32 3C 00 01 00 00
          move.w #LATE_FF00, d0       ;30 3C FF 00 00 00

          move #$0001, d7             ;3E 3C 00 00 00 01
          move #$00FF, d6             ;3C 3C 00 00 FF 00
          move #$0100, d5             ;3A 3C 00 01 00 00
          move #$FF00, d4             ;38 3C FF 00 00 00
          move #LATE_0001, d3         ;36 3C 00 00 00 01
          move #LATE_00FF, d2         ;34 3C 00 00 FF 00
          move #LATE_0100, d1         ;32 3C 00 01 00 00
          move #LATE_FF00, d0         ;30 3C FF 00 00 00



          move.w #$0001, (a7)         ;3E BC 00 01
          move.w #$00FF, (a6)         ;3C BC 00 FF
          move.w #$0100, (a5)         ;3A BC 01 00
          move.w #$FF00, (a4)         ;38 BC FF 00
          move.w #LATE_0001, (a3)     ;36 BC 00 01
          move.w #LATE_00FF, (a2)     ;34 BC 00 FF
          move.w #LATE_0100, (a1)     ;32 BC 01 00
          move.w #LATE_FF00, (a0)     ;30 BC FF 00

          move #$0001, (a7)         ;3E BC 00 01
          move #$00FF, (a6)         ;3C BC 00 FF
          move #$0100, (a5)         ;3A BC 01 00
          move #$FF00, (a4)         ;38 BC FF 00
          move #LATE_0001, (a3)     ;36 BC 00 01
          move #LATE_00FF, (a2)     ;34 BC 00 FF
          move #LATE_0100, (a1)     ;32 BC 01 00
          move #LATE_FF00, (a0)     ;30 BC FF 00


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

          move #$0001, $FF(a0)         ;31 7C 00 01 00 FF
          move #$00FF, $FE(a1)         ;33 7C 00 FF 00 FE
          move #$0100, LATE_02(a2)     ;35 7C 01 00 00 02
          move #$FF00, LATE_01(a3)     ;37 7C FF 00 00 01
          move #LATE_0001, $FF(a4)     ;39 7C 00 01 00 FF
          move #LATE_00FF, $FE(a5)     ;3B 7C 00 FF 00 FE
          move #LATE_0100, LATE_02(a6) ;3D 7C 01 00 00 02
          move #LATE_FF00, LATE_01(a7) ;3F 7C FF 00 00 01



          move.w #$0001, (a7)+           ;3E FC 00 01
          move.w #$00FF, (a6)+           ;3C FC 00 FF
          move.w #$0100, (a5)+           ;3A FC 01 00
          move.w #$FF00, (a4)+           ;38 FC FF 00
          move.w #LATE_0001, (a3)+       ;36 FC 00 01
          move.w #LATE_00FF, (a2)+       ;34 FC 00 FF
          move.w #LATE_0100, (a1)+       ;32 FC 01 00
          move.w #LATE_FF00, (a0)+       ;30 FC FF 00

          move #$0001, (a7)+           ;3E FC 00 01
          move #$00FF, (a6)+           ;3C FC 00 FF
          move #$0100, (a5)+           ;3A FC 01 00
          move #$FF00, (a4)+           ;38 FC FF 00
          move #LATE_0001, (a3)+       ;36 FC 00 01
          move #LATE_00FF, (a2)+       ;34 FC 00 FF
          move #LATE_0100, (a1)+       ;32 FC 01 00
          move #LATE_FF00, (a0)+       ;30 FC FF 00


          move.w #$0001, -(a7)           ;3F 3C 00 01
          move.w #$00FF, -(a6)           ;3D 3C 00 FF
          move.w #$0100, -(a5)           ;3B 3C 01 00
          move.w #$FF00, -(a4)           ;39 3C FF 00
          move.w #LATE_0001, -(a3)       ;37 3C 00 01
          move.w #LATE_00FF, -(a2)       ;35 3C 00 FF
          move.w #LATE_0100, -(a1)       ;33 3C 01 00
          move.w #LATE_FF00, -(a0)       ;31 3C FF 00

          move #$0001, -(a7)           ;3F 3C 00 01
          move #$00FF, -(a6)           ;3D 3C 00 FF
          move #$0100, -(a5)           ;3B 3C 01 00
          move #$FF00, -(a4)           ;39 3C FF 00
          move #LATE_0001, -(a3)       ;37 3C 00 01
          move #LATE_00FF, -(a2)       ;35 3C 00 FF
          move #LATE_0100, -(a1)       ;33 3C 01 00
          move #LATE_FF00, -(a0)       ;31 3C FF 00



          move.w ($0001).w, ($FF00).w           ;31 F8 00 01 FF 00
          move.w ($00FF).w, ($0100).w           ;31 F8 00 FF 01 00
          move.w ($0100).w, (LATE_00FF).w       ;31 F8 01 00 00 FF
          move.w ($FF00).w, (LATE_0001).w       ;31 F8 FF 00 00 01
          move.w (LATE_0001).w, ($FF00).w       ;31 F8 00 01 FF 00
          move.w (LATE_00FF).w, ($0100).w       ;31 F8 00 FF 01 00
          move.w (LATE_0100).w, (LATE_00FF).w   ;31 F8 01 00 00 FF
          move.w (LATE_FF00).w, (LATE_0001).w   ;31 F8 FF 00 00 01

          move.w ($0001), ($FF00).w             ;31 F8 00 01 FF 00
          move.w ($00FF), ($0100).w             ;31 F8 00 FF 01 00
          move.w ($0100), (LATE_00FF).w         ;31 F8 01 00 00 FF
          move.w ($FF00), (LATE_0001).w         ;31F9 0000FF00 0001  <- auto up!
          move.w (LATE_0001), ($FF00).w         ;31F9 00000001 FF00  <- auto up!
          move.w (LATE_00FF), ($0100).w         ;31F9 000000FF 0100  <- auto up!
          move.w (LATE_0100), (LATE_00FF).w     ;31F9 00000100 00FF  <- auto up!
          move.w (LATE_FF00), (LATE_0001).w     ;31F9 0000FF00 0001  <- auto up!

          move.w ($0001).w, ($FF00)             ;33 F8 00 01 FF 00
          move.w ($00FF).w, ($0100)             ;31 F8 00 FF 01 00  <- auto up!
          move.w ($0100).w, (LATE_00FF)         ;33 F8 01 00 00 FF
          move.w ($FF00).w, (LATE_0001)         ;33 F8 FF 00 00 01
          move.w (LATE_0001).w, ($FF00)         ;33 F8 00 01 FF 00
          move.w (LATE_00FF).w, ($0100)         ;31 F8 00 FF 01 00  <- auto up!
          move.w (LATE_0100).w, (LATE_00FF)     ;33 F8 01 00 00 FF
          move.w (LATE_FF00).w, (LATE_0001)     ;33 F8 FF 00 00 01

          move ($0001), ($FF00).w               ;31 F8 00 01 FF 00
          move ($00FF), ($0100).w               ;31 F8 00 FF 01 00
          move ($0100), (LATE_00FF).w           ;31 F8 01 00 00 FF
          move ($FF00), (LATE_0001).w           ;31 F9 FF 00 00 01  <- auto up!
          move (LATE_0001), ($FF00).w           ;31 F9 00 01 FF 00  <- auto up!
          move (LATE_00FF), ($0100).w           ;31 F9 00 FF 01 00  <- auto up!
          move (LATE_0100), (LATE_00FF).w       ;31 F9 01 00 00 FF  <- auto up!
          move (LATE_FF00), (LATE_0001).w       ;31 F9 FF 00 00 01  <- auto up!

          move ($0001).w, ($FF00)               ;33 F8 00 01 FF 00
          move ($00FF).w, ($0100)               ;31 F8 00 FF 01 00  <- auto up!
          move ($0100).w, (LATE_00FF)           ;33 F8 01 00 00 FF
          move ($FF00).w, (LATE_0001)           ;33 F8 FF 00 00 01
          move (LATE_0001).w, ($FF00)           ;33 F8 00 01 FF 00
          move (LATE_00FF).w, ($0100)           ;31 F8 00 FF 01 00  <- auto up!
          move (LATE_0100).w, (LATE_00FF)       ;33 F8 01 00 00 FF
          move (LATE_FF00).w, (LATE_0001)       ;33 F8 FF 00 00 01

          move ($0001), ($FF00)                 ;33 F8 00 01 FF 00
          move ($00FF), ($0100)                 ;31 F8 00 FF 01 00  <- auto up!
          move ($0100), (LATE_00FF)             ;33 F8 01 00 00 FF
          move ($FF00), (LATE_0001)             ;33 F9 FF 00 00 01
          move (LATE_0001), ($FF00)             ;33 F9 00 01 FF 00
          move (LATE_00FF), ($0100)             ;31 F9 00 FF 01 00  <- auto up!
          move (LATE_0100), (LATE_00FF)         ;33 F9 01 00 00 FF
          move (LATE_FF00), (LATE_0001)         ;33 F9 FF 00 00 01


          move.w ($0001).w, ($FF000000).l           ;33 F8 00 01 FF 00 00 00
          move.w ($00FF).w, ($00010000).l           ;33 F8 00 FF 00 01 00 00
          move.w ($0100).w, (LATE_0000FF00).l       ;33 F8 01 00 00 00 FF 00
          move.w ($FF00).w, (LATE_00000001).l       ;33 F8 FF 00 00 00 00 01
          move.w (LATE_0001).w, ($FF000000).l       ;33 F8 00 01 FF 00 00 00
          move.w (LATE_00FF).w, ($00010000).l       ;33 F8 00 FF 00 01 00 00
          move.w (LATE_0100).w, (LATE_0000FF00).l   ;33 F8 01 00 00 00 FF 00
          move.w (LATE_FF00).w, (LATE_00000001).l   ;33 F8 FF 00 00 00 00 01

          move.w ($0001), ($FF000000).l             ;33 F8 00 01 FF 00 00 00
          move.w ($00FF), ($00010000).l             ;33 F8 00 FF 00 01 00 00
          move.w ($0100), (LATE_0000FF00).l         ;33 F8 01 00 00 00 FF 00
          move.w ($FF00), (LATE_00000001).l         ;33 F9 FF 00 00 00 00 01  <
          move.w (LATE_0001), ($FF000000).l         ;33 F9 00 01 FF 00 00 00  <
          move.w (LATE_00FF), ($00010000).l         ;33 F9 00 FF 00 01 00 00  <
          move.w (LATE_0100), (LATE_0000FF00).l     ;33 F9 01 00 00 00 FF 00  <
          move.w (LATE_FF00), (LATE_00000001).l     ;33 F9 FF 00 00 00 00 01  <

          move ($0001), ($FF000000).l               ;33 F8 00 01 FF 00 00 00
          move ($00FF), ($00010000).l               ;33 F8 00 FF 00 01 00 00
          move ($0100), (LATE_0000FF00).l           ;33 F8 01 00 00 00 FF 00
          move ($FF00), (LATE_00000001).l           ;33F9 0000FF00 00000001  <
          move (LATE_0001), ($FF000000).l           ;33F9 00000001 FF000000  <
          move (LATE_00FF), ($00010000).l           ;33F9 000000FF 00010000  <
          move (LATE_0100), (LATE_0000FF00).l       ;33F9 00000100 0000FF00  <
          move (LATE_FF00), (LATE_00000001).l       ;33F9 0000FF00 00000001  <

          move.w ($00000001).l, ($FF00).w           ;31F9 00000001 FF00
          move.w ($0000FF00).l, ($0100).w           ;31F9 0000FF00 0100
          move.w ($00010000).l, (LATE_00FF).w       ;31F9 00010000 00FF
          move.w ($FF000000).l, (LATE_0001).w       ;31F9 FF000000 0001
          move.w (LATE_00000001).l, ($FF00).w       ;31F9 00000001 FF00
          move.w (LATE_0000FF00).l, ($0100).w       ;31F9 0000FF00 0100
          move.w (LATE_00010000).l, (LATE_00FF).w   ;31F9 00010000 00FF
          move.w (LATE_FF000000).l, (LATE_0001).w   ;31F9 FF000000 0001

          move ($00000001).l, ($FF00).w             ;31F9 00000001 FF00
          move ($0000FF00).l, ($0100).w             ;31F9 0000FF00 0100
          move ($00010000).l, (LATE_00FF).w         ;31F9 00010000 00FF
          move ($FF000000).l, (LATE_0001).w         ;31F9 FF000000 0001
          move (LATE_00000001).l, ($FF00).w         ;31F9 00000001 FF00
          move (LATE_0000FF00).l, ($0100).w         ;31F9 0000FF00 0100
          move (LATE_00010000).l, (LATE_00FF).w     ;31F9 00010000 00FF
          move (LATE_FF000000).l, (LATE_0001).w     ;31F9 FF000000 0001

          move.w ($00000001).l, ($FF00)             ;33F9 00000001 0000FF00   <
          move.w ($0000FF00).l, ($0100)             ;31F9 0000FF00 0100
          move.w ($00010000).l, (LATE_00FF)         ;33F9 00010000 000000FF   <
          move.w ($FF000000).l, (LATE_0001)         ;33F9 FF000000 00000001   <
          move.w (LATE_00000001).l, ($FF00)         ;33F9 00000001 0000FF00   <
          move.w (LATE_0000FF00).l, ($0100)         ;31F9 0000FF00 0100
          move.w (LATE_00010000).l, (LATE_00FF)     ;33F9 00010000 000000FF   <
          move.w (LATE_FF000000).l, (LATE_0001)     ;33F9 FF000000 00000001   <

          move ($00000001).l, ($FF00)               ;33F9 00000001 0000FF00   <
          move ($0000FF00).l, ($0100)               ;31F9 0000FF00 0100
          move ($00010000).l, (LATE_00FF)           ;33F9 00010000 000000FF   <
          move ($FF000000).l, (LATE_0001)           ;33F9 FF000000 00000001   <
          move (LATE_00000001).l, ($FF00)           ;33F9 00000001 0000FF00   <
          move (LATE_0000FF00).l, ($0100)           ;31F9 0000FF00 0100
          move (LATE_00010000).l, (LATE_00FF)       ;33F9 00010000 000000FF   <
          move (LATE_FF000000).l, (LATE_0001)       ;33F9 FF000000 00000001   <




          move.w ($0001).w, d0        ;30 38 00 01
          move.w ($00FF).w, d1        ;32 38 00 FF
          move.w ($0100).w, d2        ;34 38 01 00
          move.w ($FF00).w, d3        ;36 38 FF 00
          move.w (LATE_0001).w, d4    ;38 38 00 01
          move.w (LATE_00FF).w, d5    ;3A 38 00 FF
          move.w (LATE_0100).w, d6    ;3C 38 01 00
          move.w (LATE_FF00).w, d7    ;3E 38 FF 00

          move.w ($0001), d0          ;3038 0001      <downgrade
          move.w ($00FF), d1          ;3238 00FF      <downgrade
          move.w ($0100), d2          ;3438 0100      <downgrade
          move.w ($FF00), d3          ;3639 0000FF00  <downgrade
          move.w (LATE_0001), d4      ;3839 00000001
          move.w (LATE_00FF), d5      ;3A39 000000FF
          move.w (LATE_0100), d6      ;3C39 00000100
          move.w (LATE_FF00), d7      ;3E39 0000FF00

          move ($0001), d0            ;3038 0001
          move ($00FF), d1            ;3238 00FF
          move ($0100), d2            ;3438 0100
          move ($FF00), d3            ;3639 0000FF00
          move (LATE_0001), d4        ;3839 00000001
          move (LATE_00FF), d5        ;3A39 000000FF
          move (LATE_0100), d6        ;3C39 00000100
          move (LATE_FF00), d7        ;3E39 0000FF00


          move.b ($0001).w, (a0)      ;10 B8 00 01
          move.b ($00FF).w, (a1)      ;12 B8 00 FF
          move.b ($0100).w, (a2)      ;14 B8 01 00
          move.b ($FF00).w, (a3)      ;16 B8 FF 00
          move.b (LATE_0001).w, (a4)  ;18 B8 00 01
          move.b (LATE_00FF).w, (a5)  ;1A B8 00 FF
          move.b (LATE_0100).w, (a6)  ;1C B8 01 00
          move.b (LATE_FF00).w, (a7)  ;1E B8 FF 00

          move.b ($0001), (a0)        ;10B8 0001      <downgrade
          move.b ($00FF), (a1)        ;12B8 00FF      <downgrade
          move.b ($0100), (a2)        ;14B8 0100      <downgrade
          move.b ($FF00), (a3)        ;16B9 0000FF00
          move.b (LATE_0001), (a4)    ;18B9 00000001
          move.b (LATE_00FF), (a5)    ;1AB9 000000FF
          move.b (LATE_0100), (a6)    ;1CB9 00000100
          move.b (LATE_FF00), (a7)    ;1EB9 0000FF00

          move.b ($00000001).l, (a0)      ;10B9 00000001
          move.b ($0000FF00).l, (a1)      ;12B9 0000FF00
          move.b ($00010000).l, (a2)      ;14B9 00010000
          move.b ($FF000000).l, (a3)      ;16B9 FF000000
          move.b (LATE_00000001).l, (a4)  ;18B9 00000001
          move.b (LATE_0000FF00).l, (a5)  ;1AB9 0000FF00
          move.b (LATE_00010000).l, (a6)  ;1CB9 00010000
          move.b (LATE_FF000000).l, (a7)  ;1EB9 FF000000

          move ($00000001).l, (a0)      ;10B9 00000001
          move ($0000FF00).l, (a1)      ;12B9 0000FF00
          move ($00010000).l, (a2)      ;14B9 00010000
          move ($FF000000).l, (a3)      ;16B9 FF000000
          move (LATE_00000001).l, (a4)  ;18B9 00000001
          move (LATE_0000FF00).l, (a5)  ;1AB9 0000FF00
          move (LATE_00010000).l, (a6)  ;1CB9 00010000
          move (LATE_FF000000).l, (a7)  ;1EB9 FF000000



          move.b ($0001).w, $FF(a0)         ;1178 0001 00FF
          move.b ($00FF).w, $01(a1)         ;1378 00FF 0001
          move.b ($0100).w, LATE_FF(a2)     ;1578 0100 00FF
          move.b ($FF00).w, LATE_01(a3)     ;1778 FF00 0001
          move.b (LATE_0001).w, $FF(a4)     ;1978 0001 00FF
          move.b (LATE_00FF).w, $01(a5)     ;1B78 00FF 0001
          move.b (LATE_0100).w, LATE_FF(a6) ;1D78 0100 00FF
          move.b (LATE_FF00).w, LATE_01(a7) ;1F78 FF00 0001

          move.b ($0001), $FF(a0)           ;1178 0001 00FF     <downgrade
          move.b ($00FF), $01(a1)           ;1378 00FF 0001     <downgrade
          move.b ($0100), LATE_FF(a2)       ;1578 0100 00FF     <downgrade
          move.b ($FF00), LATE_01(a3)       ;1779 0000FF00 0001
          move.b (LATE_0001), $FF(a4)       ;1979 00000001 00FF
          move.b (LATE_00FF), $01(a5)       ;1B79 000000FF 0001
          move.b (LATE_0100), LATE_FF(a6)   ;1D79 00000100 00FF
          move.b (LATE_FF00), LATE_01(a7)   ;1F79 0000FF00 0001

          move ($0001).w, $FF(a0)           ;3178 0001 00FF
          move ($00FF).w, $01(a1)           ;3378 00FF 0001
          move ($0100).w, LATE_FF(a2)       ;3578 0100 00FF
          move ($FF00).w, LATE_01(a3)       ;3778 FF00 0001
          move (LATE_0001).w, $FF(a4)       ;3978 0001 00FF
          move (LATE_00FF).w, $01(a5)       ;3B78 00FF 0001
          move (LATE_0100).w, LATE_FF(a6)   ;3D78 0100 00FF
          move (LATE_FF00).w, LATE_01(a7)   ;3F78 FF00 0001

          move ($0001), $FF(a0)             ;3178 0001 00FF     <downgrade
          move ($00FF), $01(a1)             ;3378 00FF 0001     <downgrade
          move ($0100), LATE_FF(a2)         ;3578 0100 00FF     <downgrade
          move ($FF00), LATE_01(a3)         ;3779 0000FF00 0001
          move (LATE_0001), $FF(a4)         ;3979 00000001 00FF
          move (LATE_00FF), $01(a5)         ;3B79 000000FF 0001
          move (LATE_0100), LATE_FF(a6)     ;3D79 00000100 00FF
          move (LATE_FF00), LATE_01(a7)     ;3F79 0000FF00 0001


          move.b ($0001).w, (a0)+     ;10F8 0001
          move.b ($00FF).w, (a1)+     ;12F8 00FF
          move.b ($0100).w, (a2)+     ;14F8 0100
          move.b ($FF00).w, (a3)+     ;16F8 FF00
          move.b (LATE_0001).w, (a4)+ ;18F8 0001
          move.b (LATE_00FF).w, (a5)+ ;1AF8 00FF
          move.b (LATE_0100).w, (a6)+ ;1CF8 0100
          move.b (LATE_FF00).w, (a7)+ ;1EF8 FF00

          move.b ($0001), (a0)+       ;10F8 0001     <downgrade
          move.b ($00FF), (a1)+       ;12F8 00FF     <downgrade
          move.b ($0100), (a2)+       ;14F8 0100     <downgrade
          move.b ($FF00), (a3)+       ;16F9 0000FF00
          move.b (LATE_0001), (a4)+   ;18F9 00000001
          move.b (LATE_00FF), (a5)+   ;1AF9 000000FF
          move.b (LATE_0100), (a6)+   ;1CF9 00000100
          move.b (LATE_FF00), (a7)+   ;1EF9 0000FF00

          move ($0001).w, (a0)+       ;30F8 0001
          move ($00FF).w, (a1)+       ;32F8 00FF
          move ($0100).w, (a2)+       ;34F8 0100
          move ($FF00).w, (a3)+       ;36F8 FF00
          move (LATE_0001).w, (a4)+   ;38F8 0001
          move (LATE_00FF).w, (a5)+   ;3AF8 00FF
          move (LATE_0100).w, (a6)+   ;3CF8 0100
          move (LATE_FF00).w, (a7)+   ;3EF8 FF00

          move ($0001), (a0)+         ;30F8 0001     <downgrade
          move ($00FF), (a1)+         ;32F8 00FF     <downgrade
          move ($0100), (a2)+         ;34F8 0100     <downgrade
          move ($FF00), (a3)+         ;36F9 0000FF00
          move (LATE_0001), (a4)+     ;38F9 00000001
          move (LATE_00FF), (a5)+     ;3AF9 000000FF
          move (LATE_0100), (a6)+     ;3CF9 00000100
          move (LATE_FF00), (a7)+     ;3EF9 0000FF00



          move.b ($0001).w, -(a0)     ;1138 0001
          move.b ($00FF).w, -(a1)     ;1338 00FF
          move.b ($0100).w, -(a2)     ;1538 0100
          move.b ($FF00).w, -(a3)     ;1738 FF00
          move.b (LATE_0001).w, -(a4) ;1938 0001
          move.b (LATE_00FF).w, -(a5) ;1B38 00FF
          move.b (LATE_0100).w, -(a6) ;1D38 0100
          move.b (LATE_FF00).w, -(a7) ;1F38 FF00

          move.b ($0001), -(a0)       ;1138 0001     <downgrade
          move.b ($00FF), -(a1)       ;1338 00FF     <downgrade
          move.b ($0100), -(a2)       ;1538 0100     <downgrade
          move.b ($FF00), -(a3)       ;1739 0000FF00
          move.b (LATE_0001), -(a4)   ;1939 00000001
          move.b (LATE_00FF), -(a5)   ;1B39 000000FF
          move.b (LATE_0100), -(a6)   ;1D39 00000100
          move.b (LATE_FF00), -(a7)   ;1F39 0000FF00

          move ($0001).w, -(a0)       ;3138 0001
          move ($00FF).w, -(a1)       ;3338 00FF
          move ($0100).w, -(a2)       ;3538 0100
          move ($FF00).w, -(a3)       ;3738 FF00
          move (LATE_0001).w, -(a4)   ;3938 0001
          move (LATE_00FF).w, -(a5)   ;3B38 00FF
          move (LATE_0100).w, -(a6)   ;3D38 0100
          move (LATE_FF00).w, -(a7)   ;3F38 FF00

          move ($0001), -(a0)         ;3138 0001     <downgrade
          move ($00FF), -(a1)         ;3338 00FF     <downgrade
          move ($0100), -(a2)         ;3538 0100     <downgrade
          move ($FF00), -(a3)         ;3739 0000FF00
          move (LATE_0001), -(a4)     ;3939 00000001
          move (LATE_00FF), -(a5)     ;3B39 000000FF
          move (LATE_0100), -(a6)     ;3D39 00000100
          move (LATE_FF00), -(a7)     ;3F39 0000FF00



          move.b d0, ($0001).l        ;13C0 00000001
          move.b d1, ($00FF).l        ;13C1 000000FF
          move.b d2, ($0100).l        ;13C2 00000100
          move.b d3, ($FF00).l        ;13C3 0000FF00
          move.b d4, (LATE_0001).l    ;13C4 00000001
          move.b d5, (LATE_00FF).l    ;13C5 000000FF
          move.b d6, (LATE_0100).l    ;13C6 00000100
          move.b d7, (LATE_FF00).l    ;13C7 0000FF00

          move.b d0, ($0001).w        ;11C0 0001
          move.b d1, ($00FF).w        ;11C1 00FF
          move.b d2, ($0100).w        ;11C2 0100
          move.b d3, ($FF00).w        ;11C3 FF00
          move.b d4, (LATE_0001).w    ;11C4 0001
          move.b d5, (LATE_00FF).w    ;11C5 00FF
          move.b d6, (LATE_0100).w    ;11C6 0100
          move.b d7, (LATE_FF00).w    ;11C7 FF00

          move.b d0, ($0001)          ;11C0 0001     <downgrade
          move.b d1, ($00FF)          ;11C1 00FF     <downgrade
          move.b d2, ($0100)          ;11C2 0100     <downgrade
          move.b d3, ($FF00)          ;13C3 0000FF00
          move.b d4, (LATE_0001)      ;13C4 00000001
          move.b d5, (LATE_00FF)      ;13C5 000000FF
          move.b d6, (LATE_0100)      ;13C6 00000100
          move.b d7, (LATE_FF00)      ;13C7 0000FF00


          move.l d0, ($00000001).l        ;23C0 00000001
          move.l d1, ($0000FF00).l        ;23C1 0000FF00
          move.l d2, ($00010000).l        ;23C2 00010000
          move.l d3, ($FF000000).l        ;23C3 FF000000
          move.l d4, (LATE_00000001).l    ;23C4 00000001
          move.l d5, (LATE_0000FF00).l    ;23C5 0000FF00
          move.l d6, (LATE_00010000).l    ;23C6 00010000
          move.l d7, (LATE_FF000000).l    ;23C7 FF000000

          move.l d0, ($00000001)          ;21C0 0001        <downgrade
          move.l d1, ($0000FF00)          ;23C1 0000FF00
          move.l d2, ($00010000)          ;23C2 00010000
          move.l d3, ($FF000000)          ;23C3 FF000000
          move.l d4, (LATE_00000001)      ;23C4 00000001
          move.l d5, (LATE_0000FF00)      ;23C5 0000FF00
          move.l d6, (LATE_00010000)      ;23C6 00010000
          move.l d7, (LATE_FF000000)      ;23C7 FF000000

          move.w d0, ($00000001)          ;31C0 0001        <downgrade
          move.w d1, ($0000FF00)          ;33C1 0000FF00
          move.w d2, ($00010000)          ;33C2 00010000
          move.w d3, ($FF000000)          ;33C3 FF000000
          move.w d4, (LATE_00000001)      ;33C4 00000001
          move.w d5, (LATE_0000FF00)      ;33C5 0000FF00
          move.w d6, (LATE_00010000)      ;33C6 00010000
          move.w d7, (LATE_FF000000)      ;33C7 FF000000

          move d0, ($00000001)            ;31C0 0001        <downgrade
          move d1, ($0000FF00)            ;33C1 0000FF00
          move d2, ($00010000)            ;33C2 00010000
          move d3, ($FF000000)            ;33C3 FF000000
          move d4, (LATE_00000001)        ;33C4 00000001
          move d5, (LATE_0000FF00)        ;33C5 0000FF00
          move d6, (LATE_00010000)        ;33C6 00010000
          move d7, (LATE_FF000000)        ;33C7 FF000000


          move.l d0, $0001(a7)            ;2F40 0001
          move.l d1, $00FF(a6)            ;2D41 00FF
          move.l d2, $0100(a5)            ;2B42 0100
          move.l d3, $7F00(a4)            ;2943 7F00
          move.l d4, LATE_0001(a3)        ;2744 0001
          move.l d5, LATE_00FF(a2)        ;2545 00FF
          move.l d6, LATE_0100(a1)        ;2346 0100
          move.l d7, LATE_7F00(a0)        ;2147 7F00

          move.w d0, $0001(a7)            ;3F40 0001
          move.w d1, $00FF(a6)            ;3D41 00FF
          move.w d2, $0100(a5)            ;3B42 0100
          move.w d3, $7F00(a4)            ;3943 7F00
          move.w d4, LATE_0001(a3)        ;3744 0001
          move.w d5, LATE_00FF(a2)        ;3545 00FF
          move.w d6, LATE_0100(a1)        ;3346 0100
          move.w d7, LATE_7F00(a0)        ;3147 7F00

          move.b d0, $0001(a7)            ;1F40 0001
          move.b d1, $00FF(a6)            ;1D41 00FF
          move.b d2, $0100(a5)            ;1B42 0100
          move.b d3, $7F00(a4)            ;1943 7F00
          move.b d4, LATE_0001(a3)        ;1744 0001
          move.b d5, LATE_00FF(a2)        ;1545 00FF
          move.b d6, LATE_0100(a1)        ;1346 0100
          move.b d7, LATE_7F00(a0)        ;1147 7F00



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

          move.b d0, d7                ;1E 00
          move.b d1, d6                ;1C 01
          move.b d2, d5                ;1A 02
          move.b d3, d4                ;18 03
          move.b d4, d3                ;16 04
          move.b d5, d2                ;14 05
          move.b d6, d1                ;12 06
          move.b d7, d0                ;10 07

          move d0, d7                  ;3E 00
          move d1, d6                  ;3C 01
          move d2, d5                  ;3A 02
          move d3, d4                  ;38 03
          move d4, d3                  ;36 04
          move d5, d2                  ;34 05
          move d6, d1                  ;32 06
          move d7, d0                  ;30 07


          move.b d0, (a7)              ;1E 80
          move.b d1, (a6)              ;1C 81
          move.b d2, (a5)              ;1A 82
          move.b d3, (a4)              ;18 83
          move.b d4, (a3)              ;16 84
          move.b d5, (a2)              ;14 85
          move.b d6, (a1)              ;12 86
          move.b d7, (a0)              ;10 87

          move.w d0, (a7)              ;3E 80
          move.w d1, (a6)              ;3C 81
          move.w d2, (a5)              ;3A 82
          move.w d3, (a4)              ;38 83
          move.w d4, (a3)              ;36 84
          move.w d5, (a2)              ;34 85
          move.w d6, (a1)              ;32 86
          move.w d7, (a0)              ;30 87

          move.l d0, (a7)              ;2E 80
          move.l d1, (a6)              ;2C 81
          move.l d2, (a5)              ;2A 82
          move.l d3, (a4)              ;28 83
          move.l d4, (a3)              ;26 84
          move.l d5, (a2)              ;24 85
          move.l d6, (a1)              ;22 86
          move.l d7, (a0)              ;20 87

          move d0, (a7)                ;3E 80
          move d1, (a6)                ;3C 81
          move d2, (a5)                ;3A 82
          move d3, (a4)                ;38 83
          move d4, (a3)                ;36 84
          move d5, (a2)                ;34 85
          move d6, (a1)                ;32 86
          move d7, (a0)                ;30 87


          move.b d0, $01(a7)           ;1F 40 00 01
          move.b d1, $FF(a6)           ;1D 41 00 FF
          move.b d2, LATE_01(a5)       ;1B 42 00 01
          move.b d3, LATE_FF(a4)       ;19 43 00 FF
          move.b d4, $01(a3)           ;17 44 00 01
          move.b d5, $FF(a2)           ;15 45 00 FF
          move.b d6, LATE_01(a1)       ;13 46 00 01
          move.b d7, LATE_FF(a0)       ;11 47 00 FF

          move.w d0, $01(a7)           ;3F 40 00 01
          move.w d1, $FF(a6)           ;3D 41 00 FF
          move.w d2, LATE_01(a5)       ;3B 42 00 01
          move.w d3, LATE_FF(a4)       ;39 43 00 FF
          move.w d4, $01(a3)           ;37 44 00 01
          move.w d5, $FF(a2)           ;35 45 00 FF
          move.w d6, LATE_01(a1)       ;33 46 00 01
          move.w d7, LATE_FF(a0)       ;31 47 00 FF

          move.l d0, $01(a7)           ;2F 40 00 01
          move.l d1, $FF(a6)           ;2D 41 00 FF
          move.l d2, LATE_01(a5)       ;2B 42 00 01
          move.l d3, LATE_FF(a4)       ;29 43 00 FF
          move.l d4, $01(a3)           ;27 44 00 01
          move.l d5, $FF(a2)           ;25 45 00 FF
          move.l d6, LATE_01(a1)       ;23 46 00 01
          move.l d7, LATE_FF(a0)       ;21 47 00 FF

          move d0, $01(a7)             ;3F 40 00 01
          move d1, $FF(a6)             ;3D 41 00 FF
          move d2, LATE_01(a5)         ;3B 42 00 01
          move d3, LATE_FF(a4)         ;39 43 00 FF
          move d4, $01(a3)             ;37 44 00 01
          move d5, $FF(a2)             ;35 45 00 FF
          move d6, LATE_01(a1)         ;33 46 00 01
          move d7, LATE_FF(a0)         ;31 47 00 FF



          move.b d0, (a7)+             ;1E C0
          move.b d1, (a6)+             ;1C C1
          move.b d2, (a5)+             ;1A C2
          move.b d3, (a4)+             ;18 C3
          move.b d4, (a3)+             ;16 C4
          move.b d5, (a2)+             ;14 C5
          move.b d6, (a1)+             ;12 C6
          move.b d7, (a0)+             ;10 C7

          move.w d0, (a7)+             ;3E C0
          move.w d1, (a6)+             ;3C C1
          move.w d2, (a5)+             ;3A C2
          move.w d3, (a4)+             ;38 C3
          move.w d4, (a3)+             ;36 C4
          move.w d5, (a2)+             ;34 C5
          move.w d6, (a1)+             ;32 C6
          move.w d7, (a0)+             ;30 C7

          move.l d0, (a7)+             ;2E C0
          move.l d1, (a6)+             ;2C C1
          move.l d2, (a5)+             ;2A C2
          move.l d3, (a4)+             ;28 C3
          move.l d4, (a3)+             ;26 C4
          move.l d5, (a2)+             ;24 C5
          move.l d6, (a1)+             ;22 C6
          move.l d7, (a0)+             ;20 C7

          move d0, (a7)+               ;3E C0
          move d1, (a6)+               ;3C C1
          move d2, (a5)+               ;3A C2
          move d3, (a4)+               ;38 C3
          move d4, (a3)+               ;36 C4
          move d5, (a2)+               ;34 C5
          move d6, (a1)+               ;32 C6
          move d7, (a0)+               ;30 C7


          move.b d0, -(a7)             ;1F 00
          move.b d1, -(a6)             ;1D 01
          move.b d2, -(a5)             ;1B 02
          move.b d3, -(a4)             ;19 03
          move.b d4, -(a3)             ;17 04
          move.b d5, -(a2)             ;15 05
          move.b d6, -(a1)             ;13 06
          move.b d7, -(a0)             ;11 07

          move.w d0, -(a7)             ;3F 00
          move.w d1, -(a6)             ;3D 01
          move.w d2, -(a5)             ;3B 02
          move.w d3, -(a4)             ;39 03
          move.w d4, -(a3)             ;37 04
          move.w d5, -(a2)             ;35 05
          move.w d6, -(a1)             ;33 06
          move.w d7, -(a0)             ;31 07

          move.l d0, -(a7)             ;2F 00
          move.l d1, -(a6)             ;2D 01
          move.l d2, -(a5)             ;2B 02
          move.l d3, -(a4)             ;29 03
          move.l d4, -(a3)             ;27 04
          move.l d5, -(a2)             ;25 05
          move.l d6, -(a1)             ;23 06
          move.l d7, -(a0)             ;21 07

          move d0, -(a7)               ;3F 00
          move d1, -(a6)               ;3D 01
          move d2, -(a5)               ;3B 02
          move d3, -(a4)               ;39 03
          move d4, -(a3)               ;37 04
          move d5, -(a2)               ;35 05
          move d6, -(a1)               ;33 06
          move d7, -(a0)               ;31 07




          move.b (a0), d7              ;1E 10
          move.b (a1), d6              ;1C 11
          move.b (a2), d5              ;1A 12
          move.b (a3), d4              ;18 13
          move.b (a4), d3              ;16 14
          move.b (a5), d2              ;14 15
          move.b (a6), d1              ;12 16
          move.b (a7), d0              ;10 17

          move.w (a0), d7              ;3E 10
          move.w (a1), d6              ;3C 11
          move.w (a2), d5              ;3A 12
          move.w (a3), d4              ;38 13
          move.w (a4), d3              ;36 14
          move.w (a5), d2              ;34 15
          move.w (a6), d1              ;32 16
          move.w (a7), d0              ;30 17

          move.l (a0), d7              ;2E 10
          move.l (a1), d6              ;2C 11
          move.l (a2), d5              ;2A 12
          move.l (a3), d4              ;28 13
          move.l (a4), d3              ;26 14
          move.l (a5), d2              ;24 15
          move.l (a6), d1              ;22 16
          move.l (a7), d0              ;20 17

          move (a0), d7                ;3E 10
          move (a1), d6                ;3C 11
          move (a2), d5                ;3A 12
          move (a3), d4                ;38 13
          move (a4), d3                ;36 14
          move (a5), d2                ;34 15
          move (a6), d1                ;32 16
          move (a7), d0                ;30 17



          move.b (a0), (a7)            ;1E 90
          move.b (a1), (a6)            ;1C 91
          move.b (a2), (a5)            ;1A 92
          move.b (a3), (a4)            ;18 93
          move.b (a4), (a3)            ;16 94
          move.b (a5), (a2)            ;14 95
          move.b (a6), (a1)            ;12 96
          move.b (a7), (a0)            ;10 97

          move.w (a0), (a7)            ;3E 90
          move.w (a1), (a6)            ;3C 91
          move.w (a2), (a5)            ;3A 92
          move.w (a3), (a4)            ;38 93
          move.w (a4), (a3)            ;36 94
          move.w (a5), (a2)            ;34 95
          move.w (a6), (a1)            ;32 96
          move.w (a7), (a0)            ;30 97

          move.l (a0), (a7)            ;2E 90
          move.l (a1), (a6)            ;2C 91
          move.l (a2), (a5)            ;2A 92
          move.l (a3), (a4)            ;28 93
          move.l (a4), (a3)            ;26 94
          move.l (a5), (a2)            ;24 95
          move.l (a6), (a1)            ;22 96
          move.l (a7), (a0)            ;20 97

          move (a0), (a7)              ;3E 90
          move (a1), (a6)              ;3C 91
          move (a2), (a5)              ;3A 92
          move (a3), (a4)              ;38 93
          move (a4), (a3)              ;36 94
          move (a5), (a2)              ;34 95
          move (a6), (a1)              ;32 96
          move (a7), (a0)              ;30 97



          move.b (a0), $01(a7)         ;1F50 0001
          move.b (a1), $FF(a6)         ;1D51 00FF
          move.b (a2), LATE_01(a5)     ;1B52 0001
          move.b (a3), LATE_FF(a4)     ;1953 00FF
          move.b (a4), $01(a3)         ;1754 0001
          move.b (a5), $FF(a2)         ;1555 00FF
          move.b (a6), LATE_01(a1)     ;1356 0001
          move.b (a7), LATE_FF(a0)     ;1157 00FF

          move.w (a0), $01(a7)         ;3F50 0001
          move.w (a1), $FF(a6)         ;3D51 00FF
          move.w (a2), LATE_01(a5)     ;3B52 0001
          move.w (a3), LATE_FF(a4)     ;3953 00FF
          move.w (a4), $01(a3)         ;3754 0001
          move.w (a5), $FF(a2)         ;3555 00FF
          move.w (a6), LATE_01(a1)     ;3356 0001
          move.w (a7), LATE_FF(a0)     ;3157 00FF

          move.l (a0), $01(a7)         ;2F50 0001
          move.l (a1), $FF(a6)         ;2D51 00FF
          move.l (a2), LATE_01(a5)     ;2B52 0001
          move.l (a3), LATE_FF(a4)     ;2953 00FF
          move.l (a4), $01(a3)         ;2754 0001
          move.l (a5), $FF(a2)         ;2555 00FF
          move.l (a6), LATE_01(a1)     ;2356 0001
          move.l (a7), LATE_FF(a0)     ;2157 00FF

          move (a0), $01(a7)           ;3F50 0001
          move (a1), $FF(a6)           ;3D51 00FF
          move (a2), LATE_01(a5)       ;3B52 0001
          move (a3), LATE_FF(a4)       ;3953 00FF
          move (a4), $01(a3)           ;3754 0001
          move (a5), $FF(a2)           ;3555 00FF
          move (a6), LATE_01(a1)       ;3356 0001
          move (a7), LATE_FF(a0)       ;3157 00FF


          move.b (a0), (a7)+           ;1E D0
          move.b (a1), (a6)+           ;1C D1
          move.b (a2), (a5)+           ;1A D2
          move.b (a3), (a4)+           ;18 D3
          move.b (a4), (a3)+           ;16 D4
          move.b (a5), (a2)+           ;14 D5
          move.b (a6), (a1)+           ;12 D6
          move.b (a7), (a0)+           ;10 D7

          move.w (a0), (a7)+           ;3E D0
          move.w (a1), (a6)+           ;3C D1
          move.w (a2), (a5)+           ;3A D2
          move.w (a3), (a4)+           ;38 D3
          move.w (a4), (a3)+           ;36 D4
          move.w (a5), (a2)+           ;34 D5
          move.w (a6), (a1)+           ;32 D6
          move.w (a7), (a0)+           ;30 D7

          move.l (a0), (a7)+           ;2E D0
          move.l (a1), (a6)+           ;2C D1
          move.l (a2), (a5)+           ;2A D2
          move.l (a3), (a4)+           ;28 D3
          move.l (a4), (a3)+           ;26 D4
          move.l (a5), (a2)+           ;24 D5
          move.l (a6), (a1)+           ;22 D6
          move.l (a7), (a0)+           ;20 D7

          move (a0), (a7)+             ;3E D0
          move (a1), (a6)+             ;3C D1
          move (a2), (a5)+             ;3A D2
          move (a3), (a4)+             ;38 D3
          move (a4), (a3)+             ;36 D4
          move (a5), (a2)+             ;34 D5
          move (a6), (a1)+             ;32 D6
          move (a7), (a0)+             ;30 D7


          move.b (a0), -(a7)           ;1F 10
          move.b (a1), -(a6)           ;1D 11
          move.b (a2), -(a5)           ;1B 12
          move.b (a3), -(a4)           ;19 13
          move.b (a4), -(a3)           ;17 14
          move.b (a5), -(a2)           ;15 15
          move.b (a6), -(a1)           ;13 16
          move.b (a7), -(a0)           ;11 17

          move.w (a0), -(a7)           ;3F 10
          move.w (a1), -(a6)           ;3D 11
          move.w (a2), -(a5)           ;3B 12
          move.w (a3), -(a4)           ;39 13
          move.w (a4), -(a3)           ;37 14
          move.w (a5), -(a2)           ;35 15
          move.w (a6), -(a1)           ;33 16
          move.w (a7), -(a0)           ;31 17

          move.l (a0), -(a7)           ;2F 10
          move.l (a1), -(a6)           ;2D 11
          move.l (a2), -(a5)           ;2B 12
          move.l (a3), -(a4)           ;29 13
          move.l (a4), -(a3)           ;27 14
          move.l (a5), -(a2)           ;25 15
          move.l (a6), -(a1)           ;23 16
          move.l (a7), -(a0)           ;21 17

          move (a0), -(a7)             ;3F 10
          move (a1), -(a6)             ;3D 11
          move (a2), -(a5)             ;3B 12
          move (a3), -(a4)             ;39 13
          move (a4), -(a3)             ;37 14
          move (a5), -(a2)             ;35 15
          move (a6), -(a1)             ;33 16
          move (a7), -(a0)             ;31 17



          move.w $01(a7), ($FF00).w           ;31EF 0001 FF00
          move.w $02(a6), ($0100).w           ;31EE 0002 0100
          move.w $FE(a5), ($00FF).w           ;31ED 00FE 00FF
          move.w $FF(a4), ($0001).w           ;31EC 00FF 0001
          move.w LATE_01(a3), (LATE_FF00).w   ;31EB 0001 FF00
          move.w LATE_02(a2), (LATE_0100).w   ;31EA 0002 0100
          move.w LATE_FE(a1), (LATE_00FF).w   ;31E9 00FE 00FF
          move.w LATE_FF(a0), (LATE_0001).w   ;31E8 00FF 0001

          move.w $01(a7), ($FF00)             ;33EF 0001 0000FF00   << downgrade
          move.w $02(a6), ($0100)             ;31EE 0002 0100
          move.w $FE(a5), ($00FF)             ;31ED 00FE 00FF
          move.w $FF(a4), ($0001)             ;31EC 00FF 0001
          move.w LATE_01(a3), (LATE_FF00)     ;33EB 0001 0000FF00   << downgrade
          move.w LATE_02(a2), (LATE_0100)     ;33EA 0002 00000100   << downgrade
          move.w LATE_FE(a1), (LATE_00FF)     ;33E9 00FE 000000FF   << downgrade
          move.w LATE_FF(a0), (LATE_0001)     ;33E8 00FF 00000001   << downgrade

          move $01(a7), ($FF00).w             ;31EF 0001 FF00
          move $02(a6), ($0100).w             ;31EE 0002 0100
          move $FE(a5), ($00FF).w             ;31ED 00FE 00FF
          move $FF(a4), ($0001).w             ;31EC 00FF 0001
          move LATE_01(a3), (LATE_FF00).w     ;31EB 0001 FF00
          move LATE_02(a2), (LATE_0100).w     ;31EA 0002 0100
          move LATE_FE(a1), (LATE_00FF).w     ;31E9 00FE 00FF
          move LATE_FF(a0), (LATE_0001).w     ;31E8 00FF 0001

          move $01(a7), ($FF00)               ;33EF 0001 0000FF00   << downgrade
          move $02(a6), ($0100)               ;31EE 0002 0100
          move $FE(a5), ($00FF)               ;31ED 00FE 00FF
          move $FF(a4), ($0001)               ;31EC 00FF 0001
          move LATE_01(a3), (LATE_FF00)       ;33EB 0001 0000FF00   << downgrade
          move LATE_02(a2), (LATE_0100)       ;33EA 0002 00000100   << downgrade
          move LATE_FE(a1), (LATE_00FF)       ;33E9 00FE 000000FF   << downgrade
          move LATE_FF(a0), (LATE_0001)       ;33E8 00FF 00000001   << downgrade



          move.b $01(a0),d7        ;1E28 0001
          move.b $02(a1),d6        ;1C29 0002
          move.b $FE(a2),d5        ;1A2A 00FE
          move.b $FF(a3),d4        ;182B 00FF
          move.b LATE_01(a4),d3    ;162C 0001
          move.b LATE_02(a5),d2    ;142D 0002
          move.b LATE_FE(a6),d1    ;122E 00FE
          move.b LATE_FF(a7),d0    ;102F 00FF

          move.w $01(a0),d7        ;3E28 0001
          move.w $02(a1),d6        ;3C29 0002
          move.w $FE(a2),d5        ;3A2A 00FE
          move.w $FF(a3),d4        ;382B 00FF
          move.w LATE_01(a4),d3    ;362C 0001
          move.w LATE_02(a5),d2    ;342D 0002
          move.w LATE_FE(a6),d1    ;322E 00FE
          move.w LATE_FF(a7),d0    ;302F 00FF

          move.l $01(a0),d7        ;2E28 0001
          move.l $02(a1),d6        ;2C29 0002
          move.l $FE(a2),d5        ;2A2A 00FE
          move.l $FF(a3),d4        ;282B 00FF
          move.l LATE_01(a4),d3    ;262C 0001
          move.l LATE_02(a5),d2    ;242D 0002
          move.l LATE_FE(a6),d1    ;222E 00FE
          move.l LATE_FF(a7),d0    ;202F 00FF

          move $01(a0),d7          ;3E28 0001
          move $02(a1),d6          ;3C29 0002
          move $FE(a2),d5          ;3A2A 00FE
          move $FF(a3),d4          ;382B 00FF
          move LATE_01(a4),d3      ;362C 0001
          move LATE_02(a5),d2      ;342D 0002
          move LATE_FE(a6),d1      ;322E 00FE
          move LATE_FF(a7),d0      ;302F 00FF



          move.b $01(a0), (a7)        ;1EA8 0001
          move.b $02(a1), (a6)        ;1CA9 0002
          move.b $FE(a2), (a5)        ;1AAA 00FE
          move.b $FF(a3), (a4)        ;18AB 00FF
          move.b LATE_01(a4), (a3)    ;16AC 0001
          move.b LATE_02(a5), (a2)    ;14AD 0002
          move.b LATE_FE(a6), (a1)    ;12AE 00FE
          move.b LATE_FF(a7), (a0)    ;10AF 00FF

          move.w $01(a0), (a7)        ;3EA8 0001
          move.w $02(a1), (a6)        ;3CA9 0002
          move.w $FE(a2), (a5)        ;3AAA 00FE
          move.w $FF(a3), (a4)        ;38AB 00FF
          move.w LATE_01(a4), (a3)    ;36AC 0001
          move.w LATE_02(a5), (a2)    ;34AD 0002
          move.w LATE_FE(a6), (a1)    ;32AE 00FE
          move.w LATE_FF(a7), (a0)    ;30AF 00FF

          move.l $01(a0), (a7)        ;2EA8 0001
          move.l $02(a1), (a6)        ;2CA9 0002
          move.l $FE(a2), (a5)        ;2AAA 00FE
          move.l $FF(a3), (a4)        ;28AB 00FF
          move.l LATE_01(a4), (a3)    ;26AC 0001
          move.l LATE_02(a5), (a2)    ;24AD 0002
          move.l LATE_FE(a6), (a1)    ;22AE 00FE
          move.l LATE_FF(a7), (a0)    ;20AF 00FF

          move $01(a0), (a7)          ;3EA8 0001
          move $02(a1), (a6)          ;3CA9 0002
          move $FE(a2), (a5)          ;3AAA 00FE
          move $FF(a3), (a4)          ;38AB 00FF
          move LATE_01(a4), (a3)      ;36AC 0001
          move LATE_02(a5), (a2)      ;34AD 0002
          move LATE_FE(a6), (a1)      ;32AE 00FE
          move LATE_FF(a7), (a0)      ;30AF 00FF



          move.b $01(a0), $FF(a7)           ;1F68 0001 00FF
          move.b $02(a1), $FE(a6)           ;1D69 0002 00FE
          move.b $FE(a2), LATE_02(a5)       ;1B6A 00FE 0002
          move.b $FF(a3), LATE_01(a4)       ;196B 00FF 0001
          move.b LATE_01(a4), $FF(a3)       ;176C 0001 00FF
          move.b LATE_02(a5), $FE(a2)       ;156D 0002 00FE
          move.b LATE_FE(a6), LATE_02(a1)   ;136E 00FE 0002
          move.b LATE_FF(a7), LATE_01(a0)   ;116F 00FF 0001

          move.w $01(a0), $FF(a7)           ;3F68 0001 00FF
          move.w $02(a1), $FE(a6)           ;3D69 0002 00FE
          move.w $FE(a2), LATE_02(a5)       ;3B6A 00FE 0002
          move.w $FF(a3), LATE_01(a4)       ;396B 00FF 0001
          move.w LATE_01(a4), $FF(a3)       ;376C 0001 00FF
          move.w LATE_02(a5), $FE(a2)       ;356D 0002 00FE
          move.w LATE_FE(a6), LATE_02(a1)   ;336E 00FE 0002
          move.w LATE_FF(a7), LATE_01(a0)   ;316F 00FF 0001

          move.l $01(a0), $FF(a7)           ;2F68 0001 00FF
          move.l $02(a1), $FE(a6)           ;2D69 0002 00FE
          move.l $FE(a2), LATE_02(a5)       ;2B6A 00FE 0002
          move.l $FF(a3), LATE_01(a4)       ;296B 00FF 0001
          move.l LATE_01(a4), $FF(a3)       ;276C 0001 00FF
          move.l LATE_02(a5), $FE(a2)       ;256D 0002 00FE
          move.l LATE_FE(a6), LATE_02(a1)   ;236E 00FE 0002
          move.l LATE_FF(a7), LATE_01(a0)   ;216F 00FF 0001

          move $01(a0), $FF(a7)             ;3F68 0001 00FF
          move $02(a1), $FE(a6)             ;3D69 0002 00FE
          move $FE(a2), LATE_02(a5)         ;3B6A 00FE 0002
          move $FF(a3), LATE_01(a4)         ;396B 00FF 0001
          move LATE_01(a4), $FF(a3)         ;376C 0001 00FF
          move LATE_02(a5), $FE(a2)         ;356D 0002 00FE
          move LATE_FE(a6), LATE_02(a1)     ;336E 00FE 0002
          move LATE_FF(a7), LATE_01(a0)     ;316F 00FF 0001


          move.b $01(a0), (a7)+         ;1EE8 0001
          move.b $02(a1), (a6)+         ;1CE9 0002
          move.b $FE(a2), (a5)+         ;1AEA 00FE
          move.b $FF(a3), (a4)+         ;18EB 00FF
          move.b LATE_01(a4), (a3)+     ;16EC 0001
          move.b LATE_02(a5), (a2)+     ;14ED 0002
          move.b LATE_FE(a6), (a1)+     ;12EE 00FE
          move.b LATE_FF(a7), (a0)+     ;10EF 00FF

          move.w $01(a0), (a7)+         ;3EE8 0001
          move.w $02(a1), (a6)+         ;3CE9 0002
          move.w $FE(a2), (a5)+         ;3AEA 00FE
          move.w $FF(a3), (a4)+         ;38EB 00FF
          move.w LATE_01(a4), (a3)+     ;36EC 0001
          move.w LATE_02(a5), (a2)+     ;34ED 0002
          move.w LATE_FE(a6), (a1)+     ;32EE 00FE
          move.w LATE_FF(a7), (a0)+     ;30EF 00FF

          move.l $01(a0), (a7)+         ;2EE8 0001
          move.l $02(a1), (a6)+         ;2CE9 0002
          move.l $FE(a2), (a5)+         ;2AEA 00FE
          move.l $FF(a3), (a4)+         ;28EB 00FF
          move.l LATE_01(a4), (a3)+     ;26EC 0001
          move.l LATE_02(a5), (a2)+     ;24ED 0002
          move.l LATE_FE(a6), (a1)+     ;22EE 00FE
          move.l LATE_FF(a7), (a0)+     ;20EF 00FF

          move $01(a0), (a7)+           ;3EE8 0001
          move $02(a1), (a6)+           ;3CE9 0002
          move $FE(a2), (a5)+           ;3AEA 00FE
          move $FF(a3), (a4)+           ;38EB 00FF
          move LATE_01(a4), (a3)+       ;36EC 0001
          move LATE_02(a5), (a2)+       ;34ED 0002
          move LATE_FE(a6), (a1)+       ;32EE 00FE
          move LATE_FF(a7), (a0)+       ;30EF 00FF



          move.b $01(a0), -(a7)         ;1F28 0001
          move.b $02(a1), -(a6)         ;1D29 0002
          move.b $FE(a2), -(a5)         ;1B2A 00FE
          move.b $FF(a3), -(a4)         ;192B 00FF
          move.b LATE_01(a4), -(a3)     ;172C 0001
          move.b LATE_02(a5), -(a2)     ;152D 0002
          move.b LATE_FE(a6), -(a1)     ;132E 00FE
          move.b LATE_FF(a7), -(a0)     ;112F 00FF

          move.w $01(a0), -(a7)         ;3F28 0001
          move.w $02(a1), -(a6)         ;3D29 0002
          move.w $FE(a2), -(a5)         ;3B2A 00FE
          move.w $FF(a3), -(a4)         ;392B 00FF
          move.w LATE_01(a4), -(a3)     ;372C 0001
          move.w LATE_02(a5), -(a2)     ;352D 0002
          move.w LATE_FE(a6), -(a1)     ;332E 00FE
          move.w LATE_FF(a7), -(a0)     ;312F 00FF

          move.l $01(a0), -(a7)         ;2F28 0001
          move.l $02(a1), -(a6)         ;2D29 0002
          move.l $FE(a2), -(a5)         ;2B2A 00FE
          move.l $FF(a3), -(a4)         ;292B 00FF
          move.l LATE_01(a4), -(a3)     ;272C 0001
          move.l LATE_02(a5), -(a2)     ;252D 0002
          move.l LATE_FE(a6), -(a1)     ;232E 00FE
          move.l LATE_FF(a7), -(a0)     ;212F 00FF

          move $01(a0), -(a7)           ;3F28 0001
          move $02(a1), -(a6)           ;3D29 0002
          move $FE(a2), -(a5)           ;3B2A 00FE
          move $FF(a3), -(a4)           ;392B 00FF
          move LATE_01(a4), -(a3)       ;372C 0001
          move LATE_02(a5), -(a2)       ;352D 0002
          move LATE_FE(a6), -(a1)       ;332E 00FE
          move LATE_FF(a7), -(a0)       ;312F 00FF



          move.b (a0)+, ($0001).w       ;11D8 0001
          move.b (a1)+, ($00FF).w       ;11D9 00FF
          move.b (a2)+, ($0100).w       ;11DA 0100
          move.b (a3)+, ($FF00).w       ;11DB FF00
          move.b (a4)+, (LATE_0001).w   ;11DC 0001
          move.b (a5)+, (LATE_00FF).w   ;11DD 00FF
          move.b (a6)+, (LATE_0100).w   ;11DE 0100
          move.b (a7)+, (LATE_FF00).w   ;11DF FF00

          move.b (a0)+, ($0001)         ;11D8 0001      <downgrade!
          move.b (a1)+, ($00FF)         ;11D9 00FF      <downgrade!
          move.b (a2)+, ($0100)         ;11DA 0100      <downgrade!
          move.b (a3)+, ($FF00)         ;13DB 0000FF00
          move.b (a4)+, (LATE_0001)     ;13DC 00000001
          move.b (a5)+, (LATE_00FF)     ;13DD 000000FF
          move.b (a6)+, (LATE_0100)     ;13DE 00000100
          move.b (a7)+, (LATE_FF00)     ;13DF 0000FF00

          move (a0)+, ($0001).w         ;31D8 0001
          move (a1)+, ($00FF).w         ;31D9 00FF
          move (a2)+, ($0100).w         ;31DA 0100
          move (a3)+, ($FF00).w         ;31DB FF00
          move (a4)+, (LATE_0001).w     ;31DC 0001
          move (a5)+, (LATE_00FF).w     ;31DD 00FF
          move (a6)+, (LATE_0100).w     ;31DE 0100
          move (a7)+, (LATE_FF00).w     ;31DF FF00

          move (a0)+, ($0001)           ;31D8 0001      <downgrade!
          move (a1)+, ($00FF)           ;31D9 00FF      <downgrade!
          move (a2)+, ($0100)           ;31DA 0100      <downgrade!
          move (a3)+, ($FF00)           ;33DB 0000FF00
          move (a4)+, (LATE_0001)       ;33DC 00000001
          move (a5)+, (LATE_00FF)       ;33DD 000000FF
          move (a6)+, (LATE_0100)       ;33DE 00000100
          move (a7)+, (LATE_FF00)       ;33DF 0000FF00



          move.b (a7)+, d0          ;101F
          move.b (a6)+, d1          ;121E
          move.b (a5)+, d2          ;141D
          move.b (a4)+, d3          ;161C
          move.b (a3)+, d4          ;181B
          move.b (a2)+, d5          ;1A1A
          move.b (a1)+, d6          ;1C19
          move.b (a0)+, d7          ;1E18

          move.w (a7)+, d0          ;301F
          move.w (a6)+, d1          ;321E
          move.w (a5)+, d2          ;341D
          move.w (a4)+, d3          ;361C
          move.w (a3)+, d4          ;381B
          move.w (a2)+, d5          ;3A1A
          move.w (a1)+, d6          ;3C19
          move.w (a0)+, d7          ;3E18

          move.l (a7)+, d0          ;201F
          move.l (a6)+, d1          ;221E
          move.l (a5)+, d2          ;241D
          move.l (a4)+, d3          ;261C
          move.l (a3)+, d4          ;281B
          move.l (a2)+, d5          ;2A1A
          move.l (a1)+, d6          ;2C19
          move.l (a0)+, d7          ;2E18

          move (a7)+, d0            ;301F
          move (a6)+, d1            ;321E
          move (a5)+, d2            ;341D
          move (a4)+, d3            ;361C
          move (a3)+, d4            ;381B
          move (a2)+, d5            ;3A1A
          move (a1)+, d6            ;3C19
          move (a0)+, d7            ;3E18



          move.b (a0)+, (a7)        ;1E98
          move.b (a1)+, (a6)        ;1C99
          move.b (a2)+, (a5)        ;1A9A
          move.b (a3)+, (a4)        ;189B
          move.b (a4)+, (a3)        ;169C
          move.b (a5)+, (a2)        ;149D
          move.b (a6)+, (a1)        ;129E
          move.b (a7)+, (a0)        ;109F

          move.w (a0)+, (a7)        ;3E98
          move.w (a1)+, (a6)        ;3C99
          move.w (a2)+, (a5)        ;3A9A
          move.w (a3)+, (a4)        ;389B
          move.w (a4)+, (a3)        ;369C
          move.w (a5)+, (a2)        ;349D
          move.w (a6)+, (a1)        ;329E
          move.w (a7)+, (a0)        ;309F

          move.l (a0)+, (a7)        ;2E98
          move.l (a1)+, (a6)        ;2C99
          move.l (a2)+, (a5)        ;2A9A
          move.l (a3)+, (a4)        ;289B
          move.l (a4)+, (a3)        ;269C
          move.l (a5)+, (a2)        ;249D
          move.l (a6)+, (a1)        ;229E
          move.l (a7)+, (a0)        ;209F

          move (a0)+, (a7)          ;3E98
          move (a1)+, (a6)          ;3C99
          move (a2)+, (a5)          ;3A9A
          move (a3)+, (a4)          ;389B
          move (a4)+, (a3)          ;369C
          move (a5)+, (a2)          ;349D
          move (a6)+, (a1)          ;329E
          move (a7)+, (a0)          ;309F



          move.b (a0)+, $01(a7)         ;1F58 0001
          move.b (a1)+, $FF(a6)         ;1D59 00FF
          move.b (a2)+, LATE_01(a5)     ;1B5A 0001
          move.b (a3)+, LATE_FF(a4)     ;195B 00FF
          move.b (a4)+, $01(a3)         ;175C 0001
          move.b (a5)+, $FF(a2)         ;155D 00FF
          move.b (a6)+, LATE_01(a1)     ;135E 0001
          move.b (a7)+, LATE_FF(a0)     ;115F 00FF

          move.w (a0)+, $01(a7)         ;3F58 0001
          move.w (a1)+, $FF(a6)         ;3D59 00FF
          move.w (a2)+, LATE_01(a5)     ;3B5A 0001
          move.w (a3)+, LATE_FF(a4)     ;395B 00FF
          move.w (a4)+, $01(a3)         ;375C 0001
          move.w (a5)+, $FF(a2)         ;355D 00FF
          move.w (a6)+, LATE_01(a1)     ;335E 0001
          move.w (a7)+, LATE_FF(a0)     ;315F 00FF

          move.l (a0)+, $01(a7)         ;2F58 0001
          move.l (a1)+, $FF(a6)         ;2D59 00FF
          move.l (a2)+, LATE_01(a5)     ;2B5A 0001
          move.l (a3)+, LATE_FF(a4)     ;295B 00FF
          move.l (a4)+, $01(a3)         ;275C 0001
          move.l (a5)+, $FF(a2)         ;255D 00FF
          move.l (a6)+, LATE_01(a1)     ;235E 0001
          move.l (a7)+, LATE_FF(a0)     ;215F 00FF

          move (a0)+, $01(a7)           ;3F58 0001
          move (a1)+, $FF(a6)           ;3D59 00FF
          move (a2)+, LATE_01(a5)       ;3B5A 0001
          move (a3)+, LATE_FF(a4)       ;395B 00FF
          move (a4)+, $01(a3)           ;375C 0001
          move (a5)+, $FF(a2)           ;355D 00FF
          move (a6)+, LATE_01(a1)       ;335E 0001
          move (a7)+, LATE_FF(a0)       ;315F 00FF



          move.b (a0)+, (a7)+       ;1E D8
          move.b (a1)+, (a6)+       ;1C D9
          move.b (a2)+, (a5)+       ;1A DA
          move.b (a3)+, (a4)+       ;18 DB
          move.b (a4)+, (a3)+       ;16 DC
          move.b (a5)+, (a2)+       ;14 DD
          move.b (a6)+, (a1)+       ;12 DE
          move.b (a7)+, (a0)+       ;10 DF

          move.w (a0)+, (a7)+       ;3E D8
          move.w (a1)+, (a6)+       ;3C D9
          move.w (a2)+, (a5)+       ;3A DA
          move.w (a3)+, (a4)+       ;38 DB
          move.w (a4)+, (a3)+       ;36 DC
          move.w (a5)+, (a2)+       ;34 DD
          move.w (a6)+, (a1)+       ;32 DE
          move.w (a7)+, (a0)+       ;30 DF

          move.l (a0)+, (a7)+       ;2E D8
          move.l (a1)+, (a6)+       ;2C D9
          move.l (a2)+, (a5)+       ;2A DA
          move.l (a3)+, (a4)+       ;28 DB
          move.l (a4)+, (a3)+       ;26 DC
          move.l (a5)+, (a2)+       ;24 DD
          move.l (a6)+, (a1)+       ;22 DE
          move.l (a7)+, (a0)+       ;20 DF

          move (a0)+, (a7)+         ;3E D8
          move (a1)+, (a6)+         ;3C D9
          move (a2)+, (a5)+         ;3A DA
          move (a3)+, (a4)+         ;38 DB
          move (a4)+, (a3)+         ;36 DC
          move (a5)+, (a2)+         ;34 DD
          move (a6)+, (a1)+         ;32 DE
          move (a7)+, (a0)+         ;30 DF



          move.b -(a7), d0          ;1027
          move.b -(a6), d1          ;1226
          move.b -(a5), d2          ;1425
          move.b -(a4), d3          ;1624
          move.b -(a3), d4          ;1823
          move.b -(a2), d5          ;1A22
          move.b -(a1), d6          ;1C21
          move.b -(a0), d7          ;1E20

          move.w -(a7), d0          ;3027
          move.w -(a6), d1          ;3226
          move.w -(a5), d2          ;3425
          move.w -(a4), d3          ;3624
          move.w -(a3), d4          ;3823
          move.w -(a2), d5          ;3A22
          move.w -(a1), d6          ;3C21
          move.w -(a0), d7          ;3E20

          move.l -(a7), d0          ;2027
          move.l -(a6), d1          ;2226
          move.l -(a5), d2          ;2425
          move.l -(a4), d3          ;2624
          move.l -(a3), d4          ;2823
          move.l -(a2), d5          ;2A22
          move.l -(a1), d6          ;2C21
          move.l -(a0), d7          ;2E20

          move -(a7), d0            ;3027
          move -(a6), d1            ;3226
          move -(a5), d2            ;3425
          move -(a4), d3            ;3624
          move -(a3), d4            ;3823
          move -(a2), d5            ;3A22
          move -(a1), d6            ;3C21
          move -(a0), d7            ;3E20


          move.b -(a7), -(a0)       ;1127
          move.b -(a6), -(a1)       ;1326
          move.b -(a5), -(a2)       ;1525
          move.b -(a4), -(a3)       ;1724
          move.b -(a3), -(a4)       ;1923
          move.b -(a2), -(a5)       ;1B22
          move.b -(a1), -(a6)       ;1D21
          move.b -(a0), -(a7)       ;1F20

          move.w -(a7), -(a0)       ;3127
          move.w -(a6), -(a1)       ;3326
          move.w -(a5), -(a2)       ;3525
          move.w -(a4), -(a3)       ;3724
          move.w -(a3), -(a4)       ;3923
          move.w -(a2), -(a5)       ;3B22
          move.w -(a1), -(a6)       ;3D21
          move.w -(a0), -(a7)       ;3F20

          move.l -(a7), -(a0)       ;2127
          move.l -(a6), -(a1)       ;2326
          move.l -(a5), -(a2)       ;2525
          move.l -(a4), -(a3)       ;2724
          move.l -(a3), -(a4)       ;2923
          move.l -(a2), -(a5)       ;2B22
          move.l -(a1), -(a6)       ;2D21
          move.l -(a0), -(a7)       ;2F20

          move -(a7), -(a0)         ;3127
          move -(a6), -(a1)         ;3326
          move -(a5), -(a2)         ;3525
          move -(a4), -(a3)         ;3724
          move -(a3), -(a4)         ;3923
          move -(a2), -(a5)         ;3B22
          move -(a1), -(a6)         ;3D21
          move -(a0), -(a7)         ;3F20


          move.w a0, ($0001).w      ;31C8 0001
          move.w a1, ($00FF).w      ;31C9 00FF
          move.w a2, ($0100).w      ;31CA 0100
          move.w a3, ($FF00).w      ;31CB FF00
          move.w a4, (LATE_0001).w  ;31CC 0001
          move.w a5, (LATE_00FF).w  ;31CD 00FF
          move.w a6, (LATE_0100).w  ;31CE 0100
          move.w a7, (LATE_FF00).w  ;31CF FF00

          move.w a0, ($0001)        ;31C8 0001      <downgrade
          move.w a1, ($00FF)        ;31C9 00FF      <downgrade
          move.w a2, ($0100)        ;31CA 0100      <downgrade
          move.w a3, ($FF00)        ;33CB 0000FF00
          move.w a4, (LATE_0001)    ;33CC 00000001
          move.w a5, (LATE_00FF)    ;33CD 000000FF
          move.w a6, (LATE_0100)    ;33CE 00000100
          move.w a7, (LATE_FF00)    ;33CF 0000FF00

          move.l a0, ($0001).w      ;21C8 0001
          move.l a1, ($00FF).w      ;21C9 00FF
          move.l a2, ($0100).w      ;21CA 0100
          move.l a3, ($FF00).w      ;21CB FF00
          move.l a4, (LATE_0001).w  ;21CC 0001
          move.l a5, (LATE_00FF).w  ;21CD 00FF
          move.l a6, (LATE_0100).w  ;21CE 0100
          move.l a7, (LATE_FF00).w  ;21CF FF00

          move.l a0, ($0001)        ;21C8 0001      <downgrade
          move.l a1, ($00FF)        ;21C9 00FF      <downgrade
          move.l a2, ($0100)        ;21CA 0100      <downgrade
          move.l a3, ($FF00)        ;23CB 0000FF00
          move.l a4, (LATE_0001)    ;23CC 00000001
          move.l a5, (LATE_00FF)    ;23CD 000000FF
          move.l a6, (LATE_0100)    ;23CE 00000100
          move.l a7, (LATE_FF00)    ;23CF 0000FF00

          move a0, ($0001).w        ;31C8 0001
          move a1, ($00FF).w        ;31C9 00FF
          move a2, ($0100).w        ;31CA 0100
          move a3, ($FF00).w        ;31CB FF00
          move a4, (LATE_0001).w    ;31CC 0001
          move a5, (LATE_00FF).w    ;31CD 00FF
          move a6, (LATE_0100).w    ;31CE 0100
          move a7, (LATE_FF00).w    ;31CF FF00



          move.w a7, d0             ;300F
          move.w a6, d1             ;320E
          move.w a5, d2             ;340D
          move.w a4, d3             ;360C
          move.w a3, d4             ;380B
          move.w a2, d5             ;3A0A
          move.w a1, d6             ;3C09
          move.w a0, d7             ;3E08

          move.l a7, d0             ;200F
          move.l a6, d1             ;220E
          move.l a5, d2             ;240D
          move.l a4, d3             ;260C
          move.l a3, d4             ;280B
          move.l a2, d5             ;2A0A
          move.l a1, d6             ;2C09
          move.l a0, d7             ;2E08

          move a7, d0               ;300F
          move a6, d1               ;320E
          move a5, d2               ;340D
          move a4, d3               ;360C
          move a3, d4               ;380B
          move a2, d5               ;3A0A
          move a1, d6               ;3C09
          move a0, d7               ;3E08


          move.w a7, (a0)           ;308F
          move.w a6, (a1)           ;328E
          move.w a5, (a2)           ;348D
          move.w a4, (a3)           ;368C
          move.w a3, (a4)           ;388B
          move.w a2, (a5)           ;3A8A
          move.w a1, (a6)           ;3C89
          move.w a0, (a7)           ;3E88

          move.l a7, (a0)           ;208F
          move.l a6, (a1)           ;228E
          move.l a5, (a2)           ;248D
          move.l a4, (a3)           ;268C
          move.l a3, (a4)           ;288B
          move.l a2, (a5)           ;2A8A
          move.l a1, (a6)           ;2C89
          move.l a0, (a7)           ;2E88

          move a7, (a0)             ;308F
          move a6, (a1)             ;328E
          move a5, (a2)             ;348D
          move a4, (a3)             ;368C
          move a3, (a4)             ;388B
          move a2, (a5)             ;3A8A
          move a1, (a6)             ;3C89
          move a0, (a7)             ;3E88


          move.w a0, $01(a7)        ;3F48 0001
          move.w a1, $FF(a6)        ;3D49 00FF
          move.w a2, LATE_01(a5)    ;3B4A 0001
          move.w a3, LATE_FF(a4)    ;394B 00FF
          move.w a4, $01(a3)        ;374C 0001
          move.w a5, $FF(a2)        ;354D 00FF
          move.w a6, LATE_01(a1)    ;334E 0001
          move.w a7, LATE_FF(a0)    ;314F 00FF

          move.l a0, $01(a7)        ;2F48 0001
          move.l a1, $FF(a6)        ;2D49 00FF
          move.l a2, LATE_01(a5)    ;2B4A 0001
          move.l a3, LATE_FF(a4)    ;294B 00FF
          move.l a4, $01(a3)        ;274C 0001
          move.l a5, $FF(a2)        ;254D 00FF
          move.l a6, LATE_01(a1)    ;234E 0001
          move.l a7, LATE_FF(a0)    ;214F 00FF

          move a0, $01(a7)          ;3F48 0001
          move a1, $FF(a6)          ;3D49 00FF
          move a2, LATE_01(a5)      ;3B4A 0001
          move a3, LATE_FF(a4)      ;394B 00FF
          move a4, $01(a3)          ;374C 0001
          move a5, $FF(a2)          ;354D 00FF
          move a6, LATE_01(a1)      ;334E 0001
          move a7, LATE_FF(a0)      ;314F 00FF



          move.w a2, (sp)           ;3E8A
          move.w a0, (a7)           ;3E88
          move.w a7, (a0)           ;308F
          move.w a2, (sp)+          ;3ECA
          move.w a0, (a7)+          ;3EC8
          move.w a7, (a0)+          ;30CF
          move.w a2, -(sp)          ;3F0A
          move.w a0, -(a7)          ;3F08
          move.w a7, -(a0)          ;310F

          move.l a2, (sp)           ;2E8A
          move.l a0, (a7)           ;2E88
          move.l a7, (a0)           ;208F
          move.l a2, (sp)+          ;2ECA
          move.l a0, (a7)+          ;2EC8
          move.l a7, (a0)+          ;20CF
          move.l a2, -(sp)          ;2F0A
          move.l a0, -(a7)          ;2F08
          move.l a7, -(a0)          ;210F

          move.w a2, (sp)           ;3E8A
          move.w a0, (a7)           ;3E88
          move.w a7, (a0)           ;308F
          move.w a2, (sp)+          ;3ECA
          move.w a0, (a7)+          ;3EC8
          move.w a7, (a0)+          ;30CF
          move.w a2, -(sp)          ;3F0A
          move.w a0, -(a7)          ;3F08
          move.w a7, -(a0)          ;310F



          move.b $01(a0,d7.w), ($FF00).w          ;11F0 7001 FF00
          move.b $02(a1,d6.w), ($0100).w          ;11F1 6002 0100
          move.b $7E(a2,d5.w), (LATE_00FF).w      ;11F2 507E 00FF
          move.b $7F(a3,d4.w), (LATE_0001).w      ;11F3 407F 0001
          move.b LATE_01(a4,d3.w), (LATE_FF00).w  ;11F4 3001 FF00
          move.b LATE_02(a5,d2.w), (LATE_0100).w  ;11F5 2002 0100
          move.b LATE_7E(a6,d1.w), ($00FF).w      ;11F6 107E 00FF
          move.b LATE_7F(a7,d0.w), ($0001).w      ;11F7 007F 0001

          move.w $01(a0,d7.w), ($FF00).w          ;31F0 7001 FF00
          move.w $02(a1,d6.w), ($0100).w          ;31F1 6002 0100
          move.w $7E(a2,d5.w), (LATE_00FF).w      ;31F2 507E 00FF
          move.w $7F(a3,d4.w), (LATE_0001).w      ;31F3 407F 0001
          move.w LATE_01(a4,d3.w), (LATE_FF00).w  ;31F4 3001 FF00
          move.w LATE_02(a5,d2.w), (LATE_0100).w  ;31F5 2002 0100
          move.w LATE_7E(a6,d1.w), ($00FF).w      ;31F6 107E 00FF
          move.w LATE_7F(a7,d0.w), ($0001).w      ;31F7 007F 0001

          move.l $01(a0,d7.w), ($FF00).w          ;21F0 7001 FF00
          move.l $02(a1,d6.w), ($0100).w          ;21F1 6002 0100
          move.l $7E(a2,d5.w), (LATE_00FF).w      ;21F2 507E 00FF
          move.l $7F(a3,d4.w), (LATE_0001).w      ;21F3 407F 0001
          move.l LATE_01(a4,d3.w), (LATE_FF00).w  ;21F4 3001 FF00
          move.l LATE_02(a5,d2.w), (LATE_0100).w  ;21F5 2002 0100
          move.l LATE_7E(a6,d1.w), ($00FF).w      ;21F6 107E 00FF
          move.l LATE_7F(a7,d0.w), ($0001).w      ;21F7 007F 0001

          move $01(a0,d7.w), ($FF00).w            ;31F0 7001 FF00
          move $02(a1,d6.w), ($0100).w            ;31F1 6002 0100
          move $7E(a2,d5.w), (LATE_00FF).w        ;31F2 507E 00FF
          move $7F(a3,d4.w), (LATE_0001).w        ;31F3 407F 0001
          move LATE_01(a4,d3.w), (LATE_FF00).w    ;31F4 3001 FF00
          move LATE_02(a5,d2.w), (LATE_0100).w    ;31F5 2002 0100
          move LATE_7E(a6,d1.w), ($00FF).w        ;31F6 107E 00FF
          move LATE_7F(a7,d0.w), ($0001).w        ;31F7 007F 0001

          move.b $01(a0,d7.w), ($FF00)            ;13F0 7001 0000FF00
          move.b $02(a1,d6.w), ($0100)            ;11F1 6002 0100       <downgrade
          move.b $7E(a2,d5.w), (LATE_00FF)        ;13F2 507E 000000FF
          move.b $7F(a3,d4.w), (LATE_0001)        ;13F3 407F 00000001
          move.b LATE_01(a4,d3.w), (LATE_FF00)    ;13F4 3001 0000FF00
          move.b LATE_02(a5,d2.w), (LATE_0100)    ;13F5 2002 00000100
          move.b LATE_7E(a6,d1.w), ($00FF)        ;11F6 107E 00FF       <downgrade
          move.b LATE_7F(a7,d0.w), ($0001)        ;11F7 007F 0001       <downgrade

          move.w $01(a0,d7.w), ($FF00)            ;33F0 7001 0000FF00
          move.w $02(a1,d6.w), ($0100)            ;31F1 6002 0100       <downgrade
          move.w $7E(a2,d5.w), (LATE_00FF)        ;33F2 507E 000000FF
          move.w $7F(a3,d4.w), (LATE_0001)        ;33F3 407F 00000001
          move.w LATE_01(a4,d3.w), (LATE_FF00)    ;33F4 3001 0000FF00
          move.w LATE_02(a5,d2.w), (LATE_0100)    ;33F5 2002 00000100
          move.w LATE_7E(a6,d1.w), ($00FF)        ;31F6 107E 00FF       <downgrade
          move.w LATE_7F(a7,d0.w), ($0001)        ;31F7 007F 0001       <downgrade

          move.l $01(a0,d7.w), ($FF00)            ;23F0 7001 0000FF00
          move.l $02(a1,d6.w), ($0100)            ;21F1 6002 0100       <downgrade
          move.l $7E(a2,d5.w), (LATE_00FF)        ;23F2 507E 000000FF
          move.l $7F(a3,d4.w), (LATE_0001)        ;23F3 407F 00000001
          move.l LATE_01(a4,d3.w), (LATE_FF00)    ;23F4 3001 0000FF00
          move.l LATE_02(a5,d2.w), (LATE_0100)    ;23F5 2002 00000100
          move.l LATE_7E(a6,d1.w), ($00FF)        ;21F6 107E 00FF       <downgrade
          move.l LATE_7F(a7,d0.w), ($0001)        ;21F7 007F 0001       <downgrade

          move $01(a0,d7.w), ($FF00)              ;33F0 7001 0000FF00
          move $02(a1,d6.w), ($0100)              ;31F1 6002 0100       <downgrade
          move $7E(a2,d5.w), (LATE_00FF)          ;33F2 507E 000000FF
          move $7F(a3,d4.w), (LATE_0001)          ;33F3 407F 00000001
          move LATE_01(a4,d3.w), (LATE_FF00)      ;33F4 3001 0000FF00
          move LATE_02(a5,d2.w), (LATE_0100)      ;33F5 2002 00000100
          move LATE_7E(a6,d1.w), ($00FF)          ;31F6 107E 00FF       <downgrade
          move LATE_7F(a7,d0.w), ($0001)          ;31F7 007F 0001       <downgrade

          move.b $01(a0,d7.l), ($FF00).w          ;11F0 7801 FF00
          move.b $02(a1,d6.l), ($0100).w          ;11F1 6802 0100
          move.b $7E(a2,d5.l), (LATE_00FF).w      ;11F2 587E 00FF
          move.b $7F(a3,d4.l), (LATE_0001).w      ;11F3 487F 0001
          move.b LATE_01(a4,d3.l), (LATE_FF00).w  ;11F4 3801 FF00
          move.b LATE_02(a5,d2.l), (LATE_0100).w  ;11F5 2802 0100
          move.b LATE_7E(a6,d1.l), ($00FF).w      ;11F6 187E 00FF
          move.b LATE_7F(a7,d0.l), ($0001).w      ;11F7 087F 0001

          move.w $01(a0,d7.l), ($FF00).w          ;31F0 7801 FF00
          move.w $02(a1,d6.l), ($0100).w          ;31F1 6802 0100
          move.w $7E(a2,d5.l), (LATE_00FF).w      ;31F2 587E 00FF
          move.w $7F(a3,d4.l), (LATE_0001).w      ;31F3 487F 0001
          move.w LATE_01(a4,d3.l), (LATE_FF00).w  ;31F4 3801 FF00
          move.w LATE_02(a5,d2.l), (LATE_0100).w  ;31F5 2802 0100
          move.w LATE_7E(a6,d1.l), ($00FF).w      ;31F6 187E 00FF
          move.w LATE_7F(a7,d0.l), ($0001).w      ;31F7 087F 0001

          move.l $01(a0,d7.l), ($FF00).w          ;21F0 7801 FF00
          move.l $02(a1,d6.l), ($0100).w          ;21F1 6802 0100
          move.l $7E(a2,d5.l), (LATE_00FF).w      ;21F2 587E 00FF
          move.l $7F(a3,d4.l), (LATE_0001).w      ;21F3 487F 0001
          move.l LATE_01(a4,d3.l), (LATE_FF00).w  ;21F4 3801 FF00
          move.l LATE_02(a5,d2.l), (LATE_0100).w  ;21F5 2802 0100
          move.l LATE_7E(a6,d1.l), ($00FF).w      ;21F6 187E 00FF
          move.l LATE_7F(a7,d0.l), ($0001).w      ;21F7 087F 0001

          move $01(a0,d7.l), ($FF00).w            ;31F0 7801 FF00
          move $02(a1,d6.l), ($0100).w            ;31F1 6802 0100
          move $7E(a2,d5.l), (LATE_00FF).w        ;31F2 587E 00FF
          move $7F(a3,d4.l), (LATE_0001).w        ;31F3 487F 0001
          move LATE_01(a4,d3.l), (LATE_FF00).w    ;31F4 3801 FF00
          move LATE_02(a5,d2.l), (LATE_0100).w    ;31F5 2802 0100
          move LATE_7E(a6,d1.l), ($00FF).w        ;31F6 187E 00FF
          move LATE_7F(a7,d0.l), ($0001).w        ;31F7 087F 0001

          move.b $01(a0,d7), ($FF00).w            ;11F0 7001 FF00
          move.b $02(a1,d6), ($0100).w            ;11F1 6002 0100
          move.b $7E(a2,d5), (LATE_00FF).w        ;11F2 507E 00FF
          move.b $7F(a3,d4), (LATE_0001).w        ;11F3 407F 0001
          move.b LATE_01(a4,d3), (LATE_FF00).w    ;11F4 3001 FF00
          move.b LATE_02(a5,d2), (LATE_0100).w    ;11F5 2002 0100
          move.b LATE_7E(a6,d1), ($00FF).w        ;11F6 107E 00FF
          move.b LATE_7F(a7,d0), ($0001).w        ;11F7 007F 0001

          move.w $01(a0,d7), ($FF00).w            ;31F0 7001 FF00
          move.w $02(a1,d6), ($0100).w            ;31F1 6002 0100
          move.w $7E(a2,d5), (LATE_00FF).w        ;31F2 507E 00FF
          move.w $7F(a3,d4), (LATE_0001).w        ;31F3 407F 0001
          move.w LATE_01(a4,d3), (LATE_FF00).w    ;31F4 3001 FF00
          move.w LATE_02(a5,d2), (LATE_0100).w    ;31F5 2002 0100
          move.w LATE_7E(a6,d1), ($00FF).w        ;31F6 107E 00FF
          move.w LATE_7F(a7,d0), ($0001).w        ;31F7 007F 0001

          move.l $01(a0,d7), ($FF00).w            ;21F0 7001 FF00
          move.l $02(a1,d6), ($0100).w            ;21F1 6002 0100
          move.l $7E(a2,d5), (LATE_00FF).w        ;21F2 507E 00FF
          move.l $7F(a3,d4), (LATE_0001).w        ;21F3 407F 0001
          move.l LATE_01(a4,d3), (LATE_FF00).w    ;21F4 3001 FF00
          move.l LATE_02(a5,d2), (LATE_0100).w    ;21F5 2002 0100
          move.l LATE_7E(a6,d1), ($00FF).w        ;21F6 107E 00FF
          move.l LATE_7F(a7,d0), ($0001).w        ;21F7 007F 0001

          move $01(a0,d7), ($FF00).w              ;31F0 7001 FF00
          move $02(a1,d6), ($0100).w              ;31F1 6002 0100
          move $7E(a2,d5), (LATE_00FF).w          ;31F2 507E 00FF
          move $7F(a3,d4), (LATE_0001).w          ;31F3 407F 0001
          move LATE_01(a4,d3), (LATE_FF00).w      ;31F4 3001 FF00
          move LATE_02(a5,d2), (LATE_0100).w      ;31F5 2002 0100
          move LATE_7E(a6,d1), ($00FF).w          ;31F6 107E 00FF
          move LATE_7F(a7,d0), ($0001).w          ;31F7 007F 0001

          move.b $01(a0,d7), ($FF00)              ;13F0 7001 0000FF00
          move.b $02(a1,d6), ($0100)              ;11F1 6002 0100       <downgrade
          move.b $7E(a2,d5), (LATE_00FF)          ;13F2 507E 000000FF
          move.b $7F(a3,d4), (LATE_0001)          ;13F3 407F 00000001
          move.b LATE_01(a4,d3), (LATE_FF00)      ;13F4 3001 0000FF00
          move.b LATE_02(a5,d2), (LATE_0100)      ;13F5 2002 00000100
          move.b LATE_7E(a6,d1), ($00FF)          ;11F6 107E 00FF       <downgrade
          move.b LATE_7F(a7,d0), ($0001)          ;11F7 007F 0001       <downgrade

          move.w $01(a0,d7), ($FF00)              ;33F0 7001 0000FF00
          move.w $02(a1,d6), ($0100)              ;31F1 6002 0100       <downgrade
          move.w $7E(a2,d5), (LATE_00FF)          ;33F2 507E 000000FF
          move.w $7F(a3,d4), (LATE_0001)          ;33F3 407F 00000001
          move.w LATE_01(a4,d3), (LATE_FF00)      ;33F4 3001 0000FF00
          move.w LATE_02(a5,d2), (LATE_0100)      ;33F5 2002 00000100
          move.w LATE_7E(a6,d1), ($00FF)          ;31F6 107E 00FF       <downgrade
          move.w LATE_7F(a7,d0), ($0001)          ;31F7 007F 0001       <downgrade

          move.l $01(a0,d7), ($FF00)              ;23F0 7001 0000FF00
          move.l $02(a1,d6), ($0100)              ;21F1 6002 0100       <downgrade
          move.l $7E(a2,d5), (LATE_00FF)          ;23F2 507E 000000FF
          move.l $7F(a3,d4), (LATE_0001)          ;23F3 407F 00000001
          move.l LATE_01(a4,d3), (LATE_FF00)      ;23F4 3001 0000FF00
          move.l LATE_02(a5,d2), (LATE_0100)      ;23F5 2002 00000100
          move.l LATE_7E(a6,d1), ($00FF)          ;21F6 107E 00FF       <downgrade
          move.l LATE_7F(a7,d0), ($0001)          ;21F7 007F 0001       <downgrade

          move $01(a0,d7), ($FF00)                ;33F0 7001 0000FF00
          move $02(a1,d6), ($0100)                ;31F1 6002 0100       <downgrade
          move $7E(a2,d5), (LATE_00FF)            ;33F2 507E 000000FF
          move $7F(a3,d4), (LATE_0001)            ;33F3 407F 00000001
          move LATE_01(a4,d3), (LATE_FF00)        ;33F4 3001 0000FF00
          move LATE_02(a5,d2), (LATE_0100)        ;33F5 2002 00000100
          move LATE_7E(a6,d1), ($00FF)            ;31F6 107E 00FF       <downgrade
          move LATE_7F(a7,d0), ($0001)            ;31F7 007F 0001       <downgrade


          move.b (a0,d7.w), d0   ;1030 7000
          move.b (a1,d6.w), d1   ;1231 6000
          move.b (a2,d5.w), d2   ;1432 5000
          move.b (a3,d4.w), d3   ;1633 4000
          move.b (a4,d3.w), d4   ;1834 3000
          move.b (a5,d2.w), d5   ;1A35 2000
          move.b (a6,d1.w), d6   ;1C36 1000
          move.b (a7,d0.w), d7   ;1E37 0000

          move.w (a0,d7.w), d0   ;3030 7000
          move.w (a1,d6.w), d1   ;3231 6000
          move.w (a2,d5.w), d2   ;3432 5000
          move.w (a3,d4.w), d3   ;3633 4000
          move.w (a4,d3.w), d4   ;3834 3000
          move.w (a5,d2.w), d5   ;3A35 2000
          move.w (a6,d1.w), d6   ;3C36 1000
          move.w (a7,d0.w), d7   ;3E37 0000

          move.l (a0,d7.w), d0   ;2030 7000
          move.l (a1,d6.w), d1   ;2231 6000
          move.l (a2,d5.w), d2   ;2432 5000
          move.l (a3,d4.w), d3   ;2633 4000
          move.l (a4,d3.w), d4   ;2834 3000
          move.l (a5,d2.w), d5   ;2A35 2000
          move.l (a6,d1.w), d6   ;2C36 1000
          move.l (a7,d0.w), d7   ;2E37 0000

          move (a0,d7.w), d0     ;3030 7000
          move (a1,d6.w), d1     ;3231 6000
          move (a2,d5.w), d2     ;3432 5000
          move (a3,d4.w), d3     ;3633 4000
          move (a4,d3.w), d4     ;3834 3000
          move (a5,d2.w), d5     ;3A35 2000
          move (a6,d1.w), d6     ;3C36 1000
          move (a7,d0.w), d7     ;3E37 0000

          move.b (a0,d7.l), d0   ;1030 7800
          move.b (a1,d6.l), d1   ;1231 6800
          move.b (a2,d5.l), d2   ;1432 5800
          move.b (a3,d4.l), d3   ;1633 4800
          move.b (a4,d3.l), d4   ;1834 3800
          move.b (a5,d2.l), d5   ;1A35 2800
          move.b (a6,d1.l), d6   ;1C36 1800
          move.b (a7,d0.l), d7   ;1E37 0800

          move.w (a0,d7.l), d0   ;3030 7800
          move.w (a1,d6.l), d1   ;3231 6800
          move.w (a2,d5.l), d2   ;3432 5800
          move.w (a3,d4.l), d3   ;3633 4800
          move.w (a4,d3.l), d4   ;3834 3800
          move.w (a5,d2.l), d5   ;3A35 2800
          move.w (a6,d1.l), d6   ;3C36 1800
          move.w (a7,d0.l), d7   ;3E37 0800

          move.l (a0,d7.l), d0   ;2030 7800
          move.l (a1,d6.l), d1   ;2231 6800
          move.l (a2,d5.l), d2   ;2432 5800
          move.l (a3,d4.l), d3   ;2633 4800
          move.l (a4,d3.l), d4   ;2834 3800
          move.l (a5,d2.l), d5   ;2A35 2800
          move.l (a6,d1.l), d6   ;2C36 1800
          move.l (a7,d0.l), d7   ;2E37 0800

          move (a0,d7.l), d0     ;3030 7800
          move (a1,d6.l), d1     ;3231 6800
          move (a2,d5.l), d2     ;3432 5800
          move (a3,d4.l), d3     ;3633 4800
          move (a4,d3.l), d4     ;3834 3800
          move (a5,d2.l), d5     ;3A35 2800
          move (a6,d1.l), d6     ;3C36 1800
          move (a7,d0.l), d7     ;3E37 0800

          move.b (a0,d7), d0     ;1030 7000
          move.b (a1,d6), d1     ;1231 6000
          move.b (a2,d5), d2     ;1432 5000
          move.b (a3,d4), d3     ;1633 4000
          move.b (a4,d3), d4     ;1834 3000
          move.b (a5,d2), d5     ;1A35 2000
          move.b (a6,d1), d6     ;1C36 1000
          move.b (a7,d0), d7     ;1E37 0000

          move.w (a0,d7), d0     ;3030 7000
          move.w (a1,d6), d1     ;3231 6000
          move.w (a2,d5), d2     ;3432 5000
          move.w (a3,d4), d3     ;3633 4000
          move.w (a4,d3), d4     ;3834 3000
          move.w (a5,d2), d5     ;3A35 2000
          move.w (a6,d1), d6     ;3C36 1000
          move.w (a7,d0), d7     ;3E37 0000

          move.l (a0,d7), d0     ;2030 7000
          move.l (a1,d6), d1     ;2231 6000
          move.l (a2,d5), d2     ;2432 5000
          move.l (a3,d4), d3     ;2633 4000
          move.l (a4,d3), d4     ;2834 3000
          move.l (a5,d2), d5     ;2A35 2000
          move.l (a6,d1), d6     ;2C36 1000
          move.l (a7,d0), d7     ;2E37 0000

          move (a0,d7), d0       ;3030 7000
          move (a1,d6), d1       ;3231 6000
          move (a2,d5), d2       ;3432 5000
          move (a3,d4), d3       ;3633 4000
          move (a4,d3), d4       ;3834 3000
          move (a5,d2), d5       ;3A35 2000
          move (a6,d1), d6       ;3C36 1000
          move (a7,d0), d7       ;3E37 0000


          move.b $01(a0,d7.w), d0       ;1030 7001
          move.b $02(a1,d6.w), d1       ;1231 6002
          move.b $7E(a2,d5.w), d2       ;1432 507E
          move.b $7F(a3,d4.w), d3       ;1633 407F
          move.b LATE_01(a4,d3.w), d4   ;1834 3001
          move.b LATE_02(a5,d2.w), d5   ;1A35 2002
          move.b LATE_7E(a6,d1.w), d6   ;1C36 107E
          move.b LATE_7F(a7,d0.w), d7   ;1E37 007F

          move.w $01(a0,d7.w), d0       ;3030 7001
          move.w $02(a1,d6.w), d1       ;3231 6002
          move.w $7E(a2,d5.w), d2       ;3432 507E
          move.w $7F(a3,d4.w), d3       ;3633 407F
          move.w LATE_01(a4,d3.w), d4   ;3834 3001
          move.w LATE_02(a5,d2.w), d5   ;3A35 2002
          move.w LATE_7E(a6,d1.w), d6   ;3C36 107E
          move.w LATE_7F(a7,d0.w), d7   ;3E37 007F

          move.l $01(a0,d7.w), d0       ;2030 7001
          move.l $02(a1,d6.w), d1       ;2231 6002
          move.l $7E(a2,d5.w), d2       ;2432 507E
          move.l $7F(a3,d4.w), d3       ;2633 407F
          move.l LATE_01(a4,d3.w), d4   ;2834 3001
          move.l LATE_02(a5,d2.w), d5   ;2A35 2002
          move.l LATE_7E(a6,d1.w), d6   ;2C36 107E
          move.l LATE_7F(a7,d0.w), d7   ;2E37 007F

          move $01(a0,d7.w), d0         ;3030 7001
          move $02(a1,d6.w), d1         ;3231 6002
          move $7E(a2,d5.w), d2         ;3432 507E
          move $7F(a3,d4.w), d3         ;3633 407F
          move LATE_01(a4,d3.w), d4     ;3834 3001
          move LATE_02(a5,d2.w), d5     ;3A35 2002
          move LATE_7E(a6,d1.w), d6     ;3C36 107E
          move LATE_7F(a7,d0.w), d7     ;3E37 007F

          move.b $01(a0,d7.l), d0       ;1030 7801
          move.b $02(a1,d6.l), d1       ;1231 6802
          move.b $7E(a2,d5.l), d2       ;1432 587E
          move.b $7F(a3,d4.l), d3       ;1633 487F
          move.b LATE_01(a4,d3.l), d4   ;1834 3801
          move.b LATE_02(a5,d2.l), d5   ;1A35 2802
          move.b LATE_7E(a6,d1.l), d6   ;1C36 187E
          move.b LATE_7F(a7,d0.l), d7   ;1E37 087F

          move.w $01(a0,d7.l), d0       ;3030 7801
          move.w $02(a1,d6.l), d1       ;3231 6802
          move.w $7E(a2,d5.l), d2       ;3432 587E
          move.w $7F(a3,d4.l), d3       ;3633 487F
          move.w LATE_01(a4,d3.l), d4   ;3834 3801
          move.w LATE_02(a5,d2.l), d5   ;3A35 2802
          move.w LATE_7E(a6,d1.l), d6   ;3C36 187E
          move.w LATE_7F(a7,d0.l), d7   ;3E37 087F

          move.l $01(a0,d7.l), d0       ;2030 7801
          move.l $02(a1,d6.l), d1       ;2231 6802
          move.l $7E(a2,d5.l), d2       ;2432 587E
          move.l $7F(a3,d4.l), d3       ;2633 487F
          move.l LATE_01(a4,d3.l), d4   ;2834 3801
          move.l LATE_02(a5,d2.l), d5   ;2A35 2802
          move.l LATE_7E(a6,d1.l), d6   ;2C36 187E
          move.l LATE_7F(a7,d0.l), d7   ;2E37 087F

          move $01(a0,d7.l), d0         ;3030 7801
          move $02(a1,d6.l), d1         ;3231 6802
          move $7E(a2,d5.l), d2         ;3432 587E
          move $7F(a3,d4.l), d3         ;3633 487F
          move LATE_01(a4,d3.l), d4     ;3834 3801
          move LATE_02(a5,d2.l), d5     ;3A35 2802
          move LATE_7E(a6,d1.l), d6     ;3C36 187E
          move LATE_7F(a7,d0.l), d7     ;3E37 087F

          move.b $01(a0,d7), d0         ;1030 7001
          move.b $02(a1,d6), d1         ;1231 6002
          move.b $7E(a2,d5), d2         ;1432 507E
          move.b $7F(a3,d4), d3         ;1633 407F
          move.b LATE_01(a4,d3), d4     ;1834 3001
          move.b LATE_02(a5,d2), d5     ;1A35 2002
          move.b LATE_7E(a6,d1), d6     ;1C36 107E
          move.b LATE_7F(a7,d0), d7     ;1E37 007F

          move.w $01(a0,d7), d0         ;3030 7001
          move.w $02(a1,d6), d1         ;3231 6002
          move.w $7E(a2,d5), d2         ;3432 507E
          move.w $7F(a3,d4), d3         ;3633 407F
          move.w LATE_01(a4,d3), d4     ;3834 3001
          move.w LATE_02(a5,d2), d5     ;3A35 2002
          move.w LATE_7E(a6,d1), d6     ;3C36 107E
          move.w LATE_7F(a7,d0), d7     ;3E37 007F

          move.l $01(a0,d7), d0         ;2030 7001
          move.l $02(a1,d6), d1         ;2231 6002
          move.l $7E(a2,d5), d2         ;2432 507E
          move.l $7F(a3,d4), d3         ;2633 407F
          move.l LATE_01(a4,d3), d4     ;2834 3001
          move.l LATE_02(a5,d2), d5     ;2A35 2002
          move.l LATE_7E(a6,d1), d6     ;2C36 107E
          move.l LATE_7F(a7,d0), d7     ;2E37 007F

          move $01(a0,d7), d0           ;3030 7001
          move $02(a1,d6), d1           ;3231 6002
          move $7E(a2,d5), d2           ;3432 507E
          move $7F(a3,d4), d3           ;3633 407F
          move LATE_01(a4,d3), d4       ;3834 3001
          move LATE_02(a5,d2), d5       ;3A35 2002
          move LATE_7E(a6,d1), d6       ;3C36 107E
          move LATE_7F(a7,d0), d7       ;3E37 007F


          move.b (a0,d7.w), (a0)        ;10B0 7000
          move.b (a1,d6.w), (a1)        ;12B1 6000
          move.b (a2,d5.w), (a2)        ;14B2 5000
          move.b (a3,d4.w), (a3)        ;16B3 4000
          move.b (a4,d3.w), (a4)        ;18B4 3000
          move.b (a5,d2.w), (a5)        ;1AB5 2000
          move.b (a6,d1.w), (a6)        ;1CB6 1000
          move.b (a7,d0.w), (a7)        ;1EB7 0000

          move.w (a0,d7.w), (a0)        ;30B0 7000
          move.w (a1,d6.w), (a1)        ;32B1 6000
          move.w (a2,d5.w), (a2)        ;34B2 5000
          move.w (a3,d4.w), (a3)        ;36B3 4000
          move.w (a4,d3.w), (a4)        ;38B4 3000
          move.w (a5,d2.w), (a5)        ;3AB5 2000
          move.w (a6,d1.w), (a6)        ;3CB6 1000
          move.w (a7,d0.w), (a7)        ;3EB7 0000

          move.l (a0,d7.w), (a0)        ;20B0 7000
          move.l (a1,d6.w), (a1)        ;22B1 6000
          move.l (a2,d5.w), (a2)        ;24B2 5000
          move.l (a3,d4.w), (a3)        ;26B3 4000
          move.l (a4,d3.w), (a4)        ;28B4 3000
          move.l (a5,d2.w), (a5)        ;2AB5 2000
          move.l (a6,d1.w), (a6)        ;2CB6 1000
          move.l (a7,d0.w), (a7)        ;2EB7 0000

          move (a0,d7.w), (a0)          ;30B0 7000
          move (a1,d6.w), (a1)          ;32B1 6000
          move (a2,d5.w), (a2)          ;34B2 5000
          move (a3,d4.w), (a3)          ;36B3 4000
          move (a4,d3.w), (a4)          ;38B4 3000
          move (a5,d2.w), (a5)          ;3AB5 2000
          move (a6,d1.w), (a6)          ;3CB6 1000
          move (a7,d0.w), (a7)          ;3EB7 0000

          move.b (a0,d7.l), (a0)        ;10B0 7800
          move.b (a1,d6.l), (a1)        ;12B1 6800
          move.b (a2,d5.l), (a2)        ;14B2 5800
          move.b (a3,d4.l), (a3)        ;16B3 4800
          move.b (a4,d3.l), (a4)        ;18B4 3800
          move.b (a5,d2.l), (a5)        ;1AB5 2800
          move.b (a6,d1.l), (a6)        ;1CB6 1800
          move.b (a7,d0.l), (a7)        ;1EB7 0800

          move.w (a0,d7.l), (a0)        ;30B0 7800
          move.w (a1,d6.l), (a1)        ;32B1 6800
          move.w (a2,d5.l), (a2)        ;34B2 5800
          move.w (a3,d4.l), (a3)        ;36B3 4800
          move.w (a4,d3.l), (a4)        ;38B4 3800
          move.w (a5,d2.l), (a5)        ;3AB5 2800
          move.w (a6,d1.l), (a6)        ;3CB6 1800
          move.w (a7,d0.l), (a7)        ;3EB7 0800

          move.l (a0,d7.l), (a0)        ;20B0 7800
          move.l (a1,d6.l), (a1)        ;22B1 6800
          move.l (a2,d5.l), (a2)        ;24B2 5800
          move.l (a3,d4.l), (a3)        ;26B3 4800
          move.l (a4,d3.l), (a4)        ;28B4 3800
          move.l (a5,d2.l), (a5)        ;2AB5 2800
          move.l (a6,d1.l), (a6)        ;2CB6 1800
          move.l (a7,d0.l), (a7)        ;2EB7 0800

          move (a0,d7.l), (a0)          ;30B0 7800
          move (a1,d6.l), (a1)          ;32B1 6800
          move (a2,d5.l), (a2)          ;34B2 5800
          move (a3,d4.l), (a3)          ;36B3 4800
          move (a4,d3.l), (a4)          ;38B4 3800
          move (a5,d2.l), (a5)          ;3AB5 2800
          move (a6,d1.l), (a6)          ;3CB6 1800
          move (a7,d0.l), (a7)          ;3EB7 0800

          move.b (a0,d7), (a0)          ;10B0 7000
          move.b (a1,d6), (a1)          ;12B1 6000
          move.b (a2,d5), (a2)          ;14B2 5000
          move.b (a3,d4), (a3)          ;16B3 4000
          move.b (a4,d3), (a4)          ;18B4 3000
          move.b (a5,d2), (a5)          ;1AB5 2000
          move.b (a6,d1), (a6)          ;1CB6 1000
          move.b (a7,d0), (a7)          ;1EB7 0000

          move.w (a0,d7), (a0)          ;30B0 7000
          move.w (a1,d6), (a1)          ;32B1 6000
          move.w (a2,d5), (a2)          ;34B2 5000
          move.w (a3,d4), (a3)          ;36B3 4000
          move.w (a4,d3), (a4)          ;38B4 3000
          move.w (a5,d2), (a5)          ;3AB5 2000
          move.w (a6,d1), (a6)          ;3CB6 1000
          move.w (a7,d0), (a7)          ;3EB7 0000

          move.l (a0,d7), (a0)          ;20B0 7000
          move.l (a1,d6), (a1)          ;22B1 6000
          move.l (a2,d5), (a2)          ;24B2 5000
          move.l (a3,d4), (a3)          ;26B3 4000
          move.l (a4,d3), (a4)          ;28B4 3000
          move.l (a5,d2), (a5)          ;2AB5 2000
          move.l (a6,d1), (a6)          ;2CB6 1000
          move.l (a7,d0), (a7)          ;2EB7 0000

          move (a0,d7), (a0)            ;30B0 7000
          move (a1,d6), (a1)            ;32B1 6000
          move (a2,d5), (a2)            ;34B2 5000
          move (a3,d4), (a3)            ;36B3 4000
          move (a4,d3), (a4)            ;38B4 3000
          move (a5,d2), (a5)            ;3AB5 2000
          move (a6,d1), (a6)            ;3CB6 1000
          move (a7,d0), (a7)            ;3EB7 0000


          move.b $01(a0,d7.w), $FF(a0)            ;1170 7001 00FF
          move.b $02(a1,d6.w), $FE(a1)            ;1371 6002 00FE
          move.b $7E(a2,d5.w), LATE_02(a2)        ;1572 50FE 0002
          move.b $7F(a3,d4.w), LATE_01(a3)        ;1773 40FF 0001
          move.b LATE_01(a4,d3.w), $FF(a4)        ;1974 3001 00FF
          move.b LATE_02(a5,d2.w), $FE(a5)        ;1B75 2002 00FE
          move.b LATE_7E(a6,d1.w), LATE_02(a6)    ;1D76 10FE 0002
          move.b LATE_7F(a7,d0.w), LATE_01(a7)    ;1F77 00FF 0001

          move.w $01(a0,d7.w), $FF(a0)            ;3170 7001 00FF
          move.w $02(a1,d6.w), $FE(a1)            ;3371 6002 00FE
          move.w $7E(a2,d5.w), LATE_02(a2)        ;3572 50FE 0002
          move.w $7F(a3,d4.w), LATE_01(a3)        ;3773 40FF 0001
          move.w LATE_01(a4,d3.w), $FF(a4)        ;3974 3001 00FF
          move.w LATE_02(a5,d2.w), $FE(a5)        ;3B75 2002 00FE
          move.w LATE_7E(a6,d1.w), LATE_02(a6)    ;3D76 10FE 0002
          move.w LATE_7F(a7,d0.w), LATE_01(a7)    ;3F77 00FF 0001

          move.l $01(a0,d7.w), $FF(a0)            ;2170 7001 00FF
          move.l $02(a1,d6.w), $FE(a1)            ;2371 6002 00FE
          move.l $7E(a2,d5.w), LATE_02(a2)        ;2572 50FE 0002
          move.l $7F(a3,d4.w), LATE_01(a3)        ;2773 40FF 0001
          move.l LATE_01(a4,d3.w), $FF(a4)        ;2974 3001 00FF
          move.l LATE_02(a5,d2.w), $FE(a5)        ;2B75 2002 00FE
          move.l LATE_7E(a6,d1.w), LATE_02(a6)    ;2D76 10FE 0002
          move.l LATE_7F(a7,d0.w), LATE_01(a7)    ;2F77 00FF 0001

          move $01(a0,d7.w), $FF(a0)              ;3170 7001 00FF
          move $02(a1,d6.w), $FE(a1)              ;3371 6002 00FE
          move $7E(a2,d5.w), LATE_02(a2)          ;3572 50FE 0002
          move $7F(a3,d4.w), LATE_01(a3)          ;3773 40FF 0001
          move LATE_01(a4,d3.w), $FF(a4)          ;3974 3001 00FF
          move LATE_02(a5,d2.w), $FE(a5)          ;3B75 2002 00FE
          move LATE_7E(a6,d1.w), LATE_02(a6)      ;3D76 10FE 0002
          move LATE_7F(a7,d0.w), LATE_01(a7)      ;3F77 00FF 0001

          move.b $01(a0,d7.l), $FF(a0)            ;1170 7801 00FF
          move.b $02(a1,d6.l), $FE(a1)            ;1371 6802 00FE
          move.b $7E(a2,d5.l), LATE_02(a2)        ;1572 58FE 0002
          move.b $7F(a3,d4.l), LATE_01(a3)        ;1773 48FF 0001
          move.b LATE_01(a4,d3.l), $FF(a4)        ;1974 3801 00FF
          move.b LATE_02(a5,d2.l), $FE(a5)        ;1B75 2802 00FE
          move.b LATE_7E(a6,d1.l), LATE_02(a6)    ;1D76 18FE 0002
          move.b LATE_7F(a7,d0.l), LATE_01(a7)    ;1F77 08FF 0001

          move.w $01(a0,d7.l), $FF(a0)            ;3170 7801 00FF
          move.w $02(a1,d6.l), $FE(a1)            ;3371 6802 00FE
          move.w $7E(a2,d5.l), LATE_02(a2)        ;3572 58FE 0002
          move.w $7F(a3,d4.l), LATE_01(a3)        ;3773 48FF 0001
          move.w LATE_01(a4,d3.l), $FF(a4)        ;3974 3801 00FF
          move.w LATE_02(a5,d2.l), $FE(a5)        ;3B75 2802 00FE
          move.w LATE_7E(a6,d1.l), LATE_02(a6)    ;3D76 18FE 0002
          move.w LATE_7F(a7,d0.l), LATE_01(a7)    ;3F77 08FF 0001

          move.l $01(a0,d7.l), $FF(a0)            ;2170 7801 00FF
          move.l $02(a1,d6.l), $FE(a1)            ;2371 6802 00FE
          move.l $7E(a2,d5.l), LATE_02(a2)        ;2572 58FE 0002
          move.l $7F(a3,d4.l), LATE_01(a3)        ;2773 48FF 0001
          move.l LATE_01(a4,d3.l), $FF(a4)        ;2974 3801 00FF
          move.l LATE_02(a5,d2.l), $FE(a5)        ;2B75 2802 00FE
          move.l LATE_7E(a6,d1.l), LATE_02(a6)    ;2D76 18FE 0002
          move.l LATE_7F(a7,d0.l), LATE_01(a7)    ;2F77 08FF 0001

          move $01(a0,d7.l), $FF(a0)              ;3170 7801 00FF
          move $02(a1,d6.l), $FE(a1)              ;3371 6802 00FE
          move $7E(a2,d5.l), LATE_02(a2)          ;3572 58FE 0002
          move $7F(a3,d4.l), LATE_01(a3)          ;3773 48FF 0001
          move LATE_01(a4,d3.l), $FF(a4)          ;3974 3801 00FF
          move LATE_02(a5,d2.l), $FE(a5)          ;3B75 2802 00FE
          move LATE_7E(a6,d1.l), LATE_02(a6)      ;3D76 18FE 0002
          move LATE_7F(a7,d0.l), LATE_01(a7)      ;3F77 08FF 0001

          move.b $01(a0,d7), $FF(a0)              ;1170 7001 00FF
          move.b $02(a1,d6), $FE(a1)              ;1371 6002 00FE
          move.b $7E(a2,d5), LATE_02(a2)          ;1572 50FE 0002
          move.b $7F(a3,d4), LATE_01(a3)          ;1773 40FF 0001
          move.b LATE_01(a4,d3), $FF(a4)          ;1974 3001 00FF
          move.b LATE_02(a5,d2), $FE(a5)          ;1B75 2002 00FE
          move.b LATE_7E(a6,d1), LATE_02(a6)      ;1D76 10FE 0002
          move.b LATE_7F(a7,d0), LATE_01(a7)      ;1F77 00FF 0001

          move.w $01(a0,d7), $FF(a0)              ;3170 7001 00FF
          move.w $02(a1,d6), $FE(a1)              ;3371 6002 00FE
          move.w $7E(a2,d5), LATE_02(a2)          ;3572 50FE 0002
          move.w $7F(a3,d4), LATE_01(a3)          ;3773 40FF 0001
          move.w LATE_01(a4,d3), $FF(a4)          ;3974 3001 00FF
          move.w LATE_02(a5,d2), $FE(a5)          ;3B75 2002 00FE
          move.w LATE_7E(a6,d1), LATE_02(a6)      ;3D76 10FE 0002
          move.w LATE_7F(a7,d0), LATE_01(a7)      ;3F77 00FF 0001

          move.l $01(a0,d7), $FF(a0)              ;2170 7001 00FF
          move.l $02(a1,d6), $FE(a1)              ;2371 6002 00FE
          move.l $7E(a2,d5), LATE_02(a2)          ;2572 50FE 0002
          move.l $7F(a3,d4), LATE_01(a3)          ;2773 40FF 0001
          move.l LATE_01(a4,d3), $FF(a4)          ;2974 3001 00FF
          move.l LATE_02(a5,d2), $FE(a5)          ;2B75 2002 00FE
          move.l LATE_7E(a6,d1), LATE_02(a6)      ;2D76 10FE 0002
          move.l LATE_7F(a7,d0), LATE_01(a7)      ;2F77 00FF 0001

          move $01(a0,d7), $FF(a0)                ;3170 7001 00FF
          move $02(a1,d6), $FE(a1)                ;3371 6002 00FE
          move $7E(a2,d5), LATE_02(a2)            ;3572 50FE 0002
          move $7F(a3,d4), LATE_01(a3)            ;3773 40FF 0001
          move LATE_01(a4,d3), $FF(a4)            ;3974 3001 00FF
          move LATE_02(a5,d2), $FE(a5)            ;3B75 2002 00FE
          move LATE_7E(a6,d1), LATE_02(a6)        ;3D76 10FE 0002
          move LATE_7F(a7,d0), LATE_01(a7)        ;3F77 00FF 0001


          move.b (a0,d7.w), (a0)+       ;10F0 7000
          move.b (a1,d6.w), (a1)+       ;12F1 6000
          move.b (a2,d5.w), (a2)+       ;14F2 5000
          move.b (a3,d4.w), (a3)+       ;16F3 4000
          move.b (a4,d3.w), (a4)+       ;18F4 3000
          move.b (a5,d2.w), (a5)+       ;1AF5 2000
          move.b (a6,d1.w), (a6)+       ;1CF6 1000
          move.b (a7,d0.w), (a7)+       ;1EF7 0000

          move.w (a0,d7.w), (a0)+       ;30F0 7000
          move.w (a1,d6.w), (a1)+       ;32F1 6000
          move.w (a2,d5.w), (a2)+       ;34F2 5000
          move.w (a3,d4.w), (a3)+       ;36F3 4000
          move.w (a4,d3.w), (a4)+       ;38F4 3000
          move.w (a5,d2.w), (a5)+       ;3AF5 2000
          move.w (a6,d1.w), (a6)+       ;3CF6 1000
          move.w (a7,d0.w), (a7)+       ;3EF7 0000

          move.l (a0,d7.w), (a0)+       ;20F0 7000
          move.l (a1,d6.w), (a1)+       ;22F1 6000
          move.l (a2,d5.w), (a2)+       ;24F2 5000
          move.l (a3,d4.w), (a3)+       ;26F3 4000
          move.l (a4,d3.w), (a4)+       ;28F4 3000
          move.l (a5,d2.w), (a5)+       ;2AF5 2000
          move.l (a6,d1.w), (a6)+       ;2CF6 1000
          move.l (a7,d0.w), (a7)+       ;2EF7 0000

          move (a0,d7.w), (a0)+         ;30F0 7000
          move (a1,d6.w), (a1)+         ;32F1 6000
          move (a2,d5.w), (a2)+         ;34F2 5000
          move (a3,d4.w), (a3)+         ;36F3 4000
          move (a4,d3.w), (a4)+         ;38F4 3000
          move (a5,d2.w), (a5)+         ;3AF5 2000
          move (a6,d1.w), (a6)+         ;3CF6 1000
          move (a7,d0.w), (a7)+         ;3EF7 0000

          move.b (a0,d7.l), (a0)+       ;10F0 7800
          move.b (a1,d6.l), (a1)+       ;12F1 6800
          move.b (a2,d5.l), (a2)+       ;14F2 5800
          move.b (a3,d4.l), (a3)+       ;16F3 4800
          move.b (a4,d3.l), (a4)+       ;18F4 3800
          move.b (a5,d2.l), (a5)+       ;1AF5 2800
          move.b (a6,d1.l), (a6)+       ;1CF6 1800
          move.b (a7,d0.l), (a7)+       ;1EF7 0800

          move.w (a0,d7.l), (a0)+       ;30F0 7800
          move.w (a1,d6.l), (a1)+       ;32F1 6800
          move.w (a2,d5.l), (a2)+       ;34F2 5800
          move.w (a3,d4.l), (a3)+       ;36F3 4800
          move.w (a4,d3.l), (a4)+       ;38F4 3800
          move.w (a5,d2.l), (a5)+       ;3AF5 2800
          move.w (a6,d1.l), (a6)+       ;3CF6 1800
          move.w (a7,d0.l), (a7)+       ;3EF7 0800

          move.l (a0,d7.l), (a0)+       ;20F0 7800
          move.l (a1,d6.l), (a1)+       ;22F1 6800
          move.l (a2,d5.l), (a2)+       ;24F2 5800
          move.l (a3,d4.l), (a3)+       ;26F3 4800
          move.l (a4,d3.l), (a4)+       ;28F4 3800
          move.l (a5,d2.l), (a5)+       ;2AF5 2800
          move.l (a6,d1.l), (a6)+       ;2CF6 1800
          move.l (a7,d0.l), (a7)+       ;2EF7 0800

          move (a0,d7.l), (a0)+         ;30F0 7800
          move (a1,d6.l), (a1)+         ;32F1 6800
          move (a2,d5.l), (a2)+         ;34F2 5800
          move (a3,d4.l), (a3)+         ;36F3 4800
          move (a4,d3.l), (a4)+         ;38F4 3800
          move (a5,d2.l), (a5)+         ;3AF5 2800
          move (a6,d1.l), (a6)+         ;3CF6 1800
          move (a7,d0.l), (a7)+         ;3EF7 0800

          move.b (a0,d7), (a0)+         ;10F0 7000
          move.b (a1,d6), (a1)+         ;12F1 6000
          move.b (a2,d5), (a2)+         ;14F2 5000
          move.b (a3,d4), (a3)+         ;16F3 4000
          move.b (a4,d3), (a4)+         ;18F4 3000
          move.b (a5,d2), (a5)+         ;1AF5 2000
          move.b (a6,d1), (a6)+         ;1CF6 1000
          move.b (a7,d0), (a7)+         ;1EF7 0000

          move.w (a0,d7), (a0)+         ;30F0 7000
          move.w (a1,d6), (a1)+         ;32F1 6000
          move.w (a2,d5), (a2)+         ;34F2 5000
          move.w (a3,d4), (a3)+         ;36F3 4000
          move.w (a4,d3), (a4)+         ;38F4 3000
          move.w (a5,d2), (a5)+         ;3AF5 2000
          move.w (a6,d1), (a6)+         ;3CF6 1000
          move.w (a7,d0), (a7)+         ;3EF7 0000

          move.l (a0,d7), (a0)+         ;20F0 7000
          move.l (a1,d6), (a1)+         ;22F1 6000
          move.l (a2,d5), (a2)+         ;24F2 5000
          move.l (a3,d4), (a3)+         ;26F3 4000
          move.l (a4,d3), (a4)+         ;28F4 3000
          move.l (a5,d2), (a5)+         ;2AF5 2000
          move.l (a6,d1), (a6)+         ;2CF6 1000
          move.l (a7,d0), (a7)+         ;2EF7 0000

          move (a0,d7), (a0)+           ;30F0 7000
          move (a1,d6), (a1)+           ;32F1 6000
          move (a2,d5), (a2)+           ;34F2 5000
          move (a3,d4), (a3)+           ;36F3 4000
          move (a4,d3), (a4)+           ;38F4 3000
          move (a5,d2), (a5)+           ;3AF5 2000
          move (a6,d1), (a6)+           ;3CF6 1000
          move (a7,d0), (a7)+           ;3EF7 0000


          move.b (a0,d7.w), -(a0)       ;1130 7000
          move.b (a1,d6.w), -(a1)       ;1331 6000
          move.b (a2,d5.w), -(a2)       ;1532 5000
          move.b (a3,d4.w), -(a3)       ;1733 4000
          move.b (a4,d3.w), -(a4)       ;1934 3000
          move.b (a5,d2.w), -(a5)       ;1B35 2000
          move.b (a6,d1.w), -(a6)       ;1D36 1000
          move.b (a7,d0.w), -(a7)       ;1F37 0000

          move.w (a0,d7.w), -(a0)       ;3130 7000
          move.w (a1,d6.w), -(a1)       ;3331 6000
          move.w (a2,d5.w), -(a2)       ;3532 5000
          move.w (a3,d4.w), -(a3)       ;3733 4000
          move.w (a4,d3.w), -(a4)       ;3934 3000
          move.w (a5,d2.w), -(a5)       ;3B35 2000
          move.w (a6,d1.w), -(a6)       ;3D36 1000
          move.w (a7,d0.w), -(a7)       ;3F37 0000

          move.l (a0,d7.w), -(a0)       ;2130 7000
          move.l (a1,d6.w), -(a1)       ;2331 6000
          move.l (a2,d5.w), -(a2)       ;2532 5000
          move.l (a3,d4.w), -(a3)       ;2733 4000
          move.l (a4,d3.w), -(a4)       ;2934 3000
          move.l (a5,d2.w), -(a5)       ;2B35 2000
          move.l (a6,d1.w), -(a6)       ;2D36 1000
          move.l (a7,d0.w), -(a7)       ;2F37 0000

          move (a0,d7.w), -(a0)         ;3130 7000
          move (a1,d6.w), -(a1)         ;3331 6000
          move (a2,d5.w), -(a2)         ;3532 5000
          move (a3,d4.w), -(a3)         ;3733 4000
          move (a4,d3.w), -(a4)         ;3934 3000
          move (a5,d2.w), -(a5)         ;3B35 2000
          move (a6,d1.w), -(a6)         ;3D36 1000
          move (a7,d0.w), -(a7)         ;3F37 0000

          move.b (a0,d7.l), -(a0)       ;1130 7800
          move.b (a1,d6.l), -(a1)       ;1331 6800
          move.b (a2,d5.l), -(a2)       ;1532 5800
          move.b (a3,d4.l), -(a3)       ;1733 4800
          move.b (a4,d3.l), -(a4)       ;1934 3800
          move.b (a5,d2.l), -(a5)       ;1B35 2800
          move.b (a6,d1.l), -(a6)       ;1D36 1800
          move.b (a7,d0.l), -(a7)       ;1F37 0800

          move.w (a0,d7.l), -(a0)       ;3130 7800
          move.w (a1,d6.l), -(a1)       ;3331 6800
          move.w (a2,d5.l), -(a2)       ;3532 5800
          move.w (a3,d4.l), -(a3)       ;3733 4800
          move.w (a4,d3.l), -(a4)       ;3934 3800
          move.w (a5,d2.l), -(a5)       ;3B35 2800
          move.w (a6,d1.l), -(a6)       ;3D36 1800
          move.w (a7,d0.l), -(a7)       ;3F37 0800

          move.l (a0,d7.l), -(a0)       ;2130 7800
          move.l (a1,d6.l), -(a1)       ;2331 6800
          move.l (a2,d5.l), -(a2)       ;2532 5800
          move.l (a3,d4.l), -(a3)       ;2733 4800
          move.l (a4,d3.l), -(a4)       ;2934 3800
          move.l (a5,d2.l), -(a5)       ;2B35 2800
          move.l (a6,d1.l), -(a6)       ;2D36 1800
          move.l (a7,d0.l), -(a7)       ;2F37 0800

          move (a0,d7.w), -(a0)         ;3130 7000
          move (a1,d6.w), -(a1)         ;3331 6000
          move (a2,d5.w), -(a2)         ;3532 5000
          move (a3,d4.w), -(a3)         ;3733 4000
          move (a4,d3.w), -(a4)         ;3934 3000
          move (a5,d2.w), -(a5)         ;3B35 2000
          move (a6,d1.w), -(a6)         ;3D36 1000
          move (a7,d0.w), -(a7)         ;3F37 0000

          move.b (a0,d7), -(a0)         ;1130 7000
          move.b (a1,d6), -(a1)         ;1331 6000
          move.b (a2,d5), -(a2)         ;1532 5000
          move.b (a3,d4), -(a3)         ;1733 4000
          move.b (a4,d3), -(a4)         ;1934 3000
          move.b (a5,d2), -(a5)         ;1B35 2000
          move.b (a6,d1), -(a6)         ;1D36 1000
          move.b (a7,d0), -(a7)         ;1F37 0000

          move.w (a0,d7), -(a0)         ;3130 7000
          move.w (a1,d6), -(a1)         ;3331 6000
          move.w (a2,d5), -(a2)         ;3532 5000
          move.w (a3,d4), -(a3)         ;3733 4000
          move.w (a4,d3), -(a4)         ;3934 3000
          move.w (a5,d2), -(a5)         ;3B35 2000
          move.w (a6,d1), -(a6)         ;3D36 1000
          move.w (a7,d0), -(a7)         ;3F37 0000

          move.l (a0,d7), -(a0)         ;2130 7000
          move.l (a1,d6), -(a1)         ;2331 6000
          move.l (a2,d5), -(a2)         ;2532 5000
          move.l (a3,d4), -(a3)         ;2733 4000
          move.l (a4,d3), -(a4)         ;2934 3000
          move.l (a5,d2), -(a5)         ;2B35 2000
          move.l (a6,d1), -(a6)         ;2D36 1000
          move.l (a7,d0), -(a7)         ;2F37 0000

          move (a0,d7), -(a0)           ;3130 7000
          move (a1,d6), -(a1)           ;3331 6000
          move (a2,d5), -(a2)           ;3532 5000
          move (a3,d4), -(a3)           ;3733 4000
          move (a4,d3), -(a4)           ;3934 3000
          move (a5,d2), -(a5)           ;3B35 2000
          move (a6,d1), -(a6)           ;3D36 1000
          move (a7,d0), -(a7)           ;3F37 0000


          ;TODO ??
          move.b $01(pc,d7.w), ($FF00).w    ;11FB 7001 FF00   -Displacement out of range
          move.b $02(pc,d6.w), ($0100).w    ;11FB 6002 0100   -Displacement out of range
          move.b $7E(pc,d5.w), ($00FF).w    ;11FB 507E 00FF   -Displacement out of range
          move.b $7F(pc,d4.w), ($0001).w    ;11FB 407F 0001   -Displacement out of range
          move.b $01(pc,d3.w), ($FF00).w    ;11FB 3001 FF00   -Displacement out of range
          move.b $02(pc,d2.w), ($0100).w    ;11FB 2002 0100   -Displacement out of range
          move.b $7E(pc,d1.w), ($00FF).w    ;11FB 107E 00FF   -Displacement out of range
          move.b $7F(pc,d0.w), ($0001).w    ;11FB 007F 0001   -Displacement out of range

          ;move.w $01(pc,d7.w), ($FF00).w    ;31FB 7001 FF00   -Displacement out of range
;          move.w $02(pc,d6.w), ($0100).w    ;31FB 6002 0100   -Displacement out of range
;          move.w $7E(pc,d5.w), ($00FF).w    ;31FB 507E 00FF   -Displacement out of range
;          move.w $7F(pc,d4.w), ($0001).w    ;31FB 407F 0001   -Displacement out of range
;          move.w $01(pc,d3.w), ($FF00).w    ;31FB 3001 FF00   -Displacement out of range
;          move.w $02(pc,d2.w), ($0100).w    ;31FB 2002 0100   -Displacement out of range
;          move.w $7E(pc,d1.w), ($00FF).w    ;31FB 107E 00FF   -Displacement out of range
;          move.w $7F(pc,d0.w), ($0001).w    ;31FB 007F 0001   -Displacement out of range
;
;          move.l $01(pc,d7.w), ($FF00).w    ;21FB 7001 FF00   -Displacement out of range
;          move.l $02(pc,d6.w), ($0100).w    ;21FB 6002 0100   -Displacement out of range
;          move.l $7E(pc,d5.w), ($00FF).w    ;21FB 507E 00FF   -Displacement out of range
;          move.l $7F(pc,d4.w), ($0001).w    ;21FB 407F 0001   -Displacement out of range
;          move.l $01(pc,d3.w), ($FF00).w    ;21FB 3001 FF00   -Displacement out of range
;          move.l $02(pc,d2.w), ($0100).w    ;21FB 2002 0100   -Displacement out of range
;          move.l $7E(pc,d1.w), ($00FF).w    ;21FB 107E 00FF   -Displacement out of range
;          move.l $7F(pc,d0.w), ($0001).w    ;21FB 007F 0001   -Displacement out of range
;
;          move $01(pc,d7.w), ($FF00).w      ;31FB 7001 FF00   -Displacement out of range
;          move $02(pc,d6.w), ($0100).w      ;31FB 6002 0100   -Displacement out of range
;          move $7E(pc,d5.w), ($00FF).w      ;31FB 507E 00FF   -Displacement out of range
;          move $7F(pc,d4.w), ($0001).w      ;31FB 407F 0001   -Displacement out of range
;          move $01(pc,d3.w), ($FF00).w      ;31FB 3001 FF00   -Displacement out of range
;          move $02(pc,d2.w), ($0100).w      ;31FB 2002 0100   -Displacement out of range
;          move $7E(pc,d1.w), ($00FF).w      ;31FB 107E 00FF   -Displacement out of range
;          move $7F(pc,d0.w), ($0001).w      ;31FB 007F 0001   -Displacement out of range



          move.b $01(pc,d7.w), d0       ;10 3B 70 01  -Displacement out of range
          move.b $02(pc,d6.w), d1       ;12 3B 60 02  -Displacement out of range
          move.b $7E(pc,d5.w), d2       ;14 3B 50 FE  -Displacement out of range
          move.b $7F(pc,d4.w), d3       ;16 3B 40 FF  -Displacement out of range
          move.b LATE_01(pc,d3.w), d4   ;18 3B 30 01  -Displacement out of range
          move.b LATE_02(pc,d2.w), d5   ;1A 3B 20 02  -Displacement out of range
          move.b LATE_7E(pc,d1.w), d6   ;1C 3B 10 FE  -Displacement out of range
          move.b LATE_7F(pc,d0.w), d7   ;1E 3B 00 FF  -Displacement out of range

          move.b $01(pc,d7.w), (a0)     ;10 BB 70 01  -Displacement out of range
          move.b $02(pc,d6.w), (a1)     ;12 BB 60 02  -Displacement out of range
          move.b $7E(pc,d5.w), (a2)     ;14 BB 50 7E  -Displacement out of range
          move.b $7F(pc,d4.w), (a3)     ;16 BB 40 7F  -Displacement out of range
          move.b LATE_01(pc,d3.w), (a4) ;18 BB 30 01  -Displacement out of range
          move.b LATE_02(pc,d2.w), (a5) ;1A BB 20 02  -Displacement out of range
          move.b LATE_7E(pc,d1.w), (a6) ;1C BB 10 7E  -Displacement out of range
          move.b LATE_7F(pc,d0.w), (a7) ;1E BB 00 7F  -Displacement out of range

          move.b $01(pc,d7.w), $7F(a0)          ;11 7B 70 01 00 7F  -Displacement out of range
          move.b $02(pc,d6.w), $7E(a1)          ;13 7B 60 02 00 7E  -Displacement out of range
          move.b $7E(pc,d5.w), LATE_02(a2)      ;15 7B 50 7E 00 02  -Displacement out of range
          move.b $7F(pc,d4.w), LATE_01(a3)      ;17 7B 40 7F 00 01  -Displacement out of range
          move.b LATE_01(pc,d3.w), $7F(a4)      ;19 7B 30 01 00 7F  -Displacement out of range
          move.b LATE_02(pc,d2.w), $7E(a5)      ;1B 7B 20 02 00 7E  -Displacement out of range
          move.b LATE_7E(pc,d1.w), LATE_02(a6)  ;1D 7B 10 7E 00 02  -Displacement out of range
          move.b LATE_7F(pc,d0.w), LATE_01(a7)  ;1F 7B 00 7F 00 01  -Displacement out of range


          move #$0001,sr                ;46 FC 00 01
          move #$FF00,sr                ;46 FC FF 00
          move #LATE_0001,sr            ;46 FC 00 01
          move #LATE_FF00,sr            ;46 FC FF 00


          movea.w #$0001,a0             ;307C 0001
          movea.w #$00FF,a1             ;327C 00FF
          movea.w #$0100,a2             ;347C 0100
          movea.w #$FF00,a3             ;367C FF00
          movea.w #LATE_0001,a4         ;387C 0001
          movea.w #LATE_00FF,a5         ;3A7C 00FF
          movea.w #LATE_0100,a6         ;3C7C 0100
          movea.w #LATE_FF00,a7         ;3E7C FF00

          movea.l #$0001,a0             ;207C 00000001
          movea.l #$00FF,a1             ;227C 000000FF
          movea.l #$0100,a2             ;247C 00000100
          movea.l #$FF00,a3             ;267C 0000FF00
          movea.l #LATE_0001,a4         ;287C 00000001
          movea.l #LATE_00FF,a5         ;2A7C 000000FF
          movea.l #LATE_0100,a6         ;2C7C 00000100
          movea.l #LATE_FF00,a7         ;2E7C 0000FF00

          movea #$0001,a0               ;307C 0001
          movea #$00FF,a1               ;327C 00FF
          movea #$0100,a2               ;347C 0100
          movea #$FF00,a3               ;367C FF00
          movea #LATE_0001,a4           ;387C 0001
          movea #LATE_00FF,a5           ;3A7C 00FF
          movea #LATE_0100,a6           ;3C7C 0100
          movea #LATE_FF00,a7           ;3E7C FF00


          ;TODO ??
          movea.w ($0001).w,a7          ;3E 78 00 01 -Invalid syntax
          movea.w ($00FF).w,a6          ;3C 78 00 FF -Invalid syntax
          movea.w ($0100).w,a5          ;3A 78 01 00 -Invalid syntax
          movea.w ($FF00).w,a4          ;38 78 FF 00 -Invalid syntax
          movea.w (LATE_0001).w,a3      ;36 78 00 01 -Invalid syntax
          movea.w (LATE_00FF).w,a2      ;34 78 00 FF -Invalid syntax
          movea.w (LATE_0100).w,a1      ;32 78 01 00 -Invalid syntax
          movea.w (LATE_FF00).w,a0      ;30 78 FF 00 -Invalid syntax


          movea.w d7, a0                ;3047
          movea.w d6, a1                ;3246
          movea.w d5, a2                ;3445
          movea.w d4, a3                ;3644
          movea.w d3, a4                ;3843
          movea.w d2, a5                ;3A42
          movea.w d1, a6                ;3C41
          movea.w d0, a7                ;3E40
          movea.w d0, sp                ;3E40

          movea.l d7, a0                ;2047
          movea.l d6, a1                ;2246
          movea.l d5, a2                ;2445
          movea.l d4, a3                ;2644
          movea.l d3, a4                ;2843
          movea.l d2, a5                ;2A42
          movea.l d1, a6                ;2C41
          movea.l d0, a7                ;2E40
          movea.l d0, sp                ;2E40

          movea d7, a0                  ;3047
          movea d6, a1                  ;3246
          movea d5, a2                  ;3445
          movea d4, a3                  ;3644
          movea d3, a4                  ;3843
          movea d2, a5                  ;3A42
          movea d1, a6                  ;3C41
          movea d0, a7                  ;3E40
          movea d0, sp                  ;3E40


          movea.w a7, a0                ;304F
          movea.w a6, a1                ;324E
          movea.w a5, a2                ;344D
          movea.w a4, a3                ;364C
          movea.w a3, a4                ;384B
          movea.w a2, a5                ;3A4A
          movea.w a1, a6                ;3C49
          movea.w a0, a7                ;3E48

          movea.l a7, a0                ;204F
          movea.l a6, a1                ;224E
          movea.l a5, a2                ;244D
          movea.l a4, a3                ;264C
          movea.l a3, a4                ;284B
          movea.l a2, a5                ;2A4A
          movea.l a1, a6                ;2C49
          movea.l a0, a7                ;2E48

          movea a7, a0                  ;304F
          movea a6, a1                  ;324E
          movea a5, a2                  ;344D
          movea a4, a3                  ;364C
          movea a3, a4                  ;384B
          movea a2, a5                  ;3A4A
          movea a1, a6                  ;3C49
          movea a0, a7                  ;3E48


          movea.w (a7), a0              ;3057
          movea.w (sp), a0              ;3057
          movea.w (a6), a1              ;3256
          movea.w (a5), a2              ;3455
          movea.w (a4), a3              ;3654
          movea.w (a3), a4              ;3853
          movea.w (a2), a5              ;3A52
          movea.w (a1), a6              ;3C51
          movea.w (a0), a7              ;3E50

          movea.l (a7), a0              ;2057
          movea.l (sp), a0              ;2057
          movea.l (a6), a1              ;2256
          movea.l (a5), a2              ;2455
          movea.l (a4), a3              ;2654
          movea.l (a3), a4              ;2853
          movea.l (a2), a5              ;2A52
          movea.l (a1), a6              ;2C51
          movea.l (a0), a7              ;2E50

          movea (a7), a0                ;3057
          movea (sp), a0                ;3057
          movea (a6), a1                ;3256
          movea (a5), a2                ;3455
          movea (a4), a3                ;3654
          movea (a3), a4                ;3853
          movea (a2), a5                ;3A52
          movea (a1), a6                ;3C51
          movea (a0), a7                ;3E50


          movea.w $01(a7), a0           ;306F 0001
          movea.w $02(a6), a1           ;326E 0002
          movea.w $FE(a5), a2           ;346D 00FE
          movea.w $FF(a4), a3           ;366C 00FF
          movea.w LATE_01(a3), a4       ;386B 0001
          movea.w LATE_02(a2), a5       ;3A6A 0002
          movea.w LATE_FE(a1), a6       ;3C69 00FE
          movea.w LATE_FF(a0), a7       ;3E68 00FF

          movea.l $01(a7), a0           ;206F 0001
          movea.l $02(a6), a1           ;226E 0002
          movea.l $FE(a5), a2           ;246D 00FE
          movea.l $FF(a4), a3           ;266C 00FF
          movea.l LATE_01(a3), a4       ;286B 0001
          movea.l LATE_02(a2), a5       ;2A6A 0002
          movea.l LATE_FE(a1), a6       ;2C69 00FE
          movea.l LATE_FF(a0), a7       ;2E68 00FF

          movea $01(a7), a0             ;306F 0001
          movea $02(a6), a1             ;326E 0002
          movea $FE(a5), a2             ;346D 00FE
          movea $FF(a4), a3             ;366C 00FF
          movea LATE_01(a3), a4         ;386B 0001
          movea LATE_02(a2), a5         ;3A6A 0002
          movea LATE_FE(a1), a6         ;3C69 00FE
          movea LATE_FF(a0), a7         ;3E68 00FF


          movea.w (a7)+, a0             ;305F
          movea.w (sp)+, a0             ;305F
          movea.w (a6)+, a1             ;325E
          movea.w (a5)+, a2             ;345D
          movea.w (a4)+, a3             ;365C
          movea.w (a3)+, a4             ;385B
          movea.w (a2)+, a5             ;3A5A
          movea.w (a1)+, a6             ;3C59
          movea.w (a0)+, a7             ;3E58

          movea.l (a7)+, a0             ;205F
          movea.l (sp)+, a0             ;205F
          movea.l (a6)+, a1             ;225E
          movea.l (a5)+, a2             ;245D
          movea.l (a4)+, a3             ;265C
          movea.l (a3)+, a4             ;285B
          movea.l (a2)+, a5             ;2A5A
          movea.l (a1)+, a6             ;2C59
          movea.l (a0)+, a7             ;2E58

          movea (a7)+, a0               ;305F
          movea (sp)+, a0               ;305F
          movea (a6)+, a1               ;325E
          movea (a5)+, a2               ;345D
          movea (a4)+, a3               ;365C
          movea (a3)+, a4               ;385B
          movea (a2)+, a5               ;3A5A
          movea (a1)+, a6               ;3C59
          movea (a0)+, a7               ;3E58


          movea.w -(a7), a0             ;3067
          movea.w -(sp), a0             ;3067
          movea.w -(a6), a1             ;3266
          movea.w -(a5), a2             ;3465
          movea.w -(a4), a3             ;3664
          movea.w -(a3), a4             ;3863
          movea.w -(a2), a5             ;3A62
          movea.w -(a1), a6             ;3C61
          movea.w -(a0), a7             ;3E60

          movea.l -(a7), a0             ;2067
          movea.l -(sp), a0             ;2067
          movea.l -(a6), a1             ;2266
          movea.l -(a5), a2             ;2465
          movea.l -(a4), a3             ;2664
          movea.l -(a3), a4             ;2863
          movea.l -(a2), a5             ;2A62
          movea.l -(a1), a6             ;2C61
          movea.l -(a0), a7             ;2E60

          movea -(a7), a0               ;3067
          movea -(sp), a0               ;3067
          movea -(a6), a1               ;3266
          movea -(a5), a2               ;3465
          movea -(a4), a3               ;3664
          movea -(a3), a4               ;3863
          movea -(a2), a5               ;3A62
          movea -(a1), a6               ;3C61
          movea -(a0), a7               ;3E60



          movea.w (a0,d7.w), a7          ;3E70 7000
          movea.w (a1,d6.w), a6          ;3C71 6000
          movea.w (a2,d5.w), a5          ;3A72 5000
          movea.w (a3,d4.w), a4          ;3873 4000
          movea.w (a4,d3.w), a3          ;3674 3000
          movea.w (a5,d2.w), a2          ;3475 2000
          movea.w (a6,d1.w), a1          ;3276 1000
          movea.w (a7,d0.w), a0          ;3077 0000

          movea.l (a0,d7.w), a7          ;2E70 7000
          movea.l (a1,d6.w), a6          ;2C71 6000
          movea.l (a2,d5.w), a5          ;2A72 5000
          movea.l (a3,d4.w), a4          ;2873 4000
          movea.l (a4,d3.w), a3          ;2674 3000
          movea.l (a5,d2.w), a2          ;2475 2000
          movea.l (a6,d1.w), a1          ;2276 1000
          movea.l (a7,d0.w), a0          ;2077 0000

          movea (a0,d7.w), a7            ;3E70 7000
          movea (a1,d6.w), a6            ;3C71 6000
          movea (a2,d5.w), a5            ;3A72 5000
          movea (a3,d4.w), a4            ;3873 4000
          movea (a4,d3.w), a3            ;3674 3000
          movea (a5,d2.w), a2            ;3475 2000
          movea (a6,d1.w), a1            ;3276 1000
          movea (a7,d0.w), a0            ;3077 0000

          movea.w (a0,d7.l), a7          ;3E70 7800
          movea.w (a1,d6.l), a6          ;3C71 6800
          movea.w (a2,d5.l), a5          ;3A72 5800
          movea.w (a3,d4.l), a4          ;3873 4800
          movea.w (a4,d3.l), a3          ;3674 3800
          movea.w (a5,d2.l), a2          ;3475 2800
          movea.w (a6,d1.l), a1          ;3276 1800
          movea.w (a7,d0.l), a0          ;3077 0800

          movea.l (a0,d7.l), a7          ;2E70 7800
          movea.l (a1,d6.l), a6          ;2C71 6800
          movea.l (a2,d5.l), a5          ;2A72 5800
          movea.l (a3,d4.l), a4          ;2873 4800
          movea.l (a4,d3.l), a3          ;2674 3800
          movea.l (a5,d2.l), a2          ;2475 2800
          movea.l (a6,d1.l), a1          ;2276 1800
          movea.l (a7,d0.l), a0          ;2077 0800

          movea (a0,d7.l), a7            ;3E70 7800
          movea (a1,d6.l), a6            ;3C71 6800
          movea (a2,d5.l), a5            ;3A72 5800
          movea (a3,d4.l), a4            ;3873 4800
          movea (a4,d3.l), a3            ;3674 3800
          movea (a5,d2.l), a2            ;3475 2800
          movea (a6,d1.l), a1            ;3276 1800
          movea (a7,d0.l), a0            ;3077 0800

          movea.w (a0,d7), a7            ;3E70 7000
          movea.w (a1,d6), a6            ;3C71 6000
          movea.w (a2,d5), a5            ;3A72 5000
          movea.w (a3,d4), a4            ;3873 4000
          movea.w (a4,d3), a3            ;3674 3000
          movea.w (a5,d2), a2            ;3475 2000
          movea.w (a6,d1), a1            ;3276 1000
          movea.w (a7,d0), a0            ;3077 0000

          movea.l (a0,d7), a7            ;2E70 7000
          movea.l (a1,d6), a6            ;2C71 6000
          movea.l (a2,d5), a5            ;2A72 5000
          movea.l (a3,d4), a4            ;2873 4000
          movea.l (a4,d3), a3            ;2674 3000
          movea.l (a5,d2), a2            ;2475 2000
          movea.l (a6,d1), a1            ;2276 1000
          movea.l (a7,d0), a0            ;2077 0000

          movea (a0,d7), a7              ;3E70 7000
          movea (a1,d6), a6              ;3C71 6000
          movea (a2,d5), a5              ;3A72 5000
          movea (a3,d4), a4              ;3873 4000
          movea (a4,d3), a3              ;3674 3000
          movea (a5,d2), a2              ;3475 2000
          movea (a6,d1), a1              ;3276 1000
          movea (a7,d0), a0              ;3077 0000

          movea.l $01(a0,d7.w), a7       ;2E70 7001
          movea.l $7F(a1,d6.w), a6       ;2C71 607F
          movea.l LATE_01(a2,d5.w), a5   ;2A72 5001
          movea.l LATE_7F(a3,d4.w), a4   ;2873 407F
          movea.l $01(a4,d3.w), a3       ;2674 3001
          movea.l $7F(a5,d2.w), a2       ;2475 207F
          movea.l LATE_01(a6,d1.w), a1   ;2276 1001
          movea.l LATE_7F(a7,d0.w), a0   ;2077 007F

          movea.l $01(a0,d7.l), a7       ;2E70 7801
          movea.l $7F(a1,d6.l), a6       ;2C71 687F
          movea.l LATE_01(a2,d5.l), a5   ;2A72 5801
          movea.l LATE_7F(a3,d4.l), a4   ;2873 487F
          movea.l $01(a4,d3.l), a3       ;2674 3801
          movea.l $7F(a5,d2.l), a2       ;2475 287F
          movea.l LATE_01(a6,d1.l), a1   ;2276 1801
          movea.l LATE_7F(a7,d0.l), a0   ;2077 087F

          movea.l $01(a0,d7), a7         ;2E70 7001
          movea.l $7F(a1,d6), a6         ;2C71 607F
          movea.l LATE_01(a2,d5), a5     ;2A72 5001
          movea.l LATE_7F(a3,d4), a4     ;2873 407F
          movea.l $01(a4,d3), a3         ;2674 3001
          movea.l $7F(a5,d2), a2         ;2475 207F
          movea.l LATE_01(a6,d1), a1     ;2276 1001
          movea.l LATE_7F(a7,d0), a0     ;2077 007F

          movea.w $01(a0,d7.w), a7       ;3E70 7001
          movea.w $7F(a1,d6.w), a6       ;3C71 607F
          movea.w LATE_01(a2,d5.w), a5   ;3A72 5001
          movea.w LATE_7F(a3,d4.w), a4   ;3873 407F
          movea.w $01(a4,d3.w), a3       ;3674 3001
          movea.w $7F(a5,d2.w), a2       ;3475 207F
          movea.w LATE_01(a6,d1.w), a1   ;3276 1001
          movea.w LATE_7F(a7,d0.w), a0   ;3077 007F

          movea.w $01(a0,d7.l), a7       ;3E70 7801
          movea.w $7F(a1,d6.l), a6       ;3C71 687F
          movea.w LATE_01(a2,d5.l), a5   ;3A72 5801
          movea.w LATE_7F(a3,d4.l), a4   ;3873 487F
          movea.w $01(a4,d3.l), a3       ;3674 3801
          movea.w $7F(a5,d2.l), a2       ;3475 287F
          movea.w LATE_01(a6,d1.l), a1   ;3276 1801
          movea.w LATE_7F(a7,d0.l), a0   ;3077 087F

          movea.w $01(a0,d7), a7         ;3E70 7001
          movea.w $7F(a1,d6), a6         ;3C71 607F
          movea.w LATE_01(a2,d5), a5     ;3A72 5001
          movea.w LATE_7F(a3,d4), a4     ;3873 407F
          movea.w $01(a4,d3), a3         ;3674 3001
          movea.w $7F(a5,d2), a2         ;3475 207F
          movea.w LATE_01(a6,d1), a1     ;3276 1001
          movea.w LATE_7F(a7,d0), a0     ;3077 007F

          movea $01(a0,d7.w), a7         ;3E70 7001
          movea $7F(a1,d6.w), a6         ;3C71 607F
          movea LATE_01(a2,d5.w), a5     ;3A72 5001
          movea LATE_7F(a3,d4.w), a4     ;3873 407F
          movea $01(a4,d3.w), a3         ;3674 3001
          movea $7F(a5,d2.w), a2         ;3475 207F
          movea LATE_01(a6,d1.w), a1     ;3276 1001
          movea LATE_7F(a7,d0.w), a0     ;3077 007F

          movea $01(a0,d7.l), a7         ;3E70 7801
          movea $7F(a1,d6.l), a6         ;3C71 687F
          movea LATE_01(a2,d5.l), a5     ;3A72 5801
          movea LATE_7F(a3,d4.l), a4     ;3873 487F
          movea $01(a4,d3.l), a3         ;3674 3801
          movea $7F(a5,d2.l), a2         ;3475 287F
          movea LATE_01(a6,d1.l), a1     ;3276 1801
          movea LATE_7F(a7,d0.l), a0     ;3077 087F

          movea $01(a0,d7), a7           ;3E70 7001
          movea $7F(a1,d6), a6           ;3C71 607F
          movea LATE_01(a2,d5), a5       ;3A72 5001
          movea LATE_7F(a3,d4), a4       ;3873 407F
          movea $01(a4,d3), a3           ;3674 3001
          movea $7F(a5,d2), a2           ;3475 207F
          movea LATE_01(a6,d1), a1       ;3276 1001
          movea LATE_7F(a7,d0), a0       ;3077 007F


          movep.w $0001(a0),d7           ;0F08 0001
          movep.w $00FF(a1),d6           ;0D09 00FF
          movep.w $0100(a2),d5           ;0B0A 0100
          movep.w $7F00(a3),d4           ;090B 7F00
          movep.w LATE_0001(a4),d3       ;070C 0001
          movep.w LATE_00FF(a5),d2       ;050D 00FF
          movep.w LATE_0100(a6),d1       ;030E 0100
          movep.w LATE_7F00(a7),d0       ;010F 7F00

          movep.l $0001(a0),d7           ;0F48 0001
          movep.l $00FF(a1),d6           ;0D49 00FF
          movep.l $0100(a2),d5           ;0B4A 0100
          movep.l $7F00(a3),d4           ;094B 7F00
          movep.l LATE_0001(a4),d3       ;074C 0001
          movep.l LATE_00FF(a5),d2       ;054D 00FF
          movep.l LATE_0100(a6),d1       ;034E 0100
          movep.l LATE_7F00(a7),d0       ;014F 7F00

          movep $0001(a0),d7             ;0F08 0001
          movep $00FF(a1),d6             ;0D09 00FF
          movep $0100(a2),d5             ;0B0A 0100
          movep $7F00(a3),d4             ;090B 7F00
          movep LATE_0001(a4),d3         ;070C 0001
          movep LATE_00FF(a5),d2         ;050D 00FF
          movep LATE_0100(a6),d1         ;030E 0100
          movep LATE_7F00(a7),d0         ;010F 7F00



          movep.w d7,$0001(a0)           ;0F88 0001
          movep.w d6,$00FF(a1)           ;0D89 00FF
          movep.w d5,$0100(a2)           ;0B8A 0100
          movep.w d4,$7F00(a3)           ;098B 7F00
          movep.w d3,LATE_0001(a4)       ;078C 0001
          movep.w d2,LATE_00FF(a5)       ;058D 00FF
          movep.w d1,LATE_0100(a6)       ;038E 0100
          movep.w d0,LATE_7F00(a7)       ;018F 7F00

          movep.l d7,$0001(a0)           ;0FC8 0001
          movep.l d6,$00FF(a1)           ;0DC9 00FF
          movep.l d5,$0100(a2)           ;0BCA 0100
          movep.l d4,$7F00(a3)           ;09CB 7F00
          movep.l d3,LATE_0001(a4)       ;07CC 0001
          movep.l d2,LATE_00FF(a5)       ;05CD 00FF
          movep.l d1,LATE_0100(a6)       ;03CE 0100
          movep.l d0,LATE_7F00(a7)       ;01CF 7F00

          movep d7,$0001(a0)             ;0F88 0001
          movep d6,$00FF(a1)             ;0D89 00FF
          movep d5,$0100(a2)             ;0B8A 0100
          movep d4,$7F00(a3)             ;098B 7F00
          movep d3,LATE_0001(a4)         ;078C 0001
          movep d2,LATE_00FF(a5)         ;058D 00FF
          movep d1,LATE_0100(a6)         ;038E 0100
          movep d0,LATE_7F00(a7)         ;018F 7F00


          moveq #$01, d7        ;7E 01
          moveq #$02, d6        ;7C 02
          moveq #$FE, d5        ;7A FE
          moveq #$FF, d4        ;78 FF
          moveq #LATE_01, d3    ;76 01
          moveq #LATE_02, d2    ;74 02
          moveq #LATE_FE, d1    ;72 FE
          moveq #LATE_FF, d0    ;70 FF


          muls.w #$01, d7       ;CFFC 0001
          muls.w #$02, d6       ;CDFC 0002
          muls.w #$FE, d5       ;CBFC 00FE
          muls.w #$FF, d4       ;C9FC 00FF
          muls.w #LATE_01, d3   ;C7FC 0001
          muls.w #LATE_02, d2   ;C5FC 0002
          muls.w #LATE_FE, d1   ;C3FC 00FE
          muls.w #LATE_FF, d0   ;C1FC 00FF

          muls #$01, d7         ;CFFC 0001
          muls #$02, d6         ;CDFC 0002
          muls #$FE, d5         ;CBFC 00FE
          muls #$FF, d4         ;C9FC 00FF
          muls #LATE_01, d3     ;C7FC 0001
          muls #LATE_02, d2     ;C5FC 0002
          muls #LATE_FE, d1     ;C3FC 00FE
          muls #LATE_FF, d0     ;C1FC 00FF


          muls.w ($0001).w, d7       ;CFF8 0001
          muls.w ($00FF).w, d6       ;CDF8 00FF
          muls.w ($0100).w, d5       ;CBF8 0100
          muls.w ($FF00).w, d4       ;C9F8 FF00
          muls.w (LATE_0001).w, d3   ;C7F8 0001
          muls.w (LATE_00FF).w, d2   ;C5F8 00FF
          muls.w (LATE_0100).w, d1   ;C3F8 0100
          muls.w (LATE_FF00).w, d0   ;C1F8 FF00

          muls.w ($00000001).l, d7       ;CFF9 00000001
          muls.w ($0000FF00).l, d6       ;CDF9 0000FF00
          muls.w ($00010000).l, d5       ;CBF9 00010000
          muls.w ($FF000000).l, d4       ;C9F9 FF000000
          muls.w (LATE_00000001).l, d3   ;C7F9 00000001
          muls.w (LATE_0000FF00).l, d2   ;C5F9 0000FF00
          muls.w (LATE_00010000).l, d1   ;C3F9 00010000
          muls.w (LATE_FF000000).l, d0   ;C1F9 FF000000

          muls.w ($0001), d7         ;CFF8 0001      <downgrade
          muls.w ($00FF), d6         ;CDF8 00FF      <downgrade
          muls.w ($0100), d5         ;CBF8 0100      <downgrade
          muls.w ($FF00), d4         ;C9F9 0000FF00
          muls.w (LATE_0001), d3     ;C7F9 00000001
          muls.w (LATE_00FF), d2     ;C5F9 000000FF
          muls.w (LATE_0100), d1     ;C3F9 00000100
          muls.w (LATE_FF00), d0     ;C1F9 0000FF00

          muls ($0001).w, d7         ;CFF8 0001
          muls ($00FF).w, d6         ;CDF8 00FF
          muls ($0100).w, d5         ;CBF8 0100
          muls ($FF00).w, d4         ;C9F8 FF00
          muls (LATE_0001).w, d3     ;C7F8 0001
          muls (LATE_00FF).w, d2     ;C5F8 00FF
          muls (LATE_0100).w, d1     ;C3F8 0100
          muls (LATE_FF00).w, d0     ;C1F8 FF00

          muls ($00000001).l, d7       ;CFF9 00000001
          muls ($0000FF00).l, d6       ;CDF9 0000FF00
          muls ($00010000).l, d5       ;CBF9 00010000
          muls ($FF000000).l, d4       ;C9F9 FF000000
          muls (LATE_00000001).l, d3   ;C7F9 00000001
          muls (LATE_0000FF00).l, d2   ;C5F9 0000FF00
          muls (LATE_00010000).l, d1   ;C3F9 00010000
          muls (LATE_FF000000).l, d0   ;C1F9 FF000000

          muls ($0001), d7           ;CFF8 0001      <downgrade
          muls ($00FF), d6           ;CDF8 00FF      <downgrade
          muls ($0100), d5           ;CBF8 0100      <downgrade
          muls ($FF00), d4           ;C9F9 0000FF00
          muls (LATE_0001), d3       ;C7F9 00000001
          muls (LATE_00FF), d2       ;C5F9 000000FF
          muls (LATE_0100), d1       ;C3F9 00000100
          muls (LATE_FF00), d0       ;C1F9 0000FF00


          muls.w d0,d7               ;CFC0
          muls.w d1,d6               ;CDC1
          muls.w d2,d5               ;CBC2
          muls.w d3,d4               ;C9C3
          muls.w d4,d3               ;C7C4
          muls.w d5,d2               ;C5C5
          muls.w d6,d1               ;C3C6
          muls.w d7,d0               ;C1C7

          muls d0,d7                 ;CFC0
          muls d1,d6                 ;CDC1
          muls d2,d5                 ;CBC2
          muls d3,d4                 ;C9C3
          muls d4,d3                 ;C7C4
          muls d5,d2                 ;C5C5
          muls d6,d1                 ;C3C6
          muls d7,d0                 ;C1C7


          muls.w (a7), d0            ;C1D7
          muls.w (a6), d1            ;C3D6
          muls.w (a5), d2            ;C5D5
          muls.w (a4), d3            ;C7D4
          muls.w (a3), d4            ;C9D3
          muls.w (a2), d5            ;CBD2
          muls.w (a1), d6            ;CDD1
          muls.w (a0), d7            ;CFD0

          muls (a7), d0              ;C1D7
          muls (a6), d1              ;C3D6
          muls (a5), d2              ;C5D5
          muls (a4), d3              ;C7D4
          muls (a3), d4              ;C9D3
          muls (a2), d5              ;CBD2
          muls (a1), d6              ;CDD1
          muls (a0), d7              ;CFD0


          muls.w $01(a7), d0         ;C1EF 0001
          muls.w $02(a6), d1         ;C3EE 0002
          muls.w $FE(a5), d2         ;C5ED 00FE
          muls.w $FF(a4), d3         ;C7EC 00FF
          muls.w LATE_01(a3), d4     ;C9EB 0001
          muls.w LATE_02(a2), d5     ;CBEA 0002
          muls.w LATE_FE(a1), d6     ;CDE9 00FE
          muls.w LATE_FF(a0), d7     ;CFE8 00FF

          muls $01(a7), d0           ;C1EF 0001
          muls $02(a6), d1           ;C3EE 0002
          muls $FE(a5), d2           ;C5ED 00FE
          muls $FF(a4), d3           ;C7EC 00FF
          muls LATE_01(a3), d4       ;C9EB 0001
          muls LATE_02(a2), d5       ;CBEA 0002
          muls LATE_FE(a1), d6       ;CDE9 00FE
          muls LATE_FF(a0), d7       ;CFE8 00FF


          muls.w (a7)+, d0           ;C1DF
          muls.w (a6)+, d1           ;C3DE
          muls.w (a5)+, d2           ;C5DD
          muls.w (a4)+, d3           ;C7DC
          muls.w (a3)+, d4           ;C9DB
          muls.w (a2)+, d5           ;CBDA
          muls.w (a1)+, d6           ;CDD9
          muls.w (a0)+, d7           ;CFD8

          muls (a7)+, d0             ;C1DF
          muls (a6)+, d1             ;C3DE
          muls (a5)+, d2             ;C5DD
          muls (a4)+, d3             ;C7DC
          muls (a3)+, d4             ;C9DB
          muls (a2)+, d5             ;CBDA
          muls (a1)+, d6             ;CDD9
          muls (a0)+, d7             ;CFD8


          muls.w -(a7), d0           ;C1E7
          muls.w -(a6), d1           ;C3E6
          muls.w -(a5), d2           ;C5E5
          muls.w -(a4), d3           ;C7E4
          muls.w -(a3), d4           ;C9E3
          muls.w -(a2), d5           ;CBE2
          muls.w -(a1), d6           ;CDE1
          muls.w -(a0), d7           ;CFE0

          muls -(a7), d0             ;C1E7
          muls -(a6), d1             ;C3E6
          muls -(a5), d2             ;C5E5
          muls -(a4), d3             ;C7E4
          muls -(a3), d4             ;C9E3
          muls -(a2), d5             ;CBE2
          muls -(a1), d6             ;CDE1
          muls -(a0), d7             ;CFE0


          mulu.w #$01, d7            ;CEFC 0001
          mulu.w #$02, d6            ;CCFC 0002
          mulu.w #$FE, d5            ;CAFC 00FE
          mulu.w #$FF, d4            ;C8FC 00FF
          mulu.w #LATE_01, d3        ;C6FC 0001
          mulu.w #LATE_02, d2        ;C4FC 0002
          mulu.w #LATE_FE, d1        ;C2FC 00FE
          mulu.w #LATE_FF, d0        ;C0FC 00FF

          mulu #$01, d7              ;CEFC 0001
          mulu #$02, d6              ;CCFC 0002
          mulu #$FE, d5              ;CAFC 00FE
          mulu #$FF, d4              ;C8FC 00FF
          mulu #LATE_01, d3          ;C6FC 0001
          mulu #LATE_02, d2          ;C4FC 0002
          mulu #LATE_FE, d1          ;C2FC 00FE
          mulu #LATE_FF, d0          ;C0FC 00FF


          mulu.w ($0001).w, d7       ;CEF8 0001
          mulu.w ($00FF).w, d6       ;CCF8 00FF
          mulu.w ($0100).w, d5       ;CAF8 0100
          mulu.w ($FF00).w, d4       ;C8F8 FF00
          mulu.w (LATE_0001).w, d3   ;C6F8 0001
          mulu.w (LATE_00FF).w, d2   ;C4F8 00FF
          mulu.w (LATE_0100).w, d1   ;C2F8 0100
          mulu.w (LATE_FF00).w, d0   ;C0F8 FF00

          mulu.w ($0001).l, d7       ;CEF9 00000001
          mulu.w ($00FF).l, d6       ;CCF9 000000FF
          mulu.w ($0100).l, d5       ;CAF9 00000100
          mulu.w ($FF00).l, d4       ;C8F9 0000FF00
          mulu.w (LATE_0001).l, d3   ;C6F9 00000001
          mulu.w (LATE_00FF).l, d2   ;C4F9 000000FF
          mulu.w (LATE_0100).l, d1   ;C2F9 00000100
          mulu.w (LATE_FF00).l, d0   ;C0F9 0000FF00

          mulu.w ($0001), d7         ;CEF8 0001   <downgrade
          mulu.w ($00FF), d6         ;CCF8 00FF   <downgrade
          mulu.w ($0100), d5         ;CAF8 0100   <downgrade
          mulu.w ($FF00), d4         ;C8F9 0000FF00
          mulu.w (LATE_0001), d3     ;C6F9 00000001
          mulu.w (LATE_00FF), d2     ;C4F9 000000FF
          mulu.w (LATE_0100), d1     ;C2F9 00000100
          mulu.w (LATE_FF00), d0     ;C0F9 0000FF00

          mulu ($0001).w, d7         ;CEF8 0001
          mulu ($00FF).w, d6         ;CCF8 00FF
          mulu ($0100).w, d5         ;CAF8 0100
          mulu ($FF00).w, d4         ;C8F8 FF00
          mulu (LATE_0001).w, d3     ;C6F8 0001
          mulu (LATE_00FF).w, d2     ;C4F8 00FF
          mulu (LATE_0100).w, d1     ;C2F8 0100
          mulu (LATE_FF00).w, d0     ;C0F8 FF00

          mulu ($0001).l, d7         ;CEF9 00000001
          mulu ($00FF).l, d6         ;CCF9 000000FF
          mulu ($0100).l, d5         ;CAF9 00000100
          mulu ($FF00).l, d4         ;C8F9 0000FF00
          mulu (LATE_0001).l, d3     ;C6F9 00000001
          mulu (LATE_00FF).l, d2     ;C4F9 000000FF
          mulu (LATE_0100).l, d1     ;C2F9 00000100
          mulu (LATE_FF00).l, d0     ;C0F9 0000FF00

          mulu ($0001), d7           ;CEF8 0001   <downgrade
          mulu ($00FF), d6           ;CCF8 00FF   <downgrade
          mulu ($0100), d5           ;CAF8 0100   <downgrade
          mulu ($FF00), d4           ;C8F9 0000FF00
          mulu (LATE_0001), d3       ;C6F9 00000001
          mulu (LATE_00FF), d2       ;C4F9 000000FF
          mulu (LATE_0100), d1       ;C2F9 00000100
          mulu (LATE_FF00), d0       ;C0F9 0000FF00


          mulu.w d0,d7               ;CEC0
          mulu.w d1,d6               ;CCC1
          mulu.w d2,d5               ;CAC2
          mulu.w d3,d4               ;C8C3
          mulu.w d4,d3               ;C6C4
          mulu.w d5,d2               ;C4C5
          mulu.w d6,d1               ;C2C6
          mulu.w d7,d0               ;C0C7

          mulu d0,d7                 ;CEC0
          mulu d1,d6                 ;CCC1
          mulu d2,d5                 ;CAC2
          mulu d3,d4                 ;C8C3
          mulu d4,d3                 ;C6C4
          mulu d5,d2                 ;C4C5
          mulu d6,d1                 ;C2C6
          mulu d7,d0                 ;C0C7


          mulu.w (a7), d0            ;C0D7
          mulu.w (a6), d1            ;C2D6
          mulu.w (a5), d2            ;C4D5
          mulu.w (a4), d3            ;C6D4
          mulu.w (a3), d4            ;C8D3
          mulu.w (a2), d5            ;CAD2
          mulu.w (a1), d6            ;CCD1
          mulu.w (a0), d7            ;CED0

          mulu (a7), d0              ;C0D7
          mulu (a6), d1              ;C2D6
          mulu (a5), d2              ;C4D5
          mulu (a4), d3              ;C6D4
          mulu (a3), d4              ;C8D3
          mulu (a2), d5              ;CAD2
          mulu (a1), d6              ;CCD1
          mulu (a0), d7              ;CED0


          mulu.w $01(a7), d0         ;C0 EF 00 01
          mulu.w $02(a6), d1         ;C2 EE 00 02
          mulu.w $FE(a5), d2         ;C4 ED 00 FE
          mulu.w $FF(a4), d3         ;C6 EC 00 FF
          mulu.w LATE_01(a3), d4     ;C8 EB 00 01
          mulu.w LATE_02(a2), d5     ;CA EA 00 02
          mulu.w LATE_FE(a1), d6     ;CC E9 00 FE
          mulu.w LATE_FF(a0), d7     ;CE E8 00 FF

          mulu $01(a7), d0           ;C0 EF 00 01
          mulu $02(a6), d1           ;C2 EE 00 02
          mulu $FE(a5), d2           ;C4 ED 00 FE
          mulu $FF(a4), d3           ;C6 EC 00 FF
          mulu LATE_01(a3), d4       ;C8 EB 00 01
          mulu LATE_02(a2), d5       ;CA EA 00 02
          mulu LATE_FE(a1), d6       ;CC E9 00 FE
          mulu LATE_FF(a0), d7       ;CE E8 00 FF


          mulu.w (a7)+, d0           ;C0 DF
          mulu.w (a6)+, d1           ;C2 DE
          mulu.w (a5)+, d2           ;C4 DD
          mulu.w (a4)+, d3           ;C6 DC
          mulu.w (a3)+, d4           ;C8 DB
          mulu.w (a2)+, d5           ;CA DA
          mulu.w (a1)+, d6           ;CC D9
          mulu.w (a0)+, d7           ;CE D8

          mulu (a7)+, d0             ;C0 DF
          mulu (a6)+, d1             ;C2 DE
          mulu (a5)+, d2             ;C4 DD
          mulu (a4)+, d3             ;C6 DC
          mulu (a3)+, d4             ;C8 DB
          mulu (a2)+, d5             ;CA DA
          mulu (a1)+, d6             ;CC D9
          mulu (a0)+, d7             ;CE D8


          mulu.w -(a7), d0           ;C0 E7
          mulu.w -(a6), d1           ;C2 E6
          mulu.w -(a5), d2           ;C4 E5
          mulu.w -(a4), d3           ;C6 E4
          mulu.w -(a3), d4           ;C8 E3
          mulu.w -(a2), d5           ;CA E2
          mulu.w -(a1), d6           ;CC E1
          mulu.w -(a0), d7           ;CE E0

          mulu -(a7), d0             ;C0 E7
          mulu -(a6), d1             ;C2 E6
          mulu -(a5), d2             ;C4 E5
          mulu -(a4), d3             ;C6 E4
          mulu -(a3), d4             ;C8 E3
          mulu -(a2), d5             ;CA E2
          mulu -(a1), d6             ;CC E1
          mulu -(a0), d7             ;CE E0


          neg.b d0              ;4400
          neg.b d1              ;4401
          neg.w d2              ;4442
          neg.w d3              ;4443
          neg.l d4              ;4484
          neg.l d5              ;4485
          neg d6                ;4446
          neg d7                ;4447

          neg.b (a0)            ;4410
          neg.b (a1)            ;4411
          neg.w (a2)            ;4452
          neg.w (a3)            ;4453
          neg.l (a4)            ;4494
          neg.l (a5)            ;4495
          neg (a6)              ;4456
          neg (a7)              ;4457

          neg.b $01(a0)         ;4428 0001
          neg.b $02(a1)         ;4429 0002
          neg.w $FE(a2)         ;446A 00FE
          neg.w $FF(a3)         ;446B 00FF
          neg.l LATE_01(a4)     ;44AC 0001
          neg.l LATE_02(a5)     ;44AD 0002
          neg LATE_FE(a6)       ;446E 00FE
          neg LATE_FF(a7)       ;446F 00FF

          neg.b (a0)+           ;4418
          neg.b (a1)+           ;4419
          neg.w (a2)+           ;445A
          neg.w (a3)+           ;445B
          neg.l (a4)+           ;449C
          neg.l (a5)+           ;449D
          neg (a6)+             ;445E
          neg (a7)+             ;445F

          neg.b -(a0)           ;4420
          neg.b -(a1)           ;4421
          neg.w -(a2)           ;4462
          neg.w -(a3)           ;4463
          neg.l -(a4)           ;44A4
          neg.l -(a5)           ;44A5
          neg -(a6)             ;4466
          neg -(a7)             ;4467

          nop                   ;4E 71

          not.w ($0001).w       ;4678 0001
          not.w ($00FF).w       ;4678 00FF
          not.w ($0100).w       ;4678 0100
          not.w ($FF00).w       ;4678 FF00
          not.w (LATE_0001).w   ;4678 0001
          not.w (LATE_00FF).w   ;4678 00FF
          not.w (LATE_0100).w   ;4678 0100
          not.w (LATE_FF00).w   ;4678 FF00

          not.w ($0001).l       ;4679 00000001
          not.w ($00FF).l       ;4679 000000FF
          not.w ($0100).l       ;4679 00000100
          not.w ($FF00).l       ;4679 0000FF00
          not.w (LATE_0001).l   ;4679 00000001
          not.w (LATE_00FF).l   ;4679 000000FF
          not.w (LATE_0100).l   ;4679 00000100
          not.w (LATE_FF00).l   ;4679 0000FF00

          not.w ($0001)         ;4678 0001    <downgrade
          not.w ($00FF)         ;4678 00FF    <downgrade
          not.w ($0100)         ;4678 0100    <downgrade
          not.w ($FF00)         ;4679 0000FF00
          not.w (LATE_0001)     ;4679 00000001
          not.w (LATE_00FF)     ;4679 000000FF
          not.w (LATE_0100)     ;4679 00000100
          not.w (LATE_FF00)     ;4679 0000FF00

          not.l ($0001).w       ;46B8 0001
          not.l ($00FF).w       ;46B8 00FF
          not.l ($0100).w       ;46B8 0100
          not.l ($FF00).w       ;46B8 FF00
          not.l (LATE_0001).w   ;46B8 0001
          not.l (LATE_00FF).w   ;46B8 00FF
          not.l (LATE_0100).w   ;46B8 0100
          not.l (LATE_FF00).w   ;46B8 FF00

          not.l ($0001).l       ;46B9 00000001
          not.l ($00FF).l       ;46B9 000000FF
          not.l ($0100).l       ;46B9 00000100
          not.l ($FF00).l       ;46B9 0000FF00
          not.l (LATE_0001).l   ;46B9 00000001
          not.l (LATE_00FF).l   ;46B9 000000FF
          not.l (LATE_0100).l   ;46B9 00000100
          not.l (LATE_FF00).l   ;46B9 0000FF00

          not.l ($0001)         ;46B8 0001    <downgrade
          not.l ($00FF)         ;46B8 00FF    <downgrade
          not.l ($0100)         ;46B8 0100    <downgrade
          not.l ($FF00)         ;46B9 0000FF00
          not.l (LATE_0001)     ;46B9 00000001
          not.l (LATE_00FF)     ;46B9 000000FF
          not.l (LATE_0100)     ;46B9 00000100
          not.l (LATE_FF00)     ;46B9 0000FF00

          not ($0001).w         ;4678 0001
          not ($00FF).w         ;4678 00FF
          not ($0100).w         ;4678 0100
          not ($FF00).w         ;4678 FF00
          not (LATE_0001).w     ;4678 0001
          not (LATE_00FF).w     ;4678 00FF
          not (LATE_0100).w     ;4678 0100
          not (LATE_FF00).w     ;4678 FF00

          not ($0001).l         ;4679 00000001
          not ($00FF).l         ;4679 000000FF
          not ($0100).l         ;4679 00000100
          not ($FF00).l         ;4679 0000FF00
          not (LATE_0001).l     ;4679 00000001
          not (LATE_00FF).l     ;4679 000000FF
          not (LATE_0100).l     ;4679 00000100
          not (LATE_FF00).l     ;4679 0000FF00

          not ($0001)           ;4678 0001    <downgrade
          not ($00FF)           ;4678 00FF    <downgrade
          not ($0100)           ;4678 0100    <downgrade
          not ($FF00)           ;4679 0000FF00
          not (LATE_0001)       ;4679 00000001
          not (LATE_00FF)       ;4679 000000FF
          not (LATE_0100)       ;4679 00000100
          not (LATE_FF00)       ;4679 0000FF00



          not.b d0              ;4600
          not.b d1              ;4601
          not.w d2              ;4642
          not.w d3              ;4643
          not.l d4              ;4684
          not.l d5              ;4685
          not d6                ;4646
          not d7                ;4647


          not.b (a0)            ;4610
          not.b (a1)            ;4611
          not.w (a2)            ;4652
          not.w (a3)            ;4653
          not.l (a4)            ;4694
          not.l (a5)            ;4695
          not (a6)              ;4656
          not (a7)              ;4657


          not.b $01(a0)         ;4628 0001
          not.w $02(a1)         ;4669 0002
          not.l $FE(a2)         ;46AA 00FE
          not   $FF(a3)         ;466B 00FF
          not.b LATE_01(a4)     ;462C 0001
          not.w LATE_02(a5)     ;466D 0002
          not.l LATE_FE(a6)     ;46AE 00FE
          not   LATE_FF(a7)     ;466F 00FF


          not.b (a0)+           ;4618
          not.b (a1)+           ;4619
          not.w (a2)+           ;465A
          not.w (a3)+           ;465B
          not.l (a4)+           ;469C
          not.l (a5)+           ;469D
          not   (a6)+           ;465E
          not   (a7)+           ;465F

          not.b -(a0)           ;4620
          not.b -(a1)           ;4621
          not.w -(a2)           ;4662
          not.w -(a3)           ;4663
          not.l -(a4)           ;46A4
          not.l -(a5)           ;46A5
          not   -(a6)           ;4666
          not   -(a7)           ;4667

          ori.b #$01, d7             ;0007 0001
          ori.b #$02, d6             ;0006 0002
          ori.b #$FE, d5             ;0005 00FE
          ori.b #$FF, d4             ;0004 00FF
          ori.b #LATE_01, d3         ;0003 0001
          ori.b #LATE_02, d2         ;0002 0002
          ori.b #LATE_FE, d1         ;0001 00FE
          ori.b #LATE_FF, d0         ;0000 00FF

          ori.w #$0001, d7           ;0047 0001
          ori.w #$00FF, d6           ;0046 00FF
          ori.w #$0100, d5           ;0045 0100
          ori.w #$FF00, d4           ;0044 FF00
          ori.w #LATE_0001, d3       ;0043 0001
          ori.w #LATE_00FF, d2       ;0042 00FF
          ori.w #LATE_0100, d1       ;0041 0100
          ori.w #LATE_FF00, d0       ;0040 FF00

          ori.l #$00000001, d7       ;0087 00000001
          ori.l #$0000FF00, d6       ;0086 0000FF00
          ori.l #$00010000, d5       ;0085 00010000
          ori.l #$00FF0000, d4       ;0084 00FF0000
          ori.l #LATE_00000001, d3   ;0083 00000001
          ori.l #LATE_0000FF00, d2   ;0082 0000FF00
          ori.l #LATE_00010000, d1   ;0081 00010000
          ori.l #LATE_00FF0000, d0   ;0080 00FF0000

          ori   #$0001, d7           ;0047 0001
          ori   #$00FF, d6           ;0046 00FF
          ori   #$0100, d5           ;0045 0100
          ori   #$FF00, d4           ;0044 FF00
          ori   #LATE_0001, d3       ;0043 0001
          ori   #LATE_00FF, d2       ;0042 00FF
          ori   #LATE_0100, d1       ;0041 0100
          ori   #LATE_FF00, d0       ;0040 FF00


          ori.b #$01, ($0001).w            ;0038 00010001
          ori.b #$02, ($00FF).w            ;0038 000200FF
          ori.b #$FE, (LATE_0100).w        ;0038 00FE0100
          ori.b #$FF, (LATE_FF00).w        ;0038 00FFFF00
          ori.b #LATE_01, ($0001).w        ;0038 00010001
          ori.b #LATE_02, ($00FF).w        ;0038 000200FF
          ori.b #LATE_FE, (LATE_0100).w    ;0038 00FE0100
          ori.b #LATE_FF, (LATE_FF00).w    ;0038 00FFFF00

          ori.w #$0001, ($0001).w          ;0078 00010001
          ori.w #$00FF, ($00FF).w          ;0078 00FF00FF
          ori.w #$0100, (LATE_0100).w      ;0078 01000100
          ori.w #$FF00, (LATE_FF00).w      ;0078 FF00FF00
          ori.w #LATE_0001, ($0001).w      ;0078 00010001
          ori.w #LATE_00FF, ($00FF).w      ;0078 00FF00FF
          ori.w #LATE_0100, (LATE_0100).w  ;0078 01000100
          ori.w #LATE_FF00, (LATE_FF00).w  ;0078 FF00FF00

          ori.l #$0001, ($0001).w          ;00B8 00000001 0001
          ori.l #$00FF, ($00FF).w          ;00B8 000000FF 00FF
          ori.l #$0100, (LATE_0100).w      ;00B8 00000100 0100
          ori.l #$FF00, (LATE_FF00).w      ;00B8 0000FF00 FF00
          ori.l #LATE_0001, ($0001).w      ;00B8 00000001 0001
          ori.l #LATE_00FF, ($00FF).w      ;00B8 000000FF 00FF
          ori.l #LATE_0100, (LATE_0100).w  ;00B8 00000100 0100
          ori.l #LATE_FF00, (LATE_FF00).w  ;00B8 0000FF00 FF00

          ori   #$0001, ($0001).w          ;0078 00010001
          ori   #$00FF, ($00FF).w          ;0078 00FF00FF
          ori   #$0100, (LATE_0100).w      ;0078 01000100
          ori   #$FF00, (LATE_FF00).w      ;0078 FF00FF00
          ori   #LATE_0001, ($0001).w      ;0078 00010001
          ori   #LATE_00FF, ($00FF).w      ;0078 00FF00FF
          ori   #LATE_0100, (LATE_0100).w  ;0078 01000100
          ori   #LATE_FF00, (LATE_FF00).w  ;0078 FF00FF00

          ori.b #$01, ($0001).l            ;0039 0001 00000001
          ori.b #$02, ($00FF).l            ;0039 0002 000000FF
          ori.b #$FE, (LATE_0100).l        ;0039 00FE 00000100
          ori.b #$FF, (LATE_FF00).l        ;0039 00FF 0000FF00
          ori.b #LATE_01, ($0001).l        ;0039 0001 00000001
          ori.b #LATE_02, ($00FF).l        ;0039 0002 000000FF
          ori.b #LATE_FE, (LATE_0100).l    ;0039 00FE 00000100
          ori.b #LATE_FF, (LATE_FF00).l    ;0039 00FF 0000FF00

          ori.w #$0001, ($0001).l          ;0079 0001 00000001
          ori.w #$00FF, ($00FF).l          ;0079 00FF 000000FF
          ori.w #$0100, (LATE_0100).l      ;0079 0100 00000100
          ori.w #$FF00, (LATE_FF00).l      ;0079 FF00 0000FF00
          ori.w #LATE_0001, ($0001).l      ;0079 0001 00000001
          ori.w #LATE_00FF, ($00FF).l      ;0079 00FF 000000FF
          ori.w #LATE_0100, (LATE_0100).l  ;0079 0100 00000100
          ori.w #LATE_FF00, (LATE_FF00).l  ;0079 FF00 0000FF00

          ori.l #$0001, ($0001).l          ;00B9 00000001 00000001
          ori.l #$00FF, ($00FF).l          ;00B9 000000FF 000000FF
          ori.l #$0100, (LATE_0100).l      ;00B9 00000100 00000100
          ori.l #$FF00, (LATE_FF00).l      ;00B9 0000FF00 0000FF00
          ori.l #LATE_0001, ($0001).l      ;00B9 00000001 00000001
          ori.l #LATE_00FF, ($00FF).l      ;00B9 000000FF 000000FF
          ori.l #LATE_0100, (LATE_0100).l  ;00B9 00000100 00000100
          ori.l #LATE_FF00, (LATE_FF00).l  ;00B9 0000FF00 0000FF00

          ori   #$0001, ($0001).l          ;0079 0001 00000001
          ori   #$00FF, ($00FF).l          ;0079 00FF 000000FF
          ori   #$0100, (LATE_0100).l      ;0079 0100 00000100
          ori   #$FF00, (LATE_FF00).l      ;0079 FF00 0000FF00
          ori   #LATE_0001, ($0001).l      ;0079 0001 00000001
          ori   #LATE_00FF, ($00FF).l      ;0079 00FF 000000FF
          ori   #LATE_0100, (LATE_0100).l  ;0079 0100 00000100
          ori   #LATE_FF00, (LATE_FF00).l  ;0079 FF00 0000FF00

          ori.b #$01, ($0001)              ;0038 00010001
          ori.b #$02, ($00FF)              ;0038 000200FF
          ori.b #$FE, (LATE_0100)          ;0039 00FE 00000100
          ori.b #$FF, (LATE_FF00)          ;0039 00FF 0000FF00
          ori.b #LATE_01, ($0001)          ;0038 00010001
          ori.b #LATE_02, ($00FF)          ;0038 000200FF
          ori.b #LATE_FE, (LATE_0100)      ;0039 00FE 00000100
          ori.b #LATE_FF, (LATE_FF00)      ;0039 00FF 0000FF00

          ori.w #$0001, ($0001)            ;0078 00010001
          ori.w #$00FF, ($00FF)            ;0078 00FF00FF
          ori.w #$0100, (LATE_0100)        ;0079 0100 00000100
          ori.w #$FF00, (LATE_FF00)        ;0079 FF00 0000FF00
          ori.w #LATE_0001, ($0001)        ;0078 00010001
          ori.w #LATE_00FF, ($00FF)        ;0078 00FF00FF
          ori.w #LATE_0100, (LATE_0100)    ;0079 0100 00000100
          ori.w #LATE_FF00, (LATE_FF00)    ;0079 FF00 0000FF00

          ori.l #$0001, ($0001)            ;00B8 00000001 0001
          ori.l #$00FF, ($00FF)            ;00B8 000000FF 00FF
          ori.l #$0100, (LATE_0100)        ;00B9 00000100 0000 0100
          ori.l #$FF00, (LATE_FF00)        ;00B9 0000FF00 0000FF00
          ori.l #LATE_0001, ($0001)        ;00B8 00000001 0001
          ori.l #LATE_00FF, ($00FF)        ;00B8 000000FF 00FF
          ori.l #LATE_0100, (LATE_0100)    ;00B9 00000100 00000100
          ori.l #LATE_FF00, (LATE_FF00)    ;00B9 0000FF00 0000FF00

          ori   #$0001, ($0001)            ;0078 00010001
          ori   #$00FF, ($00FF)            ;0078 00FF00FF
          ori   #$0100, (LATE_0100)        ;0079 0100 00000100
          ori   #$FF00, (LATE_FF00)        ;0079 FF00 0000FF00
          ori   #LATE_0001, ($0001)        ;0078 00010001
          ori   #LATE_00FF, ($00FF)        ;0078 00FF00FF
          ori   #LATE_0100, (LATE_0100)    ;0079 0100 00000100
          ori   #LATE_FF00, (LATE_FF00)    ;0079 FF00 0000FF00



          ori.b #$01, (a7)                 ;0017 0001
          ori.b #$02, (a6)                 ;0016 0002
          ori.b #$FE, (a5)                 ;0015 00FE
          ori.b #$FF, (a4)                 ;0014 00FF
          ori.b #LATE_01, (a3)             ;0013 0001
          ori.b #LATE_02, (a2)             ;0012 0002
          ori.b #LATE_FE, (a1)             ;0011 00FE
          ori.b #LATE_FF, (a0)             ;0010 00FF

          ori.w #$01, (a7)                 ;0057 0001
          ori.w #$02, (a6)                 ;0056 0002
          ori.w #$FE, (a5)                 ;0055 00FE
          ori.w #$FF, (a4)                 ;0054 00FF
          ori.w #LATE_01, (a3)             ;0053 0001
          ori.w #LATE_02, (a2)             ;0052 0002
          ori.w #LATE_FE, (a1)             ;0051 00FE
          ori.w #LATE_FF, (a0)             ;0050 00FF

          ori.l #$01, (a7)                 ;0097 00000001
          ori.l #$02, (a6)                 ;0096 00000002
          ori.l #$FE, (a5)                 ;0095 000000FE
          ori.l #$FF, (a4)                 ;0094 000000FF
          ori.l #LATE_01, (a3)             ;0093 00000001
          ori.l #LATE_02, (a2)             ;0092 00000002
          ori.l #LATE_FE, (a1)             ;0091 000000FE
          ori.l #LATE_FF, (a0)             ;0090 000000FF

          ori   #$01, (a7)                 ;0057 0001
          ori   #$02, (a6)                 ;0056 0002
          ori   #$FE, (a5)                 ;0055 00FE
          ori   #$FF, (a4)                 ;0054 00FF
          ori   #LATE_01, (a3)             ;0053 0001
          ori   #LATE_02, (a2)             ;0052 0002
          ori   #LATE_FE, (a1)             ;0051 00FE
          ori   #LATE_FF, (a0)             ;0050 00FF


          ori.b #$01, $FF(a0)              ;0028 0001 00FF
          ori.b #$02, $FE(a1)              ;0029 00FF 00FE
          ori.b #$FE, LATE_02(a2)          ;002A 0100 0002
          ori.b #$FF, LATE_01(a3)          ;002B FF00 0001
          ori.b #LATE_01, $FF(a4)          ;002C 0001 00FF
          ori.b #LATE_02, $FE(a5)          ;002D 00FF 00FE
          ori.b #LATE_FE, LATE_02(a6)      ;002E 0100 0002
          ori.b #LATE_FF, LATE_01(a7)      ;002F FF00 0001

          ori.w #$0001, $FF(a0)            ;0068 0001 00FF
          ori.w #$00FF, $FE(a1)            ;0069 00FF 00FE
          ori.w #$0100, LATE_02(a2)        ;006A 0100 0002
          ori.w #$FF00, LATE_01(a3)        ;006B FF00 0001
          ori.w #LATE_0001, $FF(a4)        ;006C 0001 00FF
          ori.w #LATE_00FF, $FE(a5)        ;006D 00FF 00FE
          ori.w #LATE_0100, LATE_02(a6)    ;006E 0100 0002
          ori.w #LATE_FF00, LATE_01(a7)    ;006F FF00 0001

          ori.l #$00000001, $FF(a0)            ;00A8 00000001 00FF
          ori.l #$0000FF00, $FE(a1)            ;00A9 0000FF00 00FE
          ori.l #$00010000, LATE_02(a2)        ;00AA 00010000 0002
          ori.l #$FF000000, LATE_01(a3)        ;00AB FF000000 0001
          ori.l #LATE_00000001, $FF(a4)        ;00AC 00000001 00FF
          ori.l #LATE_0000FF00, $FE(a5)        ;00AD 0000FF00 00FE
          ori.l #LATE_00010000, LATE_02(a6)    ;00AE 00010000 0002
          ori.l #LATE_FF000000, LATE_01(a7)    ;00AF FF000000 0001

          ori   #$0001, $FF(a0)            ;0068 0001 00FF
          ori   #$00FF, $FE(a1)            ;0069 00FF 00FE
          ori   #$0100, LATE_02(a2)        ;006A 0100 0002
          ori   #$FF00, LATE_01(a3)        ;006B FF00 0001
          ori   #LATE_0001, $FF(a4)        ;006C 0001 00FF
          ori   #LATE_00FF, $FE(a5)        ;006D 00FF 00FE
          ori   #LATE_0100, LATE_02(a6)    ;006E 0100 0002
          ori   #LATE_FF00, LATE_01(a7)    ;006F FF00 0001


          ori.b #$01, (a0)+                ;0018 0001
          ori.b #$02, (a1)+                ;0019 0002
          ori.b #$FE, (a2)+                ;001A 00FE
          ori.b #$FF, (a3)+                ;001B 00FF
          ori.b #LATE_01, (a4)+            ;001C 0001
          ori.b #LATE_02, (a5)+            ;001D 0002
          ori.b #LATE_FE, (a6)+            ;001E 00FE
          ori.b #LATE_FF, (a7)+            ;001F 00FF

          ori.w #$0001, (a0)+              ;0058 0001
          ori.w #$00FF, (a1)+              ;0059 00FF
          ori.w #$0100, (a2)+              ;005A 0100
          ori.w #$FF00, (a3)+              ;005B FF00
          ori.w #LATE_0001, (a4)+          ;005C 0001
          ori.w #LATE_00FF, (a5)+          ;005D 00FF
          ori.w #LATE_0100, (a6)+          ;005E 0100
          ori.w #LATE_FF00, (a7)+          ;005F FF00

          ori.l #$00000001, (a0)+          ;0098 00000001
          ori.l #$0000FF00, (a1)+          ;0099 0000FF00
          ori.l #$00010000, (a2)+          ;009A 00010000
          ori.l #$FF000000, (a3)+          ;009B FF000000
          ori.l #LATE_00000001, (a4)+      ;009C 00000001
          ori.l #LATE_0000FF00, (a5)+      ;009D 0000FF00
          ori.l #LATE_00010000, (a6)+      ;009E 00010000
          ori.l #LATE_FF000000, (a7)+      ;009F FF000000

          ori   #$0001, (a0)+              ;0058 0001
          ori   #$00FF, (a1)+              ;0059 00FF
          ori   #$0100, (a2)+              ;005A 0100
          ori   #$FF00, (a3)+              ;005B FF00
          ori   #LATE_0001, (a4)+          ;005C 0001
          ori   #LATE_00FF, (a5)+          ;005D 00FF
          ori   #LATE_0100, (a6)+          ;005E 0100
          ori   #LATE_FF00, (a7)+          ;005F FF00



          ori.b #$01, -(a0)                    ;0020 0001
          ori.b #$02, -(a1)                    ;0021 0002
          ori.b #$FE, -(a2)                    ;0022 00FE
          ori.b #$FF, -(a3)                    ;0023 00FF
          ori.b #LATE_01, -(a4)                ;0024 0001
          ori.b #LATE_02, -(a5)                ;0025 0002
          ori.b #LATE_FE, -(a6)                ;0026 00FE
          ori.b #LATE_FF, -(a7)                ;0027 00FF

          ori.w #$0001, $FF(a0)                ;0068 0001 00FF
          ori.w #$00FF, $FE(a1)                ;0069 00FF 00FE
          ori.w #$0100, LATE_02(a2)            ;006A 0100 0002
          ori.w #$FF00, LATE_01(a3)            ;006B FF00 0001
          ori.w #LATE_0001, $FF(a4)            ;006C 0001 00FF
          ori.w #LATE_00FF, $FE(a5)            ;006D 00FF 00FE
          ori.w #LATE_0100, LATE_02(a6)        ;006E 0100 0002
          ori.w #LATE_FF00, LATE_01(a7)        ;006F FF00 0001

          ori.l #$00000001, $FF(a0)            ;00A8 00000001 00FF
          ori.l #$0000FF00, $FE(a1)            ;00A9 0000FF00 00FE
          ori.l #$00010000, LATE_02(a2)        ;00AA 00010000 0002
          ori.l #$FF000000, LATE_01(a3)        ;00AB FF000000 0001
          ori.l #LATE_00000001, $FF(a4)        ;00AC 00000001 00FF
          ori.l #LATE_0000FF00, $FE(a5)        ;00AD 0000FF00 00FE
          ori.l #LATE_00010000, LATE_02(a6)    ;00AE 00010000 0002
          ori.l #LATE_FF000000, LATE_01(a7)    ;00AF FF000000 0001

          ori   #$0001, $FF(a0)                ;0068 0001 00FF
          ori   #$00FF, $FE(a1)                ;0069 00FF 00FE
          ori   #$0100, LATE_02(a2)            ;006A 0100 0002
          ori   #$FF00, LATE_01(a3)            ;006B FF00 0001
          ori   #LATE_0001, $FF(a4)            ;006C 0001 00FF
          ori   #LATE_00FF, $FE(a5)            ;006D 00FF 00FE
          ori   #LATE_0100, LATE_02(a6)        ;006E 0100 0002
          ori   #LATE_FF00, LATE_01(a7)        ;006F FF00 0001


          ori.b #$01,(a0,d7.w)                ;0030 0001 7000
          ori.b #$FF,(a1,d6.w)                ;0031 00FF 6000
          ori.b #LATE_01,(a2,d5.w)            ;0032 0001 5000
          ori.b #LATE_FF,(a3,d4.w)            ;0033 00FF 4000
          ori.b #$01,(a4,d3.w)                ;0034 0001 3000
          ori.b #$FF,(a5,d2.w)                ;0035 00FF 2000
          ori.b #LATE_01,(a6,d1.w)            ;0036 0001 1000
          ori.b #LATE_FF,(a7,d0.w)            ;0037 00FF 0000

          ori.w #$0001,(a0,d7.w)              ;0070 0001 7000
          ori.w #$00FF,(a1,d6.w)              ;0071 00FF 6000
          ori.w #LATE_0100,(a2,d5.w)          ;0072 0100 5000
          ori.w #LATE_FF00,(a3,d4.w)          ;0073 FF00 4000
          ori.w #$0001,(a4,d3.w)              ;0074 0001 3000
          ori.w #$FF00,(a5,d2.w)              ;0075 00FF 2000
          ori.w #LATE_0001,(a6,d1.w)          ;0076 0100 1000
          ori.w #LATE_FF00,(a7,d0.w)          ;0077 FF00 0000

          ori.l #$00000001,(a0,d7.w)          ;00B0 00000001 7000
          ori.l #$0000FF00,(a1,d6.w)          ;00B1 000000FF 6000
          ori.l #LATE_00010000,(a2,d5.w)      ;00B2 00000100 5000
          ori.l #LATE_FF000000,(a3,d4.w)      ;00B3 0000FF00 4000
          ori.l #$00000001,(a4,d3.w)          ;00B4 00000001 3000
          ori.l #$0000FF00,(a5,d2.w)          ;00B5 000000FF 2000
          ori.l #LATE_00010000,(a6,d1.w)      ;00B6 00000100 1000
          ori.l #LATE_FF000000,(a7,d0.w)      ;00B7 0000FF00 0000

          ori   #$0001,(a0,d7.w)              ;0070 0001 7000
          ori   #$00FF,(a1,d6.w)              ;0071 00FF 6000
          ori   #LATE_0100,(a2,d5.w)          ;0072 0100 5000
          ori   #LATE_FF00,(a3,d4.w)          ;0073 FF00 4000
          ori   #$0001,(a4,d3.w)              ;0074 0001 3000
          ori   #$FF00,(a5,d2.w)              ;0075 00FF 2000
          ori   #LATE_0001,(a6,d1.w)          ;0076 0100 1000
          ori   #LATE_FF00,(a7,d0.w)          ;0077 FF00 0000


          ori.b #$01, $7F(a0,d7.w)            ;0030 0001 707F
          ori.b #$02, $01(a1,d6.w)            ;0031 0002 6001
          ori.b #$FE, LATE_7F(a2,d5.w)        ;0032 00FE 507F
          ori.b #$FF, LATE_01(a3,d4.w)        ;0033 00FF 4001
          ori.b #LATE_01, $7F(a4,d3.w)        ;0034 0001 307F
          ori.b #LATE_02, $01(a5,d2.w)        ;0035 0002 2001
          ori.b #LATE_FE, LATE_7F(a6,d1.w)    ;0036 00FE 107F
          ori.b #LATE_FF, LATE_01(a7,d0.w)    ;0037 00FF 0001

          ori.w #$0001, $7F(a0,d7.w)          ;0070 0001 707F
          ori.w #$00FF, $01(a1,d6.w)          ;0071 00FF 6001
          ori.w #$0100, LATE_7F(a2,d5.w)      ;0072 0100 507F
          ori.w #$FF00, LATE_01(a3,d4.w)      ;0073 FF00 4001
          ori.w #LATE_0001, $7F(a4,d3.w)      ;0074 0001 307F
          ori.w #LATE_00FF, $01(a5,d2.w)      ;0075 00FF 2001
          ori.w #LATE_0100, LATE_7F(a6,d1.w)  ;0076 0100 107F
          ori.w #LATE_FF00, LATE_01(a7,d0.w)  ;0077 FF00 0001

          ori.l #$00000001, $7F(a0,d7.w)          ;00B0 00000001 707F
          ori.l #$0000FF00, $01(a1,d6.w)          ;00B1 0000FF00 6001
          ori.l #$00010000, LATE_7F(a2,d5.w)      ;00B2 00010000 507F
          ori.l #$FF000000, LATE_01(a3,d4.w)      ;00B3 FF000000 4001
          ori.l #LATE_00000001, $7F(a4,d3.w)      ;00B4 00000001 307F
          ori.l #LATE_0000FF00, $01(a5,d2.w)      ;00B5 0000FF00 2001
          ori.l #LATE_00010000, LATE_7F(a6,d1.w)  ;00B6 00010000 107F
          ori.l #LATE_FF000000, LATE_01(a7,d0.w)  ;00B7 FF000000 0001

          ori   #$0001, $7F(a0,d7.w)          ;0070 0001 707F
          ori   #$00FF, $01(a1,d6.w)          ;0071 00FF 6001
          ori   #$0100, LATE_7F(a2,d5.w)      ;0072 0100 507F
          ori   #$FF00, LATE_01(a3,d4.w)      ;0073 FF00 4001
          ori   #LATE_0001, $7F(a4,d3.w)      ;0074 0001 307F
          ori   #LATE_00FF, $01(a5,d2.w)      ;0075 00FF 2001
          ori   #LATE_0100, LATE_7F(a6,d1.w)  ;0076 0100 107F
          ori   #LATE_FF00, LATE_01(a7,d0.w)  ;0077 FF00 0001


          ori #$0001,sr                       ;007C 0001
          ori #$00FF,sr                       ;007C 00FF
          ori.w #$0100,sr                     ;007C 0100
          ori.w #$FF00,sr                     ;007C FF00
          ori.w #LATE_0001,sr                 ;007C 0001
          ori.w #LATE_00FF,sr                 ;007C 00FF
          ori #LATE_0100,sr                   ;007C 0100
          ori #LATE_FF00,sr                   ;007C FF00

          or.b d0, d7                         ;8E00
          or.b d1, d6                         ;8C01
          or.w d2, d5                         ;8A42
          or.w d3, d4                         ;8843
          or.l d4, d3                         ;8684
          or.l d5, d2                         ;8485
          or   d6, d1                         ;8246
          or   d7, d0                         ;8047


          or.b ($0001).w, d0                  ;8038 0001
          or.b ($00FF).w, d1                  ;8238 00FF
          or.b ($0100).w, d2                  ;8438 0100
          or.b ($FF00).w, d3                  ;8638 FF00
          or.b (LATE_0001).w, d4              ;8838 0001
          or.b (LATE_00FF).w, d5              ;8A38 00FF
          or.b (LATE_0100).w, d6              ;8C38 0100
          or.b (LATE_FF00).w, d7              ;8E38 FF00

          or.w ($0001).w, d0                  ;8078 0001
          or.w ($00FF).w, d1                  ;8278 00FF
          or.w ($0100).w, d2                  ;8478 0100
          or.w ($FF00).w, d3                  ;8678 FF00
          or.w (LATE_0001).w, d4              ;8878 0001
          or.w (LATE_00FF).w, d5              ;8A78 00FF
          or.w (LATE_0100).w, d6              ;8C78 0100
          or.w (LATE_FF00).w, d7              ;8E78 FF00

          or.l ($0001).w, d0                  ;80B8 0001
          or.l ($00FF).w, d1                  ;82B8 00FF
          or.l ($0100).w, d2                  ;84B8 0100
          or.l ($FF00).w, d3                  ;86B8 FF00
          or.l (LATE_0001).w, d4              ;88B8 0001
          or.l (LATE_00FF).w, d5              ;8AB8 00FF
          or.l (LATE_0100).w, d6              ;8CB8 0100
          or.l (LATE_FF00).w, d7              ;8EB8 FF00

          or   ($0001).w, d0                  ;8078 0001
          or   ($00FF).w, d1                  ;8278 00FF
          or   ($0100).w, d2                  ;8478 0100
          or   ($FF00).w, d3                  ;8678 FF00
          or   (LATE_0001).w, d4              ;8878 0001
          or   (LATE_00FF).w, d5              ;8A78 00FF
          or   (LATE_0100).w, d6              ;8C78 0100
          or   (LATE_FF00).w, d7              ;8E78 FF00

          or.b ($0001).l, d0                  ;8039 00000001
          or.b ($00FF).l, d1                  ;8239 000000FF
          or.b ($0100).l, d2                  ;8439 00000100
          or.b ($FF00).l, d3                  ;8639 0000FF00
          or.b (LATE_0001).l, d4              ;8839 00000001
          or.b (LATE_00FF).l, d5              ;8A39 000000FF
          or.b (LATE_0100).l, d6              ;8C39 00000100
          or.b (LATE_FF00).l, d7              ;8E39 0000FF00

          or.w ($0001).l, d0                  ;8079 00000001
          or.w ($00FF).l, d1                  ;8279 000000FF
          or.w ($0100).l, d2                  ;8479 00000100
          or.w ($FF00).l, d3                  ;8679 0000FF00
          or.w (LATE_0001).l, d4              ;8879 00000001
          or.w (LATE_00FF).l, d5              ;8A79 000000FF
          or.w (LATE_0100).l, d6              ;8C79 00000100
          or.w (LATE_FF00).l, d7              ;8E79 0000FF00

          or.l ($0001).l, d0                  ;80B9 00000001
          or.l ($00FF).l, d1                  ;82B9 000000FF
          or.l ($0100).l, d2                  ;84B9 00000100
          or.l ($FF00).l, d3                  ;86B9 0000FF00
          or.l (LATE_0001).l, d4              ;88B9 00000001
          or.l (LATE_00FF).l, d5              ;8AB9 000000FF
          or.l (LATE_0100).l, d6              ;8CB9 00000100
          or.l (LATE_FF00).l, d7              ;8EB9 0000FF00

          or   ($0001).l, d0                  ;8079 00000001
          or   ($00FF).l, d1                  ;8279 000000FF
          or   ($0100).l, d2                  ;8479 00000100
          or   ($FF00).l, d3                  ;8679 0000FF00
          or   (LATE_0001).l, d4              ;8879 00000001
          or   (LATE_00FF).l, d5              ;8A79 000000FF
          or   (LATE_0100).l, d6              ;8C79 00000100
          or   (LATE_FF00).l, d7              ;8E79 0000FF00

          or.b ($0001), d0                    ;8039 00000001
          or.b ($00FF), d1                    ;8239 000000FF
          or.b ($0100), d2                    ;8439 00000100
          or.b ($FF00), d3                    ;8639 0000FF00
          or.b (LATE_0001), d4                ;8839 00000001
          or.b (LATE_00FF), d5                ;8A39 000000FF
          or.b (LATE_0100), d6                ;8C39 00000100
          or.b (LATE_FF00), d7                ;8E39 0000FF00

          or.w ($0001), d0                    ;8079 00000001
          or.w ($00FF), d1                    ;8279 000000FF
          or.w ($0100), d2                    ;8479 00000100
          or.w ($FF00), d3                    ;8679 0000FF00
          or.w (LATE_0001), d4                ;8879 00000001
          or.w (LATE_00FF), d5                ;8A79 000000FF
          or.w (LATE_0100), d6                ;8C79 00000100
          or.w (LATE_FF00), d7                ;8E79 0000FF00

          or.l ($0001), d0                    ;80B9 00000001
          or.l ($00FF), d1                    ;82B9 000000FF
          or.l ($0100), d2                    ;84B9 00000100
          or.l ($FF00), d3                    ;86B9 0000FF00
          or.l (LATE_0001), d4                ;88B9 00000001
          or.l (LATE_00FF), d5                ;8AB9 000000FF
          or.l (LATE_0100), d6                ;8CB9 00000100
          or.l (LATE_FF00), d7                ;8EB9 0000FF00

          or   ($0001), d0                    ;8079 00000001
          or   ($00FF), d1                    ;8279 000000FF
          or   ($0100), d2                    ;8479 00000100
          or   ($FF00), d3                    ;8679 0000FF00
          or   (LATE_0001), d4                ;8879 00000001
          or   (LATE_00FF), d5                ;8A79 000000FF
          or   (LATE_0100), d6                ;8C79 00000100
          or   (LATE_FF00), d7                ;8E79 0000FF00


          or.b (a7), d0                       ;8017
          or.b (a6), d1                       ;8216
          or.w (a5), d2                       ;8455
          or.w (a4), d3                       ;8654
          or.l (a3), d4                       ;8893
          or.l (a2), d5                       ;8A92
          or   (a1), d6                       ;8C51
          or   (a0), d7                       ;8E50


          or.b $01(a0),d7                     ;8E28 0001
          or.b $02(a1),d6                     ;8C29 0002
          or.b $FE(a2),d5                     ;8A2A 00FE
          or.b $FF(a3),d4                     ;882B 00FF
          or.b LATE_01(a4),d3                 ;862C 0001
          or.b LATE_02(a5),d2                 ;842D 0002
          or.b LATE_FE(a6),d1                 ;822E 00FE
          or.b LATE_FF(a7),d0                 ;802F 00FF

          or.w $01(a0),d7                     ;8E68 0001
          or.w $02(a1),d6                     ;8C69 0002
          or.w $FE(a2),d5                     ;8A6A 00FE
          or.w $FF(a3),d4                     ;886B 00FF
          or.w LATE_01(a4),d3                 ;866C 0001
          or.w LATE_02(a5),d2                 ;846D 0002
          or.w LATE_FE(a6),d1                 ;826E 00FE
          or.w LATE_FF(a7),d0                 ;806F 00FF

          or.l $01(a0),d7                     ;8EA8 0001
          or.l $02(a1),d6                     ;8CA9 0002
          or.l $FE(a2),d5                     ;8AAA 00FE
          or.l $FF(a3),d4                     ;88AB 00FF
          or.l LATE_01(a4),d3                 ;86AC 0001
          or.l LATE_02(a5),d2                 ;84AD 0002
          or.l LATE_FE(a6),d1                 ;82AE 00FE
          or.l LATE_FF(a7),d0                 ;80AF 00FF

          or   $01(a0),d7                     ;8E68 0001
          or   $02(a1),d6                     ;8C69 0002
          or   $FE(a2),d5                     ;8A6A 00FE
          or   $FF(a3),d4                     ;886B 00FF
          or   LATE_01(a4),d3                 ;866C 0001
          or   LATE_02(a5),d2                 ;846D 0002
          or   LATE_FE(a6),d1                 ;826E 00FE
          or   LATE_FF(a7),d0                 ;806F 00FF


          or.b (a7)+, d0                      ;801F
          or.b (a6)+, d1                      ;821E
          or.w (a5)+, d2                      ;845D
          or.w (a4)+, d3                      ;865C
          or.l (a3)+, d4                      ;889B
          or.l (a2)+, d5                      ;8A9A
          or   (a1)+, d6                      ;8C59
          or   (a0)+, d7                      ;8E58

          or.b -(a7), d0                      ;8027
          or.b -(a6), d1                      ;8226
          or.w -(a5), d2                      ;8465
          or.w -(a4), d3                      ;8664
          or.l -(a3), d4                      ;88A3
          or.l -(a2), d5                      ;8AA2
          or   -(a1), d6                      ;8C61
          or   -(a0), d7                      ;8E60


          or.b d0, ($0001).w           ;8138 0001
          or.b d1, ($00FF).w           ;8338 00FF
          or.w d2, ($0100).w           ;8578 0100
          or.w d3, ($FF00).w           ;8778 FF00
          or.l d4, (LATE_0001).w       ;89B8 0001
          or.l d5, (LATE_00FF).w       ;8BB8 00FF
          or   d6, (LATE_0100).w       ;8D78 0100
          or   d7, (LATE_FF00).w       ;8F78 FF00

          or.b d0, ($0001).l           ;8139 00000001
          or.b d1, ($00FF).l           ;8339 000000FF
          or.w d2, ($0100).l           ;8579 00000100
          or.w d3, ($FF00).l           ;8779 0000FF00
          or.l d4, (LATE_0001).l       ;89B9 00000001
          or.l d5, (LATE_00FF).l       ;8BB9 000000FF
          or   d6, (LATE_0100).l       ;8D79 00000100
          or   d7, (LATE_FF00).l       ;8F79 0000FF00

          or.b d0, ($0001)             ;8138 0001   < downgrade
          or.b d1, ($00FF)             ;8338 00FF   < downgrade
          or.w d2, ($0100)             ;8578 0100   < downgrade
          or.w d3, ($FF00)             ;8779 0000FF00
          or.l d4, (LATE_0001)         ;89B9 00000001
          or.l d5, (LATE_00FF)         ;8BB9 000000FF
          or   d6, (LATE_0100)         ;8D79 00000100
          or   d7, (LATE_FF00)         ;8F79 0000FF00


          or.b d0,(a7)           ;8117
          or.b d1,(a6)           ;8316
          or.w d2,(a5)           ;8555
          or.w d3,(a4)           ;8754
          or.l d4,(a3)           ;8993
          or.l d5,(a2)           ;8B92
          or   d6,(a1)           ;8D51
          or   d7,(a0)           ;8F50


          or.b d0, $01(a7)       ;812F 0001
          or.b d1, $02(a6)       ;832E 0002
          or.w d2, $FE(a5)       ;856D 00FE
          or.w d3, $FF(a4)       ;876C 00FF
          or.l d4, LATE_01(a3)   ;89AB 0001
          or.l d5, LATE_02(a2)   ;8BAA 0002
          or   d6, LATE_FE(a1)   ;8D69 00FE
          or   d7, LATE_FF(a0)   ;8F68 00FF

          or.b d0,(a7)+          ;811F
          or.b d1,(a6)+          ;831E
          or.w d2,(a5)+          ;855D
          or.w d3,(a4)+          ;875C
          or.l d4,(a3)+          ;899B
          or.l d5,(a2)+          ;8B9A
          or   d6,(a1)+          ;8D59
          or   d7,(a0)+          ;8F58

          or.b d0,-(a7)          ;8127
          or.b d1,-(a6)          ;8326
          or.w d2,-(a5)          ;8565
          or.w d3,-(a4)          ;8764
          or.l d4,-(a3)          ;89A3
          or.l d5,-(a2)          ;8BA2
          or   d6,-(a1)          ;8D61
          or   d7,-(a0)          ;8F60

          pea ($0001).w           ;4878 0001
          pea ($00FF).w           ;4878 00FF
          pea ($0100).w           ;4878 0100
          pea ($FF00).w           ;4878 FF00
          pea (LATE_0001).w       ;4878 0001
          pea (LATE_00FF).w       ;4878 00FF
          pea (LATE_0100).w       ;4878 0100
          pea (LATE_FF00).w       ;4878 FF00

          pea ($0001).l           ;4879 00000001
          pea ($00FF).l           ;4879 000000FF
          pea ($0100).l           ;4879 00000100
          pea ($FF00).l           ;4879 0000FF00
          pea (LATE_0001).l       ;4879 00000001
          pea (LATE_00FF).l       ;4879 000000FF
          pea (LATE_0100).l       ;4879 00000100
          pea (LATE_FF00).l       ;4879 0000FF00

          pea ($0001)             ;4878 0001    < downgrade
          pea ($00FF)             ;4878 00FF    < downgrade
          pea ($0100)             ;4878 0100    < downgrade
          pea ($FF00)             ;4879 0000FF00
          pea (LATE_0001)         ;4879 00000001
          pea (LATE_00FF)         ;4879 000000FF
          pea (LATE_0100)         ;4879 00000100
          pea (LATE_FF00)         ;4879 0000FF00

          ;does not exist
          ;pea d0                  ;48 40
          ;pea d1                  ;48 41
          ;pea d2                  ;48 42
          ;pea d3                  ;48 43
          ;pea d4                  ;48 44
          ;pea d5                  ;48 45
          ;pea d6                  ;48 46
          ;pea d7                  ;48 47

          pea (a0)                ;4850
          pea (a1)                ;4851
          pea (a2)                ;4852
          pea (a3)                ;4853
          pea (a4)                ;4854
          pea (a5)                ;4855
          pea (a6)                ;4856
          pea (a7)                ;4857

          pea $01(a7)             ;48 6F 00 01
          pea $02(a6)             ;48 6E 00 02
          pea $FE(a5)             ;48 6D 00 FE
          pea $FF(a4)             ;48 6C 00 FF
          pea LATE_01(a3)         ;48 6B 00 01
          pea LATE_02(a2)         ;48 6A 00 02
          pea LATE_FE(a1)         ;48 69 00 FE
          pea LATE_FF(a0)         ;48 68 00 FF

          rol.b d0, d7            ;E13F
          rol.b d1, d6            ;E33E
          rol.w d2, d5            ;E57D
          rol.w d3, d4            ;E77C
          rol.l d4, d3            ;E9BB
          rol.l d5, d2            ;EBBA
          rol   d6, d1            ;ED79
          rol   d7, d0            ;EF78

          ror.b #8, d7            ;E01F
          ror.b #1, d6            ;E21E
          ror.w #2, d5            ;E45D
          ror.w #3, d4            ;E65C
          ror.l #4, d3            ;E89B
          ror.l #5, d2            ;EA9A
          ror   #6, d1            ;EC59
          ror   #7, d0            ;EE58

          rol   ($0001).w         ;E7F8 0001
          rol   ($00FF).w         ;E7F8 00FF
          rol.w ($0100).w         ;E7F8 0100
          rol.w ($FF00).w         ;E7F8 FF00
          rol.w (LATE_0001).w     ;E7F8 0001
          rol.w (LATE_00FF).w     ;E7F8 00FF
          rol   (LATE_0100).w     ;E7F8 0100
          rol   (LATE_FF00).w     ;E7F8 FF00

          rol   ($0001).l         ;E7F9 00000001
          rol   ($00FF).l         ;E7F9 000000FF
          rol.w ($0100).l         ;E7F9 00000100
          rol.w ($FF00).l         ;E7F9 0000FF00
          rol.w (LATE_0001).l     ;E7F9 00000001
          rol.w (LATE_00FF).l     ;E7F9 000000FF
          rol   (LATE_0100).l     ;E7F9 00000100
          rol   (LATE_FF00).l     ;E7F9 0000FF00

          rol   ($0001)           ;E7F8 0001      < downgrade
          rol   ($00FF)           ;E7F8 00FF      < downgrade
          rol.w ($0100)           ;E7F8 0100      < downgrade
          rol.w ($FF00)           ;E7F9 0000FF00
          rol.w (LATE_0001)       ;E7F9 00000001
          rol.w (LATE_00FF)       ;E7F9 000000FF
          rol   (LATE_0100)       ;E7F9 00000100
          rol   (LATE_FF00)       ;E7F9 0000FF00

          rte       ;4E 73
          rtr       ;4E 77
          rts       ;4E 75

          st ($0001).w         ;50F8 0001
          st ($00FF).w         ;50F8 00FF
          st ($0100).w         ;50F8 0100
          st ($FF00).l         ;50F9 0000FF00
          st (LATE_0001).l     ;50F9 00000001
          st (LATE_00FF).l     ;50F9 000000FF
          st (LATE_0100)       ;50F9 00000100
          st (LATE_FF00)       ;50F9 0000FF00

          sf ($0001).w         ;51F8 0001
          sf ($00FF).w         ;51F8 00FF
          sf ($0100).w         ;51F8 0100
          sf ($FF00).l         ;51F9 0000FF00
          sf (LATE_0001).l     ;51F9 00000001
          sf (LATE_00FF).l     ;51F9 000000FF
          sf (LATE_0100)       ;51F9 00000100
          sf (LATE_FF00)       ;51F9 0000FF00

          shi ($0001).w        ;52F8 0001
          shi ($00FF).w        ;52F8 00FF
          shi ($0100).w        ;52F8 0100
          shi ($FF00).l        ;52F9 0000FF00
          shi (LATE_0001).l    ;52F9 00000001
          shi (LATE_00FF).l    ;52F9 000000FF
          shi (LATE_0100)      ;52F9 00000100
          shi (LATE_FF00)      ;52F9 0000FF00

          sls ($0001).w        ;53F8 0001
          sls ($00FF).w        ;53F8 00FF
          sls ($0100).w        ;53F8 0100
          sls ($FF00).l        ;53F9 0000FF00
          sls (LATE_0001).l    ;53F9 00000001
          sls (LATE_00FF).l    ;53F9 000000FF
          sls (LATE_0100)      ;53F9 00000100
          sls (LATE_FF00)      ;53F9 0000FF00

          scc ($0001).w        ;54F8 0001
          scc ($00FF).w        ;54F8 00FF
          scc ($0100).w        ;54F8 0100
          scc ($FF00).l        ;54F9 0000FF00
          scc (LATE_0001).l    ;54F9 00000001
          scc (LATE_00FF).l    ;54F9 000000FF
          scc (LATE_0100)      ;54F9 00000100
          scc (LATE_FF00)      ;54F9 0000FF00

          scs ($0001).w        ;55F8 0001
          scs ($00FF).w        ;55F8 00FF
          scs ($0100).w        ;55F8 0100
          scs ($FF00).l        ;55F9 0000FF00
          scs (LATE_0001).l    ;55F9 00000001
          scs (LATE_00FF).l    ;55F9 000000FF
          scs (LATE_0100)      ;55F9 00000100
          scs (LATE_FF00)      ;55F9 0000FF00

          sne ($0001).w        ;56F8 0001
          sne ($00FF).w        ;56F8 00FF
          sne ($0100).w        ;56F8 0100
          sne ($FF00).l        ;56F9 0000FF00
          sne (LATE_0001).l    ;56F9 00000001
          sne (LATE_00FF).l    ;56F9 000000FF
          sne (LATE_0100)      ;56F9 00000100
          sne (LATE_FF00)      ;56F9 0000FF00

          seq ($0001).w        ;57F8 0001
          seq ($00FF).w        ;57F8 00FF
          seq ($0100).w        ;57F8 0100
          seq ($FF00).l        ;57F9 0000FF00
          seq (LATE_0001).l    ;57F9 00000001
          seq (LATE_00FF).l    ;57F9 000000FF
          seq (LATE_0100)      ;57F9 00000100
          seq (LATE_FF00)      ;57F9 0000FF00

          svc ($0001).w        ;58F8 0001
          svc ($00FF).w        ;58F8 00FF
          svc ($0100).w        ;58F8 0100
          svc ($FF00).l        ;58F9 0000FF00
          svc (LATE_0001).l    ;58F9 00000001
          svc (LATE_00FF).l    ;58F9 000000FF
          svc (LATE_0100)      ;58F9 00000100
          svc (LATE_FF00)      ;58F9 0000FF00

          svs ($0001).w        ;59F8 0001
          svs ($00FF).w        ;59F8 00FF
          svs ($0100).w        ;59F8 0100
          svs ($FF00).l        ;59F9 0000FF00
          svs (LATE_0001).l    ;59F9 00000001
          svs (LATE_00FF).l    ;59F9 000000FF
          svs (LATE_0100)      ;59F9 00000100
          svs (LATE_FF00)      ;59F9 0000FF00

          spl ($0001).w        ;5AF8 0001
          spl ($00FF).w        ;5AF8 00FF
          spl ($0100).w        ;5AF8 0100
          spl ($FF00).l        ;5AF9 0000FF00
          spl (LATE_0001).l    ;5AF9 00000001
          spl (LATE_00FF).l    ;5AF9 000000FF
          spl (LATE_0100)      ;5AF9 00000100
          spl (LATE_FF00)      ;5AF9 0000FF00

          smi ($0001).w        ;5BF8 0001
          smi ($00FF).w        ;5BF8 00FF
          smi ($0100).w        ;5BF8 0100
          smi ($FF00).l        ;5BF9 0000FF00
          smi (LATE_0001).l    ;5BF9 00000001
          smi (LATE_00FF).l    ;5BF9 000000FF
          smi (LATE_0100)      ;5BF9 00000100
          smi (LATE_FF00)      ;5BF9 0000FF00

          sge ($0001).w        ;5CF8 0001
          sge ($00FF).w        ;5CF8 00FF
          sge ($0100).w        ;5CF8 0100
          sge ($FF00).l        ;5CF9 0000FF00
          sge (LATE_0001).l    ;5CF9 00000001
          sge (LATE_00FF).l    ;5CF9 000000FF
          sge (LATE_0100)      ;5CF9 00000100
          sge (LATE_FF00)      ;5CF9 0000FF00

          slt ($0001).w        ;5DF8 0001
          slt ($00FF).w        ;5DF8 00FF
          slt ($0100).w        ;5DF8 0100
          slt ($FF00).l        ;5DF9 0000FF00
          slt (LATE_0001).l    ;5DF9 00000001
          slt (LATE_00FF).l    ;5DF9 000000FF
          slt (LATE_0100)      ;5DF9 00000100
          slt (LATE_FF00)      ;5DF9 0000FF00

          sgt ($0001).w        ;5EF8 0001
          sgt ($00FF).w        ;5EF8 00FF
          sgt ($0100).w        ;5EF8 0100
          sgt ($FF00).l        ;5EF9 0000FF00
          sgt (LATE_0001).l    ;5EF9 00000001
          sgt (LATE_00FF).l    ;5EF9 000000FF
          sgt (LATE_0100)      ;5EF9 00000100
          sgt (LATE_FF00)      ;5EF9 0000FF00

          sle ($0001).w        ;5FF8 0001
          sle ($00FF).w        ;5FF8 00FF
          sle ($0100).w        ;5FF8 0100
          sle ($FF00).l        ;5FF9 0000FF00
          sle (LATE_0001).l    ;5FF9 00000001
          sle (LATE_00FF).l    ;5FF9 000000FF
          sle (LATE_0100)      ;5FF9 00000100
          sle (LATE_FF00)      ;5FF9 0000FF00

          stop #$0001          ;4E 72 00 01
          stop #$00FF          ;4E 72 00 FF
          stop #$0100          ;4E 72 01 00
          stop #$FF00          ;4E 72 FF 00
          stop #LATE_0001      ;4E 72 00 01
          stop #LATE_00FF      ;4E 72 00 FF
          stop #LATE_0100      ;4E 72 01 00
          stop #LATE_FF00      ;4E 72 FF 00


          suba.w #$0001,a0            ;90FC 0001 - 5348
          suba.w #$00FF,a1            ;92FC 00FF
          suba.w #$0100,a2            ;94FC 0100
          suba.w #$FF00,a3            ;96FC FF00
          suba.w #LATE_0001,a4        ;98FC 0001 - 534C
          suba.w #LATE_00FF,a5        ;9AFC 00FF
          suba.w #LATE_0100,a6        ;9CFC 0100
          suba.w #LATE_FF00,a7        ;9EFC FF00
          suba.w #$1234,sp            ;9EFC 1234

          suba.l #$0001,a0            ;91FC 00000001  - 5388
          suba.l #$00FF,a1            ;93FC 000000FF
          suba.l #$0100,a2            ;95FC 00000100
          suba.l #$FF00,a3            ;97FC 0000FF00
          suba.l #LATE_0001,a4        ;99FC 00000001
          suba.l #LATE_00FF,a5        ;9BFC 000000FF
          suba.l #LATE_0100,a6        ;9DFC 00000100
          suba.l #LATE_FF00,a7        ;9FFC 0000FF00
          suba.l #$1234,sp            ;9FFC 00001234

          suba   #$0001,a0            ;90FC 0001 - 5348
          suba   #$00FF,a1            ;92FC 00FF
          suba   #$0100,a2            ;94FC 0100
          suba   #$FF00,a3            ;96FC FF00
          suba   #LATE_0001,a4        ;98FC 0001 - 534C
          suba   #LATE_00FF,a5        ;9AFC 00FF
          suba   #LATE_0100,a6        ;9CFC 0100
          suba   #LATE_FF00,a7        ;9EFC FF00
          suba   #$1234,sp            ;9EFC 1234


          suba.w ($0001).w, a0          ;90F8 0001
          suba.w ($00FF).w, a1          ;92F8 00FF
          suba.w ($0100).w, a2          ;94F8 0100
          suba.w ($FF00).w, a3          ;96F8 FF00
          suba.w (LATE_0001).w, a4      ;98F8 0001
          suba.w (LATE_00FF).w, a5      ;9AF8 00FF
          suba.w (LATE_0100).w, a6      ;9CF8 0100
          suba.w (LATE_FF00).w, a7      ;9EF8 FF00
          suba.w ($1234).w, sp          ;9EF8 1234

          suba.w ($0001).l, a0          ;90F9 00000001
          suba.w ($00FF).l, a1          ;92F9 000000FF
          suba.w ($0100).l, a2          ;94F9 00000100
          suba.w ($FF00).l, a3          ;96F9 0000FF00
          suba.w (LATE_0001).l, a4      ;98F9 00000001
          suba.w (LATE_00FF).l, a5      ;9AF9 000000FF
          suba.w (LATE_0100).l, a6      ;9CF9 00000100
          suba.w (LATE_FF00).l, a7      ;9EF9 0000FF00
          suba.w ($1234).l, sp          ;9EF9 00001234

          suba.l ($0001).w, a0          ;91F8 0001
          suba.l ($00FF).w, a1          ;93F8 00FF
          suba.l ($0100).w, a2          ;95F8 0100
          suba.l ($FF00).w, a3          ;97F8 FF00
          suba.l (LATE_0001).w, a4      ;99F8 0001
          suba.l (LATE_00FF).w, a5      ;9BF8 00FF
          suba.l (LATE_0100).w, a6      ;9DF8 0100
          suba.l (LATE_FF00).w, a7      ;9FF8 FF00
          suba.l ($1234).w, sp          ;9FF8 1234

          suba.l ($000001).l, a0        ;91F9 0000 0001
          suba.l ($00FF00).l, a1        ;93F9 0000 FF00
          suba.l ($010000).l, a2        ;95F9 0001 0000
          suba.l ($FF0000).l, a3        ;97F9 00FF 0000
          suba.l (LATE_000001).l, a4    ;99F9 0000 0001
          suba.l (LATE_00FF00).l, a5    ;9BF9 0000 FF00
          suba.l (LATE_010000).l, a6    ;9DF9 0001 0000
          suba.l (LATE_FF0000).l, a7    ;9FF9 00FF 0000
          suba.l ($123456).l, sp        ;9FF9 0012 3456

          suba   ($0001).w, a0          ;90F8 0001
          suba   ($00FF).w, a1          ;92F8 00FF
          suba   ($0100).w, a2          ;94F8 0100
          suba   ($FF00).w, a3          ;96F8 FF00
          suba   (LATE_0001).w, a4      ;98F8 0001
          suba   (LATE_00FF).w, a5      ;9AF8 00FF
          suba   (LATE_0100).w, a6      ;9CF8 0100
          suba   (LATE_FF00).w, a7      ;9EF8 FF00
          suba   ($1234).w, sp          ;9EF8 1234

          suba   ($0001).l, a0          ;90F9 00000001
          suba   ($00FF).l, a1          ;92F9 000000FF
          suba   ($0100).l, a2          ;94F9 00000100
          suba   ($FF00).l, a3          ;96F9 0000FF00
          suba   (LATE_0001).l, a4      ;98F9 00000001
          suba   (LATE_00FF).l, a5      ;9AF9 000000FF
          suba   (LATE_0100).l, a6      ;9CF9 00000100
          suba   (LATE_FF00).l, a7      ;9EF9 0000FF00
          suba   ($1234).l, sp          ;9EF9 00001234


          suba.w d7, a0                 ;90C7
          suba.w d6, a1                 ;92C6
          suba.w d5, a2                 ;94C5
          suba.w d4, a3                 ;96C4
          suba.l d3, a4                 ;99C3
          suba.l d2, a5                 ;9BC2
          suba   d1, a6                 ;9CC1
          suba   d0, a7                 ;9EC0
          suba   d0, sp                 ;9EC0

          suba.w (sp), a0               ;90D7
          suba.w (a7), a1               ;92D7
          suba.w (a6), a2               ;94D6
          suba.w (a5), a3               ;96D5
          suba.l (a4), a4               ;99D4
          suba.l (a3), a5               ;9BD3
          suba   (a2), a6               ;9CD2
          suba   (a1), a7               ;9ED1
          suba   (a0), sp               ;9ED0

          suba.w $0001(a0), a7          ;9EE8 0001
          suba.w $00FF(a1), a6          ;9CE9 00FF
          suba.w $0100(a2), a5          ;9AEA 0100
          suba.w $7F00(a3), a4          ;98EB 7F00
          suba.l LATE_0001(a4), a3      ;97EC 0001
          suba.l LATE_00FF(a5), a2      ;95ED 00FF
          suba   LATE_0100(a6), a1      ;92EE 0100
          suba   LATE_7F00(a7), a0      ;90EF 7F00

          suba.w (sp)+, a0              ;90DF
          suba.w (a7)+, a1              ;92DF
          suba.w (a6)+, a2              ;94DE
          suba.w (a5)+, a3              ;96DD
          suba.l (a4)+, a4              ;99DC
          suba.l (a3)+, a5              ;9BDB
          suba   (a2)+, a6              ;9CDA
          suba   (a1)+, a7              ;9ED9
          suba   (a0)+, sp              ;9ED8

          suba.w -(sp), a0              ;90E7
          suba.w -(a7), a1              ;92E7
          suba.w -(a6), a2              ;94E6
          suba.w -(a5), a3              ;96E5
          suba.l -(a4), a4              ;99E4
          suba.l -(a3), a5              ;9BE3
          suba   -(a2), a6              ;9CE2
          suba   -(a1), a7              ;9EE1
          suba   -(a0), sp              ;9EE0

          suba.w (a0,d7.w), a7          ;9EF0 7000
          suba.w (a1,d6  ), a6          ;9CF1 6000
          suba.w (a2,d5.l), a5          ;9AF2 5800
          suba.w (a3,d4  ), a4          ;98F3 4000
          suba.l (a4,d3.w), a3          ;97F4 3000
          suba.l (a5,d2  ), a2          ;95F5 2000
          suba.l (a6,d1.l), a1          ;93F6 1800
          suba.l (a7,d0  ), a0          ;91F7 0000
          suba   (a3,d4  ), a4          ;98F3 4000
          suba   (a4,d3.w), a3          ;96F4 3000
          suba   (a5,d2.l), a2          ;94F5 2800

          suba.w $01(a0,d7.w), a7       ;9EF0 7001
          suba.w $7F(a1,d6), a6         ;9CF1 607F
          suba.w LATE_01(a2,d5.l), a5   ;9AF2 5801
          suba.w LATE_7F(a3,d4), a4     ;98F3 407F
          suba.l $01(a4,d3.w), a3       ;97F4 3001
          suba.l $7F(a5,d2), a2         ;95F5 207F
          suba.l LATE_01(a6,d1.l), a1   ;93F6 1801
          suba.l LATE_7F(a7,d0), a0     ;91F7 007F
          suba   LATE_7F(a3,d4), a4     ;98F3 407F
          suba   $01(a4,d3.w), a3       ;96F4 3001
          suba   $7F(a5,d2.l), a2       ;94F5 287F

          subi.b #$01,d0                ;0400 0001  - 5300
          subi.b #$FF,d1                ;0401 00FF
          subi.b #$01,d2                ;0402 0001  - 5302
          subi.b #$FF,d3                ;0403 00FF
          subi.b #LATE_01,d4            ;0404 0001  - 5304
          subi.b #LATE_FF,d5            ;0405 00FF
          subi.b #LATE_01,d6            ;0406 0001  - 5306
          subi.b #LATE_FF,d7            ;0407 00FF

          subi.w #$01,d0                ;0440 0001  - 5340
          subi.w #$FF,d1                ;0441 00FF
          subi.w #$01,d2                ;0442 0001  - 5342
          subi.w #$FF,d3                ;0443 00FF
          subi.w #LATE_01,d4            ;0444 0001
          subi.w #LATE_FF,d5            ;0445 00FF
          subi.w #LATE_01,d6            ;0446 0001
          subi.w #LATE_FF,d7            ;0447 00FF

          subi.l #$00000001,d0          ;0480 00000001  - 5380
          subi.l #$000000FF,d1          ;0481 000000FF
          subi.l #$00000100,d2          ;0482 00000100
          subi.l #$0000FF00,d3          ;0483 0000FF00
          subi.l #$00010000,d4          ;0484 00010000
          subi.l #$00FF0000,d5          ;0485 00FF0000
          subi.l #$01000000,d6          ;0486 01000000
          subi.l #$FF000000,d7          ;0487 FF000000

          subi   #$01,d0                ;0440 0001  - 5340
          subi   #$FF,d1                ;0441 00FF
          subi   #$01,d2                ;0442 0001  - 5342
          subi   #$FF,d3                ;0443 00FF
          subi   #LATE_01,d4            ;0444 0001
          subi   #LATE_FF,d5            ;0445 00FF
          subi   #LATE_01,d6            ;0446 0001
          subi   #LATE_FF,d7            ;0447 00FF


          subi.b #$01, ($FF00).w                ;0438 0001 FF00  - 5338 FF00
          subi.b #$02, ($0100).w                ;0438 0002 0100  - 5538 0100
          subi.b #$FE, (LATE_00FF).w            ;0438 00FE 00FF
          subi.b #$FF, (LATE_0001).w            ;0438 00FF 0001
          subi.b #LATE_01, ($FF00).w            ;0438 0001 FF00
          subi.b #LATE_02, ($0100).w            ;0438 0002 0100
          subi.b #LATE_FE, (LATE_00FF).w        ;0438 00FE 00FF
          subi.b #LATE_FF, (LATE_0001).w        ;0438 00FF 0001

          subi.b #$01, ($FF000000).l            ;0439 0001 FF00 0000  - 5339 FF000000
          subi.b #$02, ($00010000).l            ;0439 0002 0001 0000  - 5539 00010000
          subi.b #$FE, (LATE_0000FF00).l        ;0439 00FE 0000 FF00
          subi.b #$FF, (LATE_00000001).l        ;0439 00FF 0000 0001
          subi.b #LATE_01, ($FF000000).l        ;0439 0001 FF00 0000
          subi.b #LATE_02, ($00010000).l        ;0439 0002 0001 0000
          subi.b #LATE_FE, (LATE_0000FF00).l    ;0439 00FE 0000 FF00
          subi.b #LATE_FF, (LATE_00000001).l    ;0439 00FF 0000 0001

          subi.b #$01, ($FF000000)              ;0439 0001 FF00 0000  - 5339 FF000000
          subi.b #$02, ($00010000)              ;0439 0002 0001 0000  - 5539 00010000
          subi.b #$FE, (LATE_0000FF00)          ;0439 00FE 0000 FF00
          subi.b #$FF, (LATE_00000001)          ;0439 00FF 0000 0001
          subi.b #LATE_01, ($FF000000)          ;0439 0001 FF00 0000
          subi.b #LATE_02, ($00010000)          ;0439 0002 0001 0000
          subi.b #LATE_FE, (LATE_0000FF00)      ;0439 00FE 0000 FF00
          subi.b #LATE_FF, (LATE_00000001)      ;0439 00FF 0000 0001

          subi.w #$0001, ($FF00).w              ;0478 0001 FF00  - 5378 FF00
          subi.w #$00FF, ($0100).w              ;0478 00FF 0100
          subi.w #$0100, (LATE_00FF).w          ;0478 0100 00FF
          subi.w #$FF00, (LATE_0001).w          ;0478 FF00 0001
          subi.w #LATE_0001, ($FF00).w          ;0478 0001 FF00
          subi.w #LATE_00FF, ($0100).w          ;0478 00FF 0100
          subi.w #LATE_0100, (LATE_00FF).w      ;0478 0100 00FF
          subi.w #LATE_FF00, (LATE_0001).w      ;0478 FF00 0001

          subi.w #$0001, ($FF000000).l          ;0479 0001 FF00 0000  - 5379 FF000000
          subi.w #$00FF, ($00010000).l          ;0479 00FF 0001 0000
          subi.w #$0100, (LATE_0000FF00).l      ;0479 0100 0000 FF00
          subi.w #$FF00, (LATE_00000001).l      ;0479 FF00 0000 0001
          subi.w #LATE_0001, ($FF000000).l      ;0479 0001 FF00 0000
          subi.w #LATE_00FF, ($00010000).l      ;0479 00FF 0001 0000
          subi.w #LATE_0100, (LATE_0000FF00).l  ;0479 0100 0000 FF00
          subi.w #LATE_FF00, (LATE_00000001).l  ;0479 FF00 0000 0001

          subi.w #$0001, ($FF000000)            ;0479 0001 FF00 0000  - 5379 FF000000
          subi.w #$00FF, ($00010000)            ;0479 00FF 0001 0000
          subi.w #$0100, (LATE_0000FF00)        ;0479 0100 0000 FF00
          subi.w #$FF00, (LATE_00000001)        ;0479 FF00 0000 0001
          subi.w #LATE_0001, ($FF000000)        ;0479 0001 FF00 0000
          subi.w #LATE_00FF, ($00010000)        ;0479 00FF 0001 0000
          subi.w #LATE_0100, (LATE_0000FF00)    ;0479 0100 0000 FF00
          subi.w #LATE_FF00, (LATE_00000001)    ;0479 FF00 0000 0001

          subi.l #$00000001, ($FF00).w          ;04B8 0000 0001 FF00  - 53B8 FF00
          subi.l #$0000FF00, ($0100).w          ;04B8 0000 FF00 0100
          subi.l #$00010000, (LATE_00FF).w      ;04B8 0001 0001 00FF
          subi.l #$FF000000, (LATE_0001).w      ;04B8 FF00 0001 0001
          subi.l #LATE_00000001, ($FF00).w      ;04B8 0000 0001 FF00
          subi.l #LATE_0000FF00, ($0100).w      ;04B8 0000 FF00 0100
          subi.l #LATE_00010000, (LATE_00FF).w  ;04B8 0001 0000 00FF
          subi.l #LATE_FF000000, (LATE_0001).w  ;04B8 FF00 0000 0001

          subi.l #$0001, ($FF000000).l          ;04B9 00000001 FF00 0000  - 53B9 FF000000
          subi.l #$00FF, ($00010000).l          ;04B9 000000FF 0001 0000
          subi.l #$0100, (LATE_0000FF00).l      ;04B9 00000100 0000 FF00
          subi.l #$FF00, (LATE_00000001).l      ;04B9 0000FF00 0000 0001
          subi.l #LATE_0001, ($FF000000).l      ;04B9 00000001 FF00 0000
          subi.l #LATE_00FF, ($00010000).l      ;04B9 000000FF 0001 0000
          subi.l #LATE_0100, (LATE_0000FF00).l  ;04B9 00000100 0000 FF00
          subi.l #LATE_FF00, (LATE_00000001).l  ;04B9 0000FF00 0000 0001

          subi.l #$0001, ($FF000000)            ;04B9 00000001 FF00 0000  - 53B9 FF000000
          subi.l #$00FF, ($00010000)            ;04B9 000000FF 0001 0000
          subi.l #$0100, (LATE_0000FF00)        ;04B9 00000100 0000 FF00
          subi.l #$FF00, (LATE_00000001)        ;04B9 0000FF00 0000 0001
          subi.l #LATE_0001, ($FF000000)        ;04B9 00000001 FF00 0000
          subi.l #LATE_00FF, ($00010000)        ;04B9 000000FF 0001 0000
          subi.l #LATE_0100, (LATE_0000FF00)    ;04B9 00000100 0000 FF00
          subi.l #LATE_FF00, (LATE_00000001)    ;04B9 0000FF00 0000 0001

          subi   #$0001, ($FF00).w              ;0478 0001 FF00  - 5378 FF00
          subi   #$00FF, ($0100).w              ;0478 00FF 0100
          subi   #$0100, (LATE_00FF).w          ;0478 0100 00FF
          subi   #$FF00, (LATE_0001).w          ;0478 FF00 0001
          subi   #LATE_0001, ($FF00).w          ;0478 0001 FF00
          subi   #LATE_00FF, ($0100).w          ;0478 00FF 0100
          subi   #LATE_0100, (LATE_00FF).w      ;0478 0100 00FF
          subi   #LATE_FF00, (LATE_0001).w      ;0478 FF00 0001

          subi   #$0001, ($FF000000).l          ;0479 0001 FF00 0000  - 5379 FF000000
          subi   #$00FF, ($00010000).l          ;0479 00FF 0001 0000
          subi   #$0100, (LATE_0000FF00).l      ;0479 0100 0000 FF00
          subi   #$FF00, (LATE_00000001).l      ;0479 FF00 0000 0001
          subi   #LATE_0001, ($FF000000).l      ;0479 0001 FF00 0000
          subi   #LATE_00FF, ($00010000).l      ;0479 00FF 0001 0000
          subi   #LATE_0100, (LATE_0000FF00).l  ;0479 0100 0000 FF00
          subi   #LATE_FF00, (LATE_00000001).l  ;0479 FF00 0000 0001

          subi   #$0001, ($FF000000)            ;0479 0001 FF00 0000  - 5379 FF000000
          subi   #$00FF, ($00010000)            ;0479 00FF 0001 0000
          subi   #$0100, (LATE_0000FF00)        ;0479 0100 0000 FF00
          subi   #$FF00, (LATE_00000001)        ;0479 FF00 0000 0001
          subi   #LATE_0001, ($FF000000)        ;0479 0001 FF00 0000
          subi   #LATE_00FF, ($00010000)        ;0479 00FF 0001 0000
          subi   #LATE_0100, (LATE_0000FF00)    ;0479 0100 0000 FF00
          subi   #LATE_FF00, (LATE_00000001)    ;0479 FF00 0000 0001


          subi.b #$01, (a7)                     ;0417 0001  - 5317
          subi.b #$02, (a6)                     ;0416 0002  - 5516
          subi.w #$FE, (a5)                     ;0455 00FE
          subi.w #$FF, (a4)                     ;0454 00FF
          subi.l #LATE_01, (a3)                 ;0493 0001  - 5313
          subi.l #LATE_02, (a2)                 ;0492 0002  - 5512
          subi   #LATE_FE, (a1)                 ;0451 00FE
          subi   #LATE_FF, (a0)                 ;0450 00FF


          subi.b #$01, $FF(a0)                  ;0428 0001 00FF  - 5328 00FF
          subi.b #$02, $FE(a1)                  ;0429 0002 00FE  - 5529 00FE
          subi.b #$FE, LATE_02(a2)              ;042A 00FE 0002
          subi.b #$FF, LATE_01(a3)              ;042B 00FF 0001
          subi.b #LATE_01, $FF(a4)              ;042C 0001 00FF
          subi.b #LATE_02, $FE(a5)              ;042D 0002 00FE
          subi.b #LATE_FE, LATE_02(a6)          ;042E 00FE 0002
          subi.b #LATE_FF, LATE_01(a7)          ;042F 00FF 0001

          subi.w #$0001, $FF(a0)                ;0468 0001 00FF  - 5368 00FF
          subi.w #$00FF, $FE(a1)                ;0469 00FF 00FE
          subi.w #$0100, LATE_02(a2)            ;046A 0100 0002
          subi.w #$FF00, LATE_01(a3)            ;046B FF00 0001
          subi.w #LATE_0001, $FF(a4)            ;046C 0001 00FF
          subi.w #LATE_00FF, $FE(a5)            ;046D 00FF 00FE
          subi.w #LATE_0100, LATE_02(a6)        ;046E 0100 0002
          subi.w #LATE_FF00, LATE_01(a7)        ;046F FF00 0001

          subi.l #$00000001, $FF(a0)            ;04A8 00000001 00FF  - 53A8 00FF
          subi.l #$0000FF00, $01(a1)            ;04A9 0000FF00 0001
          subi.l #$00010000, LATE_FF(a2)        ;04AA 00010000 00FF
          subi.l #$FF000000, LATE_01(a3)        ;04AB FF000000 0001
          subi.l #LATE_00000001, $FF(a4)        ;04AC 00000001 00FF
          subi.l #LATE_0000FF00, $01(a5)        ;04AD 0000FF00 0001
          subi.l #LATE_00010000, LATE_FF(a6)    ;04AE 00010000 00FF
          subi.l #LATE_FF000000, LATE_01(a7)    ;04AF FF000000 0001

          subi   #$0001, $FF(a0)                ;0468 0001 00FF  - 5368 00FF
          subi   #$00FF, $FE(a1)                ;0469 00FF 00FE
          subi   #$0100, LATE_02(a2)            ;046A 0100 0002
          subi   #$FF00, LATE_01(a3)            ;046B FF00 0001
          subi   #LATE_0001, $FF(a4)            ;046C 0001 00FF
          subi   #LATE_00FF, $FE(a5)            ;046D 00FF 00FE
          subi   #LATE_0100, LATE_02(a6)        ;046E 0100 0002
          subi   #LATE_FF00, LATE_01(a7)        ;046F FF00 0001


          subi.b #$01, (a0)+                ;0418 0001  - 5318
          subi.b #$FF, (a1)+                ;0419 0002  - 5519
          subi.w #$0001, (a2)+              ;045A 00FE  - 535A
          subi.w #$FFFF, (a3)+              ;045B 00FF
          subi.l #$01, (a4)+                ;049C 00000001  - 539C
          subi.l #LATE_02, (a5)+            ;049D 00000002
          subi   #LATE_0001, (a6)+          ;045E 0001
          subi   #LATE_FF00, (a7)+          ;045F 00FF

          subi.b #$01, -(a0)              ;0420 0001  - 5320
          subi.b #$02, -(a1)              ;0421 0002  - 5521
          subi.w #$FE, -(a2)              ;0462 00FE
          subi.w #$FF, -(a3)              ;0463 00FF
          subi.l #LATE_01, -(a4)          ;04A4 00000001  - 5324
          subi.l #LATE_02, -(a5)          ;04A5 00000002  - 5525
          subi   #LATE_FE, -(a6)          ;0466 00FE
          subi   #LATE_FF, -(a7)          ;0467 00FF

          subq.b #1, ($0001).w            ;5338 0001
          subq.b #2, ($00FF).w            ;5538 00FF
          subq.b #3, ($0100).l            ;5739 00000100
          subq.b #4, ($FF00)              ;5939 0000FF00
          subq.b #5, (LATE_0001).w        ;5B38 0001
          subq.b #6, (LATE_00FF).l        ;5D39 000000FF
          subq.b #7, (LATE_0100).l        ;5F39 00000100
          subq.b #8, (LATE_FF00)          ;5139 0000FF00

          subq.w #1, ($0001).w            ;5378 0001
          subq.w #2, ($00FF).w            ;5578 00FF
          subq.w #3, ($0100).l            ;5779 00000100
          subq.w #4, ($FF00)              ;5979 0000FF00
          subq.w #5, (LATE_0001).w        ;5B78 0001
          subq.w #6, (LATE_00FF).l        ;5D79 000000FF
          subq.w #7, (LATE_0100).l        ;5F79 00000100
          subq.w #8, (LATE_FF00)          ;5179 0000FF00

          subq.l #1, ($0001).w            ;53B8 0001
          subq.l #2, ($00FF).w            ;55B8 00FF
          subq.l #3, ($0100).l            ;57B9 00000100
          subq.l #4, ($FF00)              ;59B9 0000FF00
          subq.l #5, (LATE_0001).w        ;5BB8 0001
          subq.l #6, (LATE_00FF).l        ;5DB9 000000FF
          subq.l #7, (LATE_0100).l        ;5FB9 00000100
          subq.l #8, (LATE_FF00)          ;51B9 0000FF00

          subq   #1, ($0001).w            ;5378 0001
          subq   #2, ($00FF).w            ;5578 00FF
          subq   #3, ($0100).l            ;5779 00000100
          subq   #4, ($FF00)              ;5979 0000FF00
          subq   #5, (LATE_0001).w        ;5B78 0001
          subq   #6, (LATE_00FF).l        ;5D79 000000FF
          subq   #7, (LATE_0100).l        ;5F79 00000100
          subq   #8, (LATE_FF00)          ;5179 0000FF00


          subq.b #1, d7                   ;5307
          subq.b #2, d6                   ;5506
          subq.w #3, d5                   ;5745
          subq.w #4, d4                   ;5944
          subq.l #5, d3                   ;5B83
          subq.l #6, d2                   ;5D82
          subq   #7, d1                   ;5F41
          subq   #8, d0                   ;5140

          subq.b #1, (a7)                 ;5317
          subq.b #2, (a6)                 ;5516
          subq.w #3, (a5)                 ;5755
          subq.w #4, (a4)                 ;5954
          subq.l #5, (a3)                 ;5B93
          subq.l #6, (a2)                 ;5D92
          subq   #7, (a1)                 ;5F51
          subq   #8, (a0)                 ;5150

          subq.b #1, $01(a7)              ;532F 0001
          subq.b #2, $02(a6)              ;552E 0002
          subq.w #3, $FE(a5)              ;576D 00FE
          subq.w #4, $FF(a4)              ;596C 00FF
          subq.l #5, LATE_01(a3)          ;5BAB 0001
          subq.l #6, LATE_02(a2)          ;5DAA 0002
          subq   #7, LATE_FE(a1)          ;5F69 00FE
          subq   #8, LATE_FF(a0)          ;5168 00FF

          subq.b #1, (a7)+                ;531F
          subq.b #2, (a6)+                ;551E
          subq.w #3, (a5)+                ;575D
          subq.w #4, (a4)+                ;595C
          subq.l #5, (a3)+                ;5B9B
          subq.l #6, (a2)+                ;5D9A
          subq   #7, (a1)+                ;5F59
          subq   #8, (a0)+                ;5158

          subq.b #1, -(a7)                ;5327
          subq.b #2, -(a6)                ;5526
          subq.w #3, -(a5)                ;5765
          subq.w #4, -(a4)                ;5964
          subq.l #5, -(a3)                ;5BA3
          subq.l #6, -(a2)                ;5DA2
          subq   #7, -(a1)                ;5F61
          subq   #8, -(a0)                ;5160

          subq.b #1, a7                   ;530F
          subq.b #2, a6                   ;550E
          subq.w #3, a5                   ;574D
          subq.w #4, a4                   ;594C
          subq.l #5, a3                   ;5B8B
          subq.l #6, a2                   ;5D8A
          subq   #7, a1                   ;5F49
          subq   #8, a0                   ;5148


          sub.b d0,d7             ;9E00
          sub.b d1,d6             ;9C01
          sub.w d2,d5             ;9A42
          sub.w d3,d4             ;9843
          sub.l d4,d3             ;9684
          sub.l d5,d2             ;9485
          sub   d6,d1             ;9246
          sub   d7,d0             ;9047

          sub.b ($0001).w, d0     ;9038 0001
          sub.b ($00FF).w, d1     ;9238 00FF
          sub.w ($0100).w, d2     ;9478 0100
          sub.w ($FF00).w, d3     ;9678 FF00
          sub.l (LATE_0001).w, d4 ;98B8 0001
          sub.l (LATE_00FF).w, d5 ;9AB8 00FF
          sub   (LATE_0100).w, d6 ;9C78 0100
          sub   (LATE_FF00).w, d7 ;9E78 FF00

          sub.b ($0001).l, d0     ;9039 00000001
          sub.b ($00FF).l, d1     ;9239 000000FF
          sub.w ($0100).l, d2     ;9479 00000100
          sub.w ($FF00).l, d3     ;9679 0000FF00
          sub.l (LATE_0001).l, d4 ;98B9 00000001
          sub.l (LATE_00FF).l, d5 ;9AB9 000000FF
          sub   (LATE_0100).l, d6 ;9CB9 00000100
          sub   (LATE_FF00).l, d7 ;9EB9 0000FF00

          sub.b ($0001), d0       ;9038 0001
          sub.b ($00FF), d1       ;9238 00FF
          sub.w ($0100), d2       ;9478 0100
          sub.w ($FF00), d3       ;9679 0000FF00
          sub.l (LATE_0001), d4   ;98B9 00000001
          sub.l (LATE_00FF), d5   ;9AB9 000000FF
          sub   (LATE_0100), d6   ;9CB9 00000100
          sub   (LATE_FF00), d7   ;9EB9 0000FF00


          sub.b (a7), d0          ;9017
          sub.b (a6), d1          ;9216
          sub.w (a5), d2          ;9455
          sub.w (a4), d3          ;9654
          sub.l (a3), d4          ;9893
          sub.l (a2), d5          ;9A92
          sub   (a1), d6          ;9C51
          sub   (a0), d7          ;9E50

          sub.b $01(a0),d7        ;9E28 0001
          sub.b $02(a1),d6        ;9C29 0002
          sub.w $FE(a2),d5        ;9A6A 00FE
          sub.w $FF(a3),d4        ;986B 00FF
          sub.l LATE_01(a4),d3    ;96AC 0001
          sub.l LATE_02(a5),d2    ;94AD 0002
          sub   LATE_FE(a6),d1    ;926E 00FE
          sub   LATE_FF(a7),d0    ;906F 00FF

          sub.b (a0)+,d7          ;9E18
          sub.b (a1)+,d6          ;9C19
          sub.w (a2)+,d5          ;9A5A
          sub.w (a3)+,d4          ;985B
          sub.l (a4)+,d3          ;969C
          sub.l (a5)+,d2          ;949D
          sub   (a6)+,d1          ;925E
          sub   (a7)+,d0          ;905F

          sub.b -(a0),d7          ;9E20
          sub.b -(a1),d6          ;9C21
          sub.w -(a2),d5          ;9A62
          sub.w -(a3),d4          ;9863
          sub.l -(a4),d3          ;96A4
          sub.l -(a5),d2          ;94A5
          sub   -(a6),d1          ;9266
          sub   -(a7),d0          ;9067

          sub.w a0,d7             ;9E48
          sub.w a1,d6             ;9C49
          sub.w a2,d5             ;9A4A
          sub.w a3,d4             ;984B
          sub.l a4,d3             ;968C
          sub.l a5,d2             ;948D
          sub   a6,d1             ;924E
          sub   a7,d0             ;904F

          sub.b d0, ($0001).w     ;9138 0001
          sub.b d1, ($00FF).w     ;9338 00FF
          sub.w d2, ($0100).w     ;9578 0100
          sub.w d3, ($FF00).w     ;9778 FF00
          sub.l d4, (LATE_0001).w ;99B8 0001
          sub.l d5, (LATE_00FF).w ;9BB8 00FF
          sub   d6, (LATE_0100).w ;9D78 0100
          sub   d7, (LATE_FF00).w ;9F78 FF00

          sub.b d0, ($0001).l     ;9139 00000001
          sub.b d1, ($00FF).l     ;9339 000000FF
          sub.w d2, ($0100).l     ;9579 00000100
          sub.w d3, ($FF00).l     ;9779 0000FF00
          sub.l d4, (LATE_0001).l ;99B9 00000001
          sub.l d5, (LATE_00FF).l ;9BB9 000000FF
          sub   d6, (LATE_0100).l ;9D79 00000100
          sub   d7, (LATE_FF00).l ;9F79 0000FF00

          sub.b d0, ($0001)       ;9139 00000001
          sub.b d1, ($00FF)       ;9339 000000FF
          sub.w d2, ($0100)       ;9579 00000100
          sub.w d3, ($FF00)       ;9779 0000FF00
          sub.l d4, (LATE_0001)   ;99B9 00000001
          sub.l d5, (LATE_00FF)   ;9BB9 000000FF
          sub   d6, (LATE_0100)   ;9D79 00000100
          sub   d7, (LATE_FF00)   ;9F79 0000FF00

          sub.b d0,(a7)           ;9117
          sub.b d1,(a6)           ;9316
          sub.w d2,(a5)           ;9555
          sub.w d3,(a4)           ;9754
          sub.l d4,(a3)           ;9993
          sub.l d5,(a2)           ;9B92
          sub   d6,(a1)           ;9D51
          sub   d7,(a0)           ;9F50

          sub.b d0, $01(a7)       ;912F 0001
          sub.b d1, $02(a6)       ;932E 0002
          sub.w d2, $FE(a5)       ;956D 00FE
          sub.w d3, $FF(a4)       ;976C 00FF
          sub.l d4, LATE_01(a3)   ;99AB 0001
          sub.l d5, LATE_02(a2)   ;9BAA 0002
          sub   d6, LATE_FE(a1)   ;9D69 00FE
          sub   d7, LATE_FF(a0)   ;9F68 00FF

          sub.b d0,(a7)+          ;911F
          sub.b d1,(a6)+          ;931E
          sub.w d2,(a5)+          ;955D
          sub.w d3,(a4)+          ;975C
          sub.l d4,(a3)+          ;999B
          sub.l d5,(a2)+          ;9B9A
          sub   d6,(a1)+          ;9D59
          sub   d7,(a0)+          ;9F58

          sub.b d0,-(a7)          ;9127
          sub.b d1,-(a6)          ;9326
          sub.w d2,-(a5)          ;9565
          sub.w d3,-(a4)          ;9764
          sub.l d4,-(a3)          ;99A3
          sub.l d5,-(a2)          ;9BA2
          sub   d6,-(a1)          ;9D61
          sub   d7,-(a0)          ;9F60

          swap d0                 ;48 40
          swap d1                 ;48 41
          swap d2                 ;48 42
          swap d3                 ;48 43
          swap d4                 ;48 44
          swap d5                 ;48 45
          swap d6                 ;48 46
          swap d7                 ;48 47

          tas.b ($0001).w         ;4AF8 0001
          tas.b ($00FF).w         ;4AF8 00FF
          tas.b ($0100).l         ;4AF9 00000100
          tas.b ($FF00)           ;4AF9 0000FF00
          tas   (LATE_0001).w     ;4AF8 0001
          tas   (LATE_00FF).w     ;4AF8 00FF
          tas   (LATE_0100).l     ;4AF9 00000100
          tas   (LATE_FF00)       ;4AF9 0000FF00

          tas.b d0                ;4AC0
          tas.b d1                ;4AC1
          tas.b d2                ;4AC2
          tas.b d3                ;4AC3
          tas   d4                ;4AC4
          tas   d5                ;4AC5
          tas   d6                ;4AC6
          tas   d7                ;4AC7

          tas.b (a0)              ;4AD0
          tas.b (a1)              ;4AD1
          tas.b (a2)              ;4AD2
          tas.b (a3)              ;4AD3
          tas   (a4)              ;4AD4
          tas   (a5)              ;4AD5
          tas   (a6)              ;4AD6
          tas   (a7)              ;4AD7

          tas.b $01(a7)           ;4AEF 0001
          tas.b $02(a6)           ;4AEE 0002
          tas.b $FE(a5)           ;4AED 00FE
          tas.b $FF(a4)           ;4AEC 00FF
          tas   LATE_01(a3)       ;4AEB 0001
          tas   LATE_02(a2)       ;4AEA 0002
          tas   LATE_FE(a1)       ;4AE9 00FE
          tas   LATE_FF(a0)       ;4AE8 00FF

          trap #1                 ;4E 47
          trap #2                 ;4E 46
          trap #3                 ;4E 45
          trap #4                 ;4E 44
          trap #5                 ;4E 43
          trap #6                 ;4E 42
          trap #7                 ;4E 41
          trap #8                 ;4E 40

          trapv                   ;4E 76

          tst.b d0                ;4A00
          tst.b d1                ;4A01
          tst.w d2                ;4A42
          tst.w d3                ;4A43
          tst.l d4                ;4A84
          tst.l d5                ;4A85
          tst   d6                ;4A46
          tst   d7                ;4A47

          tst.b ($0001).w         ;4A38 0001
          tst.b ($00FF).w         ;4A38 00FF
          tst.b ($0100).l         ;4A39 00000100
          tst.b ($FF00)           ;4A39 0000FF00
          tst.b (LATE_0001).w     ;4A38 0001
          tst.b (LATE_00FF).w     ;4A38 00FF
          tst.b (LATE_0100).l     ;4A39 00000100
          tst.b (LATE_FF00)       ;4A39 0000FF00

          tst.w ($0001).w         ;4A78 0001
          tst.w ($00FF).w         ;4A78 00FF
          tst.w ($0100).l         ;4A79 00000100
          tst.w ($FF00)           ;4A79 0000FF00
          tst.w (LATE_0001).w     ;4A78 0001
          tst.w (LATE_00FF).w     ;4A78 00FF
          tst.w (LATE_0100).l     ;4A79 00000100
          tst.w (LATE_FF00)       ;4A79 0000FF00

          tst.l ($0001).w         ;4AB8 0001
          tst.l ($00FF).w         ;4AB8 00FF
          tst.l ($0100).l         ;4AB9 00000100
          tst.l ($FF00)           ;4AB9 0000FF00
          tst.l (LATE_0001).w     ;4AB8 0001
          tst.l (LATE_00FF).w     ;4AB8 00FF
          tst.l (LATE_0100).l     ;4AB9 00000100
          tst.l (LATE_FF00)       ;4AB9 0000FF00

          tst ($0001).w           ;4A78 0001
          tst ($00FF).w           ;4A78 00FF
          tst ($0100).l           ;4A79 00000100
          tst ($FF00)             ;4A79 0000FF00
          tst (LATE_0001).w       ;4A78 0001
          tst (LATE_00FF).w       ;4A78 00FF
          tst (LATE_0100).l       ;4A79 00000100
          tst (LATE_FF00)         ;4A79 0000FF00


          tst.b (a0)              ;4A10
          tst.b (a1)              ;4A11
          tst.w (a2)              ;4A52
          tst.w (a3)              ;4A53
          tst.l (a4)              ;4A94
          tst.l (a5)              ;4A95
          tst   (a6)              ;4A56
          tst   (a7)              ;4A57

          tst.b $01(a0)           ;4A28 0001
          tst.w $02(a1)           ;4A69 0002
          tst.l $FE(a2)           ;4AAA 00FE
          tst   $FF(a3)           ;4A6B 00FF
          tst.b LATE_01(a4)       ;4A2C 0001
          tst.w LATE_02(a5)       ;4A6D 0002
          tst.l LATE_FE(a6)       ;4AAE 00FE
          tst   LATE_FF(a7)       ;4A6F 00FF

          unlk a0                 ;4E 58
          unlk a1                 ;4E 59
          unlk a2                 ;4E 5A
          unlk a3                 ;4E 5B
          unlk a4                 ;4E 5C
          unlk a5                 ;4E 5D
          unlk a6                 ;4E 5E
          unlk a7                 ;4E 5F

LATE_01 = $01
LATE_02 = $02
LATE_7E = $7E
LATE_7F = $7F
LATE_FE = $FE
LATE_FF = $FF

LATE_0001 = $0001
LATE_00FF = $00FF
LATE_0100 = $0100
LATE_7F00 = $7F00
LATE_FF00 = $FF00
LATE_000001 = $000001
LATE_0000FF = $0000FF
LATE_00FF00 = $00FF00
LATE_010000 = $010000
LATE_FF0000 = $FF0000
LATE_123456 = $123456

LATE_00000001 = $00000001
LATE_0000FF00 = $0000FF00
LATE_00010000 = $00010000
LATE_FF000000 = $FF000000
LATE_00FF0000 = $00FF0000
