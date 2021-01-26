* = $01F8
          ; overwrite stack
          !word Start - 1

Start          
          ;clear screen
          jsr $E544
          
---          
          ldx #3
--          
          lda DELTA_LEN,x
          tay
          sec
          sbc #4
          sta DELTA_LEN,x
          
-          
          lda #128
.DrawPos = * + 1    
          sta $0400 - 2
          
          ;inc pos
          lda DELTA_LO,x
          clc
          adc .DrawPos
          sta .DrawPos
          lda DELTA_HI,x
          adc .DrawPos + 1
          sta .DrawPos + 1
          
          dey
EndlessLoop          
          bmi EndlessLoop
          bne -
          
          dex
          bpl --
          bmi ---
          
 
DELTA_LO
          !byte 256 - 40, 256 - 1, 40, 1
          
          ;ROM has $ff,$ff,$00,$00
DELTA_HI = $ecb7
          ;!byte 255,255,0,0
          
DELTA_LEN
          !byte 22,39,24,41
  
