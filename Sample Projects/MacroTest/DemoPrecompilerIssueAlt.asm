*=$0801
!basic ContrivedTest

!macro TestCase .someNumber {

  ;; Right below is how the code could look (logically less complex) but now all cases compile as expected (!?)
  !if (.someNumber < 0) {
    !message "Case X for ", .someNumber
  } else if (.someNumber >= 7) {
    !message "Case Y for ", .someNumber
  } else if (.someNumber >= 4) {
    !message "Case Z for ", .someNumber
  } else {
    !message "Unknown case for ", .someNumber
  }

  ;; Right below is how the code had to be rewritten (logically more complex) for it to previously compile
  ;!if (.someNumber < 0) {
  ;  !message "Case X for ", .someNumber
  ;}
  ;!if (.someNumber >= 7) {
  ;  !message "Case Y for ", .someNumber
  ;}
  ;!if ((.someNumber < 7) and (.someNumber >= 4)) {
  ;  !message "Case Z for ", .someNumber
  ;}
  ;!if ((.someNumber < 4) and (.someNumber >= 0)) {
  ;  !message "Unknown case for ", .someNumber
  ;}

  ; Just for some fun... (not really affecting the test)
  lda #(.someNumber & 15)
  sta $d020  ; Set the border color
  ora #64    ; Turn into a printable character
  jsr $FFD2  ; Outputs character for number (CHROUT)
}

ContrivedTest: 
  +TestCase -1
  +TestCase 0
  +TestCase 3
  +TestCase 4
  +TestCase 6
  +TestCase 7
  rts
