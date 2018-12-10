; ============================================================================
; Demo unexpected behaviour seen on the C64Studio 5.8 WIP build of 2018-12-04.
; This produces several E1010 (Could not evaluate !8) messages when assembled.
; The lines with !8 are problematic, but similar lines with !5 or !9 are OK.
; ============================================================================

*=$0801
!basic *, ":", $8F, " 20181205", DemoIssue
DemoIssue:

!macro LoadValueA .theValue {
  lda #.theValue
}

!macro LoadValueB .notValue {
  lda #!.notValue
}

!macro LoadValueC .whatValue {
  +LoadValueA (-1 xor .whatValue)
  +LoadValueB (!.whatValue)
}

; ----------------------------------------------------------------------------

  ldy #0 : sty $d020      ; Black

  ldx #1                  ; OK
  cpx #%00000001
  bne .exitBad            ; OK (continue)
  +LoadValueA 1           ; OK
  cmp #%00000001
  bne .exitBad            ; OK (continue)

  ldx #!2                 ; OK
  cpx #%11111101
  bne .exitBad            ; OK (continue)
  +LoadValueA !2          ; OK
  cmp #%11111101
  bne .exitBad            ; OK (continue)

  ldx #(-1 xor 3)         ; OK
  cpx #%11111100
  bne .exitBad            ; OK (continue)
  +LoadValueA (-1 xor 3)  ; OK
  cmp #!3                 ; OK
  bne .exitBad            ; OK (continue)

  ldx #4                  ; OK
  cpx #%00000100
  bne .exitBad            ; OK (continue)
  +LoadValueB 4           ; OK
  cmp #%11111011
  bne .exitBad            ; OK (continue)

  ldx #!5                 ; OK
  cpx #%11111010
  bne .exitBad            ; OK (continue)
  +LoadValueB !5          ; OK
  cmp #%00000101
  bne .exitBad            ; OK (continue)

  ldx #(-1 xor 6)         ; OK
  cpx #%11111001
  bne .exitBad            ; OK (continue)
  +LoadValueB (-1 xor 6)  ; OK
  cmp #6                  ; OK
  bne .exitBad

  ldx #7                  ; OK
  cpx #%00000111
  bne .exitBad            ; OK (continue)
  +LoadValueC 7           ; OK
  cmp #%00000111
  bne .exitBad            ; OK (continue)

  ldx #!5                 ; OK (just to check again -- above and here OK)
  cpx #%11111010
  bne .exitBad            ; OK (continue)
  ldx #!8                 ; ***** ERROR ***** (E1010 - "Could not evaluate !8")  -- WHAT!?
  cpx #%11110111
  bne .exitBad            ; OK (continue)
  +LoadValueC !5          ; OK
  cmp #%11111010
  bne .exitBad            ; OK (continue)
  +LoadValueC !9          ; OK (weird that !9 is OK but !8 is NOT)
  cmp #%11110110
  bne .exitBad            ; OK (continue)
  +LoadValueC !8          ; ***** ERROR ***** (E1010 - "Could not evaluate !8")  -- WHAT!? (and this line causes 2 extra errors)
  cmp #%11110111
  bne .exitBad            ; OK (continue)


  ldy #5 : sty $d020      ; Green
  rts

.exitBad:
  ldy #2 : sty $d020      ; Red
  rts

; ============================================================================
