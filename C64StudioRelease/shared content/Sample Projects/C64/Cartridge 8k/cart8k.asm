!to "cart8k.crt",cart8crt

!src <c64.asm>

* = $8000
          ;RESET vector (cold start)
          !word ColdStart
          ;NMI vector   (warm start, RESTORE)
          !word WarmStart
          ;magic number (must be "CBM80")
          !pet "CBM80"

!lzone ColdStart
          ;KERNAL RESET ROUTINE
          sei
          stx VIC.CONTROL_2   ;activate VIC
          jsr $fda3           ;init CIA
          jsr $fd50           ;check RAM
          jsr $fd15           ;init kernal vectors
          jsr $ff5b           ;init VIC
          cli                 ;enable interrupts

          ;... fall through


!lzone WarmStart
          jsr DisplayText

.InfiniteLoop
          inc VIC.BORDER_COLOR
          jmp .InfiniteLoop



!lzone DisplayText
          ldx #0

.loop
          lda text, x   ;Load next char value
          cmp #$FF      ;Have we reached the end of the string
          beq .Done     ;If null, jump to .Done
          sta $0400, x  ;Write char to screen
          inx
          jmp .loop

.Done
          rts

text
          !scr "rom bank 0"
          !byte $FF



