; ============================================================================
; Demo regression behaviour noticed since C64Studio 5.8 build of 2018-09-16,
; even though this file assembles perfectly with the WIP build of 2018-09-15.
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

!macro Fill1000v2 .startAddress, .byteConstant {
  lda #.byteConstant
  +Fill1000 .startAddress
}

SCREEN_RAM = $0400  ; 1024
SCREENCODE = 46

  *=$0801
  !basic

  ldy #0
forever:
  tya
  +Fill1000 SCREEN_RAM
  +Fill1000v2 SCREEN_RAM,32
  +Fill1000v2 SCREEN_RAM, SCREENCODE
  iny
  jmp forever

; ============================================================================
