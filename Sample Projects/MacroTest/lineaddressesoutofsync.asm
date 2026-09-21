*=$0801
!basic 10, START

!macro Dummy {
  nop
  nop

  Label:
  nop
  nop

  @Label:
  nop
  nop

  .Label:
  nop
  nop

+
  nop
  nop

+ nop
  nop
}

START:
  +Dummy

  nop
  nop

  Label2:
  nop
  nop

  @Label2:
  nop
  nop

  .Label2:
  nop
  nop

+
  nop
  nop

+ nop
  nop

  rts
