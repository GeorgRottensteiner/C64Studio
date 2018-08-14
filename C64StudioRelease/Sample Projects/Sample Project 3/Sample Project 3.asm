!to"menu.prg",cbm

;----------------------------------------------------------
; Dynamic Lightbar for 6502-Assembler
;
; [/] 04.April 2011 M. Sachse http://www.cbmhardware.de
;----------------------------------------------------------

firstline    = $0a
lastline     = $0b
s_lo         = $fb
s_hi         = $fc
linelenght   = $fe
get          = $ffe4

*=$0801
!by $0c,$08,$0a,$00,$9e,$32,$30,$36,$34,$00,$00,$00,$00
*=$0810


; setup

               lda #$04                ; ofs first line
               sta s_hi
               lda #$f0
               sta s_lo
               lda #$28                ; lenght of the line
               sta linelenght
               lda #$00                ; first line (+ofs)
               sta firstline
               lda #$0d                ; last line (+ofs)
               sta lastline




; build inverted highlighting at first line
;

               ldy #$00
-              lda (s_lo),y
               ora #%10000000
               sta (s_lo),y
               iny
               cpy linelenght
               bne -



; move by keyboard

main:
              jsr get
              cmp #$20                     ; spc-key
              beq end
              cmp #$11                     ; cursor up
              beq move_up
              cmp #$91                     ; cursor down
              beq move_down
              jmp main
end:          rts



; move-routines



move_down:
              lda menuline
              cmp lastline
              bne +
              jmp main
+             inc menuline
              clc
              ldy #$00
-             lda (s_lo),y
              and #%01111111
              sta (s_lo),y
              iny
              bcc +
              inc s_hi
+             cpy linelenght
              bne -
              lda s_lo
              clc
              adc #$28
              sta s_lo
              bcc +
              inc s_hi
+             clc
              ldy #$00
-             lda (s_lo),y
              ora #%10000000
              sta (s_lo),y
              iny
              bcc +
              inc s_hi
+             cpy linelenght
              bne -
              jmp main



move_up:
              lda menuline
              cmp firstline
              bne +
              jmp main
+             dec menuline
              clc
              ldy #$00
-             lda (s_lo),y
              and #%01111111
              sta (s_lo),y
              iny
              bcc +
              inc s_hi
+             cpy linelenght
              bne -
              lda s_lo
              sec
              sbc #$28
              sta s_lo
              bcs +
              dec s_hi
+             clc
              ldy #$00
-             lda (s_lo),y
              ora #%10000000
              sta (s_lo),y
              iny
              bcc +
              inc s_hi
+             cpy linelenght
              bne -
              jmp main

menuline    !by $00
