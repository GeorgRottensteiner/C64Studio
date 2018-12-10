; ============================================================================
; Demo unexpected behaviour seen on the C64Studio 5.8 WIP build of 2018-12-04.
; When assembled, This produces an E1001 (Could not evaluate expression).
; ============================================================================

!macro MacroA1 .theParam1 {
  !if ((.theParam1 < 1) or (.theParam1 > 0)) {
    bvc *+4
    !byte $11, .theParam1
  }
}

!macro MacroB2 .theParam1, .theParam2 {
  !if ((.theParam2 < 1) or (.theParam2 > 0)) {
    bvc *+5
    !byte $22, .theParam1, .theParam2
  }
}

!macro MacroC3 .theParam1, .theParam2, .theParam3 {
  !if ((.theParam3 < 1) or (.theParam3 > 0)) {
    bvc *+6
    !byte $33, .theParam1, .theParam2, .theParam3
  }
}

!macro MacroD4 .theParam1, .theParam2, .theParam3, .theParam4 {
  !if ((.theParam4 < 1) or (.theParam4 > 0)) {
    bvc *+7
    !byte $44, .theParam1, .theParam2, .theParam3, .theParam4
  }
}

!macro MacroD3 .theParam1, .theParam2, .theParam3, .theParam4 {
  !if ((.theParam3 < 1) or (.theParam3 > 0)) {
    bvc *+7
    !byte $43, .theParam1, .theParam2, .theParam3, .theParam4
  }
}

; ----------------------------------------------------------------------------
*=$0801
!basic *, ":", $8F, " 20181208", DemoIssue
DemoIssue:

  GlobalConst = 11

  ldy #7 : sty $d020             ; Yellow
  clv

  .LocalConstX = 22
  +MacroA1 GlobalConst            ; OK
  +MacroA1 .LocalConstX           ; OK
  +MacroB2 1, GlobalConst         ; OK
  +MacroB2 1, .LocalConstX        ; OK
  +MacroC3 1, 2, GlobalConst      ; OK
  +MacroC3 1, 2, .LocalConstX     ; OK
  +MacroD4 1, 2, 3, GlobalConst   ; OK
  +MacroD4 1, 2, 3, .LocalConstX  ; OK
  +MacroD3 1, 2, 3, GlobalConst   ; OK
  +MacroD3 1, 2, 3, .LocalConstX  ; OK
  +MacroD4 1, 2, GlobalConst, 4   ; OK
  +MacroD4 1, 2, .LocalConstX, 4  ; OK
  +MacroD3 1, 2, GlobalConst, 4   ; OK
  +MacroD3 1, 2, .LocalConstX, 4  ; OK

!zone {
  .LocalConstY = 33
  +MacroA1 GlobalConst            ; OK
  +MacroA1 .LocalConstY           ; OK
  +MacroB2 1, GlobalConst         ; OK
  +MacroB2 1, .LocalConstY        ; OK
  +MacroC3 1, 2, GlobalConst      ; OK
  +MacroC3 1, 2, .LocalConstY     ; OK
  +MacroD4 1, 2, 3, GlobalConst   ; OK
  +MacroD4 1, 2, 3, .LocalConstY  ; OK
  +MacroD3 1, 2, 3, GlobalConst   ; OK
  +MacroD3 1, 2, 3, .LocalConstY  ; OK
  +MacroD4 1, 2, GlobalConst, 4   ; OK
  +MacroD4 1, 2, .LocalConstY, 4  ; OK
  +MacroD3 1, 2, GlobalConst, 4   ; OK
  +MacroD3 1, 2, .LocalConstY, 4  ; *** FAIL ***
}  ; !zone

  ldy #5 : sty $d020             ; Green
  rts

; ============================================================================
