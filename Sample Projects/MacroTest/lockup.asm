; ============================================================================
; Demo of C64Studio's bogus-state/lockup issue from 2018-09-04.
; The single line between the stars (near line 25) should be commented
; in/out to cause/prevent the lockup. (Combined with a C64Studio restart.)
; ============================================================================

; The chunk of code below (regarding DEFINED_SOMETHING) was originally
; in a separate include file (using "!source"), but was reworked and
; simplified as the section below that can still reproduce the issue.
; ====================== [first line from include file] ======================
!ifndef DEFINED_SOMETHING {
DEFINED_SOMETHING = 1
; ----------------------------------------------------------------------------

!macro MainMacro .someValue {

;; BUG: If the next "!if" line is uncommented, doing a Build will cause
;; C64Studio to stop working (i.e., it cannot build anything thereafter,
;; even a restart of C64Studio won'f fix the lockup).
;; Instead of C64Studio borking like this, the user probably expects to
;; rather see an error, as is often the case in situations similar to
;; this, such as maybe E1008 (Macro function XXX, started in line YYY,
;; is missing end statement) and/or E1005 (Missing closing bracket).
;; ******************************
  !if (.someValue = 0) {  ; Comment this line in/out, and restart & rebuild
;; ******************************
;    bit 150
;  } else {
    bit 200
;  }
}

;; Compiler Messages also change when adding/removing the next macro's 5 lines.
!macro TrailingMacro .someValue, .moreValue {
  bit 250
  +MainMacro .someValue
  bit 251
}

; ----------------------------------------------------------------------------
}  ; DEFINED_SOMETHING
; ====================== [last line from include file] =======================

  *=$0801
  !basic

  +MainMacro 42

  rts

; ============================================================================