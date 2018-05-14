
*=$c000

STACK_LEVEL0 = 0
GLOBAL_VAR = 3

!macro PUSH value
   ; some fancy code to move stack values down
   STACK_LEVEL0=value
!end

!macro SOME_MACRO_THAT_USES_PUSH
   +PUSH 7
!end

+SOME_MACRO_THAT_USES_PUSH