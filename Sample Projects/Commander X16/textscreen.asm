!cpu W65C02

!src <commanderx16.asm>


VIDEO_RAM = $01b000 ;$0f800

* = $0801

!basic

          lda #( VIDEO_RAM >> 16 ) | $10
          sta VERA.ADDRx_H

          lda #( VIDEO_RAM >> 8 ) & $ff
          sta VERA.ADDRx_M
          lda #VIDEO_RAM & $ff
          sta VERA.ADDRx_L

          lda VERA.CTRL
          and #$fe
          sta VERA.CTRL

          lda #2
          sta VERA.DATA0
          lda #5
          sta VERA.DATA0
          lda #3
          sta VERA.DATA0
          lda #$f6
          sta VERA.DATA0

          rts
