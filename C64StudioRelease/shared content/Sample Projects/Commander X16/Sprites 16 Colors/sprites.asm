!cpu W65C02

!src <commanderx16.asm>

SPRITE_DATA_VRAM    = $004000


* = $0801

!basic
          stz VERA.CTRL

          ;enable sprites
          lda VERA.DC_VIDEO
          ora #VERA.DC_VIDEO_SPRITES_ENABLE
          sta VERA.DC_VIDEO

          ;set up sprite graphic data
          lda #( SPRITE_DATA_VRAM >> 16 ) | VERA.ADDRESS_INCREASE_1
          sta VERA.ADDRx_H
          lda #( SPRITE_DATA_VRAM >> 8 ) & $ff
          sta VERA.ADDRx_M
          lda #SPRITE_DATA_VRAM & $ff
          sta VERA.ADDRx_L

          ;copy sprite data to VRAM
          ldx #0
-
          lda SPRITE_DATA_8_8_4BPP,x
          sta VERA.DATA0

          inx
          cpx #32
          bne -

          ;set up sprite graphic data
          lda #( ( SPRITE_DATA_VRAM + 32 ) >> 16 ) | VERA.ADDRESS_INCREASE_1
          sta VERA.ADDRx_H
          lda #( ( SPRITE_DATA_VRAM + 32 ) >> 8 ) & $ff
          sta VERA.ADDRx_M
          lda #( SPRITE_DATA_VRAM + 32 ) & $ff
          sta VERA.ADDRx_L

          ;copy sprite data to VRAM
          ldx #0
-
          lda SPRITE_DATA_16_16_4BPP,x
          sta VERA.DATA0

          inx
          cpx #128
          bne -

          ;set up sprite graphic data
          lda #( ( SPRITE_DATA_VRAM + 32 + 128 ) >> 16 ) | VERA.ADDRESS_INCREASE_1
          sta VERA.ADDRx_H
          lda #( ( SPRITE_DATA_VRAM + 32 + 128 ) >> 8 ) & $ff
          sta VERA.ADDRx_M
          lda #( SPRITE_DATA_VRAM + 32 + 128 ) & $ff
          sta VERA.ADDRx_L

          ;copy sprite data to VRAM
          ldx #0
          ldy #0
-
.ReadPos = * + 1
          lda SPRITE_DATA_32_32_4BPP,x
          sta VERA.DATA0

          inx
          bne -

          inc .ReadPos + 1
          iny
          cpy #2
          bne -

          ;set up sprite graphic data
          lda #( ( SPRITE_DATA_VRAM + 32 + 128 + 512 ) >> 16 ) | VERA.ADDRESS_INCREASE_1
          sta VERA.ADDRx_H
          lda #( ( SPRITE_DATA_VRAM + 32 + 128 + 512 ) >> 8 ) & $ff
          sta VERA.ADDRx_M
          lda #( SPRITE_DATA_VRAM + 32 + 128 + 512 ) & $ff
          sta VERA.ADDRx_L

          ;copy sprite data to VRAM
          ldx #0
          ldy #0
-
.ReadPos = * + 1
          lda SPRITE_DATA_64_64_4BPP,x
          sta VERA.DATA0

          inx
          bne -

          inc .ReadPos + 1
          iny
          cpy #8
          bne -


          ;set up sprite 0 attributes
          lda #( VERA.VRAM_SPRITE_ATTRIBUTES >> 16 ) | VERA.ADDRESS_INCREASE_1
          sta VERA.ADDRx_H
          lda #( VERA.VRAM_SPRITE_ATTRIBUTES >> 8 ) & $ff
          sta VERA.ADDRx_M
          lda #VERA.VRAM_SPRITE_ATTRIBUTES & $ff
          sta VERA.ADDRx_L

          ;pointer to sprite data
          lda #<( SPRITE_DATA_VRAM >> 5 )
          sta VERA.DATA0
          lda #>( SPRITE_DATA_VRAM >> 5 )
          sta VERA.DATA0

          ;x
          lda #100
          sta VERA.DATA0
          lda #0
          sta VERA.DATA0

          ;y
          lda #120
          sta VERA.DATA0
          lda #0
          sta VERA.DATA0

          lda #VERA.SPRITE_IN_FRONT_OF_LAYER_1
          sta VERA.DATA0

          lda #VERA.SPRITE_WIDTH_8 | VERA.SPRITE_HEIGHT_8 | VERA.SPRITE_PALETTE_OFFSET_0
          sta VERA.DATA0


          ;set up sprite 1 attributes
          lda #( ( VERA.VRAM_SPRITE_ATTRIBUTES + 8 ) >> 16 ) | VERA.ADDRESS_INCREASE_1
          sta VERA.ADDRx_H
          lda #( ( VERA.VRAM_SPRITE_ATTRIBUTES + 8 ) >> 8 ) & $ff
          sta VERA.ADDRx_M
          lda #( VERA.VRAM_SPRITE_ATTRIBUTES + 8 ) & $ff
          sta VERA.ADDRx_L

          ;pointer to sprite data
          lda #<( ( SPRITE_DATA_VRAM + 32 ) >> 5 )
          sta VERA.DATA0
          lda #>( ( SPRITE_DATA_VRAM + 32 ) >> 5 )
          sta VERA.DATA0

          ;x
          lda #120
          sta VERA.DATA0
          lda #0
          sta VERA.DATA0

          ;y
          lda #120
          sta VERA.DATA0
          lda #0
          sta VERA.DATA0

          lda #VERA.SPRITE_IN_FRONT_OF_LAYER_1 | VERA.SPRITE_MODE_4BPP
          sta VERA.DATA0

          lda #VERA.SPRITE_WIDTH_16 | VERA.SPRITE_HEIGHT_16 | VERA.SPRITE_PALETTE_OFFSET_0
          sta VERA.DATA0

          ;set up sprite 2 attributes
          lda #( ( VERA.VRAM_SPRITE_ATTRIBUTES + 16 ) >> 16 ) | VERA.ADDRESS_INCREASE_1
          sta VERA.ADDRx_H
          lda #( ( VERA.VRAM_SPRITE_ATTRIBUTES + 16 ) >> 8 ) & $ff
          sta VERA.ADDRx_M
          lda #( VERA.VRAM_SPRITE_ATTRIBUTES + 16 ) & $ff
          sta VERA.ADDRx_L

          ;pointer to sprite data
          lda #<( ( SPRITE_DATA_VRAM + 32 + 128 ) >> 5 )
          sta VERA.DATA0
          lda #>( ( SPRITE_DATA_VRAM + 32 + 128 ) >> 5 )
          sta VERA.DATA0

          ;x
          lda #160
          sta VERA.DATA0
          lda #0
          sta VERA.DATA0

          ;y
          lda #120
          sta VERA.DATA0
          lda #0
          sta VERA.DATA0

          lda #VERA.SPRITE_IN_FRONT_OF_LAYER_1 | VERA.SPRITE_MODE_4BPP
          sta VERA.DATA0

          lda #VERA.SPRITE_WIDTH_32 | VERA.SPRITE_HEIGHT_32 | VERA.SPRITE_PALETTE_OFFSET_0
          sta VERA.DATA0

          ;set up sprite 3 attributes
          lda #( ( VERA.VRAM_SPRITE_ATTRIBUTES + 24 ) >> 16 ) | VERA.ADDRESS_INCREASE_1
          sta VERA.ADDRx_H
          lda #( ( VERA.VRAM_SPRITE_ATTRIBUTES + 24 ) >> 8 ) & $ff
          sta VERA.ADDRx_M
          lda #( VERA.VRAM_SPRITE_ATTRIBUTES + 24 ) & $ff
          sta VERA.ADDRx_L

          ;pointer to sprite data
          lda #<( ( SPRITE_DATA_VRAM + 32 + 128 + 512 ) >> 5 )
          sta VERA.DATA0
          lda #>( ( SPRITE_DATA_VRAM + 32 + 128 + 512 ) >> 5 )
          sta VERA.DATA0

          ;x
          lda #210
          sta VERA.DATA0
          lda #0
          sta VERA.DATA0

          ;y
          lda #120
          sta VERA.DATA0
          lda #0
          sta VERA.DATA0

          lda #VERA.SPRITE_IN_FRONT_OF_LAYER_1 | VERA.SPRITE_MODE_4BPP
          sta VERA.DATA0

          lda #VERA.SPRITE_WIDTH_64 | VERA.SPRITE_HEIGHT_64 | VERA.SPRITE_PALETTE_OFFSET_0
          sta VERA.DATA0

          rts



SPRITE_DATA_8_8_4BPP
          !media "8x8x16.spriteproject",SPRITE,0,1

SPRITE_DATA_16_16_4BPP
          !media "16x16x16.spriteproject",SPRITE,0,1

SPRITE_DATA_32_32_4BPP
          !media "32x32x16.spriteproject",SPRITE,0,1

SPRITE_DATA_64_64_4BPP
          !media "64x64x16.spriteproject",SPRITE,0,1
