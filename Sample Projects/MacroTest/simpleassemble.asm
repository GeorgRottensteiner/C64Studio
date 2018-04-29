* = $ff00
jmp $12345

* = $ffff

  lda #$ff
  
  
  Zeropage_Routine = $0020

jmp Zeropage_Routine

!byte 50+100

Zeropage_Routine2