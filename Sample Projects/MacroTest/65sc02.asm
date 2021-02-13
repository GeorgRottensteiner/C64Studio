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