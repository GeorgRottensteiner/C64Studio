*=$0801
!basic

ifstack = 17

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

          bne iflabel##ifstack##ifstackptr

}
!macro test_ENDIF {
  num = ifstack##ifstackptr
  iflabel##num

xxlabel##ifstack##ifstackptr
}



+test_IF
  nop
  nop
+test_ENDIF

rts