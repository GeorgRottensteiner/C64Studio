!if 0 {
* = $0801
!basic 0,"",init_code

init_code:
          ;inc $d020
          rts
}
          
 
        
* = $0801
!basic 0," MEGASTYLE ",init_code


init_code:
          inc $d020
          jmp init_code
          