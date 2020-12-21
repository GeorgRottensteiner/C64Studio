*=$0326
  !word init
  !word $f6e6

*=$0400
  !scr "hello world                          "
  
init:
  inc $d020
  jmp *