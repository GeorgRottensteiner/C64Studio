!cpu m65

*=$2001
    !basic
;  !byte $09,$20           ; End of command
;  !byte $0a,$00           ; Line 10
;  !byte $fe,$02,$30,$00   ; Bank 0 command
;  !byte <end, >end        ; end of command
;  !byte $14,$00           ; Line 20
;  !byte $9e               ; SYS
;  !text "8214"            ; Start Address
;  !byte $00               ; end
;end:
;  !byte $00,$00           ; End of basic
;
;*=$2016
;;entry:
  sei
  
  ; Set memory
  lda #$35
  sta $01

  ; Enable 40Mhz
  lda #$41
  sta $00

  ; Enable VIC4 registers
  lda #$00
  tax
  tay
  taz
  map
  eom
  
  ; Turn off CiA
  lda #$7f
  sta $dc0d
  sta $dd0d
  
  ; Disable D65 rom write protection
  lda #$70
  sta $d640
  eom
  
  
  ; Turn off raster interrupts
  lda #$00
  sta $d01a

  cli
  
  ldz #$00
  stz $d020
  stz $d021
  jmp *