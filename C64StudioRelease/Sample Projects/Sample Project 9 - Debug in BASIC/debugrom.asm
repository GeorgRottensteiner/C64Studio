;Sample Project 9 from C64Studio
;Author Georg Rottensteiner
;
;This sample shows that debug stepping through the kernal (and other un-assembled code) works.
;Set a breakpoint or press Ctrl-F10 at line 17, debug step forward and the disassembly
;window appears.

!to "debugrom.prg",cbm

* = $0801
          ;SYS 2064
          !byte $0C,$08,$0A,$00,$9E,$20,$32,$30,$36,$34,$00,$00,$00,$00,$00

          lda #0
          sta 1024

          ;call clear screen
          jsr $E544

          ;and back to BASIC
          rts