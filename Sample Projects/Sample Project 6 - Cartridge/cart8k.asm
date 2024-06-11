!to "cart8k.crt",CART8CRT

*=$8000                     ;cart start address
          !word Reset          ;RESET vector
          !word Reset          ;NMI vector
          !byte $c3, $c2, $cd  ;CBM <- cartridge ID
          !text "80"           ;80

Reset
          sei
          stx $d016          ;init VIC
          jsr $fda3          ;init CIA
          jsr $fd50          ;RAM test/clear
          jsr $fd15          ;init kernal vectors
          jsr $ff5b          ;init VIC
          cli                ;re-enable interrupts

          ldy #0
-
          lda TEXT,y
          sta $0400,y
          iny
          cpy #11
          bne -

          jmp *

TEXT
          !scr "hello world"