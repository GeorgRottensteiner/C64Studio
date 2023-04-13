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