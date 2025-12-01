!to "magicdesk.crt" , magicdeskcrt

!src <c64.asm>

!macro HandleBank bank
!bank bank,$2000
!zone BANK##bank

!pseudopc $8000
          !word launcher
          !word launcher

          !pet "CBM80"

launcher
          sei
          stx VIC.CONTROL_2
          jsr $fda3 ;prepare irq
          jsr $fd50 ;init memory
          jsr $fd15 ;init i/o
          jsr $ff5b ;init video
          cli

-
          lda #'B' - 64
          sta $0400
          lda #'A' - 64
          sta $0401
          lda #'N' - 64
          sta $0402
          lda #'K' - 64
          sta $0403

          lda #bank
          clc
          adc #$30
          sta $0405

          lda #$01
          bit $dc00
          bne .NotUpPressed

.WaitForRelease
          lda #$01
          bit $dc00
          beq .WaitForRelease

          lda #( bank + 1 ) % 8
          sta $de00
.NotUpPressed

          jmp -
!end



* = $8000

          +HandleBank 0
          +HandleBank 1
          +HandleBank 2
          +HandleBank 3
          +HandleBank 4
          +HandleBank 5
          +HandleBank 6
          +HandleBank 7



;          ;bank 0
;* = $8000

;!bank 0,$2000
;          !word launcher
;          !word launcher

;          !pet "CBM80"

;launcher
;          sei
;          stx VIC.CONTROL_2
;          jsr $fda3 ;prepare irq
;          jsr $fd50 ;init memory
;          jsr $fd15 ;init i/o
;          jsr $ff5b ;init video
;          cli

;-
;          +DisplayScreen 0
;          lda #$01
;          bit $dc00
;          bne .NotUpPressed

;.WaitForRelease
;          lda #$01
;          bit $dc00
;          beq .WaitForRelease

;          lda #1
;          sta $de00
;.NotUpPressed

;          jmp -

;          ;bank 1
;!bank 1,$2000



;!zone BANK1

;!pseudopc $8000

;          !word .launcher
;          !word .launcher
;          !pet "CBM80"

;.launcher
;          sei
;          stx $d016
;          jsr $fda3 ;prepare irq
;          jsr $fd50 ;init memory
;          jsr $fd15 ;init i/o
;          jsr $ff5b ;init video
;          cli

;-
;          +DisplayScreen 1

;          lda #$01
;          bit $dc00
;          bne .NotUpPressed

;.WaitForRelease
;          lda #$01
;          bit $dc00
;          beq .WaitForRelease

;          lda #2
;          sta $de00
;.NotUpPressed

;          jmp -

;          ;bank 2

;!bank 2,$2000
;!zone BANK2
;!pseudopc $8000

;          !word .launcher
;          !word .launcher
;          !pet "CBM80"

;.launcher
;          sei
;          stx $d016
;          jsr $fda3 ;prepare irq
;          jsr $fd50 ;init memory
;          jsr $fd15 ;init i/o
;          jsr $ff5b ;init video
;          cli

;-
;          +DisplayScreen 2

;          lda #$01
;          bit $dc00
;          bne .NotUpPressed

;.WaitForRelease
;          lda #$01
;          bit $dc00
;          beq .WaitForRelease

;          lda #3
;          sta $de00
;.NotUpPressed

;          jmp -


;          ;bank 3
;!bank 3,$2000
;!zone BANK3
;!pseudopc $8000

;          !word .launcher
;          !word .launcher
;          !pet "CBM80"

;.launcher
;          sei
;          stx $d016
;          jsr $fda3 ;prepare irq
;          jsr $fd50 ;init memory
;          jsr $fd15 ;init i/o
;          jsr $ff5b ;init video
;          cli

;-
;          +DisplayScreen 3

;          lda #$01
;          bit $dc00
;          bne .NotUpPressed

;.WaitForRelease
;          lda #$01
;          bit $dc00
;          beq .WaitForRelease

;          lda #4
;          sta $de00
;.NotUpPressed

;          jmp -


;          ;bank 4
;!bank 4,$2000
;!zone BANK4
;!pseudopc $8000

;          !word .launcher
;          !word .launcher
;          !pet "CBM80"

;.launcher
;          sei
;          stx $d016
;          jsr $fda3 ;prepare irq
;          jsr $fd50 ;init memory
;          jsr $fd15 ;init i/o
;          jsr $ff5b ;init video
;          cli

;-
;          +DisplayScreen 4

;          lda #$01
;          bit $dc00
;          bne .NotUpPressed

;.WaitForRelease
;          lda #$01
;          bit $dc00
;          beq .WaitForRelease

;          lda #5
;          sta $de00
;.NotUpPressed

;          jmp -


;          ;bank 5
;!bank 5,$2000
;!zone BANK5
;!pseudopc $8000

;          !word .launcher
;          !word .launcher
;          !pet "CBM80"

;.launcher
;          sei
;          stx $d016
;          jsr $fda3 ;prepare irq
;          jsr $fd50 ;init memory
;          jsr $fd15 ;init i/o
;          jsr $ff5b ;init video
;          cli

;-
;          +DisplayScreen 5

;          lda #$01
;          bit $dc00
;          bne .NotUpPressed

;.WaitForRelease
;          lda #$01
;          bit $dc00
;          beq .WaitForRelease

;          lda #6
;          sta $de00
;.NotUpPressed

;          jmp -




;          ;bank 6
;!bank 6,$2000
;!zone BANK6
;!pseudopc $8000

;          !word .launcher
;          !word .launcher
;          !pet "CBM80"

;.launcher
;          sei
;          stx $d016
;          jsr $fda3 ;prepare irq
;          jsr $fd50 ;init memory
;          jsr $fd15 ;init i/o
;          jsr $ff5b ;init video
;          cli

;-
;          +DisplayScreen 6

;          lda #$01
;          bit $dc00
;          bne .NotUpPressed

;.WaitForRelease
;          lda #$01
;          bit $dc00
;          beq .WaitForRelease

;          lda #7
;          sta $de00
;.NotUpPressed

;          jmp -


;          ;bank 7
;!bank 7,$2000
;!zone BANK7
;!pseudopc $8000

;          !word .launcher
;          !word .launcher
;          !pet "CBM80"

;.launcher
;          sei
;          stx $d016
;          jsr $fda3 ;prepare irq
;          jsr $fd50 ;init memory
;          jsr $fd15 ;init i/o
;          jsr $ff5b ;init video
;          cli

;-
;          +DisplayScreen 7

;          lda #$01
;          bit $dc00
;          bne .NotUpPressed

;.WaitForRelease
;          lda #$01
;          bit $dc00
;          beq .WaitForRelease

;          lda #0
;          sta $de00
;.NotUpPressed

;          jmp -

