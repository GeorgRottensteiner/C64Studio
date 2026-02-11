wic_crt = 1

* = $2000

!if wic_crt = 0 {
   jsr load_music
} else {
          }


ICH_BIN_EIN_LABEL

TX
!SCR " THIS IS MY NEW 8X8 SCROLLER. AWESOME! "
          !BYTE 0 ; 0 means end of scroll reacvhed

!SCR " this is my new 8x8 scroller. awesome! "