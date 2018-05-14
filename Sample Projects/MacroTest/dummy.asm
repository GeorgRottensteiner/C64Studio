* = $2000
                           CHARACTER_ROM   = 1024
                           CUSTOM_CHAR_SET = $4000
                           !macro copy_character_range_from_rom start, end 
                           ;removed some code for simplicity 
                           ; Copy char rom to ram 
                           !for char = start to end 
                           ldx #0 
                           - 
                           lda CHARACTER_ROM + char*8, x 
                           sta CUSTOM_CHAR_SET + char*8, x 
                           inx 
                           cpx #8 
                           bne - 
                           !end 
                           !end
                           +copy_character_range_from_rom 0, 2
                           rts
                           
                            

*=$c000

STACK_LEVEL0 = 0

GLOBAL_VAR = 3


!macro PUSH value

   ; some fancy code to move stack values down

   STACK_LEVEL0=value

!end


!macro SOME_MACRO_THAT_USES_PUSH

   +PUSH GLOBAL_VAR

!end


+SOME_MACRO_THAT_USES_PUSH                           