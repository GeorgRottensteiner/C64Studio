; ============================================================================
; Demo unexpected behaviour noticed with C64Studio 5.8 build of 2018-09-08.
; Produces E1001 "Failed to evaluate -2147483648"
; and E1001 "Could not evaluate expression: (.theValue = 0)".
; ============================================================================

*=$0801
!basic

inc $d020

;.theValue = -2147483647  ; This works.  :)
.theValue = -2147483648  ; *** This FAILS. ***  (is a valid 32-bit integer!)
;.theValue = -65536  ; This works.  :)
;.theValue = -256  ; This works.  :)
;.theValue = 256  ; This works.  :)
;.theValue = 65535  ; This works.  :)
;.theValue = 65536  ; This works.  :)
;.theValue = -2147483647  ; This works.  :)

!if (.theValue = 0) {
  !error "ERROR"
} else {
  !message "Good!"
}

inc $d020

rts

; ============================================================================
