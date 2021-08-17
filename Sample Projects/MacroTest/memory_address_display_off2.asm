*=$0801

!for j = 0 to 1
  !byte 0
  !for i = 0 to 10
    ;!byte 0
  !end
  ;!align 255,0
!end
;!byte 0,0,0,0
;!align 255,0

i2 = 5
;!for i2 = 3 to 0 step -1
  !for j2 = 0 to 1 ;39 step 1
    !byte (i2 * 40) + j2
  !end
;!end