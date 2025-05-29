*= $5800
!media   "test2.graphicscreen",color
        *= $5c00
!media   "test2.graphicscreen",screen
        *= $6000
!media   "test2.graphicscreen",bitmap

         *= $0801
         !basic

         jmp start

         *= $2800

start
         lda #$00
loop1    lda $5800,x
         sta $d800,x
         lda $5900,x
         sta $d900,x
         lda $5a00,x
         sta $da00,x
         lda $5b00,x
         sta $db00,x
         inx
         bne loop1

         lda #$00
         sta $d020
         sta $d021

         lda #$3b
         sta $d011
         lda #%01111000
         sta $d018
         lda #%11111110
         sta $dd00
         lda #%00011000
         sta $d016

         jmp *