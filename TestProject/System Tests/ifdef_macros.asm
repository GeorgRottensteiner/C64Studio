!to "test2.prg",cbm

!IFDEF TEST {
  !MACRO d020_mach {
    inc $d020
  }
} else {
  !MACRO d020_mach {
    dec $d020
  }
}
  
*=$0801
!basic loop
loop:
  +d020_mach
  jmp loop