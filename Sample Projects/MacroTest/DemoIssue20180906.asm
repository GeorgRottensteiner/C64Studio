; ============================================================================
; Demo of C64Studio 5.8 messages for !addr/!address issue of 2018-09-06.
; Produces E1004 "Missing opening brace" and W1000 "Unused label }".
; ============================================================================

!source "DemoIssue20180906_Include.asm"
!source "DemoIssue20180906_Include.asm"

; ----------------------------------------------------------------------------

  *=$0801
  !basic

  nop
!ifdef sid_v3_control {
  !message "Address label defined okay."
  lda #$80
  sta $d418
  lda #$80
  sta sid_v3_control
  lda $d41b
} else {
  !warn "Oops, address label NOT defined. Where did it go? It is needed!"
  brk
}

  rts

; ============================================================================
