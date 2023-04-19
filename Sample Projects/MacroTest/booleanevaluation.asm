* = $c000

true = 1=1

!if true {
  !message "true is true"
  } else {
  !message "true is false"
  }

!if !true {
  !message "true is also false"
  } else {
  !message "true is also true"
  }


!message "true is ",true