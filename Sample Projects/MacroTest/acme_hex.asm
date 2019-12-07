* = $0801

!basic

          lda #0
          sta $d020
          rts
          
!h f0 f1 f2 f3 f4 f5 f6 f7  ; insert values 0xf0..0xf7
    !h f0f1f2f3 f4f5f6f7    ; insert values 0xf0..0xf7
    !hex f0f1f2f3f4f5f6f7   ; insert values 0xf0..0xf7
    ;!h f0f 1f2  ; ERROR: space inside pair!
    ;!h 0x00, $00  ; ERROR: "0x", "," and "$" are forbidden!
    ;!h SOME_SYMBOL  ; ERROR: symbols are forbidden!
    !h ABCD ; insert value 0xAB, then 0xCD (CAUTION, big-endian)
