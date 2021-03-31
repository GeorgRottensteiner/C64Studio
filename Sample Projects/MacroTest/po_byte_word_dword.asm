!to "po_byte_word_dword.prg",cbm

;HAVE_ERRORS

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
          !16 2,3,4,5,6,7
          
          ;ok
          !le16 2,3,4,5,6,7
          
          ;ok
          !be16 1
          
          ;ok
          !be16 2,3,4,5,6,7
          
          ;ok
          !32 2,3,4,5,6,7
          
          ;ok
          !le32 2,3,4,5,6,7
          
          ;ok
          !be32 1
          
          !ifdef HAVE_ERRORS {
          ;not ok
          !word -1
          
          ;not ok
          !word -1,30+500,20000+51000
          }