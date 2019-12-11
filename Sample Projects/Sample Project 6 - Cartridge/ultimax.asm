!to "ultimax.crt",ultimax8crt

* = $e000        
NMI
          pla
          tay
          pla
          tax
          pla
          rti
        
RESET
          lda #2
          lda #3
-
          inc $d020
          jmp -
        
          
          ;Ultimax mode has no Kernal, so we need to provide entry pointers ourself!
* = $fffc
          !word RESET
          !word NMI