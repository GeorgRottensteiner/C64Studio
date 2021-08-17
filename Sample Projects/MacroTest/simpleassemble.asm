* = $ff00
jmp $1234 ;5

* = $ffff

  lda #$ff


  Zeropage_Routine = $0020

jmp Zeropage_Routine

!byte 50+100

Zeropage_Routine2

