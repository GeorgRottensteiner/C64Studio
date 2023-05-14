.x = "Y"

!if .x = "Y" {
  !message ".x is a string: ", .x
} else if .x > -1 {
  !message ".x is a number: ", .x
}

!macro test .x {
  !if .x = "Y" {
    !message ".x is a string: ", .x
  } else if .x > -1 {
    !message ".x is a number: ", .x
  }
}


+test "Y"