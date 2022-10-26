!cpu W65C02

!src <commanderx16.asm>


VIDEO_RAM = $01b000

* = $0801

!basic

          lda #( VIDEO_RAM >> 16 ) | $10
          sta VERA.ADDRx_H

          lda #( VIDEO_RAM >> 8 ) & $ff
          sta VERA.ADDRx_M
          lda #VIDEO_RAM & $ff
          sta VERA.ADDRx_L

          ;set DC sel
          lda #2
          sta VERA.CTRL
          stz VERA.DC_VSTART
          lda #( 480 / 2 )
          sta VERA.DC_VSTOP

          stz VERA.DC_HSTART
          lda #( 640 >> 2 )
          sta VERA.DC_HSTOP

          ;clear DC sel
          stz VERA.CTRL

          ; text mode: disable layer 0
          lda VERA.DC_VIDEO
          and #$ef
          sta VERA.DC_VIDEO
          lda #6 << 4 | 1 ; blue on white
          ;sta where?

          ;map width/height?
          ;'Map Width', 'Map Height' specify the dimensions of the tile map:
          ;Value Map width / height
          ;0 32 tiles
          ;1 64 tiles
          ;2 128 tiles
          ;3 256 tiles
          ;'Tile Width', 'Tile Height' specify the dimensions of a singl
          ;Color Depth Description
          ;0 1 bpp
          ;1 2 bpp
          ;2 4 bpp
          ;3 8 bpp
          ;VERA_CONFIG_MAP_WIDTH_64 | VERA_CONFIG_MAP_HEIGHT_32 | VERA_CONFIG_4BPP;
          lda #$00
          sta VERA.L0_CONFIG

;from screen.s (ROM)
; mode        $00: 80x60
;             $01: 80x30
;             $02: 40x60
;             $03: 40x30
;             $04: 40x15
;             $05: 20x30
;             $06: 20x15
;             $80: 320x240@256c + 40x30 text
;             $81: 640x400@16c ; XXX currently unsupported
;modes:  .byte   0,   1,   2,   3,   4,   5,   6, $80
;scale:  .byte $00, $01, $10, $11, $12, $21, $22, $11 ; hi-nyb: x >> n, lo-nyb: y >> n

          ;scaling 1:1
          lda #128 ;128
          sta VERA.DC_HSCALE
          ;scaling 1:1
          lda #64 ;128
          sta VERA.DC_VSCALE

          lda #80
          sta $d9 ;llen  .res 1           ;$D9 x resolution
          lda #30
          sta $da ;nlines  .res 1           ;$DA y resolution
          lda #31
          sta $db ;nlinesp1 .res 1          ;    X16: y resolution + 1
          lda #29
          sta $de ;nlinesm1 .res 1          ;    X16: y resolution - 1


          ldy #0
--
          ldx #0
-
.ReadPos = * + 1
          lda SCREEN_DATA,x
          sta VERA.DATA0

          inx
          cpx #80 * 2
          bne -

          ;jump to next line
          inc VERA.ADDRx_M
          stz VERA.ADDRx_L

          ;update source data pointer
          lda .ReadPos
          clc
          adc #80 * 2
          sta .ReadPos
          bcc +
          inc .ReadPos + 1
+
          iny
          cpy #30
          bne --

          rts


SCREEN_DATA
          !media "textscreen80x30.charscreen",CHARCOLORINTERLEAVED
