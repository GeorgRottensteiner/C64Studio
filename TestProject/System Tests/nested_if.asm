FACTOR_A = 5
FACTOR_B = 6

* = $2000

        !byte 17

  !if FACTOR_A = FACTOR_B {
      !if FACTOR_B = 6 {
          !byte 1
        }
    }


  
!if FACTOR_A != FACTOR_B {
!if FACTOR_B = 6 {
  !byte 2
}
}
      
      !byte 18
      
      
