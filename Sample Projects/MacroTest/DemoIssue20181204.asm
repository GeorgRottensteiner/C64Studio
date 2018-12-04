; ============================================================================
; Demo unexpected behaviour seen on the C64Studio 5.8 WIP build of 2018-11-30.
; This produces E1000 (Syntax error) messages when assembled.
; ============================================================================

*=$0801
!basic *, ":", $8F, " 20181204", DemoIssue
DemoIssue:
  dec $d020

  RASTER = $D012

!zone TestA1 {  ; This works
-:dex
  bne -
}

!zone TestA2 {  ; This works
- dex
  bne -
}

!zone TestB1 {  ; This works
.loop:
  bne +
  inx
  jmp .loop
+
}

!zone TestB2 {  ; This works
.loop
  beq +
  inx
  jmp .loop
+
}

!zone TestC1 {  ; This works
-
  lda $D012
  cmp $D012
  bne -
}

!zone TestC2 {  ; This works
-:
  lda $D012
  cmp $D012
  bne -
}

!zone TestC3 {  ; This works
-:lda $D012
  cmp $D012
  bne -
}

!zone TestC4 {  ; This works
- lda $D012
  cmp $D012
  bne -
}

!macro TestC5 addressLabel {  ; This works
-:lda addressLabel
  cmp addressLabel
  bne -
}
  +TestC5 $D012  ; This works
  +TestC5 RASTER  ; This works

!macro TestC6 .addressLabel {  ; This works
-:lda .addressLabel
  cmp .addressLabel
  bne -
}
  +TestC6 $D012  ; This works
  +TestC6 RASTER  ; This works

!macro TestC7 addressLabel {  ; *** This works ONLY SOMETIMES ***
- lda addressLabel
  cmp addressLabel
  bne -
}
  +TestC7 $D012  ; This works
  +TestC7 RASTER  ; *** FAILED! ***

!macro TestC8 .addressLabel {  ; *** This works ONLY SOMETIMES ***
- lda .addressLabel
  cmp .addressLabel
  bne -
}
  +TestC8 $D012  ; This works
  +TestC8 RASTER  ; *** FAILED! ***

  inc $d020
  rts

; ============================================================================
