; ============================================================================
; Demo unexpected behaviour seen on the C64Studio 5.8 WIP build of 2018-11-18.
; To investigate, uncomment the 8 lines below one at a time, following that
; line's instruction for its Project|Properties|Configuration|Predefines
; setting; then rebuild, run and check the results for each test-case.
; ============================================================================

;         ; Test-case A.1 (undefined here, and also nothing in project Predefines)
;TEST=0   ; Test-case B.1 (uncomment this line to specify value here, but nothing in project Predefines)
;TEST=1   ; Test-case C.1 (uncomment this line to specify value here, but nothing in project Predefines)
;TEST=-1  ; Test-case D.1 (uncomment this line to specify value here, but nothing in project Predefines)
;         ; Test-case A.2 (undefined here and nothing in project Predefines)
;         ; Test-case B.2 : In project Predefines specify TEST=0
;         ; Test-case C.2 : In project Predefines specify TEST=1
;         ; Test-case D.2 : In project Predefines specify TEST=-1

; ----------------------------------------------------------------------------

*=$0801
!basic

lda #2  ; Red

!ifndef TEST {
  !message "TEST is undefined"
  lda #11  ; Dark Gray
} else {
  !message "TEST is defined as ", TEST
  !if TEST {
    !message "The TEST value is considered TRUE"
    lda #5  ; Green
  } else {
    !warn "However, TEST is considered FALSE"
    lda #4  ; Purple
  }
}

sta $d020
rts

; ============================================================================
