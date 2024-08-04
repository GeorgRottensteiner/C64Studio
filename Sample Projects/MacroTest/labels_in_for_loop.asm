* = $2000

Dungeon = 1024
SCREEN_MEMORY = $c000
SCREEN_COLOR = $d800


game_loop

!for i = 0 to 3
;TODO auto zone
;!zone

          ldx #250
-
          lda Dungeon + (250 * i - 1), x
          sta SCREEN_MEMORY + (250 * i - 1), x
          cmp #'w' ; Wall
          bne .next
          lda #8 ; BROWN
          jmp .end
.next
          lda #1 ; WHITE
.end
          sta SCREEN_COLOR + (250 * i - 1), x
          dex
          bne -
 !end

          jmp game_loop

!zone

.sowas

!byte 0

!zone

  .sowas2



;00  $2000              * = $2000
;01  $2000
;02  $2000              Dungeon = 1024
;03  $2000              SCREEN_MEMORY = $c000
;04  $2000              SCREEN_COLOR = $d800
;05  $2000
;06  $2000
;07  $2000              game_loop
;08  $2000
;09  $2000              ;was for loop start for i
;10  $2000              ;TODO auto zone; loop with i=0
;11  $2000              ;!zone; loop with i=0
;12  $2000              ; loop with i=0
;13  $2000  A2 FA       ldx #250; loop with i=0
;14  $2002              -; loop with i=0
;15  $2002  BD FF 03    lda Dungeon + (250 * i - 1), x; loop with i=0
;16  $2005  9D FF BF    sta SCREEN_MEMORY + (250 * i - 1), x; loop with i=0
;17  $2008  C9 77       cmp #'w' ; Wall; loop with i=0
;18  $200A  D0 05       bne ._C64STUDIO_LL_0__27_.next; loop with i=0
;19  $200C  A9 08       lda #8 ; BROWN; loop with i=0
;20  $200E  4C 13 20    jmp ._C64STUDIO_LL_0__27_.end; loop with i=0
;21  $2011              ._C64STUDIO_LL_0__27_.next; loop with i=0
;22  $2011  A9 01       lda #1 ; WHITE; loop with i=0
;23  $2013              ._C64STUDIO_LL_0__27_.end; loop with i=0
;24  $2013  9D FF D7    sta SCREEN_COLOR + (250 * i - 1), x; loop with i=0
;25  $2016  CA          dex; loop with i=0
;26  $2017  D0 E9       bne -; loop with i=0
;27  $2019              ;TODO auto zone; loop with i=1
;28  $2019              ;!zone; loop with i=1
;29  $2019              ; loop with i=1
;30  $2019  A2 FA       ldx #250; loop with i=1
;31  $201B              -; loop with i=1
;32  $201B  BD F9 04    lda Dungeon + (250 * i - 1), x; loop with i=1
;33  $201E  9D F9 C0    sta SCREEN_MEMORY + (250 * i - 1), x; loop with i=1
;34  $2021  C9 77       cmp #'w' ; Wall; loop with i=1
;35  $2023  D0 05       bne ._C64STUDIO_LL_0__27_.next; loop with i=1
;36  $2025  A9 08       lda #8 ; BROWN; loop with i=1
;37  $2027  4C 2C 20    jmp ._C64STUDIO_LL_0__27_.end; loop with i=1
;38  $202A              ._C64STUDIO_LL_0__27_.next; loop with i=1
;39  $202A  A9 01       lda #1 ; WHITE; loop with i=1
;40  $202C              ._C64STUDIO_LL_0__27_.end; loop with i=1
;41  $202C  9D F9 D8    sta SCREEN_COLOR + (250 * i - 1), x; loop with i=1
;42  $202F  CA          dex; loop with i=1
;43  $2030  D0 E9       bne -; loop with i=1
;44  $2032              ;TODO auto zone; loop with i=2
;45  $2032              ;!zone; loop with i=2
;46  $2032              ; loop with i=2
;47  $2032  A2 FA       ldx #250; loop with i=2
;48  $2034              -; loop with i=2
;49  $2034  BD F3 05    lda Dungeon + (250 * i - 1), x; loop with i=2
;50  $2037  9D F3 C1    sta SCREEN_MEMORY + (250 * i - 1), x; loop with i=2
;51  $203A  C9 77       cmp #'w' ; Wall; loop with i=2
;52  $203C  D0 05       bne ._C64STUDIO_LL_1__27_._C64STUDIO_LL_0__27_.next; loop with i=2
;53  $203E  A9 08       lda #8 ; BROWN; loop with i=2
;54  $2040  4C 45 20    jmp ._C64STUDIO_LL_1__27_._C64STUDIO_LL_0__27_.end; loop with i=2
;55  $2043              ._C64STUDIO_LL_1__27_._C64STUDIO_LL_0__27_.next; loop with i=2
;56  $2043  A9 01       lda #1 ; WHITE; loop with i=2
;57  $2045              ._C64STUDIO_LL_1__27_._C64STUDIO_LL_0__27_.end; loop with i=2
;58  $2045  9D F3 D9    sta SCREEN_COLOR + (250 * i - 1), x; loop with i=2
;59  $2048  CA          dex; loop with i=2
;60  $2049  D0 E9       bne -; loop with i=2
;61  $204B              ;TODO auto zone; loop with i=3
;62  $204B              ;!zone; loop with i=3
;63  $204B              ; loop with i=3
;64  $204B  A2 FA       ldx #250; loop with i=3
;65  $204D              -; loop with i=3
;66  $204D  BD ED 06    lda Dungeon + (250 * i - 1), x; loop with i=3
;67  $2050  9D ED C2    sta SCREEN_MEMORY + (250 * i - 1), x; loop with i=3
;68  $2053  C9 77       cmp #'w' ; Wall; loop with i=3
;69  $2055  D0 05       bne ._C64STUDIO_LL_2__44_._C64STUDIO_LL_1__27_._C64STUDIO_LL_0__27_.next; loop with i=3
;70  $2057  A9 08       lda #8 ; BROWN; loop with i=3
;71  $2059  4C 5E 20    jmp ._C64STUDIO_LL_2__44_._C64STUDIO_LL_1__27_._C64STUDIO_LL_0__27_.end; loop with i=3
;72  $205C              ._C64STUDIO_LL_2__44_._C64STUDIO_LL_1__27_._C64STUDIO_LL_0__27_.next; loop with i=3
;73  $205C  A9 01       lda #1 ; WHITE; loop with i=3
;74  $205E              ._C64STUDIO_LL_2__44_._C64STUDIO_LL_1__27_._C64STUDIO_LL_0__27_.end; loop with i=3
;75  $205E  9D ED DA    sta SCREEN_COLOR + (250 * i - 1), x; loop with i=3
;76  $2061  CA          dex; loop with i=3
;77  $2062  D0 E9       bne -; loop with i=3
;78  $2064              ;was loop end for i
;79  $2064
;80  $2064  4C 00 20    jmp game_loop
;81  $2067
;82  $2067              !zone
;83  $2067
;84  $2067              .sowas
;85  $2067
;86  $2067  00          !byte 0
;87  $2068
;88  $2068              !zone
;89  $2068
;90  $2068              .sowas2
