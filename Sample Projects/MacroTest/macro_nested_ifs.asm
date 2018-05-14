* = $2000

REGISTER = 1
A_REG = 2
Y_REG = 3
X_REG = 4

!macro IF type1, value1, condition, type2, value2
   .error=1
   !if type1=REGISTER & type2=REGISTER {
      !if value1=A_REG & value2=X_REG {
         .error=0
         stx .tmp+1 
         .tmp:
         cmp #$ff
      }  

      !if value1=A_REG & value2=Y_REG {
         .error=0 ; <========================= HIER 
      }


   }

   !if .error=1 {
      !error "Wrong Parameters in 'IF'" 
   }

!end
  
  
          +IF REGISTER, A_REG, 17, REGISTER, Y_REG
          rts