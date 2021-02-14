!cpu 65ce02


* = $1000
   ;       lda ($ff),z
;          
;          phw #$1234
;-          
;          lbra -
;          
;          jmp ($1234)
;          
;          
;          jmp ($1234,x)
          
          LDA ($12,SP),Y
  
lsmf2  

!byte 1,2,3
!byte 4,5,6

huhu