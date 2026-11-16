!macro callme otherlabel {
  !ifdef otherlabel {
    !message "otherlabel was defined"
} else {
  !message "otherlabel was not defined"
  }
}

* = $0801
!basic
          +callme END_OF_FILE


          rts

END_OF_FILE

          +callme END_OF_FILE
          rts