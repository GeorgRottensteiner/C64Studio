; ============================================================================
; Demo the error that was found on 2018-08-30 with the new special-buid
; version of C64Studio (the one built on 2018-08-25/26). This code compiles
; perfectly on the previous official version (v.5.7b of 2018-05-25).
; (If the code compiles it does an endless loop, but that is not the point.)
; ============================================================================

!macro RedactedMacro .someValue {
  !if (.someValue < 0) {
    !warn "Weird value (", .someValue, ")"
  }
  !if (.someValue >= 400) {
    !warn "Too large value (", .someValue, ")"
  }
  !if ((.someValue >= 300) and (.someValue < 400)) {
    !warn "Unwanted value ", .someValue, "!"
  }
  !if ((.someValue >= 275) and (.someValue < 300)) {
    !warn "Stupid value ", .someValue, "!"
  }

.doStuff:
  cpy #(.someValue & $FF)
  bne .doStuff

  cpx #$80
  !if (.someValue >= 256) {
    bcc .doStuff
  } else {
    bcs .doStuff
  }
}

; ============================================================================

  *=$0801
  !basic  ; This makes the code below a standalone RUN-able program from BASIC

  +RedactedMacro 138

  nop
  ;brk

  rts

; ============================================================================
