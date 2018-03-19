!to "pobyte.prg",cbm

HAVE_ERRORS

* = $2000

          
          ;ok
          !byte 1
          
          ;ok
          !byte 2,3,4,5,6,7
          
          ;ok
          !byte %...##...
          
          !ifdef HAVE_ERRORS {
          ;not ok
          !byte -1
          
          ;not ok
          !byte -1,30+300          
          
          ;not ok
          !byte %#........
          
          ;nok
          !byte 0,0,0,
          
          ;nok
          !word 0,0,0,
          }          
           
          ;ok
          !word 1
          
          ;ok
          !word 2,3,4,5,6,7
          
          !ifdef HAVE_ERRORS {
          ;not ok
          !word -1
          
          ;not ok
          !word -1,30+500,20000+51000
          }