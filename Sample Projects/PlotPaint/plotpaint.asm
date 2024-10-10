!src <c64.asm>

SCREEN_COLOR  = $d800
SCREEN_CHAR   = $0400

ZEROPAGE_POINTER_1    = $fc
ZEROPAGE_POINTER_2    = $fe

* = $0801
!basic
          lda #0
          sta VIC.BORDER_COLOR
          lda #5
          sta VIC.BACKGROUND_COLOR

          ldx #0
-
          lda #1
          sta SCREEN_COLOR + 0 * 250,x
          sta SCREEN_COLOR + 1 * 250,x
          sta SCREEN_COLOR + 2 * 250,x
          sta SCREEN_COLOR + 3 * 250,x

          lda #32
          sta SCREEN_CHAR + 0 * 250,x
          sta SCREEN_CHAR + 1 * 250,x
          sta SCREEN_CHAR + 2 * 250,x
          sta SCREEN_CHAR + 3 * 250,x


          inx
          cpx #250
          bne -


          jmp PrepareLoop


* = $0840
SPRITE_DATA
          !media "plotpaint.spriteproject",SPRITE,0,1



!lzone PrepareLoop
          lda #24
          sta VIC.SPRITE_X_POS
          lda #50
          sta VIC.SPRITE_Y_POS
          lda #SPRITE_DATA / 64
          sta 2040
          lda #1
          sta VIC.SPRITE_COLOR
          sta VIC.SPRITE_ENABLE


!lzone PaintLoop
          lda #210
-
          cmp VIC.RASTER_POS
          bne -

          inc COLOR_POS
          lda COLOR_POS
          lsr
          lsr
          and #$07
          tay
          lda PAINT_MODE
          beq .Draw
          lda COLOR_FADE_ERASE,y
          jmp +
.Draw
          lda COLOR_FADE,y
+
          sta VIC.SPRITE_COLOR

          lda JOYSTICK_PORT_II
          sta JOY_VALUE

          ;fire released?
          and #$10
          beq +

          lda #1
          sta BUTTON_RELEASED

+
          lda #0
          sta MOVED
          lda MOVE_DELAY
          beq +
          dec MOVE_DELAY
+

          lda MOVE_DELAY
          beq .CanMove
          jmp .MoveBlocked
.CanMove

          lda JOY_VALUE
          and #$04
          bne .NotLeft

          lda CURSOR_X
          beq .NotLeft

          dec CURSOR_X
          lda VIC.SPRITE_X_POS
          sec
          sbc #4
          sta VIC.SPRITE_X_POS
          bcs +
          lda #0
          sta VIC.SPRITE_X_EXTEND

+
          inc MOVED

.NotLeft

          lda JOY_VALUE
          and #$08
          bne .NotRight

          lda CURSOR_X
          cmp #79
          beq .NotRight

          inc CURSOR_X
          lda VIC.SPRITE_X_POS
          clc
          adc #4
          sta VIC.SPRITE_X_POS
          bne +
          lda #1
          sta VIC.SPRITE_X_EXTEND

+
          inc MOVED

.NotRight

          lda JOY_VALUE
          and #$01
          bne .NotUp

          lda CURSOR_Y
          beq .NotUp

          dec CURSOR_Y
          lda VIC.SPRITE_Y_POS
          sec
          sbc #4
          sta VIC.SPRITE_Y_POS
          inc MOVED

.NotUp

          lda JOY_VALUE
          and #$02
          bne .NotDown

          lda CURSOR_Y
          cmp #49
          beq .NotDown

          inc CURSOR_Y
          lda VIC.SPRITE_Y_POS
          clc
          adc #4
          sta VIC.SPRITE_Y_POS
          inc MOVED

.NotDown

          lda MOVED
          beq .NotMoved

          lda MOVE_DELAY_MAX
          sta MOVE_DELAY


.NotMoved

.MoveBlocked
          lda JOY_VALUE
          and #$10
          bne .NotFire

          lda #0
          sta BUTTON_RELEASED

          jsr Plot

.NotFire
          jsr KERNAL.GETIN
          beq .NoKey

          cmp #133    ;f1
          bne +

          inc VIC.BORDER_COLOR
          jmp PaintLoop
+

          cmp #134    ;f3
          bne +

          inc VIC.BACKGROUND_COLOR
          jmp PaintLoop
+
          cmp #135    ;f5
          bne +

          inc PAINT_COLOR
          jmp PaintLoop
+
          cmp #136    ;f7
          bne +

          lda PAINT_MODE
          eor #$01
          sta PAINT_MODE
          jmp PaintLoop
+
          cmp #48
          bcc +
          cmp #56
          bcs +

          sec
          sbc #48
          sta MOVE_DELAY_MAX

+


.NoKey
          jmp PaintLoop



!lzone Plot
          lda CURSOR_Y
          lsr
          tay
          lda SCREEN_LO,y
          sta ZEROPAGE_POINTER_1
          lda SCREEN_HI,y
          sta ZEROPAGE_POINTER_1 + 1

          lda CURSOR_X
          lsr
          tay

          lda CURSOR_X
          and #$01
          sta .DX
          lda CURSOR_Y
          and #$01
          sta .DY

          ldx #0
          lda (ZEROPAGE_POINTER_1),y
-
          cmp PATTERN,x
          beq .FoundMatch

          inx
          cpx #COMPARE_TABLE_MAX
          bne -

          jmp PaintLoop


.FoundMatch
          stx .BITS
          lda .DX
          asl
          ora .DY
          tax

          lda PAINT_MODE
          beq .Draw

          ;erase
          lda BIT_TABLE,x
          eor #$ff
          and .BITS
          jmp .Erased


.Draw
          lda .BITS
          ora BIT_TABLE,x
.Erased
          tax

          lda PATTERN,x
          sta (ZEROPAGE_POINTER_1),y

          lda ZEROPAGE_POINTER_1
          sta ZEROPAGE_POINTER_2
          lda ZEROPAGE_POINTER_1 + 1
          clc
          adc #>( SCREEN_COLOR - SCREEN_CHAR )
          sta ZEROPAGE_POINTER_2 + 1

          lda PAINT_COLOR
          sta (ZEROPAGE_POINTER_2),y

          jmp PaintLoop

.BITS
          !byte 0

BIT_TABLE
          !byte 1,2,4,8

PATTERN
          !byte 32
          !byte 126
          !byte 123
          !byte 97
          !byte 124
          !byte 226
          !byte 255
          !byte 236

          !byte 108
          !byte 127
          !byte 98
          !byte 252
          !byte 225
          !byte 251
          !byte 254
          !byte 160

COMPARE_TABLE
          ;DX/DY 0/0
          !byte 32
          !byte 126
          !byte 123
          !byte 97
          !byte 124
          !byte 226
          !byte 32
          !byte 32

          ;DX/DY 0/1
          !byte 32
          !byte 32
          !byte 32
          !byte 32
          !byte 32
          !byte 32
          !byte 32
          !byte 32

          ;DX/DY 1/0
          !byte 32
          !byte 32
          !byte 32
          !byte 32
          !byte 32
          !byte 32
          !byte 32
          !byte 32

          ;DX/DY 1/1
          !byte 32
          !byte 32
          !byte 32
          !byte 32
          !byte 32
          !byte 32
          !byte 32
          !byte 32

COMPARE_TABLE_MAX = * - COMPARE_TABLE

.DX
          !byte 0

.DY
          !byte 0




CURSOR_X
          !byte 0
CURSOR_Y
          !byte 0

COLOR_FADE
          !byte 1,15,12,11,0,11,12,15
COLOR_FADE_ERASE
          !byte 10,10,2,2,0,2,2,10

COLOR_POS
          !byte 0

JOY_VALUE
          !byte 0

BUTTON_RELEASED
          !byte 0

PAINT_COLOR
          !byte 1

;0 = paint, 1 = erase
PAINT_MODE
          !byte 0

MOVED
          !byte 0
MOVE_DELAY
          !byte 0
MOVE_DELAY_MAX
          !byte 0

SCREEN_LO
          !fill 25,<( SCREEN_CHAR + i * 40 )

SCREEN_HI
          !fill 25,>( SCREEN_CHAR + i * 40 )