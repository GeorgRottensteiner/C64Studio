; ============================================================================
; Demo regression behaviour noticed since C64Studio 5.8 build of 2018-09-16.
; This produces various E1001 "Could not evaluate LABEL" errors.
; ============================================================================

!macro Fill1000 .startAddress {
  ldx #250
.loop
  sta .startAddress-1+0,x
  sta .startAddress-1+250,x
  sta .startAddress-1+500,x
  sta .startAddress-1+750,x
  dex
  bne .loop
}

SCREEN_RAM  = $0400  ; 1024

  *=$0801
  !basic

  ldy #0
forever:
  tya
  +Fill1000 SCREEN_RAM
  iny
  jmp forever

; ============================================================================
