*=$0801
!macro test{
  -
  bne - ;Das klappt nicht, der Assembler
        ;ist von einem Kommentar hinter 
        ;einem lokalen Label in einem
        ;Makro überfordert ...
}
+test