*=$0801
!basic

ifstackptr = 0
iflabelnumber = 0

!macro test_PUSHIFSTACK {
  iflabelnumber = iflabelnumber + 1
  ifstackptr = ifstackptr + 1
  ifstack##ifstackptr = iflabelnumber
}

!macro test_IF {

  +test_PUSHIFSTACK


  ; hier ist das PROBLEM !!!!!!!!!!!!!!
  .num = ifstack##ifstackptr
  bne iflabel##.num

}
!macro test_ENDIF {
  num = ifstack##ifstackptr
  iflabel##num:
}

+test_IF
  nop
  nop
+test_ENDIF

rts